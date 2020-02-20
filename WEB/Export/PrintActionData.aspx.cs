// --------------------------------
// <copyright file="PrintActionData.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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

        var formatedDescription = ToolsPdf.NormalizeFileName(action.Description);
        formatedDescription = formatedDescription.Replace(@"\", "/");

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_IncidentAction"],
            formatedDescription,
            DateTime.Now);

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
            if (action.IncidentId.HasValue)
            {
                var incident = Incident.GetById(action.IncidentId.Value, action.CompanyId);
                origin = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}: {1}",
                    dictionary["Item_Incident"],
                    incident.Description);
            }
        }

        if (action.Origin == 4)
        {
            if (action.BusinessRiskId.HasValue)
            {
                if (action.BusinessRiskId > Constant.DefaultId)
                {
                    var businessRisk = BusinessRisk.ById(action.CompanyId, action.BusinessRiskId.Value);
                    origin = businessRisk.Description;
                    originSufix = " (" + dictionary["Item_BusinessRisk"] + ")";
                }
            }
        }
        if (action.Origin == 5)
        {
            if (action.ObjetivoId.HasValue)
            {
                if (action.ObjetivoId > Constant.DefaultId)
                {
                    var objetivo = Objetivo.ById(Convert.ToInt32(action.ObjetivoId.Value), action.CompanyId);
                    origin = objetivo.Name;
                    originSufix = " (" + dictionary["Item_Objetivo"] + ")";
                }
            }
        }
        if (action.Origin == 6)
        {
            if (action.Oportunity.Id > Constant.DefaultId)
            {
                 var oportunidad = Oportunity.ById(Convert.ToInt32(action.Oportunity.Id), action.CompanyId);
                    origin = oportunidad.Description;
                    originSufix = " (" + dictionary["Item_Oportunity"] + ")";
            }
        }
        else
        {
            switch (action.ReporterType)
            {
                case 1:
                    reporterType = dictionary["Item_IncidentAction_ReporterType1"];
                    if (action.Department != null && action.Department.Id > 0)
                    {
                        var department = Department.ById(action.Department.Id, action.CompanyId);
                        reporter = department.Description;
                    }
                    break;
                case 2:
                    reporterType = dictionary["Item_IncidentAction_ReporterType2"];
                    if (action.Provider != null && action.Provider.Id > 0)
                    {
                        var provider = Provider.ById(action.Provider.Id, action.CompanyId);
                        reporter = provider.Description;
                    }
                    break;
                case 3:
                    reporterType = dictionary["Item_IncidentAction_ReporterType3"];
                    if (action.Customer != null && action.Customer.Id > 0)
                    {
                        var customer = Customer.ById(action.Customer.Id, action.CompanyId);
                        reporter = customer.Description;
                    }
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

        // Descripción
        table.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Description"], Rectangle.NO_BORDER));
        table.AddCell(ValueCell(action.Description, ToolsPdf.BorderNone, alignLeft, 1));

        // Tipo
        table.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Type"], Rectangle.NO_BORDER));
        table.AddCell(ValueCell(type, ToolsPdf.BorderNone, alignLeft, 1));

        // Origen
        table.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Origin"], Rectangle.NO_BORDER));
        table.AddCell(ValueCell(origin + originSufix, ToolsPdf.BorderNone, alignLeft, 3));

        // Reportador
        if (action.Origin != 4 && action.Origin != 5 && !string.IsNullOrEmpty(reporter))
        {
            table.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Reporter"], Rectangle.NO_BORDER));
            table.AddCell(ValueCell(reporterType + " (" + reporter + ")", ToolsPdf.BorderNone, alignLeft, 3));
        }

        // WhatHappend
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_WhatHappened"]));
        table.AddCell(TextAreaCell(Environment.NewLine + action.WhatHappened, borderSides, alignLeft, 4));
        table.AddCell(BlankRow());
        table.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + action.WhatHappenedBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], action.WhatHappenedOn), borderBR, alignRight, 2));

        // Causes
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Causes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + action.Causes, borderSides, alignLeft, 4));
        table.AddCell(BlankRow());
        table.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + action.CausesBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], action.CausesOn), borderBR, alignRight, 2));

        // Actions
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Actions"]));
        table.AddCell(TextAreaCell(Environment.NewLine + action.Actions, borderSides, alignLeft, 4));
        table.AddCell(BlankRow());
        table.AddCell(TextAreaCell(dictionary["Item_IncidentAction_Field_Responsible"] + ": " + action.ActionsBy.FullName, borderBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_DateExecution"], action.ActionsOn), borderBR, alignRight, 2));

        // Monitoring
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Monitoring"]));
        table.AddCell(TextAreaCell(Environment.NewLine + action.Monitoring, ToolsPdf.BorderAll, alignLeft, 4));

        // Close
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Close"]));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1}",dictionary["Item_IncidentAction_Field_Responsible"] ,action.ClosedBy.FullName), borderTBL, alignLeft, 2));
        table.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "\n{0}: {1:dd/MM/yyyy}", dictionary["Common_DateClose"], action.ClosedOn), borderTBR, alignRight, 2));

        // Notes
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Notes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + action.Notes, ToolsPdf.BorderAll, alignLeft, 4));

        document.Add(table);

        // Costes
        var costs = IncidentActionCost.GetByIncidentActionId(actionId, companyId);
        if (costs.Count > 0)
        {
            var times = new Font(arial, 8, Font.NORMAL, BaseColor.BLACK);
            var fontSummary = new Font(arial, 9, Font.BOLD, BaseColor.BLACK);
            var headerFontFinal = new Font(headerFont, 9, Font.NORMAL, BaseColor.BLACK);
            
            // @alex: hay que crear la tabla con 6 columnas en lugar de 5
            //var tableCosts = new PdfPTable(5)
            var tableCosts = new PdfPTable(6)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 1,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };

            // @alex: se añade una nueva columna de 10f para la fecha
            //tableCosts.SetWidths(new float[] { 35f, 10f, 10f, 10f, 20f });
            tableCosts.SetWidths(new float[] { 35f, 10f, 10f, 10f, 10f, 20f });

            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Description"]));
            // @alex: se añade una nueva cabecera
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Common_Date"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Amount"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Quantity"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Total"]));
            tableCosts.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_ReportedBy"]));

            decimal total = 0;
            decimal totalAccion = 0;
            int cont = 0;
            int contAccion = 0;

            // Acciones
            foreach (var cost in costs.Where(c => c.Active == true))
            {
                tableCosts.AddCell(ToolsPdf.DataCell(cost.Description, times));

                // @alex: se añade la columna en la misma posición que en el listado de la ficha
                tableCosts.AddCell(ToolsPdf.DataCellCenter(cost.Date, times));

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
                Colspan = 3//@ alex: al haber una columna más el colspan crece de 2 a 3
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

            tableCosts.AddCell(new PdfPCell(new Phrase(Tools.PdfMoneyFormat(total), times))
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

            
            document.SetPageSize(PageSize.A4.Rotate());
            document.NewPage();
            document.Add(tableCosts);
        }

        document.Close();

        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=Accion.pdf");
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
            BackgroundColor = BaseColor.WHITE,
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