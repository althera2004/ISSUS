var save = true;

function AcceptSave() { SaveConfirmed(true); }
function CancelSave() { }

function EmployeeDelete(id, name) {
    $('#DeleteEmployeeName').html(name);
    var dialog = $("#EmployeeDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        "buttons": [
                {
                    "html": "<i class=\"con-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        DeleteEmployeeConfirmed(id);
                    }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
        ]
    });
}

function DeleteEmployeeConfirmed(id) {
    if (formacion.Id === 0) {
        var survivors = new Array();
        for (var x = 0; x < SelectedEmployees.length; x++) {
            if (SelectedEmployees[x].AssistantId !== id) {
                survivors.push(SelectedEmployees[x]);
            }
        }

        SelectedEmployees = new Array();
        VoidTable("SelectedEmployeesTable");
        for (var y = 0; y < survivors.length; y++) {
            SelectedEmployees.push(survivors[y]);
            InsertEmployeeRow(survivors[y]);
        }

        $("#EmployeeDeleteDialog").dialog("close");
        return false;
    }

    var webMethod = "/Async/LearningActions.asmx/DeleteAssistant";
    var data = { assistantId: id, companyId: Company.Id, userId: user.Id, learningId: formacion.Id };
    $("#EmployeeDeleteDialog").dialog("close");

    LoadingShow();
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            // Eliminar de la lista
            var survivors = new Array();
            for (var x = 0; x < SelectedEmployees.length; x++) {
                if (SelectedEmployees[x].AssistantId !== id) {
                    survivors.push(SelectedEmployees[x]);
                }
            }

            SelectedEmployees = new Array();
            VoidTable('SelectedEmployeesTable');
            for (var y = 0; y < survivors.length; y++) {
                SelectedEmployees.push(survivors[y]);
                InsertEmployeeRow(survivors[y]);
            }

        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function Realizado() {
    // Complete(int companyId, AssistantData[] assistants, int userId)
    var assistants = new Array();
    for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
        if (SelectedEmployeesTable.childNodes[x].childNodes[0].childNodes[0].checked === true) {
            var id = SelectedEmployeesTable.childNodes[x].id;
            var AssistantId = id.split('|')[0] * 1;
            var EmployeeId = id.split('|')[1] * 1;
            assistants.push({ AssistantId: AssistantId, EmployeeId: EmployeeId, LearningId: formacion.Id });
        }
    }

    if (assistants.length === 0) {
        alert(Dictionary.Item_Learning_Error_MimumOneAssistant);
    }
    else {
        var webMethod = "/Async/LearningActions.asmx/Complete";
        var data = { assistants: assistants, companyId: Company.Id, userId: user.Id };
        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                LoadingHide();
                if (msg.d.Success === true) {
                    var result;
                    eval("result = " + msg.d.MessageError + ";");
                    for (var x = 0; x < result.length; x++) {
                        var trId = result[x].AssistantId + '|' + result[x].EmployeeId;
                        if (document.getElementById(trId) !== null) {
                            document.getElementById(trId).childNodes[2].childNodes[0].innerHTML = Dictionary.Common_Yes;
                            document.getElementById(trId).childNodes[2].childNodes[0].style.color = 'green';
                            document.getElementById(trId).childNodes[2].childNodes[0].onclick = function () { Toggle(this, 'Completed', this.id, 1); };
                            document.getElementById(trId).childNodes[2].childNodes[0].id = result[x].AssistantId;
                        }
                    }
                }
                else {
                    alert(msg.d.MessageError);
                }
            },
            error: function (msg) {
                LoadingHide();
                alertUI(msg.responseText);
            }
        });
    }
}

