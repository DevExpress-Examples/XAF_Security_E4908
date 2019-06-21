<!-- default file list -->

# How to: Use the Integrated Mode of the Security System in Non-XAF Applications
## Console
The complete tutorial for console applications is available in the  [How to: Use the Integrated Mode of the Security System in Non-XAF Applications](http://documentation.devexpress.com/#Xaf/CustomDocument3558") topic.
## Blazor (server-side)
### Prerequisites

* [Visual Studio 2019 16.1 Preview 3 or later](https://visualstudio.com/preview) with following workloads:
  * **ASP.NET and web development**;
  * **.NET Core cross-platform development**.
* [.NET Core SDK 3 Preview 5 or later](https://www.microsoft.com/net/download/all)
* [DevExpress UI Components for Blazor (CTP)](https://www.devexpress.com/blazor/)

This example uses the [Server-side hosting model](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-3.0#server-side).

### Follow these steps to run the Blazor project:
1. Run the XafSolution.Win project and log in as "Admin" with an empty password to create the database.
2. Register the DevExpress Early Access feed in Visual Studio's NuGet Package Manager:
  * Open the "Package Manager Settings",
  * Add a new NuGet source:
  ```https://nuget.devexpress.com/early-access/api```.
3. Run the NonXAFSecurityBlazorServerSide project.
