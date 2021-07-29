using BusinessObjectsLibrary.EFCore.BusinessObjects;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevExtreme.OData.EFCore {
	public class SecurityProvider : IDisposable {
		public SecurityStrategyComplex Security { get; private set; }
		public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
		IConfiguration config;
		IHttpContextAccessor contextAccessor;
		public SecurityProvider(IConfiguration config, IHttpContextAccessor contextAccessor) {
			this.config = config;
			this.contextAccessor = contextAccessor;
			if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
				Initialize();
			}
		}
		public void Initialize() {
			Security = GetSecurity(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
			ObjectSpaceProvider = GetObjectSpaceProvider(Security);
			Login(Security, ObjectSpaceProvider);
		}
		public SecurityStrategyComplex GetSecurity(string authenticationName, object parameter) {
			AuthenticationMixed authentication = new AuthenticationMixed();
			authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
			authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
			authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
			authentication.SetupAuthenticationProvider(authenticationName, parameter);
			SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
			return security;
		}
		private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
			string connectionString = config.GetConnectionString("EFCore");
			SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, typeof(ApplicationDbContext),
				(builder, _) => builder.UseSqlServer(connectionString));
			return objectSpaceProvider;
		}
		public void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
			IObjectSpace objectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
			security.Logon(objectSpace);
		}
		// Signs into HttpContext and creates a cookie.
		private void SignIn(HttpContext httpContext, string userName) {
			List<Claim> claims = new List<Claim>{
 		new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
 	};
			ClaimsIdentity id = new ClaimsIdentity(
				claims, "ApplicationCookie",
				ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType
			);
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
			catch (Exception ex) {
				return false;
			}
		}
		public void Dispose() {
			Security?.Dispose();
			((SecuredEFCoreObjectSpaceProvider)ObjectSpaceProvider)?.Dispose();
		}
	}
}
