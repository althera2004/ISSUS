﻿jQuery(function ($) {
    $("#CmbIdioma").val(Company.Language);

    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
            if (("title_html" in this.options) && this.options.title_html === true) {
                title.html($title);
            }
            else {
                title.text($title);
            }
        }
    }));

    $("#BtnSaveLogo").click(SaveLogo);
    $("#BtnCancel").click(function () { document.location = "Dashboard.aspx"; });
    $("#BtnSave").click(SaveCompany);

    $("#BtnShowAddress").on("click", function (e) {
        e.preventDefault();
        ShowAddressPopup();
    });

    $("#BtnEquipmentChangeImage").on("click", function (e) {
        e.preventDefault();
        CompanyChangeImage();
    });

    $("#BtnCountryAdd").on("click", function (e) {
        e.preventDefault();
        SaveCountries();
    });

    $("#BtnCountryDiscard").on("click", function (e) {
        e.preventDefault();
        DiscardCountries();
    });
    
    $("#imgInp").change(function () {
        readURL(this);
    });

    if (ApplicationUser.ShowHelp === true) {
        SetToolTip("TxtName", Dictionary.Item_Company_Help_Name);
        SetToolTip("TxtNif", Dictionary.Item_Company_Help_Nif);
        SetToolTip("DivCmbAddress", Dictionary.Item_Company_Help_Common_SelectAddress);
        SetToolTip("BtnShowAddress", Dictionary.Item_Company_Help_BAR_Addresses);
        SetToolTip("TxtNewAddress", Dictionary.Item_Company_Help_Address);
        SetToolTip("TxtNewAddressPostalCode", Dictionary.Item_Company_Help_ZipCode);
        SetToolTip("TxtNewAddressCity", Dictionary.Item_Company_Help_City);
        SetToolTip("TxtNewAddressProvince", Dictionary.Item_Company_Help_Provincia);
        SetToolTip("DivCmbPais", Dictionary.Item_Company_Help_Pais);
        SetToolTip("TxtNewAddressPhone", Dictionary.Item_Company_Help_Phone);
        SetToolTip("TxtNewAddressFax", Dictionary.Item_Company_Help_Fax);
        SetToolTip("TxtNewAddressMobile", Dictionary.Item_Company_Help_Mobile);
        SetToolTip("TxtNewAddressEmail", Dictionary.Item_Company_Help_Email);

        $("[data-rel=tooltip]").tooltip();
        $("[data-rel=popover]").popover({ "container": "body" });
    }

    $("#CmbPais").ddslick({ data: ddData });

    if (ApplicationUser.ShowHelp === true) {
        $("#DivCmbPais .dd-options").on("mouseover", function () { $("#DivCmbPais").tooltip("destroy"); });
        $("#DivCmbPais .dd-options").on("mouseout", function () { SetToolTip("DivCmbPais", Dictionary.Item_Employee_Help_Pais); });
    }

    $("#EquipmentImg").css("width", "");
    $("#EquipmentImg").css("height", "");
    $("#EquipmentImg").css("max-height", "30px");

    var placeholder = $("#piechart-placeholder").css({ "width": "90%", "min-height": "150px" });

    console.log("QuotePercentage", ToMoneyFormat(diskQuote[diskQuote.length-1].value, 2));
    $("#QuotePercentage").html(ToMoneyFormat(diskQuote[diskQuote.length - 1].value, 2));

    function drawPieChart(placeholder, data, position) {
        CmbAddressChanged();
        console.log("drawPieChart1", data);
        console.log("drawPieChart2", diskQuote);
        $.plot(placeholder, diskQuote, {
            "series": {
                "pie": {
                    "show": true,
                    "tilt": 0.8,
                    "highlight": {
                        "opacity": 0.25
                    },
                    "stroke": {
                        "color": "#fff",
                        "width": 2
                    },
                    "startAngle": 45
                }
            },
            "legend": {
                "show": true,
                "position": position || "e",
                "labelBoxBorderColor": null,
                "margin": [-30, 15]
            },
            "grid": {
                "hoverable": true,
                "clickable": false
            }
        });
    }

    drawPieChart(placeholder, diskQuote);
    document.getElementById("disk").className = "tab-pane";
    placeholder.data("chart", data);
    placeholder.data("draw", drawPieChart);
});

