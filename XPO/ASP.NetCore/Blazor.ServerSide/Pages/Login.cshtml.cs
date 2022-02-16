using Blazor.ServerSide.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Blazor.ServerSide.Pages {
    public class LoginModel : PageModel {
        readonly SecurityStandartAuthenticationService xafSecurityAuthentication;

        public LoginModel(SecurityStandartAuthenticationService xafSecurityAuthentication) {
            this.xafSecurityAuthentication = xafSecurityAuthentication;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet() {
            Input = new InputModel();
            string userName = Request.Cookies["userName"]?.ToString();
            Input.UserName = userName ?? "User";
        }
        public IActionResult OnPost() {
            Response.Cookies.Append("userName", Input.UserName ?? string.Empty);
            if (ModelState.IsValid) {

                ClaimsPrincipal principal = xafSecurityAuthentication.Authenticate(Input.UserName, Input.Password);
                if (principal != null) {
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return Redirect("/");
                }
                ModelState.AddModelError("Error", "User name or password is incorrect");
            }
            return Page();
        }
    }
    public class InputModel {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
