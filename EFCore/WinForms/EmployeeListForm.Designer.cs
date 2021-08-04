namespace WindowsFormsApplication {
	partial class EmployeeListForm {
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
            this.employeeGrid = new DevExpress.XtraGrid.GridControl();
            this.employeeGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Employee = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Department = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.newBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.deleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.editBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.employeeGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // employeeGrid
            // 
            this.employeeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.employeeGrid.Location = new System.Drawing.Point(0, 157);
            this.employeeGrid.MainView = this.employeeGridView;
            this.employeeGrid.Margin = new System.Windows.Forms.Padding(0);
            this.employeeGrid.Name = "employeeGrid";
            this.employeeGrid.Size = new System.Drawing.Size(685, 433);
            this.employeeGrid.TabIndex = 2;
            this.employeeGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.employeeGridView});
			// 
			// employeeGridView
			// 
			this.employeeGridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.employeeGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Employee,
            this.Department});
            this.employeeGridView.CustomizationFormBounds = new System.Drawing.Rectangle(1031, 474, 260, 232);
            this.employeeGridView.GridControl = this.employeeGrid;
            this.employeeGridView.Name = "employeeGridView";
            this.employeeGridView.OptionsDetail.EnableMasterViewMode = false;
            this.employeeGridView.OptionsView.ShowIndicator = false;
            this.employeeGridView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.EmployeeGridView_RowClick);
            this.employeeGridView.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.GridView_CustomRowCellEdit);
            this.employeeGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.EmployeeGridView_FocusedRowObjectChanged);
            // 
            // Employee
            // 
            this.Employee.Caption = "Employee";
            this.Employee.FieldName = "FullName";
            this.Employee.Name = "Employee";
            this.Employee.OptionsColumn.AllowEdit = false;
            this.Employee.OptionsColumn.FixedWidth = true;
            this.Employee.Visible = true;
            this.Employee.VisibleIndex = 0;
            this.Employee.Width = 421;
            // 
            // Department
            // 
            this.Department.Caption = "Department";
            this.Department.FieldName = "Department.Title";
            this.Department.Name = "Department";
            this.Department.OptionsColumn.AllowEdit = false;
            this.Department.OptionsColumn.FixedWidth = true;
            this.Department.Visible = true;
            this.Department.VisibleIndex = 1;
            this.Department.Width = 246;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.CommandLayout = DevExpress.XtraBars.Ribbon.CommandLayout.Simplified;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.newBarButtonItem,
            this.deleteBarButtonItem,
            this.editBarButtonItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 5;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(685, 157);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
			this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
			// 
			// newBarButtonItem
			// 
			this.newBarButtonItem.Caption = "New";
            this.newBarButtonItem.Id = 1;
            this.newBarButtonItem.ImageOptions.SvgImage = global::WindowsFormsApplication.Properties.Resources.Action_New;
            this.newBarButtonItem.Name = "newBarButtonItem";
            this.newBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButtonItem_ItemClick);
            // 
            // deleteBarButtonItem
            // 
            this.deleteBarButtonItem.Caption = "Delete";
            this.deleteBarButtonItem.Id = 2;
            this.deleteBarButtonItem.ImageOptions.SvgImage = global::WindowsFormsApplication.Properties.Resources.Action_Delete;
            this.deleteBarButtonItem.Name = "deleteBarButtonItem";
            this.deleteBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButtonItem_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.ItemLinks.Add(this.newBarButtonItem, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.editBarButtonItem);
            this.ribbonPageGroup1.ItemLinks.Add(this.deleteBarButtonItem);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "General";
            // 
            // editBarButtonItem
            // 
            this.editBarButtonItem.Caption = "Edit";
            this.editBarButtonItem.Id = 4;
            this.editBarButtonItem.ImageOptions.SvgImage = global::WindowsFormsApplication.Properties.Resources.Action_Edit;
            this.editBarButtonItem.Name = "editBarButtonItem";
            this.editBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButtonItem_ItemClick);
            // 
            // EmployeeListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 590);
            this.ControlBox = false;
            this.Controls.Add(this.employeeGrid);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "EmployeeListForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.EmployeeListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.employeeGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
		#endregion
		private DevExpress.XtraGrid.GridControl employeeGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView employeeGridView;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem newBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem deleteBarButtonItem;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraGrid.Columns.GridColumn Employee;
        private DevExpress.XtraGrid.Columns.GridColumn Department;
        private DevExpress.XtraBars.BarButtonItem editBarButtonItem;
    }
}