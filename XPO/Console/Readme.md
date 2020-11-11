<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF application using XPO ORM for data access. The application outputs secured data to the 'result.txt' file. 

>If you are using EF Core for data access, [follow this tutorial](https://www.devexpress.com/go/XAF_Security_NonXAF_Console_EFCore.aspx).

For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](CS) and [VB](VB) sub-directories.
 
### Prerequisites
- [.NET Core SDK 3.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Two unified installers for .NET Framework and .NET Core 3.1 Desktop Development](https://www.devexpress.com/Products/Try/).
  - We recommend that you select all  products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.
- Build the following solutions or projects depending on your target framework:
  - .NET Framework: *NonXAFSecurityExamples.sln* or *ConsoleApplication/XafSolution.Win*.
  - .NET Core: *NonXAFSecurityExamples.NetCore.sln* or *ConsoleApplication.NetCore/XafSolution.Win.NetCore*.
- Run the *XafSolution.Win/XafSolution.Win.NetCore* project to log in under 'User' or 'Admin' with an empty password. The application will generate a database with business objects from the *XafSolution.Module/XafSolution.Module.NetCore* project.
- Add the *XafSolution.Module/XafSolution.Module.NetCore* assembly reference to your test application.

***

1. Initialize the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system and register the business objects that you will access from your code.
	
	[](#tab/tabid-csharp)
	
	```csharp
	using DevExpress.ExpressApp;
	using DevExpress.Persistent.BaseImpl.PermissionPolicy;
	//...
	XpoTypesInfoHelper.GetXpoTypeInfoSource();
    XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
    XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
    XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
	```
2. Open the application configuration file. It is an XML file located in the application folder. The Console application configuration file is _App.config_. Add the following line in this file.
	
	[](#tab/tabid-xml)
	
	```xml
	<add name="ConnectionString" connectionString="Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"/>
	```
	
	Substitute "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.
	
3. Initialize the Security System.
	
	[](#tab/tabid-csharp)
	
	```csharp
    AuthenticationStandard authentication = new AuthenticationStandard();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
    security.RegisterXPOAdapterProviders();
	```
4. Create a **SecuredObjectSpaceProvider** object. It allows you to create a **SecuredObjectSpace** to ensure a secured data access.
	[](#tab/tabid-csharp)
	
	```csharp
	string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
	SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
	```
5. Perform a logon. The code below demonstrates how to do this as a user named "User" who has an empty password.
[](#tab/tabid-csharp)
	
	```csharp
    string userName = "User";
    string password = string.Empty;
    authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
	IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
    security.Logon(loginObjectSpace);
	```
6. Now you can create **SecuredObjectSpace** and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, *IObjectSpace.GetObjects*) **OR** if you prefer, the familiar **UnitOfWork** object accessible through the *SecuredObjectSpace.Session* property.

	[](#tab/tabid-csharp)
	
	```csharp
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("List of the 'Employee' objects:\n");
    using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
            // The XPO way:
            // var session = ((SecuredObjectSpace)securedObjectSpace).Session;
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
