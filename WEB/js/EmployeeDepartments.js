function RenderDepartmentsPopup() {
    return;
    /*departmentsCompany = departmentsCompany.sort(SortByName);
    // 1.- Crear la lista de departamentos
    VoidTable('DepartmentsPopupBody');
    var target = document.getElementById('DepartmentsPopupBody');
    for (var x = 0; x < departmentsCompany.length; x++) {
        CompanyDepartmentRow(departmentsCompany[x], target);
    }*/
}

function DepartmentAddTag(id,name) {
    var tags = document.getElementById('DepartmentsTagsDiv');
    var spanTag = document.createElement('span');
    spanTag.id = id;
    spanTag.appendChild(document.createTextNode(name));
    spanTag.style.cursor = 'pointer';
    spanTag.className = 'tag';
    var buttonTag = document.createElement('button');
    buttonTag.type = 'button';
    buttonTag.className = 'close';
    buttonTag.title = Dictionary.Common_Delete;
    buttonTag.appendChild(document.createTextNode('x'));
    buttonTag.onclick = function () { DepartmentDesassociation(id, name); };
    spanTag.appendChild(buttonTag);
    tags.appendChild(spanTag);
}

function CompanyDepartmentRow(department, target) {
    var selected = DepartmetIsSelected(department.Id);
    var tr = document.createElement('tr');
    tr.id = department.Id;
    
    var td1 = document.createElement('td');
    td1.appendChild(document.createTextNode(department.Name));
    if (selected===true) {
        td1.style.fontWeight = 'bold';
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
        span1.onclick = function () { alert(Dictionary.Common_ErrorMessage_IsSelected); };
    }
    else {
        span1.onclick = function () { DepartmentAssociationAction(department.Id); };
    }

    div.appendChild(span1);

    var span2 = document.createElement('span');
    span2.className = 'btn btn-xs btn-info';
    span2.onclick = function () { DepartmentPopupUpdate(this); };
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement('i');
    i2.className = 'icon-edit bigger-120';
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(' '));
    div.appendChild(span2);

    var span3 = document.createElement('span');
    span3.className = 'btn btn-xs btn-danger';
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement('i');
    i3.className = 'icon-trash bigger-120';
    span3.appendChild(i3);
    if (DepartmetIsSelected(department.Id) === true) {
        span3.onclick = function () { alert(Dictionary.Common_ErrorMessage_CanNotDelete); };
    }
    else {
        span3.onclick = function () { DepartmentDelete(this); };
    }

    div.appendChild(document.createTextNode(" "));
    div.appendChild(span3);
    td2.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}

function DepartmetIsSelected(departmentId) {
    for (var x = 0; x < departmentsEmployee.length; x++) {
        if (departmentsEmployee[x].Id === departmentId) {
            return true;
        }
    }
    return false;
}

function RenderDeparmentsEmployee() {
    return;
    /*VoidTable('DeparmentsEmployee');
    var target = document.getElementById('DeparmentsEmployee');
    for (var x = 0; x < departmentsEmployee.length; x++) {
        EmployeeDepartmentRow(departmentsEmployee[x], target)
    }*/
}

function EmployeeDepartmentRow(department, target) {
    var selected = DepartmetIsSelected(department.Id);

    var tr = document.createElement('tr');
    tr.id = department.Id;

    var td1 = document.createElement('td');
    td1.appendChild(document.createTextNode(department.Name));

    var td2 = document.createElement('td');
    var div = document.createElement('div');

    var span2 = document.createElement('span');
    span2.className = 'btn btn-xs btn-info';
    span2.onclick = function () { DepartmentUpdate(2, this); };
    span2.title = Dictionary.Common_Edit + ' ' + department.Name;
    var i2 = document.createElement('i');
    i2.className = 'icon-edit bigger-120';
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(' '));
    div.appendChild(span2);

    var span3 = document.createElement('span');
    span3.className = 'btn btn-xs btn-danger';
    span3.title = Dictionary.Item_Employee_Popup_UnlinkJobPosition_Message + ' ' + department.Name;
    var i3 = document.createElement('i');
    i3.className = 'icon-trash bigger-120';
    span3.appendChild(i3);
    span3.onclick = function () { DepartmentDesassociation(this); };

    div.appendChild(document.createTextNode(' '));
    div.appendChild(span3);
    td2.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}

function GetDepartmentNameById(id) {
    for (var x = 0; x < departmentsCompany.length; x++) {
        if (departmentsCompany[x].Id === id) {
            return departmentsCompany[x].Name;
        }
    }

    return Dictionary.NoDefinido;
}