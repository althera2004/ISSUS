// --------------------------------
// <copyright file="FormacionExportList.aspx.cs" company="Sbrinna">
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

public partial class ExportFormacionExportList : Page
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
    public static ActionResult PDF(int companyId, string yearFrom, string yearTo, int mode, string listOrder)
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

        // le damos nombre al fichero final item_company_fecha.pdf
        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_LearningList"],
            company.Code,
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

        // evento para poner titulo y pie de página cada vez que salta de página
        writer.PageEvent = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, company.Id),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = company.Id,
            CompanyName = company.Name,
            Title = dictionary["Item_LearningList"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(225, 225, 225);
        var rowPair = new iTS.BaseColor(255, 255, 255);
        var rowEven = new iTS.BaseColor(240, 240, 240);

        // ------------ FONTS 
        var awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        criteriaFont = new iTS.Font(arial, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var titleFont = new iTS.Font(arial, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);

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

        //------ CRITERIA
        var criteriatable = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100
        };
        criteriatable.SetWidths(new float[] { 3f, 12f, 3f, 22f });

        string periode = string.Empty;
        if (!string.IsNullOrEmpty(yearFrom) && yearFrom != "0" && !string.IsNullOrEmpty(yearTo) && yearTo != "0")
        {
            periode = yearFrom + " - " + yearTo;
        }
        else if (!string.IsNullOrEmpty(yearFrom) && yearFrom != "0")
        {
            periode = dictionary["Common_From"] + " " + yearFrom;
        }
        else if (!string.IsNullOrEmpty(yearTo) && yearTo != "0")
        {
            periode = dictionary["Item_Incident_List_Filter_To"] + " " + yearTo;
        }

        if (string.IsNullOrEmpty(periode))
        {
            periode = dictionary["Item_Learning_Filter_AllPeriode"];
        };

        string modeText = dictionary["Common_All_Female_Plural"];
        if (mode == 0) { modeText = dictionary["Item_Learning_Status_InProgress"]; }
        if (mode == 1) { modeText = dictionary["Item_Learning_Status_Finished"]; }
        if (mode == 2) { modeText = dictionary["Item_Learning_Status_Evaluated"]; }        

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Status"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(modeText, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        pdfDoc.Add(criteriatable);

        var table = new iTSpdf.PdfPTable(5)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 15f, 5f, 5f, 5f, 5f });
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Course"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_ListHeader_DateComplete"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Cost"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_EstimatedDate"]));
        int cont = 0;

        DateTime? yFrom = null;
        DateTime? yTo = null;

        if (!string.IsNullOrEmpty(yearFrom) && yearFrom !="0")
        {
            yFrom = Tools.TextToDate(yearFrom);
        }

        if (!string.IsNullOrEmpty(yearTo) && yearTo != "0")
        {
            yTo = Tools.TextToDate(yearTo);
        }

        var learningFilter = new LearningFilter(companyId) { Mode = mode, YearFrom = yFrom, YearTo = yTo };
        decimal totalCost = 0;
        int count = 0;

        var data = learningFilter.Filter().ToList();

        if (!string.IsNullOrEmpty(listOrder))
        {
            switch (listOrder.ToUpperInvariant())
            {
                default:
                case "TH0|ASC":
                    data = data.OrderBy(d => d.Description).ToList();
                    break;
                case "TH0|DESC":
                    data = data.OrderByDescending(d => d.Description).ToList();
                    break;
            }
        }

        foreach (var learning in data)
        {
            count++;
            learning.ObtainAssistance();
            string assist = string.Empty;
            bool first = true;
            foreach (var alumno in learning.Assistance)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    assist += ", ";
                }

                assist += alumno.Employee.FullName;
            }

            if (string.IsNullOrEmpty(assist))
            {
                assist = "(" + dictionary["Item_LearningList_NoAssistants"] + ")";
            }

            int border = 0;
            table.AddCell(ToolsPdf.DataCell(learning.Description,times));
            table.AddCell(ToolsPdf.DataCellCenter(learning.RealFinish, times));
            table.AddCell(ToolsPdf.DataCellMoney(learning.Amount, times));
            totalCost += learning.Amount;

            string statusText = string.Empty;
            switch (learning.Status)
            {
                default:
                case 0: statusText = dictionary["Item_Learning_Status_InProgress"]; break;
                case 1: statusText = dictionary["Item_Learning_Status_Finished"]; break;
                case 2: statusText = dictionary["Item_Learning_Status_Evaluated"]; break;
            }

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times))
            {
                Border = border,
                Padding = ToolsPdf.PaddingTableCell,
                PaddingTop = ToolsPdf.PaddingTopTableCell
            });

            string fecha = Tools.TranslatedMonth(learning.DateEstimated.Month, dictionary) + " " + learning.DateEstimated.Year;
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(fecha, times))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
            cont++;
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

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        string totalText = string.Format("{0:#,##0.00}", totalCost);
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalText, times))
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