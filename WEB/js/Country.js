
function filter() {
    var pattern = document.getElementById("TxtCountryName").value.toLowerCase();
    for (var x = 0; x < document.getElementById("AvailablesDiv").childNodes.length; x++) {
        var tag = document.getElementById("AvailablesDiv").childNodes[x];
        if (tag.tagName === "DIV") {
            if (tag.childNodes[2].innerHTML.toLowerCase().indexOf(pattern) !== -1) {
                tag.style.display = "";
            }
            else {
                tag.style.display = "none";
            }
        }
    }
}

function SelectCountry(sender) {
    if (sender.checked === true) {
        document.getElementById("SelectedCountries").value += sender.id + "|";
    }
    else {
        document.getElementById("SelectedCountries").value = document.getElementById("SelectedCountries").value.split(sender.id + "|").join("");
    }
}

function SelectCountryDelete(sender) {
    if (sender.checked === true) {
        document.getElementById("SelectedCountriesDelete").value += sender.id + "|";
    }
    else {
        document.getElementById("SelectedCountriesDelete").value = document.getElementById("SelectedCountriesDelete").value.split(sender.id + "|").join("");
    }
}

function SelectCountryNoDelete(sender) {
    warningInfoUI(Dictionary.Item_Country_Message_NoDelete, null, 300);
    sender.checked = false;
}

function DiscardCountries() {
    var data = {
        "countries": document.getElementById("SelectedCountriesDelete").value,
        "companyId": Company.Id
    }

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/CompanyActions.asmx/DiscardCountries",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {

                var ids = document.getElementById("SelectedCountriesDelete").value.split("|");
                $("#SelectedCountriesDelete").val("");
                for (var x = 0; x < ids.length; x++) {
                    CountrySetSelected(ids[x], false);
                }
                RenderCountries();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function SaveCountries() {
    var data = {
        "countries": document.getElementById("SelectedCountries").value,
        "companyId": Company.Id
    }

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/CompanyActions.asmx/SelectCountries",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            LoadingHide();
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
            else {
                var ids = $("#SelectedCountries").val().split("|");
                $("#SelectedCountries").val("");
                for (var x = 0; x < ids.length; x++) {
                    CountrySetSelected(ids[x], true);
                }

                RenderCountries();
                $("#TxtCountryName").val("");
                alertInfoUI(Dictionary.Item_Country_Message_SaveWarning);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function RenderCountries() {
    /*document.getElementById('SelectedDiv').innerHTML = '';
    document.getElementById('AvailablesDiv').innerHTML = '';
    for (var x = 0; x < countries.length; x++) {
        RenderCountryTag(countries[x]);
    }*/
}

function RenderCountryTag(country)
{
    var div = document.createElement("DIV");
    div.className = "col-sm-3";
    div.id = "CT" + country.Id;
    var check = document.createElement("INPUT");
    check.type = "checkbox";
    check.id = country.Id;

    if (country.Deletable !== true)
    {
        check.onclick = function () { SelectCountryNoDelete(this); };
        //check.disabled = true;
    }
    else
    {
        if(country.Selected === true)
        {
            check.onclick = function () { SelectCountryDelete(this); };
        }
        else
        {
            check.onclick = function () { SelectCountry(this); };
        }
    }

    var img = document.createElement("IMG");
    img.style.marginLeft = "4px";
    img.src = "assets/flags/" + country.Id + ".png";

    var span = document.createElement("SPAN");
    span.style.marginLeft = "4px";
    span.appendChild(document.createTextNode(country.Description));

    div.appendChild(check);
    div.appendChild(img);
    div.appendChild(span);

    if(country.Selected===true)
    {
        document.getElementById('SelectedDiv').appendChild(div);
    }
    else
    {
        document.getElementById('AvailablesDiv').appendChild(div);
    }
}

function CountrySetSelected(id, selected)
{
    id = id * 1;
    for (var x = 0; x < countries.length; x++) {
        if(countries[x].Id===id)
        {
            countries[x].Selected = selected;
            break;
        }
    }
}