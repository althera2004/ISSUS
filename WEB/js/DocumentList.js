function DocumentUpdate(sender) {
    document.location = "DocumentView.aspx?id=" + sender.id;
}

function DocumentDelete(sender) {
    console.log(sender.id);
    var description = DocumentDescriptionById(sender.id * 1);
    $("#DocumentName").html(description);
    $("#DocumentDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 500,
        "modal": true,
        "title": Dictionary.Item_Document_PopupDelete_Title,
        "title_html": true,
        "buttons":
            [
                {
                    "id": "BtnDeleteOk",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        var ok = true;
                        if (ok === false) {
                            window.scrollTo(0, 0);
                            return false;
                        }

                        $(this).dialog("close");
                        DocumentDeleteConfirmed(sender.id * 1);
                    }
                },
                {
                    "id": "BtnDeleteCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        ClearFieldTextMessages("TxtNewReason");
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function DocumentDeleteConfirmed(id) {
    var data = {
        "documentId": id,
        "companyId": Company.Id,
        "userId": user.Id,
        "reason": $("#TxtNewReason").val()
    };

    LoadingShow();
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/DocumentDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = document.location + "";
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alert(jqXHR.responseText);
        }
    });
}

function Restore(documentId) {
    var data = {
        "documentId": documentId,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/Restore",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = document.location + '';
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
}

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true)
                title.html($title);
            else title.text($title);
        }
    }));
});

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 380);
    $("#ListDataDivInactive").height(containerHeight - 380);
}

window.onload = function () {
    Resize();
    if (Filter.indexOf("A") !== -1) { document.getElementById("Chk1").checked = true; }
    if (Filter.indexOf("I") !== -1) { document.getElementById("Chk2").checked = true; }
    var parts = Filter.split('|');
    console.log(parts);
    FillComboCategorias();
    if (parts.length > 2) {
        console.log(parts[1], parts[2]);
        $("#CmbCategory").val(parts[1] * 1);
        $("#CmbOrigin").val(parts[2] * 1);
    }

    if (document.getElementById("Chk1").checked === true && document.getElementById("Chk2").checked === false) {
        $("#Chk1").attr("disabled", "disabled");
    }

    if (document.getElementById("Chk1").checked === false && document.getElementById("Chk2").checked === true) {
        $("#Chk2").attr("disabled", "disabled");
    }

    RenderDocumentTable();
    $("#CmbCategory").on("change", FilterChanged);
    $("#CmbOrigin").on("change", FilterChanged);
    $("#th0").click();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
};

window.onresize = function () { Resize(); };

function FillComboCategorias() {
    for (var x = 0; x < categories.length; x++) {
        var option = document.createElement("OPTION");
        option.value = categories[x].Id;
        option.appendChild(document.createTextNode(categories[x].Description));
		document.getElementById("CmbCategory").appendChild(option);
    }
}

function RenderDocumentTable() {
    $("#ListDataTable").html("");
    var res = "";
    var count = 0;
    var categorySelected = $("#CmbCategory").val() * 1;
    var originSelected = $("#CmbOrigin").val() * 1;
    for (var x = 0; x < documents.length; x++) {
        var show = false;
        if (documents[x].Baja === false && Filter.indexOf("A") !== -1) { show = true; }
        if (documents[x].Baja === true && Filter.indexOf("I") !== -1) { show = true; }

        if (originSelected === 0 && documents[x].Origin.Id > 0) { show = false; }
        if (originSelected === 1 && documents[x].Origin.Id === 0) { show = false; }

        if (categorySelected > 0) {
            if (documents[x].Category.Id !== categorySelected) { show = false; }
        }

        if (show === true) {
            res += RenderDocumentRow(documents[x]);
            count++;
        }
    }

    $("#TotalList").html(count);

    if (listOrder === null) {
        listOrder = "th0|ASC";
    }

    var th = listOrder.split('|')[0];
    var order = listOrder.split('|')[1];
    console.log(listOrder);
    $("#" + th).click();
    if (document.getElementById(th).className.indexOf(order) === -1) {
        $("#" + th).click();
    }
}

