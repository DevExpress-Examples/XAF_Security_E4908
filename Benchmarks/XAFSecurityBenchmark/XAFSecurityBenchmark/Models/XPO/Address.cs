using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.XPO {
    [DefaultProperty(nameof(FullAddress))]
    [CalculatedPersistentAliasAttribute(nameof(FullAddress), nameof(FullAddressPersistentAlias))]
    public class Address : BaseObject, IAddress {
        private const string defaultFullAddressFormat = "{Country.Name}; {StateProvince}; {City}; {Street}; {ZipPostal}";
        private const string defaultfullAddressPersistentAlias = "concat(Country.Name,' ', StateProvince, ' ', City, ' ', Street, ' ', ZipPostal)";
        static Address() {
            AddressImpl.FullAddressFormat = defaultFullAddressFormat;
        }
        public Address(Session session) : base(session) { }

        private static string fullAddressPersistentAlias = defaultfullAddressPersistentAlias;
        private AddressImpl address = new AddressImpl();
        public static string FullAddressPersistentAlias {
            get { return fullAddressPersistentAlias; }
        }

        public static void SetFullAddressFormat(string format, string persistentAlias) {
            AddressImpl.FullAddressFormat = format;
            fullAddressPersistentAlias = persistentAlias;
        }

        public string Street {
            get { return address.Street; }
            set {
                string oldValue = address.Street;
                address.Street = value;
                OnChanged(nameof(Street), oldValue, address.Street);
            }
        }
        public string City {
            get { return address.City; }
            set {
                string oldValue = address.City;
                address.City = value;
                OnChanged(nameof(City), oldValue, address.City);
            }
        }
        public string StateProvince {
            get { return address.StateProvince; }
            set {
                string oldValue = address.StateProvince;
                address.StateProvince = value;
                OnChanged(nameof(StateProvince), oldValue, address.StateProvince);
            }
        }
        public string ZipPostal {
            get { return address.ZipPostal; }
            set {
                string oldValue = address.ZipPostal;
                address.ZipPostal = value;
                OnChanged(nameof(ZipPostal), oldValue, address.ZipPostal);
            }
        }
        ICountry IAddress.Country {
            get { return address.Country; }
            set {
                ICountry oldValue = address.Country;
                address.Country = value;
                OnChanged(nameof(Country), oldValue, address.Country);
            }
        }
        public Country Country {
            get { return address.Country as Country; }
            set {
                ICountry oldValue = address.Country;
                address.Country = value as ICountry;
                OnChanged(nameof(Country), oldValue, address.Country);
            }
        }
        public string FullAddress {
            get { return GetFullAddress(); }
        }

        protected virtual string GetFullAddress() {
            return ObjectFormatter.Format(AddressImpl.FullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
        }
    }
}
