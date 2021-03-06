﻿var SelectedIncidentCostId;
var SelectedIncidentCost;
var filter = "IA";

function FilterChanged() {
    if (document.getElementById("Chk1") !== null) {
        filter = "";

        // Alex: Mantener al menos un check seleccionado
        $("#Chk1").removeAttr("disabled");
        $("#Chk2").removeAttr("disabled");
        if (document.getElementById("Chk1").checked === true && document.getElementById("Chk2").checked === false) {
            $("#Chk1").attr("disabled", "disabled");
        }

        if (document.getElementById("Chk1").checked === false && document.getElementById("Chk2").checked === true) {
            $("#Chk2").attr("disabled", "disabled");
        }

        if (document.getElementById("Chk1").checked === true) { filter += "I"; }
        if (document.getElementById("Chk2").checked === true) { filter += "A"; }
        IncidentCostRenderTable("IncidentCostsTableData");
    }
}

function IncidentCostRenderTable(tableName) {
    VoidTable(tableName);
    var target = document.getElementById(tableName);
    var total = 0;
    var count = 0;
    for (var x = 0; x < IncidentCosts.length; x++) {
        if (IncidentCosts[x].Active === true) {
            var show = true;
            if (IncidentCosts[x].Source === "A") {
                if (typeof user.Grants.IncidentActions === "undefined" || user.Grants.IncidentActions.Read === false) {
                    show = false;
                }
            }

            var source = IncidentCosts[x].Source;
            if (source === "A" && filter.indexOf("A") === -1) { show = false; }
            if (source.indexOf("I") !== -1 && filter.indexOf("I") === -1) { show = false; }

            if (show === true) {
                total += IncidentCostRenderRow(IncidentCosts[x], target);
                count++;
            }
        }
    }

    if (count === 0) {
        $("#IncidentCostsTableVoid").show();
        $("#" + tableName).hide();
    }
    else {
        $("#IncidentCostsTableVoid").hide();
        $("#" + tableName).show();
    }

    $("#NumberCosts").html(count);
    $("#TotalCosts").html(ToMoneyFormat(total, 2));
}

function IncidentCostRenderRow(incidentCost, target) {
    console.log(incidentCost);
    var row = document.createElement("TR");
    var tdDescription = document.createElement("TD");
    var tdDate = document.createElement("TD");
    var tdAmount = document.createElement("TD");
    var tdQuantity = document.createElement("TD");
    var tdTotal = document.createElement("TD");
    var tdResponsible = document.createElement("TD");

    row.id = incidentCost.Id;

    tdDate.align = "center";
    tdAmount.align = "right";
    tdQuantity.align = "right";
    tdTotal.align = "right";

    tdDate.style.width = "100px";
    tdAmount.style.width = "90px";
    tdQuantity.style.width = "90px";
    tdTotal.style.width = "120px";
    tdResponsible.style.width = "200px";

    var fecha = "";
    if (incidentCost.Date !== null) {
        console.log();
        if (typeof incidentCost.Date === "string") {
            fecha = FormatDate(GetDateYYYYMMDD(incidentCost.Date.toString()), "/");
        }
        else {
            fecha = FormatDate(incidentCost.Date, "/");
        }
    }

    tdDescription.appendChild(document.createTextNode(incidentCost.Description));
    tdDate.appendChild(document.createTextNode(fecha));
    tdAmount.appendChild(document.createTextNode(ToMoneyFormat(incidentCost.Amount,2)));
    tdQuantity.appendChild(document.createTextNode(ToMoneyFormat(incidentCost.Quantity,2)));
    tdTotal.appendChild(document.createTextNode(ToMoneyFormat(incidentCost.Amount * incidentCost.Quantity, 2)));
    tdResponsible.appendChild(document.createTextNode(incidentCost.Responsible.Value));

    var tdActions = document.createElement("TD");
    tdActions.style.width = "90px";
   
    if (typeof Incident.ClosedOn !== "undefined" && Incident.ClosedOn !== null && Incident.ClosedOn !== "") {
        // La acción esta cerrada y no se puede editar ni eliminar el coste
    }
    else {
        // Hay que tener en cuenta que si el coste es de una acción y está cerrada no se puede editar
        var showButtons = true;
        if (incidentCost.Source === "A") {
            if (IncidentAction.ClosedOn !== null) {
                showButtons = false;
            }
        }

        if (showButtons === true) {
            //Botón Editar Coste
            var iconEdit = document.createElement("SPAN");
            iconEdit.className = "btn btn-xs btn-info";
            var innerEdit = document.createElement("I");
            innerEdit.className = "icon-edit bigger-120";
            iconEdit.appendChild(innerEdit);
            iconEdit.onclick = function () { IncidentCostEdit(this.parentNode.parentNode.id); };
            tdActions.appendChild(iconEdit);
            //Botón Eliminar Coste
            var iconDelete = document.createElement("SPAN");
            iconDelete.className = "btn btn-xs btn-danger";
            var innerDelete = document.createElement("I");
            innerDelete.className = "icon-trash bigger-120";
            iconDelete.appendChild(innerDelete);
            iconDelete.onclick = function () { IncidentCostDelete(this.parentNode.parentNode.id); };
            tdActions.appendChild(document.createTextNode(" "));
            tdActions.appendChild(iconDelete);
        }
    }

    tdTotal.className = "hidden-480";
    tdResponsible.className = "hidden-480";
    tdActions.className = "hidden-480";

    row.appendChild(tdDescription);
    row.appendChild(tdDate);
    row.appendChild(tdAmount);
    row.appendChild(tdQuantity);
    row.appendChild(tdTotal);
    row.appendChild(tdResponsible);
    row.appendChild(tdActions);
    target.appendChild(row);

    return incidentCost.Amount * incidentCost.Quantity;
}

