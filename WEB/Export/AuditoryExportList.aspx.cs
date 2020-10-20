// --------------------------------
// <copyright file="AuditoryExportList.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace Issus.Export
{
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
    using PDF_Tests;
    using System.Data.SqlClient;
    using System.Data;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    public partial class AuditoryExportList : Page
    {
        public static Dictionary<string, string> Dictionary;

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static ActionResult PDF(
            int companyId,
            bool externa,
            string from,
            bool interna,
            bool provider,
            bool status0,
            bool status1,
            bool status2,
            bool status3,
            bool status4,
            bool status5,
            string to,
            string filterText,
            string listOrder)
        {
            var source = "AuditoryExportList";
            var res = ActionResult.NoAction;
            var user = HttpContext.Current.Session["User"] as ApplicationUser;
            Dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
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
                Dictionary["Item_Auditories"],
                formatedDescription,
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
                Title = Dictionary["Item_Auditories"].ToUpperInvariant()
            };

            pdfDoc.Open();

            var titleTable = new iTSpdf.PdfPTable(1);
            titleTable.SetWidths(new float[] { 50f });
            titleTable.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Dictionary["Item_EquipmentList"], company.Name), ToolsPdf.LayoutFonts.TitleFont))
            {
                HorizontalAlignment = iTS.Element.ALIGN_CENTER,
                Border = iTS.Rectangle.NO_BORDER
            });

            //------ CRITERIA
            var criteriatable = new iTSpdf.PdfPTable(4)
            {
                WidthPercentage = 100
            };

            criteriatable.SetWidths(new float[] { 10f, 50f, 10f, 80f });

            #region texts

            string criteriaProccess = Dictionary["Common_All_Male_Plural"];

            string periode = string.Empty;
            if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
            {
                periode = Dictionary["Item_Incident_List_Filter_From"] + " " + from;
            }
            else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                periode = Dictionary["Item_Incident_List_Filter_To"] + " " + to;
            }
            else if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                periode = from + " - " + to;
            }
            else
            {
                periode = Dictionary["Common_All_Male"];
            }

            string typetext = string.Empty;
            bool firtsType = false;
            if (interna)
            {
                typetext += Dictionary["Item_Adutory_Type_Label_0"];
                firtsType = false;
            }
            if (externa)
            {
                typetext += firtsType ? string.Empty : ", ";
                typetext += Dictionary["Item_Adutory_Type_Label_1"];
                firtsType = false;
            }
            if (provider)
            {
                typetext += firtsType ? string.Empty : ", ";
                typetext += Dictionary["Item_Adutory_Type_Label_2"];
            }
            if (firtsType)
            {
                typetext = Dictionary["Common_All_Female_Plural"];
            }

            string statustext = string.Empty;
            bool firstStatus = false;
            if (status0)
            {
                statustext += Dictionary["Item_Adutory_Status_Label_0"];
                firstStatus = false;
            }
            if (status1)
            {
                statustext += firstStatus ? string.Empty : ", ";
                statustext += Dictionary["Item_Adutory_Status_Label_1"];
                firstStatus = false;
            }
            if (status2)
            {
                statustext += firstStatus ? string.Empty : ", ";
                statustext += Dictionary["Item_Adutory_Status_Label_2"];
                firstStatus = false;
            }
            if (status3)
            {
                statustext += firstStatus ? string.Empty : ", ";
                statustext += Dictionary["Item_Adutory_Status_Label_3"];
                firstStatus = false;
            }
            if (status4)
            {
                statustext += firstStatus ? string.Empty : ", ";
                statustext += Dictionary["Item_Adutory_Status_Label_4"];
                firstStatus = false;
            }
            if (status5)
            {
                statustext += firstStatus ? string.Empty : ", ";
                statustext += Dictionary["Item_Adutory_Status_Label_5"];
            }
            if (firstStatus)
            {
                statustext = Dictionary["Common_All_Male_Plural"];
            }


            #endregion

            criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Common_Period"]));
            criteriatable.AddCell(ToolsPdf.CriteriaCellData(periode));
            criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Item_Auditory_Filter_Type"]));
            criteriatable.AddCell(ToolsPdf.CriteriaCellData(typetext));
            criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Item_Auditory_Filter_Status"]));
            criteriatable.AddCell(ToolsPdf.CriteriaCellData(statustext));
            if (!string.IsNullOrEmpty(filterText))
            {
                criteriatable.AddCell(ToolsPdf.CriteriaCellLabel(Dictionary["Common_PDF_Filter_Contains"]));
                criteriatable.AddCell(ToolsPdf.CriteriaCellData(filterText));
            }
            else
            {
                criteriatable.AddCell(ToolsPdf.CriteriaCellData(string.Empty));
                criteriatable.AddCell(ToolsPdf.CriteriaCellData(string.Empty));
            }

            pdfDoc.Add(criteriatable);
            //---------------------------

            var tableWidths = new float[] { 40f, 10f, 10f, 10f, 10f, 10f };
            var table = new iTSpdf.PdfPTable(tableWidths.Length)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 1,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };

            table.SetWidths(tableWidths);

            table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Auditory_ListHeader_Name"]));
            table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Auditory_Filter_Type"]));
            table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Auditory_ListHeader_Status"]));
            table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Auditory_ListHeader_Planned"]));
            table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Auditory_ListHeader_Closed"]));
            table.AddCell(ToolsPdf.HeaderCell(Dictionary["Item_Auditory_ListHeader_Ammount"]));

            int cont = 0;
            var data = new List<Auditory>();
            using (var cmd = new SqlCommand("Auditory_Filter"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@From", from));
                    cmd.Parameters.Add(DataParameter.Input("@To", to));
                    cmd.Parameters.Add(DataParameter.Input("@TypeInterna", interna));
                    cmd.Parameters.Add(DataParameter.Input("@TypeExterna", externa));
                    cmd.Parameters.Add(DataParameter.Input("@TypeProveedor", provider));
                    cmd.Parameters.Add(DataParameter.Input("@Status0", status0));
                    cmd.Parameters.Add(DataParameter.Input("@Status1", status1));
                    cmd.Parameters.Add(DataParameter.Input("@Status2", status2));
                    cmd.Parameters.Add(DataParameter.Input("@Status3", status3));
                    cmd.Parameters.Add(DataParameter.Input("@Status4", status4));
                    cmd.Parameters.Add(DataParameter.Input("@Status5", status5));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newAuditory = new Auditory
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryFilter.Id),
                                    Description = rdr.GetString(ColumnsAuditoryFilter.Nombre),
                                    Amount = rdr.GetDecimal(ColumnsAuditoryFilter.Amount),
                                    Status = rdr.GetInt32(ColumnsAuditoryFilter.Status),
                                    Type = rdr.GetInt32(ColumnsAuditoryFilter.Type)
                                };

                                if (!rdr.IsDBNull(ColumnsAuditoryFilter.PlannedOn))
                                {
                                    newAuditory.PlannedOn = rdr.GetDateTime(ColumnsAuditoryFilter.PlannedOn);
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryFilter.ValidatedOn))
                                {
                                    newAuditory.ValidatedOn = rdr.GetDateTime(ColumnsAuditoryFilter.ValidatedOn);
                                }

                                data.Add(newAuditory);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, source);
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

            switch (listOrder.ToUpperInvariant())
            {
                default:
                case "TH1|ASC":
                    data = data.OrderBy(d => d.Status).ToList();
                    break;
                case "TH1|DESC":
                    data = data.OrderByDescending(d => d.Status).ToList();
                    break;
                case "TH2|ASC":
                    data = data.OrderBy(d => d.Description).ToList();
                    break;
                case "TH2|DESC":
                    data = data.OrderByDescending(d => d.Description).ToList();
                    break;
                case "TH3|ASC":
                    data = data.OrderBy(d => d.PlannedOn).ToList();
                    break;
                case "TH3|DESC":
                    data = data.OrderByDescending(d => d.PlannedOn).ToList();
                    break;
                case "TH4|ASC":
                    data = data.OrderBy(d => d.ValidatedOn).ToList();
                    break;
                case "TH4|DESC":
                    data = data.OrderByDescending(d => d.ValidatedOn).ToList();
                    break;
                case "TH5|ASC":
                    data = data.OrderBy(d => d.Amount).ToList();
                    break;
                case "TH5|DESC":
                    data = data.OrderByDescending(d => d.Amount).ToList();
                    break;
            }

            foreach (var auditory in data)
            {
                if (!string.IsNullOrEmpty(filterText))
                {
                    var match = auditory.Description;
                    if (match.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        continue;
                    }
                }
                cont++;
                string statustextCell = string.Empty;
                switch (auditory.Status)
                {
                    case 0: statustextCell = Dictionary["Item_Adutory_Status_Label_0"]; break;
                    case 1: statustextCell = Dictionary["Item_Adutory_Status_Label_1"]; break;
                    case 2: statustextCell = Dictionary["Item_Adutory_Status_Label_2"]; break;
                    case 3: statustextCell = Dictionary["Item_Adutory_Status_Label_3"]; break;
                    case 4: statustextCell = Dictionary["Item_Adutory_Status_Label_4"]; break;
                    case 5: statustextCell = Dictionary["Item_Adutory_Status_Label_5"]; break;
                }

                string typetextCell = string.Empty;
                switch (auditory.Type)
                {
                    case 0: typetextCell = Dictionary["Item_Adutory_Type_Label_0"]; break;
                    case 1: typetextCell = Dictionary["Item_Adutory_Type_Label_1"]; break;
                    case 2: typetextCell = Dictionary["Item_Adutory_Type_Label_2"]; break;
                }

                table.AddCell(ToolsPdf.DataCell(auditory.Description));
                table.AddCell(ToolsPdf.DataCellCenter(typetextCell));
                table.AddCell(ToolsPdf.DataCellCenter(statustextCell));
                table.AddCell(ToolsPdf.DataCellCenter(auditory.PlannedOn));
                table.AddCell(ToolsPdf.DataCellCenter(auditory.ValidatedOn));
                table.AddCell(ToolsPdf.DataCellMoney(auditory.Amount));
            }

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
                CultureInfo.InvariantCulture,
                @"{0}: {1}",
                Dictionary["Common_RegisterCount"],
                cont), ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 4
            });

            table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Empty, ToolsPdf.LayoutFonts.Times))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                Colspan = 4
            });

            pdfDoc.Add(table);
            pdfDoc.CloseDocument();
            res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
            return res;
        }
    }
}