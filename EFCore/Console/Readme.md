<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a Console App (.NET Core) with Entity Framework Core. The application outputs secured data to the console.

>For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](CS) and [VB](VB) sub-directories.
 
## Prerequisites. Create a Database and Populate It with User, Role, Permission and Other Data
- [.NET Core SDK 3.1+](https://dotnet.microsoft.com/download/dotnet-core) and [EF Core 3.1](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.1.2) (EF Core 5 is to be supported).
- [Download and run a unified installer for .NET Framework (v20.1+)](https://www.devexpress.com/Products/Try/) or [obtain a DevExpress NuGet Feed URL](https://docs.devexpress.com/GeneralInformation/115912/installation/install-devexpress-controls-using-nuget-packages).
  - *We recommend that you select all  products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later*.
  
- Open the *ConsoleApplication.EFCore.sln* solution and edit the [EFCore/DatabaseUpdater/App.config](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/20.1/EFCore/DatabaseUpdater/App.config) file so that `DBSERVER` refers to your database server name or its IP address (for a local database server, use `localhost`, `(local)` or `.`):
	
[](#tab/tabid-xml)
	
```xml
<connectionStrings>
    <add name="ConnectionString" 
        connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreTestDB;Integrated Security=True"
    />
</connectionStrings>
```

- Build and run the *DatabaseUpdater* project. This console application will create a database with user, role, permission and other data based on the `ApplicationDbContext` and `Updater` classes and the ORM data model in the *BusinessObjectsLibrary* project. For more information, see [Predefined Users, Roles and Permissions](https://docs.devexpress.com/eXpressAppFramework/119065/concepts/security-system/predefined-users-roles-and-permissions).


## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options

- Create a new **Console App (.NET Core)** project and add the [EFCore/BusinessObjectsLibrary](/EFCore/BusinessObjectsLibrary) project reference. *BusinessObjectsLibrary* adds important NuGet dependencies:
```xml
    <PackageReference Include="DevExpress.ExpressApp.EFCore" Version="20.1.3-ctp" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.2" />
```
The `DevExpress.ExpressApp.EFCore` NuGet package contains the PermissionPolicyUser, PermissionPolicyRole and other XAF's Security System API.

- Add NuGet packages for Entity Framework Core with SQL Server:
```xml
   <PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="3.1.2" />
   <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.2" />
```
- In *YourConsoleApplication/Program.cs*, create a `SecurityStrategyComplex` instance using [AuthenticationStandard](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.AuthenticationStandard) (a simple Forms Authentication with a login and password) and password options ([EnableRfc2898 and SupportLegacySha512](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System)).
	
[](#tab/tabid-csharp)
	
```csharp
using BusinessObjectsLibrary.EFCore.BusinessObjects;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
//...

static void Main() {
    PasswordCryptographer.EnableRfc2898 = true;
    PasswordCryptographer.SupportLegacySha512 = false;

    AuthenticationStandard authentication = new AuthenticationStandard();

    SecurityStrategyComplex security = new SecurityStrategyComplex(
        typeof(PermissionPolicyUser), typeof(PermissionPolicyRole),
        authentication
    );
```	
[Full code](/EFCore/Console/CS/Program.cs#L19)

- Create a `SecuredEFCoreObjectSpaceProvider` instance using the `EFCoreDatabaseProviderHandler` delegate and the `UseSqlServer` extension:


[](#tab/tabid-csharp)
	
```csharp
SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(
    security, typeof(ApplicationDbContext),
    XafTypesInfo.Instance, ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString,
    (builder, connectionString) => builder.UseSqlServer(connectionString)
);
```
[Full code](/EFCore/Console/CS/Program.cs#L27)

- In *YourConsoleApplication/App.config*, add the same connection string as in **Prerequisites**.

This provider allows you to create secured [IObjectSpace](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) instances to perform secured CRUD (create-read-update-delete) operations. Object Space is an ORM-independent implementation of the well-known Repository and Unit Of Work design patterns (for instance, `SecuredEFCoreObjectSpace` is an IObjectSpace implementation for EF Core that wraps DbContext).
	
## Step 2. Authentication. Log in as a 'User' with an Empty Password

[](#tab/tabid-csharp)
	
```csharp
authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName: "User", password: string.Empty));
IObjectSpace loginObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace();
try {
    security.Logon(loginObjectSpace);
}
catch(SqlException sqlEx) {
    if(sqlEx.Number == 4060) {
        throw new Exception(sqlEx.Message + Environment.NewLine + ApplicationDbContext.DatabaseConnectionFailedMessage, sqlEx);
    }
}
```

[Full code](/EFCore/Console/CS/Program.cs#L34)

To log off or sign out, call the `security.Logoff` method.

## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights
Create a `SecuredEFCoreObjectSpace` instance to access protected data and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, `IObjectSpace.GetObjects`). Note that `SecuredEFCoreObjectSpace` returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the `SecurityStrategy.CanRead` method to determine when to mask default values with the **Protected Content** placeholder in the UI.

[](#tab/tabid-csharp)
	
```csharp
using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
    foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) { 
        bool canRead = security.CanRead(securedObjectSpace, employee, memberName: nameof(Employee.Department));
        var department = canRead ? employee.Department.Title : "Protected Content";
        Console.WriteLine($"{employee.FullName,-40}{department,-40}");
    }
}
security.Logoff();
```
[Full code](/EFCore/Console/CS/Program.cs#L46)

***

> The same implementation steps can be used in other .NET apps.

## Run and Test the App

Our console application will display employees and mask departments if their titles do not contain 'Development'.

![](/images/Console.png)
