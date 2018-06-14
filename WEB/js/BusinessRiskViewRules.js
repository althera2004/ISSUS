function RulesRenderPopup() {
    VoidTable("SelectableRules");
    RulesCompany.sort(CompareRules);
    var target = document.getElementById("SelectableRules");
    for (var x = 0; x < RulesCompany.length; x++) {
        RulesPopupRow(RulesCompany[x], target);
    }

    var height = $("#SelectableRules").height();
    if (height > 0) {
        $("#DivTableRulesFiller").height(311 - height);
        $("#DivTableRulesFiller").show();
    }
    else {
        $("#DivTableRulesFiller").hide();
    }
}

function RulesPopupRow(item, target) {
    if (item.Active === false) {
        return;
    }

    var tr = document.createElement("tr");
    tr.id = item.Id;
    tr.style.height = "30px";
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    if (RulesSelected === item.Id) {
        td1.style.fontWeight = "bold";
    }
    td1.appendChild(document.createTextNode(item.Description));

    var tdLimit = document.createElement("td");
    tdLimit.style.width = "60px";
    tdLimit.align = "right";
    tdLimit.appendChild(document.createTextNode(item.Limit));

    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if (RulesSelected === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { RulesChanged(this); };
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
    span2.onclick = function () { RulesUpdate(this); };

    var span3 = document.createElement("span");
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);

    if (RulesSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected, "dialogRules"); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable, "dialogRules"); };
    }
    else {
        span3.onclick = function () { RulesDelete(this); };
    }

    div.appendChild(document.createTextNode(" "));
    div.appendChild(span3);
    td2.appendChild(div);
    td2.style.width = "133px";


    tr.appendChild(td1);
    tr.appendChild(tdLimit);
    tr.appendChild(td2);
    target.appendChild(tr);
}

