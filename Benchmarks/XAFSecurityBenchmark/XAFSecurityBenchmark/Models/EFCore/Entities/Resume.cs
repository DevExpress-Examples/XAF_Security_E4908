using System;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class Resume {
        public Resume() {
            Portfolio = new List<PortfolioFileData>();
        }
        public Int32 ID { get; protected set; }

        public virtual IList<PortfolioFileData> Portfolio { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual FileData File { get; set; }
    }
}
