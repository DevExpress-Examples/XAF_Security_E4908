using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Security.Internal;
using DevExpress.ExpressApp.Security.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace Blazor.ServerSide.Helpers {
    public class SecurityProvider : IDisposable {
        public SecurityStrategyComplex Security { get; private set; }
        public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
        XpoDataStoreProviderService xpoDataStoreProviderService;
        readonly ISecurityUserProvider securityUserProvider;
        private readonly IUserClaimsProvider userClaimsProvider;
        readonly AuthenticationStateProvider authenticationStateProvider;
        readonly IHttpContextAccessor httpContextAccessor;

        //TODO BORISOV: ISecurityUserProvider - подумать над этим интерфейсом 
        public SecurityProvider(IHttpContextAccessor httpContextAccessor,  ISecurityStrategyBase security, XpoDataStoreProviderService xpoDataStoreProviderService, AuthenticationStateProvider authenticationStateProvider, ISecurityUserProvider securityUserProvider, IUserClaimsProvider userClaimsProvider) {
            this.httpContextAccessor = httpContextAccessor;
            this.authenticationStateProvider = authenticationStateProvider;
            this.securityUserProvider = securityUserProvider;
            this.userClaimsProvider = userClaimsProvider;
            Security = (SecurityStrategyComplex)security;
            this.xpoDataStoreProviderService = xpoDataStoreProviderService;
            if (httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated ?? false) {
                Initialize();
            } else {
                ObjectSpaceProvider = GetObjectSpaceProvider(Security);
            }
        }
        public async void Initialize() {
            // ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
            ObjectSpaceProvider = GetObjectSpaceProvider(Security);
            Login(Security, ObjectSpaceProvider);
        }
        private void SignIn(HttpContext httpContext, string userKey, string userName) {
            var claims = userClaimsProvider.CreateUserClaims(userKey, userName, null);
            ClaimsIdentity id = new ClaimsIdentity(claims, SecurityDefaults.PasswordAuthentication, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal principal = new ClaimsPrincipal(id);
            httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        public bool InitConnection(string userName, string password) {
            AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
            Security.Logoff();
            //((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(AuthenticationStandardProvider).Name, parameters);

            IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(Security);
            try {
                IObjectSpace objectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
                var xafUser = (ISecurityUser)securityUserProvider.Authenticate(objectSpace, parameters);
                if (xafUser != null) {
                    string userKey = objectSpace.GetKeyValueAsString(xafUser);
                    SignIn(httpContextAccessor.HttpContext, userKey, xafUser.UserName);
                    return true;
                }
            } catch {

            }
            return false;
        }
        private IObjectSpaceProvider GetObjectSpaceProvider(ISecurityStrategyBase security) {
            SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)security, xpoDataStoreProviderService.GetDataStoreProvider(), true);
            RegisterEntities(objectSpaceProvider);
            return objectSpaceProvider;
        }
        private void Login(ISecurityStrategyBase security, IObjectSpaceProvider objectSpaceProvider) {
            IObjectSpace objectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
            try {
                security.Logon(objectSpace);
            } catch {
                httpContextAccessor.HttpContext.SignOutAsync();
                //navigationManager.NavigateTo($"/Login", forceLoad: true);
            }
            
        }
        public static void RegisterEntities(IObjectSpaceProvider objectSpaceProvider) {
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
        }
        public void Dispose() {
            //TODO: WTF?
            //(IDisposable)Security)?.Dispose();

            ((SecuredObjectSpaceProvider)ObjectSpaceProvider)?.Dispose();
        }
    }
}
