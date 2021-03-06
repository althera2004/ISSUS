﻿var SaveAction = false;
var CausesRequired = false;
var ActionsRequired = false;
var ClosedRequired = false;
var MinStepValue = 1;
var SlidersActive = true;
var CostBlocked = false;
var anulationData = null;
var reloadAfterAnulate = false;

jQuery(function ($) {
    FillCmbRules();
    // Posibilidad de poner HTML en los títulos de los popup
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
    $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

    // Botón de lanzamiento de popup de normas
    $("#BtnSelectRules").on("click", function (e) {
        e.preventDefault();
        RulesRenderPopup();
        $("#dialogRules").removeClass("hide").dialog({
            "resizable": false,
            "modal": true,
            "title": Dictionary.Item_Oportunity_SelectRuleType,
            "title_html": true,
            "width": 800,
            "buttons":
            [
                {
                    "id": "BtnNewRuleSave",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        RulesInsert();
                    }
                },
                {
                    "id": "BtnNewRuleCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
        });
    });

    $("#Result").prop("readonly", true);
    $("#Code").prop("disabled", true);

    //Things to do if the Oportunity already exists
    if (Oportunity.Id > 0) {
        var cost = GetCostByCode(Oportunity.Cost);
        var impact = GetImpactByCode(Oportunity.Impact);
        $("#CmbProcess").val(Oportunity.Process.Id);

        if (Oportunity.Cost > 0 && Oportunity.Impact > 0) {
            MinStepValue = 1;
        }

        RenderStepsSliders();

        $("#CmbRules").val(Oportunity.Rule.Id);
        $("#CmbProcess").val(Oportunity.Process.Id);

        IncidentActionCostRenderTable("IncidentActionCostsTableData");

        if (typeof user.Grants.Oportunity === "undefined" || user.Grants.Oportunity.Write === false) {
            $("#costes .btn-info").hide();
            $(".btn-danger").hide();
        }
    }
    //Things to do if the BusinessRisk doesn't exist
    else {
        // ISSUS - 233
        $("#Result").val("-");
        $("#Code").val("");
        $("#InitialValue").val(0);
        RenderStepsSliders();
    }

    $("#BtnSave").on("click", SaveBtnPressed);
    $("#BtnCancel").on("click", Cancel);

    $("#TxtActionCauses").on("keyup", SetCloseRequired);
    $("#TxtActionCauses").bind("paste", SetCloseRequired);
    $("#TxtActionActions").on("keyup", SetCloseRequired);
    $("#TxtActionActions").bind("paste", SetCloseRequired);

    $("#BtnNewCost").on("click", function (e) {
        e.preventDefault();
        ShowNewCostPopup(0);
    });

    $("#BtnCostBAR").on("click", function (e) {
        e.preventDefault();
        ShowCostBarPopup($("#TxtCostDescription"));
    });
});

function CompareRules(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) {
        return -1;
    }

    if (a.Description.toUpperCase() > b.Description.toUpperCase()) {
        return 1;
    }

    return 0;
}

function UpdateStartLabels() {
    var cost = Oportunity.Cost;
    var impact = Oportunity.Impact;

    var costName = Dictionary.Common_NotSet_Female;
    var impactName = Dictionary.Common_NotSet_Female;

    // Poner el nombre de la probabilidad/severidad
    for (var x = 0; x < CostImpactList.length; x++) {
        if (CostImpactList[x].Type === 0 && CostImpactList[x].Code === cost) {
            costName = CostImpactList[x].Description;
        }

        if (CostImpactList[x].Type === 1 && CostImpactList[x].Code === impact) {
            impactName = CostImpactList[x].Description;
        }
    }

    $("#TxtStartCostLabel").html(Dictionary.Item_Oportunity_LabelField_Cost + ":&nbsp;<strong>" + costName + "</strong>");
    $("#TxtStartImpactLabel").html(Dictionary.Item_Oportunity_LabelField_Impact + ":&nbsp;<strong>" + impactName + "</strong>");
}

function UpdateResult() {
    //console.log("UpdateResult");
    //Si hay una acción indica que la pestaña inicial está bloqueada
    if (Action.Id > 0) { UpdateStartLabels(); }

    var cost = Oportunity.Cost;
    var impact = Oportunity.Impact;

    var rangeColor = ["#777", "#d40", "#ffb752", "#fd3", "#4aa4ce", "#4aa4ce"];
    if (document.getElementById("ImpactRange") !== null) {
        if (document.getElementById("ImpactRange").nextElementSibling !== null) {
            document.getElementById("ImpactRange").nextElementSibling.style.backgroundColor = rangeColor[impact];
        }
    }

    if (document.getElementById("CostRange") !== null) {
        if (document.getElementById("CostRange").nextElementSibling !== null) {
            document.getElementById("CostRange").nextElementSibling.style.backgroundColor = rangeColor[cost];
        }
    }

    // ISSUS - 233
    var calculatedResult = cost * impact;
    if (calculatedResult > 0) {
        $("#Result").html(calculatedResult);
        $("#Result").css("color", calculatedResult < rule.Limit ? "#3f3" : "#4aa4ce");
    }
    else {
        $("#Result").html("-");
        $("#Result").css("color", "#fff");
    }
}

function UpdateFinalResult() {
    var cost = Oportunity.FinalCost;
    var impact = Oportunity.FinalImpact;

    var rangeColor = ["#777", "#d40", "#ffb752", "#fd3", "#4aa4ce", "#4aa4ce"];
    if (document.getElementById("FinalImpactRange") !== null) {
        if (document.getElementById("FinalImpactRange").nextElementSibling !== null) {
            document.getElementById("FinalImpactRange").nextElementSibling.style.backgroundColor = rangeColor[impact];
        }
    }

    if (document.getElementById("FinalCostRange") !== null) {
        if (document.getElementById("FinalCostRange").nextElementSibling !== null) {
            document.getElementById("FinalCostRange").nextElementSibling.style.backgroundColor = rangeColor[cost];
        }
    }

    var calculatedResult = cost * impact;
    if (calculatedResult > 0) {
        $("#FinalResult").html(calculatedResult);
        $("#FinalResult").css("color", calculatedResult < rule.Limit ? "#4aa4ce" : "#3f3");
    }
    else {
        $("#FinalResult").html("-");
        $("#FinalResult").css("color", "#fff");
    }
}

function ApplyActionRadio() {
    // Para elegir las acciones hay que tener evaluado el riesgo
    if (Oportunity.Cost === 0 || Oportunity.Impact === 0 || Oportunity.Cost === null || Oportunity.Impact === null) {
        alertUI(Dictionary.Item_Oportunity_ErrorMessage_ResultRequired);
        document.getElementById("ApplyActionYes").checked = false;
        document.getElementById("ApplyActionNo").checked = false;
        return false;
    }

    if (document.getElementById("ApplyActionYes").checked === true) {
        if (user.Grants.IncidentActions.Write !== true) {
            alertUI(Dictionary.Item_IncidentAction_Message_NoGrants, null, 500);
            $("#ApplyActionYes").removeAttr("checked");
            return false;
        }

        ApplyActionTrue();
    }
    else {
        ApplyActionFalse();
    }
}

