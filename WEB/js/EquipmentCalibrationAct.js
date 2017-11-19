var SelectedEquipmentCalibrationAct;
var SelectedEquipmentCalibrationActId;
var SelectedEquipmentCalibrationActOperation;
var CalibrationInternalDefinition;
var CalibrationExternalDefinition;

var CalibrationInternalActive = false;
var CalibrationExternalActive = false;
var CalibrationInternalExists = false;
var CalibrationExternalExists = false;

function SetCalibrationForm() {
    document.getElementById('CalibrationInternalActive').checked = this.Equipment.InternalCalibration != null;
    LockInternalCalibrationForm(document.getElementById('CalibrationInternalActive').checked);

    document.getElementById('CalibrationExternalActive').checked = this.Equipment.ExternalCalibration != null;
    LockExternalCalibrationForm(document.getElementById('CalibrationExternalActive').checked);

    if (!document.getElementById('CalibrationInternalActive').checked && !document.getElementById('CalibrationExternalActive').checked) {
        document.getElementById('CalibrationWarning').style.display = '';
        document.getElementById('CalibrationDivTable').style.display = 'none';
    }
    else {
        document.getElementById('CalibrationWarning').style.display = 'none';
        document.getElementById('CalibrationDivTable').style.display = '';
    }
}

function CalibrationInternalResetForm() {
    $('#TxtCalibrationInternalOperation').val('');
    $('#TxtCalibrationInternalPeriodicity').val('');
    $('#TxtCalibrationInternalUncertainty').val('');
    $('#TxtCalibrationInternalRange').val('');
    $('#TxtCalibrationInternalPattern').val('');
    $('#TxtCalibrationInternalCost').val('');
    $('#CmbCalibrationInternalResponsible').val('');
    $('#TxtCalibrationInternalNotes').val('');
    $('#BtnCalibrationInternalSave').val('');
    CalibrationInternalSetForm();
}

function CalibrationExternalResetForm() {
    $('#TxtCalibrationExternalOperation').val('');
    $('#TxtCalibrationExternalPeriodicity').val('');
    $('#TxtCalibrationExternalUncertainty').val('');
    $('#TxtCalibrationExternalRange').val('');
    $('#TxtCalibrationExternalPattern').val('');
    $('#TxtCalibrationExternalCost').val('');
    $('#CmbCalibrationExternalResponsible').val('');
    $('#CmbCalibrationExternalProvider').val('');
    $('#TxtCalibrationExternalNotes').val('');
    $('#BtnCalibrationExternalSave').val('');
    CalibrationExternalSetForm();
}

function CalibrationInternalSetForm() {
    var active = document.getElementById('CalibrationInternalActive').checked;
    $('#TxtCalibrationInternalOperation').prop('readonly', !active);
    $('#TxtCalibrationInternalPeriodicity').prop('readonly', !active);
    $('#TxtCalibrationInternalUncertainty').prop('readonly', !active);
    $('#TxtCalibrationInternalRange').prop('readonly', !active);
    $('#TxtCalibrationInternalPattern').prop('readonly', !active);
    $('#TxtCalibrationInternalCost').prop('readonly', !active);
    $('#CmbCalibrationInternalResponsible').prop('disabled', !active);
    $('#CmbCalibrationInternalResponsible').val(ApplicationUser.Employee.Id);
    $('#TxtCalibrationInternalNotes').prop('readonly', !active);
    $('#BtnCalibrationInternalSave').prop('disabled', !active);
    CalibrationInternalActive = active;
    CalibrationInternalExists = Equipment.InternalCalibration != null && Equipment.InternalCalibration.Id > 0;
    ShowNewCalibrationButton();
}

