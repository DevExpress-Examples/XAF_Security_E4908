using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjectsLibrary.BusinessObjects {

    [DefaultProperty(nameof(Number))]
    public class PhoneNumber {

        [Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual int ID { get; set; }

        public virtual string Number { get; set; }

        public virtual string PhoneType { get; set; }

        public virtual Employee Party { get; set; }

        public override string ToString() {
            return Number;
        }
    }
}
