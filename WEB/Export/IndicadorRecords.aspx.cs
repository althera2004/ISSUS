// --------------------------------
// <copyright file="IndicadorRecords.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

/// <summary>
/// Implements reporting in PDF and Excel for "indicador" records
/// </summary>
public partial class Export_IndicadorRecords : Page
{
    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;
    public static Font fontAwe;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult Excel(int companyId, DateTime? dateFrom, DateTime? dateTo, string indicadorName, int indicadorId, string listOrder)
    {
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Company company = new Company(companyId);

        List<IndicadorRegistro> registros = IndicadorRegistro.GetByIndicador(indicadorId, companyId).ToList();

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            dictionary["Item_Indicador_RecordsReportTitle"],
            indicadorName,
            DateTime.Now);

        HSSFWorkbook wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        HSSFSheet sh = (HSSFSheet)wb.CreateSheet(dictionary["Item_Indicador_RecordsReportTitle"]);
        HSSFSheet shCriteria = (HSSFSheet)wb.CreateSheet(dictionary["Common_SearchCriteria"]);

        ICellStyle moneyCellStyle = wb.CreateCellStyle();
        IDataFormat hssfDataFormat = wb.CreateDataFormat();
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

        ICellStyle decimalFormat = wb.CreateCellStyle();
        decimalFormat.DataFormat = wb.CreateDataFormat().GetFormat("#.00");

        ICellStyle integerformat = wb.CreateCellStyle();
        integerformat.DataFormat = wb.CreateDataFormat().GetFormat("#0");

        CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 4);
        sh.AddMergedRegion(cra);
        if (sh.GetRow(0) == null) { sh.CreateRow(0); }
        sh.GetRow(0).CreateCell(0);
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Indicador_RecordsReportTitle"], indicadorName));
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        IDataFormat dataFormatCustom = wb.CreateDataFormat();

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
        List<string> headers = new List<string>() { 
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
            sh.GetRow(3).GetCell(countColumns).SetCellValue(headerLabel.ToString());
            sh.GetRow(3).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        int countRow = 4;
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom.Value).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo.Value).ToList();
        }

        switch (listOrder.ToUpperInvariant())
        {
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

        foreach (IndicadorRegistro r in registros)
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }
            string metaText = IndicadorRegistro.ComparerLabelSign(r.MetaComparer, dictionary);
            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Meta);
            string alarmText = IndicadorRegistro.ComparerLabelSign(r.AlarmaComparer, dictionary);

            string statusLabel = dictionary["Item_Objetivo_StatusLabelWithoutMeta"];
            if (metaText == "=" && r.Value == r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">" && r.Value > r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">=" && r.Value >= r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<" && r.Value < r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<=" && r.Value <= r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (!string.IsNullOrEmpty(alarmText))
            {
                if (alarmText == ">" && r.Value > r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == ">=" && r.Value >= r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<" && r.Value < r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<=" && r.Value <= r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
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
            sh.GetRow(countRow).GetCell(1).SetCellValue(Convert.ToDouble(r.Value));

            // Date
            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
            sh.GetRow(countRow).GetCell(2).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
            sh.GetRow(countRow).GetCell(2).SetCellValue(r.Date);

            // Comments
            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
            sh.GetRow(countRow).GetCell(3).SetCellValue(r.Comments);

            // Meta
            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            sh.GetRow(countRow).GetCell(4).SetCellValue(metaText);

            // Alarm
            if (!r.Alarma.HasValue)
            {
                alarmText = string.Empty;
            }
            else
            {
                alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, r.Alarma);
            }

            if (sh.GetRow(countRow).GetCell(5) == null) { sh.GetRow(countRow).CreateCell(5); }
            sh.GetRow(countRow).GetCell(5).SetCellValue(alarmText);

            // Responsible
            if (sh.GetRow(countRow).GetCell(6) == null) { sh.GetRow(countRow).CreateCell(6); }
            sh.GetRow(countRow).GetCell(6).SetCellValue(r.Responsible.FullName);

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
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Company company = new Company(companyId);

        List<IndicadorRegistro> registros = IndicadorRegistro.GetByIndicador(indicadorId, companyId).ToList();

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

        iTS.Document pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        iTSpdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
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
            Title = string.Format(CultureInfo.InvariantCulture, "{0} {1}", dictionary["Item_Indicador_RecordsReportTitle"].ToUpperInvariant(), indicadorName)
        };

        pdfDoc.Open();

        iTS.BaseColor backgroundColor = new iTS.BaseColor(220, 220, 220);

        // ------------ FONTS 
        string pathFonts = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        iTSpdf.BaseFont awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        iTSpdf.BaseFont dataFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\calibri.ttf", pathFonts), BaseFont.CP1250, BaseFont.EMBEDDED);
        iTS.Font times = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font timesBold = new iTS.Font(dataFont, 10, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font headerFont = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        criteriaFont = new iTS.Font(dataFont, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font titleFont = new iTS.Font(dataFont, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

        var fontAwesomeIcon = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        fontAwe = new Font(fontAwesomeIcon, 10);
        // -------------------        


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
        else if (dateFrom.HasValue && !dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateFrom.Value,
                dictionary["Common_From"]);
        }
        else if (!dateFrom.HasValue && dateTo.HasValue)
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

        var borderNone = iTS.Rectangle.NO_BORDER;
        
        float[] cirteriaWidths = new float[] { 25f, 250f };
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(2)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(cirteriaWidths);
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };
        criteriatable.AddCell(criteria2);

        string typeText = string.Empty;
        iTSpdf.PdfPCell criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };
        criteriatable.AddCell(criteria3);

        pdfDoc.Add(criteriatable);

        float[] widths = new float[] { 15f, 10f, 15f, 15f, 20f, 20f, 30f };
        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(7)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(widths);
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Status"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Value"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Date"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Comments"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Meta"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Alarm"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Responsible"].ToUpperInvariant(), times));

        // Aplicar filtro
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom.Value).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo.Value).ToList();
        }

        switch (listOrder.ToUpperInvariant())
        {
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
        foreach (IndicadorRegistro r in registros)
        {
            cont++;
            string metaText = IndicadorRegistro.ComparerLabelSign(r.MetaComparer, dictionary);
            string alarmText = IndicadorRegistro.ComparerLabelSign(r.AlarmaComparer, dictionary);

            string statusLabel = dictionary["Item_Objetivo_StatusLabelWithoutMeta"];
            if (metaText == "=" && r.Value == r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">" && r.Value > r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == ">=" && r.Value >= r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<" && r.Value < r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (metaText == "<=" && r.Value <= r.Meta) { statusLabel = dictionary["Item_Objetivo_StatusLabelMeta"]; }
            else if (!string.IsNullOrEmpty(alarmText))
            {
                if (alarmText == "=" && r.Value == r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == ">" && r.Value > r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == ">=" && r.Value >= r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<" && r.Value < r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else if (alarmText == "<=" && r.Value <= r.Alarma) { statusLabel = dictionary["Item_Objetivo_StatusLabelWarning"]; }
                else
                {
                    statusLabel = dictionary["Item_Objetivo_StatusLabelNoMeta"];
                }
            }
            else
            {
                statusLabel = dictionary["Item_Objetivo_StatusLabelNoMeta"];
            }

            table.AddCell(ToolsPdf.DataCell(statusLabel, times));

            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Meta);
            alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, r.Alarma);
            table.AddCell(ToolsPdf.DataCellRight(string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", r.Value), times));
            table.AddCell(ToolsPdf.DataCell(r.Date, times));
            table.AddCell(ToolsPdf.DataCell(r.Comments, times));
            table.AddCell(ToolsPdf.DataCell(metaText, times));
            table.AddCell(ToolsPdf.DataCell(alarmText, times));
            table.AddCell(ToolsPdf.DataCell(r.Responsible.FullName, times));
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
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
            Colspan = 5
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}