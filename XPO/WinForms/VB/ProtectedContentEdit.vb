Imports DevExpress.Accessibility
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace WindowsFormsApplication
    Public Class ProtectedContentEdit
        Inherits TextEdit
        Public Sub New()
            MyBase.New()
            Enabled = False
        End Sub
        Friend Const EditorName As String = "ProtectedContentEdit"
        Friend Const ProtectedContentText As String = "Protected Content"
        Shared Sub New()
            RepositoryItemProtectedContentTextEdit.Register()
        End Sub
        Public Overrides ReadOnly Property EditorTypeName As String
            Get
                Return EditorName
            End Get
        End Property
    End Class

    Public Class RepositoryItemProtectedContentTextEdit
        Inherits RepositoryItemTextEdit

        Shared Sub New()
            Register()
        End Sub
        Public Sub New()
            ExportMode = ExportMode.DisplayText
        End Sub
        Friend Shared Sub Register()
            If Not EditorRegistrationInfo.[Default].Editors.Contains(ProtectedContentEdit.EditorName) Then
                EditorRegistrationInfo.[Default].Editors.Add(New EditorClassInfo(ProtectedContentEdit.EditorName, GetType(ProtectedContentEdit), GetType(RepositoryItemProtectedContentTextEdit), GetType(TextEditViewInfo), New TextEditPainter(), designTimeVisible:=True, EditImageIndexes.TextEdit, GetType(TextEditAccessible)))
            End If
        End Sub
        Public Overrides Function GetDisplayText(ByVal format As FormatInfo, ByVal editValue As Object) As String
            Return ProtectedContentEdit.ProtectedContentText
        End Function
        Public Overrides ReadOnly Property EditorTypeName As String
            Get
                Return ProtectedContentEdit.EditorName
            End Get
        End Property
        Public Overrides Property [ReadOnly] As Boolean
            Get
                Return True
            End Get
            Set(ByVal value As Boolean)
            End Set
        End Property
    End Class
End Namespace
