using System;
using System.ComponentModel;
using System.Collections.Generic;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.EFCore {
    [DefaultProperty(nameof(Position.Title))]
    public class Position : IPosition {
        public Position() {
            Departments = new List<Department>();
            Contacts = new List<Contact>();
        }
        [Browsable(false)]
        public Int32 ID { get; protected set; }
        public string Title { get; set; }
        public virtual IList<Department> Departments { get; set; }

        public virtual IList<Contact> Contacts { get; set; }

        public void AddDepartment(IDepartment department) {
            Departments.Add((Department)department);
        }
    }
}
