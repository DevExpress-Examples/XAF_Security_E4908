using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blazor.ServerSide.Pages {
    public class LogOutModel : PageModel {
        public IActionResult OnGet() {
            this.HttpContext.SignOutAsync();
            return Redirect("/Login");
        }
    }
}