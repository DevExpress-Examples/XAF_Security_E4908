using System;
using System.ComponentModel;
using System.Collections.Generic;
using XAFSecurityBenchmark.Models.Base;
using System.Collections.ObjectModel;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {
    [DefaultProperty(nameof(Position.Title))]
    public class Position : BaseObject, IPosition {
        public virtual string Title { get; set; }
        public virtual IList<Department> Departments { get; set; } = new ObservableCollection<Department>();
        public virtual IList<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

        public void AddDepartment(IDepartment department) {
            Departments.Add((Department)department);
        }
    }
}
