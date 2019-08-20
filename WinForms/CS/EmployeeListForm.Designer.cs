using System;
using DevExpress.XtraEditors;

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
            this.employeeBindingSource = new DevExpress.Xpo.XPBindingSource(this.components);
            this.employeeGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.employee = new DevExpress.XtraGrid.Columns.GridColumn();
            this.department = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.newBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.deleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.employeeGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // employeeGrid
            // 
            this.employeeGrid.DataSource = this.employeeBindingSource;
            this.employeeGrid.Location = new System.Drawing.Point(0, 141);
            this.employeeGrid.MainView = this.employeeGridView;
            this.employeeGrid.Name = "employeeGrid";
            this.employeeGrid.Size = new System.Drawing.Size(893, 437);
            this.employeeGrid.TabIndex = 2;
            this.employeeGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.employeeGridView});
			this.employeeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// EmployeeBindingSource
			// 
			this.employeeBindingSource.DisplayableProperties = "FullName;Department.Title";
            this.employeeBindingSource.ObjectType = typeof(XafSolution.Module.BusinessObjects.Employee);
            // 
            // employeeGridView
            // 
            this.employeeGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.employee,
            this.department});
            this.employeeGridView.CustomizationFormBounds = new System.Drawing.Rectangle(1031, 474, 260, 232);
            this.employeeGridView.GridControl = this.employeeGrid;
            this.employeeGridView.Name = "employeeGridView";
            this.employeeGridView.OptionsDetail.EnableMasterViewMode = false;
            this.employeeGridView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.EmployeeGridView_RowClick);
            this.employeeGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.EmployeeGridView_FocusedRowObjectChanged);
			this.employeeGridView.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(GridView_CustomRowCellEdit);
            // 
            // Employee
            // 
            this.employee.Caption = "Employee";
            this.employee.FieldName = "FullName";
            this.employee.Name = "Employee";
            this.employee.OptionsColumn.AllowEdit = false;
            this.employee.Visible = true;
            this.employee.VisibleIndex = 0;
            // 
            // Department
            // 
            this.department.Caption = "Department";
            this.department.FieldName = "Department.Title";
            this.department.Name = "Department";
            this.department.OptionsColumn.AllowEdit = false;
            this.department.Visible = true;
            this.department.VisibleIndex = 1;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
			this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
			this.ribbonControl1.ExpandCollapseItem,
			this.ribbonControl1.SearchEditItem,
			this.newBarButtonItem,
			this.deleteBarButtonItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(897, 143);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // newBarButtonItem
            // 
            this.newBarButtonItem.Caption = "New";
            this.newBarButtonItem.Id = 1;
            this.newBarButtonItem.Name = "newBarButtonItem";
            this.newBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButtonItem_ItemClick);
            // 
            // deleteBarButtonItem
            // 
            this.deleteBarButtonItem.Caption = "Delete";
            this.deleteBarButtonItem.Id = 2;
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
            this.ribbonPageGroup1.ItemLinks.Add(this.newBarButtonItem);
            this.ribbonPageGroup1.ItemLinks.Add(this.deleteBarButtonItem);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Edit";
			this.ribbonPageGroup1.AllowTextClipping = false;
			// 
			// EmployeeListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 590);
            this.ControlBox = false;
            this.Controls.Add(this.employeeGrid);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "EmployeeListForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employees";
			this.Load += new System.EventHandler(this.EmployeeListForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.employeeGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
		#endregion
		private DevExpress.XtraGrid.GridControl employeeGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView employeeGridView;
		private DevExpress.XtraGrid.Columns.GridColumn employee;
		private DevExpress.XtraGrid.Columns.GridColumn department;
		private DevExpress.Xpo.XPBindingSource employeeBindingSource;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem newBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem deleteBarButtonItem;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
    }
}