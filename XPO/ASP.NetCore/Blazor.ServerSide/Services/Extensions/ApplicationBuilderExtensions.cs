using DatabaseUpdater;
using DevExpress.ExpressApp.Services;
using SecutirySharedLibrary.Services;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData(this WebApplication app) {
            using (var scope = app.Services.CreateScope()) {
                ObjectSpaceFactoryBase objectSpaceFactory = (ObjectSpaceFactoryBase)scope.ServiceProvider.GetRequiredService<IObjectSpaceFactory>();
                using (var objectSpace = objectSpaceFactory.CreateUpdatingObjectSpace(true)) {
                    new Updater(objectSpace).UpdateDatabase();
                }
            }
            return app;
        }
    }
}
