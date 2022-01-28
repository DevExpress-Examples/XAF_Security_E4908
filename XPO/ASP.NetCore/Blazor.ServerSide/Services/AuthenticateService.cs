//namespace Blazor.ServerSide.Services {
//    public class AuthenticateService {

//        private void SignIn(HttpContext httpContext, string userKey, string userName) {
//            var claims = userClaimsProvider.CreateUserClaims(userKey, userName, null);
//            ClaimsIdentity id = new ClaimsIdentity(claims, SecurityDefaults.PasswordAuthentication, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
//            ClaimsPrincipal principal = new ClaimsPrincipal(id);
//            httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
//        }

//        private void SignOut() {
//        }
//    }
//}
