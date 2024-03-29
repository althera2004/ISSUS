﻿var EquipmentSelected;
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
        "success": function () {
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
    $("#EquipmentDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Equipment_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "id": "BtnDeleteOk",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    EquipmentDeleteAction();
                }
            },
            {
                "id": "BtnDeleteCancel",
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
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true)
                title.html($title);
            else title.text($title);
        }
    }));
});

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 430);
    $("#ListDataDivCosts").height(containerHeight - 440);
}

window.onload = function () {
    var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);
    Resize();
    document.getElementById("RBOperation1").checked = Filter.indexOf("C") !== -1;
    document.getElementById("RBOperation2").checked = Filter.indexOf("V") !== -1;
    document.getElementById("RBOperation3").checked = Filter.indexOf("M") !== -1;
    document.getElementById("RBStatus1").checked = Filter.indexOf("1") !== -1;
    document.getElementById("RBStatus2").checked = Filter.indexOf("2") !== -1;
    RenderTable();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");

    if (document.getElementById("RBOperation1").checked === true && document.getElementById("RBOperation2").checked === false && document.getElementById("RBOperation3").checked === false) {
        $("#RBOperation1").attr("disabled", "disabled");
    }

    if (document.getElementById("RBOperation1").checked === false && document.getElementById("RBOperation2").checked === true && document.getElementById("RBOperation3").checked === false) {
        $("#RBOperation2").attr("disabled", "disabled");
    }

    if (document.getElementById("RBOperation1").checked === false && document.getElementById("RBOperation2").checked === false && document.getElementById("RBOperation3").checked === true) {
        $("#RBOperation3").attr("disabled", "disabled");
    }

    if (document.getElementById("RBStatus1").checked === true && document.getElementById("RBStatus2").checked === false) {
        $("#RBStatus1").attr("disabled", "disabled");
    }

    if (document.getElementById("RBStatus1").checked === false && document.getElementById("RBStatus2").checked === true) {
        $("#RBStatus2").attr("disabled", "disabled");
    }

    $("#BtnExportList").after("<button class=\"btn btn-info\" style=\"display:none;\" type=\"button\" id=\"BtnExportCosts\" onclick=\"ExportCosts('PDF');\"><i class=\"icon-print bigger-110\"></i>Imprimir</button>");

    $("#TabEquipmentList").on("click", function () {
        $("#BtnNewItem").css("visibility", "visible"); $("#BtnExportList").show(); $("#BtnExportCosts").hide();
    });
    $("#TabCostList").on("click", function () { $("#BtnNewItem").css("visibility", "hidden"); $("#BtnExportList").hide(); $("#BtnExportCosts").show(); });

    SetFilterCostLayout();
    SetFilterCosts();

    $("#RBCI").on("change", SetFilterCosts);
    $("#RBCE").on("change", SetFilterCosts);
    $("#RBVI").on("change", SetFilterCosts);
    $("#RBVE").on("change", SetFilterCosts);
    $("#RBMI").on("change", SetFilterCosts);
    $("#RBME").on("change", SetFilterCosts);
    $("#RBRI").on("change", SetFilterCosts);
    $("#RBRE").on("change", SetFilterCosts);
    $("#TabCostList").on("click", SetFilterCosts);
};

function SetFilterCostLayout() {
    // Set costs filter
    $("#RBCI").prop("checked", false);
    $("#RBCE").prop("checked", false);
    $("#RBVI").prop("checked", false);
    $("#RBVE").prop("checked", false);
    $("#RBMI").prop("checked", false);
    $("#RBME").prop("checked", false);
    $("#RBRI").prop("checked", false);
    $("#RBRE").prop("checked", false);

    console.log("FilterCosts", FilterCosts);
    var filterCostsPart = FilterCosts.split("|");
    var from = filterCostsPart[0];
    var to = filterCostsPart[1];
    if (from !== "") {
        $("#TxtDateFrom").val(FormatYYYYMMDD(from, '/'));
    }

    if (to !== "") {
        $("#TxtDateTo").val(FormatYYYYMMDD(to, '/'));
    }

    if (FilterCosts.indexOf("|CI") !== -1) { $("#RBCI").prop("checked", true); }
    if (FilterCosts.indexOf("|CE") !== -1) { $("#RBCE").prop("checked", true); }
    if (FilterCosts.indexOf("|VI") !== -1) { $("#RBVI").prop("checked", true); }
    if (FilterCosts.indexOf("|VE") !== -1) { $("#RBVE").prop("checked", true); }
    if (FilterCosts.indexOf("|MI") !== -1) { $("#RBMI").prop("checked", true); }
    if (FilterCosts.indexOf("|ME") !== -1) { $("#RBME").prop("checked", true); }
    if (FilterCosts.indexOf("|RI") !== -1) { $("#RBRI").prop("checked", true); }
    if (FilterCosts.indexOf("|RE") !== -1) { $("#RBRE").prop("checked", true); }
};

