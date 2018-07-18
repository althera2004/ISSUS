<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="IncidentList.aspx.cs" Inherits="IncidentList" %>

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
    <script type="text/javascript">
        var Filter = <%=this.Filter %>;
        var Departments = <%=this.DepartmentsList %>;
        var Providers = <%=this.ProvidersList %>;
        var Customers = <%=this.CustomersList %>;
        var IncidentList;
        var IncidentSelectedId;
        var IncidentSelected;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <!--/div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="col-sm-12">
                                    
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_Incident_List_Filter_Periode1"] %>:</strong></td>
										    <td>
                                                <div class="col-xs-12 col-sm-12">
												    <div class="input-group">
													    <input class="form-control date-picker" style="width:100px;" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													    <span class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();" id="TxtDateFromBtn">
														    <i class="icon-calendar bigger-110"></i>
													    </span>
												    </div>
											        <span class="ErrorMessage" id="TxtDateFromErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
											        <span class="ErrorMessage" id="TxtDateFromErrorDateRange" style="display:none;"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											        <span class="ErrorMessage" id="TxtDateFromDateMalformed" style="display:none;"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                </div>
										    </td>
                                            <td id="TxtDateToLabel"><%=this.Dictionary["Item_Incident_List_Filter_Periode2"] %></td>
										    <td>
                                                <div class="col-xs-12 col-sm-12">
												    <div class="input-group">
													    <input class="form-control date-picker" style="width:100px;" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													    <span class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();" id="TxtDateToBtn">
														    <i class="icon-calendar bigger-110"></i>
													    </span>
												    </div>
											        <span class="ErrorMessage" id="TxtDateToErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
											        <span class="ErrorMessage" id="TxtDateToErrorDateRange" style="display:none;"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											        <span class="ErrorMessage" id="TxtDateToDateMalformed" style="display:none;"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                </div>
										    </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td style="width:100px;padding-left:20px;"><strong><%=this.Dictionary["Item_Incident_List_Filter_Status"] %>:</strong></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="RIncidentStatus1" checked="checked" /><%=this.Dictionary["Item_Incident_Status1"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="RIncidentStatus2" checked="checked" /><%=this.Dictionary["Item_Incident_Status2"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="RIncidentStatus3" checked="checked" /><%=this.Dictionary["Item_Incident_Status3"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="RIncidentStatus4" /><%=this.Dictionary["Item_Incident_Status4"] %></td>

                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td> 
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>
                                                <!--div class="col-xs-12 col-sm-12"-->                                      
                                                    <!--button style="width:100px;display:none;" class="btn btn-success" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Common_Filter"] %></button-->
                                                    <button class="btn btn-success" type="button" id="BtnRecordShowAll" title="<%=this.Dictionary["Common_All_Female_Plural"] %>"><i class="icon-list"></i></button>
                                                    <button class="btn btn-success" type="button" id="BtnRecordShowNone" title="<%=this.Dictionary["Common_None_Female"] %>"><i class="icon-remove-circle"></i></button>
                                                <!--/div-->
                                            </td>
                                            
                                        </tr>

                                        <tr>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
										    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
										    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                            <td style="width:100px;padding-left:20px;"><strong><%=this.Dictionary["Item_IncidentAction_Header_Origin"] %>:</strong></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="radio" id="ROrigin0" name="ROrigin" onclick="ShowCombos(0);" checked="checked" /><%=this.Dictionary["Item_Incident_Origin0"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="radio" id="ROrigin1" name="ROrigin" onclick="ShowCombos(1);" /><%=this.Dictionary["Item_Incident_Origin1"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="radio" id="ROrigin2" name="ROrigin" onclick="ShowCombos(2);" /><%=this.Dictionary["Item_Incident_Origin2"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="radio" id="ROrigin3" name="ROrigin" onclick="ShowCombos(3);" /><%=this.Dictionary["Item_Incident_Origin3"] %></td>
                                            <td>
                                                <select style="width:200px;display:none;" id="CmbOrigin1"></select>
                                                <select style="width:200px;display:none;" id="CmbOrigin2"></select>
                                                <select style="width:200px;display:none;" id="CmbOrigin3"></select>
                                            </td>

                                        </tr>
                                    </table>
                                    
                                </div> 



                                <!-- GTK CLG Start -->

                                <!--div class="col-sm-12">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:250px;">
                                                <div class="row">
                                                    <label id="TxtDateFromLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_Incident_List_Filter_From"] %></label>										
                                                    <div class="col-xs-9 col-sm-9 tooltip-info" id="TxtDateFromDiv">
                                                        <div class="input-group">
                                                            <input class="form-control date-picker" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                            <span id="TxtDateFromBtn" class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();">
                                                                <i class="icon-calendar bigger-110"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="width:100px;">&nbsp;</td>
                                            <td style="width:100px;padding-left:20px;"><strong><%=this.Dictionary["Item_Incident_List_Filter_Status"] %>:</strong></td>
                                            <td><input type="checkbox" id="RIncidentStatus1" checked="checked" /><%=this.Dictionary["Item_Incident_Status1"] %></td>
                                            <td><input type="checkbox" id="RIncidentStatus2" checked="checked" /><%=this.Dictionary["Item_Incident_Status2"] %></td>
                                            <td><input type="checkbox" id="RIncidentStatus3" checked="checked" /><%=this.Dictionary["Item_Incident_Status3"] %></td>
                                            <td><input type="checkbox" id="RIncidentStatus4" /><%=this.Dictionary["Item_Incident_Status4"] %></td>
                                            <!--td style="width:200px;">&nbsp;-->
                                            <!--td>--
                                            <!--div class="row">
                                                <div class="col-sm-1"></div>                                        
                                                <div class="col-sm-11" style="text-align:right"-->
                                                    <!--button style="width:100px;display:none;" class="btn btn-success" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Common_Filter"] %></!--button>
                                                    <button style="width:100px;" class="btn btn-success" type="button" id="BtnRecordShowAll"><i class="icon-list bigger-110"></i><%=this.Dictionary["Common_All_Female_Plural"] %></button>
                                                    <button style="width:100px;display:none;" class="btn btn-success" type="button" id="BtnRecordShowNone"><i class="icon-remove-circle bigger-110"></i>Cap</button>
                                                <!--/div>
                                            </!--div-->
                                            <!--/td>
                                        </tr>
                                        <tr>
                                            <td style="width:250px;">
                                                <div class="row">
                                                    <label id="TxtDateToLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_Incident_List_Filter_To"] %></label>										
                                                    <div class="col-xs-9 col-sm-9 tooltip-info" id="TxtDateToDiv">
                                                        <div class="input-group">
                                                            <input class="form-control date-picker" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                            <span id="TxtDateToBtn" class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();">
                                                                <i class="icon-calendar bigger-110"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="width:100px;">&nbsp;</td>
                                            <td style="width:100px;padding-left:20px;"><strong><%=this.Dictionary["Item_IncidentAction_Header_Origin"] %>:</strong></td>
                                            <td><input type="radio" id="ROrigin0" name="ROrigin" onclick="ShowCombos(0);" checked="checked" /><%=this.Dictionary["Item_Incident_Origin0"] %></td>
                                            <td><input type="radio" id="ROrigin1" name="ROrigin" onclick="ShowCombos(1);" /><%=this.Dictionary["Item_Incident_Origin1"] %></td>
                                            <td><input type="radio" id="ROrigin2" name="ROrigin" onclick="ShowCombos(2);" /><%=this.Dictionary["Item_Incident_Origin2"] %></td>
                                            <td><input type="radio" id="ROrigin3" name="ROrigin" onclick="ShowCombos(3);" /><%=this.Dictionary["Item_Incident_Origin3"] %></td>
                                            <td>
                                                <select style="width:200px;display:none;" id="CmbOrigin1"></select>
                                                <select style="width:200px;display:none;" id="CmbOrigin2"></select>
                                                <select style="width:200px;display:none;" id="CmbOrigin3"></select>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--div class="row">
                                        <div class="col-sm-9"></div>                                        
                                        <div class="col-sm-3" style="text-align:right">
                                            <button class="btn btn-success" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><//%=this.Dictionary["Common_Filter"] %></button>
                                            <button class="btn btn-success" type="button" id="BtnRecordShowAll"><i class="icon-list bigger-110"></i>Tots</button>
                                            <button class="btn btn-success" type="button" id="BtnRecordShowNone" style="display: none;"><i class="icon-remove-circle bigger-110"></i>Cap</button>
                                        </div>
                                    </div-->                                
                                <!--/div> 

                                <!-- GTK CLG End -->



                                <div style="height:8px;clear:both;"></div>
                                <div class="row" style="margin-top:20px;">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <!--<th onclick="Sort(this,'ItemTableData','text',true);" id="th1" class="sort" style="width:90px;"><%=this.Dictionary["Item_Incident_Header_Number"] %></th>-->
                                                        <th style="width:60px;"><%=this.Dictionary["Item_Incident_Header_Status"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','date',false);" id="th1" class="sort search"  style="width:100px; text-align:center"><%=this.Dictionary["Item_Incident_Header_Open"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','text',false);" id="th0" class="sort search"><%=this.Dictionary["Item_Incident_Header_Description"] %></th>
														<th style="width:250px;" onclick="Sort(this,'ListDataTable','text',false);" id="th3" class="sort search" ><%=this.Dictionary["Item_Incident_Header_Origin"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','text',false);" id="th4" class="sort hidden-480 search" style="width:90px; text-align:center"><%=this.Dictionary["Item_Incident_Header_ActionNumber"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','money',false);" id="th5" class="sort hidden-480 search" style="width:100px; text-align:center"><%=this.Dictionary["Item_Incident_Header_Cost"] %></th>
                                                        <!-- <th onclick="Sort(this,'ListDataTable','date',false);" id="th6" class="sort hidden-480 search" style="width:100px !important; text-align:center"><%=this.Dictionary["Item_Incident_Header_Close"] %></th> IKEA 090318-->
                                                        <th class="hidden-480" style="width:107px !important;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable"></tbody>
                                                </table>
                                                <table id="ItemTableError" style="display:none;width:100%;">
                                                    <tr>
                                                        <td colspan="10" align="center" style="color:#aa0000;">
                                                            <table style="border:none;width:100%;">
                                                                <tr>
                                                                    <td rowspan="2" style="border:none;text-align:right;"><i class="icon-warning-sign" style="font-size:48px;"></i></td>        
                                                                    <td style="border:none;">
                                                                        <h4><%=this.Dictionary["Item_Incident_FilterError_Title"] %></h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border:none;">
                                                                        <ul>
                                                                            <li id="ErrorDateFrom"><%=this.Dictionary["Commo_Error_FilterDateFrom"]%></li>
                                                                            <li id="ErrorDateTo"><%=this.Dictionary["Commo_Error_FilterDateTo"]%></li>
                                                                            <li id="ErrorDate"><%=this.Dictionary["Item_Incident_FilterError_Date"]%></li>
                                                                            <li id="ErrorStatus"><%=this.Dictionary["Item_Incident_FilterError_Status"]%></li>
                                                                            <li id="ErrorOrigin"><%=this.Dictionary["Item_Incident_FilterError_Origin"] %></li>
                                                                            <li id="ErrorDepartment"><%=this.Dictionary["Item_Incident_FilterError_Department"] %></li>
                                                                            <li id="ErrorProvider"><%=this.Dictionary["Item_Incident_FilterError_Provider"] %></li>
                                                                            <li id="ErrorCustomer"><%=this.Dictionary["Item_Incident_FilterError_Customer"] %></li>
                                                                        </ul>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="ItemTableVoid" style="display:none;width:100%;">
                                                    <tr>
                                                        <td colspan="10" align="center" style="color:#0000aa;">
                                                            <table style="border:none;width:100%;">
                                                                <tr>
                                                                    <td rowspan="2" style="border:none;text-align:right;"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                    <td style="border:none;">
                                                                        <h4><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>                                            
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
														<th>
															<i>
																<%=this.Dictionary["Common_RegisterCount"] %>:
																&nbsp;
																<span id="NumberCosts"></span>
															</i>
														</th>
                                                        <th style="width:90px; font-weight:bold; text-align:right"><%=this.Dictionary["Common_Total"] %></th>
                                                        <th style="width:100px;"><div id="TotalCosts" style="width:100%;text-align:right;font-weight:bold;"></div></th>
                                                        <th style="width:107px;"></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->						
                            </!--div><!-- /.col -->
                            <div id="IncidentDeleteDialog" class="hide" style="width:600px;">
                                <p><%=this.Dictionary["Item_Incident_PopupDelete_Message"] %>&nbsp;<strong><span id="IncidentDeleteName"></span></strong>?</p>
                                <div class="alert alert-danger"><%=this.Dictionary["Item_Incident_PopupDelete_Message_Actions"] %></div>
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
        <script type="text/javascript" src="/js/IncidentList.js?<%=this.AntiCache %>"></script>
</asp:Content>