function CalibrationExternalSetForm() {
    var active = document.getElementById('CalibrationExternalActive').checked;
    $('#TxtCalibrationExternalOperation').prop('readonly', !active);
    $('#TxtCalibrationExternalPeriodicity').prop('readonly', !active);
    $('#TxtCalibrationExternalUncertainty').prop('readonly', !active);
    $('#TxtCalibrationExternalRange').prop('readonly', !active);
    $('#TxtCalibrationExternalPattern').prop('readonly', !active);
    $('#TxtCalibrationExternalCost').prop('readonly', !active);
    $('#CmbCalibrationExternalResponsible').prop('disabled', !active);
    $('#CmbCalibrationExternalResponsible').val(ApplicationUser.Employee.Id);
    $('#CmbCalibrationExternalProvider').prop('disabled', !active);
    $('#CmbCalibrationExternalProvider').val(0);
    $('#TxtCalibrationExternalNotes').prop('readonly', !active);
    $('#BtnCalibrationExternalProviderBAR').prop('disabled', !active);
    $('#BtnCalibrationExternalSave').prop('disabled', !active);
    CalibrationExternalActive = active;
    CalibrationExternalExists = Equipment.ExternalCalibration != null && Equipment.ExternalCalibration.Id > 0;
    ShowNewCalibrationButton();
}

function CalibrationInternalCheckActive() {
    document.getElementById('CalibrationInternalActive').checked = true;
    return false;
}

function CalibrationExternalCheckActive() {
    document.getElementById('CalibrationExternalActive').checked = true;
    return false;
}

function LockInternalCalibrationForm(active) {
    if (active === false && this.Equipment.InternalCalibration !== null && this.Equipment.InternalCalibration.Id === 0) {
        CalibrationInternalResetForm();
        return;
    }

    if (active === false && this.Equipment.InternalCalibration === null) {
        CalibrationInternalResetForm();
        return;
    }

    if (active === false && CalibrationExternalExists === false) {
        try {
            warningInfoUI(Dictionary.Common_Error_LastActive, null, 400);
            document.getElementById('CalibrationInternalActive').checked = true;
            return false;
        }
        catch (ex) { }
    }

    if (active === false && this.Equipment.InternalCalibration != null && this.Equipment.InternalCalibration.Id > 0) {
        promptInfoUI("seguro", 300, CalibrationInternalDelete, CalibrationInternalCheckActive);
    }
    else {
        CalibrationInternalSetForm();
    }
}

function LockExternalCalibrationForm(active) {
    if (active === false && this.Equipment.ExternalCalibration !== null && this.Equipment.ExternalCalibration.Id === 0) {
        CalibrationExternalResetForm();
        return;
    }

    if (active === false && this.Equipment.ExternalCalibration === null) {
        CalibrationExternalResetForm();
        return;
    }

    if (active === false && CalibrationInternalExists === false) {
        try {
            warningInfoUI(Dictionary.Common_Error_LastActive, null, 400);
            document.getElementById('CalibrationExternalActive').checked = true;
            return false;
        }
        catch (ex) { }
    }

    if (active === false && this.Equipment.ExternalCalibration != null && this.Equipment.ExternalCalibration.Id > 0) {
        promptInfoUI("seguro", 300, CalibrationExternalDelete, CalibrationExternalCheckActive);
    }
    else {
        CalibrationExternalSetForm();
    }
}

function ShowNewCalibrationButton() {
    if (CalibrationInternalExists === true || CalibrationExternalExists === true) {
        document.getElementById('BtnNewCalibration').style.display = '';
        document.getElementById('CalibrationWarning').style.display = 'none';
    }
    else {
        document.getElementById('BtnNewCalibration').style.display = 'none';
        document.getElementById('CalibrationWarning').style.display = '';
    }
}

function EquipmentCalibrationActRenderTable(targetName) {
    VoidTable(targetName);
    var target = document.getElementById(targetName);
    var total = 0;
    for (var x = 0; x < EquipmentCalibrationActList.length; x++) {
        // WEKE
        if (x === 8) {
            break;
        }
        if (EquipmentCalibrationActList[x].Active === true) {
            total += EquipmentCalibrationActRenderRow(EquipmentCalibrationActList[x], targetName);
        }
    }

    $("#TableEquipmentCalibrationActTotalLabel").html("<i style=\"color:#aaa;font-weight:bold;\">" + Dictionary.Common_RegisterCount +":&nbsp;"+ EquipmentCalibrationActList.length + "</i><span style=\"float:right\">" + Dictionary.Common_Total + ":</span>");
    document.getElementById('TableEquipmentCalibrationActTotal').innerHTML = ToMoneyFormat(total, 2);
    if ($("#CalibrationDivTable #th0").attr('class') === "sort  ASC") {
        $("#CalibrationDivTable #th0").click();
    }
    else {
        $("#CalibrationDivTable #th0").click();
        $("#CalibrationDivTable #th0").click();
    }
}

