Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Linq
Imports System.Web
Imports System.Web.Optimization
Imports System.Web.Routing
Imports System.Web.Security
Imports System.Web.SessionState
Imports DatabaseUpdater
Imports DevExpress.ExpressApp.Xpo

Namespace WebFormsApplication
	Public Class [Global]
		Inherits HttpApplication

		Private Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
			' Code that runs on application startup
			RouteConfig.RegisterRoutes(RouteTable.Routes)
			BundleConfig.RegisterBundles(BundleTable.Bundles)

			Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			CreateDemoData(connectionString)
		End Sub
		Private Shared Sub CreateDemoData(ByVal connectionString As String)
			Using objectSpaceProvider = New XPObjectSpaceProvider(connectionString)
				ConnectionHelper.RegisterEntities(objectSpaceProvider)
				Using objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(True)
					Dim updater As Updater = New Updater(objectSpace)
					updater.UpdateDatabase()
				End Using
			End Using
		End Sub
	End Class
End Namespace