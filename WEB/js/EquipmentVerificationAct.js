var SelectedEquipmentVerificationAct;
var SelectedEquipmentVerificationActId;
var SelectedEquipmentVerificationActOperation;
var VerificationInternalDefinition;
var VerificationExternalDefinition;

var VerificationInternalActive = false;
var VerificationExternalActive = false;
var VerificationInternalExists = false;
var VerificationExternalExists = false;

function SetVerificationForm() {
    document.getElementById('VerificationInternalActive').checked = this.Equipment.InternalVerification != null;
    LockInternalVerificationForm(document.getElementById('VerificationInternalActive').checked);

    document.getElementById('VerificationExternalActive').checked = this.Equipment.ExternalVerification != null;
    LockExternalVerificationForm(document.getElementById('VerificationExternalActive').checked);

    if (!document.getElementById('VerificationInternalActive').checked && !document.getElementById('VerificationExternalActive').checked) {
        document.getElementById('VerificationWarning').style.display = '';
        document.getElementById('VerificationDivTable').style.display = 'none';
    }
    else {
        document.getElementById('VerificationWarning').style.display = 'none';
        document.getElementById('VerificationDivTable').style.display = '';
    }
}

function VerificationInternalResetForm() {
    $('#TxtVerificationInternalOperation').val('');
    $('#TxtVerificationInternalPeriodicity').val('');
    $('#TxtVerificationInternalUncertainty').val('');
    $('#TxtVerificationInternalRange').val('');
    $('#TxtVerificationInternalPattern').val('');
    $('#TxtVerificationInternalCost').val('');
    $('#CmbVerificationInternalResponsible').val('');
    $('#TxtVerificationInternalNotes').val('');
    $('#BtnVerificationInternalSave').val('');
    VerificationInternalSetForm();
}

function VerificationExternalResetForm() {
    $('#TxtVerificationExternalOperation').val('');
    $('#TxtVerificationExternalPeriodicity').val('');
    $('#TxtVerificationExternalUncertainty').val('');
    $('#TxtVerificationExternalRange').val('');
    $('#TxtVerificationExternalPattern').val('');
    $('#TxtVerificationExternalCost').val('');
    $('#CmbVerificationExternalResponsible').val('');
    $('#TxtVerificationExternalNotes').val('');
    $('#BtnVerificationExternalSave').val('');
    $('#CmbVerificationExternalProvider').val('');
    VerificationExternalSetForm();
}

function VerificationInternalSetForm() {
    var active = document.getElementById('VerificationInternalActive').checked;
    $('#TxtVerificationInternalOperation').prop('readonly', !active);
    $('#TxtVerificationInternalPeriodicity').prop('readonly', !active);
    $('#TxtVerificationInternalUncertainty').prop('readonly', !active);
    $('#TxtVerificationInternalRange').prop('readonly', !active);
    $('#TxtVerificationInternalPattern').prop('readonly', !active);
    $('#TxtVerificationInternalCost').prop('readonly', !active);
    $('#CmbVerificationInternalResponsible').prop('disabled', !active);
    $('#CmbVerificationInternalResponsible').val(active ? ApplicationUser.Employee.Id : '');
    $('#TxtVerificationInternalNotes').prop('readonly', !active);
    $('#BtnVerificationInternalSave').prop('disabled', !active);
    VerificationInternalActive = active;
    VerificationInternalExists = Equipment.InternalVerification != null && Equipment.InternalVerification.Id > 0;
    ShowNewVerificationButton();
}

function VerificationExternalSetForm() {
    var active = document.getElementById('VerificationExternalActive').checked;
    $('#TxtVerificationExternalOperation').prop('readonly', !active);
    $('#TxtVerificationExternalPeriodicity').prop('readonly', !active);
    $('#TxtVerificationExternalUncertainty').prop('readonly', !active);
    $('#TxtVerificationExternalRange').prop('readonly', !active);
    $('#TxtVerificationExternalPattern').prop('readonly', !active);
    $('#TxtVerificationExternalCost').prop('readonly', !active);
    $('#CmbVerificationExternalResponsible').prop('disabled', !active);
    $('#CmbVerificationExternalResponsible').val(ApplicationUser.Employee.Id);
    $('#CmbVerificationExternalProvider').prop('disabled', !active);
    $('#CmbVerificationExternalProvider').val(0);
    $('#TxtVerificationExternalNotes').prop('readonly', !active);
    $('#BtnVerificationExternalProviderBAR').prop('disabled', !active);
    $('#BtnVerificationExternalSave').prop('disabled', !active);
    VerificationExternalActive = active;
    VerificationExternalExists = Equipment.ExternalVerification != null && Equipment.ExternalVerification.Id > 0;
    ShowNewVerificationButton();
}

