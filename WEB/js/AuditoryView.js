var AppWindow = null;
var foundSelectedId = null;
var foundSelected = null;
var improvementSelectedId = null;
var improvementSelected = null;
var auditoryPlanningSelectedId = null;
var auditoryPlanningSelected = null;
var AuditoryTypes = {
    "Interna": 0,
    "Externa": 1,
    "Proveedor": 2
};

var AuditoryStatus = {
    "Planificando": 0,
    "Planificada": 1,
    "EnCurso": 2,
    "Pendiente": 3,
    "Cerrada": 4,
    "Validada": 5
};

$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
    "_title": function (title) {
        var $title = this.options.title || "&nbsp;";
        if (("title_html" in this.options) && this.options.title_html === true) {
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

window.onblur = function () {
    console.log("blur");
};

window.onfocus = function () {
    //document.location = document.location + "";
};

function Reload() {
    document.location = "AuditoryView.aspx?id=" + Auditory.Id + "&t=2";
}

window.onload = function () {
    $("#nav-search").hide();
    $("#BtnCancel").on("click", function () { document.location = "/AuditoryList.aspx"; });
    if (Auditory.Id > 0) {
        RenderPlanningTable();
        var text = eval("Dictionary.Item_Adutory_Status_Label_" + Auditory.Status);
        $("h1").append(" <i>(" + text + ")</i>");
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

        if (getParameterByName("t") === "2") {
            $("#tabQuestionaries").click();
        }
    } else {
        $("#CmbRules").chosen();
        if (Auditory.Type !== AuditoryTypes.Externa) {
            if (Auditory.Id > 0) {
                $("#DivNewAuditory").hide();
            }
            else {
                $("#DivNewAuditory").show();
                $("#DivYesPlanning").hide();
            }
        }
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
    $("#BtnCloseAuditoria").on("click", PopupCloseShow);
    $("#BtnReopenAuditoria").on("click", PopupReopenShow);
    $("#BtnValidarAuditoria").on("click", PopupValidarShow);

    if (Completo === true) {
        $("#CloseCuestionarioDIV").show();
        //$("#TxtCloseQuestionsOn").removeAttr("disabled");
        //$("#TxtCloseQuestionsOn").css("background", "transparent");
    }

    if (Auditory.Status > AuditoryStatus.EnCurso) {
        $("#TxtCloseQuestionsOn").attr("disabled", "disabled");
        $("#TxtCloseQuestionsOn").css("background", "#ccc");
        $("#CuestionarioDataTable .btn-success").hide();
    }
    else if (Auditory.Status === AuditoryStatus.EnCurso) {
        $("#TxtStartQuestionsOn").removeAttr("disabled");
        $("#TxtStartQuestionsOn").css("background", "transparent");
        //$("#TxtCloseQuestionsOn").removeAttr("disabled");
        //$("#TxtCloseQuestionsOn").css("background", "transparent");
        $("#BtnCloseCuestionarios").show();
    }

    if (Auditory.Status > AuditoryStatus.Pendiente) {
        $("#HallazgosDataTable .btn").hide();
        $("#MejorasDataTable .btn").hide();
    }

    if (Auditory.Status === AuditoryStatus.Pendiente) {
        $("#DivCloseButton").show();
        $("#BtnReopenCuestionarios").show();
    }

    if (Auditory.Status === AuditoryStatus.Cerrada) {
        $("#DivValidationButton").show();
        $("#DivCloseResume").show();
    }

    if (Auditory.Status === AuditoryStatus.Validada) {
        $("#DivCloseResume").removeClass("col-sm-12");
        $("#DivValidationResume").removeClass("col-sm-12");
        $("#DivCloseResume").addClass("col-sm-6");
        $("#DivValidationResume").addClass("col-sm-6");
        $("#DivValidationResume").show();
        $("#DivCloseResume").show();
        $("#HallazgosDataTable .btn").hide();
        $("#ListDataDivMejoras .btn").hide();
        RenderRealActions();
    }

    // Externas
    // ------------------------------------------------

    if (Auditory.Type === AuditoryTypes.Externa) {
        $("#DivNoPlanning").remove();
        $("#DivNoQuestions").remove();
        $("#DivYesPlanning").show();

        if (Auditory.Status === AuditoryStatus.Planificando) {
            $("#CmbPlanningResponsible").removeAttr("disabled");
            $("#TxtAuditoryPlanningDate").removeAttr("disabled");
        }
        else {
            if (Auditory.Status === AuditoryStatus.Planificada || Auditory.Status === AuditoryStatus.EnCurso) {
                $("#BtnCloseAuditoria").hide();
            }
        }

        if (Auditory.Status < AuditoryStatus.Cerrada) {
            $("#TxtReportStart").removeAttr("disabled");
            $("#TxtReportEnd").removeAttr("disabled");
            $("#BtnActionAdd").on("click", function () { IncidentActionShowPopup(-1); });
            RenderZombies();
            $("#DivCloseButton").show();
        }

        if (Auditory.Status === AuditoryStatus.Cerrada) {
            RenderZombies();
            $("#DivClosedResume").show();
            $("#BtnActionAdd").hide

            $("#scrollTableDivIncidentActions").hide();
            $("#scrollTableDivIncidentActionsReal").show();
            RenderRealActions();
        }
        else {
            $("#scrollTableDivIncidentActions").show();
            $("#scrollTableDivIncidentActionsReal").hide();
        }
    }
    // ------------------------------------------------

    $(".timepicker").timepicker({
        timeFormat: "G:i"
    });

    if (Auditory.Status === AuditoryStatus.Planificando) {
        CalculeTotalQuestions();
    }
    else {
        RenderCuestionarios();
        RenderFounds();
        RenderImprovements();
    }
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

    if (AuditoryPlanning.length < 1) {
        $("#DivNoPlanning").show();
        $("#DivYesPlanning").hide();
        $("#CmbPlanningResponsible").attr("disabled", "disabled");
        $("#TxtAuditoryPlanningDate").attr("disabled", "disabled");
    } else {
        $("#DivNoPlanning").hide();
        $("#DivYesPlanning").show();
        $("#CmbPlanningResponsible").removeAttr("disabled");
        $("#TxtAuditoryPlanningDate").removeAttr("disabled");
    }
}

function ShowPopupPlanningDialog(id) {
    AuditoringPlanningReset();
    $("#TxtProviderEmailRow").hide();
    auditoryPlanningSelectedId = id;
    auditoryPlanningSelected = AuditoryPlanningGetById(id);
    /*if (auditoryPlanningSelected === null) {

    }*/

    var title = Dictionary.Item_AuditoryPlanning_Title_PopupUpdate;
    if (id < 0) {
        title = Dictionary.Item_AuditoryPlanning_Title_PopupAdd;
        $("#TxtPlanningDate").val(FormatDate(new Date, "/"));
        $("#TxtHour").val("");
        $("#TxtDuration").val("");
        $("#CmbProcess").val(-1);
        $("#CmbAuditor").val(-1);
        $("#CmbAudited").val(-1);
        $("#ChkSendMail").removeAttr("checked");
        $("#TxtProviderEmail").val("");
    }
    else {
        if (typeof auditoryPlanningSelected.Date === "object") {
            $("#TxtPlanningDate").val(FormatDate(auditoryPlanningSelected.Date,"/"));
        }
        else {
            $("#TxtPlanningDate").val(auditoryPlanningSelected.Date);
        }
        
        $("#TxtHour").val(MinutesToHour(auditoryPlanningSelected.Hour));
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
        ChkSendMailChanged();
    }

    $("#PopupPlanningDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + title + "</h4>",
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnAuditoryPlanningSaveOk",
                    "html": "<i class=\"icon-save bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        AuditoryPlanningSave();
                    }
                },
                {
                    "Id": "BtnAuditoryPlanningSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function AuditoringPlanningReset() {
    $("#CmbProcessLabel").css("color", "#333");
    $("#TxtPlanningDateLabel").css("color", "#333");
    $("#TxtHourLabel").css("color", "#333");
    $("#TxtDurationLabel").css("color", "#333");
    $("#CmbAuditorLabel").css("color", "#333");
    $("#CmbAuditedLabel").css("color", "#333");
    $("#TxtProviderEmailLabel").css("color", "#333");
    $("#CmbProcessErrorRequired").hide();
    $("#TxtPlanningDateErrorRequired").hide();
    $("#TxtPlanningDateMalformed").hide();
    $("#TxtHourRequired").hide();
    $("#TxtDurationRequired").hide();
    $("#TxtDurationMalformed").hide();
    $("#CmbAuditorErrorRequired").hide();
    $("#CmbAuditedErrorRequired").hide();
    $("#CmbAuditedErrorSame").hide();
    $("#TxtProviderEmailErrorRequired").hide();
    $("#TxtProviderEmailMalformed").hide();
}

function AuditoryPlanningValidate() {
    AuditoringPlanningReset();
    var ok = true;

    if ($("#TxtPlanningDate").val() === "") {
        ok = false;
        $("#TxtPlanningDateLabel").css("color", "#f00");
        $("#TxtPlanningDateErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtPlanningDate").val()) === false) {
            $("#TxtPlanningDateLabel").css("color", "#f00");
            $("#TxtPlanningDateMalformed").show();
        }
    }

    if ($("#TxtHour").val() === "") {
        ok = false;
        $("#TxtHourLabel").css("color", "#f00");
        $("#TxtHourRequired").show();
    }

    if ($("#TxtDuration").val() === "") {
        ok = false;
        $("#TxtDurationLabel").css("color", "#f00");
        $("#TxtDurationRequired").show();
    }

    if ($("#CmbProcess").val() * 1 < 1) {
        ok = false;
        $("#CmbProcessLabel").css("color", "#f00");
        $("#CmbProcessErrorRequired").show();
    }

    if ($("#CmbAuditor").val() * 1 < 1) {
        ok = false;
        $("#CmbAuditorLabel").css("color", "#f00");
        $("#CmbAuditorErrorRequired").show();
    }

    if ($("#CmbAudited").val() * 1 < 1) {
        ok = false;
        $("#CmbAuditedLabel").css("color", "#f00");
        $("#CmbAuditedErrorRequired").show();
    }

    if ($("#CmbAuditor").val() * 1 > 0) {
        var userId = $("#CmbAuditor").val() * 1;
        var employeeId = -1;
        for (var x = 0; x < UserEmployess.length; x++) {
            if (UserEmployess[x].U === userId) {
                employeeId = UserEmployess[x].E;
                break;
            }
        }

        if (employeeId > 0) {
            if (employeeId === $("#CmbAudited").val() * 1) {
                ok = false;
                $("#CmbAuditorLabel").css("color", "#f00");
                $("#CmbAuditedLabel").css("color", "#f00");
                $("#CmbAuditedErrorSame").show();
            }
        }
    }

    if (Auditory.Type === AuditoryTypes.Proveedor) {
        if (document.getElementById("ChkSendMail").checked === true) {
            if ($("#TxtProviderEmail").val() === "") {
                ok = false;
                $("#TxtProviderEmailErrorRequired").show();
            }
            else {
                if (validateEmail($("#TxtProviderEmail").val()) === false) {
                    ok = false;
                    $("#TxtProviderEmailMalformed").show();

                }
            }
        }
    }

    return ok;
}

function AuditoryPlanningSave() {
    if (AuditoryPlanningValidate() === false) { return false; }
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
            CalculeTotalQuestions();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ShowAuditoryPlanningDeleteDialog(id) {
    auditoryPlanningSelectedId = id;
    auditoryPlanningSelected = AuditoryPlanningGetById(id);
    $("#AuditoryPlanningDeleteDialog").removeClass("hide").dialog({
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
    $("#ErrorProviderCustomerDiv").hide();
    $("#ProviderCustomerErrorRequired").hide();
    $("#RBProvider").parent().css("color", "#333");
    $("#RBCustomer").parent().css("color", "#333");
    if (document.getElementById("RBProvider").checked === true) { $("#ProviderDiv").show(); }
    if (document.getElementById("RBCustomer").checked === true) { $("#CustomerDiv").show(); }
}

function AuditoryValidate() {
    var ok = true;
    if ($("#TxtName").val() === "") {
        ok = false;
        $("#TxtNameLabel").css("color", "#f00");
        $("#TxtNameErrorRequired").show();
    }

    if ($("#TxtAmount").val() === "") {
        ok = false;
        $("#TxtAmountLabel").css("color", "#f00");
        $("#TextAmountErrorRequired").show();
    }

    if ($("#TxtDescription").val() === "") {
        ok = false;
        $("#TxtDescriptionLabel").css("color", "#f00");
        $("#TextDescriptionErrorRequired").show();
    }

    if ($("#TxtScope").val() === "") {
        ok = false;
        $("#TxtScopeLabel").css("color", "#f00");
        $("#TxtScopeErrorRequired").show();
    }

    CalculateRules();
    if ($("#TxtRulesId").val() === "") {
        ok = false;
        $("#TxtRulesIdLabel").css("color", "#f00");
        $("#TxtRulesIdErrorRequired").show();
    }

    if ($("#CmbInternalResponsible").val() * 1 < 0) {
        ok = false;
        $("#CmbInternalResponsibleLabel").css("color", "#f00");
        $("#CmbInternalResponsibleErrorRequired").show();
    }

    if (Auditory.Type === AuditoryTypes.Interna) {
        if ($("#TxtAuditorTeam").val() === "") {
            ok = false;
            $("#TxtAuditorTeamLabel").css("color", "#f00");
            $("#TxtAuditorTeamErrorRequired").show();
        }
    }

    if (Auditory.Type === AuditoryTypes.Proveedor) {
        if ($("#CmbProvider").val() * 1 < 0) {
            ok = false;
            $("#CmbProviderLabel").css("color", "#f00");
            $("#CmbProviderErrorRequired").show();
        }

        if ($("#TxtAddress").val() === "") {
            ok = false;
            $("#TxtAddressLabel").css("color", "#f00");
            $("#TxtAddressErrorRequired").show();
        }
    }
    else {
        if ($("#CmbAddress").val() * 1 < 0) {
            ok = false;
            $("#CmbAddressLabel").css("color", "#f00");
            $("#CmbAddressErrorRequired").show();
        }
    }

    if (Auditory.Type === AuditoryTypes.Externa) {
        if ($("#TxtAuditorTeam").val() === "") {
            ok = false;
            $("#TxtAuditorTeamLabel").css("color", "#f00");
            $("#TxtAuditorTeamErrorRequired").show();
        }

        if ($("#TxtPreviewDate").val() === "") {
            ok = false;
            $("#TxtPreviewDateLabel").css("color", "#f00");
            $("#TxtPreviewDateErrorRequired").show();
        }
        else {
            if (validateDate($("#TxtPreviewDate").val()) === false) {
                ok = false;
                $("#TxtPreviewDateLabel").css("color", "#f00");
                $("#TxtPlannedDateErrorMailMalformed").show();
            }
        }

        if (document.getElementById("RBProvider").checked === false && document.getElementById("RBCustomer").checked === false) {
            ok = false;
            $("#ErrorProviderCustomerDiv").show();
            $("#ProviderCustomerErrorRequired").show();
            $("#RBProvider").parent().css("color", "#f00");
            $("#RBCustomer").parent().css("color", "#f00");
        }
        else if (document.getElementById("RBProvider").checked === true) {
            if ($("#CmbProvider").val() * 1 < 1) {
                ok = false;
                $("#RBProvider").parent().css("color", "#f00");
                $("#CmbProviderErrorRequired").show();
            }
        }
        else {
            if ($("#CmbCustomer").val() * 1 < 1) {
                ok = false;
                $("#RBCustomer").parent().css("color", "#f00");
                $("#CmbCustomerErrorRequired").show();
            }
        }
    }

    if ($("#CmbPlanningResponsible").val() * 1 > 0) {
        if ($("#TxtAuditoryPlanningDate").val() === "") {
            ok = false;
            $("#TxtAuditoryPlanningDateLabel").css("color", "#f00");
            $("#TxtAuditoryPlanningDateErrorRequired").show();
        }
    }

    return ok;
}

function SaveAuditory() {
    if (AuditoryValidate() === false) { return false; }
    console.log("SaveAuditory");
    var customer = { "Id": -1 };
    var provider = { "Id": -1 };
    var enterpriseAddress = "";
    var companyAddress = -1;
    var previewDate = null;
    switch (Auditory.Type) {
        case AuditoryTypes.Interna:
            companyAddress = $("#CmbAddress").val();
            enterpriseAddress = $("#CmbAddress option:selected").text();
            break;
        case AuditoryTypes.Externa:
            companyAddress = $("#CmbAddress").val();
            enterpriseAddress = $("#CmbAddress option:selected").text();
            if (document.getElementById("RBProvider").checked===true)
            {
                provider = { "Id": $("#CmbProvider").val() * 1 };
            } else
            {
                customer = { "Id": $("#CmbCustomer").val() * 1 };
            }
            previewDate = GetDate($("#TxtPreviewDate").val(), "/", false);
            break;
        case AuditoryTypes.Proveedor:
            provider = { "Id": $("#CmbProvider").val() * 1 };
            companyAddress = 1;
            enterpriseAddress = $("#TxtAddress").val();
            break;
    }

    var auditoryData = {
        "Id": Auditory.Id,
        "CompanyId": Company.Id,
        "Type": Auditory.Type,
        "Description": $("#TxtName").val(),
        "Descripcion": $("#TxtDescription").val(),
        "Scope": $("#TxtScope").val(),
        "Amount": StringToNumber($("#TxtAmount").val(), ".", ","),
        "Notes": $("#TxtNotes").val(),
        "CompanyAddressId": companyAddress,
        "EnterpriseAddress": enterpriseAddress,
        "AuditorTeam": Auditory.Type === 0 ? "" : $("#TxtAuditorTeam").val(),
        "PlannedBy": { "Id": $("#CmbPlanningResponsible").val() * 1 },
        "PlannedOn": GetDate($("#TxtAuditoryPlanningDate").val(), "/", false),
        "InternalResponsible": { "Id": $("#CmbInternalResponsible").val() * 1 },
        "Active": true,
        "ValidatedBy": { "Id": -1 },
        "ValidatedUserBy": { "Id": -1 },
        "ClosedBy": { "Id": -1 },
        "CreatedBy": { "Id": -1 },
        "ModifiedBy": { "Id": -1 },
        "Customer": customer,
        "Provider": provider,
        "PreviewDate": previewDate
    };

    var toPlanned = false;
    if ((typeof Auditory.PlannedOn === "undefined" || Auditory.PlannedOn === null || Auditory.PlannedOn === "") && $("#TxtAuditoryPlanningDate").val() !== "") {
        toPlanned = true;
    }

    if (Auditory.Type === AuditoryTypes.Externa && Auditory.Status === AuditoryStatus.EnCurso) {
        if ($("#TxtStartQuestionsOn").val() !== "") {
            auditoryData.ReportStart = GetDate($("#TxtStartQuestionsOn").val(), "/", false);
        }
    }

    var finishQuestions = false;
    if (toPlanned === true && Auditory.Type === AuditoryTypes.Externa) {
        finishQuestions = true;
        auditoryData.Status = AuditoryStatus.EnCurso;        
    }
    else if (Auditory.Status > AuditoryStatus.Planificando) {
        if ((typeof Auditory.ReportEnd === "undefined" || Auditory.ReportEnd === null || Auditory.ReportEnd === "") && $("#TxtCloseQuestionsOn").val() !== "") {
            auditoryData.ReportEnd = GetDate($("#TxtCloseQuestionsOn").val(), "/", true);
            auditoryData.Status = AuditoryStatus.Pendiente;
        }
        else {
            auditoryData.Status = Auditory.Status;
        }
    }

    CalculateRules();
    var data = {
        "auditory": auditoryData,
        "rules": $("#TxtRulesId").val(),
        "applicationUserId": ApplicationUser.Id,
        "toPlanned": toPlanned,
        "finishQuestions": finishQuestions
    };

    var webMethod = "/Async/AuditoryActions.asmx/Insert";
    if (Auditory.Id > 0) {
        var oldAuditory = Auditory;
        webMethod = "/Async/AuditoryActions.asmx/Update";
        if (typeof Auditory.ReportEnd !== "undefined" && Auditory.ReportEnd !== null && Auditory.ReportEnd !== "") {
            oldAuditory.ReportEnd = GetDate(Auditory.ReportEnd, "/", true);
        }
        if (typeof Auditory.PlannedOn !== "undefined" && Auditory.PlannedOn !== null && Auditory.PlannedOn !== "") {
            oldAuditory.PlannedOn = GetDate(Auditory.PlannedOn, "/", true);
        }
        if (typeof Auditory.PlannedOn !== "undefined" && Auditory.PlannedOn !== null && Auditory.PlannedOn !== "") {
            oldAuditory.ValidatedOn = GetDate(Auditory.ValidatedOn, "/", true);
        }
        data["oldAuditory"] = oldAuditory;
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
    console.log("CalculateRules");
    var res = "";

    if (Auditory.Status < AuditoryStatus.Planificada) {
        var rules = $("#CmbRules").val();
        if (rules !== null && rules !== "") {
            for (var x = 0; x < rules.length; x++) {
                res += rules[x] + "|";
            }
        }
    }
    else {
        res = Auditory.Rules;
    }

    $("#TxtRulesId").val(res);
}

function QuestionaryPlay(cuestionarioId, editable) {
    AppWindow = window.open("/QuestionaryPlay.aspx?a=" + Auditory.Id + "&c=" + cuestionarioId + "&e=" + (editable === true ? "1" : "0"));
}

function RenderFounds() {
    var res = "";
    var count = 0;
    for (var x = 0; x < Founds.length; x++) {
        var found = Founds[x];
        if (found.Active === true) {
            count++;
            res += "<tr id=\"" + found.Id + "\" style=\"border-left:none;\">";
            res += "<td>" + found.Text + "</td>";
            res += "<td style=\"width:200px;\">" + found.Requeriment + "</td>";
            res += "<td style=\"width:200px;\">" + found.Unconformity + "</td>";
            res += "<td style=\"width:80px;text-align:center\">";
            res += found.Action === true ? Dictionary.Common_Yes : Dictionary.Common_No;
            res += "</td>";
            res += "<td class=\"hidden-480\" style=\"width: 90px;white-space:nowrap;\">";
            if (Auditory.Status !== 5) {
                res += "  <span class=\"btn btn-xs btn-info\" onclick=\"ShowPopupFoundDialog(" + found.Id + "); \">";
                res += "    <i class=\"icon-edit bigger-120\"></i>";
                res += "  </span>";
                res += "  &nbsp;";
                res += "  <span class=\"btn btn-xs btn-danger\" onclick=\"ShowPopupFoundDeleteDialog(" + found.Id + "); \">";
                res += "    <i class=\"icon-trash bigger-120\"></i>";
                res += "  </span>";
            }

            res += "</td>";
            res += "</tr>";
        }
    }

    $("#SpanHallazgosTotal").html(count);
    if (count > 0) {
        $("#HallazgosDataTable").html(res);
        $("#ListDataDivHallazgos").show();
        $("#NoDataHallazgos").hide();
    }
    else {
        $("#HallazgosDataTable").html("");
        $("#ListDataDivHallazgos").hide();
        $("#NoDataHallazgos").show();
    }

    ReviseNoActions();
}

function RenderImprovements() {
    var res = "";
    var count = 0;
    for (var x = 0; x < Improvements.length; x++) {
        var improvement = Improvements[x];
        if (improvement.Active === true) {
            count++;
            res += "<tr id=\"" + improvement.Id + "\" style=\"border-left:none;\">";
            res += "<td>" + improvement.Text + "</td>";
            res += "<td style=\"width:80px;text-align:center\">";
            res += improvement.Action === true ? Dictionary.Common_Yes : Dictionary.Common_No;
            res += "</td>";
            res += "<td class=\"hidden-480\" style=\"width: 90px;white-space:nowrap;\">";
            if (Auditory.Status !== 5) {
                res += "  <span class=\"btn btn-xs btn-info\" onclick=\"ShowPopupImprovementDialog(" + improvement.Id + "); \">";
                res += "    <i class=\"icon-edit bigger-120\"></i>";
                res += "  </span>";
                res += "  &nbsp;";
                res += "  <span class=\"btn btn-xs btn-danger\" onclick=\"ShowPopupImprovementDeleteDialog(" + improvement.Id + "); \">";
                res += "    <i class=\"icon-trash bigger-120\"></i>";
                res += "  </span>";
            }

            res += "</td>";
            res += "</tr>";
        }
    }

    $("#SpanMejorasTotal").html(count);
    if (count > 0) {
        $("#MejorasDataTable").html(res);
        $("#ListDataDivMejoras").show();
        $("#NoDataMejoras").hide();
    }
    else {
        $("#MejorasDataTable").html("");
        $("#ListDataDivMejoras").hide();
        $("#NoDataMejoras").show();
    }

    ReviseNoActions();
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

// ----------------------- Report ------------------
function foundGetById(id) {
    id = id * 1;
    for (var x = 0; x < Founds.length; x++) {
        if (Founds[x].Id === id) {
            return Founds[x];
        }
    }

    return null;
}

function improvementGetById(id) {
    id = id * 1;
    for (var x = 0; x < Improvements.length; x++) {
        if (Improvements[x].Id === id) {
            return Improvements[x];
        }
    }

    return null;
}

function ShowPopupFoundDialog(id) {
    $("#TxtText").removeAttr("disabled");
    $("#TxtRequeriment").removeAttr("disabled");
    $("#TxtUnconformity").removeAttr("disabled");
    $("#TxtText").css("background", "transparent");
    $("#TxtRequeriment").css("background", "transparent");
    $("#TxtUnconformity").css("background", "transparent");
    $("#ChkActionFound").removeAttr("disabled");

    foundSelectedId = id;
    foundSelected = foundGetById(id);

    var title = Dictionary.Item_Auditory_Title_PopupUpdateFound;
    if (id < 0) {
        title = Dictionary.Item_Auditory_Title_PopupAddFound;
        $("#TxtText").val("");
        $("#TxtRequeriment").val("");
        $("#TxtUnconformity").val("");
        $("#ChkActionFound").attr("checked", false);
    }
    else {
        $("#TxtText").val(foundSelected.Text);
        $("#TxtRequeriment").val(foundSelected.Requeriment);
        $("#TxtUnconformity").val(foundSelected.Unconformity);
        $("#ChkActionFound").attr("checked", foundSelected.Action);
    }

    $("#PopupFoundDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": title,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnFoundSaveOK",
                    "html": "<i class=\"icon-save bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        FoundSave();
                    }
                },
                {
                    "Id": "BtnFoundSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ShowPopupImprovementDialog(id) {
    $("#TxtImprovement").removeAttr("disabled");
    $("#TxtImprovement").css("background", "transparent");
    $("#ChkActionImprovement").removeAttr("disabled");

    improvementSelectedId = id;
    improvementSelected = improvementGetById(id);

    var title = Dictionary.Item_Auditory_Title_PopupUpdateImprovement;
    if (id < 0) {
        title = Dictionary.Item_Auditory_Title_PopupAddImprovement;
        $("#TxtImprovement").val("");
        $("#ChkActionImprovement").attr("checked", false);
    }
    else {
        $("#TxtImprovement").val(improvementSelected.Text);
        $("#ChkActionImprovement").attr("checked", improvementSelected.Action);
    }

    var dialog = $("#PopupImprovementDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": title,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnImprovementSaveOk",
                    "html": "<i class=\"icon-save bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        ImprovementSave();
                    }
                },
                {
                    "Id": "BtnImprovementSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function FoundSave() {
    foundSelected = {
        "Id": foundSelectedId,
        "CompanyId": Company.Id,
        "AuditoryId": Auditory.Id,
        "CuestionarioId": foundSelected.CuestionarioId,
        "Text": $("#TxtText").val(),
        "Requeriment": $("#TxtRequeriment").val(),
        "Unconformity": $("#TxtUnconformity").val(),
        "Action": document.getElementById("ChkActionFound").checked === true,
        "Active": true
    };

    var data = {
        "found": foundSelected,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/FoundSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (foundSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Founds.length; x++) {
                    if (Founds[x].Id === foundSelectedId) {
                        temp.push(foundSelected);
                    }
                    else {
                        temp.push(Founds[x]);
                    }
                }

                Founds = temp;
            }
            else {
                foundSelected.Id = msg.d.MessageError * 1;
                Founds.push(foundSelected);
            }

            $("#PopupFoundDialog").dialog("close");
            RenderFounds();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ImprovementSave() {
    improvementSelected = {
        "Id": improvementSelectedId,
        "CompanyId": Company.Id,
        "AuditoryId": Auditory.Id,
        "CuestionarioId": improvementSelected.CuestionarioId,
        "Action": document.getElementById("ChkActionImprovement").checked === true,
        "Text": $("#TxtImprovement").val(),
        "Active": true
    };

    var data = {
        "improvement": improvementSelected,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/ImprovementSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (improvementSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Improvements.length; x++) {
                    if (Improvements[x].Id === improvementSelectedId) {
                        temp.push(improvementSelected);
                    }
                    else {
                        temp.push(Improvements[x]);
                    }
                }

                Improvements = temp;
            }
            else {
                improvementSelected.Id = msg.d.MessageError * 1;
                Improvements.push(improvementSelected);
            }

            $("#PopupImprovementDialog").dialog("close");
            RenderImprovements();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ShowPopupFoundDeleteDialog(id) {
    foundSelectedId = id * 1;
    foundSelected = foundGetById(foundSelectedId);
    $("#foundName").html(foundSelected.Text);
    $("#foundDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Auditory_Title_PopupDeleteFound,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnFoundDeleteOk",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        FoundDeleteConfirmed();
                    }
                },
                {
                    "Id": "BtnFoundDeleteCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });

}

function ShowPopupImprovementDeleteDialog(id) {
    improvementSelectedId = id * 1;
    improvementSelected = improvementGetById(improvementSelectedId);
    var dialog = $("#improvementDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Auditory_Title_PopupDeleteImprovement,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnImprovementDeleteOk",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        ImprovementDeleteConfirmed();
                    }
                },
                {
                    "Id": "BtnImprovementDeleteCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });

}

function FoundDeleteConfirmed() {
    var data = {
        "id": foundSelectedId,
        "companyId": Company.Id,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/FoundDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (foundSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Founds.length; x++) {
                    if (Founds[x].Id !== foundSelectedId) {
                        temp.push(Founds[x]);
                    }
                }

                Founds = temp;
            }

            $("#foundDeleteDialog").dialog("close");
            RenderFounds();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ImprovementDeleteConfirmed() {
    var data = {
        "id": improvementSelectedId,
        "companyId": Company.Id,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/ImprovementDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (improvementSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Improvements.length; x++) {
                    if (Improvements[x].Id !== improvementSelectedId) {
                        temp.push(Improvements[x]);
                    }
                }

                Improvements = temp;
            }

            $("#improvementDeleteDialog").dialog("close");
            RenderImprovements();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}
// -------------------------------------------------

function PopupValidarReset() {
    $("#CmbValidatedBy").removeAttr("disabled");
    $("#CmbValidatedBy").css("background", "transparent");
    $("#TxtValidatedOn").removeAttr("disabled");
    $("#TxtValidatedOn").css("background", "transparent");
    $("#CmbValidatedByLabel").css("color", "#000");
    $("#TxtValidatedOnLabel").css("color", "#000");
    $("#CmbValidatedByErrorRequired").hide();
    $("#TxtValidatedOnErrorRequired").hide();
    $("#TxtValidatedOnErrorDateMalformed").hide();
    $("#TxtValidatedOnErrorCross").hide();

    var noActionsMessage = "";
    var actions = 0;
    if (Founds.length > 0) {
        for (var x = 0; x < Founds.length; x++) {
            if (Founds[x].Action === true) {
                actions = 1;
                break;
            }
        }

        if (actions === 0) {
            noActionsMessage = Dictionary.Item_Auditory_FoundNoAction;
        }
    }

    actions = 0;
    if (Improvements.length > 0) {
        for (var i = 0; i < Improvements.length; i++) {
            if (Improvements[i].Action === true) {
                actions = 1;
                break;
            }
        }

        if (actions === 0) {
            if (noActionsMessage !== "") { noActionsMessage += "<br />"; }
            noActionsMessage = Dictionary.Item_Auditory_ImprovementNoAction;
        }
    }

    if (noActionsMessage !== "") {
        $("#NoActionsPopupMessage").html(noActionsMessage);
        $("#NoActionsPopup").removeClass("hide").dialog({
            "resizable": false,
            "modal": true,
            "title": Dictionary.Common_Warning,
            "title_html": true,
            "width": 400,
            "buttons":
                [
                    {
                        "Id": "BtnContinueValidationOK",
                        "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                        "class": "btn btn-success btn-xs",
                        "click": function () {
                            forceValidate = true;
                            $(this).dialog("close");
                            PopupValidarShow();
                        }
                    },
                    {
                        "Id": "BtnContinueValidationCancel",
                        "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                        "class": "btn btn-xs",
                        "click": function () {
                            $(this).dialog("close");
                        }
                    }
                ]
        });
        return false;
    }
    else {
        return true;
    }
}

function PopupClosedReset() {
    $("#CmbClosedBy").removeAttr("disabled");
    $("#CmbClosedBy").css("background", "transparent");
    $("#TxtClosedOn").removeAttr("disabled");
    $("#TxtClosedOn").css("background", "transparent");
    $("#CmbClosedByLabel").css("color", "#000");
    $("#TxtClosedOnLabel").css("color", "#000");
    $("#CmbClosedByErrorRequired").hide();
    $("#TxtClosedOnErrorRequired").hide();
    $("#TxtClosedOnErrorDateMalformed").hide();
    $("#TxtClosedOnErrorCross").hide();
}

function PopupReopenShow() {
    $("#ReopenPopup").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Auditory_Popup_ReopenTitle,
        "title_html": true,
        "width": 400,
        "buttons":
            [
                {
                    "Id": "BtnReopenSaveOK",
                    "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        ReopenConfirmed();
                    }
                },
                {
                    "Id": "BtnReopenSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

var forceValidate = false;
function PopupCloseShow() {
    $("#CmbClosedBy").css("background", "transparent");
    $("#CmbClosedBy").removeAttr("disabled");
    $("#TxtClosedOn").removeAttr("disabled");
    $("#ClosedPopup").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Auditory_Btn_Close,
        "title_html": true,
        "width": 400,
        "buttons":
            [
                {
                    "Id": "BtnCloseSaveOK",
                    "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Item_Auditory_Btn_Close,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        CloseConfirmed();
                    }
                },
                {
                    "Id": "BtnCloseSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ReopenConfirmed() {
    var data = {
        "auditoryId": Auditory.Id,
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/Reopen",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            $("#ReopenPopup").dialog("close");
            $("#DivCloseButton").show();
            $("#DivCloseResume").hide();
            $("#DivValidationButton").show();
            document.location = document.location + "";
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function CloseConfirmed() {
    PopupClosedReset();
    var ok = true;
    if ($("#CmbClosedBy").val() * 1 < 1) {
        ok = false;
        $("#CmbClosedByLabel").css("color", "#f00");
        $("#CmbClosedByErrorRequired").show();
    }

    if ($("#TxtClosedOn").val() === "") {
        ok = false;
        $("#TxtClosedOnLabel").css("color", "#f00");
        $("#TxtClosedOnErrorRequired").show();
    } else {
        if (validateDate($("#TxtClosedOn").val()) === false) {
            ok = false;
            $("#TxtClosedOnLabel").css("color", "#f00");
            $("#TxtClosedOnErrorDateMalformed").show();
        }
        else {
            var reportEnd = GetDate($("#TxtCloseQuestionsOn").val(), "/", false);
            var validationDate = GetDate($("#TxtClosedOn").val(), "/", false);
            if (validationDate < reportEnd) {
                ok = false;
                $("#TxtClosedOnLabel").css("color", "#f00");
                $("#TxtClosedOnErrorCross").show();
            }
        }
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "auditoryId": Auditory.Id,
        "questionaryStart": GetDate($("#TxtStartQuestionsOn").val(), "/", false),
        "questionaryEnd": GetDate($("#TxtCloseQuestionsOn").val(), "/", false),
        "closedBy": $("#CmbClosedBy").val() * 1,
        "closedOn": GetDate($("#TxtClosedOn").val(), "/", false),
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/Close",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            $("#ClosedPopup").dialog("close");
            $("#DivCloseButton").hide();
            $("#DivCloseResume").show();
            $("#DivValidationButton").hide();
            document.location = document.location + "";
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function PopupValidarShow() {
    if (forceValidate !== true) {
        if (PopupValidarReset() === false) {
            return false;
        }
    }

    forceValidate = false;

    $("#ValidationPopup").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Auditory_Btn_Validation,
        "title_html": true,
        "width": 400,
        "buttons":
            [
                {
                    "Id": "BtnFoundSaveOK",
                    "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Item_Auditory_Btn_PopupValidation,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        ValidationConfirmed();
                    }
                },
                {
                    "Id": "BtnFoundSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ValidationConfirmed() {
    var ok = true;
    if ($("#CmbValidatedBy").val() * 1 < 1) {
        ok = false;
        $("#CmbValidatedByLabel").css("color", "#f00");
        $("#CmbValidatedByErrorRequired").show();
    }

    if ($("#TxtValidatedOn").val() === "") {
        ok = false;
        $("#TxtValidatedOnLabel").css("color", "#f00");
        $("#TxtValidatedOnErrorRequired").show();
    } else {
        if (validateDate($("#TxtValidatedOn").val()) === false) {
            ok = false;
            $("#TxtValidatedOnLabel").css("color", "#f00");
            $("#TxtValidatedOnErrorDateMalformed").show();
        }
        else {
            var reportEnd = GetDate(Auditory.ReportEnd, "/", false);
            var validationDate = GetDate($("#TxtValidatedOn").val(), "/", false);
            if (validationDate < reportEnd) {
                $("#TxtValidatedOnLabel").css("color", "#f00");
                $("#TxtValidatedOnErrorCross").show();
            }
        }
    }

    if (ok === false) { return false; }

    var data = {
        "auditoryId": Auditory.Id,
        "validatedBy": $("#CmbValidatedBy").val() * 1,
        "validatedOn": GetDate($("#TxtValidatedOn").val(), "/", false),
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/Validate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = document.location + "";
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

var zombieSelectedId = null;
var zombieSelected = null;
function IncidentActionShowPopup(id) {
    ZombieResetValidationForm();
    zombieSelectedId = id * 1;
    zombieSelected = ZombieById(zombieSelectedId);
    if (zombieSelected === null) {
        $("#TxtIncidentActionDescription").val("");
        $("#TxtWhatHappend").val("");
        $("#CmbWhatHappendBy").val(-1);
        $("#TxtWahtHappendOn").val("");
        document.getElementById("RBactionType1").checked = false;
        document.getElementById("RBactionType2").checked = false;
    }
    else {
        $("#TxtIncidentActionDescription").val(zombieSelected.Description);
        $("#TxtWhatHappend").val(zombieSelected.WhatHappend);
        $("#CmbWhatHappendBy").val(zombieSelected.WhatHappendBy.Id);
        $("#TxtWahtHappendOn").val(zombieSelected.WhatHappendOn);
        document.getElementById("RBactionType1").checked = zombieSelected.ActionType === 1;
        document.getElementById("RBactionType2").checked = zombieSelected.ActionType === 2;
    }

    $("#TxtIncidentActionDescription").removeAttr("disabled");
    $("#TxtWhatHappend").removeAttr("disabled");
    $("#CmbWhatHappendBy").removeAttr("disabled");
    $("#TxtWahtHappendOn").removeAttr("disabled");
    $("#TxtWhatHappend").css("background", "transparent");
    $("#CmbWhatHappendBy").css("background", "transparent");
    $("#RBactionType1").removeAttr("disabled");
    $("#RBactionType2").removeAttr("disabled");

    $("#IncidentActionPopup").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_IncidentAction,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnIncidentActionSaveOK",
                    "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        ZombieSaveConfirmed();
                    }
                },
                {
                    "Id": "BtnIncidentActionSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ZombieResetValidationForm() {
    $("#TxtIncidentActionTypeLabel").css("color", "#333");
    $("#TxtIncidentActionDescriptionLabel").css("color", "#333");
    $("#TxtWhatHappendLabel").css("color", "#333");
    $("#CmbWhatHappendByLabel").css("color", "#333");
    $("#TxtWahtHappendOnLabel").css("color", "#333");
    $("#TxtIncidentActionTypeErrorRequired").hide();
    $("#TxtIncidentActionDescriptionErrorRequired").hide();
    $("#TxtWhatHappendErrorRequired").hide();
    $("#CmbWhatHappendByErrorRequired").hide();
    $("#TxtWahtHappendOnErrorRequired").hide();
    $("#TxtWahtHappendOnErrorDateMalformed").hide();
    $("#TxtWahtHappendOnErrorCross").hide();
}

function ZombieValidateForm() {
    ZombieResetValidationForm();
    var ok = true;

    if (document.getElementById("RBactionType1").checked === false && document.getElementById("RBactionType2").checked === false) {
        ok = false;
        $("#TxtIncidentActionTypeLabel").css("color", "#f00");
        $("#TxtIncidentActionTypeErrorRequired").show();
    }

    if ($("#TxtIncidentActionDescription").val() === "") {
        ok = false;
        $("TxtIncidentActionDescriptionLabel").css("color", "#f00");
        $("#TxtIncidentActionDescriptionErrorRequired").show();
    }

    if ($("#TxtWhatHappend").val() === "") {
        ok = false;
        $("#TxtWhatHappendLabel").css("color", "#f00");
        $("#CmbWhatHappendByErrorRequired").show();
    }

    if ($("#CmbWhatHappendBy").val() * 1 < 1) {
        ok = false;
        $("#CmbWhatHappendByLabel").css("color", "#f00");
        $("#TxtWhatHappendErrorRequired").show();
    }

    if ($("#TxtWahtHappendOn").val() === "") {
        ok = false;
        $("#TxtWahtHappendOnLabel").css("color", "#f00");
        $("#TxtWahtHappendOnErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtWahtHappendOn").val()) === false) {
            ok = false;
            $("#TxtWahtHappendOnLabel").css("color", "#f00");
            $("#TxtWahtHappendOnErrorDateMalformed").show();
        }
        else {
            // Comprobar que la fecha no es anterior a la fecha más antigua de las planificaciones
            var limitDate = GetDate(AuditoryPlanning[0].Date, "/");
            for (var p = 1; p < AuditoryPlanning.length; p++) {
                var candidate = GetDate(AuditoryPlanning[p].Date, "/");
                if (candidate < limitDate) {
                    limitDate = candidate;
                }
            }

            var actionDate = GetDate($("#TxtWahtHappendOn").val(), "/");
            if (actionDate < limitDate) {
                ok = false;
                $("#TxtWahtHappendOnLabel").css("color", "#f00");
                $("#TxtWahtHappendOnErrorCross").show();
            }
        }
    }

    return ok;
}

function ZombieSaveConfirmed() {
    if (ZombieValidateForm() === false) { return false; }

    zombieSelected = {
        "Id": zombieSelectedId,
        "ActionType": document.getElementById("RBactionType1").checked === true ? 1 : 2,
        "Description": $("#TxtIncidentActionDescription").val(),
        "WhatHappend": $("#TxtWhatHappend").val(),
        "WhatHappendBy": { "Id": $("#CmbWhatHappendBy").val() * 1 },
        "WhatHappendOn": GetDate($("#TxtWahtHappendOn").val(), "/", true),
        "AuditoryId": Auditory.Id,
        "CompanyId": Auditory.CompanyId
    };

    var data = {
        "zombie": zombieSelected
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/SaveZombie",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (zombieSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Zombies.length; x++) {
                    if (Zombies[x].Id === zombieSelectedId) {
                        zombieSelected.WhatHappendOn = FormatDate(zombieSelected.WhatHappendOn, "/");
                        temp.push(zombieSelected);
                    }
                    else {
                        temp.push(Zombies[x]);
                    }
                }

                Zombies = temp;
            }
            else {
                zombieSelected.Id = msg.d.MessageError * 1;
                zombieSelected.WhatHappendOn = FormatDate(zombieSelected.WhatHappendOn, "/");
                Zombies.push(zombieSelected);
            }

            $("#IncidentActionPopup").dialog("close");
            RenderZombies();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ZombieById(id) {
    for (var x = 0; x < Zombies.length; x++) {
        if (Zombies[x].Id === id * 1) {
            return Zombies[x];
        }
    }
    return null;
}

function RenderZombies() {
    var res = "";

    if (Zombies.length > 0) {
        for (var x = 0; x < Zombies.length; x++) {
            res += "<tr>";
            res += "<td style=\"width:120px;\">" + (Zombies[x].ActionType === 1 ? Dictionary.Item_IncidentAction_Type1 : Dictionary.Item_IncidentAction_Type2) + "</td>";
            res += "<td>" + Zombies[x].Description + "</td>";
            res += "<td style=\"width:120px;\">" + Zombies[x].WhatHappendOn + "</td>";
            res += "<td style=\"width:90px;\">";
            res += "  <span class=\"btn btn-xs btn-info\" id=\"" + Zombies[x].Id + "\" onclick=\"IncidentActionShowPopup(this.id)\">";
            res += "    <i class=\"icon-edit bigger-120\"></i></span>";
            res += "  <span class=\"btn btn-xs btn-danger\" id=\"" + Zombies[x].Id + "\">";
            res += "    <i class=\"icon-trash bigger-120\"></i></span>";
            res += "</td>";
            res += "</tr>";
        }

        $("#IncidentActionsDataTable").html(res);
        $("#ListDataDivIncidentActions").show();
        $("#NoDataIncidentActions").hide();
        $("#SpanIncidentActionsTotal").html(Zombies.length);
    }
    else {

        $("#IncidentActionsDataTable").html("");
        $("#ListDataDivIncidentActions").hide();
        $("#NoDataIncidentActions").show();
        $("#SpanIncidentActionsTotal").html("0");
    }
}

function CmbPlanningResponsibleChanged() {
    var id = $("#CmbPlanningResponsible").val() * 1;
    if (id > 0) {
        $("#TxtAuditoryPlanningDateLabel").html(Dictionary.Item_Auditory_Label_PlanningDate + "<span style=\"color:#f00;\">*</span>");
    }
    else {
        $("#TxtAuditoryPlanningDateLabel").html(Dictionary.Item_Auditory_Label_PlanningDate);
    }
}

function ChkSendMailChanged() {
    if (Auditory.Type === AuditoryTypes.Proveedor) {
        if (document.getElementById("ChkSendMail").checked === true) {
            $("#TxtProviderEmailRow").show();
        }
        else {
            $("#TxtProviderEmailRow").hide();
        }
    }
}

function CalculeTotalQuestions() {
    if (Auditory.Type === AuditoryTypes.Externa) { return false; }
    if (Auditory.Status === AuditoryStatus.Planificando) {
        var total = 0;
        var rules = $("#CmbRules").val();

        if (rules !== null && rules.length > 0) {
            var process = [];
            for (var p = 0; p < AuditoryPlanning.length; p++) {
                process.push(AuditoryPlanning[p].Process.Id);
            }

            if (process.length > 0) {
                for (var q = 0; q < TotalQuestions.length; q++) {
                    if ($.inArray(TotalQuestions[q].N.toString(), rules) > -1 && $.inArray(TotalQuestions[q].P, process) > -1) {
                        total += TotalQuestions[q].T;
                    }
                }
            }

        }

        if (Auditory.Id > 0) {
            if (total > 0) {
                $("#DivNoQuestions").hide();
                $("#CmbPlanningResponsible").removeAttr("disabled");
                $("#TxtAuditoryPlanningDate").removeAttr("disabled");
            }
            else {
                $("#DivNoQuestions").show();
                $("#CmbPlanningResponsible").attr("disabled", "disabled");
                $("#TxtAuditoryPlanningDate").attr("disabled", "disabled");
            }
        }
    }
}

function RenderCuestionarios() {
    var res = "";
    var cuestionariosCerrables = true;
    for (var x = 0; x < Cuestionarios.length; x++) {
        var cuestionario = Cuestionarios[x];
        var percent = (cuestionario.C / cuestionario.T) * 100;
        var warning = "";
        if (cuestionario.Co < cuestionario.C && cuestionario.F === 0) {
            warning = "&nbsp;<i class=\"fa fa-warning\" style=\"color:#f77;\" title=\"" + Dictionary.Item_Auditory_Message_NoCompliantNoFound + "\"></i>";
            cuestionariosCerrables = false;
        }

        res += "<tr id=\"Cuestionario_" + cuestionario.Id + "\">";
        res += "  <td style=\"height: 30px;width:120px;text-align:center;";
        res += "             background: -webkit-linear-gradient(left, #cfc " + percent + "%, #fcc " + percent + "%);";
        res += "             background: -moz-linear-gradient(left, #cfc " + percent + "%, #fcc " + percent + "%);";
        res += "             background: -ms-linear-gradient(left, #cfc " + percent + "%, #fcc " + percent + "%);";
        res += "             background: linear-gradient(left, #cfc " + percent + "%, #fcc " + percent + "%);\">";
        res += "     <strong>" + cuestionario.C + " / " + cuestionario.T + "</strong></td>";
        res += "  <td>" + cuestionario.Description + warning + "</td>";
        res += " <td style=\"width:50px;text-align:center;\">";
        if (Auditory.Status === AuditoryStatus.EnCurso || Auditory.Status === AuditoryStatus.Planificada) {
            res += "      <span class=\"btn btn-xs btn-success\" id=\"" + cuestionario.Id + "\" title=\"Continuar cuestionario\" onclick=\"QuestionaryPlay(" + cuestionario.Id + ", true);\">";
            res += "      <i class=\"icon-play bigger-120\"></i>";
            res += "    </span>";
        }
        else {
            res += "      <span class=\"btn btn-xs btn-success\" id=\"" + cuestionario.Id + "\" title=\"Ver cuestionario\" onclick=\"QuestionaryPlay(" + cuestionario.Id + ", false);\">";
            res += "      <i class=\"icon-eye-open bigger-120\"></i>";
            res += "    </span>";
        }
        res += "  </td>";
        res += "</tr>";
    }

    if (cuestionariosCerrables === true) {
        $("#BtnCloseCuestionarios").show();
    } else {
        $("#BtnCloseCuestionarios").hide();
    }

    $("#SpanCuestionarioTotal").html(Cuestionarios.length);
    $("#CuestionarioDataTable").html(res);
}

function GetReportData() {
    var data = {
        "id": Auditory.Id,
        "companyId": Company.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/ReportData",
        "data": JSON.stringify(data, null, 2),
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "success": function (msg) {
            var result = null;
            eval("result = " + msg.d.ReturnValue + ";");
            console.log(result);

            Cuestionarios = result.Cuestionarios;
            Founds = result.Founds;
            Improvements = result.Improvements;

            RenderCuestionarios();
            RenderFounds();
            RenderImprovements();
        }
    });
}

// ------ Close cuestionarios -----------------------
function CloseCuestionariosPopup() {
    $("#TxtCloseCuestionario").removeAttr("disabled");
    $("#TxtCloseCuestionarioBtn").removeAttr("disabled");
    $("#CloseCuestionarioPopup").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Auditory_Popup_CloseQuestionary,
        "title_html": true,
        "width": 400,
        "buttons":
            [
                {
                    "Id": "BtnReopenCuestionarioOK",
                    "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        CloseCuestionariosConfirmed();
                    }
                },
                {
                    "Id": "BtnReopenCuestionarioCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function CloseCuestionariosConfirmed() {
    var ok = true;
    $("#TxtCloseCuestionarioLabel").css("color", "#333");
    $("#TxtTxtCloseCuestionarioErrorRequired").hide();
    $("#TxtTxtCloseCuestionarioErrorDateMalformed").hide();
    $("#TxtTxtCloseCuestionarioErrorCross").hide();

    if ($("#TxtCloseCuestionario").val() === "") {
        ok = false;
        $("#TxtCloseCuestionarioLabel").css("color", "#f00");
        $("#TxtTxtCloseCuestionarioErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtCloseCuestionario").val()) === "false") {
            ok = false;
            $("#TxtCloseCuestionarioLabel").css("color", "#f00");
            $("#TxtTxtCloseCuestionarioErrorDateMalformed").show();
        }
        else {
            var dateStart = GetDate($("#TxtStartQuestionsOn").val(), "/");
            var dateEnd = GetDate($("#TxtCloseCuestionario").val(), "/");
            if (dateEnd < dateStart) {
                ok = false;
                $("#TxtCloseCuestionarioLabel").css("color", "#f00");
                $("#TxtTxtCloseCuestionarioErrorCross").show();
            }
        }
    }

    if (ok === false) { return; }

    var data = {
        "questionaryStart": GetDate($("#TxtStartQuestionsOn").val(), "/"),
        "questionaryEnd": GetDate($("#TxtCloseCuestionario").val(), "/"),
        "auditoryId": Auditory.Id,
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/CloseCuestionarios",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            document.location = document.location + "";
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}
// ---------------------------------------------------

// ------ Reopen cuestionarios -----------------------
function ReopenCuestionariosPopup() {
    $("#CustionariosReoenDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Auditory_Popup_ReopenQuestionary,
        "title_html": true,
        "width": 400,
        "buttons":
            [
                {
                    "Id": "BtnReopenCuestionarioOK",
                    "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        ReopenCuestionariosConfirmed();
                    }
                },
                {
                    "Id": "BtnReopenCuestionarioCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ReopenCuestionariosConfirmed() {
    var data = {
        "auditoryId": Auditory.Id,
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/ReopenCuestionarios",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            document.location = document.location + "";
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}
// ---------------------------------------------------

function RenderRealActions() {
    console.log("RenderRealActions");
    var items = new Array();
    var target = document.getElementById("IncidentActionsDataTableReal");
    $("#IncidentActionsDataTableReal").html("");
    list = RealActions;

    if (list.length === 0) {
        $("#ItemTableVoid").show();
        $("#NumberCosts").html("0");
        target.style.display = "none";
        return false;
    }

    var total = 0;

    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        var row = document.createElement("TR");
        var tdNumber = document.createElement("TD");
        var tdOpen = document.createElement("TD");
        var tdType = document.createElement("TD");
        var tdStatus = document.createElement("TD");
        var tdDescription = document.createElement("TD");
        var tdAmount = document.createElement("TD");

        total += list[x].Amount;

        var status = 1;
        var colorStatus = "#f00";
        if (item.ClosedOn !== null) { status = 4; }
        else if (item.ActionsOn !== null) { status = 3; }
        else if (item.CausesOn !== null) { status = 2; }

        var type = "";
        if (item.ActionType === 1) { type = Dictionary.Item_IncidentAction_Type1; }
        if (item.ActionType === 2) { type = Dictionary.Item_IncidentAction_Type2; }
        if (item.ActionType === 3) { type = Dictionary.Item_IncidentAction_Type3; }

        row.id = item.Id;

        var iconStatus = document.createElement("I");
        if (status === 1) {
            colorStatus = "#f00";
            iconStatus.className = "fa icon-pie-chart";
            iconStatus.title = Dictionary.Item_IndicentAction_Status1;
        }
        if (status === 2) {
            colorStatus = "#dd0";
            iconStatus.className = "fa icon-pie-chart";
            iconStatus.title = Dictionary.Item_IndicentAction_Status2;
        }
        if (status === 3) {
            colorStatus = "#070";
            iconStatus.className = "fa icon-play";
            iconStatus.title = Dictionary.Item_IndicentAction_Status3;
        }
        if (status === 4) {
            colorStatus = "#000";
            iconStatus.className = "fa icon-lock";
            iconStatus.title = Dictionary.Item_IndicentAction_Status4;
        }

        iconStatus.style.color = colorStatus;
        tdNumber.appendChild(iconStatus);

        tdOpen.appendChild(document.createTextNode(FormatYYYYMMDD(item.WhatHappenedOn, "/")));
        tdType.appendChild(document.createTextNode(type));
        tdStatus.appendChild(iconStatus);

        var actionLinkDescription = document.createElement("A");
        actionLinkDescription.appendChild(document.createTextNode(item.Description));
        actionLinkDescription.href = "ActionView.aspx?id=" + item.Id;
        tdDescription.appendChild(actionLinkDescription);

        tdAmount.appendChild(document.createTextNode(ToMoneyFormat(item.Amount, 2)));

        tdType.style.width = "120px";
        tdOpen.style.width = "100px";
        tdOpen.align = "center";
        tdStatus.style.width = "65px";
        tdStatus.align = "center";
        tdAmount.align = "right";

        row.appendChild(tdStatus);
        row.appendChild(tdType);
        row.appendChild(tdDescription);
        row.appendChild(tdOpen);

        var iconEdit = document.createElement("SPAN");
        iconEdit.className = "btn btn-xs btn-info";
        iconEdit.id = item.Number;
        var innerEdit = document.createElement("I");
        innerEdit.className = "icon-eye-open bigger-120";
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { document.location = "ActionView.aspx?id=" + this.parentNode.parentNode.id; };

        var tdActions = document.createElement("TD");
        tdActions.style.width = "50px";

        tdActions.appendChild(iconEdit);

        row.appendChild(tdActions);
        target.appendChild(row);

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }
    }

    if (list.length === 0) {
        $("#NoDataIncidentActionsReal").show();
        $("#ListDataDivIncidentActionsReal").hide();
        $("#SpanIncidentActionsTotalReal").html("0");
    }
    else {
        $("#NoDataIncidentActionsReal").hide();
        $("#ListDataDivIncidentActionsReal").show();
        $("#SpanIncidentActionsTotalReal").html(RealActions.length);
    }
}

function ReviseNoActions() {
    $("#NoActionF").hide();
    $("#NoActionI").hide();
    $("#DivNoActions").hide();

    // Si no hay hallazgos o mejoras, no hay hay aviso de falta de acciones
    var AF = Founds.length === 0;
    var AI = Improvements.length === 0;

    for (var x = 0; x < Founds.length; x++) {
        if (Founds[x].Action === true) {
            AF = true;
            break;
        }
    }

    for (var y = 0; y < Improvements.length; y++) {
        if (Improvements[y].Action === true) {
            AI = true;
            break;
        }
    }

    if (AF === false) { $("#NoActionF").show(); $("#DivNoActions").show(); }
    if (AI === false) { $("#NoActionI").show(); $("#DivNoActions").show(); }
}