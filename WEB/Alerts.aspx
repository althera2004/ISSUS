<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="Alerts.aspx.cs" Inherits="Alerts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css2" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive">
                                            <table class="table table-bordered table-striped" style="margin-bottom:50px;">
                                                <%=this.DataHeader.Render %>
                                                <tbody id="ListDataTable"><asp:Literal runat="server" ID="AlertsData"></asp:Literal></tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>				
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
</asp:Content>