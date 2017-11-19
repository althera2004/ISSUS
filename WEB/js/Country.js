
function filter() {
    var pattern = document.getElementById('TxtCountryName').value.toLowerCase();
    for (var x = 0; x < document.getElementById('AvailablesDiv').childNodes.length; x++) {
        var tag = document.getElementById('AvailablesDiv').childNodes[x];
        if (tag.tagName === 'DIV') {
            if (tag.childNodes[2].innerHTML.toLowerCase().indexOf(pattern) != -1) {
                tag.style.display = '';
            }
            else {
                tag.style.display = 'none';
            }
        }
    }
}

function SelectCountry(sender) {
    if (sender.checked === true) {
        document.getElementById('SelectedCountries').value += sender.id + '|';
    }
    else {
        document.getElementById('SelectedCountries').value = document.getElementById('SelectedCountries').value.split(sender.id + '|').join('');
    }
}

function SelectCountryDelete(sender) {
    if (sender.checked === true) {
        document.getElementById('SelectedCountriesDelete').value += sender.id + '|';
    }
    else {
        document.getElementById('SelectedCountriesDelete').value = document.getElementById('SelectedCountriesDelete').value.split(sender.id + '|').join('');
    }
}

function SelectCountryNoDelete(sender) {
    warningInfoUI(Dictionary.Item_Country_Message_NoDelete, null, 300);
    sender.checked = false;
}

function DiscardCountries() {
    var webMethod = "/Async/CompanyActions.asmx/DiscardCountries";
    var data = {
        'countries': document.getElementById('SelectedCountriesDelete').value,
        'companyId': Company.Id
    }

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
            else {

                var ids = document.getElementById('SelectedCountriesDelete').value.split('|');
                document.getElementById('SelectedCountriesDelete').value = '';
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
    var webMethod = "/Async/CompanyActions.asmx/SelectCountries";
    var data = {
        'countries': document.getElementById('SelectedCountries').value,
        'companyId': Company.Id
    }

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
            else {
                var ids = document.getElementById('SelectedCountries').value.split('|');
                document.getElementById('SelectedCountries').value = '';
                for (var x = 0; x < ids.length; x++) {
                    CountrySetSelected(ids[x], true);
                }
                RenderCountries();
                document.getElementById('TxtCountryName').value = '';
                alertInfoUI(Dictionary.Item_Country_Message_SaveWarning);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}
/*
function RenderCountriesAdded() {
    var Avialables = document.getElementById('AvailablesDiv');
    var Selected = document.getElementById('SelectedDiv');
    var ids = document.getElementById('SelectedCountries').value.split('|');
    document.getElementById('SelectedCountries').value = '';
    for (var x = 0; x < ids.length; x++) {
        if (ids[x] != '') {
            var tag = document.getElementById('CT' + ids[x]);
            tag.checked = false;
            Selected.appendChild(tag);
        }
    }
}

function RenderCountriesDiscarted() {
    var Avialables = document.getElementById('AvailablesDiv');
    var Selected = document.getElementById('SelectedDiv');
    var ids = document.getElementById('SelectedCountriesDelete').value.split('|');
    document.getElementById('SelectedCountriesDelete').value = '';
    for (var x = 0; x < ids.length; x++) {
        if (ids[x] != '') {
            var tag = document.getElementById('CT' + ids[x]);
            tag.checked = false;
            Avialables.appendChild(tag);
        }
    }
}
*/
function RenderCountries() {
    /*document.getElementById('SelectedDiv').innerHTML = '';
    document.getElementById('AvailablesDiv').innerHTML = '';
    for (var x = 0; x < countries.length; x++) {
        RenderCountryTag(countries[x]);
    }*/
}

function RenderCountryTag(country)
{
    var div = document.createElement('DIV');
    div.className = 'col-sm-3';
    div.id = 'CT' + country.Id;
    var check = document.createElement('INPUT');
    check.type = 'checkbox';
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

    var img = document.createElement('IMG');
    img.style.marginLeft = '4px';
    img.src = 'assets/flags/' + country.Id + '.png';

    var span = document.createElement('SPAN');
    span.style.marginLeft = '4px';
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