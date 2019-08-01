using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using XafSolution.Module.BusinessObjects;

namespace ASPNETCoreODataService.Controllers {
	public class EmployeesController : BaseController {
		public EmployeesController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config) : base(xpoDataStoreProviderService, config) { }

		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			Init();
			IQueryable<Employee> employees = ObjectSpace.GetObjects<Employee>().AsQueryable();
			return Ok(employees);
		}
	}
}
