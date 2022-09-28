using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.Persistent.Base;

namespace BusinessObjectsLibrary.BusinessObjects {

    [DefaultProperty(nameof(FullAddress))]
    public class Address {
        [Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual int ID { get; set; }

        public virtual String Street { get; set; }

        public virtual String City { get; set; }

        public virtual String StateProvince { get; set; }

        public virtual String ZipPostal { get; set; }

        public virtual Country Country { get; set; }

        [InverseProperty(nameof(Employee.Address1)), Browsable(false)]
        public virtual IList<Employee> Parties1 { get; set; } = new ObservableCollection<Employee>();

        [InverseProperty(nameof(Employee.Address2)), Browsable(false)]
        public virtual IList<Employee> Parties2 { get; set; } = new ObservableCollection<Employee>();

        [NotMapped]
        public String FullAddress {
            get { return $"{Country?.Name}; {StateProvince}; {City}; {Street}; {ZipPostal}"; }
        }
    }
}
