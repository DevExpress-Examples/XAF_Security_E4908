Imports DevExpress.ExpressApp
Imports System
Imports System.Windows.Forms
Imports DevExpress.ExpressApp.Security
Imports DevExpress.XtraEditors

Namespace WindowsFormsApplication
	Partial Public Class LoginForm
		Inherits DevExpress.XtraEditors.XtraForm

		Private security As SecurityStrategyComplex
		Private objectSpaceProvider As IObjectSpaceProvider
		Public Sub New(ByVal security As SecurityStrategyComplex, ByVal objectSpaceProvider As IObjectSpaceProvider)
			InitializeComponent()
			Me.security = security
			Me.objectSpaceProvider = objectSpaceProvider
			Me.userNameEdit.Text = "User"
		End Sub
		Private Sub Login_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonLogin.Click
			Dim logonObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
			Dim userName As String = userNameEdit.Text
			Dim password As String = passwordEdit.Text
			security.Authentication.SetLogonParameters(New AuthenticationStandardLogonParameters(userName, password))
			Try
				security.Logon(logonObjectSpace)
				DialogResult = DialogResult.OK
				Close()
			Catch ex As Exception
				XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End Try
		End Sub
		Private Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonCancel.Click
			Application.Exit()
		End Sub

		Private Sub UserNameEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles userNameEdit.Validating
			Dim message As String = If(String.IsNullOrEmpty(userNameEdit.Text), "The user name must not be empty. Try Admin or User.", String.Empty)
			dxErrorProvider1.SetError(userNameEdit, message)
		End Sub
	End Class
End Namespace
