var selectedRegistros = [];
var firstChart = true;
var UnitSelected = null;
var $myPayloadMeter;
var lockOrderList = false;

window.onload = function () {
    $("#build").hide();

    IndicadorTypeLayout();
    FillComboUnidad();
    $("#CmbType").on("change", IndicadorTypeLayout);
    $("#CmbProcess").on("change", ProcessLayout);
    $("#BtnSave").on("click", Save);
    $("#BtnCancel").on("click", function (e) { document.location = referrer; });
    $("#BtnAnular").on("click", AnularPopup);
    $("#BtnRestaurar").on("click", Restore);

    $("#BtnUnitsBAR").on("click", function (e) {
        e.preventDefault();
        ShowUnitsBarPopup();
    });

    FillForm();

    if (Registros.length === 0) {
        $("#IndicadorRegistrosTable").hide();
        $("#ItemTableVoid").show();
    }

    if (typeof Indicador.EndDate !== "undefined" && Indicador.EndDate !== null) {
        $("#BtnRecordNew").hide();
    }

    Resize();

    var options = $.extend({}, $.datepicker.regional[userLanguage], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);

    $("#BtnRecordShowNone").on("click", IndicadorRegistroNone);
    $("#BtnRecordShowAll").on("click", IndicadorRegistroAll);
    $("#BtnRecordFilter").on("click", IndicadorRegistroFilter);
    $("#BtnRecordNew").on("click", RecordNew);

    $("#TxtRecordsFromDate").on("change", IndicadorRegistroFilter);
    $("#TxtRecordsToDate").on("change", IndicadorRegistroFilter);

    IndicadorRegistroFilter();
    $("#th1").click();
    $("#th1").click();

    $("#Tabgraphics").on("click", DrawGraphics);

    $("#CmbProcess").on("change", CmbProcessChanged);

    if (IndicadoresObjetivo.length > 0) {
        document.getElementById("CmbProcess").disabled = true;
        $("#ProcessTypeLabel").html(Dictionary.Item_Indicador_Message_ProcessBlocked);
        $("#CmbType").val(1);
    }

    $("#CmbProcess").after("<div id=\"LabelProcessType\" style=\"margin:4px;\"></div>");
    $("#DivCmbResponsible").removeAttr("style");
    $("#DivCmbProcess").removeAttr("style");
    $("#TxtMeta").val(ToMoneyFormat(Indicador.Meta, 2));

    $("#TxtAlarma").val(ToMoneyFormat(Indicador.Alarma, 2, true));
    ProcessLayout();
    AnulateLayout();

    if (Indicador.EndDate !== null) {
        DisableLayout();
    }

    $("#CmbResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbResponsible").val() * 1, Employees, this); });
    $("#CmbResponsibleRecord").on("change", function () { WarningEmployeeNoUserCheck($("#CmbResponsibleRecord").val() * 1, Employees, this); });

    RenderTableHistorico();

    if (ApplicationUser.Grants.Indicador.Read === false) {
        $("input").attr("disabled", "disabled");
        $("textarea").attr("disabled", "disabled");
        $("select").attr("disabled", "disabled");
        $("#BtnAnular").hide();
        $("#BtnRestore").hide();
        $("#BtnSave").hide();
        $("#Tabrecords").hide();
        $("#Tabgraphics").hide();
        $("#Tabhistoric").hide();
    }
    else {
        if (document.location.toString().indexOf("&Tab=Records") !== -1) {
            $("#Tabrecords a").click();
            $("#BtnRecordNew").click();
        }
    }

    Compute();

    if (IndicadorId > 0) {
        $("#BtnPrint").on("click", PrintData);
    }
    else {
        $("#BtnPrint").hide();
    }
};

window.onresize = function () { Resize(); };

function CmbProcessChanged() {
    if ($("#CmbProcess").val() * 1 > 0) {
        $("#CmbType").val(2);
    }
    else {
        $("#CmbType").val(0);
    }

    ProcessLayout();
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 480);
    $("#ListDataDivHistorico").height(containerHeight - 370);
}

function IndicadorTypeLayout() {
    var type = $("#CmbType").val() * 1;
    $("#CmbProcessLabel").attr("disabled", "disabled");
    $("#CmbProcess").attr("disabled", "disabled");
    // $("#CmbObjetivoLabel").attr("disabled", "disabled");
    $("#CmbObjetivo").attr("disabled", "disabled");
    $("#ProcessTypeLabel").attr("disabled", "disabled");

    if (IndicadoresObjetivo.legnth === 0) {
        $("#CmbProcessLabel").removeAttr("disabled");
        $("#CmbProcess").removeAttr("disabled");
    }
    else {
        $("#CmbProcess").val(0);
        ProcessLayout();
        document.getElementById("CmbProcessLabel").style.color = "#000";
        // document.getElementById("CmbProcessErrorRequired").style.display = "none";
    }

    /*if (type === 0 || type === 1) {
        // $("#CmbObjetivoLabel").removeAttr("disabled");
        $("#CmbObjetivo").removeAttr("disabled");
        $("#ProcessTypeLabel").removeAttr("disabled");
    }
    else {
        $("#CmbObjetivo").val(0);
        // document.getElementById("CmbObjetivoLabel").style.color = "#000";
        // document.getElementById("CmbObjetivoErrorRequired").style.display = "none";
    }*/
}

function ProcessLayout() {
    console.log("ProcessLayout", $("#CmbProcess").val());
    var processId = $("#CmbProcess").val() * 1;
    var processType = GetProcessType(processId);
    if (IndicadoresObjetivo.length === 0) {
        $("#CmbProcess").removeAttr("disabled");
        if ($("#CmbProcess").val() * 1 > 0) {
            $("#LabelProcessType").html(Dictionary.Item_ProcessType + ":&nbsp;<strong>" + processType + "</strong>");
        }
        else {
            $("#LabelProcessType").html("");
        }
    }
    else {
        $("#LabelProcessType").html("");
        $("#CmbProcess").attr("disabled", "disabled");
        $("#CmbProcess").parent().attr("title", Dictionary.Item_Indicador_Message_BlockedByObjetivo);
    }
}

