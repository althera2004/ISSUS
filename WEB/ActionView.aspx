<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ActionView.aspx.cs" Inherits="ActionView" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
    <link type="text/css" rel="stylesheet" href="/Document-Viewer/style.css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/yepnope.1.5.3-min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/ttw-document-viewer.min.js"></script>
    <script type="text/javascript">
        var GrantToWrite = <%=this.GrantToWrite %>;
        var IncidentAction = <%=this.IncidentAction.Json %>;
        var IncidentActionId = <%=this.IncidentActionId %>;
        var Incident = <%=this.Incident.Json %>;
        var Costs = <%=this.CostsJson %>;
        var Providers = <%=this.ProvidersJson %>;
        var Customers = <%=this.CustomersJson %>;
        var Employees = <%=this.EmployeesJson %>;
        var Departments = <%=this.DepartmentsJson %>;
        var IncidentActionCosts = <%=this.IncidentActionCosts %>;
        var CompanyIncidentActionCosts = <%=this.CompanyIncidentActionCosts %>;
        var typeItemId = 13;
        var itemId = IncidentActionId;
        var Objetivo = <%=this.Objetivo.JsonKeyValue %>;
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
                                                                <%=this.TxtDescription.Render %>
                                                            </div>
                                                            <div class="form-group" id="IncidentDiv" style="display: none;">
                                                                <label class="col-sm-1 control-label no-padding-right"><strong><%=this.Dictionary["Item_IncidentAction_Label_Origin"] %></strong></label>
                                                                <label class="col-sm-11"><%=this.Dictionary["Item_IncidentAction_Label_Incident"] %>&nbsp;<%=this.Incident.Link %></label>
                                                            </div>
                                                            <div class="form-group" id="BusinessRiskDiv" style="display: none;">
                                                                <label class="col-sm-1 control-label no-padding-right"><strong><%=this.Dictionary["Item_IncidentAction_Label_Origin"] %></strong></label>                                                                
                                                                <label class="col-sm-11"><%=this.Dictionary["Item_IncidentAction_Label_BusinessRisk"] %>&nbsp;<%=this.BusinessRisk.Link %></label>
                                                            </div>
                                                            <div class="form-group" id="ObjetivoDiv" style="display: none;">
                                                                <label class="col-sm-1 control-label no-padding-right"><strong><%=this.Dictionary["Item_IncidentAction_Label_Origin"] %></strong></label>                                                                
                                                                <label class="col-sm-11"><%=this.Dictionary["Item_IncidentAction_Label_Objetivo"] %>&nbsp;<%=this.Objetivo.Link %></label>
                                                            </div>
                                                            <div class="form-group" id="OportunityDiv" style="display: none;">
                                                                <label class="col-sm-1 control-label no-padding-right"><strong><%=this.Dictionary["Item_IncidentAction_Label_Origin"] %></strong></label>                                                                
                                                                <label class="col-sm-11"><%=this.Dictionary["Item_IncidentAction_Label_Oportunity"] %>&nbsp;<%=this.Objetivo.Link %></label>
                                                            </div>
                                                            <div class="form-group" id="ROriginDiv">
                                                                <label id="ROriginLabel" class="col-sm-2"><%=this.Dictionary["Item_IncidentAction_Label_Origin"] %><span style="color: #f00;">*</span></label>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="0" id="ROrigin1" name="ROrigin" /><%=this.Dictionary["Item_IncidentAction_Origin1"] %></div>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="1" id="ROrigin2" name="ROrigin" /><%=this.Dictionary["Item_IncidentAction_Origin2"] %></div>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="1" id="ROrigin3" name="ROrigin" /><%=this.Dictionary["Item_IncidentAction_Origin5"] %></div>
                                                                <div class="col-sm-2">
                                                                    <span class="ErrorMessage" id="ROriginErrorRequired" style="display: none;"><%=this.Dictionary["Common_Required"]%></span>
                                                                </div>
                                                            </div>
                                                            <div class="form-group" id="RTypeDiv">
                                                                <label id="RTypeLabel" class="col-sm-2"><%=this.Dictionary["Item_IncidentAction_Label_Type"] %><span style="color: #f00;">*</span></label>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="0" id="RType1" name="RType" /><%=this.Dictionary["Item_IncidentAction_Type1"] %></div>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="1" id="RType2" name="RType" /><%=this.Dictionary["Item_IncidentAction_Type2"] %></div>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="2" id="RType3" name="RType" /><%=this.Dictionary["Item_IncidentAction_Type3"] %></div>
                                                                <div class="col-sm-2">
                                                                    <span class="ErrorMessage" id="RTypeErrorRequired" style="display: none;"><%=this.Dictionary["Common_Required"]%></span>
                                                                </div>
                                                            </div>
                                                            <div class="form-group" id="RReporterDiv">
                                                                <label id="RReporterTypeLabel" class="col-sm-2"><%=this.Dictionary["Item_IncidentAction_Label_Reporter"] %><span style="color: #f00;">*</span></label>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="0" id="RReporterType1" name="RReporterType" onclick="RReporterTypeChanged();" /><%=this.Dictionary["Item_IncidentAction_ReporterType1"] %></div>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="1" id="RReporterType2" name="RReporterType" onclick="RReporterTypeChanged();" /><%=this.Dictionary["Item_IncidentAction_ReporterType2"] %></div>
                                                                <div class="col-sm-2">
                                                                    <input type="radio" value="2" id="RReporterType3" name="RReporterType" onclick="RReporterTypeChanged();" /><%=this.Dictionary["Item_IncidentAction_ReporterType3"] %></div>
                                                                <div class="col-sm-4" style="display: none" id="RReporterTypeCmb">
                                                                    <%=this.CmbReporterDepartment.Render%>
                                                                    <div id="DivCmbReporterType3">
                                                                        <div class="col-sm-9" id="" style="height: 35px !important;">
                                                                            <select id="CmbReporterType3" class="form-control col-xs-12 col-sm-12"></select>
                                                                            <input style="display: none;" type="text" readonly="readonly" id="CmbReporterType3Value" placeholder="<%= this.Dictionary["Item_Provider"] %>" class="col-xs-12 col-sm-12" />
                                                                        </div>
                                                                        <div class="col-sm-3" id="CmbReporterType3Bar">
                                                                            <button class="btn btn-light" style="height: 30px;" title="<%= this.Dictionary["Item_Providers"] %>" id="BtnCmbReporterType3BAR" disabled="">...</button></div>
                                                                    </div>
                                                                    <div id="DivCmbReporterType2">
                                                                        <div class="col-sm-9" id="" style="height: 35px !important;">
                                                                            <select id="CmbReporterType2" class="form-control col-xs-12 col-sm-12"></select>
                                                                            <input style="display: none;" type="text" readonly="readonly" id="CmbReporterType2Value" placeholder="<%= this.Dictionary["Item_Department"] %>" class="col-xs-12 col-sm-12" />
                                                                        </div>
                                                                        <div class="col-sm-3" id="CmbReporterType2Bar">
                                                                            <button class="btn btn-light" style="height: 30px;" title="<%= this.Dictionary["Item_Departments"] %>" id="BtnCmbReporterType2BAR" disabled="">...</button></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <span class="ErrorMessage" id="RReporterTypeErrorRequired" style="display: none;"><%=this.Dictionary["Common_Required"]%></span>
                                                            <div class="form-group">
                                                                <%=this.TxtWhatHappened.Render %>
                                                                <div class="col-sm-4">
                                                                    <%=this.CmbWhatHappenedResponsible.Render %>
                                                                    <%=this.WhatHappenedDate.Render %>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                            <div class="form-group">
                                                                <%=this.TxtCauses.Render %>
                                                                <div class="col-sm-4">
                                                                    <%=this.CmbCausesResponsible.Render %>
                                                                    <%=this.CausesDate.Render %>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                            <div class="form-group">
                                                                <%=this.TxtActions.Render %>
                                                                <div class="col-sm-4">
                                                                    <%=this.CmbActionsResponsible.Render %>
                                                                    <%=this.ActionsDate.Render %>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                            <div class="form-group">
                                                                <%=this.TxtMonitoring.Render %>
                                                            </div>
                                                            <hr />
                                                            <div class="form-group">
                                                                <%=this.TxtNotes.Render %>
                                                            </div>
                                                        </form>
                                                    </div> 
                                                    <%=this.FormFooter %>
                                                </div>
                                                <div id="costes" class="tab-pane">
                                                    <div class="col-sm-12">
                                                        <div class="col-sm-8">
                                                            <h4><%=this.Dictionary["Item_IncidentAction_Tab_Costs"] %></h4>
                                                        </div>
                                                        <div class="col-sm-4" style="text-align:right;">
                                                            <h4 class="pink" style="right:0;">
                                                                <button class="btn btn-success" type="button" id="BtnNewCost">
                                                                    <i class="icon-plus-sign bigger-110"></i>
                                                                    <%=this.Dictionary["Item_IncidentActionCost_Btn_New"] %>
                                                                </button>
                                                            </h4>
                                                        </div>
                                                    </div>	
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="table-responsive" id="scrollTableDiv">
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr>
                                                                            <th><%=this.Dictionary["Item_IncidentActionCost_Header_Description"] %></th>	
																			<th style="width:120px;"><%=this.Dictionary["Item_IncidentActionCost_Header_Amount"]%></th>	
                                                                            <th class="hidden-480" style="width:120px;"><%=this.Dictionary["Item_IncidentActionCost_Header_Quantity"]%></th>	
                                                                            <th style="width:150px;"><%=this.Dictionary["Item_IncidentActionCost_Header_Total"]%></th>	
                                                                            <th class="hidden-480" style="width:300px;"><%=this.Dictionary["Item_IncidentActionCost_Header_ReportedBy"]%></th>
                                                                            <th class="hidden-480" style="width:107px;"></th>												
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                                <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                    <table class="table table-bordered table-striped" style="border-top: none;">
                                                                        <tbody id="IncidentActionCostsTableData"></tbody>
                                                                    </table>
                                                                    <table id="IncidentActionCostsTableVoid" style="display:none;height:100%;width:100%;">
                                                                        <tr>
                                                                            <td colspan="10" align="center" style="background-color:#ddddff;color:#0000aa;">
                                                                                <table style="border:none;">
                                                                                    <tr>
                                                                                        <td rowspan="2" style="border:none;"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                                        <td style="border:none;">
                                                                                            <h4><%=this.Dictionary["Item_IncidentActionCost_Filter_NoData"] %></h4>
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
                                                                            <th style="color:#aaa;width:380px;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="NumberCosts"></span></i></th>
                                                                            <th style="width:90px;font-weight:bold;"><%=this.Dictionary["Common_Total"] %></th>
                                                                            <th style="width:120px;"><div id="TotalCosts" style="width:100%;text-align:right;font-weight:bold;"></div></th>
                                                                            <th></th>
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
                                                            <h4 style="float:left;">&nbsp;<%= this.Dictionary["Item_Attachment_SectionTitle"] %></h4>
                                                        </div>
                                                        <div class="col-sm-4" style="text-align:right;">
                                                            
                                                            <h4 class="pink" style="right:0;">
                                                                <button class="btn btn-success" type="button" id="BtnNewUploadfile" onclick="UploadFile();">
                                                                    <i class="icon-plus-sign bigger-110"></i>
                                                                    <%= this.Dictionary["Item_Attachment_Btn_New"] %>
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
                            
                            <div id="dialogNewCost" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="FormDialogNewMaintaiment">    
                                    <!--<div class="form-group" id="RIncidentActionCostRow">
                                        <div class="col-sm-6">
                                            <input type="radio" name="RIncidentActionCost" id="RIncidentActionCostBased" onclick="RIncidentActionCostChanged();" /><%=this.Dictionary["Item_IncidentActionCost_Radio_SelectCost"] %>
                                        </div>
                                        <div class="col-sm-6">
                                            <input type="radio" name="RIncidentActionCost" id="RIncidentActionCostNew" onclick="RIncidentActionCostChanged();" /><%=this.Dictionary["Item_IncidentActionCost_Radio_ManualEntry"] %>
                                        </div>
                                    </div>     
                                    <div class="form-group" id="CmbIncidentActionCostDescriptionRow">
                                        <label id="CmbIncidentActionCostDescriptionLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Cost"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <select id="CmbIncidentActionCostDescription" onchange="CmbIncidentActionCostDescriptionChanged();" class="col-xs-12 col-sm-12"></select>
                                            <span class="ErrorMessage" id="CmbIncidentActionCostDescriptionErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>   -->                  
                                    <div class="form-group" id="TxtIncidentActionCostDescriptionRow">
                                        <label id ="TxtIncidentActionCostDescriptionLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentActionCostDescription"><%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Description"] %><span class="required">*</span></label>
                                        <div class="col-sm-7">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtIncidentActionCostDescription" placeholder="<%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Description"] %>" value="" maxlength="100" />
                                            <span class="ErrorMessage" id="TxtIncidentActionCostDescriptionErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                        <div class="col-sm-2" id="DivBtnCostBAR">
                                            <button class="btn btn-light" style="height: 30px;" title="<%=this.Dictionary["Item_CostDefinitions"] %>" id="BtnCostBAR">...</button>
                                        </div>
                                    </div>                               
                                    <div class="form-group">
                                        <label id ="TxtIncidentActionCostAmountLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentActionCostAmount"><%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Amount"] %><span class="required">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtIncidentActionCostAmount" placeholder="<%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Amount"] %>" value="" maxlength="8" />
                                        </div>
                                        <div class="col-sm-6">
                                            <span class="ErrorMessage" id="TxtIncidentActionCostAmountErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>                             
                                    <div class="form-group">
                                        <label id ="TxtIncidentActionCostQuantityLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentActionCostQuantity"><%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Quantity"] %><span class="required">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtIncidentActionCostQuantity" placeholder="<%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Quantity"] %>" value="<%=this.Dictionary["Item_IncidentActionCost_FieldLabel_Quantity"] %>" maxlength="8" onblur="this.value=$.trim(this.value);" />
                                        </div>
                                        <div class="col-sm-6">
                                            <span class="ErrorMessage" id="TxtIncidentActionCostQuantityErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>   
                                    <div class="form-group">
                                        <label id="CmdIncidentActionCostResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentActionCost_FieldLabel_ReportedBy"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <select id="CmdIncidentActionCostResponsible" class="col-xs-12 col-sm-12"></select>
                                            <span class="ErrorMessage" id="CmdIncidentActionCostResponsibleRequiredLabel" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                </form>
                            </div>
                            
                            <div id="dialogIncidentActionCostDelete" class="hide" style="width:500px;">
                                <p>¿Seguro&nbsp;<strong><span id="dialogIncidentActionCostDeleteName"></span></strong>?</p>
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
                                        <input type="text" name="name" value=" " class="col-sm-12" id="TxtCostNewName" size="100" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Description"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostNewNameErrorRequired" style="display: none;"><%= this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtCostNewNameErrorDuplicated" style="display: none;"><%= this.Dictionary["Common_AlreadyExists"] %></span>
                                    </div>
                                </div>
                                <div style="clear:both;height:12px;"></div>
                                <div class="form-group">
                                    <label id="TxtCostNewAmountLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Amount"] %><span style="color: #f00;">*</span></label>
                                    <div class="col-sm-9">
                                        <input class="col-sm-12 money-bank" type="text" id="TxtCostNewAmount" size="50" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Amount"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostNewAmountErrorRequired" style="display: none;"><%= this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="CostUpdateDialog" class="hide" style="width:600px;">                                
                                <div class="form-group">
                                    <label id="TxtCostNameLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Description"] %><span style="color: #f00;">*</span></label>
                                    <div class="col-sm-9">
                                        <input type="text" name="name" value=" " class="col-sm-12" id="TxtCostName" size=100" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Description"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostNameErrorRequired" style="display: none;"><%= this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtCostNameErrorDuplicated" style="display: none;"><%= this.Dictionary["Common_AlreadyExists"] %></span>
                                    </div>
                                </div>
                                <div style="clear:both;height:12px;"></div>
                                <div class="form-group">
                                    <label id="TxtCostAmountLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Amount"] %><span style="color: #f00;">*</span></label>
                                    <div class="col-sm-9">
                                        <input class="col-sm-12 money-bank" type="text" id="TxtCostAmount" size="50" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Amount"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                        <span class="ErrorMessage" id="TxtCostAmountErrorRequired" style="display: none;"><%= this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="CostDeleteDialog" class="hide" style="width:600px;">
                                <p>Borrar coste&nbsp;<strong><span id="CostName"></span></strong>?</p>
                            </div>
                            <!-- -------------------------------------->

                            <%=this.ProviderBarPopups.Render %>
                            <%=this.CustomerBarPopups.Render %>

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
        <script type="text/javascript" src="/js/common.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/Actions.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/IncidentActionCost.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/ActionProvider.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/ActionCustomer.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/CostBar.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript">
            jQuery(function ($) {

                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || "&nbsp;"
                        if (("title_html" in this.options) && this.options.title_html == true)
                            title.html($title);
                        else title.text($title);
                    }
                }));

                var options = $.extend({}, $.datepicker.regional["<%=this.ApplicationUser.Language%>"], { autoclose: true, todayHighlight: true });
                $(".date-picker").datepicker(options);

                $("#BtnSave").on("click", function (e) { e.preventDefault(); SaveAction(); });
                $("#BtnCancel").on("click", function (e) { document.location = referrer; });
                $("#BtnSaveAction").on("click", function (e) { e.preventDefault(); SaveIncident(); });
                $("#BtnCancelAction").on("click", function (e) { document.location = referrer; });
                $("#BtnPrint").on("click", PrintData);

                $("#BtnNewCost").on("click", function (e) {
                    e.preventDefault();
                    ShowNewCostPopup(0);
                });   

                $("#CmbReporterType2Bar").on("click", function (e) {
                    e.preventDefault();
                    ShowProviderBarPopup($("#CmbReporterType2"));
                });                

                $("#CmbReporterType3Bar").on("click", function (e) {
                    e.preventDefault();
                    ShowCustomerBarPopup($("#CmbReporterType3"));
                });

                $("#BtnCostBAR").on("click", function (e){
                    e.preventDefault();
                    ShowCostBarPopup($("#TxtCostDescription"));
                });

                // Control wizard de la acción
                $("#TxtWhatHappened").on("keyup", function (e) { e.preventDefault(); TxtWhatHappenedChanged(); });
                $("#TxtCauses").on("keyup", function (e) { e.preventDefault(); TxtCausesChanged(); });
                $("#TxtActions").on("keyup", function (e) { e.preventDefault(); TxtActionsChanged(); });            
                $("#CmbClosedResponsible").on("change", function (e) { e.preventDefault(); SetCloseRequired(); });
            });

            function SaveAction() {
                ClearFieldTextMessages("TxtDescription");
                ClearFieldTextMessages("ROrigin");
                ClearFieldTextMessages("RType");
                ClearFieldTextMessages("RReporterType");
                ClearFieldTextMessages("TxtWhatHappened");
                ClearFieldTextMessages("TxtWhatHappenedResponsible");
                ClearFieldTextMessages("TxtWhatHappenedDate");
                var ok = true;
                var ErrorMessage = new Array();

                var dateWhatHappened = null;
                var dateCauses = null;
                var dateActions = null;
                var dateActionsExecution = null;
                var dateClose = null;
                var dateCloseExecution = null;

                if (document.getElementById("TxtDescription").value === "") {
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_DescriptionRequired);                
                    SetFieldTextMessages("TxtDescription");
                    ok = false;
                }

                if(IncidentAction.IncidentId < 1 && IncidentAction.BusinessRiskId < 1 && IncidentAction.ObjetivoId < 1)
                {
                    if (!document.getElementById("ROrigin1").checked && !document.getElementById("ROrigin2").checked) {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_OriginRequired);
                        document.getElementById("ROriginLabel").style.color = "#f00";
                    } else {
                        document.getElementById("ROriginLabel").style.color = "#000";
                    }

                    if (!document.getElementById("RType1").checked && !document.getElementById("RType2").checked && !document.getElementById("RType3").checked) {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_TypeRequired);
                        document.getElementById("RTypeLabel").style.color = "#f00";
                    } else {
                        document.getElementById("RTypeLabel").style.color = "#000";
                    }

                    if (!document.getElementById("RReporterType1").checked && !document.getElementById("RReporterType2").checked && !document.getElementById("RReporterType3").checked) {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ReportedByRequired);
                        document.getElementById("RReporterTypeLabel").style.color = "#f00";
                    } else {
                        var origin = true;
                        if (document.getElementById("RReporterType1").checked && $("#CmbReporterType1").val() * 1 === 0) { origin = false; }
                        if (document.getElementById("RReporterType2").checked && $("#CmbReporterType2").val() * 1 === 0) { origin = false; }
                        if (document.getElementById("RReporterType3").checked && $("#CmbReporterType3").val() * 1 === 0) { origin = false; }
                        if (origin === true) {
                            document.getElementById("RReporterTypeLabel").style.color = "#000";
                        } else {
                            ok = false;
                            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ReportedByRequired);
                            document.getElementById("RReporterTypeLabel").style.color = "#f00";
                        }
                    }
                }

                var state = 1;
                if ($("#TxtWhatHappened").val() === "") 
                {
                    ok=false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequired);
                    SetFieldTextMessages("TxtWhatHappened");
                }
                
                if (document.getElementById("CmbWhatHappenedResponsible").value * 1 === 0) {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequiredResponsible);
                    SetFieldTextMessages("CmbWhatHappenedResponsible");
                }

                if ($("#TxtWhatHappenedDate").val() === "") {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRquiredDate);
                    SetFieldTextMessages("TxtWhatHappenedDate");
                }
                else
                {
                    if(!RequiredDateValue("TxtWhatHappenedDate"))
                    {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_WhatHappened);
                    }
                    else
                    {
                        dateWhatHappened = GetDate($("#TxtWhatHappenedDate").val(), "/", false);
                    }
                }

                if(IncidentCausesRequired===true)
                {
                    if ($("#TxtCauses").val() === "") {
                        ok=false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequired);
                        SetFieldTextMessages("TxtCauses");

                    }
                    if (document.getElementById("CmbCausesResponsible").value * 1 === 0) {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredResponsible);
                        SetFieldTextMessages("CmbCausesResponsible");
                    }

                    if ($("#TxtCausesDate").val() === "") {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredDate);
                        SetFieldTextMessages("TxtCausesDate");
                    }
                    else
                    {
                        if(!RequiredDateValue("TxtCausesDate"))
                        {
                            ok = false;
                            ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Causes);
                        }
                        else
                        {
                            dateCauses = GetDate($("#TxtCausesDate").val(), "/", false);
                        }
                    }
                }

                if(IncidentActionsRequired===true)
                {
                    if ($("#TxtActions").val() === "") 
                    {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequired);
                        SetFieldTextMessages("TxtActions");
                    }

                    if (document.getElementById("CmbActionsResponsible").value * 1 === 0) {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredResponsible);
                        SetFieldTextMessages("CmbActionsResponsible");
                    }

                    if ($("#TxtActionsDate").val() === "") {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredDate);
                        SetFieldTextMessages("TxtActionsDate");
                    }
                    else
                    {
                        if(!RequiredDateValue("TxtActionsDate"))
                        {
                            ok = false;
                            ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Actions);
                        }
                        else
                        {
                            dateActions = GetDate($("#TxtActionsDate").val(), "/", false);
                        }
                    }
                }

                if(IncidentClosedRequired===true)
                {

                    if (document.getElementById("CmbClosedResponsible").value * 1 === 0) {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseResponsibleRequired);
                        SetFieldTextMessages("CmbClosedResponsible");
                    }

                    if ($("#TxtClosedDate").val() === "") {
                        ok = false;
                        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseDate);
                        SetFieldTextMessages("TxtClosedDate");
                    }
                    else
                    {
                        if(!RequiredDateValue("TxtClosedDate"))
                        {
                            ok = false;
                            ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Close);
                        }
                        else
                        {
                            dateClose = GetDate($("#TxtClosedDate").val(), "/", false);
                        }
                    }
                }

                // Detect untemporality dates
                var okDates=true;
                if(dateWhatHappened !== null)
                {
                    if(dateCauses !== null && dateWhatHappened > dateCauses)
                    {
                        okDates = false;
                        SetFieldTextMessages("TxtWhatHappenedDate");
                        SetFieldTextMessages("TxtCausesDate");
                    }

                    if(dateActions !== null && dateWhatHappened > dateActions)
                    {
                        okDates = false;
                        SetFieldTextMessages("TxtWhatHappenedDate");
                        SetFieldTextMessages("TxtActionsDate");
                    }

                    if(dateClose !== null && dateWhatHappened > dateClose)
                    {
                        okDates = false;
                        SetFieldTextMessages("TxtWhatHappenedDate");
                        SetFieldTextMessages("TxtClosedDate");
                    }
                }

                if(dateCauses !== null)
                {
                    if(dateActions !== null && dateCauses > dateActions)
                    {
                        okDates = false;
                        SetFieldTextMessages("TxtActionsDate");
                        SetFieldTextMessages("TxtCausesDate");
                    }

                    if(dateClose !== null && dateCauses > dateClose)
                    {
                        okDates = false;
                        SetFieldTextMessages("TxtCausesDate");
                        SetFieldTextMessages("TxtClosedDate");
                    }
                }

                if(dateActions !== null)
                {
                    if(dateClose !== null && dateActions > dateClose)
                    {
                        okDates = false;
                        SetFieldTextMessages("TxtActionsDate");
                        SetFieldTextMessages("TxtClosedDate");
                    }
                }

                if(okDates === false)
                {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_UntemporalyDates);
                }

                if (ok === false) {
                    var ErrorContent = "<ul>";
                    for(var x=0; x<ErrorMessage.length; x++)
                    {
                        ErrorContent+= "<li>" + ErrorMessage[x] + "</li>";
                    }
                    ErrorContent+="</ul>";
                    warningInfoUI("<strong>" + Dictionary.Common_Message_FormErrors + "</strong><br />" + ErrorContent, null, 600);
                    return false;
                }

                var ROrigin = 0;
                if (document.getElementById("ROrigin1").checked) { ROrigin = 1; }
                if (document.getElementById("ROrigin2").checked) { ROrigin = 2; }
                if (document.getElementById("ROrigin3").checked) { ROrigin = 5; }

                var Rtype = 0;
                if (document.getElementById("RType1").checked) { Rtype = 1; }
                if (document.getElementById("RType2").checked) { Rtype = 2; }
                if (document.getElementById("RType3").checked) { Rtype = 3; }

                var RReporter = 0;
                if (document.getElementById("RReporterType1").checked) { RReporter = 1; }
                if (document.getElementById("RReporterType2").checked) { RReporter = 2; }
                if (document.getElementById("RReporterType3").checked) { RReporter = 3; }

                var Department = { "Id": RReporter == 1 ? $("#CmbReporterType1").val() * 1 : 0 };
                var Provider = { "Id": RReporter == 2 ? $("#CmbReporterType2").val() * 1 : 0 };
                var Customer = { "Id": RReporter == 3 ? $("#CmbReporterType3").val() * 1 : 0 };

                if (IncidentAction.IncidentId > 0) {
                    RReporter = IncidentAction.ReporterType;
                    Department = IncidentAction.Department;
                    Provider = IncidentAction.Provider;
                    Customer = IncidentAction.Customer;

                    // Incidencia
                    ROrigin = 3;

                    // Correctiva
                    Rtype = 2;
                }

                if (IncidentAction.BusinessRiskId > 0) {
                    RReporter = IncidentAction.ReporterType;
                    Department = IncidentAction.Department;
                    Provider = IncidentAction.Provider;
                    Customer = IncidentAction.Customer;

                    // Analisis de riesgo
                    ROrigin = 4;

                    // Preventiva
                    Rtype = 3;
                }

                var whatHappenedOn = GetDate($("#TxtWhatHappenedDate").val(), "-");
                var causesOn = GetDate($("#TxtCausesDate").val(), "-");
                var actionsOn = GetDate($("#TxtActionsDate").val(), "-");
                var actionsSchedule = null; // ISSUS-10 GetDate($("#TxtActionsSchedule").val(), "-");
                var closedOn = GetDate($("#TxtClosedDate").val(), "-");
                var closedExecutorOn = null; // ISSUS-10 GetDate($("#TxtClosedExecutorDate").val(), "-");

                var action =
                {
                    "Id": IncidentActionId,
                    "CompanyId": Company.Id,
                    "ActionType": Rtype,
                    "Description": $("#TxtDescription").val(),
                    "Origin": ROrigin,
                    "ReporterType": RReporter,
                    "Department": Department,
                    "Provider": Provider,
                    "Customer": Customer,
                    "Number": IncidentAction.Number,
                    "IncidentId": IncidentAction.IncidentId,
                    "BusinessRiskId": IncidentAction.BusinessRiskId,
                    "Objetivo": Objetivo,
                    "WhatHappened": $("#TxtWhatHappened").val(),
                    "WhatHappenedBy": { "Id": $("#CmbWhatHappenedResponsible").val() },
                    "WhatHappenedOn": whatHappenedOn,
                    "Causes": $("#TxtCauses").val(),
                    "CausesBy": { "Id": $("#CmbCausesResponsible").val() },
                    "CausesOn": causesOn,
                    "Actions": $("#TxtActions").val(),
                    "ActionsBy": { "Id": $("#CmbActionsResponsible").val() },
                    "ActionsOn": actionsOn,
                    "ActionsExecuter": { "Id": 0 },
                    "ActionsSchedule": actionsSchedule,
                    "Monitoring": $("#TxtMonitoring").val(),
                    "ClosedBy": { "Id": $("#CmbClosedResponsible").val() },
                    "ClosedOn": closedOn,
                    "ClosedExecutor": {"Id": 0 },
                    "ClosedExecutorOn": closedExecutorOn,
                    "Notes": $("#TxtNotes").val()
                };

                var webMethod = "/Async/IncidentActionsActions.asmx/Save";
                var data = { "incidentAction": action, "userId": user.Id };
                $.ajax({
                    "type": "POST",
                    "url": webMethod,
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": JSON.stringify(data, null, 2),
                    "success": function (msg) {
                        document.location = referrer;

                    },
                    "error": function (msg) {
                        alertUI(msg.responseText);
                    }
                });

            }

            function CmbReporterDepartmentsFill() {
                var target = document.getElementById("CmbReporterType1");
                for (var x = 0; x < Departments.length; x++) {
                    var option = document.createElement("OPTION");
                    option.value = Departments[x].Id;
                    option.appendChild(document.createTextNode(Departments[x].Description));
                    target.appendChild(option);
                }
            }

            function CmbReporterProvidersFill() {
                VoidTable("CmbReporterType2");
                var target = document.getElementById("CmbReporterType2");

                var optionDefault = document.createElement("OPTION");
                optionDefault.value = 0;
                optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
                target.appendChild(optionDefault);

                for (var x = 0; x < Providers.length; x++) {
                    var option = document.createElement("OPTION");
                    option.value = Providers[x].Id;
                    option.appendChild(document.createTextNode(Providers[x].Description));
                    target.appendChild(option);
                }
            }

            function CmbReporterCustomersFill() {
                VoidTable("CmbReporterType3");
                var target = document.getElementById("CmbReporterType3");

                var optionDefault = document.createElement("OPTION");
                optionDefault.value = 0;
                optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
                target.appendChild(optionDefault);

                for (var x = 0; x < Customers.length; x++) {
                    var option = document.createElement("OPTION");
                    option.value = Customers[x].Id;
                    option.appendChild(document.createTextNode(Customers[x].Description));
                    target.appendChild(option);
                }
            }

            CmbReporterProvidersFill();
            CmbReporterCustomersFill();
            CmbReporterDepartmentsFill();

            function RReporterTypeChanged() {
                document.getElementById("RReporterTypeCmb").style.display = "";
                document.getElementById("DivCmbReporterType1").style.display = document.getElementById("RReporterType1").checked ? "" : "none";
                document.getElementById("DivCmbReporterType2").style.display = document.getElementById("RReporterType2").checked ? "" : "none";
                document.getElementById("DivCmbReporterType3").style.display = document.getElementById("RReporterType3").checked ? "" : "none";

                /* ISSUS-91
                if (document.getElementById("RReporterType1").checked) {
                    document.getElementById("CmbReporterType2").value = 0;
                    document.getElementById("CmbReporterType3").value = 0;
                }

                if (document.getElementById("RReporterType2").checked) {
                    document.getElementById("CmbReporterType1").value = 0;
                    document.getElementById("CmbReporterType3").value = 0;
                }

                if (document.getElementById("RReporterType3").checked) {
                    document.getElementById("CmbReporterType1").value = 0;
                    document.getElementById("CmbReporterType2").value = 0;
                }
                */
            }

            
            if(document.getElementById("IncidentActionCostsTableData")!=null)
            {
                IncidentActionCostRenderTable("IncidentActionCostsTableData");
            }

            // ISSUS-190
            document.getElementById("TxtDescription").focus();

            function PrintData() {
                window.open("/export/PrintActionData.aspx?id=" + IncidentAction.Id + "&companyId=" + IncidentAction.CompanyId);
            }

            // Control de permisos
            if(typeof ApplicationUser.Grants.IncidentActions === "undefined" || ApplicationUser.Grants.IncidentActions.Write === false){
                $(".btn-danger").hide();
                $("input").attr("disabled",true);
                $("textarea").attr("disabled",true);
                $("select").attr("disabled",true);
                $("select").css("background-color","#eee");
                $("#BtnNewUploadfileVersion").hide();
                $("#BtnNewCost").hide();
                $("#BtnNewUploadfile").hide();
                $("#IncidentActionCostsTableData .btn-info").hide();
                
            }

            function Resize() {
                var listTable = document.getElementById("ListDataDiv");
                var containerHeight = $(window).height();
                listTable.style.height = (containerHeight - 450) + "px";
            }

            window.onload = function () {
                SetLayout();
                FormLoad();
                Resize();
                $("#ImgCompany").after("");
                if (IncidentAction.ClosedOn !== null) {
                    $("input").attr("disabled", "disabled");
                    $("select").attr("disabled", "disabled");
                    $("textarea").attr("disabled", "disabled");
                    $("#BtnNewCost").hide();
                    $("#BtnNewUploadfile").hide();
                    $(".icon-trash").parent().hide();
                    $(".icon-edit").parent().hide();

                    $("#CmbClosedResponsible").removeAttr("disabled");
                    $("#TxtClosedDate").removeAttr("disabled");
                    //$("#TxtNotes").removeAttr("disabled");
                }

                $("#menuoption-13 a").show();
            }

            window.onresize = function () { Resize(); }
        </script>
</asp:Content>