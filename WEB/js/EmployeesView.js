function SaveSkill()
{
    var data = { 
        skills: {
            "Id": 0,
            "Employee": employee,
            "Academic": $("#TxtAcademic").val().trim(),
            "Specific": $("#TxtSpecific").val().trim(),
            "WorkExperience": $("#TxtWorkExperience").val().trim(),
            "Ability": $("#TxtHability").val().trim(),
            "AcademicValid": document.getElementById('AcademicValidYes').checked ? true : (document.getElementById('AcademicValidNo').checked ? false : null),
            "SpecificValid": document.getElementById('SpecificValidYes').checked ? true : (document.getElementById('SpecificValidNo').checked ? false : null),
            "WorkExperienceValid": document.getElementById('WorkExperienceValidYes').checked ? true : (document.getElementById('WorkExperienceValidNo').checked ? false : null),
            "AbilityValid": document.getElementById('HabilityValidYes').checked ? true : (document.getElementById('HabilityValidNo').checked ? false : null)
        },
        userId: user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EmployeeActions.asmx/EmployeeSkillsInsert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            document.location = referrer;
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function UpdateSkill()
{
    var data = { 
        oldSkills: employeeSkills,
        newSkills: {
            "Id": 0,
            "Employee": employee,
            "Academic": document.getElementById('TxtAcademic').value.trim(),
            "Specific": document.getElementById('TxtSpecific').value.trim(),
            "WorkExperience": document.getElementById('TxtWorkExperience').value.trim(),
            "Ability": document.getElementById('TxtHability').value.trim(),
            "AcademicValid": document.getElementById('AcademicValidYes').checked ? true : (document.getElementById('AcademicValidNo').checked ? false : null),
            "SpecificValid": document.getElementById('SpecificValidYes').checked ? true : (document.getElementById('SpecificValidNo').checked ? false : null),
            "WorkExperienceValid": document.getElementById('WorkExperienceValidYes').checked ? true : (document.getElementById('WorkExperienceValidNo').checked ? false : null),
            "AbilityValid": document.getElementById('HabilityValidYes').checked ? true : (document.getElementById('HabilityValidNo').checked ? false : null)
        },
        userId: user.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/EmployeeActions.asmx/EmployeeSkillsUpdate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = referrer;
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ValidateForm()
{
    var ok = true;
    var errorNif = false;
    var ddData = $("#CmbPais").data("ddslick");
    var country = ddData.selectedData.value;
    $("#TxtNombreErrorDuplicated").hide();
    $("#TxtApellido1ErrorDuplicated").hide();
    $("#TxtEmailErrorDuplicated").hide();

    if(RequiredFieldText("TxtNombre") === false) { ok = false; }
    if(RequiredFieldText("TxtApellido1") === false) { ok = false; }
    var nifError = false;
    if($("#TxtNif").val() !== "" && country === "España")
    {
        if(MalFormedNif("TxtNif") === false){ nifError = true; ok = false; }                    
    }
    else
    {
        $("#TxtNifLabel").css("color", "#000");
        $("#TxtNifErrorMalformed").hide();
    }

    if($("#TxtNif").val() !== "")
    {
        for (var x = 0; x < Company.Employees.length; x++) {
            if (Company.Employees[x].Nif === document.getElementById("TxtNif").value && Company.Employees[x].Id !== employeeId && Company.Employees[x].Active === true) {
                document.getElementById("TxtNifErrorDuplicated").style.display = "block";
                document.getElementById("TxtNifErrorDuplicated").innerHTML = Dictionary.Item_Employee_ErrorMessage_NifAlreadyExists + ":<br />" + Company.Employees[x].Name + " " + Company.Employees[x].LastName;
                document.getElementById("TxtNifLabel").style.color = "#f00";
                ok = false;
                break;
            }
            else {
                $("#TxtNifErrorDuplicated").hide();
                if (errorNif === false) {
                    $("#TxtNifLabel").css("color", "#000");
                }
            }
        }
    }
    else
    {
        $("#TxtNifErrorDuplicated").hide();
        if(errorNif === false)
        {
            $("#TxtNifLabel").css("color", "#000");
        }
    }

    if($("#TxtNombre").val() !== "" &&
       $("#TxtApellido1").val() !== "" &&
       $("#TxtEmail").val() !== "")
    {
        var foundEmployee=false;
        for(var y=0; y<Company.Employees.length;y++)
        {
            if(Company.Employees[y].Name.toLowerCase() === document.getElementById("TxtNombre").value.toLowerCase() &&
               Company.Employees[y].LastName.toLowerCase() === document.getElementById("TxtApellido1").value.toLowerCase() &&
               Company.Employees[y].Email.toLowerCase() === document.getElementById("TxtEmail").value.toLowerCase() &&
               Company.Employees[y].Id !== employeeId &&
               Company.Employees[y].Active === true)
            {
                foundEmployee = true;
                break;
            }
        }

        if(foundEmployee === true)
        {
            $("#TxtNombreErrorDuplicated").show();
            $("#TxtApellido1ErrorDuplicated").show();
            $("#TxtEmailErrorDuplicated").show();
            $("#TxtNombreLabel").css("color", "#f00");
            $("#TxtApellido1Label").css("color", "#f00");
            $("#TxtEmailLabel").css("color", "#f00");
            ok = false;
        }
        else
        {
            $("#TxtNombreErrorDuplicated").hide();
            $("#TxtApellido1ErrorDuplicated").hide();
            $("#TxtEmailErrorDuplicated").hide();
        }
    }

    if(RequiredFieldText("TxtEmail") === false) { ok = false; }
    else
    {
        if(MalFormedEmail("TxtEmail") === false) { ok = false; }
    }
    return ok;
}

function Restore()
{
    var data = {
        "employeeId": employee.Id,
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
        SetToolTip("TxtNombre", Dictionary.Item_Employee_Help_Nombre);
        SetToolTip("TxtApellido1", Dictionary.Item_Employee_Help_LastName);
        SetToolTip("TxtNif", Dictionary.Item_Employee_Help_Nif);
        SetToolTip("TxtTelefono", Dictionary.Item_Employee_Help_Telefono);
        SetToolTip("TxtEmail", Dictionary.Item_Employee_Help_Email);
        SetToolTip("TxtDireccion", Dictionary.Item_Employee_Help_Direccion);
        SetToolTip("TxtCp", Dictionary.Item_Employee_Help_PostalCode);
        SetToolTip("TxtPoblacion", Dictionary.Item_Employee_Help_Poblacion);
        SetToolTip("TxtProvincia", Dictionary.Item_Employee_Help_Provincia);
        SetToolTip("DivCmbPais", Dictionary.Item_Employee_Help_Pais);
        SetToolTip("TxtNotas", Dictionary.Item_Employee_Help_Notes);
        SetToolTip("TxtAcademic", Dictionary.Item_Employee_Help_Academic);
        SetToolTip("AcademicValidYes", Dictionary.Item_Employee_Help_AcademicValidYes);
        SetToolTip("AcademicValidNo", Dictionary.Item_Employee_Help_AcademicValidNo);
        SetToolTip("TxtSpecific", Dictionary.Item_Employee_Help_Especific);
        SetToolTip("SpecificValidYes", Dictionary.Item_Employee_Help_EspecificValidYes);
        SetToolTip("SpecificValidNo", Dictionary.Item_Employee_Help_EspecificValidNo);
        SetToolTip("TxtWorkExperience", Dictionary.Item_Employee_Help_Experience);
        SetToolTip("WorkExperienceValidYes", Dictionary.Item_Employee_Help_ExperienceValidYes);
        SetToolTip("WorkExperienceValidNo", Dictionary.Item_Employee_Help_ExperienceValidNo);
        SetToolTip("TxtHability", Dictionary.Item_Employee_Help_Abilities);
        SetToolTip("HabilityValidYes", Dictionary.Item_Employee_Help_AbilitiesValidYes);
        SetToolTip("HabilityValidNo", Dictionary.Item_Employee_Help_AbilitiesValidNo);
        if (document.getElementById("TxtEndDate") !== null) {
            if (employeeId === ApplicationUser.Employee.Id) {
                SetToolTip("TxtEndDate", Dictionary.Item_Employee_Help_DisactivateDateSelfLocked);
                SetToolTip("BtnEndDate", Dictionary.Item_Employee_Help_DisactivateDateSelfLocked);
                document.getElementById("TxtEndDate").disabled = true;
            }
            else {
                SetToolTip("TxtEndDate", Dictionary.Item_Employee_Help_DisactivateDate);
                SetToolTip("BtnEndDate", Dictionary.Item_Employee_Help_DisactivateDate);
            }
        }
        SetToolTip("TxtUserName", Dictionary.Item_Employee_Help_UserName);
        SetToolTip("BtnChangeUserName", Dictionary.Item_Employee_Help_BtnUserName);
        SetToolTip("Button1", Dictionary.Item_Employee_Help_ResetPassword);
        SetToolTip("chkAdministration", Dictionary.Item_Employee_Help_PermisoAdministracion);
        SetToolTip("chkProcess", Dictionary.Item_Employee_Help_PermisoProceso);
        SetToolTip("chkDocuments", Dictionary.Item_Employee_Help_PermisoDocumentos);
        SetToolTip("chkLearning", Dictionary.Item_Employee_Help_PermisoFormacion);
        SetToolTip("BtnNewJobPosition", Dictionary.Item_Employee_Help_Btn_Link);

        $("[data-rel=tooltip]").tooltip();
    }

    $("#BtnRestore").on("click", Restore);
    $("#BtnCancel").on("click", Cancel);
    $("#BtnCancelFormacion").on("click", Cancel);
    $("#BtnCancelInternalLearning").on("click", Cancel);

    function Cancel() {
        var location = document.location + "";
        if (location.indexOf("&New=true") !== -1) {
            document.location = "EmployeesList.aspx";
        }
        else {
            document.location = referrer;
        }
    }

    $("#BtnSave").on("click", SaveEmployee);
    $("#BtnSaveFormacion").on("click", SaveEmployee);
    $("#BtnSaveInternalLearning").on("click", SaveEmployee);
    $("#BtnAnular").on("click", AnularPopup);

    function SaveEmployee() {
        if (ValidateForm() === false) {
            window.scrollTo(0, 0);
            return false;
        }

        var ddData = $("#CmbPais").data("ddslick");
        var country = ddData.selectedData.value;

        if (employeeId < 1) {
            var webMethod = "/Async/EmployeeActions.asmx/Insert";
            var data = {
                "newEmployee":
                {
                    "Id": employee.Id,
                    "Name": $("#TxtNombre").val(),
                    "LastName": $('#TxtApellido1').val(),
                    "Nif": $('#TxtNif').val(),
                    "Email": $('#TxtEmail').val(),
                    "Phone": $('#TxtTelefono').val(),
                    "Address":
                    {
                        "Address": $('#TxtDireccion').val(),
                        "PostalCode": $('#TxtCp').val(),
                        "City": $('#TxtPoblacion').val(),
                        "Province": $('#TxtProvincia').val(),
                        "Country": country
                    },
                    "CompanyId": Company.Id,
                    "Notes": $('#TxtNotas').val()
                },
                "userId": user.Id
            };

            LoadingShow(Dictionary.Common_Message_Saving);
            $.ajax({
                "type": "POST",
                "url": webMethod,
                "contentType": "application/json; charset=utf-8",
                "dataType": "json",
                "data": JSON.stringify(data, null, 2),
                "success": function (response) {
                    LoadingHide();
                    if (response.d.Success === true) {
                        newId = response.d.MessageError.split('|')[0] * 1;
                        alertInfoUI(Dictionary.Item_Employee_Message_InsertSucess, Reload);

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
        else {
            var endDate = null;
            var CanSave = false;
            /*if(document.getElementById('TxtEndDate').value !== '')
            {
                endDate = GetDate($('#TxtEndDate').val(), "/", false);
                if(HasActions)
                {
                    EmployeeDeleteAlert(employee.Id, $('#TxtNombre').val() + ' ' + $('#TxtApellido1').val());
                }
                else {
                    SaveEmployeeConfirmed();
                }
            }
            else {*/
                SaveEmployeeConfirmed();
            /*}*/
        }
    }

    function SaveEmployeeConfirmed(){
        var webMethod = "/Async/EmployeeActions.asmx/Update";
        var ddData = $('#CmbPais').data('ddslick');
        var country = ddData.selectedData.value * 1;
        var data = {
            "oldEmployee": employee,
            "newEmployee":
            {
                "Id": employee.Id,
                "Name": $('#TxtNombre').val(),
                "LastName": $('#TxtApellido1').val(),
                "Nif": $('#TxtNif').val(),
                "Email": $('#TxtEmail').val(),
                "Phone": $('#TxtTelefono').val(),
                "Address":
                {
                    "Address": $('#TxtDireccion').val(),
                    "PostalCode": $('#TxtCp').val(),
                    "City": $('#TxtPoblacion').val(),
                    "Province": $('#TxtProvincia').val(),
                    "Country": country
                },
                "CompanyId": Company.Id,
                "Notes": $('#TxtNotas').val(),
                "DisabledDate": GetDate($('#TxtEndDate').val(),'-', false)
            },
            "userId": user.Id
        };

        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (response) {
                if (response.d.Success === true) {
                    LoadingHide();
                    if (employeeSkills.Id === 0) {
                        SaveSkill();
                    }
                    else {
                        UpdateSkill();
                    }
                }
                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                LoadingHide();
                alert(jqXHR.responseText);
            }
        });
    }

    $("#BtnNewJobPosition").on("click", function (e) {
        e.preventDefault();
        ShowJobPositionAsociationPopup();
    });

    $("#DepartmentNewAsociationBtn").on("click", function (e) {
        e.preventDefault();
        DeparmentNewPopup();
    });

    //$("#DepartmentAssociationBtn").on("click", function (e) { ShowDepartmentPopup(); });

    var i = 0;
    for(var x=0; x<ddData.length;x++)
    {
        if (ddData[x].value === employee.Address.Country) { i = x; break; }
        console.log("pais", ddData[x]);
    }

    $('#CmbPais').ddslick({data: ddData});
    $('#CmbPais').ddslick('select', {index: i });

    if(ApplicationUser.ShowHelp===true){
        $('#DivCmbPais .dd-options').on('mouseover', function(e) { $( "#DivCmbPais" ).tooltip( "destroy" ); }); 
        $('#DivCmbPais .dd-options').on('mouseout', function(e) { SetToolTip('DivCmbPais',Dictionary.Item_Employee_Help_Pais); }); 
    }

    /*$('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: "ca"
    })*/
    var options = $.extend({}, $.datepicker.regional[user.Language], { "autoclose": true, "todayHighlight": true });
    $(".date-picker").datepicker(options);
    $(".date-picker").on("blur", function () { DatePickerChanged(this); });
});            

function RenderDepartments() {
    departmentsCompany = departmentsCompany.sort(SortByName);
    departmentsEmployee = departmentsEmployee.sort(SortByName);
    var target = document.getElementById("DeparmentsEmployee");
    target.innerHTML = "";
    for (var x = 0; x < departmentsEmployee.length; x++) {
        EmployeeDepartmentRow(departmentsEmployee[x], target)
    }
}

function SortByName() {
    return function (a, b) {
        if (a['Name'] > b['Name']) {
            return 1;
        } else if (a['Name'] < b['Name']) {
            return -1;
        }
        return 0;
    }
}

function CompanyDepartmentsAdd(id, name) {
    departmentsCompany.push({ Id: id, Name: name });
}

function EmployeeDepartmentsAdd(id) {
    var name = "";
    for (var x = 0; x < departmentsCompany.length; x++) {
        if (departmentsCompany[x].Id === id) {
            name = departmentsCompany[x].Name;
        }
    }

    if (name !== "") {
        departmentsEmployee.push({ "Id": id, "Name": name });
    }
}

function EmployeeDepartmentsDelete(id) {
    var temp = new Array();
    for (var x = 0; x < departmentsEmployee.length; x++) {
        if (departmentsEmployee[x].Id !== id) {
            temp.push(departmentsEmployee[x]);
        }
    }

    departmentsEmployee = temp;
}

RenderDeparmentsEmployee();

if (SecurityGroups.Administration === true) {
    document.getElementById("chkAdministration").checked = true;
}

if (SecurityGroups.Process === true) {
    document.getElementById("chkProcess").checked = true;
}

if (SecurityGroups.Documents === true) {
    document.getElementById("chkDocuments").checked = true;
}

if (SecurityGroups.Learning === true) {
    document.getElementById("chkLearning").checked = true;
}

var newId;
function Reload() {
    document.location = "EmployeesView.aspx?id=" + newId + "&New=true";
}

function EmployeeDeleteAlertNo() {
    return false;
}

function EmployeeDeleteAlertYes() {
    document.location = 'EmployeeSubstitution.aspx?id=' + EmployeeDeleteId + '&enddate=' + FormatDate(GetDate(document.getElementById('TxtEndDate').value, '/'), '');
}

// ISSUS-190
document.getElementById("TxtNombre").focus();

function DeleteJobPosition(id, name) {
    $("#JobPositionName").html(name);
    JobPositionSelected = id;
    var dialog = $("#JobPositionDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">&nbsp;" + Dictionary.Common_Warning + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    JobPositionDeleteAction();
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function GetJobPositionById(id) {
    for (var x = 0; x < JobPositionCompany.length; x++) {
        if (JobPositionCompany[x].Id === id) {
            return JobPositionCompany[x];
        }
    }

    return null;
}

function UpdateSkillProfile() {
    console.log("UpdateSkillProfile");
    var academic = "";
    var specified = "";
    var experience = "";
    var habilities = "";

    for (var x = 0; x < jobPositionEmployee.length; x++) {
        if (jobPositionEmployee[x].EndDate !== true) {
            var jobPosition = GetJobPositionById(jobPositionEmployee[x].Id);
            if (jobPosition !== null) {
                academic += jobPosition.AcademicSkills + "\n";
                specified += jobPosition.SpecificSkills + "\n";
                experience += jobPosition.WorkExperience + "\n";
                habilities += jobPosition.Abilities + "\n";
            }
        }
    }

    $("#TxtJobPositionAcademic").html(academic);
    $("#TxtJobPositionSpecific").html(specified);
    $("#TxtJobPositionWorkExperience").html(experience);
    $("#TxtJobPositionHability").html(habilities);
}

var JobPositionSelected;
function JobPositionDeleteAction() {
    var data = {
        "employeeId": employeeId,
        "jobPositionId": JobPositionSelected
    };
    $("#JobPositionDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/EmployeeActions.asmx/DeleteJobPosition",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            HideJobPositionRow(jobPositionEmployee);
            UpdateSkillProfile();
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function HideJobPositionRow(list)
{
    var victim = document.getElementById(JobPositionSelected);
    victim.parentNode.removeChild(victim);

    var temp = new Array();
    for (var x = 0; x < list.length; x++)
    {
        if (list[x].Id !== JobPositionSelected) {
            temp.push(list[x]);
        }
    }

    jobPositionEmployee = new Array();
    for (var y = 0; y < temp.length; y++) {
        jobPositionEmployee.push(temp[y]);
    }
}

console.log("ApplicationUser.Grants.Employee.Write", ApplicationUser.Grants.Employee.Write);
if (ApplicationUser.Grants.Employee.Write === false) {
    $("#TxtNombre").attr("disabled", "disabled");
    $("#TxtApellido1").attr("disabled", "disabled");
    $("#TxtNif").attr("disabled", "disabled");
    $("#TxtTelefono").attr("disabled", "disabled");
    $("#TxtEmail").attr("disabled", "disabled");
    $("#TxtDireccion").attr("disabled", "disabled");
    $("#TxtCp").attr("disabled", "disabled");
    $("#TxtPoblacion").attr("disabled", "disabled");
    $("#TxtProvincia").attr("disabled", "disabled");
    $("#TxtNotas").attr("disabled", "disabled");
    $("#TxtEndDate").attr("disabled", "disabled");
    $("#CmbPais").attr("disabled", "disabled");

    $("#TxtAcademic").attr("readOnly", "readonly");
    $("#TxtSpecific").attr("readOnly", "readonly");
    $("#TxtWorkExperience").attr("readOnly", "readonly");
    $("#TxtHability").attr("readOnly", "readonly");

    $("#WorkExperienceValidYes").attr("disabled", "disabled");
    $("#WorkExperienceValidNo").attr("disabled", "disabled");
    $("#HabilityValidYes").attr("disabled", "disabled");
    $("#HabilityValidNo").attr("disabled", "disabled");
    $("#AcademicValidYes").attr("disabled", "disabled");
    $("#AcademicValidNo").attr("disabled", "disabled");
    $("#SpecificValidYes").attr("disabled", "disabled");
    $("#SpecificValidNo").attr("disabled", "disabled");

    $("#BtnEndDate").hide();
    $("#BtnNewJobPosition").hide();
    $("#BtnSave").hide();
    $("#BtnSaveFormacion").hide();
    $("#BtnSaveInternalLearning").hide();
    $(".btn-warning").hide();

    $("#BtnNewUploadfile").hide();
    $("#UploadFilesContainer .btn-danger").hide();
    $("#UploadFilesList .btn-danger").hide();
}

function AnularPopup() {
    if (employeeId === ApplicationUser.Employee.Id) {
        warningInfoUI(Dictionary.Item_Employee_Error_AutoDelete, null, 400);
        return false;
    }

    if (HasActions) {
        EmployeeDeleteAlert(employee.Id, $('#TxtNombre').val() + ' ' + $('#TxtApellido1').val());
        return false;
    }

    $("#TxtEndDate").val(FormatDate(new Date(), "/"));
    var dialog = $("#dialogAnular").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Employee_PopupAnular_Title,
        "width": 600,
        "buttons":
        [
            {
                "id": "BtnAnularSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_Employee_Btn_Inactive,
                "class": "btn btn-success btn-xs",
                "click": function () { AnularConfirmed(); }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

var anulationData = null;
function AnularConfirmed() {
    console.log("AnularConfirmed");
    var ok = true;
    $("#TxtEndDateLabel").css("color", "#000");
    $("#TxtEndDateErrorRequired").hide();
    $("#TxtEndDateMalformed").hide();
    
    if ($("#TxtEndDate").val() === "") {
        ok = false;
        $("#TxtEndDateLabel").css("color", "#f00");
        $("#TxtEndDateErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtEndDate").val()) === false) {
            ok = false;
            $("#TxtEndDateLabel").css("color", "#f00");
            $("#TxtEndDateMalformed").show();
        }
    }

    if (ok === false) {
        return false;
    }

    //Anulate(int indicadorId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
    var webMethod = "/Async/EmployeeActions.asmx/Disable";
    var data = {
        "employeeId": employeeId,
        "companyId": Company.Id,
        "endDate": GetDate($("#TxtEndDate").val(), "/"),
        "userId": user.Id
    };
    anulationData = data;
    $("#dialogAnular").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            document.location = referrer;
            //AnulateLayout();
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

window.onload = function () {
    if (employee.DisabledDate !== null) {
        res = "";
        res += "<br /><div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
        res += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
        res += "    <h3 style=\"display:inline;\">" + Dictionary.Item_Employee_Label_InactiveTitle + "</h3><br />";
        res += "    <p style=\"margin-left:50px;\">";
        //res += "    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Dictionary.Item_Employee_Label_InactiveDate + ": <strong>" + employee.DisabledDate + "</strong></p></div>";
        res += "    " + Dictionary.Item_Employee_Label_InactiveDate + ": <strong>" + employee.DisabledDate + "</strong>";
        res += "    </p>";
        res += "</div>";
        $("#oldFormFooter").before(res);
    }

    if (document.getElementById("AcademicValidYes") !== null) {
        if (SkillAcademicValid !== null) {
            if (SkillAcademicValid === true) {
                document.getElementById("AcademicValidYes").checked = true;
            }
            else {
                document.getElementById("AcademicValidNo").checked = true;
            }
        }
    }

    if (document.getElementById("SpecificValidYes") !== null) {
        if (SkillSpecificValid !== null) {
            if (SkillSpecificValid === true) {
                document.getElementById("SpecificValidYes").checked = true;
            }
            else {

                document.getElementById("SpecificValidNo").checked = true;
            }
        }
    }

    if (document.getElementById("WorkExperienceValidYes") !== null) {
        if (SkillWorkExperienceValid !== null) {
            if (SkillWorkExperienceValid === true) {
                document.getElementById("WorkExperienceValidYes").checked = true;
            }
            else {

                document.getElementById("WorkExperienceValidNo").checked = true;
            }
        }
    }

    if (document.getElementById("HabilityValidYes") !== null) {
        if (SkillHabilityValid !== null) {
            if (SkillHabilityValid === true) {
                document.getElementById("HabilityValidYes").checked = true;
            }
            else {

                document.getElementById("HabilityValidNo").checked = true;
            }
        }
    }

    UpdateSkillProfile();
};

function EmployeeDeleteAlert(id, description) {
    EmployeeDeleteId = id;
    promptInfoUI(Dictionary.Item_Employee_Message_InactivateWarning, 300, EmployeeDeleteAlertYes, null);
    return false;
}