<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="DocumentView.aspx.cs" Inherits="DocumentView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <!-- Script de la página -->
    <script type="text/javascript">
        var documento = <%=this.DocumentoJson %>;
        var documentId = <%=this.DocumentId %>;
        var companyId = <%=this.Company.Id %>;
        var userId = <%=this.ApplicationUser.Id %>;
        var userName = "<%=this.ApplicationUser.UserName %>";
        var categorias = <%= this.CategoriasJson %>;
        var procedencias = <%= this.ProcedenciasJson %>;
        var sourceSelected = documentId === -1 ? false : documento.Source ? 2 : 1;
        var categorySelected = documentId === -1 ? 0 : documento.Category.Id;
        var procedenciaSelected = documentId === -1 ? 0 : documento.Origin.Id;
        var selectedReason = "";
        var companyDocuments = [<%=this.CompanyDocuments %>];
        var firstDate = <%=this.FirstVersionDate%>;
        var attachs = <%=this.Attachs%>;
        var attachActual = <%=this.DocumentAttachActual%>;
        var pageType = "form";
        var Employees = <%=this.EmployeesJson %>;
        var requerimentDefinitions = <%=this.RequerimentDefinitionList %>;
        var requerimentActs = <%=this.RequerimentActList %>;
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
                                            <% if (this.DocumentId != -1)
                                                   { %>
                                            <ul class="nav nav-tabs padding-18">                                                
                                                <li class="active">
                                                    <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Document_Tab_Principal"]%></a>
                                                </li>
                                                <li class="" id="TabRequeriments">
                                                    <a data-toggle="tab" href="#requeriments"><%=this.Dictionary["Item_Document_Tab_Requeriments"]%></a>
                                                </li>
                                                <li class="" id="TabHistoricoSelector">
                                                    <a data-toggle="tab" href="#feed"><%=this.Dictionary["Item_Document_Tab_Versions"]%></a>
                                                </li>
                                            </ul>
                                            <% } %>
                                            <div class="tab-content no-border padding-24">
                                                <div id="home" class="tab-pane active">                                                
                                                    <form class="form-horizontal" role="form">
                                                        <div class="form-group">
                                                            <label id="TxtCodigoLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_Code"]%></label>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtCodigo" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Code"] %>" class="col-xs-12 col-sm-12 tooltip-info" maxlength="25" onblur="this.value=$.trim(this.value);" />
                                                                <span class="ErrorMessage" id="TxtCodigoErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                <span class="ErrorMessage" id="TxtCodigoErrorDuplicated"><%=this.Dictionary["Item_Document_ErrorMessage_AlreadyExists"] %></span>
                                                            </div>
                                                            <label id="TxtDocumentoLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_Name"]%></label>
                                                            <div class="col-sm-8">
                                                                <input type="text" id="TxtDocumento" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Name"] %>" class="col-xs-12 col-sm-12 tooltip-info" maxlength="100" onblur="this.value=$.trim(this.value);" />
                                                                <span class="ErrorMessage" id="TxtDocumentoErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                <span class="ErrorMessage" id="TxtDocumentoErrorDuplicated"><%=this.Dictionary["Item_Document_ErrorMessage_AlreadyExists"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtStartDateLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_CreationDate"]%></label>
                                                            <div class="col-sm-2">
                                                                <input type="text" style="display: none;" id="TxtStartDate_" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_CreationDate"] %>" class="col-xs-12 col-sm-12" readonly="readonly" />
                                                                <div class="row">
                                                                    <div class="col-xs-12 col-sm-12 tooltip-info" id="DivStartDate">
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker_start" id="TxtStartDate" type="text" data-date-format="dd/mm/yyyy" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_CreationDate"] %>" maxlength="10" />
                                                                            <span class="input-group-addon" onclick="document.getElementById('TxtStartDate').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <span class="ErrorMessage" id="TxtStartDateErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtStartDatePreviousRevision"><%=this.Dictionary["Item_Document_ErrorMessage_StartDatePreviousRevision"] %></span>
                                                                </div>
                                                            </div>
                                                            <label id="TxtRevisionDateLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_RevisionDate"]%></label>
                                                            <div class="col-sm-2">
                                                                <div class="row">
                                                                    <div class="col-xs-12 col-sm-12 tooltip-info" id="DivTxtRevisionDate">
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker_start" id="TxtRevisionDate" type="text" data-date-format="dd/mm/yyyy" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_RevisionDate"] %>" maxlength="10" />
                                                                            <span class="input-group-addon" onclick="document.getElementById('TxtRevisionDate').focus();" id="TxtRevisionDateBtn">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <span class="ErrorMessage" id="TxtRevisionDateErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtRevisionDatePreviousRevision"><%=this.Dictionary["Item_Document_ErrorMessage_StartDatePreviousRevision"] %></span>
                                                                </div>
                                                            </div>
                                                            <label id="TxtRevisionLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_Version"]%></label>
                                                            <div class="col-sm-1">
                                                                <input type="text" <% if (this.DocumentId > 0)
                                                                                      { %>readonly="readonly" <% } %>id="TxtRevision" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Version"] %>" class="col-xs-12 col-sm-12 integerFormated" value="<%= this.Document.LastVersion.Version == 0 ? string.Empty : this.Document.LastVersion.Version.ToString().Trim() %>" maxlength="4" onkeypress="validate(event)" onblur="this.value=$.trim(this.value);" />
                                                                <span class="ErrorMessage" id="TxtRevisionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <div class="cols-sm-1">
                                                                <button class="btn btn-info" type="button" id="BtnNewVersion" style="padding: 0 !important">
                                                                    <i class="icon-plus bigger-110"></i>
                                                                    <%=this.Dictionary["Item_Document_Button_NewVersion"] %>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <hr />
                                                        <% if (this.DocumentId > 0) { %>
                                                        <div class="form-group">
                                                            <label id="TxtAttachLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_RevisionAttachment"]%></label>
                                                            <div class="col-sm-1">
                                                                <button class="btn btn-success" style="padding:0 !important;" type="button" id="BtnNewUploadfileVersion" onclick="UploadFile();">
                                                                    <i class="icon-plus bigger-110"></i>
                                                                    <span id="BtnAttachText"><%=this.Dictionary["Item_DocumentAttachment_Button_New"] %></span>
                                                                </button>
                                                            </div>
                                                            <label id="ActualDocumentLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_DocumentAttachment_Description"] %>:</label>
                                                            <label id="ActualDocumentLink" class="col-sm-6" style="font-weight:bold;"></label>
                                                            <div class="col-sm-2" id="tdIconsDiv"></div>
                                                        </div>
                                                        <% } %>
                                                        <hr />
                                                        <div class="space-4"></div>
                                                        <div class="form-group">
                                                            <label id="TxtCategoryLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_Category"]%></label>
                                                            <div class="col-sm-6" id="DivCmbCategory" style="height: 35px !important;">
                                                                <select id="CmbCategory" onchange="CmbCategoryChanged();" class="col-xs-12 col-sm-12"></select>
                                                                <input style="display: none;" type="text" readonly="readonly" id="TxtCategory" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Category"] %>" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtCategoryErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <div class="col-sm-1"><span class="btn btn-light" style="height: 30px;" title="<%=this.Dictionary["Item_Document_Button_CategoryBAR"] %>" id="BtnCategory">...</span></div>
                                                        <!--/div>
                                                        <div class="space-4"></div>
                                                        <div class="form-group"-->
                                                            <label id="TxtConservacionLabel" class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_Document_FieldLabel_Conservation"] %></label>
                                                            <div class="col-sm-1">
                                                                <input type="text" class="col-xs-10 col-sm-12" id="TxtConservacion" maxlength="4" onkeypress="validate(event)" onblur="this.value=$.trim(this.value);" />
                                                                <span class="ErrorMessage" id="TxtConservacionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <div class="col-sm-2" id="DivCmbConservacion" style="height: 35px !important;">
                                                                <select class="form-control col-xs-10 col-sm-10" id="CmbConservacion" data-placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Conservation"] %>">
                                                                    <asp:Literal runat="server" ID="LtConservacion"></asp:Literal>
                                                                </select>
                                                                <span class="ErrorMessage" id="CmbConservacionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="space-4"></div>
                                                        <div class="form-group">
                                                            <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_Document_FieldLabel_Origin"] %></label>
                                                            <div class="col-sm-3" id="DivCmbOrigen" style="height: 35px !important;">
                                                                <select class="form-control col-xs-10 col-sm-10" id="CmbOrigen" data-placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Origin"] %>" onchange="OrigenChanged(this);">
                                                                    <option value="1"><%=this.Dictionary["Common_Internal"] %></option>
                                                                    <option value="2"><%=this.Dictionary["Common_External"] %></option>
                                                                </select>
                                                            </div>
                                                            <label id="TxtProcedenciaLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_Source"]%></label>
                                                            <div class="col-sm-6" id="DivCmbProcedencia" style="height: 35px !important;">
                                                                <select id="CmbProcedencia" onchange="CmbProcedenciaChanged();" class="col-xs-12 col-sm-12"></select>
                                                                <input style="display: none;" type="text" readonly="readonly" id="TxtProcedencia" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Source"] %>" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtProcedenciaErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <div class="col-sm-1"><span class="btn btn-light" style="height: 30px;" title="<%=this.Dictionary["Item_Document_Button_SourceBAR"] %>" id="BtnProcedencia">...</span></div>
                                                        </div>
                                                        <div class="space-4"></div>
                                                        <div class="form-group">
                                                            <label id="TxtUbicacionLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_Location"]%></label>
                                                            <div class="col-sm-11">
                                                                <input type="text" id="TxtUbicacion" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Location"] %>" class="col-xs-12 col-sm-12" maxlength="100" onblur="this.value=$.trim(this.value);" />
                                                                <span class="ErrorMessage" id="TxtUbicacionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="space-4"></div>
                                                        <div class="form-group">
                                                            <label id="TxtLinkFielLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Document_FieldLabel_LinkField"]%></label>
                                                            <div class="col-sm-10">
                                                                <input type="text" id="TxtLinkField" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_LinkField"]%>" class="col-xs-12 col-sm-12" maxlength="500" onblur="this.value=$.trim(this.value);DOCUMENT_LinkViewLayout();" />
                                                            </div>
                                                            <button type="button" id="BtnLinkFieldView" class="btn btn-info" style="padding: 0 !important" onclick="DOCUMENT_ViewLink();" disabled="disabled">&nbsp;<i class="fa fa-globe"></i>&nbsp;</button>
                                                        </div>
                                                        <div class="space-4"></div>
                                                        <div class="form-group">
                                                            <label id="Label5" class="col-sm-12"><%=this.Dictionary["Item_Document_FieldLabel_Reason"]%></label>
                                                            <div class="col-sm-12">
                                                                <!--input type="text" readonly="readonly" id="TxtMotivo" placeholder="<%=this.Dictionary["Item_Document_FieldLabel_Reason"] %>" class="col-xs-12 col-sm-12" value="</div>/%=this.LastVersion.Reason %>" /-->
                                                                <textarea disabled="disabled" id="TxtMotivo" class="form-control col-xs-12 col-sm-12" maxlength="500" rows="3" onblur="this.value=$.trim(this.value);"><%=this.Document.LastVersion.Reason %></textarea>
                                                                </div>
                                                        </div>
                                                    </form>

                                                </div>
                                                <div id="requeriments" class="tab-pane">
                                                    <div class="col-sm-12" style="margin-bottom:4px;">
                                                        <div class="col-sm-6"><h4><%=this.Dictionary["Item_DocumentRequerimentAct_Plural"] %></h4></div>
                                                        <div class="col-sm-6" style="text-align:right"><%=this.RequerimentNewAct.Render %></div>
                                                    </div>                                                                                                       
                                                    <table class="table table-bordered table-striped" id="DocumentRequerimentActTable">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th onclick="Sort(this,'TableDocumentRequerimentAct','date');" id="th0" class="sort" style="width:90px;"><%=this.Dictionary["Item_DocumentRequerimentAct_Header_Date"] %></th>
                                                                <th onclick="Sort(this,'TableDocumentRequerimentAct','text');" id="th1" class="sort" style="width:400px;"><%=this.Dictionary["Item_DocumentRequerimentAct_Header_Requeriment"] %></th>
                                                                <th id="th14" class=""><%=this.Dictionary["Item_DocumentRequerimentAct_Header_Observations"] %></th>	
                                                                <th id="th15" class="" style="width:200px;"><%=this.Dictionary["Item_DocumentRequerimentAct_Header_Responsible"] %></th>	
                                                                <th onclick="Sort(this,'TableDocumentRequerimentAct','money');" id="th4" class="sort" style="width:70px;"><%=this.Dictionary["Item_DocumentRequerimentAct_Header_Cost"] %></th>	
                                                                <th onclick="Sort(this,'TableDocumentRequerimentAct','date');" id="th5" class="sort" style="width:120px;"><%=this.Dictionary["Item_DocumentRequerimentAct_Header_Expiration"] %></th>
                                                                <th style="width:80px;">&nbsp;</th>														
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="7" style="padding:0;">
                                                                    <div class="scrollTable" style="width:100%;height:200px;overflow-y:scroll;">
                                                                        <table style="width:100%;">
                                                                            <tbody id="TableDocumentRequerimentAct"></tbody>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4"><div id="TableDocumentRequerimentActTotalLabel" style="width:100%"></div></td>
                                                                <td align="right" style="font-weight: bold;"><span id="TableDocumentRequerimentActTotal"></span></td>
                                                                <td colspan="2">&nbsp;</td>
                                                            </tr>
                                                        </tbody>                                                        
                                                    </table>
                                                    <div class="col-sm-12" style="margin-bottom:4px;">
                                                        <div class="col-sm-6"><h4><%=this.Dictionary["Item_DocumentRequerimentDefinition_Plural"] %></h4></div>
                                                        <div class="col-sm-6" style="text-align:right"><%=this.RequerimentNewDefinition.Render  %></div>
                                                    </div>
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th onclick="Sort(this,'TableDocumentRequerimentDefinition','text',false);" id="th0" class="sort" style="width:400px;"><%=this.Dictionary["Item_DocumentRequerimentDefinition_Header_Requeriment"] %></th>
                                                                <th onclick="Sort(this,'TableDocumentRequerimentDefinition','money',false);" id="th1" class="sort" style="width:150px;cursor:pointer;"><%=this.Dictionary["Item_DocumentRequerimentDefinition_Header_Periodicity"] %></th>	
                                                                <th><%=this.Dictionary["Item_DocumentRequerimentDefinition_Header_Actuacio"] %></th>	
                                                                <th onclick="Sort(this,'TableDocumentRequerimentDefinition','money',false);" id="th3" class="sort" style="width:90px;cursor:pointer;"><%=this.Dictionary["Item_DocumentRequerimentDefinition_Header_Cost"] %></th>	
                                                                <th style="width:80px;">&nbsp;</th>											
                                                            </tr>
                                                        </thead>
                                                        <tbody id="TableDocumentRequerimentDefinitionContainer">
                                                            <tr>
                                                                <td colspan="6" style="padding:0;">
                                                                    <div class="scrollTable" style="width:100%;height:200px;overflow-y:scroll;">
                                                                        <table style="width:100%;">
                                                                            <tbody id="TableDocumentRequerimentDefinition"></tbody>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div id="feed" class="tab-pane">
                                                        <div class="col-sm-6">
                                                            <h4><%=this.Dictionary["Item_Document_VersionAttachmentTitle"] %></h4>
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
                                                                    <th style="width:80px;"><%= this.Dictionary["Item_Document_ListRevision_RevisionNumber"] %></th>
                                                                    <th style="width:90px;"><%=this.Dictionary["Item_Document_ListRevision_Date"] %></th>
                                                                    <th><%=this.Dictionary["Item_Document_ListRevision_Reason"] %></th>
                                                                    <th style="width:120px;"><%=this.Dictionary["Item_Document"] %></th>
                                                                    <th style="width:80px;"><%= this.Dictionary["Item_Document_ListRevision_ApprovedBy"] %></th>
                                                                    <th style="width:150px;">&nbsp;</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody id="TableDocumentVersion">
                                                                <asp:Literal runat="server" ID="LtHistorico"></asp:Literal>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="attachFile" class="hide" style="width: 800px;">
                                <div class="table-responsive">
                                    <form action="/dummy.html" class="dropzone well dz-clickable" id="dropzone">
                                        <input type="file" id="fileName" name="fileName" multiple="multiple" style="position: absolute; top: -100000px;" />
                                        <div class="dz-default dz-message">
                                            <span id="UploadMessage">
                                                <span class="bigger-150 bolder">
                                                    <i class="ace-icon fa fa-caret-right red"></i>
                                                    <%=this.Dictionary["Item_DocumentAttachment_UpladTitle1"] %>
                                                </span>
                                                <%=this.Dictionary["Item_DocumentAttachment_UpladTitle2"] %>
                                                <i class="upload-icon ace-icon fa fa-cloud-upload blue fa-2x"></i>
                                            </span>
                                            <span id="UploadMessageSelected" style="display: none;">
                                                <span class="bigger-150 bolder">
                                                    <i class="ace-icon fa  icon-file-text blue">&nbsp;</i>
                                                    <span id="UploadMessageSelectedFileName"></span>
                                                </span>&nbsp;
                                                <i style="cursor: pointer;" class="ace-icon icon-ok-sign green fa-2x" onclick="ShowPreview();"></i>
                                                &nbsp;
                                                <i class="ace-icon icon-remove-sign red fa-2x" onclick="RestoreUpload();"></i>
                                            </span>
                                        </div>
                                    </form>
                                    <div class="alert alert-danger" id="AttachWarning">
                                        <i class="ace-icon icon-warning-sign fa-2x">&nbsp;</i><strong><%=this.Dictionary["Item_DocumentAttachment_ReplaceWarning"] %></strong>
                                    </div>
                                    <div class="col-sm-12">
                                        <label class="input-append col-sm-2"><%=this.Dictionary["Item_DocumentAttachment_PopupUpload_Description_Label"] %></label>
                                        <label class="input-append col-sm-10"><input class="col-sm-11" id="AttachmentFileDescription" /></label>
                                    </div>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id="dialogCategory" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><%=this.Dictionary["Item_Document_PopupCategory_Header"] %></th>
                                                <th style="width:120px;" class="hidden-480">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="CategorySelectable">
                                        </tbody>
                                    </table>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id="dialogProcedencia" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><%=this.Dictionary["Item_Document_PopupSource_Header"] %></th>
                                                <th style="width:120px;" class="hidden-480">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="ProcedenciaSelectable">
                                        </tbody>
                                    </table>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id="ReasonDialog" class="hide" style="width:350px;">
                                <p><%=this.Dictionary["Item_Document_Message_NewVersion"] %></p>
                                <div class="form-group">
                                    <%=this.Dictionary["Item_Document_FieldLabel_Reason"] %><br />
                                    <textarea id="TxtNewReason" cols="40" rows="3" onblur="this.value=$.trim(this.value);"></textarea>
                                    <span class="ErrorMessage" id="TxtNewReasonErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                </div>
                                <div class="form-group">
                                        <label id="TxtNewRevisionDateLabel" class="col-sm-3 control-label no-padding-right" for="TxtEndDate"><%=this.Dictionary["Item_Document_FieldLabel_RevisionDate"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" style="width:100px;" id="TxtNewRevision" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtNewRevisionBtn" class="input-group-addon" onclick="document.getElementById('TxtNewRevision').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                    <span class="ErrorMessage" id="TxtNewRevisionErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                    <span class="ErrorMessage" id="TxtNewRevisionMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                    <span class="ErrorMessage" id="TxtNewRevisionCrossDate"><%= this.Dictionary["Item_Document_ErrorMessage_RevisionOverDate"] %></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                            </div>
                            
                            <div id="CategoryDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_DocumentCategory_PopupDelete_Message"] %>&nbsp;<strong><span id="CategoryName"></span></strong>?</p>
                            </div>

                            <div id="CategoryUpdateDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Common_Name"] %>&nbsp;<input type="text" id="TxtCategoryName" placeholder="<%=this.Dictionary["Common_Name"] %>" onblur="this.value=$.trim(this.value);" size="50" maxlength="50" /></p>
                                <span class="ErrorMessage" id="TxtCategoryNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtCategoryNameErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>

                            <div id="CategoryInsertDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Common_Name"] %>&nbsp;<input type="text" id="TxtCategoryNewName" placeholder="<%=this.Dictionary["Common_Name"] %>" onblur="this.value=$.trim(this.value);" size="50" maxlength="50" /></p>
                                <span class="ErrorMessage" id="TxtCategoryNewNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtCategoryNewNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
                            
                            <div id="ProcedenciaDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_DocumentSource_PopupDelete_Message"] %>&nbsp;<strong><span id="ProcedenciaName"></span></strong>?</p>
                            </div>

                            <div id="ProcedenciaUpdateDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Common_Name"] %>&nbsp;<input type="text" id="TxtProcedenciaName" placeholder="<%=this.Dictionary["Common_Name"] %>" onblur="this.value=$.trim(this.value);" size="50" maxlength="50" /></p>
                                <span class="ErrorMessage" id="TxtProcedenciaNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtProcedenciaNameErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>

                            <div id="ProcedenciaInsertDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Common_Name"] %>&nbsp;<input type="text" id="TxtProcedenciaNewName" placeholder="<%=this.Dictionary["Common_Name"] %>" onblur="this.value=$.trim(this.value);" size="50" maxlength="50" /></p>
                                <span class="ErrorMessage" id="TxtProcedenciaNewNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtProcedenciaNewNameErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>

    

                            <div id="dialogAnular" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form" id="FormDialogAnular">
                                    <div class="form-group">
                                        <label id="TxtEndDateLabel" class="col-sm-3 control-label no-padding-right" for="TxtEndDate"><%=this.Dictionary["Item_Document_FieldLabel_InactiveDate"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtEndDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtEndDateBtn" class="input-group-addon" onclick="document.getElementById('TxtEndDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                    <span class="ErrorMessage" id="TxtEndDateErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                    <span class="ErrorMessage" id="TxtEndDateMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                    <span class="ErrorMessage" id="TxtEndDateCrossDate"><%= this.Dictionary["Item_Document_ErrorMessage_CrossDate"] %></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtAnularCommentsLabel" class="col-sm-3 control-label no-padding-right" for="TxtRegistroComments"><%=this.Dictionary["Item_ObjetivoRecord_FieldLabel_Reason"] %></label>
                                        <div class="col-sm-9">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtAnularComments" rows="5"></textarea>
                                            <span class="ErrorMessage" id="TxtAnularCommentsErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                </form>
                            </div>

                                
                            
                            <div id="dialogNewRequerimentDefinition" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="FormDialogNewRequerimentDefinition">
                                    <div class="form-group">
                                        <label id ="TxtNewRequerimentDefinitionRequerimentLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewRequerimentDefinitionRequeriment"><%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Requeriment"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtNewRequerimentDefinitionRequeriment" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Requeriment"] %>" rows="4" maxlength="200" onblur="this.value=$.trim(this.value);"></textarea>
                                            <span class="ErrorMessage" id="TxtNewRequerimentDefinitionRequerimentErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtNewRequerimentDefinitionRequerimentDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"]%></span>
                                        </div>
                                    </div>                             
                                    <div class="form-group">
                                        <label id ="TxtNewRequerimentDefinitionPeriodicityLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewRequerimentDefinitionPeriodicity"><%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Periodicity"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <input type="text" class="col-xs-12 col-sm-12 integerFormated" id="TxtNewRequerimentDefinitionPeriodicity" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Periodicity"] %>" value="" maxlength="8" />
                                            <span class="ErrorMessage" id="TxtNewRequerimentDefinitionPeriodicityErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>                             
                                    <div class="form-group">
                                        <label id ="TxtNewRequerimentDefinitionActuacioLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewRequerimentDefinitionActuacio"><%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Actuacio"] %></label>
                                        <div class="col-sm-9">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtNewRequerimentDefinitionActuacio" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Actuacio"] %>" rows="3" maxlength="100" onblur="this.value=$.trim(this.value);"></textarea>
                                        </div>
                                    </div>                           
                                    <div class="form-group">
                                        <!-- ISSUS-18 -->
                                        <label id ="TxtNewRequerimentDefinitionCostLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewRequerimentDefinitionCost"><%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Cost"] %></label>
                                        <div class="col-sm-9">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtNewRequerimentDefinitionCost" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Cost"] %>" value="" maxlength="8" />
                                        </div>
                                    </div> 
                                    <div class="form-group">
                                        <label id="CmbNewDefinitionResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Responisble"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                                <select id="CmbNewDefinitionResponsible" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbNewDefinitionResponsibleValue" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Responisble"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbNewDefinitionResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                </form>
                            </div>
                            
                            <div id="dialogNewRequerimentAct" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="FormDialogNewRequerimentAct">
                                    <div class="form-group">                                        
                                        <label id="CmbNewActDefinitionIdLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_DocumentAct_Popup_FieldLabel_Definition"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                                <select id="CmbNewActDefinitionId" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbNewActDefinitionIdValue" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Responisble"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbNewActDefinitionIdErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                    <div class="form-group">                                        
                                        <label id="TxtNewRequerimentActDateLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_DocumentAct_Popup_FieldLabel_Date"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtNewRequerimentActDateDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtNewRequerimentActDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtNewRequerimentActDateDateBtn" class="input-group-addon" onclick="document.getElementById('TxtNewRequerimentActDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            <span class="ErrorMessage" id="TxtNewRequerimentActDateErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtNewRequerimentActDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"]%></span>
                                            <span class="ErrorMessage" id="TxtNewRequerimentActDateOverTime">No pot ser anterior al darrer registre</span>
                                        
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id ="TxtNewRequerimentActObservationsLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewRequerimentDefinitionActuacio"><%=this.Dictionary["Item_DocumentAct_Popup_FieldLabel_Observations"] %></label>
                                        <div class="col-sm-9">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtNewRequerimentActObservations" placeholder="<%=this.Dictionary["Item_DocumentAct_Popup_FieldLabel_Observations"] %>" rows="3" maxlength="100" onblur="this.value=$.trim(this.value);"></textarea>
                                        </div>
                                    </div>                           
                                    <div class="form-group">
                                        <label id ="TxtNewRequerimentActCostLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewRequerimentDefinitionCost"><%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Cost"] %></label>
                                        <div class="col-sm-9">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtNewRequerimentActCost" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Cost"] %>" value="" maxlength="8" />
                                        </div>
                                    </div> 
                                    <div class="form-group">
                                        <label id="CmbNewActResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Responisble"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                                <select id="CmbNewActResponsible" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbNewActResponsibleValue" placeholder="<%=this.Dictionary["Item_DocumentRequeriment_Popup_FieldLabel_Responisble"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbNewActResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
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
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/DocumentView.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/DocumentRequeriment.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/Procedencia.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/Categoria.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/DocumentAttachment.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript">
            function OrigenChanged(sender) {
                sourceSelected = sender.value;
                SetOrigen();
            }

            function SetOrigen() {
                // Dejar las alertas procedencias en reset
                $("#TxtProcedenciaLabel").css("color", "#000");
                $("#TxtProcedenciaErrorRequired").hide();

                if (sourceSelected == 1) {
                    $("#TxtProcedenciaLabel").hide();
                    $("#BtnProcedencia").hide();
                    $("#CmbProcedencia").hide();
                }

                if (sourceSelected == 2) {
                    $("#TxtProcedenciaLabel").show();
                    $("#BtnProcedencia").show();
                    $("#CmbProcedencia").show();
                }
            }

            function SetReason() {                
                var ok = true;
                $("#TxtCodigoErrorDuplicated").hide();
                $("#TxtDocumentoErrorDuplicated").hide();
                $("#TxtStartDateLabel").css("color", "#000");
                $("#TxtStartDateErrorRequired").hide();
                $("#TxtStartDatePreviousRevision").hide();
                if(!RequiredFieldText("TxtCodigo")) { ok = false; }
                if(!RequiredFieldText("TxtDocumento")) { ok = false; }

                var duplicated = false;
                if(ok === true)
                {
                    for(var x = 0; x < companyDocuments.length; x++)
                    {
                        if(document.getElementById("TxtCodigo").value.toLowerCase() === companyDocuments[x].Code.toLowerCase() &&
                           document.getElementById("TxtDocumento").value.toLowerCase() === companyDocuments[x].Description.toLowerCase() &&
                           companyDocuments[x].Id != documentId)
                        {
                            duplicated = true;
                            break;
                        }
                    }

                    if(duplicated === true)
                    {
                        $("#TxtCodigoErrorDuplicated").show();
                        $("#TxtDocumentoErrorDuplicated").show();
                        ok = false;
                    }
                }

                if(!RequiredFieldText("TxtRevision")) { ok = false; }

                var conservacionok = true;
                if (!RequiredFieldText("TxtConservacion")) { ok = false; }

                if (document.getElementById("CmbConservacion").value == 0) {
                    ok = false;
                    conservacionok = false;
                    $("#CmbConservacionErrorRequired").show();
                }
                else
                {
                    $("#CmbConservacionErrorRequired").hide();
                }

                if(conservacionok === true)
                {                
                    $("#TxtConservacionLabel").css("color", "#000");
                }
                else
                {
                    $("#TxtConservacionLabel").css("color", "#f00");
                }

                if (!RequiredFieldText("TxtCategory")) { ok = false; }

                if(document.getElementById("CmbOrigen").value == 2) {
                    if (!RequiredFieldText("TxtProcedencia")) { ok = false; }
                }
                else
                {
                    $("#TxtProcedenciaLabel").css("color", "#000");
                    $("#TxtProcedenciaErrorRequired").hide();
                }

                // ISSUS-210 - Fecha alta no puede ser posterior a fecha de revision
                if($("#TxtStartDate").val()==="")
                {
                    ok = false;
                    document.getElementById("TxtStartDateLabel").style.color="#f00";
                    document.getElementById("TxtStartDateErrorRequired").style.display = "";
                }
                else
                {
                    var startdate = GetDate($("#TxtStartDate").val(),"-");
                    if(startdate > firstDate)
                    {
                        ok = false;
                        document.getElementById("TxtStartDateLabel").style.color = "#f00";
                        document.getElementById("TxtStartDatePreviousRevision").style.display = "";
                    }
                }

                if ($("#TxtRevisionDate").val() === "") {
                    ok = false;
                    document.getElementById("TxtRevisionDateLabel").style.color = "#f00";
                    $("#TxtRevisionDateErrorRequired").show();
                }
                else {
                    var startdate = GetDate($("#TxtStartDate").val(), "-");
                    var revdate = GetDate($("#TxtRevisionDate").val(), "-");
                    if (revdate < startdate) {
                        ok = false;
                        document.getElementById("TxtRevisionDateLabel").style.color = "#f00";
                        $("#TxtRevisionDatePreviousRevision").html(Dictionary.Item_Document_ErrorMessage_RevisionOverDate + " " + $("#TxtStartDate").val());
                        $("#TxtRevisionDatePreviousRevision").show();
                    }
                }


                if(ok===false)
                {
                    window.scrollTo(0, 0); 
                    return false;
                }

                Save();
            }

            jQuery(function ($) {
                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || '&nbsp;'
                        if (("title_html" in this.options) && this.options.title_html == true)
                            title.html($title);
                        else title.text($title);
                    }
                }));
                
                $("#TxtRevision").css("text-align", "right");
                $("#TxtConservacion").css("text-align", "right");
               
                var options = $.extend({}, $.datepicker.regional["<%=this.ApplicationUser.Language %>"], { autoclose: true, todayHighlight: true });
                $("#TxtEndDate").datepicker(options);
                $("#TxtNewRevision").datepicker(options);
                $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

                var options = $.extend({}, $.datepicker.regional["<%=this.ApplicationUser.Language %>"], { autoclose: true, todayHighlight: true, maxDate: firstDate });
                $(".date-picker_start").datepicker(options);
                $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

                if(ApplicationUser.ShowHelp===true){
                    SetToolTip("DivEndDate", Dictionary.Item_Document_Help_EndDate);
                    SetToolTip("TxtDocumento", Dictionary.Item_Document_Help_Name);
                    SetToolTip("TxtCodigo", Dictionary.Item_Document_Help_Codigo);
                    SetToolTip("DivCmbCategory", Dictionary.Item_Document_Help_Categoria);
                    SetToolTip("BtnCategory", Dictionary.Item_Document_Help_BARCategoria);
                    SetToolTip("TxtStartDate", Dictionary.Item_Document_Help_StartDate);
                    SetToolTip("TxtConservacion", Dictionary.Item_Document_Help_ConservacionCantidad);
                    SetToolTip("DivCmbConservacion", Dictionary.Item_Document_Help_ConservacionTiempo);
                    SetToolTip("DivCmbOrigen", Dictionary.Item_Document_Help_Origin);
                    SetToolTip("DivCmbProcedencia", Dictionary.Item_Document_Help_Source);
                    SetToolTip("BtnProcedencia", Dictionary.Item_Document_Help_BARProcedencia);
                    SetToolTip("TxtUbicacion", Dictionary.Item_Document_Help_Ubicacion);
                    SetToolTip("TxtMotivo", Dictionary.Item_Document_Help_Reason);
                    SetToolTip("TxtRevision", Dictionary.Item_Document_Help_Revision);
                    SetToolTip("BtnNewVersion", Dictionary.Item_Document_Help_NewRevision);
                    $("[data-rel=tooltip]").tooltip();
                }

                $("#BtnCategory").on("click", function (e) {
                    e.preventDefault();
                    RenderCategoryTable();
                    $("#dialogCategory").removeClass("hide").dialog({
                        "resizable": false,
                        "modal": true,
                        "title": Dictionary.Item_Document_PopupCategory_Title,
                        "title_html": true,
                        "width": 800,
                        "buttons": [
                            {
                                "id": "BtnNewCategory",
                                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                                "class": "btn btn-success btn-xs",
                                "click": function () {
                                    CategoryInsert();
                                }
                            },
                            {
                                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                                "class": "btn btn-xs",
                                "click": function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]

                    });
                });

                $("#BtnProcedencia").on("click", function (e) {
                    e.preventDefault();
                    RenderProcedenciasTable();
                    $("#dialogProcedencia").removeClass("hide").dialog({
                        "resizable": false,
                        "modal": true,
                        "title": Dictionary.Item_Document_PopupSource_Title,
                        "title_html": true,
                        "width": 800,
                        "buttons": [
                            {
                                "id": "BtnNewProcedenciaSave",
                                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                                "class": "btn btn-success btn-xs",
                                "click": function () {
                                    ProcedenciaInsert();
                                }
                            },
                            {
                                "id": "BtnNewProcedenciaCancel",
                                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                                "class": "btn btn-xs",
                                "click": function () { $(this).dialog("close"); }
                            }
                        ]
                    });
                });

            });

            if (documentId != -1) {
                $("#TxtCodigo").val(documento.Code);
                $("#TxtDocumento").val(documento.Description);
                $("#TxtStartDate").val(documento.StartDate);
                $("#TxtEndDate").val(documento.EndDate);
                $("#TxtRevisionDate").val(documento.RevisionDate);
                $("#TxtUbicacion").val(documento.Location);
                $("#TxtConservacion").val(documento.Conservation);
                $("#TxtLinkField").val(documento.LinkField);
                document.getElementById("CmbOrigen").value = sourceSelected;
                SetCategoryText();
                SetProcedenciaText();
                SetOrigen();
                DOCUMENT_LinkViewLayout();
            }
            else {
                $("#TabHistoricoSelector").hide();
                $("#TxtStartDate").val(FormatDate(new Date(), "/"));
                $("#BtnNewVersion").hide();
                $("#TxtProcedencia").hide();
                $("#TxtProcedenciaLabel").hide();
                $("#BtnProcedencia").hide();
                $("#CmbProcedencia").hide();
            }

            FillCmbCategory();
            FillCmbProcedencia();

            // Control de permisos
            if (typeof ApplicationUser.Grants.Document === "undefined" || ApplicationUser.Grants.Document.Write === false) {
                $(".btn-danger").hide();
                $("input").attr("disabled", true);
                $("textarea").attr("disabled", true);
                $("select").attr("disabled", true);
                $("select").css("background-color", "#eee");
                $("#BtnNewUploadfileVersion").hide();
                $("#BtnNewVersion").hide();
                $("#BtnCategory").hide();
            }
        </script>
</asp:Content>