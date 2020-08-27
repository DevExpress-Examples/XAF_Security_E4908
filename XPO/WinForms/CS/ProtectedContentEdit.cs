using DevExpress.Accessibility;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication {
    public class ProtectedContentEdit : TextEdit {
        public ProtectedContentEdit() : base() {
            Enabled = false;
        }
        internal const string EditorName = "ProtectedContentEdit";
        internal const string ProtectedContentText = "*******";
        static ProtectedContentEdit() { RepositoryItemProtectedContentTextEdit.Register(); }
        public override string EditorTypeName => EditorName;
    }

    public class RepositoryItemProtectedContentTextEdit : RepositoryItemTextEdit {
        static RepositoryItemProtectedContentTextEdit() { Register(); }
        public RepositoryItemProtectedContentTextEdit() { ExportMode = ExportMode.DisplayText; }
        internal static void Register() {
            if(!EditorRegistrationInfo.Default.Editors.Contains(ProtectedContentEdit.EditorName)) {
                EditorRegistrationInfo.Default.Editors
                    .Add(new EditorClassInfo(ProtectedContentEdit.EditorName,
                                             typeof(ProtectedContentEdit),
                                             typeof(RepositoryItemProtectedContentTextEdit),
                                             typeof(TextEditViewInfo),
                                             new TextEditPainter(),
                                             designTimeVisible: true,
                                             EditImageIndexes.TextEdit,
                                             typeof(TextEditAccessible)));
            }
        }
        public override string GetDisplayText(FormatInfo format, object editValue) { return ProtectedContentEdit.ProtectedContentText; }
        public override string EditorTypeName => ProtectedContentEdit.EditorName;
        public override bool ReadOnly { get { return true; } set { } }
    }
}
