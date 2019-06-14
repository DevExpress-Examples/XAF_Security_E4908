Imports System
Imports System.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.ClientServer
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.BaseImpl.PermissionPolicy
Imports DevExpress.Persistent.Base

Namespace Security_Integrated_Console.Win
	Friend Module Program
		Private Sub UpdateDatabase(ByVal objectSpace As IObjectSpace)
			Dim userAdmin As PermissionPolicyUser = objectSpace.CreateObject(Of PermissionPolicyUser)()
			userAdmin.UserName = "Admin"
			userAdmin.SetPassword("")
			Dim adminRole As PermissionPolicyRole = objectSpace.CreateObject(Of PermissionPolicyRole)()
			adminRole.IsAdministrative = True
			userAdmin.Roles.Add(adminRole)

			Dim userJohn As PermissionPolicyUser = objectSpace.CreateObject(Of PermissionPolicyUser)()
			userJohn.UserName = "User"
			Dim userRole As PermissionPolicyRole = objectSpace.FindObject(Of PermissionPolicyRole)(New BinaryOperator("Name", "Users"))
			userRole = objectSpace.CreateObject(Of PermissionPolicyRole)()
			userRole.AddObjectPermission(Of Person)(SecurityOperations.Read, "[FirstName] == 'User person'", SecurityPermissionState.Allow)
			userJohn.Roles.Add(userRole)

			Dim adminPerson As Person = objectSpace.FindObject(Of Person)(New BinaryOperator("FirstName", "Person for Admin"))
			adminPerson = objectSpace.CreateObject(Of Person)()
			adminPerson.FirstName = "Admin person"

			Dim userPerson As Person = objectSpace.CreateObject(Of Person)()
			userPerson.FirstName = "User person"
			objectSpace.CommitChanges()
		End Sub

		<STAThread>
		Sub Main()
			XpoTypesInfoHelper.GetXpoTypeInfoSource()
			XafTypesInfo.Instance.RegisterEntity(GetType(Person))
			XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyUser))
			XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyRole))

			Console.WriteLine("Update database...")
			Dim dataSet As New DataSet()
			Console.WriteLine("Database has been updated successfully.")

			Dim directProvider As New XPObjectSpaceProvider(New MemoryDataStoreProvider(dataSet))
			Dim directObjectSpace As IObjectSpace = directProvider.CreateObjectSpace()
			UpdateDatabase(directObjectSpace)

			Dim auth As New AuthenticationStandard()
			Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), auth)
			SecuritySystem.SetInstance(security)
			Dim osProvider As New SecuredObjectSpaceProvider(security, New MemoryDataStoreProvider(dataSet))

			auth.SetLogonParameters(New AuthenticationStandardLogonParameters("User", ""))
			Console.WriteLine("Logging 'User' user...")
			security.Logon(directObjectSpace)
			Console.WriteLine("'User' is logged on.")
			Console.WriteLine("List of the 'Person' objects:")
			Using securedObjectSpace As IObjectSpace = osProvider.CreateObjectSpace()
				For Each person As Person In securedObjectSpace.GetObjects(Of Person)()
					Console.WriteLine(person.FirstName)
				Next person
			End Using

			auth.SetLogonParameters(New AuthenticationStandardLogonParameters("Admin", ""))
			Console.WriteLine("Logging 'Admin' user...")
			security.Logon(directObjectSpace)
			Console.WriteLine("Admin is logged on.")
			Console.WriteLine("List of the 'Person' objects:")
			Using securedObjectSpace As IObjectSpace = osProvider.CreateObjectSpace()
				For Each person As Person In securedObjectSpace.GetObjects(Of Person)()
					Console.WriteLine(person.FirstName)
				Next person
			End Using

			Console.WriteLine("Press enter to exit...")
			Console.ReadLine()
		End Sub
	End Module
End Namespace
