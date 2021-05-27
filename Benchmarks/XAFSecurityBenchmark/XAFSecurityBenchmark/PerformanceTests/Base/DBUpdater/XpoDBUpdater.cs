using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using XAFSecurityBenchmark.Models.XPO;
using XAFSecurityBenchmark.PerformanceTests.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    public class XpoDBUpdater : DBUpdaterBase<
                CustomPermissionPolicyUser, PermissionPolicyRole, PermissionPolicyTypePermissionObject, PermissionPolicyMemberPermissionsObject, PermissionPolicyObjectPermissionsObject, PermissionPolicyNavigationPermissionObject,
                Contact, DemoTask, Department> {

        private static IDBUpdater _instance = null;
        public static IDBUpdater Instance => _instance;
        public static void InitializeInstance(string keyPropertyName) {
            if(_instance == null) {
                _instance = new XpoDBUpdater(keyPropertyName);
            }
        }

        protected XpoDBUpdater(string keyPropertyName) : base(keyPropertyName) { }

        protected override IObjectSpaceProvider CreateUpdatingObjectSpaceProvider() => new XPObjectSpaceProvider(TestSetConfig.XPOConnectionStrings, null);

        protected override void RegisterEntities() {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            base.RegisterEntities();
        }

        protected override ITransactionHelper CreateUpdatingObjectHelper(IObjectSpace updatingObjectSpace) => new XpoSecuredObjectHelper(updatingObjectSpace);
    }
}
