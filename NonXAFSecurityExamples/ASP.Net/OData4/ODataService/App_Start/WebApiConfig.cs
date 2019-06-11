using DevExpress.Persistent.BaseImpl;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using XafSolution.Module.BusinessObjects;

namespace ODataService {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            ODataModelBuilder modelBuilder = CreateODataModelBuilder();

            ODataBatchHandler batchHandler =
                new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer);

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: modelBuilder.GetEdmModel(),
                batchHandler: batchHandler);
        }
        private static ODataModelBuilder CreateODataModelBuilder() {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            var parties = builder.EntitySet<Party>("Parties");
            var employees = builder.EntitySet<Employee>("Employees");
            var departments = builder.EntitySet<Department>("Departments");

            parties.EntityType.HasKey(t => t.Oid);
            employees.EntityType.HasKey(t => t.Oid);
            departments.EntityType.HasKey(t => t.Oid);

            builder.Action("InitializeDatabase");
            return builder;
        }
    }
}
