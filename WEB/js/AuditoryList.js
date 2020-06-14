var lockOrderList = false;
Resize();

var StatusColors = {
    "Planificando": "#6fb3e0",
    "Planificada": "#ff0",
    "EnCurso": "#ffb752",
    "Pendiente": "#d15b47",
    "Cerrada": "#87b87f",
    "Validada": "#555"
};

jQuery(function ($) {
    var options = $.extend({}, $.datepicker.regional[userLanguage], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);

    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
            if ("title_html" in this.options && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));

    $("#BtnSearch").on("click", function (e) {
        e.preventDefault();
        AuditoryGetFilter();
    });

    $("#BtnRecordShowAll").on("click", function (e) {
        e.preventDefault();
        AuditoryListGetAll();
    });
});

function AuditoryListGetAll() {
    var ok = true;
    VoidTable("ListDataTable");
    $("#TxtDateFrom").val("");
    $("#TxtDateTo").val("");
    for (var x = 0; x < 3; x++) { document.getElementById("ChkType" + x).checked = true; document.getElementById("ChkType" + x).disabled = false; }
    for (var y = 0; y < 5; y++) { document.getElementById("ChkStatus" + y).checked = true; document.getElementById("ChkStatus" + y).disabled = false; }
    AuditoryGetFilter();
}

function IncidentListGetNone() {
    $("#BtnRecordShowAll").hide();
    $("#BtnRecordShowNone").show();
    var ok = true;
    VoidTable("ListDataTable");

    $("#CmbProcess").val(-1);
    $("#CmbRules").val(-1);
    $("#CmbApartadosNorma").val(-1);
    AuditoryGetFilter();
}

