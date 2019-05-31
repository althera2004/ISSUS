var ADJUDICACION_Context = {
    "certificaciones": null,
    "id": null,
    "clientePropietarioId": null,
    "ClientePropietario": null,
    "localidadId": null,
    "OTFilerOpen": true,
    "InventarioFilerOpen": true,
    "LastIndexInventario": null,
    "LastIndexOT": null,
    "TipoAdjudicacion": null,
    "OTTableFistLoad": true,
    "CodigoOTManual": false,
    "Importe": null,
    "IVA": null,
    "DGBI": null,
    "Security": null,
    "Baja": null
};

var ADJUDICACION_OTList_Columns = {
    "CodigoOT": 0,
    "RefHost": 1,
    "Descripcion": 2,
    "Direccion": 3,
    "Barrio": 4,
    "Elemento": 5,
    "Recepcion": 6,
    "Inicio": 7,
    "Certificacion": 8,
    "Estado": 9,
    "Urgente": 10,
    "Iris": 11,
    "Host": 12
};

var ADJUDICACION_InventarioList_Columns = {
    "CodigoInventario": 0,
    "CodigoOT": 1,
    "Direccion": 2,
    "Barrio": 3,
    "Elemento": 4,
    "Patologia": 5,
    "NivelProblema": 6
};

var table_AdjudicacionAmpliacion;

var markers = new Array();
var markersInventari = new Array();
var mapEdifici = null;
var mapOT = null;
var mapInventari = null;
var myNewChart_4;
var myNewChart_6;
var tableOTLoaded = false;
var tableEdificiLoaded = false;
var tableInventarioLoaded = false;
var resto = 0;

function PrepareExpedientes() {
    var res = "<li class=\"\" id=\"tabExpedientes\">";
    res += "<a data-toggle=\"tab\" id=\"Expedientes_Link\" href=\"#ExpedientesDiv\" style=\"background-color:#efefef;margin-top:1px;color:#333 !important;\"><span>Expedientes</span></a>";
    res += "</li>";
    res += "<li class=\"\" id=\"tabMapaGeneral\">";
    res += "<a data-toggle=\"tab\" id=\"Expedientes_Link\" href=\"#MapaGeneralDiv\" style=\"background-color:#efefef;margin-top:1px;color:#333 !important;\"><span>Mapa general</span></a>";
    res += "</li>";
    var content = "<div class=\"tab-pane fade in no-padding-bottom active\" id=\"ExpedientesDiv\">";
    content += "  <section class=\"col\" style=\"width:100%;margin-top:20px;\" id=\"ExpedienteTable\">";
    content += "    <div class=\"col col-sm-12\">";
    content += "      <table id=\"datatable_col_reorder_Expediente\" class=\"table table-striped table-bordered table-hover\" style=\"width:100%\"></table>";
    content += "    </div>";
    content += "  </section>";
    content += "  <section class=\"col\" style=\"width:100%;margin-top:20px;display:none;\"id=\"ExpedienteForm\">";
    content += "    <div class=\"col col-sm-12\">";
    content += "      form";
    content += "      <table id=\"datatable_col_reorder_ExpedienteInventario\" class=\"table table-striped table-bordered table-hover\" style=\"width:100%\"></table>";
    content += "    </div>";
    content += "  </section>";
    content += "</div>";
    content += "<div class=\"tab-pane fade in no-padding-bottom active\" id=\"MapaGeneralDiv\">mapa</div>";

    $("#tabAdjudicacion4").after(res);
    $("#myTabContent").append(content);
    ADJUDICACION_RenderTableExpedientes();
    $("#datatable_col_reorder_Expediente_wrapper .ColVis").append("<a class=\"btn btn-default btn-success btn-edit\" onclick=\"ADJUDICACION_ExpedienteNew();\" id=\"BtnExpedienteNew\" style=\"float: right; padding: 8px;margin-right:4px;\"><i class=\"fa fa-plus\"></i>&nbsp;Nou expedient</a>&nbsp;");
}

function ADJUDICACION_RenderTableExpedientes() {
    var table_Expediente = $("#datatable_col_reorder_Expediente").dataTable({
        "processing": true,
        "data": FK.Expediente,
        "oLanguage": {
            "sProcessing": "<i class=\"fa fa-gear fa-spin fa-fw\"></i>" + Dictionary.DataTable.Processing + " <strong>Puntos de suministro en el alcance de cada auditoría</strong> ...",
            "sLengthMenu": Dictionary.DataTable.LengthMenu,
            "sZeroRecords": Dictionary.DataTable.ZeroRecords,
            "sInfo": Dictionary.DataTable.Info,
            "sInfoEmpty": Dictionary.DataTable.InfoEmpty,
            "sInfoFiltered": Dictionary.DataTable.InfoFiltered,
            "sInfoPostFix": Dictionary.DataTable.InfoPostFix,
            "sSearch": Dictionary.DataTable.Search,
            "sUrl": "",
            "oPaginate": {
                "sFirst": Dictionary.DataTable.Paginate.First,
                "sPrevious": Dictionary.DataTable.Paginate.Previous,
                "sNext": Dictionary.DataTable.Paginate.Next,
                "sLast": Dictionary.DataTable.Paginate.Last
            }
        },
        "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-4'f><'col-sm-8 col-xs-8 hidden-xs'C|l>>r" +
            "t" + // the table
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-sm-6 col-xs-12'p>>",
        "autoWidth": true,
        "aaSorting": [[0, "asc"]],
        "aoColumns":
            [
                { "sTitle": "Codi", "sortable": true, "hideShowExclude": false, mDataProp: "Code", "mRender": function (data) { return data; } },
                { "sTitle": "Adjudicatari", "sortable": true, "hideShowExclude": false, mDataProp: "Adjudicatario", "mRender": function (data) { return data; } },
                { "sTitle": "Dotació", "sortable": true, "hideShowExclude": false, mDataProp: "Dotacion", "mRender": function (data) { return data; } },
                {
                    "sortable": false,
                    "width": 30,
                    "hideShowExclude": true,
                    "mDataProp": "Id",
                    "mRender": function (data, type, full) {
                        return "<div style=\"white-space: nowrap;\">" + "<a id=\"BtnExpedienteView_" + data.Id + "\" onclick=\"ADJUDICACION_ExpedienteView(" + data + ");\" class=\"btn-edit\"><span><i class=\"fa fa-edit\"></i></span></a><a style=\"margin-left:5px;\" data-toggle=\"modal\" class=\"BtnDelete\" data-target=\"#dialog-delete\"><span onclick=\"PrepareInactivePopup(" + data + ", \'table_AuditorySupplyPoint\');\"><i class=\"fa fa-trash-o\"></i></span></a></div>";
                }
                }
            ],
        "fnDrawCallback": function () { dataTableSuccess("TableExpedientesDiv"); }
    });

    BtnNewUpdate("Expediente", "Nou expedient", ADJUDICACION_ExpedienteNew);
}

function ADJUDICACION_ExpedienteNew() {
    ADJUDICACION_ExpedienteView(-1);
}

function ADJUDICACION_ExpedienteView(id) {
    $("#ExpedienteTable").hide();
    $("#ExpedienteForm").show();

    var customAjax = "/CustomersFramework/Constraula/Data/ItemDataBase.aspx?InstanceName=" + CustomerName + "&Action=ITEM_INCIDENCIAExpedientable_GETBYADJUDICACION";
    if (id > 0) {
        customAjax = "/CustomersFramework/Constraula/Data/ItemDataBase.aspx?InstanceName=" + CustomerName + "&Action=ITEM_INCIDENCIAGETBYEXPEDIENTEADJUDICACION";
    }

    var table_ExpedienteInventario = $('#datatable_col_reorder_ExpedienteInventario').dataTable({
        "ajax": {
            "url": customAjax,
            "dataSrc": "data",
            "type": "POST"
        },
        "deferRender": true,
        "scrollY": "50vh",
        "scrollCollapse": true,
        "processing": true,
        "pageLength": 50,
        "oLanguage": {
            "sProcessing": '<i class="fa fa-gear fa-spin fa-fw"></i>' + Dictionary.DataTable.Processing + ' <strong>Actuacions</strong> ...',
            "sLengthMenu": Dictionary.DataTable.LengthMenu,
            "sZeroRecords": Dictionary.DataTable.ZeroRecords,
            "sInfo": Dictionary.DataTable.Info,
            "sInfoEmpty": Dictionary.DataTable.InfoEmpty,
            "sInfoFiltered": Dictionary.DataTable.InfoFiltered,
            "sInfoPostFix": Dictionary.DataTable.InfoPostFix,
            "sSearch": Dictionary.DataTable.Search,
            "sUrl": "",
            "oPaginate": {
                "sFirst": Dictionary.DataTable.Paginate.First,
                "sPrevious": Dictionary.DataTable.Paginate.Previous,
                "sNext": Dictionary.DataTable.Paginate.Next,
                "sLast": Dictionary.DataTable.Paginate.Last
            }
        },
        "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-4'f><'col-sm-8 col-xs-8 hidden-xs'C|l>>r" +
            "t" + // the table
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-sm-6 col-xs-12'p>>",
        "autoWidth": true,
        "aaSorting": [[0, 'asc']],
        "aoColumns":
            [
                { "sTitle": "Codi", "sortable": true, "hideShowExclude": false, mDataProp: "CodigoIncidencia" },
                { "sTitle": "CodiOT", "sortable": true, "hideShowExclude": false, mDataProp: "CodigoOT" },
                { "sTitle": "Direcció", "sortable": true, "hideShowExclude": false, mDataProp: "Direccion" },
                { "sTitle": "Barri", "sortable": true, "hideShowExclude": false, mDataProp: "BarrioId", "mRender": function (data, type, full, field) { return GetDescription(data, type, full, field); }, "sClass": "hidden-mobile" },
                { "sTitle": "Element", "sortable": true, "hideShowExclude": false, mDataProp: "ElementoDescription", "mRender": function (data, type, full, field) { return GetDescription(data, type, full, field); } },
                { "sTitle": "Patologia", "sortable": true, "hideShowExclude": false, mDataProp: "PatologiaId", "mRender": function (data, type, full, field) { return GetDescription(data, type, full, field); }, "sClass": "hidden-mobile" },
                { "sTitle": "Nivell de problema", "sortable": true, "hideShowExclude": false, mDataProp: "NivelProblemaDescription", "mRender": function (data, type, full, field) { return GetDescription(data, type, full, field); }, "sClass": "hidden-mobile" }
            ],
        "fnDrawCallback": function () { dataTableSuccess("datatable_col_reorder_ExpedienteInventario"); },
        "createdRow": function (row, data, index) { if (typeof createdRow_Inventario !== "undefined") { createdRow_Inventario(row, data, index); } }
    });
}

function ADJUDICACION_CustomActions() {

    // Esconde la línea que tiene el importe, pero necesitamos el campo para guardar
    $("#HiddenRow").hide();
    if (User.Core === true) {
        ADJUDICACION_Core();
    }

    // Reprogramar botón eliminar del popup
    $("#BtnDeleteAccept").removeAttr("onclick");
    $("#BtnDeleteAccept").on("click", ACTUACION_DeleteActuacionConfirmed);
    $("#tabAdjudicacion1").on("click", function () { ADJUDICACION_Tab = 1; });
    $("#tabAdjudicacion3").on("click", function () { ADJUDICACION_Tab = 3; });

    // Ocultar número de filas de ampliaciones
    $("#datatable_col_reorder_AdjudicacionAmpliacion_wrapper .dataTables_length").hide();

    console.log("ADJUDICACION_CustomActions", Data.CodigoOTManual);
    $("#datatable_col_reorder_Certificaciones_wrapper .ColVis").hide();
    BtnNewUpdate("PrespuestosCentros", "Afegir pressupost", ADJUDICACION_GoNewPresupuesto);

    // CodigoOTManual
    if (typeof Data.Id !== "undefined") {
        var codigoOTManualText = "Numeració automàtica";
        if (Data.CodigoOTManual === true) {
            codigoOTManualText = "Introducció manual";
            ADJUDICACION_Context.CodigoOTManual = true;
        }
        $("#AdjudicacionTxtPrefijoOT").parent().parent().parent().append(codigoOTManualText);
        ADJUDICACION_Context.IVA = Data.IVA;
        ADJUDICACION_Context.DGBI = Data.DGBI;
        ADJUDICACION_Context.Security = Data.Security;
        ADJUDICACION_Context.Baja = Data.Baja;
    }

    $("#datatable_col_reorder_Certificaciones_wrapper .ColVis").hide();
    // bloquear pestañas
    // $("#tabAdjudicacion5").hide();
    $("#tabAdjudicacion6").hide();
    $("#tabAdjudicacion7").hide();
    if (User.Core !== true) {
        for (var i = 0; i < User.Groups.length; i++) {
            if (User.Groups[i] < 6) {
                //$("#tabAdjudicacion5").show();
                $("#tabAdjudicacion6").show();
                $("#tabAdjudicacion7").show();
                break;
            }
        }
    }
    else {
        //$("#tabAdjudicacion5").show();
        $("#tabAdjudicacion6").show();
        $("#tabAdjudicacion7").show();
    }

    $("#Adjudicacion3 H3").on("click", ADJUDICACION_OTFilter_Toggle);
    $("#Adjudicacion3 H3").prepend("<i class=\"fa fa-compress\" id=\"OTFilterToogle\"></i>&nbsp;");
    $("#Adjudicacion1 H3").on("click", ADJUDICACION_InventarioFilter_Toggle);
    $("#Adjudicacion1 H3").prepend("<i class=\"fa fa-compress\" id=\"InventarioFilterToogle\"></i>&nbsp;");
    ADJUDICACION_OTFilter_Expand();
    ADJUDICACION_InventarioFilter_Expand();

    GetFilter();

    // Eliminar COLVis
    $(".ColVis_Button").hide();

    $("#JarvisTitle").hide();

    // Eliminar COLVis de apliaciones
    $("#datatable_col_reorder_AdjudicacionAmpliacion_wrapper .ColVis_Button").hide();
    $("#datatable_col_reorder_AdjudicacionAmpliacion_wrapper #BtnNew").css("padding", "8px");
    $("#datatable_col_reorder_AdjudicacionAmpliacion_wrapper #BtnNew").html("<i class=\"fa fa-plus\"></i>&nbsp;Afegir amplicació");
    var table = $("#datatable_col_reorder_AdjudicacionAmpliacion").DataTable();

    // Poner el simbolo %
    if (document.getElementById("AdjudicacionTxtImporte") !== null && document.getElementById("AdjudicacionTxtImporte").previousElementSibling !== null) {
        document.getElementById("AdjudicacionTxtImporte").previousElementSibling.className = "icon-append fa fa-euro";
    }

    if (document.getElementById("AdjudicacionTxtDGBI") !== null && document.getElementById("AdjudicacionTxtDGBI").previousElementSibling !== null) {
        document.getElementById("AdjudicacionTxtDGBI").previousElementSibling.className = "icon-append fa percent-sign";
    }

    if (document.getElementById("AdjudicacionTxtIVA") !== null && document.getElementById("AdjudicacionTxtIVA").previousElementSibling !== null) {
        document.getElementById("AdjudicacionTxtIVA").previousElementSibling.className = "icon-append fa percent-sign";
    }

    if (document.getElementById("AdjudicacionTxtBaja") !== null && document.getElementById("AdjudicacionTxtBaja").previousElementSibling !== null) {
        document.getElementById("AdjudicacionTxtBaja").previousElementSibling.className = "icon-append fa percent-sign";
    }

    if (document.getElementById("AdjudicacionTxtSecurity") !== null && document.getElementById("AdjudicacionTxtSecurity").previousElementSibling !== null) {
        document.getElementById("AdjudicacionTxtSecurity").previousElementSibling.className = "icon-append fa percent-sign";
    }

    // Poner el tipo de adjudicacion
    if (typeof Data.Id !== "undefined") {
        $("#AdjudicacionTxtTipo").val(Data.Tipo);
        ADJUDICACION_Context.id = Data.Id;
        ADJUDICACION_Context.clientePropietarioId = Data.ClientePropietarioId;
        ADJUDICACION_Context.ClientePropietario = GetByIdFromList(FK.ClientePropietario, Data.ClientePropietarioId);
        ADJUDICACION_Context.TipoAdjudicacion = Data.Tipo;
        ADJUDICACION_Context.localidadId = Data.LocalidadId;
        ADJUDICACION_GetLastIndex();
    }

    // Poner la ayuda a las series
    if (typeof Data.Id === "undefined") {
        var infoText = "<p class=\"alert alert-info\" id=\"CoreWarning\">";
        infoText += "        <i class=\"fa fa-info-circle fa-fw fa-lg\"></i><strong>Ajut:</strong>&nbsp;";
        infoText += "        Els prefixos es faran servir per a enumerar els inventaris i ot's d'aquesta adjudicació.&nbsp;Es recomana no repetir prefixos de altres adjudicacions.";
        infoText += "</p>";
        $("#PrefijoAyuda").html(infoText);
        $("#PrefijoAyuda").show();
    }

    if (typeof Data.Id !== "undefined") {
        var ButtonPDFContrata = "<button type=\"button\" id=\"btn-ExcelContrata\" class=\"btn btn-primary\" onclick=\"ADJUDICACION_ContrataExcel();\" style=\"display:none;\">";
        ButtonPDFContrata += "<i class=\"fa fa-file-pdf-o\"></i>&nbsp;Exportar dades a Excel";
        ButtonPDFContrata += "</button>";
        $("#footer-button").prepend(ButtonPDFContrata);

        var ButtonInventarioReport = "<div class=\"btn-group dropup\">";
        ButtonInventarioReport += "<button class=\"btn btn-primary dropdown-toggle txt-color-white\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-files-o\"></i>&nbsp;Informes&nbsp;<span class=\"caret\"></span></button>";
        ButtonInventarioReport += "<ul class=\"dropdown-menu pull-left text-left\">";
        ButtonInventarioReport += " <li><a href=\"javascript:ADJUDICACION_ReportGeneral();\">Resum general</a></li>";
        //ButtonInventarioReport += " <li><a href=\"javascript:void(0);\">Resum particular</a></li>";
        ButtonInventarioReport += " <li><a href=\"javascript:ADJUDICACION_ReportMensual();\">Informe mensual</a></li>";
        ButtonInventarioReport += " <li class=\"divider\"></li>";
        ButtonInventarioReport += " <li><a href=\"javascript:ADJUDICACION_PrintPDFInventario(0);\">Plànol inventari</a></li>";
        ButtonInventarioReport += " <li><a href=\"javascript:ADJUDICACION_PrintPDFInventario(1);\">Plànol inventari calor</a></li>";
        ButtonInventarioReport += " <li><a href=\"javascript:ADJUDICACION_PrintPDFOT(0);\">Plànol OT's</a></li>";
        ButtonInventarioReport += " <li><a href=\"javascript:ADJUDICACION_PrintPDFOT(1);\">Plànol OT's calor</a></li>";
        //ButtonInventarioReport += " <li class=\"divider\"></li>";
        //ButtonInventarioReport += " <li><a href=\"javascript:void(0);\">Informe personalitzat</a></li>";
        ButtonInventarioReport += "</ul>";
        ButtonInventarioReport += "</div>";

        $("#footer-button").prepend(ButtonInventarioReport);

        //ADJUDICACION_RenderEconomics();
    }

    $("#mapModal").on("shown.bs.modal", function (e) {
        console.log("Map rsize");
        if (typeof mapOT !== "undefined" && mapOT !== null) { mapOT._onResize(); }
        if (typeof mapInventari !== "undefined" && mapInventari !== null) { mapInventari._onResize(); }
        if (typeof mapEdifici !== "undefined" && mapEdifici !== null) { mapEdifici._onResize(); }
    });

    // filtro range fechas
    // ----------------------------------------------------------
    $.fn.dataTableExt.afnFiltering.push(
        function (oSettings, aData, iDataIndex) {

            if (oSettings.nTable.id === "datatable_col_reorder_OTViaPublica") {
                var recepcionIni = document.getElementById("AdjudicacionTxtFechaRecepcionDesde").value;
                var recepcionFin = document.getElementById("AdjudicacionTxtFechaRecepcionHasta").value;

                var inicioIni = document.getElementById("AdjudicacionTxtFechaEjecucionDesde").value;
                var inicioFin = document.getElementById("AdjudicacionTxtFechaEjecucionHasta").value;

                var certificacionIni = document.getElementById("AdjudicacionTxtFechaCertificacionDesde").value;
                var certificacionFin = document.getElementById("AdjudicacionTxtFechaCertificacionHasta").value;

                // console.log("Filter OTViaPublica");                
                recepcionIni = recepcionIni.substring(6, 10) + recepcionIni.substring(3, 5) + recepcionIni.substring(0, 2);
                recepcionFin = recepcionFin.substring(6, 10) + recepcionFin.substring(3, 5) + recepcionFin.substring(0, 2);

                inicioIni = inicioIni.substring(6, 10) + inicioIni.substring(3, 5) + inicioIni.substring(0, 2);
                inicioFin = inicioFin.substring(6, 10) + inicioFin.substring(3, 5) + inicioFin.substring(0, 2);

                certificacionIni = certificacionIni.substring(6, 10) + certificacionIni.substring(3, 5) + certificacionIni.substring(0, 2);
                certificacionFin = certificacionFin.substring(6, 10) + certificacionFin.substring(3, 5) + certificacionFin.substring(0, 2);

                var show = true;

                // primero se filtran las urgentes porque se descartan más
                var urgenteSelect = document.getElementById("AdjudicacionTxtUrgent").checked;
                if (urgenteSelect === true) {
                    if (aData[ADJUDICACION_OTList_Columns.Urgente] === "false" || aData[ADJUDICACION_OTList_Columns.Estado] === "Certificat") {
                        return false;
                    }
                }

                // control fecha recepcion
                var datoRecepcionIni = aData[ADJUDICACION_OTList_Columns.Recepcion].substring(6, 10) + aData[ADJUDICACION_OTList_Columns.Recepcion].substring(3, 5) + aData[ADJUDICACION_OTList_Columns.Recepcion].substring(0, 2);
                var datoRecepcionFin = aData[ADJUDICACION_OTList_Columns.Recepcion].substring(6, 10) + aData[ADJUDICACION_OTList_Columns.Recepcion].substring(3, 5) + aData[ADJUDICACION_OTList_Columns.Recepcion].substring(0, 2);
                if (recepcionIni !== "" && recepcionIni > datoRecepcionIni) { return false; }
                if (recepcionFin !== "" && recepcionFin < datoRecepcionFin) { return false; }


                // control fecha inicio
                var datoInicioIni = aData[ADJUDICACION_OTList_Columns.Inicio].substring(6, 10) + aData[ADJUDICACION_OTList_Columns.Inicio].substring(3, 5) + aData[ADJUDICACION_OTList_Columns.Inicio].substring(0, 2);
                var datoInicioFin = aData[ADJUDICACION_OTList_Columns.Inicio].substring(6, 10) + aData[ADJUDICACION_OTList_Columns.Inicio].substring(3, 5) + aData[ADJUDICACION_OTList_Columns.Inicio].substring(0, 2);
                if (inicioIni !== "") {
                    if (recepcionIni > datoInicioIni) { return false; }
                    if (datoInicioIni === "") { return false; }
                }

                if (inicioFin !== "") {
                    if (recepcionFin < datoInicioFin) { return false; }
                    if (datoInicioFin === "") { return false; }
                }

                // control fecha certificacionvar
                datoCertificacionIni = aData[ADJUDICACION_OTList_Columns.Certificacion].substring(6, 10) + aData[ADJUDICACION_OTList_Columns.Certificacion].substring(3, 5) + aData[ADJUDICACION_OTList_Columns.Certificacion].substring(0, 2);
                var datoCertificacionFin = aData[ADJUDICACION_OTList_Columns.Certificacion].substring(6, 10) + aData[ADJUDICACION_OTList_Columns.Certificacion].substring(3, 5) + aData[ADJUDICACION_OTList_Columns.Certificacion].substring(0, 2);
                if (certificacionIni !== "") {
                    if (certificacionIni > datoCertificacionIni) { return false; }
                    if (datoCertificacionIni === "") { return false; }
                }

                if (certificacionFin !== "") {
                    if (certificacionFin < datoCertificacionFin) { return false; }
                    if (datoCertificacionFin === "") { return false; }
                }

                return show;
            }
            else {
                return true;
            }
        });
    // ---------------------------------------------------------- 
}

