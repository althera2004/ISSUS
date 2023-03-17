// --------------------------------
// <copyright file="ExportIndicadorExportData.aspx.cs" company="OpenFramework">
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
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class ExportIndicadorExportData : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int indicadorId = Convert.ToInt32(Request.QueryString["id"]);
        int companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var indicador = Indicador.ById(indicadorId, user.CompanyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var formatedDescription = ToolsPdf.NormalizeFileName(indicador.Description);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Indicador"],
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
            CompanyId = indicador.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Indicador"].ToUpperInvariant()
        };

        document.Open();

        var table = new PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };
        table.SetWidths(new float[] { 15f, 30f, 15f, 30f });

        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_Name"], indicador.Description, 3);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_Responsible"], indicador.Responsible.FullName, 3);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_StartDate"], indicador.StartDate);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_Periodicity"], indicador.Periodicity);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_Process"], indicador.Proceso.Description);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_Unidad"], indicador.Unidad.Description);

        string meta = dictionary["Common_Comparer_" + indicador.MetaComparer] + " " + string.Format(CultureInfo.InvariantCulture, "{0:##0.00}",  indicador.Meta);
        string alarma = string.Empty;
        if (!string.IsNullOrEmpty(indicador.AlarmaComparer))
        {
            alarma = dictionary["Common_Comparer_" + indicador.AlarmaComparer] + " " + string.Format(CultureInfo.InvariantCulture, "{0:##0.00}", indicador.Alarma);
        }

        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_Meta"], meta);
        ToolsPdf.AddDataLabel(table, dictionary["Item_Indicador_Field_Alarma"], alarma);

        AddTextArea(table, dictionary["Item_Indicador_Field_Calculo"], indicador.Calculo, 4);

        document.Add(table);

        #region Registros        
        var registros = IndicadorRegistro.ByIndicadorId(Convert.ToInt32(indicador.Id), companyId).ToList();
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
                string metaRegistro = dictionary["Common_Comparer_" + registro.MetaComparer] + " " + registro.Meta.ToString();
                tableRegistros.AddCell(ToolsPdf.DataCell(registro.Value));
                tableRegistros.AddCell(ToolsPdf.DataCell(registro.Date));
                tableRegistros.AddCell(ToolsPdf.DataCell(registro.Comments));
                tableRegistros.AddCell(ToolsPdf.DataCell(metaRegistro));
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
        #endregion

        if (user.HasGrantToRead(ApplicationGrant.IncidentActions))
        {
            #region Historico
            var historico = ObjetivoHistorico.ByObjetivoId(indicadorId);
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

                    if (objetivoHistorico.Reason == "Restore")
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