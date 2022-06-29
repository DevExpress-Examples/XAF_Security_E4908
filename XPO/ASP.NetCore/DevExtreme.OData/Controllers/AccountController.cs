using DevExpress.ExpressApp.Security;
using System.Security.Claims;
using DevExpress.ExpressApp.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DevExtreme.OData.Controllers {
    public class AccountController : ODataController {
        readonly IStandardAuthenticationService authenticationStandard;

        public AccountController(IStandardAuthenticationService authenticationStandard) {
            this.authenticationStandard = authenticationStandard;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password) {
            Response.Cookies.Append("userName", userName ?? string.Empty);
            ClaimsPrincipal principal = authenticationStandard.Authenticate(new AuthenticationStandardLogonParameters(userName, password));
            if(principal != null) {
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok();
            }
            return Unauthorized();
        }
        [HttpGet("Logout")]
        public ActionResult Logout() {
            HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
