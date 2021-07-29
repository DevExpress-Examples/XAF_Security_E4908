using System;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using System.Text.Json;
using DevExtreme.OData.EFCore;
using BusinessObjectsLibrary.EFCore.BusinessObjects;
using DevExpress.ExpressApp.EFCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
			IQueryable<Employee> employees = objectSpace.GetObjectsQuery<Employee>();
			return Ok(employees);
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
		[HttpPatch]
		public ActionResult Patch(int key, [FromBody] JsonElement serializedProperties) {
			Employee employee = objectSpace.FirstOrDefault<Employee>(e => e.ID == key);
			if(employee != null) {
				JsonParser.ParseJson<Employee>(serializedProperties, employee, objectSpace);
				objectSpace.CommitChanges();
				return Ok(employee);
			}
			return NotFound();
		}
		[HttpPost]
		public ActionResult Post([FromBody] JsonElement serializedProperties) {
			Employee newEmployee = objectSpace.CreateObject<Employee>();
			JsonParser.ParseJson<Employee>(serializedProperties, newEmployee, objectSpace);
			objectSpace.CommitChanges(); 
			return Ok(newEmployee);
		}
		public void Dispose() {
			objectSpace?.Dispose();
			securityProvider?.Dispose();
		}
	}
}
