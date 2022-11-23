using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {

    [DefaultProperty(nameof(Name))]
    public class Country : BaseObject, ICountry {
        public virtual String Name { get; set; }
        public virtual String PhoneCode { get; set; }
        public virtual IList<Address> Addresses { get; set; } = new ObservableCollection<Address>();

        public override String ToString() {
            return Name;
        }
    }
}
