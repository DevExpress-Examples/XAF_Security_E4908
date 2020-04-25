using BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DatabaseUpdater {
    public class Updater {
        private const string AdministratorUserName = "Admin";
        private const string AdministratorRoleName = "Administrators";
        private const string DefaultUserName = "User";
        private const string DefaultUserRoleName = "Users";
        private IObjectSpace ObjectSpace { get; }
        public Updater(IObjectSpace objectSpace) { ObjectSpace = objectSpace; }

        public void UpdateDatabase() {
            CreateUser();
            CreateAdmin();
            CreateEmployees();
        }
        private void CreateUser() {
            PermissionPolicyUser defaultUser = ObjectSpace.GetObjectsQuery<PermissionPolicyUser>().FirstOrDefault(u => u.UserName == DefaultUserName);
            if(defaultUser == null) {
                defaultUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                defaultUser.UserName = DefaultUserName;
                defaultUser.SetPassword("");
                defaultUser.Roles.Add(GetUserRole());
                ObjectSpace.CommitChanges();
            }
        }
        private void CreateAdmin() {
            PermissionPolicyUser adminUser = ObjectSpace.GetObjectsQuery<PermissionPolicyUser>().FirstOrDefault(u => u.UserName == AdministratorUserName);
            if(adminUser == null) {
                adminUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                adminUser.UserName = AdministratorUserName;
                adminUser.SetPassword("");
                adminUser.Roles.Add(GetAdminRole());
                ObjectSpace.CommitChanges();
            }
        }
        private PermissionPolicyRole GetAdminRole() {
            PermissionPolicyRole adminRole = ObjectSpace.GetObjectsQuery<PermissionPolicyRole>().FirstOrDefault(u => u.Name == AdministratorRoleName);
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = AdministratorRoleName;
                adminRole.IsAdministrative = true;
            }
            return adminRole;
        }
        private PermissionPolicyRole GetUserRole() {
            PermissionPolicyRole userRole = ObjectSpace.GetObjectsQuery<PermissionPolicyRole>().FirstOrDefault(u => u.Name == DefaultUserRoleName);
            if(userRole == null) {
                userRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                userRole.Name = DefaultUserRoleName;
                userRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                userRole.AddTypePermissionsRecursively<Department>(SecurityOperations.Read, SecurityPermissionState.Deny);
                userRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Read, SecurityPermissionState.Allow);
                userRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Write, SecurityPermissionState.Allow);
                var protectedDepartment = "Development";
                // Allow users to read all departments if their title contains 'Development'.
                CriteriaOperator departmentCriteria = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty(nameof(Department.Title)), new OperandValue(protectedDepartment));
                userRole.AddObjectPermission<Department>(SecurityOperations.Read, departmentCriteria.ToString(), SecurityPermissionState.Allow);
                // The line below shows the same permission declaration using string criteria. For more information on criteria language syntax (both string and strongly-typed formats), see https://docs.devexpress.com/CoreLibraries/4928/devexpress-data-library/criteria-language-syntax.
                // userRole.AddObjectPermission<Department>(SecurityOperations.Read, "Contains(Title, 'Development')", SecurityPermissionState.Allow);
                CriteriaOperator employeeCriteria = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty(nameof(Employee.Department) + "." + nameof(Department.Title)), new OperandValue(protectedDepartment));
                userRole.AddObjectPermission<Employee>(SecurityOperations.Delete, employeeCriteria.ToString(), SecurityPermissionState.Allow);
                userRole.AddMemberPermission<Employee>(SecurityOperations.Write, nameof(Employee.LastName), (!employeeCriteria).ToString(), SecurityPermissionState.Deny);
                // The code lines below require a custom criteria function implementation (CurrentUserId) in non-XAF apps (https://docs.devexpress.com/eXpressAppFramework/113480/concepts/filtering/custom-function-criteria-operators).
                //userRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                //userRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                //userRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            }
            return userRole;
        }
        private void CreateEmployees() {
            DataTable employeesTable = GetEmployeesDataTable();
            foreach(DataRow employeeRow in employeesTable.Rows) {
                string email = Convert.ToString(employeeRow["EmailAddress"]);
                Employee employee = ObjectSpace.GetObjectsQuery<Employee>().FirstOrDefault(e => e.Email == email);
                if(employee == null) {
                    employee = ObjectSpace.CreateObject<Employee>();
                    employee.Email = email;
                    employee.FirstName = Convert.ToString(employeeRow["FirstName"]);
                    employee.LastName = Convert.ToString(employeeRow["LastName"]);
                    employee.Birthday = Convert.ToDateTime(employeeRow["BirthDate"]);

                    string departmentTitle = Convert.ToString(employeeRow["GroupName"]);
                    Department department = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse($"{nameof(Department.Title)}=?", departmentTitle), true);
                    if(department == null) {
                        department = ObjectSpace.CreateObject<Department>();
                        department.Title = departmentTitle;
                        Random rnd = new Random();
                        department.Office = $"{rnd.Next(1, 7)}0{rnd.Next(9)}";
                    }
                    employee.Department = department;
                }
            }
            ObjectSpace.CommitChanges();
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
    }
}
