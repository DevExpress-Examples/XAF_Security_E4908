using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreMvcApplication.Controllers {
	public class AuthenticationController : Controller {
		SecurityProvider securityProvider;
		public AuthenticationController(SecurityProvider securityProvider){
			this.securityProvider = securityProvider;
		}
		[HttpPost]
		[Route("Login")]
		[AllowAnonymous]
		public ActionResult Login(string userName, string password) {
			ActionResult result;
			if(securityProvider.InitConnection(userName, password)) {
				result = Ok();
			}
			else {
				result = Unauthorized();
			}
			return result;
		}
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
		protected override void Dispose(bool disposing) {
			if(disposing) {
				securityProvider?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}

