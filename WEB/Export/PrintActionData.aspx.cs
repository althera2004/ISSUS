// --------------------------------
// <copyright file="PrintActionData.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class Export_PrintActionData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        long actionId = Convert.ToInt64(Request.QueryString["id"].ToString());
        int companyId = Convert.ToInt32(Request.QueryString["companyId"].ToString());
        Company company = new Company(companyId);
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        IncidentAction action = IncidentAction.ById(actionId, user.CompanyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_IncidentAction"],
            action.Description.Replace(@"""", "'"),
            DateTime.Now);

        // FONTS
        string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        this.headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        this.arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        string type = string.Empty;
        string origin = string.Empty;
        string originSufix = string.Empty;
        string reporterType = string.Empty;
        string reporter = string.Empty;

        if (action.ActionType == 1) { type = dictionary["Item_IncidentAction_Type1"]; }
        if (action.ActionType == 2) { type = dictionary["Item_IncidentAction_Type2"]; }
        if (action.ActionType == 3) { type = dictionary["Item_IncidentAction_Type3"]; }

        if (action.Origin == 1) { origin = dictionary["Item_IncidentAction_Origin1"]; }
        if (action.Origin == 2) { origin = dictionary["Item_IncidentAction_Origin2"]; }
        if (action.Origin == 3)
        {
            Incident incident = Incident.GetById(action.IncidentId, action.CompanyId);
            origin = "Incidencia:" + incident.Description;
        }

        if (action.Origin == 4)
        {
            if (action.BusinessRiskId != null)
            {
                BusinessRisk businessRisk = BusinessRisk.GetById(action.CompanyId, action.BusinessRiskId);
                origin = businessRisk.Description;
                originSufix = " (" + dictionary["Item_BusinessRisk"] + ")";
            }
        }
        else
        {

            switch (action.ReporterType)
            {
                case 1:
                    reporterType = dictionary["Item_IncidentAction_ReporterType1"];
                    Department department = Department.GetById(action.Department.Id, action.CompanyId);
                    reporter = department.Description;
                    break;
                case 2:
                    reporterType = dictionary["Item_IncidentAction_ReporterType2"];
                    Provider provider = Provider.ById(action.Provider.Id, action.CompanyId);
                    reporter = provider.Description;
                    break;
                case 3:
                    reporterType = dictionary["Item_IncidentAction_ReporterType3"];
                    Customer customer = Customer.ById(action.Customer.Id, action.CompanyId);
                    reporter = customer.Description;
                    break;
            }
        }

        string status = string.Empty;

        iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 65, 55);

        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Temp\\" + fileName, FileMode.Create));
        TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = action.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_IncidentAction"].ToUpperInvariant()
        };

        writer.PageEvent = PageEventHandler;

        document.Open();
        iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
        iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

        PdfPTable table = new PdfPTable(4);

        // actual width of table in points
        table.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 15f, 30f, 15f, 30f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 0;
        //leave a gap before and after the table
        //table.SpacingBefore = 5f;
        //table.SpacingAfter = 30f;

        var borderNone = Rectangle.NO_BORDER;
        var borderSides = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER;
        var borderAll = Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
        var borderTBL = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderTBR = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
        var borderBL = Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderBR = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;

        var alignLeft = Element.ALIGN_LEFT;
        var alignRight = Element.ALIGN_RIGHT;

        Font labelFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.DARK_GRAY);
        Font valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);

        //document.Add(new Phrase("\n"));

        // Descripción
        table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Description"], Rectangle.NO_BORDER));
        table.AddCell(valueCell(action.Description, borderNone, alignLeft, 1));

        // Tipo
        table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Type"], Rectangle.NO_BORDER));
        table.AddCell(valueCell(type, borderNone, alignLeft, 1));

        // Origen
        table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Origin"], Rectangle.NO_BORDER));
        table.AddCell(valueCell(origin + originSufix, borderNone, alignLeft, 3));

        // Reportador
        if (action.Origin != 4)
        {
            table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Reporter"], Rectangle.NO_BORDER));
            table.AddCell(valueCell(reporterType + " (" + reporter + ")", borderNone, alignLeft, 3));
        }

        // WhatHappend
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_WhatHappened"]));
        table.AddCell(textAreaCell(Environment.NewLine + action.WhatHappened, borderSides, alignLeft, 4));
        table.AddCell(blankRow());
        table.AddCell(textAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + action.WhatHappenedBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(textAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], action.WhatHappenedOn), borderBR, alignRight, 2));

        // Causes
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_Causes"]));
        table.AddCell(textAreaCell(Environment.NewLine + action.Causes, borderSides, alignLeft, 4));
        table.AddCell(blankRow());
        table.AddCell(textAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + action.CausesBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(textAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], action.CausesOn), borderBR, alignRight, 2));

        // Actions
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_Actions"]));
        table.AddCell(textAreaCell(Environment.NewLine + action.Actions, borderSides, alignLeft, 4));
        table.AddCell(blankRow());
        table.AddCell(textAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + action.ActionsBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(textAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_DateExecution"], action.ActionsOn), borderBR, alignRight, 2));

        // Monitoring
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_Monitoring"]));
        table.AddCell(textAreaCell(Environment.NewLine + action.Monitoring, borderAll, alignLeft, 4));

        // Close
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_Close"]));
        table.AddCell(textAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1}",dictionary["Item_IncidentAction_Field_Responsible"] ,action.ClosedBy.FullName), borderTBL, alignLeft, 2));
        table.AddCell(textAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1:dd/MM/yyyy}", dictionary["Common_DateClose"], action.ClosedOn), borderTBR, alignRight, 2));

        // Notes
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_Notes"]));
        table.AddCell(textAreaCell(Environment.NewLine + action.Notes, borderAll, alignLeft, 4));

        document.Add(table);
        document.Close();

        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=Accion.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(Request.PhysicalApplicationPath + "\\Temp\\" + fileName);
        Response.Flush();
        Response.Clear();
    }

    private PdfPCell labelCell(string label, int borders)
    {
        Font labelFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.DARK_GRAY);
        PdfPCell cell = new PdfPCell(new Phrase(label + ":", labelFont));
        cell.Border = borders;
        cell.HorizontalAlignment = 2;
        return cell;
    }

    private PdfPCell valueCell(string value, int borders, int align, int colSpan)
    {
        Font valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);
        PdfPCell cell = new PdfPCell(new Phrase(value, valueFont));
        cell.Colspan = colSpan;
        cell.Border = borders;
        cell.HorizontalAlignment = align;
        return cell;
    }

    private PdfPCell textAreaCell(string value, int borders, int align, int colSpan)
    {
        Font valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);
        PdfPCell cell = new PdfPCell(new Phrase(value, valueFont));
        cell.Colspan = colSpan;
        cell.Border = borders;
        cell.HorizontalAlignment = align;
        cell.Padding = 10;
        cell.PaddingTop = 0;
        return cell;
    }

    private PdfPCell titleCell(string value)
    {
        Font valueFont = new Font(this.headerFont, 11, Font.NORMAL, BaseColor.BLACK);
        PdfPCell cell = new PdfPCell(new Phrase(value, valueFont));
        cell.Colspan = 4;
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.BackgroundColor = new BaseColor(225, 225, 225);
        cell.Padding = 8;
        cell.PaddingTop = 6;
        return cell;
    }

    private PdfPCell blankRow()
    {
        PdfPCell cell = new PdfPCell(new Phrase("\n"));
        cell.Colspan = 4;
        cell.Border = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER;
        return cell;
    }

    private PdfPCell separationRow()
    {
        PdfPCell cell = new PdfPCell(new Phrase("\n"));
        cell.Colspan = 4;
        cell.Border = Rectangle.NO_BORDER;
        return cell;
    }
}