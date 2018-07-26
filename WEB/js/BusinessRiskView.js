var SaveAction = false;
var ApplyActionFinal = false;
var AssumedFinal = false;
var CausesRequired = false;
var ActionsRequired = false;
var ClosedRequired = false;
var MinStepValue = 1;
var MinStepValueFinalProbability = 1;
var MinStepValueFinalSeverity = 1;
var SlidersStartActive = true;
var reloadAfterSave = false;

$("#Tabhome").on("click", function () { $("#BtnCancel").show();$("#oldFormFooter").show(); });
$("#Tabaccion").on("click", function () {
    $("#oldFormFooter").show();
    if (Action.Id < 0) {
        $("#BtnCancel").hide();
    }
});
$("#Tabcostes").on("click", function () {
    $("#oldFormFooter").show();
    if (Action.Id < 0) {
        $("#BtnCancel").hide();
    }
});
$("#Tabgraphic").on("click", function () { $("#oldFormFooter").show(); $("#oldFormFooter").show(); });
$("#TabhistoryActions").on("click", function () { $("#BtnCancel").show(); });
$("#TabuploadFiles").on("click", function () { $("#oldFormFooter").hide(); });

jQuery(function ($) {
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

    // Internacionalizad datepicker en catalán
    var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);
    $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

    // Botón de lanzamiento de popup de normas
    $("#BtnSelectRules").on("click", function (e) {
        e.preventDefault();
        RulesRenderPopup();
        var dialog = $("#dialogRules").removeClass("hide").dialog({
            "resizable": false,
            "modal": true,
            "title": Dictionary.Item_BusinessRisk_SelectRuleType,
            "title_html": true,
            "width": 800,
            "buttons":
            [
                {
                    "id": "BtnNewAddresSave",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        RulesInsert();
                    }
                },
                {
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
    if (businessRisk.PreviousBusinessRiskId > 0) {
        $("#InitialValue").prop("disabled", true);
    }

    //Things to do if the BusinessRisk already exists
    if (businessRisk.Id > 0) {
        var probability = GetProbabilityByCode(businessRisk.StartProbability);
        var severity = GetSeverityByCode(businessRisk.StartSeverity);
        $("#CmbProcess").val(businessRisk.Process.Id);
        //$("#ProbabilityValue").html(probability.Code + " - " + probability.Description);
        //$("#SeverityValue").html(severity.Code + " - " + severity.Description);

        if (businessRisk.StartProbability > 0 && businessRisk.StartSeverity > 0) {
            MinStepValue = 1;
        }

        RenderStepsSliders();

        // Si el riesgo es significativo no se puede elegir "NO" en aplicar acción
        if (businessRisk.StartProbability * businessRisk.StartSeverity >= rule.Limit)
        {
            document.getElementById("ApplyActionNoStart").style.visibility = "hidden";
            $("#ApplyActionNoStart").removeAttr("checked");
        }

        if (businessRisk.Assumed === true) {
            if (document.getElementById("Assumed") !== null) {
                document.getElementById("Assumed").checked = true;
            }
        }
        else {
            if (businessRisk.ApplyAction === true) {
                $("#ApplyActionYesStart").prop("checked", 1);
                ApplyActionTrue();
            }
            IncidentActionCostRenderTable("IncidentActionCostsTableData");

            if (typeof ApplicationUser.Grants.BusinessRisk === "undefined" || ApplicationUser.Grants.BusinessRisk.Write === false) {
                $("#costes .btn-info").hide();
                $(".btn-danger").hide();
            }
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

    $("#BtnSave").on("click", function (e) { SaveBtnPressed(); });
    $("#BtnCancel").on("click", function (e) { Cancel(); });
    $("#BtnSave2").on("click", function (e) { SaveBtnPressed(); });
    $("#BtnCancel2").on("click", function (e) { Cancel(); });

    if (document.getElementById("StartApplyActionYes").checked === true) {
        $("#Tabaccion").show();
        SaveAction = true;
    } else {
        $("#Tabaccion").hide();
        $("#Tabcostes").hide();
        SaveAction = false;
    }

    $("#TxtActionCauses").change(function (e) { SetCloseRequired(); });
    $("#CmbActionCausesResponsible").change(function (e) { SetCloseRequired(); });
    $("#TxtActionCausesDate").change(function (e) { SetCloseRequired(); });
    $("#TxtActionActions").change(function (e) { SetCloseRequired(); });
    $("#CmbActionActionsResponsible").change(function (e) { SetCloseRequired(); });
    $("#TxtActionActionsDate").change(function (e) { SetCloseRequired(); });
    $("#CmbActionClosedResponsible").change(function (e) { SetCloseRequired(); });
    $("#TxtActionClosedDate").change(function (e) { SetCloseRequired(); });

    $("#BtnNewCost").on("click", function (e) {
        e.preventDefault();
        ShowNewCostPopup(0);
    });

    $("#BtnCostBAR").on("click", function (e) {
        e.preventDefault();
        ShowCostBarPopup($("#TxtCostDescription"));
    });

    $("#Tabgraphic").on("click", resizegrafico);

    //Sincronizar los datos en la pestaña de situación inicial y de acción
    /*
    document.getElementById('Description').onblur = function () { syncFields('TxtActionWhatHappened', 'Description') };
    document.getElementById('TxtActionWhatHappened').onblur = function () { syncFields('Description', 'TxtActionWhatHappened') };
    document.getElementById('Causes').onblur = function () { syncFields('TxtActionCauses', 'Causes') };
    document.getElementById('TxtActionCauses').onblur = function () { syncFields('Causes', 'TxtActionCauses') };
    */
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

    var probability = businessRisk.StartProbability;
    var severity = businessRisk.StartSeverity;

    var probabilityName = Dictionary.Common_NotSet_Female;
    var severityName = Dictionary.Common_NotSet_Female;

    // Poner el nombre de la probabilidad/severidad
    for (var x = 0; x < ProbabilitySeverityList.length; x++) {
        if (ProbabilitySeverityList[x].Type === 0 && ProbabilitySeverityList[x].Code === probability) {
            probabilityName = ProbabilitySeverityList[x].Description;
        }

        if (ProbabilitySeverityList[x].Type === 1 && ProbabilitySeverityList[x].Code === severity) {
            severityName = ProbabilitySeverityList[x].Description;
        }
    }

    // @cristina: lo dejo oculto hasta que nos pongamos con los layouts
    //$('#TxtStartProbabilityLabel').html(Dictionary.Item_BusinessRisk_LabelField_Probability + ':&nbsp;<strong>' + probabilityName + '</strong>');
    //$('#TxtStartSeverityLabel').html(Dictionary.Item_BusinessRisk_LabelField_Severity + ':&nbsp;<strong>' + severityName + '</strong>');
}

function UpdateResult() {
    console.log("UpdateResult");
    //Si hay una acción indica que la pestaña inicial está bloqueada
    if (Action.Id > 0) { UpdateStartLabels(); }
    
    var probability = businessRisk.StartProbability;
    var severity = businessRisk.StartSeverity;

    var rangeColor = ['#777', '#4aa4ce', '#4aa4ce', '#fd3', '#ffb752', '#d40'];
    if (document.getElementById('StartSeverityRange') !== null) {
        if (document.getElementById('StartSeverityRange').nextElementSibling !== null) {
            document.getElementById('StartSeverityRange').nextElementSibling.style.backgroundColor = rangeColor[severity];
        }
    }
    if (document.getElementById('StartProbabilityRange') !== null) {
        if (document.getElementById('StartProbabilityRange').nextElementSibling !== null) {
            document.getElementById('StartProbabilityRange').nextElementSibling.style.backgroundColor = rangeColor[probability];
        }
    }

    // ISSUS - 233
    var calculatedResult = probability * severity;
    if (calculatedResult > 0) {
        $("#Result").html(probability * severity);
        $("#Result").css("color", probability * severity < rule.Limit ? '#3f3' : '#f33');
    }
    else {
        $("#Result").html("-");
        $("#Result").css("color", "#fff");
    }

    if (probability * severity < rule.Limit) {
        $("#ApplyActionNoStart").show();
        $("#ApplyActionAsumed").hide();
        $("#StartApplyActionAssumed").attr("checked", false);
    }
    else {
        $("#ApplyActionAsumed").show();
        $("#ApplyActionNoStart").hide();
        $("#StartApplyActionNo").attr("checked", false);
    }
}

function UpdateFinalResult() {
    var probability = businessRisk.FinalProbability;
    var severity = businessRisk.FinalSeverity;

    var probabilityName = Dictionary.Common_NotSet_Female;
    var severityName = Dictionary.Common_NotSet_Female;

    for (var x = 0; x < ProbabilitySeverityList.length; x++) {
        if (ProbabilitySeverityList[x].Type === 0 && ProbabilitySeverityList[x].Code === probability) {
            probabilityName = ProbabilitySeverityList[x].Description;
        }

        if (ProbabilitySeverityList[x].Type === 1 && ProbabilitySeverityList[x].Code === severity) {
            severityName = ProbabilitySeverityList[x].Description;
        }
    }
    var rangeColor = ["#777", "#4aa4ce", "#4aa4ce", "#fd3", "#ffb752", "#d40"];
    if (document.getElementById("FinalSeverityRange") !== null) {
        if (document.getElementById("FinalSeverityRange").nextElementSibling !== null) {
            document.getElementById("FinalSeverityRange").nextElementSibling.style.backgroundColor = rangeColor[businessRisk.FinalSeverity];
        }
    }
    if (document.getElementById("FinalProbabilityRange") !== null) {
        if (document.getElementById("FinalProbabilityRange").nextElementSibling !== null) {
            document.getElementById("FinalProbabilityRange").nextElementSibling.style.backgroundColor = rangeColor[businessRisk.FinalProbability];
        }
    }

    // @cristina: lo dejo oculto hasta que nos pongamos con los layouts
    //$("#TxtFinalProbabilityLabel").html(Dictionary.Item_BusinessRisk_LabelField_Probability + ":&nbsp;<strong>" + probabilityName + "</strong>");
    //$("#TxtFinalSeverityLabel").html(Dictionary.Item_BusinessRisk_LabelField_Severity + ":&nbsp;<strong>" + severityName + "</strong>");
    $("#FinalResult").html(businessRisk.FinalProbability * businessRisk.FinalSeverity);
    businessRisk.FinalResult = businessRisk.FinalProbability * businessRisk.FinalSeverity;

    if (businessRisk.FinalResult === 0)
    {
        $("#FinalResult").html("-");
        $("#FinalResult").css("color", "#333");
        document.getElementById("ApplyActionNoFinal").style.visibility = "visible";
    }
    else if(businessRisk.FinalResult < rule.Limit)
    {
        $("#FinalResult").css("color", "#87b87f");
        document.getElementById("ApplyActionNoFinal").style.visibility = "visible";
    }
    else
    {
        $("#FinalResult").css("color", "#f00");
        document.getElementById("ApplyActionNoFinal").style.visibility = "hidden";
    }
}

function ApplyActionRadio() {
    // Para elegir las acciones hay que tener evaluado el riesgo
    if (businessRisk.StartProbability === 0 || businessRisk.StartSeverity === 0) {
        alertUI(Dictionary.Item_BusinessRisk_ErrorMessage_ResultRequired);
        document.getElementById("StartApplyActionAssumed").checked = false;
        document.getElementById("StartApplyActionYes").checked = false;
        document.getElementById("StartApplyActionNo").checked = false;
        return false;
    }

    if (document.getElementById("StartApplyActionYes").checked === true) {
        ApplyActionTrue();
        SetCloseRequired();
        alertUI(Dictionary.Item_BusinessRisk_Warning_ActionTabAvailable, null, 500);
    }
    else {
        ApplyActionFalse();
    }
}

function FinalApplyActionRadio() {
    // Para elegir las acciones hay que tener evaluado el riesgo
    if (businessRisk.FinalProbability === 0 || businessRisk.FinalSeverity === 0) {
        alertUI(Dictionary.Item_BusinessRisk_ErrorMessage_ResultRequired);
        document.getElementById("FinalApplyActionAssumed").checked = false;
        document.getElementById("FinalApplyActionYes").checked = false;
        document.getElementById("FinalApplyActionNo").checked = false;
        document.getElementById("TxtFinalDate").disabled = true;
        document.getElementById("TxtFinalDateBtn").disabled = true;
        return false;
    }

    document.getElementById("TxtFinalDate").disabled = false;
    document.getElementById("TxtFinalDateBtn").disabled = false;
}

function ApplyActionTrue() {
    //Show action, cost and final status tabs and content
    $("#Tabaccion").show();
    $("#Tabcostes").show();
    $("#Tabgraphic").show();

    //Disable information editing on the risk
    $("#Name").prop("disabled", true);
    $("#DateStart").prop("disabled", true);
    $("#CmbRules").prop("disabled", true);
    $("#BtnSelectRules").hide();
    $("#CmbProcess").prop("disabled", true);
    $("#ApplyAction2").prop("disabled", true);
    $("#input-span-slider-probability").slider("disable");
    $("#input-span-slider-severity").slider("disable");
    SlidersActive = false;

    //Apply value to the final status tab
    $("#Initial-input-span-slider-probability").slider({ value: businessRisk.StartProbability });
    $("#Initial-input-span-slider-severity").slider({ value: businessRisk.StartSeverity });
    document.getElementById("InitialResult").value = businessRisk.StartResult;

    SaveAction = true;
    if (Action.Description === "") {
        document.getElementById("TxtActionDescription").value = $("#Name").val();
    }

    if (Action.WhatHappened === "") {
        document.getElementById("TxtActionWhatHappened").value = $("#Description").val();
    }

    if (Action.Causes === "") {
        document.getElementById("TxtActionCauses").value = $("#Causes").val();
    }

    if (Action.WhatHappenedBy.Id < 0) {
        $("#CmbActionWhatHappenedResponsible").val(ApplicationUser.Employee.Id);
    }

    if (Action.WhatHappenedOn === null) {
        $("#TxtActionWhatHappenedDate").val(FormatDate(new Date(), "/"));
    }

    if (Action.CausesBy.Id < 0) {
        $("#CmbActionCausesResponsible").val(ApplicationUser.Employee.Id);
    }

    if (Action.CausesOn === null) {
        $("#TxtActionCausesDate").val(FormatDate(new Date(), "/"));
    }

    var rangeColor = ["#777", "#4aa4ce", "#4aa4ce", "#fd3", "#ffb752", "#d40"];
    if (document.getElementById("InitialSeverityRange") !== null) {
        if (document.getElementById("InitialSeverityRange").nextElementSibling !== null) {
            document.getElementById("InitialSeverityRange").nextElementSibling.style.backgroundColor = rangeColor[businessRisk.StartSeverity];
        }
    }

    if (document.getElementById("InitialProbabilityRange") !== null) {
        if (document.getElementById("InitialProbabilityRange").nextElementSibling !== null) {
            document.getElementById("InitialProbabilityRange").nextElementSibling.style.backgroundColor = rangeColor[businessRisk.StartProbability];
        }
    }
}

function ApplyActionFalse() {
    //Hide action, cost and final status tabs and content
    $("#Tabaccion").hide();
    $("#Tabcostes").hide();
    $("#Tabgraphic").hide();

    //Enable information editing on the risk
    $("#Name").prop("disabled", false);
    $("#DateStart").prop("disabled", false);
    $("#CmbRules").prop("disabled", false);
    $("#BtnSelectRules").show();
    $("#CmbProcess").prop("disabled", false);
    $("#ApplyAction2").prop("disabled", false);
    $("#Assumed").prop("disabled", false);
    $("#input-span-slider-probability").slider("enable");
    $("#input-span-slider-severity").slider("enable");
    SlidersActive = true;
    SaveAction = false;
}

function FinalAssumedChanged() {
    if (document.getElementById("FinalAssumed").checked === true) {
        document.getElementById("FinalApplyAction1").disabled = true;
        document.getElementById("FinalApplyAction2").disabled = true;
        document.getElementById("FinalApplyAction1").checked = false;
        document.getElementById("FinalApplyAction2").checked = false;
    }
    else {
        document.getElementById("FinalApplyAction1").disabled = false;
        document.getElementById("FinalApplyAction2").disabled = false;
    }
}

function FinalApplyActionTrue() {
    ApplyActionFinal = true;
}

function FinalApplyActionFalse() {
    ApplyActionFinal = false;
}

function BusinessRiskInsert(previousId) {
    // 1.- Modificar en la BBDD
    var id = 0;
    UpdateResult();

    var startAction = 0;
    if (document.getElementById('StartApplyActionAssumed').checked === true) { startAction = 1; }
    if (document.getElementById('StartApplyActionNo').checked === true) { startAction = 2; }
    if (document.getElementById('StartApplyActionYes').checked === true) { startAction = 3; }

    var result = 0;
    if ($("#Result").val() !== "" && $("#Result").val() !== "-") {
        result = $("#Result").val() * 1;
    }

    var data = {
        "businessRisk": {
            "CompanyId": Company.Id,
            "Description": document.getElementById("Name").value,
            "Code": document.getElementById("Code").value * 1,
            "Rules": { "Id": document.getElementById("CmbRules").value * 1 },
            "ItemDescription": document.getElementById("Description").value,
            "StartControl": document.getElementById("StartControl").value,
            "Notes": document.getElementById("Notes").value,
            "Causes": document.getElementById("Causes").value,
            "StartProbability": businessRisk.StartProbability,
            "StartSeverity": businessRisk.StartSeverity,
            "StartResult": businessRisk.StartProbability * businessRisk.StartSeverity,
            "StartAction": startAction,
            "DateStart": GetDate(document.getElementById("DateStart").value, '/'),
            "Process": { "Id": document.getElementById("CmbProcess").value * 1 },
            "InitialValue": document.getElementById("InitialValue").value * 1,
            "previousBusinessRiskId": previousId,
            "assumed": document.getElementById("StartApplyActionAssumed").checked === true
        },
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/BusinessRiskActions.asmx/BusinessRiskInsert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            if (response.d.Success === true) {
                var newRisk = businessRisk.Id < 1;
                if (newRisk === true) {
                    businessRisk.Id = response.d.MessageError * 1;
                    if (SaveAction === true) {
                        console.log("risk+action");
                        SaveIncidentAction(businessRisk.Id, response.d.MessageError * 1, true);
                    }

                    alertInfoUI(Dictionary.Item_BusinessRisk_Message_InsertSucess, Reload);
                }
                else if (SaveAction === true) {// && ApplyActionFinal === true) {
                    console.log('risk+action');
                    SaveIncidentAction(businessRisk.Id, response.d.MessageError * 1);
                    //document.location = 'BusinessRisksList.aspx';
                }
                else {
                    console.log('risk updated');
                    document.location = 'BusinessRisksList.aspx';
                }
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    return false;
}

function Reload() {
    document.location = 'BusinessRiskView.aspx?id=' + businessRisk.Id;
}

function BusinessRiskUpdate(sender) {
    // 1.- Modificar en la BBDD

    var startAction = 0;
    if (document.getElementById('StartApplyActionAssumed').checked === true) { startAction = 1; }
    if (document.getElementById('StartApplyActionNo').checked === true) { startAction = 2; }
    if (document.getElementById('StartApplyActionYes').checked === true) { startAction = 3; }

    var finalAction = 0;
    if (document.getElementById('FinalApplyActionAssumed') !== null)
    {
        if (document.getElementById('FinalApplyActionAssumed').checked === true) { finalAction = 1; }
        if (document.getElementById('FinalApplyActionNo').checked === true) { finalAction = 2; }
        if (document.getElementById('FinalApplyActionYes').checked === true) { finalAction = 3; }
        
    }

    if (document.getElementById('TxtFinalDate') !== null) {
        businessRisk.FinalDate = GetDate(document.getElementById('TxtFinalDate').value, '/', false);
    }

    var data = {
        "newBusinessRisk": {
            "Id": businessRisk.Id,
            "Description": document.getElementById("Name").value,
            "Code": document.getElementById("Code").value,
            "Process": { "Id": document.getElementById("CmbProcess").value },
            "DateStart": GetDate(document.getElementById("DateStart").value, "/"),
            "ItemDescription": document.getElementById("Description").value,
            "Notes": document.getElementById("Notes").value,
            "Causes": document.getElementById("Causes").value,
            "StartControl": document.getElementById("StartControl").value,
            "InitialValue": document.getElementById("InitialValue").value,
            "Rules": { "Id": document.getElementById("CmbRules").value },

            "StartProbability": businessRisk.StartProbability,
            "StartSeverity": businessRisk.StartSeverity,
            "StartResult": businessRisk.StartProbability * businessRisk.StartSeverity,
            "StartAction": startAction,

            "FinalProbability": businessRisk.FinalProbability,
            "FinalSeverity": businessRisk.FinalSeverity,
            "FinalResult": businessRisk.FinalProbability * businessRisk.FinalSeverity,
            "FinalAction": finalAction,
            "FinalDate": GetDate(document.getElementById("TxtFinalDate").value, "/", false),
            "assumed": document.getElementById("StartApplyActionAssumed").checked === true,
            "CompanyId": Company.Id
        },
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/BusinessRiskActions.asmx/BusinessRiskUpdate",
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
                    if (businessRisk.FinalDate !== null) {
                        var newBusinessRiskId = response.d.MessageError * 1;
                        SaveIncidentAction(businessRisk.Id, newBusinessRiskId, false);
                    }
                    else {
                        SaveIncidentAction(businessRisk.Id, 0, false);
                    }
                }
                else {
                    //document.location = 'BusinessRiskView.aspx?id=' + businessRisk.Id;
                    document.location = 'BusinessRisksList.aspx';
                }
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    return false;
}

function SaveIncidentAction(businessRiskId, newBusinessRiskId, reload) {
    var reloadAfterSave = reload;
    var action =
        {
            "Id": Action.Id,
            "CompanyId": Company.Id,
            "ActionType": 3, // Preventiva
            "Description": $('#TxtActionDescription').val(),
            "Origin": 4, // BusinessRisk
            "ReporterType": 1,
            "Department": Action.Department,
            "Provider": Action.Provider,
            "Customer": Action.Customer,
            "Number": 0,
            "BusinessRiskId": businessRiskId,
            "IncidentId": -1,
            "WhatHappened": $('#TxtActionWhatHappened').val(),
            "WhatHappenedBy": { "Id": $('#CmbActionWhatHappenedResponsible').val() },
            "WhatHappenedOn": GetDate($('#TxtActionWhatHappenedDate').val(), "/", false),
            "Causes": $('#TxtActionCauses').val(),
            "CausesBy": { "Id": $('#CmbActionCausesResponsible').val() },
            "CausesOn": GetDate($('#TxtActionCausesDate').val(), "/", false),
            "Actions": $('#TxtActionActions').val(),
            "ActionsBy": { "Id": $('#CmbActionActionsResponsible').val() },
            "ActionsOn": GetDate($('#TxtActionActionsDate').val(), "/", false),
            "Monitoring": $('#TxtActionMonitoring').val(),
            "ClosedBy": { "Id": $('#CmbActionClosedResponsible').val() },
            "ClosedOn": GetDate($('#TxtActionClosedDate').val(), "/", false),
            "Notes": $('#TxtActionNotes').val(),
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
            if (reloadAfterSave === true) {
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
        if (this.businessRisk.Id === -1) {
            BusinessRiskInsert(-1);
        }
        else {
            BusinessRiskUpdate();
        }
    }
}

function Cancel() {
    document.location = referrer;
}

function SetCloseRequired() {
    /////////////////////////////////
    //Set required fields for risks//
    /////////////////////////////////

    FieldSetRequired("TxtRulesLabel", Dictionary.Item_BusinessRisk_LabelField_Rules, true);
    FieldSetRequired("TxtProcessLabel", Dictionary.Item_BusinessRisk_LabelField_Process, true);
    FieldSetRequired("TxtDateStartLabel", Dictionary.Item_BusinessRisk_LabelField_DateStart, true);
    FieldSetRequired("TxtResultLabel", Dictionary.Item_BusinessRisk_LabelField_Result, true);
    FieldSetRequired("TxtNameLabel", Dictionary.Item_BusinessRisk_LabelField_Name, true);

    ///////////////////////////////////
    //Set required fields for actions//
    ///////////////////////////////////
    FieldSetRequired("TxtActionDescriptionLabel", Dictionary.Item_IncidentAction_Label_Description, true);
    FieldSetRequired("TxtActionWhatHappenedLabel", Dictionary.Item_IncidentAction_Field_WhatHappened, true);
    FieldSetRequired("CmbActionWhatHappenedResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleWhatHappend, true);
    FieldSetRequired("TxtActionWhatHappenedDateLabel", Dictionary.Common_Date, true);

    //Checking if Causes is required
    if (document.getElementById("CmbActionCausesResponsible").value * 1 !== 0 || document.getElementById("StartApplyActionYes").checked === true) {
        FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, true);
        FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, true);
        FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Common_Date, true);
        CausesRequired = true;
    }
    else if (document.getElementById("TxtActionCauses").value !== "") {
        FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, true);
        FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, true);
        FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Common_Date, true);
        CausesRequired = true;
    }
    else if (document.getElementById("TxtActionCausesDate").value !== "") {
        FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, true);
        FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, true);
        FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Common_Date, true);
        CausesRequired = true;
    }
    else {
        FieldSetRequired("TxtActionCausesLabel", Dictionary.Item_IncidentAction_Field_Causes, false);
        FieldSetRequired("CmbActionCausesResponsibleLabel", Dictionary.Item_IncidentAction_Field_ResponsibleCauses, false);
        FieldSetRequired("TxtActionCausesDateLabel", Dictionary.Common_Date, false);
        CausesRequired = false;
    }

    //Checking if Actions is required
    if (document.getElementById('CmbActionActionsResponsible').value * 1 !== 0) {
        FieldSetRequired('TxtActionActionsLabel', Dictionary.Item_IncidentAction_Field_Actions, true);
        FieldSetRequired('CmbActionActionsResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleActions, true);
        FieldSetRequired('TxtActionActionsDateLabel', Dictionary.Common_Date, true);
        ActionsRequired = true;
    }
    else if (document.getElementById('TxtActionActions').value !== '') {
        FieldSetRequired('TxtActionActionsLabel', Dictionary.Item_IncidentAction_Field_Actions, true);
        FieldSetRequired('CmbActionActionsResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleActions, true);
        FieldSetRequired('TxtActionActionsDateLabel', Dictionary.Common_Date, true);
        ActionsRequired = true;
    }
    else if (document.getElementById('TxtActionActionsDate').value !== '') {
        FieldSetRequired('TxtActionActionsLabel', Dictionary.Item_IncidentAction_Field_Actions, true);
        FieldSetRequired('CmbActionActionsResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleActions, true);
        FieldSetRequired('TxtActionActionsDateLabel', Dictionary.Common_Date, true);
        ActionsRequired = true;
    }
    else {
        FieldSetRequired('TxtActionActionsLabel', Dictionary.Item_IncidentAction_Field_Actions, false);
        FieldSetRequired('CmbActionActionsResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleActions, false);
        FieldSetRequired('TxtActionActionsDateLabel', Dictionary.Common_Date, false);
        ActionsRequired = false;
    }

    //Checking if Closed is required
    if (document.getElementById('CmbActionClosedResponsible').value * 1 !== 0) {
        FieldSetRequired('CmbActionClosedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
        FieldSetRequired('TxtActionClosedDateLabel', Dictionary.Common_Date, true);
        ClosedRequired = true;
    }
    else if (document.getElementById('TxtActionClosedDate').value !== '') {
        FieldSetRequired('CmbActionClosedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleClose, true);
        FieldSetRequired('TxtActionClosedDateLabel', Dictionary.Common_Date, true);
        ClosedRequired = true;
    }
    else {
        FieldSetRequired('CmbActionClosedResponsibleLabel', Dictionary.Item_IncidentAction_Field_ResponsibleClose, false);
        FieldSetRequired('TxtActionClosedDateLabel', Dictionary.Common_Date, false);
        ClosedRequired = false;
    }

}

function ValidateData() {
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
    if (document.getElementById("CmbRules").value * 1 === 0) {
        ErrorMessageInicial.push(Dictionary.Item_BusinessRisk_ErrorMessage_RulesRequired);
        SetFieldTextMessages("TxtRules");
        ok = false;
    }

    // Es obligatorio indicar el proceso
    if (document.getElementById("CmbProcess").value * 1 === 0) {
        ErrorMessageInicial.push(Dictionary.Item_BusinessRisk_ErrorMessage_ProcessRequired);
        SetFieldTextMessages("TxtProcess");
        ok = false;
    }

    // Es obligatorio indicar el nombre del riesgo
    if (document.getElementById("Name").value === "") {
        ErrorMessageInicial.push(Dictionary.Item_BusinessRisk_ErrorMessage_NameRequired);
        SetFieldTextMessages("TxtName");
        ok = false;
    }

    // Es obligatorio indicar la fecha de inicio del riesgo
    if ($("#DateStart").val() === "") {
        ErrorMessageInicial.push(Dictionary.Item_BusinessRisk_ErrorMessage_DateStartRequired);
        SetFieldTextMessages("TxtDateStart");
        ok = false;
    } else {
        var data = GetDate(document.getElementById("DateStart").value, "/", false);

        // El inicio del riesgo no puede ser superior a hoy
        if (data > new Date()) {
            ErrorMessageInicial.push(Dictionary.Item_BusinessRisk_ErrorMessage_DateUpToLimit);
            document.getElementById("TxtDateStartLabel").style.color = "#f00";
            document.getElementById("DateStartDateUpToLimit").style.display = "";
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

        document.getElementById("TxtFinalResultLabel").style.color = "#000";
        document.getElementById("TxtFinalProbabilityLabel").style.color = "#000";
        document.getElementById("TxtFinalSeverityLabel").style.color = "#000";

        // La descripción de la acción es obligatoria
        if (document.getElementById("TxtActionDescription").value === "") {
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
                var riskStartDate = GetDate($("#DateStart").val(), "/", false);
                if (riskStartDate > dateWhatHappened) {
                    $("#TxtActionWhatHappenedDateOverDate").show();
                    $("#DateStartDateOutOfDate").show();
                    $("#TxtActionWhatHappenedDateLabel").css("color", "#f00");
                    $("#TxtDateStartLabel").css("color", "#f00");
                    ErrorMessageInicial.push(Dictionary.Item_BusinessRisk_ErrorMessage_StartDateOverdate);
                    ErrorMessageAccion.push(Dictionary.Item_BusinessRisk_ErrorMessage_WhatHappendOverdate);
                    ok = false;
                }
            }
        }

        if (document.getElementById("CmbActionWhatHappenedResponsible").value * 1 === 0) {
            ok = false;
            ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_WhatHappenedRequiredResponsible);
            SetFieldTextMessages("CmbActionWhatHappenedResponsible");
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
            if (document.getElementById("CmbActionCausesResponsible").value * 1 === 0) {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredResponsible);
                SetFieldTextMessages("CmbActionCausesResponsible");
            }

            if ($('#TxtActionCausesDate').val() === '') {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CausesRequiredDate);
                SetFieldTextMessages('TxtActionCausesDate');
            }
            else {
                if (!RequiredDateValue('TxtActionCausesDate')) {
                    ok = false;
                    ErrorMessageAccion.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Causes);
                }
                else {
                    dateCauses = GetDate($('#TxtActionCausesDate').val(), '/', false);
                }
            }
        }

        if (ActionsRequired === true) {
            ClearFieldTextMessages('TxtActionActions');
            ClearFieldTextMessages('TxtActionActionsResponsible');
            ClearFieldTextMessages('TxtActionActionsDate');
            if ($('#TxtActionActions').val() === '') {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequired);
                SetFieldTextMessages('TxtActionActions');
            }

            if (document.getElementById('CmbActionActionsResponsible').value * 1 === 0) {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredResponsible);
                SetFieldTextMessages('CmbActionActionsResponsible');
            }

            if ($('#TxtActionActionsDate').val() === '') {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_ActionsRequiredDate);
                SetFieldTextMessages('TxtActionActionsDate');
            }
            else {
                if (!RequiredDateValue('TxtActionActionsDate')) {
                    ok = false;
                    ErrorMessageAccion.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Actions);
                }
                else {
                    dateActions = GetDate($('#TxtActionActionsDate').val(), '/', false);
                }
            }

        }

        if (ClosedRequired === true) {
            ClearFieldTextMessages('TxtActionClosedDate');
            ClearFieldTextMessages('TxtActionClosedResponsible');
            if (document.getElementById('CmbActionClosedResponsible').value * 1 === 0) {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseExecutor);
                SetFieldTextMessages('TxtActionClosedDate');
            }
            if (document.getElementById('TxtActionClosedDate').value === '') {
                ok = false;
                ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_CloseExecutorDate);
                SetFieldTextMessages('TxtActionClosedDate');
            }
            else {
                if (!RequiredDateValue('TxtActionClosedDate')) {
                    ok = false;
                    ErrorMessageAccion.push(Dictionary.Item_IncidentAction_Error_DateMalformed_Actions);
                }
                else {
                    dateClose = GetDate($('#TxtActionClosedDate').val(), '/', false);
                }
            }
        }

        // Sincronizacion de fechas
        if (dateWhatHappened !== null) {
            if (dateCauses !== null && dateWhatHappened > dateCauses) {
                okDates = false;
                SetFieldTextMessages('TxtActionWhatHappenedDate');
                SetFieldTextMessages('TxtActionCausesDate');
            }
            if (dateActions !== null && dateWhatHappened > dateActions) {
                okDates = false;
                SetFieldTextMessages('TxtActionWhatHappenedDate');
                SetFieldTextMessages('TxtActionActionsDate');
            }
            if (dateClose !== null && dateWhatHappened > dateClose) {
                okDates = false;
                SetFieldTextMessages('TxtActionWhatHappenedDate');
                SetFieldTextMessages('TxtActionClosedDate');
            }
        }

        if (dateCauses !== null) {
            if (dateActions !== null && dateCauses > dateActions) {
                okDates = false;
                SetFieldTextMessages('TxtActionActionsDate');
                SetFieldTextMessages('TxtActionCausesDate');
            }
            if (dateClose !== null && dateCauses > dateClose) {
                okDates = false;
                SetFieldTextMessages('TxtActionCausesDate');
                SetFieldTextMessages('TxtActionClosedDate');
            }
        }

        if (dateActions !== null) {
            if (dateClose !== null && dateActions > dateClose) {
                okDates = false;
                SetFieldTextMessages('TxtActionCausesDate');
                SetFieldTextMessages('TxtActionClosedDate');
            }
        }

        if (okDates === false) {
            ok = false;
            ErrorMessageAccion.push(Dictionary.Item_IncidentAction_ErrorMessage_UntemporalyDates);
        }
    }

    // Si se marca la fecha de situación final se creará un nuevo riesgo y hay que rellenar los datos
    if ($("#TxtFinalDate").val() !== "") {
        console.log(businessRisk.FinalProbability);
        console.log(businessRisk.FinalSeverity);

        if (businessRisk.FinalProbability === 0 && businessRisk.FinalSeverity === 0) {
            ErrorMessageFinal.push(Dictionary.Item_BusinessRisk_ErrorMessage_ResultRequired);
            document.getElementById('TxtFinalResultLabel').style.color = '#f00';
            document.getElementById('TxtFinalProbabilityLabel').style.color = '#f00';
            document.getElementById('TxtFinalSeverityLabel').style.color = '#f00';
            SetFieldTextMessages('FinalResult');
            ok = false;
        }
        else {
            if (businessRisk.FinalProbability === 0) {
                ok = false;
                document.getElementById('TxtFinalProbabilityLabel').style.color = '#f00';
                ErrorMessageFinal.push(Dictionary.Item_BusinessRisk_ErrorMessage_FinalProbabilityRequired);
            }
            else {
                document.getElementById('TxtFinalProbabilityLabel').style.color = '#000';
            }

            if (businessRisk.FinalSeverity === 0) {
                ok = false;
                document.getElementById('TxtFinalSeverityLabel').style.color = '#f00';
                ErrorMessageFinal.push(Dictionary.Item_BusinessRisk_ErrorMessage_FinalSeverityRequired);
            }
            else {
                document.getElementById('TxtFinalSeverityLabel').style.color = '#000';
            }
        }

        if (document.getElementById("FinalApplyActionYes").checked === false &&
            document.getElementById("FinalApplyActionNo").checked === false &&
            document.getElementById("FinalApplyActionAssumed").checked === false) {
            ok = false;
            ErrorMessageFinal.push(Dictionary.Item_BusinessRisk_ErrorMessage_ClosedRequiredData);
        }

        //Close date check
        if (document.getElementById("TxtFinalDate").value === "") {
            ErrorMessageFinal.push(Dictionary.Item_BusinessRisk_ErrorMessage_DateCloseRequired);
            SetFieldTextMessages("TxtFinalDate");
            ok = false;
        }
        else {
            var data = GetDate(document.getElementById("TxtFinalDate").value, "/", false);
            var closeAction = GetDateYYYYMMDD(Action.ClosedOn, false);

            if (data < closeAction) {
                ErrorMessageFinal.push(Dictionary.Item_BusinessRisk_ErrorMessage_CloseBeforeAction);
                document.getElementById("TxtFinalDateLabel").style.color = "#f00";
                ok = false;
            }
            else {
                if (!RequiredDateValue("TxtFinaldate")) {
                    ok = false;
                    ErrorMessageFinal.push(Dictionary.Common_Error_DateMalformed);
                }
                else {
                    dateActions = GetDate($("#TxtFinalDate").val(), "/", false);
                }
            }
        }
    }

    if (ok === false) {
        var ErrorContent = '';


        if (ErrorMessageInicial.length > 0) {
            ErrorContent += '<strong>' + Dictionary.Item_BusinessRisk_Tab_Basic + '</strong><ul>';
            for (var x = 0; x < ErrorMessageInicial.length; x++) {
                ErrorContent += '<li>' + ErrorMessageInicial[x] + '</li>';
            }
            ErrorContent += '</ul>';
        }


        if (ErrorMessageAccion.length > 0) {
            ErrorContent += '<strong>' + Dictionary.Item_BusinessRisk_Tab_Action + '</strong><ul>';
            for (var x = 0; x < ErrorMessageAccion.length; x++) {
                ErrorContent += '<li>' + ErrorMessageAccion[x] + '</li>';
            }
            ErrorContent += '</ul>';
        }


        if (ErrorMessageFinal.length > 0) {
            ErrorContent += '<strong>' + Dictionary.Item_BusinessRisk_Tab_Graphics + '</strong><ul>';
            for (var x = 0; x < ErrorMessageFinal.length; x++) {
                ErrorContent += '<li>' + ErrorMessageFinal[x] + '</li>';
            }
            ErrorContent += '</ul>';
        }

        warningInfoUI("<strong>" + Dictionary.Common_Message_FormErrors + "</strong><br />" + ErrorContent, null, 600);
        return false;
    }
    else {
        return true;
    }

}

