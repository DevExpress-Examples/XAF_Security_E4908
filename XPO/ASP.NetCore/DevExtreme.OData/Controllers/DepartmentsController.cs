using System;
using System.Linq;
using BusinessObjectsLibrary;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Configuration;

namespace ASPNETCoreODataService.Controllers {
	public class DepartmentsController : ODataController, IDisposable {
		SecurityProvider securityProvider;
		IObjectSpace objectSpace;
		public DepartmentsController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
			objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			IQueryable<Department> departments = ((XPQuery<Department>)objectSpace.GetObjectsQuery<Department>());
			return Ok(departments);
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get(Guid key) {
			Department department = objectSpace.GetObjectByKey<Department>(key);
			return department != null ? Ok(department) : (ActionResult)NoContent();
		}
		public void Dispose() {
			objectSpace?.Dispose();
			securityProvider?.Dispose();
		}
	}
}
