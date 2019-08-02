using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreODataService {
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
			}
			else {
				context.Response.Redirect(authenticationPagePath);
			}
		}
		private static bool IsAllowAnonymous(HttpContext context) {
			string referer = ((HttpRequestHeaders)context.Request.Headers).HeaderReferer.FirstOrDefault();
			return context.Request.Path.HasValue && context.Request.Path.StartsWithSegments(authenticationPagePath)
				|| referer != null && referer.Contains(authenticationPagePath);
		}
	}
}
