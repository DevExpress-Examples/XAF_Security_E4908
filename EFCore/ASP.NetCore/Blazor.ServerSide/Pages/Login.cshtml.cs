using Blazor.ServerSide.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Blazor.ServerSide.Pages {
    public class LoginModel : PageModel {
        [BindProperty]
        public InputModel Input { get; set; }
        private SecurityProvider securityProvider;
        public LoginModel(SecurityProvider securityProvider) {
            this.securityProvider = securityProvider;
        }
        public void OnGet() {
            Input = new InputModel();
            string userName = Request.Cookies["userName"]?.ToString();
            Input.UserName = userName ?? "User";
        }
        public IActionResult OnPost() {
            Response.Cookies.Append("userName", Input.UserName ?? string.Empty);
            if(ModelState.IsValid) {
                if(securityProvider.InitConnection(Input.UserName, Input.Password)) {
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