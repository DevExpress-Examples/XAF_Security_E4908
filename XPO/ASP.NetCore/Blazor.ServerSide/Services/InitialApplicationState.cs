using Blazor.ServerSide.Helpers;
using DatabaseUpdater;
using DevExpress.ExpressApp.Xpo;

namespace Blazor.ServerSide.Services {
    public class InitialApplicationState {
        public string DemoDataStoreId { get; set; }
    }
    public class InitialApplicationStateProvider {
        private readonly DevExpress.ExpressApp.Blazor.DemoServices.IDemoDataStoreProvider demoDataStoreProvider;
        public InitialApplicationStateProvider(DevExpress.ExpressApp.Blazor.DemoServices.IDemoDataStoreProvider demoDataStoreProvider) {
            this.demoDataStoreProvider = demoDataStoreProvider;
        }
        public InitialApplicationState Create() {
            InitialApplicationState state = new InitialApplicationState();
            state.DemoDataStoreId = demoDataStoreProvider.DataStoreId;
            return state;
        }
    }
    public class InitialApplicationStateInitializer {
        private readonly DevExpress.ExpressApp.Blazor.DemoServices.IDemoDataStoreProvider demoDataStoreProvider;
        private readonly IConfiguration config;
        public InitialApplicationStateInitializer(DevExpress.ExpressApp.Blazor.DemoServices.IDemoDataStoreProvider demoDataStoreProvider, IConfiguration config) {
            this.demoDataStoreProvider = demoDataStoreProvider;
            this.config = config;
        }

        bool isCompatibilityChecked = false;
        public void Initialize(InitialApplicationState state) {
            demoDataStoreProvider.DataStoreId = state.DemoDataStoreId;
            if (!isCompatibilityChecked) {
                using (var objectSpaceProvider = new XPObjectSpaceProvider(config.GetConnectionString("ConnectionString"))) {
                    SecurityProvider.RegisterEntities(objectSpaceProvider);
                    using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                        new Updater(objectSpace).UpdateDatabase();
                    }
                }
            }
            isCompatibilityChecked = true;
        }
    }
}
