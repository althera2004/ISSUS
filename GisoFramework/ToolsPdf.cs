﻿// --------------------------------
// <copyright file="ToolsPdf.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>Tools for pdf generation</summary>
    public static class ToolsPdf
    {
        /// <summary>Padding of cell table</summary>
        public const float PaddingTableCell = 8;

        /// <summary>Padding top of cell table</summary>
        public const float PaddingTopTableCell = 6;

        /// <summary>Padding topo of cell criteria table</summary>
        public const float PaddingTopCriteriaCell = 4;

        /// <summary>Cell with all borders</summary>
        public const int BorderAll = Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;

        /// <summary>Cell without</summary>
        public const int BorderNone = Rectangle.NO_BORDER;

        /// <summary>Cell with only bottom border</summary>
        public const int BorderBottom = Rectangle.BOTTOM_BORDER;

        /// <summary>Cell with only bottom border</summary>
        public const int BorderTop= Rectangle.TOP_BORDER;

        /// <summary>Background color for summary row</summary>
        public static readonly BaseColor SummaryBackgroundColor = BaseColor.LIGHT_GRAY;

        /// <summary>Background color for line row</summary>
        public static readonly BaseColor LineBackgroundColor = BaseColor.WHITE;

        /// <summary>Background color for header row</summary>
        public static readonly BaseColor HeaderBackgroundColor = BaseColor.LIGHT_GRAY;

        /// <summary>Base font for headers</summary>
        public static readonly BaseFont HeaderFont = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        
        /// <summary>Base font for type Arial</summary>
        public static readonly BaseFont Arial = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        /// <summary>Base font for type Awesome (icons)</summary>
        public static readonly BaseFont AwesomeFont = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        /// <summary>Base font for data</summary>
        public static readonly BaseFont DataFont = BaseFont.CreateFont(ToolsPdf.FontPath, BaseFont.CP1250, BaseFont.EMBEDDED);        
        
        /// <summary>Gets the path to load ttf files</summary>
        public static string FontPath
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", HttpContext.Current.Request.PhysicalApplicationPath);
            }
        }

        public static string NormalizeFileName(string fileName)
        {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
            }

            return fileName.Replace('#', '_').Replace('%', '_').Replace('´', '_').Replace(@"\", "/");
        }

        public static void AddTableTitle(PdfPTable table, string title)
        {
            table.AddCell(new PdfPCell(new Phrase(title))
            {
                Colspan = table.NumberOfColumns,
                Border = Rectangle.NO_BORDER,
                PaddingTop = 20f,
                PaddingBottom = 20f,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
        }

        public static void AddDataLabel(PdfPTable table, string label, string value)
        {
            AddDataLabel(table, label, value, 1);
        }

        public static void AddDataLabel(PdfPTable table, string label, DateTime value)
        {
            AddDataLabel(table, label, string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), 1);
        }

        public static void AddDataLabel(PdfPTable table, string label, int value)
        {
            AddDataLabel(table, label, string.Format(CultureInfo.InvariantCulture, "{0}", value), 1);
        }

        public static void AddDataLabel(PdfPTable table, string label, string value, int colsSpan)
        {
            table.AddCell(new PdfPCell(new Phrase(label + ":", new Font(ToolsPdf.Arial, 10, Font.BOLD, BaseColor.BLACK)))
            {
                Border = ToolsPdf.BorderNone
            });

            table.AddCell(new PdfPCell(new Phrase(value, new Font(ToolsPdf.Arial, 10, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = ToolsPdf.BorderNone,
                Colspan = colsSpan
            });
        }

        public static void AddCriteria(PdfPTable table, string labelString, string value)
        {
            table.AddCell(new PdfPCell(new Phrase(labelString + " :", ToolsPdf.LayoutFonts.TimesBold))
            {
                Border = ToolsPdf.BorderNone,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 6f,
                PaddingTop = 4f
            });

            table.AddCell(new PdfPCell(new Phrase(value, ToolsPdf.LayoutFonts.Times))
            {
                Border = ToolsPdf.BorderNone,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 6f,
                PaddingTop = 4f
            });
        }
        
        /// <summary>Creates a label cell for criteria table</summary>
        /// <param name="label">Label to show</param>
        /// <returns>Cel for list header</returns>
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

        /// <summary>Creates a label cell for criteria table</summary>
        /// <param name="label">Label to show</param>
        /// <returns>Cel for list header</returns>
        public static PdfPCell CriteriaCellData(string label)
        {
            return CriteriaCellData(label, 1);
        }

        /// <summary>Creates a data cell for criteria table</summary>
        /// <param name="label">Label to show</param>
        /// <param name="span">Columns span</param>
        /// <returns>Cel for list header</returns>
        public static PdfPCell CriteriaCellData(string label, int span)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            var cell = new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0}", finalLabel), LayoutFonts.Times))
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

        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <returns>Cel for list header</returns>
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

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTable(string value)
        {
            return CellTable(value, LayoutFonts.Times);
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
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

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellRight(string value)
        {
            return DataCell(value, LayoutFonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellRight(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Integer value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellRight(int value)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), LayoutFonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Integer value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellRight(int value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Long value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellRight(long value)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), LayoutFonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Long value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellRight(long value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Integer value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(int value)
        {
            return DataCell(value.ToString(), LayoutFonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Integer value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(decimal value)
        {
            return DataCell(value.ToString(), LayoutFonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Long value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(long value)
        {
            return DataCell(value.ToString(), LayoutFonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Text value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(string value)
        {
            return DataCell(value, LayoutFonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Text value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Long value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(long value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Long value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(DateTime? value)
        {
            if(value == null)
            {
                return DataCell(string.Empty, LayoutFonts.Times, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture,"{0:dd/MM/yyyy}", value.Value), LayoutFonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Decimal value to show in money format</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellMoney(decimal? value)
        {
            string valueText = string.Empty;
            if (value.HasValue)
            {
                valueText = Tools.PdfMoneyFormat(value.Value);
            }

            return DataCellRight(valueText, LayoutFonts.Times);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Decimal value to show in money format</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellMoney(decimal? value, Font font)
        {
            string valueText = string.Empty;
            if (value.HasValue)
            {
                valueText = Tools.PdfMoneyFormat(value.Value);
            }

            return DataCellRight(valueText, font);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Decimal value to show </param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellMoney(decimal value, Font font)
        {
            string valueText = Tools.PdfMoneyFormat(value);
            return DataCellRight(valueText, font);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">DateTime value to show </param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(DateTime value)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), LayoutFonts.Times, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">DateTime value to show </param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(DateTime value, Font font)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content with alignment</summary>
        /// <param name="value">DateTime value to show </param>
        /// <param name="font">Font for content</param>
        /// <param name="alignment">Content aligment</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(DateTime value, Font font, int alignment)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">Text value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellCenter(string value)
        {
            return DataCell(value, LayoutFonts.Times, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">Text value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellCenter(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">DateTime value to show</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellCenter(DateTime? value)
        {
            if (value == null)
            {
                return DataCell(string.Empty, LayoutFonts.Times, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), LayoutFonts.Times, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">DateTime value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCellCenter(DateTime? value, Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">DateTime value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(DateTime? value, Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font);
        }

        /// <summary>Creates a cell table with content aligment</summary>
        /// <param name="value">DateTime value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="alignment">Content alignment</param>
        /// <returns>Cell table</returns>
        public static PdfPCell DataCell(DateTime? value, Font font, int alignment)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Text value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="alignment">Content alignment</param>
        /// <returns>Cell table</returns>
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

        /// <summary>Default fonts definition</summary>
        public struct LayoutFonts
        {
            /// <summary>Font in bold type</summary>
            public static readonly Font TimesBold = new Font(ToolsPdf.Arial, 8, Font.BOLD, BaseColor.BLACK);

            /// <summary>Font in normal type</summary>
            public static readonly Font Times = new Font(DataFont, 8, Font.NORMAL, BaseColor.BLACK);

            /// <summary>Font for titles</summary>
            public static readonly Font TitleFont = new Font(Arial, 18, Font.BOLD, BaseColor.BLACK);
        }
    }
}