using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Task = System.Threading.Tasks.Task;

namespace BlazorServerSide {
	public class EmployeeService {
		public IObjectSpace securedObjectSpace;
		public Task<IList<Employee>> Get() {
			var query = securedObjectSpace.GetObjects<Employee>();
			return Task.FromResult(query);
		}
	}
}