function RulesDelete(sender) {
    document.getElementById('dialogRules').parentNode.style.cssText += 'z-Index:1039 !important';
    $('#RuleName').html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#RuleDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Rules_Popup_DeleteProcessType_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    $(this).dialog("close");
                    RulesDeleteConfirmed(Selected);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () { document.getElementById("dialogRules").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function RulesInsert(sender) {
    document.getElementById('dialogRules').parentNode.style.cssText += 'z-Index:1039 !important';
    document.getElementById('TxtNewRulesNameErrorRequired').style.display = 'none';
    document.getElementById('TxtNewRulesNameErrorDuplicated').style.display = 'none';
    document.getElementById('CmbNewLimitErrorRequired').style.display = 'none';
    document.getElementById('CmbNewLimitOutOfRange').style.display = 'none';
    $("#TxtNewRulesName").val("");
    $("#TxtNewRulesNotes").val("");
    $("#CmbNewLimit").val("");
    var Selected = 0;
    var dialog = $("#RulesInsertDialog").removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Rules_Popup_AddRule_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    var ok = true;
                    if (document.getElementById('TxtNewRulesName').value === '') {
                        document.getElementById('TxtNewRulesNameErrorRequired').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtNewRulesNameErrorRequired').style.display = 'none';
                    }

                    var duplicated = false;
                    for (var x = 0; x < RulesCompany.length; x++) {
                        if (document.getElementById('TxtNewRulesName').value.toLowerCase() === RulesCompany[x].Description.toLowerCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtNewRulesNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtNewRulesNameErrorDuplicated').style.display = 'none';
                    }

                    if (document.getElementById('CmbNewLimit').value === '') {
                        document.getElementById('TxtNewLimitLabel').style.color = '#f00';
                        document.getElementById('CmbNewLimitErrorRequired').style.display = '';
                        ok = false;

                    } else {
                        if (document.getElementById('CmbNewLimit').value * 1 < 1 || document.getElementById('CmbNewLimit').value * 1 > 25) {
                            ok = false;
                            document.getElementById('TxtNewLimitLabel').style.color = '#f00';
                            document.getElementById('CmbNewLimitOutOfRange').style.display = '';
                        }
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById('TxtNewRulesNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtNewRulesNameErrorDuplicated').style.display = 'none';
                    $(this).dialog("close");
                    RulesInsertConfirmed(document.getElementById('TxtNewRulesName').value, document.getElementById('TxtNewRulesNotes').value, document.getElementById('CmbNewLimit').value);
                    return null;
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
        close: function () { document.getElementById('dialogRules').parentNode.style.cssText += 'z-Index:1050 !important'; }
    });
}

function RulesUpdate(sender) {
    document.getElementById('dialogRules').parentNode.style.cssText += 'z-Index:1039 !important';
    document.getElementById('TxtRulesNameErrorRequired').style.display = 'none';
    document.getElementById('TxtRulesNameErrorDuplicated').style.display = 'none';
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    for (var x = 0; x < RulesCompany.length; x++) {
        if (RulesCompany[x].Id === Selected) {
            var rule = RulesCompany[x];
            break;
        }
    }
    $('#TxtRulesName').val(rule.Description);
    $('#TxtRulesNotes').val(rule.Notes);
    $('#CmbUpdateLimit').val(rule.Limit);
    var dialog = $("#RulesUpdateDialog").removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Rules_Popup_UpdateRules_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    var ok = true;
                    if (document.getElementById('TxtRulesName').value === '') {
                        document.getElementById('TxtRulesNameErrorRequired').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtRulesNameErrorRequired').style.display = 'none';
                    }

                    var duplicated = false;
                    for (var x = 0; x < RulesCompany.length; x++) {
                        if (document.getElementById('TxtRulesName').value.toLowerCase() === RulesCompany[x].Description.toLowerCase() && Selected !== RulesCompany[x].Id && RulesCompany[x].Active === true) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtRulesNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtRulesNameErrorDuplicated').style.display = 'none';
                    }

                    if (document.getElementById('TxtLimitLabel').value === '') {
                        document.getElementById('TxtNewLimitLabel').style.color = '#f00';
                        document.getElementById('CmbUpdateLimitErrorRequired').style.display = '';
                        ok = false;

                    } else {
                        if (document.getElementById('TxtLimitLabel').value * 1 < 1 || document.getElementById('CmbUpdateLimit').value * 1 > 25) {
                            ok = false;
                            document.getElementById('TxtNewLimitLabel').style.color = '#f00';
                            document.getElementById('CmbUpdateLimitOutOfRange').style.display = '';
                        }
                    }


                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById('TxtRulesNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtRulesNameErrorDuplicated').style.display = 'none';
                    $(this).dialog("close");
                    RulesUpdateConfirmed(Selected, document.getElementById('TxtRulesName').value, document.getElementById('TxtRulesNotes').value, document.getElementById('CmbUpdateLimit').value);
                    return null;
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
        close: function () { document.getElementById('dialogRules').parentNode.style.cssText += 'z-Index:1050 !important'; }
    });
}

