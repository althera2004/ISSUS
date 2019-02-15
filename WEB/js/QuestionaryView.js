var selected = null;
var textToUpdate = "";

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
    TableQuestionsLayout();
    RenderQuestionsList();
}

function RenderQuestionsList() {
    $("#QuestionsTotal").html(Questions.length);
    if (Questions.length > 0) {
        $("#ListDataDiv").show();
        $("#ItemTableVoid").hide();

        var res = "";
        for (var q = 0; q < Questions.length; q++) {
            res += "<tr id=\"" + Questions[q].Id + "\">";
            res += "<td>" + Questions[q].Description + "</td>";
            res += "<td style=\"width:90px;\">";
            res += "<span class=\"btn btn-xs btn-info\" onclick=\"EditQuestion(" + Questions[q].Id + ");\" title=\"" + Dictionary.Item_QuestionaryQuestion_Tooltip_EditButton + "\"><i class=\"icon-edit bigger-120\"></i></span>";
            res += "&nbsp;"
            res += "<span class=\"btn btn-xs btn-danger\" onclick=\"DeleteQuestion(" + Questions[q].Id + ");\" title=\"" + Dictionary.Item_QuestionaryQuestion_Tooltip_DeleteButton + "\"><i class=\"icon-trash bigger-120\"></i></span>";
            res += "</td>";
            res += "</tr>";
        }

        $("#ListDataTable").html(res);
    }
    else {
        $("#ListDataDiv").hide();
        $("#ItemTableVoid").show();
    }
}

function TableQuestionsLayout() {
    if (Questionary.Id > 0) {
        $("#scrollTableDiv").show();
        $("#ItemTableVoid").show();
        $("#TableQuestionsHeader").show();
    }
    else {
        $("#DivNewQuestionary").show();
    }
}

function SaveQuestionary() {
    var data = {
        "questionary":
        {
            "Id": Questionary.Id,
            "CompanyId": Company.Id,
            "Description": $("#TxtName").val(),
            "Rule": { "Id": + $("#CmbRule").val() * 1, "Description": "" },
            "Process": { "Id": + $("#CmbProcess").val() * 1, "Description": "" },
            "ApartadoNorma": "weke",
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

function CmbRuleChanged() {

}

function QuestionById(id) {
    for (var x = 0; x < Questions.length; x++) {
        if (Questions[x].Id !== selected) {
            return Questions[x];
        }
    }
    return null;
}

function DeleteQuestion(id) {
    var question = QuestionById(id);
    if (question === null) {
        return;
    }
    $("#QuestionaryQuestionName").html(question.Description);
    selected = id * 1;
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
                        DeleteQuestionConfirmed(Selected);
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
    var question = QuestionById(id);
    if (question === null) {
        return;
    }
    $("#TxtQuestionaryQuestionUpdateName").val(question.Description);
    selected = id * 1;
    $("#QuestionaryQuestionUpdateDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_ItemQuestion_Popup_Edit_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                        EditQuestionConfirmed(Selected);
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
    var data = {
        "questionId": id,
        "question": textToUpdate,
        "companyId": Company.Id,
        "userId": user.Id
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/QuestionaryActions.asmx/EditQuestion",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            for (var x = 0; x < Questions.length; x++) {
                if (Questions[x].Id === selected) {
                    Questions.Description = textToUpdate;
                }
            }

            RenderQuestionsList();
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function InsertQuestion(id) {
    $("#TxtQuestionaryQuestionUpdateName").val("");
    selected = -1;
    var dialog = $("#QuestionaryQuestionInsertDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": "<h4 class=\"smaller\">" + Dictionary.Item_ItemQuestion_Popup_Insert_Title + "</h4>",
        "title_html": true,
        "buttons":
            [
                {
                    "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Yes,
                    "class": "btn btn-danger btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                        EditQuestionConfirmed(Selected);
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

function InsertQuestionConfirmed(id) {
    textToUpdate = $("#TxtQuestionaryQuestionNewName").val();
    var data = {
        "questionId": id,
        "question": textToUpdate,
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
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}