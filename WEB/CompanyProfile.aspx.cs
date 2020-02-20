// --------------------------------
// <copyright file="CompanyProfile.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class CompanyProfile : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    public string Logo
    {
        get
        {
            return Company.GetLogoFileName(this.Company.Id);
        }
    }

    /// <summary>Gets a random value to prevents static cache files</summary>
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
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", this.Company.DiskQuote);//.Replace(',', '*').Replace('.', ',').Replace('*', '.');
        }
    }

    public string DiskQuote
    {
        get
        {
            return UploadFile.GetQuota(this.Company);
        }
    }

    public ImageSelector ImgLogo { get; set; }

    public string DefaultCountry
    {
        get
        {
            foreach (var country in this.Company.Countries)
            {
                if (country.Id.ToString() == this.Company.DefaultAddress.Country)
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
            return this.formFooter.Render(this.Dictionary);
        }
    }

    public string CountryData { get; private set; }
    public string Countries { get; private set; }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public int DefaultAddressId
    {
        get
        {
            return this.master.Company.DefaultAddress.Id;
        }
    }

    public Company Company { get; private set; }

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
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var address in this.master.Company.Addresses)
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

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);

            }
            else
            {
                this.Go();
            }
        }

        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.user = Session["User"] as ApplicationUser;
        this.Company = Session["Company"] as Company;
        GetCompany();
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_CompanyData");
        this.master.Titulo = "Item_CompanyData";
        this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(Convert.ToInt32(Session["CompanyId"]), TargetType.Company);
        this.RenderCountries();

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Action = "success", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        this.ImgLogo = new ImageSelector
        {
            Name = "Equipment",
            ImageName = string.Format(CultureInfo.InvariantCulture, @"images\Logos\{0}", this.Logo),
            Width = 300,
            Height = 300,
            Label = this.Dictionary["Item_Equipment_Field_Image_Label"]
        };

        this.RenderCountriesTab();
        this.LtIdiomas.Text = "<option value=\"es\"" + (this.user.Language == "es" ? " selected=\"selected\"" : string.Empty) + ">Castellano</option>";
        this.LtIdiomas.Text += "<option value=\"ca\"" + (this.user.Language == "ca" ? " selected=\"selected\"" : string.Empty) + ">Català</option>";
        this.LtIdiomas.Text += "<option value=\"en\"" + (this.user.Language == "en" ? " selected=\"selected\"" : string.Empty) + ">English</option>";
    }

    private void RenderCountries()
    {
        var res = new StringBuilder();
        string countryCompare = this.Dictionary["Common_None"];
		try 
		{
			countryCompare = this.Company.Id < 0 ? this.Dictionary["Common_None"] : this.Company.DefaultAddress.Country;
		}
		catch(Exception ex)
		{
		}
		
        res.Append(string.Format(@"{{
            ""text"": ""{0}"",
            ""value"": ""0"",
            ""selected"": {1},
            ""description"": ""{0}""
        }}", this.Dictionary["Common_None"], countryCompare == this.Dictionary["Common_None"] ? "true" : "false"));

        if (this.user.Language == "es")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Cataluña", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "España", "España", 1);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "Francia", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Great Britain" ? "true" : "false", "Gran Bretaña", "Great Britain", 12);
        }

        if (this.user.Language == "ca")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalunya", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Espanya", "España", 1);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "França", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Great Britain" ? "true" : "false", "Gran Bretanya", "Gran Bretanya", 12);
        }

        if (this.user.Language == "en")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalonia", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Spain", "España", 1);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Basque Country", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "France", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Great Britain" ? "true" : "false", "Great Britain", "Great Britain", 12);
        }

        this.CountryData = res.ToString();
    }

    private void RenderCountriesTab()
    {
        var selected = new StringBuilder();
        var availables = new StringBuilder();
        var countries = new StringBuilder(Environment.NewLine).Append("[");
        bool first = true;
        foreach (var country in Country.GetAll(this.Company.Id))
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
            string pattern = @"{{""Id"":{0}, ""Description"":""{1}"", ""Selected"": {2}, ""Deletable"": {3}}}";

            countries.Append(string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                country.Id,
                country.Description,
                country.Selected ? "true" : "false",
                country.CanBeDelete ? "true" : "false"));
        }

        this.Countries = countries.Append(Environment.NewLine).Append("]").ToString();
    }

    private void GetCompany()
    {
        using (var cmd = new SqlCommand("Company_GetById"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters["@CompanyId"].Value = this.Company.Id;

                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            this.Company.Headquarters = rdr.GetString(10);
                        }
                    }
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
    }
}