function FilterChanged() {
    /*
    $("#Chk1").removeAttr("disabled");
    $("#Chk2").removeAttr("disabled");
    $(".processActive").hide();
    $(".processInactive").hide();
    var filter = "";
    var total = 0;
    if (document.getElementById("Chk1").checked === true && document.getElementById("Chk2").checked === false) {
        $("#Chk1").attr("disabled", "disabled");
    }

    if (document.getElementById("Chk2").checked === true && document.getElementById("Chk1").checked === false) {
        $("#Chk2").attr("disabled", "disabled");
    }

    if (document.getElementById("Chk1").checked === true) {
        $(".processActive").show();
        total += $(".processActive").length;
        filter += "A";
    }

    var data = { "filter": filter };
    if (document.getElementById("Chk2").checked === true) {
        $(".processInactive").show();
        total += $(".processInactive").length;
        filter += "I";
    }

    $("#TotalList").html(total);

    if (total > 0) {
        $("#NoData").hide();
        $("#TableData").show();
    } else {
        $("#NoData").show();
        $("#TableData").hide();}

    $.ajax({
        "type": "POST",
        "url": "/Async/ProcessActions.asmx/SetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
        },
        "error": function (msg) {
            console.log(msg);
        }
    });*/
}

function ProcessNoDelete() {
    alertUI(Dictionary.Common_Warning_Undelete, null);
}

function ProcessDelete(sender) {
    $("#ProcessName").html(sender.parentNode.parentNode.childNodes[0].childNodes[0].innerHTML);
    var Selected = sender.parentNode.parentNode.id * 1;
    $("#ProcessDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Process_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "ProcessDeleteBtnOk",
                    "html": "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                        ProcessDeleteConfirmed(Selected);
                    }
                },
                {
                    "id": "ProcessDeleteBtnCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function ProcessDeleteConfirmed(id) {
    var data = {
        "companyId": Company.Id,
        "processId": id,
        "userId": user.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/ProcessActions.asmx/DeleteProcess",
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
        "error": function (jqXHR) {
            alert(jqXHR.responseText);
        }
    });
}

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

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 310);
}

window.onload = function () {
    // if (filter.indexOf("A") !== -1) { document.getElementById("Chk1").checked = true; }
    // if (filter.indexOf("I") !== -1) { document.getElementById("Chk2").checked = true; }
    // FilterChanged();
    Resize();
    if (!$("#th0").hasClass("ASC")) {
        $("#th0").click();
        $("#th0").click();
    }
};

window.onresize = function () { Resize(); };