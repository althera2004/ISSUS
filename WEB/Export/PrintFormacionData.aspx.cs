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

public partial class Export_PrintFormacionData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        var learningId = Convert.ToInt32(Request.QueryString["id"].ToString());
        var companyId = Convert.ToInt32(Request.QueryString["companyId"].ToString());
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


        var fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Learning"],
            learning.Description,
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

        iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 40, 40, 65, 55);

        var writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Temp\\" + fileName, FileMode.Create));
        var PageEventHandler = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = learning.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Learning"]
        };

        writer.PageEvent = PageEventHandler;

        var borderSides = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;


        document.Open();
        iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
        iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

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
            case 0: statusText = dictionary["Item_Learning_Status_InProgress"]; break;
            case 1: statusText = dictionary["Item_Learning_Status_Finished"]; break;
            case 2: statusText = dictionary["Item_Learning_Status_Evaluated"]; break;
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
        /*var borderNone = iTS.Rectangle.NO_BORDER;
        var borderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;
        var borderTBL = iTS.Rectangle.TOP_BORDER + iTS.Rectangle.BOTTOM_BORDER + iTS.Rectangle.LEFT_BORDER;
        var borderTBR = iTS.Rectangle.TOP_BORDER + iTS.Rectangle.BOTTOM_BORDER + iTS.Rectangle.RIGHT_BORDER;*/
        iTS.BaseColor backgroundColor = new iTS.BaseColor(225, 225, 225);
        iTS.BaseColor rowPair = new iTS.BaseColor(255, 255, 255);
        iTS.BaseColor rowEven = new iTS.BaseColor(240, 240, 240);
        iTS.Font headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

        document.SetPageSize(PageSize.A4.Rotate());
        document.NewPage();

        iTSpdf.PdfPTable tableMaintenance = new iTSpdf.PdfPTable(3)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f
        };

        tableMaintenance.SetWidths(new float[] { 90f, 20f, 20f });
        
        tableMaintenance.AddCell(new PdfPCell(new Phrase(learning.Description, descriptionFont))
        {
            Colspan = 5,
            Border = Rectangle.NO_BORDER,
            PaddingTop = 20f,
            PaddingBottom = 20f,
            HorizontalAlignment = Element.ALIGN_CENTER
        });

        tableMaintenance.AddCell(ToolsPdf.HeaderCell(dictionary["Item_LearningAssistants"].ToUpperInvariant(), headerFontFinal));
        tableMaintenance.AddCell(ToolsPdf.HeaderCell(dictionary["Item_LearningAssistant_Status_Done"].ToUpperInvariant(), headerFontFinal));
        tableMaintenance.AddCell(ToolsPdf.HeaderCell(dictionary["Item_LearningAssistant_Status_Evaluated"].ToUpperInvariant(), headerFontFinal));

        int cont = 0;
        ReadOnlyCollection<Equipment> data = Equipment.GetList(companyId);
        bool pair = false;
        iTS.Font times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        learning.ObtainAssistance();
        foreach (var assistance in learning.Assistance)
        {

            int border = 0;
            BaseColor lineBackground = pair ? rowEven : rowPair;

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(assistance.Employee.FullName, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                Padding = 6f,
                PaddingTop = 4f
            });

            string completedText = dictionary["Common_Internal"];
            if (assistance.Completed.HasValue)
            {
                completedText = assistance.Completed.Value ? dictionary["Common_Yes"] : dictionary["Common_No"];
            }

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(completedText, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 6f,
                PaddingTop = 4f
            });

            string successText = string.Empty;
            if (assistance.Success.HasValue)
            {
                successText = assistance.Success.Value ? dictionary["Common_Yes"] : dictionary["Common_No"];
            }
            
            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(successText, times))
            {
                Border = border,
                BackgroundColor = lineBackground,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 6f,
                PaddingTop = 4f
            });
            cont++;
        }

        // TotalRow
        if(learning.Assistance.Count == 0)
        {
            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_LearningList_NoAssistants"], descriptionFont))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = rowEven,
                Padding = 6f,
                PaddingTop = 4f,
                Colspan = 3,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
        }

        tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(
            CultureInfo.InvariantCulture,
            @"{0}: {1}",
            dictionary["Common_RegisterCount"],
            cont), times))
        {
            Border = iTS.Rectangle.TOP_BORDER,
            BackgroundColor = rowEven,
            Padding = 6f,
            PaddingTop = 4f,
            Colspan = 3
        });

        document.Add(tableMaintenance);
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
        Font valueFont = new Font(this.headerFont, 11, Font.BOLD, BaseColor.BLACK);
        return new PdfPCell(new Phrase(value.ToUpperInvariant(), valueFont))
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
        Font valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);
        return new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0}:", value.ToUpperInvariant()), valueFont))
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
        Font valueFont = new Font(this.arial, 10, Font.BOLD, BaseColor.BLACK);
        return new PdfPCell(new Phrase(value, valueFont))
        {
            Colspan = colsPan,
            HorizontalAlignment = Element.ALIGN_LEFT,
            Padding = 8,
            PaddingTop = 6,
            Border = Rectangle.NO_BORDER
        };
    }

    private PdfPCell TextAreaCell(string value, int borders, int align, int colSpan)
    {
        Font valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);
        return new PdfPCell(new Phrase(value, valueFont))
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
        Font valueFont = new Font(this.headerFont, 11, Font.NORMAL, BaseColor.BLACK);
        return new PdfPCell(new Phrase(value, valueFont))
        {
            Colspan = 6,
            HorizontalAlignment = Element.ALIGN_CENTER,
            BackgroundColor = new BaseColor(225, 225, 225),
            Padding = 8,
            PaddingTop = 6
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