using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace Blazor.ServerSide.Helpers {
    public static class ObjectSpaceProviderTypesInfoHelper {
        public static void RegisterDemoEntities(this IObjectSpaceProvider objectSpaceProvider) {
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
        }
    }
}
