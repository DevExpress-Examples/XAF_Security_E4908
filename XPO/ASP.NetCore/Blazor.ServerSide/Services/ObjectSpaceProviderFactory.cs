using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;

namespace Blazor.ServerSide.Services {
    public class ObjectSpaceProviderFactory : IObjectSpaceProviderFactory {
        readonly ISecurityStrategyBase security;
        readonly IXpoDataStoreProvider xpoDataStoreProvider;
        readonly ITypesInfo typesInfo;

        readonly IServiceProvider serviceProvider;
        public ObjectSpaceProviderFactory(IServiceProvider serviceProvider, ISecurityStrategyBase security, IXpoDataStoreProvider xpoDataStoreProvider, ITypesInfo typesInfo) {
            this.security = security;
            this.serviceProvider = serviceProvider;
            this.typesInfo = typesInfo;
            this.xpoDataStoreProvider = xpoDataStoreProvider;

        }

        IEnumerable<IObjectSpaceProvider> IObjectSpaceProviderFactory.CreateObjectSpaceProviders() {
            yield return new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)security, xpoDataStoreProvider, typesInfo, null, true);
        }
    }
}
