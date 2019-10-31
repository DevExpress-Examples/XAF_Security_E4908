using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormsApplication {
	public partial class LoginForm : Page {
		protected void Page_Init(object sender, EventArgs e) {
			UserNameBox.Text = Request.Cookies.Get("userName")?.Value ?? "User";
			LoginButton.Focus();
		}
		protected void LoginButton_Click(object sender, EventArgs e) {
			string userName = UserNameBox.Text;
			string password = PasswordBox.Text;
			AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
			SecurityStrategyComplex security = ConnectionHelper.GetSecurity(typeof(AuthenticationStandardProvider).Name, parameters);
			SecuredObjectSpaceProvider objectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(security);
			IObjectSpace logonObjectSpace = objectSpaceProvider.CreateObjectSpace();
			try {
				security.Logon(logonObjectSpace);
			}
			catch {	}
			if(security.IsAuthenticated) {
				SetCookie(userName);
				FormsAuthentication.RedirectFromLoginPage(userName, true);
			}
			else {
				ClientScript.RegisterStartupScript(GetType(), null, "errorMessage();", true);
			}
			security.Dispose();
			objectSpaceProvider.Dispose();
		}
		private void SetCookie(string userName) {
			HttpCookie cookie = new HttpCookie("userName", userName);
			HttpContext.Current.Response.Cookies.Add(cookie);
		}
	}
}