function VerificationInternalCheckActive() {
    document.getElementById('VerificationInternalActive').checked = true;
    return false;
}

function VerificationExternalCheckActive() {
    document.getElementById('VerificationExternalActive').checked = true;
    return false;
}

function LockInternalVerificationForm(active) {
    if (active === false && this.Equipment.InternalVerification !== null && this.Equipment.InternalVerification.Id === 0) {
        VerificationInternalResetForm();
        return;
    }

    if (active === false && this.Equipment.InternalVerification === null) {
        VerificationInternalResetForm();
        return;
    }

    if (active === false && VerificationExternalExists === false) {
        try {
            warningInfoUI(Dictionary.Common_Error_LastActive, null, 400);
            document.getElementById('VerificationInternalActive').checked = true;
            return false;
        }
        catch (ex) { }
    }

    if (active === false && this.Equipment.InternalVerification != null && this.Equipment.InternalVerification.Id > 0) {
        promptInfoUI("seguro", 300, VerificationInternalDelete, VerificationInternalCheckActive);
    }
    else {
        VerificationInternalSetForm();
    }
}

function LockExternalVerificationForm(active) {
    if (active === false && this.Equipment.ExternalVerification !== null && this.Equipment.ExternalVerification.Id === 0) {
        VerificationExternalResetForm();
        return;
    }

    if (active === false && this.Equipment.ExternalVerification === null) {
        VerificationExternalResetForm();
        return;
    }

    if (active === false && VerificationInternalExists === false) {
        try {
            warningInfoUI(Dictionary.Common_Error_LastActive, null, 400);
            document.getElementById('VerificationExternalActive').checked = true;
            return false;
        }
        catch (ex) { }
    }

    if (active === false && this.Equipment.ExternalVerification != null && this.Equipment.ExternalVerification.Id > 0) {
        promptInfoUI("seguro", 300, VerificationExternalDelete, VerificationExternalCheckActive);
    }
    else {
        VerificationExternalSetForm();
    }
}

function ShowNewVerificationButton() {
    if (VerificationInternalExists === true || VerificationExternalExists === true) {
        document.getElementById('BtnNewVerification').style.display = '';
        document.getElementById('VerificationWarning').style.display = 'none';
    }
    else {
        document.getElementById('BtnNewVerification').style.display = 'none';
        document.getElementById('VerificationWarning').style.display = '';
    }
}

function EquipmentVerificationActRenderTable(targetName) {
    VoidTable(targetName);
    var target = document.getElementById(targetName);
    var total = 0;
    for (var x = 0; x < EquipmentVerificationActList.length; x++) {
        if (x === 2) {
            break;
        }
        if (EquipmentVerificationActList[x].Active === true) {
            total += EquipmentVerificationActRenderRow(EquipmentVerificationActList[x], targetName);
        }
    }

    $("#TableEquipmentVerificationActTotalLabel").html("<i style=\"color:#aaa;font-weight:bold;\">" + Dictionary.Common_RegisterCount + ":&nbsp;" + EquipmentVerificationActList.length + "</i><span style=\"float:right\">" + Dictionary.Common_Total + ":</span>");
    document.getElementById('TableEquipmentVerificationActTotal').innerHTML = ToMoneyFormat(total, 2);
    if ($("#VerificationDivTable #th0").attr('class') === "sort  ASC") {
        $("#VerificationDivTable #th0").click();
    }
    else {
        $("#VerificationDivTable #th0").click();
        $("#VerificationDivTable #th0").click();
    }
}

