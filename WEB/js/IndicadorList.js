var IndicadorList = null;
var filterData = null;

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

    var options = $.extend({}, $.datepicker.regional["<%=this.UserLanguage %>"], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);

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
    document.getElementById("nav-search-input").value = "";
    var ok = true;
    document.getElementById("ListDataTable").style.display = "none";
    document.getElementById("ItemTableError").style.display = "none";
    document.getElementById("ItemTableVoid").style.display = "none";
    document.getElementById("ErrorDate").style.display = "none";
    document.getElementById("ErrorStatus").style.display = "none";
    document.getElementById("ErrorType").style.display = "none";
    var from = GetDate($("#TxtDateFrom").val(), "-");
    var to = GetDate($("#TxtDateTo").val(), "-");

    var indicatorType = $("#CmbTipusIndicador").val() * 1;
    var processType = $("#CmbProcessType").val() * 1; if (processType === 0) { processType = null; }
    var process = $("#CmbProcess").val() * 1; if (process === 0) { process = null; }
    var objetivo = $("#CmbObjetivo").val() * 1; if (objetivo === 0) { objetivo = null; }

    if (from !== null && to !== null) {
        if (from > to) {
            ok = false;
            document.getElementById("ErrorDate").style.display = "";
        }
    }

    var status = 0;
    if (document.getElementById("RBStatus1").checked === true) { status = 1; }
    if (document.getElementById("RBStatus2").checked === true) { status = 2; }

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
        type: "POST",
        url: "/Async/IndicadorActions.asmx/GetFilter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(filterData, null, 2),
        success: function (msg) {
            eval("IndicadorList=" + msg.d + ";");
            ItemRenderTable(IndicadorList);
            if (exportType !== "undefined") {
                if (exportType === "PDF") {
                    ExportPDF();
                }
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function IndicadorGetNone() {
    document.getElementById("BtnRecordShowAll").style.display = "";
    document.getElementById("BtnRecordShowNone").style.display = "none";
    document.getElementById("TxtDateFrom").value = "";
    document.getElementById("TxtDateTo").value = "";
    $("#CmbProcess").val(0);
    $("#CmbProcessType").val(0);
    $("#CmbObjetivo").val(0);
    $("#CmbTipusIndicador").val(0);
    document.getElementById("RBStatus1").checked = false;
    VoidTable("ListDataTable");
}

function IndicadorGetAll() {
    document.getElementById("BtnRecordShowAll").style.display = "none";
    document.getElementById("BtnRecordShowNone").style.display = "";
    document.getElementById("TxtDateFrom").value = "";
    document.getElementById("TxtDateTo").value = "";
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
        document.getElementById("ItemTableVoid").style.display = "";
        $("#NumberCosts").html("0");
        target.style.display = "none";
        return false;
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
        document.getElementById('nav-search').style.display = 'none';
    }
    else {
        document.getElementById('nav-search').style.display = '';

        items.sort(function (a, b) {
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        })
        var autocomplete = $('.nav-search-input').typeahead();
        autocomplete.data('typeahead').source = items;

        $('#nav-search-input').keyup(FilterList);
        $('#nav-search-input').change(FilterList);
    }

    $("#NumberCosts").html(list.length);
    $("#th1").click();
    if (document.getElementById("th1").className.indexOf("DESC") !== -1) {
        $("#th1").click();
    }
}

function IndicadorDelete(sender) {
    IndicadorSelectedId = sender.parentNode.parentNode.id * 1;
    IndicadorSelected = IndicadorGetById(IndicadorSelectedId);
    if (IndicadorSelectedId === null) { return false; }
    $("#IndicadorDeleteName").html(IndicadorSelected.IndicadorDescription);
    var dialog = $("#IndicadorDeleteDialog").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Common_Delete,
        title_html: true,
        buttons:
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    IndicadorDeleteConfirmed();
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
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            IndicadorGetFilter();
        },
        error: function (msg) {
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
    var listTable = document.getElementById("ListDataDiv");
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 420) + "px";
}

window.onload = function () {
    /*$(".breadcrumb").css("font-size", "18px");
    $(".breadcrumb").css("margin-top", "8px");
    $(".page-header").hide();
    $("#FooterButton").css("height", "40px");
    $("#ImgCompany").hide();
    $("#HeaderButtons").hide();
    $("#ImgCompany").parent().append($("#HeaderButtons").html());*/


    //$("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export("PDF");\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
    Resize();

    console.log("Filter", Filter);
    if (Filter !== null) {
        console.log("Filter", Filter);
        document.getElementById("TxtDateFrom").value = GetDateYYYYMMDDText(Filter.from, "/", false);
        document.getElementById("TxtDateTo").value = GetDateYYYYMMDDText(Filter.to, "/", false);
        $("#CmbTipusIndicador").val(Filter.indicatorType);
        if (Filter.status === 0) { document.getElementById("RBStatus0").checked = true; }
        if (Filter.status === 1) { document.getElementById("RBStatus1").checked = true; }
        if (Filter.status === 2) { document.getElementById("RBStatus2").checked = true; }
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

    IndicadorGetFilter();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export();\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;")
}

window.onresize = function () { Resize(); }

function Export() {
    IndicadorGetFilter("PDF");
}

function ExportPDF() {
    console.clear();
    console.log(filterData);
    var webMethod = "/Export/IndicadorExport.aspx/PDF";
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(filterData, null, 2),
        success: function (msg) {
            LoadingHide();
            //successInfoUI(msg.d.MessageError, Go, 200);
            var link = document.createElement("a");
            link.id = "download";
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = "_blank";
            document.body.appendChild(link);
            document.body.removeChild(link);
            $("#download").trigger("click");
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog("close");
        },
        error: function (msg) {
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