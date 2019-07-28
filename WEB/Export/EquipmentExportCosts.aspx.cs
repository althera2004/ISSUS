// --------------------------------
// <copyright file="EquipmentExportCosts.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.DataAccess;
using GisoFramework.Item;
using iTextSharp.text.pdf;
using PDF_Tests;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;

/// <summary>Generates PDF list based on equipment costs filter</summary>
public partial class ExportEquipmentExportCosts : Page
{
    /// <summary>Dictionary for fixed labels</summary>
    public static Dictionary<string, string> dictionary;

    /// <summary>Load event of page</summary>
    /// <param name="sender">Page loaded</param>
    /// <param name="e">Arguments of event</param>
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Generates PDF list based on equipment costs filter
    /// </summary>
    /// <param name="from">First date of costs</param>
    /// <param name="to">Last date of costs</param>
    /// <param name="filter">Indicates typology of costs and state of equipments</param>
    /// <param name="companyId">Company identifier</param>
    /// <returns>Link to generatoed PDF</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(string from, string to, string filter, int companyId)
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

        var fromDate = Tools.TextToDateYYYYMMDD(from);
        var toDate = Tools.TextToDateYYYYMMDD(to);

        var formatedDescription = ToolsPdf.NormalizeFileName(company.Name);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_EquipmentList"],
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

