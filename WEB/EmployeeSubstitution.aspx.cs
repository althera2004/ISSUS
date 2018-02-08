// --------------------------------
// <copyright file="EmployeeSubstitution.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
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

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Indicates if employee is active</summary>
    private bool active;

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

            return this.formFooter.Render(this.dictionary);
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


    public FormDatePicker TxtEndDate { get; set; }
    TabBar tabBar = new TabBar() { Id = "EmployeeTabBar" };

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    private int employeeId;
    private Employee employee;

    public Employee Employee
    {
        get
        {
            return this.employee;
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

    public int EmployeeId
    {
        get
        {
            return this.employeeId;
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        if (this.Request.QueryString["id"] != null)
        {
            this.employeeId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["enddate"] != null)
        {
            this.EndDate = this.Request.QueryString["enddate"].ToString().Trim();
        }

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string label = "Item_Employee_Title_Delete";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Employees", "EmployeesList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = this.dictionary["Item_Employee"];
        this.formFooter = new FormFooter();

        if (employeeId > 0)
        {
            this.employee = new Employee(this.employeeId, true);
            if (this.employee.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }

            if (this.employee.DisabledDate.HasValue)
            {
                this.active = false;
            }


            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Employee"], employee.FullName);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
            this.tabBar.AddTab(new Tab() { Id = "home", Selected = true, Active = true, Label = this.dictionary["Item_Employee_Tab_Delete"], Available = true });


            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ban-circle", Text = this.dictionary["Item_Employee_Btn_Inactive"], Action = "danger" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
            this.formFooter.ModifiedBy = this.employee.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.employee.ModifiedOn;

            this.TxtEndDate = new FormDatePicker()
            {
                Id = "TxtEndDate",
                Label = this.dictionary["Item_Employee_FieldLabel_InactiveDate"],
                ColumnsSpanLabel = 2,
                ColumnsSpan = 2,
                Required = true,
                Value = DateTime.Now
            };
        }
    }
}