function EquipmentVerificationActRenderRow(equipmentVerificationAct, targetName) {
    var target = document.getElementById(targetName);
    var row = document.createElement('TR');
    var tdFecha = document.createElement('TD');
    var tdType = document.createElement('TD');
    var tdResult = document.createElement('TD');
    var tdMax = document.createElement('TD');
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

    if (equipmentVerificationAct.Result > equipmentVerificationAct.MaxResult) {
        tdResult.style.color = '#f00';
    }

    tdMax.appendChild(document.createTextNode(ToMoneyFormat(equipmentVerificationAct.MaxResult, 6)));
    tdResult.appendChild(document.createTextNode(ToMoneyFormat(equipmentVerificationAct.Result, 6)));
    // ISSUS-18
    if (equipmentVerificationAct.Cost !== null) {
        tdCost.appendChild(document.createTextNode(ToMoneyFormat(equipmentVerificationAct.Cost, 2)));
    }
    tdType.appendChild(document.createTextNode(equipmentVerificationAct.EquipmentVerificationType == 0 ? Dictionary.Common_Internal : Dictionary.Common_External));
    tdFecha.appendChild(document.createTextNode(FormatYYYYMMDD(equipmentVerificationAct.Date, '/')));

    var vto = null;
    if (typeof equipmentVerificationAct.Expiration != "undefined") { vto = FormatYYYYMMDD(equipmentVerificationAct.Expiration, '/'); }
    if (typeof equipmentVerificationAct.Vto != "undefined") { vto = FormatYYYYMMDD(equipmentVerificationAct.Vto, '/'); }

    tdVto.appendChild(document.createTextNode(vto));
    tdResponsible.appendChild(document.createTextNode(equipmentVerificationAct.Responsible.Value));

    row.id = 'EquipmentVerificationAct' + equipmentVerificationAct.Id;
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
        iconEdit.onclick = function (e) { EquipmentVerificationActEdit(this) }
        var innerEdit = document.createElement('I');
        innerEdit.className = 'icon-edit bigger-120';
        iconEdit.appendChild(innerEdit);

        iconDelete.className = 'btn btn-xs btn-danger';
        iconDelete.title = Dictionary.Common_Delete;
        iconDelete.onclick = function (e) { EquipmentVerificationActDelete(this) }
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
    return equipmentVerificationAct.Cost * 1;
}

function EquipmentVerificationActgetById(id) {
    for (var x = 0; x < EquipmentVerificationActList.length; x++) {
        if (EquipmentVerificationActList[x].Id == id) {
            return EquipmentVerificationActList[x];
        }
    }

    return null;
}

function EquipmentVerificationActRemoveFromList(id) {
    var temp = new Array();

    for (var x = 0; x < EquipmentVerificationActList.length; x++) {
        if (EquipmentVerificationActList[x].Id != id) {
            temp.push(EquipmentVerificationActList[x]);
        }
    }

    EquipmentVerificationActList = new Array();

    for (var x = 0; x < temp.length; x++) {
        EquipmentVerificationActList.push(temp[x]);
    }
}

function EquipmentVerificationActListUpdate(equipmentVerificationAct) {
    for (var x = 0; x < EquipmentVerificationActList.length; x++) {
        if (EquipmentVerificationActList[x].Id == equipmentVerificationAct.Id) {
            EquipmentVerificationActList[x] = equipmentVerificationAct;
        }
    }
}

function EquipmentVerificationActNewFormReset(EquipmentVerificationDefinition) {
    document.getElementById('TxtEquipmentVerificationActCostMalformed').style.display = 'none';
    document.getElementById('REquipmentVerificationActTypeErrorRequired').style.display = 'none';
    var internal = document.getElementById('VerificationInternalActive').checked;
    var external = document.getElementById('VerificationExternalActive').checked;

    document.getElementById('REquipmentVerificationActTypeInternal').disabled = !VerificationInternalExists;
    document.getElementById('REquipmentVerificationActTypeExternal').disabled = !VerificationExternalExists;

    var onlyExternal = false;
    if (!internal === true || !external === true) {
        if (external === true) {
            onlyExternal = true;
            document.getElementById('REquipmentVerificationActTypeExternal').checked = true;
        }
        else {
            document.getElementById('REquipmentVerificationActTypeInternal').checked = true;
        }
    }
    else {
        document.getElementById('REquipmentVerificationActTypeInternal').checked = false;
        document.getElementById('REquipmentVerificationActTypeExternal').checked = false;
    }

    SelectedEquipmentVerificationActId = -1;
    document.getElementById('REquipmentVerificationActTypeErrorRequired').style.display = 'none';
    ClearFieldTextMessages('TxtEquipmentVerificationActResult');
    ClearFieldTextMessages('TxtEquipmentVerificationActDate');
    ClearFieldTextMessages('TxtEquipmentVerificationActCost');
    ClearFieldTextMessages('CmbEquipmentVerificationActProvider');
    ClearFieldTextMessages('CmbEquipmentVerificationActResponsible');
    $('#TxtEquipmentVerificationActDate').val('');
    $('#TxtEquipmentVerificationActResult').val('');
    $('#TxtEquipmentVerificationActObservations').val('');
    $('#TxtEquipmentVerificationActCost').val('');
    $('#CmbEquipmentVerificationActResponsible').val(ApplicationUser.Employee.Id);
    $('#TxtEquipmentVerificationActDate').val(FormatDate(new Date, '/'));
    if (onlyExternal === true) {
        document.getElementById('CmbEquipmentVerificationActProviderRow').style.display = '';
        $('#CmbEquipmentVerificationActProvider').val($('#CmbVerificationExternalProvider').val());
    }
    else {
        document.getElementById('CmbEquipmentVerificationActProviderRow').style.display = 'none';
    }
}

