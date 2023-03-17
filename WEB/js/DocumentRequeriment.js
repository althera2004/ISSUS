var RequerimentDefinition = {
    "Id": -1
};

var RequerimentAct = {
    "Id": -1
};

function DOCUMENTREQUERIMENT_Init() {
    DOCUMENTREQUERIMENTDEFINITION_RenderTable();
    DOCUMENTREQUERIMENTACT_RenderTable();
    $("#BtnNewRequerimentDefinition").on("click", function () { DOCUMENTREQUERIMENTDEFINITION_PopupNewDefinition_Show(-1); });
    $("#BtnNewRequerimentAct").on("click", function () { DOCUMENTREQUERIMENTACT_PopupNewDefinition_Show(-1); });
    $("#TxtNewRequerimentActDate").datepicker();
}

function DOCUMENTREQUERIMENTDEFINITION_RenderTable() {
    var res = "";
    if (requerimentDefinitions.length > 0) {
        for (var x = 0; x < requerimentDefinitions.length; x++) {
            var requeriment = requerimentDefinitions[x];
            res += DOCUMENTREQUERIMENTDEFINITION_RenderRow(requeriment);
        }
    }
    else {

    }

    $("#TableDocumentRequerimentDefinition").html(res);
}

function DOCUMENTREQUERIMENTACT_RenderTable() {
    var res = "";
    if (requerimentActs.length > 0) {
        for (var x = 0; x < requerimentActs.length; x++) {
            var requeriment = requerimentActs[x];
            res += DOCUMENTREQUERIMENTACT_RenderRow(requeriment);
        }
    }
    else {

    }

    $("#TableDocumentRequerimentAct").html(res);
}

function DOCUMENTREQUERIMENTDEFINITION_RenderRow(data) {
    var res = "<tr>";

    res += "<td style=\"width:400px;\">" + data.Requeriment + "</td>";
    res += "<td style=\"text-align:right;width:150px;\">" + data.Periodicity + "&nbsp;</td>";
    res += "<td>" + data.Actuacio + "</td>";
    res += "<td style=\"text-align:right;width:90px;\">" + ToMoneyFormat(data.Cost) + "&nbsp;&euro;&nbsp;</td>";

    res += "<td style=\"width:60px;text-align:center;\">";
    res += "<i class=\"fa fa-pencil blue\" onclick=\"DOCUMENTREQUERIMENTDEFINITION_PopupNewDefinition_Show(" + data.Id + ")\"></i>";
    res += "&nbsp;&nbsp;";
    res += "<i class=\"fa fa-times red\"></i>";
    res += "</td>";

    res += "</tr>";
    return res;
}

function DOCUMENTREQUERIMENTACT_RenderRow(data) {
    var res = "<tr>";

    var date = data.Date.toString();
    date = date.substr(6, 2) + "/" + date.substr(4, 2) + "/" + date.substr(0, 4);
    var vto = data.Expiration.toString();
    vto = vto.substr(6, 2) + "/" + vto.substr(4, 2) + "/" + vto.substr(0, 4);

    var definition = DOCUMENTREQUERIMENTDEFINITION_ById(data.DocumentRequerimentDefinitionId);

    res += "<td style=\"width:90px;\">" + date + "</td>";
    res += "<td style=\"width:400px;\">" + definition.Requeriment + "</td>";
    res += "<td>" + data.Observations + "</td>";
    res += "<td style=\"width:200px;\">" + data.Responsible.Value + "</td>";
    res += "<td style=\"text-align:right;width:90px;\">" + ToMoneyFormat(data.Cost) + "&nbsp;&euro;&nbsp;</td>";
    res += "<td style=\"width:120px;\">" + vto + "</td>";
    res += "<td style=\"width:60px;text-align:center;\">";
    res += "<i class=\"fa fa-pencil blue\" onclick=\"DOCUMENTREQUERIMENTACT_PopupNewDefinition_Show(" + data.Id + ")\"></i>";
    res += "&nbsp;&nbsp;";
    res += "<i class=\"fa fa-times red\"></i>";
    res += "</td>";

    res += "</tr>";
    return res;
}

function DOCUMENTREQUERIMENTDEFINITION_ById(id) {
    if (id > 0) {
        if (requerimentDefinitions.length > 0) {
            for (var x = 0; x < requerimentDefinitions.length; x++) {
                if (requerimentDefinitions[x].Id === id) {
                    return requerimentDefinitions[x];
                }
            }
        }
    }

    return {
        "Id": -1,
        "CompanyId": Company.Id,
        "Requeriment": "",
        "Actuacio": "",
        "Cost": 0,
        "Periodicity": 0,
        "DocumentId": documentId,
        "Responsible": { "Id": -1 }
    };
}

