var lockOrderList = false;

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;';
            if (("title_html" in this.options) && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));

    /*$('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: 'ca'
    });*/
    var options = $.extend({}, $.datepicker.regional["ca"], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);

    $("#BtnSearch").on('click', function (e) {
        e.preventDefault();
        IncidentGetFilter();
    });

    $('#BtnRecordShowAll').on('click', function (e) {
        e.preventDefault();
        IncidentListGetAll();
    });

    $('#BtnRecordShowNone').on('click', function (e) {
        e.preventDefault();
        IncidentListGetNone();
    });
});

function IncidentListGetAll() {
    document.getElementById('BtnRecordShowAll').style.display = 'none';
    document.getElementById('BtnRecordShowNone').style.display = '';
    var ok = true;
    VoidTable('ListDataTable');

    document.getElementById('RIncidentStatus1').checked = true;
    document.getElementById('RIncidentStatus2').checked = true;
    document.getElementById('RIncidentStatus3').checked = true;
    document.getElementById('RIncidentStatus4').checked = true;

    document.getElementById('ROrigin0').checked = true;
    document.getElementById('ROrigin1').checked = false;
    document.getElementById('ROrigin2').checked = false;
    document.getElementById('ROrigin3').checked = false;
    document.getElementById('ItemTableError').style.display = 'none';
    document.getElementById('ItemTableVoid').style.display = 'none';
    document.getElementById('ErrorDate').style.display = 'none';
    document.getElementById('ErrorStatus').style.display = 'none';
    document.getElementById('ErrorOrigin').style.display = 'none';
    document.getElementById('ErrorDepartment').style.display = 'none';
    document.getElementById('ErrorProvider').style.display = 'none';
    document.getElementById('ErrorCustomer').style.display = 'none';
    var from = GetDate($('#TxtDateFrom').val(), '-');
    var to = GetDate($('#TxtDateTo').val(), '-');
    IncidentGetFilter();
}

function IncidentListGetNone() {
    document.getElementById('BtnRecordShowAll').style.display = '';
    document.getElementById('BtnRecordShowNone').style.display = 'none';
    var ok = true;
    VoidTable('ListDataTable');

    document.getElementById('RIncidentStatus1').checked = false;
    document.getElementById('RIncidentStatus2').checked = false;
    document.getElementById('RIncidentStatus3').checked = false;
    document.getElementById('RIncidentStatus4').checked = false;

    document.getElementById('ROrigin0').checked = false;
    document.getElementById('ROrigin1').checked = false;
    document.getElementById('ROrigin2').checked = false;
    document.getElementById('ROrigin3').checked = false;
    document.getElementById('ItemTableError').style.display = 'none';
    document.getElementById('ItemTableVoid').style.display = 'none';
    document.getElementById('ErrorDate').style.display = 'none';
    document.getElementById('ErrorStatus').style.display = 'none';
    document.getElementById('ErrorOrigin').style.display = 'none';
    document.getElementById('ErrorDepartment').style.display = 'none';
    document.getElementById('ErrorProvider').style.display = 'none';
    document.getElementById('ErrorCustomer').style.display = 'none';
    var from = GetDate($('#TxtDateFrom').val(), '-');
    var to = GetDate($('#TxtDateTo').val(), '-');
}

var targetCmbOrigin1 = document.getElementById('CmbOrigin1');
/*var departmentDefaultOption = document.createElement('OPTION');
departmentDefaultOption.value = 0;
departmentDefaultOption.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
target.appendChild(departmentDefaultOption);*/

var departmentAllOption = document.createElement('OPTION');
departmentAllOption.value = -2;
departmentAllOption.appendChild(document.createTextNode(Dictionary.Common_All_Male_Plural));
targetCmbOrigin1.appendChild(departmentAllOption);

for (var x = 0; x < Departments.length; x++) {
    var optionCmbOrigin1 = document.createElement('OPTION');
    optionCmbOrigin1.value = Departments[x].Id;
    optionCmbOrigin1.appendChild(document.createTextNode(Departments[x].Description));
    targetCmbOrigin1.appendChild(optionCmbOrigin1);
}

var targetCmbOrigin2 = document.getElementById('CmbOrigin2');
/*var providerDefaultOption = document.createElement('OPTION');
providerDefaultOption.value = 0;
providerDefaultOption.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
target.appendChild(providerDefaultOption);*/

