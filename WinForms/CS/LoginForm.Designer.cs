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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.buttonLogin = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.pictureEditImage = new DevExpress.XtraEditors.PictureEdit();
            this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.passwordEdit = new DevExpress.XtraEditors.TextEdit();
            this.userNameEdit = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemUserName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemPassword = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItemImage = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemLogin = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditImage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userNameEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLogin
            // 
            this.buttonLogin.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogin.Appearance.Options.UseFont = true;
            this.buttonLogin.Location = new System.Drawing.Point(265, 121);
            this.buttonLogin.MaximumSize = new System.Drawing.Size(70, 26);
            this.buttonLogin.MinimumSize = new System.Drawing.Size(70, 26);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(70, 26);
            this.buttonLogin.StyleController = this.layoutControl1;
            this.buttonLogin.TabIndex = 0;
            this.buttonLogin.Text = "Log In";
            this.buttonLogin.Click += new System.EventHandler(this.Login_button_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomization = false;
            this.layoutControl1.Controls.Add(this.pictureEditImage);
            this.layoutControl1.Controls.Add(this.cancelButton);
            this.layoutControl1.Controls.Add(this.buttonLogin);
            this.layoutControl1.Controls.Add(this.passwordEdit);
            this.layoutControl1.Controls.Add(this.userNameEdit);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(414, 240, 298, 800);
            this.layoutControl1.OptionsFocus.EnableAutoTabOrder = false;
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(415, 153);
            this.layoutControl1.TabIndex = 5;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // pictureEditImage
            // 
            this.pictureEditImage.EditValue = ((object)(resources.GetObject("pictureEditImage.EditValue")));
            this.pictureEditImage.Location = new System.Drawing.Point(6, 6);
            this.pictureEditImage.Margin = new System.Windows.Forms.Padding(2);
            this.pictureEditImage.MaximumSize = new System.Drawing.Size(80, 80);
            this.pictureEditImage.MinimumSize = new System.Drawing.Size(80, 80);
            this.pictureEditImage.Name = "pictureEditImage";
            this.pictureEditImage.Properties.AllowFocused = false;
            this.pictureEditImage.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEditImage.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEditImage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEditImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEditImage.Properties.ShowMenu = false;
            this.pictureEditImage.Properties.SvgImageSize = new System.Drawing.Size(70, 70);
            this.pictureEditImage.Size = new System.Drawing.Size(80, 80);
            this.pictureEditImage.StyleController = this.layoutControl1;
            this.pictureEditImage.TabIndex = 7;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(339, 121);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.MaximumSize = new System.Drawing.Size(70, 24);
            this.cancelButton.MinimumSize = new System.Drawing.Size(70, 24);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(70, 24);
            this.cancelButton.StyleController = this.layoutControl1;
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // passwordEdit
            // 
            this.passwordEdit.Location = new System.Drawing.Point(150, 47);
            this.passwordEdit.Name = "passwordEdit";
            this.passwordEdit.Properties.PasswordChar = '*';
            this.passwordEdit.Size = new System.Drawing.Size(259, 20);
            this.passwordEdit.StyleController = this.layoutControl1;
            this.passwordEdit.TabIndex = 3;
            // 
            // userNameEdit
            // 
            this.dxErrorProvider1.SetIconAlignment(this.userNameEdit, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
            this.userNameEdit.Location = new System.Drawing.Point(150, 23);
            this.userNameEdit.Name = "userNameEdit";
            this.userNameEdit.Size = new System.Drawing.Size(259, 20);
            this.userNameEdit.StyleController = this.layoutControl1;
            this.userNameEdit.TabIndex = 2;
            this.userNameEdit.Validating += new System.ComponentModel.CancelEventHandler(this.UserNameEdit_Validating);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemUserName,
            this.layoutControlItemPassword,
            this.simpleLabelItem1,
            this.simpleLabelItem2,
            this.emptySpaceItem2,
            this.layoutControlItemImage,
            this.layoutControlGroupButtons});
            this.Root.Name = "Root";
            this.Root.OptionsItemText.TextToControlDistance = 4;
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(415, 153);
            this.Root.TextVisible = false;
            // 
            // layoutControlItemUserName
            // 
            this.layoutControlItemUserName.Control = this.userNameEdit;
            this.layoutControlItemUserName.CustomizationFormText = "User Name";
            this.layoutControlItemUserName.Location = new System.Drawing.Point(84, 17);
            this.layoutControlItemUserName.Name = "layoutControlItemUserName";
            this.layoutControlItemUserName.Size = new System.Drawing.Size(323, 24);
            this.layoutControlItemUserName.Text = "User Name:";
            this.layoutControlItemUserName.TextSize = new System.Drawing.Size(56, 13);
            // 
            // layoutControlItemPassword
            // 
            this.layoutControlItemPassword.Control = this.passwordEdit;
            this.layoutControlItemPassword.Location = new System.Drawing.Point(84, 41);
            this.layoutControlItemPassword.Name = "layoutControlItemPassword";
            this.layoutControlItemPassword.Size = new System.Drawing.Size(323, 24);
            this.layoutControlItemPassword.Text = "Password:";
            this.layoutControlItemPassword.TextSize = new System.Drawing.Size(56, 13);
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AllowHotTrack = false;
            this.simpleLabelItem1.Location = new System.Drawing.Point(84, 65);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(323, 17);
            this.simpleLabelItem1.Text = "This demo app does not require a password for login";
            this.simpleLabelItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(251, 13);
            // 
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.AllowHotTrack = false;
            this.simpleLabelItem2.Location = new System.Drawing.Point(84, 0);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.Size = new System.Drawing.Size(323, 17);
            this.simpleLabelItem2.Text = "Enter your user name (Admin or User) to proceed.";
            this.simpleLabelItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(241, 13);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(84, 82);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(323, 10);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItemImage
            // 
            this.layoutControlItemImage.Control = this.pictureEditImage;
            this.layoutControlItemImage.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemImage.Name = "layoutControlItemImage";
            this.layoutControlItemImage.Size = new System.Drawing.Size(84, 92);
            this.layoutControlItemImage.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemImage.TextVisible = false;
            // 
            // layoutControlGroupButtons
            // 
            this.layoutControlGroupButtons.GroupBordersVisible = false;
            this.layoutControlGroupButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemLogin,
            this.layoutControlItemCancel,
            this.emptySpaceItem1,
            this.emptySpaceItem3});
            this.layoutControlGroupButtons.Location = new System.Drawing.Point(0, 92);
            this.layoutControlGroupButtons.Name = "layoutControlGroupButtons";
            this.layoutControlGroupButtons.OptionsItemText.TextToControlDistance = 0;
            this.layoutControlGroupButtons.Size = new System.Drawing.Size(407, 53);
            this.layoutControlGroupButtons.TextVisible = false;
            // 
            // layoutControlItemLogin
            // 
            this.layoutControlItemLogin.Control = this.buttonLogin;
            this.layoutControlItemLogin.Location = new System.Drawing.Point(259, 23);
            this.layoutControlItemLogin.Name = "layoutControlItemLogin";
            this.layoutControlItemLogin.Size = new System.Drawing.Size(74, 30);
            this.layoutControlItemLogin.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemLogin.TextVisible = false;
            // 
            // layoutControlItemCancel
            // 
            this.layoutControlItemCancel.Control = this.cancelButton;
            this.layoutControlItemCancel.Location = new System.Drawing.Point(333, 23);
            this.layoutControlItemCancel.Name = "layoutControlItemCancel";
            this.layoutControlItemCancel.Size = new System.Drawing.Size(74, 30);
            this.layoutControlItemCancel.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemCancel.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 23);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(259, 30);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(407, 23);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
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
            this.ClientSize = new System.Drawing.Size(415, 153);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditImage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userNameEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
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
        private DevExpress.XtraEditors.SimpleButton cancelButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCancel;
        private DevExpress.XtraEditors.PictureEdit pictureEditImage;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemImage;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupButtons;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
    }
}

