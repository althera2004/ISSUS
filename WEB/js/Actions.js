var IncidentActionlist;
var IncidentActionSelected;
var IncidentActionSelectedId;
var IncidentActionStatus = 0;
var IncidentCausesRequired = false;
var IncidentActionsRequired = false;
var IncidentClosedRequired = false;
var anulationData = null;

var Origins = {
    "Auditory": 1,
    "Propuesta": 2,
    "Incident": 3,
    "BusinessRisk": 4,
    "Objetivo": 5,
    "Oportunity": 6
};

var ReporterType = {
    "Interna": 1,
    "Proveedor": 2,
    "Customer": 3
};

var ActionType = {
    "Mejora": 1,
    "Correctiva": 2,
    "Preventiva": 3
};

jQuery(function ($) {
    $("h1").parent().removeClass("col-sm-8");
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

    var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);

    $("#BtnSave").on("click", function (e) { e.preventDefault(); SaveAction(); });
    $("#BtnCancel").on("click", function (e) { e.preventDefault(); document.location = referrer; });
    $("#BtnSaveAction").on("click", function (e) { e.preventDefault(); SaveIncident(); });
    $("#BtnCancelAction").on("click", function (e) { e.preventDefault(); document.location = referrer; });
    $("#BtnPrint").on("click", PrintData);

    $("#BtnNewCost").on("click", function (e) {
        e.preventDefault();
        ShowNewCostPopup(0);
    });

    $("#CmbReporterType2Bar").on("click", function (e) {
        e.preventDefault();
        ShowProviderBarPopup($("#CmbReporterType2"));
    });

    $("#CmbReporterType3Bar").on("click", function (e) {
        e.preventDefault();
        ShowCustomerBarPopup($("#CmbReporterType3"));
    });

    $("#BtnCostBAR").on("click", function (e) {
        e.preventDefault();
        ShowCostBarPopup($("#TxtCostDescription"));
    });

    // Control wizard de la acción
    $("#TxtWhatHappened").on("keyup", function (e) { e.preventDefault(); TxtWhatHappenedChanged(); });
    $("#TxtCauses").on("keyup", function (e) { e.preventDefault(); TxtCausesChanged(); });
    $("#TxtActions").on("keyup", function (e) { e.preventDefault(); TxtActionsChanged(); });
    $("#CmbClosedResponsible").on("change", function (e) { e.preventDefault(); SetCloseRequired(); });
});

