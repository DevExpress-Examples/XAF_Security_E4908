using DevExpress.ExpressApp.Blazor.AmbientContext;
using DevExpress.ExpressApp.Blazor.Services;

namespace Blazor.ServerSide.Services {
    internal sealed class ValueManagerContextActivator : IValueManagerStorageContainerInitializer {
        void IValueManagerStorageContainerInitializer.Initialize() {
            if (!ValueManagerContext.IsActive) {
                ValueManagerContext.Activate();
            }
        }
    }
}
