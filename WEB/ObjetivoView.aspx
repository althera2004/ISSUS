<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ObjetivoView.aspx.cs" Inherits="ObjetivoView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" type="text/css" href="/assets/css/jquery.dynameter.css" />
    <style type="text/css">
        .iconfile {
            border: 1px solid #777;
            background-color: #fdfdfd;
            -webkit-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            -moz-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            padding-left:0!important;
            padding-top:4px !important;
            padding-bottom:4px !important;
            margin-bottom:12px !important;
        }
        #scrollTableDiv, #scrollTableDivHistorico, #scrollTableDivActions{
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
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
    <script type="text/javascript">
        var ItemData = <%= this.ItemData %>;
        var OriginalItemData = <%=this.ItemData %>;
        var Objetivos = <%= this.Objetivos %>;
        var Registros = <%=this.Registros %>;
        var Actions = <%=this.ActionsList %>;
        var Employees = <%= this.Employees %>;
        var IndicadorObjetivo = <%=this.IndicadoresObjetivo %>;
        var IndicadorName = "<%=this.IndicadorName %>";
        var Periodicidades = <%=this.Periodicities %>;
        var orderList = "";
        var ActionsOpen = false;
        var Historic = <%=this.Historic %>;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <%=this.TabBar %>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="home" class="tab-pane active">       
                                                    <div class="row">
                                                        <form class="form-horizontal" role="form">
                                                            <div class="col-sm-12">
                                                                <div class="form-group">
                                                                    <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Objetivo_FieldLabel_Name"] %><span style="color:#f00">*</span></label>
                                                                    <div class="col-sm-11">                                                                                                                            
                                                                        <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Objetivo_FieldLabel_Name"] %>" class="col-xs-12 col-sm-12 tooltip-info" value="" maxlength="100" onblur="this.value=$.trim(this.value);" />
                                                                        <span class="ErrorMessage" id="TxtNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                        <span class="ErrorMessage" id="TxtNameErrorDuplicated"><%=this.Dictionary["Common_AlreadyExists"] %></span>
                                                                    </div>
                                                                </div>          
                                                                <div style="height:12px;clear:both;">&nbsp;</div>  
                                                                <div class="form-group">
                                                                    <label class="col-sm-1 control-label no-padding-right" id="TxtDescriptionLabel"><%= this.Dictionary["Item_Objetivo_FieldLabel_Description"] %><span style="color:#f00">*</span></label>
                                                                    <div class="col-sm-11">
                                                                        <textarea rows="3" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtDescription"></textarea>
                                                                        <span class="ErrorMessage" id="TxtDescriptionErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                    </div>
                                                                    <div class="col-sm-11">&nbsp;</div>
                                                                </div>                                                           
                                                                <div class="form-group">
                                                                    <%=this.CmbResponsible.Render %>
                                                                    <label id="TxtFechaAltaLabel" class="col-sm-1 control-label no-padding-right"><%= this.Dictionary["Item_Objetivo_FieldLabel_DateStart"] %><span style="color:#f00">*</span></label>
                                                                    <div class="col-sm-2">
                                                                        <div class="row">
                                                                            <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtFechaAltaDiv">
                                                                                <div class="input-group">
                                                                                    <input class="form-control date-picker" id="TxtFechaAlta" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                                                    <span id="TxtFechaAltaBtn" class="input-group-addon" onclick="document.getElementById('TxtFechaAlta').focus();">
                                                                                        <i class="icon-calendar bigger-110"></i>
                                                                                    </span>
                                                                                </div>
                                                                                <span class="ErrorMessage" id="TxtFechaAltaErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                                <span class="ErrorMessage" id="TxtFechaAltaDateMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <label id="TxtFechaCierrePrevistaLabel" class="col-sm-2 control-label no-padding-right"><%= this.Dictionary["Item_Objetivo_FieldLabel_ClosePreviewDate"] %><span style="color:#f00">*</span></label>
                                                                    <div class="col-sm-2">
                                                                        <div class="row">
                                                                            <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtFechaCierrePrevistaDiv">
                                                                                <div class="input-group">
                                                                                    <input class="form-control date-picker" id="TxtFechaCierrePrevista" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                                                                    <span id="TxtFechaCierrePrevistaBtn" class="input-group-addon" onclick="document.getElementById('TxtFechaCierrePrevista').focus();">
                                                                                        <i class="icon-calendar bigger-110"></i>
                                                                                    </span>
                                                                                </div>
                                                                                    <span class="ErrorMessage" id="TxtFechaCierrePrevistaErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                                    <span class="ErrorMessage" id="TxtFechaCierrePrevistaDateMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                                    <span class="ErrorMessage" id="TxtFechaCierrePrevistaCrossDate"><%= this.Dictionary["Item_Objetivo_Error_PreviewEndDate"] %></span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div style="height:12px;clear:both;">&nbsp;</div>
                                                            <div class="for-group">
                                                                <label id="TxtVinculatedToIndicatorLabel" class="col-sm-1 control-label no-padding-right" style="padding: 0;"><%=this.Dictionary["Item_Objetivo_FieldLabel_Viculated"] %></label>
                                                                <div class="col-sm-1"><input runat="server" type="radio" id="RVinculatedYes" name="status" value="0" />&nbsp;<%=this.Dictionary["Common_Yes"] %></div>
                                                                <div class="col-sm-2"><input runat="server" type="radio" id="RVinculatedNo" name="status" value="1" />&nbsp;<%=this.Dictionary["Common_No"] %></div>
                                                                <%=this.CmbIndicador.Render %>
                                                                <label id="CmbMetaLabel" class="col-sm-1 control-label no-padding-right" style="display: none;">Meta</label>
                                                                <div class="col-sm-4" id="DivCmbMetaComparer" style="height: 35px !important; display: none;" title="" data-rel="tooltip">
                                                                    <select style="float: left; width: 55%;" class="form-control" id="CmbMetaComparer" data-placeholder="">
                                                                        <option value="">Seleccionar</option>
                                                                        <option value="eq"><%=this.Dictionary["Common_Comparer_eq"] %> (=)</option>
                                                                        <option value="gt"><%=this.Dictionary["Common_Comparer_gt"] %> (&gt;)</option>
                                                                        <option value="eqgt"><%=this.Dictionary["Common_Comparer_eqgt"] %> (=&gt;)</option>
                                                                        <option value="lt"><%=this.Dictionary["Common_Comparer_lt"] %> (&lt;)</option>
                                                                        <option value="eqlt"><%=this.Dictionary["Common_Comparer_eqlt"] %> (&lt;=)</option>
                                                                    </select>
                                                                    &nbsp;
                                                                    <input style="display: inline; width: 33%; height: 30px;" type="text" id="TxtMeta" placeholder="Meta" class="tooltip-info decimalFormated nullable" value="0" maxlength="14" onblur="this.value=$.trim(this.value);" />
                                                                    <span class="ErrorMessage" id="CmbMetaErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                                <label id="CmbObjetivoBlocked" class="col-sm-4 control-label no-padding-right" style="display: none; text-align: left; color: #777;"><i><%=this.Dictionary["Item_Objetivo_Message_IndicadorBlocked"] %></i></label>
                                                                <label id="TxtPeriodicityLabel" class="col-sm-1 control-label no-padding-right"><%= this.Dictionary["Item_Objetivo_FieldLabel_Periodicity"] %><span style="color: #f00">*</span></label>
                                                                <div class="col-sm-1">
                                                                    <input type="text" id="TxtPeriodicity" placeholder="Periodicitat" class="col-xs-12 col-sm-12 tooltip-info integerFormated" value="0" maxlength="3" onblur="this.value=$.trim(this.value);" />
                                                                    <span class="ErrorMessage" id="TxtPeriodicityErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                </div>
																<label id="Label1" class="col-sm-1"><%=this.Dictionary["Common_Label_Days"] %></label>
															</div>
                                                            <div style="height:12px;clear:both;">&nbsp;</div>
                                                            <div class="form-group">
                                                                <label class="col-sm-12" id="TxtMetodologiaLabel"><%= this.Dictionary["Item_Objetivo_FieldLabel_Methodology"] %></label>
                                                                <div class="col-sm-12">
                                                                    <textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtMetodologia"><%=this.Objetivo.Methodology %></textarea>
                                                                    <span class="ErrorMessage" id="TxtMetodologiaRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                                <!--div class="col-sm-11">&nbsp;</div-->
                                                            </div>
                                                            <div style="height:12px;clear:both;">&nbsp;</div>                                                            
                                                            <div class="form-group">
                                                                <label class="col-sm-12" id="TxtRecursosLabel"><%= this.Dictionary["Item_Objetivo_FieldLabel_Resources"] %></label>
                                                                <div class="col-sm-12">
                                                                    <textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtRecursos"><%=this.Objetivo.Resources %></textarea>
                                                                    <span class="ErrorMessage" id="TxtRecursosRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                                <!--div class="col-sm-11">&nbsp;</div-->
                                                            </div>
                                                            <div style="height:12px;clear:both;">&nbsp;</div>                                                           
                                                            <div class="form-group">
                                                                <label class="col-sm-12" id="TxtNotesLabel"><%= this.Dictionary["Item_Objetivo_FieldLabel_Notes"] %></label>
                                                                <div class="col-sm-12">
                                                                    <textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtNotes"></textarea>
                                                                </div>
                                                                <!--div class="col-sm-11">&nbsp;</div-->
                                                            </div>
                                                        </form>
                                                    </div>
                                                    <div class="row">
                                                        <%=this.FormFooter %>
                                                    </div>
                                                </div>

                                                <div id="actions" class="tab-pane">
                                                    <!--h4 id="ActionsListTitle"><%=this.Dictionary["Item_Objetivo_TabActions"] %></h4-->
                                                    
                                                    <div class="row">
                                                    <div class="col-sm-12">
                                    
                                                        <table cellpadding="2" cellspacing="2">
                                                            <tr>
                                                                <td><h4 id="ActionsListTitle"><%=this.Dictionary["Item_Objetivo_TabActions"] %></h4></td>

                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                                                <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Periode1"] %>:</strong></td>
										                        <td>
                                                                    <div class="col-xs-12 col-sm-12">
												                        <div class="input-group">
													                        <input class="form-control date-picker" style="width:100px;" id="TxtActionsFromDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                        <span class="input-group-addon" onclick="document.getElementById('TxtActionsFromDate').focus();" id="TxtActionsFromDateBtn">
														                        <i class="icon-calendar bigger-110"></i>
													                        </span>
												                        </div>
											                            <span class="ErrorMessage" id="TxtDateFromErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                            <span class="ErrorMessage" id="TxtDateFromErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                            <span class="ErrorMessage" id="TxtDateFromDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                    </div>
										                        </td>
                                                                <td id="TxtDateToLabel"><%=this.Dictionary["Item_IncidentAction_List_Filter_Periode2"] %></td>
										                        <td>
                                                                    <div class="col-xs-12 col-sm-12">
												                        <div class="input-group">
													                        <input class="form-control date-picker" style="width:100px;" id="TxtActionsToDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                        <span class="input-group-addon" onclick="document.getElementById('TxtActionsToDate').focus();" id="TxtActionsToDateBtn">
														                        <i class="icon-calendar bigger-110"></i>
													                        </span>
												                        </div>
											                            <span class="ErrorMessage" id="TxtDateToErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                            <span class="ErrorMessage" id="TxtDateToErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                            <span class="ErrorMessage" id="TxtDateToDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                    </div>
										                        </td>

                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                                                <td style="width:100%; text-align:right;">
                                                                    <button class="btn btn-success" type="button" id="BtnActionsNew"><i class="icon-plus bigger-110"></i><%=this.Dictionary["Item_Indicador_New_Button"] %></button>
                                                                    <button class="btn btn-success" type="button" id="BtnActionsFilter" style="display: none;"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Item_Indicador_Filter_Button"] %></button>
                                                                    <button class="btn btn-success" type="button" id="BtnActionsShowAll" style="display: none;"><i class="icon-list bigger-110"></i><%=this.Dictionary["Common_All_Male_Plural"] %></button>
                                                                    <button class="btn btn-success" type="button" id="BtnActionsShowNone" style="display: none;"><i class="icon-remove-circle bigger-110"></i><%=this.Dictionary["Common_None_Male"] %></button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    </div>

                                                    <div class="table-responsive" id="scrollTableDivActions">
                                                        <table class="table table-bordered table-striped" style="margin-top: 4px; margin-bottom: 0;">
                                                            <thead class="thin-border-bottom" id="RegistrosTHeadActions">
                                                                <tr>
                                                                    <th id="th0" onclick="Sort(this,'ObjetivoActionsTable','text',false);" class="sort" ><%=this.Dictionary["Item_IncidentAction_Header_Description"]%></th>
                                                                    <th id="th1" onclick="Sort(this,'ObjetivoActionsTable','date',false);" class="sort" style="width: 100px;"><%=this.Dictionary["Item_IncidentAction_Header_Open"]%></th>
                                                                    <th id="th2" onclick="Sort(this,'ObjetivoActionsTable','text',false);" class="sort" style="width: 90px;text-align:center;"><%=this.Dictionary["Item_IncidentAction_Header_Status"]%></th>
                                                                    <th id="th3" onclick="Sort(this,'ObjetivoActionsTable','date',false);" class="sort"  style="width: 100px;"><%=this.Dictionary["Item_IncidentAction_Header_ImplementDate"]%></th>
                                                                    <th id="th4" onclick="Sort(this,'ObjetivoActionsTable','money',false);" class="sort" style="width: 150px;text-align:center;"><%=this.Dictionary["Item_IncidentAction_Header_Cost"]%></th>
                                                                    <th style="width: 107px;">&nbsp;</th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDivActions" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ObjetivoActionsTable">
                                                                    <asp:Literal runat="server" ID="LtAccionesData"></asp:Literal>
                                                                </tbody>
                                                                <tfoot id="ItemTableVoidActions" style="display: none; height: 100%;">
                                                                    <tr>
                                                                        <td colspan="10" align="center" style="background-color: #ddddff; color: #0000aa;">
                                                                            <table style="border: none;">
                                                                                <tr>
                                                                                    <td rowspan="2" style="border: none;" class="NoData"><i class="icon-info-sign" style="font-size: 48px;"></i></td>
                                                                                    <td style="border: none;">
                                                                                        <h4 class="NoData" style="font-size: 24px;"><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tfoot>
                                                                <tfoot id="ItemTableErrorActions" style="display: none; height: 100%;">
                                                                    <tr>
                                                                        <td colspan="10" align="center" style="background-color: #ffffdd; color: #aa0000;">
                                                                            <table style="border: none;">
                                                                                <tr>
                                                                                    <td rowspan="2" style="border: none;"><i class="icon-warning-sign" style="font-size: 48px;"></i></td>
                                                                                    <td style="border: none;">
                                                                                        <h4><%=this.Dictionary["Item_EquipmentRecords_FilterError_Title"] %></h4>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="border: none;">
                                                                                        <ul>
                                                                                            <li id="ActionsErrorDate"><%=this.Dictionary["Item_EquipmentRecords_FilterError_Dates"]%></li>
                                                                                            <li id="ActionsErrorDateMalformedFrom"><%=this.Dictionary["Item_EquipmentRecord_Error_FilterDateFromMalformed"]%></li>
                                                                                            <li id="ActionsErrorDateMalformedTo"><%=this.Dictionary["Item_EquipmentRecord_Error_FilterDateToMalformed"]%></li>
                                                                                        </ul>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tfoot>
                                                            </table>
                                                        </div>                                                                                      
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooterActions">
                                                                    <th style="color:#aaa;">
															            <i>
																            <%=this.Dictionary["Common_RegisterCount"] %>:
																            &nbsp;
																            <span id="NumberCostsActions"></span>
															            </i>
														            </th>
                                                                    <th style="color:#aaa;text-align:right;">
															            <i>
																            <%=this.Dictionary["Common_Total"] %>:
															            </i>
														            </th>
                                                                    <th style="color:#aaa;width:150px;text-align:right;">
															            <i>
																            <span id="NumberCostsActionsTotal"></span>
															            </i>
														            </th>
                                                                    <th style="width:107px;">&nbsp;</th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div>
                                                </div>

                                                <div id="records" class="tab-pane">                                                 
                                                    
                                                    <h4 id="RecordListTitle"><%=this.Dictionary["Item_Objetivo_Tab_Records"] %></h4>


                                                    <div class="row">
                                                    <div class="col-sm-12">
                                    
                                                        <table cellpadding="2" cellspacing="2">
                                                            <tr>

                                                                <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_Indicador_Filter_Periode1"] %>:</strong></td>
										                        <td>
                                                                    <div class="col-xs-12 col-sm-12">
												                        <div class="input-group">
													                        <input class="form-control date-picker" style="width:100px;" id="TxtRecordsFromDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                        <span class="input-group-addon" onclick="document.getElementById('TxtRecordsFromDate').focus();" id="TxtRecordsFromDateBtn">
														                        <i class="icon-calendar bigger-110"></i>
													                        </span>
												                        </div>
											                            <span class="ErrorMessage" id="TxtDateFromErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                            <span class="ErrorMessage" id="TxtDateFromErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                            <span class="ErrorMessage" id="TxtDateFromDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                    </div>
										                        </td>

                                                                <td id="TxtDateToLabel"><%=this.Dictionary["Item_Indicador_Filter_Periode2"] %></td>
										                        <td>
                                                                    <div class="col-xs-12 col-sm-12">
												                        <div class="input-group">
													                        <input class="form-control date-picker" style="width:100px;" id="TxtRecordsToDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                        <span class="input-group-addon" onclick="document.getElementById('TxtRecordsToDate').focus();" id="TxtRecordsToDateBtn">
														                        <i class="icon-calendar bigger-110"></i>
													                        </span>
												                        </div>
											                            <span class="ErrorMessage" id="TxtDateToErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                            <span class="ErrorMessage" id="TxtDateToErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                            <span class="ErrorMessage" id="TxtDateToDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                    </div>
										                        </td>

                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                                                <td style="width:100%; text-align:right;">
                                                                    <button class="btn btn-success" type="button" id="BtnRecordNew"><i class="icon-plus bigger-110"></i><%=this.Dictionary["Item_Indicador_New_Button"] %></button>
                                                                    <button class="btn btn-success" type="button" id="BtnRecordFilter" style="display:none;"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Item_Indicador_Filter_Button"] %></button>
                                                                    <button class="btn btn-success" type="button" id="BtnRecordShowAll" style="display:none;"><i class="icon-list bigger-110"></i><%=this.Dictionary["Common_All_Male_Plural"] %></button>
                                                                    <button class="btn btn-success" type="button" id="BtnRecordShowNone" style="display: none;"><i class="icon-remove-circle bigger-110"></i><%=this.Dictionary["Common_None_Male"] %></button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    </div>


                                                    <!--div class="alert alert-info" style="display: none;" id="DivIndicadorRecordsMessage">
                                                        <strong><i class="icon-info-sign"></i></strong>
                                                        <h5 style="display: inline;"><%=this.Dictionary["Item_Objetivo_Tab_RecordsFromIndicatorMessage"].ToString().Replace("##", "<strong>" + this.IndicadorName + "</strong>") %></h5>
                                                    </div>
                                                    <div class="row">
                                                        <label id="TxtRecordsFromDateLabel" class="col-sm-1 control-label no-padding-right" for="TxtNewMaintainmentOperation"><%=this.Dictionary["Common_From"] %></label>
                                                        <div class="col-sm-2">
                                                            <div class="row">
                                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtRecordsFromDateDiv">
                                                                    <div class="input-group">
                                                                        <input class="form-control date-picker" id="TxtRecordsFromDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                                        <span id="TxtRecordsFromDateBtn" class="input-group-addon" onclick="document.getElementById('TxtRecordsFromDate').focus();">
                                                                            <i class="icon-calendar bigger-110"></i>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <label id="TxtRecordsToDateLabel" class="col-sm-1 control-label no-padding-right" for="TxtNewMaintainmentOperation"><%=this.Dictionary["Common_To"] %></label>
                                                        <div class="col-sm-2">
                                                            <div class="row">
                                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtRecordsToDateDiv">
                                                                    <div class="input-group">
                                                                        <input class="form-control date-picker" id="TxtRecordsToDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                                        <span id="TxtRecordsToDateBtn" class="input-group-addon" onclick="document.getElementById('TxtRecordsToDate').focus();">
                                                                            <i class="icon-calendar bigger-110"></i>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6" style="text-align: right;">
                                                            <button class="btn btn-success" type="button" id="BtnRecordNew"><i class="icon-plus bigger-110"></i><%=this.Dictionary["Item_Indicador_New_Button"] %></button>
                                                            <button class="btn btn-success" type="button" id="BtnRecordFilter" style="display:none;"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Item_Indicador_Filter_Button"] %></button>
                                                            <button class="btn btn-success" type="button" id="BtnRecordShowAll"><i class="icon-list bigger-110"></i><%=this.Dictionary["Common_All_Male_Plural"] %></button>
                                                            <button class="btn btn-success" type="button" id="BtnRecordShowNone" style="display: none;"><i class="icon-remove-circle bigger-110"></i><%=this.Dictionary["Common_None_Male"] %></button>
                                                        </div>
                                                    </div-->


                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin-top: 4px; margin-bottom: 0;">
                                                            <thead class="thin-border-bottom" id="RegistrosTHead">
                                                                <tr>
                                                                    <td colspan="8" style="text-align: right;">
                                                                        <span title="<%=this.Dictionary["Common_PrintPdf"] %>" class="btn btn-xs btn-info" onclick="lockOrderList=true;ObjetivoRegistroFilter('PDF');"><i class="icon-file-pdf bigger-120"></i>&nbsp;PDF</span>
                                                                        &nbsp;
                                                                        <span title="<%=this.Dictionary["Common_PrintExcel"] %>" class="btn btn-xs btn-info" onclick="lockOrderList=true;ObjetivoRegistroFilter('Excel');"><i class="icon-file-excel bigger-120"></i>&nbsp;Excel</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th id="th0" style="width: 35px;"></th>
                                                                    <th id="th1" onclick="Sort(this,'ObjetivoRegistrosTable','money',false);" class="sort" style="width: 90px;"><%=this.Dictionary["Item_Indicador_TableRecords_Header_Value"]%></th>
                                                                    <th id="th2" onclick="Sort(this,'ObjetivoRegistrosTable','date',false);" class="sort" style="width: 90px;"><%=this.Dictionary["Item_Indicador_TableRecords_Header_Date"]%></th>
                                                                    <th id="th3"><%=this.Dictionary["Item_Indicador_TableRecords_Header_Comments"]%></th>
                                                                    <th id="th4" style="width: 120px;"><%=this.Dictionary["Item_Indicador_TableRecords_Header_Meta"]%></th>
                                                                    <!--th id="th4" onclick="Sort(this,'ObjetivoRegistrosTable','money',false);" class="sort" style="width: 120px;"><%=this.Dictionary["Item_Indicador_TableRecords_Header_Meta"]%></!--th>
                                                                    <!--<th id="th5" onclick="Sort(this,'ObjetivoRegistrosTable','money',false);" class="sort" style="width: 120px;"><%=this.Dictionary["Item_Indicador_TableRecords_Header_Alarm"]%></th>-->
                                                                    <th id="th5" onclick="Sort(this,'ObjetivoRegistrosTable','text',false);" class="sort" style="width: 175px;"><%=this.Dictionary["Item_Indicador_TableRecords_Header_Responsible"]%></th>
                                                                    <th style="width: 107px;">&nbsp;</th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ObjetivoRegistrosTable">
                                                                    <asp:Literal runat="server" ID="LtRegistrosData"></asp:Literal>
                                                                </tbody>
                                                                <tfoot id="ItemTableVoid" style="display: none; height: 100%;">
                                                                    <tr>
                                                                        <td colspan="10" align="center" style="background-color: #ddddff; color: #0000aa;">
                                                                            <table style="border: none;">
                                                                                <tr>
                                                                                    <td rowspan="2" style="border: none;" class="NoData"><i class="icon-info-sign" style="font-size: 48px;"></i></td>
                                                                                    <td style="border: none;">
                                                                                        <h4 class="NoData" style="font-size: 24px;"><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tfoot>
                                                                <tfoot id="ItemTableError" style="display: none; height: 100%;">
                                                                    <tr>
                                                                        <td colspan="10" align="center" style="background-color: #ffffdd; color: #aa0000;">
                                                                            <table style="border: none;">
                                                                                <tr>
                                                                                    <td rowspan="2" style="border: none;"><i class="icon-warning-sign" style="font-size: 48px;"></i></td>
                                                                                    <td style="border: none;">
                                                                                        <h4><%=this.Dictionary["Item_EquipmentRecords_FilterError_Title"] %></h4>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="border: none;">
                                                                                        <ul>
                                                                                            <li id="ErrorDate"><%=this.Dictionary["Item_EquipmentRecords_FilterError_Dates"]%></li>
                                                                                            <li id="ErrorDateMalformedFrom"><%=this.Dictionary["Item_EquipmentRecord_Error_FilterDateFromMalformed"]%></li>
                                                                                            <li id="ErrorDateMalformedTo"><%=this.Dictionary["Item_EquipmentRecord_Error_FilterDateToMalformed"]%></li>
                                                                                        </ul>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tfoot>
                                                            </table>
                                                        </div>                                                                                      
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;">
															            <i>
																            <%=this.Dictionary["Common_RegisterCount"] %>:
																            &nbsp;
																            <span id="NumberCosts"></span>
															            </i>
														            </th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div id="graphics" class="tab-pane">
                                                    <div class="alert alert-info" style="display: none;" id="GraphicsNoData">
                                                        <strong><i class="icon-info-sign fa-2x"></i></strong>
                                                        <h3 style="display: inline;"><%=this.Dictionary["Item_Objetivo_GraphicsNoData"] %></h3>
                                                    </div>
                                                    <div id="barChartDiv" class="col col-sm-12"></div>
                                                    <div id="circularGaugeContainer" style="display:none;height:200px;margin:0 auto"></div>
                                                </div>
                                                <div id="historic" class="tab-pane">
                                                    
                                                    <div class="table-responsive" id="scrollTableDivHistorico">
                                                        <table class="table table-bordered table-striped" style="margin-top: 4px; margin-bottom: 0;">
                                                            <thead class="thin-border-bottom" id="HistoricoTHead">
                                                                <tr>
                                                                    <th id="th0" onclick="Sort(this,'ObjetivoRegistrosTable','money',false);" class="sort" style="width:100px;"><%=this.Dictionary["Item_Objetivo_FieldLabel_Action"]%></th>
                                                                    <th id="th1" onclick="Sort(this,'ObjetivoRegistrosTable','date',false);" class="sort" style="width:95px;"><%=this.Dictionary["Item_IndicatorRecord_FieldLabel_Date"]%></th>
                                                                    <th id="th2"><%=this.Dictionary["Item_ObjetivoRecord_FieldLabel_Reason"]%></th>
                                                                    <th id="th3" onclick="Sort(this,'ObjetivoRegistrosTable','text',false);" class="sort" style="width: 256px;"><%=this.Dictionary["Item_Objetivo_FieldLabel_CloseResponsible"]%></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDivHistorico" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ObjetivoHistoricoTable"></tbody>
                                                                <tfoot id="ItemTableHistoricoVoid" style="display: none; height: 100%;">
                                                                    <tr>
                                                                        <td colspan="10" align="center" style="background-color: #ddddff; color: #0000aa;">
                                                                            <table style="border: none;">
                                                                                <tr>
                                                                                    <td rowspan="2" style="border: none;" class="NoData"><i class="icon-info-sign" style="font-size: 48px;"></i></td>
                                                                                    <td style="border: none;">
                                                                                        <h4 class="NoData" style="font-size: 24px;"><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tfoot>
                                                            </table>
                                                        </div>                                                                                      
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHistoricFooter">
                                                                    <th style="color:#aaa;">
															            <i>
																            <%=this.Dictionary["Common_RegisterCount"] %>:
																            &nbsp;
																            <span id="NumberHistoric"></span>
															            </i>
														            </th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>                                            
                                        </div>
                                    </div>
                                </div>
                                <div style="height:30px;">&nbsp;</div>
                            </div>
    
                            <!-- Popup dialogs -->

                            <div id="dialogAnular" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form" id="FormDialogAnular">
                                    <div class="form-group">
                                        <label id="TxtAnularCommentsLabel" class="col-sm-3 control-label no-padding-right" for="TxtRegistroComments"><%=this.Dictionary["Item_ObjetivoRecord_FieldLabel_Reason"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtAnularComments" rows="5"></textarea>
                                            <span class="ErrorMessage" id="TxtAnularCommentsErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtFechaCierreRealLabel" class="col-sm-3 control-label no-padding-right" for="TxtRecordDate"><%=this.Dictionary["Item_IndicatorRecord_FieldLabel_Date"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtFechaCierreRealDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtFechaCierreReal" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtFechaCierreRealBtn" class="input-group-addon" onclick="document.getElementById('TxtFechaCierreReal').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                    <span class="ErrorMessage" id="TxtFechaCierreRealErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                    <span class="ErrorMessage" id="TxtFechaCierreRealDateMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                    <span class="ErrorMessage" id="TxtFechaCierreRealCrossDate"><%= this.Dictionary["Item_Objetivo_Error_PreviewEndDate"] %></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <%=this.CmbResponsibleClose.Render %>
                                    </div>
                                </form>
                            </div>

    
                            <div id="dialogDataChanged" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form" id="FormDialogAnular">
                                    <p><%=this.Dictionary["Item_Objetivo_DataChangedWarning"] %></p>                                    
                                </form>
                            </div>
                            
                            <div id="dialogNewRecord" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="FormDialogNewMaintaiment">
                                    <div class="form-group"> 
                                        <label id ="TxtRegistroValueLabel" class="col-sm-3 control-label no-padding-right" for="TxtRegistroValue"><%=this.Dictionary["Item_IndicatorRecord_FieldLabel_Value"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtRegistroValue" placeholder="" value="" maxlength="8" onkeypress="validate(event);" />
                                            <span class="ErrorMessage" id="TxtRegistroValueErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div> 
                                    <div class="form-group">
                                        <label id ="TxtRecordDateLabel" class="col-sm-3 control-label no-padding-right" for="TxtRecordDate"><%=this.Dictionary["Item_IndicatorRecord_FieldLabel_Date"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtRecordDateDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtRecordDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtRecordDateBtn" class="input-group-addon" onclick="document.getElementById('TxtRecordDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                    <span class="ErrorMessage" id="TxtRecordDateRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                    <span class="ErrorMessage" id="TxtRecordDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                    <span class="ErrorMessage" id="TxtRecordDateMaximumToday"><%=this.Dictionary["Common_Error_MaximumToday"] %></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                            
                                    <div class="form-group">
                                        <label id ="TxtRegistroCommentsLabel" class="col-sm-3 control-label no-padding-right" for="TxtRegistroComments"><%=this.Dictionary["Item_IndicatorRecord_FieldLabel_Comments"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtRegistroComments" rows="5"></textarea>
                                            <span class="ErrorMessage" id="TxtRegistroCommentsErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>   
                                    <div class="form-group">
                                        <%=this.CmbResponsibleRecord.Render %>
                                    </div>
                                </form>
                            </div>
                            
                            <div id="dialogDeleteRecord" class="hide" style="width:500px;">
                                <p><strong><span id="dialogDeleteName"></span></strong>?</p>
                            </div>

                            <div id="IncidentActionDeleteDialog" class="hide" style="width:500px;">
                                <p>&nbsp;<strong><span id="IncidentActionDeleteName"></span></strong>?</p>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>   
        <!-- script type="text/javascript" src="/assets/js/jquery.dynameter.js"></script -->
        <script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/globalize/0.1.1/globalize.min.js"></script>
        <script type="text/javascript" src="/assets/js/dx.chartjs.js"></script>
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/Chart.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/ObjetivoView.js?ac=<%=this.AntiCache %>"></script>
</asp:Content>

