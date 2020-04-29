<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a Console application with Entity Framework Core. The application outputs secured data to the 'result.txt' file.

>For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](CS) and [VB](VB) sub-directories.
 
## Prerequisites
- [.NET Core SDK 3.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Unified Installer for .NET Framework](https://www.devexpress.com/Products/Try/).
  - We recommend that you select all  products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.
  
- Open the *EFCoreNonXAFConsoleSecurityExample.NetCore* solution and edit the [EFCore/DatabaseUpdater/App.config](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/20.1/EFCore/DatabaseUpdater/App.config) file so that `DBSERVER` refers to your database server name or its IP address (for a local database server, use `localhost`, `(local)` or `.`):
	
[](#tab/tabid-xml)
	
```xml
<configuration>
  <connectionStrings>
    <add name="ConnectionString" connectionString="Data Source=DBSERVER;Initial Catalog=EFCoreTestDB;Integrated Security=True"/>
```

- Build and run the *DatabaseUpdater* project. The console application will generate a database and populate it with business objects, security roles, and users based on the ORM data model in the *BusinessObjectsLibrary* project. For more information, see [Predefined Users, Roles and Permissions](https://docs.devexpress.com/eXpressAppFramework/119065/concepts/security-system/predefined-users-roles-and-permissions).


## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options

- Create a .NET Core console application and add the `DevExpress.ExpressApp.EFCore` NuGet package.
- Add the [EFCore/BusinessObjectsLibrary](/EFCore/BusinessObjectsLibrary) project reference to your console application (this project contains `ApplicationDbContext` and the ORM data model).
- In *YourConsoleApplication/Program.cs*, create a `SecurityStrategyComplex` instance using [AuthenticationStandard](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.AuthenticationStandard) (a simple Forms Authentication with a login and password) and password options ([EnableRfc2898 and SupportLegacySha512](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System)).
	
[](#tab/tabid-csharp)
	
```csharp
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

- In *YourConsoleApplication/App.config*, add the same connection string as in **Step 1**.

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
        throw new Exception(sqlEx.Message + Environment.NewLine + MessageHelper.OpenDatabaseFailed, sqlEx);
    }
}
```

[Full code](/EFCore/Console/CS/Program.cs#L34)

To log off or sign out, call the `security.Logoff` method.

## Step 3. Authorization. Read and Manipulate Data Based on User Access Rights
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

## Run and Test the App

Our console application will display employees and mask departments if their titles do not contain 'Development'.

![](/images/Console.png)
