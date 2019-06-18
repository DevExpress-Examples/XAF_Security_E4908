using DevExpress.ExpressApp.Security;
using Microsoft.AspNet.OData;
using ODataService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XafSolution.Module.BusinessObjects;

namespace ODataService.Controllers {
	public class TestController : ODataController {
		[EnableQuery]
		public IQueryable<Test> Get() {
			List<Test> testList = new List<Test>();
			IList<Employee> employeeList = ConnectionHelper.ObjectSpace.GetObjects<Employee>();
			
			List<string> memberList = new List<string>();   //хардкод вместо typeInfo
			memberList.Add("Department");

			foreach(Employee employee in employeeList) {
				foreach(string member in memberList) {
					bool permission = ConnectionHelper.security.IsGranted(new PermissionRequest(ConnectionHelper.ObjectSpace, typeof(Employee), SecurityOperations.Read, employee, member));
					Test test = new Test();
					test.Oid = employee.Oid;
					test.Data.Add("Employee", employee);
					test.Data.Add(member, permission);
					testList.Add(test);
				}
			}
			return testList.AsQueryable();
		}
	}
}