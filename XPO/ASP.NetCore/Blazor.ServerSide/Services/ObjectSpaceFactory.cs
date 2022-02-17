using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using SecutirySharedLibrary.Services;

namespace Blazor.ServerSide.Services {
    public class ObjectSpaceFactory : ObjectSpaceFactoryBase {
        readonly IXpoDataStoreProvider xpoDataStoreProvider;
        readonly ITypesInfo typesInfo;
        public ObjectSpaceFactory(ISecurityStrategyBase security, IXpoDataStoreProvider xpoDataStoreProvider, PrincipalAuthenticationService xafSecurityLogin, ITypesInfo typesInfo) : base(security, xafSecurityLogin) {
            this.typesInfo = typesInfo;
            this.xpoDataStoreProvider = xpoDataStoreProvider;
        }

        protected override IObjectSpaceProvider CreateObjectSpaceProvider(ISecurityStrategyBase security) {
            SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)security, xpoDataStoreProvider, typesInfo, null, true);
            return objectSpaceProvider;
        }
    }
}
