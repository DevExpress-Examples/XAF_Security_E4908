Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.Xpo
Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports XafSolution.Module.BusinessObjects

Namespace WindowsFormsApplication
    Partial Public Class EmployeeDetailForm
        Inherits DevExpress.XtraBars.Ribbon.RibbonForm

        Private securedObjectSpace As IObjectSpace
        Private security As SecurityStrategyComplex
        Private objectSpaceProvider As IObjectSpaceProvider
        Private employee As Employee
        Private visibleMembers As Dictionary(Of String, String)
        Public Sub New(ByVal employee As Employee)
            InitializeComponent()
            Me.employee = employee
            visibleMembers = New Dictionary(Of String, String)()
            visibleMembers.Add(nameof(Employee.FirstName), "First Name:")
            visibleMembers.Add(nameof(Employee.LastName), "Last Name:")
            visibleMembers.Add(nameof(Employee.Department), "Department:")
        End Sub
        Private Sub EmployeeDetailForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            security = CType(MdiParent, MainForm).Security
            objectSpaceProvider = CType(MdiParent, MainForm).ObjectSpaceProvider
            securedObjectSpace = objectSpaceProvider.CreateObjectSpace()
            If employee Is Nothing Then
                employee = securedObjectSpace.CreateObject(Of Employee)()
            Else
                employee = securedObjectSpace.GetObject(employee)
                deleteBarButtonItem.Enabled = security.IsGranted(New PermissionRequest(securedObjectSpace, GetType(Employee), SecurityOperations.Delete, employee))
            End If
            employeeBindingSource.DataSource = employee
            AddControls()
        End Sub
        Private Sub AddControls()
            For Each pair As KeyValuePair(Of String, String) In visibleMembers
                Dim memberName As String = pair.Key
                Dim caption As String = pair.Value
                AddControl(dataLayoutControl1.AddItem(), employee, memberName, caption)
            Next pair
        End Sub
        Private Sub AddControl(ByVal layout As LayoutControlItem, ByVal targetObject As Object, ByVal memberName As String, ByVal caption As String)
            layout.Text = caption
            Dim type As Type = targetObject.GetType()
            Dim control As BaseEdit
            If security.IsGranted(New PermissionRequest(securedObjectSpace, type, SecurityOperations.Read, targetObject, memberName)) Then
                control = GetControl(type, memberName)
                If control IsNot Nothing Then
                    control.DataBindings.Add(New Binding("EditValue", employeeBindingSource, memberName, True, DataSourceUpdateMode.OnPropertyChanged))
                    control.Enabled = security.IsGranted(New PermissionRequest(securedObjectSpace, type, SecurityOperations.Write, targetObject, memberName))
                End If
            Else
                control = New ProtectedContentEdit()
                control.Enabled = False
            End If
            dataLayoutControl1.Controls.Add(control)
            layout.Control = control
        End Sub
        Private Function GetControl(ByVal type As Type, ByVal memberName As String) As BaseEdit
            Dim control As BaseEdit = Nothing
            Dim typeInfo As ITypeInfo = securedObjectSpace.TypesInfo.PersistentTypes.FirstOrDefault(Function(t) t.Name = type.Name)
            Dim memberInfo As IMemberInfo = typeInfo.Members.FirstOrDefault(Function(t) t.Name = memberName)
            If memberInfo IsNot Nothing Then
                If memberInfo.IsAssociation Then
                    control = New ComboBoxEdit()
                    CType(control, ComboBoxEdit).Properties.Items.AddRange(TryCast(securedObjectSpace.GetObjects(Of Department)(), XPCollection(Of Department)))
                Else
                    control = New TextEdit()
                End If
            End If
            Return control
        End Function
        Private Sub SaveBarButtonItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles saveBarButtonItem.ItemClick
            securedObjectSpace.CommitChanges()
            Close()
        End Sub
        Private Sub CloseBarButtonItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles closeBarButtonItem.ItemClick
            Close()
        End Sub
        Private Sub DeleteBarButtonItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles deleteBarButtonItem.ItemClick
            securedObjectSpace.Delete(employee)
            securedObjectSpace.CommitChanges()
            Close()
        End Sub
    End Class
End Namespace
