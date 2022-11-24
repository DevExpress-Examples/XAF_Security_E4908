using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using XAFSecurityBenchmark.Models.EFCore;
using XAFSecurityBenchmark.PerformanceTests.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    public class EFCoreDBUpdater : DBUpdaterBase<
                CustomPermissionPolicyUser, PermissionPolicyRole, PermissionPolicyTypePermissionObject, PermissionPolicyMemberPermissionsObject, PermissionPolicyObjectPermissionsObject, PermissionPolicyNavigationPermissionObject,
                Contact, DemoTask, Department> {

        private static IDBUpdater _instance = null;
        public static IDBUpdater Instance => _instance;
        public static void InitializeInstance(string keyPropertyName) {
            if (_instance == null) {
                _instance = new EFCoreDBUpdater(keyPropertyName);
            }
        }

        protected EFCoreDBUpdater(string keyPropertyName) : base(keyPropertyName) { }

        protected override IObjectSpaceProvider CreateUpdatingObjectSpaceProvider() => new EFCoreObjectSpaceProvider<EFCoreContext>((EFCoreDatabaseProviderHandler<EFCoreContext>)null);
        protected override ITransactionHelper CreateUpdatingObjectHelper(IObjectSpace updatingObjectSpace) => new EFCoreObjectHelper(() => new EFCoreContext());
    }
}
