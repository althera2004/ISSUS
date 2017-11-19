var RuleLimitFromDB;
var rule = { "Id": 0 };
var BusinessRiskSelected;

function BusinessRiskDeleteAction() {
    var webMethod = "/Async/BusinessRiskActions.asmx/BusinessRiskDelete";
    var data = { businessRiskId: BusinessRiskSelected, companyId: Company.Id, userId: user.Id };
    $("#BusinessRiskDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            document.location = document.location + '';
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function BusinessRiskDelete(id, name) {
    $('#BusinessRiskName').html(name);
    BusinessRiskSelected = id;
    var dialog = $("#BusinessRiskDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_BusinessRisk_Popup_Delete_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    BusinessRiskDeleteAction();
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function BusinessRiskUpdate(id, name) {
    document.location = 'BusinessRiskView.aspx?id=' + id;
    return false;
}

function BussinesRiskListGetAll() {
    document.getElementById('BtnRecordShowAll').style.display = 'none';
    document.getElementById('BtnRecordShowNone').style.display = '';
    var ok = true;
    VoidTable('ItemTableData');
    $('#CmbRules').val(0);
    $('#CmbType').val(0);
    $('#CmbProcess').val(0);
    $('#TxtDateFrom').val('');
    $('#TxtDateTo').val('');
    var from = GetDate($('#TxtDateFrom').val(), '-');
    var to = GetDate($('#TxtDateTo').val(), '-');
    BusinessRiskGetFilter();
}

function BusinessRiskListGetNone() {
    document.getElementById('BtnRecordShowAll').style.display = '';
    document.getElementById('BtnRecordShowNone').style.display = 'none';
    VoidTable('ListDataTable');
    $('#TxtDateFrom').val("");
    $('#TxtDateTo').val("");
    $('#CmbRules').val(0);
    $('#CmbProcess').val(0);
    $('#CmbType').val(0);
}

function BusinessRiskGetFilter(exportType) {
    var ok = true;
    VoidTable('ListDataTable');

    var from = GetDate($('#TxtDateFrom').val(), '-');
    var to = GetDate($('#TxtDateTo').val(), '-');

    var rulesId = $('#CmbRules').val() * 1;
    var processId = $('#CmbProcess').val() * 1;
    var type = $('#CmbType').val() * 1;

    if (from !== null && to !== null) {
        if (from > to) {
            ok = false;
            document.getElementById('ErrorDate').style.display = '';
        }
    }

    if (ok === false) {
        document.getElementById('ItemTableError').style.display = '';
        return false;
    }

    var data = {
        companyId: Company.Id,
        from: from,
        to: to,
        rulesId: rulesId,
        processId: processId,
        type: type
    };

    $.ajax({
        type: "POST",
        url: "/Async/BusinessRiskActions.asmx/GetFilter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            eval("BusinessRiskList=" + msg.d + ";");
            originalLimits = BusinessRiskList;
            BusinessRiskRenderTable(BusinessRiskList);
            if (exportType !== "undefined") {
                if (exportType === "PDF") {
                    ExportPDF();
                }
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

var originalLimits = new Array();

function GetRuleById(id)
{
    for (var x = 0; x < CompanyRules.length; x++)
    {
        if(CompanyRules[x].Id === id)
        {
            return CompanyRules[x];
        }
    }

    return null;
}

function BusinessRiskRenderTable(list) {
    if (typeof ApplicationUser.Grants.BusinessRisk === "undefined")
    {
        ApplicationUser.Grants.push({ "BusinessGrant": { "Read": false, "Write": false, "Delete": false } });
    }

    var items = new Array();
    var target = document.getElementById('ListDataTable');
    VoidTable('ListDataTable');
    // Eliminar las lineas de Normas
    d3.selectAll("svg > *").remove();
    $("#NumberCosts").html("0");

    if (list.length === 0) {
        document.getElementById("ItemTableVoid").style.display = "";
        document.getElementById("GraphicTableVoid").style.display = "";
        document.getElementById("svggrafic").style.display = "none";
        document.getElementById("BtnChangeIpr").style.display = "none";
        if ($("#CmbRules").val() * 1 > 0) {
            var actualFilterRule = RuleGetById($("#CmbRules").val() * 1);
            $("#RuleDescription").html("<strong>" + actualFilterRule.Description + "</strong>");
        }
        else {
            $("#RuleDescription").html(Dictionary.Common_All_Female_Plural);
        }
        return false;
    }
    else {
        document.getElementById("ItemTableVoid").style.display = "none";
    }

    // Establecer el valor de la norma actual si la hay
    RuleLimitFromDB = -1;
    actualRuleLimit = -1;
    if (document.getElementById('CmbRules').value > 0) {
        actualRuleLimit = list[0].RuleLimit;
        RuleLimitFromDB = actualRuleLimit;
        $("#input-span-slider").slider({ value: RuleLimitFromDB });
        RenderSteps();
    }

    var total = 0;

    // Se vacía el JSON del gráfico
    BusinessRiskGraph = new Array();

    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        var row = document.createElement('TR');
        var tdStatus = document.createElement('TD');
        var tdOpenDate = document.createElement('TD');
        var tdName = document.createElement('TD');
        var tdProcess = document.createElement('TD');
        var tdRules = document.createElement('TD');
        var tdInitialResult = document.createElement('TD');
        var tdFinalResult = document.createElement('TD');

        var objProcess = eval('(' + item.Process + ')');
        var objRules = eval('(' + item.Rules + ')');

        if (objProcess.Id > 0) {
            if (user.Grants.Proccess.Read === false) {
                tdProcess.appendChild(document.createTextNode(objProcess.Description));
            }
            else {
                var link = document.createElement('A');
                link.href = 'ProcesosView.aspx?id=' + objProcess.Id;
                link.appendChild(document.createTextNode(objProcess.Description));
                tdProcess.appendChild(link);
            }
        }
        if (objRules.Id > 0) {

            if (user.Grants.Rules === null || user.Grants.Rules.Read === false || user.Grants.Rules.Write == false) {
                tdRules.appendChild(document.createTextNode(objRules.Description));
            }
            else {
                var link = document.createElement('A');
                link.href = 'RulesView.aspx?id=' + objRules.Id;
                link.appendChild(document.createTextNode(objRules.Description));
                tdRules.appendChild(link);
            }
        }

        row.id = item.BusinessRiskId;

        var businessRiskLink = document.createElement('A');
        businessRiskLink.href = 'BusinessRiskView.aspx?id=' + item.BusinessRiskId;
        businessRiskLink.appendChild(document.createTextNode(item.Description));

        icon = document.createElement('I');
        tdStatus.appendChild(icon);

        var realResult = item.FinalResult;
        if (realResult === 0) {
            realResult = item.StartResult;
        }

        var realAction = item.FinalAction;
        if (realAction === 0) {
            realAction = item.RealAction;
        }

        if (item.Assumed === true) {
            icon.style.color = '#FFC97D';
            icon.title = Dictionary.Item_BusinessRisk_Status_Assumed;
            icon.className = 'icon-circle bigger-110';
        }
        else if (realResult == 0) {
            icon.style.color = '#777777';
            icon.title = Dictionary.Item_BusinessRisk_Status_Unevaluated;
            icon.className = 'icon-warning-sign bigger-110';
        }
        else if (realAction === 1)
        {
            icon.style.color = '#FFC97D';
            icon.title = Dictionary.Item_BusinessRisk_Status_Assumed;
            icon.className = 'icon-circle bigger-110';
        }
        else
        {
            if(realResult < item.RuleLimit)
            {
                icon.style.color = '#A5CA9F';
                icon.title = Dictionary.Item_BusinessRisk_Status_NotSignificant;
                icon.className = 'icon-circle bigger-110';
            }
            else {
                icon.style.color = '#DC8475';
                icon.title = Dictionary.Item_BusinessRisk_Status_Significant;
                icon.className = 'icon-circle bigger-110';
            }
        }

        tdOpenDate.appendChild(document.createTextNode(FormatYYYYMMDD(item.OpenDate, '/')));
        tdInitialResult.appendChild(document.createTextNode(realResult == 0 ? '' : realResult));
        tdFinalResult.appendChild(document.createTextNode(item.RuleLimit == 0 ? '' : item.RuleLimit));
        tdName.appendChild(businessRiskLink);

        tdOpenDate.style.width = '90px';
        tdInitialResult.align = 'center';
        tdFinalResult.align = 'center';

        tdStatus.style.width = "40px";
        tdRules.style.width = "120px";
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

        var iconEdit = document.createElement('SPAN');
        iconEdit.className = 'btn btn-xs btn-info';
        iconEdit.id = item.Number;
        var innerEdit = document.createElement('I');
        innerEdit.className = ApplicationUser.Grants.BusinessRisk.Write ? 'icon-edit bigger-120' : 'icon-eye-open bigger-120';
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { document.location = 'BusinessRiskView.aspx?id=' + this.parentNode.parentNode.id; };

        if (ApplicationUser.Grants.BusinessRisk.Delete === true) {
            var iconDelete = document.createElement('SPAN');
            iconDelete.className = 'btn btn-xs btn-danger';
            iconDelete.id = item.Number;
            var innerDelete = document.createElement('I');
            innerDelete.className = 'icon-trash bigger-120';
            iconDelete.appendChild(innerDelete);
            iconDelete.onclick = function () { BusinessRiskDelete(this.parentNode.parentNode.id); };
        }

        var tdActions = document.createElement('TD');

        tdActions.appendChild(iconEdit);        
        if (ApplicationUser.Grants.BusinessRisk.Delete === true) {
            tdActions.appendChild(document.createTextNode(' '));
            tdActions.appendChild(iconDelete);
        }

        tdActions.style.width = "90px";
        row.appendChild(tdActions);

        target.appendChild(row);

        // Se añade el elemento al JSON del gráfico
        // si el reisgo está evaluado
        if (realResult > 0) {
            BusinessRiskGraph.push({
                "Id": item.BusinessRiskId,
                "Description": item.Description,
                "Code": item.Code,
                "Rules": item.Rules,
                "Result": realResult,
                "Assumed": realAction == 1,
                "RuleLimit": item.RuleLimit
            });
        }

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }

        if ($.inArray(objRules.Description, items) === -1) {
            items.push(objRules.Description);
        } 

        if ($.inArray(objProcess.Description, items) === -1) {
            items.push(objProcess.Description);
        }
    }

    document.getElementById('GraphicTableVoid').style.display = 'none';
    document.getElementById('svggrafic').style.display = '';
    exampleData();
    RenderChart();

    if (document.getElementById('CmbRules').value * 1 > 0) {
        DrawRuleLine();
        document.getElementById('BtnChangeIpr').style.display = '';
        rule = RuleGetById($("#CmbRules").val() * 1);
        if (rule !== null) {
            actualRuleLimit = rule.Limit;
            $('#RuleDescription').html('<strong>' + rule.Description + '</strong> ' + Dictionary.Item_Rules_FieldLabel_Limit + ': <strong>' + RuleLimitFromDB + '</strong>');
        }
    }
    else {
        rule = { "Id": 0 };
        resizegrafico($("#CmbRules").val() * 1);
        document.getElementById('BtnChangeIpr').style.display = 'none';
        $('#RuleDescription').html(Dictionary.Common_All_Female_Plural);
        actualRuleLimit = -1;
    }

    if (items.length === 0) {
        document.getElementById('nav-search').style.display = 'none';
    }
    else {
        document.getElementById('nav-search').style.display = '';

        items.sort(function (a, b) {
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        })
        var autocomplete = $('.nav-search-input').typeahead();
        autocomplete.data('typeahead').source = items;

        $('#nav-search-input').keyup(FilterList);
        $('#nav-search-input').change(FilterList);
    }

    $("#NumberCosts").html(list.length);
    $("#th1").click();
    if (document.getElementById("th1").className.indexOf("DESC") !== -1) {
        $("#th1").click()
    }
}

window.onload = function () {
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
            $("#TxtDateFrom").val(GetDateYYYYMMDDText(Filter.from, '/', false));
        }
        if (Filter.to !== null) {
            $("#TxtDateTo").val(GetDateYYYYMMDDText(Filter.to, '/', false));
        }
    }

        BusinessRiskGetFilter();
}

function RuleGetById(id)
{
    for(var x=0;x<CompanyRules.length;x++)
    {
        if(CompanyRules[x].Id === id)
        {
            return CompanyRules[x];
        }
    }

    return null;
}

function SetRule(id)
{
    if(rule.Id < 1)
    {
        return false;
    }

    actualRuleLimit = id;
    console.log("id:"+id);
    d3.selectAll("svg > *").remove();
    exampleData();
    RenderChart();
    DrawRuleLine();
    $("#input-span-slider").slider({ value: id });
}

function NewIpr()
{
    var candidate = new Array();
    for (var x=0;x<BusinessRiskGraph.length;x++)
    {
        if(BusinessRiskGraph[x].Assumed===false)
        {
            if(BusinessRiskGraph[x].Result <= RuleLimitFromDB)
            {
                if(BusinessRiskGraph[x].Result > actualRuleLimit)
                {
                    candidate.push(BusinessRiskGraph[x]);
                }
            }
        }
    }

    if(candidate.length>0)
    {
        message = Dictionary.Item_BusinessRisk_Message_IPR.replace('#', candidate.length);
        promptInfoUI(message, 450, NewIprConfirmed, null);
        return;
    }

    candidate = new Array();
    for (var x=0;x<BusinessRiskGraph.length;x++)
    {
        if(BusinessRiskGraph[x].Assumed===false)
        {
            if(BusinessRiskGraph[x].Result >= RuleLimitFromDB)
            {
                if(BusinessRiskGraph[x].Result < actualRuleLimit)
                {
                    candidate.push(BusinessRiskGraph[x]);
                }
            }
        }
    }

    if(candidate.length>0)
    {
        message = Dictionary.Item_BusinessRisk_Message_IPR_2.replace('#', candidate.length);
        promptInfoUI(message, 450, NewIprConfirmed, null);
        return;
    }

    NewIprConfirmed();
    return;
};

function NewIprConfirmed()
{
    var webMethod = "/Async/RulesActions.asmx/SetLimit";
    var data = {
        "rules":
            {
                "Id": rule.Id,
                "Limit": actualRuleLimit,
                "CompanyId": companyId
            },
        "companyId": companyId,
        "userId": user.Id
    };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            BusinessRiskGetFilter();
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function RenderSteps()
{
    VoidTable('steps');
    for(var x = 1; x<26;x++)
    {
        var span = document.createElement('span');
        span.id = x;
        span.className = 'tick';
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement('BR'));
        span.appendChild(document.createTextNode('|'));
        span.style.left = ((100/24)*(x-1))+'%';
        document.getElementById('steps').appendChild(span);
        span.onclick = function(){ SetRule(this.id) };
        if(x=== RuleLimitFromDB)
        {
            span.style.color = '#00f';
            span.style.fontWeight='bold';
        }
    }
}

function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var containerHeight = $(window).height();
    var finalHeight = containerHeight - 510;
    if (finalHeight < 400) {
        finalHeight = 400;
    }
    listTable.style.height = (finalHeight) + 'px';
}

window.onload = function () {
    Resize();
    $("#BtnRecordShowAll").click();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
}
window.onresize = function () { Resize(); }

function Export() {
    BusinessRiskGetFilter("PDF");
}

function ExportPDF() {
    var from = $('#TxtDateFrom').val();
    var to = $('#TxtDateTo').val();

    var rulesId = $('#CmbRules').val() * 1;
    var processId = $('#CmbProcess').val() * 1;
    var typeId = $('#CmbType').val() * 1;

    var data =
    {
        companyId: Company.Id,
        from: from,
        to: to,
        rulesId: rulesId,
        processId: processId,
        typeId: typeId
    };


    var webMethod = "/Export/BusinessriskExportList.aspx/PDF";
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            //successInfoUI(msg.d.MessageError, Go, 200);
            var link = document.createElement('a');
            link.id = 'download';
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = '_blank';
            document.body.appendChild(link);
            document.body.removeChild(link);
            $('#download').trigger('click');
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog('close');
        },
        error: function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}