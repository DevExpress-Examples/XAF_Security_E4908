This example demonstrates how to protect your data with the [XAF's Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) in the following client-server web app:
 * Server: an OData v4 service built with the [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2).
 * Client: an HTML/JavaScript app with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/).
 
## Prerequisites
* [Visual Studio 2017 or 2019](https://visualstudio.microsoft.com/vs/) with the following workloads:
  * **ASP.NET and web development**
  * **.NET Core cross-platform development**
* [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/all)
* [Unified Installer for .NET and HTML5 Developers](https://www.devexpress.com/Products/Try/)
  * We recommend that you select all the DevExpress products when you run the installation. It will register local NuGet package sources, item and project templates required for these tutorials. You can uninstall unnecessary components later.
- Build the solution and run the *XafSolution.Win* project to log in under 'User' or 'Admin' with an empty password. It will generate a database with business objects from the *XafSolution.Module* project.

## How It Works

1. Log in under 'User' with an empty password in the basic logon form.
![](/images/ODataLoginPage.png)
2. Notice that secured data is displayed as 'Protected Content'.
![](/images/ODataListView.png)
3. Press the Logoff button and log in under 'Admin' to see all records.

## Implementation Overview
>The current description provides an overview of required steps at this stage. We will create detailed step-by-step instructions in the future. If you have any difficulties, [submit a ticket](https://www.devexpress.com/ask) in the DevExpress Support Center to receive quick and guaranteed help from DevExpress.

1. Configure the backend service in the [Startup.cs](Startup.cs) file. Notice the [UnauthorizedRedirectMiddleware.cs](UnauthorizedRedirectMiddleware.cs) file that contains a [middleware pipeline](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.2) that redirects anonymous requests.
2. Access the XAF's Security System with the [ConnectionHelper](Helpers/ConnectionHelper.cs) class.
3. Access business object data using the following controllers: [EmployeesController](Controllers/EmployeesController.cs), [DepartmentsController](Controllers/DepartmentsController.cs), [BaseController](Controllers/BaseController.cs).
4. Handle the Login and Logoff commands using [AccountController](Controllers/AccountController.cs).
5. Collect security permissions and send them to the client using [ActionsController](Controllers/ActionsController.cs).
6. Implement the client-side UI as demonstrated in the following files: [Index.html](wwwroot/Index.html), [Authentication.html](wwwroot/Authentication.html), [index_code.js](wwwroot/js/index_code.js), [authentication_code.js](wwwroot/js/authentication_code.js).
