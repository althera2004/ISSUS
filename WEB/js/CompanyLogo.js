function SaveLogo() {
    //check whether browser fully supports all File API
    if (window.File && window.FileReader && window.FileList && window.Blob) {

        if (!$('#imageInput').val()) //check empty input filed
        {
            alertUI(Dictionary.Common_Warning_ImageRequired);
            return false
        }

        var fsize = $('#imageInput')[0].files[0].size; //get file size
        var ftype = $('#imageInput')[0].files[0].type; // get file type


        //allow only valid image file types 
        switch (ftype) {
            case 'image/png':
            case 'image/jpeg':
            case 'image/pjpeg':
            case 'image/gif':
                break;
            default:
                alertUI("<b>" + ftype + "</b>&nbsp;" + Dictionary.Common_Warning_MimeType);
                return false
        }

        //Allowed file size is less than 50KB (25428)
        if (fsize > 50000) {
            alertUI("<b>" + fsize + "</b>&nbps" + dictionary.Common_Warning_ImageTooBig);
            return false
        }

        LoadingShow(Dictionary.Save);
        var fd = new FormData();
        var file = document.getElementById('imageInput');
        for (var i = 0; i < file.files.length; i++) {
            fd.append('_file', file.files[i]);
        }

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/ChangeLogo.aspx', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                document.getElementById('LogoImage').src = 'images/Logos/' + Company.Id + '.jpg?' + (new Date().getMilliseconds());
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

function CompanyChangeImage(actionSelected) {
    document.getElementById('actual').src = document.getElementById('EquipmentImg').src;
    document.getElementById('blah').src = '/images/noimage.jpg';
    var dialog = $("#ChangeImageDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Item_Company_Logo,
        title_html: true,
        width: 500,
        buttons:
        [
            {
                id: 'BtnNewAddresSave',
                html: "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                click: function () {
                    CompanyChangeImageConfirmed();
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function CompanyChangeImageConfirmed() {
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
            case 'image/png':
            case 'image/jpeg':
            case 'image/pjpeg':
            case 'image/gif':
                break;
            default:
                alertUI("<b>" + ftype + "</b>&nbsp;" + Dictionary.Common_Warning_MimeType);
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
        xhr.open('POST', '/ChangeLogo.aspx', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var size;
                eval("size = " + xhr.responseText + ";");
                document.getElementById('EquipmentImg').style.width = size.Width;
                document.getElementById('EquipmentImg').style.height = size.Height;
                document.getElementById('EquipmentImg').src = document.getElementById('blah').src;
                $("#ChangeImageDialog").dialog('close');
            }
        };
        xhr.send(fd);
    }
    else
    {
        alertUI(Dictionary.Common_Warning_BrowserOld);
        return false;
    }
}


function _CompanyChangeImageConfirmed() {
    document.getElementById('EquipmentImg').src = document.getElementById('blah').src;
    $("#CompanyImageDialog").dialog('close');
}

function CompanyDeleteImage() {
    var dialog = $("#DeleteImageDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">'+Dictionary.Item_CompanyData_Popup_ImageDelete_Title+'</h4>',
        title_html: true,
        width: 500,
        buttons:
        [
            {
                id: 'BtnNewAddresSave',
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    EquipmentDeleteImageConfirmed();
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]

    });
}

function CompanyDeleteImageConfirmed() {
    document.getElementById('EquipmentImg').src = '/images/noimage.jpg';
    document.getElementById('EquipmentImg').style.width = '100px';
    document.getElementById('EquipmentImg').style.height = '100px';
    $("#DeleteImageDialog").dialog('close');
}