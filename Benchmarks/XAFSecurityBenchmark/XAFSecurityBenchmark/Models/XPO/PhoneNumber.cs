using System;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.XPO {
    [DefaultProperty(nameof(Number))]
    public class PhoneNumber : BaseObject, IPhoneNumber {
        private PhoneNumberImpl phone = new PhoneNumberImpl();
        private Party party = null;

        public PhoneNumber(Session session) : base(session) { }

        public override string ToString() {
            return Number;
        }
        [Persistent]
        public string Number {
            get { return phone.Number; }
            set {
                string oldValue = phone.Number;
                phone.Number = value;
                OnChanged(nameof(Number), oldValue, phone.Number);
            }
        }
        [Association("Party-PhoneNumbers")]
        public Party Party {
            get { return party; }
            set { SetPropertyValue(nameof(Party), ref party, value); }
        }
        public string PhoneType {
            get { return phone.PhoneType; }
            set {
                string oldValue = phone.PhoneType;
                phone.PhoneType = value;
                OnChanged(nameof(PhoneType), oldValue, phone.PhoneType);
            }
        }
    }

    public class PhoneType : BaseObject {
        public PhoneType(Session session) : base(session) { }
        private string typeName;
        public string TypeName {
            get { return typeName; }
            set { SetPropertyValue(nameof(TypeName), ref typeName, value); }
        }
    }
}
