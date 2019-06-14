using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ODataService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XafSolution.Module.BusinessObjects;

namespace ODataService.Controllers {
	public class DepartmentsController : ODataController {
		[EnableQuery]
		public IQueryable<Department> Get() {
			return ConnectionHelper.ObjectSpace.GetObjects<Department>().AsQueryable();
		}
	}
}