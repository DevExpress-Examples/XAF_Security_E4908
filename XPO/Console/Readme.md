<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF application using XPO ORM for data access. The application outputs secured data to the 'result.txt' file. 

>If you are using EF Core for data access, [follow this tutorial](https://www.devexpress.com/go/XAF_Security_NonXAF_Console_EFCore.aspx).
 
### Prerequisites
- [.NET SDK 5.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Download and run our Unified Component Installer](https://www.devexpress.com/Products/Try/) or add [NuGet feed URL](https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url) to Visual Studio nuget feeds.
  - *We recommend that you select all products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.*
***

## How to add Security System to an existent project.

1. Add [DevExpress.ExpressApp.Security.Xpo](https://nuget.devexpress.com/packages/DevExpress.ExpressApp.Security.Xpo) and [DevExpress.Persistent.BaseImpl](https://nuget.devexpress.com/packages/DevExpress.Persistent.BaseImpl) nuget packages to your project.

2. Register the business objects that you will access from your code in the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system.
	
	[](#tab/tabid-csharp)
	
	```csharp
	using DevExpress.ExpressApp;
	using DevExpress.Persistent.BaseImpl.PermissionPolicy;	
	//...
    XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
    XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
    XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
	```
3. Open the application configuration file. It is an XML file located in the application folder. The Console application configuration file is _App.config_. Add the following line in this file.
	
	[](#tab/tabid-xml)
	
	```xml
	<add name="ConnectionString" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=XPOTestDB;Integrated Security=True"/>
	```
	
	Substitute "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.
	
4. Initialize the Security System.
	
	[](#tab/tabid-csharp)
	
	```csharp
    AuthenticationStandard authentication = new AuthenticationStandard();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
    security.RegisterXPOAdapterProviders();
	```
5. Create a **SecuredObjectSpaceProvider** object. It allows you to create a **SecuredObjectSpace** to ensure a secured data access.
	[](#tab/tabid-csharp)
	
	```csharp
	string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
	SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
	```
6. Perform a logon. The code below demonstrates how to do this as a user named "User" who has an empty password.
[](#tab/tabid-csharp)
	
	```csharp
    string userName = "User";
    string password = string.Empty;
    authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
	IObjectSpace loginObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace();
    security.Logon(loginObjectSpace);
	```
	- How to create user and password from code, see the [Updater.cs](https://github.com/DevExpress-Examples/XAF_Security_E4908/blob/under_construction/XPO/DatabaseUpdater/Updater.cs) class.
7. Now you can create **SecuredObjectSpace** and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, *IObjectSpace.GetObjects*) **OR** if you prefer, the familiar **UnitOfWork** object accessible through the *SecuredObjectSpace.Session* property.

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

Note that SecuredObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the SecurityStrategy.CanRead method to determine when to mask default values with the "*******" placeholder in the UI.
