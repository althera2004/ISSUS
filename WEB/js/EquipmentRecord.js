function EquipmentRecordGetFilter(filterData, exportType) {
    $.ajax({
        type: "POST",
        url: "/Async/EquipmentActions.asmx/GetFilter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(filterData, null, 2),
        success: function (msg) {
            var EquipmentRecordList;
            eval("EquipmentRecordList=" + msg.d + ";");
            EquipmentRecordRenderTable(EquipmentRecordList);
            if (typeof exportType !== "undefined") {
                if (exportType === "PDF") {
                    ExportPDF();
                }

                if (exportType === "Excel") {
                    ExportExcel;
                }
            }
        },
        error: function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function EquipmentRecordGetNone() {
    document.getElementById('BtnRecordShowAll').style.display = '';
    document.getElementById('BtnRecordShowNone').style.display = 'none';

    document.getElementById('CalInt').checked = false;
    document.getElementById('CalExt').checked = false;
    document.getElementById('VerInt').checked = false;
    document.getElementById('VerExt').checked = false;
    document.getElementById('ManInt').checked = false;
    document.getElementById('ManExt').checked = false;
    document.getElementById('RepInt').checked = false;
    document.getElementById('RepExt').checked = false;
    document.getElementById('TxtRecordsFromDate').value = '';
    document.getElementById('TxtRecordsToDate').value = '';

    var filterData =
    {
        equipmentId: Equipment.Id,
        companyId: Company.Id,
        calibrationInternal: false,
        calibrationExternal: false,
        verificationInternal: false,
        verificationExternal: false,
        maintenanceInternal: false,
        maintenanceExternal: false,
        repairInternal: false,
        repairExternal: false,
        dateFrom: null,
        dateTo: null
    };
    VoidTable('EquipmentRecordTable');
}

function EquipmentRecordGetAll() {
    document.getElementById('BtnRecordShowAll').style.display = 'none';
    document.getElementById('BtnRecordShowNone').style.display = '';

    document.getElementById('CalInt').checked = true;
    document.getElementById('CalExt').checked = true;
    document.getElementById('VerInt').checked = true;
    document.getElementById('VerExt').checked = true;
    document.getElementById('ManInt').checked = true;
    document.getElementById('ManExt').checked = true;
    document.getElementById('RepInt').checked = true;
    document.getElementById('RepExt').checked = true;
    document.getElementById('TxtRecordsFromDate').value = '';
    document.getElementById('TxtRecordsToDate').value = '';

    var filterData =
    {
        equipmentId: Equipment.Id,
        companyId: Company.Id,
        calibrationInternal : true,
        calibrationExternal : true,
        verificationInternal : true,
        verificationExternal : true,
        maintenanceInternal : true,
        maintenanceExternal : true,
        repairInternal : true,
        repairExternal : true,
        dateFrom : null,
        dateTo : null
    };
    EquipmentRecordGetFilter(filterData);
}

function EquipmentRecordGetFromFilter(exportType) {
    var ok = true;
    document.getElementById("ErrorItem").style.display = 'none';
    document.getElementById("ErrorDate").style.display = 'none';
    document.getElementById('ErrorDateMalformedFrom').style.display = 'none';
    document.getElementById('ErrorDateMalformedTo').style.display = 'none';

    var dateFrom = null
    if (document.getElementById('TxtRecordsFromDate').value !== '') {

        if (!RequiredDateValue('TxtRecordsFromDate')) {
            ok = false;
            document.getElementById('ErrorDateMalformedFrom').style.display = '';
        }
        else {
            dateFrom = GetDate($('#TxtRecordsFromDate').val(), '-');
        }
    }

    var dateTo = null;
    if (document.getElementById('TxtRecordsToDate').value !== '') {
        if (!RequiredDateValue('TxtRecordsToDate')) {
            ok = false;
            document.getElementById('ErrorDateMalformedTo').style.display = '';
        }
        else {
            dateTo = GetDate($('#TxtRecordsToDate').val(), '-');
        }
    }

    if (
        document.getElementById('CalInt').checked === false &&
        document.getElementById('CalExt').checked === false &&
        document.getElementById('VerInt').checked === false &&
        document.getElementById('VerExt').checked === false &&
        document.getElementById('ManInt').checked === false &&
        document.getElementById('ManExt').checked === false &&
        document.getElementById('RepInt').checked === false &&
        document.getElementById('RepExt').checked === false
    )
    {
        ok = false;
        document.getElementById("ErrorItem").style.display = '';
    }

    if (dateFrom != null && dateTo != null)
    {
        if (dateFrom > dateTo) {
            ok = false;
            document.getElementById('ErrorDate').style.display = '';
        }
    }

    if(ok===false)
    {
        document.getElementById('EquipmentRecordTable').style.display = 'none';
        document.getElementById('ItemTableError').style.display = '';
        document.getElementById('ItemTableVoid').style.display = 'none';
        return false;
    }
    var filterData =
    {
        equipmentId: Equipment.Id,
        companyId: Company.Id,
        calibrationInternal : document.getElementById('CalInt').checked,
        calibrationExternal : document.getElementById('CalExt').checked,
        verificationInternal : document.getElementById('VerInt').checked,
        verificationExternal : document.getElementById('VerExt').checked,
        maintenanceInternal : document.getElementById('ManInt').checked,
        maintenanceExternal : document.getElementById('ManExt').checked,
        repairInternal : document.getElementById('RepInt').checked,
        repairExternal : document.getElementById('RepExt').checked,
        dateFrom: dateFrom,
        dateTo: dateTo
    };
    EquipmentRecordGetFilter(filterData, exportType);
}

function EquipmentRecordRenderTable(EquipmentRecordList) {
    var target = document.getElementById('EquipmentRecordTable');
    VoidTable(target.id);

    // Ocultar los footers antes de mostrar el resultado
    target.style.display = 'none';
    document.getElementById('ItemTableError').style.display = 'none';
    document.getElementById('ItemTableVoid').style.display = 'none';

    if (EquipmentRecordList.length == 0)
    {
        document.getElementById('ItemTableVoid').style.display = '';
        return;
    }

    target.style.display='';
    var total = 0;
    for (var x = 0; x < EquipmentRecordList.length; x++) {
        total += EquipmentRecordRenderRow(EquipmentRecordList[x], target);
    }

    var row = document.createElement('TR');
    var td1 = document.createElement('TD');
    var tdLabel = document.createElement('TD');
    var tdValue = document.createElement('TD');

    td1.colSpan = 3;
    var td1i = document.createElement("I");
    td1i.style.color = "#aaa";
    td1i.appendChild(document.createTextNode(Dictionary.Common_RegisterCount + ": " + EquipmentRecordList.length));
    td1.appendChild(td1i);


    tdLabel.style.fontWeight = 'bold';
    tdValue.style.fontWeight = 'bold';
    tdValue.align = "right";
    tdLabel.align = "right";
    tdLabel.appendChild(document.createTextNode(Dictionary.Item_EquipmentRepair_HeaderList_Total));
    tdValue.appendChild(document.createTextNode(ToMoneyFormat(total,2)));
    row.appendChild(td1);
    row.appendChild(tdLabel);
    row.appendChild(tdValue);
    target.appendChild(row);

    $("#RegistrosTHead #th0").click();
    var className = $("#RegistrosTHead #th0").attr("class");
    while (className.indexOf("DESC") === -1) {
        $("#RegistrosTHead #th0").click();
        className = $("#RegistrosTHead #th0").attr("class");
    }
}

function EquipmentRecordRenderRow(EquipmentRecord, target) {
    var row = document.createElement('TR');
    var tdDate = document.createElement('TD');
    var tdItemType = document.createElement('TD');
    var tdOperation = document.createElement('TD');
    var tdResponsible = document.createElement('TD');
    var tdCost = document.createElement('TD');

    tdDate.align = 'center';
    tdCost.align = 'right';

    tdDate.appendChild(document.createTextNode(FormatYYYYMMDD(EquipmentRecord.Date,'/')));
    tdItemType.appendChild(document.createTextNode(EquipmentRecord.Type));
    tdOperation.appendChild(document.createTextNode(EquipmentRecord.Operation));
    tdResponsible.appendChild(document.createTextNode(EquipmentRecord.Responsible.Value));

    if (EquipmentRecord.Cost !== null) {
        tdCost.appendChild(document.createTextNode(ToMoneyFormat(EquipmentRecord.Cost, 2)));
    }

    row.appendChild(tdDate);
    row.appendChild(tdItemType);
    row.appendChild(tdOperation);
    row.appendChild(tdResponsible);
    row.appendChild(tdCost);
    target.appendChild(row);

    return EquipmentRecord.Cost * 1;
}

function ExportExcel()
{
    var dateFrom = null
    if (document.getElementById('TxtRecordsFromDate').value !== '') {

        if (!RequiredDateValue('TxtRecordsFromDate')) {
            ok = false;
            document.getElementById('ErrorDateMalformedFrom').style.display = '';
        }
        else {
            dateFrom = GetDate($('#TxtRecordsFromDate').val(), '-');
        }
    }

    var dateTo = null;
    if (document.getElementById('TxtRecordsToDate').value !== '') {
        if (!RequiredDateValue('TxtRecordsToDate')) {
            ok = false;
            document.getElementById('ErrorDateMalformedTo').style.display = '';
        }
        else {
            dateTo = GetDate($('#TxtRecordsToDate').val(), '-');
        }
    }
    var filterData =
    {
        equipmentId: Equipment.Id,
        companyId: Company.Id,
        calibrationInternal : document.getElementById('CalInt').checked,
        calibrationExternal : document.getElementById('CalExt').checked,
        verificationInternal : document.getElementById('VerInt').checked,
        verificationExternal : document.getElementById('VerExt').checked,
        maintenanceInternal : document.getElementById('ManInt').checked,
        maintenanceExternal : document.getElementById('ManExt').checked,
        repairInternal : document.getElementById('RepInt').checked,
        repairExternal : document.getElementById('RepExt').checked,
        dateFrom: dateFrom,
        dateTo: dateTo,
        fileType: 'Excel'
    };
    Export(filterData);
}

function ExportPDF() {
    var dateFrom = null
    if (document.getElementById('TxtRecordsFromDate').value !== '') {

        if (!RequiredDateValue('TxtRecordsFromDate')) {
            ok = false;
            document.getElementById('ErrorDateMalformedFrom').style.display = '';
        }
        else {
            dateFrom = GetDate($('#TxtRecordsFromDate').val(), '-');
        }
    }

    var dateTo = null;
    if (document.getElementById('TxtRecordsToDate').value !== '') {
        if (!RequiredDateValue('TxtRecordsToDate')) {
            ok = false;
            document.getElementById('ErrorDateMalformedTo').style.display = '';
        }
        else {
            dateTo = GetDate($('#TxtRecordsToDate').val(), '-');
        }
    }
    var filterData =
    {
        equipmentId: Equipment.Id,
        companyId: Company.Id,
        calibrationInternal: document.getElementById('CalInt').checked,
        calibrationExternal: document.getElementById('CalExt').checked,
        verificationInternal: document.getElementById('VerInt').checked,
        verificationExternal: document.getElementById('VerExt').checked,
        maintenanceInternal: document.getElementById('ManInt').checked,
        maintenanceExternal: document.getElementById('ManExt').checked,
        repairInternal: document.getElementById('RepInt').checked,
        repairExternal: document.getElementById('RepExt').checked,
        dateFrom: dateFrom,
        dateTo: dateTo,
        fileType: 'PDF'
    };
    Export(filterData);
}

function Export(data)
{
    console.log("Export", "data.fileType");
    var webMethod = "/Export/EquipmentRecords.aspx/" + data.fileType; 
    LoadingShow(Dictionary.Common_Report_Rendering);
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function (msg) {
            LoadingHide();
            //successInfoUI(msg.d.MessageError, Go, 200);
            var link = document.createElement('a');
            link.id = 'download';
            link.href = msg.d.MessageError;
            link.download = msg.d.MessageError;
            link.target = '_blank';
            document.body.appendChild(link);
            document.body.removeChild(link);
            $('#download').trigger('click');
            window.open(msg.d.MessageError);
            $("#dialogAddAddress").dialog('close');
        },
        error: function (msg) {
            LoadingHide();
            alertUI("error:" + msg.responseText);
        }
    });
}