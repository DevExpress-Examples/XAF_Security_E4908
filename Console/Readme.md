<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF application. The application outputs secured data to the 'result.txt' file.

>For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](/CS) and [VB](/VB) sub-directories.
 
### Prerequisites
* Run the XafSolution.Win project and log in with the 'User' or 'Admin' username and empty password to generate a database with business objects from the XafSolution.Module project.
* Add the XafSolution.Module reference to use these classes in your application.

***

1. Initialize the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system and register the business objects that you will access from your code.
	
	[](#tab/tabid-csharp)
	
	```csharp
	using DevExpress.ExpressApp;
	using DevExpress.Persistent.BaseImpl.PermissionPolicy;
	//...
	XpoTypesInfoHelper.GetXpoTypeInfoSource();
    XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
    XafTypesInfo.Instance.RegisterEntity(typeof(Person));
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
    AuthenticationStandard auth = new AuthenticationStandard();
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
    auth.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
	IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
    security.Logon(loginObjectSpace);
	```
6. Now you can create the **SecuredObjectSpace** object and call the Object Space's methods (e.g., [IObjectSpace.GetObjects](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.IObjectSpace.GetObjects.overloads)).
	[](#tab/tabid-csharp)
	
	```csharp
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("List of the 'Employee' objects:\n");
    using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
		foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) {
			stringBuilder.Append(string.Format("Full name: {0}\n", employee.FullName));
			if(security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, employee, "Department"))) {
				stringBuilder.Append(string.Format("Department: {0}\n", employee.Department.Title));
			}
			else {
				stringBuilder.Append("Department: [Protected content]\n");
			}
		} 
	}
	```

SecuredObjectSpace returns unavailable data as default values. Use the [SecurityStrategy.IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) method to replace this values with the "Protected content" placeholder.

> Make sure that the static [EnableRfc2898 and SupportLegacySha512 properties](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System) in your non-XAF application have same values as in the XAF application where passwords were set. Otherwise you won't be able to login.
