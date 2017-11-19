<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ProcesosView.aspx.cs" Inherits="ProcesosView" %>

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
    <script type="text/javascript" src="js/common.js"></script>
    <script type="text/javascript">
    var process = <%=this.Proceso.Json %>;
    var processList = [<%=this.ProcesosListJson %>];
    var processTypeSelected = process.ProcessType;
    var jobPositionSelected = process.JobPosition.Id;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active" id="TabHome">
                                                    <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Process_Tab_Principal"] %></a>
                                                </li>
                                                <li id="tabIndicators">                                                    
                                                    <a data-toggle="tab" href="#indicators"><%=this.Dictionary["Item_Process_Tab_Indicators"] %></a>
                                                </li>
                                                <% if (this.Admin)
                                                   { %>
                                                <li class="" id="TabTrazas">
                                                    <a data-toggle="tab" href="#trazas"><%=this.Dictionary["Item_Process_Tab_Traces"]%></a>
                                                </li>
                                                <% } %>
                                            </ul>
                                            <div class="tab-content no-border padding-24">
                                                <div id="home" class="tab-pane active">                                                
                                                    <form class="form-horizontal" role="form">
                                                        <div class="form-group">
                                                            <label id="TxtNameLabel" class="col-sm-2 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Process_FieldLabel_Name"] %></label>
                                                            <%=this.TxtName %>
                                                            <!--<div class="col-sm-7">
                                                                <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Process_FieldLabel_Name"] %>" class="col-xs-12 col-sm-12" value="<%=this.Proceso.Description.Replace("\"","\\\"") %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                                                <span class="ErrorMessage" id="TxtNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                                <span class="ErrorMessage" id="TxtNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                                                            </div>-->
                                                        </div>
                                                        <div class="form-group">                                                                
                                                            <label id="TxtJobPositionLabel" class="col-sm-2 control-label no-padding-right" id="Label1"><%=this.Dictionary["Item_Process_FieldLabel_Responsible"] %></label>
                                                            <div class="col-sm-7" id="DivCmbJobPosition" style="height:35px !important;">
                                                                <select id="CmbJobPosition" onchange="CmbJobPositionChanged();" class="col-xs-12 col-sm-12"></select>
                                                                <input style="display:none;" readonly="readonly" type="text" id="TxtJobPosition" placeholder="<%=this.Dictionary["Item_Process_FieldLabel_Responsible"] %>" class="col-xs-12 col-sm-12" value="<%=this.Proceso.JobPosition.Description %>" />
                                                                <span class="ErrorMessage" id="TxtJobPositionErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <!-- <div class="col-sm-1"><span class="btn btn-light" style="height:30px;" id="BtnSelectJobPosition" title="<%=this.Dictionary["Item_Process_SelectJobPosition"] %>">...</span></div> -->
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtProcessTypeLabel" class="col-sm-2 control-label no-padding-right" id="Label2"><%=this.Dictionary["Item_Process_FieldLabel_Type"] %></label>
                                                            <div class="col-sm-7" id="DivCmbTipo" style="height:35px !important;">
                                                                <select id="CmbTipo" onchange="CmbTipoChanged();" class="col-xs-12 col-sm-12"></select>
                                                                <input style="display:none;" readonly="readonly" type="text" id="TxtProcessType" placeholder="<%=this.Dictionary["Item_Process_FieldLabel_Type"] %>" class="col-xs-12 col-sm-12" value="" />
                                                                <span class="ErrorMessage" id="TxtProcessTypeErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <div class="col-sm-1"><span class="btn btn-light" style="height:30px;" id="BtnSelectProcessType" title="<%=this.Dictionary["Item_Process_Button_ProcessTypeBAR"] %>">...</span></div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-2 control-label no-padding-right">1.- <%=this.Dictionary["Item_Process_FieldLabel_Start"] %></label>
                                                            <div class="col-sm-7"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtInicio"><%=this.Proceso.Start %></textarea></div>
                                                            <div class="col-sm-12">&nbsp;</div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-2 control-label no-padding-right">2.- <%=this.Dictionary["Item_Process_FieldLabel_Development"] %></label>
                                                            <div class="col-sm-7"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtDesarrollo"><%=this.Proceso.Work %></textarea></div>
                                                            <div class="col-sm-12">&nbsp;</div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-2 control-label no-padding-right">3.- <%=this.Dictionary["Item_Process_FieldLabel_End"] %></label>
                                                            <div class="col-sm-7"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtFinalizacion"><%=this.Proceso.End %></textarea></div>
                                                            <div class="col-sm-12">&nbsp;</div>
                                                        </div>  
                                                        <br />
                                                        <%=this.FormFooter %>
                                                    </form>
                                                </div>
                                                <div id="indicators" class="tab-pane">
                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeader">
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_Process_ListIndicatorsHeader_Name"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th1" class="search sort" style="cursor:pointer;width:120px;"><%=this.Dictionary["Item_Process_ListIndicatorsHeader_Meta"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th2" class="search sort" style="cursor:pointer;width:120px;"><%=this.Dictionary["Item_Process_ListIndicatorsHeader_Alarm"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th3" class="search sort" style="cursor:pointer;width:150px;"><%=this.Dictionary["Item_Process_ListIndicatorsHeader_Unit"] %></th>
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th4" class="search sort" style="cursor:pointer;width:150px;"><%=this.Dictionary["Item_Process_ListIndicatorsHeader_Responsible"] %></th>
			                                                        <th style="width:106px;">&nbsp;</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTable"><asp:Literal runat="server" ID="IndicatorsData"></asp:Literal></tbody>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="ProviderDataTotal"></asp:Literal></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div> <!-- /.table-responsive -->                                             
                                                    
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

                            <div id="dialogJobPosition" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><%=this.Dictionary["Item_JobPosition"] %></th>
                                                <th style="width:40px;" class="hidden-480">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="SelectableJobPosition">
                                        </tbody>
                                    </table>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->
                            
                            <div id="dialogProcessType" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><%=this.Dictionary["Item_Process_PopupType_Header"] %></th>
                                                <th style="width:150px;"  class="hidden-480">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="SelectableProcessType">
                                        </tbody>
                                    </table>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id="ProcessTypeDeleteDialog" class="hide" style="width:600px;">
                                <p><%=this.Dictionary["Item_ProcessType_PopupDelete_Message"] %>&nbsp;<strong><span id="ProcessTypeName"></span></strong>?</p>
                            </div>
                            <div id="ProcessTypeUpdateDialog" class="hide" style="width:600px;">
                                <p><%=this.Dictionary["Item_Process_FieldLabel_Name"] %>&nbsp;&nbsp;<input type="text" id="TxtProcessTypeName" size="50" placeholder="<%= this.Dictionary["Item_Process_FieldLabel_Name"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" /></p>
                                <span class="ErrorMessage" id="TxtProcessTypeNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtProcessTypeNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_AlreadyExists"] %></span>
                            </div>
                            <div id="ProcessTypeInsertDialog" class="hide" style="width:600px;">
                                <p><%=this.Dictionary["Item_Process_FieldLabel_Name"] %>&nbsp;&nbsp;<input type="text" id="TxtProcessTypeNewName" size="50" placeholder="<%= this.Dictionary["Item_Process_FieldLabel_Name"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" /></p>
                                <span class="ErrorMessage" id="TxtProcessTypeNewNameErrorRequired" style="display:none;"><%= this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtProcessTypeNewNameErrorDuplicated" style="display:none;"><%= this.Dictionary["Common_AlreadyExists"] %></span>
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
        <script type="text/javascript" src="js/ProcessType.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="js/ProcesosView.js?<%=this.AntiCache %>"></script>
</asp:Content>