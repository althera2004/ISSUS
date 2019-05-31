function SaveProcess() {
    var ok = true;
    $("#TxtNameErrorDuplicated").hide();
    if (!RequiredFieldText("TxtName")) { ok = false; }
    else
    {
        var duplicated = false;
        for (var x = 0; x < processList.length; x++) {
            if (processList[x].Description.toLowerCase() === document.getElementById('TxtName').value.toLowerCase() && processList[x].Id !== process.Id) {
                duplicated = true;
                break;
            }
        }

        if (duplicated === true) {
            $("#TxtNameErrorDuplicated").show();
            $("#TxtNameLabel").css("color", "#f00");
            ok = false;
        }
    }

    if (!RequiredFieldText("TxtJobPosition")) { ok = false; }
    if (!RequiredFieldText("TxtProcessType")) { ok = false; }

    if (ok === false) {
        window.scrollTo(0, 0);
        return false;
    }

    if (process.Id === 0) {
        ProcessInsert();
    }
    else {
        ProcessUpdate();
    }
}

function ProcessInsert() {
    var data = {
        "newProcess": {
            "Id": process.Id,
            "Description": $("#TxtName").val(),
            "CompanyId": Company.Id,
            "JobPosition": { "Id": jobPositionSelected },
            "ProcessType": processTypeSelected,
            "Start": $("#TxtInicio").val(),
            "Work": $("#TxtDesarrollo").val(),
            "End": $("#TxtFinalizacion").val()
        },
        "userId": user.Id
    };

    LoadingShow("");
    $.ajax({
        "type": "POST",
        "url": "/Async/ProcessActions.asmx/Insert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = referrer;
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function ProcessUpdate() {
    var data = {
        "oldProcess": process,
        "newProcess": {
            "Id": process.Id,
            "Description": $("#TxtName").val(),
            "CompanyId": process.CompanyId,
            "JobPosition": { "Id": jobPositionSelected },
            "ProcessType": processTypeSelected,
            "Start": $("#TxtInicio").val(),
            "Work": $("#TxtDesarrollo").val(),
            "End": $("#TxtFinalizacion").val(),
        },
        "userId": user.Id
    };

    LoadingShow("");
    $.ajax({
        "type": "POST",
        "url": "/Async/ProcessActions.asmx/Update",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = referrer;
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function jobPositionChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#dialogJobPosition").dialog("close");
    for (var x = 0; x < processTypeCompany.length; x++) {
        if (jobPositionCompany[x].Id === id) {
            jobPositionSelected = id;
            document.getElementById('TxtJobPosition').value = jobPositionCompany[x].Description;
            break;
        }
    }

    FillCmbJobPosition();
}

function ProcessTypeChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#dialogProcessType").dialog('close');
    for (var x = 0; x < processTypeCompany.length; x++) {
        if (processTypeCompany[x].Id === id) {
            processTypeSelected = id;
            document.getElementById('TxtProcessType').value = processTypeCompany[x].Description;
            break;
        }
    }

    FillCmbTipo();
}

function ProcessTypeDelete(sender) {
    document.getElementById("dialogProcessType").parentNode.style.cssText += "z-Index:1039 !important";
    $("#ProcessTypeName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#ProcessTypeDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Process_Popup_DeleteProcessType_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    $(this).dialog("close");
                    ProcessTypeDeleteConfirmed(Selected);
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
        "close": function () { document.getElementById("dialogProcessType").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function ProcessTypeUpdate(sender) {
    document.getElementById('dialogProcessType').parentNode.style.cssText += 'z-Index:1039 !important';
    document.getElementById('TxtProcessTypeNameErrorRequired').style.display = 'none';
    document.getElementById('TxtProcessTypeNameErrorDuplicated').style.display = 'none';
    $('#TxtProcessTypeName').val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#ProcessTypeUpdateDialog").removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Process_Popup_UpdateProcessType_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    var ok = true;
                    if (document.getElementById('TxtProcessTypeName').value === '') {
                        document.getElementById('TxtProcessTypeNameErrorRequired').style.display = 'block';
                        ok = false;
                    }
                    else {
                        $("#TxtProcessTypeNameErrorRequired").hide();
                    }

                    var duplicated = false;
                    for (var x = 0; x < processTypeCompany.length; x++) {
                        if (document.getElementById('TxtProcessTypeName').value.toLowerCase() === processTypeCompany[x].Description.toLowerCase() && Selected !== processTypeCompany[x].Id && processTypeCompany[x].Active === true) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtProcessTypeNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtProcessTypeNameErrorDuplicated').style.display = 'none';
                    }


                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById('TxtProcessTypeNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtProcessTypeNameErrorDuplicated').style.display = 'none';
                    $(this).dialog("close");
                    ProcessTypeUpdateConfirmed(Selected, document.getElementById('TxtProcessTypeName').value);
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
        close: function () { document.getElementById('dialogProcessType').parentNode.style.cssText += 'z-Index:1050 !important'; }
    });
}

function ProcessTypeInsert(sender) {
    document.getElementById("dialogProcessType").parentNode.style.cssText += "z-Index:1039 !important";
    $("#TxtProcessTypeNewNameErrorRequired").hide();
    $("#TxtProcessTypeNewNameErrorDuplicated").hide();
    $("#TxtProcessTypeNewName").val("");
    var Selected = 0;
    var dialog = $("#ProcessTypeInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Process_Popup_AddProcessType_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    if ($("#TxtProcessTypeNewName").val() === "") {
                        $("#TxtProcessTypeNewNameErrorRequired").show();
                        ok = false;
                    }
                    else {
                        $("#TxtProcessTypeNewNameErrorRequired").hide();
                    }

                    var duplicated = false;
                    for (var x = 0; x < processTypeCompany.length; x++) {
                        if (document.getElementById('TxtProcessTypeNewName').value.toLowerCase() === processTypeCompany[x].Description.toLowerCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById('TxtProcessTypeNewNameErrorDuplicated').style.display = 'block';
                        ok = false;
                    }
                    else {
                        document.getElementById('TxtProcessTypeNewNameErrorDuplicated').style.display = 'none';
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById('TxtProcessTypeNewNameErrorRequired').style.display = 'none';
                    document.getElementById('TxtProcessTypeNewNameErrorDuplicated').style.display = 'none';
                    $(this).dialog("close");
                    ProcessTypeInsertConfirmed(document.getElementById('TxtProcessTypeNewName').value);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ],
        "close": function () { document.getElementById('dialogProcessType').parentNode.style.cssText += 'z-Index:1050 !important'; }
    });
}

function ProcessTypeDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var webMethod = "/Async/ProcessActions.asmx/DesactiveProcessType";
    var description = "";
    for (var x = 0; x < processTypeCompany.length; x++) {
        if (processTypeCompany[x].Id === id) {
            description = processTypeCompany[x].Description;
            break;
        }
    }
    var data = {
        'processTypeId': id,
        'description': description,
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

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var x = 0; x < processTypeCompany.length; x++) {
        if (processTypeCompany[x].Id !== id) {
            temp.push(processTypeCompany[x]);
        }
    }

    processTypeCompany = new Array();
    for (var x = 0; x < temp.length; x++) {
        processTypeCompany.push(temp[x]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById('SelectableProcessType');
    for (var x = 0; x < target.childNodes.length; x++) {
        if (target.childNodes[x].id === id) {
            target.childNodes[x].style.display = 'none';
            break;
        }
    }

    FillCmbTipo();
}

function ProcessTypeUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/ProcessActions.asmx/UpdateProcessType";
    var description = '';
    for (var x = 0; x < processTypeCompany.length; x++) {
        if (processTypeCompany[x].Id === id) {
            description = processTypeCompany[x].Description;
            break;
        }
    }
    var data = {
        'processTypeId': id,
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
    for (var x = 0; x < processTypeCompany.length; x++) {
        if (processTypeCompany[x].Id !== id) {
            temp.push(processTypeCompany[x]);
        }
        else {
            var item = processTypeCompany[x];
            temp.push(
            {
                "Id": item.Id,
                "Description": newDescription,
                "Active": item.Active,
                "Deletable": item.Delete
            });
        }
    }

    processTypeCompany = new Array();
    for (var x = 0; x < temp.length; x++) {
        processTypeCompany.push(temp[x]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById('SelectableProcessType');
    for (var x = 0; x < target.childNodes.length; x++) {
        if (target.childNodes[x].id === id) {
            target.childNodes[x].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (processTypeSelected === id) {
        document.getElementById('TxtProcessType').value = newDescription;
    }

    FillCmbTipo();
}

function ProcessTypeInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/ProcessActions.asmx/InsertProcessType";
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
                var processType =
                {
                    "Id": newId,
                    "Description": newDescription,
                    "Active": true,
                    "Deletable": true
                };
                processTypeCompany.push(processType);

                // 3.- Modificar la fila de la tabla del popup
                ProccessTypeRenderPopup();
                FillCmbTipo();
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

    FillCmbTipo();
}

jQuery(function ($) {

    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;'
            if (("title_html" in this.options) && this.options.title_html === true)
                title.html($title);
            else title.text($title);
        }
    }));

    if (ApplicationUser.ShowHelp === true) {
        SetToolTip('TxtName', Dictionary.Item_Process_Help_Name);
        SetToolTip('DivCmbJobPosition', Dictionary.Item_Process_Help_Responsible);
        SetToolTip('DivCmbTipo', Dictionary.Item_Process_Help_ProcessType);
        SetToolTip('BtnSelectProcessType', Dictionary.Item_Process_Help_BARType);
        SetToolTip('TxtInicio', Dictionary.Item_Process_Help_Start);
        SetToolTip('TxtDesarrollo', Dictionary.Item_Process_Help_Deploy);
        SetToolTip('TxtFinalizacion', Dictionary.Item_Process_Help_End);
        $('[data-rel=tooltip]').tooltip();
    }

    $('#BtnSave').click(SaveProcess);
    $('#BtnCancel').click(function (e) { document.location = referrer; });
    $("#BtnSelectProcessType").on('click', function (e) {
        e.preventDefault();

        ProccessTypeRenderPopup();

        var dialog = $("#dialogProcessType").removeClass("hide").dialog({
            resizable: false,
            modal: true,
            title: Dictionary.Item_Process_SelectProcessType,
            title_html: true,
            width: 800,
            buttons:
            [
                {
                    id: 'BtnNewAddresSave',
                    html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                    "class": "btn btn-success btn-xs",
                    click: function () {
                        ProcessTypeInsert();
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
    });

    $("#BtnSelectJobPosition").on('click', function (e) {
        e.preventDefault();

        VoidTable('SelectableJobPosition');
        var target = document.getElementById('SelectableJobPosition');
        for (var x = 0; x < jobPositionCompany.length; x++) {
            var jobPosition = jobPositionCompany[x];

            var tr = document.createElement('tr');
            var td1 = document.createElement('td');
            tr.id = jobPosition.Id;
            td1.style.cursor = 'pointer';
            td1.appendChild(document.createTextNode(jobPosition.Description));
            if (jobPositionSelected === jobPosition.Id) {
                td1.style.fontWeight = 'bold';
            }

            var td2 = document.createElement('td');
            var div = document.createElement('div');
            var span1 = document.createElement('span');
            span1.className = 'btn btn-xs btn-success';
            span1.title = Dictionary.Common_SelectAll;
            var i1 = document.createElement('i');
            i1.className = 'icon-star bigger-120';
            span1.appendChild(i1);

            if (jobPositionSelected === jobPosition.Id) {
                span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
            }
            else {
                span1.onclick = function () { jobPositionChanged(this); };
            }

            div.appendChild(span1);
            td2.appendChild(div);

            tr.appendChild(td1);
            tr.appendChild(td2);
            target.appendChild(tr);
        }

        var dialog = $("#dialogJobPosition").removeClass("hide").dialog({
            resizable: false,
            modal: true,
            title: Dictionary.Item_Process_SelectJobPosition,
            title_html: true,
            width: 800,
            buttons: [
                {
                    html: "<i class='icon-remove bigger-110'></i>&nbsp; Cancel·lar",
                    "class": "btn btn-xs",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]

        });
    });

    $('[data-rel=tooltip]').tooltip({ container: 'body' });
    $('[data-rel=popover]').popover({ container: 'body' });

});

for (var x = 0; x < processTypeCompany.length; x++) {
    if (processTypeCompany[x].Id === processTypeSelected) {
        document.getElementById('TxtProcessType').value = processTypeCompany[x].Description;
        break;
    }

    FillCmbTipo();
}

function FillCmbJobPosition() {
    VoidTable('CmbJobPosition');
    var optionDefault = document.createElement('option');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById('CmbJobPosition').appendChild(optionDefault);

    for (var x = 0; x < jobPositionCompany.length; x++) {
        var option = document.createElement('option');
        option.value = jobPositionCompany[x].Id;
        option.appendChild(document.createTextNode(jobPositionCompany[x].Description));
        if (jobPositionSelected === jobPositionCompany[x].Id) {
            option.selected = true;
        }

        document.getElementById('CmbJobPosition').appendChild(option);
    }
}

function FillCmbTipo() {
    VoidTable("CmbTipo");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbTipo").appendChild(optionDefault);

    for (var x = 0; x < processTypeCompany.length; x++) {
        var option = document.createElement("option");
        option.value = processTypeCompany[x].Id;
        option.appendChild(document.createTextNode(processTypeCompany[x].Description));
        if (processTypeSelected === processTypeCompany[x].Id) {
            option.selected = true;
        }

        document.getElementById("CmbTipo").appendChild(option);
    }
}

function CmbJobPositionChanged() {
    jobPositionSelected = $("#CmbJobPosition").val() * 1;
    var text = "";
    for (var x = 0; x < jobPositionCompany.length; x++) {
        if (jobPositionSelected === jobPositionCompany[x].Id) {
            text = jobPositionCompany[x].Description;
            break;
        }
    }

    $("#TxtJobPosition").val(text);
}

function CmbTipoChanged() {
    processTypeSelected = document.getElementById('CmbTipo').value * 1;
    var text = '';
    for (var x = 0; x < processTypeCompany.length; x++) {
        if (processTypeSelected === processTypeCompany[x].Id) {
            text = processTypeCompany[x].Description;
            break;
        }
    }

    $("#TxtProcessType").val(text);
}

if (ApplicationUser.Grants.Proccess.Write === false) {
    document.getElementById("CmbJobPosition").disabled = true;
    document.getElementById("CmbTipo").disabled = true;
    document.getElementById("CmbJobPosition").style.backgroundColor = "#f5f5f5";
    document.getElementById("CmbTipo").style.backgroundColor = "#f5f5f5";
    document.getElementById("TxtInicio").disabled = true;
    document.getElementById("TxtDesarrollo").disabled = true;
    document.getElementById("TxtFinalizacion").disabled = true;
    $("#BtnSelectProcessType").hide();
    $("#BtnSave").hide();
}

window.onload = function () {
    $("#BtnRestaurar").on("click", Restaurar);
    $("#BtnAnular").on("click", AnularPopup);
    ButtonLayout();
    document.getElementById("CmbJobPosition").focus();
    FillCmbJobPosition();
    FillCmbTipo();
    Resize();
    var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);
};

function ButtonLayout() {
    $("#BtnRestaurar").hide();
    $("#BtnAnular").hide();
    return;
    if (process.DisabledOn === null) {
        $("#BtnAnular").show();
        console.log(1);
    } else {
        console.log(2);
        $("#BtnRestaurar").show();
    }
}

function Restaurar() {

}

window.onresize = function () { Resize(); };

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 360);
}

function AnularPopup() {
    $("#dialogAnular").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Process_PopupAnular_Title,
        "width": 400,
        "buttons":
            [
                {
                    "id": "BtnAnularOk",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_Process_Btn_Anular,
                    "class": "btn btn-success btn-xs",
                    "click": function () { AnularConfirmed(); }
                },
                {
                    "id": "BtnAnularCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
    });
}

function AnularConfirmed() {
    $("#TxtDateLabel").css("color", "#000");
    $("#CmbDisabledLabel").css("color", "#000");
    $("#TxtDateErrorRequired").hide();
    $("#TxtDateErrorMalformed").hide();
    $("#CmbDisabledErrorRequired").hide();

    var ok = true;

    if ($("#TxtDate").val() === "") {
        ok = false;
        $("#TxtDateLabel").css("color", "#f00");
        $("#TxtDateDateRequired").show();
    }
    else {
        if (validateDate($("#TxtDate").val()) === false) {
            ok = false;
            $("#TxtDateLabel").css("color", "#f00");
            $("#TxtDateMalformed").show();
        }
    }

    if ($("#CmbDisabledBy").val() * 1 < 1) {
        ok = false;
        $("#CmbDisabledLabel").css("color", "#f00");
        $("#CmbDisabledErrorRequired").show();
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "processId": process.Id,
        "companyId": Company.Id,
        "responsible": $("#CmbDisabledBy").val() * 1,
        "date": GetDate($("#TxtDate").val(), "/"),
        "applicationUserId": user.Id
    };
    anulationData = data;
    $("#dialogAnular").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ProcessActions.asmx/Anulate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            ButtonLayout();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}