// --------------------------------
// <copyright file="CargosView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using GisoFramework.Item;
using GisoFramework.Activity;
using GisoFramework;
using GisoFramework.DataAccess;
using System.Collections.ObjectModel;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class CargosView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>
    /// Job position identifier
    /// </summary>
    private int jobPositionId;

    /// <summary>
    /// Job position
    /// </summary>
    private JobPosition cargo;

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

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

    private FormFooter formFooter;

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    public JobPosition Cargo { get { return this.cargo; } }

    public string TxtName
    {
        get
        {
            return new SbrinnaCoreFramework.UI.FormText()
            {
                Name = "TxtName",
                Value = this.cargo.Description,
                ColumnSpan = 10,
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                MaximumLength = 100,
                Placeholder = this.dictionary["Item_JobPosition"],
                GrantToWrite = this.GrantToWrite
            }.Render;
        }
    }

    public string BarDepartment
    {
        get
        {
            return new SbrinnaCoreFramework.UI.FormBar()
            {
                Name = "Department",
                ValueName = "TxtDepartmentName",
                Value = this.cargo.Department.Description,
                ButtonBar = "BtnDepartment",
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                ColumnSpan = 8,
                BarToolTip = this.Dictionary["Item_JobPosition_Button_DepartmentsBAR"],
                GrantToWrite = this.GrantToWrite,
                GrantToEdit = this.user.HasGrantToWrite(ApplicationGrant.Department)
            }.Render;
        }
    }

    public string CmbResponsible
    {
        get
        {
            ReadOnlyCollection<JobPosition> jobPositions = JobPosition.JobsPositionByCompany(this.company.Id);
            FormSelect select = new FormSelect()
            {
                Name = "CmbResponsible",
                ColumnsSpan = 4,
                GrantToWrite = this.GrantToWrite,
                ChangeEvent = "SelectedResponsible = this.value;",
                Placeholder = this.dictionary["Common_Responsible"],
                Required = false,
                ToolTip = "Item_JobPosition_Help_Responsible",
                Value = this.cargo.Responsible != null ? this.cargo.Responsible.Description : string.Empty,
                DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectOne"] }
            };

            foreach (JobPosition jobPosition in jobPositions)
            {
                bool selected = false;
                if (this.cargo.Id != jobPosition.Id)
                {
                    bool bucle = IsBucle(this.cargo.Id, jobPosition.Id, jobPositions);
                    if (!bucle)
                    {
                        if (this.cargo.Responsible != null && this.cargo.Responsible.Id == jobPosition.Id)
                        {
                            selected = true;
                        }

                        select.AddOption(new FormSelectOption() { Selected = selected, Value = jobPosition.Id.ToString(), Text = jobPosition.Description });
                    }
                }
            }

            return select.Render;
        }
    }

    public bool GrantTraces
    {
        get
        {
            return this.user.HasTraceGrant();
        }
    }

    public bool GrantToWrite
    {
        get
        {
            return this.user.HasGrantToWrite(ApplicationGrant.JobPosition);
        }
    }

    public bool NewJobPosition
    {
        get
        {
            return this.jobPositionId < 1;
        }
    }

    public bool Admin
    {
        get
        {
            return this.user.Admin;
        }
    }

    /// <summary>
    /// Gets a Json structure of job position
    /// </summary>
    public string CargoJson
    {
        get
        {
            return this.cargo.Json;
        }
    }

    /// <summary>
    /// Gets the dictionary for interface texts
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
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
        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        
        // Security access control
        if (!this.user.HasGrantToRead(ApplicationGrant.JobPosition))
        {
            this.Response.Redirect("NoPrivileges.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Parameters control
        if (this.Request.QueryString["id"] != null)
        {
            this.jobPositionId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        
        this.formFooter = new FormFooter();
        if (this.user.HasGrantToWrite(ApplicationGrant.JobPosition))
        {
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
        }

        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (jobPositionId > 0)
        {
            this.cargo = new JobPosition(this.jobPositionId, this.company.Id);
            if (this.cargo.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }

            this.formFooter.ModifiedBy = this.cargo.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.cargo.ModifiedOn;

            this.RenderEmployees();
            this.master.TitleInvariant = true;
        }
        else
        {
            this.cargo = JobPosition.Empty;
            this.TableEmployees.Text = string.Empty;
        }

        if (!IsPostBack)
        {
            if (this.user.HasTraceGrant())
            {
                this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.jobPositionId, TargetType.JobPosition);
            }
        }

        string label = this.jobPositionId == -1 ? "Item_JobPosition_BreadCrumb_Edit" : string.Format("{0}: <strong>{1}</strong>", this.dictionary["Item_JobPosition"], this.cargo.Description);
        this.master.AddBreadCrumb("Item_JobPositions", "CargosList.aspx", false);
        this.master.AddBreadCrumb("Item_JobPosition_BreadCrumb_Edit");
        this.master.Titulo = label;
    }

    private void RenderEmployees()
    {
        StringBuilder res = new StringBuilder();        
        foreach (Employee employee in this.cargo.Employees)
        {
            res.Append(employee.JobPositionListRow);
        }

        this.TableEmployees.Text = res.ToString();
    }

    private bool IsBucle(long actualId, long id, ReadOnlyCollection<JobPosition> jobPositions)
    {
        JobPosition parent = GetById(id, jobPositions);
        if (parent == null)
        {
            return false;
        }

        if (parent.Id == actualId)
        {
            return true;
        }

        if (parent.Responsible == null)
        {
            return false;
        }

        return IsBucle(actualId, parent.Responsible.Id, jobPositions);
    }

    private JobPosition GetById(long id, ReadOnlyCollection<JobPosition> jobPositions)
    {
        foreach(JobPosition jobPosition in jobPositions)
        {
            if(jobPosition.Id == id)
            {
                return jobPosition;
            }
        }

        return null;
    }
}