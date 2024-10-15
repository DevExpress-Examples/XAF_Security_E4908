using BusinessObjectsLibrary.BusinessObjects;
using DatabaseUpdater;
using DevExpress.ExpressApp;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData(this WebApplication app) {
            using var scope = app.Services.CreateScope();
            var nonSecuredObjectSpaceFactory = scope.ServiceProvider.GetRequiredService<INonSecuredObjectSpaceFactory>();
            using var objectSpace = nonSecuredObjectSpaceFactory
                .CreateNonSecuredObjectSpace<Employee>();
            new Updater(objectSpace).UpdateDatabase();
            return app;
        }
    }
}
