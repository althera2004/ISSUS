// --------------------------------
// <copyright file="ExportObjetivoExportData.aspx.cs" company="Sbrinna">
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
using iTextSharp.text;
using PDF_Tests;
using iTextSharp.text.pdf;

public partial class ExportObjetivoExportData : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int objetivoId = Convert.ToInt32(Request.QueryString["id"]);
        int companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var objetivo = Objetivo.ById(objetivoId, user.CompanyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var formatedDescription = ToolsPdf.NormalizeFileName(objetivo.Name);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Objetivo"],
            formatedDescription,
            DateTime.Now);

        string type = string.Empty;
        string origin = string.Empty;
        string originSufix = string.Empty;
        string reporterType = string.Empty;
        string reporter = string.Empty;

        

        string status = string.Empty;
        var document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 65, 55);
        var writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Temp\\" + fileName, FileMode.Create));
        writer.PageEvent = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = objetivo.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Objetivo"].ToUpperInvariant()
        };

        document.Open();

        var table = new PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };
        table.SetWidths(new float[] { 15f, 30f, 15f, 30f });

        ToolsPdf.AddDataLabel(table, dictionary["Item_Objetivo_FieldLabel_Name"], objetivo.Name, 3);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Objetivo_FieldLabel_Responsible"], objetivo.Responsible.FullName, 3);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Objetivo_FieldLabel_DateStart"], objetivo.StartDate);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Objetivo_FieldLabel_ClosePreviewDate"], objetivo.PreviewEndDate);

        if(objetivo.VinculatedToIndicator)
        {
            var indicador = Indicador.ById(objetivo.IndicatorId.Value, companyId);
            ToolsPdf.AddDataLabel(table, dictionary["Item_Objetivo_FieldLabel_Indicator"], indicador.Description);
            ToolsPdf.AddDataLabel(table, dictionary["Item_Objetivo_FieldLabel_Periodicity"], indicador.Periodicity);
        }

        AddTextArea(table, dictionary["Item_Objetivo_FieldLabel_Description"], objetivo.Description, 4);
        AddTextArea(table, dictionary["Item_Objetivo_FieldLabel_Methodology"], objetivo.Methodology, 4);
        AddTextArea(table, dictionary["Item_Objetivo_FieldLabel_Resources"], objetivo.Resources, 4);
        AddTextArea(table, dictionary["Item_Objetivo_FieldLabel_Notes"], objetivo.Notes, 4);

        document.Add(table);

        #region Acciones
        var acciones = IncidentAction.ByObjetivoId(objetivoId, companyId);
        if (acciones.Count > 0)
        {

            document.SetPageSize(PageSize.A4.Rotate());
            document.NewPage();

            var tableAcciones = new PdfPTable(5)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 1,
                SpacingBefore = 20f
            };

            tableAcciones.SetWidths(new float[] { 20f, 120f, 20f, 20f, 20f });
            ToolsPdf.AddTableTitle(tableAcciones, dictionary["Item_Objetivo_RecordsReportTitle"]);
            tableAcciones.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Status"]));
            tableAcciones.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Open"]));
            tableAcciones.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Description"]));
            tableAcciones.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_ImplementDate"]));
            tableAcciones.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Cost"]));
                                   
            int cont = 0;
            foreach (var accion in acciones)
            {
                tableAcciones.AddCell(ToolsPdf.DataCell(accion.Status));
                tableAcciones.AddCell(ToolsPdf.DataCell(accion.WhatHappenedOn));
                tableAcciones.AddCell(ToolsPdf.DataCell(accion.Description));
                tableAcciones.AddCell(ToolsPdf.DataCell(accion.ActionsOn));
                tableAcciones.AddCell(ToolsPdf.DataCell(0));
                cont++;
            }

            // TotalRow
            tableAcciones.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant() + ":", ToolsPdf.LayoutFonts.TimesBold))
            {
                Border = Rectangle.TOP_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8f
            });

            tableAcciones.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", cont), ToolsPdf.LayoutFonts.TimesBold))
            {
                Border = Rectangle.TOP_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 8f
            });

            tableAcciones.AddCell(new PdfPCell(new Phrase(string.Empty)) { Colspan = 3, Border = Rectangle.TOP_BORDER });

            document.Add(tableAcciones);
        }
        #endregion

        #region Registros
        if (objetivo.VinculatedToIndicator)
        {
            var registros = IndicadorRegistro.ByIndicadorId(objetivo.IndicatorId.Value, companyId).ToList();
            if (registros.Count > 0)
            {

                document.SetPageSize(PageSize.A4.Rotate());
                document.NewPage();

                var tableRegistros = new PdfPTable(5)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 1,
                    SpacingBefore = 20f
                };

                tableRegistros.SetWidths(new float[] { 20f, 20f, 120f, 40f, 50f });
                ToolsPdf.AddTableTitle(tableRegistros, dictionary["Item_Objetivo_RecordsReportTitle"]);
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Value"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Date"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Comments"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Meta"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Responsible"]));

                int cont = 0;
                foreach (var registro in registros)
                {
                    string meta = dictionary["Common_Comparer_" + registro.MetaComparer] + " " + registro.Meta.ToString();
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Value));
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Date));
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Comments));
                    tableRegistros.AddCell(ToolsPdf.DataCell(meta));
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Responsible.FullName));
                    cont++;
                }

                // TotalRow
                tableRegistros.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant() + ":", ToolsPdf.LayoutFonts.TimesBold))
                {
                    Border = Rectangle.TOP_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8f
                });

                tableRegistros.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", cont), ToolsPdf.LayoutFonts.TimesBold))
                {
                    Border = Rectangle.TOP_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 8f
                });

                tableRegistros.AddCell(new PdfPCell(new Phrase(string.Empty)) { Colspan = 3, Border = Rectangle.TOP_BORDER });

                document.Add(tableRegistros);
            }
        }
        else
        {
            var registros = ObjetivoRegistro.GetByObjetivo(objetivoId, companyId).ToList();
            if (registros.Count > 0)
            {

                document.SetPageSize(PageSize.A4.Rotate());
                document.NewPage();

                var tableRegistros = new PdfPTable(5)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 1,
                    SpacingBefore = 20f
                };

                tableRegistros.SetWidths(new float[] { 20f, 120f, 20f, 40f, 50f });

                tableRegistros.AddCell(new PdfPCell(new Phrase(dictionary["Item_Objetivo_RecordsReportTitle"]))
                {
                    Colspan = 5,
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 20f,
                    PaddingBottom = 20f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Value"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Date"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Comments"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Meta"]));
                tableRegistros.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_TableRecords_Header_Responsible"]));

                int cont = 0;
                foreach (var registro in registros)
                {
                    string meta = dictionary["Common_Comparer_" + registro.MetaComparer] + " " + registro.Meta.ToString();
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Value));
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Date));
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Comments));
                    tableRegistros.AddCell(ToolsPdf.DataCell(meta));
                    tableRegistros.AddCell(ToolsPdf.DataCell(registro.Responsible.FullName));
                    cont++;
                }

                // TotalRow
                tableRegistros.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant() + ":", ToolsPdf.LayoutFonts.TimesBold))
                {
                    Border = Rectangle.TOP_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8f
                });

                tableRegistros.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", cont), ToolsPdf.LayoutFonts.TimesBold))
                {
                    Border = Rectangle.TOP_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 8f
                });

                tableRegistros.AddCell(new PdfPCell(new Phrase(string.Empty)) { Colspan = 3, Border = Rectangle.TOP_BORDER });

                document.Add(tableRegistros);
            }
        }
        #endregion

        #region Historico
        var historico = ObjetivoHistorico.ByObjetivoId(objetivoId);
        if (historico.Count > 0)
        {

            document.SetPageSize(PageSize.A4.Rotate());
            document.NewPage();

            var tableHistorico = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 1,
                SpacingBefore = 20f
            };

            tableHistorico.SetWidths(new float[] { 20f, 20f, 120f, 50f });
            ToolsPdf.AddTableTitle(tableHistorico, dictionary["Item_Objetivo_TabHistoric"]);
            tableHistorico.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_FieldLabel_Action"]));
            tableHistorico.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IndicatorRecord_FieldLabel_Date"]));
            tableHistorico.AddCell(ToolsPdf.HeaderCell(dictionary["Item_ObjetivoRecord_FieldLabel_Reason"]));
            tableHistorico.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Objetivo_FieldLabel_CloseResponsible"]));

            int cont = 0;
            foreach (var objetivoHistorico in historico)
            {
                var actionText = string.Empty;
                var description = string.Empty;
                
                if(objetivoHistorico.Reason == "Restore")
                {
                    actionText = dictionary["Item_ObjetivoHistorico_StatusRestore"];
                }
                else
                {
                    actionText = dictionary["Item_ObjetivoHistorico_StatusAnulate"];
                    description = objetivoHistorico.Reason;
                }

                tableHistorico.AddCell(ToolsPdf.DataCell(actionText));
                tableHistorico.AddCell(ToolsPdf.DataCell(objetivoHistorico.Date));
                tableHistorico.AddCell(ToolsPdf.DataCell(description));
                tableHistorico.AddCell(ToolsPdf.DataCell(objetivoHistorico.Employee.FullName));
                cont++;
            }

            // TotalRow
            tableHistorico.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant() + ":", ToolsPdf.LayoutFonts.TimesBold))
            {
                Border = Rectangle.TOP_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8f
            });

            tableHistorico.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", cont), ToolsPdf.LayoutFonts.TimesBold))
            {
                Border = Rectangle.TOP_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 8f
            });

            tableHistorico.AddCell(new PdfPCell(new Phrase(string.Empty)) { Colspan = 2, Border = Rectangle.TOP_BORDER });

            document.Add(tableHistorico);
        }
        #endregion

        document.Close();

        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=Accion.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(Request.PhysicalApplicationPath + "\\Temp\\" + fileName);
        Response.Flush();
        Response.Clear();
    }

    private void AddTextArea(PdfPTable table, string label, string value, int colSpan)
    {
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(label));
        table.AddCell(TextAreaCell(Environment.NewLine + value, ToolsPdf.BorderAll, Element.ALIGN_LEFT, colSpan));
    }

    private PdfPCell LabelCell(string label, int borders)
    {
        return new PdfPCell(new Phrase(label + ":", new Font(ToolsPdf.Arial, 10, Font.NORMAL, BaseColor.DARK_GRAY)))
        {
            Border = borders,
            HorizontalAlignment = 2
        };
    }

    private PdfPCell ValueCell(string value, int borders, int align, int colSpan)
    {
        return new PdfPCell(new Phrase(value, new Font(ToolsPdf.Arial, 10, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = colSpan,
            Border = borders,
            HorizontalAlignment = align
        };
    }

    private PdfPCell TextAreaCell(string value, int borders, int align, int colSpan)
    {
        return new PdfPCell(new Phrase(value, new Font(ToolsPdf.Arial, 10, Font.NORMAL, BaseColor.BLACK)))
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
        return new PdfPCell(new Phrase(value, new Font(ToolsPdf.HeaderFont, 11, Font.NORMAL, BaseColor.BLACK)))
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