function ApplyActionTrue() {
    //Show action, cost and final status tabs and content
    $("#Tabaccion").show();

    if (Action.Id > 0) {
        $("#Tabcostes").show();
    }

    // Sólo se muestra la pestaña Final si no es una oportunidad nueva
    if (OportunityId > 0) {
        $("#Tabgraphic").show();

        //Disable information editing on oportunity
        $("#TxtDescription").prop("disabled", true);
        $("#DateStart").attr("disabled", true);
        $("#CmbRules").attr("disabled", true);
        $("#CmbRules").css("background-color", "#eee");
        $("#BtnSelectRules").hide();
        $("#CmbProcess").attr("disabled", true);
        $("#CmbProcess").css("background-color", "#eee");
    }

    $("#ApplyAction2").attr("disabled", true);

    SlidersActive = false;
    SaveAction = true;
    SetCloseRequired();
    alertUI(Dictionary.Item_Oportunity_Warning_ActionTabAvailable, null, 600);

    //Apply value to the final status tab
    $("#Initial-input-span-slider-cost").slider({ "value": Oportunity.Cost });
    $("#Initial-input-span-slider-impact").slider({ "value": Oportunity.Impact });
    $("#Result").val(Oportunity.Result);

    SaveAction = true;
    if (Action.Description === "") {
        $("#TxtActionDescription").val($("#TxtDescription").val());
    }

    if (Action.WhatHappened === "") {
        $("#TxtActionWhatHappened").val($("#TxtItemDescription").val());
    }

    if (Action.Causes === "") {
        $("#TxtActionCauses").val($("#TxtCauses").val());
    }

    if (Action.WhatHappenedBy.Id < 0) {
        $("#CmbActionWhatHappenedResponsible").val(ApplicationUser.Employee.Id);
    }

    if (Action.WhatHappenedOn === null) {
        $("#TxtActionWhatHappenedDate").val(FormatDate(new Date(), "/"));
    }

    if ($("#TxtCauses").val() !== "") {
        if (Action.CausesBy.Id < 0) {
            $("#CmbActionCausesResponsible").val(ApplicationUser.Employee.Id);
        }

        if (Action.CausesOn === null) {
            $("#TxtActionCausesDate").val(FormatDate(new Date(), "/"));
        }
    }
    else {
        $("#CmbActionCausesResponsible").val(0);
        $("#TxtActionCausesDate").val("");
    }

    if (IncidentAction.Id < 0) {
        $("#CmbActionActionsResponsible").val(0);
        $("#TxtActionActionsDate").val("");
    }

    var rangeColor = ["#777", "#4aa4ce", "#4aa4ce", "#fd3", "#ffb752", "#d40"];
    if (document.getElementById("InitialImpactRange") !== null) {
        $("#InitialImpactRange").hide();
        if (document.getElementById("InitialImpactRange").nextElementSibling !== null) {
            document.getElementById("InitialImpactRange").nextElementSibling.style.backgroundColor = rangeColor[Oportunity.Impact];
        }
    }

    if (document.getElementById("InitialCostRange") !== null) {
        $("#InitialCostRange").hide();
        if (document.getElementById("InitialCostRange").nextElementSibling !== null) {
            document.getElementById("InitialCostRange").nextElementSibling.style.backgroundColor = rangeColor[Oportunity.Cost];
        }
    }
}

function ApplyActionFalse() {
    //Show action, cost and final status tabs and content
    $("#Tabaccion").hide();
    $("#Tabcostes").hide();
    $("#Tabgraphic").hide();

    //Enable information editing on the oportunity
    $("#TxtDescription").prop("disabled");
    $("#DateStart").removeAttr("disabled");
    $("#CmbRules").removeAttr("disabled");
    $("#BtnSelectRules").show();
    $("#CmbProcess").removeAttr("disabled");
    $("#ApplyAction2").removeAttr("disabled");
    $("#Assumed").removeAttr("disabled");
    $("#input-span-slider-probability").slider("enable");
    $("#input-span-slider-severity").slider("enable");
    SlidersActive = true;
    SaveAction = false;
}

function OportunityInsert(previousId) {
    // 1.- Modificar en la BBDD
    var id = 0;
    UpdateResult();

    var startAction = 0;
    if (document.getElementById("ApplyActionNo").checked === true) { startAction = 2; }
    if (document.getElementById("ApplyActionYes").checked === true) { startAction = 3; }

    var result = 0;
    if ($("#Result").val() !== "" && $("#Result").val() !== "-") {
        result = $("#Result").val() * 1;
    }

    var data = {
        "oportunity": {
            "Active": true,
            "AnulateBy": { "Id": -1 },
            "AnulateDate": null,
            "AnulateReason": "",
            "ApplyAction": document.getElementById("ApplyActionYes").checked,
            "Causes": $("#TxtCauses").val(),
            "Control": $("#TxtControl").val(),
            "Cost": Oportunity.Cost,
            "CreatedBy": { Id: -1 },
            "CreatedOn": GetDate(Oportunity.CreatedOn, "/", false),
            "DateStart": GetDate($("#DateStart").val(), "/", false),
            "Description": $("#TxtDescription").val(),
            "Id": Oportunity.Id,
            "CompanyId": Company.Id,
            "Impact": Oportunity.Impact,
            "ItemDescription": $("#TxtItemDescription").val(),
            "ModifiedBy": { "Id": -1 },
            "ModifiedOn": GetDate(Oportunity.CreatedOn, "/", false),
            "Notes": $("#TxtNotes").val(),
            "Process": { "Id": $("#CmbProcess").val() * 1 },
            "Result": Oportunity.Impact * Oportunity.Cost,
            "Rule": { "Id": $("#CmbRules").val() * 1 }
        },
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/OportunityActions.asmx/Insert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }

            if (response.d.Success === true) {
                var newOportunity = Oportunity.Id < 1;
                Oportunity.Id = response.d.MessageError * 1;
                if (SaveAction === true) {
                    console.log("risk + action");
                    SaveIncidentAction(Oportunity.Id, true);
                }
                else {
                    alertInfoUI(Dictionary.Item_Oportunity_Message_InsertSucess, Reload);
                }
            }
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    return false;
}