var providerAllOption = document.createElement('OPTION');
providerAllOption.value = -2;
providerAllOption.appendChild(document.createTextNode(Dictionary.Common_All_Male_Plural));
targetCmbOrigin2.appendChild(providerAllOption);

for (var x2 = 0; x2 < Providers.length; x2++) {
    var optionCmbOrigin2 = document.createElement('OPTION');
    optionCmbOrigin2.value = Providers[x2].Id;
    optionCmbOrigin2.appendChild(document.createTextNode(Providers[x2].Description));
    targetCmbOrigin2.appendChild(optionCmbOrigin2);
}

var targetCmbOrigin3 = document.getElementById('CmbOrigin3');
/*var customerDefaultOption = document.createElement('OPTION');
customerDefaultOption.value = 0;
customerDefaultOption.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
target.appendChild(customerDefaultOption);*/

var customerAllOption = document.createElement('OPTION');
customerAllOption.value = -2;
customerAllOption.appendChild(document.createTextNode(Dictionary.Common_All_Male_Plural));
targetCmbOrigin3.appendChild(customerAllOption);

for (var x3 = 0; x3 < Customers.length; x3++) {
    var optionCmbOrigin3 = document.createElement('OPTION');
    optionCmbOrigin3.value = Customers[x3].Id;
    optionCmbOrigin3.appendChild(document.createTextNode(Customers[x3].Description));
    targetCmbOrigin3.appendChild(optionCmbOrigin3);
}

function ShowCombos(x) {
    document.getElementById("CmbOrigin1").style.display = x === 1 ? "" : "none";
    document.getElementById("CmbOrigin2").style.display = x === 2 ? "" : "none";
    document.getElementById("CmbOrigin3").style.display = x === 3 ? "" : "none";
}

ShowCombos(0);

