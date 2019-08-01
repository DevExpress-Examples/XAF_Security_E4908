using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ASPNETCoreODataService.Controllers {
	public class AccountController : BaseController {
		public AccountController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config) : base(xpoDataStoreProviderService, config) { }
		[HttpGet]
		[ODataRoute("Login(userName={userName}, password={password})")]
		[AllowAnonymous]
		public ActionResult Login(string userName, string password) {
			ActionResult result;
			string connectionString = Config.GetConnectionString("XafApplication");
			if(ConnectionHelper.InitConnection(userName, password, HttpContext, XpoDataStoreProviderService, connectionString)) {
				result = Ok();
			}
			else {
				result = Unauthorized();
			}
			return result;
		}

		[HttpGet]
		[ODataRoute("Logoff()")]
		public ActionResult Logoff() {
			HttpContext.SignOutAsync();
			return Ok();
		}
	}
}
