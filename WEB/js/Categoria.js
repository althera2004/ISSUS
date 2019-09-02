function CmbCategoryChanged() {
    categorySelected = $("#CmbCategory").val() * 1;
    var text = "";
    for (var x = 0; x < categorias.length; x++) {
        if (categorias[x].Id === categorySelected) {
            text = categorias[x].Description;
        }
    }

    $("#TxtCategory").val(text);
}

function FillCmbCategory() {
    VoidTable("CmbCategory");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbCategory").appendChild(optionDefault);

    for (var x = 0; x < categorias.length; x++) {
        var option = document.createElement("option");
        option.value = categorias[x].Id;
        option.appendChild(document.createTextNode(categorias[x].Description));
        if (categorySelected === categorias[x].Id) {
            option.selected = true;
        }

        document.getElementById("CmbCategory").appendChild(option);
    }
}

function RenderCategoryTable()
{
    // Cargar las categorias en la tabla
    var target = document.getElementById("CategorySelectable");
    VoidTable("CategorySelectable");
    categorias.sort(CompareDocumentCategory);
    for (var x = 0; x < categorias.length; x++) {
        var category = categorias[x];
        var tr = document.createElement("tr");
        tr.id = category.Id;
        var td1 = document.createElement("td");
        var td2 = document.createElement("td");
        if (categorySelected === category.Id) { td1.style.fontWeight = "bold"; }
        td1.appendChild(document.createTextNode(category.Description));

        var div = document.createElement("div");
        var span1 = document.createElement("span");
        span1.className = "btn btn-xs btn-success";
        span1.title = Dictionary.Common_SelectAll;
        var i1 = document.createElement("i");
        i1.className = "icon-star bigger-120";
        span1.appendChild(i1);

        if (categorySelected === category.Id) {
            span1.onclick = function () { alertUI(Dictionary.Common_Selected, "dialogCategory"); };
        }
        else {
            span1.onclick = function () { CategoryChanged(this); };
        }

        div.appendChild(span1);

        var span2 = document.createElement("span");
        span2.className = "btn btn-xs btn-info";
        span2.title = Dictionary.Common_Edit;
        var i2 = document.createElement("i");
        i2.className = "icon-edit bigger-120";
        span2.appendChild(i2);
        if (category.Editable === "Kernel") {
            span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete, "dialogCategory"); };
        }
        else if (category.Editable === "InUse") {
            span2.onclick = function () { alertUI(Dictionary.InUse, "dialogCategory"); };
        }
        else {
            span2.onclick = function () { CategoryUpdate(this); };
        }

        div.appendChild(document.createTextNode(" "));
        div.appendChild(span2);

        var span3 = document.createElement("span");
        span3.className = "btn btn-xs btn-danger";
        span3.title = Dictionary.Common_Delete;
        var i3 = document.createElement("i");
        i3.className = "icon-trash bigger-120";
        span3.appendChild(i3);

        if (category.Deletable === "Kernel") {
            span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete, "dialogCategory"); };
        }
        else if (category.Deletable === "InUse" || category.Id == categorySelected) {
            span3.onclick = function () { alertUI(Dictionary.Common_ErrorMessage_InUse, "dialogCategory"); };
        }
        else {
            span3.onclick = function () { CategoryDelete(this); };
        }

        div.appendChild(document.createTextNode(' '));
        div.appendChild(span3);
        td2.appendChild(div);

        tr.appendChild(td1);
        tr.appendChild(td2);
        target.appendChild(tr);
    }
}

function CompareDocumentCategory(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) return -1;
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) return 1;
    return 0;
}

function CategoryChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    $("#dialogCategory").dialog("close");
    categorySelected = id;
    SetCategoryText();
    FillCmbCategory();
}

function SetCategoryText() {
    for (var x = 0; x < categorias.length; x++) {
        if (categorias[x].Id === categorySelected) {
            $("#TxtCategory").val(categorias[x].Description);
            break;
        }
    }
}

function SetCategoria(e) {
    var categoriaId = e.target.id.split("-")[1];
    var comboItems = document.getElementById("CmbCategoria").childNodes;
    for (var x = 0; x < comboItems.length; x++) {
        var item = comboItems[x];
        if (item.tagName == "OPTION") {
            if (item.value == categoriaId) {
                item.selected = true;
            }
            else {
                item.selected = false;
            }
        }
    }

    FillCmbCategory();
    $("#dialogCategory").dialog("close");
}

