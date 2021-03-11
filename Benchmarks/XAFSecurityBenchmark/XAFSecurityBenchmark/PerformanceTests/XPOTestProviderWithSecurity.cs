using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.XPO;
using XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater;
using XAFSecurityBenchmark.PerformanceTests.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests {
    public class XPOTestProviderWithSecurity : TestSecuredProviderBase<CustomPermissionPolicyUser, PermissionPolicyRole,
        Contact, DemoTask, Department> {

        static XPOTestProviderWithSecurity() {
            XpoDBUpdater.InitializeInstance(nameof(BaseObject.Oid));
        }

        private string ConnectionString => TestSetConfig.XPOConnectionStrings;

        protected override IObjectSpaceProvider CreateUpdatingObjectSpaceProvider() => new XPObjectSpaceProvider(ConnectionString, null);
        protected override IObjectSpaceProvider CreateSecuredObjectSpaceProvider(ISelectDataSecurityProvider selectDataSecurityProvider) =>
            new SecuredObjectSpaceProvider(selectDataSecurityProvider, ConnectionString, null);

        protected override ITransactionHelper CreateObjectHelper(IObjectSpace objectSpace) => new XpoSecuredObjectHelper(objectSpace);
        protected override IDBUpdater DBUpdater => XpoDBUpdater.Instance;

        public override string ToString() {
            Version version = typeof(IXPObject).Assembly.GetName().Version;
            return "XPO (Security)";
        }
    }
}
