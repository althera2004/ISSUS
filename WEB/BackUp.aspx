<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="BackUp.aspx.cs" Inherits="BackUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <asp:Literal runat="server" ID="LtExplain"></asp:Literal>
    <div style="clear:both;">&nbsp;</div>
    <button class="btn btn-success" type="button" id="Button1" onclick="Go();">
        <i class="icon-check bigger-110"></i>
        <%=this.Dictionary["Item_Backup_Btn_Go"]%>
    </button>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/assets/js/chosen.jquery.min.js"></script>
        <script type="text/javascript" src="/assets/js/fuelux/fuelux.spinner.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/moment.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/daterangepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-colorpicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.knob.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.autosize.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.inputlimiter.1.3.1.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.maskedinput.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-tag.min.js"></script>
        <script type="text/javascript" src="/js/common.js?ac<%= this.AntiCache %>"></script>
    <script type="text/javascript">
    function Go()
    {
        var data =
        {
            "companyId": <%=this.CompanyId %>
        };

        var webMethod = "/Export/ExportBackup.aspx/Go";
        LoadingShow(Dictionary.Common_Report_Rendering);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                LoadingHide();
                var link = document.createElement("a");
                link.id = "download";
                link.href = msg.d.MessageError;
                link.download = msg.d.MessageError;
                link.target = "_blank";
                document.body.appendChild(link);
                document.body.removeChild(link);
                $("#download").trigger("click");
                window.open(msg.d.MessageError);
                $("#dialogAddAddress").dialog("close");
            },
            error: function (msg) {
                LoadingHide();
                alertUI("error:" + msg.responseText);
            }
        });
        }
    </script>
</asp:Content>