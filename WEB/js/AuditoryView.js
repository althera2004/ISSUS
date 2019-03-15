var AuditoryTypeInterna = 0;
var AuditoryTypeExterna = 1;
var AuditoryTypeProveedor = 2;

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

    // Las auditorías planificads no pueden añadir normas
    if (Auditory.PlannedOn !== null && Auditory.PlannedOn !== "") {
        $("#AuditoryRulesDiv input").attr("disabled", "disabled");
        $("input").attr("disabled", "disabled");
        $("textarea").attr("disabled", "disabled");
        $("textarea").css("backgroundColor", "#eee");
        $("select").attr("disabled", "disabled");
        $("select").css("backgroundColor", "#eee");
        $("#BtnNewItem").remove();
        $("td .btn-info").remove();
        $("td .btn-danger").remove();
        $("#TxtNotes").removeAttr("disabled");
        $("#TxtNotes").css("backgroundColor", "transparent");
    } else {
        $("#CmbRules").chosen();
    }

    if (Auditory.Id > 0) {
        if (Auditory.Type === 1) {
            if (Auditory.Customer.Id > 0) {
                $("#CustomerDiv").show();
                $("#RBCustomer").attr("checked", "checked");
            }

            if (Auditory.Provider.Id > 0) {
                $("#ProviderDiv").show();
                $("#RBProvider").attr("checked", "checked");
            }
        }

        $("#TxtAmount").val(ToMoneyFormat(Auditory.Amount, 2));
    }

    $("#BtnSave").on("click", SaveAuditory);
};

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

        if (auditoryPlanningSelected.SendMail === true && Auditory.Type !== 1) {
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
                    "Id": "BtnAuditoryPlanningSaveOk",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        AuditoryPlanningSave();
                    }
                },
                {
                    "Id": "BtnAuditoryPlanningSaveCancel",
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

function RBExternalTypeChanged() {
    $("#ProviderDiv").hide();
    $("#CustomerDiv").hide();

    if (document.getElementById("RBProvider").checked === true) {
        $("#ProviderDiv").show();
    }

    if (document.getElementById("RBCustomer").checked === true) {
        $("#CustomerDiv").show();
    }
}

function SaveAuditory() {
    console.log("SaveAuditory");
    var customer = { "Id": -1 };
    var provider = { "Id": -1 };

    var auditoryData = {
        "Id": Auditory.Id,
        "CompanyId": Company.Id,
        "Type": Auditory.Type,
        "Description": $("#TxtName").val(),
        "Descripcion": $("#TxtDescription").val(),
        "Scope": $("#TxtScope").val(),
        "Amount": StringToNumber($("#TxtAmount").val(), ".", ","),
        "Notes": $("#TxtNotes").val(),
        "CompanyAddressId": $("#CmbAddress").val() * 1,
        "EnterpriseAddress": $("#CmbAddress").val(),
        "AuditorTeam": Auditory.Type === 0 ? "" : $("#TxtAuditorTeam").val(),
        "PlannedBy": { "Id": $("#CmbPlanningResponsible").val() * 1 },
        "PlannedOn": GetDate($("#TxtAuditoryPlanningDate").val(), "/", true),
        "InternalResponsible": { "Id": $("#CmbInternalResponsible").val() * 1 },
        "Active": true,
        "ValidatedBy": { "Id": -1 },
        "ValidatedUserBy": { "Id": -1 },
        "ClosedBy": { "Id": -1 },
        "CreatedBy": { "Id": -1 },
        "ModifiedBy": { "Id": -1 },
        "Customer": customer,
        "Provider": provider
    };

    var toPlanned = false;
    if (Auditory.PlannedOn === null && $("#TxtAuditoryPlanningDate").val() !== "") {
        toPlanned = true;
    }

    CalculateRules();
    var data = {
        "auditory": auditoryData,
        "rules": $("#TxtRulesId").val(),
        "applicationUserId": ApplicationUser.Id,
        "toPlanned": toPlanned
    };

    var webMethod = "/Async/AuditoryActions.asmx/Insert";
    if (Auditory.Id > 0) {
        webMethod = "/Async/AuditoryActions.asmx/Update";
        data["oldAuditory"] = Auditory;
    }

    console.log(data);
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (Auditory.Id < 0) {
                document.location = "/AuditoryView.aspx?id=" + msg.d.MessageError;
            }
            else {
                document.location = referrer;
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function CalculateRules() {
    var res = "";
    $.each($("#AuditoryRulesDiv input:checkbox"), function () {
        console.log($(this)[0].id);
        if ($(this)[0].checked === true) {
            res += $(this)[0].id.split("_")[1] + "|";
        }
    });

    $("#TxtRulesId").val(res);
}

function QuestionaryPlay(auditoryId, cuestionarioId) {
    window.open("/QuestionaryPlay.aspx?a=" + auditoryId + "&c=" + cuestionarioId);
}