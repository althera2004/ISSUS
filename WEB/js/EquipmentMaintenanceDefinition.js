var SelectedEquipmentMaintenanceDefinition;
var SelectedEquipmentDefinitionSelectedId;

function EquipmentMaintenanceDefinitionRenderTable(targetName) {
    VoidTable(targetName);
    var target = document.getElementById(targetName);
    var total = 0;
    for (var x = 0; x < EquipmentMaintenanceDefinitionList.length; x++) {
        total += EquipmentMaintenanceDefinitionRenderRow(EquipmentMaintenanceDefinitionList[x], targetName);
    }
}

function EquipmentMaintenanceDefinitionRenderRow(equipmentMaintenance, targetName) {
    var target = document.getElementById(targetName);
    var row = document.createElement("TR");
    var tdOperation = document.createElement("TD");
    var tdType = document.createElement("TD");
    var tdPeriodicity = document.createElement("TD");
    var tdAccessories = document.createElement("TD");
    var tdCost = document.createElement("TD");

    tdPeriodicity.align = "right";
    tdCost.align = "right";

    var MaintenanceType = equipmentMaintenance.MaintenanceType === 0 ? Dictionary.Common_Internal : Dictionary.Common_External;

    tdOperation.appendChild(document.createTextNode(equipmentMaintenance.Description));
    tdType.appendChild(document.createTextNode(MaintenanceType));
    tdPeriodicity.appendChild(document.createTextNode(ToMoneyFormat(equipmentMaintenance.Periodicity, 0))); // ISSUS-129 + " " + Dictionary.Common_Days.toLowerCase()));

    if (typeof (equipmentMaintenance.Accessories) === "undefined") {
        tdAccessories.appendChild(document.createTextNode(""));
    }
    else {
        tdAccessories.appendChild(document.createTextNode(equipmentMaintenance.Accessories));
    }

    if (equipmentMaintenance.Cost !== null) {
        tdCost.appendChild(document.createTextNode(ToMoneyFormat(equipmentMaintenance.Cost * 1, 2)));
    }

    row.id = "EquipmentMaintenanceDefinition" + equipmentMaintenance.Id;

    tdOperation.style.width = "400px";
    tdType.style.width = "90px";
    tdPeriodicity.style.width = "100px";
    tdCost.style.width = "70px";

    row.appendChild(tdOperation);
    row.appendChild(tdType);
    row.appendChild(tdPeriodicity);
    row.appendChild(tdAccessories);
    row.appendChild(tdCost);

    if (GrantToWrite) {
        var tdActions = document.createElement("TD");
        var iconEdit = document.createElement("SPAN");
        var iconDelete = document.createElement("SPAN");
        var iconRegister = document.createElement("SPAN");

        iconEdit.className = "btn btn-xs btn-info";
        iconEdit.title = Dictionary.Common_Edit;
        iconEdit.onclick = function (e) { EquipmentMaintenanceDefinitionEdit(this); };
        var innerEdit = document.createElement("I");
        innerEdit.className = "icon-edit bigger-120";
        iconEdit.appendChild(innerEdit);

        iconDelete.className = "btn btn-xs btn-danger";
        iconDelete.title = Dictionary.Common_Delete;
        iconDelete.onclick = function (e) { EquipmentMaintananceDefinitionDelete(this); };
        var innerDelete = document.createElement("I");
        innerDelete.className = "icon-trash bigger-120";
        iconDelete.appendChild(innerDelete);

        iconRegister.className = "btn btn-xs btn-success";
        iconRegister.title = Dictionary.Item_EquipmentMaintenance_Button_Register;
        iconRegister.onclick = function (e) { EquipmentMaintenanceDefinitionRegister(this); };
        var innerRegister = document.createElement("I");
        innerRegister.className = "icon-star bigger-120";
        iconRegister.appendChild(innerRegister);

        tdActions.appendChild(iconEdit);
        tdActions.appendChild(document.createTextNode(" "));
        tdActions.appendChild(iconDelete);
        tdActions.appendChild(document.createTextNode(" "));
        // tdActions.appendChild(iconRegister);
        tdActions.style.width = "90px";
        row.appendChild(tdActions);
    }

    target.appendChild(row);
    if (equipmentMaintenance.Cost === null) {
        return 0;
    }

    return equipmentMaintenance.Cost * 1;
}