function RulesDeleteConfirmed(id) {
    var webMethod = "/Async/RulesActions.asmx/RulesDelete";
    var description = '';
    for (var x = 0; x < RulesCompany.length; x++) {
        if (RulesCompany[x].Id === id) {
            description = RulesCompany[x].Description;
            break;
        }
    }
    var data = {
        'rulesId': id,
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
            RulesRenderPopup();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var x2 = 0; x2 < RulesCompany.length; x2++) {
        if (RulesCompany[x2].Id !== id) {
            temp.push(RulesCompany[x2]);
        }
    }

    RulesCompany = new Array();
    for (var x3 = 0; x3 < temp.length; x3++) {
        RulesCompany.push(temp[x3]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById('SelectableRules');
    for (var x4 = 0; x4 < target.childNodes.length; x4++) {
        if (target.childNodes[x4].id === id) {
            target.childNodes[x4].style.display = 'none';
            break;
        }
    }

    FillCmbRules();
}

function RulesInsertConfirmed(newDescription, newNotes, newLimit) {
    // 1.- Modificar en la BBDD
    var id = 0;
    var data = {
        "rules": {
            "CompanyId": Company.Id,
            "Description": newDescription,
            "Notes": newNotes,
            "Limit": newLimit
        },
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: "/Async/RulesActions.asmx/RulesInsert",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            if (response.d.Success === true) {
                RulesCompany.push(
                {
                    "Id": response.d.MessageError * 1,
                    "Description": newDescription,
                    "Notes": newNotes,
                    "Limit": newLimit * 1,
                    "Editable": "true",
                    "Deletable": "true"
                });
                RulesRenderPopup();
                FillCmbRules();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function RulesUpdateConfirmed(id, newDescription, newNotes, newLimit) {
    // 1.- Modificar en la BBDD
    var description = "";
    for (var x = 0; x < RulesCompany.length; x++) {
        if (RulesCompany[x].Id === id) {
            description = RulesCompany[x].Description;
            notes = RulesCompany[x].Notes;
            limit = RulesCompany[x].Limit;
            break;
        }
    }
    var data = {
        "newRules": {
            "Id": id,
            "CompanyId": Company.Id,
            "Description": newDescription,
            "Notes": newNotes,
            "Limit": newLimit
        },
        "oldRules": {
            "Id": id,
            "CompanyId": Company.Id,
            "Description": description,
            "Notes": notes,
            "Limit": limit
        },
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/RulesActions.asmx/RulesUpdate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            RulesRenderPopup();
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var y = 0; y < RulesCompany.length; y++) {
        if (RulesCompany[y].Id !== id) {
            temp.push(RulesCompany[y]);
        }
        else {
            //Item modificado en el popup
            var item = RulesCompany[y];
            temp.push(
                {
                    "Id": item.Id,
                    "Description": newDescription,
                    "Notes": newNotes,
                    "Limit": newLimit,
                    "Editable": item.Editable,
                    "Deletable": item.Delete
                });
        }
    }

    RulesCompany = new Array();
    for (var x2 = 0; x2 < temp.length; x2++) {
        RulesCompany.push(temp[x2]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById("SelectableRules");
    for (var x3 = 0; x3 < target.childNodes.length; x3++) {
        if (target.childNodes[x3].id === id) {
            target.childNodes[x3].childNodes[0].innerHTML = newDescription;
            target.childNodes[x3].childNodes[1].innerHTML = newLimit;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (RulesSelected === id) {
        $("#TxtRules").val(newDescription);
    }

    FillCmbRules();
}

function FillCmbRules() {
    VoidTable("CmbRules");
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById('CmbRules').appendChild(optionDefault);

    for (var x = 0; x < RulesCompany.length; x++) {
        var option = document.createElement('option');
        option.value = RulesCompany[x].Id;
        option.appendChild(document.createTextNode(RulesCompany[x].Description));
        if (RulesSelected === RulesCompany[x].Id) {
            option.selected = true;
        }

        document.getElementById("CmbRules").appendChild(option);
    }

    CmbRulesChanged();
}

function RulesChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#dialogRules").dialog("close");
    for (var x = 0; x < RulesCompany.length; x++) {
        if (RulesCompany[x].Id === id) {
            RulesSelected = id;
            $("#TxtRules").val(RulesCompany[x].Description);
            $("#IPR").val(RulesCompany[x].Limit);
            break;
        }
    }

    FillCmbRules();
}

function CmbRulesChanged() {
    RulesSelected = $("#CmbRules").val() * 1;
    var text = "";
    var ipr = 0;
    for (var x = 0; x < RulesCompany.length; x++) {
        if (RulesSelected === RulesCompany[x].Id) {
            text = RulesCompany[x].Description;
            ipr = RulesCompany[x].Limit;
            rule = RulesCompany[x];
            break;
        }
    }

    $("#TxtRules").val(text);
    $("#IPR").html(ipr);
    UpdateResult();
}