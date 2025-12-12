using System;

namespace BusinessObjectsLibrary.BusinessObjects {
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
