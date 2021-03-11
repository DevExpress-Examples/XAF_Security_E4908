using System;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl.EF;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class DemoTask : Task, IDemoTask {
        public DemoTask()
            : base() {
            Priority = Base.Enums.Priority.Normal;
            Contacts = new List<Contact>();
        }

        public virtual IList<Contact> Contacts { get; set; }
        private Base.Enums.Priority priority;
        public Base.Enums.Priority Priority {
            get { return priority; }
            set { SetPropertyValue(ref priority, value); }
        }

        public override string ToString() {
            return Subject;
        }
        public int ActualWorkHours { get; set; }
        public int EstimatedWorkHours { get; set; }

        public void SetAssignedTo(IContact contact) {
            AssignedTo = (Party)contact;
        }
    }
}
