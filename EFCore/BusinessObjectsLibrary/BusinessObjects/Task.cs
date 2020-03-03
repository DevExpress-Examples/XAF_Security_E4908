using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects {
    [DefaultProperty(nameof(Subject))]
    public class Task : ITask, IXafEntityObject, INotifyPropertyChanged, IObjectSpaceLink {
        private bool isLoaded = false;
        private Int32 id;
        private DateTime? dateCompleted;
        private String subject;
        private String description;
        private DateTime? dueDate;
        private DateTime? startDate;
        private Int32 percentCompleted;
        private Party assignedTo;
        private TaskStatus status;
        private IObjectSpace objectSpace;

        [Key]
        //[Browsable(false)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Int32 ID {
            get { return id; }
            protected set { id = value; }
        }
        public DateTime? DateCompleted {
            get { return dateCompleted; }
            protected set { SetPropertyValue(ref dateCompleted, value); }
        }
        public String Subject {
            get { return subject; }
            set { SetPropertyValue(ref subject, value); }
        }
        [FieldSize(FieldSizeAttribute.Unlimited)]
        public String Description {
            get { return description; }
            set { SetPropertyValue(ref description, value); }
        }
        public DateTime? DueDate {
            get { return dueDate; }
            set { SetPropertyValue(ref dueDate, value); }
        }
        public DateTime? StartDate {
            get { return startDate; }
            set { SetPropertyValue(ref startDate, value); }
        }
        [Browsable(false)]
        [NotMapped]
        public Int32 Status_Int {
            get { return (Int32)Status; }
            set { Status = (TaskStatus)value; }
        }
        public Int32 PercentCompleted {
            get { return percentCompleted; }
            set { SetPropertyValue(ref percentCompleted, value); }
        }
        public virtual Party AssignedTo {
            get { return assignedTo; }
            set { SetReferencePropertyValue(ref assignedTo, value); }
        }

        [Column(nameof(Status_Int))]
        public TaskStatus Status {
            get {
                return status;
            }
            set {
                status = value;
                if(isLoaded) {
                    if(value == TaskStatus.Completed) {
                        DateCompleted = DateTime.Now;
                    } else {
                        DateCompleted = null;
                    }
                }
            }
        }

        // ITask
        DateTime ITask.DateCompleted {
            get {
                if(DateCompleted.HasValue) {
                    return DateCompleted.Value;
                } else {
                    return DateTime.MinValue;
                }
            }
        }
        DateTime ITask.DueDate {
            get {
                if(DueDate.HasValue) {
                    return DueDate.Value;
                } else {
                    return DateTime.MinValue;
                }
            }
            set { DueDate = value; }
        }
        DateTime ITask.StartDate {
            get {
                if(StartDate.HasValue) {
                    return StartDate.Value;
                } else {
                    return DateTime.MinValue;
                }
            }
            set { StartDate = value; }
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted() {
            Status = TaskStatus.Completed;
        }

        #region IXafEntityObject
        public void OnCreated() { }
        public void OnSaving() { }
        public void OnLoaded() {
            isLoaded = true;
        }
        #endregion

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
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        private void OnPropertyChanged(string propertyName) {
            if((propertyChanged != null) && (objectSpace == null || objectSpace.IsModified || objectSpace.IsDeleting)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }
        #endregion
    }
    public interface ITask {
        void MarkCompleted();
        string Subject { get; set; }
        string Description { get; set; }
        DateTime DueDate { get; set; }
        DateTime StartDate { get; set; }
        TaskStatus Status { get; set; }
        Int32 PercentCompleted { get; set; }
        DateTime DateCompleted { get; }
    }
    public enum TaskStatus {
        [ImageName("State_Task_NotStarted")]
        NotStarted,
        [ImageName("State_Task_InProgress")]
        InProgress,
        [ImageName("State_Task_WaitingForSomeoneElse")]
        WaitingForSomeoneElse,
        [ImageName("State_Task_Deferred")]
        Deferred,
        [ImageName("State_Task_Completed")]
        Completed
    }
}
