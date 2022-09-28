using System;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using XAFSecurityBenchmark.Models.EFCore;
using XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater;
using XAFSecurityBenchmark.PerformanceTests.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests {
    public class EFCoreTestProviderWithSecurity : TestSecuredProviderBase<CustomPermissionPolicyUser, PermissionPolicyRole,
        Contact, DemoTask, Department> {

        static EFCoreTestProviderWithSecurity() {
            EFCoreDBUpdater.InitializeInstance(nameof(CustomPermissionPolicyUser.ID));
        }

        protected override IObjectSpaceProvider CreateUpdatingObjectSpaceProvider() => new EFCoreObjectSpaceProvider<EFCoreContext>((EFCoreDatabaseProviderHandler<EFCoreContext>)null);
        protected override IObjectSpaceProvider CreateSecuredObjectSpaceProvider(ISelectDataSecurityProvider selectDataSecurityProvider) =>
            new SecuredEFCoreObjectSpaceProvider<EFCoreContext>(selectDataSecurityProvider, (optionsBuilder, connectionString) => {
                optionsBuilder
                    .UseSqlServer(TestSetConfig.EFCoreConnectionStrings)
                    .UseLazyLoadingProxies()
                    .UseChangeTrackingProxies();
            });

        protected override ITransactionHelper CreateObjectHelper(IObjectSpace objectSpace) => new EFCoreSecuredObjectHelper(objectSpace);
        protected override IDBUpdater DBUpdater => EFCoreDBUpdater.Instance;

        public override string ToString() {
            return "EF Core 5 (Security)";
        }
    }
}
