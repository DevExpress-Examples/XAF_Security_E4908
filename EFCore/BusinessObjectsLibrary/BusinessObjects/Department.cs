using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessObjectsLibrary.EFCore.BusinessObjects {
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("Title")]
    public class Department : INotifyPropertyChanged {
        private string title;
        private string office;
        private IList<Employee> employees = new List<Employee>();
        private int id;
        //[Browsable(false)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Int32 ID {
            get { return id; }
            protected set { id = value; }
        }
        public string Title {
            get {
                return title;
            }
            set {
                SetPropertyValue(ref title, value);
            }
        }
        public string Office {
            get {
                return office;
            }
            set {
                SetReferencePropertyValue(ref office, value);
            }
        }
        public virtual IList<Employee> Employees {
            get { return employees; }
            set { SetReferencePropertyValue(ref employees, value); }
        }
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
    }
}
