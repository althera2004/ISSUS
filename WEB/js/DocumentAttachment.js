function RestoreUpload() {
    document.getElementById("fileName").files = null;
    $("#UploadMessage").show();
    $("#UploadMessageSelected").hide();
    $("#UploadMessageSelectedFileName").html("");
}

function ShowSelected() {
    var file = document.getElementById("fileName").files[0];
    $("#UploadMessage").hide();
    $("#UploadMessageSelected").show();
    $("#UploadMessageSelectedFileName").html(file.name);
}

function ShowUploadSelect() {
    $("#fileName").trigger("click");
}

// Add events
$("input[type=file]").on("change", ShowSelected);
$("#UploadMessage").on("click", ShowUploadSelect);

function documentsModeView(x) {
    if (x === 0) {
        document.getElementById("BtnModeList").className = "btn btn-info";
        document.getElementById("BtnModeGrid").className = "btn";
        $("#UploadFilesContainer").hide();
        $("#UploadFilesList").show();
    }
    else {
        document.getElementById("BtnModeList").className = "btn";
        document.getElementById("BtnModeGrid").className = "btn btn-info";
        $("#UploadFilesContainer").show();
        $("#UploadFilesList").hide();
    }
}

function GetAttachmentById(id)
{
    for (var x = 0; x < attachs.length; x++)
    {
        if(attachs[x].Id === id)
        {
            return attachs[x];
        }
    }
    return null;
}

function ShowPDF(attachmentId) {
    //initialize the document viewer
    var viewer = new DocumentViewer({
        $anchor: $('#container'),
        width: 600,
        isModal: true
    });

    var attach = GetAttachmentById(attachmentId);
    console.log(attach);
    var documentName = "Document_" + documentId + "_V" + attach.Version + "_" + attachmentId + "." + attach.Extension;
    console.log(documentName);

    var fileName = "/DOCS/" + ApplicationUser.CompanyId + "/" + documentName;
    viewer.load(fileName);
    $(".document-viewer-wrapper").css("margin-left", $(".document-viewer-wrapper").css("width").split("px")[0] / -2);
}

