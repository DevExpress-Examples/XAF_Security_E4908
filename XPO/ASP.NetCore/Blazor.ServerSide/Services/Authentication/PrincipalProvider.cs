using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Principal;

namespace Blazor.ServerSide.Services {
    public interface IPrincipalProviderInitializer {
        public Task InitializeUserAsync();
        public void InitializeUser(IPrincipal principal);
    }

    public class PrincipalProvider : IPrincipalProvider, IPrincipalProviderInitializer {
        private bool isUser;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public PrincipalProvider(AuthenticationStateProvider authenticationStateProvider) {
            this.authenticationStateProvider = authenticationStateProvider;
        }
        private IPrincipal user;

        public IPrincipal User {
            get {
                if (!isUser) {
                    throw new Exception("User is not initialized");
                }
                return user;
            }
        }

        public async Task InitializeUserAsync() {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            InitializeUser(authState.User);
        }

        public void InitializeUser(IPrincipal principal) {
            ArgumentNullException.ThrowIfNull(principal, nameof(principal));
            user = principal;
            isUser = true;
        }
    }
}
