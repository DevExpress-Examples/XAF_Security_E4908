using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;

namespace Blazor.ServerSide.Helpers {
    public interface ISecurityProvider {
        SecurityStrategy GetSecurity();
    }

    public class SecurityProvider : ISecurityProvider, IDisposable {
        private ISecurityStrategyBase security;
        private IObjectSpace loginObjectSpace;
        private readonly IObjectSpaceProviderService objectSpaceProvider;
        private readonly XafSecurityLoginService xafSecurityLogin;

        public SecurityProvider(ISecurityStrategyBase security, IObjectSpaceProviderService objectSpaceProvider, XafSecurityLoginService xafSecurityLogin) {
            this.security = security;
            this.objectSpaceProvider = objectSpaceProvider;
            this.xafSecurityLogin = xafSecurityLogin;
        }
        SecurityStrategy ISecurityProvider.GetSecurity() {
            if (!xafSecurityLogin.IsLoginOperationExecuted) {
                loginObjectSpace = objectSpaceProvider.CreateNonSecuredObjectSpace(security.UserType);
                if (!xafSecurityLogin.TryLogin(loginObjectSpace)) {
                    //TODO test that behaviour
                    throw new Exception("Authentication failed");
                }
            }

            return (SecurityStrategy)security;
        }

        void IDisposable.Dispose() {
            loginObjectSpace?.Dispose();
            loginObjectSpace = null;
        }
    }
}
