using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ASPNETCoreODataService.Controllers {
	public class AccountController : BaseController {
		public AccountController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper)
			: base(xpoDataStoreProviderService, config, securityHelper) { }
		[HttpPost]
		[ODataRoute("Login")]
		[AllowAnonymous]
		public ActionResult Login(string userName, string password) {
			ActionResult result;
			string connectionString = Config.GetConnectionString("XafApplication");
			if(SecurityProvider.InitConnection(userName, password, HttpContext, XpoDataStoreProviderService, connectionString)) {
				result = Ok();
			}
			else {
				result = Unauthorized();
			}
			return result;
		}

		[HttpGet]
		[ODataRoute("Logout()")]
		public ActionResult Logout() {
			HttpContext.SignOutAsync();
			return Ok();
		}
	}
}
