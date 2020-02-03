using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using XafSolution.Module.BusinessObjects;

namespace ASPNETCoreODataService {
	public class SecurityProvider : IDisposable {
		public SecurityStrategyComplex Security { get; private set; }
		public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
		XpoDataStoreProviderService xpoDataStoreProviderService;
		IConfiguration config;
		IHttpContextAccessor contextAccessor;
		public SecurityProvider(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, IHttpContextAccessor contextAccessor) {
			this.xpoDataStoreProviderService = xpoDataStoreProviderService;
			this.config = config;
			this.contextAccessor = contextAccessor;
			if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
				Initialize();
			}
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
		private void SignIn(HttpContext httpContext, string userName) {
			List<Claim> claims = new List<Claim>{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
			};
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			ClaimsPrincipal principal = new ClaimsPrincipal(id);
			httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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
			string connectionString = config.GetConnectionString("XafApplication");
			SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProviderService.GetDataStoreProvider(connectionString, null, true), true);
			RegisterEntities(objectSpaceProvider);
			return objectSpaceProvider;
		}
		private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
			IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
			security.Logon(objectSpace);
		}
		private void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
			objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
			objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
			objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
		}
		public void Dispose() {
			Security?.Dispose();
			((SecuredObjectSpaceProvider)ObjectSpaceProvider)?.Dispose();
		}
	}
}
