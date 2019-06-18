using DevExpress.ExpressApp.Security;
using Microsoft.AspNet.OData;
using ODataService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XafSolution.Module.BusinessObjects;

namespace ODataService.Controllers {
	public class RequestsController : ODataController {
		[EnableQuery]
		public IQueryable<Request> Get() {
			List<Request> requestList = new List<Request>();
			IList<Employee> employeeList = ConnectionHelper.ObjectSpace.GetObjects<Employee>();
			List<string> memberList = new List<string>();   //хардкод вместо typeInfo
			memberList.Add("Department");
			foreach(Employee employee in employeeList) {
				foreach(string member in memberList) {
					bool permission = ConnectionHelper.security.IsGranted(new PermissionRequest(ConnectionHelper.ObjectSpace, typeof(Employee), SecurityOperations.Read, employee, member));
					requestList.Add(new Request(employee.Oid, member, permission));
				}
			}
			return requestList.AsQueryable();
		}
	}
}