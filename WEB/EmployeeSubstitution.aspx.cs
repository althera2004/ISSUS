// --------------------------------
// <copyright file="EmployeeSubstitution.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class EmployeeSubstitution : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    public string Action { get; private set; }

    public string Employees
    {
        get
        {
            return Employee.CompanyListJson(this.company.Id);
        }
    }

    public string UserLanguage
    {
        get
        {
            return this.user.Language;
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

    public string EndDate { get; set; }

    /// <summary>
    /// Gets or sets if user show help in interface
    /// </summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string CompanyId
    {
        get
        {
            return this.company.Id.ToString();
        }
    }

    private FormFooter formFooter;

    public string FormFooter
    {
        get
        {
            if (this.formFooter == null)
            {
                return string.Empty;
            }

            return this.formFooter.Render(this.Dictionary);
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


    public FormDatePicker TxtEndDate { get; set; }
    TabBar tabBar = new TabBar() { Id = "EmployeeTabBar" };

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    public Employee Employee { get; private set; }

    /// <summary>Gets dictionary for fixed labels
    public Dictionary<string, string> Dictionary { get; private set; }

    public int EmployeeId { get; private set; }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.Action = "Baja";
        if (this.Request.QueryString["id"] != null)
        {
            this.EmployeeId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["enddate"] != null)
        {
            this.EndDate = this.Request.QueryString["enddate"].ToString().Trim();
        }

        if (this.Request.QueryString["action"] != null)
        {
            var actionData = this.Request.QueryString["action"].ToString().Trim();
			if(actionData == "delete") 
			{
				this.Action = "delete";
			}
        }

        this.user = Session["User"] as ApplicationUser;
        this.company = Session["company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string label = "Item_Employee_Title_Delete";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Employees", "EmployeesList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = this.Dictionary["Item_Employee"];
        this.formFooter = new FormFooter();

        if (this.EmployeeId > 0)
        {
            this.Employee = new Employee(this.EmployeeId, true);
            if (this.Employee.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_Employee"], this.Employee.FullName);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
            this.tabBar.AddTab(new Tab { Id = "home", Selected = true, Active = true, Label = this.Dictionary["Item_Employee_Tab_Delete"], Available = true });

            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ban-circle", Text = this.Dictionary["Item_Employee_Btn_Inactive"], Action = "danger" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });
            this.formFooter.ModifiedBy = this.Employee.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Employee.ModifiedOn;

            this.TxtEndDate = new FormDatePicker()
            {
                Id = "TxtEndDate",
                Label = this.Dictionary["Item_Employee_FieldLabel_InactiveDate"],
                ColumnsSpanLabel = Constant.ColumnSpan2,
                ColumnsSpan = Constant.ColumnSpan2,
                Required = true,
                Value = DateTime.Now
            };
        }
    }
}