using System;

namespace XAFSecurityBenchmark.Models.Base {
    public class PhoneNumberImpl {
        private string number;
        public string Number {
            get { return number; }
            set { number = value; }
        }
        private string phoneType;
        public string PhoneType {
            get { return phoneType; }
            set { phoneType = value; }
        }
    }
}