function IncidentCostSetPopupFormFill() {
    ClearFieldTextMessages("TxtIncidentActionCostDescription");
    ClearFieldTextMessages("TxtIncidentActionCostAmount");
    ClearFieldTextMessages("TxtIncidentCostQuantity");
    ClearFieldTextMessages("CmdIncidentCostResponsible");
    SelectedIncidentCost = IncidentCostGetById(SelectedIncidentCostId, IncidentCosts);
    $("#TxtIncidentActionCostDescription").val(SelectedIncidentCost.Description);
    $("#TxtIncidentActionCostAmount").val(ToMoneyFormat(SelectedIncidentCost.Amount, 2));
    $("#TxtIncidentCostQuantity").val(ToMoneyFormat(SelectedIncidentCost.Quantity, 2));
    if (typeof SelectedIncidentCost.Date === "string") {
        $("#TxtIncidentCostDate").val(FormatDate(GetDateYYYYMMDD(SelectedIncidentCost.Date), "/"));
    }
    else {
        $("#TxtIncidentCostDate").val(FormatDate(SelectedIncidentCost.Date, "/"));
    }
    $("#CmdIncidentCostResponsible").val(SelectedIncidentCost.Responsible.Id);
    $("#TxtIncidentCostDateErrorRequired").hide();
    $("#TxtIncidentCostDateErrorMalformed").hide();
    $("#TxtIncidentCostDateErrorRange").hide();
}

function IncidentCostSetPopupFormReset(newIncidentCost) {
    ClearFieldTextMessages("TxtIncidentActionCostDescription");
    ClearFieldTextMessages("TxtIncidentActionCostAmount");
    ClearFieldTextMessages("TxtIncidentCostQuantity");
    ClearFieldTextMessages("CmdIncidentCostResponsible");

    $("#TxtIncidentActionCostDescription").val("");
    $("#TxtIncidentActionCostAmount").val("");
    $("#TxtIncidentCostQuantity").val(1);
    $("#CmbCmbIncidentCostDescription").val(0);
    $("#CmdIncidentCostResponsible").val(ApplicationUser.Employee.Id);
    $("#TxtIncidentCostDate").val("");
    $("#TxtIncidentCostDateErrorRequired").hide();
    $("#TxtIncidentCostDateErrorMalformed").hide();
    $("#TxtIncidentCostDateErrorRange").hide();
}

function RIncidentCostChanged() {
    return;
    /*var IncidentCostNew = document.getElementById("RIncidentCostNew").checked;

    document.getElementById("CmbIncidentCostDescriptionRow").style.display = IncidentCostNew ? "none" : "";
    document.getElementById("TxtIncidentCostDescriptionRow").style.display = IncidentCostNew ? "" : "none";
    $("#TxtIncidentCostAmount").prop("readonly", !IncidentCostNew);*/
}

function CmbIncidentCostDescriptionChanged() {
    $("#TxtIncidentCostAmount").val("");
    var incidentCostId = $("#CmbIncidentCostDescription").val() * 1;
    for (var x = 0; x < CompanyIncidentCosts.length; x++) {
        if (CompanyIncidentCosts[x].Id === incidentCostId) {
            $("#TxtIncidentCostAmount").val(ToMoneyFormat(CompanyIncidentCosts[x].Amount, 2));
        }
    }
}

