using System.Linq;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.XPO;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    class XpoObjectHelper : ITransactionHelper {
        UnitOfWork session;
        public XpoObjectHelper(UnitOfWork session) {
            this.session = session;
        }
        public void Dispose() { }
        public void BeginTransaction() { }
        public void SaveChanges() => session.CommitChanges();
        public void EndTransaction() { }

        public IContact CreateContact() => new Contact(session);
        public IDemoTask CreateTask() => new DemoTask(session);
        public IPhoneNumber CreatePhoneNumber(IContact forContact) {
            var phoneNumber = new PhoneNumber(session);
            phoneNumber.Party = (Party)forContact;
            return phoneNumber;
        }
        public IAddress CreateAddress() => new Address(session);
        public ICountry CreateCountry(IAddress forAddress) {
            var country = new Country(session);
            forAddress.Country = country;
            return country;
        }
        public IPosition CreatePosition() => new Position(session);

        public void RemoveAllTestData() {
            //clear links
            session.ExecuteNonQuery($"UPDATE [{GetTableName<Contact>()}] SET {nameof(Contact.Location)} = null");
            session.ExecuteNonQuery($"UPDATE [{GetTableName<Task>()}] SET {nameof(Task.AssignedTo)} = null");
            session.ExecuteNonQuery($"UPDATE [{GetTableName<Address>()}] SET {nameof(Address.Country)} = null");
            session.ExecuteNonQuery($"UPDATE [{GetTableName<PhoneNumber>()}] SET {nameof(PhoneNumber.Party)} = null");

            //clear intermediate tables
            session.ExecuteNonQuery($"DELETE FROM [DemoTaskTasks_ContactContacts]");
            session.ExecuteNonQuery($"DELETE FROM [PositionPositions_DepartmentDepartments]");


            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Location>()}]");

            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Contact>()}]");
            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Person>()}]");
            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Party>()}]");

            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Country>()}]");
            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Address>()}]");

            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<PhoneNumber>()}]");
            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Position>()}]");

            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<DemoTask>()}]");
            session.ExecuteNonQuery($"DELETE FROM [{GetTableName<Task>()}]");
        }
        public void UpdateQueryOptimizationStatistics() {
            //session.ExecuteNonQuery("UPDATE STATISTICS[DemoTaskTasks_ContactContacts]");
            //session.ExecuteNonQuery($"UPDATE STATISTICS[{GetTableName<Party>()}]");
            //session.ExecuteNonQuery($"UPDATE STATISTICS[{GetTableName<Task>()}]");
            //session.ExecuteNonQuery($"UPDATE STATISTICS[{GetTableName<Department>()}]");
            //session.ExecuteNonQuery($"UPDATE STATISTICS[{GetTableName<Position>()}]");
            //session.ExecuteNonQuery($"UPDATE STATISTICS[{GetTableName<PermissionPolicyUser>()}]");
            //session.ExecuteNonQuery($"UPDATE STATISTICS[{GetTableName<CustomPermissionPolicyUser>()}]");

            //UPDATE STATISTICS[DemoTaskTasks_ContactContacts]
            //UPDATE STATISTICS[Party]
            //UPDATE STATISTICS[Task]
            //UPDATE STATISTICS[Department]
            //UPDATE STATISTICS[Position]
            //UPDATE STATISTICS[PermissionPolicyUser]
            //UPDATE STATISTICS[CustomPermissionPolicyUser]
        }

        public ICustomPermissionPolicyUser GetSecurityUser(string userName) => session.Query<CustomPermissionPolicyUser>().Where(user => user.UserName == userName).FirstOrDefault();

        string GetTableName<T>() => session.GetClassInfo(typeof(T)).TableName;
    }
}
