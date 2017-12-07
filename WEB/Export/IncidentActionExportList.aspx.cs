// --------------------------------
// <copyright file="IncidentActionExportList.aspx.cs" company="Sbrinna">
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

public partial class Export_IncidentActionExportList : Page
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
        string from,
        string to,
        bool statusIdentified,
        bool statusAnalyzed,
        bool statusInProgress,
        bool statusClose,
        bool typeImprovement,
        bool typeFix,
        bool typePrevent,
        int origin)
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
            dictionary["Item_IncidentActionList"],
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
            Title = dictionary["Item_IncidentActions"].ToUpperInvariant()
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


        iTSpdf.PdfPTable titleTable = new iTSpdf.PdfPTable(1);
        float[] titleWidths = new float[] { 50f };
        titleTable.SetWidths(titleWidths);
        iTSpdf.PdfPCell titleCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), titleFont));
        titleCell.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        titleCell.Border = iTS.Rectangle.NO_BORDER;
        titleTable.AddCell(titleCell);

        var borderNone = iTS.Rectangle.NO_BORDER;
        var borderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;
        var borderTBL = iTS.Rectangle.TOP_BORDER + iTS.Rectangle.BOTTOM_BORDER + iTS.Rectangle.LEFT_BORDER;
        var borderTBR = iTS.Rectangle.TOP_BORDER + iTS.Rectangle.BOTTOM_BORDER + iTS.Rectangle.RIGHT_BORDER;


        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(7);

        // actual width of table in points
        table.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //------ CRITERIA
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(4);
        float[] cirteriaWidths = new float[] { 20f, 50f, 15f, 100f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;

        #region texts

        string criteriaOrigin = dictionary["Common_All_Male"];
        if (origin == 1) { criteriaOrigin = dictionary["Item_IncidentAction_Origin1"]; }
        if (origin == 2) { criteriaOrigin = dictionary["Item_IncidentAction_Origin2"]; }
        if (origin == 3) { criteriaOrigin = dictionary["Item_IncidentAction_Origin3"]; }
        if (origin == 4) { criteriaOrigin = dictionary["Item_IncidentAction_Origin4"]; }
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

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold));
        criteria1Label.Border = borderNone;
        criteria1Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1Label.Padding = 6f;
        criteria1Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times));
        criteria1.Border = borderNone;
        criteria1.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1.Padding = 6f;
        criteria1.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Customer_Header_Type"]+ " :", timesBold));
        criteria2Label.Border = borderNone;
        criteria2Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2Label.Padding = 6f;
        criteria2Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(typetext, times));
        criteria2.Border = borderNone;
        criteria2.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2.Padding = 6f;
        criteria2.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria3Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Status"]+ " :", timesBold));
        criteria3Label.Border = borderNone;
        criteria3Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria3Label.Padding = 6f;
        criteria3Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times));
        criteria3.Border = borderNone;
        criteria3.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria3.Padding = 6f;
        criteria3.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria4Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Origin"]+ " :", timesBold));
        criteria4Label.Border = borderNone;
        criteria4Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria4Label.Padding = 6f;
        criteria4Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria4 = new iTSpdf.PdfPCell(new iTS.Phrase(criteriaOrigin, times));
        criteria4.Border = borderNone;
        criteria4.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria4.Padding = 6f;
        criteria4.PaddingTop = 4f;


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

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 30f, 10f, 10f, 10f, 10f, 10f, 10f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 1;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerDescripcion = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Description"].ToUpperInvariant(), headerFontFinal));
        headerDescripcion.Border = borderAll;
        headerDescripcion.BackgroundColor = backgroundColor;
        headerDescripcion.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerDescripcion.Padding = 8f;
        headerDescripcion.PaddingTop = 6f;

        iTSpdf.PdfPCell headerType = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Type"].ToUpperInvariant(), headerFontFinal));
        headerType.Border = borderAll;
        headerType.BackgroundColor = backgroundColor;
        headerType.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerType.Padding = 8f;
        headerType.PaddingTop = 6f;

        iTSpdf.PdfPCell headerOpen = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Open"].ToUpperInvariant(), headerFontFinal));
        headerOpen.Border = borderAll;
        headerOpen.BackgroundColor = backgroundColor;
        headerOpen.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerOpen.Padding = 8f;
        headerOpen.PaddingTop = 6f;

        iTSpdf.PdfPCell headerStatus = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Status"].ToUpperInvariant(), headerFontFinal));
        headerStatus.Border = borderAll;
        headerStatus.BackgroundColor = backgroundColor;
        headerStatus.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerStatus.Padding = 8f;
        headerStatus.PaddingTop = 6f;

        iTSpdf.PdfPCell headerOrigen = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Origin"].ToUpperInvariant(), headerFontFinal));
        headerOrigen.Border = borderAll;
        headerOrigen.BackgroundColor = backgroundColor;
        headerOrigen.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerOrigen.Padding = 8f;
        headerOrigen.PaddingTop = 6f;

        iTSpdf.PdfPCell headerPrevision = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_ImplementDate"].ToUpperInvariant(), headerFontFinal));
        headerPrevision.Border = borderAll;
        headerPrevision.BackgroundColor = backgroundColor;
        headerPrevision.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerPrevision.Padding = 8f;
        headerPrevision.PaddingTop = 6f;

        iTSpdf.PdfPCell headerCierre = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Close"].ToUpperInvariant(), headerFontFinal));
        headerCierre.Border = borderAll;
        headerCierre.BackgroundColor = backgroundColor;
        headerCierre.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerCierre.Padding = 8f;
        headerCierre.PaddingTop = 6f;

        table.AddCell(headerDescripcion);
        table.AddCell(headerType);
        table.AddCell(headerOpen);
        table.AddCell(headerStatus);
        table.AddCell(headerOrigen);
        table.AddCell(headerPrevision);
        table.AddCell(headerCierre);

        int cont = 0;
        List<IncidentActionFilterItem> data = HttpContext.Current.Session["IncidentActionFilterData"] as List<IncidentActionFilterItem>;
        bool pair = false;
        foreach (IncidentActionFilterItem action in data)
        {
            int border = 0;

            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            iTSpdf.PdfPCell cellDescription = new iTSpdf.PdfPCell(new iTS.Phrase(action.Description, times));
            cellDescription.Border = border;
            cellDescription.BackgroundColor = lineBackground;
            cellDescription.Padding = 6f;
            cellDescription.PaddingTop = 4f;
            table.AddCell(cellDescription);

            string actionTypeText = string.Empty;
            if (action.ActionType == 1) { actionTypeText = dictionary["Item_IncidentAction_Type1"]; }
            if (action.ActionType == 2) { actionTypeText = dictionary["Item_IncidentAction_Type2"]; }
            if (action.ActionType == 3) { actionTypeText = dictionary["Item_IncidentAction_Type3"]; }

            iTSpdf.PdfPCell typeCell = new iTSpdf.PdfPCell(new iTS.Phrase(actionTypeText, times));
            typeCell.Border = border;
            typeCell.BackgroundColor = lineBackground;
            typeCell.Padding = 6f;
            typeCell.PaddingTop = 4f;
            table.AddCell(typeCell);

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture,"{0:dd/MM/yyyy}", action.OpenDate), times));
            cellDate.Border = border;
            cellDate.BackgroundColor = lineBackground;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            cellDate.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellDate);

            string statustext = string.Empty;
            if (action.Status == 1) { statustext = dictionary["Item_IndicentAction_Status1"]; }
            if (action.Status == 2) { statustext = dictionary["Item_IndicentAction_Status2"]; }
            if (action.Status == 3) { statustext = dictionary["Item_IndicentAction_Status3"]; }
            if (action.Status == 4) { statustext = dictionary["Item_IndicentAction_Status4"]; }

            iTSpdf.PdfPCell cellStatus = new iTSpdf.PdfPCell(new iTS.Phrase(statustext, times));
            cellStatus.Border = border;
            cellStatus.BackgroundColor = lineBackground;
            cellStatus.Padding = 6f;
            cellStatus.PaddingTop = 4f;
            cellStatus.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellStatus);

            string originText = string.Empty;
            if (action.Origin == 1) { originText = dictionary["Item_IncidentAction_Origin1"]; }
            if (action.Origin == 2) { originText = dictionary["Item_IncidentAction_Origin2"]; }
            if (action.Origin == 3) { originText = dictionary["Item_IncidentAction_Origin3"]; }
            if (action.Origin == 4) { originText = dictionary["Item_IncidentAction_Origin4"]; }

            iTSpdf.PdfPCell cellOrigin = new iTSpdf.PdfPCell(new iTS.Phrase(originText, times));
            cellOrigin.Border = border;
            cellOrigin.BackgroundColor = lineBackground;
            cellOrigin.Padding = 6f;
            cellOrigin.PaddingTop = 4f;
            cellOrigin.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellOrigin);

            iTSpdf.PdfPCell cellPrevision = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.ImplementationDate), times));
            cellPrevision.Border = border;
            cellPrevision.BackgroundColor = lineBackground;
            cellPrevision.Padding = 6f;
            cellPrevision.PaddingTop = 4f;
            cellPrevision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellPrevision);

            iTSpdf.PdfPCell cellCloseDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.CloseDate), times));
            cellCloseDate.Border = border;
            cellCloseDate.BackgroundColor = lineBackground;
            cellCloseDate.Padding = 6f;
            cellCloseDate.PaddingTop = 4f;
            cellCloseDate.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellCloseDate);

            cont++;
        }

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}