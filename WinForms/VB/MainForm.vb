Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports System
Imports System.Linq
Imports System.Windows.Forms

Namespace WindowsFormsApplication
    Partial Public Class MainForm
        Inherits DevExpress.XtraBars.Ribbon.RibbonForm

        Public Property Security() As SecurityStrategyComplex
        Public Property ObjectSpaceProvider() As IObjectSpaceProvider
        Public Sub New(ByVal security As SecurityStrategyComplex, ByVal objectSpaceProvider As IObjectSpaceProvider)
            InitializeComponent()
            Me.Security = security
            Me.ObjectSpaceProvider = objectSpaceProvider
        End Sub
        Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            ShowLoginForm()
        End Sub
        Private Sub LogoffButtonItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles logoffButtonItem.ItemClick
			For Each form As Form In MdiChildren
				form.Close()
			Next form
			Dim userName As String = Security.UserName
			Security.Logoff()
            Hide()
			ShowLoginForm(userName)
		End Sub
		Private Sub ShowLoginForm(Optional ByVal userName As String = "User")
			Using loginForm As New LoginForm(Security, ObjectSpaceProvider, userName)
				Dim dialogResult As DialogResult = loginForm.ShowDialog()
				If dialogResult = System.Windows.Forms.DialogResult.OK Then
					CreateListForm()
					Show()
				Else
					Close()
				End If
			End Using
		End Sub
		Private Sub CreateListForm()
            Dim employeeForm As New EmployeeListForm()
            employeeForm.MdiParent = Me
            employeeForm.WindowState = FormWindowState.Maximized
            employeeForm.Show()
        End Sub
    End Class
End Namespace
