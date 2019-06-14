using System;
using System.Collections.Concurrent;
using System.Web.Http;
using System.Web.Http.Validation;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using ODataService.Helpers;
using XafSolution.Module.BusinessObjects;

namespace ODataService {
	public class WebApiApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			RegisterEntities();

			string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			AuthenticationStandard auth = new AuthenticationStandard();
			SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
			SecuredObjectSpaceProvider osProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
			IObjectSpace nonSecuredObjectSpace = osProvider.CreateNonsecuredObjectSpace();
			auth.SetLogonParameters(new AuthenticationStandardLogonParameters("User", ""));
			security.Logon(nonSecuredObjectSpace);
			ConnectionHelper.security = security;
			ConnectionHelper.ObjectSpace = osProvider.CreateObjectSpace();

			GlobalConfiguration.Configuration.Services.Replace(typeof(IBodyModelValidator), new CustomBodyModelValidator());
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}

		public class CustomBodyModelValidator : DefaultBodyModelValidator {
			readonly ConcurrentDictionary<Type, bool> persistentTypes = new ConcurrentDictionary<Type, bool>();
			public override bool ShouldValidateType(Type type) {
				return persistentTypes.GetOrAdd(type, t => !typeof(IXPSimpleObject).IsAssignableFrom(t));
			}
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
