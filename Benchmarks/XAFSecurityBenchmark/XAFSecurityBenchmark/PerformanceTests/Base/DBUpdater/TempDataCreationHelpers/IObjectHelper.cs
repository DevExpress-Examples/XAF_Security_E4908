using System;
using DevExpress.Persistent.Base.General;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    public interface IObjectHelper {
        ICustomPermissionPolicyUser GetSecurityUser(string userName);
        IContact CreateContact();
        IDemoTask CreateTask();
        IPhoneNumber CreatePhoneNumber(IContact forContact);
        IAddress CreateAddress();
        ICountry CreateCountry(IAddress forAddress);
        IPosition CreatePosition();

        void RemoveAllTestData();
        void UpdateQueryOptimizationStatistics();
    }
    public interface ITransactionHelper : IObjectHelper {
        void BeginTransaction();
        void SaveChanges();
        void EndTransaction();
    }
}