function AddAddressPopupRow(item, target) {
    var tr = document.createElement("tr");
    tr.id = item.Id;
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    if (addressSelected === item.Id) {
        td1.style.fontWeight = "bold";
    }

    td1.appendChild(document.createTextNode(item.Address + ", " + item.City));

    var div = document.createElement("div");
    var span1 = document.createElement("span");
    span1.className = "btn btn-xs btn-success";
    span1.title = Dictionary.Common_SelectAll;
    var i1 = document.createElement("i");
    i1.className = "icon-star bigger-120";
    span1.appendChild(i1);

    if (addressSelected === item.Id) {
        span1.onclick = function () { alertUI(Dictionary.Common_Selected); };
    }
    else{
        span1.onclick = function () { AddressChanged(this); };
    }

    div.appendChild(span1);

    var span2 = document.createElement("span");
    span2.className = "btn btn-xs btn-info";
    span2.onclick = function () { ShowAddAddressPopup(2, this); };
    span2.title = Dictionary.Common_Edit;
    var i2 = document.createElement("i");
    i2.className = "icon-edit bigger-120";
    span2.appendChild(i2);
    div.appendChild(document.createTextNode(" "));
    div.appendChild(span2);

    var span3 = document.createElement("span");
    span3.className = "btn btn-xs btn-danger";
    span3.title = Dictionary.Common_Delete;
    var i3 = document.createElement("i");
    i3.className = "icon-trash bigger-120";
    span3.appendChild(i3);
    if (addressSelected === item.Id || Company.DefaultAddress.Id === item.Id) {
        span3.onclick = function () { alertUI(Dictionary.Common_Error_NoDeletable); };
    }
    else{
        span3.onclick = function () { CompanyAddressDelete(this); };
    }

    div.appendChild(document.createTextNode(" "));
    div.appendChild(span3);
    td2.appendChild(div);
    tr.appendChild(td1);
    tr.appendChild(td2);
    target.appendChild(tr);
}

function AddressChanged(sender) {
    $("#dialogShowAddress").dialog("close");
    var id = sender.parentNode.parentNode.parentNode.id;
    addressSelected = id * 1;
    document.getElementById("CmbAddress").value = addressSelected;
    FillAddressFields();
    CompanySetDefaultAddressAddress();
    FillCmbAddresses();
}

function CmbAddressChanged()
{
    addressSelected = $("#CmbAddress").val();
    FillAddressFields();
    CompanySetDefaultAddressAddress();
}

function FillCmbAddresses()
{
    VoidTable("CmbAddress");
    for (var x = 0; x < addresses.length; x++)
    {
        var option = document.createElement("option");
        option.value = addresses[x].Id;
        var text = addresses[x].Address + ", " + addresses[x].City;
        option.appendChild(document.createTextNode(text));
        if(addresses[x].Id === addressSelected)
        {
            option.selected = true;
        }

        document.getElementById("CmbAddress").appendChild(option);
    }
}

function GetCountryById(id) {
    for (var x = 0; x < ddData.length; x++) {
        if (ddData[x].value * 1 === id) {
            return ddData[x].description;
        }
    }

    return "";
}

function ValidateForm() {
    var ok = true;
    if ($("#TxtName").val() === "") {
        ok = false;
        $("#TxtNameErrorRequired").style.display = "block";
        $("#TxtNameLabel").css("color", Color.Error);
    }
    else {
        $("#TxtNameErrorRequired").hide();
        $("#TxtNameLabel").css("color", "#000");
    }

    if ($("#TxtNif").val() === "") {
        ok = false;
        $("#TxtNifErrorRequired").show();;
        $("#TxtNifErrorMalformed").hide();
        $("#TxtNifLabel").css("color", Color.Error);
    }
    else {
        $("#TxtNifErrorRequired").hide();

        if (valida_nif_cif_nie(document.getElementById("TxtNif").value) < 1 && $("#TxtCountry").val() === "España") {
            ok = false;
            $("#TxtNifErrorMalformed").style.display = "block";
            $("#TxtNifLabel").css("color", Color.Error);
        }
        else {
            $("#TxtNifErrorMalformed").hide();
            $("#TxtNifLabel").css("color", "#000");
        }
    }

    return ok;
}

