using System;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {

    [DefaultProperty(nameof(Number))]
    public class PhoneNumber : BaseObject, IPhoneNumber {
        public virtual String Number { get; set; }
        public virtual String PhoneType { get; set; }
        public virtual Party Party { get; set; }

        public override String ToString() {
            return Number;
        }
    }
}
