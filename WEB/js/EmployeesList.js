var EmployeeDeleteId;

function NoDelete() {
    alertUI(Dictionary.Item_Employee_Message_NoDeletableByJobPositionLink);
}

function EmployeeUpdate(id) {
    document.location = 'EmployeesView.aspx?id=' + id;
}

function GoDepartment(id)
{
    document.location = 'DepartmentView.aspx?id=' + id;
}

function EmployeeDeleteAlertNo() {
    return false;
}

function EmployeeDeleteAlertYes() {
    document.location = 'EmployeeSubstitution.aspx?id=' + EmployeeDeleteId;
}

function EmployeeDeleteAlert(id, description) {
    if (id === ApplicationUser.Employee.Id) {
        warningInfoUI(Dictionary.Item_Employee_Error_AutoDelete, null, 300);
        return false;
    }
    EmployeeDeleteId = id;
    promptInfoUI(Dictionary.Item_Employee_Message_Delete, 300, EmployeeDeleteAlertYes, EmployeeDeleteAlertNo);
}

function EmployeeDelete(id, description) {
    if (id === ApplicationUser.Employee.Id)
    {
        warningInfoUI(Dictionary.Item_Employee_Error_AutoDelete, null, 300);
        return false;
    }

    document.getElementById('EmployeeName').innerHTML = description;
    var dialog = $("#EmployeeDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Employee_Popup_Delete_Title + '</h4>',
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

function EmployeeDeleteConfirmed(id)
{
    var webMethod = "/Async/EmployeeActions.asmx/EmployeeDelete";
    var data = {
        'employeeId': id,
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
            
function Restore(employeeId)
{
    var webMethod = "/Async/EmployeeActions.asmx/Restore";
    var data = {
        'employeeId': employeeId,
        'companyId': Company.Id,
        'userId': user.Id
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
            if (("title_html" in this.options) && this.options.title_html === true)
            {
                title.html($title);
            }
            else{ title.text($title);
            }
        }
    }));
                
    $('#SelectorTabActive').on('click', function (e) { document.getElementById('BtnNewItem').style.visibility = 'visible'; });
    $('#SelectorTabInactive').on('click', function (e) { document.getElementById('BtnNewItem').style.visibility = 'hidden'; });
});

function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var listTable2 = document.getElementById('ListDataDiv2');
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 380) + 'px';
    listTable2.style.height = (containerHeight - 380) + 'px';
}

window.onload = function () {
    Resize();
    $("#th0").click();
}
window.onresize = function () { Resize(); }