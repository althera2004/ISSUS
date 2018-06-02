var IncidentActionlist;
var IncidentActionSelected;
var IncidentActionSelectedId;
var IncidentActionStatus = 0;
var IncidentCausesRequired = false;
var IncidentActionsRequired = false;
var IncidentClosedRequired = false;

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
    if (document.getElementById('TxtWhatHappened').value.length === 0 && !locked) {
        FieldSetRequired('TxtWhatHappenedLabel', Dictionary.Item_IncidentAction_Field_WhatHappened, false);
        FieldSetRequired('CmbWhatHappenedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleWhatHappend, false);
        FieldSetRequired('TxtWhatHappenedDateLabel', Dictionary.Common_Date, false);
        document.getElementById('CmbWhatHappenedResponsible').value = 0;
        if (document.getElementById('CmbWhatHappenedResponsible').value * 1 === 0) { document.getElementById('CmbWhatHappenedResponsible').value = ApplicationUser.Employee.Id; }
        IncidentWhatHappenedRequired = false;
    }
    else {
        FieldSetRequired('TxtWhatHappenedLabel', Dictionary.Item_IncidentAction_Field_WhatHappened, true);
        FieldSetRequired('CmbWhatHappenedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleWhatHappend, true);
        FieldSetRequired('TxtWhatHappenedDateLabel', Dictionary.Common_Date, true);
        if (document.getElementById('CmbWhatHappenedResponsible').value * 1 === 0)
        {

            document.getElementById('CmbWhatHappenedResponsible').value = ApplicationUser.Employee.Id;
        }
        if (document.getElementById('TxtWhatHappenedDate').value === '') {
            document.getElementById('TxtWhatHappenedDate').value = FormatDate(new Date(), '/');
        }
        IncidentWhatHappenedRequired = true;
    }
}

function TxtCausesChanged(locked) {
    if (IncidentActionStatus > 1 || locked === true || IncidentActionsRequired === true) { locked = true; }
    if (document.getElementById('TxtCauses').value.length === 0 && !locked) {
        FieldSetRequired('TxtCausesLabel', Dictionary.Item_IncidentAction_Field_Causes, false);
        FieldSetRequired('CmbCausesResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleCauses, false);
        FieldSetRequired('TxtCausesDateLabel', Dictionary.Item_IncidentAction_Field_Date, false);
        document.getElementById('CmbCausesResponsible').value = 0;
        IncidentCausesRequired = false;
        TxtWhatHappenedChanged(false);
    }
    else {
        FieldSetRequired('TxtCausesLabel', Dictionary.Item_IncidentAction_Field_Causes, true);
        FieldSetRequired('CmbCausesResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleCauses, true);
        FieldSetRequired('TxtCausesDateLabel', Dictionary.Item_IncidentAction_Field_Date, true);
        if (document.getElementById('CmbCausesResponsible').value * 1 === 0) { document.getElementById('CmbCausesResponsible').value = ApplicationUser.Employee.Id; }
        if (document.getElementById('TxtCausesDate').value === '') { document.getElementById('TxtCausesDate').value = FormatDate(new Date(), '/'); }
        IncidentCausesRequired = true;
        TxtWhatHappenedChanged(true);
    }
}

function TxtActionsChanged(locked) {
    if (IncidentActionStatus > 2 || locked === true || IncidentClosedRequired === true) { locked = true; }
    if (document.getElementById('TxtActions').value.length === 0 && !locked) {
        FieldSetRequired('TxtActionsLabel', Dictionary.Item_IncidentAction_Field_Actions, false);
        FieldSetRequired('CmbActionsResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleActions, false);
        FieldSetRequired('TxtActionsDateLabel', Dictionary.Common_DateExecution, false);
        // ISSUS-10 FieldSetRequired('CmbActionsExecuterLabel', Dictionary.Item_IncidentAction_Field_Executer, false);
        // ISSUS-10 FieldSetRequired('TxtActionsScheduleLabel', Dictionary.Item_IncidentAction_Field_Date, false);
        document.getElementById('CmbActionsResponsible').value = 0;
        // ISSUS-10 document.getElementById('CmbActionsExecuter').value = 0;
        document.getElementById('TxtActionsDate').value = '';
        // ISSUS-10 document.getElementById('TxtActionsSchedule').value = '';
        IncidentActionsRequired = false;
        TxtCausesChanged(false);
    }
    else {
        FieldSetRequired('TxtActionsLabel', Dictionary.Item_IncidentAction_Field_Actions, true);
        FieldSetRequired('CmbActionsResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleActions, true);
        FieldSetRequired('TxtActionsDateLabel', Dictionary.Common_DateExecution, true);
        // ISSUS-10 FieldSetRequired('CmbActionsExecuterLabel', Dictionary.Item_IncidentAction_Field_Executer, true);
        // ISSUS-10 FieldSetRequired('TxtActionsScheduleLabel', Dictionary.Item_IncidentAction_Field_Date, true);
        if (document.getElementById('CmbActionsResponsible').value * 1 === 0) { document.getElementById('CmbActionsResponsible').value = ApplicationUser.Employee.Id; }
        // ISSUS-10 if (document.getElementById('CmbActionsExecuter').value * 1 === 0) { document.getElementById('CmbActionsExecuter').value = ApplicationUser.Employee.Id; }
        if (document.getElementById('TxtActionsDate').value === '') { document.getElementById('TxtActionsDate').value = FormatDate(new Date(), '/'); }
        // ISSUS-10 if (document.getElementById('TxtActionsSchedule').value === '') { document.getElementById('TxtActionsSchedule').value = FormatDate(new Date(), '/'); }
        IncidentActionsRequired = true;
        TxtCausesChanged(true);
    }
}

function SetCloseRequired() {
    IncidentClosedRequired = false;
    if (document.getElementById('CmbClosedResponsible').value * 1 !== 0) { IncidentClosedRequired = true; }
    // else if (document.getElementById('TxtClosedDate').value !== '') { IncidentClosedRequired = true; }
    // else if (document.getElementById('CmbClosedExecutor').value * 1 !== 0) { IncidentClosedRequired = true; }
    // else if (document.getElementById('TxtClosedExecutorDate').value !== '') { IncidentClosedRequired = true; }

    if (IncidentClosedRequired === true)
    {
        FieldSetRequired('CmbClosedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
        FieldSetRequired('TxtClosedDateLabel', Dictionary.Common_Date, true);
        // ISSUS-10 FieldSetRequired('CmbClosedExecutorLabel', Dictionary.Item_IncidentAction_Field_Executer, true);
        // ISSUS-10 FieldSetRequired('TxtClosedExecutorDateLabel', Dictionary.Common_Date, true);
        if (document.getElementById('CmbClosedResponsible').value * 1 === 0) { document.getElementById('CmbClosedResponsible').value = ApplicationUser.Employee.Id; }
        // ISSUS-10 if (document.getElementById('CmbClosedExecutor').value * 1 === 0) { document.getElementById('CmbClosedExecutor').value = ApplicationUser.Employee.Id; }
        if (document.getElementById('TxtClosedDate').value === '') { document.getElementById('TxtClosedDate').value = FormatDate(new Date, '/'); }
        // ISSUS-10 if (document.getElementById('TxtClosedExecutorDate').value === '') { document.getElementById('TxtClosedExecutorDate').value = FormatDate(new Date, '/'); }
        TxtActionsChanged(true);
    }
    else {
        FieldSetRequired('CmbClosedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleClose, false);
        FieldSetRequired('TxtClosedDateLabel', Dictionary.Common_Date, false);
        // ISSUS-10 FieldSetRequired('CmbClosedExecutorLabel', Dictionary.Item_IncidentAction_Field_Executer, false);
        // ISSUS-10 FieldSetRequired('TxtClosedExecutorDateLabel', Dictionary.Common_Date, false);
        document.getElementById('CmbClosedResponsible').value = 0;
        // ISSUS-10 document.getElementById('CmbClosedExecutor').value = 0;
        document.getElementById('TxtClosedDate').value = '';
        // ISSUS-10 document.getElementById('TxtClosedExecutorDate').value = '';
        TxtActionsChanged(false);
    }
}

$("#CmbActionsResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionsResponsible").val() * 1, Employees, this); });
$("#CmbClosedResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbClosedResponsible").val() * 1, Employees, this); });

$("#BtnAnular").hide();
$("#BtnRestaurar").hide();
if (IncidentAction.ClosedOn === null) {
    $("#BtnAnular").show();
} else {
    $("#BtnRestaurar").show();
    AnulateLayout();
}

$("#BtnAnular").on("click", AnularPopup);
$("#BtnRestaurar").on("click", Restore);

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
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

/// <var>The anulation data</var>
var anulationData = null;

function AnularConfirmed() {
    /// <summary>
    /// Anulars the confirmed.
    /// </summary>
    /// <returns></returns>
    document.getElementById("TxtClosedDateLabel").style.color = "#000";
    document.getElementById("CmbClosedResponsibleLabel").style.color = "#000";
    document.getElementById("TxtClosedDateDateRequired").style.display = "none";
    document.getElementById("TxtClosedDateDateMalformed").style.display = "none";
    document.getElementById("CmbClosedResponsibleErrorRequired").style.display = "none";

    var ok = true;

    if ($("#TxtClosedDate").val() === "") {
        ok = false;
        document.getElementById("TxtClosedDateLabel").style.color = "#f00";
        document.getElementById("TxtClosedDateDateRequired").style.display = "";
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
        document.getElementById("CmbClosedResponsibleLabel").style.color = "#f00";
        document.getElementById("CmbClosedResponsibleErrorRequired").style.display = "";
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
    /// <summary>
    /// Anulates the layout.
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Restores this instance.
    /// </summary>
    /// <returns></returns>
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
    if (IncidentAction.Origin == 1) { document.getElementById("ROrigin1").checked = true; }
    if (IncidentAction.Origin == 2) { document.getElementById("ROrigin2").checked = true; }

    if (IncidentAction.Origin == 3) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").show();
        $("#BusinessRiskDiv").hide();
        $("#ObjetivoDiv").hide();
        $("#OportunityDiv").hide();
    }
    else if (IncidentAction.Origin == 4) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").hide();
        $("#BusinessRiskDiv").show();
        $("#ObjetivoDiv").hide();
        $("#OportunityDiv").hide();
    }
    else if (IncidentAction.Origin == 5) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").hide();
        $("#BusinessRiskDiv").hide();
        $("#ObjetivoDiv").show();
        $("#OportunityDiv").hide();
    }
    else if (IncidentAction.Origin == 6) {
        $("#ROriginDiv").hide();
        $("#RTypeDiv").hide();
        $("#RReporterDiv").hide();
        $("#IncidentDiv").hide();
        $("#BusinessRiskDiv").hide();
        $("#ObjetivoDiv").hide();
        $("#OportunityDiv").show();
    }
    else {
        if (IncidentAction.ReporterType == 1) {
            document.getElementById("RReporterType1").checked = true;
            $("#CmbReporterType1").val(IncidentAction.Department.Id);
        }

        if (IncidentAction.ReporterType == 2) {
            document.getElementById("RReporterType2").checked = true;
            $("#CmbReporterType2").val(IncidentAction.Provider.Id);
        }

        if (IncidentAction.ReporterType == 3) {
            document.getElementById("RReporterType3").checked = true;
            $("#CmbReporterType3").val(IncidentAction.Customer.Id);
        }

        if (IncidentAction.ActionType == 1) { document.getElementById("RType1").checked = true; }
        if (IncidentAction.ActionType == 2) { document.getElementById("RType2").checked = true; }
        if (IncidentAction.ActionType == 3) { document.getElementById("RType3").checked = true; }
        RReporterTypeChanged();
    }
}