function RealizadoFail() {
    // Complete(int companyId, AssistantData[] assistants, int userId)
    var assistants = new Array();
    for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
        if (SelectedEmployeesTable.childNodes[x].childNodes[0].childNodes[0].checked === true) {
            var id = SelectedEmployeesTable.childNodes[x].id;
            var AssistantId = id.split('|')[0] * 1;
            var EmployeeId = id.split('|')[1] * 1;
            assistants.push({ AssistantId: AssistantId, EmployeeId: EmployeeId, LearningId: formacion.Id });
        }
    }

    if (assistants.length === 0) {
        alert(Dictionary.Item_Learning_Error_MimumOneAssistant);
    }
    else {
        var webMethod = "/Async/LearningActions.asmx/CompleteFail";
        var data = { assistants: assistants, companyId: Company.Id, userId: user.Id };
        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                LoadingHide();
                if (msg.d.Success === true) {
                    var result;
                    eval("result = " + msg.d.MessageError + ";");
                    for (var x = 0; x < result.length; x++) {
                        var trId = result[x].AssistantId + '|' + result[x].EmployeeId;
                        if (document.getElementById(trId) !== null) {
                            document.getElementById(trId).childNodes[2].childNodes[0].innerHTML = Dictionary.Common_No;
                            document.getElementById(trId).childNodes[2].childNodes[0].style.color = 'red';
                            document.getElementById(trId).childNodes[3].childNodes[0].innerHTML = '-';
                            document.getElementById(trId).childNodes[3].childNodes[0].style.color = 'grey';
                            document.getElementById(trId).childNodes[2].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[3].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[2].childNodes[0].onclick = function () { Toggle(this, 'Completed', this.id * 1, 1); };
                            document.getElementById(trId).childNodes[3].childNodes[0].onclick = function () { Toggle(this, 'Success', this.id * 1, 1); };
                        }
                    }
                }
                else {
                    alert(msg.d.MessageError);
                }
            },
            error: function (msg) {
                LoadingHide();
                alertUI(msg.responseText);
            }
        });
    }
}

function Unevaluated() {
    // Complete(int companyId, AssistantData[] assistants, int userId)
    var assistants = new Array();
    for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
        if (SelectedEmployeesTable.childNodes[x].childNodes[0].childNodes[0].checked === true) {
            var id = SelectedEmployeesTable.childNodes[x].id;
            var AssistantId = id.split('|')[0] * 1;
            var EmployeeId = id.split('|')[1] * 1;
            assistants.push({ AssistantId: AssistantId, EmployeeId: EmployeeId, LearningId: formacion.Id });
        }
    }

    if (assistants.length === 0) {
        alert(Dictionary.Item_Learning_Error_MimumOneAssistant);
    }
    else {
        var webMethod = "/Async/LearningActions.asmx/Unevaluated";
        var data = { assistants: assistants, companyId: Company.Id, userId: user.Id };
        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                LoadingHide();
                if (msg.d.Success === true) {
                    var result;
                    eval("result = " + msg.d.MessageError + ";");
                    for (var x = 0; x < result.length; x++) {
                        var trId = result[x].AssistantId + '|' + result[x].EmployeeId;
                        if (document.getElementById(trId) !== null) {
                            document.getElementById(trId).childNodes[2].childNodes[0].innerHTML = "-";
                            document.getElementById(trId).childNodes[2].childNodes[0].style.color = 'greay';
                            document.getElementById(trId).childNodes[3].childNodes[0].innerHTML = '-';
                            document.getElementById(trId).childNodes[3].childNodes[0].style.color = 'grey';
                            document.getElementById(trId).childNodes[2].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[3].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[2].childNodes[0].onclick = function () { Toggle(this, 'Completed', this.id * 1, 1); };
                            document.getElementById(trId).childNodes[3].childNodes[0].onclick = function () { Toggle(this, 'Success', this.id * 1, 1); };
                        }
                    }
                }
                else {
                    alert(msg.d.MessageError);
                }
            },
            error: function (msg) {
                LoadingHide();
                alertUI(msg.responseText);
            }
        });
    }
}

