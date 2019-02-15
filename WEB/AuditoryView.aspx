<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="AuditoryView.aspx.cs" Inherits="AuditoryView" %>

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
    <script type="text/javascript">
        var Auditory = <%=this.Auditory.Json %>;
        var AuditoryPlanning = <%=this.Planning %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div id="user-profile-2" class="user-profile">
                                    <div class="tabbable">
                                        <% if (!this.NewAuditory)
                                            { %>
                                        <ul class="nav nav-tabs padding-18">
                                            <li class="active">
                                                <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Auditory_Tab_Principal"]%></a>
                                            </li>
                                            <li class"" style="display:none;">
                                                <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Auditory_Tab_Questionaries"]%></a>
                                            </li>
                                            <!--<% if (this.GrantTraces) { %>
                                            <li class="" id="TabTrazas">
                                                <a data-toggle="tab" href="#trazas"><%=this.Dictionary["Item_Auditory_Tab_Traces"]%></a>
                                            </li>
                                            <% } %>-->
                                        </ul>
                                        <% } %>
                                        <div class="tab-content no-border padding-24">
                                            <div id="home" class="tab-pane active">                                                
                                                <div class="form-horizontal" role="form">
                                                    <% if (this.Auditory.Type == 0) { %>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="CmbTypeLabel"><%=this.Dictionary["Item_Auditory_Label_Type"] %><span style="color:#f00">*</span></label>
                                                        <label class="col-sm-2 control-label" style="text-align:left;"><strong><%=this.Dictionary["Item_Adutory_Type_Label_" + this.Auditory.Type.ToString()] %></strong></label>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Auditory_Label_Name"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-8">
                                                            <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Auditory_Label_Name"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Description %>" />                                                            
                                                        </div>
                                                    </div> 
                                                    <% } %>
                                                    <% if (this.Auditory.Type == 1) { %>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="CmbTypeLabel"><%=this.Dictionary["Item_Auditory_Label_Type"] %><span style="color:#f00">*</span></label>
                                                        <label class="col-sm-2 control-label" style="text-align:left;"><strong><%=this.Dictionary["Item_Adutory_Type_Label_" + this.Auditory.Type.ToString()] %></strong></label>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Auditory_Label_Name"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-8">
                                                            <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Auditory_Label_Name"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Description %>" />                                                            
                                                        </div>
                                                    </div> 
                                                    <% } %>
                                                    <% if (this.Auditory.Type == 2) { %>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="CmbTypeLabel"><%=this.Dictionary["Item_Auditory_Label_Type"] %><span style="color:#f00">*</span></label>
                                                        <label class="col-sm-2 control-label" style="text-align:left;"><strong><%=this.Dictionary["Item_Adutory_Type_Label_" + this.Auditory.Type.ToString()] %></strong></label>                                                        
                                                        <label class="col-sm-1 control-label no-padding-right" id="CmbProviderLabel" for="CmbProvider"><%=this.Dictionary["Item_Auditory_Label_Provider"] %><span class="required">*</span></label>
                                                        <div class="col-sm-3">
                                                            <select id="CmbProvider" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtCmbProvider"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="CmbProviderValue" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="CmbProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Auditory_Label_Name"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-4">
                                                            <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Auditory_Label_Name"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Description %>" />                                                            
                                                        </div>
                                                    </div> 
                                                    <% } %>
                                                    <div class="form-group">
                                                        <label class="col-sm-1 control-label" id="CmbRulesLabel" for="CmbRules"><%=this.Dictionary["Item_Auditory_Label_Rules"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-11">
                                                            <select id="CmbRules" multiple="multiple" class="col-xs-12 col-sm-12 tooltip-info" onchange="CmbRulesChanged();">
                                                                <option value="-1"><%=this.Dictionary["Common_SelectOne"] %></option>
                                                                <asp:Literal runat="server" ID="LtCmbRules"></asp:Literal>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="TxtAmountLabel" for="TxtAmountCost"><%=this.Dictionary["Item_Auditory_Label_Amount"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-2">
                                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtAmount" placeholder="<%=this.Dictionary["Item_Auditory_Label_Amount"] %>" value="" maxlength="8" />
                                                            <span class="ErrorMessage" id="TextAmountErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <label class="col-sm-1 control-label no-padding-right" id="CmbInternalResponsibleLabel" for="CmbInternalResponsible"><%=this.Dictionary["Item_Auditory_Label_InternalResponsible"] %><span class="required">*</span></label>
                                                        <div class="col-sm-4">
                                                            <select id="CmbInternalResponsible" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtCmbInternalResponsible"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="CCmbInternalResponsibleValue" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="CmbInternalResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <% if (this.Auditory.Type == 1) { %>
                                                        <label id="TxtDatePlannedLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Auditory_Label_PannedDate"] %></label>
                                                            <div class="col-sm-2">
                                                                <div class="row">
                                                                    <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtDatePlannedDiv">
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker" id="TxtDatePlanned" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                                                            <span id="TxtDatePlannedBtn" class="input-group-addon" onclick="document.getElementById('TxtDatePlanned').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                        <span class="ErrorMessage" id="TxtPlannedDateErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                        <span class="ErrorMessage" id="TxtPlannedDateErrorMailMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        <% } %>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="TxtDescriptionLabel" class="col-sm-2"><%=this.Dictionary["Item_Auditory_Label_Description"]%></label>
                                                        <textarea rows="3" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtDescription"></textarea>
                                                    </div>
                                                    <% if (this.Auditory.Type == 0 || this.Auditory.Type == 2) { %>
                                                        <div class="form-group">
                                                        <label id="TxtScopeLabel" class="col-sm-1 no-padding-right"><%=this.Dictionary["Item_Auditory_Label_Scope"]%></label>
                                                        <div class="col-sm-11">
                                                            <input type="text" id="TxtScope" placeholder="<%=this.Dictionary["Item_Auditory_Label_Scope"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Scope %>" />                                                            
                                                        </div>
                                                    </div> 
                                                    <% } %>                                                    
                                                    <% if (this.Auditory.Type == 0 || this.Auditory.Type == 1) { %>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="CmbAddressLabel" for="CmbAddress"><%=this.Dictionary["Item_Auditory_Label_Address"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-11">
                                                            <select id="CmbAddress" class="col-xs-12 col-sm-12 tooltip-info">
                                                                <option value="-1"><%=this.Dictionary["Common_SelectOne"] %></option>
                                                                <asp:Literal runat="server" ID="LtCmbAddress"></asp:Literal>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <% } %>
                                                    <% if (this.Auditory.Type == 2) { %>
                                                    <div class="form-group">
                                                        <label id="TxtAddressLabel" class="col-sm-1"><%=this.Dictionary["Item_Auditory_Label_Address"]%></label>
                                                        <div class="col-sm-11">
                                                            <input type="text" id="TxtAddress" placeholder="<%=this.Dictionary["Item_Auditory_Label_Address"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.EnterpriseAddress %>" />                                                            
                                                        </div>
                                                    </div>
                                                    <% } %>
                                                    <% if (this.Auditory.Type == 1) { %>
                                                    <div class="form-group">
                                                        <label id="TxtAuditorTeamLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Auditory_Label_AuditorTeam"]%></label>
                                                        <textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtAuditorTeam"><%=this.Auditory.AuditorTeam %></textarea>
                                                    </div>
                                                    <% } %>
                                                    <div class="form-group">
                                                        <label id="TxtNotesLabel" class="col-sm-2 no-padding-right"><%=this.Dictionary["Item_Auditory_Label_Notes"]%></label>
                                                        <textarea rows="3" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtNotes"><%=this.Auditory.Notes %></textarea>
                                                    </div>
                                                    <div class="row" id="TablePlanningHeader" style="display:none;">
                                                        <div class="col-xs-5"><h4><%=this.Dictionary["Item_Auditory_PlanningTable_Title"]%></h4></div>
                                                        <div class="col-xs-4">
                                                            <div class="nav-search" id="nav-search-question" style="display: block;">                            
                                                                <span class="input-icon">
                                                                    <input type="text" placeholder="<%=this.Dictionary["Common_Search"] %>"..." class="nav-search-input" id="nav-search-input-question" autocomplete="off" />
                                                                    <i class="icon-search nav-search-icon"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-2"><button class="btn btn-success" type="button" id="BtnNewItem" onclick="ShowPopupPlanningDialog(-1);" style="margin-top:6px;height:28px;padding-top:0;"><i class="icon-plus bigger-110"></i><%= this.Dictionary["Item_Auditory_Button_AddPlanning"] %></button></div>
                                                    </div>                                                    								
                                                    <div class="table-responsive" id="scrollTableDiv" style="display:none;">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeader">
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort" style="width:100px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Date"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th1" class="search sort" style="width:70px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Hour"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th2" class="search sort" style="width:90px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Duration"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th3" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Process"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th4" class="search sort" style="width:150px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Auditor"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th5" class="search sort" style="width:150px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Audited"] %></th>
			                                                        <th style="width:107px;">&nbsp;</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="display:none;overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-left:none;border-top: none;min-height:120px;">
                                                                <tbody id="PlanningDataTable"></tbody>
                                                            </table>
                                                        </div>
                                                        <div id="NoData" style="display:none;width:100%;height:99%;min-height:120px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                        <table class="table table-bordered table-striped" style="margin: 0" >
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanPlanningTotal"></span></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div> <!-- /.table-responsive -->
                                                    <%=this.FormFooter %>
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

                            <div id="AuditoryPlanningDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_AuditoryPlanning_PopupDelete_Message"] %>&nbsp;<strong><span id="AuditoryPlanningName"></span></strong>?</p>
                            </div>

                            <div id="PopupPlanningDialog" class="hide" style="width:400px;overflow:hidden;">                                
                                <div class="form-group" style="clear:both;">
                                    <label id="TxtPlanningDateLabel" class="col-sm-4 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Date"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <div class="row">
                                            <div class="col-xs-10 col-sm-10 tooltip-info" id="TxtPlanningDateDiv">
                                                <div class="input-group">
                                                    <input class="form-control date-picker" id="TxtPlanningDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                    <span id="TxtPlanningDateLabelBtn" class="input-group-addon" onclick="document.getElementById('TxtPlanningDate').focus();">
                                                        <i class="icon-calendar bigger-110"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <span class="ErrorMessage" id="TxtPlanningDateErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        <span class="ErrorMessage" id="TxtPlanningDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"]%></span>
                                    </div>
                                </div>
                                <div class="form-group" style="clear:both;">
                                    <label id ="TxtHourLabel" class="col-sm-4 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_AuditoryPlanning_Field_Hour"] %></label>
                                    <div class="col-sm-8">
                                        <input type="text" class="col-xs-12 col-sm-12" id="TxtHour" placeholder="<%=this.Dictionary["Item_AuditoryPlanning_Field_Hour"] %>" value="" maxlength="12" />
                                        <span class="ErrorMessage" id="TxtHourMalformed"><%=this.Dictionary["Common_Error_MoneyMalformed"] %></span>
                                    </div>
                                </div>    
                                <div class="form-group" style="clear:both;">
                                    <label id ="TxtDurationLabel" class="col-sm-4 control-label no-padding-right" for="TxtDuration"><%=this.Dictionary["Item_AuditoryPlanning_Field_Duration"] %></label>
                                    <div class="col-sm-8">
                                        <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtDuration" placeholder="<%=this.Dictionary["Item_AuditoryPlanning_Field_Duration"] %>" value="" maxlength="12" />
                                        <span class="ErrorMessage" id="TxtDurationMalformed"><%=this.Dictionary["Common_Error_MoneyMalformed"] %></span>
                                    </div>
                                </div> 
                                <div class="form-group" style="clear:both;">
                                    <label id="CmbProcessLabel" class="col-sm-4 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Process"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <select id="CmbProcess" class="col-xs-12 col-sm-12"><asp:Literal runat="server" ID ="LtProcessList"></asp:Literal></select>
                                        <span class="ErrorMessage" id="CmbProcessErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="form-group" style="clear:both;">
                                    <label id="CmbAuditorLabel" class="col-sm-4 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Auditor"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <select id="CmbAuditor" class="col-xs-12 col-sm-12"><asp:Literal runat="server" ID ="LtAuditorList"></asp:Literal></select>
                                        <span class="ErrorMessage" id="CmbAuditorErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="form-group" style="clear:both;">
                                    <label id="CmbAuditedLabel" class="col-sm-4 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Audited"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <select id="CmbAudited" class="col-xs-12 col-sm-12"><asp:Literal runat="server" ID ="LtAuditedList"></asp:Literal></select>
                                        <span class="ErrorMessage" id="CmbAuditedErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="form-group" style="clear:both;">
                                    <div class="col-sm-1"><input type="checkbox" id="ChkSendMail" /></div>
                                    <label id="ChkSendMailLabel" class="col-sm-11 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_SendMail"] %></label>
                                    
                                </div>     
                                <div class="form-group" style="clear:both;display:none;" id="TxtProviderEmailRow">
                                    <label id ="TxtProviderEmailLabel" class="col-sm-4 control-label no-padding-right" for="TxtProviderEmail"><%=this.Dictionary["Item_AuditoryPlanning_Label_EmailProvider"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <input type="text" class="col-xs-12 col-sm-12 decimalFormated6" id="TxtProviderEmail" placeholder="<%=this.Dictionary["Item_AuditoryPlanning_Label_EmailProvider"] %>" value="" maxlength="12" />
                                        <span class="ErrorMessage" id="TxtProviderEmailErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        <span class="ErrorMessage" id="TxtProviderEmailMalformed"><%=this.Dictionary["Common_MessageMailMalformed"]%></span>
                                    </div>
                                </div> 
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/AuditoryView.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>

