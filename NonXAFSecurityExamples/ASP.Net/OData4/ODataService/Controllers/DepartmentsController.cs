using DevExpress.Xpo;
using Microsoft.AspNet.OData;
using ODataService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XafSolution.Module.BusinessObjects;

namespace ODataService.Controllers {
	public class DepartmentsController : ODataController {
		[EnableQuery]
		public IQueryable<Department> Get() {
			return ConnectionHelper.ObjectSpace.GetObjects<Department>().AsQueryable();
		}
	}
}