Namespace WindowsFormsApplication
	Partial Public Class LoginForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(LoginForm))
			Me.buttonLogin = New DevExpress.XtraEditors.SimpleButton()
			Me.layoutControl1 = New DevExpress.XtraLayout.LayoutControl()
			Me.pictureEditImage = New DevExpress.XtraEditors.PictureEdit()
			Me.cancelButton = New DevExpress.XtraEditors.SimpleButton()
			Me.passwordEdit = New DevExpress.XtraEditors.TextEdit()
			Me.userNameEdit = New DevExpress.XtraEditors.TextEdit()
			Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
			Me.layoutControlItemUserName = New DevExpress.XtraLayout.LayoutControlItem()
			Me.layoutControlItemPassword = New DevExpress.XtraLayout.LayoutControlItem()
			Me.simpleLabelItem1 = New DevExpress.XtraLayout.SimpleLabelItem()
			Me.simpleLabelItem2 = New DevExpress.XtraLayout.SimpleLabelItem()
			Me.emptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
			Me.layoutControlItemImage = New DevExpress.XtraLayout.LayoutControlItem()
			Me.layoutControlGroupButtons = New DevExpress.XtraLayout.LayoutControlGroup()
			Me.layoutControlItemLogin = New DevExpress.XtraLayout.LayoutControlItem()
			Me.layoutControlItemCancel = New DevExpress.XtraLayout.LayoutControlItem()
			Me.emptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
			Me.emptySpaceItem3 = New DevExpress.XtraLayout.EmptySpaceItem()
			Me.dxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
			CType(Me.layoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.layoutControl1.SuspendLayout()
			CType(Me.pictureEditImage.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.passwordEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.userNameEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlItemUserName, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlItemPassword, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.simpleLabelItem1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.simpleLabelItem2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.emptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlItemImage, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlGroupButtons, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlItemLogin, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlItemCancel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.emptySpaceItem3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' buttonLogin
			' 
			Me.buttonLogin.Location = New System.Drawing.Point(265, 121)
            Me.buttonLogin.Margin = New System.Windows.Forms.Padding(2)
			Me.buttonLogin.MaximumSize = New System.Drawing.Size(70, 26)
			Me.buttonLogin.MinimumSize = New System.Drawing.Size(70, 26)
			Me.buttonLogin.Name = "buttonLogin"
			Me.buttonLogin.Size = New System.Drawing.Size(70, 26)
			Me.buttonLogin.StyleController = Me.layoutControl1
			Me.buttonLogin.TabIndex = 0
			Me.buttonLogin.Text = "Log In"
			' 
			' layoutControl1
			' 
			Me.layoutControl1.AllowCustomization = False
			Me.layoutControl1.Controls.Add(Me.pictureEditImage)
			Me.layoutControl1.Controls.Add(Me.cancelButton)
			Me.layoutControl1.Controls.Add(Me.buttonLogin)
			Me.layoutControl1.Controls.Add(Me.passwordEdit)
			Me.layoutControl1.Controls.Add(Me.userNameEdit)
			Me.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.layoutControl1.Location = New System.Drawing.Point(0, 0)
			Me.layoutControl1.Name = "layoutControl1"
			Me.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(414, 240, 298, 800)
			Me.layoutControl1.OptionsFocus.EnableAutoTabOrder = False
			Me.layoutControl1.Root = Me.Root
			Me.layoutControl1.Size = New System.Drawing.Size(415, 153)
			Me.layoutControl1.TabIndex = 5
			Me.layoutControl1.Text = "layoutControl1"
			' 
			' pictureEditImage
			' 
			Me.pictureEditImage.EditValue = (CObj(resources.GetObject("pictureEditImage.EditValue")))
			Me.pictureEditImage.Location = New System.Drawing.Point(6, 6)
			Me.pictureEditImage.Margin = New System.Windows.Forms.Padding(2)
			Me.pictureEditImage.MaximumSize = New System.Drawing.Size(80, 80)
			Me.pictureEditImage.MinimumSize = New System.Drawing.Size(80, 80)
			Me.pictureEditImage.Name = "pictureEditImage"
			Me.pictureEditImage.Properties.AllowFocused = False
			Me.pictureEditImage.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.pictureEditImage.Properties.Appearance.Options.UseBackColor = True
			Me.pictureEditImage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.pictureEditImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto
			Me.pictureEditImage.Properties.ShowMenu = False
			Me.pictureEditImage.Properties.SvgImageSize = New System.Drawing.Size(64, 64)
			Me.pictureEditImage.Size = New System.Drawing.Size(80, 80)
			Me.pictureEditImage.StyleController = Me.layoutControl1
			Me.pictureEditImage.TabIndex = 7
			' 
			' cancelButton
			' 
			Me.cancelButton.Location = New System.Drawing.Point(339, 121)
			Me.cancelButton.Margin = New System.Windows.Forms.Padding(2)
			Me.cancelButton.MaximumSize = New System.Drawing.Size(70, 26)
			Me.cancelButton.MinimumSize = New System.Drawing.Size(70, 26)
			Me.cancelButton.Name = "cancelButton"
			Me.cancelButton.Size = New System.Drawing.Size(70, 26)
			Me.cancelButton.StyleController = Me.layoutControl1
			Me.cancelButton.TabIndex = 1
			Me.cancelButton.Text = "Cancel"
			' 
			' passwordEdit
			' 
			Me.passwordEdit.Location = New System.Drawing.Point(150, 47)
			Me.passwordEdit.Name = "passwordEdit"
			Me.passwordEdit.Properties.PasswordChar = "*"c
			Me.passwordEdit.Size = New System.Drawing.Size(259, 20)
			Me.passwordEdit.StyleController = Me.layoutControl1
			Me.passwordEdit.TabIndex = 3
			' 
			' userNameEdit
			' 
			Me.dxErrorProvider1.SetIconAlignment(Me.userNameEdit, System.Windows.Forms.ErrorIconAlignment.MiddleRight)
			Me.userNameEdit.Location = New System.Drawing.Point(150, 23)
			Me.userNameEdit.Name = "userNameEdit"
			Me.userNameEdit.Size = New System.Drawing.Size(259, 20)
			Me.userNameEdit.StyleController = Me.layoutControl1
			Me.userNameEdit.TabIndex = 2
			' 
			' Root
			' 
			Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
			Me.Root.GroupBordersVisible = False
			Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() { Me.layoutControlItemUserName, Me.layoutControlItemPassword, Me.simpleLabelItem1, Me.simpleLabelItem2, Me.emptySpaceItem2, Me.layoutControlItemImage, Me.layoutControlGroupButtons})
			Me.Root.Name = "Root"
			Me.Root.OptionsItemText.TextToControlDistance = 4
			Me.Root.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4)
			Me.Root.Size = New System.Drawing.Size(415, 153)
			Me.Root.TextVisible = False
			' 
			' layoutControlItemUserName
			' 
			Me.layoutControlItemUserName.Control = Me.userNameEdit
			Me.layoutControlItemUserName.CustomizationFormText = "User Name"
			Me.layoutControlItemUserName.Location = New System.Drawing.Point(84, 17)
			Me.layoutControlItemUserName.Name = "layoutControlItemUserName"
			Me.layoutControlItemUserName.Size = New System.Drawing.Size(323, 24)
			Me.layoutControlItemUserName.Text = "User Name:"
			Me.layoutControlItemUserName.TextSize = New System.Drawing.Size(56, 13)
			' 
			' layoutControlItemPassword
			' 
			Me.layoutControlItemPassword.Control = Me.passwordEdit
			Me.layoutControlItemPassword.Location = New System.Drawing.Point(84, 41)
			Me.layoutControlItemPassword.Name = "layoutControlItemPassword"
			Me.layoutControlItemPassword.Size = New System.Drawing.Size(323, 24)
			Me.layoutControlItemPassword.Text = "Password:"
			Me.layoutControlItemPassword.TextSize = New System.Drawing.Size(56, 13)
			' 
			' simpleLabelItem1
			' 
			Me.simpleLabelItem1.AllowHotTrack = False
			Me.simpleLabelItem1.Location = New System.Drawing.Point(84, 65)
			Me.simpleLabelItem1.Name = "simpleLabelItem1"
			Me.simpleLabelItem1.Size = New System.Drawing.Size(323, 17)
			Me.simpleLabelItem1.Text = "This demo app does not require a password for login"
			Me.simpleLabelItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
			Me.simpleLabelItem1.TextSize = New System.Drawing.Size(251, 13)
			' 
			' simpleLabelItem2
			' 
			Me.simpleLabelItem2.AllowHotTrack = False
			Me.simpleLabelItem2.Location = New System.Drawing.Point(84, 0)
			Me.simpleLabelItem2.Name = "simpleLabelItem2"
			Me.simpleLabelItem2.Size = New System.Drawing.Size(323, 17)
			Me.simpleLabelItem2.Text = "Enter your user name (Admin or User) to proceed."
			Me.simpleLabelItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
			Me.simpleLabelItem2.TextSize = New System.Drawing.Size(241, 13)
			' 
			' emptySpaceItem2
			' 
			Me.emptySpaceItem2.AllowHotTrack = False
			Me.emptySpaceItem2.Location = New System.Drawing.Point(84, 82)
			Me.emptySpaceItem2.Name = "emptySpaceItem2"
			Me.emptySpaceItem2.Size = New System.Drawing.Size(323, 10)
			Me.emptySpaceItem2.TextSize = New System.Drawing.Size(0, 0)
			' 
			' layoutControlItemImage
			' 
			Me.layoutControlItemImage.Control = Me.pictureEditImage
			Me.layoutControlItemImage.Location = New System.Drawing.Point(0, 0)
			Me.layoutControlItemImage.Name = "layoutControlItemImage"
			Me.layoutControlItemImage.Size = New System.Drawing.Size(84, 92)
			Me.layoutControlItemImage.TextSize = New System.Drawing.Size(0, 0)
			Me.layoutControlItemImage.TextVisible = False
			' 
			' layoutControlGroupButtons
			' 
			Me.layoutControlGroupButtons.GroupBordersVisible = False
			Me.layoutControlGroupButtons.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() { Me.layoutControlItemLogin, Me.layoutControlItemCancel, Me.emptySpaceItem1, Me.emptySpaceItem3})
			Me.layoutControlGroupButtons.Location = New System.Drawing.Point(0, 92)
			Me.layoutControlGroupButtons.Name = "layoutControlGroupButtons"
			Me.layoutControlGroupButtons.OptionsItemText.TextToControlDistance = 0
			Me.layoutControlGroupButtons.Size = New System.Drawing.Size(407, 53)
			Me.layoutControlGroupButtons.TextVisible = False
			' 
			' layoutControlItemLogin
			' 
			Me.layoutControlItemLogin.Control = Me.buttonLogin
			Me.layoutControlItemLogin.Location = New System.Drawing.Point(259, 23)
			Me.layoutControlItemLogin.Name = "layoutControlItemLogin"
			Me.layoutControlItemLogin.Size = New System.Drawing.Size(74, 30)
			Me.layoutControlItemLogin.TextSize = New System.Drawing.Size(0, 0)
			Me.layoutControlItemLogin.TextVisible = False
			' 
			' layoutControlItemCancel
			' 
			Me.layoutControlItemCancel.Control = Me.cancelButton
			Me.layoutControlItemCancel.Location = New System.Drawing.Point(333, 23)
			Me.layoutControlItemCancel.Name = "layoutControlItemCancel"
			Me.layoutControlItemCancel.Size = New System.Drawing.Size(74, 30)
			Me.layoutControlItemCancel.TextSize = New System.Drawing.Size(0, 0)
			Me.layoutControlItemCancel.TextVisible = False
			' 
			' emptySpaceItem1
			' 
			Me.emptySpaceItem1.AllowHotTrack = False
			Me.emptySpaceItem1.Location = New System.Drawing.Point(0, 23)
			Me.emptySpaceItem1.Name = "emptySpaceItem1"
			Me.emptySpaceItem1.Size = New System.Drawing.Size(259, 30)
			Me.emptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
			' 
			' emptySpaceItem3
			' 
			Me.emptySpaceItem3.AllowHotTrack = False
			Me.emptySpaceItem3.Location = New System.Drawing.Point(0, 0)
			Me.emptySpaceItem3.Name = "emptySpaceItem3"
			Me.emptySpaceItem3.Size = New System.Drawing.Size(407, 23)
			Me.emptySpaceItem3.TextSize = New System.Drawing.Size(0, 0)
			' 
			' dxErrorProvider1
			' 
			Me.dxErrorProvider1.ContainerControl = Me
			' 
			' LoginForm
			' 
			Me.AcceptButton = Me.buttonLogin
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(415, 153)
			Me.Controls.Add(Me.layoutControl1)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.Icon = My.Resources.ExpressApp
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "LoginForm"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "Welcome to the XAF's Security in Non-XAF Apps Demo!"
			CType(Me.layoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.layoutControl1.ResumeLayout(False)
			CType(Me.pictureEditImage.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.passwordEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.userNameEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlItemUserName, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlItemPassword, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.simpleLabelItem1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.simpleLabelItem2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.emptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlItemImage, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlGroupButtons, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlItemLogin, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlItemCancel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.emptySpaceItem3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private WithEvents buttonLogin As DevExpress.XtraEditors.SimpleButton
		Private layoutControl1 As DevExpress.XtraLayout.LayoutControl
		Private passwordEdit As DevExpress.XtraEditors.TextEdit
		Private WithEvents userNameEdit As DevExpress.XtraEditors.TextEdit
		Private Root As DevExpress.XtraLayout.LayoutControlGroup
		Private layoutControlItemUserName As DevExpress.XtraLayout.LayoutControlItem
		Private layoutControlItemPassword As DevExpress.XtraLayout.LayoutControlItem
		Private simpleLabelItem1 As DevExpress.XtraLayout.SimpleLabelItem
		Private simpleLabelItem2 As DevExpress.XtraLayout.SimpleLabelItem
		Private layoutControlItemLogin As DevExpress.XtraLayout.LayoutControlItem
		Private emptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
		Private WithEvents cancelButton As DevExpress.XtraEditors.SimpleButton
		Private layoutControlItemCancel As DevExpress.XtraLayout.LayoutControlItem
		Private pictureEditImage As DevExpress.XtraEditors.PictureEdit
		Private layoutControlItemImage As DevExpress.XtraLayout.LayoutControlItem
		Private layoutControlGroupButtons As DevExpress.XtraLayout.LayoutControlGroup
		Private emptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
		Private emptySpaceItem3 As DevExpress.XtraLayout.EmptySpaceItem
		Private dxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
	End Class
End Namespace

