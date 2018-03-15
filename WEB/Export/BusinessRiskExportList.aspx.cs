// --------------------------------
// <copyright file="BusinessRiskExportList.aspx.cs" company="Sbrinna">
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
using System.Linq;
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

public partial class ExportBusinessRiskExportList : Page
{
    BaseFont HeaderFont = null;
    BaseFont Arial = null;

    public static Font CriteriaFont;
    public static Dictionary<string, string> Dictionary;
    public static Font FontAwe;

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(
        int companyId,
        string from,
        string to,
        long rulesId,
        long processId,
        int typeId,
        string listOrder)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            Dictionary["Item_BusinessRisks"],
            company.Name,
            DateTime.Now);

        #region Fonts
        string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        var headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        CriteriaFont = new iTS.Font(arial, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var titleFont = new iTS.Font(arial, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var fontAwesomeIcon = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        FontAwe = new Font(fontAwesomeIcon, 10);
        #endregion

        var pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, company.Id),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = company.Id,
            CompanyName = company.Name,
            Title = Dictionary["Item_BusinessRisks"].ToUpperInvariant()
        };

        pdfDoc.Open();


        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Dictionary["Item_EquipmentList"], company.Name), titleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });
        
        //------ CRITERIA
        var criteriatable = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 10f, 50f, 10f, 100f });

        #region texts

        string criteriaProccess = Dictionary["Common_All_Male_Plural"];
        if (processId > 0)
        {
            var process = new Process(processId, companyId);
            if (!string.IsNullOrEmpty(process.Description))
            {
                criteriaProccess = process.Description;
            }
        }

        string periode = string.Empty;
        if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
        {
            periode = Dictionary["Item_Incident_List_Filter_From"] + " " + from;
        }
        else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = Dictionary["Item_Incident_List_Filter_To"] + " " + to;
        }
        else if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = from + " - " + to;
        }
        else
        {
            periode = Dictionary["Common_All_Male"];
        }

        string typetext = Dictionary["Common_All_Male_Plural"];
        if (typeId == 1) { typetext = Dictionary["Item_BusinessRisk_Status_Assumed"]; }
        if (typeId == 2) { typetext = Dictionary["Item_BusinessRisk_Status_Significant"]; }
        if (typeId == 3) { typetext = Dictionary["Item_BusinessRisk_Status_NotSignificant"]; }
        if (typeId == 4) { typetext = Dictionary["Item_BusinessRisk_Status_Unevaluated"]; }

        string ruleDescription = Dictionary["Common_All_Female_Plural"];
        if (rulesId > 0)
        {
            var rule = Rules.GetById(companyId, rulesId);
            if (!string.IsNullOrEmpty(rule.Description))
            {
                ruleDescription = rule.Description;
            }
        }
        #endregion

        var criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Common_Period"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Item_BusinesRisk_ListHeader_Process"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(typetext, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria3Label = new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Item_BusinesRisk_ListHeader_Rule"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(ruleDescription, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria4Label = new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Item_BusinesRisk_ListHeader_Type"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria4 = new iTSpdf.PdfPCell(new iTS.Phrase(criteriaProccess, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        criteriatable.AddCell(criteria1Label);
        criteriatable.AddCell(criteria1);
        criteriatable.AddCell(criteria2Label);
        criteriatable.AddCell(criteria2);
        criteriatable.AddCell(criteria3Label);
        criteriatable.AddCell(criteria3);
        criteriatable.AddCell(criteria4Label);
        criteriatable.AddCell(criteria4);

        pdfDoc.Add(criteriatable);
        //---------------------------

        var table = new iTSpdf.PdfPTable(7)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 10f, 10f, 30f, 20f, 20f, 10f, 10f });

        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_IncidentAction_Header_Type"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Date"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Description"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Process"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Rule"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_StartValue"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_IPR"].ToUpperInvariant(), headerFontFinal));

        int cont = 0;
        var data = HttpContext.Current.Session["BusinessRiskFilterData"] as List<BusinessRiskFilterItem>;

        switch (listOrder.ToUpperInvariant())
        {
            case "TH1|ASC":
                data = data.OrderBy(d => d.OpenDate).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.OpenDate).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.Description).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.Description).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.Process.Description).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.Process.Description).ToList();
                break;
            case "TH4|ASC":
                data = data.OrderBy(d => d.Rule.Description).ToList();
                break;
            case "TH4|DESC":
                data = data.OrderByDescending(d => d.Rule.Description).ToList();
                break;
            case "TH5|ASC":
                data = data.OrderBy(d => d.InitialResult).ToList();
                break;
            case "TH5|DESC":
                data = data.OrderByDescending(d => d.InitialResult).ToList();
                break;
            case "TH6|ASC":
                data = data.OrderBy(d => d.Rule.Limit).ToList();
                break;
            case "TH6|DESC":
                data = data.OrderByDescending(d => d.Rule.Limit).ToList();
                break;
        }

        foreach (BusinessRiskFilterItem risk in data)
        {
            cont++;
            string typeText = string.Empty;
            if (risk.Assumed) { typeText = Dictionary["Item_BusinessRisk_Status_Assumed"]; }
            else if (risk.InitialResult == 0) { typeText = Dictionary["Item_BusinessRisk_Status_Unevaluated"]; }
            else if (risk.InitialResult < risk.Rule.Limit) { typeText = Dictionary["Item_BusinessRisk_Status_NotSignificant"]; }
            else { typeText = Dictionary["Item_BusinessRisk_Status_Significant"]; }

            string initialResultText = risk.InitialResult == 0 ? string.Empty : risk.InitialResult.ToString();

            table.AddCell(ToolsPdf.DataCellCenter(typeText, times));
            table.AddCell(ToolsPdf.DataCellCenter(risk.OpenDate, times));
            table.AddCell(ToolsPdf.DataCell(risk.Description, times));
            table.AddCell(ToolsPdf.DataCell(risk.Process.Description,times));
            table.AddCell(ToolsPdf.DataCellCenter(risk.Rule.Description, times));
            table.AddCell(ToolsPdf.DataCellCenter(initialResultText, times));
            table.AddCell(ToolsPdf.DataCellRight(risk.Rule.Limit.ToString(), times));
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            Dictionary["Common_RegisterCount"],
            cont), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 4
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            Colspan = 4
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}