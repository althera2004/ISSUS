var SelectedIncidentActionCostId;
var SelectedIncidentActionCost;

function IncidentActionCostRenderTable(tableName) {
    VoidTable(tableName);
    var target = document.getElementById(tableName);

    if (IncidentActionCosts.length === 0) {
        $("#IncidentActionCostsTableVoid").show();
        $("#" + tableName).parent().hide();
    }
    else {
        $("#IncidentActionCostsTableVoid").hide();
        $("#" + tableName).parent().show();
    }

    var total = 0;
    for (var x = 0; x < IncidentActionCosts.length; x++) {
        if (IncidentActionCosts[x].Active === true) {
            total += IncidentActionCostRenderRow(IncidentActionCosts[x], target);
        }
    }

    $("#NumberCosts").html(IncidentActionCosts.length);
    $("#TotalCosts").html(ToMoneyFormat(total, 2));
}

function IncidentActionCostRenderRow(incidentActionCost, target) {
    var row = document.createElement("TR");
    var tdDescription = document.createElement("TD");
    var tdAmount = document.createElement("TD");
    var tdQuantity = document.createElement("TD");
    var tdTotal = document.createElement("TD");
    var tdResponsible = document.createElement("TD");

    //tdDescription.style.width = "290px";
    tdAmount.style.width = "120px";
    tdQuantity.style.width = "120px";
    tdTotal.style.width = "150px";
	tdResponsible.style.width = "300px";

    row.id = incidentActionCost.Id;

    tdAmount.align = "right";
    tdQuantity.align = "right";
    tdTotal.align = "right";

    tdDescription.appendChild(document.createTextNode(incidentActionCost.Description));
    tdAmount.appendChild(document.createTextNode(ToMoneyFormat(incidentActionCost.Amount, 2)));
    tdQuantity.appendChild(document.createTextNode(ToMoneyFormat(incidentActionCost.Quantity, 2)));
    tdTotal.appendChild(document.createTextNode(ToMoneyFormat(incidentActionCost.Amount * incidentActionCost.Quantity, 2)));
    tdResponsible.appendChild(document.createTextNode(incidentActionCost.Responsible.Value));
    var iconEdit = document.createElement("SPAN");
    iconEdit.className = "btn btn-xs btn-info";
    var innerEdit = document.createElement("I");
    innerEdit.className = "icon-edit bigger-120";
    iconEdit.appendChild(innerEdit);
    iconEdit.onclick = function () { IncidentActionCostEdit(this.parentNode.parentNode.id); };

    var iconDelete = document.createElement("SPAN");
    iconDelete.className = "btn btn-xs btn-danger";
    var innerDelete = document.createElement("I");
    innerDelete.className = "icon-trash bigger-120";
    iconDelete.appendChild(innerDelete);
    iconDelete.onclick = function () { IncidentActionCostDelete(this.parentNode.parentNode.id); };

    var tdActions = document.createElement("TD");

    tdActions.appendChild(iconEdit);
    tdActions.appendChild(document.createTextNode(" "));
    tdActions.appendChild(iconDelete);

    tdTotal.className = "hidden-480";
    tdResponsible.className = "hidden-480";
    tdActions.className = "hidden-480";
    tdActions.style.width = "90px";

    row.appendChild(tdDescription);
    row.appendChild(tdAmount);
    row.appendChild(tdQuantity);
    row.appendChild(tdTotal);
    row.appendChild(tdResponsible);
    row.appendChild(tdActions);
    target.appendChild(row);

    return incidentActionCost.Amount * incidentActionCost.Quantity;
}

function IncidentActionCostSetPopupFormFill() {
    ClearFieldTextMessages("CmbIncidentActionCostDescription");
    ClearFieldTextMessages("TxtIncidentActionCostDescription");
    ClearFieldTextMessages("TxtIncidentActionCostAmount");
    ClearFieldTextMessages("TxtIncidentActionCostQuantity");
    ClearFieldTextMessages("CmdIncidentActionCostResponsible");
    SelectedIncidentActionCost = IncidentActionCostGetById(SelectedIncidentActionCostId, IncidentActionCosts);
    $("#TxtIncidentActionCostDescription").val(SelectedIncidentActionCost.Description);
    $("#TxtIncidentActionCostAmount").val(ToMoneyFormat(SelectedIncidentActionCost.Amount, 2));
    $("#TxtIncidentActionCostQuantity").val(ToMoneyFormat(SelectedIncidentActionCost.Quantity, 2));
    $("#CmdIncidentActionCostResponsible").val(SelectedIncidentActionCost.Responsible.Id);
    /*document.getElementById("RIncidentActionCostRow").style.display = "none";
    document.getElementById("RIncidentActionCostNew").checked = true;*/
    RIncidentActionCostChanged();
}

