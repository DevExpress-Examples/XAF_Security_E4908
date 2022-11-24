using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using XAFSecurityBenchmark.Models.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel;

namespace XAFSecurityBenchmark.Models.EFCore {

    [DefaultProperty(nameof(Department.Title))]
    public class Department : BaseObject, IDepartment {
        public virtual String Title { get; set; }
        public virtual String Office { get; set; }
        public virtual IList<Position> Positions { get; set; } = new ObservableCollection<Position>();
        public virtual IList<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();
        public virtual IList<CustomPermissionPolicyUser> Users { get; set; } = new ObservableCollection<CustomPermissionPolicyUser>();
        public virtual string Location { get; set; }

        [StringLength(4096)]
        public virtual string Description { get; set; }
        public virtual Contact DepartmentHead { get; set; }

        public void AddPositions(IPosition position) {
            Positions.Add((Position)position);
        }

        public override bool Equals(object obj) {
            return obj is Department department &&
                   ID.Equals(department.ID);
        }
        public override int GetHashCode() {
            return ID.GetHashCode();
        }
    }
}
