﻿var IncidentActionlist;
var IncidentActionSelected;
var IncidentActionSelectedId;
var IncidentActionStatus = 0;
var IncidentCausesRequired = false;
var IncidentActionsRequired = false;
var IncidentClosedRequired = false;
var anulationData = null;

jQuery(function ($) {

    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;"
            if (("title_html" in this.options) && this.options.title_html === true)
                title.html($title);
            else title.text($title);
        }
    }));

    var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);

    $("#BtnSave").on("click", function (e) { e.preventDefault(); SaveAction(); });
    $("#BtnCancel").on("click", function (e) { document.location = referrer; });
    $("#BtnSaveAction").on("click", function (e) { e.preventDefault(); SaveIncident(); });
    $("#BtnCancelAction").on("click", function (e) { document.location = referrer; });
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

    if ($("#TxtDescription").val() === "") {
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_DescriptionRequired);
        SetFieldTextMessages("TxtDescription");
        ok = false;
    }

    if (IncidentAction.IncidentId < 1 && IncidentAction.BusinessRiskId < 1 && IncidentAction.ObjetivoId < 1 && IncidentAction.OportunityId < 1) {
        if (!document.getElementById("ROrigin1").checked && !document.getElementById("ROrigin2").checked) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_OriginRequired);
            $("#ROriginLabel").css("color", "#f00");
        } else {
            $("#ROriginLabel").css("color", "#000");
        }

        if (!document.getElementById("RType1").checked && !document.getElementById("RType2").checked && !document.getElementById("RType3").checked) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_TypeRequired);
            $("#RTypeLabel").css("color", "#f00");
        } else {
            $("#RTypeLabel").css("color", "#000");
        }

        if (!document.getElementById("RReporterType1").checked && !document.getElementById("RReporterType2").checked && !document.getElementById("RReporterType3").checked) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ReportedByRequired);
            $("#RReporterTypeLabel").css("color", "#f00");
        } else {
            var origin = true;
            if (document.getElementById("RReporterType1").checked && $("#CmbReporterType1").val() * 1 === 0) { origin = false; }
            if (document.getElementById("RReporterType2").checked && $("#CmbReporterType2").val() * 1 === 0) { origin = false; }
            if (document.getElementById("RReporterType3").checked && $("#CmbReporterType3").val() * 1 === 0) { origin = false; }
            if (origin === true) {
                $("#RReporterTypeLabel").css("color", "#000");
            } else {
                ok = false;
                ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_ReportedByRequired);
                $("#RReporterTypeLabel").css("color", "#f00");
            }
        }
    }

    var state = 1;
    if ($("#TxtWhatHappened").val() === "") {
        ok = false;
        ErrorMessage.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequired);
        SetFieldTextMessages("TxtWhatHappened");
    }

    if (document.getElementById("CmbWhatHappenedResponsible").value * 1 === 0) {
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

        if (document.getElementById("CmbActionsResponsible").value * 1 === 0) {
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

        if (document.getElementById("CmbClosedResponsible").value * 1 === 0) {
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
    }

    if (dateActions !== null) {
        if (dateClose !== null && dateActions > dateClose) {
            okDates = false;
            SetFieldTextMessages("TxtActionsDate");
            SetFieldTextMessages("TxtClosedDate");
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

    var ROrigin = 0;
    if (document.getElementById("ROrigin1").checked) { ROrigin = 1; }
    if (document.getElementById("ROrigin2").checked) { ROrigin = 2; }
    if (document.getElementById("ROrigin3").checked) { ROrigin = 5; }

    var Rtype = 0;
    if (document.getElementById("RType1").checked) { Rtype = 1; }
    if (document.getElementById("RType2").checked) { Rtype = 2; }
    if (document.getElementById("RType3").checked) { Rtype = 3; }

    var RReporter = 0;
    if (document.getElementById("RReporterType1").checked) { RReporter = 1; }
    if (document.getElementById("RReporterType2").checked) { RReporter = 2; }
    if (document.getElementById("RReporterType3").checked) { RReporter = 3; }

    var Department = { "Id": RReporter === 1 ? $("#CmbReporterType1").val() * 1 : 0 };
    var Provider = { "Id": RReporter === 2 ? $("#CmbReporterType2").val() * 1 : 0 };
    var Customer = { "Id": RReporter === 3 ? $("#CmbReporterType3").val() * 1 : 0 };

    if (IncidentAction.IncidentId > 0) {
        RReporter = IncidentAction.ReporterType;
        Department = IncidentAction.Department;
        Provider = IncidentAction.Provider;
        Customer = IncidentAction.Customer;

        // Incidencia
        ROrigin = 3;

        // Correctiva
        Rtype = 2;
    }

    if (IncidentAction.BusinessRiskId > 0) {
        RReporter = IncidentAction.ReporterType;
        Department = IncidentAction.Department;
        Provider = IncidentAction.Provider;
        Customer = IncidentAction.Customer;

        // Analisis de riesgo
        ROrigin = 4;

        // Preventiva
        Rtype = 3;
    }

    if (IncidentAction.ObjetivoId > 0) {
        RReporter = IncidentAction.ReporterType;
        Department = IncidentAction.Department;
        Provider = IncidentAction.Provider;
        Customer = IncidentAction.Customer;

        // Objetivo
        ROrigin = 5;

        // Preventiva
        Rtype = 3;
    }

    if (typeof IncidentAction.Oportunity !== "undefined" && IncidentAction.Oportunity !== null) {
        if (IncidentAction.Oportunity.Id > 0) {
            RReporter = IncidentAction.ReporterType;
            Department = IncidentAction.Department;
            Provider = IncidentAction.Provider;
            Customer = IncidentAction.Customer;

            // Analisis de riesgo
            ROrigin = 6;

            // Preventiva
            Rtype = 3;
        }
    }

    var whatHappenedOn = GetDate($("#TxtWhatHappenedDate").val(), "-");
    var causesOn = GetDate($("#TxtCausesDate").val(), "-");
    var actionsOn = GetDate($("#TxtActionsDate").val(), "-");
    var actionsSchedule = null; // ISSUS-10 GetDate($("#TxtActionsSchedule").val(), "-");
    var closedOn = GetDate($("#TxtClosedDate").val(), "-");
    var closedExecutorOn = null; // ISSUS-10 GetDate($("#TxtClosedExecutorDate").val(), "-");

    var action =
        {
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
            "Objetivo": Objetivo,
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

    var webMethod = "/Async/IncidentActionsActions.asmx/Save";
    var data = { "incidentAction": action, "userId": user.Id };
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
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
    CmbReporterProvidersFill();
    CmbReporterCustomersFill();
    CmbReporterDepartmentsFill();
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
}

window.onresize = function () { Resize(); }

function CmdResponsibleFill() {
    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true) {
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
    } else if (IncidentAction.ActionsOn !== null) {
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
    if (document.getElementById("TxtCauses").value.length === 0 && !locked) {
        FieldSetRequired("TxtCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, false);
        FieldSetRequired("CmbCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, false);
        FieldSetRequired("TxtCausesDateLabel", Dictionary.Item_IncidentAction_Field_Date, false);
        $("#CmbCausesResponsible").val(0);
        IncidentCausesRequired = false;
        TxtWhatHappenedChanged(false);
    }
    else {
        FieldSetRequired("TxtCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, true);
        FieldSetRequired("CmbCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, true);
        FieldSetRequired("TxtCausesDateLabel", Dictionary.Item_IncidentAction_Field_Date, true);
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
    if (document.getElementById("TxtActions").value.length === 0 && !locked) {
        FieldSetRequired("TxtActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, false);
        FieldSetRequired("CmbActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleActions, false);
        FieldSetRequired("TxtActionsDateLabel", Dictionary.Common_DateExecution, false);
        $("#CmbActionsResponsible").val(0);
        $("#TxtActionsDate").val("");
        IncidentActionsRequired = false;
        TxtCausesChanged(false);
    }
    else {
        FieldSetRequired("TxtActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, true);
        FieldSetRequired("CmbActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleActions, true);
        FieldSetRequired("TxtActionsDateLabel", Dictionary.Common_DateExecution, true);
        if ($("#CmbActionsResponsible").val() * 1 === 0) { document.getElementById("CmbActionsResponsible").value = ApplicationUser.Employee.Id; }
        if ($("#TxtActionsDate").val() === "") { document.getElementById("TxtActionsDate").value = FormatDate(new Date(), "/"); }
        IncidentActionsRequired = true;
        TxtCausesChanged(true);
    }
}

function SetCloseRequired() {
    IncidentClosedRequired = false;
    if (document.getElementById('CmbClosedResponsible').value * 1 !== 0) { IncidentClosedRequired = true; }

    if (IncidentClosedRequired === true)
    {
        FieldSetRequired('CmbClosedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
        FieldSetRequired('TxtClosedDateLabel', Dictionary.Common_Date, true);
        if (document.getElementById('CmbClosedResponsible').value * 1 === 0) { document.getElementById('CmbClosedResponsible').value = ApplicationUser.Employee.Id; }
        if ($("#TxtClosedDate").val() === "") { document.getElementById('TxtClosedDate').value = FormatDate(new Date, '/'); }
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
        alertUI("Revise los campos obligatorios");
        return false;
    }

    $("#CmbClosedResponsibleLabel").html(Dictionary.Item_IncidentAction_Field_ResponsibleClose + "<span style=\"color:#f00;\">*</span>");
    $("#TxtClosedDateLabel").html(Dictionary.Item_IncidentAction_Field_Date + "<span style=\"color:#f00;\">*</span>");
    $("#CmbClosedResponsibleLabel").removeClass("control-label");
    $("#CmbClosedResponsibleLabel").removeClass("no-padding-right");
    $("#TxtClosedDate").val(FormatDate(new Date(), "/"));
    $("#CmbClosedResponsible").val(user.Employee.Id);
    var dialog = $("#dialogAnular").removeClass("hide").dialog({
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
    $("#TxtClosedDateLabel").css("color", "#000");
    $("#CmbClosedResponsibleLabel").css("color", "#000");
    $("#TxtClosedDateDateRequired").hide();
    $("#TxtClosedDateDateMalformed").hide();
    $("#CmbClosedResponsibleErrorRequired").hide();

    var ok = true;

    if ($("#TxtClosedDate").val() === "") {
        ok = false;
        $("#TxtClosedDateLabel").css("color", "#f00");
        $("#TxtClosedDateDateRequired").show();
    }
    else {
        if (validateDate($("#TxtClosedDate").val()) === false) {
            ok = false;
            $("#TxtClosedDateLabel").css("color", "#f00");
            $("#TxtClosedDateDateMalformed").show();
        }
    }

    if ($("#CmbClosedResponsible").val() * 1 < 1) {
        ok = false;
        $("#CmbClosedResponsibleLabel").css("color", "#f00");
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
        "success": function (msg) {
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
        var message = "<div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_IncidentAction_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndDate + ": <strong>" + GetDateYYYYMMDDText(IncidentAction.ClosedOn,"/", false) + "</strong><br />";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndResponsible + ": <strong>" + IncidentAction.ClosedBy.Value + "</strong>";
        message += "    </p>";
        message += "</div><br /><br /><br />";
        $("#home").append(message);
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
        type: "POST",
        url: "/Async/IncidentActionsActions.asmx/Restore",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            Incident.ClosedOn = null;
            document.location = document.location + "";
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function SetLayout() {
    if (IncidentAction.Origin === 1) { document.getElementById("ROrigin1").checked = true; }
    if (IncidentAction.Origin === 2) { document.getElementById("ROrigin2").checked = true; }

    if (IncidentAction.Origin === 3) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").show();
        $("#BusinessRiskDiv").hide();
        $("#ObjetivoDiv").hide();
        $("#OportunityDiv").hide();
    }
    else if (IncidentAction.Origin === 4) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").hide();
        $("#BusinessRiskDiv").show();
        $("#ObjetivoDiv").hide();
        $("#OportunityDiv").hide();
    }
    else if (IncidentAction.Origin === 5) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").hide();
        $("#BusinessRiskDiv").hide();
        $("#ObjetivoDiv").show();
        $("#OportunityDiv").hide();
        document.getElementById("ROrigin3").checked = true;
        document.getElementById("RType1").checked = true;
    }
    else if (IncidentAction.Origin === 6) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").hide();
        $("#BusinessRiskDiv").hide();
        $("#ObjetivoDiv").hide();
        $("#OportunityDiv").show();
    }
    else {
        if (IncidentAction.ReporterType === 1) {
            document.getElementById("RReporterType1").checked = true;
            $("#CmbReporterType1").val(IncidentAction.Department.Id);
        }

        if (IncidentAction.ReporterType === 2) {
            document.getElementById("RReporterType2").checked = true;
            $("#CmbReporterType2").val(IncidentAction.Provider.Id);
        }

        if (IncidentAction.ReporterType === 3) {
            document.getElementById("RReporterType3").checked = true;
            $("#CmbReporterType3").val(IncidentAction.Customer.Id);
        }

        if (IncidentAction.ActionType === 1) { document.getElementById("RType1").checked = true; }
        if (IncidentAction.ActionType === 2) { document.getElementById("RType2").checked = true; }
        if (IncidentAction.ActionType === 3) { document.getElementById("RType3").checked = true; }
        RReporterTypeChanged();
    }
}