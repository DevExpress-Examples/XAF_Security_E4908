using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Blazor.ServerSide.Pages {
    public class LoginModel : PageModel {
        readonly IStandardAuthenticationService authenticationStandard;

        public LoginModel(IStandardAuthenticationService authenticationStandard) {
            this.authenticationStandard = authenticationStandard;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet() {
            if(User.Identity.IsAuthenticated) {
                return Redirect("/");
            }
            else {
                Input = new InputModel();
                string userName = Request.Cookies["userName"]?.ToString();
                Input.UserName = userName ?? "User";
                return Page();
            }
        }
        public IActionResult OnPost() {
            Response.Cookies.Append("userName", Input.UserName ?? string.Empty);
            if(ModelState.IsValid) {
                ClaimsPrincipal principal = authenticationStandard.Authenticate(new AuthenticationStandardLogonParameters(Input.UserName, Input.Password));
                if(principal != null) {
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