function Item_LearningAssistant_Status_Evaluated() {
    var assistants = new Array();
    for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
        if (SelectedEmployeesTable.childNodes[x].childNodes[0].childNodes[0].checked === true) {
            var id = SelectedEmployeesTable.childNodes[x].id;
            var AssistantId = id.split('|')[0] * 1;
            var EmployeeId = id.split('|')[1] * 1;
            assistants.push({ AssistantId: AssistantId, EmployeeId: EmployeeId, LearningId: formacion.Id });
        }
    }

    if (assistants.length === 0) {
        alert('Hay que seleccionar algún asistente');
    }
    else {
        var webMethod = "/Async/LearningActions.asmx/Success";
        var data = { assistants: assistants, companyId: Company.Id, userId: user.Id };
        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                if (msg.d.Success === true) {
                    var result;
                    eval("result = " + msg.d.MessageError + ";");
                    for (var x = 0; x < result.length; x++) {
                        var trId = result[x].AssistantId + '|' + result[x].EmployeeId;
                        if (document.getElementById(trId) !== null) {
                            document.getElementById(trId).childNodes[2].childNodes[0].innerHTML = Dictionary.Common_Yes;
                            document.getElementById(trId).childNodes[2].childNodes[0].style.color = 'green';
                            document.getElementById(trId).childNodes[3].childNodes[0].innerHTML = Dictionary.Common_Yes;
                            document.getElementById(trId).childNodes[3].childNodes[0].style.color = 'green';
                            document.getElementById(trId).childNodes[2].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[3].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[2].childNodes[0].onclick = function () { Toggle(this, 'Completed', this.id * 1, 1); };
                            document.getElementById(trId).childNodes[3].childNodes[0].onclick = function () { Toggle(this, 'Success', this.id * 1, 1); };
                        }
                    }
                }
                else {
                    alert(msg.d.MessageError);
                }
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function Item_LearningAssistant_Status_EvaluatedFail() {
    var assistants = new Array();
    for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
        if (SelectedEmployeesTable.childNodes[x].childNodes[0].childNodes[0].checked === true) {
            var id = SelectedEmployeesTable.childNodes[x].id;
            var AssistantId = id.split('|')[0] * 1;
            var EmployeeId = id.split('|')[1] * 1;
            assistants.push({ AssistantId: AssistantId, EmployeeId: EmployeeId, LearningId: formacion.Id });
        }
    }

    if (assistants.length === 0) {
        alert('Hay que seleccionar algún asistente');
    }
    else {
        var webMethod = "/Async/LearningActions.asmx/SuccessFail";
        var data = { assistants: assistants, companyId: Company.Id, userId: user.Id };
        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                if (msg.d.Success === true) {
                    var result;
                    eval("result = " + msg.d.MessageError + ";");
                    for (var x = 0; x < result.length; x++) {
                        var trId = result[x].AssistantId + '|' + result[x].EmployeeId;
                        if (document.getElementById(trId) !== null) {
                            document.getElementById(trId).childNodes[2].childNodes[0].innerHTML = Dictionary.Common_Yes;
                            document.getElementById(trId).childNodes[2].childNodes[0].style.color = 'green';
                            document.getElementById(trId).childNodes[3].childNodes[0].innerHTML = Dictionary.Common_No;
                            document.getElementById(trId).childNodes[3].childNodes[0].style.color = 'red';
                            document.getElementById(trId).childNodes[2].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[3].childNodes[0].id = result[x].AssistantId;
                            document.getElementById(trId).childNodes[2].childNodes[0].onclick = function () { Toggle(this, 'Completed', this.id * 1, 1); };
                            document.getElementById(trId).childNodes[3].childNodes[0].onclick = function () { Toggle(this, 'Success', this.id * 1, 1); };
                        }
                    }
                }
                else {
                    alert(msg.d.MessageError);
                }
            },
            error: function (msg) {
                alertUI(msg.responseText);
            }
        });
    }
}

function SelectAllAssistants(sender) {
    if (sender.checked === true) {
        sender.title = Dictionary.Item_LearningAsistant_Button_UnselectAll;
    }
    else {
        sender.title = Dictionary.Item_LearningAsistant_Button_SelectAll;
    }

    for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
        SelectedEmployeesTable.childNodes[x].childNodes[0].childNodes[0].checked = sender.checked;
    }
}

function SelectAll(sender) {
    if (sender.checked === true) {
        sender.title = Dictionary.Item_LearningAsistant_Button_UnselectAll;
    }
    else {
        sender.title = Dictionary.Item_LearningAsistant_Button_SelectAll;
    }

    var target = document.getElementById('SelectableEmployeesTable').childNodes;
    for (var x = 0; x < target.length; x++) {
        if (target[x].tagName === 'TR') {
            target[x].childNodes[0].childNodes[0].checked = sender.checked;
        }
    }
}

