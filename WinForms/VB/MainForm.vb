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
            Security.Logoff()
            Hide()
            ShowLoginForm()
        End Sub
        Private Sub ShowLoginForm()
            Using loginForm As New LoginForm(Security, ObjectSpaceProvider)
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
