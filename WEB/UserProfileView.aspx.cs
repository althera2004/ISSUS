// --------------------------------
// <copyright file="UserProfileView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using GisoFramework;
using GisoFramework.Item;
using GisoFramework.UserInterface;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class UserProfileView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Gets user logged in session</summary>
    public ApplicationUser ApplicationUser { get; private set; }

    /// <summary>Company of session</summary>
    private Company company;

    private FormFooter formFooter;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    private StringBuilder shortcutsJson;
    private StringBuilder userShortcuts;

    public string HomePage { get { return Session["Home"].ToString().Trim(); } }

    public string Description
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

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
        }
    }

    public string UserShortcuts
    {
        get
        {
            return this.userShortcuts.ToString();
        }
    }

    public string ShortcutsJson
    {
        get
        {
            return this.shortcutsJson.ToString();
        }
    }

    public string ShowHelpChecked
    {
        get
        {
            return this.ApplicationUser.ShowHelp ? " checked=\"checked\"" : string.Empty;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>Gets code of company</summary>
    public string CompanyCode
    {
        get
        {
            return this.company.Code;
        }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            this.ApplicationUser = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.ApplicationUser.Id))
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
        this.ApplicationUser = Session["User"] as ApplicationUser;
        this.company = Session["company"] as Company;
        this.ApplicationUser = new ApplicationUser(this.ApplicationUser.Id);
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AddBreadCrumbInvariant(this.ApplicationUser.UserName);
        this.master.Titulo = this.ApplicationUser.UserName;
        this.master.TitleInvariant = true;

        this.CmbShorcuts.Items.Clear();
        this.CmbShorcuts.Items.Add(new ListItem(this.Dictionary["Common_None"], "0"));
        bool first = true;
        this.shortcutsJson = new StringBuilder("[");
        foreach (var shortcut in Shortcut.Available(this.ApplicationUser.Id))
        {
            if (first)
            {
                first = false;
            }
            else
            {
                this.shortcutsJson.Append(",");
            }

            this.shortcutsJson.Append(shortcut.Json(this.Dictionary));
            CmbShorcuts.Items.Add(new ListItem(this.Dictionary[shortcut.Label], shortcut.Id.ToString(CultureInfo.GetCultureInfo("en-us"))));
        }

        this.shortcutsJson.Append("]");
        this.RenderShortCuts();

        if (!this.IsPostBack)
        {
            this.CmbShorcuts.Attributes.Add("onchange", "CmbShortcutsChanged(this.value);");
        }

        var avatars = new StringBuilder();
        var filePaths = Directory.GetFiles(string.Format(CultureInfo.InvariantCulture, @"{0}/assets/avatars/", this.Request.PhysicalApplicationPath));
        foreach (string fileName in filePaths)
        {
            string title = this.Dictionary["Common_SelectAll"];
            string color = "avatar";
            string action = string.Format(CultureInfo.InvariantCulture, @"SelectAvatar('{0}');", Path.GetFileName(fileName));
            if (fileName.IndexOf(this.ApplicationUser.Avatar) != -1)
            {
                color = "avatarSelected";
                title = this.Dictionary["Common_Selected"];
                action = string.Format("alert('{0}');", title);
            }

            avatars.Append(string.Format(@"<div id=""{0}"" class=""{1}""><img src=""/assets/avatars/{0}"" title=""{2}"" onclick=""{3}"" /></div>", Path.GetFileName(fileName), color, title, action));
        }

        this.LtAvatar.Text = avatars.ToString();

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Text = this.Dictionary["Common_Accept"], Icon = "icon-ok", Action = "success" });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Text = this.Dictionary["Common_Cancel"], Icon = "icon-undo" });
    }

    private void RenderShortCuts()
    {
        var res = new StringBuilder(@"<div class=""sidebar-shortcuts"" id=""sidebar-shortcuts"">");
        var big = new StringBuilder(@"<div class=""sidebar-shortcuts-large"" id=""sidebar-shortcuts-large"">");
        var small = new StringBuilder(@"<div class=""sidebar-shortcuts-mini"" id=""sidebar-shortcuts-mini"">");
        this.userShortcuts = new StringBuilder("[");

        //if (this.user.MenuShortcuts != null)
        //{
            if (this.ApplicationUser.MenuShortcuts.Blue != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Blue.Label))
            {
                big.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"
                            <span id=""ButtonBlue"" class=""btn btn-info"" style=""height:32px;"" onclick=""select('Blue','{0}');"" title=""{0}"">
                                <i id=""IconBlue"" class=""{1}""></i>
                            </span>",
                    this.Dictionary[this.ApplicationUser.MenuShortcuts.Blue.Label],
                    this.ApplicationUser.MenuShortcuts.Blue.Icon);
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Blue"", ""Id"":{0}}},", this.ApplicationUser.MenuShortcuts.Blue.Id));
            }
            else
            {
                big.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"
                            <span id=""ButtonBlue"" class=""btn btn-info"" style=""height:32px;"" onclick=""select('Blue','{0}');"" title=""{0}"">
                                <i id=""IconBlue"" class=""{1}""></i>
                            </span>", 
                    string.Empty,
                    string.Empty);
                this.userShortcuts.Append(@"{""Color"":""Blue"", ""Id"":null},");
            }

            if (this.ApplicationUser.MenuShortcuts.Green != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Green.Label))
            {
                big.Append(string.Format(@"
                            <span id=""ButtonGreen"" class=""btn btn-success"" style=""height:32px;"" onclick=""select('Green','{1}');"" title=""{1}"">
                                <i id=""IconGreen"" class=""{2}""></i>
                            </span>", this.ApplicationUser.MenuShortcuts.Green.Link, this.Dictionary[this.ApplicationUser.MenuShortcuts.Green.Label], this.ApplicationUser.MenuShortcuts.Green.Icon));
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Green"", ""Id"":{0}}},", this.ApplicationUser.MenuShortcuts.Green.Id));
            }
            else
            {
                big.Append(string.Format(@"
                            <span id=""ButtonGreen"" class=""btn btn-success"" style=""height:32px;"" onclick=""select('Green','{1}');"" title=""{1}"">
                                <i id=""IconGreen"" class=""{2}""></i>
                            </span>", string.Empty, string.Empty, string.Empty));
                this.userShortcuts.Append(@"{""Color"":""Green"", ""Id"":null},");
            }

            if (this.ApplicationUser.MenuShortcuts.Red != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Red.Label))
            {
                big.Append(string.Format(@"
                            <span id=""ButtonRed"" class=""btn btn-danger"" style=""height:32px;"" onclick=""select('Red','{1}');"" title=""{1}"">
                                <i id=""IconRed"" class=""{2}""></i>
                            </span>", this.ApplicationUser.MenuShortcuts.Red.Link, this.Dictionary[this.ApplicationUser.MenuShortcuts.Red.Label], this.ApplicationUser.MenuShortcuts.Red.Icon));
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Red"", ""Id"":{0}}},", this.ApplicationUser.MenuShortcuts.Red.Id));
            }
            else
            {
                big.Append(string.Format(@"
                            <span id=""ButtonRed"" class=""btn btn-danger"" style=""height:32px;"" onclick=""select('Red','{1}');"" title=""{1}"">
                                <i id=""IconRed"" class=""{2}""></i>
                            </span>", string.Empty, string.Empty, string.Empty));
                this.userShortcuts.Append(@"{""Color"":""Red"", ""Id"":null},");
            }

            if (this.ApplicationUser.MenuShortcuts.Yellow != null && !string.IsNullOrEmpty(this.ApplicationUser.MenuShortcuts.Yellow.Label))
            {
                big.Append(string.Format(@"
                            <span id=""ButtonYellow"" class=""btn btn-warning"" style=""height:32px;"" onclick=""select('Yellow','{1}');"" title=""{1}"">
                                <i id=""IconYellow"" class=""{2}""></i>
                            </span>", this.ApplicationUser.MenuShortcuts.Yellow.Link, this.Dictionary[this.ApplicationUser.MenuShortcuts.Yellow.Label], this.ApplicationUser.MenuShortcuts.Yellow.Icon));
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Yellow"", ""Id"":{0}}}", this.ApplicationUser.MenuShortcuts.Yellow.Id));
            }
            else
            {
                big.Append(string.Format(@"
                            <span id=""ButtonYellow"" class=""btn btn-warning"" style=""height:32px;"" onclick=""select('Yellow','{1}');"" title=""{1}"">
                                <i id=""IconYellow"" class=""{2}""></i>
                            </span>", string.Empty, string.Empty, string.Empty));
                this.userShortcuts.Append(@"{""Color"":""Yellow"", ""Id"":null}");
            }
        //}

        this.userShortcuts.Append("]");
        big.Append("</div>");
        small.Append("</div>");

        res.Append(big).Append(small);
        res.Append(@"</div>");

        this.LtMenuShortCuts.Text = res.ToString();

        this.LtIdiomas.Text = "<option value=\"es\"" + (this.ApplicationUser.Language == "es" ? " selected=\"selected\"" : string.Empty) + ">Castellano</option>";
        this.LtIdiomas.Text += "<option value=\"ca\"" + (this.ApplicationUser.Language == "ca" ? " selected=\"selected\"" : string.Empty) + ">Català</option>";
        this.LtIdiomas.Text += "<option value=\"en\"" + (this.ApplicationUser.Language == "en" ? " selected=\"selected\"" : string.Empty) + ">English</option>";
    }
}