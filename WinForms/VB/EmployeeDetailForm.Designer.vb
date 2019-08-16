Namespace WindowsFormsApplication
    Partial Public Class EmployeeDetailForm
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
            Me.ribbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
            Me.saveBarButtonItem = New DevExpress.XtraBars.BarButtonItem()
            Me.closeBarButtonItem = New DevExpress.XtraBars.BarButtonItem()
            Me.newButtonItem = New DevExpress.XtraBars.BarButtonItem()
            Me.deleteButtonItem = New DevExpress.XtraBars.BarButtonItem()
            Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
            Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            Me.employeeBindingSource = New DevExpress.Xpo.XPBindingSource(Me.components)
            Me.dataLayoutControl1 = New DevExpress.XtraDataLayout.DataLayoutControl()
            Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
            CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dataLayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' ribbonControl1
            ' 
            Me.ribbonControl1.ExpandCollapseItem.Id = 0
            Me.ribbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() { Me.ribbonControl1.ExpandCollapseItem, Me.ribbonControl1.SearchEditItem, Me.saveBarButtonItem, Me.closeBarButtonItem, Me.newButtonItem, Me.deleteButtonItem})
            Me.ribbonControl1.Location = New System.Drawing.Point(0, 0)
            Me.ribbonControl1.MaxItemId = 5
            Me.ribbonControl1.Name = "ribbonControl1"
            Me.ribbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() { Me.ribbonPage1})
            Me.ribbonControl1.Size = New System.Drawing.Size(781, 143)
            Me.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
            ' 
            ' saveBarButtonItem
            ' 
            Me.saveBarButtonItem.Caption = "Save"
            Me.saveBarButtonItem.Id = 1
            Me.saveBarButtonItem.Name = "saveBarButtonItem"
            ' 
            ' closeBarButtonItem
            ' 
            Me.closeBarButtonItem.Caption = "Close"
            Me.closeBarButtonItem.Id = 2
            Me.closeBarButtonItem.Name = "closeBarButtonItem"
            ' 
            ' newButtonItem
            ' 
            Me.newButtonItem.Caption = "New"
            Me.newButtonItem.Enabled = False
            Me.newButtonItem.Id = 3
            Me.newButtonItem.Name = "newButtonItem"
            ' 
            ' deleteButtonItem
            ' 
            Me.deleteButtonItem.Caption = "Delete"
            Me.deleteButtonItem.Enabled = False
            Me.deleteButtonItem.Id = 4
            Me.deleteButtonItem.Name = "deleteButtonItem"
            ' 
            ' ribbonPage1
            ' 
            Me.ribbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() { Me.ribbonPageGroup1})
            Me.ribbonPage1.Name = "ribbonPage1"
            Me.ribbonPage1.Text = "Home"
            ' 
            ' ribbonPageGroup1
            ' 
            Me.ribbonPageGroup1.ItemLinks.Add(Me.saveBarButtonItem)
            Me.ribbonPageGroup1.ItemLinks.Add(Me.closeBarButtonItem)
            Me.ribbonPageGroup1.ItemLinks.Add(Me.deleteButtonItem)
            Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
            Me.ribbonPageGroup1.Text = "Edit"
            ' 
            ' EmployeeBindingSource
            ' 
            Me.employeeBindingSource.DisplayableProperties = "FirstName;LastName;Department;Department.Title"
            ' 
            ' dataLayoutControl1
            ' 
            Me.dataLayoutControl1.DataSource = Me.employeeBindingSource
            Me.dataLayoutControl1.Location = New System.Drawing.Point(12, 147)
            Me.dataLayoutControl1.Name = "dataLayoutControl1"
            Me.dataLayoutControl1.Root = Me.Root
            Me.dataLayoutControl1.Size = New System.Drawing.Size(344, 161)
            Me.dataLayoutControl1.TabIndex = 7
            Me.dataLayoutControl1.Text = "dataLayoutControl1"
            Me.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
            ' 
            ' Root
            ' 
            Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            Me.Root.GroupBordersVisible = False
            Me.Root.Name = "Root"
            Me.Root.Size = New System.Drawing.Size(344, 161)
            Me.Root.TextVisible = False
            ' 
            ' EmployeeDetailForm
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(781, 544)
            Me.Controls.Add(Me.dataLayoutControl1)
            Me.Controls.Add(Me.ribbonControl1)
            Me.Name = "EmployeeDetailForm"
            Me.Ribbon = Me.ribbonControl1
            Me.Text = "EmployeeDetailForm"
            CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dataLayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        #End Region
        Private ribbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
        Private WithEvents saveBarButtonItem As DevExpress.XtraBars.BarButtonItem
        Private WithEvents closeBarButtonItem As DevExpress.XtraBars.BarButtonItem
        Private ribbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
        Private ribbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
        Private employeeBindingSource As DevExpress.Xpo.XPBindingSource
        Private dataLayoutControl1 As DevExpress.XtraDataLayout.DataLayoutControl
        Private Root As DevExpress.XtraLayout.LayoutControlGroup
        Private newButtonItem As DevExpress.XtraBars.BarButtonItem
        Private WithEvents deleteButtonItem As DevExpress.XtraBars.BarButtonItem
    End Class
End Namespace