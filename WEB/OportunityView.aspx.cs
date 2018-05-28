// --------------------------------
// <copyright file="OportunityView.aspx.cs" company="Sbrinna">
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
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using GisoFramework.DataAccess;
using GisoFramework.Item.Binding;

public partial class OportunityView : Page
{
    #region Fields
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Barra de BusinessRisk<summary>
    private TabBar tabBar = new TabBar { Id = "BusinessRiskTabBar" };

    /// <summary>Footer of main tab</summary>
    private FormFooter formFooter;

    /// <summary>Footer of actions tab</summary>
    private FormFooter formFooterActions;

    /// <summary>Action associated to businessRisk</summary>
    private IncidentAction incidentAction = IncidentAction.Empty;

    /// <summary>Action List associated to businessRisk</summary>
    private IncidentAction incidentActionHistory = IncidentAction.Empty;

    /// <summary>Gets oportinity identifier</summary>
    public long OportunityId { get; private set; }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public bool historyActionActive;

    /// <summary>Public access to businessRisk</summary>
    public Oportunity Oportunity { get; private set; }

    public string ActualLimit
    {
        get
        {
            if (this.Oportunity.Id > 0)
            {
                return this.Oportunity.Rule.Limit.ToString();
            }

            return "-";
        }
    }

    /// <summary>Public access to company</summary>
    public Company Company { get; private set; }

    /// <summary>
    /// Public access to dictionary
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Public access to user
    /// </summary>
    public new ApplicationUser User
    {
        get
        {
            return this.user;
        }
    }

    /// <summary>
    /// Render of tabBar
    /// </summary>
    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    /// <summary>
    /// Render of formFooter
    /// </summary>
    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    /// <summary>
    /// Render of formFooterActions
    /// </summary>
    public string FormFooterActions
    {
        get
        {
            return this.formFooterActions.Render(this.dictionary);
        }
    }

    /// <summary>
    /// Public access to incidentAction
    /// </summary>
    public IncidentAction IncidentAction
    {
        get
        {
            return this.incidentAction;
        }
    }

    /// <summary>
    /// Public access to all incidentAction
    /// </summary>
    public IncidentAction IncidentActionHistory
    {
        get
        {
            return this.incidentActionHistory;
        }
    }

    /// <summary>Cost definitions by company</summary>
    public string CompanyIncidentCosts
    {
        get
        {
            if (this.Oportunity.Id < 1)
            {
                return Constant.EmptyJsonList;
            }

            return IncidentCost.ByCompany(this.company.Id);
        }
    }

    public string CostsJson
    {
        get
        {
            return CostDefinition.ByCompanyJson(this.company.Id);
        }
    }

