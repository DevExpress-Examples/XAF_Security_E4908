using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Internal;
using DevExpress.ExpressApp.Services;
using System.Security.Claims;

namespace SecutirySharedLibrary.Services {
    public class SecurityStandartAuthenticationService {

        readonly ISecurityUserProvider securityUserProvider;
        readonly ISecurityStrategyBase security;
        readonly IObjectSpaceFactory objectSpaceFactory;

        public SecurityStandartAuthenticationService(ISecurityStrategyBase security, IObjectSpaceFactory objectSpaceFactory, ISecurityUserProvider securityUserProvider) {
            this.security = security;
            this.objectSpaceFactory = objectSpaceFactory;
            this.securityUserProvider = securityUserProvider;
        }

        private ClaimsPrincipal CreatePrincipal(string userKey, string userName) {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, userKey),
                new Claim(ClaimTypes.Name, userName),
                new Claim(SecurityDefaults.AuthenticationPassed, SecurityDefaults.AuthenticationPassed)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, SecurityDefaults.PasswordAuthentication, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal principal = new ClaimsPrincipal(id);
            return principal;
        }

        public ClaimsPrincipal? Authenticate(string userName, string password) {
            using (IObjectSpace loginObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace(security.UserType)) {
                AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
                security.Logoff();
                try {
                    var xafUser = (ISecurityUser)securityUserProvider.Authenticate(loginObjectSpace, parameters);
                    if (xafUser != null) {
                        string userKey = loginObjectSpace.GetKeyValueAsString(xafUser);
                        return CreatePrincipal(userKey, xafUser.UserName);
                    }
                } catch {
                    //XafSecurity authentication failed
                }
                return null;

            }
        }
    }
}
