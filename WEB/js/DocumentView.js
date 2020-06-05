var anulationData = null;
var client;

//Connect Options
var options = {
    "timeout": 3,
    "useSSL": true,
    "onSuccess": function () {
        MQTTAfterConnect();
    },
    "onFailure": function (message) {
        console.log("Connection failed: " + message.errorMessage);
    }
};

function MQTTAfterConnect(message) {
    try {
        var topic = "issus/" + companyId + "/Versioned/#";
        client.subscribe(topic, { "qos": 1 });
        console.log("subscription", topic);
    }
    catch (e) {
        console.log(e);
    }
}

var publish = function (payload, topic, qos) {
    try {
        //Send your message (also possible to serialize it as JSON or protobuf or just use a string, no limitations)
        var message = new Messaging.Message(userId + "|" + payload);
        message.destinationName = topic;
        message.qos = qos;
        client.send(message);
        console.log("topic", topic);
    }
    catch (e) {
        console.log(e);
    }
};

$(document).ready(function () {
    $("#BtnNewVersion").click(Versioned);
    $("#BtnSave").click(SetReason);
    $("#BtnCancel").click(function (e) {
        document.location = referrer;
    });

    if (documento.LastVersion === 0) {
        $("#TxtRevision").val(0);
    }

    $("#BtnAnular").on("click", AnularPopup);
    $("#BtnRestaurar").on("click", Restore);

    if (typeof documento.EndDate !== "undefined" && documento.EndDate !== null && documento.EndDate !== "") {
        AnulateLayout();
    }

    if (documentId > 0) {
        $("#TxtRevisionDate").attr("disabled", "disabled");
        $("#TxtRevisionDateBtn").hide();
    }
    else {
        $("#TxtRevision").val(0);
    }

    console.log("MQTT connect");
    client = new Messaging.Client("broker.mqttdashboard.com", 8000, "issus_" + parseInt(Math.random() * 100, 10));

    client.onConnectionLost = function (responseObject) {
        //Depending on your scenario you could implement a reconnect logic here
        console.log("connection lost: " + responseObject.errorMessage);
    };

    //Gets called whenever you receive a message for your subscriptions
    client.onMessageArrived = function (message) {
        //Do something with the push message you received
        //$('#messages').append('<span>Topic: ' + message.destinationName + '  | ' + message.payloadString + '</span><br/>');
        MQTTversioned(message);
    };

    client.connect(options);
    console.log("MQTT connect - END");

    if (documentId === -1) {
        $("#TxtRevisionDate").attr("disabled", "disabled");
        $("#TxtRevisionDateBtn").hide();
        $("#TxtStartDate").on("change", CopyStartToRevision);
        CopyStartToRevision();
    }
});

function CopyStartToRevision() {
    $("#TxtRevisionDate").val($("#TxtStartDate").val());
}

function MQTTversioned(message) {
    var token = message.destinationName;
    var value = message.payloadString;
    if (value.split('|')[0] === userId.toString()) {
        return false;
    }

    if (value.split('|')[1] === documentId.toString()) {
        var version = value.split('|')[2];
        alert("Acaban de versionar el document a la versión " + version);
        console.log(message.destinationName, message.payloadString);
    }
}

function Versioned() {
    $("#TxtNewReason").val("");
    $("#TxtNewReasonErrorRequired").hide();
    $("#ReasonDialog").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Document_Button_NewVersion,
        "title_html": true,
        "width": 400,
        "buttons": [
            {
                "id": "BtnNewCategorySave",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Common_Accept,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    var ok = true;
                    $("#TxtNewReasonErrorRequired").hide();
                    $("#TxtNewRevisionErrorRequired").hide();
                    $("#TxtNewRevisionMalformed").hide();
                    $("#TxtNewRevisionCrossDate").hide();

                    if ($("#TxtNewReason").val() === "") {
                        $("#TxtNewReasonErrorRequired").show();
                        ok = false;
                    }

                    if ($("#TxtNewRevision").val() === "") {
                        ok = false;
                        $("#TxtNewRevisionErrorRequired").show();
                    }
                    else {
                        if (validateDate($("#TxtNewRevision").val()) === false) {
                            ok = false;
                            $("#TxtNewRevisionMalformed").show();
                        }
                        else {
                            var revdate = GetDate($("#TxtNewRevision").val(), "/", false);
                            var oldDate = GetDate($("#TxtRevisionDate").val(), "/", false);
                            if (revdate < oldDate) {
                                ok = false;
                                $("#TxtNewRevisionCrossDate").html(Dictionary.Item_Document_ErrorMessage_RevisionOverDate + " " + $("#TxtRevisionDate").val());
                                $("#TxtNewRevisionCrossDate").show();
                            }
                        }
                    }

                    if (ok === true) {
                        VersionedConfirmed();
                    }

                    return null;
                }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $("#TxtNewReasonErrorRequired").hide();
                    $(this).dialog("close");
                    return null;
                }
            }
        ]
    });
}

