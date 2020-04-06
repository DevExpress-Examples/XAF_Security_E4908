Namespace WindowsFormsApplication
    Partial Public Class MainForm
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
            Me.logoutButtonItem = New DevExpress.XtraBars.BarButtonItem()
            Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
            Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            Me.documentManager1 = New DevExpress.XtraBars.Docking2010.DocumentManager(Me.components)
            Me.nativeMdiView1 = New DevExpress.XtraBars.Docking2010.Views.NativeMdi.NativeMdiView(Me.components)
            Me.barAndDockingController1 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
            CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.documentManager1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nativeMdiView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.barAndDockingController1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' ribbonControl1
            ' 
            Me.ribbonControl1.AllowMdiChildButtons = False
            Me.ribbonControl1.CommandLayout = DevExpress.XtraBars.Ribbon.CommandLayout.Simplified
            Me.ribbonControl1.Controller = Me.barAndDockingController1
            Me.ribbonControl1.ExpandCollapseItem.Id = 0
            Me.ribbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() { Me.ribbonControl1.ExpandCollapseItem, Me.ribbonControl1.SearchEditItem, Me.logoutButtonItem})
            Me.ribbonControl1.Location = New System.Drawing.Point(0, 0)
            Me.ribbonControl1.MaxItemId = 2
            Me.ribbonControl1.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always
            Me.ribbonControl1.Name = "ribbonControl1"
            Me.ribbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() { Me.ribbonPage1})
            Me.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            Me.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide
            Me.ribbonControl1.Size = New System.Drawing.Size(685, 82)
            Me.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
            ' 
            ' logoffButtonItem
            ' 
            Me.logoutButtonItem.Caption = "Log Out"
            Me.logoutButtonItem.Id = 1
            Me.logoutButtonItem.ImageOptions.SvgImage = My.Resources.Action_Logoff
            Me.logoutButtonItem.Name = "logoffButtonItem"
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
            Me.ribbonPageGroup1.ItemLinks.Add(Me.logoutButtonItem)
            Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
            Me.ribbonPageGroup1.Text = "General"
            ' 
            ' documentManager1
            ' 
            Me.documentManager1.BarAndDockingController = Me.barAndDockingController1
            Me.documentManager1.MdiParent = Me
            Me.documentManager1.MenuManager = Me.ribbonControl1
            Me.documentManager1.View = Me.nativeMdiView1
            Me.documentManager1.ViewCollection.AddRange(New DevExpress.XtraBars.Docking2010.Views.BaseView() { Me.nativeMdiView1})
            ' 
            ' barAndDockingController1
            ' 
            Me.barAndDockingController1.PropertiesRibbon.DefaultSimplifiedRibbonGlyphSize = 32
            ' 
            ' MainForm
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(685, 600)
            Me.Controls.Add(Me.ribbonControl1)
            Me.Icon = My.Resources.ExpressApp
            Me.IsMdiContainer = True
            Me.Name = "MainForm"
            Me.Ribbon = Me.ribbonControl1
            Me.Text = "XAF's Security in Non-XAF Apps Demo"
            CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.documentManager1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nativeMdiView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.barAndDockingController1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        #End Region

        Private ribbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
        Private WithEvents logoutButtonItem As DevExpress.XtraBars.BarButtonItem
        Private ribbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
        Private ribbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
        Private documentManager1 As DevExpress.XtraBars.Docking2010.DocumentManager
        Private nativeMdiView1 As DevExpress.XtraBars.Docking2010.Views.NativeMdi.NativeMdiView
        Private barAndDockingController1 As DevExpress.XtraBars.BarAndDockingController
    End Class
End Namespace