using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;

namespace XAFSecurityBenchmark.Models.EFCore {

    [DefaultProperty(nameof(Name))]
    public class Country : ICountry {

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
