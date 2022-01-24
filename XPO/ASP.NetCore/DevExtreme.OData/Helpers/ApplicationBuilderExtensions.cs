using DevExpress.ExpressApp.Xpo;
using DatabaseUpdater;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData(this WebApplication app) {
            XpoDataStoreProviderService xpoDataStoreProviderService = app.Services.GetRequiredService<XpoDataStoreProviderService>();
            TypesInfo typesInfo = app.Services.GetRequiredService<TypesInfo>();
            using (var objectSpaceProvider = new XPObjectSpaceProvider(xpoDataStoreProviderService.GetDataStoreProvider(), typesInfo, null)) {
                using(var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                    new Updater(objectSpace).UpdateDatabase();
                }
            }
            return app;
        }
    }
}
