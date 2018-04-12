//  the data that powers the bar chart, a simple array of numeric values
var graphicshow = false;
var actualRuleLimit = -1;
var chartdata = [];
for (var x = 0; x < BusinessRiskGraph.length; x++) {
    chartdata.push (BusinessRiskGraph[x].Result * 20);
}

var chart;
var myColors = ["#1f77b4", "#ff7f0e", "#2ca02c"];
var graphicdata;
var linedata;
var myData = new Array;
var chartData;

function GetRiskNameById(id)
{
    id = id * 1;
    for (var x = 0; x < BusinessRiskList.length; x++)
    {
        if (BusinessRiskList[x].BusinessRiskId === id)
        {
            return BusinessRiskList[x].Description;
        }
    }
    return '';
}

function RenderChart() {

    nv.addGraph(function () {
        //Create BarChart
        chart = nv.models.discreteBarChart()
            .x(function (d) { return d.label })
            .y(function (d) { return d.value })
            .tooltips(true)

            .showValues(true)
            .showXAxis(false)
            .transitionDuration(0)
            .forceY([0, 25])
            .color(myColors)
            .tooltipContent(function (key, x, y, e) {
                if (e.value >= 0) {
                    return '<div style="min-width:150px;-webkit-border-radius: 10px;-moz-border-radius: 10px;border-radius: 10px;border:3px solid #DBE6FF;background:rgba(242,246,252,0.8);-webkit-box-shadow: #B3B3B3 2px 2px 2px;-moz-box-shadow: #B3B3B3 2px 2px 2px; box-shadow: #B3B3B3 2px 2px 2px;padding-top:4px;padding-left:4px;"><strong>' + GetRiskNameById(x) + '</strong><p>' + Dictionary.Item_BusinessRisk_LabelField_Result + ': <strong>' + y + '</strong></p></div>';
                } else {
                    return "";
                }
            });

        chart.yAxis.tickFormat(d3.format(",.0f"));
        chart.yAxis.tickValues([5, 10, 15, 20]);
        chart.valueFormat(d3.format("d"));

        //Add data to BarChart
        chartData = d3.select("#chart svg").datum(graphicdata);
        chartData.call(chart);

        nv.utils.windowResize(chart.update);
        return chart;
    }, function () {
        console.log("callback");
        d3.selectAll(".discreteBar").on("click",
            function (e) {
                document.location = "/BusinessRiskView.aspx?id=" + e.label;
            });
    });
}

function exampleData() {
    function y() {
        myColors = new Array();
        var y = [];

        BusinessRiskGraph.sort(function (a, b) {
            return parseFloat(b.Result) - parseFloat(a.Result);
        });

        for (var x = 0; x < BusinessRiskGraph.length; x++) {
            var label = BusinessRiskGraph[x].Id.toString();
            y.push(
                {
                    "label": label,
                    "value": BusinessRiskGraph[x].Result
                }
            );

            var finalColor = "#ffb752";
            console.log(BusinessRiskGraph[x].Description, BusinessRiskGraph[x].Assumed);
            if (BusinessRiskGraph[x].Assumed === false && BusinessRiskGraph[x].FinalAction !== 1) {
                var limit = typeof actualRuleLimit === "undefined" ? 0 : actualRuleLimit;
                if (limit < 0 ) {
                    limit = BusinessRiskGraph[x].RuleLimit;
                }
                if (BusinessRiskGraph[x].Result < limit) {
                    finalColor = "#87b87f";
                }
                else {
                    finalColor = "#d15b47";
                }
            }

            myColors.push(finalColor);
            myData.push({ "label": label, "value": 5 });
        }
        return y;
    }

    linedata = [
        {
            key: "Cumulative Return",
            values: myData
        }
    ]

    graphicdata= [
        {
            key: "Cumulative Return",
            values: y()
        }
    ]
}

function resizegrafico(transparent)
{
    if (graphicshow === true && transparent === false)
    {
        return false;
    }

    var width = $("#ListDataHeader").width();
    var widthS = $("#svggrafic").width();
    var width1 = $("#IncidentCostsTableData").width();
    var width2 = $("#IncidentCostsTableVoid").width();
    var width3 = $("#ListActions").width();
    var width4 = $("#accion").width();

    if (widthS !== null) { if (width < widthS) { width = widthS; } }
    if (width1 !== null) { if (width < width1) { width = width1; } }
    if (width2 !== null) { if (width < width2) { width = width2; } }
    if (width3 !== null) { if (width < width3) { width = width3; } }
    if (width4 !== null) { if (width < width4) { width = width4; } }

    width = $("#widthTest").width() - 100;
    if (typeof width !== "undefined" && typeof chart !=="undefined") {
        chart.width(width);
        chart.update();
    }
    
    var canvas = document.getElementById("svggrafic");
    var height = $("#svggrafic").height() - 50;
    if (height === null)
    {
        height = 500;
    }

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

function DrawRuleLine()
{
    var width = $("#ListDataTable").width();
    if (width < 1)
    {
        width = $("#svggrafic").width();
    }
    var height = $("#svggrafic").height() - 48;
    d3.select("#chart svg").append("line")
        .style("stroke", "red")
        .attr("x1", 65)
        .attr("y1", height * (actualRuleLimit / -26) + height)
        .attr("x2", width)
        .attr("y2", height * (actualRuleLimit / -26) + height);

    if (RuleLimitFromDB !== actualRuleLimit) {
        d3.select("#chart svg").append("line")
            .style("stroke", "blue")
            .attr("x1", 65)
            .attr("y1", height * (RuleLimitFromDB / -26) + height)
            .attr("x2", width)
            .attr("y2", height * (RuleLimitFromDB / -26) + height)
            .style("stroke-dasharray", ("3, 3"));

        document.getElementById("BtnNewIpr").disabled = false;
    }
    else {
        document.getElementById("BtnNewIpr").disabled = true;
    }

    resizegrafico(true);
}

function unresizegrafico()
{
    graphicshow = false;
}