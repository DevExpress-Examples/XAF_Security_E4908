Imports DevExpress.ExpressApp.Xpo
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Web

Namespace WebFormsApplication
    Public NotInheritable Class XpoDataStoreProviderService

        Private Sub New()
        End Sub

        Private Shared dataStoreProvider As IXpoDataStoreProvider
        Public Shared Function GetDataStoreProvider(ByVal connection As IDbConnection, ByVal enablePoolingInConnectionString As Boolean) As IXpoDataStoreProvider
            If dataStoreProvider Is Nothing Then
                Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, enablePoolingInConnectionString)
            End If
            Return dataStoreProvider
        End Function
    End Class
End Namespace