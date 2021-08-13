using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using DevExpress.ExpressApp;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using BusinessObjectsLibrary.BusinessObjects;

namespace MvcApplication.Controllers {
	[Authorize]
	[Route("api/[controller]")]
	public class EmployeesController : Microsoft.AspNetCore.Mvc.Controller {
		SecurityProvider securityProvider;
		IObjectSpace objectSpace;
		public EmployeesController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
			objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
		}
		[HttpGet]
		public object Get(DataSourceLoadOptions loadOptions) {
			IQueryable<Employee> employees = objectSpace.GetObjectsQuery<Employee>();
			return DataSourceLoader.Load(employees, loadOptions);
		}
		[HttpDelete]
		public ActionResult Delete(int key) {
			Employee existing = objectSpace.GetObjectByKey<Employee>(key);
			if(existing != null) {
				objectSpace.Delete(existing);
				objectSpace.CommitChanges();
				return NoContent();
			}
			return NotFound();
		}
		[HttpPut]
		public ActionResult Update(int key, string values) {
			Employee employee = objectSpace.GetObjectByKey<Employee>(key);
			if(employee != null) {
				JsonParser.ParseJObject<Employee>(JObject.Parse(values), employee, objectSpace);
				return Ok(employee);
			}
			return NotFound();
		}
		[HttpPost]
		public ActionResult Add(string values) {
			Employee employee = objectSpace.CreateObject<Employee>();
			JsonParser.ParseJObject<Employee>(JObject.Parse(values), employee, objectSpace);
			return Ok(employee);
		}
		protected override void Dispose(bool disposing) {
			if(disposing) {
				objectSpace?.Dispose();
				securityProvider?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