function SetSeverity(code, object) {
    document.getElementById("SeverityDataContainer").value = code;
    var num = document.getElementById("SelectableSeverity").childNodes.length;
    for (var x = 0; x < num; x++) {
        document.getElementById("SelectableSeverity").childNodes[x].style.fontWeight = 'normal';
    }
    object.parentNode.parentNode.style.fontWeight = 'bold';
}

function SetProbability(code, object) {
    document.getElementById("ProbabilityDataContainer").value = code;
    var num = document.getElementById("SelectableProbability").childNodes.length;
    for (var x = 0; x < num; x++) {
        document.getElementById("SelectableProbability").childNodes[x].style.fontWeight = 'normal';
    }
    object.parentNode.parentNode.style.fontWeight = 'bold';
}

function GetProbabilityByCode(code) {
    for (var x = 0; x < ProbabilitySeverityList.length; x++) {
        var item = ProbabilitySeverityList[x];
        if (item.Type === 0) {
            if (item.Code === code) {
                return item;
            }
        }
    }
    return null;
}

function GetSeverityByCode(code) {
    for (var x = 0; x < ProbabilitySeverityList.length; x++) {
        var item = ProbabilitySeverityList[x];
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
        if (Employees[x].Active === true) {
            var option = document.createElement('OPTION');
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            document.getElementById('CmdIncidentActionCostResponsible').appendChild(option);
        }
    }
}

