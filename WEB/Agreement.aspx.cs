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
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var user = HttpContext.Current.Session["User"] as ApplicationUser;
        var company = HttpContext.Current.Session["Company"] as Company;

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(CultureInfo.InvariantCulture, @"Agreement\{0}_{1}.pdf", "Agreement", company.Id);
        if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        // Se genera el path completo de la plantilla del idioma en concreto
        var templatepath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement_{1}.tpl", path, language);

        // Si no existiera la plantilla se genera el path completo de la plantilla sin traducir
        if (!File.Exists(templatepath))
        {
            templatepath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement.tpl", path);
        }

        string text = string.Empty;
        using (var rdr = new StreamReader(templatepath))
        {
            text = rdr.ReadToEnd();
        }

        text = text.Replace("#COMPANY_NAME#", company.Name);
        text = text.Replace("#USER_NAME#", user.UserName);
        text = text.Replace("#EMAIL#", user.Email);
        text = text.Replace("\r", string.Empty);
        text = text.Replace("#DATE#", Constant.NowText);

        var paragraphs = text.Split('\n');
        CreatePDFFromHTMLFile(text, string.Format(CultureInfo.InvariantCulture, @"{0}{1}", path, fileName));
        using (var cmd = new SqlCommand(string.Format(CultureInfo.InvariantCulture, "UPDATE Company SET Agreement = 1 WHERE Id = {0}", company.Id)))
        {
            cmd.CommandType = CommandType.Text;
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (Exception ex)
                {
                    res.SetFail(ex);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        return res;
    }

    /// <summary>Creates a PDF file from an HTML source</summary>
    /// <param name="html">HTML source</param>
    /// <param name="fileName">Name of PDF file</param>
    public static void CreatePDFFromHTMLFile(string html, string fileName)
    {
        try
        {
            var document = new iTS.Document();
            PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
            document.Open();
            var hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(html));
            document.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
}