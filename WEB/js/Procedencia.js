function CmbProcedenciaChanged() {
    procedenciaSelected = document.getElementById('CmbProcedencia').value * 1;
    var text = '';
    for (var x = 0; x < procedencias.length; x++) {
        if (procedencias[x].Id == procedenciaSelected) {
            text = procedencias[x].Description;
            break;
        }
    }

    document.getElementById('TxtProcedencia').value = text;
}

function FillCmbProcedencia() {
    VoidTable('CmbProcedencia');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById('CmbProcedencia').appendChild(optionDefault);

    for (var x = 0; x < procedencias.length; x++) {
        var option = document.createElement('option');
        option.value = procedencias[x].Id;
        option.appendChild(document.createTextNode(procedencias[x].Description));
        if (procedenciaSelected == procedencias[x].Id) {
            option.selected = true;
        }

        document.getElementById('CmbProcedencia').appendChild(option);
    }
}

function RenderProcedenciasTable() {
    // Cargar las procedencias en la tabla
    var target = document.getElementById('ProcedenciaSelectable');
    procedencias.sort(CompareDocumentProcedencia);
    VoidTable('ProcedenciaSelectable');
    for (var x = 0; x < procedencias.length; x++) {
        var procedencia = procedencias[x];
        var tr = document.createElement('tr');
        tr.id = procedencia.Id;
        var td1 = document.createElement('td');
        var td2 = document.createElement('td');
        if (procedenciaSelected === procedencia.Id) { td1.style.fontWeight = 'bold'; }
        td1.appendChild(document.createTextNode(procedencia.Description));

        var div = document.createElement('div');
        var span1 = document.createElement('span');
        span1.className = 'btn btn-xs btn-success';
        span1.title = Dictionary["Common_SelectAll"];
        var i1 = document.createElement('i');
        i1.className = 'icon-star bigger-120';
        span1.appendChild(i1);

        if (procedenciaSelected === procedencia.Id) {
            span1.onclick = function () { alertUI(Dictionary.Common_Selected,'dialogProcedencia'); }
        }
        else {
            span1.onclick = function () { ProcedenciaChanged(this); };
        }

        div.appendChild(span1);

        var span2 = document.createElement('span');
        span2.className = 'btn btn-xs btn-info';
        span2.title = Dictionary.Common_Edit;
        var i2 = document.createElement('i');
        i2.className = 'icon-edit bigger-120';
        span2.appendChild(i2);
        if (procedencia.Editable === false) {
            span2.onclick = function () { alertUI(Dictionary.Common_Error_NoEditable,'dialogProcedencia'); }
        }
        else {
            span2.onclick = function () { ProcedenciaUpdate(this); };
        }
        div.appendChild(document.createTextNode(' '));
        div.appendChild(span2);

        var span3 = document.createElement('span');
        span3.className = 'btn btn-xs btn-danger';

        span3.title = Dictionary.Common_Delete;
        var i3 = document.createElement('i');
        i3.className = 'icon-trash bigger-120';
        span3.appendChild(i3);
        if (procedencia.Deletable === false || procedenciaSelected == procedencia.Id) {
            span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable, 'dialogProcedencia'); }
        }
        else {
            span3.onclick = function () { ProcedenciaDelete(this); };
        }
        div.appendChild(document.createTextNode(' '));
        div.appendChild(span3);
        td2.appendChild(div);


        tr.appendChild(td1);
        tr.appendChild(td2);
        target.appendChild(tr);
    }
}

function CompareDocumentProcedencia(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) { return -1; }
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) { return 1; }
    return 0;
}

function ProcedenciaChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#dialogProcedencia").dialog('close');
    procedenciaSelected = id;
    SetProcedenciaText();
    FillCmbProcedencia();
}

function SetProcedenciaText() {
    for (var x = 0; x < procedencias.length; x++) {
        if (procedencias[x].Id === procedenciaSelected) {
            document.getElementById('TxtProcedencia').value = procedencias[x].Description;
            break;
        }
    }
}

