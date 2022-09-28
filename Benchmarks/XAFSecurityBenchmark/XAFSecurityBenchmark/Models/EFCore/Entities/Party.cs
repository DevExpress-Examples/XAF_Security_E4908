using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations;

namespace XAFSecurityBenchmark.Models.EFCore {

    public abstract class Party {

        [Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual Int32 ID { get; set; }

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