function EquipmentCalibrationActRenderRow(equipmentCalibrationAct, targetName) {
    var target = document.getElementById(targetName);
    var row = document.createElement('TR');
    var tdFecha = document.createElement('TD');
    var tdType = document.createElement('TD');
    var tdMax = document.createElement('TD');
    var tdResult = document.createElement('TD');
    var tdResponsible = document.createElement('TD');
    var tdCost = document.createElement('TD');
    var tdVto = document.createElement('TD');

    tdFecha.style.width = '90px';
    tdType.style.width = '90px';
    tdMax.style.width = '130px';
    tdResult.style.width = '120px';
    tdCost.style.width = '120px';
    tdVto.style.width = '90px';

    tdFecha.align = 'center';
    tdVto.align = 'center';
    tdCost.align = 'right';
    tdResult.align = 'right';
    tdMax.align = 'right';

    if (equipmentCalibrationAct.Result > equipmentCalibrationAct.MaxResult) {
        tdResult.style.color = '#f00';
    }

    tdMax.appendChild(document.createTextNode(ToMoneyFormat(equipmentCalibrationAct.MaxResult, 6)));
    tdResult.appendChild(document.createTextNode(ToMoneyFormat(equipmentCalibrationAct.Result, 6)));
    if (equipmentCalibrationAct.Cost !== null) {
        tdCost.appendChild(document.createTextNode(ToMoneyFormat(equipmentCalibrationAct.Cost, 2)));
    }
    tdType.appendChild(document.createTextNode(equipmentCalibrationAct.EquipmentCalibrationType == 0 ? Dictionary.Common_Internal : Dictionary.Common_External));
    tdFecha.appendChild(document.createTextNode(FormatYYYYMMDD(equipmentCalibrationAct.Date, '/')));

    var vto = null;
    if (typeof equipmentCalibrationAct.Expiration != "undefined") { vto = FormatYYYYMMDD(equipmentCalibrationAct.Expiration, '/'); }
    if (typeof equipmentCalibrationAct.Vto != "undefined") { vto = FormatYYYYMMDD(equipmentCalibrationAct.Vto, '/'); }

    tdVto.appendChild(document.createTextNode(vto));
    tdResponsible.appendChild(document.createTextNode(equipmentCalibrationAct.Responsible.Value));

    row.id = 'EquipmentCalibrationAct' + equipmentCalibrationAct.Id;
    row.appendChild(tdFecha);
    row.appendChild(tdType);
    row.appendChild(tdMax);
    row.appendChild(tdResult);
    row.appendChild(tdResponsible);
    row.appendChild(tdCost);
    row.appendChild(tdVto);

    if (GrantToWrite) {
        var tdActions = document.createElement('TD');
        var iconEdit = document.createElement('SPAN');
        var iconDelete = document.createElement('SPAN');

        iconEdit.className = 'btn btn-xs btn-info';
        iconEdit.title = Dictionary.Common_Edit;
        iconEdit.onclick = function (e) { EquipmentCalibrationActEdit(this) }
        var innerEdit = document.createElement('I');
        innerEdit.className = 'icon-edit bigger-120';
        iconEdit.appendChild(innerEdit);

        iconDelete.className = 'btn btn-xs btn-danger';
        iconDelete.title = Dictionary.Common_Delete;
        iconDelete.onclick = function (e) { EquipmentCalibrationActDelete(this) }
        var innerDelete = document.createElement('I');
        innerDelete.className = 'icon-trash bigger-120';
        iconDelete.appendChild(innerDelete);

        tdActions.appendChild(iconEdit);
        tdActions.appendChild(document.createTextNode(' '));
        tdActions.appendChild(iconDelete);
        tdActions.style.width = '90px';
        row.appendChild(tdActions);
    }

    target.appendChild(row);
    return equipmentCalibrationAct.Cost * 1;
}