function ADJUDICACION_AfterPageLoad() {
    $("#tabAdjudicacion6").on("click", ADJUDICACION_FixCertificacionHeader);
    //Comprobar en el itembuilderdefinition que es adjudicacion para ejecutar este codigo
    //y evitar que se ejecute al acceder a una OT hija desde la adjudicacion
    if ($(ItemBuilderDefinition.ItemName).selector === "Adjudicacion") {
        //si es nueva
        if (typeof Data.Id === "undefined" || Data.Id < 0) {
            //esconder pestañas extra
            $("#tabAdjudicacion1,#tabAdjudicacion2,#tabAdjudicacion3,#tabAdjudicacion4,#tabAdjudicacion5").hide();
        }
        //si es existente
        else {
            //bloquear campos de datos de adjudicacion
            if (User.Core === true) {
                $("#AdjudicacionTxtImporte,#AdjudicacionTxtIVA,#AdjudicacionTxtDGBI,#AdjudicacionTxtBaja,#AdjudicacionTxtSecurity,#AdjudicacionTxtAutomatico,#AdjudicacionTxtTipo,#AdjudicacionTxtClientePropietarioId,#AdjudicacionTxtLocalidadId")
                    .attr("disabled", true) //deshabilitamos
                    .css("background-color", "#eee") //ponemos el fondo gris
                    .next().hide(); //escondemos su hijo (si lo tiene, si no como es jquery no peta)
            }
            else {
                $("#AdjudicacionTxtInicio,#AdjudicacionTxtFinal,#AdjudicacionTxtPrefijoInventario,#AdjudicacionTxtPrefijoOT,#AdjudicacionTxtNumContrato,#AdjudicacionTxtDescripcion,#AdjudicacionTxtImporte,#AdjudicacionTxtIVA,#AdjudicacionTxtDGBI,#AdjudicacionTxtBaja,#AdjudicacionTxtSecurity,#AdjudicacionTxtAutomatico,#AdjudicacionTxtTipo,#AdjudicacionTxtClientePropietarioId,#AdjudicacionTxtLocalidadId")
                    .attr("disabled", true) //deshabilitamos
                    .css("background-color", "#eee") //ponemos el fondo gris
                    .next().hide(); //escondemos su hijo (si lo tiene, si no como es jquery no peta)
            }

            //etiquietas de importe/saldo en lista de estado de contrata(certificacion)
            var acumulado = 0;
            for (var x = 0; x < FK.Certificacion.length; x++) {
                if (FK.Certificacion[x].AdjudicacionId === Data.Id) {
                    acumulado += FK.Certificacion[x].Importe;
                }
            }

            var saldoactual = Data.Importe - acumulado;

            var importesaldo = $("" +
                "<div class=\"col col-sm-2\">" +
                "   <label class=\"input\" name=\"ImporteLabel\" id=\"ImporteLabel\">Import PEM sense baixa</label>" +
                "</div>" +
                "<div class=\"col col-sm-4\">" +
                "   <label class=\"input\">" +
                "       <input disabled=\"disabled\" style=\"background-color: #FFFF99;\" type=\"text\" name=\"ImporteInput\" id=\"ImporteInput\" class=\"money-bank\" precision=\"2\" style=\"text-align: right;\">" +
                "   </label>" +
                "</div>" +
                "<div class=\"col col-sm-2\">" +
                "   <label class=\"input\" name=\"SaldoLabel\" id=\"SaldoLabel\">Saldo pendent</label>" +
                "</div>" +
                "<div class=\"col colsm-4\">" +
                "   <label class=\"input\">" +
                "       <input disabled style=\"background-color: #FFFF99;\" type=\"text\" name=\"SaldoInput\" id=\"SaldoInput\" class=\"money-bank\" precision=\"2\" style=\"text-align: right;\">" +
                "   </label>" +
                "</div>");

            $("#placeholderImporteSaldo").append(importesaldo);
            $("#ImporteInput").val(ToMoneyFormat(Data.Importe) + " €");
            $("#SaldoInput").val(ToMoneyFormat(saldoactual) + " €");
            $("#placeholderImporteSaldo").show();

            //por defecto quieren que se vea la petaña de OTs
            if (FormId === "AdjudicacionEdifici") {
                $("#Adjudicacion3").html("<div id=\"map\"></div>");
                $("#tabAdjudicacion3").on("click", forceResizeMap);
                $("#Adjudicacion2_Link").click();
                RenderMapEdificis();
                BtnNewUpdate("Actuacion", "Nova OT de edifici", ADJUDICACION_GoNewOT);
                $("#datatable_col_reorder_AdjudicacionCentros_wrapper #BtnNew").hide();
                ADJUDICACION_RenderFilterOTEdificio();
            }
            else if (FormId === "AdjudicacionViaPublica") {
                // Adaptar filtro de búsqueda OT
                ADJUDICACION_FilterOTLayout();

                if (ADJUDICACION_Tab === 3) {
                    $("#Adjudicacion3_Link").click();
                }
                else if (ADJUDICACION_Tab === 1) {
                    $("#Adjudicacion1_Link").click();
                }

                $("#Adjudicacion4").html("<div id=\"map\"></div>");
                $("#tabAdjudicacion4").on("click", ADJUDICACION_TabAdjudicacion4_Clicked);
                $("#Adjudicacion2").html("<div id=\"mapInventari\"></div>");
                $("#tabAdjudicacion2").on("click", ADJUDICACION_TabAdjudicacion2_Clicked);
                RenderMapInventario();
                RenderMapOTs();
                BtnNewUpdate("OTViaPublica", "Nova OT via pública", ADJUDICACION_GoNewOT);
                BtnNewUpdate("Inventario", "Nou inventari", ADJUDICACION_GoNewInventari);

                ADJUDICACION_RenderFilterViaPublica();
                ADJUDICACION_RenderFilterOTViaPublica();
                ADJUDICACION_RenderFilterInventario();

                $("#placeholderExportTodo").html("<a class=\"btn btn-default btn-success btn-edit\" onclick=\"ADJUDICACION_PopupTodoExcel();\" id=\"BtnTodoExcel\" style=\"float: right; padding: 8px;margin-right:4px;\"><i class=\"fa fa-file-excel-o\"></i>&nbsp;Expotar inventarios y OTs a Excel</a>&nbsp;&nbsp;&nbsp;");
                $("#placeholderExportTodo").show();
                $("#datatable_col_reorder_Inventario_wrapper .ColVis").append("<a class=\"btn btn-default btn-success btn-edit\" onclick=\"ADJUDICACION_PopupInventarioExcel();\" id=\"BtnInventarioExcel\" style=\"float: right; padding: 8px;margin-right:4px;\"><i class=\"fa fa-file-excel-o\"></i>&nbsp;Expotar a Excel</a>&nbsp;");
                $("#datatable_col_reorder_OTViaPublica_wrapper .ColVis").append("<a class=\"btn btn-default btn-success btn-edit\" onclick=\"ADJUDICACION_PopupOTViaPublicaExcel();\" id=\"BtnVPExcel\" style=\"float: right; padding: 8px;margin-right:4px;\"><i class=\"fa fa-file-excel-o\"></i>&nbsp;Expotar a Excel</a>&nbsp;");
                $("#datatable_col_reorder_OTViaPublica_wrapper .ColVis").append("<a class=\"btn btn-default btn-success btn-edit\" onclick=\"ADJUDICACION_PopupOTViaPublicaPDF();\" id=\"BtnVPPDF\" style=\"float: right; padding: 8px;margin-right:4px;\"><i class=\"fa fa-file-pdf-o\"></i>&nbsp;Expotar a PDF</a>&nbsp;");
            }
            else {
                // Modificar botones de añadir
                BtnNewUpdate("Incidencia", "Nou inventari", ADJUDICACION_GoNewIncidencia);
                BtnNewUpdate("OTViaPublica", "Nova OT", ADJUDICACION_GoNewOT);
            }
        }
    }

    $("#datatable_col_reorder_Certificacion_wrapper").hide();
    $("#tabAdjudicacion5").on("click", ADJUDICACION_Report);
    $("#tabAdjudicacion0").on("click", ADJUDICACION_Ficha);

    if (typeof Data.Id !== "undefined") {
        ADJUDICACION_TableCertificacionesRedraw();
    }

    ADJUDICACION_PrepareMailFields();

    $("#datatable_col_reorder_OTViaPublica").DataTable().order([[0, "desc"]]).draw();
    $("#datatable_col_reorder_Inventario").DataTable().order([[0, "desc"]]).draw();


    if (GetGrant("adjudicacion").Write === false) {
        $("#btn-submitExit").remove();
        $("#datatable_col_reorder_AdjudicacionAmpliacion_wrapper .ColVis").remove();
    }

    if (typeof Data.Id !== "undefined" && Data.Id > 0) {
        console.log("Campos readonly");


        var clientePropietario = GetByIdFromList(FK.ClientePropietario, Data.ClientePropietarioId);
        var localidad = GetByIdFromList(FK.Localidad, Data.LocalidadId);
        if (GetGrant("Adjudicacion").Write === true) {
            $("#AdjudicacionTxtClientePropietarioId").html("<option value=\"" + clientePropietario.Id + "\" selected=\"selected\">" + clientePropietario.Description + "</option>");
            $("#AdjudicacionTxtLocalidadId").html("<option value=\"" + localidad.Id + "\" selected=\"selected\">" + localidad.Description + "</option>");
        }
        else {
            if (clientePropietario !== null) {
                $("#AdjudicacionTxtClientePropietarioId").html(clientePropietario.Description);
            }

            if (localidad !== null) {
                $("#AdjudicacionTxtLocalidadId").html(localidad.Description);
            }

            if (Data.Tipo === 1) {
                $("#AdjudicacionTxtTipo").html("Via pública");
            }
            else {
                if (Data.Tipo === 2) {
                    $("#AdjudicacionTxtTipo").html("Edificis");
                }
            }
        }
    }
}

function ADJUDICACION_TabAdjudicacion4_Clicked() {
    $("#btn-printPDFInventario").hide();
    $("#btn-printPDFOT").show();
    $("#btn-ExcelContrata").hide();
    forceResizeMap();
    RenderMarkersOTs();
}

function ADJUDICACION_TabAdjudicacion2_Clicked() {
    $("#btn-printPDFInventario").show();
    $("#btn-printPDFOT").hide();
    $("#btn-ExcelContrata").hide();
    forceResizeMap();
}

function ADJUDICACION_PrintPDFInventario(mapType) {
    window.open("/CustomersFramework/Constraula/Pages/PDFMap.aspx?type=Inventario&id=" + Data.Id + "&icon=" + (mapType === 1 ? "ttf" : "icon"));
}

function ADJUDICACION_PrintPDFOT(mapType) {
    window.open("/CustomersFramework/Constraula/Pages/PDFMap.aspx?type=OT&id=" + Data.Id + "&icon=" + (mapType === 1 ? "ttf" : "icon"));
}

function ADJUDICACION_Report() {
    $("#btn-printPDFInventario").hide();
    $("#btn-printPDFOT").hide();
    $("#btn-ExcelContrata").show();
    $("#ImporteInput").val(ToMoneyFormat(Data.Importe, 2) + " €");
    ADJUDICACION_RenderEstadoContrata();
}

function ADJUDICACION_RenderFilterViaPublica() {
    // CreateSelect($("#AdjudicacionTxtEstadOTId").parent().parent(), "TxtEstadoOTId", FK.EstadoOT)
}

function ADJUDICACION_GetBarrios() {
    console.log("ADJUDICACION_GetBarrios");
    var res = new Array();
    for (var x = 0; x < FK.Barrio.length; x++) {
        if (FK.Barrio[x].ClientePropietarioId === Data.ClientePropietarioId) {
            if (FK.Barrio[x].Active === true) {
                res.push(FK.Barrio[x]);
            }
        }
    }
    return res;
}

