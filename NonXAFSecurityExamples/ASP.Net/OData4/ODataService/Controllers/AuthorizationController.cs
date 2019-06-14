using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNet.OData;
using ODataService.Helpers;

using System.Web.Mvc;
using XafSolution.Module.BusinessObjects;

namespace ODataService.Controllers {
	public class AuthorizationController : ODataController {
		[HttpPost]
		public ActionResult Post(AuthorizationData data) {
			RegisterEntities();
			string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			AuthenticationStandard auth = new AuthenticationStandard();
			SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
			SecuredObjectSpaceProvider osProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
			IObjectSpace nonSecuredObjectSpace = osProvider.CreateNonsecuredObjectSpace();
			auth.SetLogonParameters(new AuthenticationStandardLogonParameters(data.UserName, data.Password));
			security.Logon(nonSecuredObjectSpace);
			ConnectionHelper.security = security;
			ConnectionHelper.ObjectSpace = osProvider.CreateObjectSpace();
			return new EmptyResult();
		}

		private static void RegisterEntities() {
			XpoTypesInfoHelper.GetXpoTypeInfoSource();
			XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
			XafTypesInfo.Instance.RegisterEntity(typeof(Person));
			XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
			XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
		}
	}
}