SetCloseRequired();

function IsHistoric() {
    return false;
    if (BusinessRiskHistory.length > 0) {
        var actualId = BusinessRiskHistory[BusinessRiskHistory.length - 1].Id;
        if (businessRisk.Id !== actualId) {
            $("#HistoricMessageDiv").hide();
            $("#HistoricMessageDiv").html("");
            var message = Dictionary.Item_BusinessRisk_MessageHistoric.split("#");

            message = message[0] + BusinessRiskHistory[BusinessRiskHistory.length - 1].DateStart + message[1] + " ";

            document.getElementById("HistoricMessageDiv").appendChild(document.createTextNode(message));
            var a = document.createElement("A");
            a.href = "/BusinessRiskView.aspx?id=" + actualId;
            a.style.fontWeight = "bold";
            a.appendChild(document.createTextNode(Dictionary.Common_Here));
            document.getElementById("HistoricMessageDiv").appendChild(a);
        }
    }
}

IsHistoric();

if (businessRisk.Id > 0) {
    $("#DateStart").val(businessRisk.DateStart);
}
else {
    $("#DateStart").val(FormatDate(new Date(), "/"));
}

$("#DateStart").datepicker({ maxDate: "0" });
$("#DateClose").datepicker({ minDate: GetDate(businessRisk.DateStart, "/", false) });
$(".date-picker").on("blur", function () { DatePickerChanged(this); });

