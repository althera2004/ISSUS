window.onload = function () {
    $("#navbar").remove();
    $("#menu-toggler").remove();
    $("#sidebar").remove();
    $("#logofooter").remove();
    $("#oldFormFooter").remove();
    $("#nav-search").remove();
    $("#breadcrumbs").remove();
    $("#main-container").css("padding-top", "10px");

    $("h1").parent().after("<h1>" + this.Dictionary.Item_Auditory + ": <strong>" + AuditoryName + "</strong></h1>");
    $("h1").css("padding-top", "12px");
    RenderFounds();
    RenderImprovements();
};

function Toggle(sender) {
    console.log("Toggle", sender.id + " - " + $("#" + sender.id).data("status"));

    var id = sender.id.split('Q')[1] * 1;
    var data = {
        "questionId": id,
        "status": $("#" + sender.id).data("status")
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/QuestionToggle",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                var parts = response.d.MessageError.split('|');

                var text = "-";
                var color = "333";
                if (parts[1] === "1") { text = "Cumple"; color = "070"; }
                else if (parts[1] === "2") { text = "No cumple"; color = "700"; }
                var html = "<span id=\"Q" + parts[0] + "\" style=\"color:#" + color + ";cursor:pointer;\" onclick=\"Toggle(this);\" data-status=\""+ parts[1] +"\">" + text + "</span>";
                $("#Q" + parts[0]).parent().html(html);

            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function foundGetById(id) {
    id = id * 1;
    for (var x = 0; x < Founds.length; x++) {
        if (Founds[x].Id === id) {
            return Founds[x];
        }
    }

    return null;
}

function improvementGetById(id) {
    id = id * 1;
    for (var x = 0; x < Improvements.length; x++) {
        if (Improvements[x].Id === id) {
            return Improvements[x];
        }
    }

    return null;
}

var foundSelectedId = null;
var foundSelected = null;
var improvementSelectedId = null;
var improvementSelected = null;

function ShowPopupFoundDialog(id) {
    foundSelectedId = id;
    foundSelected = foundGetById(id);

    var title = Dictionary.Item_Auditory_Title_PopupUpdateFound;
    if (id < 0) {
        title = Dictionary.Item_Auditory_Title_PopupAddFound;
        $("#TxtText").val("");
        $("#TxtRequeriment").val("");
        $("#TxtUnconformity").val("");
    }
    else {
        $("#TxtText").val(foundSelected.Text);
        $("#TxtRequeriment").val(foundSelected.Requeriment);
        $("#TxtUnconformity").val(foundSelected.Unconformity);
    }

    var dialog = $("#PopupFoundDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": title,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnFoundSaveOK",
                    "html": "<i class=\"icon-save bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        FoundSave();
                    }
                },
                {
                    "Id": "BtnFoundSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ShowPopupImprovementDialog(id) {
    improvementSelectedId = id;
    improvementSelected = improvementGetById(id);

    var title = Dictionary.Item_Auditory_Title_PopupUpdateImprovement;
    if (id < 0) {
        title = Dictionary.Item_Auditory_Title_PopupAddImprovement;
        $("#TxtText").val("");
    }
    else {
        $("#TxtText").val(improvementSelected.Text);
    }

    var dialog = $("#PopupImprovementDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": title,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnImprovementSaveOk",
                    "html": "<i class=\"icon-save bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        ImprovementSave();
                    }
                },
                {
                    "Id": "BtnImprovementSaveCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function RenderFounds() {
    var res = "";
    var count = 0;
    for (var x = 0; x < Founds.length; x++) {
        var found = Founds[x];
        if (found.Active === true) {
            count++;
            res += "<tr id=\"" + found.Id + "\">";
            res += "<td>" + found.Text + "</td>";
            res += "<td style=\"width:200px;\">" + found.Requeriment + "</td>";
            res += "<td style=\"width:200px;\">" + found.Unconformity + "</td>";
            res += "<td class=\"hidden-480\" style=\"width: 90px;white-space:nowrap;\">";
            res += "  <span class=\"btn btn-xs btn-info\" onclick=\"ShowPopupFoundDialog(" + found.Id + "); \">";
            res += "    <i class=\"icon-edit bigger-120\"></i>";
            res += "  </span>";
            res += "  &nbsp;";
            res += "  <span class=\"btn btn-xs btn-danger\" onclick=\"ShowPopupFoundDeleteDialog(" + found.Id + "); \">";
            res += "    <i class=\"icon-trash bigger-120\"></i>";
            res += "  </span>";
            res += "</td>";
            res += "</tr>";
        }
    }

    $("#SpanHallazgosTotal").html(count);
    if (count > 0) {
        $("#HallazgosDataTable").html(res);
        $("#ListDataDivHallazgos").show();
        $("#NoDataHallazgos").hide();
    }
    else {
        $("#HallazgosDataTable").html("");
        $("#ListDataDivHallazgos").hide();
        $("#NoDataHallazgos").show();
    }
}

function RenderImprovements() {
    var res = "";
    var count = 0;
    for (var x = 0; x < Improvements.length; x++) {
        var improvement = Founds[x];
        if (improvement.Active === true) {
            count++;
            res += "<tr id=\"" + improvement.Id + "\">";
            res += "<td>" + improvement.Text + "</td>";
            res += "<td class=\"hidden-480\" style=\"width: 90px;white-space:nowrap;\">";
            res += "  <span class=\"btn btn-xs btn-info\" onclick=\"ShowPopupImprovementDialog(" + improvement.Id + "); \">";
            res += "    <i class=\"icon-edit bigger-120\"></i>";
            res += "  </span>";
            res += "  &nbsp;";
            res += "  <span class=\"btn btn-xs btn-danger\" onclick=\"ShowPopupImprovementDeleteDialog(" + improvement.Id + "); \">";
            res += "    <i class=\"icon-trash bigger-120\"></i>";
            res += "  </span>";
            res += "</td>";
            res += "</tr>";
        }
    }

    $("#SpanMejorasTotal").html(count);
    if (count > 0) {
        $("#MejorasDataTable").html(res);
        $("#MejorasDataTable").show();
        $("#NoDataMejoras").hide();
    }
    else {
        $("#MejorasDataTable").html("");
        $("#MejorasDataTable").hide();
        $("#NoDataMejoras").show();
    }
}

function FoundSave() {
    foundSelected = {
        "Id": foundSelectedId,
        "CompanyId": Company.Id,
        "AuditoryId": AuditoryId,
        "CuestionarioId": QuestionaryId,
        "Text": $("#TxtText").val(),
        "Requeriment": $("#TxtRequeriment").val(),
        "Unconformity": $("#TxtUnconformity").val(),
        "Active": true
    };

    var data = {
        "found": foundSelected,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/FoundSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (foundSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Founds.length; x++) {
                    if (Founds[x].Id === foundSelectedId) {
                        temp.push(foundSelected);
                    }
                    else {
                        temp.push(Founds[x]);
                    }
                }

                Founds = temp;
            }
            else {
                foundSelected.Id = msg.d.MessageError * 1;
                Founds.push(foundSelected);
            }

            $("#PopupFoundDialog").dialog("close");
            RenderFounds();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ImprovementSave() {
    improvementSelected = {
        "Id": foundSelectedId,
        "CompanyId": Company.Id,
        "AuditoryId": AuditoryId,
        "CuestionarioId": QuestionaryId,
        "Text": $("TxtImprovement").val(),
        "Active": true
    };

    var data = {
        "improvement": improvementSelected,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/ImprovementSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (improvementSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Improvements.length; x++) {
                    if (Improvements[x].Id === improvementSelectedId) {
                        temp.push(improvementSelected);
                    }
                    else {
                        temp.push(Improvements[x]);
                    }
                }

                Improvements = temp;
            }
            else {
                improvementSelected.Id = msg.d.MessageError * 1;
                Improvements.push(improvementSelected);
            }

            $("#PopupImprovementDialog").dialog("close");
            RenderImprovements();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ShowPopupFoundDeleteDialog(id) {
    foundSelectedId = id * 1;
    foundSelected = foundGetById(foundSelectedId);
    var dialog = $("#foundDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": title,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnFoundDeleteOk",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        FoundDeleteConfirmed();
                    }
                },
                {
                    "Id": "BtnFoundDeleteCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });

}

function ShowPopupImprovementDeleteDialog(id) {
    improvementSelectedId = id * 1;
    improvementSelected = improvementGetById(improvementSelectedId);
    var dialog = $("#improvementDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": title,
        "title_html": true,
        "width": 600,
        "buttons":
            [
                {
                    "Id": "BtnImprovementDeleteOk",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        ImprovementDeleteConfirmed();
                    }
                },
                {
                    "Id": "BtnImprovementDeleteCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });

}

function FoundDeleteConfirmed() {
    var data = {
        "id": foundSelectedId,
        "companyId": Company.Id,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/FoundDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (foundSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Founds.length; x++) {
                    if (Improvements[x].Id !== foundSelectedId) {
                        temp.push(Improvements[x]);
                    }
                }

                Founds = temp;
            }

            $("#foundDeleteDialog").dialog("close");
            RenderFounds();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ImprovementDeleteConfirmed() {
    var data = {
        "id": improvementSelectedId,
        "companyId": Company.Id,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/AuditoryActions.asmx/ImprovementDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("msg", msg);
            if (improvementSelectedId > 0) {
                var temp = [];
                for (var x = 0; x < Improvements.length; x++) {
                    if (Improvements[x].Id !== improvementSelectedId) {
                        temp.push(Improvements[x]);
                    }
                }

                Improvements = temp;
            }

            $("#improvementDeleteDialog").dialog("close");
            RenderImprovements();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}