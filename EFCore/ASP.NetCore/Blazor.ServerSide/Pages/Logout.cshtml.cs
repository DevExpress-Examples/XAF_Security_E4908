using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blazor.ServerSide.Pages
{
    public class LogoutModel : PageModel
    {
        HttpContext httpContext;
        public LogoutModel(IHttpContextAccessor contextAccessor) {
            httpContext = contextAccessor.HttpContext;
        }
        public async Task<IActionResult> OnGet()
        {
            await httpContext.SignOutAsync();
            return Redirect("/Login");
        }
    }
}
