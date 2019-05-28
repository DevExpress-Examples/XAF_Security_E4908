using System;
using System.Linq;
using System.Data;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using XafSolution.Module.BusinessObjects;
using System.IO;

namespace XafSolution.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            CreateDepartments();
            CreateEmployees();
            CreateUser();
            CreateAdmin();
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
        private void CreateEmployees() {
            DataTable employeesTable = GetEmployeesDataTable();
            foreach(DataRow employeeRow in employeesTable.Rows) {
                string email = Convert.ToString(employeeRow["EmailAddress"]);
                Employee employee = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("Email=?", email));
                if(employee == null) {
                    employee = ObjectSpace.CreateObject<Employee>();
                    employee.Email = email;
                    employee.FirstName = Convert.ToString(employeeRow["FirstName"]);
                    employee.LastName = Convert.ToString(employeeRow["LastName"]);
                    employee.Birthday = Convert.ToDateTime(employeeRow["BirthDate"]);
                    
                    string departmentTitle = Convert.ToString(employeeRow["GroupName"]);
                    Department department = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title=?", departmentTitle), true);
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
            Department devDepartment = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'Development Department'"));
            if(devDepartment == null) {
                devDepartment = ObjectSpace.CreateObject<Department>();
                devDepartment.Title = "Development Department";
                devDepartment.Office = "205";
            }
            Department seoDepartment = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'SEO'"));
            if(seoDepartment == null) {
                seoDepartment = ObjectSpace.CreateObject<Department>();
                seoDepartment.Title = "SEO";
                seoDepartment.Office = "703";
            }
        }
        private void CreateUser() {
            PermissionPolicyUser sampleUser = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "User"));
            if(sampleUser == null) {
                sampleUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                sampleUser.UserName = "User";
                sampleUser.SetPassword("");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);
        }
        private void CreateAdmin() {
            PermissionPolicyUser userAdmin = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
            if(userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<PermissionPolicyUser>();
                userAdmin.UserName = "Admin";
                userAdmin.SetPassword("");
            }
            PermissionPolicyRole adminRole = CreateAdminRole();
            userAdmin.Roles.Add(adminRole);
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
        private PermissionPolicyRole CreateAdminRole() {
            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
            return adminRole;
        }
        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

				defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
				defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/Department_ListView", SecurityPermissionState.Allow);
				defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/Employee_ListView", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Department>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddObjectPermission<Department>(SecurityOperations.Read, "Contains([Title], 'Development')", SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<Employee>(SecurityOperations.Read, SecurityPermissionState.Allow);
			}
            return defaultRole;
        }
    }
}
