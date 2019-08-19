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
            Me.login_button = New System.Windows.Forms.Button()
            Me.layoutControl1 = New DevExpress.XtraLayout.LayoutControl()
            Me.passwordEdit = New DevExpress.XtraEditors.TextEdit()
            Me.userNameEdit = New DevExpress.XtraEditors.TextEdit()
            Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.emptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.layoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.labelControl1 = New DevExpress.XtraEditors.LabelControl()
            Me.labelControl2 = New DevExpress.XtraEditors.LabelControl()
            Me.labelControl3 = New DevExpress.XtraEditors.LabelControl()
            CType(Me.layoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.layoutControl1.SuspendLayout()
            CType(Me.passwordEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.userNameEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' login_button
            ' 
            Me.login_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(0)))
            Me.login_button.Location = New System.Drawing.Point(138, 164)
            Me.login_button.Name = "login_button"
            Me.login_button.Size = New System.Drawing.Size(76, 24)
            Me.login_button.TabIndex = 0
            Me.login_button.Text = "Login"
            Me.login_button.UseVisualStyleBackColor = True
            ' 
            ' layoutControl1
            ' 
            Me.layoutControl1.Controls.Add(Me.passwordEdit)
            Me.layoutControl1.Controls.Add(Me.userNameEdit)
            Me.layoutControl1.Location = New System.Drawing.Point(12, 44)
            Me.layoutControl1.Name = "layoutControl1"
            Me.layoutControl1.Root = Me.Root
            Me.layoutControl1.Size = New System.Drawing.Size(336, 84)
            Me.layoutControl1.TabIndex = 5
            Me.layoutControl1.Text = "layoutControl1"
            ' 
            ' passwordEdit
            ' 
            Me.passwordEdit.Location = New System.Drawing.Point(66, 36)
            Me.passwordEdit.Name = "passwordEdit"
            Me.passwordEdit.Properties.PasswordChar = "*"c
            Me.passwordEdit.Size = New System.Drawing.Size(258, 20)
            Me.passwordEdit.StyleController = Me.layoutControl1
            Me.passwordEdit.TabIndex = 5
            ' 
            ' userNameEdit
            ' 
            Me.userNameEdit.Location = New System.Drawing.Point(66, 12)
            Me.userNameEdit.Name = "userNameEdit"
            Me.userNameEdit.Size = New System.Drawing.Size(258, 20)
            Me.userNameEdit.StyleController = Me.layoutControl1
            Me.userNameEdit.TabIndex = 4
            ' 
            ' Root
            ' 
            Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            Me.Root.GroupBordersVisible = False
            Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() { Me.layoutControlItem1, Me.emptySpaceItem1, Me.layoutControlItem2})
            Me.Root.Name = "Root"
            Me.Root.Size = New System.Drawing.Size(336, 84)
            Me.Root.TextVisible = False
            ' 
            ' layoutControlItem1
            ' 
            Me.layoutControlItem1.Control = Me.userNameEdit
            Me.layoutControlItem1.CustomizationFormText = "User name"
            Me.layoutControlItem1.Location = New System.Drawing.Point(0, 0)
            Me.layoutControlItem1.Name = "layoutControlItem1"
            Me.layoutControlItem1.Size = New System.Drawing.Size(316, 24)
            Me.layoutControlItem1.Text = "User name"
            Me.layoutControlItem1.TextSize = New System.Drawing.Size(51, 13)
            ' 
            ' emptySpaceItem1
            ' 
            Me.emptySpaceItem1.AllowHotTrack = False
            Me.emptySpaceItem1.Location = New System.Drawing.Point(0, 48)
            Me.emptySpaceItem1.Name = "emptySpaceItem1"
            Me.emptySpaceItem1.Size = New System.Drawing.Size(316, 16)
            Me.emptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
            ' 
            ' layoutControlItem2
            ' 
            Me.layoutControlItem2.Control = Me.passwordEdit
            Me.layoutControlItem2.Location = New System.Drawing.Point(0, 24)
            Me.layoutControlItem2.Name = "layoutControlItem2"
            Me.layoutControlItem2.Size = New System.Drawing.Size(316, 24)
            Me.layoutControlItem2.Text = "Password"
            Me.layoutControlItem2.TextSize = New System.Drawing.Size(51, 13)
            ' 
            ' labelControl1
            ' 
            Me.labelControl1.Location = New System.Drawing.Point(24, 15)
            Me.labelControl1.Name = "labelControl1"
            Me.labelControl1.Size = New System.Drawing.Size(262, 13)
            Me.labelControl1.TabIndex = 6
            Me.labelControl1.Text = "Welcome to the XAF's Security in Non-XAF apps Demo!"
            ' 
            ' labelControl2
            ' 
            Me.labelControl2.Location = New System.Drawing.Point(24, 34)
            Me.labelControl2.Name = "labelControl2"
            Me.labelControl2.Size = New System.Drawing.Size(241, 13)
            Me.labelControl2.TabIndex = 7
            Me.labelControl2.Text = "Enter your user name (Admin or User) to proceed."
            ' 
            ' labelControl3
            ' 
            Me.labelControl3.Location = New System.Drawing.Point(24, 119)
            Me.labelControl3.Name = "labelControl3"
            Me.labelControl3.Size = New System.Drawing.Size(251, 13)
            Me.labelControl3.TabIndex = 8
            Me.labelControl3.Text = "This demo app does not require a password for login"
            ' 
            ' LoginForm
            ' 
            Me.AcceptButton = Me.login_button
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(358, 197)
            Me.Controls.Add(Me.labelControl3)
            Me.Controls.Add(Me.labelControl2)
            Me.Controls.Add(Me.labelControl1)
            Me.Controls.Add(Me.layoutControl1)
            Me.Controls.Add(Me.login_button)
            Me.Name = "LoginForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Authentication"
            CType(Me.layoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.layoutControl1.ResumeLayout(False)
            CType(Me.passwordEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.userNameEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        #End Region

        Private WithEvents login_button As System.Windows.Forms.Button
        Private layoutControl1 As DevExpress.XtraLayout.LayoutControl
        Private passwordEdit As DevExpress.XtraEditors.TextEdit
        Private userNameEdit As DevExpress.XtraEditors.TextEdit
        Private Root As DevExpress.XtraLayout.LayoutControlGroup
        Private layoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
        Private emptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
        Private layoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
        Private labelControl1 As DevExpress.XtraEditors.LabelControl
        Private labelControl2 As DevExpress.XtraEditors.LabelControl
        Private labelControl3 As DevExpress.XtraEditors.LabelControl
    End Class
End Namespace

