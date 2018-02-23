function CompanyAddressDeleteConfirmed(id)
{
    var webMethod = "/Async/CompanyActions.asmx/DeleteAddress";
    var actionAddress = id;
    var data = {
        "companyId": Company.Id,
        "addressId": id,
        "userId": user.Id
    };

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (response) {
            if (response.d.Success === true) {
                // Traspaso de supervivientes
                var temp = new Array();
                for (var x = 0; x < addresses.length; x++) {
                    if (addresses[x].Id != actionAddress) {
                        temp.push(addresses[x]);
                    }
                }

                addresses = new Array();
                for (var x = 0; x < temp.length; x++) {
                    addresses.push(temp[x]);
                }

                $("#dialogShowAddress").dialog("close");
                ShowAddressPopup();
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
function ShowAddAddressPopup(actionSelected, sender) {
    document.getElementById("dialogShowAddress").parentNode.style.cssText += "z-Index:1039 !important";
    action = actionSelected;
    actionAddress = -1;
    ResetNewAddressFormValidation();

    // Si la accion == 1 hay vaciar los campos de posibles anteriores valores
    if (action === 1) {
        document.getElementById("TxtNewAddress").value = "";
        document.getElementById("TxtNewAddressPostalCode").value = "";
        document.getElementById("TxtNewAddressCity").value = "";
        document.getElementById("TxtNewAddressProvince").value = "";
        document.getElementById("TxtNewAddressCountry").value = "";
        document.getElementById("TxtNewAddressPhone").value = "";
        document.getElementById("TxtNewAddressFax").value = "";
        document.getElementById("TxtNewAddressMobile").value = "";
        document.getElementById("TxtNewAddressEmail").value = "";
        for (var x = 1; x < addresses.length; x++) {
            addresses.selected = false;
        }

        $("#CmbPais").ddslick("select", { index: 0 });
    }

    // Si la accion == 2 hay que rellenar los datos del formulario
    if (action === 2) {
        var id = sender.parentNode.parentNode.parentNode.id * 1;
        actionAddress = id;
        var item = null;
        for (var x = 0; x < addresses.length; x++) {
            if (addresses[x].Id === id) {
                item = addresses[x];
                break;
            }
        }

        if (item != null) {
            document.getElementById("TxtNewAddress").value = item.Address;
            document.getElementById("TxtNewAddressPostalCode").value = item.PostalCode;
            document.getElementById("TxtNewAddressCity").value = item.City;
            document.getElementById("TxtNewAddressProvince").value = item.Province;
            document.getElementById("TxtNewAddressCountry").value = item.Country;
            document.getElementById("TxtNewAddressPhone").value = item.Phone;
            document.getElementById("TxtNewAddressFax").value = item.Fax;
            document.getElementById("TxtNewAddressMobile").value = item.Mobile;
            document.getElementById("TxtNewAddressEmail").value = item.Email;

            var i = 0;
            for (var x = 0; x < ddData.length; x++) {
                if (ddData[x].value == item.Country) {
                    i = x;
                    break;
                }
            }

            document.getElementById("CmbPais").value = i;
            $("#CmbPais").ddslick({ data: ddData });
            $("#CmbPais").ddslick("select", { index: i });
        }
    }

    dialog = $("#dialogAddAddress").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: (action == 1 ? Dictionary.Item_CompanyAddress_PopupTile_Add : Dictionary.Item_CompanyAddress_PopupTile_Edit),
        title_html: true,
        width: 800,
        height: 500,
        buttons:
        [
            {
                "id": "BtnNewAddresSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () { SaveAddress(); }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ],
        close: function () { document.getElementById('dialogShowAddress').parentNode.style.cssText += 'z-Index:1050 !important'; }
    });
}

function CompanyAddressDelete(sender) {
    document.getElementById("dialogShowAddress").parentNode.style.cssText += "z-Index:1039 !important";
    $("#AddressName").html(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
    Selected = sender.parentNode.parentNode.parentNode.id * 1;
    var dialog = $("#AddressDeleteDialog").removeClass("hide").dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Common_Delete,
        title_html: true,
        width: 400,
        buttons:
        [
            {
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    $(this).dialog("close");
                    CompanyAddressDeleteConfirmed(Selected);
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () { document.getElementById("dialogShowAddress").parentNode.style.cssText += "z-Index:1050 !important"; }
    });
}

function ShowAddressPopup() {
    // Cargar direcciones en la table
    VoidTable("DireccionesSelectable");
    var target = document.getElementById("DireccionesSelectable");
    for (var x = 0; x < addresses.length; x++) {
        AddAddressPopupRow(addresses[x], target);
    }

    var dialog = $("#dialogShowAddress").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: Dictionary.Item_CompanyAddress_PopupTitle,
        title_html: true,
        width: 800,
        buttons:
        [
            {
                "id": "BtnNewAddresSave",
                "html": "<i class=\"icon-plus bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () { ShowAddAddressPopup(1); }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function FillAddressFields()
{
    for (var x = 0; x < addresses.length; x++) {
        if (addresses[x].Id == addressSelected) {            
            $("#TxtDireccion").val(addresses[x].Address);
            $("#TxtPostalCode").val(addresses[x].PostalCode);
            $("#TxtCity").val(addresses[x].City);
            $("#TxtProvince").val(addresses[x].Province);
            $("#TxtCountry").val(GetCountryById(addresses[x].Country * 1));
            $("#TxtPhone").val(addresses[x].Phone);
            $("#TxtMobile").val(addresses[x].Mobile);
            $("#TxtFax").val(addresses[x].Fax);
            $("#TxtEmail").val(addresses[x].Email);
            break;
        }
    }
}

function CompanySetDefaultAddressAddress()
{
    var webMethod = "/Async/CompanyActions.asmx/SetDefaultAddress";
    var data = {
        "companyId": Company.Id,
        "addressId": addressSelected,
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
            alert(jqXHR.responseText);
        }
    });

    return false;
}