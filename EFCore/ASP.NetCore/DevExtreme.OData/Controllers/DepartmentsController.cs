using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DevExpress.ExpressApp;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Core;

namespace DevExtreme.OData.Controllers {
	public class DepartmentsController : ODataController, IDisposable {
		IObjectSpace objectSpace;
		public DepartmentsController(IObjectSpaceFactory securityProvider) {
			objectSpace = securityProvider.CreateObjectSpace<Department>();
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			IQueryable<Department> departments = objectSpace.GetObjectsQuery<Department>();
			return Ok(departments);
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get(int key) {
			Department department = objectSpace.GetObjectByKey<Department>(key);
			return department != null ? Ok(department) : (ActionResult)NoContent();
		}
		public void Dispose() {
			objectSpace?.Dispose();
		}
	}
}
