//  the data that powers the bar chart, a simple array of numeric values
var chartdataBusinessRisk = [];
var graphicShowBusinessRisk = false;
var chartBusinessRisk;
var myColorsBusinessRisk = ["#1f77b4", "#ff7f0e", "#2ca02c"];
var graphicDataBusinessRisk;
var BusinessRisk;
var myDataBusinessRisk = [];
exampleDataBusinessRisk();

function RenderChartBusinessRisk() {

    nv.addGraph(function () {
        //Create BarChart
        chartBusinessRisk = nv.models.discreteBarChart()
            .x(function (d) { return d.label })    //Specify the data accessors.
            .y(function (d) { return d.value })
            .tooltips(false)
            .staggerLabels(true)    //Too many bars and not enough room? Try staggering labels.
            .showValues(true)       //...instead, show the bar value right on top of each bar.

            .transitionDuration(150)
            .forceY([0, 25])
            .color(myColorsBusinessRisk);

        chartBusinessRisk.yAxis.tickFormat(d3.format(',.0f'));
        chartBusinessRisk.yAxis.tickValues([5, 10, 15, 20]);
        chartBusinessRisk.valueFormat(d3.format('d'));

        //Add data to BarChart
        chartDataBusinessRisk = d3.select('#chartBusinessRisk svg').datum(graphicDataBusinessRisk);
        chartDataBusinessRisk.call(chartBusinessRisk);

        nv.utils.windowResize(chart.update);
        return chartBusinessRisk;
    });
}

RenderChartBusinessRisk();
$(".discreteBar").on("click", function (e) { console.log(e) });

function exampleDataBusinessRisk() {
    function y() {
        myColorsBusinessRisk = [];
        var y = [];
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

            myColorsBusinessRisk.push(finalColor);

        }
        return y;
    }

    graphicDataBusinessRisk = [
        {
            key: "Cumulative Return",
            values: y()
        }
    ]
}

function resizegrafico(transparent) {
    if (graphicShowBusinessRisk === true && transparent === false) {
        return false;
    }

    //var width = $('#ListForm').width();
    //var widthS = $('#svggraficBusinessRisk').width();
    //var width1 = $('#IncidentCostsTableData').width();
    //var width2 = $('#IncidentCostsTableVoid').width();
    //var width3 = $('#DivHistoryTableDiv').width();

    //if (widthS !== null) { if (width < widthS) { width = widthS; } }
    //if (width1 !== null) { if (width < width1) { width = width1; } }
    //if (width2 !== null) { if (width < width2) { width = width2; } }
    //if (width3 !== null) { if (width < width3) { width = width3; } }

    var width = $('#user-profile-2').width() - 100;

    chartBusinessRisk.width(width);
    chartBusinessRisk.update();

    var canvas = document.getElementById("svggraficBusinessRisk");
    var height = $("#svggraficBusinessRisk").height() - 50;

    d3.select("#chartBusinessRisk svg").append("line")
    .style("stroke", "gray")
    .attr("x1", 65)
    .attr("y1", height)
    .attr("x2", width)
    .attr("y2", height);

    d3.select("#chartBusinessRisk svg").append("line")
    .style("stroke", "gray")
    .attr("x1", 65)
    .attr("y1", 0)
    .attr("x2", 65)
    .attr("y2", height);
    graphicShowBusinessRisk = true;
}

function unresizegrafico() {
    graphicShowBusinessRisk = false;
}