using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;

namespace DevExtreme.OData.Controllers {
    public class AccountController : ODataController {
        private readonly SignInManager signInManager;
        private readonly IAntiforgery antiforgery;

        public AccountController(SignInManager signInManager, IAntiforgery antiforgery) {
            this.signInManager = signInManager;
            this.antiforgery = antiforgery;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string userName, string password) {
            Response.Cookies.Append("userName", userName ?? string.Empty);
            var authenticationResult = signInManager.AuthenticateByLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
            if(authenticationResult.Succeeded) {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal);
                try {
                    HttpContext.User = authenticationResult.Principal;
                    var tokens = antiforgery.GetAndStoreTokens(HttpContext);
                    return Ok(tokens.RequestToken);
                } finally {
                    HttpContext.User = new ClaimsPrincipal();
                }
            }
            return Unauthorized();
        }
        [HttpGet("Logout")]
        public async Task<ActionResult> Logout() {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
