using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.Base.Enums;

namespace XAFSecurityBenchmark.Models.XPO {
    public class Contact : Person, IContact {
        private string webPageAddress;
        private Contact manager;
        private string nickName;
        private string spouseName;
        private TitleOfCourtesy titleOfCourtesy;
        private string notes;
        private DateTime? anniversary;
        public Contact(Session session) :
            base(session) {
        }
        public string WebPageAddress {
            get {
                return webPageAddress;
            }
            set {
                SetPropertyValue(nameof(WebPageAddress), ref webPageAddress, value);
            }
        }
        public Contact Manager {
            get {
                return manager;
            }
            set {
                SetPropertyValue(nameof(Manager), ref manager, value);
            }
        }
        public string NickName {
            get {
                return nickName;
            }
            set {
                SetPropertyValue(nameof(NickName), ref nickName, value);
            }
        }
        public string SpouseName {
            get {
                return spouseName;
            }
            set {
                SetPropertyValue(nameof(SpouseName), ref spouseName, value);
            }
        }
        public TitleOfCourtesy TitleOfCourtesy {
            get {
                return titleOfCourtesy;
            }
            set {
                SetPropertyValue(nameof(TitleOfCourtesy), ref titleOfCourtesy, value);
            }
        }
        public DateTime? Anniversary {
            get {
                return anniversary;
            }
            set {
                SetPropertyValue(nameof(Anniversary), ref anniversary, value);
            }
        }
        [Size(4096)]
        public string Notes {
            get {
                return notes;
            }
            set {
                SetPropertyValue(nameof(Notes), ref notes, value);
            }
        }
        private Department department;
        [Association("Department-Contacts")]
        public Department Department {
            get {
                return department;
            }
            set {
                SetPropertyValue(nameof(Department), ref department, value);
                if(!IsLoading) {
                    Position = null;
                    if(Manager != null && Manager.Department != value) {
                        Manager = null;
                    }
                }
            }
        }
        private Position position;
        public Position Position {
            get {
                return position;
            }
            set {
                SetPropertyValue(nameof(Position), ref position, value);
            }
        }
        [Association("Contact-DemoTask")]
        public XPCollection<DemoTask> Tasks {
            get {
                return GetCollection<DemoTask>(nameof(Tasks));
            }
        }

        private Location location;

        [Aggregated]
        [System.ComponentModel.Browsable(false)]
        public Location Location {
            get {
                return location;
            }
            set {
                SetPropertyValue(nameof(Location), ref location, value);
            }
        }
        [PersistentAlias("FullName")]
        [System.ComponentModel.Browsable(false)]
        public string Title {
            get { return Convert.ToString(EvaluateAlias(nameof(Title))); }
        }
        [System.ComponentModel.Browsable(false)]
        [PersistentAlias("Location.Latitude")]
        public double Latitude {
            get { return Convert.ToDouble(EvaluateAlias(nameof(Latitude))); }
        }
        [System.ComponentModel.Browsable(false)]
        [PersistentAlias("Location.Longitude")]
        public double Longitude {
            get { return Convert.ToDouble(EvaluateAlias(nameof(Longitude))); }
        }

        IDepartment IContact.Department => Department;

        public override void AfterConstruction() {
            base.AfterConstruction();

            location = new Location(Session);
            location.Contact = this;
        }

        void IContact.SetDepartment(IDepartment department) {
            Department = (Department)department;
        }
        void IContact.AddTasks(IList<ITask> tasks) {
            Tasks.AddRange(tasks.Cast<DemoTask>());
        }
        void IContact.SetPosition(IPosition position) {
            Position = (Position)position;
        }
        void IContact.SetAddress1(IAddress address) {
            Address1 = (Address)address;
        }
    }
}
