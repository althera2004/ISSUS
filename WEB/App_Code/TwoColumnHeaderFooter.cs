// --------------------------------
// <copyright file="TwoColumnHeaderFooter.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace PDF_Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class TwoColumnHeaderFooter : PdfPageEventHelper
    {
        /// <summary>This is the contentbyte object of the writer</summary> 
        PdfContentByte cb;

        /// <summary>This is the BaseFont we are going to use for the header / footer</summary> 
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        #region Properties
        public string Title { get; set; }
        public List<string> Titles;
        public string HeaderLeft { get; set; }
        public string HeaderRight { get; set; }
        public Font HeaderFont { get; set; }
        public Font FooterFont { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Date { get; set; }
        public string CreatedBy { get; set; }
        PdfTemplate template, footerTemplate;
        public string IssusLogo { get; set; }
        public string CompanyLogo { get; set; }
        #endregion
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                //PrintTime = DateTime.Now;
                //this.bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //cb = writer.DirectContent;
                //template = cb.CreateTemplate(50, 50);
                // ------------ FONTS 
                var path = HttpContext.Current.Request.PhysicalApplicationPath;
                var pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
                if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
                }

                PrintTime = DateTime.Now;
                //this.bf = BaseFont.CreateFont(BaseFont.Ar, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                this.bf = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;

            var title = this.Title;

            if(this.Titles != null && this.Titles.Count > 1 && writer.PageNumber > 1)
            {
                title = this.Titles[writer.PageNumber - 1];
            }

            // Lineas
            cb.SetLineWidth(0.5f);
            cb.MoveTo(40f, document.PageSize.Height -30f);
            cb.LineTo(document.PageSize.Width - 40f, document.PageSize.Height - 30f);
            cb.Stroke();
            cb.MoveTo(40f, document.PageSize.Height -60f );
            cb.LineTo(document.PageSize.Width - 40f, document.PageSize.Height - 60f);
            cb.Stroke();

            // Titulo
            cb.BeginText();
            cb.SetFontAndSize(this.bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
                title,
                pageSize.GetRight(document.PageSize.Width / 2),
                pageSize.GetTop(50), 0);
            cb.EndText();

            // Empresa
            cb.BeginText();
            cb.SetFontAndSize(this.bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT,
                this.CompanyName,
                pageSize.GetLeft(40),
                pageSize.GetTop(48), 0);
            cb.EndText();

            // Fecha
            cb.BeginText();
            cb.SetFontAndSize(this.bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
                this.Date,
                pageSize.GetRight(40),
                pageSize.GetTop(42), 0);
            cb.EndText();

            // Generado
            cb.BeginText();
            cb.SetFontAndSize(this.bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
                this.CreatedBy,
                pageSize.GetRight(40),
                pageSize.GetTop(53), 0);
            cb.EndText();
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            var text = pageN + " de ";
            float len = bf.GetWidthPoint(text, 8);
            var pageSize = document.PageSize;
            
            // Numero de pagina
            // Add a unique (empty) template for each page here
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix((pageSize.Width / 2) - len, pageSize.GetBottom(30));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, (pageSize.Width / 2), pageSize.GetBottom(30));

            var logoIssus = Image.GetInstance(this.IssusLogo);
            logoIssus.ScalePercent(20f);
            logoIssus.SetAbsolutePosition(40f, 24f);
            document.Add(logoIssus);            

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
                "issus.scrambotika.com",
                pageSize.GetRight(40),
                pageSize.GetBottom(30), 0);
            cb.EndText();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();
        }
    }
}