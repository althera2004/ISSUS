var firstLoad = true;

function customOnLoad() {
    console.log("customOnLoad");
    $("#LinkFollowViewAll").attr("href", $("#BtnFavorites").attr("href"));

    
    $("#breadcrumbs").append("<button style=\"float:right;margin-top:2px;\" type=\"reset\" id=\"reset-widgets\" class=\"btn btn-danger btn-white btn-bold btn-round\">" + Dictionary.Common_DashboardReset + "</button>");
    $("#reset-widgets").on("click", function () {
        ace.data.remove("demo", "widget-state");
        ace.data.remove("demo", "widget-order");
        document.location.reload();
    });

    $(".sparkline").each(function () {
        var $box = $(this).closest('.infobox');
        var barColor = !$box.hasClass("infobox-dark") ? $box.css("color") : "#FFF";
        $(this).sparkline("html",
            {
                "tagValuesAttribute": "data-values",
                "type": "bar",
                "barColor": barColor,
                "chartRangeMin": $(this).data("min") || 0
            });
    });


    $('#recent-box [data-rel="tooltip"]').tooltip({ "placement": tooltip_placement });
    function tooltip_placement(context, source) {
        var $source = $(source);
        var $parent = $source.closest(".tab-content");
        var off1 = $parent.offset();
        var w1 = $parent.width();

        var off2 = $source.offset();
        //var w2 = $source.width();

        if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) {
            return "right";
        }

        return "left";
    }

    firstLoad = false;
}

function customOnResize() {
    ResizeViewer();
}

function ResizeViewer() {
    var height = 190;
    var containerHeight = $(window).height();
    $("#pdfViewer").height(containerHeight - height);
}

jQuery(function ($) {
    // scrollables
    $(".scrollable").each(function () {
        var $this = $(this);
        $(this).ace_scroll({
            size: $this.attr("data-size") || 100
        });
    });

    $(".scrollable-horizontal").each(function () {
        var $this = $(this);
        $(this).ace_scroll(
            {
                "horizontal": true,
                "styleClass": "scroll-top",
                "size": $this.attr("data-size") || 500,
                "mouseWheelLock": true
            }
        ).css({ "padding-top": 12 });
    });

    $(window).on("resize.scroll_reset", function () {
        $(".scrollable-horizontal").ace_scroll("reset");
    });


    $("#id-checkbox-vertical").prop("checked", false).on("click", function () {
        $("#widget-toolbox-1").toggleClass("toolbox-vertical")
            .find(".btn-group").toggleClass("btn-group-vertical")
            .filter(":first").toggleClass("hidden")
            .parent().toggleClass("btn-toolbar");
    });


    // widget boxes
    // widget box drag & drop example
    $(".widget-container-col").sortable({
        "connectWith": ".widget-container-col",
        "items": "> .widget-box",
        "handle": ace.vars["touch"] ? ".widget-title" : false,
        "cancel": ".fullscreen",
        "opacity": 0.8,
        "revert": true,
        "forceHelperSize": true,
        "placeholder": "widget-placeholder",
        "forcePlaceholderSize": true,
        "tolerance": "pointer",
        "start": function (event, ui) {
            //when an element is moved, it's parent becomes empty with almost zero height.
            //we set a min-height for it to be large enough so that later we can easily drop elements back onto it
            ui.item.parent().css({ 'min-height': ui.item.height() })
            //ui.sender.css({"min-height":ui.item.height() , "background-color" : "#F5F5F5"})
        },
        "update": function (event, ui) {
            ui.item.parent({ "min-height": "" });

            //save widget positions
            var widget_order = {};
            $(".widget-container-col").each(function () {
                var container_id = $(this).attr("id");
                widget_order[container_id] = [];


                $(this).find("> .widget-box").each(function () {
                    var widget_id = $(this).attr("id");
                    widget_order[container_id].push(widget_id);
                });
            });

            ace.data.set("demo", "widget-order", widget_order, null, true);
        }
    });


    ///////////////////////

    //when a widget is shown/hidden/closed, we save its state for later retrieval
    $(document).on("shown.ace.widget hidden.ace.widget closed.ace.widget", ".widget-box", function (event) {
        var widgets = ace.data.get("demo", "widget-state", true);
        if (widgets === null) widgets = {};

        var id = $(this).attr("id");
        widgets[id] = event.type;
        ace.data.set("demo", "widget-state", widgets, null, true);
    });


    (function () {
        //restore widget order
        var container_list = ace.data.get("demo", "widget-order", true);
        if (container_list) {
            for (var container_id in container_list) if (container_list.hasOwnProperty(container_id)) {

                var widgets_inside_container = container_list[container_id];
                if (widgets_inside_container.length == 0) continue;

                for (var i = 0; i < widgets_inside_container.length; i++) {
                    var widget = widgets_inside_container[i];
                    $("#" + widget).appendTo("#" + container_id);
                }

            }
        }


        //restore widget state
        var widgets = ace.data.get("demo", "widget-state", true);
        if (widgets !== null) {
            for (var id in widgets) if (widgets.hasOwnProperty(id)) {
                var state = widgets[id];
                var widget = $("#" + id);
                if
                    (
                    (state === "shown" && widget.hasClass("collapsed"))
                    ||
                    (state === "hidden" && !widget.hasClass("collapsed"))
                ) {
                    widget.widget_box("toggleFast");
                }
                else if (state === "closed") {
                    widget.widget_box("closeFast");
                }
            }
        }

        $("#main-widget-container").removeClass("invisible");
    })();
});

