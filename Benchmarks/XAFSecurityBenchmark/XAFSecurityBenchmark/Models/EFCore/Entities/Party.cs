using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {

    public abstract class Party : BaseObject {

        public virtual Byte[] Photo { get; set; }

        [Aggregated]
        public virtual IList<PhoneNumber> PhoneNumbers { get; set; } = new ObservableCollection<PhoneNumber>();

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public virtual Address Address1 { get; set; }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public virtual Address Address2 { get; set; }

        public abstract String DisplayName { get; }

        public override String ToString() {
            return DisplayName;
        }
    }
}
