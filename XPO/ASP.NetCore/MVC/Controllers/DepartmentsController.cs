using DevExpress.ExpressApp;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessObjectsLibrary.BusinessObjects;

namespace MvcApplication.Controllers {
	[Authorize]
	[Route("api/[controller]")]
	public class DepartmentsController : Microsoft.AspNetCore.Mvc.Controller {
		SecurityProvider securityProvider;
		IObjectSpace objectSpace;
		public DepartmentsController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
			objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
		}
		[HttpGet]
		public object Get(DataSourceLoadOptions loadOptions) {
			IQueryable<Department>  departments = objectSpace.GetObjectsQuery<Department>();
			return DataSourceLoader.Load(departments, loadOptions);
		}
		protected override void Dispose(bool disposing) {
			if(disposing) {
				objectSpace?.Dispose();
				securityProvider?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
