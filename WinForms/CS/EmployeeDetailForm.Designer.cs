namespace WindowsFormsApplication {
	partial class EmployeeDetailForm {
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
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.saveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.closeBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.deleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.employeeBindingSource = new DevExpress.Xpo.XPBindingSource(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.saveBarButtonItem,
            this.closeBarButtonItem,
            this.deleteBarButtonItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 5;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(781, 143);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // saveBarButtonItem
            // 
            this.saveBarButtonItem.Caption = "Save";
            this.saveBarButtonItem.Id = 1;
            this.saveBarButtonItem.Name = "saveBarButtonItem";
            this.saveBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveBarButtonItem_ItemClick);
            // 
            // closeBarButtonItem
            // 
            this.closeBarButtonItem.Caption = "Close";
            this.closeBarButtonItem.Id = 2;
            this.closeBarButtonItem.Name = "closeBarButtonItem";
            this.closeBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CloseBarButtonItem_ItemClick);
            // 
            // deleteBarButtonItem
            // 
            this.deleteBarButtonItem.Caption = "Delete";
            this.deleteBarButtonItem.Enabled = false;
            this.deleteBarButtonItem.Id = 4;
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
            this.ribbonPageGroup1.ItemLinks.Add(this.saveBarButtonItem);
            this.ribbonPageGroup1.ItemLinks.Add(this.closeBarButtonItem);
            this.ribbonPageGroup1.ItemLinks.Add(this.deleteBarButtonItem);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Edit";
            // 
            // employeeBindingSource
            // 
            this.employeeBindingSource.DisplayableProperties = "FirstName;LastName;Department;Department.Title";
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.DataSource = this.employeeBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 143);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(781, 401);
            this.dataLayoutControl1.TabIndex = 7;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(781, 401);
            this.Root.TextVisible = false;
            // 
            // EmployeeDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 544);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "EmployeeDetailForm";
            this.Ribbon = this.ribbonControl1;
            this.Text = "Employee Details";
            this.Load += new System.EventHandler(this.EmployeeDetailForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem saveBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem closeBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem deleteBarButtonItem;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.Xpo.XPBindingSource employeeBindingSource;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
    }
}