<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="CargosList.aspx.cs" Inherits="CargosList" %>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th style="cursor:pointer;" onclick="Sort(this,'ListDataTable','text',false);" id="th0" class="sort search"><%=this.Dictionary["Item_JobPosition_ListHeader_Name"] %></th>
                                                        <th style="cursor:pointer;width:400px;" onclick="Sort(this,'ListDataTable','text',false);" id="th1" class="sort search"><%=this.Dictionary["Item_JobPosition_ListHeader_Responsible"] %></th>
                                                        <th style="cursor:pointer;width:400px;" onclick="Sort(this,'ListDataTable','text',false);" id="th2" class="sort search hidden-480"><%=this.Dictionary["Item_JobPosition_ListHeader_Department"] %></th>
                                                        <th style="width:107px;">&nbsp;</th></tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="CargosData"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="CargosDataTotal"></asp:Literal></i></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                        <!-- /.table-responsive -->
                                    </div>
                                    <!-- /span -->
                                </div>
                                <!-- /row -->
                            </div><!-- /.col -->
                            
                            <div id="JobPositionDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_JobPosition_PopupDelete_Message"] %>&nbsp;<strong><span id="JobPositionName"></span></strong>?</p>
                                <!--<span id="TxtNewReasonLabel"><%=this.Dictionary["Item_Document_PopupDelete_Message"]%><br /></span>
                                <textarea id="TxtNewReason" cols="40" rows="3"></textarea>
                                <span class="ErrorMessage" id="TxtNewReasonErrorRequired" style="display:none;"> <%=this.Dictionary["Item_JobPosition_Error_DeleteReasonRequired"] %></span>-->
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript">
            function JobPositionDelete(id, description) {
                document.getElementById('JobPositionName').innerHTML = description;
                var dialog = $("#JobPositionDeleteDialog").removeClass("hide").dialog({
                    resizable: false,
                    modal: true,
                    title: Dictionary.Common_Delete,
                    title_html: true,
                    buttons:
                    [
                        {
                            html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                            "class": "btn btn-danger btn-xs",
                            click: function () {
                                $(this).dialog("close");
                                JobPositionDeleteConfirmed(id);
                            }
                        },
                        {
                            html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                            "class": "btn btn-xs",
                            click: function () {
                                ClearFieldTextMessages('TxtNewReason');
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            }

            function JobPositionDeleteConfirmed(id)
            {
                var webMethod = "/Async/JobPositionActions.asmx/Delete";
                var data = {
                    'jobPositionId': id,
                    'companyId': Company.Id,
                    'userId': user.Id,
                    'reason': '' //document.getElementById('TxtNewReason').value
                };

                LoadingShow('');
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

            function Export()
            {
                alertUI('Not implmented on Release Build');
            }

            function Resize() {
                var listTable = document.getElementById('ListDataDiv');
                var containerHeight = $(window).height();
                listTable.style.height = (containerHeight - 310) + 'px';
            }

            window.onload = function () { Resize(); }
            window.onresize = function () { Resize(); }
        </script>
</asp:Content>



