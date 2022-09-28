using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.Persistent.Base.General;

namespace XAFSecurityBenchmark.Models.EFCore {

    [DefaultProperty(nameof(Number))]
    public class PhoneNumber : IPhoneNumber {

        [Key]
        public virtual int ID { get; set; }

        public virtual String Number { get; set; }
        public virtual String PhoneType { get; set; }
        public virtual Party Party { get; set; }

        public override String ToString() {
            return Number;
        }
    }
}