function ADJUDICACION_RenderFilterOTViaPublica() {
    console.log("ADJUDICACION_RenderFilterOTViaPublica");
    // Rellenar campo de estados OT y asociar envento de filter

    var estadosFiltro = [
        { "Id": 2, "Description": "Pendent" },
        { "Id": 1, "Description": "En procés" },
        { "Id": 4, "Description": "Fet" },
        { "Id": 3, "Description": "Certificat" },
        { "Id": 5, "Description": "Anulada" }
    ];

    ADJUDICACION_FillComboFiltroOTEstado();
    FillCombo("AdjudicacionTxtBarrioId", ADJUDICACION_GetBarrios(), false, false);
    $("#AdjudicacionTxtEstadoOTId").on("change", ADJUDICACION_AdjudicacionTxtEstadoOTIdChanged);
    $("#AdjudicacionTxtBarrioId").on("change", ADJUDICACION_AdjudicacionTxtBarrioIdChanged);
    $("#AdjudicacionTxtFechaRecepcionDesde").on("change", ADJUDICACION_TableOTFilter);
    $("#AdjudicacionTxtFechaRecepcionHasta").on("change", ADJUDICACION_TableOTFilter);
    $("#AdjudicacionTxtFechaEjecucionDesde").on("change", ADJUDICACION_TableOTFilter);
    $("#AdjudicacionTxtFechaEjecucionHasta").on("change", ADJUDICACION_TableOTFilter);
    $("#AdjudicacionTxtFechaCertificacionDesde").on("change", ADJUDICACION_TableOTFilter);
    $("#AdjudicacionTxtFechaCertificacionHasta").on("change", ADJUDICACION_TableOTFilter);
    $("#AdjudicacionTxtUrgent").on("click", ADJUDICACION_TableOTFilter);
    $("#ChkFilterUrgenteMap").on("click", ADJUDICACION_TableOTFilterMap);

    $("#AdjudicacionTxtInventariFilterElementoId").on("change", ADJUDICACION_TableInventarioFilter);
    $("#AdjudicacionTxtInventariFilterNivelProblemaId").on("change", ADJUDICACION_TableInventarioFilter);
    $("#AdjudicacionTxtInventariFilterBarrioId").on("change", ADJUDICACION_TableInventarioFilter);

    // Anulado por simplicación de fechas del filtro
    /*document.getElementById("AdjudicacionTxtEstadoOTIdLabel").parentNode.className = "col col-sm-1";
    document.getElementById("AdjudicacionTxtEstadoOTId").parentNode.parentNode.className = "col col-sm-3 col-lg-2";
    document.getElementById("AdjudicacionTxtFechaRecepcionDesdeLabel").parentNode.className = "col col-sm-1";
    document.getElementById("AdjudicacionTxtFechaRecepcionDesde").parentNode.parentNode.className = "col col-sm-3 col-lg-2";
    document.getElementById("AdjudicacionTxtFechaRecepcionHastaLabel").parentNode.className = "col col-sm-1";
    document.getElementById("AdjudicacionTxtFechaRecepcionHasta").parentNode.parentNode.className = "col col-sm-3 col-lg-2";
    document.getElementById("AdjudicacionTxtFechaEjecucionDesdeLabel").parentNode.className = "col col-sm-1";
    document.getElementById("AdjudicacionTxtFechaEjecucionDesde").parentNode.parentNode.className = "col col-sm-3 col-lg-2";
    document.getElementById("AdjudicacionTxtFechaEjecucionHastaLabel").parentNode.className = "col col-sm-1";
    document.getElementById("AdjudicacionTxtFechaEjecucionHasta").parentNode.parentNode.className = "col col-sm-3 col-lg-2";
    document.getElementById("AdjudicacionTxtFechaCertificacionDesdeLabel").parentNode.className = "col col-sm-1";
    document.getElementById("AdjudicacionTxtFechaCertificacionDesde").parentNode.parentNode.className = "col col-sm-3 col-lg-2";
    document.getElementById("AdjudicacionTxtFechaCertificacionHastaLabel").parentNode.className = "col col-sm-1";
    document.getElementById("AdjudicacionTxtFechaCertificacionHasta").parentNode.parentNode.className = "col col-sm-3 col-lg-2";

    document.getElementById("AdjudicacionTxtBarrioIdLabel").parentNode.className = "col col-sm-2 col-lg-1";
    document.getElementById("AdjudicacionTxtBarrioId").parentNode.parentNode.className = "col col-sm-4 col-lg-3 col-lg-2";*/
}

function ADJUDICACION_RenderFilterInventario() {
    FillCombo("AdjudicacionTxtInventariFilterElementoId", FK.Elemento, false, false);
    FillCombo("AdjudicacionTxtInventariFilterNivelProblemaId", FK.NivelProblema, false, false);
    FillCombo("AdjudicacionTxtInventariFilterBarrioId", ADJUDICACION_GetBarrios(), false, false);

    document.getElementById("AdjudicacionTxtInventariFilterElementoIdLabel").parentNode.className = "col col-sm-2 col-lg-1";
    document.getElementById("AdjudicacionTxtInventariFilterElementoId").parentNode.parentNode.className = "col col-sm-4 col-lg-3";
    document.getElementById("AdjudicacionTxtInventariFilterNivelProblemaIdLabel").parentNode.className = "col col-sm-2 col-lg-1";
    document.getElementById("AdjudicacionTxtInventariFilterNivelProblemaId").parentNode.parentNode.className = "col col-sm-4 col-lg-3";
    document.getElementById("AdjudicacionTxtInventariFilterBarrioIdLabel").parentNode.className = "col col-sm-2 col-lg-1";
    document.getElementById("AdjudicacionTxtInventariFilterBarrioId").parentNode.parentNode.className = "col col-sm-4 col-lg-3";
}

function ADJUDICACION_RenderFilterOTEdificio() { }

function ADJUDICACION_AdjudicacionTxtBarrioIdChanged() {
    $("#CmbFilterOTMapBarrio").val($("#AdjudicacionTxtBarrioId").val());
    ADJUDICACION_TableOTFilter();
}

function ADJUDICACION_CmbFilterOTMapEstadoChanged() {
    $("#AdjudicacionTxtEstadoOTId").val($("#CmbFilterOTMapEstado").val());
    ADJUDICACION_TableOTFilter();
}

function ADJUDICACION_AdjudicacionTxtEstadoOTIdChanged() {
    $("#CmbFilterOTMapEstado").val($("#AdjudicacionTxtEstadoOTId").val());
    ADJUDICACION_TableOTFilter();
}

function ADJUDICACION_CmbFilterOTMapBarrioChanged() {
    $("#AdjudicacionTxtBarrioId").val($("#CmbFilterOTMapBarrio").val());
    ADJUDICACION_TableOTFilter();
}

function ADJUDICACION_TableOTFilterMap() {
    document.getElementById("AdjudicacionTxtUrgent").checked = document.getElementById("ChkFilterUrgenteMap").checked;
    ADJUDICACION_TableOTFilter();
}

function ADJUDICACION_TableOTFilter() {
    var filterValueEstado = $("#AdjudicacionTxtEstadoOTId :selected").text();
    if (filterValueEstado === Dictionary.Common_Select) { filterValueEstado = ""; }

    var filterValueBarrio = $("#AdjudicacionTxtBarrioId :selected").text();
    if (filterValueBarrio === Dictionary.Common_Select) { filterValueBarrio = ""; }

    console.log("ADJUDICACION_TableOTFilter", filterValueBarrio + " " + filterValueEstado);

    var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
    table.column(ADJUDICACION_OTList_Columns.Estado).search(filterValueEstado);
    table.column(ADJUDICACION_OTList_Columns.Barrio).search(filterValueBarrio);
    table.draw();
    ADJUDICACION_SetFilter();
}

function ADJUDICACION_TableInventarioFilter() {
    var filterElemento = $("#AdjudicacionTxtInventariFilterElementoId :selected").text();
    if (filterElemento === Dictionary.Common_Select) { filterElemento = ""; }

    var filterValueBarrio = $("#AdjudicacionTxtInventariFilterBarrioId :selected").text();
    if (filterValueBarrio === Dictionary.Common_Select) { filterValueBarrio = ""; }

    var filterValueNivelProblema = $("#AdjudicacionTxtInventariFilterNivelProblemaId :selected").text();
    if (filterValueNivelProblema === Dictionary.Common_Select) { filterValueNivelProblema = ""; }

    console.log("ADJUDICACION_TableInventarioFilter", filterValueBarrio + " " + filterElemento + " " + filterValueNivelProblema);

    var table = $("#datatable_col_reorder_Inventario").DataTable();
    table.column(ADJUDICACION_InventarioList_Columns.Elemento).search(filterElemento);
    table.column(ADJUDICACION_InventarioList_Columns.Barrio).search(filterValueBarrio);
    table.column(ADJUDICACION_InventarioList_Columns.NivelProblema).search(filterValueNivelProblema);
    table.draw();
    ADJUDICACION_SetFilter();
}

function ADJUDICACION_Ficha() {
    console.log("ADJUDICACION", "Ficha");

    $("#btn-printPDFInventario").hide();
    $("#btn-printPDFOT").hide();
    $("#btn-ExcelContrata").hide();

    $("#AdjudicacionTxtImporte").val(ToMoneyFormat(Data.Importe, 2));
    $("#AdjudicacionTxtIVA").val(ToMoneyFormat(Data.IVA, 2));
    $("#AdjudicacionTxtBaja").val(ToMoneyFormat(Data.Baja, 2));
    $("#AdjudicacionTxtSecurity").val(ToMoneyFormat(Data.Security, 2));
}

function ADJUDICACION_GoNewAmpliacion() {
    var params = new Array();
    params.push("&AdjudicacionIdparam=" + Data.Id);
    GoEncryptedItemView("AmpliacionAdjudicacion", -1, "AmplicacionByAdjudicacion", params);
}

function ADJUDICACION_GoNewInventari() {
    var params = new Array();
    params.push("&AdjudicacionIdparam=" + Data.Id);
    GoEncryptedItemView("Actuacion", -1, "Inventario", params);
}

function ADJUDICACION_GoNewIncidencia() {
    var params = new Array();
    params.push("&AdjudicacionIdparam=" + Data.Id);
    GoEncryptedItemView("Actuacion", -1, "Custom", params);
}

function ADJUDICACION_GoNewPresupuesto() {
    var params = new Array();
    params.push("&AdjudicacionIdparam=" + Data.Id);
    GoEncryptedItemView("Presupuesto", -1, "PresupuestoByAdjudicacion", params);
    return false;
}

function ADJUDICACION_GoNewOT() {
    var params = new Array();
    params.push("&AdjudicacionIdparam=" + Data.Id);

    var destForm = "Custom"; //valor por defecto
    if (Data.Tipo === 1) {
        destForm = "OTViaPublica";
    }
    else if (Data.Tipo === 2) {
        destForm = "OTEdificio";
    }

    GoEncryptedItemView("Actuacion", -1, destForm, params);
    return false;
}

function ADJUDICACION_RenderEstadoContrata() {
    $("#Adjudicacion5 hr").hide();
    $("#NoCertificacion").remove();
    $("#CertificacionReport").remove();
    $("#CertificacionReport2").remove();
    var table = $("#datatable_col_reorder_Certificacion").DataTable();
    var tableData = table.data();
    var total = Data.Importe;
    var resto = 0;
    var dataLabel = new Array();
    var dataFet = new Array();
    var dataPendent = new Array();
    var dataMes = new Array();
    if (tableData.length > 0) {
        var res = "<div class=\"row\" id=\"CertificacionReport\"><div class=\"col col-sm-12\"><table style=\"width:100%;\" class=\"table table-hover\">";
        res += "<thead>";
        res += "    <tr>";
        res += "        <th style=\"text-align:right;\">N<sup>o</sup> OT's</th>";
        res += "        <th>Mes</th>";
        res += "        <th style=\"text-align:right;\">PEM</th>";
        res += "        <th style=\"text-align:right;\">PEC sense IVA</th>";
        res += "        <th style=\"text-align:right;\">IVA (" + Data.IVA + "%)</th>";
        res += "        <th style=\"text-align:right;\">PEC amb IVA</th>";
        res += "        <th style=\"text-align:right;\">% Certificat</th>";
        res += "    </tr>";
        res += "</thead>";
        var validMonth = 0;

        var totalAccumulated = {
            "NOTs": 0,
            "Total": 0,
            "PECsinIVA": 0,
            "PECIVA": 0,
            "IVA": 0
        };

        var monthName = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];
        var finalData = [];
        var iniMonth = new Date(Data.Inicio).getMonth();
        var iniYear = new Date(Data.Inicio).getFullYear();
        var finMonth = new Date(Data.Final).getMonth();
        var finYear = new Date(Data.Final).getFullYear();
        var totalMonths = (finYear - iniYear) * 12 + (finMonth - iniMonth) + 1;
        // console.log(Data.Inicio, Data.Final);

        // Poner los datos anteriores al periodo
        for (var x = 0; x < tableData.length; x++) {
            if (tableData[x].DateYear < iniYear) {
                finalData.push(tableData[x]);
            }
            else {
                if (tableData[x].DateYear === iniYear && tableData[x].DateMonth < iniMonth) {
                    finalData.push(tableData[x]);
                }
            }
        }

        var actualDate = new Date().getFullYear() * 12 + new Date().getMonth();
        while (iniYear * 12 + iniMonth <= finYear * 12 + finMonth) {
            var actualMonthName = monthName[iniMonth] + " / " + iniYear;
            var found = false;
            for (var yx = 0; yx < tableData.length; yx++) {
                if (tableData[yx].Month === actualMonthName) {
                    finalData.push(tableData[yx]);
                    found = true;
                    break;
                }
            }

            if (found === false) {
                finalData.push(
                    {
                        "AdjudicacionId": Data.Id,
                        "Month": actualMonthName,
                        "NOTs": 0,
                        "Pendiente": 0,
                        "Total": 0,
                        "DateMonth": iniMonth,
                        "DateYear": iniYear
                    });
            }

            iniMonth++;
            if (iniMonth > 11) {
                iniMonth = 0;
                iniYear++;
            }

            if (iniYear * 12 + iniMonth > actualDate) {
                break;
            }
        }

        tableData = finalData;

        for (var x = 0; x < tableData.length; x++) {
            if (x > 0 && tableData[x].Pendiente === 0) {
                tableData[x].Pendiente = tableData[x - 1].Pendiente
            }

            var PEM = tableData[x].Total;
            var SIS = Math.round(PEM * ADJUDICACION_Context.Security * 100) / 10000;
            var PEMSIS = PEM + SIS;
            var Baixa = Math.round(PEMSIS * ADJUDICACION_Context.Baja * 100) / 10000;
            var PEMBaixa = PEMSIS - Baixa;
            var DGBI = Math.round(PEMBaixa * ADJUDICACION_Context.DGBI * 100) / 10000;
            var PECsinIVA = PEMBaixa + DGBI;
            var IVA = Math.round(PECsinIVA * ADJUDICACION_Context.IVA * 100) / 10000;
            var PECIVA = PECsinIVA + IVA;
            var percent = Math.round(tableData[x].Pendiente / total * 10000) / 100;

            var background = "background: -webkit-linear-gradient(left, #77e298 " + percent + "%, #f4d2d2 " + percent + "%); background: -moz-linear-gradient(left, #77e298 " + percent + "%, white " + percent + "%);";
            if (percent > 100) {
                background = "background-color:#f00;color:#ff0;font-weight:bold;";
            }

            res += "<tr>";
            res += "<td align=\"right\">" + tableData[x].NOTs + "</td>";
            res += "<td>" + tableData[x].Month + "</td>";
            res += "<td align=\"right\"><strong>" + ToMoneyFormat(tableData[x].Total, 2) + "&nbsp;&euro;</strong></td>";
            res += "<td align=\"right\">" + ToMoneyFormat(PECsinIVA, 2) + "&nbsp;&euro;</td>";
            res += "<td align=\"right\">" + ToMoneyFormat(IVA, 2) + "&nbsp;&euro;</td>";
            res += "<td align=\"right\">" + ToMoneyFormat(PECIVA, 2) + "&nbsp;&euro;</td>";
            res += "<td align=\"right\" style=\"" + background + "\">" + ToDecimalFormat(percent, 2) + " %</td>";
            res += "</tr>";
            resto = tableData[x].Pendiente;

            totalAccumulated.NOTs += tableData[x].NOTs;
            totalAccumulated.Total += tableData[x].Total;
            totalAccumulated.PECsinIVA += PECsinIVA;
            totalAccumulated.IVA += IVA;
            totalAccumulated.PECIVA += PECIVA;

            if (tableData[x].Month.indexOf("enero") !== -1 || tableData[x].Month.indexOf("gener") !== -1) {
                if (validMonth === 0) {
                    validMonth = 1;
                }
                else {
                    validMonth++;
                }
            }
            else {
                if (validMonth > 0) {
                    validMonth++;
                }
            }

            dataMes.push(tableData[x].Total);
            dataLabel.push(tableData[x].Month);
            dataFet.push(tableData[x].Pendiente);

            if (validMonth === 0) {
                dataPendent.push(null);
            }
            else {
                dataPendent.push(Math.round(Data.Importe / totalMonths * validMonth * 100) / 100);
            }
        }

        res += "<tr>";
        res += "<td align=\"right\" style=\"font-weight:bold;\">" + totalAccumulated.NOTs + "</td>";
        res += "<td align=\"right\">&nbsp;</td>";
        res += "<td align=\"right\" style=\"font-weight:bold;\"><strong>" + ToMoneyFormat(totalAccumulated.Total, 2) + "&nbsp;&euro;</strong></td>";
        //res += "<td align=\"right\">" + ToMoneyFormat(SIS, 2) + "&nbsp;&euro;</td>";
        //res += "<td align=\"right\">" + ToMoneyFormat(PEMSIS, 2) + "&nbsp;&euro;</td>";
        //res += "<td align=\"right\">" + ToMoneyFormat(Baixa, 2) + "&nbsp;&euro;</td>";
        //res += "<td align=\"right\">" + ToMoneyFormat(PEMBaixa, 2) + "&nbsp;&euro;</td>";
        //res += "<td align=\"right\">" + ToMoneyFormat(DGBI, 2) + "&nbsp;&euro;</td>";
        res += "<td align=\"right\" style=\"font-weight:bold;\">" + ToMoneyFormat(totalAccumulated.PECsinIVA, 2) + "&nbsp;&euro;</td>";
        res += "<td align=\"right\" style=\"font-weight:bold;\">" + ToMoneyFormat(totalAccumulated.IVA, 2) + "&nbsp;&euro;</td>";
        res += "<td align=\"right\" style=\"font-weight:bold;\">" + ToMoneyFormat(totalAccumulated.PECIVA, 2) + "&nbsp;&euro;</td>";
        //res += "<td align=\"right\">" + ToMoneyFormat(tableData[x].Pendiente, 2) + "&nbsp;&euro;</td>";
        res += "<td>&nbsp;</td>";
        res += "</tr>";

        //console.log("dataLabel", dataLabel);

        res += "</table>";
        res += "</div>";

        res += "</div>";
        res += "<hr />";
        res += "<div class=\"row\" id=\"CertificacionReport2\" style=\"margin-top:12px;\">";

        // -------------- GRAFICO PASTEL
        res += "<div class=\"col col-sm-3\" style=\"text-align:center;\">";
        res += "<h4><strong>Percentatge executat</strong></h4><br /><br /><canvas id=\"pieChart\"></canvas>"
        res += "</div> ";
        //----------------------------------

        // -------------- GRAFICO BARRAS
        res += "<div class=\"col col-sm-9\" style=\"text-align:center;\"id=\"CertificacionReport2\">";
        res += "<h4><strong>Evolució mensual</strong></h4><br /><br />";
        res += "<canvas id=\"barChart\" style=\"height:300px;\"></canvas>"
        res += "</div>";
        //----------------------------------

        res += "</div>";

        $("#datatable_col_reorder_Certificacion_wrapper").after(res);
        $("#SaldoInput").val(ToMoneyFormat(total - resto, 2) + " €");

        // console.clear();
        setTimeout(function () { ADJUDICACION_EstadoContrataGraphics(resto, total, dataFet, dataPendent, dataMes, dataLabel); }, 500);
    }
    else {
        $("#datatable_col_reorder_Certificacion_wrapper").after("<h4 id=\"NoCertificacion\">No hi ha dades de certificació</h4>");
    }
}

