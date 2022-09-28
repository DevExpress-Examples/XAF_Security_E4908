using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjectsLibrary.BusinessObjects {

    [DefaultProperty(nameof(Name))]
    public class Country {

        [Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual int ID { get; set; }

        public virtual String Name { get; set; }

        public virtual String PhoneCode { get; set; }

        public virtual IList<Address> Addresses { get; set; } = new ObservableCollection<Address>();

        public override String ToString() {
            return Name;
        }
    }
}
