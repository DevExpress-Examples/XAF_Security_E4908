using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class Resume {
        public virtual Int32 ID { get; set; }
        public virtual IList<PortfolioFileData> Portfolio { get; set; } = new ObservableCollection<PortfolioFileData>();
        public virtual Contact Contact { get; set; }
        public virtual FileData File { get; set; }
    }
}
