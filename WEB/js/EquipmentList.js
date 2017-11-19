var EquipmentSelected;
function EquipmentDeleteAction() {
    var webMethod = "/Async/EquipmentActions.asmx/Delete";
    var data = { equipmentId: EquipmentSelected, reason: '', companyId: Company.Id, userId: user.Id };
    $("#EquipmentDeleteDialog").dialog("close");
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

function EquipmentDelete(id, name) {
    $('#EquipmentName').html(name);
    EquipmentSelected = id;
    var dialog = $("#EquipmentDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Equipment_Popup_Delete_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    EquipmentDeleteAction();
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

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;'
            if (("title_html" in this.options) && this.options.title_html == true)
                title.html($title);
            else title.text($title);
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

    $("#BtnNewItem").before("<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"Export('PDF');\"><i class=\"icon-print bigger-110\"></i>" + Dictionary.Common_ListPdf + "</button>&nbsp;")
}
window.onresize = function () { Resize(); }

function Export(fileType) {
    console.log("Export", fileType);
    var webMethod = "/Export/EquipmentExportList.aspx/" + fileType;
    var data = { "companyId": Company.Id };
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            //successInfoUI(msg.d.MessageError, Go, 200);
            var link = document.createElement('a');
            link.id = 'download';
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = '_blank';
            document.body.appendChild(link);
            document.body.removeChild(link);
            $('#download').trigger('click');
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog('close');
        },
        error: function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}