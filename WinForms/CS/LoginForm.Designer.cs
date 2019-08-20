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
			this.login_button = new System.Windows.Forms.Button();
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.passwordEdit = new DevExpress.XtraEditors.TextEdit();
			this.userNameEdit = new DevExpress.XtraEditors.TextEdit();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.passwordEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.userNameEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			this.SuspendLayout();
			// 
			// login_button
			// 
			this.login_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.login_button.Location = new System.Drawing.Point(138, 164);
			this.login_button.Name = "login_button";
			this.login_button.Size = new System.Drawing.Size(76, 24);
			this.login_button.TabIndex = 0;
			this.login_button.Text = "Login";
			this.login_button.UseVisualStyleBackColor = true;
			this.login_button.Click += new System.EventHandler(this.Login_button_Click);
			// 
			// layoutControl1
			// 
			this.layoutControl1.Controls.Add(this.passwordEdit);
			this.layoutControl1.Controls.Add(this.userNameEdit);
			this.layoutControl1.Location = new System.Drawing.Point(12, 44);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.Root;
			this.layoutControl1.Size = new System.Drawing.Size(336, 84);
			this.layoutControl1.TabIndex = 5;
			this.layoutControl1.Text = "layoutControl1";
			// 
			// passwordEdit
			// 
			this.passwordEdit.Location = new System.Drawing.Point(66, 36);
			this.passwordEdit.Name = "passwordEdit";
			this.passwordEdit.Properties.PasswordChar = '*';
			this.passwordEdit.Size = new System.Drawing.Size(258, 20);
			this.passwordEdit.StyleController = this.layoutControl1;
			this.passwordEdit.TabIndex = 5;
			// 
			// userNameEdit
			// 
			this.userNameEdit.Location = new System.Drawing.Point(66, 12);
			this.userNameEdit.Name = "userNameEdit";
			this.userNameEdit.Size = new System.Drawing.Size(258, 20);
			this.userNameEdit.StyleController = this.layoutControl1;
			this.userNameEdit.TabIndex = 4;
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
			this.layoutControlItem1,
			this.emptySpaceItem1,
			this.layoutControlItem2});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(336, 84);
			this.Root.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.userNameEdit;
			this.layoutControlItem1.CustomizationFormText = "User name";
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(316, 24);
			this.layoutControlItem1.Text = "User name";
			this.layoutControlItem1.TextSize = new System.Drawing.Size(51, 13);
			// 
			// emptySpaceItem1
			// 
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 48);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(316, 16);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.passwordEdit;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(316, 24);
			this.layoutControlItem2.Text = "Password";
			this.layoutControlItem2.TextSize = new System.Drawing.Size(51, 13);
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(24, 15);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(262, 13);
			this.labelControl1.TabIndex = 6;
			this.labelControl1.Text = "Welcome to the XAF\'s Security in Non-XAF apps Demo!";
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(24, 34);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(241, 13);
			this.labelControl2.TabIndex = 7;
			this.labelControl2.Text = "Enter your user name (Admin or User) to proceed.";
			// 
			// labelControl3
			// 
			this.labelControl3.Location = new System.Drawing.Point(24, 119);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(251, 13);
			this.labelControl3.TabIndex = 8;
			this.labelControl3.Text = "This demo app does not require a password for login";
			// 
			// LoginForm
			// 
			this.AcceptButton = this.login_button;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(358, 197);
			this.Controls.Add(this.labelControl3);
			this.Controls.Add(this.labelControl2);
			this.Controls.Add(this.labelControl1);
			this.Controls.Add(this.layoutControl1);
			this.Controls.Add(this.login_button);
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Authentication";
			this.Icon = WindowsFormsApplication.Properties.Resources.ExpressApp;
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.passwordEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.userNameEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button login_button;
		private DevExpress.XtraLayout.LayoutControl layoutControl1;
		private DevExpress.XtraEditors.TextEdit passwordEdit;
		private DevExpress.XtraEditors.TextEdit userNameEdit;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.LabelControl labelControl3;
	}
}

