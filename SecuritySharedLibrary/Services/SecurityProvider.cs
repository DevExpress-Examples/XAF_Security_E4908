using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Services;

namespace SecutirySharedLibrary.Services {
    public interface ISecurityProvider {
        SecurityStrategy GetSecurity();
    }

    public class SecurityProvider : ISecurityProvider {
        private ISecurityStrategyBase security;
        private readonly IObjectSpaceFactory objectSpaceFactory;
        private readonly PrincipalAuthenticationService xafSecurityLogin;

        public SecurityProvider(ISecurityStrategyBase security, IObjectSpaceFactory objectSpaceFactory, PrincipalAuthenticationService xafSecurityLogin) {
            this.security = security;
            this.objectSpaceFactory = objectSpaceFactory;
            this.xafSecurityLogin = xafSecurityLogin;
        }
        SecurityStrategy ISecurityProvider.GetSecurity() {
            xafSecurityLogin.XafSecurityEnsureLogon(objectSpaceFactory);
            return (SecurityStrategy)security;
        }
    }
}
