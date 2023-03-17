function alertUI(message, width) {
    $("#AlertUIMessage").html(message);
    $("#AlertUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": width === null ? 300 : width,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons":
            [
                {
                    "id": "alertUI_BtnOK",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-xs btn-success",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function alertInfoUI(message, action) {
    $("#InfoUIMessage").html(message);
    $("#InfoUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "id": "alertInfoUI_BtnOK",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (typeof action !== "undefined" && action !== null) {
                        action();
                    }
                }
            }
        ]

    });
}

function alertInfoNoGrantsUI(message) {
    $("#InfoUIMessage").html(message);
    $("#InfoUINoGrantsDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "id": "alertInfoNoGrantsUI_BtnOK",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    var id = cmbEmployee.value;
                    EmployeeSetGrant(id, itemGrantId);
                }
            },
            {
                "id": "alertInfoNoGrantsUI_BtnCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    if (typeof cmbEmployee !== "undefined" && cmbEmployee !== null) {
                        cmbEmployee.value = 0;
                    }
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function warningInfoUI(message, action, width) {
    $("#WarningUIMessage").html(message);
    $("#WarningUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": width === null ? 300 : width,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "id": "warningInfoUI_BtnOK",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (typeof action !== "undefined" && action !== null) {
                        action();
                    }
                }
            }
        ]

    });
}

function successInfoUI(message, action, width) {
    $("#SuccessUIMessage").html(message);
    $("#SuccessUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": width === null ? 300 : width,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "id": "successInfoUI_BtnOK",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (typeof action !== "undefined" && action !== null) {
                        action();
                    }
                }
            }
        ]

    });
}

function timeoutAlert() {
    if (timeoutAlerted === true) { return false; }
    if (timePassed * 1 < 30) {
        console.log("===> Timeout falso intento", timePassed);
        return false;
    }

    timeoutAlerted = false;
    $("#TimeoutDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 500,
        "modal": true,
        "title": Dictionary.Common_TiemoutSessionTitle,
        "title_html": true,
        "buttons":
            [
                {
                    "id": "timeoutAlert_BtnOK",
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-xs btn-success",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]

    }).on("dialogclose", function (event) {
        document.location = "LogOut.aspx";
    });
}

function promptInfoUI(message, width, actionYes, actionNo) {
    $("#PromptUIMessage").html(message);
    $("#PromptUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": width === null ? 300 : width,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "id": "promptInfoUI_BtnOK",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (actionYes !== null) {
                        actionYes();
                    }
                }
            },
            {
                "id": "promptInfoUI_BtnCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs btn-danger",
                "click": function () {
                    $(this).dialog("close");
                    if (actionNo !== null) {
                        actionNo();
                    }
                }
            }
        ]
    });
}

// @alex la variable forced que se envía desde el upload hace que salga el mensaje
function LoadingShow(message, forced) {
    if (typeof forced === "undefined" || forced === null || forced === false) { return; }
    $("#LoadingMessage").html(message);
    var dialog = $("#LoadingDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": message,
        "title_html": true

    });
}

function LoadingHide() {
    try {
        $("#LoadingDialog").dialog("close");
    }
    catch (e) { console.log(e); }
}