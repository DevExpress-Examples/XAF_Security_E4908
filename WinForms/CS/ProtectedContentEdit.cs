using DevExpress.ExpressApp.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication {
	public class ProtectedContentEdit : StringEdit {
		static ProtectedContentEdit() {
			RepositoryItemProtectedContentTextEdit.Register();
		}
		public ProtectedContentEdit() { }
		public override string EditorTypeName { get { return RepositoryItemProtectedContentTextEdit.EditorName; } }
	}
	public class StringEdit : TextEdit {
		static StringEdit() {
			RepositoryItemStringEdit.Register();
		}
		public StringEdit() { }
		public StringEdit(int maxLength) {
			((RepositoryItemStringEdit)Properties).MaxLength = maxLength;
		}
		public override string EditorTypeName { get { return RepositoryItemStringEdit.EditorName; } }
	}
	public class RepositoryItemStringEdit : RepositoryItemTextEdit {
		internal const string EditorName = "StringEdit";
		internal static void Register() {
			if(!EditorRegistrationInfo.Default.Editors.Contains(EditorName)) {
				EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(StringEdit),
					typeof(RepositoryItemStringEdit),
					typeof(TextEditViewInfo), new TextEditPainter(), true, EditImageIndexes.TextEdit,
					typeof(DevExpress.Accessibility.TextEditAccessible)));
			}
		}
		static RepositoryItemStringEdit() {
			Register();
		}

		public override string EditorTypeName { get { return EditorName; } }
		public RepositoryItemStringEdit(int maxLength) : this() {
			MaxLength = maxLength;
		}
		public RepositoryItemStringEdit() {
			Mask.MaskType = MaskType.None;
			if(Mask.MaskType != MaskType.RegEx) {
				Mask.UseMaskAsDisplayFormat = true;
			}
		}
		public void Init(string displayFormat, string editMask, EditMaskType maskType) {
			Init(editMask, maskType);
			if(!string.IsNullOrEmpty(displayFormat)) {
				Mask.UseMaskAsDisplayFormat = false;
				DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
				DisplayFormat.FormatString = displayFormat;
			}
		}
		public void Init(string editMask, EditMaskType maskType) {
			if(!string.IsNullOrEmpty(editMask)) {
				Mask.EditMask = editMask;
				switch(maskType) {
					case EditMaskType.RegEx:
						Mask.UseMaskAsDisplayFormat = false;
						Mask.MaskType = MaskType.RegEx;
						break;
					default:
						Mask.MaskType = MaskType.Simple;
						break;
				}
			}
		}
	}
}
