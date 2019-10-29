var selectedRows = [];

function LearningUpdate(id) {
    document.location = "FormacionView.aspx?id=" + id;
}

function LearningDeleteDisabled() {
    alertUI(Dictionary.Common_ErrorMessage_CanNotDelete);
}

function LearningDeleteDisabled1() {
    alertUI(Dictionary.Item_Learning_ErrorMessage_NoDeleteFinished);
}

function LearningDeleteDisabled2() {
    alertUI(Dictionary.Item_Learning_ErrorMessage_NoDeleteEvaluated);
}

function selectRow(sender) {
    var id = sender.id.split("|")[0];
    var learningId = sender.id.split("|")[1];
    var employeeId = sender.id.split("|")[2];

    var selectedRowsTemp = new Array();
    var passed = false;

    for (var x = 0; x < selectedRows.length; x++) {
        if (selectedRows[x].AssistantId !== id) {
            selectedRowsTemp.push(selectedRows[x]);
        }
        else {
            if (sender.checked) {
                selectedRowsTemp.push(selectedRows[x]);
                passed = true;
            }
        }
    }

    if (sender.checked && !passed) {
        selectedRowsTemp.push({ AssistantId: id, LearningId: learningId, EmployeeId: employeeId });
    }

    selectedRows = selectedRowsTemp;
}

function RestoreFilter() {
    document.getElementById("status0").checked = true;
    document.getElementById("status1").checked = true;
    document.getElementById("status2").checked = true;
    document.getElementById("status3").checked = true;
    $("#TxtDateFrom").val("");
    $("#TxtDateTo").val("");
    Go();
}

function Go() {
    CheckBoxLayout();    
    SetFilter();
    RenderLearningTable();
}

function CheckBoxLayout() {
    $("#status0").removeAttr("disabled");
    $("#status1").removeAttr("disabled");
    $("#status2").removeAttr("disabled");
    $("#status3").removeAttr("disabled");

    var selected = document.getElementById("status0").checked ? 1 : 0;
    selected += document.getElementById("status1").checked ? 1 : 0;
    selected += document.getElementById("status2").checked ? 1 : 0;
    selected += document.getElementById("status3").checked ? 1 : 0;

    if (selected === 1) {
        if (document.getElementById("status0").checked === true) { $("#status0").attr("disabled", "disabled"); }
        if (document.getElementById("status1").checked === true) { $("#status1").attr("disabled", "disabled"); }
        if (document.getElementById("status2").checked === true) { $("#status2").attr("disabled", "disabled"); }
        if (document.getElementById("status3").checked === true) { $("#status3").attr("disabled", "disabled"); }
    }
}