CmbResponsibleFill();

// Problemas de comillas en el nombre
if (businessRisk.Id > 0) {
    $('#Name').val(businessRisk.Description);
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

function RenderStartSliders() {
    var MinStepValue = 1;

        $("#input-span-slider-startprobability").slider({
            "value": businessRisk.StartProbability,
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
                $("#input-span-slider-startprobability").slider({ "value": this.id });
                businessRisk.StartProbability = val;
                UpdateResult();
                return null;
            }
        });

        $("#input-span-slider-startseverity").slider({
            "value": businessRisk.StartSeverity,
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
                $("#input-span-slider-severity").slider({ "value": this.id });
                businessRisk.StartSeverity = val;
                UpdateResult();
                return null;
            }
        });

    if (Action.Id > 0)
    {
        $("#StartProbabilityRange").hide();
        $("#StartSeverityRange").hide();
    }

    VoidTable("stepsStartProbability");
    VoidTable("stepsStartSeverity");

    for (var x = MinStepValue; x < 6; x++) {
        var span = document.createElement("span");
        span.id = x;
        span.className = "tick";
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement("BR"));
        span.appendChild(document.createTextNode('|'));
        span.style.left = ((100 / (5 - MinStepValue)) * (x - MinStepValue)) + "%";
        switch (x) {
            case 1:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_1;
                break;
            case 2:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_2;
                break;
            case 3:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_3;
                break;
            case 4:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_4;
                break;
            case 5:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_5;
                break;
        }
        document.getElementById('stepsStartProbability').appendChild(span);
        if (Action.Id < 1) {
            span.onclick = function () {
                if (SlidersStartActive) {
                    $("#input-span-slider-startprobability").slider({ value: this.id });
                    businessRisk.StartProbability = this.id * 1;
                    UpdateResult();
                }
            };
        }
    }

    for (var x2 = MinStepValue; x2 < 6; x2++) {
        var spanStep = document.createElement("span");
        spanStep.id = x2;
        spanStep.className = "tick";
        spanStep.appendChild(document.createTextNode(x2));
        spanStep.appendChild(document.createElement("BR"));
        spanStep.appendChild(document.createTextNode("|"));
        spanStep.style.left = ((100 / (5 - MinStepValue)) * (x2 - MinStepValue)) + "%";
        switch (x2) {
            case 1:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_1;
                break;
            case 2:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_2;
                break;
            case 3:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_3;
                break;
            case 4:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_4;
                break;
            case 5:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_5;
                break;
        }
        document.getElementById("stepsStartSeverity").appendChild(spanStep);
        if (Action.Id < 1) {
            spanStep.onclick = function () {
                if (SlidersStartActive) {
                    $("#input-span-slider-startseverity").slider({ "value": this.id });
                    businessRisk.StartSeverity = this.id * 1;
                    UpdateResult();
                }
            };
        }
    }

    if (businessRisk.Id > 0) {
        UpdateResult();
    }
}