function EquipmentMaintenanceDefinitiongetById(id) {
    for (var x = 0; x < EquipmentMaintenanceDefinitionList.length; x++) {
        if (EquipmentMaintenanceDefinitionList[x].Id == id) {
            return EquipmentMaintenanceDefinitionList[x];
        }
    }

    return null;
}

function EquipmentMaintenanceDefinitionRemoveFromList(id) {
    var temp = new Array();

    for (var x = 0; x < EquipmentMaintenanceDefinitionList.length; x++) {
        if (EquipmentMaintenanceDefinitionList[x].Id != id) {
            temp.push(EquipmentMaintenanceDefinitionList[x]);
        }
    }

    EquipmentMaintenanceDefinitionList = new Array();

    for (var y = 0; y < temp.length; y++) {
        EquipmentMaintenanceDefinitionList.push(temp[y]);
    }
}

function EquipmentMaintenanceDefinitionListUpdate(equipmentMaintenance) {
    for (var x = 0; x < EquipmentMaintenanceDefinitionList.length; x++) {
        if (EquipmentMaintenanceDefinitionList[x].Id == equipmentMaintenance.Id) {
            EquipmentMaintenanceDefinitionList[x] = equipmentMaintenance;
        }
    }
}

function EquipmentMaintenanceNewFormReset() {
    ClearFieldTextMessages("TxtNewMaintainmentOperation");
    ClearFieldTextMessages("TxtNewMaintainmentPeriodicity");
    ClearFieldTextMessages("TxtNewMaintainmentCost");
    ClearFieldTextMessages("CmbNewMaintainmentProvider");
    ClearFieldTextMessages("CmbNewMaintainmentResponsible");
    $("#RMaintainmentTypeInternal").prop("checked", false);
    $("#RMaintainmentTypeExternal").prop("checked", false);
    document.getElementById("RMaintainmentTypeErrorRequired").style.display = "none";
    $("#TxtNewMaintainmentOperation").val("");
    $("#TxtNewMaintainmentPeriodicity").val("");
    $("#TxtNewMaintainmentAccessories").val("");
    $("#TxtNewMaintainmentCost").val("");
    $("#CmbNewMaintainmentProvider").val(0);
    $("#CmbNewMaintainmentResponsible").val(ApplicationUser.Employee.Id);
}

function EquipmentMaintenanceEditFormFill(equipmentMaintenanceDefinition) {
    document.getElementById("RMaintainmentTypeErrorRequired").style.display = "none";
    ClearFieldTextMessages("TxtNewMaintainmentOperation");
    ClearFieldTextMessages("TxtNewMaintainmentPeriodicity");
    ClearFieldTextMessages("TxtNewMaintainmentCost");
    ClearFieldTextMessages("CmbNewMaintainmentProvider");
    ClearFieldTextMessages("CmbNewMaintainmentResponsible");
    $("#RMaintainmentTypeInternal").prop("checked", equipmentMaintenanceDefinition.MaintenanceType === 0);
    $("#RMaintainmentTypeExternal").prop("checked", equipmentMaintenanceDefinition.MaintenanceType === 1);
    document.getElementById("dialogNewMaintaimentProviderRow").style.display = equipmentMaintenanceDefinition.MaintenanceType === 0 ? "none" : "";
    $("#TxtNewMaintainmentOperation").val(equipmentMaintenanceDefinition.Description);
    $("#TxtNewMaintainmentPeriodicity").val(ToMoneyFormat(equipmentMaintenanceDefinition.Periodicity,0));
    $("#TxtNewMaintainmentAccessories").val(equipmentMaintenanceDefinition.Accessories);
    if (equipmentMaintenanceDefinition.Cost !== null) {
        $("#TxtNewMaintainmentCost").val(ToMoneyFormat(equipmentMaintenanceDefinition.Cost, 2));
    }
    else {
        $("#TxtNewMaintainmentCost").val("");
    }
    $("#CmbNewMaintainmentProvider").val(equipmentMaintenanceDefinition.Provider.Id);
    $("#CmbNewMaintainmentResponsible").val(equipmentMaintenanceDefinition.Responsible.Id);
    if ($("#CmbNewMaintainmentResponsible").val() === null) {
        $("#CmbNewMaintainmentResponsible").val(ApplicationUser.Employee.Id);
    }
}

