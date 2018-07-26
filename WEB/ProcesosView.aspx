<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ProcesosView.aspx.cs" Inherits="ProcesosView" %>

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
    <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
    <script type="text/javascript">
        var process = <%=this.Proceso.Json %>;
        var processList = [<%=this.ProcesosListJson %>];
        var processTypeSelected = process.ProcessType;
        var jobPositionSelected = process.JobPosition.Id;
        var typeItemId = 9;
        var itemId = <%= this.Proceso.Id %>;
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
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active" id="TabHome">
                                                    <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Process_Tab_Principal"] %></a>
                                                </li>
                                                <li id="tabIndicators">                                                    
                                                    <a data-toggle="tab" href="#indicators"><%=this.Dictionary["Item_Process_Tab_Indicators"] %></a>
                                                </li>
                                                <% if (this.Proceso.Id > 0) { %>
                                                <li id="tabAttachments">                                                    
                                                    <a data-toggle="tab" href="#uploadFiles"><%=this.Dictionary["Item_IncidentAction_Tab_UploadFiles"] %></a>
                                                </li>
                                                <% } %>
                                            </ul>
                                            <div class="tab-content no-border padding-24">
                                                <div id="home" class="tab-pane active">                                                
                                                    <form class="form-horizontal" role="form">
                                                        <div class="form-group">
                                                            <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_Process_FieldLabel_Name"] %><span style="color:#f00">*</span></label>
                                                            <%=this.TxtName %>
                                                        </div>
                                                        <div class="form-group">                                                             <label id="TxtProcessTypeLabel" class="col-sm-1 control-label no-padding-right" id="Label2"><%=this.Dictionary["Item_Process_FieldLabel_Type"] %><span style="color:#f00">*</span></label>
                                                            <div class="col-sm-3" id="DivCmbTipo" style="height:35px !important;">
                                                                <select id="CmbTipo" onchange="CmbTipoChanged();" class="col-xs-12 col-sm-12"></select>
                                                                <input style="display:none;" readonly="readonly" type="text" id="TxtProcessType" placeholder="<%=this.Dictionary["Item_Process_FieldLabel_Type"] %>" class="col-xs-12 col-sm-12" value="" />
                                                                <span class="ErrorMessage" id="TxtProcessTypeErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <div class="col-sm-1"><span class="btn btn-light" style="height:30px;" id="BtnSelectProcessType" title="<%=this.Dictionary["Item_Process_Button_ProcessTypeBAR"] %>">...</span></div>
                                                        
                                                        
                                                            <label id="TxtJobPositionLabel" class="col-sm-2 control-label no-padding-right" id="Label1"><%=this.Dictionary["Item_Process_FieldLabel_Responsible"] %><span style="color:#f00">*</span></label>
                                                            <div class="col-sm-5" id="DivCmbJobPosition" style="height:35px !important;">
                                                                <select id="CmbJobPosition" onchange="CmbJobPositionChanged();" class="col-xs-12 col-sm-12"></select>
                                                                <input style="display:none;" readonly="readonly" type="text" id="TxtJobPosition" placeholder="<%=this.Dictionary["Item_Process_FieldLabel_Responsible"] %>" class="col-xs-12 col-sm-12" value="<%=this.Proceso.JobPosition.Description %>" />
                                                                <span class="ErrorMessage" id="TxtJobPositionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>	
                                                        <div class="form-group">
                                                            <label class="col-sm-12">1.- <%=this.Dictionary["Item_Process_FieldLabel_Start"] %></label>
                                                            <div class="col-sm-12"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtInicio"><%=this.Proceso.Start %></textarea></div>
                                                            <!--div class="col-sm-12">&nbsp;</div-->
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-12">2.- <%=this.Dictionary["Item_Process_FieldLabel_Development"] %></label>
                                                            <div class="col-sm-12"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtDesarrollo"><%=this.Proceso.Work %></textarea></div>
                                                            <!--div class="col-sm-12">&nbsp;</div-->
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-12">3.- <%=this.Dictionary["Item_Process_FieldLabel_End"] %></label>
                                                            <div class="col-sm-12"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtFinalizacion"><%=this.Proceso.End %></textarea></div>
                                                            <!--div class="col-sm-12">&nbsp;</div-->
                                                        </div>  
															  
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
			                                                        <th style="width:107px;">&nbsp;</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="<%=this.IndicadoresDataTotal.Text != "0" ? "" : "display:none;" %>border-top: none;margin:0;">
                                                                <tbody id="ListDataTable"><asp:Literal runat="server" ID="IndicatorsData"></asp:Literal></tbody>
                                                            </table>
                                                            <table id="IndicadoresTableVoid" style="<%=this.IndicadoresDataTotal.Text == "0" ? "" : "display:none;" %>width:100%;margin:0;height:100%;">
                                                                <tr>
                                                                    <td style="color:#434382;background-color:#ccccff;">
                                                                        <div style="width:100%;text-align:center;">
                                                                            <span><i class="icon-info-sign" style="font-size:24px;"></i></span>        
                                                                            <span style="font-size:20px;"><%=this.Dictionary["Item_Process_NoIndicators"] %></span>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="IndicadoresDataTotal"></asp:Literal></i></th>
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
                                <span class="ErrorMessage" id="TxtProcessTypeNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtProcessTypeNameErrorDuplicated"><%=this.Dictionary["Common_AlreadyExists"] %></span>
                            </div>
                            <div id="ProcessTypeInsertDialog" class="hide" style="width:600px;">
                                <p><%=this.Dictionary["Item_Process_FieldLabel_Name"] %>&nbsp;&nbsp;<input type="text" id="TxtProcessTypeNewName" size="50" placeholder="<%= this.Dictionary["Item_Process_FieldLabel_Name"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" /></p>
                                <span class="ErrorMessage" id="TxtProcessTypeNewNameErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtProcessTypeNewNameErrorDuplicated"><%= this.Dictionary["Common_AlreadyExists"] %></span>
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
        <script type="text/javascript" src="/js/ProcessType.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/ProcesosView.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>