function OportunityUpdate(sender) {
    // 1.- Modificar en la BBDD
    var startAction = 0;
    if (document.getElementById("ApplyActionNo").checked === true) { startAction = 2; }
    if (document.getElementById("ApplyActionYes").checked === true) { startAction = 3; }

    SaveAction = document.getElementById("ApplyActionYes").checked

    var finalApplyAction = null;
    if (document.getElementById("FinalApplyActionYes").checked === true) {
        finalApplyAction = true;
    }
    else {
        if (document.getElementById("FinalApplyActionNo").checked === true) {
            finalApplyAction = false;
        }
    }

    var data = {
        "oportunity": {
            "Active": true,
            "Code": Oportunity.Code,
            "AnulateBy": { "Id": -1 },
            "AnulateDate": null,
            "AnulateReason": "",
            "ApplyAction": SaveAction,
            "Causes": $("#TxtCauses").val(),
            "Control": $("#TxtControl").val(),
            "Cost": Oportunity.Cost,
            "CreatedBy": { Id: -1 },
            "CreatedOn": GetDate(Oportunity.CreatedOn, "/", false),
            "DateStart": GetDate($("#DateStart").val(), "/", false),
            "Description": $("#TxtDescription").val(),
            "Id": Oportunity.Id,
            "CompanyId": Company.Id,
            "Impact": Oportunity.Impact,
            "ItemDescription": $("#TxtItemDescription").val(),
            "ModifiedBy": { "Id": -1 },
            "ModifiedOn": GetDate(Oportunity.CreatedOn, "/", false),
            "Notes": $("#TxtNotes").val(),
            "Process": { "Id": $("#CmbProcess").val() * 1 },
            "Result": Oportunity.Impact * Oportunity.Cost,
            "Rule": { "Id": $("#CmbRules").val() * 1 },
            "FinalDate": GetDate($("#TxtFinalDate").val(), "/", true),
            "FinalCost": Oportunity.FinalCost,
            "FinalImpact": Oportunity.FinalImpact,
            "FinalResult": Oportunity.FinalCost * Oportunity.FinalImpact,
            "FinalApplyAction": finalApplyAction
        },
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };

    console.log(data);

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/OportunityActions.asmx/Update",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
                if (SaveAction === true) {
                    SaveIncidentAction(Oportunity.Id, false);
                }
                else {
                    document.location = "BusinessRisksList.aspx";
                }
            }
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    return false;
}

