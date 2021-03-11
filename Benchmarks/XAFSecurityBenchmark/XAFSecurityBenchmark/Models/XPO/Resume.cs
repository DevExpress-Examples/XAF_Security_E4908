using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace XAFSecurityBenchmark.Models.XPO {
    public class Resume : BaseObject {
        private Contact contact;
        private FileData file;
        public Resume(Session session)
            : base(session) {
        }
        [Aggregated]
        public FileData File {
            get {
                return file;
            }
            set {
                SetPropertyValue(nameof(File), ref file, value);
            }
        }
        public Contact Contact {
            get {
                return contact;
            }
            set {
                SetPropertyValue(nameof(Contact), ref contact, value);
            }
        }
        [Aggregated, Association("Resume-PortfolioFileData")]
        public XPCollection<PortfolioFileData> Portfolio {
            get {
                return GetCollection<PortfolioFileData>(nameof(Portfolio));
            }
        }
    }
}
