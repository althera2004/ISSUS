// --------------------------------
// <copyright file="PrintIncidentData.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class ExportPrintIncidentData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        var incidentId = Convert.ToInt64(Request.QueryString["id"]);
        var companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var incident = Incident.GetById(incidentId, user.CompanyId);
        var path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Incident"],
            incident.Description.Replace(@"""", "'"),
            DateTime.Now);

        // FONTS
        var pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
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

        if (incident.Origin == 1) { origin = dictionary["Item_Incident_Origin1"]; }
        if (incident.Origin == 2) { origin = dictionary["Item_Incident_Origin2"]; }
        if (incident.Origin == 3) { origin = dictionary["Item_Incident_Origin3"]; }

        if (incident.Department.Id > 0)
        {
            reporterType = dictionary["Item_Incident_Origin1"];
            reporter = Department.ById(incident.Department.Id, incident.CompanyId).Description;
        }
        else if(incident.Provider.Id > 0)
        {
            reporterType = dictionary["Item_Incident_Origin2"];
            reporter = Provider.ById(incident.Provider.Id, incident.CompanyId).Description;
        }
        else if (incident.Customer.Id > 0)
        {
            reporterType = dictionary["Item_Incident_Origin3"];
            reporter = Customer.ById(incident.Customer.Id, incident.CompanyId).Description;
        }

        string status = string.Empty;

        var document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 65, 55);

        var writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Temp\\" + fileName, FileMode.Create));
        var pageEventHandler = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = incident.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Incident"]
        };

        pageEventHandler.Titles = new List<string>
        {
            dictionary["Item_IncidentAction"]
        };

        writer.PageEvent = pageEventHandler;
        document.Open();

        var table = new PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };

        table.SetWidths(new float[] { 15f, 30f, 15f, 30f });

        var borderNone = Rectangle.NO_BORDER;
        var borderSides = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER;
        var borderAll = Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
        var borderTBL = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderTBR = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
        var borderBL = Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderBR = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;

        var alignLeft = Element.ALIGN_LEFT;
        var alignRight = Element.ALIGN_RIGHT;

        var labelFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.DARK_GRAY);
        var valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);

        // Descripción
        table.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Description"], Rectangle.NO_BORDER));
        table.AddCell(ValueCell(incident.Description, borderNone, alignLeft, 3));

        // Reportador
        if (incident.Origin != 4)
        {
            table.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Reporter"], Rectangle.NO_BORDER));
            table.AddCell(ValueCell(reporterType + " (" + reporter + ")", borderNone, alignLeft, 3));
        }

        // WhatHappend
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_WhatHappened"]));
        table.AddCell(TextAreaCell(Environment.NewLine + incident.WhatHappened, borderSides, alignLeft, 4));
        table.AddCell(BlankRow());
        table.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + incident.WhatHappenedBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], incident.WhatHappenedOn), borderBR, alignRight, 2));

        // Causes
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Causes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + incident.Causes, borderSides, alignLeft, 4));
        table.AddCell(BlankRow());
        table.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + incident.CausesBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], incident.CausesOn), borderBR, alignRight, 2));

        // Actions
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Actions"]));
        table.AddCell(TextAreaCell(Environment.NewLine + incident.Actions, borderSides, alignLeft, 4));
        table.AddCell(BlankRow());
        table.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + incident.ActionsBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_DateExecution"], incident.ActionsOn), borderBR, alignRight, 2));

        // Close
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Close"]));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1}", dictionary["Item_IncidentAction_Field_Responsible"], incident.ClosedBy.FullName), borderTBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1:dd/MM/yyyy}", dictionary["Common_DateClose"], incident.ClosedOn), borderTBR, alignRight, 2));

        // Notes
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Notes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + incident.Notes, borderAll, alignLeft, 4));

        document.Add(table);

        // Añadir posible acción
        var action = IncidentAction.ByIncidentId(incident.Id, companyId);
        if(action.Id > 0)
        {
            var tableAction = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };

            tableAction.SetWidths(new float[] { 15f, 30f, 15f, 30f });

            // Descripción
            //tableAction.AddCell(valueCell(dictionary["Item_Incident_PDF_ActionPageTitle"]+"*", borderNone, alignLeft, 4));
            var headerFont = new Font(this.arial, 15, Font.NORMAL, BaseColor.BLACK);
            tableAction.AddCell(new PdfPCell(new Phrase(dictionary["Item_Incident_PDF_ActionPageTitle"], headerFont))
            {
                Colspan = 4,
                Border = ToolsPdf.BorderBottom,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
            tableAction.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Description"], Rectangle.NO_BORDER));
            tableAction.AddCell(ValueCell(action.Description, borderNone, alignLeft, 3));

            // WhatHappend
            tableAction.AddCell(SeparationRow());
            tableAction.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_WhatHappened"]));
            tableAction.AddCell(TextAreaCell(Environment.NewLine + action.WhatHappened, borderSides, alignLeft, 4));
            tableAction.AddCell(BlankRow());
            tableAction.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + action.WhatHappenedBy.FullName, borderBL, alignLeft, 2));
            tableAction.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], action.WhatHappenedOn), borderBR, alignRight, 2));

            // Causes
            var causesFullName = string.Empty;
            var causesDate = string.Empty;
            if (action.CausesBy != null)
            {
                causesFullName = action.CausesBy.FullName;
                causesDate = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.CausesOn);
            }
            tableAction.AddCell(SeparationRow());
            tableAction.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Causes"]));
            tableAction.AddCell(TextAreaCell(Environment.NewLine + action.Causes, borderSides, alignLeft, 4));
            tableAction.AddCell(BlankRow());
            tableAction.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + causesFullName, borderBL, alignLeft, 2));
            tableAction.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", dictionary["Common_Date"], causesDate), borderBR, alignRight, 2));

            // Actions
            var actionFullName = string.Empty;
            var actionDate = string.Empty;
            if (action.ActionsBy != null)
            {
                actionFullName = action.ActionsBy.FullName;
                actionDate = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.ActionsOn);
            }
            tableAction.AddCell(SeparationRow());
            tableAction.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Actions"]));
            tableAction.AddCell(TextAreaCell(Environment.NewLine + action.Actions, borderSides, alignLeft, 4));
            tableAction.AddCell(BlankRow());
            tableAction.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + actionFullName, borderBL, alignLeft, 2));
            tableAction.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", dictionary["Common_DateExecution"], actionDate ), borderBR, alignRight, 2));

            // Monitoring
            tableAction.AddCell(SeparationRow());
            tableAction.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Monitoring"]));
            tableAction.AddCell(TextAreaCell(Environment.NewLine + action.Monitoring, borderAll, alignLeft, 4));

            // Close
            var closedFullName = string.Empty;
            var closedDate = string.Empty;
            if(action.ClosedBy != null)
            {
                closedFullName = action.ClosedBy.FullName;
                closedDate = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", action.ClosedOn);
            }
            tableAction.AddCell(SeparationRow());
            tableAction.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Close"]));
            tableAction.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1}", dictionary["Item_IncidentAction_Field_Responsible"], closedFullName), borderTBL, alignLeft, 2));
            tableAction.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1}", dictionary["Common_DateClose"], closedDate), borderTBR, alignRight, 2));

            // Notes
            tableAction.AddCell(SeparationRow());
            tableAction.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Notes"]));
            tableAction.AddCell(TextAreaCell(Environment.NewLine + action.Notes, borderAll, alignLeft, 4));

            document.NewPage();
            document.Add(tableAction);
        }

        // Costes
        var costs = IncidentCost.AllCosts(incidentId, companyId);
        if(costs.Count > 0)
        {
            var times = new Font(arial, 8, Font.NORMAL, BaseColor.BLACK);
            var fontSummary = new Font(arial, 9, Font.BOLD, BaseColor.BLACK);
            var headerFontFinal = new Font(headerFont, 9, Font.NORMAL, BaseColor.BLACK);
            var tableCosts = new PdfPTable(5)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 1,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };

            tableCosts.SetWidths(new float[] { 35f, 10f, 10f, 10f, 20f });

            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Description"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Amount"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Quantity"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Total"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_ReportedBy"]));

            decimal total = 0;
            decimal totalIncidencia = 0;
            decimal totalAccion = 0;
            int cont = 0;
            int contIncidencia = 0;
            int contAccion = 0;
            foreach (var cost in costs.Where(c => c.Source == "I"))
            {
                tableCosts.AddCell(ToolsPdf.DataCell(cost.Description, times));
                tableCosts.AddCell(ToolsPdf.DataCellMoney(cost.Amount, times));
                tableCosts.AddCell(ToolsPdf.DataCellMoney(cost.Quantity, times));
                tableCosts.AddCell(ToolsPdf.DataCellMoney(cost.Quantity * cost.Amount, times));
                tableCosts.AddCell(ToolsPdf.DataCellCenter(cost.Responsible.FullName, times));
                total += cost.Amount * cost.Quantity;
                totalIncidencia += cost.Amount * cost.Quantity;
                cont++;
                contIncidencia++;
            }

            tableCosts.AddCell(new PdfPCell(new Phrase(string.Format(
               CultureInfo.InvariantCulture,
               @"{0} {2}: {1}",
               dictionary["Common_RegisterCount"],
               contIncidencia,
               dictionary["Item_Incident"]), times))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 2
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(dictionary["Common_Total"], times))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1,
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(Tools.PdfMoneyFormat(totalIncidencia), times))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1,
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(string.Format(string.Empty, times)))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1
            });

            // Acciones
            foreach (var cost in costs.Where(c => c.Source == "A"))
            {
                tableCosts.AddCell(ToolsPdf.DataCell(cost.Description, times));
                tableCosts.AddCell(ToolsPdf.DataCellMoney(cost.Amount, times));
                tableCosts.AddCell(ToolsPdf.DataCellMoney(cost.Quantity, times));
                tableCosts.AddCell(ToolsPdf.DataCellMoney(cost.Quantity * cost.Amount, times));
                tableCosts.AddCell(ToolsPdf.DataCellCenter(cost.Responsible.FullName, times));
                total += cost.Amount * cost.Quantity;
                totalAccion = cost.Amount * cost.Quantity;
                cont++;
                contAccion++;
            }

            tableCosts.AddCell(new PdfPCell(new Phrase(string.Format(
                CultureInfo.InvariantCulture,
                @"{0} {2}: {1}",
                dictionary["Common_RegisterCount"],
                contAccion,
               dictionary["Item_IncidentAction"]), times))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 2
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(dictionary["Common_Total"], times))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1,
                HorizontalAlignment = Rectangle.ALIGN_RIGHT
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(Tools.PdfMoneyFormat(totalAccion), times))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1,
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(string.Format(string.Empty, times)))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1
            });

            // resumen
            tableCosts.AddCell(new PdfPCell(new Phrase(string.Format(
                CultureInfo.InvariantCulture,
                @"{0}: {1}",
                dictionary["Common_RegisterCount"],
                cont), fontSummary))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 2
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(dictionary["Common_Total"], fontSummary))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1,
                HorizontalAlignment = Rectangle.ALIGN_RIGHT
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(Tools.PdfMoneyFormat(total), fontSummary))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1,
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
            });

            tableCosts.AddCell(new PdfPCell(new Phrase(string.Format(string.Empty, fontSummary)))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = ToolsPdf.SummaryBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 1
            });

            document.SetPageSize(PageSize.A4.Rotate());
            document.NewPage();
            document.Add(tableCosts);
        }

        document.Close();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=Incidencia.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(Request.PhysicalApplicationPath + "\\Temp\\" + fileName);
        Response.Flush();
        Response.Clear();
    }

    private PdfPCell LabelCell(string label, int borders)
    {
        return new PdfPCell(new Phrase(label + ":", new Font(this.arial, 10, Font.NORMAL, BaseColor.DARK_GRAY)))
        {
            Border = borders,
            HorizontalAlignment = 2
        };
    }

    private PdfPCell ValueCell(string value, int borders, int align, int colSpan)
    {
        return new PdfPCell(new Phrase(value, new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = colSpan,
            Border = borders,
            HorizontalAlignment = align
        };
    }

    private PdfPCell TextAreaCell(string value, int borders, int align, int colSpan)
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

    private PdfPCell TitleCell(string value)
    {
        return new PdfPCell(new Phrase(value, new Font(this.headerFont, 11, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = 4,
            HorizontalAlignment = Element.ALIGN_CENTER,
            BackgroundColor = new BaseColor(225, 225, 225),
            Padding = 8,
            PaddingTop = 6
        };
    }

    private PdfPCell BlankRow()
    {
        return new PdfPCell(new Phrase("\n"))
        {
            Colspan = 4,
            Border = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER
        };
    }

    private PdfPCell SeparationRow()
    {
        return new PdfPCell(new Phrase("\n"))
        {
            Colspan = 4,
            Border = Rectangle.NO_BORDER
        };
    }
}