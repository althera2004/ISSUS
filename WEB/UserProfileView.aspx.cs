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

    /// <summary>
    /// User logged in session
    /// </summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private FormFooter formFooter;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private int userId;
    private StringBuilder shortcutsJson;
    private StringBuilder userShortcuts;

    public string HomePage { get { return Session["Home"].ToString().Trim(); } }

    public string Description
    {
        get
        {
            if(!string.IsNullOrEmpty(this.user.Description))
            {
                return this.user.Description;
            }

            return this.user.UserName;
        }
    }

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    public string Avatar
    {
        get
        {
            return this.user.AvatarImage;
        }
    }

    /// <summary>
    /// Gets a value indicating if user has Admin privileges in Company
    /// </summary>
    public bool Admin
    {
        get
        {
            return this.user.Admin;
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

    public ApplicationUser ApplicationUser
    {
        get
        {
            return this.user;
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string ShowHelpChecked
    {
        get
        {
            return this.user.ShowHelp ? " checked=\"checked\"" : string.Empty;
        }
    }

    public string UserJson
    {
        get
        {
            return this.user.Json;
        }
    }

    /// <summary>
    /// Gets dictionary for fixed labels
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Gets code of company
    /// </summary>
    public string CompanyCode
    {
        get
        {
            return this.company.Code;
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
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
        this.company = Session["company"] as Company;
        this.user = new ApplicationUser(this.user.Id);
        //this.user.Employee = new Employee(this.user.Employee.Id, false);
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AddBreadCrumbInvariant(this.user.UserName);
        this.master.Titulo = this.user.UserName;
        this.master.TitleInvariant = true;

        this.CmbShorcuts.Items.Clear();
        this.CmbShorcuts.Items.Add(new ListItem(this.dictionary["Common_None"], "0"));
        bool first = true;
        this.shortcutsJson = new StringBuilder("[");
        foreach (Shortcut shortcut in Shortcut.Available(this.user.Id))
        {
            if (first)
            {
                first = false;
            }
            else
            {
                this.shortcutsJson.Append(",");
            }

            this.shortcutsJson.Append(shortcut.Json(this.dictionary));
            CmbShorcuts.Items.Add(new ListItem(this.dictionary[shortcut.Label], shortcut.Id.ToString(CultureInfo.GetCultureInfo("en-us"))));
        }

        this.shortcutsJson.Append("]");
        this.RenderShortCuts();

        if (!this.IsPostBack)
        {
            this.CmbShorcuts.Attributes.Add("onchange", "CmbShortcutsChanged(this.value);");
        }

        StringBuilder avatars = new StringBuilder();
        string[] filePaths = Directory.GetFiles(string.Format(CultureInfo.GetCultureInfo("es-es"), @"{0}/assets/avatars/", this.Request.PhysicalApplicationPath));
        foreach (string fileName in filePaths)
        {
            string title = this.dictionary["Common_SelectAll"];
            string color = "avatar";
            string action = string.Format(CultureInfo.GetCultureInfo("es-es"), @"SelectAvatar('{0}');", Path.GetFileName(fileName));
            if (fileName.IndexOf(this.user.Avatar) != -1)
            {
                color = "avatarSelected";
                title = this.dictionary["Common_Selected"];
                action = string.Format("alert('{0}');", title);
            }

            avatars.Append(string.Format(@"<div id=""{0}"" class=""{1}""><img src=""/assets/avatars/{0}"" title=""{2}"" onclick=""{3}"" /></div>", Path.GetFileName(fileName), color, title, action));
        }

        this.LtAvatar.Text = avatars.ToString();

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new SbrinnaCoreFramework.UI.UIButton() { Id = "BtnSave", Text = this.dictionary["Common_Accept"], Icon = "icon-ok", Action = "success" });
        this.formFooter.AddButton(new SbrinnaCoreFramework.UI.UIButton() { Id = "BtnCancel", Text = this.dictionary["Common_Cancel"], Icon = "icon-undo" });
    }

    private void RenderShortCuts()
    {
        StringBuilder res = new StringBuilder(@"<div class=""sidebar-shortcuts"" id=""sidebar-shortcuts"">");
        StringBuilder big = new StringBuilder(@"<div class=""sidebar-shortcuts-large"" id=""sidebar-shortcuts-large"">");
        StringBuilder small = new StringBuilder(@"<div class=""sidebar-shortcuts-mini"" id=""sidebar-shortcuts-mini"">");
        this.userShortcuts = new StringBuilder("[");

        if (this.user.MenuShortcuts != null)
        {
            if (this.user.MenuShortcuts.Blue != null && !string.IsNullOrEmpty(this.user.MenuShortcuts.Blue.Label))
            {
                big.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"
                            <span id=""ButtonBlue"" class=""btn btn-info"" style=""height:32px;"" onclick=""select('Blue','{0}');"" title=""{0}"">
                                <i id=""IconBlue"" class=""{1}""></i>
                            </span>",
                    this.Dictionary[this.user.MenuShortcuts.Blue.Label],
                    this.user.MenuShortcuts.Blue.Icon);
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Blue"", ""Id"":{0}}},", this.user.MenuShortcuts.Blue.Id));
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

            if (this.user.MenuShortcuts.Green != null && !string.IsNullOrEmpty(this.user.MenuShortcuts.Green.Label))
            {
                big.Append(string.Format(@"
                            <span id=""ButtonGreen"" class=""btn btn-success"" style=""height:32px;"" onclick=""select('Green','{1}');"" title=""{1}"">
                                <i id=""IconGreen"" class=""{2}""></i>
                            </span>", this.user.MenuShortcuts.Green.Link, this.Dictionary[this.user.MenuShortcuts.Green.Label], this.user.MenuShortcuts.Green.Icon));
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Green"", ""Id"":{0}}},", this.user.MenuShortcuts.Green.Id));
            }
            else
            {
                big.Append(string.Format(@"
                            <span id=""ButtonGreen"" class=""btn btn-success"" style=""height:32px;"" onclick=""select('Green','{1}');"" title=""{1}"">
                                <i id=""IconGreen"" class=""{2}""></i>
                            </span>", string.Empty, string.Empty, string.Empty));
                this.userShortcuts.Append(@"{""Color"":""Green"", ""Id"":null},");
            }

            if (this.user.MenuShortcuts.Red != null && !string.IsNullOrEmpty(this.user.MenuShortcuts.Red.Label))
            {
                big.Append(string.Format(@"
                            <span id=""ButtonRed"" class=""btn btn-danger"" style=""height:32px;"" onclick=""select('Red','{1}');"" title=""{1}"">
                                <i id=""IconRed"" class=""{2}""></i>
                            </span>", this.user.MenuShortcuts.Red.Link, this.Dictionary[this.user.MenuShortcuts.Red.Label], this.user.MenuShortcuts.Red.Icon));
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Red"", ""Id"":{0}}},", this.user.MenuShortcuts.Red.Id));
            }
            else
            {
                big.Append(string.Format(@"
                            <span id=""ButtonRed"" class=""btn btn-danger"" style=""height:32px;"" onclick=""select('Red','{1}');"" title=""{1}"">
                                <i id=""IconRed"" class=""{2}""></i>
                            </span>", string.Empty, string.Empty, string.Empty));
                this.userShortcuts.Append(@"{""Color"":""Red"", ""Id"":null},");
            }

            if (this.user.MenuShortcuts.Yellow != null && !string.IsNullOrEmpty(this.user.MenuShortcuts.Yellow.Label))
            {
                big.Append(string.Format(@"
                            <span id=""ButtonYellow"" class=""btn btn-warning"" style=""height:32px;"" onclick=""select('Yellow','{1}');"" title=""{1}"">
                                <i id=""IconYellow"" class=""{2}""></i>
                            </span>", this.user.MenuShortcuts.Yellow.Link, this.Dictionary[this.user.MenuShortcuts.Yellow.Label], this.user.MenuShortcuts.Yellow.Icon));
                this.userShortcuts.Append(string.Format(@"{{""Color"":""Yellow"", ""Id"":{0}}}", this.user.MenuShortcuts.Yellow.Id));
            }
            else
            {
                big.Append(string.Format(@"
                            <span id=""ButtonYellow"" class=""btn btn-warning"" style=""height:32px;"" onclick=""select('Yellow','{1}');"" title=""{1}"">
                                <i id=""IconYellow"" class=""{2}""></i>
                            </span>", string.Empty, string.Empty, string.Empty));
                this.userShortcuts.Append(@"{""Color"":""Yellow"", ""Id"":null}");
            }
        }

        this.userShortcuts.Append("]");
        big.Append("</div>");
        small.Append("</div>");

        res.Append(big).Append(small);
        res.Append(@"</div>");

        this.LtMenuShortCuts.Text = res.ToString();

        this.LtIdiomas.Text = "<option value=\"es\"" + (this.user.Language == "es" ? " selected=\"selected\"" : string.Empty) + ">Castellano</option>";
        this.LtIdiomas.Text += "<option value=\"ca\"" + (this.user.Language == "ca" ? " selected=\"selected\"" : string.Empty) + ">Català</option>";
    }
}