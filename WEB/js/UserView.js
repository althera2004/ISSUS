function GrantChanged(mode, id, sender) {
    var grants = $("#Contentholder1_Grants").val();

    var tag = mode + id + "|";

    if (sender.checked === true) {
        grants += tag;
    }
    else {
        grants = grants.split(tag).join("");
    }

    if (mode === "R" && !sender.checked) {
        document.getElementById("CheckboxWrite" + id).checked = false;
        grants = grants.split("W" + id + "|").join("");
    }

    if (mode === "W" && sender.checked) {
        document.getElementById("CheckboxRead" + id).checked = true;
        grants += "R" + id + "|";
    }

    document.getElementById("Contentholder1_Grants").value = grants;
    TestCBAll();
}

function SaveGrants() {
    var calculatedGrants = "|";
    for (var x = 0; x < CBR.length; x++) {
        if (CBR[x].checked === true) {
            calculatedGrants += "R" + CBR[x].id.substring(12) + "|";
        }
    }

    for (var y = 0; y < CBW.length; y++) {
        if (CBW[y].checked === true) {
            calculatedGrants += "W" + CBW[y].id.substring(13) + "|";
        }
    }

    var data = {
        "grants": calculatedGrants,
        "itemUserId": itemUser.Id,
        "userId": ApplicationUser.Id
    };
    console.log(calculatedGrants);

    $.ajax({
        "type": "POST",
        "url": "/Async/LoginActions.asmx/Grants",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {                           
            document.location = referrer;
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

var newUserId = null;
function ChangeUserName()
{
    var ok = true;
    $("#TxtUserNameErrorDuplicated").hide();
    if (!RequiredFieldText("TxtUserName") || !RequiredFieldText("TxtUserEmail") ) {
        ok = false;
    }
    else
    {
        var duplicated = false;
        for (var x = 0; x < CompanyUserNames.length; x++) {
            if (CompanyUserNames[x].UserName.toLowerCase() === document.getElementById("TxtUserName").value.toLowerCase() &&
                CompanyUserNames[x].UserId !== ItemUserId) {
                duplicated = true;
                ok = false;
                break;
            }
        }

        if(duplicated === true)
        {
            ok = false;
            $("#TxtUserNameLabel").css("color", "#f00");
            $("#TxtUserNameErrorDuplicated").show();
        }

        var duplicatedEmail = false;
        for (var y = 0; y < userEmails.length; y++) {
            if (userEmails[y].Email.toLowerCase() === document.getElementById("TxtUserEmail").value.toLowerCase() &&
                userEmails[y].UserId !== ItemUserId) {
                duplicatedEmail = true;
                ok = false;
                break;
            }
        }

        if(duplicatedEmail === true)
        {
            ok = false;
            $("#TxtUserEmailLabel").css("color", "#f00");
            $("#TxtUserEmailErrorDuplicated").show();
        }
    }

    if(ok===false)
    {
        return false;
    }

    var webMethod = "/Async/LoginActions.asmx/ChangeUserName";
    if(itemUser.Id < 1)
    {
        webMethod = "/Async/LoginActions.asmx/InsertUser";
    }

    var email = $('#TxtUserEmail').val();
    var data =
        {
            "itemUser":
            {
                "Id": itemUser.Id,
                "UserName": $("#TxtUserName").val(),
                "Email": email,
                "CompanyId": CompanyId,
                "Admin": document.getElementById("ChkAdmin").checked === true,
                "Language": $("#CmbIdioma").val()
            },
            "employeeId": $('#CmbEmployee').val() * 1,
            "userId": user.Id
        };

    console.log("SAVE",data);

    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === true) {
                if (itemUser.Id < 1) {
                    console.log("SAVE OK", msg.d);
                    itemUser.Id = msg.d.ReturnValue * 1;
                    newUserId = itemUser.Id;
                    alertInfoUI(Dictionary.Item_User_Message_WelcommeMailSent, grantsAvaiable);
                }
                else {
                    SaveGrants();
                }
            }
            else {
                alertUI(msg.d.MessageError);
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function ResetPassword()
{ 
    var data = { "userId": ItemUserId, "companyId": Company.Id };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: "/Async/LoginActions.asmx/ResetPassword",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            if(msg.d.Success!==false)
            {
                alertInfoUI(Dictionary.Item_User_Message_ResetPassword_Ok);
            }
            else
            {
                alertUI(msg.d.MessageError);
            }
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}
            
jQuery(function ($) {
    $("#BtnSave").click(ChangeUserName);
    $("#BtnCancel").click(function (e) { document.location = referrer; });

    var i = 0;
    for(var x=0; x<ddData.length; x++)
    {
        if(ddData[x].value === ApplicationUser.Employee.Address.Country) { i=x; break; }
    }

    $("#CmbPais").ddslick({"data": ddData});
    $("#CmbPais").ddslick('select', { "index": i });

    if (document.location.toString().indexOf("&p") !== -1) {
        $("#TabPermisos a").click();
    }
});

// ISSUS-190
//document.getElementById("TxtUserName").focus();

// ISSUS-259
function grantsAvaiable()
{
    $("#TabPermisos").show();
    alertInfoUI(Dictionary.Item_User_Message_GrantsAvaiable, GoNew);                
}

function GoNew() { document.location = "UserView.aspx?id=" + newUserId + "&p";}

function CmbEmployeeChanged() {
    EmployeeLayout();
    var id = $("#CmbEmployee").val() * 1;
    var employeeSelected = null;
    for (var x = 0; x < Company.Employees.length; x++) {
        if (Company.Employees[x].Id === id) {
            employeeSelected = Company.Employees[x];
        }
    }

    if (employeeSelected !== null) {
        $("#TxtNombre").val(employeeSelected.Name);
        $("#TxtApellido1").val(employeeSelected.LastName);
        $("#TxtNif").val(employeeSelected.Nif);
        $("#TxtTelfono").val(employeeSelected.Phone);
        $("#TxtEmail").val(employeeSelected.Email);
        $("#TxtDireccion").val(employeeSelected.Address.Address);
        $("#TxtCp").val(employeeSelected.Address.PostalCode);
        $("#TxtPoblacion").val(employeeSelected.Address.City);
        $("#TxtProvincia").val(employeeSelected.Address.Province);
        $("#TxtPais").val(CountryById(employeeSelected.Address.Country));
        $(".employeeProfile").css("visibility", "visible");
        $(".emailed").hide();
    }
    else {
        $(".employeeProfile").hide();
        $(".employeeProfile").css("visibility", "hidden");
    }
}
            
if($("#TxtPais").val()!=="")
{
    $("#TxtPais").val(CountryById($("#TxtPais").val() * 1));
}

function CountryById(id)
{
    for(var x=0; x<Company.Countries.length; x++)
    {
        if(Company.Countries[x].Id === id)
        {
            return Company.Countries[x].Name;
        }
    }

    return "";
}

function EmployeeLayout() {
    if ($("#CmbEmployee").val() * 1 > 0) {
        $(".employeeProfile").css("display", "block");
    }
    else {
        $(".employeeProfile").css("display", "none");
    }
}

window.onload = function () {
    EmployeeLayout();
    if (itemUser.Id > 0 && ApplicationUser.Grants.User.Write !== false) {
        $("#TabPermisos").show();
        AdminLayout();

        $("#ChkAdmin").on("click", SetAdmin);
    }
};

function SetAdmin() {
    itemUser.Admin = document.getElementById("ChkAdmin").checked;
    AdminLayout();
}

function ReadAll() {
    var grant = document.getElementById("RAll").checked;
    if (grant === true) {
        var grants = "";
        for (var x = 0; x < CBR.length; x++) {
            CBR[x].checked = true;
            GrantChanged("R", CBR[x].id.substring(12) * 1, CBR[x]);
        }

        document.getElementById("Contentholder1_Grants").value = grants;
    }
    else {
        for (var y = 0; y < CBR.length; y++) {
            CBR[y].checked = false;
            GrantChanged("R", CBR[y].id.substring(12) * 1, CBR[y]);
        }

        for (var z = 0; z < CBW.length; z++) {
            CBW[z].checked = false;
            GrantChanged("W", CBW[z].id.substring(13) * 1, CBR[z]);
        }

        document.getElementById("WAll").checked = false;
        document.getElementById("Contentholder1_Grants").value = "";
    }
}

function WriteAll() {
    var grant = document.getElementById("WAll").checked;
    if (grant === true) {
        for (var x = 0; x < CBR.length; x++) {
            CBR[x].checked = true;
            GrantChanged("R", CBR[x].id.substring(13) * 1, CBR[x]);
        }
        for (var y = 0; y < CBW.length; y++) {
            CBW[y].checked = true;
            GrantChanged("W", CBW[y].id.substring(13) * 1, CBR[y]);
        }

        document.getElementById("RAll").checked = true;
    }
    else {
        for (var z = 0; z < CBW.length; z++) {
            CBW[z].checked = false;
            GrantChanged("R", CBW[z].id.substring(13) * 1, CBR[z]);
        }
    }
}

function TestCBAll() {
    for (var x = 0; x < CBR.length; x++) {
        if (CBR[x].checked === false) {
            document.getElementById("RAll").checked = false;
            document.getElementById("WAll").checked = false;
            return;
        }
    }

    document.getElementById("RAll").checked = true;

    for (var y = 0; y < CBW.length; y++) {
        if (CBW[y].checked === false) {
            document.getElementById("WAll").checked = false;
            return;
        }
    }

    document.getElementById("WAll").checked = true;
}
            
var CBR = document.getElementsByClassName("CBR");
var CBW = document.getElementsByClassName("CBW");
TestCBAll();

function AdminLayout() {
    if (itemUser.Admin === true) {
        $("#DivPrimaryUser").show();
        for (var x = 0; x < CBR.length; x++) { CBR[x].disabled = true; CBR[x].checked = true; }
        for (var y = 0; y < CBW.length; y++) { CBW[y].disabled = true; CBW[y].checked = true; }
        document.getElementById("RAll").disabled = true;
        document.getElementById("WAll").disabled = true;
        document.getElementById("RAll").checked = true;
        document.getElementById("WAll").checked = true;
    }
    else {
        $("#DivPrimaryUser").hide();
        for (var z = 0; z < CBR.length; z++) { CBR[z].disabled = false; }
        for (var w = 0; w < CBW.length; w++) { CBW[w].disabled = false; }
        document.getElementById("RAll").disabled = false;
        document.getElementById("WAll").disabled = false;
    }
}
console.log(itemUser);

if (ApplicationUser.Grants.User.Write === false) {
    document.getElementById("TxtUserName").disabled = true;
    document.getElementById("TxtUserEmail").disabled = true;
    document.getElementById("CmbEmployee").disabled = true;
    document.getElementById("CmbEmployee").style.backgroundColor = "#f5f5f5";

    $("#BtnSave").hide();
    $("#Button1").hide();
    $("#ResetH4").hide();
    $("#ResetAlert").hide();
}

if (user.Id === itemUser.Id) {
    $("#Button1").attr("disabled", "disabled");
    $("#ResetAlert").html(Dictionary.Item_User_Help_ResetPasswordOwner);
}

if (itemUser.PrimaryUser === true) {
    document.getElementById("ChkPrimaryUser").checked = true;
    document.getElementById("ChkAdmin").disabled = true;
}

if (itemUser.Admin === true) {
    document.getElementById("ChkAdmin").checked = true;
}