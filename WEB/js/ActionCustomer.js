var CustomerSelected = 0;
var ItemIdDelete;
var ItemIdUpdate;
var Cmb;
var itemName = "Customer";
var popupDialogId = "#dialog" + itemName;
var popupInsertDialogId = "#" + itemName + "InsertDialog";
var popupUpdateDialogId = "#" + itemName + "UpdateDialog";
var popupDeleteDialogId = "#" + itemName + "DeleteDialog";
var sourceList;
eval("sourceList = " + itemName + "s;");

// Bar popup for bar item Customer
function ShowCustomerBarPopup(cmb) {
    Cmb = cmb;
    CustomerSelected = cmb.val() * 1;
    CustomerRenderPopup();
    var dialog = $(popupDialogId).removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: eval("Dictionary.Item_" + itemName + "s"),
        title_html: true,
        width: 600,
        buttons:
        [
            {
                id: "Btn" + itemName + "Save",
                html: "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                click: function () { CustomerInsert(); }
            },
            {
                html: "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                click: function () { $(this).dialog("close"); }
            }
        ]
    });
}

// Selection functions for bar item Customer
function CustomerChanged(sender) {
    var id = sender.parentNode.parentNode.parentNode.id * 1;
    Cmb.val(id);
    $(popupDialogId).dialog("close");
}

