var MinStepValue = rule.Id > 0 ? 1 : 0;

function ValidateForm()
{
    var ok = true;
    document.getElementById('TxtNameLabel').style.color = '#000';
    document.getElementById('TxtLimitLabel').style.color = '#000';
    document.getElementById('TxtNameErrorRequired').style.display = 'none';
    document.getElementById('TxtNameErrorDuplicated').style.display = 'none';
    document.getElementById('TxtLimitErrorRequired').style.display = 'none';
    document.getElementById('TxtLimitErrorOutOfRange').style.display = 'none';

    var name = document.getElementById('TxtName').value.trim();
    var limit = document.getElementById('TxtLimit').value.trim();

    if (name === '')
    {
        ok = false;
        document.getElementById('TxtNameLabel').style.color = '#f00';
        document.getElementById('TxtNameErrorRequired').style.display = '';
    }
    else {
        for (var x=0;x<companyRules.length;x++)
        {
            if(companyRules[x].Description === name && companyRules[x].Id !== rule.Id)
            {
                ok = false;
                document.getElementById('TxtNameLabel').style.color = '#f00';
                document.getElementById('TxtNameErrorDuplicated').style.display = '';
                break;
            }
        }
    }

    if (limit === '') {
        ok = false;
        document.getElementById('TxtLimitLabel').style.color = '#f00';
        document.getElementById('TxtLimitErrorRequired').style.display = '';
    }
    else {
        var value = limit * 1;
        if(value <1 || value > 25)
        {
            document.getElementById('TxtLimitLabel').style.color = '#f00';
            document.getElementById('TxtLimitErrorOutOfRange').style.display = '';
            ok = false;
        }
    }

    return ok;
}

function Save() {
    if (ValidateForm() === false) {
        return false;
    }

    var webMethod = '';
    var data = null;
    if (rule.Id > 0) {
        webMethod = "/Async/RulesActions.asmx/RulesUpdate";
        data = {
            "newRules":
            {
                "Id": rule.Id,
                "Description": document.getElementById('TxtName').value.trim(),
                "Limit": document.getElementById('TxtLimit').value.trim() * 1,
                "Notes": document.getElementById('TxtNotes').value.trim(),
                "CompanyId": companyId
            },
            "oldRules": rule,
            "companyId": Company.Id,
            "userId": ApplicationUser.Id
        };
    }
    else {
        webMethod = "/Async/RulesActions.asmx/RulesInsert";
        data = {
            "rules":
            {
                "Id": rule.Id,
                "Description": document.getElementById('TxtName').value.trim(),
                "Limit": document.getElementById('TxtLimit').value.trim() * 1,
                "Notes": document.getElementById('TxtNotes').value.trim(),
                "CompanyId": companyId
            },
            "companyId": Company.Id,
            "userId": ApplicationUser.Id
        };
    }

    $("#DepartmentDesassociationDialog").dialog("close");
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if(msg.d.Success === false)
            {
                alertUI(msg.d.MessageError);
            }
            else
            {
                document.location = referrer;
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

jQuery(function ($) {

    $("#BtnSave").click(Save);
    $("#BtnCancel").click(function (e) {
        //document.location = document.referrer;
        document.location = referrer;
    });

    //override dialog"s title function to allow for HTML titles
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;"
            if (("title_html" in this.options) && this.options.title_html === true)
                title.html($title);
            else title.text($title);
        }
    }));

    if (ApplicationUser.ShowHelp === true) {
        SetToolTip("TxtName", Dictionary.Item_Department_Help_Field_Name);
        $("[data-rel=tooltip]").tooltip();
    }
});

RenderStepsSliders();

function RenderStepsSliders() {
    $("#input-span-slider-limit").slider({
        "value": rule.Limit,
        "range": "min",
        "min": MinStepValue,
        "max": 25,
        "step": 1,
        "slide": function (event, ui) {
            var val = parseInt(ui.value);
            if (val === 0) {
                return false;
            }
            $("#input-span-slider-probability").slider({ value: this.id });
            rule.Limit = val;
            $("#TxtLimit").val(rule.Limit);
        }
    });

    VoidTable("stepsLimit")
    for (var x = MinStepValue; x < 26; x++) {
        var span = document.createElement("span");
        span.id = x;
        span.className = "tick";
        span.appendChild(document.createTextNode(x));
        span.appendChild(document.createElement("BR"));
        span.appendChild(document.createTextNode("|"));
        span.style.left = ((100 / (25 - MinStepValue)) * (x - MinStepValue)) + "%";
        document.getElementById("stepsLimit").appendChild(span);
        if (x > 0) {
            span.onclick = function () {
                $("#input-span-slider-limit").slider({ value: this.id });
                rule.Limit = this.id * 1;
                $('#TxtLimit').val(rule.Limit);
            };
        }
    }
}

$('#TxtName').focus();

function Resize() {
    var listTable = document.getElementById("ListDataDiv");
    var containerHeight = $(window).height();
    var finalHeigth = containerHeight - 720;
    if (finalHeigth < 160) {
        finalHeigth = 160;
    }
    listTable.style.height = (finalHeigth) + "px";
}

if (typeof ApplicationUser.Grants.Rules === "undefined" || ApplicationUser.Grants.Rules.Write === false) {
    $(".btn-danger").hide();
    $("input").attr("disabled", true);
    $("textarea").attr("disabled", true);
    $("select").attr("disabled", true);
    $("select").css("background-color", "#eee");
    $("#BtnSave").hide();
    $(".ui-slider-handle").hide();
}


window.onload = function () {
    Resize();
}

window.onresize = function () { Resize(); }

$("#th1").click();