function EquipmentMaintenanceDefinitionValidateForm() {
    var ok = true;
    if (!document.getElementById('RMaintainmentTypeInternal').checked && !document.getElementById('RMaintainmentTypeExternal').checked) {
        ok = false;
        document.getElementById('RMaintainmentTypeErrorRequired').style.display = '';
    }
    else {
        document.getElementById('RMaintainmentTypeErrorRequired').style.display = 'none';
    }

    if (!RequiredFieldText('TxtNewMaintainmentOperation')) {
        ok = false;
    }
    if (!RequiredFieldText('TxtNewMaintainmentPeriodicity')) {
        ok = false;
    }
    /* ISSUS-18
    if (!RequiredFieldText('TxtNewMaintainmentCost')) {
        ok = false;
    }
    */
    if (!RequiredFieldCombo('CmbNewMaintainmentProvider') && document.getElementById('RMaintainmentTypeExternal').checked) {
        ok = false;
    }
    if (!RequiredFieldCombo('CmbNewMaintainmentResponsible')) {
        ok = false;
    }

    return ok;
}

function FillCmbNewMaintainmentResponsible() {
    VoidTable("CmbNewMaintainmentResponsible");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbNewMaintainmentResponsible").appendChild(optionDefault);

    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true || MaintainmentNewResponsibleSelected == Employees[x].Id) {
            var option = document.createElement("option");
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            if (MaintainmentNewResponsibleSelected == Employees[x].Id) {
                option.selected = true;
            }

            document.getElementById("CmbNewMaintainmentResponsible").appendChild(option);
        }
    }
}

