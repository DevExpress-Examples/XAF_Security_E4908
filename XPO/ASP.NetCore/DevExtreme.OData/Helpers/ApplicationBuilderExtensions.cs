using DevExpress.ExpressApp.Xpo;
using DatabaseUpdater;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData(this WebApplication app) {
            IXpoDataStoreProvider xpoDataStoreProvider = app.Services.GetRequiredService<IXpoDataStoreProvider>();
            ITypesInfo typesInfo = app.Services.GetRequiredService<ITypesInfo>();
            using (var objectSpaceProvider = new XPObjectSpaceProvider(xpoDataStoreProvider, typesInfo, null)) {
                using(var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                    new Updater(objectSpace).UpdateDatabase();
                }
            }
            return app;
        }
    }
}
