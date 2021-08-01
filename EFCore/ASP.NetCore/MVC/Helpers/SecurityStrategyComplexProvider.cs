using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApplication {
    public class SecurityStrategyComplexProvider {
		SecurityStrategyComplex security;
		
		public SecurityStrategyComplex GetSecurity() {
			if(security == null) {
				AuthenticationMixed authentication = new AuthenticationMixed();
				authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
				authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
				authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
				security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
			}
			return security;
		}
	}
}
