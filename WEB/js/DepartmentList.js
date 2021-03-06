﻿var DepartmentSelected;
function DepartmentDeleteAction() {
    var data = {
        "departmentId": DepartmentSelected,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#DepartmentDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DepartmentActions.asmx/DepartmentDelete",
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

function DepartmentDelete(id, name) {
    $("#DepartmentName").html(name);
    DepartmentSelected = id;
    $("#DepartmentDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Department_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        DepartmentDeleteAction();
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

function DepartmentUpdate(id) {
    document.location = 'DepartmentView.aspx?id=' + id;
    return false;
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

    $("#BtnNewDepartment").on("click", function () {
        document.location = "DepartmentView.aspx?id=-1";
        return false;
    });
});

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 310);
}

window.onload = function () { Resize(); };
window.onresize = function () { Resize(); };