<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ProvidersList.aspx.cs" Inherits="ProvidersList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv{
            background-color:#fafaff;
            border:1px solid #e0e0e0;
            border-top:none;
            display:block;
        }
        .truncate {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            padding:0;
            margin:0;
        }

        TR:first-child{border-left:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
			                                            <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_Provider_ListHeader_Name"] %></th>
			                                            <th style="width:106px;">&nbsp;</th>
		                                            </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable"><asp:Literal runat="server" ID="ProviderData"></asp:Literal></tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="ProviderDataTotal"></asp:Literal></i></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div> <!-- /.table-responsive -->
                                    </div>
                                </div><!-- /row -->								
                            </div><!-- /.col -->
    <div id="ProviderDeleteDialog" class="hide">
        <p><%=this.Dictionary["Item_Provider_PopupTitle_Delete"] %>&nbsp;<strong><span id="ProviderName"></span></strong>?</p>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="js/common.js"></script>
        <script type="text/javascript" src="js/ProvidersList.js?<%=this.AntiCache %>"></script>
</asp:Content>

