// --------------------------------
// <copyright file="ObjetivoRecords.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using PDF_Tests;
using DR = System.Drawing;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;

/// <summary>Implements reporting in PDF and Excel for "objetivo" records</summary>
public partial class ExportObjetivoRecords : Page
{
    public static Font CriteriaFont;
    public static Dictionary<string, string> Dictionary;
    public static Font FontAwe;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult Excel(int companyId, DateTime? dateFrom, DateTime? dateTo, string objetivoName, int objetivoId, int indicadorId, string listOrder)
    {
        if (indicadorId > 0)
        {
            var indicador = Indicador.ById(indicadorId, companyId);
            return ExcelIndicador(companyId, dateFrom, dateTo, indicador.Description, indicadorId, objetivoName, listOrder);
        }

        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        var registros = ObjetivoRegistro.GetByObjetivo(objetivoId, companyId).ToList();

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            Dictionary["Item_Objetivo_RecordsReportTitle"],
            objetivoName,
            Constant.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        var sh = (HSSFSheet)wb.CreateSheet(Dictionary["Item_Objetivo_RecordsReportTitle"]);
        var shCriteria = (HSSFSheet)wb.CreateSheet(Dictionary["Common_SearchCriteria"]);

        var moneyCellStyle = wb.CreateCellStyle();
        var hssfDataFormat = wb.CreateDataFormat();
        moneyCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var headerCellStyle = wb.CreateCellStyle();
        var headerFont = wb.CreateFont();
        headerFont.Boldweight = (short)FontBoldWeight.Bold;
        headerCellStyle.SetFont(headerFont);
        headerCellStyle.BorderBottom = BorderStyle.Double;

        var totalCellStyle = wb.CreateCellStyle();
        var totalFont = wb.CreateFont();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalCellStyle.SetFont(headerFont);
        totalCellStyle.BorderTop = BorderStyle.Double;

        var totalValueCellStyle = wb.CreateCellStyle();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalValueCellStyle.SetFont(headerFont);
        totalValueCellStyle.BorderTop = BorderStyle.Double;
        totalValueCellStyle.BorderBottom = BorderStyle.None;
        totalValueCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var titleCellStyle = wb.CreateCellStyle();
        var titleFont = wb.CreateFont();
        titleFont.Boldweight = (short)FontBoldWeight.Bold;
        titleFont.FontHeight = 400;
        titleCellStyle.SetFont(titleFont);

        var decimalFormat = wb.CreateCellStyle();
        decimalFormat.DataFormat = wb.CreateDataFormat().GetFormat("#.00");

        var integerformat = wb.CreateCellStyle();
        integerformat.DataFormat = wb.CreateDataFormat().GetFormat("#0");

        var cra = new CellRangeAddress(0, 1, 0, 4);
        sh.AddMergedRegion(cra);
        if (sh.GetRow(0) == null) { sh.CreateRow(0); }
        sh.GetRow(0).CreateCell(0);
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Dictionary["Item_Indicador_RecordsReportTitle"], objetivoName));
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        var dataFormatCustom = wb.CreateDataFormat();

        // Condiciones del filtro
        if (shCriteria.GetRow(1) == null) { shCriteria.CreateRow(1); }
        if (shCriteria.GetRow(2) == null) { shCriteria.CreateRow(2); }
        if (shCriteria.GetRow(3) == null) { shCriteria.CreateRow(3); }

        if (shCriteria.GetRow(1).GetCell(1) == null) { shCriteria.GetRow(1).CreateCell(1); }
        shCriteria.GetRow(1).GetCell(1).SetCellValue(Dictionary["Item_Indicador_Field_Name"]);
        if (shCriteria.GetRow(1).GetCell(2) == null) { shCriteria.GetRow(1).CreateCell(2); }
        shCriteria.GetRow(1).GetCell(2).SetCellValue(objetivoName);


        if (shCriteria.GetRow(2).GetCell(1) == null) { shCriteria.GetRow(2).CreateCell(1); }
        shCriteria.GetRow(2).GetCell(1).SetCellValue(Dictionary["Common_From"]);
        if (shCriteria.GetRow(2).GetCell(2) == null) { shCriteria.GetRow(2).CreateCell(2); }
        string fromValue = "-";
        if (dateFrom.HasValue)
        {
            fromValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateFrom.Value);
        }
        shCriteria.GetRow(2).GetCell(2).SetCellValue(fromValue);

        if (shCriteria.GetRow(3).GetCell(1) == null) { shCriteria.GetRow(3).CreateCell(1); }
        shCriteria.GetRow(3).GetCell(1).SetCellValue(Dictionary["Common_To"]);
        if (shCriteria.GetRow(3).GetCell(2) == null) { shCriteria.GetRow(3).CreateCell(2); }
        string toValue = "-";
        if (dateTo.HasValue)
        {
            toValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateTo.Value);
        }

        shCriteria.GetRow(3).GetCell(2).SetCellValue(toValue);

        // Crear Cabecera
        var headers = new List<string>() {
            Dictionary["Item_Objetivo_TableRecords_Header_Status"].ToUpperInvariant(),
            Dictionary["Item_Objetivo_TableRecords_Header_Value"].ToUpperInvariant(),
            Dictionary["Item_Objetivo_TableRecords_Header_Date"].ToUpperInvariant(),
            Dictionary["Item_Objetivo_TableRecords_Header_Comments"].ToUpperInvariant(),
            Dictionary["Item_Objetivo_TableRecords_Header_Meta"].ToUpperInvariant(),
            Dictionary["Item_Objetivo_TableRecords_Header_Responsible"].ToUpperInvariant()
        };

        int countColumns = 0;
        foreach (string headerLabel in headers)
        {
            if (sh.GetRow(3) == null) { sh.CreateRow(3); }

            if (sh.GetRow(3).GetCell(countColumns) == null)
            {
                sh.GetRow(3).CreateCell(countColumns);
            }
            sh.GetRow(3).GetCell(countColumns).SetCellValue(headerLabel);
            sh.GetRow(3).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        int countRow = 4;

        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo).ToList();
        }

        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH1|ASC":
                registros = registros.OrderBy(d => d.Value).ToList();
                break;
            case "TH1|DESC":
                registros = registros.OrderByDescending(d => d.Value).ToList();
                break;
            case "TH2|ASC":
                registros = registros.OrderBy(d => d.Date).ToList();
                break;
            case "TH2|DESC":
                registros = registros.OrderByDescending(d => d.Date).ToList();
                break;
            case "TH3|ASC":
                registros = registros.OrderBy(d => d.Id).ToList();
                break;
            case "TH3|DESC":
                registros = registros.OrderByDescending(d => d.Id).ToList();
                break;
            case "TH4|ASC":
                registros = registros.OrderBy(d => d.Meta).ToList();
                break;
            case "TH4|DESC":
                registros = registros.OrderByDescending(d => d.Meta).ToList();
                break;
            case "TH5|ASC":
                registros = registros.OrderBy(d => d.Responsible.FullName).ToList();
                break;
            case "TH5|DESC":
                registros = registros.OrderByDescending(d => d.Responsible.FullName).ToList();
                break;
        }

        int cont = 0;
        var dataPoints = new List<PointData>();
        foreach (var registroObjetivo in registros)
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }
            string metaText = IndicadorRegistro.ComparerLabel(registroObjetivo.MetaComparer, Dictionary); string statusLabel = Dictionary["Item_Objetivo_StatusLabelWithoutMeta"];
            if (metaText == "eq" && registroObjetivo.Value == registroObjetivo.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "gt" && registroObjetivo.Value > registroObjetivo.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "eqgt" && registroObjetivo.Value >= registroObjetivo.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "lt" && registroObjetivo.Value < registroObjetivo.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "eqlt" && registroObjetivo.Value <= registroObjetivo.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else
            {
                statusLabel = Dictionary["Item_Objetivo_StatusLabelNoMeta"];
            }

            // Status
            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
            sh.GetRow(countRow).GetCell(0).SetCellValue(statusLabel);

            // Value
            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
            sh.GetRow(countRow).GetCell(1).SetCellType(CellType.Numeric);
            sh.GetRow(countRow).GetCell(1).CellStyle = moneyCellStyle;
            sh.GetRow(countRow).GetCell(1).SetCellValue(Convert.ToDouble(registroObjetivo.Value));

            // Date
            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
            sh.GetRow(countRow).GetCell(2).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
            sh.GetRow(countRow).GetCell(2).SetCellValue(registroObjetivo.Date);

            // Comments
            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
            sh.GetRow(countRow).GetCell(3).SetCellValue(registroObjetivo.Comments);

            // Meta
            if (!registroObjetivo.Meta.HasValue)
            {
                metaText = string.Empty;
            }
            else
            {
                metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, registroObjetivo.Meta.Value);
            }

            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            sh.GetRow(countRow).GetCell(4).SetCellValue(metaText);

            // Alarm
            //string alarmText = IndicadorRegistro.ComparerLabel(r.AlarmaComparer, dictionary);
            //alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Alarma);
            //if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            //sh.GetRow(countRow).GetCell(4).SetCellValue(alarmText);

            // Responsible
            if (sh.GetRow(countRow).GetCell(5) == null) { sh.GetRow(countRow).CreateCell(5); }
            sh.GetRow(countRow).GetCell(5).SetCellValue(registroObjetivo.Responsible.FullName);

            countRow++;
        }

        sh.SetColumnWidth(0, 4000);
        sh.SetColumnWidth(1, 4000);
        sh.SetColumnWidth(2, 4000);
        sh.SetColumnWidth(3, 10000);
        sh.SetColumnWidth(4, 8400);
        sh.SetColumnWidth(5, 8400);

        if (!path.EndsWith("\\"))
        {
            path += "\\Temp\\";
        }
        else
        {
            path += "Temp\\";
        }

        using (var fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create, FileAccess.Write))
        {
            wb.Write(fs);
        }

        res.SetSuccess(string.Format("/Temp/{0}", fileName));
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult ExcelIndicador(int companyId, DateTime? dateFrom, DateTime? dateTo, string indicadorName, int indicadorId, string objetivoName, string listOrder)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        var registros = IndicadorRegistro.ByIndicadorId(indicadorId, companyId).ToList();
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            Dictionary["Item_Objetivo_RecordsReportTitle"],
            indicadorName,
            DateTime.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        var sh = (HSSFSheet)wb.CreateSheet(Dictionary["Item_Indicador_RecordsReportTitle"]);
        var shCriteria = (HSSFSheet)wb.CreateSheet(Dictionary["Common_SearchCriteria"]);

        var moneyCellStyle = wb.CreateCellStyle();
        var hssfDataFormat = wb.CreateDataFormat();
        moneyCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var headerCellStyle = wb.CreateCellStyle();
        var headerFont = wb.CreateFont();
        headerFont.Boldweight = (short)FontBoldWeight.Bold;
        headerCellStyle.SetFont(headerFont);
        headerCellStyle.BorderBottom = BorderStyle.Double;

        var totalCellStyle = wb.CreateCellStyle();
        var totalFont = wb.CreateFont();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalCellStyle.SetFont(headerFont);
        totalCellStyle.BorderTop = BorderStyle.Double;

        var totalValueCellStyle = wb.CreateCellStyle();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalValueCellStyle.SetFont(headerFont);
        totalValueCellStyle.BorderTop = BorderStyle.Double;
        totalValueCellStyle.BorderBottom = BorderStyle.None;
        totalValueCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var titleCellStyle = wb.CreateCellStyle();
        var titleFont = wb.CreateFont();
        titleFont.Boldweight = (short)FontBoldWeight.Bold;
        titleFont.FontHeight = 400;
        titleCellStyle.SetFont(titleFont);

        var decimalFormat = wb.CreateCellStyle();
        decimalFormat.DataFormat = wb.CreateDataFormat().GetFormat("#.00");

        var integerformat = wb.CreateCellStyle();
        integerformat.DataFormat = wb.CreateDataFormat().GetFormat("#0");

        var cra = new CellRangeAddress(0, 1, 0, 4);
        sh.AddMergedRegion(cra);
        if (sh.GetRow(0) == null) { sh.CreateRow(0); }
        sh.GetRow(0).CreateCell(0);
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Dictionary["Item_Objetivo_RecordsReportTitle"], indicadorName));
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        var dataFormatCustom = wb.CreateDataFormat();

        // Condiciones del filtro
        if (shCriteria.GetRow(1) == null) { shCriteria.CreateRow(1); }
        if (shCriteria.GetRow(2) == null) { shCriteria.CreateRow(2); }
        if (shCriteria.GetRow(3) == null) { shCriteria.CreateRow(3); }

        if (shCriteria.GetRow(1).GetCell(1) == null) { shCriteria.GetRow(1).CreateCell(1); }
        shCriteria.GetRow(1).GetCell(1).SetCellValue(Dictionary["Item_Indicador_Field_Name"]);
        if (shCriteria.GetRow(1).GetCell(2) == null) { shCriteria.GetRow(1).CreateCell(2); }
        shCriteria.GetRow(1).GetCell(2).SetCellValue(indicadorName);


        if (shCriteria.GetRow(2).GetCell(1) == null) { shCriteria.GetRow(2).CreateCell(1); }
        shCriteria.GetRow(2).GetCell(1).SetCellValue(Dictionary["Common_From"]);
        if (shCriteria.GetRow(2).GetCell(2) == null) { shCriteria.GetRow(2).CreateCell(2); }
        string fromValue = "-";
        if (dateFrom.HasValue)
        {
            fromValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateFrom.Value);
        }
        shCriteria.GetRow(2).GetCell(2).SetCellValue(fromValue);

        if (shCriteria.GetRow(3).GetCell(1) == null) { shCriteria.GetRow(3).CreateCell(1); }
        shCriteria.GetRow(3).GetCell(1).SetCellValue(Dictionary["Common_To"]);
        if (shCriteria.GetRow(3).GetCell(2) == null) { shCriteria.GetRow(3).CreateCell(2); }
        string toValue = "-";
        if (dateTo.HasValue)
        {
            toValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateTo.Value);
        }

        shCriteria.GetRow(3).GetCell(2).SetCellValue(toValue);

        // Crear Cabecera
        var headers = new List<string>() {
            Dictionary["Item_Indicador_TableRecords_Header_Status"].ToUpperInvariant(),
            Dictionary["Item_Indicador_TableRecords_Header_Value"].ToUpperInvariant(),
            Dictionary["Item_Indicador_TableRecords_Header_Date"].ToUpperInvariant(),
            Dictionary["Item_Indicador_TableRecords_Header_Comments"].ToUpperInvariant(),
            Dictionary["Item_Indicador_TableRecords_Header_Meta"].ToUpperInvariant(),
            Dictionary["Item_Indicador_TableRecords_Header_Alarm"].ToUpperInvariant(),
            Dictionary["Item_Indicador_TableRecords_Header_Responsible"].ToUpperInvariant()
        };

        string warningText = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    Dictionary["Item_Objetivo_Tab_RecordsFromIndicator"],
                    indicadorName);
        sh.CreateRow(3);
        sh.GetRow(3).CreateCell(0);
        sh.GetRow(3).GetCell(0).SetCellValue(warningText);

        int countColumns = 0;
        foreach (string headerLabel in headers)
        {
            if (sh.GetRow(5) == null) { sh.CreateRow(5); }

            if (sh.GetRow(5).GetCell(countColumns) == null)
            {
                sh.GetRow(5).CreateCell(countColumns);
            }
            sh.GetRow(5).GetCell(countColumns).SetCellValue(headerLabel);
            sh.GetRow(5).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        int countRow = 6;
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo).ToList();
        }

        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH1|ASC":
                registros = registros.OrderBy(d => d.Value).ToList();
                break;
            case "TH1|DESC":
                registros = registros.OrderByDescending(d => d.Value).ToList();
                break;
            case "TH2|ASC":
                registros = registros.OrderBy(d => d.Date).ToList();
                break;
            case "TH2|DESC":
                registros = registros.OrderByDescending(d => d.Date).ToList();
                break;
            case "TH3|ASC":
                registros = registros.OrderBy(d => d.Id).ToList();
                break;
            case "TH3|DESC":
                registros = registros.OrderByDescending(d => d.Id).ToList();
                break;
            case "TH4|ASC":
                registros = registros.OrderBy(d => d.Meta).ToList();
                break;
            case "TH4|DESC":
                registros = registros.OrderByDescending(d => d.Meta).ToList();
                break;
            case "TH5|ASC":
                registros = registros.OrderBy(d => d.Responsible.FullName).ToList();
                break;
            case "TH5|DESC":
                registros = registros.OrderByDescending(d => d.Responsible.FullName).ToList();
                break;
        }

        foreach (var registro in registros)
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }
            string metaText = IndicadorRegistro.ComparerLabelSign(registro.MetaComparer, Dictionary);
            string alarmText = IndicadorRegistro.ComparerLabelSign(registro.AlarmaComparer, Dictionary);

            string statusLabel = Dictionary["Item_Objetivo_StatusLabelWithoutMeta"];
            if (metaText == "=" && registro.Value == registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">" && registro.Value > registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">=" && registro.Value >= registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<" && registro.Value < registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<=" && registro.Value <= registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">" && registro.Value > registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">=" && registro.Value >= registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<" && registro.Value < registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<=" && registro.Value <= registro.Meta) { statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (!string.IsNullOrEmpty(alarmText))
            {
                if (alarmText == ">" && registro.Value > registro.Alarma) { statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == ">=" && registro.Value >= registro.Alarma) { statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<" && registro.Value < registro.Alarma) { statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<=" && registro.Value <= registro.Alarma) { statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else
                {
                    statusLabel = Dictionary["Item_Objetivo_StatusLabelNoMeta"];
                }
            }
            else
            {
                statusLabel = Dictionary["Item_Objetivo_StatusLabelNoMeta"];
            }

            // Status
            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
            sh.GetRow(countRow).GetCell(0).SetCellValue(statusLabel);

            // Value
            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
            sh.GetRow(countRow).GetCell(1).SetCellType(CellType.Numeric);
            sh.GetRow(countRow).GetCell(1).CellStyle = moneyCellStyle;
            sh.GetRow(countRow).GetCell(1).SetCellValue(Convert.ToDouble(registro.Value));

            // Date
            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
            sh.GetRow(countRow).GetCell(2).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
            sh.GetRow(countRow).GetCell(2).SetCellValue(registro.Date);

            // Comments
            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
            sh.GetRow(countRow).GetCell(3).SetCellValue(registro.Comments);

            // Meta
            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, registro.Meta);
            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            sh.GetRow(countRow).GetCell(4).SetCellValue(metaText);

            // Alarm
            if (!string.IsNullOrEmpty(alarmText))
            {
                alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, registro.Alarma);
            }
            else
            {
                alarmText = string.Empty;
            }

            if (sh.GetRow(countRow).GetCell(5) == null) { sh.GetRow(countRow).CreateCell(5); }
            sh.GetRow(countRow).GetCell(5).SetCellValue(alarmText);

            // Responsible
            if (sh.GetRow(countRow).GetCell(6) == null) { sh.GetRow(countRow).CreateCell(6); }
            sh.GetRow(countRow).GetCell(6).SetCellValue(registro.Responsible.FullName);

            countRow++;
        }

        sh.SetColumnWidth(0, 4000);
        sh.SetColumnWidth(1, 4000);
        sh.SetColumnWidth(2, 4000);
        sh.SetColumnWidth(3, 10000);
        sh.SetColumnWidth(4, 8400);
        sh.SetColumnWidth(5, 8400);
        sh.SetColumnWidth(6, 8400);

        if (!path.EndsWith("\\"))
        {
            path += "\\Temp\\";
        }
        else
        {
            path += "Temp\\";
        }

        using (var fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create, FileAccess.Write))
        {
            wb.Write(fs);
        }

        res.SetSuccess(string.Format("/Temp/{0}", fileName));
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(int companyId, DateTime? dateFrom, DateTime? dateTo, string objetivoName, int objetivoId, int indicadorId, string listOrder)
    {
        var res = ActionResult.NoAction;

        if (indicadorId > 0)
        {
            var indicador = Indicador.ById(indicadorId, companyId);
            return PDFIndicador(companyId, dateFrom, dateTo, indicador.Description, indicadorId, objetivoName, objetivoId, listOrder);
        }

        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        var registros = ObjetivoRegistro.GetByObjetivo(objetivoId, companyId).ToList();
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            Dictionary["Item_Objetivo_RecordsReportTitle"],
            objetivoName,
            DateTime.Now);

        var pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = companyId,
            CompanyName = company.Name,
            Title = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Dictionary["Item_Objetivo_RecordsReportTitle"].ToUpperInvariant(), objetivoName)
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(220, 220, 220);

        // ------------ FONTS 
        string pathFonts = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        var dataFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\calibri.ttf", pathFonts), BaseFont.CP1250, BaseFont.EMBEDDED);
        var times = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var timesBold = new iTS.Font(dataFont, 10, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var headerFont = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        CriteriaFont = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        // -------------------        


        string periode = string.Empty;

        if (dateFrom.HasValue && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{2} {0:dd/MM/yyyy} {3} {1:dd/MM/yyyy}",
                dateFrom.Value,
                dateTo.Value,
                Dictionary["Common_From"].ToLowerInvariant(),
                Dictionary["Common_To"].ToLowerInvariant());

        }
        else if (dateFrom.HasValue && dateTo == null)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateFrom.Value,
                Dictionary["Common_From"]);
        }
        else if (dateFrom == null && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateTo.Value,
                Dictionary["Common_To"]);
        }

        if (string.IsNullOrEmpty(periode))
        {
            periode = Dictionary["Common_PeriodAll"];
        }

        var criteriatable = new iTSpdf.PdfPTable(2)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 25f, 250f });
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Common_Period"], timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        string typeText = string.Empty;

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        pdfDoc.Add(criteriatable);

        var table = new iTSpdf.PdfPTable(6)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 15f, 10f, 15f, 45f, 20f, 30f });

        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Value"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Date"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Comments"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Meta"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Responsible"]));
        int cont = 0;

        // Aplicar filtro
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo).ToList();
        }
        
        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH1|ASC":
                registros = registros.OrderBy(d => d.Value).ToList();
                break;
            case "TH1|DESC":
                registros = registros.OrderByDescending(d => d.Value).ToList();
                break;
            case "TH2|ASC":
                registros = registros.OrderBy(d => d.Date).ToList();
                break;
            case "TH2|DESC":
                registros = registros.OrderByDescending(d => d.Date).ToList();
                break;
            case "TH3|ASC":
                registros = registros.OrderBy(d => d.Id).ToList();
                break;
            case "TH3|DESC":
                registros = registros.OrderByDescending(d => d.Id).ToList();
                break;
            case "TH4|ASC":
                registros = registros.OrderBy(d => d.Meta).ToList();
                break;
            case "TH4|DESC":
                registros = registros.OrderByDescending(d => d.Meta).ToList();
                break;
            case "TH5|ASC":
                registros = registros.OrderBy(d => d.Responsible.FullName).ToList();
                break;
            case "TH5|DESC":
                registros = registros.OrderByDescending(d => d.Responsible.FullName).ToList();
                break;
        }


        var dataPoints = new List<PointData>();
        foreach (var registro in registros.OrderByDescending(r=>r.Date))
        {
            var color = 0;
            string statusLabel = Dictionary["Item_Objetivo_StatusLabelWithoutMeta"];            
            if (registro.MetaComparer == "eq" && registro.Value == registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (registro.MetaComparer == "gt" && registro.Value > registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (registro.MetaComparer == "eqgt" && registro.Value >= registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (registro.MetaComparer == "lt" && registro.Value < registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (registro.MetaComparer == "eqlt" && registro.Value <= registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else
            {
                statusLabel = Dictionary["Item_Objetivo_StatusLabelNoMeta"];
            }

            table.AddCell(ToolsPdf.DataCell(statusLabel, times));
            table.AddCell(ToolsPdf.DataCellMoney(registro.Value, times));
            table.AddCell(ToolsPdf.DataCell(registro.Date, times));
            table.AddCell(ToolsPdf.DataCell(registro.Comments, times));

            string metaText = IndicadorRegistro.ComparerLabelSign(registro.MetaComparer, Dictionary);
            if (!registro.Meta.HasValue)
            {
                metaText = string.Empty;
            }
            else
            {
                metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, registro.Meta);
            }

            table.AddCell(ToolsPdf.DataCell(metaText, times));
            table.AddCell(ToolsPdf.DataCell(registro.Responsible.FullName, times));

            dataPoints.Add(new PointData
            {
                Value = registro.Value,
                Meta = registro.Meta ?? 0,
                Alarma = 0,
                Date = registro.Date,
                Status = color
            });
            cont++;
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            Dictionary["Common_RegisterCount"],
            cont), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Colspan = 4
        });

        string graphName = string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, "graph.jpg");
        dataPoints = dataPoints.OrderBy(dp => dp.Date).ToList();
        using (var ch = new Chart())
        {
            ch.ChartAreas.Add(new ChartArea("Valor"));
            var series = new Series();
            ch.Width = 800;
            ch.Height = 350;
            ch.Series.Add("Values");
            ch.Series["Values"].ChartType = SeriesChartType.Column;
            ch.Series["Values"].YValueType = ChartValueType.Double;

            ch.Series.Add("Meta");
            ch.Series["Meta"].ChartType = SeriesChartType.Line;
            ch.Series["Meta"].BorderWidth = 3;
            ch.Series["Meta"].BorderColor = DR.Color.Green;
            ch.Series["Meta"].YValueType = ChartValueType.Double;

            ch.Series.Add("Alarma");
            ch.Series["Alarma"].ChartType = SeriesChartType.Line;
            ch.Series["Alarma"].BorderWidth = 3;
            ch.Series["Alarma"].BorderColor = DR.Color.Pink;
            ch.Series["Alarma"].YValueType = ChartValueType.Double;

            foreach (var dataPoint in dataPoints)
            {
                ch.Series["Values"].Points.AddXY(string.Format("{0:dd/MM/yyyy}", dataPoint.Date), dataPoint.Value);
                ch.Series["Meta"].Points.AddY(dataPoint.Meta);
                ch.Series["Alarma"].Points.AddY(dataPoint.Alarma ?? 0);
            }

            ch.Series["Values"].IsValueShownAsLabel = true;
            ch.Series["Values"].Font = new DR.Font("Arial", 8, DR.FontStyle.Bold);

            ch.Series["Values"].ChartArea = "Valor";
            ch.Series["Meta"].ChartArea = "Valor";
            ch.Series["Alarma"].ChartArea = "Valor";

            ch.ChartAreas["Valor"].AxisX.LabelStyle.Font = new DR.Font("Arial", 8);
            ch.ChartAreas["Valor"].AxisX.MajorGrid.Enabled = false;
            ch.ChartAreas["Valor"].AxisX.LabelStyle.Angle = 75;
            ch.ChartAreas["Valor"].AxisY.LabelStyle.Font = new DR.Font("Arial", 10);
            ch.ChartAreas["Valor"].RecalculateAxesScale();

            int cp = 0;
            foreach (DataPoint Point in ch.Series["Values"].Points)
            {
                switch (dataPoints[cp].Status)
                {
                    default:
                    case 0: Point.Color = DR.Color.DarkGray; break;
                    case 1: Point.Color = DR.Color.Green; break;
                    case 2: Point.Color = DR.Color.Red; break;
                    case 3: Point.Color = DR.Color.Orange; break;
                }

                cp++;
            }

            ch.Series.Add(series);
            ch.SaveImage(graphName, ChartImageFormat.Jpeg);
        }

        var tif = Image.GetInstance(graphName);
        tif.ScalePercent(100);

        pdfDoc.Add(table);
        pdfDoc.NewPage();
        pdfDoc.Add(new iTS.Phrase("Grafico", times));
        pdfDoc.Add(tif);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
    
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDFIndicador(int companyId, DateTime? dateFrom, DateTime? dateTo, string indicadorName, int indicadorId, string objetivoName, int objetivoId, string listOrder)
    {
        var res = ActionResult.NoAction;
        var objetivo = Objetivo.ById(objetivoId, companyId);
        var fechaInicio = objetivo.StartDate;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        var registros = IndicadorRegistro.ByIndicadorId(indicadorId, companyId).ToList();

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            Dictionary["Item_Objetivo_RecordsReportTitle"],
            indicadorName,
            DateTime.Now);

        var pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = companyId,
            CompanyName = company.Name,
            Title = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Dictionary["Item_Objetivo_RecordsReportTitle"].ToUpperInvariant(), objetivoName)
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(220, 220, 220);

        // ------------ FONTS 
        string pathFonts = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        var awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var dataFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\calibri.ttf", pathFonts), BaseFont.CP1250, BaseFont.EMBEDDED);
        var times = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var timesBold = new iTS.Font(dataFont, 10, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var headerFont = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        CriteriaFont = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var titleFont = new iTS.Font(dataFont, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var fontAwesomeIcon = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        FontAwe = new Font(fontAwesomeIcon, 10);
        // -------------------        


        string periode = string.Empty;

        if (dateFrom.HasValue && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{2} {0:dd/MM/yyyy} {3} {1:dd/MM/yyyy}",
                dateFrom.Value,
                dateTo.Value,
                Dictionary["Common_From"].ToLowerInvariant(),
                Dictionary["Common_To"].ToLowerInvariant());

        }
        else if (dateFrom.HasValue && dateTo == null)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateFrom.Value,
                Dictionary["Common_From"]);
        }
        else if (dateFrom == null && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateTo.Value,
                Dictionary["Common_To"]);
        }

        if (string.IsNullOrEmpty(periode))
        {
            periode = Dictionary["Common_PeriodAll"];
        }

        periode += listOrder;

        var criteriatable = new iTSpdf.PdfPTable(2)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 25f, 250f });
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Common_Period"], timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        string typeText = string.Empty;

        var criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };
        criteriatable.AddCell(criteria3);

        string warningText = string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1}",
            Dictionary["Item_Objetivo_Tab_RecordsFromIndicator"],
            indicadorName);

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(warningText, times))
        {
            Colspan = 2,
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        pdfDoc.Add(criteriatable);

        var table = new iTSpdf.PdfPTable(7)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 15f, 10f, 10f, 15f, 20f, 20f, 30f });

        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Status"].ToUpperInvariant()));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Value"].ToUpperInvariant()));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Date"].ToUpperInvariant()));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Comments"].ToUpperInvariant()));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Meta"].ToUpperInvariant()));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Alarm"].ToUpperInvariant()));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Indicador_TableRecords_Header_Responsible"].ToUpperInvariant()));        

        int cont = 0;
        // Aplicar filtro
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo).ToList();
        }

        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH1|ASC":
                registros = registros.OrderBy(d => d.Value).ToList();
                break;
            case "TH1|DESC":
                registros = registros.OrderByDescending(d => d.Value).ToList();
                break;
            case "TH2|ASC":
                registros = registros.OrderBy(d => d.Date).ToList();
                break;
            case "TH2|DESC":
                registros = registros.OrderByDescending(d => d.Date).ToList();
                break;
            case "TH3|ASC":
                registros = registros.OrderBy(d => d.Id).ToList();
                break;
            case "TH3|DESC":
                registros = registros.OrderByDescending(d => d.Id).ToList();
                break;
            case "TH4|ASC":
                registros = registros.OrderBy(d => d.Meta).ToList();
                break;
            case "TH4|DESC":
                registros = registros.OrderByDescending(d => d.Meta).ToList();
                break;
            case "TH5|ASC":
                registros = registros.OrderBy(d => d.Responsible.FullName).ToList();
                break;
            case "TH5|DESC":
                registros = registros.OrderByDescending(d => d.Responsible.FullName).ToList();
                break;
        }

        // Aplicar filtro
        registros = registros.Where(r => r.Date >= objetivo.StartDate).ToList();

        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo).ToList();
        }

        
        var dataPoints = new List<PointData>();
        foreach (var registro in registros.OrderByDescending(r=>r.Date))
        {
            cont++;
            string metaText = IndicadorRegistro.ComparerLabelSign(registro.MetaComparer, Dictionary);
            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, registro.Meta);
            string alarmText = IndicadorRegistro.ComparerLabelSign(registro.MetaComparer, Dictionary);
            alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, registro.Meta);
            int color = 0;

            string statusLabel = Dictionary["Item_Objetivo_StatusLabelWithoutMeta"];
            if (metaText == "eq" && registro.Value == registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "gt" && registro.Value > registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "eqgt" && registro.Value >= registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "lt" && registro.Value < registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "eqlt" && registro.Value <= registro.Meta) { color = 1; statusLabel = Dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (!string.IsNullOrEmpty(alarmText))
            {
                if (alarmText == "gt" && registro.Value > registro.Alarma) { color = 2; statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "eqgt" && registro.Value >= registro.Alarma) { color = 2; statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "lt" && registro.Value < registro.Alarma) { color = 2; statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "eqlt" && registro.Value <= registro.Alarma) { color = 2; statusLabel = Dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else
                {
                    statusLabel = Dictionary["Item_Objetivo_StatusLabelNoMeta"];
                    color = 3;
                }
            }
            else
            {
                statusLabel = Dictionary["Item_Objetivo_StatusLabelNoMeta"];
                color = 3;
            }

            table.AddCell(ToolsPdf.DataCell(statusLabel, times));
            table.AddCell(ToolsPdf.DataCellRight(string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", registro.Value), times));
            table.AddCell(ToolsPdf.DataCellCenter(registro.Date, times));
            table.AddCell(ToolsPdf.DataCell(registro.Comments, times));
            table.AddCell(ToolsPdf.DataCell(metaText, times));
            table.AddCell(ToolsPdf.DataCell(alarmText, times));
            table.AddCell(ToolsPdf.DataCell(registro.Responsible.FullName, times));

            dataPoints.Add(new PointData
            {
                Value = registro.Value,
                Meta = registro.Meta,
                Alarma = registro.Alarma,
                Date = registro.Date,
                Status = color
            });
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            Dictionary["Common_RegisterCount"],
            cont), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 3
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Colspan = 4
        });

        string graphName = string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, "graph.jpg");
        dataPoints = dataPoints.OrderBy(dp => dp.Date).ToList();
        using (var ch = new Chart())
        {
            ch.ChartAreas.Add(new ChartArea("Valor"));
            var series = new Series();
            ch.Width = 800;
            ch.Height = 350;
            ch.Series.Add("Values");
            ch.Series["Values"].ChartType = SeriesChartType.Column;
            ch.Series["Values"].YValueType = ChartValueType.Double;

            ch.Series.Add("Meta");
            ch.Series["Meta"].ChartType = SeriesChartType.Line;
            ch.Series["Meta"].BorderWidth = 3;
            ch.Series["Meta"].BorderColor = DR.Color.Green;
            ch.Series["Meta"].YValueType = ChartValueType.Double;

            ch.Series.Add("Alarma");
            ch.Series["Alarma"].ChartType = SeriesChartType.Line;
            ch.Series["Alarma"].BorderWidth = 3;
            ch.Series["Alarma"].BorderColor = DR.Color.Pink;
            ch.Series["Alarma"].YValueType = ChartValueType.Double;

            foreach (var dataPoint in dataPoints)
            {
                ch.Series["Values"].Points.AddXY(string.Format("{0:dd/MM/yyyy}", dataPoint.Date), dataPoint.Value);
                ch.Series["Meta"].Points.AddY(dataPoint.Meta);
                ch.Series["Alarma"].Points.AddY(dataPoint.Alarma ?? 0);
            }

            ch.Series["Values"].IsValueShownAsLabel = true;
            ch.Series["Values"].Font = new DR.Font("Arial", 8, DR.FontStyle.Bold);

            ch.Series["Values"].ChartArea = "Valor";
            ch.Series["Meta"].ChartArea = "Valor";
            ch.Series["Alarma"].ChartArea = "Valor";

            ch.ChartAreas["Valor"].AxisX.LabelStyle.Font = new DR.Font("Arial", 8);
            ch.ChartAreas["Valor"].AxisX.MajorGrid.Enabled = false;
            ch.ChartAreas["Valor"].AxisX.LabelStyle.Angle = 75;
            ch.ChartAreas["Valor"].AxisY.LabelStyle.Font = new DR.Font("Arial", 10);
            ch.ChartAreas["Valor"].RecalculateAxesScale();

            int cp = 0;
            foreach (var Point in ch.Series["Values"].Points)
            {
                switch (dataPoints[cp].Status)
                {
                    default:
                    case 0: Point.Color = DR.Color.DarkGray; break;
                    case 1: Point.Color = DR.Color.Green; break;
                    case 2: Point.Color = DR.Color.Red; break;
                    case 3: Point.Color = DR.Color.Orange; break;
                }

                cp++;
            }

            ch.Series.Add(series);
            ch.SaveImage(graphName, ChartImageFormat.Jpeg);
        }

        var tif = Image.GetInstance(graphName);
        tif.ScalePercent(100);

        pdfDoc.Add(table);
        pdfDoc.NewPage();
        pdfDoc.Add(new iTS.Phrase("Grafico", times));
        pdfDoc.Add(tif);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }

    /// <summary>Data that represents a record in chart</summary>
    private struct PointData
    {
        /// <summary>Gets or sets value of record</summary>
        public decimal Value { get; set; }

        /// <summary>Gets or sets meta vlue assigned to record</summary>
        public decimal Meta { get; set; }

        /// <summary>Gets or sets alarm value assigned to record</summary>
        public decimal? Alarma { get; set; }

        /// <summary>Gets or sets date of record</summary>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets status from result or record</summary>
        public int Status { get; set; }
    }
}