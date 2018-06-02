<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="OportunityView.aspx.cs" Inherits="OportunityView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
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
    <script type="text/javascript">        
        var RulesCompany = <%=this.RulesJson%>;
        var rule = <%=this.Oportunity.Rule.Json%>;
        var RulesSelected = this.rule.Id;
        var Oportunity = <%=this.Oportunity.Json%>;
        var OportunityId = Oportunity.Id;
        var IncidentAction = <%=this.IncidentAction.Json %>;
        var CostImpactList = <%=this.CostImpactJson%>;
        var Action = <%=this.IncidentAction.Json%>;
        var IncidentActionCosts = <%=this.IncidentCosts %>;
        var CompanyIncidentActionCosts = <%=this.CompanyIncidentCosts %>;
        var Employees = <%=this.EmployeesJson %>;
        var CloseDate = "";
        var Costs = <%=this.CostsJson %>;
        var typeItemId = 26;
        var itemId = <%=this.Oportunity.Id %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
    <link rel="stylesheet" href="/Document-Viewer/style.css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/yepnope.1.5.3-min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/ttw-document-viewer.min.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <%=this.TabBar %>
                                            <div class="tab-content no-border padding-24">
                                                <div id="home" class="tab-pane active">                                                
                                                    <form class="form-horizontal" role="form" id="ListForm">
                                                        <div class="form-group">
                                                            <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Common_Name"]%></label>
                                                            <%=this.TxtName %>
                                                            <div class="hidden"><%=this.TxtCode %></div>
                                                            <label id="TxtDateStartLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_DateStart"]%></label>
                                                            <div class="col-sm-2">
                                                                <div class="row">
                                                                    <div class="col-xs-12 col-sm-12 tooltip-info" id="DateStartDiv">
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker" id="DateStart" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                                                            <span id="DateStartBtn" class="input-group-addon" onclick="document.getElementById('DateStart').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                        <span class="ErrorMessage" id="DateStartDateMalformed" style="display:none;"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                        <span class="ErrorMessage" id="DateStartDateOutOfDate" style="display:none;"><%=this.Dictionary["Item_Oportunity_ErrorMesagge_OutOfDate"] %></span>
                                                                        <span class="ErrorMessage" id="DateStartDateUpToLimit" style="display:none;"><%=this.Dictionary["Item_Oportunity_ErrorMessage_DateUpToLimit"] %></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">

                                                            <label id="TxtRulesLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Rules"]%></label>
                                                            <div class="col-sm-5" id="DivCmbRules" style="height:35px !important;">
                                                                <select id="CmbRules" onchange="CmbRulesChanged();" class="col-xs-10 col-sm-10"></select>
                                                                <input style="display:none;" type="text" readonly="readonly" id="TxtRules" placeholder="<%=this.Dictionary["Item_Oportunity_LabelField_Rules"] %>" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtCategoryErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                                <div class="col-sm-2"><span class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Oportunity_Button_CategoryBAR"] %>" id="BtnSelectRules">...</span></div>
                                                            </div>                                                            
                                                            
                                                            <label id="TxtProcessLabel" class="col-sm-1 no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Process"]%></label>
                                                            <div class="col-sm-5" id="DivCmbProcess" style="height:35px !important;">
                                                                <select id="CmbProcess" onchange="" class="col-xs-12 col-sm-12"><asp:Literal runat="server" ID ="LTProcess"></asp:Literal></select>
                                                                <input style="display:none;" type="text" readonly="readonly" id="TxtProcess" placeholder="<%=this.Dictionary["Item_Oportunity_LabelField_Process"] %>" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtProcessErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>                                                            
                                                            
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtDescriptionLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Description"]%></label>
                                                        </div>
                                                        <div class="form-group" style="margin-left:0px;margin-right:0px;">
                                                            <%=this.TxtItemDescription %>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtCausesLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Causes"]%></label>
                                                        </div>
                                                        <div class="form-group" style="margin-left:0px;margin-right:0px;">
                                                            <%=this.TxtCauses %>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtControlLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Control"]%></label>
                                                        </div>
                                                        <div class="form-group" style="margin-left:0px;margin-right:0px;">
                                                            <%=this.TxtControl %>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtNotesLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Notes"]%></label>
                                                        </div>
                                                        <div class="form-group" style="margin-left:0px;margin-right:0px;">
                                                            <%=this.TxtNotes %>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-4 no-padding-left">
                                                                <label id="TxtCostLabel" class="col-sm-12 no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Cost"]%></label>
                                                                <div class="row col-sm-11">
                                                                    <div class="col-sm-12">
                                                                        <div class="steps" id="stepsCost"></div>
                                                                        <div class="help-block ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" id="input-span-slider-cost">                                                                            
                                                                            <span class="ui-slider-handle ui-state-default ui-corner-all" id="CostRange" tabindex="0" style="left: 0%;"></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <label class="col-sm-1 no-padding-right" style="padding-top:25px;padding-left: 25px;">X</label>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <label id="TxtImpactLabel" class="col-sm-12 no-padding-right"><%=this.Dictionary["Item_Oportunity_LabelField_Impact"]%></label>
                                                                <div class="row col-sm-11">
                                                                    <div class="col-sm-12">
                                                                        <div class="steps" id="stepsImpact"></div>
                                                                        <div class="help-block ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" id="input-span-slider-impact">
                                                                            <span class="ui-slider-handle ui-state-default ui-corner-all" id="ImpactRange" tabindex="0" style="left: 0%;"></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <label class="col-sm-1 no-padding-right" style="padding-top:25px;padding-left: 25px;">=</label>
                                                            </div>
                                                            <div class="col-sm-1">
                                                                <label id="TxtResultLabel" class="col-sm-12 no-padding-right" style="padding-bottom: 20px;"><%=this.Dictionary["Item_Oportunity_LabelField_Result"]%></label>
                                                                <span id="Result" class="col-sm-3" style="margin-left:10px;text-align:center;font-size:20px;font-weight:bold;"><%= this.Oportunity.Result %></span>
                                                            </div>                                                            
                                                            <div class="col-sm-2">
                                                                <label id="TxtIPRLabel" class="col-sm-12 no-padding-right" style="padding-bottom: 20px;"><%=this.Dictionary["Item_Oportunity_LabelField_IPR"]%></label>
                                                                <span id="IPR" class="col-sm-3" style="margin-left:10px;text-align:center;font-size:20px;font-weight:bold;"><%= this.ActualLimit %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group" id="ActionActivator">
                                                            <div class="col-sm-3">
                                                                <input type="radio" value="1" id="ApplyActionYes" name="ApplyAction" onclick="ApplyActionRadio();" />&nbsp;<%=this.Dictionary["Item_Oportunity_LabelField_ApplyActionYes"] %>
                                                            </div>
                                                            <div class="col-sm-3" id="ApplyActionNoStart">
                                                                <input type="radio" value="2" id="ApplyActionNo" name="ApplyAction" onclick="ApplyActionRadio();" />&nbsp;<%=this.Dictionary["Item_Oportunity_LabelField_ApplyActionNo"] %>
                                                            </div>
                                                        </div>
                                                    </form>
                                                </div>
                                                <div id="accion" class="tab-pane">
                                                    <div>
                                                        <form class="form-horizontal" role="form">   
                                                            <div class="form-group">
                                                                <%=this.TxtActionDescription %>
                                                            </div>
                                                            <div class="form-group">
                                                                <%=this.TxtActionWhatHappened%>
                                                                <div class="col-sm-4">
                                                                    <%=this.ComboActionWhatHappenedResponsible.Render%>
                                                                    <%=this.TxtActionWhatHappenedDate %>
                                                                </div>
                                                            </div>   
                                                            <hr />  
                                                            <div class="form-group">
                                                                <%=this.TxtActionCauses%>
                                                                <div class="col-sm-4">                                                                    
                                                                    <%=this.ComboActionCausesResponsible.Render%>
                                                                    <%=this.TxtActionCausesDate%>
                                                                </div>
                                                            </div>    
                                                            <hr /> 
                                                            <div class="form-group">
                                                                <%=this.TxtActionActions%>
                                                                <div class="col-sm-4">
                                                                    <%=this.ComboActionActionsResponsible.Render%>
                                                                    <%=this.TxtActionActionsDate%>
                                                                </div>
                                                            </div>   
                                                            <hr />
                                                            <%=this.TxtActionMonitoring%> 
                                                            <%=this.TxtActionNotes%>
                                                        </form>
                                                    </div> 
                                                </div>
                                                <div id="costes" class="tab-pane">
                                                    <div class="col-sm-12">
                                                        <div class="col-sm-8">
                                                            <h4><%=this.Dictionary["Item_Incident_Tab_Costs"] %></h4>
                                                        </div>
                                                        <div class="col-sm-4" style="text-align:right;">
                                                            <h4 class="pink" style="right:0;">
                                                                <button class="btn btn-success" type="button" id="BtnNewCost">
                                                                    <i class="icon-plus-sign bigger-110"></i>
                                                                    <%=this.Dictionary["Item_IncidentCost_Btn_New"] %>
                                                                </button>
                                                            </h4>
                                                        </div>
                                                    </div>  
                                                    <div class="row">
                                                        <div class="alert alert-info" style="display: block;" id="DivPrimaryUser">
                                                            <strong><i class="icon-info-sign fa-2x"></i></strong>
                                                            <h3 style="display:inline;"><%=this.Dictionary["Item_Oportunity_Help_CostBlocked"] %></h3>
                                                        </div>  
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="table-responsive" id="scrollTableDiv">
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr>
                                                                            <th style="width:290px;"><%=this.Dictionary["Item_IncidentCost_Header_Description"] %></th>   
                                                                            <th style="width:90px;"><%=this.Dictionary["Item_IncidentCost_Header_Amount"]%></th>  
                                                                            <th style="width:90px;"><%=this.Dictionary["Item_IncidentCost_Header_Quantity"]%></th>   
                                                                            <th style="width:120px;"><%=this.Dictionary["Item_IncidentCost_Header_Total"]%></th>  
                                                                            <th class="hidden-480"><%=this.Dictionary["Item_IncidentCost_Header_ReportedBy"]%></th>
                                                                            <th class="hidden-480" style="width:107px;"></th>                                                
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                                <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                    <table class="table table-bordered table-striped" style="border-top: none;">
                                                                        <tbody id="IncidentActionCostsTableData"></tbody>
                                                                        <tbody id="IncidentActionCostsTableVoid" style="display:none;height:100%;width:100%;">
                                                                            <tr>
                                                                                <td colspan="10" align="center" style="background-color:#ddddff;color:#0000aa;">
                                                                                    <table style="border:none;">
                                                                                        <tr>
                                                                                            <td rowspan="2" style="border:none;"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                                            <td style="border:none;">
                                                                                                <h4><%=this.Dictionary["Item_Oportunity_Filter_NoData"] %></h4>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
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
                                                <div id="trazas" class="tab-pane" style="margin-bottom:24px;">													
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
                                            <div class="row">                                                            
                                                <%=this.FormFooter %>
                                            </div>
                                    </div>
                                </div>
                            </div>
                            <div id="dialogRules" class="hide" style="width:500px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped" style="margin-bottom:0;">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><%=this.Dictionary["Item_Rules_Popup_Header"] %></th>
                                                <th style="width:50px;"><%=this.Dictionary["Item_Rules_Popup_Limit"] %></th>
                                                <th style="width:150px;"  class="hidden-480">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                    </table>
                                    <div style="height:300px;overflow:auto;margin-top:0;" id="DivTableRulesContainer">
                                        <table class="table table-bordered table-striped" id="TableRules">
                                            <tbody id="SelectableRules" style="height:200px;overflow:visible;"></tbody>
                                        </table>
                                        <div id="DivTableRulesFiller">&nbsp;</div>
                                    </div>
                                </div><!-- /.table-responsive -->
                            </div>
                            <div id="RuleDeleteDialog" class="hide" style="width:600px;">
                                <p><%=this.Dictionary["Item_Rules_PopupDelete_Message"] %>&nbsp;<strong><span id="RuleName"></span></strong>?</p>
                            </div>
                            <div id="RulesUpdateDialog" class="hide" style="width:600px;">
                                <div class="col-sm-12" style="margin-bottom:12px;">
                                <label id="TxtRulesNameLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Rules_FieldLabel_Name"]%><span style="color:#f00;">*</span></label>
                                <input type="text" id="TxtRulesName" class="col-sm-10" maxlength="50" placeholder="<%= this.Dictionary["Item_Rules_FieldLabel_Name"] %>"  onblur="this.value=$.trim(this.value);" />
                                    <span class="ErrorMessage" id="TxtRulesNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                    <span class="ErrorMessage" id="TxtRulesNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_AlreadyExists"] %></span>
                                </div>
                                <div class="col-sm-12" style="margin-bottom:12px;">
                                    <label id="TxtRulesNotesLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Rules_FieldLabel_Notes"]%></label>
                                    <textarea id="TxtRulesNotes" rows="3" class="col-sm-10" maxlength="2000" onblur="this.value=$.trim(this.value);"></textarea>
                                </div>
                                <div class="col-sm-12" style="margin-bottom:12px;">
                                    <label id="TxtLimitLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Rules_FieldLabel_Limit"]%><span style="color:#f00;">*</span></label>
                                    <input type="text" id="CmbUpdateLimit" class="col-sm-2 integerFormated" />
                                    <span class="ErrorMessage" id="CmbUpdateLimitErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                    <span class="ErrorMessage" id="CmbUpdateLimitOutOfRange" style="display:none;"><%=this.Dictionary["Item_Rules_Error_LimitOutOfRange"] %></span>                               
                                </div>
                            </div>
                            <div id="RulesInsertDialog" class="hide" style="width:600px;">
                                <div class="col-sm-12" style="margin-bottom:12px;">
                                    <label id="TxtNewRulesNameLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Rules_FieldLabel_Name"]%><span style="color:#f00;">*</span></label>
                                    <input type="text" id="TxtNewRulesName" class="col-sm-10" maxlength="50" placeholder="<%= this.Dictionary["Item_Rules_FieldLabel_Name"] %>"  onblur="this.value=$.trim(this.value);" />
                                    <span class="ErrorMessage" id="TxtNewRulesNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                    <span class="ErrorMessage" id="TxtNewRulesNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_AlreadyExists"] %></span>
                                </div>
                                <div class="col-sm-12" style="margin-bottom:12px;">
                                    <label id="TxtNewRulesNotesLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Rules_FieldLabel_Notes"]%></label>
                                    <textarea id="TxtNewRulesNotes" class="col-sm-10" maxlength="2000" rows="3" onblur="this.value=$.trim(this.value);"></textarea>
                                </div>
                                <div class="col-sm-12">
                                    <label id="TxtNewLimitLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Rules_FieldLabel_Limit"]%><span style="color:#f00;">*</span></label>
                                    <input type="text" id="CmbNewLimit" class="col-sm-2 integerFormated" />
                                    <span class="ErrorMessage" id="CmbNewLimitErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                    <span class="ErrorMessage" id="CmbNewLimitOutOfRange" style="display:none;"><%=this.Dictionary["Item_Rules_Error_LimitOutOfRange"] %></span>
                                </div>
                            </div>
                            <div id ="RemainderDialog" class="hide">
                                <div class="col-sm-6">
                                    <table class="table">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th id="CostTitle"><%=this.Dictionary["Item_Oportunity_LabelField_Cost"]%><span id="ProbabilityRequired" style="color: rgb(255, 0, 0);" class="hide"> <%=this.Dictionary["Common_Required"] %></span></th>
                                                <th style="width:150px;"  class="hidden-480">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="SelectableCost"><asp:Literal runat="server" ID="LTCostData"></asp:Literal></tbody>
                                    </table>
                                </div>
                                <div class="col-sm-6">
                                    <table class="table">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th id="ImpactTitle"><%=this.Dictionary["Item_Oportunity_LabelField_Impact"]%><span id="SeverityRequired" style="color: rgb(255, 0, 0);" class="hide"> <%=this.Dictionary["Common_Required"] %></span></th>
                                                <th style="width:150px;"  class="hidden-480">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="SelectableSeverity"><asp:Literal runat="server" ID="LTImpactData"></asp:Literal></tbody>
                                    </table>
                                </div>
                                <div style="display:none;">
                                    <input type="text" id="CostDataContainer" />
                                    <input type="text" id="ImpactDataContainer" />
                                </div>
                            </div>

                            <div id="dialogNewCost" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="FormDialogNewMaintaiment">           
                                    <div class="form-group" id="TxtIncidentCostDescriptionRow">
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
                                        <label id ="TxtIncidentActionCostAmountLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentCostAmount"><%=this.Dictionary["Item_IncidentCost_FieldLabel_Amount"] %><span class="required">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtIncidentActionCostAmount" placeholder="" value="" maxlength="8" />
                                        </div>
                                        <div class="col-sm-6">
                                            <span class="ErrorMessage" id="TxtIncidentActionCostAmountErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>                             
                                    <div class="form-group">
                                        <label id ="TxtIncidentActionCostQuantityLabel" class="col-sm-3 control-label no-padding-right" for="TxtIncidentCostQuantity"><%=this.Dictionary["Item_IncidentCost_FieldLabel_Quantity"] %><span class="required">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtIncidentActionCostQuantity" placeholder="" value="" maxlength="8" onblur="this.value=$.trim(this.value);" />
                                        </div>
                                        <div class="col-sm-6">
                                            <span class="ErrorMessage" id="TxtIncidentActionCostQuantityErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>   
                                    <div class="form-group">
                                        <label id="CmdIncidentActionCostResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentCost_FieldLabel_ReportedBy"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <select id="CmdIncidentActionCostResponsible" class="col-xs-12 col-sm-12"></select>
                                            <span class="ErrorMessage" id="CmdIncidentActionCostResponsibleRequiredLabel" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
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
                                        <input type="text" name="name" value=" " class="col-sm-12" id="TxtCostName" size="50" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Description"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
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



                            <div id="dialogActionDetails" class="hide" style="width:600px;">
                                <table style="width:100%" cellpadding="4">
                                    <tr>
                                        <td><strong><%=this.Dictionary["Item_Rules_FieldLabel_Name"]%></strong></td>
                                        <td colspan="3"><input type="text" id="TxtActionDescriptionView" class="col-sm-11" disabled="disabled" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong><%=this.Dictionary["Item_IncidentAction_Field_WhatHappened"]%></strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><textarea id="TxtActionWhatHappenedView" rows="3" disabled="disabled" class="col-sm-12" disabled="disabled" style="background-color:#eee;"></textarea></td>
                                    </tr>
                                    <tr>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_ResponsibleWhatHappend"]%></td>
                                        <td><input type="text" id="TxtActionWhatHappenedByView" disabled="disabled" /> </td>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_Schelude"]%></td>
                                        <td><input type="text" id="TxtActionWhatHappenedOnView" disabled="disabled" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong><%=this.Dictionary["Item_IncidentAction_Field_Causes"]%></strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><textarea id="TxtActionCausesView" rows="3" disabled="disabled" class="col-sm-12" style="background-color:#eee;"></textarea></td>
                                    </tr>
                                    <tr>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_ResponsibleCauses"]%></td>
                                        <td><input type="text" id="TxtActionCausesByView" disabled="disabled" /></td>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_Schelude"]%></td>
                                        <td><input type="text" id="TxtActionCausesOnView" disabled="disabled" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong><%=this.Dictionary["Item_IncidentAction_Field_Actions"]%></strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><textarea id="TxtActionActionsView" rows="3" disabled="disabled" class="col-sm-12" style="background-color:#eee;"></textarea></td>
                                    </tr>
                                    <tr>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_ResponsibleActions"]%></td>
                                        <td><input type="text" id="TxtActionActionsByView" disabled="disabled" /></td>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_Schelude"]%></td>
                                        <td><input type="text" id="TxtActionActionsOnView" disabled="disabled" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong><%=this.Dictionary["Item_IncidentAction_Field_Monitoring"]%></strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><textarea id="TxtActionMonitoringView" rows="3" disabled="disabled" class="col-sm-12" style="background-color:#eee;"></textarea></td>
                                    </tr>
                                    <tr>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_ResponsibleClose"]%></td>
                                        <td><input type="text" id="TxtActionClosedByView" disabled="disabled" /> </td>
                                        <td><%=this.Dictionary["Item_IncidentAction_Field_Schelude"]%></td>
                                        <td><input type="text" id="TxtActionClosedOnView" disabled="disabled" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong><%=this.Dictionary["Item_Rules_FieldLabel_Notes"]%></strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><textarea id="TxtActionNotesView" rows="3" disabled="disabled" class="col-sm-12" style="background-color:#eee;"></textarea></td>
                                    </tr>
                                </table>
                            </div>

    

                                
                            <div id="PopupUploadFile" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <form action="/dummy.html" class="dropzone well dz-clickable" id="dropzone">
                                        <input type="file" id="fileName" name="fileName" multiple style="position:absolute;top:-100000px;"/>
                                        <div class="dz-default dz-message">
                                            <span id="UploadMessage">
                                                <span class="bigger-125 bolder">
                                                    <i class="ace-icon fa fa-caret-right red"></i>
                                                </span>
                                                <span class="bigger-125"> <%=this.Dictionary["Item_DocumentAttachment_UpladTitle1"] %>
                                                <%=this.Dictionary["Item_DocumentAttachment_UpladTitle2"] %></span> &nbsp;&nbsp;&nbsp;
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
                                            <label class="input-append col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_DocumentAttachment_PopupUpload_Description_Label"] %></label>
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
                                        <%=this.ComboActionClosedResponsible.Render %>
                                    </div>
                                    <div class="form-group">
                                        <%=this.TxtActionClosedDate %>
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
    <script type="text/javascript" src="/js/IncidentActionCost.js?<%=this.AntiCache %>"></script>
    <script type="text/javascript" src="/js/OportunityView.js?<%=this.AntiCache %>"></script>
    <script type="text/javascript" src="/js/OportunityViewRules.js?<%=this.AntiCache %>"></script>
    <script src="//d3js.org/d3.v3.min.js"></script>
    <script type="text/javascript" src="/js/nv.d3.js"></script>
    <script type="text/javascript" src="/js/CostBar.js?<%=this.AntiCache %>"></script>
    <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>