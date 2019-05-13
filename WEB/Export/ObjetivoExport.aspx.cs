// --------------------------------
// <copyright file="ObjetivoExportList.aspx.cs" company="OpenFramework">
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class ExportObjetivoExport : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    public static Font criteriaFont;
    public static Dictionary<string, string> dictionary;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(
        int companyId,
        DateTime? from,
        DateTime? to,
        int status,
        string listOrder,
        string filterText)
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

        var formatedDescription = ToolsPdf.NormalizeFileName(company.Name);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Objetivo_List"],
            formatedDescription,
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
            Title = dictionary["Item_Objetivo_List"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var rowEven = new iTS.BaseColor(240, 240, 240);
        
        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), ToolsPdf.LayoutFonts.TitleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });


        #region Criteria
        var criteriatable = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 20f, 50f, 20f, 150f });

        #region texts
        string periode = string.Empty;
        if (from.HasValue && to == null)
        {
            periode = string.Format(CultureInfo.InvariantCulture, @"{0} {1:dd/MM/yyyy}", dictionary["Item_IncidentAction_List_Filter_From"], from);
        }
        else if (from == null && to.HasValue)
        {
            periode = string.Format(CultureInfo.InvariantCulture, @"{0} {1:dd/MM/yyyy}", dictionary["Item_IncidentAction_List_Filter_To"], to);
        }
        else if (from.HasValue && to.HasValue)
        {
            periode = string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy} {1:dd/MM/yyyy}", from, to);
        }
        else
        {
            periode = dictionary["Common_All_Male"];
        }

        string statusText = dictionary["Common_All"];
        if (status == 1)
        {
            statusText = dictionary["Item_ObjetivoAction_List_Filter_ShowActive"];
        }

        if (status == 2)
        {
            statusText = dictionary["Item_ObjetivoAction_List_Filter_ShowClosed"];
        }
        #endregion

        ToolsPdf.AddCriteria(criteriatable, dictionary["Common_Period"], periode);
        ToolsPdf.AddCriteria(criteriatable, dictionary["Item_IncidentAction_Header_Status"], statusText);

        if (!string.IsNullOrEmpty(filterText))
        {
            ToolsPdf.AddCriteria(criteriatable, dictionary["Common_PDF_Filter_Contains"], filterText);
        }
        pdfDoc.Add(criteriatable);
        #endregion

        var table = new iTSpdf.PdfPTable(5)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 7f, 50f, 30f, 12f, 12f });

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Common_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_Name"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_Responsible"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_StartDate"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_Header_PreviewEndDate"]));

        int cont = 0;
        var data = Objetivo.Filter(companyId,  from, to, status).ToList();
        foreach(var item in data)
        {
            if (!item.Objetivo.EndDate.HasValue)
            {
                item.Objetivo.EndDate = item.Objetivo.PreviewEndDate;
            }
        }

        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH0|ASC":
                data = data.OrderBy(d => d.Objetivo.Name).ToList();
                break;
            case "TH0|DESC":
                data = data.OrderByDescending(d => d.Objetivo.Name).ToList();
                break;
            case "TH1|ASC":
                data = data.OrderBy(d => d.Objetivo.Responsible.FullName).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.Objetivo.Responsible.FullName).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.Objetivo.StartDate).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.Objetivo.StartDate).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.Objetivo.EndDate).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.Objetivo.EndDate).ToList();
                break;
        }

        cont = 0;
        foreach (var item in data)
        {
            if (!string.IsNullOrEmpty(filterText))
            {
                var show = false;
                if (item.Objetivo.Name.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    show = true;
                }

                if (item.Objetivo.Responsible.FullName.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    show = true;
                }

                if (!show)
                {
                    continue;
                }
            }

            string endDateText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.PreviewEndDate);
            if (item.Objetivo.EndDate.HasValue)
            {
                endDateText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.EndDate);
            }

            table.AddCell(ToolsPdf.DataCell(item.Objetivo.Active ? dictionary["Common_Active"] : dictionary["Common_Inactve"], ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCell(item.Objetivo.Name, ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCell(item.Objetivo.Responsible.FullName, ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCellCenter(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", item.Objetivo.StartDate), ToolsPdf.LayoutFonts.Times));
            table.AddCell(ToolsPdf.DataCellCenter(endDateText, ToolsPdf.LayoutFonts.Times));
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
            PaddingTop = 4f,
            Colspan = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 3
        });

        pdfDoc.Add(table);

        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}