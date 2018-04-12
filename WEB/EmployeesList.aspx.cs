// --------------------------------
// <copyright file="EmployeesList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class EmployeesList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    public string UserLogin { get { return this.user.UserName; } }

    public string EmployeesJson { get; private set; }

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

    public string Filter
    {
        get
        {
            if (this.Session["EmployeeFilter"] == null)
            {
                return "AI";
            }

            return this.Session["EmployeeFilter"].ToString().ToUpperInvariant();
        }
    }

    public UIDataHeader DataHeader { get; set; }

    public new string User
    {
        get
        {
            return this.user.Json;
        }
    }

    public string CompanyId
    {
        get
        {
            return this.company.Id.ToString();
        }
    }

    public string CompanyData
    {
        get
        {
            return Company.Json(this.company);
        }
    }

    public Company Company { get { return this.company; } }

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
        this.user = new ApplicationUser(Convert.ToInt32(Session["UserId"]));
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>; 
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        this.master.AddBreadCrumb("Item_Employees");
        this.master.Titulo = "Item_Employees";
        this.company = (Company)Session["Company"];
        this.RenderEmployeeData();

        if (this.user.HasGrantToWrite(ApplicationGrant.Employee))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Employee_Button_New", "EmployeesView.aspx");
        }

        this.DataHeader = new UIDataHeader() { Id = "ListDataHeader", ActionsItem = 2 }; ;
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Common_Name"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_JobPosition"] });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th2", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Departments"] });
    }

    private void RenderEmployeeData()
    {
        StringBuilder active = new StringBuilder();
        StringBuilder sea = new StringBuilder();
        bool first = true;
        int contData = 0;
        ReadOnlyCollection<Employee> employees = Employee.GetList(this.company.Id);
        foreach (Employee employee in employees)
        {
            if (employee.Active)
            {

                if (first)
                {
                    first = false;
                }
                else
                {
                    sea.Append(",");
                    active.Append(",");
                }

                if (employee.FullName.IndexOf("\"") != -1)
                {
                    sea.Append(string.Format(@"'{0}'", employee.FullName));
                }
                else
                {
                    sea.Append(string.Format(@"""{0}""", employee.FullName));
                }

                //active.Append(employee.ListRow(this.dictionary, this.user.Grants));
                active.Append(employee.JsonListRow(this.dictionary, this.user.Grants));
                contData++;
            }
        }

        //this.EmployeeData.Text = active.ToString();
        this.EmployeesJson = "[" + active + "]";
        this.master.SearcheableItems = sea.ToString();
    }
}