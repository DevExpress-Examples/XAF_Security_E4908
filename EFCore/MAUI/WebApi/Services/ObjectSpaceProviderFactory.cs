using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using WebAPI.BusinessObjects;

namespace WebAPI.Services;

public sealed class ObjectSpaceProviderFactory : IObjectSpaceProviderFactory {
    readonly ISecurityStrategyBase _security;
    readonly ITypesInfo _typesInfo;
    readonly IXafDbContextFactory<WebAPIEFCoreDbContext> _dbFactory;

    public ObjectSpaceProviderFactory(ISecurityStrategyBase security, ITypesInfo typesInfo, IXafDbContextFactory<WebAPIEFCoreDbContext> dbFactory) {
        _security = security;
        _typesInfo = typesInfo;
        _dbFactory = dbFactory;
    }

    IEnumerable<IObjectSpaceProvider> IObjectSpaceProviderFactory.CreateObjectSpaceProviders() {
        yield return new SecuredEFCoreObjectSpaceProvider<WebAPIEFCoreDbContext>((ISelectDataSecurityProvider)_security, _dbFactory, _typesInfo);
        yield return new NonPersistentObjectSpaceProvider(_typesInfo, null);
    }
}
