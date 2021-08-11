using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Configuration;
using System;

namespace DevExtreme.OData.Controllers {
	public class AccountController : ODataController, IDisposable {
		SecurityProvider securityProvider;
		public AccountController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
		}
		[HttpPost("Login")]
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
		[HttpGet("Logout")]
		public ActionResult Logout() {
			HttpContext.SignOutAsync();
			return Ok();
		}
		void IDisposable.Dispose() {
			securityProvider?.Dispose();
		}
	}
}
