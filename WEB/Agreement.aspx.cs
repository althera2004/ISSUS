using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF_Tests;
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
using iTSpdf = iTextSharp.text.pdf;

public partial class Agreement : Page
{
    //private string languageBrowser;
    private string ip;
    //private string companyCode;

    public ApplicationUser User { get; private set; }
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

            string[] files = Directory.GetFiles(path);
            Random rnd = new Random();
            int index = rnd.Next(0, files.Count() - 1);
            string res = Path.GetFileName(files[index]);
            Session["BK"] = res;
            return res;
        }
    }

    public string LanguageBrowser
    {
        get
        {
            return Thread.CurrentThread.CurrentUICulture.LCID.ToString();
        }
    }


    public string IP
    {
        get { return this.ip; }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.User = this.Session["User"] as ApplicationUser;
        this.Company = this.Session["Company"] as Company;
        this.RenderAgreement();
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
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement.tpl", path);
        }

        string text = string.Empty;
        using(StreamReader rdr = new StreamReader(path))
        {
            text = rdr.ReadToEnd();
        }

        text = text.Replace("#COMPANY_NAME#", "<strong>" + this.Company.Name + "</strong>");
        text = text.Replace("#USER_NAME#", "<strong>" + this.User.UserName + "</strong>");
        text = text.Replace("#EMAIL#", "<strong>" + this.User.Email + "</strong>");
        text = text.Replace("\r", "</p>");
        text = text.Replace("\n", string.Empty);
        text = text.Replace("#DATE#", string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", DateTime.Now));

        this.LtAgreement.Text = "<p>" + text;
    }

    /// <summary>
    /// Creates agreement document
    /// </summary>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult CreateDocument(int companyId, int userId)
    {
        ActionResult res = ActionResult.NoAction;
        Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        ApplicationUser user = HttpContext.Current.Session["User"] as ApplicationUser;
        Company company = HttpContext.Current.Session["Company"] as Company;

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}.pdf",
            "Agreement",
            company.Name);

        // FONTS
        string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        BaseFont headerFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        BaseFont arial = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        iTS.Document pdfDoc = new iTS.Document(iTS.PageSize.A4, 40, 40, 80, 50);
        iTSpdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc,
           new FileStream(
               string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}", path, fileName),
               FileMode.Create));

        writer.PageEvent = new TwoColumnHeaderFooter()
        {
            CompanyLogo = string.Format(CultureInfo.InvariantCulture, @"{0}\images\logos\{1}.jpg", path, company.Id),
            IssusLogo = string.Format(CultureInfo.InvariantCulture, "{0}issus.png", path),
            Date = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
            CreatedBy = user.UserName,
            CompanyId = company.Id,
            CompanyName = company.Name,
            Title = dictionary["Agreement_Document_Title"].ToUpperInvariant()
        };

        pdfDoc.Open();

        if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        // Se extrae el lenguage por defecto de la empresa
        string language = company.Language;

        // Se genera el path completo de la plantilla del idioma en concreto
        var templatepath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement_{1}.tpl", path, language);

        // Si no existiera la plantilla se genera el path completo de la plantilla sin traducir
        if (!File.Exists(path))
        {
            templatepath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\Agreement.tpl", path);
        }

        string text = string.Empty;
        using (StreamReader rdr = new StreamReader(templatepath))
        {
            text = rdr.ReadToEnd();
        }

        text = text.Replace("#COMPANY_NAME#",company.Name);
        text = text.Replace("#USER_NAME#", user.UserName);
        text = text.Replace("#EMAIL#", user.Email);
        text = text.Replace("\r", string.Empty);
        text = text.Replace("#DATE#", string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", DateTime.Now));

        string[] paragraphs = text.Split('\n');

        foreach (string paragraph in paragraphs) {
            pdfDoc.Add(new Paragraph(paragraph));
        }

        pdfDoc.Open();
        pdfDoc.CloseDocument();

        /*using(StreamWriter fileText = new StreamWriter(path + fileName.Replace(".pdf",".txt")))
        {
            fileText.Write(HttpContext.Current.Request.ToString());
        }*/

        using(SqlCommand cmd = new SqlCommand("UPDATE Company SET Agreement = 1 WHERE Id = " + company.Id.ToString()))
        {
            cmd.CommandType = CommandType.Text;
            using(SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch(Exception ex)
                {
                    res.SetFail(ex);
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

        return res;
    }
}