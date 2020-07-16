using BusinessObjectsLibrary.EFCore.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DatabaseUpdater.EFCore {
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
            PermissionPolicyUser defaultUser = ObjectSpace.FirstOrDefault<PermissionPolicyUser>(u => u.UserName == DefaultUserName);
            if(defaultUser == null) {
                defaultUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                defaultUser.UserName = DefaultUserName;
                defaultUser.SetPassword("");
                defaultUser.Roles.Add(GetUserRole());
                ObjectSpace.CommitChanges();
            }
        }
        private void CreateAdmin() {
            PermissionPolicyUser adminUser = ObjectSpace.FirstOrDefault<PermissionPolicyUser>(u => u.UserName == AdministratorUserName);
            if(adminUser == null) {
                adminUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                adminUser.UserName = AdministratorUserName;
                adminUser.SetPassword("");
                adminUser.Roles.Add(GetAdminRole());
                ObjectSpace.CommitChanges();
            }
        }
        private PermissionPolicyRole GetAdminRole() {
            PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(u => u.Name == AdministratorRoleName);
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = AdministratorRoleName;
                adminRole.IsAdministrative = true;
            }
            return adminRole;
        }
        private PermissionPolicyRole GetUserRole() {
            PermissionPolicyRole userRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(u => u.Name == DefaultUserRoleName);
            if(userRole == null) {
                userRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                userRole.Name = DefaultUserRoleName;
                // Allow users to read departments only if their title contains 'Development'. 
                const string protectedDepartment = "Development";
                CriteriaOperator departmentCriteria = new FunctionOperator(FunctionOperatorType.Contains,
                    new OperandProperty(nameof(Department.Title)), new OperandValue(protectedDepartment)
                );
                userRole.AddObjectPermission<Department>(SecurityOperations.Read, (!departmentCriteria).ToString() /*"!Contains(Title, 'Development')"*/, SecurityPermissionState.Deny);
                // Allow users to read and modify employee records and their fields by criteria.
                userRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Read, SecurityPermissionState.Allow);
                userRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Write, SecurityPermissionState.Allow);
                CriteriaOperator employeeCriteria = new FunctionOperator(FunctionOperatorType.Contains, 
                    new OperandProperty(nameof(Employee.Department) + "." + nameof(Department.Title)), new OperandValue(protectedDepartment)
                );
                userRole.AddObjectPermission<Employee>(SecurityOperations.Delete, employeeCriteria.ToString()/*"Contains(Department.Title, 'Development')"*/, SecurityPermissionState.Allow);
                userRole.AddMemberPermission<Employee>(SecurityOperations.Write, nameof(Employee.LastName), (!employeeCriteria).ToString() /*"!Contains(Department.Title, 'Development')"*/, SecurityPermissionState.Deny);
                // For more information on criteria language syntax (both string and strongly-typed formats), see https://docs.devexpress.com/CoreLibraries/4928/.
            }
            return userRole;
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