function ValidateNewForm() {
    var ok = true;
    var ddData = $("#CmbPais").data("ddslick");
    var country = ddData.selectedData.value;
    document.getElementById("TxtNewAddressCountry").value = country;
    if (!RequiredFieldText("TxtNewAddress")) { ok = false; }
    if (!RequiredFieldText("TxtNewAddressPostalCode")) { ok = false; }
    if (!RequiredFieldText("TxtNewAddressCity")) { ok = false; }
    if (!RequiredFieldText("TxtNewAddressProvince")) { ok = false; }
    if (!RequiredFieldText("TxtNewAddressCountry")) { ok = false; }
    if (!RequiredFieldText("TxtNewAddressEmail")) { ok = false;}
    else { if (!MalFormedEmail("TxtNewAddressEmail")) { ok = false; } }
    if(!RequiredBothFieldText("TxtNewAddressPhone", "TxtNewAddressMobile")) { ok = false; }
    return ok;
}

function ResetNewAddressFormValidation() {
    $("#TxtNewAddressErrorRequired").hide();
    $("#TxtNewAddressPostalCodeErrorRequired").hide();
    $("#TxtNewAddressCityErrorRequired").hide();
    $("#TxtNewAddressProvinceErrorRequired").hide();
    $("#TxtNewAddressCountryErrorRequired").hide();
    $("#TxtNewAddressEmailErrorRequired").hide();
    $("#TxtNewAddressPhoneErrorRequired").hide();
    $("#TxtNewAddressMobileErrorRequired").hide();

    $("#TxtNewAddressLabel").css("color", "#000");
    $("#TxtNewAddressPostalCodeLabel").css("color", "#000");
    $("#TxtNewAddressCityLabel").css("color", "#000");
    $("#TxtNewAddressProvinceLabel").css("color", "#000");
    $("#TxtNewAddressCountryLabel").css("color", "#000");
    $("#TxtNewAddressEmailLabel").css("color", "#000");
    $("#TxtNewAddressPhoneLabel").css("color", "#000");
    $("#TxtNewAddressMobileLabel").css("color", "#000");
}

function SaveAddress() {
    if(!ValidateNewForm()) { window.scrollTo(0, 0); return false; }
    if(action===1) {
        AddressSave();
    }
    else {
        AddressUpdate();
    }
}

function AddressSave()
{
    var data = {
        "address":
        {
            "Id": -1,
            "Company": { "Id": Company.Id },
            "Address": $("#TxtNewAddress").val(),
            "PostalCode": $("#TxtNewAddressPostalCode").val(),
            "City": $("#TxtNewAddressCity").val(),
            "Province": $("#TxtNewAddressProvince").val(),
            "Country": $("#TxtNewAddressCountry").val(),
            "Phone": $("#TxtNewAddressPhone").val(),
            "Mobile": $("#TxtNewAddressMobile").val(),
            "Fax": $("#TxtNewAddressFax").val(),
            "Email": $("#TxtNewAddressEmail").val(),
            "Notes": ""
        },
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/CompanyActions.asmx/SaveAddress",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success === true) {                
                $("#dialogAddAddress").dialog("close");    
                addresses.push(
                {
                    "Id": response.d.MessageError * 1,
                    "Company": { "Id": Company.Id },
                    "Address": $("#TxtNewAddress").val(),
                    "PostalCode": $("#TxtNewAddressPostalCode").val(),
                    "City": $("#TxtNewAddressCity").val(),
                    "Province": $("#TxtNewAddressProvince").val(),
                    "Country": $("#TxtNewAddressCountry").val(),
                    "Phone": $("#TxtNewAddressPhone").val(),
                    "Mobile": $("#TxtNewAddressMobile").val(),
                    "Fax": $("#TxtNewAddressFax").val(),
                    "Email": $("#TxtNewAddressEmail").val(),
                    "Notes": ""
                });
                ShowAddressPopup();
                FillCmbAddresses();
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alert(jqXHR.responseText);
        }
    });
}