function EquipmentVerificationActEditFormFill(EquipmentVerificationAct) {
    SelectedEquipmentActSelectedId = EquipmentVerificationAct.Id;
    document.getElementById('TxtEquipmentVerificationActCostMalformed').style.display = 'none';
    document.getElementById('REquipmentVerificationActTypeErrorRequired').style.display = 'none';

    if (SelectedEquipmentVerificationAct.EquipmentVerificationType == 0) {
        document.getElementById('REquipmentVerificationActTypeInternal').checked = true;
        document.getElementById('CmbEquipmentVerificationActProviderRow').style.display = 'none';
    } else {
        document.getElementById('REquipmentVerificationActTypeExternal').checked = true;
        document.getElementById('CmbEquipmentVerificationActProviderRow').style.display = '';
    }

    // Controlar que sólo esté disponible el check activo
    if (SelectedEquipmentVerificationAct.EquipmentVerificationType == 0) {
        document.getElementById('REquipmentVerificationActTypeExternal').disabled = !VerificationExternalExists;
    }
    else {
        document.getElementById('REquipmentVerificationActTypeInternal').disabled = !VerificationInternalExists;
    }

    document.getElementById('REquipmentVerificationActTypeErrorRequired').style.display = 'none';
    ClearFieldTextMessages('TxtEquipmentVerificationActResult');
    ClearFieldTextMessages('TxtEquipmentVerificationActDate');
    ClearFieldTextMessages('TxtEquipmentVerificationActCost');
    ClearFieldTextMessages('CmbEquipmentVerificationActProvider');
    ClearFieldTextMessages('CmbEquipmentVerificationActResponsible');
    $('#TxtEquipmentVerificationActDate').val(FormatYYYYMMDD(EquipmentVerificationAct.Date, '/'));
    $('#TxtEquipmentVerificationActObservations').val(EquipmentVerificationAct.Observations);
    $('#TxtEquipmentVerificationActResult').val(ToMoneyFormat(EquipmentVerificationAct.Result, 2));
    if (EquipmentVerificationAct.Cost !== null) {
        $('#TxtEquipmentVerificationActCost').val(ToMoneyFormat(EquipmentVerificationAct.Cost, 2));
    } else {
        $('#TxtEquipmentVerificationActCost').val();
    }
    $('#CmbEquipmentVerificationActProvider').val(EquipmentVerificationAct.Provider.Id);
    $('#CmbEquipmentVerificationActResponsible').val(EquipmentVerificationAct.Responsible.Id);
    if ($('#CmbEquipmentVerificationActResponsible').val() === null) {
        $('#CmbEquipmentVerificationActResponsible').val(ApplicationUser.Employee.Id);
    }    
}

