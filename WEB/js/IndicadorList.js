﻿var IndicadorList = null;
var filterData = null;
var lockOrderList = false;

function CmbTipusIndicadorChange() {
    var tipus = $("#CmbTipusIndicador").val() * 1;
    $("#CmdObjetivoLabel").hide();
    $("#CmbProcesoLabel").hide();
    $("#CmbProcesoTipoLabel").hide();
    $("#CmbProcess").hide();
    $("#CmbObjetivo").hide();
    $("#CmbProcessType").hide();
    $("#CmbProcess").val(0);
    $("#CmbObjetivo").val(0);
    $("#CmbProcessType").val(0);

    if (tipus === 1) {
        $("#CmbProcesoLabel").show();
        $("#CmbProcesoTipoLabel").show();
        $("#CmbProcess").show();
        $("#CmbProcessType").show();
    }

    if (tipus === 2) {
        $("#CmdObjetivoLabel").show();
        $("#CmbObjetivo").show();
    }

    IndicadorGetFilter();
}

function GetProcessById(id) {
    for (var x = 0; x < Procesos.length; x++) {
        if (Procesos[x].Id === id) {
            return Procesos[x];
        }
    }

    return null;
}

function CmbProcessChanged() {
    var processId = $("#CmbProcess").val() * 1;
    var proceso = GetProcessById(processId);
    if (proceso !== null) {
        $("#CmbProcessType").attr("disabled", "disabled");
        $("#CmbProcessType").val(proceso.ProcessType);
    }
    else {
        $("#CmbProcessType").removeAttr("disabled");
        $("#CmbProcessType").val(0);
    }

    IndicadorGetFilter();
}

function CmbProcessTypeChanged() {
    IndicadorGetFilter();
}

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));

    var options = $.extend({}, $.datepicker.regional["ca"], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);
    $(".date-picker").on("blur", function () { DatePickerChanged(this); });

    $("#BtnSearch").on("click", function (e) {
        e.preventDefault();
        IndicadorGetFilter();
    });

    $("#BtnRecordShowAll").on("click", function (e) {
        e.preventDefault();
        IndicadorGetAll();
    });

    $("#BtnRecordShowNone").on("click", function (e) {
        e.preventDefault();
        IndicadorGetNone();
    });
});

