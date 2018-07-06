//  the data that powers the bar chart, a simple array of numeric values
var chartdata = [];
var graphicshow = false;
var chart;
var myColors = ["#1f77b4", "#ff7f0e", "#2ca02c"];
var graphicdata;
var linedata;
var myData = new Array;
var chartData;
exampleData();

function RenderChart() {

    nv.addGraph(function () {
        //Create BarChart
        chart = nv.models.discreteBarChart()
            .x(function (d) { return d.label })    //Specify the data accessors.
            .y(function (d) { return d.value })
            .tooltips(false)
            .staggerLabels(true)    //Too many bars and not enough room? Try staggering labels.
            .showValues(true)       //...instead, show the bar value right on top of each bar.

            .transitionDuration(150)
            .forceY([0, 25])
            .color(myColors);

        chart.yAxis.tickFormat(d3.format(',.0f'));
        chart.yAxis.tickValues([5, 10, 15, 20]);
        chart.valueFormat(d3.format('d'));

        //Add data to BarChart
        chartData = d3.select('#chart svg').datum(graphicdata);
        chartData.call(chart);

        nv.utils.windowResize(chart.update);
        return chart;
    });
}

RenderChart();
$(".discreteBar").on("click", function (e) { console.log(e) });

function exampleData() {
    function y() {
        myColors = new Array();
        var y = new Array();
        var diferenciador = " ";
        console.log(BusinessRiskHistory);
        for (var x = 0; x < BusinessRiskHistory.length; x++) {
            var label = BusinessRiskHistory[x].DateStart;

            // In case we have different changes on the same day and we don't want to be overlaped
            for (var cont = 0; cont < BusinessRiskHistory.length; cont++) {
                if (label === BusinessRiskHistory[cont].DateStart && x !== cont) {
                    label = label + diferenciador;
                    diferenciador += " ";
                }
            }

            y.push(
                {
                    "label": label,
                    "value": BusinessRiskHistory[x].Result
                }
            );

            var finalColor = "#ffb752";
            if (BusinessRiskHistory[x].Assumed === false) {
                var limit = rule.Limit;
                if (limit < 0) {
                    limit = 0;
                }
                if (BusinessRiskHistory[x].Result < limit) {
                    finalColor = "#87b87f";
                }
                else {
                    finalColor = "#d15b47";
                }
            }

            myColors.push(finalColor);

        }
        return y;
    }

    graphicdata = [
        {
            key: "Cumulative Return",
            values: y()
        }
    ]
}

function resizegrafico(transparent) {
    if (graphicshow === true && transparent === false) {
        return false;
    }

    //var width = $('#ListForm').width();
    //var widthS = $('#svggrafic').width();
    //var width1 = $('#IncidentCostsTableData').width();
    //var width2 = $('#IncidentCostsTableVoid').width();
    //var width3 = $('#DivHistoryTableDiv').width();

    //if (widthS !== null) { if (width < widthS) { width = widthS; } }
    //if (width1 !== null) { if (width < width1) { width = width1; } }
    //if (width2 !== null) { if (width < width2) { width = width2; } }
    //if (width3 !== null) { if (width < width3) { width = width3; } }

    var width = $('#user-profile-2').width() - 100;

    chart.width(width);
    chart.update();

    var canvas = document.getElementById("svggrafic");
    var height = $("#svggrafic").height() - 50;

    d3.select("#chart svg").append("line")
    .style("stroke", "gray")
    .attr("x1", 65)
    .attr("y1", height)
    .attr("x2", width)
    .attr("y2", height);

    d3.select("#chart svg").append("line")
    .style("stroke", "gray")
    .attr("x1", 65)
    .attr("y1", 0)
    .attr("x2", 65)
    .attr("y2", height);
    graphicshow = true;
}

function unresizegrafico() {
    graphicshow = false;
}