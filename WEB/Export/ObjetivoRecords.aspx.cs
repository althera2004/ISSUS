// --------------------------------
// <copyright file="ObjetivoRecords.aspx.cs" company="Sbrinna">
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
/// Implements reporting in PDF and Excel for "objetivo" records
/// </summary>
public partial class Export_ObjetivoRecords : Page
{
    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;
    public static Font fontAwe;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult Excel(int companyId, DateTime? dateFrom, DateTime? dateTo, string objetivoName, int objetivoId, int indicadorId)
    {
        if (indicadorId > 0)
        {
            Indicador indicador = Indicador.GetById(indicadorId, companyId);
            return ExcelIndicador(companyId, dateFrom, dateTo, indicador.Description, indicadorId, objetivoName);
        }

        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Company company = new Company(companyId);

        List<ObjetivoRegistro> registros = ObjetivoRegistro.GetByObjetivo(objetivoId, companyId).ToList();

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            dictionary["Item_Objetivo_RecordsReportTitle"],
            objetivoName,
            DateTime.Now);

        HSSFWorkbook wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        HSSFSheet sh = (HSSFSheet)wb.CreateSheet(dictionary["Item_Objetivo_RecordsReportTitle"]);
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
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Indicador_RecordsReportTitle"], objetivoName));
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;


        IDataFormat dataFormatCustom = wb.CreateDataFormat();

        // Condiciones del filtro
        if (shCriteria.GetRow(1) == null) { shCriteria.CreateRow(1); }
        if (shCriteria.GetRow(2) == null) { shCriteria.CreateRow(2); }
        if (shCriteria.GetRow(3) == null) { shCriteria.CreateRow(3); }

        if (shCriteria.GetRow(1).GetCell(1) == null) { shCriteria.GetRow(1).CreateCell(1); }
        shCriteria.GetRow(1).GetCell(1).SetCellValue(dictionary["Item_Indicador_Field_Name"]);
        if (shCriteria.GetRow(1).GetCell(2) == null) { shCriteria.GetRow(1).CreateCell(2); }
        shCriteria.GetRow(1).GetCell(2).SetCellValue(objetivoName);


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
            dictionary["Item_Objetivo_TableRecords_Header_Value"].ToUpperInvariant(),
            dictionary["Item_Objetivo_TableRecords_Header_Date"].ToUpperInvariant(),
            dictionary["Item_Objetivo_TableRecords_Header_Comments"].ToUpperInvariant(),
            dictionary["Item_Objetivo_TableRecords_Header_Meta"].ToUpperInvariant(),
            //dictionary["Item_Objetivo_TableRecords_Header_Alarm"].ToUpperInvariant(),
            dictionary["Item_Objetivo_TableRecords_Header_Responsible"].ToUpperInvariant()
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

