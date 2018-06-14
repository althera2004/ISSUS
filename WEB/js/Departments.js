$(document).ready(function () {
    RenderTable();

    document.getElementById('TxtName').focus();
});

function RenderTable() {
    var target = document.getElementById('TableBody');
    if (target === null) {
        return;
    }

    for (var x = 0; x < Departments.length; x++) {
        var tr = document.createElement('tr');
        var td = document.createElement('td');
        var department = Departments[x];
        tr.id = department.Id;
        td.appendChild(document.createTextNode(department.Description));
        tr.appendChild(td);
        target.appendChild(tr);
    }
}

 function JobPositionUpdate(id) {
     document.location = 'CargosView.aspx?id=' + id;
 }

 function EmployeeUpdate(id) {
     document.location = 'EmployeesView.aspx?id=' + id;
 }

 function NoDelete() {
     alert(Dictionary.Common_CanNotDelete);
 }

 function DepartmentDesassociationConfirmed(id) {
     var webMethod = "/Async/EmployeeActions.asmx/DesassociateDepartment";
     var data = { employeeId: id, companyId: Company.Id, departmentId: departmentId};
     $("#DepartmentDesassociationDialog").dialog("close");
     $.ajax({
         type: "POST",
         url: webMethod,
         contentType: "application/json; charset=utf-8",
         dataType: "json",
         data: JSON.stringify(data, null, 2),
         success: function (msg) {
             document.location = document.location + '';
         },
         error: function (msg) {
             alertUI(msg.responseText);
         }
     });
 }

 function Save() {
    var ok = true;
    if (!RequiredFieldText('TxtName')) { ok = false; }
    else
    {
        var duplicated = false;
        for (var x = 0; x < departments.length; x++) {
            var description = departments[x].Description.toLowerCase();
            if (description == document.getElementById('TxtName').value.toLowerCase() && departments[x].Id != departmentId) {
                duplicated = true;
                break;
            }
        }

        if (duplicated === true) {
            document.getElementById('TxtNameLabel').style.color = '#f00';
            document.getElementById('TxtNameErrorDuplicated').style.display = 'block';
            ok = false;
        }
        else {
            document.getElementById('TxtNameLabel').style.color = '#000';
            document.getElementById('TxtNameErrorDuplicated').style.display = 'none';
        }
    }

    if (ok === false) {
        window.scrollTo(0, 0);
        return false;
    }
    else {
        var webMethod = "/Async/DepartmentActions.asmx/DepartmentUpdate";
        var data = {
            'departmentId': departmentId,
            'name': document.getElementById('TxtName').value,
            'companyId': Company.Id,
            'userId': user.Id
        };

        if (departmentId === -1) {
            webMethod = "/Async/DepartmentActions.asmx/DepartmentInsert";
            data = {
                'name': document.getElementById('TxtName').value,
                'companyId': Company.Id,
                'userId': user.Id
            };
        }

        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (response) {
                LoadingHide();
                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
                else {
                    document.location = referrer;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                LoadingHide();
                alertUI(jqXHR.responseText);
            }
        });
    }
}

function EmployeeDelete(id, name) {
    $('#DepartmentDesassociationText').html(name);
    DepartmentSelected = id;
    var dialog = $("#DepartmentDesassociationDialog").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: Item_Employee_Popup_UnlinkJobPosition_Message,
        title_html: true,
        buttons: [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    DepartmentDesassociationConfirmed(id);
                }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

jQuery(function ($) {
    $('#BtnSave').click(Save);
    $('#BtnCancel').click(function (e) {
        document.location = referrer;
    });

    //override dialog's title function to allow for HTML titles
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

    if (ApplicationUser.ShowHelp === true) {
        SetToolTip('TxtName', Dictionary.Item_Department_Help_Field_Name);
        $('[data-rel=tooltip]').tooltip();
    }
});

if (typeof ApplicationUser.Grants.Department === "undefined" || ApplicationUser.Grants.Department.Write === false) {
    $(".btn-danger").hide();
    $("input").attr("disabled", true);
    $("textarea").attr("disabled", true);
    $("select").attr("disabled", true);
    $("select").css("background-color", "#eee");
    $("#BtnSave").hide();
}

if (typeof ApplicationUser.Grants.JobPosition === "undefined" || ApplicationUser.Grants.JobPosition.Write === false) {
    $(".icon-edit").addClass("icon-eye-open");
    $(".icon-edit").removeClass("icon-edit");
}