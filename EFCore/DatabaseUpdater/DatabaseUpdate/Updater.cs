using System;
using System.Data;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using System.IO;
using DevExpress.Persistent.Base;
using BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects;

namespace DatabaseUpdater {
    public class Updater {
        private IObjectSpace objectSpace { get; }

        public Updater(IObjectSpace objectSpace) {
            this.objectSpace = objectSpace;
        }
        public void UpdateDatabase() {
            CreatePersons();
            CreateUser();
            CreateAdmin();
            objectSpace.CommitChanges();
        }
        private DataTable GetPersonsDataTable() {
            string shortName = "Persons.xml";
            string embeddedResourceName = Array.Find<string>(this.GetType().Assembly.GetManifestResourceNames(), (s) => { return s.Contains(shortName); });
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedResourceName);
            if(stream == null) {
                throw new Exception(string.Format("Cannot read Persons data from the {0} file!", shortName));
            }
            DataSet ds = new DataSet();
            ds.ReadXml(stream);
            return ds.Tables["Person"];
        }
        private void CreatePersons() {
            DataTable PersonsTable = GetPersonsDataTable();
            foreach(DataRow PersonRow in PersonsTable.Rows) {
                string email = Convert.ToString(PersonRow["EmailAddress"]);
                Person Person = objectSpace.FindObject<Person>(CriteriaOperator.Parse("Email=?", email));
                if(Person == null) {
                    Person = objectSpace.CreateObject<Person>();
                    Person.Email = email;
                    Person.FirstName = Convert.ToString(PersonRow["FirstName"]);
                    Person.LastName = Convert.ToString(PersonRow["LastName"]);
                    Person.Birthday = Convert.ToDateTime(PersonRow["BirthDate"]);

                    string departmentTitle = Convert.ToString(PersonRow["GroupName"]);
                }
            }
        }
        private void CreateUser() {
            PermissionPolicyUser sampleUser = objectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "User"));
            if(sampleUser == null) {
                sampleUser = objectSpace.CreateObject<PermissionPolicyUser>();
                sampleUser.UserName = "User";
                sampleUser.SetPassword("");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.AddRole(defaultRole);
        }
        private void CreateAdmin() {
            PermissionPolicyUser userAdmin = objectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
            if(userAdmin == null) {
                userAdmin = objectSpace.CreateObject<PermissionPolicyUser>();
                userAdmin.UserName = "Admin";
                userAdmin.SetPassword("");
            }
            PermissionPolicyRole adminRole = CreateAdminRole();
            userAdmin.AddRole(adminRole);
        }
        private PermissionPolicyRole CreateAdminRole() {
            PermissionPolicyRole adminRole = objectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if(adminRole == null) {
                adminRole = objectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
            return adminRole;
        }
        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = objectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = objectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);  
                
                defaultRole.AddTypePermissionsRecursively<Person>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Person>(SecurityOperations.Write, SecurityPermissionState.Allow);

                defaultRole.AddObjectPermission<Person>(SecurityOperations.Read, "Contains([Email], '@administration.com')", SecurityPermissionState.Deny);
                defaultRole.AddMemberPermission<Person>(SecurityOperations.Read, "Email;Birthday", "Contains([Email], '@management.com')", SecurityPermissionState.Deny);

            }
            return defaultRole;
        }
    }
}
