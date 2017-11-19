var data = '';

function RenderTable()
{
    VoidTable('TableEmployeeElements');
    for(var x=0;x<asignations.length;x++)
    {
        RenderRow(asignations[x]);
        data += asignations[x].AssignationType + '-' + asignations[x].ItemId + '|';
    }

    document.getElementById('TxtData').value = data;
}

function RenderRow(asignation)
{
    var target = document.getElementById('TableEmployeeElements');
    var tr = document.createElement('TR');
    var td1 = document.createElement('TD');
    var label = '';
    switch (asignation.AssignationType)
    {
        case 'E': label = Dictionary.Item_Employee_Delete_Item_Equipment; break;
        case 'ECDI': label = Dictionary.Item_Employee_Delete_Item_CalibrationInternal; break;
        case 'ECDE': label = Dictionary.Item_Employee_Delete_Item_CalibrationExternal; break;
        case 'EVDI': label = Dictionary.Item_Employee_Delete_Item_VerificationInternal; break;
        case 'EVDE': label = Dictionary.Item_Employee_Delete_Item_VerificationExternal; break;
        case 'EMDE': label = Dictionary.Item_Employee_Delete_Item_MaintenanceInternal; break;
        case 'EMDI': label = Dictionary.Item_Employee_Delete_Item_MaintenanceExternal; break;
        case 'IAE': label = Dictionary.Item_Employee_Delete_Item_IncidentActionExecutor; break;
        default: label = ''; break;
    }

    td1.appendChild(document.createTextNode(label));

    var td2 = document.createElement('TD');
    td2.appendChild(document.createTextNode(asignation.Description));

    var td3 = document.createElement('TD');
    td3.appendChild(RenderCmbSubstitute(asignation.AssignationType, asignation.ItemId));

    tr.appendChild(td1);
    tr.appendChild(td2);

    // ISSUS-101
    // tr.appendChild(td3);
    target.appendChild(tr);
}

function RenderCmbSubstitute(itemtype, itemid)
{
    var target = document.createElement('SELECT');
    target.id = itemtype + '-' + itemid;

    var optionDefault = document.createElement('OPTION');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);

    for(var x=0;x<Company.Employees.length;x++)
    {
        if (Company.Employees[x].Id !== employeeId && Company.Employees[x].Active === true && Company.Employees[x].DisabledDate === null) {
            var option = document.createElement('OPTION');
            option.value = Company.Employees[x].Id;
            option.appendChild(document.createTextNode(Company.Employees[x].Name + ' ' + Company.Employees[x].LastName));
            target.appendChild(option);
        }
    }

    return target;
}

function ChangeAll() {
    var value = document.getElementById('All').value;
    var target = document.getElementById('TableEmployeeElements');
    for (var x = 0; x < target.childNodes.length; x++) {
        var row = target.childNodes[x];
        var select = row.lastChild.firstChild;
        select.value = value;
    }
}

function FillCmbAll()
{
    var target = document.getElementById('All');

    var optionDefault = document.createElement('OPTION');
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectOne));
    target.appendChild(optionDefault);

    for (var x = 0; x < Company.Employees.length; x++) {
        if (Company.Employees[x].Id !== employeeId && Company.Employees[x].Active === true && Company.Employees[x].DisabledDate === null) {
            var option = document.createElement('OPTION');
            option.value = Company.Employees[x].Id;
            option.appendChild(document.createTextNode(Company.Employees[x].Name + ' ' + Company.Employees[x].LastName));
            target.appendChild(option);
        }
    }
}

$('#BtnSave').on('click', SaveEmployee);
$('#BtnCancel').on('click', Cancel);

function Cancel() {
    var location = document.location + '';
    if (location.indexOf('&New=true') != -1) {
        
    }
    else {
        document.location = referrer;
    }
}

function SaveEmployee() {
    var ok = true;
    /*data = '';
    var target = document.getElementById('TableEmployeeElements');
    for (var x = 0; x < target.childNodes.length; x++) {
        var row = target.childNodes[x];
        row.style.color = '#000';
        data += row.lastChild.firstChild.id + ':' + row.lastChild.firstChild.value + '|';
        if (row.lastChild.firstChild.value * 1 === 0) {
            row.style.color = '#F00';
            ok = false;
        }
    }

    if (ok === false) {
        warningInfoUI(Dictionary.Item_Employee_List_Delete_Error, null, 300)
        return false;
    }*/
    
    if($('#All').val() * 1 === 0)
    {
        warningInfoUI("Se ha de seleccionar un sustituto", null, 300);
        return false;
    }

    var webMethod = "/Async/EmployeeActions.asmx/Substitute";
    var data = {
        "data": $('#TxtData').val(),
        "userId": user.Id,
        "companyId": companyId,
        "actualEmployee": employeeId,
        "newEmployee": $('#All').val() * 1
    };

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                document.location = 'EmployeesList.aspx';
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

FillCmbAll();
RenderTable();