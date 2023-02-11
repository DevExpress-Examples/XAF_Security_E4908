using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using WebAPI.BusinessObjects;

namespace WebAPI.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();

        var sampleUser = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "User");
        if(sampleUser == null) {
            sampleUser = ObjectSpace.CreateObject<ApplicationUser>();
            sampleUser.UserName = "User";
            // Set a password if the standard authentication type is used
            sampleUser.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            // ObjectSpace.CommitChanges(); //This line persists created object(s).
            // ((ISecurityUserWithLoginInfo)sampleUser).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(sampleUser));
        }
        var defaultRole = CreateDefaultRole();
        sampleUser.Roles.Add(defaultRole);

        var editorUser = ObjectSpace.FirstOrDefault<ApplicationUser>(user=>user.UserName=="Editor")??ObjectSpace.CreateObject<ApplicationUser>();
        if (ObjectSpace.IsNewObject(editorUser)) {
            //create Editor User/Role
            editorUser.UserName="Editor";

            var editorRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            editorRole.Name = "EditorRole";
            editorRole.AddTypePermission<Post>(SecurityOperations.CRUDAccess, SecurityPermissionState.Allow);
            editorRole.AddTypePermission<ApplicationUser>(SecurityOperations.CRUDAccess, SecurityPermissionState.Allow);

            editorUser.Roles.Add(editorRole);
            editorUser.Roles.Add(defaultRole);
            editorUser.Photo = ObjectSpace.CreateObject<MediaDataObject>();
            editorUser.Photo.MediaData = GetResourceByName("Janete");


            //create Viewer User/Role
            var viewerUser = ObjectSpace.CreateObject<ApplicationUser>();
            viewerUser.UserName = "Viewer";
            viewerUser.Photo = ObjectSpace.CreateObject<MediaDataObject>();
            viewerUser.Photo.MediaData = GetResourceByName("John");
            var viewerRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            viewerRole.Name = "ViewerRole";
            viewerRole.AddTypePermission<Post>(SecurityOperations.Read, SecurityPermissionState.Allow);
            viewerRole.AddTypePermission<ApplicationUser>(SecurityOperations.Read, SecurityPermissionState.Allow);
            viewerUser.Roles.Add(viewerRole);
            viewerUser.Roles.Add(defaultRole);

            //commit
            ObjectSpace.CommitChanges();

            //assign authentication type
            foreach (var user in new[] { editorUser, viewerUser }.Cast<ISecurityUserWithLoginInfo>()) {
	            user.CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication,
		            ObjectSpace.GetKeyValueAsString(user));
            }

            //sample posts
            var post = ObjectSpace.CreateObject<Post>();
            post.Title = "Hello World";
            post.Content = "This is a FREE API for everybody";
            post.Author=editorUser;
            post = ObjectSpace.CreateObject<Post>();
            post.Title = "Hello MAUI";
            post.Content = "Please smash the like button to help our videos get discovered";
            post.Author=editorUser;
        }

        var userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");
        if(userAdmin == null) {
            userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
            userAdmin.UserName = "Admin";
            // Set a password if the standard authentication type is used
            userAdmin.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
        }
        // If a role with the Administrators name doesn't exist in the database, create this role
        var adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if(adminRole == null) {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
        }
        adminRole.IsAdministrative = true;
		userAdmin.Roles.Add(adminRole);
        ObjectSpace.CommitChanges(); //This line persists created object(s).
    }

    private byte[] GetResourceByName(string shortName) {
	    string embeddedResourceName = Array.Find(GetType().Assembly.GetManifestResourceNames(), (s) => s.Contains(shortName));
	    var stream = GetType().Assembly.GetManifestResourceStream(embeddedResourceName!);
	    if(stream == null) {
		    throw new Exception($"Cannot read data from the {shortName} file!");
	    }

	    using var memoryStream = new MemoryStream();
       stream.CopyTo(memoryStream);
	    return memoryStream.ToArray();
    }
    private PermissionPolicyRole CreateDefaultRole() {
        var defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if(defaultRole == null) {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ReportDataV2>(SecurityOperations.CRUDAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<MediaDataObject>(SecurityOperations.CRUDAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<MediaResourceObject>(SecurityOperations.CRUDAccess, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