function SaveIncidentAction(OportunityId, reload) {
    reloadAfterAnulate = reload;
    var actionsBy = $("#CmbActionActionsResponsible").val();
    if (actionsBy === null) { actionsBy = -1;}
    var causesBy = $("#CmbActionCausesResponsible").val();
    if (causesBy === null) { causesBy = -1; }

    var data =
        {
            "incidentAction": {
                "Id": Action.Id,
                "CompanyId": Company.Id,
                "ActionType": 3, // Preventiva
                "Description": $("#TxtActionDescription").val(),
                "Origin": 6, // Oportunity
                "ReporterType": 1,
                "Department": Action.Department,
                "Provider": Action.Provider,
                "Customer": Action.Customer,
                "Number": 0,
                "BusinessRiskId": -1,
                "Oportunity": { "Id": OportunityId, "Description": "" },
                "IncidentId": -1,
                "WhatHappened": $("#TxtActionWhatHappened").val(),
                "WhatHappenedBy": { "Id": $("#CmbActionWhatHappenedResponsible").val() },
                "WhatHappenedOn": GetDate($("#TxtActionWhatHappenedDate").val(), "/", false),
                "Causes": $("#TxtActionCauses").val(),
                "CausesBy": { "Id": causesBy },
                "CausesOn": GetDate($("#TxtActionCausesDate").val(), "/", false),
                "Actions": $("#TxtActionActions").val(),
                "ActionsBy": { "Id": actionsBy },
                "ActionsOn": GetDate($("#TxtActionActionsDate").val(), "/", false),
                "Monitoring": $("#TxtActionMonitoring").val(),
                "ClosedBy": { "Id": $("#CmbActionClosedResponsible").val() },
                "ClosedOn": GetDate($("#TxtActionClosedDate").val(), "/", false),
                "Notes": $("#TxtActionNotes").val(),
                "Active": true
            },
            "userId": ApplicationUser.Id
        };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/Save",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (reloadAfterAnulate === true) {
                Reload();
            }
            else {
                document.location = "BusinessRisksList.aspx";
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function SaveBtnPressed() {
    if (ValidateData() === true) {
        if (Oportunity.Id === -1) {
            OportunityInsert(-1);
        }
        else {
            console.log("saveaction", SaveAction);
            OportunityUpdate();
        }
    }
}

function Cancel() {
    document.location = referrer;
}

function Reload() {
    document.location = "OportunityView.aspx?id=" + Oportunity.Id;
}

function SetCloseRequired() {
    console.log("SetCloseRequired");
    var targetId = null;
    var pastedValue = null;
    if (typeof e !== "undefined" && e !== null && e.type === "paste") {
        targetId = e.originalEvent.target.id;
        pastedValue = e.originalEvent.clipboardData.getData('Text');
    }
    /////////////////////////////////
    //Set required fields for risks//
    /////////////////////////////////
    FieldSetRequired("TxtRulesLabel", Dictionary.Item_Oportunity_LabelField_Rules, true);
    FieldSetRequired("TxtProcessLabel", Dictionary.Item_Oportunity_LabelField_Process, true);
    FieldSetRequired("TxtDateStartLabel", Dictionary.Item_Oportunity_LabelField_DateStart, true);
    FieldSetRequired("TxtResultLabel", Dictionary.Item_Oportunity_LabelField_Result, true);
    FieldSetRequired("TxtNameLabel", Dictionary.Item_Oportunity_LabelField_Name, true);

    ///////////////////////////////////
    //Set required fields for actions//
    ///////////////////////////////////
    FieldSetRequired("TxtActionDescriptionLabel", Dictionary.Item_IncidentAction_Label_Description, true);
    FieldSetRequired("TxtActionWhatHappenedLabel", Dictionary.Item_IncidentAction_Field_WhatHappened, true);
    FieldSetRequired("CmbActionWhatHappenedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleWhatHappend, true);
    FieldSetRequired("TxtActionWhatHappenedDateLabel", Dictionary.Common_Date, true);

    //Checking if Causes is required
    if ($("#TxtActionCauses").val().trim() !== "" || (targetId === "TxtActionCauses" && pastedValue !== null)) {
        CausesRequired = true;
    }
    else {
        CausesRequired = false;
    }

    //Checking if Actions is required
    if ($("#TxtActionActions").val().trim() !== "" || (targetId === "TxtActionActions" && pastedValue !== null)) {
        ActionsRequired = true;
    }
    else {
        ActionsRequired = false;
    }

    if (ActionsRequired === true) {
        if ($("#CmbActionActionsResponsible").val() * 1 === 0) {
            $("#CmbActionActionsResponsible").val(ApplicationUser.Employee.Id);
        }

        if ($("#TxtActionActionsDate").val().trim() === "") {
            $("#TxtActionActionsDate").val(FormatDate(new Date(), "/"));
        }

        CausesRequired = true;
    }
    else {
        $("#CmbActionActionsResponsible").val("");
        $("#TxtActionActionsDate").val("");
    }

    if (CausesRequired === true) {
        if ($("#CmbActionCausesResponsible").val() * 1 === 0) {
            $("#CmbActionCausesResponsible").val(ApplicationUser.Employee.Id);
        }

        if ($("#TxtActionCausesDate").val().trim() === "") {
            $("#TxtActionCausesDate").val(FormatDate(new Date(), "/"));
        }
    }
    else {
        $("#CmbActionCausesResponsible").val("");
        $("#TxtActionCausesDate").val("");
    }

    FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, CausesRequired);
    FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, CausesRequired);
    FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Common_Date, CausesRequired);

    FieldSetRequired("TxtActionActionsLabel", Dictionary.Item_IncidentAction_Field_Actions, ActionsRequired);
    FieldSetRequired("CmbActionActionsResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleActions, ActionsRequired);
    FieldSetRequired("TxtActionActionsDateLabel", Dictionary.Common_Date, ActionsRequired);

    //Checking if Closed is required
    if ($("#CmbActionClosedResponsible").val() * 1 !== 0) {
        FieldSetRequired("CmbActionClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
        FieldSetRequired("TxtActionClosedDateLabel", Dictionary.Common_Date, true);
        ClosedRequired = true;
    }
    else if ($("#TxtActionClosedDate").val() !== "") {
        FieldSetRequired("CmbActionClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
        FieldSetRequired("TxtActionClosedDateLabel", Dictionary.Common_Date, true);
        ClosedRequired = true;
    }
    else {
        FieldSetRequired("CmbActionClosedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleClose, false);
        FieldSetRequired("TxtActionClosedDateLabel", Dictionary.Common_Date, false);
        ClosedRequired = false;
    }
}

function ValidateData() {
    console.log("ValidateData");
    var ok = true;
    var ErrorMessageInicial = new Array();
    var ErrorMessageAccion = new Array();
    var ErrorMessageFinal = new Array();

    ClearFieldTextMessages("TxtRules");
    ClearFieldTextMessages("TxtProcess");
    ClearFieldTextMessages("TxtProbability");
    ClearFieldTextMessages("TxtSeverity");
    ClearFieldTextMessages("TxtName");
    ClearFieldTextMessages("TxtDateStart");
    ClearFieldTextMessages("TxtResult");
    ClearFieldTextMessages("TxtInitialValue");
    $("#DateStartDateOutOfDate").hide();
    $("#DateStartDateUpToLimit").hide();

    // Es obligatorio indicar la norma
    if ($("#CmbRules").val() * 1 === 0) {
        ErrorMessageInicial.push(Dictionary.Item_Oportunity_ErrorMessage_RulesRequired);
        SetFieldTextMessages("TxtRules");
        ok = false;
    }

    // Es obligatorio indicar el proceso
    if ($("#CmbProcess").val() * 1 === 0) {
        ErrorMessageInicial.push(Dictionary.Item_Oportunity_ErrorMessage_ProcessRequired);
        SetFieldTextMessages('TxtProcess');
        ok = false;
    }

    // Es obligatorio indicar el nombre del riesgo
    if ($("#TxtDescription").val() === "") {
        ErrorMessageInicial.push(Dictionary.Item_Oportunity_ErrorMessage_NameRequired);
        SetFieldTextMessages("TxtDescription");
        ok = false;
    }

    // Es obligatorio indicar la fecha de inicio del riesgo
    if ($("#DateStart").val() === "") {
        ErrorMessageInicial.push(Dictionary.Item_Oportunity_ErrorMessage_DateStartRequired);
        SetFieldTextMessages("TxtDateStart");
        ok = false;
    } else {
        var data = GetDate(document.getElementById("DateStart").value, "/", false);

        // El inicio del riesgo no puede ser superior a hoy
        if (data > new Date()) {
            ErrorMessageInicial.push(Dictionary.Item_Oportunity_ErrorMessage_DateUpToLimit);
            document.getElementById("TxtDateStartLabel").style.color = "#f00";
            $("#DateStartDateUpToLimit").show();
            ok = false;
        }
    }

    // Si hay acción, se aplican las misma validaciones que en el resto de acciones de la aplicación
    console.log("SaveAction", SaveAction);
    if (SaveAction === true) {
        var okDates = true;
        var dateWhatHappened = null;
        var dateCauses = null;
        var dateActions = null;
        var dateActionsExecution = null;
        var dateClose = null;
        var dateCloseExecution = null;

        ClearFieldTextMessages("TxtActionDescription");
        ClearFieldTextMessages("TxtActionWhatHappened");
        ClearFieldTextMessages("TxtActionWhatHappenedResponsible");
        ClearFieldTextMessages("TxtActionWhatHappenedDate");

        // La descripción de la acción es obligatoria
        if ($("#TxtActionDescription").val() === "") {
            ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_DescriptionRequired);
            SetFieldTextMessages("TxtActionDescription");
            ok = false;
        }

        // ¿Qué ha pasado? es obligatorio
        //--------------------------------------------------------------------------
        if ($("#TxtActionWhatHappened").val() === "") {
            ok = false;
            ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequired);
            SetFieldTextMessages("TxtActionWhatHappened");
        }

        if (document.getElementById("CmbActionWhatHappenedResponsible").value * 1 === 0) {
            ok = false;
            ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequiredResponsible);
            SetFieldTextMessages("CmbActionWhatHappenedResponsible");
        }

        if ($("#TxtActionWhatHappenedDate").val() === "") {
            ok = false;
            ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequiredDate);
            SetFieldTextMessages("TxtActionWhatHappenedDate");
        }
        else {
            if (!RequiredDateValue("TxtActionWhatHappenedDate")) {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_Error_DateMalformed_WhatHappened);
            }
            else {
                dateWhatHappened = GetDate($("#TxtActionWhatHappenedDate").val(), "/", false);
                var startDate = GetDate($("#DateStart").val(), "/", false);
                if (startDate > dateWhatHappened) {
                    $("#TxtActionWhatHappenedDateLabel").css("color", Color.Error);
                    ErrorMessageAccion.push(Dictionary.Item_BusinessRisk_ErrorMessage_ActionVerDate);
                    ok = false;
                }
            }
        }
        //--------------------------------------------------------------------------

        if (CausesRequired === true) {
            ClearFieldTextMessages("TxtActionCauses");
            ClearFieldTextMessages("TxtActionCausesResponsible");
            ClearFieldTextMessages("TxtActionCausesDate");
            if ($("#TxtActionCauses").val() === "") {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequired);
                SetFieldTextMessages("TxtActionCauses");

            }
            if ($("#CmbActionCausesResponsible").val() * 1 === 0) {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredResponsible);
                SetFieldTextMessages("CmbActionCausesResponsible");
            }

            if ($("#TxtActionCausesDate").val() === "") {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredDate);
                SetFieldTextMessages("TxtActionCausesDate");
            }
            else {
                if (!RequiredDateValue("TxtActionCausesDate")) {
                    ok = false;
                    ErrorMessageAccion.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Causes);
                }
                else {
                    dateCauses = GetDate($("#TxtActionCausesDate").val(), "/", false);
                }
            }
        }

        if (ActionsRequired === true) {
            ClearFieldTextMessages("TxtActionActions");
            ClearFieldTextMessages("TxtActionActionsResponsible");
            ClearFieldTextMessages("TxtActionActionsDate");
            if ($("#TxtActionActions").val() === "") {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequired);
                SetFieldTextMessages("TxtActionActions");
            }

            if ($("#CmbActionActionsResponsible").val() * 1 === 0) {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredResponsible);
                SetFieldTextMessages("CmbActionActionsResponsible");
            }

            if ($("#TxtActionActionsDate").val() === "") {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredDate);
                SetFieldTextMessages("TxtActionActionsDate");
            }
            else {
                if (!RequiredDateValue("TxtActionActionsDate")) {
                    ok = false;
                    ErrorMessageAccion.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Actions);
                }
                else {
                    dateActions = GetDate($("#TxtActionActionsDate").val(), "/", false);
                }
            }
        }

        if (ClosedRequired === true) {
            ClearFieldTextMessages("TxtActionClosedDate");
            ClearFieldTextMessages("TxtActionClosedResponsible");
            if (document.getElementById("CmbActionClosedResponsible").value * 1 === 0) {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseExecutor);
                SetFieldTextMessages("TxtActionClosedDate");
            }
            if (document.getElementById("TxtActionClosedDate").value === "") {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseExecutorDate);
                SetFieldTextMessages("TxtActionClosedDate");
            }
            else {
                if (!RequiredDateValue("TxtActionClosedDate")) {
                    ok = false;
                    ErrorMessageAccion.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Actions);
                }
                else {
                    dateClose = GetDate($("#TxtActionClosedDate").val(), "/", false);
                }
            }
        }

        // Sincronizacion de fechas
        if (dateWhatHappened !== null) {
            if (dateCauses !== null && dateWhatHappened > dateCauses) {
                okDates = false;
                SetFieldTextMessages("TxtActionWhatHappenedDate");
                SetFieldTextMessages("TxtActionCausesDate");
            }
            if (dateActions !== null && dateWhatHappened > dateActions) {
                okDates = false;
                SetFieldTextMessages("TxtActionWhatHappenedDate");
                SetFieldTextMessages("TxtActionActionsDate");
            }
            if (dateClose !== null && dateWhatHappened > dateClose) {
                okDates = false;
                SetFieldTextMessages("TxtActionWhatHappenedDate");
                SetFieldTextMessages("TxtActionClosedDate");
            }
        }

        if (dateCauses !== null) {
            if (dateActions !== null && dateCauses > dateActions) {
                okDates = false;
                SetFieldTextMessages("TxtActionActionsDate");
                SetFieldTextMessages("TxtActionCausesDate");
            }
            if (dateClose !== null && dateCauses > dateClose) {
                okDates = false;
                SetFieldTextMessages("TxtActionCausesDate");
                SetFieldTextMessages("TxtActionClosedDate");
            }
        }

        if (dateActions !== null) {
            if (dateClose !== null && dateActions > dateClose) {
                okDates = false;
                SetFieldTextMessages("TxtActionCausesDate");
                SetFieldTextMessages("TxtActionClosedDate");
            }
        }

        if (okDates === false) {
            ok = false;
            ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_UntemporalyDates);
        }
    }

    if (ok === false) {
        var ErrorContent = "";
        if (ErrorMessageInicial.length > 0) {
            ErrorContent += "<strong>" + Dictionary.Item_Oportunity_Tab_Basic + "</strong><ul>";
            for (var x = 0; x < ErrorMessageInicial.length; x++) {
                ErrorContent += "<li>" + ErrorMessageInicial[x] + "</li>";
            }

            ErrorContent += "</ul>";
        }

        if (ErrorMessageAccion.length > 0) {
            ErrorContent += "<strong>" + Dictionary.Item_Oportunity_Tab_Action + "</strong><ul>";
            for (var y = 0; y < ErrorMessageAccion.length; y++) {
                ErrorContent += "<li>" + ErrorMessageAccion[y] + "</li>";
            }

            ErrorContent += "</ul>";
        }

        warningInfoUI("<strong>" + Dictionary.Common_Message_FormErrors + "</strong><br />" + ErrorContent, null, 600);
        return false;
    }
    else {
        return true;
    }
}

