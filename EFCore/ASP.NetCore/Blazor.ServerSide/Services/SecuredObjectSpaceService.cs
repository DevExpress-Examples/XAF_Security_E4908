using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Blazor.ServerSide.Services {
    public class SecuredObjectSpaceService : IDisposable {
        readonly IHttpContextAccessor httpContextAccessor;
        readonly Type dbContextType;
        readonly EFCoreDatabaseProviderHandler databaseProviderHandler;
        readonly IList<IDisposable> objectsToDispose = new List<IDisposable>();
        public SecuredObjectSpaceService(IHttpContextAccessor httpContextAccessor, Type dbContextType, EFCoreDatabaseProviderHandler databaseProviderHandler) {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContextType = dbContextType;
            this.databaseProviderHandler = databaseProviderHandler;
            Initialize();
        }
        SecurityStrategy SecurityStrategy { get; set; }
        SecuredEFCoreObjectSpaceProvider ObjectSpaceProvider { get; set; }
        void Initialize() {
            SecurityStrategy = GetSecurityStrategy();
            ObjectSpaceProvider = GetObjectSpaceProvider(SecurityStrategy, dbContextType, databaseProviderHandler);
        }
        static SecurityStrategyComplex GetSecurityStrategy() {
            var authentication = new AuthenticationMixed();
            authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
            authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
            authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
            return new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
        }
        static SecuredEFCoreObjectSpaceProvider GetObjectSpaceProvider(ISelectDataSecurityProvider security, Type dbContextType, EFCoreDatabaseProviderHandler databaseProviderHandler) {
            return new SecuredEFCoreObjectSpaceProvider(security, dbContextType, databaseProviderHandler);
        }
        bool LogonCore() {
            IObjectSpace objectSpace = ObjectSpaceProvider.CreateNonsecuredObjectSpace();
            try {
                SecurityStrategy.Logon(objectSpace);
                return true;
            } catch (AuthenticationException) {
                return false;
            }
        }
        public bool LogonWithUserName(string userName, string password) {
            AuthenticationStandardLogonParameters logonParameters = new AuthenticationStandardLogonParameters(userName, password);
            ((AuthenticationMixed)SecurityStrategy.Authentication).SetupAuthenticationProvider(nameof(AuthenticationStandardProvider), logonParameters);
            return LogonCore();
        }
        public bool LogonWithIdentity() {
            ((AuthenticationMixed)SecurityStrategy.Authentication).SetupAuthenticationProvider(nameof(IdentityAuthenticationProvider), httpContextAccessor.HttpContext.User.Identity);
            return LogonCore();
        }
        public async Task SignInAsync(string userName) {
            var claims = new List<Claim>() {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var principal = new ClaimsPrincipal(id);
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal)
                .ConfigureAwait(false);
        }
        public bool CanWrite(object targetObject, string memberName) {
            return SecurityStrategy.CanWrite(targetObject, memberName);
        }
        public bool CanWrite(object targetObject) {
            return SecurityStrategy.CanWrite(targetObject);
        }
        public bool CanRead(object targetObject, string propertyName) {
            return SecurityStrategy.CanRead(targetObject, propertyName);
        }
        public bool CanDelete(object targetObject) {
            return SecurityStrategy.CanDelete(targetObject);
        }
        public bool CanCreate<T>() where T:class {
            return SecurityStrategy.CanCreate<T>();
        }
        public IQueryable<T> GetObjectsQuery<T>() where T:class {
            IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace();
            objectsToDispose.Add(os);
            return os.GetObjectsQuery<T>();
        }
        public async Task<T[]> GetObjectsAsync<T>(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default) where T:class {
            using(IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace()) {
                var q = os.GetObjectsQuery<T>();
                if(predicate != null) {
                    q = q.Where(predicate);
                }
                return await q.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        public void SaveChanges<T>(T obj) where T:class {
            using(IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace()) {
                T attachedObj = os.GetObject(obj);
                if(attachedObj == null) {
                    attachedObj = os.CreateObject<T>();
                }
                if(attachedObj != obj) {
                    ITypeInfo typeInfo = os.TypesInfo.FindTypeInfo(typeof(T));
                    foreach(IMemberInfo memberInfo in typeInfo.Members) {
                        if(memberInfo.IsPersistent && !memberInfo.IsKey) {
                            object oldValue = memberInfo.GetValue(attachedObj);
                            object newValue = memberInfo.GetValue(obj);
                            if(oldValue != newValue) {
                                memberInfo.SetValue(attachedObj, newValue);
                            }
                        }
                    }
                }
                os.CommitChanges();
            }
        }
        public void Delete(object obj) {
            using(IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace()) {
                os.Delete(obj);
                os.CommitChanges();
            }
        }
        public void Dispose() {
            foreach(IDisposable disposable in objectsToDispose) {
                disposable.Dispose();
            }
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection {
    using Blazor.ServerSide.Services;
    using Microsoft.EntityFrameworkCore;
    public static class SecuredObjectSpaceServiceExtensions {
        public static IServiceCollection AddSecuredObjectSpace<TContext>(this IServiceCollection services, EFCoreDatabaseProviderHandler databaseProviderHandler) where TContext : DbContext {
            return services.AddScoped(sp => CreateService(sp, typeof(TContext), databaseProviderHandler));
        }
        static SecuredObjectSpaceService CreateService(IServiceProvider serviceProvider, Type dbContextType, EFCoreDatabaseProviderHandler databaseProviderHandler) {
            IHttpContextAccessor httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            var objectSpaceService = new SecuredObjectSpaceService(httpContextAccessor, dbContextType, databaseProviderHandler);
            if(httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                objectSpaceService.LogonWithIdentity();
            }
            return objectSpaceService;
        }
    }
}
