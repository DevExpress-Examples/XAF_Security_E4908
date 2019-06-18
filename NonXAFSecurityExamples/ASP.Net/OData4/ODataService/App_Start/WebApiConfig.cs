using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using DevExpress.Persistent.BaseImpl;
using XafSolution.Module.BusinessObjects;
using System.Collections.Generic;

namespace ODataService {
	public static class WebApiConfig {
		public static void Register(HttpConfiguration config) {
			config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
			ODataModelBuilder modelBuilder = CreateODataModelBuilder();

			ODataBatchHandler batchHandler =
				new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer);

			config.MapODataServiceRoute(
				routeName: "ODataRoute",
				routePrefix: null,
				model: modelBuilder.GetEdmModel(),
				batchHandler: batchHandler);
		}

		static ODataModelBuilder CreateODataModelBuilder() {
			//ODataModelBuilder builder = new ODataConventionModelBuilder();
			//var parties = builder.EntitySet<Party>("Parties");
			//var employees = builder.EntitySet<Employee>("Employees");
			////var departments = builder.EntitySet<Department>("Departments");
			//var employeeTypeConfiguration = builder.StructuralTypes.First(t => t.ClrType == typeof(Employee));
			//employeeTypeConfiguration.AddProperty(typeof(Employee).GetProperty("FullName"));

			//parties.EntityType.HasKey(t => t.Oid);
			//employees.EntityType.HasKey(t => t.Oid);

			//employees.EntityType.ComplexProperty(t => t.Department);
			//return builder;

			ODataModelBuilder builder = new ODataConventionModelBuilder();
			EntitySetConfiguration<Employee> employees = builder.EntitySet<Employee>("Employees");
			EntitySetConfiguration<Party> parties = builder.EntitySet<Party>("Parties");
			EntitySetConfiguration<AuthorizationData> authorizationData = builder.EntitySet<AuthorizationData>("Authorization");
			EntitySetConfiguration<Request> requests = builder.EntitySet<Request>("Requests");
			EntitySetConfiguration<Test> test = builder.EntitySet<Test>("Test");
			test.EntityType.HasKey(t => t.Oid);

			employees.EntityType.HasKey(t => t.Oid);
			authorizationData.EntityType.HasKey(t => t.UserName);
			parties.EntityType.HasKey(t => t.Oid);
			requests.EntityType.HasKey(t => t.Oid);

			employees.EntityType.ComplexProperty(t => t.Department);

			var employeeTypeConfiguration = builder.StructuralTypes.First(t => t.ClrType == typeof(Employee));
			employeeTypeConfiguration.AddProperty(typeof(Employee).GetProperty("FullName"));



			return builder;
		}
	}
}
