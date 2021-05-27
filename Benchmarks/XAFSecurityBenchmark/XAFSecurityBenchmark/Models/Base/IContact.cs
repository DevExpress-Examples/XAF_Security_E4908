using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.Base.General;
using XAFSecurityBenchmark.Models.Base.Enums;

namespace XAFSecurityBenchmark.Models.Base {
    public interface IContact : IPerson {
        TitleOfCourtesy TitleOfCourtesy { get; set; }
        IDepartment Department { get; }
        DateTime? Anniversary { get; set; }

        void SetDepartment(IDepartment department);
        void AddTasks(IList<ITask> tasks);
        void SetPosition(IPosition position);
        void SetAddress1(IAddress address);
    }
}
