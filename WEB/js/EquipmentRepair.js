var SelectedEquipmentRepair;
var SelectedEquipmentRepairId;

function EquipmentRepairRenderTable(targetName) {
    VoidTable(targetName);

    EquipmentRepairList.sort(function (a, b) {

        var da = a.Date;
        var db = b.Date;

        if (typeof da !== "object") {
            da = GetDateYYYYMMDDText(da, '/');
        }

        if (typeof db !== "object") {
            da = GetDateYYYYMMDDText(db, '/');

        }

        return db - da;
    });

    var target = document.getElementById(targetName);
    var total = 0; for (var x = 0; x < EquipmentRepairList.length; x++) {
        if (EquipmentRepairList[x].Active === true) {
            total += EquipmentRepairRenderRow(EquipmentRepairList[x], targetName);
        }
    }

    $("#TableEquipmentRepairTotalLabel").html("<i style=\"color:#aaa;font-weight:bold;\">" + Dictionary.Common_RegisterCount + ":&nbsp;" + EquipmentRepairList.length + "</i><span style=\"float:right\">" + Dictionary.Common_Total + ":</span>");
    document.getElementById('TableEquipmentRepairTotal').innerHTML = ToMoneyFormat(total, 2);

    if ($("#TableEquipmentRepairMain #th0").attr('class') === "sort  ASC") {
        $("#TableEquipmentRepairMain #th0").click();
    }
    else {
        $("#TableEquipmentRepairMain #th0").click();
        $("#TableEquipmentRepairMain #th0").click();
    }
}

