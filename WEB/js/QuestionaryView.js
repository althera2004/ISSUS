var selected = null;
var textToUpdate = "";
var nomatchValue = "";

jQuery(function ($) {
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || "&nbsp;"
            if (("title_html" in this.options) && this.options.title_html === true)
                title.html($title);
            else title.text($title);
        }
    }));
});

window.onload = function () {
    $("#nav-search").hide();
    $("#BtnSave").on("click", SaveQuestionary);
    $("#BtnCancel").on("click", function () { document.location = referrer; });
    RenderQuestionsList();
    $("#BtnNewItem").on("click", InsertQuestion);
    FillApartadoNorma();
    $("#CmbProcess").chosen().css("min-width", "99%");
    $("#CmbRule").chosen().css("min-width", "99%");
    $("#CmbApartadoNorma").chosen({ "adminNewValues": true }).on("chosen:hiding_dropdown", function () {
        nomatchValue = $("#DivCmbApartadoNorma .chosen-search input").val();
        CmbApartadoNormaChanged();
    }).on("chosen:showing_dropdown", function () {
        $("#CmbApartadoNorma").val("");
        }).css("min-width", "99%");
    TableQuestionsLayout();
};

function RenderQuestionsList() {
    $("#scrollTableDiv").show();
    $("#ListDataDiv").hide();
    $("#NoData").hide();

    if (Questions.length > 0) {
        var res = "";
        for (var x = 0; x < Questions.length; x++) {
            res += "<tr id=\"" + Questions[x].Id + "\">";
            res += "  <td>" + Questions[x].Description + "</td>";
            res += "    </td>";
            res += "  <td style=\"width:90px;\">";
            res += "    <span class=\"btn btn-xs btn-info\" id=\"" + Questions[x].Id + "\" onclick=\"EditQuestion(" + Questions[x].Id + ");\">";
            res += "        <i class=\"icon-edit bigger-120\"></i>";
            res += "    </span>";
            res += "    <span class=\"btn btn-xs btn-danger\" id=\"" + Questions[x].Id + "\" onclick=\"DeleteQuestion(" + Questions[x].Id + ");\">";
            res += "      <i class=\"icon-trash bigger-120\"></i>";
            res += "    </span>";
            res += "  </td>";
            res += "</tr>";
        }

        $("#ListDataDiv").show();
    }
    else {
        $("#NoData").show();
    }

    $("#QuestionsTotal").html(Questions.length);
    $("#ListDataTable").html(res);
}

function TableQuestionsLayout() {
    if (Questionary.Id > 0) {
        $("#DivNewQuestionary").hide();
        $("#scrollTableDiv").show();
        $("#ItemTableVoid").show();
        $("#TableQuestionsHeader").show();
    }
    else {
        $("#DivNewQuestionary").show();
        $("#scrollTableDiv").hide();
        $("#ItemTableVoid").hide();
        $("#TableQuestionsHeader").hide();
    }
}

function ValidateForm() {
    var ok = true;
    $("#TxtNameLabel").css("color", "#000");
    $("#TxtNameErrorRequired").hide();
    $("#TxtProcessNameLabel").css("color", "#000");
    $("#TxtProcessNameErrorRequired").hide();
    $("#TxtRuleNameLabel").css("color", "#000");
    $("#TxtRuleNameErrorRequired").hide();

    if ($("#TxtName").val() === "") {
        $("#TxtNameLabel").css("color", Color.Error);
        $("#TxtNameErrorRequired").show();
        ok = false;
    }

    if ($("#CmbProcess").val() * 1 < 1) {
        $("#TxtProcessNameLabel").css("color", Color.Error);
        $("#TxtProcessNameErrorRequired").show();
        ok = false;
    }

    if ($("#CmbRule").val() * 1 < 1) {
        $("#TxtRuleNameLabel").css("color", Color.Error);
        $("#TxtRuleNameErrorRequired").show();
        ok = false;
    }

    return ok;
}