function EquipmentCalibrationActgetById(id) {
    for (var x = 0; x < EquipmentCalibrationActList.length; x++) {
        if (EquipmentCalibrationActList[x].Id == id) {
            return EquipmentCalibrationActList[x];
        }
    }

    return null;
}

function EquipmentCalibrationActRemoveFromList(id) {
    var temp = new Array();

    for (var x = 0; x < EquipmentCalibrationActList.length; x++) {
        if (EquipmentCalibrationActList[x].Id != id) {
            temp.push(EquipmentCalibrationActList[x]);
        }
    }

    EquipmentCalibrationActList = new Array();

    for (var x = 0; x < temp.length; x++) {
        EquipmentCalibrationActList.push(temp[x]);
    }
}

function EquipmentCalibrationActListUpdate(equipmentCalibrationAct) {
    for (var x = 0; x < EquipmentCalibrationActList.length; x++) {
        if (EquipmentCalibrationActList[x].Id == equipmentCalibrationAct.Id) {
            EquipmentCalibrationActList[x] = equipmentCalibrationAct;
        }
    }
}

function EquipmentCalibrationActNewFormReset(EquipmentCalibrationDefinition) {

    document.getElementById('REquipmentCalibrationActTypeErrorRequired').style.display = 'none';
    var internal = document.getElementById('CalibrationInternalActive').checked;
    var external = document.getElementById('CalibrationExternalActive').checked;

    document.getElementById('REquipmentCalibrationActTypeInternal').disabled = !CalibrationInternalExists;
    document.getElementById('REquipmentCalibrationActTypeExternal').disabled = !CalibrationExternalExists;

    var onlyExternal = false;
    if (!internal === true || !external === true) {
        if (external === true) {
            onlyExternal = true;
            document.getElementById('REquipmentCalibrationActTypeExternal').checked = true;
        }
        else {
            document.getElementById('REquipmentCalibrationActTypeInternal').checked = true;
        }
    }
    else {
        document.getElementById('REquipmentCalibrationActTypeInternal').checked = false;
        document.getElementById('REquipmentCalibrationActTypeExternal').checked = false;
    }

    SelectedEquipmentCalibrationActId = -1;
    document.getElementById('REquipmentCalibrationActTypeErrorRequired').style.display = 'none';
    ClearFieldTextMessages('TxtEquipmentCalibrationActDate');
    ClearFieldTextMessages('TxtEquipmentCalibrationActCost');
    ClearFieldTextMessages('TxtEquipmentCalibrationActResult');
    ClearFieldTextMessages('CmbEquipmentCalibrationActProvider');
    ClearFieldTextMessages('CmbEquipmentCalibrationActResponsible');
    $('#TxtEquipmentCalibrationActDate').val('');
    $('#TxtEquipmentCalibrationActObservations').val('');
    $('#TxtEquipmentCalibrationActResult').val('');
    $('#TxtEquipmentCalibrationActCost').val('');
    $('#CmbEquipmentCalibrationActResponsible').val(ApplicationUser.Employee.Id);
    $('#TxtEquipmentCalibrationActDate').val(FormatDate(new Date, '/'));
    if (onlyExternal === true) {
        document.getElementById('CmbEquipmentCalibrationActProviderRow').style.display = '';
        $('#CmbEquipmentCalibrationActProvider').val($('#CmbCalibrationExternalProvider').val());
    }
    else {
        document.getElementById('CmbEquipmentCalibrationActProviderRow').style.display = 'none';
    }
}

