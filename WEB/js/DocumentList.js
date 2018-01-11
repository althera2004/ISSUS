function DocumentUpdate(sender) {
    document.location = "DocumentView.aspx?id=" + sender.id;
}

function DocumentDelete(sender) {
    console.log(sender.id);
    var description = DocumentDescriptionById(sender.id * 1);
    $("#DocumentName").html(description);
    var dialog = $("#DocumentDeleteDialog").removeClass("hide").dialog({
        resizable: false,
        width: 500,
        modal: true,
        title: Dictionary.Item_Document_PopupDelete_Title,
        title_html: true,
        buttons:
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    var ok = true;
                    if (!RequiredFieldText("TxtNewReason")) { ok = false; }
                    if (ok === false) {
                        window.scrollTo(0, 0);
                        return false;
                    }

                    $(this).dialog("close");
                    DocumentDeleteConfirmed(sender.id * 1);
                }
            },
            {
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
    var webMethod = "/Async/DocumentActions.asmx/DocumentDelete";
    var data = {
        "documentId": id,
        "companyId": Company.Id,
        "userId": user.Id,
        "reason": $("#TxtNewReason").val()
    };

    LoadingShow();
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = document.location + "";
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alert(jqXHR.responseText);
        }
    });
}

function Restore(documentId) {
    var webMethod = "/Async/DocumentActions.asmx/Restore";
    var data = {
        "documentId": documentId,
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
            if (response.d.Success === true) {
                document.location = document.location + '';
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
    var listTable = document.getElementById("ListDataDiv");
    var listTableInactive = document.getElementById("ListDataDivInactive");
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 370) + "px";
}

window.onload = function () {
    Resize();
    if (Filter.indexOf("A") !== -1) { document.getElementById("Chk1").checked = true; }
    if (Filter.indexOf("I") !== -1) { document.getElementById("Chk2").checked = true; }
    RenderDocumentTable();
    $("#th0").click();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
}

window.onresize = function () { Resize(); }

function RenderDocumentTable() {
    $("#ListDataTable").html("");
    var res = "";
    var count = 0;
    for (var x = 0; x < documents.length; x++) {
        var show = false;
        if (documents[x].Baja === false && Filter.indexOf("A") !== -1) { show = true; }
        if (documents[x].Baja === true && Filter.indexOf("I") !== -1) { show = true; }
        if (show === true) {
            res += RenderDocumentRow(documents[x]);
            count++;
        }
    }

    $("#TotalRecords").html(count);


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
    var tdActions = document.createElement("TD");

    if (data.Baja === true) {
        tr.style.fontStyle = "italic";
    }

    if (user.Grants["Employee"].Read === false) {
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

    var buttonEdit = document.createElement("SPAN");
    buttonEdit.id = data.Id;
    buttonEdit.title = Dictionary.Common_Edit + " " + data.Description;
    buttonEdit.className = "btn btn-xs btn-info";
    buttonEdit.onclick = function () { DocumentUpdate(this); }
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
        buttonDelete.onclick = function () { DocumentDelete(this); }
        var deleteIcon = document.createElement("I");
        deleteIcon.className = "icon-trash bigger-120";
        buttonDelete.appendChild(deleteIcon);
        tdActions.appendChild(document.createTextNode(" "));
        tdActions.appendChild(buttonDelete);
    }

    tdCode.style.width = "110px";
    tdRevision.style.width = "110px";
    tdActions.style.width = "90px";

    var target = document.getElementById("ListDataTable");
    tr.appendChild(tdName);
    tr.appendChild(tdCode);
    tr.appendChild(tdRevision);
    tr.appendChild(tdActions);
    target.appendChild(tr);


   /* var res = "<tr" + style + ">";
    res += "    <td>";
    res += "        <a href=\"DocumentView.aspx?id=" + document.Id + "\" title=\"" + Dictionary.Common_Edit + " " + document.Description + "\">" + document.Description + "</a>";
    res += "    </td>";
    res += "    <td class=\"hidden-480\" style=\"width:110px;\">" + document.Code + "</td>";
    res += "    <td class=\"hidden-480\" align=\"right\" style=\"width: 110px; \">" + document.LastVersion + "</td>";
    res += "    <td style=\"width:90px;\">";
    res += "        <span title=\"" + Dictionary.Common_Edit + " '" + document.Description + "'\" class=\"btn btn-xs btn-info\" onclick=\"DocumentUpdate(" + document.Id + ", '" + document.Description + "'); \">";
    res += "            <i class=\"icon-eye-" + iconEdit + " bigger-120\"></i>";
    res += "       </span >&nbsp;";
    if (user.Grants["Employee"].Delete === true) {
        res += "       <span title=\"" + Dictionary.Common_Delete + " '" + document.Description + "'\" class=\"btn btn-xs btn-danger\" onclick=\"DocumentDelete(" + document.Id + ", '" + document.Description + "'); \">";
        res += "            <i class=\"icon-trash bigger-120\"></i>";
        res += "        </span>";
    }
    res += "    </td>";
    res += "</tr>";

    return res;*/
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
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
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
        error: function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}

function FilterChanged() {
    SetFilter();
    RenderDocumentTable();
}

function SetFilter() {
    Filter = "";
    if (document.getElementById("Chk1").checked === true) { Filter += "A"; }
    if (document.getElementById("Chk2").checked === true) { Filter += "I"; }

    var webMethod = "/Async/DocumentActions.asmx/SetFilter";
    var data = { "filter": Filter };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            console.log("SetFilter", "OK");
        },
        error: function (msg) {
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