using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;


namespace XAFSecurityBenchmark.Models.XPO {
    [DefaultProperty(nameof(Subject))]
    [ImageName("BO_Task")]
    public class Task : BaseObject, ITask {
        private TaskImpl task = new TaskImpl();
		private Party assignedTo;
#if MediumTrust
        [Persistent("DateCompleted"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime dateCompleted {
            get { return task.DateCompleted; }
            set {
                DateTime oldValue = task.DateCompleted;
                task.DateCompleted = value;
                OnChanged("dateCompleted", oldValue, task.DateCompleted);
            }
        }
#else
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Persistent("DateCompleted")]
        private DateTime dateCompleted {
            get { return task.DateCompleted; }
            set {
                DateTime oldValue = task.DateCompleted;
                task.DateCompleted = value;
                OnChanged(nameof(dateCompleted), oldValue, task.DateCompleted);
            }
        }
#endif
		public Task(Session session) : base(session) { }

        protected override void OnLoading() {
            task.IsLoading = true;
            base.OnLoading();
        }
        protected override void OnLoaded() {
            base.OnLoaded();
			task.DateCompleted = dateCompleted;
            task.IsLoading = false;
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted() {
            TaskStatus oldStatus = task.Status;
            task.MarkCompleted();
            OnChanged(nameof(Status), oldStatus, task.Status);
            OnChanged(nameof(PercentCompleted));
        }

		public string Subject {
			get { return task.Subject; }
			set {
                string oldValue = task.Subject;
                task.Subject = value;
                OnChanged(nameof(Subject), oldValue, task.Subject);
            }
		}
		[Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
		public string Description {
			get { return task.Description; }
			set {
                string oldValue = task.Description;
                task.Description = value;
                OnChanged(nameof(Description), oldValue, task.Description);
            }
		}
		public DateTime DueDate {
			get { return task.DueDate; }
			set {
                DateTime oldValue = task.DueDate;
                task.DueDate = value;
                OnChanged(nameof(DueDate), oldValue, task.DueDate);
            }
		}
		public DateTime StartDate {
			get { return task.StartDate; }
			set {
                DateTime oldValue = task.StartDate;
                task.StartDate = value;
                OnChanged(nameof(StartDate), oldValue, task.StartDate);
            }
		}

		public Party AssignedTo {
			get { return assignedTo; }
            set { SetPropertyValue(nameof(AssignedTo), ref assignedTo, value); }
		}
		public TaskStatus Status {
			get { return task.Status; }
            set {
                TaskStatus oldValue = task.Status;
                task.Status = value;
                OnChanged(nameof(Status), oldValue, task.Status);
            }
		}
		public Int32 PercentCompleted {
			get { return task.PercentCompleted; }
			set {
                Int32 oldValue = task.PercentCompleted;
                task.PercentCompleted = value;
                OnChanged(nameof(PercentCompleted), oldValue, task.PercentCompleted);
            }
		}
		public DateTime DateCompleted {
			get { return dateCompleted; }
		}
	}
}
