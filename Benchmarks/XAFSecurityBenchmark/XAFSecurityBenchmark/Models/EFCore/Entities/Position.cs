using System;
using System.ComponentModel;
using System.Collections.Generic;
using XAFSecurityBenchmark.Models.Base;
using System.Collections.ObjectModel;

namespace XAFSecurityBenchmark.Models.EFCore {
    [DefaultProperty(nameof(Position.Title))]
    public class Position : IPosition {
        public virtual Int32 ID { get; set; }
        public virtual string Title { get; set; }
        public virtual IList<Department> Departments { get; set; } = new ObservableCollection<Department>();
        public virtual IList<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

        public void AddDepartment(IDepartment department) {
            Departments.Add((Department)department);
        }
    }
}
