function CompareProcessType(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function ProccessTypeRenderPopup() {
    VoidTable('SelectableProcessType');
    processTypeCompany.sort(CompareProcessType);
    var target = document.getElementById('SelectableProcessType');
    for (var x = 0; x < processTypeCompany.length; x++) {
        processTypePopupRow(processTypeCompany[x], target)
    }
}

function processTypePopupRow(item, target) {
    if (item.Active === false) return;
    var tr = document.createElement('tr');
    tr.id = item.Id;
    var td1 = document.createElement('td');
    var td2 = document.createElement('td');
    if (processTypeSelected === item.Id) {
        td1.style.fontWeight = 'bold';
    }
    td1.appendChild(document.createTextNode(item.Description));

    var div = document.createElement('div');
    var span1 = document.createElement('span');
    span1.className = 'btn btn-xs btn-success';
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement('i');
    i1.className = 'icon-star bigger-120';
    span1.appendChild(i1);

    if (processTypeSelected === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { ProcessTypeChanged(this); };
    }

    div.appendChild(span1);

    var span2 = document.createElement('span');
    span2.className = 'btn btn-xs btn-info';
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement('i');
    i2.className = 'icon-edit bigger-120';
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(' '));
    div.appendChild(span2);

    if (item.Id < 4) {
        span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Edit, 'dialogProcessType'); };
    }
    else {
        span2.onclick = function () { ProcessTypeUpdate(this); };
    }

    var span3 = document.createElement('span');
    span3.className = 'btn btn-xs btn-danger';
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement('i');
    i3.className = 'icon-trash bigger-120';
    span3.appendChild(i3);

    if (processTypeSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected, 'dialogProcessType'); };
    }
    else if (item.Id < 4) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete, 'dialogProcessType'); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable, 'dialogProcessType'); };
    }
    else {
        span3.onclick = function () { ProcessTypeDelete(this); };
    }

    div.appendChild(document.createTextNode(' '));
    div.appendChild(span3);
    td2.appendChild(div);


    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}