function DOCUMENTREQUERIMENTACT_ById(id) {
    if (id > 0) {
        if (requerimentActs.length > 0) {
            for (var x = 0; x < requerimentActs.length; x++) {
                if (requerimentActs[x].Id === id) {
                    return requerimentActs[x];
                }
            }
        }
    }

    return {
        "Id": -1,
        "CompanyId": Company.Id,
        "Observations": "",
        "DocumentRequerimentDefinitionId": -1,
        "Cost": 0,
        "Periodicity": 0,
        "DocumentId": documentId,
        "Responsible": { "Id": -1 }
    };
}

function DOCUMENTREQUERIMENTDEFINITION_PopupNewDefinition_Show(id) {
    RequerimentDefinition = DOCUMENTREQUERIMENTDEFINITION_ById(id);

    $("#TxtNewRequerimentDefinitionRequeriment").val(RequerimentDefinition.Requeriment);
    $("#TxtNewRequerimentDefinitionPeriodicity").val(RequerimentDefinition.Periodicity);
    $("#TxtNewRequerimentDefinitionActuacio").val(RequerimentDefinition.Actuacio);
    $("#TxtNewRequerimentDefinitionCost").val(RequerimentDefinition.Cost);
    $("#CmbNewDefinitionResponsible").val(RequerimentDefinition.Responsible.Id);
    DOCUMENTREQUERIMENTDEFINITION_FillCmbResponsible();

    console.log("DOCUMENTREQUERIMENT_PopupNewDefinition_Show", id);
    $("#dialogNewRequerimentDefinition").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": RequerimentDefinition.Id < 0 ? Dictionary.Item_DocumentRequeriment_Popup_NewDefinition_Title : Dictionary.Item_DocumentRequeriment_Popup_UpdateDefinition_Title,
        "title_html": true,
        "width": 500,
        "buttons": [
            {
                "id": "BtnDocumentRequerimentDefinitionSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + (RequerimentDefinition.Id < 0 ? Dictionary.Common_Add : Dictionary.Common_Save),
                "class": "btn btn-success btn-xs",
                "click": function () {
                    DOCUMENTREQUERIMENTDEFINITION_Save();
                }
            },
            {
                "id": "BtnDocumentRequerimentDefinitionCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}


function DOCUMENTREQUERIMENTACT_PopupNewDefinition_Show(id) {
    RequerimentAct = DOCUMENTREQUERIMENTACT_ById(id);

    var cmbDefinitionData = "<option value=\"-1\">" + Dictionary.Common_SelectOne + "</option>";
    for (var x = 0; x < requerimentDefinitions.length; x++) {
        cmbDefinitionData += "<option value=\"" + requerimentDefinitions[x].Id + "\">";
        cmbDefinitionData += requerimentDefinitions[x].Requeriment;
        cmbDefinitionData += "</option>";
    }

    $("#CmbNewActDefinitionId").html(cmbDefinitionData);


    var date = "";
    if (typeof RequerimentAct.Date !== "undefined") {
        date = RequerimentAct.Date.toString();
        date = date.substr(6, 2) + "/" + date.substr(4, 2) + "/" + date.substr(0, 4);
    }
    var vto = "";
    if (typeof RequerimentAct.Expiration !== "undefined") {
        vto = RequerimentAct.Expiration.toString();
        vto = vto.substr(6, 2) + "/" + vto.substr(4, 2) + "/" + vto.substr(0, 4);
    }

    $("#CmbNewActDefinitionId").val(RequerimentAct.DocumentRequerimentDefinitionId);
    $("#TxtNewRequerimentActDate").val(date);
    $("#TxtNewRequerimentActObservations").val(RequerimentAct.Observations);
    $("#TxtNewRequerimentActCost").val(RequerimentAct.Cost);
    $("#CmbNewActResponsible").val(RequerimentAct.Responsible.Id);
    DOCUMENTREQUERIMENTACT_FillCmbResponsible();

    console.log("DOCUMENTREQUERIMENTACT_PopupNewDefinition_Show", id);
    $("#dialogNewRequerimentAct").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": RequerimentAct.Id < 0 ? Dictionary.Item_DocumentAct_Popup_NewDefinition_Title : Dictionary.Item_DocumentAct_Popup_UpdateDefinition_Title,
        "title_html": true,
        "width": 500,
        "buttons": [
            {
                "id": "BtnDocumentRequerimentActSave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + (RequerimentAct.Id < 0 ? Dictionary.Common_Add : Dictionary.Common_Save),
                "class": "btn btn-success btn-xs",
                "click": function () {
                    DOCUMENTREQUERIMENTDEFINITIONACT_Save();
                }
            },
            {
                "id": "BtnDocumentRequerimentActCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function DOCUMENTREQUERIMENTDEFINITION_Save() {
    var ok = true;
    $("#TxtNewRequerimentDefinitionRequerimentErrorRequired").hide();
    $("#TxtNewRequerimentDefinitionPeriodicityErrorRequired").hide();
    $("#CmbNewDefinitionResponsibleErrorRequired").hide();
    if ($("#TxtNewRequerimentDefinitionRequeriment").val() === "") {
        ok = false;
        $("#TxtNewRequerimentDefinitionRequerimentErrorRequired").show();
    }

    if ($("#TxtNewRequerimentDefinitionPeriodicity").val() === "" || $("#TxtNewRequerimentDefinitionPeriodicity").val() * 1 < 1) {
        ok = false;
        $("#TxtNewRequerimentDefinitionPeriodicityErrorRequired").show();
    }

    if ($("#CmbNewDefinitionResponsible").val() * 1 < 1) {
        ok = false;
        $("#CmbNewDefinitionResponsibleErrorRequired").show();
    }

    RequerimentDefinition.Description = $("#TxtNewRequerimentDefinitionRequeriment").val();
    RequerimentDefinition.Requeriment = $("#TxtNewRequerimentDefinitionRequeriment").val();
    RequerimentDefinition.Actuacio = $("#TxtNewRequerimentDefinitionActuacio").val();

    var cost = $("#TxtNewRequerimentDefinitionCost").val();
    if (isNaN(cost)) {
        cost = ParseInputValueToNumber($("#TxtNewRequerimentDefinitionCost").val());
    }

    RequerimentDefinition.Cost = cost;
    alert($("#TxtNewRequerimentDefinitionCost").val() + " -- " + cost);
    RequerimentDefinition.Periodicity = $("#TxtNewRequerimentDefinitionPeriodicity").val() * 1;
    RequerimentDefinition.Responsible = {
        "Id": $("#CmbNewDefinitionResponsible").val() * 1
    };

    if (ok === true) {
        var data = {
            "documentRequerimentDefinition": RequerimentDefinition,
            "userId": ApplicationUser.Id
        };

        LoadingShow(Dictionary.Common_Message_Saving);
        $.ajax({
            "type": "POST",
            "url": "/Async/DocumentRequerimentDefintionActions.asmx/Save",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                console.log(msg);

                if (RequerimentDefinition.Id < 0) {
                    RequerimentDefinition.Id = msg.d.ReturnValue * 1;
                    requerimentDefinitions.push(RequerimentDefinition);
                }
                else {
                    var temp = [];
                    for (var x = 0; x < requerimentDefinitions.length; x++) {
                        if (requerimentDefinitions[x].Id !== RequerimentDefinition.Id) {
                            temp.push(requerimentDefinitions[x])
                        } else {
                            temp.push(RequerimentDefinition);
                        }
                    }

                    requerimentDefinitions = temp;
                }

                DOCUMENTREQUERIMENT_RenderTable();
                $("#BtnDocumentRequerimentDefinitionCancel").click();
                //LoadingHide();
            },
            "error": function (msg) {
                //LoadingHide();
                alertUI("error:" + msg.d.MessageError);
            }
        });
    }
}

function DOCUMENTREQUERIMENTDEFINITIONACT_Save() {
    alert(RequerimentAct.Id);
}

function DOCUMENTREQUERIMENTDEFINITION_FillCmbResponsible() {
    VoidTable("CmbNewDefinitionResponsible");
    var optionDefault = document.createElement("option");
    optionDefault.value = 0;
    optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
    document.getElementById("CmbNewDefinitionResponsible").appendChild(optionDefault);

    for (var x = 0; x < Employees.length; x++) {
        if ((Employees[x].Active === true && Employees[x].DisabledDate === null) || RequerimentDefinition.Responsible.Id === Employees[x].Id) {
            var option = document.createElement("option");
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            if (RequerimentDefinition.Responsible.Id === Employees[x].Id) {
                option.selected = true;
            }

            document.getElementById("CmbNewDefinitionResponsible").appendChild(option);
        }
    }
}

function DOCUMENTREQUERIMENTACT_FillCmbResponsible() {
    VoidTable("CmbNewActResponsible");
    if (RequerimentAct.Id < 0) {
        var optionDefault = document.createElement("option");
        optionDefault.value = 0;
        optionDefault.appendChild(document.createTextNode(Dictionary.Common_SelectAll));
        document.getElementById("CmbNewActResponsible").appendChild(optionDefault);
    }

    for (var x = 0; x < Employees.length; x++) {
        if ((Employees[x].Active === true && Employees[x].DisabledDate === null) || RequerimentAct.Responsible.Id === Employees[x].Id) {
            var option = document.createElement("option");
            option.value = Employees[x].Id;
            option.appendChild(document.createTextNode(Employees[x].FullName));
            if (RequerimentAct.Responsible.Id === Employees[x].Id) {
                option.selected = true;
            }

            document.getElementById("CmbNewActResponsible").appendChild(option);
        }
    }
}