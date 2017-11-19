<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="FormacionView.aspx.cs" Inherits="FormacionView" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript" src="js/common.js"></script>
    <link rel="stylesheet" href="/Document-Viewer/style.css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/yepnope.1.5.3-min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/ttw-document-viewer.min.js"></script>
    <script type="text/javascript">
        var formacion = <%=this.Learning.Json %>;
        var SelectedEmployees = <%= this.JsonAssistance%>;
        var lastNewAssistantId = 0;   
        var typeItemId = 10;
        var itemId = formacion.Id;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active" id="TabHome">
                                                    <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Learning_Tab_Principal"]%></a>
                                                </li>
                                                <% if(this.Learning.Id > 0) { %>
                                                <li class="" id="TabuploadFiles">
                                                    <a data-toggle="tab" href="#uploadFiles"><%=this.Dictionary["Item_Learning_Tab_UploadFiles"]%></a>
                                                </li>
                                                <% }
                                                    if (this.Admin)
                                                   { %>
                                                <li class="" id="TabTrazas">
                                                    <a data-toggle="tab" href="#trazas"><%=this.Dictionary["Trazas"]%></a>
                                                </li>
                                                <% } %>
                                            </ul>
                                            <div class="tab-content no-border padding-24">
                                                <div id="home" class="tab-pane active">                                                
                                                    <form class="form-horizontal" role="form">
                                                        <div style="display:none;">
                                                            <div class="col-sm-1 control-label">
                                                                <input value="0" type="radio" id="RBStatus1" name="RBStatus" class="col-xs-12 col-sm-12" />
                                                            </div>           
                                                            <label class="col-sm-1 no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_Learning_Status_InProgress"] %></label>
                                                            <div class="col-sm-1 control-label"><input value="1" type="radio" id="RBStatus2" name="RBStatus" class="col-xs-12 col-sm-12" /></div> 
                                                            <label class="col-sm-1 no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_Learning_Status_Done"] %></label>
                                                            <div class="col-sm-1 control-label"><input value="2" type="radio" id="RBStatus3" name="RBStatus" class="col-xs-12 col-sm-12" /></div> 
                                                            <label class="col-sm-1 no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_Learning_Status_Evaluated"] %></label>                                                        
                                                            <div class="col-sm-7">&nbsp;</div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtYearLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Common_Year"] %><span style="color:#f00;">*</span></label>
                                                            <div class="col-sm-1">
                                                                <input <% if (this.Learning.Status == 2) { %>readonly="readonly" <% } %>type="text" id="TxtYear" placeholder="<%=this.Dictionary["Common_Year"] %>" class="col-xs-12 col-sm-12 integerFormated" maxlength="4"<% if(this.ShowHelp) { %> title="<%=this.Dictionary["Item_Learning_FieldLabel_Year"]%>"<% } %> />
                                                                <span class="ErrorMessage" id="TxtYearErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div> 
                                                            <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Learning_FieldLabel_Course"] %><span style="color:#f00;">*</span></label>
                                                            <div class="col-sm-9">
                                                                <input <% if (this.Learning.Status == 2) { %>readonly="readonly" <% } %>type="text" id="TxtName" placeholder="<%=this.Dictionary["Item_Learning_FieldLabel_Course"] %>" class="col-xs-12 col-sm-12"<% if(this.ShowHelp) { %> title="<%=this.Dictionary["Item_Learning_FieldLabel_Name"]%>"<% } %> maxlength="100" />
                                                                <span class="ErrorMessage" id="TxtNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="space-4"></div>
                                                        <div class="form-group">
                                                            <!--<label class="col-sm-2 control-label" for="form-input-readonly"><%=this.Dictionary["Item_LearningAssistants"] %></label>-->
                                                            <% if (this.Learning.Status == 0)
                                                               { %>
                                                               <button class="btn btn-success" type="button" id="BtnSelectEmpleado">
                                                                <i class="icon-plus-sign bigger-110"></i>
                                                                <%=this.Dictionary["Item_Learning_SelectAssistants"] %>
                                                            </button>
                                                            <!--<div class="col-sm-1"><span class="btn btn-light" style="height:30px;" id="BtnSelectEmpleado_" title="<%=this.Dictionary["Item_Learning_SelectAssistants"] %>">...</span></div>-->
                                                            <% } %>
                                                        </div>                                                            
                                                        <div class="row">
                                                            <div class="col-xs-12">
                                                                <div class="table-responsive">
                                                                    <table class="table table-bordered table-striped">
                                                                                <thead class="thin-border-bottom">
                                                                                    <tr>
                                                                                        <% if (this.Learning.Status == 1)
                                                                                           { %>
                                                                                        <th style="width:23px;"><input type="checkbox" title="<%=this.Dictionary["Common_SelectAll todos"] %>" onclick="SelectAllAssistants(this);" /></th>
                                                                                        <% } %>
                                                                                        <th><%=this.Dictionary["Item_LearningAssistants"] %></th>

                                                                                        <% if(this.Learning.Status==0) { %>
                                                                                            <th class="hidden-480" style="width:80px;">&nbsp;</th>
                                                                                        <% }else  { %>
                                                                                        <th class="hidden-480" style="width:80px;"><%=this.Dictionary["Item_LearningAssistant_Status_Done"] %></th>
                                                                                        <th class="hidden-480" style="width:80px;"><%=this.Dictionary["Item_LearningAssistant_Status_Evaluated"]%></th>
                                                                                        <% } %>
                                                                                    </tr>
                                                                                </thead>
                                                                                <tbody id="SelectedEmployeesTable"><asp:Literal runat="server" ID="TableAssistance"></asp:Literal></tbody>
                                                                            </table>
                                                                </div><!-- /.table-responsive -->
                                                            </div><!-- /span -->
                                                            <% if (this.Learning.Status == 1) { %>
                                                            <div class="col-xs-12">
                                                                <button class="btn btn-info" type="button" id="BtnRealizado" onclick="Realizado();">
                                                                    <i class="icon-ok"></i>
                                                                    <%=this.Dictionary["Item_LearningAssistant_Status_Done"] %>
                                                                </button>
                                                                &nbsp; &nbsp; &nbsp;
                                                                <button class="btn btn-info" type="button" id="BtnItem_LearningAssistant_Status_Evaluated" onclick="Item_LearningAssistant_Status_Evaluated();">
                                                                    <i class="icon-ok"></i>
                                                                    <%=this.Dictionary["Item_LearningAssistant_Status_Evaluated"] %>
                                                                </button>
                                                            </div>
                                                            <% } %>
                                                        </div><!-- /row -->	
                                                        <div class="space-4"></div>
                                                        <hr />
                                                        <div class="form-group">
                                                            <label id="CmbFechaPrevistaMesLabel" class="col-sm-2 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_Learning_FieldLabel_EstimatedDate"] %><span style="color:#f00;">*</span></label>
                                                            <div class="col-sm-2" id="DivCmbFechaPrevistaMes" style="height:35px !important;">
                                                                <% if (this.Learning.Status < 2) { %>
                                                                <select class="col-xs-12 col-sm-12" <% if (this.Learning.Status ==2) { %>readonly="readonly" <% } %>id="CmbFechaPrevistaMes">
                                                                    <option value="0"<%=this.Learning.DateEstimated.Month == 0 ? "selected=\"selected\"" : string.Empty %>><%=this.Dictionary["Common_Month"]%></option>
                                                                    <option value="1"<%=this.Learning.DateEstimated.Month == 1 ? "selected=\"selected\"" : string.Empty %>><%=this.Dictionary["Common_MonthName_January"]%></option>
                                                                    <option value="2"<%=this.Learning.DateEstimated.Month == 2 ? "selected=\"selected\"" : string.Empty %>><%=this.Dictionary["Common_MonthName_February"]%></option>
                                                                    <option value="3"><%=this.Dictionary["Common_MonthName_March"]%></option>
                                                                    <option value="4"><%=this.Dictionary["Common_MonthName_April"]%></option>
                                                                    <option value="5"><%=this.Dictionary["Common_MonthName_May"]%></option>
                                                                    <option value="6"><%=this.Dictionary["Common_MonthName_June"]%></option>
                                                                    <option value="7"><%=this.Dictionary["Common_MonthName_July"]%></option>
                                                                    <option value="8"><%=this.Dictionary["Common_MonthName_August"]%></option>
                                                                    <option value="9"><%=this.Dictionary["Common_MonthName_September"]%></option>
                                                                    <option value="10"><%=this.Dictionary["Common_MonthName_October"]%></option>
                                                                    <option value="11"><%=this.Dictionary["Common_MonthName_November"]%></option>
                                                                    <option value="12"><%=this.Dictionary["Common_MonthName_December"]%></option>
                                                                </select>
                                                                <span class="ErrorMessage" id="CmbFechaPrevistaMesErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                                                <% } else { %>
                                                                <input type="text" readonly="readonly" class="col-xs-12 col-sm-12" value="<%=this.MesPrevisto %>" id="TxtFechaPresvistaMesReadOnly"  />
                                                                <% } %>
                                                            </div>
                                                            <div class="col-sm-2" id="DivCmbYearPrevisto" style="height:35px !important;">
                                                                <% if (this.Learning.Status != 2)
                                                                   { %>
                                                                <select onchange="document.getElementById('TxtFechaPrevistaYear').value = this.value;" id="CmbYearPrevisto" class="col-xs-12 col-sm-12" >
                                                                    <asp:Literal runat="server" ID="LtYearPrevistos"></asp:Literal>
                                                                </select>
                                                                <% } %>
                                                                <input <% if (this.Learning.Status != 2) { %>style="display:none;" <% } %> type="text" <% if (this.Learning.Status ==2) { %>readonly="readonly" <% } %>id="TxtFechaPrevistaYear" placeholder="<%=this.Dictionary["Common_Year"] %>" class="col-xs-12 col-sm-12" onkeypress="validate(event)" maxlength="4"  />
                                                                <span class="ErrorMessage" id="TxtFechaPrevistaYearErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <label id="TxtHoursLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Learning_FieldLabel_Hours"] %><span style="color:#f00;">*</span></label>
                                                            <div class="col-sm-2">
                                                                <input type="text" <% if (this.Learning.Status ==2) { %>readonly="readonly" <% } %>id="TxtHours" placeholder="<%=this.Dictionary["Item_Learning_FieldLabel_Hours"] %>" class="col-xs-12 col-sm-12 integerFormated" maxlength="9" />
                                                                <span class="ErrorMessage" id="TxtHoursErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                            <label id="TxtAmountLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Learning_FieldLabel_Amount"] %></label>
                                                            <div class="col-sm-2">
                                                                <input type="text" <% if (this.Learning.Status ==2) { %>readonly="readonly" <% } %>id="TxtAmount" placeholder="<%=this.Dictionary["Item_Learning_FieldLabel_Amount"] %>" class="col-xs-12 col-sm-12 money-bank" maxlength="9" />
                                                                <span class="ErrorMessage" id="TxtAmountErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtMasterLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Learning_FieldLabel_Coach"] %><span style="color:#f00;">*</span></label>
                                                            <div class="col-sm-7">
                                                                <input type="text" <% if (this.Learning.Status ==2) { %>readonly="readonly" <% } %>id="TxtMaster" placeholder="<%=this.Dictionary["Item_Learning_FieldLabel_Coach"] %>" class="col-xs-12 col-sm-12" maxlength="100" />
                                                                <span class="ErrorMessage" id="TxtMasterErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtRealStartLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Learning_FieldLabel_StartDate"] %></label>
                                                            <div class="col-sm-2">
                                                                <div class="row">
                                                                    <div class="col-xs-12 col-sm-12">
                                                                        <div class="input-group">
                                                                            <input <% if (this.Learning.Status == 2) { %>readonly="readonly" <% } %>class="form-control <% if (this.Learning.Status == 2) { %>_<% } %>date-picker" id="TxtRealStart" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                                            <span  <% if (this.Learning.Status == 2) { %>style="display:none;" <% } %>class="input-group-addon" onclick="document.getElementById('TxtRealStart').focus();" id="BtnRealStart">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <span class="ErrorMessage" id="TxtRealStartErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtRealStartErrorDateRange" style="display:none;"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
                                                                    <span class="ErrorMessage" id="TxtRealStartDateMalformed" style="display:none;"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                </div>
                                                            </div>
                                                            <label id="TxtRealFinishLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Learning_FieldLabel_EndDate"] %></label>
                                                            <div class="col-sm-2">
                                                                <div class="row">
                                                                    <div class="col-xs-12 col-sm-12">
                                                                        <div class="input-group">
                                                                            <input <% if (this.Learning.Status == 2) { %>readonly="readonly" <% } %>class="form-control <% if (this.Learning.Status == 2) { %>_<% } %>date-picker" id="TxtRealFinish" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                                            <span <% if (this.Learning.Status == 2) { %>style="display:none;" <% } %>class="input-group-addon" onclick="document.getElementById('TxtRealFinish').focus();" id="BtnRealFinish">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <span class="ErrorMessage" id="TxtRealFinishErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtRealFinishErrorDateRange" style="display:none;"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
                                                                    <span class="ErrorMessage" id="TxtRealFinishDateMalformed" style="display:none;"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                        </div>
                                                        <div class="for-group">
                                                            <label class="col-sm-12"><%=this.Dictionary["Item_Learning_Objetive"] %></label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-12"><textarea rows="5" <% if (this.Learning.Status == 2) { %>readonly="readonly" <% } %>class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtObjetivo"></textarea></div>
                                                        </div>
                                                        <div class="for-group">
                                                            <label class="col-sm-12"><%=this.Dictionary["Item_Learning_FieldLabel_Methodology"] %></label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-12"><textarea rows="5" <% if (this.Learning.Status == 2) { %>readonly="readonly" <% } %>class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtMetodologia"></textarea></div>
                                                        </div>
                                                        <div class="for-group">
                                                            <label class="col-sm-12"><%=this.Dictionary["Item_Learning_FieldLabel_Notes"] %></label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-12"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="500" id="TxtNotes"></textarea></div>
                                                        </div>
                                                        <%=this.FormFooter %>
                                                    </form>
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
                                <div id="assistantDialog" class="hide" style="width:800px;height:50%;">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-striped">
                                            <thead class="thin-border-bottom">
                                                <tr>
                                                    <th class="hidden-480" width="20px;">
                                                        <input type="checkbox" title="<%=this.Dictionary["Common_SelectAll todos"] %>" onclick="SelectAll(this);" />
                                                    </th>
                                                    <th>
                                                        <i class="icon-caret-right blue"></i><%=this.Dictionary["Common_Name"] %>
                                                    </th>													
                                                </tr>
                                            </thead>
                                            <tbody id="SelectableEmployeesTable">
                                            </tbody>
                                        </table>
                                    </div><!-- /.table-responsive -->
                                </div><!-- #dialog-message -->
                                
                            <div id="EmployeeDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Common_Delete"]%><strong>&nbsp;<span id="DeleteEmployeeName"></span></strong></p>
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
        <script type="text/javascript" src="assets/js/autoNumeric.js"> </script>
        <script type="text/javascript" src="js/FormacionView.js?ac<%= this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>

