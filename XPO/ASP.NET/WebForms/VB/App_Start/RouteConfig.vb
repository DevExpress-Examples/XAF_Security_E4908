Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls

Namespace WebFormsApplication
    Public NotInheritable Class RouteConfig

        Private Sub New()
        End Sub

        Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
            Dim settings = New FriendlyUrlSettings()
            settings.AutoRedirectMode = RedirectMode.Permanent
            routes.EnableFriendlyUrls(settings)
        End Sub
    End Class
End Namespace
