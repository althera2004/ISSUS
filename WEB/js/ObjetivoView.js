var lockOrderList = false;
var firstChart = true;
var CloseRequired = false;
var recordsGraph = [];
var preiodicidadIndicador = null;

window.onload = function () {
    if (ItemData.Id < 1) {
        $("#TxtFechaCierrePrevista").val("");
        document.getElementById("Contentholder1_RVinculatedNo").checked = true;
    }
    else {
        $("#TxtName").val(ItemData.Name);
        $("#TxtDescription").val(ItemData.Description);
        $("#TxtMethodology").val(ItemData.Methodology);
        $("#TxtResources").val(ItemData.Resources);
        $("#TxtPeriodicity").val(ItemData.RevisionId);
        $("#TxtNotes").val(ItemData.Notes);
        $("#TxtFechaAlta").val(ItemData.StartDate);
        $("#TxtFechaCierrePrevista").val(ItemData.PreviewEndDate);
        $("#TxtFechaCierreReal").val(ItemData.EndDate);
        $("#CmbMetaComparer").val(ItemData.MetaComparer);
        $("#TxtMeta").val(ItemData.Meta);
        if (ItemData.VinculatedToIndicator === true) {
            document.getElementById("Contentholder1_RVinculatedYes").checked = true;
        }
        if (ItemData.VinculatedToIndicator === false) {
            document.getElementById("Contentholder1_RVinculatedNo").checked = true;
        }

        $("#BtnAnular").on("click", AnularPopup);
        $("#BtnRestaurar").on("click", Restore);

        $("#TxtActionsFromDate").on("change", RenderActionsTable);
        $("#TxtActionsToDate").on("change", RenderActionsTable);
        $("#TxtRecordsFromDate").on("change", ObjetivoRegistroFilter);
        $("#TxtRecordsToDate").on("change", ObjetivoRegistroFilter);
        RenderActionsTable();
        ObjetivoRegistroFilter();

        $("#BtnActionsNew").on("click", ActionNew);
    }

    var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);

    $("#BtnRecordShowNone").on("click", ObjetivoRegistroNone);
    $("#BtnRecordShowAll").on("click", ObjetivoRegistroAll);
    $("#BtnRecordFilter").on("click", ObjetivoRegistroFilter);
    $("#BtnRecordNew").on("click", RecordNew);
    $("#BtnSave").on("click", Save);
    $("#BtnCancel").on("click", function (e) { document.location = referrer; });

    $("#Contentholder1_RVinculatedYes").on("change", IndicatorVinculatedLayout);
    $("#Contentholder1_RVinculatedNo").on("change", IndicatorVinculatedLayout);
    $("#CmbIndicador").on("change", CmbIndicadorChanged);
    IndicatorVinculatedLayout();

    if (Registros.length == 0) {
        $("#ObjetivoRegistrosTable").hide();
        $("#ItemTableVoid").show();
    }

    Resize();
    ObjetivoRegistroFilter();

    DrawGraphics();
    $("#Tabgraphics").on("click", DrawGraphics);

    if (ItemData.VinculatedToIndicator === false) {
        $("#TxtMeta").val(ItemData.Meta);
        $("#CmbMetaComparer").val(ItemData.MetaComparer);
    }

    $("#TxtRecordDateMaximumToday").after("<span class=\"ErrorMessage\" id=\"TxtRecordDateMinimum\" style=\"display:none\">" + Dictionary.Item_Objetivo_Error_DateMinimum + "</span>");
    IndicatorVinculatedLayout();
    AnulateLayout();

    $("#CmbResponsible").chosen();
    $("#CmbMetaComparer").chosen();

    if (ItemData.EndDate !== null) {
        DisableLayout();
    }
	
	//gtk aquí ocultar botón
	if (ItemData.EndDate !== null) {
        $("#BtnRecordNew").hide();
    }

    $("#CmbResponsible").on("change", function () { WarningEmployeeNoUserCheck($("#CmbResponsible").val() * 1, Employees, this); });
    $("#CmbResponsibleRecord").on("change", function () { WarningEmployeeNoUserCheck($("#CmbResponsibleRecord").val() * 1, Employees, this); });

    RenderTableHistorico();
}

window.onresize = function () { Resize(); }

function Resize() {
    var listTable = document.getElementById("ListDataDiv");
    var histTable = document.getElementById("ListDataDivHistorico");
    var acctTable = document.getElementById("ListDataDivActions");
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 494) + "px";
    histTable.style.height = (containerHeight - 370) + "px";
    acctTable.style.height = (containerHeight - 451) + "px";
}

function IndicatorVinculatedLayout() {
    if (document.getElementById("Contentholder1_RVinculatedNo").checked === true) {
        $("#CmbIndicadorLabel").hide();
        $("#DivCmbIndicador").hide();
        $("#CmbIndicador").val(0);
        $("#CmbMetaLabel").show();
        $("#DivCmbMetaComparer").show();
        $("#RecordListTitle").html(Dictionary.Item_Objetivo_Tab_Records);
        //$("#DivIndicadorRecordsMessage").hide();
        $("#TxtPeriodicity").removeAttr("disabled");
		$("#Label1").html(Dictionary.Common_LabelDays);
    }
    else {
        $("#CmbIndicadorLabel").show();
        $("#DivCmbIndicador").show();
        $("#CmbIndicador").chosen();
        $("#CmbMetaLabel").hide();
        $("#DivCmbMetaComparer").hide();
		$("#Label1").html(Dictionary.Item_Objetivo_CustomLabelDays);
        $("#RecordListTitle").html(Dictionary.Item_Objetivo_Tab_RecordsFromIndicator + " <strong><i>&quot;" + IndicadorName + "&quot;</i></strong>");
        //$("#DivIndicadorRecordsMessage").show();
        $("#TxtPeriodicity").attr("disabled", "disabled");
        if (preiodicidadIndicador === null) {
            $("#TxtPeriodicity").val("");
        }
        else {
            $("#TxtPeriodicity").val(preiodicidadIndicador);
        }
    }
}

