// --------------------------------
// <copyright file="IndicadorRecords.aspx.cs" company="Sbrinna">
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
using DR = System.Drawing;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using PDF_Tests;

/// <summary>Implements reporting in PDF and Excel for "indicador" records</summary>
public partial class ExportIndicadorRecords : Page
{
    public static Dictionary<string, string> dictionary;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult Excel(int companyId, DateTime? dateFrom, DateTime? dateTo, string indicadorName, int indicadorId, string listOrder)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        var registros = IndicadorRegistro.ByIndicadorId(indicadorId, companyId).ToList();

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var formatedDescription = ToolsPdf.NormalizeFileName(indicadorName);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            dictionary["Item_Indicador_RecordsReportTitle"],
            formatedDescription,
            DateTime.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        var sh = (HSSFSheet)wb.CreateSheet(dictionary["Item_Indicador_RecordsReportTitle"]);
        var shCriteria = (HSSFSheet)wb.CreateSheet(dictionary["Common_SearchCriteria"]);

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
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Indicador_RecordsReportTitle"], indicadorName));
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        var dataFormatCustom = wb.CreateDataFormat();

        // Condiciones del filtro
        if (shCriteria.GetRow(1) == null) { shCriteria.CreateRow(1); }
        if (shCriteria.GetRow(2) == null) { shCriteria.CreateRow(2); }
        if (shCriteria.GetRow(3) == null) { shCriteria.CreateRow(3); }

        if (shCriteria.GetRow(1).GetCell(1) == null) { shCriteria.GetRow(1).CreateCell(1); }
        shCriteria.GetRow(1).GetCell(1).SetCellValue(dictionary["Item_Indicador_Field_Name"]);
        if (shCriteria.GetRow(1).GetCell(2) == null) { shCriteria.GetRow(1).CreateCell(2); }
        shCriteria.GetRow(1).GetCell(2).SetCellValue(indicadorName);


