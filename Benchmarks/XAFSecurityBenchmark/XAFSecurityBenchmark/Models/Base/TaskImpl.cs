using System;
using DevExpress.Persistent.Base;

namespace XAFSecurityBenchmark.Models.Base {
    public class TaskImpl {
        private bool isLoading;
		private string subject;
		private string description;
		private DateTime dueDate;
		private DateTime startDate;
		private TaskStatus status = TaskStatus.NotStarted;
		private Int32 percentCompleted;
		private DateTime dateCompleted;
		private void CheckDateCompleted() {
			if(Status == TaskStatus.Completed) {
				dateCompleted = DateTime.Now;
			}
			else {
				dateCompleted = DateTime.MinValue;
			}
		}

        public void MarkCompleted() {
			Status = TaskStatus.Completed;
		}

		public string Subject {
			get { return subject; }
			set { subject = value; }
		}
		public string Description {
			get { return description; }
			set { description = value; }
		}
		public DateTime DueDate {
			get { return dueDate; }
			set { dueDate = value; }
		}
		public DateTime StartDate {
			get { return startDate; }
			set { startDate = value; }
		}
		public TaskStatus Status {
			get { return status; }
			set {
				if(status != value) {
					status = value;
					if(IsLoading)
						return;

					switch(status) {
						case TaskStatus.NotStarted:
							percentCompleted = 0;
							break;
						case TaskStatus.Completed:
							percentCompleted = 100;
							break;
						case TaskStatus.InProgress:
							if(percentCompleted == 100)
								percentCompleted = 75;
							break;
						case TaskStatus.WaitingForSomeoneElse:
						case TaskStatus.Deferred:
							if(percentCompleted == 100)
								percentCompleted = 0;
							break;
					}
					CheckDateCompleted();
				}
			}
		}
		public Int32 PercentCompleted {
			get { return percentCompleted; }
			set {
				if(percentCompleted != value) {
					percentCompleted = value;

					if(IsLoading)
						return;
					if(percentCompleted == 100)
						status = TaskStatus.Completed;
					if(percentCompleted == 0)
						status = TaskStatus.NotStarted;
					if((percentCompleted > 0) && (percentCompleted < 100))
						status = TaskStatus.InProgress;
					CheckDateCompleted();
				}
			}
		}
		public DateTime DateCompleted {
			get { return dateCompleted; }
			set {
				if(isLoading) {
					dateCompleted = value;
				}
			}
		}
        public bool IsLoading {
            get { return isLoading; }
            set { isLoading = value; }
        }

    }
}
