using System.Collections.Generic;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.Base.Enums;

namespace XAFSecurityBenchmark.Models.Base {
    public interface IDemoTask : ITask {
        int ActualWorkHours { get; set; }
        int EstimatedWorkHours { get; set; }
        Priority Priority { get; set; }

        void SetAssignedTo(IContact contact);
    }
}
