Imports BusinessObjectsLibrary.EFCore.BusinessObjects
Imports DevExpress.EntityFrameworkCore.Security
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl.EF.PermissionPolicy
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore
Imports System.Configuration

Namespace ConsoleApplication
	' ## Prerequisites. 
	' 1) Add the 'DevExpress.ExpressApp.EFCore' and 'Microsoft.EntityFrameworkCore*' NuGet packages; 
	' 2) Define your ORM data model and DbContext (explore the 'BusinessObjectsLibrary' project);
	' 3) Create a database with user, role and permission data (run the 'DatabaseUpdater' project).
	Friend Class Program
		Shared Sub Main()
			' ## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options
			Dim authentication As New AuthenticationStandard()
			Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), authentication)
			Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			Dim objectSpaceProvider As SecuredEFCoreObjectSpaceProvider = New SecuredEFCoreObjectSpaceProvider(security, GetType(ApplicationDbContext), Function(builder, __) builder.UseSqlServer(connectionString))

			' ## Step 2. Authentication. Log in as a 'User' with an Empty Password
			authentication.SetLogonParameters(New AuthenticationStandardLogonParameters(userName:="User", password:=String.Empty))
			Dim loginObjectSpace As IObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace()
			Try
				security.Logon(loginObjectSpace)
			Catch sqlEx As SqlException
				If sqlEx.Number = 4060 Then
					Throw New Exception(sqlEx.Message & Environment.NewLine & ApplicationDbContext.DatabaseConnectionFailedMessage, sqlEx)
				End If
			End Try

			' ## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights
			Console.WriteLine($"{"Full Name",-40}{"Department",-40}")
			Using securedObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
				' User cannot read protected entities like PermissionPolicyRole.
				Debug.Assert(securedObjectSpace.GetObjects(Of PermissionPolicyRole)().Count = 0)
				For Each employee As Employee In securedObjectSpace.GetObjects(Of Employee)() ' User can read Employee data.
					' User can read Department data by criteria.
					Dim canRead As Boolean = security.CanRead(securedObjectSpace, employee, memberName:=NameOf(Employee.Department))
					Debug.Assert((Not canRead) = (employee.Department Is Nothing))
					' Mask protected property values when User has no 'Read' permission.
					Dim department = If(canRead, employee.Department.Title, "*******")
					Console.WriteLine($"{employee.FullName,-40}{department,-40}")
				Next employee
			End Using
			security.Logoff()

			Console.WriteLine("Press any key to exit...")
			Console.ReadKey()
		End Sub
	End Class
End Namespace
