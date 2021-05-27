using System;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.XPO;
using XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater;
using XAFSecurityBenchmark.PerformanceTests.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests {
    public class XPOPerfTestProvider : TestProviderBase {
        private UnitOfWork session;
        private IDataLayer dataLayer;
        private ITransactionHelper objectHelper;

        static XPOPerfTestProvider() {
            XpoDBUpdater.InitializeInstance(nameof(BaseObject.Oid));
        }

        Expression<Func<Contact, bool>> ContactsFilterPredicate(ICustomPermissionPolicyUser currentUser) =>
            contact => contact.Department == currentUser.Department;
        Expression<Func<DemoTask, bool>> TasksFilterPredicate(ICustomPermissionPolicyUser currentUser) =>
            task => task.Contacts.Any(contact => contact.Department.Users.Any(user => user == currentUser)) || ((Contact)task.AssignedTo).Department == currentUser.Department;

        private IDataLayer GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption) {
            string connstr = TestSetConfig.XPOConnectionStrings;
            IDataLayer dl = XpoDefault.GetDataLayer(connstr, autoCreateOption);
            dl.Dictionary.GetDataStoreSchema(typeof(Contact));
            return dl;
        }
        private UnitOfWork GetUnitOfWork(IDataLayer dataLayer) => new UnitOfWork(dataLayer) { IdentityMapBehavior = IdentityMapBehavior.Strong };
        
        protected override ICustomPermissionPolicyUser GetUser() {
            CustomPermissionPolicyUser currentUser = session.Query<CustomPermissionPolicyUser>().Where(o => o.UserName == TestSetConfig.TestUser).FirstOrDefault();
            CheckUserData(currentUser);
            return currentUser;
        }
        protected override IDBUpdater DBUpdater => XpoDBUpdater.Instance;//new XpoDBUpdater(KeyPropertyName);

        public override void InsertEmptyContact(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            for(int i = 1; i < recordsCount + 1; i++) {
                var contact = new Contact(session) { FirstName = $"Contact{i}" };
                ((IContact)contact).SetDepartment(currentUser.Department);
            }
            session.CommitChanges();
        }
        public override void InsertContact(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            for(int i = 1; i < recordsCount + 1; i++) {
                var contact = new Contact(session);
                objectHelper.FillContact(contact, currentUser.Department, i);
            }
            session.CommitChanges();
        }
        public override void GetContacts(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            CheckUserData(currentUser);
            var q = session.Query<Contact>().Where(ContactsFilterPredicate(currentUser)).Take(recordsCount);
            foreach(var t in q) { }
            CheckCollectionCount(q, recordsCount);
        }
        public override void GetTasks(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            var q = session.Query<DemoTask>().Where(TasksFilterPredicate(currentUser)).Take(recordsCount);
            foreach(var t in q) { }
            CheckCollectionCount(q, recordsCount);
        }
        public override void UpdateContacts(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            session.ExplicitBeginTransaction();
            try {
                foreach(var contact in session.Query<Contact>().Where(ContactsFilterPredicate(currentUser)).Take(recordsCount)) {
                    contact.Anniversary = DateTime.Now;
                }
                session.CommitChanges();
            }
            finally {
                session.ExplicitCommitTransaction();
            }
        }
        public override void UpdateTasks(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            session.ExplicitBeginTransaction();
            try {
                foreach(var task in session.Query<DemoTask>().Where(TasksFilterPredicate(currentUser)).Take(recordsCount)) {
                    task.DueDate = DateTime.Now.AddHours(24).Date;
                }
                session.CommitChanges();
            }
            finally {
                session.ExplicitCommitTransaction();
            }
        }

        public override void InitSession() {
            dataLayer = GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption.SchemaAlreadyExists);
            session = GetUnitOfWork(dataLayer);
            session.TypesManager.EnsureIsTypedObjectValid();
            objectHelper = new XpoObjectHelper(session);
            session.Connect();
        }
        public override void TearDownSession() {
            if(dataLayer != null) {
                IDisposable dld = dataLayer as IDisposable;
                if(dld != null)
                    dld.Dispose();
                dataLayer = null;
            }
            if(session != null) {
                session.Dispose();
                session = null;
            }
            objectHelper = null;
        }

        public override string ToString() {
            return "XPO (No Security)";
        }
    }
}
