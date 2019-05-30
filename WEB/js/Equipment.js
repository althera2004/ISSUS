var VerificationInternalActive = false;
var VerificationExternalActive = false;
var VerificationInternalExists = false;
var VerificationExternalExists = false;
var EquipmentNewId;
$("#BtnRecordShowNone").remove();

var initialDateText = Dictionary.Item_EquipmentErrorInitialDate + " " + limitInitialDate;
$("#TxtStartDateDateMalformed").after("<span class=\"ErrorMessage\" id=\"TxtStartDatePostInitial\" style=\"display:none;\">" + initialDateText + "</span>");
$("#TxtScaleDivision").val(ToMoneyFormat(Equipment.ScaleDivision,4));
function CalibrationCheckChanged() {
    if (Equipment.Id === 0) {
        return;
    }
    document.getElementById("Tabcalibracion").style.display = document.getElementById("Contentholder1_status0").checked ? "" : "none";
}

function VerificationCheckChanged() {
    if (Equipment.Id === 0) {
        return;
    }
    document.getElementById("Tabverificacion").style.display = document.getElementById("Contentholder1_status1").checked ? "" : "none";
}

function MaintenanceCheckChanged() {
    if (Equipment.Id === 0) {
        return;
    }
    document.getElementById("Tabmantenimiento").style.display = document.getElementById("Contentholder1_status2").checked ? "" : "none";
}

function ShowNewVerificationButton() {
    if (document.getElementById("VerificationInternalActive").checked === true || document.getElementById("VerificationExternalActive").checked) {
        $("#BtnNewVerification").show();
        $("#VerificationWarning").hide();
    }
    else {
        $("#BtnNewVerification").hide();
        $("#VerificationWarning").show();
    }
}

