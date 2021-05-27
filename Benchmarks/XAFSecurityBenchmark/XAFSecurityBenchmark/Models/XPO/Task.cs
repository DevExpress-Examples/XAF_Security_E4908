using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.Base.Enums;

namespace XAFSecurityBenchmark.Models.XPO {
    public class DemoTask : Task, IComparable, IDemoTask {
        private Priority priority;
        private int estimatedWorkHours;
        private int actualWorkHours;
        public DemoTask(Session session)
            : base(session) {
        }

        public Priority Priority {
            get {
                return priority;
            }
            set {
                SetPropertyValue(nameof(Priority), ref priority, value);
            }
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Priority = Priority.Normal;
        }

        [Association("Contact-DemoTask")]
        public XPCollection<Contact> Contacts {
            get {
                return GetCollection<Contact>(nameof(Contacts));
            }
        }
        public override string ToString() {
            return this.Subject;
        }
        public int EstimatedWorkHours {
            get {
                return estimatedWorkHours;
            }
            set {
                SetPropertyValue<int>(nameof(EstimatedWorkHours), ref estimatedWorkHours, value);
            }
        }
        public int ActualWorkHours {
            get {
                return actualWorkHours;
            }
            set {
                SetPropertyValue<int>(nameof(ActualWorkHours), ref actualWorkHours, value);
            }
        }

        public void SetAssignedTo(IContact contact) {
            AssignedTo = (Party)contact;
        }
    }
}