function FormDirty() {
    return false;
}

function DASHBOARD_ReportColectivos(dias) {
    $(".DiasX").css("color", "#77f");
    $(".DiasX").css("font-weight", "normal");
    $("#Dias" + dias).css("color", "#0b4a98");
    $("#Dias" + dias).css("font-weight", "bold");
    $("#LlamadasColectivosDias").html(dias);
    $.ajax({
        "type": "POST",
        "url": "/Instances/AsyB/Data/DataReports.aspx/BashBoardLlamadaByColectivo30Dias",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({ "dias": dias }, null, 2),
        "success": function (msg) {
            console.log(msg.d);
            var data = [];
            eval("data = " + msg.d + ";");
            var chartPie1, chartPie1Data;
            var dataPie1 = data;//[{ "label": "AME", "value": 491 }, { "label": "ASISA", "value": 24679 }, { "label": "AVANTSALUD", "value": 1 }, { "label": "CASER", "value": 8415 }, { "label": "COLECTIVO DETRIAVALL", "value": 674 }, { "label": "DIVINA PASTORA", "value": 19940 }, { "label": "HNA", "value": 580 }, { "label": "ILERA", "value": 172 }, { "label": "MASCOTA Y SALUD", "value": 36586 }, { "label": "MEDIPREMIUM", "value": 518 }, { "label": "NATIONALE NETHERLANDEN", "value": 310 }, { "label": "NORTEHISPANA", "value": 487 }, { "label": "ODONTÓLOGOS", "value": 402 }, { "label": "SEGURCAIXA", "value": 34742 }];
            console.log(dataPie1);
            nv.addGraph(function () {
                chartPie1 = nv.models.pieChart()
                    .x(function (d) { return d.label; })
                    .y(function (d) { return d.value; })

                    //.legendPosition("right")
                    .labelThreshold(.05)
                    .height(300)
                    .showLabels(true)
                    .labelType("percent")
                    .donut(true).donutRatio(0.1);

                chartPie1Data = d3.select('#Pie1 svg').datum(dataPie1);
                chartPie1Data.transition().duration(500).call(chartPie1);
                nv.utils.windowResize(chartPie1.update);
                return chartPie1;
            });

            $(".easy-pie-chart.percentage").each(function () {
                var $box = $(this).closest(".infobox");
                var barColor = $(this).data("color") || (!$box.hasClass("infobox-dark") ? $box.css("color") : "rgba(255,255,255,0.95)");
                var trackColor = barColor == "rgba(255,255,255,0.95)" ? "rgba(255,255,255,0.25)" : "#E2E2E2";
                var size = parseInt($(this).data("size")) || 50;
                $(this).easyPieChart({
                    barColor: barColor,
                    trackColor: trackColor,
                    scaleColor: false,
                    lineCap: "butt",
                    lineWidth: parseInt(size / 10),
                    animate: ace.vars['old_ie'] ? false : 1000,
                    size: size
                });
            });
        },
        "error": function (msg) {
            $("#BtnAseguradoUpdateData").html("<i class=\"fa fa-save\"></i> Actualizar");
            $("#BtnAseguradoUpdateData").enable();
            PopupWarning(msg.responseText);
        }
    });
}

