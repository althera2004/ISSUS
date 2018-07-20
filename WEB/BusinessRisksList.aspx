<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="BusinessRisksList.aspx.cs" Inherits="BusinessRisksList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" href="/css/nv.d3.css" />
    <style>
        .ui-datepicker {z-index:1000!important;}
         
        .bar {
          fill: steelblue;
        }
 
        .bar:hover {
          fill: brown;
        }
 
        .axis {
          font: 10px sans-serif;
        }
 
        .axis path,
        .axis line {
          fill: none;
          stroke: #000;
          shape-rendering: crispEdges;
        }
 
        .x.axis path {
          display: none;
        }

        #DivChangeRule .btn{width:25px!important;text-align:center;padding:0;}

        .iprButton {width:100%;margin:0;padding:0;}

        .steps {
            border: 1px solid transparent; /*follows #slider2 style for sizing purposes */
            position: relative;
            height: 30px;
            margin-bottom:-5px;
            font-size:11px;
            }
        .tick {
            border: 1px solid transparent; /*follows slide handle style for sizing purposes*/
            position: absolute;
            width: 1.2em;
            margin-left: -.6em;
            text-align:center;
            left: 0;
            cursor:pointer;
            }

        .mynvtooltip{
            background-color:#eee;
            border:1px solid #777;
        }

        #scrollTableDiv,#scrollTableDivOportunity{
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

        #BtnNewItem, BtnNewOportunity {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var layout = <%=this.Layout %>;
        var companyId = <%=this.Company.Id%>;
        var Filter = <%=this.FilterBusinessRisk %>;
        var FilterOportunity = <%=this.FilterOportunity %>;
        var BussinessRiskList = [];
        var OportunityList = [];
        var BusinessRiskGraph = <%=this.RiskJson%>;
        var OportunityGraph = [];
        var CompanyRules = <%=RulesJson%>;
        var FilterType = 1;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="col-sm-12" id="widthTest">
                                    
                                    
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_BusinessRisksAndOportunities_Filter_Periode1"] %>:</strong></td>
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
                                            <td id="TxtDateToLabel"><%=this.Dictionary["Item_BusinessRisksAndOportunities_Filter_Periode2"] %></td>
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

                                            <td><strong><%=this.Dictionary["Item_BusinessRisk_List_Filter_Rules"] %>:</strong></td>
                                            <td>
                                                <select id="CmbRules" class="col-sm-12">
                                                    <asp:Literal runat="server" ID="LtCmbRulesOptions"></asp:Literal>
                                                </select>
                                            </td>

                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                            <td><strong><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type"] %>:</strong></td>
                                            <td>
                                                <select id="CmbType" class="col-sm-12">
                                                    <option value="0"><%=this.Dictionary["Common_All_Male_Plural"] %></option>
                                                    <option value="1"><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type_Assumed"] %></option>
                                                    <option value="2"><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type_Significant"] %></option>
                                                    <option value="3"><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type_NotSignificant"] %></option>
                                                    <option value="4"><%=this.Dictionary["Item_BusinessRisk_Status_Unevaluated"] %></option>
                                                </select>
                                            </td>
                                            
                                        </tr>

                                        <tr>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                            <td><strong><%=this.Dictionary["Item_BusinessRisk_List_Filter_Process"] %>:</strong></td>
                                            <td>
                                                <select id="CmbProcess" class="col-sm-12">
                                                    <asp:Literal runat="server" ID="LtCmbProcessOptions"></asp:Literal>
                                                </select>
                                            </td>

                                        </tr>
                                    </table>



                                    <!-- GTK CLG Start -->

                                    <!--table style="width:100%;">
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
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td><%=this.Dictionary["Item_BusinessRisk_List_Filter_Rules"] %>:</td>
                                            <td>
                                                <select id="CmbRules" class="col-sm-10">
                                                    <asp:Literal runat="server" ID="LtCmbRulesOptions_CLG"></asp:Literal>
                                                </select>
                                            </td>
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type"] %>:</td>
                                            <td style="width:180px;">
                                                <select id="CmbType" class="col-sm-10">
                                                    <option value="0"><%=this.Dictionary["Common_All_Male_Plural"] %></option>
                                                    <option value="1"><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type_Assumed"] %></option>
                                                    <option value="2"><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type_Significant"] %></option>
                                                    <option value="3"><%=this.Dictionary["Item_BusinessRisk_List_Filter_Type_NotSignificant"] %></option>
                                                    <option value="4"><%=this.Dictionary["Item_BusinessRisk_Status_Unevaluated"] %></option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 250px;">
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
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td><%=this.Dictionary["Item_BusinessRisk_List_Filter_Process"] %>:</td>
                                            <td>
                                                <select id="CmbProcess" class="col-sm-10">
                                                    <asp:Literal runat="server" ID="LtCmbProcessOptions_CLG"></asp:Literal>
                                                </select>
                                            </td>
                                             
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td style="width:180px;">
                                                <div class="col-sm-3" style="text-align:right;display:none;">
                                            <button class="btn btn-success" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Common_Filter"] %></button>                                        
                                        </div>
                                        <div class="col-sm-2" style="text-align:right;">&nbsp;</div>
                                        <div class="col-sm-3" style="text-align:right;">                                        
                                            <button style="width:100px;display:none;" class="btn btn-success" type="button" id="BtnRecordShowAll"><i class="icon-list bigger-110"></i><%=this.Dictionary["Common_All_Male_Plural"] %></button>
                                            <button style="width:100px;display:none;" class="btn btn-success" type="button" id="BtnRecordShowNone"><i class="icon-remove-circle bigger-110"></i><%=this.Dictionary["Common_None_Male"] %></button>
                                        </div>
                                            </td> 
                                        </tr>
                                    </table-->

                                    <!-- GTK CLG End -->


                                </div> 
                                
                                <div class="col-sm-12" id="widthTestOportunity" style="display:none;">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:250px;">
                                                <div class="row">
                                                    <label id="TxtOportunityDateFromLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_Incident_List_Filter_From"] %></label>
                                                    <div class="col-xs-9 col-sm-9 tooltip-info" id="TxtOportunityDateFromDiv">
                                                        <div class="input-group">
                                                            <input class="form-control date-picker" id="TxtOportunityDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                            <span id="TxtOportunityDateFromBtn" class="input-group-addon" onclick="document.getElementById('TxtOportunityDateFrom').focus();">
                                                                <i class="icon-calendar bigger-110"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td><%=this.Dictionary["Item_BusinessRisk_List_Filter_Rules"] %>:</td>
                                            <td>
                                                <select id="OportunityCmbRules" class="col-sm-10">
                                                    <asp:Literal runat="server" ID="LtCmbRulesOportunityOptions"></asp:Literal>
                                                </select>
                                            </td>
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td style="width:180px;"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 250px;">
                                                <div class="row">
                                                    <label id="TxtOportunityDateToLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_Incident_List_Filter_To"] %></label>
                                                    <div class="col-xs-9 col-sm-9 tooltip-info" id="TxtOportunityDateToDiv">
                                                        <div class="input-group">
                                                            <input class="form-control date-picker" id="TxtOportunityDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                            <span id="TxtOportunityDateToBtn" class="input-group-addon" onclick="document.getElementById('TxtOportunityDateTo').focus();">
                                                                <i class="icon-calendar bigger-110"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td><%=this.Dictionary["Item_BusinessRisk_List_Filter_Process"] %>:</td>
                                            <td>
                                                <select id="OportunityCmbProcess" class="col-sm-10">
                                                    <asp:Literal runat="server" ID="LtCmbProcessOportunityOptions"></asp:Literal>
                                                </select>
                                            </td>
                                             
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td style="width: 25px;">&nbsp;</td>
                                            <td style="width:180px;">
                                                <div class="col-sm-3" style="text-align:right;display:none;">
                                            <button class="btn btn-success" type="button" id="BtnSearchOportunity"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Common_Filter"] %></button>                                        
                                        </div>
                                        <div class="col-sm-2" style="text-align:right;">&nbsp;</div>
                                        <div class="col-sm-3" style="text-align:right;">                                        
                                            <button style="width:100px;display:none;" class="btn btn-success" type="button" id="BtnRecordShowAllOportunity"><i class="icon-list bigger-110"></i><%=this.Dictionary["Common_All_Male_Plural"] %></button>
                                        </div>
                                            </td> 
                                        </tr>
                                    </table>
                                </div> 





                                <div style="height:8px;clear:both;"></div>
                                <div class="tabbable">
                                    <ul class="nav nav-tabs padding-18">
                                        <li class="active">
                                            <a data-toggle="tab" href="#list" id="tabbasic"><%=this.Dictionary["Item_BusinessRisk_Title_List"] %></a>
                                        </li>
                                        <li class="">
                                            <a data-toggle="tab" href="#graphic" id="tabgraficos"><%=this.Dictionary["Item_BusinessRisk_Title_Graph"] %></a>
                                        </li>
                                        <li class="">
                                            <a data-toggle="tab" href="#oportunity" id="taboportunity" style="display:none;"><%=this.Dictionary["Item_Oportunities"] %></a>
                                        </li>
                                        <li class="">
                                            <a data-toggle="tab" href="#graphicoportunity" id="tabgraficosoportunity" style="display:none;"><%=this.Dictionary["Item_Oportunity_Title_Graph"] %></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content no-border padding-24">
                                        <div id="list" class="tab-pane active">  
                                            <div class="row">
                                                <div class="col-xs-12">
                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
		                                                        <tr id="ListDataHeader">
                                                                    <th style="width:60px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Status"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable','date');" id="th1" class="sort search" style="width:90px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Date"] %></th>
                                                                    <th onclick="Sort(this,'ListDataTable','text');" id="th2" class="search sort"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Description"] %></th>
                                                                    <th onclick="Sort(this,'ListDataTable','text');" id="th3" class="hidden-480 search sort" style="width:200px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Process"] %></th>
																	<th onclick="Sort(this,'ListDataTable','text');" id="th4" class="hidden-480 search sort" style="width:200px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Rule"] %></th>
																	<th onclick="Sort(this,'ListDataTable','money');" id="th5" class="hidden-480 search sort" style="width:90px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_StartValue"] %></th>
																	<th onclick="Sort(this,'ListDataTable','money');" id="th6" class="hidden-480 search sort" style="width:80px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_IPR"] %></th>
																	<th style="width:107px;">&nbsp;</th>
		                                                        </tr>
	                                                        </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTable"><asp:Literal runat="server" ID="BusinessRiskData"></asp:Literal></tbody>
                                                            </table>
                                                            <table id="ItemTableVoid" style="display:none;width:100%">
                                                                <tr>
                                                                    <td colspan="10" align="center" style="color:#0000aa;">
                                                                        <table style="border:none;width:100%">
                                                                            <tr>
                                                                                <td rowspan="2" style="border:none;text-align:right"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                                <td style="border:none;">
                                                                                    <h4><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0;">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="NumberCosts"></span></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div><!-- /.table-responsive -->
                                                </div><!-- /span -->
                                            </div><!-- /row -->								
                                        </div><!-- /.col -->
                                        <div id="graphic" class="tab-pane">                                            
                                            <div class="col-sm-12">
                                                <div id="DivChangeRule"><%=this.Dictionary["Item_Rule"] %>:&nbsp;<span id="RuleDescriptionBusinessRisk"></span></div>
                                            </div>
                                            <div class="row" id="BtnChangeIpr" style="display:none;">
                                                <div class="col-sm-10">
                                                    <div class="steps" id="steps"></div>
                                                    <div class="help-block ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" id="input-span-slider">                                                        
                                                        <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 9.09091%;"></span>
                                                    </div>
                                                </div>
                                                <div class="col-sm-2">
                                                    <input id="BtnNewIpr" type="button" value="<%=this.Dictionary["Item_BusinessRisk_Btn_NewIPR"] %>" onclick="NewIpr()" />											            
                                                </div>
                                            </div>
                                            <div class="cols-sm-12">
                                                <div id="chartBusinessRisk" style="width:100%;">
                                                    <svg style='height:420px;width:100%' id="svggraficBusinessRisk"></svg>
                                                    <table id="GraphicTableVoid" style="height:500px;width:100%;display:none;">
                                                        <tr>
                                                            <td colspan="10" align="center" style="background-color:#ddddff;color:#0000aa;">
                                                                <table style="border:none;">
                                                                    <tr>
                                                                        <td rowspan="2" style="border:none;"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                        <td style="border:none;">
                                                                            <h4><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="oportunity" class="tab-pane">                                            
                                                    <div class="table-responsive" id="scrollTableDivOportunity">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
		                                                        <tr id="ListDataHeaderOportunity">
                                                                    <th style="width:60px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Status"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTableOportunity','date');" id="th1" class="sort search" style="width:90px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Date"] %></th>
                                                                    <th onclick="Sort(this,'ListDataTableOportunity', 'text');" id="th2" class="search sort"><%=this.Dictionary["Item_Oportunity"] %></th>
                                                                    <th onclick="Sort(this,'ListDataTableOportunity', 'text');" id="th3" class="hidden-480 search sort" style="width:200px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Process"] %></th>
																	<th onclick="Sort(this,'ListDataTableOportunity', 'text');" id="th4" class="hidden-480 search sort" style="width:200px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_Rule"] %></th>
																	<th onclick="Sort(this,'ListDataTableOportunity', 'money');" id="th5" class="hidden-480 search sort" style="width:90px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_StartValue"] %></th>
																	<th onclick="Sort(this,'ListDataTableOportunity', 'money');" id="th6" class="hidden-480 search sort" style="width:80px;"><%=this.Dictionary["Item_BusinesRisk_ListHeader_IPR"] %></th>
																	<th style="width:107px;">&nbsp;</th>
		                                                        </tr>
	                                                        </thead>
                                                        </table>
                                                        <div id="ListDataDivOportunity" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTableOportunity"><asp:Literal runat="server" ID="Literal1"></asp:Literal></tbody>
                                                            </table>
                                                            <table id="ItemTableVoidOportunity" style="display:none;width:100%">
                                                                <tr>
                                                                    <td colspan="10" align="center" style="color:#0000aa;">
                                                                        <table style="border:none;width:100%">
                                                                            <tr>
                                                                                <td rowspan="2" style="border:none;text-align:right"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                                <td style="border:none;">
                                                                                    <h4><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0;">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooterOportunity">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="NumberCostsOportunity"></span></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div><!-- /.table-responsive -->
                                                
                                        </div>
                                        <div id="graphicoportunity" class="tab-pane">                                            
                                            <div class="col-sm-12">
                                                <div id="DivChangeRuleOportunity"><%=this.Dictionary["Item_Rule"] %>:&nbsp;<span id="RuleDescriptionOportunity"></span></div>
                                            </div>
                                            <div class="row" id="BtnChangeIprOportunity" style="display:none;">
                                                <div class="col-sm-10">
                                                    <div class="steps" id="stepsoportunity"></div>
                                                    <div class="help-block ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" id="input-span-slideroportunity">                                                        
                                                        <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 9.09091%;"></span>
                                                    </div>
                                                </div>
                                                <div class="col-sm-2">
                                                    <input id="BtnNewIproportunity" type="button" value="<%=this.Dictionary["Item_Oportunity_Btn_NewIPR"] %>" onclick="NewIproportunity()" />											            
                                                </div>
                                            </div>
                                            <div class="cols-sm-12">
                                                <div id="chartOportunity" style="width:100%;">
                                                    <svg style='height:420px;width:100%' id="svggraficoportunity"></svg>
                                                    <table id="GraphicTableVoidOportunity" style="height:500px;width:100%;display:none;">
                                                        <tr>
                                                            <td colspan="10" align="center" style="background-color:#ddddff;color:#0000aa;">
                                                                <table style="border:none;">
                                                                    <tr>
                                                                        <td rowspan="2" style="border:none;"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                        <td style="border:none;">
                                                                            <h4><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
    
                                                        <div id="OportunityDeleteDialog" class="hide" style="width:500px;">
                                                            <p><%=this.Dictionary["Item_Oportunity_PopupDelete_Message"] %>&nbsp;<strong><span id="BusinessRiskName"></span></strong>?</p>
                                                        </div>
    
                                                        <div id="BusinessRiskDeleteDialog" class="hide" style="width:500px;">
                                                            <p><%=this.Dictionary["Item_BusinessRisk_PopupDelete_Message"] %>&nbsp;<strong><span id="BusinessRiskName"></span></strong>?</p>
                                                        </div>
                                                        <div id="BusinessRiskUpdateDialog" class="hide" style="width:500px;">
                                                            <p><span id="TxtBusinessRiskNameLabel"><%=this.Dictionary["Common_Name"] %></span>&nbsp;<input type="text" id="TxtBusinessRiskName" /></p>
                                                            <span class="ErrorMessage" id="TxtBusinessRiskNameErrorRequired" style="display:none;"> <%=this.Dictionary["Common_Required"] %></span>
                                                            <span class="ErrorMessage" id="TxtBusinessRiskNameErrorDuplicated" style="display:none;"> <%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                                                        </div>
                                                        <div id="BusinessRiskInsertDialog" class="hide" style="width:500px;">
                                                            <p><span id="TxtBusinessRiskNewNameLabel"><%=this.Dictionary["Common_Name"] %></span>&nbsp;<input type="text" id="TxtBusinessRiskNewName" /></p>
                                                            <span class="ErrorMessage" id="TxtBusinessRiskNewNameErrorRequired" style="display:none;"> <%=this.Dictionary["Common_Required"] %></span>
                                                            <span class="ErrorMessage" id="TxtBusinessRiskNewNameErrorDuplicated" style="display:none;"> <%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                                                        </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server"> 
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="/js/common.js"></script>
        <script type="text/javascript" src="/js/BusinessRiskList.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="//d3js.org/d3.v3.min.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/nv.d3.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/BusinessRiskChart.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/OportunityChart.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript">            
            var currentMousePos = { x: -1, y: -1 };
            jQuery(function ($) {
                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || "&nbsp;";
                        if (("title_html" in this.options) && this.options.title_html === true) {
                            title.html($title);
                        }
                        else {
                            title.text($title);
                        }
                    }
                }));

                var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { autoclose: true, todayHighlight: true });
                $(".date-picker").datepicker(options);
                $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

                $("#BtnNewBusinessRisk").on("click", function (e) {
                    document.location = "BusinessRiskView.aspx?id=-1";
                    return false;
                });

                $("#BtnNewOportunity").on("click", function (e) {
                    document.location = "OportunityView.aspx?id=-1";
                    return false;
                });

                $("#BtnSearchOportunity").on("click", function (e) {
                    e.preventDefault();
                    OportunityGetFilter();
                });

                $("#BtnSearch").on("click", function (e) {
                    e.preventDefault();
                    BusinessRiskGetFilter();
                });

                $("#BtnRecordShowAll").on("click", function (e) {
                    e.preventDefault();
                    BussinesRiskListGetAll();
                });

                $("#BtnRecordShowAllOportunity").on("click", function (e) {
                    e.preventDefault();
                    OportunityListGetAll();
                });

                $("#BtnRecordShowNone").on("click", function (e) {
                    e.preventDefault();
                    BusinessRiskListGetNone();
                });
                
                $("#tabgraficos").on("click", BusinessRiskGetFilter);
                $("#tabbasic").on("click", BusinessRiskGetFilter);
                $("#tabgraficosoportunity").on("click", OportunityGetFilter);
                $("#taboportunity").on("click", OportunityGetFilter);
            });
            
            $("#input-span-slider").slider({
                "value": RuleLimitFromDBBusinessRisk,
                "range": "min",
                "min": 1,
                "max": 25,
                "step": 1,
                "slide": function (event, ui) {
                    var val = parseInt(ui.value);
                    SetRule(val);
                }
            });
            
            $("#input-span-sliderOportunity").slider({
                "value": RuleLimitFromDBOportunity,
                "range": "min",
                "min": 1,
                "max": 25,
                "step": 1,
                "slide": function (event, ui) {
                    var val = parseInt(ui.value);
                    SetRuleOportunity(val);
                }
            });

            $(document).mousemove(function(event) {
                currentMousePos.x = event.pageX;
                currentMousePos.y = event.pageY;
                var position = $("#svggraficBusinessRisk").offset();
                $(".xy-tooltip").css({top: currentMousePos.y - position.top - 30, left: currentMousePos.x - position.left + 10});
            });
        </script>
</asp:Content>