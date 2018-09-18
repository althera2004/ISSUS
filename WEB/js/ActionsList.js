var lockOrderList = false;
var listOrder = "th2|DESC";

function IncidentActionGetFilter(exportType) {
    var ok = true;
    $("#nav-search-input").val();
    $("#ListDataTable").hide();
    $("#ItemTableError").hide();
    $("#ItemTableVoid").hide();
    $("#ErrorDate").hide();
    $("#rrorStatus").hide();
    $("#ErrorType").hide();
    var from = GetDate($("#TxtDateFrom").val(), "-");
    var to = GetDate($("#TxtDateTo").val(), "-");

    var status1 = document.getElementById("chkStatus1").checked;
    var status2 = document.getElementById("chkStatus2").checked;
    var status3 = document.getElementById("chkStatus3").checked;
    var status4 = document.getElementById("chkStatus4").checked;

    var type1 = document.getElementById("RType1").checked;
    var type2 = document.getElementById("RType2").checked;
    var type3 = document.getElementById("RType3").checked;

    if (from !== null && to !== null) {
        if (from > to) {
            ok = false;
            $("#ErrorDate").show();
        }
    }

    if (!status1 && !status2 && !status3 && !status4) {
        ok = false;
        $("#ErrorStatus").show();
    }

    if (!type1 && !type2 && !type3) {
        ok = false;
        $("#ErrorType").show();
    }

    if (ok === false) {
        $("#ItemTableError").show();
        return false;
    }

    var data =
    {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "statusIdnetified": status1,
        "statusAnalyzed": status2,
        "statusInProgress": status3,
        "statusClose": status4,
        "typeImprovement": type1,
        "typeFix": type2,
        "typePrevent": type3,
        "origin": $("#CmbOrigin").val() * 1,
        "reporter": $("#CmbReporter").val() * 1
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/GetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            eval("IncidentActionlist=" + msg.d + ";");
            ItemRenderTable(IncidentActionlist);
            if (exportType !== "undefined" && exportType !== null) {
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

function ItemRenderTable(list) {
    var items = new Array();
    var target = document.getElementById("ListDataTable");
    VoidTable("ListDataTable");
    target.style.display = "";

    if (list.length === 0) {
        $("#ItemTableVoid").show();
        $("#NumberCosts").html("0");
        target.style.display = "none";
        return false;
    }

    var total = 0;

    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        var row = document.createElement("TR");
        var tdNumber = document.createElement("TD");
        var tdOpen = document.createElement("TD");
        var tdType = document.createElement("TD");
        var tdStatus = document.createElement("TD");
        var tdOrigin = document.createElement("TD");
        var tdDescription = document.createElement("TD");
        var tdAction = document.createElement("TD");
        var tdAmount = document.createElement("TD");

        total += list[x].Amount;

        var status = "";
        var colorStatus = "#fff;";
        if (item.Status === 1) { status = Dictionary.Item_IndicentAction_Status1; colorStatus = "#f00"; }
        if (item.Status === 2) { status = Dictionary.Item_IndicentAction_Status2; colorStatus = "#dd0"; }
        if (item.Status === 3) { status = Dictionary.Item_IndicentAction_Status3; colorStatus = "#070"; }
        if (item.Status === 4) { status = Dictionary.Item_IndicentAction_Status4; colorStatus = "#000"; }

        var type = "";
        if (item.ActionType === 1) { type = Dictionary.Item_IncidentAction_Type1; }
        if (item.ActionType === 2) { type = Dictionary.Item_IncidentAction_Type2; }
        if (item.ActionType === 3) { type = Dictionary.Item_IncidentAction_Type3; }

        var origin = "";
        if (item.Origin === 1) { origin = document.createTextNode(Dictionary.Item_IncidentAction_Origin1); }
        if (item.Origin === 2) { origin = document.createTextNode(Dictionary.Item_IncidentAction_Origin2); }
        if (item.Origin === 3) {
            origin = document.createElement("A");
            origin.href = "IncidentView.aspx?id=" + item.Associated.Id;
            origin.appendChild(document.createTextNode(Dictionary.Item_IncidentAction_Origin3));
            var spantext = document.createElement("span");
            spantext.style.display = "none";
            spantext.appendChild(document.createTextNode(item.Associated.Description));
            origin.appendChild(spantext);
			origin.title = item.Associated.Description;
        }

        if (item.Origin === 4) {
            origin = document.createElement("A");
            origin.href = "BusinessRiskView.aspx?id=" + item.Associated.Id;
            origin.appendChild(document.createTextNode(Dictionary.Item_IncidentAction_Origin4));
            var businessRiskName = document.createElement("SPAN");
            businessRiskName.style.display = "none";
            businessRiskName.appendChild(document.createTextNode(item.Associated.Description));
            origin.appendChild(businessRiskName);
			origin.title = item.Associated.Description;
        }

        if (item.Origin === 5) {
            origin = document.createElement("A");
            origin.href = "ObjetivoView.aspx?id=" + item.Associated.Id;
            origin.appendChild(document.createTextNode(Dictionary.Item_IncidentAction_Origin5));
            var objetivoName = document.createElement("SPAN");
            objetivoName.style.display = "none";
            objetivoName.appendChild(document.createTextNode(item.Associated.Description));
            origin.appendChild(objetivoName);
            origin.title = item.Associated.Description;
        }

        if (item.Origin === 6) {
            origin = document.createElement("A");
            origin.href = "OportunityView.aspx?id=" + item.Associated.Id;
            origin.appendChild(document.createTextNode(Dictionary.Item_IncidentAction_Origin6));
            var oportunityName = document.createElement("SPAN");
            oportunityName.style.display = "none";
            oportunityName.appendChild(document.createTextNode(item.Associated.Description));
            origin.appendChild(oportunityName);
            origin.title = item.Associated.Description;
        }

        var actionLink = document.createElement("A");
        actionLink.appendChild(document.createTextNode(item.Number));
        actionLink.href = "ActionView.aspx?id=" + item.Id;

        row.id = item.Id;

        var iconStatus = document.createElement("I");
		if (item.Status === 1) {
            iconStatus.className = "fa icon-pie-chart";
			iconStatus.title = Dictionary.Item_IndicentAction_Status1;
        }
		if (item.Status === 2) {
            iconStatus.className = "fa icon-pie-chart";
			iconStatus.title = Dictionary.Item_IndicentAction_Status2;
        }
		if (item.Status === 3) {
            iconStatus.className = "fa icon-play";
			iconStatus.title = Dictionary.Item_IndicentAction_Status3;
        }
        if (item.Status === 4) {
            iconStatus.className = "fa icon-lock";
			iconStatus.title = Dictionary.Item_IndicentAction_Status4;
        }
        
		iconStatus.style.color = colorStatus;
        tdNumber.appendChild(iconStatus);

        tdOpen.appendChild(document.createTextNode(FormatYYYYMMDD(item.OpenDate, "/")));
        tdType.appendChild(document.createTextNode(type));
        tdStatus.appendChild(iconStatus);
        tdOrigin.appendChild(origin);

        var actionLinkDescription = document.createElement("A");
        actionLinkDescription.appendChild(document.createTextNode(item.Description));
        actionLinkDescription.href = "ActionView.aspx?id=" + item.Id;
        tdDescription.appendChild(actionLinkDescription);

        tdAction.appendChild(document.createTextNode(FormatYYYYMMDD(item.ImplementationDate, "/")));
        tdAmount.appendChild(document.createTextNode(ToMoneyFormat(item.Amount, 2)));

        tdType.style.width = "100px";
		tdOpen.style.width = "100px";
		tdOpen.align = "center";
		tdStatus.style.width = "60px";
		tdStatus.align = "center";
        tdOrigin.style.width = "250px";
        tdAction.style.width = "100px";
		tdAction.align = "center";
        tdAmount.style.width = "100px";
        tdAmount.align = "right";
        
        row.appendChild(tdStatus);
        row.appendChild(tdOpen);
        row.appendChild(tdDescription);
        row.appendChild(tdOrigin);
        row.appendChild(tdType);
        row.appendChild(tdAction);
        row.appendChild(tdAmount);

        var iconEdit = document.createElement("SPAN");
        iconEdit.className = "btn btn-xs btn-info";
        iconEdit.id = item.Number;
        var innerEdit = document.createElement("I");
        innerEdit.className = ApplicationUser.Grants.IncidentActions.Write ? "icon-edit bigger-120" : "icon-eye-open bigger-120";
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { document.location = "ActionView.aspx?id=" + this.parentNode.parentNode.id; };

        if (ApplicationUser.Grants.IncidentActions.Delete === true) {
            var iconDelete = document.createElement("SPAN");
            iconDelete.className = "btn btn-xs btn-danger";
            iconDelete.id = item.Number;
            var innerDelete = document.createElement("I");
            innerDelete.className = "icon-trash bigger-120";
            iconDelete.appendChild(innerDelete);

            if (item.Origin === 3) {
                iconDelete.onclick = function () { NoDeleteIncident(); };
            }
            else if (item.Origin === 4) {
                iconDelete.onclick = function () { NoDeleteBusinessRisk(); };
            }
            else if (item.Origin === 5) {
                iconDelete.onclick = function () { NoDeleteObjetive(); };
            }
            else if (item.Origin === 6) {
                iconDelete.onclick = function () { NoDeleteOportunity(); };
            }
            else {
                iconDelete.onclick = function () { IncidentActionDelete(this); };
            }
        }

        var tdActions = document.createElement("TD");
        tdActions.style.width = "90px";

        tdActions.appendChild(iconEdit);
        if (ApplicationUser.Grants.IncidentActions.Delete) {
            tdActions.appendChild(document.createTextNode(" "));
            tdActions.appendChild(iconDelete);
        }

        row.appendChild(tdActions);
        target.appendChild(row);

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }
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
    $("#TotalCosts").html(ToMoneyFormat(total, 2));

    if (lockOrderList === false) {
        $("#th1").click();
        if (document.getElementById("th1").className.indexOf("DESC") !== -1) { $("#th1").click(); }
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

function IncidentActionDelete(sender) {
    IncidentActionSelectedId = sender.parentNode.parentNode.id * 1;
    IncidentActionSelected = IncidentActiongetById(IncidentActionSelectedId);
    console.log("IncidentActionDelete", IncidentActionSelectedId);
    if (IncidentActionSelected === null) { return false; }
    $("#IncidentActionDeleteName").html(IncidentActionSelected.Description);
    var dialog = $("#IncidentActionDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Common_Delete + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    IncidentActionDeleteConfirmed();
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

function IncidentActionDeleteConfirmed() {
    var data = {
        "incidentActionId": IncidentActionSelectedId,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#IncidentActionDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/IncidentActionsActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            IncidentActionGetFilter();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function IncidentActiongetById(id) {
    for (var x = 0; x < IncidentActionlist.length; x++) {
        if (IncidentActionlist[x].Id === id) {
            return IncidentActionlist[x];
        }
    }
    return null;
}

function NoDeleteBusinessRisk() {
    alertInfoUI(Dictionary.Item_IncidentAction_ErrorMessage_NoDeleteBusinessRisk, null);
}

function NoDeleteObjetive() {
    alertInfoUI(Dictionary.Item_IncidentAction_ErrorMessage_NoDeleteObjetive, null);
}

function NoDeleteOportunity() {
    alertInfoUI(Dictionary.Item_IncidentAction_ErrorMessage_NoDeleteOportunity, null);
}

function NoDeleteIncident() {
    alertInfoUI(Dictionary.Item_IncidentAction_ErrorMessage_NoDeleteIncident, null);
}

$("#nav-search").hide();

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 450);
}

window.onload = function () {
    $("#th2").click();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
    Resize();

    $("#chkStatus1").on("click", IncidentActionGetFilter);
    $("#chkStatus2").on("click", IncidentActionGetFilter);
    $("#chkStatus3").on("click", IncidentActionGetFilter);
    $("#chkStatus4").on("click", IncidentActionGetFilter);
    $("#CmbOrigin").on("click", IncidentActionGetFilter);
    $("#CmbReporter").on("click", IncidentActionGetFilter);
    $("#RType1").on("click", IncidentActionGetFilter);
    $("#RType2").on("click", IncidentActionGetFilter);
    $("#Rtype3").on("click", IncidentActionGetFilter);
    $("#TxtDateFrom").on("change", IncidentActionGetFilter);
    $("#TxtDateTo").on("change", IncidentActionGetFilter);
};

window.onresize = function () { Resize(); };

function Export() {
    lockOrderList = true;
    IncidentActionGetFilter("PDF");
}

function ExportPDF() {
    var from = $("#TxtDateFrom").val();
    var to =$("#TxtDateTo").val();

    var status1 = document.getElementById("chkStatus1").checked;
    var status2 = document.getElementById("chkStatus2").checked;
    var status3 = document.getElementById("chkStatus3").checked;
    var status4 = document.getElementById("chkStatus4").checked;

    var type1 = document.getElementById("RType1").checked;
    var type2 = document.getElementById("RType2").checked;
    var type3 = document.getElementById("RType3").checked;

    var data =
    {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "statusIdentified": status1,
        "statusAnalyzed": status2,
        "statusInProgress": status3,
        "statusClose": status4,
        "typeImprovement": type1,
        "typeFix": type2,
        "typePrevent": type3,
        "origin": $("#CmbOrigin").val() * 1,
        "reporter": $("#CmbReporter").val() * 1,
        "listOrder": listOrder
    };

    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/IncidentActionExportList.asmx/PDF",
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