function RenderDocumentRow(data) {
    var style = "";
    var iconEditChar = "edit";
    if (user.Grants["Employee"].Write === false) {
        iconEditChar = "eye-open";
    }

    var tr = document.createElement("TR");
    var tdName = document.createElement("TD");
    var tdCode = document.createElement("TD");
    var tdRevision = document.createElement("TD");
    var tdCategory = document.createElement("TD");
    var tdOrigin = document.createElement("TD");
    var tdLocation = document.createElement("TD");
    var tdActions = document.createElement("TD");

    if (data.Baja === true) {
        tr.style.fontStyle = "italic";
    }

    if (typeof user.Grants.Employee === "undefined" || user.Grants.Employee.Read === false) {
        tdName.appendChild(document.createTextNode(data.Description));
    }
    else {
        var linkName = document.createElement("A");
        linkName.title = Dictionary.Common_Edit + " " + data.Description;
        linkName.href = "DocumentView.aspx?id=" + data.Id;
        linkName.appendChild(document.createTextNode(data.Description));
        tdName.appendChild(linkName);
    }

    tdCode.appendChild(document.createTextNode(data.Code));
    tdRevision.appendChild(document.createTextNode(data.LastVersion));
    tdCategory.appendChild(document.createTextNode(data.Category.Description));
    tdOrigin.appendChild(document.createTextNode(data.Origin.Id === 0 ? Dictionary.Common_Internal : Dictionary.Common_External));
    tdLocation.appendChild(document.createTextNode(data.Location));

    var buttonEdit = document.createElement("SPAN");
    buttonEdit.id = data.Id;
    buttonEdit.title = Dictionary.Common_Edit + " " + data.Description;
    buttonEdit.className = "btn btn-xs btn-info";
    buttonEdit.onclick = function () { DocumentUpdate(this); };
    var iconEdit = document.createElement("I");
    iconEdit.className = "icon-" + iconEditChar + " bigger-120";
    buttonEdit.appendChild(iconEdit);
    tdActions.appendChild(buttonEdit);

    if (user.Grants["Employee"].Delete === true) {
        var buttonDelete = document.createElement("SPAN");
        buttonDelete.id = data.Id;
        buttonDelete.Description = data.Description;
        buttonDelete.title = Dictionary.Common_Delete + " " + data.Description;
        buttonDelete.className = "btn btn-xs btn-danger";
        buttonDelete.onclick = function () { DocumentDelete(this); };
        var deleteIcon = document.createElement("I");
        deleteIcon.className = "icon-trash bigger-120";
        buttonDelete.appendChild(deleteIcon);
        tdActions.appendChild(document.createTextNode(" "));
        tdActions.appendChild(buttonDelete);
    }

    tdCode.style.width = "250px";
    tdCategory.style.width = "200px";
    tdOrigin.style.width = "110px";
    tdLocation.style.width = "200px";
    tdRevision.style.width = "80px";
    tdRevision.align = "right";
    tdActions.style.width = "90px";

    var target = document.getElementById("ListDataTable");
    tr.appendChild(tdName);
    tr.appendChild(tdCode);
    tr.appendChild(tdCategory);
    tr.appendChild(tdOrigin);
    tr.appendChild(tdLocation);
    tr.appendChild(tdRevision);
    tr.appendChild(tdActions);
    target.appendChild(tr);
}

function Export(fileType) {
    console.log("Export", fileType);
    var webMethod = "/Export/DocumentExportList.aspx/" + fileType;
    var data = {
        "companyId": Company.Id,
        "filter": Filter,
        "listOrder": listOrder
    };
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            LoadingHide();
            //successInfoUI(msg.d.MessageError, Go, 200);
            var link = document.createElement('a');
            link.id = 'download';
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = '_blank';
            document.body.appendChild(link);
            document.body.removeChild(link);
            $('#download').trigger('click');
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog('close');
        },
        "error": function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}

function FilterChanged() {
    $("#Chk1").removeAttr("disabled");
    $("#Chk2").removeAttr("disabled");
    if (document.getElementById("Chk1").checked === false) { $("#Chk2").attr("disabled", "disabled"); }
    if (document.getElementById("Chk2").checked === false) { $("#Chk1").attr("disabled", "disabled"); }
    SetFilter();
    RenderDocumentTable();
}

function SetFilter() {
    Filter = "";
    if (document.getElementById("Chk1").checked === true) { Filter += "A"; }
    if (document.getElementById("Chk2").checked === true) { Filter += "I"; }
	Filter += "|" + $("#CmbCategory").val();
    Filter += "|" + $("#CmbOrigin").val();

    var data = { "filter": Filter };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/SetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("SetFilter", "OK");
        },
        "error": function (msg) {
            console.log("SetFilter", msg.responseText);
        }
    });
}

function DocumentDescriptionById(id) {
    for (var x = 0; x < documents.length; x++) {
        if (documents[x].Id === id) {
            return documents[x].Description;
        }
    }
    return "";
}