function EquipmentCalibrationActEditFormFill(EquipmentCalibrationAct) {
    SelectedEquipmentCalibrationActId = EquipmentCalibrationAct.Id;

    document.getElementById('REquipmentCalibrationActTypeErrorRequired').style.display = 'none';

    if (SelectedEquipmentCalibrationAct.EquipmentCalibrationType == 0) {
        document.getElementById('REquipmentCalibrationActTypeInternal').checked = true;
        document.getElementById('CmbEquipmentCalibrationActProviderRow').style.display = 'none';
    } else {
        document.getElementById('REquipmentCalibrationActTypeExternal').checked = true;
        document.getElementById('CmbEquipmentCalibrationActProviderRow').style.display = '';
    }

    // Controlar que sólo esté disponible el check activo
    if (SelectedEquipmentCalibrationAct.EquipmentCalibrationType == 0) {
        document.getElementById('REquipmentCalibrationActTypeExternal').disabled = !CalibrationExternalExists;
    }
    else {
        document.getElementById('REquipmentCalibrationActTypeInternal').disabled = !CalibrationInternalExists;
    }

    document.getElementById('REquipmentCalibrationActTypeErrorRequired').style.display = 'none';
    ClearFieldTextMessages('TxtEquipmentCalibrationActDate');
    ClearFieldTextMessages('TxtEquipmentCalibrationActCost');
    ClearFieldTextMessages('TxtEquipmentCalibrationActResult');
    ClearFieldTextMessages('CmbEquipmentCalibrationActProvider');
    ClearFieldTextMessages('CmbEquipmentCalibrationActResponsible');
    $('#TxtEquipmentCalibrationActDate').val(FormatYYYYMMDD(EquipmentCalibrationAct.Date, '/'));
    $('#TxtEquipmentCalibrationActObservations').val(EquipmentCalibrationAct.Observations);
    $('#TxtEquipmentCalibrationActResult').val(ToMoneyFormat(EquipmentCalibrationAct.Result, 2));
    if (EquipmentCalibrationAct.Cost !== null) {
        $('#TxtEquipmentCalibrationActCost').val(ToMoneyFormat(EquipmentCalibrationAct.Cost, 2));
    }
    else {
        $('#TxtEquipmentCalibrationActCost').val();
    }
    $('#CmbEquipmentCalibrationActProvider').val(EquipmentCalibrationAct.Provider.Id);
    $('#CmbEquipmentCalibrationActResponsible').val(EquipmentCalibrationAct.Responsible.Id);
    if ($('#CmbEquipmentCalibrationActResponsible').val() === null) {
        $('#CmbEquipmentCalibrationActResponsible').val(ApplicationUser.Employee.Id);
    }
}

function EquipmentCalibrationActValidateForm() {
    var ok = true;
    if (!document.getElementById('REquipmentCalibrationActTypeInternal').checked && !document.getElementById('REquipmentCalibrationActTypeExternal').checked) {
        ok = false;
        document.getElementById('REquipmentCalibrationActTypeErrorRequired').style.display = '';
    }
    else {
        document.getElementById('REquipmentCalibrationActTypeErrorRequired').style.display = 'none';
    }

    if (!RequiredFieldText('TxtEquipmentCalibrationActDate')) {
        ok = false;
    }

    if (!RequiredFieldText('TxtEquipmentCalibrationActResult')) {
        ok = false;
    }

    /* ISSUS-18
    if (!RequiredFieldText('TxtEquipmentCalibrationActCost')) {
        ok = false;
    }
    */

    if (!RequiredFieldCombo('CmbEquipmentCalibrationActProvider') && document.getElementById('REquipmentCalibrationActTypeExternal').checked) {
        ok = false;
    }

    if (!RequiredFieldCombo('CmbEquipmentCalibrationActResponsible')) {
        ok = false;
    }

    return ok;
}

function FillCmbEquipmentCalibrationActProvider() {
    VoidTable('CmbEquipmentCalibrationActProvider');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    document.getElementById('CmbEquipmentCalibrationActProvider').appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement('option');
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        document.getElementById('CmbEquipmentCalibrationActProvider').appendChild(option);
    }
}

