using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Blazor.ServerSide.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace Blazor.ServerSide.Pages {
    public class LoginModel : PageModel {
        readonly SecuredObjectSpaceService objectSpaceService;
        public LoginModel(SecuredObjectSpaceService objectSpaceService) {
            this.objectSpaceService = objectSpaceService;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public void OnGet() {
            Input = new InputModel();
            Input.UserName = Request.Cookies["userName"] ?? "User";
        }
        public async Task<IActionResult> OnPost() {
            Response.Cookies.Append("userName", Input.UserName);
            if(ModelState.IsValid) {
                if(objectSpaceService.LogonWithUserName(Input.UserName, Input.Password)) {
                    await objectSpaceService.SignInAsync(Input.UserName);
                    StringValues returnUrl;
                    if(!Request.Query.TryGetValue("returnUrl", out returnUrl)) {
                        returnUrl = "/";
                    }
                    return Redirect(returnUrl);
                } else {
                    ModelState.AddModelError("Error", "User name or passord is incorrect.");
                }
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
