using System;
using System.Collections.Generic;
using System.Text;

namespace XAFSecurityBenchmark.Models.Base {
    public interface IDepartment {
        string Title { get; set; }

        void AddPositions(IPosition position);
    }
}
