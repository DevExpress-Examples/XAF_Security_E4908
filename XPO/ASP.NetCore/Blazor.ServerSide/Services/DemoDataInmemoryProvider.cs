using DevExpress.ExpressApp.Blazor.DemoServices;

namespace Blazor.ServerSide.Services {
    public class DemoDataInMemoryProvider {
        private static AsyncLocal<string> ambienceDemoDataKey = new AsyncLocal<string>();

        public DemoDataInMemoryProvider(IDemoDataStoreProvider demoDataStoreProvider) {
            if (ambienceDemoDataKey.Value is null && demoDataStoreProvider.DataStoreId is not null) {
                ambienceDemoDataKey.Value = demoDataStoreProvider.DataStoreId;
            } else if (demoDataStoreProvider.DataStoreId is null && ambienceDemoDataKey.Value is not null) {
                demoDataStoreProvider.DataStoreId = ambienceDemoDataKey.Value;
            }
        }
    }
}
