// --------------------------------
// <copyright file="PrintBusinessRiskData.aspx.cs" company="Sbrinna">
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
using System.Text;
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
using iTextSharp.text.pdf;
using PDF_Tests;

public partial class ExportPrintBusinessRiskData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        long businessRiskId = Convert.ToInt64(Request.QueryString["id"]);
        int companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var businessRisk = BusinessRisk.ById( user.CompanyId, businessRiskId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string formatedDescription = businessRisk.Description.Replace("?", string.Empty);
        formatedDescription = formatedDescription.Replace("#", string.Empty);
        formatedDescription = formatedDescription.Replace("/", string.Empty);
        formatedDescription = formatedDescription.Replace("\\", string.Empty);
        formatedDescription = formatedDescription.Replace(":", string.Empty);
        formatedDescription = formatedDescription.Replace(";", string.Empty);
        formatedDescription = formatedDescription.Replace(".", string.Empty);
        formatedDescription = formatedDescription.Replace("\"", "ʺ");

        var alignLeft = Element.ALIGN_LEFT;

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_BusinessRisk"],
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
            CompanyId = businessRisk.CompanyId,
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

        table.AddCell(new PdfPCell(new Phrase(businessRisk.Description, descriptionFont))
        {
            Colspan = 4,
            Border = Rectangle.NO_BORDER,
            PaddingTop = 20f,
            PaddingBottom = 20f,
            HorizontalAlignment = Element.ALIGN_CENTER
        });

        table.AddCell(TitleCell(dictionary["Item_BusinessRisk_Tab_Basic"], 4));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_DateStart"]));
        table.AddCell(TitleData(string.Format(CultureInfo.InvariantCulture,@"{0:dd/MM/yyyy}", businessRisk.DateStart)));

        table.AddCell(TitleLabel(dictionary["Item_Process"]));
        table.AddCell(TitleData(businessRisk.Process.Description));

        table.AddCell(TitleLabel(dictionary["Item_Rule"]));
        table.AddCell(TitleData(businessRisk.Rules.Description));


        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_IPR"]));
        table.AddCell(TitleData(businessRisk.Rules.Limit.ToString()));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_Probability"]));
        table.AddCell(TitleData(businessRisk.StartProbability.ToString()));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_Severity"]));
        table.AddCell(TitleData(businessRisk.StartProbability.ToString()));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_Status"]));
        table.AddCell(TitleData(dictionary["Item_BusinessRisk_Status_Assumed"]));

        // WhatHappend
        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_BusinessRisk_LabelField_Description"]));
        table.AddCell(TextAreaCell(Environment.NewLine + businessRisk.Description, ToolsPdf.BorderAll, alignLeft, 4));

        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_BusinessRisk_LabelField_Causes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + businessRisk.Causes, ToolsPdf.BorderAll, alignLeft, 4));

        table.AddCell(SeparationRow());
        table.AddCell(TitleCell(dictionary["Item_BusinessRisk_LabelField_Notes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + businessRisk.Notes, ToolsPdf.BorderAll, alignLeft, 4));



        table.AddCell(TitleCell(dictionary["Item_BusinessRisk_Tab_Graphics"], 4));


        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_IPR"]));
        table.AddCell(TitleData(businessRisk.Rules.Limit.ToString()));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_Probability"]));
        table.AddCell(TitleData(businessRisk.FinalProbability.ToString()));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_Status"]));
        table.AddCell(TitleData(dictionary["Item_BusinessRisk_Status_Assumed"]));

        table.AddCell(TitleLabel(dictionary["Item_BusinessRisk_LabelField_Status"]));
        table.AddCell(TitleData(dictionary["Item_BusinessRisk_Status_Assumed"]));



        document.Add(table);
        #endregion

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
}