function FillCmbEquipmentCalibrationActResponsible() {
    VoidTable('CmbEquipmentCalibrationActResponsible');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    document.getElementById('CmbEquipmentCalibrationActResponsible').appendChild(optionDefault);

    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true) {
            var option = document.createElement('option');
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].Value));
            document.getElementById('CmbEquipmentCalibrationActResponsible').appendChild(option);
        }
    }

    $('#CmbEquipmentCalibrationActResponsible').val(ApplicationUser.Employee.Id);
}

function REquipmentCalibrationActTypeChanged() {
    if (document.getElementById('REquipmentCalibrationActTypeExternal').checked === true) {
        document.getElementById('CmbEquipmentCalibrationActProviderRow').style.display = '';
    } else {
        document.getElementById('CmbEquipmentCalibrationActProviderRow').style.display = 'none';
    }
}

function ShowDialogNewCalibrationPopup(actionSelected) {
    FillCmbEquipmentCalibrationActProvider();
    FillCmbEquipmentCalibrationActResponsible();
    EquipmentCalibrationActNewFormReset();

    if (actionSelected == "-1") {
        document.getElementById('REquipmentCalibrationActTypeInternal').checked = true;
        document.getElementById('REquipmentCalibrationActTypeExternal').disabled = true;
    }

    if (actionSelected == "-2") {
        document.getElementById('REquipmentCalibrationActTypeExternal').checked = true;
        document.getElementById('REquipmentCalibrationActTypeInternal').disabled = true;
        document.getElementById('CmbEquipmentCalibrationActProviderRow').style.display = '';
        $('#CmbEquipmentCalibrationActProvider').val($('#CmbCalibrationExternalProvider').val());
    }

    if (!document.getElementById('CalibrationInternalActive').checked && !document.getElementById('CalibrationExternalActive').checked) {
        warningInfoUI(Dictionary.Common_Error_NoCalibrationsDefined, null);
        return false;
    }

    var dialog = $("#dialogEquipmentCalibrationForm").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_EquipmentCalibrationAct_PopupNew_Title + '</h4></div>',
        title_html: true,
        width: 400,
        buttons: [
                        {
                            id: 'BtnNewAddresSave',
                            html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Add,
                            "class": "btn btn-success btn-xs",
                            click: function () {
                                EquipmentCalibrationSave();
                            }
                        },
                        {
                            html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                            "class": "btn btn-xs",
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]

    });
}