function SaveAction() {
    ClearFieldTextMessages("TxtDescription");
    ClearFieldTextMessages("ROrigin");
    ClearFieldTextMessages("RType");
    ClearFieldTextMessages("RReporterType");
    ClearFieldTextMessages("TxtWhatHappened");
    ClearFieldTextMessages("TxtWhatHappenedResponsible");
    ClearFieldTextMessages("TxtWhatHappenedDate");
    var ok = true;
    var ErrorMessage = new Array();

    var dateWhatHappened = null;
    var dateCauses = null;
    var dateActions = null;
    var dateActionsExecution = null;
    var dateClose = null;
    var dateCloseExecution = null;

    if (typeof IncidentAction.OportunityId === "undefined") {
        IncidentAction.OportunityId = -1;
    }

    if ($("#TxtDescription").val() === "") {
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_DescriptionRequired);
        SetFieldTextMessages("TxtDescription");
        ok = false;
    }

    if ($("#ROrigin1").prop("checked") === false && $("#ROrigin2").prop("checked") === false) {
        ok = false;
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_OriginRequired);
    }
    else {
        // Se asume que si el Id es -1, es nuevo y sólo puede ser "Propuesta por dirección" ó "Auditoria"
        if (IncidentAction.Id < 0) {
            //IncidentAction.Origin = Origins.Propuesta;
            if ($("#ROrigin1").prop("checked") === true) { IncidentAction.Origin = 1; }
            if ($("#ROrigin2").prop("checked") === true) { IncidentAction.Origin = 2; }
        }
    }


    // Sólo para propuestas por dirección y auditorias
    if (IncidentAction.Origin === Origins.Propuesta  || IncidentAction.Origin === Origins.Auditory) {
        if (!document.getElementById("RType1").checked && !document.getElementById("RType2").checked && !document.getElementById("RType3").checked) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_TypeRequired);
            $("#RTypeLabel").css("color", Color.Error);
        } else {
            $("#RTypeLabel").css("color", Color.Label);
        }

        if (!document.getElementById("RReporterType1").checked && !document.getElementById("RReporterType2").checked && !document.getElementById("RReporterType3").checked) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ReportedByRequired);
            $("#RReporterTypeLabel").css("color", Color.Error);
        } else {
            var origin = true;
            if (document.getElementById("RReporterType1").checked && $("#CmbReporterType1").val() * 1 === 0) { origin = false; }
            if (document.getElementById("RReporterType2").checked && $("#CmbReporterType2").val() * 1 === 0) { origin = false; }
            if (document.getElementById("RReporterType3").checked && $("#CmbReporterType3").val() * 1 === 0) { origin = false; }
            if (origin === true) {
                $("#RReporterTypeLabel").css("color", Color.Label);
            } else {				
				if(IncidentAction.Origin === Origins.Auditory &&  IncidentAction.AuditoryId !== null)
				{
					// en las acciones de aditorias reales, no se verifica el origen del reportador
                }
                else if (IncidentAction.Origin === Origins.Auditory && IncidentAction.AuditoryId == null && document.getElementById("RReporterType1").checked) {
                    // en las acciones de aditorias reales, no se verifica el origen del reportador
                }
				else {
					ok = false;
					ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ReportedByRequired);
					$("#RReporterTypeLabel").css("color", Color.Error);
				}
            }
        }
    }

    var state = 1;
    if ($("#TxtWhatHappened").val() === "") {
        ok = false;
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequired);
        SetFieldTextMessages("TxtWhatHappened");
    }

    if ($("#CmbWhatHappenedResponsible").val() * 1 === 0) {
        ok = false;
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequiredResponsible);
        SetFieldTextMessages("CmbWhatHappenedResponsible");
    }

    if ($("#TxtWhatHappenedDate").val() === "") {
        ok = false;
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRquiredDate);
        SetFieldTextMessages("TxtWhatHappenedDate");
    }
    else {
        if (!RequiredDateValue("TxtWhatHappenedDate")) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_WhatHappened);
        }
        else {
            dateWhatHappened = GetDate($("#TxtWhatHappenedDate").val(), "/", false);
        }
    }

    if (IncidentCausesRequired === true) {
        if ($("#TxtCauses").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequired);
            SetFieldTextMessages("TxtCauses");

        }
        if (document.getElementById("CmbCausesResponsible").value * 1 === 0) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredResponsible);
            SetFieldTextMessages("CmbCausesResponsible");
        }

        if ($("#TxtCausesDate").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredDate);
            SetFieldTextMessages("TxtCausesDate");
        }
        else {
            if (!RequiredDateValue("TxtCausesDate")) {
                ok = false;
                ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Causes);
            }
            else {
                dateCauses = GetDate($("#TxtCausesDate").val(), "/", false);
            }
        }
    }

    if (IncidentActionsRequired === true) {
        if ($("#TxtActions").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequired);
            SetFieldTextMessages("TxtActions");
        }

        if ($("#CmbActionsResponsible").val() * 1 === 0) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredResponsible);
            SetFieldTextMessages("CmbActionsResponsible");
        }

        if ($("#TxtActionsDate").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredDate);
            SetFieldTextMessages("TxtActionsDate");
        }
        else {
            if (!RequiredDateValue("TxtActionsDate")) {
                ok = false;
                ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Actions);
            }
            else {
                dateActions = GetDate($("#TxtActionsDate").val(), "/", false);
            }
        }
    }

    if (IncidentClosedRequired === true) {
        if ($("#CmbClosedResponsible").val() * 1 === 0) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseResponsibleRequired);
            SetFieldTextMessages("CmbClosedResponsible");
        }

        if ($("#TxtClosedDate").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseDate);
            SetFieldTextMessages("TxtClosedDate");
        }
        else {
            if (!RequiredDateValue("TxtClosedDate")) {
                ok = false;
                ErrorMessage.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Close);
            }
            else {
                dateClose = GetDate($("#TxtClosedDate").val(), "/", false);
            }
        }
    }

    // Detect untemporality dates
    var okDates = true;
    if (dateWhatHappened !== null) {
        if (dateCauses !== null && dateWhatHappened > dateCauses) {
            okDates = false;
            SetFieldTextMessages("TxtWhatHappenedDate");
            SetFieldTextMessages("TxtCausesDate");
        }

        if (dateActions !== null && dateWhatHappened > dateActions) {
            okDates = false;
            SetFieldTextMessages("TxtWhatHappenedDate");
            SetFieldTextMessages("TxtActionsDate");
        }

        if (dateClose !== null && dateWhatHappened > dateClose) {
            okDates = false;
            SetFieldTextMessages("TxtWhatHappenedDate");
            SetFieldTextMessages("TxtClosedDate");
        }

        if (Objetivo.Id > 0) {
            var InicioObjetivo = GetDate(Objetivo.StartDate, "/", false);
            if (dateWhatHappened < InicioObjetivo) {
                ok = false;
                ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_BeforeStartObjective + " (" + Objetivo.StartDate + ")");
                SetFieldTextMessages("TxtWhatHappenedDate");
            }

            if (Objetivo.PreviewEndDate !== null) {
                var FinalObjetivo = GetDate(Objetivo.PreviewEndDate, "/", false);
                if (dateWhatHappened > FinalObjetivo) {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_AfterEndObjective + " (" + Objetivo.PreviewEndDate + ")");
                    SetFieldTextMessages("TxtWhatHappenedDate");
                }
            }

            if (Objetivo.EndDate !== null) {
                var FinalObjetivo = GetDate(Objetivo.EndDate, "/", false);
                if (dateWhatHappened > FinalObjetivo) {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_AfterEndObjective + " (" + Objetivo.EndDate + ")");
                    SetFieldTextMessages("TxtWhatHappenedDate");
                }
            }
        }
    }

    if (dateCauses !== null) {
        if (dateActions !== null && dateCauses > dateActions) {
            okDates = false;
            SetFieldTextMessages("TxtActionsDate");
            SetFieldTextMessages("TxtCausesDate");
        }

        if (dateClose !== null && dateCauses > dateClose) {
            okDates = false;
            SetFieldTextMessages("TxtCausesDate");
            SetFieldTextMessages("TxtClosedDate");
        }

        if (Objetivo.Id > 0) {
           if (Objetivo.PreviewEndDate !== null) {
                var FinalObjetivo = GetDate(Objetivo.PreviewEndDate, "/", false);
                if (dateCauses > FinalObjetivo) {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_AfterEndObjective + " (" + Objetivo.PreviewEndDate + ")");
                    SetFieldTextMessages("TxtCausesDate");
                }
            }

            if (Objetivo.EndDate !== null) {
                var FinalObjetivo = GetDate(Objetivo.EndDate, "/", false);
                if (dateCauses > FinalObjetivo) {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_AfterEndObjective + " (" + Objetivo.EndDate + ")");
                    SetFieldTextMessages("TxtCausesDate");
                }
            }
        }
    }

    if (dateActions !== null) {
        if (dateClose !== null && dateActions > dateClose) {
            okDates = false;
            SetFieldTextMessages("TxtActionsDate");
            SetFieldTextMessages("TxtClosedDate");
        }

        if (Objetivo.Id > 0) {
            if (Objetivo.PreviewEndDate !== null) {
                var FinalObjetivo = GetDate(Objetivo.PreviewEndDate, "/", false);
                if (dateActions > FinalObjetivo) {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_AfterEndObjective + " (" + Objetivo.PreviewEndDate+")");
                    SetFieldTextMessages("TxtActionsDate");
                }
            }

            if (Objetivo.EndDate !== null) {
                var FinalObjetivo = GetDate(Objetivo.EndDate, "/", false);
                if (dateActions > FinalObjetivo) {
                    ok = false;
                    ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_AfterEndObjective + " (" + Objetivo.EndDate + ")");
                    SetFieldTextMessages("TxtActionsDate");
                }
            }
        }
    }

    if (okDates === false) {
        ok = false;
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_UntemporalyDates);
    }

    if (ok === false) {
        var ErrorContent = "<ul>";
        for (var x = 0; x < ErrorMessage.length; x++) {
            ErrorContent += "<li>" + ErrorMessage[x] + "</li>";
        }
        ErrorContent += "</ul>";
        warningInfoUI("<strong>" + Dictionary.Common_Message_FormErrors + "</strong><br />" + ErrorContent, null, 600);
        return false;
    }

    var Rtype = 0;
    if (document.getElementById("RType1").checked) { Rtype = ActionType.Mejora; }
    if (document.getElementById("RType2").checked) { Rtype = ActionType.Correctiva; }
    if (document.getElementById("RType3").checked) { Rtype = ActionType.Preventiva; }

    var RReporter = 0;
    if (document.getElementById("RReporterType1").checked) { RReporter = ReporterType.Interna; }
    if (document.getElementById("RReporterType2").checked) { RReporter = ReporterType.Proveedor; }
    if (document.getElementById("RReporterType3").checked) { RReporter = ReporterType.Customer; }

    var Department = { "Id": RReporter === ReporterType.Interna ? $("#CmbReporterType1").val() * 1 : 0 };
    var Provider = { "Id": RReporter === ReporterType.Proveedor ? $("#CmbReporterType2").val() * 1 : 0 };
    var Customer = { "Id": RReporter === ReporterType.Customer ? $("#CmbReporterType3").val() * 1 : 0 };

    var ROrigin = 0;
    if (IncidentAction.Id === -1) {
        if ($("#ROrigin1").prop("checked") === true) { ROrigin = 1; };
        if ($("#ROrigin2").prop("checked") === true) { ROrigin = 2; };
    }
    else if (IncidentAction.AuditoryId > 0) {
        RReporter = IncidentAction.ReporterType;
        Department = IncidentAction.Department;
        Provider = IncidentAction.Provider;
        Customer = IncidentAction.Customer;
        ROrigin = IncidentAction.Origin;
    }
    else if (IncidentAction.IncidentId > 0) {
        RReporter = IncidentAction.ReporterType;
        Department = IncidentAction.Department;
        Provider = IncidentAction.Provider;
        Customer = IncidentAction.Customer;

        // Incidencia
        ROrigin = 3;

        // Correctiva
        Rtype = 2;
    }
    else if (IncidentAction.BusinessRiskId > 0) {
        RReporter = IncidentAction.ReporterType;
        Department = IncidentAction.Department;
        Provider = IncidentAction.Provider;
        Customer = IncidentAction.Customer;

        // Analisis de riesgo
        ROrigin = Origins.BusinessRisk;

        // Preventiva
        Rtype = ActionType.Preventiva;
    }
    else if (IncidentAction.ObjetivoId > 0) {
        RReporter = IncidentAction.ReporterType;
        Department = IncidentAction.Department;
        Provider = IncidentAction.Provider;
        Customer = IncidentAction.Customer;

        // Objetivo
        ROrigin = Origins.Objetivo;

        // Preventiva
        Rtype = ActionType.Preventiva;
    }
    else if (typeof IncidentAction.Oportunity !== "undefined" && IncidentAction.Oportunity !== null) {
        if (IncidentAction.Oportunity.Id > 0) {
            RReporter = IncidentAction.ReporterType;
            Department = IncidentAction.Department;
            Provider = IncidentAction.Provider;
            Customer = IncidentAction.Customer;

            // Analisis de riesgo
            ROrigin = Origins.Oportunity;

            // Preventiva
            Rtype = ActionType.Preventiva;
        }
    }
	else {
		// 2022-15-05 si no tiene nada asociado es el que venia de base datos
		ROrigin = IncidentAction.Origin;
	}

    var whatHappenedOn = GetDate($("#TxtWhatHappenedDate").val(), "-");
    var causesOn = GetDate($("#TxtCausesDate").val(), "-");
    var actionsOn = GetDate($("#TxtActionsDate").val(), "-");
    var actionsSchedule = null;
    var closedOn = GetDate($("#TxtClosedDate").val(), "-");
    var closedExecutorOn = null;

    var action = {
        "Id": IncidentActionId,
        "CompanyId": Company.Id,
        "ActionType": Rtype,
        "Description": $("#TxtDescription").val(),
        "Origin": ROrigin,
        "ReporterType": RReporter,
        "Department": Department,
        "Provider": Provider,
        "Customer": Customer,
        "Number": IncidentAction.Number,
        "IncidentId": IncidentAction.IncidentId,
        "BusinessRiskId": IncidentAction.BusinessRiskId,
        "Objetivo": { "Id": Objetivo.Id },
        "Oportunity": IncidentAction.Oportunity,
        "WhatHappened": $("#TxtWhatHappened").val(),
        "WhatHappenedBy": { "Id": $("#CmbWhatHappenedResponsible").val() },
        "WhatHappenedOn": whatHappenedOn,
        "Causes": $("#TxtCauses").val(),
        "CausesBy": { "Id": $("#CmbCausesResponsible").val() },
        "CausesOn": causesOn,
        "Actions": $("#TxtActions").val(),
        "ActionsBy": { "Id": $("#CmbActionsResponsible").val() },
        "ActionsOn": actionsOn,
        "ActionsExecuter": { "Id": 0 },
        "ActionsSchedule": actionsSchedule,
        "Monitoring": $("#TxtMonitoring").val(),
        "ClosedBy": { "Id": $("#CmbClosedResponsible").val() },
        "ClosedOn": closedOn,
        "ClosedExecutor": { "Id": 0 },
        "ClosedExecutorOn": closedExecutorOn,
        "Notes": $("#TxtNotes").val()
    };

    var data = { "incidentAction": action, "userId": user.Id };
    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/Save",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            document.location = referrer;
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function CmbReporterDepartmentsFill() {
    var target = document.getElementById("CmbReporterType1");
    for (var x = 0; x < Departments.length; x++) {
        var option = document.createElement("OPTION");
        option.value = Departments[x].Id;
        option.appendChild(document.createTextNode(Departments[x].Description));
        target.appendChild(option);
    }
}

