// --------------------------------
// <copyright file="BusinessRiskExportList.aspx.cs" company="Sbrinna">
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
using PDF_Tests;

public partial class ExportBusinessRiskExportList : Page
{
    public static Dictionary<string, string> Dictionary;

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(
        int companyId,
        string from,
        string to,
        long rulesId,
        long processId,
        int typeId,
        string listOrder)
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
            Dictionary["Item_BusinessRisks"],
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
            Title = Dictionary["Item_BusinessRisks"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Dictionary["Item_EquipmentList"], company.Name), ToolsPdf.LayoutFonts.TitleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });
        
        //------ CRITERIA
        var criteriatable = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 10f, 50f, 10f, 80f });

        #region texts

        string criteriaProccess = Dictionary["Common_All_Male_Plural"];
        if (processId > 0)
        {
            var process = new Process(processId, companyId);
            if (!string.IsNullOrEmpty(process.Description))
            {
                criteriaProccess = process.Description;
            }
        }

        string periode = string.Empty;
        if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
        {
            periode = Dictionary["Item_Incident_List_Filter_From"] + " " + from;
        }
        else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = Dictionary["Item_Incident_List_Filter_To"] + " " + to;
        }
        else if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            periode = from + " - " + to;
        }
        else
        {
            periode = Dictionary["Common_All_Male"];
        }

        string typetext = Dictionary["Common_All_Male_Plural"];
        if (typeId == 1) { typetext = Dictionary["Item_BusinessRisk_Status_Assumed"]; }
        if (typeId == 2) { typetext = Dictionary["Item_BusinessRisk_Status_Significant"]; }
        if (typeId == 3) { typetext = Dictionary["Item_BusinessRisk_Status_NotSignificant"]; }
        if (typeId == 4) { typetext = Dictionary["Item_BusinessRisk_Status_Unevaluated"]; }

        string ruleDescriptionBusinessRisk = Dictionary["Common_All_Female_Plural"];
        if (rulesId > 0)
        {
            var rule = Rules.GetById(companyId, rulesId);
            if (!string.IsNullOrEmpty(rule.Description))
            {
                ruleDescriptionBusinessRisk = rule.Description;
            }
        }
        #endregion

        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Common_Period"]));
        criteriatable.AddCell(ToolsPdf.CriteriaCellData(periode));
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Item_BusinesRisk_ListHeader_Process"]));
        criteriatable.AddCell(ToolsPdf.CriteriaCellData(typetext));
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Item_BusinesRisk_ListHeader_Rule"]));
        criteriatable.AddCell(ToolsPdf.CriteriaCellData(ruleDescriptionBusinessRisk));
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Item_BusinesRisk_ListHeader_Type"]));
        criteriatable.AddCell(ToolsPdf.CriteriaCellData(criteriaProccess));

        pdfDoc.Add(criteriatable);
        //---------------------------

        var table = new iTSpdf.PdfPTable(7)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 10f, 10f, 30f, 20f, 20f, 10f, 10f });

        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_IncidentAction_Header_Type"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Date"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Description"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Process"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_Rule"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_StartValue"]));
        table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_BusinesRisk_ListHeader_IPR"]));

        int cont = 0;
        var data = HttpContext.Current.Session["BusinessRiskFilterData"] as List<BusinessRiskFilterItem>;

        switch (listOrder.ToUpperInvariant())
        {
            default:
            case "TH1|ASC":
                data = data.OrderBy(d => d.OpenDate).ToList();
                break;
            case "TH1|DESC":
                data = data.OrderByDescending(d => d.OpenDate).ToList();
                break;
            case "TH2|ASC":
                data = data.OrderBy(d => d.Description).ToList();
                break;
            case "TH2|DESC":
                data = data.OrderByDescending(d => d.Description).ToList();
                break;
            case "TH3|ASC":
                data = data.OrderBy(d => d.Process.Description).ToList();
                break;
            case "TH3|DESC":
                data = data.OrderByDescending(d => d.Process.Description).ToList();
                break;
            case "TH4|ASC":
                data = data.OrderBy(d => d.Rule.Description).ToList();
                break;
            case "TH4|DESC":
                data = data.OrderByDescending(d => d.Rule.Description).ToList();
                break;
            case "TH5|ASC":
                data = data.OrderBy(d => d.InitialResult).ToList();
                break;
            case "TH5|DESC":
                data = data.OrderByDescending(d => d.InitialResult).ToList();
                break;
            case "TH6|ASC":
                data = data.OrderBy(d => d.Rule.Limit).ToList();
                break;
            case "TH6|DESC":
                data = data.OrderByDescending(d => d.Rule.Limit).ToList();
                break;
        }

        foreach (var risk in data)
        {
            cont++;
            string typeText = string.Empty;
            if (risk.Assumed) { typeText = Dictionary["Item_BusinessRisk_Status_Assumed"]; }
            else if (risk.InitialResult == 0) { typeText = Dictionary["Item_BusinessRisk_Status_Unevaluated"]; }
            else if (risk.InitialResult < risk.Rule.Limit) { typeText = Dictionary["Item_BusinessRisk_Status_NotSignificant"]; }
            else { typeText = Dictionary["Item_BusinessRisk_Status_Significant"]; }

            string initialResultText = risk.InitialResult == 0 ? string.Empty : risk.InitialResult.ToString();

            table.AddCell(ToolsPdf.DataCellCenter(typeText));
            table.AddCell(ToolsPdf.DataCellCenter(risk.OpenDate));
            table.AddCell(ToolsPdf.DataCell(risk.Description));
            table.AddCell(ToolsPdf.DataCell(risk.Process.Description));
            table.AddCell(ToolsPdf.DataCellCenter(risk.Rule.Description));
            table.AddCell(ToolsPdf.DataCellCenter(initialResultText));
            table.AddCell(ToolsPdf.DataCellRight(risk.Rule.Limit));
        }

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            Dictionary["Common_RegisterCount"],
            cont), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 4
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            Colspan = 4
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}