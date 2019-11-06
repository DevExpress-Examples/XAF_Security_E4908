using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace AspNetCoreMvcApplication.Controllers {
    public class AuthenticationController : Controller {
        [HttpGet]
        [Route("Logout")]
        public async Task<ActionResult> Logout() {
            await HttpContext.SignOutAsync();
            return Ok();
        }
        [Route("Authentication")]
        public IActionResult Authentication() {
            string userName = HttpContext.Request.Cookies["userName"];
            if (string.IsNullOrWhiteSpace(userName)) {
                userName = "User";
            }
            Models.Login login = new Models.Login();
            login.UserName = userName;
            return View(login);
        }
        [HttpPost]
        [Route("Authentication")]
        public IActionResult Authentication(Models.Login login) {            
            if (!ModelState.IsValid) {
                HttpContext.Response.StatusCode = 401;
                return View(login);
            }
            HttpContext.Response.Cookies.Append("userName", login.UserName);
            return Redirect("/");
        }
    }

    public static class ModelStateExtensions {
        public static string GetErrorMessage(this ModelStateDictionary modelState) {
            string errorText = string.Empty;
            foreach (ModelStateEntry modelStateEntry in modelState.Values) {
                foreach (ModelError modelError in modelStateEntry.Errors) {
                    errorText += modelError.ErrorMessage;
                }
            }
            return errorText;
        }
    }
}

