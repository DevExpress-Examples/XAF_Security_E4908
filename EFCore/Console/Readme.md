<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF application using EF Core for data access. The application outputs secured data to the console.

>If you are using XPO ORM for data access, [follow this tutorial](https://github.com/DevExpress-Examples/XAF_Security_E4908/tree/master/XPO/Console).
 
## Prerequisites

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
    <PackageReference Include="DevExpress.ExpressApp.EFCore" Version="21.2.4" />
    <PackageReference Include="DevExpress.Persistent.BaseImpl.EFCore" Version="21.2.4" />
    ```
3. Install Entity Framework Core, as described in the [Installing Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/get-started/overview/install) article.
4. Open the application configuration file (_App.config__. Add the following line to this file.
    
    ```xml
    <add name="ConnectionString" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreTestDB;Integrated Security=True;MultipleActiveResultSets=True"/>
    ```

    > **NOTE** 
    >
    > The Security System requires [Multiple Active Result Sets](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/enabling-multiple-active-result-sets) in EF Core-based applications connected to the MS SQL database. We do not recommend that you remove `MultipleActiveResultSets=True;` from the connection string or set the `MultipleActiveResultSets` parameter to `false`.
    
- Create an instance of `TypesInfo` required for the correct operation of the Security System.
    ```csharp
    TypesInfo typesInfo = new TypesInfo();
    ```

- Initialize the Security System.
    
    ```csharp
    AuthenticationStandard authentication = new AuthenticationStandard();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
    ```

- Create a **SecuredEFCoreObjectSpaceProvider** object. It allows you to create a **EFCoreObjectSpace** to ensure a secured data access.
    
    ```csharp
    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, typeof(ApplicationDbContext),
        typesInfo, connectionString, (builder, connectionString) => builder.UseSqlServer(connectionString));
    ```

- Call CreateDemoData method at the beginning of the Main method of Program.cs:

    ```csharp
    private static void CreateDemoData(string connectionString, TypesInfo typesInfo) {
        using (var objectSpaceProvider = new EFCoreObjectSpaceProvider(typeof(ApplicationDbContext), typesInfo, connectionString,
    (builder, connectionString) => builder.UseSqlServer(connectionString)))
        using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
            new Updater(objectSpace).UpdateDatabase();
        }
    }
    ```
    For more details about how to create demo data from code, see the [Updater.cs](/EFCore/DatabaseUpdater/Updater.cs) class.

## Step 2. Authentication. Log in as a 'User' with an Empty Password

- Perform a logon. The code below demonstrates how to do this as a user named "User" who has an empty password.
    
    ```csharp
    string userName = "User";
    string password = string.Empty;
    authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
    IObjectSpace loginObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace();
    security.Logon(loginObjectSpace);
    ```

## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights

- Create **ObjectSpace** instance to access protected data and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, *IObjectSpace.GetObjects*) **OR** if you prefer, the familiar **DbContext** object accessible through the *EFCoreObjectSpace.DbContext* property.

    ```csharp
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("List of the 'Employee' objects:\n");
    using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
            // The EFCore way:
            // var dbContext = ((EFCoreObjectSpace)securedObjectSpace).DbContext;
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