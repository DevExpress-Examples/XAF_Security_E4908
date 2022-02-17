using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Services;

namespace SecutirySharedLibrary.Services {
    public abstract class ObjectSpaceFactoryBase : IDisposable, IObjectSpaceFactory {
        readonly ISecurityStrategyBase security;
        readonly PrincipalAuthenticationService xafSecurityLogin;
        IObjectSpaceProvider? objectSpaceProvider;
        IObjectSpaceProvider? nonSecuredObjectSpaceProvider;

        public ObjectSpaceFactoryBase(ISecurityStrategyBase security, PrincipalAuthenticationService xafSecurityLogin) {
            this.security = (SecurityStrategyComplex)security;
            this.xafSecurityLogin = xafSecurityLogin;
        }
        protected abstract IObjectSpaceProvider CreateObjectSpaceProvider(ISecurityStrategyBase security);

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
