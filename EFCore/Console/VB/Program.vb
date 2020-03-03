Imports BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects
Imports ConsoleApplication.EFCore.NetCore.DatabaseUpdate
Imports DevExpress.EntityFrameworkCore.Security
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports DevExpress.ExpressApp.EFCore
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.ClientServer
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl.EF.PermissionPolicy
Imports Microsoft.EntityFrameworkCore
Imports System
Imports System.Configuration
Imports System.IO
Imports System.Text

Namespace ConsoleApplication
    Friend Class Program
        Shared Sub Main()
            Dim authentication As New AuthenticationStandard()
            Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), authentication)


            Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            Dim securedObjectSpaceProvider As New SecuredEFCoreObjectSpaceProvider(security, GetType(ConsoleDbContext), XafTypesInfo.Instance, connectionString,
                Function(builder, cs) builder.UseSqlServer(cs))

            RegisterEntities()

            PasswordCryptographer.EnableRfc2898 = True
            PasswordCryptographer.SupportLegacySha512 = False


            Dim userName As String = "User"
            Dim password As String = String.Empty
            authentication.SetLogonParameters(New AuthenticationStandardLogonParameters(userName, password))
            Dim loginObjectSpace As IObjectSpace = securedObjectSpaceProvider.CreateNonsecuredObjectSpace()
            security.Logon(loginObjectSpace)

            Using file As New StreamWriter("result.txt", False)
                Dim stringBuilder As New StringBuilder()
                stringBuilder.Append($"{userName} is logged on." & ControlChars.Lf)
                stringBuilder.Append("List of the 'Person' objects:" & ControlChars.Lf)
                Using securedObjectSpace As IObjectSpace = securedObjectSpaceProvider.CreateObjectSpace()
                    For Each person As Person In securedObjectSpace.GetObjects(Of Person)()
                        stringBuilder.Append("=========================================" & ControlChars.Lf)
                        stringBuilder.Append($"Full name: {person.FullName}" & ControlChars.Lf)
                        If security.CanRead(person, NameOf(person.Email)) Then
                            stringBuilder.Append($"Email: {person.Email}" & ControlChars.Lf)
                        Else
                            stringBuilder.Append("Email: [Protected content]]" & ControlChars.Lf)
                        End If
                    Next person
                End Using
                file.Write(stringBuilder)
            End Using
            Console.WriteLine(String.Format("The result.txt file has been created in the {0} directory.", Environment.CurrentDirectory))
            Console.WriteLine("Press any key to close.")
            Console.ReadLine()
        End Sub
        Private Shared Sub RegisterEntities()
            XafTypesInfo.Instance.RegisterEntity(GetType(Person))
            XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyUser))
            XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyRole))
        End Sub
    End Class
End Namespace