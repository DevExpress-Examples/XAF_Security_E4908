using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using XafSolution.Module.BusinessObjects;

namespace ASPNETCoreODataService.Controllers {
	public class DepartmentsController : BaseController {
		public DepartmentsController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config) : base(xpoDataStoreProviderService, config) { }

		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			Init();
			IQueryable<Department> departments = ObjectSpace.GetObjects<Department>().AsQueryable();
			return Ok(departments);
		}
	}
}
