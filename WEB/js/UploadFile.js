function RestoreUpload() {
    document.getElementById('fileName').files = null;
    document.getElementById('UploadMessage').style.display = '';
    document.getElementById('UploadMessageSelected').style.display = 'none';
    document.getElementById('UploadMessageSelectedFileName').innerHTML = '';
}

function ShowSelected() {
    var file = document.getElementById('fileName').files[0];
    document.getElementById('UploadMessage').style.display = 'none';
    document.getElementById('UploadMessageSelected').style.display = '';
    document.getElementById('UploadMessageSelectedFileName').innerHTML = file.name;
}

function ShowUploadSelect() {
    $('#fileName').trigger("click");
}

// Add events
$("#fileName").on("change", ShowSelected);

$("#UploadMessage").on("click", ShowUploadSelect);

function documentsModeView(x) {
    if (x == 0) {
        document.getElementById("BtnModeList").className = "btn btn-info";
        document.getElementById("BtnModeGrid").className = "btn";
        document.getElementById("UploadFilesContainer").style.display = "none";
        document.getElementById("UploadFilesList").style.display = "";
    }
    else {
        document.getElementById("BtnModeList").className = "btn";
        document.getElementById("BtnModeGrid").className = "btn btn-info";
        document.getElementById("UploadFilesContainer").style.display = "";
        document.getElementById("UploadFilesList").style.display = "none";
    }
}

function ShowPDF(documentName) {
    //initialize the document viewer
    var viewer = new DocumentViewer({
        $anchor: $('#container'),
        width: 600,
        isModal: true
    });

    var fileName = '/DOCS/' + ApplicationUser.CompanyId + '/' + documentName;
    viewer.load(fileName);
    $(".document-viewer-wrapper").css("margin-left", $(".document-viewer-wrapper").css("width").split('px')[0] / -2);
}

