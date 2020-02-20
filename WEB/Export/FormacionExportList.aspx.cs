// --------------------------------
// <copyright file="FormacionExportList.aspx.cs" company="OpenFramework">
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
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;

public partial class ExportFormacionExportList : Page
{
    public static Dictionary<string, string> dictionary;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(int companyId, string yearFrom, string yearTo, string mode, string listOrder, string textFilter)
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
            dictionary["Item_LearningList"],
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

        // evento para poner titulo y pie de página cada vez que salta de página
        writer.PageEvent = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, company.Id),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = company.Id,
            CompanyName = company.Name,
            Title = dictionary["Item_LearningList"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(225, 225, 225);
        var rowEven = new iTS.BaseColor(240, 240, 240);

        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), ToolsPdf.LayoutFonts.TitleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        });

        //------ CRITERIA
        var criteriatable = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100
        };
        criteriatable.SetWidths(new float[] { 3f, 12f, 3f, 22f });

        string periode = string.Empty;
        if (!string.IsNullOrEmpty(yearFrom) && yearFrom != "0" && !string.IsNullOrEmpty(yearTo) && yearTo != "0")
        {
            periode = yearFrom + " - " + yearTo;
        }
        else if (!string.IsNullOrEmpty(yearFrom) && yearFrom != "0")
        {
            periode = dictionary["Common_From"] + " " + yearFrom;
        }
        else if (!string.IsNullOrEmpty(yearTo) && yearTo != "0")
        {
            periode = dictionary["Item_Incident_List_Filter_To"] + " " + yearTo;
        }

        if (string.IsNullOrEmpty(periode))
        {
            periode = dictionary["Item_Learning_Filter_AllPeriode"];
        };

        string modeText = dictionary["Common_All_Female_Plural"];
        if (!string.IsNullOrEmpty(mode))
        {
            modeText = string.Empty;
            bool first = true;
            if (mode.IndexOf("0") != -1)
            {
                modeText += dictionary["Item_Learning_Status_InProgress"];
                first = false;
            }

            if (mode.IndexOf("1") != -1)
            {
                if (!first) { modeText += ", "; }
                modeText += dictionary["Item_Learning_Status_Started"];
                first = false;
            }
            if (mode.IndexOf("2") != -1)
            {
                if (!first) { modeText += ", "; }
                modeText += dictionary["Item_Learning_Status_Finished"];
                first = false;
            }
            if (mode.IndexOf("3") != -1)
            {
                if (!first) { modeText += ", "; }
                modeText += dictionary["Item_Learning_Status_Evaluated"];
            }
        }

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Period"] + " :", ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(periode, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Status"] + " :", ToolsPdf.LayoutFonts.TimesBold))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(modeText, ToolsPdf.LayoutFonts.Times))
        {
            Border = ToolsPdf.BorderNone,
            HorizontalAlignment = iTS.Element.ALIGN_LEFT,
            Padding = 6f,
            PaddingTop = 4f
        });

        if (!string.IsNullOrEmpty(textFilter))
        {
            criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_PDF_Filter_Contains"] + " :", ToolsPdf.LayoutFonts.TimesBold))
            {
                Border = ToolsPdf.BorderNone,
                HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                Padding = 6f,
                PaddingTop = 4f
            });

            criteriatable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(textFilter, ToolsPdf.LayoutFonts.Times))
            {
                Border = ToolsPdf.BorderNone,
                HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan =5
            });
        }

        pdfDoc.Add(criteriatable);

        var table = new iTSpdf.PdfPTable(5)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        table.SetWidths(new float[] { 15f, 5f, 5f, 5f, 5f });
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Course"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_EstimatedDate"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_ListHeader_DateComplete"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Status"]));
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Learning_FieldLabel_Cost"]));
        int cont = 0;

        DateTime? yFrom = null;
        DateTime? yTo = null;

        if (!string.IsNullOrEmpty(yearFrom) && yearFrom != "0")
        {
            // yFrom = Tools.TextToDate(yearFrom);
            var parts = yearFrom.Split('/');
            yFrom = new DateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[0]));
        }

        if (!string.IsNullOrEmpty(yearTo) && yearTo != "0")
        {
            // yTo = Tools.TextToDate(yearTo);
            var parts = yearTo.Split('/');
            yTo = new DateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[0]));
        }

        var learningFilter = new LearningFilter(companyId)
        {
            Pendent = mode.IndexOf("0") != -1,
            Started = mode.IndexOf("1") != -1,
            Finished = mode.IndexOf("2") != -1,
            Evaluated = mode.IndexOf("3") != -1,
            YearFrom = yFrom,
            YearTo = yTo
        };

        decimal totalCost = 0;
        int count = 0;

        var data = learningFilter.Filter().ToList();

        if (!string.IsNullOrEmpty(listOrder))
        {
            switch (listOrder.ToUpperInvariant())
            {
                default:
                case "TH0|ASC":
                    data = data.OrderBy(d => d.Description).ToList();
                    break;
                case "TH0|DESC":
                    data = data.OrderByDescending(d => d.Description).ToList();
                    break;
                case "TH1|ASC":
                    data = data.OrderBy(d => d.RealStart).ToList();
                    break;
                case "TH1|DESC":
                    data = data.OrderByDescending(d => d.RealStart).ToList();
                    break;
                case "TH2|ASC":
                    data = data.OrderBy(d => d.RealFinish).ToList();
                    break;
                case "TH2|DESC":
                    data = data.OrderByDescending(d => d.RealFinish).ToList();
                    break;
                case "TH3|ASC":
                    data = data.OrderBy(d => d.Status).ToList();
                    break;
                case "TH3|DESC":
                    data = data.OrderByDescending(d => d.Status).ToList();
                    break;
                case "TH4|ASC":
                    data = data.OrderBy(d => d.Amount).ToList();
                    break;
                case "TH4|DESC":
                    data = data.OrderByDescending(d => d.Amount).ToList();
                    break;
            }
        }

        // aplicar filtro de texto
        if (!string.IsNullOrEmpty(textFilter))
        {
            data = data.Where(d => d.Description.IndexOf(textFilter, StringComparison.OrdinalIgnoreCase) != -1).ToList();
        }

        foreach (var learning in data)
        {
            count++;
            learning.ObtainAssistance();
            string assist = string.Empty;
            bool first = true;
            foreach (var alumno in learning.Assistance)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    assist += ", ";
                }

                assist += alumno.Employee.FullName;
            }

            if (string.IsNullOrEmpty(assist))
            {
                assist = "(" + dictionary["Item_LearningList_NoAssistants"] + ")";
            }

            int border = 0;
            table.AddCell(ToolsPdf.DataCell(learning.Description));
            table.AddCell(ToolsPdf.DataCellCenter(learning.DateEstimated));
            /*
            string fecha = Tools.TranslatedMonth(learning.DateEstimated.Month, dictionary) + " " + learning.DateEstimated.Year;
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(fecha, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
            */
            if (learning.RealFinish != null)
            {
                if (learning.RealFinish.Value.Year == 1970)
                {
                    table.AddCell(ToolsPdf.DataCell(string.Empty));
                }
                else
                {
                    table.AddCell(ToolsPdf.DataCellCenter(learning.RealFinish));
                }
            }
            else
            {
                table.AddCell(ToolsPdf.DataCell(string.Empty));
            }

            string statusText = string.Empty;
            switch (learning.Status)
            {
                default:
                case 0: statusText = dictionary["Item_Learning_Status_InProgress"]; break;
                case 1: statusText = dictionary["Item_Learning_Status_Started"]; break;
                case 2: statusText = dictionary["Item_Learning_Status_Finished"]; break;
                case 3: statusText = dictionary["Item_Learning_Status_Evaluated"]; break;
            }

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(statusText, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                Padding = ToolsPdf.PaddingTableCell,
                PaddingTop = ToolsPdf.PaddingTopTableCell,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });

            table.AddCell(ToolsPdf.DataCellMoney(learning.Amount));
            totalCost += learning.Amount;

            cont++;
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            count);

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 3
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        string totalText = string.Format("{0:#,##0.00}", totalCost);
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalText, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }
}