function ADJUDICACION_EstadoContrataGraphics(resto, total, dataFet, dataPendent, dataMes, dataLabel) {
    if ($("#Adjudicacion5").css("display") === "none") { return; }
    var restoValue = resto * 100 / total
    var pendentValue = (total - resto) * 100 / total;
    var data = [
        {
            "value": restoValue,
            "color": "#77e298",
            "highlight": "rgba(220,220,220,0.8)",
            "label": "Fet"
        },
        {
            "value": pendentValue,
            "color": "#46BFBD",
            "highlight": "#5AD3D1",
            "label": "Green"
        }
    ];

    if (restoValue > 100) {
        pendentValue = 0;
        data = [
            {
                "value": restoValue,
                "color": "#ee1111",
                "highlight": "rgba(220,20,20,0.8)",
                "label": "Fet"
            }
        ];
    }

    var canvas = document.getElementById("pieChart");
    var ctx = canvas.getContext("2d");
    var midX = canvas.width / 2;
    var midY = canvas.height / 2

    // Create a pie chart
    var myPieChart = new Chart(ctx).Pie(data, {
        showTooltips: false,
        onAnimationProgress: drawSegmentValues
    });

    var radius = myPieChart.outerRadius;

    function drawSegmentValues() {
        for (var i = 0; i < myPieChart.segments.length; i++) {
            ctx.fillStyle = "black";
            var textSize = canvas.width / 15;
            ctx.font = textSize + "px Verdana";

            // Get needed variables
            var value = myPieChart.segments[i].value;
            value = ToMoneyFormat(value, 2) + "%";
            var startAngle = myPieChart.segments[i].startAngle;
            var endAngle = myPieChart.segments[i].endAngle;
            var middleAngle = startAngle + ((endAngle - startAngle) / 2);

            // Compute text location
            var posX = (radius / 2) * Math.cos(middleAngle) + midX;
            var posY = (radius / 2) * Math.sin(middleAngle) + midY;

            // Text offside by middle
            var w_offset = ctx.measureText(value).width / 2;
            var h_offset = textSize / 4;


            ctx.fillText(value, posX - w_offset, posY + h_offset);
        }
    }

    // ---------- BARRAS
    var barOptions = {
        "scaleBeginAtZero": true,
        "scaleShowGridLines": true,
        "scaleGridLineColor": "rgba(0,0,0,.05)",
        "scaleGridLineWidth": 1,
        "barShowStroke": true,
        "barStrokeWidth": 1,
        "barValueSpacing": 5,
        "barDatasetSpacing": 1,
        "responsive": true,
        "legendTemplate": "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"
    }

    console.log(dataMes);
    console.log("data mes", dataMes);
    console.log("data fet", dataFet);
    console.log("data pendent", dataPendent);

    var overlayData = {
        labels: dataLabel,
        datasets: [
            {
                "label": "Previsió",
                "type": "line",
                "fillColor": "rgba(244,210,210,0.25)",
                "strokeColor": "rgba(216,33,0,0.8)",
                "highlightFill": "rgba(151,187,205,0.75)",
                "highlightStroke": "rgba(151,187,205,1)",
                "data": dataPendent
            }, {
                "label": "Realitzat",
                "type": "bar",
                "fillColor": "rgba(119,226,152,0.5)",
                "strokeColor": "rgba(59,183,38,0.8)",
                "highlightFill": "rgba(59,183,38,0.75)",
                "highlightStroke": "rgba(94,114,95,1)",
                "data": dataFet
            },
            {
                "label": "Mensual",
                "fillColor": "#275b89",
                "strokeColor": "rgba(77,110,240,0.8)",
                "highlightFill": "rgba(77,100,240,0.75)",
                "highlightStroke": "rgba(77,100,240,1)",
                "scaleStep": total / 50,
                "data": dataMes
            }
        ]
    };

    window.myOverlayChart = new Chart(document.getElementById("barChart").getContext("2d")).Overlay(overlayData, {
        "populateSparseData": true,
        "overlayBars": false,
        "datasetFill": true,
    });
}

