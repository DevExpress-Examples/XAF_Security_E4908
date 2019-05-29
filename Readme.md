<!-- default file list -->

# How to: Use the Integrated Mode of the Security System in Non-XAF Applications
## Console

<p>The complete description for console applications is available in the <a href="http://documentation.devexpress.com/#Xaf/CustomDocument3558"><u>How to: Use the Integrated Mode of the Security System in Non-XAF Applications</u></a> topic.</p><br />


<br/>

## Blazor (server-side)
### Prerequisites

* [Visual Studio 2019 16.1 Preview 3 or later](https://visualstudio.com/preview) with the following workloads:
  * **ASP.NET and web development**
  * **.NET Core cross-platform development**
* [.NET Core SDK 3 Preview 5 or later](https://www.microsoft.com/net/download/all)
* [DevExpress UI Components for Blazor (CTP)](https://www.devexpress.com/blazor/)

This app uses the [Server-side hosting model](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-3.0#server-side).

### Folow this steps to run Blazor (server-side)
1. Run XafSolution.Win application and log in as "Admin" with empty password to create database
2. Register the DevExpress Early Access feed in Visual Studio's NuGet Package Manager
  * Open the "Package Manager Settings"
  * Add new NuGet source:
  ```https://nuget.devexpress.com/early-access/api```
3. Run NonXAFSecurityBlazorServerSide project

