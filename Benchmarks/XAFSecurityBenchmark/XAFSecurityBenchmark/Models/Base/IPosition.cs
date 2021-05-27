using System;
using System.Collections.Generic;
using System.Text;

namespace XAFSecurityBenchmark.Models.Base {
    public interface IPosition {
        string Title { get; set; }

        void AddDepartment(IDepartment department);
    }
}
