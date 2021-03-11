using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace XAFSecurityBenchmark.Models.EFCore {
    [System.ComponentModel.DefaultProperty(nameof(Latitude))]
    public class Location : INotifyPropertyChanged {
        private Contact contact;
        private double latitude;
        private double longitude;
        private int id;
        private int contactRef;

        public override string ToString() {
            string latitudePrefix = Latitude > 0 ? "N" : "S";
            string longitudePrefix = Longitude > 0 ? "E" : "W";
            return string.Format("{0}{1:0.###}, {2}{3:0.###}", latitudePrefix, Math.Abs(Latitude), longitudePrefix, Math.Abs(Longitude));
        }
        [Key]
        public int ID {
            get { return id; }
            protected set { id = value; }
        }
        public int ContactRef {
            get {
                return contactRef;
            }
            set {
                SetPropertyValue(ref contactRef, value);
            }
        }

        [Browsable(false)]
        public virtual Contact Contact {
            get {
                return contact;
            }
            set {
                SetReferencePropertyValue(ref contact, value);
            }
        }

        [NotMapped]
        public string Title {
            get { return Contact.FullName; }
        }

        public double Latitude {
            get {
                return latitude;
            }
            set {
                SetPropertyValue(ref latitude, value);
            }
        }

        public double Longitude {
            get {
                return longitude;
            }
            set {
                SetPropertyValue(ref longitude, value);
            }
        }
        #region INotifyPropertyChanged
        private PropertyChangedEventHandler propertyChanged;
        protected bool SetPropertyValue<T>(ref T propertyValue, T newValue, [CallerMemberName] string propertyName = null) where T : struct {
            if(EqualityComparer<T>.Default.Equals(propertyValue, newValue)) {
                return false;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected bool SetPropertyValue<T>(ref T? propertyValue, T? newValue, [CallerMemberName] string propertyName = null) where T : struct {
            if(EqualityComparer<T?>.Default.Equals(propertyValue, newValue)) {
                return false;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected bool SetPropertyValue(ref string propertyValue, string newValue, [CallerMemberName] string propertyName = null) {
            if(propertyValue == newValue) {
                return false;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected bool SetReferencePropertyValue<T>(ref T propertyValue, T newValue, [CallerMemberName] string propertyName = null) where T : class {
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
