using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreODataService.Controllers {
	[Route("api/[controller]")]
	public class BaseController : ODataController {
		protected XpoDataStoreProviderService XpoDataStoreProviderService { get; set; }
		protected SecurityProvider SecurityProvider { get; set; }
		protected IConfiguration Config { get; set; }
		public BaseController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper) {
			XpoDataStoreProviderService = xpoDataStoreProviderService;
			Config = config;
			SecurityProvider = securityHelper;
		}
	}
}
