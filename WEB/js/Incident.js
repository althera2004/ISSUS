var SaveAction = false;
var IncidentWhatHappenedRequired = true;
var IncidentCausesRequired = IncidentStatus > 1;
var IncidentActionsRequired = IncidentStatus > 2;
var IncidentClosedRequired = IncidentStatus > 3;

var IncidentActionCausesRequired = IncidentAction.Causes !== '';
var IncidentActionActionsRequired = IncidentAction.Actions !== '';
var IncidentActionClosedRequired = IncidentAction.ClosedBy !== null;

function IncidentFormAfterLoad() {
    CmbReporterProvidersFill();
    CmbReporterCustomersFill();
    CmbReporterDepartmentsFill();
    CmbIncidentCostDescriptionFill();
    CmdResponsibleFill();

    if (Incident.Id > 0) {
        IncidentCostRenderTable("IncidentCostsTableData");
        if (ApplicationUser.Grants.IncidentActions.Write === false) {
            $("#costes .btn-info").hide();
            $("#costes .btn-danger").hide();
        }
        if (Incident.Department.Id > 0) { document.getElementById('RReporterType1').checked = true; document.getElementById('CmbReporterType1').value = Incident.Department.Id; }
        if (Incident.Provider.Id > 0) { document.getElementById('RReporterType2').checked = true; document.getElementById('CmbReporterType2').value = Incident.Provider.Id; }
        if (Incident.Customer.Id > 0) { document.getElementById('RReporterType3').checked = true; document.getElementById('CmbReporterType3').value = Incident.Customer.Id; }

        if (IncidentAction.Id > 0) {
            document.getElementById('Tabaccion').style.display = "";
            if (document.getElementById('Tabaccion') !== null) {
                document.getElementById("RActionYes").checked = true;
            }
        }
        else {
            document.getElementById("RActionNo").checked = true;
            if (document.getElementById("Tabaccion") !== null) {
                document.getElementById("Tabaccion").style.display = "none";
            }
        }

        RReporterTypeChanged();
    }
    else
    {
        document.getElementById("CmbWhatHappenedResponsible").value = ApplicationUser.Employee.Id;
        document.getElementById("TxtWhatHappenedDate").value = FormatDate(new Date(), "/");
    }

    FieldSetRequired("TxtDescriptionLabel", Dictionary.Item_Incident_Field_Description, true);
    FieldSetRequired("RReporterTypeLabel", Dictionary.Item_IncidentAction_Label_Reporter, true);
    FieldSetRequired("TxtWhatHappenedLabel", Dictionary.Item_Incident_Field_WhatHappened, true);
    FieldSetRequired("CmbWhatHappenedResponsibleLabel", Dictionary.Item_Incident_Field_WhatHappenedResponsible, true);
    FieldSetRequired("TxtWhatHappenedDateLabel", Dictionary.Item_Incident_Field_WhatHappenedDate,true);    

    // Init fields to no required
    if (IncidentStatus > 1) {
        FieldSetRequired("TxtCausesLabel", Dictionary.Item_Incident_Field_Causes, true);
        FieldSetRequired("CmbCausesResponsibleLabel", Dictionary.Item_Incident_Field_CausesResponsible, true);
        FieldSetRequired("TxtCausesDateLabel", Dictionary.Item_Incident_Field_CausesDate, true);
        if (IncidentStatus > 2) {
            FieldSetRequired("TxtActionsLabel", Dictionary.Item_Incident_Field_Actions, true);
            FieldSetRequired("CmbActionsResponsibleLabel", Dictionary.Item_Incident_Field_ActionsResponsible, true);
            // ISSUS-74 FieldSetRequired("CmbActionsExecuterLabel", Dictionary.Item_Incident_Field_ActionsExecuter, true);
            FieldSetRequired("TxtActionsDateLabel", Dictionary.Common_DateExecution, true);
            // ISSUS-74 FieldSetRequired("TxtActionsScheduleLabel", Dictionary.Item_Incident_Field_ActionsSchedule, true);
            if (IncidentStatus > 3) {
                FieldSetRequired("CmbClosedResponsibleLabel", Dictionary.Item_Incident_Field_CloseResponsible, true);
                FieldSetRequired("TxtClosedDateLabel", Dictionary.Item_Incident_Field_CloseDate, true);
            }
        }
    }
}

