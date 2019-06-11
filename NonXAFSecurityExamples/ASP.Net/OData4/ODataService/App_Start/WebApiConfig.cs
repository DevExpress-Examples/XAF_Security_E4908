using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using DevExpress.Persistent.BaseImpl;
using XafSolution.Module.BusinessObjects;

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
			ODataModelBuilder builder = new ODataConventionModelBuilder();
			var parties = builder.EntitySet<Party>("Parties");
			var employees = builder.EntitySet<Employee>("Employees");

			parties.EntityType.HasKey(t => t.Oid);
			employees.EntityType.HasKey(t => t.Oid);
			employees.EntityType.ComplexProperty(t => t.Department);
			return builder;
		}
	}
}
