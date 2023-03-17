var SelectedEquipmentMaintenanceAct;
var SelectedEquipmentMaintenanceActId;
var SelectedEquipmentMaintenanceActAction;

function EquipmentMaintenanceActRenderTable(targetName) {
    VoidTable(targetName);
    var target = document.getElementById(targetName);
    var total = 0;

    EquipmentMaintenanceActList.sort(function (a, b) {
        return b.Date - a.Date;
    });

    for (var x = 0; x < EquipmentMaintenanceActList.length; x++) {
        if (EquipmentMaintenanceActList[x].Active === true) {
            total += EquipmentMaintenanceActRenderRow(EquipmentMaintenanceActList[x], targetName);
        }
    }

    $("#TableEquipmentMaintenanceActTotalLabel").html(Dictionary.Common_RegisterCount + ":&nbsp;<strong>" + EquipmentMaintenanceActList.length + "</strong><span style=\"float:right\">" + Dictionary.Common_Total + ":</span>");
    document.getElementById('TableEquipmentMaintenanceActTotal').innerHTML = ToMoneyFormat(total, 2);
    if ($("#EquipmentMaintenanceActTable #th0").attr('class') === "sort  ASC") {
        $("#EquipmentMaintenanceActTable #th0").click();
    }
    else {
        $("#EquipmentMaintenanceActTable #th0").click();
        $("#EquipmentMaintenanceActTable #th0").click();
    }
}

function EquipmentMaintenanceActRenderRow(equipmentMaintenanceAct, targetName) {
    var target = document.getElementById(targetName);
    var row = document.createElement("TR");
    var tdFecha = document.createElement("TD");
    var tdOperation = document.createElement("TD");
    var tdObservaciones = document.createElement("TD");
    var tdCost = document.createElement("TD");
    var tdResponsible = document.createElement("TD");
    var tdVto = document.createElement("TD");

    tdFecha.align = "center";
    tdVto.align = "center";
    tdCost.align = "right";

    var equipmentMaintenance = EquipmentMaintenanceDefinitiongetById(equipmentMaintenanceAct.EquipmentMaintenanceDefinitionId);

    tdOperation.appendChild(document.createTextNode(equipmentMaintenanceAct.Description));
    tdFecha.appendChild(document.createTextNode(FormatYYYYMMDD(equipmentMaintenanceAct.Date, "/")));
    tdVto.appendChild(document.createTextNode(FormatYYYYMMDD(equipmentMaintenanceAct.Expiration, "/")));
    tdResponsible.appendChild(document.createTextNode(equipmentMaintenanceAct.Responsible.Value));
    //tdObservaciones.appendChild(document.createTextNode(equipmentMaintenanceAct.Observations));

    if (typeof (equipmentMaintenanceAct.Observations) === "undefined") {
        tdObservaciones.appendChild(document.createTextNode(""));
    }
    else {
        tdObservaciones.appendChild(document.createTextNode(equipmentMaintenanceAct.Observations));
    }

    if (equipmentMaintenanceAct.Cost !== null) {
        tdCost.appendChild(document.createTextNode(ToMoneyFormat(equipmentMaintenanceAct.Cost * 1, 2)));
    }

    row.id = "EquipmentMaintenanceAct" + equipmentMaintenanceAct.Id;

    tdFecha.style.width = "90px";
    tdOperation.style.width = "400px";
    tdResponsible.style.width = "200px";
    tdCost.style.width = "70px";
    tdVto.style.width = "120px";

    row.appendChild(tdFecha);
    row.appendChild(tdOperation);
    row.appendChild(tdObservaciones);
    row.appendChild(tdResponsible);
    row.appendChild(tdCost);
    row.appendChild(tdVto);

    if (GrantToWrite) {
        var tdActions = document.createElement("TD");
        var iconEdit = document.createElement("SPAN");
        var iconDelete = document.createElement("SPAN");

        iconEdit.className = "btn btn-xs btn-info";
        iconEdit.title = Dictionary.Common_Edit;
        iconEdit.onclick = function (e) { EquipmentMaintenanceActEdit(this); };
        var innerEdit = document.createElement("I");
        if (Equipment.EndDate !== null) {
            innerEdit.className = "icon-eye-open bigger-120";
        }
        else {
            innerEdit.className = "icon-edit bigger-120";
        }
        iconEdit.appendChild(innerEdit);

        iconDelete.className = "btn btn-xs btn-danger";
        iconDelete.title = Dictionary.Common_Delete;
        iconDelete.onclick = function (e) { EquipmentMaintananceActDelete(this); };
        var innerDelete = document.createElement("I");
        innerDelete.className = "icon-trash bigger-120";
        iconDelete.appendChild(innerDelete);

        tdActions.appendChild(iconEdit);
        tdActions.appendChild(document.createTextNode(" "));
        tdActions.appendChild(iconDelete);
        tdActions.style.width = "90px";
        row.appendChild(tdActions);
    }

    target.appendChild(row);
    return equipmentMaintenanceAct.Cost * 1;
}