function IncidentActionFormAfterLoad() {
    // Si la incidencia es nueva se oculta la pestaña de acciones
    if (Incident.Id === 0)
    {
        document.getElementById('Tabaccion').style.display = 'none';
    }

    // What happened siempre obligatorio
    FieldSetRequired("TxtActionWhatHappenedLabel", Dictionary.Item_IncidentAction_Field_WhatHappened, true);
    FieldSetRequired("CmbActionWhatHappenedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleWhatHappend, true);
    FieldSetRequired("TxtActionWhatHappenedDateLabel", Dictionary.Common_Date, true);

    // Causes
    FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, IncidentActionCausesRequired);
    FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, IncidentActionCausesRequired);
    FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Common_Date, IncidentActionCausesRequired);

    // Actions
    FieldSetRequired("TxtActionActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, IncidentActionActionsRequired);
    FieldSetRequired("CmbActionActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleActions, IncidentActionActionsRequired);
    FieldSetRequired("TxtActionActionsDateLabel", Dictionary.Common_Date, IncidentActionActionsRequired);
    // ISSUS-10 FieldSetRequired("CmbActionActionsExecuterLabel", Dictionary.Item_IncidentAction_Field_Executer, IncidentActionActionsRequired);
    // ISSUS-10 FieldSetRequired("TxtActionActionsScheduleLabel", Dictionary.Item_IncidentAction_Field_Schelude, IncidentActionActionsRequired);

    // Close
    FieldSetRequired("CmbActionClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleClose, IncidentActionClosedRequired);
    FieldSetRequired("TxtActionClosedDateLabel", Dictionary.Item_IncidentAction_Field_Close, IncidentActionClosedRequired);
}

function RReporterTypeChanged() {
    document.getElementById("RReporterTypeCmb").style.display = "";
    document.getElementById("DivCmbReporterType1").style.display = document.getElementById("RReporterType1").checked ? "" : "none";
    document.getElementById("DivCmbReporterType2").style.display = document.getElementById("RReporterType2").checked ? "" : "none";
    document.getElementById("DivCmbReporterType3").style.display = document.getElementById("RReporterType3").checked ? "" : "none";

    /* ISSUS-91
    if (document.getElementById("RReporterType1").checked) {
        document.getElementById("CmbReporterType2").value = 0;
        document.getElementById("CmbReporterType3").value = 0;
    }

    if (document.getElementById("RReporterType2").checked) {
        document.getElementById("CmbReporterType1").value = 0;
        document.getElementById("CmbReporterType3").value = 0;
    }

    if (document.getElementById("RReporterType3").checked) {
        document.getElementById("CmbReporterType1").value = 0;
        document.getElementById("CmbReporterType2").value = 0;
    }
    */
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

function CmdResponsibleFill() {
    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true) {
            var option = document.createElement("OPTION");
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            document.getElementById("CmdIncidentCostResponsible").appendChild(option);
        }
    }
}

function SaveIncident() {
    var ok = true;
    var ErrorMessage = new Array();
    var ErrorMessageActions = new Array();

    // Reset form error color
    ClearFieldTextMessages("TxtDescription");
    ClearFieldTextMessages("ROrigin");
    ClearFieldTextMessages("RType");
    ClearFieldTextMessages("RReporterType");
    ClearFieldTextMessages("TxtWhatHappened");
    ClearFieldTextMessages("CmbWhatHappenedResponsible");
    ClearFieldTextMessages("TxtWhatHappenedDate");
    ClearFieldTextMessages("TxtCausesLabel");
    ClearFieldTextMessages("CmbCausesResponsible");
    ClearFieldTextMessages("TxtCausesDate");
    ClearFieldTextMessages("TxtActions");
    ClearFieldTextMessages("CmbActionsResponsible");
    ClearFieldTextMessages("TxtActionsDate");
    // ISSUS-74 ClearFieldTextMessages("CmbActionsExecuter");
    // ISSUS-74 ClearFieldTextMessages("TxtActionsSchedule");

    // Reset form error color for action if needed
    if (document.getElementById("RActionYes").checked === true) {
        ClearFieldTextMessages("TxtActionDescription");
        ClearFieldTextMessages("TxtActionWhatHappened");
        ClearFieldTextMessages("CmbActionWhatHappenedResponsible");
        ClearFieldTextMessages("TxtActionWhatHappenedDate");
        ClearFieldTextMessages("TxtActionCauses");
        ClearFieldTextMessages("CmbActionCausesResponsible");
        ClearFieldTextMessages("TxtActionCausesDate");
        ClearFieldTextMessages("TxtActionActions");
        ClearFieldTextMessages("CmbActionActionsResponsible");
        ClearFieldTextMessages("TxtActionActionsDate");
        // ISSUS-10 ClearFieldTextMessages("CmbActionActionsExecuter");
        // ISSUS-10 ClearFieldTextMessages("TxtActionActionsSchedule");
        ClearFieldTextMessages("CmbActionClosedResponsible");
        ClearFieldTextMessages("TxtActionClosedDate");
    }

    var DateWhatHappened = null;
    var DateCauses = null;
    var DateActions = null;
    var DateActionsSchedule = null;
    var DateClose = null;

    var DateActionWhatHappened = null;
    var DateActionCauses = null;
    var DateActionActions = null;
    var DateActionActionsSchedule = null;
    var DateActionClose = null;

    if (document.getElementById("TxtDescription").value === "") {
        ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_DescriptionRequired);
        SetFieldTextMessages("TxtDescription");
        ok = false;
    }

    if (!document.getElementById("RReporterType1").checked && !document.getElementById("RReporterType2").checked && !document.getElementById("RReporterType3").checked) {
        ok = false;
        document.getElementById("RReporterTypeLabel").style.color = "#f00";
        ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ReporterTypeRequired);
    } else {
        var origin = true;
        if (document.getElementById("RReporterType1").checked && $("#CmbReporterType1").val() * 1 === 0) {
            origin = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ReporterDepartmentRequired);
        }

        if (document.getElementById("RReporterType2").checked && $("#CmbReporterType2").val() * 1 === 0) {
            origin = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ReporterProviderRequired);
        }

        if (document.getElementById('RReporterType3').checked && $('#CmbReporterType3').val() * 1 === 0) {
            origin = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ReporterCustomerRequired);
        }

        if (origin === true) {
            document.getElementById('RReporterTypeLabel').style.color = '#000';
        } else {
            ok = false;
            document.getElementById('RReporterTypeLabel').style.color = '#f00';
        }
    }

    if ($("#TxtWhatHappened").val() === "") {
        ok = false;
        ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_WhatHappenedRequired);
        SetFieldTextMessages("TxtWhatHappened");
    }

    if (document.getElementById("CmbWhatHappenedResponsible").value * 1 === 0) {
        ok = false;
        ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_WhatHappenedResponsibleRequired);
        SetFieldTextMessages("CmbWhatHappenedResponsible");
    }

    if ($("#TxtWhatHappenedDate").val() === "") {
        ok = false;
        ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_WhatHappenedDateRequired);
        SetFieldTextMessages("TxtWhatHappenedDate");
    }
    else {
        if (!RequiredDateValue("TxtWhatHappenedDate")) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_Error_DateMalformed_WhatHappened);
        }
        else {
            DateWhatHappened = GetDate($("#TxtWhatHappenedDate").val(), "/", false);
        }
    }

    if (IncidentCausesRequired === true || IncidentActionsRequired === true || IncidentClosedRequired === true) {
        if ($("#TxtCauses").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_CausesRequired);
            SetFieldTextMessages("TxtCauses");
        }

        if (document.getElementById("CmbCausesResponsible").value * 1 === 0) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_CausesResponsibleRequired);
            SetFieldTextMessages("CmbCausesResponsible");
        }

        if ($("#TxtCausesDate").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_CausesDateRequired);
            SetFieldTextMessages("TxtCausesDate");
        }
        else {
            if (!RequiredDateValue("TxtCausesDate")) {
                ok = false;
                ErrorMessage.push(Dictionary.Item_Incident_Error_DateMalformed_Causes);
            }
            else {
                DateCauses = GetDate($("#TxtCausesDate").val(), "/", false);
            }
        }
    }

    if (IncidentActionsRequired === true || IncidentClosedRequired === true) {
        if ($("#TxtActions").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ActionsRequired);
            SetFieldTextMessages("TxtActions");
        }

        if (document.getElementById("CmbActionsResponsible").value * 1 === 0) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ActionsResponsibleRequired);
            SetFieldTextMessages("CmbActionsResponsible");
        }

        if ($("#TxtActionsDate").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ActionsDateRequired);
            SetFieldTextMessages("TxtActionsDate");
        }
        else {
            if (!RequiredDateValue("TxtActionsDate")) {
                ok = false;
                ErrorMessage.push(Dictionary.Item_Incident_Error_DateMalformed_Actions);
            }
            else {
                DateActions = GetDate($("#TxtActionsDate").val(), "/", false);
            }
        }

        /* ISSUS-74
        if (document.getElementById("CmbActionsExecuter").value * 1 === 0) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ActionsExecuterRequired);
            SetFieldTextMessages("CmbActionsExecuter");
        }

        if ($("#TxtActionsSchedule").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ActionsScheduleRequired);
            SetFieldTextMessages("TxtActionsSchedule");
        }
        else
        {
            DateActionsSchedule = GetDate($("#TxtActionsSchedule").val(), "/", false);
        }*/
    }

    if (IncidentClosedRequired === true) {
        if (document.getElementById("CmbClosedResponsible").value * 1 === 0) {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_CloseResponsibleRequired);
            SetFieldTextMessages("CmbClosedResponsible");
        }

        if ($("#TxtClosedDate").val() === "") {
            ok = false;
            ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_ClosedDateRequired);
            SetFieldTextMessages("TxtClosedDate");
        }
        else {
            if (!RequiredDateValue("TxtClosedDate")) {
                ok = false;
                ErrorMessage.push(Dictionary.Item_Incident_Error_DateMalformed_Close);
            }
            else {
                DateClose = GetDate($("#TxtClosedDate").val(), "/", false);
            }
        }
    }

    // revisar fechas
    var okFechas = true;
    console.log("Fechas incidencia");
    console.log(DateWhatHappened);
    console.log(DateCauses);
    console.log(DateActions);
    console.log(DateClose);
    if (DateWhatHappened !== null) {
        if (DateCauses !== null && DateWhatHappened > DateCauses) {
            okFechas = false;
            SetFieldTextMessages("TxtWhatHappenedDate");
            SetFieldTextMessages("TxtCausesDate");
        }

        if (DateActions !== null && DateWhatHappened > DateActions) {
            okFechas = false;
            SetFieldTextMessages("TxtWhatHappenedDate");
            SetFieldTextMessages("TxtActionsDate");
        }

        //if (DateActionsSchedule !== null && DateWhatHappened > DateActionsSchedule) {
        //    okFechas = false;
        //    SetFieldTextMessages("TxtWhatHappenedDate");
        //    SetFieldTextMessages("TxtActionsSchedule");
        //}

        if (DateClose !== null && DateWhatHappened > DateClose) {
            okFechas = false;
            SetFieldTextMessages("TxtWhatHappenedDate");
            SetFieldTextMessages("TxtClosedDate");
        }
    }

    if (DateCauses !== null) {
        if (DateActions !== null && DateCauses > DateActions) {
            okFechas = false;
            SetFieldTextMessages("TxtCausesDate");
            SetFieldTextMessages("TxtActionsDate");
        }

        //if (DateActionsSchedule !== null && DateCauses > DateActionsSchedule) {
        //    okFechas = false;
        //    SetFieldTextMessages("TxtCausesDate");
        //    SetFieldTextMessages("TxtActionsSchedule");
        //}

        if (DateClose !== null && DateCauses > DateClose) {
            okFechas = false;
            SetFieldTextMessages("TxtCausesDate");
            SetFieldTextMessages("TxtClosedDate");
        }
    }

    if (DateActions !== null) {
        //if (DateActionsSchedule !== null && DateActions > DateActionsSchedule) {
        //    okFechas = false;
        //    SetFieldTextMessages("TxtActionsDate");
        //    SetFieldTextMessages("TxtActionsSchedule");
        //}

        if (DateClose !== null && DateActions > DateClose) {
            okFechas = false;
            SetFieldTextMessages("TxtActionsDate");
            SetFieldTextMessages("TxtClosedDate");
        }
    }

    //if (DateActionsSchedule !== null) {
    //    if (DateClose !== null && DateActionsSchedule > DateClose) {
    //        okFechas = false;
    //        SetFieldTextMessages("TxtActionsSchedule");
    //        SetFieldTextMessages("TxtClosedDate");
    //    }
    //}

    if (okFechas === false) {
        ok = false;
        ErrorMessage.push(Dictionary.Item_Incident_ErrorMessage_DatesUntemporality);
    }

    SaveAction = false;
    if (document.getElementById("RActionYes").checked === true) {
        SaveAction = true;
        if (document.getElementById("TxtActionDescription").value === "") {
            ok = false;
            SetFieldTextMessages("TxtActionDescription");
            ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_DescriptionRequired);
        }

        if (document.getElementById("TxtActionWhatHappened").value === "") {
            ok = false;
            SetFieldTextMessages("TxtActionWhatHappened");
            ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequired);
        }

        if (document.getElementById("CmbActionWhatHappenedResponsible").value * 1 === 0) {
            ok = false;
            SetFieldTextMessages("CmbActionWhatHappenedResponsible");
            ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequiredResponsible);
        }

        if (document.getElementById("TxtActionWhatHappenedDate").value === "") {
            ok = false;
            SetFieldTextMessages("TxtActionWhatHappenedDate");
            ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRquiredDate);
        }
        else {
            if (!RequiredDateValue("TxtActionWhatHappenedDate")) {
                ok = false;
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_Error_DateMalformed_WhatHappened);
            }
            else {
                DateActionWhatHappened = GetDate($("#TxtActionWhatHappenedDate").val(), "/", false);
            }
        }

        if (IncidentActionCausesRequired === true) {
            if (document.getElementById("TxtActionCauses").value === "") {
                ok = false;
                SetFieldTextMessages("TxtActionCauses");
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequired);
            }

            if (document.getElementById("CmbActionCausesResponsible").value * 1 === 0) {
                ok = false;
                SetFieldTextMessages("CmbActionCausesResponsible");
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredResponsible);
            }

            if (document.getElementById("TxtActionCausesDate").value === "") {
                ok = false;
                SetFieldTextMessages("TxtActionCausesDate");
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredDate);
            }
            else {
                if (!RequiredDateValue("TxtActionCausesDate")) {
                    ok = false;
                    ErrorMessageActions.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Causes);
                }
                else {
                    DateActionCauses = GetDate($("#TxtActionCausesDate").val(), "/", false);
                }
            }
        }

        if (IncidentActionActionsRequired === true) {

            if (document.getElementById("TxtActionActions").value === "") {
                ok = false;
                SetFieldTextMessages("TxtActionActions");
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequired);
            }

            if (document.getElementById("CmbActionActionsResponsible").value * 1 === 0) {
                ok = false;
                SetFieldTextMessages("CmbActionActionsResponsible");
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredResponsible);
            }

            if (document.getElementById("TxtActionActionsDate").value === "") {
                ok = false;
                SetFieldTextMessages("TxtActionActionsDate");
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredDate);
            }
            else {
                if (!RequiredDateValue("TxtActionActionsDate")) {
                    ok = false;
                    ErrorMessageActions.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Actions);
                }
                else {
                    DateActionActions = GetDate($("#TxtActionActionsDate").val(), "/", false);
                }
            }

            /* ISSUS-10 
            if (document.getElementById("CmbActionActionsExecuter").value * 1 === 0) {
                ok = false;
                SetFieldTextMessages("CmbActionActionsExecuter");
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredExcutor);
            } */

            /* ISSUS-10 
            if (document.getElementById("TxtActionActionsSchedule").value === "") {
                ok = false;
                SetFieldTextMessages("TxtActionActionsSchedule")
                ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredSchedule);
            }
            else {
                DateActionActionsSchedule = GetDate($("#TxtActionActionsSchedule").val(), "/", false);
            }
            */

            if (IncidentActionClosedRequired === true) {
                if (document.getElementById("CmbActionClosedResponsible").value * 1 === 0) {
                    ok = false;
                    SetFieldTextMessages(CmbActionClosedResponsible);
                    ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseResponsibleRequired);
                }

                if (document.getElementById("TxtActionClosedDate").value === "") {
                    ok = false;
                    SetTextFieldMessages("TxtActionClosedDate");
                    ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseDate);
                }
                else {
                    if (!RequiredDateValue("TxtActionClosedDate")) {
                        ok = false;
                        ErrorMessageActions.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Close);
                    }
                    else {
                        DateActionClose = GetDate($("#TxtActionClosedDate").val(), "/", false);
                    }
                }
            }
        }

        var okDatesAction = true;
        if (DateActionWhatHappened !== null) {
            if (DateActionCauses !== null && DateActionCauses < DateActionWhatHappened) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionWhatHappenedDate");
                SetFieldTextMessages("TxtActionCausesDate");
            }

            if (DateActionActions !== null && DateActionActions < DateActionWhatHappened) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionWhatHappenedDate");
                SetFieldTextMessages("TxtActionActionsDate");
            }

            /* ISSUS-10
            if (DateActionActionsSchedule !== null && DateActionActionsSchedule < DateActionWhatHappened) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionWhatHappenedDate");
                SetFieldTextMessages("TxtActionActionsSchedule");
            }
            */

            if (DateActionClose !== null && DateActionClose < DateActionWhatHappened) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionWhatHappenedDate");
                SetFieldTextMessages("TxtActionClosedDate");
            }
        }

        if (DateActionCauses !== null) {
            if (DateActionActions !== null && DateActionActions < DateActionCauses) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionCausesDate");
                SetFieldTextMessages("TxtActionActionsDate");
            }

            /* ISSUS-10
            if (DateActionActionsSchedule != null && DateActionActionsSchedule < DateActionCauses) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionCausesDate");
                SetFieldTextMessages("TxtActionActionsSchedule");
            }
            */

            if (DateActionClose !== null && DateActionClose < DateActionCauses) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionCausesDate");
                SetFieldTextMessages("TxtActionClosedDate");
            }
        }

        if (DateActionActions !== null) {
            /* ISSUS-10
            if (DateActionActionsSchedule != null && DateActionActionsSchedule < DateActionActions) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionActionsDate");
                SetFieldTextMessages("TxtActionActionsSchedule");
            }
            */

            if (DateActionClose !== null && DateActionClose < DateActionActions) {
                okDatesAction = false;
                SetFieldTextMessages("TxtActionActionsDate");
                SetFieldTextMessages("TxtActionClosedDate");
            }
        }

        /* ISSUS-10
        if (DateActionActionsSchedule !== null) {
            if (DateActionClose !== null && DateActionClose < DateActionActionsSchedule) {
                okDatesAction = false;
                SetFieldTextMessages('TxtActionActionsSchedule');
                SetFieldTextMessages('TxtActionClosedDate');
            }
        }
        */

        if (okDatesAction === false) {
            ok = false;
            ErrorMessageActions.push(Dictionary.Item_IncidentAction_ErrorMessage_UntemporalyDates);
        }
    }

    if (ok === false) {
        var ErrorContent = "";
        if (ErrorMessage.length > 0) {
            ErrorContent += "<h4>" + Dictionary.Item_Incident + "</h4><ul>";

            for (var xi = 0; xi < ErrorMessage.length; xi++) {
                ErrorContent += "<li>" + ErrorMessage[xi] + "</li>";
            }
            ErrorContent += "</ul>";
        }
        if (ErrorMessageActions.length > 0) {
            ErrorContent += "<h4>" + Dictionary.Item_IncidentAction + "</h4><ul>";

            for (var xa = 0; xa < ErrorMessageActions.length; xa++) {
                ErrorContent += "<li>" + ErrorMessageActions[xa] + "</li>";
            }
            ErrorContent += "</ul>";
        }

        warningInfoUI("<strong>" + Dictionary.Common_Message_FormErrors + "</strong><br />" + ErrorContent, null, 600);
        return false;
    }

    var RReporter = 0;
    if (document.getElementById("RReporterType1").checked) { RReporter = 1; }
    if (document.getElementById("RReporterType2").checked) { RReporter = 2; }
    if (document.getElementById("RReporterType3").checked) { RReporter = 3; }

    var incidentDepartment = { Id: RReporter == 1 ? $("#CmbReporterType1").val() * 1 : 0 };
    var incidentProvider = { Id: RReporter == 2 ? $("#CmbReporterType2").val() * 1 : 0 };
    var incidentCustomer = { Id: RReporter == 3 ? $("#CmbReporterType3").val() * 1 : 0 };

    var incident =
        {
            "Id": IncidentId,
            "CompanyId": Company.Id,
            "Description": $("#TxtDescription").val(),
            "ReporterType": RReporter,
            "Department": incidentDepartment,
            "Provider": incidentProvider,
            "Customer": incidentCustomer,
            "Number": 0,
            "ApplyAction": document.getElementById("RActionYes").checked,
            "WhatHappened": $("#TxtWhatHappened").val(),
            "WhatHappenedBy": { Id: $("#CmbWhatHappenedResponsible").val() },
            "WhatHappenedOn": DateWhatHappened,
            "Causes": $("#TxtCauses").val(),
            "CausesBy": { Id: $("#CmbCausesResponsible").val() },
            "CausesOn": DateCauses,
            "Actions": $("#TxtActions").val(),
            "ActionsBy": { Id: $("#CmbActionsResponsible").val() },
            "ActionsOn": DateActions,
            "ActionsExecuter": { Id: $("#CmbActionsExecuter").val() },
            "ActionsSchedule": null, // ISSUS-74 GetDate($("#TxtActionsSchedule").val(), "-"),
            "Monitoring": $("#TxtMonitoring").val(),
            "ClosedBy": { Id: $("#CmbClosedResponsible").val() },
            "ClosedOn": DateClose,
            "Notes": $("#TxtNotes").val(),
            "Anotations": $("#TxtAnotations").val()
        };

    console.log(incident);

    var webMethod;
    var data;
    if (IncidentId < 1) {
        webMethod = "/Async/IncidentActions.asmx/Insert";
        data = {
            "incident": incident,
            "userId": user.Id
        };
        $.ajax({
            "type": "POST",
            "url": webMethod,
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                // ISSUS-36
                if (incident.ApplyAction) {
                    var action =
                        {
                            "Id": IncidentAction.Id,
                            "CompanyId": Company.Id,
                            "ActionType": 2, // Correctiva
                            "Description": $("#TxtActionDescription").val(),
                            "Origin": 3, // Incidencia
                            "ReporterType": incident.ReporterType,
                            "Department": incident.Department,
                            "Provider": incident.Provider,
                            "Customer": incident.Customer,
                            "Number": 0,
                            "IncidentId": msg.d.MessageError * 1,
                            "WhatHappened": $("#TxtActionWhatHappened").val(),
                            "WhatHappenedBy": { "Id": $("#CmbActionWhatHappenedResponsible").val() },
                            "WhatHappenedOn": GetDate($("#TxtActionWhatHappenedDate").val()),
                            "Causes": $("#TxtActionCauses").val(),
                            "CausesBy": { "Id": $("#CmbActionCausesResponsible").val() },
                            "CausesOn": GetDate($("#TxtActionCausesDate").val()),
                            "Actions": $("#TxtActionActions").val(),
                            "ActionsBy": { "Id": $("#CmbActionActionsResponsible").val() },
                            "ActionsOn": GetDate($("#TxtActionActionsDate").val()),
                            "Monitoring": $("#TxtActionMonitoring").val(),
                            "ClosedBy": { "Id": $("#CmbActionClosedResponsible").val() },
                            "ClosedOn": GetDate($("#TxtActionClosedDate").val()),
                            "Notes": $("#TxtActionNotes").val(),
                            "Active": true
                        };

                    webMethod = "/Async/IncidentActionsActions.asmx/Save";
                    data = {
                        "incidentAction": action,
                        "userId": user.Id
                    };
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
                else {
                    document.location = referrer;
                }
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        //if (incident.ApplyAction) {
        //    var action =
        //    {
        //        "Id": IncidentAction.Id,
        //        "CompanyId": Company.Id,
        //        "ActionType": 2, // Correctiva
        //        "Description": $('#TxtActionDescription').val(),
        //        "Origin": 3, // Incidencia
        //        "ReporterType": incident.ReporterType,
        //        "Department": incident.Department,
        //        "Provider": incident.Provider,
        //        "Customer": incident.Customer,
        //        "Number": 0,
        //        "IncidentId": incident.Id,
        //        "WhatHappend": $('#TxtActionWhatHappend').val(),
        //        "WhatHappendBy": { "Id": $('#CmbActionWhatHappendResponsible').val() },
        //        "WhatHappendOn": GetDate($('#TxtActionWhatHappendDate').val()),
        //        "Causes": $('#TxtActionCauses').val(),
        //        "CausesBy": { "Id": $('#CmbActionCausesResponsible').val() },
        //        "CausesOn": GetDate($('#TxtActionCausesDate').val()),
        //        "Actions": $('#TxtActionActions').val(),
        //        "ActionsBy": { "Id": $('#CmbActionActionsResponsible').val() },
        //        "ActionsOn": GetDate($('#TxtActionActionsDate').val()),
        //        "Monitoring": $('#TxtActionMonitoring').val(),
        //        "ClosedBy": { "Id": $('#CmbActionClosedResponsible').val() },
        //        "ClosedOn": GetDate($('#TxtActionClosedDate').val()),
        //        "Notes": $('#TxtActionNotes').val(),
        //        "Active": true
        //    };

        //    var webMethod = "/Async/IncidentActionsActions.asmx/Save";
        //    var data = { incidentAction: action, userId: user.Id };
        //    $.ajax({
        //        type: "POST",
        //        url: webMethod,
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        data: JSON.stringify(data, null, 2),
        //        success: function (msg) {
        //        },
        //        error: function (msg) {
        //            alertUI(msg.responseText);
        //        }
        //    });
        //}

        webMethod = "/Async/IncidentActions.asmx/Update";
        Incident.WhatHappenedOn = GetDate(FormatYYYYMMDD(Incident.WhatHappenedOn), "-", true);
        Incident.CausesOn = GetDate(FormatYYYYMMDD(Incident.CausesOn), "-", true);
        Incident.ActionsOn = GetDate(FormatYYYYMMDD(Incident.ActionsOn), "-", true);
        Incident.ClosedOn = GetDate(FormatYYYYMMDD(Incident.ClosedOn), "-", true);
        var dataIncident = { newIncident: incident, oldIncident: Incident, userId: user.Id };
        $.ajax({
            "type": "POST",
            "url": webMethod,
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(dataIncident, null, 2),
            "success": function (msg) {
                if (incident.ApplyAction) {
                    var action =
                        {
                            "Id": IncidentAction.Id,
                            "CompanyId": Company.Id,
                            "ActionType": 2, // Correctiva
                            "Description": $("#TxtActionDescription").val(),
                            "Origin": 3, // Incidencia
                            "ReporterType": incident.ReporterType,
                            "Department": incident.Department,
                            "Provider": incident.Provider,
                            "Customer": incident.Customer,
                            "Number": IncidentAction.Number,
                            "IncidentId": incident.Id,
                            "WhatHappened": $("#TxtActionWhatHappened").val(),
                            "WhatHappenedBy": { "Id": $("#CmbActionWhatHappenedResponsible").val() },
                            "WhatHappenedOn": DateActionWhatHappened,
                            "Causes": $("#TxtActionCauses").val(),
                            "CausesBy": { "Id": $("#CmbActionCausesResponsible").val() },
                            "CausesOn": DateActionCauses,
                            "Actions": $("#TxtActionActions").val(),
                            "ActionsBy": { "Id": $("#CmbActionActionsResponsible").val() },
                            "ActionsOn": DateActionActions,
                            "Monitoring": $("#TxtActionMonitoring").val(),
                            "ClosedBy": { "Id": $("#CmbActionClosedResponsible").val() },
                            "ClosedOn": DateActionClose,
                            "Notes": $("#TxtActionNotes").val(),
                            "Active": true
                        };

                    webMethod = "/Async/IncidentActionsActions.asmx/Save";
                    data = {
                        "incidentAction": action,
                        "userId": user.Id
                    };
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
                else {
                    document.location = referrer;
                }
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function RActionChanged() {
    /*if (Incident.Id < 1) {
        warningInfoUI(Dictionary.Item_Incident_Warning_ActionSaveIncident, null, 500)
        return false;
    }*/

    if(document.getElementById("RActionYes").checked===true)
    {
        alertUI(Dictionary.Item_Incident_Warning_ActionTabAvailable, null, 500);

        document.getElementById("Tabaccion").style.display = "";
        if (IncidentAction.Id === 0) {
            // Copiar los datos de la incidencia a la acción
            document.getElementById("TxtActionDescription").value = document.getElementById("TxtDescription").value;
            document.getElementById("TxtActionWhatHappened").value = document.getElementById("TxtWhatHappened").value;
            document.getElementById("CmbActionWhatHappenedResponsible").value = document.getElementById("CmbWhatHappenedResponsible").value;
            document.getElementById("TxtActionWhatHappenedDate").value = document.getElementById("TxtWhatHappenedDate").value;
            document.getElementById("TxtActionCauses").value = document.getElementById("TxtCauses").value;
            document.getElementById("CmbActionCausesResponsible").value = document.getElementById("CmbCausesResponsible").value;
            document.getElementById("TxtActionCausesDate").value = document.getElementById("TxtCausesDate").value;
            // document.getElementById("TxtActionActions").value = document.getElementById("TxtActions").value;
            // document.getElementById("CmbActionActionsResponsible").value = document.getElementById("CmbActionsResponsible").value;
            // document.getElementById("TxtActionActionsDate").value = document.getElementById("TxtActionsDate").value;
            // ISSUS-10 document.getElementById("CmbActionActionsExecuter").value = document.getElementById("CmbActionsExecuter").value;
            // ISSUS-10 document.getElementById("TxtActionActionsSchedule").value = document.getElementById("TxtActionsSchedule").value;

            // Se establece el estado de la acción
            IncidentActionCausesRequired = document.getElementById("TxtActionCauses").value !== "";
            IncidentActionActionsRequired = document.getElementById("TxtActionActions").value !== "";
            // ISSUS-10 IncidentActionClosedRequired = (document.getElementById("CmbActionActionsExecuter").value * 1) > 0;

            // Se lanza el comprobador para poder el required en los campos necesarios
            SetCloseRequired();
        }
    }
    else {
        if (IncidentAction.Id > 0) {
            promptInfoUI(Dictionary.Item_Incident_Message_DeleteAction, 300, RActionChangedNoAccept, RActionChangedNoCancel)
        }
        else {
            RActionChangedNoAccept();
        }
    }
}

function LastRequiredLevel() {
    if ($("#CmbClosedResponsible").val() * 1 > 0 || $("#TxtClosedDate").val() !== "") { return 4; }
    if ($("#TxtActions").val() !== "") { return 3; }
    if ($("#TxtCauses").val() !== "") { return 2; }
    return 1;
}

function LastRequiredLevelAction() {
    if ($("#CmbActionClosedResponsible").val() * 1 > 0 || $("#TxtActionClosedDate").val() !== "") { return 4; }
    if ($("#TxtActionActions").val() !== "") { return 3; }
    if ($("#TxtActionCauses").val() !== "") { return 2; }
    return 1;
}

function RActionChangedNoCancel() {
    document.getElementById("RActionYes").checked = true;
    return false;
}

function RActionChangedNoAccept() {
    document.getElementById("Tabaccion").style.display = "none";
    IncidentAction = {
        "Id": 0,
        "CompanyId": 0,
        "ActionType": 0,
        "Description": "",
        "Origin": 0,
        "ReporterType": 0,
        "Department": { "Id": -1, "Description": "" },
        "Provider": { "Id": -1, "Description": "" },
        "Customer": { "Id": 0, "Description": "" },
        "Number": 0,
        "IncidentId": 0,
        "WhatHappened": "",
        "WhatHappenedBy": null,
        "WhatHappenedOn": null,
        "Causes": "",
        "CausesBy": null,
        "CausesOn": null,
        "Actions": "",
        "ActionsBy": null,
        "ActionsOn": null,
        "Monitoring": "",
        "ClosedBy": null,
        "ClosedOn": null,
        "Notes": "",
        "Active": false
    };
}

function TxtCausesChanged(locked) {
    IncidentCausesRequired = document.getElementById("TxtCauses").value.length > 0;
    if (LastRequiredLevel() > 2 || locked === true) { locked = true; }
    if (document.getElementById("TxtCauses").value.length === 0 && !locked) {
        FieldSetRequired("TxtCausesLabel", Dictionary.Item_Incident_Field_Causes, false);
        FieldSetRequired("CmbCausesResponsibleLabel", Dictionary.Item_Incident_Field_CausesResponsible, false);
        FieldSetRequired("TxtCausesDateLabel", Dictionary.Item_Incident_Field_CausesDate, false);
        document.getElementById("CmbCausesResponsible").value = 0;
        document.getElementById("TxtCausesDate").value = "";
    }
    else {
        FieldSetRequired("TxtCausesLabel", Dictionary.Item_Incident_Field_Causes, true);
        FieldSetRequired("CmbCausesResponsibleLabel", Dictionary.Item_Incident_Field_CausesResponsible, true);
        FieldSetRequired("TxtCausesDateLabel", Dictionary.Item_Incident_Field_CausesDate, true);
        if (document.getElementById("CmbCausesResponsible").value * 1 === 0) { document.getElementById("CmbCausesResponsible").value = ApplicationUser.Employee.Id; }
        if (document.getElementById("TxtCausesDate").value === "") { document.getElementById("TxtCausesDate").value = FormatDate(new Date(), "/");}
    }
}

function TxtActionsChanged(locked) {
    IncidentActionsRequired = document.getElementById("TxtActions").value.length > 0;
    if (LastRequiredLevel() > 3 || locked === true ) { locked = true; }
    if (document.getElementById("TxtActions").value.length === 0 && !locked) {
        FieldSetRequired("TxtActionsLabel", Dictionary.Item_Incident_Field_Actions, false);
        FieldSetRequired("CmbActionsResponsibleLabel", Dictionary.Item_Incident_Field_ActionsResponsible, false);
        FieldSetRequired("TxtActionsDateLabel", Dictionary.Common_DateExecution, false);
        document.getElementById("CmbActionsResponsible").value = 0;
        document.getElementById("TxtActionsDate").value = "";
        TxtCausesChanged(false);
    }
    else {
        FieldSetRequired("TxtActionsLabel", Dictionary.Item_Incident_Field_Actions, true);
        FieldSetRequired("CmbActionsResponsibleLabel", Dictionary.Item_Incident_Field_ActionsResponsible, true);
        FieldSetRequired("TxtActionsDateLabel", Dictionary.Common_DateExecution, true);
        if (document.getElementById("CmbActionsResponsible").value * 1 === 0) { document.getElementById("CmbActionsResponsible").value = ApplicationUser.Employee.Id; }
        if (document.getElementById("TxtActionsDate").value === "") { document.getElementById("TxtActionsDate").value = FormatDate(new Date(), "/"); }
        TxtCausesChanged(true);
    }
}

function CmbClosedResponsibleChanged()
{
    var value = document.getElementById("CmbClosedResponsible").value * 1;
    IncidentClosedRequired = value !== 0;
    FieldSetRequired("CmbClosedResponsibleLabel", Dictionary.Item_Incident_Field_CloseResponsible, IncidentClosedRequired);
    FieldSetRequired("TxtClosedDateLabel", Dictionary.Item_Incident_Field_CloseDate, IncidentClosedRequired);

    if (IncidentClosedRequired === false) {
        document.getElementById("CmbClosedResponsible").value = 0;
        document.getElementById("TxtClosedDate").value = "";
    }
    else {
        if (document.getElementById("CmbClosedResponsible").value * 1 === 0) { document.getElementById("CmbClosedResponsible").value = ApplicationUser.Employee.Id; }
        if (document.getElementById("TxtClosedDate").value === "") { document.getElementById("TxtClosedDate").value = FormatDate(new Date, "/"); }
    }
    TxtActionsChanged(IncidentClosedRequired);
}

// Control wizard de la acción
function TxtActionWhatHappenedChanged(locked) {
    locked = true;
    if (document.getElementById("TxtActionWhatHappened").value.length === 0 && !locked) {
        FieldSetRequired("TxtActionWhatHappenedLabel", Dictionary.Item_IncidentAction_Field_WhatHappened, false);
        FieldSetRequired("CmbActionWhatHappenedResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, false);
        FieldSetRequired("TxtActionWhatHappenedDateLabel", Dictionary.Item_IncidentAction_Field_Date, false);
        document.getElementById("CmbActionWhatHappenedResponsible").value = 0;
    }
    else {
        FieldSetRequired("TxtActionWhatHappenedLabel", Dictionary.Item_IncidentAction_Field_WhatHappened, true);
        FieldSetRequired("CmbActionWhatHappenedResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, true);
        FieldSetRequired("TxtActionWhatHappenedDateLabel", Dictionary.Item_IncidentAction_Field_Date, true);
        if (document.getElementById("CmbActionWhatHappenedResponsible").value * 1 === 0) { document.getElementById("CmbActionWhatHappenedResponsible").value = ApplicationUser.Employee.Id; }
    }
}

function TxtActionCausesChanged(locked) {
    IncidentCausesRequired = document.getElementById("TxtCauses").value.length > 0;
    if (LastRequiredLevelAction() > 2 || locked === true) { locked = true; }
    if (document.getElementById("TxtActionCauses").value.length === 0 && !locked) {
        FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, false);
        FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, false);
        FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Item_IncidentAction_Field_Date, false);
        document.getElementById("CmbActionCausesResponsible").value = 0;
        IncidentActionCausesRequired = false;
        TxtActionWhatHappenedChanged(false);
    }
    else {
        FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, true);
        FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, true);
        FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Item_IncidentAction_Field_Date, true);
        if (document.getElementById("CmbActionCausesResponsible").value * 1 === 0) { document.getElementById("CmbActionCausesResponsible").value = ApplicationUser.Employee.Id; }
        IncidentActionCausesRequired = true;
        TxtActionWhatHappenedChanged(true);
    }
}

function TxtActionActionsChanged(locked) {
    IncidentActionActionsRequired = document.getElementById("TxtActions").value.length > 0;
    if (LastRequiredLevelAction() > 3 || locked === true) { locked = true; }
    if (document.getElementById("TxtActionActions").value.length === 0 && !locked) {
        FieldSetRequired("TxtActionActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, false);
        FieldSetRequired("CmbActionActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, false);
        FieldSetRequired("TxtActionActionsDateLabel", Dictionary.Item_IncidentAction_Field_Date, false);
        // ISSUS-10 FieldSetRequired("CmbActionActionsExecuterLabel", Dictionary.Item_IncidentAction_Field_Executer, false);
        // ISSUS-10 FieldSetRequired("TxtActionActionsScheduleLabel", Dictionary.Item_IncidentAction_Field_Schelude, false);
        document.getElementById("CmbActionActionsResponsible").value = 0;
        IncidentActionActionsRequired = false;
        TxtActionCausesChanged(false);
    }
    else {
        FieldSetRequired("TxtActionActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, true);
        FieldSetRequired("CmbActionActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, true);
        FieldSetRequired("TxtActionActionsDateLabel", Dictionary.Item_IncidentAction_Field_Date, true);
        // ISSUS-10 FieldSetRequired("CmbActionActionsExecuterLabel", Dictionary.Item_IncidentAction_Field_Executer, true);
        // ISSUS-10 FieldSetRequired("TxtActionActionsScheduleLabel", Dictionary.Item_IncidentAction_Field_Schelude, true);
        if (document.getElementById("CmbActionActionsResponsible").value * 1 === 0) { document.getElementById("CmbActionActionsResponsible").value = ApplicationUser.Employee.Id; }
        IncidentActionActionsRequired = true;
        TxtActionCausesChanged(true);
    }
}

function SetCloseRequired() {
    IncidentActionClosedRequired = false;
    if (document.getElementById("CmbActionClosedResponsible").value * 1 !== 0) { IncidentActionClosedRequired = true; }
    else if (document.getElementById("TxtActionClosedDate").value !== "") { IncidentActionClosedRequired = true; }

    if (IncidentActionClosedRequired === true) {
        FieldSetRequired("CmbActionClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, true);
        FieldSetRequired("TxtActionClosedDateLabel", Dictionary.Common_Date, true);
        if (document.getElementById("CmbActionClosedResponsible").value === 0) {
            document.getElementById("CmbClosedResponsible").value = ApplicationUser.Employee.Id;
        }
    }
    else {
        FieldSetRequired("CmbActionClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_Responsible, false);
        FieldSetRequired("TxtActionClosedDateLabel", Dictionary.Common_Date, false);
    }

    TxtActionActionsChanged(IncidentActionClosedRequired);
}

if (ApplicationUser.Grants.Incident.Write === false) {
    document.getElementById("RActionYes").disabled = true;
    document.getElementById("RActionNo").disabled = true;
    document.getElementById("RReporterType1").disabled = true;
    document.getElementById("RReporterType2").disabled = true;
    document.getElementById("RReporterType3").disabled = true; 
    document.getElementById("CmbReporterType1").disabled = true;
    document.getElementById("CmbReporterType2").disabled = true;
    document.getElementById("CmbReporterType3").disabled = true;

    // Desactivar datePicker
    document.getElementById("TxtWhatHappenedDate").className = "form-control";
    document.getElementById("TxtCausesDate").className = "form-control";
    document.getElementById("TxtActionsDate").className = "form-control";
    document.getElementById("TxtClosedDate").className = "form-control";

    $("#CmbReporterType1Bar").hide();
    $("#CmbReporterType2Bar").hide();
    $("#CmbReporterType3Bar").hide();
    $("#BtnSave").hide();
    $("#BtnNewCost").hide();

    $("#BtnNewUploadfile").hide();
    $("#UploadFilesContainer .btn-danger").hide();
    $("#UploadFilesList .btn-danger").hide();
}
else {
    // ISSUS-190
    document.getElementById('TxtDescription').focus();
}

if (ApplicationUser.Grants.IncidentActions.Write === false) {
    if (document.getElementById("TxtActionWhatHappened") !== null) {
        document.getElementById("TxtActionWhatHappened").disabled = true;
        document.getElementById("TxtActionWhatHappenedDate").disabled = true;
        document.getElementById("TxtActionWhatHappenedDateBtn").style.display = "none";
        document.getElementById("TxtActionCauses").disabled = true;
        document.getElementById("TxtActionCausesDate").disabled = true;
        document.getElementById("TxtActionCausesDateBtn").style.display = "none";
        document.getElementById("TxtActionActions").disabled = true;
        document.getElementById("TxtActionActionsDate").disabled = true;
        document.getElementById("TxtActionActionsDateBtn").style.display = "none";
        document.getElementById("TxtActionMonitoring").disabled = true;
        document.getElementById("TxtActionClosedDate").disabled = true;
        document.getElementById("TxtActionClosedDateBtn").style.display = "none";
        document.getElementById("TxtActionNotes").disabled = true;
        $("#BtnSaveAction").hide();
    }
}

function Resize() {
    var listTable = document.getElementById("ListDataDiv");
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 450) + "px";
}

window.onload = function () {
    Resize();

    if (Incident.ClosedOn !== null) {
        $("input").attr("disabled", "disabled");
        $("select").attr("disabled", "disabled");
        $("textarea").attr("disabled", "disabled");
        $("#BtnNewCost").hide();
        $("#BtnNewUploadfile").hide();
        $(".icon-trash").parent().hide();
        $(".icon-edit").parent().hide();

        $("#CmbClosedResponsible").removeAttr("disabled");
        $("#TxtClosedDate").removeAttr("disabled");
        //$("#TxtNotes").removeAttr("disabled");

        $("#Chk1").removeAttr("disabled");
        $("#Chk2").removeAttr("disabled");
    }

    $("#menuoption-12 a").show();
    $("#menuoption-13 a").show();
}

window.onresize = function () { Resize(); }

$("#CmbActionsResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionsResponsible").val() * 1, Employees); });
$("#CmbClosedResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbClosedResponsible").val() * 1, Employees); });
$("#CmbActionActionsResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionActionsResponsible").val() * 1, Employees); });
$("#CmbActionClosedResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionClosedResponsible").val() * 1, Employees); });