function EquipmentRepairRenderRow(EquipmentRepair, targetName) {
    var target = document.getElementById(targetName);
    var row = document.createElement('TR');
    var tdFecha = document.createElement('TD');
    var tdDescription = document.createElement('TD');
    var tdObservaciones = document.createElement('TD');
    var tdCost = document.createElement('TD');
    var tdResponsible = document.createElement('TD');

    tdFecha.style.width = '90px';
    //tdDescription.style.width = '180px';
    tdCost.style.width = '70px';
    tdResponsible.style.width = '200px';

    tdFecha.align = 'center';
    tdCost.align = 'right';

    tdDescription.appendChild(document.createTextNode(EquipmentRepair.Description));
    tdFecha.appendChild(document.createTextNode(FormatYYYYMMDD(EquipmentRepair.Date, '/')));
    tdResponsible.appendChild(document.createTextNode(EquipmentRepair.Responsible.Value));

    if (typeof (EquipmentRepair.Observations) === 'undefined') {
        tdObservaciones.appendChild(document.createTextNode(''));
    }
    else {
        tdObservaciones.appendChild(document.createTextNode(EquipmentRepair.Observations));
    }

    if (EquipmentRepair.Cost !== null) {
        tdCost.appendChild(document.createTextNode(ToMoneyFormat(EquipmentRepair.Cost * 1, 2)));
    }

    row.id = 'EquipmentRepair' + EquipmentRepair.Id;
    row.appendChild(tdFecha);
    row.appendChild(tdDescription);
    //row.appendChild(tdObservaciones);
    row.appendChild(tdResponsible);
    row.appendChild(tdCost);

    if (GrantToWrite) {
        var tdActions = document.createElement('TD');
        var iconEdit = document.createElement('SPAN');
        var iconDelete = document.createElement('SPAN');

        iconEdit.className = 'btn btn-xs btn-info';
        iconEdit.title = Dictionary.Common_Edit;
        iconEdit.onclick = function (e) { EquipmentRepairEdit(this) }
        var innerEdit = document.createElement('I');
        innerEdit.className = 'icon-edit bigger-120';
        iconEdit.appendChild(innerEdit);

        iconDelete.className = 'btn btn-xs btn-danger';
        iconDelete.title = Dictionary.Common_Delete;
        iconDelete.onclick = function (e) { EquipmentRepairDelete(this) }
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
    if (EquipmentRepair.Cost === null) {
        return 0;
    }

    return EquipmentRepair.Cost * 1;
}

function EquipmentRepairgetById(id) {
    for (var x = 0; x < EquipmentRepairList.length; x++) {
        if (EquipmentRepairList[x].Id == id) {
            return EquipmentRepairList[x];
        }
    }

    return null;
}

function EquipmentRepairRemoveFromList(id) {
    var temp = new Array();

    for (var x = 0; x < EquipmentRepairList.length; x++) {
        if (EquipmentRepairList[x].Id != id) {
            temp.push(EquipmentRepairList[x]);
        }
    }

    EquipmentRepairList = new Array();

    for (var x = 0; x < temp.length; x++) {
        EquipmentRepairList.push(temp[x]);
    }
}

function EquipmentRepairListUpdate(EquipmentRepair) {
    for (var x = 0; x < EquipmentRepairList.length; x++) {
        if (EquipmentRepairList[x].Id == EquipmentRepair.Id) {
            EquipmentRepairList[x] = EquipmentRepair;
        }
    }
}

function EquipmentRepairFormClearErrors() {
    ClearFieldTextMessages('TxtEquipmentRepairDate');
    ClearFieldTextMessages('TxtEquipmentRepairCost');
    ClearFieldTextMessages('TxtEquipmentRepairDescription');
    ClearFieldTextMessages('CmbEquipmentRepairProvider');
    ClearFieldTextMessages('CmbEquipmentRepairResponsible');
    document.getElementById('CmbEquipmentRepairProviderRow').style.display = 'none';
    document.getElementById('REquipmentRepairTypeErrorRequired').style.display = 'none';
}

function EquipmentRepairNewFormReset() {
    SelectedEquipmentRepairId = -1;
    EquipmentRepairFormClearErrors();
    document.getElementById('REquipmentRepairTypeInternal').checked = true;
    $('#TxtEquipmentRepairDate').val('');
    $('#TxtEquipmentRepairDescription').val('');
    $('#TxtEquipmentRepairTools').val('');
    $('#TxtEquipmentRepairObservations').val('');
    $('#TxtEquipmentRepairCost').val('');
    $('#CmbEquipmentRepairProvider').val(0);
    $('#CmbEquipmentRepairResponsible').val(ApplicationUser.Employee.Id);
    $('#TxtEquipmentRepairDate').val(FormatDate(new Date(),'/'));
}

function EquipmentRepairEditFormFill(EquipmentRepair) {
    SelectedEquipmentRepairId = EquipmentRepair.Id;
    EquipmentRepairFormClearErrors();

    if (EquipmentRepair.RepairType === 0) {
        document.getElementById('REquipmentRepairTypeInternal').checked = true;
        document.getElementById('CmbEquipmentRepairProviderRow').style.display = 'none';
    } else {
        document.getElementById('REquipmentRepairTypeExternal').checked = true;
        document.getElementById('CmbEquipmentRepairProviderRow').style.display = '';
    }

    $('#TxtEquipmentRepairDate').val(FormatYYYYMMDD(EquipmentRepair.Date, '/'));
    $('#TxtEquipmentRepairDescription').val(EquipmentRepair.Description);
    $('#TxtEquipmentRepairTools').val(EquipmentRepair.Tools);
    $('#TxtEquipmentRepairObservations').val(EquipmentRepair.Observations);
    if (EquipmentRepair.Cost !== null) {
        $('#TxtEquipmentRepairCost').val(ToMoneyFormat(EquipmentRepair.Cost, 2));
    }
    else {
        $('#TxtEquipmentRepairCost').val();
    }
    $('#CmbEquipmentRepairProvider').val(EquipmentRepair.Provider.Id);
    $('#CmbEquipmentRepairResponsible').val(EquipmentRepair.Responsible.Id);
    if($('#CmbEquipmentRepairResponsible').val()===null)
    {
        $('#CmbEquipmentRepairResponsible').val(ApplicationUser.Employee.Id);
    }
}

function EquipmentRepairValidateForm() {
    EquipmentRepairFormClearErrors();
    var ok = true;
    if (!RequiredFieldText('TxtEquipmentRepairDate')) {
        ok = false;
    }
    /*if (!RequiredFieldText('TxtEquipmentRepairCost')) {
        ok = false;
    }*/
    if (!RequiredFieldCombo('CmbEquipmentRepairProvider') && document.getElementById('RMaintainmentTypeExternal').checked) {
        ok = false;
    }
    if (!RequiredFieldCombo('CmbEquipmentRepairResponsible')) {
        ok = false;
    }

    return true;
}

function FillCmbEquipmentRepairProvider() {
    VoidTable('CmbEquipmentRepairProvider');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById('CmbEquipmentRepairProvider').appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement('option');
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        document.getElementById('CmbEquipmentRepairProvider').appendChild(option);
    }
}

