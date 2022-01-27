using System.Configuration;
using System.Diagnostics;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using BusinessObjectsLibrary.BusinessObjects;
using DatabaseUpdater;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;

// ## Step 0. Preparation. Create or update database
TypesInfo typesInfo = new TypesInfo();
RegisterEntities(typesInfo);
string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
IXpoDataStoreProvider dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null);
CreateDemoData(typesInfo, dataStoreProvider);

// ## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options
AuthenticationStandard authentication = new AuthenticationStandard();
SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
security.RegisterXPOAdapterProviders();
SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, dataStoreProvider, typesInfo, null);

// ## Step 2. Authentication. Log in as a 'User' with an Empty Password
authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName: "User", password: string.Empty));
IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
security.Logon(loginObjectSpace);

// ## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights
Console.WriteLine($"{"Full Name",-40}{"Department",-40}");
using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
	// User cannot read protected entities like PermissionPolicyRole.
	Debug.Assert(securedObjectSpace.GetObjects<PermissionPolicyRole>().Count == 0);
	foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) { // User can read Employee data.
		// User can read Department data by criteria.
		bool canRead = security.CanRead(securedObjectSpace, employee, memberName: nameof(Employee.Department));
		Debug.Assert(!canRead == (employee.Department == null));
		// Mask protected property values when User has no 'Read' permission.
		var department = canRead ? employee.Department.Title : "*******";
		Console.WriteLine($"{employee.FullName,-40}{department,-40}");
	}
}
security.Logoff();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static void RegisterEntities(TypesInfo typesInfo) {
	typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
	typesInfo.RegisterEntity(typeof(Employee));
	typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
	typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
}

static void CreateDemoData(TypesInfo typesInfo, IXpoDataStoreProvider dataStoreProvider) {
	using (var objectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, typesInfo, null)) {
		using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
			new Updater(objectSpace).UpdateDatabase();
		}
	}
}
