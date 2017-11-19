var SelectedFinishDate = null;
var SelectedStartDate = null;

function ShowJobPositionAsociationPopup()
{
    JobPositionSelected = -1;
    RenderJobPositionPopup();
    $("#JobPositionAssociationDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">&nbsp;' + Dictionary.Item_Employee_Popup_LinkJobPosition_Message + '</h4>',
        width: 800,
        title_html: true,
        buttons: [
            {
                html: '<i class="icon-remove bigger-110"></i>&nbsp;' + Dictionary.Common_Cancel,
                "class": 'btn btn-xs',
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

var DeletableJobPostionRow = null;

function UnassociatedJobPosition(sender) {
    document.getElementById("TxtFinishDateErrorMaximumToday").style.display = "none";
    var id = sender.parentNode.parentNode.id * 1;
    var jobPosition = GetJobPositionById(id);

    if (jobPosition === null) {
        return false;
    }

    console.log(sender);
    $('#BtnStartCopyDate').val(sender.parentNode.parentNode.childNodes[3].innerHTML);
    $('#TxtFinishDate').val('');
    DeletableJobPostionRow = sender;
    
    var jobPosition = GetJobPositionById(id);
    $("#JobPositionDesassociationText").html(jobPosition.Description);
    var dialog = $("#JobPositionDesassociationDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Item_Employee_Button_Unlink,
        title_html: true,
        buttons: [
                {
                    html: "<i class='icon-trash bigger-110'></i>&nbsp; " + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    click: function () {
                        UnassociatedJobPositionConfirmed(id);
                    }
                },
                {
                    html: "<i class='icon-remove bigger-110'></i>&nbsp; " + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]

    });
}

function UnassociatedJobPositionConfirmed(id) {
    document.getElementById('TxtFinishDateErrorRequired').style.display = 'none';
    document.getElementById('TxtFinishDateErrorMaximumToday').style.display = 'none';
    document.getElementById('TxtFinishDateErrorBeforeStart').style.display = 'none';

    if ($('#TxtFinishDate').val() === '')
    {
        document.getElementById('TxtFinishDateErrorRequired').style.display = 'block';
        return false;
    }

    var finishDate = GetDate($('#TxtFinishDate').val(), '/', false);
    if(finishDate > new Date())
    {
        document.getElementById('TxtFinishDateErrorMaximumToday').style.display = 'block';
        return false;
    }

    var startDate = GetDate($('#BtnStartCopyDate').val(), '/', false);
    if (startDate > finishDate)
    {
        document.getElementById('TxtFinishDateErrorBeforeStart').style.display = 'block';
        return false;
    }

    SelectedFinishDate = finishDate;
    var webMethod = "/Async/EmployeeActions.asmx/DesassociateJobPosition";
    var data = {
        'employeeId': employee.Id,
        'companyId': Company.Id,
        'date': finishDate,
        'jobPositionId': id,
        'userId': user.Id
    };
    $("#JobPositionDesassociationDialog").dialog("close");
    LoadingShow('');
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            // delete to actual jobpositions
            for (var x = 0; x < jobPositionEmployee.length; x++) {
                if (jobPositionEmployee[x].Id == id) {
                    jobPositionEmployee[x].EndDate = true;
                }
            }

            // hide hitorical row
            DeletableJobPostionRow.parentNode.appendChild(document.createTextNode(FormatDate(SelectedFinishDate, '/')));
            DeletableJobPostionRow.parentNode.align = 'center';
            // DeletableJobPostionRow.parentNode.parentNode.lastChild.innerHTML = '';
            DeletableJobPostionRow.parentNode.removeChild(DeletableJobPostionRow);

        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function RenderJobPositionPopup() {
    var index = new Array();
    var targetList = new Array();
    for (var x = 0; x < jobPositionCompany.length; x++) {
        var value = jobPositionCompany[x].Description;
        index.push({ Key: x, Value: value });
    }

    index.sort(sort_by('Value', true, function (a) { return a.toUpperCase(); }));

    for (var x2 = 0; x2 < index.length; x2++) {
        targetList.push(jobPositionCompany[index[x2].Key]);
    }

    jobPositionCompany = new Array();
    for (var x3 = 0; x3 < targetList.length; x3++) {
        jobPositionCompany.push(targetList[x3]);
    }


    // 1.- Crear la lista de departamentos
    VoidTable('JobPositionAssociationDialogTable');
    var target = document.getElementById('JobPositionAssociationDialogTable');
    for (var x4 = 0; x4 < jobPositionCompany.length; x4++) {
        JobPositionPopupRow(jobPositionCompany[x4], target);
    }
}

function JobPositionPopupRow(jobPosition, target) {
    var selected = JobPositionIsSelected(jobPosition.Id);

    var tr = document.createElement('tr');
    tr.id = jobPosition.Id;

    var td1 = document.createElement('td');
    td1.appendChild(document.createTextNode(jobPosition.Description));
    if (selected === true) {
        td1.style.fontWeight = 'bold';
    }

    var tdDept = document.createElement('td');
    tdDept.appendChild(document.createTextNode(jobPosition.Department.Name));
    if (selected === true) {
        tdDept.style.fontWeight = 'bold';
    }

    var td2 = document.createElement('td');
    var div = document.createElement('div');
    var span1 = document.createElement('span');
    span1.className = 'btn btn-xs btn-success';
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement('i');
    i1.className = 'icon-star bigger-120';
    span1.appendChild(i1);

    if (selected === true) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected, 'JobPositionAssociationDialog'); };
    }
    else {
        span1.onclick = function () { JobPositionAssociationAction(this); };
    }

    div.appendChild(span1);
    td2.appendChild(div);
    
    tr.appendChild(td1);
    tr.appendChild(tdDept);
    tr.appendChild(td2);
    target.appendChild(tr);
}

$('#JobPositionAssociationDateDialog').on('dialogclose', function (event) {
    $('#JobPositionAssociationDialog').dialog('open');
});

function JobPositionAssociationAction(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $('#JobPositionAssociationDialog').dialog('close');
    $('#TxtStartDate').val('');
    var dialog = $("#JobPositionAssociationDateDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Item_Employee_Button_LinkJobPosition,
        title_html: true,
        buttons: [
            {
                "html": "<i class='icon-trash bigger-110'></i>&nbsp; " + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    JobPositionAssociationActionConfirmed(id);
                }
            },
            {
                "html": "<i class='icon-remove bigger-110'></i>&nbsp; " + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $('#JobPositionAssociationDialog').dialog('open');
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function JobPositionAssociationActionConfirmed(id)
{
    document.getElementById('TxtStartDateErrorRequired').style.display = 'none';
    document.getElementById('TxtStartDateErrorMaximumToday').style.display = 'none';

    if ($('#TxtStartDate').val() === '') {
        document.getElementById('TxtStartDateErrorRequired').style.display = 'block';
        return false;
    }

    var StartDate = GetDate($('#TxtStartDate').val(), '/', false);
    if (StartDate > new Date()) {
        document.getElementById('TxtStartDateErrorMaximumToday').style.display = 'block';
        return false;
    }

    SelectedStartDate = StartDate;
    $("#JobPositionAssociationDateDialog").dialog('close');
    var webMethod = "/Async/EmployeeActions.asmx/AssociateJobPosition";
    var data = {
        'employeeId': employee.Id,
        'companyId': Company.Id,
        'date': StartDate,
        'jobPositionId': id,
        'userId': user.Id
    };
    $("#JobPositionAssociationDialog").dialog("close");
    LoadingShow('');
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            // add to actual jobpositions
            var jobposition = GetJobPositionById(id);
            jobPositionEmployee.push({ "Id": id, "Description": jobposition.Description, "EndDate": false });
            // render hitorical row
            var target = document.getElementById('WorkExperienceDataTable');

            var jobPositionLink = document.createElement('a');
            jobPositionLink.title = Dictionary.Common_Edit + ' ' + jobposition.Description;
            jobPositionLink.appendChild(document.createTextNode(jobposition.Description));
            jobPositionLink.href = 'CargosView.aspx?id=' + id;

            var departmentLink = document.createElement('a');
            departmentLink.title = Dictionary.Common_Edit + ' ' + jobposition.Department.Name;
            departmentLink.appendChild(document.createTextNode(jobposition.Department.Name));
            departmentLink.href = 'DepartmentView.aspx?id=' + jobposition.Department.Id;

            var tr = document.createElement('tr');
            tr.id = id;
            var td1 = document.createElement('td');
            td1.appendChild(jobPositionLink);

            var td2 = document.createElement('td');
            td2.appendChild(departmentLink);

            var td3 = document.createElement('td');
            if (jobposition.Responsible === null) {
                td3.appendChild(document.createTextNode(' '));
            }
            else {
                td3.appendChild(document.createTextNode(jobposition.Responsible.Description));
            }

            var td4 = document.createElement('td');
            td4.appendChild(document.createTextNode(FormatDate(SelectedStartDate, '/'))); td4.align = 'center';

            var td5 = document.createElement('td'); //td5.appendChild(unlinkButton);
            td5.innerHTML = ' <button class="btn btn-warning" type="button" style="padding:0 !important" onclick="UnassociatedJobPosition(this);"><i class="icon-remove bigger-110"></i>Desvincular</button>';

            var td6 = document.createElement('td');
            td6.align = 'center';
            td6.innerHTML = '<span title="Eliminar" class="btn btn-xs btn-danger" onclick="DeleteJobPosition(' + jobposition.Id + ',\'' + jobposition.Description + '\');"><i class="icon-trash bigger-120"></i></span>';


            tr.appendChild(td1);
            tr.appendChild(td2);
            tr.appendChild(td3);
            tr.appendChild(td4);
            tr.appendChild(td5);
            tr.appendChild(td6);
            target.appendChild(tr);
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function GetJobPositionById(id) {
    for (var x = 0; x < jobPositionCompany.length; x++) {
        if (jobPositionCompany[x].Id === id) {
            return jobPositionCompany[x];
        }
    }
    return Dictionary.NoDefinido;
}

function JobPositionIsSelected(id) {
    for (var x = 0; x < jobPositionEmployee.length; x++) {
        if (jobPositionEmployee[x].Id == id && jobPositionEmployee[x].EndDate === false) {
            return true;
        }
    }
    return false;
}

function SortBy(nameField) {
    return function (a, b) {
        if (a[nameField] > b[nameField]) {
            return 1;
        } else if (a[nameField] < b[nameField]) {
            return -1;
        }
        return 0;
    }
}