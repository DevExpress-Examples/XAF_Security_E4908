using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Data.Extensions;
using System.ComponentModel.DataAnnotations;
using DevExpress.Persistent.BaseImpl.EF;
using XAFSecurityBenchmark.Models.Base;
using DevExpress.Persistent.Base.General;
using System.Linq;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class Contact : Person, IContact {
        private int titleOfCourtesyInt;
        public Contact() {
            Resumes = new List<Resume>();
            Contacts = new List<Contact>();
            Tasks = new List<DemoTask>();
        }
        public String WebPageAddress { get; set; }
        public String NickName { get; set; }
        public String SpouseName { get; set; }
        [Browsable(false)]
        public Int32 TitleOfCourtesy_Int {
            get { return titleOfCourtesyInt; }
            protected set { SetPropertyValue(ref titleOfCourtesyInt, value); }
        }
        public Nullable<DateTime> Anniversary { get; set; }
        [StringLength(4096)]
        public String Notes { get; set; }
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
        public virtual IList<Resume> Resumes { get; set; }
        public virtual Contact Manager { get; set; }
        public virtual IList<Contact> Contacts { get; set; }
        public virtual IList<DemoTask> Tasks { get; set; }

        [NotMapped]
        public Base.Enums.TitleOfCourtesy TitleOfCourtesy {
            get { return (Base.Enums.TitleOfCourtesy)TitleOfCourtesy_Int; }
            set { TitleOfCourtesy_Int = (Int32)value; }
        }
        private Location location;
        public virtual Location Location {
            get {
                return location;
            }
            set {
                SetReferencePropertyValue(ref location, value);
            }
        }

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