function SetImpact(code, object) {
    $("#ImpactDataContainer").val(code);
    var num = document.getElementById("SelectableImpact").childNodes.length;
    for (var x = 0; x < num; x++) {
        document.getElementById("SelectableImpact").childNodes[x].style.fontWeight = "normal";
    }

    object.parentNode.parentNode.style.fontWeight = "bold";
}

function SetCost(code, object) {
    $("#CostDataContainer").val(code);
    var num = document.getElementById("SelectableCost").childNodes.length;
    for (var x = 0; x < num; x++) {
        document.getElementById("SelectableCost").childNodes[x].style.fontWeight = "normal";
    }
    object.parentNode.parentNode.style.fontWeight = "bold";
}

function GetCostByCode(code) {
    for (var x = 0; x < CostImpactList.length; x++) {
        var item = CostImpactList[x];
        if (item.Type === 0) {
            if (item.Code === code) {
                return item;
            }
        }
    }

    return null;
}

function GetImpactByCode(code) {
    for (var x = 0; x < CostImpactList.length; x++) {
        var item = CostImpactList[x];
        if (item.Type === 1) {
            if (item.Code === code) {
                return item;
            }
        }
    }

    return null;
}

function CmbResponsibleFill() {
    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true && Employees[x].DisabledDate === null) {
            var option = document.createElement("OPTION");
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            document.getElementById("CmdIncidentActionCostResponsible").appendChild(option);
        }
    }
}

if (Oportunity.Id > 0) {
    $("#DateStart").val(Oportunity.DateStart);
}
else {
    $("#DateStart").val(FormatDate(new Date(), "/"));
}

$("#DateStart").datepicker({ "maxDate": "0" });
$("#DateClose").datepicker({
    "minDate": GetDate(Oportunity.DateStart, "/", false)
});
$(".date-picker").on("blur", function () { DatePickerChanged(this); });

CmbResponsibleFill();

// Problemas de comillas en el nombre
if (Oportunity.Id > 0) {
    $("#Name").val(Oportunity.Description);
}

function ActionsDialog(sender) {
    var id = sender.parentNode.parentNode.id * 1;
    var dialog = $("#dialogActionDetails").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Action_Popup_Details_Title,
        "title_html": true,
        "width": 800
    });

    var actualAction = null;
    for (var x = 0; x < IncidentActionHistory.length; x++) {
        if (IncidentActionHistory[x].Id === id) {
            actualAction = IncidentActionHistory[x];
            break;
        }
    }

    if (actualAction !== null) {
        $("#TxtActionDescriptionView").val(actualAction.Description);
        $("#TxtActionWhatHappenedView").val(actualAction.WhatHappened);
        $("#TxtActionWhatHappenedByView").val(actualAction.WhatHappenedBy.Value);
        $("#TxtActionWhatHappenedOnView").val(FormatYYYYMMDD(actualAction.WhatHappenedOn, "/"));
        $("#TxtActionCausesView").val(actualAction.Causes);
        $("#TxtActionCausesByView").val(actualAction.CausesBy.Value);
        $("#TxtActionCausesOnView").val(FormatYYYYMMDD(actualAction.CausesOn, "/"));
        $("#TxtActionActionsView").val(actualAction.Actions);
        $("#TxtActionActionsByView").val(actualAction.ActionsBy.Value);
        $("#TxtActionActionsOnView").val(FormatYYYYMMDD(actualAction.ActionsOn, "/"));
        $("#TxtActionMonitoringView").val(actualAction.Monitoring);
        $("#TxtActionClosedByView").val(actualAction.ClosedBy.Value);
        $("#TxtActionClosedOnView").val(FormatYYYYMMDD(actualAction.ClosedOn, "/"));
        $("#TxtActionNotesView").val(actualAction.Notes);
    }
}

