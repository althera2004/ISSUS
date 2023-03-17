<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" runat="server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" href="assets/css/ace.min.reports.css" />
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

        .percentage { font-size:12px;}

		.noborder { border:none; }
        .widget-container-col{border:1px solid #000;}

        TR:first-child{border-left:none;}
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <div class="col-sm-12">
        <!-- PAGE CONTENT BEGINS -->
        <div class="invisible" id="main-widget-container">
            <div class="row">
                <div class="col-sm-12 widget-container-col" id="widget-container-col-main">
                    <div class="widget-box" id="widget-box-4">
                        <div class="widget-header widget-header-large">
                            <h4 class="widget-title">Uso de espacio en disco</h4>

                            <div class="widget-toolbar">
                                <a href="#" data-action="collapse">
                                    <i class="ace-icon fa fa-chevron-up"></i>
                                </a>

                                <a href="#" data-action="close">
                                    <i class="ace-icon fa fa-times"></i>
                                </a>
                            </div>
                        </div>

                        <div class="widget-body">
                            <div class="widget-main">
                                <h5><%=this.Dictionary["Item_Attach_DiskQuote"] %>&nbsp;<%=this.AsignedQuote %>MB &nbsp;<i class="fa fa-eye" id="TablediskToogle" onclick="REPORTS_TableDiskToogle();"></i></h5>
                                <div class="progress m-t-xs full progress-striped">
                                    <div id="DiskProgressBar" style="width: 75%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="75" role="progressbar" class=" progress-bar">
                                        75%
                                    </div>
                                </div>
                                <div id="TableDisk" style="display: none;">
                                    <table style="width: 100%;">
                                        <thead>
                                            <tr>
                                                <th>Ítem</th>
                                                <th style="text-align: right; width: 150px;">Espacio ocupado</th>
                                                <th style="text-align: right; width: 75px;">%</th>
                                            </tr>
                                        </thead>
                                        <tbody id="TableDiskBody" cellpadding="4" style="font: normal 12px Arial"></tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6 widget-container-col" id="widget-container-col-1">
                    <div class="widget-box widget-color-blue" id="widget-box-1">
                        <div class="widget-header">
                            <h5 class="widget-title"><i class="fa fa-exclamation-triangle"></i>&nbsp;No conformitats</h5>
                            <div class="widget-toolbar">
                                <a href="#" data-action="collapse">
                                    <i class="ace-icon fa fa-chevron-up"></i>
                                </a>
                                <a href="#" data-action="close">
                                    <i class="ace-icon fa fa-times"></i>
                                </a>
                            </div>
                        </div>

                        <div class="widget-body">
                            <div class="widget-main">
                                <div>
                                    <div class="infobox infobox-red noborder">
                                        <div class="infobox-icon">
                                            <i class="ace-icon fa fa-exclamation-triangle"></i>
                                        </div>

                                        <div class="infobox-data">
                                            <span class="infobox-data-number"><%=this.ChartIncidentTotal %></span>
                                            <div class="infobox-content">No conformitats</div>
                                        </div>
                                    </div>
                                </div>

                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-progress">
                                        <div class="easy-pie-chart percentage" data-percent="<%=this.ChartIncident1 %>" data-size="35">
                                            <span class="percent"><%=this.ChartIncident1 %></span>%
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">Identificades</span>
                                    </div>
                                </div>
                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-progress">
                                        <div class="easy-pie-chart percentage" data-percent="<%=this.ChartIncident2 %>" data-size="35">
                                            <span class="percent"><%=this.ChartIncident2 %></span>%
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">Analitzades</span>
                                    </div>
                                </div>
                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-progress">
                                        <div class="easy-pie-chart percentage" data-percent="<%=this.ChartIncident3 %>" data-size="35">
                                            <span class="percent"><%=this.ChartIncident3 %></span>%
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">En progrés</span>
                                    </div>
                                </div>
                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-icon">
                                        <i class="ace-icon fa fa-euro"></i>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-data-number"><%=this.ChartIncident4 %></span>
                                        <div class="infobox-content">Costos</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 widget-container-col" id="widget-container-col-2">
                    <div class="widget-box widget-color-blue" id="widget-box-Actions">
                        <div class="widget-header">
                            <h5 class="widget-title"><i class="fa fa-exclamation-triangle"></i>&nbsp;Accions</h5>
                            <div class="widget-toolbar">
                                <a href="#" data-action="collapse">
                                    <i class="ace-icon fa fa-chevron-up"></i>
                                </a>
                                <a href="#" data-action="close">
                                    <i class="ace-icon fa fa-times"></i>
                                </a>
                            </div>
                        </div>

                        <div class="widget-body">
                            <div class="widget-main">
                                <div>
                                    <div class="infobox infobox-red noborder">
                                        <div class="infobox-icon">
                                            <i class="ace-icon fa fa-exclamation-triangle"></i>
                                        </div>

                                        <div class="infobox-data">
                                            <span class="infobox-data-number"><%=this.ChartIncidentActionTotal %></span>
                                            <div class="infobox-content">No conformitats</div>
                                        </div>
                                    </div>
                                </div>

                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-progress">
                                        <div class="easy-pie-chart percentage" data-percent="<%=this.ChartIncidentAction1 %>" data-size="35">
                                            <span class="percent"><%=this.ChartIncidentAction1 %></span>%
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">Identificades</span>
                                    </div>
                                </div>
                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-progress">
                                        <div class="easy-pie-chart percentage" data-percent="<%=this.ChartIncidentAction2 %>" data-size="35">
                                            <span class="percent"><%=this.ChartIncidentAction2 %></span>%
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">Analitzades</span>
                                    </div>
                                </div>
                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-progress">
                                        <div class="easy-pie-chart percentage" data-percent="<%=this.ChartIncidentAction3 %>" data-size="35">
                                            <span class="percent"><%=this.ChartIncidentAction3 %></span>%
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">En progrés</span>
                                    </div>
                                </div>
                                <div class="infobox infobox-blue2 noborder">
                                    <div class="infobox-progress">
                                        <div class="easy-pie-chart percentage" data-percent="<%=this.ChartIncidentAction4 %>" data-size="35">
                                            <span class="percent"><%=this.ChartIncidentAction4 %></span>%
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">Tancades</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">

                <div class="col-xs-12 col-sm-6 widget-container-col" id="widget-container-col-21">
                </div>

                <div class="col-xs-12 col-sm-6 widget-container-col" id="widget-container-col-22">
                </div>
            </div>
            <!-- /.row -->


            <div class="row">
                <div class="col-xs-12 col-sm-6 widget-container-col" id="widget-container-col-3">
                    <div class="widget-box widget-color-orange collapsed" id="widget-box-3">
                        <div class="widget-header widget-header-small">
                            <h6 class="widget-title">
                                <i class="ace-icon fa fa-sort"></i>
                                Small Header & Collapsed
                            </h6>

                            <div class="widget-toolbar">
                                <a href="#" data-action="settings">
                                    <i class="ace-icon fa fa-cog"></i>
                                </a>

                                <a href="#" data-action="reload">
                                    <i class="ace-icon fa fa-refresh"></i>
                                </a>

                                <a href="#" data-action="collapse">
                                    <i class="ace-icon fa fa-plus" data-icon-show="fa-plus" data-icon-hide="fa-minus"></i>
                                </a>

                                <a href="#" data-action="close">
                                    <i class="ace-icon fa fa-times"></i>
                                </a>
                            </div>
                        </div>

                        <div class="widget-body">
                            <div class="widget-main">
                                <div class="m">
                                    <div class="progress m-t-xs full progress-striped">
                                        <div style="width: 75%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="75" role="progressbar" class="progress-bar progress-bar-warning">
                                            175%
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-xs-12 col-sm-6 widget-container-col" id="widget-container-col-4">
                </div>
            </div>
            <div class="text-center">
                <button type="reset" id="reset-widgets" class="btn btn-danger btn-white btn-bold btn-round">Reset Position/State</button>
            </div>
        </div>
        <!-- PAGE CONTENT ENDS -->
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">  
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/assets/js/jquery.easy-pie-chart.min.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/assets/js/jquery.knob.min.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.2/d3.js" charset="utf-8"></script>
        <script type="text/javascript" src="/nv.d3/nv.d3.js"></script>
        <script type="text/javascript">
            var diskQuote = <%=this.DiskQuote %>;
            pageType = "DashBoard";

            function Resize() {
                var containerHeight = $(window).height();
                $("#ListDataDiv").height(containerHeight - 350);
            }

            window.onload = function () {
                Resize();
                $(".page-header .col-sm-4").html("<button class=\"btn btn-success\" type=\"button\" id=\"BtnShowSchedule\" onclick=\"document.location='Schedule.aspx';\"><i class=\"icon-calendar bigger-110\"></i> " + Dictionary.DashBoard_Calendar + "</button>");
                //launchGraphics();
            }
            window.onresize = function () { Resize(); }

            

            jQuery(function ($) {

                $('#simple-colorpicker-1').ace_colorpicker({ pull_right: true }).on('change', function () {
                    var color_class = $(this).find('option:selected').data('class');
                    var new_class = 'widget-box';
                    if (color_class != 'default') new_class += ' widget-color-' + color_class;
                    $(this).closest('.widget-box').attr('class', new_class);
                });


                // scrollables
                $('.scrollable').each(function () {
                    var $this = $(this);
                    $(this).ace_scroll({
                        size: $this.attr('data-size') || 100,
                        //styleClass: 'scroll-left scroll-margin scroll-thin scroll-dark scroll-light no-track scroll-visible'
                    });
                });
                $('.scrollable-horizontal').each(function () {
                    var $this = $(this);
                    $(this).ace_scroll(
                        {
                            horizontal: true,
                            styleClass: 'scroll-top',//show the scrollbars on top(default is bottom)
                            size: $this.attr('data-size') || 500,
                            mouseWheelLock: true
                        }
                    ).css({ 'padding-top': 12 });
                });

                $(window).on('resize.scroll_reset', function () {
                    $('.scrollable-horizontal').ace_scroll('reset');
                });


                $('#id-checkbox-vertical').prop('checked', false).on('click', function () {
                    $('#widget-toolbox-1').toggleClass('toolbox-vertical')
                        .find('.btn-group').toggleClass('btn-group-vertical')
                        .filter(':first').toggleClass('hidden')
                        .parent().toggleClass('btn-toolbar')
                });

                /**
                //or use slimScroll plugin
                $('.slim-scrollable').each(function () {
                    var $this = $(this);
                    $this.slimScroll({
                        height: $this.data('height') || 100,
                        railVisible:true
                    });
                });
                */


                /**$('.widget-box').on('setting.ace.widget' , function(e) {
                    e.preventDefault();
                });*/

                /**
                $('.widget-box').on('show.ace.widget', function(e) {
                    //e.preventDefault();
                    //this = the widget-box
                });
                $('.widget-box').on('reload.ace.widget', function(e) {
                    //this = the widget-box
                });
                */

                //$('#my-widget-box').widget_box('hide');



                // widget boxes
                // widget box drag & drop example
                $('.widget-container-col').sortable({
                    connectWith: '.widget-container-col',
                    items: '> .widget-box',
                    handle: ace.vars['touch'] ? '.widget-title' : false,
                    cancel: '.fullscreen',
                    opacity: 0.8,
                    revert: true,
                    forceHelperSize: true,
                    placeholder: 'widget-placeholder',
                    forcePlaceholderSize: true,
                    tolerance: 'pointer',
                    start: function (event, ui) {
                        //when an element is moved, it's parent becomes empty with almost zero height.
                        //we set a min-height for it to be large enough so that later we can easily drop elements back onto it
                        ui.item.parent().css({ 'min-height': ui.item.height() })
                        //ui.sender.css({'min-height':ui.item.height() , 'background-color' : '#F5F5F5'})
                    },
                    update: function (event, ui) {
                        ui.item.parent({ 'min-height': '' })
                        //p.style.removeProperty('background-color');


                        //save widget positions
                        var widget_order = {}
                        $('.widget-container-col').each(function () {
                            var container_id = $(this).attr('id');
                            widget_order[container_id] = []


                            $(this).find('> .widget-box').each(function () {
                                var widget_id = $(this).attr('id');
                                widget_order[container_id].push(widget_id);
                                //now we know each container contains which widgets
                            });
                        });

                        ace.data.set('demo', 'widget-order', widget_order, null, true);
                    }
                });


                ///////////////////////

                //when a widget is shown/hidden/closed, we save its state for later retrieval
                $(document).on('shown.ace.widget hidden.ace.widget closed.ace.widget', '.widget-box', function (event) {
                    var widgets = ace.data.get('demo', 'widget-state', true);
                    if (widgets == null) widgets = {}

                    var id = $(this).attr('id');
                    widgets[id] = event.type;
                    ace.data.set('demo', 'widget-state', widgets, null, true);
                });


                (function () {
                    //restore widget order
                    var container_list = ace.data.get('demo', 'widget-order', true);
                    if (container_list) {
                        for (var container_id in container_list) if (container_list.hasOwnProperty(container_id)) {

                            var widgets_inside_container = container_list[container_id];
                            if (widgets_inside_container.length == 0) continue;

                            for (var i = 0; i < widgets_inside_container.length; i++) {
                                var widget = widgets_inside_container[i];
                                $('#' + widget).appendTo('#' + container_id);
                            }

                        }
                    }


                    //restore widget state
                    var widgets = ace.data.get('demo', 'widget-state', true);
                    if (widgets != null) {
                        for (var id in widgets) if (widgets.hasOwnProperty(id)) {
                            var state = widgets[id];
                            var widget = $('#' + id);
                            if
                                (
                                (state == 'shown' && widget.hasClass('collapsed'))
                                ||
                                (state == 'hidden' && !widget.hasClass('collapsed'))
                            ) {
                                widget.widget_box('toggleFast');
                            }
                            else if (state == 'closed') {
                                widget.widget_box('closeFast');
                            }
                        }
                    }


                    $('#main-widget-container').removeClass('invisible');


                    //reset saved positions and states
                    $('#reset-widgets').on('click', function () {
                        ace.data.remove('demo', 'widget-state');
                        ace.data.remove('demo', 'widget-order');
                        document.location.reload();
                    });

                })();

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
                        animate: ace.vars['old_ie'] ? false : 1000,
                        size: size
                    });
                })

                RenderDiskQuote();
            });


            function RenderDiskQuote() {
                var res = "";
                var ocupado = 0;
                for (var x = 0; x < diskQuote.length - 1; x++) {
                    res += "<tr>";
                    res += "  <td style=\"text-align:left;\">" + diskQuote[x].label + "</td>";
                    res += "  <td style=\"text-align:right;\">&nbsp;&nbsp;" + ToMoneyFormat(diskQuote[x].total, 2) + " MB</td>";
                    res += "  <td style=\"text-align:right;\">&nbsp;&nbsp;" + ToMoneyFormat(diskQuote[x].value, 2) + " %</td>";
                    res += "</tr > ";
                    ocupado += diskQuote[x].value;
                }

                $("#TableDiskBody").html(res);
                $("#DiskProgressBar").html(ToMoneyFormat(ocupado, 2) + "%");
                $("#DiskProgressBar").css("width", + ocupado + "%");
                $("#DiskProgressBar").attr("aria-valuenow", ocupado);

                if (ocupado < 80) { $("#DiskProgressBar").addClass(" progress-bar-success"); }
                else if (ocupado < 100) { $("#DiskProgressBar").addClass(" progress-bar-warning"); }
                else { $("#DiskProgressBar").addClass(" progress-bar-danger"); }
            }

            function REPORTS_TableDiskToogle() {
                if ($("#TablediskToogle").hasClass("fa-eye")) {
                    $("#TablediskToogle").removeClass("fa-eye");
                    $("#TablediskToogle").addClass("fa-eye-slash");
                    $("#TableDisk").show();
                } else {
                    $("#TablediskToogle").removeClass("fa-eye-slash");
                    $("#TablediskToogle").addClass("fa-eye");
                    $("#TableDisk").hide();
                }
            }
		
        </script>
</asp:Content>