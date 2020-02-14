<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-sm-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row" style="padding-bottom:8px;" id="SelectRow">
                                    <div class="col-xs-12">
                                        <div class="col-xs-2">
                                            <input type="checkbox" id="Chk1" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["DashBoard_SelectOwner"] %>
                                        </div>
                                        <div class="col-xs-3">
                                            <input type="checkbox" id="Chk2" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["DashBoard_SelectOthers"] %>
                                        </div>
                                        <div class="col-xs-4">
                                            <input type="checkbox" id="Chk3" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["DashBoard_OnlyPassed"] %>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin:0;margin-top:6px;">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th id="th0" class="search sort" onclick="Sort(this,'ListDataTable');"><%=this.Dictionary["Common_Task"] %></th>
                                                        <th id="th1" class="search sort" onclick="Sort(this,'ListDataTable');" style="width:350px;cursor:pointer;"><%=this.Dictionary["Item_Document_ListHeader_Origin"] %></th>
                                                        <th id="th2" class="search sort" onclick="Sort(this,'ListDataTable');" style="cursor:pointer;width:250px;"><%=this.Dictionary["Common_Responsible"] %></th>
                                                        <th id="th3" class="search sort" onclick="Sort(this,'ListDataTable', 'date');" style="width:107px;cursor:pointer;"><%=this.Dictionary["Common_Date"] %></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="LtScheduledTasks"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <td><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<strong><span id="TotalRows"></span></strong></td>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->                                        
                                    </div><!-- /span -->
                                </div>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">  
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript">
            var Filter = <%=this.Filter %>;
            var Tasks = <%=this.Tasks %>;

            function Resize() {
                var containerHeight = $(window).height();
                $("#ListDataDiv").height(containerHeight - 350);
            }

            window.onload = function () {
                SetFilter();
                RenderTaskTable();
                $("#th3").click();
                Resize();
                $(".page-header .col-sm-4").html("<button class=\"btn btn-success\" type=\"button\" id=\"BtnShowSchedule\" onclick=\"document.location='Schedule.aspx';\"><i class=\"icon-calendar bigger-110\"></i> " + Dictionary.DashBoard_Calendar + "</button>");
            }
            window.onresize = function () { Resize(); }

            function SetFilter() {
                console.log(Filter);
                document.getElementById("Chk1").checked = Filter.Owners;
                document.getElementById("Chk2").checked = Filter.Others;
                document.getElementById("Chk3").checked = Filter.Passed;
            }

            function FilterChanged() {
                $("#Chk1").removeAttr("disabled");
                $("#Chk2").removeAttr("disabled");
                if (document.getElementById("Chk1").checked === false) {
                    $("#Chk2").attr("disabled", "disabled");
                }
                if (document.getElementById("Chk2").checked === false) {
                    $("#Chk1").attr("disabled", "disabled");
                }

                RenderTaskTable();
                var filterData =
                    {
                        "owners": document.getElementById("Chk1").checked,
                        "others": document.getElementById("Chk2").checked,
                        "passed": document.getElementById("Chk3").checked
                    };

                $.ajax({
                    "type": "POST",
                    "url": "/Async/DashBoardActions.asmx/SetFilter",
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": JSON.stringify(filterData, null, 2),
                    "success": function (msg) { },
                    "error": function (msg) {
                        alertUI(msg.responseText);
                    }
                });
            }

            function RenderTaskTable() {
                var owners = document.getElementById("Chk1").checked;
                var others = document.getElementById("Chk2").checked;
                var passed = document.getElementById("Chk3").checked;
                $("#ListDataTable").html("");
                var count = 0;
                for (var x = 0; x < Tasks.length; x++) {
                    if (passed === true && Tasks[x].color !== "#f00") {
                        continue;
                    }

                    if (owners === true && others === true) {
                        RenderTaskRow(Tasks[x]);
                        count++;
                    }
                    else if (owners === true) {
                        if (user.Employee.Id === Tasks[x].ResponsibleId) {
                            RenderTaskRow(Tasks[x]);
                            count++;
                        }
                    }
                    else if (others === true) {
                        if (user.Employee.Id !== Tasks[x].ResponsibleId) {
                            RenderTaskRow(Tasks[x]);
                            count++;
                        }
                    }
                }

                $("#TotalRows").html(count);
            }

            function RenderTaskRow(task) {
                var target = document.getElementById("ListDataTable");
                var tr = document.createElement("TR");
                tr.style.cursor = "pointer";
                tr.location = task.location;
                tr.ResponsibleId = task.ResponsibleId;

                if (task.Type === "A") {
                    task.location = "/ActionView.aspx?" + task.location.split('?')[1];
                }
                tr.onclick = function () { document.location = task.location; };

                var tdName = document.createElement("TD");
                var tdTarget = document.createElement("TD");
                var tdResponsible = document.createElement("TD");
                var tdDate = document.createElement("TD");

                tdTarget.style.width = "350px";
                tdTarget.style.paddingLeft = "4px";
                tdResponsible.style.width = "250px";
                tdDate.style.width = "90px";
                tdDate.style.textAlign = "center";

                tdTarget.style.color = task.color;
                tdName.style.color = task.color;
                tdResponsible.style.color = task.color;
                tdDate.style.color = task.color;

				
				var divName = document.createElement("DIV");
                divName.style.textOverflow = "ellipsis";
                divName.style.overflow = "hidden";
                divName.style.whiteSpace = "nowrap";
                divName.style.width = $("#th0").width()-18 + "px";
                divName.appendChild(document.createTextNode(task.labelType));
                tdName.title = task.labelType;
                tdName.appendChild(divName);				
				
                tdDate.appendChild(document.createTextNode(task.Date));

                var divTarget = document.createElement("DIV");
                divTarget.style.textOverflow = "ellipsis";
                divTarget.style.overflow = "hidden";
                divTarget.style.whiteSpace = "nowrap";
                divTarget.style.width = "310px";
                divTarget.appendChild(document.createTextNode(task.Item));
                tdTarget.title = task.Item;
                tdTarget.appendChild(divTarget);

                tdResponsible.appendChild(document.createTextNode(task.Responsible));
                if (task.Provider !== "") {
                    tdResponsible.appendChild(document.createElement("BR"));
                    var bold = document.createElement("STRONG");
                    bold.appendChild(document.createTextNode(task.Provider));
                    tdResponsible.appendChild(bold);
                }

                tr.appendChild(tdName);
                tr.appendChild(tdTarget);
                tr.appendChild(tdResponsible);
                tr.appendChild(tdDate);

                target.appendChild(tr);
            }

            if (user.Admin !== true) {
                $("#SelectRow").hide();
            }
        </script>
</asp:Content>