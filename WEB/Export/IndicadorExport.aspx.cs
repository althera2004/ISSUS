// --------------------------------
// <copyright file="IndicadorExportList.aspx.cs" company="Sbrinna">
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

public partial class Export_IndicadorExport : Page
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
        int indicatorType,
        DateTime? from, 
        DateTime? to,
        int? processId,
        int? processTypeId,
        int? targetId,
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
            dictionary["Item_Indicador_List"],
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
            Title = dictionary["Item_Indicador_List"].ToUpperInvariant()
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


        iTSpdf.PdfPTable table = new iTSpdf.PdfPTable(5);

        // actual width of table in points
        table.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //------ CRITERIA
        iTSpdf.PdfPTable criteriatable = new iTSpdf.PdfPTable(6);
        float[] cirteriaWidths = new float[] { 20f, 50f, 20f, 50f, 20f, 150f };
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
            periode = string.Format(CultureInfo.InvariantCulture,@"{0:dd/MM/yyyy} {1:dd/MM/yyyy}", from, to);
        }
        else
        {
            periode = dictionary["Common_All_Male"];
        }        

        iTSpdf.PdfPCell criteriaPeriodeLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold));
        criteriaPeriodeLabel.Border = borderNone;
        criteriaPeriodeLabel.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteriaPeriodeLabel.Padding = 6f;
        criteriaPeriodeLabel.PaddingTop = 4f;
        criteriatable.AddCell(criteriaPeriodeLabel);

        iTSpdf.PdfPCell criteriaPeriode = new iTSpdf.PdfPCell(new iTS.Phrase(periode, times));
        criteriaPeriode.Border = borderNone;
        criteriaPeriode.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteriaPeriode.Padding = 6f;
        criteriaPeriode.PaddingTop = 4f;
        criteriatable.AddCell(criteriaPeriode);

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
        criteriatable.AddCell(criteriaBlank);

        iTSpdf.PdfPCell criteriaIndicadorTypeLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicaddor_Filter_TypeLabel"] + " :", timesBold));
        criteriaIndicadorTypeLabel.Border = borderNone;
        criteriaIndicadorTypeLabel.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteriaIndicadorTypeLabel.Padding = 6f;
        criteriaIndicadorTypeLabel.PaddingTop = 4f;
        criteriatable.AddCell(criteriaIndicadorTypeLabel);

        string indicadorTypeText = dictionary["Common_All_Male_Plural"];
        switch (indicatorType)
        {
            case 0:
                indicadorTypeText = dictionary["Common_All_Male_Plural"];
                break;
            case 1:
                indicadorTypeText = dictionary["Item_Indicaddor_Filter_TypeProcess"];
                break;
            case 2:
                indicadorTypeText = dictionary["Item_Indicaddor_Filter_TypeObjetivo"];
                break;
        }

        iTSpdf.PdfPCell criteriaIndicadorType = new iTSpdf.PdfPCell(new iTS.Phrase(indicadorTypeText, times));
        criteriaIndicadorType.Border = borderNone;
        criteriaIndicadorType.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        criteriaIndicadorType.Padding = 6f;
        criteriaIndicadorType.PaddingTop = 4f;
        criteriatable.AddCell(criteriaIndicadorType);

        if (indicatorType == 0)
        {
            criteriatable.AddCell(criteriaBlank);
            criteriatable.AddCell(criteriaBlank);
            criteriatable.AddCell(criteriaBlank);
            criteriatable.AddCell(criteriaBlank);
        }
        else if (indicatorType == 1)
        {
            iTSpdf.PdfPCell criteriaProcessLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Process"] + " :", timesBold));
            criteriaProcessLabel.Border = borderNone;
            criteriaProcessLabel.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            criteriaProcessLabel.Padding = 6f;
            criteriaProcessLabel.PaddingTop = 4f;
            criteriatable.AddCell(criteriaProcessLabel);

            string processText = dictionary["Common_All_Male_Plural"];
            if (processId.HasValue)
            {
                Process p = new Process(processId.Value, companyId);
                processText = p.Description;
            }

            iTSpdf.PdfPCell criteriaProcess = new iTSpdf.PdfPCell(new iTS.Phrase(processText, times));
            criteriaProcess.Border = borderNone;
            criteriaProcess.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            criteriaProcess.Padding = 6f;
            criteriaProcess.PaddingTop = 4f;
            criteriatable.AddCell(criteriaProcess);

            iTSpdf.PdfPCell criteriaProcessTypeLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_ProcessType"] + " :", timesBold));
            criteriaProcessTypeLabel.Border = borderNone;
            criteriaProcessTypeLabel.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            criteriaProcessTypeLabel.Padding = 6f;
            criteriaProcessTypeLabel.PaddingTop = 4f;
            criteriatable.AddCell(criteriaProcessTypeLabel);

            string processTypeCriteriaText = dictionary["Common_All_Male_Plural"];
            if (processTypeId.HasValue)
            {
                switch (processTypeId.Value)
                {
                    case 1:
                        processTypeCriteriaText = dictionary["Item_ProcessType_Name_Principal"];
                        break;
                    case 2:
                        processTypeCriteriaText = dictionary["Item_ProcessType_Name_Support"];
                        break;
                    case 3:
                        processTypeCriteriaText = dictionary["Item_ProcessType_Name_Estrategic"];
                        break;
                    default:
                        processTypeCriteriaText = string.Empty;
                        ReadOnlyCollection<ProcessType> ts = ProcessType.ObtainByCompany(companyId, dictionary);
                        foreach (ProcessType t in ts)
                        {
                            if (t.Id == processTypeId.Value)
                            {
                                processTypeCriteriaText = t.Description;
                            }
                        }

                        break;
                }
            }

            iTSpdf.PdfPCell criteriaProcessType = new iTSpdf.PdfPCell(new iTS.Phrase(processTypeCriteriaText, times));
            criteriaProcessType.Border = borderNone;
            criteriaProcessType.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            criteriaProcessType.Padding = 6f;
            criteriaProcessType.PaddingTop = 4f;
            criteriatable.AddCell(criteriaProcessType);
        }
        else if (indicatorType == 2)
        {
            iTSpdf.PdfPCell criteriaProcessLabel = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Objetivo"] + " :", timesBold));
            criteriaProcessLabel.Border = borderNone;
            criteriaProcessLabel.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            criteriaProcessLabel.Padding = 6f;
            criteriaProcessLabel.PaddingTop = 4f;
            criteriatable.AddCell(criteriaProcessLabel);

            string objetivoText = dictionary["Common_All_Male_Plural"];

            if (targetId.HasValue)
            {
                Objetivo o = Objetivo.GetById(targetId.Value, companyId);
                objetivoText = o.Description;
            }

            iTSpdf.PdfPCell criteriaProcess = new iTSpdf.PdfPCell(new iTS.Phrase(objetivoText, times));
            criteriaProcess.Border = borderNone;
            criteriaProcess.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            criteriaProcess.Padding = 6f;
            criteriaProcess.PaddingTop = 4f;

            criteriatable.AddCell(criteriaProcess);
            criteriatable.AddCell(criteriaProcess);
        }

            pdfDoc.Add(criteriatable);
        //---------------------------

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 30f, 10f, 30f, 10f, 10f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 1;
        //leave a gap before and after the table
        table.SpacingBefore = 20f;
        table.SpacingAfter = 30f;

        iTSpdf.PdfPCell headerDescripcion = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_Header_Description"].ToUpperInvariant(), headerFontFinal));
        headerDescripcion.Border = borderAll;
        headerDescripcion.BackgroundColor = backgroundColor;
        headerDescripcion.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerDescripcion.Padding = 8f;
        headerDescripcion.PaddingTop = 6f;

        iTSpdf.PdfPCell headerDate = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_Header_StartDate"].ToUpperInvariant(), headerFontFinal));
        headerDate.Border = borderAll;
        headerDate.BackgroundColor = backgroundColor;
        headerDate.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerDate.Padding = 8f;
        headerDate.PaddingTop = 6f;

        iTSpdf.PdfPCell headerProcess = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_Header_Process"].ToUpperInvariant(), headerFontFinal));
        headerProcess.Border = borderAll;
        headerProcess.BackgroundColor = backgroundColor;
        headerProcess.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerProcess.Padding = 8f;
        headerProcess.PaddingTop = 6f;

        iTSpdf.PdfPCell headerProcessType = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_Header_ProcessType"], headerFontFinal));
        headerProcessType.Border = borderAll;
        headerProcessType.BackgroundColor = backgroundColor;
        headerProcessType.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerProcessType.Padding = 8f;
        headerProcessType.PaddingTop = 6f;

        iTSpdf.PdfPCell headerResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicador_Header_ObjetivoResponsible"].ToUpperInvariant(), headerFontFinal));
        headerResponsible.Border = borderAll;
        headerResponsible.BackgroundColor = backgroundColor;
        headerResponsible.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
        headerResponsible.Padding = 8f;
        headerResponsible.PaddingTop = 6f;

        table.AddCell(headerDescripcion);
        table.AddCell(headerDate);
        table.AddCell(headerProcess);
        table.AddCell(headerProcessType);
        table.AddCell(headerResponsible);

        int cont = 0;
        ReadOnlyCollection<IndicadorFilterItem> data = Indicador.Filter(companyId, indicatorType, from, to, processId, processTypeId, targetId, status);
        bool pair = false;
        foreach (IndicadorFilterItem item in data)
        {
            int border = 0;

            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            iTSpdf.PdfPCell cellDescription = new iTSpdf.PdfPCell(new iTS.Phrase(item.Indicador.Description, times));
            cellDescription.Border = border;
            cellDescription.BackgroundColor = lineBackground;
            cellDescription.Padding = 6f;
            cellDescription.PaddingTop = 4f;
            table.AddCell(cellDescription);

            iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.StartDate), times));
            cellDate.Border = border;
            cellDate.BackgroundColor = lineBackground;
            cellDate.Padding = 6f;
            cellDate.PaddingTop = 4f;
            cellDate.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            table.AddCell(cellDate);

            string statustext = string.Empty;

            iTSpdf.PdfPCell cellProcess = new iTSpdf.PdfPCell(new iTS.Phrase(item.Proceso.Description, times));
            cellProcess.Border = border;
            cellProcess.BackgroundColor = lineBackground;
            cellProcess.Padding = 6f;
            cellProcess.PaddingTop = 4f;
            cellProcess.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            table.AddCell(cellProcess);

            string processTypeText = string.Empty;
            switch (item.Proceso.ProcessType)
            {
                case 1:
                    processTypeText = dictionary["Item_ProcessType_Name_Principal"];
                    break;
                case 2:
                    processTypeText = dictionary["Item_ProcessType_Name_Support"];
                    break;
                case 3:
                    processTypeText = dictionary["Item_ProcessType_Name_Estrategic"];
                    break;
            }

            iTSpdf.PdfPCell cellProcessType = new iTSpdf.PdfPCell(new iTS.Phrase(processTypeText, times));
            cellProcessType.Border = border;
            cellProcessType.BackgroundColor = lineBackground;
            cellProcessType.Padding = 6f;
            cellProcessType.PaddingTop = 4f;
            cellProcessType.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            table.AddCell(cellProcessType);

            iTSpdf.PdfPCell cellResponsible = new iTSpdf.PdfPCell(new iTS.Phrase(item.ObjetivoResponsible, times));
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
 