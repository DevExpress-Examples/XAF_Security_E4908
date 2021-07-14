Imports BusinessObjectsLibrary
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.Xpo
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports System
Imports System.Linq
Imports System.Windows.Forms

Namespace WindowsFormsApplication
    Partial Public Class EmployeeListForm
        Inherits DevExpress.XtraBars.Ribbon.RibbonForm

        Private securedObjectSpace As IObjectSpace
        Private security As SecurityStrategyComplex
        Private objectSpaceProvider As IObjectSpaceProvider
        Private protectedContentTextEdit As RepositoryItemProtectedContentTextEdit
        Public Sub New()
            InitializeComponent()
        End Sub
        Private Sub EmployeeListForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            security = CType(MdiParent, MainForm).Security
            objectSpaceProvider = CType(MdiParent, MainForm).ObjectSpaceProvider
            securedObjectSpace = objectSpaceProvider.CreateObjectSpace()
            employeeBindingSource.DataSource = securedObjectSpace.GetObjects(Of Employee)()
            newBarButtonItem.Enabled = security.CanCreate(Of Employee)()
            protectedContentTextEdit = New RepositoryItemProtectedContentTextEdit()
        End Sub
        Private Sub GridView_CustomRowCellEdit(ByVal sender As Object, ByVal e As CustomRowCellEditEventArgs) Handles employeeGridView.CustomRowCellEdit
            Dim fieldName As String = e.Column.FieldName
            Dim targetObject As Object = employeeGridView.GetRow(e.RowHandle)
            If Not security.CanRead(targetObject, fieldName) Then
                e.RepositoryItem = protectedContentTextEdit
            End If
        End Sub
        Private Sub CreateDetailForm(Optional ByVal employee As Employee = Nothing)
            Dim detailForm As New EmployeeDetailForm(employee)
            detailForm.MdiParent = MdiParent
            detailForm.WindowState = FormWindowState.Maximized
            detailForm.Show()
            AddHandler detailForm.FormClosing, AddressOf DetailForm_FormClosing
        End Sub
        Private Sub DetailForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            Dim collection As XPBaseCollection = CType(employeeBindingSource.DataSource, XPBaseCollection)
            collection.Reload()
        End Sub
        Private Sub EmployeeGridView_RowClick(ByVal sender As Object, ByVal e As RowClickEventArgs) Handles employeeGridView.RowClick
            If e.Clicks = 2 Then
                EditEmployee()
            End If
        End Sub
        Private Sub EmployeeGridView_FocusedRowObjectChanged(ByVal sender As Object, ByVal e As FocusedRowObjectChangedEventArgs) Handles employeeGridView.FocusedRowObjectChanged
            deleteBarButtonItem.Enabled = security.CanDelete(e.Row)
        End Sub
        Private Sub NewBarButtonItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles newBarButtonItem.ItemClick
            CreateDetailForm()
        End Sub
        Private Sub DeleteBarButtonItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles deleteBarButtonItem.ItemClick
            Dim cellObject As Object = employeeGridView.GetRow(employeeGridView.FocusedRowHandle)
            securedObjectSpace.Delete(cellObject)
            securedObjectSpace.CommitChanges()
        End Sub
        Private Sub EditBarButtonItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles editBarButtonItem.ItemClick
            EditEmployee()
        End Sub
        Private Sub EditEmployee()

            Dim employee_Renamed As Employee = TryCast(employeeGridView.GetRow(employeeGridView.FocusedRowHandle), Employee)
            CreateDetailForm(employee_Renamed)
        End Sub
    End Class
End Namespace
