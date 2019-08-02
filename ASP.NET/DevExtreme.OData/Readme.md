This example demonstrates how to protect your data with the [XAF Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) on the [ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2) [ODATA4](https://www.odata.org/documentation/) service on the server side with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/) UI on the client side.

Create an authentication page to enter the user credentials and perform the log in to XAF Security System.
![](/images/ODataLoginPage.png)

Display data on DevExtreme Data Grid. The protected data is displayed as 'Protected Content'.
![](/images/ODataListView.png)

## Prerequisites

- Restore Nuget packages. To obtain your DevExpress feed url, see the [DevExpress NuGet Gallery](https://nuget.devexpress.com/)
- Run the XafSolution.Win project and log in with the 'User' or 'Admin' username and empty password to generate a database with business objects from the XafSolution.Module project

## Main implementation steps

1. Configure the application in the [Startup.cs](Startup.cs) file
2. Get access to XAF Security System with the [ConnectionHelper](Helpers/ConnectionHelper.cs) class
3. Get access to business data through the entity controllers: [EmployeesController](Controllers/EmployeesController.cs), [DepartmentsController](Controllers/DepartmentsController.cs), [BaseController](Controllers/BaseController.cs)
4. Handle the Log in and Log off actions in the [AccountController](Controllers/AccountController.cs)
5. Collect security permissions and send them to the client with the [ActionsController](Controllers/ActionsController.cs)
6. Implement the client side of your application with some HTML pages and Javascript files: [Index.html](wwwroot/Index.html), [Authentication.html](wwwroot/Authentication.html), [index_code.js](wwwroot/js/index_code.js), [authentication_code.js](wwwroot/js/authentication_code.js)