function IncidentActionCostSetPopupFormReset(newIncidentActionCost) {
    ClearFieldTextMessages("CmbIncidentActionCostDescription");
    ClearFieldTextMessages("TxtIncidentActionCostDescription");
    ClearFieldTextMessages("TxtIncidentActionCostAmount");
    ClearFieldTextMessages("TxtIncidentActionCostQuantity");
    ClearFieldTextMessages("CmdIncidentActionCostResponsible");
    /*document.getElementById("RIncidentActionCostRow").style.display = "";
    CmbIncidentActionCostDescriptionFill();
    if (CompanyIncidentActionCosts.length === 0) {
        $("#RIncidentActionCostBased").prop("disabled", true);
        document.getElementById("RIncidentActionCostNew").checked = true;
    }
    else {
        $("#RIncidentActionCostBased").prop("disabled", false);
        document.getElementById("RIncidentActionCostBased").checked = true;
    }

    RIncidentActionCostChanged();*/
    $("#TxtIncidentActionCostDescription").val("");
    $("#TxtIncidentActionCostAmount").val("");
    $("#TxtIncidentActionCostQuantity").val("");
    $("#CmbCmbIncidentActionCostDescription").val(0);
    $("#CmdIncidentActionCostResponsible").val(ApplicationUser.Employee.Id);
}

function RIncidentActionCostChanged() {
    return;
    var IncidentActionCostNew = document.getElementById("RIncidentActionCostNew").checked;

    document.getElementById("CmbIncidentActionCostDescriptionRow").style.display = IncidentActionCostNew ? "none" : "";
    document.getElementById("TxtIncidentActionCostDescriptionRow").style.display = IncidentActionCostNew ? "" : "none";
    $("#TxtIncidentActionCostAmount").prop("readonly", !IncidentActionCostNew);
}

function CmbIncidentActionCostDescriptionChanged() {
    $("#TxtIncidentActionCostAmount").val("");
    var incidentActionCostId = $("#CmbIncidentActionCostDescription").val() * 1;
    for (var x = 0; x < CompanyIncidentActionCosts.length; x++) {
        if (CompanyIncidentActionCosts[x].Id === incidentActionCostId) {
            $("#TxtIncidentActionCostAmount").val(ToMoneyFormat(CompanyIncidentActionCosts[x].Amount, 2));
        }
    }
}