window.onresize = function () { Resize(); };

function Export(fileType) {
    console.log("Export", fileType);
    var data = {
        "companyId": Company.Id,
        "listOrder": listOrder,
        "filterText": $("#nav-search-input").val().trim()
    };
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/EquipmentExportList.asmx/" + fileType,
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

function RBOperationChanged() {
    $("#RBOperation1").removeAttr("disabled");
    $("#RBOperation2").removeAttr("disabled");
    $("#RBOperation3").removeAttr("disabled");

    if (document.getElementById("RBOperation1").checked === true && document.getElementById("RBOperation2").checked === false && document.getElementById("RBOperation3").checked === false) {
        $("#RBOperation1").attr("disabled", "disabled");
    }

    if (document.getElementById("RBOperation1").checked === false && document.getElementById("RBOperation2").checked === true && document.getElementById("RBOperation3").checked === false) {
        $("#RBOperation2").attr("disabled", "disabled");
    }

    if (document.getElementById("RBOperation1").checked === false && document.getElementById("RBOperation2").checked === false && document.getElementById("RBOperation3").checked === true) {
        $("#RBOperation3").attr("disabled", "disabled");
    }

    RenderTable();
}

function RBStatusChanged() {
    $("#RBStatus1").removeAttr("disabled");
    $("#RBStatus2").removeAttr("disabled");
    if (document.getElementById("RBStatus1").checked === true && document.getElementById("RBStatus2").checked === false) {
        $("#RBStatus1").attr("disabled", "disabled");
    }

    if (document.getElementById("RBStatus2").checked === true && document.getElementById("RBStatus1").checked === false) {
        $("#RBStatus2").attr("disabled", "disabled");
    }

    RenderTable();
}

function RenderTable() {
    SetFilter();
    $("#ListDataTable").html("");
    var temp = [];
    for (var x = 0; x < Equipments.length; x++) {
        var ok = false;
        var equipment = Equipments[x];
        if (document.getElementById("RBStatus1").checked === true && equipment.Activo === true) { ok = true; }
        if (document.getElementById("RBStatus2").checked === true && equipment.Activo === false) { ok = true; }
        if (ok === true) {
            if (document.getElementById("RBOperation1").checked === true && equipment.Calibracion === true) {
                temp.push(equipment);
            }
            else if (document.getElementById("RBOperation2").checked === true && equipment.Verificacion === true) {
                temp.push(equipment);
            }
            else if (document.getElementById("RBOperation3").checked === true && equipment.Mantenimiento === true) {
                temp.push(equipment);
            }
        }
    }

    $("#TotalList").html(temp.length);
    var total = 0;
    if (temp.length > 0) {
        for (var y = 0; y < temp.length; y++) {
            total += RenderRow(temp[y]);
        }
    }

    $("#TotalAmount").html(ToMoneyFormat(total, 2));

    if (listOrder === null) {
        $("#th0").click();
    }
    else {
        var column = listOrder.split("|")[0];
        var order = listOrder.split("|")[1];

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

    // @cristina --> aquí se decide el tipo de fuente para toda la fila
    if (equipment.Activo === false) {
        tr.style.fontStyle = "italic";
    }

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

    var rowCost = 0;
    for (var c = 0; c < Costs.length; c++) {
        if (Costs[c].E === equipment.Id) {
            if (Costs[c].T === "R") { rowCost += Costs[c].A; }
            if (Costs[c].T === "C" && $("#RBOperation1").prop("checked") === true) { rowCost += Costs[c].A; }
            if (Costs[c].T === "V" && $("#RBOperation2").prop("checked") === true) { rowCost += Costs[c].A; }
            if (Costs[c].T === "M" && $("#RBOperation3").prop("checked") === true) { rowCost += Costs[c].A; }
        }
    }

    tdCoste.appendChild(document.createTextNode(ToMoneyFormat(rowCost, 2)));

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
        var buttonEdit = document.createElement("span");
        buttonEdit.title = Dictionary.Common_Edit + " " + equipment.Descripcion;
        buttonEdit.className = "btn btn-xs btn-info";
        buttonEdit.onclick = function () { document.location = "EquipmentView.aspx?id=" + equipment.Id; };
        var iconEdit = document.createElement("i");
        iconEdit.className = "icon-edit bigger-120";
        buttonEdit.appendChild(iconEdit);
        tdAcciones.appendChild(buttonEdit);

        if (user.Grants["Equipment"].Delete === true) {
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

        if (equipment.Baja === true) {
            tr.style.fontStyle = "italic";
        }

    }

    tr.appendChild(tdDescripcion);
    tr.appendChild(tdUbicacion);
    tr.appendChild(tdResponsable);
    tr.appendChild(tdAdjuntos);
    tr.appendChild(tdAcciones);

    document.getElementById("ListDataTable").appendChild(tr);

    return rowCost;
}

function SetFilter() {
    var Filter = "";
    if (document.getElementById("RBOperation1").checked === true) { Filter += "C"; }
    if (document.getElementById("RBOperation2").checked === true) { Filter += "V"; }
    if (document.getElementById("RBOperation3").checked === true) { Filter += "M"; }
    Filter += "|";
    //if (document.getElementById("RBStatus0").checked === true) { Filter += "0"; }
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
            console.log("SetFilter", msg);
        },
        "error": function (msg) {
            console.log("SetFilter", msg.responseText);
        }
    });
}

