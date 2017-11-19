// --------------------------------
// <copyright file="ObjetivoExportList.aspx.cs" company="Sbrinna">
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
        int status)
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
               string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}", path, fileName),
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


        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(4);

        // actual width of table in points
        table.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //------ CRITERIA
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(4);
        float[] cirteriaWidths = new float[] { 20f, 50f, 20f, 150f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;

        iTSpdf.PdfPCell criteriaBlank = new iTSpdf.PdfPCell(new iTS.Phrase(".", times));
        criteriaBlank.Border = borderNone;
        criteriaBlank.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteriaBlank.Padding = 6f;
        criteriaBlank.PaddingTop = 4f;

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

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold));
        criteria1Label.Border = borderNone;
        criteria1Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1Label.Padding = 6f;
        criteria1Label.PaddingTop = 4f;
        criteriatable.AddCell(criteria1Label);

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times));
        criteria1.Border = borderNone;
        criteria1.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1.Padding = 6f;
        criteria1.PaddingTop = 4f;
        criteriatable.AddCell(criteria1);

        iTSpdf.PdfPCell criteriaStatusLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Status"] + " :", timesBold));
        criteriaStatusLabel.Border = borderNone;
        criteriaStatusLabel.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteriaStatusLabel.Padding = 6f;
        criteriaStatusLabel.PaddingTop = 4f;
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

        iTSpdf.PdfPCell criteriaStatus = new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times));
        criteriaStatus.Border = borderNone;
        criteriaStatus.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteriaStatus.Padding = 6f;
        criteriaStatus.PaddingTop = 4f;
        criteriatable.AddCell(criteriaStatus);
        criteriatable.AddCell(criteriaBlank);

        pdfDoc.Add(criteriatable);
        //---------------------------

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 30f, 10f, 10f, 30f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 1;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerDescripcion = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Objetivo_Header_Name"].ToUpperInvariant(), headerFontFinal));
        headerDescripcion.Border = borderAll;
        headerDescripcion.BackgroundColor = backgroundColor;
        headerDescripcion.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerDescripcion.Padding = 8f;
        headerDescripcion.PaddingTop = 6f;

        iTSpdf.PdfPCell headerStartDate = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Objetivo_Header_StartDate"].ToUpperInvariant(), headerFontFinal));
        headerStartDate.Border = borderAll;
        headerStartDate.BackgroundColor = backgroundColor;
        headerStartDate.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerStartDate.Padding = 8f;
        headerStartDate.PaddingTop = 6f;

        iTSpdf.PdfPCell headerEndDate = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Objetivo_Header_PreviewEndDate"].ToUpperInvariant(), headerFontFinal));
        headerEndDate.Border = borderAll;
        headerEndDate.BackgroundColor = backgroundColor;
        headerEndDate.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerEndDate.Padding = 8f;
        headerEndDate.PaddingTop = 6f;

        iTSpdf.PdfPCell headerResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Objetivo_Header_Responsible"].ToUpperInvariant(), headerFontFinal));
        headerResponsible.Border = borderAll;
        headerResponsible.BackgroundColor = backgroundColor;
        headerResponsible.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerResponsible.Padding = 8f;
        headerResponsible.PaddingTop = 6f;

        table.AddCell(headerDescripcion);
        table.AddCell(headerStartDate);
        table.AddCell(headerEndDate);
        table.AddCell(headerResponsible);

        int cont = 0;
        ReadOnlyCollection<ObjetivoFilterItem> data = Objetivo.Filter(companyId,  from, to, status);
        bool pair = false;
        foreach (ObjetivoFilterItem item in data)
        {
            int border = 0;

            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            iTSpdf.PdfPCell cellDescription = new iTSpdf.PdfPCell(new iTS.Phrase(item.Objetivo.Name, times));
            cellDescription.Border = border;
            cellDescription.BackgroundColor = lineBackground;
            cellDescription.Padding = 6f;
            cellDescription.PaddingTop = 4f;
            table.AddCell(cellDescription);

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.StartDate), times));
            cellDate.Border = border;
            cellDate.BackgroundColor = lineBackground;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            cellDate.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellDate);

            string endDateText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.PreviewEndDate);
            if (item.Objetivo.EndDate.HasValue)
            {
                endDateText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.EndDate);
            }

            iTSpdf.PdfPCell cellEndDate = new iTSpdf.PdfPCell(new iTS.Phrase(endDateText, times));
            cellEndDate.Border = border;
            cellEndDate.BackgroundColor = lineBackground;
            cellEndDate.Padding = 6f;
            cellEndDate.PaddingTop = 4f;
            cellEndDate.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellEndDate);

            iTSpdf.PdfPCell cellResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(item.Objetivo.Responsible.FullName, times));
            cellResponsible.Border = border;
            cellResponsible.BackgroundColor = lineBackground;
            cellResponsible.Padding = 6f;
            cellResponsible.PaddingTop = 4f;
            cellResponsible.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            table.AddCell(cellResponsible);
            cont++;
        }

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}DOCS/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}