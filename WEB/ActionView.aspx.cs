﻿// --------------------------------
// <copyright file="actionview.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using System.Collections.ObjectModel;
using System.Text;
using SbrinnaCoreFramework;
using System.IO;

public partial class ActionView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private bool grantToWrite;

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

    public long IncidentActionId { get; set; }
    public IncidentAction IncidentAction { get; set; }
    public Incident Incident { get; set; }
    public BusinessRisk BusinessRisk { get; set; }

    TabBar tabBar = new TabBar() { Id = "IncidentActionTabBar" };

    public BarPopup ProviderBarPopups { get; set; }
    public BarPopup CustomerBarPopups { get; set; }

    public string IncidentActionCosts
    {
        get
        {
            if (this.IncidentActionId == 0)
            {
                return "[]";
            }

            return IncidentActionCost.GetByIncidentAction(this.IncidentActionId, this.company.Id);
        }
    }

    public string CompanyIncidentActionCosts
    {
        get
        {
            if (this.IncidentActionId == 0)
            {
                return "[]";
            }

            return IncidentActionCost.GetByCompany(this.company.Id);
        }
    }

    #region Form
    public FormText TxtDescription { get; set; }
    public FormTextArea TxtWhatHappened { get; set; }
    public FormTextArea TxtCauses { get; set; }
    public FormTextArea TxtActions { get; set; }
    public FormTextArea TxtMonitoring { get; set; }
    public FormTextArea TxtNotes { get; set; }
    public FormSelect CmbWhatHappenedResponsible { get; set; }
    public FormSelect CmbCausesResponsible { get; set; }
    public FormSelect CmbActionsResponsible { get; set; }
    public FormSelect CmbActionsExecuter { get; set; }
    public FormSelect CmbClosedResponsible { get; set; }
    public FormSelect CmbClosedExecutor { get; set; }
    public FormSelect CmbReporterDepartment { get; set; }
    public FormSelect CmbReporterCustomer { get; set; }
    public FormSelect CmbReporterProvider { get; set; }
    public FormDatePicker WhatHappenedDate { get; set; }
    public FormDatePicker CausesDate { get; set; }
    public FormDatePicker ActionsDate { get; set; }
    public FormDatePicker ActionsSchedule { get; set; }
    public FormDatePicker ClosedDate { get; set; }
    public FormDatePicker ClosedExecutorDate { get; set; }
    #endregion

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    /// <summary>
    /// Indicates if employee is active
    /// </summary>
    private bool active;

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

    public bool IsAdmin
    {
        get
        {
            return this.user.Admin;
        }
    }

    public bool Active
    {
        get
        {
            return this.active;
        }
    }

    private string returnScript;
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

    public string ReturnScript
    {
        get
        {
            return this.returnScript;
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

    public string DepartmentsJson
    {
        get
        {
            return Department.GetByCompanyJsonList(this.company.Id);
        }
    }

    public string ProvidersJson
    {
        get
        {
            return Provider.GetByCompanyJson(this.company.Id);
        }
    }

    public string CustomersJson
    {
        get
        {
            return Customer.GetByCompanyJson(this.company.Id);
        }
    }

    public string CostsJson
    {
        get
        {
            return CostDefinition.GetByCompanyJson(this.company.Id);
        }
    }

    public string EmployeesJson
    {
        get
        {
            return Employee.GetByCompanyJson(this.company.Id);
        }
    }

    public string GrantToWrite
    {
        get
        {
            return this.grantToWrite ? "true" : "false";
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
        this.Incident = Incident.Empty;
        this.BusinessRisk = BusinessRisk.Empty;

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
        if (this.Request.QueryString["id"] != null)
        {
            this.IncidentActionId = Convert.ToInt64(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'ActionsList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_IncidentActions", "ActionList.aspx", false);
        this.master.AddBreadCrumb("Item_IncidentActions_Detail");
        this.grantToWrite = this.user.HasGrantToWrite(ApplicationGrant.IncidentActions);
        this.Incident = Incident.Empty;

        if (this.IncidentActionId > 0)
        {
            this.IncidentAction = IncidentAction.GetById(this.IncidentActionId, this.company.Id);
            if (this.IncidentAction.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.IncidentAction = IncidentAction.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_IncidentAction"], this.IncidentAction.Description);

            this.formFooter = new FormFooter();
            this.formFooter.ModifiedBy = this.IncidentAction.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.IncidentAction.ModifiedOn;
            this.formFooter.AddButton(new UIButton() { Id = "BtnPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"], ColumnsSpan = 12 });

            this.master.ItemCode = this.IncidentAction.Description;

            if (this.IncidentAction.IncidentId > 0)
            {
                this.Incident = Incident.GetById(this.IncidentAction.IncidentId, this.company.Id);
            }

            if(this.IncidentAction.BusinessRiskId>0)
            {
                this.BusinessRisk = BusinessRisk.GetById(this.company.Id, this.IncidentAction.BusinessRiskId);
            }

            this.RenderDocuments();
        }
        else
        {
            this.master.Titulo = "Item_IncidentActions_New_Label";
            this.IncidentAction = IncidentAction.Empty;
            /*this.formFooter.ModifiedBy = this.dictionary["Nuevo"];
            this.formFooter.ModifiedOn = DateTime.Now;
            this.formFooter.AddButton(Button.FormSaveButton);
            this.formFooter.AddButton(Button.FormCancelButton);*/
            this.formFooter = new FormFooter(this.dictionary, this.grantToWrite);
        }

        this.tabBar.AddTab(new Tab() { Id = "home", Selected = true, Active = true, Label = this.dictionary["Item_IncidentAction_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab() { Id = "costes", Available = this.user.HasGrantToRead(ApplicationGrant.Cost) && this.IncidentActionId > 0, Active = this.IncidentActionId > 0, Label = this.dictionary["Item_IncidentAction_Tab_Costs"] });
        this.tabBar.AddTab(new Tab() { Id = "uploadFiles", Available = true, Active = this.IncidentActionId > 0, Hidden = this.IncidentActionId < 1, Label = this.Dictionary["Item_IncidentAction_Tab_UploadFiles"] });
        // this.tabBar.AddTab(new Tab() { Id = "trazas", Available = this.user.HasTraceGrant() && this.IncidentActionId > 0, Active = this.IncidentActionId > 0, Label = this.dictionary["Item_IncidentAction_Tab_Traces"] });

        this.RenderForm();
        
        this.ProviderBarPopups = new BarPopup()
        {
            Id = "Provider",
            DeleteMessage = this.dictionary["Common_DeleteMessage"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
            Description = "Proveedor",
            FieldName = this.dictionary["Common_Name"],
            BarTitle = this.Dictionary["Item_Providers"]
        };

        this.CustomerBarPopups = new BarPopup()
        {
            Id = "Customer",
            DeleteMessage = this.dictionary["Common_DeleteMessage"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
            Description = "Cliente",
            FieldName = this.dictionary["Common_Name"],
            BarTitle = this.Dictionary["Item_Customers"]
        };
    }

    public void RenderForm()
    {
        this.TxtWhatHappened = new FormTextArea() { TitleLabel=true,  Rows = 3, Value = this.IncidentAction.WhatHappened, Name = "TxtWhatHappened", Label = this.dictionary["Item_IncidentAction_Field_WhatHappened"], ColumnsSpan = 8, ColumnsSpanLabel = 12,MaxLength=500, Embedded = true };
        this.TxtCauses = new FormTextArea() { TitleLabel = true, Rows = 3, Value = this.IncidentAction.Causes, Name = "TxtCauses", Label = this.dictionary["Item_IncidentAction_Field_Causes"], ColumnsSpan = 8, ColumnsSpanLabel = 12, MaxLength = 500, Embedded = true };
        this.TxtActions = new FormTextArea() { TitleLabel = true, Rows = 3, Value = this.IncidentAction.Actions, Name = "TxtActions", Label = this.dictionary["Item_IncidentAction_Field_Actions"], ColumnsSpan = 8, ColumnsSpanLabel = 12, MaxLength = 500, Embedded = true };
        this.TxtMonitoring = new FormTextArea() { TitleLabel = true, Rows = 3, Value = this.IncidentAction.Monitoring, Name = "TxtMonitoring", Label = this.dictionary["Item_IncidentAction_Field_Monitoring"], ColumnsSpan = 12, ColumnsSpanLabel = 12, MaxLength = 500, Embedded = true };
        this.TxtNotes = new FormTextArea() { TitleLabel = true, Rows = 3, Value = this.IncidentAction.Notes, Name = "TxtNotes", Label = this.dictionary["Item_IncidentAction_Field_Notes"], ColumnsSpan = 12, ColumnsSpanLabel = 12, MaxLength = 500, Embedded = true };

        this.CmbWhatHappenedResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleWhatHappend"],
            ColumnsSpan = 8,
            Name = "CmbWhatHappenedResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbCausesResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleCauses"],
            ColumnsSpan = 8,
            Name = "CmbCausesResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionsResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleActions"],
            ColumnsSpan = 8,
            Name = "CmbActionsResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionsExecuter = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_Executer"],
            ColumnsSpan = 8,
            Name = "CmbActionsExecuter",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbClosedResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 2,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleClose"],
            ColumnsSpan = 3,
            Name = "CmbClosedResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbClosedExecutor = new FormSelect()
        {
            ColumnsSpanLabel = 2,
            Label = this.dictionary["Item_IncidentAction_Field_Executer"],
            ColumnsSpan = 3,
            Name = "CmbClosedExecutor",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        long WhatHappenrdResponsibleId = this.IncidentAction.WhatHappenedBy == null ? 0 : this.IncidentAction.WhatHappenedBy.Id;
        long CausesResponsibleId = this.IncidentAction.CausesBy == null ? 0 : this.IncidentAction.CausesBy.Id;
        long ActionsResponsibleId = this.IncidentAction.ActionsBy == null ? 0 : this.IncidentAction.ActionsBy.Id;
        long ActionsExecuterId = this.IncidentAction.ActionsExecuter == null ? 0 : this.IncidentAction.ActionsExecuter.Id;
        long ClosedResponsibleId = this.IncidentAction.ClosedBy == null ? 0 : this.IncidentAction.ClosedBy.Id;
        long ClosedExecutorId = this.IncidentAction.ClosedExecutor == null ? 0 : this.IncidentAction.ClosedExecutor.Id;

        foreach (Employee e in this.company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbWhatHappenedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == WhatHappenrdResponsibleId });
                this.CmbCausesResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == CausesResponsibleId });
                this.CmbActionsResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == ActionsResponsibleId });
                this.CmbActionsExecuter.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == ActionsExecuterId });
                this.CmbClosedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == ClosedResponsibleId });
            }
            else
            {
                if (this.IncidentAction.WhatHappenedBy != null)
                {
                    if (e.Id == this.IncidentAction.WhatHappenedBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbWhatHappenedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.CausesBy != null)
                {
                    if (e.Id == this.IncidentAction.CausesBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbCausesResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsBy != null)
                {
                    if (e.Id == this.IncidentAction.ActionsBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionsResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsExecuter != null)
                {
                    if (e.Id == this.IncidentAction.ActionsExecuter.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionsExecuter.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ClosedBy != null)
                {
                    if (e.Id == this.IncidentAction.ClosedBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbClosedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }
            }
        }

        this.CmbReporterDepartment = new FormSelect()
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType1",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbReporterProvider = new FormSelect()
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType2",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbReporterCustomer = new FormSelect()
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType3",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.TxtDescription = new FormText()
        {
            ColumnSpan = 10,
            ColumnSpanLabel = 2,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_IncidentAction_Label_Description"],
            MaximumLength = 100,
            Name = "TxtDescription",
            Placeholder = this.Dictionary["Item_IncidentAction_Label_Description"],
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            Value = this.IncidentAction.Description

        };

        this.WhatHappenedDate = new FormDatePicker()
        {
            Id = "TxtWhatHappenedDate",
            Label = this.dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.WhatHappenedOn
        };

        this.CausesDate = new FormDatePicker()
        {
            Id = "TxtCausesDate",
            Label = this.dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.CausesOn
        };

        this.ActionsDate = new FormDatePicker()
        {
            Id = "TxtActionsDate",
            Label = this.dictionary["Common_DateExecution"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.ActionsOn
        };

        this.ActionsSchedule = new FormDatePicker()
        {
            Id = "TxtActionsSchedule",
            Label = this.dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.ActionsSchedule
        };

        this.ClosedDate = new FormDatePicker()
        {
            Id = "TxtClosedDate",
            Label = this.dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = 2,
            ColumnsSpan = 2,
            Value = this.IncidentAction.ClosedOn
        };

        this.ClosedExecutorDate = new FormDatePicker()
        {
            Id = "TxtClosedExecutorDate",
            Label = this.dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = 2,
            ColumnsSpan = 2,
            Value = this.IncidentAction.ClosedExecutorOn
        };
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        ReadOnlyCollection<UploadFile> files = UploadFile.GetByItem(13, this.IncidentActionId, this.company.Id);
        StringBuilder res = new StringBuilder();
        StringBuilder resList = new StringBuilder();
        int contCells = 0;
        ReadOnlyCollection<string> extensions = ToolsFile.ExtensionToShow;
        foreach (UploadFile file in files)
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