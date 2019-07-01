<!-- default file list -->

# How to: Use the Integrated Mode of the Security System in Non-XAF Applications

## Scenario
You have an XAF desktop or web application with custom business objects, users, roles and their security permission settings stored in an existing database (see [XafSolution](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/19.1.4%2B/XafSolution)).
You want to develop an alternative UI client, which is technically a non-XAF .NET application. Also, you need to reuse your existing business objects and access your existing XAF data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview). You want to save your time and do not want to re-implement all the data access, complex security authentication, and authorization functions from scratch.

.NET application platforms where ready-to-use XAF security system APIs can be used:
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

>To help us prioritize our future development, please tell us about the platforms and use-case scenarios you are most interested in.
## Main Features
The main XAF security system features commonly used in line-of-business applications on all supported platforms include:

**1. Role-based access control with database permission storage.**

   1.1. Access control permissions linked to roles and users that can be stored in more than a dozen of popular databases powered by the XPO ORM or any custom storage.
  - [Type permissions](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview#type-permissions) grant Read, Write, Create, and Delete access to all objects that belong to a particular type.
  - [Object Permissions](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview#object-permissions) work in conjunction with Type Permissions and grant access to object instances that fit a specified criterion.
  - [Member Permissions](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview#member-permissions) grant access to specific members unconditionally or based on a criterion.

   1.2. Powerful and [easy-to-use APIs](https://docs.devexpress.com/eXpressAppFramework/119065/concepts/security-system/predefined-users,-roles-and-permissions#set-permissions-for-non-administrative-roles) to configure users, roles and their permissions in code or visually in XAF apps.

   1.3. Support for an extension or replacement with a [fully custom](https://docs.devexpress.com/eXpressAppFramework/113384/task-based-help/security/how-to-implement-custom-security-objects-users,-roles,-operation-permissions) user, role, and permission objects to meet the needs of your business domain or implement various integration scenarios.

**2. Authentication**.

   2.1. Built-in [authentication types](https://docs.devexpress.com/eXpressAppFramework/119064/concepts/security-system/authentication): Forms (with username/password) and Active Directory (Windows user).

   2.2. A modern and secure algorithm for [password generation](https://docs.devexpress.com/eXpressAppFramework/112649/concepts/security-system/passwords-in-the-security-system) and validation.

   2.3. Support for extension or replacement with fully [custom authentication strategies and logon parameters](https://docs.devexpress.com/eXpressAppFramework/119064/concepts/security-system/authentication#custom-authentication).

**3. Authorization**.

   3.1. Just two code lines to read secure records filtered out by the currently logged user, roles and their permissions. When you set up [SecuredObjectSpaceProvider](https://documentation.devexpress.com/eXpressAppFramework/113437/Task-Based-Help/Security/How-to-Change-the-Client-Side-Security-Mode-from-UI-Level-to-Integrated-in-XPO-applications), you can create an unlimited number of secure data contexts - your data query and modification APIs will be unchanged.
  
   3.2. Fine-grained access control for base and inherited objects, one to many and many to many object [relationships](https://docs.devexpress.com/eXpressAppFramework/116170/concepts/security-system/permissions-for-associated-objects), individual columns with or without criteria (example: can read the Full Name field, but cannot see and modify Salary) and specific object instances only.
   
   3.3. Straightforward APIs to [check CRUD](https://docs.devexpress.com/eXpressAppFramework/112769/getting-started/comprehensive-tutorial/security-system/access-the-security-system-in-code) or custom access rights for UI element customizations. With that, you can hide or mask protected grid columns, editors in detail forms as well as disable menu toolbar commands like New, Delete, Edit, etc.

   3.4. Security permission caching for the best performance and two built-in [Permission Policies](https://docs.devexpress.com/eXpressAppFramework/116172/concepts/security-system/permission-policies) determine Security System's behavior when there are no explicitly specified permissions for a specific type, object, or member.
  
   3.5. Proven by production with thousands of complex customer applications in the last decade. DevExpress Support and a [diagnostic tool](https://www.devexpress.com/Support/Center/Question/Details/T589182) are always at your service to troubleshoot complex security permission configurations in case of any issues.
