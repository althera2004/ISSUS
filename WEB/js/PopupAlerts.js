function alertUI(message) {
    $("#AlertUIMessage").html(message);
    var dialog = $("#AlertUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons":
        [
            {
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
    var dialog = $("#InfoUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (action != null) {
                        action();
                    }
                }
            }
        ]

    });
}

function warningInfoUI(message, action, width) {
    $("#WarningUIMessage").html(message);
    var dialog = $("#WarningUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": width == null ? 300 : width,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (action != null) {
                        action();
                    }
                }
            }
        ]

    });
}

function successInfoUI(message, action, width) {
    $("#SuccessUIMessage").html(message);
    var dialog = $("#SuccessUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": width == null ? 300 : width,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (action != null) {
                        action();
                    }
                }
            }
        ]

    });
}

function timeoutAlert() {
    if (timeoutAlerted === true) {
        return false
    };

    if (timePassed * 1 < 30) {
        console.log("===> Timeout falso intento", timePassed);
        return false;
    }

    timeoutAlerted = false;
    var dialog = $("#TimeoutDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 500,
        "modal": true,
        "title": Dictionary.Common_TiemoutSessionTitle,
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]

    }).on("dialogclose", function (event) {
        document.location = "LogOut.aspx";
    })
}

function promptInfoUI(message, width, actionYes, actionNo) {
    $("#PromptUIMessage").html(message);
    var dialog = $("#PromptUIDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": width == null ? 300 : width,
        "modal": true,
        "title": Dictionary.Common_Warning,
        "title_html": true,
        "buttons": [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-xs btn-success",
                "click": function () {
                    $(this).dialog("close");
                    if (actionYes != null) {
                        actionYes();
                    }
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs btn-danger",
                "click": function () {
                    $(this).dialog("close");
                    if (actionNo != null) {
                        actionNo();
                    }
                }
            }
        ]

    });
}

function LoadingShow(message) {
    return;
    $("#LoadingMessage").html(message);
    var dialog = $("#LoadingDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Common_Working,
        "title_html": true

    });
}

function LoadingHide() {
    return;
    $("#LoadingDialog").dialog("close");
}