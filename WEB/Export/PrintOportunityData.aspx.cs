// --------------------------------
// <copyright file="PrintOportunityData.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class ExportPrintOportunityData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        var borderSides = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER;
        var borderBL = Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderBR = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
        var borderTBL = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderTBR = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
        var alignRight = Element.ALIGN_RIGHT;

        long oportunityId = Convert.ToInt64(Request.QueryString["id"]);
        int companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var oportunity = Oportunity.ById(oportunityId, user.CompanyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var formatedDescription = ToolsPdf.NormalizeFileName(oportunity.Description);

        var alignLeft = Element.ALIGN_LEFT;

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Oportunity"],
            formatedDescription,
            DateTime.Now);

        var pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        this.headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        this.arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var descriptionFont = new Font(this.headerFont, 12, Font.BOLD, BaseColor.BLACK);
        var document = new iTextSharp.text.Document(PageSize.A4, 40, 40, 65, 55);

        var writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Temp\\" + fileName, FileMode.Create));
        var pageEventHandler = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = oportunity.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_BusinessRisk"]
        };

        writer.PageEvent = pageEventHandler;
        document.Open();

        #region Dades bàsiques
        var table = new PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };

        table.SetWidths(new float[] { 30f, 50f, 30f, 50f });

        table.AddCell(new PdfPCell(new Phrase(oportunity.Description, descriptionFont))
        {
            Colspan = 4,
            Border = Rectangle.NO_BORDER,
            PaddingTop = 20f,
            PaddingBottom = 20f,
            HorizontalAlignment = Element.ALIGN_CENTER
        });

        table.AddCell(TitleCell(dictionary["Item_BusinessRisk_Tab_Basic"], 4));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_DateStart"]));
        table.AddCell(TitleData(string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", oportunity.DateStart)));

        table.AddCell(TitleLabel(dictionary["Item_Process"]));
        table.AddCell(TitleData(oportunity.Process.Description));

        table.AddCell(TitleLabel(dictionary["Item_Rule"]));
        table.AddCell(TitleData(oportunity.Rule.Description));

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_IPR"]));
        table.AddCell(TitleData(oportunity.Rule.Limit.ToString()));

        string costText = oportunity.Cost.ToString();
        if (costText == "0")
        {
            costText = "-";
        }

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_Cost"]));
        table.AddCell(TitleData(costText));

        string impactText = oportunity.Impact.ToString();
        if (impactText == "0")
        {
            impactText = "-";
        }

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_Impact"]));
        table.AddCell(TitleData(impactText));

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_Status"]));
        table.AddCell(TitleData(dictionary["Item_BusinessRisk_Status_Assumed"]));

        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_Oportunity_LabelField_Description"]));
        table.AddCell(TextAreaCell(Environment.NewLine + oportunity.Description, ToolsPdf.BorderAll, alignLeft, 4));

        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_Oportunity_LabelField_Causes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + oportunity.Causes, ToolsPdf.BorderAll, alignLeft, 4));

        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_Oportunity_LabelField_Notes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + oportunity.Notes, ToolsPdf.BorderAll, alignLeft, 4));

        table.AddCell(TitleCell(dictionary["Item_Oportunity_Tab_Graphics"], 4));

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_IPR"]));
        table.AddCell(TitleData(oportunity.Rule.Limit.ToString()));

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_Status"]));
        table.AddCell(TitleData(dictionary["Item_BusinessRisk_Status_Assumed"]));

        string finalCostText = oportunity.FinalCost.ToString();
        if (finalCostText == "0")
        {
            finalCostText = "-";
        }

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_Cost"]));
        table.AddCell(TitleData(finalCostText));

        string finalImpactText = oportunity.FinalImpact.ToString();
        if (finalImpactText == "0")
        {
            finalImpactText = "-";
        }

        table.AddCell(TitleLabel(dictionary["Item_Oportunity_LabelField_Impact"]));
        table.AddCell(TitleData(finalImpactText));

        document.Add(table);
        #endregion

        if (user.HasGrantToRead(ApplicationGrant.IncidentActions))
        {
            // Añadir posible acción
            var action = IncidentAction.ByOportunityId(oportunity.Id, companyId);
            if (action.Id > 0)
            {
                var tableAction = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };

                tableAction.SetWidths(new float[] { 15f, 30f, 15f, 30f });

                // Descripción
                var headerFont = new Font(this.arial, 15, Font.NORMAL, BaseColor.BLACK);
                tableAction.AddCell(new PdfPCell(new Phrase(dictionary["Item_Incident_PDF_ActionPageTitle"], headerFont))
                {
                    Colspan = 4,
                    Border = ToolsPdf.BorderBottom,
                    HorizontalAlignment = Rectangle.ALIGN_CENTER
                });
                tableAction.AddCell(LabelCell(dictionary["Item_IncidentAction_Label_Description"], Rectangle.NO_BORDER));
                tableAction.AddCell(ValueCell(action.Description, ToolsPdf.BorderNone, alignLeft, 3));

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
                tableAction.AddCell(TextAreaCell(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", dictionary["Common_DateExecution"], actionDate), borderBR, alignRight, 2));

                // Monitoring
                tableAction.AddCell(SeparationRow());
                tableAction.AddCell(TitleCell(dictionary["Item_IncidentAction_Field_Monitoring"]));
                tableAction.AddCell(TextAreaCell(Environment.NewLine + action.Monitoring, ToolsPdf.BorderAll, alignLeft, 4));

                // Close
                var closedFullName = string.Empty;
                var closedDate = string.Empty;
                if (action.ClosedBy != null)
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
                tableAction.AddCell(TextAreaCell(Environment.NewLine + action.Notes, ToolsPdf.BorderAll, alignLeft, 4));

                document.NewPage();
                document.Add(tableAction);
            }

            #region Historico acciones
            var historico = IncidentAction.ByOportunityCode(oportunity.Code, company.Id).Where(ia => ia.Oportunity.Id != oportunity.Id).OrderBy(incidentAction => incidentAction.WhatHappenedOn).ToList();
            if (historico.Count > 0)
            {
                var backgroundColor = new iTS.BaseColor(225, 225, 225);
                var rowPair = new iTS.BaseColor(255, 255, 255);
                var rowEven = new iTS.BaseColor(240, 240, 240);
                var headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

                document.SetPageSize(PageSize.A4.Rotate());
                document.NewPage();

                var tableHistoric = new iTSpdf.PdfPTable(5)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 1,
                    SpacingBefore = 20f
                };

                tableHistoric.SetWidths(new float[] { 20f, 30f, 120f, 20f, 20f });

                tableHistoric.AddCell(new PdfPCell(new Phrase(dictionary["Item_BusinessRisk_Tab_HistoryActions"], descriptionFont))
                {
                    Colspan = 5,
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 20f,
                    PaddingBottom = 20f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                var valueFont = new Font(this.headerFont, 11, Font.BOLD, BaseColor.BLACK);
                tableHistoric.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Open"]));
                tableHistoric.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Status"]));
                tableHistoric.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Description"]));
                tableHistoric.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_ImplementDate"]));
                tableHistoric.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Close"]));

                int cont = 0;
                foreach (var accion in historico)
                {
                    string statusText = dictionary["Item_Incident_Status1"];

                    if (accion.CausesOn.HasValue)
                    {
                        statusText = dictionary["Item_Incident_Status2"];
                    }

                    if (accion.ActionsOn.HasValue)
                    {
                        statusText = dictionary["Item_Incident_Status3"];
                    }

                    if (accion.ClosedOn.HasValue)
                    {
                        statusText = dictionary["Item_Incident_Status4"];
                    }

                    tableHistoric.AddCell(ToolsPdf.DataCellCenter(accion.WhatHappenedOn, ToolsPdf.LayoutFonts.Times));
                    tableHistoric.AddCell(ToolsPdf.DataCell(statusText, ToolsPdf.LayoutFonts.Times));
                    tableHistoric.AddCell(ToolsPdf.DataCell(accion.Description, ToolsPdf.LayoutFonts.Times));
                    tableHistoric.AddCell(ToolsPdf.DataCellCenter(accion.ActionsOn, ToolsPdf.LayoutFonts.Times));
                    tableHistoric.AddCell(ToolsPdf.DataCellCenter(accion.ClosedOn, ToolsPdf.LayoutFonts.Times));

                    cont++;
                }

                tableHistoric.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant() + ": " + cont.ToString(), ToolsPdf.LayoutFonts.Times))
                {
                    Border = ToolsPdf.BorderTop,
                    Colspan = 5,
                    Padding = 8f
                });

                document.Add(tableHistoric);
            }
            #endregion

            #region Costes
            var costs = IncidentActionCost.ByOportunityId(oportunity.Id, company.Id);
            if (costs.Count > 0)
            {
                var backgroundColor = new iTS.BaseColor(225, 225, 225);
                var rowPair = new iTS.BaseColor(255, 255, 255);
                var rowEven = new iTS.BaseColor(240, 240, 240);
                var headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

                document.SetPageSize(PageSize.A4.Rotate());
                document.NewPage();

                var tableCost = new iTSpdf.PdfPTable(5)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 1,
                    SpacingBefore = 20f
                };

                tableCost.SetWidths(new float[] { 90f, 40f, 30f, 60f, 20f });

                tableCost.AddCell(new PdfPCell(new Phrase(dictionary["Item_Incident_Tab_Costs"], descriptionFont))
                {
                    Colspan = 5,
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 20f,
                    PaddingBottom = 20f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                var valueFont = new Font(this.headerFont, 11, Font.BOLD, BaseColor.BLACK);
                tableCost.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Description"]));
                tableCost.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Amount"]));
                tableCost.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Quantity"]));
                tableCost.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_Total"]));
                tableCost.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentCost_Header_ReportedBy"]));

                int cont = 0;
                decimal costTotal = 0;
                foreach (var cost in costs)
                {

                    tableCost.AddCell(ToolsPdf.DataCell(cost.Description, ToolsPdf.LayoutFonts.Times));
                    tableCost.AddCell(ToolsPdf.DataCellMoney(cost.Amount, ToolsPdf.LayoutFonts.Times));
                    tableCost.AddCell(ToolsPdf.DataCellMoney(cost.Quantity, ToolsPdf.LayoutFonts.Times));
                    tableCost.AddCell(ToolsPdf.DataCellMoney(cost.Amount * cost.Quantity, ToolsPdf.LayoutFonts.Times));
                    tableCost.AddCell(ToolsPdf.DataCell(cost.Responsible.FullName, ToolsPdf.LayoutFonts.Times));

                    costTotal += cost.Amount * cost.Quantity;
                    cont++;
                }

                tableCost.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_RegisterCount"].ToUpperInvariant() + ": " + cont.ToString(), ToolsPdf.LayoutFonts.Times))
                {
                    Border = ToolsPdf.BorderTop,
                    Colspan = 2,
                    Padding = 8f
                });

                tableCost.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant() + ":", ToolsPdf.LayoutFonts.Times))
                {
                    Border = ToolsPdf.BorderTop,
                    Colspan = 1,
                    Padding = 8f,
                    HorizontalAlignment = alignRight
                });

                tableCost.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(Tools.PdfMoneyFormat(costTotal), ToolsPdf.LayoutFonts.Times))
                {
                    Border = ToolsPdf.BorderTop,
                    Colspan = 1,
                    Padding = 8f,
                    HorizontalAlignment = alignRight
                });



                tableCost.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
                {
                    Border = ToolsPdf.BorderTop,
                    Colspan = 1,
                    Padding = 8f,
                    HorizontalAlignment = alignRight
                });

                document.Add(tableCost);
            }

            #endregion
        }

        document.Close();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=outfile.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(Request.PhysicalApplicationPath + "\\Temp\\" + fileName);
        Response.Flush();
        Response.Clear();
    }

    private PdfPCell TitleCell(string value)
    {
        return TitleCell(value, 4);
    }

    private PdfPCell TitleCell(string value, int colSpan)
    {
        return new PdfPCell(new Phrase(value.ToUpperInvariant(), new Font(this.headerFont, 11, Font.BOLD, BaseColor.BLACK)))
        {
            Colspan = colSpan,
            HorizontalAlignment = Element.ALIGN_LEFT,
            Padding = 8,
            PaddingTop = 6,
            Border = Rectangle.BOTTOM_BORDER
        };
    }

    private PdfPCell TitleLabel(string value)
    {
        return new PdfPCell(
            new Phrase(
                string.Format(
                    CultureInfo.InvariantCulture, "{0}:",
                    value.ToUpperInvariant()
                    ),
                new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = 1,
            HorizontalAlignment = Element.ALIGN_RIGHT,
            Padding = 8,
            PaddingTop = 6,
            Border = Rectangle.NO_BORDER
        };
    }

    private PdfPCell TitleData(string value)
    {
        return TitleData(value, 1);
    }

    private PdfPCell TitleData(string value, int colsPan)
    {
        return new PdfPCell(new Phrase(value, new Font(this.arial, 10, Font.BOLD, BaseColor.BLACK)))
        {
            Colspan = colsPan,
            HorizontalAlignment = Element.ALIGN_LEFT,
            Padding = 8,
            PaddingTop = 6,
            Border = Rectangle.NO_BORDER
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
}