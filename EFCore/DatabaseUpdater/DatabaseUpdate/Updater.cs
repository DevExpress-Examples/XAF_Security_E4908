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
        private IObjectSpace objectSpace;

        public Updater(IObjectSpace objectSpace) {
            this.objectSpace = objectSpace;
        }

        public void UpdateDatabase() {
            CreateDepartments();
            CreateEmployees();
            CreateUser();
            CreateAdmin();
            objectSpace.CommitChanges();
        }
        private DataTable GetEmployeesDataTable() {
            string shortName = "Employees.xml";
            string embeddedResourceName = Array.Find<string>(this.GetType().Assembly.GetManifestResourceNames(), (s) => { return s.Contains(shortName); });
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedResourceName);
            if(stream == null) {
                throw new Exception(string.Format("Cannot read employees data from the {0} file!", shortName));
            }
            DataSet ds = new DataSet();
            ds.ReadXml(stream);
            return ds.Tables["Employee"];
        }
        private void CreateEmployees() {
            DataTable employeesTable = GetEmployeesDataTable();
            foreach(DataRow employeeRow in employeesTable.Rows) {
                string email = Convert.ToString(employeeRow["EmailAddress"]);
                Employee employee = objectSpace.FindObject<Employee>(CriteriaOperator.Parse("Email=?", email));
                if(employee == null) {
                    employee = objectSpace.CreateObject<Employee>();
                    employee.Email = email;
                    employee.FirstName = Convert.ToString(employeeRow["FirstName"]);
                    employee.LastName = Convert.ToString(employeeRow["LastName"]);
                    employee.Birthday = Convert.ToDateTime(employeeRow["BirthDate"]);

                    string departmentTitle = Convert.ToString(employeeRow["GroupName"]);
                    Department department = objectSpace.FindObject<Department>(CriteriaOperator.Parse("Title=?", departmentTitle), true);
                    if(department == null) {
                        department = objectSpace.CreateObject<Department>();
                        department.Title = departmentTitle;
                        Random rnd = new Random();
                        department.Office = string.Format("{0}0{0}", rnd.Next(1, 7), rnd.Next(9));
                    }
                    employee.Department = department;
                }
            }
        }
        private void CreateDepartments() {
            Department devDepartment = objectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'Development Department'"));
            if(devDepartment == null) {
                devDepartment = objectSpace.CreateObject<Department>();
                devDepartment.Title = "Development Department";
                devDepartment.Office = "205";
            }
            Department seoDepartment = objectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'SEO'"));
            if(seoDepartment == null) {
                seoDepartment = objectSpace.CreateObject<Department>();
                seoDepartment.Title = "SEO";
                seoDepartment.Office = "703";
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
            sampleUser.Roles.Add(defaultRole);
        }
        private void CreateAdmin() {
            PermissionPolicyUser userAdmin = objectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
            if(userAdmin == null) {
                userAdmin = objectSpace.CreateObject<PermissionPolicyUser>();
                userAdmin.UserName = "Admin";
                userAdmin.SetPassword("");
            }
            PermissionPolicyRole adminRole = CreateAdminRole();
            userAdmin.Roles.Add(adminRole);
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
                defaultRole.AddTypePermissionsRecursively<Department>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddObjectPermission<Department>(SecurityOperations.Read, "Contains([Title], 'Development')", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Write, SecurityPermissionState.Allow);
                defaultRole.AddObjectPermission<Employee>(SecurityOperations.Delete, "Contains([Department.Title], 'Development')", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<Employee>(SecurityOperations.Write, "LastName", "Not Contains([Department.Title], 'Development')", SecurityPermissionState.Deny);
            }
            return defaultRole;
        }
    }
}