        foreach (ObjetivoRegistro r in registros.OrderByDescending(r => r.Date))
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }

            // Value
            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
            sh.GetRow(countRow).GetCell(0).SetCellType(CellType.Numeric);
            sh.GetRow(countRow).GetCell(0).CellStyle = moneyCellStyle;
            sh.GetRow(countRow).GetCell(0).SetCellValue(Convert.ToDouble(r.Value));

            // Date
            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
            sh.GetRow(countRow).GetCell(1).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
            sh.GetRow(countRow).GetCell(1).SetCellValue(r.Date);

            // Comments
            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
            sh.GetRow(countRow).GetCell(2).SetCellValue(r.Comments);

            // Meta
            string metaText = IndicadorRegistro.ComparerLabel(r.MetaComparer, dictionary);
            if (!r.Meta.HasValue)
            {
                metaText = string.Empty;
            }
            else
            {
                metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Meta.Value);
            }

            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
            sh.GetRow(countRow).GetCell(3).SetCellValue(metaText);

            // Alarm
            //string alarmText = IndicadorRegistro.ComparerLabel(r.AlarmaComparer, dictionary);
            //alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Alarma);
            //if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            //sh.GetRow(countRow).GetCell(4).SetCellValue(alarmText);

            // Responsible
            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            sh.GetRow(countRow).GetCell(4).SetCellValue(r.Responsible.FullName);

            countRow++;
        }


        //if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }
        //if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
        //if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
        //if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
        //if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }

        //sh.GetRow(countRow).GetCell(0).SetCellValue(string.Empty);
        //sh.GetRow(countRow).GetCell(0).CellStyle = totalCellStyle;
        //sh.GetRow(countRow).GetCell(1).SetCellValue(string.Empty);
        //sh.GetRow(countRow).GetCell(1).CellStyle = totalCellStyle;
        //sh.GetRow(countRow).GetCell(2).SetCellValue(string.Empty);
        //sh.GetRow(countRow).GetCell(2).CellStyle = totalCellStyle;
        //sh.GetRow(countRow).GetCell(3).SetCellValue(dictionary["Common_Total"]);
        //sh.GetRow(countRow).GetCell(3).CellStyle = totalCellStyle;


        //if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
        //sh.GetRow(countRow).GetCell(4).SetCellValue(Convert.ToDouble(total));
        //sh.GetRow(countRow).GetCell(4).SetCellType(CellType.Numeric);
        //sh.GetRow(countRow).GetCell(4).CellStyle = totalValueCellStyle;

        sh.SetColumnWidth(0, 4000);
        sh.SetColumnWidth(1, 4000);
        sh.SetColumnWidth(2, 10000);
        sh.SetColumnWidth(3, 8400);
        //sh.SetColumnWidth(4, 8400);
        sh.SetColumnWidth(4, 8400);

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
    public static ActionResult ExcelIndicador(int companyId, DateTime? dateFrom, DateTime? dateTo, string indicadorName, int indicadorId, string objetivoName)
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
            dictionary["Item_Objetivo_RecordsReportTitle"],
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
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Objetivo_RecordsReportTitle"], indicadorName));
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
            dictionary["Item_Indicador_TableRecords_Header_Value"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Date"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Comments"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Meta"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Alarm"].ToUpperInvariant(),
            dictionary["Item_Indicador_TableRecords_Header_Responsible"].ToUpperInvariant()
        };

        string warningText = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    dictionary["Item_Objetivo_Tab_RecordsFromIndicator"],
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
            sh.GetRow(5).GetCell(countColumns).SetCellValue(headerLabel.ToString());
            sh.GetRow(5).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        int countRow = 6;
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom.Value).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo.Value).ToList();
        }

        foreach (IndicadorRegistro r in registros.OrderByDescending(r => r.Date))
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }

            // Value
            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
            sh.GetRow(countRow).GetCell(0).SetCellType(CellType.Numeric);
            sh.GetRow(countRow).GetCell(0).CellStyle = moneyCellStyle;
            sh.GetRow(countRow).GetCell(0).SetCellValue(Convert.ToDouble(r.Value));

            // Date
            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
            sh.GetRow(countRow).GetCell(1).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
            sh.GetRow(countRow).GetCell(1).SetCellValue(r.Date);

            // Comments
            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
            sh.GetRow(countRow).GetCell(2).SetCellValue(r.Comments);

            // Meta
            string metaText = IndicadorRegistro.ComparerLabel(r.MetaComparer, dictionary);
            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Meta);
            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
            sh.GetRow(countRow).GetCell(3).SetCellValue(metaText);

            // Alarm
            string alarmText = IndicadorRegistro.ComparerLabel(r.AlarmaComparer, dictionary);
            if (!string.IsNullOrEmpty(alarmText))
            {
                alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, r.Alarma);
            }
            else
            {
                alarmText = string.Empty;
            }

            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            sh.GetRow(countRow).GetCell(4).SetCellValue(alarmText);

            // Responsible
            if (sh.GetRow(countRow).GetCell(5) == null) { sh.GetRow(countRow).CreateCell(5); }
            sh.GetRow(countRow).GetCell(5).SetCellValue(r.Responsible.FullName);

            countRow++;
        }

        sh.SetColumnWidth(0, 4000);
        sh.SetColumnWidth(1, 4000);
        sh.SetColumnWidth(2, 10000);
        sh.SetColumnWidth(3, 8400);
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
    public static ActionResult PDF(int companyId, DateTime? dateFrom, DateTime? dateTo, string objetivoName, int objetivoId, int indicadorId)
    {
        ActionResult res = ActionResult.NoAction;

        if (indicadorId > 0)
        {
            Indicador indicador = Indicador.GetById(indicadorId, companyId);
            return PDFIndicador(companyId, dateFrom, dateTo, indicador.Description, indicadorId, objetivoName, objetivoId);
        }


        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Company company = new Company(companyId);

        List<ObjetivoRegistro> registros = ObjetivoRegistro.GetByObjetivo(objetivoId, companyId).ToList();

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Objetivo_RecordsReportTitle"],
            objetivoName,
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
            Title = string.Format(CultureInfo.InvariantCulture, "{0} {1}", dictionary["Item_Objetivo_RecordsReportTitle"].ToUpperInvariant(), objetivoName)
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
        var borderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;

        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(2);
        float[] cirteriaWidths = new float[] { 25f, 250f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;

        iTSpdf.PdfPCell criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold));
        criteria2Label.Border = borderNone;
        criteria2Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2Label.Padding = 6f;
        criteria2Label.PaddingTop = 4f;
        criteriatable.AddCell(criteria2Label);

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times));
        criteria2.Border = borderNone;
        criteria2.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2.Padding = 6f;
        criteria2.PaddingTop = 4f;
        criteriatable.AddCell(criteria2);

        string typeText = string.Empty;

        iTSpdf.PdfPCell criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times));
        criteria3.Border = borderNone;
        criteria3.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria3.Padding = 6f;
        criteria3.PaddingTop = 4f;
        criteriatable.AddCell(criteria3);

        pdfDoc.Add(criteriatable);


        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(5);

        // actual width of table in points
        table.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 10f, 10f, 45f, 20f, 30f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 0;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerValue = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Value"].ToUpperInvariant(), headerFont));
        headerValue.Border = borderAll;
        headerValue.BackgroundColor = backgroundColor;
        headerValue.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerValue.Padding = 8f;
        headerValue.PaddingTop = 6f;

        iTSpdf.PdfPCell headerDate = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Date"].ToUpperInvariant(), headerFont));
        headerDate.Border = borderAll;
        headerDate.BackgroundColor = backgroundColor;
        headerDate.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerDate.Padding = 8f;
        headerDate.PaddingTop = 6f;

        iTSpdf.PdfPCell headerComments = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Comments"].ToUpperInvariant(), headerFont));
        headerComments.Border = borderAll;
        headerComments.BackgroundColor = backgroundColor;
        headerComments.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerComments.Padding = 8f;
        headerComments.PaddingTop = 6f;

        iTSpdf.PdfPCell headerMeta = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Meta"].ToUpperInvariant(), headerFont));
        headerMeta.Border = borderAll;
        headerMeta.BackgroundColor = backgroundColor;
        headerMeta.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerMeta.Padding = 8f;
        headerMeta.PaddingTop = 6f;

        /*iTSpdf.PdfPCell headerAlarm = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Alarm"].ToUpperInvariant(), headerFont));
        headerAlarm.Border = borderAll;
        headerAlarm.BackgroundColor = backgroundColor;
        headerAlarm.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerAlarm.Padding = 8f;
        headerAlarm.PaddingTop = 6f;*/

        iTSpdf.PdfPCell headerResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Responsible"].ToUpperInvariant(), headerFont));
        headerResponsible.Border = borderAll;
        headerResponsible.BackgroundColor = backgroundColor;
        headerResponsible.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerResponsible.Padding = 8f;
        headerResponsible.PaddingTop = 6f;

        table.AddCell(headerValue);
        table.AddCell(headerDate);
        table.AddCell(headerComments);
        table.AddCell(headerMeta);
        //table.AddCell(headerAlarm);
        table.AddCell(headerResponsible);

        decimal totalCost = 0;
        int cont = 0;

        // Aplicar filtro
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom.Value).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo.Value).ToList();
        }

        foreach (ObjetivoRegistro r in registros.OrderByDescending(r => r.Date))
        {
            int border = 0;

            iTSpdf.PdfPCell cellValue = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture,"{0:#,##0.00}", r.Value), times));
            cellValue.Border = border;
            cellValue.HorizontalAlignment = iTS.Element.ALIGN_RIGHT;
            cellValue.Padding = 6f;
            cellValue.PaddingTop = 4f;
            table.AddCell(cellValue);

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", r.Date), times));
            cellDate.Border = border;
            cellDate.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            table.AddCell(cellDate);

            iTSpdf.PdfPCell cellComments = new iTSpdf.PdfPCell(new iTS.Phrase(r.Comments, times));
            cellComments.Border = border;
            cellComments.Padding = 6f;
            cellComments.PaddingTop = 4f;
            table.AddCell(cellComments);

            string metaText = IndicadorRegistro.ComparerLabel(r.MetaComparer, dictionary);
            if (!r.Meta.HasValue)
            {
                metaText = string.Empty;
            }
            else
            {
                metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Meta);
            }

            iTSpdf.PdfPCell cellMeta = new iTSpdf.PdfPCell(new iTS.Phrase(metaText, times));
            cellMeta.Border = border;
            cellMeta.Padding = 6f;
            cellMeta.PaddingTop = 4f;
            table.AddCell(cellMeta);

            /*string alarmText = IndicadorRegistro.ComparerLabel(r.MetaComparer, dictionary);
            alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, r.Meta);
            iTSpdf.PdfPCell cellAlarm = new iTSpdf.PdfPCell(new iTS.Phrase(alarmText, times));
            cellAlarm.Border = border;
            cellAlarm.Padding = 6f;
            cellAlarm.PaddingTop = 4f;
            table.AddCell(cellAlarm);*/

            iTSpdf.PdfPCell cellResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(r.Responsible.FullName, times));
            cellResponsible.Border = border;
            cellResponsible.Padding = 6f;
            cellResponsible.PaddingTop = 4f;
            table.AddCell(cellResponsible);

            cont++;
        }

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
    
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDFIndicador(int companyId, DateTime? dateFrom, DateTime? dateTo, string indicadorName, int indicadorId, string objetivoName, int objetivoId)
    {
        ActionResult res = ActionResult.NoAction;
        Objetivo objetivo = Objetivo.GetById(objetivoId, companyId);
        DateTime fechaInicio = objetivo.StartDate;
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
            dictionary["Item_Objetivo_RecordsReportTitle"],
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
            Title = string.Format(CultureInfo.InvariantCulture, "{0} {1}", dictionary["Item_Objetivo_RecordsReportTitle"].ToUpperInvariant(), objetivoName)
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
        var borderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;

        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(2);
        float[] cirteriaWidths = new float[] { 25f, 250f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;

        iTSpdf.PdfPCell criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold));
        criteria2Label.Border = borderNone;
        criteria2Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2Label.Padding = 6f;
        criteria2Label.PaddingTop = 4f;
        criteriatable.AddCell(criteria2Label);

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times));
        criteria2.Border = borderNone;
        criteria2.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2.Padding = 6f;
        criteria2.PaddingTop = 4f;
        criteriatable.AddCell(criteria2);

        string typeText = string.Empty;

        iTSpdf.PdfPCell criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times));
        criteria3.Border = borderNone;
        criteria3.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria3.Padding = 6f;
        criteria3.PaddingTop = 4f;
        criteriatable.AddCell(criteria3);

        string warningText = string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1}",
            dictionary["Item_Objetivo_Tab_RecordsFromIndicator"],
            indicadorName);
        iTSpdf.PdfPCell criteria4 = new iTSpdf.PdfPCell(new iTS.Phrase(warningText, times));
        criteria4.Colspan = 2;
        criteria4.Border = borderNone;
        criteria4.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria4.Padding = 6f;
        criteria4.PaddingTop = 4f;
        criteriatable.AddCell(criteria4);

        pdfDoc.Add(criteriatable);


        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(6);

        // actual width of table in points
        table.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 10f, 10f, 15f, 20f, 20f, 30f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 0;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerValue = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Value"].ToUpperInvariant(), headerFont));
        headerValue.Border = borderAll;
        headerValue.BackgroundColor = backgroundColor;
        headerValue.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerValue.Padding = 8f;
        headerValue.PaddingTop = 6f;

        iTSpdf.PdfPCell headerDate = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Date"].ToUpperInvariant(), headerFont));
        headerDate.Border = borderAll;
        headerDate.BackgroundColor = backgroundColor;
        headerDate.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerDate.Padding = 8f;
        headerDate.PaddingTop = 6f;

        iTSpdf.PdfPCell headerComments = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Comments"].ToUpperInvariant(), headerFont));
        headerComments.Border = borderAll;
        headerComments.BackgroundColor = backgroundColor;
        headerComments.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerComments.Padding = 8f;
        headerComments.PaddingTop = 6f;

        iTSpdf.PdfPCell headerMeta = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Meta"].ToUpperInvariant(), headerFont));
        headerMeta.Border = borderAll;
        headerMeta.BackgroundColor = backgroundColor;
        headerMeta.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerMeta.Padding = 8f;
        headerMeta.PaddingTop = 6f;

        iTSpdf.PdfPCell headerAlarm = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Alarm"].ToUpperInvariant(), headerFont));
        headerAlarm.Border = borderAll;
        headerAlarm.BackgroundColor = backgroundColor;
        headerAlarm.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerAlarm.Padding = 8f;
        headerAlarm.PaddingTop = 6f;

        iTSpdf.PdfPCell headerResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_TableRecords_Header_Responsible"].ToUpperInvariant(), headerFont));
        headerResponsible.Border = borderAll;
        headerResponsible.BackgroundColor = backgroundColor;
        headerResponsible.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerResponsible.Padding = 8f;
        headerResponsible.PaddingTop = 6f;

        table.AddCell(headerValue);
        table.AddCell(headerDate);
        table.AddCell(headerComments);
        table.AddCell(headerMeta);
        table.AddCell(headerAlarm);
        table.AddCell(headerResponsible);

        int cont = 0;

        // Aplicar filtro
        if (dateFrom.HasValue)
        {
            registros = registros.Where(r => r.Date >= dateFrom.Value).ToList();
        }

        if (dateTo.HasValue)
        {
            registros = registros.Where(r => r.Date <= dateTo.Value).ToList();
        }

        foreach (IndicadorRegistro r in registros.Where(r=>r.Date >= fechaInicio).OrderByDescending(r => r.Date))
        {
            int border = 0;

            iTSpdf.PdfPCell cellValue = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", r.Value), times));
            cellValue.Border = border;
            cellValue.HorizontalAlignment = iTS.Element.ALIGN_RIGHT;
            cellValue.Padding = 6f;
            cellValue.PaddingTop = 4f;
            table.AddCell(cellValue);

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", r.Date), times));
            cellDate.Border = border;
            cellDate.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            table.AddCell(cellDate);

            iTSpdf.PdfPCell cellComments = new iTSpdf.PdfPCell(new iTS.Phrase(r.Comments, times));
            cellComments.Border = border;
            cellComments.Padding = 6f;
            cellComments.PaddingTop = 4f;
            table.AddCell(cellComments);

            string metaText = IndicadorRegistro.ComparerLabel(r.MetaComparer, dictionary);
            metaText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", metaText, r.Meta);
            iTSpdf.PdfPCell cellMeta = new iTSpdf.PdfPCell(new iTS.Phrase(metaText, times));
            cellMeta.Border = border;
            cellMeta.Padding = 6f;
            cellMeta.PaddingTop = 4f;
            table.AddCell(cellMeta);

            string alarmText = IndicadorRegistro.ComparerLabel(r.MetaComparer, dictionary);
            alarmText = string.Format(CultureInfo.InvariantCulture, "{0} {1:#,##0.00}", alarmText, r.Meta);
            iTSpdf.PdfPCell cellAlarm = new iTSpdf.PdfPCell(new iTS.Phrase(alarmText, times));
            cellAlarm.Border = border;
            cellAlarm.Padding = 6f;
            cellAlarm.PaddingTop = 4f;
            table.AddCell(cellAlarm);

            iTSpdf.PdfPCell cellResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(r.Responsible.FullName, times));
            cellResponsible.Border = border;
            cellResponsible.Padding = 6f;
            cellResponsible.PaddingTop = 4f;
            table.AddCell(cellResponsible);

            cont++;
        }

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }

}