<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="UserList" %>

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
                                                        <th id="th0" style="width:40px;">&nbsp;</th>
			                                            <th onclick="Sort(this,'ListDataTable');" id="th1" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_User_List_Header_UserName"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable');" id="th2" class="search sort" style="cursor:pointer;width:300px;"><%=this.Dictionary["Item_User_List_Header_EmployeeName"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable');" id="th3" class="search sort" style="cursor:pointer;width:300px;"><%=this.Dictionary["Item_User_List_Header_Email"] %></th>
			                                            <th style="width:107px;">&nbsp;</th>
		                                            </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="UsersData"></asp:Literal></tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <td><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<strong><asp:Literal runat="server" ID="UsersDataTotal"></asp:Literal></strong></td>
                                                    </tr>
                                                </thead>
                                            </table>

                                        </div>
                                        <!-- /.table-responsive -->
                                    </div>
                                        </div><!-- /row -->	
                                    </div>
                            <div id="UserDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_User_PopupDelete_Message"] %>&nbsp;<strong><span id="UserName"></span></strong>?</p>
                            </div>
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
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UserList.js?<%=this.AntiCache %>"></script>
</asp:Content>

