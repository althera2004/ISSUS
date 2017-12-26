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

public partial class Export_EquipmentList : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;
    public static Font fontAwe;

    private static string GetFilter()
    {
        if (HttpContext.Current.Session["EquipmentFilter"] == null)
        {
            return "CVM|0";
        }

        return HttpContext.Current.Session["EquipmentFilter"].ToString().ToUpperInvariant();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(int companyId, string listOrder)
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


        float[] widths = new float[] { 20f, 10f, 5f, 15f };
        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };
        table.SetWidths(widths);

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Code"].ToUpperInvariant() + " - " + dictionary["Item_Equipment_Header_Description"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Location"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Cost"].ToUpperInvariant(), headerFontFinal));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Responsible"].ToUpperInvariant(), headerFontFinal));

        int cont = 0;
        List<Equipment> data = Equipment.GetList(companyId).ToList();

        string filter = GetFilter();
        if (filter.IndexOf("1", StringComparison.OrdinalIgnoreCase) != -1)
        {
            data = data.Where(d => !d.EndDate.HasValue).ToList();
        }

        if (filter.IndexOf("2", StringComparison.OrdinalIgnoreCase) != -1)
        {
            data = data.Where(d => d.EndDate.HasValue).ToList();
        }

        List<Equipment> dataC = new List<Equipment>();
        if (filter.IndexOf("C", StringComparison.OrdinalIgnoreCase) != -1)
        {
            dataC = data.Where(d => d.IsCalibration == true).ToList();
        }

        List<Equipment> dataV = new List<Equipment>();
        if (filter.IndexOf("V", StringComparison.OrdinalIgnoreCase) != -1)
        {
            dataV = data.Where(d => d.IsVerification == true).ToList();
        }

        List<Equipment> dataM = new List<Equipment>();
        if (filter.IndexOf("M", StringComparison.OrdinalIgnoreCase) != -1)
        {
            dataM = data.Where(d => d.IsMaintenance == true).ToList();
        }

        data = dataC;
        foreach (Equipment equipmentV in dataV)
        {
            if (!data.Any(d => d.Id == equipmentV.Id))
            {
                data.Add(equipmentV);
            }
        }

        foreach (Equipment equipmentM in dataM)
        {
            if (!data.Any(d => d.Id == equipmentM.Id))
            {
                data.Add(equipmentM);
            }
        }

        // aplicar filtros

        //------ CRITERIA
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(2);
        float[] cirteriaWidths = new float[] { 8f, 50f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;


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

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_List_Filter_ShowByOperation"] + " :", timesBold));
        criteria1Label.Border = borderNone;
        criteria1Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1Label.Padding = 6f;
        criteria1Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(operativaText, times));
        criteria1.Border = borderNone;
        criteria1.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1.Padding = 6f;
        criteria1.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_List_Filter_ShowByStatus"] + " :", timesBold));
        criteria2Label.Border = borderNone;
        criteria2Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2Label.Padding = 6f;
        criteria2Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times));
        criteria2.Border = borderNone;
        criteria2.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2.Padding = 6f;
        criteria2.PaddingTop = 4f;

        criteriatable.AddCell(criteria1Label);
        criteriatable.AddCell(criteria1);
        criteriatable.AddCell(criteria2Label);
        criteriatable.AddCell(criteria2);

        bool pair = false;
        decimal total = 0;
        int border = 0;

        if (!string.IsNullOrEmpty(listOrder))
        {
            switch (listOrder.ToUpperInvariant())
            {
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

            BaseColor lineBackground = pair ? rowEven : rowPair;

            iTSpdf.PdfPCell typeCell = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Code + " - " + equipment.Description, times));
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

            string totalCost = string.Format("{0:#,##0.00}", equipment.TotalCost);
            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(totalCost, times));
            cellDate.Border = border;
            cellDate.BackgroundColor = lineBackground;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            cellDate.HorizontalAlignment = 2;
            table.AddCell(cellDate);

            iTSpdf.PdfPCell responsibleCell = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Responsible.FullName, times));
            responsibleCell.Border = border;
            responsibleCell.BackgroundColor = lineBackground;
            responsibleCell.Padding = 6f;
            responsibleCell.PaddingTop = 4f;
            table.AddCell(responsibleCell);

            cont++;
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont);
        iTSpdf.PdfPCell totalRegistrosCell = new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, times));
        totalRegistrosCell.Border = iTS.Rectangle.TOP_BORDER;
        totalRegistrosCell.BackgroundColor = rowEven;
        totalRegistrosCell.Padding = 6f;
        totalRegistrosCell.PaddingTop = 4f;
        table.AddCell(totalRegistrosCell);

        iTSpdf.PdfPCell totalLabelCell = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), times));
        totalLabelCell.Border = iTS.Rectangle.TOP_BORDER;
        totalLabelCell.BackgroundColor = rowEven;
        totalLabelCell.Padding = 6f;
        totalLabelCell.PaddingTop = 4f;
        totalLabelCell.HorizontalAlignment = 2;
        table.AddCell(totalLabelCell);

        string totalText = string.Format("{0:#,##0.00}", total);
        iTSpdf.PdfPCell totalCell = new iTSpdf.PdfPCell(new iTS.Phrase(totalText, times));
        totalCell.Border = iTS.Rectangle.TOP_BORDER;
        totalCell.BackgroundColor = rowEven;
        totalCell.Padding = 6f;
        totalCell.PaddingTop = 4f;
        totalCell.HorizontalAlignment = 2;
        table.AddCell(totalCell);

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 2
        });

        pdfDoc.Add(criteriatable);
        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}