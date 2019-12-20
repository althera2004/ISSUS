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

    // Las auditorías planificadas no pueden añadir normas
    if (Auditory.PlannedOn !== null && Auditory.PlannedOn !== "") {
        $("#AuditoryRulesDiv input").attr("disabled", "disabled");
        $("input").attr("disabled", "disabled");
        $("textarea").attr("disabled", "disabled");
        $("textarea").css("backgroundColor", "#eee");
        $("select").attr("disabled", "disabled");
        $("select").css("backgroundColor", "#eee");
        $("#BtnNewItem").remove();
        $("#BtnProviderBAR").remove();
        //$("td .btn-info").remove();
        $("td .btn-danger").remove();
        $("#TxtNotes").removeAttr("disabled");
        $("#TxtNotes").css("backgroundColor", "transparent");
        $("#TxtAmount").removeAttr("disabled");
        $("#TxtAmount").css("backgroundColor", "transparent");
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
                $("#CmbCustomerBar").show();
                $("#RBCustomer").attr("checked", "checked");
            }

            if (Auditory.Provider.Id > 0) {
                $("#ProviderDiv").show();
                $("#CmbProviderBar").show();
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
        $("#TxtReportStartBtn").hide();
        $("#TxtReportEndBtn").hide();
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


    // BAR Popups
    $("#BtnProviderBAR").on("click", ShowProviderBarPopup);
    $("#BtnCustomerBAR").on("click", ShowCustomerBarPopup);
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
    $("#CmbProviderBar").hide();
    $("#CmbCustomerBar").hide();
    $("#ErrorProviderCustomerDiv").hide();
    $("#ProviderCustomerErrorRequired").hide();
    $("#RBProvider").parent().css("color", "#333");
    $("#RBCustomer").parent().css("color", "#333");
    if (document.getElementById("RBProvider").checked === true) { $("#ProviderDiv").show(); $("#CmbProviderBar").show(); }
    if (document.getElementById("RBCustomer").checked === true) { $("#CustomerDiv").show(); $("#CmbCustomerBar").show(); }
}

