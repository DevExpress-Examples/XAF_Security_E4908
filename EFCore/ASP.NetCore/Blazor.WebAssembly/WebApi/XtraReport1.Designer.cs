namespace WebAPI
{
	partial class XtraReport1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
			this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.label1 = new DevExpress.XtraReports.UI.XRLabel();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.table1 = new DevExpress.XtraReports.UI.XRTable();
			this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
			this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.table2 = new DevExpress.XtraReports.UI.XRTable();
			this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
			this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
			this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
			this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
			this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
			this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
			this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
			this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
			this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
			this.collectionDataSource1 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
			((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// TopMargin
			// 
			this.TopMargin.Name = "TopMargin";
			// 
			// BottomMargin
			// 
			this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pageInfo1,
            this.pageInfo2});
			this.BottomMargin.Name = "BottomMargin";
			// 
			// pageInfo1
			// 
			this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.pageInfo1.Name = "pageInfo1";
			this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
			this.pageInfo1.SizeF = new System.Drawing.SizeF(325F, 23F);
			this.pageInfo1.StyleName = "PageInfo";
			// 
			// pageInfo2
			// 
			this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(325F, 0F);
			this.pageInfo2.Name = "pageInfo2";
			this.pageInfo2.SizeF = new System.Drawing.SizeF(325F, 23F);
			this.pageInfo2.StyleName = "PageInfo";
			this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			this.pageInfo2.TextFormatString = "Page {0} of {1}";
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.label1});
			this.ReportHeader.HeightF = 60F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// label1
			// 
			this.label1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.label1.Name = "label1";
			this.label1.SizeF = new System.Drawing.SizeF(650F, 24.19433F);
			this.label1.StyleName = "Title";
			this.label1.Text = "Report Title";
			// 
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
			this.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
			this.GroupHeader1.HeightF = 28F;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// table1
			// 
			this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.table1.Name = "table1";
			this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
			this.table1.SizeF = new System.Drawing.SizeF(650F, 28F);
			// 
			// tableRow1
			// 
			this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell1,
            this.tableCell2,
            this.tableCell3});
			this.tableRow1.Name = "tableRow1";
			this.tableRow1.Weight = 1D;
			// 
			// tableCell1
			// 
			this.tableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.tableCell1.Name = "tableCell1";
			this.tableCell1.StyleName = "DetailCaption1";
			this.tableCell1.StylePriority.UseBorders = false;
			this.tableCell1.StylePriority.UseTextAlignment = false;
			this.tableCell1.Text = "Post Id";
			this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.tableCell1.Weight = 0.35497415395883414D;
			// 
			// tableCell2
			// 
			this.tableCell2.Name = "tableCell2";
			this.tableCell2.StyleName = "DetailCaption1";
			this.tableCell2.Text = "Title";
			this.tableCell2.Weight = 0.25555807260366586D;
			// 
			// tableCell3
			// 
			this.tableCell3.Name = "tableCell3";
			this.tableCell3.StyleName = "DetailCaption1";
			this.tableCell3.Text = "Content";
			this.tableCell3.Weight = 0.3894677734375D;
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table2});
			this.Detail.HeightF = 25F;
			this.Detail.Name = "Detail";
			// 
			// table2
			// 
			this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.table2.Name = "table2";
			this.table2.OddStyleName = "DetailData3_Odd";
			this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
			this.table2.SizeF = new System.Drawing.SizeF(650F, 25F);
			// 
			// tableRow2
			// 
			this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell4,
            this.tableCell5,
            this.tableCell6});
			this.tableRow2.Name = "tableRow2";
			this.tableRow2.Weight = 11.5D;
			// 
			// tableCell4
			// 
			this.tableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.tableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PostId]")});
			this.tableCell4.Name = "tableCell4";
			this.tableCell4.StyleName = "DetailData1";
			this.tableCell4.StylePriority.UseBorders = false;
			this.tableCell4.StylePriority.UseTextAlignment = false;
			this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.tableCell4.Weight = 0.35497415395883414D;
			// 
			// tableCell5
			// 
			this.tableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Title]")});
			this.tableCell5.Name = "tableCell5";
			this.tableCell5.StyleName = "DetailData1";
			this.tableCell5.Weight = 0.25555807260366586D;
			// 
			// tableCell6
			// 
			this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Content]")});
			this.tableCell6.Name = "tableCell6";
			this.tableCell6.StyleName = "DetailData1";
			this.tableCell6.Weight = 0.3894677734375D;
			// 
			// objectDataSource1
			// 
			this.objectDataSource1.DataSource = typeof(WebAPI.BusinessObjects.Post);
			this.objectDataSource1.Name = "objectDataSource1";
			// 
			// Title
			// 
			this.Title.BackColor = System.Drawing.Color.Transparent;
			this.Title.BorderColor = System.Drawing.Color.Black;
			this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.Title.BorderWidth = 1F;
			this.Title.Font = new DevExpress.Drawing.DXFont("Arial", 14.25F);
			this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
			this.Title.Name = "Title";
			this.Title.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
			// 
			// DetailCaption1
			// 
			this.DetailCaption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
			this.DetailCaption1.BorderColor = System.Drawing.Color.White;
			this.DetailCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
			this.DetailCaption1.BorderWidth = 2F;
			this.DetailCaption1.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
			this.DetailCaption1.ForeColor = System.Drawing.Color.White;
			this.DetailCaption1.Name = "DetailCaption1";
			this.DetailCaption1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
			this.DetailCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// DetailData1
			// 
			this.DetailData1.BorderColor = System.Drawing.Color.Transparent;
			this.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
			this.DetailData1.BorderWidth = 2F;
			this.DetailData1.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F);
			this.DetailData1.ForeColor = System.Drawing.Color.Black;
			this.DetailData1.Name = "DetailData1";
			this.DetailData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
			this.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// DetailData3_Odd
			// 
			this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
			this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
			this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.DetailData3_Odd.BorderWidth = 1F;
			this.DetailData3_Odd.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F);
			this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
			this.DetailData3_Odd.Name = "DetailData3_Odd";
			this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
			this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// PageInfo
			// 
			this.PageInfo.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
			this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
			this.PageInfo.Name = "PageInfo";
			this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
			// 
			// collectionDataSource1
			// 
			this.collectionDataSource1.Name = "collectionDataSource1";
			this.collectionDataSource1.ObjectTypeName = "WebAPI.BusinessObjects.Post";
			this.collectionDataSource1.TopReturnedRecords = 0;
			// 
			// XtraReport1
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1,
            this.Detail});
			this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1,
            this.collectionDataSource1});
			this.DataSource = this.collectionDataSource1;
			this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
			this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.DetailData3_Odd,
            this.PageInfo});
			this.Version = "22.2";
			((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

		#endregion

		private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
		private DevExpress.XtraReports.UI.XRPageInfo pageInfo1;
		private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
		private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
		private DevExpress.XtraReports.UI.XRLabel label1;
		private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
		private DevExpress.XtraReports.UI.XRTable table1;
		private DevExpress.XtraReports.UI.XRTableRow tableRow1;
		private DevExpress.XtraReports.UI.XRTableCell tableCell1;
		private DevExpress.XtraReports.UI.XRTableCell tableCell2;
		private DevExpress.XtraReports.UI.XRTableCell tableCell3;
		private DevExpress.XtraReports.UI.DetailBand Detail;
		private DevExpress.XtraReports.UI.XRTable table2;
		private DevExpress.XtraReports.UI.XRTableRow tableRow2;
		private DevExpress.XtraReports.UI.XRTableCell tableCell4;
		private DevExpress.XtraReports.UI.XRTableCell tableCell5;
		private DevExpress.XtraReports.UI.XRTableCell tableCell6;
		private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
		private DevExpress.XtraReports.UI.XRControlStyle Title;
		private DevExpress.XtraReports.UI.XRControlStyle DetailCaption1;
		private DevExpress.XtraReports.UI.XRControlStyle DetailData1;
		private DevExpress.XtraReports.UI.XRControlStyle DetailData3_Odd;
		private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
		private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource1;
	}
}