function DASHBOARD_ReportProvincias(dias) {
    $(".DiasProvinciasX").css("color", "#77f");
    $(".DiasProvinciasX").css("font-weight", "normal");
    $("#DiasProvincias" + dias).css("color", "#0b4a98");
    $("#DiasProvincias" + dias).css("font-weight", "bold");
    $("#LlamadasProvinciasDias").html(dias);
    $.ajax({
        "type": "POST",
        "url": "/Instances/AsyB/Data/DataReports.aspx/BashBoardLlamadaByProvincias30Dias",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({ "dias": dias }, null, 2),
        "success": function (msg) {
            console.log(msg.d);
            var data = [];
            eval("data = " + msg.d + ";");
            var chartPie1, chartPie1Data;
            var dataPie1 = data;
            console.log(dataPie1);
            nv.addGraph(function () {
                chartPie1 = nv.models.pieChart()
                    .x(function (d) { return d.label; })
                    .y(function (d) { return d.value; })

                    .legendPosition("right")
                    .labelThreshold(.05)
                    .height(300)
                    .showLabels(true)
                    .labelType("percent")
                    .donut(true).donutRatio(0.1);

                chartPie1Data = d3.select('#PieProvincias svg').datum(dataPie1);
                chartPie1Data.transition().duration(500).call(chartPie1);
                nv.utils.windowResize(chartPie1.update);
                return chartPie1;
            });

            $(".easy-pie-chart.percentage").each(function () {
                var $box = $(this).closest(".infobox");
                var barColor = $(this).data("color") || (!$box.hasClass("infobox-dark") ? $box.css("color") : "rgba(255,255,255,0.95)");
                var trackColor = barColor == "rgba(255,255,255,0.95)" ? "rgba(255,255,255,0.25)" : "#E2E2E2";
                var size = parseInt($(this).data("size")) || 50;
                $(this).easyPieChart({
                    barColor: barColor,
                    trackColor: trackColor,
                    scaleColor: false,
                    lineCap: "butt",
                    lineWidth: parseInt(size / 10),
                    animate: ace.vars['old_ie'] ? false : 1000,
                    size: size
                });
            });
        },
        "error": function (msg) {
            $("#BtnAseguradoUpdateData").html("<i class=\"fa fa-save\"></i> Actualizar");
            $("#BtnAseguradoUpdateData").enable();
            PopupWarning(msg.responseText);
        }
    });
}

