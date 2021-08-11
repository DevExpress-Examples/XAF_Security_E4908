using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebFormsApplication {
	public static class ConnectionHelper {
		public static SecuredObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
			SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, XpoDataStoreProviderService.GetDataStoreProvider(null, true), true);
			RegisterEntities(objectSpaceProvider);
			return objectSpaceProvider;
		}
		public static SecurityStrategyComplex GetSecurity(string authenticationName, object parameter) {
			AuthenticationMixed authentication = new AuthenticationMixed();
			authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
			authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
			authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
			authentication.SetupAuthenticationProvider(authenticationName, parameter);
			SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
			security.RegisterXPOAdapterProviders();
			return security;
		}
		public static void RegisterEntities(IObjectSpaceProvider objectSpaceProvider) {
			objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
			objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
			objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
		}
	}
}