function AuditoryValidate() {
    var ok = true;
    if ($("#TxtName").val() === "") {
        ok = false;
        $("#TxtNameLabel").css("color", Color.Error);
        $("#TxtNameErrorRequired").show();
    }

    if ($("#TxtAmount").val() === "") {
        ok = false;
        $("#TxtAmountLabel").css("color", Color.Error);
        $("#TextAmountErrorRequired").show();
    }

    if ($("#TxtDescription").val() === "") {
        ok = false;
        $("#TxtDescriptionLabel").css("color", Color.Error);
        $("#TextDescriptionErrorRequired").show();
    }

    if ($("#TxtScope").val() === "") {
        ok = false;
        $("#TxtScopeLabel").css("color", Color.Error);
        $("#TxtScopeErrorRequired").show();
    }

    CalculateRules();
    if ($("#TxtRulesId").val() === "") {
        ok = false;
        $("#TxtRulesIdLabel").css("color", Color.Error);
        $("#TxtRulesIdErrorRequired").show();
    }

    if ($("#CmbInternalResponsible").val() * 1 < 0) {
        ok = false;
        $("#CmbInternalResponsibleLabel").css("color", Color.Error);
        $("#CmbInternalResponsibleErrorRequired").show();
    }

    if ($("#CmbAddress").val() * 1 < 0) {
        ok = false;
        $("#CmbAddressLabel").css("color", Color.Error);
        $("#CmbAddressErrorRequired").show();
    }

        if ($("#TxtAuditorTeam").val() === "") {
            ok = false;
            $("#TxtAuditorTeamLabel").css("color", Color.Error);
            $("#TxtAuditorTeamErrorRequired").show();
        }

        if ($("#TxtPreviewDate").val() === "") {
            ok = false;
            $("#TxtPreviewDateLabel").css("color", Color.Error);
            $("#TxtPreviewDateErrorRequired").show();
        }
        else {
            if (validateDate($("#TxtPreviewDate").val()) === false) {
                ok = false;
                $("#TxtPreviewDateLabel").css("color", Color.Error);
                $("#TxtPlannedDateErrorMailMalformed").show();
            }
        }

    if (document.getElementById("RBProvider").checked === false && document.getElementById("RBCustomer").checked === false) {
        ok = false;
        $("#ErrorProviderCustomerDiv").show();
        $("#ProviderCustomerErrorRequired").show();
        $("#RBProvider").parent().css("color", Color.Error);
        $("#RBCustomer").parent().css("color", Color.Error);
    }
    else if (document.getElementById("RBProvider").checked === true) {
        if ($("#CmbProvider").val() * 1 < 1) {
            ok = false;
            $("#RBProvider").parent().css("color", Color.Error);
            $("#CmbProviderErrorRequired").show();
        }
    }
    else {
        if ($("#CmbCustomer").val() * 1 < 1) {
            ok = false;
            $("#RBCustomer").parent().css("color", Color.Error);
            $("#CmbCustomerErrorRequired").show();
        }
    }

    if ($("#CmbPlanningResponsible").val() * 1 > 0) {
        if ($("#TxtAuditoryPlanningDate").val() === "") {
            ok = false;
            $("#TxtAuditoryPlanningDateLabel").css("color", Color.Error);
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
        $("#CmbClosedByLabel").css("color", Color.Error);
        $("#CmbClosedByErrorRequired").show();
    }

    if ($("#TxtClosedOn").val() === "") {
        ok = false;
        $("#TxtClosedOnLabel").css("color", Color.Error);
        $("#TxtClosedOnErrorRequired").show();
    } else {
        if (validateDate($("#TxtClosedOn").val()) === false) {
            ok = false;
            $("#TxtClosedOnLabel").css("color", Color.Error);
            $("#TxtClosedOnErrorDateMalformed").show();
        }
        else {
            var reportEnd = GetDate($("#TxtCloseQuestionsOn").val(), "/", false);
            var validationDate = GetDate($("#TxtClosedOn").val(), "/", false);
            if (validationDate < reportEnd) {
                ok = false;
                $("#TxtClosedOnLabel").css("color", Color.Error);
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
    $("#CmbValidatedByLabel").css("color", Color.Label);
    $("#TxtValidatedOnLabel").css("color", Color.Label);
    $("#CmbValidatedByErrorRequired").hide();
    $("#TxtValidatedOnErrorRequired").hide();
    $("#TxtValidatedOnErrorDateMalformed").hide();
    $("#TxtValidatedOnErrorCross").hide();

    if ($("#TxtStartQuestionsOn").val() === "" || $("#TxtCloseQuestionsOn").val() === "") {
        alertUI(Dictionary.Item_Auditory_ErrorMessage_QuestionairesClosed);
        return false;
    }

    // Alex: Primero se comprueban que las fechas sean correctas
    var fechaInicio = GetDate($("#TxtStartQuestionsOn").val(), "/", false);
    var fechaFinal = GetDate($("#TxtCloseQuestionsOn").val(), "/", false);

    if (fechaInicio > fechaFinal) {
        alertUI(Dictionary.Item_Auditory_ErrorMessage_QuestionairesCrossDate);
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
        // Alex: Si hay acciones zombies buscamos la fecha más antigua para comparar con la fecha final
        for (var z = 0; z < Zombies.length; z++) {

            // alex: cogemos la fecha de la accióm zombie
            var fechaZombie = GetDate(Zombies[z].WhatHappendOn, "/", false);

            // alex: sólo que haya una anterior a la final se indica el error
            if (fechaZombie < fechaFinal) {
                alertUI(Dictionary.Item_Auditory_ErrorMessage_ZobieDatesCross);
                return false;
            }
        }

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
                    "Id": "BtnValidateOK",
                    "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Item_Auditory_Btn_PopupValidation,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        ValidationConfirmed();
                    }
                },
                {
                    "Id": "BtnValidateCancel",
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
    // alex: si hubieran errores de una validación anterior se ocultan;
    $("#CmbValidatedByLabel").css("color", Color.Label);
    $("#CmbValidatedByErrorRequired").hide();
    $("#TxtValidatedOnLabel").css("color", Color.Label);
    $("#TxtValidatedOnErrorRequired").hide();
    $("#TxtValidatedOnErrorDateMalformed").hide();        
    $("#TxtValidatedOnErrorCross").hide();


    var ok = true;
    if ($("#CmbValidatedBy").val() * 1 < 1) {
        ok = false;
        $("#CmbValidatedByLabel").css("color", Color.Error);
        $("#CmbValidatedByErrorRequired").show();
    }

    if ($("#TxtValidatedOn").val() === "") {
        ok = false;
        $("#TxtValidatedOnLabel").css("color", Color.Error);
        $("#TxtValidatedOnErrorRequired").show();
    } else {
        $("#TxtValidatedOnErrorRequired").hide();
        if (validateDate($("#TxtValidatedOn").val()) === false) {
            ok = false;
            $("#TxtValidatedOnLabel").css("color", Color.Error);
            $("#TxtValidatedOnErrorDateMalformed").show();        }
        else {
            var closeDate = GetDate($("#TxtCloseQuestionsOn").val(), "/", false);
            var validationDate = GetDate($("#TxtValidatedOn").val(), "/", false);
            if (validationDate < closeDate) {
                // alex: se comenta el error en popup porque ya lo muestra en el fomrulario
                //alertUI(Dictionary.Item_Auditory_ErrorMessage_ValidateCrossDate);
                $("#TxtValidatedOnLabel").css("color", Color.Error);
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
        "notes": $("#TxtNotes").val(),
        "puntosFuertes": "",
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
            alertInfoUI(Dictionary.Item_Employee_Message_InsertSucess, Reload);
            //successInfoUI("todo ha ido bien", "auditorias", null);
            //document.location = document.location + "";
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
        $("#TxtIncidentActionTypeLabel").css("color", Color.Error);
        $("#TxtIncidentActionTypeErrorRequired").show();
    }

    if ($("#TxtIncidentActionDescription").val() === "") {
        ok = false;
        $("TxtIncidentActionDescriptionLabel").css("color", Color.Error);
        $("#TxtIncidentActionDescriptionErrorRequired").show();
    }

    if ($("#TxtWhatHappend").val() === "") {
        ok = false;
        $("#TxtWhatHappendLabel").css("color", Color.Error);
        $("#CmbWhatHappendByErrorRequired").show();
    }

    if ($("#CmbWhatHappendBy").val() * 1 < 1) {
        ok = false;
        $("#CmbWhatHappendByLabel").css("color", Color.Error);
        $("#TxtWhatHappendErrorRequired").show();
    }

    if ($("#TxtWahtHappendOn").val() === "") {
        ok = false;
        $("#TxtWahtHappendOnLabel").css("color", Color.Error);
        $("#TxtWahtHappendOnErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtWahtHappendOn").val()) === false) {
            ok = false;
            $("#TxtWahtHappendOnLabel").css("color", Color.Error);
            $("#TxtWahtHappendOnErrorDateMalformed").show();
        }
        else {
            // Comprobar que la fecha no es anterior a la fecha prevista de auditoria
            var plannedOn = GetDate($("#TxtPreviewDate").val(), "/");
            var actionDate = GetDate($("#TxtWahtHappendOn").val(), "/");
            if (actionDate < plannedOn) {
                ok = false;
                $("#TxtWahtHappendOnLabel").css("color", Color.Error);
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

// --------------- Provider/Customer BAR
// ---------------------------------------------------------------

// -- PROVIDERS 

var BARProviderSelected = null;
function ShowProviderBarPopup() {
    BARProviderSelected = $("#CmbProvider").val() * 1;
    ProviderRenderPopup();
    $("#dialogProvider").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Providers + "</h4>",
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "id": "BtnProviderSave",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                    "class": "btn btn-success btn-xs",
                    "click": function () { ProviderInsert(); }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
    });
}

function ProviderRenderPopup() {
    VoidTable("SelectableProvider");
    var target = document.getElementById("SelectableProvider");
    Providers.sort(CompareProviders);
    for (var x = 0; x < Providers.length; x++) {
        ProviderPopupRow(Providers[x], target);
    }
}

function CompareProviders(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function ProviderPopupRow(item, target) {
    if (item.Active === false) return;
    var tr = document.createElement("tr");
    tr.id = item.Id;
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    if (BARProviderSelected === item.Id) { td1.style.fontWeight = "bold"; }
    td1.appendChild(document.createTextNode(item.Description));

    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if (BARProviderSelected === item.Id) { span1.onclick = function () { alertUI(Dictionary.Common_Selected); }; }
    else { span1.onclick = function () { ProviderChanged(this); }; }

    div.appendChild(span1);

    var span2 = document.createElement("span");
    span2.className = "btn btn-xs btn-info";
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement("i");
    i2.className = "icon-edit bigger-120";
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span2);

    if (item.Id < 0) { span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); }; }
    else { span2.onclick = function () { ProviderUpdate(this); }; }

    var span3 = document.createElement("span");
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);

    if (BARProviderSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else if (item.Id < 0) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else {
        span3.onclick = function () { ProviderDelete(this); };
    }
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span3);
    td2.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}

function ProviderChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#CmbProvider").val(id);
    $("#dialogProvider").dialog("close");
}

function ProviderInsert(sender) {
    $("#TxtProviderNewNameErrorRequired").hide();
    $("#TxtProviderNewNameErrorDuplicated").hide();
    $("#TxtProviderNewName").val("");
    BARProviderSelected = null;
    $("#ProviderInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Equipment_Popup_AddProvider_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "ProviderInsertOk",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        var ok = true;
                        if ($("#TxtProviderNewName").val() === "") {
                            $("#TxtProviderNewNameErrorRequired").show();
                            ok = false;
                        }
                        else {
                            $("#TxtProviderNewNameErrorRequired").hide();
                        }

                        var duplicated = false;
                        for (var x = 0; x < Providers.length; x++) {
                            if ($("#TxtProviderNewName").val().toLowerCase() === Providers[x].Description.toLowerCase()) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            $("#TxtProviderNewNameErrorDuplicated").show();
                            ok = false;
                        }

                        if (ok === false) { window.scrollTo(0, 0); return false; }

                        $("#TxtProviderNewNameErrorRequired").hide();
                        $("#TxtProviderNewNameErrorDuplicated").hide();
                        $(this).dialog("close");
                        ProviderInsertConfirmed($("#TxtProviderNewName").val());
                    }
                },
                {
                    "id": "ProviderInsertCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
    });
}

function ProviderInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var description = "";
    var data = {
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    var newId = 0;
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ProviderActions.asmx/Insert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                Providers.push({ "Id": newId, "Description": newDescription, "Active": true, "Deletable": true });

                // 3.- Modificar la fila de la tabla del popup
                ProviderRenderPopup();
                FillCmbProvider();
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

function ProviderUpdate(sender) {
    $("#TxtProviderNameErrorRequired").hide();
    $("#TxtProviderNameErrorDuplicated").hide();
    $("#TxtProviderName").val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    BARProviderSelected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#ProviderUpdateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Common_Edit + "</h4>",
        "title_html": true,
        "buttons": [
            {
                "Id": "ProviderUpdateOk",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    if ($("#TxtProviderName").val() === "") {
                        $("#TxtProviderNameErrorRequired").show();
                        ok = false;
                    }
                    else {
                        $("#TxtProviderNameErrorRequired").hide();
                    }

                    var duplicated = false;
                    for (var x = 0; x < Providers.length; x++) {
                        if ($("#TxtProviderName").val().toLowerCase() === Providers[x].Description.toLowerCase() && Selected !== Providers[x].Id && Providers[x].Active === true) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        $("#TxtProviderNameErrorDuplicated").show();
                        ok = false;
                    }
                    else {
                        $("#TxtProviderNameErrorDuplicated").hide();
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    $("#TxtProviderNameErrorRequired").hide();
                    $("#TxtProviderNameErrorDuplicated").hide();
                    $(this).dialog("close");
                    ProviderUpdateConfirmed(BARProviderSelected, document.getElementById("TxtProviderName").value);
                }
            },
            {
                "Id": "ProviderUpdateCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function ProviderUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    for (var x = 0; x < Providers.length; x++) {
        if (Providers[x].Id === id) {
            description = Providers[x].Description;
            break;
        }
    }
    var data = {
        "providerId": id,
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ProviderActions.asmx/Update",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var y = 0; y < Providers.length; y++) {
        if (Providers[y].Id !== id) {
            temp.push(Providers[y]);
        }
        else {
            var item = Providers[y];
            temp.push({
                "Id": item.Id,
                "Description": newDescription,
                "Active": item.Active,
                "Deletable": item.Delete
            });
        }
    }

    Providers = [];
    for (var w = 0; w < temp.length; w++) {
        Providers.push(temp[w]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById("SelectableProvider");
    for (var z = 0; z < target.childNodes.length; z++) {
        if (target.childNodes[z].id * 1 === id) {
            target.childNodes[z].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (BARProviderSelected === id) {
        $("#CmbProvider").val(newDescription);
    }

    FillCmbProvider();
}

function ProviderDelete(sender) {
    $("#ProviderName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    BARProviderSelected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#ProviderDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Common_Delete + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "BARProviderDeleteBtnOK",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                        ProviderDeleteConfirmed(BARProviderSelected);
                    }
                },
                {
                    "id": "BARProviderDeleteBtnCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
    });
}

function ProviderDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var description = "";
    for (var x = 0; x < Providers.length; x++) {
        if (Providers[x].Id === id) {
            description = Providers[x].Description;
            break;
        }
    }

    var data = {
        "providerId": id,
        "description": description,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ProviderActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success !== true) { alertUI(response.d.MessageError); }
        },
        "error": function (jqXHR) {
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var w = 0; w < Providers.length; w++) {
        if (Providers[w].Id !== id) { temp.push(Providers[w]); }
    }

    Providers = new Array();
    for (var y = 0; y < temp.length; y++) {
        Providers.push(temp[y]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById("SelectableProvider");
    for (var z = 0; z < target.childNodes.length; z++) {
        if (target.childNodes[z].id * 1 === id * 1) {
            target.childNodes[z].style.display = "none";
            break;
        }
    }

    FillCmbProvider();
}

function FillCmbProvider() {
    var res = "<option value=\"0\">" + Dictionary.Common_SelectAll + "</option>";
    for (var x = 0; x < Providers.length; x++) {
        res += "<option value=\"" + Providers[x].Id + "\"";
        if (BARProviderSelected === Providers[x].Id) {
            res += " selected=\"selected\"";
        }

        res += ">" + Providers[x].Description + "</option>";
    }

    $("#CmbProvider").html(res);
}

// --- CUSTOMERS
var BARCustomerSelected = null;
function ShowCustomerBarPopup() {
    BARCustomerSelected = $("#CmbCustomer").val() * 1;
    CustomerRenderPopup();
    $("#dialogCustomer").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Customers + "</h4>",
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "id": "BtnCustomerSave",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                    "class": "btn btn-success btn-xs",
                    "click": function () { CustomerInsert(); }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
    });
}

function CustomerRenderPopup() {
    VoidTable("SelectableCustomer");
    var target = document.getElementById("SelectableCustomer");
    Customers.sort(CompareCustomers);
    for (var x = 0; x < Customers.length; x++) {
        CustomerPopupRow(Customers[x], target);
    }
}

function CompareCustomers(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function CustomerPopupRow(item, target) {
    if (item.Active === false) return;
    var tr = document.createElement("tr");
    tr.id = item.Id;
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    if (BARCustomerSelected === item.Id) { td1.style.fontWeight = "bold"; }
    td1.appendChild(document.createTextNode(item.Description));

    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if (BARCustomerSelected === item.Id) { span1.onclick = function () { alertUI(Dictionary.Common_Selected); }; }
    else { span1.onclick = function () { CustomerChanged(this); }; }

    div.appendChild(span1);

    var span2 = document.createElement("span");
    span2.className = "btn btn-xs btn-info";
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement("i");
    i2.className = "icon-edit bigger-120";
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span2);

    if (item.Id < 0) { span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); }; }
    else { span2.onclick = function () { CustomerUpdate(this); }; }

    var span3 = document.createElement("span");
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);

    if (BARCustomerSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else if (item.Id < 0) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else {
        span3.onclick = function () { CustomerDelete(this); };
    }
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span3);
    td2.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}

function CustomerChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#CmbCustomer").val(id);
    $("#dialogCustomer").dialog("close");
}

function CustomerInsert(sender) {
    $("#TxtCustomerNewNameErrorRequired").hide();
    $("#TxtCustomerNewNameErrorDuplicated").hide();
    $("#TxtCustomerNewName").val("");
    BARCustomerSelected = null;
    $("#CustomerInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Equipment_Popup_AddCustomer_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "CustomerInsertOk",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        var ok = true;
                        if ($("#TxtCustomerNewName").val() === "") {
                            $("#TxtCustomerNewNameErrorRequired").show();
                            ok = false;
                        }
                        else {
                            $("#TxtCustomerNewNameErrorRequired").hide();
                        }

                        var duplicated = false;
                        for (var x = 0; x < Customers.length; x++) {
                            if ($("#TxtCustomerNewName").val().toLowerCase() === Customers[x].Description.toLowerCase()) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            $("#TxtCustomerNewNameErrorDuplicated").show();
                            ok = false;
                        }

                        if (ok === false) { window.scrollTo(0, 0); return false; }

                        $("#TxtCustomerNewNameErrorRequired").hide();
                        $("#TxtCustomerNewNameErrorDuplicated").hide();
                        $(this).dialog("close");
                        CustomerInsertConfirmed($("#TxtCustomerNewName").val());
                    }
                },
                {
                    "id": "CustomerInsertCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
    });
}

function CustomerInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var description = "";
    var data = {
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    var newId = 0;
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/CustomerActions.asmx/Insert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                Customers.push({ "Id": newId, "Description": newDescription, "Active": true, "Deletable": true });

                // 3.- Modificar la fila de la tabla del popup
                CustomerRenderPopup();
                FillCmbCustomer();
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

function CustomerUpdate(sender) {
    $("#TxtCustomerNameErrorRequired").hide();
    $("#TxtCustomerNameErrorDuplicated").hide();
    $("#TxtCustomerName").val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    BARCustomerSelected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#CustomerUpdateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Common_Edit + "</h4>",
        "title_html": true,
        "buttons": [
            {
                "Id": "CustomerUpdateOk",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    if ($("#TxtCustomerName").val() === "") {
                        $("#TxtCustomerNameErrorRequired").show();
                        ok = false;
                    }
                    else {
                        $("#TxtCustomerNameErrorRequired").hide();
                    }

                    var duplicated = false;
                    for (var x = 0; x < Customers.length; x++) {
                        if ($("#TxtCustomerName").val().toLowerCase() === Customers[x].Description.toLowerCase() && Selected !== Customers[x].Id && Customers[x].Active === true) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        $("#TxtCustomerNameErrorDuplicated").show();
                        ok = false;
                    }
                    else {
                        $("#TxtCustomerNameErrorDuplicated").hide();
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    $("#TxtCustomerNameErrorRequired").hide();
                    $("#TxtCustomerNameErrorDuplicated").hide();
                    $(this).dialog("close");
                    CustomerUpdateConfirmed(BARCustomerSelected, document.getElementById("TxtCustomerName").value);
                }
            },
            {
                "Id": "CustomerUpdateCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function CustomerUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var data = {
        "customerId": id,
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/CustomerActions.asmx/UpdateSimple",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var y = 0; y < Customers.length; y++) {
        if (Customers[y].Id !== id) {
            temp.push(Customers[y]);
        }
        else {
            var item = Customers[y];
            temp.push({
                "Id": item.Id,
                "Description": newDescription,
                "Active": item.Active,
                "Deletable": item.Delete
            });
        }
    }

    Customers = [];
    for (var w = 0; w < temp.length; w++) {
        Customers.push(temp[w]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById("SelectableCustomer");
    for (var z = 0; z < target.childNodes.length; z++) {
        if (target.childNodes[z].id * 1 === id) {
            target.childNodes[z].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (BARCustomerSelected === id) {
        $("#CmbCustomer").val(newDescription);
    }

    FillCmbCustomer();
}

function CustomerDelete(sender) {
    $("#CustomerName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    BARCustomerSelected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#CustomerDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Common_Delete + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "BARCustomerDeleteBtnOK",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                        CustomerDeleteConfirmed(BARCustomerSelected);
                    }
                },
                {
                    "id": "BARCustomerDeleteBtnCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
    });
}

function CustomerDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var description = "";
    for (var x = 0; x < Customers.length; x++) {
        if (Customers[x].Id === id) {
            description = Customers[x].Description;
            break;
        }
    }

    var data = {
        "customerId": id,
        "description": description,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/CustomerActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success !== true) { alertUI(response.d.MessageError); }
        },
        "error": function (jqXHR) {
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var w = 0; w < Customers.length; w++) {
        if (Customers[w].Id !== id) { temp.push(Customers[w]); }
    }

    Customers = new Array();
    for (var y = 0; y < temp.length; y++) {
        Customers.push(temp[y]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById("SelectableCustomer");
    for (var z = 0; z < target.childNodes.length; z++) {
        if (target.childNodes[z].id * 1 === id * 1) {
            target.childNodes[z].style.display = "none";
            break;
        }
    }

    FillCmbCustomer();
}

function FillCmbCustomer() {
    var res = "<option value=\"0\">" + Dictionary.Common_SelectAll + "</option>";
    for (var x = 0; x < Customers.length; x++) {
        res += "<option value=\"" + Customers[x].Id + "\"";
        if (BARCustomerSelected === Customers[x].Id) {
            res += " selected=\"selected\"";
        }

        res += ">" + Customers[x].Description + "</option>";
    }

    $("#CmbCustomer").html(res);
}
// ---------------------------------------------------------------

// Botones de acceso a acciones reales
$(".icon-eye-open").each(function (index) {
    var tda = $(this).parent().parent().parent().children()[2];
    var query = $(tda).html().split('?')[1].split("\"")[0];
    $(this).on("click", function () { document.location = "ActionView.aspx?" + query; });
});