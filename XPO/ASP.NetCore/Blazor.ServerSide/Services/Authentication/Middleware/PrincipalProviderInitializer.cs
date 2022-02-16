using Blazor.ServerSide.Services;

namespace Blazor.ServerSide.Services {
    public class PrincipalProviderInitializer {
        private readonly RequestDelegate next;
        public PrincipalProviderInitializer(RequestDelegate next) {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, IPrincipalProviderInitializer principalProviderInitializer) {
            principalProviderInitializer.InitializeUser(context.User);
            await next(context);
        }
    }
}