function AuditoryGetFilter(exportType) {
    var ok = true;
    VoidTable("ListDataTable");
    $("#ItemTableError").hide();
    $("#ItemTableVoid").hide();
    var from = $("#TxtDateFrom").val();
    var to = $("#TxtDateTo").val();

    if (from === "") { from = null; }
    if (to === "") { to = null; }

    var data = {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "status0": document.getElementById("ChkStatus0").checked,
        "status1": document.getElementById("ChkStatus1").checked,
        "status2": document.getElementById("ChkStatus2").checked,
        "status3": document.getElementById("ChkStatus3").checked,
        "status4": document.getElementById("ChkStatus4").checked,
        "status5": document.getElementById("ChkStatus5").checked,
        "interna": document.getElementById("ChkType0").checked,
        "externa": document.getElementById("ChkType1").checked,
        "provider": document.getElementById("ChkType2").checked
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/GetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            eval("AuditoryList=" + msg.d + ";");
            ItemRenderTable(AuditoryList);
            if (typeof exportType !== "undefined" && exportType !== null && exportType === "PDF") {
                ExportPDF();
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function Go(sender) {
    for (var x = 0; x < AuditoryList.length; x++) {
        if (AuditoryList[x].Id === sender.id * 1) {
            if (AuditoryList[x].Type === 1) { document.location = "AuditoryExternaView.aspx?id=" + sender.id; }
            else { document.location = "AuditoryView.aspx?id=" + sender.id; }
        }
    }

    return false;    
}

function ItemRenderTable(list) {
    $("#LoadingData").hide();
    items = new Array();
    var target = document.getElementById("ListDataTable");
    VoidTable("ListDataTable");
    target.style.display = "";
    var total = 0;
    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        var row = document.createElement("TR");
        var tdStatus = document.createElement("TD");
        var tdDescription = document.createElement("TD");
        var tdPlanned = document.createElement("TD");
        var tdValidated = document.createElement("TD");
        var tdAmount = document.createElement("TD");
        row.id = item.Id;
        total += item.Amount;

        var iconStatus = document.createElement("I");
        iconStatus.className = "icon-circle bigger-110";
        switch (item.Status) {
            case 0:
                iconStatus.style.color = StatusColors.Planificando;
                iconStatus.title = Dictionary.Item_Adutory_Status_Label_0;
                break;
            case 1:
                iconStatus.style.color = StatusColors.Planificada;
                iconStatus.title = Dictionary.Item_Adutory_Status_Label_1;
                break;
            case 2:
                iconStatus.style.color = StatusColors.EnCurso;
                iconStatus.title = Dictionary.Item_Adutory_Status_Label_2;
                break;
            case 3:
                iconStatus.style.color = StatusColors.Pendiente;
                iconStatus.title = Dictionary.Item_Adutory_Status_Label_3;
                break;
            case 4:
                iconStatus.style.color = StatusColors.Cerrada;
                iconStatus.title = Dictionary.Item_Adutory_Status_Label_4;
                break;
            case 5:
                iconStatus.style.color = StatusColors.Validada;
                iconStatus.title = Dictionary.Item_Adutory_Status_Label_5;
                break;
        }

        tdStatus.appendChild(iconStatus);

        var AuditoryLink = document.createElement("A");
        AuditoryLink.title = item.Description;
        if (item.Type === 1) {
            AuditoryLink.href = "AuditoryExternaView.aspx?id=" + item.Id;
        }
        else {
            AuditoryLink.href = "AuditoryView.aspx?id=" + item.Id;
        }

        AuditoryLink.appendChild(document.createTextNode(item.Description));
        tdDescription.appendChild(AuditoryLink);

        tdPlanned.appendChild(document.createTextNode(item.PlannedOn));
        tdValidated.appendChild(document.createTextNode(item.ValidatedOn));
        tdAmount.appendChild(document.createTextNode(ToMoneyFormat(item.Amount)));

        tdStatus.style.width = "45px";
        tdStatus.style.textAlign = "center";
        tdPlanned.style.width = "100px";
        tdPlanned.style.textAlign = "center";
        tdValidated.style.width = "100px";
        tdValidated.style.textAlign = "center";
        tdAmount.style.width = "150px";
        tdAmount.style.textAlign = "right";

        row.appendChild(tdStatus);
        row.appendChild(tdDescription);
        row.appendChild(tdPlanned);
        row.appendChild(tdValidated);
        row.appendChild(tdAmount);

        var tdActions = document.createElement("TD");

        var iconEdit = document.createElement("SPAN");
        iconEdit.className = "btn btn-xs btn-info";
        iconEdit.id = item.Id;
        var innerEdit = document.createElement("I");
        innerEdit.className = "icon-edit bigger-120";
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { Go(this); };
        tdActions.appendChild(iconEdit);

        if (ApplicationUser.Grants.Auditory.Delete === true) {
            var iconDelete = document.createElement("SPAN");
            iconDelete.className = "btn btn-xs btn-danger";
            iconDelete.id = item.Id;
            var innerDelete = document.createElement("I");
            innerDelete.className = "icon-trash bigger-120";
            iconDelete.appendChild(innerDelete);
            iconDelete.onclick = function () { AuditoryDelete(this); };
            tdActions.appendChild(document.createTextNode(" "));
            tdActions.appendChild(iconDelete);
        }

        tdActions.style.width = "90px";
        row.appendChild(tdActions);
        target.appendChild(row);

        if ($.inArray(item.Description, items) === -1) {
            items.push(item.Description);
        }
    }

    if (list.length === 0) {
        $("#nav-search").hide();
        $("#ListDataDiv").hide();
        $("#NoData").show();
    }
    else {
        $("#nav-search").show();
        $("#ListDataDiv").show();
        $("#NoData").hide();

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

    $("#TotalList").html(list.length);
    $("#TotalAmount").html(ToMoneyFormat(total));

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

function ShowStatusHelp() {
    $("#StatusHelpDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Auditory_Help_Status_Title + "</h4>",
        "title_html": true,
        "width": 500,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ShowSelectDialog() {
    var dialog = $("#SelectDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Common_Warning + "</h4>",
        "title_html": true,
        "width": 250,
        "buttons":
            [
                {
                    "id": "PopupSelectDialogAccept",
                    "html": Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        $("#ErrorSelect").hide();
                        if (document.getElementById("AuditoryTypeSelect0").checked) {
                            document.location = "AuditoryView.aspx?id=-1&t=0";
                        }
                        else if (document.getElementById("AuditoryTypeSelect1").checked) {
                            document.location = "AuditoryExternaView.aspx?id=-1&t=1";
                        }
                        else if (document.getElementById("AuditoryTypeSelect2").checked) {
                            document.location = "AuditoryView.aspx?id=-1&t=2";
                        }
                        else {
                            $("#ErrorSelect").show();
                        }
                    }
                },
                {
                    "id": "PopupSelectDialogCancel",
                    "html": Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function AuditoryDelete(sender) {
    AuditorySelectedId = sender.id * 1;
    AuditorySelected = AuditoryGetById(AuditorySelectedId);
    if (AuditorySelected === null) { return false; }
    $("#AuditoryDeleteName").html(AuditorySelected.Description);
    $("#AuditoryDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Auditory_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "BtnAuditoryDeleteOK",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        AuditoryDeleteConfirmed();
                    }
                },
                {
                    "id": "BtnAuditoryDeleteCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function AuditoryDeleteConfirmed() {
    var data = {
        "auditoryId": AuditorySelectedId,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#AuditoryDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            AuditoryGetFilter();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AuditoryGetById(id) {
    id = id * 1;
    for (var x = 0; x < AuditoryList.length; x++) {
        if (AuditoryList[x].Id === id) {
            return AuditoryList[x];
        }
    }

    return null;
}

function AuditoryDuplicate(sender) {
    $("#AuditoryNewDescriptionErrorRequired").hide();
    $("#AuditoryNewDescriptionErrorDuplicated").hide();
    AuditorySelectedId = sender.id * 1;
    AuditorySelected = AuditoryGetById(AuditorySelectedId);
    if (AuditorySelected === null) { return false; }
    $("#AuditoryToDuplicateName").html(AuditorySelected.Description);
    $("#AuditoryNewDescription").val(ProposeName(AuditorySelected.Description));
    $("#AuditoryDuplicateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 800,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Auditory_Popup_Duplicate_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "BtnAuditoryDuplicatedOK",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        AuditoryDuplicatedConfirmed();
                    }
                },
                {
                    "id": "BtnAuditoryDUplicatedCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function AuditoryDuplicatedConfirmed() {
    $("#AuditoryNewDescriptionErrorRequired").hide();
    $("#AuditoryNewDescriptionErrorDuplicated").hide();
    if ($("#AuditoryNewDescription").val() === "") {
        $("#AuditoryNewDescriptionErrorRequired").show();
        return false;
    }

    if (DuplicatedName($("#AuditoryNewDescription").val()) === true) {
        $("#AuditoryNewDescriptionErrorDuplicated").show();
        return false;
    }

    var data = {
        "AuditoryId": AuditorySelectedId,
        "description": $("#AuditoryNewDescription").val(),
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#AuditoryDuplicateDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/Duplicate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            AuditoryGetFilter();
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
    while (DuplicatedName(text + " - COPIA " + x)) { x++; }
    return original + " - COPIA " + x;
}

function DuplicatedName(text) {
    var finalText = text.toUpperCase();
    for (var x = 0; x < AuditoryList.length; x++) {
        if (AuditoryList[x].Description.toUpperCase() === finalText) {
            return true;
        }
    }

    return false;
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 410);
    $("#NoData").height(containerHeight - 410);
}

window.onload = function () {
    $("#StatusIcon0").css("color", StatusColors.Planificando);
    $("#StatusIcon1").css("color", StatusColors.Planificada);
    $("#StatusIcon2").css("color", StatusColors.EnCurso);
    $("#StatusIcon3").css("color", StatusColors.Pendiente);
    $("#StatusIcon4").css("color", StatusColors.Cerrada);
    $("#StatusIcon5").css("color", StatusColors.Validada);

    if (document.getElementById("BtnNewItem") !== null) {
        document.getElementById("BtnNewItem").onclick = null;
        $("#BtnNewItem").on("click", ShowSelectDialog);
    }

    $("#nav-search").hide();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
    FilterLayout();
    ChkTypeChanged();
    ChkStatusChanged();
    AuditoryGetFilter();
};

window.onresize = function () { Resize(); };

function FilterLayout() {
    console.log(Filter);
    document.getElementById("ChkType0").checked = Filter.interna;
    document.getElementById("ChkType1").checked = Filter.externa;
    document.getElementById("ChkType2").checked = Filter.provider;
    document.getElementById("ChkStatus0").checked = Filter.status0;
    document.getElementById("ChkStatus1").checked = Filter.status1;
    document.getElementById("ChkStatus2").checked = Filter.status2;
    document.getElementById("ChkStatus3").checked = Filter.status3;
    document.getElementById("ChkStatus4").checked = Filter.status4;
    document.getElementById("ChkStatus5").checked = Filter.status5;
}

function Export() {
    lockOrderList = true;
    AuditoryGetFilter("PDF");
}

function ExportPDF() {
    var from = $("#TxtDateFrom").val();
    var to = $("#TxtDateTo").val();
    var data = {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "status0": document.getElementById("ChkStatus0").checked,
        "status1": document.getElementById("ChkStatus1").checked,
        "status2": document.getElementById("ChkStatus2").checked,
        "status3": document.getElementById("ChkStatus3").checked,
        "status4": document.getElementById("ChkStatus4").checked,
        "status5": document.getElementById("ChkStatus5").checked,
        "interna": document.getElementById("ChkType0").checked,
        "externa": document.getElementById("ChkType1").checked,
        "provider": document.getElementById("ChkType2").checked,
        "filterText": $("#nav-search-input").val(),
        "listOrder": "1ASC"
    };

    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/AuditoryExportList.aspx/PDF",
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
            alertUI("Error:" + msg.responseText);
        }
    });
}

function CmbRulesChange() {
    FillApartadoNorma();
    AuditoryGetFilter();
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

function ChkTypeChanged() {
    var count = 0;
    for (var x = 0; x < 3; x++) {
        if (document.getElementById("ChkType" + x).checked === true) { count++; }
    }

    if (count > 1) {
        for (var y = 0; y < 3; y++) {
            document.getElementById("ChkType" + y).disabled = false;
        }
    }
    else {
        for (var z = 0; z < 3; z++) {
            if (document.getElementById("ChkType" + z).checked === true) {
                document.getElementById("ChkType" + z).disabled = true;
            }
        }
    }

    AuditoryGetFilter();
}

function ChkStatusChanged() {
    var count = 0;
    for (var x = 0; x < 6; x++) {
        if (document.getElementById("ChkStatus" + x).checked === true) { count++; }
    }

    if (count > 1) {
        for (var y = 0; y < 6; y++) {
            document.getElementById("ChkStatus" + y).disabled = false;
        }
    }
    else {
        for (var z = 0; z < 6; z++) {
            if (document.getElementById("ChkStatus" + z).checked === true) {
                document.getElementById("ChkStatus" + z).disabled = true;
            }
        }
    }

    AuditoryGetFilter();
}