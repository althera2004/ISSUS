var EquipmentSelected;
function EquipmentDeleteAction() {
    var data = {
        "equipmentId": EquipmentSelected,
        "reason": "",
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#EquipmentDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EquipmentActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = document.location + "";
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function EquipmentDelete(id, name) {
    $("#EquipmentName").html(name);
    EquipmentSelected = id;
    var dialog = $("#EquipmentDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Equipment_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    EquipmentDeleteAction();
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;"
            if (("title_html" in this.options) && this.options.title_html === true)
                title.html($title);
            else title.text($title);
        }
    }));
});

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 390);
}

window.onload = function () {
    Resize();
    document.getElementById("RBOperation1").checked = Filter.indexOf("C") !== -1;
    document.getElementById("RBOperation2").checked = Filter.indexOf("V") !== -1;
    document.getElementById("RBOperation3").checked = Filter.indexOf("M") !== -1;
    document.getElementById("RBStatus0").checked = Filter.indexOf("0") !== -1;
    document.getElementById("RBStatus1").checked = Filter.indexOf("1") !== -1;
    document.getElementById("RBStatus2").checked = Filter.indexOf("2") !== -1;
    RenderTable();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;")    
}
window.onresize = function () { Resize(); }

function Export(fileType) {
    console.log("Export", fileType);
    var data = {
        "companyId": Company.Id,
        "listOrder": listOrder
    };
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/EquipmentExportList.aspx/" + fileType,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            LoadingHide();
            //successInfoUI(msg.d.MessageError, Go, 200);
            var link = document.createElement("a");
            link.id = "download";
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = "_blank";
            document.body.appendChild(link);
            $("#download").trigger("click");
            document.body.removeChild(link);
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog("close");
        },
        "error": function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}

function RenderTable() {
    SetFilter();
    console.log("RenderTable");
    $("#ListDataTable").html("");
    var temp = [];
    for (var x = 0; x < Equipments.length; x++) {
        var ok = true;
        var equipment = Equipments[x];
        if (document.getElementById("RBStatus1").checked === true && equipment.Activo === false) { ok = false; }
        if (document.getElementById("RBStatus2").checked === true && equipment.Activo === true) { ok = false; }
        if (ok === true) {
            if (document.getElementById("RBOperation1").checked === true && equipment.Calibracion === true) { temp.push(equipment); }
            else
                if (document.getElementById("RBOperation2").checked === true && equipment.Verificacion === true) { temp.push(equipment); }
                else
                    if (document.getElementById("RBOperation3").checked === true && equipment.Mantenimiento === true) { temp.push(equipment); }
        }
    }

    $("#TotalRecords").html(temp.length);
    var total = 0;
    if (temp.length > 0) {
        for (var y = 0; y < temp.length; y++) {
            RenderRow(temp[y]);
            total += temp[y].Coste;
        }
    }

    $("#TotalCost").html(ToMoneyFormat(total, 2));

    if (listOrder === null) {
        $("#th0").click();
    }
    else {
        var column = listOrder.split('|')[0];
        var order = listOrder.split('|')[1];

        $("#" + column).click();
        if (document.getElementById(column).className.indexOf(order) === -1) {
            $("#" + column).click();
        }
    }
}