function SetProcedencia(e) {
    var procedenciaId = e.target.id.split('-')[1];
    var comboItems = document.getElementById('CmbProcedencia').childNodes
    for (var x = 0; x < comboItems.length; x++) {
        var item = comboItems[x];
        if (item.tagName == 'OPTION') {
            if (item.value == procedenciaId) {
                item.selected = true;
            }
            else {
                item.selected = false;
            }
        }
    }
    FillCmbProcedencia();
    $('#dialogProcedencia').dialog('close');
}

function ProcedenciaDelete(sender) {
    document.getElementById('dialogProcedencia').parentNode.style.cssText += 'z-Index:1039 !important';
    $('#ProcedenciaName').html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#ProcedenciaDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_DocumentSource_Popup_Delete_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    $(this).dialog("close");
                    ProcedenciaDeleteConfirmed(Selected);
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () { document.getElementById('dialogProcedencia').parentNode.style.cssText += 'z-Index:1050 !important'; }

    });
}

function ProcedenciaDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var webMethod = "/Async/DocumentActions.asmx/ProcedenciaDelete";
    var description = '';
    for (var x = 0; x < procedencias.length; x++) {
        if (procedencias[x].Id === id) {
            description = procedencias[x].Description;
            break;
        }
    }
    var data = {
        'procedenciaId': id,
        'description': description,
        'companyId': Company.Id,
        'userId': user.Id
    };

    LoadingShow('');
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

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var x = 0; x < procedencias.length; x++) {
        if (procedencias[x].Id !== id) {
            temp.push(procedencias[x]);
        }
    }

    procedencias = new Array();
    for (var x = 0; x < temp.length; x++) {
        procedencias.push(temp[x]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById('ProcedenciaSelectable');
    for (var x = 0; x < target.childNodes.length; x++) {
        if (target.childNodes[x].id == id) {
            target.childNodes[x].style.display = 'none';
            break;
        }
    }

    FillCmbProcedencia();
}

function ProcedenciaUpdate(sender) {
    document.getElementById('dialogProcedencia').parentNode.style.cssText += 'z-Index:1039 !important';
    $('#TxtProcedenciaName').val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    document.getElementById('TxtProcedenciaNewNameErrorRequired').style.display = 'none';
    document.getElementById('TxtProcedenciaNewNameErrorDuplicated').style.display = 'none';
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#ProcedenciaUpdateDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        width: 500,
        title: '<h4 class="smaller">' + Dictionary.Item_DocumentSource_Popup_Update_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-success btn-xs",
                click: function () {
                    document.getElementById('TxtProcedenciaNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtProcedenciaNameErrorDuplicated').style.display = 'none';
                    if (document.getElementById('TxtProcedenciaName').value == '') {
                        document.getElementById('TxtProcedenciaNameErrorRequired').style.display = 'block';
                        return false;
                    }

                    document.getElementById('TxtProcedenciaNameErrorRequired').style.display = 'none';

                    var ok = true;
                    var duplicated = false;
                    var newName = document.getElementById('TxtProcedenciaName').value.toUpperCase();
                    for (var x = 0; x < procedencias.length; x++) {
                        if 
                        (
                            newName == procedencias[x].Description.toUpperCase()
                            &&
                            Selected != procedencias[x].Id
                        ) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtProcedenciaNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtProcedenciaNameErrorDuplicated').style.display = 'none';
                    }

                    if (ok === false) {
                        window.scrollTo(0, 0);
                        return false;
                    }


                    $(this).dialog("close");
                    ProcedenciaUpdateConfirmed(Selected, document.getElementById('TxtProcedenciaName').value);
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    document.getElementById('TxtProcedenciaNameErrorRequired').style.display = 'none';
                    $(this).dialog("close");
                }
            }
        ],
        close: function () { document.getElementById('dialogProcedencia').parentNode.style.cssText += 'z-Index:1050 !important'; }

    });
}

function ProcedenciaUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/DocumentActions.asmx/ProcedenciaUpdate";
    var data = {
        'procedenciaId': id,
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
    for (var x = 0; x < procedencias.length; x++) {
        if (procedencias[x].Id !== id) {
            temp.push(procedencias[x]);
        }
        else {
            var item = procedencias[x];
            temp.push({ "Id": item.Id, "Description": newDescription, "Active": item.Active, "Deletable": item.Delete });
        }
    }

    procedencias = new Array();
    for (var x = 0; x < temp.length; x++) {
        procedencias.push(temp[x]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById('ProcedenciaSelectable');
    for (var x = 0; x < target.childNodes.length; x++) {
        if (target.childNodes[x].id == id) {
            target.childNodes[x].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (procedenciaSelected === id) {
        document.getElementById('TxtProcedencia').value = newDescription;
    }

    FillCmbProcedencia();
}

function ProcedenciaInsert(sender) {
    document.getElementById('dialogProcedencia').parentNode.style.cssText += 'z-Index:1039 !important';
    document.getElementById('TxtProcedenciaNewName').value = '';
    document.getElementById('TxtProcedenciaNewNameErrorRequired').style.display = 'none';
    document.getElementById('TxtProcedenciaNewNameErrorDuplicated').style.display = 'none';
    Selected = 0;

    var dialog = $("#ProcedenciaInsertDialog").removeClass('hide').dialog({
        resizable: false,
        width: 500,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Document_Popup_AddSource_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                id: "BtnNewProcedenciaAccept",
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-success btn-xs",
                click: function () {
                    if (document.getElementById('TxtProcedenciaNewName').value == '') {
                        document.getElementById('TxtProcedenciaNewNameErrorRequired').style.display = 'block';
                        return false;
                    }

                    var ok = true;
                    var duplicated = false;
                    for (var x = 0; x < procedencias.length; x++) {
                        if (document.getElementById('TxtProcedenciaNewName').value.toUpperCase() == procedencias[x].Description.toUpperCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtProcedenciaNewNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtProcedenciaNewNameErrorDuplicated').style.display = 'none';
                    }

                    if (ok === false) {
                        window.scrollTo(0, 0);
                        return false;
                    }

                    document.getElementById('TxtProcedenciaNewNameErrorRequired').style.display = 'none';
                    $(this).dialog("close");
                    ProcedenciaInsertConfirmed(Selected, document.getElementById('TxtProcedenciaNewName').value);
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    document.getElementById('TxtProcedenciaNewNameErrorRequired').style.display = 'none';
                    $(this).dialog("close");
                }
            }
        ],
        close: function () {
            $(document).keypress(function (e) {
                if (e.which == 13) {
                    var overlay = $('.ui-widget-overlay');
                    if (overlay.length === 0) {
                        if (document.getElementById('BtnSave') !== null) {
                            $('#BtnSave').click();
                        }
                        e.preventDefault();
                    }
                }
            });
            document.getElementById('dialogProcedencia').parentNode.style.cssText += 'z-Index:1050 !important';
        }

    });

    $(document).keypress(function (e) {
        if (e.which == 13) {
            var overlay = $('.ui-widget-overlay');
            if (document.getElementById('BtnNewProcedenciaAccept') !== null) {
                $('#BtnNewProcedenciaAccept').click();
            }
            e.preventDefault();
        }
    });
}

function ProcedenciaInsertConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/DocumentActions.asmx/ProcedenciaInsert";
    var data = {
        'procedenciaId': 0,
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
                var procedencia = { "Id": newId, "Description": newDescription, "Active": true, "Deletable": true };
                procedencias.push(procedencia);

                // 3.- Modificar la fila de la tabla del popup
                RenderProcedenciasTable();

                FillCmbProcedencia();
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