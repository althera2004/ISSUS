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

        .node {
            cursor: pointer;
        }
            .node circle {
                fill: #fff;
                stroke: steelblue;
                stroke-width: 1.5px;
            }
            .node text {
                font: 10px sans-serif;
            }
        .link {
            fill: none;
            stroke: #ccc;
            stroke-width: 1.5px;
        }

        .google-visualization-orgchart-node {
            border: none !important;
        }

        .google-visualization-orgchart-node-medium {
            font-size:12px!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <form class="form-horizontal" role="form">
                                <div class="tabbable">
                                    <ul class="nav nav-tabs padding-18">
                                        <li class="active" id="TabHomeSelector">
                                            <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_JobPositions"] %></a>
                                        </li>
                                        <li class="" id="TabGraph"onclick="$('#PieWidget').show();">
                                            <a data-toggle="tab" href="#graph"><%=this.Dictionary["Item_JobPosition_Graph"] %></a>
                                        </li>
                                    </ul>                                    
                                    <div class="tab-content no-border padding-24">
                                        <div id="home" class="tab-pane active">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="table-responsive" id="scrollTableDiv">
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataHeader">
                                                                        <th style="cursor:pointer;" onclick="Sort(this,'ListDataTable','text',false);" id="th0" class="sort search"><%=this.Dictionary["Item_JobPosition_ListHeader_Name"] %></th>
                                                                        <th style="cursor:pointer;width:250px;" onclick="Sort(this,'ListDataTable','text',false);" id="th1" class="sort search"><%=this.Dictionary["Item_JobPosition_ListHeader_Responsible"] %></th>
                                                                        <th style="cursor:pointer;width:250px;" onclick="Sort(this,'ListDataTable','text',false);" id="th2" class="sort search hidden-480"><%=this.Dictionary["Item_JobPosition_ListHeader_Department"] %></th>
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
                                                                        <td><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<strong id="TotalList"><asp:Literal runat="server" ID="CargosDataTotal"></asp:Literal></strong></td>
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
                                        </div>
                                        <div id="graph" class="tab-pane">
                                            <div id="chart_div"></div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                            
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
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
        <script type="text/javascript">
            var graphRows = <%=this.GraphRows%>;
            function JobPositionDelete(id, description) {
                $("#JobPositionName").html(description);
                $("#JobPositionDeleteDialog").removeClass("hide").dialog({
                    "resizable": false,
                    "modal": true,
                    "title": Dictionary.Common_Delete,
                    "title_html": true,
                    "buttons":
                    [
                        {
                            "id": "BtnDeleteOk",
                            "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                            "class": "btn btn-danger btn-xs",
                            "click": function () {
                                $(this).dialog("close");
                                JobPositionDeleteConfirmed(id);
                            }
                        },
                        {
                            "id": "BtnDeleteCancel",
                            "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                            "class": "btn btn-xs",
                            "click": function () {
                                ClearFieldTextMessages("TxtNewReason");
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            }

            function JobPositionDeleteConfirmed(id)
            {
                var data = {
                    "jobPositionId": id,
                    "companyId": Company.Id,
                    "userId": user.Id,
                    "reason": "" 
                };

                LoadingShow("");
                $.ajax({
                    "type": "POST",
                    "url": "/Async/JobPositionActions.asmx/Delete",
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": JSON.stringify(data, null, 2),
                    "success": function (response) {   
                        LoadingHide();                 
                        if (response.d.Success === true) {
                            document.location = document.location + "";
                        }
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                    },
                    "error": function (jqXHR) {
                        LoadingHide();
                        alertUI(jqXHR.responseText);
                    }
                });
            }

            jQuery(function ($) {
                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || "&nbsp;"
                        if (("title_html" in this.options) && this.options.title_html == true)
                            title.html($title);
                        else title.text($title);
                    }
                }));
            });

            function Export()
            {
                alertUI("Not implmented on Release Build");
            }

            function Resize() {
                var containerHeight = $(window).height();
                $("#ListDataDiv").height(containerHeight - 370);
            }

            window.onload = function () {
                Resize();
                drawChart();
            }

            window.onresize = function () { Resize(); }

            google.charts.load('current', {packages:["orgchart"]});
            google.charts.setOnLoadCallback(drawChart);

            function drawChart() {
                var data = new google.visualization.DataTable();
                data.addColumn("string", "Name");
                data.addColumn("string", "Manager");
                data.addColumn("string", "ToolTip");
                data.addRows(graphRows);

                // Create the chart.
                var chart = new google.visualization.OrgChart(document.getElementById("chart_div"));
                chart.draw(data, { "allowHtml": true });
            }

        </script>
</asp:Content>



