using System;
using System.Linq;
using System.Collections.Generic;
using XAFSecurityBenchmark.Models.Base;
using System.Collections.ObjectModel;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class DemoTask : Task, IDemoTask {
        public virtual IList<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();
        public virtual Base.Enums.Priority Priority { get; set; }
        public virtual int ActualWorkHours { get; set; }
        public virtual int EstimatedWorkHours { get; set; }

        public void SetAssignedTo(IContact contact) {
            AssignedTo = (Party)contact;
        }
        public override string ToString() {
            return Subject;
        }
    }
}
