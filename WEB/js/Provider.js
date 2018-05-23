var ProviderSelected = 0;
var MaintainmentNewResponsibleSelected = 0;
var ItemAffected = "";

// Bar popup for bar item Provider
function ShowProviderBarPopup(itemSender) {
    ItemAffected = itemSender;
    console.log("ShowProviderBarPopup:" + itemSender);

    if (itemSender === "EquipmentCalibrationDefinition") {
        ProviderSelected = $("#CmbCalibrationExternalProvider").val() * 1;
        console.log("EquipmentCalibrationDefinition:" + ProviderSelected);
    }

    if (itemSender === "EquipmentVerificationDefinition") {
        ProviderSelected = $("#CmbVerificationExternalProvider").val() * 1;
        console.log("EquipmentVerificationDefinition:" + ProviderSelected);
    }

    if (itemSender === "EquipmentCalibrationAct") {
        ProviderSelected = $("#CmbEquipmentCalibrationActProvider").val() * 1;
        console.log("EquipmentCalibrationAct:" + ProviderSelected);
    }

    if (itemSender === "EquipmentVerificationAct") {
        ProviderSelected = $("#CmbEquipmentVerificationActProvider").val() * 1;
        console.log("EquipmentVerificationAct:" + ProviderSelected);
    }

    if (itemSender === "EquipmentMaintenanceDefinition") {
        ProviderSelected = $("#CmbNewMaintainmentProvider").val() * 1;
        console.log("EquipmentMaintenanceDefinition:" + ProviderSelected);
    }

    if (itemSender === "EquipmentMaintenanceAct") {
        ProviderSelected = $("#CmbEquipmentMaintenanceActProvider").val() * 1;
        console.log("EquipmentMaintenanceAct:" + ProviderSelected);
    }

    if (itemSender === "EquipmentEquipmentRepair") {
        ProviderSelected = $("#CmbEquipmentRepairProvider").val() * 1;
        console.log("EquipmentEquipmentRepair:" + ProviderSelected);
    }

    ProviderRenderPopup();
    var dialog = $("#dialogProvider").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Providers,
        "title_html": true,
        "width": 600,
        "buttons": [
            {
                "id": "BtnProviderSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () { ProviderInsert(); }
            },
            {
                "id": "BtnProviderCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

for (var x = 0; x < Providers.length; x++) {
    if (Providers[x].Id === ProviderSelected) {
        document.getElementById("TxtProvider").value = Provider[x].Description;
        break;
    }

    Providers.sort(CompareProviders);
}

FillCmbProviders();

// Selection functions for bar item Provider
function ProviderChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    console.log("Change:" + ItemAffected + " - " + id);
    $("#dialogProvider").dialog("close");
    FillCmbProviders();
    for (var x = 0; x < Providers.length; x++) {
        if (Providers[x].Id === id) {
            ProviderSelected = id;

            if (ItemAffected === "EquipmentCalibrationDefinition")
            {
                ProviderCalibrationDefinition = id;
            }

            if (ItemAffected === "EquipmentVerificationDefinition")
            {
                ProviderVerificationDefinition = id;
            }

            document.getElementById("CmbEquipmentCalibrationActProvider").value = ProviderSelected;
            document.getElementById("CmbEquipmentVerificationActProvider").value = ProviderSelected;
            document.getElementById("CmbNewMaintainmentProviderValue").value = Providers[x].Description;
            document.getElementById("CmbEquipmentMaintenanceActProvider").value = ProviderSelected;
            document.getElementById("CmbEquipmentRepairProvider").value = ProviderSelected;
            break;
        }
    }
}

function FillCmbProviders() {
    FillcmbProviderMaintenanceDefinition();
    FillcmbProviderCalibrationDefinition();
    FillcmbProviderVerificationDefinition();
    FillCmbEquipmentRepairProvider();
    FillCmbProviderCalibrationAct();
    FillCmbProviderVerificationAct();
}

function FillCmbEquipmentRepairProvider() {
    VoidTable("CmbEquipmentRepairProvider");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbEquipmentRepairProvider").appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement("option");
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        if (ProviderSelected === Providers[x].Id) {
            option.selected = true;
        }

        document.getElementById("CmbEquipmentRepairProvider").appendChild(option);
    }
}

function FillcmbProviderMaintenanceDefinition() {
    VoidTable("CmbNewMaintainmentProvider");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbNewMaintainmentProvider").appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement("option");
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        if (ProviderSelected === Providers[x].Id) {
            option.selected = true;
        }

        document.getElementById("CmbNewMaintainmentProvider").appendChild(option);
    }
}

