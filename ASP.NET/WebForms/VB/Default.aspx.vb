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
            If Not IsGranted(SecurityOperations.Read, employee, e.Column) Then
                e.Editor.Value = "Protected Content"
                e.Editor.Enabled = False
            Else
                If Not IsGranted(SecurityOperations.Write, employee, e.Column) Then
                    e.Editor.Enabled = False
                End If
            End If
        End Sub
        Protected Sub EmployeeGrid_CommandButtonInitialize(ByVal sender As Object, ByVal e As ASPxGridViewCommandButtonEventArgs)
            If e.ButtonType = ColumnCommandButtonType.[New] Then
				If Not IsGranted(SecurityOperations.Create) Then
					e.Text = String.Empty
				End If
			End If
            If e.ButtonType = ColumnCommandButtonType.Delete Then
                Dim employee As Employee = TryCast(DirectCast(sender, ASPxGridView).GetRow(e.VisibleIndex), Employee)
                e.Visible = IsGranted(SecurityOperations.Delete, employee)
            End If
        End Sub
        Protected Sub EmployeeGrid_HtmlDataCellPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableDataCellEventArgs)
            Dim employee As Employee = objectSpace.GetObjectByKey(Of Employee)(e.KeyValue)
            If Not IsGranted(SecurityOperations.Read, employee, e.DataColumn) Then
                e.Cell.Text = "Protected content"
            End If
        End Sub
        Protected Sub LogoffButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            FormsAuthentication.SignOut()
            FormsAuthentication.RedirectToLoginPage()
        End Sub
        Private Shared Function GetMemberName(ByVal column As GridViewDataColumn) As String
            Return column.FieldName.Split("!"c)(0)
        End Function
        Private Function IsGranted(ByVal operation As String, Optional ByVal employee As Object = Nothing, Optional ByVal column As GridViewDataColumn = Nothing) As Boolean
            Dim memberName As String = If(column IsNot Nothing, GetMemberName(column), Nothing)
            Return security.IsGranted(New PermissionRequest(objectSpace, GetType(Employee), operation, employee, memberName))
        End Function
    End Class
End Namespace