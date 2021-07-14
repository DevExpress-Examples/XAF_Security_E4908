Imports BusinessObjectsLibrary
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.ClientServer
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.BaseImpl.PermissionPolicy
Imports System
Imports System.Configuration
Imports System.Linq
Imports System.Windows.Forms

Namespace WindowsFormsApplication
    Friend NotInheritable Class Program

        Private Sub New()
        End Sub

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread>
        Shared Sub Main()
            RegisterEntities()
            Dim authentication As New AuthenticationStandard()
            Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), authentication)
            security.RegisterXPOAdapterProviders()
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            Dim objectSpaceProvider As IObjectSpaceProvider = New SecuredObjectSpaceProvider(security, connectionString, Nothing)

            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Dim mainForm As New MainForm(security, objectSpaceProvider)
            Application.Run(mainForm)
        End Sub
        Private Shared Sub RegisterEntities()
            XpoTypesInfoHelper.GetXpoTypeInfoSource()
            XafTypesInfo.Instance.RegisterEntity(GetType(Employee))
            XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyUser))
            XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyRole))
        End Sub
    End Class
End Namespace
