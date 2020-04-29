using DevExpress.ExpressApp.Editors;
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
	public class RepositoryItemProtectedContentTextEdit : RepositoryItemTextEdit {
		internal const string EditorName = "ProtectedContentEdit";
		internal static void Register() {
			if(!EditorRegistrationInfo.Default.Editors.Contains(EditorName)) {
				EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(ProtectedContentEdit),
					typeof(RepositoryItemProtectedContentTextEdit),
					typeof(TextEditViewInfo), new TextEditPainter(), true, EditImageIndexes.TextEdit,
					typeof(DevExpress.Accessibility.TextEditAccessible)));
			}
		}
		static RepositoryItemProtectedContentTextEdit() {
			Register();
		}
		public override string GetDisplayText(DevExpress.Utils.FormatInfo format, object editValue) {
			return ProtectedContentText;
		}
		public RepositoryItemProtectedContentTextEdit() {
			ExportMode = ExportMode.DisplayText;
		}
		public override void Assign(RepositoryItem item) {
			base.Assign(item);
			ProtectedContentText = ((RepositoryItemProtectedContentTextEdit)item).ProtectedContentText;
		}
		public override bool ReadOnly {
			get { return true; }
			set {; }
		}
		public string ProtectedContentText { get; set; } = EditorsFactory.ProtectedContentDefaultText;
		public override string EditorTypeName { get { return EditorName; } }
	}
}
