﻿// --------------------------------
// <copyright file="UserView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

/// <summary>
/// Implements user view page
/// </summary>
public partial class UserView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    public string Debug { get; private set; }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>Indicates if employee is active</summary>
    private bool active;

    private string countryData;

    private string returnScript;
    private int userItemId;
    private ApplicationUser userItem;

    private StringBuilder companyUserNames;

    public string Emails
    {
        get
        {
            StringBuilder res = new StringBuilder("[");
            ReadOnlyCollection<ApplicationUser> users = ApplicationUser.CompanyUsers(this.company.Id);
            bool first = true;
            foreach (ApplicationUser userItem in users)
            {
                if(first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.AppendFormat(CultureInfo.InvariantCulture, @"{{""UserId"":{0},""Email"":""{1}""}}", userItem.Id, userItem.Email);
            }

            res.Append("]");
            return res.ToString();
        }
    }

    /// <summary>
    /// Gets country data for icon combo
    /// </summary>
    public string CountryData
    {
        get
        {
            return this.countryData;
        }
    }

    /// <summary>
    /// Gets a value indicating whether company identifier
    /// </summary>
    public string CompanyId
    {
        get
        {
            return this.company.Id.ToString().Trim();
        }
    }


    public int UserItemId
    {
        get
        {
            return userItemId;
        }

        set
        {
            userItemId = value;
        }
    }

    public ApplicationUser UserItem
    {
        get
        {
            return this.userItem;
        }

        set
        {
            this.userItem = value;
        }
    }

    public string CompanyUserNames
    {
        get
        {
            return this.companyUserNames.ToString();
        }
    }

    /// <summary>
    /// Gets a value indicating whether if user show help in interface
    /// </summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public bool Active
    {
        get
        {
            return this.active;
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

    public string ReturnScript
    {
        get
        {
            return this.returnScript;
        }
    }

    private FormFooter formFooter;
    private FormFooter formFooterLearning;


    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    public string FormFooterLearning
    {
        get
        {
            return this.formFooterLearning.Render(this.dictionary);
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.active = true;
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
            {
                this.Response.Redirect("NoAccesible.aspx", true);
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
        this.TabTrazas.Visible = ((ApplicationUser)Session["user"]).Admin;

        if (this.Request.QueryString["id"] != null)
        {
            this.userItemId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'UsersList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.formFooter = new FormFooter();
        this.formFooterLearning = new FormFooter();

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string label = "Item_User";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Users", "UserList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = "Item_User";

        if (this.userItemId > 0)
        {
            //try
            //{
                this.userItem = ApplicationUser.GetById(this.userItemId, this.company.Id);
            //    this.Debug = string.Empty;
            //}
            //catch(Exception ex)
            //{
            //    this.userItem = ApplicationUser.Empty;
            //    this.Debug = GisoFramework.Tools.JsonCompliant(ex.Message);
            //}

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_User"], this.userItem.UserName);

            this.formFooter.ModifiedBy = string.Empty;
            this.formFooter.ModifiedOn = DateTime.Now.Date;

            string grants = "|";

            StringBuilder res = new StringBuilder();
            List<UserGrant> permisos = this.userItem.EffectiveGrants.OrderBy(o => o.Item.Description).ToList();
            foreach (UserGrant grant in permisos)
            {
                res.Append(grant.Render());
                if (grant.GrantToRead)
                {
                    grants += string.Format(CultureInfo.GetCultureInfo("en-us"), "R{0}|", grant.Item.Code);
                }
                if (grant.GrantToWrite)
                {
                    grants += string.Format(CultureInfo.GetCultureInfo("en-us"), "W{0}|", grant.Item.Code);
                }

                this.Grants.InnerText = grants;
            }

            this.LtGrantList.Text = res.ToString();
        }
        else
        {
            this.userItem = ApplicationUser.Empty;
            string grants = "|";
            StringBuilder res = new StringBuilder();
            List<UserGrant> permisos = this.user.EffectiveGrants.OrderBy(o => o.Item.Description).ToList();
            foreach (UserGrant grant in permisos)
            {
                grant.GrantToDelete = false;
                grant.GrantToRead = false;
                grant.GrantToWrite = false;
                res.Append(grant.Render());
                if (grant.GrantToRead)
                {
                    grants += string.Format(CultureInfo.GetCultureInfo("en-us"), "R{0}|", grant.Item.Code);
                }
                if (grant.GrantToWrite)
                {
                    grants += string.Format(CultureInfo.GetCultureInfo("en-us"), "W{0}|", grant.Item.Code);
                }

                this.Grants.InnerText = grants;
            }

            this.LtGrantList.Text = res.ToString();
        }

        this.companyUserNames = new StringBuilder();
        bool firstUserName = true;
        foreach (KeyValuePair<string, int> userName in ApplicationUser.CompanyUserNames(this.company.Id))
        {
            if (firstUserName)
            {
                firstUserName = false;
            }
            else
            {
                this.companyUserNames.Append(",");
            }

            this.companyUserNames.Append(string.Format(@"{{""UserName"":""{0}"",""UserId"":{1}}}", userName.Key, userName.Value));
        }

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        this.RenderCmbEmployeeData();
    }

    private void RenderCmbEmployeeData()
    {
        StringBuilder res = new StringBuilder();
        ReadOnlyCollection<Employee> employees = this.company.EmployessWithoutUser;
        List<Employee> employeesSorted = employees.ToList();

        bool hasEmployee = false;
        if (this.userItem.Id > 0)
        {
            if (this.userItem.Employee.Id > 0)
            {
                employeesSorted.Add(this.userItem.Employee);
                hasEmployee = true;
            }
        }

        if (!hasEmployee)
        {
            res.Append(@"<option value=""0"">").Append(this.Dictionary["Common_SelectOne"]).Append("</option>");
        }
        else
        {
            res.Append(@"<option value=""0"">").Append(this.Dictionary["Common_SelectOne"]).Append("</option>");
        }

        employeesSorted = employeesSorted.OrderBy(e => e.FullName).ToList();
        foreach (Employee employee in employeesSorted)
        {
            res.AppendFormat(
                CultureInfo.GetCultureInfo("en-us"),
                @"<option value=""{0}""{2}>{1}</option>",
                employee.Id,
                employee.FullName,
                employee.Id == this.userItem.Employee.Id ? " selected=\"selected\"" : string.Empty);
        }

        this.CmbEmployeeData.Text = res.ToString();
    }
}