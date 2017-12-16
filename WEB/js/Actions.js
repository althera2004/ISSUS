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
            var option = document.createElement('OPTION');
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].Value));
            document.getElementById('CmdIncidentActionCostResponsible').appendChild(option);
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

$("#CmbActionsResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionsResponsible").val() * 1, Employees); });
$("#CmbClosedResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbClosedResponsible").val() * 1, Employees); });