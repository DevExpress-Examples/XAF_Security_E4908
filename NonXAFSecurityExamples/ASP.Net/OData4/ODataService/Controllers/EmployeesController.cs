using DevExpress.ExpressApp;
using DevExpress.Xpo;
using Microsoft.AspNet.OData;
using ODataService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XafSolution.Module.BusinessObjects;

namespace ODataService.Controllers {
	public class EmployeesController : ODataController {
		[EnableQuery]
		public IQueryable<Employee> Get() {
			return ConnectionHelper.ObjectSpace.GetObjects<Employee>().AsQueryable();
		}
	}
}