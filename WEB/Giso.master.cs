// --------------------------------
// <copyright file="Giso.master.cs" company="Sbrinna">
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
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Alerts;
using GisoFramework.Item;
using GisoFramework.UserInterface;
using SbrinnaCoreFramework.UI;

/// <summary>Implements the master page of application</summary>
public partial class Giso : MasterPage
{
    /// <summary>Title of page</summary>
    private string titulo;

    /// <summary>List of navigation history urls</summary>
    private List<string> navigation;

    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public int PendentTasks { get; set; }

    public string IssusVersion
    {
        get
        {
            return ConfigurationManager.AppSettings["issusversion"];
        }
    }

    public string BK
    {
        get
        {
            return Session["BK"] as string;
        }
    }

    /// <summary>Gets the navigation history</summary>    
    public string NavigationHistory
    {
        get
        {
            var res = new StringBuilder(Environment.NewLine).Append("<!-- Havigation history -->").Append(Environment.NewLine);
            foreach(string link in this.navigation)
            {
                res.Append(string.Format(CultureInfo.InvariantCulture, "    {0}{1}<br />", link, Environment.NewLine));
            }

            res.Append("<!-- -->").Append(Environment.NewLine);
            return res.ToString();
        }
    }

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    public ApplicationUser ApplicationUser { get; set; }

    private Collection<BreadcrumbItem> breadCrumb;

    public string ItemCode { get; set; }

    public string Skin
    {
        get
        {
            string customCss = string.Format(CultureInfo.InvariantCulture, @"customization/{0}.css", this.Company.Id);
            if (File.Exists(this.Request.PhysicalApplicationPath + customCss))
            {
                return string.Format(CultureInfo.InvariantCulture, @"<link rel=""stylesheet"" href=""{0}"" />", customCss);
            }

            return "<link rel=\"stylesheet\" href=\"assets/css/ace.min.css\" />";
        }
    }

    /// <summary>Gets or sets a JSON list of searcheable items</summary>
    public string SearcheableItems { get; set; }

