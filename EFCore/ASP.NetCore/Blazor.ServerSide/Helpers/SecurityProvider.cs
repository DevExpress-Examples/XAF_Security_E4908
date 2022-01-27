using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using BusinessObjectsLibrary.BusinessObjects;

namespace Blazor.ServerSide.Helpers {
    public class SecurityProvider : IDisposable {
        public SecurityStrategyComplex Security { get; private set; }
        public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
        IHttpContextAccessor contextAccessor;
        IDbContextFactory<ApplicationDbContext> xafDbContextFactory;
        public SecurityProvider(SecurityStrategyComplex security, IDbContextFactory<ApplicationDbContext> xafDbContextFactory, IHttpContextAccessor contextAccessor) {
            this.xafDbContextFactory = xafDbContextFactory;
            Security = security;
            this.contextAccessor = contextAccessor;
            if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                Initialize();
            }
        }
        public bool InitConnection(string userName, string password) {
            AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
            Security.Logoff();
            ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(AuthenticationStandardProvider).Name, parameters);
            IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(Security);
            try {
                Login(Security, objectSpaceProvider);
                SignIn(contextAccessor.HttpContext, userName);
                return true;
            } catch {
                return false;
            }
        }
        public void Initialize() {
            ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
            ObjectSpaceProvider = GetObjectSpaceProvider(Security);
            Login(Security, ObjectSpaceProvider);
        }
        private void SignIn(HttpContext httpContext, string userName) {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal principal = new ClaimsPrincipal(id);
            httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
            SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, xafDbContextFactory, security.TypesInfo);
            return objectSpaceProvider;
        }
        private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
            IObjectSpace objectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
            security.Logon(objectSpace);
        }
        public void Dispose() {
            Security?.Dispose();
            ((SecuredEFCoreObjectSpaceProvider)ObjectSpaceProvider)?.Dispose();
        }
    }
}