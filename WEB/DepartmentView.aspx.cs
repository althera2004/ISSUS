﻿// --------------------------------
// <copyright file="DepartmentView.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class DepartmentView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    private long departmentId;
    private Department department;
    private FormFooter formFooter;

    private TabBar tabBar = new TabBar { Id = "DepartmentTabBar" };

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string TxtName
    {
        get
        {
            return new FormText
            {
                Name = "TxtName",
                Value = this.department.Description,
                ColumnSpan = 11,
                Placeholder = this.Dictionary["Item_Department"],
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Department),
                MaximumLength = Constant.DefaultDatabaseVarChar
            }.Render;
        }
    }

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
        }
    }

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    public string Departments
    {
        get
        {
            var res = new StringBuilder();
            bool first = true;
            foreach (var department in Department.ByCompany(((Company)Session["Company"]).Id))
            {
                if (!department.Deleted)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(department.JsonKeyValue);
                }
            }

            return res.ToString();
        }
    }

    public long DepartmentId
    {
        get
        {
            return this.departmentId;
        }
    }

    public Department Department
    {
        get
        {
            return this.department;
        }
    }

    public Company Company
    {
        get
        {
            return this.company;
        }
    }

    public Dictionary<string, string> Dictionary { get; private set; }

    public new ApplicationUser User
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

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!int.TryParse(this.Request.QueryString["id"], out test))
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.company = this.Session["company"] as Company;
        this.Dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.user = this.Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.departmentId = Convert.ToInt64(this.Request.QueryString["id"]);
        }

        string label = "Item_Department_Title_DepartmentDetails";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Departments", "DepartmentsList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;

        if (!this.Page.IsPostBack)
        {
            this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.departmentId, TargetType.Department);
        }

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        if (this.departmentId != -1)
        {
            this.department = new Department(this.departmentId, this.company.Id);
            if (this.department.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.department = Department.Empty;
            }

            this.formFooter.ModifiedBy = this.department.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.department.ModifiedOn;
            var tableEmployees = new StringBuilder();
            foreach (var employee in this.department.Employees)
            {
                tableEmployees.Append(employee.DepartmentListRow(this.Dictionary, this.departmentId));
            }

            this.TableEmployees.Text = tableEmployees.ToString();

            var tableJobPosition = new StringBuilder();
            foreach (var jobPosition in this.department.JobPositions)
            {
                tableJobPosition.Append(jobPosition.EmployeeRow(this.Dictionary));
            }

            this.TableJobPosition.Text = tableJobPosition.ToString();
            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_Department"], this.department.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.department = Department.Empty;
            this.TableEmployees.Text = string.Empty;
            this.TableJobPosition.Text = string.Empty;
        }

        this.tabBar.AddTab(new Tab { Id = "home", Available = true, Active = true, Selected = true, Label = this.Dictionary["Item_Department_Tab_Principal"] });
        // this.tabBar.AddTab(new Tab() { Id = "trazas", Label = this.dictionary["Item_Department_Tab_Traces"], Active = this.departmentId > 0, Available = this.user.HasTraceGrant() });       
    }
}