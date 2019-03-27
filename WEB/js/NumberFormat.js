var numberActual;

function AcceptKey(code, type) {
    var ok = false;
    if (code === 8) { ok = true; }
    if (code === 9) { ok = true; }
    if (code === 13) { ok = true; }
    if (code === 46) { ok = true; }
    if (code === 37) { ok = true; }
    if (code === 39) { ok = true; }
    if (code === 27) { ok = true; }
    if (code === 109) { ok = true; }
    if (code > 95 && code < 106) { ok = true; }
    if (code > 36 && code < 41) { ok = true; }
    if (code > 47 && code < 58) { ok = true; }

    if (type === "decimal" || type === "money") {
        if (code === 188 || code === 110 || code === 190) {
            ok = true;
        }
    }

    return ok;
}

function numberDecimalDown(e) {
    var code = (e.keyCode ? e.keyCode : e.which);

    if (AcceptKey(code, "decimal") === false) {
        e.preventDefault();
        e.stopPropagation();
    }

    numberActual = e.currentTarget.value;
    if (code === 188 || code === 110 || code === 190) {
        e.currentTarget.value = numberActual.split(",").join(".");
        if (numberActual.indexOf(".") != -1) {
            e.preventDefault();
            e.stopPropagation();
        }
    }
}

function numberIntegerDown(e) {
    var code = (e.keyCode ? e.keyCode : e.which);
    numberActual = e.currentTarget.value;

    if (AcceptKey(code, "integer") === false) {
        e.preventDefault();
        e.stopPropagation();
    }
}

function numberDecimalUp(e) {
    console.log("this", $(this));
    var code = (e.keyCode ? e.keyCode : e.which);
    numberActual = e.currentTarget.value;

    if (numberActual.indexOf(",") != -1) {
        e.currentTarget.value = numberActual.split(",").join(".");
    }

    if (numberActual.split(".").length > 2) {
        console.log("doble-up");
        e.currentTarget.value = numberActual;
        e.preventDefault();
        e.stopPropagation();
    }
}

function numberDecimalFocus(e) {
    keyFocus = e.currentTarget.value;
    console.log("numberDecimalFocus", e.currentTarget.value * 1);
    e.currentTarget.value = e.currentTarget.value.toString().split(".").join("").split(",").join(".");
    console.log("numberDecimalFocus", e.currentTarget.value * 1);
    if (e.currentTarget.value * 1 === 0) {
        e.currentTarget.value = "";
    }

    e.currentTarget.value = e.currentTarget.value.toString().split(Dictionary.NumericDecimalSeparator).join(".");
}

function numberIntegerFocus(e) {
    if (e.currentTarget.value * 1 === 0) {
        e.currentTarget.value = "";
    }

    if (isNaN(value)) {
        e.currentTarget.value = keyFocus;
        value = keyFocus;
        e.preventDefault();
        e.stopPropagation();
        console.log("Not a number", e.currentTarget.value);
    }

    // @alex: poner el formato en magnitud (sin puntos de millares)
    e.currentTarget.value = e.currentTarget.value.toString().split(".").join("").split(",").join(".");
}

function numberDecimalBlur(e) {
    var value = e.currentTarget.value;

    if (isNaN(value)) {
        e.currentTarget.value = keyFocus;
        value = keyFocus;
        e.preventDefault();
        e.stopPropagation();
        console.log("Not a number", e.currentTarget.value);
    }

    value = Math.round(value * 100) / 100;
    e.currentTarget.value = value.toString().split('.').join(Dictionary.NumericDecimalSeparator);
}

function moneyBlur(e) {
    var value = e.currentTarget.value;

    if (isNaN(value)) {
        e.currentTarget.value = keyFocus;
        value = keyFocus;
        e.preventDefault();
        e.stopPropagation();
        console.log("Not a number", e.currentTarget.value);
    }

    if (e.currentTarget.className.indexOf("nullable") !== -1) {
        if (value === "") {
            console.log("moneyBlur", "null value");
            return false;
        }
    }

    if ($.isNumeric(value) === false) {
        value = value = StringToNumberNullable(value, ".", ",");
    }

    if (e.currentTarget.className.indexOf("nullable") != -1 && value === "") {
    }
    else {
        value = Math.round(value * 100) / 100;
        value = ToMoneyFormat(value, 2);
        e.currentTarget.value = value;
    }
}

