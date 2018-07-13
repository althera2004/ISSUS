<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="EquipmentList.aspx.cs" Inherits="EquipmentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" runat="Server">
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
    <script type="text/javascript">
        var Equipments = <%=this.EquipmentsJson %>;
        var Filter = "<%=this.Filter %>";
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <div class="col-sm-6">
                                    <table style="width:100%;">
                                        <tr>
                                            <td>
                                                <strong><%=this.Dictionary["Item_Equipment_List_Filter_ShowByOperation"] %>:</strong>
                                            </td>
                                            <td>
                                                <div class="row">
                                                    <input type="checkbox" name="RBOperation" id="RBOperation1" onclick="RenderTable();" checked="checked" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowCalibration"] %>
                                                    &nbsp;&nbsp;
                                                    <input type="checkbox" name="RBOperation" id="RBOperation2" onclick="RenderTable();" checked="checked" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowVerification"] %>
                                                    &nbsp;&nbsp;
                                                    <input type="checkbox" name="RBOperation" id="RBOperation3" onclick="RenderTable();" checked="checked" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowMaintenance"] %>
                                                </div>
                                            </td>
                                        <!--/tr>
                                    </table>
                                    </div>
                                <div class="col-sm-6">
                                    <table style="width:100%;">
                                        <tr   style="width:120px;"-->     
                                            <td>
                                                <strong><%=this.Dictionary["Item_Equipment_List_Filter_ShowByStatus"] %>:</strong>
                                            </td>                                       
                                            <td>
                                                <div class="row">
                                                    <input type="radio" name="RBStatus" id="RBStatus1" onclick="RenderTable();" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowActive"] %>
                                                    &nbsp;&nbsp;
                                                    <input type="radio" name="RBStatus" id="RBStatus2" onclick="RenderTable();" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowClosed"] %>
													&nbsp;&nbsp;
													<input type="radio" name="RBStatus" id="RBStatus0" onclick="RenderTable();" checked="checked" /><%= this.Dictionary["Common_All"] %>
												</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </div> 
                                <div style="height:8px;clear:both;"></div>
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
														<th onclick="Sort(this,'ListDataTable');" id="th0" class="search hidden-40 sort"><%=this.Dictionary["Item_Equipment_Header_Code"] %> - <%=this.Dictionary["Item_Equipment_Header_Description"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable');" id="th1" class="search hidden-480 sort" style="width:350px;"><%=this.Dictionary["Item_Equipment_Header_Location"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable');" id="th2" class="search hidden-480 sort" style="width:250px;"><%=this.Dictionary["Item_Equipment_Header_Responsible"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable', 'money');" id="th3" class="search hidden-480 sort" style="width:120px;"><%=this.Dictionary["Item_Equipment_Header_Cost"] %></th>
			                                            <th style="width:35px;"></th>
			                                            <th style="width:107px;">&nbsp;</th>
		                                            </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="EquipmentData"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>                                            
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalRecords"></span></i></th>
                                                        <th style="width:250px;text-align:right;"><%=this.Dictionary["Common_Total"] %></th>
                                                        <th style="width:120px;text-align:right;"><span id="TotalCost"></span></th>
                                                        <th style="width:141px;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->								
                            </div><!-- /.col -->

                            <!-- Popups -->
                            <div id="EquipmentDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Equipment_Message_DeleteQuestion"] %>&nbsp;<strong><span id="EquipmentName"></span></strong>?</p>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="js/EquipmentList.js?ac=<%=this.AntiCache %>""></script>
</asp:Content>

