using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Components;

namespace Blazor.ServerSide.Components {
    public class CellEditTemplateBase<T> : ComponentBase {
        [Parameter] public T CurrentObject { get; set; }
        [Parameter] public string PropertyName { get; set; }
        [Inject] private ISecurityProvider SecurityProvider { get; set; }

        private SecurityStrategy Security { get; set; }

        protected string ProtectedContent { get; } = "*******";
        protected bool CanWrite => CurrentObject is null ? Security.CanWrite(typeof(T), PropertyName) : Security.CanWrite(CurrentObject, PropertyName);
        protected bool CanRead => CurrentObject is null ? Security.CanRead(typeof(T), PropertyName) : Security.CanRead(CurrentObject, PropertyName);

        protected override void OnInitialized() {
            Security = (SecurityStrategy)SecurityProvider.GetSecurity();
            base.OnInitialized();
        }
    }
}
