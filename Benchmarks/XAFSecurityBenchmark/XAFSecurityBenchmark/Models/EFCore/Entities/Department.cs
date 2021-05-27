using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.EFCore {
    [DefaultProperty(nameof(Department.Title))]
    public class Department : IDepartment {
        public Department() {
            Positions = new List<Position>();
            Contacts = new List<Contact>();
        }
        [Browsable(false)]
        public Int32 ID { get; protected set; }
        public String Title { get; set; }
        public String Office { get; set; }
        public virtual IList<Position> Positions { get; set; }
        public virtual IList<Contact> Contacts { get; set; }
        public virtual IList<CustomPermissionPolicyUser> Users { get; set; }
        public string Location { get; set; }
        [StringLength(4096)]
        public string Description { get; set; }

        public virtual Contact DepartmentHead { get; set; }

        public void AddPositions(IPosition position) {
            Positions.Add((Position)position);
        }
    }
}
