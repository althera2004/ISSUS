function CompareEquipmentScaleDivision(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

// Bar popup for bar item EquipmentScaleDivision
function ShowEquipmentScaleDivisionBarPopup() {
    EquipmentScaleDivisionRenderPopup();
    var dialog = $("#dialogEquipmentScaleDivision").removeClass("hide").dialog({
        id: 'dialogEquipmentScaleDivision',
        resizable: false,
        modal: true,
        title: 'División de escala',
        title_html: true,
        width: 600,
        buttons: [
            {
                id: 'BtnEquipmentScaleDivisionSave',
                html: "<i class='icon-ok bigger-110'></i>&nbsp; Afegir",
                "class": "btn btn-success btn-xs",
                click: function () {
                    $(this).dialog("close");
                    EquipmentScaleDivisionInsert();
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp; Cancel·lar",
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
}

// Selection functions for bar item EquipmentScaleDivision
function EquipmentScaleDivisionChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#dialogEquipmentScaleDivision").dialog('close');
    for (var x = 0; x < EquipmentScaleDivision.length; x++) {
        if (EquipmentScaleDivision[x].Id === id) {
            EquipmentScaleDivisionSelected = id;
            document.getElementById('TxtEquipmentScaleDivision').value = EquipmentScaleDivision[x].Description;
            break;
        }
    }

    FillCmbEquipmentScaleDivision();
}

function FillCmbEquipmentScaleDivision() {
    if (document.getElementById('CmbEquipmentScaleDivision') === null) {
        return false;
    }

    VoidTable('CmbEquipmentScaleDivision');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    document.getElementById('CmbEquipmentScaleDivision').appendChild(optionDefault);

    for (var x = 0; x < EquipmentScaleDivision.length; x++) {
        var item = EquipmentScaleDivision[x];
        if (item.Active === true || item.Id === Equipment.ScaleDivision) {
            var option = document.createElement('option');
            option.value = item.Id;
            option.appendChild(document.createTextNode(item.Description));
            if (EquipmentScaleDivisionSelected === item.Id) {
                option.selected = true;
            }

            document.getElementById('CmbEquipmentScaleDivision').appendChild(option);
        }
    }
}

// Insert functions for bar item EquipmentScaleDivision
function EquipmentScaleDivisionInsert(sender) {    
    document.getElementById('TxtEquipmentScaleDivisionNewNameErrorRequired').style.display = 'none';
    document.getElementById('TxtEquipmentScaleDivisionNewNameErrorDuplicated').style.display = 'none';
    $('#TxtEquipmentScaleDivisionNewName').val('');
    var Selected = 0;
    var dialog = $("#EquipmentScaleDivisionInsertDialog").removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Equipment_Popup_AddScaleDivision_Title+'</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    var ok = true;
                    if (document.getElementById('TxtEquipmentScaleDivisionNewName').value === '') {
                        document.getElementById('TxtEquipmentScaleDivisionNewNameErrorRequired').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtEquipmentScaleDivisionNewNameErrorRequired').style.display = 'none';
                    }

                    var duplicated = false;
                    for (var x = 0; x < EquipmentScaleDivision.length; x++) {
                        if (document.getElementById('TxtEquipmentScaleDivisionNewName').value.toLowerCase() === EquipmentScaleDivision[x].Description.toLowerCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtEquipmentScaleDivisionNewNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtEquipmentScaleDivisionNewNameErrorDuplicated').style.display = 'none';
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById('TxtEquipmentScaleDivisionNewNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtEquipmentScaleDivisionNewNameErrorDuplicated').style.display = 'none';                        
                    EquipmentScaleDivisionInsertConfirmed(document.getElementById('TxtEquipmentScaleDivisionNewName').value);
                }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function (event, ui) { ShowEquipmentScaleDivisionBarPopup(); }
    });
}

function EquipmentScaleDivisionInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/EquipmentScaleDivisionActions.asmx/Insert";
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
            $('#EquipmentScaleDivisionInsertDialog').dialog("close");
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                EquipmentScaleDivision.push({ "Id": newId, "Description": newDescription, "Active": true, "Deletable": true });

                // 3.- Modificar la fila de la tabla del popup
                //EquipmentScaleDivisionRenderPopup();
                FillCmbEquipmentScaleDivision();
                ShowEquipmentScaleDivisionBarPopup();
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

    //FillCmbEquipmentScaleDivision();
}

// Update function for bar item EquipmentScaleDivision
function EquipmentScaleDivisionUpdate(sender) {
    $("#dialogEquipmentScaleDivision").dialog("close");
    document.getElementById("TxtEquipmentScaleDivisionNameErrorRequired").style.display = "none";
    document.getElementById("TxtEquipmentScaleDivisionNameErrorDuplicated").style.display = "none";
    $("#TxtEquipmentScaleDivisionName").val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#EquipmentScaleDivisionUpdateDialog").removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: Dictionary.Common_Edit,
        title_html: true,
        buttons:
        [
            {
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    var ok = true;
                    if (document.getElementById("TxtEquipmentScaleDivisionName").value === "") {
                        document.getElementById("TxtEquipmentScaleDivisionNameErrorRequired").style.display = "block";
                        ok = false;
                    }
                    else {
                        document.getElementById("TxtEquipmentScaleDivisionNameErrorRequired").style.display = "none";
                    }

                    var duplicated = false;
                    for (var x = 0; x < EquipmentScaleDivision.length; x++) {
                        if (document.getElementById("TxtEquipmentScaleDivisionName").value.toLowerCase() === EquipmentScaleDivision[x].Description.toLowerCase() && Selected !== EquipmentScaleDivision[x].Id && EquipmentScaleDivision[x].Active === true) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById("TxtEquipmentScaleDivisionNameErrorDuplicated").style.display = "block";
                        ok = false;
                    }
                    else {
                        document.getElementById("TxtEquipmentScaleDivisionNameErrorDuplicated").style.display = "none";
                    }


                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById("TxtEquipmentScaleDivisionNameErrorRequired").style.display = "none";
                    document.getElementById("TxtEquipmentScaleDivisionNameErrorDuplicated").style.display = "none";
                    EquipmentScaleDivisionUpdateConfirmed(Selected, document.getElementById("TxtEquipmentScaleDivisionName").value);
                }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function (event, ui) { ShowEquipmentScaleDivisionBarPopup(); }
    });
}

function EquipmentScaleDivisionUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/EquipmentScaleDivisionActions.asmx/Update";
    var description = "";
    for (var x = 0; x < EquipmentScaleDivision.length; x++) {
        if (EquipmentScaleDivision[x].Id === id) {
            description = EquipmentScaleDivision[x].Value;
            break;
        }
    }
    var data = {
        "EquipmentScaleDivisionId": id,
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
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
            $("#EquipmentScaleDivisionUpdateDialog").dialog('close');
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
    for (var y = 0; y < EquipmentScaleDivision.length; y++) {
        if (EquipmentScaleDivision[y].Id !== id) {
            temp.push(EquipmentScaleDivision[y]);
        }
        else {
            var item = EquipmentScaleDivision[x];
            temp.push({ "Id": item.Id, "Description": newDescription, "Active": item.Active, "Deletable": item.Delete });
        }
    }

    EquipmentScaleDivision = new Array();
    for (var z = 0; z < temp.length; z++) {
        EquipmentScaleDivision.push(temp[z]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById('SelectableEquipmentScaleDivision');
    for (var w = 0; w < target.childNodes.length; w++) {
        if (target.childNodes[w].id === id) {
            target.childNodes[w].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (EquipmentScaleDivisionSelected === id) {
        document.getElementById('TxtEquipmentScaleDivision').value = newDescription;
    }

    FillCmbEquipmentScaleDivision();
    ShowEquipmentScaleDivisionBarPopup();
}

// Delete functions for bar item EquipmentScaleDivision
function EquipmentScaleDivisionDelete(sender) {
    $("#EquipmentScaleDivisionName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#EquipmentScaleDivisionDeleteDialog").removeClass("hide").dialog({
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
                    EquipmentScaleDivisionDeleteConfirmed(Selected);
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

function EquipmentScaleDivisionDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var webMethod = '/Async/EquipmentScaleDivisionActions.asmx/Delete';
    var description = '';
    for (var x = 0; x < EquipmentScaleDivision.length; x++) {
        if (EquipmentScaleDivision[x].Id === id) {
            description = EquipmentScaleDivision[x].Value;
            break;
        }
    }

    var data = {
        'EquipmentScaleDivisionId': id,
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
    for (var w = 0; w < EquipmentScaleDivision.length; w++) {
        if (EquipmentScaleDivision[w].Id !== id) { temp.push(EquipmentScaleDivision[w]); }
    }

    EquipmentScaleDivision = new Array();
    for (var z = 0; z < temp.length; z++) {
        EquipmentScaleDivision.push(temp[z]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById("SelectableEquipmentScaleDivision");
    for (var y = 0; y < target.childNodes.length; y++) {
        if (target.childNodes[y].id === id) {
            target.childNodes[y].style.display = "none";
            break;
        }
    }

    FillCmbEquipmentScaleDivision();
}


// Common scripts
function EquipmentScaleDivisionRenderPopup() {
    VoidTable('SelectableEquipmentScaleDivision');
    EquipmentScaleDivision.sort(CompareEquipmentScaleDivision);
    var target = document.getElementById('SelectableEquipmentScaleDivision');
    for (var x = 0; x < EquipmentScaleDivision.length; x++) {
        EquipmentScaleDivisionPopupRow(EquipmentScaleDivision[x], target)
    }
}

function EquipmentScaleDivisionPopupRow(item, target) {
    if (item.Active === false) return;
    var tr = document.createElement('tr');
    tr.id = item.Id;
    var td1 = document.createElement('td');
    var td2 = document.createElement('td');
    if (EquipmentScaleDivisionSelected === item.Id) {
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

    if (EquipmentScaleDivisionSelected === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { EquipmentScaleDivisionChanged(this); };
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
        span2.onclick = function () { EquipmentScaleDivisionUpdate(this); };
    }

    var span3 = document.createElement('span');
    span3.className = 'btn btn-xs btn-danger';
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement('i');
    i3.className = 'icon-trash bigger-120';
    span3.appendChild(i3);

    if (EquipmentScaleDivisionSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else if (item.Id < 0) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else {
        span3.onclick = function () { EquipmentScaleDivisionDelete(this); };
    }
    div.appendChild(document.createTextNode(' '));
    div.appendChild(span3);
    td2.appendChild(div);


    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}