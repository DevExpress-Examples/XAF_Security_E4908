using Microsoft.AspNetCore.Authentication;

namespace Blazor.ServerSide.Services {
    public class LogOut {

        private readonly RequestDelegate next;
        public LogOut(RequestDelegate next) {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, ILogger<LogOut> logger = null) {
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