function FillCmbEquipmentRepairResponsible() {
    VoidTable('CmbEquipmentRepairResponsible');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById('CmbEquipmentRepairResponsible').appendChild(optionDefault);

    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true) {
            var option = document.createElement('option');
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].Value));
            document.getElementById('CmbEquipmentRepairResponsible').appendChild(option);
        }
    }
}

function dialogMaintaimentRepairTypeChanged() {
    if (document.getElementById('REquipmentRepairTypeInternal').checked === true) {
        document.getElementById('CmbEquipmentRepairProviderRow').style.display = 'none';
    }
    else {
        document.getElementById('CmbEquipmentRepairProviderRow').style.display = '';
    }
}

function dialogMaintaimentRepairFormValidate() {
    ClearFieldTextMessages('TxtEquipmentRepairDate');
    ClearFieldTextMessages('TxtEquipmentRepairCost');
    ClearFieldTextMessages('TxtEquipmentRepairDescription');
    ClearFieldTextMessages('CmbEquipmentRepairProvider');
    ClearFieldTextMessages('CmbEquipmentRepairResponsible');
    document.getElementById('REquipmentRepairTypeErrorRequired').style.display = 'none';
    var ok = true;
    if (!document.getElementById('REquipmentRepairTypeInternal').checked && !document.getElementById('REquipmentRepairTypeExternal').checked) {
        ok = false;
        document.getElementById('REquipmentRepairTypeErrorRequired').style.display = '';
    }
    if (!RequiredFieldText('TxtEquipmentRepairDate')) {
        ok = false;
    }
    /*if (!RequiredFieldText('TxtEquipmentRepairCost')) {
        ok = false;
    }*/
    if (!RequiredFieldText('TxtEquipmentRepairDescription')) {
        ok = false;
    }
    if (document.getElementById('REquipmentRepairTypeExternal').checked) {

        if (!RequiredFieldCombo('CmbEquipmentRepairProvider')) {
            ok = false;
        }
    }
    if (!RequiredFieldCombo('CmbEquipmentRepairResponsible')) {
        ok = false;
    }
    return ok;
}

