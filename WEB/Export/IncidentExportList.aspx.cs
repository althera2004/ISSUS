// --------------------------------
// <copyright file="IncidentExportList.aspx.cs" company="Sbrinna">
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

public partial class ExportIncidentExportList : Page
{
    BaseFont HeaderFont = null;
    BaseFont Arial = null;

    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;
    public static Font fontAwe;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(
        int companyId,
        string from,
        string to,
        bool statusIdnetified,
        bool statusAnalyzed,
        bool statusInProgress,
        bool statusClose,
        int origin,
        int departmentId,
        int providerId,
        int customerId,
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

        var fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_IncidentList"],
            company.Name,
            DateTime.Now);

        // FONTS
        var pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        var headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

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
            Title = dictionary["Item_IncidentList"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(225, 225, 225);
        var rowPair = new iTS.BaseColor(255, 255, 255);
        var rowEven = new iTS.BaseColor(240, 240, 240);

        // ------------ FONTS 
        var awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        criteriaFont = new iTS.Font(arial, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var titleFont = new iTS.Font(arial, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

        var fontAwesomeIcon = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        fontAwe = new Font(fontAwesomeIcon, 10);
        // -------------------

        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), titleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });

        var borderNone = iTS.Rectangle.NO_BORDER;

        //------ CRITERIA
        var criteriatable = new iTSpdf.PdfPTable(2)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 8f, 50f });

        #region texts

        var criteriaOrigin = dictionary["Item_Incident_Origin0"];
        if (origin == 1)
        {
            if (departmentId > 0)
            {
                var department = Department.GetById(departmentId, companyId);
                criteriaOrigin = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Incident_Origin1"], department.Description);
            }
            else
            {
                criteriaOrigin = string.Format(CultureInfo.InvariantCulture, "{0} - ({1})", dictionary["Item_Incident_Origin1"], dictionary["Common_All_Male_Plural"]);
            }
        }
        if (origin == 2)
        {
            if (providerId > 0)
            {
                var provider = Provider.GetById(providerId, companyId);
                criteriaOrigin = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Incident_Origin2"], provider.Description);
            }
            else
            {
                criteriaOrigin = string.Format(CultureInfo.InvariantCulture, "{0} - ({1})", dictionary["Item_Incident_Origin2"], dictionary["Common_All_Male_Plural"]);
            }
        }
        if (origin == 3)
        {
            if (customerId > 0)
            {
                var customer = Customer.GetById(customerId, companyId);
                criteriaOrigin = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Incident_Origin3"], customer.Description);
            }
            else
            {
                criteriaOrigin = string.Format(CultureInfo.InvariantCulture, "{0} - ({1})", dictionary["Item_Incident_Origin3"], dictionary["Common_All_Male_Plural"]);
            }
        }

        var periode = string.Empty;
        if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
        {
            periode = dictionary["Item_Incident_List_Filter_From"] + " " + from;
        }
        else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = dictionary["Item_Incident_List_Filter_To"] + " " + to;
        }
        else if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = from + " " + to;
        }
        else
        {
            periode = dictionary["Common_All_Male"];
        }

        var typetext = string.Empty;
        var firstType = false;
        //if (typeImprovement)
        //{
        //    typetext = dictionary["Item_IncidentAction_Type1"];
        //    firstType = false;
        //}
        //if (typeFix)
        //{
        //    if (!firstType)
        //    {
        //        typetext += " - ";
        //    }
        //    typetext += dictionary["Item_IncidentAction_Type2"];
        //    firstType = false;
        //}
        //if (typePrevent)
        //{
        //    if (!firstType)
        //    {
        //        typetext += " - ";
        //    }
        //    typetext += dictionary["Item_IncidentAction_Type3"];
        //}

        var statusText = string.Empty;
        var firstStatus = true;
        if (statusIdnetified)
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

        var criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"] + " :", timesBold))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, timesBold))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria3Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Status"] + " :", timesBold))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria4Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Origin"] + " :", timesBold))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria4 = new iTSpdf.PdfPCell(new iTS.Phrase(criteriaOrigin, times))
        {
            Border = borderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };


        criteriatable.AddCell(criteria1Label);
        criteriatable.AddCell(criteria1);
        //criteriatable.AddCell(criteria2Label);
        //criteriatable.AddCell(criteria2);
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

        table.SetWidths(new float[] { 35f, 10f, 10f, 20f, 8f, 10f, 10f });

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Incident_Header_Description"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Incident_Header_Open"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Incident_Header_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Incident_Header_Origin"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Incident_Header_ActionNumber"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Incident_Header_Cost"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Incident_Header_Close"]));

        int cont = 0;
        decimal totalCost = 0;
        var data = HttpContext.Current.Session["IncidentFilterData"] as List<IncidentFilterItem>;
        bool pair = false;

        foreach(IncidentFilterItem item in data)
        {
            var originValue = string.Empty;
            if (!string.IsNullOrEmpty(item.Customer.Description))
            {
                originValue = item.Customer.Description;
            }
            if (!string.IsNullOrEmpty(item.Provider.Description))
            {
                originValue = item.Provider.Description;
            }
            if (!string.IsNullOrEmpty(item.Department.Description))
            {
                originValue = item.Department.Description;
            }

            item.OriginText = originValue;
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
                data = data.OrderBy(d => d.Open).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.Open).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.OriginText).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.OriginText).ToList();
                break;
            case "TH4|ASC":
                data = data.OrderBy(d => d.Action.Description).ToList();
                break;
            case "TH4|DESC":
                data = data.OrderByDescending(d => d.Action.Description).ToList();
                break;
            case "TH5|ASC":
                data = data.OrderBy(d => d.Amount).ToList();
                break;
            case "TH5|DESC":
                data = data.OrderByDescending(d => d.Amount).ToList();
                break;
            case "TH6|ASC":
                data = data.OrderBy(d => d.Close).ToList();
                break;
            case "TH6|DESC":
                data = data.OrderByDescending(d => d.Close).ToList();
                break;
        }

        foreach (var incidentFilter in data)
        {
            cont++;
            totalCost += incidentFilter.Amount;
            var incident = Incident.GetById(incidentFilter.Id, companyId);
            var action = IncidentAction.GetByIncidentId(incident.Id, companyId);

            var lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            var statustext = string.Empty;
            if (incidentFilter.Status == 1) { statustext = dictionary["Item_IndicentAction_Status1"]; }
            if (incidentFilter.Status == 2) { statustext = dictionary["Item_IndicentAction_Status2"]; }
            if (incidentFilter.Status == 3) { statustext = dictionary["Item_IndicentAction_Status3"]; }
            if (incidentFilter.Status == 4) { statustext = dictionary["Item_IndicentAction_Status4"]; }

            var actionDescription = string.Empty;
            if (!string.IsNullOrEmpty(action.Description))
            {
                actionDescription = dictionary["Common_Yes"];
            }

            var originText = string.Empty;
            if (incidentFilter.Origin == 1) { originText = dictionary["Item_IncidentAction_Origin1"]; }
            if (incidentFilter.Origin == 2) { originText = dictionary["Item_IncidentAction_Origin2"]; }
            if (incidentFilter.Origin == 3) { originText = dictionary["Item_IncidentAction_Origin3"]; }
            if (incidentFilter.Origin == 4) { originText = dictionary["Item_IncidentAction_Origin4"]; }

            table.AddCell(ToolsPdf.DataCell(incidentFilter.Description, times));
            table.AddCell(ToolsPdf.DataCellCenter(incidentFilter.Open, times));
            table.AddCell(ToolsPdf.DataCell(statustext, times));
            table.AddCell(ToolsPdf.DataCell(incidentFilter.OriginText, times));
            table.AddCell(ToolsPdf.DataCell(actionDescription, times));
            table.AddCell(ToolsPdf.DataCellMoney(incidentFilter.Amount, times));
            table.AddCell(ToolsPdf.DataCellCenter(incidentFilter.Close, times));
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 4
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(Tools.PdfMoneyFormat(totalCost), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 2
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}