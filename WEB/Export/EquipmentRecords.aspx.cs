// --------------------------------
// <copyright file="EquipmentRecords.aspx.cs" company="Sbrinna">
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
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using PDF_Tests;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;

/// <summary>
/// Implements reporting in PDF and Excel for equipment records
/// </summary>
public partial class Export_EquipmentRecords : Page
{
    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;
    public static Font fontAwe;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult Excel(
        long equipmentId,
        int companyId,
        bool calibrationInternal,
        bool calibrationExternal,
        bool verificationInternal,
        bool verificationExternal,
        bool maintenanceInternal,
        bool maintenanceExternal,
        bool repairInternal,
        bool repairExternal,
        DateTime? dateFrom,
        DateTime? dateTo)
    {
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Equipment equipment = Equipment.GetById(equipmentId, companyId);
        List<EquipmentRecord> GetFilter = HttpContext.Current.Session["EquipmentFilter"] as List<EquipmentRecord>;
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            dictionary["Item_Equipment"],
            equipment.Description,
            DateTime.Now);

        HSSFWorkbook wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        HSSFSheet sh = (HSSFSheet)wb.CreateSheet(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Equipment"], dictionary["Item_Equipment_Tab_Records"]));
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
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Equipment"], dictionary["Item_Equipment_Tab_Records"]));
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;


        IDataFormat dataFormatCustom = wb.CreateDataFormat();

        // Condiciones del filtro
        if (shCriteria.GetRow(1) == null) { shCriteria.CreateRow(1); }
        if (shCriteria.GetRow(2) == null) { shCriteria.CreateRow(2); }
        if (shCriteria.GetRow(3) == null) { shCriteria.CreateRow(3); }
        if (shCriteria.GetRow(4) == null) { shCriteria.CreateRow(4); }
        if (shCriteria.GetRow(5) == null) { shCriteria.CreateRow(5); }

        if (shCriteria.GetRow(1).GetCell(1) == null) { shCriteria.GetRow(1).CreateCell(1); }
        shCriteria.GetRow(1).GetCell(1).SetCellValue(dictionary["Calibration-Int"]);
        if (shCriteria.GetRow(1).GetCell(3) == null) { shCriteria.GetRow(1).CreateCell(3); }
        shCriteria.GetRow(1).GetCell(3).SetCellValue(calibrationInternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(1).GetCell(4) == null) { shCriteria.GetRow(1).CreateCell(4); }
        shCriteria.GetRow(1).GetCell(4).SetCellValue(dictionary["Calibration-Ext"]);
        if (shCriteria.GetRow(1).GetCell(6) == null) { shCriteria.GetRow(1).CreateCell(6); }
        shCriteria.GetRow(1).GetCell(6).SetCellValue(calibrationInternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(2).GetCell(1) == null) { shCriteria.GetRow(2).CreateCell(1); }
        shCriteria.GetRow(2).GetCell(1).SetCellValue(dictionary["Verification-Int"]);
        if (shCriteria.GetRow(2).GetCell(3) == null) { shCriteria.GetRow(2).CreateCell(3); }
        shCriteria.GetRow(2).GetCell(3).SetCellValue(verificationInternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(2).GetCell(4) == null) { shCriteria.GetRow(2).CreateCell(4); }
        shCriteria.GetRow(2).GetCell(4).SetCellValue(dictionary["Verification-Ext"]);
        if (shCriteria.GetRow(2).GetCell(6) == null) { shCriteria.GetRow(2).CreateCell(6); }
        shCriteria.GetRow(2).GetCell(6).SetCellValue(verificationExternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(3).GetCell(1) == null) { shCriteria.GetRow(3).CreateCell(1); }
        shCriteria.GetRow(3).GetCell(1).SetCellValue(dictionary["Item_Equipment_FilterLabel_MaintenanceInternal"]);
        if (shCriteria.GetRow(3).GetCell(3) == null) { shCriteria.GetRow(3).CreateCell(3); }
        shCriteria.GetRow(3).GetCell(3).SetCellValue(maintenanceInternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(3).GetCell(4) == null) { shCriteria.GetRow(3).CreateCell(4); }
        shCriteria.GetRow(3).GetCell(4).SetCellValue(dictionary["Item_Equipment_FilterLabel_MaintenanceExternal"]);
        if (shCriteria.GetRow(3).GetCell(6) == null) { shCriteria.GetRow(3).CreateCell(6); }
        shCriteria.GetRow(3).GetCell(6).SetCellValue(maintenanceInternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(4).GetCell(1) == null) { shCriteria.GetRow(4).CreateCell(1); }
        shCriteria.GetRow(4).GetCell(1).SetCellValue(dictionary["Item_Equipment_FilterLabel_RepairInternal"]);
        if (shCriteria.GetRow(4).GetCell(3) == null) { shCriteria.GetRow(4).CreateCell(3); }
        shCriteria.GetRow(4).GetCell(3).SetCellValue(repairInternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(4).GetCell(4) == null) { shCriteria.GetRow(4).CreateCell(4); }
        shCriteria.GetRow(4).GetCell(4).SetCellValue(dictionary["Item_Equipment_FilterLabel_RepairExternal"]);
        if (shCriteria.GetRow(4).GetCell(6) == null) { shCriteria.GetRow(4).CreateCell(6); }
        shCriteria.GetRow(4).GetCell(6).SetCellValue(repairExternal ? dictionary["Common_Yes"] : dictionary["Common_No"]);

        if (shCriteria.GetRow(5).GetCell(1) == null) { shCriteria.GetRow(5).CreateCell(1); }
        shCriteria.GetRow(5).GetCell(1).SetCellValue(dictionary["Common_From"]);
        if (shCriteria.GetRow(5).GetCell(3) == null) { shCriteria.GetRow(5).CreateCell(3); }
        string fromValue = "-";
        if (dateFrom.HasValue)
        {
            fromValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateFrom.Value);
        }
        shCriteria.GetRow(5).GetCell(3).SetCellValue(fromValue);