function Save() {
    if (Validate() === false) {
        return false;
    }

    var alarmaFinalValue = null;
    if ($("#TxtAlarma").val() !== "") {
        alarmaFinalValue = StringToNumberNullable($("#TxtAlarma").val(), ".", ",")
    }

    var data = {
        "indicador": {
            "Id": IndicadorId,
            "CompanyId": ApplicationUser.CompanyId,
            "Description": $("#TxtDescription").val(),
            "Calculo": $("#TxtCalculo").val(),
            "Objetivo": { "Id": $("#CmbObjetivo").val() * 1 },
            "Proceso": { "Id": $("#CmbProcess").val() * 1 },
            "MetaComparer": $("#CmbMetaComparer").val(),
            "Meta": StringToNumberNullable($("#TxtMeta").val(), ".", ","),
            "Unidad": { "Id": $("#CmbUnidad").val() * 1 },
            "AlarmaComparer": $("#CmbAlarmaComparer").val(),
            "Alarma": alarmaFinalValue,
            "Periodicity": $("#TxtPeriodicity").val() * 1,
            "Responsible": { "Id": $("#CmbResponsible").val() * 1 },
            "StartDate": GetDate($("#TxtStartDate").val(), "/", false),
            "EndDate": Indicador.EndDate,
            "EndReason": Indicador.EndReason,
            "EndResponsible": { "Id": Indicador.EndResponsible.Id },
            "CreatedBy": { "Id": -1 },
            "CreatedOn": new Date(),
            "ModifiedBy": { "Id": -1 },
            "ModifiedOn": new Date(),
            "Active": false
        },
        "applicationUserId": ApplicationUser.Id
    };

    console.log(data);

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IndicadorActions.asmx/Save",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
				if(IndicadorId < 0){
					document.location = "IndicadorView.aspx?id=" + response.d.MessageError;
				}
				else {
					document.location = referrer;
				}
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function Validate() {
    var ok = true;
    var errorMessage = "";

    document.getElementById("TxtDescriptionLabel").style.color = "#000";
    document.getElementById("TxtStartDateLabel").style.color = "#000";
    document.getElementById("TxtCalculoLabel").style.color = "#000";
    document.getElementById("TxtPeriodicityLabel").style.color = "#000";
    document.getElementById("CmbUnidadLabel").style.color = "#000";
    document.getElementById("CmbMetaLabel").style.color = "#000";
    $("#TxtDescriptionErrorRequired").hide();
    $("#TxtStartDateErrorRequired").hide();
    $("#TxtStartDateErrorMalformed").hide();
    $("#TxtCalculoErrorRequired").hide();
    $("#TxtPeriodicityErrorRequired").hide();
    $("#CmbUnidadErrorRequired").hide();
    $("#CmbMetaErrorRequired").hide();

    if ($("#TxtDescription").val() === "") {
        ok = false;
        document.getElementById("TxtDescriptionLabel").style.color = "#f00";
        $("#TxtDescriptionErrorRequired").show();
    }

    if ($("#TxtStartDate").val() === "") {
        ok = false;
        $("#TxtStartDateErrorRequired").show();
        $("#TxtStartDateLabel").css("color", Color.Error);
    }
    else if (!validateDate($("#TxtStartDate").val())) {
        ok = false;
        $("#TxtStartDateErrorMalformed").show();
        $("#TxtStartDateLabel").css("color", Color.Error);
    }

    if ($("#TxtCalculo").val() === "") {
        ok = false;
        $("TxtCalculoLabel").css("color", Color.Error);
        $("#TxtCalculoErrorRequired").show();
    }

    if ($("#TxtPeriodicity").val() === "" || $("#TxtPeriodicity").val() * 1 === 0) {
        ok = false;
        $("TxtPeriodicityLabel").css("color", Color.Error);
        $("#TxtPeriodicityErrorRequired").show();
    }

    if ($("#CmbUnidad").val() * 1 === 0) {
        ok = false;
        $("CmbUnidadLabel").css("color", Color.Error);
        $("#CmbUnidadErrorRequired").show();
    }

    if ($("#CmbMetaComparer").val() * 1 === 0 || $("#TxtMeta").val() * 1 === 0) {
        ok = false;
        $("#CmbMetaLabel").css("color", Color.Error);
        $("#CmbMetaErrorRequired").show();
    }

    return ok;
}

function GetProcessType(id) {
    for (var x = 0; x < Procesos.length; x++) {
        if (Procesos[x].Id === id) {
            for (var y = 0; y < ProcesosType.length; y++) {
                if (ProcesosType[y].Id === Procesos[x].ProcessType) {
                    return ProcesosType[y].Description;
                }
            }

            return "";
        }
    }

    return "";
}

function FillComboUnidad() {
    var target = document.getElementById("CmbUnidad");
    for (var x = 0; x < Unidades.length; x++) {
        if (Unidades[x].Active === true) {
            var option = document.createElement("OPTION");
            option.value = Unidades[x].Id;
            option.appendChild(document.createTextNode(Unidades[x].Description));
            target.appendChild(option);
        }
    }
}

function FillForm() {
    if (Indicador.Id > 0) {
        $("#TxtDescription").val(Indicador.Description);
        $("#TxtCalculo").val(Indicador.Calculo.split("\\n").join("\n"));
        $("#CmbMetaComparer").val(Indicador.MetaComparer);
        $("#CmbAlarmaComparer").val(Indicador.AlarmaComparer);
        $("#TxtMeta").val(Indicador.Meta);
        $("#TxtAlarma").val(Indicador.Alarma);
        $("#TxtStartDate").val(Indicador.StartDate);
        $("#CmbUnidad").val(Indicador.Unidad.Id);
        $("#TxtPeriodicity").val(Indicador.Periodicity);

        if (Indicador.Proceso.Id !== null) {
            $("#CmbProcess").val(Indicador.Proceso.Id);            
        }

        ProcessLayout();
    }
}

function RenderRegistroRow(registro) {

    var colorAssolit = "#a5ca9f!important";
    var colorNoAssolit = "#e59729!important";
    var colorAlarma = "#dc8475!important";

    var color = "#dc8475";
    var statusLabel = Dictionary.Item_Indicador_StatusLabelWithoutMeta;
    var icon = "icon-circle bigger-110";
    var metaText = ToMoneyFormat(registro.Meta, 2);
    var alarmaText = ToMoneyFormat(registro.Alarma, 2);
    if (registro.Meta === null) {
        color = "#444";
        metaText = "";
        statusLabel = Dictionary.Item_Indicador_StatusLabelWithoutMeta;
    }
    else if (registro.MetaComparer === "=" && registro.Value === registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === ">" && registro.Value > registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === ">=" && registro.Value >= registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === "<" && registro.Value < registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === "<=" && registro.Value <= registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === ">" && registro.Value > registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === ">=" && registro.Value >= registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === "<" && registro.Value < registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.MetaComparer === "<=" && registro.Value <= registro.Meta) { color = colorAssolit; statusLabel = Dictionary.Item_Indicador_StatusLabelMeta; }
    else if (registro.Alarma !== null) {
        if (registro.AlarmaComparer === ">" && registro.Value > registro.Alarma) { color = colorAlarma; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Indicador_StatusLabelWarning; }
        else if (registro.AlarmaComparer === ">=" && registro.Value >= registro.Alarma) { color = colorAlarma; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Indicador_StatusLabelWarning; }
        else if (registro.AlarmaComparer === "<" && registro.Value < registro.Alarma) { color = colorAlarma; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Indicador_StatusLabelWarning; }
        else if (registro.AlarmaComparer === "<=" && registro.Value <= registro.Alarma) { color = colorAlarma; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Indicador_StatusLabelWarning; }
        else {
            color = colorNoAssolit;
            statusLabel = Dictionary.Item_Indicador_StatusLabelNoMeta;
        }
    }
    else {
        color = colorNoAssolit;
        statusLabel = Dictionary.Item_Indicador_StatusLabelNoMeta;
    }

    var row = "";
    row += "<tr>";
    row += "    <td style=\"width:35px;\">";
    row += "        <i title=\"" + statusLabel + "\" class=\"" + icon + "\" style=\"color:" + color + ";\"></i>";
    row += "    </td>";
    row += "    <td align=\"right\" style=\"width:140px;\">";
    row += ToMoneyFormat(registro.Value, 2) + "</td>";
    row += "    <td align=\"center\" style=\"width:90px;\">" + registro.Date + "</td>";
    row += "    <td>" + registro.Comments + "</td>";
    row += "    <td align=\"right\" style=\"width:120px;\">" + registro.MetaComparer + " " + ToMoneyFormat(registro.Meta, 2) + "</td>";
    if (registro.Alarma === null) {
        row += "    <td align=\"right\" style=\"width:120px;\">&nbsp;</td>";
    } else {
        row += "    <td align=\"right\" style=\"width:120px;\">" + registro.AlarmaComparer + " " + alarmaText + "</td>";
    }

    row += "    <td style=\"width:175px;\">" + registro.Responsible.Value + "</td>";
    row += "    <td style=\"width:90px;\">";

    //gtk aquí ocultar botón	
	if (typeof Indicador.EndDate !== "undefined" && Indicador.EndDate !== null) {
		row += "		&nbsp;";
		row += "        &nbsp;";
		row += "        &nbsp;";
    }
	else {
		row += "         <span title=\"" + Dictionary.Common_Edit + "\" class=\"btn btn-xs btn-info\" onclick=\"RecordEdit(" + registro.Id + ");\"><i class=\"icon-edit bigger-120\"></i></span>";
		row += "        &nbsp;";
		row += "        <span title=\"" + Dictionary.Common_Delete + "\" class=\"btn btn-xs btn-danger\" onclick=\"RecordDelete(" + registro.Id + ");\"><i class=\"icon-trash bigger-120\"></i></span>";
	}
    row += "     </td>";
    row += "</tr>";
    $("#IndicadorRegistrosTable").append(row);
    return color;
}

function IndicadorRegistroFilterValidate() {
    $("#ErrorDateMalformedFrom").hide();
    $("#ErrorDateMalformedTo").hide();
    $("#ErrorDate").hide();
    var ok = true;

    if ($("#TxtRecordsFromDate").val() !== "") {
        if (!validateDate($("#TxtRecordsFromDate").val())) {
            ok = false;
            $("#ErrorDateMalformedFrom").show();
        }
    }

    if ($("#TxtRecordsToDate").val() !== "") {
        if (!validateDate($("#TxtRecordsToDate").val())) {
            ok = false;
            $("#ErrorDateMalformedTo").show();
        }
    }

    if (ok === true) {
        if ($("#TxtRecordsFromDate").val() !== "" && $("#TxtRecordsToDate").val()!=="")
        {
            var dateFrom = GetDate($("#TxtRecordsFromDate").val(), "/", true);
            var dateTo = GetDate($("#TxtRecordsToDate").val(), "/", true);

            if (dateFrom > dateTo) {
                ok = false;
                $("#ErrorDate").show();
            }
        }
    }

    return ok;
}

function IndicadorRegistroFilter(exportType) {
    selectedRegistros = [];
    if (IndicadorRegistroFilterValidate() === false) {
        $("#IndicadorRegistrosTable").hide();
        $("#ItemTableError").show();
        $("#ItemTableVoid").hide();
        return false;
    }

    if (typeof exportType !== "undefined") {
        lockOrderList = true;
    }

    $("#IndicadorRegistrosTable").html("");
    $("#IndicadorRegistrosTable").show();
    $("#ItemTableError").hide();
    $("#ItemTableVoid").hide();
    var count = 0;
    for (var x = 0; x < Registros.length; x++) {
        var dateFrom = GetDate($("#TxtRecordsFromDate").val(), "/", false);
        var dateTo = GetDate($("#TxtRecordsToDate").val(), "/", false);

        var show = true;

        if (dateFrom !== null) {
            dateFromRegistro = GetDate(Registros[x].Date, "/", false);
            if (dateFromRegistro < dateFrom) {
                show = false;
            }
        }

        if (dateTo !== null) {
            dateToRegistro = GetDate(Registros[x].Date, "/", false);
            if (dateToRegistro > dateTo) {
                show = false;
            }
        }

        if (show === true) {
            count++;
            Registros[x]["Color"] = RenderRegistroRow(Registros[x]);
            selectedRegistros.push(Registros[x]);
        }
    }
    console.log(Registros);

    $("#NumberCosts").html(count);

    if (count === 0) {
        $("#IndicadorRegistrosTable").hide();
        $("#ItemTableError").hide();
        $("#ItemTableVoid").show();
    }

    if (listOrder === null) {
        listOrder = "th2|DESC";
    }

    console.log(listOrder);
    if (lockOrderList) {
        var th = listOrder.split('|')[0];
        var sort = listOrder.split('|')[1];

        $("#" + th).click();
        if (document.getElementById(th).className.indexOf(sort) === -1) {
            $("#" + th).click();
        }

    }

    lockOrderList = false;
    if (exportType === "PDF") {
        Export("PDF");
    }

    if (exportType === "Excel") {
        Export("Excel");
    }
}

function IndicadorRegistroAll() {
    $("#TxtRecordsFromDate").val("");
    $("#TxtRecordsToDate").val("");
    IndicadorRegistroFilter();
    //$("#BtnRecordShowAll").hide();
    //$("#BtnRecordShowNone").show();
    $("#IndicadorRegistrosTable").show();
    $("#ItemTableError").hide();
    $("#ItemTableVoid").hide();
}

function IndicadorRegistroNone() {
    $("#TxtRecordsFromDate").val("");
    $("#TxtRecordsToDate").val("");
    $("#IndicadorRegistrosTable").html("");
    $("#IndicadorRegistrosTable").hide();
    $("#ItemTableError").hide();
    $("#ItemTableVoid").show();
    $("#BtnRecordShowAll").show();
    $("#BtnRecordShowNone").hide();
}

var selectedRecordId = null;

function AnularPopup() {
    $("#TxtAnularDate").val(FormatDate(new Date(), "/"));
    $("#TxtAnularComments").html("");
    $("#CmbResponsibleAnularRecord").val(user.Employee.Id);
    var dialog = $("#dialogAnular").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Indicador_PopupAnular_Title,
        "width": 600,
        "buttons":
        [
            {
                "id": "BtnAnularSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_Indicador_Btn_Anular,
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
    $("#TxtAnularCommentsLabel").css("color", "#000");
    $("#TxtAnularDateLabel").css("color", "#000");
    $("#CmbResponsibleAnularRecordLabel").css("color", "#000");
    $("#TxtAnularCommentsErrorRequired").hide();
    $("#TxtAnularDateRequired").hide();
    $("#TxtAnularDateMalformed").hide();
    $("#TxtAnularDateMaximumToday").hide();
    $("#CmbResponsibleAnularRecordErrorRequired").hide();

    var ok = true;
    if ($("#TxtAnularComments").val() === "") {
        ok = false;
        document.getElementById("TxtAnularCommentsLabel").css("color", Color.Error);
        document.getElementById("TxtAnularCommentsErrorRequired").show();
    }

    if ($("#TxtAnularDate").val() === "") {
        ok = false;
        $("#TxtAnularDateLabel").css("color", Color.Error);
        $("#TxtAnularDateRequired").show();
    }
    else {
        if (validateDate($("#TxtAnularDate").val()) === false) {
            ok = false;
            $("#TxtAnularDateLabel").css("color", Color.Error);
            $("#TxtAnularDateMalformed").show();
        }
        else {
            var date = GetDate($("#TxtAnularDate").val(), "/", false);
            if (date > new Date()) {
                $("#TxtAnularDateLabel").css("color", Color.Error);
                $("#TxtAnularDateMaximumToday").show();
            }
        }
    }

    if ($("#CmbResponsibleAnularRecord").val() * 1 < 1) {
        ok = false;
        $("#CmbResponsibleAnularRecordLabel").css("color", Color.Error);
        $("#CmbResponsibleAnularRecordErrorRequired").show();
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "indicadorId": Indicador.Id,
        "companyId": Company.Id,
        "reason": $("#TxtAnularComments").val(),
        "responsible": $("#CmbResponsibleAnularRecord").val() * 1,
        "date": GetDate($("#TxtAnularDate").val(), "/"),
        "applicationUserId": user.Id
    };
    $("#dialogAnular").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IndicadorActions.asmx/Anulate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = referrer;
            //AnulateLayout();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function RecordNew() {
    if (typeof Indicador.EndDate !== "undefined" && Indicador.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Indicador_Message_IndicadorClosed, null);
        return false;
    }

    RegistroFormReset();
    RecordEdit(-1);
}

function RecordEdit(id) {
    if (typeof Indicador.EndDate !== "undefined" && Indicador.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Indicador_Message_IndicadorClosedUpdate, null);
        return false;
    }

    selectedRecordId = id * 1;
    FillFormRegistro(selectedRecordId);
    var title = selectedRecordId === -1 ? Dictionary.Item_IndicatorRecord_PopupTitle_Insert : Dictionary.Item_IndicatorRecord_PopupTitle_Update;
    var dialog = $("#dialogNewRecord").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": title,
        "width": 500,
        "buttons":
        [
            {
                "id": "BtnNewRegistroSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + (selectedRecordId > 0 ? Dictionary.Common_Change : Dictionary.Common_Add),
                "class": "btn btn-success btn-xs",
                "click": function () { IndicadorRegistroSave(); }
            },
            {
                "id": "BtnNewRegistroCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]

    });
}

function RecordDelete(id) {
    if (typeof Indicador.EndDate !== "undefined" && Indicador.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Indicador_Message_IndicadorClosedDelete, null);
        return false;
    }

    selectedRecordId = id;
    var registro = RegistroGetById(id);
    if(registro !== null){
        $("#dialogDeleteName").html(Dictionary.Item_Indicador_Popup_DeleteRecord_Message);
    }
    var dialog = $("#dialogDeleteRecord").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    RecordDeleteConfirmed();
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

function RecordDeleteConfirmed() {
    var data = {
        "registroId": selectedRecordId,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    $("#dialogDeleteRecord").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IndicadorActions.asmx/DeleteRegistro",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            var temp = new Array();
            for (var x = 0; x < Registros.length; x++) {
                if (Registros[x].Id !== selectedRecordId) {
                    temp.push(Registros[x]);
                }
            }

            Registros = temp;
            IndicadorRegistroFilter();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function FillFormRegistro(id) {
    if (id > 0) {
        var registro = RegistroGetById(id);
        if (registro !== null) {
            var value = registro.Value.toString();
            if (isNaN(value)) {
                value = value.split('.').join(',');
            }
			if( value === "") { value = "0,00"; }
            $("#TxtRegistroValue").val(ToMoneyFormat(value));
            $("#TxtRecordDate").val(registro.Date);
            $("#TxtRegistroComments").val(registro.Comments);
            $("#CmbResponsibleRecord").val(registro.Responsible.Id);
        }
        else {
            FillFormRegistro(-1);
        }
    }
    else {
        $("#TxtRegistroValue").val("");
        $("#TxtRecordDate").val(FormatDate(new Date(), "/"));
        $("#TxtRegistroComments").val("");
        $("#CmbResponsibleRecord").val(user.Employee.Id);
    }
}

function RegistroGetById(id) {
    for (var x = 0; x < Registros.length; x++) {
        if (Registros[x].Id === id) {
            return Registros[x];
        }
    }

    return null;
}

function RegistroFormReset() {
    $("#TxtRegistroValueLabel").css("color", "#333");
    $("#TxtRegistroValueErrorRequired").hide();
    $("#TxtRecordDateLabel").css("color", "#333");
    $("#TxtRecordDateRequired").hide();
    $("#TxtRecordDateMalformed").hide();
    $("#TxtRecordDateMaximumToday").hide();
    $("#CmbResponsibleRecordLabel").css("color", "#333");
    $("#CmbResponsibleRecordErrorRequired").hide();
    $("#TxtRecordDatePrevious").hide();
}

function ValidateRegistroForm() {
    var ok = true;
    RegistroFormReset();
    if ($("#TxtRegistroValue").val() === "") {
        ok = false;
        $("#TxtRegistroValueLabel").css("color", Color.Error);
        $("#TxtRegistroValueErrorRequired").show();
    }

    if ($("#CmbResponsibleRecord").val() * 1 < 1) {
        ok = false;
        $("#CmbResponsibleRecordLabel").css("color", Color.Error);
        $("#CmbResponsibleRecordErrorRequired").show();
    }

    if ($("#TxtRecordDate").val() === "") {
        ok = false;
        $("#TxtRecordDateLabel").css("color", Color.Error);
        $("#TxtRecordDateRequired").show();
    }
    else if (validateDate($("#TxtRecordDate").val()) === false) {
        ok = false;
        $("#TxtRecordDateLabel").css("color", Color.Error);
        $("#TxtRecordDateMalformed").show();
    }    
    else {
        var date = GetDate($("#TxtRecordDate").val(), "/", false);
        if (date > new Date()) {
            ok = false;
            $("#TxtRecordDateLabel").css("color", Color.Error);
            $("#TxtRecordDateMaximumToday").show();
        } else {
            var IndicadorStartDate = GetDate($("#TxtStartDate").val(), "/", true);
            if (IndicadorStartDate > date) {
                ok = false;
                $("#TxtRecordDateLabel").css("color", Color.Error);
                $("#TxtRecordDatePrevious").show();
            }
        }
    }

    return ok;
}

function IndicadorRegistroSave() {
    if (ValidateRegistroForm() === false) {
        return false;
    }

    var data = {
            "Id": selectedRecordId,
            "CompanyId": Indicador.CompanyId,
            "Indicador": Indicador.Id,
            "Value": StringToNumber($("#TxtRegistroValue").val(), ".", ","),
            "Date": GetDate($("#TxtRecordDate").val(), "/", false),
            "Comments": $("#TxtRegistroComments").val(),
            "MetaComparer": Indicador.MetaComparer,
            "Meta": Indicador.Meta,
            "AlarmaComparer": Indicador.AlarmaComparer,
            "Alarma": Indicador.Alarma,
            "Responsible": $("#CmbResponsibleRecord").val() * 1 ,
        "applicationUserId": ApplicationUser.Id
    };

    console.log(data);

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IndicadorActions.asmx/SaveRegistro",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
                console.log(selectedRecordId, response.d.ReturnValue);
                $("#dialogNewRecord").dialog("close");
                var res;
                eval("res=" + response.d.ReturnValue + ";");
                if (selectedRecordId < 0) {
					console.log("Registro", res);
                    Registros.push(res);
                }
                else {
                    var temp = new Array();
                    for (var x = 0; x < Registros.length; x++) {
                        if (Registros[x].Id === selectedRecordId) {
                            temp.push(res);
                        }
                        else {
                            temp.push(Registros[x]);
                        }
                    }

                    Registros = temp;
                }

                $("#BtnRecordShowAll").click();
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI("****"+jqXHR.responseText);
        }
    });
}

