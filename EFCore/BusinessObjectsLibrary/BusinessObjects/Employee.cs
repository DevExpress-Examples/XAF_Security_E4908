using DevExpress.ExpressApp.DC;
using System;

namespace BusinessObjectsLibrary.BusinessObjects {
    public class Employee {

        public virtual int ID { get; set; }

        public virtual String FirstName { get; set; }

        public virtual String LastName { get; set; }

        public virtual DateTime? Birthday { get; set; }

        [FieldSize(255)]
        public virtual String Email { get; set; }

        public virtual Department Department { get; set; }

        public string FullName {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