function afterdataTableSuccess(tableName) {
    //console.log("ADJUDICACION afterdataTableSuccess", tableName);

    if (tableName === "datatable_col_reorder_Certificacion") {
        //console.log("Importe", Data.Importe);
        ADJUDICACION_RenderEstadoContrata();
    }
    if (tableName === "datatable_col_reorder_Certificaciones") {
        console.log("datatable_col_reorder_Certificaciones");
        $("#datatable_col_reorder_Certificaciones .BtnDelete").hide();
        $("#datatable_col_reorder_Certificaciones .btn-edit").each(function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var row = table.row(this.parentNode.parentNode.parentNode);
            var data = row.data();
            console.log(data);
            //this.id = "item" + data.Id;
            $(this).parent().html("<a id=\"Actuacion_Certificaciones\" onclick=\"ADJUDICACION_SaveCertificacion(this);\" class=\" btn-edit\"><span><i class=\"fa fa-save\"></i></span></a>");
        });
    }

    // se comprueba que es la tabla que toca por si hay mas en el formulario
    if (tableName === "datatable_col_reorder_OTViaPublica") {
        var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
        table.column(ADJUDICACION_OTList_Columns.Urgente).visible(false);

        if (GrantDelete("Actuacion") === false) {
            $("#datatable_col_reorder_OTViaPublica .BtnDelete").hide();
        }

        // Se elimina el evento de BtnDelete que se gestiona a continuación
        $("#datatable_col_reorder_OTViaPublica .BtnDelete").each(function (index) {
            $(this).removeAttr("onclick");
            $(this).removeAttr("data-target");
            $(this).removeAttr("data-toggle");
            if ($(this).parent().html().indexOf("Print") === -1) {
                $(this).after("<a style=\"margin-left:5px;\" class=\"BtnPrint\"><i class=\"fa fa-print\"></i></a>");
            }
        });

        $("#datatable_col_reorder_OTViaPublica td .BtnDelete").on("click", function (e) {
            console.log(e);
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            ACTUACION_DeleteActuacion(data, "OT");
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_OTViaPublica tbody").unbind("click").on("click", "td .BtnPrint", function (e) {
            console.log(e);
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            window.open("/CustomersFramework/Constraula/Pages/PDFActuacionData.aspx?id=" + data.Id + "&tipo=OT&userId=" + User.Id);
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(1)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(2)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(3)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(4)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(5)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(6)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(7)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(8)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(9)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(10)", function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTViaPublica", null);
        });

        //por cada boton de editar de la tabla
        $("#datatable_col_reorder_OTViaPublica .btn-edit").each(function () {
            var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
            var row = table.row(this.parentNode.parentNode.parentNode);
            var data = row.data();
            //if (data.Latitud === null) { this.parentNode.lastChild.style.display = 'none'; }
            if (data.Urgente === true && (data.EstadoOTId === 7 || data.EstadoOTId === 6 || data.EstadoOTId === 2 || data.EstadoOTId === 1)) {
                for (var x = 0; x < this.parentNode.childNodes.length; x++) {
                    this.parentNode.childNodes[x].style.color = "#ff0";
                }

                for (y = 0; y < this.parentNode.childNodes[10].childNodes; y++) {
                    this.parentNode.childNodes[10].childNodes[y].style.color = "#ff0";
                }
            }
            var xx = "GoEncryptedItemView('Actuacion', " + data.Id + ", 'OTViaPublica');";
            this.id = "item" + data.Id;
            $("#" + this.id).removeAttr("onclick");
            $("#" + this.id).attr("onclick", xx);
        });

        $("#datatable_col_reorder_OTViaPublica TR").css("cursor", "hand");

        if (typeof mapOT !== "undefined") {
            console.log("Poner markers OT");
            RenderMarkersOTs();
        }

        tableOTLoaded = true;
    }

    if (tableName === "datatable_col_reorder_AdjudicacionCentros") {
        // Se elimina el evento de BtnDelete que se gestiona a continuación
        $("#datatable_col_reorder_AdjudicacionCentros .BtnDelete").each(function (index) {
            $(this).removeAttr("onclick");
        });

        $("#datatable_col_reorder_AdjudicacionCentros .BtnDelete").hide();

        $("#datatable_col_reorder_AdjudicacionCentros tbody").on("click", "tr", function () {
            var table = $("#datatable_col_reorder_AdjudicacionCentros").DataTable();
            var data = table.row(this).data();
            GoEncryptedItemView("Centro", data.Id, "Custom");
        });

        //por cada boton de editar de la tabla
        $("#datatable_col_reorder_AdjudicacionCentros .btn-edit").each(function () {
            var table = $("#datatable_col_reorder_AdjudicacionCentros").DataTable();
            var row = table.row(this.parentNode.parentNode.parentNode);
            var data = row.data();
            this.id = "item" + data.Id;
            $("#" + this.id).removeAttr("onclick");
            $("#" + this.id).attr("onclick", "GoEncryptedItemView(\"Centro\", " + data.Id + ", \"Custom\");");
        });

        if (typeof mapEdifici !== "undefined") {
            console.log("Poner markers edificions");
            RenderMarkersEdificis();
        }

        tableEdificiLoaded = true;
    }

    if (tableName === "datatable_col_reorder_Inventario") {
        var table = $("#datatable_col_reorder_Inventario").DataTable();

        if (GrantDelete("Actuacion") === false) {
            $("#datatable_col_reorder_Inventario .BtnDelete").hide();
        }

        // Se elimina el evento de BtnDelete que se gestiona a continuación
        $("#datatable_col_reorder_Inventario .BtnDelete").each(function (index) {
            $(this).removeAttr("onclick");
            $(this).removeAttr("data-target");
            $(this).removeAttr("data-toggle");
            if ($(this).parent().html().indexOf("Print") === -1) {
                $(this).after("<a style=\"margin-left:5px;\" class=\"BtnPrint\"><i class=\"fa fa-print\"></i></a>");
            }
        });

        $("#datatable_col_reorder_Inventario td .BtnDelete").unbind("click").on("click", function (e) {
            console.log(e);
            var table = $("#" + tableName).DataTable();
            var data = table.row($(this).parents("tr")).data();
            ACTUACION_DeleteActuacion(data, "Inventari");
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_Inventario tbody").unbind("click").on("click", "td .BtnPrint", function (e) {
            console.log(e);
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            window.open("/CustomersFramework/Constraula/Pages/PDFActuacionData.aspx?id=" + data.Id + "&tipo=Inventario&userId=" + User.Id);
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_Inventario tbody").on("click", "td:nth-child(1)", function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });
        $("#datatable_col_reorder_Inventario tbody").on("click", "td:nth-child(2)", function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });
        $("#datatable_col_reorder_Inventario tbody").on("click", "td:nth-child(3)", function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });
        $("#datatable_col_reorder_Inventario tbody").on("click", "td:nth-child(4)", function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });
        $("#datatable_col_reorder_Inventario tbody").on("click", "td:nth-child(5)", function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });
        $("#datatable_col_reorder_Inventario tbody").on("click", "td:nth-child(6)", function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });
        $("#datatable_col_reorder_Inventario tbody").on("click", "td:nth-child(7)", function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });

        //por cada boton de editar de la tabla
        $("#datatable_col_reorder_Inventario .btn-edit").each(function () {
            var table = $("#datatable_col_reorder_Inventario").DataTable();
            var row = table.row(this.parentNode.parentNode.parentNode);
            var data = row.data();
            var xx = "GoEncryptedItemView('Actuacion', " + data.Id + ", 'Inventario');";
            this.id = "item" + data.Id;
            $("#" + this.id).removeAttr("onclick");
            $("#" + this.id).attr("onclick", xx);
        });

        if (typeof mapInventari !== "undefined") {
            console.log("Poner markers Inventario");
            RenderMarkersInventario();
        }

        tableInventarioLoaded = true;
    }

    if (tableName === "datatable_col_reorder_Actuacion") {
        if (GrantDelete("Actuacion") === false) {
            $("#datatable_col_reorder_Actuacion .BtnDelete").hide();
        }

        // Se elimina el evento de BtnDelete que se gestiona a continuación
        $("#datatable_col_reorder_Actuacion .BtnDelete").each(function (index) {
            $(this).removeAttr("onclick");
            $(this).removeAttr("data-target");
            $(this).removeAttr("data-toggle");
            if ($(this).parent().html().indexOf("Print") === -1) {
                $(this).after("<a style=\"margin-left:5px;\" class=\"BtnPrint\"><i class=\"fa fa-print\"></i></a>");
            }
        });

        $("#datatable_col_reorder_Actuacion td .BtnDelete").on("click", function (e) {
            console.log(e);
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            ACTUACION_DeleteActuacion(data, "OT");
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_Actuacion tbody").unbind("click").on("click", "td .BtnPrint", function (e) {
            console.log(e);
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            window.open("/CustomersFramework/Constraula/Pages/PDFActuacionData.aspx?id=" + data.Id + "&tipo=OTEdificio&userId=" + User.Id);
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_Actuacion tbody").on("click", "td:nth-child(1)", function () {
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTEdificio", null);
        });
        $("#datatable_col_reorder_Actuacion tbody").on("click", "td:nth-child(2)", function () {
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTEdificio", null);
        });
        $("#datatable_col_reorder_Actuacion tbody").on("click", "td:nth-child(3)", function () {
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTEdificio", null);
        });
        $("#datatable_col_reorder_Actuacion tbody").on("click", "td:nth-child(4)", function () {
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTEdificio", null);
        });
        $("#datatable_col_reorder_OTViaPublica tbody").on("click", "td:nth-child(5)", function () {
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "OTEdificio", null);
        });

        //por cada boton de editar de la tabla
        $("#datatable_col_reorder_Actuacion .btn-edit").each(function () {
            var table = $("#datatable_col_reorder_Actuacion").DataTable();
            var row = table.row(this.parentNode.parentNode.parentNode);
            var data = row.data();
            //if (data.Latitud === null) { this.parentNode.lastChild.style.display = 'none'; }
            if (data.Urgente === true && (data.EstadoOTId === 7 || data.EstadoOTId === 6 || data.EstadoOTId === 2 || data.EstadoOTId === 1)) {
                for (var x = 0; x < this.parentNode.childNodes.length; x++) {
                    this.parentNode.childNodes[x].style.color = "#ff0";
                }

                for (y = 0; y < this.parentNode.childNodes[10].childNodes; y++) {
                    this.parentNode.childNodes[10].childNodes[y].style.color = "#ff0";
                }
            }
            var xx = "GoEncryptedItemView('Actuacion', " + data.Id + ", 'OTEdificio');";
            this.id = "item" + data.Id;
            $("#" + this.id).removeAttr("onclick");
            $("#" + this.id).attr("onclick", xx);
        });

        $("#datatable_col_reorder_Actuacion TR").css("cursor", "hand");

        tableOTLoaded = true;
        RenderMarkersEdificis();
    }

    if (tableName === "datatable_col_reorder_AdjudicacionAmpliacion") {
        var res = 0;
        var table = $("#datatable_col_reorder_AdjudicacionAmpliacion").DataTable();
        table_AdjudicacionAmpliacion = table;
        var data = table.rows().data();
        console.log("datatable_col_reorder_AdjudicacionAmpliacion", data);
        for (var x = 0; x < data.length; x++) {
            res += data[x].Importe;
        }

        Data.Importe = res;

        var table = $("#datatable_col_reorder_AdjudicacionAmpliacion").DataTable();

        if (GrantDelete("Actuacion") === false) {
            $("#datatable_col_reorder_AdjudicacionAmpliacion .BtnDelete").hide();
        }

        // Se elimina el evento de BtnDelete que se gestiona a continuación
        $("#datatable_col_reorder_AdjudicacionAmpliacion .BtnDelete").each(function (index) {
            $(this).removeAttr("onclick");
            $(this).removeAttr("data-target");
            $(this).removeAttr("data-toggle");
        });

        $("#datatable_col_reorder_AdjudicacionAmpliacion td .BtnDelete").unbind("click").on("click", function (e) {
            console.log(e);
            var table = $("#" + tableName).DataTable();
            var data = table.row($(this).parents("tr")).data();
            ACTUACION_DeleteAmpliacion(data);
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_AdjudicacionAmpliacion tbody").on("click", "td:nth-child(1)", function () {
            var table = $("#datatable_col_reorder_AdjudicacionAmpliacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });

        $("#datatable_col_reorder_AdjudicacionAmpliacion tbody").on("click", "td:nth-child(2)", function () {
            var table = $("#datatable_col_reorder_AdjudicacionAmpliacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });

        $("#datatable_col_reorder_AdjudicacionAmpliacion tbody").on("click", "td:nth-child(3)", function () {
            var table = $("#datatable_col_reorder_AdjudicacionAmpliacion").DataTable();
            var data = table.row($(this).parents("tr")).data();
            GoEncryptedItemView("Actuacion", data.Id, "Inventario", null);
        });

        //por cada boton de editar de la tabla
        $("#datatable_col_reorder_AdjudicacionAmpliacion .btn-edit").each(function () {
            var table = $("#datatable_col_reorder_AdjudicacionAmpliacion").DataTable();
            var row = table.row(this.parentNode.parentNode.parentNode);
            var data = row.data();
        });

        if (GetGrant("AdjudicacionAmpliacion").Write === false) {
            $("#datatable_col_reorder_AdjudicacionAmpliacion .btn-edit").hide();
        }

        ADJUDICACION_RenderEconomics();
    }
}

function createdRow_Inventario(row, data, index) {
    if (typeof data.Color !== "undefined" && data.Color !== null) {
        if (data.Color !== "") {
            $(row).css("background-color", data.Color);
        }
    }

    $(row).addClass("pointer");
}

function createdRow_OTViaPublica(row, data, index) {
    if (data.U === true && (data.EstadoOTId === 7 || data.EstadoOTId === 6 || data.EstadoOTId === 2 || data.EstadoOTId === 1)) {
        $(row).css("background-color", "#f33");
        $(row).css("color", "#ff0");
    }
    else if (typeof data.Color !== "undefined" && data.Color !== null) {
        if (data.Color !== "") {
            $(row).css("background-color", data.Color);
        }
    }

    $(row).addClass("pointer");
}

function createdRow_Actuacion(row, data, index) {
    if (typeof data.Color !== "undefined" && data.Color !== null) {
        if (data.Color !== "") {
            $(row).css("background-color", data.Color);
        }
    }

    $(row).addClass("pointer");
}

function RenderMarkersEdificis() {
    if (tableEdificiLoaded === false) { return; }
    console.log("RenderMarkersEdificis");
    var tableData = $("#datatable_col_reorder_AdjudicacionCentros").DataTable();
    //console.log(table);
    //var tableData = table.data();
    var mapData = tableData.rows({ filter: "applied" }).data();
    for (var x = 0; x < markers.length; x++) {
        mapEdifici.removeLayer(markers[x]);
    }

    // Además de quitar los markers del mapa hay que vaciar la lista
    markers = new Array();

    var centered = false;

    for (var x = 0; x < mapData.length; x++) {
        var data = mapData[x];
        if (data.Latitud !== null && data.Longitud !== null) {
            if (centered === false) {
                if (x === 0) {
                    mapEdifici.setView([data.Latitud, data.Longitud], 14);
                    centered = true;
                }
            }

            var tipologiaCentro = "";
            if (typeof data.TCD !== "undefined" && data.TCD !== null) {
                tipologiaCentro = "&nbsp;(" + data.TCD + ")";
            }

            var mapIcon = L.icon({
                "iconUrl": "/marker-icon.png",
                "iconSize": [25, 40],
                "iconAnchor": [-12, 40],
                "popupAnchor": [-3, -40]
            });

            var itemViewUrl = "GoEncryptedItemView(\"Centro\"," + data.Id + ", \"Custom\")";

            var marker = null;
            if (mapIcon !== null) {
                marker = L.marker([data.Latitud, data.Longitud], { icon: mapIcon });
            }
            else {
                marker = L.marker([data.Latitud, data.Longitud]);
            }

            marker.properties = {};
            marker.properties.TipologiaCentroId = data.TipologiaCentroId * 1;
            var foto = "/CustomersFramework/Constraula/Data/Centro_Fachada_" + Data.Id + ".png";
            console.log("Foto", foto);

            var Le = " - ";
            var Pr = " - ";

            if (data.L !== null) { Le = data.L; }
            if (data.P !== null) { Pr = data.P; }

            var popupContent = "<div onclick=\"GoEncryptedItemView(\"Centro\", " + data.Id + ", \"Custom\")\"><span style=\"cursor:pointer;\">Edifici:<strong>" + data.Codigo + " - " + data.Descripcion + "</strong><br />" + data.Direccion + tipologiaCentro;
            popupContent += "<div class=\"row\">";
            popupContent += "  <div class=\"col-sm-6 mapPopupImg\"><img id=\"FotoEdifici" + data.Id + "\" src=\"" + foto + "\" style=\"width:100%;height:auto;\" onerror=\"ADJUDICACION_ReparePhotoOT(this)\" /></div>";
            popupContent += "  <div class=\"col-sm-6 \">Cert.Legionela:<br/>" + Le + "<br /<br />Mt.Preventiu:<br/>" + Pr + "</div>";
            popupContent += "</row>";
            popupContent += "</span></div>";

            marker.bindPopup(popupContent);

            markers.push(marker);
        }
    }

    for (var x = 0; x < markers.length; x++) {
        markers[x].addTo(mapEdifici);
    }

    if (mapEdifici !== null) { mapEdifici._onResize(); }
}

function RenderMapEdificis() {
    // Evita crear el mapa sin contenedor
    if ($("#map").length === 0) { return; }

    // Evita reinicializar el mapa
    if (mapEdifici !== null) { return; }

    //console.log("RenderMapEdificis");
    var containerHeight = $(window).height();
    var mapHeight = (containerHeight - 260) + "px";
    $("#map").css("height", mapHeight);

    mapEdifici = L.map("map", {
        center: [41.4475097, 2.1598185],
        zoom: 11
    });

    // Capa offline
    L.tileLayer("http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", { minZoom: 1, maxZoom: 18, attribution: "&copy; <a href=\"http://www.sbrinna.com\">Sbrinna</a> contributors" }).addTo(mapEdifici);
    $(".leaflet-control-attribution").hide();

    var filter = L.control({ position: "topright" });

    filter.onAdd = function (mapEdifici) {
        this._div = L.DomUtil.create("div", "info");
        this.update();
        return this._div;
    };

    filter.update = function (props) {
        resFilter = "<h4><strong>Tipologia</strong></h4>";
        resFilter += "<div id=\"TipologiaCentrosDIV\">";
        for (var x = 0; x < FK.TipologiaCentros.length; x++) {
            resFilter += "<input type=\"checkbox\" id=\"TipologiaCentros" + FK.TipologiaCentros[x].Id + "\" checked=\"checked\" onclick=\"EdificisFilterMarkers();\" />&nbsp;" + FK.TipologiaCentros[x].Description + "<br />";
        }

        resFilter += "</div>";
        this._div.innerHTML = resFilter;
    };

    filter.addTo(mapEdifici);
}

function EdificisFilterMarkers() {
    console.log("EdificisFilterMarkers");

    var TipologiaCentros = new Array();
    $("#TipologiaCentrosDIV input").each(function () {
        if (this.checked === true) {
            var id = this.id.split("TipologiaCentros")[1] * 1;
            TipologiaCentros.push(id);
        }
    });

    for (var x = 0; x < markers.length; x++) {
        mapEdifici.removeLayer(markers[x]);
    }

    for (var x = 0; x < markers.length; x++) {
        var show = true;
        if ($.inArray(markers[x].properties.TipologiaCentroId, TipologiaCentros) === -1) { show = false; }
        if (show === true) { markers[x].addTo(mapEdifici); }
    }
}

function RenderMarkersOTs() {
    var table = $("#datatable_col_reorder_OTViaPublica").DataTable();
    // var mapData = table.data();
    var mapData = table.rows({ filter: "applied" }).data();
    for (var x = 0; x < markers.length; x++) {
        mapOT.removeLayer(markers[x]);
    }

    // Además de quitar los markers del mapa hay que vaciar la lista
    markers = new Array();

    var centered = false;
    for (var x = 0; x < mapData.length; x++) {
        var data = mapData[x];
        if (data.Latitud !== null && data.Longitud !== null) {
            if (centered === false) {
                if (x === 0) {
                    mapOT.setView([data.Latitud, data.Longitud], 14);
                    centered = true;
                }
            }

            var elemento = "";
            var problema = "";
            var urgente = false;

            if (typeof data.Categoria !== "undefined" && data.Categoria) {
                elemento = data.Categoria;
            }

            if (typeof data.NivelUrgencia !== "undefined" && data.NivelUrgencia !== null) {
                problema = data.NivelUrgencia;
            }

            if (typeof data.Urgente !== "undefined" && data.Urgente !== null) {
                urgente = data.Urgente;
            }

            var xxx = elemento + " " + problema;


            var mapIcon = L.icon({
                "iconUrl": "/CustomersFramework/Constraula/Data/img/ICONO_icono_" + data.IconoId + ".png",
                "iconSize": [40, 40],
                "iconAnchor": [30, 40],
                "popupAnchor": [-10, -40]
            });

            if (data.IconoId === null) {
                mapIcon = L.icon({
                    "iconUrl": "/CustomersFramework/Constraula/Data/img/Icono_Icono_0.png",
                    "iconSize": [40, 40],
                    "iconAnchor": [30, 40],
                    "popupAnchor": [-10, -40]
                });
            }

            var itemViewUrl = "GoEncryptedItemView('Actuacion', " + data.Id + ", 'OTViaPublica')";

            var marker = null;
            if (mapIcon !== null) {
                marker = L.marker([data.Latitud, data.Longitud], { icon: mapIcon });
            }
            else {
                marker = L.marker([data.Latitud, data.Longitud]);
            }

            var direccion = data.D;
            var foto = "http://ctman.constraula.com/CustomersFramework/Constraula/Data/PhotoGallery/" + Data.Id + "/" + data.O + ".jpg";
            var fotoFin = "http://ctman.constraula.com/CustomersFramework/Constraula/Data/PhotoGallery/" + Data.Id + "/" + data.O + "-ok.jpg";

            marker.properties = {
                "BarrioId": data.BarrioId,
                "CategoriaId": data.CategoriaId,
                "NivelUrgenciaId": data.NivelUrgenciaId,
                "Urgente": data.U,
                "EstadoOTId": data.EstadoOTId
            };

            var popupContent = "<span class=\"mapPopup\" style=\"cursor:pointer;\" onclick=\"" + itemViewUrl + "\">";
            popupContent += "OT: <strong>" + data.O + "</strong><br /><strong>" + direccion + "</strong><br />" + xxx + "<br/>";
            popupContent += "<div class=\"row\">";
            popupContent += "  <div class=\"col-sm-6 mapPopupImg\"><img id=\"FotoOT" + data.Id + "\" src=\"" + foto + "\" style=\"width:100%;height:auto;\" onerror=\"ADJUDICACION_ReparePhotoOT(this)\" /></div>";
            popupContent += "  <div class=\"col-sm-6 mapPopupImg\"><img id=\"FotoOTFin" + data.Id + "\" src=\"" + fotoFin + "\" style=\"width:100%;height:auto;\" onerror=\"ADJUDICACION_ReparePhotoOT(this)\" /></div>";
            popupContent += "</row>";
            popupContent += "</span>";
            marker.bindPopup(popupContent);
            markers.push(marker);
        }
    }

    for (var z = 0; z < markers.length; z++) { markers[z].addTo(mapOT); }
    if (mapOT !== null) { mapOT._onResize(); }
}

function RenderMapOTs() {
    // Evita crear el mapa sin contenedor
    if ($("#map").length === 0) {
        console.log("RenderMapOTs", "no hay mapa");
        return;
    }

    // Evita reinicializar el mapa
    if (mapOT !== null) {
        console.log("RenderMapOTs", "El mapa ya existe");
        return;
    }

    //console.log("RenderMapOTs");
    var containerHeight = $(window).height();
    var mapHeight = (containerHeight - 310) + "px";
    $("#map").css("height", mapHeight);

    mapOT = L.map("map", {
        "center": [41.4475097, 2.1598185],
        "zoom": 13
    });

    // Capa offline
    L.tileLayer("http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", { minZoom: 1, maxZoom: 18, attribution: "&copy; <a href=\"http://www.sbrinna.com\">Sbrinna</a> contributors" }).addTo(mapOT);
    $(".leaflet-control-attribution").hide();

    var filter = L.control({ position: "topright" });

    filter.onAdd = function (map) {
        this._div = L.DomUtil.create("div", "info");
        this.update();
        return this._div;
    };

    filter.update = function (props) {
        resFilter = "<h4 onclick=\"$('#CategoriasDIV').slideToggle()\"><strong>Elements</strong></h4>";

        resFilter += "<div id=\"CategoriasDIV\">";
        /*for (var x = 0; x < FK.Categoria.length; x++) {
            if (FK.Categoria[x].Active === true) {
                if (FormId === "AdjudicacionViaPublica" && FK.Categoria[x].TipoAdjudicacion == 1) {
                    var icono = GetByFieldFromList(FK.IconoOT, "CategoriaId", FK.Categoria[x].Id);
                    var iconImg = "";
                    if (icono !== null) {
                        iconImg = "&nbsp;<img src=\"" + icono.Icono + "\" style=\"margin-top:-8px;width:24px;\" />";
                    }
                    resFilter += "<input type=\"checkbox\" id=\"Categoria" + FK.Categoria[x].Id + "\" checked=\"checked\" onclick=\"OTFilterMarkers();\" />&nbsp;" + iconImg + FK.Categoria[x].Description + "<br />";
                }
            }
        }*/
        for (var x = 0; x < FK.Elemento.length; x++) {
            if (FK.Elemento[x].Active === true) {
                resFilter += "<input type=\"checkbox\" id=\"Categoria" + FK.Elemento[x].Id + "\" checked=\"checked\" onclick=\"OTFilterMarkers();\" />&nbsp;";
                resFilter += "<img src=\"" + FK.Elemento[x].Icono + "\" style=\"margin-top:-8px;width:24px;\" />&nbsp;";
                resFilter += FK.Elemento[x].Description + "<br />";
            }
        }
        resFilter += "</div><hr />";


        resFilter += "<h4><strong>Estat</strong></h4>";

        resFilter += "<div id=\"EstadoDIV\">";
        resFilter += "<select id=\"CmbFilterOTMapEstado\" style=\"width:100%;\" onchange=\"ADJUDICACION_CmbFilterOTMapEstadoChanged();\">";
        resFilter += "  <option value=\"0\">Tots</option>";
        resFilter += "  <option value=\"2\">Pendent</option>";
        resFilter += "  <option value=\"1\">En procès</option>";
        resFilter += "  <option value=\"4\">Fet</option>";
        resFilter += "  <option value=\"3\">Cerfiticat</option>";
        resFilter += "  <option value=\"5\">Anulada</option>";
        resFilter += "</select>";
        resFilter += "</div><hr />";

        resFilter += "<input type=\"checkbox\" id=\"ChkFilterUrgenteMap\" onchange=\"OTFilterMarkers();\" />Només urgents<hr />";

        resFilter += "<h4><strong>Barri</strong></h4>";
        resFilter += "<select id=\"CmbFilterOTMapBarrio\" style=\"width:100%;\" onchange=\"ADJUDICACION_CmbFilterOTMapBarrioChanged();\"><option value=\"0\">Tots</option>";
        for (var x = 0; x < FK.Barrio.length; x++) {
            if (FK.Barrio[x].ClientePropietarioId === Data.ClientePropietarioId) {
                resFilter += "<option value=\"" + FK.Barrio[x].Id + "\">" + FK.Barrio[x].Description + "</option>";;
            }
        }
        resFilter += "</select>";
        this._div.innerHTML = resFilter;
    };

    filter.addTo(mapOT);
}

function OTFilterMarkers() {
    console.log("OTFilterMarkers");

    var categorias = new Array();
    $("#CategoriasDIV input").each(function () {
        if (this.checked === true) {
            var id = this.id.split("Categoria")[1] * 1;
            categorias.push(id);
        }
    });

    for (var x = 0; x < markers.length; x++) {
        mapOT.removeLayer(markers[x]);
    }

    var barrioId = $("#CmbFilterOTMapBarrio").val() * 1;
    for (var x = 0; x < markers.length; x++) {
        var estadoPass = false;
        var estadoSelected = $("#CmbFilterOTMapEstado").val() * 1;
        var markerEstado = markers[x].properties.EstadoOTId;
        if (estadoSelected === 0) { estadoPass = true; }
        else if (estadoSelected === 2 && (markerEstado === 2 || markerEstado === 7)) { estadoPass = true; }
        else if (estadoSelected === 1 && markerEstado === 1) { estadoPass = true; }
        else if (estadoSelected === 4 && markerEstado === 4) { estadoPass = true; }
        else if (estadoSelected === 3 && markerEstado === 3) { estadoPass = true; }
        else if (estadoSelected === 5 && markerEstado === 5) { estadoPass = true; }

        var show = true;
        if (estadoPass === false) {
            show = false;
        }

        if ($.inArray(markers[x].properties.CategoriaId, categorias) === -1) {
            show = false;
        }

        if (barrioId !== 0 && markers[x].properties.BarrioId !== barrioId) {
            show = false;
        }

        if (document.getElementById("ChkFilterUrgenteMap").checked === true && markers[x].properties.Urgente !== true) {
            show = false;
        }

        if (show === true) {
            markers[x].addTo(mapOT);
        }
    }
}

function RenderMarkersInventario() {
    var table = $("#datatable_col_reorder_Inventario").DataTable();
    //var mapData = table.data();
    var mapData = table.rows({ filter: "applied" }).data();
    for (var x = 0; x < markersInventari.length; x++) {
        mapInventari.removeLayer(markersInventari[x]);
    }

    markersInventari = new Array();
    var centered = false;
    for (var x = 0; x < mapData.length; x++) {
        var data = mapData[x];
        if (data.Latitud !== null && data.Longitud !== null) {
            if (centered === false) {
                if (x === 0) {
                    mapInventari.setView([data.Latitud, data.Longitud], 14);
                    centered = true;
                }
            }

            var elemento = "";
            var problema = "";

            if (typeof data.ElementoDescription !== "undefined" && data.ElementoDescription !== null) {
                elemento = data.ElementoDescription;
            }

            if (typeof data.NivelProblemaDescription !== "undefined" && data.NivelProblemaDescription !== null) {
                problema = data.NivelProblemaDescription;
            }

            var xxx = elemento + " " + problema;
            var mapIcon = L.icon({
                "iconUrl": "/CustomersFramework/Constraula/Data/img/ICONO_icono_" + data.IconoId + ".png",
                "iconSize": [40, 40],
                "iconAnchor": [30, 40],
                "popupAnchor": [-10, -40]
            });

            var itemViewUrl = "GoEncryptedItemView('Actuacion'," + data.Id + ", 'Inventario')";
            var marker = null;
            if (mapIcon !== null) {
                marker = L.marker([data.Latitud, data.Longitud], { icon: mapIcon });
            }
            else {
                marker = L.marker([data.Latitud, data.Longitud]);
            }

            var direccion = data.Direccion;
            var foto = "http://ctman.constraula.com/CustomersFramework/Constraula/Data/PhotoGallery/" + Data.Id + "/" + data.CodigoIncidencia + ".jpg";

            marker.properties = {};
            marker.properties.BarrioId = data.BarrioId;
            marker.properties.ElementoId = data.ElementoId;
            marker.properties.NivelProblemaId = data.NivelProblemaId;

            var popupContent = "<span class=\"mapPopup\" style=\"cursor:pointer;\" onclick=\"" + itemViewUrl + "\">";
            popupContent += "Inventari: <strong>" + data.CodigoIncidencia + "</strong><br /><strong>" + direccion + "</strong><br />" + xxx + "<br/>";
            popupContent += "<div class=\"row\">";
            popupContent += "  <div class=\"col-sm-12 mapPopupImg\"><img id=\"FotoOT" + data.Id + "\" src=\"" + foto + "\" style=\"width:100%;height:auto;\" onerror=\"ADJUDICACION_ReparePhotoOT(this)\" /></div>";
            popupContent += "</row>";
            popupContent += "</span>";
            marker.bindPopup(popupContent);
            markersInventari.push(marker);
        }
    }

    for (var x = 0; x < markersInventari.length; x++) { markersInventari[x].addTo(mapInventari); }
    if (mapInventari !== null) { mapInventari._onResize(); }
}

function RenderMapInventario() {
    // Evita crear el mapa sin contenedor
    if ($("#mapInventari").length === 0) { return; }

    // Evita reinicializar el mapa
    if (mapInventari !== null) { return; }

    //console.log("RenderMapInventario");
    var containerHeight = $(window).height();
    var mapHeight = (containerHeight - 310) + "px";
    $("#mapInventari").css("height", mapHeight);

    mapInventari = L.map("mapInventari", {
        "center": [41.4475097, 2.1598185],
        "zoom": 13
    });

    // Capa offline
    L.tileLayer("http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", { minZoom: 1, maxZoom: 18, attribution: "&copy; <a href=\"http://www.sbrinna.com\">Sbrinna</a> contributors" }).addTo(mapInventari);
    $(".leaflet-control-attribution").hide();

    var filter = L.control({ position: "topright" });

    filter.onAdd = function (mapInventari) {
        this._div = L.DomUtil.create("div", "info");
        this.update();
        return this._div;
    };

    filter.update = function (props) {
        resFilter = "<h4 onclick=\"$('#ElementosDIV').slideToggle()\"><strong>Elements</strong></h4>";

        resFilter += "<div id=\"ElementosDIV\">";
        for (var x = 0; x < FK.Elemento.length; x++) {
            if (FK.Elemento[x].Active === true) {
                resFilter += "<input type=\"checkbox\" id=\"Element" + FK.Elemento[x].Id + "\" checked=\"checked\" onclick=\"InventarioFilterMarkers();\" />&nbsp;";
                resFilter += "<img src=\"" + FK.Elemento[x].Icono + "\" style=\"margin-top:-8px;width:24px;\" />&nbsp;";
                resFilter += FK.Elemento[x].Description + "<br />";
            }
        }
        resFilter += "</div><hr />";

        resFilter += "<h4><strong>Nivell perillositat</strong></h4>";

        resFilter += "<div id=\"NivelDIV\">";
        for (var x = 0; x < FK.NivelProblema.length; x++) {
            if (FK.NivelProblema[x].Active === true) {
                resFilter += "<input type=\"checkbox\" id=\"Nivel" + FK.NivelProblema[x].Id + "\" checked=\"checked\" onclick=\"InventarioFilterMarkers();\" />&nbsp;<span style=\"width:25px;border:1px solid #000;background-color:" + FK.NivelProblema[x].Color + "\">&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;" + FK.NivelProblema[x].Description + "<br />";
            }
        }

        resFilter += "</div><hr />";
        resFilter += "<h4><strong>Barri</strong></h4>";
        resFilter += "<select id=\"CmbFilterInventarioMapBarrio\" style=\"width:100%;\" onchange=\"InventarioFilterMarkers();\"><option value=\"0\">Tots</option>";
        for (var x = 0; x < FK.Barrio.length; x++) {
            if (FK.Barrio[x].ClientePropietarioId === Data.ClientePropietarioId) {
                resFilter += "<option value=\"" + FK.Barrio[x].Description + "\">" + FK.Barrio[x].Description + "</option>";;
            }
        }
        resFilter += "</select>";

        this._div.innerHTML = resFilter;
    };

    filter.addTo(mapInventari);
}

function InventarioFilterMarkers() {
    //console.log("FilterMarkers");

    var barrioId = $("#CmbFilterInventarioMapBarrio").val();

    var elementos = new Array();
    $("#ElementosDIV input").each(function () {
        if (document.getElementById(this.id).checked === true) {
            var id = this.id.split("Element")[1] * 1;
            elementos.push(id);
        }
    });

    var niveles = new Array();
    $("#NivelDIV input").each(function () {
        if (document.getElementById(this.id).checked === true) {
            var id = this.id.split("Nivel")[1] * 1;
            niveles.push(id);
        }
    });

    for (var x = 0; x < markersInventari.length; x++) {
        mapInventari.removeLayer(markersInventari[x]);
    }

    for (var x = 0; x < markersInventari.length; x++) {
        var show = true;
        if ($.inArray(markersInventari[x].properties.ElementoId, elementos) === -1) { show = false; }
        if ($.inArray(markersInventari[x].properties.NivelProblemaId, niveles) === -1) { show = false; }
        if (barrioId !== "0" && markersInventari[x].properties.BarrioId !== barrioId) { show = false; }
        if (show === true) { markersInventari[x].addTo(mapInventari); }
    }
}

function forceResizeMap() {
    //console.log("forceResizeMap");
    var containerHeight = $(window).height();
    var mapHeight = (containerHeight - (fullscreen ? 50 : 310)) + "px";
    $("#map").css("height", mapHeight);
    $("#mapOT").css("height", mapHeight);
    $("#mapInventari").css("height", mapHeight);

    if (mapEdifici !== null) if (typeof mapEdifici._onResize !== "undefined") { { mapEdifici._onResize(); } }
    if (mapOT !== null) if (typeof mapOT._onResize !== "undefined") { { mapOT._onResize(); } }
    if (mapInventari !== null) if (typeof mapInventari._onResize !== "undefined") { { mapInventari._onResize(); } }

    setTimeout(function () {
        if (mapEdifici !== null) if (typeof mapEdifici._onResize !== "undefined") { { mapEdifici._onResize(); } }
        if (mapOT !== null) if (typeof mapOT._onResize !== "undefined") { { mapOT._onResize(); } }
        if (mapInventari !== null) if (typeof mapInventari._onResize !== "undefined") { { mapInventari._onResize(); } }
    }, 700);
}

var fullscreen = false;
function fullscreenToggled() {
    fullscreen = !fullscreen;
    //console.log("FullScreen");
    forceResizeMap();
}

$(".jarviswidget-fullscreen-btn").on("click", fullscreenToggled);

function ADJUDICACION_TableCertificacionesRedraw() {
    return;
    $("#datatable_col_reorder_Certificaciones").dataTable().fnDestroy();
    var query = "";
    try {
        query = eval("queryList" + "Certificaciones");
        //console.log("Certificaciones", query);
    }
    catch (e) {
        query = "";
    }
    table_Certificaciones = $("#datatable_col_reorder_Certificaciones").dataTable({
        "ajax": {
            "url": "/CustomersFramework/Constraula/Data/ItemDataBase.aspx?Action=ACTUACIONES_GETCERTIFICACION" + query,
            "dataSrc": "data",
            "type": "POST"
        },
        "processing": true,
        "pageLength": 50,
        "oLanguage": {
            "sProcessing": "<i class=\"fa fa-gear fa-spin fa-fw\"></i>" + Dictionary.DataTable.Processing + " <strong>Actuacions</strong> ...",
            "sLengthMenu": Dictionary.DataTable.LengthMenu,
            "sZeroRecords": Dictionary.DataTable.ZeroRecords,
            "sInfo": Dictionary.DataTable.Info,
            "sInfoEmpty": Dictionary.DataTable.InfoEmpty,
            "sInfoFiltered": Dictionary.DataTable.InfoFiltered,
            "sInfoPostFix": Dictionary.DataTable.InfoPostFix,
            "sSearch": Dictionary.DataTable.Search,
            "sUrl": "",
            "oPaginate": {
                "sFirst": Dictionary.DataTable.Paginate.First,
                "sPrevious": Dictionary.DataTable.Paginate.Previous,
                "sNext": Dictionary.DataTable.Paginate.Next,
                "sLast": Dictionary.DataTable.Paginate.Last
            }
        },
        "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-4'f><'col-sm-8 col-xs-8 hidden-xs'l>>r" +
            "t" + // the table
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-sm-6 col-xs-12'p>>",
        "autoWidth": true,
        "aaSorting": [[3, "asc"]],
        "aoColumns":
            [
                { "sTitle": "CodiOT", "sortable": true, "hideShowExclude": false, mDataProp: "CodigoOT" },
                { "sTitle": "Direcció", "sortable": true, "hideShowExclude": false, mDataProp: "Direccion" },
                { "sTitle": "Categoria", "sortable": true, "hideShowExclude": false, mDataProp: "CategoriaDescription" },
                { "sTitle": "Certificació", "sortable": true, "hideShowExclude": false, mDataProp: "FechaCertificacion", "mRender": function (data, type, full) { return ADJUDICACION_DateMonth(data, type, full); } },
                { "sTitle": "Valoració", "sortable": true, "hideShowExclude": false, mDataProp: "Valoracion", "mRender": function (data, type, full) { return ADJUDICACION_CertificacionImporte(data, type, full) + " €"; }, "sClass": "cellContentRight", "type": "currency" },
                {
                    "sortable": false,
                    "width": 30,
                    "hideShowExclude": true,
                    "mDataProp": "Id",
                    "mRender": function (data, type, full) {
                        return "<div style=\"white-space: nowrap;\"><a id=\"Actuacion_Certificaciones\" onclick=\"ADJUDICACION_SaveCertificacion(this);\" class=\"btn-edit\"><span><i class=\"fa fa-save\"></i></span></a></div>";
                    }
                }
            ],
        "fnDrawCallback": function () { dataTableSuccess("datatable_col_reorder_Certificaciones"); },
        "createdRow": function (row, data, index) { if (typeof createdRow_Certificaciones !== "undefined") { createdRow_Certificaciones(row, data, index); } }
    });

    new $.fn.dataTable.ColReorder(table_Certificaciones, { "iFixedColumns": 1, "fnReorderCallback": SaveTableConfig });
}

function ADJUDICACION_FixCertificacionHeader() {
    /*$("#datatable_col_reorder_Certificaciones THEAD TR ").css("height", "30px");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(1)").html("Codi OT");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(2)").html("Direcció");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(3)").html("Categoria");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(4)").html("Certificació");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(5)").html("Valoració");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(6)").html("");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(1)").removeAttr("style");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(2)").removeAttr("style");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(3)").removeAttr("style");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(4)").removeAttr("style");
    $("#datatable_col_reorder_Certificaciones THEAD TR :nth-child(5)").removeAttr("style");*/
}

function ADJUDICACION_CertificacionImporte(data, type, full) {
    if (data === null) {
        res = "<input type=\"text\" style=\"width:120px;text-align:right;\" value=\"\" id=\"CertificacionImporte" + full.Id + "\" />&nbsp;&euro;";
    }
    else {
        res = "<input type=\"text\" style=\"width:120px;text-align:right;\" value=\"" + data + "\" id=\"CertificacionImporte" + full.Id + "\" />&nbsp;&euro;";
    }

    return res;
}

function ADJUDICACION_RenderHost(data, type, full) {
    if (data === null) {
        res = "<input type=\"text\" style=\"width:80px;text-align:right;\" value=\"\" id=\"CertificacionHost" + full.Id + "\" />";
    }
    else {
        res = "<input type=\"text\" style=\"width:80px;text-align:right;\" value=\"" + data + "\" id=\"CertificacionHost" + full.Id + "\" />";
    }

    return res;
}

function ADJUDICACION_RenderCertificacionInicio(data, type, full) {
    if (data === null) {
        res = "<input type=\"text\" style=\"width:90px;text-align:right;\ class=\"datepicker certificaciones\" value=\"\" id=\"CertificacionInicio" + full.Id + "\" />";
    }
    else {
        res = "<input type=\"text\" style=\"width:90px;text-align:right;\" class=\"datepicker certificaciones\" value=\"" + data + "\" id=\"CertificacionInicio" + full.Id + "\" />";
    }

    return res;
}

function ADJUDICACION_RenderCertificacionFinal(data, type, full) {
    if (data === null) {
        res = "<input type=\"text\" style=\"width:90px;text-align:right;\" value=\"\" id=\"CertificacionFinal" + full.Id + "\" />";
    }
    else {
        res = "<input type=\"text\" style=\"width:90px;text-align:right;\" value=\"" + data + "\" id=\"CertificacionFinal" + full.Id + "\" />";
    }

    return res;
}

function ADJUDICACION_DateMonth(data, type, full) {
    var month = 0;
    var year = 2017;
    if (data !== null) {
        month = data.split("/")[0] * 1;
        year = data.split("/")[1];
    }

    var res = "<select id=\"CertificacionMonth" + full.Id + "\" style=\"width:90px;\">";
    res += "    <option value=\"0\"" + (month === 0 ? " selected=\"selected\"" : "") + ">...</option>";
    res += "    <option value=\"1\"" + (month === 1 ? " selected=\"selected\"" : "") + ">Gener</option>";
    res += "    <option value=\"2\"" + (month === 2 ? " selected=\"selected\"" : "") + ">Febrer</option>";
    res += "    <option value=\"3\"" + (month === 3 ? " selected=\"selected\"" : "") + ">Març</option>";
    res += "    <option value=\"4\"" + (month === 4 ? " selected=\"selected\"" : "") + ">Abril</option>";
    res += "    <option value=\"5\"" + (month === 5 ? " selected=\"selected\"" : "") + ">Maig</option>";
    res += "    <option value=\"6\"" + (month === 6 ? " selected=\"selected\"" : "") + ">Juny</option>";
    res += "    <option value=\"7\"" + (month === 7 ? " selected=\"selected\"" : "") + ">Juliol</option>";
    res += "    <option value=\"8\"" + (month === 8 ? " selected=\"selected\"" : "") + ">Agost</option>";
    res += "    <option value=\"9\"" + (month === 9 ? " selected=\"selected\"" : "") + ">Setembre</option>";
    res += "    <option value=\"10\"" + (month === 10 ? " selected=\"selected\"" : "") + ">Octubre</option>";
    res += "    <option value=\"11\"" + (month === 11 ? " selected=\"selected\"" : "") + ">Novembre</option>";
    res += "    <option value=\"12\"" + (month === 12 ? " selected=\"selected\"" : "") + ">Decembre</option>";
    res += "</select>";
    res += " / " + year;// + "("+data+")";
    return res;
}

function ADJUDICACION_SaveCertificacion(sender) {
    console.log("ADJUDICACION_SaveCertificacion");
    var table = $("#datatable_col_reorder_Certificaciones").DataTable();
    var row = table.row(sender.parentNode.parentNode.parentNode);

    // Editar
    var data = row.data();

    //console.log(row);
    //console.log(data);

    var host = $("#CertificacionHost" + data.Id).val();
    var month = $("#CertificacionMonth" + data.Id).val();
    var importe = null;

    if ($("#CertificacionImporte" + data.Id).val() !== "") {
        importe = $("#CertificacionImporte" + data.Id).val();
        if ($.isNumeric(importe) === false) {
            var importeText = $("#CertificacionImporte" + data.Id).val().split(',');
            importe = importeText.join('.');
            $("#CertificacionImporte" + data.Id).val(importe);
        }
    }

    var fechaInicio = $("#CertificacionInicio" + data.Id).val();
    var fechaFinal = $("#CertificacionFinal" + data.Id).val();

    var ok = true;
    $("#CertificacionInicio" + data.Id).css("background-color", "#fff");
    if (!validateDate(fechaInicio, true)) {
        $("#CertificacionInicio" + data.Id).css("background-color", "#f33");
        ok = false;
    }
    $("#CertificacionFinal" + data.Id).css("background-color", "#fff");
    if (!validateDate(fechaFinal, true)) {
        $("#CertificacionFinal" + data.Id).css("background-color", "#f33");
        ok = false;
    }

    if (ok === false) {
        return false;
    }

    var dataSend =
    {
        "id": data.Id,
        "host": host,
        "month": month,
        "importe": importe,
        "orignalDate": data.FechaCertificacion,
        "fechaInicio": fechaInicio,
        "fechaFinal": fechaFinal,
        "adjudicacionId": Data.Id,
        "applicationUserId": User.Id
    };



    $.ajax({
        "type": "POST",
        "url": "/CustomersFramework/Constraula/Data/ItemDataBase.aspx/SaveCertificacion",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(dataSend, null, 2),
        "success": function (msg) {
            $(".botTempo").click();
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError);







            }
            else {
                var OTId = msg.d.ReturnValue * 1;
                var OT = GetByIdFromList(FK.Actuacion, OTId);
                var text = "";
                if (OT !== null) {
                    text = " <strong>" + OT.CodigoOT + "</strong>";
                }
                TableRefresh("Certificacion");
                ADJUDICACION_Report();

                PopupActionResult("S'ha desat la cerficació" + text + " correctament.", "Operació realitzada correctament.");








            }
        },
        error: function (msg) {
            $(".botTempo").click();
            PopupErrorSmall(msg.responseText);







        }
    });

    return false;
}

function ADJUDICACION_ReparePhotoOT(sender) {
    sender.src = "/img/noimage.png";
    sender.style.width = "120px";
}

function ADJUDICACION_GetFilter(filter) {
    console.log("GetFilter");
    if (filter !== null) {
        $("#AdjudicacionTxtFechaRecepcionDesde").val(filter.OT.FechaInicioDesde);
        $("#AdjudicacionTxtFechaRecepcionHasta").val(filter.OT.FechaInicioHasta);
        $("#AdjudicacionTxtFechaEjecucionDesde").val(filter.OT.FechaEjecucionDesde);
        $("#AdjudicacionTxtFechaEjecucionHasta").val(filter.OT.FechaEjecucionHasta);
        $("#AdjudicacionTxtFechaCertificacionDesde").val(filter.OT.FechaCertificacionDesde);
        $("#AdjudicacionTxtFechaCertificacionHasta").val(filter.OT.FechaCertificacionHasta);
        $("#AdjudicacionTxtEstadoOTId").val(filter.OT.EstadoOT);
        $("#AdjudicacionTxtBarrioId").val(filter.OT.Barrio);
        $("#CmbFilterOTMapEstado").val(filter.OT.EstadoOT);
        $("#CmbFilterOTMapBarrio").val(filter.OT.Barrio);
        document.getElementById("AdjudicacionTxtUrgent").checked = filter.OT.Urgente;

        if (document.getElementById("ChkFilterUrgenteMap") !== null) {
            document.getElementById("ChkFilterUrgenteMap").checked = filter.OT.Urgente;
        }

        $("#AdjudicacionTxtInventariFilterElementoId").val(filter.Inventario.Elemento);
        $("#AdjudicacionTxtInventariFilterNivelProblemaId").val(filter.Inventario.NivelProblema);
        $("#AdjudicacionTxtInventariFilterBarrioId").val(filter.Inventario.Barrio);

        if ($("#AdjudicacionTxtFechaRecepcionDesde").val() !== "" || $("#AdjudicacionTxtFechaRecepcionHasta").val() !== "") {
            $("#DataType").val(1);
        }
        else if ($("#AdjudicacionTxtFechaFinalizacionDesde").val() !== "" || $("#AdjudicacionTxtFechaFinalizacionHasta").val() !== "") {
            $("#DataType").val(2);
        }
        else if ($("#AdjudicacionTxtFechaCertificacionDesde").val() !== "" || $("#AdjudicacionTxtFechaCertificacionHasta").val() !== "") {
            $("#DataType").val(3);
        }

        ADJUDACION_FilterOT_DataTypeChange();
        //ADJUDICACION_TableOTFilter();
        ADJUDICACION_TableInventarioFilter();
        ADJUDICACION_TableOTFilter();
    }
    else {
        // ADJUDICACION_OTFilter_Compress();
    }

    //ADJUDICACION_OTFilter_Expand();
}

function ADJUDICACION_SetFilter() {
    var filter = {
        "OT": {
            "FechaInicioDesde": $("#AdjudicacionTxtFechaRecepcionDesde").val(),
            "FechaInicioHasta": $("#AdjudicacionTxtFechaRecepcionHasta").val(),
            "FechaEjecucionDesde": $("#AdjudicacionTxtFechaEjecucionDesde").val(),
            "FechaEjecucionHasta": $("#AdjudicacionTxtFechaEjecucionHasta").val(),
            "FechaCertificacionDesde": $("#AdjudicacionTxtFechaCertificacionDesde").val(),
            "FechaCertificacionHasta": $("#AdjudicacionTxtFechaCertificacionHasta").val(),
            "Urgente": document.getElementById("AdjudicacionTxtUrgent").checked,
            "EstadoOT": $("#AdjudicacionTxtEstadoOTId").val() * 1,
            "Barrio": $("#AdjudicacionTxtBarrioId").val() * 1
        },
        "Inventario": {
            "Elemento": $("#AdjudicacionTxtInventariFilterElementoId").val(),
            "NivelProblema": $("#AdjudicacionTxtInventariFilterNivelProblemaId").val(),
            "Barrio": $("#AdjudicacionTxtInventariFilterBarrioId").val()
        }
    };

    SaveFilter(filter);
}

function ADJUDICACION_PopupTodoExcel() {
    var data =
    {
        "applicationUserId": User.Id,
        "applicationUserFullName": User.FullName,
        "adjudicacionId": Data.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/CustomersFramework/Constraula/Pages/ExportTodo.aspx/ExportFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError);
            }
            else {
                var link = document.createElement("a");
                link.id = "download";
                link.href = msg.d.MessageError;
                link.download = msg.d.MessageError;
                document.body.appendChild(link);
                document.location = msg.d.MessageError;
                document.body.removeChild(link);
            }
        },
        error: function (msg) {
            PopupErrorSmall(msg.responseText);
        }
    });
}