function EquipmentCalibrationSave() {
    var ok = EquipmentCalibrationActValidateForm();
    if (ok === false) { return false; }

    var expiration = GetDate($('#TxtEquipmentCalibrationActDate').val(), '-');
    var range = document.getElementById('REquipmentCalibrationActTypeInternal').checked ? ($('#TxtCalibrationInternalPeriodicity').val() * 1) : ($('#TxtCalibrationExternalPeriodicity').val() * 1);
    var uncertainty = document.getElementById('REquipmentCalibrationActTypeInternal').checked ? (ParseInputValueToNumber($('#TxtCalibrationInternalUncertainty').val())) : (ParseInputValueToNumber($('#TxtCalibrationExternalUncertainty').val()));
    SelectedEquipmentCalibrationActOperation = document.getElementById('REquipmentCalibrationActTypeInternal').checked ? ($('#TxtCalibrationInternalOperation').val()) : ($('#TxtCalibrationExternalOperation').val());
    expiration.setDate(expiration.getDate() + range * 1);

    var cost = null;
    if ($('#TxtEquipmentCalibrationActCost').val() !== '') {
        cost = ParseInputValueToNumber($('#TxtEquipmentCalibrationActCost').val());
    }
    var result = ParseInputValueToNumber($('#TxtEquipmentCalibrationActResult').val());
    
    SelectedEquipmentCalibrationAct = {
        Id: SelectedEquipmentCalibrationActId,
        CompanyId: Company.Id,
        EquipmentId: Equipment.Id,
        EquipmentCalibrationType: document.getElementById('REquipmentCalibrationActTypeInternal').checked ? 0 : 1,
        Result: result,
        MaxResult: uncertainty,
        Description: SelectedEquipmentCalibrationActOperation,
        Date: GetDate($('#TxtEquipmentCalibrationActDate').val(), '-'),
        Expiration: expiration,
        Active: true,
        Cost: cost,
        Provider: { Id: $('#CmbEquipmentCalibrationActProvider').val() },
        Responsible: {
            Id: $('#CmbEquipmentCalibrationActResponsible').val(),
            Value: $("#CmbEquipmentCalibrationActResponsible option:selected").text()
        }
    };

    if (SelectedEquipmentCalibrationActId < 1) {
        var webMethod = "/Async/EquipmentCalibrationActActions.asmx/Insert";
        var data = { equipmentCalibrationAct: SelectedEquipmentCalibrationAct, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                SelectedEquipmentCalibrationAct.Id = msg.d.MessageError * 1;
                EquipmentCalibrationActList.push(SelectedEquipmentCalibrationAct);
                EquipmentCalibrationActRenderTable('TableEquipmentCalibrationAct');
                $('#dialogEquipmentCalibrationForm').dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var webMethod = "/Async/EquipmentCalibrationActActions.asmx/Update";
        var data = { equipmentCalibrationAct: SelectedEquipmentCalibrationAct, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                EquipmentCalibrationActListUpdate(SelectedEquipmentCalibrationAct);
                EquipmentCalibrationActRenderTable('TableEquipmentCalibrationAct');
                $('#dialogEquipmentCalibrationForm').dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function CalibrationInternalSave() {
    var validationResult = CalibrationInternalValidateForm();
    if (validationResult != "") {
        warningInfoUI(validationResult, null, 300);
        return false;
    }

    EquipmentCalibrationInternalDefinitionSave();
}

function CalibrationExternalSave() {
    var validationResult = CalibrationExternalValidateForm();
    if (validationResult != "") {
        warningInfoUI(validationResult, null, 300);
        return false;
    }

    EquipmentCalibrationExternalDefinitionSave();
}

function CalibrationInternalValidateForm() {
    var ok = true;
    if (!RequiredFieldText('TxtCalibrationInternalOperation')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationInternalPeriodicity')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationInternalUncertainty')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationInternalRange')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationInternalPattern')) {
        ok = false;
    }

    /* ISSUS-18
    if (!RequiredFieldText('TxtCalibrationInternalCost')) {
        ok = false;
    }
    */

    if (!RequiredFieldCombo('CmbCalibrationInternalResponsible')) {
        ok = false;
    }

    if (ok === false) {
        return Dictionary.Common_Form_Errors;
    }

    return "";
}

function CalibrationExternalValidateForm() {
    var ok = true;
    if (!RequiredFieldText('TxtCalibrationExternalOperation')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationExternalPeriodicity')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationExternalUncertainty')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationExternalRange')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtCalibrationExternalPattern')) {
        ok = false;
    }

    /* ISSUS-19 
    if (!RequiredFieldText('TxtCalibrationExternalPattern')) {
        ok = false;
    }
    */

    /* ISSUS-18
    if (!RequiredFieldText('TxtCalibrationExternalCost')) {
        ok = false;
    }
    */

    if (!RequiredFieldCombo('CmbCalibrationExternalResponsible')) {
        ok = false;
    }
    if (!RequiredFieldCombo('CmbCalibrationExternalProvider')) {
        ok = false;
    } 

    if (ok === false) {
        return Dictionary.Common_Form_Errors;
    }

    return '';
}