function CmbReporterProvidersFill() {
    VoidTable("CmbReporterType2");
    var target = document.getElementById("CmbReporterType2");

    var optionDefault = document.createElement("OPTION");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement("OPTION");
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        target.appendChild(option);
    }
}

function CmbReporterCustomersFill() {
    VoidTable("CmbReporterType3");
    var target = document.getElementById("CmbReporterType3");

    var optionDefault = document.createElement("OPTION");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);

    for (var x = 0; x < Customers.length; x++) {
        var option = document.createElement("OPTION");
        option.value = Customers[x].Id;
        option.appendChild(document.createTextNode(Customers[x].Description));
        target.appendChild(option);
    }
}

function RReporterTypeChanged() {
	if($("#ROrigin1").length > 0)
	{
    if (document.getElementById("RReporterType1").checked && document.getElementById("ROrigin1").checked) {
        $("#RReporterTypeCmb").hide();
        return;
    }
	}

    $("#RReporterTypeCmb").show();
    document.getElementById("DivCmbReporterType1").style.display = document.getElementById("RReporterType1").checked ? "" : "none";
    document.getElementById("DivCmbReporterType2").style.display = document.getElementById("RReporterType2").checked ? "" : "none";
    document.getElementById("DivCmbReporterType3").style.display = document.getElementById("RReporterType3").checked ? "" : "none";
}

