using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base.Security;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.Base.Enums;
using XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater;
using System.Linq;

namespace XAFSecurityBenchmark.PerformanceTests.DBUpdater {
    public interface IDBUpdater {
        void CheckAndUpdateDB(bool fullUpdate);
        void CleanupTestData();
    }
    public abstract class DBUpdaterBase<UserType, RoleType, TypePermission, MemberPermissions, ObjectPermissions, NavigationPermission, ContactType, TaskType, DepartmentType> : IDBUpdater
        where UserType : class, IAuthenticationStandardUser, ICustomPermissionPolicyUser
        where RoleType : class, IPermissionPolicyRole
        where TypePermission : class, IPermissionPolicyTypePermissionObject
        where MemberPermissions : class, IPermissionPolicyMemberPermissionsObject
        where ObjectPermissions : class, IPermissionPolicyObjectPermissionsObject
        where NavigationPermission : class, IPermissionPolicyNavigationPermissionObject
        where ContactType : class, IPerson, IContact
        where TaskType : class, ITask
        where DepartmentType : class, IDepartment {

        private IObjectSpaceProvider updatingObjectSpaceProvider;
        private string keyPropertyName;

        protected DBUpdaterBase(string keyPropertyName) {
            RegisterEntitiesCore();
            updatingObjectSpaceProvider = CreateUpdatingObjectSpaceProvider();
            updatingObjectSpaceProvider.UpdateSchema();
            this.keyPropertyName = keyPropertyName;
        }

        private void RegisterEntitiesCore() {
            RegisterEntities();
            XafTypesInfo.Instance.RegisterEntity(typeof(ContactType));
            XafTypesInfo.Instance.RegisterEntity(typeof(UserType));
            XafTypesInfo.Instance.RegisterEntity(typeof(RoleType));
        }
        protected virtual void RegisterEntities() { }

        public void CheckAndUpdateDB(bool fullUpdate) {
            Console.WriteLine($"-----------------------{nameof(CheckAndUpdateDB)}-----------------------");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using(IObjectSpace updatingObjectSpace = updatingObjectSpaceProvider.CreateObjectSpace()) {
                if(!TestDataSetIsActual(updatingObjectSpace)) {
                    CleanupAllTestData(updatingObjectSpace);
                    updatingObjectSpace.CommitChanges();

                    CreateSecurityObjects(updatingObjectSpace);
                    if(fullUpdate) {
                        TemporaryTestObjectsHelper.CreateTestDataSet(CreateUpdatingObjectHelper(updatingObjectSpace));
                    }

                    updatingObjectSpace.CommitChanges();
                }
            }
            sw.Stop();
            Console.WriteLine($"----------------End-of-{nameof(CheckAndUpdateDB)}--Time:{sw.Elapsed}");
        }
        private bool TestDataSetIsActual(IObjectSpace updatingObjectSpace) {
            bool result = updatingObjectSpace.GetObjectsQuery<ContactType>().Count() == ExpectedContactsCount &&
                   updatingObjectSpace.GetObjectsQuery<TaskType>().Count() == ExpectedTasksCount;
            return result;
        }

        private int ExpectedTasksCount =>
            TestSetConfig.ContactCountPerUserToCreate * TestSetConfig.Users.Length * (TestSetConfig.TasksLinkedToContact + TestSetConfig.TasksAssigedToContact);
        private int ExpectedContactsCount => TestSetConfig.ContactCountPerUserToCreate * TestSetConfig.Users.Length;

        private void CreateSecurityObjects(IObjectSpace updatingObjectSpace) {
            var userSam = updatingObjectSpace.FirstOrDefault<UserType>(user => user.UserName == "Sam");
            if(userSam == null) {
                userSam = updatingObjectSpace.CreateObject<UserType>();
                ((IAuthenticationActiveDirectoryUser)userSam).UserName = "Sam";
                userSam.SetPassword("");
            }

            // If a role with the Administrators name doesn't exist in the database, create this role
            RoleType adminRole = updatingObjectSpace.FirstOrDefault<RoleType>(role => role.Name == "Administrators");
            if(adminRole == null) {
                adminRole = updatingObjectSpace.CreateObject<RoleType>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;

            // If a role with the Users name doesn't exist in the database, create this role
            RoleType userRole = updatingObjectSpace.FirstOrDefault<RoleType>(role => role.Name == "Users");
            if(userRole == null) {
                userRole = updatingObjectSpace.CreateObject<RoleType>();
                userRole.Name = "Users";
                userRole.PermissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
                userRole.AddTypePermission<RoleType>(SecurityOperations.FullAccess, SecurityPermissionState.Deny);
                userRole.AddTypePermission<UserType>(SecurityOperations.FullAccess, SecurityPermissionState.Deny);
                userRole.AddObjectPermission<UserType>(SecurityOperations.ReadOnlyAccess, $"[{keyPropertyName}] = CurrentUserId()", SecurityPermissionState.Allow);
                userRole.AddMemberPermission<UserType>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", null, SecurityPermissionState.Allow);
                userRole.AddMemberPermission<UserType>(SecurityOperations.Write, "StoredPassword", null, SecurityPermissionState.Allow);
                userRole.AddTypePermission<RoleType>(SecurityOperations.Read, SecurityPermissionState.Allow);
                userRole.AddTypePermission<TypePermission>("Write;Delete;Create", SecurityPermissionState.Deny);
                userRole.AddTypePermission<MemberPermissions>("Write;Delete;Create", SecurityPermissionState.Deny);
                userRole.AddTypePermission<ObjectPermissions>("Write;Delete;Create", SecurityPermissionState.Deny);



                userRole.AddTypePermission<ContactType>(SecurityOperations.FullObjectAccess, SecurityPermissionState.Deny);
                userRole.AddObjectPermission<ContactType>(SecurityOperations.FullObjectAccess, $"[Department].[Users][[{keyPropertyName}] == CurrentUserId()].Exists()", SecurityPermissionState.Allow);

                userRole.AddMemberPermission<ContactType>(SecurityOperations.ReadWriteAccess, nameof(IPerson.FirstName), $"Contains([{nameof(IPerson.FirstName)}], '1Е2Е3')", SecurityPermissionState.Deny);
                userRole.AddMemberPermission<ContactType>(SecurityOperations.ReadWriteAccess, nameof(IPerson.LastName), $"Contains([{nameof(IPerson.LastName)}], '1Е2Е3')", SecurityPermissionState.Deny);
                userRole.AddMemberPermission<ContactType>(SecurityOperations.ReadWriteAccess, nameof(IPerson.Email), $"Contains([{nameof(IPerson.Email)}], '1Е2Е3')", SecurityPermissionState.Deny);
                userRole.AddMemberPermission<ContactType>(SecurityOperations.ReadWriteAccess, nameof(IPerson.Birthday), $"[{nameof(IPerson.Birthday)}] > #2050-03-22 13:18:51#", SecurityPermissionState.Deny);


                userRole.AddTypePermission<TaskType>(SecurityOperations.FullObjectAccess, SecurityPermissionState.Deny);
                userRole.AddObjectPermission<TaskType>(SecurityOperations.FullObjectAccess, $"[Contacts][[Department].[Users][[{keyPropertyName}] == CurrentUserId()].Exists()]", SecurityPermissionState.Allow);

                if(typeof(TaskType).IsSubclassOf(typeof(DevExpress.Persistent.BaseImpl.Task))) {
                    userRole.AddObjectPermission<TaskType>(SecurityOperations.FullObjectAccess, $"[AssignedTo].<Contact>[Department].[Users][[{keyPropertyName}] == CurrentUserId()].Exists()", SecurityPermissionState.Allow);
                }
                else {
                    userRole.AddObjectPermission<TaskType>(SecurityOperations.FullObjectAccess, "Upcast(AssignedTo, 'XAFSecurityBenchmark.Models.EFCore.Contact', 'Department') == CurrentUserDepartment()", SecurityPermissionState.Allow);
                }
            }
            updatingObjectSpace.CommitChanges();
            // Add the Administrators role to the user Sam
            userSam.SetUserRole(adminRole);
            // Add the Users role to a user
            foreach(string userName in TestSetConfig.Users) {
                var justUser = updatingObjectSpace.FirstOrDefault<UserType>(user => user.UserName == userName);
                if(justUser == null) {
                    justUser = updatingObjectSpace.CreateObject<UserType>();
                    ((IAuthenticationActiveDirectoryUser)justUser).UserName = userName;
                    justUser.SetPassword("");

                    justUser.SetUserRole(userRole);
                }

                string userDepartmentName = $"The {userName} department!";
                var userDepartment = updatingObjectSpace.FirstOrDefault<DepartmentType>(department => department.Title == userDepartmentName);
                if(userDepartment == null) {
                    userDepartment = updatingObjectSpace.CreateObject<DepartmentType>();
                    userDepartment.Title = userDepartmentName;
                }
                justUser.SetDepartment(userDepartment);
            }
            updatingObjectSpace.CommitChanges();
        }
        private void DeleterSecurityObjects(IObjectSpace updatingObjectSpace) {
            updatingObjectSpace.RemoveAllObjects<UserType>();
            updatingObjectSpace.CommitChanges();

            updatingObjectSpace.RemoveAllObjects<TypePermission>();
            updatingObjectSpace.RemoveAllObjects<MemberPermissions>();
            updatingObjectSpace.RemoveAllObjects<ObjectPermissions>();
            updatingObjectSpace.RemoveAllObjects<NavigationPermission>();
            updatingObjectSpace.CommitChanges();

            updatingObjectSpace.RemoveAllObjects<RoleType>();
            updatingObjectSpace.CommitChanges();

            updatingObjectSpace.RemoveAllObjects<ContactType>();
            updatingObjectSpace.RemoveAllObjects<DepartmentType>();
            updatingObjectSpace.RemoveAllObjects<TaskType>();
            updatingObjectSpace.CommitChanges();
        }

        private void CleanupAllTestData(IObjectSpace updatingObjectSpace) {
            CleanupTestData();
            DeleterSecurityObjects(updatingObjectSpace);
        }

        public void CleanupTestData() {
            using(IObjectSpace updatingObjectSpace = updatingObjectSpaceProvider.CreateObjectSpace()) {
                var testDataSet = CreateUpdatingObjectHelper(updatingObjectSpace);
                testDataSet.BeginTransaction();
                testDataSet.RemoveAllTestData();
                testDataSet.SaveChanges();
                testDataSet.EndTransaction();
            }
        }

        protected abstract IObjectSpaceProvider CreateUpdatingObjectSpaceProvider();
        protected abstract ITransactionHelper CreateUpdatingObjectHelper(IObjectSpace updatingObjectSpace);
    }
}
