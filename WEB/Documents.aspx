<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="Documents.aspx.cs" Inherits="Documents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv, #scrollTableDiv2{
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="tabbable">
                                    <ul class="nav nav-tabs padding-18">
                                        <li class="active">
                                            <a data-toggle="tab" href="#active" onclick="document.getElementById('BtnNewItem').parentNode.style.visibility='visible';"><%=this.Dictionary["Item_Document_Status_ActivePlural"] %></a>
                                        </li>
                                        <li class="">
                                            <a data-toggle="tab" href="#inactive" onclick="document.getElementById('BtnNewItem').parentNode.style.visibility='hidden';"><%=this.Dictionary["Item_Document_Status_InactivePlural"] %></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content no-border padding-24" style="height:500px;">
                                        <div id="active" class="tab-pane active">  
                                            <div class="row">
                                                <div class="col-xs-12">
                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead>
                                                                <tr id="ListDataHeader">
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_Document_ListHeader_Name"] %></th>
			                                                        <th id="th1" class="search" style="width:110px;"><%=this.Dictionary["Item_Document_ListHeader_Code"] %></th>
			                                                        <th id="th2" class="" style="width:110px;"><%=this.Dictionary["Item_Document_ListHeader_Revision"] %></th>
			                                                        <th style="width:106px;">&nbsp;</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTable">
                                                                    <asp:Literal runat="server" ID="LtDocumentsActive"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="DocumentDataTotal"></asp:Literal></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div><!-- /.table-responsive -->
                                                </div><!-- /span -->
                                            </div><!-- /row -->	
                                        </div>
                                        <div id="inactive" class="tab-pane">
                                            <div class="row">
                                                <div class="col-xs-12">
                                                    <div class="table-responsive" id="scrollTableDiv2">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead>
                                                                <tr>
                                                                    <th onclick="Sort(this,'DocumentsDataTableInactive');" id="th0" class="" style="cursor:pointer;"><%=this.Dictionary["Item_Document_List_Header_Name"] %></th>
                                                                    <th onclick="Sort(this,'DocumentsDataTableInactive');" id="th1" class="hidden-480" style="width:110px;cursor:pointer;"><%=this.Dictionary["Item_Document_List_Header_Code"] %></th>
                                                                    <th onclick="Sort(this,'DocumentsDataTableInactive');" id="th3" class="hidden-480" style="width:60px;cursor:pointer;"><%=this.Dictionary["Item_Document_List_Header_Revision"] %></th>
                                                                    <th onclick="Sort(this,'DocumentsDataTableInactive');" id="th4" class="hidden-480" style="width:120px;cursor:pointer;"><%=this.Dictionary["Item_Document_List_Header_Responsible"] %></th>
                                                                    <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Document_List_Header_Date"] %></th>
                                                                    <th style="width:106px;">&nbsp;</th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDivInactive" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="DocumentsDataTableInactive"><asp:Literal runat="server" ID="LtDocumentsInactive"></asp:Literal></tbody>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooterInactive">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="DocumentInactiveDataTotal"></asp:Literal></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div><!-- /.table-responsive -->
                                                </div><!-- /span -->
                                            </div><!-- /row -->	
                                        </div>
                                    </div>
                                </div>                                                                            
                            </div><!-- /.col -->
                            
                            <div id="DocumentDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Document_PopupDelete_Message"] %>&nbsp;<strong><span id="DocumentName"></span></strong>?</p>
                                <span id="TxtNewReasonLabel"><%=this.Dictionary["Item_Document_PopupDelete_Message"]%><br /></span>
                                <textarea id="TxtNewReason" cols="40" rows="3"></textarea>
                                <span class="ErrorMessage" id="TxtNewReasonErrorRequired" style="display:none;"> <%=this.Dictionary["Item_Document_Error_DeleteReasonRequired"] %></span>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server"> 
        <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="js/common.js"></script>
        <script type="text/javascript">
            function DocumentUpdate(id)
            {
                document.location = 'DocumentView.aspx?id=' + id;
            }

            function DocumentDelete(id, description) {
                document.getElementById('DocumentName').innerHTML = description;
                var dialog = $("#DocumentDeleteDialog").removeClass('hide').dialog({
                    resizable: false,
                    width: 500,
                    modal: true,
                    title: Dictionary.Item_Document_PopupDelete_Title,
                    title_html: true,
                    buttons:
                    [
                        {
                            html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                            "class": "btn btn-danger btn-xs",
                            click: function () {
                                var ok = true;
                                if(!RequiredFieldText('TxtNewReason')) { ok = false; }
                                if(ok === false)
                                {
                                    window.scrollTo(0, 0); 
                                    return false;
                                }

                                $(this).dialog("close");
                                DocumentDeleteConfirmed(id);
                            }
                        },
                        {
                            html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                            "class": "btn btn-xs",
                            click: function () {
                                ClearFieldTextMessages('TxtNewReason');
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            }

            function DocumentDeleteConfirmed(id)
            {
                var webMethod = "/Async/DocumentActions.asmx/DocumentDelete";
                var data = {
                    'documentId': id,
                    'companyId': Company.Id,
                    'userId': user.Id,
                    'reason': document.getElementById('TxtNewReason').value
                };

                LoadingShow();
                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (response) {    
                        LoadingHide();                
                        if (response.d.Success === true) {
                            document.location = document.location + '';
                        }
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        LoadingHide();
                        alert(jqXHR.responseText);
                    }
                });
            }

            function Restore(documentId) {
                var webMethod = "/Async/DocumentActions.asmx/Restore";
                var data = {
                    'documentId': documentId,
                    'companyId': Company.Id,
                    'userId': user.Id
                };

                LoadingShow(Dictionary.Common_Message_Saving);
                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (response) {
                        LoadingHide();
                        if (response.d.Success === true) {
                            document.location = document.location + '';
                        }
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        LoadingHide();
                        alertUI(jqXHR.responseText);
                    }
                });
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
            });

            function Resize() {
                var listTable = document.getElementById('ListDataDiv');
                var listTableInactive = document.getElementById('ListDataDivInactive');
                var containerHeight = $(window).height();
                listTable.style.height = (containerHeight - 370) + 'px';
                listTableInactive.style.height = (containerHeight - 370) + 'px';                
            }

            window.onload = function () {
                Resize();
                $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
            }
            window.onresize = function () { Resize(); }

            function Export(fileType) {
                console.log("Export", fileType);
                var webMethod = "/Export/DocumentExportList.aspx/" + fileType;
                var data = { "companyId": Company.Id };
                LoadingShow(Dictionary.Common_Report_Rendering);
                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (msg) {
                        LoadingHide();
                        //successInfoUI(msg.d.MessageError, Go, 200);
                        var link = document.createElement('a');
                        link.id = 'download';
                        link.href = msg.d.MessageError;
                        link.download = msg.d.MessageError;
                        link.target = '_blank';
                        document.body.appendChild(link);
                        document.body.removeChild(link);
                        $('#download').trigger('click');
                        window.open(msg.d.MessageError);
                        $("#dialogAddAddress").dialog('close');
                    },
                    error: function (msg) {
                        LoadingHide();
                        alertUI("error:" + msg.responseText);
                    }
                });
            }
        </script>
</asp:Content>