var AttachSelected;
function DeleteUploadFileConfirmed() {
    var data = {
        "attachId": AttachSelected,
        "companyId": Company.Id
    };
    $("#DeleteAttachDialog").dialog("close");
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentAttachmentActions.asmx/Delete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.clear();
            AttachDeleted(msg.d.MessageError * 1);
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function DocumentAttachDelete(id, description) {
    $("#AttachName").html(description);
    AttachSelected = id;
    $("#DeleteAttachDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Attach_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "id": "BtnDeleteFileOk",
                "html": "<i class= \"icon-trash bigger-110\"></i>" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    DeleteUploadFileConfirmed();
                }
            },
            {
                "id": "BtnDeleteFileCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>" + Dictionary.Common_No,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function UploadFile() {
    RestoreUpload();
    $("#UploadFileDescription").val("");
    if (attachActual !== null)
    {
        $("#AttachWarning").show();
    }
    else {
        $("#AttachWarning").hide();
    }

    $("#attachFile").removeClass("hide").dialog({
        "modal": true,
        "title": "<h4 class=\"smaller\">&nbsp;"+ Dictionary.Item_DocumentAttachment_AddFile + "</h4>",
        "title_html": true,
        "width": 800,
        "buttons":
        [
            {
                "id": "UçBtnUploadfileOk",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    UploadFileGo(documentId);
                }
            },
            {
                "id": "UçBtnUploadfileCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function UploadFileGo(documentId) {
    //check whether browser fully supports all File API
    if (window.File && window.FileReader && window.FileList && window.Blob) {

        if ($("#AttachmentFileDescription").val() === "")
        {
            alertUI(Dictionary.Item_DocumentAttachment_PopupUpload_Description_ErrorRequired);
            return false;
        }

        if (!$("#fileName").val()) //check empty input filed
        {
            alertUI(Dictionary.Common_Error_NoFileSelected);
            return false;
        }

        var fsize = $("#fileName")[0].files[0].size; //get file size
        var ftype = $("#fileName")[0].files[0].type; // get file type

        console.log(ftype);
        //allow only valid image file types 
        switch (ftype) {
            case "text/plain":
            case "image/png":
            case "image/jpeg":
            case "application/msword":
            case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
            case "application/pdf":
                break;
            default:
                alertUI("<i>" + ftype + "</i><br />" + Dictionary.Common_Error_UnsupportedFile);
                return false
        }

        //Allowed file size is less than 50KB (25428)
        /*if (fsize > 50000) {
            alertUI("<b>" + fsize + "</b> Too big Image file! <br />Please reduce the size of your photo using an image editor.");
            return false
        }*/

        LoadingShow(Dictionary.Save);
        var fd = new FormData();
        var file = document.getElementById("imageInput");
        for (var i = 0; i < $("#fileName")[0].files.length; i++) {
            fd.append("_file", $("#fileName")[0].files[i]);
        }

        fd.append("DocumentoId", documentId);
        fd.append("Version", $("#TxtRevision").val() * 1);
        fd.append("ItemId", documentId);
        fd.append("Description", $("#AttachmentFileDescription").val());
        fd.append("CompanyId", ApplicationUser.CompanyId);
        fd.append("ApplicationUserId", ApplicationUser.Id);

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/Async/UploadDocumentAttachment.aspx", true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var result;
                eval("result= " + xhr.responseText + ";");
                InsertAttachemntRow(result);
                attachActual = result;
                SetAttachLayout();
            }
        };
        xhr.send(fd);
    }
    else {
        alertUI("Please upgrade your browser, because your current browser lacks some new features we need!");
        return false;
    }
}

function InsertAttachemntRow(documentAttachment) {
    // console.log("after insert",documentAttachment);
    attachs.push(documentAttachment);
    $("#DOC" + documentAttachment.Version).html(documentAttachment.Description);

    //var tdIconsHtml = '<span class="btn btn-xs btn-success" onclick="ShowPDF(' + documentAttachment.Id + ');"><i class="icon-eye-open bigger-120"></i></span>' +
    var tdIconsHtml = '<span class="btn btn-xs btn-success" onclick="window.open(\'/DOCS/' + documentAttachment.CompanyId + '/Document_' + documentId + '_v' + documentAttachment.Version + '_' + documentAttachment.Id + '.' + documentAttachment.Extension + '\');"><i class="icon-eye-open bigger-120"></i></span>' +
        '&nbsp;<span title="' + Dictionary.Common_Delete + '" class="btn btn-xs btn-danger" onclick="DocumentAttachDelete(' + documentAttachment.Id + ',\'' + documentAttachment.Description + '\');"><i class="icon-trash bigger-120"></i></span></td>';

    var tdIcons = document.getElementById("Icons" + documentAttachment.Version);
    tdIcons.innerHTML = tdIconsHtml;
    $("#attachFile").dialog("close");
}

function SetAttachLayout() {
    console.log("SetAttachLayout", attachActual);
    if (attachActual !== null) {
        $("#ActualDocumentLabel").show();
        $("#ActualDocumentLink").show();

        var extension = attachActual.Extension;
        tdIconsHtml = "";
        //if (extension !== "txt" && extension !== "png" && extension !== "gif" && extension !== "jpg") {
        //    tdIconsHtml += "";
        //}
        //else {
        //    tdIconsHtml += "    <span class=\"btn btn-xs btn-success\" onclick=\"ShowPDF(' + attachActual.Id + ');\"><i class=\"icon-eye-open bigger-120\"></i></span>";
        //}

        //var tdIconsHtml = '<span class="btn btn-xs btn-success" onclick="ShowPDF(' + attachActual.Id + ');"><i class="icon-eye-open bigger-120"></i></span>' +
        tdIconsHtml += '&nbsp;<span class="btn btn-xs btn-success" onclick="window.open(\'/DOCS/' + attachActual.CompanyId + '/Document_' + documentId + '_v' + attachActual.Version + '_' + attachActual.Id + '.' + attachActual.Extension + '\');"><i class="icon-eye-open bigger-120"></i></span>';
        tdIconsHtml += '&nbsp;<span title="' + Dictionary.Common_Delete + '" class="btn btn-xs btn-danger" onclick="DocumentAttachDelete(' + attachActual.Id + ',\'' + attachActual.Description + '\');"><i class="icon-trash bigger-120"></i></span></td>';

        $("#ActualDocumentLink").html(attachActual.Description);
        $("#BtnAttachText").html(Dictionary.Item_DocumentAttachment_Button_Replace);
        $("#tdIconsDiv").html(tdIconsHtml);

    } else {
        $("#ActualDocumentLabel").hide();
        $("#ActualDocumentLink").hide();
        $("#tdIconsDiv").html("");
        $("#BtnAttachText").html(Dictionary.Item_DocumentAttachment_Button_New);
    }
}

function AttachDeleted(id)
{
    if (attachActual !== null) {
        if (id = attachActual.Id) {
            attachActual = null;
            SetAttachLayout();
        }
    }

    var attach = GetAttachmentById(id);
    if(id!==null)
    {
        $("#DOC" + attach.Version).html("");
        $("#Icons" + attach.Version).html("");
    }
}

SetAttachLayout();