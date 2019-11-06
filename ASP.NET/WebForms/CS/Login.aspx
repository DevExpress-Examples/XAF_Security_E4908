<%@ Page Title="Authentication" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebFormsApplication.LoginForm" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function errorMessage() {
            alert("User name or password is incorrect");
        }
        function validateLoginForm(s, e) {
            e.processOnServer = ASPxClientEdit.ValidateGroup('LoginForm');
        }
    </script>
    <h1 style="margin-top:5%; margin-bottom:5%;">Welcome to the XAF's Security in Non-XAF Apps Demo</h1>
    <div style="text-align:center">
        <h3>Enter your user name (<b>Admin</b> or <b>User</b>) to proceed.</h3>
        <div style="width:300px; margin:auto">
            <div style="margin-top: 7%">
                <dx:ASPxTextBox ID="UserNameBox" runat="server" NullText="User Name" Font-Size="Medium" Width="300px" TabIndex="2" CssClass="round_corner" >
                    <ValidationSettings ValidationGroup="LoginForm">
                        <RequiredField IsRequired="true" ErrorText="The user name must not be empty" />
                    </ValidationSettings>
                    <RootStyle CssClass="centered"></RootStyle>
                    <CaptionSettings Position="Top" HorizontalAlign="Left"/>  
                </dx:ASPxTextBox>
            </div>
            <div  style="margin-top: 3%">
                <dx:ASPxTextBox ID="PasswordBox" runat="server" Password="True" NullText="Password" Font-Size="Medium" Width="300px" TabIndex="3" CssClass="round_corner">
                    <RootStyle CssClass="centered"></RootStyle>
                    <CaptionSettings Position="Top" HorizontalAlign="Left"/>  
                </dx:ASPxTextBox>
            </div>
            <h3 style="margin-bottom:0; color:#bdbdbd">This demo app does not require</h3>
            <h3 style="margin:0; color:#bdbdbd">a password for login.</h3>
            <div style="margin-top: 15%;">
                <dx:ASPxButton ID="LoginButton" runat="server" OnClick="LoginButton_Click" Text="Log In" Font-Size="Medium" Width="30%" TabIndex="1" CssClass="round_corner">
                    <ClientSideEvents Click="validateLoginForm" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</asp:Content>

