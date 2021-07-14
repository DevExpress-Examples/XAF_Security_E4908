using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BlazorApplication.NetCore {
	public class SecurityProvider : IDisposable {
		public SecurityStrategyComplex Security { get; private set; }
		public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
		private XpoDataStoreProviderService xpoDataStoreProviderService;
		private IHttpContextAccessor contextAccessor;
		private IEnumerable<Type> securedTypes;
		public SecurityProvider(XpoDataStoreProviderService xpoDataStoreProviderService, IHttpContextAccessor contextAccessor, IOptions<SecurityOptions> securityOptions) {
			this.xpoDataStoreProviderService = xpoDataStoreProviderService;
			this.contextAccessor = contextAccessor;
			securedTypes = securityOptions.Value.SecuredTypes;
			if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
				Initialize();
			}
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
			SecurityStrategyComplex security = GetSecurity(typeof(AuthenticationStandardProvider).Name, parameters);
			IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(security);
			try {
				Login(security, objectSpaceProvider);
				SignIn(contextAccessor.HttpContext, userName);
				return true;
			}
			catch {
				return false;
			}
		}
		public void Initialize() {
			Security = GetSecurity(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
			ObjectSpaceProvider = GetObjectSpaceProvider(Security);
			Login(Security, ObjectSpaceProvider);
		}
		private SecurityStrategyComplex GetSecurity(string authenticationName, object parameter) {
			AuthenticationMixed authentication = new AuthenticationMixed();
			authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
			authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
			authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
			authentication.SetupAuthenticationProvider(authenticationName, parameter);
			SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
			security.RegisterXPOAdapterProviders();
			return security;
		}
		private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
			SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProviderService.GetDataStoreProvider(), true);
			RegisterEntities(objectSpaceProvider);
			return objectSpaceProvider;
		}
		private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
			IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
			security.Logon(objectSpace);
		}
		private void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
			if(securedTypes != null) {
				foreach(Type type in securedTypes) {
					objectSpaceProvider.TypesInfo.RegisterEntity(type);
				}
			}
		}
		public void Dispose() {
			Security?.Dispose();
			((SecuredObjectSpaceProvider)ObjectSpaceProvider)?.Dispose();
		}
	}

	public class SecurityOptions {
		public IEnumerable<Type> SecuredTypes { get; set; }
	}
}

namespace Microsoft.Extensions.DependencyInjection {
	using BlazorApplication.NetCore;

	public static class SecurityProviderExtensions {
		public static IServiceCollection AddXafSecurity(this IServiceCollection services) {
			return services.AddSingleton<XpoDataStoreProviderService>()
				.AddScoped<SecurityProvider>();
		}
		public static IServiceCollection AddSecuredTypes(this IServiceCollection service, params Type[] types) {
			return service.Configure<SecurityOptions>(options => {
				options.SecuredTypes = types;
			});
		}
	}
}

