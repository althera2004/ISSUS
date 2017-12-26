using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;

namespace GisoFramework
{
    public static class ToolsPdf
    {
        public static readonly iTS.BaseColor HeaderBackgroundColor = new iTS.BaseColor(220, 220, 220);
        public const int BorderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;
        public const int BorderNone = iTS.Rectangle.NO_BORDER;

        public static iTSpdf.PdfPCell HeaderCell(string label, iTS.Font font)
        {
            return new iTSpdf.PdfPCell(new iTS.Phrase(label.ToUpperInvariant(), font))
            {
                Border = BorderAll,
                BackgroundColor = HeaderBackgroundColor,
                HorizontalAlignment = iTS.Element.ALIGN_CENTER,
                Padding = 8f,
                PaddingTop = 6f
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

        public static iTSpdf.PdfPCell DataCell(string value, iTS.Font font)
        {
            return DataCell(value, font, iTS.Rectangle.ALIGN_LEFT);
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
            if (!value.HasValue)
            {
                return DataCell(string.Empty, font, iTS.Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, iTS.Rectangle.ALIGN_CENTER);
        }

        public static iTSpdf.PdfPCell DataCell(DateTime? value, iTS.Font font)
        {
            if (!value.HasValue)
            {
                return DataCell(string.Empty, font, iTS.Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font);
        }

        public static iTSpdf.PdfPCell DataCell(DateTime? value, iTS.Font font, int alignment)
        {
            if (!value.HasValue)
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