function numberDecimalBlur6(e) {
    var value = e.currentTarget.value;

    if (isNaN(value)) {
        e.currentTarget.value = keyFocus;
        value = keyFocus;
        e.preventDefault();
        e.stopPropagation();
        console.log("Not a number", e.currentTarget.value);
    }

    if (e.currentTarget.className.indexOf("nullable") != -1 && value === "") {
    }
    else {
        value = Math.round(value * 1000000) / 1000000;
        // @alex: poner puntos de millar
        // e.currentTarget.value = value.toString().split('.').join(Dictionary.NumericDecimalSeparator);
        e.currentTarget.value = ToMoneyFormat(value, 6);
    }
}

function numberDecimalBlur4(e) {
    var value = e.currentTarget.value;
    if (e.currentTarget.className.indexOf("nullable") != -1 && value === "") {
    }
    else {
        value = Math.round(value * 10000) / 10000;
        // @alex: poner puntos de millar
        // e.currentTarget.value = value.toString().split('.').join(Dictionary.NumericDecimalSeparator);
        e.currentTarget.value = ToMoneyFormat(value, 4);
    }
}

function numberDecimalBlur6(e) {
    var value = e.currentTarget.value;
    if (e.currentTarget.className.indexOf("nullable") != -1 && value === "") {
    }
    else {
        value = Math.round(value * 1000000) / 1000000;
        // @alex: poner puntos de millar
        // e.currentTarget.value = value.toString().split('.').join(Dictionary.NumericDecimalSeparator);
        e.currentTarget.value = ToMoneyFormat(value, 6);
    }
}

function numberIntegerBlur(e) {
    e.currentTarget.value = ToMoneyFormat(e.currentTarget.value * 1, 0);
}

$("input.money-bank").on("keyup", numberDecimalUp);
$("input.money-bank").on("keydown", numberDecimalDown);
$("input.money-bank").on("focus", numberDecimalFocus);
$("input.money-bank").on("blur", moneyBlur);

$("input.decimalFormated").on("keyup", numberDecimalUp);
$("input.decimalFormated").on("keydown", numberDecimalDown);
$("input.decimalFormated").on("focus", numberDecimalFocus);
$("input.decimalFormated").on("blur", numberDecimalBlur6);

$("input.decimalFormated4").on("keyup", numberDecimalUp);
$("input.decimalFormated4").on("keydown", numberDecimalDown);
$("input.decimalFormated4").on("focus", numberDecimalFocus);
$("input.decimalFormated4").on("blur", numberDecimalBlur4);

$("input.decimalFormated6").on("keyup", numberDecimalUp);
$("input.decimalFormated6").on("keydown", numberDecimalDown);
$("input.decimalFormated6").on("focus", numberDecimalFocus);
$("input.decimalFormated6").on("blur", numberDecimalBlur6);

//$("input.integerFormated").on("keyup", numberIntegerUp);
$("input.integerFormated").on("keydown", numberIntegerDown);
$("input.integerFormated").on("focus", numberIntegerFocus);
$("input.integerFormated").on("blur", numberIntegerBlur);

// @alex: con estas funciones se pasan las magnitudes de los campos al formato de cada uno
// --------------------------------------------------------------------------------------------
$(".money-bank").each(function () {
    $(this).val(ToMoneyFormat(ParseInputValueToNumber($(this).val()), 2));
});
$(".decimalFormated").each(function () {
    $(this).val(ToMoneyFormat(ParseInputValueToNumber($(this).val()), 2));
});
$(".decimalFormated4").each(function () {
    $(this).val(ToMoneyFormat(ParseInputValueToNumber($(this).val()), 4));
});
$(".decimalFormated6").each(function () {
    $(this).val(ToMoneyFormat(ParseInputValueToNumber($(this).val()), 6));
});
$(".integerFormated").each(function () {
    $(this).val(ToMoneyFormat(ParseInputValueToNumber($(this).val()), 0));
});
// --------------------------------------------------------------------------------------------