    /// <summary>Gets the url of referrer page</summary>
    public string Referrer
    {
        get
        {
            string actual = this.Request.RawUrl;
            string res = string.Empty;

            if(actual.ToUpperInvariant().IndexOf("DASHBOARD.ASPX") != -1)
            {
                var newNavigation = new List<string>
                {
                    "Dashboard.aspx"
                };
                Session["Navigation"] = newNavigation;
                return "DashBoard.aspx";
            }

            string last1 = string.Empty;
            string last2 = string.Empty;
            string last3 = string.Empty;

            if (this.navigation.Count > 2)
            {
                last1 = this.navigation[this.navigation.Count - 1];
                last2 = this.navigation[this.navigation.Count - 2];
                last3 = this.navigation[this.navigation.Count - 3];
            }
            else if (this.navigation.Count > 1)
            {
                last1 = this.navigation[this.navigation.Count - 1];
                last2 = this.navigation[this.navigation.Count - 2];
            }
            else if (this.navigation.Count == 1)
            {
                last1 = this.navigation[0];
            }

            if(actual == last1)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last2;
            }
            else if (actual == last2)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last3;
            }
            else if (last1.IndexOf(".aspx?id=-1") != -1)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last2;
            }
            else
            {
                res = last1;
            }

            if (actual != res)
            {
                this.navigation.Add(actual);
            }

            Session["Navigation"] = this.navigation;


            return res;
        }
    }

    /// <summary>Gets or sets a value indicating whether if is an administration page</summary>
    public bool AdminPage { get; set; }

    /// <summary>Gets de breadcrumb elements</summary>
    public Collection<BreadcrumbItem> BreadCrumb
    {
        get
        {
            return this.breadCrumb;
        }
    }

    /// <summary>Gets a value indicating whether if the text of title is translatable</summary>
    public bool TitleInvariant { get; set; }

    /// <summary>Gets the HTML code for breadcrumb object</summary>
    public string RenderBreadCrumb
    {
        get
        {
            var res = new StringBuilder("<li><i class=\"icon-cog home-icon\"></i><a href=\"").Append(Session["home"].ToString()).Append("\">").Append(this.dictionary["Common_Home"]).Append("</a></li>");
            foreach (var item in this.breadCrumb)
            {
                string label = item.Invariant ? item.Label : this.dictionary[item.Label];
                if (item.Leaf)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<li class=""active"">{0}</li>",
                        label);
                }
                else
                {
                    string link = "#";
                    if (!string.IsNullOrEmpty(item.Link))
                    {
                        link = item.Link;
                    }

                    res.Append("<li><a href=\"").Append(link).Append("\" title=\"").Append(label).Append("\">").Append(this.dictionary[item.Label]).Append("</a></li>");
                }
            }

            return res.ToString();
        }
    }

    public string UserName
    {
        get
        {
            if(!string.IsNullOrEmpty(this.ApplicationUser.Description))
            {
                return this.ApplicationUser.Description;
            }

            return this.ApplicationUser.UserName;
        }
    }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {            
            return this.dictionary;
        }
    }

    public string UserLanguage
    {
        get
        {
            return this.ApplicationUser.Language;
        }
    }

    public Company Company { get; private set; }

    public string Titulo
    {
        get
        {
            if (this.TitleInvariant)
            {
                return this.titulo;
            }

            return this.dictionary[this.titulo];
        }

        set
        {
            this.titulo = value;
        }
    }

    public UIButton ButtonNewItem { get; set; }

    public string ButtonNewItemHtml
    {
        get
        {
            if (this.ButtonNewItem != null)
            {
                return this.ButtonNewItem.Render;
            }

            return string.Empty;
        }
    }

    public void AddBreadCrumb(string label)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem
        {
            Link = "#",
            Label = label,
            Leaf = true
        };

        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumbInvariant(string label)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem
        {
            Link = "#",
            Label = label,
            Leaf = true,
            Invariant = true
        };

        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumb(string label, bool leaf)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem
        {
            Link = "#",
            Label = label,
            Leaf = leaf
        };

        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumb(string label, string link, bool leaf)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem
        {
            Link = link,
            Label = label,
            Leaf = leaf
        };

        this.breadCrumb.Add(newBreadCrumb);
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.LtBuild.Text = ConfigurationManager.AppSettings["issusVersion"];
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.Session["LastTime"] = DateTime.Now;
        this.navigation = Session["Navigation"] as List<string>;

        this.ApplicationUser = Session["User"] as ApplicationUser;
        this.Company = Session["Company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        // Renew session
        //// ---------------------------------
        this.Session["User"] = this.ApplicationUser;
        this.Session["Company"] = this.Company;
        this.Session["Dictionary"] = this.dictionary;
        //// ---------------------------------

        this.LeftMenu.Text += MenuOption.RenderMenu((ReadOnlyCollection<MenuOption>)Session["Menu"]);
        this.RenderShortCuts();

        string logo = Company.GetLogoFileName(this.Company.Id);
        this.ImgCompany.ImageUrl = string.Format("/images/Logos/{0}?ac={1}", logo, Guid.NewGuid());
        this.ImgCompany.Attributes.Add("height", "30");

        var tasks = ScheduledTask.ByEmployee(this.ApplicationUser.Employee.Id, this.Company.Id).Where(t => t.Expiration >= DateTime.Now.AddYears(-1)).ToList();
        tasks = tasks.OrderByDescending(t => t.Expiration).ToList();
        var printedTasks = new List<ScheduledTask>();
        foreach (var task in tasks)
        {
            if (printedTasks.Any(t => t.Description.Equals(task.Description) && t.Equipment.Id == task.Equipment.Id && task.TaskType == t.TaskType))
            {
                continue;
            }

            printedTasks.Add(task);
        }

        this.PendentTasks = printedTasks.Count;
    }

    private void RenderShortCuts()
    {
        var res = new StringBuilder(@"<div class=""sidebar-shortcuts"" id=""sidebar-shortcuts"">");        
        var big = new StringBuilder(@"<div class=""sidebar-shortcuts-large"" id=""sidebar-shortcuts-large"">");
        var small = new StringBuilder(@"<div class=""sidebar-shortcuts-mini"" id=""sidebar-shortcuts-mini"">");
        bool showShortCuts = false;
        //if (this.ApplicationUser.MenuShortcuts != null)
        //{
            if (this.ApplicationUser.MenuShortcuts.Blue != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Blue.Label))
            {
                showShortCuts = true;
                big.Append(string.Format(@"<button type=""button"" class=""btn btn-info"" style=""height:32px;"" onclick=""document.location='{0}';"" title=""{1}""><i class=""{2}""></i></button>", this.ApplicationUser.MenuShortcuts.Blue.Link, this.Dictionary[this.ApplicationUser.MenuShortcuts.Blue.Label], this.ApplicationUser.MenuShortcuts.Blue.Icon));
                small.Append(@"<span class=""btn btn-info""></span>");
            }

            if (this.ApplicationUser.MenuShortcuts.Green != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Green.Label))
            {
                showShortCuts = true;
                big.Append(string.Format(@"<button type=""button"" class=""btn btn-success"" style=""height:32px;"" onclick=""document.location='{0}';"" title=""{1}""><i class=""{2}""></i></button>", this.ApplicationUser.MenuShortcuts.Green.Link, this.Dictionary[this.ApplicationUser.MenuShortcuts.Green.Label], this.ApplicationUser.MenuShortcuts.Green.Icon));
                small.Append(@"<span class=""btn btn-success""></span>");
            }

            if (this.ApplicationUser.MenuShortcuts.Red != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Red.Label))
            {
                showShortCuts = true;
                big.Append(string.Format(@"<button type=""button"" class=""btn btn-danger"" style=""height:32px;"" onclick=""document.location='{0}';"" title=""{1}""><i class=""{2}""></i></button>", this.ApplicationUser.MenuShortcuts.Red.Link, this.Dictionary[this.ApplicationUser.MenuShortcuts.Red.Label], this.ApplicationUser.MenuShortcuts.Red.Icon));
                small.Append(@"<span class=""btn btn-danger""></span>");
            }

            if (this.ApplicationUser.MenuShortcuts.Yellow != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Yellow.Label))
            {
                showShortCuts = true;
                big.Append(string.Format(@"<button type=""button"" class=""btn btn-warning"" style=""height:32px;"" onclick=""document.location='{0}';"" title=""{1}""><i class=""{2}""></i></button>", this.ApplicationUser.MenuShortcuts.Yellow.Link, this.Dictionary[this.ApplicationUser.MenuShortcuts.Yellow.Label], this.ApplicationUser.MenuShortcuts.Yellow.Icon));
                small.Append(@"<span class=""btn btn-warning""></span>");
            }
        //}
        
        big.Append("</div>");
        small.Append("</div>");

        res.Append(big).Append(small);
        res.Append(@"</div>");

        if (showShortCuts)
        {
            this.LtMenuShortCuts.Text = res.ToString();
        }
        else
        {
            this.LtMenuShortCuts.Text = string.Empty;
        }

        this.Alerts();
    }

    /// <summary>Render alerts items on top menu</summary>
    private void Alerts()
    {
        int cont = 0;
        var res = new StringBuilder();
        var show = Session["AlertsDefinition"] as ReadOnlyCollection<AlertDefinition>;
        var alertsTags = new List<string>();
        foreach (var alertDefinition in show)
        {
            if (!this.ApplicationUser.HasGrantToRead(alertDefinition.ItemType))
            {
                continue;
            }

            var alerts = alertDefinition.Extract();
            foreach (string result in alerts)
            {
                alertsTags.Add(result);
            }

            cont += alerts.Count;
        }

        foreach(string tag in alertsTags.Take(8))
        {
            res.Append(tag);
        }

        res.Append("<div></div>");

        if(cont > 8)
        {
            res.AppendFormat(@"<li class=""dropdown-footer""><a href=""Alerts.aspx"">{0}<i class=""ace-icon fa fa-arrow-right""></i></a></li>", this.dictionary["Common_ViewAll"]);
        }

        this.LtAlertsCount.Text = cont.ToString();
        this.LtAlertsCount2.Text = cont.ToString();
        this.LtAlerts.Text = res.ToString();
    }
}