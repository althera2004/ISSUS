var forceValidate = false;
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

function Reload() {
    document.location = "AuditoryExternaView.aspx?id=" + Auditory.Id + "&t=2";
}

window.onload = function () {
    $("#nav-search").hide();
    $("#BtnCancel").on("click", function () { document.location = "/AuditoryList.aspx"; });
    if (Auditory.Id > 0) {
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
        //$("td .btn-info").remove();
        $("td .btn-danger").remove();
        $("#TxtNotes").removeAttr("disabled");
        $("#TxtNotes").css("backgroundColor", "transparent");
    } else {
        $("#CmbRules").chosen();
        if (Auditory.Id > 0) {
            $("#DivNewAuditory").hide();
        }
        else {
            $("#DivNewAuditory").show();
            $("#DivYesPlanning").hide();
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
    $("#BtnCloseAuditoria").on("click", PopupValidarShow);
    $("#BtnReopenAuditoria").on("click", PopupReopenShow);

    if (Auditory.Status > AuditoryStatus.Pendiente) {
        $("#TxtCloseQuestionsOn").attr("disabled", "disabled");
        $("#TxtCloseQuestionsOn").css("background", "#ccc");
        $("#CuestionarioDataTable .btn-success").hide();
    }
    else if (Auditory.Status === AuditoryStatus.Pendiente) {
        $("#TxtStartQuestionsOn").removeAttr("disabled");
        $("#TxtStartQuestionsOn").css("background", "transparent");
        $("#TxtCloseQuestionsOn").removeAttr("disabled");
        $("#TxtCloseQuestionsOn").css("background", "transparent");
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
        $("#DivAcionsZombies").show();
    }
    else {
        $("#scrollTableDivIncidentActions").remove();
        $("#BtnActionAdd").remove();
        $("#ListDataDivIncidentActionsReal").show();
    }

    if (Auditory.Status > AuditoryStatus.Planificando) {
        $("#CmbAddress").parent().parent().hide();
    }

    if (Auditory.Status === AuditoryStatus.Pendiente) {
        $("#scrollTableDivIncidentActionsReal").hide();
        $("#BtnCloseAuditoria").show();
    } else {
        $("#BtnCloseAuditoria").hide();
    }

    $("#TxtCloseQuestionsOn").on("change", TxtCloseQuestionsOnChanged);
};

function TxtCloseQuestionsOnChanged() {
    console.log("TxtReportEndBtnChanged");
    if ($("#TxtReportEndBtn").val() === "") {
        $("#DivCloseButton").hide();
    }
    else {
        $("#DivCloseButton").show();
    }
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

    if ($("#CmbAddress").val() * 1 < 0) {
        ok = false;
        $("#CmbAddressLabel").css("color", "#f00");
        $("#CmbAddressErrorRequired").show();
    }

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
    companyAddress = $("#CmbAddress").val();
    enterpriseAddress = $("#CmbAddress option:selected").text();
    if (document.getElementById("RBProvider").checked === true) {
        provider = { "Id": $("#CmbProvider").val() * 1 };
    } else {
        customer = { "Id": $("#CmbCustomer").val() * 1 };
    }

    previewDate = GetDate($("#TxtPreviewDate").val(), "/", false);

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
        "PlannedBy": { "Id": $("#CmbInternalResponsible").val() * 1 },
        "PlannedOn": previewDate,
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

    if ($("#TxtStartQuestionsOn").val() !== "") {
        auditoryData.ReportStart = GetDate($("#TxtStartQuestionsOn").val(), "/", false);
    }

    var finishQuestions = false;
    if (toPlanned === true) {
        finishQuestions = true;
        auditoryData.Status = AuditoryStatus.Pendiente;
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
                document.location = "/AuditoryExternaView.aspx?id=" + msg.d.MessageError;
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

    if ($("#TxtStartQuestionsOn").val() === "" || $("#TxtCloseQuestionsOn").val() === "") {
        alertUI(Dictionary.Item_Auditory_ErrorMessage_QuestionairesClosed);
        return false;
    }

    if (Zombies.length === 0) {
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
        "questionaryStart": GetDate($("#TxtStartQuestionsOn").val(), "/", false),
        "questionaryEnd": GetDate($("#TxtCloseQuestionsOn").val(), "/", false),
        "validatedBy": $("#CmbValidatedBy").val() * 1,
        "validatedOn": GetDate($("#TxtValidatedOn").val(), "/", false),
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/ValidateExternal",
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
            // Comprobar que la fecha no es anterior a la fecha prevista de auditoria
            var plannedOn = GetDate($("#TxtPreviewDate").val(), "/");
            var actionDate = GetDate($("#TxtWahtHappendOn").val(), "/");
            if (actionDate < plannedOn) {
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

function RenderRealActions() {

}