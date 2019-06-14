using DevExpress.ExpressApp.Security;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ODataService.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XafSolution.Module.BusinessObjects;



namespace ODataService.Controllers {
	public class EmployeesController :  ODataController {
		//[EnableQuery]
		//public IQueryable<Employee> Get() {
		//	//var x = ConnectionHelper.ObjectSpace.GetObjects<Employee>().AsQueryable();
		//	//var y = from e in x
		//	//		select new {
		//	//			e.FullName,
		//	//			Department = "123"
		//	//		};

		//	return  ConnectionHelper.ObjectSpace.GetObjects<Employee>().AsQueryable();
		//}
		[EnableQuery]
		public IQueryable<PrintingData> Get() {
			var employeeList = ConnectionHelper.ObjectSpace.GetObjects<Employee>().AsQueryable();
			var list = new List<PrintingData>();
			foreach(var employee in employeeList) {
				string name = employee.FullName;
				string department = "Protected content";
				if(ConnectionHelper.security.IsGranted(new PermissionRequest(ConnectionHelper.ObjectSpace, typeof(Employee), SecurityOperations.Read, employee, "Department"))) {
					department = employee.Department.Title;
				}
				list.Add(new PrintingData(name, department));
			}
			return list.AsQueryable();
		}
	}
}