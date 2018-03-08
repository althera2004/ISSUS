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

public partial class Export_DocumentExportList : Page
{
    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;
    public static Font fontAwe;

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(int companyId, string filter, string listOrder)
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
            Title = dictionary["Item_DocumentList"].ToUpperInvariant()
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
        float[] titleWidths = new float[] { 20f };
        titleTable.SetWidths(titleWidths);
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), titleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });


        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(6)
        {
            WidthPercentage = 100
        };
        float[] cirteriaWidths = new float[] { 10f, 20f, 12f, 30f, 10f, 80f };
        criteriatable.SetWidths(cirteriaWidths);

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Status"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        iTSpdf.PdfPCell criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_FieldLabel_Category"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        iTSpdf.PdfPCell criteria3Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Document_FieldLabel_Origin"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        string statusText = dictionary["Common_All_Male_Plural"];
        string category = dictionary["Common_All_Female_Plural"];
        string origin = dictionary["Common_All_Male_Plural"];
        if (filter.IndexOf("A|") != -1 )
        {
            statusText = dictionary["Common_Active_Plural"];
        }

        if (filter.StartsWith("I", StringComparison.OrdinalIgnoreCase))
        {
            statusText = dictionary["Common_Inactive_Plural"];
        }

        if (filter.StartsWith("|", StringComparison.OrdinalIgnoreCase))
        {
            statusText = dictionary["Common_None"];
        }

        ReadOnlyCollection<GisoFramework.Item.Document> documents = GisoFramework.Item.Document.GetByCompany(companyId);
        List<GisoFramework.Item.Document> data = new List<GisoFramework.Item.Document>();

        if (filter.IndexOf("A") != -1)
        {
            data = documents.Where(d => d.EndDate.HasValue == false).ToList();
        }

        if (filter.IndexOf("I") != -1)
        {
            data.AddRange(documents.Where(d => d.EndDate.HasValue == true).ToList());
        }

        string[] parts = filter.Split('|');
        if (parts[1] != "-1")
        {
            data = data.Where(d => d.Category.Id == Convert.ToInt32(parts[1])).ToList();
            ReadOnlyCollection<DocumentCategory> cats = DocumentCategory.GetByCompany(companyId);
            DocumentCategory cat = cats.Where(c => c.Id == Convert.ToInt32(parts[1])).First();
            category = cat.Description;
        }

        if (parts[2] == "0")
        {
            data = data.Where(d => d.Origin.Id == 0).ToList();
            origin = dictionary["Common_Internal"];
        }

        if (parts[2] == "1")
        {
            data = data.Where(d => d.Origin.Id > 0).ToList();
            origin = dictionary["Common_External"];
        }

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(category, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        iTSpdf.PdfPCell criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(origin, times))
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

        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(6)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1
        };
        
        //float[] widths = new float[] { 10f, 20f, 20f, 5f, 5f };
        float[] widths = new float[] { 20f, 5f, 15f, 15f, 10f, 5f };
        table.SetWidths(widths);

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Document_ListHeader_Name"], headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Document_ListHeader_Code"], headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Document_ListHeader_Category"], headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Document_ListHeader_Origin"], headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Document_ListHeader_Location"], headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Document_ListHeader_Revision"], headerFontFinal));

        switch (listOrder.ToUpperInvariant())
        {
            case "TH0|ASC":
                data = data.OrderBy(d => d.Description).ToList();
                break;
            case "TH0|DESC":
                data = data.OrderByDescending(d => d.Description).ToList();
                break;
            case "TH1|ASC":
                data = data.OrderBy(d => d.Code).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.Code).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.LastNumber).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.LastNumber).ToList();
                break;
        }
        
        int count = 0;
        foreach (GisoFramework.Item.Document document in data)
        {
            count++;
            table.AddCell(ToolsPdf.DataCell(document.Description, times));
            table.AddCell(ToolsPdf.DataCell(document.Code, times));
            table.AddCell(ToolsPdf.DataCell(document.Category.Description, times));
            table.AddCell(ToolsPdf.DataCell(document.Origin.Id == 0 ? dictionary["Common_Internal"] : dictionary["Common_External"], times));
            table.AddCell(ToolsPdf.DataCell(document.Location, times));
            table.AddCell(ToolsPdf.DataCellRight(document.LastNumber, times));
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            count);

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 4
        });

        pdfDoc.Add(criteriatable);
        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}