var currentMousePos = { "x": -1, "y": -1 };
var originalLimits = [];
var RuleLimitFromDBBusinessRisk = null;
var RuleLimitFromDBoportunity = null;
var rule = { "Id": 0 };
var BusinessRiskSelected;
var OportunitySelected;
var lockOrderList = false;
var lockOrderListOportunity = false;

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));
});

function BusinessRiskDeleteConfirmed() {
    var data = {
        "businessRiskId": BusinessRiskSelected,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#BusinessRiskDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/BusinessRiskActions.asmx/BusinessRiskDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = document.location + '';
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function BusinessRiskDelete(id, name) {
    $("#BusinessRiskName").html(name);
    BusinessRiskSelected = id;
    var dialog = $("#BusinessRiskDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_BusinessRisk_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    BusinessRiskDeleteConfirmed();
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

function OportunityDeleteConfirmed() {
    var data = {
        "oportunityId": OportunitySelected,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };
    $("#OportunityDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/OportunityActions.asmx/Inactivate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = document.location + "";
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function OportunityDelete(id, name) {
    $("#OportunityName").html(name);
    OportunitySelected = id;
    var dialog = $("#OportunityDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Opoprtunity_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        OportunityDeleteConfirmed();
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

function BusinessRiskUpdate(id, name) {
    document.location = "BusinessRiskView.aspx?id=" + id;
    return false;
}

function BussinesRiskListGetAll() {
    var ok = true;
    VoidTable("ItemTableData");
    $("#CmbRules").val(0);
    $("#CmbType").val(0);
    $("#CmbProcess").val(0);
    $("#TxtDateFrom").val("");
    $("#TxtDateTo").val("");
    var from = GetDate($("#TxtDateFrom").val(), "-");
    var to = GetDate($("#TxtDateTo").val(), "-");
    BusinessRiskGetFilter();
}

function OportunityListGetAll() {
    var ok = true;
    VoidTable("ItemTableDataOportunity");
    $("#OportunityCmbRules").val(0);
    $("#OportunityCmbProcess").val(0);
    $("#TxtOportunityDateFrom").val("");
    $("#TxtOportunityDateTo").val("");
    var from = GetDate($("#TxtOportunityDateFrom").val(), "-");
    var to = GetDate($("#TxtOportunityDateTo").val(), "-");
    OportunityGetFilter();
}

function BusinessRiskListGetNone() {
    $("#BtnRecordShowAll").show();
    $("#BtnRecordShowNone").hide();
    VoidTable("ListDataTable");
    $("#TxtDateFrom").val("");
    $("#TxtDateTo").val("");
    $("#CmbRules").val(0);
    $("#CmbProcess").val(0);
    $("#CmbType").val(0);
}

function OportunityGetFilter(exportType) {
    var ok = true;
    VoidTable("ListDataTableOportunity");

    var from = GetDate($("#TxtOportunityDateFrom").val(), "-");
    var to = GetDate($("#TxtOportunityDateTo").val(), "-");

    var rulesId = $("#OportunityCmbRules").val() * 1;
    var processId = $("#OportunityCmbProcess").val() * 1;

    if (from !== null && to !== null) {
        if (from > to) {
            ok = false;
            $("#ErrorDateOportunity").show();
        }
    }

    if (ok === false) {
        $("#ItemTableErrorOportunity").show();
        return false;
    }

    var data = {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "rulesId": rulesId,
        "processId": processId,
        "itemType": 1
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/OportunityActions.asmx/GetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            eval("OportunityList=" + msg.d + ";");
            originalLimits = OportunityList;
            OportunityRenderTable(OportunityList);
            if (exportType !== "undefined") {
                if (exportType === "PDF") {
                    ExportPDF();
                }
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function BusinessRiskGetFilter(exportType) {
    var ok = true;
    VoidTable("ListDataTable");

    var from = GetDate($("#TxtDateFrom").val(), "-");
    var to = GetDate($("#TxtDateTo").val(), "-");

    var rulesId = $("#CmbRules").val() * 1;
    var processId = $("#CmbProcess").val() * 1;
    var type = $("#CmbType").val() * 1;

    if (from !== null && to !== null) {
        if (from > to) {
            ok = false;
            $("#TxtDateFromErrorDateRange").show();
        }
    }

    if (ok === false) {
        $("#ItemTableError").show();
        return false;
    }

    var data = {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "rulesId": rulesId,
        "processId": processId,
        "type": type,
        "itemType": 0
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/BusinessRiskActions.asmx/GetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            eval("BusinessRiskList=" + msg.d + ";");
            originalLimits = BusinessRiskList;
            BusinessRiskRenderTable(BusinessRiskList);
            if (exportType !== "undefined") {
                if (exportType === "PDF") {
                    ExportPDF();
                }
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function OportunityExists(list, code) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Code === code) {
            return x;
        }
    }

    return -1;
}

function OportunityRenderTable(list) {
    if (typeof ApplicationUser.Grants.Oportunity === "undefined") {
        ApplicationUser.Grants.push({ "Oportunity": { "Read": false, "Write": false, "Delete": false } });
    }

    var temp = [];
    for (var x = 0; x < list.length; x++) {
        var exists = OportunityExists(temp, list[x].Code);

        if (exists < 0) {
            temp.push(list[x]);
        }
        else {
            if (temp[exists].OportunityId < list[x].OportunityId) {
                temp[exists] = list[x];
            }
        }
    }

    list = temp;

    console.log("oport", list);

    var items = [];
    VoidTable("ListDataTableOportunity");
    d3.selectAll("svg > *").remove();
    $("#NumberCostsOportunity").html("0");

    if (list.length === 0) {
        $("#ItemTableVoidOportunity").show();
        $("#GraphicTableVoidOportunity").show();
        $("#svggraficoportunity").hide();
        $("#BtnChangeIprOportunity").hide();
        if ($("#OportunityCmbRules").val() * 1 > 0) {
            var actualFilterRule = RuleGetById($("#OportunityCmbRules").val() * 1);
            $("#RuleDescriptionOportunity").html("<strong>" + actualFilterRule.Description + "</strong>");
        }
        else {
            $("#RuleDescriptionOportunity").html(Dictionary.Common_All_Female_Plural);
        }
        return false;
    }
    else {
        $("#ItemTableVoidOportunity").hide();
    }

    // Establecer el valor de la norma actual si la hay
    RuleLimitFromDBOportunity = -1;
    actualRuleLimitOportunity = -1;
    if ($("#OportunityCmbRules").val() > 0) {
        actualRuleLimitOportunity = list[0].RuleLimit;
        RuleLimitFromDBOportunity = actualRuleLimitOportunity;
        $("#input-span-slideroportunity").slider({ "value": RuleLimitFromDBOportunity });
        RenderStepsOportunity();
    }

    var total = 0;

    // Se vacía el JSON del gráfico
    OportunityGraph = [];

    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        var realResult = item.Result
        RenderOportunityRow(item);

        // Se añade el elemento al JSON del gráfico
        // si el reisgo está evaluado
        if (realResult > 0) {
            OportunityGraph.push({
                "Id": item.OportunityId,
                "Description": item.Description,
                "Code": item.Code,
                "Rules": item.Rules,
                "Result": realResult,
                "Assumed": item.Assumed, //realAction === 1,
                "RuleLimit": item.RuleLimit,
                "FinalAction": item.FinalAction
            });
        }

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }

        /*if ($.inArray(objRules.Description, items) === -1) {
            items.push(objRules.Description);
        }

        if ($.inArray(objProcess.Description, items) === -1) {
            items.push(objProcess.Description);
        }*/
    }

    $("#GraphicTableVoidOportunity").hide("none");
    $("#svggraficoportunity").show();
    exampleDataoportunity();
    RenderchartOportunity();
    $(".discreteBar").on("click", function (e) { console.log(e) });

    if (document.getElementById("CmbRules").value * 1 > 0) {
        DrawRuleLineOportunity();
        document.getElementById("BtnChangeIprOportunity").style.display = "";
        rule = RuleGetById($("#OportunityCmbRules").val() * 1);
        if (rule !== null) {
            actualRuleLimitOportunity = rule.Limit;
            $("#RuleDescriptionOportunity").html("<strong>" + rule.Description + "</strong> " + Dictionary.Item_Rules_FieldLabel_Limit + ": <strong>" + RuleLimitFromDBOportunity + "</strong>");
        }
    }
    else {
        rule = { "Id": 0 };
        resizeGraficoOportunity($("#OportunityCmbRules").val() * 1);
        document.getElementById("BtnChangeIpr").style.display = "none";
        $("#RuleDescriptionOportunity").html(Dictionary.Common_All_Female_Plural);
        actualRuleLimitOportunity = -1;
    }

    if (items.length === 0) {
        $("#nav-search").hide();
    }
    else {
        $("#nav-search").show();

        items.sort(function (a, b) {
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        });
        var autocomplete = $(".nav-search-input").typeahead();
        autocomplete.data('typeahead').source = items;

        $("#nav-search-input").keyup(FilterList);
        $("#nav-search-input").change(FilterList);
    }

    $("#NumberCostsOportunity").html(list.length);
    if (lockOrderListOportunity === false) {
        $("#ListDataHeaderOportunity #th1").click();
        if ($("#ListDataHeaderOportunity #th1").hasClass("DESC")) {
            $("#ListDataHeaderOportunity #th1").click();
        }
    }
    else {
        var column = listOrder.split('|')[0];
        var order = listOrder.split('|')[1];

        $("#" + column).click();
        if (document.getElementById(column).className.indexOf(order) === -1) {
            $("#" + column).click();
        }
    }
}

function BusinessRiskRenderTable(list) {
    if (typeof ApplicationUser.Grants.BusinessRisk === "undefined")
    {
        ApplicationUser.Grants.push({ "BusinessGrant": { "Read": false, "Write": false, "Delete": false } });
    }

    var items = [];
    VoidTable("ListDataTable");
    d3.selectAll("svg > *").remove();
    $("#NumberCosts").html("0");

    if (list.length === 0) {
        $("#ItemTableVoid").show();
        $("#GraphicTableVoid").show();
        $("#svggraficBusinessRisk").hide();
        $("#BtnChangeIpr").hide();
        if ($("#CmbRules").val() * 1 > 0) {
            var actualFilterRule = RuleGetById($("#CmbRules").val() * 1);
            $("#RuleDescriptionBusinessRisk").html("<strong>" + actualFilterRule.Description + "</strong>");
        }
        else {
            $("#RuleDescriptionBusinessRisk").html(Dictionary.Common_All_Female_Plural);
        }
        return false;
    }
    else {
        $("#ItemTableVoid").hide();
    }

    // Establecer el valor de la norma actual si la hay
    RuleLimitFromDBBusinessRisk = -1;
    actualRuleLimitBusinessRisk = -1;
    if (document.getElementById("CmbRules").value > 0) {
        actualRuleLimitBusinessRisk = list[0].RuleLimit;
        RuleLimitFromDBBusinessRisk = actualRuleLimitBusinessRisk;
        $("#input-span-slider").slider({ "value": RuleLimitFromDBBusinessRisk });
        RenderStepsBusinessRisk();
    }

    var total = 0;
    BusinessRiskGraph = [];

    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        var realResult = item.StartResult;
        RenderBusinessRiskRow(item);

        // Se añade el elemento al JSON del gráfico
        // si el reisgo está evaluado
        if (realResult > 0) {
            BusinessRiskGraph.push({
                "Id": item.BusinessRiskId,
                "Description": item.Description,
                "Code": item.Code,
                "Rules": item.Rules,
                "Result": realResult,
                "Assumed": item.Assumed, //realAction === 1,
                "RuleLimit": item.RuleLimit,
                "FinalAction": item.FinalAction
            });
        }

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }

        /*if ($.inArray(objRules.Description, items) === -1) {
            items.push(objRules.Description);
        } 

        if ($.inArray(objProcess.Description, items) === -1) {
            items.push(objProcess.Description);
        }*/
    }

    $("#GraphicTableVoid").hide("none");
    $("#svggraficBusinessRisk").show();
    exampleDataBusinessRisk();
    RenderChartBusinessRisk();
    $(".discreteBar").on("click", function (e) { console.log(e) });

    if (document.getElementById("CmbRules").value * 1 > 0) {
        DrawRuleLineBusinessRisk();
        $("#BtnChangeIpr").show();
        rule = RuleGetById($("#CmbRules").val() * 1);
        if (rule !== null) {
            actualRuleLimitBusinessRisk = rule.Limit;
            $("#RuleDescriptionBusinessRisk").html("<strong>" + rule.Description + "</strong> " + Dictionary.Item_Rules_FieldLabel_Limit + ": <strong>" + RuleLimitFromDBBusinessRisk + "</strong>");
        }
    }
    else {
        rule = { "Id": 0 };
        resizegrafico($("#CmbRules").val() * 1);
        document.getElementById("BtnChangeIpr").style.display = "none";
        $("#RuleDescriptionBusinessRisk").html(Dictionary.Common_All_Female_Plural);
        actualRuleLimitBusinessRisk = -1;
    }

    if (items.length === 0) {
        $("#nav-search").hide();
    }
    else {
        $("#nav-search").show();

        items.sort(function (a, b) {
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        });
        var autocomplete = $(".nav-search-input").typeahead();
        autocomplete.data("typeahead").source = items;

        $("#nav-search-input").keyup(FilterList);
        $("#nav-search-input").change(FilterList);
    }

    $("#NumberCosts").html(list.length);
    if (lockOrderList === false) {
        $("#th1").click();
        if (document.getElementById("th1").className.indexOf("DESC") !== -1) {
            $("#th1").click();
        }
    }
    else {
        var column = listOrder.split('|')[0];
        var order = listOrder.split('|')[1];

        $("#" + column).click();
        if (document.getElementById(column).className.indexOf(order) === -1) {
            $("#" + column).click();
        }
    }
}

function RenderOportunityRow(item) {
    var row = document.createElement("TR");
    var tdStatus = document.createElement("TD");
    var tdOpenDate = document.createElement("TD");
    var tdName = document.createElement("TD");
    var tdProcess = document.createElement("TD");
    var tdRules = document.createElement("TD");
    var tdInitialResult = document.createElement("TD");
    var tdFinalResult = document.createElement("TD");

    var objProcess = eval("(" + item.Process + ")");
    var objRules = eval("(" + item.Rules + ")");

    if (objProcess.Id > 0) {
        if (typeof user.Grants.Process === "undefined" || user.Grants.Proccess.Read === false) {
            tdProcess.appendChild(document.createTextNode(objProcess.Description));
        }
        else {
            var link = document.createElement("A");
            link.href = "ProcesosView.aspx?id=" + objProcess.Id;
            link.appendChild(document.createTextNode(objProcess.Description));
            tdProcess.appendChild(link);
        }
    }
    if (objRules.Id > 0) {

        if (typeof user.Grants.Rules === "undefined" || user.Grants.Rules === null || user.Grants.Rules.Read === false || user.Grants.Rules.Write === false) {
            tdRules.appendChild(document.createTextNode(objRules.Description));
        }
        else {
            var linkRule = document.createElement("A");
            linkRule.href = "RulesView.aspx?id=" + objRules.Id;
            linkRule.appendChild(document.createTextNode(objRules.Description));
            tdRules.appendChild(linkRule);
        }
    }

    row.id = item.OportunityId;

    var oportunityLink = document.createElement("A");
    oportunityLink.href = "OportunityView.aspx?id=" + item.OportunityId;

    oportunityLink.appendChild(document.createTextNode(item.Description));

    icon = document.createElement("I");
    tdStatus.appendChild(icon);

    var realResult = item.StartResult

    if (item.Result === 0) {
        icon.style.color = "#777777";
        icon.title = Dictionary.Item_Oportunity_Status_Unevaluated;
        icon.className = "icon-warning-sign bigger-110";
    } else {
        if (item.Result < item.RuleLimit) {

            icon.style.color = "#A5CA9F";
            icon.title = Dictionary.Item_Oportunity_Status_NotSignificant;
            icon.className = "icon-circle bigger-110";
        }
        else {
            icon.style.color = "#DC8475";
            icon.title = Dictionary.Item_Oportunity_Status_Significant;
            icon.className = "icon-circle bigger-110";
        }
    }

    tdOpenDate.appendChild(document.createTextNode(FormatYYYYMMDD(item.OpenDate, "/")));
    tdInitialResult.appendChild(document.createTextNode(item.Result === 0 ? "" : item.Result));
    tdFinalResult.appendChild(document.createTextNode(item.RuleLimit));
    tdName.appendChild(oportunityLink);

    tdOpenDate.style.width = "90px";
    tdInitialResult.align = "center";
    tdFinalResult.align = "center";

    tdStatus.style.width = "60px";
    tdStatus.align = "center";

    tdRules.style.width = "200px";
    tdProcess.style.width = "200px";
    tdInitialResult.style.width = "90px";
    tdFinalResult.style.width = "80px";

    row.appendChild(tdStatus);
    row.appendChild(tdOpenDate);
    row.appendChild(tdName);
    row.appendChild(tdProcess);
    row.appendChild(tdRules);
    row.appendChild(tdInitialResult);
    row.appendChild(tdFinalResult);

    var iconEdit = document.createElement("SPAN");
    iconEdit.className = "btn btn-xs btn-info";
    iconEdit.id = item.Number;
    var innerEdit = document.createElement("I");
    innerEdit.className = ApplicationUser.Grants.BusinessRisk.Write ? "icon-edit bigger-120" : "icon-eye-open bigger-120";
    iconEdit.appendChild(innerEdit);
    iconEdit.onclick = function () { document.location = "OportunityView.aspx?id=" + this.parentNode.parentNode.id; };

    if (ApplicationUser.Grants.Oportunity.Delete === true) {
        var iconDelete = document.createElement("SPAN");
        iconDelete.className = "btn btn-xs btn-danger";
        iconDelete.id = item.Number;
        var innerDelete = document.createElement("I");
        innerDelete.className = "icon-trash bigger-120";
        iconDelete.appendChild(innerDelete);
        iconDelete.onclick = function () { OportunityDelete(this.parentNode.parentNode.id); };
    }

    var tdActions = document.createElement("TD");

    tdActions.appendChild(iconEdit);

    if (ApplicationUser.Grants.Oportunity.Delete === true) {
        tdActions.appendChild(document.createTextNode(" "));
        tdActions.appendChild(iconDelete);
    }

    tdActions.style.width = "90px";
    row.appendChild(tdActions);

    document.getElementById("ListDataTableOportunity").appendChild(row);
}

function RenderBusinessRiskRow(item) {
    var row = document.createElement("TR");
    var tdStatus = document.createElement("TD");
    var tdOpenDate = document.createElement("TD");
    var tdName = document.createElement("TD");
    var tdProcess = document.createElement("TD");
    var tdRules = document.createElement("TD");
    var tdInitialResult = document.createElement("TD");
    var tdFinalResult = document.createElement("TD");

    var objProcess = eval("(" + item.Process + ")");
    var objRules = eval("(" + item.Rules + ")");

    if (objProcess.Id > 0) {
        if (typeof user.Grants.Process === "undefined" || user.Grants.Proccess.Read === false) {
            tdProcess.appendChild(document.createTextNode(objProcess.Description));
        }
        else {
            var link = document.createElement("A");
            link.href = "ProcesosView.aspx?id=" + objProcess.Id;
            link.appendChild(document.createTextNode(objProcess.Description));
            tdProcess.appendChild(link);
        }
    }
    if (objRules.Id > 0) {

        if (typeof user.Grants.Rules === "undefined" || user.Grants.Rules === null || user.Grants.Rules.Read === false || user.Grants.Rules.Write === false) {
            tdRules.appendChild(document.createTextNode(objRules.Description));
        }
        else {
            var linkRule = document.createElement("A");
            linkRule.href = "RulesView.aspx?id=" + objRules.Id;
            linkRule.appendChild(document.createTextNode(objRules.Description));
            tdRules.appendChild(linkRule);
        }
    }

    row.id = item.BusinessRiskId;

    var businessRiskLink = document.createElement("A");
    businessRiskLink.href = "BusinessRiskView.aspx?id=" + item.BusinessRiskId;
    businessRiskLink.appendChild(document.createTextNode(item.Description));

    icon = document.createElement("I");
    tdStatus.appendChild(icon);

    var realResult = item.StartResult;
    /*if (realResult === 0) {
        realResult = item.StartResult;
    }*/

    var realAction = item.FinalAction;
    if (realAction === 0) {
        realAction = item.RealAction;
    }

    if (item.Assumed === true) {
        icon.style.color = "#FFC97D";
        icon.title = Dictionary.Item_BusinessRisk_Status_Assumed;
        icon.className = "icon-circle bigger-110";
    }
    else if (realResult === 0) {
        icon.style.color = "#777777";
        icon.title = Dictionary.Item_BusinessRisk_Status_Unevaluated;
        icon.className = "icon-warning-sign bigger-110";
    }
    else if (realAction === 1) {
        icon.style.color = "#FFC97D";
        icon.title = Dictionary.Item_BusinessRisk_Status_Assumed;
        icon.className = "icon-circle bigger-110";
    }
    else {
        if (realResult < item.RuleLimit) {
            icon.style.color = "#A5CA9F";
            icon.title = Dictionary.Item_BusinessRisk_Status_NotSignificant;
            icon.className = "icon-circle bigger-110";
        }
        else {
            icon.style.color = "#DC8475";
            icon.title = Dictionary.Item_BusinessRisk_Status_Significant;
            icon.className = "icon-circle bigger-110";
        }
    }

    tdOpenDate.appendChild(document.createTextNode(FormatYYYYMMDD(item.OpenDate, "/")));
    tdInitialResult.appendChild(document.createTextNode(realResult === 0 ? "" : realResult));
    tdFinalResult.appendChild(document.createTextNode(item.RuleLimit === 0 ? "" : item.RuleLimit));
    tdName.appendChild(businessRiskLink);

    tdOpenDate.style.width = "90px";
    tdInitialResult.align = "center";
    tdFinalResult.align = "center";

    tdStatus.style.width = "60px";
    tdStatus.align = "center";

    tdRules.style.width = "200px";
    tdProcess.style.width = "200px";
    tdInitialResult.style.width = "90px";
    tdFinalResult.style.width = "80px";

    row.appendChild(tdStatus);
    row.appendChild(tdOpenDate);
    row.appendChild(tdName);
    row.appendChild(tdProcess);
    row.appendChild(tdRules);
    row.appendChild(tdInitialResult);
    row.appendChild(tdFinalResult);

    var iconEdit = document.createElement("SPAN");
    iconEdit.className = "btn btn-xs btn-info";
    iconEdit.id = item.Number;
    var innerEdit = document.createElement("I");
    innerEdit.className = ApplicationUser.Grants.BusinessRisk.Write ? "icon-edit bigger-120" : "icon-eye-open bigger-120";
    iconEdit.appendChild(innerEdit);
    iconEdit.onclick = function () { document.location = "BusinessRiskView.aspx?id=" + this.parentNode.parentNode.id; };

    if (ApplicationUser.Grants.BusinessRisk.Delete === true) {
        var iconDelete = document.createElement("SPAN");
        iconDelete.className = "btn btn-xs btn-danger";
        iconDelete.id = item.Number;
        var innerDelete = document.createElement("I");
        innerDelete.className = "icon-trash bigger-120";
        iconDelete.appendChild(innerDelete);
        iconDelete.onclick = function () { BusinessRiskDelete(this.parentNode.parentNode.id); };
    }

    var tdActions = document.createElement("TD");

    tdActions.appendChild(iconEdit);
    if (ApplicationUser.Grants.BusinessRisk.Delete === true) {
        tdActions.appendChild(document.createTextNode(" "));
        tdActions.appendChild(iconDelete);
    }

    tdActions.style.width = "90px";
    row.appendChild(tdActions);

    document.getElementById("ListDataTable").appendChild(row);
}

function RuleGetById(id) {
    for (var x = 0; x < CompanyRules.length; x++) {
        if (CompanyRules[x].Id === id) {
            return CompanyRules[x];
        }
    }

    return null;
}

function SetRule(id)
{
    if(rule.Id < 1) { return false; }
    actualRuleLimitBusinessRisk = id;
    d3.selectAll("svg > *").remove();
    exampleDataBusinessRisk();
    RenderChartBusinessRisk();
    DrawRuleLineBusinessRisk();
    $("#input-span-slider").slider({ "value": id });
    $(".discreteBar").on("click", function (e) { console.log(e); });
}

function SetRuleOportunity(id) {
    if (rule.Id < 1) { return false; }
    actualRuleLimitOportunity = id;
    d3.selectAll("svg > *").remove();
    exampleDataOportunity();
    RenderchartOportunity();
    DrawRuleLineOportunity();
    $("#input-span-slideroportunity").slider({ "value": id });
    $(".discreteBar").on("click", function (e) { console.log(e); });
}

function NewIpr()
{
    var candidate = new Array();
    for (var x = 0; x < BusinessRiskGraph.length; x++) {
        if (BusinessRiskGraph[x].Assumed === false) {
            if (BusinessRiskGraph[x].Result <= RuleLimitFromDBBusinessRisk) {
                if (BusinessRiskGraph[x].Result > actualRuleLimitBusinessRisk) {
                    candidate.push(BusinessRiskGraph[x]);
                }
            }
        }
    }

    if (candidate.length > 0) {
        message = Dictionary.Item_BusinessRisk_Message_IPR.replace("#", candidate.length);
        promptInfoUI(message, 450, NewIprConfirmed, null);
        return;
    }

    candidate = new Array();
    for (var y = 0; y < BusinessRiskGraph.length; y++) {
        if (BusinessRiskGraph[y].Assumed === false) {
            if (BusinessRiskGraph[y].Result >= RuleLimitFromDBBusinessRisk) {
                if (BusinessRiskGraph[y].Result < actualRuleLimitBusinessRisk) {
                    candidate.push(BusinessRiskGraph[y]);
                }
            }
        }
    }

    if (candidate.length > 0) {
        message = Dictionary.Item_BusinessRisk_Message_IPR_2.replace("#", candidate.length);
        promptInfoUI(message, 450, NewIprConfirmed, null);
        return;
    }

    NewIprConfirmed();
    return;
}

function NewIprConfirmed()
{
    var data = {
        "rules":
        {
            "Id": rule.Id,
            "Limit": actualRuleLimitBusinessRisk,
            "CompanyId": companyId
        },
        "companyId": companyId,
        "userId": user.Id
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/RulesActions.asmx/SetLimit",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            BusinessRiskGetFilter();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function RenderStepsBusinessRisk() {
    VoidTable("steps");
    for (var x = 1; x < 26; x++) {
        var span = document.createElement("span");
        span.id = x;
        span.className = "tick";
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement("BR"));
        span.appendChild(document.createTextNode("|"));
        span.style.left = ((100 / 24) * (x - 1)) + "%";
        document.getElementById("steps").appendChild(span);
        span.onclick = function () { SetRule(this.id) };
        if (x === RuleLimitFromDBBusinessRisk) {
            span.style.color = "#00f";
            span.style.fontWeight = "bold";
        }
    }
}

function RenderStepsOportunity() {
    VoidTable("stepsoportunity");
    for (var x = 1; x < 26; x++) {
        var span = document.createElement("span");
        span.id = x;
        span.className = "tick";
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement("BR"));
        span.appendChild(document.createTextNode("|"));
        span.style.left = ((100 / 24) * (x - 1)) + "%";
        document.getElementById("stepsoportunity").appendChild(span);
        span.onclick = function () { SetRule(this.id) };
        if (x === RuleLimitFromDBOportunity) {
            span.style.color = "#00f";
            span.style.fontWeight = "bold";
        }
    }
}

function Resize() {
    var containerHeight = $(window).height();
    var finalHeight = containerHeight - 530;
    if (finalHeight < 350) {
        finalHeight = 350;
    }

    $("#ListDataDiv").height((finalHeight) + "px");
    $("#ListDataDivOportunity").height((finalHeight) + "px");
}

window.onload = function () {
    $("#BtnNewItem").after("<button class=\"btn btn-success\" type=\"button\" id=\"BtnNewOportunity\" onclick=\"document.location = 'OportunityView.aspx?id=-1';\"><i class=\"icon-plus bigger-110\"></i>" + Dictionary.Item_Oportunity_Button_New + "</button>");
    $("H1").html("<input type=\"radio\" id=\"RR\" name=\"RType\" checked=\"checked\" style=\"margin-top:12px;\" />&nbsp;" + Dictionary.Item_BusinessRisks + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"radio\" id=\"RO\" name=\"RType\" style=\"margin-top:12px;\" />&nbsp;" + Dictionary.Item_Oportunities);

    BusinessRiskGetFilter();
    OportunityGetFilter();
    SetLayout(layout);

    $("#RR").on("click", function () { SetLayout(1); BusinessRiskGetFilter(); });
    $("#RO").on("click", function () { SetLayout(2); OportunityGetFilter(); });

    $("#BtnRecordShowAll").click();
    $("#BtnRecordShowAllOportunity").click();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");

    if (Filter === null && FilterOportunity !== null) {
        document.getElementById("RO").checked = true;
    }

    if (Filter !== null) {
        if (Filter.rulesId !== null) {
            $("#CmbRules").val(Filter.rulesId);
        }

        if (Filter.processId !== null) {
            $("#CmbProcess").val(Filter.processId);
        }

        if (Filter.type !== null) {
            $("#CmbType").val(Filter.type);
        }

        if (Filter.from !== null) {
            $("#TxtDateFrom").val(GetDateYYYYMMDDText(Filter.from, "/", false));
        }

        if (Filter.to !== null) {
            $("#TxtDateTo").val(GetDateYYYYMMDDText(Filter.to, "/", false));
        }

        if (Filter.itemType !== null) {
            if (Filter.itemType === 0) {
                document.getElementById("RR").checked = true;
            }

            if (Filter.itemType === 1) {
                document.getElementById("RO").checked = true;
            }
        }
    }

    $("#CmbRules").on("change", BusinessRiskGetFilter);
    $("#CmbProcess").on("change", BusinessRiskGetFilter);
    $("#TxtDateFrom").on("change", BusinessRiskGetFilter);
    $("#TxtDateTo").on("change", BusinessRiskGetFilter);
    $("#CmbType").on("change", BusinessRiskGetFilter);

    $("#OportunityCmbRules").on("change", OportunityGetFilter);
    $("#OportunityCmbProcess").on("change", OportunityGetFilter);
    $("#TxtOportunityDateFrom").on("change", OportunityGetFilter);
    $("#TxtOportunityDateTo").on("change", OportunityGetFilter);

    Resize();

    var options = $.extend({}, $.datepicker.regional[ApplicationUser.Language], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);
    $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

    $("#BtnNewBusinessRisk").on("click", function (e) {
        document.location = "BusinessRiskView.aspx?id=-1";
        return false;
    });

    $("#BtnNewOportunity").on("click", function (e) {
        document.location = "OportunityView.aspx?id=-1";
        return false;
    });

    $("#BtnSearchOportunity").on("click", function (e) {
        e.preventDefault();
        OportunityGetFilter();
    });

    $("#BtnSearch").on("click", function (e) {
        e.preventDefault();
        BusinessRiskGetFilter();
    });

    $("#BtnRecordShowAll").on("click", function (e) {
        e.preventDefault();
        BussinesRiskListGetAll();
    });

    $("#BtnRecordShowAllOportunity").on("click", function (e) {
        e.preventDefault();
        OportunityListGetAll();
    });

    $("#BtnRecordShowNone").on("click", function (e) {
        e.preventDefault();
        BusinessRiskListGetNone();
    });

    $("#tabgraficos").on("click", BusinessRiskGetFilter);
    $("#tabbasic").on("click", BusinessRiskGetFilter);
    $("#tabgraficosoportunity").on("click", OportunityGetFilter);
    $("#taboportunity").on("click", OportunityGetFilter);

    $("#input-span-slider").slider({
        "value": RuleLimitFromDBBusinessRisk,
        "range": "min",
        "min": 1,
        "max": 25,
        "step": 1,
        "slide": function (event, ui) {
            var val = parseInt(ui.value);
            SetRule(val);
        }
    });

    $("#input-span-sliderOportunity").slider({
        "value": RuleLimitFromDBOportunity,
        "range": "min",
        "min": 1,
        "max": 25,
        "step": 1,
        "slide": function (event, ui) {
            var val = parseInt(ui.value);
            SetRuleOportunity(val);
        }
    });

    if (layout === 2) {
        $("#RO").click();
    }
};

window.onresize = function () { Resize(); };

function Export() {
    lockOrderList = true;
    BusinessRiskGetFilter("PDF");
}

function ExportPDF() {
    var from = $("#TxtDateFrom").val();
    var to = $("#TxtDateTo").val();

    var rulesId = $("#CmbRules").val() * 1;
    var processId = $("#CmbProcess").val() * 1;
    var typeId = $("#CmbType").val() * 1;
    var data = null;

    if (document.getElementById("RR").checked === true) {
        data =
            {
                "companyId": Company.Id,
                "from": from,
                "to": to,
                "rulesId": rulesId,
                "processId": processId,
                "typeId": typeId,
                "listOrder": listOrder
            };

        LoadingShow(Dictionary.Common_Report_Rendering);
        $.ajax({
            "type": "POST",
            "url": "/Export/BusinessriskExportList.aspx/PDF",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                LoadingHide();
                var link = document.createElement("a");
                link.id = "download";
                link.href = msg.d.MessageError;
                link.download = msg.d.MessageError;
                link.target = "_blank";
                document.body.appendChild(link);
                document.body.removeChild(link);
                $("#download").trigger("click");
                window.open(msg.d.MessageError);
                $("#dialogAddAddress").dialog("close");
            },
            "error": function (msg) {
                LoadingHide();
                alertUI("error:" + msg.responseText);
            }
        });
    }

    if (document.getElementById("RO").checked === true) {
        data =
            {
                "companyId": Company.Id,
                "from": from,
                "to": to,
                "rulesId": rulesId,
                "processId": processId,
                "listOrder": listOrder
            };

        LoadingShow(Dictionary.Common_Report_Rendering);
        $.ajax({
            "type": "POST",
            "url": "/Export/OportunityExportList.aspx/PDF",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                LoadingHide();
                var link = document.createElement("a");
                link.id = "download";
                link.href = msg.d.MessageError;
                link.download = msg.d.MessageError;
                link.target = "_blank";
                document.body.appendChild(link);
                document.body.removeChild(link);
                $("#download").trigger("click");
                window.open(msg.d.MessageError);
                $("#dialogAddAddress").dialog("close");
            },
            "error": function (msg) {
                LoadingHide();
                alertUI("error:" + msg.responseText);
            }
        });
    }
}

function SetLayout(type) {
    if (type === 1) {
        $("#widthTest").show();
        $("#widthTestOportunity").hide();
        $("#tabbasic").show();
        $("#tabgraficos").show();
        $("#taboportunity").hide();
        $("#tabgraficosoportunity").hide();
        $("#tabbasic").click();
        $("#BtnNewItem").show();
        $("#BtnNewOportunity").hide();
    }

    if (type === 2) {
        $("#widthTest").hide();
        $("#widthTestOportunity").show();
        $("#tabbasic").hide();
        $("#tabgraficos").hide();
        $("#taboportunity").show();
        $("#tabgraficosoportunity").show();
        $("#taboportunity").click();
        $("#BtnNewItem").hide();
        $("#BtnNewOportunity").show();
    }

    var data = {
        "type": type
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/OportunityActions.asmx/SetLayout",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

$(document).mousemove(function (event) {
    currentMousePos.x = event.pageX;
    currentMousePos.y = event.pageY;
    var position = $("#svggraficBusinessRisk").offset();
    $(".xy-tooltip").css({ top: currentMousePos.y - position.top - 30, left: currentMousePos.x - position.left + 10 });
});