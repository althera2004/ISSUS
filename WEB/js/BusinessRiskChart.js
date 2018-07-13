//  the data that powers the bar chart, a simple array of numeric values
var graphicShowBusinessRisk = false;
var actualRuleLimitBusinessRisk = -1;
var chartdataBusinessRisk = [];
for (var x = 0; x < BusinessRiskGraph.length; x++) {
    chartdataBusinessRisk.push (BusinessRiskGraph[x].Result * 20);
}

var chartBusinessRisk;
var myColorsBusinessRisk = ["#1f77b4", "#ff7f0e", "#2ca02c"];
var graphicDataBusinessRisk;
var linedataBusinessRisk;
var myDataBusinessRisk = [];

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
    return "";
}

function RenderChartBusinessRisk() {

    nv.addGraph(function () {
        //Create BarChart
        chartBusinessRisk = nv.models.discreteBarChart()
            .x(function (d) { return d.label })
            .y(function (d) { return d.value })
            .tooltips(true)

            .showValues(true)
            .showXAxis(false)
            .transitionDuration(0)
            .forceY([0, 25])
            .color(myColorsBusinessRisk)
            .tooltipContent(function (key, x, y, e) {
                if (e.value >= 0) {
                    return '<div style="min-width:150px;-webkit-border-radius: 40px;-moz-border-radius: 10px;border-radius: 10px;border:3px solid #DBE6FF;background:rgba(242,246,252,0.8);-webkit-box-shadow: #B3B3B3 2px 2px 2px;-moz-box-shadow: #B3B3B3 2px 2px 2px; box-shadow: #B3B3B3 2px 2px 2px;padding-top:4px;padding-left:4px;"><strong>' + GetRiskNameById(x) + '</strong><p>' + Dictionary.Item_BusinessRisk_LabelField_Result + ': <strong>' + y + '</strong></p></div>';
                } else {
                    return "";
                }
            });

        chartBusinessRisk.yAxis.tickFormat(d3.format(",.0f"));
        chartBusinessRisk.yAxis.tickValues([5, 10, 15, 20]);
        chartBusinessRisk.valueFormat(d3.format("d"));

        //Add data to BarChart
        chartDataBusinessRisk = d3.select("#chartBusinessRisk svg").datum(graphicDataBusinessRisk);
        chartDataBusinessRisk.call(chartBusinessRisk);

        nv.utils.windowResize(chartBusinessRisk.update);
        return chartBusinessRisk;
    }, function () {
        //console.log("callback");
        d3.selectAll(".discreteBar").on("click",
            function (e) {
                document.location = "/BusinessRiskView.aspx?id=" + e.label;
            });
    });
}

function exampleDataBusinessRisk() {
    function y() {
        myColorsBusinessRisk = [];
        myDataBusinessRisk = [];
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
            //console.log(BusinessRiskGraph[x].Description, BusinessRiskGraph[x].Assumed);
            if (BusinessRiskGraph[x].Assumed === false && BusinessRiskGraph[x].FinalAction !== 1) {
                var limit = typeof actualRuleLimitBusinessRisk === "undefined" ? 0 : actualRuleLimitBusinessRisk;
                if (limit < 0) {
                    limit = BusinessRiskGraph[x].RuleLimit;
                }
                if (BusinessRiskGraph[x].Result < limit) {
                    finalColor = "#87b87f";
                }
                else {
                    finalColor = "#d15b47";
                }
            }

            myColorsBusinessRisk.push(finalColor);
            myDataBusinessRisk.push({ "label": label, "value": 5 });
            console.log("BR", myDataBusinessRisk);
        }
        return y;
    }

    linedataBusinessRisk = [
        {
            "key": "Cumulative Return",
            "values": myDataBusinessRisk
        }
    ]

    graphicDataBusinessRisk = [
        {
            "key": "Cumulative Return",
            "values": y()
        }
    ]
}

function resizegrafico(transparent)
{
    if (graphicShowBusinessRisk === true && transparent === false)
    {
        return false;
    }

    var width = $("#widthTest").width() - 100;
    if (typeof width !== "undefined" && typeof chart !=="undefined") {
        chartBusinessRisk.width(width);
        chartBusinessRisk.update();
    }
    
    var canvas = document.getElementById("svggraficBusinessRisk");
    var height = $("#svggraficBusinessRisk").height() - 50;
    if (height === null)
    {
        height = 500;
    }

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

function DrawRuleLineBusinessRisk()
{
    var width = $("#ListDataTable").width();
    if (width < 1)
    {
        width = $("#svggraficBusinessRisk").width();
    }

    var height = $("#svggraficBusinessRisk").height() - 48;
    d3.select("#chartBusinessRisk svg").append("line")
        .style("stroke", "red")
        .attr("x1", 65)
        .attr("y1", height * (actualRuleLimitBusinessRisk / -26) + height)
        .attr("x2", width)
        .attr("y2", height * (actualRuleLimitBusinessRisk / -26) + height);

    if (RuleLimitFromDBBusinessRisk !== actualRuleLimitBusinessRisk) {
        d3.select("#chartBusinessRisk svg").append("line")
            .style("stroke", "blue")
            .attr("x1", 65)
            .attr("y1", height * (RuleLimitFromDBBusinessRisk / -26) + height)
            .attr("x2", width)
            .attr("y2", height * (RuleLimitFromDBBusinessRisk / -26) + height)
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
    graphicShowBusinessRisk = false;
}