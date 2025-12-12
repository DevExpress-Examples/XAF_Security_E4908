
using DevExpress.Persistent.Base;

namespace BusinessObjectsLibrary.BusinessObjects {
    public class AddressImpl {
		private static string fullAddressFormat;

		public static string FullAddressFormat {
			get { return fullAddressFormat; }
			set {
				fullAddressFormat = value;
				//if(string.IsNullOrEmpty(fullAddressFormat)) {
				//	fullAddressFormat = defaultFullAddressFormat;
				//}
			}
		}
        private string street;
        private string city;
        private string stateProvince;
        private string zipPostal;
        private ICountry country;

        public string Street {
            get { return street; }
            set { street = value; }
        }
        public string City {
            get { return city; }
            set { city = value; }
        }
        public string StateProvince {
            get { return stateProvince; }
            set { stateProvince = value; }
        }
        public string ZipPostal {
            get { return zipPostal; }
            set { zipPostal = value; }
        }
        public ICountry Country {
            get { return country; }
            set { country = value; }
        }
        public string FullAddress {
            get {
				return ObjectFormatter.Format(fullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
            }
        }
    }
}