/*
function RenderStartSliders() {
    //console.log("RenderStartSliders");
    

    VoidTable("stepsCost");
    VoidTable("stepsImpact");

    for (var x = MinStepValue; x < 6; x++) {
        var span = document.createElement("span");
        span.id = x;
        span.className = 'tick';
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement("BR"));
        span.appendChild(document.createTextNode('|'));
        span.style.left = ((100 / (5 - MinStepValue)) * (x - MinStepValue)) + "%";
        switch (x) {
            case 1:
                span.title = Dictionary.Item_Oportunity_Tooltip_Cost_1;
                break;
            case 2:
                span.title = Dictionary.Item_Oportunity_Tooltip_Cost_2;
                break;
            case 3:
                span.title = Dictionary.Item_Oportunity_Tooltip_Cost_3;
                break;
            case 4:
                span.title = Dictionary.Item_Oportunity_Tooltip_Cost_4;
                break;
            case 5:
                span.title = Dictionary.Item_Oportunity_Tooltip_Cost_5;
                break;
        }
        document.getElementById('stepsCost').appendChild(span);
        if (Action.Id < 1) {
            span.onclick = function () {
                if (SlidersActive) {
                    $("#input-span-slider-cost").slider({ 'value': this.id });
                    Oportunity.Cost = this.id * 1;
                    UpdateResult();
                }
            };
            span.style.cursor = "default";
        }
    }

    for (var x2 = MinStepValue; x2 < 6; x2++) {
        var spanStep = document.createElement('span');
        spanStep.id = x2;
        spanStep.className = 'tick';
        spanStep.appendChild(document.createTextNode(x2));
        spanStep.appendChild(document.createElement('BR'));
        spanStep.appendChild(document.createTextNode('|'));
        spanStep.style.left = ((100 / (5 - MinStepValue)) * (x2 - MinStepValue)) + '%';
        switch (x2) {
            case 1:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_1;
                break;
            case 2:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_2;
                break;
            case 3:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_3;
                break;
            case 4:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_4;
                break;
            case 5:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_5;
                break;
        }
        document.getElementById('stepsImpact').appendChild(spanStep);
        if (Action.Id < 1) {
            spanStep.onclick = function () {
                if (SlidersActive) {
                    $("#input-span-slider-impact").slider({ value: this.id });
                    Oportunity.Impact = this.id * 1;
                    UpdateResult();
                }
            };
            span.style.cursor = 'default';
        }
    }
    if (Oportunity.Id > 0) {
        UpdateResult();
    }
}
*/
function RenderStepsSliders() {
    //RenderStartSliders();

    var MinStepValue = 1;

    // Sliders situación inicial
    $("#input-span-slider-cost").slider({
        "value": Oportunity.Cost,
        "range": "min",
        "min": MinStepValue,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) {
            if (Action.Id > 0) {
                return false;
            }
            var val = parseInt(ui.value);
            if (val === 0) {
                return false;
            }
            $("#input-span-slider-cost").slider({ "value": this.id });
            Oportunity.Cost = val;
            UpdateResult();
            return null;
        }
    });

    $("#input-span-slider-impact").slider({
        "value": Oportunity.Impact,
        "range": "min",
        "min": MinStepValue,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) {
            if (Action.Id > 0) { return false; }
            var val = parseInt(ui.value);
            if (val === 0) {
                return false;
            }
            $("#input-span-slider-impact").slider({ "value": this.id });
            Oportunity.Impact = val;
            UpdateResult();
            return null;
        }
    });

    // Sliders inicio de situación final
    $("#Initial-input-span-slider-cost").slider({
        "value": Oportunity.Cost,
        "range": "min",
        "min": MinStepValue,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) { return null; }
    });

    $("#Initial-input-span-slider-impact").slider({
        "value": Oportunity.Impact,
        "range": "min",
        "min": MinStepValue,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) { return null; }
    });

    // Sliders situación final
    $("#Final-input-span-slider-cost").slider({
        "value": Oportunity.FinalCost,
        "range": "min",
        "min": 1,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) {
            var val = parseInt(ui.value);
            if (val > 0) {
                $("#inputFinal-input-span-slider-cost").slider({ "value": this.id });
                Oportunity.FinalCost = val;
                UpdateFinalResult();
            }
            return null;
        }
    });

    $("#Final-input-span-slider-impact").slider({
        "value": Oportunity.FinalImpact,
        "range": "min",
        "min": 1,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) {
            var val = parseInt(ui.value);
            if (val > 0) {
                $("#Final-input-span-slider-impact").slider({ "value": this.id });
                Oportunity.FinalImpact = val;
                UpdateFinalResult();
            }
            return null;
        }
    });

    RenderCostSlider("stepsCost");
    RenderCostSlider("InitialStepsCost");
    RenderCostSlider("FinalStepsCost");

    RenderImpactSlider("stepsImpact");
    RenderImpactSlider("InitialStepsImpact");
    RenderImpactSlider("FinalStepsImpact");

    // Los slider de la situación inicial en la pestaña final se eliminan
    $("#InitialCostRange").remove();
    $("#InitialImpactRange").remove();

    $("#FinalIPR").html($("#IPR").html());

    UpdateResult();
    UpdateFinalResult();

    if (Oportunity.FinalApplyAction === null) {
        $("#TxtFinalDate").attr("disabled", "disabled");
    }

    if (Oportunity.FinalApplyAction === true) {
        document.getElementById("FinalApplyActionYes").checked = true;
    }

    if (Oportunity.FinalApplyAction === false) {
        document.getElementById("FinalApplyActionNo").checked = true;
    }

    if (Oportunity.FinalDate !== null) {
        $("#FinalApplyActionYes").attr("disabled", "disabled");
        $("#FinalApplyActionNo").attr("disabled", "disabled");
        $("#TxtFinalDate").attr("disabled", "disabled");
    }
}

function RenderCostSlider(contentName) {
    VoidTable(contentName);
    for (var x2 = 1; x2 < 6; x2++) {
        var spanStep = document.createElement("span");
        spanStep.id = x2;
        spanStep.className = "tick";
        spanStep.appendChild(document.createTextNode(x2));
        spanStep.appendChild(document.createElement("BR"));
        spanStep.appendChild(document.createTextNode("|"));
        spanStep.style.left = ((100 / (5 - MinStepValue)) * (x2 - MinStepValue)) + "%";
        switch (x2) {
            case 1:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Cost_1;
                break;
            case 2:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Cost_2;
                break;
            case 3:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Cost_3;
                break;
            case 4:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Cost_4;
                break;
            case 5:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Cost_5;
                break;
        }

        document.getElementById(contentName).appendChild(spanStep);
    }
}

function RenderImpactSlider(contentName) {
    VoidTable(contentName);
    for (var x2 = 1; x2 < 6; x2++) {
        var spanStep = document.createElement("span");
        spanStep.id = x2;
        spanStep.className = "tick";
        spanStep.appendChild(document.createTextNode(x2));
        spanStep.appendChild(document.createElement("BR"));
        spanStep.appendChild(document.createTextNode("|"));
        spanStep.style.left = ((100 / (5 - MinStepValue)) * (x2 - MinStepValue)) + "%";
        switch (x2) {
            case 1:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_1;
                break;
            case 2:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_2;
                break;
            case 3:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_3;
                break;
            case 4:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_4;
                break;
            case 5:
                spanStep.title = Dictionary.Item_Oportunity_Tooltip_Impact_5;
                break;
        }

        document.getElementById(contentName).appendChild(spanStep);
    }
}