function SetFilterCosts() {
    var Filter = "";
    $(".RBCost").removeAttr("disabled");
    if ($("#RBCE").prop("checked") === true) { Filter += "|CE"; }
    if ($("#RBVE").prop("checked") === true) { Filter += "|VE"; }
    if ($("#RBME").prop("checked") === true) { Filter += "|ME"; }
    if ($("#RBRE").prop("checked") === true) { Filter += "|RE"; }
    if ($("#RBCI").prop("checked") === true) { Filter += "|CI"; }
    if ($("#RBVI").prop("checked") === true) { Filter += "|VI"; }
    if ($("#RBMI").prop("checked") === true) { Filter += "|MI"; }
    if ($("#RBRI").prop("checked") === true) { Filter += "|RI"; }

    if ($(".RBCost:checked").length < 2) {
        $(".RBCost:checked").attr("disabled", "disabled");
    }

    var from = GetDate($("#TxtDateFrom").val(), "/", true);
    var to = GetDate($("#TxtDateTo").val(), "/", true);

    if (from.getFullYear() === 1970) { from = ""; } else {
        from = GetDateYYYYMMDDToText(from, false);
    }
    if (to.getFullYear() === 1970) { to = ""; } else {
        to = GetDateYYYYMMDDToText(to, false);
    }

    var data = {
        "from": from,
        "to": to,
        "filter": Filter,
        "companyId": Company.Id
    };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EquipmentActions.asmx/SetFilterCosts",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("SetFilterCosts", msg);
            RenderTableCosts(eval(msg.d));
        },
        "error": function (msg) {
            console.log("SetFilter", msg.responseText);
        }
    });
}