function SaveEquipment() {
    var resultValidate = ValidateForm(EquipmentForm);
    if (resultValidate.length > 0) {
        var text = "<h4>" + Dictionary.Common_Form_Errors + "</h4><ul>";
        for (var x = 0; x < resultValidate.length; x++) {
            text += "<li>" + resultValidate[x] + "</li>";
        }

        text += "</ul>";
        warningInfoUI(text, null, 600);
        return false;
    }

    var InternalCalibration = emptyCalibration;
    var ExternalCalibration = emptyCalibration;
    var InternalVerification = emptyVerification;
    var ExternalVerification = emptyVerification;

    if (document.getElementById("CalibrationInternalActive").checked === true) {
        InternalCalibration = {
            "Id": Equipment.InternalCalibration.Id === 0 ? -1 : Equipment.InternalCalibration.Id,
            "EquipmentId": Equipment.Id,
            "CompanyId": Company.Id,
            "CalibrationType": 0,
            "Description": $("#TxtCalibrationInternalOperation").val(),
            "Periodicity": ParseInputValueToNumber($("#TxtCalibrationInternalPeriodicity").val()),
            "Uncertainty": ParseInputValueToNumber($("#TxtCalibrationInternalUncertainty").val()),
            "Range": $("#TxtCalibrationInternalRange").val(),
            "Pattern": $("#TxtCalibrationInternalPattern").val(),
            "Cost": ParseInputValueToNumber($("#TxtCalibrationInternalCost").val()),
            "Notes": $("#TxtCalibrationInternalNotes").val(),
            "Provider": { Id: 0 },
            "Responsible": GetEmployeeById($("#CmbCalibrationInternalResponsible").val() * 1)
        };
    }

    if (document.getElementById("CalibrationExternalActive").checked === true) {
        ExternalCalibration = {
            "Id": Equipment.ExternalCalibration.Id === 0 ? -1 : Equipment.ExternalCalibration.Id,
            "EquipmentId": Equipment.Id,
            "CompanyId": Company.Id,
            "CalibrationType": 1,
            "Description": $("#TxtCalibrationExternalOperation").val(),
            "Periodicity": ParseInputValueToNumber($("#TxtCalibrationExternalPeriodicity").val()),
            "Uncertainty": ParseInputValueToNumber($("#TxtCalibrationExternalUncertainty").val()),
            "Range": $("#TxtCalibrationExternalRange").val(),
            "Pattern": $("#TxtCalibrationExternalPattern").val(),
            "Cost": ParseInputValueToNumber($("#TxtCalibrationExternalCost").val()),
            "Notes": $("#TxtCalibrationExternalNotes").val(),
            "Provider": { "Id": $("#CmbCalibrationExternalProvider").val() * 1 },
            "Responsible": GetEmployeeById($("#CmbCalibrationExternalResponsible").val() * 1)
        };
    }

    if (document.getElementById("VerificationInternalActive").checked === true) {
        InternalVerification = {
            "Id": Equipment.InternalVerification.Id === 0 ? -1 : Equipment.InternalVerification.Id,
            "EquipmentId": Equipment.Id,
            "CompanyId": Company.Id,
            "VerificationType": 0,
            "Description": $("#TxtVerificationInternalOperation").val(),
            "Periodicity": ParseInputValueToNumber($("#TxtVerificationInternalPeriodicity").val()),
            "Uncertainty": StringToNumberNullable($("#TxtVerificationInternalUncertainty").val(), ".", ","),
            "Range": $("#TxtVerificationInternalRange").val(),
            "Pattern": $("#TxtVerificationInternalPattern").val(),
            "Cost": StringToNumberNullable($("#TxtVerificationInternalCost").val(), ".", ","),
            "Notes": $("#TxtVerificationInternalNotes").val(),
            "Responsible": GetEmployeeById($("#CmbVerificationInternalResponsible").val() * 1)
        };
    }

    if (document.getElementById("VerificationExternalActive").checked === true) {
        ExternalVerification = {
            "Id": Equipment.ExternalVerification.Id === 0 ? -1 : Equipment.ExternalVerification.Id,
            "EquipmentId": Equipment.Id,
            "CompanyId": Company.Id,
            "VerificationType": 1,
            "Description": $("#TxtVerificationExternalOperation").val(),
            "Periodicity": ParseInputValueToNumber($("#TxtVerificationExternalPeriodicity").val()),
            "Uncertainty": StringToNumberNullable($("#TxtVerificationExternalUncertainty").val(), ".", ","),
            "Range": $("#TxtVerificationExternalRange").val(),
            "Pattern": $("#TxtVerificationExternalPattern").val(),
            "Cost": StringToNumberNullable($("#TxtVerificationExternalCost").val(), ".", ","),
            "Notes": $("#TxtVerificationExternalNotes").val(),
            "Provider": { "Id": $("#CmbVerificationExternalProvider").val() * 1 },
            "Responsible": GetEmployeeById($("#CmbVerificationExternalResponsible").val() * 1)
        };
    }

    var NewEquipment = {
        "Id": Equipment.Id,
        "CompanyId": Company.Id,
        "Code": $("#TxtCode").val(),
        "Description": $("#TxtDescription").val(),
        "TradeMark": $("#TxtTradeMark").val(),
        "Model": $("#TxtModel").val(),
        "SerialNumber": $("#TxtSerialNumber").val(),
        "Location": $("#TxtLocation").val(),
        "MeasureRange": ParseInputValueToNumber($("#TxtMeasureRange").val()),
        "ScaleDivisionValue": ParseInputValueToNumber($("#TxtScaleDivision").val()),
        "MeasureUnit": {
            "Id": $("#CmbEquipmentScaleDivision").val() * 1,
            "Value": $("#CmbEquipmentScaleDivision option:selected").text()
        },
        "Responsible": GetEmployeeById($("#CmbResponsible").val() * 1),
        "IsCalibration": document.getElementById("Contentholder1_status0").checked,
        "IsVerification": document.getElementById("Contentholder1_status1").checked,
        "IsMaintenance": document.getElementById("Contentholder1_status2").checked,
        "Notes": $("#TxtNotes").val(),
        "Observations": $("#TxtObservations").val(),
        "Active": true,
        "InternalCalibration": InternalCalibration,
        "ExternalCalibration": ExternalCalibration,
        "InternalVerification": InternalVerification,
        "ExternalVerification": ExternalVerification,
        "StartDate": GetDate($("#TxtStartDate").val(), "/", false)
    };

    if (Equipment.StartDate !== null) {
        Equipment.StartDate = GetDate(GetDateYYYYMMDDText(Equipment.StartDate, "/", false), "/", false);
    }

    if (Equipment.Id > 0) {
        var dataUpdate = {
            "newItem": NewEquipment,
            "oldItem": Equipment,
            "companyId": Company.Id,
            "userId": user.Id,
            "scaleDivision": $("#TxtScaleDivision").val()
        };
        $.ajax({
            "type": "POST",
            "url": "/Async/EquipmentActions.asmx/Update",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(dataUpdate, null, 2),
            "success": function (msg) {
                document.location = referrer;
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var dataInsert = {
            "equipment": NewEquipment,
            "companyId": Company.Id,
            "userId": user.Id,
            "scaleDivision": $("#TxtScaleDivision").val()
        };
        $.ajax({
            "type": "POST",
            "url": "/Async/EquipmentActions.asmx/Insert",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(dataInsert, null, 2),
            "success": function (msg) {
                if (msg.d.MessageError * 1 > 0) {
                    alertInfoUI(Dictionary.Item_Equipment_Message_InsertSucess, Reload);
                    EquipmentNewId = msg.d.MessageError * 1;
                }
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }

    return false;
}

function Reload()
{
    document.location = "EquipmentView.aspx?id=" + EquipmentNewId;
}

// ISSUS-190
document.getElementById("TxtCode").focus();

function PrintData() {
    window.open("/export/PrintEquipmentData.aspx?id=" + Equipment.Id + "&companyId=" + Equipment.CompanyId);
    return false;
}

// Poner los actos ordenados
window.onload = function () {
    $("#VerificationDivTable #th0").click();
    $("#VerificationDivTable #th0").click();
    $("#CalibrationDivTable #th0").click();
    $("#CalibrationDivTable #th0").click();
    $("#EquipmentMaintenanceActTable #th0").click();
    $("#EquipmentMaintenanceActTable #th0").click();
    $("#TableEquipmentRepairMain #th0").click();
    $("#TableEquipmentRepairMain #th0").click();

    // Filtro automatico registros
    $("#CalInt").on("click", EquipmentRecordGetFromFilter);
    $("#CalExt").on("click", EquipmentRecordGetFromFilter);
    $("#VerInt").on("click", EquipmentRecordGetFromFilter);
    $("#VerExt").on("click", EquipmentRecordGetFromFilter);
    $("#ManInt").on("click", EquipmentRecordGetFromFilter);
    $("#ManExt").on("click", EquipmentRecordGetFromFilter);
    $("#RepInt").on("click", EquipmentRecordGetFromFilter);
    $("#RepExt").on("click", EquipmentRecordGetFromFilter);
    $("#TxtRecordsFromDate").on("change", EquipmentRecordGetFromFilter);
    $("#TxtRecordsToDate").on("change", EquipmentRecordGetFromFilter);

    if (typeof ApplicationUser.Grants.Equipment === "undefined" || ApplicationUser.Grants.Equipment === false) {
        $("input").attr("disabled", true);
        $("textarea").attr("disabled", true);
        $("select").attr("disabled", true);

        $("#BtnCalibrationInternalSave").hide();
        $("#BtnCalibrationExternalSave").hide();
        $("#BtnNewCalibration").hide();
        $("#BtnVerificationInternalSave").hide();
        $("#BtnVerificationExternalSave").hide();
        $("#BtnNewVerification").hide();
        $("#BtnNewMaintainment").hide();
        $("#BtnNewMaintainmentAct").hide();
        $("#EquipmentRepairNewBtn").hide();
        $("#BtnNewUploadfile").hide();
        $(".btn-danger").hide();

        $("#BtnSave").hide();
        $("#BtnCalibrationSave").hide();
        $("#BtnVerificationSave").hide();
        $("#BtnMaintenanceSave").hide();
        $("#BtnRepairSave").hide();
        $("#BtnRecordsSave").hide();
    }

    if (Equipment.InternalVerification.Uncertainty === null) {
        $("#TxtVerificationInternalUncertainty").val("");
    }

    if (Equipment.InternalVerification.Cost === null) {
        $("#TxtVerificationInternalCost").val("");
    }

    if (Equipment.ExternalVerification.Uncertainty === null) {
        $("#TxtVerificationExternalUncertainty").val("");
    }

    if (Equipment.ExternalVerification.Cost === null) {
        $("#TxtVerificationExternalCost").val("");
    }

    AnulateLayout();
};

function ValidateForm(form) {
    var result = new Array();
    $("#TxtStartDatePostInitial").hide();
    for (var x = 0; x < form.RequiredFields.length; x++) {
        if (RequiredFieldText(form.RequiredFields[x]) === false) { result.push('Revise los campos obligatorios del equipo.'); }
    }

    for (var y = 0; y < form.DuplicatedFields.length; y++) {
        if (DuplicatedFiled(form.DuplicatedFields[y]) === false) { result.push('Los datos del equipo están repetidos.'); }
    }

    for (var z = 0; z < form.MinimumOptions.length; z++) {
        if (RequiredMinimumCheckBox(form.MinimumOptions[z]) === false) { result.push('Elija almenos una opción.'); }
    }

    if (!RequiredFieldCombo('CmbResponsible')) {
        result.push('Hay que informar el responsable del equipo.');
    }

    if (Equipment.Id > 0) {
        var CalibrationSet = document.getElementById('Contentholder1_status0').checked;
        var VerificationSet = document.getElementById('Contentholder1_status1').checked;
        var MaintenanceSet = document.getElementById('Contentholder1_status2').checked;

        if (CalibrationSet === true) {
            var CInternalSet = document.getElementById('CalibrationInternalActive').checked;
            var CExternalSet = document.getElementById('CalibrationExternalActive').checked;
            if (CInternalSet === false && CExternalSet === false) {
                result.push('Hay que definir almenos una calibración.')
            }
            else {

                if (CInternalSet === true) {
                    if (!RequiredFieldText('TxtCalibrationInternalOperation')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_InternalOperation_Required) }
                    if (!RequiredFieldText('TxtCalibrationInternalPeriodicity')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_InternalPeriodicity_Required) }
                    if (!RequiredFieldText('TxtCalibrationInternalUncertainty')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_InternalUncertainty_Required) }
                    if (!RequiredFieldText('TxtCalibrationInternalRange')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_InternalRangeRequired) }
                    if (!RequiredFieldText('TxtCalibrationInternalPattern')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_InternalPatternRequired) }
                    // ISSUS-18
                    //if(!RequiredFieldText('TxtCalibrationInternalCost')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_InternalCost_Required)}
                    if (!RequiredFieldCombo('CmbCalibrationInternalResponsible')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_InternalResponsibleRequired) }
                }

                if (CExternalSet === true) {
                    if (!RequiredFieldText('TxtCalibrationExternalOperation')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_ExternalOperation_Required) }
                    if (!RequiredFieldText('TxtCalibrationExternalPeriodicity')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_ExternalPeriodicity_Required) }
                    if (!RequiredFieldText('TxtCalibrationExternalUncertainty')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_ExternalUncertainty_Required) }
                    if (!RequiredFieldText('TxtCalibrationExternalRange')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_ExternalRange_Required) }
                    if (!RequiredFieldText('TxtCalibrationExternalPattern')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_ExternalPattern_Required) }
                    // ISSUS-18
                    //if(!RequiredFieldText('TxtCalibrationExternalCost')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_ExternalCost_Required)}
                    if (!RequiredFieldCombo('CmbCalibrationExternalResponsible')) { result.push(Dictionary.Item_EquipmentCalibration_ErrorMessage_ExternalResponsible_Required) }
                }
            }
        }

        if (VerificationSet === true) {
            var VInternalSet = document.getElementById("VerificationInternalActive").checked;
            var VExternalSet = document.getElementById("VerificationExternalActive").checked;
            if (VInternalSet === false && VExternalSet === false) {
                result.push("Hay que definir almenos una verificación.");
            }
            else {
                if (VInternalSet === true) {
                    if (!RequiredFieldText('TxtVerificationInternalOperation')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_InternalOperation_Required) }
                    if(!RequiredFieldText('TxtVerificationInternalPeriodicity')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_InternalPeriodicity_Required)}
                    //if(!RequiredFieldText('TxtVerificationInternalUncertainty')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_InternalUncertainty_Required)}
                    //if (!RequiredFieldText('TxtVerificationInternalRange')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_InternalRange_Required) }
                    //if (!RequiredFieldText('TxtVerificationInternalPattern')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_InternalPattern_Required) }
                    // ISSUS-18
                    // if(!RequiredFieldText('TxtVerificationInternalCost')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_InternalCost_Required)}
                    if (!RequiredFieldCombo('CmbVerificationInternalResponsible')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_InternalResponsible_Required) }
                }

                if (VExternalSet === true) {
                    if (!RequiredFieldText('TxtVerificationExternalOperation')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_ExternalOperation_Required) }
                    if(!RequiredFieldText('TxtVerificationExternalPeriodicity')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_ExternalPeriodicity_Required)}
                    //if(!RequiredFieldText('TxtVerificationExternalUncertainty')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_ExternalUncertainty_Required)}
                    //if(!RequiredFieldText('TxtVerificationExternalRange')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_ExternalRange_Required)}
                    //if (!RequiredFieldText('TxtVerificationExternalPattern')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_ExternalPattern_Required) }
                    // ISSUS-18
                    // if(!RequiredFieldText('TxtVerificationExternalCost')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_ExternalCost_Required)}
                    if (!RequiredFieldCombo('CmbVerificationExternalResponsible')) { result.push(Dictionary.Item_EquipmentVerification_ErrorMessage_ExternalResponsible_Required) }
                }
            }
        }

        if (MaintenanceSet === true) {
            if (EquipmentMaintenanceDefinitionList.length === 0) {
                result.push(Dictionary.Item_EquipmentMaintenance_Required)
            }
        }

        // @cristina no tener en cuenta fecha alta al validar datos
        if ($("#TxtStartDate").val() !== "") {
            if (limitInitialDate !== "") {
                var date1 = GetDate($("#TxtStartDate").val(), "/", false);
                var date2 = GetDate(limitInitialDate, "/", false);
                if (date1 > date2) {
                    $("#TxtStartDatePostInitial").show();
                    result.push(Dictionary.Item_EquipmentErrorInitialDate + " " + limitInitialDate);
                }
            }
        }
    }

    return result;
}

