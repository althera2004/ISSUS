<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="RulesList.aspx.cs" Inherits="RulesList" %>

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
                                                        <th onclick="Sort(this,'ListDataTable','text');" id="th0" class="sort search"><%=this.Dictionary["Item_Rules_Popup_Header"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','money');" id="th1" class="sort search" style="width: 100px;"><%=this.Dictionary["Item_Rules_Popup_HeaderLimit"] %></th>
                                                        <th style="width:107px;" class="hidden-480">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="CustomerData"></asp:Literal></tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="RulesDataTotal"></asp:Literal></i></th>
                                                    </tr>
                                                </thead>
                                            </table>

                                        </div>
                                        <!-- /.table-responsive -->
                                    </div>
                                    <!-- /span -->
                                </div>
                                <!-- /row -->
                            </div><!-- /.col -->

                            
    <div id="RuleDeleteDialog" class="hide" style="width: 600px;">
        <p><%=this.Dictionary["Item_Rules_PopupDelete_Message"] %>&nbsp;<strong><span id="RuleName"></span></strong>?</p>
    </div>                                                    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/RulesList.js?<%=this.AntiCache %>"></script>
</asp:Content>

