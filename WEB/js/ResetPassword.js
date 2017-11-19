$(document).ready(function () {
    $('#ErrorMessage').hide();
    $('#BtnLogin').click(ResetPassword);
    $('#TxtUserName').focus();
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        Login();
    }
});

function ResetPassword() {
    $('#ErrorMessage').hide();
    var ok = true;
    var errorMessage = '';
    if ($('#TxtPassword1').val() === '') {
        ok = false;
        $('#TxtPassword1').css('background-color', '#f77');
        errorMessage = 'La contraseña es obligatoria.';
    }
    else {
        $('#TxtPassword1').css('background-color', 'transparent');
    }

    if ($('#TxtPassword2').val() !== $('#TxtPassword1').val()) {
        ok = false;
        $('#TxtPassword2').css('background-color', '#f77');
        if (errorMessage !== '') {
            errorMessage += '<br />';
        }
        errorMessage += this.Dictionary.Item_User_ErrorMessage_PaswordsNoMatch;
    }
    else {
        $('#TxtPassword2').css('background-color', 'transparent');
    }

    if (ok) {
        var webMethod = "/Async/LoginActions.asmx/ChangePassword";
        var data = {
            'userId': user.Id,
            'oldPassword': op,
            'newPassword': $('#TxtPassword1').val(),
            'companyId': CompanyId
        };

        $.ajax({
            type: "POST",
            url: webMethod,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data, null, 2),
            success: function (msg) {
                var result = msg.d;
                $('#Password').val($('#TxtPassword1').val());
                $('#LoginForm').submit();
                return false;
            },
            error: function (msg) {
                document.getElementById('ErrorMessage').style.display = 'block';
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