function VersionedConfirmed() {
    var data = {
        "date": $("#TxtNewRevision").val(),
        "documentId": documentId,
        "userId": userId,
        "companyId": Company.Id,
        "version": $("#TxtRevision").val(),
        "reason": $("#TxtNewReason").val()
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/Versioned",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.getElementById("TxtRevision").value = msg.d.MessageError;
            document.getElementById("TxtRevisionDate").value = FormatDate(new Date(), "/");
            document.getElementById("TxtMotivo").value = document.getElementById("TxtNewReason").value;
            InsertVersionRow();
            $("#ReasonDialog").dialog("close");
            attachActual = null;
            SetAttachLayout();
            publish(documentId + "|" + $("#TxtRevision").val(), "issus/" + companyId + "/Versioned", 2);
        },
        "error": function (msg) {
            LoadingHide();
            alertUI("error:" + msg.d.MessageError);
            $("#ReasonDialog").dialog("close");
        }
    });
}

function InsertVersionRow() {
    var target=document.getElementById("TableDocumentVersion");
    var tr = document.createElement("TR");
    var td1 = document.createElement("TD");
    td1.appendChild(document.createTextNode(document.getElementById("TxtRevision").value));
    var td2 = document.createElement("TD");
    td2.appendChild(document.createTextNode(FormatDate(new Date(), "/")));
    var td3 = document.createElement("TD");
    td3.appendChild(document.createTextNode(document.getElementById("TxtMotivo").value));
    var td4 = document.createElement("TD");
    td4.appendChild(document.createTextNode(ApplicationUser.Employee.Name));

    var td5 = document.createElement("TD");
    td5.id = "Icons" + document.getElementById("TxtRevision").value;
    td5.appendChild(document.createTextNode(" "));

    var tdVoid = document.createElement("TD");
    tdVoid.id = "DOC" + document.getElementById("TxtRevision").value;
    tdVoid.appendChild(document.createTextNode(" "));

    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);
    tr.appendChild(tdVoid);
    tr.appendChild(td4);
    tr.appendChild(td5);
    
    target.innerHTML = tr.innerHTML + target.innerHTML;
}

function Save() {
    if (documento.Id > 0) {
        Update();
    }
    else {
        Insert();
    }
}

function Insert() {
    var data = {
        "newDocument": {
            "Id": -1,
            "CompanyId": Company.Id,
            "Code": $("#TxtCodigo").val(),
            "Description": $("#TxtDocumento").val(),
            "StartDate": GetDate($("#TxtStartDate").val(), "-"),
            "EndDate": GetDate($("#TxtEndDate").val(), "-"),
            "Category": { "Id": categorySelected },
            "Origin": { "Id": procedenciaSelected },
            "Conservation": $("#TxtConservacion").val(),
            "ConservationType": $("#CmbConservacion").val(),
            "Source": $("#CmbOrigen").val() * 1 === 2,
            "Location": $("#TxtUbicacion").val(),
            "Active": true
        },
        "revisionDate": $("#TxtRevisionDate").val(),
        "version": $("#TxtRevision").val() * 1,
        "reason": selectedReason,
        "userId": userId
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/Insert",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            LoadingHide();
            document.location = referrer;
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.d.MessageError);
        }
    });
}

function Update() {
    var webMethod = "/Async/DocumentActions.asmx/Update";
    documento.StartDate = GetDate(documento.StartDate, "-");
    documento.EndDate = GetDate(documento.EndDate, "-");
    documento.RevisionDate = GetDate(documento.RevisionDate, "-");
    var data = {
        "oldDocument": documento,
        "newDocument": {
            "Id": documento.Id,
            "CompanyId": companyId,
            "Code": document.getElementById("TxtCodigo").value,
            "Description": document.getElementById("TxtDocumento").value,
            "StartDate": GetDate(document.getElementById("TxtStartDate").value, "/"),
            //"EndDate": GetDate(document.getElementById("TxtEndDate").value, "/"),
            "Category": { "Id": categorySelected },
            "RevisionDate": GetDate(document.getElementById("TxtRevisionDate").value, "/"),
            "Origin": { "Id": procedenciaSelected },
            "Conservation": $("#TxtConservacion").val(),
            "ConservationType": $("#CmbConservacion").val(),
            "Source": $("#CmbOrigen").val() * 1 === 2,
            "Location": $("#TxtUbicacion").val(),
            "Active": true
        },
        "reason": selectedReason,
        "userId": userId
    };

    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": webMethod,
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            LoadingHide();
            document.location = referrer;
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg);
        }
    });
}