    /// <summary>Json containing the items in the Rules BAR</summary>
    public string RulesJson
    {
        get
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (Rules rule in Rules.GetBar(this.company.Id))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }
                res.Append(rule.JsonBAR);
            }
            res.Append("]");
            return res.ToString();
        }
    }

    /// <summary>Gets all the employees in the company</summary>
    public string EmployeesJson
    {
        get
        {
            return Employee.CompanyListJson(this.company.Id);
            //return Employee.GetByCompanyJson(this.company.Id);
        }
    }

    /// <summary>Json containing the items in the probability and severity combos</summary>
    public string CostImpactJson
    {
        get
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var proSev in CostImpactRange.GetActive(this.company.Id))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }
                res.Append(proSev.Json);
            }

            res.Append("]");
            return res.ToString();
        }
    }

    public string IncidentCosts
    {
        get
        {
            if (this.Oportunity.Id == 0)
            {
                return Constant.EmptyJsonList;
            }

            return IncidentActionCost.GetByBusinessRisk(this.Oportunity.Id, this.company.Id);
        }
    }
    #endregion

    #region OportunityForm

    /// <summary>Text box for the Name</summary>
    public string TxtName
    {
        get
        {
            return new FormText
            {
                Name = "TxtDescription",
                Value = this.Oportunity.Description,
                ColumnSpan = 8,
                Placeholder = this.dictionary["Item_Oportunity_LabelField_Name"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk),
                MaximumLength = 100
            }.Render;
        }
    }

    /// <summary>Text box for the Code</summary>
    public string TxtCode
    {
        get
        {
            return new FormText
            {
                Name = "TxtCode",
                Value = string.Format(CultureInfo.InvariantCulture, "{0:00000}", this.Oportunity.Code),
                ColumnSpan = 2,
                Placeholder = this.dictionary["Item_Oportunity_LabelField_Code"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk)
            }.Render;
        }
    }

    /// <summary>DatePicker for Start Date</summary>
    public string TxtDateStart
    {
        get
        {
            return new FormDatePicker
            {
                Id = "DateStart",
                Value = this.Oportunity.DateStart,
                ColumnsSpan = Constant.ColumnSpan2
            }.Render;
        }
    }

    /// <summary>TextArea for the StartControl</summary>
    public string TxtControl
    {
        get
        {
            return new FormTextArea
            {
                Name = "TxtControl",
                Value = this.Oportunity.Control,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    /// <summary>TextArea for the Description</summary>
    public string TxtItemDescription
    {
        get
        {
            return new FormTextArea
            {
                Name = "TxtItemDescription",
                Value = this.Oportunity.ItemDescription,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    /// <summary>TextArea for Notes</summary>
    public string TxtNotes
    {
        get
        {
            return new FormTextArea
            {
                Name = "TxtNotes",
                Value = this.Oportunity.Notes,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    /// <summary>TextArea for Causes</summary>
    public string TxtCauses
    {
        get
        {
            return new FormTextArea
            {
                Name = "TxtCauses",
                Value = this.Oportunity.Causes,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    #endregion

    #region ActionForm

    public string TxtActionDescription
    {
        get
        {
            return new FormText
            {
                ColumnSpan = 10,
                ColumnSpanLabel = 2,
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk),
                Label = this.Dictionary["Item_IncidentAction_Label_Description"],
                MaximumLength = 100,
                Name = "TxtActionDescription",
                Placeholder = this.Dictionary["Item_IncidentAction_Label_Description"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Value = this.incidentAction.Description
            }.Render;
        }
    }

    public string TxtActionWhatHappened
    {
        get
        {
            return new FormTextArea
            {
                Rows = 5,
                Value = this.IncidentAction.WhatHappened,
                Name = "TxtActionWhatHappened",
                Label = this.dictionary["Item_IncidentAction_Field_WhatHappened"],
                ColumnsSpan = Constant.ColumnSpan8,
                ColumnsSpanLabel = 12,
                Embedded = true,
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    public string TxtActionCauses
    {
        get
        {
            return new FormTextArea
            {
                Rows = 5,
                Value = this.IncidentAction.Causes,
                Name = "TxtActionCauses",
                Label = this.dictionary["Item_IncidentAction_Field_Causes"],
                ColumnsSpan = Constant.ColumnSpan8,
                ColumnsSpanLabel = 12,
                Embedded = true,
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    public string TxtActionActions
    {
        get
        {
            return new FormTextArea
            {
                Rows = 5,
                Value = this.IncidentAction.Actions,
                Name = "TxtActionActions",
                Label = this.dictionary["Item_IncidentAction_Field_Actions"],
                ColumnsSpan = Constant.ColumnSpan8,
                ColumnsSpanLabel = 12,
                Embedded = true,
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    public string TxtActionMonitoring
    {
        get
        {
            return new FormTextArea
            {
                Rows = 5,
                Value = this.IncidentAction.Monitoring,
                Name = "TxtActionMonitoring",
                Label = this.dictionary["Item_IncidentAction_Field_Monitoring"],
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    public string TxtActionNotes
    {
        get
        {
            return new FormTextArea
            {
                Rows = 5,
                Value = this.IncidentAction.Notes,
                Name = "TxtActionNotes",
                Label = this.dictionary["Item_IncidentAction_Field_Notes"],
                MaxLength = Constant.MaximumTextAreaLength
            }.Render;
        }
    }

    public FormSelect ComboActionWhatHappenedResponsible { get; set; }

    public FormSelect ComboActionCausesResponsible { get; set; }

    public FormSelect ComboActionActionsResponsible { get; set; }

    public FormSelect ComboActionActionsExecuter { get; set; }

    public FormSelect ComboActionClosedResponsible { get; set; }

    public string TxtActionWhatHappenedDate
    {
        get
        {
            return new FormDatePicker
            {
                Id = "TxtActionWhatHappenedDate",
                Label = this.dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.WhatHappenedOn
            }.Render;
        }
    }

    public string TxtActionCausesDate
    {
        get
        {
            return new FormDatePicker
            {
                Id = "TxtActionCausesDate",
                Label = this.dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.CausesOn
            }.Render;
        }
    }

    public string TxtActionActionsDate
    {
        get
        {
            return new FormDatePicker
            {
                Id = "TxtActionActionsDate",
                Label = this.dictionary["Common_DateExecution"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.ActionsOn
            }.Render;
        }
    }

    public string TxtActionActionsSchedule
    {
        get
        {
            return new FormDatePicker
            {
                Id = "TxtActionActionsSchedule",
                Label = this.dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.ActionsSchedule
            }.Render;
        }
    }

    public string TxtActionClosedDate
    {
        get
        {
            return new FormDatePicker
            {
                Id = "TxtActionClosedDate",
                Label = this.dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan6,
                Value = this.IncidentAction.ClosedOn,
                Required = true
            }.Render;
        }
    }

    #endregion

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

            Context.ApplicationInstance.CompleteRequest();
        }
    }

    /// <summary>Main action to load page elements</summary>
    private void Go()
    {
        this.company = this.Session["company"] as Company;
        this.dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.user = this.Session["User"] as ApplicationUser;
        this.Session["User"] = this.user;

        if (this.Request.QueryString["id"] != null)
        {
            this.OportunityId = Convert.ToInt64(this.Request.QueryString["id"]);
        }

        string label = "Item_Oportunity_Title_OportunityDetails";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        if (user.HasGrantToRead(26))
        {
            this.master.AddBreadCrumb("Item_Oportunity", "BusinessRisksList.aspx", Constant.NotLeaft);
        }
        else
        {
            this.master.AddBreadCrumb("Item_Oportunity");
        }

        this.master.AddBreadCrumb(label);
        if (!this.Page.IsPostBack)
        {
            //this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.businessRisk, TargetTypes.BusinessRisk);
        }

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        this.formFooterActions = new FormFooter();
        this.formFooterActions.AddButton(new UIButton { Id = "BtnSave2", Icon = "icon-ok", Action = "success", Text = this.dictionary["Common_Accept"] });
        this.formFooterActions.AddButton(new UIButton { Id = "BtnCancel2", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (this.OportunityId != -1)
        {
            this.Oportunity = Oportunity.ById(this.OportunityId, this.company.Id);
            this.master.Titulo = label + this.Oportunity.Description;
            this.incidentAction = IncidentAction.ByBusinessRiskId(this.Oportunity.Id, this.company.Id);
            if (this.Oportunity.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.Oportunity = Oportunity.Empty;
            }

            this.formFooter.ModifiedBy = this.Oportunity.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Oportunity.ModifiedOn;
            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Oportunity"], this.Oportunity.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.master.Titulo = label + this.dictionary["Common_New"];
            this.Oportunity = Oportunity.Empty;
            this.incidentAction = IncidentAction.Empty;
        }

        historyActionActive = false;

        this.tabBar.AddTab(new Tab { Id = "home", Selected = true, Active = true, Label = this.dictionary["Item_Oportunity_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab { Id = "accion", Available = this.user.HasGrantToRead(ApplicationGrant.IncidentActions), Active = true, Label = this.dictionary["Item_Oportunity_Tab_Action"] });
        this.tabBar.AddTab(new Tab { Id = "costes", Available = this.user.HasGrantToRead(ApplicationGrant.Cost), Active = true, Label = this.dictionary["Item_Oportunity_Tab_Costs"] });
        this.tabBar.AddTab(new Tab { Id = "uploadFiles", Available = true, Active = true, Label = this.dictionary["Item_Oportunity_Tab_UploadFiles"], Hidden = this.Oportunity.Id < 1 });

        RenderProcess();
        RenderLimit();
        RenderProbabilitySeverity();
        RenderActionsForm();
        this.RenderDocuments();
        //RenderRemainder();
    }

    /// <summary>Renders the selectable Processes</summary>
    private void RenderProcess()
    {
        var processeCollection = Process.ByCompany(this.company.Id);
        var processList = new StringBuilder();
        processList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""0"">{0}</option>", this.Dictionary["Common_SelectOne"]));
        foreach (var process in processeCollection.OrderBy(process => process.Description))
        {
            processList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{1}</option>", process.Id, process.Description));
        }

        this.LTProcess.Text = processList.ToString();
    }

    /// <summary>Renders the selectable Rules</summary>
    private void RenderLimit()
    {
        var limitCollection = ProbabilitySeverityRange.All(this.company.Id);
        var limitList = new StringBuilder();
        var probabilityList = new StringBuilder();
        var severityList = new StringBuilder();
        probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""0"">{0}</option>", this.Dictionary["Common_SelectOne"]));
        probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<optgroup label=""Probability"">", this.Dictionary["Common_SelectOne"]));
        severityList.Append(string.Format(CultureInfo.InvariantCulture, @"<optgroup label=""Severity"">", this.Dictionary["Common_SelectOne"]));
        foreach (var limit in limitCollection.OrderBy(limit => limit.Code))
        {
            if (limit.Type == ProbabilitySeverityRange.ProbabilitySeverityType.Probability)
            {
                probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{1}</option>", limit.Id, limit.Description));
            }
            else
            {
                severityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{1}</option>", limit.Id, limit.Description));
            }
        }

        limitList.Append(probabilityList);
        limitList.Append(severityList);
    }

    /// <summary>Renders the selectable ProbabilitySeverityRanges</summary>
    private void RenderProbabilitySeverity()
    {
        var probabilitySeverityCollection = ProbabilitySeverityRange.GetActive(this.company.Id);
        var severityList = new StringBuilder();
        severityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""0"">{0}</option>", this.Dictionary["Common_SelectOne"]));
        var probabilityList = new StringBuilder();
        probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""0"">{0}</option>", this.Dictionary["Common_SelectOne"]));
        foreach (var probabilitySeverity in probabilitySeverityCollection.OrderBy(probabilitySeverity => probabilitySeverity.Code))
        {
            if (probabilitySeverity.Type == ProbabilitySeverityRange.ProbabilitySeverityType.Severity)
            {
                severityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{0} - {1}</option>", probabilitySeverity.Code, probabilitySeverity.Description));
            }
            else
            {
                probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{0} - {1}</option>", probabilitySeverity.Code, probabilitySeverity.Description));
            }
        }
    }

    private void RenderActionsForm()
    {
        var defaultOption = FormSelectOption.DefaultOption(this.dictionary);

        this.ComboActionWhatHappenedResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleWhatHappend"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionWhatHappenedResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk),
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionCausesResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleCauses"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionCausesResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk),
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionActionsResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleActions"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionActionsResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk),
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionActionsExecuter = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.dictionary["Item_IncidentAction_Field_Responsible"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionActionsExecuter",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk),
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionClosedResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.dictionary["Item_IncidentAction_Field_ResponsibleClose"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionClosedResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk),
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = defaultOption
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
                this.ComboActionWhatHappenedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == whatHappenedResponsibleId });
                this.ComboActionCausesResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == causesResponsibleId });
                this.ComboActionActionsResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsResponsibleId });
                this.ComboActionActionsExecuter.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsExecuterId });
                this.ComboActionClosedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == closedResponsibleId });
            }
            else
            {
                if (this.IncidentAction.WhatHappenedBy != null)
                {
                    if (e.Id == this.IncidentAction.WhatHappenedBy.Id && !e.Active)
                    {
                        this.ComboActionWhatHappenedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.CausesBy != null)
                {
                    if (e.Id == this.IncidentAction.CausesBy.Id && !e.Active)
                    {
                        this.ComboActionCausesResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsBy != null)
                {
                    if (e.Id == this.IncidentAction.ActionsBy.Id && !e.Active)
                    {
                        this.ComboActionActionsResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsExecuter != null)
                {
                    if (e.Id == this.IncidentAction.ActionsExecuter.Id && !e.Active)
                    {
                        this.ComboActionActionsExecuter.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ClosedBy != null)
                {
                    if (e.Id == this.IncidentAction.ClosedBy.Id && !e.Active)
                    {
                        this.ComboActionClosedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }
            }
        }
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(26, this.OportunityId, this.company.Id).ToList();

        if (this.incidentAction.Id > 0)
        {
            files.AddRange(UploadFile.GetByItem(13, this.incidentAction.Id, this.company.Id));
        }

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