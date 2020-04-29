Imports BusinessObjectsLibrary.EFCore.NetCore
Imports BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects
Imports DevExpress.EntityFrameworkCore.Security
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl.EF.PermissionPolicy
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore
Imports System
Imports System.Configuration
Imports System.Diagnostics

Namespace ConsoleApplication
	' Prerequisites. Add the 'DevExpress.ExpressApp.EFCore' NuGet package, define your ORM data model and create a database with user, role and permission data (run the 'DatabaseUpdater' app first).
	Friend Class Program
		Shared Sub Main()
			' ## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options
			PasswordCryptographer.EnableRfc2898 = True
			PasswordCryptographer.SupportLegacySha512 = False
			Dim authentication As New AuthenticationStandard()
			Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), authentication)
			Dim objectSpaceProvider As New SecuredEFCoreObjectSpaceProvider(security, GetType(ApplicationDbContext), XafTypesInfo.Instance, ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString, Function(builder, connectionString) builder.UseSqlServer(connectionString))

			' ## Step 2. Authentication. Log in as a 'User' with an Empty Password
			authentication.SetLogonParameters(New AuthenticationStandardLogonParameters(userName:= "User", password:= String.Empty))
			Dim loginObjectSpace As IObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace()
			Try
				security.Logon(loginObjectSpace)
			Catch sqlEx As SqlException
				If sqlEx.Number = 4060 Then
					Throw New Exception(sqlEx.Message & Environment.NewLine & MessageHelper.OpenDatabaseFailed, sqlEx)
				End If
			End Try

			' ## Step 3. Authorization. Read and Manipulate Data Based on User Access Rights
			Console.WriteLine($"{"Full Name",-40}{"Department",-40}")
			Using securedObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
				' User cannot read protected entities like PermissionPolicyRole.
				Debug.Assert(securedObjectSpace.GetObjects(Of PermissionPolicyRole)().Count = 0)
				For Each employee As Employee In securedObjectSpace.GetObjects(Of Employee)() ' User can read Employee data.
				    ' User can read Department data by criteria.
					Dim canRead As Boolean = security.CanRead(securedObjectSpace, employee, memberName:= NameOf(Employee.Department))
					Debug.Assert((Not canRead) = (employee.Department Is Nothing))
					' Mask protected property values when User has no 'Read' permission.
					Dim department = If(canRead, employee.Department.Title, "Protected Content")
					Console.WriteLine($"{employee.FullName,-40}{department,-40}")
				Next employee
			End Using
			security.Logoff()

			Console.WriteLine("Press any key to exit...")
			Console.ReadKey()
		End Sub
	End Class
End Namespace
