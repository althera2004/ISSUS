﻿var ProviderSelected = 0;
var Cmb;

// Bar popup for bar item Provider
function ShowProviderBarPopup(cmb) {
    Cmb = cmb;
    ProviderSelected = cmb.val() * 1;
    ProviderRenderPopup();
    var dialog = $("#dialogProvider").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Item_Providers,
        title_html: true,
        width: 600,
        buttons:
        [
            {
                id: 'BtnProviderSave',
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                click: function () { ProviderInsert(); }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
}

// Selection functions for bar item Provider
function ProviderChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    Cmb.val(id);
    $("#dialogProvider").dialog('close');
}

// Insert functions for bar item Provider
function ProviderInsert(sender) {
    document.getElementById('TxtProviderNewNameErrorRequired').style.display = 'none';
    document.getElementById('TxtProviderNewNameErrorDuplicated').style.display = 'none';
    $('#TxtProviderNewName').val('');
    var Selected = 0;
    var dialog = $("#ProviderInsertDialog").removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Equipment_Popup_AddProvider_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    var ok = true;
                    var duplicated = false;
                    for (var x = 0; x < Providers.length; x++) {
                        if (document.getElementById('TxtProviderNewName').value.toLowerCase() == Providers[x].Description.toLowerCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtProviderNewNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtProviderNewNameErrorDuplicated').style.display = 'none';
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById('TxtProviderNewNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtProviderNewNameErrorDuplicated').style.display = 'none';
                    $(this).dialog("close");
                    ProviderInsertConfirmed(document.getElementById('TxtProviderNewName').value);
                }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]

    });
}

function ProviderInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/ProviderActions.asmx/Insert";
    var description = '';
    var data = {
        'description': newDescription,
        'companyId': Company.Id,
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
                Providers.push({ "Id": newId, "Description": newDescription, "Active": true, "Deletable": true });

                // 3.- Modificar la fila de la tabla del popup
                ProviderRenderPopup();
                CmbReporterProvidersFill();
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

    CmbReporterProvidersFill();
}

// Update function for bar item Provider
function ProviderUpdate(sender) {
    document.getElementById('TxtProviderNameErrorRequired').style.display = 'none';
    document.getElementById('TxtProviderNameErrorDuplicated').style.display = 'none';
    $('#TxtProviderName').val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#ProviderUpdateDialog").removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: Dictionary.Common_Edit,
        title_html: true,
        buttons: [
                {
                    html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    click: function () {
                        var ok = true;
                        if (document.getElementById('TxtProviderName').value === '') {
                            document.getElementById('TxtProviderNameErrorRequired').style.display = 'block';
                            ok = false;
                        }
                        else {
                            document.getElementById('TxtProviderNameErrorRequired').style.display = 'none';
                        }

                        var duplicated = false;
                        for (var x = 0; x < Providers.length; x++) {
                            if (document.getElementById('TxtProviderName').value.toLowerCase() == Providers[x].Description.toLowerCase() && Selected != Providers[x].Id && Providers[x].Active === true) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            document.getElementById('TxtProviderNameErrorDuplicated').style.display = 'block';
                            ok = false;
                        }
                        else {
                            document.getElementById('TxtProviderNameErrorDuplicated').style.display = 'none';
                        }


                        if (ok === false) { window.scrollTo(0, 0); return false; }

                        document.getElementById('TxtProviderNameErrorRequired').style.display = 'none';
                        document.getElementById('TxtProviderNameErrorDuplicated').style.display = 'none';
                        $(this).dialog("close");
                        ProviderUpdateConfirmed(Selected, document.getElementById('TxtProviderName').value);
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

function ProviderUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/ProviderActions.asmx/Update";
    var description = '';
    for (var x = 0; x < Providers.length; x++) {
        if (Providers[x].Id === id) {
            description = Providers[x].Description;
            break;
        }
    }
    var data = {
        'ProviderId': id,
        'description': newDescription,
        'companyId': Company.Id,
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
    for (var x2 = 0; x2 < Providers.length; x2++) {
        if (Providers[x2].Id !== id) {
            temp.push(Providers[x2]);
        }
        else {
            var item = Providers[x2];
            temp.push({ "Id": item.Id, "Description": newDescription, "Active": item.Active, "Deletable": item.Delete });
        }
    }

    Providers = new Array();
    for (var x3 = 0; x3 < temp.length; x3++) {
        Providers.push(temp[x3]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById('SelectableProvider');
    for (var x4 = 0; x4 < target.childNodes.length; x4++) {
        if (target.childNodes[x4].id == id) {
            target.childNodes[x4].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (ProviderSelected === id) {
        document.getElementById('TxtProvider').value = newDescription;
    }

    FillCmbProviders();
}

// Delete functions for bar item Provider
function ProviderDelete(sender) {
    $('#ProviderName').html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $('#ProviderDeleteDialog').removeClass("hide").dialog({
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
                    ProviderDeleteConfirmed(Selected);
                }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
}

function ProviderDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var webMethod = '/Async/ProviderActions.asmx/Delete';
    var description = '';
    for (var x = 0; x < Providers.length; x++) {
        if (Providers[x].Id === id) {
            description = Providers[x].Description;
            break;
        }
    }

    var data = {
        'providerId': id,
        'description': description,
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
    for (var x2 = 0; x2 < Providers.length; x2++) {
        if (Providers[x2].Id !== id) { temp.push(Providers[x2]); }
    }

    Providers = new Array();
    for (var x3 = 0; x3 < temp.length; x3++) {
        Providers.push(temp[x3]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById('SelectableProvider');
    for (var x4 = 0; x4 < target.childNodes.length; x4++) {
        if (target.childNodes[x4].id * 1 == id * 1) {
            target.childNodes[x4].style.display = 'none';
            break;
        }
    }

    CmbReporterProvidersFill();
}

// Common scripts
function ProviderRenderPopup() {
    VoidTable('SelectableProvider');
    var target = document.getElementById('SelectableProvider');
    Providers.sort(CompareProviders);
    for (var x = 0; x < Providers.length; x++) {
        ProviderPopupRow(Providers[x], target);
    }
}

function CompareProviders(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) { return -1; }
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) { return 1; }
    return 0;
}

function ProviderPopupRow(item, target) {
    if (item.Active === false) { return; }
    var tr = document.createElement('tr');
    tr.id = item.Id;
    var td1 = document.createElement('td');
    var td2 = document.createElement('td');
    if (ProviderSelected === item.Id) {
        td1.style.fontWeight = 'bold';
    }
    td1.appendChild(document.createTextNode(item.Description));

    var div = document.createElement('div');
    var span1 = document.createElement('span');
    span1.className = 'btn btn-xs btn-success';
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement('i');
    i1.className = 'icon-star bigger-120';
    span1.appendChild(i1);

    if (ProviderSelected === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { ProviderChanged(this); };
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
        span2.onclick = function () { ProviderUpdate(this); };
    }

    var span3 = document.createElement('span');
    span3.className = 'btn btn-xs btn-danger';
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement('i');
    i3.className = 'icon-trash bigger-120';
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