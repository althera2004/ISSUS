var CostSelected = 0;
var newDescription;
var newAmount;

// Bar popup for bar item Provider
function ShowCostBarPopup(cmb) {
    CostRenderPopup();
    var dialog = $("#dialogCost").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Item_CostDefinitions,
        title_html: true,
        width: 600,
        buttons:
        [
            {
                id: 'BtnCostSave',
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                click: function () { CostInsert(); }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
}

// Common scripts
function CostRenderPopup() {
    VoidTable('SelectableCost');
    var target = document.getElementById('SelectableCost');
    Costs.sort(CompareCosts);
    for (var x = 0; x < Costs.length; x++) {
        CostPopupRow(Costs[x], target)
    }
}

function CompareCosts(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function CostPopupRow(item, target) {
    if (item.Active === false) return;
    var tr = document.createElement('tr');
    tr.id = item.Id;
    var td1 = document.createElement('td');
    var td2 = document.createElement('td');
    td2.align = 'right';
    var tdActions = document.createElement('td');
    if (CostSelected === item.Id) {
        td1.style.fontWeight = 'bold';
        td2.style.fontWeight = 'bold';
    }

    td1.appendChild(document.createTextNode(item.Description));
    td2.appendChild(document.createTextNode(ToMoneyFormat(item.Amount, 2)));

    var div = document.createElement('div');
    var span1 = document.createElement('span');
    span1.className = 'btn btn-xs btn-success';
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement('i');
    i1.className = 'icon-star bigger-120';
    span1.appendChild(i1);

    if (CostSelected === item.Id && 1 === 2) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { CostChanged(this); };
    }

    div.appendChild(span1);

    var span2 = document.createElement('span');
    span2.className = 'btn btn-xs btn-info';
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement('i');
    i2.className = 'icon-edit bigger-120';
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(' '));
    div.appendChild(span2);

    if (item.Id < 0) {
        span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else {
        span2.onclick = function () { CostUpdate(this); };
    }

    var span3 = document.createElement('span');
    span3.className = 'btn btn-xs btn-danger';
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement('i');
    i3.className = 'icon-trash bigger-120';
    span3.appendChild(i3);

    if (CostSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else if (item.Id < 0) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else {
        span3.onclick = function () { CostDelete(this); };
    }
    div.appendChild(document.createTextNode(' '));
    div.appendChild(span3);
    tdActions.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(tdActions);
    target.appendChild(tr);
}

function ResetCostInsertForm()
{
    document.getElementById('TxtCostNewNameLabel').style.color = '#000';
    document.getElementById('TxtCostNewAmountLabel').style.color = '#000';
    document.getElementById('TxtCostNewNameErrorRequired').style.display = 'none';
    document.getElementById('TxtCostNewNameErrorDuplicated').style.display = 'none';
    document.getElementById('TxtCostNewAmountErrorRequired').style.display = 'none';
}

// Insert functions for bar item Cost
function CostInsert(sender) {
    ResetCostInsertForm();
    $('#TxtCostNewName').val('');
    $('#TxtCostNewAmount').val('');
    var Selected = 0;
    var dialog = $("#CostInsertDialog").removeClass('hide').dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_CostDefinition_Popup_AddCost_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    var ok = true;
                    var duplicated = false;
                    ResetCostInsertForm();

                    if ($('#TxtCostNewName').val() === '') {
                        ok = false;
                        document.getElementById('TxtCostNewNameErrorRequired').style.display = '';
                        document.getElementById('TxtCostNewNameLabel').style.color = '#f00';
                    }
                    else {
                        for (var x = 0; x < Costs.length; x++) {
                            if (document.getElementById('TxtCostNewName').value.toLowerCase() == Costs[x].Description.toLowerCase()) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            document.getElementById('TxtCostNewNameErrorDuplicated').style.display = 'block';
                            ok = false;
                        }
                        else {
                            document.getElementById('TxtCostNewNameErrorDuplicated').style.display = 'none';
                        }
                    }

                    if ($('#TxtCostNewAmount').val() === '') {
                        ok = false;
                        document.getElementById('TxtCostNewAmountErrorRequired').style.display = '';
                        document.getElementById('TxtCostNewAmountLabel').style.color = '#f00';
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById('TxtCostNewNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtCostNewNameErrorDuplicated').style.display = 'none';
                    $(this).dialog("close");
                    CostInsertConfirmed();
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
}

function CostInsertConfirmed() {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/CostDefinitionActions.asmx/Insert";
    newDescription = $('#TxtCostNewName').val();
    newAmount = ParseInputValueToNumber($('#TxtCostNewAmount').val());
    var data = {
        'costDefinition':
        {
            'Description': newDescription,
            'Amount': newAmount,
            'CompanyId': Company.Id
        },
        'userId': user.Id
    };

    var newId = 0;
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                Costs.push({ "Id": newId, "Description": newDescription, "Amount": newAmount, "Active": true, "Deletable": true });

                // 3.- Modificar la fila de la tabla del popup
                CostRenderPopup();
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}


// Update function for bar item Cost


function ResetCostUpdateForm() {
    document.getElementById('TxtCostNameLabel').style.color = '#000';
    document.getElementById('TxtCostAmountLabel').style.color = '#000';
    document.getElementById('TxtCostNameErrorRequired').style.display = 'none';
    document.getElementById('TxtCostNameErrorDuplicated').style.display = 'none';
    document.getElementById('TxtCostAmountErrorRequired').style.display = 'none';
}

function CostUpdate(sender) {
    ResetCostUpdateForm();
    $('#TxtCostName').val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    $('#TxtCostAmount').val(sender.parentNode.parentNode.parentNode.childNodes[1].innerHTML);
    CostSelected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#CostUpdateDialog").removeClass('hide').dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: Dictionary.Common_Edit,
        title_html: true,
        buttons: [
                {
                    html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    click: function () {
                        var ok = true;
                        var duplicated = false;
                        ResetCostUpdateForm();
                        if ($('#TxtCostName').val() === '') {
                            ok = false;
                            document.getElementById('TxtCostNameErrorRequired').style.display = '';
                            document.getElementById('TxtCostNameLabel').style.color = '#f00';
                        }
                        else {
                            for (var x = 0; x < Costs.length; x++) {
                                if (document.getElementById('TxtCostName').value.toLowerCase() == Costs[x].Description.toLowerCase() && CostSelected !== Costs[x].Id*1) {
                                    duplicated = true;
                                    break;
                                }
                            }

                            if (duplicated === true) {
                                document.getElementById('TxtCostNameErrorDuplicated').style.display = 'block';
                                ok = false;
                            }
                            else {
                                document.getElementById('TxtCostNameErrorDuplicated').style.display = 'none';
                            }
                        }

                        if ($('#TxtCostAmount').val() === '') {
                            ok = false;
                            document.getElementById('TxtCostAmountErrorRequired').style.display = '';
                            document.getElementById('TxtCostAmountLabel').style.color = '#f00';
                        }

                        if (ok === false) { window.scrollTo(0, 0); return false; }

                        document.getElementById('TxtCostNameErrorRequired').style.display = 'none';
                        document.getElementById('TxtCostNameErrorDuplicated').style.display = 'none';
                        $(this).dialog("close");
                        CostUpdateConfirmed();
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

function CostUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/CostDefinitionActions.asmx/Update";
    newDescription = $('#TxtCostName').val();
    newAmount = ParseInputValueToNumber($('#TxtCostAmount').val());
    var data = {
        'costDefinition':
        {
            'Id': CostSelected,
            'Description': newDescription,
            'Amount': newAmount,
            'CompanyId': Company.Id
        },
        'userId': user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var x = 0; x < Costs.length; x++) {
        if (Costs[x].Id !== CostSelected) {
            temp.push(Costs[x]);
        }
        else {
            var item = Costs[x];
            temp.push({ "Id": item.Id, "Description": newDescription, "Amount": newAmount, "Active": item.Active, "Deletable": item.Delete });
        }
    }

    Costs = new Array();
    for (var x = 0; x < temp.length; x++) {
        Costs.push(temp[x]);
    }

    // 3.- Modificar la fila de la tabla del popup
    CostRenderPopup();
}

// Delete functions for bar item Cost
function CostDelete(sender) {
    $('#CostName').html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $('#CostDeleteDialog').removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Common_Delete,
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    $(this).dialog("close");
                    CostDeleteConfirmed(Selected);
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
}

function CostDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var webMethod = '/Async/CostDefinitionActions.asmx/Inactive';
    var description = '';
    for (var x = 0; x < Costs.length; x++) {
        if (Costs[x].Id === id) {
            description = Costs[x].Description;
            break;
        }
    }

    var data = {
        'costDefinitionId': id,
        'companyId': Company.Id,
        'userId': user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: 'POST',
        url: webMethod,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success !== true) { alertUI(response.d.MessageError); }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var x = 0; x < Costs.length; x++) {
        if (Costs[x].Id !== id) { temp.push(Costs[x]); }
    }

    Costs = new Array();
    for (var x = 0; x < temp.length; x++) {
        Costs.push(temp[x]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById('SelectableCost');
    for (var x = 0; x < target.childNodes.length; x++) {
        if (target.childNodes[x].id == id) {
            target.childNodes[x].style.display = 'none';
            break;
        }
    }
}

// Selection functions for bar item Cost
function CostChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    var cost = GetCostById(id);
    if (cost !== null) {
        $('#TxtIncidentActionCostDescription').val(cost.Description);
        $('#TxtIncidentActionCostAmount').val(cost.Amount);
        $('#TxtIncidentActionCostAmount').focus();
    }

    $("#dialogCost").dialog('close');
}

function GetCostById(id)
{
    for (var x = 0; x < Costs.length; x++) {
        if (Costs[x].Id === id) {
            return Costs[x];
        }
    }

    return null;
}