// ANULAR / RESTAURAR -------------------------------
function AnularPopup() {
    $("#TxtEndDate").val(FormatDate(new Date(), "/"));
    $("#TxtAnularComments").html("");
    $("#CmbEndResponsible").val(user.Employee.Id);
    var dialog = $("#dialogAnular").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Equipment_PopupAnular_Title,
        "width": 600,
        "buttons":
        [
            {
                "id": "BtnAnularSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
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
    $("#TxtEndReasonLabel").css("color", "#000");
    $("#TxtEndDateLabel").css("color", "#000");
    $("#CmbEndResponsibleLabel").css("color", "#000");
    $("#TxtEndReasonErrorRequired").hide();
    $("#TxtEndDateErrorRequired").hide();
    $("#TxtEndDateMalformed").hide();
    $("#CmbEndResponsibleErrorRequired").hide();

    var ok = true;
    if ($("#TxtEndReason").val() === "") {
        ok = false;
        document.getElementById("TxtEndReasonLabel").style.color = "#f00";
        $("#TxtEndReasonErrorRequired").show();
    }

    if ($("#TxtEndDate").val() === "") {
        ok = false;
        document.getElementById("TxtEndDateLabel").style.color = "#f00";
        $("#TxtEndDateRequired").show();
    }
    else {
        if (validateDate($("#TxtEndDate").val()) === false) {
            ok = false;
            $("#TxtEndDateLabel").css("color", "#f00");
            $("#TxtEndDateMalformed").show();
        }
    }

    if ($("#CmbEndResponsible").val() * 1 < 1) {
        ok = false;
        document.getElementById("CmbEndResponsibleLabel").style.color = "#f00";
        $("#CmbEndResponsibleErrorRequired").show();
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "equipmentId": Equipment.Id,
        "companyId": Company.Id,
        "reason": $("#TxtEndReason").val(),
        "responsible": $("#CmbEndResponsible").val() * 1,
        "date": GetDate($("#TxtEndDate").val(), "/"),
        "applicationUserId": user.Id
    };
    anulationData = data;
    $("#dialogAnular").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EquipmentActions.asmx/Anulate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = referrer;
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AnulateLayout() {
    $("#BtnRestaurar").hide();
    if (Equipment.EndDate !== null) {
        var message = "<br /><div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_Equipment_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_Equipment_FieldLabel_EndReason + ": <strong>" + Equipment.EndReason + "</strong><br />";
        message += "        " + Dictionary.Item_Equipment_FieldLabel_EndDate + ": <strong>" + FormatYYYYMMDD(Equipment.EndDate, "/") + "</strong><br />";
        message += "        " + Dictionary.Item_Equipment_FieldLabel_EndResponsible + ": <strong>" + Equipment.EndResponsible.Value + "</strong>";
        message += "    </p>";
        message += "</div>";
        //$("#home").append(message);
        $("#oldFormFooter").before(message);
        $("#BtnAnular").hide();
        $("#BtnRestaurar").show();

        $("input").attr("disabled", "disabled");
        $("select").attr("disabled", "disabled");
        $("select").css("backgroundColor", "#eee;");
        $("textarea").attr("disabled", "disabled");
        $("textarea").css("backgroundColor", "#eee;");
        $("#BtnEquipmentChangeImage").hide();
        $("#BtnSave").hide();
        $("#BtnEquipmentScaleDivisionBAR").hide();
        $("#BtnCalibrationInternalSave").hide();
        $("#BtnCalibrationExternalSave").hide();
        $("#BtnCalibrationExternalProviderBAR").hide();
        $("#BtnVerificationInternalSave").hide();
        $("#BtnVerificationExternalSave").hide();
        $("#BtnVerificationExternalProviderBAR").hide();
        $("#BtnNewCalibration").hide();
        $("#BtnNewVerification").hide();
        $("#BtnNewMaintainment").hide();
        $("#BtnNewMaintainmentAct").hide();
        $("#EquipmentRepairNewBtn").hide();
        $("#BtnNewUploadfile").hide();
        $("TD .btn-danger").hide();
        $(".document-container .btn-danger").hide();
    }
    else {
        $("#DivAnulateMessage").hide();
        $("#BtnAnular").show();
    }
}

function Restore() {
    var data = {
        "equipmentId": Equipment.Id,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EquipmentActions.asmx/Restore",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = document.location + "";
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}
// --------------------------------------------------

$("#CmbCalibrationInternalResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbCalibrationInternalResponsible").val() * 1, Employees, this); });
$("#CmbCalibrationExternalResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbCalibrationExternalResponsible").val() * 1, Employees, this); });
//("#CmbVerificationInternalResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbVerificationInternalResponsible").val() * 1, Employees, this); });
$("#CmbVerificationExternalResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbVerificationExternalResponsible").val() * 1, Employees, this); });
//$("#CmbEquipmentVerificationActResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbEquipmentVerificationActResponsible").val() * 1, Employees, this); });
$("#CmbNewMaintainmentResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbNewMaintainmentResponsible").val() * 1, Employees, this); });