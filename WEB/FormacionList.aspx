<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="FormacionList.aspx.cs" Inherits="FormacionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .tags
        {
            display:block !important;
            width:100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var selectedRows = new Array();

        function LearningUpdate(id) {
            document.location = 'FormacionView.aspx?id=' + id;
        }

        function LearningDeleteDisabled() {
            alert(Dictionary.Common_ErrorMessage_CanNotDelete);
        }

        function selectRow(sender)
        {
            var id = sender.id.split('|')[0];
            var learningId = sender.id.split('|')[1];
            var employeeId = sender.id.split('|')[2];

            var selectedRowsTemp = new Array();
            var passed = false;

            for (var x=0; x<selectedRows.length;x++)
            {
                if(selectedRows[x].AssistantId!= id)
                {
                    selectedRowsTemp.push(selectedRows[x]);
                }
                else
                {
                    if(sender.checked)
                    {
                        selectedRowsTemp.push(selectedRows[x]);
                        passed = true;
                    }
                }
            }

            if(sender.checked && !passed)
            {
                selectedRowsTemp.push({AssistantId:id,LearningId:learningId,EmployeeId:employeeId});
            }
            
            selectedRows = selectedRowsTemp;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td>
                                            <%=this.Dictionary["Item_Learning_Filter_FromYear"] %>:
                                            <select onchange="Go(0, this.value);" id="CmbYearFrom">
                                                <asp:Literal runat="server" ID="LtYearFrom"></asp:Literal>
                                            </select>
                                        </td>
                                        <td>
                                            <%=this.Dictionary["Item_Learning_Filter_ToYear"] %>:
                                            <select onchange="Go(1, this.value);" id="CmbYearTo">
                                                <asp:Literal runat="server" ID="LtYearTo"></asp:Literal>
                                            </select>
                                        </td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status0" name="status" value="0" onclick="Go(2,0);" /><%=this.Dictionary["Item_Learning_Status_InProgress"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status1" name="status" value="1" onclick="Go(2,1);" /><%=this.Dictionary["Item_Learning_Status_Done"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status2" name="status" value="2" onclick="Go(2,2);" /><%=this.Dictionary["Item_Learning_Status_Evaluated"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status3" name="status" value="3" onclick="Go(2,3);" /><%=this.Dictionary["Common_All_Female_Plural"] %></td>
                                    </tr>
                                </table>
                            </div>
                            <div style="height:12px;clear:both;"></div>
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive">
                                            <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeader">
                                                                <th onclick="Sort(this,'ListDataTable');" id="th1" class="sort search" style="width:200px;cursor:pointer;"><%=this.Dictionary["Item_Learning_ListHeader_Course"] %></th>
                                                                <th><%=this.Dictionary["Item_Learning_ListHeader_Assistants"] %></th>
                                                                <th class="hidden-480" style="width:100px;"><%=this.Dictionary["Item_Learning_ListHeader_EstimatedDate"] %></th>
                                                                <th class="hidden-480" style="width:100px;"><%=this.Dictionary["Item_Learning_ListHeader_Cost"] %></th>
                                                                <th class="hidden-480" style="width:90px !important;">&nbsp;</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="ListDataTable">
                                                            <asp:Literal runat="server" ID="LtLearningTable"></asp:Literal>
                                                        </tbody>
                                                        <tfoot class="thin-border-bottom">
                                                            <tr id="ListDataFooter">
                                                                <td style="color:#333;" colspan="2"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="LtCount"></asp:Literal></i></td>
                                                                <td style="color:#333;" align="right"><strong><%=this.Dictionary["Common_Total"] %>:&nbsp</strong></td>
                                                                <td style="color:#333;" align="right"><strong><asp:Literal runat="server" ID="LtTotal"></asp:Literal></strong></td>
                                                                <td style="color:#333;"></td>
                                                            </tr>
                                                        </tfoot>
                                                    </table>
                                            <br />
                                            <br />
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->						
                            </div>
                            
                            <div id="LearningDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Learning_PopupDelete_Message"] %>&nbsp;<strong><span id="LearningName"></span></strong>?</p>
                                <!--<span id="TxtNewReasonLabel"><%=this.Dictionary["Item_Document_PopupDelete_Message"]%><br /></span>
                                <textarea id="TxtNewReason" cols="40" rows="3"></textarea>
                                <span class="ErrorMessage" id="TxtNewReasonErrorRequired" style="display:none;"> <%=this.Dictionary["Item_Learning_Error_DeleteReasonRequired"] %></span>-->
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
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <!-- page scripts -->
        <script type="text/javascript">
            function Go(param, value) {
                var yearFrom = document.getElementById("CmbYearFrom").value;
                var yearTo = document.getElementById("CmbYearTo").value;
                var mode = 3;
                if (document.getElementById("Contentholder1_status0").checked) mode = 0;
                if (document.getElementById("Contentholder1_status1").checked) mode = 1;
                if (document.getElementById("Contentholder1_status2").checked) mode = 2;
                if (yearTo > 0) {
                    if (yearFrom > yearTo) {
                        alert("El año de origen no puede ser superior al año final");
                        window.scrollTo(0, 0); 
                        return false;
                    }
                }

                document.location = 'FormacionList.aspx?mode=' + mode + '&yearFrom=' + yearFrom + '&yearTo=' + yearTo;
            }

            function LearningDelete(id, description) {
                document.getElementById('LearningName').innerHTML = description;
                var dialog = $("#LearningDeleteDialog").removeClass('hide').dialog({
                    resizable: false,
                    width: 500,
                    modal: true,
                    title: Dictionary.Common_Delete,
                    title_html: true,
                    buttons: [
                            {
                                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                                "class": "btn btn-danger btn-xs",
                                click: function () {
                                    var ok = true;
                                    //if(!RequiredFieldText('TxtNewReason')) { ok = false; }
                                    if(ok === false)
                                    {
                                        window.scrollTo(0, 0); 
                                        return false;
                                    }

                                    $(this).dialog("close");
                                    LearningDeleteConfirmed(id);
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

            function LearningDeleteConfirmed(id)
            {
                var webMethod = "/Async/LearningActions.asmx/Delete";
                var data = {
                    'learningId': id,
                    'companyId': Company.Id,
                    'userId': user.Id,
                    'reason': '' //document.getElementById('TxtNewReason').value
                };

                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (response) {                    
                        if (response.d.Success === true) {
                            document.location = document.location + '';
                        }
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(jqXHR.responseText);
                    }
                });
            }

            jQuery(function ($) {
                //override dialog's title function to allow for HTML titles
                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || '&nbsp;'
                        if (("title_html" in this.options) && this.options.title_html == true)
                            title.html($title);
                        else title.text($title);
                    }
                }));

                $('#BtnCompleted').on('click', function (e) {
                    var webMethod = "/Async/LearningActions.asmx/Complete";
                    var data = {
                        'companyId': Company.Id,
                        'assistants': selectedRows,
                        'userId': user.Id
                    };

                    $.ajax({
                        type: "POST",
                        url: webMethod,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(data, null, 2),
                        success: function (response) {
                            if (response.d.Success === true) {
                                document.location = document.location + '';
                            }
                            if (response.d.Success !== true) {
                                alertUI(response.d.MessageError);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            alert(jqXHR.responseText);
                        }
                    });
                });

                $('#BtnSuccess').on('click', function (e) {
                    var webMethod = "/Async/LearningActions.asmx/Success";
                    var data = {
                        'companyId': Company.Id,
                        'assistants': selectedRows,
                        'userId': user.Id
                    };

                    $.ajax({
                        type: "POST",
                        url: webMethod,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(data, null, 2),
                        success: function (response) {
                            if (response.d.Success === true) {
                                document.location = document.location + '';
                            }
                            if (response.d.Success !== true) {
                                alertUI(response.d.MessageError);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            alert(jqXHR.responseText);
                        }
                    });
                });

                $('#BtnDelete').on('click', function (e) {
                    var webMethod = "/Async/LearningActions.asmx/Delete";
                    var data = {
                        'companyId': Company.Id,
                        'assistants': selectedRows,
                        'userId': user.Id
                    };

                    $.ajax({
                        type: "POST",
                        url: webMethod,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(data, null, 2),
                        success: function (response) {
                            if (response.d.Success === true) {
                                document.location = document.location + '';
                            }
                            if (response.d.Success !== true) {
                                alertUI(response.d.MessageError);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            alert(jqXHR.responseText);
                        }
                    });
                });
            });

            if (typeof user.Grants.Learning === "undefined" || user.Grants.Learning.Write === false) {
                $(".icon-edit").addClass("icon-eye-open");
            }

            if (typeof user.Grants.Learning === "undefined" || user.Grants.Learning.Delete === false) {
                $(".btn-danger").hide();
            }

            function Export() {
                var status = 3;
                if (document.getElementById("Contentholder1_status0").checked == true) { status = 0; }
                if (document.getElementById("Contentholder1_status1").checked == true) { status = 1; }
                if (document.getElementById("Contentholder1_status2").checked == true) { status = 2; }
                var data =
                {
                    companyId: Company.Id,
                    yearFrom: $("#CmbYearFrom").val(),
                    yearTo: $("#CmbYearTo").val(),
                    mode: status
                };

                var webMethod = "/Export/FormacionExportList.aspx/PDF";
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

            window.onload = function () {
                $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
                $(".page-header .col-sm-8").addClass("col-sm-6");
                $(".page-header .col-sm-8").removeClass("col-sm-8");
                $(".page-header .col-sm-4").addClass("col-sm-6");
                $(".page-header .col-sm-4").removeClass("col-sm-4");
            }
        </script>
</asp:Content>

