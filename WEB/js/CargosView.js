var originalDepartmentId = cargo.Department.Id;
var DepartmentSelected;
var DepartmentUpdatedId;

function DepartmentDelete(sender) {
    document.getElementById("dialogDepartment").parentNode.style.cssText += "z-Index:1039 !important";
    DepartmentSelected = sender.id * 1;
    var department = GetCompanyDepartment(DepartmentSelected);
    $("#DepartmentName").html(department.Name);
    var dialog = $("#DepartmentDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Delete,
        "title_html": true,
        buttons:
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    DepartmentDeleteAction();
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ],
        "close": function () {
            document.getElementById("dialogDepartment").parentNode.style.cssText += "z-Index:1050 !important";
        }
    });
}

function DepartmentDeleteAction(id) {
    var data = { departmentId: DepartmentSelected, companyId: Company.Id, userId: user.Id };
    $("#DepartmentDeleteDialog").dialog("close");
    $.ajax({
        "type": "POST",
        "url": "/Async/DepartmentActions.asmx/DepartmentDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            // Eliminar de la lista de departamentos
            var temp = new Array();
            for (var x = 0; x < departmentsCompany.length; x++) {
                if (departmentsCompany[x].Id !== DepartmentSelected) {
                    temp.push(departmentsCompany[x]);
                }
            }

            departmentsCompany = new Array();
            for (var y = 0; y < temp.length; y++) {
                departmentsCompany.push(temp[y]);
            }

            $("#dialogDepartment").dialog("close");
            ShowDepartmentPopup();
            FillDepartmentCombo();
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function SetDepartment(e) {
    var departmentId = e.target.id.split("-")[1];
    SelectedDepartment = departmentId;
    cargo.Department.Id = departmentId;
    var departmentName = "";

    for (var x = 0; x < departments.length; x++) {
        if (departments[x].Id === departmentId) {
            departmentName = departments[x].Description;
            cargo.Department.Id = departmentId;
            break;
        }
    }

    FillDepartmentCombo();
    $("#TxtDepartmentName").val(departmentName);
    $("#dialogDepartment").dialog("close");
}

function DepartmentAssociationAction(id) {
    SelectedDepartment = id;
    cargo.Department.Id = id;
    FillDepartmentCombo();
    $("#TxtDepartmentName").val(GetCompanyDepartment(id).Description);
    $("#dialogDepartment").dialog("close");
}

function CmbDepartmentChanged() {
    SelectedDepartment = document.getElementById("CmbDepartment").value * 1;
    cargo.Department.Id = SelectedDepartment;
    FillDepartmentCombo();
    if (SelectedDepartment === 0) {
        $("#TxtDepartmentName").val("");
    }
    else {
        $("#TxtDepartmentName").val(GetCompanyDepartment(SelectedDepartment).Description);
    }
}

function getDepartmentName(id) {
    for (var x = 0; x < departmentsCompany.length; x++) {
        if (departmentsCompany[x].Id === id) {
            SelectedDepartment = departmentsCompany[x].Id;
            return departmentsCompany[x].Description;
        }
    }

    return "";
}

function RenderDepartmentsPopup() {
    VoidTable("DepartmentsTablePopup");
    var target = document.getElementById("DepartmentsTablePopup");
    departmentsCompany.sort(CompareDepartments);
    for (var x = 0; x < departmentsCompany.length; x++) {
        CompanyDepartmentRow(departmentsCompany[x], target);
    }
}

function CompanyDepartmentRow(department, target) {
    var selected = department.Id === cargo.Department.Id;
    var tr = document.createElement("tr");
    tr.id = department.Id;

    var td1 = document.createElement("td");
    td1.appendChild(document.createTextNode(department.Description));
    if (selected === true) {
        td1.style.fontWeight = "bold";
    }

    var td2 = document.createElement("td");
    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if (selected === true) {
        span1.onclick = function () { warningInfoUI(Dictionary.Common_Selected, null, null, "dialogDepartment"); };
    }
    else {
        span1.onclick = function () { DepartmentAssociationAction(department.Id); };
    }

    div.appendChild(span1);

    var span2 = document.createElement("span");
    span2.className = "btn btn-xs btn-info";
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement("i");
    i2.className = "icon-edit bigger-120";
    span2.appendChild(i2);
    span2.onclick = function () { DepartmentUpdate(this); };
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span2);

    var span3 = document.createElement("span");
    span3.id = department.Id;
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);

    if (selected === true) {
        span3.onclick = function () { alertUI(Dictionary.Common_ErrorMessage_InUse, "dialogDepartment"); };
    }
    else if (DepartmentHasJobPosition(department.Id)) {
        span3.onclick = function () { alertUI(Dictionary.Item_JobPosition_ErrorMessage_HasJobPositionLinked, "dialogDepartment"); };
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

function SortByName() {
    return function (a, b) {
        if (a["Name"] > b["Name"]) {
            return 1;
        } else if (a["Name"] < b["Name"]) {
            return -1;
        }
        return 0;
    };
}

function ShowDepartmentPopup() {
    RenderDepartmentsPopup();
    var dialog = $("#dialogDepartment").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Departments + "</h4>",
        "title_html": true,
        "width": 800,
        "buttons": [
            {
                "id": "BtnNewDepartmentSave",
                "html": "<i class=\"icon-plus bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    DepartmentInsert();
                }
            },
            {
                "html": "<i class=\"icon-undo bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function ValidateJobPositionForm() {
    var ok = true;
    $("#TxtNameErrorDuplicated").hide();
    if (!RequiredFieldText("TxtName")) { ok = false; }
    else
    {
        var duplicated = false;
        for (var x = 0; x < jobPositionCompany.length; x++) {
            if (jobPositionCompany[x].Description === $("#TxtName").val() && jobPositionCompany[x].Id !== cargo.Id) {
                duplicated = true;
                break;
            }
        }

        if (duplicated === true) {
            ok = false;
            $("#TxtNameLabel").css("color", "#f00");
            $("#TxtNameErrorDuplicated").show();
        }
    }

    if (!RequiredFieldText("TxtDepartmentName")) { ok = false; }
    return ok;
}

jQuery(function ($) {
    $('#TxtDepartmentName').val(getDepartmentName(SelectedDepartment));

    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        "_title": function (title) {
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));

    if (ApplicationUser.ShowHelp === true) {
        SetToolTip("TxtName", Dictionary.Item_JobPosition_Help_Name);
        SetToolTip("DivCmbResponsible", Dictionary.Item_JobPosition_Help_Responsible);
        SetToolTip("DivCmbDepartment", Dictionary.Item_JobPosition_Help_Departamento);
        SetToolTip("BtnDepartment", Dictionary.Item_JobPosition_Help_BARDepartamento);
        SetToolTip("TxtResponsabilidades", Dictionary.Item_JobPosition_Help_Responsabilidades);
        SetToolTip("TxtNotas", Dictionary.Item_JobPosition_Help_Notes);
        SetToolTip("TxtFormacionAcademicaDeseada", Dictionary.Item_JobPosition_Help_Academica);
        SetToolTip("TxtFormacionEspecificaDeseada", Dictionary.Item_JobPosition_Help_Especifica);
        SetToolTip("TxtExperienciaLaboral", Dictionary.Item_JobPosition_Help_Experiencia);
        SetToolTip("TxtHabilidades", Dictionary.Item_JobPosition_Help_Habilidades);
        $("[data-rel=tooltip]").tooltip();
    }

    $("#BtnCancel").click(function (e) {
        document.location = referrer;
    });

    if (cargo.Id > 0) {
        $("#BtnSave").on("click", function (e) {
            if (ValidateJobPositionForm() === false) {
                window.scrollTo(0, 0);
                return false;
            }

            // Puede que no haya responsable
            var ResponsibleItem = null;
            if (SelectedResponsible * 1 > 0) {
                ResponsibleItem = { Id: SelectedResponsible * 1, CompanyId: Company.Id };
            }

            cargo.Department.id = originalDepartmentId;
            var data = {
                "oldJobPositionId": cargo.Id,
                "newJobPosition":
                {
                    "Id": cargo.Id,
                    "Description": $("#TxtName").val(),
                    "CompanyId": Company.Id,
                    "Responsible": ResponsibleItem,
                    "Department": { Id: SelectedDepartment, CompanyId: Company.Id },
                    "Responsibilities": $("#TxtResponsabilidades").val(),
                    "Notes": $("#TxtNotas").val(),
                    "AcademicSkills": $("#TxtFormacionAcademicaDeseada").val(),
                    "SpecificSkills": $("#TxtFormacionEspecificaDeseada").val(),
                    "WorkExperience": $("#TxtExperienciaLaboral").val(),
                    "Habilities": $("#TxtHabilidades").val()
                },
                "userId": user.Id
            };

            $.ajax({
                "type": "POST",
                "url": "/Async/JobPositionActions.asmx/Update",
                "contentType": "application/json; charset=utf-8",
                "dataType": "json",
                "data": JSON.stringify(data, null, 2),
                "success": function (response) {
                    if (response.d.Success === true) {
                        //document.location = document.referrer;
                        document.location = referrer;
                    }
                    if (response.d.Success !== true) {
                        alertUI(response.d.MessageError);
                    }
                },
                "error": function (jqXHR, textStatus, errorThrown) {
                    alert(jqXHR.responseText);
                }
            });
        });
    }
    else {
        $("#BtnSave").on("click", function (e) {
            if (ValidateJobPositionForm() === false) {
                window.scrollTo(0, 0);
                return false;
            }

            // Puede que no haya responsable
            var ResponsibleItem = null;
            if (SelectedResponsible * 1 > 0) {
                ResponsibleItem = { Id: SelectedResponsible * 1, CompanyId: Company.Id };
            }

            cargo.Department.id = originalDepartmentId;
            var data = {
                "newJobPosition":
                {
                    "Id": cargo.Id,
                    "Description": $("#TxtName").val(),
                    "CompanyId": Company.Id,
                    "Responsible": ResponsibleItem,//{ Id: SelectedResponsible, CompanyId: Company.Id },
                    "Department": { Id: SelectedDepartment, CompanyId: Company.Id },
                    "Responsibilities": $("#TxtResponsabilidades").val(),
                    "Notes": $("#TxtNotas").val(),
                    "AcademicSkills": $("#TxtFormacionAcademicaDeseada").val(),
                    "SpecificSkills": $("#TxtFormacionEspecificaDeseada").val(),
                    "WorkExperience": $("#TxtExperienciaLaboral").val(),
                    "Habilities": $("#TxtHabilidades").val()
                },
                "userId": user.Id
            };

            $.ajax({
                "type": "POST",
                "url": "/Async/JobPositionActions.asmx/Insert",
                "contentType": "application/json; charset=utf-8",
                "dataType": "json",
                "data": JSON.stringify(data, null, 2),
                "success": function (response) {
                    if (response.d.Success === true) {
                        document.location = "CargosList.aspx";
                    }
                    if (response.d.Success !== true) {
                        alertUI(response.d.MessageError);
                    }
                },
                "error": function (jqXHR, textStatus, errorThrown) {
                    alert(jqXHR.responseText);
                }
            });
        });
    }

    $("#BtnDepartment").on("click", function (e) {
        e.preventDefault();
        ShowDepartmentPopup();
    });
});

function DepartmentInsert() {
    document.getElementById("dialogDepartment").parentNode.style.cssText += "z-Index:1039 !important";
    $("#TxtDepartmentNewName").val("");
    $("#TxtDepartmentNewNameErrorRequired").hide();
    $("#TxtDepartmentNewNameErrorDuplicated").hide();
    $("#TxtProcessTypeNewName").val("");
    var Selected = 0;
    $("#DepartmentInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Department_PopupAdd_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    $("#TxtDepartmentNewNameErrorRequired").hide();
                    $("#TxtDepartmentNewNameErrorDuplicated").hide();

                    if (!RequiredFieldText("TxtDepartmentNewName")) { ok = false; }
                    else
                    {
                        var duplicated = false;
                        for (var x = 0; x < departmentsCompany.length; x++) {
                            if ($("#TxtDepartmentNewName").val().toLowerCase() === departmentsCompany[x].Description.toLowerCase()) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            $("#TxtDepartmentNewNameErrorDuplicated").show()";
                            ok = false;
                        }
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    $("#TxtDepartmentNewNameErrorRequired").hide();
                    $("#TxtDepartmentNewNameErrorDuplicated").hide();
                    $(this).dialog("close");
                    DepartmentInsertConfirmed($("#TxtDepartmentNewName").val());
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ],
        "close": function () { document.getElementById("dialogDepartment").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function DepartmentInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var description = "";
    var data = {
        "name": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    var newId = 0;
    $.ajax({
        "type": "POST",
        "url": "/Async/DepartmentActions.asmx/DepartmentInsert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                var newDepartment = {
                    "Id": newId,
                    "Description": newDescription,
                    "Active": true,
                    "Deletable": true
                };
                departmentsCompany.push(newDepartment);

                // 3.- Modificar la fila de la tabla del popup
                RenderDepartmentsPopup();
                FillDepartmentCombo();
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            alert(jqXHR.responseText);
        }
    });
}

function CompareDepartments(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function DepartmentUpdate(sender) {
    document.getElementById("dialogDepartment").parentNode.style.cssText += "z-Index:1039 !important";
    var DepartmentUpdatedId = sender.parentNode.parentNode.parentNode.id * 1;
    var department = GetCompanyDepartment(DepartmentUpdatedId);
    $("#TxtDepartmentUpdateName").val(department.Description);
    $("#TxtDepartmentUpdateNameErrorRequired").hide();
    $("#TxtDepartmentUpdateNameErrorDuplicated").hide();
    $("#DepartmentUpdateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": Dictionary.Common_Edit,
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    if (!RequiredFieldText("TxtDepartmentUpdateName")) { ok = false; }
                    else
                    {
                        var duplicated = false;
                        for (var x = 0; x < departmentsCompany.length; x++) {
                            if ($("#TxtDepartmentUpdateName").val().toLowerCase() === departmentsCompany[x].Description.toLowerCase() && departmentsCompany[x].Id !== DepartmentUpdatedId) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            $("#TxtDepartmentUpdateNameErrorDuplicated").show();
                            ok = false;
                        }
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }
                    $("#TxtDepartmentUpdateNameErrorRequired").hide();
                    $("#TxtDepartmentUpdateNameErrorDuplicated").hide();
                    DepartmentUpdateConfirmed(DepartmentUpdatedId, $("#TxtDepartmentUpdateName").val());
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () { document.getElementById("dialogDepartment").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function DepartmentUpdateConfirmed(id, name) {
    // 1.- Modificar en la BBDD
    var data = {
        "departmentId": id,
        "name": name,
        "companyId": Company.Id,
        "userId": user.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/DepartmentActions.asmx/DepartmentUpdate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            alert(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var x = 0; x < departmentsCompany.length; x++) {
        if (departmentsCompany[x].Id !== id) {
            temp.push(departmentsCompany[x]);
        }
        else {
            var item = departmentsCompany[x];
            temp.push({
				"Id": item.Id,
				"Description": name,
				"Active": item.Active,
				"Deletable": item.Delete 
			});
        }
    }

    departmentsCompany = new Array();
    for (var y = 0; y < temp.length; y++) {
        departmentsCompany.push(temp[y]);
    }

    // 3.- Modificar la fila de la tabla del popup
    RenderDepartmentsPopup();
    FillDepartmentCombo();

    // 4.- Modificar el texto si es el seleccionado
    if (SelectedDepartment === id) {
        document.getElementById("TxtDepartmentName").value = name;
    }
}

function FillDepartmentCombo() {
    VoidTable("CmbDepartment");
    if (cargo.Id < 1) {
        var voidOption = document.createElement("option");
        voidOption.value = 0;
        voidOption.appendChild(document.createTextNode(""));
        document.getElementById("CmbDepartment").appendChild(voidOption);
    }

    for (var x = 0; x < departmentsCompany.length; x++) {
        var option = document.createElement("option");
        option.value = departmentsCompany[x].Id;
        option.appendChild(document.createTextNode(departmentsCompany[x].Description));
        if (departmentsCompany[x].Id === SelectedDepartment) {
            option.setAttribute("selected", "selected");
        }
        document.getElementById("CmbDepartment").appendChild(option);
    }
}

FillDepartmentCombo();

if (ApplicationUser.Grants.JobPosition.Write === false) {
    $("#TxtName").attr("disabled", "disabled");
    $("#TxtResponsabilidades").attr("disabled", "disabled");
    $("#TxtNotas").attr("disabled", "disabled");
    $("#TxtFormacionAcademicaDeseada").attr("disabled", "disabled");
    $("#TxtFormacionEspecificaDeseada").attr("disabled", "disabled");
    $("#TxtExperienciaLaboral").attr("disabled", "disabled");
    $("#TxtHabilidades").attr("disabled", "disabled");

    $("#CmbDepartment").attr("disabled", "disabled");
    $("#CmbDepartment").css("background-color", "#f5f5f5");
    $("#CmbResponsible").attr("disabled", "disabled");
    $("#CmbResponsible").css("background-color", "#f5f5f5");

    $("#BtnDepartment").hide();
    $("#BtnSave").hide();
}
else {
    // ISSUS-190
    document.getElementById("TxtName").focus();
}