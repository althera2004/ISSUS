// --------------------------------
// <copyright file="CargosView.aspx.cs" company="Sbrinna">
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
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;
using System.Globalization;
using System.IO;

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

    /// <summary>Job position identifier</summary>
    private int jobPositionId;

    /// <summary>Job position</summary>
    private JobPosition cargo;

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
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
            return new FormText
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
            return new FormBar
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
            var jobPositions = JobPosition.JobsPositionByCompany(this.company.Id);
            var select = new FormSelect
            {
                Name = "CmbResponsible",
                ColumnsSpan = Constant.ColumnSpan4,
                GrantToWrite = this.GrantToWrite,
                ChangeEvent = "SelectedResponsible = this.value;",
                Placeholder = this.dictionary["Common_Responsible"],
                Required = false,
                ToolTip = "Item_JobPosition_Help_Responsible",
                Value = this.cargo.Responsible != null ? this.cargo.Responsible.Description : string.Empty,
                DefaultOption = new FormSelectOption { Text = this.dictionary["Common_SelectOne"] }
            };

            foreach (var jobPosition in jobPositions)
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

                        select.AddOption(new FormSelectOption { Selected = selected, Value = jobPosition.Id.ToString(), Text = jobPosition.Description });
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

    /// <summary>Gets a Json structure of job position</summary>
    public string CargoJson
    {
        get
        {
            return this.cargo.Json;
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
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
            }
            else if (!int.TryParse(this.Request.QueryString["id"], out test))
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
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
        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        
        // Security access control
        if (!this.user.HasGrantToRead(ApplicationGrant.JobPosition))
        {
            this.Response.Redirect("NoPrivileges.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Parameters control
        if (this.Request.QueryString["id"] != null)
        {
            this.jobPositionId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        
        this.formFooter = new FormFooter();
        if (this.user.HasGrantToWrite(ApplicationGrant.JobPosition))
        {
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
        }

        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (jobPositionId > 0)
        {
            this.cargo = new JobPosition(this.jobPositionId, this.company.Id);
            if (this.cargo.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }

            this.formFooter.ModifiedBy = this.cargo.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.cargo.ModifiedOn;

            this.RenderEmployees();
            this.master.TitleInvariant = true;
            this.RenderDocuments();
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
        this.master.AddBreadCrumb("Item_JobPositions", "CargosList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb("Item_JobPosition_BreadCrumb_Edit");
        this.master.Titulo = label;
    }

    private void RenderEmployees()
    {
        var res = new StringBuilder();        
        foreach (var employee in this.cargo.Employees)
        {
            res.Append(employee.JobPositionListRow);
        }

        this.TableEmployees.Text = res.ToString();
    }

    private bool IsBucle(long actualId, long id, ReadOnlyCollection<JobPosition> jobPositions)
    {
        var parent = GetById(id, jobPositions);
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
        foreach(var jobPosition in jobPositions)
        {
            if(jobPosition.Id == id)
            {
                return jobPosition;
            }
        }

        return null;
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(3, this.jobPositionId, this.company.Id);
        var res = new StringBuilder();
        var resList = new StringBuilder();
        int contCells = 0;
        var extensions = ToolsFile.ExtensionToShow;
        foreach (var file in files)
        {
            decimal finalSize = ToolsFile.FormatSize((decimal)file.Size);
            string fileShowed = string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description;
            if (fileShowed.Length > 15)
            {
                fileShowed = fileShowed.Substring(0, 15) + "...";
            }

            string viewButton = string.Format(
                CultureInfo.InvariantCulture,
                @"<div class=""col-sm-2 btn-success"" onclick=""ShowPDF('{0}');""><i class=""icon-eye-open bigger-120""></i></div>",
                file.FileName
                );

            string listViewButton = string.Format(
                CultureInfo.InvariantCulture,
                @"<span class=""btn btn-xs btn-success"" onclick=""ShowPDF('{0}');"">
                            <i class=""icon-eye-open bigger-120""></i>
                        </span>",
                file.FileName);

            var fileExtension = Path.GetExtension(file.FileName);

            if (!extensions.Contains(fileExtension))
            {
                viewButton = "<div class=\"col-sm-2\">&nbsp;</div>";
                listViewButton = "<span style=\"margin-left:30px;\">&nbsp;</span>";
            }

            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<div id=""{0}"" class=""col-sm-3 document-container"">
                        <div class=""col-sm-6"">&nbsp</div>
                        {10}
                        <div class=""col-sm-2 btn-info""><a class=""icon-download bigger-120"" href=""/DOCS/{3}/{4}"" target=""_blank"" style=""color:#fff;""></a></div>
                        <div class=""col-sm-2 btn-danger"" onclick=""DeleteUploadFile({0},'{1}');""><i class=""icon-trash bigger-120""></i></div>
                        <div class=""col-sm-12 iconfile"" style=""max-width: 100%;"">
                            <div class=""col-sm-4""><img src=""/images/FileIcons/{2}.png"" /></div>
                            <div class=""col-sm-8 document-name"">
                                <strong title=""{1}"">{9}</strong><br />
                                {7}: {5:dd/MM/yyyy}
                                {8}: {6:#,##0.00} MB
                            </div>
                        </div>
                    </div>",
                    file.Id,
                    string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description,
                    file.Extension,
                    this.company.Id,
                    file.FileName,
                    file.CreatedOn,
                    finalSize,
                    this.Dictionary["Item_Attachment_Header_CreateDate"],
                    this.dictionary["Item_Attachment_Header_Size"],
                    fileShowed,
                    viewButton);

            resList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr id=""tr{2}"">
                    <td>{1}</td>
                    <td align=""center"" style=""width:90px;"">{4:dd/MM/yyyy}</td>
                    <td align=""right"" style=""width:120px;"">{5:#,##0.00} MB</td>
                    <td style=""width:150px;"">
                        {6}
                        <span class=""btn btn-xs btn-info"">
                            <a class=""icon-download bigger-120"" href=""/DOCS/{3}/{0}"" target=""_blank"" style=""color:#fff;""></a>
                        </span>
                        <span class=""btn btn-xs btn-danger"" onclick=""DeleteUploadFile({2},'{1}');"">
                            <i class=""icon-trash bigger-120""></i>
                        </span>
                    </td>
                </tr>",
                file.FileName,
                string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description,
                file.Id,
                this.company.Id,
                file.CreatedOn,
                finalSize,
                listViewButton);

            contCells++;
            if (contCells == 4)
            {
                contCells = 0;
                res.Append("<div style=\"clear:both\">&nbsp;</div>");
            }
        }

        this.LtDocuments.Text = res.ToString();
        this.LtDocumentsList.Text = resList.ToString();
    }
}