function DASHBOARD_ReportLlamadaHistorico(dias) {
    var data = {
        "dias": dias
    };

    $.ajax({
        "type": "POST",
        "url": "/Instances/AsyB/Data/DataReports.aspx/BashBoardLlamadaHistorico",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({}, null, 2),
        "success": function (msg) {
            console.log(msg.d);
            var data = [];
            eval("data = " + msg.d + ";");
            console.log(data);

            var accData = [];
            var total = 0;
            for (var x = 0; x < data.length; x++) {
                total += data[x].value;
                accData.push({ "label": data[x].label, "value": total });
            }

            for (var y = 0; y < accData.length; y++) {
                if (accData[y].label === "0") {
                    $("#HistoricoLlamadasTotal").html(accData[y].value);
                }
                if (accData[y].label === "1") {
                    var foo = accData[y].value + 274;
                    $("#HistoricoLlamadas90Dias").html(foo);
                    $("#HistoricoLlamadas90DiasPercent").html(Math.round(foo * 100 / total));
                    $("#HistoricoLlamadas90DiasGraph").data('easyPieChart').update(Math.round(foo * 100 / total));
                }
                if (accData[y].label === "2") {
                    $("#HistoricoLlamadas30Dias").html(accData[y].value);
                    $("#HistoricoLlamadas30DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoLlamadas30DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
                if (accData[y].label === "3") {
                    $("#HistoricoLlamadas7Dias").html(accData[y].value);
                    $("#HistoricoLlamadas7DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoLlamadas7DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
            }
        },
        "error": function (msg) { }
    });
}

function DASHBOARD_ReportCentrosHistorico() {
    $.ajax({
        "type": "POST",
        "url": "/Instances/AsyB/Data/DataReports.aspx/BashBoardCentrosHistorico",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({}, null, 2),
        "success": function (msg) {
            console.log(msg.d);
            var data = [];
            eval("data = " + msg.d + ";");
            console.log(data);

            var accData = [];
            var total = 0;
            for (var x = 0; x < data.length; x++) {
                total += data[x].value;
                accData.push({ "label": data[x].label, "value": total });
            }

            for (var y = 0; y < accData.length; y++) {
                if (accData[y].label === "0") {
                    $("#HistoricoCentrosTotal").html(accData[y].value);
                }
                if (accData[y].label === "1") {
                    $("#HistoricoCentros90Dias").html(accData[y].value);
                    $("#HistoricoCentros90DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoCentros90DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
                if (accData[y].label === "2") {
                    $("#HistoricoCentros30Dias").html(accData[y].value);
                    $("#HistoricoCentros30DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoCentros30DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
                if (accData[y].label === "3") {
                    $("#HistoricoCentros7Dias").html(accData[y].value);
                    $("#HistoricoCentros7DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoCentros7DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
            }
        },
        "error": function (msg) { }
    });
}

function DASHBOARD_ReportEspecialistasHistorico() {
    $.ajax({
        "type": "POST",
        "url": "/Instances/AsyB/Data/DataReports.aspx/BashBoardEspecialistasHistorico",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({}, null, 2),
        "success": function (msg) {
            console.log(msg.d);
            var data = [];
            eval("data = " + msg.d + ";");
            console.log("ESPECIALISTAS", data);

            var accData = [];
            var total = 0;
            for (var x = 0; x < data.length; x++) {
                total += data[x].value;
                accData.push({ "label": data[x].label, "value": total });
            }

            $("#HistoricoEspecialistasTotal").html(total);
            for (var y = 0; y < accData.length; y++) {
                if (accData[y].label === "1") {
                    $("#HistoricoEspecialistas90Dias").html(accData[y].value);
                    $("#HistoricoEspecialistas90DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoEspecialistas90DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
                if (accData[y].label === "2") {
                    $("#HistoricoEspecialistas30Dias").html(accData[y].value);
                    $("#HistoricoEspecialistas30DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoEspecialistas30DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
                if (accData[y].label === "3") {
                    $("#HistoricoEspecialistas7Dias").html(accData[y].value);
                    $("#HistoricoEspecialistas7DiasPercent").html(Math.round(accData[y].value * 100 / total));
                    $("#HistoricoEspecialistas7DiasGraph").data('easyPieChart').update(Math.round(accData[y].value * 100 / total));
                }
            }
        },
        "error": function (msg) { }
    });
}

function GoInformeMensual() {
    var id = $("#CmbColectivo").val();
    var mes = $("#CmbMonth").val();
    var year = $("#CmbYear").val();
    window.open("/Instances/AsyB/export/informellamadas.aspx?id=" + id + "&m=" + mes + "&y=" + year);
}

function DASHBOARD_Especialistas() {
    $.ajax({
        "type": "POST",
        "url": "/Instances/AsyB/Data/DataReports.aspx/DashboardEspecialistas",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({}, null, 2),
        "success": function (msg) {
            var data = [
                {
                    "key": "Series 1",
                    "color": "#d67777",
                    "values": []
                }
            ];
            eval("data[0].values = " + msg.d + ";");
            console.log(data);
            nv.addGraph(function () {
                var chart = nv.models.multiBarHorizontalChart()
                    .x(function (d) { return d.label; })
                    .y(function (d) { return d.value; })
                    .margin({ "top": 30, "right": 20, "bottom": 50, "left": 300 })
                    .showValues(false)
                    .groupSpacing(0.0)
                    .showControls(false);

                chart.yAxis.tickFormat(d3.format(",.2f"));

                d3.select("#chartEspecialistas svg")
                    .datum(data)
                    .call(chart);

                nv.utils.windowResize(chart.update);

                return chart;
            });
        },
        "error": function () { }
    });
}

function DASHBOARD_EspecialistasProvincias() {
    var dataSend = {
        "provinciaId": $("#EspecialistasProvincias").val() * 1,
        "procedenciaLlamadaId": $("#EspecialistasColectivo").val() * 1
    };
    $.ajax({
        "type": "POST",
        "url": "/Instances/AsyB/Data/DataReports.aspx/DashboardEspecialistas",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(dataSend, null, 2),
        "success": function (msg) {
            var data = [
                {
                    "key": "Especialidades",
                    "values": []
                }
            ];
            eval("data[0].values = " + msg.d + ";");
            console.log("Especialidades provincias", data);
            nv.addGraph(function () {
                var chart = nv.models.discreteBarChart()
                    .margin(10, 10, 10, 500)
                    .x(function (d) { return d.label; })
                    .y(function (d) { return d.value; })
                    .staggerLabels(false)
                    .tooltips(true)
                    .showValues(true)
                    .duration(350);

                d3.select('#chartEspecialistasProvincias svg')
                    .datum(data)
                    .call(chart);

                nv.utils.windowResize(chart.update);
                var xTicks = d3.select('.nv-x.nv-axis > g').selectAll('g');
                xTicks
                    .selectAll('text')
                    .attr('transform', function (d, i, j) { return 'translate (20, 25) rotate(45 0,0)' });

                return chart;
            });
        },
        "error": function () { }
    });

    //Each bar represents a single discrete quantity.
    function exampleData() {
        return [
            {
                key: "Cumulative Return",
                values: [
                    {
                        "label": "A Label",
                        "value": 29.765957771107
                    },
                    {
                        "label": "B Label",
                        "value": 0
                    },
                    {
                        "label": "C Label",
                        "value": 32.807804682612
                    },
                    {
                        "label": "D Label",
                        "value": 196.45946739256
                    },
                    {
                        "label": "E Label",
                        "value": 0.19434030906893
                    },
                    {
                        "label": "F Label",
                        "value": 98.079782601442
                    },
                    {
                        "label": "G Label",
                        "value": 13.925743130903
                    },
                    {
                        "label": "H Label",
                        "value": 5.1387322875705
                    }
                ]
            }
        ];
    }
}

function GetFKItemProcedenciaLlamadaCallbak() {
    console.log(FK.ProcedenciaLlamada.Data);
    var res = "<option value=\"0\">Seleccionar...</option>";
    res += "<option value=\"-1\">AsyB</option>";
    for (var x = 0; x < FK.ProcedenciaLlamada.Data.length; x++) {
        var p = FK.ProcedenciaLlamada.Data[x];
        if (p.Active === true) {
            res += "<option value=\"" + p.Id + "\">" + p.Description + "</option>";
        }
    }

    $("#EspecialistasColectivo").html(res);
    $("#CmbColectivo").html(res);
}