using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;

namespace BusinessObjectsLibrary.EFCore.BusinessObjects {
    [DefaultProperty(nameof(FullAddress))]
    public class Address : IAddress, INotifyPropertyChanged {
        private const string defaultFullAddressFormat = "{Country.Name}; {StateProvince}; {City}; {Street}; {ZipPostal}";
        private Int32 id;
        private String street;
        private String city;
        private String stateProvince;
        private String zipPostal;
        private Country country;
        private IList<Party> parties1;
        private IList<Party> parties2;

        public Address() {
            Parties1 = new List<Party>();
            Parties2 = new List<Party>();
        }
        [Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Int32 ID {
            get { return id; }
            protected set { id = value; }
        }
        public String Street {
            get { return street; }
            set { SetPropertyValue(ref street, value); }
        }
        public String City {
            get { return city; }
            set { SetPropertyValue(ref city, value); }
        }
        public String StateProvince {
            get { return stateProvince; }
            set { SetPropertyValue(ref stateProvince, value); }
        }
        public String ZipPostal {
            get { return zipPostal; }
            set { SetPropertyValue(ref zipPostal, value); }
        }
        public virtual Country Country {
            get { return country; }
            set { SetReferencePropertyValue(ref country, value); }
        }
        [InverseProperty(nameof(Party.Address1)), Browsable(false)]
        public virtual IList<Party> Parties1 {
            get { return parties1; }
            set { SetReferencePropertyValue(ref parties1, value); }
        }
        [InverseProperty(nameof(Party.Address2)), Browsable(false)]
        public virtual IList<Party> Parties2 {
            get { return parties2; }
            set { SetReferencePropertyValue(ref parties2, value); }
        }

        [NotMapped]
        public String FullAddress {
            get { return ObjectFormatter.Format(AddressImpl.FullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
        }

        ICountry IAddress.Country {
            get { return Country; }
            set { Country = value as Country; }
        }

        static Address() {
            AddressImpl.FullAddressFormat = defaultFullAddressFormat;
        }

        public static void SetFullAddressFormat(String format) {
            AddressImpl.FullAddressFormat = format;
        }

        #region INotifyPropertyChanged
        private PropertyChangedEventHandler propertyChanged;
        protected bool SetPropertyValue<T>(ref T propertyValue, T newValue, [CallerMemberName]string propertyName = null) where T : struct {
            if(EqualityComparer<T>.Default.Equals(propertyValue, newValue)) {
                return false;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected bool SetPropertyValue<T>(ref T? propertyValue, T? newValue, [CallerMemberName]string propertyName = null) where T : struct {
            if(EqualityComparer<T?>.Default.Equals(propertyValue, newValue)) {
                return false;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected bool SetPropertyValue(ref string propertyValue, string newValue, [CallerMemberName]string propertyName = null) {
            if(propertyValue == newValue) {
                return false;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected bool SetReferencePropertyValue<T>(ref T propertyValue, T newValue, [CallerMemberName]string propertyName = null) where T : class {
            if(propertyValue == newValue) {
                return false;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string propertyName) {
            if(propertyChanged != null) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }
        #endregion
    }
    public interface IAddress {
        string Street { get; set; }
        string City { get; set; }
        string StateProvince { get; set; }
        string ZipPostal { get; set; }
        ICountry Country { get; set; }
        string FullAddress { get; }
    }
}
