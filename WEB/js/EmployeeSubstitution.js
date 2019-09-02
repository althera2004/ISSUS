var data = "";

function RenderTable()
{
    VoidTable("TableEmployeeElements");
    for(var x=0;x<asignations.length;x++)
    {
        RenderRow(asignations[x]);
        data += asignations[x].AssignationType + "-" + asignations[x].ItemId + "|";
    }
}

function RenderRow(asignation)
{
    var target = document.getElementById("TableEmployeeElements");
    var tr = document.createElement("TR");
    var td1 = document.createElement("TD");
    var label = "";
    switch (asignation.AssignationType)
    {
        case "E": label = Dictionary.Item_Employee_Delete_Item_Equipment; break;
        case "ECDI": label = Dictionary.Item_Employee_Delete_Item_CalibrationInternal; break;
        case "ECDE": label = Dictionary.Item_Employee_Delete_Item_CalibrationExternal; break;
        case "EVDI": label = Dictionary.Item_Employee_Delete_Item_VerificationInternal; break;
        case "EVDE": label = Dictionary.Item_Employee_Delete_Item_VerificationExternal; break;
        case "EMDI": label = Dictionary.Item_Employee_Delete_Item_MaintenanceInternal; break;
        case "EMDE": label = Dictionary.Item_Employee_Delete_Item_MaintenanceExternal; break;
        case "IAE": label = Dictionary.Item_Employee_Delete_Item_IncidentActionExecutor; break;
        default: label = ""; break;
    }

    td1.appendChild(document.createTextNode(label));

    var td2 = document.createElement("TD");
    td2.appendChild(document.createTextNode(asignation.Description));

    var td3 = document.createElement("TD");
    td3.appendChild(RenderCmbSubstitute(asignation.AssignationType, asignation.ItemId));

    tr.appendChild(td1);
    tr.appendChild(td2);

    // ISSUS-101
    tr.appendChild(td3);
    target.appendChild(tr);
}

function RenderCmbSubstitute(itemtype, itemid)
{
    var target = document.createElement("SELECT");
    target.id = itemtype + "-" + itemid;
    target.className = "SelectSubstitute";

    var optionDefault = document.createElement("OPTION");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);

    for(var x=0;x<Company.Employees.length;x++)
    {
        if (Company.Employees[x].Id !== employeeId && Company.Employees[x].Active === true && Company.Employees[x].DisabledDate === null) {
            var option = document.createElement("OPTION");
            option.value = Company.Employees[x].Id;
            option.appendChild(document.createTextNode(Company.Employees[x].Name + " " + Company.Employees[x].LastName));
            target.appendChild(option);
        }
    }

    return target;
}

function ChangeAll() {
    var value = document.getElementById("All").value;
    var target = document.getElementById("TableEmployeeElements");
    for (var x = 0; x < target.childNodes.length; x++) {
        var row = target.childNodes[x];
        var select = row.lastChild.firstChild;
        select.value = value;
    }
}

function FillCmbAll()
{
    var target = document.getElementById("All");
    var optionDefault = document.createElement("OPTION");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);
    for (var x = 0; x < Company.Employees.length; x++) {
        if (Company.Employees[x].Id !== employeeId && Company.Employees[x].Active === true && Company.Employees[x].DisabledDate === null) {
            var option = document.createElement("OPTION");
            option.value = Company.Employees[x].Id;
            option.appendChild(document.createTextNode(Company.Employees[x].Name + " " + Company.Employees[x].LastName));
            target.appendChild(option);
        }
    }
}

$("#BtnSave").on("click", SaveEmployee);
$("#BtnCancel").on("click", Cancel);

function Cancel() {
    var location = document.location + "";
    if (location.indexOf('&New=true') != -1) {
        
    }
    else {
        document.location = referrer;
    }
}

function SaveEmployee() {
    var ok = true;
    console.log("SaveEmployee");

    var ok = true;
    var errorMessage = "";
    if ($("#TxtEndDate").val() === "") {
        ok = false;
        errorMessage = $("#TxtEndDateDateRequired").html();
        errorMessage += "<br />";
    }
    else {
        if (!validateDate($("#TxtEndDate").val())) {
            ok = false;
            errorMessage = $("#TxtEndDateDateMalformed").html();
            errorMessage += "<br />";
        }
    }

    var okSubst = true;
    $(".SelectSubstitute").each(function () {
        var $this = $(this);
        $(this).css("background-color", "transparent");
        var res = $(this).attr("id") + "|" + $(this).val() + "#";
        if ($(this).val() * 1 < 1) {
            okSubst = false;
            $(this).css("background-color", "#fdd");
        }
        $("#Subst").html($("#Subst").html() + res);
    });

    if (okSubst === false) {
        errorMessage += Dictionary.Item_Employee_List_Delete_Error;
    }

    if (ok === false) {
        warningInfoUI(errorMessage, null, 300);
        return false;
    }

    var webMethod = "/Async/EmployeeActions.asmx/Substitute";
    var data = {
        "endDate": GetDate($("#TxtEndDate").val(), "/", false),
        "userId": user.Id,
        "companyId": companyId,
        "actualEmployee": employeeId,
        "substitutions": $("#Subst").val()
    };

	if(action === "delete"){
		webMethod = "/Async/EmployeeActions.asmx/Substitute";
		data = {
			"employeeId": employeeId,
			"companyId": companyId,
			"reason": "",
			"userId": user.Id
		};
	}

    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = "EmployeesList.aspx";
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

FillCmbAll();
RenderTable();

$("select").on("change", function (e) {
    console.log(e.target.value);
    WarningEmployeeNoUserCheck(e.target.value * 1, Employees);
});