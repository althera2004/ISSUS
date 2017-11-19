function alertUI(message, blockPopup) {
    if (blockPopup !== null) {
        lockedPopupId = blockPopup;
        document.getElementById(blockPopup).parentNode.style.cssText += 'z-Index:1039 !important';
    }
    else {
        lockedPopupId = '';
    }
    $('#AlertUIMessage').html(message);
    var dialog = $("#AlertUIDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Aviso,
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () {
            if (lockedPopupId !== '') {
                document.getElementById(lockedPopupId).parentNode.style.cssText += 'z-Index:1050 !important';
            }
        }
    });
}

function alertInfoUI(message, action, blockPopup) {
    if (blockPopup !== null) {
        lockedPopupId = blockPopup;
        document.getElementById(blockPopup).parentNode.style.cssText += 'z-Index:1039 !important';
    }
    else {
        lockedPopupId = '';
    }
    $('#InfoUIMessage').html(message);
    var dialog = $("#InfoUIDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Aviso,
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                click: function () {
                    $(this).dialog("close");
                    if (action !== null) { action(); }
                }
            }
        ],
        close: function () {
            if (lockedPopupId !== '') {
                document.getElementById(lockedPopupId).parentNode.style.cssText += 'z-Index:1050 !important';
            }
        }
    });
}

function warningInfoUI(message, action, width, blockPopup) {
    if (blockPopup !== null) {
        lockedPopupId = blockPopup;
        document.getElementById(blockPopup).parentNode.style.cssText += 'z-Index:1039 !important';
    }
    else {
        lockedPopupId = '';
    }
    $('#WarningUIMessage').html(message);
    var dialog = $("#WarningUIDialog").removeClass('hide').dialog({
        resizable: false,
        width: width === null ? 300 : width,
        modal: true,
        title: Dictionary.Aviso,
        title_html: true,
        parent: blockPopup,
        buttons:
        [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                click: function () {
                    $(this).dialog("close");
                    if (action !== null) {
                        action();
                    }
                }
            }
        ],
        close: function () {
            if (lockedPopupId !== '') {
                document.getElementById(lockedPopupId).parentNode.style.cssText += 'z-Index:1050 !important';
            }
        }
    });
}

function successInfoUI(message, action, width) {
    if (blockPopup !== null) {
        lockedPopupId = blockPopup;
        document.getElementById(blockPopup).parentNode.style.cssText += 'z-Index:1039 !important';
    }
    else {
        lockedPopupId = '';
    }
    $('#SuccessUIMessage').html(message);
    var dialog = $("#SuccessUIDialog").removeClass('hide').dialog({
        resizable: false,
        width: width === null ? 300 : width,
        modal: true,
        title: Dictionary.Aviso,
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-xs btn-success",
                click: function () {
                    $(this).dialog("close");
                    if (action !== null) {
                        action();
                    }
                }
            }
        ],
        close: function () {
            if (lockedPopupId !== '') {
                document.getElementById(lockedPopupId).parentNode.style.cssText += 'z-Index:1050 !important';
            }
        }
    });
}

function promptInfoUI(message, width, actionYes, actionNo) {
    $('#PromptUIMessage').html(message);
    var dialog = $("#PromptUIDialog").removeClass('hide').dialog({
        resizable: false,
        width: width === null ? 300 : width,
        modal: true,
        title: Dictionary.Aviso,
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-xs btn-success",
                click: function () {
                    $(this).dialog("close");
                    if (actionYes !== null) {
                        actionYes();
                    }
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs btn-danger",
                click: function () {
                    $(this).dialog("close");
                    if (actionNo !== null) {
                        actionNo();
                    }
                }
            }
        ]

    });
}

function LoadingShow(message) {
    return;
    /* $('#LoadingMessage').html(message);
    var dialog = $("#LoadingDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Common_Working,
        title_html: true
    }); */
}

function LoadingHide() {
    return;
    /* $('#LoadingDialog').dialog('close'); */
}