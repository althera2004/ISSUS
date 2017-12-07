// --------------------------------
// <copyright file="BusinessRiskExportList.aspx.cs" company="Sbrinna">
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
using System.Linq;
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

public partial class Export_BusinessRiskExportList : Page
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
        long rulesId,
        long processId,
        int typeId)
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
            dictionary["Item_BusinessRisks"],
            company.Name,
            DateTime.Now);

        #region Fonts
        string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        BaseFont headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        BaseFont arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        iTSpdf.BaseFont awesomeFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        iTS.Font times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        criteriaFont = new iTS.Font(arial, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font titleFont = new iTS.Font(arial, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font symbolFont = new iTS.Font(awesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var fontAwesomeIcon = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\fontawesome-webfont.ttf", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        fontAwe = new Font(fontAwesomeIcon, 10);
        #endregion

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
            Title = dictionary["Item_BusinessRisks"].ToUpperInvariant()
        };

        pdfDoc.Open();

        iTS.BaseColor backgroundColor = new iTS.BaseColor(225, 225, 225);
        iTS.BaseColor rowPair = new iTS.BaseColor(255, 255, 255);
        iTS.BaseColor rowEven = new iTS.BaseColor(240, 240, 240);


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

        //------ CRITERIA
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(4);
        float[] cirteriaWidths = new float[] { 10f, 50f, 10f, 100f };
        criteriatable.SetWidths(cirteriaWidths);
        criteriatable.WidthPercentage = 100;

        #region texts

        string criteriaProccess = dictionary["Common_All_Male_Plural"];
        if (processId > 0)
        {
            Process process = new Process(processId, companyId);
            if (!string.IsNullOrEmpty(process.Description))
            {
                criteriaProccess = process.Description;
            }
        }

        string periode = string.Empty;
        if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
        {
            periode = dictionary["Item_Incident_List_Filter_From"] + " " + from;
        }
        else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = dictionary["Item_Incident_List_Filter_To"] + " " + to;
        }
        else if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = from + " - " + to;
        }
        else
        {
            periode = dictionary["Common_All_Male"];
        }

        string typetext = dictionary["Common_All_Male_Plural"];
        if (typeId == 1) { typetext = dictionary["Item_BusinessRisk_Status_Assumed"]; }
        if (typeId == 2) { typetext = dictionary["Item_BusinessRisk_Status_Significant"]; }
        if (typeId == 3) { typetext = dictionary["Item_BusinessRisk_Status_NotSignificant"]; }
        if (typeId == 4) { typetext = dictionary["Item_BusinessRisk_Status_Unevaluated"]; }

        string ruleDescription = dictionary["Common_All_Female_Plural"];
        if (rulesId > 0)
        {
            Rules rule = Rules.GetById(companyId, rulesId);
            if (!string.IsNullOrEmpty(rule.Description))
            {
                ruleDescription = rule.Description;
            }
        }
        #endregion

        iTSpdf.PdfPCell criteria1Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"] + " :", timesBold));
        criteria1Label.Border = borderNone;
        criteria1Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1Label.Padding = 6f;
        criteria1Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria1 = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times));
        criteria1.Border = borderNone;
        criteria1.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria1.Padding = 6f;
        criteria1.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria2Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_Process"] + " :", timesBold));
        criteria2Label.Border = borderNone;
        criteria2Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2Label.Padding = 6f;
        criteria2Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria2 = new iTSpdf.PdfPCell(new iTS.Phrase(typetext, times));
        criteria2.Border = borderNone;
        criteria2.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria2.Padding = 6f;
        criteria2.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria3Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_Rule"] + " :", timesBold));
        criteria3Label.Border = borderNone;
        criteria3Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria3Label.Padding = 6f;
        criteria3Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria3 = new iTSpdf.PdfPCell(new iTS.Phrase(ruleDescription, times));
        criteria3.Border = borderNone;
        criteria3.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria3.Padding = 6f;
        criteria3.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria4Label = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_Type"] + " :", timesBold));
        criteria4Label.Border = borderNone;
        criteria4Label.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteria4Label.Padding = 6f;
        criteria4Label.PaddingTop = 4f;

        iTSpdf.PdfPCell criteria4 = new iTSpdf.PdfPCell(new iTS.Phrase(criteriaProccess, times));
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

        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(7);
        table.WidthPercentage = 100;
        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 10f, 10f, 30f, 20f, 20f, 10f, 10f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 1;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerDescripcion = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_Description"].ToUpperInvariant(), headerFontFinal));
        headerDescripcion.Border = borderAll;
        headerDescripcion.BackgroundColor = backgroundColor;
        headerDescripcion.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerDescripcion.Padding = 8f;
        headerDescripcion.PaddingTop = 6f;

        iTSpdf.PdfPCell headerType = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Type"].ToUpperInvariant(), headerFontFinal));
        headerType.Border = borderAll;
        headerType.BackgroundColor = backgroundColor;
        headerType.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerType.Padding = 8f;
        headerType.PaddingTop = 6f;

        iTSpdf.PdfPCell headerOpen = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_Date"].ToUpperInvariant(), headerFontFinal));
        headerOpen.Border = borderAll;
        headerOpen.BackgroundColor = backgroundColor;
        headerOpen.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerOpen.Padding = 8f;
        headerOpen.PaddingTop = 6f;

        iTSpdf.PdfPCell headerProcess = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_Process"].ToUpperInvariant(), headerFontFinal));
        headerProcess.Border = borderAll;
        headerProcess.BackgroundColor = backgroundColor;
        headerProcess.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerProcess.Padding = 8f;
        headerProcess.PaddingTop = 6f;

        iTSpdf.PdfPCell headerRule = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_Rule"].ToUpperInvariant(), headerFontFinal));
        headerRule.Border = borderAll;
        headerRule.BackgroundColor = backgroundColor;
        headerRule.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerRule.Padding = 8f;
        headerRule.PaddingTop = 6f;

        iTSpdf.PdfPCell headerActualValue = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_StartValue"].ToUpperInvariant(), headerFontFinal));
        headerActualValue.Border = borderAll;
        headerActualValue.BackgroundColor = backgroundColor;
        headerActualValue.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerActualValue.Padding = 8f;
        headerActualValue.PaddingTop = 6f;

        iTSpdf.PdfPCell headerIpr = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_BusinesRisk_ListHeader_IPR"].ToUpperInvariant(), headerFontFinal));
        headerIpr.Border = borderAll;
        headerIpr.BackgroundColor = backgroundColor;
        headerIpr.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
        headerIpr.Padding = 8f;
        headerIpr.PaddingTop = 6f;

        table.AddCell(headerType);
        table.AddCell(headerOpen);
        table.AddCell(headerDescripcion);
        table.AddCell(headerProcess);
        table.AddCell(headerRule);
        table.AddCell(headerActualValue);
        table.AddCell(headerIpr);

        int cont = 0;
        List<BusinessRiskFilterItem> data = HttpContext.Current.Session["BusinessRiskFilterData"] as List<BusinessRiskFilterItem>;
        bool pair = false;
        foreach (BusinessRiskFilterItem risk in data)
        {
            int border = 0;

            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            iTSpdf.PdfPCell cellDescription = new iTSpdf.PdfPCell(new iTS.Phrase(risk.Description, times));
            cellDescription.Border = border;
            cellDescription.BackgroundColor = lineBackground;
            cellDescription.Padding = 6f;
            cellDescription.PaddingTop = 4f;

            iTSpdf.PdfPCell processCell = new iTSpdf.PdfPCell(new iTS.Phrase(risk.Process.Description, times));
            processCell.Border = border;
            processCell.BackgroundColor = lineBackground;
            processCell.Padding = 6f;
            processCell.PaddingTop = 4f;

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", risk.OpenDate), times));
            cellDate.Border = border;
            cellDate.BackgroundColor = lineBackground;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            cellDate.HorizontalAlignment = Rectangle.ALIGN_CENTER;

            iTSpdf.PdfPCell ruleCell = new iTSpdf.PdfPCell(new iTS.Phrase(risk.Rule.Description, times));
            ruleCell.Border = border;
            ruleCell.BackgroundColor = lineBackground;
            ruleCell.Padding = 6f;
            ruleCell.PaddingTop = 4f;
            ruleCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

            string typeText = string.Empty;
            if (risk.Assumed) { typeText = dictionary["Item_BusinessRisk_Status_Assumed"]; }
            else if (risk.InitialResult == 0) { typeText = dictionary["Item_BusinessRisk_Status_Unevaluated"]; }
            else if (risk.InitialResult < risk.Rule.Limit) { typeText = dictionary["Item_BusinessRisk_Status_NotSignificant"]; }
            else { typeText = dictionary["Item_BusinessRisk_Status_Significant"]; }

            iTSpdf.PdfPCell cellType = new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times));
            cellType.Border = border;
            cellType.BackgroundColor = lineBackground;
            cellType.Padding = 6f;
            cellType.PaddingTop = 4f;
            cellType.HorizontalAlignment = Rectangle.ALIGN_CENTER;

            iTSpdf.PdfPCell cellActualValue = new iTSpdf.PdfPCell(new iTS.Phrase(risk.InitialResult.ToString(), times));
            cellActualValue.Border = border;
            cellActualValue.BackgroundColor = lineBackground;
            cellActualValue.Padding = 6f;
            cellActualValue.PaddingTop = 4f;
            cellActualValue.HorizontalAlignment = Rectangle.ALIGN_CENTER;

            iTSpdf.PdfPCell cellIpr = new iTSpdf.PdfPCell(new iTS.Phrase(risk.Rule.Limit.ToString(), times));
            cellIpr.Border = border;
            cellIpr.BackgroundColor = lineBackground;
            cellIpr.Padding = 6f;
            cellIpr.PaddingTop = 4f;
            cellIpr.HorizontalAlignment = Rectangle.ALIGN_CENTER;

            table.AddCell(cellType);
            table.AddCell(cellDate);
            table.AddCell(cellDescription);
            table.AddCell(processCell);
            table.AddCell(ruleCell);
            table.AddCell(cellActualValue);
            table.AddCell(cellIpr);

            cont++;
        }

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}