function FillcmbProviderCalibrationDefinition() {
    VoidTable("CmbCalibrationExternalProvider");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbCalibrationExternalProvider").appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement("option");
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        if (ProviderCalibrationDefinition === Providers[x].Id) {
            option.selected = true;
        }

        document.getElementById("CmbCalibrationExternalProvider").appendChild(option);
    }
}

function FillCmbProviderCalibrationAct() {
    VoidTable("CmbEquipmentCalibrationActProvider");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbEquipmentCalibrationActProvider").appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement("option");
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        document.getElementById("CmbEquipmentCalibrationActProvider").appendChild(option);
    }
}

function FillCmbProviderVerificationAct() {
    VoidTable("CmbEquipmentVerificationActProvider");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbEquipmentVerificationActProvider").appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement("option");
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        document.getElementById("CmbEquipmentVerificationActProvider").appendChild(option);
    }
}

function FillcmbProviderVerificationDefinition() {
    VoidTable('CmbVerificationExternalProvider');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById('CmbVerificationExternalProvider').appendChild(optionDefault);

    for (var x = 0; x < Providers.length; x++) {
        var option = document.createElement('option');
        option.value = Providers[x].Id;
        option.appendChild(document.createTextNode(Providers[x].Description));
        if (ProviderVerificationDefinition === Providers[x].Id) {
            option.selected = true;
        }

        document.getElementById('CmbVerificationExternalProvider').appendChild(option);
    }
}

