This example demostrates how to protect your data with the [XAF Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) on the [ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2) [ODATA4](https://www.odata.org/documentation/) service on the server side with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/) UI on the client side.

## Prerequisites

- Install the following Nuget packages:

    DevExtreme:
    - DevExtreme.AspNet.Data
    - DevExtreme.AspNet.Mvc
    - DevExtreme.AspNet.Core


    Xaf Security System:
    - Devexpress.ExpressApp.Security.Xpo
    - Devexpress.Persistent.BaseImpl


    Microsoft:
    - Microsoft.AspNetCore.OData
    - Microsoft.AspNetCore.Authentication
    - Microsoft.AspNetCore.Authentication.Cookies
    - Microsoft.AspNetCore.CookiePolicy
    - Microsoft.AspNetCore.HttpsPolicy
    - Microsoft.AspNetCore.Identity
    - Microsoft.AspNetCore
    - Microsoft.AspNetCore.Mvc
    - Microsoft.AspNetCore.StaticFiles
    
    To obtain your DevExpress feed url, see the [DevExpress NuGet Gallery](https://nuget.devexpress.com/).

- Install packages from the bower.json file, follow the instruction provided on its official site: [bower.io](https://bower.io/).
- Run the XafSolution.Win project and log in with the 'User' or 'Admin' username and empty password to generate a database with business objects from the XafSolution.Module project.
