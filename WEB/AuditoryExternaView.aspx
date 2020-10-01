<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="AuditoryExternaView.aspx.cs" Inherits="AuditoryExternaView" %>

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
            cursor: default;
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
        
        .current a       { background: hsla(230, 79%, 69%, 1); color:#fff; }
        .current a:after { border-left: 30px solid hsla(230, 79%, 69%,1); }
        
        .past0 a { background-color:#6fb3e0; color:#000;}
        .past0 a:after { border-left: 30px solid #6fb3e0; }
        .past1 a { background-color:#ff0 ;color:#000;}
        .past1 a:after { border-left: 30px solid #ff0; }
        .past2 a { background-color:#ffb752; color:#000;}
        .past2 a:after { border-left: 30px solid #ffb752; }
        .past3 a { background-color:#d15b47; color:#000;}
        .past3 a:after { border-left: 30px solid #d15b47; }
        .past4 a { background-color:#87b87f; color:#000;}
        .past4 a:after { border-left: 30px solid #87b87f; }
        .past5 a { background-color:#555; color:#000;}
        .past5 a:after { border-left: 30px solid #555; }

        .future a       { background: hsla(230, 37%, 86%, 1); }
        .future a:after { border-left: 30px solid hsla(230, 37%, 86%,1); }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var Auditory = <%=this.Auditory.Json %>;
        var Zombies = <%=this.Zombie %>;
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
                            <div style="margin-top:-12px;">
                                <ul class="breadcrumbWizard">
                                    <li style="cursor:default;" class="<%= this.Auditory.Status > -1 ? "past0" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_0"] %></a></li>
                                    <li style="cursor:default;" class="<%= this.Auditory.Status > 0 ? "past1" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_1"] %></a></li>
                                    <li style="cursor:default;" class="<%= this.Auditory.Status > 2 ? "past3" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_3"] %></a></li>
                                    <li style="cursor:default;" class="<%= this.Auditory.Status > 3 ? "past5" : "future" %>"><a href="#0"><%=this.Dictionary["Item_Adutory_Status_Label_5"] %></a></li>
                                </ul>
                            </div>
                            <div style="margin-top:30px;">
                                <div id="user-profile-2" class="user-profile">
                                    <div class="tabbable">
                                        <% if (!this.NewAuditory)
                                            { %>
                                        <ul class="nav nav-tabs padding-18">
                                            <li class="active">
                                                <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Auditory_Tab_Principal"]%></a>
                                            </li>
                                            <% if (Auditory.Type != 1)
                                                { %>
                                            <li class="" <% if (this.Auditory.Status < 1) { %>style="display:none;" <% } %>>
                                                <a data-toggle="tab" href="#questionaries" id="tabQuestionaries"><%=this.Dictionary["Item_Auditory_Tab_Questionaries"]%></a>
                                            </li>
                                            <li class="" <% if (this.Auditory.Status < 1) { %>style="display:none;" <% } %>>
                                                <a data-toggle="tab" href="#report" id="TabReport"><%=this.Dictionary["Item_Auditory_Tab_Report"]%></a>
                                            </li>
                                            <% } else { %>
                                            <li class="" <% if (this.Auditory.Status < 1) { %>style="display:none;" <% } %>>
                                                <a data-toggle="tab" href="#reportExternal" id="TabReportExternal"><%=this.Dictionary["Item_Auditory_Tab_Report"]%></a>
                                            </li>
                                            <% } %>
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
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="TxtTypeLabel"><%=this.Dictionary["Item_Auditory_Label_Type"] %><span style="color:#f00">*</span></label>
                                                        <label class="col-sm-2 control-label" style="text-align:left;"><strong><%=this.Dictionary["Item_Adutory_Type_Label_" + this.Auditory.Type.ToString()] %></strong></label>
                                                        <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Auditory_Label_Name"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-8">
                                                            <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Auditory_Label_Name"] %>" class="col-xs-12 col-sm-12" value="<%=this.Auditory.Description %>" />
                                                            <span class="ErrorMessage" id="TxtNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div> 
                                                    <div class="form-group">
                                                        <div class="col-sm-2"><input type="radio" id="RBProvider" name="RBExternalType" onclick="RBExternalTypeChanged();" />&nbsp;<%=this.Dictionary["Item_Provider"] %></div>
                                                        <div class="col-sm-2"><input type="radio" id="RBCustomer" name="RBExternalType" onclick="RBExternalTypeChanged();" />&nbsp;<%=this.Dictionary["Item_Customer"] %></div>
                                                        <div class="col-sm-3" id="ProviderDiv" style="display:none;">
                                                            <select id="CmbProvider" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtCmbProvider"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="CmbProviderValue" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="CmbProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <div class="col-sm-1" id="CmbProviderBar" style="display:none;"><button type="button" class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnProviderBAR">...</button></div>                                                        
                                                        <div class="col-sm-3" id="CustomerDiv" style="display:none;">
                                                            <select id="CmbCustomer" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtCmbCustomer"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="CmbCustomerValue" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="CmbCustomerErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <div class="col-sm-1" id="CmbCustomerBar" style="display:none;"><button type="button" class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Customers"] %>" id="BtnCustomerBAR">...</button></div>                                                        
                                                        <div class="col-sm-3" id="ErrorProviderCustomerDiv" style="display:none;">
                                                            <span class="ErrorMessage" id="ProviderCustomerErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-1" id="TxtRulesIdLabel" for="CmbRules"><%=this.Dictionary["Item_Auditory_Label_Rules"] %><span style="color:#f00">*</span></label>
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
                                                        <label id="TxtPreviewDateLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Auditory_Label_PannedDate"] %><span class="required">*</span></label>
                                                            <div class="col-sm-2">
                                                                <div class="row">
                                                                    <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtPreviewDateDiv">
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker" id="TxtPreviewDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%=string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyy}", this.Auditory.PreviewDate) %>" />
                                                                            <span id="TxtPreviewDateBtn" class="input-group-addon" onclick="document.getElementById('TxtPreviewDate').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                        <span class="ErrorMessage" id="TxtPlannedDateErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                                        <span class="ErrorMessage" id="TxtPlannedDateErrorMailMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                    </div>
                                                                </div>
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
                                                        <label class="col-sm-1" id="CmbAddressLabel" for="CmbAddress"><%=this.Dictionary["Item_Auditory_Label_Address"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-11">
                                                            <select id="CmbAddress" class="col-xs-12 col-sm-12 tooltip-info">
                                                                <option value="-1"><%=this.Dictionary["Common_SelectOne"] %></option>
                                                                <asp:Literal runat="server" ID="LtCmbAddress"></asp:Literal>
                                                            </select>
                                                            <span class="ErrorMessage" id="CmbAddressErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="TxtAuditorTeamLabel" class="col-sm-1"><%=this.Dictionary["Item_Auditory_Label_AuditorTeam"]%><span class="required">*</span></label>
                                                        <div class="col-sm-11">
                                                            <textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtAuditorTeam"><%=this.Auditory.AuditorTeam %></textarea>
                                                            <span class="ErrorMessage" id="TxtAuditorTeamErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="TxtNotesLabel" class="col-sm-1 no-padding-right"><%=this.Dictionary["Item_Auditory_Label_Notes"]%></label>
                                                        <div class="col-sm-11">
                                                            <textarea rows="3" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtNotes"><%=this.Auditory.Notes %></textarea>
                                                        </div>
                                                    </div>                                                    
                                                    <%=this.FormFooter %>
                                                </div>
                                            </div>
                                            <div id="reportExternal" class="tab-pane">
                                                <div class="form-horizontal" role="form">
                                                    <div class="row">
                                                        <label class="col col-sm-1" for="TxtReportStart"><%=this.Dictionary["Item_Auditory_Report_StartLabel"] %></label>
                                                        <div class="col col-sm-2">
                                                            <div class="input-group">
                                                                <input style="width:90px;" class="form-control date-picker" id="TxtStartQuestionsOn" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ReportStart) %>" />
                                                                <span id="TxtReportStartBtn" class="input-group-addon" onclick="document.getElementById('TxtStartQuestionsOn').focus();">
                                                                    <i class="icon-calendar bigger-110"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <label class="col col-sm-1" for="TxtReportEnd"><%=this.Dictionary["Item_Auditory_Report_EndLabel"] %></label>
                                                        <div class="col col-sm-2">
                                                            <div class="input-group">
                                                                <input style="width:90px;" class="form-control date-picker" id="TxtCloseQuestionsOn" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ReportEnd) %>" />
                                                                <span id="TxtReportEndBtn" class="input-group-addon" onclick="document.getElementById('TxtCloseQuestionsOn').focus();">
                                                                    <i class="icon-calendar bigger-110"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <button type="button" class="btn btn-success" id="BtnCloseAuditoria" style="display:none;padding-top:0;height:30px;"><i class="fa fa-check"></i>&nbsp;<%=this.Dictionary["Item_Auditory_Btn_Validation"] %></button>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" id="TableIncidentActionsHeader">
                                                    <div class="col-sm-6"><h4><%=this.Dictionary["Item_IncidentActions"] %></h4></div>
                                                    <div class="col-sm-6" style="text-align:right;">
                                                        
                                                    </div>
                                                </div>

                                                <div class="table-responsive" id="scrollTableDivIncidentActions">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderIncidentActions">
			                                                    <th style="width:120px;"><%=this.Dictionary["Item_IncidentAction_Header_Type"] %></th>
			                                                    <th><%=this.Dictionary["Item_IncidentAction_Header_Description"] %></th>
			                                                    <th style="width:120px;"><%=this.Dictionary["Item_IncidentAction_Header_Open"] %></th>
			                                                    <th style="width:107px;">
                                                                    <button type="button" id="BtnActionAdd" class="btn btn-success" style="margin-top:6px;height:28px;padding-top:0;float:right;"><i class="fa fa-plus"></i>&nbsp;<%=this.Dictionary["Item_Auditory_Btn_AddIncidentAction"] %></button>
			                                                    </th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivIncidentActions" style="display:none;overflow:scroll;overflow-x:hidden; padding: 0;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="IncidentActionsDataTable"><asp:Literal runat="server" ID="Literal1"></asp:Literal></tbody>
                                                        </table>
                                                    </div>
                                                    <div id="NoDataIncidentActions" style="width:100%;height:99%;min-height:200px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterIncidentActions">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanIncidentActionsTotal">0</span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->
                                                
                                                <div class="alert alert-info" style="display:none;" id="DivAcionsZombies">
                                                    <strong><i class="icon-info-sign"></i></strong>
                                                    <div style="display:inline;"><%=this.Dictionary["Item_Auditory_Message_ActionsZombie"] %></div>
                                                </div>

                                                <div class="table-responsive" id="scrollTableDivIncidentActionsReal" style="margin-top:4px;border-left:1px solid #ddd;">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderIncidentActionsReal">
			                                                    <th style="width:90px;"><%=this.Dictionary["Item_IncidentAction_Header_Status"] %></th>
			                                                    <th style="width:120px;"><%=this.Dictionary["Item_IncidentAction_Header_Type"] %></th>
			                                                    <th><%=this.Dictionary["Item_IncidentAction_Header_Description"] %></th>
			                                                    <th style="width:100px;"><%=this.Dictionary["Item_IncidentAction_Header_Open"] %></th>
			                                                    <th style="width:67px;">&nbsp;</th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivIncidentActionsReal" style="display:none;overflow:scroll;overflow-x:hidden; padding: 0;min-height:200px;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="IncidentActionsDataTableReal"><asp:Literal runat="server" ID="LtActionsData"></asp:Literal></tbody>
                                                        </table>
                                                    </div>
                                                    <!-- <div id="NoDataIncidentActionsReal" style="width:100%;height:99%;min-height:200px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>-->
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterIncidentActionsReal">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanIncidentActionsTotalReal"><asp:Literal runat="server" ID="LtActionsDataCount"></asp:Literal></span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->

                                                <div class="col-sm-12 alert alert-info" style="display:none;margin-top:12px;margin-bottom:40px;" id="DivClosedResume">
                                                    <div class="col-sm-6">
                                                    <strong><i class="icon-info-sign fa-2x"></i></strong>
                                                    <h3 style="display:inline;"><%=this.Dictionary["Item_Auditory_Message_Closed"] %></h3>
                                                        <p style="margin-left:50px;">
                                                            <%=this.Dictionary["Item_Auditory_Label_ClosedOn"] %>:&nbsp;<span id="SpanClosedOn" style="font-weight:bold;"><%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ClosedOn) %></span>
                                                            <br />
                                                            <%=this.Dictionary["Item_Auditory_Label_ClosedBy"] %>:&nbsp;<span id="SpanClosedBy" style="font-weight:bold;"><%=this.Auditory.ClosedBy.UserName %></span>
                                                        </p>
                                                    </div>
                                                    <!--<div class="col-sm-6" style="text-align:right;">
                                                        <button type="button" class="btn btn-success" id="BtnReopenAuditoria"><i class="fa fa-recycle"></i>&nbsp;<%=this.Dictionary["Item_Auditory_Popup_ReopenTitle"] %></button>
                                                    </div>-->
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

                            <div id="ClosedPopup" class="hide">
                                <div class="row"> 
                                    <label class="col-sm-4 control-label no-padding-right" id="CmbClosedByLabel" for="CmbClosedBy"><%=this.Dictionary["Item_Auditory_Label_ClosedBy"] %><span class="required">*</span></label>
                                    <div class="col-sm-8">
                                        <select id="CmbClosedBy" class="form-control col-xs-12 col-sm-12"><asp:Literal runat="server" ID="LtClosedByList"></asp:Literal></select>
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

                            <div id="NoActionsPopup" class="hide">
                                <p><span id="NoActionsPopupMessage"><%=this.Dictionary["Item_Adutory_Message_NoActions"] %></span></p>
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

                            <!-- Report popups -->
    
                            <div id="PopupUploadFile" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <form action="/dummy.html" class="dropzone well dz-clickable" id="dropzone">
                                        <input type="file" id="fileName" name="fileName" multiple="multiple" style="position:absolute;top:-100000px;"/>
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
                            <%=this.CustomerBarPopups.Render %>

    
                            <div id="IncidentActionDeleteDialog" class="hide" style="width:500px;">
                                <p>&nbsp;<strong><span id="IncidentActionDeleteName"></span></strong>?</p>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.timepicker.js"></script>
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/AuditoryExternaView.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>