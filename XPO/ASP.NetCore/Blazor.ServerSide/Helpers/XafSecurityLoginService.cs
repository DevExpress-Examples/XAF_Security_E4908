using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Components;

namespace Blazor.ServerSide.Helpers {

    public class XafSecurityLoginService {
        readonly ISecurityStrategyBase security;
        readonly IPrincipalProvider principalProvider;
        readonly NavigationManager navigationManager;
        bool isLoginOperationExecuted;

        public XafSecurityLoginService(ISecurityStrategyBase security, NavigationManager navigationManager, IPrincipalProvider principalProvider) {
            this.security = security;
            this.navigationManager = navigationManager;
            this.principalProvider = principalProvider;
        }

        public bool IsLoginOperationExecuted => isLoginOperationExecuted;

        public bool TryLogin(IObjectSpace loginObjectSpace) {
            if (principalProvider.User?.Identity.IsAuthenticated ?? false) {
                try {
                    isLoginOperationExecuted = true;
                    security.Logon(loginObjectSpace);
                    return true;
                } catch {
                    //related to LogOutMiddleware
                    navigationManager.NavigateTo($"api/logout", forceLoad: true);
                }
            }
            return false;
        }
    }
}
