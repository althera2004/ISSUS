<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="EmployeesList.aspx.cs" Inherits="EmployeesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv,#scrollTableDiv2{
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
    <script type="text/javascript">
        var employees = <%=this.EmployeesJson %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div class="col-xs-12">
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <!--<ul class="nav nav-tabs padding-18">
                                                <li class="active">
                                                    <a data-toggle="tab" href="#active" id="SelectorTabActive"><%=this.Dictionary["Common_Active_Plural"] %></a>
                                                </li>
                                                <li class="">
                                                    <a data-toggle="tab" href="#inactive" id="SelectorTabInactive"><%=this.Dictionary["Common_Inactive_Plural"] %></a>
                                                </li>
                                            </ul>-->
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="active" class="tab-pane active"> 
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="table-responsive" id="scrollTableDiv">
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr id="ListDataHeader">
			                                                                <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort"><%=this.Dictionary["Common_Name"] %></th>
			                                                                <th id="th1" class="search" style="width:250px;"><%=this.Dictionary["Item_JobPosition"] %></th>
			                                                                <th id="th2" class="search" style="width:250px;"><%=this.Dictionary["Item_Departments"] %></th>
			                                                                <th style="width:106px;">&nbsp;</th>
		                                                                </tr>
                                                                    </thead>
                                                                </table>
                                                                <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                    <table class="table table-bordered table-striped" style="border-top: none;">
                                                                        <tbody id="ListDataTable">
                                                                            <asp:Literal runat="server" ID="EmployeeData"></asp:Literal>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr id="ListDataFooter">
                                                                            <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="EmployeeDataTotal"></asp:Literal></i></th>
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                            </div>
                                                            <!-- /.table-responsive -->
                                                        </div><!-- /span -->
                                                    </div><!-- /row -->	
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div id="EmployeeDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Employee_PopupDelete_Message"] %>&nbsp;<strong><span id="EmployeeName"></span></strong>?</p>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="assets/js/chosen.jquery.min.js"></script>
        <script type="text/javascript" src="assets/js/fuelux/fuelux.spinner.min.js"></script>
        <script type="text/javascript" src="assets/js/date-time/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="assets/js/date-time/moment.min.js"></script>
        <script type="text/javascript" src="assets/js/date-time/daterangepicker.min.js"></script>
        <script type="text/javascript" src="assets/js/bootstrap-colorpicker.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.knob.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.autosize.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.inputlimiter.1.3.1.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.maskedinput.min.js"></script>
        <script type="text/javascript" src="assets/js/bootstrap-tag.min.js"></script>
        <script type="text/javascript" src="js/common.js"></script>
        <script type="text/javascript" src="js/EmployeesList.js?<%=this.AntiCache %>"></script>
</asp:Content>

