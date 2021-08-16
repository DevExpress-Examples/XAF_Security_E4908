<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF application using XPO ORM for data access. The application outputs secured data to the 'result.txt' file. 

>If you are using EF Core for data access, [follow this tutorial](https://www.devexpress.com/go/XAF_Security_NonXAF_Console_EFCore.aspx).
 
### Prerequisites
- [.NET SDK 5.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Download and run our Unified Component Installer](https://www.devexpress.com/Products/Try/) or add [NuGet feed URL](https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url) to Visual Studio nuget feeds.
  - *We recommend that you select all products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.*
***
> **!NOTE:** If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

***

# How to create console application with Security system.

## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options

- Add devexpress NuGet packages to your project:

    ```xml
    <PackageReference Include="DevExpress.Persistent.BaseImpl" Version="21.1.5" />
    <PackageReference Include="DevExpress.ExpressApp.Security.Xpo" Version="21.1.5" />
    ```

- Open the application configuration file. It is an XML file located in the application folder. The Console application configuration file is _App.config_. Add the following line in this file.
	
	[](#tab/tabid-xml)
	
	```xml
	<add name="ConnectionString" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=XPOTestDB;Integrated Security=True"/>
	```
	
	Substitute "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.
	
- Initialize the Security System.
	
	[](#tab/tabid-csharp)
	
	```csharp
    AuthenticationStandard authentication = new AuthenticationStandard();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
    security.RegisterXPOAdapterProviders();
	```

- Create a **SecuredObjectSpaceProvider** object. It allows you to create a **XPObjectSpace** to ensure a secured data access.
	[](#tab/tabid-csharp)
	
	```csharp
	string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
	SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
	```

- Register the business objects that you will access from your code in the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system.
	
	[](#tab/tabid-csharp)
	
	```csharp
		objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
		objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
		objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
	```
- How to create demo data from code, see the [Updater.cs](/XPO/DatabaseUpdater/Updater.cs) class.

## Step 2. Authentication. Log in as a 'User' with an Empty Password

- Perform a logon. The code below demonstrates how to do this as a user named "User" who has an empty password.

	[](#tab/tabid-csharp)
	
	```csharp
    string userName = "User";
    string password = string.Empty;
    authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
	IObjectSpace loginObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace();
    security.Logon(loginObjectSpace);
	```
	- How to create user and password from code, see the [Updater.cs](/XPO/DatabaseUpdater/Updater.cs) class.

## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights

- Create **ObjectSpace** instance to access protected data and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, *IObjectSpace.GetObjects*) **OR** if you prefer, the familiar **UnitOfWork** object accessible through the *SecuredObjectSpace.Session* property.

	[](#tab/tabid-csharp)
	
	```csharp
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("List of the 'Employee' objects:\n");
    using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
            // The XPO way:
            // var session = ((XPObjectSpace)securedObjectSpace).Session;
            // 
            // The XAF way:
		foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) {
			stringBuilder.Append(string.Format("Full name: {0}\n", employee.FullName));
			if(security.CanRead(employee, nameof(Department))) {
				stringBuilder.Append(string.Format("Department: {0}\n", employee.Department.Title));
			}
			else {
				stringBuilder.Append("Department: *******\n");
			}
		} 
	}
	```

Note that ObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the SecurityStrategy.CanRead method to determine when to mask default values with the "*******" placeholder in the UI.

## Run and Test the App

Our console application will display employees and mask departments if their titles do not contain 'Development'.

![](/images/Console.png)