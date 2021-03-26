<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="DashboardReports.aspx.cs" Inherits="DashboardReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" runat="server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link href="/nv.d3/nv.d3.css" rel="stylesheet" type="text/css" />
    <style>
        .nvtooltip  .value {display:none;}
    </style>
    <style type="text/css">
        #scrollTableDiv{
            background-color:#fafaff;
            border:1px solid #e0e0e0;
            border-top:none;
            display:block;
        }

        .truncate {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            padding:0;
            margin:0;
        }

        TR:first-child{border-left:none;}
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-sm-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row">
                                    <div class="row">

									<div class="widget-box col-sm-12">
											<div class="widget-header widget-header-flat widget-header-small">
												<h5 class="widget-title">
													<i class="ace-icon fa fa-signal"></i>
													Evolución de incidencias y acciones
												</h5></div>

											<div class="widget-body">
												<div class="widget-main">
										<div class="infobox infobox-red">
											<div class="infobox-icon">
												<i class="icon-warning-sign"></i>
											</div>

											<div class="infobox-data">
												<span class="infobox-data-number">32</span>
												<div class="infobox-content">Incidencias abiertas</div>
											</div>

											<div class="stat stat-success" title="Último mes">8%</div>
										</div>
										<div class="infobox infobox-red">
                                            <div class="infobox-chart">
                                                <span class="sparkline" data-values="196,128,202,177,154,94,2,12,32">
                                                    <canvas width="44" height="33" style="display: inline-block; width: 44px; height: 33px; vertical-align: top;"></canvas>
                                                </span>
                                            </div>

											<div class="infobox-data">
												<span class="infobox-data-number">123</span>
												<div class="infobox-content">Incidencias</div>
											</div>

											<div class="badge badge-success">
												7.2%
												<i class="ace-icon fa fa-arrow-up"></i>
											</div>
										</div>

										<div class="infobox infobox-red">
											<div class="infobox-progress">
												<div class="easy-pie-chart percentage" data-percent="42" data-size="46" style="height: 46px; width: 46px; line-height: 45px;">
													<span class="percent">42</span>%
												<canvas height="46" width="46"></canvas></div>
											</div>

											<div class="infobox-data">
												<span class="infobox-text">Cerradas</span>

												<div class="infobox-content">
													Últimos 12 meses
												</div>
											</div>
										</div>
									<div class="vspace-12-sm"></div>

										<div class="infobox infobox-blue">
											<div class="infobox-icon">
												<i class="icon-tags"></i>
											</div>

											<div class="infobox-data">
												<span class="infobox-data-number">65</span>
												<div class="infobox-content">Acciones pendientes</div>
											</div>

											<div class="stat stat-success" title="Último mes">2%</div>
										</div>


										<div class="infobox infobox-blue">
                                            <div class="infobox-chart">
                                                <span class="sparkline" data-values="196,128,202,177,154,94,2,12,32">
                                                    <canvas width="44" height="33" style="display: inline-block; width: 44px; height: 33px; vertical-align: top;"></canvas>
                                                </span>
                                            </div>

											<div class="infobox-data">
												<span class="infobox-data-number">12</span>
												<div class="infobox-content">Acciones</div>
											</div>

											<div class="badge badge-success">
												33.4%
												<i class="ace-icon fa fa-arrow-down"></i>
											</div>
										</div>

										<div class="infobox infobox-blue">
											<div class="infobox-progress">
												<div class="easy-pie-chart percentage" data-percent="66" data-size="46" style="height: 46px; width: 46px; line-height: 45px;">
													<span class="percent">66</span>%
												<canvas height="46" width="46"></canvas></div>
											</div>

											<div class="infobox-data">
												<span class="infobox-text">Cerradas</span>

												<div class="infobox-content">
													Últimos 12 meses
												</div>
											</div>
										</div>
                                                    </div>
                                            </div>
                                    </div>

									<div class="vspace-12-sm"></div>

                                        <div class="row">        
                                                <div class="col-xs-12 col-sm-6 widget-container-col ui-sortable" style="min-height: 300px;">										
			                                        <div class="widget-box ui-sortable-handle" style="opacity: 1; z-index: 0;" id="PieWidget">
				                                        <div class="widget-header"><h5 class="widget-title">Incidencias por estado</h5></div>
				                                        <div class="widget-body">
					                                        <div id="Pie1" style="height:300px;"><svg class="nvd3-svg"></svg></div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 col-sm-6 widget-container-col ui-sortable" style="min-height: 300px;">
                                                    <div class="widget-box ui-sortable-handle" style="opacity: 1; z-index: 0;" id="tableWidget">
				                                        <div class="widget-header"><h5 class="widget-title">Incidencias por origen</h5></div>
				                                        <div class="widget-body">
				                                        <div class="widget-body">
					                                        <div id="Pie2" style="height:300px;"><svg class="nvd3-svg"></svg></div>
                                                        </div>
                                                        </div>
                                                    </div></div>
                                            </div>

                                </div>

									<div class="vspace-12-sm"></div>

                                        <div class="row">        
                                                <div class="col-xs-12 col-sm-4 widget-container-col ui-sortable" style="min-height: 300px;">										
			                                        <div class="widget-box ui-sortable-handle" style="opacity: 1; z-index: 0;" id="PieWidget">
				                                        <div class="widget-header"><h5 class="widget-title">Acciones por estado</h5></div>
				                                        <div class="widget-body">
					                                        <div id="Pie3" style="height:300px;"><svg class="nvd3-svg"></svg></div>
                                                        </div>
                                                    </div>
                                                </div>      
                                                <div class="col-xs-12 col-sm-4 widget-container-col ui-sortable" style="min-height: 300px;">										
			                                        <div class="widget-box ui-sortable-handle" style="opacity: 1; z-index: 0;" id="PieWidget">
				                                        <div class="widget-header"><h5 class="widget-title">Acciones por tipo</h5></div>
				                                        <div class="widget-body">
					                                        <div id="Pie4" style="height:300px;"><svg class="nvd3-svg"></svg></div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 col-sm-4 widget-container-col ui-sortable" style="min-height: 300px;">
                                                    <div class="widget-box ui-sortable-handle" style="opacity: 1; z-index: 0;" id="tableWidget">
				                                        <div class="widget-header"><h5 class="widget-title">Acciones por origen</h5></div>
				                                        <div class="widget-body">
				                                        <div class="widget-body">
					                                        <div id="Pie5" style="height:300px;"><svg class="nvd3-svg"></svg></div>
                                                        </div>
                                                        </div>
                                                    </div></div>
                                            </div>

                                </div>

									<div class="vspace-12-sm"></div>

                                        <div class="row">        
                                                <div class="col-sm-12 widget-container-col ui-sortable" style="min-height: 400px;">										
			                                        <div class="widget-box ui-sortable-handle" style="opacity: 1; z-index: 0;" id="PieWidget222">
				                                        <div class="widget-header"><h5 class="widget-title">Gasto por equipo</h5></div>
				                                        <div class="widget-body">
					                                        <div id="chart"><svg style="height:400px;"></svg></div>
                                                        </div>
                                                    </div>
                                                </div> 
                                            </div>

                                </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">  
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/assets/js/jquery.easy-pie-chart.min.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/assets/js/jquery.knob.min.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.2/d3.js" charset="utf-8"></script>
        <script type="text/javascript" src="/nv.d3/nv.d3.js"></script>
        <script type="text/javascript">

            function Resize() {
                var containerHeight = $(window).height();
                $("#ListDataDiv").height(containerHeight - 350);
            }

            window.onload = function () {
                Resize();
                $(".page-header .col-sm-4").html("<button class=\"btn btn-success\" type=\"button\" id=\"BtnShowSchedule\" onclick=\"document.location='Schedule.aspx';\"><i class=\"icon-calendar bigger-110\"></i> " + Dictionary.DashBoard_Calendar + "</button>");
                launchGraphics();
            }
            window.onresize = function () { Resize(); }

            
            jQuery(function ($) {
                $('.easy-pie-chart.percentage').each(function () {
                    var $box = $(this).closest('.infobox');
                    var barColor = $(this).data('color') || (!$box.hasClass('infobox-dark') ? $box.css('color') : 'rgba(255,255,255,0.95)');
                    var trackColor = barColor == 'rgba(255,255,255,0.95)' ? 'rgba(255,255,255,0.25)' : '#E2E2E2';
                    var size = parseInt($(this).data('size')) || 50;
                    $(this).easyPieChart({
                        barColor: barColor,
                        trackColor: trackColor,
                        scaleColor: false,
                        lineCap: 'butt',
                        lineWidth: parseInt(size / 10),
                        animate: 1000,
                        size: size
                    });
                })

                $('.sparkline').each(function () {
                    var $box = $(this).closest('.infobox');
                    var barColor = !$box.hasClass('infobox-dark') ? $box.css('color') : '#FFF';
                    $(this).sparkline('html',
                        {
                            tagValuesAttribute: 'data-values',
                            type: 'bar',
                            barColor: barColor,
                            chartRangeMin: $(this).data('min') || 0
                        });
                });
            });


            function launchGraphics() {
                var chartPie1;
                var chartPie1Data;
                var pie1data = [{ "label": "Identificada", "total": 0.17853546142578125, "value": 0.17853546142578125 }, { "label": "Analizada", "total": 0.4, "value": 0.4 }, { "label": "En progreso", "total": 0.2, "value": 0.2 }, { "label": "Cerrada", "total": 0.0806436538696289, "value": 0.0806436538696289 }];
                nv.addGraph(function () {
                    chartPie1 = nv.models.pieChart()
                        .x(function (d) { return d.label })
                        .y(function (d) { return d.value })
                        .height(300)
                        .showLabels(true)
                        .labelType("percent")
                        .donut(true).donutRatio(0.1);
                    chartPie1Data = d3.select("#Pie1 svg").datum(pie1data);
                    chartPie1Data.transition().duration(500).call(chartPie1);
                    nv.utils.windowResize(chartPie1.update);

                    return chartPie1;
                });
                var chartPie2;
                var chartPie2Data;
                var pie2data = [{ "label": "Interno", "total": 0.17853546142578125, "value": 0.17853546142578125 }, { "label": "Proveedor", "total": 0.4, "value": 0.4 }, { "label": "Cliente", "total": 0.0806436538696289, "value": 0.0806436538696289 }];
                nv.addGraph(function () {
                    chartPie2 = nv.models.pieChart()
                        .x(function (d) { return d.label })
                        .y(function (d) { return d.value })
                        .height(300)
                        .showLabels(true)
                        .labelType("percent")
                        .donut(true).donutRatio(0.1);
                    chartPie1Data = d3.select("#Pie2 svg").datum(pie2data);
                    chartPie1Data.transition().duration(500).call(chartPie2);
                    nv.utils.windowResize(chartPie2.update);

                    return chartPie1;
                });

                var chartPie3;
                var chartPie3Data;
                var pie3data = [{ "label": "Identificada", "total": 0.17853546142578125, "value": 0.17853546142578125 }, { "label": "Analizada", "total": 0.4, "value": 0.4 }, { "label": "En progreso", "total": 0.2, "value": 0.2 }, { "label": "Cerrada", "total": 0.8806436538696289, "value": 0.0806436538696289 }];
                nv.addGraph(function () {
                    chartPie3 = nv.models.pieChart()
                        .x(function (d) { return d.label })
                        .y(function (d) { return d.value })
                        .height(300)
                        .showLabels(true)
                        .labelType("percent")
                        .donut(true).donutRatio(0.1);
                    chartPie3Data = d3.select("#Pie3 svg").datum(pie3data);
                    chartPie3Data.transition().duration(500).call(chartPie3);
                    nv.utils.windowResize(chartPie3.update);

                    return chartPie3;
                });

                var chartPie4;
                var chartPie4Data;
                var pie4data = [{ "label": "Millora", "total": 0.17853546142578125, "value": 0.17853546142578125 }, { "label": "Correctiva", "total": 0.4, "value": 0.4 }, { "label": "Preventiva", "total": 0.0806436538696289, "value": 0.0806436538696289 }];
                nv.addGraph(function () {
                    chartPie4 = nv.models.pieChart()
                        .x(function (d) { return d.label })
                        .y(function (d) { return d.value })
                        .height(300)
                        .showLabels(true)
                        .labelType("percent")
                        .donut(true).donutRatio(0.1);
                    chartPie4Data = d3.select("#Pie4 svg").datum(pie4data);
                    chartPie4Data.transition().duration(500).call(chartPie4);
                    nv.utils.windowResize(chartPie4.update);

                    return chartPie4;
                });

                var chartPie5;
                var chartPi54Data;
                var pie5data = [{ "label": "Incidencia", "total": 0.17853546142578125, "value": 0.17853546142578125 }, { "label": "Análisis de riesgo", "total": 0.4, "value": 0.4 }, { "label": "Oportunidad", "total": 0.0806436538696289, "value": 0.0806436538696289 }, { "label": "Objetivo", "total": 0.0806436538696289, "value": 0.0806436538696289 }, { "label": "Auditoría", "total": 0.0806436538696289, "value": 0.0206436538696289 }, { "label": "Propuesta por direción", "total": 0.0306436538696289, "value": 0.0806436538696289 }];
                nv.addGraph(function () {
                    chartPie5 = nv.models.pieChart()
                        .x(function (d) { return d.label })
                        .y(function (d) { return d.value })
                        .height(300)
                        .showLabels(true)
                        .labelType("percent")
                        .donut(true).donutRatio(0.1);
                    chartPie5Data = d3.select("#Pie5 svg").datum(pie5data);
                    chartPie5Data.transition().duration(500).call(chartPie5);
                    nv.utils.windowResize(chartPie5.update);

                    return chartPie5;
                });

                var x = [{ "key": "Mantenimientos", "values": [{ "x": 0, "y": 2.942848235496889 }, { "x": 1, "y": 2.7932409212411837 }, { "x": 2, "y": 2.3324603775314126 }, { "x": 3, "y": 1.6724967962525301 }, { "x": 4, "y": 1.1305669445797843 }, { "x": 5, "y": 0.6034598074778262 }, { "x": 6, "y": 0.38418727600304375 }, { "x": 7, "y": 0.23167451071111317 }, { "x": 8, "y": 0.21239757249883692 }, { "x": 9, "y": 0.11488410127053075 }, { "x": 10, "y": 0.19762112191864262 }, { "x": 11, "y": 0.5277553523031155 }, { "x": 12, "y": 1.558662065112884 }, { "x": 13, "y": 2.1504061133829753 }, { "x": 14, "y": 4.028953287612453 }, { "x": 15, "y": 5.21863539337415 }, { "x": 16, "y": 4.266106666340443 }, { "x": 17, "y": 2.8399821689679694 }, { "x": 18, "y": 1.4743329505364835 }, { "x": 19, "y": 0.5429336656569288 }, { "x": 20, "y": 0.22701474715757908 }, { "x": 21, "y": 0.18680177126820907 }, { "x": 22, "y": 0.1984077097437487 }, { "x": 23, "y": 0.11465664699191334 }, { "x": 24, "y": 0.19113098721390584 }, { "x": 25, "y": 0.14946337587272296 }, { "x": 26, "y": 0.12197037773043748 }, { "x": 27, "y": 0.17398173793099106 }, { "x": 28, "y": 0.19515495853338644 }, { "x": 29, "y": 0.13737161240270732 }, { "x": 30, "y": 0.1487327196144544 }, { "x": 31, "y": 0.10177337116920625 }, { "x": 32, "y": 0.1427153954781729 }, { "x": 33, "y": 0.14167354123144135 }, { "x": 34, "y": 0.12302714870416742 }] }, { "key": "Reparaciones", "values": [{ "x": 0, "y": 0.12768342074253344 }, { "x": 1, "y": 0.16436746656819445 }, { "x": 2, "y": 0.16533146478232283 }, { "x": 3, "y": 0.14057818104842854 }, { "x": 4, "y": 0.17717706011630688 }, { "x": 5, "y": 0.16854635019889333 }, { "x": 6, "y": 0.17700088584784127 }, { "x": 7, "y": 0.17159950288058984 }, { "x": 8, "y": 0.18789352492555766 }, { "x": 9, "y": 0.228883539084681 }, { "x": 10, "y": 0.5952262008772091 }, { "x": 11, "y": 1.1370963970884125 }, { "x": 12, "y": 1.108275730467499 }, { "x": 13, "y": 1.7527412906614717 }, { "x": 14, "y": 4.782383457219163 }, { "x": 15, "y": 8.78688469120323 }, { "x": 16, "y": 9.430201079111825 }, { "x": 17, "y": 5.867649190689798 }, { "x": 18, "y": 2.2330200160869467 }, { "x": 19, "y": 0.5571084340841671 }, { "x": 20, "y": 0.18590519438232678 }, { "x": 21, "y": 0.11844073572628595 }, { "x": 22, "y": 0.1214314000903051 }, { "x": 23, "y": 0.15916985518680352 }, { "x": 24, "y": 0.12465785757069482 }, { "x": 25, "y": 0.1582952173376692 }, { "x": 26, "y": 0.13034446125758295 }, { "x": 27, "y": 0.1519265456351022 }, { "x": 28, "y": 0.1816215884733522 }, { "x": 29, "y": 0.10045271316585658 }, { "x": 30, "y": 0.15756435704067365 }, { "x": 31, "y": 0.13450669991620243 }, { "x": 32, "y": 0.10243766076681186 }, { "x": 33, "y": 0.19303209595146822 }, { "x": 34, "y": 0.14422778644298914 }] } ];

                nv.addGraph(function () {
                    var chart = nv.models.multiBarChart();

                    //var x = data();

                    //console.log(JSON.stringify(x, 2, null));

                    chart.xAxis
                        .tickFormat(d3.format(',f'));

                    chart.yAxis
                        .tickFormat(d3.format(',.1f'));

                    d3.select('#chart svg')
                        .datum(x)
                        .transition().duration(500)
                        .call(chart)
                        ;

                    nv.utils.windowResize(chart.update);

                    return chart;
                });

                function data() {
                    return stream_layers(3, 10 + Math.random() * 100, .1).map(function (data, i) {
                        var x = {
                            key: 'Stream' + i,
                            values: data
                        };


                        return x;
                    });
                }

                /* Inspired by Lee Byron's test data generator. */
                function stream_layers(n, m, o) {
                    if (arguments.length < 3) o = 0;
                    function bump(a) {
                        var x = 1 / (.1 + Math.random()),
                            y = 2 * Math.random() - .5,
                            z = 10 / (.1 + Math.random());
                        for (var i = 0; i < m; i++) {
                            var w = (i / m - y) * z;
                            a[i] += x * Math.exp(-w * w);
                        }
                    }
                    return d3.range(n).map(function () {
                        var a = [], i;
                        for (i = 0; i < m; i++) a[i] = o + o * Math.random();
                        for (i = 0; i < 5; i++) bump(a);
                        return a.map(stream_index);
                    });
                }

                /* Another layer generator using gamma distributions. */
                function stream_waves(n, m) {
                    return d3.range(n).map(function (i) {
                        return d3.range(m).map(function (j) {
                            var x = 20 * j / m - i / 3;
                            return 2 * x * Math.exp(-.5 * x);
                        }).map(stream_index);
                    });
                }

                function stream_index(d, i) {
                    return { x: i, y: Math.max(0, d) };
                }
            }


		
        </script>
</asp:Content>