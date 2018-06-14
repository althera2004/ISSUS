// --------------------------------
// <copyright file="PrintFormacionData.aspx.cs" company="Sbrinna">
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

public partial class ExportPrintFormacionData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        var learningId = Convert.ToInt32(Request.QueryString["id"]);
        var companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var learning = new Learning(learningId, companyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string formatedDescription = learning.Description.Replace("?", string.Empty);
        formatedDescription = formatedDescription.Replace("#", string.Empty);
        formatedDescription = formatedDescription.Replace("/", string.Empty);
        formatedDescription = formatedDescription.Replace("\\", string.Empty);
        formatedDescription = formatedDescription.Replace(":", string.Empty);
        formatedDescription = formatedDescription.Replace(";", string.Empty);
        formatedDescription = formatedDescription.Replace(".", string.Empty);

        var fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Learning"],
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
            CompanyId = learning.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Learning"]
        };

        writer.PageEvent = pageEventHandler;

        var borderSides = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;


        document.Open();

        #region Dades bàsiques
        // Ficha pincipal
        var table = new PdfPTable(6)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };

        table.SetWidths(new float[] { 30f, 30f, 30f, 30f, 30f, 30f });

        table.AddCell(TitleLabel(dictionary["Item_Learning_FieldLabel_Course"]));
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(learning.Description, descriptionFont))
        {
            Border = 0,
            BackgroundColor = new iTS.BaseColor(255, 255, 255),
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = Rectangle.ALIGN_LEFT,
            Colspan = 5
        });

        table.AddCell(TitleLabel(dictionary["Item_Learning_FieldLabel_EstimatedDate"]));
        table.AddCell(ToolsPdf.DataCell(learning.DateEstimated, descriptionFont));

        table.AddCell(TitleLabel(dictionary["Item_Learning_FieldLabel_Hours"]));
        table.AddCell(ToolsPdf.DataCell(learning.Hours, descriptionFont));

        string statusText = string.Empty;
        switch (learning.Status)
        {
            case 1: statusText = dictionary["Item_Learning_Status_Finished"]; break;
            case 2: statusText = dictionary["Item_Learning_Status_Evaluated"]; break;
            default: statusText = dictionary["Item_Learning_Status_InProgress"]; break;
        }

        table.AddCell(TitleLabel(dictionary["Item_Learning_ListHeader_Status"]));
        table.AddCell(ToolsPdf.DataCell(statusText, descriptionFont));

        table.AddCell(TitleLabel(dictionary["Item_Learning_FieldLabel_StartDate"]));
        table.AddCell(ToolsPdf.DataCell(learning.RealStart, descriptionFont));

        table.AddCell(TitleLabel(dictionary["Item_Learning_FieldLabel_EndDate"]));
        table.AddCell(ToolsPdf.DataCell(learning.RealFinish, descriptionFont));

        table.AddCell(TitleLabel(dictionary["Item_Learning_FieldLabel_Amount"]));
        table.AddCell(ToolsPdf.DataCellMoney(learning.Amount, descriptionFont));

        table.AddCell(TitleLabel(dictionary["Item_Learning_FieldLabel_Coach"]));
        table.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(learning.Master, descriptionFont))
        {
            Border = 0,
            BackgroundColor = new iTS.BaseColor(255, 255, 255),
            Padding = 6f,
            PaddingTop = 4f,
            HorizontalAlignment = Rectangle.ALIGN_LEFT,
            Colspan = 5
        });
        
        // Objective
        table.AddCell(SeparationRow());
        table.AddCell(TitleAreaCell(dictionary["Item_Learning_Objetive"]));
        table.AddCell(TextAreaCell(Environment.NewLine + learning.Objective, borderSides, Rectangle.ALIGN_LEFT, 6));

        // Methodology
        table.AddCell(SeparationRow());
        table.AddCell(TitleAreaCell(dictionary["Item_Learning_FieldLabel_Methodology"]));
        table.AddCell(TextAreaCell(Environment.NewLine + learning.Methodology, borderSides, Rectangle.ALIGN_LEFT, 6));

        // Notes
        table.AddCell(SeparationRow());
        table.AddCell(TitleAreaCell(dictionary["Item_Learning_FieldLabel_Notes"]));
        table.AddCell(TextAreaCell(Environment.NewLine + learning.Notes, borderSides, Rectangle.ALIGN_LEFT, 6));

        document.Add(table);
        #endregion

        #region Asistentes
        var backgroundColor = new iTS.BaseColor(225, 225, 225);
        var rowPair = new iTS.BaseColor(255, 255, 255);
        var rowEven = new iTS.BaseColor(240, 240, 240);
        var headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

        document.SetPageSize(PageSize.A4.Rotate());
        document.NewPage();

        var tableAssistants = new iTSpdf.PdfPTable(3)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f
        };

        tableAssistants.SetWidths(new float[] { 90f, 20f, 20f });        
        tableAssistants.AddCell(new PdfPCell(new Phrase(learning.Description, descriptionFont))
        {
            Colspan = 5,
            Border = Rectangle.NO_BORDER,
            PaddingTop = 20f,
            PaddingBottom = 20f,
            HorizontalAlignment = Element.ALIGN_CENTER
        });

        tableAssistants.AddCell(ToolsPdf.HeaderCell(dictionary["Item_LearningAssistants"]));
        tableAssistants.AddCell(ToolsPdf.HeaderCell(dictionary["Item_LearningAssistant_Status_Done"]));
        tableAssistants.AddCell(ToolsPdf.HeaderCell(dictionary["Item_LearningAssistant_Status_Evaluated"]));

        int cont = 0;
        var data = Equipment.GetList(companyId);
        bool pair = false;
        var times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        learning.ObtainAssistance();
        foreach (var assistance in learning.Assistance)
        {
            int border = 0;
            var lineBackground = pair ? rowEven : rowPair;

            tableAssistants.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(assistance.Employee.FullName, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = ToolsPdf.PaddingTableCell,
                PaddingTop = ToolsPdf.PaddingTopTableCell
            });

            string completedText = string.Empty;
            if (assistance.Completed.HasValue)
            {
                completedText = assistance.Completed.Value ? dictionary["Common_Yes"] : dictionary["Common_No"];
            }

            tableAssistants.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(completedText, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = ToolsPdf.PaddingTableCell,
                PaddingTop = ToolsPdf.PaddingTopTableCell
            });

            string successText = string.Empty;
            if (assistance.Success.HasValue)
            {
                successText = assistance.Success.Value ? dictionary["Common_Yes"] : dictionary["Common_No"];
            }
            
            tableAssistants.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(successText, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = ToolsPdf.PaddingTableCell,
                PaddingTop = ToolsPdf.PaddingTopTableCell
            });
            cont++;
        }

        // TotalRow
        if(learning.Assistance.Count == 0)
        {
            tableAssistants.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_LearningList_NoAssistants"], descriptionFont))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = ToolsPdf.PaddingTableCell,
                PaddingTop = ToolsPdf.PaddingTopTableCell,
                Colspan = 3,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
        }

        tableAssistants.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = ToolsPdf.PaddingTableCell,
            PaddingTop = ToolsPdf.PaddingTopTableCell,
            Colspan = 3
        });

        document.Add(tableAssistants);
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
            Padding = ToolsPdf.PaddingTableCell,
            PaddingTop = ToolsPdf.PaddingTopTableCell,
            Border = Rectangle.BOTTOM_BORDER
        };
    }

    private PdfPCell TitleLabel(string value)
    {
        return new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0}:", value.ToUpperInvariant()), new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = 1,
            HorizontalAlignment = Element.ALIGN_RIGHT,
            Padding = ToolsPdf.PaddingTableCell,
            PaddingTop = ToolsPdf.PaddingTopTableCell,
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
            Padding = ToolsPdf.PaddingTableCell,
            PaddingTop = ToolsPdf.PaddingTopTableCell,
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

    private PdfPCell TitleAreaCell(string value)
    {
        return new PdfPCell(new Phrase(value, new Font(this.headerFont, 11, Font.NORMAL, BaseColor.BLACK)))
        {
            Colspan = 6,
            HorizontalAlignment = Element.ALIGN_CENTER,
            BackgroundColor = new BaseColor(225, 225, 225),
            Padding = ToolsPdf.PaddingTableCell,
            PaddingTop = ToolsPdf.PaddingTopTableCell
        };
    }

    private PdfPCell BlankRow()
    {
        return new PdfPCell(new Phrase("\n"))
        {
            Colspan = 6,
            Border = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER
        };
    }

    private PdfPCell SeparationRow()
    {
        return new PdfPCell(new Phrase("\n"))
        {
            Colspan = 6,
            Border = Rectangle.NO_BORDER
        };
    }
}