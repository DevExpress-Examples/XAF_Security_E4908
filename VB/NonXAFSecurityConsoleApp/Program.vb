Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.ClientServer
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.BaseImpl.PermissionPolicy
Imports MainDemo.Module.BusinessObjects
Imports System
Imports System.Configuration
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace NonXAFSecurityConsoleApp
	Friend Class Program
		Shared Sub Main()
			RegisterEntities()
			Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			Dim auth As New AuthenticationStandard()
			Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), auth)
			security.RegisterXPOAdapterProviders()
			Dim osProvider As New SecuredObjectSpaceProvider(security, connectionString, Nothing)

			Dim userName As String = "John"
			Dim password As String = ""
			auth.SetLogonParameters(New AuthenticationStandardLogonParameters(userName, password))
			security.Logon(osProvider.CreateNonsecuredObjectSpace())
			Using file As New StreamWriter("result.txt", False)
				Dim stringBuilderb As New StringBuilder()
				stringBuilderb.Append(String.Format("{0} is logged on." & vbLf, userName))
				Dim securedObjectSpace As IObjectSpace = osProvider.CreateObjectSpace()
				stringBuilderb.Append("List of the 'Contact' objects:" & vbLf)
				For Each contact As Contact In securedObjectSpace.GetObjects(Of Contact)()
					stringBuilderb.Append("=========================================" & vbLf)
					stringBuilderb.Append(String.Format("Full name: {0}" & vbLf, contact.FullName))
					If security.IsGranted(New PermissionRequest(securedObjectSpace, GetType(Contact), SecurityOperations.Read, contact, "Department")) Then
						stringBuilderb.Append(String.Format("Department: {0}" & vbLf, contact.Department.Title))
					Else
						stringBuilderb.Append("Department: [Protected content]" & vbLf)
					End If
				Next contact
				file.Write(stringBuilderb)
			End Using
		End Sub
		Private Shared Sub RegisterEntities()
			XpoTypesInfoHelper.GetXpoTypeInfoSource()
			XafTypesInfo.Instance.RegisterEntity(GetType(Contact))
			XafTypesInfo.Instance.RegisterEntity(GetType(Person))
			XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyUser))
			XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyRole))
		End Sub
	End Class
End Namespace