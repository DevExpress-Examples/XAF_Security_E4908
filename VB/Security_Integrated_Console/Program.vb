Imports System
Imports System.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.ClientServer
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.BaseImpl

Namespace Security_Integrated_Console.Win
    Friend NotInheritable Class Program

        Private Sub New()
        End Sub

        Private Shared Sub UpdateDatabase(ByVal objectSpace As IObjectSpace)
            Dim userAdmin As SecuritySystemUser = objectSpace.CreateObject(Of SecuritySystemUser)()
            userAdmin.UserName = "Admin"
            userAdmin.SetPassword("")
            Dim adminRole As SecuritySystemRole = objectSpace.CreateObject(Of SecuritySystemRole)()
            adminRole.IsAdministrative = True
            userAdmin.Roles.Add(adminRole)

            Dim userJohn As SecuritySystemUser = objectSpace.CreateObject(Of SecuritySystemUser)()
            userJohn.UserName = "User"
            Dim userRole As SecuritySystemRole = objectSpace.FindObject(Of SecuritySystemRole)(New BinaryOperator("Name", "Users"))
            userRole = objectSpace.CreateObject(Of SecuritySystemRole)()
            userRole.AddObjectAccessPermission(Of Person)("[FirstName] == 'User person'", SecurityOperations.Read)
            userJohn.Roles.Add(userRole)

            Dim adminPerson As Person = objectSpace.FindObject(Of Person)(New BinaryOperator("FirstName", "Person for Admin"))
            adminPerson = objectSpace.CreateObject(Of Person)()
            adminPerson.FirstName = "Admin person"

            Dim userPerson As Person = objectSpace.CreateObject(Of Person)()
            userPerson.FirstName = "User person"
            objectSpace.CommitChanges()
        End Sub

        <STAThread> _
        Shared Sub Main()
            XpoTypesInfoHelper.GetXpoTypeInfoSource()
            XafTypesInfo.Instance.RegisterEntity(GetType(Person))
            XafTypesInfo.Instance.RegisterEntity(GetType(SecuritySystemUser))
            XafTypesInfo.Instance.RegisterEntity(GetType(SecuritySystemRole))

            Console.WriteLine("Update database...")
            Dim dataSet As New DataSet()
            Console.WriteLine("Database has been updated successfully.")
            Dim directProvider As New XPObjectSpaceProvider(New MemoryDataStoreProvider(dataSet))
            Dim directObjectSpace As IObjectSpace = directProvider.CreateObjectSpace()
            UpdateDatabase(directObjectSpace)

            Dim auth As New AuthenticationStandard()
            auth.SetLogonParameters(New AuthenticationStandardLogonParameters("User", ""))

            Dim security As New SecurityStrategyComplex(GetType(SecuritySystemUser), GetType(DevExpress.ExpressApp.Security.Strategy.SecuritySystemRole), auth)
            SecuritySystem.SetInstance(security)
            Console.WriteLine("Logging 'User' user...")
            security.Logon(directObjectSpace)
            Console.WriteLine("'User' is logged on.")

            Dim osProvider As New SecuredObjectSpaceProvider(security, New MemoryDataStoreProvider(dataSet))
            Dim securedObjectSpace As IObjectSpace = osProvider.CreateObjectSpace()

            Console.WriteLine("List of the 'Person' objects:")
            For Each person As Person In securedObjectSpace.GetObjects(Of Person)()
                Console.WriteLine(person.FirstName)
            Next person
            auth.SetLogonParameters(New AuthenticationStandardLogonParameters("Admin", ""))
            Console.WriteLine("Logging 'Admin' user...")
            security.Logon(directObjectSpace)
            Console.WriteLine("Admin is logged on.")

            securedObjectSpace = osProvider.CreateObjectSpace()
            Console.WriteLine("List of the 'Person' objects:")
            For Each person As Person In securedObjectSpace.GetObjects(Of Person)()
                Console.WriteLine(person.FirstName)
            Next person

            Console.WriteLine("Press enter to exit...")
            Console.ReadLine()
        End Sub
    End Class
End Namespace