function syncFields(target, source) {
    document.getElementById(target).value = document.getElementById(source).value;
}

if (Oportunity.Result > 0) {
    if (Oportunity.ApplyAction === true) {
        document.getElementById("ApplyActionYes").checked = true;
    }
    else {

        document.getElementById("ApplyActionNo").checked = true;
    }
}

// No se puede cerrar el riesgo si la acción no está cerrada
if (Action.ClosedOn === null) {
    $("#DivClosingRisk").hide();
    $("#DivClosingRiskUnavailable").show();
}
else {
    $("#DivClosingRisk").show();
    $("#DivClosingRiskUnavailable").hide();
}

if (Action.Id > 0) {
    document.getElementById("ApplyActionYes").disabled = true;
    document.getElementById("ApplyActionNo").disabled = true;
}

// Controles iniciales
if (Oportunity.StartAction === 2) {
    document.getElementById("ApplyActionNo").checked = true;
    $("#Tabgraphic").hide();
}
if (Oportunity.StartAction === 3) {
    document.getElementById("ApplyActionYes").checked = true;
}

if (ApplicationUser.Grants.Rules !== null) {
    if (ApplicationUser.Grants.Rules.Write === false) {
        $("#BtnSelectRules").hide();
    }
}

if (typeof ApplicationUser.Grants.BusinessRisk === "undefined" || ApplicationUser.Grants.BusinessRisk.Write === false) {
    $("input").attr("disabled", true);
    $("select").attr("disabled", true);
    $("textarea").attr("disabled", true);
    $(".ui-slider-handle").hide();
    $("#BtnSave").hide();
    $("#BtnNewCost").hide();
    $("#BtnNewUploadfile").hide();
    $("select").css("background-color", "#eee");
}


function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 450);
}

window.onload = function () {
    $("#nav-search").remove();
    $("#TxtActionWhatHappenedDateDateMalformed").after("<span class=\"ErrorMessage\" id=\"TxtActionWhatHappenedDateOverDate\" style=\"display: none;\">" + Dictionary.Item_BusinessRisk_ErrorMessage_ActionVerDate + "</span>");


    $("#Tabhome").on("click", function () { $("#BtnAnular").hide(); $("#BtnCancel").show(); $("#oldFormFooter").show(); });

    $("#Tabaccion").on("click", function () {
        if (Action.Id > 0) { $("#BtnAnular").show(); $("#BtnCancel").show(); }
        else {
            $("#BtnCancel").hide();
        }
        $("#oldFormFooter").show();
    });
    $("#Tabcostes").on("click", function () { $("#BtnCancel").hide(); $("#BtnAnular").hide(); $("#oldFormFooter").show(); });
    $("#TabuploadFiles").on("click", function () { $("#BtnCancel").hide(); $("#BtnAnular").hide(); $("#oldFormFooter").hide(); });

    if (Oportunity.Id > 0) {
        $("#BtnPrint").on("click", PrintData);
    }
    else {
        $("#BtnPrint").hide();
    }

    if (Oportunity.ApplyAction === false) {
        $("#Tabaccion").hide();
        $("#Tabcostes").hide();
        if (Oportunity.Cost !== 0 && Oportunity.Cost !== null && Oportunity.Impact !== 0 && Oportunity.Impact !== null) {
            document.getElementById("ApplyActionNo").checked = true;
        }
    }
    else {
        SaveAction = true;
        $("#Tabaccion").show();
        $("#Tabcostes").show();
        document.getElementById("ApplyActionYes").checked = true;

        if (OportunityId > 0) {
            $("#Tabgraphic").show();

            //Disable information editing on oportunity
            $("#TxtDescription").prop("disabled", true);
            $("#DateStart").attr("disabled", true);
            $("#CmbRules").attr("disabled", true);
            $("#CmbRules").css("background-color", "#eee");
            $("#BtnSelectRules").hide();
            $("#CmbProcess").attr("disabled", true);
            $("#CmbProcess").css("background-color", "#eee");

            $("#CostRange").hide();
            $("#ImpactRange").hide();
        }

        $("#ApplyAction2").attr("disabled", true);

        SlidersActive = false;
        SaveAction = true;

        //Apply value to the final status tab
        $("#Initial-input-span-slider-cost").slider({ "value": Oportunity.Cost });
        $("#Initial-input-span-slider-impact").slider({ "value": Oportunity.Impact });
        $("#Result").val(Oportunity.Result);
    }

    Resize();

    document.getElementById("TxtDescription").focus();
    $("#Tabhome").on("click", HideAnulateActionButton);
    $("#Tabaccion").on("click", ShowAnulateActionButton);
    $("#Tabcostes").on("click", HideAnulateActionButton);
    $("#Tabgraphic").on("click", function () {
        $("#BtnAnular").hide();
        $("#BtnRestaurar").hide();
        $("#BtnCancel").show();
    });
    $("#TabhistoryActions").on("click", HideAnulateActionButton);
    $("#TabuploadFiles").on("click", HideAnulateActionButton);

    var res = "&nbsp;<button class=\"btn btn-danger\" type=\"button\" id=\"BtnAnular\" style=\"display:inline-block;\"><i class=\"icon-ban-circle bigger-110\"></i>" + Dictionary.Item_BusinessRisk_Button_CloseAction + "</button>";
    res += "&nbsp;<button class=\"btn btn-primary\" type=\"button\" id=\"BtnRestaurar\" style=\"display:inline-block;\"><i class=\"icon-undo bigger-110\"></i>" + Dictionary.Item_BusinessRisk_Button_RestoreAction + "</button>";

    $("#ItemButtons").prepend(res);

    $("#BtnAnular").on("click", AnularPopup);
    $("#BtnRestaurar").on("click", Restore);
    $("#TxtActionClosedDateDateMalformed").after("<span class=\"ErrorMessage\" id=\"TxtActionClosedDateErrorCross\" style=\"display:none;\">" + Dictionary.Item_BusinessRisk_ErrorMessage_ClosedRequiredDataOutTime + "</span>")
    $("#Tabhome").click();
    $("#CmbActionActionsResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionActionsResponsible").val() * 1, Employees, this); });

    // ------------- Anular accion
    $("#BtnAnular").hide();
    $("#BtnRestaurar").hide();
    if (Action.ClosedOn === null && Action.Id > 0) { $("#BtnAnular").show(); }
    else { $("#BtnRestaurar").show(); AnulateLayout(); }

    $("#BtnAnular").hide();
    $("#BtnRestaurar").hide();

    SetCloseRequired();

    if (typeof Action === "undefined") { CostBlocked = true; }
    else if (typeof Action.Id === "undefined") { CostBlocked = true; }
    else if (Action.Id === null) { CostBlocked = true; }
    else if (Action.Id < 1) { CostBlocked = true; }

    if (CostBlocked === true) {
        $("#BtnNewCost").hide();
        $("#scrollTableDiv").hide();
    }
    else {
        $("#DivPrimaryUser").hide();
    }

    if (typeof user.Grants.IncidentActions === "undefined" || user.Grants.IncidentActions.Read === false) {
        var resAccion = "";
        resAccion += "<div class=\"alert alert-danger\">";
        resAccion += "<strong> <i class=\"icon-warning-sign fa-2x\"></i></strong >";
        resAccion += "<h3 style=\"display:inline;\">" + Dictionary.Common_Message_ItemNoAccess + "</h3>";
        resAccion += "</div>";
        $("#accion").html(resAccion);
        $("#costes").html(resAccion);
        $("#historyActions").html(resAccion);
        $("#BtnAnular").remove();
        $("#BtnRestaurar").remove();
    }
};

