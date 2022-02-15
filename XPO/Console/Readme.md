<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF application using XPO ORM for data access. The application outputs secured data to the console. 

>If you are using EF Core for data access, [follow this tutorial](https://github.com/DevExpress-Examples/XAF_Security_E4908/tree/master/EFCore/Console).
 
### Prerequisites

- [Visual Studio 2022 v17.0+](https://visualstudio.microsoft.com/vs/)
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Download and run our Unified Component Installer](https://www.devexpress.com/Products/Try/) or add [NuGet feed URL](https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url) to Visual Studio NuGet feeds.
  
  *We recommend that you select all products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.*


> **NOTE** 
>
> If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options

1. Create a new .NET 6 console application or use an existing application.
2. Add DevExpress NuGet packages to your project:

    ```xml
    <PackageReference Include="DevExpress.Persistent.BaseImpl.Xpo" Version="21.2.4" />
    <PackageReference Include="DevExpress.ExpressApp.Security.Xpo" Version="21.2.4" />
    ```
3. Open the application configuration file (_App.config_). Add the following line in this file.
    
    ```xml
    <add name="ConnectionString" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=XPOTestDB;Integrated Security=True"/>
    ```

4. Create an instance of `TypesInfo` required for the correct operation of the Security System.
    ```csharp
    TypesInfo typesInfo = new TypesInfo();
    ```

5. Register the business objects that you will access from your code in the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system.

    ```csharp
    typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
    typesInfo.RegisterEntity(typeof(Employee));
    typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
    typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
    ```

6. Call the `CreateDemoData` method at the beginning of the `Main` method of _Program.cs_:
    ```csharp
    static void CreateDemoData(TypesInfo typesInfo, IXpoDataStoreProvider dataStoreProvider) {
        using (var objectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, typesInfo, null)) {
            using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                new Updater(objectSpace).UpdateDatabase();
            }
        }
    }
    ```
    For more details about how to create demo data from code, see the [Updater.cs](/XPO/DatabaseUpdater/Updater.cs) class.
    
7. Initialize the Security System.
    
    ```csharp
    AuthenticationStandard authentication = new AuthenticationStandard();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
    security.RegisterXPOAdapterProviders();
    ```

8. Create a **SecuredObjectSpaceProvider** object. It allows you to create a **XPObjectSpace** to ensure a secured data access.
    
    ```csharp
    SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, dataStoreProvider, typesInfo, null);
    ```

## Step 2. Authentication. Log in as a 'User' with an Empty Password

Logon as a "User" with an empty password.

    ```csharp
    string userName = "User";
    string password = string.Empty;
    authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
    IObjectSpace loginObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace();
    security.Logon(loginObjectSpace);
    ```
For information on how to create user and password from code, see the [Updater.cs](/XPO/DatabaseUpdater/Updater.cs) class.

## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights

1. Create an **ObjectSpace** instance to access protected data and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, *IObjectSpace.GetObjects*) **OR** if you prefer, the familiar **UnitOfWork** object accessible through the *SecuredObjectSpace.Session* property.
    
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

Note that `ObjectSpace` returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the `SecurityStrategy.CanRead` method to determine when to mask default values with the "*******" placeholder in the UI.

## Run and Test the App

Our console application will display employees and mask departments if their titles do not contain 'Development'.

![](/images/Console.png)