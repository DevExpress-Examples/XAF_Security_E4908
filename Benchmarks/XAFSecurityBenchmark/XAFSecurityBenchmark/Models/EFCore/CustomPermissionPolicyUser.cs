using System;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class CustomPermissionPolicyUser : PermissionPolicyUser, ICustomPermissionPolicyUser {
        public virtual Department Department { get; set; }

        IDepartment ICustomPermissionPolicyUser.Department {
            get { return Department; }
        }

        void ICustomPermissionPolicyUser.SetDepartment(IDepartment department) {
            Department = (Department)department;
        }

        void ICustomPermissionPolicyUser.SetUserRole(IPermissionPolicyRole role) {
            Roles.Add((PermissionPolicyRole)role);
        }
    }
}
