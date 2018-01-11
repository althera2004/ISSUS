var selectedRows = new Array();

function LearningUpdate(id) {
    document.location = "FormacionView.aspx?id=" + id;
}

function LearningDeleteDisabled() {
    alert(Dictionary.Common_ErrorMessage_CanNotDelete);
}

function selectRow(sender) {
    var id = sender.id.split("|")[0];
    var learningId = sender.id.split("|")[1];
    var employeeId = sender.id.split("|")[2];

    var selectedRowsTemp = new Array();
    var passed = false;

    for (var x = 0; x < selectedRows.length; x++) {
        if (selectedRows[x].AssistantId !== id) {
            selectedRowsTemp.push(selectedRows[x]);
        }
        else {
            if (sender.checked) {
                selectedRowsTemp.push(selectedRows[x]);
                passed = true;
            }
        }
    }

    if (sender.checked && !passed) {
        selectedRowsTemp.push({ AssistantId: id, LearningId: learningId, EmployeeId: employeeId });
    }

    selectedRows = selectedRowsTemp;
}

function Go(param, value) {
    var yearFrom = document.getElementById("CmbYearFrom").value;
    var yearTo = document.getElementById("CmbYearTo").value;
    var mode = 3;
    if (document.getElementById("Contentholder1_status0").checked) mode = 0;
    if (document.getElementById("Contentholder1_status1").checked) mode = 1;
    if (document.getElementById("Contentholder1_status2").checked) mode = 2;
    if (yearTo > 0) {
        if (yearFrom > yearTo) {
            alert("El año de origen no puede ser superior al año final");
            window.scrollTo(0, 0);
            return false;
        }
    }

    document.location = "FormacionList.aspx?mode=" + mode + "&yearFrom=" + yearFrom + "&yearTo=" + yearTo;
}

function LearningDelete(id, description) {
    $("#LearningName").html(description);
    var dialog = $("#LearningDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 500,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        "buttons": [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    var ok = true;
                    //if(!RequiredFieldText('TxtNewReason')) { ok = false; }
                    if (ok === false) {
                        window.scrollTo(0, 0);
                        return false;
                    }

                    $(this).dialog("close");
                    LearningDeleteConfirmed(id);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () {
                    ClearFieldTextMessages("TxtNewReason");
                    $(this).dialog("close");
                }
            }
        ]

    });
}

function LearningDeleteConfirmed(id) {
    var webMethod = "/Async/LearningActions.asmx/Delete";
    var data = {
        "learningId": id,
        "companyId": Company.Id,
        "userId": user.Id,
        "reason": "" //document.getElementById('TxtNewReason').value
    };

    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                document.location = document.location + "";
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
}

jQuery(function ($) {
    //override dialog's title function to allow for HTML titles
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

    $("#BtnCompleted").on("click", function (e) {
        var data = {
            "companyId": Company.Id,
            "assistants": selectedRows,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/LearningActions.asmx/Complete",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (response) {
                if (response.d.Success === true) {
                    document.location = document.location + "";
                }

                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR.responseText);
            }
        });
    });

    $("#BtnSuccess").on("click", function (e) {
        var data = {
            "companyId": Company.Id,
            "assistants": selectedRows,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/LearningActions.asmx/Success",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (response) {
                if (response.d.Success === true) {
                    document.location = document.location + "";
                }

                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR.responseText);
            }
        });
    });

    $("#BtnDelete").on("click", function (e) {
        var data = {
            "companyId": Company.Id,
            "assistants": selectedRows,
            "userId": user.Id
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/LearningActions.asmx/Delete",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (response) {
                if (response.d.Success === true) {
                    document.location = document.location + '';
                }
                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR.responseText);
            }
        });
    });
});

if (typeof user.Grants.Learning === "undefined" || user.Grants.Learning.Write === false) {
    $(".icon-edit").addClass("icon-eye-open");
}

if (typeof user.Grants.Learning === "undefined" || user.Grants.Learning.Delete === false) {
    $(".btn-danger").hide();
}

function Export() {
    var status = 3;
    if (document.getElementById("Contentholder1_status0").checked === true) { status = 0; }
    if (document.getElementById("Contentholder1_status1").checked === true) { status = 1; }
    if (document.getElementById("Contentholder1_status2").checked === true) { status = 2; }
    var data =
        {
            "companyId": Company.Id,
            "yearFrom": $("#CmbYearFrom").val(),
            "yearTo": $("#CmbYearTo").val(),
            "mode": status,
            "listOrder": listOrder
        };

    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        "type": "POST",
        "url": "/Export/FormacionExportList.aspx/PDF",
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

window.onload = function () {
    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;");
    $(".page-header .col-sm-8").addClass("col-sm-6");
    $(".page-header .col-sm-8").removeClass("col-sm-8");
    $(".page-header .col-sm-4").addClass("col-sm-6");
    $(".page-header .col-sm-4").removeClass("col-sm-4");
    $("#th0").click();
};