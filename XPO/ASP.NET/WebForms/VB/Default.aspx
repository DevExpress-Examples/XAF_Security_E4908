<%@ Page Title="Employees" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WebFormsApplication._Default" %>

<%@ Register assembly="DevExpress.Xpo.v20.2" namespace="DevExpress.Xpo" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.2" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%">
        <tr>
            <td><h2 style="margin:1%">Employees</h2></td>
            <td><div style="margin:1%; text-align: right;">
            <dx:ASPxButton ID="LogoutButton" runat="server" OnClick="LogoutButton_Click" Text="Log Out" Font-Size="Medium" CssClass="round_corner">
            </dx:ASPxButton>
        </div></td>
        </tr>
    </table>
        <dx:ASPxGridView ID="EmployeeGrid" runat="server" AutoGenerateColumns="False" DataSourceID="EmployeeDataSource" Border-BorderStyle="None"
            KeyFieldName="Oid" 
            SettingsBehavior-ConfirmDelete="true"
            SettingsEditing-Mode="PopupEditForm"
            SettingsPopup-EditForm-HorizontalAlign="WindowCenter"
            SettingsPopup-EditForm-VerticalAlign="WindowCenter"
            SettingsLoadingPanel-Mode="ShowAsPopup"
            SettingsLoadingPanel-Delay="0"
            OnRowInserted="EmployeeGrid_RowInserted" 
            OnRowDeleted="EmployeeGrid_RowDeleted" 
            OnRowUpdated="EmployeeGrid_RowUpdated"
            OnCellEditorInitialize="EmployeeGrid_CellEditorInitialize"
            OnCommandButtonInitialize="EmployeeGrid_CommandButtonInitialize"
            OnHtmlDataCellPrepared="EmployeeGrid_HtmlDataCellPrepared"
            Width="100%">
            <SettingsPager PageSize="30" />  
            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupFooter="VisibleAlways" ShowGroupPanel="True" />
<SettingsPopup>
<HeaderFilter MinHeight="140px"></HeaderFilter>
</SettingsPopup>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="Oid" ReadOnly="True" VisibleIndex="0" Visible="False">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="5" ShowInCustomizationForm="True">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="Department" FieldName="Department!Key" ShowInCustomizationForm="True" VisibleIndex="7">
                    <PropertiesComboBox DataSourceID="DepartmentDataSource" TextField="Title" ValueField="Oid">
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="1">
                </dx:GridViewCommandColumn>
                <dx:GridViewBandColumn Caption="Employee" VisibleIndex="3">
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="FirstName" ShowInCustomizationForm="True" VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LastName" ShowInCustomizationForm="True" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:GridViewBandColumn>
            </Columns>
        </dx:ASPxGridView>
        <dx:XpoDataSource ID="EmployeeDataSource" runat="server" ServerMode="False" TypeName="XafSolution.Module.BusinessObjects.Employee">
        </dx:XpoDataSource>
        <dx:XpoDataSource ID="DepartmentDataSource" runat="server" ServerMode="True" TypeName="XafSolution.Module.BusinessObjects.Department">
        </dx:XpoDataSource>
</asp:Content>