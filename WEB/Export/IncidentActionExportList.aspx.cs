// --------------------------------
// <copyright file="IncidentActionExportList.aspx.cs" company="Sbrinna">
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
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class ExportIncidentActionExportList : Page
{
    public static Dictionary<string, string> dictionary;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(
        int companyId,
        string from,
        string to,
        bool statusIdentified,
        bool statusAnalyzed,
        bool statusInProgress,
        bool statusClose,
        bool typeImprovement,
        bool typeFix,
        bool typePrevent,
        int origin,
        int reporter,
        string listOrder)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var formatedDescription = ToolsPdf.NormalizeFileName(company.Name);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_IncidentActionList"],
            formatedDescription,
            DateTime.Now);

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
            Title = dictionary["Item_IncidentActions"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(225, 225, 225);
        var rowPair = new iTS.BaseColor(255, 255, 255);
        var rowEven = new iTS.BaseColor(240, 240, 240);

        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        var titleCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), ToolsPdf.LayoutFonts.TitleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = ToolsPdf.BorderNone
        };

        titleTable.AddCell(titleCell);

        #region Criteria
        var criteriatable = new iTSpdf.PdfPTable(4);
        criteriatable.SetWidths(new float[] { 20f, 50f, 15f, 100f });
        criteriatable.WidthPercentage = 100;

        #region texts

        string criteriaOrigin = dictionary["Common_All_Male"];
        if (origin == 1) { criteriaOrigin = dictionary["Item_IncidentAction_Origin1"]; }
        if (origin == 2) { criteriaOrigin = dictionary["Item_IncidentAction_Origin2"]; }
        if (origin == 3) { criteriaOrigin = dictionary["Item_IncidentAction_Origin3"]; }
        if (origin == 4) { criteriaOrigin = dictionary["Item_IncidentAction_Origin46"]; }
        if (origin == 5) { criteriaOrigin = dictionary["Item_IncidentAction_Origin5"]; }

        string reporterText = dictionary["Common_All_Male"];
        if (reporter == 1) { reporterText = dictionary["Item_IncidentAction_ReporterType1"]; }
        if (reporter == 2) { reporterText = dictionary["Item_IncidentAction_ReporterType2"]; }
        if (reporter == 3) { reporterText = dictionary["Item_IncidentAction_ReporterType3"]; }


        string periode = string.Empty;
        if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
        {
            periode = dictionary["Item_IncidentAction_List_Filter_From"] + " " + from;
        }
        else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = dictionary["Item_IncidentAction_List_Filter_To"] + " " + to;
        }
        else if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = from + " " + to;
        }
        else
        {
            periode = dictionary["Common_All_Male"];
        }

        string typetext = string.Empty;

        bool firstType = false;
        if (typeImprovement)
        {
            typetext = dictionary["Item_IncidentAction_Type1"];
            firstType = false;
        }
        if (typeFix)
        {
            if (!firstType)
            {
                typetext += " - ";
            }
            typetext += dictionary["Item_IncidentAction_Type2"];
            firstType = false;
        }
        if (typePrevent)
        {
            if (!firstType)
            {
                typetext += " - ";
            }
            typetext += dictionary["Item_IncidentAction_Type3"];
        }

        string statusText = string.Empty;
        bool firstStatus = true;
        if (statusIdentified)
        {
            firstStatus = false;
            statusText += dictionary["Item_IndicentAction_Status1"];
        }

        if (statusAnalyzed)
        {
            if (!firstStatus)
            {
                statusText += " - ";
            }
            statusText += dictionary["Item_IndicentAction_Status2"];
            firstStatus = false;

        }

        if (statusInProgress)
        {
            if (!firstStatus)
            {
                statusText += " - ";
            }
            statusText += dictionary["Item_IndicentAction_Status3"];
            firstType = false;
        }

        if (statusClose)
        {
            if (!firstType)
            {
                statusText += " - ";
            }
            statusText += dictionary["Item_IndicentAction_Status4"];

            firstType = false;
        }
        #endregion

        ToolsPdf.AddCriteria(criteriatable, dictionary["Common_Period"], periode);
        ToolsPdf.AddCriteria(criteriatable, dictionary["Item_Customer_Header_Type"], typetext);
        ToolsPdf.AddCriteria(criteriatable, dictionary["Item_IncidentAction_Header_Status"], statusText);
        ToolsPdf.AddCriteria(criteriatable, dictionary["Item_IncidentAction_Header_Origin"], criteriaOrigin);
        ToolsPdf.AddCriteria(criteriatable, dictionary["Item_IncidentAction_Label_Reporter"], reporterText);
        ToolsPdf.AddCriteria(criteriatable, string.Empty, string.Empty);
        pdfDoc.Add(criteriatable);
        #endregion

        var table = new iTSpdf.PdfPTable(8)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 30f, 10f, 10f, 10f, 10f, 10f, 10f, 10f });
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Description"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Type"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Open"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Origin"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Cost"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_ImplementDate"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Close"]));
        
        int cont = 0;
        decimal totalCost = 0;
        var data = HttpContext.Current.Session["IncidentActionFilterData"] as List<IncidentActionFilterItem>;

        foreach(IncidentActionFilterItem item in data)
        {
            string originText = string.Empty;
            if (item.Origin == 1) { originText = dictionary["Item_IncidentAction_Origin1"]; }
            if (item.Origin == 2) { originText = dictionary["Item_IncidentAction_Origin2"]; }
            if (item.Origin == 3) { originText = dictionary["Item_IncidentAction_Origin3"]; }
            if (item.Origin == 4) { originText = dictionary["Item_IncidentAction_Origin4"]; }
            if (item.Origin == 5) { originText = dictionary["Item_IncidentAction_Origin5"]; }
            if (item.Origin == 6) { originText = dictionary["Item_IncidentAction_Origin6"]; }
            item.OriginText = originText;
        }

        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH0|ASC":
                data = data.OrderBy(d => d.Description).ToList();
                break;
            case "TH0|DESC":
                data = data.OrderByDescending(d => d.Description).ToList();
                break;
            case "TH1|ASC":
                data = data.OrderBy(d => d.ActionType).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.ActionType).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.OpenDate).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.OpenDate).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.Status).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.Status).ToList();
                break;
            case "TH4|ASC":
                data = data.OrderBy(d => d.OriginText).ToList();
                break;
            case "TH4|DESC":
                data = data.OrderByDescending(d => d.OriginText).ToList();
                break;
            case "TH5|ASC":
                data = data.OrderBy(d => d.ImplementationDate).ToList();
                break;
            case "TH5|DESC":
                data = data.OrderByDescending(d => d.ImplementationDate).ToList();
                break;
            case "TH6|ASC":
                data = data.OrderBy(d => d.CloseDate).ToList();
                break;
            case "TH6|DESC":
                data = data.OrderByDescending(d => d.CloseDate).ToList();
                break;
        }

        foreach (IncidentActionFilterItem action in data)
        {
            int border = 0;
            totalCost += action.Amount;

            string actionTypeText = string.Empty;
            switch (action.ActionType)
            {
                default:
                case 1: actionTypeText = dictionary["Item_IncidentAction_Type1"]; break;
                case 2: actionTypeText = dictionary["Item_IncidentAction_Type2"]; break;
                case 3: actionTypeText = dictionary["Item_IncidentAction_Type3"]; break;
            }

            string statustext = string.Empty;
            if (action.Status == 1) { statustext = dictionary["Item_IndicentAction_Status1"]; }
            if (action.Status == 2) { statustext = dictionary["Item_IndicentAction_Status2"]; }
            if (action.Status == 3) { statustext = dictionary["Item_IndicentAction_Status3"]; }
            if (action.Status == 4) { statustext = dictionary["Item_IndicentAction_Status4"]; }

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(action.Description, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f
            });

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(actionTypeText, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f
            });

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.OpenDate), ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
            
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(statustext, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(action.OriginText, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
            
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(Tools.PdfMoneyFormat(action.Amount), ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_RIGHT
            });
            
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.ImplementationDate), ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.CloseDate), ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = ToolsPdf.LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });

            cont++;
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 4
        });
        
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });
        
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(Tools.PdfMoneyFormat(totalCost), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });
        
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Colspan = 2
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}