function IncidentActionCostEdit(id) {
    SelectedIncidentActionCostId = id * 1;
    IncidentActionCostSetPopupFormFill();
    var dialog = $("#dialogNewCost").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_IncidentActionCost_PopupTitle_Update + "</h4>",
        "title_html": true,
        "width": 500,
        "buttons":
        [
            {
                "id": "BtnNewAddresSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Change,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    IncidentActionCostSave();
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

function ShowNewCostPopup(actionSelected) {
    SelectedIncidentActionCostId = 0;
    IncidentActionCostSetPopupFormReset();
    var dialog = $("#dialogNewCost").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_IncidentCost_PopupTitle_Add + "</h4>",
        "title_html": true,
        "width": 500,
        "buttons":
        [
            {
                "id": "BtnNewAddresSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    IncidentActionCostSave();
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

function IncidentActionCostValidateForm() {
    var ok = true;
    //var based = document.getElementById('RIncidentActionCostBased').checked;

    ClearFieldTextMessages('CmbIncidentActionCostDescription');
    ClearFieldTextMessages('TxtIncidentActionCostDescription');
    ClearFieldTextMessages('TxtIncidentActionCostAmount');
    ClearFieldTextMessages('TxtIncidentActionCostQuantity');
    ClearFieldTextMessages('CmdIncidentActionCostResponsible');

    //if (based === true) {
    //    if (!RequiredFieldCombo('CmbIncidentActionCostDescription')) {
    //        ok = false;
    //    }
    //}
    //else {
        if (!RequiredFieldText('TxtIncidentActionCostDescription')) {
            ok = false;
        }
    //}

    if (!RequiredFieldText('TxtIncidentActionCostAmount')) {
        ok = false;
    }

    if (!RequiredFieldText('TxtIncidentActionCostQuantity')) {
        ok = false;
    }

    if (!RequiredFieldCombo('CmdIncidentActionCostResponsible')) {
        ok = false;
    }

    return ok;
}

function IncidentActionCostSave() {
    var ok = IncidentActionCostValidateForm();
    if (ok === false) {
        return false;
    }

    var Description = $('#TxtIncidentActionCostDescription').val();
    /*if (document.getElementById('RIncidentActionCostBased').checked) {
        var id = $('#CmbIncidentActionCostDescription').val();
        var IncidentActionCostSearched = IncidentActionCostGetById(id, CompanyIncidentActionCosts);
        if (IncidentActionCostSearched !== null) {
            Description = IncidentActionCostSearched.Description;
        }
    }*/

    var IncidentActionCost = SelectedIncidentActionCost;

    var amount = ParseInputValueToNumber($('#TxtIncidentActionCostAmount').val());
    var quantity = ParseInputValueToNumber($('#TxtIncidentActionCostQuantity').val());

    var finalAction;
    if (typeof IncidentAction === 'undefined') {
        if (typeof Action !== 'undefined') {
            finalAction = Action;
        }
    }
    else {
        finalAction = IncidentAction;
    }

    SelectedIncidentActionCost =
    {
        "Id": SelectedIncidentActionCostId,
        "IncidentActionId": finalAction.Id,
        "CompanyId": Company.Id,
        "Description": Description,
        "Amount": amount,
        "Quantity": quantity,
        "Responsible": { "Id": $('#CmdIncidentActionCostResponsible').val() * 1, Value: $('#CmdIncidentActionCostResponsible option:selected').text(), Active: true },
        "Active": true
    };

    if (SelectedIncidentActionCostId === 0) {
        var dataInsert = { incidentActionCost: SelectedIncidentActionCost, userId: user.Id };
        $.ajax({
            type: "POST",
            url: "/Async/IncidentActionCostActions.asmx/Insert",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataInsert, null, 2),
            success: function (msg) {
                SelectedIncidentActionCost.Id = msg.d.MessageError * 1;
                IncidentActionCosts.push(SelectedIncidentActionCost);
                CompanyIncidentActionCosts.push(SelectedIncidentActionCost);
                IncidentActionCostRenderTable('IncidentActionCostsTableData');
                $("#dialogNewCost").dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var dataUpdate = { newIncidentActionCost: SelectedIncidentActionCost, oldIncidentActionCost: IncidentActionCost, userId: user.Id };
        $.ajax({
            type: "POST",
            url: "/Async/IncidentActionCostActions.asmx/Update",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataUpdate, null, 2),
            success: function (msg) {
                UpdateIncidentActionCosts(SelectedIncidentActionCost);
                UpdateCompanyIncidentActionCosts(SelectedIncidentActionCost);
                IncidentActionCostRenderTable('IncidentActionCostsTableData');
                $("#dialogNewCost").dialog('close');
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function IncidentActionCostGetById(id, incidentActionCostsList) {
    for (var x = 0; x < incidentActionCostsList.length; x++) {
        if (incidentActionCostsList[x].Id === id * 1) {
            return incidentActionCostsList[x];
        }
    }
    return null;
}

function CmbIncidentActionCostDescriptionFill() {
    var target = document.getElementById('CmbIncidentActionCostDescription');
    VoidTable('CmbIncidentActionCostDescription');
    var optionDefault = document.createElement('OPTION');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);

    for (var x = 0; x < CompanyIncidentActionCosts.length; x++) {
        if (CompanyIncidentActionCosts[x].Active === true) {
            var found = false;
            for (var y = 0; y < target.childNodes.length; y++) {
                if (target.childNodes[y].innerHTML == CompanyIncidentActionCosts[x].Description + ' - ' + ToMoneyFormat(CompanyIncidentActionCosts[x].Amount, 2)) {
                    found = true;
                    break;
                }
            }

            if (found === false) {
                var option = document.createElement('OPTION');
                option.value = CompanyIncidentActionCosts[x].Id;
                option.appendChild(document.createTextNode(CompanyIncidentActionCosts[x].Description + ' - ' + ToMoneyFormat(CompanyIncidentActionCosts[x].Amount, 2)));
                target.appendChild(option);
            }
        }
    }
}

function UpdateIncidentActionCosts(indicentActionCost) {
    var temp = new Array();
    for (var x = 0; x < IncidentActionCosts.length; x++) {
        if (IncidentActionCosts[x].Id === indicentActionCost.Id) {
            temp.push(indicentActionCost);
        }
        else {
            temp.push(IncidentActionCosts[x]);
        }
    }

    IncidentActionCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        IncidentActionCosts.push(temp[x2]);
    }
}

function DeleteIncidentActionCosts(indicentActionCost) {
    var temp = new Array();
    for (var x3 = 0; x3 < IncidentActionCosts.length; x3++) {
        if (IncidentActionCosts[x3].Id !== indicentActionCost.Id) {
            temp.push(IncidentActionCosts[x3]);
        }
    }

    IncidentActionCosts = new Array();
    for (var x4 = 0; x4 < temp.length; x4++) {
        IncidentActionCosts.push(temp[x4]);
    }
}

function UpdateCompanyIncidentActionCosts(indicentActionCost) {
    var temp = new Array();
    for (var x = 0; x < CompanyIncidentActionCosts.length; x++) {
        if (CompanyIncidentActionCosts[x].Id === indicentActionCost.Id) {
            temp.push(indicentActionCost);
        }
        else {
            temp.push(CompanyIncidentActionCosts[x]);
        }
    }

    CompanyIncidentActionCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        CompanyIncidentActionCosts.push(temp[x2]);
    }
}

function DeleteCompanyIncidentActionCosts(indicentActionCost) {
    var temp = new Array();
    for (var x = 0; x < CompanyIncidentActionCosts.length; x++) {
        if (CompanyIncidentActionCosts[x].Id !== indicentActionCost.Id) {
            temp.push(CompanyIncidentActionCosts[x]);
        }
    }

    CompanyIncidentActionCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        CompanyIncidentActionCosts.push(temp[x2]);
    }
}

function IncidentActionCostDelete(id) {
    SelectedIncidentActionCostId = id * 1;
    SelectedIncidentActionCost = IncidentActionCostGetById(SelectedIncidentActionCostId, IncidentActionCosts);
    if (SelectedIncidentActionCost === null) { return false; }
    document.getElementById('dialogIncidentActionCostDeleteName').innerHTML = SelectedIncidentActionCost.Description + ' - ' + decimalFormat(SelectedIncidentActionCost.Amount);
    var dialog = $("#dialogIncidentActionCostDelete").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_IncidentActionCost_PopupTitle_Delete + '</h4>',
        title_html: true,
        width: 500,
        buttons:
        [
            {
                id: 'BtnNewAddresSave',
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    IncidentActionCostDelteConfirmed();
                }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function IncidentActionCostDelteConfirmed() {
    var webMethod = "/Async/IncidentActionCostActions.asmx/Delete";
    var data = { incidentId: SelectedIncidentActionCostId, reason: '', userId: user.Id, companyId: Company.Id };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            DeleteIncidentCosts(SelectedIncidentActionCost);
            //DeleteCompanyIncidentCosts(SelectedIncidentActionCost);
            IncidentActionCostRenderTable('IncidentActionCostsTableData');
            $("#dialogIncidentActionCostDelete").dialog('close');
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function DeleteIncidentCosts(indicentCost) {
    var temp = new Array();
    for (var x = 0; x < IncidentActionCosts.length; x++) {
        if (IncidentActionCosts[x].Id !== indicentCost.Id) {
            temp.push(IncidentActionCosts[x]);
        }
    }

    IncidentActionCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        IncidentActionCosts.push(temp[x2]);
    }
}