<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="RulesView.aspx.cs" Inherits="RulesView" %>

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
    <script type="text/javascript">
        var rule = <%=this.Rule.Json %>;
        var oldIPR = rule.Limit;
        var companyRules = <%=RulesJson %>;
        var companyId = <%=this.company.Id %>;
        var businessRisk = <%=this.BusinessRisksJson %>;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div id="user-profile-2" class="user-profile">
                                    <div class="tabbable">
                                        <ul class="nav nav-tabs padding-18">
                                            <li class="active">
                                                <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Rules_Tab_Principal"]%></a>
                                            </li>
                                            <% if (this.Rule.Id > 0) { %>
                                            <li class="" id="History">
                                                <a data-toggle="tab" href="#history"><%=this.Dictionary["Item_Rules_Tab_History"]%></a>
                                            </li>
                                            <% } %>
                                            <% if (this.GrantTraces && false) { %>
                                            <li class="" id="TabTrazas">
                                                <a data-toggle="tab" href="#trazas"><%=this.Dictionary["Item_Rules_Tab_Traces"]%></a>
                                            </li>
                                            <% } %>
                                        </ul>
                                        <div class="tab-content no-border padding-24">
                                            <div id="home" class="tab-pane active">                                                
                                                <div class="form-horizontal" role="form">
                                                    <div class="form-group">
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Rules_FieldLabel_Name"] %><span style="color:#f00;">*</span></label>                                        
                                                        <div class="col-sm-5">
                                                            <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Rules_FieldLabel_Name"] %>" value="<%=this.Rule.Description %>" class="col-xs-12 col-sm-12 tooltip-info" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                                            <span class="ErrorMessage" id="TxtNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            <span class="ErrorMessage" id="TxtNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_AlreadyExists"] %></span>
                                                        </div>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtLimitLabel"><%=this.Dictionary["Item_Rules_FieldLabel_Limit"] %><span style="color:#f00;">*</span></label>                                        
                                                        <div class="col-sm-5">
                                                                <div class="col-sm-12">
                                                                    <div class="steps" id="stepsLimit"></div>
                                                                    <div class="help-block ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" id="input-span-slider-limit">
                                                                        <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left:0%;"></span>
                                                                    </div>
                                                                </div>
                                                            <input type="text" size="2" id="TxtLimit" placeholder="<%=this.Dictionary["Item_Rules_FieldLabel_Limit"] %>" value="<%=this.Rule.Limit %>" class="col-xs-12 col-sm-12 tooltip-info integerFormated" maxlength="2" style="display:none;" />
                                                            <span class="ErrorMessage" id="TxtLimitErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            <span class="ErrorMessage" id="TxtLimitErrorOutOfRange" style="display:none;"><%=this.Dictionary["Item_Rules_Error_LimitOutOfRange"] %></span>
                                                        </div>					                    
                                                    </div>
                                                    <div class="for-group">
                                                        <label class="col-sm-12"><%=this.Dictionary["Item_Rules_FieldLabel_Notes"] %></label>                                                        
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-12"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtNotes"><%=this.Rule.Notes %></textarea></div>
                                                    </div>    
                                                    <% if (this.Rule.Id > 0) { %>
                                                    <h4><%=this.Dictionary["Item_Rules_Section_BusinessRisk"]%></h4>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="table-responsive" id="scrollTableDiv">
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr id="ListDataHeader">
                                                                            <th id="th0" style="display:none;width:90px;"><%=this.Dictionary["Item_Rules_Section_BusinessRisk_Header_Code"]%></th>
                                                                            <th id="th1" onclick="Sort(this,'TableJobPositionDataTable','text',false);"><%=this.Dictionary["Item_Rules_Section_BusinessRisk_Header_Name"]%></th>
                                                                            <th id="th2" onclick="Sort(this,'TableJobPositionDataTable','text',false);" style="width:120px;"><%=this.Dictionary["Item_Rules_Section_BusinessRisk_Header_Status"]%></th>
                                                                            <th id="th3" onclick="Sort(this,'TableJobPositionDataTable','text',false);" style="width:86px;"><%=this.Dictionary["Item_Rules_Section_BusinessRisk_Header_Result"]%></th>
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                                <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;height:400px;">
                                                                    <table class="table table-bordered table-striped" style="border-top: none;">
                                                                        <tbody id="TableJobPositionDataTable">
                                                                            <asp:Literal runat="server" ID="TableBusiness"></asp:Literal>
                                                                        </tbody>
                                                                    </table>
                                                                </div>                                                                                       
                                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr id="ListDataFooter">
                                                                            <th style="color:#aaa;">
															                    <i>
																                    <%=this.Dictionary["Common_RegisterCount"] %>:
																                    &nbsp;
																                    <asp:Literal runat="server" ID="TotalData"></asp:Literal>
															                    </i>
														                    </th>
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                            </div><!-- /.table-responsive -->
                                                        </div><!-- /span -->
                                                    </div><!-- /row -->	
                                                    <% } %>                                               
                                                    <%=this.FormFooter %>
                                                </div>
                                            </div>

                                            <div id="history" class="tab-pane">
                                                <div class="col-sm-6">
                                                    <h4><%=this.Dictionary["Item_Rules_HistoryTitle"] %></h4>
                                                </div>
                                                <!--<div class="col-sm-6" style="text-align:right;">                                                            
                                                    <h4 class="pink" style="right:0;">
                                                        <button class="btn btn-success" type="button" id="BtnNewUploadfile" onclick="UploadFile();">
                                                            <i class="icon-plus-sign bigger-110"></i>
                                                            <%=this.Dictionary["Item_DocumentAttachment_Button_New"] %>
                                                        </button>
                                                    </h4>
                                                </div>	-->											
                                                <table class="table table-bordered table-striped">
                                                    <thead class="thin-border-bottom">
                                                        <tr>
                                                            <th style="width:80px;"><%= this.Dictionary["Item_Rule_HistoryHeader_IPR"] %></th>
                                                            <th><%=this.Dictionary["Item_Rule_HistoryHeader_Reason"] %></th>
                                                            <th style="width:100px;"><%= this.Dictionary["Item_Rule_HistoryHeader_Date"] %></th>
                                                            <th style="width:250px;"><%= this.Dictionary["Item_Rule_HistoryHeader_ApprovedBy"] %></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="TableRuleHistory">
                                                        <asp:Literal runat="server" ID="LtHistorico"></asp:Literal>
                                                    </tbody>
                                                </table>
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
                            <div id="dialogDepartment" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><i class="icon-caret-right blue"></i><%=this.Dictionary["Item_JobPosition_List_Departments_Department"] %></th>
                                                <th style="width:150px;white-space:nowrap;">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="DepartmentsTablePopup"></tbody>
                                    </table>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id="dialogChangeIPR" class="hide" style="width:800px;">
                                <div class="col col-xs-12" style="height:300px;overflow:auto">    
                                    <ul id="BusinessRiskList"></ul>
                                </div>
                                <div class="col col-xs-12"><%=this.Dictionary["Item_Rule_ChangeIPRMessage"] %></div>                                
                            </div><!-- #dialog-message -->

                            <div id="dialogChangeIPRReason" class="hide" style="width:800px;">
                                <div class="col col-xs-12"><%=this.Dictionary["Item_Rule_ChangeIPRLabel"] %></div>
                                <div class="col col-xs-12" style="height:150px;overflow:auto">    
                                    <textarea id="TxtReason" rows="5" style="width:99%;"></textarea>
                                    <span class="ErrorMessage" id="TxtReasonErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                </div>
                            </div><!-- #dialog-message -->

                            <div id="DepartmentDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Department_PopupDelete_Message"] %>&nbsp;<strong><span id="DepartmentName"></span></strong>?</p>
                            </div>
                            <div id="DepartmentUpdateDialog" class="hide" style="width:600px;">
                                <p>
                                    <label class="col-sm-2 control-label no-padding-right" id="TxtDepartmentUpdateNameLabel"><%=this.Dictionary["Item_JobPosition_FieldLabel_Name"] %>:</label> 
                                    &nbsp;&nbsp;
                                    <input type="text" id="TxtDepartmentUpdateName" size="50" placeholder="<%= this.Dictionary["Item_JobPosition_FieldLabel_Name"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                </p>
                                <span class="ErrorMessage" id="TxtDepartmentUpdateNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtDepartmentUpdateNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
                            <div id="DepartmentInsertDialog" class="hide" style="width:600px;">
                                <p>
                                    <label class="col-sm-2 control-label no-padding-right" id="TxtDepartmentNewNameLabel"><%=this.Dictionary["Item_JobPosition_FieldLabel_DepartmentName"] %></label> 
                                    &nbsp;&nbsp;
                                    <input type="text" id="TxtDepartmentNewName" size="50" placeholder="<%= this.Dictionary["Item_JobPosition_FieldLabel_DepartmentName"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                </p>
                                <span class="ErrorMessage" id="TxtDepartmentNewNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtDepartmentNewNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
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
        <script type="text/javascript" src="/js/RulesView.js?<%=this.AntiCache %>"></script>
</asp:Content>