function EquipmentMaintenanceActgetById(id) {
    for (var x = 0; x < EquipmentMaintenanceActList.length; x++) {
        if (EquipmentMaintenanceActList[x].Id === id * 1) {
            return EquipmentMaintenanceActList[x];
        }
    }

    return null;
}

function EquipmentMaintenanceActRemoveFromList(id) {
    var temp = new Array();

    for (var x = 0; x < EquipmentMaintenanceActList.length; x++) {
        if (EquipmentMaintenanceActList[x].Id !== id * 1) {
            temp.push(EquipmentMaintenanceActList[x]);
        }
    }

    EquipmentMaintenanceActList = new Array();

    for (var x = 0; x < temp.length; x++) {
        EquipmentMaintenanceActList.push(temp[x]);
    }
}

function EquipmentMaintenanceActListUpdate(equipmentMaintenanceAct) {
    for (var x = 0; x < EquipmentMaintenanceActList.length; x++) {
        if (EquipmentMaintenanceActList[x].Id === equipmentMaintenanceAct.Id) {
            EquipmentMaintenanceActList[x] = equipmentMaintenanceAct;
        }
    }
}

function EquipmentMaintenanceActNewFormReset(EquipmentMaintenanceDefinition) {
    SelectedEquipmentActSelectedId = -1;
    ClearFieldTextMessages("TxtEquipmentMaintenanceActDate");
    ClearFieldTextMessages("TxtEquipmentMaintenanceActCost");
    ClearFieldTextMessages("CmbEquipmentMaintenanceActProvider");
    ClearFieldTextMessages("CmbEquipmentVerificationActResponsible");
    var external = false;
    if (EquipmentMaintenanceDefinition === null) {
        document.getElementById("TxtEquipmentMaintenanceActDate").disabled = true;
        document.getElementById("TxtEquipmentMaintenanceActCost").disabled = true;
        document.getElementById("TxtEquipmentMaintenanceActObservations").disabled = true;
        document.getElementById("CmbEquipmentMaintenanceActProvider").disabled = true;
        document.getElementById("CmbEquipmentVerificationActResponsible").disabled = true;
        document.getElementById("CmbEquipmentMaintenanceActProviderRow").style.display = "none";
        $("#CmbEquipmentVerificationActResponsible").val(ApplicationUser.Employee.Id);
        $("#TxtEquipmentMaintenanceActCost").val("");
    }
    else {
        document.getElementById("TxtEquipmentMaintenanceActDate").disabled = false;
        document.getElementById("TxtEquipmentMaintenanceActCost").disabled = false;
        document.getElementById("TxtEquipmentMaintenanceActObservations").disabled = false;
        document.getElementById("CmbEquipmentMaintenanceActProvider").disabled = false;
        document.getElementById("CmbEquipmentVerificationActResponsible").disabled = false;
        external = EquipmentMaintenanceDefinition.MaintenanceType === 1;
        if (external === true) {
            $("#CmbEquipmentMaintenanceActProvider").val(EquipmentMaintenanceDefinition.Provider.Id);
        }

        $("#CmbEquipmentVerificationActResponsible").val(EquipmentMaintenanceDefinition.Responsible.Id);
        if (EquipmentMaintenanceDefinition.Cost !== null) {
            $("#TxtEquipmentMaintenanceActCost").val(ToMoneyFormat(EquipmentMaintenanceDefinition.Cost, 2));
        }
        else {
            $("#TxtEquipmentMaintenanceActCost").val("");
        }
    }

    $("#TxtEquipmentMaintenanceActObservations").val("");
    if (external === true) {
        document.getElementById("CmbEquipmentMaintenanceActProviderRow").style.display = "";
    }
    else {
        document.getElementById("CmbEquipmentMaintenanceActProviderRow").style.display = "none";
    }

    $("#TxtEquipmentMaintenanceActDate").val(FormatDate(new Date(), "/"));
}