function RenderStepsSliders() {
    var MinStepValue = 1;
    RenderStartSliders();

    //////////////////////////////////////////////////////////////////////////
    //Sliders in "Situació final" refering the initial situation of the risk//
    //////////////////////////////////////////////////////////////////////////
    $("#Initial-input-span-slider-probability").slider({
        "value": businessRisk.StartProbability,
        "range": "min",
        "min": 1,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) { return null; }
    });
    $("#Initial-input-span-slider-probability").slider("disable");
    $("#Initial-input-span-slider-severity").slider({
        "value": businessRisk.StartSeverity,
        "range": "min",
        "min": 1,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) { return null; }
    });
    $("#Initial-input-span-slider-severity").slider("disable");
    VoidTable("InitialStepsProbability");
    for (var x = 1; x < 6; x++) {
        var span = document.createElement("span");
        span.id = x;
        span.className = "tick";
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement("BR"));
        span.appendChild(document.createTextNode("|"));
        span.style.left = ((100 / (5 - MinStepValue)) * (x - MinStepValue)) + "%";
        switch (x) {
            case 1:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_1;
                break;
            case 2:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_2;
                break;
            case 3:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_3;
                break;
            case 4:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_4;
                break;
            case 5:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_5;
                break;
        }
        document.getElementById("InitialStepsProbability").appendChild(span);
    }
    VoidTable("InitialStepsSeverity");
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
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_1;
                break;
            case 2:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_2;
                break;
            case 3:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_3;
                break;
            case 4:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_4;
                break;
            case 5:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_5;
                break;
        }
        document.getElementById("InitialStepsSeverity").appendChild(spanStep);
    }

    var rangeColor = ["#777", "#4aa4ce", "#4aa4ce", "#fd3", "#ffb752", "#d40"];
    document.getElementById("InitialProbabilityRange").style.backgroundColor = rangeColor[businessRisk.StartProbability];
    document.getElementById("InitialSeverityRange").style.backgroundColor = rangeColor[businessRisk.StartSeverity];
    //////////////////////////////////////////////////////////////////////////
    //Sliders in "Situació final" refering the initial situation of the risk//
    //////////////////////////////////////////////////////////////////////////


    //Check min value for the sliders
    if (businessRisk.FinalProbability > 0) {
        MinStepValueFinalProbability = 1;
    }

    if (businessRisk.FinalSeverity > 0) {
        MinStepValueFinalSeverity = 1;
    }

    console.log("Final==> P:" + finalProbability + " S:" + finalSeverity);

    $("#Final-input-span-slider-probability").slider({
        "value": businessRisk.FinalProbability,
        "range": "min",
        "min": MinStepValueFinalProbability,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) {
            var val = parseInt(ui.value);
            if (val === 0) {
                return false;
            }
            $("#input-span-slider-probability").slider({ "value": this.id });
            businessRisk.FinalProbability = val;
            UpdateFinalResult();
            return null;
        }
    });
    $("#Final-input-span-slider-severity").slider({
        "value": businessRisk.FinalSeverity,
        "range": "min",
        "min": MinStepValueFinalSeverity,
        "max": 5,
        "step": 1,
        "slide": function (event, ui) {
            var val = parseInt(ui.value);
            if (val === 0) {
                return false;
            }
            $("#input-span-slider-severity").slider({ "value": this.id });
            businessRisk.FinalSeverity = val;
            UpdateFinalResult();
            return null;
        }
    });
    VoidTable("FinalStepsProbability");
    for (var x = MinStepValueFinalProbability; x < 6; x++) {
        var span = document.createElement("span");
        span.id = x;
        span.className = "tick";
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement("BR"));
        span.appendChild(document.createTextNode("|"));
        span.style.left = ((100 / (5 - MinStepValueFinalProbability)) * (x - MinStepValueFinalProbability)) + "%";
        switch (x) {
            case 1:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_1;
                break;
            case 2:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_2;
                break;
            case 3:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_3;
                break;
            case 4:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_4;
                break;
            case 5:
                span.title = Dictionary.Item_BusinessRisk_Tooltip_Probability_5;
                break;
        }
        document.getElementById("FinalStepsProbability").appendChild(span);
        if (x > 0) {
            span.onclick = function () {
                $("#Final-input-span-slider-probability").slider({ "value": this.id });
                businessRisk.FinalProbability = this.id * 1;
                UpdateFinalResult();
            };
        }
    }
    VoidTable("FinalStepsSeverity");
    for (var x2 = MinStepValueFinalSeverity; x2 < 6; x2++) {
        var spanStep = document.createElement("span");
        spanStep.id = x2;
        spanStep.className = "tick";
        spanStep.appendChild(document.createTextNode(x2));
        spanStep.appendChild(document.createElement("BR"));
        spanStep.appendChild(document.createTextNode("|"));
        spanStep.style.left = ((100 / (5 - MinStepValueFinalSeverity)) * (x2 - MinStepValueFinalSeverity)) + "%";
        switch (x2) {
            case 1:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_1;
                break;
            case 2:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_2;
                break;
            case 3:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_3;
                break;
            case 4:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_4;
                break;
            case 5:
                spanStep.title = Dictionary.Item_BusinessRisk_Tooltip_Severity_5;
                break;
        }
        document.getElementById("FinalStepsSeverity").appendChild(spanStep);
        if (x2 > 0) {
            spanStep.onclick = function () {
                $("#Final-input-span-slider-severity").slider({ "value": this.id });
                businessRisk.FinalSeverity = this.id * 1;
                UpdateFinalResult();
            };
        }
    }

    //Update final result (textbox and slider color)
    UpdateFinalResult();
}

