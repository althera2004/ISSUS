// --------------------------------
// <copyright file="DocumentExportList.aspx.cs" company="OpenFramework">
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
using PDF_Tests;

public partial class ExportDocumentExportList : Page
{
    public static Dictionary<string, string> Dictionary;

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(int companyId, string filter, string listOrder)
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

        var formatedDescription = ToolsPdf.NormalizeFileName(company.Name);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            Dictionary["Item_DocumentList"],
            formatedDescription,
            DateTime.Now);

        var pdfDoc = new iTS.Document(iTS.PageSize.A4.Rotate(), 40, 40, 80, 50);
        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, company.Id),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = company.Id,
            CompanyName = company.Name,
            Title = Dictionary["Item_DocumentList"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 20f });
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Dictionary["Item_EquipmentList"], company.Name), ToolsPdf.LayoutFonts.TitleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });

        var criteriatable = new iTSpdf.PdfPTable(6)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 10f, 20f, 12f, 30f, 10f, 80f });

        var criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Common_Status"] + " :", ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Item_Document_FieldLabel_Category"] + " :", ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        var criteria3Label = new iTSpdf.PdfPCell(new iTS.Phrase(Dictionary["Item_Document_FieldLabel_Origin"] + " :", ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        string statusText = Dictionary["Common_All_Male_Plural"];
        string category = Dictionary["Common_All_Female_Plural"];
        string origin = Dictionary["Common_All_Male_Plural"];
        if (filter.IndexOf("A|") != -1 )
        {
            statusText = Dictionary["Common_Active_Plural"];
        }

        if (filter.StartsWith("I", StringComparison.OrdinalIgnoreCase))
        {
            statusText = Dictionary["Common_Inactive_Plural"];
        }

        if (filter.StartsWith("|", StringComparison.OrdinalIgnoreCase))
        {
            statusText = Dictionary["Common_None"];
        }

        var documents = GisoFramework.Item.Document.ByCompany(companyId);
        var data = new List<GisoFramework.Item.Document>();

        // @alex: "A" indica que en el filtro se ha marcado "activos"
        if (filter.IndexOf("A") != -1)
        {
            data = documents.Where(d => d.EndDate.HasValue == false).ToList();
        }

        // @alex: "I" indica que en el filtro se ha marcado "inactivos"
        if (filter.IndexOf("I") != -1)
        {
            data.AddRange(documents.Where(d => d.EndDate.HasValue).ToList());
        }

        var parts = filter.Split('|');
        if (parts[1] != "-1")
        {
            data = data.Where(d => d.Category.Id == Convert.ToInt32(parts[1])).ToList();
            var cats = DocumentCategory.ByCompany(companyId);
            var cat = cats.First(c => c.Id == Convert.ToInt32(parts[1]));
            category = cat.Description;
        }

        if (parts[2] == "0")
        {
            data = data.Where(d => d.Origin.Id == 0).ToList();
            origin = Dictionary["Common_Internal"];
        }

        if (parts[2] == "1")
        {
            data = data.Where(d => d.Origin.Id > 0).ToList();
            origin = Dictionary["Common_External"];
        }

        criteriatable.AddCell(criteria1Label);
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(statusText, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });
        criteriatable.AddCell(criteria2Label);
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(category, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });
        criteriatable.AddCell(criteria3Label);
        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(origin, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        var table = new iTSpdf.PdfPTable(6)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1
        };

        table.SetWidths(new float[] { 20f, 5f, 15f, 15f, 10f, 5f });
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Document_ListHeader_Name"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Document_ListHeader_Code"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Document_ListHeader_Category"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Document_ListHeader_Origin"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Document_ListHeader_Location"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Document_ListHeader_Revision"]));

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
        foreach (var document in data)
        {
            count++;
            table.AddCell(ToolsPdf.DataCell(document.Description));
            table.AddCell(ToolsPdf.DataCell(document.Code));
            table.AddCell(ToolsPdf.DataCell(document.Category.Description));
            table.AddCell(ToolsPdf.DataCell(document.Origin.Id == 0 ? Dictionary["Common_Internal"] : Dictionary["Common_External"]));
            table.AddCell(ToolsPdf.DataCell(document.Location));
            table.AddCell(ToolsPdf.DataCell(document.LastNumber));
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            Dictionary["Common_RegisterCount"],
            count);

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Padding = 6f,
            PaddingTop = 4f
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = ToolsPdf.SummaryBackgroundColor,
            Colspan = 5
        });

        pdfDoc.Add(criteriatable);
        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}