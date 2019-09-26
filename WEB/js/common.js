var Color = {
    "Error": "#f00",
    "Label": "#333"
};

/// Redondea un valor decimal a dos decimales
function decimalFormat(value) {
    return parseFloat(Math.round(value * 100) / 100).toFixed(2);
}

/// Sólo admite cifras "." y "," al pulsar una tecla
function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    if (key === ".") { key = ","; }
    var regex = /[0-9]|\,/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

/// Sólo admite cifras al pulsar una tecla
function validateInteger(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

/// Sólo admite cifras al pulsar una tecla
function validateIntegerFormated(event) {
    var value = StringToNumber(event.currentTarget.value);
    var regex = /[0-9]/;
    if (!regex.test(value)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

/// Sólo admite cifras y "." al pulsar una tecla
function validateDecimal(evt, value) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[\-\+]?\s*[0-9]{1,3}(\.[0-9]{3})*,[0-9]+/;
    if (!regex.test(value) ) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

/// Valida que texto es una fecha
function validateDate(dateString) {
    // First check for the pattern
    if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
    {
        if (!/^\d{1,2}\/\d{1,2}\/\d{2}$/.test(dateString))
        {
            return false;
        }
    }

    // Parse the date parts to integers
    var parts = dateString.split("/");
    var day = parseInt(parts[0], 10) * 1;
    var month = parseInt(parts[1], 10) * 1;
    var year = parseInt(parts[2], 10) * 1;
    if (year < 1000) {
        year += 2000;
    }

    // Check the ranges of month and year
    if (year < 1000 || year > 3000 || month === 0 || month > 12)
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Adjust for leap years
    if (year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0))
        monthLength[1] = 29;

    // Check the range of the day
    return day > 0 && day <= monthLength[month - 1];
}

/// Vacía el contenido de una table a través del id
function VoidTable(name) {
    var target = document.getElementById(name);
    if (target !== null) {
        while (target.childNodes.length > 0) {
            target.removeChild(target.lastChild);
        }
    }
}

/// Rellena los datos de la ficha de la empresa (traspasar a la ficha de empresa)
function FillEmpresaForm() {
    $("#TxtMailContact").val(CompanyData.MailContact);
    $("#TxtWeb").val(CompanyData.Web);
    $("#SpanSubscriptionStart").html(CompanyData.SubscriptionStart);
    $("#SpanSubscriptionEnd").html(CompanyData.SubscriptionEnd);
}

/// Convierte una fecha en texto con formato dd-mm-yyyy
function FormatDate(date, separator) {
    if (date === null) { return ""; }
    if (separator === null) { separator = "-"; }
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var res = "";
    if (day < 10) { res += "0"; }
    res += day + separator;
    if (month < 10) { res += "0"; }
    return res += month + separator + year;
}

/// Convierte un texto de fecha YYYYMMDD en texto con formato dd-mm-yyyy
/// Necesario cuando la fecha debe ser ordenable
function FormatYYYYMMDD(date, separator) {
    if (date === null) { return ""; }

    if (typeof (date) === "object") {
        return FormatDate(date, separator);
    }

    if (separator === null) { separator = "-"; }
    date = date.toString();
    var year = date.substring(0, 4) * 1;
    var month = date.substring(4, 6) * 1;
    var day = date.substring(6, 8) * 1;
    var res = "";
    if (day < 10) { res += "0"; }
    res += day + separator;
    if (month < 10) { res += "0"; }
    return res += month + separator + year;
}

/// Convierte una fecha en texto con formato YYYYMMDD
function GetDateYYYYMMDDToText(date, nullable) {

    if (date === null || date === "") {
        if (nullable === null) {
            return null;
        }
        else {
            if (nullable === false) {
                return null;
            }
            else {
                return "";
            }
        }
    }

    var day = date.getDate().toString();
    if (day.length < 2) { day = "0" + day; }
    var month = (date.getMonth() + 1).toString();
    if (month.length < 2) { month = "0" + month; }
    var year = date.getFullYear().toString();
    return year +  month +  day;
}

/// Convierte un texto de fecha YYYYMMDD en texto con formato dd-mm-yyyy indicando separador
function GetDateYYYYMMDDText(date, separator, nullable) {

    if (date === null || date === "") {
        if (nullable === null) {
            return null;
        }
        else {
            if (nullable === false) {
                return null;
            }
            else {
                return new Date(1970, 1, 1);
            }
        }
    }

    if (separator === null) { separator = "-"; }
    date = date.toString();
    var day = date.substr(6, 2);
    var month = date.substr(4, 2);
    var year = date.substr(0, 4);
    return day + separator + month + separator + year;
}

/// Obtiene una fecha a partir de un texto con formato YYYYMMDD
function GetDateYYYYMMDD(date, nullable) {
    if (date === "") {
        if (typeof nullable === "undefined" || nullable === null) {
            return null;
        }
        else {
            if (nullable === false) {
                return null;
            }
            else {
                return new Date(1970, 1, 1);
            }
        }
    }

    date = date.toString();
    var day = date.substr(6, 2) * 1;
    var month = date.substr(4, 2) * 1 - 1;
    var year = date.substr(0, 4) * 1;
    return new Date(year, month, day);
}

/// Obtiene una fecha a partir de un texto con formato dd-mm-yyyy
function GetDate(date, separator, nullable) {
    if (date === "") {
        if (typeof nullable === "undefined" || nullable === null) {
            return null;
        }
        else {
            if (nullable === false) {
                return null;
            }
            else {
                return new Date(1970, 1, 1);
            }
        }
    }

    if (separator === "/") { separator = "-"; }
    if (separator === null) { separator = "-"; }
    date = date.split("/").join("-");
    var day = date.split(separator)[0] * 1;
    var month = (date.split(separator)[1] * 1) -1;
    var year = date.split(separator)[2] * 1;
    if (year < 100) {
        year += 2000;
    }
    return new Date(year, month, day);
}

/// Convierte un texto en número indicando separador de millares y decimal
function StringToNumber(text, milesSeparator, decimalSeparator) {
    text = text.toString();
    milesSeparator = milesSeparator || Dictionary.NumericMilesSeparator;
    decimalSeparator = decimalSeparator || Dictionary.NumericDecimalSeparator;
    text = text.split(milesSeparator).join('');
    text = text.split(decimalSeparator).join('.');
    return text * 1;
}

/// Convierte un texto en número indicando separador de millares y decimal admitiendo valores nulos
function StringToNumberNullable(text, milesSeparator, decimalSeparator) {
    if (text === "") {
        return null;
    }

    text = text.toString();
    milesSeparator = milesSeparator || Dictionary.NumericMilesSeparator;
    decimalSeparator = decimalSeparator || Dictionary.NumericDecimalSeparator;
    text = text.split(milesSeparator).join('');
    text = text.split(decimalSeparator).join('.');
    return text * 1;
}

/// Conviert un número en texto
function NumberToString(number) {
    var integer = Math.floor(number);
    var decimal = number - integer;
    return (integer + '.' + decimal.toFixed(2).toString().split('.')[1]) * 1;
}

//----------------------------------------------------------------------
/// Ordena un array de objetos indicando el campo y el orden ASC/DESC
var sort_by = function (field, reverse, primer) {
    var key = primer ? function (x) { return primer(x[field]); } : function (x) { return x[field]; };
    reverse = [-1, 1][+!!reverse];
    return function (a, b) {
        return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
    };
};

/// Ordena el contenido de una lista
function Sort(sender, targetName, dataType, HasTotalFooter) {
    var sortType = sender.className.indexOf("ASC") !== -1 ? "DESC" : "ASC";
    listOrder = sender.id + "|" + sortType;

    // Eliminar el sortype de los th
    for (var x = 0; x < sender.parentNode.childNodes.length; x++) {
        if (typeof (sender.parentNode.childNodes[x].className) !== "undefined") {
            sender.parentNode.childNodes[x].className = sender.parentNode.childNodes[x].className.split('ASC').join('').split('DESC').join('');
        }
    }

    var target = document.getElementById(targetName);
    var top = target.childNodes.length;
    var footer = null;
    if (typeof HasTotalFooter !== "undefined" && HasTotalFooter === true) {
        top--;
    }

    if (top < 2) { return;}

    var id = sender.id.split("th").join("") * 1;
    sender.className = sender.className + (sortType === "DESC" ? " DESC" : " ASC");
    var rows = new Array();
    for (var z = 0; z < top; z++) {
        if (target.childNodes[z].tagName === "TR") {
            rows.push(target.childNodes[z]);
        }
    }

    if (typeof HasTotalFooter !== "undefined" && HasTotalFooter === true)
    {
        footer = target.lastChild;
    }

    while (target.childNodes.length > 0) {
        target.removeChild(target.lastChild);
    }

    var index = new Array();

    for (var y = 0; y < rows.length;  y++) {
        var pivot = rows[y].childNodes[id];
        if (rows[y].childNodes[0].nodeName === "#text") {
            pivot = rows[y].childNodes[id * 2 + 1];
        }
        var value = pivot.innerHTML;
        if (pivot.childNodes.length > 0 && typeof (pivot.childNodes[0].tagName) !== "undefined") {
            value = pivot.childNodes[0].innerHTML;
        }

        if (dataType === "date") {
            value = value.split('/')[2] + value.split('/')[1] + value.split('/')[0];
        }

        if (dataType === "money") {
            value = ParseInputValueToNumber(value);
        }

        index.push({ Key: y, Value: value });
    }

    if (dataType === "money") {
        index.sort(sort_by("Value", sortType === "ASC", function (a) { return a * 1; }));
    }
    else {
        index.sort(sort_by("Value", sortType === "ASC", function (a) { return a.toUpperCase(); }));
    }

    for (var w = 0; w < index.length; w++) {
        target.appendChild(rows[index[w].Key]);
    }

    if (typeof HasTotalFooter !== "undefined" && HasTotalFooter === true) {
        target.appendChild(footer);
    }
}

//--------------------- VALIDACION NIF
//Retorna: 1 = NIF ok, 2 = CIF ok, 3 = NIE ok, -1 = NIF error, -2 = CIF error, -3 = NIE error, 0 = ??? error
function valida_nif_cif_nie( a )
{
    var temp = a.toUpperCase();
    var cadenadni = "TRWAGMYFPDXBNJZSQVHLCKE";
 
    if (temp !== '') {
        //si no tiene un formato valido devuelve error
        if ((!/^[A-Z]{1}[0-9]{7}[A-Z0-9]{1}$/.test(temp) && !/^[T]{1}[A-Z0-9]{8}$/.test(temp)) && !/^[0-9]{8}[A-Z]{1}$/.test(temp)) {
            return 0;
        }

        //comprobacion de NIFs estandar
        if (/^[0-9]{8}[A-Z]{1}$/.test(temp)) {
            posicion = a.substring(8, 0) % 23;
            letra = cadenadni.charAt(posicion);
            var letradni = temp.charAt(8);
            if (letra === letradni) {
                return 1;
            }
            else {
                return -1;
            }
        }

        //algoritmo para comprobacion de codigos tipo CIF
        suma = parseInt(a.charAt(2)) + parseInt(a.charAt(4)) + parseInt(a.charAt(6));

        for (i = 1; i < 8; i += 2) {
            temp1 = 2 * parseInt(a.charAt(i));
            temp1 += "";
            temp1 = temp1.substring(0, 1);
            temp2 = 2 * parseInt(a.charAt(i));
            temp2 += "";
            temp2 = temp2.substring(1, 2);
            if (temp2 === "") {
                temp2 = "0";
            }

            suma += (parseInt(temp1) + parseInt(temp2));
        }

        suma += "";
        n = 10 + (parseInt(suma.substring(suma.length - 1, suma.length)) * -1);

        //comprobacion de NIFs especiales (se calculan como CIFs)
        if (/^[KLM]{1}/.test(temp)) {
            if (a.charAt(8) === String.fromCharCode(64 + n)) {
                return 1;
            }
            else {
                return -1;
            }
        }

        //comprobacion de CIFs
        if (/^[ABCDEFGHJNPQRSUVW]{1}/.test(temp)) {
            temp = n + "";
            if (a.charAt(8) === String.fromCharCode(64 + n) || a.charAt(8) === parseInt(temp.substring(temp.length - 1, temp.length))) {
                return 2;
            }
            else {
                return -2;
            }
        }

        //comprobacion de NIEs
        //T
        if (/^[T]{1}[A-Z0-9]{8}$/.test(temp)) {
            if (a.charAt(8) === /^[T]{1}[A-Z0-9]{8}$/.test(temp)) {
                return 3;
            }
            else {
                return -3;
            }
        }

        //XYZ
        if (/^[XYZ]{1}/.test(temp)) {
            temp = temp.replace("X", "0");
            temp = temp.replace("Y", "1");
            temp = temp.replace("Z", "2");
            pos = temp.substring(0, 8) % 23;

            if (a.toUpperCase().charAt(8) === cadenadni.substring(pos, pos + 1)) {
                return 3;
            }
            else {
                return -3;
            }
        }
    }
 
    return 0;
}

/// Indica si el texto tiene el formato de email
function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

// VALIDACION DE FORMULARIOS
// --------------------------------------------------------------

/// Resetea los errores de validación de un campo
function ClearFieldTextMessages(fieldName) {
    $("#" + fieldName + "ErrorRequired").hide();
    $("#" + fieldName + "ErrorDuplicated").hide();
    $("#" + fieldName + "Label").css("color", "#000");
}

/// Pinta de rojo la etiqueta de un campo
function SetFieldTextMessages(fieldName) {
    $("#" + fieldName + "Label").css("color", Color.Error);
}

/// Establece un campo como obligatio, añade * en rojo a la etiqueta
function SetAsRequired(fieldName) {
    $("#" + fieldName + "Label").html($("#" + fieldName + fieldName + "Label").html() + "<span style=\"color:#f00;\">*</span>");
}

/// Comprueba si el valor de un campo está duplicado
function DuplicatedFiled(field) {
    ok = true;
    var fieldName = field.FieldName;
    var item = document.getElementById(field.FieldName).value;
    for (var x = 0; x < field.Values.length; x++) {
        if (field.Values[x].Value === item && field.Values[x].Id !== field.Id) {
            ok = false;
            break;
        }
    }

    if (ok === false) {
        $("#" + fieldName + "ErrorDuplicated").show();
        $("#" + fieldName + "Label").css("color", Color.Error);
    }
    else {
        $("#" + fieldName + "ErrorDuplicated").hide();
        $("#" + fieldName + "Label").css("color", "#000");
    }

    return ok;
}

/// Comprueba si un campo tiene valor
function RequiredFieldText(fieldName) {
    ok = true;
    if ($("#" + fieldName).val().trim() === "") {
        ok = false;
        $("#" + fieldName + "ErrorRequired").show();
        $("#" + fieldName + "Label").css("color", Color.Error);
    }
    else {
        $("#" + fieldName + "ErrorRequired").hide();
        $("#" + fieldName + "Label").css("color", "#000");
    }

    return ok;
}

/// Comprueba si un select tiene valor seleccionado
function RequiredFieldCombo(fieldName) {
    ok = true;
    if ($("#"+fieldName).val() === "0") {
        ok = false;
        $("#" + fieldName + "ErrorRequired").show();
        $("#" + fieldName + "Label").css("color", Color.Error);
    }
    else {
        $("#" + fieldName + "ErrorRequired").hide();
        $("#" + fieldName + "Label").css("color", "#000");
    }

    return ok;
}

/// Comprueba que almenos uno de los dos campos tiene valir
function RequiredBothFieldText(fieldName1, fieldName2) {
    ok = true;
    if (document.getElementById(fieldName1).value.trim() === "" && document.getElementById(fieldName2).value.trim() === "") {
        ok = false;
        $("#" + fieldName1 + "ErrorRequired").show();
        $("#" + fieldName1 + "Label").css("color", Color.Error);
        $("#" + fieldName2 + "ErrorRequired").show();
        $("#" + fieldName2 + "Label").style.color = "#f00";
    }
    else {
        $("#" + fieldName1 + "ErrorRequired").hide();
        $("#" + fieldName1 + "Label").css("color", "#000");
        $("#" + fieldName2 + "ErrorRequired").hide();
        $("#" + fieldName2 + "Label").css("color", "#000");
    }

    return ok;
}

/// Comprueba que los dos campos tienen el mismo valor. P.e.:"Contreña" y "Validar contraseña"
function MatchRequiredBothFieldText(fieldName1, fieldName2) {
    ok = true;
    if ($("#" + fieldName1).val() === "" || $("#" + fieldName2).val() === "") {
        return ok;
    }

    if (document.getElementById(fieldName1).value !== document.getElementById(fieldName2).value) {
        ok = false;
        $("#" + fieldName1 + "ErrorMatch").show();
        $("#" + fieldName1 + "Label").css("color", Color.Error);
        $("#" + fieldName2 + "ErrorMatch").show();
        $("#" + fieldName2 + "Label").css("color", Color.Error);
    }
    else {
        $("#" + fieldName1 + "ErrorMatch").hide();
        $("#" + fieldName1 + "Label").css("color", "#000");
        $("#" + fieldName2 + "ErrorMatch").hide();
        $("#" + fieldName2 + "Label").css("color", "#000");
    }

    return ok;
}

/// Comprueba si un texto tiene el formato válidfo para NIF/NIE/CIF
function MalFormedNif(fieldName) {
    ok = true;
    if (valida_nif_cif_nie(document.getElementById(fieldName).value) < 1) {
        ok = false;
        $("#" + fieldName + "ErrorMalformed").show();
        $("#" + fieldName + "Label").css("color", Color.Error);
    }
    else {
        $("#" + fieldName + "ErrorMalformed").hide();
        $("#" + fieldName + "Label").css("color", "#000");
        $("#" + fieldName).val($("#" + fieldName).val().toUpperCase());
    }

    return ok;
}

/// Comprueba si un texto tiene el formato válido para un email
function MalFormedEmail(fieldName) {
    ok = true;
    if (validateEmail(document.getElementById(fieldName).value) < 1) {
        ok = false;
        $("#" + fieldName + "ErrorMalformed").show();
        $("#" + fieldName + "Label").css("color", Color.Error);
    }
    else {
        $("#" + fieldName + "ErrorMalformed").hide();
        $("#" + fieldName + "Label").css("Color", "#000");
        $("#" + fieldName).val($("#" + fieldName).val().toLowerCase());
    }

    return ok;
}

/// Comprueba si se alcanza el mínimo de checkbox marcados
function RequiredMinimumCheckBox(criteria) {
    var res = true;
    var count = 0;
    for (var x = 0; x < criteria.Options.length; x++) {
        if (document.getElementById(criteria.Options[x]).checked === true) { count++; }
    }

    if (count < criteria.Minimum) {
        ok = false;
        $("#" + criteria.Message).show();
    }
    else {
        ok = true;
        $("#" + criteria.Message).hide();
    }

    return ok;
}

/// Comprueba que el valor sea una fecha
function RequiredDateValue(fieldName) {
    if (document.getElementById(fieldName) !== null) {
        var dateText = $("#" + fieldName).val();
        if (!validateDate(dateText)) {
                $("#" + fieldName + "Label").css("color", Color.Error);
            $("#" + fieldName + "DateMalformed").show();
            return false;
        }
    }

    return true;
}

/// Obtiene un empleado a través del id
function GetCompanyEmployee(employeeId) {
    for (var x = 0; x < Company.Employees.length; x++) {
        if (Company.Employees[x].Id === employeeId) {
            return Company.Employees[x];
        }
    }

    return null;
}

/// Obtiene un departamento a través del id
function GetCompanyDepartment(departmentId) {
    for (var x = 0; x < departmentsCompany.length; x++) {
        if (departmentsCompany[x].Id === departmentId) {
            return departmentsCompany[x];
        }
    }

    return null;
}

/// Indica si un departamento tiene cargo asignado para evitar ser eliminado
function DepartmentHasJobPosition(departmentId) {
    for (var x = 0; x < jobPositionCompany.length; x++) {
        if (jobPositionCompany[x].Department.Id === departmentId) {
            return true;
        }
    }

    return false;
}

/// Establece el tooltipo para un campo de formulario
function SetToolTip(id, text) {
    if(document.getElementById(id) !== null) {
        var control = document.getElementById(id);
        control.title = text;
        control.setAttribute("data-rel", "tooltip");
        if (control.tagName === "DIV") {
            control.setAttribute("data-placement", "top");
            return;
        }

        control.setAttribute("data-placement", "right");
    }
}

/// Deshabilita el tooltipo para un campo de formulario
function UnsetToolTip(id) {
    if (document.getElementById(id) !== null) {
        var control = document.getElementById(id);
        control.removeAttribute("data-rel", "tooltip");
    }
}

/// Genera un aviso de obligatoriedad
function CreateRequiredAlert() {
    var requiredAlert = document.createElement("SPAN");
    requiredAlert.style.color = "#f00";
    requiredAlert.appendChild(document.createTextNode("*"));
    return requiredAlert;
}

/// Establece un campo campo obligatorio
function FieldSetRequired(fieldName, label, required) {
    try{
        target = document.getElementById(fieldName);
        target.innerHTML = "";
        target.appendChild(document.createTextNode(label));
        if (required === true) {
            target.appendChild(CreateRequiredAlert());
        }
        else {
            target.style.color = "";
        }
    }
    catch (e) {
        console.log("FieldSetRequired", e);
    }
}

/// Convierto el valor de un campo en formato "money" #.###,##
function ToMoneyFormatValue(sender, decimals) {
    var value = sender.value;
    if (value === "") { return; }
    sender.value = ToMoneyFormat(value, decimals);
}

/// Convierto un texto en formato "money" #.###,##
function ToMoneyFormat(value, decimals, nullable) {
    if (nullable === true && value === null) {
        return "";
    }

    var pow = 100;
    if (typeof decimals !== "undefined") {
        pow = Math.pow(10, decimals);
    }
    else {
        decimals = 2;
    }

    value = parseFloat(Math.round(value * pow) / pow).toFixed(decimals);
    var res = value;
    var entera = "";
    var enteraRes = "";
    var decimal = "";

    entera = value.split(".")[0];
    decimal = value.split(".")[1];
    if(typeof decimal === "undefined")
    {
        decimal = "0";
    }

    while(decimal.length < decimals)
    {
        decimal += "0";
    }

    var negative = "";
    if (entera.length > 0 && entera.substr(0,1) === "-") {
        negative = "-";
        entera = entera.substr(1);
    }

    while (entera.length > 0)
    {
        if(entera.length < 4)
        {
            enteraRes = entera + "." + enteraRes;
            entera = "";
        }
        else
        {
            enteraRes = entera.substr(entera.length - 3, 3) + Dictionary.NumericMilesSeparator + enteraRes;
            entera = entera.substr(0, entera.length - 3);
        }        
    }

    enteraRes = negative + enteraRes;

    if (decimals === 0)
    {
        return enteraRes.substr(0, enteraRes.length - 1);
    }

    return enteraRes.substr(0, enteraRes.length - 1) + Dictionary.NumericDecimalSeparator + decimal;
}

// Extend the default Number object with a formatMoney() method:
// usage: someVar.formatMoney(decimalPlaces, symbol, thousandsSeparator, decimalSeparator)
// defaults: (2, "$", ",", ".")
Number.prototype.formatMoney = function (places, symbol, thousand, decimal) {
    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "$";
    //thousand = thousand || ",";
    decimal = decimal || ".";
    var number = this,
        negative = number < 0 ? "-" : "",
        i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    var res = symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
    console.log(res);
    return res;
};

/// Prepara un valor para ser tratado como número
function PrepareNumberForInput(value)
{
    return value.toString().split('.').join(Dictionary.NumericDecimalSeparator);
}

/// Prepara el valor de un campo para ser tratado como número
function PrepareInputNumber(sender)
{
    if (sender.value === "") return;
    var value = sender.value;
    if (Dictionary.NumericMilesSeparator === ".")
    {
        value = value.split(Dictionary.NumericMilesSeparator).join("");
    }

    sender.value = value * 1;
}

/// Noop
function ValidateMoneyValue(sender)
{
    return false;
}

// --------- Numeric transform
/// Convierte un texto en valor Integer
function ParseNumberToInputValue(value)
{
    return value.toString().split(".").join(Dictionary.NumericDecimalSeparator);
}

/// Convierte un texto en valor Integer, permite valores nulos
function ParseInputValueToNumber(value, nullable) {
    if (typeof value === "undefined") { return null; }

    if (nullable === true && value === "") {
        return "";
    }

    value = value.split(".").join("");
    value = value.split(",").join(".");
    return value * 1;
}

/// Convierte un texto en valor Money
function ParseInputValueToMoney(value) {
    value = parseFloat(Math.round(value * 100) / 100).toFixed(2);
    return ParseNumberToInputValue(value);
}

/// Convierte un valor Decimal en Integer
function Decimal2Integer(value) {
    return Math.floor(value);
}

var cmbEmployee = null;
/// Indica si un empleado seleccionado no tiene usuario asociado
function WarningEmployeeNoUserCheck(employeeId, employeesList, cmbSender) {
    if (employeeId < 1) {
        return false;
    }
    cmbEmployee = cmbSender;
    console.log("WarningEmployeeNoUserCheck");
    for (var x = 0; x < employeesList.length; x++) {
        if (employeesList[x].Id === employeeId) {
            if (employeesList[x].HasUserAssigned === false) {
                WarningEmployeeNoUser();
                return;
            }
        }
    }

    if (typeof EmployeesGrant !== "undefined" && EmployeesGrant !== null) {
        var found = false;
        for (var y = 0; y < EmployeesGrant.length; y++) {
            if (EmployeesGrant[y] === employeeId) {
                found = true;
                break;
            }
        }

        if (found === false) {
            if (ApplicationUser.Admin === false) {
                alertUI("El empleado no tiene permisos para esta acción. Consulte con el administrador");
            }
            else {

                alertInfoNoGrantsUI("El empleado no tiene permisos para esta acción. Consulte con el administrador");
            }
        }
    }
}

/// Lanza un popup avisando que el empleado seleccionado no tiene usuario asociado
function WarningEmployeeNoUser() {
    alertUI(Dictionary.Item_Employee_Warning_NoUser);
}

/// Establece el formato de fecha dd/mm/yyyy cuando cambia el valor de un campo fecha
function DatePickerChanged(sender) {
    var value = sender.value;
    if (validateDate(value, "/", false) === false) {
        sender.value = "";
        return false;
    }

    var date = GetDate(sender.value, "/", false);
    value = FormatDate(date, "/");
    sender.value = value;
}

/// Establece los permisos de un usuario
function EmployeeSetGrant(employeeId, itemId) {
    //public ActionResult SetGrant(long employeeId, int companyId, int itemId, int applicationUserId)
    var data = {
        "employeeId": employeeId,
        "itemId": itemId,
        "companyId": Company.Id,
        "applicationUserId": user.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/EmployeeActions.asmx/SetGrant",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

/// Comvierte un campo editable en etiqueta
function InputToLabel(imputName) {
    $("#" + imputName).parent().html("<strong>" + $("#" + imputName).val() + "</strong>");
}

/// Crear un texto ellipsys fijado en un límite de 50 caracteres
function Ellipsys(text, length) {
    var res = text;
    res = text > 50 ? (text.substr(0, 49) + "...") : text;
    return res;
}

function MinutesToHour(minutes) {
    var h = 0;
    while (minutes > 59) {
        minutes -= 60;
        h++;
    }

    res = h + ":";
    if (minutes < 10) { res += "0"; }
    return res += minutes;
}

function HourToMinutes(minutes) {
    var parts = minutes.split(':');
    return parts[0] * 60 + parts[1] * 1;
}