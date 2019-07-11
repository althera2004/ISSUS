var MinStepValue = rule.Id > 0 ? 1 : 0;
var reason = "";

function ValidateForm()
{
    var ok = true;
    $("#TxtNameLabel").css("color", "#000");
    $("#TxtLimitLabel").css("color", "#000");
    $("#TxtNameErrorRequired").hide();
    $("#TxtNameErrorDuplicated").hide();
    $("#TxtLimitErrorRequired").hide();
    $("#TxtLimitErrorOutOfRange").hide();

    var name = document.getElementById("TxtName").value.trim();
    var limit = document.getElementById("TxtLimit").value.trim();

    if (name === "") {
        ok = false;
        $("#TxtNameLabel").css("color", Color.Error);
        $("#TxtNameErrorRequired").show();
    }
    else {
        for (var x = 0; x < companyRules.length; x++) {
            if (companyRules[x].Description === name && companyRules[x].Id !== rule.Id) {
                ok = false;
                $("#TxtNameLabel").css("color", Color.Error);
                $("#TxtNameErrorDuplicated").show();
                break;
            }
        }
    }

    if (limit === "") {
        ok = false;
        $("#TxtLimitLabel").css("color", Color.Error);
        $("#TxtLimitErrorRequired").show();
    }
    else {
        var value = limit * 1;
        if(value <1 || value > 25)
        {
            $("#TxtLimitLabel").css("color",  "#f00");
            $("#TxtLimitErrorOutOfRange").show();
            ok = false;
        }
    }

    return ok;
}

function Save() {
    if (ValidateForm() === false) {
        return false;
    }

    var newIPR = $("#TxtLimit").val() * 1;

    if (oldIPR !== newIPR) {
        if (ShowIPRChanges(newIPR, oldIPR) === false) {
            return false;
        }

        AskReason();
        return false;
    }

    SaveConfirmed();
}

function SaveConfirmed() {
    var webMethod = "";
    var data = null;
    if (rule.Id > 0) {
        rule.Limit = oldIPR;
        webMethod = "/Async/RulesActions.asmx/RulesUpdate";
        data = {
            "newRules":
                {
                    "Id": rule.Id,
                    "Description": $("#TxtName").val().trim(),
                    "Limit": $("#TxtLimit").val().trim() * 1,
                    "Notes": $("#TxtNotes").val().trim(),
                    "CompanyId": companyId
                },
            "oldRules": rule,
            "reason": reason,
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
                    "Description": $("#TxtName").val().trim(),
                    "Limit": $("#TxtLimit").val().trim() * 1,
                    "Notes": $("#TxtNotes").val().trim(),
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
        document.location = referrer;
    });

    //override dialog"s title function to allow for HTML titles
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;";
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

    VoidTable("stepsLimit");
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


function Resize() {
    var containerHeight = $(window).height();
    var finalHeigth = containerHeight - 720;
    if (finalHeigth < 160) {
        finalHeigth = 160;
    }
    $("#ListDataDiv").height(finalHeigth);
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
    RenderStepsSliders();
    $("#TxtName").focus();
    $("#th1").click();
};

window.onresize = function () { Resize(); };

function ShowIPRChanges(newIPR, oldIPR) {
    var risks = [];
    $("#BusinessRiskList").html("");
    for (var x = 0; x < businessRisk.length; x++) {
        var risk = businessRisk[x];
        if (newIPR > oldIPR) {
            if (risk.Value > oldIPR && risk.Value <= newIPR) {
                risks.push(risk);
            }
        }

        if (newIPR < oldIPR) {
            if (risk.Value < oldIPR && risk.Value >= newIPR) {
                risks.push(risk);
            }
        }
    }

    if (risks.length > 0) {
        for (var h = 0; h < risks.length; h++) {
            var li = document.createElement("LI");
            li.appendChild(document.createTextNode(risks[h].Name));
            document.getElementById("BusinessRiskList").appendChild(li);
        }

        var dialog = $("#dialogChangeIPR").removeClass("hide").dialog({
            "resizable": false,
            "width": 500,
            "modal": true,
            "title": "<h4 class=\"smaller\">" + Dictionary.Item_Rule_ChangeIPRTitle + "</h4>",
            "title_html": true,
            "buttons":
                [
                    {
                        "id": "DeleteUploadFileBtnOk",
                        "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                        "class": "btn btn-danger btn-xs",
                        "click": function () {
                            $(this).dialog("close");
                            AskReason();
                        }
                    },
                    {
                        "id": "DeleteUploadFileBtnCancel",
                        "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                        "class": "btn btn-xs",
                        "click": function () {
                            $(this).dialog("close");
                        }
                    }
                ]
        });
    }
    else {
        return true;
    }

    return false;
}

function AskReason() {
    $("#TxtReasonErrorRequired").hide();
    $("TxtReason").val();
    var dialog = $("#dialogChangeIPRReason").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_Rule_ChangeIPRTitle + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "id": "DeleteUploadFileBtnOk",
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        reason = $("#TxtReason").val();
                        if (reason === "") {
                            $("#TxtReasonErrorRequired").show();
                            return false;
                        }

                        $(this).dialog("close");
                        SaveConfirmed();
                    }
                },
                {
                    "id": "DeleteUploadFileBtnCancel",
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}