function IndicadorGetFilter(exportType) {
    $("#nav-search-input").val();
    var ok = true;
    $("#ListDataTable").hide();
    $("#ItemTableError").hide();
    $("#NoData").hide();
    $("#ErrorDate").hide();
    $("#ErrorStatus").hide();
    $("#ErrorType").hide();
    var from = GetDate($("#TxtDateFrom").val(), "-");
    var to = GetDate($("#TxtDateTo").val(), "-");

    var indicatorType = $("#CmbTipusIndicador").val() * 1;
    var processType = $("#CmbProcessType").val() * 1; if (processType === 0) { processType = null; }
    var process = $("#CmbProcess").val() * 1; if (process === 0) { process = null; }
    var objetivo = $("#CmbObjetivo").val() * 1; if (objetivo === 0) { objetivo = null; }

    if (from !== null && to !== null) {
        if (from > to) {
            ok = false;
            $("#ErrorDate").show();
        }
    }

    var status = 0;
    $("#RBStatus1").removeAttr("disabled");
    $("#RBStatus2").removeAttr("disabled");
    if (document.getElementById("RBStatus1").checked === true &&
        document.getElementById("RBStatus2").checked === true) { status = 0; }
    else {
        if (document.getElementById("RBStatus1").checked === true) {
            status = 1;
            $("#RBStatus1").attr("disabled", "disabled");
        }
        if (document.getElementById("RBStatus2").checked === true) {
            status = 2;
            $("#RBStatus2").attr("disabled", "disabled");
        }
    }
     
    filterData =
    {
        "companyId": Company.Id,
        "indicatorType": indicatorType,
        "from": from,
        "to": to,
        "processId": process,
        "processTypeId": processType,
        "targetId": objetivo,
        "status": status
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/IndicadorActions.asmx/GetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(filterData, null, 2),
        "success": function (msg) {
            eval("IndicadorList=" + msg.d + ";");
            ItemRenderTable(IndicadorList);
            if (exportType !== "undefined" && exportType !== null) {
                if (exportType === "PDF") {
                    ExportPDF();
                }
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function IndicadorGetNone() {
    $("#BtnRecordShowAll").show();
    $("#BtnRecordShowNone").hide();
    $("#TxtDateFrom").val("");
    $("#TxtDateTo").val("");
    $("#CmbProcess").val(0);
    $("#CmbProcessType").val(0);
    $("#CmbObjetivo").val(0);
    $("#CmbTipusIndicador").val(0);
    document.getElementById("RBStatus1").checked = false;
    VoidTable("ListDataTable");
}

function IndicadorGetAll() {
    $("#TxtDateFrom").val("");
    $("#TxtDateTo").val("");
    $("#CmbProcess").val(0);
    $("#CmbProcessType").val(0);
    $("#CmbObjetivo").val(0);
    $("#CmbTipusIndicador").val(0);
    document.getElementById("RBStatus0").checked = true;
    IndicadorGetFilter();
}

function ItemRenderTable(list) {
    var items = new Array();
    var target = document.getElementById("ListDataTable");
    VoidTable("ListDataTable");
    target.style.display = "";

    if (list.length === 0) {
        $("#NoData").show();
        $("#TotalList").html("0");
        $("#DataTable").hide();
        return false;
    } else {
        $("#DataTable").show();
    }

    var total = 0;
    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        console.log("Item", item);
        var row = document.createElement("TR");
        row.id = item.IndicadorId;
        var tdIndicador = document.createElement("TD");
        var tdTipoProceso = document.createElement("TD");
        var tdProceso = document.createElement("TD");
        var tdObjetivo = document.createElement("TD");
        var tdStartDate = document.createElement("TD");
        var tdProcesoResponsible = document.createElement("TD");
        var tdObjetivoResponsible = document.createElement("TD");

        if (item.EndDate !== null) {
            row.style.fontStyle = "italic";
        }

        // ---- INDICADOR
        var indicadorLink = document.createElement("A");
        indicadorLink.href = "IndicadorView.aspx?id=" + item.IndicadorId;
        indicadorLink.appendChild(document.createTextNode(item.IndicadorDescription));
        tdIndicador.appendChild(indicadorLink);

        // ---- TIPO PROCESO
        tdTipoProceso.appendChild(document.createTextNode(TipoProcesoById(item.ProcessType)));

        // ---- PROCESO
        var procesoLink = document.createElement("A");
        procesoLink.href = "ProcesosView.aspx?id=" + item.ProcessId;
        procesoLink.appendChild(document.createTextNode(item.ProcessDescription));
        tdProceso.appendChild(procesoLink);

        // ---- OBJETIVO
        var objetivoLink = document.createElement("A");
        objetivoLink.href = "ObjetivoView.aspx?id=" + item.ObjetivoId;
        objetivoLink.appendChild(document.createTextNode(item.ObjetivoDescription));
        tdObjetivo.appendChild(objetivoLink);

        // ---- FECHA INICIO
        tdStartDate.appendChild(document.createTextNode(FormatYYYYMMDD(item.StartDate, "/")));

        // ---- RESPONSABLE PROCESO
        tdProcesoResponsible.appendChild(document.createTextNode(item.ProcessResponsible));

        // ---- RESPONSABLE OBJETIVO
        tdObjetivoResponsible.appendChild(document.createTextNode(item.ObjetivoResponsible));

        //tdDescription.style.width = "200px";

        tdStartDate.style.width = "90px";
		tdProceso.style.width = "250px";
		tdTipoProceso.style.width = "110px";
        tdObjetivo.style.width = "200px";
		tdProcesoResponsible.style.width = "100px";
		tdObjetivoResponsible.style.width = "200px";
        
        //row.appendChild(tdNumber);
        //row.appendChild(tdNumber);
        row.appendChild(tdIndicador);
        row.appendChild(tdStartDate);
        row.appendChild(tdProceso);
        row.appendChild(tdTipoProceso);
        //row.appendChild(tdObjetivo);
        //row.appendChild(tdProcesoResponsible);
        row.appendChild(tdObjetivoResponsible);

        var iconEdit = document.createElement("SPAN");
        iconEdit.className = "btn btn-xs btn-info";
        iconEdit.id = item.Number;
        var innerEdit = document.createElement("I");
        innerEdit.className = ApplicationUser.Grants.Indicador.Write ? "icon-edit bigger-120" : "icon-eye-open bigger-120";
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { document.location = "IndicadorView.aspx?id=" + this.parentNode.parentNode.id; };

        if (ApplicationUser.Grants.Indicador.Delete === true) {
            var iconDelete = document.createElement("SPAN");
            iconDelete.className = "btn btn-xs btn-danger";
            iconDelete.id = item.Number;
            var innerDelete = document.createElement("I");
            innerDelete.className = "icon-trash bigger-120";
            iconDelete.appendChild(innerDelete);

            if (item.ObjetivoId > 0) {
                iconDelete.onclick = function () { NoDeleteIndicador(); };
            }
            else if (item.Origin === 4) {
                iconDelete.onclick = function () { NoDeleteIndicador(); };
            }
            else {
                iconDelete.onclick = function () { IndicadorDelete(this); };
            }
        }

        var tdActions = document.createElement("TD");
        tdActions.style.width = "90px";

        tdActions.appendChild(iconEdit);
        if (ApplicationUser.Grants.Indicador.Delete) {
            tdActions.appendChild(document.createTextNode(" "));
            tdActions.appendChild(iconDelete);
        }
        row.appendChild(tdActions);

        target.appendChild(row);

        console.log("Search", item.IndicadorDescription);
        if ($.inArray(item.IndicadorDescription, items) === -1) {
            items.push(item.IndicadorDescription);
        }

        if ($.inArray(item.ProcessDescription, items) === -1) {
            items.push(item.ProcessDescription);
        }
    }


    if (items.length === 0) {
        $("#nav-search").hide();
    }
    else {
        $("#nav-search").show();

        items.sort(function (a, b) {
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        });

        var autocomplete = $(".nav-search-input").typeahead();
        autocomplete.data("typeahead").source = items;

        $("#nav-search-input").keyup(FilterList);
        $("#nav-search-input").change(FilterList);
    }

    $("#TotalList").html(list.length);

    if (lockOrderList === false) {
        $("#th0").click();
        if (document.getElementById("th0").className.indexOf("DESC") !== -1) {
            $("#th0").click();
        }
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

function IndicadorDelete(sender) {
    IndicadorSelectedId = sender.parentNode.parentNode.id * 1;
    IndicadorSelected = IndicadorGetById(IndicadorSelectedId);
    if (IndicadorSelectedId === null) { return false; }
    $("#IndicadorDeleteName").html(IndicadorSelected.IndicadorDescription);
    var dialog = $("#IndicadorDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        "buttons":
        [
            {
                "id": "IndicadorDeleteBtnOk",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () { IndicadorDeleteConfirmed(); }
            },
            {
                "id": "IndicadorDeleteBtnCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function IndicadorDeleteConfirmed() {
    var webMethod = "/Async/IndicadorActions.asmx/InactivateIndicator";
    var data = {
        "indicatorId": IndicadorSelectedId,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#IndicadorDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            IndicadorGetFilter();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function IndicadorGetById(id) {
    id = id * 1;
    for (var x = 0; x < IndicadorList.length; x++) {
        if (IndicadorList[x].IndicadorId === id) {
            return IndicadorList[x];
        }
    }
    return null;
}

function NoDeleteProcess() {
    alertInfoUI(Dictionary.Item_IncidentAction_ErrorMessage_NoDeleteBusinessRisk, null);
}

function NoDeleteIncident() {
    alertInfoUI(Dictionary.Item_IncidentAction_ErrorMessage_NoDeleteIncident, null);
}

$("#nav-search").hide();

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 420);
    $("#NoData").height(containerHeight - 420);
}

window.onload = function () {
    Resize();
    if (Filter !== null) {
        console.log("Filter", Filter);
        $("#TxtDateFrom").val(GetDateYYYYMMDDText(Filter.from, "/", false));
        $("#TxtDateTo").val(GetDateYYYYMMDDText(Filter.to, "/", false));
        $("#CmbTipusIndicador").val(Filter.indicatorType);

        switch (Filter.status) {
            case 0:
                document.getElementById("RBStatus1").checked = true;
                document.getElementById("RBStatus2").checked = true;
                break;
            case 1:
                document.getElementById("RBStatus1").checked = true;
                $("#RBStatus1").attr("disabled", "disabled");
                break;
            case 2:
                document.getElementById("RBStatus2").checked = true;
                $("#RBStatus2").attr("disabled", "disabled");
                break;
        }

        //if (Filter.status === 0) { document.getElementById("RBStatus0").checked = true; }
        //if (Filter.status === 1) { document.getElementById("RBStatus1").checked = true; }
        //if (Filter.status === 2) { document.getElementById("RBStatus2").checked = true; }
        $("#CmbProcess").val(Filter.process);
        if (Filter.process > 0) {
            CmbProcessChanged();
        }
        else {
            $("#CmbProcessType").val(Filter.processType);
        }

        $("#CmbObjetivo").val(Filter.objetivo);
        CmbTipusIndicadorChange();
    }
    else {
        document.getElementById("RBStatus1").checked = true;
    }

    $("#RBStatus0").on("click", IndicadorGetFilter);
    $("#RBStatus1").on("click", IndicadorGetFilter);
    $("#RBStatus2").on("click", IndicadorGetFilter);
    $("#TxtDateFrom").on("change", IndicadorGetFilter);
    $("#TxtDateTo").on("change", IndicadorGetFilter);
    $("#CmbObjetivo").on("change", IndicadorGetFilter);

    IndicadorGetFilter();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export();\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
};

window.onresize = function () { Resize(); };

function Export() {
    lockOrderList = true;
    IndicadorGetFilter("PDF");
}

function ExportPDF() {
    console.clear();
    console.log(filterData);
    var data = filterData;
    if (typeof listOrder === "undefined" || listOrder === null) {
        listOrder = "TH0|ASC";
    }
    data["listOrder"] = listOrder;
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/IndicadorExport.aspx/PDF",
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

function TipoProcesoById(id) {
    for (var x = 0; x < TiposProceso.length; x++) {
        if (TiposProceso[x].Id === id) {
            return TiposProceso[x].Description;
        }
    }

    return "";
}

function NoDeleteIndicador() {
    alertInfoUI(Dictionary.Item_Indicador_ErrorMessage_NoDelete, null);
}