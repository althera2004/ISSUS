//-----------------------------------------------------------------------
// <copyright file="Agreement.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text.pdf;
using iTS = iTextSharp.text;

/// <summary>Shows agreement document</summary>
public partial class Agreement : Page
{
    /// <summary>Gets application user of session</summary>
    public ApplicationUser ApplicationUser { get; private set; }

    /// <summary>Gets company of session</summary>
    public Company Company { get; private set; }

    /// <summary>Gets land page</summary>
    public string HomePage
    {
        get
        {
            string landPage = "/DashBoard.aspx";
            this.Session["home"] = landPage;
            return landPage;
        }
    }

    /// <summary>Gets random image of background</summary>
    public string BK
    {
        get
        {
            string path = this.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}WelcomeBackgrounds\", path);
            var files = Directory.GetFiles(path);
            int index = new Random().Next(0, files.Count() - 1);
            string res = Path.GetFileName(files[index]);
            this.Session["BK"] = res;
            return res;
        }
    }

    /// <summary>Gets language to show in page</summary>
    public string LanguageBrowser
    {
        get
        {
            return this.Company.Language;
        }
    }

    /// <summary>Creates agreement document</summary>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="language">Language of document</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult CreateDocument(int companyId, int userId, string language)
    {
        var res = ActionResult.NoAction;

        var fileNameNew = string.Empty;

        try
        {
            // Se preparan los objetos para el PDF
            var fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
            var baseFont = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            fileNameNew = string.Format(CultureInfo.InvariantCulture, @"{0}Agreement\{1}_{2}.pdf", path, "Agreement", companyId);

            // Se genera el path completo de la plantilla del idioma en concreto
            var templatepath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Contrato_ISSUS_1_{1}.pdf", path, language);

            // Si no existiera la plantilla se genera el path completo de la plantilla sin traducir
            if (!File.Exists(templatepath))
            {
                templatepath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Contrato_ISSUS_1_CA.pdf", path);
            }

            // ---------------------------------------
            using (var existingFileStream = new FileStream(templatepath, FileMode.Open))
            {
                using (var newFileStream = new FileStream(fileNameNew, FileMode.Create))
                {
                    var pdfReader = new PdfReader(existingFileStream);
                    var stamper = new PdfStamper(pdfReader, newFileStream);
                    var form = stamper.AcroFields;
                    var fieldKeys = form.Fields.Keys;

                    Dictionary<string, string> info = pdfReader.Info;
                    info["Title"] = "Agreement";
                    info["Subject"] = "Agreement";
                    info["Keywords"] = string.Empty;
                    info["Creator"] = "OpenFramework";
                    info["Author"] = "ISSUS";
                    stamper.MoreInfo = info;

                    // Se recorren todos los campos del pdf y a cada uno se pone el contenido

                    var fechaDocumento = DateTime.Now;
                    string mes = MonthName(fechaDocumento.Month, language);
                    string fechaLarga = string.Format(CultureInfo.InvariantCulture, "{0} de {1} de {2}", fechaDocumento.Day, mes, fechaDocumento.Year);
                    form.SetField("FechaLarga", fechaLarga);
                    form.SetFieldProperty("FechaLarga", "textsize", 11, null);
                    form.SetFieldProperty("FechaLarga", "textfont", baseFont, null);
                    form.RegenerateField("FechaLarga");
                    try
                    {
                        stamper.FormFlattening = true;
                        
                        stamper.Close();
                        pdfReader.Close();
                        res.SetSuccess(fileNameNew);

                        using(var cmd = new SqlCommand("UPDATE Company SET Agreement = 1 WHERE Id = " + companyId.ToString()))
                        {
                            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                            {
                                cmd.Connection = cnn;
                                cmd.CommandType = CommandType.Text;
                                try
                                {
                                    cmd.Connection.Open();
                                    cmd.ExecuteNonQuery();
                                }
                                finally
                                {
                                    if(cmd.Connection.State != ConnectionState.Closed)
                                    {
                                        cmd.Connection.Close();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //ExceptionManager.Trace(ex, "Agreement");
                        res.SetFail(ex);
                    }
                }
            }
            // ---------------------------------------
        }
        catch (Exception ex)
        {
            //ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, @"Agreemen({0}, {1})", companyId, language));
            res.SetFail(ex);
        }

        return res;
    }


    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplicationUser = this.Session["User"] as ApplicationUser;
        this.Company = this.Session["Company"] as Company;
        if (this.ApplicationUser.PrimaryUser)
        {
            this.RenderAgreement();
        }
        else
        {
            Response.Redirect("AgreementNotice.aspx");
        }
    }

    /// <summary>Generates agreements text to show in page</summary>
    private void RenderAgreement()
    {
        string path = this.Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        // Se extrae el lenguage por defecto de la empresa
        string language = this.Company.Language;

        // Se genera el path completo de la plantilla del idioma en concreto
        path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement_{1}.tpl", path, language);

        // Si no existiera la plantilla se genera el path completo de la plantilla sin traducir
        if (!File.Exists(path))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement.tpl", this.Request.PhysicalApplicationPath);
        }

        string textEs = string.Empty;

        path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement_es.tpl", this.Request.PhysicalApplicationPath);
        using (var rdr = new StreamReader(path))
        {
            textEs = rdr.ReadToEnd();
        }

        textEs = textEs.Replace("#COMPANY_NAME#", "<strong>" + this.Company.Name + "</strong>");
        textEs = textEs.Replace("#USER_NAME#", "<strong>" + this.ApplicationUser.UserName + "</strong>");
        textEs = textEs.Replace("#EMAIL#", "<strong>" + this.ApplicationUser.Email + "</strong>");
        textEs = textEs.Replace("\r", "</p>");
        textEs = textEs.Replace("\n", string.Empty);
        textEs = textEs.Replace("#DATE#", Constant.NowText);

        this.LTEs.Text = "<p>" + textEs;

        // Se genera el path completo de la plantilla del idioma en concreto        
        path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement_ca.tpl", this.Request.PhysicalApplicationPath);

        // Si no existiera la plantilla se genera el path completo de la plantilla sin traducir
        if (!File.Exists(path))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement.tpl", this.Request.PhysicalApplicationPath);
        }

        string textCa = string.Empty;
        using (var rdr = new StreamReader(path))
        {
            textCa = rdr.ReadToEnd();
        }

        textCa = textCa.Replace("#COMPANY_NAME#", "<strong>" + this.Company.Name + "</strong>");
        textCa = textCa.Replace("#USER_NAME#", "<strong>" + this.ApplicationUser.UserName + "</strong>");
        textCa = textCa.Replace("#EMAIL#", "<strong>" + this.ApplicationUser.Email + "</strong>");
        textCa = textCa.Replace("\r", "</p>");
        textCa = textCa.Replace("\n", string.Empty);
        textCa = textCa.Replace("#DATE#", Constant.NowText);
        this.LTCa.Text = "<p>" + textCa;
    }

    /// <summary>Gets name of month</summary>
    /// <param name="index">Index of month</param>
    /// <param name="languageCode">Code on laguange to translate</param>
    /// <returns>Name of month</returns>
    private static string MonthName(int index, string languageCode)
    {
        var dictionary = ApplicationDictionary.Load(languageCode);
        switch (index)
        {
            case 1: return dictionary["Common_MonthName_January"];
            case 2: return dictionary["Common_MonthName_February"];
            case 3: return dictionary["Common_MonthName_March"];
            case 4: return dictionary["Common_MonthName_April"];
            case 5: return dictionary["Common_MonthName_May"];
            case 6: return dictionary["Common_MonthName_June"];
            case 7: return dictionary["Common_MonthName_July"];
            case 8: return dictionary["Common_MonthName_August"];
            case 9: return dictionary["Common_MonthName_September"];
            case 10: return dictionary["Common_MonthName_October"];
            case 11: return dictionary["Common_MonthName_November"];
            case 12: return dictionary["Common_MonthName_December"];
            default: return string.Empty;
        }
    }
}