function IncidentCostEdit(id) {
    SelectedIncidentCostId = id * 1;
    IncidentCostSetPopupFormFill();
    $("#dialogNewCost").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_IncidentCost_PopupTitle_Update + "</h4>",
        "title_html": true,
        "width": 500,
        "buttons":
        [
            {
                "id": "BtnEditCostSaveOk",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Change,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    IncidentCostSave();
                }
            },
            {
                "id": "BtnEditCostSaveCancel",
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
    SelectedIncidentCostId = 0;
    IncidentCostSetPopupFormReset();
    $("#dialogNewCost").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_IncidentCost_PopupTitle_Add + "</h4>",
        "title_html": true,
        "width": 500,
        "buttons":
        [
            {
                "id": "BtnNewCostSaveOk",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    IncidentCostSave();
                }
            },
            {
                "id": "BtnNewCostSaveCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]

    });
}

function IncidentCostValidateForm() {
    var ok = true;
    ClearFieldTextMessages("CmbIncidentCostDescription");
    ClearFieldTextMessages("TxtIncidentCostDescription");
    ClearFieldTextMessages("TxtIncidentCostAmount");
    ClearFieldTextMessages("TxtIncidentCostQuantity");
    ClearFieldTextMessages("CmdIncidentCostResponsible");

    $("#TxtIncidentCostDateLabel").css("color", "#000");
    $("#TxtIncidentCostDateErrorRequired").hide();
    $("#TxtIncidentCostDateErrorMalformed").hide();
    $("#TxtIncidentCostDateErrorRange").hide();

    if (!RequiredFieldText("TxtIncidentActionCostDescription")) {
        ok = false;
    }

    if (!RequiredFieldText("TxtIncidentActionCostAmount")) {
        ok = false;
    }

    if (!RequiredFieldText("TxtIncidentCostQuantity")) {
        ok = false;
    }

    if (!RequiredFieldCombo("CmdIncidentCostResponsible")) {
        ok = false;
    }

    if ($("#TxtIncidentCostDate").val() === "") {
        ok = false;
        $("#TxtIncidentCostDateLabel").css("color", Color.Error);
        $("#TxtIncidentCostDateErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtIncidentCostDate").val()) === false) {
            ok = false;
            $("#TxtIncidentCostDateLabel").css("color", Color.Error);
            $("#TxtIncidentCostDateErrorMalformed").show();
        }
        else {
            var ad = GetDate($("#TxtIncidentCostDate").val(), "/", false);
            var d = GetDate($("#TxtWhatHappenedDate").val(), "/", false);
            if (ad < d) {
                ok = false;
                $("#TxtIncidentCostDateLabel").css("color", Color.Error);
                $("#TxtIncidentCostDateErrorRange").html(Dictionary.Item_Incident_Cost_Error_Range + " " + $("#TxtWhatHappenedDate").val());
                $("#TxtIncidentCostDateErrorRange").show();
            }
        }
    }

    return ok;
}

