using Blazor.ServerSide.Services;
using DatabaseUpdater;
using DevExpress.ExpressApp.Services;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData(this WebApplication app) {
            using (var scope = app.Services.CreateScope()) {
                ObjectSpaceFactory objectSpaceFactory = (ObjectSpaceFactory)scope.ServiceProvider.GetRequiredService<IObjectSpaceFactory>();
                using (var objectSpace = objectSpaceFactory.CreateUpdatingObjectSpace(true)) {
                    new Updater(objectSpace).UpdateDatabase();
                }
            }
            return app;
        }
    }
}
