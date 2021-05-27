using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.Base;

namespace XAFSecurityBenchmark.Models.Base {
    public interface ICustomPermissionPolicyUser {
        IDepartment Department { get; }
        void SetDepartment(IDepartment department);
        void SetUserRole(IPermissionPolicyRole role);
    }
}
