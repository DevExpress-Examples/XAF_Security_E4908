Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.ClientServer
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace WebFormsApplication
    Partial Public Class LoginForm
        Inherits Page

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
			Dim userName As String = Request.Cookies.Get("userName")?.Value
			If userName Is Nothing Then
				userName = "User"
			End If
			UserNameBox.Text = userName
			LoginButton.Focus()
        End Sub
        Protected Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim userName As String = UserNameBox.Text
            Dim password As String = PasswordBox.Text
            Dim parameters As New AuthenticationStandardLogonParameters(userName, password)
            Dim security As SecurityStrategyComplex = ConnectionHelper.GetSecurity(GetType(AuthenticationStandardProvider).Name, parameters)
            Dim objectSpaceProvider As SecuredObjectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(security)
            Dim logonObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
            Try
                security.Logon(logonObjectSpace)
            Catch
            End Try
            If security.IsAuthenticated Then
                SetCookie(userName)
                FormsAuthentication.RedirectFromLoginPage(userName, True)
            Else
                ClientScript.RegisterStartupScript(Me.GetType(), Nothing, "errorMessage();", True)
            End If
            security.Dispose()
            objectSpaceProvider.Dispose()
        End Sub
        Private Sub SetCookie(ByVal userName As String)
            Dim cookie As New HttpCookie("userName", userName)
            HttpContext.Current.Response.Cookies.Add(cookie)
        End Sub
    End Class
End Namespace