        if (shCriteria.GetRow(2).GetCell(1) == null) { shCriteria.GetRow(2).CreateCell(1); }
        shCriteria.GetRow(2).GetCell(1).SetCellValue(dictionary["Common_From"]);
        if (shCriteria.GetRow(2).GetCell(2) == null) { shCriteria.GetRow(2).CreateCell(2); }
        string fromValue = "-";
        if (dateFrom.HasValue)
        {
            fromValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateFrom.Value);
        }
        shCriteria.GetRow(2).GetCell(2).SetCellValue(fromValue);

        if (shCriteria.GetRow(3).GetCell(1) == null) { shCriteria.GetRow(3).CreateCell(1); }
        shCriteria.GetRow(3).GetCell(1).SetCellValue(dictionary["Common_To"]);
        if (shCriteria.GetRow(3).GetCell(2) == null) { shCriteria.GetRow(3).CreateCell(2); }
        string toValue = "-";
        if (dateTo.HasValue)
        {
            toValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateTo.Value);
        }

        shCriteria.GetRow(3).GetCell(2).SetCellValue(toValue);

        // Crear Cabecera
        var headers = new List<string>() { 
            dictionary["Item_Indicador_TableRecords_Header_Status"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Value"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Date"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Comments"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Meta"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Alarm"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Responsible"].ToUpperInvariant()
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
            case "TH4|ASC":
                registros = registros.OrderBy(d => d.Meta).ToList();
                break;
            case "TH4|DESC":
                registros = registros.OrderByDescending(d => d.Meta).ToList();
                break;
            case "TH5|ASC":
                registros = registros.OrderBy(d => d.Alarma).ToList();
                break;
            case "TH5|DESC":
                registros = registros.OrderByDescending(d => d.Alarma).ToList();
                break;
            case "TH6|ASC":
                registros = registros.OrderBy(d => d.Responsible.FullName).ToList();
                break;
            case "TH6|DESC":
                registros = registros.OrderByDescending(d => d.Responsible.FullName).ToList();
                break;
        }

        foreach (var registro in registros)
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }
            string metaText = IndicadorRegistro.ComparerLabelSign(registro.MetaComparer, dictionary);
            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, registro.Meta);
            string alarmText = IndicadorRegistro.ComparerLabelSign(registro.AlarmaComparer, dictionary);

            string statusLabel = dictionary["Item_Objetivo_StatusLabelWithoutMeta"];
            if (metaText == "=" && registro.Value == registro.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">" && registro.Value > registro.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">=" && registro.Value >= registro.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<" && registro.Value < registro.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<=" && registro.Value <= registro.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (!string.IsNullOrEmpty(alarmText))
            {
                if (alarmText == ">" && registro.Value > registro.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == ">=" && registro.Value >= registro.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<" && registro.Value < registro.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<=" && registro.Value <= registro.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else
                {
                    statusLabel = dictionary["Item_Objetivo_StatusLabelNoMeta"];
                }
            }
            else
            {
                statusLabel = dictionary["Item_Objetivo_StatusLabelNoMeta"];
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
            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            sh.GetRow(countRow).GetCell(4).SetCellValue(metaText);

            // Alarm
            if (!registro.Alarma.HasValue)
            {
                alarmText = string.Empty;
            }
            else
            {
                alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, registro.Alarma);
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
    public static ActionResult PDF(int companyId, DateTime? dateFrom, DateTime? dateTo, string indicadorName, int indicadorId, string listOrder)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
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
            dictionary["Item_Indicador_RecordsReportTitle"],
            indicadorName,
            DateTime.Now);

        var pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = companyId,
            CompanyName = company.Name,
            Title = string.Format(CultureInfo.InvariantCulture, "{0} {1}", dictionary["Item_Indicador_RecordsReportTitle"].ToUpperInvariant(), indicadorName)
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(220, 220, 220);    

        string periode = string.Empty;

        if (dateFrom.HasValue && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{2} {0:dd/MM/yyyy} {3} {1:dd/MM/yyyy}",
                dateFrom.Value,
                dateTo.Value,
                dictionary["Common_From"].ToLowerInvariant(),
                dictionary["Common_To"].ToLowerInvariant());
        }
        else if (dateFrom.HasValue && dateTo == null)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateFrom.Value,
                dictionary["Common_From"]);
        }
        else if (dateFrom == null && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateTo.Value,
                dictionary["Common_To"]);
        }

        if (string.IsNullOrEmpty(periode))
        {
            periode = dictionary["Common_PeriodAll"];
        }
        
        var criteriatable = new iTSpdf.PdfPTable(2)
        {
            WidthPercentage = 100
        };

        string typeText = string.Empty;
        criteriatable.SetWidths(new float[] { 25f, 250f });
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(dictionary["Common_Period"]));
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(periode));
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(typeText));
        pdfDoc.Add(criteriatable);

        var table = new iTSpdf.PdfPTable(7)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 15f, 10f, 15f, 15f, 20f, 20f, 30f });
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Value"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Date"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Comments"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Meta"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Alarm"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Responsible"]));

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
            case "TH4|ASC":
                registros = registros.OrderBy(d => d.Meta).ToList();
                break;
            case "TH4|DESC":
                registros = registros.OrderByDescending(d => d.Meta).ToList();
                break;
            case "TH5|ASC":
                registros = registros.OrderBy(d => d.Alarma).ToList();
                break;
            case "TH5|DESC":
                registros = registros.OrderByDescending(d => d.Alarma).ToList();
                break;
            case "TH6|ASC":
                registros = registros.OrderBy(d => d.Responsible.FullName).ToList();
                break;
            case "TH6|DESC":
                registros = registros.OrderByDescending(d => d.Responsible.FullName).ToList();
                break;
        }

        int cont = 0;
        var dataPoints = new List<PointData>();
        foreach (var registro in registros.OrderByDescending(r=>r.Date))
        {
            cont++;
            string metaText = IndicadorRegistro.ComparerLabelSign(registro.MetaComparer, dictionary);
            string alarmText = IndicadorRegistro.ComparerLabelSign(registro.AlarmaComparer, dictionary);
            int color = 0;

            string statusLabel = dictionary["Item_Objetivo_StatusLabelWithoutMeta"];
            if (metaText == "=" && registro.Value == registro.Meta) { color = 1; statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">" && registro.Value > registro.Meta) { color = 1; statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">=" && registro.Value >= registro.Meta) { color = 1; statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<" && registro.Value < registro.Meta) { color = 1; statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<=" && registro.Value <= registro.Meta) { color = 1; statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (!string.IsNullOrEmpty(alarmText))
            {
                if (alarmText == "=" && registro.Value == registro.Alarma) { color = 2; statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == ">" && registro.Value > registro.Alarma) { color = 2; statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == ">=" && registro.Value >= registro.Alarma) { color = 2; statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<" && registro.Value < registro.Alarma) { color = 2; statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<=" && registro.Value <= registro.Alarma) { color = 2; statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else
                {
                    statusLabel = dictionary["Item_Objetivo_StatusLabelNoMeta"];
                    color = 3;
                }
            }
            else
            {
                statusLabel = dictionary["Item_Objetivo_StatusLabelNoMeta"];
                color = 3;
            }

            table.AddCell(ToolsPdf.DataCell(statusLabel, ToolsPdf.LayoutFonts.Times));

            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, registro.Meta);
            alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, registro.Alarma);
            table.AddCell(ToolsPdf.DataCellRight(string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", registro.Value)));
            table.AddCell(ToolsPdf.DataCell(registro.Date));
            table.AddCell(ToolsPdf.DataCell(registro.Comments));
            table.AddCell(ToolsPdf.DataCell(metaText));
            table.AddCell(ToolsPdf.DataCell(alarmText));
            table.AddCell(ToolsPdf.DataCell(registro.Responsible.FullName));

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
            dictionary["Common_RegisterCount"],
            cont), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Colspan = 5
        });

        string graphName = string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, "graph.jpg");
        dataPoints = dataPoints.OrderBy(dp => dp.Date).ToList();
        using (var chart = new Chart())
        {
            chart.ChartAreas.Add(new ChartArea("Valor"));
            var series = new Series();
            chart.Width = 800;
            chart.Height = 350;
            chart.Series.Add("Values");
            chart.Series["Values"].ChartType = SeriesChartType.Column;
            chart.Series["Values"].YValueType = ChartValueType.Double;

            chart.Series.Add("Meta");
            chart.Series["Meta"].ChartType = SeriesChartType.Line;
            chart.Series["Meta"].BorderWidth = 3;
            chart.Series["Meta"].BorderColor = DR.Color.Green;
            chart.Series["Meta"].YValueType = ChartValueType.Double;

            chart.Series.Add("Alarma");
            chart.Series["Alarma"].ChartType = SeriesChartType.Line;
            chart.Series["Alarma"].BorderWidth = 3;
            chart.Series["Alarma"].BorderColor = DR.Color.Pink;
            chart.Series["Alarma"].YValueType = ChartValueType.Double;
            
            foreach (var dataPoint in dataPoints)
            {
                chart.Series["Values"].Points.AddXY(string.Format("{0:dd/MM/yyyy}", dataPoint.Date), dataPoint.Value);
                chart.Series["Meta"].Points.AddY(dataPoint.Meta);
                chart.Series["Alarma"].Points.AddY(dataPoint.Alarma ?? 0);
            }

            chart.Series["Values"].IsValueShownAsLabel = true;
            chart.Series["Values"].Font = new DR.Font("Arial", 8, DR.FontStyle.Bold);

            chart.Series["Values"].ChartArea = "Valor";
            chart.Series["Meta"].ChartArea = "Valor";
            chart.Series["Alarma"].ChartArea = "Valor";
            
            chart.ChartAreas["Valor"].AxisX.LabelStyle.Font = new DR.Font("Arial", 8);
            chart.ChartAreas["Valor"].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas["Valor"].AxisX.LabelStyle.Angle = 75;
            chart.ChartAreas["Valor"].AxisY.LabelStyle.Font = new DR.Font("Arial", 10);
            chart.ChartAreas["Valor"].RecalculateAxesScale();

            int cp = 0;
            foreach (var Point in chart.Series["Values"].Points)
            {
                switch(dataPoints[cp].Status)
                {
                    default:
                    case 0: Point.Color = DR.Color.DarkGray; break;
                    case 1: Point.Color = DR.Color.Green; break;
                    case 2: Point.Color = DR.Color.Red; break;
                    case 3: Point.Color = DR.Color.Orange; break;
                }

                cp++;
            }

            chart.Series.Add(series);
            chart.SaveImage(graphName, ChartImageFormat.Jpeg);
        }

        var tif = Image.GetInstance(graphName);
        tif.ScalePercent(100);

        pdfDoc.Add(table);
        pdfDoc.NewPage();
        pdfDoc.Add(new iTS.Phrase("Grafico", ToolsPdf.LayoutFonts.Times));
        pdfDoc.Add(tif);

        var logoIssus = Image.GetInstance(string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path));
        logoIssus.ScalePercent(20f);
        logoIssus.SetAbsolutePosition(40f, 24f);
        pdfDoc.Add(logoIssus);

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