using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DevExtreme.OData {
    public class UnauthorizedRedirectMiddleware {
        private const string authenticationPagePath = "/Authentication.html";
        private readonly RequestDelegate _next;
        public UnauthorizedRedirectMiddleware(RequestDelegate next) {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context) {
            if(context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated
                || IsAllowAnonymous(context)) {
                await _next(context);
            } else {
                context.Response.Redirect(authenticationPagePath);
            }
        }
        private static bool IsAllowAnonymous(HttpContext context) {
            string referer = context.Request.Headers["Referer"];
            return context.Request.Path.HasValue && context.Request.Path.StartsWithSegments(authenticationPagePath)
                || referer != null && referer.Contains(authenticationPagePath);
        }

    }
}