Imports DevExpress.ExpressApp
Imports System
Imports System.Windows.Forms
Imports DevExpress.ExpressApp.Security

Namespace WindowsFormsApplication
    Partial Public Class LoginForm
        Inherits Form

        Private security As SecurityStrategyComplex
        Private objectSpaceProvider As IObjectSpaceProvider
        Public Sub New(ByVal security As SecurityStrategyComplex, ByVal objectSpaceProvider As IObjectSpaceProvider)
            InitializeComponent()
            Me.security = security
            Me.objectSpaceProvider = objectSpaceProvider
        End Sub
        Private Sub Login_button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles login_button.Click
            Dim logonObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
            Dim userName As String = userNameEdit.Text
            Dim password As String = passwordEdit.Text
            security.Authentication.SetLogonParameters(New AuthenticationStandardLogonParameters(userName, password))
            Try
                security.Logon(logonObjectSpace)
                DialogResult = System.Windows.Forms.DialogResult.OK
                Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
