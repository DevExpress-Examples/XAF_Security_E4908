using System;
using System.Linq;
using DevExpress.Data.Filtering;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using XafSolution.Module.BusinessObjects;

namespace ASPNETCoreODataService.Controllers {
	public class DepartmentsController : SecuredController {
		public DepartmentsController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper, IHttpContextAccessor contextAccessor)
			: base(xpoDataStoreProviderService, config, securityHelper, contextAccessor) { }
		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			IQueryable<Department> departments = ObjectSpace.GetObjectsQuery<Department>();
			return Ok(departments);
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get(Guid key) {
			Department department = ObjectSpace.GetObjectByKey<Department>(key);
			return department != null ? Ok(department) : (ActionResult)NoContent();
		}
	}
}
