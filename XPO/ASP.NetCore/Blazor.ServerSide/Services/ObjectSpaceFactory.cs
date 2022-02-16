using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Services;
using DevExpress.ExpressApp.Xpo;

namespace Blazor.ServerSide.Services {
    public class ObjectSpaceFactory : IDisposable, IObjectSpaceFactory {
        readonly ISecurityStrategyBase security;
        readonly ITypesInfo typesInfo;
        readonly IXpoDataStoreProvider xpoDataStoreProvider;
        readonly PrincipalAuthenticationService xafSecurityLogin;
        IObjectSpaceProvider objectSpaceProvider;
        IObjectSpaceProvider nonSecuredObjectSpaceProvider;
        
        public ObjectSpaceFactory(ISecurityStrategyBase security, IXpoDataStoreProvider xpoDataStoreProvider, PrincipalAuthenticationService xafSecurityLogin, ITypesInfo typesInfo) {
            this.security = (SecurityStrategyComplex)security;
            this.xpoDataStoreProvider = xpoDataStoreProvider;
            this.xafSecurityLogin = xafSecurityLogin;
            this.typesInfo = typesInfo;
        }


        IObjectSpaceProvider CreateObjectSpaceProvider(ISecurityStrategyBase security) { 
            SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)security, xpoDataStoreProvider, typesInfo, null, true);
            return objectSpaceProvider;
        }

        IObjectSpaceProvider GetObjectSpaceProvider(Type entityType) {
            xafSecurityLogin.XafSecurityEnsureLogon(this);
            if (objectSpaceProvider == null) {
                objectSpaceProvider = CreateObjectSpaceProvider(security);
            }
            return objectSpaceProvider;
        }

        INonsecuredObjectSpaceProvider GetNonSecuredObjectSpaceProvider(Type entityType) {
            if (nonSecuredObjectSpaceProvider is null)
                nonSecuredObjectSpaceProvider = CreateObjectSpaceProvider(security);
            return (INonsecuredObjectSpaceProvider)nonSecuredObjectSpaceProvider;
        }
        IObjectSpace IObjectSpaceFactory.CreateObjectSpace(Type entityType) => GetObjectSpaceProvider(entityType).CreateObjectSpace();

        IObjectSpace IObjectSpaceFactory.CreateNonSecuredObjectSpace(Type entityType) => GetNonSecuredObjectSpaceProvider(entityType).CreateNonsecuredObjectSpace();

        public IObjectSpace CreateUpdatingObjectSpace(bool allowUpdateSchema) => ((IObjectSpaceProvider)GetNonSecuredObjectSpaceProvider(security.UserType)).CreateUpdatingObjectSpace(allowUpdateSchema);

        void IDisposable.Dispose() {
            if (objectSpaceProvider is IDisposable disposable) {
                disposable.Dispose();
            }

            objectSpaceProvider = null;

            if (nonSecuredObjectSpaceProvider is IDisposable disposable1) {
                disposable1.Dispose();
            }

            nonSecuredObjectSpaceProvider = null;
        }
    }
}