function PrintData() {
    window.open("/export/PrintActionData.aspx?id=" + IncidentAction.Id + "&companyId=" + IncidentAction.CompanyId);
}

// Control de permisos
if (typeof ApplicationUser.Grants.IncidentActions === "undefined" || ApplicationUser.Grants.IncidentActions.Write === false) {
    $(".btn-danger").hide();
    $("input").attr("disabled", true);
    $("textarea").attr("disabled", true);
    $("select").attr("disabled", true);
    $("select").css("background-color", "#eee");
    $("#BtnNewUploadfileVersion").hide();
    $("#BtnNewCost").hide();
    $("#BtnNewUploadfile").hide();
    $("#IncidentActionCostsTableData .btn-info").hide();
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 450);
}

window.onload = function () {
    CmbReporterProvidersFill();
    CmbReporterCustomersFill();
    CmbReporterDepartmentsFill();
    SetLayout();
    FormLoad();
    Resize();
    document.getElementById("TxtDescription").focus();
    $("#ImgCompany").after("");
    if (IncidentAction.ClosedOn !== null) {
        $("input").attr("disabled", "disabled");
        $("select").attr("disabled", "disabled");
        $("textarea").attr("disabled", "disabled");
        $("#BtnNewCost").hide();
        $("#BtnNewUploadfile").hide();
        $(".icon-trash").parent().hide();
        $(".icon-edit").parent().hide();

        $("#CmbClosedResponsible").removeAttr("disabled");
        $("#TxtClosedDate").removeAttr("disabled");
    }

    $("#menuoption-13 a").show();
    $("#CmbActionsResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionsResponsible").val() * 1, Employees, this); });
    $("#CmbClosedResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbClosedResponsible").val() * 1, Employees, this); });
    $("#BtnAnular").on("click", AnularPopup);
    $("#BtnRestaurar").on("click", Restore);
    $("#BtnAnular").hide();
    $("#BtnRestaurar").hide();
    if (document.getElementById("IncidentActionCostsTableData") !== null) {
        IncidentActionCostRenderTable("IncidentActionCostsTableData");
    }

    if (IncidentAction.ClosedOn === null) {
        $("#BtnAnular").show();
    } else {
        $("#BtnRestaurar").show();
        AnulateLayout();
    }

    if (ApplicationUser.Grants.IncidentActions.Read === false) {
        $("input").attr("disabled", "disabled");
        $("textarea").attr("disabled", "disabled");
        $("select").attr("disabled", "disabled");
        $("#BtnAnular").hide();
        $("#BtnRestore").hide();
        $("#BtnPrint").hide();
        $("#BtnSave").hide();
        $("#Tabcostes").hide();
        $("#TabuploadFiles").hide();
    }

    $("#TxtCauses").bind("paste", TxtCausesChanged);
    $("#TxtActions").bind("paste", TxtActionsChanged);

    if (IncidentAction.Origin === 2 || IncidentAction.Id < 1) {
        $("#RTypeDiv").show();
        $("#RReporterDiv").show();
    }

    // Si es de auditoría hay que enseñar el tipo de acción
    if (IncidentAction.Origin === 1) {
        $("#RTypeDiv").show();
    }

    if (IncidentAction.Id < 1) {
        TxtActionsChanged(false);
    }
};

