using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {

    [DefaultProperty(nameof(FullAddress))]
    public class Address : BaseObject, IAddress {
        public virtual String Street { get; set; }
        public virtual String City { get; set; }
        public virtual String StateProvince { get; set; }
        public virtual String ZipPostal { get; set; }
        public virtual Country Country { get; set; }

        [InverseProperty(nameof(Party.Address1)), Browsable(false)]
        public virtual IList<Party> Parties1 { get; set; } = new ObservableCollection<Party>();

        [InverseProperty(nameof(Party.Address2)), Browsable(false)]
        public virtual IList<Party> Parties2 { get; set; } = new ObservableCollection<Party>();

        [NotMapped]
        public String FullAddress {
            get { return $"{Country.Name}; {StateProvince}; {City}; {Street}; {ZipPostal}"; }
        }

        ICountry IAddress.Country { get { return Country; } set { Country = (Country)value; } }
    }
}