function LearningDelete(id, description) {
    $("#LearningName").html(description);
    $("#LearningDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 500,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        "buttons": [
            {
                "id": "BtnLearningDeleteOk",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    var ok = true;
                    if (ok === false) {
                        window.scrollTo(0, 0);
                        return false;
                    }

                    $(this).dialog("close");
                    LearningDeleteConfirmed(id);
                }
            },
            {
                "id": "BtnLearningDeleteCancel",
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

function LearningDeleteConfirmed(id) {
    var data = {
        "learningId": id,
        "companyId": Company.Id,
        "userId": user.Id,
        "reason": "" //document.getElementById('TxtNewReason').value
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/LearningActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                document.location = document.location + "";
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            alertUI(jqXHR.responseText);
        }
    });
}

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));

    $("#BtnCompleted").on("click", function (e) {
        var data = {
            "companyId": Company.Id,
            "assistants": selectedRows,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/LearningActions.asmx/Complete",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (response) {
                if (response.d.Success === true) {
                    document.location = document.location + "";
                }

                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    });

    $("#BtnSuccess").on("click", function (e) {
        var data = {
            "companyId": Company.Id,
            "assistants": selectedRows,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/LearningActions.asmx/Success",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (response) {
                if (response.d.Success === true) {
                    document.location = document.location + "";
                }

                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    });

    $("#BtnDelete").on("click", function (e) {
        var data = {
            "companyId": Company.Id,
            "assistants": selectedRows,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/LearningActions.asmx/Delete",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (response) {
                if (response.d.Success === true) {
                    document.location = document.location + "";
                }
                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            "error": function (msg) {
                alertUI(msg.responseText);
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
    var status = "";
    if (document.getElementById("status0").checked === true) { status += "0"; }
    if (document.getElementById("status1").checked === true) { status += "1"; }
    if (document.getElementById("status2").checked === true) { status += "2"; }
    if (document.getElementById("status3").checked === true) { status += "3"; }
    var data = {
        "companyId": Company.Id,
        "yearFrom": $("#TxtDateFrom").val(),
        "yearTo": $("#TxtDateTo").val(),
        "mode": status,
        "listOrder": listOrder,
        "textFilter": $("#nav-search-input").val()
    };

    console.log("Export",data);

    //LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/FormacionExportList.aspx/PDF",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            //LoadingHide();
            var link = document.createElement("a");
            link.id = "download";
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = "_blank";
            document.body.appendChild(link);
            document.body.removeChild(link);
            $("#download").trigger("click");
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog("close");
        },
        "error": function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}

function SetFilter() {
    var filter = "";
    if (document.getElementById("status0").checked === true) { filter += "0"; }
    if (document.getElementById("status1").checked === true) { filter += "1"; }
    if (document.getElementById("status2").checked === true) { filter += "2"; }
    if (document.getElementById("status3").checked === true) { filter += "3"; }
    filter += "|";
    filter += $("#TxtDateFrom").val();
    filter += "|";
    filter += $("#TxtDateTo").val();

    var data = { "filter": filter };
    //LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/LearningActions.asmx/SetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            console.log("SetFilter", "OK");
        },
        "error": function (msg) {
            console.log("SetFilter", msg.responseText);
        }
    });
}

window.onload = function () {
    // Set filter
    // ---------------------------------------
    console.log("Filter", Filter);
    var modes = Filter.split('|')[0];
    document.getElementById("status0").checked = modes.indexOf("0") !== -1;
    document.getElementById("status1").checked = modes.indexOf("1") !== -1;
    document.getElementById("status2").checked = modes.indexOf("2") !== -1;
    document.getElementById("status3").checked = modes.indexOf("3") !== -1;
    $("#TxtDateFrom").val(Filter.split('|')[1]);
    $("#TxtDateTo").val(Filter.split('|')[2]);


    CheckBoxLayout();
    // ---------------------------------------


    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
    $(".page-header .col-sm-8").addClass("col-sm-6");
    $(".page-header .col-sm-8").removeClass("col-sm-8");
    $(".page-header .col-sm-4").addClass("col-sm-6");
    $(".page-header .col-sm-4").removeClass("col-sm-4");
    $("#th0").click();
	var options = $.extend({}, $.datepicker.regional[userLanguage], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);	
	$("#TxtDateFrom").on("change", DateChange);
    $("#TxtDateTo").on("change", DateChange);

    Resize();
    RenderLearningTable();
};

window.onresize = function () {
    Resize();
};

function DateChange(){
	$("#TxtDateFromErrorDateRange").hide();
	$("#TxtDateToErrorDateRange").hide();
    $("#TxtDateFromLabel").css("color", "#000");
    $("#TxtDateToLabel").css("color", "#000");
    var dateFrom = GetDate($("#TxtDateFrom").val(), "/", false);
    var dateTo = GetDate($("#TxtDateTo").val(), "/", false);
	console.log(dateFrom, dateTo);
	
	var ok = true;
	
    if ($("#TxtDateFrom").val() !== "" && $("#TxtDateTo").val() !== "") {
        if (dateFrom > dateTo) {
            $("#TxtDateFromLabel").css("color", Color.Error);
            $("#TxtDateToLabel").css("color", Color.Error);
            $("#TxtDateFromErrorDateRange").show();
            $("#TxtDateToErrorDateRange").show();
            ok = false;
        }
    }

    if (ok === true) {
        Go();
    }
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 410);
}

