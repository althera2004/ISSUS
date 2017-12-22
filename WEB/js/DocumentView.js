$(document).ready(function () {
    $('#BtnNewVersion').click(Versioned);
    $('#BtnSave').click(SetReason);
    $('#BtnCancel').click(function (e) {
        document.location = referrer;
    });
});

function Versioned() {
    document.getElementById('TxtNewReason').value = '';
    var dialog = $("#ReasonDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Item_Document_Button_NewVersion,
        title_html: true,
        width: 400,
        buttons: [
            {
                id: 'BtnNewCategorySave',
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    if (document.getElementById('TxtNewReason').value === '') {
                        document.getElementById('TxtNewReasonErrorRequired').style.display = 'block';
                        return false;
                    }
                    else {
                        document.getElementById('TxtNewReasonErrorRequired').style.display = 'none';
                        selectedReason = document.getElementById('TxtNewReason').value;
                        VersionedConfirmed();
                    }

                    return null;
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () {
                    document.getElementById('TxtNewReasonErrorRequired').style.display = 'none';
                    $(this).dialog("close");
                    return null;
                }
            }
        ]
    });
}

function VersionedConfirmed() {
    var webMethod = "/Async/DocumentActions.asmx/Versioned";
    var data = {
        'documentId': documentId,
        'userId': userId,
        'companyId': Company.Id,
        'version': $('#TxtRevision').val(),
        'reason': selectedReason
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            document.getElementById('TxtRevision').value = msg.d.MessageError;
            document.getElementById('TxtRevisionDate').value = FormatDate(new Date(), '/');
            document.getElementById('TxtMotivo').value = document.getElementById('TxtNewReason').value;
            InsertVersionRow();
            $("#ReasonDialog").dialog('close');
            attachActual = null;
            SetAttachLayout();
        },
        error: function (msg) {
            LoadingHide();
            alertUI("error:" + msg.d.MessageError);
            $("#ReasonDialog").dialog('close');
        }
    });
}

function InsertVersionRow() {
    var target=document.getElementById('TableDocumentVersion');
    var tr = document.createElement('TR');
    var td1 = document.createElement('TD');
    td1.appendChild(document.createTextNode(document.getElementById('TxtRevision').value));
    var td2 = document.createElement('TD');
    td2.appendChild(document.createTextNode(FormatDate(new Date(), '/')));
    var td3 = document.createElement('TD');
    td3.appendChild(document.createTextNode(document.getElementById('TxtMotivo').value));
    var td4 = document.createElement('TD');
    td4.appendChild(document.createTextNode(ApplicationUser.Employee.Name));

    var td5 = document.createElement('TD');
    td5.id = 'Icons' + document.getElementById('TxtRevision').value;
    td5.appendChild(document.createTextNode(' '));

    var tdVoid = document.createElement('TD');
    tdVoid.id = 'DOC' + document.getElementById('TxtRevision').value;
    tdVoid.appendChild(document.createTextNode(' '));

    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);
    tr.appendChild(tdVoid);
    tr.appendChild(td4);
    tr.appendChild(td5);
    
    target.innerHTML = tr.innerHTML + target.innerHTML;
}

function Save() {
    if (documento.Id > 0) {
        Update();
    }
    else {
        Insert();
    }
}

function Insert() {
    var webMethod = "/Async/DocumentActions.asmx/Insert";
    var data = {
        "newDocument":
        {
            "Id": -1,
            "CompanyId": Company.Id,
            "Code": document.getElementById('TxtCodigo').value,
            "Description": document.getElementById('TxtDocumento').value,
            "StartDate": GetDate(document.getElementById('TxtStartDate').value,'-'),
            "EndDate": GetDate(document.getElementById('TxtEndDate').value,'-'),
            "Category": { "Id": categorySelected },
            "RevisionDate": GetDate(document.getElementById('TxtRevisionDate').value, '-'),
            "Origin": { "Id": procedenciaSelected },
            "Conservation": document.getElementById('TxtConservacion').value,
            "ConservationType": document.getElementById('CmbConservacion').value,
            "Source": document.getElementById('CmbOrigen').value === 2,
            "Location": document.getElementById('TxtUbicacion').value,
            "Active": true
        },
        "version": document.getElementById('TxtRevision').value * 1,
        "reason": selectedReason,
        "userId": userId
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            document.location = referrer;
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.d.MessageError);
        }
    });
}

function Update() {
    var webMethod = "/Async/DocumentActions.asmx/Update";
    documento.StartDate = GetDate(documento.StartDate, '-');
    documento.EndDate = GetDate(documento.EndDate, '-');
    documento.RevisionDate = GetDate(documento.RevisionDate, '-');
    var data = {
        "oldDocument": documento,
        "newDocument":
        {
            "Id": documento.Id,
            "CompanyId": companyId,
            "Code": document.getElementById('TxtCodigo').value,
            "Description": document.getElementById('TxtDocumento').value,
            "StartDate": GetDate(document.getElementById('TxtStartDate').value, '/'),
            //"EndDate": GetDate(document.getElementById('TxtEndDate').value, '/'),
            "Category": { "Id": categorySelected },
            "RevisionDate": GetDate(document.getElementById('TxtRevisionDate').value, '/'),
            "Origin": { "Id": procedenciaSelected },
            "Conservation": $('#TxtConservacion').val(),
            "ConservationType": $('#CmbConservacion').val(),
            "Source": $('#CmbOrigen').val() === 2,
            "Location": $('#TxtUbicacion').val(),
            "Active": true
        },
        "reason": selectedReason,
        "userId": userId
    };

    //alert(data.newDocument.EndDate);

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            //document.location = document.referrer;
            document.location = referrer;
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg);
        }
    });
}

// ISSUS-190
document.getElementById('TxtCodigo').focus();