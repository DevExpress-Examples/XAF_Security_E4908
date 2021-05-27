using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.XPO {
    public class CustomPermissionPolicyUser : PermissionPolicyUser, ICustomPermissionPolicyUser {
        public CustomPermissionPolicyUser(Session s) : base(s) { }


        private Department department;
        [Association("Department-Users")]
        public Department Department {
            get {
                return department;
            }
            set {
                SetPropertyValue(nameof(Department), ref department, value);
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
