//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Security.Claims;
//using DevExpress.ExpressApp.Security;
//using Microsoft.AspNetCore.Authentication;

//namespace Blazor.ServerSide.Services {
//    public static class SignInMiddlewareDefaults {
//        public const string SignInEndpointName = "api/signIn";
//        public const string SignOutEndpointName = "api/signOut";
//        //public const string DefaultClaimsIssuer = SecurityDefaults.PasswordAuthentication;
//    }
//    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
//    internal class SignInMiddleware {
//        private readonly RequestDelegate next;
//        public SignInMiddleware(RequestDelegate next) {
//            this.next = next;
//        }
//        public async Task Invoke(HttpContext context, ILogger<SignInMiddleware> logger = null) {
//            string requestPath = context.Request.Path.Value.TrimStart('/');
//            if(requestPath.StartsWith(SignInMiddlewareDefaults.SignInEndpointName, StringComparison.Ordinal)) {
//                IUserTokenProcessor userTokenProcessor = context.RequestServices.GetRequiredService<IUserTokenProcessor>();
//                string userToken = context.Request.Query["UserTokenName"];
//                try {
//                    var claims = userTokenProcessor.CreateUserClaims(userToken);
//                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, SecurityDefaults.PasswordAuthentication));
//                    await context.SignInAsync(user);
//                }
//                catch { }
//                context.Response.Redirect("/");
//            }
//            else if(requestPath.StartsWith(SignInMiddlewareDefaults.SignOutEndpointName, StringComparison.Ordinal)) {
//                await context.SignOutAsync();
//                context.Response.Redirect("/Login");
//            }
//            await next(context);
//        }
//    }
//}
