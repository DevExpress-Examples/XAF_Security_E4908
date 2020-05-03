using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;

namespace BusinessObjectsLibrary.EFCore.BusinessObjects {
    [DefaultProperty(nameof(Name))]
    public class Country : ICountry, INotifyPropertyChanged {
        private Int32 id;
        private String name;
        private String phoneCode;
        private IList<Address> addresses;

        public Country() {
            Addresses = new List<Address>();
        }
        [Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Int32 ID {
            get { return id; }
            protected set { id = value; }
        }
        public String Name {
            get { return name; }
            set { SetPropertyValue(ref name, value); }
        }
        public String PhoneCode {
            get { return phoneCode; }
            set { SetPropertyValue(ref phoneCode, value); }
        }
        public virtual IList<Address> Addresses {
            get { return addresses; }
            set { SetReferencePropertyValue(ref addresses, value); }
        }

        public override String ToString() {
            return Name;
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
}
