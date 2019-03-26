var lockOrderList = false;

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

    $("#BtnSearch").on("click", function (e) {
        e.preventDefault();
        QuestionaryGetFilter();
    });

    $("#BtnRecordShowAll").on("click", function (e) {
        e.preventDefault();
        IncidentListGetAll();
    });

    $("#BtnRecordShowNone").on("click", function (e) {
        e.preventDefault();
        IncidentListGetNone();
    });
});

function IncidentListGetAll() {
    //$("#BtnRecordShowAll").hide();
    //$("#BtnRecordShowNone").show();
    var ok = true;
    VoidTable("ListDataTable");

    $("#CmbProcess").val(-1);
    $("#CmbRules").val(-1);
    $("#CmbApartadosNorma").val(-1);
    QuestionaryGetFilter();
}

function IncidentListGetNone() {
    $("#BtnRecordShowAll").hide();
    $("#BtnRecordShowNone").show();
    var ok = true;
    VoidTable("ListDataTable");

    $("#CmbProcess").val(-1);
    $("#CmbRules").val(-1);
    $("#CmbApartadosNorma").val(-1);
    QuestionaryGetFilter();
}

function QuestionaryGetFilter(exportType) {
    //$("#nav-search-input").hide();
    var ok = true;
    VoidTable("ListDataTable");
    $("#ItemTableError").hide();
    $("#ItemTableVoid").hide();
    var data = {
        "companyId": Company.Id,
        "processId": $("#CmbProcess").val() * 1,
        "ruleId": $("#CmbRules").val() * 1,
        "apartadoNorma": $("#CmbApartadosNorma").val()
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/QuestionaryActions.asmx/GetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            eval("QuestionaryList=" + msg.d + ";");
            ItemRenderTable(QuestionaryList);
            if (typeof exportType !== "undefined" && exportType !== "null" && exportType === "PDF") {
                ExportPDF();
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function Go(sender) {
    document.location = "QuestionaryView.aspx?id=" + sender.id;
}

function ItemRenderTable(list) {
    items = new Array();
    var target = document.getElementById("ListDataTable");
    VoidTable("ListDataTable");
    target.style.display = "";
    var count = 0;
    for (var x = 0; x < list.length; x++) {
        if (list[x].Active === false) {
            continue;
        }

        count++;
        var item = list[x];
        var row = document.createElement("TR");
        var tdDescription = document.createElement("TD");
        var tdRule = document.createElement("TD");
        var tdProcess = document.createElement("TD");
        row.id = item.IncidentId;

        var questionaryLink = document.createElement("A");
        questionaryLink.title = item.Description;
        questionaryLink.href = "QuestionaryView.aspx?id=" + item.Id;
        questionaryLink.appendChild(document.createTextNode(item.Description));
        tdDescription.appendChild(questionaryLink);
        if (item.NQuestions == 0) {
            var icon = document.createElement("I");
            icon.className = "fa fa-warning";
            icon.style.color = "#f77";
            icon.style.marginLeft = "4px";
            icon.title = Dictionary.Item_Questionary_Warning_NoQuestions
            tdDescription.appendChild(icon);
        }

        var linkProcess = document.createElement("A");
        linkProcess.title = item.Process.Description;
        linkProcess.href = "ProcesosView.aspx?id=" + item.Process.Id;
        linkProcess.appendChild(document.createTextNode(item.Process.Description));
        tdProcess.appendChild(linkProcess);

        var linkRule = document.createElement("A");
        linkRule.title = item.Rule.Description;
        linkRule.href = "RulesView.aspx?id=" + item.Rule.Id;
        linkRule.appendChild(document.createTextNode(item.Rule.Description));
        tdRule.appendChild(linkRule);

        if (item.ApartadoNorma !== null && item.ApartadoNorma !== "") {
            tdRule.append(document.createTextNode(" - " + item.ApartadoNorma));
        }

        tdProcess.style.width = "300px";
        tdRule.style.width = "300px";

        row.appendChild(tdDescription);
        row.appendChild(tdProcess);
        row.appendChild(tdRule);

        var tdActions = document.createElement("TD");

        var iconEdit = document.createElement("SPAN");
        iconEdit.className = "btn btn-xs btn-info";
        iconEdit.id = item.Id;
        var innerEdit = document.createElement("I");
        innerEdit.className = "icon-edit bigger-120";
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { Go(this); };
        tdActions.appendChild(iconEdit);

        if (ApplicationUser.Grants.Questionary.Delete === true) {
            var iconDelete = document.createElement("SPAN");
            iconDelete.className = "btn btn-xs btn-danger";
            iconDelete.id = item.Id;
            var innerDelete = document.createElement("I");
            innerDelete.className = "icon-trash bigger-120";
            iconDelete.appendChild(innerDelete);
            iconDelete.onclick = function () { QuestionaryDelete(this); };
            tdActions.appendChild(document.createTextNode(" "));
            tdActions.appendChild(iconDelete);
        }

        if (ApplicationUser.Grants.Questionary.Write === true) {
            var iconCopy = document.createElement("SPAN");
            iconCopy.className = "btn btn-xs btn-success";
            iconCopy.id = item.Id;
            var innerCopy = document.createElement("I");
            innerCopy.className = "icon-copy bigger-120";
            iconCopy.appendChild(innerCopy);
            iconCopy.onclick = function () { QuestionaryDuplicate(this); };
            tdActions.appendChild(document.createTextNode(" "));
            tdActions.appendChild(iconCopy);
        }

        tdActions.style.width = "130px";

        row.appendChild(tdActions);

        target.appendChild(row);

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }

        if ($.inArray(item.Process.Description, items) === -1) {
            items.push(item.Process.Description);
        }

        if ($.inArray(item.Rule.Description, items) === -1) {
            items.push(item.Rule.Description);
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

    $("#QuestionaryDataTotal").html(count);

    if (lockOrderList === false) {
        $("#th0").click();
        if (document.getElementById("th0").className.indexOf("DESC") !== -1) {
            $("#th0").click();
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
    $("#CmbProcess").val(Filter.processId);
    $("#CmbRule").val(Filter.ruleId);
    $("#CmbApartadoNorma").val(Filter.apartadoNorma);
    QuestionaryGetFilter();
}

function QuestionaryDelete(sender) {
    QuestionarySelectedId = sender.id * 1;
    QuestionarySelected = QuestionaryGetById(QuestionarySelectedId);
    if (QuestionarySelected === null) { return false; }
    $("#QuestionaryDeleteName").html(QuestionarySelected.Description);
    var dialog = $("#QuestionaryDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Questionary_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        QuestionaryDeleteConfirmed();
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

function QuestionaryDeleteConfirmed() {
    var data = {
        "questionarioId": QuestionarySelectedId,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#QuestionaryDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/QuestionaryActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            QuestionaryGetFilter();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function QuestionaryGetById(id) {
    id = id * 1;
    for (var x = 0; x < QuestionaryList.length; x++) {
        if (QuestionaryList[x].Id === id) {
            return QuestionaryList[x];
        }
    }

    return null;
}

function QuestionaryDuplicate(sender) {
    $("#QuestionaryNewDescriptionErrorRequired").hide();
    $("#QuestionaryNewDescriptionErrorDuplicated").hide();
    QuestionarySelectedId = sender.id * 1;
    QuestionarySelected = QuestionaryGetById(QuestionarySelectedId);
    if (QuestionarySelected === null) { return false; }
    $("#QuestionaryToDuplicateName").html(QuestionarySelected.Description);
    $("#QuestionaryNewDescription").val(ProposeName(QuestionarySelected.Description));
    var dialog = $("#QuestionaryDuplicateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 800,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Questionary_Popup_Duplicate_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        QuestionaryDuplicatedConfirmed();
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

function QuestionaryDuplicatedConfirmed() {
    $("#QuestionaryNewDescriptionErrorRequired").hide();
    $("#QuestionaryNewDescriptionErrorDuplicated").hide();
    if ($("#QuestionaryNewDescription").val() === "") {
        $("#QuestionaryNewDescriptionErrorRequired").show();
        return false;
    }

    if (DuplicatedName($("#QuestionaryNewDescription").val()) === true) {
        $("#QuestionaryNewDescriptionErrorDuplicated").show();
        return false;
    }

    var data = {
        "questionaryId": QuestionarySelectedId,
        "description": $("#QuestionaryNewDescription").val(),
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#QuestionaryDuplicateDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/QuestionaryActions.asmx/Duplicate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            QuestionaryGetFilter();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function ProposeName(original) {
    var x = 2;
    var text = original.toUpperCase();

    while (DuplicatedName(text + " - COPIA " + x)) {
        x++;
    }

    return original + " - COPIA " + x;
}

function DuplicatedName(text) {
    var text = text.toUpperCase();
    for (var x = 0; x < QuestionaryList.length; x++) {
        if (QuestionaryList[x].Description.toUpperCase() === text) {
            return true;
        }
    }
    return false;
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 350);
}

window.onload = function () {
    $("#nav-search").hide();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
    Resize();

    $("#CmbProcess").html("");
    var res = "<option value=\"-1\">" + Dictionary.Common_All + "</option>";
    for (var x = 0; x < ProcessList.length; x++) {
        if (ProcessList[x].Active === true) {
            res += "<option value=\"" + ProcessList[x].Id + "\">" + ProcessList[x].Description + "</option>";
        }
    }
    $("#CmbProcess").html(res);

    $("#CmbRules").html("");
    var res = "<option value=\"-1\">" + Dictionary.Common_All + "</option>";
    for (var x = 0; x < RulesList.length; x++) {
        res += "<option value=\"" + RulesList[x].Id + "\">" + RulesList[x].Description + "</option>";
    }
    $("#CmbRules").html(res);

    $("#CmbProcess").on("change", QuestionaryGetFilter);
    $("#CmbRules").on("change", CmbRulesChange);
    $("#CmbApartadosNorma").on("change", QuestionaryGetFilter);

    //Select2("CmbApartadosNorma");

    ItemRenderTable(QuestionaryList);
    $("#CmbApartadosNorma").attr("disabled", "disabled");
};

window.onresize = function () { Resize(); };

function Export() {
    lockOrderList = true;
    QuestionaryGetFilter("PDF");
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

    var data = {
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

    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/QuestionaryExportList.aspx/PDF",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
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
        "error": function (msg) {
            LoadingHide();
            alertUI("Error:" + msg.responseText);
        }
    });
}

function CmbRulesChange() {
    FillApartadoNorma();
    QuestionaryGetFilter();
}

function FillApartadoNorma() {
    $("#CmbApartadosNorma").html("");
    var RuleId = $("#CmbRules").val() * 1;
    console.log("FillApartadoNorma", RuleId);
    if (RuleId > 0) {
        var res = "<option value=\"-1\">" + Dictionary.Common_All + "</option>";
        for (var x = 0; x < ApartadosNormasList.length; x++) {
            if (ApartadosNormasList[x].R === RuleId) {
                res += "<option value=\"" + ApartadosNormasList[x].A + "\">" + ApartadosNormasList[x].A + "</option>";
            }
        }

        $("#CmbApartadosNorma").removeAttr("disabled");
    }
    else {
        $("#CmbApartadosNorma").attr("disabled", "disabled");
    }

    $("#CmbApartadosNorma").html(res);
}