function CategoryDelete(sender) {
    document.getElementById("dialogCategory").parentNode.style.cssText += "z-Index:1039 !important";
    $("#CategoryName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#CategoryDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_DocumentCategory_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons": 
        [
            {
                "id": "BtnDeleteCategoryOk",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp; " + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    $(this).dialog("close");
                    CategoryDeleteConfirmed(Selected);
                }
            },
            {
                "id": "BtnDeleteCategoryCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp; " + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ],
        "close": function () {document.getElementById("dialogCategory").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function CategoryDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var description = "";
    for (var x = 0; x < categorias.length; x++) {
        if (categorias[x].Id === id) {
            description = categorias[x].Description;
            break;
        }
    }

    var data = {
        "categoryId": id,
        "description": description,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow();
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/CategoryDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var y = 0; y < categorias.length; x++) {
        if (categorias[y].Id !== id) {
            temp.push(categorias[y]);
        }
    }

    categorias = new Array();
    for (var z = 0; z < temp.length; z++) {
        categorias.push(temp[z]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById("CategorySelectable");
    for (var w = 0; w < target.childNodes.length; w++) {
        if (target.childNodes[w].id === id) {
            target.childNodes[w].style.display = "none";
            break;
        }
    }

    FillCmbCategory();
}

function CategoryUpdate(sender) {
    document.getElementById("dialogCategory").parentNode.style.cssText += "z-Index:1039 !important";
    $("#TxtCategoryName").val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    $("#CategoryUpdateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 500,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_DocumentCategory_Popup_Update_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        var ok = true;
                        if ($("#TxtCategoryName").val() === "") {
                            $("#TxtCategoryNameErrorRequired").show();
                            ok = false;
                        }
                        else {
                            $("#TxtCategoryNameErrorRequired").hide();
                        }

                        var duplicated = false;
                        for (var x = 0; x < categorias.length; x++) {
                            if ($("#TxtCategoryName").val().toLowerCase() === categorias[x].Description.toLowerCase() && categorias[x].Id !== Selected) {
                                duplicated = true;
                                break;
                            }
                        }

                        if (duplicated === true) {
                            $("#TxtCategoryNameErrorDuplicated").show();
                            ok = false;
                        }
                        else {
                            $("#TxtCategoryNameErrorDuplicated").hide();
                        }

                        if (ok === false) {
                            window.scrollTo(0, 0);
                            return false;
                        }

                        $("#TxtCategoryNameErrorRequired").hide();
                        $("#TxtCategoryNameErrorDuplicated").hide();
                        $(this).dialog("close");
                        CategoryUpdateConfirmed(Selected, $("#TxtCategoryName").val());
                    }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $("#TxtCategoryNameErrorRequired").hide();
                        $("#TxtCategoryNameErrorDuplicated").hide();
                        $(this).dialog("close");
                    }
                }
            ],
        "close": function () { document.getElementById("dialogCategory").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function CategoryUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var data = {
        "categoryId": id,
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/CategoryUpdate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var x = 0; x < categorias.length; x++) {
        if (categorias[x].Id !== id) {
            temp.push(categorias[x]);
        }
        else {
            var item = categorias[x];
            temp.push({ "Id": item.Id, "Description": newDescription, "Active": item.Active, "Deletable": item.Delete });
        }
    }

    categorias = new Array();
    for (var y = 0; y < temp.length; y++) {
        categorias.push(temp[y]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById("CategorySelectable");
    for (var z = 0; z < target.childNodes.length; z++) {
        if (target.childNodes[z].id === id) {
            target.childNodes[z].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (categorySelected === id) {
        $("#TxtCategory").val(newDescription);
    }

    FillCmbCategory();
}

function CategoryInsert() {
    document.getElementById("dialogCategory").parentNode.style.cssText += "z-Index:1039 !important";
    $("#TxtCategoryNewName").va("");
    Selected = 0;
    $("#CategoryInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 500,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Document_Popup_AddCotegory_Title + "</h4>",
        "title_html": true,
        "buttons":
        [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    if ($("#TxtCategoryNewName").val() === "") {
                        $("#TxtCategoryNewNameErrorRequired").show();
                        ok = false;
                    }
                    else {
                        $("#TxtCategoryNewNameErrorRequired").hide();
                    }

                    var duplicated = false;
                    for (var x = 0; x < categorias.length; x++) {
                        if ($("#TxtCategoryNewName").val().toLowerCase() === categorias[x].Description.toLowerCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        $("#TxtCategoryNewNameErrorDuplicated").show();
                        ok = false;
                    }
                    else {
                        $("#TxtCategoryNewNameErrorDuplicated").hide();
                    }

                    if (ok === false) {
                        window.scrollTo(0, 0);
                        return false;
                    }

                    $("#TxtCategoryNewNameErrorRequired").hide();
                    $("#TxtCategoryNewNameErrorDuplicated").hide();
                    $(this).dialog("close");
                    CategoryInsertConfirmed(Selected, $("TxtCategoryNewName").val());
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $("#TxtCategoryNewNameErrorRequired").hide();
                    $("#TxtCategoryNewNameErrorDuplicated").hide();
                    $(this).dialog("close");
                }
            }
        ],
        "close": function () { document.getElementById("dialogCategory").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function CategoryInsertConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var data = {
        "categoryId": 0,
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    var newId = 0;
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/CategoryInsert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                var categoria = { "Id": newId, "Description": newDescription, "Active": true, "Deletable": true };
                categorias.push(categoria);

                // 3.- Modificar la fila de la tabla del popup
                RenderCategoryTable();

                FillCmbCategory();
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}