// Insert functions for bar item Customer
function CustomerInsert(sender) {
    document.getElementById("TxtCustomerNewNameErrorRequired").style.display = "none";
    document.getElementById("TxtCustomerNewNameErrorDuplicated").style.display = "none";
    $("#TxtCustomerNewName").val("");
    var Selected = 0;
    var dialog = $(popupInsertDialogId).removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: "<h4 class=\"smaller\">" + Dictionary.Item_Equipment_Popup_AddCustomer_Title + "</h4>",
        title_html: true,
        buttons:
        [
            {
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    var duplicated = false;
                    for (var x = 0; x < Customers.length; x++) {
                        if (document.getElementById("TxtCustomerNewName").value.toLowerCase() === Customers[x].Description.toLowerCase()) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById("TxtCustomerNewNameErrorDuplicated").style.display = "block";
                        ok = false;
                    }
                    else {
                        document.getElementById("TxtCustomerNewNameErrorDuplicated").style.display = "none";
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById("TxtCustomerNewNameErrorRequired").style.display = "none";
                    document.getElementById("TxtCustomerNewNameErrorDuplicated").style.display = "none";
                    $(this).dialog("close");
                    CustomerInsertConfirmed(document.getElementById("TxtCustomerNewName").value);
                    return false;
                }
            },
            {
                "html": "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function CustomerInsertConfirmed(newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/" + itemName + "Actions.asmx/Insert";
    var description = "";
    var data = {
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
    };

    var newId = 0;
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            LoadingHide();
            if (response.d.Success === true) {
                newId = response.d.MessageError * 1;

                // 2.- Añadir en HTML
                Customers.push({ "Id": newId, "Description": newDescription, "Active": true, "Deletable": true });

                // 3.- Modificar la fila de la tabla del popup
                CustomerRenderPopup();
                CmbReporterCustomersFill();
                Cmb.val(CustomerSelected);
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

// Update function for bar item Customer
function CustomerUpdate(sender) {
    document.getElementById("TxtCustomerNameErrorRequired").style.display = "none";
    document.getElementById("TxtCustomerNameErrorDuplicated").style.display = "none";
    $("#TxtCustomerName").val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    ItemIdUpdate = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $(popupUpdateDialogId).removeClass("hide").dialog({
        resizable: false,
        width: 600,
        modal: true,
        title: Dictionary.Common_Edit,
        title_html: true,
        buttons:
        [
            {
                "html": "<i class='icon-ok bigger-110'></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    if (document.getElementById("TxtCustomerName").value === "") {
                        document.getElementById("TxtCustomerNameErrorRequired").style.display = "block";
                        ok = false;
                    }
                    else {
                        document.getElementById("TxtCustomerNameErrorRequired").style.display = "none";
                    }

                    var duplicated = false;
                    for (var x = 0; x < Customers.length; x++) {
                        if (document.getElementById("TxtCustomerName").value.toLowerCase() === Customers[x].Description.toLowerCase() && Selected !== Customers[x].Id && Customers[x].Active === true) {
                            duplicated = true;
                            break;
                        }
                    }

                    if (duplicated === true) {
                        document.getElementById("TxtCustomerNameErrorDuplicated").style.display = "block";
                        ok = false;
                    }
                    else {
                        document.getElementById("TxtCustomerNameErrorDuplicated").style.display = "none";
                    }

                    if (ok === false) { window.scrollTo(0, 0); return false; }

                    document.getElementById("TxtCustomerNameErrorRequired").style.display = "none";
                    document.getElementById("TxtCustomerNameErrorDuplicated").style.display = "none";
                    $(this).dialog("close");
                    CustomerUpdateConfirmed(ItemIdUpdate, document.getElementById("TxtCustomerName").value);
                    return null;
                }
            },
            {
                "html": "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]

    });
}

function CustomerUpdateConfirmed(id, newDescription) {
    // 1.- Modificar en la BBDD
    var webMethod = "/Async/" + itemName + "Actions.asmx/Update";
    var description = "";
    for (var x = 0; x < Customers.length; x++) {
        if (Customers[x].Id === id) {
            description = Customers[x].Description;
            break;
        }
    }
    var data = {
        "customerId": id,
        "description": newDescription,
        "companyId": Company.Id,
        "userId": user.Id
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Modificar en HTML
    var temp = new Array();
    for (var y = 0; y < sourceList.length; y++) {
        if (sourceList[y].Id !== ItemIdUpdate) {
            temp.push(sourceList[y]);
        }
        else {
            var item = sourceList[y];
            temp.push({ "Id": item.Id, "Description": newDescription, "Active": item.Active, "Deletable": item.Delete });
        }
    }

    Customers = new Array();
    for (var z = 0; z < temp.length; z++) {
        Customers.push(temp[z]);
    }

    // 3.- Modificar la fila de la tabla del popup
    var target = document.getElementById("SelectableCustomer");
    for (var w = 0; w < target.childNodes.length; w++) {
        if (target.childNodes[w].id === ItemIdUpdate) {
            target.childNodes[w].childNodes[0].innerHTML = newDescription;
            break;
        }
    }

    // 4.- Modificar el texto si es el seleccionado
    if (CustomerSelected === ItemIdUpdate) {
        document.getElementById("TxtCustomer").value = newDescription;
    }

    CmbReporterCustomersFill();
    Cmb.val(CustomerSelected);
}

// Delete functions for bar item Customer
function CustomerDelete(sender) {
    $("#CustomerName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    ItemIdDelete = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $(popupDeleteDialogId).removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Common_Delete,
        title_html: true,
        buttons:
        [
            {
                "html": "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    $(this).dialog("close");
                    CustomerDeleteConfirmed(ItemIdDelete);
                }
            },
            {
                "html": "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function CustomerDeleteConfirmed(id) {
    // 1.- Desactivar en la BBDD
    var webMethod = "/Async/" + itemName + "Actions.asmx/Delete";
    var description = "";
    for (var x = 0; x < sourceList.length; x++) {
        if (sourceList[x].Id === id) {
            description = sourceList[x].Description;
            break;
        }
    }

    var data = {
        "CustomerId": id,
        "description": description,
        "companyId": Company.Id,
        "userId": user.Id
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
            if (response.d.Success !== true) { alertUI(response.d.MessageError); }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });

    // 2.- Desactivar en HTML
    var temp = new Array();
    for (var y = 0; y < sourceList.length; y++) {
        if (sourceList[y].Id !== ItemIdDelete) { temp.push(sourceList[y]); }
    }

    Customers = new Array();
    for (var z = 0; z < temp.length; z++) {
        Customers.push(temp[z]);
    }

    // 3.- Eliminar la fila de la tabla del popup
    var target = document.getElementById("Selectable" + itemName);
    for (var w = 0; w < target.childNodes.length; w++) {
        if (target.childNodes[w].id === id) {
            target.childNodes[w].style.display = "none";
            break;
        }
    }

    CmbReporterCustomersFill();
    Cmb.val(CustomerSelected);
}

// Common scripts
function CustomerRenderPopup() {
    VoidTable("Selectable" + itemName);
    var target = document.getElementById("Selectable" + itemName);
    sourceList.sort(CompareCustomers);
    for (var x = 0; x < sourceList.length; x++) {
        CustomerPopupRow(sourceList[x], target);
    }
}

function CompareCustomers(a, b) {
    if (a.Description.toUpperCase() < b.Description.toUpperCase()) { return -1; }
    if (a.Description.toUpperCase() > b.Description.toUpperCase()) { return 1; }
    return 0;
}

function CustomerPopupRow(item, target) {
    if (item.Active === false) { return; }
    var tr = document.createElement("tr");
    tr.id = item.Id;
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    if (CustomerSelected === item.Id) {
        td1.style.fontWeight = "bold";
    }
    td1.appendChild(document.createTextNode(item.Description));

    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if (CustomerSelected === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else {
        span1.onclick = function () { CustomerChanged(this); };
    }

    div.appendChild(span1);

    var span2 = document.createElement("span");
    span2.className = "btn btn-xs btn-info";
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement("i");
    i2.className = "icon-edit bigger-120";
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span2);

    if (item.Id < 0) {
        span2.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else {
        span2.onclick = function () { CustomerUpdate(this); };
    }

    var span3 = document.createElement("span");
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);

    if (CustomerSelected === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else if (item.Id < 0) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_Kernel_Delete); };
    }
    else if (item.Deletable === false) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else {
        span3.onclick = function () { CustomerDelete(this); };
    }

    div.appendChild(document.createTextNode(" "));
    div.appendChild(span3);
    td2.appendChild(div);

    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}