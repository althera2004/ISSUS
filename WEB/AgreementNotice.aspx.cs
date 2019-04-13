// --------------------------------
// <copyright file="AgreementNotice.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;

public partial class AgreementNotice : Page
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
    }
}