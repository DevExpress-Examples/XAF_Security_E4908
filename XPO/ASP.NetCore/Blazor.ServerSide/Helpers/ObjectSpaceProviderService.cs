using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.WebApi.Services;

namespace Blazor.ServerSide.Helpers {
    public interface IObjectSpaceProviderService : IDisposable {
        IObjectSpace CreateObjectSpace(Type entityType);
        IObjectSpace CreateObjectSpace<T>();
        IObjectSpace CreateNonSecuredObjectSpace(Type entityType);
        IObjectSpace CreateNonSecuredObjectSpace<T>();
    }

    public class ObjectSpaceProviderService : IObjectSpaceProviderService, IWebApiObjectSpaceProvider, ITypesInfoProvider2 {
        readonly ISecurityStrategyBase security;
        readonly XpoDataStoreProviderService xpoDataStoreProviderService;
        readonly XafSecurityLoginService xafSecurityLoginService;
        IObjectSpaceProvider objectSpaceProvider;
        IObjectSpaceProvider nonSecuredObjectSpaceProvider;
        IObjectSpace loginObjectSpace;

        public ObjectSpaceProviderService(ISecurityStrategyBase security, XpoDataStoreProviderService xpoDataStoreProviderService, XafSecurityLoginService xafSecurityLoginService) {
            this.security = (SecurityStrategyComplex)security;
            this.xpoDataStoreProviderService = xpoDataStoreProviderService;
            this.xafSecurityLoginService = xafSecurityLoginService;
        }

        IObjectSpaceProvider CreateObjectSpaceProvider(ISecurityStrategyBase security) {
            SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)security, xpoDataStoreProviderService.GetDataStoreProvider(), true);
            objectSpaceProvider.RegisterDemoEntities();
            return objectSpaceProvider;
        }

        IObjectSpaceProvider GetObjectSpaceProvider(Type entityType) {
            if (objectSpaceProvider == null) {
                if (!xafSecurityLoginService.IsLoginOperationExecuted) { 
                    loginObjectSpace = ((INonsecuredObjectSpaceProvider)GetNonSecuredObjectSpaceProvider(security.UserType)).CreateNonsecuredObjectSpace();
                    if (!xafSecurityLoginService.TryLogin(loginObjectSpace)) {
                        //TODO test that behaviour
                        throw new Exception("Authentication failed");
                    }
                }
                objectSpaceProvider = CreateObjectSpaceProvider(security);
            }
            return objectSpaceProvider;
        }

        IObjectSpaceProvider GetNonSecuredObjectSpaceProvider(Type entityType) {
            if (nonSecuredObjectSpaceProvider is null)
                nonSecuredObjectSpaceProvider = CreateObjectSpaceProvider(security);
            return nonSecuredObjectSpaceProvider;
        }

        IObjectSpace CreateObjectSpace(Type entityType) => GetObjectSpaceProvider(entityType).CreateObjectSpace();
        IObjectSpace CreateNonSecuredObjectSpace(Type entityType) => GetNonSecuredObjectSpaceProvider(entityType).CreateObjectSpace();

        IObjectSpace IObjectSpaceProviderService.CreateObjectSpace(Type entityType) => CreateObjectSpace(entityType);

        IObjectSpace IObjectSpaceProviderService.CreateNonSecuredObjectSpace(Type entityType) => CreateNonSecuredObjectSpace(entityType);

        IObjectSpace IObjectSpaceProviderService.CreateObjectSpace<T>() => CreateObjectSpace(typeof(T));

        IObjectSpace IObjectSpaceProviderService.CreateNonSecuredObjectSpace<T>() => CreateNonSecuredObjectSpace(typeof(T));

        IObjectSpace IWebApiObjectSpaceProvider.CreateObjectSpace(Type objectType) => CreateObjectSpace(objectType);

        IObjectSpace IWebApiObjectSpaceProvider.CreateNonSecuredObjectSpace(Type objectType) => CreateNonSecuredObjectSpace(objectType);

        ITypesInfo ITypesInfoProvider2.GetTypesInfo() => GetNonSecuredObjectSpaceProvider(typeof(Employee)).TypesInfo;

        void IDisposable.Dispose() {
            if (objectSpaceProvider is IDisposable disposable) {
                disposable.Dispose();
            }

            objectSpaceProvider = null;

            if (nonSecuredObjectSpaceProvider is IDisposable disposable1)
                disposable1.Dispose();

            nonSecuredObjectSpaceProvider = null;

            loginObjectSpace?.Dispose();
            loginObjectSpace = null;
        }
    }
}
