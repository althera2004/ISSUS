// --------------------------------
// <copyright file="PrintEquipmentData.aspx.cs" company="Sbrinna">
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

public partial class ExportPrintEquipmentData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(long equipmentId, int companyId, long applicationUserId)
    {
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var equipment = Equipment.ById(equipmentId, companyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Equipment"],
            equipment.Description,
            DateTime.Now);

        var pdfDoc = new iTS.Document(iTS.PageSize.LETTER, 50, 50, 80, 50);


        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}Temp\{1}", path, fileName),
               FileMode.Create));

        var company = new Company(equipment.CompanyId);

        var pageEventHandler = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}: {1}", dictionary["Common_CreatedBy"], user.UserName),
            CompanyId = equipment.CompanyId,
            CompanyName = company.Name,
            Title = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Equipment"], equipment.Description)
        };

        writer.PageEvent = pageEventHandler;

        // Font styles
        var headerFont = new iTS.Font(iTSpdf.BaseFont.CreateFont(iTSpdf.BaseFont.HELVETICA, iTSpdf.BaseFont.CP1252, false), 22, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        var labelFont = new iTS.Font(iTSpdf.BaseFont.CreateFont(iTSpdf.BaseFont.HELVETICA, iTSpdf.BaseFont.CP1252, false), 14, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        var dataFont = new iTS.Font(iTSpdf.BaseFont.CreateFont(iTSpdf.BaseFont.HELVETICA, iTSpdf.BaseFont.CP1252, false), 14, iTS.Font.BOLD, iTS.BaseColor.BLACK);

        pdfDoc.Open();

        // Main data
        pdfDoc.Add(new Phrase(dictionary["Item_Equipment_Tab_Basic"], headerFont));
        var mainDataTable = new iTSpdf.PdfPTable(4)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        mainDataTable.SetWidths(new float[] { 15f, 30f, 15f, 30f });

        var mainC1 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_Code_Label"], labelFont));
        var mainC2 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_Description_Label"], labelFont));
        var mainC3 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_TradeMark_Label"], labelFont));
        var mainC4 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_Model_Label"], labelFont));

        var mainD1 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Code, dataFont));
        var mainD2 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Description, dataFont));
        var mainD3 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Trademark, dataFont));
        var mainD4 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Model, dataFont));

        mainC1.Border = 0; mainD1.Border = 0;
        mainC2.Border = 0; mainD2.Border = 0;
        mainC3.Border = 0; mainD3.Border = 0;
        mainC4.Border = 0; mainD4.Border = 0;

        mainDataTable.AddCell(mainC1); mainDataTable.AddCell(mainD1);
        mainDataTable.AddCell(mainC2); mainDataTable.AddCell(mainD2);
        mainDataTable.AddCell(mainC3); mainDataTable.AddCell(mainD3);
        mainDataTable.AddCell(mainC4); mainDataTable.AddCell(mainD4);

        pdfDoc.Add(mainDataTable);

        pdfDoc.CloseDocument();
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}Temp/{1}", ConfigurationManager.AppSettings["siteUrl"], fileName));
        return res;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        long equipmentId = Convert.ToInt64(Request.QueryString["id"]);
        int companyId = Convert.ToInt32(Request.QueryString["companyId"]);
        var company = new Company(companyId);
        var res = ActionResult.NoAction;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var equipment = Equipment.ById(equipmentId, user.CompanyId);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }



        string formatedDescription = equipment.Description.Replace("?", string.Empty);
        formatedDescription = formatedDescription.Replace("#", string.Empty);
        formatedDescription = formatedDescription.Replace("/", string.Empty);
        formatedDescription = formatedDescription.Replace("\\", string.Empty);
        formatedDescription = formatedDescription.Replace(":", string.Empty);
        formatedDescription = formatedDescription.Replace(";", string.Empty);
        formatedDescription = formatedDescription.Replace(".", string.Empty);

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_Data_{2:yyyyMMddhhmmss}.pdf",
            dictionary["Item_Equipment"],
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
            CompanyId = equipment.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Equipment"]
        };

        writer.PageEvent = pageEventHandler;
        document.Open();

        #region Dades bàsiques
        // Ficha pincipal
        var table = new PdfPTable(3)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };

        table.SetWidths(new float[] { 20f, 50f, 30f });
        
        table.AddCell(new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", equipment.Code, equipment.Description), descriptionFont))
        {
            Colspan = 3,
            Border = Rectangle.NO_BORDER,
            PaddingTop = 20f,
            PaddingBottom = 20f,
            HorizontalAlignment = Element.ALIGN_CENTER
        });

        table.AddCell(TitleCell(dictionary["Item_Equipment_Tab_Basic"], 3));
        table.AddCell(TitleLabel(dictionary["Item_Equipment_Field_TradeMark_Label"]));
        table.AddCell(TitleData(equipment.Trademark));

        if (!string.IsNullOrEmpty(equipment.Image))
        {
            string equipmentImagePath = pathFonts + "images\\equipments\\" + equipment.Id + ".jpg";
            if (File.Exists(equipmentImagePath))
            {
                var myImage = iTextSharp.text.Image.GetInstance(equipmentImagePath);
                var cellPhoto = new PdfPCell();
                cellPhoto.AddElement(myImage);
                cellPhoto.Rowspan = 7;
                cellPhoto.Border = Rectangle.NO_BORDER;
                cellPhoto.HorizontalAlignment = Element.ALIGN_CENTER;
                cellPhoto.VerticalAlignment = Element.ALIGN_TOP;
                table.AddCell(cellPhoto);
            }
            else
            {
                var cellPhoto = new PdfPCell(new Phrase("no image"))
                {
                    Rowspan = 7,
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_TOP
                };
                table.AddCell(cellPhoto);
            }
        }
        else
        {
            var cellPhoto = new PdfPCell(new Phrase("no image"))
            {
                Rowspan = 7,
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP
            };
            table.AddCell(cellPhoto);
        }


        table.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Model_Label"]));
        table.AddCell(TitleData(equipment.Model));

        table.AddCell(TitleLabel(dictionary["Item_Equipment_Field_SerialNumber_Label"]));
        table.AddCell(TitleData(equipment.SerialNumber));

        table.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Location_Label"]));
        table.AddCell(TitleData(equipment.Location));

        table.AddCell(TitleLabel(dictionary["Item_Equipment_Field_ScaleDivision_Label"]));
        table.AddCell(TitleData(equipment.ScaleDivisionValue.Value + " " + equipment.MeasureUnit.Description));

        table.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Responsible_Label"]));
        table.AddCell(TitleData(equipment.Responsible.FullName));

        table.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Notes_Label"]));
        table.AddCell(TitleData(equipment.Observations));

        document.Add(table);
        #endregion

        #region Configuració interna
        if (equipment.InternalCalibration.Id > 0)
        {
            var tableConfigurationInt = new PdfPTable(4)
            {
                SpacingAfter = 30f,
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };

            tableConfigurationInt.SetWidths(new float[] { 25f, 30f, 25f, 40f });
            tableConfigurationInt.AddCell(this.TitleCell(dictionary["Item_EquipmentCalibration_Label_Internal"]));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Operation"]));
            tableConfigurationInt.AddCell(this.TitleData(equipment.InternalCalibration.Description, 3));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Periodicity"]));
            tableConfigurationInt.AddCell(this.TitleData(equipment.InternalCalibration.Periodicity + " " + dictionary["Common_Days"], 3));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Uncertainty"]));
            tableConfigurationInt.AddCell(this.TitleData(equipment.InternalCalibration.Uncertainty.ToString()));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Range"]));
            tableConfigurationInt.AddCell(this.TitleData(equipment.InternalCalibration.Range));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Pattern"]));
            tableConfigurationInt.AddCell(this.TitleData(equipment.InternalCalibration.Pattern));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Cost"]));
            tableConfigurationInt.AddCell(this.TitleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.InternalCalibration.Cost)));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Responsible"]));
            tableConfigurationInt.AddCell(this.TitleData(equipment.InternalCalibration.Responsible.FullName, 3));
            tableConfigurationInt.AddCell(this.TitleLabel(dictionary["Item_Equipment_Field_Calibration_Notes"]));
            tableConfigurationInt.AddCell(this.TitleData(equipment.InternalCalibration.Notes, 3));
            document.Add(tableConfigurationInt);
        }
        #endregion

        #region Configuració externa
        if (equipment.ExternalCalibration.Id > 0)
        {
            var tableConfigurationExt = new PdfPTable(4)
            {
                SpacingAfter = 30f,
                KeepTogether = true,
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };

            tableConfigurationExt.SetWidths(new float[] { 25f, 30f, 25f, 40f });
            tableConfigurationExt.AddCell(TitleCell(dictionary["Item_EquipmentCalibration_Label_External"]));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Operation"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Description, 3));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Periodicity"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Periodicity + " " + dictionary["Common_Days"], 3));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Uncertainty"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Uncertainty.ToString()));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Range"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Range));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Pattern"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Pattern));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Cost"]));
            tableConfigurationExt.AddCell(TitleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.ExternalCalibration.Cost)));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Responsible"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Responsible.FullName, 3));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Provider"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Provider.Description, 3));
            tableConfigurationExt.AddCell(TitleLabel(dictionary["Item_Equipment_Field_Calibration_Notes"]));
            tableConfigurationExt.AddCell(TitleData(equipment.ExternalCalibration.Notes, 3));
            document.Add(tableConfigurationExt);
        }
        #endregion

        #region Verificació interna
        if (equipment.InternalVerification.Id > 0)
        {
            var tableVerificationInt = new PdfPTable(4)
            {
                SpacingAfter = 30f,
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };

            tableVerificationInt.SetWidths(new float[] { 25f, 30f, 25f, 40f });
            tableVerificationInt.AddCell(this.TitleCell(dictionary["Item_EquipmentVerification_Label_Internal"]));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Operation"]));
            tableVerificationInt.AddCell(this.TitleData(equipment.InternalVerification.Description, 3));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Periodicity"]));
            tableVerificationInt.AddCell(this.TitleData(equipment.InternalVerification.Periodicity + " " + dictionary["Common_Days"], 3));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Uncertainty"]));
            tableVerificationInt.AddCell(this.TitleData(equipment.InternalVerification.Uncertainty.ToString()));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Range"]));
            tableVerificationInt.AddCell(this.TitleData(equipment.InternalVerification.Range));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Pattern"]));
            tableVerificationInt.AddCell(this.TitleData(equipment.InternalVerification.Pattern));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Cost"]));
            tableVerificationInt.AddCell(this.TitleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.InternalVerification.Cost)));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Responsible"]));
            tableVerificationInt.AddCell(this.TitleData(equipment.InternalVerification.Responsible.FullName, 3));
            tableVerificationInt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Notes"]));
            tableVerificationInt.AddCell(this.TitleData(equipment.InternalVerification.Notes, 3));
            document.Add(tableVerificationInt);
        }
        #endregion

        #region Verificació externa
        if (equipment.ExternalVerification.Id > 0)
        {
            var tableVerificationExt = new PdfPTable(4)
            {
                SpacingAfter = 30f,
                KeepTogether = true,
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };

            tableVerificationExt.SetWidths(new float[] { 25f, 30f, 25f, 40f });
            tableVerificationExt.AddCell(this.TitleCell(dictionary["Item_EquipmentVerification_Label_External"]));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Operation"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Description, 3));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Periodicity"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Periodicity + " " + dictionary["Common_Days"], 3));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Uncertainty"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Uncertainty.ToString()));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Range"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Range));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Pattern"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Pattern));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Cost"]));
            tableVerificationExt.AddCell(this.TitleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.ExternalVerification.Cost)));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Responsible"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Responsible.FullName, 3));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Provider"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Provider.Description, 3));
            tableVerificationExt.AddCell(this.TitleLabel(dictionary["Item_EquipmentVerification_Field_Notes"]));
            tableVerificationExt.AddCell(this.TitleData(equipment.ExternalVerification.Notes, 3));
            document.Add(tableVerificationExt);
        }
        #endregion

        #region Mantenimientos
        if (equipment.IsMaintenance)
        {
            var borderNone = iTS.Rectangle.NO_BORDER;
            var borderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;
            var backgroundColor = new iTS.BaseColor(225, 225, 225);
            var rowPair = new iTS.BaseColor(255, 255, 255);
            var rowEven = new iTS.BaseColor(240, 240, 240);
            var headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

            document.SetPageSize(PageSize.A4.Rotate());
            document.NewPage();

            var tableMaintenance = new iTSpdf.PdfPTable(5)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 1,
                SpacingBefore = 20f
            };

            tableMaintenance.SetWidths(new float[] { 90f, 40f, 30f, 60f, 20f });

            var cellMaintenanceDescription = new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", equipment.Code, equipment.Description), descriptionFont))
            {
                Colspan = 5,
                Border = Rectangle.NO_BORDER,
                PaddingTop = 20f,
                PaddingBottom = 20f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            tableMaintenance.AddCell(cellMaintenanceDescription);

            var valueFont = new Font(this.headerFont, 11, Font.BOLD, BaseColor.BLACK);
            tableMaintenance.AddCell(new PdfPCell(new Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Plurar"].ToUpperInvariant(), valueFont))
            {
                Colspan = 5,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8,
                PaddingTop = 6,
                Border = Rectangle.NO_BORDER
            });

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Operation"].ToUpperInvariant(), headerFontFinal))
            {
                Border = borderAll,
                BackgroundColor = backgroundColor,
                HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                Padding = 8
            });

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Type"].ToUpperInvariant(), headerFontFinal))
            {
                Border = borderAll,
                BackgroundColor = backgroundColor,
                HorizontalAlignment = iTS.Element.ALIGN_CENTER,
                Padding = 8
            });

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Periodicity_PDF"].ToUpperInvariant(), headerFontFinal))
            {
                PaddingBottom = 6f,
                Border = borderAll,
                BackgroundColor = backgroundColor,
                HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                Padding = 8
            });

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Accesories"].ToUpperInvariant(), headerFontFinal))
            {
                Border = borderAll,
                BackgroundColor = backgroundColor,
                HorizontalAlignment = iTS.Element.ALIGN_LEFT,
                Padding = 8
            });

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Cost"].ToUpperInvariant(), headerFontFinal))
            {
                Border = borderAll,
                BackgroundColor = backgroundColor,
                HorizontalAlignment = iTS.Element.ALIGN_RIGHT,
                Padding = 8
            });

            int borderFirst = iTS.Rectangle.TOP_BORDER;
            int cont = 0;
            var data = Equipment.GetList(companyId);
            bool pair = false;
            var times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
            var timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
            decimal cost = 0;
            foreach (EquipmentMaintenanceDefinition maintenance in EquipmentMaintenanceDefinition.GetByCompany(equipment.Id, equipment.CompanyId))
            {
                int border = 0;
                var lineBackground = pair ? rowEven : rowPair;

                tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(maintenance.Description, times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f
                });

                string typeText = dictionary["Common_Internal"];
                if (maintenance.MaintenanceType == 1)
                {
                    typeText = dictionary["Common_External"];
                }

                tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6f,
                    PaddingTop = 4f
                });

                tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(maintenance.Periodicity + " " + dictionary["Common_Days"], times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 6f,
                    PaddingTop = 4f
                });

                tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(maintenance.Accessories, times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    Padding = 6f,
                    PaddingTop = 4f
                });

                tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", maintenance.Cost), times))
                {
                    Border = border,
                    BackgroundColor = lineBackground,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 6f,
                    PaddingTop = 4f
                });

                cont++;
                if (maintenance.Cost.HasValue)
                {
                    cost += maintenance.Cost.Value;
                }
            }

            // TotalRow
            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant() + ":", timesBold))
            {
                Border = borderFirst,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 4,
                Padding = 8f
            });

            tableMaintenance.AddCell(new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", cost), timesBold))
            {
                Border = borderFirst,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 1,
                Padding = 8f
            });

            document.Add(tableMaintenance);
        }
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
}