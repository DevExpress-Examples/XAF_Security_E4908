using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo.Metadata;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using XafSolution.Module.BusinessObjects;

namespace ASPNETCoreODataService.Controllers {
	public class EmployeesController : SecuredController {
		public EmployeesController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper, IHttpContextAccessor contextAccessor)
			: base(xpoDataStoreProviderService, config, securityHelper, contextAccessor) { }

		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			IQueryable<Employee> employees = ObjectSpace.GetObjectsQuery<Employee>();
			return Ok(employees);
		}

		[HttpDelete]
		public ActionResult Delete(Guid key) {
			Employee existing = ObjectSpace.GetObjectByKey<Employee>(key);
			if(existing != null) {
			    ObjectSpace.Delete(existing);
			    ObjectSpace.CommitChanges();
			    return NoContent();
			}
            return NotFound(); 
		}

		[HttpPatch]
		public ActionResult Patch(Guid key, [FromBody]JObject jObject) {
			Employee employee = ObjectSpace.FindObject<Employee>(new BinaryOperator(nameof(Employee.Oid), key));
			if(employee != null) {
				JsonParser.ParseJObject<Employee>(jObject, employee, ObjectSpace);
				return Ok(employee);
			}
			return NotFound();
		}

		[HttpPost]
		public ActionResult Post([FromBody]JObject jObject) {
			Employee employee = ObjectSpace.CreateObject<Employee>();
			JsonParser.ParseJObject<Employee>(jObject, employee, ObjectSpace);
			return Ok(employee);
		}
	}
}