        if (shCriteria.GetRow(5).GetCell(4) == null) { shCriteria.GetRow(5).CreateCell(4); }
        shCriteria.GetRow(5).GetCell(4).SetCellValue("Hasta");
        if (shCriteria.GetRow(5).GetCell(6) == null) { shCriteria.GetRow(5).CreateCell(6); }
        string toValue = "-";
        if (dateTo.HasValue)
        {
            toValue = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateTo.Value);
        }
        shCriteria.GetRow(5).GetCell(6).SetCellValue(toValue);


        // Crear Cabecera
        List<string> headers = new List<string>() { 
            dictionary["Item_EquipmentRepair_HeaderList_Date"],
            dictionary["Item_EquipmentRepair_HeaderList_Type"],
            dictionary["Item_EquipmentRepair_HeaderList_Operation"],
            dictionary["Item_EquipmentRepair_HeaderList_Responsible"],
            dictionary["Item_EquipmentRepair_HeaderList_Cost"]
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
        decimal total = 0;
        foreach (EquipmentRecord r in GetFilter)
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }

            // Fecha
            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
            sh.GetRow(countRow).GetCell(0).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
            sh.GetRow(countRow).GetCell(0).SetCellValue(r.Date);

            // Tipo
            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
            sh.GetRow(countRow).GetCell(1).SetCellValue(dictionary[r.Item + "-" + (r.RecordType == 0 ? "Int" : "Ext")]);

            // Operacion
            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
            sh.GetRow(countRow).GetCell(2).SetCellValue(r.Operation);

            // Responsable
            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
            sh.GetRow(countRow).GetCell(3).SetCellValue(r.Responsible.FullName);

            // Coste
            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            if (r.Cost.HasValue)
            {
                sh.GetRow(countRow).GetCell(4).SetCellValue(Convert.ToDouble(r.Cost.Value));
                sh.GetRow(countRow).GetCell(4).SetCellType(CellType.Numeric);
                sh.GetRow(countRow).GetCell(4).CellStyle = moneyCellStyle;
                total += r.Cost.Value;
            }

            countRow++;
        }


        if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }
        if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
        if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
        if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
        if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }

        sh.GetRow(countRow).GetCell(0).SetCellValue(string.Empty);
        sh.GetRow(countRow).GetCell(0).CellStyle = totalCellStyle;
        sh.GetRow(countRow).GetCell(1).SetCellValue(string.Empty);
        sh.GetRow(countRow).GetCell(1).CellStyle = totalCellStyle;
        sh.GetRow(countRow).GetCell(2).SetCellValue(string.Empty);
        sh.GetRow(countRow).GetCell(2).CellStyle = totalCellStyle;
        sh.GetRow(countRow).GetCell(3).SetCellValue(dictionary["Common_Total"]);
        sh.GetRow(countRow).GetCell(3).CellStyle = totalCellStyle;


        if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
        sh.GetRow(countRow).GetCell(4).SetCellValue(Convert.ToDouble(total));
        sh.GetRow(countRow).GetCell(4).SetCellType(CellType.Numeric);
        sh.GetRow(countRow).GetCell(4).CellStyle = totalValueCellStyle;

        sh.SetColumnWidth(0, 4000);
        sh.SetColumnWidth(1, 8400);
        sh.SetColumnWidth(1, 8400);
        sh.SetColumnWidth(2, 8400);
        sh.SetColumnWidth(3, 8400);
        sh.SetColumnWidth(4, 2800);

        // INsertar logo
        /*System.Drawing.Image image = System.Drawing.Image.FromFile(string.Format("{0}\\images\\Logos\\{1}.jpg", path, companyId));
        MemoryStream ms = new MemoryStream();
        //pull the memory stream from the image (I need this for the byte array later)
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //the drawing patriarch will hold the anchor and the master information
        HSSFPatriarch patriarch = (HSSFPatriarch)sh.CreateDrawingPatriarch();
        //store the coordinates of which cell and where in the cell the image goes
        HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 2);
        //types are 0, 2, and 3. 0 resizes within the cell, 2 doesn't
        anchor.AnchorType = 0;// AnchorType.MoveDontResize;
        //add the byte array and encode it for the excel file
        int index = wb.AddPicture(ms.ToArray(), PictureType.PNG);
        HSSFPicture signaturePicture = (HSSFPicture)patriarch.CreatePicture(anchor, index);*/
        if (!path.EndsWith("\\"))
        {
            path += "\\DOCS\\";
        }
        else
        {
            path += "DOCS\\";
        }

        using (var fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create, FileAccess.Write))
        {
            wb.Write(fs);
        }

        res.SetSuccess(string.Format("/DOCS/{0}", fileName));
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(
        long equipmentId,
        int companyId,
        bool calibrationInternal,
        bool calibrationExternal,
        bool verificationInternal,
        bool verificationExternal,
        bool maintenanceInternal,
        bool maintenanceExternal,
        bool repairInternal,
        bool repairExternal,
        DateTime? dateFrom,
        DateTime? dateTo)
    {
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Equipment equipment = Equipment.GetById(equipmentId, companyId);
        Company company = new Company(equipment.CompanyId);
        List<EquipmentRecord> data = HttpContext.Current.Session["EquipmentFilter"] as List<EquipmentRecord>;
        
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.weke.pdf",
            dictionary["Item_Equipment"],
            equipment.Description,
            DateTime.Now);

        iTS.Document pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        iTSpdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = equipment.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Equipment_PDFTitle"].ToUpperInvariant()
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

        if(dateFrom.HasValue && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"Periode: {0:dd/MM/yyyy} - {1:dd/MM/yyyy}",
                dateFrom.Value,
                dateTo.Value);
                
        }
        else if(dateFrom.HasValue && !dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateFrom.Value,
                dictionary["Common_From"]); 
        }
        else if(!dateFrom.HasValue && dateTo.HasValue)
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

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment"], timesBold));
        criteria1Label.Border = borderNone;
        criteria1Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1Label.Padding = 6f;
        criteria1Label.PaddingTop = 4f;
        criteriatable.AddCell(criteria1Label);

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Description, times));
        criteria1.Border = borderNone;
        criteria1.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1.Padding = 6f;
        criteria1.PaddingTop = 4f;
        criteriatable.AddCell(criteria1);

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

        iTSpdf.PdfPCell criteria3Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Customer_Header_Type"], timesBold));
        criteria3Label.Border = borderNone;
        criteria3Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria3Label.Padding = 6f;
        criteria3Label.PaddingTop = 4f;
        criteriatable.AddCell(criteria3Label);

        string typeText = string.Empty;
        bool firstType = true;
        if( calibrationInternal){
            firstType = false;
            typeText += dictionary["Item_EquipmentRecord_Filter_CalibrationInternal"];
        }
        if (calibrationExternal)
        {
            if (!firstType)
            {
                typeText += " - ";
            }
            typeText += dictionary["Item_EquipmentRecord_Filter_CalibrationExternal"];
            firstType = false;
            
        }
        if (verificationInternal)
        {
            if (!firstType)
            {
                typeText += " - ";
            }
            typeText += dictionary["Item_EquipmentRecord_Filter_VerificationInternal"];
            firstType = false;
        }
        if (verificationExternal)
        {
            if (!firstType)
            {
                typeText += " - ";
            }
            typeText += dictionary["Item_EquipmentRecord_Filter_VerificationExternal"];
            
            firstType = false;
        }
        if (maintenanceInternal)
        {
            if (!firstType)
            {
                typeText += " - ";
            }
            typeText += dictionary["Item_EquipmentRecord_Filter_MaintenanceInternal"];
            
            firstType = false;
        }
        if (maintenanceExternal)
        {
            if (!firstType)
            {
                typeText += " - ";
            }

            typeText += dictionary["Item_EquipmentRecord_Filter_MaintenanceExternal"];
            firstType = false;
        }
        if (repairInternal)
        {
            if (!firstType)
            {
                typeText += " - ";
            }
            typeText += dictionary["Item_EquipmentRecord_Filter_RepairInternal"];
            
            firstType = false;
        }
        if (repairExternal)
        {
            if (!firstType)
            {
                typeText += " - ";
            }
            typeText += dictionary["Item_EquipmentRecord_Filter_RepairExternal"];
        }

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
        float[] widths = new float[] { 10f, 20f, 15f, 30f, 15f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 0;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;
        
        iTSpdf.PdfPCell headerDate = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentRepair_HeaderList_Date"].ToUpperInvariant(), headerFont));
        headerDate.Border = borderAll;
        headerDate.BackgroundColor = backgroundColor;
        headerDate.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerDate.Padding = 8f;
        headerDate.PaddingTop = 6f;

        iTSpdf.PdfPCell headerType = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentRepair_HeaderList_Type"].ToUpperInvariant(), headerFont));
        headerType.Border = borderAll;
        headerType.BackgroundColor = backgroundColor;
        headerType.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerType.Padding = 8f;
        headerType.PaddingTop = 6f;

        iTSpdf.PdfPCell headerOperation = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentRepair_HeaderList_Operation"].ToUpperInvariant(), headerFont));
        headerOperation.Border = borderAll;
        headerOperation.BackgroundColor = backgroundColor;
        headerOperation.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerOperation.Padding = 8f;
        headerOperation.PaddingTop = 6f;

        iTSpdf.PdfPCell headerResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentRepair_HeaderList_Responsible"].ToUpperInvariant(), headerFont));
        headerResponsible.Border = borderAll;
        headerResponsible.BackgroundColor = backgroundColor;
        headerResponsible.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerResponsible.Padding = 8f;
        headerResponsible.PaddingTop = 6f;

        iTSpdf.PdfPCell headerCost = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentRepair_HeaderList_Cost"].ToUpperInvariant(), headerFont));
        headerCost.Border = borderAll;
        headerCost.BackgroundColor = backgroundColor;
        headerCost.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerCost.Padding = 8f;
        headerCost.PaddingTop = 6f;

        table.AddCell(headerDate);
        table.AddCell(headerType);
        table.AddCell(headerOperation);
        table.AddCell(headerResponsible);
        table.AddCell(headerCost);

        decimal totalCost = 0;
        int cont=0;
        foreach (EquipmentRecord r in data)
        {
            int border = 0;            

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", r.Date), times));
            cellDate.Border = border;
            cellDate.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            table.AddCell(cellDate);

            string itemType = dictionary[r.Item + "-" + (r.RecordType == 0 ? "Int" : "Ext")];
            iTSpdf.PdfPCell typeCell = new iTSpdf.PdfPCell(new iTS.Phrase(itemType, times));
            typeCell.Border = border;
            typeCell.Padding = 6f;
            typeCell.PaddingTop = 4f;
            table.AddCell(typeCell);

            iTSpdf.PdfPCell operationCell = new iTSpdf.PdfPCell(new iTS.Phrase(r.Operation, times));
            operationCell.Border = border;
            operationCell.Padding = 6f;
            operationCell.PaddingTop = 4f;
            table.AddCell(operationCell);

            iTSpdf.PdfPCell responsibleCell = new iTSpdf.PdfPCell(new iTS.Phrase(r.Responsible.FullName, times));
            responsibleCell.Border = border;
            responsibleCell.Padding = 6f;
            responsibleCell.PaddingTop = 4f;
            table.AddCell(responsibleCell);

            iTSpdf.PdfPCell cellCost = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", r.Cost), times));
            cellCost.Border = border;
            cellCost.Padding = 6f;
            cellCost.PaddingTop = 4f;
            cellCost.HorizontalAlignment = iTS.Element.ALIGN_RIGHT;
            table.AddCell(cellCost);

            if (r.Cost.HasValue)
            {
                totalCost += r.Cost.Value;
            }

            cont++;
        }

        // Row total
        iTSpdf.PdfPCell totalCellLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"], times));
        totalCellLabel.Border = iTS.Rectangle.TOP_BORDER;
        totalCellLabel.HorizontalAlignment = iTS.Element.ALIGN_RIGHT;
        totalCellLabel.Colspan = 4;

        iTSpdf.PdfPCell totalCellValue = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", totalCost), times));
        totalCellValue.Border = iTS.Rectangle.TOP_BORDER;
        totalCellValue.HorizontalAlignment = iTS.Element.ALIGN_RIGHT;

        table.AddCell(totalCellLabel);
        table.AddCell(totalCellValue);

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}DOCS/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }

    public static iTSpdf.PdfPCell criteriaCell(bool criteria, string label)
    {
        Chunk yes = new Chunk("\uf046", fontAwe);
        Chunk no = new Chunk("\uf096", fontAwe);
        iTSpdf.PdfPCell res = new iTSpdf.PdfPCell();
        iTS.Phrase pr = new Phrase();
        pr.Add(criteria ? yes : no);
        pr.Add(new Chunk(" " + label, criteriaFont));
        res.Border = iTS.Rectangle.NO_BORDER;
        res.AddElement(pr);
        return res;
    }
}