function ADJUDICACION_PopupInventarioExcel() {
    var data =
    {
        "textoFiltro": $("#datatable_col_reorder_Inventario_wrapper input").val(),
        "applicationUserId": User.Id,
        "applicationUserFullName": User.FullName,
        "adjudicacionId": Data.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/CustomersFramework/Constraula/Pages/ExportInventario.aspx/ExportFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError);
            }
            else {
                var link = document.createElement("a");
                link.id = "download";
                link.href = msg.d.MessageError;
                link.download = msg.d.MessageError;
                document.body.appendChild(link);
                document.location = msg.d.MessageError;
                document.body.removeChild(link);
            }
        },
        error: function (msg) {
            PopupErrorSmall(msg.responseText);
        }
    });
}

function ADJUDICACION_PopupOTViaPublicaPDF() {
    var data =
    {
        "fechaRecepcionDesde": $("#AdjudicacionTxtFechaRecepcionDesde").val(),
        "fechaRecepcionHasta": $("#AdjudicacionTxtFechaRecepcionHasta").val(),
        "fechaEjecucionDesde": $("#AdjudicacionTxtFechaEjecucionDesde").val(),
        "fechaEjecucionHasta": $("#AdjudicacionTxtFechaEjecucionHasta").val(),
        "fechaCertificacionDesde": $("#AdjudicacionTxtFechaCertificacionDesde").val(),
        "fechaCertificacionHasta": $("#AdjudicacionTxtFechaCertificacionHasta").val(),
        "urgente": document.getElementById("AdjudicacionTxtUrgent").checked,
        "estadoOT": $("#AdjudicacionTxtEstadoOTId").val() * 1,
        "barrio": $("#AdjudicacionTxtBarrioId").val() * 1,
        "textoFiltro": $("#datatable_col_reorder_OTViaPublica_filter input").val(),
        "applicationUserId": User.Id,
        "applicationUserFullName": User.FullName,
        "adjudicacionId": Data.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/CustomersFramework/Constraula/Pages/ExportOTViaPublica.aspx/ExportFilterPDF",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError);







            }
            else {
                console.log("ADJUDICACION_PopupOTViaPublicaPDF", msg.d.MessageError);
                window.open(msg.d.MessageError);
            }
        },
        "error": function (msg) {
            PopupErrorSmall(msg.responseText);







        }
    });
}

