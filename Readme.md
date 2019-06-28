<!-- default file list -->

# How to: Use the Integrated Mode of the Security System in Non-XAF Applications

## Scenario
You have an XAF desktop or web application with custom business objects, users, roles and their security permission settings stored in an existing database (see [XafSolution](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/19.1.4%2B/XafSolution)).
You want to develop an alternative UI client, technically a non-XAF .NET application, and reuse your existing business objects and access your existing XAF data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview). You want to save your time and do not want to re-implement all the data access and complex security authentication and authorization functions from scratch.

The list of .NET application platforms where ready-to-use XAF security system APIs can be used are:
* [Console](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/19.1.4%2B/Console)
* ASP.NET
  * Blazor
  * ASP.NET MVC 5
  * ASP.NET MVC Core
  * ASP.NET MVC Razor Pages
  * ASP.NET Web API with OData 4
* Xamarin.Forms
  * Android
  * IOS
  * UWP
  * [DevExpress Xamarin.Forms UI Controls](https://www.devexpress.com/xamarin/)
* Windows Forms
* WPF

>To help us prioritize future development, please tell us about the platforms and use-case scenarios you are most interested in.

## Main Features
The main XAF security system features commonly used in line-of-business application on all supported platforms are:

**1. Role-based access control with database permission storage**.

   1.1. Access control permissions linked to roles and users that can be stored in more than a dozen of popular databases powered by the XPO ORM or any custom storage.
  - [Type permissions](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview#type-permissions) grant Read, Write, Create, Delete access to all objects of a particular type.
  - [Object Permissions](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview#object-permissions) work in conjunction with Type Permissions and grant access to object instances that fit a specified criterion.
  - [Member Permissions](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview#member-permissions) grant access to specific members of an object unconditionally or by a criterion.

1.2. Powerful and [easy-to-use APIs](https://docs.devexpress.com/eXpressAppFramework/119065/concepts/security-system/predefined-users,-roles-and-permissions#set-permissions-for-non-administrative-roles) to configure users, roles and their permissions in code or visually in XAF apps.

1.3. Support for extension or replacement with [fully custom](https://docs.devexpress.com/eXpressAppFramework/113384/task-based-help/security/how-to-implement-custom-security-objects-users,-roles,-operation-permissions) user, role and permission objects to meet the needs of your business domain or implement various integration scenarios.

**2. Authentication**.

   2.1. Built-in [authentication types](https://docs.devexpress.com/eXpressAppFramework/119064/concepts/security-system/authentication): Forms (with username/password) and Active Directory (Windows user).

   2.2. Modern and secure algorithm for [password generation](https://docs.devexpress.com/eXpressAppFramework/112649/concepts/security-system/passwords-in-the-security-system) and validation.

   2.3. Support for extension or replacement with fully [custom strategies](https://docs.devexpress.com/eXpressAppFramework/119064/concepts/security-system/authentication#custom-authentication).

**3. Authorization**.

   3.1. Straighforward APIs to [check CRUD](https://docs.devexpress.com/eXpressAppFramework/112769/getting-started/comprehensive-tutorial/security-system/access-the-security-system-in-code) or other security operations for custom UI element customizations.

   3.2. Security permission caching for the best performance and two built-in [Permission Policies](https://docs.devexpress.com/eXpressAppFramework/116172/concepts/security-system/permission-policies) determine Security System's behavior when there are no explicitly specified permissions for a specific type, object or member.

   3.3. A [diagnostic tool](https://www.devexpress.com/Support/Center/Question/Details/T589182) to troubleshoot a complex security permission setup.
