namespace WindowsFormsApplication {
	partial class LoginForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.buttonLogin = new DevExpress.XtraEditors.SimpleButton();
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
			this.passwordEdit = new DevExpress.XtraEditors.TextEdit();
			this.userNameEdit = new DevExpress.XtraEditors.TextEdit();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlGroupButtons = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItemLogin = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItemCancel = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem7 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem8 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
			this.layoutControlItemUserName = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItemPassword = new DevExpress.XtraLayout.LayoutControlItem();
			this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
			this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
			this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem9 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem10 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.passwordEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.userNameEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupButtons)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLogin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUserName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPassword)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonLogin
			// 
			this.buttonLogin.Location = new System.Drawing.Point(316, 211);
			this.buttonLogin.Margin = new System.Windows.Forms.Padding(2);
			this.buttonLogin.MaximumSize = new System.Drawing.Size(70, 26);
			this.buttonLogin.MinimumSize = new System.Drawing.Size(70, 26);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(70, 26);
			this.buttonLogin.StyleController = this.layoutControl1;
			this.buttonLogin.TabIndex = 0;
			this.buttonLogin.Text = "Log In";
			this.buttonLogin.Click += new System.EventHandler(this.Login_Click);
			// 
			// layoutControl1
			// 
			this.layoutControl1.AllowCustomization = false;
			this.layoutControl1.Controls.Add(this.buttonCancel);
			this.layoutControl1.Controls.Add(this.buttonLogin);
			this.layoutControl1.Controls.Add(this.passwordEdit);
			this.layoutControl1.Controls.Add(this.userNameEdit);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(624, 143, 742, 800);
			this.layoutControl1.OptionsFocus.EnableAutoTabOrder = false;
			this.layoutControl1.Root = this.Root;
			this.layoutControl1.Size = new System.Drawing.Size(478, 255);
			this.layoutControl1.TabIndex = 5;
			this.layoutControl1.Text = "layoutControl1";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(390, 211);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
			this.buttonCancel.MaximumSize = new System.Drawing.Size(70, 26);
			this.buttonCancel.MinimumSize = new System.Drawing.Size(70, 26);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(70, 26);
			this.buttonCancel.StyleController = this.layoutControl1;
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// passwordEdit
			// 
			this.passwordEdit.Location = new System.Drawing.Point(83, 113);
			this.passwordEdit.Name = "passwordEdit";
			this.passwordEdit.Properties.NullValuePrompt = "Password";
			this.passwordEdit.Properties.PasswordChar = '*';
			this.passwordEdit.Size = new System.Drawing.Size(310, 20);
			this.passwordEdit.StyleController = this.layoutControl1;
			this.passwordEdit.TabIndex = 3;
			// 
			// userNameEdit
			// 
			this.dxErrorProvider1.SetIconAlignment(this.userNameEdit, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
			this.userNameEdit.Location = new System.Drawing.Point(83, 89);
			this.userNameEdit.Name = "userNameEdit";
			this.userNameEdit.Size = new System.Drawing.Size(310, 20);
			this.userNameEdit.StyleController = this.layoutControl1;
			this.userNameEdit.TabIndex = 2;
			this.userNameEdit.Validating += new System.ComponentModel.CancelEventHandler(this.UserNameEdit_Validating);
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
			this.layoutControlGroupButtons,
			this.layoutControlGroup1,
			this.emptySpaceItem4});
			this.Root.Name = "Root";
			this.Root.OptionsItemText.TextToControlDistance = 4;
			this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
			this.Root.Size = new System.Drawing.Size(478, 255);
			this.Root.TextVisible = false;
			// 
			// layoutControlGroupButtons
			// 
			this.layoutControlGroupButtons.GroupBordersVisible = false;
			this.layoutControlGroupButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
			this.layoutControlItemLogin,
			this.layoutControlItemCancel,
			this.emptySpaceItem1,
			this.emptySpaceItem7,
			this.emptySpaceItem8});
			this.layoutControlGroupButtons.Location = new System.Drawing.Point(0, 205);
			this.layoutControlGroupButtons.Name = "layoutControlGroupButtons";
			this.layoutControlGroupButtons.OptionsItemText.TextToControlDistance = 0;
			this.layoutControlGroupButtons.Size = new System.Drawing.Size(470, 42);
			this.layoutControlGroupButtons.TextVisible = false;
			// 
			// layoutControlItemLogin
			// 
			this.layoutControlItemLogin.Control = this.buttonLogin;
			this.layoutControlItemLogin.Location = new System.Drawing.Point(310, 0);
			this.layoutControlItemLogin.Name = "layoutControlItemLogin";
			this.layoutControlItemLogin.Size = new System.Drawing.Size(74, 30);
			this.layoutControlItemLogin.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItemLogin.TextVisible = false;
			// 
			// layoutControlItemCancel
			// 
			this.layoutControlItemCancel.Control = this.buttonCancel;
			this.layoutControlItemCancel.Location = new System.Drawing.Point(384, 0);
			this.layoutControlItemCancel.Name = "layoutControlItemCancel";
			this.layoutControlItemCancel.Size = new System.Drawing.Size(74, 30);
			this.layoutControlItemCancel.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItemCancel.TextVisible = false;
			// 
			// emptySpaceItem1
			// 
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(310, 30);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			// 
			// emptySpaceItem7
			// 
			this.emptySpaceItem7.AllowHotTrack = false;
			this.emptySpaceItem7.Location = new System.Drawing.Point(458, 0);
			this.emptySpaceItem7.Name = "emptySpaceItem7";
			this.emptySpaceItem7.Size = new System.Drawing.Size(12, 30);
			this.emptySpaceItem7.TextSize = new System.Drawing.Size(0, 0);
			// 
			// emptySpaceItem8
			// 
			this.emptySpaceItem8.AllowHotTrack = false;
			this.emptySpaceItem8.Location = new System.Drawing.Point(0, 30);
			this.emptySpaceItem8.Name = "emptySpaceItem8";
			this.emptySpaceItem8.Size = new System.Drawing.Size(470, 12);
			this.emptySpaceItem8.TextSize = new System.Drawing.Size(0, 0);
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
			this.simpleLabelItem2,
			this.layoutControlItemUserName,
			this.emptySpaceItem2,
			this.layoutControlItemPassword,
			this.simpleLabelItem1,
			this.simpleLabelItem3,
			this.emptySpaceItem5,
			this.emptySpaceItem6,
			this.emptySpaceItem9,
			this.emptySpaceItem10});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 44);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Size = new System.Drawing.Size(470, 161);
			this.layoutControlGroup1.TextVisible = false;
			// 
			// simpleLabelItem2
			// 
			this.simpleLabelItem2.AllowHotTrack = false;
			this.simpleLabelItem2.AllowHtmlStringInCaption = true;
			this.simpleLabelItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.simpleLabelItem2.AppearanceItemCaption.Options.UseFont = true;
			this.simpleLabelItem2.Location = new System.Drawing.Point(77, 0);
			this.simpleLabelItem2.Name = "simpleLabelItem2";
			this.simpleLabelItem2.Size = new System.Drawing.Size(314, 17);
			this.simpleLabelItem2.Text = "Enter your user name (<b>Admin</b> or <b>User</b>) to proceed.";
			this.simpleLabelItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
			this.simpleLabelItem2.TextSize = new System.Drawing.Size(252, 13);
			// 
			// layoutControlItemUserName
			// 
			this.layoutControlItemUserName.Control = this.userNameEdit;
			this.layoutControlItemUserName.CustomizationFormText = "User Name";
			this.layoutControlItemUserName.Location = new System.Drawing.Point(77, 39);
			this.layoutControlItemUserName.Name = "layoutControlItemUserName";
			this.layoutControlItemUserName.Size = new System.Drawing.Size(314, 24);
			this.layoutControlItemUserName.Text = "User Name:";
			this.layoutControlItemUserName.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItemUserName.TextVisible = false;
			// 
			// emptySpaceItem2
			// 
			this.emptySpaceItem2.AllowHotTrack = false;
			this.emptySpaceItem2.Location = new System.Drawing.Point(77, 133);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(314, 28);
			this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
			// 
			// layoutControlItemPassword
			// 
			this.layoutControlItemPassword.Control = this.passwordEdit;
			this.layoutControlItemPassword.Location = new System.Drawing.Point(77, 63);
			this.layoutControlItemPassword.Name = "layoutControlItemPassword";
			this.layoutControlItemPassword.Size = new System.Drawing.Size(314, 24);
			this.layoutControlItemPassword.Text = "Password:";
			this.layoutControlItemPassword.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItemPassword.TextVisible = false;
			// 
			// simpleLabelItem1
			// 
			this.simpleLabelItem1.AllowHotTrack = false;
			this.simpleLabelItem1.Enabled = false;
			this.simpleLabelItem1.Location = new System.Drawing.Point(77, 99);
			this.simpleLabelItem1.Name = "simpleLabelItem1";
			this.simpleLabelItem1.Size = new System.Drawing.Size(314, 17);
			this.simpleLabelItem1.Text = "This demo app does not require";
			this.simpleLabelItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
			this.simpleLabelItem1.TextSize = new System.Drawing.Size(151, 13);
			// 
			// simpleLabelItem3
			// 
			this.simpleLabelItem3.AllowHotTrack = false;
			this.simpleLabelItem3.Enabled = false;
			this.simpleLabelItem3.Location = new System.Drawing.Point(77, 116);
			this.simpleLabelItem3.Name = "simpleLabelItem3";
			this.simpleLabelItem3.Size = new System.Drawing.Size(314, 17);
			this.simpleLabelItem3.Text = "a password for login";
			this.simpleLabelItem3.TextSize = new System.Drawing.Size(97, 13);
			// 
			// emptySpaceItem5
			// 
			this.emptySpaceItem5.AllowHotTrack = false;
			this.emptySpaceItem5.Location = new System.Drawing.Point(391, 0);
			this.emptySpaceItem5.Name = "emptySpaceItem5";
			this.emptySpaceItem5.Size = new System.Drawing.Size(79, 161);
			this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
			// 
			// emptySpaceItem6
			// 
			this.emptySpaceItem6.AllowHotTrack = false;
			this.emptySpaceItem6.Location = new System.Drawing.Point(0, 0);
			this.emptySpaceItem6.Name = "emptySpaceItem6";
			this.emptySpaceItem6.Size = new System.Drawing.Size(77, 161);
			this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
			// 
			// emptySpaceItem9
			// 
			this.emptySpaceItem9.AllowHotTrack = false;
			this.emptySpaceItem9.Location = new System.Drawing.Point(77, 17);
			this.emptySpaceItem9.Name = "emptySpaceItem9";
			this.emptySpaceItem9.Size = new System.Drawing.Size(314, 22);
			this.emptySpaceItem9.TextSize = new System.Drawing.Size(0, 0);
			// 
			// emptySpaceItem10
			// 
			this.emptySpaceItem10.AllowHotTrack = false;
			this.emptySpaceItem10.Location = new System.Drawing.Point(77, 87);
			this.emptySpaceItem10.Name = "emptySpaceItem10";
			this.emptySpaceItem10.Size = new System.Drawing.Size(314, 12);
			this.emptySpaceItem10.TextSize = new System.Drawing.Size(0, 0);
			// 
			// emptySpaceItem4
			// 
			this.emptySpaceItem4.AllowHotTrack = false;
			this.emptySpaceItem4.Location = new System.Drawing.Point(0, 0);
			this.emptySpaceItem4.Name = "emptySpaceItem4";
			this.emptySpaceItem4.Size = new System.Drawing.Size(470, 44);
			this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
			// 
			// dxErrorProvider1
			// 
			this.dxErrorProvider1.ContainerControl = this;
			// 
			// LoginForm
			// 
			this.AcceptButton = this.buttonLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 255);
			this.Controls.Add(this.layoutControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = global::WindowsFormsApplication.Properties.Resources.ExpressApp;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Welcome to the XAF\'s Security in Non-XAF Apps Demo!";
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.passwordEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.userNameEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupButtons)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLogin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUserName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPassword)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.SimpleButton buttonLogin;
		private DevExpress.XtraLayout.LayoutControl layoutControl1;
		private DevExpress.XtraEditors.TextEdit passwordEdit;
		private DevExpress.XtraEditors.TextEdit userNameEdit;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItemUserName;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItemPassword;
		private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
		private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem2;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItemLogin;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
		private DevExpress.XtraEditors.SimpleButton buttonCancel;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCancel;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupButtons;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
		private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
		private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem3;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem6;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem7;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem8;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem9;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem10;
	}
}

