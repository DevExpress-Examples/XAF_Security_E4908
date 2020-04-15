Imports BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects
Imports DevExpress.EntityFrameworkCore.Security
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.EFCore
Imports DevExpress.ExpressApp.Security
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl.EF.PermissionPolicy
Imports Microsoft.EntityFrameworkCore
Imports System.Configuration
Imports System.IO
Imports System.Text

Namespace ConsoleApplication
    Friend Class Program
        Shared Sub Main()
            Dim authentication As New AuthenticationStandard()
            Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), authentication)


            Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            Dim objectSpaceProvider As New SecuredEFCoreObjectSpaceProvider(security, GetType(ApplicationDbContext), XafTypesInfo.Instance, connectionString,
                Function(builder, cs) builder.UseSqlServer(cs))

            PasswordCryptographer.EnableRfc2898 = True
            PasswordCryptographer.SupportLegacySha512 = False


            Dim userName As String = "User"
            Dim password As String = String.Empty
            authentication.SetLogonParameters(New AuthenticationStandardLogonParameters(userName, password))
            Dim loginObjectSpace As IObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace()
            security.Logon(loginObjectSpace)

            Using file As New StreamWriter("result.txt", False)
                Dim stringBuilder As New StringBuilder()
                stringBuilder.Append($"{userName} is logged on." & ControlChars.Lf)
                stringBuilder.Append("List of the 'Employee' objects:" & ControlChars.Lf)
                Using securedObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
                    For Each employee As Employee In securedObjectSpace.GetObjects(Of Employee)()
                        stringBuilder.Append("=========================================" & ControlChars.Lf)
                        stringBuilder.Append($"Full name: {employee.FullName}" & ControlChars.Lf)
                        If security.CanRead(employee, NameOf(Department)) Then
                            stringBuilder.Append($"Department: {employee.Department.Title}" & ControlChars.Lf)
                        Else
                            stringBuilder.Append("Department: [Protected content]" & ControlChars.Lf)
                        End If
                    Next employee
                End Using
                file.Write(stringBuilder)
            End Using
            Console.WriteLine(String.Format("The result.txt file has been created in the {0} directory.", Environment.CurrentDirectory))
            Console.WriteLine("Press any key to close a the console...")
            Console.ReadLine()
        End Sub
    End Class
End Namespace