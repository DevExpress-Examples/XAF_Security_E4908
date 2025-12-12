using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace XAFSecurityBenchmark.Models.Base {
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
    public interface ITask {
        void MarkCompleted();
        string Subject { get;set;}
        string Description { get;set;}
        DateTime DueDate { get;set;}
        DateTime StartDate { get;set;}
        TaskStatus Status { get;set;}
        Int32 PercentCompleted { get;set;}
        DateTime DateCompleted { get;}
    }
}
