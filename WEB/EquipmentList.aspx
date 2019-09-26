<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="EquipmentList.aspx.cs" Inherits="EquipmentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv, #scrollTableDivCosts{
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
        var Costs = <%=this.Costs %>;
        var Filter = "<%=this.Filter %>";
        var FilterCosts = "<%= this.FilterCosts %>";
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <div class="tabbable">
                                    <ul class="nav nav-tabs padding-18">
                                        <li class="active" id="TabEquipmentList">
                                            <a data-toggle="tab" href="#equipmentList"><%=this.Dictionary["Item_Equipment_Plural"] %></a>
                                        </li>
                                        <li class="" id="TabCostList">
                                            <a data-toggle="tab" href="#costList"><%=this.Dictionary["Item_Equipment_Tab_Costs"] %></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content no-border">
                                        <div id="equipmentList" class="tab-pane active">
                                            <div class="col-sm-10">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                            <strong><%=this.Dictionary["Item_Equipment_List_Filter_ShowByOperation"] %>:</strong>
                                                        </td>
                                                        <td>
                                                            <div class="row">
                                                                <input type="checkbox" name="RBOperation" id="RBOperation1" onclick="RBOperationChanged();" checked="checked" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowCalibration"] %>
                                                                &nbsp;&nbsp;
                                                                <input type="checkbox" name="RBOperation" id="RBOperation2" onclick="RBOperationChanged();" checked="checked" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowVerification"] %>
                                                                &nbsp;&nbsp;
                                                                <input type="checkbox" name="RBOperation" id="RBOperation3" onclick="RBOperationChanged();" checked="checked" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowMaintenance"] %>
                                                            </div>
                                                        </td>  
                                                        <td>
                                                            <strong><%=this.Dictionary["Item_Equipment_List_Filter_ShowByStatus"] %>:</strong>
                                                        </td>                                       
                                                        <td>
                                                            <div class="row">
                                                                <input type="checkbox" name="RBStatus" id="RBStatus1" onclick="RBStatusChanged();" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowActive"] %>
                                                                &nbsp;&nbsp;
                                                                <input type="checkbox" name="RBStatus" id="RBStatus2" onclick="RBStatusChanged();" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowClosed"] %>
													            <!--&nbsp;&nbsp;
													            <input type="radio" name="RBStatus" id="RBStatus0" onclick="RenderTable();" checked="checked" /><%= this.Dictionary["Common_All"] %>-->
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
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalList"></span></i></th>
                                                                    <!--<th style="width:250px;text-align:right;"><%=this.Dictionary["Common_Total"] %></th>
                                                                    <th style="width:120px;text-align:right;"><span id="TotalAmount"></span></th>
                                                                    <th style="width:141px;">&nbsp;</th>-->
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div><!-- /.table-responsive -->
                                                </div><!-- /span -->
                                            </div><!-- /row -->								
                                        </div>
                                        <div id="costList" class="tab-pane">
                                            <div class="col-xs-11">
                                                <table>
                                                    <tr>
                                                        <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_Incident_List_Filter_Periode1"] %>:</strong></td>
										                <td>
                                                            <div class="col-xs-12 col-sm-12" style="margin:0;padding:0;">
												                <div class="input-group">
													                <input class="form-control date-picker" style="width:85px;" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                <span class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();" id="TxtDateFromBtn">
														                <i class="icon-calendar bigger-110"></i>
													                </span>
												                </div>
											                    <span class="ErrorMessage" id="TxtDateFromErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                    <span class="ErrorMessage" id="TxtDateFromErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                    <span class="ErrorMessage" id="TxtDateFromDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                            </div>
										                </td>
                                                        <td>&nbsp;-&nbsp;</td>
                                                        <td>
                                                            <div class="col-xs-12 col-sm-12" style="margin:0;padding:0;">
												                <div class="input-group">
													                <input class="form-control date-picker" style="width:85px;" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                <span class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();" id="TxtDateToBtn">
														                <i class="icon-calendar bigger-110"></i>
													                </span>
												                </div>
											                    <span class="ErrorMessage" id="TxtDateToErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                    <span class="ErrorMessage" id="TxtDateToErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                    <span class="ErrorMessage" id="TxtDateToDateMalformed""><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                            </div>
										                </td>
                                                        <td style="width:25px;">&nbsp;</td>
                                                        <td><strong><%=this.Dictionary["Item_Equipment_FilterLabel_CostType"] %>:&nbsp;</strong></td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBCI" checked="checked" />&nbsp;<%= this.Dictionary["Item_Equipment_FilterLabel_Calibration_Int"] %></td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBVI" checked="checked" />&nbsp;<%= this.Dictionary["Item_Equipment_FilterLabel_Verification_Int"] %></td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBMI" checked="checked" />&nbsp;<%= this.Dictionary["Item_Equipment_FilterLabel_Maintenance_Int"] %></td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBRI" checked="checked" />&nbsp;<%= this.Dictionary["Item_Equipment_FilterLabel_Repair_Int"] %></td>  
                                                        <!--<td><strong><%=this.Dictionary["Item_Equipment_List_Filter_ShowByStatus"] %>:</strong></td>                                       
                                                        <td>
                                                            <input type="checkbox" name="RBStatusCost" id="RBCostStatus1" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowActive"] %>
                                                            &nbsp;&nbsp;
                                                            <input type="checkbox" name="RBStatusCost" id="RBCostStatus2" /><%= this.Dictionary["Item_Equipment_List_Filter_ShowClosed"] %>
                                                        </td>-->
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">&nbsp;</td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBCE" checked="checked" /><%= this.Dictionary["Item_Equipment_FilterLabel_Calibration_Ext"] %></td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBVE" checked="checked" /><%= this.Dictionary["Item_Equipment_FilterLabel_Verification_Ext"] %></td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBME" checked="checked" /><%= this.Dictionary["Item_Equipment_FilterLabel_Maintenance_Ext"] %></td>
                                                        <td><input type="checkbox" name="RBOperationCost" id="RBRE" checked="checked" /><%= this.Dictionary["Item_Equipment_FilterLabel_Repair_Ext"] %></td> 
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="col-xs-1">
                                                <button class="btn btn-success" style="height:24px;padding-top:0;" type="button" id="BtnRecordShowAll" title="<%=this.Dictionary["Common_All_Female_Plural"] %>" onclick="SetFilterCosts();"><i class="icon-list" style="margin-top:-2px;"></i>&nbsp;Filtrar</button>
                                            </div>
                                            <div style="height:8px;clear:both;"></div>
                                            
                                            <div class="row">
                                                <div class="col-xs-12">
                                                    <div class="table-responsive" id="scrollTableDivCosts">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeaderCosts">
														            <th id="TrCost"><%=this.Dictionary["Item_Equipment_Header_Code"] %> - <%=this.Dictionary["Item_Equipment_Header_Description"] %></th>
                                                                    <th id="HCI" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Calibration_Int"] %></th>
                                                                    <th id="HCE" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Calibration_Ext"] %></th>
                                                                    <th id="HVI" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Verification_Int"] %></th>
                                                                    <th id="HVE" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Verification_Ext"] %></th>
                                                                    <th id="HMI" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Maintenance_Int"] %></th>
                                                                    <th id="HME" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Maintenance_Ext"] %></th>
                                                                    <th id="HRI" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Repair_Int"] %></th>
                                                                    <th id="HRE" style="display:none;"><%=this.Dictionary["Item_Equipment_FilterLabel_Repair_Ext"] %></th>
                                                                    <th id="HT" style="display:none;"><%=this.Dictionary["Common_Total"] %></th>
                                                                    <th style="width:17px;"></th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDivCosts" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTableCosts">
                                                                    <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>                                            
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooterCosts">
														            <td id="TfCost"><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<strong id="TotalListCosts">-</strong></td>
                                                                    <td id="TCI" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TCE" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TVI" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TVE" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TMI" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TME" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TRI" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TRE" style="font-weight:bold;text-align:right;display:none;"></td>
                                                                    <td id="TT" style="font-weight:bold;text-align:right;display:none;color:#000;"></td>
                                                                    <td style="width:17px;"></td>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                    </div><!-- /.table-responsive -->
                                                </div><!-- /span -->
                                            </div><!-- /row -->	
                                        </div>
                                </div>
                            </div>

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