function IncidentGetFilter(exportType) {
    document.getElementById("nav-search-input").display = "none";
    var ok = true;
    VoidTable("ListDataTable");
    document.getElementById("ItemTableError").style.display = "none";
    document.getElementById("ItemTableVoid").style.display = "none";
    document.getElementById("ErrorDateFrom").style.display = "none";
    document.getElementById("ErrorDateTo").style.display = "none";
    document.getElementById("ErrorDate").style.display = "none";
    document.getElementById("ErrorStatus").style.display = "none";
    document.getElementById("ErrorOrigin").style.display = "none";
    document.getElementById("ErrorDepartment").style.display = "none";
    document.getElementById("ErrorProvider").style.display = "none";
    document.getElementById("ErrorCustomer").style.display = "none";
    var from = GetDate($("#TxtDateFrom").val(), "-");
    var to = GetDate($("#TxtDateTo").val(), "-");

    var status1 = document.getElementById("RIncidentStatus1").checked;
    var status2 = document.getElementById("RIncidentStatus2").checked;
    var status3 = document.getElementById("RIncidentStatus3").checked;
    var status4 = document.getElementById("RIncidentStatus4").checked;

    var origin0 = document.getElementById("ROrigin0").checked;
    var origin1 = document.getElementById("ROrigin1").checked;
    var origin2 = document.getElementById("ROrigin2").checked;
    var origin3 = document.getElementById("ROrigin3").checked;

    if ($("#TxtDateFrom").val() !== "") {
        if (!validateDate($('#TxtDateFrom').val())) {
            ok = false;
            document.getElementById('ErrorDateFrom').style.display = "";
            from = null;
        }
    }

    if ($("#TxtDateTo").val() !== "") {
        if (!validateDate($("#TxtDateTo").val())) {
            ok = false;
            document.getElementById("ErrorDateTo").style.display = "";
            to = null;
        }
    }

    if (from !== null && to !== null)
    {
        if (from > to) {
            ok = false;
            document.getElementById("ErrorDate").style.display = "";
        }
    }

    if (!status1 && !status2 && !status3 && !status4) {
        ok = false;
        document.getElementById('ErrorStatus').style.display = '';
    }

    if (!origin0) {
        if (origin1 && $('#CmbOrigin1').val() === 0) {
            ok = false;
            //document.getElementById('ErrorOrigin').style.display = '';
            document.getElementById('ErrorDepartment').style.display = '';
        }
        if (origin2 && $('#CmbOrigin2').val() === 0) {
            ok = false;
            //document.getElementById('ErrorOrigin').style.display = '';
            document.getElementById('ErrorProvider').style.display = '';
        }
        if (origin3 && $('#CmbOrigin3').val() === 0) {
            ok = false;
            //document.getElementById('ErrorOrigin').style.display = '';
            document.getElementById('ErrorCustomer').style.display = '';
        }
    }

    if (ok === false) {
        document.getElementById('ItemTableError').style.display = '';
        return false;
    }

    var origin = 0;
    var originId = 0;
    if (origin1 === true) { origin = 1; originId = $('#CmbOrigin1').val() * 1; }
    if (origin2 === true) { origin = 2; originId = $('#CmbOrigin2').val() * 1; }
    if (origin3 === true) { origin = 3; originId = $('#CmbOrigin3').val() * 1; }
    var data =
    {
        companyId: Company.Id,
        from: from,
        to: to,
        statusIdnetified: status1,
        statusAnalyzed: status2,
        statusInProgress: status3,
        statusClose: status4,
        origin: origin,
        departmentId: $('#CmbOrigin1').val() * 1,
        providerId: $('#CmbOrigin2').val() * 1,
        customerId: $('#CmbOrigin3').val() * 1
    };
    $.ajax({
        type: "POST",
        url: "/Async/IncidentActions.asmx/GetFilter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            eval("IncidentList=" + msg.d + ";");
            ItemRenderTable(IncidentList);
            if (typeof exportType !== "undefined" && exportType !== "null") {
                ExportPDF();
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ItemRenderTable(list) {
    items = new Array();
    var target = document.getElementById('ListDataTable');
    VoidTable('ListDataTable');
    target.style.display = '';

    if (list.length === 0) {
        document.getElementById('ItemTableVoid').style.display = '';
        target.style.display = 'none';
        $("#NumberCosts").html("0");
        return false;
    }

    var total = 0;
    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        var row = document.createElement('TR');
        var tdNumber = document.createElement('TD');
        var tdOpen = document.createElement('TD');
        var tdStatus = document.createElement('TD');
        var tdOrigin = document.createElement('TD');
        var tdDescription = document.createElement('TD');
        var tdAction = document.createElement('TD');
        var tdAmount = document.createElement('TD');
        var tdClose = document.createElement('TD');

        var status = '';
        var colorStatus = '#fff;'
        if (item.Status === 1) { status = Dictionary.Item_Incident_Status1; colorStatus = '#f00'; }
        if (item.Status === 2) { status = Dictionary.Item_Incident_Status2; colorStatus = '#dd0'; }
        if (item.Status === 3) { status = Dictionary.Item_Incident_Status3; colorStatus = '#070'; }
        if (item.Status === 4) { status = Dictionary.Item_Incident_Status4; colorStatus = '#000'; }

        if (item.Department.Id > 0) {
            if (user.Grants.Department.Read === false) {
                tdOrigin.appendChild(document.createTextNode(item.Department.Name));
            }
            else {
                var link = document.createElement('A');
                link.href = 'DepartmentView.aspx?id=' + item.Department.Id;
                link.appendChild(document.createTextNode(item.Department.Description));
                tdOrigin.appendChild(link);
            }
        }

        if (item.Provider.Id > 0) {
            if (user.Grants.Provider === false) {
                tdOrigin.appendChild(document.createTextNode(item.Provider.Description));
            }
            else {
                var link1 = document.createElement('A');
                link1.href = 'ProvidersView.aspx?id=' + item.Provider.Id;
                link1.appendChild(document.createTextNode(item.Provider.Description));
                tdOrigin.appendChild(link1);
            }
        }

        if (item.Customer.Id > 0) {
            if (user.Grants.Customer.Read === false) {
                tdOrigin.appendChild(document.createTextNode(item.Customer.Description));
            }
            else {
                var link2 = document.createElement('A');
                link2.href = 'CustomersView.aspx?id=' + item.Customer.Id;
                link2.appendChild(document.createTextNode(item.Customer.Description));
                tdOrigin.appendChild(link2);
            }
        }

        if (item.Action.Id === 0) {
            tdAction.appendChild(document.createTextNode(' '));
        }
        else {
            if (user.Grants.IncidentActions.Read === false) {
                //tdAction.appendChild(document.createTextNode(item.Action.Description));
				tdAction.appendChild(document.createTextNode(Dictionary.Common_View));
            }
            else {
                var link3 = document.createElement('A');
                link3.href = 'ActionView.aspx?id=' + item.Action.Id;
                //link3.appendChild(document.createTextNode(item.Action.Description));
				link3.appendChild(document.createTextNode(Dictionary.Common_View));
                tdAction.appendChild(link3);
            }
        }

        row.id = item.IncidentId;

        var incidentLink = document.createElement('A');
        incidentLink.href = 'IncidentView.aspx?id=' + item.IncidentId;
        incidentLink.appendChild(document.createTextNode(item.Code));

        tdNumber.appendChild(incidentLink);
        tdOpen.appendChild(document.createTextNode(FormatYYYYMMDD(item.Open, '/')));

        var iconStatus = document.createElement('I');
		
		if (item.Status === 1) {
            iconStatus.className = "fa icon-pie-chart";
			iconStatus.title = Dictionary.Item_Incident_Status1;
        }
		if (item.Status === 2) {
            iconStatus.className = "fa icon-pie-chart";
			iconStatus.title = Dictionary.Item_Incident_Status2;
        }
		if (item.Status === 3) {
            iconStatus.className = "fa icon-play";
			iconStatus.title = Dictionary.Item_Incident_Status3;
        }
        if (item.Status === 4) {
            iconStatus.className = "fa icon-lock";
			iconStatus.title = Dictionary.Item_Incident_Status4;
        }
        iconStatus.style.color = colorStatus;
        tdStatus.appendChild(iconStatus);
        //tdStatus.appendChild(document.createTextNode(' ' + status));

        var incidentLinkDescription = document.createElement('A');
        incidentLinkDescription.href = 'IncidentView.aspx?id=' + item.IncidentId;
        incidentLinkDescription.appendChild(document.createTextNode(item.Description));
        tdDescription.appendChild(incidentLinkDescription);

        tdClose.appendChild(document.createTextNode(FormatYYYYMMDD(item.Close, '/')));

        tdAmount.appendChild(document.createTextNode(ToMoneyFormat(item.Amount, 2)));

        tdOpen.style.width = "100px";
		tdOpen.align = "center"
		tdStatus.style.width = "60px";
		tdStatus.align = "center"
        tdOrigin.style.width = "200px";
		tdAction.style.width = "90px";
        tdAction.align = "center"
        tdAmount.style.width = "100px";
        tdAmount.align = "right";
		tdClose.style.width = "100px";
		tdClose.align = "center"

        //row.appendChild(tdNumber);
        row.appendChild(tdDescription);
        row.appendChild(tdOpen);
        row.appendChild(tdStatus);
        row.appendChild(tdOrigin);
        row.appendChild(tdAction);
        row.appendChild(tdAmount);
        row.appendChild(tdClose);

        var iconEdit = document.createElement('SPAN');
        iconEdit.className = 'btn btn-xs btn-info';
        iconEdit.id = item.Number;
        var innerEdit = document.createElement('I');
        innerEdit.className = ApplicationUser.Grants.Incident.Write ? 'icon-edit bigger-120' : 'icon-eye-open bigger-120';
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { document.location = 'IncidentView.aspx?id=' + this.parentNode.parentNode.id; };

        if (ApplicationUser.Grants.Incident.Delete === true) {
            var iconDelete = document.createElement('SPAN');
            iconDelete.className = 'btn btn-xs btn-danger';
            iconDelete.id = item.Number;
            var innerDelete = document.createElement('I');
            innerDelete.className = 'icon-trash bigger-120';
            iconDelete.appendChild(innerDelete);
            iconDelete.onclick = function () { IncidentDelete(this); };
        }

        var tdActions = document.createElement('TD');
        tdActions.style.width = "91px";

        tdActions.appendChild(iconEdit);
        if (ApplicationUser.Grants.Incident.Delete) {
            tdActions.appendChild(document.createTextNode(' '));
            tdActions.appendChild(iconDelete);
        }
        row.appendChild(tdActions);

        target.appendChild(row);
        total += item.Amount;

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }
        if ($.inArray(item.Action.Description, items) === -1) {
            items.push(item.Action.Description);
        }
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
    $("#TotalCosts").html(ToMoneyFormat(total, 2));

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

if (Filter !== null) {
    document.getElementById('TxtDateFrom').value = GetDateYYYYMMDDText(Filter.from, '/', false);
    document.getElementById('TxtDateTo').value = GetDateYYYYMMDDText(Filter.to, '/', false);
    document.getElementById('RIncidentStatus1').checked = Filter.statusIdnetified;
    document.getElementById('RIncidentStatus2').checked = Filter.statusAnalyzed;
    document.getElementById('RIncidentStatus3').checked = Filter.statusInProgress;
    document.getElementById('RIncidentStatus4').checked = Filter.statusClose;
    if (Filter.origin === 0) {
        document.getElementById('ROrigin0').checked = true;
    }

    if (Filter.origin === 1) {
        document.getElementById('ROrigin1').checked = true;
        document.getElementById('CmbOrigin1').value = Filter.departmentId;
        document.getElementById('CmbOrigin1').style.display = 'block';
    }

    if (Filter.origin === 2) {
        document.getElementById('ROrigin2').checked = true;
        document.getElementById('CmbOrigin2').value = Filter.providerId;
        document.getElementById('CmbOrigin2').style.display = 'block';
    }

    if (Filter.origin === 3) {
        document.getElementById('ROrigin3').checked = true;
        document.getElementById('CmbOrigin3').value = Filter.customerId;
        document.getElementById('CmbOrigin3').style.display = 'block';
    }

    IncidentGetFilter();
}

function IncidentDelete(sender) {
    IncidentSelectedId = sender.parentNode.parentNode.id;
    IncidentSelected = IncidentGetById(IncidentSelectedId);
    if (IncidentSelected === null) { return false; }
    $('#IncidentDeleteName').html(IncidentSelected.Description);
    var dialog = $("#IncidentDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Incident_Popup_Delete_Title+'</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    IncidentDeleteConfirmed();
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

function IncidentDeleteConfirmed() {
    var webMethod = "/Async/IncidentActions.asmx/Delete";
    var data = { incidentId: IncidentSelectedId, companyId: Company.Id, userId: user.Id };
    $("#IncidentDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            IncidentGetFilter();
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function IncidentGetById(id) {
    id = id * 1;
    for (var x = 0; x < IncidentList.length; x++) {
        if (IncidentList[x].IncidentId === id) {
            return IncidentList[x];
        }
    }

    return null;
}

IncidentGetFilter();

$("#nav-search").hide();

function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 450) + 'px';
}

window.onload = function () {
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
    Resize();
}

window.onresize = function () { Resize(); }

function Export() {
    lockOrderList = true;
    IncidentGetFilter("PDF");
}

function ExportPDF() {
    var from = $("#TxtDateFrom").val();
    var to = $("#TxtDateTo").val();

    var status1 = document.getElementById("RIncidentStatus1").checked;
    var status2 = document.getElementById("RIncidentStatus2").checked;
    var status3 = document.getElementById("RIncidentStatus3").checked;
    var status4 = document.getElementById("RIncidentStatus4").checked;

    var origin0 = document.getElementById("ROrigin0").checked;
    var origin1 = document.getElementById("ROrigin1").checked;
    var origin2 = document.getElementById("ROrigin2").checked;
    var origin3 = document.getElementById("ROrigin3").checked;

    var origin = 0;
    var originId = 0;
    if (origin1 === true) { origin = 1; originId = $("#CmbOrigin1").val() * 1; }
    if (origin2 === true) { origin = 2; originId = $("#CmbOrigin2").val() * 1; }
    if (origin3 === true) { origin = 3; originId = $("#CmbOrigin3").val() * 1; }

    var data =
    {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "statusIdnetified": status1,
        "statusAnalyzed": status2,
        "statusInProgress": status3,
        "statusClose": status4,
        "origin": origin,
        "departmentId": $("#CmbOrigin1").val() * 1,
        "providerId": $("#CmbOrigin2").val() * 1,
        "customerId": $("#CmbOrigin3").val() * 1,
        "listOrder": listOrder
    };

    var webMethod = "/Export/IncidentExportList.aspx/PDF";
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
        error: function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}