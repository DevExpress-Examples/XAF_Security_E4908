using Blazor.ServerSide.Helpers;
using DatabaseUpdater;
using DevExpress.ExpressApp.Xpo;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseDemoData(this IApplicationBuilder app, string connectionString) {
            
            using (var objectSpaceProvider = new XPObjectSpaceProvider(connectionString)) {
                using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                    new Updater(objectSpace).UpdateDatabase();
                }
            }
            return app;
        }
    }
}
