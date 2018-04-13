function EquipmentChangeImage(actionSelected) {
    if (Equipment.Id == 0) {
        alertUI('Para añadir una imagen primero hay que guardar el equipo.');
        return;
    }

    document.getElementById('actual').src = document.getElementById('EquipmentImg').src;
    document.getElementById('blah').src = '/images/noimage.jpg';
    var dialog = $("#ChangeImageDialog").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Equipment_Field_Image + '</h4>',
        title_html: true,
        width: 500,
        buttons: [
        {
            id: 'BtnNewAddresSave',
            html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
            "class": "btn btn-success btn-xs",
            click: function () {
                EquipmentChangeImageConfirmed();
            }
        },
        {
            html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
            "class": "btn btn-xs",
            click: function () {
                $(this).dialog("close");
            }
        }
        ]
    });
}

function EquipmentChangeImageConfirmed() {
    //check whether browser fully supports all File API
    if (window.File && window.FileReader && window.FileList && window.Blob) {

        if (!$('#imgInp').val()) //check empty input filed
        {
            alertUI(Dictionary.Common_Warning_ImageRequired);
            return false
        }

        var fsize = $('#imgInp')[0].files[0].size; //get file size
        var ftype = $('#imgInp')[0].files[0].type; // get file type


        //allow only valid image file types 
        switch (ftype) {
            case 'image/png': case 'image/jpeg': case 'image/pjpeg':
                break;
            default:
                alertUI("<b>" + ftype + "</b>&nbsp;"+Dictionary.Common_Warning_MimeType);
                return false
        }

        //Allowed file size is less than 50KB (25428)
        if (fsize > 5000000) {
            alertUI("<b>" + fsize + "</b>&nbsp;" + Dictionary.Common_Warning_ImageTooBig);
            return false
        }

        LoadingShow(Dictionary.Save);
        var fd = new FormData();
        var file = document.getElementById('imgInp');
        for (var i = 0; i < file.files.length; i++) {
            fd.append('_file', file.files[i]);
        }

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/ChangeEquipmentImage.aspx', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                document.getElementById('EquipmentImg').src = document.getElementById('blah').src;
                $("#ChangeImageDialog").dialog('close');
            }
        };
        xhr.send(fd);
    }
    else {
        alertUI(Dictionary.Common_Warning_BrowserOld);
        return false;
    }
}

function _EquipmentChangeImageConfirmed() {
    document.getElementById('EquipmentImg').src = document.getElementById('blah').src;
    $("#ChangeImageDialog").dialog('close');
}

function EquipmentDeleteImage() {
    var dialog = $("#DeleteImageDialog").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">'+Dictionary.Item_Equipment_Popup_ImageDelete_Title+'</h4>',
        title_html: true,
        width: 500,
        buttons: [
        {
            id: 'BtnNewAddresSave',
            html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
            "class": "btn btn-danger btn-xs",
            click: function () {
                EquipmentDeleteImageConfirmed();
            }
        },
        {
            html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
            "class": "btn btn-xs",
            click: function () {
                $(this).dialog("close");
            }
        }
        ]

    });
}

function EquipmentDeleteImageConfirmed() {
    document.getElementById('EquipmentImg').src = '/images/noimage.jpg';
    $("#DeleteImageDialog").dialog('close');
}