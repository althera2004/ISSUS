var ObjetivosList = null;
var filterData = null;

function Export() {
    ObjetivoGetFilter("PDF");
}

function ObjetivoGetFilter(exportType) {
    document.getElementById("nav-search-input").value = "";
    var ok = true;
    document.getElementById("ListDataTable").style.display = "none";
    document.getElementById("ItemTableError").style.display = "none";
    document.getElementById("ItemTableVoid").style.display = "none";
    document.getElementById("ErrorDate").style.display = "none";
    document.getElementById("ErrorStatus").style.display = "none";
    document.getElementById("ErrorType").style.display = "none";
    var from = GetDate($("#TxtDateFrom").val(), "-");
    var to = GetDate($("#TxtDateTo").val(), "-");

    if (from !== null && to !== null) {
        if (from > to) {
            ok = false;
            document.getElementById("ErrorDate").style.display = "";
        }
    }

    if (ok === false) {
        document.getElementById("ItemTableError").style.display = "";
        return false;
    }

    var status = 0;
    if (document.getElementById("RBStatus1").checked === true) { status = 1; }
    if (document.getElementById("RBStatus2").checked === true) { status = 2; }

    filterData =
    {
        "companyId": Company.Id,
        "from": from,
        "to": to,
        "status": status
        };

    console.log(filterData);

    $.ajax({
        type: "POST",
        url: "/Async/ObjetivoActions.asmx/GetFilter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(filterData, null, 2),
        success: function (msg) {
            eval("ObjetivosList=" + msg.d + ";");
            console.log(msg.d);
            ItemRenderTable(ObjetivosList);
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

function ItemRenderTable(list) {
    var items = new Array();
    var target = document.getElementById('ListDataTable');
    VoidTable('ListDataTable');
    target.style.display = '';

    if (list.length === 0) {
        document.getElementById('ItemTableVoid').style.display = '';
        $("#NumberCosts").html("0");
        target.style.display = 'none';
        return false;
    }

    var total = 0;

    for (var x = 0; x < list.length; x++) {
        var item = list[x];
        console.log("Item", item);
        var row = document.createElement("TR");
        row.id = item.Id;
        var tdObjetivo = document.createElement("TD");
        var tdObjetivoResponsible = document.createElement("TD");
        var tdStartDate = document.createElement("TD");
        var tdPreviewEndDate = document.createElement("TD");

        if (item.EndDate !== null) {
            row.style.fontStyle = "italic";
        }

        // ---- OBJETIVO
        var objetivoLink = document.createElement('A');
        objetivoLink.href = 'ObjetivoView.aspx?id=' + item.Id;
        objetivoLink.appendChild(document.createTextNode(item.Name));
        tdObjetivo.appendChild(objetivoLink);

        // ---- STARTDATE
        tdStartDate.appendChild(document.createTextNode(FormatYYYYMMDD(item.StartDate, "/")));

        // ---- PREVIEWENDDATE
        if (item.EndDate !== null) {
            tdPreviewEndDate.appendChild(document.createTextNode(FormatYYYYMMDD(item.EndDate, "/")));
            tdPreviewEndDate.style.fontWeight = "bold";
        }
        else {
            tdPreviewEndDate.appendChild(document.createTextNode(FormatYYYYMMDD(item.PreviewEndDate, "/")));
        }

        // ---- RESPONSABLE OBJETIVO
        tdObjetivoResponsible.appendChild(document.createTextNode(item.ResponsibleName));


        tdObjetivoResponsible.style.width = "200px";
        tdStartDate.style.width = "100px";
        tdPreviewEndDate.style.width = "100px";

        row.appendChild(tdObjetivo);
        row.appendChild(tdObjetivoResponsible);
        row.appendChild(tdStartDate);
        row.appendChild(tdPreviewEndDate);

        var iconEdit = document.createElement('SPAN');
        iconEdit.className = 'btn btn-xs btn-info';
        iconEdit.id = item.Number;
        var innerEdit = document.createElement('I');
        innerEdit.className = ApplicationUser.Grants.Indicador.Write ? 'icon-edit bigger-120' : 'icon-eye-open bigger-120';
        iconEdit.appendChild(innerEdit);
        iconEdit.onclick = function () { document.location = 'ObjetivoView.aspx?id=' + this.parentNode.parentNode.id; };

        if (ApplicationUser.Grants.Indicador.Delete === true) {
            var iconDelete = document.createElement('SPAN');
            iconDelete.className = 'btn btn-xs btn-danger';
            iconDelete.id = item.Number;
            var innerDelete = document.createElement('I');
            innerDelete.className = 'icon-trash bigger-120';
            iconDelete.appendChild(innerDelete);

            if (item.Origin === 3) {
                iconDelete.onclick = function () { NoDeleteObjetivo(); };
            }
            else if (item.Origin === 4) {
                iconDelete.onclick = function () { NoDeleteObjetivo(); };
            }
            else {
                iconDelete.onclick = function () { ObjetivoDelete(this); };
            }
        }


        var tdActions = document.createElement('TD');
        tdActions.style.width = "90px";

        tdActions.appendChild(iconEdit);
        if (ApplicationUser.Grants.Indicador.Delete) {
            tdActions.appendChild(document.createTextNode(' '));
            tdActions.appendChild(iconDelete);
        }
        row.appendChild(tdActions);

        target.appendChild(row);

        if ($.inArray(item.Name, items) === -1) {
            items.push(item.Name);
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
    $("#th1").click();
    if (document.getElementById("th1").className.indexOf("DESC") !== -1) {
        $("#th1").click();
    }
}

function ObjetivoDelete(sender) {
    ObjetivoSelectedId = sender.parentNode.parentNode.id;
    ObjetivoSelected = ObjetivoGetById(ObjetivoSelectedId);
    if (ObjetivoSelected === null) { return false; }
    $("#ObjetivoDeleteName").html(ObjetivoSelected.Name);
    var dialog = $("#ObjetivoDeleteDialog").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Common_Delete,
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    ObjetivoDeleteConfirmed();
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

function ObjetivoDeleteConfirmed() {
    var webMethod = "/Async/ObjetivoActions.asmx/Inactivate";
    var data = {
        objetivoId: ObjetivoSelectedId,
        companyId: Company.Id,
        applicationUserId: user.Id
    };
    $("#ObjetivoDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            ObjetivoGetFilter();
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function ObjetivoGetById(id) {
    for (var x = 0; x < ObjetivosList.length; x++) {
        if (ObjetivosList[x].Id === id) {
            return ObjetivosList[x];
        }
    }
    return null;
}

function NoDeleteObjetivo() {
    alertInfoUI(Dictionary.Item_IncidentAction_ErrorMessage_NoDeleteIncident, null);
}

$("#nav-search").hide();

function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 390) + 'px';
}

window.onload = function () {
    // Descomentar si se imprimie lista
    Resize();
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export();\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;")
}

window.onresize = function () { Resize(); }

function ExportPDF() {
    console.clear();
    console.log(filterData);
    var webMethod = "/Export/ObjetivoExport.aspx/PDF";
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(filterData, null, 2),
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