function Toggle(sender, action, value, status) {
    if (sender.parentNode.parentNode.childNodes[2].childNodes[0].innerHTML !== Dictionary.Common_Yes && action === "Success") {
        alert(Dictionary.Item_Learning_Error_NoCompleted);
        return false;
    }

    var completedCode = 0;
    var successCode = 0;

    if (action === 'Completed') {
        if (status === 0) { completedCode = 1; }
        if (status === 1) { completedCode = 2; }
        if (status === 2) { completedCode = 0; }
        sender.parentNode.parentNode.childNodes[2].childNodes[0].innerHTML = '<img src="assets/images/LoadingIcon.gif" />';
    }

    if (action === 'Success') {
        completedCode = 1;
        if (status === 0) { successCode = 1; }
        if (status === 1) { successCode = 2; }
        if (status === 2) { successCode = 0; }
        sender.parentNode.parentNode.childNodes[3].childNodes[0].innerHTML = '<img src="assets/images/LoadingIcon.gif" />';
    }

    var webMethod = "/Async/LearningActions.asmx/Reset";
    var data = { assistantId: value, companyId: Company.Id, userId: user.Id, completedCode: completedCode, successCode: successCode };
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            if (msg.d.Success === true) {
                var status;
                eval("status=" + msg.d.MessageError + ";");
                //alert(status.AssistantId+'-'+status.Completed+'-'+status.Success);
                for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
                    if (SelectedEmployeesTable.childNodes[x].id.split('|')[0] * 1 == status.AssistantId) {
                        var completedSpan = SelectedEmployeesTable.childNodes[x].childNodes[2].childNodes[0];
                        var successSpan = SelectedEmployeesTable.childNodes[x].childNodes[3].childNodes[0];
                        if (status.Completed === 0) {
                            completedSpan.innerHTML = '-';
                            completedSpan.style.color = '#000';
                            successSpan.innerHTML = '-';
                            successSpan.style.color = '#000';
                        }
                        if (status.Completed === 1) {
                            completedSpan.innerHTML = Dictionary.Common_Yes;
                            completedSpan.style.color = 'green';
                            if (status.Success === 0) {
                                successSpan.innerHTML = '-';
                                successSpan.style.color = '#000';
                            }

                            if (status.Success === 1) {
                                successSpan.innerHTML = Dictionary.Common_Yes;
                                successSpan.style.color = 'green';
                            }

                            if (status.Success === 2) {
                                successSpan.innerHTML = Dictionary.Common_No;
                                successSpan.style.color = 'red';
                            }
                        }
                        if (status.Completed === 2) {
                            completedSpan.innerHTML = Dictionary.Common_No;
                            completedSpan.style.color = 'red';
                            successSpan.innerHTML = '-';
                            successSpan.style.color = '#000';
                        }

                        completedSpan.onclick = function () { Toggle(this, 'Completed', status.AssistantId, status.Completed); };
                        successSpan.onclick = function () { Toggle(this, 'Success', status.AssistantId, status.Success); };
                        break;
                    }
                }
            }
            else {
                alertUI(msg.d.MessageError);
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });

}

function InsertEmployeeRow(assistant) {
    console.log("InserEmployee:" + assistant.AssistantId);
    var employee = null;
    for (var x = 0; x < Company.Employees.length; x++) {
        if (Company.Employees[x].Id === assistant.EmployeeId * 1) {
            employee = Company.Employees[x];
            break;
        }
    }

    if (employee !== null) {
        var tr = document.createElement('tr');
        var td1 = document.createElement('td');
        var td2 = document.createElement('td');

        var span3 = document.createElement('span');
        span3.className = 'btn btn-xs btn-danger';
        span3.onclick = function () { EmployeeDelete(assistant.AssistantId, (employee.Name + ' ' + employee.LastName).trim()); };
        span3.title = Dictionary.Common_Delete;
        var i3 = document.createElement('i');
        i3.className = 'icon-trash bigger-120';
        span3.appendChild(i3);
        td2.appendChild(span3);

        td1.appendChild(document.createTextNode((employee.Name + ' ' + employee.LastName).trim()));
        td2.align = 'center';

        tr.appendChild(td1);
        tr.appendChild(td2);
        document.getElementById('SelectedEmployeesTable').appendChild(tr);
    }
}

