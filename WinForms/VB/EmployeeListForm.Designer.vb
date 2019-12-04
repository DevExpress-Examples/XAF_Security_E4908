Imports System
Imports DevExpress.XtraEditors

Namespace WindowsFormsApplication
    Partial Public Class EmployeeListForm
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
            Me.employeeGrid = New DevExpress.XtraGrid.GridControl()
            Me.employeeBindingSource = New DevExpress.Xpo.XPBindingSource(Me.components)
            Me.employeeGridView = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.Employee = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.Department = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.ribbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
            Me.newBarButtonItem = New DevExpress.XtraBars.BarButtonItem()
            Me.deleteBarButtonItem = New DevExpress.XtraBars.BarButtonItem()
            Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
            Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            Me.editBarButtonItem = New DevExpress.XtraBars.BarButtonItem()
            CType(Me.employeeGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.employeeGridView, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' employeeGrid
            ' 
            Me.employeeGrid.DataSource = Me.employeeBindingSource
            Me.employeeGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.employeeGrid.Location = New System.Drawing.Point(0, 157)
            Me.employeeGrid.MainView = Me.employeeGridView
            Me.employeeGrid.Margin = New System.Windows.Forms.Padding(0)
            Me.employeeGrid.Name = "employeeGrid"
            Me.employeeGrid.Size = New System.Drawing.Size(685, 433)
            Me.employeeGrid.TabIndex = 2
            Me.employeeGrid.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.employeeGridView})
            ' 
            ' employeeBindingSource
            ' 
            Me.employeeBindingSource.DisplayableProperties = "FullName;Department.Title"
            Me.employeeBindingSource.ObjectType = GetType(XafSolution.Module.BusinessObjects.Employee)
            ' 
            ' employeeGridView
            ' 
            Me.employeeGridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.employeeGridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() { Me.Employee, Me.Department})
            Me.employeeGridView.CustomizationFormBounds = New System.Drawing.Rectangle(1031, 474, 260, 232)
            Me.employeeGridView.GridControl = Me.employeeGrid
            Me.employeeGridView.Name = "employeeGridView"
            Me.employeeGridView.OptionsDetail.EnableMasterViewMode = False
            Me.employeeGridView.OptionsView.ShowIndicator = False
            ' 
            ' Employee
            ' 
            Me.Employee.Caption = "Employee"
            Me.Employee.FieldName = "FullName"
            Me.Employee.Name = "Employee"
            Me.Employee.OptionsColumn.AllowEdit = False
            Me.Employee.OptionsColumn.FixedWidth = True
            Me.Employee.Visible = True
            Me.Employee.VisibleIndex = 0
            Me.Employee.Width = 421
            ' 
            ' Department
            ' 
            Me.Department.Caption = "Department"
            Me.Department.FieldName = "Department.Title"
            Me.Department.Name = "Department"
            Me.Department.OptionsColumn.AllowEdit = False
            Me.Department.OptionsColumn.FixedWidth = True
            Me.Department.Visible = True
            Me.Department.VisibleIndex = 1
            Me.Department.Width = 246
            ' 
            ' ribbonControl1
            ' 
            Me.ribbonControl1.ExpandCollapseItem.Id = 0
            Me.ribbonControl1.CommandLayout = DevExpress.XtraBars.Ribbon.CommandLayout.Simplified
            Me.ribbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() { Me.ribbonControl1.ExpandCollapseItem, Me.ribbonControl1.SearchEditItem, Me.newBarButtonItem, Me.deleteBarButtonItem, Me.editBarButtonItem})
            Me.ribbonControl1.Location = New System.Drawing.Point(0, 0)
            Me.ribbonControl1.MaxItemId = 5
            Me.ribbonControl1.Name = "ribbonControl1"
            Me.ribbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() { Me.ribbonPage1})
            Me.ribbonControl1.Size = New System.Drawing.Size(685, 157)
            Me.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
            Me.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide
            ' 
            ' newBarButtonItem
            ' 
            Me.newBarButtonItem.Caption = "New"
            Me.newBarButtonItem.Id = 1
            Me.newBarButtonItem.ImageOptions.SvgImage = My.Resources.[New]
            Me.newBarButtonItem.Name = "newBarButtonItem"
            ' 
            ' deleteBarButtonItem
            ' 
            Me.deleteBarButtonItem.Caption = "Delete"
            Me.deleteBarButtonItem.Id = 2
            Me.deleteBarButtonItem.ImageOptions.SvgImage = My.Resources.Action_Delete
            Me.deleteBarButtonItem.Name = "deleteBarButtonItem"
            ' 
            ' ribbonPage1
            ' 
            Me.ribbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() { Me.ribbonPageGroup1})
            Me.ribbonPage1.Name = "ribbonPage1"
            Me.ribbonPage1.Text = "Home"
            ' 
            ' ribbonPageGroup1
            ' 
            Me.ribbonPageGroup1.AllowTextClipping = False
            Me.ribbonPageGroup1.ItemLinks.Add(Me.newBarButtonItem, True)
            Me.ribbonPageGroup1.ItemLinks.Add(Me.editBarButtonItem)
            Me.ribbonPageGroup1.ItemLinks.Add(Me.deleteBarButtonItem)
            Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
            Me.ribbonPageGroup1.Text = "General"
            ' 
            ' editBarButtonItem
            ' 
            Me.editBarButtonItem.Caption = "Edit"
            Me.editBarButtonItem.Id = 4
            Me.editBarButtonItem.ImageOptions.SvgImage = My.Resources.edit
            Me.editBarButtonItem.Name = "editBarButtonItem"
            ' 
            ' EmployeeListForm
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(685, 590)
            Me.ControlBox = False
            Me.Controls.Add(Me.employeeGrid)
            Me.Controls.Add(Me.ribbonControl1)
            Me.Name = "EmployeeListForm"
            Me.Ribbon = Me.ribbonControl1
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            CType(Me.employeeGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.employeeGridView, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        #End Region
        Private employeeGrid As DevExpress.XtraGrid.GridControl
        Private WithEvents employeeGridView As DevExpress.XtraGrid.Views.Grid.GridView
        Private employeeBindingSource As DevExpress.Xpo.XPBindingSource
        Private ribbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
        Private WithEvents newBarButtonItem As DevExpress.XtraBars.BarButtonItem
        Private WithEvents deleteBarButtonItem As DevExpress.XtraBars.BarButtonItem
        Private ribbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
        Private ribbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
        Private Employee As DevExpress.XtraGrid.Columns.GridColumn
        Private Department As DevExpress.XtraGrid.Columns.GridColumn
        Private WithEvents editBarButtonItem As DevExpress.XtraBars.BarButtonItem
    End Class
End Namespace