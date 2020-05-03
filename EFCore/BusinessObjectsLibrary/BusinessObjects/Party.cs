using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace BusinessObjectsLibrary.EFCore.BusinessObjects {
    [DefaultProperty(nameof(DisplayName))]
    public abstract class Party: INotifyPropertyChanged {
        private Int32 id;
        private Byte[] photo;
        private IList<PhoneNumber> phoneNumbers;
        private Address address1;
        private Address address2;
        private IList<Task> assignedTasks;

        public Party() {
            PhoneNumbers = new List<PhoneNumber>();
            AssignedTasks = new List<Task>();
        }
        [Key]
        //[Browsable(false)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Int32 ID {
            get { return id; }
            protected set { id = value; }
        }
        [ImageEditor]
        public Byte[] Photo {
            get { return photo; }
            set { SetReferencePropertyValue(ref photo, value); }
        }
        [Aggregated]
        public virtual IList<PhoneNumber> PhoneNumbers {
            get { return phoneNumbers; }
            set { SetReferencePropertyValue(ref phoneNumbers, value); }
        }
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public virtual Address Address1 {
            get { return address1; }
            set { SetReferencePropertyValue(ref address1, value); }
        }
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public virtual Address Address2 {
            get { return address2; }
            set { SetReferencePropertyValue(ref address2, value); }
        }
        public virtual IList<Task> AssignedTasks {
            get { return assignedTasks; }
            set { SetReferencePropertyValue(ref assignedTasks, value); }
        }
        [NotMapped]
        public abstract String DisplayName { get; }
        public override String ToString() {
            return DisplayName;
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