function InsertAssistant(employeeId) {
    if (formacion.Id === 0)
    {
        lastNewAssistantId++;
        var assistantId = lastNewAssistantId;
        var newAssistant = { "EmployeeId": employeeId, "AssistantId": assistantId };
        SelectedEmployees.push(newAssistant);
        InsertEmployeeRow(newAssistant);
        return false;
    }

    var webMethod = "/Async/LearningActions.asmx/InsertAssistant";
    var data = { employeeId: employeeId, companyId: Company.Id, userId: user.Id, learningId: formacion.Id };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            var assistantId = msg.d.MessageError.split('|')[0] * 1;
            var employeeId = msg.d.MessageError.split('|')[1] * 1;
            var newAssistant = { "EmployeeId": employeeId, "AssistantId": assistantId };
            SelectedEmployees.push(newAssistant);
            InsertEmployeeRow(newAssistant);
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function SelectEmployees() {
    var target = document.getElementById('SelectableEmployeesTable').childNodes;
    for (var x = 0; x < target.length; x++) {
        if (target[x].tagName == 'TR') {
            if (target[x].childNodes[0].childNodes[0].checked) {
                var id = target[x].childNodes[0].childNodes[0].id.substr(3);
                InsertAssistant(id);
            }
        }
    }
    $('#assistantDialog').dialog("close");
}

function Save() {
    // Sólo si el status = 1 (Realizado) se revisa si está en condiciones de pasar a evaluado
    var evaluatedAll = false;
    if (formacion.Status == 1) {
        evaluatedAll = true;
        for (var x = 0; x < SelectedEmployeesTable.childNodes.length; x++) {
            if (SelectedEmployeesTable.childNodes[x].childNodes[2].childNodes[0].innerHTML == '-') {
                evaluatedAll = false;
                break;
            }
            else if (SelectedEmployeesTable.childNodes[x].childNodes[2].childNodes[0].innerHTML == Dictionary.Common_Yes &&
                    SelectedEmployeesTable.childNodes[x].childNodes[3].childNodes[0].innerHTML == '-') {
                evaluatedAll = false;
                break;
            }
        }
    }

    if (evaluatedAll === true) {
        promptInfoUI(Dictionary.Item_Learning_Message_Evaluation, 300, AcceptSave, CancelSave);
    }
    else {
        SaveConfirmed(false);
    }
}

function SaveConfirmed(evaluatedAll)
{
    ClearFieldTextMessages('TxtRealStart');
    ClearFieldTextMessages('TxtRealFinish');
    var d1 = null;
    if (document.getElementById('TxtRealStart').value === '')
    {
        if (!RequiredDateValue('TxtRealStart') && formacion.status > 1) {
            ok = false;
        }
    }
    else {
        d1 = GetDate($("#TxtRealStart").val(), "/", true);
    }

    var d2 = null;
    if (document.getElementById("TxtRealFinish").value === '') {
        if (!RequiredDateValue("TxtRealFinish") && formacion.status > 1) {
            ok = false;
        }
    }
    else {
        d2 = GetDate($("#TxtRealFinish").val(), "/", true);
    }
    var ok = true;
    if (!RequiredFieldText('TxtYear')) { ok = false; }
    if (!RequiredFieldText('TxtName')) { ok = false; }

    if (formacion.Status < 2) {

        if (document.getElementById('CmbFechaPrevistaMes').value === '0' || document.getElementById('TxtFechaPrevistaYear').value == '') {
            ok = false;
            document.getElementById('CmbFechaPrevistaMesLabel').style.color = '#f00';
            if (document.getElementById('CmbFechaPrevistaMes').value === '0') {
                document.getElementById('CmbFechaPrevistaMesErrorRequired').style.display = 'block';
            }
            else {
                document.getElementById('CmbFechaPrevistaMesErrorRequired').style.display = 'none';
            }

            if (document.getElementById('TxtFechaPrevistaYear').value === '') {
                document.getElementById('TxtFechaPrevistaYearErrorRequired').style.display = 'block';
            }
            else {
                document.getElementById('TxtFechaPrevistaYearErrorRequired').style.display = 'none';
            }
        }
        else {
            document.getElementById('CmbFechaPrevistaMesLabel').style.color = '#000';
            document.getElementById('CmbFechaPrevistaMesErrorRequired').style.display = 'none';
            document.getElementById('TxtFechaPrevistaYearErrorRequired').style.display = 'none';
        }

        if (!RequiredFieldText('TxtMaster')) { ok = false; }
        if (!RequiredFieldText('TxtHours')) { ok = false; }
        // ISSUS-18 if (!RequiredFieldText('TxtAmount')) { ok = false; }

        if (document.getElementById('TxtRealFinish').value !== '') {
            if (!RequiredFieldText('TxtRealStart')) { ok = false; }

            if (d1 > d2) {
                ok = false;
                document.getElementById('TxtRealStartLabel').style.color = '#f00';
                document.getElementById('TxtRealFinishLabel').style.color = '#f00';
                document.getElementById('TxtRealStartErrorDateRange').style.display = 'block';
                document.getElementById('TxtRealFinishErrorDateRange').style.display = 'block';
            }
            else {
                document.getElementById('TxtRealStartLabel').style.color = '#000';
                document.getElementById('TxtRealFinishLabel').style.color = '#000';
                if (document.getElementById('TxtRealStartErrorDateRange') !== null) {
                    document.getElementById('TxtRealStartErrorDateRange').style.display = 'none';
                    document.getElementById('TxtRealFinishErrorDateRange').style.display = ' none';
                }
            }
        }
    }

    if (formacion.Status < 1) {
        ClearFieldTextMessages('TxtRealStart');
        ClearFieldTextMessages('TxtRealFinish');
        document.getElementById('TxtRealStartDateMalformed').style.display = 'none';
        document.getElementById('TxtRealFinishDateMalformed').style.display = 'none';
        var dates = true;

        if (document.getElementById('TxtRealFinish').value !== '') {
            if (!RequiredFieldText('TxtRealStart')) { ok = false; dates = false; }

            if (dates === true) {
                if (d1 > d2) {
                    ok = false;
                    document.getElementById('TxtRealStartLabel').style.color = '#f00';
                    document.getElementById('TxtRealFinishLabel').style.color = '#f00';
                    document.getElementById('TxtRealStartErrorDateRange').style.display = 'block';
                    document.getElementById('TxtRealFinishErrorDateRange').style.display = 'block';
                }
                else {
                    document.getElementById('TxtRealStartLabel').style.color = '#000';
                    document.getElementById('TxtRealFinishLabel').style.color = '#000';
                    if (document.getElementById('TxtRealStartErrorDateRange') != null) {
                        document.getElementById('TxtRealStartErrorDateRange').style.display = 'none';
                        document.getElementById('TxtRealFinishErrorDateRange').style.display = ' none';
                    }
                }
            }
            else {
                if (document.getElementById('TxtRealStartErrorDateRange') !== null) {
                    document.getElementById('TxtRealStartErrorDateRange').style.display = 'none';
                    document.getElementById('TxtRealFinishErrorDateRange').style.display = 'none';
                }
            }
        }

        if(dates===true)
        {
            ClearFieldTextMessages('TxtRealStart');
            ClearFieldTextMessages('TxtRealFinish');
        }
    }
    else {
        if (document.getElementById('TxtRealStartErrorDateRange') !== null) {
            document.getElementById('TxtRealStartErrorDateRange').style.display = 'none';
            document.getElementById('TxtRealFinishErrorDateRange').style.display = 'none';
        }
    }

    if (ok === false) {
        window.scrollTo(0, 0);
        return false;
    }

    //Eliminar los asistentes ya existentes
    var newAssistants = new Array();
    for (var x2 = 0; x2 < SelectedEmployees.length; x2++) {
        if (formacion.Id == 0) {
            newAssistants.push(SelectedEmployees[x2]);
        }
        else {
            newAssistants.push(SelectedEmployees[x2].EmployeeId * 1);
        }
    }

    var webMethod = '';
    if (formacion.Id < 1) {
        webMethod = 'Async/LearningActions.asmx/Insert';
    }
    else {
        webMethod = 'Async/LearningActions.asmx/Update';
    }

    if (formacion.Id !== 0) {
        if (typeof (formacion.DateEstimated) == 'string') {
            formacion.DateEstimated = new Date(formacion.DateEstimated.split('/')[2] * 1, formacion.DateEstimated.split('/')[1] * 1, formacion.DateEstimated.split('/')[0] * 1);
        }

        if (formacion.RealStart !== null) { formacion.RealStart = GetDate(formacion.RealStart, '/'); }
        if (formacion.RealFinish !== null) { formacion.RealFinish = GetDate(formacion.RealFinish, '/'); }
    }

    var dateEstimated = formacion.DateEstimated;
    if (formacion.Status < 2) {
        var month = 0;
        var list = document.getElementById('CmbFechaPrevistaMes').childNodes;
        for (var x3 = 0; x3 < list.length; x3++) {
            if (list[x3].tagName === 'OPTION') {
                if (list[x3].selected === true) {
                    month = list[x3].value;
                    break;
                }
            }
        }

        dateEstimated = new Date(document.getElementById('TxtFechaPrevistaYear').value * 1, month - 1, 1);
    }

    var status = 0;
    if (document.getElementById('RBStatus2').checked) {
        status = 1;
    }
    else if (document.getElementById('RBStatus3').checked) {
        status = 2;
    }

    if (evaluatedAll === true) {
        status = 2;
    }

    var amount = ParseInputValueToNumber($('#TxtAmount').val());

    var data = {
        'oldLearning': formacion,
        'newLearning':
        {
            "Id": formacion.Id,
            "CompanyId": Company.Id,
            "Description": $('#TxtName').val(),
            "DateEstimated": dateEstimated,
            "Hours": StringToNumber($('#TxtHours').val()),
            "Amount": ParseInputValueToNumber($('#TxtAmount').val()),
            "Master": $('#TxtMaster').val(),
            "Notes": $('#TxtNotes').val(),
            "Objective": $('#TxtObjetivo').val(),
            "Methodology": $('#TxtMetodologia').val(),
            "Status": status,
            "Year": $('#TxtYear').val(),
            "RealStart": d1,
            "RealFinish": d2
        },
        'newAssistants': newAssistants,
        'userId': user.Id,
        'companyId': Company.Id
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
                //document.location = document.referrer;
                document.location = referrer;
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

    /*$('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: 'ca'
    });*/
    var options = $.extend({}, $.datepicker.regional["ca"], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);

    if (ApplicationUser.ShowHelp === true) {
        SetToolTip('TxtYear', Dictionary.Item_Learning_Help_Year);
        SetToolTip('TxtName', Dictionary.Item_Learning_Help_Nombre);
        SetToolTip('BtnSelectEmpleado', Dictionary.Item_Learning_Help_SelectAllAssistants);
        SetToolTip('DivCmbFechaPrevistaMes', Dictionary.Item_Learning_Help_MesPrevisto);
        SetToolTip('TxtFechaPresvistaMesReadOnly', Dictionary.Item_Learning_Help_MesPrevisto);
        SetToolTip('DivCmbYearPrevisto', Dictionary.Item_Learning_Help_YearPrevisto);
        SetToolTip('TxtFechaPrevistaYear', Dictionary.Item_Learning_Help_YearPrevisto);
        SetToolTip('TxtMaster', Dictionary.Item_Learning_Help_Coach);
        SetToolTip('TxtRealStart', Dictionary.Item_Learning_Help_InicioReal);
        SetToolTip('BtnRealStart', Dictionary.Item_Learning_Help_InicioReal);
        SetToolTip('TxtRealFinish', Dictionary.Item_Learning_Help_FinalReal);
        SetToolTip('BtnRealFinish', Dictionary.Item_Learning_Help_FinalReal);
        SetToolTip('TxtObjetivo', Dictionary.Item_Learning_Help_Objetivo);
        SetToolTip('TxtMetodologia', Dictionary.Item_Learning_Help_Metologia);
        SetToolTip('TxtNotes', Dictionary.Item_Learning_Help_Notas);
        SetToolTip('TxtHours', Dictionary.Item_Learning_FieldLabel_Hours);
        SetToolTip('TxtAmount', Dictionary.Item_Learning_Help_Item_Learning_FieldLabel_Amount);
        $('[data-rel=tooltip]').tooltip();
    }

    if (formacion.Year !== 0) { $('#TxtYear').val(formacion.Year); }
    $('#TxtName').val(formacion.Description);
    //$('#TxtHours').val(formacion.Hours === 0 ? '' : formacion.Hours.toLocaleString(UserCulture, { minimumFractionDigits: 0 }));
    //$('#TxtAmount').val(formacion.Amount === 0 ? '' : formacion.Amount.toLocaleString(UserCulture, { minimumFractionDigits: 2 }));
    $('#TxtHours').val(formacion.Hours === 0 ? '' : StringToNumber(formacion.Hours));
    $('#TxtAmount').val(formacion.Amount === 0 ? '' : ToMoneyFormat(formacion.Amount, 2));
    $('#TxtMaster').val(formacion.Master);
    $('#TxtNotes').val(formacion.Notes);
    $('#TxtObjetivo').val(formacion.Objective);
    $('#TxtMetodologia').val(formacion.Methodology);
    $('#TxtRealStart').val(formacion.RealStart);
    $('#TxtRealFinish').val(formacion.RealFinish);

    if (formacion.Status === 2) {
        document.getElementById('RBStatus3').checked = true;
    }
    if (formacion.Status === 1) {
        document.getElementById('RBStatus2').checked = true;
    }
    if (formacion.Status === 0) {
        document.getElementById('RBStatus1').checked = true;
    }

    var month = formacion.DateEstimated.split('/')[1] * 1;
    var year = formacion.DateEstimated.split('/')[2] * 1;

    if (year > 1) {
        $('#TxtFechaPrevistaYear').val(year);
    }

    if (year > 1 && formacion.Status < 2) {
        var list = document.getElementById('CmbFechaPrevistaMes').childNodes;
        for (var x = 0; x < list.length; x++) {
            if (list[x].tagName * 1 === 'OPTION') {
                if (list[x].value === month) {
                    list[x].selected = true;
                }
                else {
                    list[x].selected = false;
                }
            }
        }
    }

    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;'
            if (("title_html" in this.options) && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));

    $('#TxtHours').css('text-align', 'right');
    $('#TxtAmount').css('text-align', 'right');
    //$('#TxtHours').autoNumeric('init', {aSep: '.', aDec: ',', vMin: '0', vMax: '9999', mDec: '0', lZero: 'deny'});
    //$('#TxtAmount').autoNumeric('init', {aSep: '.', aDec: ',', vMin: '0', vMax: '99999.99', mDec: '2', lZero: 'deny'});

    $('#BtnCancel').on('click', function (e) { document.location = referrer; });
    $('#BtnSave').on('click', Save);

    $("#BtnSelectEmpleado").on('click', function (e) {
        e.preventDefault();

        var target = document.getElementById('SelectableEmployeesTable');
        while (target.childNodes.length > 0) {
            var victim = target.lastChild;
            target.removeChild(victim);
        }

        var openPopup = false;
        for (var x = 0; x < Company.Employees.length; x++) {
            var selectable = true;
            for (var y = 0; y < SelectedEmployees.length; y++) {
                if (SelectedEmployees[y].EmployeeId == Company.Employees[x].Id) {
                    selectable = false;
                    break;
                }
            }

            if (Company.Employees[x].Active === false || Company.Employees[x].DisabledDate !== null) { selectable = false;}

            if (selectable === true) {
                var tr = document.createElement('tr');
                var td1 = document.createElement('td');
                var td2 = document.createElement('td');
                td2.appendChild(document.createTextNode(Company.Employees[x].Name + ' ' + Company.Employees[x].LastName));
                var check = document.createElement('input');
                check.type = 'checkbox';
                check.id = 'chk' + Company.Employees[x].Id;
                td1.appendChild(check);
                tr.appendChild(td1);
                tr.appendChild(td2);
                target.appendChild(tr);
                openPopup = true;
            }
        }

        if (openPopup === false) {
            alertUI(Dictionary.Item_Learning_Error_NoEmeployeesAvailables);
            return false;
        }

        var dialog = $("#assistantDialog").removeClass('hide').css('max-height', '400px').dialog({
            resizable: false,
            modal: true,
            title: Dictionary.Item_Employees,
            title_html: true,
            width: 800,
            'max-height': 400,
            buttons: [
                {
                    id: 'BtnSelect',
                    html: "<i class='icon-ok bigger-110'></i>&nbsp; Afegir",
                    "class": "btn btn-success btn-xs",
                    click: function () {
                        SelectEmployees();
                    }
                },
                {
                    html: "<i class='icon-remove bigger-110'></i>&nbsp; Cancel·lar",
                    "class": "btn btn-xs",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]
        }).css('max-height', '400px');
    });
});

// ISSUS-190
document.getElementById('TxtYear').focus();

// ISSUS-201
if (formacion.Id > 0)
{
    if (document.getElementById('CmbFechaPrevistaMes') !== null) {
        document.getElementById('CmbFechaPrevistaMes').value = formacion.EstimatedMonthId;
    }

    console.log(formacion.EstimatedMonthId);
}
else {
    console.log('nomes');
}

if (typeof user.Grants.Learning === "undefined" || user.Grants.Learning.Write === false) {
    document.getElementById("TxtNotes").disabled = true;
    $("#BtnNewUploadfile").hide();
    $("#BtnSelectEmpleado").hide();
    $("#SelectedEmployeesTable span").disabled = true;
    $(".btn-danger").hide();
    $("input").attr("disabled", true);
    $("textarea").attr("disabled", true);
    $("select").attr("disabled", true);
    $("#BtnSave").hide();
}