window.onresize = function () { Resize(); };

function CmdResponsibleFill() {
    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true && Employees[x].DisabledDate === null) {
            // Incident cost
            var option = document.createElement("OPTION");
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            document.getElementById("CmdIncidentActionCostResponsible").appendChild(option);
        }
    }
}

function FormLoad()
{
    CmdResponsibleFill();
    SetCloseRequired();
    if (IncidentClosedRequired === true)
    {
        TxtActionsChanged(true);
    }
    else if (IncidentAction.ActionsOn !== null) {
        TxtActionsChanged(true);
    }
    else if (IncidentAction.CausesOn !== null) {
        TxtCausesChanged(true);
    }
    else {
        TxtWhatHappenedChanged(true);
    }
}

// Control wizard de la acción
function TxtWhatHappenedChanged(locked) {
    locked = true;
    if ($("#TxtWhatHappened").val().length === 0 && !locked) {
        FieldSetRequired("TxtWhatHappenedLabel", Dictionary.Item_IncidentAction_Field_WhatHappened, false);
        FieldSetRequired("CmbWhatHappenedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleWhatHappend, false);
        FieldSetRequired("TxtWhatHappenedDateLabel", Dictionary.Common_Date, false);
        document.getElementById("CmbWhatHappenedResponsible").value = 0;
        if ($("#CmbWhatHappenedResponsible").val() * 1 === 0) { $("#CmbWhatHappenedResponsible").val(ApplicationUser.Employee.Id); }
        IncidentWhatHappenedRequired = false;
    }
    else {
        FieldSetRequired("TxtWhatHappenedLabel", Dictionary.Item_IncidentAction_Field_WhatHappened, true);
        FieldSetRequired("CmbWhatHappenedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleWhatHappend, true);
        FieldSetRequired("TxtWhatHappenedDateLabel", Dictionary.Common_Date, true);
        if ($("#CmbWhatHappenedResponsible").val() * 1 === 0)
        {
            $("#CmbWhatHappenedResponsible").val(ApplicationUser.Employee.Id);
        }
        if ($("#TxtWhatHappenedDate").val() === "") {
            $("#TxtWhatHappenedDate").val(FormatDate(new Date(), "/"));
        }
        IncidentWhatHappenedRequired = true;
    }
}

function TxtCausesChanged(locked) {
    if (IncidentActionStatus > 1 || locked === true || IncidentActionsRequired === true) { locked = true; }
    if ($("#TxtCauses").val().length === 0 && !locked) {
        FieldSetRequired("TxtCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, false);
        FieldSetRequired("CmbCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, false);
        FieldSetRequired("TxtCausesDateLabel", Dictionary.Item_IncidentAction_Field_Date, false);

        $("#CmbCausesResponsible").attr("disabled", "disabled");
        $("#TxtCausesDate").attr("disabled", "disabled");
        $("#TxtCausesDateBtn").attr("disabled", "disabled");

        $("#CmbCausesResponsible").val(0);
        $("#TxtCausesDate").val("");
        IncidentCausesRequired = false;
        TxtWhatHappenedChanged(false);
    }
    else {
        FieldSetRequired("TxtCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, true);
        FieldSetRequired("CmbCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, true);
        FieldSetRequired("TxtCausesDateLabel", Dictionary.Item_IncidentAction_Field_Date, true);

        $("#CmbCausesResponsible").removeAttr("disabled");
        $("#TxtCausesDate").removeAttr("disabled");
        $("#TxtCausesDateBtn").removeAttr("disabled");

        if (document.getElementById("CmbCausesResponsible").value * 1 === 0) {
            $("#CmbCausesResponsible").val(ApplicationUser.Employee.Id);
        }
        if ($("#TxtCausesDate").val() === "") {
            $("#TxtCausesDate").val(FormatDate(new Date(), "/"));
        }
        IncidentCausesRequired = true;
        TxtWhatHappenedChanged(true);
    }
}

function TxtActionsChanged(locked) {
    if (IncidentActionStatus > 2 || locked === true || IncidentClosedRequired === true) { locked = true; }
    if ($("#TxtActions").val().length === 0 && !locked) {
        FieldSetRequired("TxtActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, false);
        FieldSetRequired("CmbActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleActions, false);
        FieldSetRequired("TxtActionsDateLabel", Dictionary.Common_DateExecution, false);

        $("#CmbActionsResponsible").attr("disabled", "disabled");
        $("#TxtActionsDate").attr("disabled", "disabled");
        $("#TxtActionsDateBtn").attr("disabled", "disabled");

        $("#CmbActionsResponsible").val(0);
        $("#TxtActionsDate").val("");
        IncidentActionsRequired = false;
        TxtCausesChanged(false);
    }
    else {
        FieldSetRequired("TxtActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, true);
        FieldSetRequired("CmbActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleActions, true);
        FieldSetRequired("TxtActionsDateLabel", Dictionary.Common_DateExecution, true);

        $("#CmbActionsResponsible").removeAttr("disabled");
        $("#TxtActionsDate").removeAttr("disabled");
        $("#TxtActionsDateBtn").removeAttr("disabled");

        if ($("#CmbActionsResponsible").val() * 1 === 0) { document.getElementById("CmbActionsResponsible").value = ApplicationUser.Employee.Id; }
        if ($("#TxtActionsDate").val() === "") { document.getElementById("TxtActionsDate").value = FormatDate(new Date(), "/"); }
        IncidentActionsRequired = true;
        TxtCausesChanged(true);
    }
}

function SetCloseRequired() {
    IncidentClosedRequired = false;
    if ($("#CmbClosedResponsible").val() * 1 !== 0) { IncidentClosedRequired = true; }

    if (IncidentClosedRequired === true)
    {
        FieldSetRequired("CmbClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
        FieldSetRequired("TxtClosedDateLabel", Dictionary.Common_Date, true);
        if ($("#CmbClosedResponsible").val() * 1 === 0) { document.getElementById("CmbClosedResponsible").value = ApplicationUser.Employee.Id; }
        if ($("#TxtClosedDate").val() === "") { document.getElementById("TxtClosedDate").value = FormatDate(new Date, "/"); }
        TxtActionsChanged(true);
    }
    else {
        FieldSetRequired("CmbClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleClose, false);
        FieldSetRequired("TxtClosedDateLabel", Dictionary.Common_Date, false);
        $("#CmbClosedResponsible").val(0);
        $("#TxtClosedDate").val("");
        TxtActionsChanged(false);
    }
}

function AnularPopup() {
    if ($("#TxtMonitoring").val() === "") {
        warningInfoUI(Dictionary.Action_NoSeguimentMessage, Dictionary.Common_Warning);
        return false;
    }

    var ok = true;
    if ($("#TxtDescription").val() === "") { ok = false; }
    if ($("#TxtWhatHappened").val() === "") { ok = false; }
    if ($("#TxtCauses").val() === "") { ok = false; }
    if ($("#TxtActions").val() === "") { ok = false; }
    if ($("#TxtWhatHappenedDate").val() === "") { ok = false; }
    if ($("#TxtCausesDate").val() === "") { ok = false; }
    if ($("#TxtActionsDate").val() === "") { ok = false; }
    if ($("#CmbWhatHappenedResponsible").val() * 1 < 1) { ok = false; }
    if ($("#CmbCausesResponsible").val() * 1 < 1) { ok = false; }
    if ($("#CmbActionsResponsible").val() * 1 < 1) { ok = false; }

    if (ok === false) {
        alertUI(Dictionary.Common_Form_CheckError);
        return false;
    }

    $("#CmbClosedResponsibleLabel").html(Dictionary.Item_IncidentAction_Field_ResponsibleClose + "<span style=\"color:#f00;\">*</span>");
    $("#TxtClosedDateLabel").html(Dictionary.Item_IncidentAction_Field_Date + "<span style=\"color:#f00;\">*</span>");
    $("#CmbClosedResponsibleLabel").removeClass("control-label");
    $("#CmbClosedResponsibleLabel").removeClass("no-padding-right");
    $("#TxtClosedDate").val(FormatDate(new Date(), "/"));
    $("#CmbClosedResponsible").val(user.Employee.Id);
    $("#dialogAnular").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_IncidentAction_PopupAnular_Title,
        "width": 400,
        "buttons":
        [
            {
                "id": "BtnAnularSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_IncidentAction_Btn_Anular,
                "class": "btn btn-success btn-xs",
                "click": function () { AnularConfirmed(); }
            },
            {
                "id": "BtnAnularCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function AnularConfirmed() {
    $("#TxtClosedDateLabel").css("color", Color.Label);
    $("#CmbClosedResponsibleLabel").css("color", Color.Label);
    $("#TxtClosedDateDateRequired").hide();
    $("#TxtClosedDateDateMalformed").hide();
    $("#CmbClosedResponsibleErrorRequired").hide();
    $("#TxtClosedDateDateOutRange").hide();

    var ok = true;

    if ($("#TxtClosedDate").val() === "") {
        ok = false;
        $("#TxtClosedDateLabel").css("color", Color.Error);
        $("#TxtClosedDateDateRequired").show();
    }
    else {
        if (validateDate($("#TxtClosedDate").val()) === false) {
            ok = false;
            $("#TxtClosedDateLabel").css("color", Color.Error);
            $("#TxtClosedDateDateMalformed").show();
        }
        else {
            var d1 = GetDate($("#TxtClosedDate").val(), "/", false);
            var d0 = GetDate($("#TxtActionsDate").val(), "/", false);
            if (d1 < d0) {
                if ($("#TxtClosedDateDateOutRange").length === 0) {
                    $("#TxtClosedDateDateMalformed").after("<span class=\"ErrorMessage\" id=\"TxtClosedDateDateOutRange\" style=\"display:none;\">" + Dictionary.Item_IncidentAction_Error_BeforeActions + "</span>");
                }

                ok = false;
                $("#TxtClosedDateDateOutRange").show();
            }
        }
    }

    if ($("#CmbClosedResponsible").val() * 1 < 1) {
        ok = false;
        $("#CmbClosedResponsibleLabel").css("color", Color.Error);
        $("#CmbClosedResponsibleErrorRequired").show();
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "incidentActionId": IncidentAction.Id,
        "companyId": Company.Id,
        "responsible": $("#CmbClosedResponsible").val() * 1,
        "date": GetDate($("#TxtClosedDate").val(), "/"),
        "applicationUserId": user.Id
    };
    anulationData = data;
    $("#dialogAnular").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/Anulate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            SaveAction();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AnulateLayout() {
    $("#BtnRestaurar").hide();
    if (IncidentAction.ClosedOn !== null) {
        var message = "<br /><div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_IncidentAction_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndDate + ": <strong>" + GetDateYYYYMMDDText(IncidentAction.ClosedOn,"/", false) + "</strong><br />";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndResponsible + ": <strong>" + IncidentAction.ClosedBy.Value + "</strong><br />";
        message += "    </p>";
        message += "</div><br />";
        //$("#home").append(message);
        $("#ClosedPlaceholder").html(message);
        $("#BtnAnular").hide();
        $("#BtnRestaurar").show();
        $("#BtnSave").hide();
    }
    else {
        $("#DivAnulateMessage").hide();
        $("#BtnAnular").show();
    }
}

function Restore() {
    var data = {
        "incidentActionId": IncidentAction.Id,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/Restore",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            Incident.ClosedOn = null;
            document.location = document.location + "";
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function SetLayout() {
    switch (IncidentAction.Origin) {
        case Origins.Auditory:
            $("#AuditoryDiv").show();
            $("#RReporterType1").parent().hide();
            $("#RType3").parent().hide();
            $("#ROriginDiv").hide();
            $("#RReporterDiv select").attr("disabled", "disabled");
            $("#RTypeDiv input").attr("disabled", "disabled");
            $("#RReporterDiv input").attr("disabled", "disabled");
            $("#RReporterDiv button").hide();
			if(document.getElementById("RBOrigin") !== null) {
				document.getElementById("ROrigin1").checked = true;
			}
            switch (IncidentAction.ReporterType) {
                case 1:
                    document.getElementById("RReporterType1").checked = true;
                    $("#CmbReporterType1").val(IncidentAction.Department.Id);
                    console.log("Department", IncidentAction.Department.Id);
                    break;
                case 2:
                    document.getElementById("RReporterType2").checked = true;
                    $("#CmbReporterType2").val(IncidentAction.Provider.Id);
                    break;
                case 3:
                    document.getElementById("RReporterType3").checked = true;
                    $("#CmbReporterType3").val(IncidentAction.Customer.Id);
                    break;
            }

            switch (IncidentAction.ActionType) {
                case 1: document.getElementById("RType1").checked = true; break;
                case 2: document.getElementById("RType2").checked = true; break;
                case 3: document.getElementById("RType3").checked = true; break;
            }

            RReporterTypeChanged();
            break;
        case Origins.Propuesta:
            switch (IncidentAction.ReporterType) {
                case 1:
                    document.getElementById("RReporterType1").checked = true;
                    $("#CmbReporterType1").val(IncidentAction.Department.Id);
                    console.log("Department", IncidentAction.Department.Id);
                    break;
                case 2:
                    document.getElementById("RReporterType2").checked = true;
                    $("#CmbReporterType2").val(IncidentAction.Provider.Id);
                    break;
                case 3:
                    document.getElementById("RReporterType3").checked = true;
                    $("#CmbReporterType3").val(IncidentAction.Customer.Id);
                    break;
            }

            switch (IncidentAction.ActionType) {
                case 1: document.getElementById("RType1").checked = true; break;
                case 2: document.getElementById("RType2").checked = true; break;
                case 3: document.getElementById("RType3").checked = true; break;
            }

            RReporterTypeChanged();
            break;
        case Origins.Incident:
            $("#ROriginDiv").hide();
            $("#RTypeDiv").hide();
            $("#RReporterDiv").hide();
            $("#IncidentDiv").show();
            $("#BusinessRiskDiv").hide();
            $("#ObjetivoDiv").hide();
            $("#OportunityDiv").hide();
            if (typeof user.Grants.Incident === "undefined" || user.Grants.Incident.Read === false) {
                $("#IncidentLink").html(Dictionary.Item_Incident + " <i style=\"color:#777;\">" + Dictionary.Common_Message_ItemNoAccess + "</i>");
            }

            break;
        case Origins.BusinessRisk:
            $("#ROriginDiv").hide();
            $("#RTypeDiv").hide();
            $("#RReporterDiv").hide();
            $("#IncidentDiv").hide();
            $("#BusinessRiskDiv").show();
            $("#ObjetivoDiv").hide();
            $("#OportunityDiv").hide();
            if (typeof user.Grants.BusinessRisk === "undefined" || user.Grants.BusinessRisk.Read === false) {
                $("#IncidentBusinessRisk").html(Dictionary.Item_BusinessRisk + " <i style=\"color:#777;\">" + Dictionary.Common_Message_ItemNoAccess + "</i>");
            }

            break;
        case Origins.Objetivo:
            $("#ROriginDiv").hide();
            $("#RTypeDiv").hide();
            $("#RReporterDiv").hide();
            $("#IncidentDiv").hide();
            $("#BusinessRiskDiv").hide();
            $("#ObjetivoDiv").show();
            $("#OportunityDiv").hide();
            //document.getElementById("ROrigin3").checked = true;
            document.getElementById("RType1").checked = true;
            if (typeof user.Grants.Objetivo === "undefined" || user.Grants.Objetivo.Read === false) {
                $("#ObjetivoLink").html(Dictionary.Item_Objetivo + " <i style=\"color:#777;\">" + Dictionary.Common_Message_ItemNoAccess + "</i>");
            }

            break;
        case Origins.Oportunity:
            $("#ROriginDiv").hide();
            $("#RTypeDiv").hide();
            $("#RReporterDiv").hide();
            $("#IncidentDiv").hide();
            $("#BusinessRiskDiv").hide();
            $("#ObjetivoDiv").hide();
            $("#OportunityDiv").show();
            if (typeof user.Grants.Oportunity === "undefined" || user.Grants.Oportunity.Read === false) {
                $("#OportunityLink").html(Dictionary.Item_Oportunity + " <i style=\"color:#777;\">" + Dictionary.Common_Message_ItemNoAccess + "</i>");
            }

            break;
    }
}