function RenderTableCosts(data) {
    console.log("RenderTableCosts");
    $("#ListDataTableCosts").html("");
    var temp = [];
    var CI = $("#RBCI").prop("checked") === true;
    var CE = $("#RBCE").prop("checked") === true;
    var VI = $("#RBVI").prop("checked") === true;
    var VE = $("#RBVE").prop("checked") === true;
    var MI = $("#RBMI").prop("checked") === true;
    var ME = $("#RBME").prop("checked") === true;
    var RI = $("#RBRI").prop("checked") === true;
    var RE = $("#RBRE").prop("checked") === true;

    // Redraw Header
    if (CI === true) { $("#HCI").show(); $("#TCI").show(); } else { $("#HCI").hide(); $("#TCI").hide(); }
    if (CE === true) { $("#HCE").show(); $("#TCE").show(); } else { $("#HCE").hide(); $("#TCE").hide(); }
    if (VI === true) { $("#HVI").show(); $("#TVI").show(); } else { $("#HVI").hide(); $("#TVI").hide(); }
    if (VE === true) { $("#HVE").show(); $("#TVE").show(); } else { $("#HVE").hide(); $("#TVE").hide(); }
    if (MI === true) { $("#HMI").show(); $("#TMI").show(); } else { $("#HMI").hide(); $("#TMI").hide(); }
    if (ME === true) { $("#HME").show(); $("#TME").show(); } else { $("#HME").hide(); $("#TME").hide(); }
    if (RI === true) { $("#HRI").show(); $("#TRI").show(); } else { $("#HRI").hide(); $("#TRI").hide(); }
    if (RE === true) { $("#HRE").show(); $("#TRE").show(); } else { $("#HRE").hide(); $("#TRE").hide(); }

    var totalCI = 0;
    var totalCE = 0;
    var totalVI = 0;
    var totalVE = 0;
    var totalMI = 0;
    var totalME = 0;
    var totalRI = 0;
    var totalRE = 0;
    var totalT = 0;

    var total = 0;
    var result = data;
    for (var s = 0; s < result.length; s++) {
        totalCI += result[s].CI;
        totalCE += result[s].CE;
        totalVI += result[s].VI;
        totalVE += result[s].VE;
        totalMI += result[s].MI;
        totalME += result[s].ME;
        totalRI += result[s].RI;
        totalRE += result[s].RE;
        totalT += RenderRowCosts(result[s]);
        total++;
    }

    $("#TotalListCosts").html($("#ListDataTableCosts TR").length);

    $("#TotalAmount").html(ToMoneyFormat(total, 2));
    $("#TCI").html(ToMoneyFormat(totalCI, 2));
    $("#TCE").html(ToMoneyFormat(totalCE, 2));
    $("#TVI").html(ToMoneyFormat(totalVI, 2));
    $("#TVE").html(ToMoneyFormat(totalVE, 2));
    $("#TMI").html(ToMoneyFormat(totalMI, 2));
    $("#TME").html(ToMoneyFormat(totalME, 2));
    $("#TRI").html(ToMoneyFormat(totalRI, 2));
    $("#TRE").html(ToMoneyFormat(totalRE, 2));
    $("#TT").html(ToMoneyFormat(totalT, 2));
    $("#TrCost").append("<th class=\"tdtemp\" style=\"width:17px;\"></th>"); 
    $("#TfCost").append("<th class=\"tdtemp\" style=\"width:17px;\"></th>"); 

    var countcell = 1;

    if (CI === true) { $("#HCI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TCI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    if (CE === true) { $("#HCE").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TCE").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    if (VI === true) { $("#HVI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TVI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    if (VE === true) { $("#HVE").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TVE").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    if (MI === true) { $("#HMI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TMI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    if (ME === true) { $("#HME").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TME").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    if (RI === true) { $("#HRI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TRI").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    if (RE === true) { $("#HRE").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); $("#TRE").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); countcell++; }
    $("#HT").width($("#ListDataTableCosts td:eq(" + countcell + ")").width());
    $("#TT").width($("#ListDataTableCosts td:eq(" + countcell + ")").width()); 
    $("#HT").show(); $("#TT").show();
    console.log(result); 
}

function RenderRowCosts(data) {
    var CI = $("#RBCI").prop("checked") === true;
    var CE = $("#RBCE").prop("checked") === true;
    var VI = $("#RBVI").prop("checked") === true;
    var VE = $("#RBVE").prop("checked") === true;
    var MI = $("#RBMI").prop("checked") === true;
    var ME = $("#RBME").prop("checked") === true;
    var RI = $("#RBRI").prop("checked") === true;
    var RE = $("#RBRE").prop("checked") === true;
    var tr = document.createElement("tr");
    var tdDescripcion = document.createElement("td");

    // @cristina --> aquí se decide el tipo de fuente para toda la fila
    if (data.Activo === false) {
        tr.style.fontStyle = "italic";
    }

    tdDescripcion.appendChild(document.createTextNode(data.D));
    //tdCoste.appendChild(document.createTextNode(ToMoneyFormat(rowCost, 2)));

    var totalFila = 0;
    tr.appendChild(tdDescripcion);

    if (CI === true) {
        var tdCI = document.createElement("TD");
        tdCI.style.maxWidth = "100px";
        tdCI.style.textAlign = "right";
        tdCI.appendChild(document.createTextNode(ToMoneyFormat(data.CI, 2)));
        totalFila += data.CI;
        tr.appendChild(tdCI);
    }

    if (CE === true) {
        var tdCE = document.createElement("TD");
        tdCE.style.maxWidth = "100px";
        tdCE.style.textAlign = "right";
        tdCE.appendChild(document.createTextNode(ToMoneyFormat(data.CE, 2)));
        totalFila += data.CE;
        tr.appendChild(tdCE);
    }

    if (VI === true) {
        var tdVI = document.createElement("TD");
        tdVI.style.maxWidth = "100px";
        tdVI.style.textAlign = "right";
        tdVI.appendChild(document.createTextNode(ToMoneyFormat(data.VI, 2)));
        totalFila += data.VI;
        tr.appendChild(tdVI);
    }

    if (VE === true) {
        var tdVE = document.createElement("TD");
        tdVE.style.maxWidth = "100px";
        tdVE.style.textAlign = "right";
        tdVE.appendChild(document.createTextNode(ToMoneyFormat(data.VE, 2)));
        totalFila += data.VE;
        tr.appendChild(tdVE);
    }

    if (MI === true) {
        var tdMI = document.createElement("TD");
        tdMI.style.maxWidth = "100px";
        tdMI.style.textAlign = "right";
        tdMI.appendChild(document.createTextNode(ToMoneyFormat(data.MI, 2)));
        totalFila += data.MI;
        tr.appendChild(tdMI);
    }

    if (ME === true) {
        var tdME = document.createElement("TD");
        tdME.style.maxWidth = "100px";
        tdME.style.textAlign = "right";
        tdME.appendChild(document.createTextNode(ToMoneyFormat(data.ME, 2)));
        totalFila += data.ME;
        tr.appendChild(tdME);
    }

    if (RI === true) {
        var tdRI = document.createElement("TD");
        tdRI.style.maxWidth = "100px";
        tdRI.style.textAlign = "right";
        tdRI.appendChild(document.createTextNode(ToMoneyFormat(data.RI, 2)));
        totalFila += data.RI;
        tr.appendChild(tdRI);
    }

    if (RE === true) {
        var tdRE = document.createElement("TD");
        tdRE.style.maxWidth = "100px";
        tdRE.style.textAlign = "right";
        tdRE.appendChild(document.createTextNode(ToMoneyFormat(data.RE, 2)));
        totalFila += data.RE;
        tr.appendChild(tdRE);
    }

    var td = document.createElement("TD");
    td.style.textAlign = "right";
    td.style.fontWeight = "bold";
    td.appendChild(document.createTextNode(ToMoneyFormat(totalFila, 2)));
    tr.appendChild(td);

    // Sólo aparece si el total es distinto a 0
    if (totalFila !== 0) {
        document.getElementById("ListDataTableCosts").appendChild(tr);
    }
    return totalFila;
}

function ExportCosts() {
    console.log("ExportCosts");
    var filter = "";
    var CI = $("#RBCI").prop("checked") === true;
    var CE = $("#RBCE").prop("checked") === true;
    var VI = $("#RBVI").prop("checked") === true;
    var VE = $("#RBVE").prop("checked") === true;
    var MI = $("#RBMI").prop("checked") === true;
    var ME = $("#RBME").prop("checked") === true;
    var RI = $("#RBRI").prop("checked") === true;
    var RE = $("#RBRE").prop("checked") === true;

    // Redraw Header
    if (CI === true) { filter += "|CI"; }
    if (CE === true) { filter += "|CE"; }
    if (VI === true) { filter += "|VI"; }
    if (VE === true) { filter += "|VE"; }
    if (MI === true) { filter += "|MI"; }
    if (ME === true) { filter += "|ME"; }
    if (RI === true) { filter += "|RI"; }
    if (RE === true) { filter += "|RE"; }


    var from = GetDate($("#TxtDateFrom").val(), "/", true);
    var to = GetDate($("#TxtDateTo").val(), "/", true);

    if (from.getFullYear() === 1970) { from = ""; } else {
        from = GetDateYYYYMMDDToText(from, false);
    }
    if (to.getFullYear() === 1970) { to = ""; } else {
        to = GetDateYYYYMMDDToText(to, false);
    }

    var data = {
        "from": from,
        "to": to,
        "companyId": Company.Id,
        "filter": filter
    };
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/EquipmentExportCosts.aspx/Pdf",
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