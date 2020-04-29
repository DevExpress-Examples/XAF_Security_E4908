Imports DevExpress.ExpressApp.Editors
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
    Public Class RepositoryItemProtectedContentTextEdit
        Inherits RepositoryItemTextEdit

        Friend Const EditorName As String = "ProtectedContentEdit"

        Friend Shared Sub Register()
            If Not EditorRegistrationInfo.[Default].Editors.Contains(EditorName) Then
                EditorRegistrationInfo.[Default].Editors.Add(New EditorClassInfo(EditorName, GetType(ProtectedContentEdit), GetType(RepositoryItemProtectedContentTextEdit), GetType(TextEditViewInfo), New TextEditPainter(), True, EditImageIndexes.TextEdit, GetType(DevExpress.Accessibility.TextEditAccessible)))
            End If
        End Sub

        Shared Sub New()
            Register()
        End Sub

        Public Overrides Function GetDisplayText(ByVal format As DevExpress.Utils.FormatInfo, ByVal editValue As Object) As String
            Return ProtectedContentText
        End Function

        Public Sub New()
            ExportMode = ExportMode.DisplayText
        End Sub

        Public Overrides Sub Assign(ByVal item As RepositoryItem)
            MyBase.Assign(item)
            ProtectedContentText = (CType(item, RepositoryItemProtectedContentTextEdit)).ProtectedContentText
        End Sub

        Public Overrides Property [ReadOnly] As Boolean
            Get
                Return True
            End Get
            Set(ByVal value As Boolean)
            End Set
        End Property

        Public Property ProtectedContentText As String = EditorsFactory.ProtectedContentDefaultText

        Public Overrides ReadOnly Property EditorTypeName As String
            Get
                Return EditorName
            End Get
        End Property
    End Class
End Namespace
