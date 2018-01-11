function ProcessNoDelete(sender) {
    alertUI(Dictionary.Common_Warning_Undelete, null);
}

function ProcessDelete(sender) {
    $('#ProcessName').html(sender.parentNode.parentNode.childNodes[0].childNodes[0].innerHTML);
    var Selected = sender.parentNode.parentNode.id * 1;
    var dialog = $("#ProcessDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Process_Popup_Delete_Title+'</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    $(this).dialog("close");
                    ProcessDeleteConfirmed(Selected);
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

function ProcessDeleteConfirmed(id) {
    var webMethod = "/Async/ProcessActions.asmx/DesactiveProcess";
    var actionAddress = id;
    var data = {
        'companyId': Company.Id,
        'processId': id,
        'userId': user.Id
    };

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            if (response.d.Success === true) {
                document.location = document.location + '';
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
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
});

function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 310) + 'px';
}

window.onload = function () {
    Resize();

    if (!$("#th0").hasClass("ASC")) {
        $("#th0").click();
        $("#th0").click();
    }

}
window.onresize = function () { Resize(); }