// Insert functions for bar item Provider
function ProviderInsert(sender) {
    document.getElementById("TxtProviderNewNameErrorRequired").style.display = "none";
    document.getElementById("TxtProviderNewNameErrorDuplicated").style.display = "none";
    $("#TxtProviderNewName").val("");
    var Selected = 0;
    var dialog = $("#ProviderInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Equipment_Popup_AddProvider_Title + "</h4>",
        "title_html": true,
        "buttons": [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;

                    var duplicated = false;
                    for (var x = 0; x < Providers.length; x++) {
                        if (document.getElementById("TxtProviderNewName").value.toLowerCase() === Providers[x].Description.toLowerCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById("TxtProviderNewNameErrorDuplicated").style.display = "block";
                        ok = false;
                    }
                    else {
                        document.getElementById("TxtProviderNewNameErrorDuplicated").style.display = "none";
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById("TxtProviderNewNameErrorRequired").style.display = "none";
                    document.getElementById("TxtProviderNewNameErrorDuplicated").style.display = "none";
                    $(this).dialog("close");
                    ProviderInsertConfirmed(document.getElementById("TxtProviderNewName").value);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]

    });
}

function ProviderInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var description = "";
    var data = {
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    var newId = 0;
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ProviderActions.asmx/Insert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                Providers.push({ "Id": newId, "Description": newDescription, "Active": true, "Deletable": true });

                // 3.- Modificar la fila de la tabla del popup
                ProviderRenderPopup();
                FillCmbProviders();
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    FillCmbProviders();
}

// Update function for bar item Provider
function ProviderUpdate(sender) {
    document.getElementById('TxtProviderNameErrorRequired').style.display = 'none';
    document.getElementById('TxtProviderNameErrorDuplicated').style.display = 'none';
    $('#TxtProviderName').val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#ProviderUpdateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": Dictionary.Common_Edit,
        "title_html": true,
        "buttons": [
                {
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        var ok = true;
                        if (document.getElementById("TxtProviderName").value === "") {
                            $("#TxtProviderNameErrorRequired").show();
                            ok = false;
                        }
                        else {
                            $("#TxtProviderNameErrorRequired").hide();
                        }

                        var duplicated = false;
                        for (var x = 0; x < Providers.length; x++) {
                            if (document.getElementById("TxtProviderName").value.toLowerCase() === Providers[x].Description.toLowerCase() && Selected !== Providers[x].Id && Providers[x].Active === true) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            $("#TxtProviderNameErrorDuplicated").show();
                            ok = false;
                        }
                        else {
                            $("#TxtProviderNameErrorDuplicated").hide();
                        }


                        if (ok === false) { window.scrollTo(0, 0); return false; }

                        $("#TxtProviderNameErrorRequired").hide();
                        $("#TxtProviderNameErrorDuplicated").hide();
                        $(this).dialog("close");
                        ProviderUpdateConfirmed(Selected, $("#TxtProviderName").val());
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

function ProviderUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var description = '';
    for (var x = 0; x < Providers.length; x++) {
        if (Providers[x].Id === id) {
            description = Providers[x].Description;
            break;
        }
    }
    var data = {
        "providerId": id,
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ProviderActions.asmx/Update",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var y = 0; y < Providers.length; y++) {
        if (Providers[y].Id !== id) {
            temp.push(Providers[y]);
        }
        else {
            var item = Providers[y];
            temp.push({
                "Id": item.Id,
                "Description": newDescription,
                "Active": item.Active,
                "Deletable": item.Delete
            });
        }
    }

    Providers = new Array();
    for (var z = 0; z < temp.length; z++) {
        Providers.push(temp[z]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById("SelectableProvider");
    for (var w = 0; w < target.childNodes.length; w++) {
        if (target.childNodes[w].id === id) {
            target.childNodes[w].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (ProviderSelected === id) {
        document.getElementById("TxtProviderName").value = newDescription;
    }

    FillCmbProviders();
}

// Delete functions for bar item Provider
function ProviderDelete(sender) {
    $("#ProviderName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#ProviderDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    $(this).dialog("close");
                    ProviderDeleteConfirmed(Selected);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function ProviderDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var description = "";
    for (var x = 0; x < Providers.length; x++) {
        if (Providers[x].Id === id) {
            description = Providers[x].Description;
            break;
        }
    }

    var data = {
        "ProviderId": id,
        "description": description,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ProviderActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
                FillCmbProviders();
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var y = 0; y < Providers.length; y++) {
        if (Providers[y].Id !== id) { temp.push(Providers[y]); }
    }

    Providers = new Array();
    for (var z = 0; z < temp.length; z++) {
        Providers.push(temp[z]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById("SelectableProvider");
    for (var w = 0; w < target.childNodes.length; w++) {
        if (target.childNodes[w].id === id) {
            target.childNodes[w].style.display = "none";
            break;
        }
    }

    FillCmbProviders();
}

// Common scripts
function ProviderRenderPopup() {
    VoidTable("SelectableProvider");
    var target = document.getElementById("SelectableProvider");
    Providers.sort(CompareProviders);
    for (var x = 0; x < Providers.length; x++) {
        ProviderPopupRow(Providers[x], target)
    }
}

function CompareProviders(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function ProviderPopupRow(item, target) {
    if (item.Active === false) return;
    var tr = document.createElement("tr");
    tr.id = item.Id;
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    if (ProviderSelected === item.Id) {
        td1.style.fontWeight = "bold";
    }
    td1.appendChild(document.createTextNode(item.Description));

    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if (ProviderSelected === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { ProviderChanged(this); };
    }

    div.appendChild(span1);

    var span2 = document.createElement("span");
    span2.className = "btn btn-xs btn-info";
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement("i");
    i2.className = "icon-edit bigger-120";
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span2);

    if (item.Id < 0) {
        span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else {
        span2.onclick = function () { ProviderUpdate(this); };
    }

    var span3 = document.createElement("span");
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);

    if (ProviderSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else if (item.Id < 0) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else {
        span3.onclick = function () { ProviderDelete(this); };
    }
    div.appendChild(document.createTextNode(' '));
    div.appendChild(span3);
    td2.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}