        writer.PageEvent = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, company.Id),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = company.Id,
            CompanyName = company.Name,
            Title = dictionary["Item_EquipmentListCosts"].ToUpperInvariant()
        };

        pdfDoc.Open();

        var backgroundColor = new iTS.BaseColor(225, 225, 225);
        var rowPair = new iTS.BaseColor(255, 255, 255);
        var rowEven = new iTS.BaseColor(240, 240, 240);

        var titleTable = new iTSpdf.PdfPTable(1);
        titleTable.SetWidths(new float[] { 50f });
        var titleCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_EquipmentList"], company.Name), ToolsPdf.LayoutFonts.TitleFont))
        {
            HorizontalAlignment = iTS.Element.ALIGN_CENTER,
            Border = iTS.Rectangle.NO_BORDER
        };

        titleTable.AddCell(titleCell);

        var CI = filter.IndexOf("|CI") != -1;
        var CE = filter.IndexOf("|CE") != -1;
        var VI = filter.IndexOf("|VI") != -1;
        var VE = filter.IndexOf("|VE") != -1;
        var MI = filter.IndexOf("|MI") != -1;
        var ME = filter.IndexOf("|ME") != -1;
        var RI = filter.IndexOf("|RI") != -1;
        var RE = filter.IndexOf("|RE") != -1;
        var AC = filter.IndexOf("|AC") != -1;
        var IN = filter.IndexOf("|IN") != -1;

        var numberColumns = 2;
        numberColumns += CI ? 1 : 0;
        numberColumns += CE ? 1 : 0;
        numberColumns += VI ? 1 : 0;
        numberColumns += VE ? 1 : 0;
        numberColumns += MI ? 1 : 0;
        numberColumns += ME ? 1 : 0;
        numberColumns += RI ? 1 : 0;
        numberColumns += RE ? 1 : 0;

        var table = new iTSpdf.PdfPTable(numberColumns)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        var widths = new List<float>();
        widths.Add(20f + (10 - numberColumns) * 5);
        if (CI) { widths.Add(5f); }
        if (CE) { widths.Add(5f); }
        if (VI) { widths.Add(5f); }
        if (VE) { widths.Add(5f); }
        if (MI) { widths.Add(5f); }
        if (ME) { widths.Add(5f); }
        if (RI) { widths.Add(5f); }
        if (RE) { widths.Add(5f); }

        widths.Add(10f);

        table.SetWidths(widths.ToArray());

        table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_Header_Description"]));
        if (CI) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Calibration_Int"])); }
        if (CE) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Calibration_Ext"])); }
        if (VI) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Verification_Int"])); }
        if (VE) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Verification_Ext"])); }
        if (MI) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Maintenance_Int"])); }
        if (ME) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Maintenance_Ext"])); }
        if (RI) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Repair_Int"])); }
        if (RE) { table.AddCell(ToolsPdf.HeaderCell(dictionary["Item_Equipment_FilterLabel_Repair_Ext"])); }
        table.AddCell(ToolsPdf.HeaderCell(dictionary["Common_Total"]));

        int cont = 0;
        var data = new List<EquipmentCost>();

        decimal totalCI = 0;
        decimal totalCE = 0;
        decimal totalVI = 0;
        decimal totalVE = 0;
        decimal totalMI = 0;
        decimal totalME = 0;
        decimal totalRI = 0;
        decimal totalRE = 0;

        using (var cmd = new SqlCommand("Equipment_GetCosts2"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@From", fromDate));
                cmd.Parameters.Add(DataParameter.Input("@to", toDate));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", company.Id));
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var active = rdr.GetBoolean(10);
                            if (active && !AC) { continue; }
                            if (!active && !IN) { continue; }

                            var newEquipmentCost = new EquipmentCost
                            {
                                Id = rdr.GetInt64(8),
                                Description = rdr.GetString(9),
                                Active = active,
                                CI = CI ? rdr.GetDecimal(0) : 0,
                                CE = CE ? rdr.GetDecimal(1) : 0,
                                VI = VI ? rdr.GetDecimal(2) : 0,
                                VE = VE ? rdr.GetDecimal(3) : 0,
                                MI = MI ? rdr.GetDecimal(4) : 0,
                                ME = ME ? rdr.GetDecimal(5) : 0,
                                RI = RI ? rdr.GetDecimal(6) : 0,
                                RE = RE ? rdr.GetDecimal(7) : 0
                            };

                            totalCI += newEquipmentCost.CI;
                            totalCE += newEquipmentCost.CE;
                            totalVI += newEquipmentCost.VI;
                            totalVE += newEquipmentCost.VE;
                            totalMI += newEquipmentCost.MI;
                            totalME += newEquipmentCost.ME;
                            totalRI += newEquipmentCost.RI;
                            totalRE += newEquipmentCost.RE;

                            data.Add(newEquipmentCost);
                        }
                    }
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }



    //------ CRITERIA
    var criteriatable = new iTSpdf.PdfPTable(2)
        {
            WidthPercentage = 100
        };

        criteriatable.SetWidths(new float[] { 8f, 50f });

        string statusText = string.Empty;
        string operativaText = string.Empty;

        if (filter.IndexOf("|AC") != -1 && filter.IndexOf("|IN") != -1)
        {
            statusText = dictionary["Common_All"];
        }
        else if (filter.IndexOf("|AC") != -1)
        {
            statusText = dictionary["Item_Equipment_List_Filter_ShowActive"];
        }
        else if (filter.IndexOf("|IN") != -1)
        {
            statusText = dictionary["Item_Equipment_List_Filter_ShowClosed"];
        }

        bool first = true;
        if (CI)
        {
            first = false;
            operativaText = dictionary["Item_Equipment_FilterLabel_Calibration_Int"];
        }
        if (CE)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_FilterLabel_Calibration_Ext"];
        }
        if (VI)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_FilterLabel_Verification_Int"];
        }
        if (VE)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_FilterLabel_Verification_Ext"];
        }
        if (MI)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_FilterLabel_Maintenance_Int"];
        }
        if (ME)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_FilterLabel_Maintenance_Ext"];
        }
        if (RI)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_FilterLabel_Repair_Int"];
        }
        if (RE)
        {
            if (!first) { operativaText += ", "; }
            first = false;
            operativaText += dictionary["Item_Equipment_FilterLabel_Repair_Ext"];
        }

        ToolsPdf.AddCriteria(criteriatable, dictionary["Item_Equipment_List_Filter_ShowByOperation"], operativaText);
        ToolsPdf.AddCriteria(criteriatable, dictionary["Item_Equipment_List_Filter_ShowByStatus"], statusText);

        bool pair = false;
        decimal total = 0;
        int border = 0;

        foreach (EquipmentCost equipment in data)
        {
            total += equipment.Total;

            var lineBackground = pair ? rowEven : rowPair;

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Description, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f
            });

            if (CI)
            {
                string CIText = string.Format("{0:#,##0.00}", equipment.CI);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(CIText, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            if (CE)
            {
                string CEText = string.Format("{0:#,##0.00}", equipment.CE);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(CEText, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            if (VI)
            {
                string VIText = string.Format("{0:#,##0.00}", equipment.VI);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(VIText, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            if (VE)
            {
                string VEText = string.Format("{0:#,##0.00}", equipment.VE);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(VEText, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            if (MI)
            {
                string MIText = string.Format("{0:#,##0.00}", equipment.MI);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(MIText, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            if (ME)
            {
                string METext = string.Format("{0:#,##0.00}", equipment.ME);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(METext, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            if (RI)
            {
                string RIText = string.Format("{0:#,##0.00}", equipment.RI);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(RIText, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            if (RE)
            {
                string REText = string.Format("{0:#,##0.00}", equipment.RE);
                table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(REText, ToolsPdf.LayoutFonts.Times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f,
                    HorizontalAlignment = 2
                });
            }

            string TotalText = string.Format("{0:#,##0.00}", equipment.Total);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(TotalText, ToolsPdf.LayoutFonts.Times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });

            cont++;
        }

        string totalRegistros = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont);
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalRegistros, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f
        });

        /*table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant(), ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });*/

        if (CI)
        {
            string totalTextCI = string.Format("{0:#,##0.00}", totalCI);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextCI, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        if (CE)
        {
            string totalTextCE = string.Format("{0:#,##0.00}", totalCE);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextCE, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        if (VI)
        {
            string totalTextVI = string.Format("{0:#,##0.00}", totalVI);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextVI, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        if (VE)
        {
            string totalTextVE = string.Format("{0:#,##0.00}", totalVE);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextVE, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        if (MI)
        {
            string totalTextMI = string.Format("{0:#,##0.00}", totalMI);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextMI, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        if (ME)
        {
            string totalTextME = string.Format("{0:#,##0.00}", totalME);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextME, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        if (RI)
        {
            string totalTextRI = string.Format("{0:#,##0.00}", totalRI);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextRI, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        if (RE)
        {
            string totalTextRE = string.Format("{0:#,##0.00}", totalRE);
            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalTextRE, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            });
        }

        string totalText = string.Format("{0:#,##0.00}", total);
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(totalText, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = 2
        });

        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Colspan = 2
        });

        pdfDoc.Add(criteriatable);
        pdfDoc.Add(table);
        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }

    private class EquipmentCost
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal CI { get; set; }
        public decimal CE { get; set; }
        public decimal VI { get; set; }
        public decimal VE { get; set; }
        public decimal MI { get; set; }
        public decimal ME { get; set; }
        public decimal RI { get; set; }
        public decimal RE { get; set; }
        public decimal Total
        {
            get
            {
                return CI + CE + VI + VE + MI + ME + RI + RE;
            }
        }
    }
}