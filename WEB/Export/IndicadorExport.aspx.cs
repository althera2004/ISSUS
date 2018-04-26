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

public partial class ExportIndicadorExport : Page
{
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
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
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
            Title = dictionary["Item_Indicador_List"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(225, 225, 225);
        var rowPair = new iTS.BaseColor(255, 255, 255);
        var rowEven = new iTS.BaseColor(240, 240, 240);

        // ------------ FONTS 
        var times = new iTS.Font(ToolsPdf.Arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);        
        var headerFontFinal = new iTS.Font(ToolsPdf.HeaderFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        criteriaFont = new iTS.Font(ToolsPdf.Arial, 10, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var titleFont = new iTS.Font(ToolsPdf.Arial, 18, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var symbolFont = new iTS.Font(ToolsPdf.AwesomeFont, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

        var fontAwesomeIcon = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        fontAwe = new Font(fontAwesomeIcon, 10);
        // -------------------

        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), titleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });

        var table = new iTSpdf.PdfPTable(6)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        //------ CRITERIA
        var criteriatable = new iTSpdf.PdfPTable(6)
        {
            WidthPercentage = 100
        };
        criteriatable.SetWidths(new float[] { 20f, 50f, 25f, 50f, 20f, 150f });

        var criteriaBlank = new iTSpdf.PdfPCell(new iTS.Phrase(".", times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell
        };

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
            periode = string.Format(CultureInfo.InvariantCulture,@"{0:dd/MM/yyyy} {1:dd/MM/yyyy}", from, to);
        }
        else
        {
            periode = dictionary["Common_All_Male"];
        }
        
        /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"], timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell
        });*/

        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(dictionary["Common_Period"]));

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(periode, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell
        });

        /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_IncidentAction_Header_Status"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell
        });*/
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(dictionary["Item_IncidentAction_Header_Status"]));

        string statusText = dictionary["Common_All"];
        if (status == 1)
        {
            statusText = dictionary["Item_ObjetivoAction_List_Filter_ShowActive"];
        }

        if (status == 2)
        {
            statusText = dictionary["Item_ObjetivoAction_List_Filter_ShowClosed"];
        }

        /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(statusText, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell
        });*/
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(statusText));
        criteriatable.AddCell(criteriaBlank);
        criteriatable.AddCell(criteriaBlank);
        
        /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Indicaddor_Filter_TypeLabel"] + " :", timesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell
        });*/
        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(dictionary["Item_Indicaddor_Filter_TypeLabel"]));

        string indicadorTypeText = string.Empty;
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
            default:
                indicadorTypeText = dictionary["Common_All_Male_Plural"];
                break;
        }

        /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(indicadorTypeText, times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell
        });*/

        criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(indicadorTypeText));

        switch (indicatorType)
        {
            case 1:
                string processText = dictionary["Common_All_Male_Plural"];
                if (processId.HasValue)
                {
                    processText = new Process(processId.Value, companyId).Description;
                }

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
                            foreach (var t in ProcessType.ObtainByCompany(companyId, dictionary))
                            {
                                if (t.Id == processTypeId)
                                {
                                    processTypeCriteriaText = t.Description;
                                }
                            }

                            break;
                    }
                }

                /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Process"] + " :", timesBold))
                {
                    Border = ToolsPdf.BorderNone,
                    HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                    Padding = ToolsPdf.PaddingTopTableCell,
                    PaddingTop = ToolsPdf.PaddingTopCriteriaCell
                });*/
                criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(dictionary["Item_Process"]));

                criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(processText, times))
                {
                    Border = ToolsPdf.BorderNone,
                    HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                    Padding = ToolsPdf.PaddingTopTableCell,
                    PaddingTop = ToolsPdf.PaddingTopCriteriaCell
                });

                /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_ProcessType"] + " :", timesBold))
                {
                    Border = ToolsPdf.BorderNone,
                    HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                    Padding = ToolsPdf.PaddingTopTableCell,
                    PaddingTop = ToolsPdf.PaddingTopCriteriaCell
                });*/
                criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(dictionary["Item_ProcessType"]));

                criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(processTypeCriteriaText, times))
                {
                    Border = ToolsPdf.BorderNone,
                    HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                    Padding = ToolsPdf.PaddingTopTableCell,
                    PaddingTop = ToolsPdf.PaddingTopCriteriaCell
                });
                break;
            case 2:

                string objetivoText = dictionary["Common_All_Male_Plural"];
                if (targetId.HasValue)
                {
                    objetivoText = Objetivo.ById(targetId.Value, companyId).Name;
                }

                /*criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Objetivo"] + " :", timesBold))
                {
                    Border = ToolsPdf.BorderNone,
                    HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                    Padding = ToolsPdf.PaddingTopTableCell,
                    PaddingTop = ToolsPdf.PaddingTopCriteriaCell
                });*/
                criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(dictionary["Item_Objetivo"]));

                criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(objetivoText, times))
                {
                    Border = ToolsPdf.BorderNone,
                    HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                    Padding = ToolsPdf.PaddingTopTableCell,
                    PaddingTop = ToolsPdf.PaddingTopCriteriaCell,
                    Colspan = 3
                });
                break;
            default:
                criteriatable.AddCell(criteriaBlank);
                criteriatable.AddCell(criteriaBlank);
                criteriatable.AddCell(criteriaBlank);
                criteriatable.AddCell(criteriaBlank);
                break;
        }

        pdfDoc.Add(criteriatable);
        //---------------------------

        table.SetWidths(new float[] { 7f, 30f, 10f, 30f, 10f, 10f });;

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Common_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_Description"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_StartDate"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_Process"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_ProcessType"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Indicador_Header_ObjetivoResponsible"]));

        int cont = 0;
        var data = Indicador.Filter(companyId, indicatorType, from, to, processId, processTypeId, targetId, status).ToList();
        switch (listOrder.ToUpperInvariant())
        {
            default:
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

        foreach (var item in data)
        {
            string processTypeText = string.Empty;
            switch (item.Proceso.ProcessType)
            {
                default:
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

            table.AddCell(ToolsPdf.DataCell(item.Status == 0 ? dictionary["Common_Active"] : dictionary["Common_Inactive"], times));
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
            Padding = ToolsPdf.PaddingTopTableCell,
            PaddingTop = ToolsPdf.PaddingTopCriteriaCell,
            Colspan = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = backgroundColor,
            Colspan = 4
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
} 