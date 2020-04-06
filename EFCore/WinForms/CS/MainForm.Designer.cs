namespace WindowsFormsApplication {
	partial class MainForm {
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
			this.logoutButtonItem = new DevExpress.XtraBars.BarButtonItem();
			this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
			this.nativeMdiView1 = new DevExpress.XtraBars.Docking2010.Views.NativeMdi.NativeMdiView(this.components);
			this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nativeMdiView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
			this.SuspendLayout();
			// 
			// ribbonControl1
			// 
			this.ribbonControl1.AllowMdiChildButtons = false;
			this.ribbonControl1.CommandLayout = DevExpress.XtraBars.Ribbon.CommandLayout.Simplified;
			this.ribbonControl1.Controller = this.barAndDockingController1;
			this.ribbonControl1.ExpandCollapseItem.Id = 0;
			this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
			this.ribbonControl1.ExpandCollapseItem,
			this.ribbonControl1.SearchEditItem,
			this.logoutButtonItem});
			this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
			this.ribbonControl1.MaxItemId = 2;
			this.ribbonControl1.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
			this.ribbonControl1.Name = "ribbonControl1";
			this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
			this.ribbonPage1});
			this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
			this.ribbonControl1.Size = new System.Drawing.Size(685, 82);
			this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
			// 
			// logoffButtonItem
			// 
			this.logoutButtonItem.Caption = "Log Out";
			this.logoutButtonItem.Id = 1;
			this.logoutButtonItem.ImageOptions.SvgImage = global::WindowsFormsApplication.Properties.Resources.Action_Logoff;
			this.logoutButtonItem.Name = "logoffButtonItem";
			this.logoutButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.LogoutButtonItem_ItemClick);
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
			this.ribbonPageGroup1.ItemLinks.Add(this.logoutButtonItem);
			this.ribbonPageGroup1.Name = "ribbonPageGroup1";
			this.ribbonPageGroup1.Text = "General";
			// 
			// documentManager1
			// 
			this.documentManager1.BarAndDockingController = this.barAndDockingController1;
			this.documentManager1.MdiParent = this;
			this.documentManager1.MenuManager = this.ribbonControl1;
			this.documentManager1.View = this.nativeMdiView1;
			this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
			this.nativeMdiView1});
			// 
			// barAndDockingController1
			// 
			this.barAndDockingController1.PropertiesRibbon.DefaultSimplifiedRibbonGlyphSize = 32;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(685, 600);
			this.Controls.Add(this.ribbonControl1);
			this.Icon = global::WindowsFormsApplication.Properties.Resources.ExpressApp;
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.Ribbon = this.ribbonControl1;
			this.Text = "XAF\'s Security in Non-XAF Apps Demo";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nativeMdiView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
		private DevExpress.XtraBars.BarButtonItem logoutButtonItem;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
		private DevExpress.XtraBars.Docking2010.Views.NativeMdi.NativeMdiView nativeMdiView1;
		private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
	}
}