function EquipmentCalibrationInternalDefinitionSave() {
    var uncertainty = StringToNumber($('#TxtCalibrationInternalUncertainty').val(), '.', ',');
    var cost = StringToNumber($('#TxtCalibrationInternalCost').val(), '.', ',');
    CalibrationInternalDefinition = {
        Id: Equipment.InternalCalibration == null ? 0 : Equipment.InternalCalibration.Id,
        EquipmentId: Equipment.Id,
        CompanyId: Equipment.CompanyId,
        CalibrationType: 0,
        Description: $('#TxtCalibrationInternalOperation').val(),
        Periodicity: ParseInputValueToNumber($('#TxtCalibrationInternalPeriodicity').val()),
        Uncertainty: uncertainty,
        Range: $('#TxtCalibrationInternalRange').val(),
        Pattern: $('#TxtCalibrationInternalPattern').val(),
        Cost: cost,
        Notes: $('#TxtCalibrationInternalNotes').val(),
        Provider: null,
        Responsible: { Id: $('#CmbCalibrationInternalResponsible').val() }
    }

    if (Equipment.InternalCalibration.Id < 1) {
        var webMethod = "/Async/EquipmentCalibrationDefinitionActions.asmx/Insert";
        var data = { equipmentCalibrationDefinition: CalibrationInternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                CalibrationInternalDefinition.Id = msg.d.MessageError * 1;
                Equipment.InternalCalibration = CalibrationInternalDefinition;
                CalibrationInternalExists = true;
                ShowNewCalibrationButton();
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var webMethod = "/Async/EquipmentCalibrationDefinitionActions.asmx/Update";
        var data = { equipmentCalibrationDefinition: CalibrationInternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                Equipment.InternalCalibration = CalibrationInternalDefinition;
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }

}

function EquipmentCalibrationExternalDefinitionSave() {
    var uncertainty = StringToNumber($('#TxtCalibrationExternalUncertainty').val(), '.', ',');
    var cost = StringToNumber($('#TxtCalibrationExternalCost').val(), '.', ',');
    CalibrationExternalDefinition = {
        Id: Equipment.ExternalCalibration == null ? 0 : Equipment.ExternalCalibration.Id,
        EquipmentId: Equipment.Id,
        CompanyId: Equipment.CompanyId,
        CalibrationType: 1,
        Description: $('#TxtCalibrationExternalOperation').val(),
        Periodicity: ParseInputValueToNumber($('#TxtCalibrationExternalPeriodicity').val()),
        Uncertainty: uncertainty,
        Range: $('#TxtCalibrationExternalRange').val(),
        Pattern: $('#TxtCalibrationExternalPattern').val(),
        Cost: cost,
        Notes: $('#TxtCalibrationExternalNotes').val(),
        Provider: { Id: $('#CmbCalibrationExternalProvider').val() },
        Responsible: { Id: $('#CmbCalibrationExternalResponsible').val() }
    }

    if (Equipment.ExternalCalibration.Id < 1) {
        var webMethod = "/Async/EquipmentCalibrationDefinitionActions.asmx/Insert";
        var data = { equipmentCalibrationDefinition: CalibrationExternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                CalibrationExternalDefinition.Id = msg.d.MessageError * 1;
                Equipment.ExternalCalibration = CalibrationExternalDefinition;
                CalibrationExternalExists = true;
                ShowNewCalibrationButton();
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var webMethod = "/Async/EquipmentCalibrationDefinitionActions.asmx/Update";
        var data = { equipmentCalibrationDefinition: CalibrationExternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                Equipment.ExternalCalibration = CalibrationExternalDefinition;
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function CalibrationInternalDelete() {
    var webMethod = "/Async/EquipmentCalibrationDefinitionActions.asmx/Delete";
    var data = { equipmentCalibrationDefinitionId: Equipment.InternalCalibration.Id, companyId: Equipment.CompanyId, userId: user.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            if (msg.d.MessageError == '') {
                CalibrationInternalExists = false;
                Equipment.InternalCalibration = null;
                CalibrationInternalResetForm();
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function CalibrationExternalDelete() {
    var webMethod = "/Async/EquipmentCalibrationDefinitionActions.asmx/Delete";
    var data = { equipmentCalibrationDefinitionId: Equipment.ExternalCalibration.Id, companyId: Equipment.CompanyId, userId: user.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            if (msg.d.MessageError == '') {
                CalibrationExternalExists = false;
                Equipment.ExternalCalibration = null;
                CalibrationExternalResetForm();
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}