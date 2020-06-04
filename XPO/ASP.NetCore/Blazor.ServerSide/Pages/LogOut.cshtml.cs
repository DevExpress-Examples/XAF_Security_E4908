using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApplication.NetCore
{
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