function syncFields(target, source) {
    document.getElementById(target).value = document.getElementById(source).value;
}

if (document.getElementById("TxtFinalProbabilityLabel") !== null)
{
    UpdateFinalResult();
}
UpdateResult();

function ValidateCloseAction() {
    if (businessRisk.FinalProbability === 0 || businessRisk.FinalSeverity === 0) {
        document.getElementById("TxtFinalDate").disabled = true;
        document.getElementById("TxtFinalDateBtn").disabled = true;
    }
    else {
        document.getElementById("TxtFinalDate").disabled = false;
        document.getElementById("TxtFinalDateBtn").disabled = false;
    }
}

//$("#TxtFinalDate").on("focus", ValidateCloseAction);

// Establecer el resultado inicial
console.log("businessRisk.Assumed", businessRisk.Assumed);
if (businessRisk.Result > 0)
{
    if(businessRisk.Assumed === true)
    {
        document.getElementById("StartApplyActionAssumed").checked = true;
    }
    else {
        if(businessRisk.ApplyAction === true)
        {
            document.getElementById("StartApplyActionYes").checked = true;
        }
        else {

            document.getElementById("StartApplyActionNo").checked = true;
        }
    }
}

// No se puede cerrar el riesgo si la acción no está cerrada
if (Action.ClosedOn === null) {
    if (document.getElementById("DivClosingRisk") !== null) {
        $("#DivClosingRisk").hide();;
        $("#DivClosingRiskUnavailable").show();
    }
}
else {
    if (document.getElementById("DivClosingRisk") !== null) {
        $("#DivClosingRisk").show();
        $("#DivClosingRiskUnavailable").hide();;
    }
}

