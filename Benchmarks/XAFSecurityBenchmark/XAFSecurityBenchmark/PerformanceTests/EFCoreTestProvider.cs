using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.EFCore;
using XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater;
using XAFSecurityBenchmark.PerformanceTests.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests {
    public class EFCoreTestProvider : TestProviderBase {
        private EFCoreContext dataContext;

        static EFCoreTestProvider() {
            EFCoreDBUpdater.InitializeInstance(nameof(CustomPermissionPolicyUser.ID));
        }

        Expression<Func<Contact, bool>> ContactsFilterPredicate(ICustomPermissionPolicyUser currentUser) =>
            contact => contact.Department == currentUser.Department;
        Expression<Func<DemoTask, bool>> TasksFilterPredicate(ICustomPermissionPolicyUser currentUser) =>
           task => task.Contacts.Any(contact => contact.Department.Users.Any(user => user == currentUser)) || ((Contact)task.AssignedTo).Department == currentUser.Department;

        protected override ICustomPermissionPolicyUser GetUser() {
            string testUserName = TestSetConfig.TestUser;
            CustomPermissionPolicyUser currentUser = dataContext.Users.Where(user => user.UserName == testUserName).Include(user => user.Department).FirstOrDefault();
            CheckUserData(currentUser);
            return currentUser;
        }
        protected override IDBUpdater DBUpdater => EFCoreDBUpdater.Instance;//new EFCoreDBUpdater(KeyPropertyName);

        public override void InsertEmptyContact(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            for(int i = 0; i < recordsCount; i++) {
                var contact = dataContext.CreateProxy<Contact>();
                contact.FirstName = $"Contact{i}";
                ((IContact)contact).SetDepartment(currentUser.Department);
                dataContext.Contacts.Add(contact);
            }
            dataContext.SaveChanges();
        }
        public override void InsertContact(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            var objectHelper = new EFCoreObjectHelper(() => dataContext);
            objectHelper.BeginTransaction();
            for (int i = 0; i < recordsCount; i++) {
                var contact = dataContext.CreateProxy<Contact>();
                dataContext.Contacts.Add(contact);
                objectHelper.FillContact(contact, currentUser.Department, i);
            }
            dataContext.SaveChanges();
        }
        public override void GetContacts(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            var q = dataContext.Contacts.AsNoTracking().Where(ContactsFilterPredicate(currentUser)).Take(recordsCount);
            foreach(var t in q) { }
            CheckCollectionCount(q, recordsCount);
        }
        public override void GetTasks(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            var q = dataContext.Tasks.AsNoTracking().Where(TasksFilterPredicate(currentUser)).Take(recordsCount);
            foreach(var t in q) { }
            CheckCollectionCount(q, recordsCount);
        }
        public override void UpdateContacts(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            foreach(var contact in dataContext.Contacts.Where(ContactsFilterPredicate(currentUser)).Take(recordsCount)) {
                contact.Anniversary = DateTime.Now;
            }
            dataContext.SaveChanges();
        }
        public override void UpdateTasks(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            foreach(var task in dataContext.Tasks.Where(TasksFilterPredicate(currentUser)).Take(recordsCount)) {
                task.DueDate = DateTime.Now.AddHours(24).Date;
            }
            dataContext.SaveChanges();
        }

        public override void InitSession() {
            dataContext = new EFCoreContext();
            dataContext.Database.OpenConnection();
        }
        public override void TearDownSession() {
            if(dataContext != null) {
                dataContext.Dispose();
                dataContext = null;
            }
        }

        public override string ToString() {
            return "EF Core 7 (No Security)";
        }
    }
}
