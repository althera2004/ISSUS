//  the data that powers the bar chart, a simple array of numeric values
var graphicShowOportunity = false;
var actualRuleLimitOportunity = -1;
var chartdataoportunity = [];
for (var x = 0; x < OportunityGraph.length; x++) {
    chartdataoportunity.push(OportunityGraph[x].Result * 20);
}

var chartOportunity;
var myColorsoportunity = ["#DC8475", "#A5CA9F"];
var graphicDataOportunity;
var linedataoportunity;
var myDataOportunity = [];
var chartDataoportunity;

function GetOportunityNameById(id) {
    id = id * 1;
    for (var x = 0; x < OportunityList.length; x++) {
        if (OportunityList[x].OportunityId === id) {
            return OportunityList[x].Description;
        }
    }
    return "";
}

function RenderchartOportunity() {
    nv.addGraph(function () {
        //Create BarChart
        chartOportunity = nv.models.discreteBarChart()
            .x(function (d) { return d.label; })
            .y(function (d) { return d.value; })
            .tooltips(true)
            .showValues(true)
            .showXAxis(false)
            .transitionDuration(0)
            .forceY([0, 25])
            .color(myColorsoportunity)
            .tooltipContent(function (key, x, y, e) {
                if (e.value >= 0) {
                    return "<div style=\"min-width:150px;-webkit-border-radius: 40px;-moz-border-radius: 10px;border-radius: 10px;border:3px solid #DBE6FF;background:rgba(242,246,252,0.8);-webkit-box-shadow: #B3B3B3 2px 2px 2px;-moz-box-shadow: #B3B3B3 2px 2px 2px; box-shadow: #B3B3B3 2px 2px 2px;padding-top:4px;padding-left:4px;\"><strong>" + GetRiskNameById(x) + "</strong><p>" + Dictionary.Item_BusinessRisk_LabelField_Result + ": <strong>" + y + "</strong></p></div>";
                } else {
                    return "";
                }
            });

        chartOportunity.yAxis.tickFormat(d3.format(",.0f"));
        chartOportunity.yAxis.tickValues([5, 10, 15, 20]);
        chartOportunity.valueFormat(d3.format("d"));

        //Add data to BarChart
        chartDataoportunity = d3.select("#chartOportunity svg").datum(graphicDataOportunity);
        chartDataoportunity.call(chartOportunity);

        nv.utils.windowResize(chartOportunity.update);
        return chartOportunity;
    }, function () {
        //console.log("callback");
        d3.selectAll(".discreteBar").on("click",
            function (e) {
                document.location = "/OportunityView.aspx?id=" + e.label;
            });
    });
}

function exampleDataoportunity() {
    function y() {
        myColorsoportunity = [];
        myDataOportunity = [];
        var y = [];

        OportunityGraph.sort(function (a, b) { return parseFloat(b.Result) - parseFloat(a.Result); });

        for (var x = 0; x < OportunityGraph.length; x++) {
            var label = OportunityGraph[x].Id.toString();
            y.push({ "label": label, "value": OportunityGraph[x].Result });

            var finalColor = "#ffb752";
            if (OportunityGraph[x].Assumed === false && OportunityGraph[x].FinalAction !== 1) {
                var limit = typeof actualRuleLimitOportunity === "undefined" ? 0 : actualRuleLimitBusinessRisk;
                if (limit < 0) {
                    limit = OportunityGraph[x].RuleLimit;
                }
                if (OportunityGraph[x].Result < limit) {
                    finalColor = "#DC8475";
                }
                else {
                    finalColor = "#A5CA9F";
                }
            }

            myColorsoportunity.push(finalColor);
            myDataOportunity.push({ "label": label, "value": 5 });
        }

        return y;
    }

    linedataoportunity = [{ "key": "Cumulative Return", "values": myDataOportunity }];
    graphicDataOportunity = [{ "key": "Cumulative Return", "values": y() }];
}

function resizeGraficoOportunity(transparent) {
    if (graphicShowOportunity === true && transparent === false) {
        return false;
    }

    var width = $("#widthTest").width() - 100;
    if (typeof width !== "undefined" && typeof chart !== "undefined") {
        chartOportunity.width(width);
        chartOportunity.update();
    }

    var canvas = document.getElementById("svggraficoportunity");
    var height = $("#svggraficoportunity").height() - 50;
    if (height === null) {
        height = 500;
    }

    d3.select("#chartOportunity svg").append("line")
        .style("stroke", "gray")
        .attr("x1", 65)
        .attr("y1", height)
        .attr("x2", width)
        .attr("y2", height);

    d3.select("#chartOportunity svg").append("line")
        .style("stroke", "gray")
        .attr("x1", 65)
        .attr("y1", 0)
        .attr("x2", 65)
        .attr("y2", height);
    graphicShowOportunity = true;
}

function DrawRuleLineOportunity() {
    var width = $("#ListDataTableOportunity").width();
    if (width < 1) {
        width = $("#svggraficoportunity").width();
    }

    var height = $("#svggraficoportunity").height() - 48;
    d3.select("#chartOportunity svg").append("line")
        .style("stroke", "red")
        .attr("x1", 65)
        .attr("y1", height * (actualRuleLimitOportunity / -26) + height)
        .attr("x2", width)
        .attr("y2", height * (actualRuleLimitOportunity / -26) + height);

    if (RuleLimitFromDBOportunity !== actualRuleLimitOportunity) {
        d3.select("#chartOportunity svg").append("line")
            .style("stroke", "blue")
            .attr("x1", 65)
            .attr("y1", height * (RuleLimitFromDBOportunity / -26) + height)
            .attr("x2", width)
            .attr("y2", height * (RuleLimitFromDBOportunity / -26) + height)
            .style("stroke-dasharray", ("3, 3"));

        document.getElementById("BtnNewIproportunity").disabled = false;
    }
    else {
        document.getElementById("BtnNewIproportunity").disabled = true;
    }

    resizeGraficoOportunity(true);
}

function unresizeGraficoOportunity() {
    graphicShowOportunity = false;
}