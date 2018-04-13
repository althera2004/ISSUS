var CostDefinitionSelected;
function CostDefinitionDeleteAction() {
    var webMethod = "/Async/CostDefinitionActions.asmx/Inactive";
    var data = { costDefinitionId: CostDefinitionSelected, companyId: Company.Id, userId: user.Id };
    $("#CostDefinitionDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            document.location = document.location + '';
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function CostDefinitionDelete(name, id) {
    $('#CostDefinitionName').html(name);
    CostDefinitionSelected = id;
    var dialog = $("#CostDefinitionDeleteDialog").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_CostDefinition_Popup_Delete_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    CostDefinitionDeleteAction();
                }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function CostDefinitionUpdate(id) {
    document.location = 'CostDefinitionView.aspx?id=' + id;
    return false;
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

    $("#BtnNewCostDefinition").on('click', function (e) {
        document.location = 'CostDefinitionView.aspx?id=-1';
        return false;
    });
});


function Resize() {
    var listTable = document.getElementById('ListDataDiv');
    var containerHeight = $(window).height();
    listTable.style.height = (containerHeight - 310) + 'px';
}

window.onload = function () { Resize(); }
window.onresize = function () { Resize(); }