function IncidentCostSave() {
    var ok = IncidentCostValidateForm();
    if (ok === false) {
        return false;
    }

    var Description = $("#TxtIncidentActionCostDescription").val();
    var IncidentCost = SelectedIncidentCost;

    if (typeof IncidentCost !== "undefined") {
        if (typeof IncidentCost.Date === "string") {
            IncidentCost.Date = GetDateYYYYMMDD(IncidentCost.Date);
        }
    }

    var amount = ParseInputValueToNumber($("#TxtIncidentActionCostAmount").val());
    var quantity = ParseInputValueToNumber($("#TxtIncidentCostQuantity").val());

    SelectedIncidentCost =
    {
        "Id": SelectedIncidentCostId,
        "IncidentId": IncidentId,
        "BusinessRiskId": BusinessRiskId,
        "CompanyId": Company.Id,
        "Description": Description,
        "Date": GetDate($("#TxtIncidentCostDate").val(),"/", false),
        "Amount": amount,
        "Quantity": quantity,
        "Responsible": { "Id": $("#CmdIncidentCostResponsible").val() * 1, Value: $("#CmdIncidentCostResponsible option:selected").text(), Active: true },
        "Active": true,
        "Source": "IA"
    };

    if (SelectedIncidentCostId === 0) {
        var dataInsert = {
            "incidentCost": SelectedIncidentCost,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/IncidentCostActions.asmx/Insert",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(dataInsert, null, 2),
            "success": function (msg) {
                SelectedIncidentCost.Id = msg.d.MessageError * 1;
                IncidentCosts.push(SelectedIncidentCost);
                CompanyIncidentCosts.push(SelectedIncidentCost);
                IncidentCostRenderTable("IncidentCostsTableData");
                $("#dialogNewCost").dialog("close");
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
    else {
        var dataUpdate = { 
            "newIncidentCost": SelectedIncidentCost,
            "oldIncidentCost": IncidentCost,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/IncidentCostActions.asmx/Update",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(dataUpdate, null, 2),
            "success": function (msg) {
                UpdateIncidentCosts(SelectedIncidentCost);
                UpdateCompanyIncidentCosts(SelectedIncidentCost);
                IncidentCostRenderTable("IncidentCostsTableData");
                $("#dialogNewCost").dialog("close");
            },
            "error": function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function IncidentCostGetById(id, incidentCostsList) {
    for (var x = 0; x < incidentCostsList.length; x++) {
        if (incidentCostsList[x].Id === id * 1) {
            return incidentCostsList[x];
        }
    }
    return null;
}

function CmbIncidentCostDescriptionFill() {
    return;
    /*var target = document.getElementById('CmbIncidentCostDescription');
    VoidTable('CmbIncidentCostDescription');
    var optionDefault = document.createElement('OPTION');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);

    for (var x = 0; x < CompanyIncidentCosts.length; x++) {
        if (CompanyIncidentCosts[x].Active === true) {
            var found = false;
            for (var y = 0; y < target.childNodes.length; y++) {
                if (target.childNodes[y].innerHTML == CompanyIncidentCosts[x].Description + ' - ' + ToMoneyFormat(CompanyIncidentCosts[x].Amount,2)) {
                    found = true;
                    break;
                }
            }

            if (found === false) {
                var option = document.createElement('OPTION');
                option.value = CompanyIncidentCosts[x].Id;
                option.appendChild(document.createTextNode(CompanyIncidentCosts[x].Description + ' - ' + ToMoneyFormat(CompanyIncidentCosts[x].Amount,2)));
                target.appendChild(option);
            }
        }
    }*/
}

function UpdateIncidentCosts(indicentCost) {
    var temp = new Array();
    for (var x = 0; x < IncidentCosts.length; x++) {
        if (IncidentCosts[x].Id === indicentCost.Id) {
            temp.push(indicentCost);
        }
        else {
            temp.push(IncidentCosts[x]);
        }
    }

    IncidentCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        IncidentCosts.push(temp[x2]);
    }
}

function DeleteIncidentCosts(indicentCost) {
    var temp = new Array();
    for (var x = 0; x < IncidentCosts.length; x++) {
        if (IncidentCosts[x].Id !== indicentCost.Id) {
            temp.push(IncidentCosts[x]);
        }
    }

    IncidentCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        IncidentCosts.push(temp[x2]);
    }
}

function UpdateCompanyIncidentCosts(indicentCost) {
    var temp = new Array();
    for (var x = 0; x < CompanyIncidentCosts.length; x++) {
        if (CompanyIncidentCosts[x].Id === indicentCost.Id) {
            temp.push(indicentCost);
        }
        else {
            temp.push(CompanyIncidentCosts[x]);
        }
    }

    CompanyIncidentCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        CompanyIncidentCosts.push(temp[x2]);
    }
}

function DeleteCompanyIncidentCosts(indicentCost) {
    var temp = new Array();
    for (var x = 0; x < CompanyIncidentCosts.length; x++) {
        if (CompanyIncidentCosts[x].Id !== indicentCost.Id) {
            temp.push(CompanyIncidentCosts[x]);
        }
    }

    CompanyIncidentCosts = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        CompanyIncidentCosts.push(temp[x2]);
    }
}

function IncidentCostDelete(id) {
    SelectedIncidentCostId = id * 1;
    SelectedIncidentCost = IncidentCostGetById(SelectedIncidentCostId, IncidentCosts);
    if (SelectedIncidentCost === null) { return false; }
    $("#dialogIncidentCostDeleteName").html(SelectedIncidentCost.Description + " - " + decimalFormat(SelectedIncidentCost.Amount));
    var dialog = $("#dialogIncidentCostDelete").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_IncidentCost_PopupTitle_Delete + "</h4>",
        "title_html": true,
        "width": 500,
        "buttons":
        [
            {
                "id": "BtnCostDeleteOk",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    IncidentCostDeleteConfirmed();
                }
            },
            {
                "id": "BtnCostDeleteCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function IncidentCostDeleteConfirmed() {
    var data = {
        "incidentId": SelectedIncidentCostId,
        "reason": "",
        "userId": user.Id,
        "companyId": Company.Id
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentCostActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            DeleteIncidentCosts(SelectedIncidentCost);
            DeleteCompanyIncidentCosts(SelectedIncidentCost);
            IncidentCostRenderTable("IncidentCostsTableData");
            $("#dialogIncidentCostDelete").dialog("close");
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}