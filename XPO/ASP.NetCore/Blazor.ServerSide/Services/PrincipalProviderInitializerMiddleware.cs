using Blazor.ServerSide.Helpers;

namespace Blazor.ServerSide.Services {
    public class PrincipalProviderInitializerMiddleware {
        private readonly RequestDelegate next;
        public PrincipalProviderInitializerMiddleware(RequestDelegate next) {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, IPrincipalProviderInitializer principalProviderInitializer) {
            principalProviderInitializer.InitializeUser(context.User);
            await next(context);
        }
    }
}