var tableTotal = 0;
var tableCount = 0;
function RenderLearningTable() {
    tableCount = 0;
    tableTotal = 0;
    $("#ListDataTable").html("");
    $("#ListDataDiv").hide();
    $("#NoData").hide();

    if (learningData.length > 0) {
        var tableContent = "";
        for (var x = 0; x < learningData.length; x++) {
            if (learningData[x].Status === 0 && document.getElementById("status0").checked === false) { continue; }
            if (learningData[x].Status === 1 && document.getElementById("status1").checked === false) { continue; }
            if (learningData[x].Status === 2 && document.getElementById("status2").checked === false) { continue; }
            if (learningData[x].Status === 3 && document.getElementById("status3").checked === false) { continue; }

            if ($("#TxtDateFrom").val() !== "") {
                var from = GetDate($("#TxtDateFrom").val(), "/", false);
                var compareStart = GetDate(learningData[x].DateEstimated, "/", false);
                if (typeof learningData[x].RealStart !== "undefined" && learningData[x].RealStart !== null && learningData[x].RealStart !== "") {
                    compareStart = GetDate(learningData[x].RealStart, "/", false);
                }

                if (compareStart < from) { continue; }
            }

            if ($("#TxtDateTo").val() !== "") {
                var to = GetDate($("#TxtDateTo").val(), "/", false);
                var compareFinish = null;

                if (typeof learningData[x].RealFinish !== "undefined" && learningData[x].RealFinish !== null && learningData[x].RealFinish !== "") {
                    compareFinish = GetDate(learningData[x].RealFinish, "/", false);
                }
                else if (typeof learningData[x].RealStart !== "undefined" && learningData[x].RealStart !== null && learningData[x].RealStart !== "") {
                    compareFinish = GetDate(learningData[x].RealStart, "/", false);
                }

                if (compareFinish !== null && compareFinish > to) { continue; }
            }

            tableContent += RenderLearningRow(learningData[x]);
            tableCount++;
            tableTotal += learningData[x].Amount;
        }

        $("#ListDataTable").html(tableContent);
        $("#ListDataDiv").show();
    }

    if (tableCount === 0) {
        $("#NoData").show();
    }

    $("#TotalList").html(tableCount);
    $("#TotalAmount").html(ToMoneyFormat(tableTotal,2));
}

function RenderLearningRow(data) {
/*<tr></tr>*/
    var realStart = "";
    if (typeof data.Realstart !== "undefined" && data.Realstart !== null && data.Realstart !== "") {
        realStart = data.Realstart;
    }

    if (realStart === "") {
        if (typeof data.DateEstimated !== "undefined" && data.DateEstimated !== null && data.DateEstimated !== "") {
            realStart = data.DateEstimated;
        }
    }

    var realFinish = "";
    var colorFinish = "";
    if (typeof data.RealFinish !== "undefined" && data.RealFinish !== null && data.RealFinish !== "") {
        if (data.RealFinish.indexOf("/1970") !== -1) {
            colorFinish = "color:transparent;";
        }

        realFinish = data.RealFinish;
    }

    var estadoText = Dictionary.Item_Learning_Status_InProgress;
    switch (data.Status) {
        case 1:
            estadoText = Dictionary.Item_Learning_Status_Started;
            break;
        case 2:
            estadoText = Dictionary.Item_Learning_Status_Finished;
            break;
        case 3:
            estadoText = Dictionary.Item_Learning_Status_Evaluated;
            break;
    }

    var res = "";
    res += "<tr><td>";
    res += "    <a href=\"FormacionView.aspx?id=" + data.Id + "\">" + data.Description + "</a>";
    res += "  </td><td align=\"center\" style=\"width:100px;white-space:nowrap;\">" + realStart;
    res += "  </td><td align=\"center\" style=\"" + colorFinish + "width:100px;white-space:nowrap;\">" + realFinish;
    res += "  </td><td align=\"center\" class=\"hidden-480\" style=\"width:100px;white-space:nowrap;\">" + estadoText;
    res += "  </td><td align=\"right\" class=\"hidden-480\" style=\"width:150px;white-space:nowrap;\">" + ToMoneyFormat(data.Amount, 2);
    res += "  </td><td class=\"hidden-480\" style=\"width:90px;white-space:nowrap;\">";
    res += "    <span title=\"Editar " + data.Id + "\" class=\"btn btn-xs btn-info\" onclick=\"LearningUpdate(" + data.Id + ");\">";
    res += "      <i class=\"icon-edit bigger-120\"></i>";
    res += "    </span>&nbsp;";
    res += "    <span title=\"Eliminar " + data.Id + "\" class=\"btn btn-xs btn-danger\" onclick=\"LearningDelete(" + data.Id + ");\">";
    res += "      <i class=\"icon-trash bigger-120\"></i>";
    res += "    </span>";
    res += "  </td>";
    res += "</tr>";
    return res;
}