// --------------------------------
// <copyright file="FormacionExportList.aspx.cs" company="Sbrinna">
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
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using PDF_Tests;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;

public partial class Export_FormacionExportList : Page
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
            dictionary["Item_LearningList"],
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
            Title = dictionary["Item_LearningList"].ToUpperInvariant()
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
        iTS.Font timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);

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
        
        //------ CRITERIA
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(4);
        float[] cirteriaWidths = new float[] { 3f, 12f, 3f, 22f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

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
        }

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        iTSpdf.PdfPCell criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Status"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        };

        string modeText = dictionary["Common_All_Female_Plural"];
        if (mode == 0) { modeText = dictionary["Item_Learning_Status_InProgress"]; }
        if (mode == 1) { modeText = dictionary["Item_Learning_Status_Finished"]; }
        if (mode == 2) { modeText = dictionary["Item_Learning_Status_Evaluated"]; }

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(modeText, times))
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

        pdfDoc.Add(criteriatable);

        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(5)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 15f, 20f, 5f, 5f, 5f };
        table.SetWidths(widths);
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Course"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_LearningAssistants"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Cost"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Status"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_EstimatedDate"].ToUpperInvariant(), headerFontFinal));
        int cont = 0;

        int? yFrom=null;
        int? yTo=null;

        if (!string.IsNullOrEmpty(yearFrom) && yearFrom !="0")
        {
            yFrom = Convert.ToInt32(yearFrom);
        }

        if (!string.IsNullOrEmpty(yearTo) && yearTo != "0")
        {
            yTo = Convert.ToInt32(yearTo);
        }

        LearningFilter learningFilter = new LearningFilter(companyId) { Mode = mode, YearFrom = yFrom, YearTo = yTo };
        decimal totalCost = 0;
        int count = 0;

        List<Learning> data = learningFilter.Filter().ToList();

        // cambio de ahora
        if (!string.IsNullOrEmpty(listOrder))
        {
            switch (listOrder.ToUpperInvariant())
            {
                case "TH0|ASC":
                    data = data.OrderBy(d => d.Description).ToList();
                    break;
                case "TH0|DESC":
                    data = data.OrderByDescending(d => d.Description).ToList();
                    break;
            }
        }

        foreach (Learning learning in data)
        {
            count++;
            learning.ObtainAssistance();
            string assist = string.Empty;
            bool first = true;
            foreach (LearningAssistance alumno in learning.Assistance)
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

            // pair = !pair;

            iTSpdf.PdfPCell courseCell = new iTSpdf.PdfPCell(new iTS.Phrase(learning.Description, times))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 4f
            };
            table.AddCell(courseCell);

            iTSpdf.PdfPCell assistantsCell = new iTSpdf.PdfPCell(new iTS.Phrase(assist, times))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 4f
            };
            table.AddCell(assistantsCell);

            string costText = string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", learning.Amount);
            iTSpdf.PdfPCell costCell = new iTSpdf.PdfPCell(new Phrase(costText, times))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            };
            table.AddCell(costCell);
            totalCost += learning.Amount;

            string statusText = string.Empty;
            switch (learning.Status)
            {
                case 0: statusText = dictionary["Item_Learning_Status_InProgress"]; break;
                case 1: statusText = dictionary["Item_Learning_Status_Finished"]; break;
                case 2: statusText = dictionary["Item_Learning_Status_Evaluated"]; break;
            }

            iTSpdf.PdfPCell statusCell = new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 4f
            };

            table.AddCell(statusCell);

            string fecha = Tools.TranslatedMonth(learning.DateEstimated.Month, dictionary) + " " + learning.DateEstimated.Year.ToString();

            iTSpdf.PdfPCell dateCell = new iTSpdf.PdfPCell(new iTS.Phrase(fecha, times))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            };

            table.AddCell(dateCell);
            cont++;
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            count);
        iTSpdf.PdfPCell totalRegistrosCell = new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f
        };
        table.AddCell(totalRegistrosCell);

        iTSpdf.PdfPCell totalLabelCell = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        };
        table.AddCell(totalLabelCell);

        string totalText = string.Format("{0:#,##0.00}", totalCost);
        iTSpdf.PdfPCell totalCell = new iTSpdf.PdfPCell(new iTS.Phrase(totalText, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        };
        table.AddCell(totalCell);

        iTSpdf.PdfPCell blankCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 2
        };
        table.AddCell(blankCell);

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}