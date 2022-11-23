using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class Resume : BaseObject {
        public virtual IList<PortfolioFileData> Portfolio { get; set; } = new ObservableCollection<PortfolioFileData>();
        public virtual Contact Contact { get; set; }
        public virtual FileData File { get; set; }
    }
}