function EquipmentRepairEdit(sender) {
    SelectedEquipmentRepairId = sender.parentNode.parentNode.id.substring(15) * 1;
    SelectedEquipmentRepair = EquipmentRepairgetById(SelectedEquipmentRepairId);
    if (SelectedEquipmentRepair == null) { return false; }
    FillCmbEquipmentRepairProvider();
    FillCmbEquipmentRepairResponsible();
    EquipmentRepairEditFormFill(SelectedEquipmentRepair);
    var dialog = $("#dialogEquipmentRepairForm").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: "<h4 class='smaller'>" + Dictionary.Item_EquipmentRepair_PopupUpdate_Title + "</h4>",
        title_html: true,
        width: 500,
        buttons: [
            {
                id: 'BtnNewAddresSave',
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Modify,
                "class": "btn btn-success btn-xs",
                click: function () {
                    EquipmentRepairSave();
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

function EquipmentRepairNew() {
    FillCmbEquipmentRepairProvider();
    FillCmbEquipmentRepairResponsible();
    EquipmentRepairNewFormReset();
    var dialog = $("#dialogEquipmentRepairForm").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: "<h4 class='smaller'>" + Dictionary.Item_EquipmentRepair_PopupNew_Title + "</h4>",
        title_html: true,
        width: 500,
        buttons: [
            {
                id: 'BtnNewAddresSave',
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                click: function () {
                    EquipmentRepairSave();
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

function EquipmentRepairSave() {
    var ok = dialogMaintaimentRepairFormValidate();
    if (ok === false) {
        return false;
    }

    var cost = null;
    if ($('#TxtEquipmentRepairCost').val() !== '') {
        cost = ParseInputValueToNumber($('#TxtEquipmentRepairCost').val());
    }

    SelectedEquipmentRepair = {
        Id: SelectedEquipmentRepairId,
        CompanyId: Company.Id,
        EquipmentId: Equipment.Id,
        RepairType: document.getElementById('REquipmentRepairTypeInternal').checked ? 0 : 1,
        Date: GetDate($('#TxtEquipmentRepairDate').val(), '-'),
        Active: true,
        Description: $('#TxtEquipmentRepairDescription').val(),
        Tools: $('#TxtEquipmentRepairTools').val(),
        Observations: $('#TxtEquipmentRepairObservations').val(),
        Cost: cost,
        Provider: { Id: $('#CmbEquipmentRepairProvider').val() },
        Responsible: { Id: $('#CmbEquipmentRepairResponsible').val(), Value: $("#CmbEquipmentRepairResponsible option:selected").text() }
    };

    if (SelectedEquipmentRepairId == -1) {
        var webMethod = "/Async/EquipmentRepairActions.asmx/Insert";
        var data = { equipmentRepair: SelectedEquipmentRepair, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                SelectedEquipmentRepair.Id = msg.d.MessageError * 1;
                EquipmentRepairList.push(SelectedEquipmentRepair);
                EquipmentRepairRenderTable('TableEquipmentRepair');
                $('#dialogEquipmentRepairForm').dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var webMethod = "/Async/EquipmentRepairActions.asmx/Update";
        var data = { equipmentRepair: SelectedEquipmentRepair, userId: user.Id };
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                EquipmentRepairListUpdate(SelectedEquipmentRepair);
                EquipmentRepairRenderTable('TableEquipmentRepair');
                $('#dialogEquipmentRepairForm').dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function EquipmentRepairDelete(sender) {
    SelectedEquipmentRepairId = sender.parentNode.parentNode.id.substring(15) * 1;
    SelectedEquipmentRepair = EquipmentRepairgetById(SelectedEquipmentRepairId);
    if (SelectedEquipmentRepair == null) { return false; }
    document.getElementById('dialogDeleteEquipmentRepairName').innerHTML = SelectedEquipmentRepair.Description;
    var dialog = $("#dialogEquipmentRepairDelete").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: "<h4 class='smaller'>" + Dictionary.Item_EquipmentRepair_PopupDelete_Title + "</h4>",
        title_html: true,
        width: 500,
        buttons: [
        {
            id: 'BtnNewAddresSave',
            html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
            "class": "btn btn-danger btn-xs",
            click: function () {
                EquipmentRepairDeleteConfirmed();
            }
        },
        {
            html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
            "class": "btn btn-xs",
            click: function () {
                $(this).dialog("close");
            }
        }
        ]
    });
}

function EquipmentRepairDeleteConfirmed() {
    var webMethod = "/Async/EquipmentRepairActions.asmx/Delete";
    var data = { equipmentRepairId: SelectedEquipmentRepairId, companyId: Company.Id, userId: user.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            EquipmentRepairRemoveFromList(SelectedEquipmentRepairId);
            EquipmentRepairRenderTable('TableEquipmentRepair');
            $("#dialogEquipmentRepairDelete").dialog("close");

        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}