function RenderRow(equipment) {
    var tr = document.createElement("tr");
    var tdCodigo = document.createElement("td");
    var tdDescripcion = document.createElement("td");
    var tdUbicacion = document.createElement("td");
    var tdResponsable = document.createElement("td");
    var tdCoste = document.createElement("td");
    var tdAdjuntos = document.createElement("td");
    var tdAcciones = document.createElement("td");

    tdCodigo.style.width = "110px";
    tdUbicacion.style.width = "350px";
    tdResponsable.style.width = "250px";
    tdCoste.style.width = "120px";
    tdAdjuntos.style.width = "35px";
    tdAcciones.style.width = "90px";

    tdCoste.style.textAlign = "right";
    tdAdjuntos.style.textAlign = "center";

    tdCodigo.className = "hidden-480";
    tdUbicacion.className = "hidden-480";
    tdCoste.className = "hidden-480";
    tdResponsable.className = "hidden-480";
    tdAdjuntos.className = "hidden-480";

    var linkCodigo = document.createElement("a");
    linkCodigo.href = "EquipmentView.aspx?id=" + equipment.Id;
    linkCodigo.appendChild(document.createTextNode(equipment.Codigo));
    tdCodigo.appendChild(linkCodigo);

    var linkDescripcion = document.createElement("a");
    linkDescripcion.href = "EquipmentView.aspx?id=" + equipment.Id;
    linkDescripcion.appendChild(document.createTextNode(equipment.Codigo));
    linkDescripcion.appendChild(document.createTextNode(" - "));
    linkDescripcion.appendChild(document.createTextNode(equipment.Descripcion));
    tdDescripcion.appendChild(linkDescripcion);

    tdUbicacion.appendChild(document.createTextNode(equipment.Ubicacion));
    if (typeof user.Grants.Employee !== "undefined" && user.Grants.Employee.Write === true) {
        var LinkResponsable = document.createElement("a");
        LinkResponsable.href = "EmployeesView.aspx?id=" + equipment.Responsable.Id;
        LinkResponsable.title = equipment.Responsable.FullName;
        LinkResponsable.appendChild(document.createTextNode(equipment.Responsable.FullName));
        tdResponsable.appendChild(LinkResponsable);
    }
    else {
        tdResponsable.appendChild(document.createTextNode(equipment.Responsable.FullName));
    }

    tdCoste.appendChild(document.createTextNode(ToMoneyFormat(equipment.Coste, 2)));

    if (equipment.Adjuntos === true) {
        var icon = document.createElement("i");
        icon.className = "icon-paperclip";
        icon.title = Dictionary.Item_Equipment_Message_Attachs;
        icon.style.cursor = "pointer";
        icon.onclick = function () {
            document.location = "EquipmentView.aspx?id=" + equipment.Id + "&Tab=TabuploadFiles";
        };
        tdAdjuntos.appendChild(icon);
    }
    else {
        tdAdjuntos.appendChild(document.createTextNode(" "));
    }

    if (user.Grants["Equipment"].Write === true) {
        /*<span title="Editar 6789 123456789 123456789 123456789 123456789 1234567890" class="btn btn-xs btn-info" onclick="document.location='EquipmentView.aspx?id=2';"><i class="icon-edit bigger-120"></i></span> */
        var buttonEdit = document.createElement("span");
        buttonEdit.title = Dictionary.Common_Edit + " " + equipment.Descripcion;
        buttonEdit.className = "btn btn-xs btn-info";
        buttonEdit.onclick = function () { document.location = "EquipmentView.aspx?id=" + equipment.Id; };
        var iconEdit = document.createElement("i");
        iconEdit.className = "icon-edit bigger-120";
        buttonEdit.appendChild(iconEdit);
        tdAcciones.appendChild(buttonEdit);

        if (user.Grants["Equipment"].Delete === true) {
            /*<span title="Eliminar 180" class="btn btn-xs btn-danger" onclick="EquipmentDelete(2,'123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 1234567890');"><i class="icon-trash bigger-120"></i></span>*/
            var buttonDelete = document.createElement("span");
            buttonDelete.title = Dictionary.Common_Delete + " " + equipment.Descripcion;
            buttonDelete.className = "btn btn-xs btn-danger";
            buttonDelete.onclick = function () { EquipmentDelete(equipment.Id, equipment.Descripcion); };
            var iconDelete = document.createElement("i");
            iconDelete.className = "icon-trash bigger-120";
            buttonDelete.appendChild(iconDelete);
            tdAcciones.appendChild(document.createTextNode(" "));
            tdAcciones.appendChild(buttonDelete);
        }
    }

    //tr.appendChild(tdCodigo);
    tr.appendChild(tdDescripcion);
    tr.appendChild(tdUbicacion);
    tr.appendChild(tdResponsable);
    tr.appendChild(tdCoste);
    tr.appendChild(tdAdjuntos);
    tr.appendChild(tdAcciones);

    document.getElementById("ListDataTable").appendChild(tr);
}

function SetFilter() {
    var Filter = "";
    if (document.getElementById("RBOperation1").checked === true) { Filter += "C"; }
    if (document.getElementById("RBOperation2").checked === true) { Filter += "V"; }
    if (document.getElementById("RBOperation3").checked === true) { Filter += "M"; }
    Filter += "|";
    if (document.getElementById("RBStatus0").checked === true) { Filter += "0"; }
    if (document.getElementById("RBStatus1").checked === true) { Filter += "1"; }
    if (document.getElementById("RBStatus2").checked === true) { Filter += "2"; }

    var data = { "filter": Filter };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EquipmentActions.asmx/SetFilter",
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