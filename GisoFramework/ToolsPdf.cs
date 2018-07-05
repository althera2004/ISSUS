
// --------------------------------
// <copyright file="ToolsPdf.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Globalization;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class ToolsPdf
    {
        public const float PaddingTableCell = 8;
        public const float PaddingTopTableCell = 6;
        public const float PaddingTopCriteriaCell = 4;
        public const int BorderAll = Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
        public const int BorderNone = Rectangle.NO_BORDER;
        public const int BorderBottom = Rectangle.BOTTOM_BORDER;

        public static readonly BaseColor SummaryBackgroundColor = BaseColor.LIGHT_GRAY;
        public static readonly BaseColor LineBackgroundColor = BaseColor.WHITE;
        public static readonly BaseColor HeaderBackgroundColor = BaseColor.LIGHT_GRAY;

        public static string FontPath
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", HttpContext.Current.Request.PhysicalApplicationPath);
            }
        }

        public static readonly BaseFont HeaderFont = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        public static readonly BaseFont Arial = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        public static readonly BaseFont AwesomeFont = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        public static readonly BaseFont DataFont = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.CP1250, BaseFont.EMBEDDED);

        public struct LayoutFonts
        {
            public static readonly Font TimesBold = new Font(ToolsPdf.Arial, 8, Font.BOLD, BaseColor.BLACK);
            public static readonly Font Times = new Font(DataFont, 10, Font.NORMAL, BaseColor.BLACK);
            public static readonly Font TitleFont = new Font(Arial, 18, Font.BOLD, BaseColor.BLACK);
        }

        public static PdfPCell CriteriaCellLabel(string label)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0} :", finalLabel), LayoutFonts.TimesBold))
            {
                Border = ToolsPdf.BorderNone,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = ToolsPdf.PaddingTopTableCell,
                PaddingTop = ToolsPdf.PaddingTopCriteriaCell
            };
        }

        public static PdfPCell CriteriaCellData(string label)
        {
            return CriteriaCellData(label, 1);
        }

        public static PdfPCell CriteriaCellData(string label, int span)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            var cell = new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0} :", finalLabel), LayoutFonts.Times))
            {
                Border = ToolsPdf.BorderNone,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = ToolsPdf.PaddingTopTableCell,
                PaddingTop = ToolsPdf.PaddingTopCriteriaCell
            };

            if (span > 1)
            {
                cell.Colspan = span;
            }

            return cell;
        }

        public static PdfPCell HeaderCell(string label)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(finalLabel.ToUpperInvariant(), LayoutFonts.Times))
            {
                Border = BorderAll,
                BackgroundColor = HeaderBackgroundColor,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = PaddingTableCell,
                PaddingTop = PaddingTopTableCell
            };
        }

        public static PdfPCell CellTable(string value)
        {
            return CellTable(value, LayoutFonts.Times);
        }

        public static PdfPCell CellTable(string value, Font font)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, font))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            };
        }

        public static PdfPCell DataCellRight(string value)
        {
            return DataCell(value, LayoutFonts.Times, Rectangle.ALIGN_RIGHT);
        }

        public static PdfPCell DataCellRight(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_RIGHT);
        }

        public static PdfPCell DataCellRight(int value)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), LayoutFonts.Times, Rectangle.ALIGN_RIGHT);
        }

        public static PdfPCell DataCellRight(int value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_RIGHT);
        }

        public static PdfPCell DataCellRight(long value)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), LayoutFonts.Times, Rectangle.ALIGN_RIGHT);
        }

        public static PdfPCell DataCellRight(long value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_RIGHT);
        }

        public static PdfPCell DataCell(int value)
        {
            return DataCell(value.ToString(), LayoutFonts.Times, Rectangle.ALIGN_LEFT);
        }

        public static PdfPCell DataCell(long value)
        {
            return DataCell(value.ToString(), LayoutFonts.Times, Rectangle.ALIGN_LEFT);
        }

        public static PdfPCell DataCell(string value)
        {
            return DataCell(value, LayoutFonts.Times, Rectangle.ALIGN_LEFT);
        }

        public static PdfPCell DataCell(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_LEFT);
        }

        public static PdfPCell DataCell(long value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_LEFT);
        }

        public static PdfPCell DataCellMoney(decimal? value)
        {
            string valueText = string.Empty;
            if (value.HasValue)
            {
                valueText = Tools.PdfMoneyFormat(value.Value);
            }

            return DataCellRight(valueText, LayoutFonts.Times);
        }

        public static PdfPCell DataCellMoney(decimal? value, Font font)
        {
            string valueText = string.Empty;
            if (value.HasValue)
            {
                valueText = Tools.PdfMoneyFormat(value.Value);
            }

            return DataCellRight(valueText, font);
        }

        public static PdfPCell DataCellMoney(decimal value, Font font)
        {
            string valueText = Tools.PdfMoneyFormat(value);
            return DataCellRight(valueText, font);
        }

        public static PdfPCell DataCell(DateTime value)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), LayoutFonts.Times);
        }

        public static PdfPCell DataCell(DateTime value, Font font)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font);
        }

        public static PdfPCell DataCell(DateTime value, Font font, int alignment)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        public static PdfPCell DataCellCenter(string value)
        {
            return DataCell(value, LayoutFonts.Times, Rectangle.ALIGN_CENTER);
        }

        public static PdfPCell DataCellCenter(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_CENTER);
        }

        public static PdfPCell DataCellCenter(DateTime? value)
        {
            if (value == null)
            {
                return DataCell(string.Empty, LayoutFonts.Times, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), LayoutFonts.Times, Rectangle.ALIGN_CENTER);
        }

        public static PdfPCell DataCellCenter(DateTime? value, Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, Rectangle.ALIGN_CENTER);
        }

        public static PdfPCell DataCell(DateTime? value, Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font);
        }

        public static PdfPCell DataCell(DateTime? value, Font font, int alignment)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        public static PdfPCell DataCell(string value, Font font, int alignment)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, font))
            {
                Border = 0,
                BackgroundColor = BaseColor.WHITE,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = alignment
            };
        }
    }
}