<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="ViewSwitcher.ascx.vb" Inherits="WebFormsApplication.ViewSwitcher" %>
<div id="viewSwitcher">
    <%:CurrentView%> view | <a href="<%:SwitchUrl%>" data-ajax="false">Switch to <%:AlternateView%></a>
</div>