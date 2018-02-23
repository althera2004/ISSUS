function SaveSkill()
{
    var webMethod = "/Async/EmployeeActions.asmx/EmployeeSkillsInsert";
    var data = { 
        skills: {
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

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
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
    document.getElementById("TxtNombreErrorDuplicated").style.display = "none";
    document.getElementById("TxtApellido1ErrorDuplicated").style.display = "none";
    document.getElementById("TxtEmailErrorDuplicated").style.display = "none";

    if(RequiredFieldText("TxtNombre") === false) { ok = false; }
    if(RequiredFieldText("TxtApellido1") === false) { ok = false; }
    var nifError = false;
    if(document.getElementById("TxtNif").value !== "" && country === "España")
    {
        if(MalFormedNif("TxtNif") === false){ nifError = true; ok = false; }                    
    }
    else
    {
        document.getElementById("TxtNifLabel").style.color = "#000";
        document.getElementById("TxtNifErrorMalformed").style.display = "none";
    }

    if(document.getElementById("TxtNif").value !== "")
    {
        for(var x=0; x<Company.Employees.length;x++)
        {
            if(Company.Employees[x].Nif === document.getElementById("TxtNif").value && Company.Employees[x].Id !== employeeId && Company.Employees[x].Active === true)
            {
                document.getElementById("TxtNifErrorDuplicated").style.display = "block";                            
                document.getElementById("TxtNifErrorDuplicated").innerHTML = Dictionary.Item_Employee_ErrorMessage_NifAlreadyExists + ":<br />" + Company.Employees[x].Name + " " + Company.Employees[x].LastName;
                document.getElementById("TxtNifLabel").style.color = "#f00";
                ok = false;
                break;
            }
            else
            {
                document.getElementById("TxtNifErrorDuplicated").style.display = "none";
                if(errorNif === false)
                {
                    document.getElementById("TxtNifLabel").style.color = "#000";
                }
            }
        }
    }
    else
    {
        document.getElementById("TxtNifErrorDuplicated").style.display = "none";
        if(errorNif === false)
        {
            document.getElementById("TxtNifLabel").style.color = "#000";
        }
    }

    if(document.getElementById("TxtNombre").value !== "" &&
       document.getElementById("TxtApellido1").value !== "" &&
       document.getElementById("TxtEmail").value !== "")
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
            document.getElementById("TxtNombreErrorDuplicated").style.display = "block";
            document.getElementById("TxtApellido1ErrorDuplicated").style.display = "block";
            document.getElementById("TxtEmailErrorDuplicated").style.display = "block";
            document.getElementById("TxtNombreLabel").style.color = "#f00";
            document.getElementById("TxtApellido1Label").style.color = "#f00";
            document.getElementById("TxtEmailLabel").style.color = "#f00";
            ok = false;
        }
        else
        {
            document.getElementById("TxtNombreErrorDuplicated").style.display = "none";
            document.getElementById("TxtApellido1ErrorDuplicated").style.display = "none";
            document.getElementById("TxtEmailErrorDuplicated").style.display = "none";
        }
    }

    //if(RequiredFieldText("TxtTelefono") === false) { ok = false; }
    if(RequiredFieldText("TxtEmail") === false) { ok = false; }
    else
    {
        if(MalFormedEmail("TxtEmail") === false) { ok = false; }
    }
    //if(RequiredFieldText("TxtDireccion") === false) { ok = false; }
    //if(RequiredFieldText("TxtCp") === false) { ok = false; }
    //if(RequiredFieldText("TxtPoblacion") === false) { ok = false; }
    //if(RequiredFieldText("TxtProvincia") === false) { ok = false; }
    //if(RequiredFieldText("TxtPais") === false) { ok = false; }
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
        "error": function (jqXHR, textStatus, errorThrown) {
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
        var location = document.location + '';
        if (location.indexOf('&New=true') !== -1) {
            document.location = 'EmployeesList.aspx';
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
    var options = $.extend({}, $.datepicker.regional["ca"], { autoclose: true, todayHighlight: true });
    $(".date-picker").datepicker(options);
    $(".date-picker").on("blur", function () { DatePickerChanged(this); });
});            

function RenderDepartments()
{
    departmentsCompany = departmentsCompany.sort(SortByName);
    departmentsEmployee = departmentsEmployee.sort(SortByName);
    var target = document.getElementById("DeparmentsEmployee");
    target.innerHTML = "";
    for(var x=0; x<departmentsEmployee.length;x++)
    {
        EmployeeDepartmentRow(departmentsEmployee[x], target)
    }
}

function SortByName()
{
    return function (a, b) {
        if( a['Name'] > b['Name']){
            return 1;
        }else if( a['Name'] < b['Name'] ){
            return -1;
        }
        return 0;
    }
}

function CompanyDepartmentsAdd(id, name)
{
    departmentsCompany.push({Id:id, Name:name});
}

function EmployeeDepartmentsAdd(id)
{
    var name="";
    for(var x=0; x<departmentsCompany.length;x++)
    {
        if(departmentsCompany[x].Id === id)
        {
            name = departmentsCompany[x].Name;
        }
    }

    if(name!=="")
    {
        departmentsEmployee.push({Id:id,Name:name});
    }
}

function EmployeeDepartmentsDelete(id)
{
    var temp = new Array();
    for(var x=0; x<departmentsEmployee.length;x++)
    {
        if(departmentsEmployee[x].Id !== id)
        {
            temp.push(departmentsEmployee[x]);
        }
    }

    departmentsEmployee = temp;
}

RenderDeparmentsEmployee();

if(SecurityGroups.Administration === true)
{
    document.getElementById("chkAdministration").checked = true;
}

if(SecurityGroups.Process === true)
{
    document.getElementById("chkProcess").checked = true;
}

if(SecurityGroups.Documents === true)
{
    document.getElementById("chkDocuments").checked = true;
}

if(SecurityGroups.Learning === true)
{
    document.getElementById("chkLearning").checked = true;
}

var newId;
function Reload()
{
    document.location = 'EmployeesView.aspx?id=' + newId + "&New=true";
}

function EmployeeDeleteAlertNo() {
    return false;
}

function EmployeeDeleteAlertYes() {
    document.location = 'EmployeeSubstitution.aspx?id=' + EmployeeDeleteId + '&enddate=' + FormatDate(GetDate(document.getElementById('TxtEndDate').value, '/'), '');
}

// ISSUS-190
document.getElementById('TxtNombre').focus();

function DeleteJobPosition(id, name) {
    $('#JobPositionName').html(name);
    JobPositionSelected = id;
    var dialog = $("#JobPositionDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">&nbsp;' + Dictionary.Common_Warning + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    JobPositionDeleteAction();
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

var JobPositionSelected;
function JobPositionDeleteAction() {
    console.log('JobPositionDeleteAction');
    var webMethod = "/Async/EmployeeActions.asmx/DeleteJobPosition";
    var data = { employeeId: employeeId, jobPositionId: JobPositionSelected };
    $("#JobPositionDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            HideJobPositionRow(jobPositionEmployee);
        },
        error: function (msg) {
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
    document.getElementById("TxtNombre").disabled = true;
    document.getElementById("TxtApellido1").disabled = true;
    document.getElementById("TxtNif").disabled = true;
    document.getElementById("TxtTelefono").disabled = true;
    document.getElementById("TxtEmail").disabled = true;
    document.getElementById("TxtDireccion").disabled = true;
    document.getElementById("TxtCp").disabled = true;
    document.getElementById("TxtPoblacion").disabled = true;
    document.getElementById("TxtProvincia").disabled = true;
    document.getElementById("TxtNotas").disabled = true;
    if (document.getElementById("TxtEndDate") !== null) {
        document.getElementById("TxtEndDate").disabled = true;
    }

    if (document.getElementById("CmbPais") !== null) {
        document.getElementById("CmbPais").disabled = true;
    }

    document.getElementById("TxtAcademic").readOnly = true;
    document.getElementById("TxtSpecific").readOnly = true;
    document.getElementById("TxtWorkExperience").readOnly = true;
    document.getElementById("TxtHability").readOnly = true;

    if (document.getElementById("WorkExperienceValidYes") !== null) {
        document.getElementById("WorkExperienceValidYes").disabled = true;
        document.getElementById("WorkExperienceValidNo").disabled = true;
        document.getElementById("HabilityValidYes").disabled = true;
        document.getElementById("HabilityValidNo").disabled = true;
        document.getElementById("AcademicValidYes").disabled = true;
        document.getElementById("AcademicValidNo").disabled = true;
        document.getElementById("SpecificValidYes").disabled = true;
        document.getElementById("SpecificValidNo").disabled = true;
    }

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
        resizable: false,
        modal: true,
        title: Dictionary.Item_Employee_PopupAnular_Title,
        width: 600,
        buttons:
        [
            {
                "id": "BtnAnularSave",
                "html": "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Item_Employee_Btn_Inactive,
                "class": "btn btn-success btn-xs",
                "click": function () { AnularConfirmed(); }
            },
            {
                "html": "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
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
        res += "    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Dictionary.Item_Employee_Label_InactiveDate + ": <strong>" + employee.DisabledDate + "</strong></p></div>";
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

    if (WorkExperienceValidYes !== null) {
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
}

function EmployeeDeleteAlert(id, description) {
    EmployeeDeleteId = id;
    promptInfoUI(Dictionary.Item_Employee_Message_InactivateWarning, 300, EmployeeDeleteAlertYes, null);
    return false;
}