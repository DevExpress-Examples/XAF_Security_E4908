using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.ServerSide.Services {

    public class PrincipalAuthenticationService : IDisposable {
        readonly ISecurityStrategyBase security;
        readonly IPrincipalProvider principalProvider;
        readonly NavigationManager navigationManager;

        IObjectSpace loginObjectSpace;
        bool isLoginOperationExecuted;

        public PrincipalAuthenticationService(ISecurityStrategyBase security, NavigationManager navigationManager, IPrincipalProvider principalProvider) {
            this.security = security;
            this.navigationManager = navigationManager;
            this.principalProvider = principalProvider;
        }

        private bool IsUserAuthenticated => principalProvider.User?.Identity.IsAuthenticated ?? false;

        public void XafSecurityEnsureLogon(IObjectSpaceFactory objectSpaceFactory) {
            if (!isLoginOperationExecuted && IsUserAuthenticated) {
                loginObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace(security.UserType);// GetNonSecuredObjectSpaceProvider(security.UserType).CreateNonsecuredObjectSpace();
                try {
                    isLoginOperationExecuted = true;
                    security.Logon(loginObjectSpace);
                } catch {
                    //related to LogOutMiddleware
                    navigationManager.NavigateTo($"api/logout", forceLoad: true);
                    throw new Exception("Authentication failed");
                }
            }
        }

        void IDisposable.Dispose() {
            loginObjectSpace?.Dispose();
            loginObjectSpace = null;
        }
    }
}
