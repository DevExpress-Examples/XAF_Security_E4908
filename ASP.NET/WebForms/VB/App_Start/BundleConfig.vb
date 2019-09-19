Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Optimization
Imports System.Web.UI

Namespace WebFormsApplication
    Public Class BundleConfig
        ' For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)
            bundles.Add((New ScriptBundle("~/bundles/WebFormsJs")).Include("~/Scripts/WebForms/WebForms.js", "~/Scripts/WebForms/WebUIValidation.js", "~/Scripts/WebForms/MenuStandards.js", "~/Scripts/WebForms/Focus.js", "~/Scripts/WebForms/GridView.js", "~/Scripts/WebForms/DetailsView.js", "~/Scripts/WebForms/TreeView.js", "~/Scripts/WebForms/WebParts.js"))

            ' Order is very important for these files to work, they have explicit dependencies
            bundles.Add((New ScriptBundle("~/bundles/MsAjaxJs")).Include("~/Scripts/WebForms/MsAjax/MicrosoftAjax.js", "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js", "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js", "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"))

            ' Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            ' ready for production, use the build tool at https://modernizr.com to pick only the tests you need
            bundles.Add((New ScriptBundle("~/bundles/modernizr")).Include("~/Scripts/modernizr-*"))
        End Sub
    End Class
End Namespace