var AttachSelected;
function DeleteUploadFileConfirmed() {
    var webMethod = "/Async/UpladFileActions.asmx/Delete";
    var data = {
        attachId: AttachSelected,
        companyId: Company.Id
    };
    $("#DeleteAttachDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            $('#' + AttachSelected).hide();
            $('#tr' + AttachSelected).hide();
        },
        error: function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function DeleteUploadFile(id, description) {
    $('#AttachName').html(description);
    AttachSelected = id;
    var dialog = $("#DeleteAttachDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Attach_Popup_Delete_Title + '</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    DeleteUploadFileConfirmed();
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

function UploadFile() {
    RestoreUpload();
    $('#UploadFileDescription').val('');
    if (document.getElementById("BtnUploadOk") !== null) {
        document.getElementById("BtnUploadOk").disabled = false;
    }
    var dialog = $("#PopupUploadFile").removeClass('hide').dialog({
        modal: true,
        title: '<h4 class="smaller">&nbsp;' + Dictionary.Item_DocumentAttachment_AddFile + '</h4>',
        title_html: true,
        width: 800,
        buttons:
        [
            {
                "html": "<i class=\"icon-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "id": "BtnUploadOk",
                "click": function () {
                    UploadFileGo();
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function UploadFileGo() {
    //check whether browser fully supports all File API
    if (window.File && window.FileReader && window.FileList && window.Blob) {

        if (!$('#fileName').val()) //check empty input filed
        {
            alertUI(Dictionary.Common_Error_NoFileSelected);
            return false
        }

        var fsize = $('#fileName')[0].files[0].size; //get file size
        var ftype = $('#fileName')[0].files[0].type; // get file type

        console.log(ftype);
        //allow only valid image file types 
        switch (ftype) {
            case 'text/plain':
            case 'image/png':
            case 'image/jpeg':
            case 'image/pjpeg':
            //case 'image/gif':
            case 'application/vnd.openxmlformats-officedocument.wordprocessingml.document':
            case 'application/pdf':
            case 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet':
            case 'application/vnd.ms-excel':
            case 'application/vnd.openxmlformats-officedocument.wordprocessingml.template':
            case 'application/vnd.oasis.opendocument.text':
            case 'application/vnd.oasis.opendocument.spreadsheet':
            case 'application/vnd.oasis.opendocument.graphics':
            case 'application/msword':
                break;
            default:
                alertUI("<i>" + ftype + "</i><br />&nbsp;" + Dictionary.Common_Warning_MimeType);
                return false
        }

        document.getElementById("BtnUploadOk").disabled = true;
        LoadingShow(Dictionary.Save);
        var fd = new FormData();
        var file = document.getElementById('imageInput');
        for (var i = 0; i < $('#fileName')[0].files.length; i++) {
            fd.append('_file', $('#fileName')[0].files[i]);
        }

        fd.append('ItemLinked', typeItemId);
        fd.append('ItemId', itemId);
        fd.append('Description', $('#UploadFileDescription').val());
        fd.append('CompanyId', ApplicationUser.CompanyId);
        fd.append('ApplicationUserId', ApplicationUser.Id);

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Async/UploadFile.aspx', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var result;
                eval("result= " + xhr.responseText + ";");
                console.log(result);
                RenderNewUploadFile(result.Id, result.Description, result.Extension, result.FileName, result.ModifiedOn, result.Size);
            }
        };
        xhr.send(fd);
    }
    else {
        alertUI(Dictionary.Common_Warning_BrowserOld);
        return false;
    }
}

function RenderNewUploadFile(id, description, extension, fileName, date, size) {
    var target = document.getElementById('UploadFilesContainer');

    console.log("Desc", description);
    console.log("filename", fileName);

    var fileNameToShow = fileName.split('_')[2];

    var finalSize = (size / 1024 / 1024);
    if (finalSize < 0.01) {
        finalSize = 0.01;
    }

    /*<div class="col-sm-3 document-container" id="1">
                        <div class="col-sm-6">&nbsp;</div>
                        <div class="col-sm-2 btn-success" onclick="ShowPDF('Employees_3_Juan Castilla Calderón.pdf');"><i class="icon-eye-open bigger-120"></i></div>
                        <div class="col-sm-2 btn-info"><a class="icon-download bigger-120" href="/DOCS/12/Employees_3_Juan Castilla Calderón.pdf" target="_blank" style="color:#fff;"></a></div>
                        <div class="col-sm-2 btn-danger" onclick="DeleteUploadFile(1,'Currículum');">
                            <i class="icon-trash bigger-120"></i>
                        </div>
                        <div class="col-sm-12 iconfile">
                            <div class="col-sm-4">
                                <img src="/images/FileIcons/PDF.png">
                            </div>
                            <div class="col-sm-8 document-name" style="max-width:100%;">Currículum</div>
                        </div>
                    </div>*/

    /*<div id="104" class="col-sm-3 document-container">
                        <div class="col-sm-6">&nbsp;</div>
                        <div class="col-sm-2">&nbsp;</div>
                        <div class="col-sm-2 btn-info"><a class="icon-download bigger-120" href="/DOCS/12/Equipments_2_Doc1.docx" target="_blank" style="color:#fff;"></a></div>
                        <div class="col-sm-2 btn-danger" onclick="DeleteUploadFile(104,'documento de pruebas');"><i class="icon-trash bigger-120"></i></div>
                        <div class="col-sm-12 iconfile" style="max-width: 100%;">
                            <div class="col-sm-12" style="margin-bottom:8px;"><strong title="documento de pruebas">documento de pruebas</strong></div>
                            <div class="col-sm-4"><img src="/images/FileIcons/DOCX.png"></div>
                            <div class="col-sm-8 document-name" style="font-size:12px;">   
                                Pujat el: <strong>22/02/2017</strong><br>
                                Tamany: <strong>0.01 MB</strong>
                            </div>
                            <div class="col-sm-12"></div>
                        </div>
                    </div>*/

    /*<div id="216" class="col-sm-3 document-container">
                        <div class="col-sm-6">&nbsp;</div>
                        <div class="col-sm-2">&nbsp;</div>
                        <div class="col-sm-2 btn-info"><a class="icon-download bigger-120" href="/DOCS/12/Processes_17_estado issus.docx" target="_blank" style="color:#fff;"></a></div>
                        <div class="col-sm-2 btn-danger" onclick="DeleteUploadFile(216,'Processes_17_estado issus.docx');"><i class="icon-trash bigger-120"></i></div>
                        <div class="col-sm-12 iconfile" style="max-width: 100%;">
                            <div class="col-sm-4"><img src="/images/FileIcons/nofile.png"></div>
                            <div class="col-sm-8 document-name">
                                <strong title="Processes_17_estado issus.docx">Processes_17_es...</strong><br>
                                Pujat el: 24/11/2017
                                Mida: 0.01 MB
                            </div>
                        </div>
                    </div>*/


    if (description === "") {
        description = fileNameToShow;
    }

    var descriptionFicha = description;
    if (descriptionFicha.length > 15) {
        descriptionFicha = descriptionFicha.substr(0, 15) + "...";
    }
    console.log(description, descriptionFicha);

    var div = document.createElement("DIV");
    div.id = id;
    div.className = "col-sm-3 document-container";


    var res = "<div class=\"col-sm-6\">&nbsp;</div>";
    if (extension !== "txt" && extension !== "png" && extension !== "gif" && extension !== "jpg") {
        res += "    <div class=\"col-sm-2\">&nbsp;</div>";
    }
    else {
        res += "    <div class=\"col-sm-2 btn-success\" onclick=\"ShowPDF('" + fileName + "');\"><i class=\"icon-eye-open bigger-120\"></i></div>";
    }
    res += "    <div class=\"col-sm-2 btn-info\"><a class=\"icon-download bigger-120\" href=\"/DOCS/" + Company.Id + "/" + fileName + "\" target=\"_blank\" style=\"color:#fff;\"></a></div>";
    res += "    <div class=\"col-sm-2 btn-danger\" onclick=\"DeleteUploadFile(" + id + ",'" + fileName + "');\"><i class=\"icon-trash bigger-120\"></i></div>";
    res += "    <div class=\"col-sm-12 iconfile\" style=\"max-width: 100%;\">";
    res += "        <div class=\"col-sm-4\"><img src=\"/images/FileIcons/" + extension + ".png\"></div>";
    res += "            <div class=\"col-sm-8 document-name\">";
    res += "                <strong title=\"Processes_17_estado issus.docx\">" + description + "</strong><br>";
    res += "                    " + Dictionary.Item_Attachment_CreateDate + ": " + date;
    res += "                            " + Dictionary.Item_Attachment_Size + ": " + ToMoneyFormat(finalSize, 2) + " MB";
    res += "                        </div>";
    res += "            </div>";

    target.appendChild(div);
    $("#" + id).html(res);

    /*<tr>
                    <td>logo.jpg</td>
                    <td></td>
                    <td>20/01/2017</td>
                    <td align="right">5.74 KB</td>
                    <td style="width:150px;">
                        <span class="btn btn-xs btn-success" onclick="ShowPDF('logo.jpg');">
                            <i class="icon-eye-open bigger-120"></i>
                        </span>
                        <span class="btn btn-xs btn-info">
                            <a class="icon-download bigger-120" href="/DOCS/12/logo.jpg" target="_blank" style="color:#fff;"></a>
                        </span>
                        <span class="btn btn-xs btn-danger" onclick="DeleteUploadFile(25,'');">
                            <i class="icon-trash bigger-120"></i>
                        </span>
                    </td>
                </tr>*/

    var tr = document.createElement('TR');
    tr.id = "tr" + id;

    var tdFileName = document.createElement('TD');
    tdFileName.appendChild(document.createTextNode(fileNameToShow));

    var td0 = document.createElement('TD');
    td0.appendChild(document.createTextNode(description));

    var td1 = document.createElement('TD');
    td1.align = 'center';
    td1.appendChild(document.createTextNode(date));

    var td2 = document.createElement('TD');
    td2.align = 'right';
    td2.appendChild(document.createTextNode(ToMoneyFormat(finalSize, 2) + " MB"));


    var td3 = document.createElement('TD');
    td3.style.width = '160px';

    var tdSpan1 = document.createElement('SPAN'); tdSpan1.className = 'btn btn-xs btn-success';
    var tdSpan2 = document.createElement('SPAN'); tdSpan2.className = 'btn btn-xs btn-info';
    var tdSpan3 = document.createElement('SPAN'); tdSpan3.className = 'btn btn-xs btn-danger';


    if (extension !== "txt" && extension !== "png" && extension !== "gif" && extension !== "jpg") {
        tdSpan1.style.visibility = "hidden";
    }

    var icon1 = document.createElement('I');
    icon1.className = 'icon-eye-open bigger-120';

    var icon2 = document.createElement('A');
    icon2.className = 'icon-download bigger-120';
    icon2.target = '_blank';
    icon2.style.color = '#fff';
    icon2.href = '/DOCS/' + ApplicationUser.CompanyId + '/' + fileName;

    var icon3 = document.createElement('I');
    icon3.className = 'icon-trash bigger-120'

    tdSpan1.appendChild(icon1);
    tdSpan1.onclick = function () { ShowPDF(fileName); };
    tdSpan2.appendChild(icon2);
    tdSpan3.appendChild(icon3);
    tdSpan3.onclick = function () { DeleteUploadFile(id, description); };

    td3.appendChild(tdSpan1);
    td3.appendChild(document.createTextNode(' '));
    td3.appendChild(tdSpan2);
    td3.appendChild(document.createTextNode(' '));
    td3.appendChild(tdSpan3);

    //tr.appendChild(tdFileName);
    tr.appendChild(td0);
    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);

    document.getElementById('TBodyDocumentsList').appendChild(tr);


    $("#PopupUploadFile").dialog("close");
}