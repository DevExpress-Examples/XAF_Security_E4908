using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using XAFSecurityBenchmark.Models.Base;
using DevExpress.Persistent.Base.General;
using System.Linq;
using DevExpress.Data.XtraReports.Native;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class Contact : Person, IContact {
        public virtual String WebPageAddress { get; set; }
        public virtual String NickName { get; set; }
        public virtual String SpouseName { get; set; }

        [Browsable(false)]
        public virtual Int32 TitleOfCourtesy_Int { get; set; }
        public virtual DateTime? Anniversary { get; set; }

        [StringLength(4096)]
        public virtual String Notes { get; set; }

        private Department department;
        public virtual Department Department {
            get {
                return department;
            }
            set {
                department = value;
                Position = null;
                if(Manager != null && Manager.Department != value) {
                    Manager = null;
                }
            }
        }

        public virtual Position Position { get; set; }
        public virtual IList<Resume> Resumes { get; set; } = new ObservableRangeCollection<Resume>();
        public virtual Contact Manager { get; set; }
        public virtual IList<Contact> Contacts { get; set; } = new ObservableRangeCollection<Contact>();
        public virtual IList<DemoTask> Tasks { get; set; } = new ObservableRangeCollection<DemoTask>();

        [NotMapped]
        public Base.Enums.TitleOfCourtesy TitleOfCourtesy {
            get { return (Base.Enums.TitleOfCourtesy)TitleOfCourtesy_Int; }
            set { TitleOfCourtesy_Int = (Int32)value; }
        }

        public virtual Location Location { get; set; }

        IDepartment IContact.Department => Department;

        void IContact.SetDepartment(IDepartment department) {
            Department = (Department)department;
        }
        void IContact.AddTasks(IList<ITask> tasks) {
            foreach(var task in tasks.Cast<DemoTask>()) {
                Tasks.Add(task);
            }
        }
        void IContact.SetPosition(IPosition position) {
            Position = (Position)position;
        }
        void IContact.SetAddress1(IAddress address) {
            Address1 = (Address)address;
        }
    }
}

