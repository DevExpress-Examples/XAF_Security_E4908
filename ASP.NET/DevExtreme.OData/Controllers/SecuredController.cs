using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ASPNETCoreODataService.Controllers {
	public class SecuredController : BaseController {
		protected SecurityStrategyComplex Security { get; set; }
		protected IObjectSpace ObjectSpace { get; set; }
		public SecuredController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper, IHttpContextAccessor contextAccessor)
			: base(xpoDataStoreProviderService, config, securityHelper) {
			Security = SecurityProvider.GetSecurity(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
			string connectionString = Config.GetConnectionString("XafApplication");
			IObjectSpaceProvider objectSpaceProvider = SecurityProvider.GetObjectSpaceProvider(Security, XpoDataStoreProviderService, connectionString);
			SecurityProvider.Login(Security, objectSpaceProvider);
			ObjectSpace = objectSpaceProvider.CreateObjectSpace();
		}
	}
}
