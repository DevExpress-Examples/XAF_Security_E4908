using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SecutirySharedLibrary.Middleware {
    public class LogOut {

        private readonly RequestDelegate next;
        public LogOut(RequestDelegate next) {
            this.next = next;
        }
        public async Task Invoke(HttpContext context) {
            string requestPath = context.Request.Path.Value.TrimStart('/');
            //related to PrincipalAuthenticationService
            if (requestPath.StartsWith("api/logout", StringComparison.Ordinal)) {
                await context.SignOutAsync();
                context.Response.Redirect("/Login");
            } else {
                await next(context);
            }
        }
    }
}
