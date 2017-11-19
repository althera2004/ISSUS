// --------------------------------
// <copyright file="DocumentExportList.aspx.cs" company="Sbrinna">
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class Export_DocumentExportList : Page
{
    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;
    public static Font fontAwe;

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(int companyId)
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
            dictionary["Item_DocumentList"],
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
            Title = dictionary["Item_DocumentList"].ToUpperInvariant()
        };

        pdfDoc.Open();

        iTS.BaseColor backgroundColor = new iTS.BaseColor(225, 225, 225);
        iTS.BaseColor rowPair = new iTS.BaseColor(255, 255, 255);
        iTS.BaseColor rowEven = new iTS.BaseColor(240, 240, 240);

        // ------------ FONTS 
        iTSpdf.BaseFont awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        iTS.Font times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        criteriaFont = new iTS.Font(arial, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font titleFont = new iTS.Font(arial, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

        var fontAwesomeIcon = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        fontAwe = new Font(fontAwesomeIcon, 10);
        // -------------------

        iTSpdf.PdfPTable titleTable = new iTSpdf.PdfPTable(1);
        float[] titleWidths = new float[] { 20f };
        titleTable.SetWidths(titleWidths);
        iTSpdf.PdfPCell titleCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), titleFont));
        titleCell.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        titleCell.Border = iTS.Rectangle.NO_BORDER;
        titleTable.AddCell(titleCell);

        var borderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;


        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(5);

        // actual width of table in points
        table.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 10f, 20f, 20f, 5f, 5f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 1;
        //leave a gap before and after the table
        //table.SpacingBefore = 20f;
        //table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerCode = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_List_Header_Code"].ToUpperInvariant(), headerFontFinal));
        headerCode.Border = borderAll;
        headerCode.BackgroundColor = backgroundColor;
        headerCode.Padding = 8f;
        headerCode.PaddingTop = 6f;
        headerCode.HorizontalAlignment = iTS.Element.ALIGN_LEFT;

        iTSpdf.PdfPCell headerDocument = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_List_Header_Document"].ToUpperInvariant(), headerFontFinal));
        headerDocument.Border = borderAll;
        headerDocument.BackgroundColor = backgroundColor;
        headerDocument.Padding = 8f;
        headerDocument.PaddingTop = 6f;
        headerDocument.HorizontalAlignment = iTS.Element.ALIGN_LEFT;

        iTSpdf.PdfPCell headerLocation = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_FieldLabel_Location"].ToUpperInvariant(), headerFontFinal));
        headerLocation.Border = borderAll;
        headerLocation.BackgroundColor = backgroundColor;
        headerLocation.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerLocation.Padding = 8f;
        headerLocation.PaddingTop = 6f;

        iTSpdf.PdfPCell headerSource = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_FieldLabel_Source"].ToUpperInvariant(), headerFontFinal));
        headerSource.Border = borderAll;
        headerSource.BackgroundColor = backgroundColor;
        headerSource.Padding = 8f;
        headerSource.PaddingTop = 6f;
        headerSource.HorizontalAlignment = iTS.Element.ALIGN_LEFT;

        iTSpdf.PdfPCell headerResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_List_Header_Responsible"].ToUpperInvariant(), headerFontFinal));
        headerResponsible.Border = borderAll;
        headerResponsible.BackgroundColor = backgroundColor;
        headerResponsible.Padding = 8f;
        headerResponsible.PaddingTop = 6f;
        headerResponsible.HorizontalAlignment = iTS.Element.ALIGN_LEFT;

        iTSpdf.PdfPCell headerVersionNumber = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_List_Header_Version"].ToUpperInvariant(), headerFontFinal));
        headerVersionNumber.Border = borderAll;
        headerVersionNumber.Padding = 8f;
        headerVersionNumber.PaddingTop = 6f;
        headerVersionNumber.BackgroundColor = backgroundColor;

        table.AddCell(headerDocument);
        table.AddCell(headerLocation);
        table.AddCell(headerResponsible);
        table.AddCell(headerCode);
        table.AddCell(headerVersionNumber);

        int cont = 0;
        ReadOnlyCollection<GisoFramework.Item.Document> data = GisoFramework.Item.Document.GetByCompany(companyId);
        bool pair = false;
        foreach (GisoFramework.Item.Document document in data)
        {
            int border = 0;

            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            iTSpdf.PdfPCell cellCode = new iTSpdf.PdfPCell(new iTS.Phrase(document.Code, times));
            cellCode.Border = border;
            cellCode.BackgroundColor = lineBackground;
            cellCode.Padding = 8f;
            cellCode.PaddingTop = 6f;

            iTSpdf.PdfPCell cellDescription = new iTSpdf.PdfPCell(new iTS.Phrase(document.Description, times));
            cellDescription.Border = border;
            cellDescription.BackgroundColor = lineBackground;
            cellDescription.Padding = 8f;
            cellDescription.PaddingTop = 6f;

            Employee emp = Employee.GetByUserId(document.LastVersion.User.Id);
            var responsable = document.LastVersion.UserCreateName;
            if(emp !=null && emp.Id > 0){
                responsable = emp.FullName;
            }

            iTSpdf.PdfPCell cellResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(responsable, times));
            cellResponsible.Border = border;
            cellResponsible.BackgroundColor = lineBackground;
            cellResponsible.Padding = 8f;
            cellResponsible.PaddingTop = 6f;

            string procedencia = dictionary["Common_Internal"];
            if (document.Source)
            {
                procedencia = document.Origin.Description;
            }

            iTSpdf.PdfPCell cellSource = new iTSpdf.PdfPCell(new iTS.Phrase(procedencia, times));
            cellSource.Border = border;
            cellSource.BackgroundColor = lineBackground;
            cellSource.Padding = 8f;
            cellSource.PaddingTop = 6f;

            iTSpdf.PdfPCell dateCell = new iTSpdf.PdfPCell(new iTS.Phrase(document.Location, times));
            dateCell.Border = border;
            dateCell.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            dateCell.BackgroundColor = lineBackground;
            dateCell.Padding = 8f;
            dateCell.PaddingTop = 6f;

            iTSpdf.PdfPCell cellVersionNumber = new iTSpdf.PdfPCell(new iTS.Phrase(document.LastVersionNumber().ToString(), times));
            cellVersionNumber.Border = border;
            cellVersionNumber.HorizontalAlignment = iTS.Element.ALIGN_RIGHT;
            cellVersionNumber.BackgroundColor = lineBackground;
            cellVersionNumber.Padding = 8f;
            cellVersionNumber.PaddingTop = 6f;

            table.AddCell(cellDescription);
            table.AddCell(dateCell);
            table.AddCell(cellResponsible);
            table.AddCell(cellCode);
            table.AddCell(cellVersionNumber);

            cont++;
        }

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}DOCS/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}