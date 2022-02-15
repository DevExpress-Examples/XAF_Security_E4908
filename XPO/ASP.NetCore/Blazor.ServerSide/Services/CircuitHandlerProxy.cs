using System;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Blazor.AmbientContext;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Blazor.ServerSide.Services {
    internal class CircuitHandlerProxy : CircuitHandler {
        private readonly IValueManagerStorageContext valueManagerStorageContext;
        public CircuitHandlerProxy(IValueManagerStorageContext valueManagerStorageContext) {
            this.valueManagerStorageContext = valueManagerStorageContext;
        }
        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken) {
            if (valueManagerStorageContext.RunWithStorage(() => ValueManager.GetValueManager<bool>("ApplicationCreationMarker").Value)) {
                valueManagerStorageContext.EnsureStorage();
            }
            return Task.CompletedTask;
        }
        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
    }

    //internal class CircuitHandlerProxy : CircuitHandler {
    //    private readonly IScopedCircuitHandler scopedCircuitHandler;
    //    public CircuitHandlerProxy(IScopedCircuitHandler scopedCircuitHandler) {
    //        this.scopedCircuitHandler = scopedCircuitHandler;
    //    }
    //    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken) {
    //        return scopedCircuitHandler.OnCircuitOpenedAsync(cancellationToken);
    //    }
    //    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken) {
    //        return scopedCircuitHandler.OnConnectionUpAsync(cancellationToken);
    //    }
    //    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken) {
    //        return scopedCircuitHandler.OnCircuitClosedAsync(cancellationToken);
    //    }
    //    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken) {
    //        return scopedCircuitHandler.OnConnectionDownAsync(cancellationToken);
    //    }
    //}
}
