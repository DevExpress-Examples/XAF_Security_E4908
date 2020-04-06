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
            Me.buttonLogin = New DevExpress.XtraEditors.SimpleButton()
            Me.layoutControl1 = New DevExpress.XtraLayout.LayoutControl()
            Me.buttonCancel = New DevExpress.XtraEditors.SimpleButton()
            Me.passwordEdit = New DevExpress.XtraEditors.TextEdit()
            Me.userNameEdit = New DevExpress.XtraEditors.TextEdit()
            Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutControlGroupButtons = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutControlItemLogin = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutControlItemCancel = New DevExpress.XtraLayout.LayoutControlItem()
            Me.emptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.emptySpaceItem7 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.emptySpaceItem8 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.layoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.simpleLabelItem2 = New DevExpress.XtraLayout.SimpleLabelItem()
            Me.layoutControlItemUserName = New DevExpress.XtraLayout.LayoutControlItem()
            Me.emptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.layoutControlItemPassword = New DevExpress.XtraLayout.LayoutControlItem()
            Me.simpleLabelItem1 = New DevExpress.XtraLayout.SimpleLabelItem()
            Me.simpleLabelItem3 = New DevExpress.XtraLayout.SimpleLabelItem()
            Me.emptySpaceItem5 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.emptySpaceItem6 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.emptySpaceItem9 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.emptySpaceItem10 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.emptySpaceItem4 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.dxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
            CType(Me.layoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.layoutControl1.SuspendLayout()
            CType(Me.passwordEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.userNameEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlGroupButtons, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlItemLogin, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlItemCancel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem8, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.simpleLabelItem2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlItemUserName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlItemPassword, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.simpleLabelItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.simpleLabelItem3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem9, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' buttonLogin
            ' 
            Me.buttonLogin.Location = New System.Drawing.Point(316, 211)
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
            Me.layoutControl1.Controls.Add(Me.buttonCancel)
            Me.layoutControl1.Controls.Add(Me.buttonLogin)
            Me.layoutControl1.Controls.Add(Me.passwordEdit)
            Me.layoutControl1.Controls.Add(Me.userNameEdit)
            Me.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.layoutControl1.Location = New System.Drawing.Point(0, 0)
            Me.layoutControl1.Name = "layoutControl1"
            Me.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(624, 143, 742, 800)
            Me.layoutControl1.OptionsFocus.EnableAutoTabOrder = False
            Me.layoutControl1.Root = Me.Root
            Me.layoutControl1.Size = New System.Drawing.Size(478, 255)
            Me.layoutControl1.TabIndex = 5
            Me.layoutControl1.Text = "layoutControl1"
            ' 
            ' buttonCancel
            ' 
            Me.buttonCancel.Location = New System.Drawing.Point(390, 211)
            Me.buttonCancel.Margin = New System.Windows.Forms.Padding(2)
            Me.buttonCancel.MaximumSize = New System.Drawing.Size(70, 26)
            Me.buttonCancel.MinimumSize = New System.Drawing.Size(70, 26)
            Me.buttonCancel.Name = "buttonCancel"
            Me.buttonCancel.Size = New System.Drawing.Size(70, 26)
            Me.buttonCancel.StyleController = Me.layoutControl1
            Me.buttonCancel.TabIndex = 1
            Me.buttonCancel.Text = "Cancel"
            ' 
            ' passwordEdit
            ' 
            Me.passwordEdit.Location = New System.Drawing.Point(83, 113)
            Me.passwordEdit.Name = "passwordEdit"
            Me.passwordEdit.Properties.NullValuePrompt = "Password"
            Me.passwordEdit.Properties.PasswordChar = "*"c
            Me.passwordEdit.Size = New System.Drawing.Size(310, 20)
            Me.passwordEdit.StyleController = Me.layoutControl1
            Me.passwordEdit.TabIndex = 3
            ' 
            ' userNameEdit
            ' 
            Me.dxErrorProvider1.SetIconAlignment(Me.userNameEdit, System.Windows.Forms.ErrorIconAlignment.MiddleRight)
            Me.userNameEdit.Location = New System.Drawing.Point(83, 89)
            Me.userNameEdit.Name = "userNameEdit"
            Me.userNameEdit.Size = New System.Drawing.Size(310, 20)
            Me.userNameEdit.StyleController = Me.layoutControl1
            Me.userNameEdit.TabIndex = 2
            ' 
            ' Root
            ' 
            Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            Me.Root.GroupBordersVisible = False
            Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() { Me.layoutControlGroupButtons, Me.layoutControlGroup1, Me.emptySpaceItem4})
            Me.Root.Name = "Root"
            Me.Root.OptionsItemText.TextToControlDistance = 4
            Me.Root.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4)
            Me.Root.Size = New System.Drawing.Size(478, 255)
            Me.Root.TextVisible = False
            ' 
            ' layoutControlGroupButtons
            ' 
            Me.layoutControlGroupButtons.GroupBordersVisible = False
            Me.layoutControlGroupButtons.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() { Me.layoutControlItemLogin, Me.layoutControlItemCancel, Me.emptySpaceItem1, Me.emptySpaceItem7, Me.emptySpaceItem8})
            Me.layoutControlGroupButtons.Location = New System.Drawing.Point(0, 205)
            Me.layoutControlGroupButtons.Name = "layoutControlGroupButtons"
            Me.layoutControlGroupButtons.OptionsItemText.TextToControlDistance = 0
            Me.layoutControlGroupButtons.Size = New System.Drawing.Size(470, 42)
            Me.layoutControlGroupButtons.TextVisible = False
            ' 
            ' layoutControlItemLogin
            ' 
            Me.layoutControlItemLogin.Control = Me.buttonLogin
            Me.layoutControlItemLogin.Location = New System.Drawing.Point(310, 0)
            Me.layoutControlItemLogin.Name = "layoutControlItemLogin"
            Me.layoutControlItemLogin.Size = New System.Drawing.Size(74, 30)
            Me.layoutControlItemLogin.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutControlItemLogin.TextVisible = False
            ' 
            ' layoutControlItemCancel
            ' 
            Me.layoutControlItemCancel.Control = Me.buttonCancel
            Me.layoutControlItemCancel.Location = New System.Drawing.Point(384, 0)
            Me.layoutControlItemCancel.Name = "layoutControlItemCancel"
            Me.layoutControlItemCancel.Size = New System.Drawing.Size(74, 30)
            Me.layoutControlItemCancel.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutControlItemCancel.TextVisible = False
            ' 
            ' emptySpaceItem1
            ' 
            Me.emptySpaceItem1.AllowHotTrack = False
            Me.emptySpaceItem1.Location = New System.Drawing.Point(0, 0)
            Me.emptySpaceItem1.Name = "emptySpaceItem1"
            Me.emptySpaceItem1.Size = New System.Drawing.Size(310, 30)
            Me.emptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' emptySpaceItem7
            ' 
            Me.emptySpaceItem7.AllowHotTrack = False
            Me.emptySpaceItem7.Location = New System.Drawing.Point(458, 0)
            Me.emptySpaceItem7.Name = "emptySpaceItem7"
            Me.emptySpaceItem7.Size = New System.Drawing.Size(12, 30)
            Me.emptySpaceItem7.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' emptySpaceItem8
            ' 
            Me.emptySpaceItem8.AllowHotTrack = False
            Me.emptySpaceItem8.Location = New System.Drawing.Point(0, 30)
            Me.emptySpaceItem8.Name = "emptySpaceItem8"
            Me.emptySpaceItem8.Size = New System.Drawing.Size(470, 12)
            Me.emptySpaceItem8.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' layoutControlGroup1
            ' 
            Me.layoutControlGroup1.GroupBordersVisible = False
            Me.layoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() { Me.simpleLabelItem2, Me.layoutControlItemUserName, Me.emptySpaceItem2, Me.layoutControlItemPassword, Me.simpleLabelItem1, Me.simpleLabelItem3, Me.emptySpaceItem5, Me.emptySpaceItem6, Me.emptySpaceItem9, Me.emptySpaceItem10})
            Me.layoutControlGroup1.Location = New System.Drawing.Point(0, 44)
            Me.layoutControlGroup1.Name = "layoutControlGroup1"
            Me.layoutControlGroup1.Size = New System.Drawing.Size(470, 161)
            Me.layoutControlGroup1.TextVisible = False
            ' 
            ' simpleLabelItem2
            ' 
            Me.simpleLabelItem2.AllowHotTrack = False
            Me.simpleLabelItem2.AllowHtmlStringInCaption = True
            Me.simpleLabelItem2.AppearanceItemCaption.Font = New System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(0)))
            Me.simpleLabelItem2.AppearanceItemCaption.Options.UseFont = True
            Me.simpleLabelItem2.Location = New System.Drawing.Point(77, 0)
            Me.simpleLabelItem2.Name = "simpleLabelItem2"
            Me.simpleLabelItem2.Size = New System.Drawing.Size(314, 17)
            Me.simpleLabelItem2.Text = "Enter your user name (<b>Admin</b> or <b>User</b>) to proceed."
            Me.simpleLabelItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
            Me.simpleLabelItem2.TextSize = New System.Drawing.Size(252, 13)
            ' 
            ' layoutControlItemUserName
            ' 
            Me.layoutControlItemUserName.Control = Me.userNameEdit
            Me.layoutControlItemUserName.CustomizationFormText = "User Name"
            Me.layoutControlItemUserName.Location = New System.Drawing.Point(77, 39)
            Me.layoutControlItemUserName.Name = "layoutControlItemUserName"
            Me.layoutControlItemUserName.Size = New System.Drawing.Size(314, 24)
            Me.layoutControlItemUserName.Text = "User Name:"
            Me.layoutControlItemUserName.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutControlItemUserName.TextVisible = False
            ' 
            ' emptySpaceItem2
            ' 
            Me.emptySpaceItem2.AllowHotTrack = False
            Me.emptySpaceItem2.Location = New System.Drawing.Point(77, 133)
            Me.emptySpaceItem2.Name = "emptySpaceItem2"
            Me.emptySpaceItem2.Size = New System.Drawing.Size(314, 28)
            Me.emptySpaceItem2.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' layoutControlItemPassword
            ' 
            Me.layoutControlItemPassword.Control = Me.passwordEdit
            Me.layoutControlItemPassword.Location = New System.Drawing.Point(77, 63)
            Me.layoutControlItemPassword.Name = "layoutControlItemPassword"
            Me.layoutControlItemPassword.Size = New System.Drawing.Size(314, 24)
            Me.layoutControlItemPassword.Text = "Password:"
            Me.layoutControlItemPassword.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutControlItemPassword.TextVisible = False
            ' 
            ' simpleLabelItem1
            ' 
            Me.simpleLabelItem1.AllowHotTrack = False
            Me.simpleLabelItem1.Enabled = False
            Me.simpleLabelItem1.Location = New System.Drawing.Point(77, 99)
            Me.simpleLabelItem1.Name = "simpleLabelItem1"
            Me.simpleLabelItem1.Size = New System.Drawing.Size(314, 17)
            Me.simpleLabelItem1.Text = "This demo app does not require"
            Me.simpleLabelItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
            Me.simpleLabelItem1.TextSize = New System.Drawing.Size(151, 13)
            ' 
            ' simpleLabelItem3
            ' 
            Me.simpleLabelItem3.AllowHotTrack = False
            Me.simpleLabelItem3.Enabled = False
            Me.simpleLabelItem3.Location = New System.Drawing.Point(77, 116)
            Me.simpleLabelItem3.Name = "simpleLabelItem3"
            Me.simpleLabelItem3.Size = New System.Drawing.Size(314, 17)
            Me.simpleLabelItem3.Text = "a password for login"
            Me.simpleLabelItem3.TextSize = New System.Drawing.Size(97, 13)
            ' 
            ' emptySpaceItem5
            ' 
            Me.emptySpaceItem5.AllowHotTrack = False
            Me.emptySpaceItem5.Location = New System.Drawing.Point(391, 0)
            Me.emptySpaceItem5.Name = "emptySpaceItem5"
            Me.emptySpaceItem5.Size = New System.Drawing.Size(79, 161)
            Me.emptySpaceItem5.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' emptySpaceItem6
            ' 
            Me.emptySpaceItem6.AllowHotTrack = False
            Me.emptySpaceItem6.Location = New System.Drawing.Point(0, 0)
            Me.emptySpaceItem6.Name = "emptySpaceItem6"
            Me.emptySpaceItem6.Size = New System.Drawing.Size(77, 161)
            Me.emptySpaceItem6.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' emptySpaceItem9
            ' 
            Me.emptySpaceItem9.AllowHotTrack = False
            Me.emptySpaceItem9.Location = New System.Drawing.Point(77, 17)
            Me.emptySpaceItem9.Name = "emptySpaceItem9"
            Me.emptySpaceItem9.Size = New System.Drawing.Size(314, 22)
            Me.emptySpaceItem9.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' emptySpaceItem10
            ' 
            Me.emptySpaceItem10.AllowHotTrack = False
            Me.emptySpaceItem10.Location = New System.Drawing.Point(77, 87)
            Me.emptySpaceItem10.Name = "emptySpaceItem10"
            Me.emptySpaceItem10.Size = New System.Drawing.Size(314, 12)
            Me.emptySpaceItem10.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' emptySpaceItem4
            ' 
            Me.emptySpaceItem4.AllowHotTrack = False
            Me.emptySpaceItem4.Location = New System.Drawing.Point(0, 0)
            Me.emptySpaceItem4.Name = "emptySpaceItem4"
            Me.emptySpaceItem4.Size = New System.Drawing.Size(470, 44)
            Me.emptySpaceItem4.TextSize = New System.Drawing.Size(0, 0)
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
            Me.ClientSize = New System.Drawing.Size(478, 255)
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
            CType(Me.passwordEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.userNameEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlGroupButtons, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlItemLogin, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlItemCancel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem8, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.simpleLabelItem2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlItemUserName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlItemPassword, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.simpleLabelItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.simpleLabelItem3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem9, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem4, System.ComponentModel.ISupportInitialize).EndInit()
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
        Private WithEvents buttonCancel As DevExpress.XtraEditors.SimpleButton
        Private layoutControlItemCancel As DevExpress.XtraLayout.LayoutControlItem
        Private layoutControlGroupButtons As DevExpress.XtraLayout.LayoutControlGroup
        Private emptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
        Private dxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
        Private emptySpaceItem4 As DevExpress.XtraLayout.EmptySpaceItem
        Private simpleLabelItem3 As DevExpress.XtraLayout.SimpleLabelItem
        Private layoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
        Private emptySpaceItem5 As DevExpress.XtraLayout.EmptySpaceItem
        Private emptySpaceItem6 As DevExpress.XtraLayout.EmptySpaceItem
        Private emptySpaceItem7 As DevExpress.XtraLayout.EmptySpaceItem
        Private emptySpaceItem8 As DevExpress.XtraLayout.EmptySpaceItem
        Private emptySpaceItem9 As DevExpress.XtraLayout.EmptySpaceItem
        Private emptySpaceItem10 As DevExpress.XtraLayout.EmptySpaceItem
    End Class
End Namespace

