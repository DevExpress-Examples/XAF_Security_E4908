using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;

namespace WebAPI.BusinessObjects;

[DefaultProperty(nameof(UserName))]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo {

	[Browsable(false)]
	[DevExpress.ExpressApp.DC.Aggregated]
	public virtual ObservableCollection<ApplicationUserLoginInfo> UserLogins { get; set; }=new();
	IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => UserLogins;

	ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey) {
		ApplicationUserLoginInfo result = ((IObjectSpaceLink)this).ObjectSpace.CreateObject<ApplicationUserLoginInfo>();
		result.LoginProviderName = loginProviderName;
		result.ProviderUserKey = providerUserKey;
		result.User = this;
		return result;
	}

	public virtual MediaDataObject Photo { get; set; }

    [FieldSize(255)]
    public virtual String Email { get; set; }
}