// ISSUS-190
document.getElementById("TxtCodigo").focus();

function AnularPopup() {
    $("#TxtEndDate").val(FormatDate(new Date(), "/"));
    $("#TxtAnularComments").val("");
    $("#dialogAnular").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Document_PopupAnular_Title,
        "width": 600,
        "buttons":
        [
            {
                "id": "BtnAnularDocument",
                "html": "<i class=\"icon-ok bigger-110\"></i>&nbsp;" + Dictionary.Item_Document_Btn_Anular,
                "class": "btn btn-success btn-xs",
                "click": function () { AnularConfirmed(); }
            },
            {
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    });
}

function AnularConfirmed() {
    console.log("AnularConfirmed");
    $("#TxtAnularCommentsLabel").css("color", Color.Label);
    $("#TxtEndDateLabel").css("color", Color.Label);
    $("#TxtAnularCommentsErrorRequired").hide();
    $("#TxtEndDateErrorRequired").hide();
    $("#TxtEndDateMalformed").hide();
    $("#TxtEndDateCrossDate").hide();
	var crossDateMessage = $("#TxtEndDateCrossDate").html();
	crossDateMessage = crossDateMessage.split("#").join($("#TxtRevisionDate").val());
	$("#TxtEndDateCrossDate").html(crossDateMessage);

    var ok = true;
    if ($("#TxtEndDate").val() === "") {
        ok = false;
        $("#TxtEndDateLabel").css("color", Color.Error);
        $("#TxtEndDateErrorRequired").show();
    }
    else {
        if (validateDate($("#TxtEndDate").val()) === false) {
            ok = false;
            $("#TxtEndDateLabel").css("color", Color.Error);
            $("#TxtEndDateMalformed").show();
        }
        else {
            var lastRevisionDate = GetDate($("#TxtRevisionDate").val(), "/", false);
            var inactivateDate = GetDate($("#TxtEndDate").val(), "/", false);
            if (inactivateDate < lastRevisionDate) {
				ok = false;
                $("#TxtEndDateLabel").css("color", Color.Error);
                $("#TxtEndDateCrossDate").show();
            }
        }
    }

    if (ok === false) {
        return false;
    }

    var data = {
        "documentId": documentId,
        "companyId": companyId,
        "endReason": $("#TxtAnularComments").val(),
        "endDate": GetDate($("#TxtEndDate").val(), "/"),
        "userId": user.Id
    };
    anulationData = data;
    $("#dialogAnular").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/Anulate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            document.location = referrer;
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function AnulateLayout() {
    $("input").attr("disabled", "disabled");
    $("select").attr("disabled", "disabled");
    $("#BtnNewUploadfileVersion").hide();
    $("#BtnNewVersion").hide();
    $("#BtnCategory").hide();
    $(".btn-danger").hide();
    $("#BtnSave").hide();
    $("input").css("background-color", "#eee");
    $("select").css("background-color", "#eee");
    $("textarea").css("background-color", "#eee");

    var message = "";
    message += "<br /><div class=\"alert alert-info\" style=\"display: block;\" id=\"DivAnulateMessage\">";
    message += "    <strong><i class=\"icon-info-sign fa-2x\"></i></strong>";
    message += "    <h3 style=\"display:inline;\">" + Dictionary.Item_Document_AnulateMessageTile + "</h3><br />";
    message += "    <p style=\"margin-left:50px;\">";
    message += "        " + Dictionary.Item_Document_FieldLabel_EndReason + ": <strong>" + documento.EndReason + "</strong><br />";
    message += "        " + Dictionary.Item_Document_FieldLabel_InactiveDate + ": <strong>" + documento.EndDate + "</strong><br />";
    message += "    </p>";
    message += "</div>";

    $("#oldFormFooter").before(message);
}

function Restore() {
    var data = {
        "documentId": documentId,
        "companyId": companyId,
        "userId": user.Id
    };
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DocumentActions.asmx/Restore",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            document.location = referrer;
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}