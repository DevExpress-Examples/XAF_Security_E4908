using BusinessObjectsLibrary;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Data;
using System.IO;

namespace DatabaseUpdater {
    public class Updater {
        private const string AdministratorUserName = "Admin";
        private const string AdministratorRoleName = "Administrators";
        private const string DefaultUserName = "User";
        private const string DefaultUserRoleName = "Users";
        private IObjectSpace ObjectSpace { get; }
        public Updater(IObjectSpace objectSpace) {
            ObjectSpace = objectSpace;
        }
        public void UpdateDatabase() {
            CreateUser();
            CreateAdmin();
            CreateEmployees();
            CreateDepartments();
            ObjectSpace.CommitChanges();
        }
        private void CreateUser() {
            PermissionPolicyUser sampleUser = ObjectSpace.FirstOrDefault<PermissionPolicyUser>(u => u.UserName == DefaultUserName);
            if(sampleUser == null) {
                sampleUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                sampleUser.UserName = DefaultUserName;
                sampleUser.SetPassword("");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);
        }
        private void CreateAdmin() {
            PermissionPolicyUser userAdmin = ObjectSpace.FirstOrDefault<PermissionPolicyUser>(u => u.UserName == AdministratorUserName);
            if(userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<PermissionPolicyUser>();
                userAdmin.UserName = AdministratorUserName;
                userAdmin.SetPassword("");
            }
            PermissionPolicyRole adminRole = CreateAdminRole();
            userAdmin.Roles.Add(adminRole);
        }
        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == DefaultUserRoleName);
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = DefaultUserRoleName;
                const string protectedDepartment = "Development";
                defaultRole.AddTypePermissionsRecursively<Department>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddObjectPermissionFromLambda<Department>(SecurityOperations.Read, t => t.Title.Contains(protectedDepartment), SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Write, SecurityPermissionState.Allow);
                defaultRole.AddObjectPermissionFromLambda<Employee>(SecurityOperations.Delete, t => t.Department.Title.Contains(protectedDepartment), SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<Employee>(SecurityOperations.Write, nameof(Employee.LastName), t => !t.Department.Title.Contains(protectedDepartment), SecurityPermissionState.Deny);
            }
            return defaultRole;
        }
        private PermissionPolicyRole CreateAdminRole() {
            PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == AdministratorRoleName);
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = AdministratorRoleName;
            }
            adminRole.IsAdministrative = true;
            return adminRole;
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
                Employee employee = ObjectSpace.FirstOrDefault<Employee>(e => e.Email == email);
                if(employee == null) {
                    employee = ObjectSpace.CreateObject<Employee>();
                    employee.Email = email;
                    employee.FirstName = Convert.ToString(employeeRow["FirstName"]);
                    employee.LastName = Convert.ToString(employeeRow["LastName"]);
                    employee.Birthday = Convert.ToDateTime(employeeRow["BirthDate"]);
                    
                    string departmentTitle = Convert.ToString(employeeRow["GroupName"]);
                    Department department = ObjectSpace.FirstOrDefault<Department>(d => d.Title == departmentTitle, true);
                    if(department == null) {
                        department = ObjectSpace.CreateObject<Department>();
                        department.Title = departmentTitle;
                        Random rnd = new Random();
                        department.Office = string.Format("{0}0{0}", rnd.Next(1, 7), rnd.Next(9));
                    }
                    employee.Department = department;
                }
            }
        }
        private void CreateDepartments() {
            Department devDepartment = ObjectSpace.FirstOrDefault<Department>(d => d.Title == "Development Department");
            if(devDepartment == null) {
                devDepartment = ObjectSpace.CreateObject<Department>();
                devDepartment.Title = "Development Department";
                devDepartment.Office = "205";
            }
            Department seoDepartment = ObjectSpace.FirstOrDefault<Department>(d => d.Title == "SEO");
            if(seoDepartment == null) {
                seoDepartment = ObjectSpace.CreateObject<Department>();
                seoDepartment.Title = "SEO";
                seoDepartment.Office = "703";
            }
        }
    }
}
