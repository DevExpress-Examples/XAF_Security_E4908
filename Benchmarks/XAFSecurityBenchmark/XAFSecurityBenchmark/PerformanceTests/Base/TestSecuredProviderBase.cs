using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base.Security;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater;

namespace XAFSecurityBenchmark.PerformanceTests {
    public abstract class TestSecuredProviderBase<UserType, RoleType, ContactType, TaskType, DepartmentType> : TestProviderBase
        where UserType : class, IAuthenticationStandardUser, ICustomPermissionPolicyUser
        where RoleType : class, IPermissionPolicyRole
        where ContactType : class, IContact
        where TaskType : class, IDemoTask
        where DepartmentType : class, IDepartment {
        protected IObjectSpaceProvider securedObjectSpaceProvider;
        private IObjectSpace securedObjectSpace;
        private ITransactionHelper securedObjectHelper;

        static TestSecuredProviderBase() {
            Models.FunctionCriteriaOperators.UpcastFunctionOperator.Register();
            Models.FunctionCriteriaOperators.CurrentUserDepartmentOperator.Register();
        }

        public override void InsertEmptyContact(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            for(int i = 1; i < recordsCount + 1; i++) {
                var contact = securedObjectSpace.CreateObject<ContactType>();
                contact.SetDepartment(currentUser.Department);
                contact.FirstName = $"Contact{i}";
            }
            securedObjectSpace.CommitChanges();
        }
        public override void InsertContact(int recordsCount) {
            ICustomPermissionPolicyUser currentUser = GetUser();
            for(int i = 1; i < recordsCount + 1; i++) {
                var contact = securedObjectSpace.CreateObject<ContactType>();
                securedObjectHelper.FillContact(contact, currentUser.Department, i);
            }
            securedObjectSpace.CommitChanges();
        }
        public override void GetContacts(int recordsCount) {
            IList contacts = securedObjectSpace.CreateCollection(typeof(ContactType));
            securedObjectSpace.SetTopReturnedObjectsCount(contacts, recordsCount);

            foreach(var t in contacts) { }
            CheckCollectionCount(contacts, recordsCount);
        }

        public override void GetTasks(int recordsCount) {
            IList tasks = securedObjectSpace.CreateCollection(typeof(TaskType));
            securedObjectSpace.SetTopReturnedObjectsCount(tasks, recordsCount);
            foreach(TaskType t in tasks) { }
            CheckCollectionCount(tasks, recordsCount);
        }
        public override void UpdateContacts(int recordsCount) {
            IList contacts = securedObjectSpace.CreateCollection(typeof(ContactType));
            securedObjectSpace.SetTopReturnedObjectsCount(contacts, recordsCount);
            foreach(ContactType contact in contacts) {
                contact.Anniversary = DateTime.Now;
            }
            securedObjectSpace.CommitChanges();
        }
        public override void UpdateTasks(int recordsCount) {
            IList tasks = securedObjectSpace.CreateCollection(typeof(TaskType));
            securedObjectSpace.SetTopReturnedObjectsCount(tasks, recordsCount);
            foreach(TaskType task in tasks) {
                task.DueDate = DateTime.Now.AddHours(24).Date;
            }
            securedObjectSpace.CommitChanges();
        }

        protected abstract IObjectSpaceProvider CreateUpdatingObjectSpaceProvider();
        protected abstract IObjectSpaceProvider CreateSecuredObjectSpaceProvider(ISelectDataSecurityProvider selectDataSecurityProvider);
        protected abstract ITransactionHelper CreateObjectHelper(IObjectSpace objectSpace);

        public override void InitSession() {
            securedObjectSpace = securedObjectSpaceProvider.CreateObjectSpace();
            securedObjectHelper = CreateObjectHelper(securedObjectSpace);
        }
        public override void TearDownSession() {
            if(securedObjectSpace != null) {
                securedObjectSpace.Dispose();
                securedObjectSpace = null;
            }
            securedObjectHelper = null;
        }

        private void SetupSecuredObjectSpaceProvider(string logonUserName) {
            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(UserType), typeof(RoleType), authentication);
            security.RegisterXPOAdapterProviders();
            securedObjectSpaceProvider = CreateSecuredObjectSpaceProvider(security);

            string userName = logonUserName;
            string password = string.Empty;
            authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
            IObjectSpace loginObjectSpace = ((INonsecuredObjectSpaceProvider)securedObjectSpaceProvider).CreateNonsecuredObjectSpace();
            security.Logon(loginObjectSpace);

            SecuritySystem.SetInstance(security);
        }
        protected override ICustomPermissionPolicyUser GetUser() {
            string userName = TestSetConfig.TestUser;
            var currentUser = securedObjectSpace.FirstOrDefault<UserType>(o => o.UserName == userName);
            CheckUserData(currentUser);
            return currentUser;
        }

        public override void GlobalTestDataSetup(bool fullUpdate) {
            base.GlobalTestDataSetup(fullUpdate);
            SetupSecuredObjectSpaceProvider(TestSetConfig.TestUser);
        }

        public override void Dispose() {
            if(SecuritySystem.Instance is IDisposable security) {
                security.Dispose();
            }
            SecuritySystem.SetInstance(null);
            base.Dispose();
        }
    }
}