window.chartColors = {
    "red": "rgb(255, 99, 132)",
    "orange": "rgb(255, 159, 64)",
    "yellow": "rgb(255, 205, 86)",
    "green": "rgb(75, 192, 192)",
    "blue": "rgb(54, 162, 235)",
    "purple": "rgb(153, 102, 255)",
    "grey": "rgb(201, 203, 207)"
};

function DrawGraphics(stop) {
    if (Registros.length === 0) {
        $("#GraphicsNoData").show();
    }
    else {
        $("#GraphicsNoData").hide();
        console.log("DrawGraphics", firstChart);
        if (firstChart === false) {
            // return;
        }

        firstChart = false;

        // ---------- BARRAS
        var barOptions = {
            "scaleBeginAtZero": true,
            "scaleShowGridLines": true,
            "scaleGridLineColor": "rgba(0,0,0,.05)",
            "scaleGridLineWidth": 1,
            "barShowStroke": false,
            "barStrokeWidth": 1,
            "barValueSpacing": 5,
            "barDatasetSpacing": 10,
            "responsive": true,
            "legendTemplate": "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>",
            "legend": {
                "display": true,
                "labels": {
                    "fontColor": "rgb(255, 99, 132)"
                }
            }
        };

        var labels = new Array();
        var values = new Array();
        var metas = new Array();
        var alarmas = new Array();

        var lastValue = null;
        var lastMeta = null;
        var lastAlarm = null;
        var lastLabel = "";


        selectedRegistros.sort(RegistrosDateSort);
        var colors = [];
        for (var x = 0; x < selectedRegistros.length; x++) {
            labels.push(selectedRegistros[x].Date);
            values.push(selectedRegistros[x].Value);
            metas.push(selectedRegistros[x].Meta);
            alarmas.push(selectedRegistros[x].Alarma);
            colors.push(selectedRegistros[x].Color);

            lastValue = selectedRegistros[x].Value * 1;
            lastMeta = selectedRegistros[x].Meta * 1;
            lastAlarm = selectedRegistros[x].Alarma * 1;
            lastLabel = selectedRegistros[x].Date;
        }

        while (labels.length < 6) {
            labels.push("");
        }

        console.log(alarmas);
        console.log(metas);

        var overlayData = {
            "colors": colors,
            "labels": labels,
            "datasets": [ {
                    "label": "Meta",
                    "type": "line",
                    "fillColor": "rgba(119,226,152,0)",
                    "strokeColor": "rgba(59,183,38,0.8)",
                    "data": metas
                },
                {
                    "label": "Valor",
                    "fillColor": "#275b89",
                    "strokeColor": "rgba(77,110,240,0.8)",
                    "data": values
                },
                {
                    "label": "Alarma",
                    "type": "line",
                    "fillColor": "rgba(244,210,210,0)",
                    "strokeColor": "rgba(216,33,0,0.8)",
                    "data": alarmas
                }
            ]
        };

        $("#barChartDiv").html("");
        this.div = document.getElementById("barChartDiv");
        this.div.style.height = "800px";
        this.chartCanvas = document.createElement("canvas");
        this.div.appendChild(this.chartCanvas);
        this.chartCanvas.style.width = $("#barChartDiv").width() + "px";
        this.chartCanvas.width = $("#barChartDiv").width();
        this.chartCanvas.height = 500;
        this.chartCanvas.id = "canvas";

        this.ctx = this.chartCanvas.getContext("2d");
        this.chart = new Chart(this.ctx).Overlay(overlayData, {
            populateSparseData: true,
            overlayBars: false,
            datasetFill: true
        });
        this.div.style.display = "none";

        setTimeout(function () {
            this.div.style.display = "block";
            if (stop !== true) {
                if ($("#canvas").css("width") === "0px") { DrawGraphics(true); }
            }
        }.bind(this), 100);

        /*console.log("gauge");

        var maxValue = lastAlarm;
        if (lastValue > maxValue) { maxValue = lastValue; }
        if (lastMeta > maxValue) { maxValue = lastMeta; }

        $("#circularGaugeContainer").dxCircularGauge({
            "width":10,
            "rangeContainer": {
                "offset": 10,
                "width": 20,
                "ranges": [
                    { "startValue": 0, "endValue": lastAlarm, "color": "#f00" },
                    { "startValue": lastAlarm, "endValue": lastMeta, "color": "#fa0" },
                    { "startValue": lastMeta, "endValue": maxValue, "color": "#0f0" }
                ]
            },
            "scale": {
                "startValue": 0,
                "endValue": maxValue,
                "majorTick": { tickInterval: lastMeta / 4 }
            },
            "title": {
                "text": Indicador.Unidad.Description,
                "subtitle": "hola",
                "position": "left"
            },
            "tooltip": {
                "enabled": true,
                "customizeText": function (arg) {
                    return arg.valueText * 1;
                }
            },
            "subvalueIndicator": {
                "type": "textCloud",
                "text": {
                    "customizeText": function (arg) {
                        return arg.valueText.split(',').join('') * 1;
                    }
                }
            },
            "value": lastValue,
            "subvalues": [lastValue]
        });*/
    }
}

