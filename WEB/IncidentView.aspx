<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="IncidentView.aspx.cs" Inherits="IncidentView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
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
    <link rel="stylesheet" href="/Document-Viewer/style.css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/yepnope.1.5.3-min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/ttw-document-viewer.min.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
    <script type="text/javascript">
        var GrantToWrite = <%=this.GrantToWrite %>;
        var IncidentId = <%=this.IncidentId %>;
        var BusinessRiskId = 0;
        var Incident = <%=this.Incident.Json %>;
        var IncidentAction = <%=this.IncidentAction.Json %>;
        var Providers = <%=this.ProvidersJson %>;
        var Customers = <%=this.CustomersJson %>;
        var Employees = <%=this.EmployeesJson %>;
        var Departments = <%=this.DepartmentsJson %>;
        var IncidentStatus = <%=this.IncidentStatus %>;
        var IncidentActionStatus = <%=this.IncidentActionStatus %>;
        var IncidentCosts = <%=this.IncidentCosts %>;
        var CompanyIncidentCosts = <%=this.CompanyIncidentCosts %>;
        var Costs = <%=this.CostsJson %>;
        var typeItemId = 12;
        var itemId = IncidentId;
        var pageType = "form";
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
                                                    <div>
                                                        <form class="form-horizontal" role="form">
															<div class="form-group">
                                                                <label id="CodeLabel" class="col-sm-1"><%=this.Dictionary["Item_IncidentAction_Label_Number"] %>:&nbsp;<strong style="background-color: transparent;padding: 4px 6px 4px 6px;color: #848484;border: none;"><%=this.Code %></strong></label>
                                                                <%=this.TxtDescription.Render %>
                                                            </div>
                                                            <span class="ErrorMessage" id="RTypeErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                                            <div class="form-group">
                                                                <label id="RReporterTypeLabel" class="col-sm-1"><%=this.Dictionary["Item_IncidentAction_Label_Reporter"] %></label>
                                                                <div class="col-sm-1" style="width:115px;"><input type="radio" value="0" id="RReporterType1" name="RReporterType" onclick="RReporterTypeChanged();" /><%=this.Dictionary["Item_IncidentAction_ReporterType1"] %></div>
                                                                <div class="col-sm-1" style="width:115px;"><input type="radio" value="1" id="RReporterType2" name="RReporterType" onclick="RReporterTypeChanged();" /><%=this.Dictionary["Item_IncidentAction_ReporterType2"] %></div>
                                                                <div class="col-sm-1" style="width:115px;"><input type="radio" value="2" id="RReporterType3" name="RReporterType" onclick="RReporterTypeChanged();" /><%=this.Dictionary["Item_IncidentAction_ReporterType3"] %></div>
                                                                <div class="col-sm-4" style="display: none" id="RReporterTypeCmb">
                                                                    <div id="DivCmbReporterType3">
                                                                        <div class="col-sm-11" id="" style="height: 35px !important;">
                                                                            <select id="CmbReporterType3" class="form-control col-xs-12 col-sm-12"></select>
                                                                            <input style="display: none;" type="text" readonly="readonly" id="CmbReporterType3Value" placeholder="Proveedor" class="col-xs-12 col-sm-12" />
                                                                        </div>
                                                                        <div class="col-sm-1" id="CmbReporterType3Bar"><button class="btn btn-light" style="height: 30px;" title="Proveïdors" id="BtnCmbReporterType3BAR" disabled="">...</button></div>
                                                                    </div>
                                                                    <div id="DivCmbReporterType2">
                                                                        <div class="col-sm-11" id="" style="height: 35px !important;">
                                                                            <select id="CmbReporterType2" class="form-control col-xs-12 col-sm-12"></select>
                                                                            <input style="display: none;" type="text" readonly="readonly" id="CmbReporterType2Value" placeholder="Proveedor" class="col-xs-12 col-sm-12" />
                                                                        </div>
                                                                        <div class="col-sm-1" id="CmbReporterType2Bar"><button class="btn btn-light" style="height: 30px;" title="Proveïdors" id="BtnCmbReporterType2BAR" disabled="">...</button></div>
                                                                    </div>
                                                                    <div id="DivCmbReporterType1">
                                                                        <div class="col-sm-11" id="" style="height: 35px !important;">
                                                                            <select id="CmbReporterType1" class="form-control col-xs-12 col-sm-12"></select>
                                                                            <input style="display: none;" type="text" readonly="readonly" id="CmbReporterType1Value" placeholder="Proveedor" class="col-xs-12 col-sm-12" />
                                                                        </div>
                                                                        <!--<div class="col-sm-1" id="CmbReporterType1Bar"><button class="btn btn-light" style="height:30px;" title="Proveïdors" id="BtnCmbReporterType1BAR" disabled="">...</button></div>-->
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!--div class="col-sm-4">&nbsp;&nbsp;&nbsp;&nbsp;</!--div-->
                                                            <span class="ErrorMessage" id="RReporterTypeErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                                            <div class="form-group">
                                                                <%=this.TxtWhatHappened.Render %>
                                                                <div class="col-sm-4">
                                                                    <div class="row">
                                                                        <%=this.CmbWhatHappenedResponsible.Render %>
                                                                    </div>
                                                                    <div class="row">
                                                                        <%=this.WhatHappenedDate.Render %>
                                                                    </div>
                                                                </div>
                                                            </div>   
                                                            <hr />  
                                                            <div class="form-group">
                                                                <%=this.TxtCauses.Render %>
                                                                <div class="col-sm-4">   
                                                                    <div class="row">
                                                                        <%=this.CmbCausesResponsible.Render %>
                                                                    </div>
                                                                    <div class="row">
                                                                        <%=this.CausesDate.Render %>
                                                                    </div>
                                                                </div>
                                                            </div>    
                                                            <hr /> 
                                                            <div class="form-group">
                                                                <%=this.TxtActions.Render %>
                                                                <div class="col-sm-4">
                                                                    <div class="row">
                                                                        <%=this.CmbActionsResponsible.Render %>
                                                                    </div>
                                                                    <div class="row">
                                                                        <%=this.ActionsDate.Render %>
                                                                    </div>
                                                                </div>
                                                            </div>   
                                                            <hr /> 
                                                            <div class="form-group">
                                                                <label class="col-sm-2"><%=this.Dictionary["Item_IncidentAction_Label_ApplyAction"] %></label>
                                                                <div class="col-sm-1"><input type="radio" name="RAction" id="RActionYes" onclick="RActionChanged();" />Sí</div>
                                                                <div class="col-sm-1"><input type="radio" name="RAction" id="RActionNo" onclick="RActionChanged();" />No</div>
                                                            </div> 
                                                            <hr />
                                                            <div class="form-group">
                                                            	<%=this.TxtNotes.Render %>
                                                            </div>																					  
                                                        </form>
                                                    </div>
                                                    <div id="ClosePlaceHolder"></div>
                                                </div>
                                                <div id="accion" class="tab-pane">       
                                                    <div>
                                                        <form class="form-horizontal" role="form">   
                                                            <div class="form-group">
                                                                <label id="CodeActionLabel" class="col-sm-1"><%=this.Dictionary["Item_IncidentAction_Label_Number"] %>:&nbsp;<strong style="background-color: transparent;padding: 4px 6px 4px 6px;color: #848484;border: none;"><%=this.ActionCode %></strong></label>                                                                
                                                                <%=this.TxtActionDescription.Render %>
                                                            </div>
                                                            <div class="form-group">
                                                                <%=this.TxtActionWhatHappened.Render%>
                                                                <div class="col-sm-4">
                                                                    <%=this.CmbActionWhatHappenedResponsible.Render%>
                                                                    <%=this.TxtActionWhatHappenedDate.Render %>
                                                                </div>
                                                            </div>   
                                                            <hr />  
                                                            <div class="form-group">
                                                                <%=this.TxtActionCauses.Render%>
                                                                <div class="col-sm-4">                                                                    
                                                                    <%=this.CmbActionCausesResponsible.Render%>
                                                                    <%=this.TxtActionCausesDate.Render%>
                                                                </div>
                                                            </div>    
                                                            <hr /> 
                                                            <div class="form-group">
                                                                <%=this.TxtActionActions.Render%>
                                                                <div class="col-sm-4">
                                                                    <%=this.CmbActionActionsResponsible.Render%>
                                                                    <%=this.TxtActionActionsDate.Render%>
                                                                </div>
                                                            </div>   
                                                            <hr />
                                                            <%=this.TxtActionMonitoring.Render%> 
                                                            <%=this.TxtActionNotes.Render%>
                                                        </form>
                                                    </div>
                                                    <div id="AccionAnulada"></div>
                                                </div>
                                                <div id="costes" class="tab-pane">
                                                    <div class="col-sm-12">                                                        
                                                        <div class="col-sm-8" id="SelectRow">
                                                            <!--div class="col-xs-12"-->
                                                                <div class="col-sm-2">
                                                                    <h4><%=this.Dictionary["Item_IncidentAction_Tab_Costs"] %></h4>
                                                                </div>
                                                                <div class="col-sm-3" style="padding-top:10px;" id="ChkIncidentCosts">
                                                                    <input type="checkbox" id="Chk1" onchange="FilterChanged();" checked="checked" />&nbsp;<%=this.Dictionary["Item_Incident_Cost_IncidentCheck"] %>
                                                                </div>
                                                                <div class="col-sm-3" style="padding-top:10px;" id="ChkActionCosts">
                                                                    <input type="checkbox" id="Chk2" onchange="FilterChanged();" checked="checked" />&nbsp;<%=this.Dictionary["Item_Incident_Cost_IncidentActionCheck"] %>
                                                                </div>
                                                            <!--/div-->
                                                        </div>
                                                        <div class="col-sm-4" style="text-align:right;">
                                                            <button class="btn btn-success" type="button" id="BtnNewCost">
                                                                <i class="icon-plus-sign bigger-110"></i>
                                                                <%=this.Dictionary["Item_IncidentCost_Btn_New"] %>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="table-responsive" id="scrollTableDiv">
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr>
                                                                            <th><%=this.Dictionary["Item_IncidentCost_Header_Description"] %></th>	
                                                                            <th style="width:100px;"><%=this.Dictionary["Common_Date"]%></th>	
                                                                            <th style="width:90px;"><%=this.Dictionary["Item_IncidentCost_Header_Amount"]%></th>	
                                                                            <th class="hidden-480" style="width:90px;"><%=this.Dictionary["Item_IncidentCost_Header_Quantity"]%></th>	
                                                                            <th style="width:120px;"><%=this.Dictionary["Item_IncidentCost_Header_Total"]%></th>	
                                                                            <th style="width:200px;"class="hidden-480"><%=this.Dictionary["Item_IncidentCost_Header_ReportedBy"]%></th>
                                                                            <th class="hidden-480" style="width:107px;"></th>												
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                                <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                    <table class="table table-bordered table-striped" style="border-top: none;margin:0;">
                                                                        <tbody id="IncidentCostsTableData"></tbody>
                                                                    </table>                                                                    
                                                                    <table id="IncidentCostsTableVoid" style="display:none;width:100%;margin:0;height:100%;">
                                                                        <tr>
                                                                            <td style="color:#434382;background-color:#ccccff;">
                                                                                <div style="width:100%;text-align:center;">
                                                                                    <span><i class="icon-info-sign" style="font-size:24px;"></i></span>        
                                                                                    <span style="font-size:20px;"><%=this.Dictionary["Item_IncidentCost_Filter_NoData"] %></span>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr id="ListDataFooter">
                                                                            <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="NumberCosts"></span></i></th>
                                                                            <th style="width:90px;font-weight:bold;"><%=this.Dictionary["Common_Total"] %></th>
                                                                            <th style="width:120px;"><div id="TotalCosts" style="width:100%;text-align:right;font-weight:bold;"></div></th>
                                                                            <th style="width:307px;"></th>
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                            </div><!-- /.table-responsive -->
                                                        </div><!-- /span -->
                                                    </div>
                                                </div>
                                                <div id="uploadFiles" class="tab-pane">
                                                    <div class="col-sm-12">
                                                        <div class="col-sm-8">
                                                            <div class="btn-group btn-corner" style="display:inline;">
												                <button id="BtnModeList" class="btn" type="button" style="border-bottom-left-radius:8px!important;border-top-left-radius:8px!important;" onclick="documentsModeView(0);"><i class="icon-th-list"></i></button>
												                <button id="BtnModeGrid" class="btn btn-info" type="button" style="border-bottom-right-radius:8px!important;border-top-right-radius:8px!important;" onclick="documentsModeView(1);"><i class="icon-th"></i></button>
											                </div>
                                                            <h4 style="float:left;">&nbsp;<%=this.Dictionary["Item_Attachment_SectionTitle"] %></h4>
                                                        </div>
                                                        <div class="col-sm-4" style="text-align:right;">
                                                            
                                                            <h4 class="pink" style="right:0;">
                                                                <button class="btn btn-success" type="button" id="BtnNewUploadfile" onclick="UploadFile();">
                                                                    <i class="icon-plus-sign bigger-110"></i>
                                                                    <%=this.Dictionary["Item_Attachment_Btn_New"] %>
                                                                </button>
                                                            </h4>
                                                        </div>
                                                    </div>
                                                    <div style="clear:both">&nbsp;</div>
                                                    <div class="col-sm-12" id="UploadFilesContainer">
                                                        <asp:Literal runat="server" ID="LtDocuments"></asp:Literal>
                                                    </div>
                                                    <div class="col-sm-12" id="UploadFilesList" style="display:none;">
                                                        <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <!--<th style="width:150px;"><%=this.Dictionary["Item_Attachment_Header_FileName"] %></th>-->
                                                                <th><%=this.Dictionary["Item_Attachment_Header_Description"] %></th>
                                                                <th style="width:90px;"><%=this.Dictionary["Item_Attachment_Header_CreateDate"] %></th>
                                                                <th style="width:120px;"><%=this.Dictionary["Item_Attachment_Header_Size"] %></th>
                                                                <th style="width:160px;"></th>													
                                                            </tr>
                                                        </thead>
                                                        <tbody id="TBodyDocumentsList">
                                                            <asp:Literal runat="server" ID="LtDocumentsList"></asp:Literal>
                                                        </tbody>
                                                    </table>
                                                    </div>
                                                </div>
                                                <div id="trazas" class="tab-pane">													
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th style="width:150px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"]%></th>
                                                                <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"]%></th>
                                                                <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"]%></th>
                                                                <th style="width:250px;"><%= this.Dictionary["Item_Tace_ListHeader_User"]%></th>													
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Literal runat="server" ID="LtTrazas"></asp:Literal>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Popup dialogs -->
                            
                            <div id="dialogNewCost" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="FormDialogNewMaintaiment">
                                    <div class="form-group" id="TxtIncidentCostDescriptionRow">
                                        <label id ="TxtIncidentActionCostDescriptionLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentActionCostDescription"><%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Description"] %><span class="required">*</span></label>
                                        <div class="col-sm-7">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtIncidentActionCostDescription" placeholder="<%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Description"] %>" value="" maxlength="100" />
                                            <span class="ErrorMessage" id="TxtIncidentActionCostDescriptionErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                        <div class="col-sm-2" id="DivBtnCostBAR">
                                            <button class="btn btn-light" style="height: 30px;" title="<%=this.Dictionary["Item_CostDefinitions"] %>" id="BtnCostBAR">...</button>
                                        </div>
                                    </div>  
                                    <div class="form-group">
                                        <label id="TxtIncidentCostDateLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Common_Date"] %><span class="required">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 date-picker" id="TxtIncidentCostDate" placeholder="<%=this.Dictionary["Common_Date"] %>" value="" />
                                        </div>
                                        <div class="col-sm-6">
                                            <span class="ErrorMessage" id="TxtIncidentCostDateErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                            <span class="ErrorMessage" id="TxtIncidentCostDateErrorMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                            <span class="ErrorMessage" id="TxtIncidentCostDateErrorRange"><%=this.Dictionary["Item_Incident_Cost_Error_Range"] %></span>
                                        </div>
                                    </div>                         
                                    <div class="form-group">
                                        <label id ="TxtIncidentActionCostAmountLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentActionCostAmount"><%=this.Dictionary["Item_IncidentCost_FieldLabel_Amount"] %><span class="required">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtIncidentActionCostAmount" placeholder="" value="" maxlength="8" />
                                        </div>
                                        <div class="col-sm-6">
                                            <span class="ErrorMessage" id="TxtIncidentActionCostAmountErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>                             
                                    <div class="form-group">
                                        <label id ="TxtIncidentCostQuantityLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentCostQuantity"><%=this.Dictionary["Item_IncidentCost_FieldLabel_Quantity"] %><span class="required">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtIncidentCostQuantity" placeholder="" value="" maxlength="8" onblur="this.value=$.trim(this.value);" />
                                        </div>
                                        <div class="col-sm-6">
                                            <span class="ErrorMessage" id="TxtIncidentCostQuantityErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>   
                                    <div class="form-group">
                                        <label id="CmdIncidentCostResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentCost_FieldLabel_ReportedBy"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <select id="CmdIncidentCostResponsible" class="col-xs-12 col-sm-12"></select>
                                            <span class="ErrorMessage" id="CmdIncidentCostResponsibleRequiredLabel"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                </form>
                            </div>
                            
                            <div id="dialogIncidentCostDelete" class="hide" style="width:500px;">
                                <p>¿Seguro&nbsp;<strong><span id="dialogIncidentCostDeleteName"></span></strong>?</p>
                            </div>

                            <!-- Cost definition BAR popups -->
                            <div id="dialogCost" class="hide" style="width:600px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><%= this.Dictionary["Item_CostDefinition_ListHeader_Name"] %></th>
                                                <th><%= this.Dictionary["Item_CostDefinition_ListHeader_Amount"] %></th>
                                                <th style="width:150px;">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="SelectableCost"></tbody>
                                    </table>
                                </div>
                            </div>

                            <div id="CostInsertDialog" class="hide" style="width:600px;">
                                <div class="form-group">
                                    <label id="TxtCostNewNameLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Description"] %><span style="color: #f00;">*</span></label>
                                    <div class="col-sm-9">
                                        <input type="text" name="name" value=" " class="col-sm-12" id="TxtCostNewName" size="50" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Description"] %>" maxlength="100" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostNewNameErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtCostNewNameErrorDuplicated"><%= this.Dictionary["Common_AlreadyExists"] %></span>
                                    </div>
                                </div>
                                <div style="clear:both;height:12px;"></div>
                                <div class="form-group">
                                    <label id="TxtCostNewAmountLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Amount"] %><span style="color: #f00;">*</span></label>
                                    <div class="col-sm-9">
                                        <input class="col-sm-12 money-bank" type="text" id="TxtCostNewAmount" size="50" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Amount"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostNewAmountErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="CostUpdateDialog" class="hide" style="width:600px;">
                                <div class="form-group">
                                    <label id="TxtCostNameLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Description"] %><span style="color: #f00;">*</span></label>
                                    <div class="col-sm-9">
                                        <input type="text" name="name" value=" " class="col-sm-12" id="TxtCostName" size="50" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Description"] %>" maxlength="100" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostNameErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtCostNameErrorDuplicated"><%= this.Dictionary["Common_AlreadyExists"] %></span>
                                    </div>
                                </div>
                                <div style="clear:both;height:12px;"></div>
                                <div class="form-group">
                                    <label id="TxtCostAmountLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Amount"] %><span style="color: #f00;">*</span></label>
                                    <div class="col-sm-9">
                                        <input class="col-sm-12 money-bank" type="text" id="TxtCostAmount" size="50" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Amount"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostAmountErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="CostDeleteDialog" class="hide" style="width:600px;">
                                <p>Borrar coste&nbsp;<strong><span id="CostName"></span></strong>?</p>
                            </div>
                            <!-- -------------------------------------->
    
                            <%=this.ProviderBarPopups.Render %>
                            <%=this.CustomerBarPopups.Render %>
    
                            <div id="PopupUploadFile" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <form action="/dummy.html" class="dropzone well dz-clickable" id="dropzone">
                                        <input type="file" id="fileName" name="fileName" multiple style="position:absolute;top:-100000px;"/>
                                        <div class="dz-default dz-message">
                                            <span id="UploadMessage">
                                                <span class="bigger-150 bolder">
                                                    <i class="ace-icon fa fa-caret-right red"></i>
                                                    <%=this.Dictionary["Item_DocumentAttachment_UpladTitle1"] %>
                                                </span>
                                                <%=this.Dictionary["Item_DocumentAttachment_UpladTitle2"] %>
                                                <i class="upload-icon ace-icon fa fa-cloud-upload blue fa-2x"></i>
                                            </span>
                                            <span id="UploadMessageSelected" style="display:none;">
                                                <span class="bigger-150 bolder">
                                                    <i class="ace-icon fa  icon-file-text blue">&nbsp;</i>
                                                    <span id="UploadMessageSelectedFileName"></span>
                                                </span>&nbsp;
                                                <i style="cursor:pointer;" class="ace-icon icon-ok-sign green fa-2x" onclick="ShowPreview();"></i>
                                                &nbsp;
                                                <i class="ace-icon icon-remove-sign red fa-2x" onclick="RestoreUpload();"></i>
                                            </span>
                                        </div>
									</form>
                                        <div class="col-sm-12">
                                            <label class="input-append col-sm-2"><%=this.Dictionary["Item_DocumentAttachment_PopupUpload_Description_Label"] %></label>
                                            <label class="input-append col-sm-10"><input class="col-sm-11" id="UploadFileDescription" name="UploadFileDescription" /></label>
                                        </div>
                                        <!--<div class="col-sm-12">
                                            <p><input type="checkbox" /> Guardar como copia local</p>
                                        </div>-->
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id="dialogAnular" class="hide" style="width: 400px;">
                                <form class="form-horizontal" role="form" id="FormDialogAnular">
                                    <div class="form-group">                                        
                                        <%=this.CmbClosedResponsible.Render %>
                                    </div>
                                    <div class="form-group">
                                        <%=this.ClosedDate.Render %>
                                    </div>
                                </form>
                            </div>

                            <div id="dialogAnularAccion" class="hide" style="width: 400px;">
                                <form class="form-horizontal" role="form" id="FormDialogAnularAccion">
                                    <div class="form-group">                                        
                                        <%=this.CmbActionClosedResponsible.Render %>
                                    </div>
                                    <div class="form-group">
                                        <%=this.TxtActionClosedDate.Render %>
                                    </div>
                                </form>
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
        <script type="text/javascript" src="/js/CostBar.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/Incident.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/IncidentCost.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/IncidentProvider.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/IncidentCustomer.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript">
            jQuery(function ($) {

                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || '&nbsp;'
                        if (("title_html" in this.options) && this.options.title_html == true) {
                            title.html($title);
                        }
                        else {
                            title.text($title);
                        }
                    }
                }));

                var options = $.extend({}, $.datepicker.regional["<%=this.ApplicationUser.Language%>"], { autoclose: true, todayHighlight: true });
                $(".date-picker").datepicker(options);
                $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

                $("#BtnSave").on("click", function (e) { e.preventDefault(); SaveIncident(); });
                $("#BtnCancel").on("click", function () { document.location = referrer; });
                $("#BtnSaveAction").on("click", function (e) { e.preventDefault(); SaveIncident(); });
                $("#BtnCancelAction").on("click", function () { document.location = referrer; });
                $("#BtnPrint").on("click", PrintData);

                $("#BtnNewCost").on("click", function (e) {
                    e.preventDefault();
                    ShowNewCostPopup(0);
                });                

                $("#CmbReporterType2Bar").on("click", function (e){
                    e.preventDefault();
                    ShowProviderBarPopup($("#CmbReporterType2"));
                });                

                $("#CmbReporterType3Bar").on("click", function (e){
                    e.preventDefault();
                    ShowCustomerBarPopup($("#CmbReporterType3"));
                });

                $("#BtnCostBAR").on("click", function (e){
                    e.preventDefault();
                    ShowCostBarPopup($("#TxtCostDescription"));
                });

                // Control wizard de la incidencia
                $('#TxtCauses').on('keyup', function (e) { e.preventDefault(); TxtCausesChanged(); });
                $('#TxtActions').on('keyup', function (e) { e.preventDefault(); TxtActionsChanged(); });
                $('#CmbClosedResponsible').on('change',function(e){e.preventDefault(); CmbClosedResponsibleChanged();});

                // Control wizard de la acción
                $('#TxtActionCauses').on('keyup', function (e) { e.preventDefault(); TxtActionCausesChanged(); });
                $('#TxtActionActions').on('keyup', function (e) { e.preventDefault(); TxtActionActionsChanged(); });
                $('#CmbActionClosedResponsible').on('change', function(e) { e.preventDefault(); SetCloseRequired(); });
                $('#TxtActionClosedDate').on('change', function(e) { e.preventDefault(); SetCloseRequired(); });


                //                <%if(this.ApplicationUser.ShowHelp) { %>
                //                SetToolTip('TxtName',"<%=this.Dictionary["Item_Company_Help_Name"] %>");
                //                SetToolTip('TxtNif',"<%=this.Dictionary["Item_Company_Help_Nif"] %>");
                //                SetToolTip('DivCmbAddress',"<%=this.Dictionary["Item_Company_Help_Common_SelectAddress"] %>");
                //                SetToolTip('BtnShowAddress',"<%=this.Dictionary["Item_Company_Help_BAR_Addresses"] %>");
                //                SetToolTip('TxtNewAddress',"<%=this.Dictionary["Item_Company_Help_Address"] %>");
                //                SetToolTip('TxtNewAddressPostalCode',"<%=this.Dictionary["Item_Company_Help_ZipCode"] %>");
                //                SetToolTip('TxtNewAddressCity',"<%=this.Dictionary["Item_Company_Help_City"] %>");
                //                SetToolTip('TxtNewAddressProvince',"<%=this.Dictionary["Item_Company_Help_Provincia"] %>");
                //                SetToolTip('DivCmbPais',"<%=this.Dictionary["Item_Company_Help_Pais"] %>");
                //                SetToolTip('TxtNewAddressPhone',"<%=this.Dictionary["Item_Company_Help_Phone"] %>");
                //                SetToolTip('TxtNewAddressFax',"<%=this.Dictionary["Item_Company_Help_Fax"] %>");
                //                SetToolTip('TxtNewAddressMobile',"<%=this.Dictionary["Item_Company_Help_Mobile"] %>");
                //                SetToolTip('TxtNewAddressEmail',"<%=this.Dictionary["Item_Company_Help_Email"] %>");

                //                $('[data-rel=tooltip]').tooltip();
                //                $('[data-rel=popover]').popover({ container: 'body' });
                //                <% } %>        

            });

            IncidentFormAfterLoad();
            IncidentActionFormAfterLoad();

            console.log(document.getElementById("RActionYes").checked);
            //if ($("#CmbClosedResponsible").val() > 0) {
            if (Incident.ClosedOn !== null)
            {
                $("#TxtDescription").attr("disabled", true);
                
                $("#RReporterType1").attr("disabled", true);
                $("#RReporterType2").attr("disabled", true);
                $("#RReporterType3").attr("disabled", true);
                $("#CmbReporterType1").attr("disabled", true);
                $("#CmbReporterType2").attr("disabled", true);
                $("#CmbReporterType3").attr("disabled", true);
                $("#CmbReporterType1Bar").hide();
                $("#CmbReporterType2Bar").hide();
                $("#CmbReporterType3Bar").hide();

                $("#TxtWhatHappened").attr("disabled", true);
                $("#CmbWhatHappenedResponsible").attr("disabled", true);
                $("#TxtWhatHappenedDate").attr("disabled", true);

                $("#TxtCauses").attr("disabled", true);
                $("#CmbCausesResponsible").attr("disabled", true);
                $("#TxtCausesDate").attr("disabled", true);

                $("#TxtActions").attr("disabled", true);
                $("#CmbActionsResponsible").attr("disabled", true);
                $("#TxtActionsDate").attr("disabled", true);

                if(document.getElementById("RActionYes").checked === true)
                {
                    $("#RActionYes").attr("disabled", true);
                    $("#RActionNo").attr("disabled", true);
                }

                AnulateLayout();
            }

            function PrintData() {
                window.open("/export/PrintIncidentData.aspx?id=" + Incident.Id + "&companyId=" + Incident.CompanyId);
            }
        </script>
</asp:Content>