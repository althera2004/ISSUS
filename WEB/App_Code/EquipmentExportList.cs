using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text.pdf;
using PDF_Tests;
using System.Globalization;
using System.Configuration;
using System.IO;

/// <summary>
/// Descripción breve de EquipmentExportList
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentExportList : System.Web.Services.WebService
{

    public EquipmentExportList()
    {
    }

    public static Dictionary<string, string> dictionary;

    private static string GetFilter()
    {
        if (HttpContext.Current.Session["EquipmentFilter"] == null)
        {
            return "CVM|0";
        }

        return HttpContext.Current.Session["EquipmentFilter"].ToString().ToUpperInvariant();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult PDF(int companyId, string listOrder)
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
            Title = dictionary["Item_EquipmentList"].ToUpperInvariant()
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
            Border = iTS.Rectangle.NO_BORDER
        };

        titleTable.AddCell(titleCell);

        var table = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };
        table.SetWidths(new float[] { 20f, 10f, 5f, 15f });

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Code"] + " - " + dictionary["Item_Equipment_Header_Description"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Location"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Cost"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Responsible"]));

        int cont = 0;
        var data = Equipment.ByCompany(companyId).ToList();

        string filter = GetFilter();
        if (filter.IndexOf("1", StringComparison.OrdinalIgnoreCase) != -1)
        {
            data = data.Where(d => !d.EndDate.HasValue).ToList();
        }

        if (filter.IndexOf("2", StringComparison.OrdinalIgnoreCase) != -1)
        {
            data = data.Where(d => d.EndDate.HasValue).ToList();
        }

        var dataC = new List<Equipment>();
        if (filter.IndexOf("C", StringComparison.OrdinalIgnoreCase) != -1)
        {
            dataC = data.Where(d => d.IsCalibration).ToList();
        }

        var dataV = new List<Equipment>();
        if (filter.IndexOf("V", StringComparison.OrdinalIgnoreCase) != -1)
        {
            dataV = data.Where(d => d.IsVerification).ToList();
        }

        var dataM = new List<Equipment>();
        if (filter.IndexOf("M", StringComparison.OrdinalIgnoreCase) != -1)
        {
            dataM = data.Where(d => d.IsMaintenance).ToList();
        }

        data = dataC;
        foreach (var equipmentV in dataV)
        {
            if (!data.Any(d => d.Id == equipmentV.Id))
            {
                data.Add(equipmentV);
            }
        }

        foreach (var equipmentM in dataM)
        {
            if (!data.Any(d => d.Id == equipmentM.Id))
            {
                data.Add(equipmentM);
            }
        }

        // aplicar filtros

        //------ CRITERIA
        var criteriatable = new iTSpdf.PdfPTable(2)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 8f, 50f });

        string statusText = string.Empty;
        string operativaText = string.Empty;

        if (filter.IndexOf("0") != -1)
        {
            statusText = dictionary["Common_All"];
        }
        else if (filter.IndexOf("1") != -1)
        {
            statusText = dictionary["Item_Equipment_List_Filter_ShowActive"];
        }
        else if (filter.IndexOf("2") != -1)
        {
            statusText = dictionary["Item_Equipment_List_Filter_ShowClosed"];
        }

        bool first = true;
        if (filter.IndexOf("C") != -1)
        {
            first = false;
            operativaText = dictionary["Item_Equipment_List_Filter_ShowCalibration"];
        }
        if (filter.IndexOf("V") != -1)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_List_Filter_ShowVerification"];
        }
        if (filter.IndexOf("M") != -1)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_List_Filter_ShowMaintenance"];
        }

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_List_Filter_ShowByOperation"] + " :", ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(operativaText, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_List_Filter_ShowByStatus"] + " :", ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(statusText, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        bool pair = false;
        decimal total = 0;
        int border = 0;

        if (!string.IsNullOrEmpty(listOrder))
        {
            switch (listOrder.ToUpperInvariant())
            {
                default:
                case "TH0|ASC":
                    data = data.OrderBy(d => d.Code).ToList();
                    break;
                case "TH0|DESC":
                    data = data.OrderByDescending(d => d.Code).ToList();
                    break;
                case "TH1|ASC":
                    data = data.OrderBy(d => d.Location).ToList();
                    break;
                case "TH1|DESC":
                    data = data.OrderByDescending(d => d.Location).ToList();
                    break;
                case "TH2|ASC":
                    data = data.OrderBy(d => d.Responsible.FullName).ToList();
                    break;
                case "TH2|DESC":
                    data = data.OrderByDescending(d => d.Responsible.FullName).ToList();
                    break;
                case "TH3|ASC":
                    data = data.OrderBy(d => d.TotalCost).ToList();
                    break;
                case "TH3|DESC":
                    data = data.OrderByDescending(d => d.TotalCost).ToList();
                    break;
            }
        }

        foreach (Equipment equipment in data)
        {
            total += equipment.TotalCost;

            var lineBackground = pair ? rowEven : rowPair;

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Code + " - " + equipment.Description, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f
            });

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Location, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f
            });

            string totalCost = string.Format("{0:#,##0.00}", equipment.TotalCost);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalCost, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Responsible.FullName, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f
            });

            cont++;
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont);
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        string totalText = string.Format("{0:#,##0.00}", total);
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalText, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 2
        });

        pdfDoc.Add(criteriatable);
        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }

}
