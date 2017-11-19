<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ItemList.aspx.cs" Inherits="ItemList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive">
                                            <table class="table table-bordered table-striped">
                                                <%=this.DataHeader.Render %>
                                                <tbody id="ListDataTable"><asp:Literal runat="server" ID="ItemData"></asp:Literal></tbody>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->								
                            </div><!-- /.col -->
                            <div id="ItemDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Department_PopupDelete_Message"] %>&nbsp;<strong><span id="ItemName"></span></strong>?</p>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="js/common.js"></script>
        <script type="text/javascript">
            var ItemSelected;

            function ItemDelete(id, name) {
                $('#ItemName').html(name);
                ProviderSelected = id;
                var dialog = $("#ItemDeleteDialog").removeClass('hide').dialog({
                    resizable: false,
                    modal: true,
                    title: Dictionary.Common_Delete,
                    title_html: true,
                    buttons: [
                            {
                                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
                                "class": "btn btn-danger btn-xs",
                                click: function () {
                                    ItemDeleteAction();
                                }
                            },
                            {
                                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                                "class": "btn btn-xs",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                });
            }

            function ProviderDeleteAction() {
                var webMethod = "/Async/<%=this.ItemName %>Actions.asmx/Delete";
                var data = { <%=this.ItemName %>Id: ItemSelected, companyId: company.Id, userId: user.Id };
                $("#ItemDeleteDialog").dialog("close");
                LoadingShow(Dictionary.Common_Message_Saving);
                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (msg) {
                        document.location = document.location + '';
                    },
                    error: function (msg) {
                        LoadingHide();
                        alertUI(msg.responseText);
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

                $("#BtnNew<%=this.ItemName %>").on('click', function (e) {
                    document.location = '<%=this.ItemName %>View.aspx?id=-1';
                    return false;
                });
            });
        </script>
</asp:Content>

