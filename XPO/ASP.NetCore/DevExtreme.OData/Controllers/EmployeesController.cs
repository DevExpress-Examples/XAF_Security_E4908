using System;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using XafSolution.Module.BusinessObjects;
#if NETCOREAPP3_1
using System.Text.Json;
#endif

namespace ASPNETCoreODataService.Controllers {
	[Route("api/[controller]")]
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
#if NETCOREAPP3_1
		public ActionResult Patch(Guid key, [FromBody]JsonElement jElement) {
			JObject jObject = JObject.Parse(jElement.ToString());
#else
		public ActionResult Patch(Guid key, [FromBody]JObject jObject) {
#endif
			Employee employee = objectSpace.FirstOrDefault<Employee>(e => e.Oid == key);
			if(employee != null) {
				JsonParser.ParseJObject<Employee>(jObject, employee, objectSpace);
				return Ok(employee);
			}
			return NotFound();
		}
		[HttpPost]
#if NETCOREAPP3_1
		public ActionResult Post([FromBody]JsonElement jElement) {
			JObject jObject = JObject.Parse(jElement.ToString());
#else
		public ActionResult Post([FromBody]JObject jObject) {
#endif
			Employee employee = objectSpace.CreateObject<Employee>();
			JsonParser.ParseJObject<Employee>(jObject, employee, objectSpace);
			return Ok(employee);
		}
		public void Dispose() {
			objectSpace?.Dispose();
			securityProvider?.Dispose();
		}
	}
}