function EquipmentVerificationActValidateForm() {
    var ok = true;
    if (!document.getElementById('REquipmentVerificationActTypeInternal').checked && !document.getElementById('REquipmentVerificationActTypeExternal').checked) {
        ok = false;
        document.getElementById('REquipmentVerificationActTypeErrorRequired').style.display = '';
    }
    else {
        document.getElementById('REquipmentVerificationActTypeErrorRequired').style.display = 'none';
    }

    if (!RequiredFieldText('TxtEquipmentVerificationActDate')) {
        ok = false;
    }

    if (!RequiredFieldText('TxtEquipmentVerificationActResult')) {
        ok = false;
    }

    /* ISSUS-18
    if (!RequiredFieldText('TxtEquipmentVerificationActCost')) {
        ok = false;
    }
    */

    if (!RequiredFieldCombo('CmbEquipmentVerificationActProvider') && document.getElementById('REquipmentVerificationActTypeExternal').checked) {
        ok = false;
    }

    if (!RequiredFieldCombo('CmbEquipmentVerificationActResponsible')) {
        ok = false;
    }

    return ok;
}

function FillCmbEquipmentVerificationActProvider() {
    VoidTable('CmbEquipmentVerificationActProvider');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    document.getElementById('CmbEquipmentVerificationActProvider').appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement('option');
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        document.getElementById('CmbEquipmentVerificationActProvider').appendChild(option);
    }
}

function FillCmbEquipmentVerificationActResponsible() {
    VoidTable('CmbEquipmentVerificationActResponsible');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    document.getElementById('CmbEquipmentVerificationActResponsible').appendChild(optionDefault);

    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true) {
            var option = document.createElement('option');
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            document.getElementById('CmbEquipmentVerificationActResponsible').appendChild(option);
        }
    }

    $('#CmbEquipmentVerificationActResponsible').val(ApplicationUser.Employee.Id);
}

function REquipmentVerificationActTypeChanged() {
    if (document.getElementById('REquipmentVerificationActTypeExternal').checked === true) {
        document.getElementById('CmbEquipmentVerificationActProviderRow').style.display = '';
    } else {
        document.getElementById('CmbEquipmentVerificationActProviderRow').style.display = 'none';
    }
}