function SaveQuestionary() {
    if (ValidateForm() === false) { return false; }
    var apartadoNorma = $("#TxtApartadoNormaName").val();
    if (apartadoNorma === "") {
        apartadoNorma = $("#CmbApartadoNorma :selected").val();
        if (apartadoNorma === "-1") {
            apartadoNorma = "";
        }
    }

    var data = {
        "questionary":
        {
            "Id": Questionary.Id,
            "CompanyId": Company.Id,
            "Description": $("#TxtName").val(),
            "Rule": { "Id": + $("#CmbRule").val() * 1, "Description": "" },
            "Process": { "Id": + $("#CmbProcess").val() * 1, "Description": "" },
            "ApartadoNorma": apartadoNorma,
            "Notes": $("#TxtNotes").val(),
            "Active": true,
            "Deletable": false
        },
        "userId": user.Id
    };
    $("#QuestionaryDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);

    var url = "/Async/QuestionaryActions.asmx/Insert";
    if (Questionary.Id > 0) {
        url = "/Async/QuestionaryActions.asmx/Update";
        data["oldQuestionary"] = Questionary;
    }

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": url,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (Questionary.Id < 0) {
                document.location = "QuestionaryView.aspx?id=" + msg.d.MessageError;
            }
            else {
                document.location = referrer;
            }
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function CmbProcessChanged() {

}

function QuestionById() {
    for (var x = 0; x < Questions.length; x++) {
        if (Questions[x].Id === selected) {
            return Questions[x];
        }
    }
    return null;
}

function DeleteQuestion(id) {
    selected = id * 1;
    var question = QuestionById();
    if (question === null) {
        return;
    }
    $("#QuestionaryQuestionName").html(question.Description);
    var dialog = $("#QuestionaryQuestionDeleteDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_ItemQuestion_Popup_Delete_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                        DeleteQuestionConfirmed(selected);
                    }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function DeleteQuestionConfirmed(id) {
    var data = {
        "questionId": id,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/QuestionaryActions.asmx/DeleteQuestion",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            var temp = [];
            for (var x = 0; x < Questions.length; x++) {
                if (Questions[x].Id !== selected) {
                    temp.push(Questions[x]);
                }
            }

            Questions = temp;
            RenderQuestionsList();
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function EditQuestion(id) {
    $("#TxtQuestionaryQuestionUpdateNameLabel").css("color", "#000");
    $("#TxtQuestionaryQuestionUpdateNameErrorRequired").hide();
    selected = id * 1;
    var question = QuestionById(id);
    if (question === null) {
        return;
    }
    $("#TxtQuestionaryQuestionUpdateName").val(question.Description);
    $("#QuestionaryQuestionUpdateDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_ItemQuestion_Popup_Edit_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        EditQuestionConfirmed(selected);
                    }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_No,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function EditQuestionConfirmed(id) {
    textToUpdate = $("#TxtQuestionaryQuestionUpdateName").val();

    if (textToUpdate === "") {
        $("#TxtQuestionaryQuestionUpdateNameLabel").css("color", Color.Error);
        $("#TxtQuestionaryQuestionUpdateNameErrorRequired").show();
        return false;
    }

    var data = {
        "questionId": id,
        "question": textToUpdate,
        "questionaryId": Questionary.Id,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/QuestionaryActions.asmx/UpdateQuestion",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            for (var x = 0; x < Questions.length; x++) {
                if (Questions[x].Id === selected) {
                    Questions[x].Description = textToUpdate;
                }
            }

            RenderQuestionsList();
            $("#QuestionaryQuestionUpdateDialog").dialog("close");
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function InsertQuestion() {
    $("#TxtQuestionaryQuestionNewNameLabel").css("color", "#000");
    $("#TxtQuestionaryQuestionNewNameErrorRequired").hide();
    console.log("InsertQuestion");
    $("#TxtQuestionaryQuestionNewName").val("");
    selected = -1;
    $("#QuestionaryQuestionInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "width": 600,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_ItemQuestion_Popup_Insert_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        InsertQuestionConfirmed(selected);
                    }
                },
                {
                    "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function InsertQuestionConfirmed(id) {
    textToUpdate = $("#TxtQuestionaryQuestionNewName").val();

    if (textToUpdate === "") {
        $("#TxtQuestionaryQuestionNewNameLabel").css("color", Color.Error);
        $("#TxtQuestionaryQuestionNewNameErrorRequired").show();
        return false;
    }

    var data = {
        "questionId": id,
        "question": textToUpdate,
        "questionaryId": Questionary.Id,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/QuestionaryActions.asmx/InsertQuestion",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            Questions.push({ "Id": response.d.MessageError * 1, "Description": textToUpdate });
            RenderQuestionsList();
            $("#QuestionaryQuestionInsertDialog").dialog("close");
        },
        "error": function (jqXHR) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function FillApartadoNorma() {
    $("#CmbApartadosNorma").html("");
    var RuleId = $("#CmbRule").val() * 1;
    console.log("FillApartadoNorma", RuleId);
    if (RuleId > 0) {
        var res = "<option value=\"-1\">" + Dictionary.Common_None_Male + "</option>";
        for (var x = 0; x < ApartadosNorma.length; x++) {
            if (ApartadosNorma[x].R === RuleId) {
                var selected = "";
                if (Questionary.ApartadoNorma === ApartadosNorma[x].A) {
                    selected = " selected=\"selected\"";
                    $("#TxtApartadoNormaName").val(ApartadosNorma[x].A);
                }

                res += "<option value=\"" + ApartadosNorma[x].A + "\"" + selected + ">" + ApartadosNorma[x].A + "</option>";
            }
        }

        $("#CmbApartadoNorma").removeAttr("disabled");
    }
    else {
        $("#CmbApartadoNorma").attr("disabled", "disabled");
    }

    $("#CmbApartadoNorma").html(res);
    $("#CmbApartadoNorma").trigger("chosen:updated");
}

function CmbApartadoNormaChanged() {
    $("#TxtApartadoNormaName").val(nomatchValue);
}