<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="AuditoryView.aspx.cs" Inherits="AuditoryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" href="assets/css/jquery.timepicker.css" />
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

        .auditoryStep { 
          list-style: none; 
          overflow: hidden; 
          font: 18px Sans-Serif;
        }
        .auditoryStep li { 
          float: left; 
        }
        .auditoryStep li a {
          color: white;
          text-decoration: none; 
          padding: 10px 0 10px 65px;
          background: brown; /* fallback color */
          background: hsla(34,85%,35%,1); 
          position: relative; 
          display: block;
          float: left;
        }

        .breadcrumbWizard {
            list-style: none;
            overflow: hidden;
            margin: 10px;
            padding: 0;
            width: 100%;
        }

        .breadcrumbWizard li {
            float: left;
        }

        .breadcrumbWizard li a {
            _color: white;
            text-decoration: none;
            padding: 4px 0 4px 55px;
            _background: brown; /* fallback color */
            _background: hsla(34,85%,35%,1);
            position: relative;
            display: block;
            float: left;
            font-size: 12px;
        }

        .breadcrumbWizard li a:after {
            content: " ";
            display: block;
            width: 0;
            height: 0;
            border-top: 50px solid transparent; /* Go big on the size, and let overflow hide */
            border-bottom: 50px solid transparent;
            _border-left: 30px solid hsla(34,85%,35%,1);
            position: absolute;
            top: 50%;
            margin-top: -50px;
            left: 100%;
            z-index: 2;
        }

        .breadcrumbWizard li a:before {
            content: " ";
            display: block;
            width: 0;
            height: 0;
            border-top: 50px solid transparent; /* Go big on the size, and let overflow hide */
            border-bottom: 50px solid transparent;
            border-left: 30px solid white;
            position: absolute;
            top: 50%;
            margin-top: -50px;
            margin-left: 1px;
            left: 100%;
            z-index: 1;
        }

        .breadcrumbWizard li:first-child a {
            padding-left: 20px;
        }

        ._breadcrumbWizard li:last-child a:after { border: 0;
          margin-right: 10px; }

        .past a       { background: hsla(233, 85%, 25% ,1); color:#fff; }
        .past a:after { border-left: 30px solid hsla(233, 85%, 25% ,1); }

        .current a       { background: hsla(230, 79%, 69%, 1); color:#fff; }
        .current a:after { border-left: 30px solid hsla(230, 79%, 69%,1); }

        .future a       { background: hsla(230, 37%, 86%, 1); }
        .future a:after { border-left: 30px solid hsla(230, 37%, 86%,1); }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var Auditory = <%=this.Auditory.Json %>;
        var AuditoryPlanning = <%=this.Planning %>;
        var Improvements = <%=this.Improvements %>;
        var Founds = <%=this.Founds %>;
        var Completo = <%=this.CuestionariosCompletos %>;
        var Zombies = <%=this.Zombie %>;
        var TotalQuestions = <%=this.TotalQuestions %>;
        var Cuestionarios = [<%=this.CuestionariosJson %>];
        var RealActions = <%= this.RealActions %>;
        var UserEmployess = <%=this.UserEmployees %>;
        var Providers = <%=this.ProvidersJson %>;
        var Customers = <%=this.CustomersJson %>;

        // For Upload files
        // --------------------------------------
        var typeItemId = 29;
        var itemId = <%=this.Auditory.Id %>;
        // --------------------------------------
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <% if (this.Auditory.Type != 1) { %>
                            <div style="margin-top:-12px;">
                                <ul class="breadcrumbWizard">
                                    <li class="<%= this.Auditory.Status == 0 ? "current" : this.Auditory.Status > 0 ? "past" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_0"] %></a></li>
                                    <li class="<%= this.Auditory.Status == 1 ? "current" : this.Auditory.Status > 1 ? "past" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_1"] %></a></li>
                                    <li class="<%= this.Auditory.Status == 2 ? "current" : this.Auditory.Status > 2 ? "past" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_2"] %></a></li>
                                    <li class="<%= this.Auditory.Status == 3 ? "current" : this.Auditory.Status > 3 ? "past" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_3"] %></a></li>
                                    <li class="<%= this.Auditory.Status == 4 ? "current" : this.Auditory.Status > 4 ? "past" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_4"] %></a></li>
                                    <li class="<%= this.Auditory.Status == 5 ? "current" : this.Auditory.Status > 5 ? "past" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_5"] %></a></li>
                                </ul>
                            </div>
                            <% } %>
                            <div style="margin-top:30px;">
                                <div id="user-profile-2" class="user-profile">
                                    <div class="tabbable">
                                        <% if (!this.NewAuditory)
                                            { %>
                                        <ul class="nav nav-tabs padding-18">
                                            <li class="active">
                                                <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Auditory_Tab_Principal"]%></a>
                                            </li>
                                            <li class="" <% if (this.Auditory.Status < 1) { %>style="display:none;" <% } %>>
                                                <a data-toggle="tab" href="#questionaries" id="tabQuestionaries"><%=this.Dictionary["Item_Auditory_Tab_Questionaries"]%></a>
                                            </li>
                                            <li class="" <% if (this.Auditory.Status < 1) { %>style="display:none;" <% } %>>
                                                <a data-toggle="tab" href="#report" id="TabReport"><%=this.Dictionary["Item_Auditory_Tab_Report"]%></a>
                                            </li>
                                            <li class="" <% if (this.Auditory.Status != 5) { %>style="display:none;" <% } %>>
                                                <a data-toggle="tab" href="#actions"><%=this.Dictionary["Item_IncidentActions"]%></a>
                                            </li>
                                            <li class="" <% if (this.Auditory.Id < 1) { %>style="display:none;" <% } %>>
                                                <a data-toggle="tab" href="#uploadFiles"><%=this.Dictionary["Item_Equipment_Tab_UploadFiles"]%></a>
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
                                                        <label class="col-sm-1" id="TxtTypeLabel"><%=this.Dictionary["Item_Auditory_Label_Type"] %><span style="color:#f00">*</span></label>
                                                        <label class="col-sm-2 control-label" style="text-align:left;"><strong><%=this.Dictionary["Item_Adutory_Type_Label_" + this.Auditory.Type.ToString()] %></strong></label>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Auditory_Label_Name"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-8">
                                                            <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Auditory_Label_Name"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Description %>" />                                                            
                                                            <span class="ErrorMessage" id="TxtNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
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
                                                        <div class="col-sm-1" id="CmbProviderBar"><button type="button" class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnProviderBAR">...</button></div>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Auditory_Label_Name"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-3">
                                                            <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Auditory_Label_Name"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Description %>" />
                                                            <span class="ErrorMessage" id="TxtNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div> 
                                                    <% } %>
                                                    <div class="form-group">
                                                        <label class="col-sm-1 control-label" id="TxtRulesIdLabel" for="CmbRules"><%=this.Dictionary["Item_Auditory_Label_Rules"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-11" id="AuditoryRulesDiv">
                                                            <input type="text" id="TxtRulesId" style="display:none;" value="<%=this.RulesIds %>" />
                                                            <asp:Literal runat="server" ID="LtCmbRules"></asp:Literal>
                                                            <span class="ErrorMessage" id="TxtRulesIdErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="TxtAmountLabel" for="TxtAmountCost"><%=this.Dictionary["Item_Auditory_Label_Amount"] %></label>
                                                        <div class="col-sm-2">
                                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtAmount" placeholder="<%=this.Dictionary["Item_Auditory_Label_Amount"] %>" value="" maxlength="8" />
                                                            <span class="ErrorMessage" id="TextAmountErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <label class="col-sm-2 control-label no-padding-right" id="CmbInternalResponsibleLabel" for="CmbInternalResponsible"><%=this.Dictionary["Item_Auditory_Label_InternalResponsible"] %><span class="required">*</span></label>
                                                        <div class="col-sm-4">
                                                            <select id="CmbInternalResponsible" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtCmbInternalResponsible"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="CmbInternalResponsibleValue" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="CmbInternalResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="TxtDescriptionLabel" class="col-sm-1 no-padding-right"><%=this.Dictionary["Item_Auditory_Label_Description"]%><span style="color:#f00;">*</span></label>
                                                        <div class="col-sm-11">
                                                            <textarea rows="3" class="form-control col-xs-11 col-sm-12" maxlength="2000" id="TxtDescription"><%=this.Auditory.Descripcion %></textarea>
                                                            <span class="ErrorMessage" id="TxtDescriptionErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                        <div class="form-group">
                                                        <label id="TxtScopeLabel" class="col-sm-1 no-padding-right"><%=this.Dictionary["Item_Auditory_Label_Scope"]%><span style="color:#f00;">*</span></label>
                                                        <div class="col-sm-11">
                                                            <input type="text" id="TxtScope" placeholder="<%=this.Dictionary["Item_Auditory_Label_Scope"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Scope %>" />                                                            
                                                            <span class="ErrorMessage" id="TxtScopeErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>                                                    
                                                    <% if ((this.Auditory.Type == 0) && this.Auditory.Status < 1) { %>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="CmbAddressLabel" for="CmbAddress"><%=this.Dictionary["Item_Auditory_Label_Address"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-11">
                                                            <select id="CmbAddress" class="col-xs-12 col-sm-12 tooltip-info">
                                                                <option value="-1"><%=this.Dictionary["Common_SelectOne"] %></option>
                                                                <asp:Literal runat="server" ID="LtCmbAddress"></asp:Literal>
                                                            </select>
                                                            <span class="ErrorMessage" id="CmbAddressErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <% } %>
                                                    <% if (this.Auditory.Type == 2 || this.Auditory.Status > 0) { %>
                                                    <div class="form-group">
                                                        <label id="TxtAddressLabel" class="col-sm-1"><%=this.Dictionary["Item_Auditory_Label_Address"]%><span class="required">*</span></label>
                                                        <div class="col-sm-11">
                                                            <input type="text" id="TxtAddress" placeholder="<%=this.Dictionary["Item_Auditory_Label_Address"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.EnterpriseAddress %>" />                                                            
                                                            <span class="ErrorMessage" id="TxtAddressErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <% } %>
                                                    <div class="form-group">
                                                        <label id="TxtNotesLabel" class="col-sm-1 no-padding-right"><%=this.Dictionary["Item_Auditory_Label_Notes"]%></label>
                                                        <div class="col-sm-11">
                                                            <textarea rows="3" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtNotes"><%=this.Auditory.Notes %></textarea>
                                                        </div>
                                                    </div>
                                                    <div class="alert alert-info" style="display:none;" id="DivNewAuditory">
                                                        <strong><i class="icon-info-sign"></i></strong>
                                                        <div style="display:inline;"><%=this.Dictionary["Item_Auditory_Message_NewAuditory"] %></div>
                                                    </div>
                                                    <div class="row" id="TablePlanningHeader" style="display:none;">
                                                        <div class="col-xs-4"><h4><%=this.Dictionary["Item_Auditory_PlanningTable_Title"]%></h4></div>
                                                        <div class="col-xs-4">
                                                            <div class="nav-search" id="nav-search-question" style="display:none;">                            
                                                                <span class="input-icon">
                                                                    <input type="text" placeholder="<%=this.Dictionary["Common_Search"] %>"..." class="nav-search-input" id="nav-search-input-question" autocomplete="off" />
                                                                    <i class="icon-search nav-search-icon"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-4">
                                                            <button class="btn btn-success" type="button" id="BtnNewItem" onclick="ShowPopupPlanningDialog(-1);" style="margin-top:6px;height:28px;padding-top:0;float:right;">
                                                                <i class="icon-plus bigger-110"></i><%= this.Dictionary["Item_Auditory_Button_AddPlanning"] %>
                                                            </button>
                                                        </div>
                                                    </div>                                                    								
                                                    <div class="table-responsive" id="scrollTableDiv" style="display:none;">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeader">
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="search _sort" style="width:100px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Date"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th1" class="search _sort" style="width:70px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Hour"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th2" class="search _sort" style="width:90px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Duration"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th3" class="search _sort" style="cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Process"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th4" class="search _sort" style="width:150px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Auditor"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th5" class="search _sort" style="width:150px;cursor:pointer;"><%=this.Dictionary["Item_AuditoryPlanning_Header_Audited"] %></th>
			                                                        <th style="width:107px;">&nbsp;</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="display:none;overflow: scroll; overflow-x: hidden; padding: 0;min-height:120px;">
                                                            <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
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
                                                    <hr />
                                                    <div class="alert alert-info" style="display:none;" id="DivNoPlanning">
                                                        <strong><i class="icon-info-sign"></i></strong>
                                                        <div style="display:inline;"><%=this.Dictionary["Item_Auditory_Message_NoPlanning"] %></div>
                                                    </div>
                                                    <div class="alert alert-warning" style="display:none;" id="DivNoQuestions">
                                                        <strong><i class="icon-warning-sign"></i></strong>
                                                        <div style="display:inline;"><%=this.Dictionary["Item_Auditory_Message_NoQuestions"] %></div>
                                                    </div>
                                                    <div class="form-group" id="DivYesPlanning"> 
                                                        <div class="col-sm-3">&nbsp;</div>
                                                        <label class="col-sm-2 control-label no-padding-right" id="CmbPlanningResponsibleLabel" for="CmbPlanningResponsible"><%=this.Dictionary["Item_Auditory_Label_PlanningResponsible"] %></label>
                                                        <div class="col-sm-4">
                                                            <select id="CmbPlanningResponsible" class="form-control col-xs-12 col-sm-12" onchange="CmbPlanningResponsibleChanged();"><asp:Literal runat="server" ID="LtAuditoryPlanningResponsible"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="CmbPlanningResponsibleValue" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="CmbPlanningResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtAuditoryPlanningDateLabel" for="TxtAuditoryPlanning"><%=this.Dictionary["Item_Auditory_Label_PlanningDate"] %></label>
                                                        <div class="col-sm-2">
                                                            <div class="input-group">
                                                                <input style="width:90px;" class="form-control date-picker" id="TxtAuditoryPlanningDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.PlannedOn) %>" />
                                                                <span id="TxtAuditoryPlanningDateLabelBtn" class="input-group-addon" onclick="document.getElementById('TxtAuditoryPlanningDate').focus();">
                                                                    <i class="icon-calendar bigger-110"></i>
                                                                </span>
                                                            </div>
                                                            <span class="ErrorMessage" id="TxtAuditoryPlanningDateErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <%=this.FormFooter %>
                                                </div>
                                            </div>
                                            <div id="questionaries" class="tab-pane">
                                                <div class="row" id="TableCuestionarioHeader">
                                                    <div class="col-xs-5"><h4><%=this.Dictionary["Item_Auditory_Title_QuestionariesAvailables"]%></h4></div>                                                        
                                                </div>                                                    								
                                                <div class="table-responsive" id="scrollTableDivCuestionario">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderCuestionario">
			                                                    <th style="width:120px;"><%=this.Dictionary["Item_AuditoryQuestionary_Header_Status"] %></th>
			                                                    <th><%=this.Dictionary["Item_AuditoryQuestionary_Header_Name"] %></th>
			                                                    <th style="width:67px;">&nbsp;</th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivCuestionario" style="border-left:1px solid #ddd;background-color:#eef;min-height:200px;overflow: scroll; overflow-x: hidden; padding: 0;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="CuestionarioDataTable"></tbody>
                                                        </table>
                                                    </div>
                                                    <div id="NoDataCuestionario" style="display:none;width:100%;height:99%;min-height:200px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterCuestionario">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanCuestionarioTotal"></span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->
                                                
                                                <div class="alert alert-info" style="display:none;" id="DivMessageCuestonariosCerrables">
                                                    <strong><i class="fa fa-info-circle fa-fw fa-lg"></i></strong>
                                                    <div style="display:inline;">&nbsp;<%=this.Dictionary["Item_Auditory_Message_QuestionaryClosable"] %></div>
                                                </div>

                                            </div>
                                            <div id="report" class="tab-pane">
                                                <div class="form-horizontal" role="form">
                                                    <div class="row">
                                                        <label class="col col-sm-1"><%=this.Dictionary["Item_Auditory_Report_StartLabel"] %></label>
                                                        <div class="col-sm-2">
                                                            <div class="input-group">
                                                                <input style="width:90px;" class="form-control date-picker" id="TxtStartQuestionsOn" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ReportStart) %>" />
                                                                <span id="TxtStartQuestionsOnLabelBtn" class="input-group-addon" onclick="document.getElementById('TxtStartQuestionsOn').focus();">
                                                                    <i class="icon-calendar bigger-110"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <label class="col col-sm-1"><%=this.Dictionary["Item_Auditory_Report_EndLabel"] %></label>
                                                        <div class="col-sm-2">
                                                            <div class="input-group">
                                                                <input style="width:90px;" class="form-control date-picker" id="TxtCloseQuestionsOn" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ReportEnd) %>" />
                                                                <span id="TxtCloseQuestionsOnLabelBtn" class="input-group-addon" onclick="document.getElementById('TxtCloseQuestionsOn').focus();">
                                                                    <i class="icon-calendar bigger-110"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <button type="button" class="btn btn-success" id="BtnCloseCuestionarios" style="display:none;margin-top:2px;height:32px;padding-top:2px;" onclick="CloseCuestionariosPopup();">
                                                                <i class="fa fa-check"></i>&nbsp;
                                                                <%=this.Dictionary["Item_Auditory_Popup_CloseQuestionary"] %>
                                                            </button>
                                                            <button type="button" class="btn btn-success" id="BtnReopenCuestionarios" style="display:none;margin-top:2px;height:32px;padding-top:2px;" onclick="ReopenCuestionariosPopup();">
                                                                <i class="fa fa-recycle"></i>&nbsp;
                                                                <%=this.Dictionary["Item_Auditory_Popup_ReopenQuestionary"] %>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <h4><%=this.Dictionary["Item_Auditory_Title_Found"] %></h4>
                                                
                                                <div class="table-responsive" id="scrollTableDivHallazgos">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderHallazgos">
			                                                    <th><%=this.Dictionary["Item_Auditory_Header_Found"] %></th>
			                                                    <th style="width:200px;"><%=this.Dictionary["Item_Auditory_Header_Requirement"] %></th>
			                                                    <th style="width:200px;"><%=this.Dictionary["Item_Auditory_Header_Result"] %></th>
			                                                    <th style="width:80px;"><%=this.Dictionary["Item_Auditory_Header_Action"] %></th>
			                                                    <th style="width:107px;">&nbsp;</th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivHallazgos" style="display:none;overflow-y:scroll;overflow-x:hidden;padding:0;border-left:1px solid #ccc;min-height:100px;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="HallazgosDataTable"></tbody>
                                                        </table>
                                                    </div>
                                                    <div id="NoDataHallazgos" class="TableNoData">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterHallazgos">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanHallazgosTotal"></span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->
                                                
                                                <h4><%=this.Dictionary["Item_Auditory_Title_Improvements"] %></h4>
                                                <div class="table-responsive" id="scrollTableDivMejoras">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderMejoras">
			                                                    <th><%=this.Dictionary["Item_Auditory_Header_Improvement"] %></th>
			                                                    <th style="width:80px;"><%=this.Dictionary["Item_Auditory_Header_Action"] %></th>
			                                                    <th style="width:107px;">&nbsp;</th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivMejoras" style="display:none;overflow-y:scroll;overflow-x:hidden;padding: 0;border-left:1px solid #ccc;min-height:100px;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="MejorasDataTable"></tbody>
                                                        </table>
                                                    </div>
                                                    <div id="NoDataMejoras" class="TableNoData">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterMejoras">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanMejorasTotal"></span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->

                                                <div class="alert alert-warning" style="display:none;" id="DivNoActions">
                                                    <strong><i class="icon-warning-sign"></i></strong>
                                                    <div style="display:inline;"><span id="NoActionF"><%=this.Dictionary["Item_Auditory_FoundNoAction"] %></span>&nbsp;<span id="NoActionI"><%=this.Dictionary["Item_Auditory_ImprovementNoAction"] %></span></div>
                                                </div>

                                                <h4><%=this.Dictionary["Item_Auditory_Label_PuntosFuertes"] %></h4>
                                                <div class="form-group">
                                                    <textarea rows="3" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtPuntosFuertes"><%=this.Auditory.PuntosFuertes %></textarea>
                                                </div>

                                                <div class="col-sm-12" style="margin-top:12px;margin-bottom:20px;display:none;" id="DivCloseButton">
                                                    <div class="col-sm-10"></div>
                                                    <div class="col-sm-2">
                                                        <button type="button" class="btn btn-success" id="BtnCloseAuditoria"><i class="fa fa-check"></i>&nbsp;<%=this.Dictionary["Item_Auditory_Btn_Close"] %></button>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12 alert alert-info" style="display:none;margin-top:12px;margin-bottom:40px;" id="DivCloseResume">
                                                    <strong><i class="icon-info-sign fa-2x"></i></strong>
                                                    <h3 style="display:inline;"><%=this.Dictionary["Item_Auditory_Message_Closed"] %></h3>
                                                        <p style="margin-left:50px;">
                                                            <%=this.Dictionary["Item_Auditory_Label_ClosedOn"] %>:&nbsp;<span id="SpanClosedOn" style="font-weight:bold;"><%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ClosedOn) %></span>
                                                            <br />
                                                            <%=this.Dictionary["Item_Auditory_Label_ClosedBy"] %>:&nbsp;<span id="SpanClosedBy" style="font-weight:bold;"><%=this.Auditory.ClosedBy.UserName %></span>
                                                        </p>
                                                </div>

                                                <div class="col-sm-12" style="margin-top:-24px;margin-bottom:32px;display:none;" id="DivValidationButton">
                                                    <div class="col-sm-10"></div>
                                                    <div class="col-sm-2">
                                                        <button type="button" class="btn btn-success" id="BtnValidarAuditoria"><i class="fa fa-check"></i>&nbsp;<%=this.Dictionary["Item_Auditory_Btn_Validation"] %></button>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12 alert alert-info" style="display:none;margin-top:12px;margin-bottom:40px;" id="DivValidationResume">
                                                    <strong><i class="icon-info-sign fa-2x"></i></strong>
                                                    <h3 style="display:inline;"><%=this.Dictionary["Item_Auditory_Message_Validated"] %></h3>
                                                        <p style="margin-left:50px;">
                                                            <%=this.Dictionary["Item_Auditory_Label_ValidatedOn"] %>:&nbsp;<span id="SpanValidatedOn" style="font-weight:bold;"><%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ValidatedOn) %></span>
                                                            <br />
                                                            <%=this.Dictionary["Item_Auditory_Label_ValidatedBy"] %>:&nbsp;<span id="SpanValidatedBy" style="font-weight:bold;"><%=this.Auditory.ValidatedBy.FullName %></span>
                                                        </p>
                                                </div>
                                            </div>
                                            <div id="actions" class="tab-pane">
                                                <div class="row">
                                                    <div class="col-sm-6"><h4><%=this.Dictionary["Item_IncidentActions"] %></h4></div>
                                                    <div class="col-sm-6" style="text-align:right;">
                                                        <button type="button" id="BtnActionAddReal" class="btn btn-success" style="margin-top:6px;height:28px;padding-top:0;float:right;"><i class="fa fa-plus"></i>&nbsp;<%=this.Dictionary["Item_Auditory_Btn_AddIncidentAction"] %></button>
                                                    </div>
                                                </div>
                                                
                                                <div class="table-responsive" id="scrollTableDivIncidentActionsReal" style="margin-top:4px;border-left:1px solid #ddd;">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderIncidentActionsReal">
			                                                    <th style="width:65px;"><%=this.Dictionary["Item_IncidentAction_Header_Status"] %></th>
			                                                    <th style="width:120px;"><%=this.Dictionary["Item_IncidentAction_Header_Type"] %></th>
			                                                    <th><%=this.Dictionary["Item_IncidentAction_Header_Description"] %></th>
			                                                    <th style="width:100px;"><%=this.Dictionary["Item_IncidentAction_Header_Open"] %></th>
			                                                    <th style="width:67px;">&nbsp;</th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivIncidentActionsReal" style="display:none;overflow:scroll;overflow-x:hidden; padding: 0;min-height:200px;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="IncidentActionsDataTableReal"><asp:Literal runat="server" ID="Literal2"></asp:Literal></tbody>
                                                        </table>
                                                    </div>
                                                    <div id="NoDataIncidentActionsReal" style="width:100%;height:99%;min-height:200px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterIncidentActionsReal">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanIncidentActionsTotalReal">0</span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->
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
                                                                <%=this.Dictionary["Item_DocumentAttachment_Button_New"] %>
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

                            <div id="IncidentActionPopup" class="hide">
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id="TxtIncidentActionTypeLabel" class="col-sm-4 control-label no-padding-right" for="TxtIncidentActionDescription"><%=this.Dictionary["Item_IncidentAction_Label_Type"] %><span class="required">*</span></label>
                                    <div class="col-sm-4">
                                        <input type="radio" name="RBActionType" id="RBactionType1" />&nbsp;<%=this.Dictionary["Item_IncidentAction_Type1"] %>
                                    </div>
                                    <div class="col-sm-4">
                                        <input type="radio" name="RBActionType" id="RBactionType2" />&nbsp;<%=this.Dictionary["Item_IncidentAction_Type2"] %>
                                    </div>
                                    <span class="ErrorMessage" id="TxtIncidentActionTypeErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                </div>
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id="TxtIncidentActionDescriptionLabel" class="col-sm-4 control-label no-padding-right" for="TxtIncidentActionDescription"><%=this.Dictionary["Item_IncidentAction_Label_Description"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <input type="text" class="col-xs-12 col-sm-12" id="TxtIncidentActionDescription" placeholder="<%=this.Dictionary["Item_IncidentAction_Label_Description"] %>" value="" maxlength="100" />
                                        <span class="ErrorMessage" id="TxtIncidentActionDescriptionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div> 
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id ="TxtWhatHappendLabel" class="col-sm-4 control-label no-padding-right" for="TxtWhatHappend"><%=this.Dictionary["Item_IncidentAction_Field_WhatHappened"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtWhatHappend"></textarea>
                                        <span class="ErrorMessage" id="TxtWhatHappendErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label class="col-sm-4 control-label no-padding-right" id="CmbWhatHappendByLabel" for="CmbWhatHappendBy"><%=this.Dictionary["Item_IncidentAction_Field_ResponsibleWhatHappend"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <select id="CmbWhatHappendBy" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtWhatHappendByList"></asp:Literal></select>
                                        <input style="display:none;" type="text" readonly="readonly" id="CmbWhatHappendByValue" class="col-xs-12 col-sm-12" />
                                        <span class="ErrorMessage" id="CmbWhatHappendByErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label class="col-sm-4 control-label no-padding-right" id="TxtWahtHappendOnLabel" for="TxtAuditoryPlanning"><%=this.Dictionary["Item_Incident_Field_WhatHappenedDate"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input style="width:90px;" class="form-control date-picker" id="TxtWahtHappendOn" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ClosedOn) %>" />
                                            <span id="TxtWahtHappendOnLabelBtn" class="input-group-addon" onclick="document.getElementById('TxtWahtHappendOn').focus();">
                                                <i class="icon-calendar bigger-110"></i>
                                            </span>
                                        </div>
                                        <span class="ErrorMessage" id="TxtWahtHappendOnErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtWahtHappendOnErrorDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                        <span class="ErrorMessage" id="TxtWahtHappendOnErrorCross"><%=this.Dictionary["Item_Auditory_ValidateCrossDate"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="CloseCuestionarioPopup" class="hide">
                                <div class="row" style="margin-top:4px;"> 
                                    <label class="col-sm-4 control-label no-padding-right" id="TxtCloseCuestionarioLabel" for="TxtAuditoryPlanning"><%=this.Dictionary["Item_Auditory_Label_ClosedOn"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input style="width:90px;" class="form-control date-picker" id="TxtCloseCuestionario" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                            <span id="TxtCloseCuestionarioBtn" class="input-group-addon" onclick="document.getElementById('TxtCloseCuestionario').focus();">
                                                <i class="icon-calendar bigger-110"></i>
                                            </span>
                                        </div>
                                        <span class="ErrorMessage" id="TxtTxtCloseCuestionarioErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtTxtCloseCuestionarioErrorDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                        <span class="ErrorMessage" id="TxtTxtCloseCuestionarioErrorCross"><%=this.Dictionary["Item_Auditory_ValidateCuestionarioCrossDate"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="ClosedPopup" class="hide">
                                <div class="row"> 
                                    <label class="col-sm-4 control-label no-padding-right" id="CmbClosedByLabel" for="CmbClosedBy"><%=this.Dictionary["Item_Auditory_Label_ClosedBy"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <select id="CmbClosedBy" class="form-control col-xs-12 col-sm-12"></select>
                                        <input style="display:none;" type="text" readonly="readonly" id="CmbClosedByValue" class="col-xs-12 col-sm-12" />
                                        <span class="ErrorMessage" id="CmbClosedByErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="row" style="margin-top:4px;"> 
                                    <label class="col-sm-4 control-label no-padding-right" id="TxtClosedOnLabel" for="TxtAuditoryPlanning"><%=this.Dictionary["Item_Auditory_Label_ClosedOn"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input style="width:90px;" class="form-control date-picker" id="TxtClosedOn" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ClosedOn) %>" />
                                            <span id="TxtClosedOnLabelBtn" class="input-group-addon" onclick="document.getElementById('TxtClosedOn').focus();">
                                                <i class="icon-calendar bigger-110"></i>
                                            </span>
                                        </div>
                                        <span class="ErrorMessage" id="TxtClosedOnErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtClosedOnErrorDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                        <span class="ErrorMessage" id="TxtClosedOnErrorCross"><%=this.Dictionary["Item_Auditory_ValidateCrossDate"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="ReopenPopup" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_Popup_ReopenMessage"] %>&nbsp;<strong><span id="AuditoryPlanningName"></span></strong>?</p>
                            </div>

                            <div id="NoActionsPopup" class="hide"">
                                <p><span id="NoActionsPopupMessage"></span></p>
                            </div>

                            <div id="ValidationPopup" class="hide">
                                <div class="row"> 
                                    <label class="col-sm-4 control-label no-padding-right" id="CmbValidatedByLabel" for="CmbValidatedBy"><%=this.Dictionary["Item_Auditory_Label_ValidatedBy"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <select id="CmbValidatedBy" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtValidatedByList"></asp:Literal></select>
                                        <input style="display:none;" type="text" readonly="readonly" id="CmbValidatedByValue" class="col-xs-12 col-sm-12" />
                                        <span class="ErrorMessage" id="CmbValidatedByErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="row" style="margin-top:4px;"> 
                                    <label class="col-sm-4 control-label no-padding-right" id="TxtValidatedOnLabel" for="TxtAuditoryPlanning"><%=this.Dictionary["Item_Auditory_Label_PlanningDate"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input style="width:90px;" class="form-control date-picker" id="TxtValidatedOn" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ValidatedOn) %>" />
                                            <span id="TxtValidatedOnLabelBtn" class="input-group-addon" onclick="document.getElementById('TxtValidatedOn').focus();">
                                                <i class="icon-calendar bigger-110"></i>
                                            </span>
                                        </div>
                                        <span class="ErrorMessage" id="TxtValidatedOnErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtValidatedOnErrorDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                        <span class="ErrorMessage" id="TxtValidatedOnErrorCross"><%=this.Dictionary["Item_Auditory_ValidateCrossDate"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="CustionariosReoenDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_Message_ReopenCuestionario"] %>?</p>
                            </div>

                            <div id="AuditoryPlanningDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_AuditoryPlanning_PopupDelete_Message"] %>&nbsp;<strong><span id="AuditoryPlanningName"></span></strong>?</p>
                            </div>

                            <div id="PopupPlanningDialog" class="hide" style="width:400px;overflow:hidden;">    
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id="CmbProcessLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Process"] %><span class="required">*</span></label>
                                    <div class="col-sm-10">
                                        <select id="CmbProcess" class="col-xs-12 col-sm-12"><asp:Literal runat="server" ID ="LtProcessList"></asp:Literal></select>
                                        <span class="ErrorMessage" id="CmbProcessErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>                            
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id="TxtPlanningDateLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Date"] %><span class="required">*</span></label>
                                    <div class="col-sm-5">
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
                                    <label id ="TxtHourLabel" class="col-sm-2 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_AuditoryPlanning_Field_Hour"] %><span class="required">*</span></label>
                                    <div class="col-sm-3">
                                        <div class="row">
                                            <div class="col-xs-10 col-sm-10 tooltip-info" id="TxtPlanningTimeDiv">
                                                <div class="input-group">
                                                    <input class="form-control timepicker" id="TxtHour" type="text" maxlength="10" />
                                                    <span id="TxtHourBtn" class="input-group-addon" onclick="document.getElementById('TxtHour').focus();">
                                                        <i class="icon-time bigger-110"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <span class="ErrorMessage" id="TxtHourRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>    
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id ="TxtDurationLabel" class="col-sm-2 control-label no-padding-right" for="TxtDuration"><%=this.Dictionary["Item_AuditoryPlanning_Field_Duration"] %><span class="required">*</span></label>
                                    <div class="col-sm-10">
                                        <input type="text" class="col-xs-2 col-sm-2 integerFormated" id="TxtDuration" placeholder="<%=this.Dictionary["Item_AuditoryPlanning_Field_Duration"] %>" value="" maxlength="12" />
                                        <span class="ErrorMessage" id="TxtDurationRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="TxtDurationMalformed"><%=this.Dictionary["Common_Error_MoneyMalformed"] %></span>
                                    </div>
                                </div> 
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id="CmbAuditorLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Auditor"] %><span class="required">*</span></label>
                                    <div class="col-sm-10">
                                        <select id="CmbAuditor" class="col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtAuditorList"></asp:Literal></select>
                                        <span class="ErrorMessage" id="CmbAuditorErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <label id="CmbAuditedLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_Audited"] %><span class="required">*</span></label>
                                    <div class="col-sm-10">
                                        <select id="CmbAudited" class="col-xs-12 col-sm-12"><asp:Literal runat="server" ID ="LtAuditedList"></asp:Literal></select>
                                        <span class="ErrorMessage" id="CmbAuditedErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="CmbAuditedErrorSame"><%=this.Dictionary["Item_Auditory_Planning_Message"] %></span>
                                    </div>
                                </div>
                                <div class="col-sm-12" style="margin-top:4px;">
                                    <div class="col-sm-1"><input type="checkbox" id="ChkSendMail" onclick="ChkSendMailChanged();" /></div>
                                    <label id="ChkSendMailLabel" class="col-sm-11 control-label no-padding-right"><%=this.Dictionary["Item_AuditoryPlanning_Label_SendMail"] %></label>                                    
                                </div>     
                                <div class="col-sm-12" style="margin-top:4px;display:none;" id="TxtProviderEmailRow">
                                    <label id ="TxtProviderEmailLabel" class="col-sm-2 control-label no-padding-right" for="TxtProviderEmail"><%=this.Dictionary["Item_AuditoryPlanning_Label_EmailProvider"] %><span class="required">*</span></label>
                                    <div class="col-sm-10">
                                        <input type="text" class="col-xs-12 col-sm-12" id="TxtProviderEmail" placeholder="<%=this.Dictionary["Item_AuditoryPlanning_Label_EmailProvider"] %>" value="" maxlength="150" />
                                        <span class="ErrorMessage" id="TxtProviderEmailErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        <span class="ErrorMessage" id="TxtProviderEmailMalformed"><%=this.Dictionary["Common_MessageMailMalformed"]%></span>
                                    </div>
                                </div> 
                            </div>

                            <!-- Report popups -->
                            
                            <div id="foundDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_PopupDelete_MessageFound"] %>&nbsp;<strong><span id="foundName"></span></strong>?</p>
                            </div>

                            <div id="improvementDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_PopupDelete_MessageImprovement"] %>&nbsp;<strong><span id="imporvementName"></span></strong>?</p>
                            </div>

                            <div id="PopupFoundDialog" class="hide" style="width:800px;overflow:hidden;">
                                <div class="row" style="clear:both;margin-bottom:8px;">
                                    <label id ="TxtTextLabel" class="col-sm-12 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Found"] %></label>
                                    <div class="col-sm-12">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtText"></textarea>
                                        <span class="ErrorMessage" id="TxtTextErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="row" style="clear:both;margin-bottom:8px;">
                                    <label id ="TxtRequerimentLabel" class="col-sm-12 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Requeriment"] %></label>
                                    <div class="col-sm-12">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtRequeriment"></textarea>
                                        <span class="ErrorMessage" id="TxtRequerimentErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="row" style="clear:both;margin-bottom:8px;">
                                    <label id ="TxUnconformityLabel" class="col-sm-12 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Unconformity"] %></label>
                                    <div class="col-sm-12">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtUnconformity"></textarea>
                                        <span class="ErrorMessage" id="TxtUnconformityErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="row" style="clear:both;margin-bottom:8px;">
                                    <div class="col-sm-12">
                                        <input type="checkbox" id="ChkActionFound" />
                                        &nbsp;
                                    <label id ="TxtActionLabel" class="control-label" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Action"] %>
                                    </label>
                                </div>
                            </div>

                            <div id="PopupImprovementDialog" class="hide" style="width:600px;overflow:hidden;">
                                <div class="row" style="clear:both;margin-bottom:8px;">
                                    <label id ="TxtImprovementTextLabel" class="col-sm-12 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Improvement"] %></label>
                                    <div class="col-sm-12">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtImprovement"></textarea>
                                        <span class="ErrorMessage" id="TxtImprovementErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="row" style="clear:both;">
                                    <div class="col-sm-12">
                                        <input type="checkbox" id="ChkActionImprovement" />
                                        &nbsp;
                                        <label id ="TxtActionLabel" class="control-label" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Action"] %>
                                        </label>
                                </div>
                            </div>
    
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
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <!-- Popups BAR -->                                
                            <%=this.ProviderBarPopups.Render %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.timepicker.js"></script>
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/AuditoryView.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>