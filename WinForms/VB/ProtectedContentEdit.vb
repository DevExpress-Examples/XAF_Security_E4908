Imports DevExpress.ExpressApp.Editors
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Mask
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
        Inherits StringEdit

        Shared Sub New()
            RepositoryItemProtectedContentTextEdit.Register()
        End Sub
        Public Sub New()
        End Sub
        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return RepositoryItemProtectedContentTextEdit.EditorName
            End Get
        End Property
    End Class
    Public Class StringEdit
        Inherits TextEdit

        Shared Sub New()
            RepositoryItemStringEdit.Register()
        End Sub
        Public Sub New()
        End Sub
        Public Sub New(ByVal maxLength As Integer)
            CType(Properties, RepositoryItemStringEdit).MaxLength = maxLength
        End Sub
        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return RepositoryItemStringEdit.EditorName
            End Get
        End Property
    End Class
    Public Class RepositoryItemStringEdit
        Inherits RepositoryItemTextEdit

        Friend Const EditorName As String = "StringEdit"
        Friend Shared Sub Register()
            If Not EditorRegistrationInfo.Default.Editors.Contains(EditorName) Then
                EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(EditorName, GetType(StringEdit), GetType(RepositoryItemStringEdit), GetType(TextEditViewInfo), New TextEditPainter(), True, EditImageIndexes.TextEdit, GetType(DevExpress.Accessibility.TextEditAccessible)))
            End If
        End Sub
        Shared Sub New()
            Register()
        End Sub

        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return EditorName
            End Get
        End Property
        Public Sub New(ByVal maxLength As Integer)
            Me.New()
            maxLength = maxLength
        End Sub
        Public Sub New()
            Mask.MaskType = MaskType.None
            If Mask.MaskType <> MaskType.RegEx Then
                Mask.UseMaskAsDisplayFormat = True
            End If
        End Sub
        Public Sub Init(ByVal _displayFormat As String, ByVal editMask As String, ByVal maskType As EditMaskType)
            Init(editMask, maskType)
            If Not String.IsNullOrEmpty(_displayFormat) Then
                Mask.UseMaskAsDisplayFormat = False
                DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
                DisplayFormat.FormatString = _displayFormat
            End If
        End Sub
        Public Sub Init(ByVal editMask As String, ByVal _maskType As EditMaskType)
            If Not String.IsNullOrEmpty(editMask) Then
                Mask.EditMask = editMask
                Select Case _maskType
                    Case EditMaskType.RegEx
                        Mask.UseMaskAsDisplayFormat = False
                        Mask.MaskType = MaskType.RegEx
                    Case Else
                        Mask.MaskType = MaskType.Simple
                End Select
            End If
        End Sub
    End Class
End Namespace
