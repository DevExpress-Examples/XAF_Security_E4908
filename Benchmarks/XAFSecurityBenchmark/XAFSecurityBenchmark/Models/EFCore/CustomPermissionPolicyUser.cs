using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class CustomPermissionPolicyUser : PermissionPolicyUser, ICustomPermissionPolicyUser {

        private Department department;
        public virtual Department Department {
            get {
                return department;
            }
            set {
                department = value;
            }
        }

        IDepartment ICustomPermissionPolicyUser.Department {
            get {
                return Department;
            }
        }

        void ICustomPermissionPolicyUser.SetDepartment(IDepartment department) {
            Department = (Department)department;
        }

        void ICustomPermissionPolicyUser.SetUserRole(IPermissionPolicyRole role) {
            Roles.Add((PermissionPolicyRole)role);
        }
    }
}