function AddressUpdate()
{
    var oldAddress = null;
    for(var x=0; x<addresses.length;x++)
    {
        if(addresses[x].Id === actionAddress)
        {
            oldAddress = addresses[x];
            break;
        }
    }

    if(oldAddress!==null)
    {
        var data = {
            "oldAddress": oldAddress,
            "newAddress":
            {
                "Id": actionAddress,
                "Company": { "Id": Company.Id },
                "Address": $("#TxtNewAddress").val(),
                "PostalCode": $("#TxtNewAddressPostalCode").val(),
                "City": $("#TxtNewAddressCity").val(),
                "Province": $("#TxtNewAddressProvince").val(),
                "Country": $("#TxtNewAddressCountry").val(),
                "Phone": $("#TxtNewAddressPhone").val(),
                "Mobile": $("#TxtNewAddressMobile").val(),
                "Fax": $("#TxtNewAddressFax").val(),
                "Email": $("#TxtNewAddressEmail").val(),
                "Notes": ""
            },
            "userId": user.Id
        };

        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            "type": "POST",
            "url": "/Async/CompanyActions.asmx/UpdateAddress",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (response) {
                LoadingHide();
                if (response.d.Success === true) {
                    $("#dialogAddAddress").dialog("close");

                    for(var x=0;x<addresses.length;x++)
                    {
                        if(addresses[x].Id === actionAddress)
                        {
                            addresses[x].Address = $("#TxtNewAddress").val();
                            addresses[x].PostalCode = $("#TxtNewAddressPostalCode").val();
                            addresses[x].City = $("#TxtNewAddressCity").val();
                            addresses[x].Province = $("#TxtNewAddressProvince").val();
                            addresses[x].Country = $("#TxtNewAddressCountry").val();
                            addresses[x].Phone = $("#TxtNewAddressPhone").val();
                            addresses[x].Mobile = $("#TxtNewAddressMobile").val();
                            addresses[x].Fax = $("#TxtNewAddressFax").val();
                            addresses[x].Email = $("#TxtNewAddressEmail").val();

                            if(addressSelected === actionAddress)
                            {
                                var country = GetCountryById($("#TxtNewAddressCountry").val());
                                $("#TxtDireccion").val($("#TxtNewAddress").val());
                                $("#TxtPostalCode").val($("#TxtNewAddressPostalCode").val());
                                $("#TxtCity").val($("#TxtNewAddressCity").val());
                                $("#TxtProvince").val($("#TxtNewAddressProvince").val());
                                $("#TxtCountry").val(country);
                                $("#TxtPhone").val($("#TxtNewAddressPhone").val());
                                $("#TxtMobile").val($("#TxtNewAddressMobile").val());
                                $("#TxtFax").val($("#TxtNewAddressFax").val());
                                $("#TxtEmail").val($("#TxtNewAddressEmail").val());
                            }

                            break;
                        }
                    }

                    ShowAddressPopup();
                    FillCmbAddresses();
                }
                if (response.d.Success !== true) {
                    alertUI(response.d.MessageError);
                }
            },
            "error": function (jqXHR) {
                LoadingHide();
                alert(jqXHR.responseText);
            }
        });
    }
}

function SaveCompany() {
    // Validar formulario
    if (ValidateForm() === false) {
        window.scrollTo(0, 0);
        return false;
    }

    //var language = Company.Language === "es" ? 1 : 2;

    var languageSelected = $("#CmbIdioma").val();

    if (typeof languageSelected === "undefined" || languageSelected === null || languageSelected === "") {
        languageSelected = Company.Language;
    }

    if (languageSelected === "1") { language = 1; }
    else if (languageSelected === "2") { language = 2; }
    
    var data = {
        "oldCompany":
        {
            "Id": Company.Id,
            "Name": Company.Name,
            "FiscalNumber": Company.Nif,
            "DefaultAddress": Company.DefaultAddress,
            "Language": Company.Language
        },
        "newCompany":
        {
            "Id": Company.Id,
            "Name": $("#TxtName").val(),
            "FiscalNumber": $("#TxtNif").val(),
            "DefaultAddress": { "Id": addressSelected },
            "Language": languageSelected
        },
        "userId": user.Id
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/CompanyActions.asmx/Save",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                document.location = "DashBoard.aspx";
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR) {
            alert(jqXHR.responseText);
        }
    });
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $("#blah").attr("src", e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

FillCmbAddresses();
RenderCountries();
if (ApplicationUser.Grants.CompanyProfile.Write === false) {
    $("#BtnShowAddress").hide();
    $("#BtnSave").hide();
    document.getElementById("TxtName").disabled = true;
    document.getElementById("TxtNif").disabled = true;
    document.getElementById("CmbAddress").disabled = true;
    document.getElementById("CmbAddress").style.backgroundColor = "#f5f5f5";
}
else {
    document.getElementById("TxtName").focus();
}