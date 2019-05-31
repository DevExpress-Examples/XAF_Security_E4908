using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;
using Task = System.Threading.Tasks.Task;

namespace BlazorServerSide {
	public class EmployeeService {
		IObjectSpace securedObjectSpace;
		SecurityStrategyComplex security;
		public EmployeeService(IObjectSpace securedObjectSpace, SecurityStrategyComplex security) {
			this.securedObjectSpace = securedObjectSpace;
			this.security = security;
		}
		public Task<IList<Employee>> Get() {
			var query = securedObjectSpace.GetObjects<Employee>();
			return Task.FromResult(query);
		}
	}
}