$("#BtnAnular").hide();
$("#BtnRestaurar").hide();
if (Incident.ClosedOn === null) {
    $("#BtnAnular").show();
} else {
    $("#BtnRestaurar").show();
    AnulateLayout();
}

$("#BtnRestaurarAction").hide();
$("#BtnAnularAction").hide();
$("#BtnRestaurarAction").hide();
if (IncidentAction.ClosedOn === null) {
    $("#BtnAnularAction").show();
} else {
    $("#BtnRestaurarAction").show();
    AnulateLayoutAccion();
}

$("#BtnAnular").on("click", AnularPopup);
$("#BtnRestaurar").on("click", Restore);
$("#BtnAnularAction").on("click", AnularPopupAccion);
$("#BtnRestaurarAction").on("click", RestoreAccion);

function AnularPopup() {
    //FieldSetRequired("CmbClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
    //FieldSetRequired("TxtClosedDateLabel", Dictionary.Common_Date, true);

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
        "title": Dictionary.Item_Incident_PopupAnular_Title,
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

var anulationData = null;
function AnularConfirmed() {
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

    if (ok === false) { return false; }

    var data = {
        "incidentId": Incident.Id,
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
        "url": "/Async/IncidentActions.asmx/Anulate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            SaveIncident();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AnulateLayout() {
    $("#BtnRestaurar").hide();
    if (Incident.ClosedOn !== null) {
        $("#DivAnulateMessage").remove();
        var message = "<div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_Incident_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_Incident_Field_CloseResponsible + ": <strong>" + Incident.ClosedBy.Value + "</strong><br />";
        message += "        " + Dictionary.Item_Incident_Field_CloseDate + ": <strong>" + $("#TxtClosedDate").val() + "</strong><br />";
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
        "incidentId": Incident.Id,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: "/Async/IncidentActions.asmx/Restore",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            Incident.EndDate = null;
            document.location = document.location + "";
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

//---- anular acciones


function AnularPopupAccion() {
    var ok = true;
    if ($("#TxtActionDescription").val() === "") { ok = false; }
    if ($("#TxtActionWhatHappened").val() === "") { ok = false; }
    if ($("#TxtActionCauses").val() === "") { ok = false; }
    if ($("#TxtActionActions").val() === "") { ok = false; }
    if ($("#TxtActionWhatHappenedDate").val() === "") { ok = false; }
    if ($("#TxtActionCausesDate").val() === "") { ok = false; }
    if ($("#TxtActionActionsDate").val() === "") { ok = false; }
    if ($("#CmbActionWhatHappenedResponsible").val() * 1 < 1) { ok = false; }
    if ($("#CmbActionCausesResponsible").val() * 1 < 1) { ok = false; }
    if ($("#CmbActionActionsResponsible").val() * 1 < 1) { ok = false; }

    if (ok === false) {
        alertUI("Revise los campos obligatorios");
        return false;
    }

    $("#CmbActionClosedResponsibleLabel").html(Dictionary.Item_IncidentAction_Field_ResponsibleClose + "<span style=\"color:#f00;\">*</span>");
    $("#TxtActionClosedDateLabel").html(Dictionary.Item_IncidentAction_Field_Date + "<span style=\"color:#f00;\">*</span>");
    $("#CmbActionClosedResponsibleLabel").removeClass("control-label");
    $("#CmbActionClosedResponsibleLabel").removeClass("no-padding-right");
    $("#TxtActionClosedDate").val(FormatDate(new Date(), "/"));
    $("#CmbActionClosedResponsible").val(user.Employee.Id);
    var dialog = $("#dialogAnularAccion").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_IncidentAction_PopupAnular_Title,
        "width": 400,
        "buttons":
        [
            {
                "id": "BtnAnularSaveAccion",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_IncidentAction_Btn_Anular,
                "class": "btn btn-success btn-xs",
                "click": function () { AnularConfirmedAccion(); }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

var anulationDataAccion = null;
function AnularConfirmedAccion() {
    document.getElementById("TxtActionClosedDateLabel").style.color = "#000";
    document.getElementById("CmbActionClosedResponsibleLabel").style.color = "#000";
    document.getElementById("TxtActionClosedDateDateRequired").style.display = "none";
    document.getElementById("TxtActionClosedDateDateMalformed").style.display = "none";
    document.getElementById("CmbActionClosedResponsibleErrorRequired").style.display = "none";

    var ok = true;

    if ($("#TxtActionClosedDate").val() === "") {
        ok = false;
        document.getElementById("TxtActionClosedDateLabel").style.color = "#f00";
        document.getElementById("TxtActionClosedDateDateRequired").style.display = "";
    }
    else {
        if (validateDate($("#TxtActionClosedDate").val()) === false) {
            ok = false;
            $("#TxtActionClosedDateLabel").css("color", "#f00");
            $("#TxtActionClosedDateDateMalformed").show();
        }
    }

    if ($("#CmbActionClosedResponsible").val() * 1 < 1) {
        ok = false;
        document.getElementById("CmbActionClosedResponsibleLabel").style.color = "#f00";
        document.getElementById("CmbActionClosedResponsibleErrorRequired").style.display = "";
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "incidentActionId": IncidentAction.Id,
        "companyId": Company.Id,
        "responsible": $("#CmbActionClosedResponsible").val() * 1,
        "date": GetDate($("#TxtActionClosedDate").val(), "/"),
        "applicationUserId": user.Id
    };
    anulationDataAccion = data;
    $("#dialogAnularAccion").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/Anulate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            SaveIncidentAction();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AnulateLayoutAccion() {
    $("#BtnRestaurarAccion").hide();
    if (IncidentAction.ClosedOn !== null) {
        $("#DivAnulateMessageAccion").remove();
        var message = "<div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessageAccion\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_IncidentAction_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_Incident_Field_CloseResponsible + ": <strong>" + IncidentAction.ClosedBy.Value + "</strong><br />";
        message += "        " + Dictionary.Item_Incident_Field_CloseDate + ": <strong>" + $("#TxtActionClosedDate").val() + "</strong><br />";
        message += "    </p>";
        $("#AccionAnulada").html(message);
        $("#BtnAnularAction").hide();
        $("#BtnRestaurarAction").show();
    }
    else {
        $("#DivAnulateMessageAccion").hide();
        $("#BtnAnularAction").show();
        $("#BtnRestaurarAction").hide();
    }
}

function RestoreAccion() {
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
            IncidentAction.ClosedOn = null;
            AnulateLayoutAccion();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function SaveIncidentAction() {
    var action =
        {
            "Id": IncidentAction.Id,
            "CompanyId": Company.Id,
            "ActionType": 2, // Correctiva
            "Description": $("#TxtActionDescription").val(),
            "Origin": 3, // Incidencia
            "ReporterType": IncidentAction.ReporterType,
            "Department": IncidentAction.Department,
            "Provider": IncidentAction.Provider,
            "Customer": IncidentAction.Customer,
            "Number": 0,
            "IncidentId": Incident.Id,
            "WhatHappened": $("#TxtActionWhatHappened").val(),
            "WhatHappenedBy": { "Id": $("#CmbActionWhatHappenedResponsible").val() },
            "WhatHappenedOn": GetDate($("#TxtActionWhatHappenedDate").val(), "/", false),
            "Causes": $("#TxtActionCauses").val(),
            "CausesBy": { "Id": $("#CmbActionCausesResponsible").val() },
            "CausesOn": GetDate($("#TxtActionCausesDate").val(), "/", false),
            "Actions": $("#TxtActionActions").val(),
            "ActionsBy": { "Id": $("#CmbActionActionsResponsible").val() },
            "ActionsOn": GetDate($("#TxtActionActionsDate").val(), "/", false),
            "Monitoring": $("#TxtActionMonitoring").val(),
            "ClosedBy": { "Id": $("#CmbActionClosedResponsible").val() },
            "ClosedOn": GetDate($("#TxtActionClosedDate").val(), "/", false),
            "Notes": $("#TxtActionNotes").val(),
            "Active": true
        };

    data = {
        "incidentAction": action,
        "userId": user.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/Save",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}