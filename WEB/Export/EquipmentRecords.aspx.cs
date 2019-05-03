// --------------------------------
// <copyright file="EquipmentRecords.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
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

/// <summary>Implements reporting in PDF and Excel for equipment records</summary>
public partial class ExportEquipmentRecords : Page
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
        DateTime? dateTo,
        string listOrder)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var equipment = Equipment.ById(equipmentId, companyId);
        var data = HttpContext.Current.Session["EquipmentRecordsFilter"] as List<EquipmentRecord>;
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var formatedDescription = ToolsPdf.NormalizeFileName(equipment.Description);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            dictionary["Item_Equipment"],
            formatedDescription,
            DateTime.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        var sh = (HSSFSheet)wb.CreateSheet(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Equipment"], dictionary["Item_Equipment_Tab_Records"]));
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
        sh.GetRow(0).GetCell(0).SetCellValue(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Equipment"], dictionary["Item_Equipment_Tab_Records"]));
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        var dataFormatCustom = wb.CreateDataFormat();

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
        var headers = new List<string>() { 
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

            sh.GetRow(3).GetCell(countColumns).SetCellValue(headerLabel);
            sh.GetRow(3).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        int countRow = 4;
        decimal total = 0;

        // Poner el tipo de registro diccionarizado
        if (data != null)
        {
            foreach (var record in data)
            {
                record.RecordTypeText = dictionary[record.Item + "-" + (record.RecordType == 0 ? "Int" : "Ext")];
            }

            switch (listOrder.ToUpperInvariant())
            {
                default:
                case "TH0|ASC":
                    data = data.OrderBy(d => d.Date).ToList();
                    break;
                case "TH0|DESC":
                    data = data.OrderByDescending(d => d.Date).ToList();
                    break;
                case "TH1|ASC":
                    data = data.OrderBy(d => d.RecordTypeText).ToList();
                    break;
                case "TH1|DESC":
                    data = data.OrderByDescending(d => d.RecordTypeText).ToList();
                    break;
                case "TH2|ASC":
                    data = data.OrderBy(d => d.Operation).ToList();
                    break;
                case "TH2|DESC":
                    data = data.OrderByDescending(d => d.Operation).ToList();
                    break;
                case "TH3|ASC":
                    data = data.OrderBy(d => d.Responsible.FullName).ToList();
                    break;
                case "TH3|DESC":
                    data = data.OrderByDescending(d => d.Responsible.FullName).ToList();
                    break;
                case "TH4|ASC":
                    data = data.OrderBy(d => d.Cost).ToList();
                    break;
                case "TH4|DESC":
                    data = data.OrderByDescending(d => d.Cost).ToList();
                    break;
            }

            foreach (var equipmentRecord in data)
            {
                if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }

                // Fecha
                if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
                sh.GetRow(countRow).GetCell(0).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                sh.GetRow(countRow).GetCell(0).SetCellValue(equipmentRecord.Date);

                // Tipo
                if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
                sh.GetRow(countRow).GetCell(1).SetCellValue(equipmentRecord.RecordTypeText);

                // Operacion
                if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
                sh.GetRow(countRow).GetCell(2).SetCellValue(equipmentRecord.Operation);

                // Responsable
                if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
                sh.GetRow(countRow).GetCell(3).SetCellValue(equipmentRecord.Responsible.FullName);

                // Coste
                if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
                if (equipmentRecord.Cost.HasValue)
                {
                    sh.GetRow(countRow).GetCell(4).SetCellValue(Convert.ToDouble(equipmentRecord.Cost.Value));
                    sh.GetRow(countRow).GetCell(4).SetCellType(CellType.Numeric);
                    sh.GetRow(countRow).GetCell(4).CellStyle = moneyCellStyle;
                    total += equipmentRecord.Cost.Value;
                }

                countRow++;
            }
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
        DateTime? dateTo,
        string listOrder)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var equipment = Equipment.ById(equipmentId, companyId);
        var company = new Company(equipment.CompanyId);
        var data = HttpContext.Current.Session["EquipmentRecordsFilter"] as List<EquipmentRecord>;
        
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Equipment"],
            equipment.Description,
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
            CompanyId = equipment.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Equipment_PDFTitle"].ToUpperInvariant()
        };

        pdfDoc.Open();
        
        var backgroundColor = new iTS.BaseColor(220, 220, 220);

        string periode = string.Empty;

        if(dateFrom.HasValue && dateTo.HasValue)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"Periode: {0:dd/MM/yyyy} - {1:dd/MM/yyyy}",
                dateFrom.Value,
                dateTo.Value);
                
        }
        else if(dateFrom.HasValue && dateTo == null)
        {
            periode = string.Format(
                CultureInfo.InvariantCulture,
                @"{1}: {0:dd/MM/yyyy}",
                dateFrom.Value,
                dictionary["Common_From"]); 
        }
        else if(dateFrom == null && dateTo.HasValue)
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

        criteriatable.SetWidths(new float[] { 25f, 250f });
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment"], ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Description, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(periode, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Customer_Header_Type"], ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        string typeText = string.Empty;
        bool firstType = true;
        if (calibrationInternal)
        {
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
        
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(typeText, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        pdfDoc.Add(criteriatable);

        var table = new iTSpdf.PdfPTable(5)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        //relative col widths in proportions - 1/3 and 2/3
        table.SetWidths(new float[] { 10f, 20f, 15f, 30f, 15f });

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_EquipmentRepair_HeaderList_Date"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_EquipmentRepair_HeaderList_Type"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_EquipmentRepair_HeaderList_Operation"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_EquipmentRepair_HeaderList_Responsible"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_EquipmentRepair_HeaderList_Cost"]));

        decimal totalCost = 0;
        int cont=0;

        // Poner el tipo de registro diccionarizado
        foreach (var record in data)
        {
            record.RecordTypeText = dictionary[record.Item + "-" + (record.RecordType == 0 ? "Int" : "Ext")];
        }

        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH0|ASC":
                data = data.OrderBy(d => d.Date).ToList();
                break;
            case "TH0|DESC":
                data = data.OrderByDescending(d => d.Date).ToList();
                break;
            case "TH1|ASC":
                data = data.OrderBy(d => d.RecordTypeText).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.RecordTypeText).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.Operation).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.Operation).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.Responsible.FullName).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.Responsible.FullName).ToList();
                break;
            case "TH4|ASC":
                data = data.OrderBy(d => d.Cost).ToList();
                break;
            case "TH4|DESC":
                data = data.OrderByDescending(d => d.Cost).ToList();
                break;
        }
        
        foreach (var equipmentRecord in data)
        {
            table.AddCell(ToolsPdf.DataCellCenter(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", equipmentRecord.Date), ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCell(equipmentRecord.RecordTypeText, ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCell(equipmentRecord.Operation, ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCell(equipmentRecord.Responsible.FullName, ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCellMoney(equipmentRecord.Cost, ToolsPdf.LayoutFonts.Times));

            if (equipmentRecord.Cost.HasValue)
            {
                totalCost += equipmentRecord.Cost.Value;
            }

            cont++;
        }

        
        string totalRegistros = string.Format(
           CultureInfo.InvariantCulture,
           @"{0}: {1}",
           dictionary["Common_RegisterCount"],
           cont);

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 2
        });

        table.AddCell(new PdfPCell(new iTS.Phrase(dictionary["Common_Total"], ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            HorizontalAlignment = iTS.Element.ALIGN_RIGHT,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Colspan = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", totalCost), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            HorizontalAlignment = iTS.Element.ALIGN_RIGHT
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}