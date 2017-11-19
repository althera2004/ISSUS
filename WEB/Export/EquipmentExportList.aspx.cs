// --------------------------------
// <copyright file="EquipmentList.aspx.cs" company="Sbrinna">
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

public partial class Export_EquipmentList : Page
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
            dictionary["Item_EquipmentList"],
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
            Title = dictionary["Item_EquipmentList"].ToUpperInvariant()
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

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 10f, 20f, 15f, 15f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 1;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerCode = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Header_Code"].ToUpperInvariant(), headerFontFinal));
        headerCode.Border = borderAll;
        headerCode.BackgroundColor = backgroundColor;
        headerCode.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerCode.Padding = 8f;
        headerCode.PaddingTop = 6f;

        iTSpdf.PdfPCell headerDescription = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Header_Description"].ToUpperInvariant(), headerFontFinal));
        headerDescription.Border = borderAll;
        headerDescription.BackgroundColor = backgroundColor;
        headerDescription.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerDescription.Padding = 8f;
        headerDescription.PaddingTop = 6f;

        iTSpdf.PdfPCell headerLocation = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Header_Location"].ToUpperInvariant(), headerFontFinal));
        headerLocation.Border = borderAll;
        headerLocation.BackgroundColor = backgroundColor;
        headerLocation.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerLocation.Padding = 8f;
        headerLocation.PaddingTop = 6f;
        
        iTSpdf.PdfPCell headerResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Header_Responsible"].ToUpperInvariant(), headerFontFinal));
        headerResponsible.Border = borderAll;
        headerResponsible.BackgroundColor = backgroundColor;
        headerResponsible.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerResponsible.Padding = 8f;
        headerResponsible.PaddingTop = 6f;

        table.AddCell(headerCode);
        table.AddCell(headerDescription);
        table.AddCell(headerLocation);
        table.AddCell(headerResponsible);

        int cont = 0;
        ReadOnlyCollection<Equipment> data = Equipment.GetList(companyId);
        bool pair = false;
        foreach (Equipment equipment in data)
        {
            int border = 0;

            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Code, times));
            cellDate.Border = border;
            cellDate.BackgroundColor = lineBackground;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            table.AddCell(cellDate);

            iTSpdf.PdfPCell typeCell = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Description, times));
            typeCell.Border = border;
            typeCell.BackgroundColor = lineBackground;
            typeCell.Padding = 6f;
            typeCell.PaddingTop = 4f;
            table.AddCell(typeCell);

            iTSpdf.PdfPCell operationCell = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Location, times));
            operationCell.Border = border;
            operationCell.BackgroundColor = lineBackground;
            operationCell.Padding = 6f;
            operationCell.PaddingTop = 4f;
            table.AddCell(operationCell);

            iTSpdf.PdfPCell responsibleCell = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Responsible.FullName, times));
            responsibleCell.Border = border;
            responsibleCell.BackgroundColor = lineBackground;
            responsibleCell.Padding = 6f;
            responsibleCell.PaddingTop = 4f;
            table.AddCell(responsibleCell);

            cont++;
        }

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}DOCS/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}