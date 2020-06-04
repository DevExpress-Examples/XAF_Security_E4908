Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Routing
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Microsoft.AspNet.FriendlyUrls.Resolvers

Namespace WebFormsApplication
    Partial Public Class ViewSwitcher
        Inherits System.Web.UI.UserControl

        Private privateCurrentView As String
        Protected Property CurrentView() As String
            Get
                Return privateCurrentView
            End Get
            Private Set(ByVal value As String)
                privateCurrentView = value
            End Set
        End Property

        Private privateAlternateView As String
        Protected Property AlternateView() As String
            Get
                Return privateAlternateView
            End Get
            Private Set(ByVal value As String)
                privateAlternateView = value
            End Set
        End Property

        Private privateSwitchUrl As String
        Protected Property SwitchUrl() As String
            Get
                Return privateSwitchUrl
            End Get
            Private Set(ByVal value As String)
                privateSwitchUrl = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            ' Determine current view
            Dim isMobile = WebFormsFriendlyUrlResolver.IsMobileView(New HttpContextWrapper(Context))
            CurrentView = If(isMobile, "Mobile", "Desktop")

            ' Determine alternate view
            AlternateView = If(isMobile, "Desktop", "Mobile")

            ' Create switch URL from the route, e.g. ~/__FriendlyUrls_SwitchView/Mobile?ReturnUrl=/Page
            Dim switchViewRouteName = "AspNet.FriendlyUrls.SwitchView"
            Dim switchViewRoute = RouteTable.Routes(switchViewRouteName)
            If switchViewRoute Is Nothing Then
                ' Friendly URLs is not enabled or the name of the switch view route is out of sync
                Me.Visible = False
                Return
            End If
            Dim url = GetRouteUrl(switchViewRouteName, New With {Key .view = AlternateView, Key .__FriendlyUrls_SwitchViews = True})
            url &= "?ReturnUrl=" & HttpUtility.UrlEncode(Request.RawUrl)
            SwitchUrl = url
        End Sub
    End Class
End Namespace