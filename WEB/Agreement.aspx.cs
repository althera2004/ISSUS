using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using iTS = iTextSharp.text;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;
using iTextSharp.text.html.simpleparser;
using System.Text;

public partial class Agreement : Page
{
    //private string languageBrowser;
    private string ip;
    //private string companyCode;

    public ApplicationUser ApplicationUser { get; private set; }
    public Company Company { get; private set; }

    public string HomePage
    {
        get
        {
            string landPage = "/DashBoard.aspx";
            Session["home"] = landPage;
            return landPage;
        }
    }

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
            Session["BK"] = res;
            return res;
        }
    }

    public string LanguageBrowser
    {
        get
        {
            return this.Company.Language;
        }
    }

    public string IP
    {
        get { return this.ip; }
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

    private void RenderAgreement()
    {
        string path = this.Request.PhysicalApplicationPath;
        if(!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        // Se extrae el lenguage por defecto de la empresa
        string language = this.Company.Language;

        // Se genera el path completo de la plantilla del idioma en concreto
        path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement_{1}.tpl", path, language);

        // Si no existiera la plantilla se genera el path completo de la plantilla sin traducir
        if(!File.Exists(path))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement.tpl", this.Request.PhysicalApplicationPath);
        }

        string textEs = string.Empty;
		
        path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement_es.tpl", this.Request.PhysicalApplicationPath);
        using(var rdr = new StreamReader(path))
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

    /// <summary>Creates agreement document</summary>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
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

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"Agreement\{0}_{1}.pdf",
            "Agreement",
            company.Name);

        // FONTS
        /*string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        var headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        var pdfDoc = new iTS.Document(iTS.PageSize.A4, 40, 40, 80, 50);
        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, company.Id),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = Constant.NowText,
            CreatedBy = user.UserName,
            CompanyId = company.Id,
            CompanyName = company.Name,
            Title = dictionary["Agreement_Document_Title"].ToUpperInvariant(),
            NoFooter = true
        };

        pdfDoc.Open();*/

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

        text = text.Replace("#COMPANY_NAME#",company.Name);
        text = text.Replace("#USER_NAME#", user.UserName);
        text = text.Replace("#EMAIL#", user.Email);
        text = text.Replace("\r", string.Empty);
        text = text.Replace("#DATE#", Constant.NowText);


        var paragraphs = text.Split('\n');

        /*foreach (string paragraph in paragraphs) {
            pdfDoc.Add(new Paragraph(paragraph));
        }*/

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

    public static void CreatePDFFromHTMLFile(string html, string file)
    {
        try
        {
            var document = new iTS.Document();
            PdfWriter.GetInstance(document, new FileStream(file, FileMode.Create));
            document.Open();
            iTextSharp.text.html.simpleparser.HTMLWorker hw =
                         new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(html));
            document.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}