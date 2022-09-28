using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class PortfolioFileData : FileAttachment, IXafEntityObject {
        public virtual Int32 DocumentType_Int { get; set; }

        public virtual int ResumeForeignKey { get; set; }

        [Required]
        [ForeignKey(nameof(ResumeForeignKey))]
        public virtual Resume Resume { get; set; }

        [NotMapped]
        public DocumentType DocumentType {
            get { return (DocumentType)DocumentType_Int; }
            set { DocumentType_Int = (Int32)value; }
        }

        #region IXafEntityObject

        public void OnCreated() {
            DocumentType = DocumentType.Unknown;
        }
        public void OnLoaded() { }
        public void OnSaving() { }

        #endregion
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
