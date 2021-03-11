using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater;
using XAFSecurityBenchmark.PerformanceTests.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests {

    public abstract class TestProviderBase : IDisposable {
        public static bool CheckResult_ForDebug = true;

        public abstract void InsertEmptyContact(int recordsCount);
        public abstract void InsertContact(int recordsCount);
        public abstract void GetContacts(int recordsCount);
        public abstract void GetTasks(int recordsCount);
        public abstract void UpdateContacts(int recordsCount);
        public abstract void UpdateTasks(int recordsCount);
        
        public abstract void InitSession();
        public abstract void TearDownSession();

        protected abstract IDBUpdater DBUpdater { get; }
        protected abstract ICustomPermissionPolicyUser GetUser();

        protected void CheckCollectionCount(IList collection, int expectedCount) {
            if(CheckResult_ForDebug) {
                CheckCollectionCountCore(expectedCount, collection.Count);
            }
        }
        protected void CheckCollectionCount<T>(IEnumerable<T> collection, int expectedCount) {
            if(CheckResult_ForDebug) {
                CheckCollectionCountCore(expectedCount, collection.Count());
            }
        }
        private void CheckCollectionCountCore(int expected, int actual) {
            if(actual != expected) {
                throw new InvalidOperationException($"The {expected} objects were expected to be taken. The actual value is {actual} in the {this.GetType().Name} test provider.");
            }
        }


        protected void CheckUserData(ICustomPermissionPolicyUser defaultUser) {
            if(CheckResult_ForDebug) {
                if(defaultUser == null) {
                    throw new InvalidOperationException($"The user is not created. The DBUpdater should populate data");
                }
                if(defaultUser.Department == null) {
                    throw new InvalidOperationException($"The user department is not created. The DBUpdater should populate data");
                }
            }
        }

        public virtual void GlobalTestDataSetup(bool fullUpdate) {
            DBUpdater.CheckAndUpdateDB(fullUpdate);
        }
        public void CleanupTestDataSet() {
            DBUpdater.CleanupTestData();
        }
        public virtual void Dispose() {
            TearDownSession();
        }
    }
}