function EquipmentMaintenanceDefinitionEdit(sender) {
    FillCmbProviders();
    FillCmbNewMaintainmentResponsible();
    SelectedEquipmentDefinitionSelectedId = sender.parentNode.parentNode.id.substring(30) * 1;
    SelectedEquipmentMaintenanceDefinition = EquipmentMaintenanceDefinitiongetById(SelectedEquipmentDefinitionSelectedId);
    if (SelectedEquipmentMaintenanceDefinition === null) { return false; }
    EquipmentMaintenanceEditFormFill(SelectedEquipmentMaintenanceDefinition);
    var dialog = $("#dialogNewMaintaiment").removeClass('hide').dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_EquipmentMaintenance_PopupConfigurationUpdate_Title + "</h4></div>",
        "title_html": true,
        "width": 500,
        buttons: [
            {
                "id": "BtnNewMaintenanceDefinitionSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                "class": "btn btn-success btn-xs",
                click: function () {
                    EquipmentMaintenanceDefinitionEditConfirmed();
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

    if (Equipment.EndDate !== null) {
        $("#BtnNewAddresSave").hide();
    }
}

function EquipmentMaintenanceDefinitionEditConfirmed() {
    var ok = EquipmentMaintenanceDefinitionValidateForm();
    if (ok === false) {
        return false;
    }

    var cost = null;
    if ($("#TxtNewMaintainmentCost").val() !== "") {
        cost = ParseInputValueToNumber($("#TxtNewMaintainmentCost").val());
    }

    var providerId = $("#CmbNewMaintainmentProvider").val();
    if (providerId == null) { providerId = -1;}

    var webMethod = "/Async/EquipmentMaintenanceActions.asmx/Update";
    SelectedEquipmentMaintenanceDefinition = {
        Id: SelectedEquipmentDefinitionSelectedId,
        CompanyId: Company.Id,
        EquipmentId: Equipment.Id,
        MaintenanceType: document.getElementById("RMaintainmentTypeInternal").checked ? 0 : 1,
        Description: $("#TxtNewMaintainmentOperation").val(),
        Periodicity: ParseInputValueToNumber($("#TxtNewMaintainmentPeriodicity").val()),
        Accessories: $("#TxtNewMaintainmentAccessories").val(),
        Cost: cost,
        Provider: { Id: providerId },
        Responsible: { Id: $("#CmbNewMaintainmentResponsible").val() }
    };

    var oldEquipmentMaintenanceDefinition = EquipmentMaintenanceDefinitiongetById(SelectedEquipmentDefinitionSelectedId);
    var data = { newEquipmentMaintenanceDefinition: SelectedEquipmentMaintenanceDefinition, oldEquipmentMaintenanceDefinition: oldEquipmentMaintenanceDefinition, userId: user.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            EquipmentMaintenanceDefinitionListUpdate(SelectedEquipmentMaintenanceDefinition);
            EquipmentMaintenanceDefinitionRenderTable("TableEquipmentMaintenanceDefinition");
            $("#dialogNewMaintaiment").dialog("close");

        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function EquipmentMaintananceDefinitionDelete(sender) {
    SelectedEquipmentDefinitionSelectedId = sender.parentNode.parentNode.id.substring(30);
    var EquipmentMaintenanceDefinition = EquipmentMaintenanceDefinitiongetById(SelectedEquipmentDefinitionSelectedId);
    if (EquipmentMaintenanceDefinition === null) { return false; }

    for (var x = 0; x < EquipmentMaintenanceActList.length; x++) {
        if (EquipmentMaintenanceActList[x].EquipmentMaintenanceDefinitionId == EquipmentMaintenanceDefinition.Id) {
            warningInfoUI(Dictionary.Item_EquipmentMaintenance_ErrorMessage_AsociateRecords, null, 300);
            return false;
        }
    }

    $("#dialogEquipmentMaintananceDefinitionDeleteName").html(EquipmentMaintenanceDefinition.Description);
    var dialog = $("#dialogEquipmentMaintananceDefinitionDelete").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_EquipmentMaintenance_PopupConfigurationDelete_Title + "</h4>",
        "title_html": true,
        "width": 500,
        "buttons": [
            {
                "id": "BtnMaintenanceDefinitionDelete",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    dialogEquipmentMaintananceDefinitionDeleteConfirmed();
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function dialogEquipmentMaintananceDefinitionDeleteConfirmed() {
    var data = {
        "equipmentMaintenanceDefinitionId": SelectedEquipmentDefinitionSelectedId,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/EquipmentMaintenanceActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        success: function (msg) {
            EquipmentMaintenanceDefinitionRemoveFromList(SelectedEquipmentDefinitionSelectedId);
            EquipmentMaintenanceDefinitionRenderTable('TableEquipmentMaintenanceDefinition');
            $("#dialogEquipmentMaintananceDefinitionDelete").dialog("close");

        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ShowMaintaimentPopup(actionSelected) {
    FillCmbProviders();
    FillCmbNewMaintainmentResponsible();
    EquipmentMaintenanceNewFormReset();
    var dialog = $("#dialogNewMaintaiment").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_EquipmentMaintenance_PopupConfiguration_Title + "</h4>",
        "title_html": true,
        "width": 450,
        "buttons": [
            {
                "id": "BtnNewMaintenance",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    MaintainmentNewSave();
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
}