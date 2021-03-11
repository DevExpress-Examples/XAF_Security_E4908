using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class PortfolioFileData : FileAttachment {
        public PortfolioFileData()
            : base() {
            DocumentType = DocumentType.Unknown;
        }

        [Browsable(false)]
        public Int32 DocumentType_Int { get; protected set; }

        [Browsable(false)]
        public int ResumeForeignKey { get; set; }

        [Required]
        [ForeignKey(nameof(ResumeForeignKey))]
        public virtual Resume Resume { get; set; }

        [NotMapped]
        public DocumentType DocumentType {
            get { return (DocumentType)DocumentType_Int; }
            set { DocumentType_Int = (Int32)value; }
        }
    }
    public enum DocumentType {
        SourceCode = 1,
        Tests = 2,
        Documentation = 3,
        Diagrams = 4,
        ScreenShots = 5,
        Unknown = 6
    }
}
