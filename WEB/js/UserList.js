function NoDelete() {
    alertUI(Dictionary.Item_Employee_Message_NoDeletableByJobPositionLink);
}

function UserUpdate(id) {
    document.location = 'UserView.aspx?id=' + id;
}

function UserDelete(id, description) {

    if (ApplicationUser.Id === id)
    {
        alertUI(Dictionary.Item_User_Suicide);
        return false;
    }

    document.getElementById('UserName').innerHTML = description;
    var dialog = $("#UserDeleteDialog").removeClass('hide').dialog({
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
                    $(this).dialog("close");
                    EmployeeDeleteConfirmed(id);
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    ClearFieldTextMessages('TxtNewReason');
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function UserDeleteConfirmed(id) {
    var webMethod = "/Async/EmployeeActions.asmx/EmployeeDelete";
    var data = {
        'employeeId': id,
        'companyId': Company.Id,
        'userId': user.Id,
        'reason': ''
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = document.location + '';
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

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
});

function EmployeeDeleteConfirmed(id) {
    var webMethod = "/Async/EmployeeActions.asmx/UserDelete";
    var data = {
        'userItemId': id,
        'companyId': Company.Id,
        'userId': user.Id,
        'reason': '' //document.getElementById('TxtNewReason').value
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = document.location + '';
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 310) + 'px';
}

window.onload = function () { Resize(); }
window.onresize = function () { Resize(); }