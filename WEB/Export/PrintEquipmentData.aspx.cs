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

public partial class Export_PrintEquipmentData : Page
{
    BaseFont headerFont = null;
    BaseFont arial = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        long equipmentId = Convert.ToInt64(Request.QueryString["id"].ToString());
        int companyId = Convert.ToInt32(Request.QueryString["companyId"].ToString());
        Company company = new Company(companyId);
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Equipment equipment = Equipment.GetById(equipmentId, user.CompanyId);

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

        // FONTS
        string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        this.headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        this.arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        Font descriptionFont = new Font(this.headerFont, 12, Font.BOLD, BaseColor.BLACK);




        iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 40, 40, 65, 55);

        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\DOCS\\" + fileName, FileMode.Create));
        TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}", user.UserName),
            CompanyId = equipment.CompanyId,
            CompanyName = company.Name,
            Title = dictionary["Item_Equipment"]
        };

        writer.PageEvent = PageEventHandler;

        document.Open();
        iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
        iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

        #region Dades bàsiques
        // Ficha pincipal
        PdfPTable table = new PdfPTable(3);
        table.WidthPercentage = 100;
        float[] widths = new float[] { 20f, 50f, 30f };
        table.SetWidths(widths);
        table.HorizontalAlignment = 0;

        var cellDescription = new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", equipment.Code, equipment.Description), descriptionFont));
        cellDescription.Colspan = 3;
        cellDescription.Border = Rectangle.NO_BORDER;
        cellDescription.PaddingTop = 20f;
        cellDescription.PaddingBottom = 20f;
        cellDescription.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(cellDescription);

        table.AddCell(titleCell(dictionary["Item_Equipment_Tab_Basic"], 3));
        table.AddCell(titleLabel(dictionary["Item_Equipment_Field_TradeMark_Label"]));
        table.AddCell(titleData(equipment.Trademark));

        if (!string.IsNullOrEmpty(equipment.Image))
        {
            string equipmentImagePath = pathFonts + "images\\equipments\\" + equipment.Id + ".jpg";

            if (File.Exists(equipmentImagePath))
            {
                iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(equipmentImagePath);
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
                var cellPhoto = new PdfPCell(new Phrase("no image"));
                cellPhoto.Rowspan = 7;
                cellPhoto.Border = Rectangle.NO_BORDER;
                cellPhoto.HorizontalAlignment = Element.ALIGN_CENTER;
                cellPhoto.VerticalAlignment = Element.ALIGN_TOP;
                table.AddCell(cellPhoto);
            }
        }
        else
        {
            var cellPhoto = new PdfPCell(new Phrase("no image"));
            cellPhoto.Rowspan = 7;
            cellPhoto.Border = Rectangle.NO_BORDER;
            cellPhoto.HorizontalAlignment = Element.ALIGN_CENTER;
            cellPhoto.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cellPhoto);
        }


        table.AddCell(titleLabel(dictionary["Item_Equipment_Field_Model_Label"]));
        table.AddCell(titleData(equipment.Model));

        table.AddCell(titleLabel(dictionary["Item_Equipment_Field_SerialNumber_Label"]));
        table.AddCell(titleData(equipment.SerialNumber));

        table.AddCell(titleLabel(dictionary["Item_Equipment_Field_Location_Label"]));
        table.AddCell(titleData(equipment.Location));

        table.AddCell(titleLabel(dictionary["Item_Equipment_Field_ScaleDivision_Label"]));
        table.AddCell(titleData(equipment.ScaleDivisionValue.Value.ToString() + " " + equipment.MeasureUnit.Description));

        table.AddCell(titleLabel(dictionary["Item_Equipment_Field_Responsible_Label"]));
        table.AddCell(titleData(equipment.Responsible.FullName));

        table.AddCell(titleLabel(dictionary["Item_Equipment_Field_Notes_Label"]));
        table.AddCell(titleData(equipment.Observations));

        document.Add(table);
        #endregion

        #region Configuració interna
        if (equipment.InternalCalibration.Id > 0)
        {
            PdfPTable tableConfigurationInt = new PdfPTable(4);
            tableConfigurationInt.SpacingAfter = 30f;
            tableConfigurationInt.WidthPercentage = 100;
            float[] tableConfigurationIntWidth = new float[] { 25f, 30f, 25f, 40f };
            tableConfigurationInt.SetWidths(tableConfigurationIntWidth);
            tableConfigurationInt.HorizontalAlignment = 0;
            tableConfigurationInt.AddCell(titleCell(dictionary["Item_EquipmentCalibration_Label_Internal"]));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Operation"]));
            tableConfigurationInt.AddCell(titleData(equipment.InternalCalibration.Description, 3));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Periodicity"]));
            tableConfigurationInt.AddCell(titleData(equipment.InternalCalibration.Periodicity.ToString() + " " + dictionary["Common_Days"], 3));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Uncertainty"]));
            tableConfigurationInt.AddCell(titleData(equipment.InternalCalibration.Uncertainty.ToString()));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Range"]));
            tableConfigurationInt.AddCell(titleData(equipment.InternalCalibration.Range));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Pattern"]));
            tableConfigurationInt.AddCell(titleData(equipment.InternalCalibration.Pattern));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Cost"]));
            tableConfigurationInt.AddCell(titleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.InternalCalibration.Cost)));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Responsible"]));
            tableConfigurationInt.AddCell(titleData(equipment.InternalCalibration.Responsible.FullName, 3));
            tableConfigurationInt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Notes"]));
            tableConfigurationInt.AddCell(titleData(equipment.InternalCalibration.Notes, 3));
            document.Add(tableConfigurationInt);
        }
        #endregion

        #region Configuració externa
        if (equipment.ExternalCalibration.Id > 0)
        {
            PdfPTable tableConfigurationExt = new PdfPTable(4);
            tableConfigurationExt.SpacingAfter = 30f;
            tableConfigurationExt.KeepTogether = true;
            tableConfigurationExt.WidthPercentage = 100;
            float[] tableConfigurationExtWidth = new float[] { 25f, 30f, 25f, 40f };
            tableConfigurationExt.SetWidths(tableConfigurationExtWidth);
            tableConfigurationExt.HorizontalAlignment = 0;
            tableConfigurationExt.AddCell(titleCell(dictionary["Item_EquipmentCalibration_Label_External"]));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Operation"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Description, 3));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Periodicity"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Periodicity.ToString() + " " + dictionary["Common_Days"], 3));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Uncertainty"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Uncertainty.ToString()));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Range"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Range));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Pattern"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Pattern));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Cost"]));
            tableConfigurationExt.AddCell(titleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.ExternalCalibration.Cost)));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Responsible"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Responsible.FullName, 3));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Provider"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Provider.Description, 3));
            tableConfigurationExt.AddCell(titleLabel(dictionary["Item_Equipment_Field_Calibration_Notes"]));
            tableConfigurationExt.AddCell(titleData(equipment.ExternalCalibration.Notes, 3));
            document.Add(tableConfigurationExt);
        }
        #endregion

        #region Verificació interna
        if (equipment.InternalVerification.Id > 0)
        {
            PdfPTable tableVerificationInt = new PdfPTable(4);
            tableVerificationInt.SpacingAfter = 30f;
            tableVerificationInt.WidthPercentage = 100;
            float[] tableVerificationIntWidth = new float[] { 25f, 30f, 25f, 40f };
            tableVerificationInt.SetWidths(tableVerificationIntWidth);
            tableVerificationInt.HorizontalAlignment = 0;
            tableVerificationInt.AddCell(titleCell(dictionary["Item_EquipmentVerification_Label_Internal"]));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Operation"]));
            tableVerificationInt.AddCell(titleData(equipment.InternalVerification.Description, 3));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Periodicity"]));
            tableVerificationInt.AddCell(titleData(equipment.InternalVerification.Periodicity.ToString() + " " + dictionary["Common_Days"], 3));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Uncertainty"]));
            tableVerificationInt.AddCell(titleData(equipment.InternalVerification.Uncertainty.ToString()));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Range"]));
            tableVerificationInt.AddCell(titleData(equipment.InternalVerification.Range));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Pattern"]));
            tableVerificationInt.AddCell(titleData(equipment.InternalVerification.Pattern));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Cost"]));
            tableVerificationInt.AddCell(titleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.InternalVerification.Cost)));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Responsible"]));
            tableVerificationInt.AddCell(titleData(equipment.InternalVerification.Responsible.FullName, 3));
            tableVerificationInt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Notes"]));
            tableVerificationInt.AddCell(titleData(equipment.InternalVerification.Notes, 3));
            document.Add(tableVerificationInt);
        }
        #endregion

        #region Verificació externa
        if (equipment.ExternalVerification.Id > 0)
        {
            PdfPTable tableVerificationExt = new PdfPTable(4);
            tableVerificationExt.SpacingAfter = 30f;
            tableVerificationExt.KeepTogether = true;
            tableVerificationExt.WidthPercentage = 100;
            float[] tableVerificationExtWidth = new float[] { 25f, 30f, 25f, 40f };
            tableVerificationExt.SetWidths(tableVerificationExtWidth);
            tableVerificationExt.HorizontalAlignment = 0;
            tableVerificationExt.AddCell(titleCell(dictionary["Item_EquipmentVerification_Label_External"]));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Operation"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Description, 3));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Periodicity"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Periodicity.ToString() + " " + dictionary["Common_Days"], 3));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Uncertainty"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Uncertainty.ToString()));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Range"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Range));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Pattern"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Pattern));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Cost"]));
            tableVerificationExt.AddCell(titleData(string.Format(CultureInfo.InvariantCulture, "{0:#0.00} €", equipment.ExternalVerification.Cost)));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Responsible"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Responsible.FullName, 3));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Provider"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Provider.Description, 3));
            tableVerificationExt.AddCell(titleLabel(dictionary["Item_EquipmentVerification_Field_Notes"]));
            tableVerificationExt.AddCell(titleData(equipment.ExternalVerification.Notes, 3));
            document.Add(tableVerificationExt);
        }
        #endregion

        #region Mantenimientos
        if (equipment.IsMaintenance)
        {
            var borderNone = iTS.Rectangle.NO_BORDER;
            var borderAll = iTS.Rectangle.RIGHT_BORDER + iTS.Rectangle.TOP_BORDER + iTS.Rectangle.LEFT_BORDER + iTS.Rectangle.BOTTOM_BORDER;
            var borderTBL = iTS.Rectangle.TOP_BORDER + iTS.Rectangle.BOTTOM_BORDER + iTS.Rectangle.LEFT_BORDER;
            var borderTBR = iTS.Rectangle.TOP_BORDER + iTS.Rectangle.BOTTOM_BORDER + iTS.Rectangle.RIGHT_BORDER;
            iTS.BaseColor backgroundColor = new iTS.BaseColor(225, 225, 225);
            iTS.BaseColor rowPair = new iTS.BaseColor(255, 255, 255);
            iTS.BaseColor rowEven = new iTS.BaseColor(240, 240, 240);
            iTS.Font headerFontFinal = new iTS.Font(headerFont, 9, iTS.Font.NORMAL, iTS.BaseColor.BLACK);

            document.SetPageSize(PageSize.A4.Rotate());
            document.NewPage();

            iTSpdf.PdfPTable tableMaintenance = new iTSpdf.PdfPTable(5);
            tableMaintenance.WidthPercentage = 100;
            float[] tableMaintenanceWidths = new float[] { 90f, 40f, 30f, 60f, 20f };
            tableMaintenance.SetWidths(tableMaintenanceWidths);
            tableMaintenance.HorizontalAlignment = 1;
            tableMaintenance.SpacingBefore = 20f;


            var cellMaintenanceDescription = new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", equipment.Code, equipment.Description), descriptionFont));
            cellMaintenanceDescription.Colspan = 5;
            cellMaintenanceDescription.Border = Rectangle.NO_BORDER;
            cellMaintenanceDescription.PaddingTop = 20f;
            cellMaintenanceDescription.PaddingBottom = 20f;
            cellMaintenanceDescription.HorizontalAlignment = Element.ALIGN_CENTER;
            tableMaintenance.AddCell(cellMaintenanceDescription);

            Font valueFont = new Font(this.headerFont, 11, Font.BOLD, BaseColor.BLACK);
            PdfPCell cellMaintenanceListTitle = new PdfPCell(new Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Plurar"].ToUpperInvariant(), valueFont));
            cellMaintenanceListTitle.Colspan = 5;
            cellMaintenanceListTitle.HorizontalAlignment = Element.ALIGN_LEFT;
            cellMaintenanceListTitle.Padding = 8;
            cellMaintenanceListTitle.PaddingTop = 6;
            cellMaintenanceListTitle.Border = Rectangle.NO_BORDER;
            tableMaintenance.AddCell(cellMaintenanceListTitle);

            iTSpdf.PdfPCell header1 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Operation"].ToUpperInvariant(), headerFontFinal));
            header1.Border = borderAll;
            header1.BackgroundColor = backgroundColor;
            header1.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            header1.Padding = 8;

            iTSpdf.PdfPCell header2 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Type"].ToUpperInvariant(), headerFontFinal));
            header2.Border = borderAll;
            header2.BackgroundColor = backgroundColor;
            header2.HorizontalAlignment = iTS.Element.ALIGN_CENTER;
            header2.Padding = 8;

            iTSpdf.PdfPCell header3 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Periodicity_PDF"].ToUpperInvariant(), headerFontFinal));
            header3.PaddingBottom = 6f;
            header3.Border = borderAll;
            header3.BackgroundColor = backgroundColor;
            header3.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            header3.Padding = 8;

            iTSpdf.PdfPCell header4 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Accesories"].ToUpperInvariant(), headerFontFinal));
            header4.Border = borderAll;
            header4.BackgroundColor = backgroundColor;
            header4.HorizontalAlignment = iTS.Element.ALIGN_LEFT;
            header4.Padding = 8;

            iTSpdf.PdfPCell header5 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_EquipmentMaintenanceDefinition_Header_Cost"].ToUpperInvariant(), headerFontFinal));
            header5.Border = borderAll;
            header5.BackgroundColor = backgroundColor;
            header5.HorizontalAlignment = iTS.Element.ALIGN_RIGHT;
            header5.Padding = 8;

            tableMaintenance.AddCell(header1);
            tableMaintenance.AddCell(header2);
            tableMaintenance.AddCell(header3);
            tableMaintenance.AddCell(header4);
            tableMaintenance.AddCell(header5);

            int borderFirst = iTS.Rectangle.TOP_BORDER;
            int borderLast = iTS.Rectangle.BOTTOM_BORDER;
            int borderMiddle = iTS.Rectangle.NO_BORDER;
            int borderUnique = iTS.Rectangle.TOP_BORDER + iTS.Rectangle.BOTTOM_BORDER;
            int cont = 0;
            ReadOnlyCollection<Equipment> data = Equipment.GetList(companyId);
            bool pair = false;
            iTS.Font times = new iTS.Font(arial, 8, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
            iTS.Font timesBold = new iTS.Font(arial, 8, iTS.Font.BOLD, iTS.BaseColor.BLACK);
            decimal cost = 0;
            foreach (EquipmentMaintenanceDefinition maintenance in EquipmentMaintenanceDefinition.GetByCompany(equipment.Id, equipment.CompanyId))
            {
                int border = 0;
                BaseColor lineBackground = pair ? rowEven : rowPair;
                // pair = !pair;

                iTSpdf.PdfPCell cellDate = new iTSpdf.PdfPCell(new iTS.Phrase(maintenance.Description, times));
                cellDate.Border = border;
                cellDate.BackgroundColor = lineBackground;
                cellDate.Padding = 6f;
                cellDate.PaddingTop = 4f;
                tableMaintenance.AddCell(cellDate);

                string typeText = dictionary["Common_Internal"];
                if (maintenance.MaintenanceType == 1)
                {
                    typeText = dictionary["Common_External"];
                }
                iTSpdf.PdfPCell typeCell = new iTSpdf.PdfPCell(new iTS.Phrase(typeText, times));
                typeCell.Border = border;
                typeCell.BackgroundColor = lineBackground;
                typeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                typeCell.Padding = 6f;
                typeCell.PaddingTop = 4f;
                tableMaintenance.AddCell(typeCell);

                iTSpdf.PdfPCell operationCell = new iTSpdf.PdfPCell(new iTS.Phrase(maintenance.Periodicity.ToString() + " " + dictionary["Common_Days"], times));
                operationCell.Border = border;
                operationCell.BackgroundColor = lineBackground;
                operationCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                operationCell.Padding = 6f;
                operationCell.PaddingTop = 4f;
                tableMaintenance.AddCell(operationCell);

                iTSpdf.PdfPCell responsibleCell = new iTSpdf.PdfPCell(new iTS.Phrase(maintenance.Accessories, times));
                responsibleCell.Border = border;
                responsibleCell.BackgroundColor = lineBackground;
                responsibleCell.Padding = 6f;
                responsibleCell.PaddingTop = 4f;
                tableMaintenance.AddCell(responsibleCell);

                iTSpdf.PdfPCell costCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", maintenance.Cost), times));
                costCell.Border = border;
                costCell.BackgroundColor = lineBackground;
                costCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                costCell.Padding = 6f;
                costCell.PaddingTop = 4f;
                tableMaintenance.AddCell(costCell);

                if (maintenance.Cost.HasValue)
                {
                    cost += maintenance.Cost.Value;
                }
                cont++;
            }

            // TotalRow
            iTSpdf.PdfPCell totalLabelCell = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Common_Total"].ToUpperInvariant()+":", timesBold));
            totalLabelCell.Border = borderFirst;
            totalLabelCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalLabelCell.Colspan = 4;
            totalLabelCell.Padding = 8f;
            tableMaintenance.AddCell(totalLabelCell);

            iTSpdf.PdfPCell totalCell = new iTSpdf.PdfPCell(new iTS.Phrase(string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", cost), timesBold));
            totalCell.Border = borderFirst;
            totalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalCell.Colspan = 1;
            totalCell.Padding = 8f;
            tableMaintenance.AddCell(totalCell);

            document.Add(tableMaintenance);
        }
        #endregion

        document.Close();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=outfile.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(Request.PhysicalApplicationPath + "\\DOCS\\" + fileName);
        Response.Flush();
        Response.Clear();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PDF(long equipmentId, int companyId, long applicationUserId)
    {
        ActionResult res = ActionResult.NoAction;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        Equipment equipment = Equipment.GetById(equipmentId, companyId);

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

        iTS.Document pdfDoc = new iTS.Document(iTS.PageSize.LETTER, 50, 50, 80, 50);


        iTSpdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}", path, fileName),
               FileMode.Create));

        Company company = new Company(equipment.CompanyId);

        TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, companyId),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0}: {1:dd/MM/yyyy}", dictionary["Common_Date"], DateTime.Now),
            CreatedBy = string.Format(CultureInfo.InvariantCulture, "{0}: {1}", dictionary["Common_CreatedBy"], user.UserName),
            CompanyId = equipment.CompanyId,
            CompanyName = company.Name,
            Title = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", dictionary["Item_Equipment"], equipment.Description)
        };

        writer.PageEvent = PageEventHandler;

        // Font styles
        iTS.Font headerFont = new iTS.Font(iTSpdf.BaseFont.CreateFont(iTSpdf.BaseFont.HELVETICA, iTSpdf.BaseFont.CP1252, false), 22, iTS.Font.BOLD, iTS.BaseColor.BLACK);
        iTS.Font labelFont = new iTS.Font(iTSpdf.BaseFont.CreateFont(iTSpdf.BaseFont.HELVETICA, iTSpdf.BaseFont.CP1252, false), 14, iTS.Font.NORMAL, iTS.BaseColor.BLACK);
        iTS.Font dataFont = new iTS.Font(iTSpdf.BaseFont.CreateFont(iTSpdf.BaseFont.HELVETICA, iTSpdf.BaseFont.CP1252, false), 14, iTS.Font.BOLD, iTS.BaseColor.BLACK);


        pdfDoc.Open();

        // Main data
        pdfDoc.Add(new Phrase(dictionary["Item_Equipment_Tab_Basic"], headerFont));
        iTSpdf.PdfPTable mainDataTable = new iTSpdf.PdfPTable(4);

        // actual width of table in points
        mainDataTable.WidthPercentage = 100;
        // fix the absolute width of the table
        // table.LockedWidth = true;

        //relative col widths in proportions - 1/3 and 2/3
        float[] widths = new float[] { 15f, 30f, 15f, 30f };
        mainDataTable.SetWidths(widths);
        mainDataTable.HorizontalAlignment = 0;
        //leave a gap before and after the table
        mainDataTable.SpacingBefore = 20f;
        mainDataTable.SpacingAfter = 30f;

        iTSpdf.PdfPCell mainC1 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_Code_Label"], labelFont));
        iTSpdf.PdfPCell mainC2 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_Description_Label"], labelFont));
        iTSpdf.PdfPCell mainC3 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_TradeMark_Label"], labelFont));
        iTSpdf.PdfPCell mainC4 = new iTSpdf.PdfPCell(new iTS.Phrase(dictionary["Item_Equipment_Field_Model_Label"], labelFont));

        iTSpdf.PdfPCell mainD1 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Code, dataFont));
        iTSpdf.PdfPCell mainD2 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Description, dataFont));
        iTSpdf.PdfPCell mainD3 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Trademark, dataFont));
        iTSpdf.PdfPCell mainD4 = new iTSpdf.PdfPCell(new iTS.Phrase(equipment.Model, dataFont));

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
        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, @"{0}DOCS/{1}", ConfigurationManager.AppSettings["siteUrl"].ToString(), fileName));
        return res;
    }

    private PdfPCell titleCell(string value)
    {
        return titleCell(value, 4);
    }

    private PdfPCell titleCell(string value, int colSpan)
    {
        Font valueFont = new Font(this.headerFont, 11, Font.BOLD, BaseColor.BLACK);
        PdfPCell cell = new PdfPCell(new Phrase(value.ToUpperInvariant(), valueFont));
        cell.Colspan = colSpan;
        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        cell.Padding = 8;
        cell.PaddingTop = 6;
        cell.Border = Rectangle.BOTTOM_BORDER;
        return cell;
    }

    private PdfPCell titleLabel(string value)
    {
        Font valueFont = new Font(this.arial, 10, Font.NORMAL, BaseColor.BLACK);
        PdfPCell cell = new PdfPCell(new Phrase(string.Format(CultureInfo.InvariantCulture, "{0}:", value.ToUpperInvariant()), valueFont));
        cell.Colspan = 1;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.Padding = 8;
        cell.PaddingTop = 6;
        cell.Border = Rectangle.NO_BORDER;
        return cell;
    }

    private PdfPCell titleData(string value)
    {
        return titleData(value, 1);
    }

    private PdfPCell titleData(string value, int colsPan)
    {
        Font valueFont = new Font(this.arial, 10, Font.BOLD, BaseColor.BLACK);
        PdfPCell cell = new PdfPCell(new Phrase(value, valueFont));
        cell.Colspan = colsPan;
        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        cell.Padding = 8;
        cell.PaddingTop = 6;
        cell.Border = Rectangle.NO_BORDER;
        return cell;
    }
}