function ADJUDICACION_PopupOTViaPublicaExcel() {
    var data =
    {
        "fechaInicioDesde": $("#AdjudicacionTxtFechaRecepcionDesde").val(),
        "fechaInicioHasta": $("#AdjudicacionTxtFechaRecepcionHasta").val(),
        "fechaEjecucionDesde": $("#AdjudicacionTxtFechaEjecucionDesde").val(),
        "fechaEjecucionHasta": $("#AdjudicacionTxtFechaEjecucionHasta").val(),
        "fechaCertificacionDesde": $("#AdjudicacionTxtFechaCertificacionDesde").val(),
        "fechaCertificacionHasta": $("#AdjudicacionTxtFechaCertificacionHasta").val(),
        "urgente": document.getElementById("AdjudicacionTxtUrgent").checked,
        "estadoOT": $("#AdjudicacionTxtEstadoOTId").val() * 1,
        "barrio": $("#AdjudicacionTxtBarrioId").val() * 1,
        "textoFiltro": $("#datatable_col_reorder_OTViaPublica_filter input").val(),
        "applicationUserId": User.Id,
        "applicationUserFullName": User.FullName,
        "adjudicacionId": Data.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/CustomersFramework/Constraula/Pages/ExportOTViaPublica.aspx/ExportFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError);







            }
            else {
                console.log("ADJUDICACION_PopupOTViaPublicaExcel", msg.d.MessageError);
                window.open(msg.d.MessageError);
            }
        },
        "error": function (msg) {
            PopupErrorBig(msg.responseText);







        }
    });
}

function ADJUDICACION_PrepareMailFields() {
    console.log("ACTUACION_PrepareMailFields");
    //$("#AdjudicacionTxtMailsGenerarOT").hide();
    // $("#AdjudicacionTxtMailsOTRealizada").hide();

    /*$('#AdjudicacionTxtMailsGenerarOT').parent().append($('<div class="col col-label"><label class="input">Tags</label></div>' +
        '<div class=" col col-2"><label class="select"><input id="AdjudicacionTxtMailsGenerarOTSelect" style="width: 100%; background: transparent;">' +
        '</input><i></i></label></div>'));*/

    var mailList = new Array();
    for (var x = 0; x < FK.User.length; x++) {
        if (FK.User[x].Email !== null && FK.User[x].Email !== "") {
            mailList.push(FK.User[x].Email);
        }
    }

    console.log(mailList);

    $("#AdjudicacionTxtMailsPresupuestoFromDistrito").css("width", "100%")
        .select2({
            "language": "ca",
            "tags": mailList,
            "tokenSeparators": [",", " "],
            "selectOnClose": true
        })
        .find(".select2-arrow").remove();

    $("#AdjudicacionTxtMailsPresupuestoFromConstraula").css("width", "100%")
        .select2({
            "language": "ca",
            "tags": mailList,
            "tokenSeparators": [",", " "],
            "selectOnClose": true
        })
        .find(".select2-arrow").remove();

    $("#AdjudicacionTxtMailsGenerarOT").css("width", "100%")
        .select2({
            "language": "ca",
            "tags": mailList,
            "tokenSeparators": [",", " "],
            "selectOnClose": true
        })
        .find(".select2-arrow").remove();

    $("#AdjudicacionTxtMailsOTRealizada").css("width", "100%")
        .select2({
            "language": "ca",
            "tags": mailList,
            "tokenSeparators": [",", " "],
            "selectOnClose": true
        })
        .find(".select2-arrow").remove();

    $("#AdjudicacionTxtMailsOTAnulada").css("width", "100%")
        .select2({
            "language": "ca",
            "tags": mailList,
            "tokenSeparators": [",", " "],
            "selectOnClose": true
        })
        .find(".select2-arrow").remove();

}

function ADJUDICACION_OTFilter_Expand() {
    // anulado por simplicación de fechas
    return false;
    if (typeof Data.Id !== "undefined") {
        $("#OTFilterRow1").show();
        $("#OTFilterRow2").show();
        $("#OTFilterRow3").show();
        if (document.getElementById("OTFilterToogle") !== null) {
            document.getElementById("OTFilterToogle").className = "fa fa-compress";
            ADJUDICACION_Context.OTFilerOpen = true;
        }
    }
}

function ADJUDICACION_OTFilter_Compress() {
    // anulado por simplicación de fechas
    return false;
    if (typeof Data.Id !== "undefined") {
        $("#OTFilterRow1").hide();
        $("#OTFilterRow2").hide();
        $("#OTFilterRow3").hide();
        if (document.getElementById("OTFilterToogle") !== null) {
            document.getElementById("OTFilterToogle").className = "fa fa-expand";
            ADJUDICACION_Context.OTFilerOpen = false;
        }
    }
}

function ADJUDICACION_InventarioFilter_Expand() {
    if (typeof Data.Id !== "undefined") {
        $("#InventarioFilterRow1").show();
        if (document.getElementById("InventarioFilterToogle") !== null) {
            document.getElementById("InventarioFilterToogle").className = "fa fa-compress";
            ADJUDICACION_Context.InventarioFilerOpen = true;
        }
    }
}

function ADJUDICACION_InventarioFilter_Compress() {
    if (typeof Data.Id !== "undefined") {
        $("#InventarioFilterRow1").hide();
        if (document.getElementById("InventarioFilterToogle") !== null) {
            document.getElementById("InventarioFilterToogle").className = "fa fa-expand";
            ADJUDICACION_Context.InventarioFilerOpen = false;
        }
    }
}

function ADJUDICACION_OTFilter_Toggle() {
    if (ADJUDICACION_Context.OTFilerOpen === true) {
        ADJUDICACION_OTFilter_Compress();
    }
    else {
        ADJUDICACION_OTFilter_Expand();
    }
}

function ADJUDICACION_InventarioFilter_Toggle() {
    if (ADJUDICACION_Context.InventarioFilerOpen === true) {
        ADJUDICACION_InventarioFilter_Compress();
    }
    else {
        ADJUDICACION_InventarioFilter_Expand();
    }
}

function ADJUDICACION_ReportGeneral() {
    $("#CommonPopupMainContainer").html("" +
        "                    <div class=\"smart-form\">" +
        "                        <fieldset>" +
        "                            <section class=\"col\" style=\"width:100%;\">" +
        "                                <strong>Introdueixi una adreça de email per enviar l'informe una vegada generat.</strong><br />" +
        "                                <div class=\"row\">" +
        "                                    <div class=\"col col-sm-2\"><label class=\"input\" id=\"InformeGeneralEmailLabel\">Email</label></div>" +
        "                                    <div class=\"col col-sm-10\">" +
        "                                        <input type=\"text\" name=\"InformeGeneralEmail\" id=\"InformeGeneralEmail\" value=\"\" placeholder=\"\" class=\"form-control\" />" +
        "                                    </div >" +
        "                                </div>" +
        "                            </section>" +
        "                        </fieldset>" +
        "                        <footer>" +
        "                            <button type=\"button\" class=\"btn btn-primary\" data-dismiss=\"modal\" id=\"CommonPopupBtnCancel\">Cancelar</button>" +
        "                            <button type=\"button\" class=\"btn btn-success\" id=\"BtnInformeGeneralGo\" onclick=\"ADJUDICACION_InformeGeneralGo();\">Generar</button>" +
        "                        </footer>" +
        "                    </div>");
    $("#CommonPopupTitleText").html("Informe general");
    $("#CommonPopupLauncher").click();
}

function ADJUDICACION_GetLastIndex() {
    console.log("LastIndex", "ini");
    $.getJSON("/CustomersFramework/Constraula/Data/ItemDataBase.aspx?Action=AdjudicacionLastIndex&adjudicacionId=" + Data.Id,
        function (data) {
            ADJUDICACION_Context.LastIndexInventario = data.data.LastIndexInventario;
            ADJUDICACION_Context.LastIndexOT = data.data.LastIndexOT;
            console.log("ADJUDICACION_GetLastIndex", ADJUDICACION_Context);
        });
    console.log("LastIndex", "end");
}

var pagedestroy = function () {
    $("#datatable_col_reorder_OTViaPublica").dataTable().fnDestroy();
    $("#datatable_col_reorder_Certificaciones").dataTable().fnDestroy();
    $("#datatable_col_reorder_Certificacion").dataTable().fnDestroy();
    $("#datatable_col_reorder_AdjudicacionCentros").dataTable().fnDestroy();
    $("#datatable_col_reorder_Inventario").dataTable().fnDestroy();

    delete window.table_OTViaPublica;
    delete window.table_Certificaciones;
    delete window.table_Certificacion;
    delete window.table_AdjudicacionCentros;
    delete window.table_Inventario;

    root.console.log("✔ Adjudicacion destroy");
    $("#content").html("");
}

function ADJUDICACION_ReportMensual() {
    $("#CommonPopupMainContainer").html("" +
        "                    <div class=\"smart-form\">" +
        "                        <fieldset>" +
        "                            <section class=\"col\" style=\"width:100%;\">" +
        "                                <strong>Sel·leciones les dates sobre les que es farà l'informe</strong><br />" +
        "                                <div class=\"row\">" +
        "                                    <div class=\"col col-sm-2\"><label class=\"input\" id=\"InformeMensualDesdeLabel\">Des de</label></div>" +
        "                                    <div class=\"col col-sm-4\">" +
        "                                        <div class=\"input-group\">" +
        "                                            <input type=\"text\" name=\"InformeMensualDesde\" id=\"InformeMensualDesde\" value=\"\" placeholder=\"\" class=\"form-control datepicker\" data-dateformat=\"Date\" size=\"10\" />" +
        "                                            <span class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>" +
        "                                        </div>" +
        "                                    </div >" +
        "                                    <div class=\"col col-sm-2\"><label class=\"input\" id=\"InformeMensualHastaLabel\">Fins</label></div>" +
        "                                    <div class=\"col col-sm-4\">" +
        "                                        <div class=\"input-group\">" +
        "                                            <input type=\"text\" name=\"InformeMensualHasta\" id=\"InformeMensualHasta\" value=\"\" placeholder=\"\" class=\"form-control datepicker\" data-dateformat=\"Date\" size=\"10\" />" +
        "                                            <span class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>" +
        "                                        </div>" +
        "                                    </div >" +
        "                                </div>" +
        "                                <br />" +
        "                                <strong>Introdueixi una adreça de email per enviar l'informe una vegada generat.</strong><br />" +
        "                                <div class=\"row\">" +
        "                                    <div class=\"col col-sm-2\"><label class=\"input\" id=\"InformeMensualEmailLabel\">Email</label></div>" +
        "                                    <div class=\"col col-sm-10\">" +
        "                                        <input type=\"text\" name=\"InformeMensualEmail\" id=\"InformeMensualEmail\" value=\"\" placeholder=\"\" class=\"form-control\" />" +
        "                                    </div >" +
        "                                </div>" +
        "                                <div class=\"row\" id=\"ReportMensualPopupError\" style=\"padding:12px;color:#f00;display:none;\">" +
        "                                </div>" +
        "                            </section>" +
        "                        </fieldset>" +
        "                        <footer>" +
        "                            <button type=\"button\" class=\"btn btn-primary\" data-dismiss=\"modal\" id=\"CommonPopupBtnCancel\">Cancelar</button>" +
        "                            <button type=\"button\" class=\"btn btn-success\" id=\"BtnInformeMensualGo\" onclick=\"ADJUDICACION_InformeMensualGo();\">Generar</button>" +
        "                        </footer>" +
        "                    </div>");
    $("#CommonPopupTitleText").html("Informe mensual");
    $("#CommonPopupLauncher").click();
    $("#InformeMensualDesde").datepicker({
        "dateFormat": "dd/mm/yy",
        "prevText": "<i class=\"fa fa-chevron-left\"></i>",
        "nextText": "<i class=\"fa fa-chevron-right\"></i>"
    });
    $("#InformeMensualHasta").datepicker({
        "dateFormat": "dd/mm/yy",
        "prevText": "<i class=\"fa fa-chevron-left\"></i>",
        "nextText": "<i class=\"fa fa-chevron-right\"></i>"
    });

    $("#InformeMensualDesde").val("01/01/2017");
    $("#InformeMensualHasta").val("01/11/2017");
    $("#InformeMensualEmail").val("jcastilla@sbrinna.com");
}

function ADJUDICACION_FillComboFiltroOTEstado() {
    var target = document.getElementById("AdjudicacionTxtEstadoOTId");
    $("#AdjudicacionTxtEstadoOTId").html("");

    var defaultOption = document.createElement("option");
    defaultOption.value = 0;
    defaultOption.appendChild(document.createTextNode(Dictionary.Common_Select));
    target.appendChild(defaultOption);

    for (var x = 0; x < FK.EstadoOT.length; x++) {
        if (FK.EstadoOT[x].Id < 6) {
            var option = document.createElement("option");
            option.value = FK.EstadoOT[x].Id;
            option.appendChild(document.createTextNode(FK.EstadoOT[x].Description));
            option.style.backgroundColor = FK.EstadoOT[x].Color;
            target.appendChild(option);
        }
    }
}

var ACTUACION_ItemToDelete = null;
function ACTUACION_DeleteActuacion(data, type) {
    console.log("delete:" + type, data);
    $("#dialog-delete H2").html("Eliminar <strong>" + type + "</strong>");
    ACTUACION_ItemToDelete = {
        "ItemName": "Actuacion",
        "ItemDefinition": ItemDefinition,
        "ItemId": data.Id,
        "ItemDescription": type === "OT" ? data.O : data.CodigoIncidencia,
        "ItemType": type
    }
    $("#ItemToDeleteLabel").html(type);
    $("#itemDeletableName").html(ACTUACION_ItemToDelete.ItemDescription);
    $("#itemDeleteMessage").html("serà eliminar de forma permanent.<br />Vol continuar?");
    $("#DeleteLauncher").click();
}

var ADJUDICACIONAMPLIACION_ItemToDelete = null;
function ACTUACION_DeleteAmpliacion(data) {
    console.log("ACTUACION_DeleteAmpliacion:", data);
    $("#dialog-delete H2").html("Eliminar <strong> ampliació</strong>");
    ADJUDICACIONAMPLIACION_ItemToDelete = {
        "ItemName": "AdjudicacionAmpliacion",
        "ItemDefinition": GetItemDefinition("AdjudicacionAmpliacion"),
        "ItemId": data.Id,
        "ItemDescription": data.Descripcion
    }
    console.log("ACTUACION_DeleteAmpliacion", ADJUDICACIONAMPLIACION_ItemToDelete)
    $("#itemDeletableName").html(ADJUDICACIONAMPLIACION_ItemToDelete.ItemDescription);
    $("#itemDeletableName").before("L'ampliació ");
    $("#itemDeleteMessage").html("amd un import de <strong>" + ToMoneyFormat(data.Importe, 2) + "</strong> serà eliminada de forma permanent.<br />Vol continuar?");
    $("#DeleteLauncher").click();
}

function ACTUACION_DeleteActuacionConfirmed() {
    if (ACTUACION_ItemToDelete === null && ADJUDICACIONAMPLIACION_ItemToDelete === null) {
        console.log("ACTUACION_DeleteActuacionConfirmed", "Nada que eliminar");
        return false;
    }

    $("#dialog-delete").click();
    try {
        $("#working").click();
    }
    catch (e) {
        console.log(e);
    }

    var data = null;
    var webMethod = "";
    if (ACTUACION_ItemToDelete !== null) {
        webMethod = "/CustomersFramework/Constraula/Data/ItemDataBase.aspx/InactiveActuacion";
        data = {
            "itemId": ACTUACION_ItemToDelete.ItemId,
            "itemType": ACTUACION_ItemToDelete.ItemType,
            "userDescription": User.TraceDescription
        };
    }
    else if (ADJUDICACIONAMPLIACION_ItemToDelete !== null) {
        webMethod = "/CustomersFramework/Constraula/Data/ItemDataBase.aspx/InactiveAdjudicacionAmplicacion";
        data = {
            "itemId": ADJUDICACIONAMPLIACION_ItemToDelete.ItemId,
            "applicationUserId": User.Id,
            "userDescription": User.TraceDescription
        };
    }

    console.log(data, webMethod);

    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            $(".botTempo").click();

            if (msg.d.Success === false) {
                var text = "";
                if (ACTUACION_ItemToDelete !== null) {
                    text = ACTUACION_ItemToDelete.Descripcion;
                }
                else {
                    text = ADJUDICACIONAMPLIACION_ItemToDelete.Descripcion;
                }

                PopupErrorBig("text");
            }
            else {
                var textNoSuccess = "";
                if (ACTUACION_ItemToDelete !== null) {
                    textNoSuccess = ACTUACION_ItemToDelete.ItemDescription;
                    TableRefresh("OTViaPublica");
                    TableRefresh("Inventario");
                }
                else {
                    textNoSuccess = ADJUDICACIONAMPLIACION_ItemToDelete.ItemDescription;
                    TableRefresh("AdjudicacionAmpliacion");
                }
                PopupActionResult("S'ha eliminat correctament l'actuació <strong>" + textNoSuccess + "</strong>", "Adjudicació eliminada");

                ACTUACION_ItemToDelete = null;
                ADJUDICACIONAMPLIACION_ItemToDelete = null;
            }
        },
        error: function (msg) {
            console.log(msg);
            PopupErrorSmall(msg.Message);
        }
    });

    return false;
}

