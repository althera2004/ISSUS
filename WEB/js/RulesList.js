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

    $('#BtnNewRule').on('click', function (e) {
        e.preventDefault();
        document.location = 'RulesView.aspx?id=-1';
    });
});

function RuleDelete(id, description) {
    //document.getElementById('dialogRules').parentNode.style.cssText += 'z-Index:1039 !important';
    $('#RuleName').html(description);
    Selected = id * 1;
    var dialog = $("#RuleDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Rules_Popup_DeleteProcessType_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    $(this).dialog("close");
                    RulesDeleteConfirmed(Selected);
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () {
            //document.getElementById('dialogRules').parentNode.style.cssText += 'z-Index:1050 !important';
        }
    });
}

function RulesDeleteConfirmed(id) {
    var webMethod = "/Async/RulesActions.asmx/RulesDelete";
    var data = {
        'RulesId': id,
        'companyId': Company.Id,
        'userId': user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
                document.location = 'RulesList.aspx';
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 310) + 'px';
}

window.onload = function () { Resize(); }
window.onresize = function () { Resize(); }