window.onresize = function () { Resize(); };

function AnularPopup() {
    var ok = true;
    if ($("#TxtActionDescription").val() === "") { ok = false; }
    if ($("#TxtActionWhatHappened").val() === "") { ok = false; }
    if ($("#TxtAcionCauses").val() === "") { ok = false; }
    if ($("#TxtActionActions").val() === "") { ok = false; }
    if ($("#TxtActionWhatHappenedDate").val() === "") { ok = false; }
    if ($("#TxtActionCausesDate").val() === "") { ok = false; }
    if ($("#TxtActionActionsDate").val() === "") { ok = false; }
    if ($("#CmbActionWhatHappenedResponsible").val() * 1 < 1) { ok = false; }
    if ($("#CmbActionCausesResponsible").val() * 1 < 1) { ok = false; }
    if ($("#CmbActionActionsResponsible").val() * 1 < 1) { ok = false; }

    document.getElementById("CmbActionClosedResponsibleLabel").style.color = "#000";
    document.getElementById("TxtActionClosedDateLabel").style.color = "#000";
    $("#CmbActionClosedResponsibleErrorRequired").hide();
    $("#TxtActionClosedDateDateMalformed").hide();
    $("#TxtActionClosedDateDateRequired").hide();
    $("#TxtActionClosedDateErrorCross").hide();

    if (ok === false) {
        alertUI(Dictionary.Common_Form_CheckError);
        return false;
    }

    $("#CmbActionClosedResponsible").val(ApplicationUser.Employee.Id);

    $("#CmbActionClosedResponsibleLabel").html(Dictionary.Item_IncidentAction_Field_ResponsibleClose + "<span style=\"color:#f00;\">*</span>");
    $("#TxtActionClosedDateLabel").html(Dictionary.Item_IncidentAction_Field_Date + "<span style=\"color:#f00;\">*</span>");
    $("#CmbActionClosedResponsibleLabel").removeClass("control-label");
    $("#CmbActionClosedResponsibleLabel").removeClass("no-padding-right");
    $("#TxtActionClosedDate").val(FormatDate(new Date(), "/"));
    $("#CmbActionClosedResponsible").val(user.Employee.Id);
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
    console.log("AnularConfirmed");
    $("#TxtActionClosedDateLabel").css("color", "#000");
    $("#CmbActionClosedResponsibleLabel").css("color", "#000");
    $("#TxtActionClosedDateDateRequired").hide();
    $("#TxtActionClosedDateDateMalformed").hide();
    $("#CmbActionClosedResponsibleErrorRequired").hide();
    $("#TxtActionClosedDateErrorCross").hide();

    var ok = true;

    if ($("#TxtActionClosedDate").val() === "") {
        ok = false;
        $("#TxtActionClosedDateLabel").css("color", Color.Error);
        $("#TxtActionClosedDateDateRequired").show();
    }
    else {
        if (validateDate($("#TxtActionClosedDate").val()) === false) {
            ok = false;
            $("#TxtActionClosedDateLabel").css("color", Color.Error);
            $("#TxtActionClosedDateDateMalformed").show();
        }
        else {
            var actionsDate = GetDate($("#TxtActionActionsDate").val(), "/", false);
            var closeDate = GetDate($("#TxtActionClosedDate").val(), "/", false);
            if (closeDate < actionsDate) {
                ok = false;
                $("#TxtActionClosedDateErrorCross").show();
                $("#TxtActionClosedDateLabel").css("color", Color.Error);
            }
        }
    }

    if ($("#CmbActionClosedResponsible").val() * 1 < 1) {
        ok = false;
        document.getElementById("CmbActionClosedResponsibleLabel").style.color = "#f00";
        $("#CmbActionClosedResponsibleErrorRequired").show();
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "incidentActionId": Action.Id,
        "companyId": Company.Id,
        "responsible": $("#CmbActionClosedResponsible").val() * 1,
        "date": GetDate($("#TxtActionClosedDate").val(), "/"),
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
            SaveIncidentAction(Oportunity.Id, true);
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AnulateLayout() {
    if (typeof user.Grants.IncidentActions === "undefined" || user.Grants.IncidentActions.Read === false) {
        $("#DivAnulateMessage").hide();
        $("#BtnRestaurar").hide();
        $("#DivAnulateMessage").remove();
        return;
    }

    $("#BtnRestaurar").hide();
    $("#DivAnulateMessage").remove();
    if (Action.ClosedOn !== null) {
        $("#BtnNewCost").hide();
        var message = "<br /><div class=\"alert alert-info\" style=\"display:block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_IncidentAction_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndDate + ": <strong>" + GetDateYYYYMMDDText(Action.ClosedOn, "/", false) + "</strong><br />";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndResponsible + ": <strong>" + Action.ClosedBy.Value + "</strong>";
        message += "    </p>";
        message += "</div>";
        $("#accion").append(message);
        $("#BtnAnular").hide();
        $("#BtnRestaurar").show();
        $("#BtnSave").hide();
        $("#accion input").attr("disabled", "disabled");
        $("#accion select").attr("disabled", "disabled");
        $("#accion textarea").attr("disabled", "disabled");
    }
    else {
        $("#BtnNewCost").show();
        $("#DivAnulateMessage").hide();
        $("#accion input").removeAttr("disabled");
        $("#accion select").removeAttr("disabled");
        $("#accion textarea").removeAttr("disabled");
        if (Action.Id > 0) {
            $("#BtnAnular").show();
        }
    }

    $("#BtnSave").show();
}

function Restore() {
    var data = {
        "incidentActionId": Action.Id,
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
            Action.ClosedOn = null;
            AnulateLayout();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function HideAnulateActionButton() {
    $("#BtnAnular").hide();
    $("#BtnRestaurar").hide();
}

function ShowAnulateActionButton() {
    if (Action.ClosedOn === null && Action.Id > 0) {
        $("#BtnAnular").show();
    } else {
        $("#BtnRestaurar").show();
        AnulateLayout();
    }
}

function FinalApplyActionRadio() {
    // Para elegir las acciones hay que tener evaluada oportunidad
    if (Oportunity.FinalCost === null || Oportunity.FinalImpact === null) {
        alertUI(Dictionary.Item_Oportunity_ErrorMessage_ResultRequired);
        document.getElementById("FinalApplyActionYes").checked = false;
        document.getElementById("FinalApplyActionNo").checked = false;
        document.getElementById("TxtFinalDate").disabled = true;
        document.getElementById("TxtFinalDateBtn").disabled = true;
        return false;
    }

    document.getElementById("TxtFinalDate").disabled = false;
    document.getElementById("TxtFinalDateBtn").disabled = false;
}

function PrintData() {
    window.open("/export/PrintOportunityData.aspx?id=" + Oportunity.Id + "&companyId=" + Company.Id);
    return false;
}