if (Action.Id > 0)
{
    document.getElementById("StartApplyActionAssumed").disabled = true;
    document.getElementById("StartApplyActionYes").disabled = true;
    document.getElementById("StartApplyActionNo").disabled = true;
}

ValidateCloseAction();

// Controles iniciales
if (businessRisk.StartAction === 0) {
    $("#Tabgraphic").hide();
}
if (businessRisk.StartAction === 1) {
    document.getElementById("StartApplyActionAssumed").checked = true;
    $("Tabgraphic").hide();
}
if (businessRisk.StartAction === 2) {
    document.getElementById("StartApplyActionNo").checked = true;
    $("Tabgraphic").hide();
}
if (businessRisk.StartAction === 3) { document.getElementById("StartApplyActionYes").checked = true; }

// Controles finales
if (document.getElementById("FinalApplyActionAssumed") !== null) {
    if (businessRisk.FinalAction === 1) { document.getElementById("FinalApplyActionAssumed").checked = true; }
    if (businessRisk.FinalAction === 2) { document.getElementById("FinalApplyActionNo").checked = true; }
    if (businessRisk.FinalAction === 3) { document.getElementById("FinalApplyActionYes").checked = true; }
}

if (ApplicationUser.Grants.Rules !== null)
{
    if(ApplicationUser.Grants.Rules.Write == false)
    {
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
    $("#DivHistoryTableDiv .btn-info").hide();
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 450);
}

