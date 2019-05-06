var EmployeeDeleteId;

function NoDelete() {
    alertUI(Dictionary.Item_Employee_Message_NoDeletableByJobPositionLink);
}

function EmployeeUpdate(id) {
    document.location = "EmployeesView.aspx?id=" + id;
}

function GoDepartment(id)
{
    document.location = "DepartmentView.aspx?id=" + id;
}

function EmployeeDeleteAlertNo() {
    return false;
}

function EmployeeDeleteAlertYes() {
    document.location = "EmployeeSubstitution.aspx?id=" + EmployeeDeleteId;
}

function EmployeeDeleteAlert(id, description) {
    if (id === ApplicationUser.Employee.Id) {
        warningInfoUI(Dictionary.Item_Employee_Error_AutoDelete, null, 300);
        return false;
    }

    EmployeeDeleteId = id;
    promptInfoUI(Dictionary.Item_Employee_Message_Delete, 300, EmployeeDeleteAlertYes, EmployeeDeleteAlertNo);
    return false;
}

function EmployeeSubstitutionYes() {
	document.location = "EmployeeSubstitution.aspx?id=" + EmployeeDeleteId + "&enddate=null&action=delete";
}

function EmployeeSubstitutionNo() {
	return false;
}

function EmployeeDelete(id, description, hasAction) {
    if (id === ApplicationUser.Employee.Id)
    {
        warningInfoUI(Dictionary.Item_Employee_Error_AutoDelete, null, 400);
        return false;
    }

    if (hasAction === true) {
		EmployeeDeleteId = id;
		promptInfoUI(Dictionary.Item_Employee_Message_Delete, 400, EmployeeSubstitutionYes, EmployeeSubstitutionNo);
        return false;
    }

    document.getElementById("EmployeeName").innerHTML = description;
    var dialog = $("#EmployeeDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
		"width": 400,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Employee_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    $(this).dialog("close");
                    EmployeeDeleteConfirmed(id);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () {
                    ClearFieldTextMessages('TxtNewReason');
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function EmployeeDeleteConfirmed(id)
{
    var data = {
        "employeeId": id,
        "companyId": Company.Id,
        "userId": user.Id,
        "reason": ""
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EmployeeActions.asmx/EmployeeDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = document.location + "";
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
            
function Restore(employeeId)
{
    var data = {
        "employeeId": employeeId,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EmployeeActions.asmx/Restore",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = document.location + "";
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true)
            {
                title.html($title);
            }
            else{ title.text($title);
            }
        }
    }));
                
    $("#SelectorTabActive").on("click", function (e) { document.getElementById("BtnNewItem").style.visibility = "visible"; });
    $("#SelectorTabInactive").on("click", function (e) { document.getElementById("BtnNewItem").style.visibility = "hidden"; });
});

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 360);
}

window.onload = function () {
    Resize();
    if (Filter.indexOf("A") !== -1) { document.getElementById("Chk1").checked = true; }
    if (Filter.indexOf("I") !== -1) { document.getElementById("Chk2").checked = true; }
    RenderEmployeeTable();
    $("#th0").click();
};

window.onresize = function () { Resize(); };

function RenderEmployeeRow(employee) {
    var style = "";
    if (employee.Baja === true) {
        style = " style=\"font-style: italic;\"";
    }
    var res = "<tr>";
    res += "  <td" + style + ">";
    res += "    "+ employee.Link;
    res += "  </td>";
    res += "  <td style=\"width: 300px;\">" + employee.Cargos +"</td>";
    res += "  <td style=\"width: 300px;\">" + employee.Departamentos +"</td>";
    res += "  <td style=\"width: 90px;\">";
    res += "    <span title=\"Editar " + employee.FullName + "\" class=\"btn btn-xs btn-info\" onclick=\"EmployeeUpdate(" + employee.Id +", 'Editar'); \">";
    res += "      <i class=\"icon-edit bigger-120\"></i>";
    res += "    </span>";
    res += "    <span title=\"Eliminar " + employee.FullName + "\" class=\"btn btn-xs btn-danger\" onclick=\"EmployeeDelete(" + employee.Id + ", '" + employee.FullName + "'," + employee.HasActions + "); \">";
    res += "      <i class=\"icon-trash bigger-120\"></i>";
    res += "    </span>";
    res += "  </td>";
    res += "</tr>";
    return res;
}

function RenderEmployeeTable() {
    $("#ListDataTable").html("");
    var res = "";
    var count = 0;
    for (var x = 0; x < employees.length; x++) {
        var show = false;
        if (Filter.indexOf("A") !== -1 && employees[x].Baja === false) { show = true; }
        if (Filter.indexOf("I") !== -1 && employees[x].Baja === true) { show = true; }
        if (show === true) {
            res += RenderEmployeeRow(employees[x]);
            count++;
        }
    }

    $("#ListDataTable").html(res);
    $("#TotalRecords").html(count);
}

function FilterChanged() {
    $("#Chk1").removeAttr("disabled");
    $("#Chk2").removeAttr("disabled");
    var count = document.getElementById("Chk1").checked === true ? 1 : 0;
    count += document.getElementById("Chk2").checked === true ? 1 : 0;

    if (count === 1) {
        if (document.getElementById("Chk1").checked === true) { $("#Chk1").attr("disabled", "disabled"); }
        if (document.getElementById("Chk2").checked === true) { $("#Chk2").attr("disabled", "disabled"); }
    }

    SetFilter();
    RenderEmployeeTable();
}

function SetFilter() {
    Filter = "";
    if (document.getElementById("Chk1").checked === true) { Filter += "A"; }
    if (document.getElementById("Chk2").checked === true) { Filter += "I"; }

    var data = { "filter": Filter };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EmployeeActions.asmx/SetFilter",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("SetFilter", "OK");
        },
        "error": function (msg) {
            console.log("SetFilter", msg.responseText);
        }
    });
}