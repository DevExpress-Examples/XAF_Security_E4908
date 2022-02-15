using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using BusinessObjectsLibrary.BusinessObjects;
using Blazor.ServerSide.Services;

namespace Blazor.ServerSide.Helpers {
	public class SecurityProvider : IDisposable {
		public SecurityStrategyComplex Security { get; private set; }
		public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
		IXpoDataStoreProvider xpoDataStoreProvider;
		IHttpContextAccessor contextAccessor;
    
		public SecurityProvider(SecurityStrategyComplex security, IXpoDataStoreProvider xpoDataStoreProvider, IHttpContextAccessor contextAccessor, DemoDataInMemoryProvider demoDataInMemoryProvider) {
			Security = security;
			this.xpoDataStoreProvider = xpoDataStoreProvider;
			this.contextAccessor = contextAccessor;
			if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
				Initialize();
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
		private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
			SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProvider, security.TypesInfo, null);
			return objectSpaceProvider;
		}
		private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
			IObjectSpace objectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
			security.Logon(objectSpace);
		}
		public void Dispose() {
			Security?.Dispose();
			((SecuredObjectSpaceProvider)ObjectSpaceProvider)?.Dispose();
		}
	}
}
