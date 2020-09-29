// --------------------------------
// <copyright file="PrintEquipmentData.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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

public partial class ExportPrintAuditoryDataExterna : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        long auditoryId = Convert.ToInt64(Request.QueryString["id"]);
        int companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var auditory = Auditory.ById(auditoryId, user.CompanyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }



        string formatedDescription = auditory.Description.Replace("?", string.Empty);
        formatedDescription = formatedDescription.Replace("#", string.Empty);
        formatedDescription = formatedDescription.Replace("/", string.Empty);
        formatedDescription = formatedDescription.Replace("\\", string.Empty);
        formatedDescription = formatedDescription.Replace(":", string.Empty);
        formatedDescription = formatedDescription.Replace(";", string.Empty);
        formatedDescription = formatedDescription.Replace(".", string.Empty);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Auditory"],
            formatedDescription,
            DateTime.Now);

        // FONTS
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
        var pageEventHandler = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = auditory.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Auditory"]
        };

        writer.PageEvent = pageEventHandler;
        document.Open();

        #region Dades bàsiques
        // Ficha pincipal
        var table = new PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };

        table.SetWidths(new float[] { 30f, 50f, 30f, 50f });

        table.AddCell(new PdfPCell(new Phrase(auditory.Description, descriptionFont))
        {
            Colspan = 4,
            Border = Rectangle.NO_BORDER,
            PaddingTop = 20f,
            PaddingBottom = 20f,
            HorizontalAlignment = Element.ALIGN_CENTER
        });

        table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_Type"]));
        table.AddCell(TitleData(dictionary["Item_Adutory_Type_Label_" + auditory.Type.ToString()]));

        if (auditory.Provider.Id > 0)
        {
            table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_Provider"]));
            table.AddCell(TitleData(auditory.Provider.Description));
        }
        else if (auditory.Customer.Id > 0)
        {
            table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_Customer"]));
            table.AddCell(TitleData(auditory.Customer.Description));
        }
        else
        {
            table.AddCell(TitleData(string.Empty, 2));
        }

        table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_Rules"]));
        var rulesText = string.Empty;
        bool firstRule = true;
        foreach (var rule in auditory.Rules)
        {
            if (firstRule)
            {
                firstRule = false;
            }
            else
            {
                rulesText += ", ";
            }

            rulesText += rule.Description;
        }
        table.AddCell(TitleData(rulesText, 3));

        table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_InternalResponsible"]));
        table.AddCell(TitleData(auditory.InternalResponsible.FullName, 3));

        //table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_PannedDate"]));
        //table.AddCell(TitleData(previewDateText));

        table.AddCell(TitleLabel(dictionary["Item_Audiotry_PDF_Planned"]));
        if (auditory.PlannedBy.Id > 0)
        {
            var planningText = string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2}",
                auditory.PlannedBy.FullName,
                dictionary["Item_Auditory_PDF_Label_date"],
                string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", auditory.PlannedOn) ?? "-");
            table.AddCell(TitleData(planningText, 3));
        }
        else
        {
            table.AddCell(TitleData(string.Empty, 3));
        }

        //table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_PlanningDate"]));
        //table.AddCell(TitleData(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", auditory.PlannedOn) ?? "-"));

        table.AddCell(TitleLabel(dictionary["Item_Auditory_PDF_Closing"]));
        if (auditory.ClosedBy.Employee.Id > 0)
        {
            var closedText = string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2}",
                auditory.ClosedBy.Employee.FullName,
                dictionary["Item_Auditory_PDF_Label_date"],
                string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", auditory.ClosedOn) ?? "-");
            table.AddCell(TitleData(closedText, 3));
        }
        else
        {
            table.AddCell(TitleData(string.Empty, 3));
        }

        //table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_ClosedOn"]));
        //table.AddCell(TitleData(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", auditory.ClosedOn) ?? "-"));

        table.AddCell(TitleLabel(dictionary["Item_Auditory_PDF_Validation"]));
        if (auditory.ValidatedBy.Id > 0)
        {
            var validatedText = string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2}",
                auditory.ValidatedBy.FullName,
                dictionary["Item_Auditory_PDF_Label_date"],
                string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", auditory.ValidatedOn) ?? "-");
            table.AddCell(TitleData(validatedText, 3));
        }
        else
        {
            table.AddCell(TitleData(string.Empty, 3));
        }

        //table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_ValidatedOn"]));
        //table.AddCell(TitleData(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", auditory.ValidatedOn) ?? "-"));

        table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_Address"]));
        table.AddCell(TitleData(auditory.EnterpriseAddress, 3));

        table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_AuditorTeam"]));
        table.AddCell(TitleData(auditory.AuditorTeam, 3));

        table.AddCell(TitleLabel(dictionary["Item_Auditory_Label_Notes"]));
        table.AddCell(TitleData(string.IsNullOrEmpty(auditory.Notes.Trim()) ? "-" : auditory.Notes, 3));

        document.Add(table);
        #endregion

        if (auditory.Status > 3)
        {
            document.SetPageSize(PageSize.A4.Rotate());
            document.NewPage();


            var widthsActions = new float[] { 5f, 30f, 5f };
            var tableAccions = new PdfPTable(widthsActions.Length)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };

            tableAccions.SetWidths(widthsActions);
            tableAccions.AddCell(new PdfPCell(new Phrase(dictionary["Item_IncidentActions"], descriptionFont))
            {
                Colspan = 3,
                Border = Rectangle.NO_BORDER,
                PaddingTop = 20f,
                PaddingBottom = 20f,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            tableAccions.AddCell(ToolsPdf.HeaderCell(dictionary["Tipo"]));
            tableAccions.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction"]));
            tableAccions.AddCell(ToolsPdf.HeaderCell(dictionary["Item_IncidentAction_Header_Open"]));

            var accions = IncidentAction.ByAuditoryId(auditory.Id, auditory.CompanyId);
            foreach (var accio in accions)
            {
                tableAccions.AddCell(ToolsPdf.DataCell(dictionary["Item_IncidentAction_Type" + accio.ActionType.ToString()]));
                tableAccions.AddCell(ToolsPdf.DataCell(accio.Description));
                tableAccions.AddCell(ToolsPdf.DataCell(accio.WhatHappenedOn));
            }

            document.Add(tableAccions);
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
}