function EquipmentMaintenanceActEditFormFill(EquipmentMaintenanceAct, external) {
    SelectedEquipmentActSelectedId = EquipmentMaintenanceAct.Id;
    ClearFieldTextMessages("TxtEquipmentMaintenanceActDate");
    ClearFieldTextMessages("TxtEquipmentMaintenanceActCost");
    ClearFieldTextMessages("CmbEquipmentMaintenanceActProvider");
    ClearFieldTextMessages("CmbEquipmentVerificationActResponsible");
    $("#TxtEquipmentMaintenanceActDate").val(FormatYYYYMMDD(EquipmentMaintenanceAct.Date, "/"));
    $("#TxtEquipmentMaintenanceActObservations").val(EquipmentMaintenanceAct.Observations);

    if (EquipmentMaintenanceAct.Cost !== null) {
        $("#TxtEquipmentMaintenanceActCost").val(ToMoneyFormat(EquipmentMaintenanceAct.Cost, 2));
    }
    else {
        $("#TxtEquipmentMaintenanceActCost").val();
    }

    if (external === 1) {
        document.getElementById("CmbEquipmentMaintenanceActProviderRow").style.display = "";
        $("#CmbEquipmentMaintenanceActProvider").val(EquipmentMaintenanceAct.Provider.Id);
    }
    else {
        document.getElementById("CmbEquipmentMaintenanceActProviderRow").style.display = "none";
    }

    $("#CmbEquipmentMaintenanceActResponsible").val(EquipmentMaintenanceAct.Responsible.Id);
    if ($("#CmbEquipmentMaintenanceActResponsible").val() === null) {
        $("#CmbEquipmentMaintenanceActResponsible").val(ApplicationUser.Employee.Id);
    }
}

function EquipmentMaintenanceActValidateForm() {
    var ok = true;
    if (!RequiredFieldText("TxtEquipmentMaintenanceActDate")) {
        ok = false;
    }
    else {
        if (validateDate($("#TxtEquipmentMaintenanceActDate").val()) === false) {
            $("#TxtEquipmentMaintenanceActDateLabel").css("color", Color.Error);
            $("#TxtEquipmentMaintenanceActDateMalformed").show();
            ok = false;
        }
    }
    /*
    if (!RequiredFieldText('TxtEquipmentMaintenanceActCost')) {
        ok = false;
    }
    */
    if (!RequiredFieldCombo('CmbEquipmentMaintenanceActProvider') && document.getElementById('RMaintainmentTypeExternal').checked) {
        ok = false;
    }
    if (!RequiredFieldCombo('CmbEquipmentVerificationActResponsible')) {
        ok = false;
    }

    var expiration = GetDate($("#TxtEquipmentMaintenanceActDate").val(), "-");

    // comprobar linea del tiempo
    if (SelectedEquipmentActSelectedId < 1) {
        for (var x = 0; x < EquipmentMaintenanceActList.length; x++) {
            if (EquipmentMaintenanceActList[x].EquipmentMaintenanceDefinitionId === SelectedEquipmentDefinitionSelectedId * 1) {
                if (GetDateYYYYMMDD(EquipmentMaintenanceActList[x].Date) > expiration) {
                    ok = false;
                    $("#TxtEquipmentMaintenanceActDateLabel").css("color", Color.Error);
                    $("#TxtEquipmentMaintenanceActDateOverTime").show();
                }
            }
        }
    }

    return ok;
}

function FillCmbEquipmentMaintainmentType() {
    VoidTable("CmbEquipmentMaintenanceType");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    document.getElementById("CmbEquipmentMaintenanceType").appendChild(optionDefault);
    for (var x = 0; x < EquipmentMaintenanceDefinitionList.length; x++) {
        var option = document.createElement("option");
        option.value = EquipmentMaintenanceDefinitionList[x].Id;
        option.appendChild(document.createTextNode(EquipmentMaintenanceDefinitionList[x].Description));
        document.getElementById("CmbEquipmentMaintenanceType").appendChild(option);
    }
}

