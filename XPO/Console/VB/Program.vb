Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.ClientServer
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl.PermissionPolicy
Imports System
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports XafSolution.Module.BusinessObjects

Namespace ConsoleApplication
    Friend Class Program
        Shared Sub Main()
            RegisterEntities()
            Dim authentication As New AuthenticationStandard()
            Dim security As New SecurityStrategyComplex(GetType(PermissionPolicyUser), GetType(PermissionPolicyRole), authentication)
            security.RegisterXPOAdapterProviders()

            Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            Dim objectSpaceProvider As New SecuredObjectSpaceProvider(security, connectionString, Nothing)

            PasswordCryptographer.EnableRfc2898 = True
            PasswordCryptographer.SupportLegacySha512 = False

            Dim userName As String = "User"
            Dim password As String = String.Empty
            authentication.SetLogonParameters(New AuthenticationStandardLogonParameters(userName, password))
            Dim loginObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
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
        Private Shared Sub RegisterEntities()
            XpoTypesInfoHelper.GetXpoTypeInfoSource()
            XafTypesInfo.Instance.RegisterEntity(GetType(Employee))
            XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyUser))
            XafTypesInfo.Instance.RegisterEntity(GetType(PermissionPolicyRole))
        End Sub
    End Class
End Namespace