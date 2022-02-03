using Microsoft.AspNetCore.Authentication;

namespace Blazor.ServerSide.Services {
    public class LogOutMiddleware {

        private readonly RequestDelegate next;
        public LogOutMiddleware(RequestDelegate next) {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, ILogger<LogOutMiddleware> logger = null) {
            string requestPath = context.Request.Path.Value.TrimStart('/');
            //related to XafSecurityLoginService
            if (requestPath.StartsWith("api/logout", StringComparison.Ordinal)) {
                await context.SignOutAsync();
                context.Response.Redirect("/Login");
            } else {
                await next(context);
            }
        }
    }
}
