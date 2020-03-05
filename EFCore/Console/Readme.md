<!-- default file list -->

# This branch is under construction...

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a Console application with Entity Framework Core. The application outputs secured data to the 'result.txt' file.

>For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](CS) and [VB](VB) sub-directories.
 
## Prerequisites
- [.NET Core SDK 3.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Unified Installer for .NET Framework](https://www.devexpress.com/Products/Try/).
  - We recommend that you select all  products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.
  
***
  
## Step 1. Create a Database and Populate It with Data

**1.** Build the 'EFCoreNonXAFSecurityExamples.NetCore' solution.

**2.** Open the [EFCore/DatabaseUpdater/App.config](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/20.1/EFCore/DatabaseUpdater/App.config) file and modify it so that it refers to your database server:
	
	[](#tab/tabid-xml)
	
	```xml
	<configuration>
  		<connectionStrings>
			<add name="ConnectionString" connectionString="Data Source=DBSERVER;Initial Catalog=ConsoleEFCoreTestDB;Integrated Security=True"/>
	```

Substitute "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.
    
**3.** Run the *DatabaseUpdater* project. The console application will generate a database and populate it with business objects, security roles, and users.



## Step 2. Initialize Data Store and XAF's Security System 

You can find all this code in the [EFCore/Console](/EFCore/Console) folder.

**1.** Create a .NET Core console application.

**2.** Initialize the Security System.
	
	[](#tab/tabid-csharp)
	
	```csharp
	AuthenticationStandard authentication = new AuthenticationStandard();
	SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
	```	
[Full code](/EFCore/Console/CS/Program.cs#L16)

**3.** Open the [EFCore/Console/CS/App.config](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/20.1/EFCore/Console/CS/App.config) file and modify it so that it refers to the same database as the DatabaseUpdater's config file ([EFCore/DatabaseUpdater/App.config](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/20.1/EFCore/DatabaseUpdater/App.config)).

**4.** Create a **SecuredEFCoreObjectSpaceProvider** object. Create an instance of the EFCoreDatabaseProviderHandler delegate with SqlServer and security extensions. It allows you to create a secured **IObjectSpace** instance to ensure secured data access.


	[](#tab/tabid-csharp)
	
	```csharp
	string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
	SecuredEFCoreObjectSpaceProvider securedObjectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, typeof(ConsoleDbContext), XafTypesInfo.Instance, connectionString,
	    (builder, connectionString) =>
	     builder.UseSqlServer(connectionString));
	```
[Full code](/EFCore/Console/CS/Program.cs#L19)

**5.** Initialize the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system and register the business objects that you will access from your code.
	
	[](#tab/tabid-csharp)
	
	```csharp
	using DevExpress.ExpressApp;
	using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
	using BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects;
	//...
	XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
	XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
	XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
	```
	
[Full code](/EFCore/Console/CS/Program.cs#L57)
## Step 3. Configure User Authentication Options and Login
**1.** Specify the static [EnableRfc2898 and SupportLegacySha512 properties](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System):
	[](#tab/tabid-csharp)
	
	```csharp
	PasswordCryptographer.EnableRfc2898 = true;
	PasswordCryptographer.SupportLegacySha512 = false;
	```
[Full code](/EFCore/Console/CS/Program.cs#L26)

**2.** Perform a logon. The code below demonstrates how to do this as a user named "User" who has an empty password.

	[](#tab/tabid-csharp)
	
	```csharp
	string userName = "User";
	string password = string.Empty;
	authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
	IObjectSpace loginObjectSpace = securedObjectSpaceProvider.CreateNonsecuredObjectSpace();
	security.Logon(loginObjectSpace);
	```
[Full code](/EFCore/Console/CS/Program.cs#L29)
## Step 4. Authorize CRUD Operations Based on User Access Rights
Now you can create a secured **IObjectSpace** instance and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, *IObjectSpace.GetObjects*).

[](#tab/tabid-csharp)
	
```csharp
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.Append("List of the 'Person' objects:\n");
using(IObjectSpace securedObjectSpace = securedObjectSpaceProvider.CreateObjectSpace()) {
    foreach(Person person in securedObjectSpace.GetObjects<Person>()) {
        stringBuilder.Append($"Full name: {person.FullName}\n");
        if(security.CanRead(person, nameof(person.Email))) {
	    stringBuilder.Append($"Email: {person.Email}\n");
	} else {
	    stringBuilder.Append("Email: [Protected content]\n");
	}
    } 
}
```
[Full code](/EFCore/Console/CS/Program.cs#L36)

Note that IObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the SecurityStrategy.CanRead method to determine when to mask default values with the "Protected Content" placeholder in the UI.

## Step 5: Run and Test the App