function ShowDialogEquipmentVerificacionPopup(actionSelected) {
    FillCmbEquipmentVerificationActProvider();
    FillCmbEquipmentVerificationActResponsible();
    EquipmentVerificationActNewFormReset();

    if (actionSelected == "-1") {
        document.getElementById('REquipmentVerificationActTypeInternal').checked = true;
        document.getElementById('REquipmentVerificationActTypeExternal').disabled = true;
    }

    if (actionSelected == "-2") {
        document.getElementById('REquipmentVerificationActTypeExternal').checked = true;
        document.getElementById('REquipmentVerificationActTypeInternal').disabled = true;
    }

    if (!document.getElementById('VerificationInternalActive').checked && !document.getElementById('VerificationExternalActive').checked) {
        warningInfoUI(Dictionary.Common_Error_NoVerificationsDefined, null);
        return false;
    }

    var dialog = $("#dialogEquipmentVerificacionForm").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_EquipmentVerification_PopupNew_Title + '</h4>',
        title_html: true,
        width: 400,
        buttons: [
                        {
                            id: 'BtnNewAddresSave',
                            html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Add,
                            "class": "btn btn-success btn-xs",
                            click: function () {
                                EquipmentVerificationSave();
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

function EquipmentVerificationSave() {
    var ok = EquipmentVerificationActValidateForm();
    if (ok === false) { return false; }

    var expiration = GetDate($('#TxtEquipmentVerificationActDate').val(), '-');
    var range = document.getElementById('REquipmentVerificationActTypeInternal').checked ? ($('#TxtVerificationInternalPeriodicity').val() * 1) : ($('#TxtVerificationExternalPeriodicity').val() * 1);
    var uncertainty = document.getElementById('REquipmentVerificationActTypeInternal').checked ? (ParseInputValueToNumber($('#TxtVerificationInternalUncertainty').val())) : (ParseInputValueToNumber($('#TxtVerificationExternalUncertainty').val()));
    SelectedEquipmentVerificationActOperation = document.getElementById('REquipmentVerificationActTypeInternal').checked ? ($('#TxtVerificationInternalOperation').val()) : ($('#TxtVerificationExternalOperation').val());
    expiration.setDate(expiration.getDate() + range * 1);

    var cost = null;
    if ($('#TxtEquipmentVerificationActCost').val() !== '') {
        cost = ParseInputValueToNumber($('#TxtEquipmentVerificationActCost').val());
    }
    var result = ParseInputValueToNumber($('#TxtEquipmentVerificationActResult').val());

    SelectedEquipmentVerificationAct = {
        Id: SelectedEquipmentVerificationActId,
        CompanyId: Company.Id,
        EquipmentId: Equipment.Id,
        EquipmentVerificationType: document.getElementById('REquipmentVerificationActTypeInternal').checked ? 0 : 1,
        Result: result,
        MaxResult: uncertainty,
        Description: SelectedEquipmentVerificationActOperation,
        Date: GetDate($('#TxtEquipmentVerificationActDate').val(), '-'),
        Expiration: expiration,
        Active: true,
        Cost: cost,
        Provider: { Id: $('#CmbEquipmentVerificationActProvider').val() },
        Responsible: {
            Id: $('#CmbEquipmentVerificationActResponsible').val(),
            Value: $("#CmbEquipmentVerificationActResponsible option:selected").text()
        }
    };

    if (SelectedEquipmentVerificationActId < 1) {
        var webMethod = "/Async/EquipmentVerificationActActions.asmx/Insert";
        var data = { equipmentVerificationAct: SelectedEquipmentVerificationAct, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                SelectedEquipmentVerificationAct.Id = msg.d.MessageError * 1;
                EquipmentVerificationActList.push(SelectedEquipmentVerificationAct);
                EquipmentVerificationActRenderTable('TableEquipmentVerificationAct');
                $('#dialogEquipmentVerificacionForm').dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var webMethod = "/Async/EquipmentVerificationActActions.asmx/Update";
        var data = { equipmentVerificationAct: SelectedEquipmentVerificationAct, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                EquipmentVerificationActListUpdate(SelectedEquipmentVerificationAct);
                EquipmentVerificationActRenderTable('TableEquipmentVerificationAct');
                $('#dialogEquipmentVerificacionForm').dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function VerificationInternalSave() {
    var validationResult = VerificationInternalValidateForm();
    if (validationResult != '') {
        warningInfoUI(validationResult, null, 300);
        return false;
    }

    EquipmentVerificationInternalDefinitionSave();
}

function VerificationExternalSave() {
    var validationResult = VerificationExternalValidateForm();
    if (validationResult != '') {
        warningInfoUI(validationResult, null, 300);
        return false;
    }

    EquipmentVerificationExternalDefinitionSave();
}

function VerificationInternalValidateForm() {
    var ok = true;
    if (!RequiredFieldText('TxtVerificationInternalOperation')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtVerificationInternalPeriodicity')) {
        ok = false;
    }
    /*if (!RequiredFieldText('TxtVerificationInternalUncertainty')) {
        ok = false;
    }*/
    /*if (!RequiredFieldText('TxtVerificationInternalRange')) {
        ok = false;
    }*/
    /*if (!RequiredFieldText('TxtVerificationInternalPattern')) {
        ok = false;
    }*/

    /* ISSUS-18
    if (!RequiredFieldText('TxtVerificationInternalCost')) {
        ok = false;
    }
    */

    if (!RequiredFieldCombo('CmbVerificationInternalResponsible')) {
        ok = false;
    }

    if (ok === false) {
        return Dictionary.Common_Form_Errors;
    }

    return '';
}

function VerificationExternalValidateForm() {
    var ok = true;
    if (!RequiredFieldText('TxtVerificationExternalOperation')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtVerificationExternalPeriodicity')) {
        ok = false;
    }
    /*if (!RequiredFieldText('TxtVerificationExternalUncertainty')) {
        ok = false;
    }*/
    /*if (!RequiredFieldText('TxtVerificationExternalRange')) {
        ok = false;
    }*/
    /*if (!RequiredFieldText('TxtVerificationExternalPattern')) {
        ok = false;
    }*/

    /* ISSUS-18
    if (!RequiredFieldText('TxtVerificationExternalCost')) {
        ok = false;
    }
    */

    if (!RequiredFieldCombo('CmbVerificationExternalResponsible')) {
        ok = false;
    }
    if (!RequiredFieldCombo('CmbVerificationExternalProvider')) {
        ok = false;
    }

    if (ok === false) {
        return Dictionary.Common_Form_Errors;
    }

    return '';
}

function EquipmentVerificationInternalDefinitionSave() {
    var uncertainty = StringToNumberNullable($("#TxtVerificationInternalUncertainty").val(), ".", ",");
    var cost = StringToNumberNullable($("#TxtVerificationInternalCost").val(), ".", ",");
    VerificationInternalDefinition = {
        "Id": Equipment.InternalVerification == null ? 0 : Equipment.InternalVerification.Id,
        "EquipmentId": Equipment.Id,
        "CompanyId": Equipment.CompanyId,
        "VerificationType": 0,
        "Description": $("#TxtVerificationInternalOperation").val(),
        "Periodicity": ParseInputValueToNumber($("#TxtVerificationInternalPeriodicity").val()),
        "Uncertainty": uncertainty,
        "Range": $("#TxtVerificationInternalRange").val(),
        "Pattern": $("#TxtVerificationInternalPattern").val(),
        "Cost": cost,
        "Notes": $("#TxtVerificationInternalNotes").val(),
        "Provider": null,
        "Responsible": { Id: $("#CmbVerificationInternalResponsible").val() }
    }

    console.log("EquipmentVerificationInternalDefinitionSave", VerificationInternalDefinition);

    if (Equipment.InternalVerification.Id < 1) {
        var webMethod = "/Async/EquipmentVerificationDefinitionActions.asmx/Insert";
        var data = { equipmentVerificationDefinition: VerificationInternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                VerificationInternalDefinition.Id = msg.d.MessageError * 1;
                Equipment.InternalVerification = VerificationInternalDefinition;
                VerificationInternalExists = true;
                ShowNewVerificationButton();
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var webMethod = "/Async/EquipmentVerificationDefinitionActions.asmx/Update";
        var data = { equipmentVerificationDefinition: VerificationInternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                Equipment.InternalVerification = VerificationInternalDefinition;
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }

}

function EquipmentVerificationExternalDefinitionSave() {
    var uncertainty = StringToNumberNullable($('#TxtVerificationExternalUncertainty').val(), '.', ',');
    var cost = StringToNumberNullable($('#TxtVerificationExternalCost').val(), '.', ',');
    VerificationExternalDefinition = {
        Id: Equipment.ExternalVerification == null ? 0 : Equipment.ExternalVerification.Id,
        EquipmentId: Equipment.Id,
        CompanyId: Equipment.CompanyId,
        VerificationType: 1,
        Description: $('#TxtVerificationExternalOperation').val(),
        Periodicity: ParseInputValueToNumber($('#TxtVerificationExternalPeriodicity').val()),
        Uncertainty: uncertainty,
        Range: $('#TxtVerificationExternalRange').val(),
        Pattern: $('#TxtVerificationExternalPattern').val(),
        Cost: cost,
        Notes: $('#TxtVerificationExternalNotes').val(),
        Provider: { Id: $('#CmbVerificationExternalProvider').val() },
        Responsible: { Id: $('#CmbVerificationExternalResponsible').val() }
    }

    if (Equipment.ExternalVerification.Id < 1) {
        var webMethod = "/Async/EquipmentVerificationDefinitionActions.asmx/Insert";
        var data = { equipmentVerificationDefinition: VerificationExternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                VerificationExternalDefinition.Id = msg.d.MessageError * 1;
                Equipment.ExternalVerification = VerificationExternalDefinition;
                VerificationExternalExists = true;
                ShowNewVerificationButton();
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var webMethod = "/Async/EquipmentVerificationDefinitionActions.asmx/Update";
        var data = { equipmentVerificationDefinition: VerificationExternalDefinition, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                Equipment.ExternalVerification = VerificationExternalDefinition;
                successInfoUI(Dictionary.Common_Action_Success);
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function VerificationInternalDelete() {
    var webMethod = "/Async/EquipmentVerificationDefinitionActions.asmx/Delete";
    var data = { equipmentVerificationDefinitionId: Equipment.InternalVerification.Id, companyId: Equipment.CompanyId, userId: user.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            if (msg.d.MessageError == '') {
                VerificationInternalExists = false;
                Equipment.InternalVerification = null;
                VerificationInternalResetForm();
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function VerificationExternalDelete() {
    var webMethod = "/Async/EquipmentVerificationDefinitionActions.asmx/Delete";
    var data = { equipmentVerificationDefinitionId: Equipment.ExternalVerification.Id, companyId: Equipment.CompanyId, userId: user.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            if (msg.d.MessageError == '') {
                VerificationExternalExists = false;
                Equipment.ExternalVerification = null;
                VerificationExternalResetForm();
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}