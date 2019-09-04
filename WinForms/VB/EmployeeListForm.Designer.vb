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
			Me.employee = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.department = New DevExpress.XtraGrid.Columns.GridColumn()
			Me.ribbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
			Me.newBarButtonItem = New DevExpress.XtraBars.BarButtonItem()
			Me.deleteBarButtonItem = New DevExpress.XtraBars.BarButtonItem()
			Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
			Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
			CType(Me.employeeGrid, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.employeeGridView, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' employeeGrid
			' 
			Me.employeeGrid.DataSource = Me.employeeBindingSource
			Me.employeeGrid.Location = New System.Drawing.Point(0, 141)
			Me.employeeGrid.MainView = Me.employeeGridView
			Me.employeeGrid.Name = "employeeGrid"
			Me.employeeGrid.Size = New System.Drawing.Size(893, 437)
			Me.employeeGrid.TabIndex = 2
			Me.employeeGrid.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.employeeGridView})
			Me.employeeGrid.Dock = System.Windows.Forms.DockStyle.Fill
			' 
			' EmployeeBindingSource
			' 
			Me.employeeBindingSource.DisplayableProperties = "FullName;Department.Title"
			Me.employeeBindingSource.ObjectType = GetType(XafSolution.Module.BusinessObjects.Employee)
			' 
			' employeeGridView
			' 
			Me.employeeGridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() { Me.employee, Me.department})
			Me.employeeGridView.CustomizationFormBounds = New System.Drawing.Rectangle(1031, 474, 260, 232)
			Me.employeeGridView.GridControl = Me.employeeGrid
			Me.employeeGridView.Name = "employeeGridView"
			Me.employeeGridView.OptionsDetail.EnableMasterViewMode = False
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.employeeGridView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.EmployeeGridView_RowClick);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.employeeGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.EmployeeGridView_FocusedRowObjectChanged);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.employeeGridView.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(GridView_CustomRowCellEdit);
			' 
			' Employee
			' 
			Me.employee.Caption = "Employee"
			Me.employee.FieldName = "FullName"
			Me.employee.Name = "Employee"
			Me.employee.OptionsColumn.AllowEdit = False
			Me.employee.Visible = True
			Me.employee.VisibleIndex = 0
			' 
			' Department
			' 
			Me.department.Caption = "Department"
			Me.department.FieldName = "Department.Title"
			Me.department.Name = "Department"
			Me.department.OptionsColumn.AllowEdit = False
			Me.department.Visible = True
			Me.department.VisibleIndex = 1
			' 
			' ribbonControl1
			' 
			Me.ribbonControl1.ExpandCollapseItem.Id = 0
			Me.ribbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() { Me.ribbonControl1.ExpandCollapseItem, Me.ribbonControl1.SearchEditItem, Me.newBarButtonItem, Me.deleteBarButtonItem})
			Me.ribbonControl1.Location = New System.Drawing.Point(0, 0)
			Me.ribbonControl1.MaxItemId = 4
			Me.ribbonControl1.Name = "ribbonControl1"
			Me.ribbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() { Me.ribbonPage1})
			Me.ribbonControl1.Size = New System.Drawing.Size(897, 143)
			Me.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
			' 
			' newBarButtonItem
			' 
			Me.newBarButtonItem.Caption = "New"
			Me.newBarButtonItem.Id = 1
			Me.newBarButtonItem.Name = "newBarButtonItem"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.newBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButtonItem_ItemClick);
			' 
			' deleteBarButtonItem
			' 
			Me.deleteBarButtonItem.Caption = "Delete"
			Me.deleteBarButtonItem.Id = 2
			Me.deleteBarButtonItem.Name = "deleteBarButtonItem"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.deleteBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButtonItem_ItemClick);
			' 
			' ribbonPage1
			' 
			Me.ribbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() { Me.ribbonPageGroup1})
			Me.ribbonPage1.Name = "ribbonPage1"
			Me.ribbonPage1.Text = "Home"
			' 
			' ribbonPageGroup1
			' 
			Me.ribbonPageGroup1.ItemLinks.Add(Me.newBarButtonItem)
			Me.ribbonPageGroup1.ItemLinks.Add(Me.deleteBarButtonItem)
			Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
			Me.ribbonPageGroup1.Text = "Edit"
			Me.ribbonPageGroup1.AllowTextClipping = False
			' 
			' EmployeeListForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(897, 590)
			Me.ControlBox = False
			Me.Controls.Add(Me.employeeGrid)
			Me.Controls.Add(Me.ribbonControl1)
			Me.Name = "EmployeeListForm"
			Me.Ribbon = Me.ribbonControl1
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "Employees"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.EmployeeListForm_Load);
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
		Private employee As DevExpress.XtraGrid.Columns.GridColumn
		Private department As DevExpress.XtraGrid.Columns.GridColumn
		Private employeeBindingSource As DevExpress.Xpo.XPBindingSource
		Private ribbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
		Private WithEvents newBarButtonItem As DevExpress.XtraBars.BarButtonItem
		Private WithEvents deleteBarButtonItem As DevExpress.XtraBars.BarButtonItem
		Private ribbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
		Private ribbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	End Class
End Namespace