function ADJUDICACION_InformeMensualGo() {
    var startDate = $("#InformeMensualDesde").val();
    var endDate = $("#InformeMensualHasta").val();
    var email = $("#InformeMensualEmail").val();
    var errorMessage = "";

    $("#InformeMensualDesdeLabel").css("color", "#000");
    $("#InformeMensualHastaLabel").css("color", "#000");
    $("#InformeMensualEmailLabel").css("color", "#000");
    $("#ReportMensualPopupError").hide();

    var ok = true;
    if (startDate === "") {
        $("#InformeMensualDesdeLabel").css("color", "#f00");
        errorMessage = "La data d'inici és obligatoria.<br />";
        ok = false;
    }

    if (endDate === "") {
        $("#InformeMensualHastaLabel").css("color", "#f00");
        errorMessage += "La data final és obligatoria.<br />";
        ok = false;
    }

    if (email === "") {
        $("#InformeMensualEmailLabel").css("color", "#f00");
        errorMessage += "S'ha d'informar del mail que rebrà l'informe.<br />";
        ok = false;
    }
    else {
        if (validateEmail(email) === false) {
            $("#InformeMensualEmailLabel").css("color", "#f00");
            errorMessage += "L'adreça de mail no és correcta.<br />";
            ok = false;
        }
    }

    if (ok === false) {
        $("#ReportMensualPopupError").html(errorMessage);
        $("#ReportMensualPopupError").show();
        return false;
    }

    window.open("/CustomersFramework/Constraula/Pages/AdjudicacionReportMensual.aspx?id=" + Data.Id + "&startDate=" + startDate + "&endDate=" + endDate);
}

function ADJUDICACION_InformeGeneralGo() {
    alert("ADJUDICACION_InformeGeneralGo");
}

function ValidateFormCustom_ADJUDICACION_Alternative() {
    console.log("ValidateFormCustom_ADJUDICACION_Alternative");
    console.log("IVA", $("#ActuacionTxtIVA").val());
    console.log("SEC", $("#ActuacionTxtSecurity").val());
    console.log("DGBI", $("#ActuacionTxtDGBI").val());
    console.log("Baja", $("#ActuacionTxtBaja").val());

    Data.IVA = $("#ActuacionTxtIVA").val();
    Data.Security = $("#ActuacionTxtSecurity").val();
    Data.DGBI = $("#ActuacionTxtDGBI").val();
    Data.Baja = $("#ActuacionTxtBaja").val();

    return true;
}

function ADJUDICACION_RenderEconomics() {
    console.log("ADJUDICACION_RenderEconomics");

    var PECsinIVA = Data.Importe;
    var IVA = PECsinIVA * ADJUDICACION_Context.IVA / 100;
    var PECIVA = PECsinIVA + IVA;

    var DGBI = PECsinIVA * (1 - 1 / (1 + ADJUDICACION_Context.DGBI / 100));
    var PEMBaixa = PECsinIVA / (100 + ADJUDICACION_Context.DGBI) * 100;

    var Baixa = PEMBaixa / (1 - (ADJUDICACION_Context.Baja / 100)) - PEMBaixa;
    var PEMSIS = PEMBaixa + Baixa;

    var SIS = PEMSIS - PEMSIS / (1 + ADJUDICACION_Context.Security / 100);
    var PEM = PEMSIS - SIS;

    var res = "<table style=\"width:100%;padding:12px;border:1px solid #777;\" class=\"table table-hover\">";

    res += "<tr><td>PEM (sense baixa)</td><td align=\"right\">" + ToMoneyFormat(PEM, 2) + "&nbsp;&euro;</td></tr>";
    res += "<tr><td><strong>" + ToMoneyFormat(ADJUDICACION_Context.Security, 2) + "%</strong> Pla de seguretat i salut</td><td align=\"right\">" + ToMoneyFormat(SIS, 2) + "&nbsp;&euro;</td></tr>";
    res += "<tr><td>PEM (Amb SiS, sense baixa) </td><td align=\"right\">" + ToMoneyFormat(PEMSIS, 2) + "&nbsp;&euro;</td></tr>";
    res += "<tr><td><strong>" + ToMoneyFormat(ADJUDICACION_Context.Baja, 2) + "%</strong> Baixa</td><td align=\"right\">" + ToMoneyFormat(Baixa, 2) + "&nbsp;&euro;</td></tr>";
    res += "<tr><td>PEM (Amb baixa aplicada)</td><td align=\"right\">" + ToMoneyFormat(PEMBaixa, 2) + "&nbsp;&euro;</td></tr>";
    res += "<tr><td><strong>" + ToMoneyFormat(ADJUDICACION_Context.DGBI, 2) + "%</strong> D.G. i B.I.</td><td align=\"right\">" + ToMoneyFormat(DGBI, 2) + "&nbsp;&euro;</td></tr>";
    res += "<tr><td>PEC (Sense IVA)</td><td align=\"right\"><strong>" + ToMoneyFormat(PECsinIVA, 2) + "&nbsp;&euro;</strong></td></tr>";
    res += "<tr><td><strong>" + ToMoneyFormat(ADJUDICACION_Context.IVA, 2) + "%</strong> d'I.V.A.</td><td align=\"right\">" + ToMoneyFormat(IVA, 2) + "&nbsp;&euro;</td></tr>";
    res += "<tr><td>PEC (Amb IVA)</td><td align=\"right\">" + ToMoneyFormat(PECIVA, 2) + "&nbsp;&euro;</td></tr>";

    res += "</table>";

    $("#placeholderEconomics").html(res);
    $("#placeholderEconomics").addClass("col").addClass("col-sm-6");
    $("#placeholderEconomics").show();

}

function ADJUDICACION_Core() {
    var res = "";
    res += "    <div class=\"alert alert-info\" style=\"color:#fff;background-color:#65a4da;padding:10px;\">";
    res += "        <i class=\"fa fa-info fa-fw fa-lg\"></i>&nbsp;";
    res += "        <strong>Avís!</strong>&nbsp;Aquesta secció només la podem veure els usuaris administradors.";
    res += "    </div>";
    res += "    <section class=\"col\" style=\"width: 100%;\">";
    res += "        <div class=\"col col-sm-4\" > <label class=\"input\" name=\"ActuacionTxtSecurityLabel\" id=\"ActuacionTxtSecurityLabel\">Percentatge seguretat</label></div >";
    res += "        <div class=\" col col-sm-8\">";
    res += "            <div class=\"input-group\">";
    res += "                <input type=\"text\" name=\"ActuacionTxtSecurity\" id=\"ActuacionTxtSecurity\" placeholder=\"Seguretat\" value=\"\" class=\"money-bank\" data-currencysymbol=\"\" data-decimalcharacter=\".\" data-digitgroupseparator=\",\" data-decimalplacesoverride=\"2\" style=\"text-align: right;\">";
    res += "			</div>";
    res += "        </div>";
    res += "    </section>";
    res += "    <section class=\"col\" style=\"width: 100%;\">";
    res += "        <div class=\"col col-sm-4\" > <label class=\"input\" name=\"ActuacionTxtBajaLabel\" id=\"ActuacionTxtBajaLabel\">Percentatge Baixa</label></div >";
    res += "        <div class=\" col col-sm-8\">";
    res += "            <div class=\"input-group\">";
    res += "                <input type=\"text\" name=\"ActuacionTxtBaja\" id=\"ActuacionTxtBaja\" placeholder=\"Baja\" value=\"\" class=\"money-bank\" data-currencysymbol=\"\" data-decimalcharacter=\".\" data-digitgroupseparator=\",\" data-decimalplacesoverride=\"2\" style=\"text-align: right;\">";
    res += "			</div>";
    res += "        </div>";
    res += "    </section>";
    res += "    <section class=\"col\" style=\"width: 100%;\">";
    res += "        <div class=\"col col-sm-4\" > <label class=\"input\" name=\"ActuacionTxtDGBILabel\" id=\"ActuacionTxtDGBILabel\">Percentatge DGBI</label></div >";
    res += "        <div class=\" col col-sm-8\">";
    res += "            <div class=\"input-group\">";
    res += "                <input type=\"text\" name=\"ActuacionTxtDGBI\" id=\"ActuacionTxtDGBI\" placeholder=\"DGBI\" value=\"\" class=\"money-bank\" data-currencysymbol=\"\" data-decimalcharacter=\".\" data-digitgroupseparator=\",\" data-decimalplacesoverride=\"2\" style=\"text-align: right;\">";
    res += "			</div>";
    res += "        </div>";
    res += "    </section>";
    res += "    <section class=\"col\" style=\"width: 100%;\">";
    res += "        <div class=\"col col-sm-4\" > <label class=\"input\" name=\"ActuacionTxtIVALabel\" id=\"ActuacionTxtIVALabel\">Percentatge I.V.A.</label></div >";
    res += "        <div class=\" col col-sm-8\">";
    res += "            <div class=\"input-group\">";
    res += "                <input type=\"text\" name=\"ActuacionTxtIVA\" id=\"ActuacionTxtIVA\" placeholder=\"I.V.A.\" value=\"\" class=\"money-bank\" data-currencysymbol=\"\" data-decimalcharacter=\".\" data-digitgroupseparator=\",\" data-decimalplacesoverride=\"2\" style=\"text-align: right;\">";
    res += "			</div>";
    res += "        </div>";
    res += "    </section>";
    res += "    <a class=\"btn btn-default btn-success btn-edit\" id=\"BtnSaveEconomics\" style=\"float:right; padding: 8px;margin:10px;\"><i class=\"fa fa-save\"></i>&nbsp;Desar configuració econòmica</a>";

    $("#placeholderEconomicsCore").html(res);
    $("#placeholderEconomicsCore").css("border", "1px solid #ccc");
    $("#placeholderEconomicsCore").css("background-color", "#fcfcff");
    $("#placeholderEconomicsCore").addClass("col").addClass("col-sm-6");
    $("#placeholderEconomicsCore").show();
    $("#BtnSaveEconomics").on("click", ADJUDICACION_SaveEconomics);

    $("#ActuacionTxtIVA").val(Data.IVA);
    $("#ActuacionTxtDGBI").val(Data.DGBI);
    $("#ActuacionTxtBaja").val(Data.Baja);
    $("#ActuacionTxtSecurity").val(Data.Security);
}

function ADJUDICACION_SaveEconomics() {
    console.log("ADJUDICACION_SaveEconomics");
    var errorMessage = [];
    var ok = true;

    if ($("#ActuacionTxtIVA").val() === "") {
        ok = false;
        errorMessage.push("El IVA es obligatori");
    }

    if ($("#ActuacionTxtDGBI").val() === "") {
        ok = false;
        errorMessage.push("El DGBI es obligatori");
    }

    if ($("#ActuacionTxtBaja").val() === "") {
        ok = false;
        errorMessage.push("La baixa es obligatoria");
    }

    if ($("#ActuacionTxtSecurity").val() === "") {
        ok = false;
        errorMessage.push("El percetatge de seguretat es obligatori");
    }

    if (ok === false) {
        var MessageError = "Antenció als següents errors<ul>";
        for (var x = 0; x < errorMessage.length; x++) {
            MessageError += "<li>" + errorMessage[x] + "</li>";
        }

        MessageError += "</ul>";
        PopupErrorBig(MessageError);
        /*$.bigBox({
            title: "¡Se ha producido un error!",
            content: "<i class='fa fa-clock-o'></i> <i>" + MessageError + "</i>",
            color: "#C46A69",
            icon: "fa fa-warning shake animated",
            timeout: 40000
        });*/
        return false;
    }

    var dataSend =
    {
        "id": Data.Id,
        "iva": $("#ActuacionTxtIVA").val() * 1,
        "dgbi": $("#ActuacionTxtDGBI").val() * 1,
        "security": $("#ActuacionTxtSecurity").val() * 1,
        "baja": $("#ActuacionTxtBaja").val() * 1,
        "applicationUserId": User.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/CustomersFramework/Constraula/Data/ItemDataBase.aspx/SaveEconomics",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(dataSend, null, 2),
        "success": function (msg) {
            $(".botTempo").click();
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError);
            }
            else {
                PopupActionResult("S'ha desat la configuració econónimca correctament.", "Operació realitzada amb èxit");

                Data.IVA = $("#ActuacionTxtIVA").val() * 1;
                Data.DGBI = $("#ActuacionTxtDGBI").val() * 1;
                Data.Security = $("#ActuacionTxtSecurity").val() * 1;
                Data.Baixa = $("#ActuacionTxtBaja").val() * 1;
                ADJUDICACION_Context.IVA = Data.IVA;
                ADJUDICACION_Context.DGBI = Data.DGBI;
                ADJUDICACION_Context.Security = Data.Security;
                ADJUDICACION_Context.Baja = Data.Baja;
                ADJUDICACION_RenderEconomics();
            }
        },
        "error": function (msg) {
            $(".botTempo").click();
            PopupErrorSmall(msg.responseText);
            /*$.smallBox({
                title: "¡Se ha producido un error!",
                content: "<i class='fa fa-clock-o'></i> <i>" + msg.responseText + "</i>",
                color: "#C46A69",
                iconSmall: "fa fa-times fa-2x fadeInRight animated",
                timeout: 4000
            });*/
        }
    });

    return false;
}

function ADJUDICACION_FilterOTLayout() {
    var res = '<div style="margin-bottom:8px;"><table style="width:100%;">';
    res += '    <tr><td style="width:30px;">&nbsp;</td>';
    res += '    <td>';
    res += '        <label class="select">';
    res += '             <select name="DateType" id="DateType">';
    res += '                 <option value="0">Sel·leccionar data...</option >';
    res += '                 <option value="1">Data recepció</option>';
    res += '                 <option value="2">Data finalització</option>';
    res += '                 <option value="3">Data certificació</option>';
    res += '             </select > ';
    res += '             <i></i>';
    res += '        </label>';
    res += '    <td>&nbsp;';
    res += '        <div class="input-group" style="margin:5px;float:left;width:110px;">';
    res += '            <input type="text" name="FechaDesde" id="FechaDesde" value="" placeholder="Des de" class="form-control datepicker datepicker-regular" data-dateformat="Date" size="10">';
    res += '                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>';
    res += '        </div>';
    res += '        &nbsp;';
    res += '        <div class="input-group" style="margin:5px;float:left;width:110px;">';
    res += '            <input type="text" name="FechaHasta" id="FechaHasta" value="" placeholder="Fins" class="form-control datepicker datepicker-regular" data-dateformat="Date" size="10">';
    res += '            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>';
    res += '        </div>';
    res += '    <td align="right">';
    res += '        <label class="input">Barri:&nbsp;</label>';
    res += '    </td>';
    res += '    <td>';
    res += '        <label class="select">';
    res += '            <select name="AdjudicacionTxtBarrioId" id="AdjudicacionTxtBarrioId" style="background-color: transparent;"><option value="0">Sel·leccionar</option><option value="15">Can Peguera</option><option value="1">Canyelles</option><option value="2">Ciutat Meridiana</option><option value="3">Guineueta</option><option value="4">Nou Barris</option><option value="5">Porta</option><option value="6">Prosperitat</option><option value="7">Roquetes</option><option value="8">Torre Baró</option><option value="9">Torre Llobera</option><option value="10">Trinitat Nova</option><option value="11">Turó de la Peira</option><option value="12">Vallbona</option><option value="13">Verdum</option><option value="14">Vilapicina</option></select>';
    res += '            <i></i>';
    res += '        </label>';
    res += '    </td>';
    res += '    <td align="right">';
    res += '        <label class="input">Estat OT:&nbsp;</label>';
    res += '    </td>';
    res += '    <td>';
    res += '        <label class="select">';
    res += '             <select name="AdjudicacionTxtEstadoOTId" id="AdjudicacionTxtEstadoOTId"><option value="0">Sel·leccionar</option><option value="5" style="background-color: rgb(132, 222, 246);">Caput</option><option value="3" style="background-color: rgb(102, 204, 102);">Certificat</option><option value="1" style="background-color: rgb(231, 231, 214);">En Proces</option><option value="4" style="background-color: rgb(229, 229, 149);">Fet</option><option value="2" style="background-color: rgb(192, 194, 196);">Pendent</option></select>';
    res += '             <i></i>';
    res += '        </label>';
    res += '    </td>';
    res += '    <td>';
    res += '        &nbsp;&nbsp;<input type="checkbox" id="AdjudicacionTxtUrgent" name="AdjudicacionTxtUrgent" />&nbsp;Només&nbsp;urgents';
    res += '	</td>';
    res += '</tr>';
    res += '</table></div>';
    $("#OTFilterRow1").html(res)

    $("#OTFilterToogle").remove();
    $("#OTFilterRow2").hide();
    $("#OTFilterRow3").hide();
    $("#OTFilterRow4").hide();

    $("#DateType").on("change", ADJUDACION_FilterOT_DataTypeChange);

    $("#FechaDesde").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        numberOfMonths: 1,
        firstDay: 1
    });

    $("#FechaHasta").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        numberOfMonths: 1,
        firstDay: 1
    });

    $("#FechaDesde").on("change", ADJUDICACION_FilterOTFechaDesdeChanged);
    $("#FechaHasta").on("change", ADJUDICACION_FilterOTFechaHastaChanged);
}

function ADJUDACION_FilterOT_DataTypeChange() {
    $("#FechaDesde").attr("disabled", "disabled");
    $("#FechaHasta").attr("disabled", "disabled");
    $("#AdjudicacionTxtFechaRecepcionDesde").val("");
    $("#AdjudicacionTxtFechaRecepcionHasta").val("");
    $("#AdjudicacionTxtFechaEjecucionDesde").val("");
    $("#AdjudicacionTxtFechaEjecucionHasta").val("");
    $("#AdjudicacionTxtFechaCertificacionDesde").val("");
    $("#AdjudicacionTxtFechaCertificacionHasta").val("");
    var tipoFecha = $("#DateType").val() * 1;
    if (tipoFecha === 0) {
        $("#FechaDesde").attr("disabled", "disabled");
        $("#FechaHasta").attr("disabled", "disabled");
    }
    else {
        $("#FechaDesde").removeAttr("disabled");
        $("#FechaHasta").removeAttr("disabled");
    }

    ADJUDICACION_TableOTFilter();
}

function ADJUDICACION_FilterOTFechaDesdeChanged() {
    var tipoFecha = $("#DateType").val() * 1;
    var fecha = $("#FechaDesde").val();
    switch (tipoFecha) {
        case 1:
            $("#AdjudicacionTxtFechaRecepcionDesde").val(fecha);
            break;
        case 2:
            $("#AdjudicacionTxtFechaEjecucionDesde").val(fecha);
            break;
        case 3:
            $("#AdjudicacionTxtFechaCertificacionDesde").val(fecha);
            break;
    }
    ADJUDICACION_TableOTFilter();
}

function ADJUDICACION_FilterOTFechaHastaChanged() {
    var tipoFecha = $("#DateType").val() * 1;
    var fecha = $("#FechaHasta").val();
    switch (tipoFecha) {
        case 1:
            $("#AdjudicacionTxtFechaRecepcionHasta").val(fecha);
            break;
        case 2:
            $("#AdjudicacionTxtFechaEjecucionHasta").val(fecha);
            break;
        case 3:
            $("#AdjudicacionTxtFechaCertificacionHasta").val(fecha);
            break;
    }
    ADJUDICACION_TableOTFilter();
}