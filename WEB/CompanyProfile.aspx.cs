using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using GisoFramework.Activity;
using GisoFramework.Item;
using GisoFramework;
using SbrinnaCoreFramework.UI;
using System.Globalization;
using SbrinnaCoreFramework;

public partial class CompanyProfile : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    public string Logo
    {
        get
        {
            return Company.GetLogoFileName(this.company.Id);
        }
    }

    /// <summary>
    /// Gets a random value to prevents static cache files
    /// </summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string AsignedQuote
    {
        get
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", this.company.DiskQuote / (1024M * 1024M)).Replace(',', '*').Replace('.', ',').Replace('*', '.');
        }
    }

    public string CompanyId
    {
        get
        {
            return this.company.Id.ToString().Trim();
        }
    }

    public string DiskQuote
    {
        get
        {
            return UploadFile.GetQuota(this.company);
        }
    }

    public ImageSelector ImgLogo { get; set; }

    public string DefaultCountry
    {
        get
        {
            foreach (Country country in this.company.Countries)
            {
                if (country.Id.ToString() == this.company.DefaultAddress.Country)
                {
                    return country.Description;
                }
            }

            return string.Empty;
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    private FormFooter formFooter;

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    public string CountryData { get; private set; }
    public string Countries { get; private set; }

    /// <summary>
    /// Gets the dictionary for interface texts
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return master.Dictionary;
        }
    }

    public int DefaultAddressId
    {
        get
        {
            return this.master.Company.DefaultAddress.Id;
        }
    }

    public Company Company
    {
        get
        {
            return this.company;
        }
    }

    public CompanyAddress CompanyDefaultAddress
    {
        get
        {
            return this.master.Company.DefaultAddress;
        }
    }

    public string Addresses
    {
        get
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (CompanyAddress address in this.master.Company.Addresses)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(address.Json).Append(Environment.NewLine);
            }

            return res.Append("]").ToString();
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.user = Session["User"] as ApplicationUser;
        this.company = Session["Company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_CompanyData");
        this.master.Titulo = "Item_CompanyData";
        this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(Convert.ToInt32(Session["CompanyId"]), TargetType.Company);
        this.RenderCountries();

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Action = "success", Icon = "icon-ok", Text = this.dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        this.ImgLogo = new ImageSelector()
        {
            Name = "Equipment",
            ImageName = string.Format(CultureInfo.InvariantCulture, @"images\Logos\{0}", this.Logo),
            Width = 300,
            Height = 300,
            Label = this.dictionary["Item_Equipment_Field_Image_Label"]
        };

        this.RenderCountriesTab();
        this.LtIdiomas.Text = "<option value=\"es\"" + (this.user.Language == "es" ? " selected=\"selected\"" : string.Empty) + ">Castellano</option>";
        this.LtIdiomas.Text += "<option value=\"ca\"" + (this.user.Language == "ca" ? " selected=\"selected\"" : string.Empty) + ">Català</option>";
    }

    private void RenderCountries()
    {
        StringBuilder res = new StringBuilder();
        string countryCompare = this.company.Id < 0 ? this.dictionary["Common_None"] : this.company.DefaultAddress.Country;
        res.Append(string.Format(@"{{
            ""text"": ""{0}"",
            ""value"": ""0"",
            ""selected"": {1},
            ""description"": ""{0}""
        }}", this.dictionary["Common_None"], countryCompare == this.dictionary["Common_None"] ? "true" : "false"));

        if (this.user.Language == "es")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Alemania" ? "true" : "false", "Alemania", "Alemania", 8);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Argentina" ? "true" : "false", "Argentina", "Argentina", 18);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Brasil" ? "true" : "false", "Brasil", "Brasil", 20);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Cataluña", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "España", "España", 1);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "Francia", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Italia" ? "true" : "false", "Italia", "Italia", 5);
            //res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Portugal" ? "true" : "false", "Portugal", "Portugal", 2);
        }

        if (this.user.Language == "ca")
        {
            //res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Alemania" ? "true" : "false", "Alemanya", "Alemania", 8);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            //res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Argentina" ? "true" : "false", "Argentina", "Argentina", 18);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            //res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Brasil" ? "true" : "false", "Brasil", "Brasil", 20);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalunya", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Espanya", "España", 1);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "França", "Francia", 7);
            //res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Italia" ? "true" : "false", "Italia", "Italia", 5);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "UK" ? "true" : "false", "UK", "UK", 12);
            //res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Portugal" ? "true" : "false", "Portugal", "Portugal", 2);
        }

        if (this.user.Language == "en")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Argentina" ? "true" : "false", "Argentina", "Argentina", 18);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Basque Country", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Brasil" ? "true" : "false", "Brazil", "Brasil", 20);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalonia", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "France", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Alemania" ? "true" : "false", "Germany", "Alemania", 8);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Italia" ? "true" : "false", "Italy", "Italia", 5);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Portugal" ? "true" : "false", "Portugal", "Portugal", 2);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Spain", "España", 1);
        }

        /*foreach (Country country in this.company.Countries)
        {
            res.Append(string.Format(@",{{
                ""text"": ""{0}"",
                ""value"": ""{2}"",
                ""selected"": {1},
                ""description"": ""{0}"",
                ""imageSrc"": ""assets/flags/{2}.png""
            }}", country.Description, country.Description == countryCompare ? "true" : "false", country.Id));
        }*/

        this.CountryData = res.ToString();
    }

    private void RenderCountriesTab()
    {
        StringBuilder selected = new StringBuilder();
        StringBuilder availables = new StringBuilder();
        StringBuilder countries = new StringBuilder(Environment.NewLine).Append("        [");
        bool first = true;
        foreach (Country country in Country.GetAll(this.company.Id))
        {
            if (country.Selected)
            {
                selected.Append(country.SelectedTag);
            }
            else
            {
                availables.Append(country.AvailableTag);
            } 
            
            if (first)
            {
                first = false;
            }
            else
            {
                countries.Append(",");
            }

            countries.Append(Environment.NewLine);
            string pattern = @"            {{""Id"":{0}, ""Description"":""{1}"", ""Selected"": {2}, ""Deletable"": {3}}}";

            countries.Append(string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                country.Id,
                country.Description,
                country.Selected ? "true" : "false",
                country.CanBeDelete ? "true" : "false"));
        }

        this.Countries = countries.Append(Environment.NewLine).Append("        ]").ToString();
    }
}