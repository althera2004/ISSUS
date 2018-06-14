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

public partial class ExportPrintActionData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        long actionId = Convert.ToInt64(Request.QueryString["id"]);
        int companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var action = IncidentAction.ById(actionId, user.CompanyId);

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
            var incident = Incident.GetById(action.IncidentId, action.CompanyId);
            origin = "Incidencia:" + incident.Description;
        }

        if (action.Origin == 4)
        {
            if (action.BusinessRiskId != null)
            {
                var businessRisk = BusinessRisk.ById(action.CompanyId, action.BusinessRiskId);
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
                    var department = Department.ById(action.Department.Id, action.CompanyId);
                    reporter = department.Description;
                    break;
                case 2:
                    reporterType = dictionary["Item_IncidentAction_ReporterType2"];
                    var provider = Provider.ById(action.Provider.Id, action.CompanyId);
                    reporter = provider.Description;
                    break;
                case 3:
                    reporterType = dictionary["Item_IncidentAction_ReporterType3"];
                    var customer = Customer.ById(action.Customer.Id, action.CompanyId);
                    reporter = customer.Description;
                    break;
                default:
                    break;
            }
        }

        string status = string.Empty;
        var document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 65, 55);
        var writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Temp\\" + fileName, FileMode.Create));
        writer.PageEvent = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = action.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_IncidentAction"].ToUpperInvariant()
        };

        document.Open();
        var styles = new iTextSharp.text.html.simpleparser.StyleSheet();
        var hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

        var widths = new float[] { 15f, 30f, 15f, 30f };
        var table = new PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };
        table.SetWidths(widths);
        var borderSides = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER;
        var borderTBL = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderTBR = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
        var borderBL = Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderBR = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;

        var alignLeft = Element.ALIGN_LEFT;
        var alignRight = Element.ALIGN_RIGHT;

        var labelFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.DARK_GRAY);
        var valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);

        //document.Add(new Phrase("\n"));

        // Descripción
        table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Description"], Rectangle.NO_BORDER));
        table.AddCell(valueCell(action.Description, ToolsPdf.BorderNone, alignLeft, 1));

        // Tipo
        table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Type"], Rectangle.NO_BORDER));
        table.AddCell(valueCell(type, ToolsPdf.BorderNone, alignLeft, 1));

        // Origen
        table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Origin"], Rectangle.NO_BORDER));
        table.AddCell(valueCell(origin + originSufix, ToolsPdf.BorderNone, alignLeft, 3));

        // Reportador
        if (action.Origin != 4)
        {
            table.AddCell(labelCell(dictionary["Item_IncidentAction_Label_Reporter"], Rectangle.NO_BORDER));
            table.AddCell(valueCell(reporterType + " (" + reporter + ")", ToolsPdf.BorderNone, alignLeft, 3));
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
        table.AddCell(textAreaCell(Environment.NewLine + action.Monitoring, ToolsPdf.BorderAll, alignLeft, 4));

        // Close
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_Close"]));
        table.AddCell(textAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1}",dictionary["Item_IncidentAction_Field_Responsible"] ,action.ClosedBy.FullName), borderTBL, alignLeft, 2));
        table.AddCell(textAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1:dd/MM/yyyy}", dictionary["Common_DateClose"], action.ClosedOn), borderTBR, alignRight, 2));

        // Notes
        table.AddCell(separationRow());
        table.AddCell(titleCell(dictionary["Item_IncidentAction_Field_Notes"]));
        table.AddCell(textAreaCell(Environment.NewLine + action.Notes, ToolsPdf.BorderAll, alignLeft, 4));

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
        return new PdfPCell(new Phrase(label + ":", new Font(this.arial, 10, Font.NORMAL, BaseColor.DARK_GRAY)))
        {
            Border = borders,
            HorizontalAlignment = 2
        };
    }

    private PdfPCell valueCell(string value, int borders, int align, int colSpan)
    {
        return new PdfPCell(new Phrase(value, new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = colSpan,
            Border = borders,
            HorizontalAlignment = align
        };
    }

    private PdfPCell textAreaCell(string value, int borders, int align, int colSpan)
    {
        return new PdfPCell(new Phrase(value, new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = colSpan,
            Border = borders,
            HorizontalAlignment = align,
            Padding = 10,
            PaddingTop = 0
        };
    }

    private PdfPCell titleCell(string value)
    {
        return new PdfPCell(new Phrase(value, new Font(this.headerFont, 11, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = 4,
            HorizontalAlignment = Element.ALIGN_CENTER,
            BackgroundColor = BaseColor.WHITE,
            Padding = 8,
            PaddingTop = 6
        };
    }

    private PdfPCell blankRow()
    {
        return new PdfPCell(new Phrase("\n"))
        {
            Colspan = 4,
            Border = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER
        };
    }

    private PdfPCell separationRow()
    {
        return new PdfPCell(new Phrase("\n"))
        {
            Colspan = 4,
            Border = Rectangle.NO_BORDER
        };
    }
}