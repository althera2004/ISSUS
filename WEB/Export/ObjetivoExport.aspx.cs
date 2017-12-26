// --------------------------------
// <copyright file="ObjetivoExportList.aspx.cs" company="Sbrinna">
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

public partial class Export_ObjetivoExport : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

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
        DateTime? from,
        DateTime? to,
        int status,
        string listOrder)
    {
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Company company = new Company(companyId);
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Objetivo_List"],
            company.Name,
            DateTime.Now);

        // FONTS
        string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        BaseFont headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        BaseFont arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        iTS.Document pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        iTSpdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
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
            Title = dictionary["Item_Objetivo_List"].ToUpperInvariant()
        };

        pdfDoc.Open();

        iTS.BaseColor backgroundColor = new iTS.BaseColor(225, 225, 225);
        iTS.BaseColor rowPair = new iTS.BaseColor(255, 255, 255);
        iTS.BaseColor rowEven = new iTS.BaseColor(240, 240, 240);

        // ------------ FONTS 
        iTSpdf.BaseFont awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        iTS.Font times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        criteriaFont = new iTS.Font(arial, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font titleFont = new iTS.Font(arial, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

        var fontAwesomeIcon = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        fontAwe = new Font(fontAwesomeIcon, 10);
        // -------------------        

        float[] titleWidths = new float[] { 50f };
        iTSpdf.PdfPTable titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(titleWidths);

        iTSpdf.PdfPCell titleCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), titleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        };

        titleTable.AddCell(titleCell);
        
        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100
        };

        //------ CRITERIA
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(4);
        float[] cirteriaWidths = new float[] { 20f, 50f, 20f, 150f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;

        iTSpdf.PdfPCell criteriaBlank = new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        string periode = string.Empty;
        if (from.HasValue && !to.HasValue)
        {
            periode = string.Format(CultureInfo.InvariantCulture, @"{0} {1:dd/MM/yyyy}", dictionary["Item_IncidentAction_List_Filter_From"], from);
        }
        else if (!from.HasValue && to.HasValue)
        {
            periode = string.Format(CultureInfo.InvariantCulture, @"{0} {1:dd/MM/yyyy}", dictionary["Item_IncidentAction_List_Filter_To"], to);
        }
        else if (from.HasValue && to.HasValue)
        {
            periode = string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy} {1:dd/MM/yyyy}", from, to);
        }
        else
        {
            periode = dictionary["Common_All_Male"];
        }

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };
        criteriatable.AddCell(criteria1Label);

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };
        criteriatable.AddCell(criteria1);

        iTSpdf.PdfPCell criteriaStatusLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Status"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };
        criteriatable.AddCell(criteriaStatusLabel);

        string statusText = dictionary["Common_All"];
        if (status == 1)
        {
            statusText = dictionary["Item_ObjetivoAction_List_Filter_ShowActive"];
        }

        if (status == 2)
        {
            statusText = dictionary["Item_ObjetivoAction_List_Filter_ShowClosed"];
        }

        iTSpdf.PdfPCell criteriaStatus = new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };
        criteriatable.AddCell(criteriaStatus);
        criteriatable.AddCell(criteriaBlank);

        pdfDoc.Add(criteriatable);
        //---------------------------

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 50f, 30f, 10f, 10f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 1;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_Name"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_Responsible"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_StartDate"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_PreviewEndDate"].ToUpperInvariant(), headerFontFinal));

        int cont = 0;
        List<ObjetivoFilterItem> data = Objetivo.Filter(companyId,  from, to, status).ToList();
        bool pair = false;

        foreach(ObjetivoFilterItem item in data)
        {
            if (!item.Objetivo.EndDate.HasValue)
            {
                item.Objetivo.EndDate = item.Objetivo.PreviewEndDate;
            }
        }

        switch (listOrder.ToUpperInvariant())
        {
            case "TH0|ASC":
                data = data.OrderBy(d => d.Objetivo.Name).ToList();
                break;
            case "TH0|DESC":
                data = data.OrderByDescending(d => d.Objetivo.Name).ToList();
                break;
            case "TH1|ASC":
                data = data.OrderBy(d => d.Objetivo.Responsible.FullName).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.Objetivo.Responsible.FullName).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.Objetivo.StartDate).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.Objetivo.StartDate).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.Objetivo.EndDate).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.Objetivo.EndDate).ToList();
                break;
        }

        cont = 0;
        foreach (ObjetivoFilterItem item in data)
        {
            int border = 0;
            cont++;
            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            iTSpdf.PdfPCell cellDescription = new iTSpdf.PdfPCell(new iTS.Phrase(item.Objetivo.Name, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f
            };
            table.AddCell(cellDescription);

            iTSpdf.PdfPCell cellResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(item.Objetivo.Responsible.FullName, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_LEFT
            };
            table.AddCell(cellResponsible);

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.StartDate), times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            };
            table.AddCell(cellDate);

            string endDateText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.PreviewEndDate);
            if (item.Objetivo.EndDate.HasValue)
            {
                endDateText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.EndDate);
            }

            iTSpdf.PdfPCell cellEndDate = new iTSpdf.PdfPCell(new iTS.Phrase(endDateText, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            };
            table.AddCell(cellEndDate);
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont);
        iTSpdf.PdfPCell totalRegistrosCell = new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f
        };

        iTSpdf.PdfPCell blankCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 3
        };

        table.AddCell(totalRegistrosCell);
        table.AddCell(blankCell);

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}