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
        int status,
        string listOrder)
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

            criteriatable.AddCell(criteriaProcessLabel);
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

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_Description"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_StartDate"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_Process"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_ProcessType"].ToUpperInvariant(), times));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_ObjetivoResponsible"].ToUpperInvariant(), times));


        int cont = 0;
        List<IndicadorFilterItem> data = Indicador.Filter(companyId, indicatorType, from, to, processId, processTypeId, targetId, status).ToList();
        bool pair = false;

        switch (listOrder.ToUpperInvariant())
        {
            case "TH0|ASC":
                data = data.OrderBy(d => d.Indicador.Description).ToList();
                break;
            case "TH0|DESC":
                data = data.OrderByDescending(d => d.Indicador.Description).ToList();
                break;
            case "TH1|ASC":
                data = data.OrderBy(d => d.StartDate).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.StartDate).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.Proceso.Description).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.Proceso.Description).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.Proceso.ProcessType).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.Proceso.ProcessType).ToList();
                break;
            case "TH4|ASC":
                data = data.OrderBy(d => d.ObjetivoResponsible).ToList();
                break;
            case "TH4|DESC":
                data = data.OrderByDescending(d => d.ObjetivoResponsible).ToList();
                break;
        }

        foreach (IndicadorFilterItem item in data)
        {
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

            BaseColor lineBackground = pair ? rowEven : rowPair;
            // pair = !pair;

            table.AddCell(ToolsPdf.DataCell(item.Indicador.Description, times));
            table.AddCell(ToolsPdf.DataCell(item.StartDate, times, Rectangle.ALIGN_CENTER));            
            table.AddCell(ToolsPdf.DataCell(item.Proceso.Description, times));
            table.AddCell(ToolsPdf.DataCell(processTypeText, times));
            table.AddCell(ToolsPdf.DataCell(item.ObjetivoResponsible, times));
            cont++;
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
           CultureInfo.InvariantCulture,
           @"{0}: {1}",
           dictionary["Common_RegisterCount"],
           cont), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Padding = 6f,
            PaddingTop = 4f
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Colspan = 4
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }
}
 