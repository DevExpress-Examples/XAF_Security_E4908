using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;
using BusinessObjectsLibrary;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using System.Text.Json;

namespace ASPNETCoreODataService.Controllers {
	public class EmployeesController : ODataController, IDisposable {
		SecurityProvider securityProvider;
		IObjectSpace objectSpace;
		public EmployeesController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
			objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			IQueryable<Employee> employees = ((XPQuery<Employee>)objectSpace.GetObjectsQuery<Employee>());
			return Ok(employees);
		}
		[HttpDelete]
		public ActionResult Delete(Guid key) {
			Employee existing = objectSpace.GetObjectByKey<Employee>(key);
			if(existing != null) {
			    objectSpace.Delete(existing);
			    objectSpace.CommitChanges();
			    return NoContent();
			}
            return NotFound(); 
		}
		[HttpPatch]
		public ActionResult Patch(Guid key, [FromBody]JsonElement jElement) {
			Employee employee = objectSpace.FirstOrDefault<Employee>(e => e.Oid == key);
			if(employee != null) {
				JsonParser.ParseJObject<Employee>(jElement, employee, objectSpace);
				return Ok(employee);
			}
			return NotFound();
		}
		[HttpPost]
		public ActionResult Post([FromBody]JsonElement jElement) {
			Employee employee = objectSpace.CreateObject<Employee>();
			JsonParser.ParseJObject<Employee>(jElement, employee, objectSpace);
			return Ok(employee);
		}
		public void Dispose() {
			objectSpace?.Dispose();
			securityProvider?.Dispose();
		}
	}
}
