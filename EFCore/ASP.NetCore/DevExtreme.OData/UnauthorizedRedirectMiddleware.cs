using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DevExtreme.OData.EFCore {
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
            IHeaderDictionary headerDictionary = context.Request.Headers;
            string referer = context.Request.Headers["Referer"];
            return context.Request.Path.HasValue && context.Request.Path.StartsWithSegments(authenticationPagePath)
                || referer != null && referer.Contains(authenticationPagePath);
        }

    }
}