function FillCmbEquipmentMaintainmentActProvider() {
    VoidTable("CmbEquipmentMaintenanceActProvider");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbEquipmentMaintenanceActProvider").appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement("option");
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        document.getElementById("CmbEquipmentMaintenanceActProvider").appendChild(option);
    }
}

function FillCmbEquipmentMaintainmentActResponsible() {
    VoidTable("CmbEquipmentMaintenanceActResponsible");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbEquipmentMaintenanceActResponsible").appendChild(optionDefault);

    for (var x = 0; x < Employees.length; x++) {
        if (Employees[x].Active === true && Employees[x].DisabledDate === null) {
            var option = document.createElement("option");
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            document.getElementById("CmbEquipmentMaintenanceActResponsible").appendChild(option);
        }
    }

    $("#CmbEquipmentMaintenanceActResponsible").val(ApplicationUser.Employee.Id);
}

function EquipmentMaintananceActDelete(sender) {
    console.log(sender);
    SelectedEquipmentMaintenanceActId = sender.parentNode.parentNode.id.substring(23);
    var EquipmentMaintenanceAct = EquipmentMaintenanceActgetById(SelectedEquipmentMaintenanceActId);
    if (EquipmentMaintenanceAct === null) { return false; }
    $("#dialogDeleteEquipmentMaintenanceActName").html(EquipmentMaintenanceAct.Description);
    var dialog = $("#dialogEquipmentMaintananceActDelete").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_EquipmentMaintenance_Popup_DeleteAct_Title,
        "title_html": true,
        "width": 500,
        "buttons": [
            {
                "id": "BtnEquipmentMaintananceActDelete",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    dialogEquipmentMaintananceActDeleteConfirmed();
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

function dialogEquipmentMaintananceActDeleteConfirmed() {
    var webMethod = "/Async/EquipmentMaintenanceActActions.asmx/Delete";
    var data = { equipmentMaintenanceActId: SelectedEquipmentMaintenanceActId, companyId: Company.Id, userId: user.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            EquipmentMaintenanceActRemoveFromList(SelectedEquipmentMaintenanceActId);
            EquipmentMaintenanceActRenderTable("TableEquipmentMaintenanceAct");
            $("#dialogEquipmentMaintananceActDelete").dialog("close");

        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function EquipmentMaintenanceActEdit(sender) {
    SelectedEquipmentMaintenanceActAction = "Update";
    SelectedEquipmentMaintenanceActId = sender.parentNode.parentNode.id.substring(23) * 1;
    SelectedEquipmentMaintenanceAct = EquipmentMaintenanceActgetById(SelectedEquipmentMaintenanceActId);
    if (SelectedEquipmentMaintenanceAct === null) { return false; }

    SelectedEquipmentDefinitionSelectedId = SelectedEquipmentMaintenanceAct.EquipmentMaintenanceDefinitionId;
    SelectedEquipmentMaintenanceDefinition = EquipmentMaintenanceDefinitiongetById(SelectedEquipmentDefinitionSelectedId);
    if (SelectedEquipmentMaintenanceDefinition === null) { return false; }

    FillCmbEquipmentMaintainmentType();
    FillCmbEquipmentMaintainmentActProvider();
    FillCmbEquipmentMaintainmentActResponsible();
    
    $("#CmbEquipmentMaintenanceType").val(SelectedEquipmentMaintenanceDefinition.Id);
    EquipmentMaintenanceActEditFormFill(SelectedEquipmentMaintenanceAct, SelectedEquipmentMaintenanceDefinition.MaintenanceType);
    $("#dialogNewEquipmentMaintenanceActOperation").html(SelectedEquipmentMaintenanceDefinition.Description + "&nbsp;<i>(" + (SelectedEquipmentMaintenanceDefinition.MaintenanceType === 0 ? Dictionary.Common_Internal : Dictionary.Common_External) + ")</i>");
    var dialog = $("#dialogNewEquipmentMaintenanceAct").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_EquipmentMaintenance_PopupInterventionUpdate,
        "title_html": true,
        "width": 500,
        "buttons": [
            {
                "id": "BtnEquipmentMaintenanceActUpdate",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    EquipmentMaintainmentActSave();
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
        $("#BtnEquipmentMaintenanceActUpdate").hide();
    }
}

function EquipmentMaintenanceDefinitionRegister(sender) {
    console.log("EquipmentMaintenanceDefinitionRegister", sender);
    SelectedEquipmentMaintenanceActAction = "New";
    $("#CmbEquipmentMaintenanceTypeErrorRequired").hide();
    if (EquipmentMaintenanceDefinitionList.length === 0) {
        warningInfoUI(Dictionary.Item_EquipmentMaintenance_Popup_Register_Error_NoDefinitions, null);
        return false;
    }

    FillCmbEquipmentMaintainmentType();
    FillCmbEquipmentMaintainmentActProvider();
    FillCmbEquipmentMaintainmentActResponsible();

    if (sender === null) {
        SelectedEquipmentDefinitionSelectedId = mantenimientoLaunchId;
        if (SelectedEquipmentDefinitionSelectedId === 0) {
            SelectedEquipmentMaintenanceDefinition = null;
            EquipmentMaintenanceActNewFormReset(SelectedEquipmentMaintenanceDefinition);
        }
        else {
            SelectedEquipmentMaintenanceDefinition = EquipmentMaintenanceDefinitiongetById(SelectedEquipmentDefinitionSelectedId);
            if (SelectedEquipmentMaintenanceDefinition === null) { return false; }
            EquipmentMaintenanceActNewFormReset(SelectedEquipmentMaintenanceDefinition);
            $("#CmbEquipmentMaintenanceType").val(mantenimientoLaunchId);
            $("#dialogNewEquipmentMaintenanceActOperation").html(SelectedEquipmentMaintenanceDefinition.Description + "&nbsp;<i>(" + (SelectedEquipmentMaintenanceDefinition.MaintenanceType === 0 ? Dictionary.Common_Internal : Dictionary.Common_External) + ")</i>");
        }
    }
    else {
        SelectedEquipmentDefinitionSelectedId = sender.parentNode.parentNode.id.substring(30) * 1;
        SelectedEquipmentMaintenanceDefinition = EquipmentMaintenanceDefinitiongetById(SelectedEquipmentDefinitionSelectedId);
        if (SelectedEquipmentMaintenanceDefinition === null) { return false; }
        EquipmentMaintenanceActNewFormReset(SelectedEquipmentMaintenanceDefinition);
        $("#dialogNewEquipmentMaintenanceActOperation").html(SelectedEquipmentMaintenanceDefinition.Description + "&nbsp;<i>(" + (SelectedEquipmentMaintenanceDefinition.MaintenanceType === 0 ? Dictionary.Common_Internal : Dictionary.Common_External) + ")</i>");
    }

    $("#dialogNewEquipmentMaintenanceAct").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_EquipmentMaintenance_PopupIntervention,
        "title_html": true,
        "width": 500,
        "buttons":
        [
            {
                "id": "BtnNewAddresSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    EquipmentMaintainmentActSave();
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

function EquipmentMaintainmentActSave() {
    document.getElementById('CmbEquipmentMaintenanceTypeErrorRequired').style.display = 'none';
    if (document.getElementById('CmbEquipmentMaintenanceType').value * 1 === 0)
    {
        document.getElementById('CmbEquipmentMaintenanceTypeErrorRequired').style.display = '';
        return false;
    }

    var ok = EquipmentMaintenanceActValidateForm();
    if (ok === false) {
        return false;
    }

    var expiration = GetDate($('#TxtEquipmentMaintenanceActDate').val(), '-');
    expiration.setDate(expiration.getDate() + SelectedEquipmentMaintenanceDefinition.Periodicity * 1);

    var cost = null;
    if ($('#TxtEquipmentMaintenanceActCost').val() !== '')
    {
        cost = ParseInputValueToNumber($('#TxtEquipmentMaintenanceActCost').val());
    }

    SelectedEquipmentMaintenanceAct = {
        Id: SelectedEquipmentActSelectedId,
        CompanyId: Company.Id,
        EquipmentId: Equipment.Id,
        EquipmentMaintenanceDefinitionId: SelectedEquipmentDefinitionSelectedId * 1,
        MaintenanceType: document.getElementById('RMaintainmentTypeInternal').checked ? 0 : 1,
        Description: SelectedEquipmentMaintenanceDefinition.Description,
        Date: GetDate($('#TxtEquipmentMaintenanceActDate').val(), '-'),
        Expiration: expiration,
        Active: true,
        Observations: $('#TxtEquipmentMaintenanceActObservations').val(),
        Cost: cost,
        Provider: { Id: $('#CmbEquipmentMaintenanceActProvider').val() * 1 },
        Responsible: { Id: $('#CmbEquipmentMaintenanceActResponsible').val() * 1, Value: $("#CmbEquipmentMaintenanceActResponsible option:selected").text() }
    };

    var data = {};
    if (SelectedEquipmentMaintenanceActAction === "New") {
        data = { "equipmentMaintenanceAct": SelectedEquipmentMaintenanceAct, "userId": user.Id };
        $.ajax({
            "type": "POST",
            "url": "/Async/EquipmentMaintenanceActActions.asmx/Insert",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                SelectedEquipmentMaintenanceAct.Id = msg.d.MessageError * 1;
                EquipmentMaintenanceActList.push(SelectedEquipmentMaintenanceAct);
                EquipmentMaintenanceActRenderTable("TableEquipmentMaintenanceAct");
                $("#dialogNewEquipmentMaintenanceAct").dialog("close");
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        data = { "equipmentMaintenanceAct": SelectedEquipmentMaintenanceAct, "userId": user.Id };
        $.ajax({
            "type": "POST",
            "url": "/Async/EquipmentMaintenanceActActions.asmx/Update",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                EquipmentMaintenanceActListUpdate(SelectedEquipmentMaintenanceAct);
                EquipmentMaintenanceActRenderTable("TableEquipmentMaintenanceAct");
                $("#dialogNewEquipmentMaintenanceAct").dialog("close");
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function MaintainmentNewSave() {
    var ok = EquipmentMaintenanceDefinitionValidateForm();
    if (ok === false) {
        return false;
    }

    var periodicity = ParseInputValueToNumber($("#TxtNewMaintainmentPeriodicity").val());
    var cost = ParseInputValueToNumber($("#TxtNewMaintainmentCost").val());

    var webMethod = "/Async/EquipmentMaintenanceActions.asmx/Insert";
    SelectedEquipmentMaintenanceDefinition = {
        "Id": -1,
        "CompanyId": Company.Id,
        "EquipmentId": Equipment.Id,
        "MaintenanceType": document.getElementById("RMaintainmentTypeInternal").checked ? 0 : 1,
        "Description": $("#TxtNewMaintainmentOperation").val(),
        "Periodicity": periodicity,
        "Accessories": $("#TxtNewMaintainmentAccessories").val(),
        "Cost": cost,
        "FirstDate": GetDate($("#NewMaintainmentFirstDate").val(), "/", true),
        "Provider": { Id: $("#CmbNewMaintainmentProvider").val() },
        "Responsible": { Id: $("#CmbNewMaintainmentResponsible").val() }
    };

    var data = {
        "equipmentMaintenance": SelectedEquipmentMaintenanceDefinition,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            SelectedEquipmentMaintenanceDefinition.Id = msg.d.MessageError * 1;
            EquipmentMaintenanceDefinitionList.push(SelectedEquipmentMaintenanceDefinition);
            EquipmentMaintenanceDefinitionRenderTable("TableEquipmentMaintenanceDefinition");
            $("#dialogNewMaintaiment").dialog("close");

        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ShowMaintaimentRecordPopup(actionSelected) {
    console.log("ShowMaintaimentRecordPopup", actionSelected);
    var dialog = $("#dialogNewMaintaimentRecord").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_EquipmentMaintenance_Popup_NewAct_Title,
        "title_html": true,
        "width": 500,
        "buttons": [
            {
                "id": "BtnNewAddresSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    ShowAddAddressPopup(1);
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

function CmbEquipmentMaintenanceTypeChanged(sender)
{
    SelectedEquipmentDefinitionSelectedId = sender.value * 1;
    SelectedEquipmentMaintenanceDefinition = EquipmentMaintenanceDefinitiongetById(SelectedEquipmentDefinitionSelectedId);
    EquipmentMaintenanceActNewFormReset(SelectedEquipmentMaintenanceDefinition);
    // $('#dialogNewEquipmentMaintenanceActOperation').html(SelectedEqumentMaintenanceDefinition.Description + '&nbsp;<i>(' + (SelectedEquipmentMaintenanceDefinition.MaintenanceType == 0 ? Dictionary.Common_Internal : Dictionary.Common_External) + ')</i>');
}