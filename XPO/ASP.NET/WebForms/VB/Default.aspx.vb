Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Web
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports XafSolution.Module.BusinessObjects

Namespace WebFormsApplication
    Partial Public Class _Default
        Inherits System.Web.UI.Page

        Private objectSpace As IObjectSpace
        Private security As SecurityStrategyComplex
        Private objectSpaceProvider As XPObjectSpaceProvider
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            security = ConnectionHelper.GetSecurity(GetType(IdentityAuthenticationProvider).Name, HttpContext.Current.User.Identity)
            objectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(security)
            Dim logonObjectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
            security.Logon(logonObjectSpace)
            objectSpace = objectSpaceProvider.CreateObjectSpace()
            EmployeeDataSource.Session = DirectCast(objectSpace, XPObjectSpace).Session
            DepartmentDataSource.Session = DirectCast(objectSpace, XPObjectSpace).Session
            EmployeeGrid.SettingsText.PopupEditFormCaption = "Employee"
            EmployeeGrid.SettingsPopup.EditForm.Width = 1000
        End Sub
        Protected Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs)
            objectSpace.Dispose()
            security.Dispose()
            objectSpaceProvider.Dispose()
        End Sub
        Protected Sub EmployeeGrid_RowUpdated(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatedEventArgs)
            objectSpace.CommitChanges()
        End Sub
        Protected Sub EmployeeGrid_RowDeleted(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletedEventArgs)
            objectSpace.CommitChanges()
        End Sub
        Protected Sub EmployeeGrid_RowInserted(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertedEventArgs)
            objectSpace.CommitChanges()
        End Sub
        Protected Sub EmployeeGrid_CellEditorInitialize(ByVal sender As Object, ByVal e As ASPxGridViewEditorEventArgs)
            Dim employee As Employee = objectSpace.GetObjectByKey(Of Employee)(e.KeyValue)
			Dim memberName As String = GetMemberName(e.Column)
			If Not security.CanRead(employee, memberName) Then
                e.Editor.Value = "*******"
                e.Editor.Enabled = False
            ElseIf Not security.CanWrite(employee, memberName) Then
                e.Editor.Enabled = False
            End If
        End Sub
        Protected Sub EmployeeGrid_CommandButtonInitialize(ByVal sender As Object, ByVal e As ASPxGridViewCommandButtonEventArgs)
            If e.ButtonType = ColumnCommandButtonType.[New] Then
                If Not security.CanCreate(Of Employee)() Then
                    e.Text = String.Empty
                End If
            End If
            If e.ButtonType = ColumnCommandButtonType.Delete Then
                Dim employee As Employee = TryCast(DirectCast(sender, ASPxGridView).GetRow(e.VisibleIndex), Employee)
                e.Visible = security.CanDelete(employee)
            End If
        End Sub
        Protected Sub EmployeeGrid_HtmlDataCellPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableDataCellEventArgs)
            Dim employee As Employee = objectSpace.GetObjectByKey(Of Employee)(e.KeyValue)
			Dim memberName As String = GetMemberName(e.DataColumn)
			If Not security.CanRead(employee, memberName) Then
                e.Cell.Text = "*******"
            End If
        End Sub
        Protected Sub LogoutButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            FormsAuthentication.SignOut()
            FormsAuthentication.RedirectToLoginPage()
        End Sub
		Private Function GetMemberName(ByVal column As GridViewDataColumn) As String
			Return column?.FieldName.Split("!"c)(0)
		End Function
	End Class
End Namespace