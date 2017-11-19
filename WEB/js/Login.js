$(document).ready(function () {
    $('#ErrorMessage').hide();
    $('#BtnLogin').click(Login);
    $('#TxtUserName').focus();
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        Login();
    }
});

function Login() {
    $('#ErrorMessage').hide();
    var ok = true;
    var errorMessage = '';
    if ($('#TxtUserName').val() === '') {
        ok = false;
        $('#TxtUserName').css('background-color', '#f00');
        errorMessage = 'El nombre de usuario es obligatorio.';
    }
    else {
        $('#TxtUserName').css('background-color', 'transparent');
    }

    if ($('#TxtPassword').val() === '') {
        ok = false;
        $('#TxtPassword').css('background-color', '#f00');
        if (errorMessage !== '') {
            errorMessage += '<br />';
        }
        errorMessage += 'La contraseña es obligatoria.';
    }
    else {
        $('#TxtPassword').css('background-color', 'transparent');
    }

    if (ok) {
        var webMethod = "/Async/LoginActions.asmx/GetLogin";
        var data = {
            'userName': $('#TxtUserName').val(),
            'password': $('#TxtPassword').val(),
            'ip': ip
        };

        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                var result = msg.d;
                console.log(msg.d);

                if (msg.d.ReturnValue.Id === -1)
                {
                    $('#ErrorSpan').show();
                    return false;
                }

                if (msg.d.Success === true)
                {
                    if(msg.d.ReturnValue.MultipleCompany === true)
                    {
                        document.location = 'Select.aspx?action=' + (Math.random() * (1000 - 100)) + '-' + msg.d.ReturnValue.Id;
                        return false;
                    }
                }

                if (result.MustResetPassword === true) {
                    $('#UserId').val(result.Id);
                    $('#CompanyId').val(result.CompanyId);
                    $('#Password').val($('#TxtPassword').val());
                    document.getElementById('LoginForm').action = 'ResetPassword.aspx';
                    $('#LoginForm').submit();
                    return false;
                }
                if (result.Id == -1) {
                    document.getElementById('ErrorSpan').style.display = 'block';
                }
                else if (result.CompanyId == 1) {
                    document.location = 'admin';
                }
                else if (result.CompanyId == 2) {
                    document.location = 'CompanySelect.aspx';
                }
                else {
                    $('#UserId').val(msg.d.ReturnValue.Id);
                    $('#CompanyId').val(msg.d.ReturnValue.CompanyId);
                    $('#LoginForm').submit();
                }

                return false;
            },
            error: function (msg) {
                document.getElementById('ErrorSpan').style.display = 'block';
            }
        });
    }
    else {
        $('#ErrorMessage').html(errorMessage);
        $('#ErrorMessage').show();
    }
}

function LoginResultToRext(value) {
    for (var x = 0; x < LoginResult.length; x++) {
        if (LoginResult[x].value == value) {
            return LoginResult[x].text;
        }
    }

    return 'undefined';
}