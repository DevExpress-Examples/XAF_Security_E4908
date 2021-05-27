using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.XPO {
    [System.ComponentModel.DefaultProperty(nameof(Position.Title))]
    public class Position : BaseObject, IPosition {
        public Position(Session session)
            : base(session) {
        }
        private string title;
        public string Title {
            get {
                return title;
            }
            set {
                SetPropertyValue(nameof(Title), ref title, value);
            }
        }
        [Association("Departments-Positions")]
        public XPCollection<Department> Departments {
            get {
                return GetCollection<Department>(nameof(Departments));
            }
        }

        public void AddDepartment(IDepartment department) {
            Departments.Add((Department)department);
        }
    }
}
