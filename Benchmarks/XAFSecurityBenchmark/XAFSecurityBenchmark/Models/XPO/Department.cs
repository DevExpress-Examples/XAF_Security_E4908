using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.XPO {
    [System.ComponentModel.DefaultProperty(nameof(Department.Title))]
    public class Department : BaseObject, IDepartment {
        private string title;
        private string description;
        private Contact departmentHead;
        private string office;
        private string location;
        public Department(Session session)
            : base(session) {
        }
        public string Title {
            get {
                return title;
            }
            set {
                SetPropertyValue(nameof(Title), ref title, value);
            }
        }
        [Size(4096)]
        public string Description {
            get {
                return description;
            }
            set {
                SetPropertyValue(nameof(Description), ref description, value);
            }
        }
        public Contact DepartmentHead {
            get {
                return departmentHead;
            }
            set {
                SetPropertyValue(nameof(DepartmentHead), ref departmentHead, value);
            }
        }
        public string Location {
            get {
                return location;
            }
            set {
                SetPropertyValue(nameof(Location), ref location, value);
            }
        }
        public string Office {
            get {
                return office;
            }
            set {
                SetPropertyValue(nameof(Office), ref office, value);
            }
        }
        [Association("Department-Contacts")]
        public XPCollection<Contact> Contacts {
            get {
                return GetCollection<Contact>(nameof(Contacts));
            }
        }
        [Association("Departments-Positions")]
        public XPCollection<Position> Positions {
            get {
                return GetCollection<Position>(nameof(Positions));
            }
        }
        [Association("Department-Users")]
        public XPCollection<CustomPermissionPolicyUser> Users {
            get {
                return GetCollection<CustomPermissionPolicyUser>(nameof(Users));
            }
        }

        public void AddPositions(IPosition position) {
            Positions.Add((Position)position);
        }
    }
}