function RegistrosDateSort(a, b) {
    return GetDate(a.Date, "/", false) - GetDate(b.Date, "/", false);
}

function UnitChanged(sender) {
    var unitId = sender.parentNode.parentNode.parentNode.id;
    $("#CmbUnidad").val(unitId);
    $("#dialogUnits").dialog("close");
}

function UnitUpdate(sender) {
    $("#TxtUnitsNewNameErrorDuplicated").hide();
    $("#TxtUnitsNewNameErrorRequired").hide();
    $("#TxtUnitsNewName").val("");
    var unidad = null;
    if (sender === null) {
        UnitSelected = -1;
    }
    else {
        UnitSelected = sender.parentNode.parentNode.parentNode.id * 1;
        unidad = UnitGetById(UnitSelected * 1);
        console.log(unidad);
    }

    $("#TxtUnitsNewName").attr("placeholder", Dictionary.Item_Unidad);

    var html = $("#TxtUnitsNewName").parent().html();
    console.log(html);
    $("#TxtUnitsNewName").parent().html(Dictionary.Item_Unidad + ":&nbsp;&nbsp;" + $("#TxtUnitsNewName").parent().html().split('&nbsp;&nbsp;')[1]);
    if (unidad !== null) {
        $("#TxtUnitsNewName").val(unidad.Description);
    }

    $(".dialogUnits").css("z-index", 1000);
    document.getElementById("UnitsInsertDialog").style.zIndex = 1150;
    var dialog = $("#UnitsInsertDialog").removeClass("hide").dialog({
        "id": "UnitsInsertDialog",
        "resizable": false,
        "modal": true,
        "title": UnitSelected > 0 ? Dictionary.Common_Modify : Dictionary.Common_Add,
        "title_html": true,
        "width": 600,
        "buttons": [
            {
                "id": "UnitSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    UnitSave();
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $("#UnitsInsertDialog").dialog("close"); }
            }
        ]
    });
}