function PrintData() {
    window.open("/export/PrintBusinessriskData.aspx?id=" + BusinessRiskId + "&companyId=" + Company.Id);
    return false;
}

window.onload = function () {
    Resize();

    $("#Tabhome").on("click", HideAnulateActionButton);
    $("#Tabaccion").on("click", ShowAnulateActionButton);
    $("#Tabcostes").on("click", HideAnulateActionButton);
    $("#Tabgraphic").on("click", HideAnulateActionButton);
    $("#TabhistoryActions").on("click", HideAnulateActionButton);
    $("#TabuploadFiles").on("click", HideAnulateActionButton);

    $("#TxtActionWhatHappenedDateDateMalformed").after("<span class=\"ErrorMessage\" id=\"TxtActionWhatHappenedDateOverDate\" style=\"display: none;\">" + Dictionary.Item_BusinessRisk_ErrorMessage_ActionVerDate + "</span>");

    if (BusinessRiskId > 0) {
        $("#BtnPrint").on("click", PrintData);
    }
    else {
        $("#BtnPrint").hide();
    }

    var res = "&nbsp;<button class=\"btn btn-danger\" type=\"button\" id=\"BtnAnular\" style=\"display:inline-block;\"><i class=\"icon-ban-circle bigger-110\"></i>" + Dictionary.Item_BusinessRisk_Button_CloseAction + "</button>";
    res += "&nbsp;<button class=\"btn btn-primary\" type=\"button\" id=\"BtnRestaurar\" style=\"display:inline-block;\"><i class=\"icon-undo bigger-110\"></i>" + Dictionary.Item_BusinessRisk_Button_RestoreAction + "</button>";

    $("#ItemButtons").prepend(res);

    $("#BtnAnular").on("click", AnularPopup);
    $("#BtnRestaurar").on("click", Restore);
    $("#TxtActionClosedDateDateMalformed").after("<span class=\"ErrorMessage\" id=\"TxtActionClosedDateErrorCross\" style=\"display:none;\">" + Dictionary.Item_BusinessRisk_ErrorMessage_ClosedRequiredDataOutTime + "</span>")
    $("#Tabhome").click();

    $("#CmbActionActionsResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbActionActionsResponsible").val() * 1, Employees, this); });

}
window.onresize = function () { Resize(); }

var CostBlocked = false;
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

// ------------- Anular accion
$("#BtnAnular").hide();
$("#BtnRestaurar").hide();
if (Action.ClosedOn === null && Action.Id > 0) {
    $("#BtnAnular").show();
} else {
    $("#BtnRestaurar").show();
    AnulateLayout();
}

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

    $("#CmbActionClosedResponsibleLabel").css("color", "#000");
    $("#TxtActionClosedDateLabel").css("color", "#000");
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

/// <var>The anulation data</var>
var anulationData = null;

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
        $("@TxtActionClosedDateLabel").css("color", "#f00");
        $("#TxtActionClosedDateDateRequired").show();
    }
    else {
        if (validateDate($("#TxtActionClosedDate").val()) === false) {
            ok = false;
            $("#TxtActionClosedDateLabel").css("color", "#f00");
            $("#TxtActionClosedDateDateMalformed").show();
        }
        else {
            var actionsDate = GetDate($("#TxtActionActionsDate").val(), "/", false);
            var closeDate = GetDate($("#TxtActionClosedDate").val(), "/", false);
            if (closeDate < actionsDate) {
                ok = false;
                $("#TxtActionClosedDateErrorCross").show();
                $("#TxtActionClosedDateLabel").css("color", "#f00");
            }
        }
    }

    if ($("#CmbActionClosedResponsible").val() * 1 < 1) {
        ok = false;
        $("#CmbActionClosedResponsibleLabel").css("color", "#f00");
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
            SaveIncidentAction(businessRisk.Id, msg.d.MessageError * 1, true);
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AnulateLayout() {
    $("#BtnRestaurar").hide();
    $("#DivAnulateMessage").remove();
    if (Action.ClosedOn !== null) {
        $("#BtnNewCost").hide();
        var message = "<div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_IncidentAction_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndDate + ":&nbsp;<strong>" + GetDateYYYYMMDDText(Action.ClosedOn,"/", false) + "</strong><br />";
        message += "        " + Dictionary.Item_IncidentAction_Label_EndResponsible + ":&nbsp;<strong>" + Action.ClosedBy.Value + "</strong>";
        message += "    </p>";
        message += "</div><br /><br /><br />";
        $("#accion").append(message);
        $("#BtnAnular").hide();
        $("#BtnRestaurar").show();

        $("#accion input").attr("readonly", "readonly");
        $("#accion select").attr("readonly", "readonly");
        $("#accion textarea").attr("readonly", "readonly");
        //$("#BtnSave2").hide();
    }
    else {
        $("#BtnNewCost").show();
        $("#DivAnulateMessage").hide();
        if (Action.Id > 0) {
            $("#BtnAnular").show();
        }
        $("#accion input").removeAttr("readonly");
        $("#accion select").removeAttr("readonly");
        $("#accion textarea").removeAttr("readonly");
    }
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