using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace ASPNETCoreODataService.Controllers {
	[Route("api/[controller]")]
	public class AccountController : ODataController, IDisposable {
		SecurityProvider securityProvider;
		public AccountController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
		}
		[HttpPost]
		[ODataRoute("Login")]
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
		[ODataRoute("Logout()")]
		public ActionResult Logout() {
			HttpContext.SignOutAsync();
			return Ok();
		}
		void IDisposable.Dispose() {
			securityProvider?.Dispose();
		}
	}
}