function GetPeriodicityByIndicadorId(indicadorId) {
    for (var x = 0; x < Periodicidades.length; x++) {
        if (Periodicidades[x].Id === indicadorId) {
            return Periodicidades[x].Periodicity;
        }
    }
    return null;
}

function CmbIndicadorChanged() {
    console.log("CmbIndicadorChanged");
    preiodicidadIndicador = GetPeriodicityByIndicadorId($("#CmbIndicador").val() * 1);
    if (preiodicidadIndicador === null) {
        $("#TxtPeriodicity").val("");
    }
    else {
        $("#TxtPeriodicity").val(preiodicidadIndicador);
    }
}

var newObjetivo = ItemData.Id < 1;
function Save(goAction) {
    var validationResult = Validate();
    if (validationResult != "") {
        warningInfoUI(validationResult, null, 300);
        return false;
    }

    var indicatorId = $("#CmbIndicador").val() * 1;
    var vinculatedToIndicator = indicatorId > 0;

    var meta = null;
    if ($("#TxtMeta").val() !== "") {
        meta = StringToNumberNullable($("#TxtMeta").val(), ".", ",");
    }

    var metaComparer = null;
    if ($("#CmbMetaComparer").val() !== "") {
        metaComparer = $("#CmbMetaComparer").val();
    }

    var data = {
        "objetivo": {
            "Id": ItemData.Id,
            "CompanyId": Company.Id,
            "Name": $("#TxtName").val(),
            "Description": $("#TxtDescription").val(),
            "Methodology": $("#TxtMetodologia").val(),
            "Resources": $("#TxtRecursos").val(),
            "Notes": $("#TxtNotes").val(),
            "VinculatedToIndicator": vinculatedToIndicator,
            "IndicatorId": indicatorId,
            "Responsible": { "Id": $("#CmbResponsible").val() * 1, "Value": "", "Active": false },
            "EndResponsible": { "Id": $("#CmbEndResponsible").val() * 1, "Value": "", "Active": false },
            "StartDate": GetDate($("#TxtFechaAlta").val(), "/", true),
            "PreviewEndDate": GetDate($("#TxtFechaCierrePrevista").val(), "/", false),
            "EndDate": GetDate($("#TxtFechaCierreReal").val(), "/", false),
            "RevisionId": $("#TxtPeriodicity").val() * 1,
            "MetaComparer": metaComparer,
            "Meta": meta,
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
        "url": "/Async/ObjetivoActions.asmx/Save",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            console.log(response.d);
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
                if (newObjetivo === true) {
                    document.location = "ObjetivoView.aspx?id=" + response.d.MessageError;
                }
                else {
                    if (typeof goAction === "undefined") {
                        document.location = referrer;
                    }
                    else {
                        document.location = "/ActionView.aspx?id=-1&o=" + ItemData.Id;
                    }
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
    document.getElementById("TxtNameLabel").style.color = "#000";
    document.getElementById("TxtNameErrorRequired").style.display = "none";
    document.getElementById("TxtNameErrorDuplicated").style.display = "none";
    document.getElementById("TxtDescriptionLabel").style.color = "#000";
    document.getElementById("TxtDescriptionErrorRequired").style.display = "none";
    document.getElementById("TxtFechaAltaLabel").style.color = "#000";
    document.getElementById("TxtFechaAltaErrorRequired").style.display = "none";
    document.getElementById("TxtFechaAltaDateMalformed").style.display = "none";
    document.getElementById("TxtFechaCierrePrevistaLabel").style.color = "#000";
    document.getElementById("TxtFechaCierrePrevistaErrorRequired").style.display = "none";
    document.getElementById("TxtFechaCierrePrevistaDateMalformed").style.display = "none";
    document.getElementById("TxtFechaCierrePrevistaCrossDate").style.display = "none";
    document.getElementById("CmbResponsibleLabel").style.color = "#000";
    document.getElementById("CmbResponsibleErrorRequired").style.display = "none";
    document.getElementById("CmbIndicadorLabel").style.color = "#000";
    document.getElementById("CmbIndicadorErrorRequired").style.display = "none";
    /*document.getElementById("TxtFechaCierreRealLabel").style.color = "#000";
    document.getElementById("TxtFechaCierreRealErrorRequired").style.display = "none";
    document.getElementById("TxtFechaCierreRealDateMalformed").style.display = "none";
    document.getElementById("TxtFechaCierreRealCrossDate").style.display = "none";
    document.getElementById("CmbEndResponsibleLabel").style.color = "#000;"
    document.getElementById("CmbEndResponsibleErrorRequired").style.display = "none";*/
    document.getElementById("TxtPeriodicityLabel").style.color = "#000;"
    document.getElementById("TxtPeriodicityErrorRequired").style.display = "none";

    if ($("#TxtName").val() === "") {
        ok = false;
        document.getElementById("TxtNameLabel").style.color = "#f00";
        document.getElementById("TxtNameErrorRequired").style.display = "";
    }
    else {
        if (ObjetivoExists($("#TxtName").val())) {
            document.getElementById("TxtNameLabel").style.color = "#f00";
            document.getElementById("TxtNameErrorDuplicated").style.display = "";
        }
    }

    if ($("#TxtPeriodicity").val() * 1 === 0) {
        ok = false;
        document.getElementById("TxtPeriodicityLabel").style.color = "#f00";
        document.getElementById("TxtPeriodicityErrorRequired").style.display = "";
    }

    if ($("#TxtDescription").val() === "") {
        ok = false;
        document.getElementById("TxtDescriptionLabel").style.color = "#f00";
        document.getElementById("TxtDescriptionErrorRequired").style.display = "";
    }

    if ($("#TxtFechaAlta").val() === "") {
        ok = false;
        document.getElementById("TxtFechaAltaLabel").style.color = "#f00";
        document.getElementById("TxtFechaAltaErrorRequired").style.display = "";
    }
    else {
        if (validateDate($("#TxtFechaAlta").val()) === false) {
            document.getElementById("TxtFechaAltaLabel").style.color = "#f00";
            document.getElementById("TxtFechaAltaDateMalformed").style.display = "";
        }
    }

    if ($("#TxtFechaCierrePrevista").val() === "") {
        ok = false;
        document.getElementById("TxtFechaCierrePrevistaLabel").style.color = "#f00";
        document.getElementById("TxtFechaCierrePrevistaErrorRequired").style.display = "";
    }
    else {
        if (validateDate($("#TxtFechaCierrePrevista").val()) === false) {
            document.getElementById("TxtFechaCierrePrevistaLabel").style.color = "#f00";
            document.getElementById("TxtFechaCierrePrevistaDateMalformed").style.display = "";
        }
    }

    if ($("#CmbResponsible").val() * 1 < 1) {
        ok = false;
        document.getElementById("CmbResponsibleLabel").style.color = "#f00";
        document.getElementById("CmbResponsibleErrorRequired").style.display = "";
    }

    if (document.getElementById("Contentholder1_RVinculatedYes").checked === true && $("#CmbIndicador").val() * 1 < 1) {
        ok = false;
        document.getElementById("CmbIndicadorLabel").style.color = "#f00";
        document.getElementById("CmbIndicadorErrorRequired").style.display = "";
    }

    if ($("#TxtFechaCierrePrevista").val() !== "" && $("#TxtFechaAlta").val() !== "") {
        var inicio = GetDate($("#TxtFechaAlta").val(), "/", true);
        var previsto = GetDate($("#TxtFechaCierrePrevista").val(), "/", true);
        if (previsto < inicio) {
            ok = false;
            document.getElementById("TxtFechaCierrePrevistaLabel").style.color = "#f00";
            document.getElementById("TxtFechaCierrePrevistaCrossDate").style.display = "";
        }
    }

    /*if ($("#TxtFechaCierreReal").val() !== "") {
        if (validateDate($("#TxtFechaCierreReal").val()) === false) {
            ok = false;
            document.getElementById("TxtFechaCierreRealLabel").style.color = "#f00";
            document.getElementById("TxtFechaCierreRealDateMalformed").style.display = "";
        }
        else {
            var inicio = GetDate($("#TxtFechaAlta").val(), "/", true);
            var previsto = GetDate($("#TxtFechaCierreReal").val(), "/", true);
            if (previsto < inicio) {
                ok = false;
                document.getElementById("TxtFechaCierreRealLabel").style.color = "#f00";
                document.getElementById("TxtFechaCierreRealCrossDate").style.display = "";
            }
        }
    }*/

    if (CloseRequired === true) {
        if ($("#CmbEndResponsible").val() * 1 < 1) {
            ok = false;
            document.getElementById("CmbEndResponsibleLabel").style.color = "#f00;"
            document.getElementById("CmbEndResponsibleErrorRequired").style.display = "";
        }

        if ($("#TxtFechaCierreReal").val() === "") {
            ok = false;
            document.getElementById("TxtFechaCierreRealLabel").style.color = "#f00";
            document.getElementById("TxtFechaCierreRealErrorRequired").style.display = "";
        }
    }

    if (ok === false) {
        return Dictionary.Common_Form_Errors;
    }

    return "";
}

function ObjetivoExists(name) {
    name = name.toUpperCase();
    for (var x = 0; x < Objetivos.lenght; x++) {
        if (Objetivos[x].Description.toUpperCase === name) {
            return true;
        }
    }

    return false;
}

// --- Registros


function RenderRegistroRow(registro) {
    var color = "#dc8475";
    var statusLabel = Dictionary.Item_Objetivo_StatusLabelWithoutMeta;
    var icon = "icon-circle bigger-110";
    var metaText = ToMoneyFormat(registro.Meta, 2);
    if (registro.Meta === null) {
        color = "#444";
        metaText = "";
    }
    else if (registro.MetaComparer === "=" && registro.Value === registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === ">" && registro.Value > registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === ">=" && registro.Value >= registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === "<" && registro.Value < registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === "<=" && registro.Value <= registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === ">" && registro.Value > registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === ">=" && registro.Value >= registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === "<" && registro.Value < registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.MetaComparer === "<=" && registro.Value <= registro.Meta) { color = "#a5ca9f"; statusLabel = Dictionary.Item_Objetivo_StatusLabelMeta; }
    else if (registro.Alarma !== null) {
        if (registro.AlarmaComparer === ">" && registro.Value > registro.Alarma) { color = "#ffc97d"; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Objetivo_StatusLabelWarning; }
        else if (registro.AlarmaComparer === ">=" && registro.Value >= registro.Alarma) { color = "#ffc97d"; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Objetivo_StatusLabelWarning; }
        else if (registro.AlarmaComparer === "<" && registro.Value < registro.Alarma) { color = "#ffc97d"; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Objetivo_StatusLabelWarning; }
        else if (registro.AlarmaComparer === "<=" && registro.Value <= registro.Alarma) { color = "#ffc97d"; icon = "ace-icon fa icon-circle bigger-110"; statusLabel = Dictionary.Item_Objetivo_StatusLabelWarning; }
        else {
            color = "#dc8475";
            statusLabel = Dictionary.Item_Objetivo_StatusLabelNoMeta;
        }
    }
    else {
        color = "#dc8475";
        statusLabel = Dictionary.Item_Objetivo_StatusLabelNoMeta;
    }

    var responsibleName = registro.Responsible.Value;
    if (responsibleName === "") {
        var employee = EmployeeGetById(registro.Responsible.Id);
        if(employee !== null){
            responsibleName = employee.FullName;
        }
    }

    var row = "";
    row += "<tr>";
    row += "    <td style=\"width:35px;\">";
    row += "        <i title=\"" + statusLabel + "\" class=\"" + icon + "\" style=\"color:" + color + ";\"></i>";
    row += "    </td>";
    // row += "    <td><a href=\"IndicadorView.aspx?id=" + registro.Indicador.Id + "\">" + registro.Indicador.Name + "</a></td>";
    row += "    <td align=\"right\" style=\"width:90px;\">" + ToMoneyFormat(registro.Value,2) + "</td>";
    row += "    <td align=\"center\" style=\"width:90px;\">" + registro.Date + "</td>";
    row += "    <td>" + registro.Comments + "</td>";
    row += "    <td align=\"right\" style=\"width:120px;\">" + registro.MetaComparer + " " + metaText + "</td>";
    //row += "    <td align=\"right\" style=\"width:120px;\">" + registro.AlarmaComparer + " " + ToMoneyFormat(registro.Alarma,2) + "</td>";
    row += "    <td style=\"width:175px;\">" + responsibleName + "</td>";
    row += "    <td style=\"width:90px;\">";
	
	//gtk aquí ocultar botón
	if (ItemData.EndDate !== null) {
        row += "        &nbsp;";
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
    $("#ObjetivoRegistrosTable").append(row);
}

function ObjetivoRegistroFilterValidate() {
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

    if (ok == true) {
        if ($("#TxtRecordsFromDate").val() !== "" && $("#TxtRecordsToDate").val() !== "") {
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

function ObjetivoRegistroFilter(exportType) {
    console.log("ObjetivoRegistroFilter", exportType)
    if (ObjetivoRegistroFilterValidate() === false) {
        $("#ObjetivoRegistrosTable").hide();
        $("#ItemTableError").show();
        $("#ItemTableVoid").hide();
        return false;
    }

    if (typeof exportType !== "undefined") {
        lockOrderList = true;
    }

    $("#ObjetivoRegistrosTable").html("");
    $("#ObjetivoRegistrosTable").show();
    $("#ItemTableError").hide();
    $("#ItemTableVoid").hide();
    var count = 0;

    // Poner fecha real en los registros
    for (var x = 0; x < Registros.length; x++) {
        Registros[x].RealDate = GetDate(Registros[x].Date, "/", false);
    }

    recordsGraph = [];
    for (var x = 0; x < Registros.length; x++) {
        var dateFrom = GetDate($("#TxtRecordsFromDate").val(), "/", false);
        var dateTo = GetDate($("#TxtRecordsToDate").val(), "/", false);

        var show = true;
        var dateInicio = GetDate(ItemData.StartDate, "/", false);
        dateFromRegistro = GetDate(Registros[x].Date, "/", false);
        if (dateFromRegistro < dateInicio) {
            show = false;
        }

        if (show === true) {
            if (dateFrom !== null) {                
                if (dateFromRegistro < dateFrom) {
                    show = false;
                }
            }
        }

        if (show === true) {
            if (dateTo !== null) {
                dateToRegistro = GetDate(Registros[x].Date, "/", false);
                if (dateToRegistro > dateTo) {
                    show = false;
                }
            }
        }

        if (show === true) {
            count++;
            RenderRegistroRow(Registros[x]);
            recordsGraph.push(Registros[x]);
        }
    }

    $("#NumberCosts").html(count);

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

    DisableVinculatedTo(count > 0);

    lockOrderList = false;
    if (typeof exportType !== "undefined" && exportType !== null) {
        if (exportType === "PDF") { Export("PDF"); }
        if (exportType === "Excel") { Export("Excel"); }
    }
}

function Export(fileType) {
    console.log("Export", fileType);
    var data = {
        "dateFrom": GetDate($("#TxtRecordsFromDate").val(), "/", false),
        "dateTo": GetDate($("#TxtRecordsToDate").val(), "/", false),
        "companyId": Company.Id,
        "objetivoName": ItemData.Name,
        "objetivoId": ItemData.Id,
        "indicadorId": ItemData.IndicatorId,
        "listOrder": listOrder
    };
    var webMethod = "/Export/ObjetivoRecords.aspx/" + fileType;
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
            var link = document.createElement("a");
            link.id = "download";
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = "_blank";
            document.body.appendChild(link);
            $("#download").trigger("click");
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog("close");
            document.body.removeChild(link);
        },
        "error": function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}

function ObjetivoRegistroAll() {
    ObjetivoRegistroFilter();
    //$("#BtnRecordShowAll").hide();
    //$("#BtnRecordShowNone").show();
    $("#ObjetivoRegistrosTable").show();
    $("#ItemTableError").hide();
    $("#ItemTableVoid").hide();
}

function ObjetivoRegistroNone() {
    $("#TxtRecordsFromDate").val("");
    $("#TxtRecordsToDate").val("");
    $("#ObjetivoRegistrosTable").html("");
    $("#ObjetivoRegistrosTable").hide();
    $("#ItemTableError").hide();
    $("#ItemTableVoid").show();
    $("#BtnRecordShowAll").show();
    $("#BtnRecordShowNone").hide();
}

var selectedRecordId = null;

function PreRecordNew() { }

function RecordNew() {
    // TRELLO "El lado oscuro"
    // Tarjeta "OBJ: Petada entrando registros (lee comentarios!!!)"
    // "... es mejor que al añadir registro, si no hay meta, no compare con nada y coloree la línea en negro."
    /*if ($("#TxtMeta").val() * 1 === 0 && $("#DivCmbMetaComparer").css("display") !== "none") {
        alertInfoUI(Dictionary.Item_Objetivo_Message_NoMeta, null);
        return false;
    }*/

    if (ItemData.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Objetivo_Message_ObjetivoClosed, null);
        return false;
    }
    if (IndicadorObjetivo !== null && typeof IndicadorObjetivo.EndDate !== "undefined" && IndicadorObjetivo.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Objetivo_Message_IndicadorClosed, null);
        return false;
    }

    RegistroFormReset();
    $("#TxtRegistroValue").val("");
    $("#TxtRecordDate").val("");
    $("#TxtRegistroComments").val("");
    $("#CmbResponsibleRecord").val(0);
    RecordEdit(-1);
}

function RecordEdit(id) {
    if (ItemData.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Objetivo_Message_ObjetivoClosedUpdate, null);
        return false;
    }
    if (IndicadorObjetivo !== null && typeof IndicadorObjetivo.EndDate !== "undefined" && IndicadorObjetivo.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Objetivo_Message_IndicadorClosedUpdate, null);
        return false;
    }

    if (IndicadorObjetivo === null) {
        $("#TxtRegistroCommentsLabel").html(Dictionary.Item_IndicatorRecord_FieldLabel_Comments+ "<span class=\"required\">*</span>");
    }
    else {
        $("#TxtRegistroCommentsLabel").html(Dictionary.Item_IndicatorRecord_FieldLabel_Comments);
    }

    selectedRecordId = id * 1;
    FillFormRegistro(selectedRecordId);
    var title = selectedRecordId == -1 ? Dictionary.Item_IndicatorRecord_PopupTitle_Insert : Dictionary.Item_IndicatorRecord_PopupTitle_Update;
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
                "click": function () { ObjetivoRegistroSave(); }
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

function FillFormRegistro(id) {
    if (id > 0) {
        var registro = RegistroGetById(id);
        if (registro !== null) {
            $("#TxtRegistroValue").val(registro.Value);
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
        $("#TxtRecordDate").val(FormatDate(new Date(),"/"));
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
    $("#TxtRecordDateMinimum").hide();
    $("#TxtRegistroCommentsLabel").css("color", "#333");
    $("#TxtRegistroCommentsErrorRequired").hide();
    $("#CmbResponsibleRecordLabel").css("color", "#333");
    $("#CmbResponsibleRecordErrorRequired").hide();
}

function ValidateRegistroForm() {
    var ok = true;
    RegistroFormReset();

    if ($("#TxtRegistroValue").val() === "") {
        ok = false;
        $("#TxtRegistroValueLabel").css("color", "#f00");
        $("#TxtRegistroValueErrorRequired").show();
    }

    if ($("#TxtRegistroComments").val() === "" && IndicadorObjetivo === null) {
        ok = false;
        $("#TxtRegistroCommentsLabel").css("color", "#f00");
        $("#TxtRegistroCommentsErrorRequired").show();
    }

    if ($("#CmbResponsibleRecord").val() * 1 < 1) {
        ok = false;
        $("#CmbResponsibleRecordLabel").css("color", "#f00");
        $("#CmbResponsibleRecordErrorRequired").show();
    }

    if ($("#TxtRecordDate").val() === "") {
        ok = false;
        $("#TxtRecordDateLabel").css("color", "#f00");
        $("#TxtRecordDateRequired").show();
    }
    else if (validateDate($("#TxtRecordDate").val()) === false) {
        ok = false;
        $("#TxtRecordDateLabel").css("color", "#f00");
        $("#TxtRecordDateMalformed").show();
    }
    else {
        var date = GetDate($("#TxtRecordDate").val(), "/", false);
        if (date > new Date()) {
            ok = false;
            $("#TxtRecordDateLabel").css("color", "#f00");
            $("#TxtRecordDateMaximumToday").show();
        }

        // Objetivo sin indicador
        if ($("#CmbIndicador").val() * 1 < 1) {
            var inicio = GetDate($("#TxtFechaAlta").val(), "/", false);
            if (date < inicio) {
                ok = false;
                $("#TxtRecordDateMinimum").html(Dictionary.Item_Objetivo_Error_DateMinimum + " <strong>" + $("#TxtFechaAlta").val() + "</strong>");
                $("#TxtRecordDateMinimum").show();
                $("#TxtRecordDateLabel").css("color", "#f00");
            }
        }
    }

    return ok;
}

function ObjetivoRegistroSave() {
    if (ValidateRegistroForm() === false) {
        return false;
    }

    var meta = 0;
    var metaComparer = "";
    var webMethod = "";
    var data = null;

    if (IndicadorObjetivo === null) {
        meta = ItemData.Meta;
        metaComparer = ItemData.MetaComparer;
        webMethod = "/Async/ObjetivoActions.asmx/ObjetivoRegistroSave";
        data = {
            "registro": {
                "Id": selectedRecordId,
                "CompanyId": Company.Id,
                "ObjetivoId": ItemData.Id,
                "Value": StringToNumber($("#TxtRegistroValue").val(), ".", ","),
                "Date": GetDate($("#TxtRecordDate").val(), "/", false),
                "Comments": $("#TxtRegistroComments").val(),
                "MetaComparer": metaComparer,
                "Meta": meta,
                //"AlarmaComparer": Indicador.AlarmaComparer,
                //"Alarma": Indicador.Alarma,
                "Responsible": { "Id": $("#CmbResponsibleRecord").val() * 1 },
                "CreatedBy": { "Id": -1 },
                "CreatedOn": new Date(),
                "ModifiedBy": { "Id": -1 },
                "ModifiedOn": new Date(),
                "Active": true
            },
            "applicationUserId": ApplicationUser.Id
        };
    } else {
        meta = IndicadorObjetivo.Meta;
        metaComparer = IndicadorObjetivo.MetaComparer;
        webMethod = "/Async/ObjetivoActions.asmx/ObjetivoIndicadorRegistroSave";
        data = {
            "registro": {
                "Id": selectedRecordId,
                "CompanyId": Company.Id,
                "Indicador": { "Id": IndicadorObjetivo.Id },
                "Value": StringToNumber($("#TxtRegistroValue").val(), ".", ","),
                "Date": GetDate($("#TxtRecordDate").val(), "/", false),
                "Comments": $("#TxtRegistroComments").val(),
                "MetaComparer": metaComparer,
                "Meta": meta,
                "AlarmaComparer": IndicadorObjetivo.AlarmaComparer,
                "Alarma": IndicadorObjetivo.Alarma,
                "Responsible": { "Id": $("#CmbResponsibleRecord").val() * 1 },
                "CreatedBy": { "Id": -1 },
                "CreatedOn": new Date(),
                "ModifiedBy": { "Id": -1 },
                "ModifiedOn": new Date(),
                "Active": true
            },
            "applicationUserId": ApplicationUser.Id
        };
    }

    console.log(webMethod, data);

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
                console.log(selectedRecordId, response.d.ReturnValue);
                $("#dialogNewRecord").dialog("close");
                var res;
                eval("res=" + response.d.ReturnValue + ";");
                if (selectedRecordId < 0) {
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

                $("#BtnRecordFilter").click();

                // Al añadir un registro se bloquea los radios de vincular a indicador
                DisableVinculatedTo(true)
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function RecordDelete(id) {
    if (ItemData.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Objetivo_Message_ObjetivoClosedDelete, null);
        return false;
    }

    if (IndicadorObjetivo !== null && typeof IndicadorObjetivo.EndDate !== "undefined" && IndicadorObjetivo.EndDate !== null) {
        alertInfoUI(Dictionary.Item_Objetivo_Message_IndicadorClosedDelete, null);
        return false;
    }

    selectedRecordId = id;
    var registro = RegistroGetById(id);
    if (registro !== null) {
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
    var webMethod = "/Async/ObjetivoActions.asmx/DeleteRegistro";
    if (IndicadorName !== "") {
        webMethod = "/Async/IndicadorActions.asmx/DeleteRegistro";
    }
    var data = {
        "registroId": selectedRecordId,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    $("#dialogDeleteRecord").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": webMethod,
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

            // Poner fecha real en los registros
            for (var x = 0; x < Registros.length; x++) {
                Registros[x].RealDate = GetDate(Registros[x].Date, "/", false);
            }

            ObjetivoRegistroFilter();
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

window.chartColors = {
    red: 'rgb(255, 99, 132)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(255, 205, 86)',
    green: 'rgb(75, 192, 192)',
    blue: 'rgb(54, 162, 235)',
    purple: 'rgb(153, 102, 255)',
    grey: 'rgb(201, 203, 207)'
};

function DrawGraphics(stop) {
    if (Registros.length === 0) {
        $("#GraphicsNoData").show();
    }
    else {
        $("#GraphicsNoData").hide();
        console.log("DrawGraphics", firstChart);

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
                    fontColor: "rgb(255, 99, 132)"
                }
            }
        }

        var labels = new Array();
        var values = new Array();
        var metas = new Array();
        var alarmas = new Array();

        var lastValue = null;
        var lastMeta = null;
        var lastAlarm = null;
        var lastLabel = "";

        recordsGraph = recordsGraph.sort(function (a, b) {
            var x = a["RealDate"];
            var y = b["RealDate"];
            return ((x < y) ? -1 : ((x > y) ? 1 : 0));
        });

        for (var x = 0; x < recordsGraph.length; x++) {
            labels.push(recordsGraph[x].Date);
            values.push(recordsGraph[x].Value);
            metas.push(recordsGraph[x].Meta);
            alarmas.push(recordsGraph[x].Alarma);

            lastValue = recordsGraph[x].Value * 1;
            lastMeta = recordsGraph[x].Meta * 1;
            lastAlarm = recordsGraph[x].Alarma * 1;
            lastLabel = recordsGraph[x].Date;
        }

        var overlayData = {
            labels: labels,
            datasets: [
                {
                    "label": "Meta",
                    "type": "line",
                    "fillColor": "rgba(119,226,152,0)",
                    "strokeColor": "rgba(59,183,38,0.8)",
                    "highlightFill": "rgba(59,183,38,0.75)",
                    "highlightStroke": "rgba(94,114,95,1)",
                    "data": metas
                },
               {
                   "label": "Valor",
                   "fillColor": "#275b89",
                   "strokeColor": "rgba(77,110,240,0.8)",
                   "highlightFill": "rgba(77,100,240,0.75)",
                   "highlightStroke": "rgba(77,100,240,1)",
                   "data": values
               }
            ]
        };

        $("#barChartDiv").html("");
        this.div = document.getElementById("barChartDiv");
        this.div.style.height = "500px";
        this.chartCanvas = document.createElement("canvas");
        this.div.appendChild(this.chartCanvas);
        this.chartCanvas.style.width = $("#barChartDiv").width() + "px";
        this.chartCanvas.style.heitgh = $("#barChartDiv").height();
        this.chartCanvas.id = "canvas";

        this.ctx = this.chartCanvas.getContext("2d");
        this.chart = new Chart(this.ctx).Overlay(overlayData, {
            populateSparseData: true,
            overlayBars: false,
            datasetFill: true,
        });
        this.div.style.display = "none";

        setTimeout(function () {
            this.div.style.display = "block";
            if (stop !== true) {
                if ($("#canvas").css("width") === "0px") { DrawGraphics(true); }
            }
        }.bind(this), 100);

        var maxValue = lastValue;
        if (lastMeta > maxValue) { maxValue = lastMeta; }

        console.log("gauge");

        var maxValue = lastAlarm;
        if (lastValue > maxValue) { maxValue = lastValue; }

        $("#circularGaugeContainer").dxCircularGauge({
            "width": 10,
            "rangeContainer": {
                "offset": 10,
                "width": 20,
                "ranges": [
                    { "startValue": 0, "endValue": lastAlarm, "color": "#f00" },
                    { "startValue": lastMeta, "endValue": maxValue, "color": "#0f0" }
                ]
            },
            "scale": {
                "startValue": 0,
                "endValue": maxValue,
                "majorTick": { tickInterval: lastMeta / 4 }
            },
            /*"title": {
                "text": ItemData.Unidad.Description,
                "subtitle": "hola",
                "position": "left"
            },*/
            "tooltip": {
                "enabled": true,
                "customizeText": function (arg) {
                    return arg.valueText * 1;
                }
            },
            "subvalueIndicator": {
                "type": "textCloud",
                "text": {
                    customizeText: function (arg) {
                        console.log(arg.valueText);
                        return arg.valueText.split(',').join('') * 1;
                    }
                }
            },
            "value": lastValue,
            "subvalues": [lastValue]
        });
    }
}

function DisableVinculatedTo(disable) {
    console.log("DisableVinculatedTo", disable);
    document.getElementById("Contentholder1_RVinculatedYes").disabled = disable;
    document.getElementById("Contentholder1_RVinculatedNo").disabled = disable;
    document.getElementById("CmbIndicador").disabled = disable;

    if (disable === true) {
        $("#Contentholder1_RVinculatedYes").parent().attr("title", Dictionary.Item_Objetivo_Message_IndicadorBlocked);
        $("#Contentholder1_RVinculatedNo").parent().attr("title", Dictionary.Item_Objetivo_Message_IndicadorBlocked);
        $("#DivCmbIndicador").attr("title", Dictionary.Item_Objetivo_Message_IndicadorBlocked);
    }
    else {
        $("#Contentholder1_RVinculatedYes").parent().removeAttr("title");
        $("#Contentholder1_RVinculatedNo").parent().removeAttr("title");
        $("#DivCmbIndicador").removeAttr("title");
    }
}

function AnularPopup() {
    $("#TxtFechaCierreReal").val(FormatDate(new Date(), "/"));
    $("#TxtAnularComments").html("");
    $("#CmbEndResponsible").val(user.Employee.Id);
    var dialog = $("#dialogAnular").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Objetivo_PopupAnular_Title,
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

var anulationData = null;
function AnularConfirmed() {
    document.getElementById("TxtAnularCommentsLabel").style.color = "#000";
    document.getElementById("TxtFechaCierreRealLabel").style.color = "#000";
    document.getElementById("CmbEndResponsibleLabel").style.color = "#000";
    $("#TxtAnularCommentsErrorRequired").hide();
    $("#TxtFechaCierreRealErrorRequired").hide();
    $("#TxtFechaCierreRealDateMalformed").hide();
    $("#TxtFechaCierreRealCrossDate").hide();
    $("#CmbEndResponsibleErrorRequired").hide();

    var ok = true;
    if ($("#TxtAnularComments").val() === "") {
        ok = false;
        document.getElementById("TxtAnularCommentsLabel").style.color = "#f00";
        $("#TxtAnularCommentsErrorRequired").show();
    }

    if ($("#TxtFechaCierreReal").val() === "") {
        ok = false;
        document.getElementById("TxtFechaCierreRealLabel").style.color = "#f00";
        $("#TxtFechaCierreRealErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtFechaCierreReal").val()) === false) {
            ok = false;
            $("#TxtFechaCierreRealLabel").css("color", "#f00");
            $("#TxtFechaCierreRealDateMalformed").show();
        }
        else {
            var date = GetDate($("#TxtFechaCierreReal").val(), "/", false);
            if (date > new Date()) {
                $("#TxtFechaCierreRealLabel").css("color", "#f00");
                $("#TxtFechaCierreRealCrossDate").show();
            }
        }
    }

    if ($("#CmbEndResponsible").val() * 1 < 1) {
        ok = false;
        document.getElementById("CmbEndResponsibleLabel").style.color = "#f00";
        $("#CmbEndResponsibleErrorRequired").show();
    }

    if (ok === false) {
        return false;
    }

    //Anulate(int indicadorId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
    var webMethod = "/Async/ObjetivoActions.asmx/Anulate";
    var data = {
        "objetivoId": ItemData.Id,
        "companyId": Company.Id,
        "reason": $("#TxtAnularComments").val(),
        "responsible": $("#CmbEndResponsible").val() * 1,
        "date": GetDate($("#TxtFechaCierreReal").val(), "/"),
        "applicationUserId": user.Id
    };
    anulationData = data;
    $("#dialogAnular").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": webMethod,
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

function AnulateLayout() {
    $("#BtnRestaurar").hide();
    if (ItemData.EndDate !== null) {
        var message = "<div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_Objetivo_AnulateMessageTile + "</h3>";
        message += "    <p style=\"margin-left:50px;\">";
        message += "        " + Dictionary.Item_IndicatorRecord_FieldLabel_Reason + ": <strong>" + ItemData.EndReason + "</strong><br />";
        message += "        " + Dictionary.Item_IndicatorRecord_FieldLabel_Date + ": <strong>" + ItemData.EndDate + "</strong><br />";
        message += "        " + Dictionary.Item_Indicador_Field_EndResponsible + ": <strong>" + ItemData.EndResponsible.Value + "</strong>";
        message += "    </p>";
        message += "</div><br /><br /><br />";
        $("#home").append(message);
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
        "objetivoId": ItemData.Id,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/ObjetivoActions.asmx/Restore",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            ItemData.EndDate = null;
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

    $("#CmbResponsible").prop("disabled", true).trigger("chosen:updated");
    $("#CmbIndicador").prop("disabled", true).trigger("chosen:updated");
    $("#CmbMetaComparer").prop("disabled", true).trigger("chosen:updated");

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
    $("#CmbResponsible").prop("disabled", false).trigger("chosen:updated");
    $("#CmbIndicador").prop("disabled", false).trigger("chosen:updated");
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
        actionText = "Restaurar"
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

var totalCostActions = 0;
function RenderActionsTable() {
    console.log("RenderActionsTable");
    totalCostActions = 0;
    var totalActions = 0;
    if (Actions.length > 0) {
        var res = "";
        for (var x = 0; x < Actions.length; x++) {
            var show = true;
            var dateAction = GetDate(Actions[x].OpenDate, "/", false);
            if ($("#TxtActionsFromDate").val() !== "") {
                var dateFrom = GetDate($("#TxtActionsFromDate").val(), "/", false);
                if (dateAction < dateFrom) {
                    show = false;
                }
            }

            if ($("#TxtActionsToDate").val() !== "") {
                var dateTo = GetDate($("#TxtActionsToDate").val(), "/", false);
                if (dateAction > dateTo) {
                    show = false;
                }
            }

            if (show === true) {
                res += RenderActionsRow(Actions[x]);
                totalActions++;
            }
        }

        $("#ObjetivoActionsTable").html(res);
        $("#NumberCostsActionsTotal").html(ToMoneyFormat(totalCostActions,2));
    }
    else {
        $("#NumberCostsActionsTotal").html(ToMoneyFormat(0, 2));
    }

    $("#NumberCostsActions").html(totalActions);
}

function RenderActionsRow(actionData) {
    totalCostActions += actionData.Cost;
    var res = "<tr id=\"" + actionData.Id + "\">";
    res += "<td><a href=\"ActionView.aspx?id=" + actionData.Id + "\">" + actionData.Description + "</a></td>";
    res += "<td align=\"center\" style=\"width: 100px;\">" + actionData.OpenDate + "</td>";
    res += "<td align=\"center\" style=\"width:60px;\">" + actionData.Status.split('*').join('"') + "</td>";
    res += "<td align=\"center\" style=\"width: 100px;\">" + actionData.PreviewDate + "</td>";
    res += "<td align=\"right\" style=\"width:150px;\">" + ToMoneyFormat(actionData.Cost, 2) + "</td>";
    res += "<td style=\"width:45px;\">";
    res += "    <span class=\"btn btn-xs btn-info\" id=\"00001\"><i class=\"icon-edit bigger- 120\"></i></span>";
    res += "</td></tr>";
    return res;
}

function DataIsChanged() {
    var indicatorId = $("#CmbIndicador").val() * 1;

    var meta = null;
    if ($("#TxtMeta").val() !== "") {
        meta = StringToNumberNullable($("#TxtMeta").val(), ".", ",");
    }

    var metaComparer = null;
    if ($("#CmbMetaComparer").val() !== "") {
        metaComparer = $("#CmbMetaComparer").val();
    }

    var actual =  {
        "Id": ItemData.Id,
        "CompanyId": Company.Id,
        "Name": $("#TxtName").val(),
        "Description": $("#TxtDescription").val(),
        "Methodology": $("#TxtMetodologia").val(),
        "Resources": $("#TxtRecursos").val(),
        "Notes": $("#TxtNotes").val(),
        "VinculatedToIndicator": indicatorId > 0,
        "IndicatorId": indicatorId,
        "Responsible": { "Id": $("#CmbResponsible").val() * 1, "Value": "", "Active": false },
        "EndResponsible": { "Id": $("#CmbEndResponsible").val() * 1, "Value": "", "Active": false },
        "StartDate": GetDate($("#TxtFechaAlta").val(), "/", true),
        "PreviewEndDate": GetDate($("#TxtFechaCierrePrevista").val(), "/", false),
        "EndDate": GetDate($("#TxtFechaCierreReal").val(), "/", false),
        "RevisionId": $("#TxtPeriodicity").val() * 1,
        "MetaComparer": metaComparer,
        "Meta": meta,
        "CreatedBy": { "Id": -1 },
        "CreatedOn": new Date(),
        "ModifiedBy": { "Id": -1 },
        "ModifiedOn": new Date(),
        "Active": false
    };

    if (OriginalItemData.Name !== actual.Name) { return true; }
    if (OriginalItemData.Description !== actual.Description) { return true; }
    if (OriginalItemData.Methodology !== actual.Methodology) { return true; }
    if (OriginalItemData.Resources !== actual.Resources) { return true; }
    if (OriginalItemData.Notes !== actual.Notes) { return true; }
    if (OriginalItemData.VinculatedToIndicator !== actual.VinculatedToIndicator) { return true; }
    if (OriginalItemData.Responsible.Id !== actual.Responsible.Id) { return true; }
    if (OriginalItemData.EndResponsible.Id !== actual.EndResponsible.Id) { return true; }
    if (OriginalItemData.StartDate !== FormatDate(actual.StartDate,"/")) { return true; }
    if (OriginalItemData.PreviewEndDate !== FormatDate(actual.PreviewEndDate,"/")) { return true; }
    if (OriginalItemData.EndDate !== FormatDate(actual.EndDate, "/") && (OriginalItemData.EndDate !== null && FormatDate(actual.EndDate, "/") !== "")) { return true; }
    if (OriginalItemData.RevisionId !== actual.RevisionId) { return true; }
    if (OriginalItemData.MetaComparer !== actual.MetaComparer) { return true; }
    if (OriginalItemData.Meta !== actual.Meta) { return true; }

    return false;
}

function ActionNew() {
    if (DataIsChanged()) {
        DataChangedPopup();
    }
    else {
        NewActionConfirmed(false);
    }
}

function DataChangedPopup() {
    var dialog = $("#dialogDataChanged").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_NoSaveData,
        "width": 600,
        "buttons":
        [
            {
                "id": "BtnSaveContinue",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_Objetivo_DataChangedWarning_save,
                "class": "btn btn-success btn-xs",
                "click": function () { NewActionConfirmed(true); }
            },
            {
                "id": "BtnContinue",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_Objetivo_DataChangedWarning_continue,
                "class": "btn btn-success btn-xs",
                "click": function () { NewActionConfirmed(false); }
            },
            {
                "id": "BtnContinueCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function NewActionConfirmed(saveObjetivo) {
    if (saveObjetivo) {
        Save(true);
        return false;
    }

    document.location = "/ActionView.aspx?id=-1&o=" + ItemData.Id;
}