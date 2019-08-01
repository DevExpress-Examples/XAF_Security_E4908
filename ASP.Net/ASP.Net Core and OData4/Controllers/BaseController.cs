using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ASPNETCoreODataService.Controllers {
	[Route("api/[controller]")]
	public class BaseController : ODataController {
		protected SecurityStrategyComplex Security { get; set; }
		protected IObjectSpace ObjectSpace { get; set; }
		protected XpoDataStoreProviderService XpoDataStoreProviderService { get; set; }
		protected IConfiguration Config { get; set; }
		public BaseController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config) {
			XpoDataStoreProviderService = xpoDataStoreProviderService;
			Config = config;
		}
		protected void Init() {
			Security = ConnectionHelper.GetSecurity(typeof(IdentityAuthenticationProvider).Name, HttpContext?.User?.Identity);
			string connectionString = Config.GetConnectionString("XafApplication");
			IObjectSpaceProvider objectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(Security, XpoDataStoreProviderService, connectionString);
			ConnectionHelper.Login(Security, objectSpaceProvider);
			ObjectSpace = objectSpaceProvider.CreateObjectSpace();
		}
	}
}
