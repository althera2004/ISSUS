$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
    "_title": function (title) {
        var $title = this.options.title || "&nbsp;"
        if (("title_html" in this.options) && this.options.title_html == true) {
            title.html($title);
        }
        else {
            title.text($title);
        }
    }
}));

var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { "dateFormat": "dd/mm/yy", "autoclose": true, "todayHighlight": true });
$(".date-picker").datepicker(options);
$(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

window.onload = function () {
    $("#nav-search").hide();
    if (Auditory.Id > 0) {
        RenderPlanningTable();
    }
}

function RenderPlanningTable()
{
    $("#scrollTableDiv").show();
    $("#ListDataDiv").hide();
    $("#NoData").hide();
    $("#TablePlanningHeader").show();

    if (AuditoryPlanning.length > 0) {
        var res = "";
        for (var x = 0; x < AuditoryPlanning.length; x++) {
            var dateText = AuditoryPlanning[x].Date;
            if (typeof dateText === "object") {
                dateText = FormatDate(dateText, "/");
            }

            res += "<tr id=\"" + AuditoryPlanning[x].Id + "\">";
            res += "  <td style=\"width:100px;text-align:center\">" + dateText + "</td>";
            res += "  <td style=\"width:70px;text-align:center\">" + MinutesToHour(AuditoryPlanning[x].Hour) + "</td>";
            res += "  <td style=\"width:90px;text-align:right\">" + AuditoryPlanning[x].Duration + "</td>";
            res += "  <td style=\"\">" + AuditoryPlanning[x].Process.Description + "</td>";
            res += "  <td style=\"width:150px;\">" + AuditoryPlanning[x].Auditor.Value + "</td>";
            res += "  <td style=\"width:150px;\">" + AuditoryPlanning[x].Audited.Value;
            if (AuditoryPlanning[x].SendMail === true) { res += " <i class=\"icon-envelope\"></i>"; }
            res += "</td>";
            res += "  <td style=\"width:90px;\">";
            res += "    <span class=\"btn btn-xs btn-info\" id=\"" + AuditoryPlanning[x].Id + "\" onclick=\"ShowPopupPlanningDialog(" + AuditoryPlanning[x].Id + ");\">";
            res += "        <i class=\"icon-edit bigger-120\"></i>";
            res += "    </span>";
            res += "    <span class=\"btn btn-xs btn-danger\" id=\"" + AuditoryPlanning[x].Id + "\" onclick=\"ShowAuditoryPlanningDeleteDialog(" + AuditoryPlanning[x].Id + ");\">";
            res += "      <i class=\"icon-trash bigger-120\"></i>";
            res += "    </span>";
            res += "  </td>";
            res += "</tr>";
        }

        $("#ListDataDiv").show();
    }
    else {
        $("#NoData").show();
    }

    $("#SpanPlanningTotal").html(AuditoryPlanning.length);
    $("#PlanningDataTable").html(res);
}

var auditoryPlanningSelectedId = null;
var auditoryPlanningSelected = null;
function ShowPopupPlanningDialog(id) {
    $("#TxtProviderEmailRow").hide();
    auditoryPlanningSelectedId = id;
    auditoryPlanningSelected = AuditoryPlanningGetById(id);
    if (auditoryPlanningSelected === null) {

    }

    var title = Dictionary.Item_AuditoryPlanning_Title_PopupAdd;
    if (id < 0) {
        title = Dictionary.Item_AuditoryPlanning_Title_PopupUpdate;
        $("#TxtPlanningDate").val(FormatDate(new Date, "/"));
        $("#TxtHour").val("");
        $("#TxtDuration").val("");
        $("#CmbProcess").val(0);
        $("#CmbAuditor").val(0);
        $("#CmbAudited").val(0);
        $("#ChkSendMail").removeAttr("checked");
        $("#TxtProviderEmail").val("");
    }
    else {
        $("#TxtPlanningDate").val(auditoryPlanningSelected.Date);
        $("#TxtHour").val(auditoryPlanningSelected.Hour);
        $("#TxtDuration").val(auditoryPlanningSelected.Duration);
        $("#CmbProcess").val(auditoryPlanningSelected.Process.Id);
        $("#CmbAuditor").val(auditoryPlanningSelected.Auditor.Id);
        $("#CmbAudited").val(auditoryPlanningSelected.Audited.Id);

        if (auditoryPlanningSelected.SendMail === true && Auditory.Type === 2) {
            $("#ChkSendMail").attr("checked", "checked");
            $("#TxtProviderEmailRow").show();
        }
        else {
            $("#ChkSendMail").removeAttr("checked");
            $("#TxtProviderEmailRow").hide();
        }

        $("#TxtProviderEmail").val(auditoryPlanningSelected.ProviderEmail);
    }

    var dialog = $("#PopupPlanningDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + title + "</h4>",
        "title_html": true,
        "width": 400,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        AuditoryPlanningSave();
                    }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function AuditoryPlanningSave() {
    auditoryPlanningSelected = {
        "Id": auditoryPlanningSelectedId,
        "CompanyId": Company.Id,
        "AuditoryId": Auditory.Id,
        "Date": GetDate($("#TxtPlanningDate").val(), "/"),
        "Hour": HourToMinutes($("#TxtHour").val()),
        "Duration": ParseInputValueToNumber($("#TxtDuration").val()),
        "Process": { "Id": $("#CmbProcess").val() * 1, "Description": $("#CmbProcess option:selected").text() },
        "Auditor": { "Id": $("#CmbAuditor").val() * 1, "Value": $("#CmbAuditor option:selected").text() },
        "Audited": { "Id": $("#CmbAudited").val() * 1, "Value": $("#CmbAudited option:selected").text() },
        "SendMail": document.getElementById("ChkSendMail").checked,
        "ProviderEmail": $("#TxtProviderEmail").val(),
        "Active": true
    };

    var data = {
        "planning": auditoryPlanningSelected,
        "applicationUserId": ApplicationUser.Id
    };

    var webMethod = "/Async/AuditoryActions.asmx/PlanningInsert";
    if (auditoryPlanningSelectedId > 0) {
        webMethod = "/Async/AuditoryActions.asmx/PlanningUpdate";
    }

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (auditoryPlanningSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < AuditoryPlanning.length; x++) {
                    if (AuditoryPlanning[x].Id === auditoryPlanningSelectedId) {
                        temp.push(auditoryPlanningSelected);
                    }
                    else {
                        temp.push(AuditoryPlanning[x]);
                    }
                }

                AuditoryPlanning = temp;
            }
            else {
                auditoryPlanningSelected.Id = msg.d.MessageError * 1;
                AuditoryPlanning.push(auditoryPlanningSelected);
            }

            $("#PopupPlanningDialog").dialog("close");
            RenderPlanningTable();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ShowAuditoryPlanningDeleteDialog(id) {
    auditoryPlanningSelectedId = id;
    auditoryPlanningSelected = AuditoryPlanningGetById(id);
    var dialog = $("#AuditoryPlanningDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_AuditoryPlanning_Title + "</h4>",
        "title_html": true,
        "width": 400,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        AuditoryPlanningDeleteConfirmed();
                    }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function AuditoryPlanningDeleteConfirmed() {
    var data = {
        "planningId": auditoryPlanningSelectedId,
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id,
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/PlanningDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            var temp = [];
                for (var x = 0; x < AuditoryPlanning.length; x++) {
                    if (AuditoryPlanning[x].Id !== auditoryPlanningSelectedId) {
                        temp.push(AuditoryPlanning[x]);
                    }
                }

            AuditoryPlanning = temp;
            $("#AuditoryPlanningDeleteDialog").dialog("close");
            RenderPlanningTable();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function AuditoryPlanningGetById(id) {
    for (var x = 0; x < AuditoryPlanning.length; x++) {
        if (AuditoryPlanning[x].Id === id) {
            return AuditoryPlanning[x];
        }
    }
    return null;
}