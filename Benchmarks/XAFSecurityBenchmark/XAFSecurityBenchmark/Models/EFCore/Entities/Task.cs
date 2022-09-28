using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations;
using DevExpress.Persistent.Base.General;

namespace XAFSecurityBenchmark.Models.EFCore {

    public class Task : ITask, IXafEntityObject, IObjectSpaceLink {

        [Key]
        public virtual int ID { get; set; }

        public virtual DateTime? DateCompleted { get; set; }
        public virtual String Subject { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual String Description { get; set; }

        public virtual DateTime? DueDate { get; set; }
        public virtual DateTime? StartDate { get; set; }

        [Browsable(false)]
        [NotMapped]
        public int Status_Int { get; set; }

        public virtual int PercentCompleted { get; set; }
        public virtual Party AssignedTo { get; set; }

        private TaskStatus status;

        [Column(nameof(Status_Int))]
        public virtual TaskStatus Status {
            get {
                return status;
            }
            set {
                status = value;
                if (isLoaded) {
                    if (value == TaskStatus.Completed) {
                        DateCompleted = DateTime.Now;
                    } else {
                        DateCompleted = null;
                    }
                }
            }
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted() {
            Status = TaskStatus.Completed;
        }

        #region ITask

        DateTime ITask.DueDate { get { return DueDate.GetValueOrDefault(); } set { DueDate = value; } }
        DateTime ITask.StartDate { get { return StartDate.GetValueOrDefault(); } set { StartDate = value; } }
        TaskStatus ITask.Status { get { return Status; } set { Status = value; } }
        DateTime ITask.DateCompleted { get { return DateCompleted.GetValueOrDefault(); } }

        #endregion

        #region IXafEntityObject

        private bool isLoaded = false;
        public virtual void OnCreated() { }
        public virtual void OnSaving() { }
        public virtual void OnLoaded() {
            isLoaded = true;
        }

        #endregion

        #region IObjectSpaceLink

        IObjectSpace objectSpace;
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get { return objectSpace; }
            set { objectSpace = value; }
        }        

        #endregion
    }
}
