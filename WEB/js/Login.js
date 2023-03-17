$(document).ready(function () {
    $("#ErrorMessage").hide();
    $("#BtnLogin").click(Login);
    $("#TxtUserName").focus();
});

$(document).keypress(function (e) {
    if (e.which === 13) {
        Login();
    }
});

function Login() {
    $("#ErrorSpan").hide();
    $("#ErrorMessage").hide();
    var ok = true;
    var errorMessage = "";
    if ($("#TxtUserName").val() === "") {
        ok = false;
        $("#TxtUserName").css("background-color", "#f00");
        errorMessage = Dictionary[language].RequiredUser;
    }
    else {
        $("#TxtUserName").css("background-color", "transparent");
    }

    if ($("#TxtPassword").val() === "") {
        ok = false;
        $("#TxtPassword").css("background-color", "#f00");
        if (errorMessage !== "") {
            errorMessage += "<br />";
        }
        errorMessage += Dictionary[language].RequiredPassword;
    }
    else {
        $("#TxtPassword").css("background-color", "transparent");
    }

    if (window.navigator.onLine === false) {
        ok = false;
        if (errorMessage !== "") {
            errorMessage += "<br />";
        }
        errorMessage = Dictionary[language].NoConnection;
    }

    if (ok) {
        var data = {
            "email": $("#TxtUserName").val(),
            "password": $("#TxtPassword").val(),
            "ip": ip
        };

        $("#BtnLogin").html(Dictionary[language].Loging);

        $.ajax({
            "type": "POST",
            "url": "/Async/LoginActions.asmx/GetLogin",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                var result = msg.d;

                if (msg.d.ReturnValue.Id === -1) {
                    $("#ErrorMessage").html(Dictionary[language].PasswordInvalid);
                    $("#ErrorMessage").show();
                    $("#BtnLogin").html(Dictionary[language].Btn);
                    return false;
                }

                if (msg.d.Success === true) {
                    if (msg.d.ReturnValue.MultipleCompany === true) {
                        document.location = "Select.aspx?action=" + (Math.random() * (1000 - 100)) + '-' + msg.d.ReturnValue.Id;
                        return false;
                    }
                }
                else {
                    if (msg.d.ReturnValue.Id === 2) {
                        $("#ErrorMessage").show();
                        $("#BtnLogin").html(Dictionary[language].Btn);
                        return false;
                    }
                }

                if (result.MustResetPassword === true) {
                    $("#UserId").val(result.Id);
                    $("#CompanyId").val(result.CompanyId);
                    $("#Password").val($("#TxtPassword").val());
                    document.getElementById("LoginForm").action = "ResetPassword.aspx";
                    $("#LoginForm").submit();
                    return false;
                }

                if (result.Id === -1) {
                    document.getElementById("ErrorSpan").style.display = "block";
                    $("#BtnLogin").html(Dictionary[language].Btn);
                }
                else if (result.CompanyId === 1) {
                    document.location = "admin";
                }
                else if (result.CompanyId === 2) {
                    document.location = "CompanySelect.aspx";
                }
                else {
                    $("#UserId").val(msg.d.ReturnValue.Id);
                    $("#CompanyId").val(msg.d.ReturnValue.CompanyId);
                    $("#LoginForm").submit();
                }

                $("#BtnLogin").html(Dictionary[language].Btn);
                return false;
            },
            "error": function () {
                document.getElementById("ErrorSpan").style.display = "block";
                $("#BtnLogin").html(Dictionary[language].Btn);
            }
        });
    }
    else {
        $("#ErrorMessage").html(errorMessage);
        $("#ErrorMessage").show();
        $("#BtnLogin").html(Dictionary[language].Btn);
    }
}

function LoginResultToRext(value) {
    for (var x = 0; x < LoginResult.length; x++) {
        if (LoginResult[x].value === value) {
            return LoginResult[x].text;
        }
    }

    return "undefined";
}