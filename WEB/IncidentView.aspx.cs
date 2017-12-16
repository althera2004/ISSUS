// --------------------------------
// <copyright file="IncidentView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

/// <summary>
/// Implements incident view page
/// </summary>
public partial class IncidentView : Page
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

    public long IncidentId { get; set; }

    public Incident Incident { get; set; }

    public IncidentAction IncidentAction { get; set; }

    private TabBar tabBar = new TabBar() { Id = "IncidentTabBar" };

    public BarPopup ProviderBarPopups { get; set; }

    public BarPopup CustomerBarPopups { get; set; }

    public string IncidentCosts
    {
        get
        {
            if (this.IncidentId == 0)
            {
                return "[]";
            }

            return IncidentCost.GetByIncident(this.IncidentId, this.company.Id);
        }
    }

    public string CostsJson
    {
        get
        {
            return CostDefinition.GetByCompanyJson(this.company.Id);
        }
    }

    public string CompanyIncidentCosts
    {
        get
        {
            if (this.IncidentId == 0)
            {
                return "[]";
            }

            return IncidentCost.GetByCompany(this.company.Id);
        }
    }

    public int IncidentStatus
    {
        get
        {
            if (this.IncidentId < 1)
            {
                return 0;
            }

            if (this.Incident.ClosedOn.HasValue)
            {
                return 4;
            }

            if (this.Incident.ActionsOn.HasValue)
            {
                return 3;
            }

            if (this.Incident.CausesOn.HasValue)
            {
                return 2;
            }

            if (this.Incident.WhatHappenedOn.HasValue)
            {
                return 1;
            }

            return 0;
        }
    }

    public int IncidentActionStatus
    {
        get
        {
            if (this.IncidentAction.Id < 1)
            {
                return 0;
            }

            if (this.IncidentAction.ClosedOn.HasValue)
            {
                return 4;
            }

            if (this.IncidentAction.ActionsOn.HasValue)
            {
                return 3;
            }

            if (this.IncidentAction.CausesOn.HasValue)
            {
                return 2;
            }

            if (this.IncidentAction.WhatHappenedOn.HasValue)
            {
                return 1;
            }

            return 0;
        }
    }

    #region IncidentForm
    public FormText TxtDescription { get; set; }
    public FormTextArea TxtWhatHappened { get; set; }
    public FormTextArea TxtCauses { get; set; }
    public FormTextArea TxtActions { get; set; }
    public FormTextArea TxtMonitoring { get; set; }
    public FormTextArea TxtAnotations { get; set; }
    public FormTextArea TxtNotes { get; set; }
    public FormSelect CmbWhatHappenedResponsible { get; set; }
    public FormSelect CmbCausesResponsible { get; set; }
    public FormSelect CmbActionsResponsible { get; set; }
    public FormSelect CmbActionsExecuter { get; set; }
    public FormSelect CmbClosedResponsible { get; set; }
    public FormSelect CmbReporterDepartment { get; set; }
    public FormSelect CmbReporterCustomer { get; set; }
    public FormSelect CmbReporterProvider { get; set; }
    public FormDatePicker WhatHappenedDate { get; set; }
    public FormDatePicker CausesDate { get; set; }
    public FormDatePicker ActionsDate { get; set; }
    public FormDatePicker ActionsSchedule { get; set; }
    public FormDatePicker ClosedDate { get; set; }
    #endregion

    #region ActionForm
    public FormText TxtActionDescription { get; set; }
    public FormTextArea TxtActionWhatHappened { get; set; }
    public FormTextArea TxtActionCauses { get; set; }
    public FormTextArea TxtActionActions { get; set; }
    public FormTextArea TxtActionMonitoring { get; set; }
    public FormTextArea TxtActionNotes { get; set; }
    public FormSelect CmbActionWhatHappenedResponsible { get; set; }
    public FormSelect CmbActionCausesResponsible { get; set; }
    public FormSelect CmbActionActionsResponsible { get; set; }
    public FormSelect CmbActionActionsExecuter { get; set; }
    public FormSelect CmbActionClosedResponsible { get; set; }
    public FormDatePicker TxtActionWhatHappenedDate { get; set; }
    public FormDatePicker TxtActionCausesDate { get; set; }
    public FormDatePicker TxtActionActionsDate { get; set; }
    public FormDatePicker TxtActionActionsSchedule { get; set; }
    public FormDatePicker TxtActionClosedDate { get; set; }
    #endregion

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating wheter if user shows help in interface
    /// </summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating wheter if user has administrative privileges
    /// </summary>
    public bool IsAdmin
    {
        get
        {
            return this.user.Admin;
        }
    }

    private string returnScript;
    private FormFooter formFooter;
    private FormFooter formFooterAction;

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

    public string FormFooterAction
    {
        get
        {
            return this.formFooterAction.Render(this.dictionary);
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
            this.IncidentId = Convert.ToInt64(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'IncidentList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.formFooter = new FormFooter();
        this.formFooterAction = new FormFooter();

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Incidents", "IncidentList.aspx", false);
        this.master.AddBreadCrumb("Item_Incident_Detail");
        this.grantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Incident);

        if (this.IncidentId > 0)
        {
            this.Incident = Incident.GetById(this.IncidentId, this.company.Id);
            if (this.Incident.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.Incident = Incident.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Incident"], this.Incident.Description);

            this.formFooter.ModifiedBy = this.Incident.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Incident.ModifiedOn;
            this.formFooter.AddButton(new UIButton() { Id = "BtnPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.formFooterAction.ModifiedBy = this.Incident.ModifiedBy.Description;
            this.formFooterAction.ModifiedOn = this.Incident.ModifiedOn;
            this.formFooterAction.AddButton(new UIButton() { Id = "BtnSaveAction", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterAction.AddButton(new UIButton() { Id = "BtnCancelAction", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.master.ItemCode = this.Incident.Description;

            this.IncidentAction = IncidentAction.GetByIncidentId(this.IncidentId, this.company.Id);
            if (this.IncidentAction.Id == 0)
            {
                DateTime? today = DateTime.Now.Date;
                this.IncidentAction.WhatHappened = this.Incident.WhatHappened;
                this.IncidentAction.WhatHappenedBy = this.Incident.WhatHappenedBy;
                this.IncidentAction.WhatHappenedOn = this.Incident.WhatHappenedOn == null ? null : today;
                this.IncidentAction.Causes = this.Incident.Causes;
                this.IncidentAction.CausesBy = this.Incident.CausesBy;
                this.IncidentAction.CausesOn = this.Incident.CausesOn == null ? null : today;
                this.IncidentAction.Actions = this.Incident.Actions;
                this.IncidentAction.ActionsBy = this.Incident.ActionsBy;
                this.IncidentAction.ActionsOn = this.IncidentAction.ActionsOn == null ? null : today;
                this.IncidentAction.ClosedBy = this.Incident.ClosedBy;
                this.IncidentAction.ClosedOn = this.Incident.ClosedOn == null ? null : today;
            }

            this.RenderDocuments();
        }
        else
        {
            this.master.Titulo = "Item_Incident_New_Label";
            this.Incident = Incident.Empty;
            this.IncidentAction = IncidentAction.Empty;
            this.formFooter.ModifiedBy = this.dictionary["Common_New"];
            this.formFooter.ModifiedOn = DateTime.Now;
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.formFooter.ModifiedBy = this.dictionary["Common_New"];
            this.formFooter.ModifiedOn = DateTime.Now;
            this.formFooterAction.AddButton(new UIButton() { Id = "BtnSaveAction", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterAction.AddButton(new UIButton() { Id = "BtnCancelAction", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
        }

        this.tabBar.AddTab(new Tab() { Id = "home", Selected = true, Active = true, Label = this.dictionary["Item_Incident_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab() { Id = "accion", Available = this.user.HasGrantToRead(ApplicationGrant.IncidentActions), Active = true, Label = this.dictionary["Item_Incident_Tab_Action"] });
        this.tabBar.AddTab(new Tab() { Id = "costes", Available = this.user.HasGrantToRead(ApplicationGrant.Cost) && this.IncidentId > 0, Active = this.IncidentId > 0, Label = this.dictionary["Item_Incident_Tab_Costs"] });
        this.tabBar.AddTab(new Tab() { Id = "uploadFiles", Available = true, Active = this.IncidentId > 0, Hidden = this.IncidentId < 1, Label = this.Dictionary["Item_Incident_Tab_UploadFiles"] });
        //// this.tabBar.AddTab(new Tab() { Id = "trazas", Available = this.user.HasGrantToRead(ApplicationGrant.Trace) && this.IncidentId > 0, Active = this.IncidentId > 0, Label = this.dictionary["Item_Incident_Tab_Traces"] });

        this.RenderForm();
        this.RenderActionForm();

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
        this.TxtWhatHappened = new FormTextArea() { Rows = 3, Value = this.Incident.WhatHappened, Name = "TxtWhatHappened", Label = this.dictionary["Item_IncidentAction_Field_WhatHappened"], ColumnsSpan = 8, ColumnsSpanLabel = 12, Embedded = true, MaxLength = 500, GrantToWrite = this.grantToWrite };
        this.TxtCauses = new FormTextArea() { Rows = 3, Value = this.Incident.Causes, Name = "TxtCauses", Label = this.dictionary["Item_Incident_Field_Causes"], ColumnsSpan = 8, ColumnsSpanLabel = 12, Embedded = true, MaxLength = 500, GrantToWrite = this.grantToWrite };
        this.TxtActions = new FormTextArea() { Rows = 3, Value = this.Incident.Actions, Name = "TxtActions", Label = this.dictionary["Item_Incident_Field_Actions"], ColumnsSpan = 8, ColumnsSpanLabel = 12, Embedded = true, MaxLength = 500, GrantToWrite = this.grantToWrite };
        this.TxtAnotations = new FormTextArea() { Rows = 3, Value = this.Incident.Annotations, Name = "TxtAnotations", Label = this.dictionary["Item_Incident_Field_Anotations"], MaxLength = 500, GrantToWrite = this.grantToWrite };
        this.TxtNotes = new FormTextArea() { Rows = 3, Value = this.Incident.Notes, Name = "TxtNotes", Label = this.dictionary["Item_Incident_Field_Notes"], ColumnsSpan = 12, ColumnsSpanLabel = 12, Embedded = true, MaxLength = 500, GrantToWrite = this.grantToWrite };

        this.CmbWhatHappenedResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_Incident_Field_WhatHappenedResponsible"],
            ColumnsSpan = 8,
            Name = "CmbWhatHappenedResponsible",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbCausesResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_Incident_Field_CausesResponsible"],
            ColumnsSpan = 8,
            Name = "CmbCausesResponsible",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionsResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_Incident_Field_ActionsResponsible"],
            ColumnsSpan = 8,
            Name = "CmbActionsResponsible",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionsExecuter = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_Incident_Field_ActionsExecuter"],
            ColumnsSpan = 8,
            Name = "CmbActionsExecuter",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbClosedResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 2,
            Label = this.dictionary["Item_Incident_Field_CloseResponsible"],
            ColumnsSpan = 5,
            Name = "CmbClosedResponsible",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        long whatHappenedResponsibleId = this.Incident.WhatHappenedBy == null ? 0 : this.Incident.WhatHappenedBy.Id;
        long causesResponsibleId = this.Incident.CausesBy == null ? 0 : this.Incident.CausesBy.Id;
        long actionsResponsibleId = this.Incident.ActionsBy == null ? 0 : this.Incident.ActionsBy.Id;
        long actionsExecuterId = this.Incident.ActionsExecuter == null ? 0 : this.Incident.ActionsExecuter.Id;
        long closedResponsibleId = this.Incident.ClosedBy == null ? 0 : this.Incident.ClosedBy.Id;

        foreach (Employee e in this.company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbWhatHappenedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == whatHappenedResponsibleId });
                this.CmbCausesResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == causesResponsibleId });
                this.CmbActionsResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsResponsibleId });
                this.CmbActionsExecuter.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsExecuterId });
                this.CmbClosedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == closedResponsibleId });
            }
            else
            {
                if (this.Incident.WhatHappenedBy != null)
                {
                    if (e.Id == this.Incident.WhatHappenedBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbWhatHappenedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.Incident.CausesBy != null)
                {
                    if (e.Id == this.Incident.CausesBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbCausesResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.Incident.ActionsBy != null)
                {
                    if (e.Id == this.Incident.ActionsBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionsResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.Incident.ActionsExecuter != null)
                {
                    if (e.Id == this.Incident.ActionsExecuter.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionsExecuter.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.Incident.ClosedBy != null)
                {
                    if (e.Id == this.Incident.ClosedBy.Id && (!e.Active || e.DisabledDate != null))
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
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectOne"], Value = "0" }
        };

        this.CmbReporterProvider = new FormSelect()
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType2",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectOne"], Value = "0" }
        };

        this.CmbReporterCustomer = new FormSelect()
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType3",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectOne"], Value = "0" }
        };

        this.TxtDescription = new FormText()
        {
            ColumnSpan = 11,
            ColumnSpanLabel = 1,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Incident_Field_Description"],
            MaximumLength = 100,
            Name = "TxtDescription",
            Placeholder = this.Dictionary["Item_Incident_Field_Description"],
            Value = this.Incident.Description
        };

        this.WhatHappenedDate = new FormDatePicker()
        {
            Id = "TxtWhatHappenedDate",
            Label = this.dictionary["Item_Incident_Field_WhatHappenedDate"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.Incident.WhatHappenedOn,
            GrantToWrite = this.grantToWrite
        };

        this.CausesDate = new FormDatePicker()
        {
            Id = "TxtCausesDate",
            Label = this.dictionary["Item_Incident_Field_CausesDate"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.Incident.CausesOn,
            GrantToWrite = this.grantToWrite
        };

        this.ActionsDate = new FormDatePicker()
        {
            Id = "TxtActionsDate",
            Label = this.dictionary["Common_DateExecution"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.Incident.ActionsOn,
            GrantToWrite = this.grantToWrite
        };

        this.ActionsSchedule = new FormDatePicker()
        {
            Id = "TxtActionsSchedule",
            Label = this.dictionary["Item_Incident_Field_ActionsSchedule"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.Incident.ActionsSchedule,
            GrantToWrite = this.grantToWrite
        };

        this.ClosedDate = new FormDatePicker()
        {
            Id = "TxtClosedDate",
            Label = this.dictionary["Item_Incident_Field_CloseDate"],
            ColumnsSpanLabel = 2,
            ColumnsSpan = 10,
            Value = this.Incident.ClosedOn,
            GrantToWrite = this.grantToWrite
        };
    }

    public void RenderActionForm()
    {
        this.TxtActionWhatHappened = new FormTextArea() { Rows = 3, Value = this.IncidentAction.WhatHappened, Name = "TxtActionWhatHappened", Label = this.dictionary["Item_IncidentAction_Field_WhatHappened"], ColumnsSpan = 8, ColumnsSpanLabel = 12, MaxLength = 500, Embedded = true };
        this.TxtActionCauses = new FormTextArea() { Rows = 3, Value = this.IncidentAction.Causes, Name = "TxtActionCauses", Label = this.dictionary["Item_IncidentAction_Field_Causes"], ColumnsSpan = 8, ColumnsSpanLabel = 12, MaxLength = 500, Embedded = true };
        this.TxtActionActions = new FormTextArea() { Rows = 3, Value = this.IncidentAction.Actions, Name = "TxtActionActions", Label = this.dictionary["Item_IncidentAction_Field_Actions"], ColumnsSpan = 8, ColumnsSpanLabel = 12, MaxLength = 500, Embedded = true };
        this.TxtActionMonitoring = new FormTextArea() { Rows = 3, Value = this.IncidentAction.Monitoring, Name = "TxtActionMonitoring", Label = this.dictionary["Item_IncidentAction_Field_Monitoring"], MaxLength = 500 };
        this.TxtActionNotes = new FormTextArea() { Rows = 3, Value = this.IncidentAction.Notes, Name = "TxtActionNotes", Label = this.dictionary["Item_IncidentAction_Field_Notes"], MaxLength = 500 };

        this.CmbActionWhatHappenedResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_Responsible"],
            ColumnsSpan = 8,
            Name = "CmbActionWhatHappenedResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionCausesResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_Responsible"],
            ColumnsSpan = 8,
            Name = "CmbActionCausesResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionActionsResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_Responsible"],
            ColumnsSpan = 8,
            Name = "CmbActionActionsResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionActionsExecuter = new FormSelect()
        {
            ColumnsSpanLabel = 4,
            Label = this.dictionary["Item_IncidentAction_Field_Responsible"],
            ColumnsSpan = 8,
            Name = "CmbActionActionsExecuter",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbActionClosedResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 2,
            Label = this.dictionary["Item_IncidentAction_Field_Responsible"],
            ColumnsSpan = 6,
            Name = "CmbActionClosedResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        long whatHappenedResponsibleId = this.IncidentAction.WhatHappenedBy == null ? 0 : this.IncidentAction.WhatHappenedBy.Id;
        long causesResponsibleId = this.IncidentAction.CausesBy == null ? 0 : this.IncidentAction.CausesBy.Id;
        long actionsResponsibleId = this.IncidentAction.ActionsBy == null ? 0 : this.IncidentAction.ActionsBy.Id;
        long actionsExecuterId = this.IncidentAction.ActionsExecuter == null ? 0 : this.IncidentAction.ActionsExecuter.Id;
        long closedResponsibleId = this.IncidentAction.ClosedBy == null ? 0 : this.IncidentAction.ClosedBy.Id;

        foreach (Employee e in this.company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbActionWhatHappenedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == whatHappenedResponsibleId });
                this.CmbActionCausesResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == causesResponsibleId });
                this.CmbActionActionsResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsResponsibleId });
                this.CmbActionActionsExecuter.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsExecuterId });
                this.CmbActionClosedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == closedResponsibleId });
            }
            else
            {
                if (this.IncidentAction.WhatHappenedBy != null)
                {
                    if (e.Id == this.IncidentAction.WhatHappenedBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionWhatHappenedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.CausesBy != null)
                {
                    if (e.Id == this.IncidentAction.CausesBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionCausesResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsBy != null)
                {
                    if (e.Id == this.IncidentAction.ActionsBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionActionsResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsExecuter != null)
                {
                    if (e.Id == this.IncidentAction.ActionsExecuter.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionActionsExecuter.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ClosedBy != null)
                {
                    if (e.Id == this.IncidentAction.ClosedBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionClosedResponsible.AddOption(new FormSelectOption() { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }
            }
        }

        this.TxtActionDescription = new FormText()
        {
            ColumnSpan = 10,
            ColumnSpanLabel = 2,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_IncidentAction_Label_Description"],
            MaximumLength = 100,
            Name = "TxtActionDescription",
            Placeholder = this.Dictionary["Item_IncidentAction_Label_Description"],
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            Value = this.IncidentAction.Description
        };

        this.TxtActionWhatHappenedDate = new FormDatePicker()
        {
            Id = "TxtActionWhatHappenedDate",
            Label = this.dictionary["Common_Date"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.WhatHappenedOn
        };

        this.TxtActionCausesDate = new FormDatePicker()
        {
            Id = "TxtActionCausesDate",
            Label = this.dictionary["Common_Date"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.CausesOn
        };

        this.TxtActionActionsDate = new FormDatePicker()
        {
            Id = "TxtActionActionsDate",
            Label = this.dictionary["Common_DateExecution"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.ActionsOn
        };

        this.TxtActionActionsSchedule = new FormDatePicker()
        {
            Id = "TxtActionActionsSchedule",
            Label = this.dictionary["Common_Date"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.IncidentAction.ActionsSchedule
        };

        this.TxtActionClosedDate = new FormDatePicker()
        {
            Id = "TxtActionClosedDate",
            Label = this.dictionary["Common_Date"],
            ColumnsSpanLabel = 2,
            ColumnsSpan = 2,
            Value = this.IncidentAction.ClosedOn
        };
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        ReadOnlyCollection<UploadFile> files = UploadFile.GetByItem(12, this.IncidentId, this.company.Id);
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