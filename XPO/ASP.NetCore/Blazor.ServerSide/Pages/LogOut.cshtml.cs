using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blazor.ServerSide.Pages {
    public class LogOutModel : PageModel {
        HttpContext httpContext;
        public LogOutModel(IHttpContextAccessor contextAccessor) {
            httpContext = contextAccessor.HttpContext;
        }
        public IActionResult OnGet() {
            httpContext.SignOutAsync();
            return Redirect("/Login");
        }
    }
}