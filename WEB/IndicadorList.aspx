<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="IndicadorList.aspx.cs" Inherits="IndicadorList" %>

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
        var Procesos = <%=this.Procesos %>;
        var TiposProceso = <%=this.TiposProceso %>;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <div class="col-sm-12">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width: 200px;">
                                                <div class="row">
                                                    <label id="TxtDateFromLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentAction_List_Filter_From"] %></label>
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
                                            <td style="width:100px;padding-left:20px;"><%=this.Dictionary["Item_Indicaddor_Filter_TypeLabel"] %>:</td>
                                            <td>
                                                <select id="CmbTipusIndicador" class="col-sm-12" onchange="CmbTipusIndicadorChange()" style="max-width:300px;">
                                                    <option value="0"><%=this.Dictionary["Common_All_Male_Plural"] %></option>
                                                    <option value ="1"><%=this.Dictionary["Item_Indicaddor_Filter_TypeProcess"] %></option>
                                                    <option value="2"><%=this.Dictionary["Item_Indicaddor_Filter_TypeObjetivo"] %></option>
                                                </select>
                                            </td>
                                            <td colspan="2">
                                                <div class="row">
                                                    <!--<label id="ChkClosedLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_ObjetivoAction_List_Filter_ShowClosed"] %></label>										
                                                    <div class="col-xs-9 col-sm-9 tooltip-info">
                                                        <input type="checkbox" id="ChkClosed" />
                                                    </div>-->
                                                    <input type="radio" name="RBStatus" id="RBStatus0" /><%= this.Dictionary["Common_All"] %>
                                                    <input type="radio" name="RBStatus" id="RBStatus1" /><%= this.Dictionary["Item_ObjetivoAction_List_Filter_ShowActive"] %>
                                                    <input type="radio" name="RBStatus" id="RBStatus2" /><%= this.Dictionary["Item_ObjetivoAction_List_Filter_ShowClosed"] %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:200px;">
                                                <div class="row">
                                                    <label id="TxtDateToLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentAction_List_Filter_To"] %></label>										
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
                                            <td style="width:100px;padding-left:20px;display:none;" id="CmdObjetivoLabel"><%=this.Dictionary["Item_Objetivo"] %>:</td>
                                            <td style="width:100px;padding-left:20px;display:none;" id="CmbProcesoLabel"><%=this.Dictionary["Item_Process"] %>:</td>
                                            <td>
                                                <select id="CmbProcess" class="col-sm-12" style="display:none;max-width:300px;" onchange="CmbProcessChanged();">
                                                    <option value="0"><%=this.Dictionary["Common_All_Male_Plural"] %></option>
                                                    <asp:Literal runat="server" ID="LtProcessList"></asp:Literal>
                                                </select>  
                                                <select id="CmbObjetivo" class="col-sm-12" style="display:none;max-width:300px;">
                                                    <option value="0"><%=this.Dictionary["Common_All_Male_Plural"] %></option>
                                                    <asp:Literal runat="server" ID="LtObjetivoList"></asp:Literal>
                                                </select> 
                                            </td>
                                            <td style="width:100px;padding-left:20px;display:none;" id="CmbProcesoTipoLabel"><%=this.Dictionary["Item_ProcessType"] %>:</td>
                                            <td style="width:250px;padding-left:20px;display:none;">  
                                            </td>
                                            <td>
                                                <select id="CmbProcessType" class="col-sm-12" style="display:none;max-width:300px;">
                                                    <option value="0"><%=this.Dictionary["Common_All_Male_Plural"] %></option>
                                                    <asp:Literal runat="server" ID="LtProcessTypeList"></asp:Literal>
                                                </select> 
                                            </td>
                                            <td colspan="2" align="right">
                                                <button class="btn btn-success" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><%= this.Dictionary["Common_Filter"] %></button>
                                                <button class="btn btn-success" type="button" id="BtnRecordShowAll"><i class="icon-list bigger-110"></i><%= this.Dictionary["Common_All_Male_Plural"] %></button>
                                                <button class="btn btn-success" type="button" id="BtnRecordShowNone" style="display: none;"><i class="icon-remove-circle bigger-110"></i><%= this.Dictionary["Common_None_Male"] %></button>
                                            </td>
                                        </tr>
                                    </table>
                                </div> 
                                <div style="height:8px;clear:both;"></div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th id="th0" class="sort search" onclick="Sort(this,'ListDataTable','text',false);"><%=this.Dictionary["Item_Indicador_Header_Description"] %></th>
                                                                                                                <th id="th1" class="sort hidden-480 search" style="width:90px;" onclick="Sort(this,'ListDataTable','date',false);"><%=this.Dictionary["Item_Indicador_Header_StartDate"] %></th>
                                                        <th id="th2" class="sort search" style="width:250px;" onclick="Sort(this,'ListDataTable','text',false);"><%=this.Dictionary["Item_Indicador_Header_Process"] %></th>
                                                        <th id="th3" class="sort search" style="width:110px;" onclick="Sort(this,'ListDataTable','text',false);"><%=this.Dictionary["Item_Indicador_Header_ProcessType"] %></th>
                                                        <!-- <th id="th3" class="search" style="width:200px;"><%=this.Dictionary["Item_Indicador_Header_Objetivo"] %></th> -->
                                                        <!-- <th id="th3" class="sort hidden-480" style="width:100px;" onclick="Sort(this,'ListDataTable','date',false);"><%=this.Dictionary["Item_Indicador_Header_ProcessResponsible"] %></th> -->
                                                        <th id="th4" class="sort hidden-480 search" style="width:200px;" onclick="Sort(this,'ListDataTable','text',false);"><%=this.Dictionary["Item_Indicador_Header_ObjetivoResponsible"] %></th>
                                                        <th style="width:107px !important;">&nbsp;</th>
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
                                                                        <h4><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorRequired"] %></h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border:none;">
                                                                        <ul>
                                                                            <li id="ErrorDate"><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorDate"] %></li>
                                                                            <li id="ErrorStatus"><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorStatus"] %></li>
                                                                            <li id="ErrorType"><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorType"] %></li>
                                                                        </ul>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="ItemTableVoid" style="display:none;width:100%">
                                                    <tr>
                                                        <td colspan="10" align="center" style="color:#0000aa;">
                                                            <table style="border:none;width:100%">
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
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->						
                            </div><!-- /.col -->
                            <br /><br />
                            <div id="IndicadorDeleteDialog" class="hide" style="width:500px;">
                                <p>&nbsp;<strong><span id="IndicadorDeleteName"></span></strong>?</p>
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
        <script type="text/javascript" src="js/IndicadorList.js?ac=<%=this.AntiCache %>"></script>
</asp:Content>