function UnitSave() {
    var ok = true;
    var description = $("#TxtUnitsNewName").val();
    $("#TxtUnitsNewNameErrorRequired").hide();
    $("#TxtUnitsNewNameErrorDuplicated").hide();

    if ($("#TxtUnitsNewName").val() === "") {
        ok = false;
        $("#TxtUnitsNewNameErrorRequired").show();
    }

    for (var x = 0; x < Unidades.length; x++) {
        var actual = Unidades[x];
        if (actual.Active === true) {
            if (actual.Id !== UnitSelected && actual.Description.toUpperCase() === description.toUpperCase()) {
                ok = false;
                $("#TxtUnitsNewNameErrorDuplicated").show();
            }
        }
    }

    if (ok === false) {
        return false;
    }

    var webMethod = "/Async/UnidadActions.asmx/Update";
    data = {
        "unidadId": UnitSelected,
        "description": description,
        "companyId": Company.Id,
        "userId": ApplicationUser.Id
    };

    if (UnitSelected < 0) {
        webMethod = "/Async/UnidadActions.asmx/Insert";
        var data = {
            "unidadId": -1,
            "description": $("#TxtUnitsNewName").val(),
            "companyId": Company.Id,
            "userId": ApplicationUser.Id
        };
    }

    // Insert(string description, int companyId, int userId)
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
                var res;
                eval("res=" + response.d.ReturnValue + ";");
                console.log(UnitSelected, res);
                if (UnitSelected < 0) {
                    UnitSelected = res.Id;
                    Unidades.push(res);
                }
                else {
                    var temp = new Array();
                    for (var x = 0; x < Unidades.length; x++) {
                        if (Unidades[x].Id === UnitSelected) {
                            temp.push(res);
                        }
                        else {
                            temp.push(Unidades[x]);
                        }
                    }

                    Unidades = temp;
                }

                UnitsRenderPopup();
                FillComboUnidades();
                $("#UnitsInsertDialog").dialog("close");
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

// Bar popup for bar item Units
function ShowUnitsBarPopup() {
    UnitsRenderPopup();
    var dialog = $("#dialogUnits").removeClass("hide").dialog({
        "id": "dialogUnits",
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Indicaddor_Popup_Units,
        "title_html": true,
        "width": 600,
        "buttons": [
            {
                "id": "BtnEquipmentScaleDivisionSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    UnitUpdate(null);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function UnitsRenderPopup() {
    VoidTable("SelectableUnits");
    Unidades.sort(CompareUnits);
    var target = document.getElementById("SelectableUnits");
    for (var x = 0; x < Unidades.length; x++) {
        UnitsPopupRow(Unidades[x], target)
    }
}

function CompareUnits(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function UnitsPopupRow(item, target) {
    console.log(item);
    if (item.Active === false) return;
    var tr = document.createElement("tr");
    tr.id = item.Id;
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    if ($("#CmbUnidad").val() * 1 === item.Id) {
        td1.style.fontWeight = "bold";
    }
    td1.appendChild(document.createTextNode(item.Description));

    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if ($("#CmbUnidad").val() * 1 === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { UnitChanged(this); };
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

    if (item.Id < 0) {
        span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else {
        span2.onclick = function () { UnitUpdate(this); };
    }

    var span3 = document.createElement("span");
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);

    if ($("#CmbUnidad").val() * 1 === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else if (item.Id < 0) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else {
        span3.onclick = function () { UnitDelete(this); };
    }

    div.appendChild(document.createTextNode(" "));
    div.appendChild(span3);
    td2.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}

function UnitDelete(sender) {
    UnitSelected = sender.parentNode.parentNode.parentNode.id * 1;
    var unidad = UnitGetById(UnitSelected);
    $("#UnitsName").html(unidad.Description);
    ProviderSelected = UnitSelected;
    var dialog = $("#UnitsDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        "buttons": [
            {
                "id": "UnitDeleteBtnOK",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    UnitDeleteConfirmed();
                }
            },
            {
                "id": "UnitDeleteBtnCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function UnitDeleteConfirmed() {
    var data = {
        "unidadId": UnitSelected,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#UnitsDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/UnidadActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            var temp = new Array();
            for (var x = 0; x < Unidades.length; x++) {
                if (Unidades[x].Id !== UnitSelected) {
                    temp.push(Unidades[x]);
                }
            }

            Unidades = temp;
            UnitsRenderPopup();
            FillComboUnidades();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function UnitGetById(id) {
    for (var x = 0; x < Unidades.length; x++) {
        if (Unidades[x].Id === id) {
            return Unidades[x];
        }
    }

    return null;
}

function FillComboUnidades() {
    var unidadId = $("#CmbUnidad").val() * 1;
    $("#CmbUnidad").html("");
    var target = document.getElementById("CmbUnidad");

    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createElement(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);
    Unidades.sort(CompareUnits);
    for (var x = 0; x < Unidades.length; x++) {
        if (Unidades[x].Active === true) {
            var option = document.createElement("option");
            option.value = Unidades[x].Id;
            option.appendChild(document.createTextNode(Unidades[x].Description));
            if (Unidades[x].Id === unidadId) {
                option.selected = true;
            }

            target.appendChild(option);
        }
    }
}

function Export(fileType) {
    console.log("Export", fileType);
    var data = {
        "dateFrom": GetDate($("#TxtRecordsFromDate").val(), "/", false),
        "dateTo": GetDate($("#TxtRecordsToDate").val(), "/", false),
        "companyId": Company.Id,
        "indicadorId": Indicador.Id,
        "indicadorName": Indicador.Description,
        "listOrder": listOrder
    };

    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/IndicadorRecords.aspx/" + fileType,
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
            document.body.removeChild(link);
            $("#download").trigger("click");
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog("close");
        },
        "error": function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}

function AnulateLayout() {
    $("#BtnRestaurar").hide();
    if (Indicador.EndDate !== null) {
        var message = "<div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_Indicador_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_IndicatorRecord_FieldLabel_Reason + ": <strong>" + Indicador.EndReason + "</strong><br />";
        message += "        " + Dictionary.Item_IndicatorRecord_FieldLabel_Date + ": <strong>" + Indicador.EndDate + "</strong><br />";
        message += "        " + Dictionary.Item_Indicador_Field_EndResponsible + ": <strong>" + Indicador.EndResponsible.Value + "</strong>";
        message += "    </p>";
        message += "</div>";
        //$("#home").append(message);
        $("#oldFormFooter").before(message);
        $("#BtnAnular").hide();
        $("#BtnRestaurar").show();
    }
    else {
        $("#DivAnulateMessage").hide();
        $("#BtnAnular").show();
    }
}

function Restore() {
    var data = {
        "indicadorId": Indicador.Id,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IndicadorActions.asmx/Restore",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            Indicador.EndDate = null;
            AnulateLayout();
            EnableLayout();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function DisableLayout() {
    $("input").attr("disabled", "disabled");
    $("select").attr("disabled", "disabled");
    $("textarea").attr("disabled", "disabled");
    $("#BtnUnitsBAR").hide();
    $("#RActionYes").attr("disabled", "disabled");
    $("#RActionNo").attr("disabled", "disabled");

    // Mantener activos los inputs de "Reobrir"
    $("#TxtAnularComments").removeAttr("disabled");
    $("#TxtAnularDate").removeAttr("disabled");
    $("#CmbResponsibleAnularRecord").removeAttr("disabled");

    // los filtros
    $("#TxtRecordsFromDate").removeAttr("disabled");
    $("#TxtRecordsToDate").removeAttr("disabled");
}

function EnableLayout() {
    $("input").removeAttr("disabled");
    $("select").removeAttr("disabled");
    $("textarea").removeAttr("disabled");
    $("#RActionYes").removeAttr("disabled");
    $("#RActionNo").removeAttr("disabled");
    $("#BtnUnitsBAR").show();
}

function RenderTableHistorico() {
    $("#ObjetivoHistoricoTable").html("");
    for (var x = 0; x < Historic.length; x++) {
        RenderHistoricoRow(Historic[x]);
    }

    $("#NumberHistoric").html(Historic.length);
}

function RenderHistoricoRow(data) {
    var target = document.getElementById("ObjetivoHistoricoTable");
    var tr = document.createElement("TR");
    var tdAction = document.createElement("TD");
    var tdDate = document.createElement("TD");
    var tdReason = document.createElement("TD");
    var tdEmployee = document.createElement("TD");

    tdAction.style.width = "100px";
    tdDate.style.width = "95px";
    tdEmployee.style.width = "240px";

    var actionText = "Anular";
    var reason = data.Reason;
    if (data.Reason === "Restore") {
        actionText = "Restaurar";
        reason = "";
    }

    tdAction.appendChild(document.createTextNode(actionText));
    tdDate.appendChild(document.createTextNode(data.Date));
    tdReason.appendChild(document.createTextNode(reason));
    tdEmployee.appendChild(document.createTextNode(data.Employee.Value));

    tr.appendChild(tdAction);
    tr.appendChild(tdDate);
    tr.appendChild(tdReason);
    tr.appendChild(tdEmployee);

    target.appendChild(tr);
}

function Compute() {
    var alarmComparer = $("#CmbAlarmaComparer").val();
    var metaComparer = $("#CmbMetaComparer").val();
    $("#CmbAlarmaComparer").html("");
    $("#CmbAlarmaComparer").attr("disabled", "disabled");

    if (metaComparer.indexOf("gt") !== -1) {
        res = "<option value=\"\">Seleccionar</option>";
        res += "<option value=\"lt\">" + Dictionary.Common_Comparer_lt + " (&lt;)</option>";
        res += "<option value=\"eqlt\">" + Dictionary.Common_Comparer_eqlt + "(&lt;=)</option>";
        $("#CmbAlarmaComparer").html(res);
        $("#CmbAlarmaComparer").removeAttr("disabled");
        $("#CmbAlarmaComparer").val(alarmComparer);
    }

    if (metaComparer.indexOf("lt") !== -1) {
        res = "<option value=\"\">Seleccionar</option>";
        res += "<option value=\"gt\">" + Dictionary.Common_Comparer_gt + " (&gt;)</option>";
        res += "<option value=\"eqgt\">" + Dictionary.Common_Comparer_eqgt + "(&gt;=)</option>";
        $("#CmbAlarmaComparer").html(res);
        $("#CmbAlarmaComparer").removeAttr("disabled");
        $("#CmbAlarmaComparer").val(alarmComparer);
    }

    if (alarmComparer === "") {
        $("#TxtAlarma").attr("disabled", "disabled");
        $("#TxtAlarma").val("");
        document.getElementById("celldanger").className = "btn-warning";
        $("#celldanger").html("");
        $("#celldangerLabel").html("");
    }
    else {
        $("#TxtAlarma").removeAttr("disabled");
        document.getElementById("celldanger").className = "btn-danger";

        $("#celldanger").html($("#CmbAlarmaComparer option:selected").text() + " " + $("#TxtAlarma").val());
        $("#celldangerLabel").html("Alarma");
    }

    $("#cellsuccess").html($("#CmbMetaComparer option:selected").text() + " " + $("#TxtMeta").val());
}

function PrintData() {
    window.open("/export/IndicadorExportData.aspx?id=" + IndicadorId + "&companyId=" + Company.Id);
    return false;
}