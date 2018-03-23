using iTextSharp.text;
using System;
// --------------------------------
// <copyright file="ToolsPdf.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    using iTS = iTextSharp.text;
    using iTSpdf = iTextSharp.text.pdf;
    using System.Globalization;
    using System.Web;

    public static class ToolsPdf
    {
        public const float PaddingTableCell = 8;
        public const float PaddingTopTableCell = 6;
        public const float PaddingTopCriteriaCell = 4;
        public static readonly BaseColor SummaryBackgroundColor = new BaseColor(240, 240, 240);
        public static readonly iTS.BaseColor LineBackgroundColor = new iTS.BaseColor(255, 255, 255);
        public static readonly iTS.BaseColor HeaderBackgroundColor = new iTS.BaseColor(220, 220, 220);
        public const int BorderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;
        public const int BorderNone = iTS.Rectangle.NO_BORDER;
        public const int BorderBottom = iTS.Rectangle.BOTTOM_BORDER;

        public static string FontPath
        {
            get
            {
                string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
                return string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts);
            }
        }

        public static readonly iTSpdf.BaseFont HeaderFont = iTSpdf.BaseFont.CreateFont(ToolsPdf.FontPath, iTSpdf.BaseFont.IDENTITY_H, iTSpdf.BaseFont.EMBEDDED);
        public static readonly iTSpdf.BaseFont Arial = iTSpdf.BaseFont.CreateFont(ToolsPdf.FontPath, iTSpdf.BaseFont.IDENTITY_H, iTSpdf.BaseFont.EMBEDDED);
        public static readonly iTSpdf.BaseFont AwesomeFont = iTSpdf.BaseFont.CreateFont(FontPath, iTSpdf.BaseFont.IDENTITY_H, iTSpdf.BaseFont.EMBEDDED);

        public static readonly iTS.Font TimesBold = new iTS.Font(ToolsPdf.Arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);

        public static iTSpdf.PdfPCell CriteriaCellLabel(string label)
        {
            return new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture,"{0} :", label), TimesBold))
            {
                Border = ToolsPdf.BorderNone,
                HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                Padding = ToolsPdf.PaddingTopTableCell,
                PaddingTop = ToolsPdf.PaddingTopCriteriaCell
            };
        }

        public static iTSpdf.PdfPCell HeaderCell(string label, iTS.Font font)
        {
            return new iTSpdf.PdfPCell(new iTS.Phrase(label.ToUpperInvariant(), font))
            {
                Border = BorderAll,
                BackgroundColor = HeaderBackgroundColor,
                HorizontalAlignment = iTS.Element.ALIGN_CENTER,
                Padding = PaddingTableCell,
                PaddingTop = PaddingTopTableCell
            };
        }

        public static iTSpdf.PdfPCell CellTable(string value, iTS.Font font)
        {
            return new iTSpdf.PdfPCell(new iTS.Phrase(value, font))
            {
                Border = iTS.Rectangle.TOP_BORDER,
                BackgroundColor = LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            };
        }

        public static iTSpdf.PdfPCell DataCellRight(string value, iTS.Font font)
        {
            return DataCell(value, font, iTS.Rectangle.ALIGN_RIGHT);
        }

        public static iTSpdf.PdfPCell DataCellRight(int value, iTS.Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, iTS.Rectangle.ALIGN_RIGHT);
        }

        public static iTSpdf.PdfPCell DataCellRight(long value, iTS.Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, iTS.Rectangle.ALIGN_RIGHT);
        }

        public static iTSpdf.PdfPCell DataCell(string value, iTS.Font font)
        {
            return DataCell(value, font, iTS.Rectangle.ALIGN_LEFT);
        }

        public static iTSpdf.PdfPCell DataCell(long value, iTS.Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, iTS.Rectangle.ALIGN_LEFT);
        }

        public static iTSpdf.PdfPCell DataCellMoney(decimal? value, iTS.Font font)
        {
            string valueText = string.Empty;
            if (value.HasValue)
            {
                valueText = Tools.PdfMoneyFormat(value.Value);
            }

            return DataCellRight(valueText, font);
        }

        public static iTSpdf.PdfPCell DataCellMoney(decimal value, iTS.Font font)
        {
            string valueText = Tools.PdfMoneyFormat(value);
            return DataCellRight(valueText, font);
        }

        public static iTSpdf.PdfPCell DataCell(DateTime value, iTS.Font font)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font);
        }

        public static iTSpdf.PdfPCell DataCell(DateTime value, iTS.Font font, int alignment)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        public static iTSpdf.PdfPCell DataCellCenter(string value, iTS.Font font)
        {
            return DataCell(value, font, iTS.Rectangle.ALIGN_CENTER);
        }

        public static iTSpdf.PdfPCell DataCellCenter(DateTime? value, iTS.Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, iTS.Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, iTS.Rectangle.ALIGN_CENTER);
        }

        public static iTSpdf.PdfPCell DataCell(DateTime? value, iTS.Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, iTS.Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font);
        }

        public static iTSpdf.PdfPCell DataCell(DateTime? value, iTS.Font font, int alignment)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, iTS.Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        public static iTSpdf.PdfPCell DataCell(string value, iTS.Font font, int alignment)
        {
            return new iTSpdf.PdfPCell(new iTS.Phrase(value, font))
            {
                Border = 0,
                BackgroundColor = new iTS.BaseColor(255, 255, 255),
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = alignment
            };
        }
    }
}
