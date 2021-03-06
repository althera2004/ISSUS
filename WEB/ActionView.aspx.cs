﻿// --------------------------------
// <copyright file="actionview.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class ActionView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company Company;

    /// <summary>Application user logged in session</summary>
    public ApplicationUser ApplicationUser { get; private set; }

    private bool grantToWrite;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public long IncidentActionId { get; private set; }
    public IncidentAction IncidentAction { get; private set; }
    public Incident Incident { get; private set; }
    public BusinessRisk BusinessRisk { get; private set; }
    public Objetivo Objetivo { get; private set; }
    public Oportunity Oportunity { get; private set; }
    public Auditory Auditory { get; private set; }

    TabBar tabBar = new TabBar { Id = "IncidentActionTabBar" };

    public BarPopup ProviderBarPopups { get; set; }
    public BarPopup CustomerBarPopups { get; set; }

    public string OriginItemLink { get; private set; }

    public string IncidentActionCosts
    {
        get
        {
            if (this.IncidentActionId == 0)
            {
                return Constant.EmptyJsonList;
            }

            return IncidentActionCost.GetByIncidentAction(this.IncidentActionId, this.Company.Id);
        }
    }

    public string CompanyIncidentActionCosts
    {
        get
        {
            if (this.IncidentActionId == 0)
            {
                return Constant.EmptyJsonList;
            }

            return IncidentActionCost.GetByCompany(this.Company.Id);
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

    /// <summary>Indicates if employee is active</summary>
    private bool active;

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

            return this.formFooter.Render(this.Dictionary);
        }
    }

    public string ReturnScript
    {
        get
        {
            return this.returnScript;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string DepartmentsJson
    {
        get
        {
            return Department.ByCompanyJsonList(this.Company.Id);
        }
    }

    public string ProvidersJson
    {
        get
        {
            return Provider.ByCompanyJson(this.Company.Id);
        }
    }

    public string CustomersJson
    {
        get
        {
            return Customer.ByCompanyJson(this.Company.Id);
        }
    }

    public string CostsJson
    {
        get
        {
            return CostDefinition.ByCompanyJson(this.Company.Id);
        }
    }

    public string EmployeesJson
    {
        get
        {
            return Employee.CompanyListJson(this.Company.Id);
        }
    }

    public string GrantToWrite
    {
        get
        {
            return this.grantToWrite ? Constant.JavaScriptTrue : Constant.JavaScriptFalse;
        }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.active = true;
        this.Incident = Incident.Empty;
        this.BusinessRisk = BusinessRisk.Empty;
        this.Objetivo = Objetivo.Empty;
        this.Oportunity = Oportunity.Empty;
        this.Auditory = Auditory.Empty;

        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            int test = 0;
            this.ApplicationUser = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.ApplicationUser.Id))
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
        if (this.Request.QueryString["id"] != null)
        {
            this.IncidentActionId = Convert.ToInt64(this.Request.QueryString["id"]);
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'ActionsList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.ApplicationUser = Session["User"] as ApplicationUser;
        this.Company = Session["company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        if (this.ApplicationUser.HasGrantToRead(ApplicationGrant.IncidentActions))
        {
            this.master.AddBreadCrumb("Item_IncidentActions", "ActionList.aspx", Constant.NotLeaft);
        }
        else
        {
            this.master.AddBreadCrumb("Item_IncidentActions");
        }

        this.master.AddBreadCrumb("Item_IncidentActions_Detail");
        this.grantToWrite = this.ApplicationUser.HasGrantToWrite(ApplicationGrant.IncidentActions);
        this.Incident = Incident.Empty;

        if (this.IncidentActionId > 0)
        {
            this.IncidentAction = IncidentAction.ById(this.IncidentActionId, this.Company.Id);
            if (this.IncidentAction.CompanyId != this.Company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.IncidentAction = IncidentAction.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_IncidentAction"], this.IncidentAction.Description);

            this.formFooter = new FormFooter
            {
                ModifiedBy = this.IncidentAction.ModifiedBy.Description,
                ModifiedOn = this.IncidentAction.ModifiedOn
            };

            this.formFooter.AddButton(new UIButton { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.Dictionary["Item_IncidentAction_Btn_Restaurar"], Action = "primary" });
            this.formFooter.AddButton(new UIButton { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.Dictionary["Item_IncidentAction_Btn_Anular"], Action = "danger" });
            this.formFooter.AddButton(new UIButton { Id = "BtnPrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"], ColumnsSpan = 12 });

            this.master.ItemCode = this.IncidentAction.Description;
            this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin2"];

            if (this.IncidentAction.IncidentId.HasValue && this.IncidentAction.IncidentId > 0)
            {
                this.Incident = Incident.GetById(this.IncidentAction.IncidentId.Value, this.Company.Id);
                this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin3"] + " " + this.Incident.Link;
            }

            if (this.IncidentAction.BusinessRiskId.HasValue && this.IncidentAction.BusinessRiskId > 0)
            {
                this.BusinessRisk = BusinessRisk.ById(this.Company.Id, this.IncidentAction.BusinessRiskId.Value);
                this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin4"] + " " + this.BusinessRisk.Link;
            }

            if(this.IncidentAction.Objetivo.Id > 0)
            {
                this.Objetivo = this.IncidentAction.Objetivo;
                this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin5"] + " " + this.Objetivo.Link;
            }

            if (this.IncidentAction.Oportunity.Id > 0)
            {
                this.Oportunity = this.IncidentAction.Oportunity;
                this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin6"] + " " + this.Oportunity.Link;
            }
            
            if (this.IncidentAction.Origin == 1)
            {
                if (this.IncidentAction.AuditoryId != null && this.IncidentAction.AuditoryId > 0)
                {
                    var auditory = Auditory.ById(this.IncidentAction.AuditoryId.Value, this.Company.Id);
                    this.OriginItemLink = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0} - {1}",
                        this.Dictionary["Item_IncidentAction_Origin1"],
                        auditory.Link);
                }
                else
                {
                    this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin1"];
                }
            }

            this.RenderDocuments();            
        }
        else
        {
            this.master.Titulo = "Item_IncidentActions_Detail";
            this.IncidentAction = IncidentAction.Empty;
            this.formFooter = new FormFooter(this.Dictionary, this.grantToWrite);

            if(this.Request.QueryString["o"] != null)
            {
                var objetivoId = Convert.ToInt32(this.Request.QueryString["o"]);
                this.Objetivo = Objetivo.ById(objetivoId, this.Company.Id);
                this.IncidentAction.Description = this.Objetivo.Name;
                this.IncidentAction.Origin = 5;
                this.IncidentAction.Objetivo = this.Objetivo;
                this.IncidentAction.WhatHappened = this.Objetivo.Description;
                this.IncidentAction.WhatHappenedOn = DateTime.Now;
            }

            this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin2"];

            if(this.IncidentAction.Origin == 1)
            {
                this.OriginItemLink = this.Dictionary["Item_IncidentAction_Origin1"];
            } 
            else if(this.IncidentAction.Origin == 5)
            {
                this.OriginItemLink = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}: {1}",
                    this.Dictionary["Item_IncidentAction_Origin5"],
                    this.Objetivo.Link);
            }
        }

        this.tabBar.AddTab(new Tab { Id = "home", Selected = true, Active = true, Label = this.Dictionary["Item_IncidentAction_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab { Id = "costes", Available = this.ApplicationUser.HasGrantToRead(ApplicationGrant.Cost) && this.IncidentActionId > 0, Active = this.IncidentActionId > 0, Label = this.Dictionary["Item_IncidentAction_Tab_Costs"] });
        this.tabBar.AddTab(new Tab { Id = "uploadFiles", Available = true, Active = this.IncidentActionId > 0, Hidden = this.IncidentActionId < 1, Label = this.Dictionary["Item_IncidentAction_Tab_UploadFiles"] });
        // this.tabBar.AddTab(new Tab { Id = "trazas", Available = this.user.HasTraceGrant() && this.IncidentActionId > 0, Active = this.IncidentActionId > 0, Label = this.dictionary["Item_IncidentAction_Tab_Traces"] });

        this.RenderForm();
        
        this.ProviderBarPopups = new BarPopup
        {
            Id = "Provider",
            DeleteMessage = this.Dictionary["Common_DeleteMessage"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
            Description = "Proveedor",
            FieldName = this.Dictionary["Item_Provider"],
            BarTitle = this.Dictionary["Item_Providers"]
        };

        this.CustomerBarPopups = new BarPopup
        {
            Id = "Customer",
            DeleteMessage = this.Dictionary["Common_DeleteMessage"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
            Description = "Cliente",
            FieldName = this.Dictionary["Item_Customer"],
            BarTitle = this.Dictionary["Item_Customers"]
        };
    }

    public void RenderForm()
    {
        this.TxtWhatHappened = new FormTextArea { Rows = 5, Value = this.IncidentAction.WhatHappened, Name = "TxtWhatHappened", Label = this.Dictionary["Item_IncidentAction_Field_WhatHappened"], ColumnsSpan = Constant.ColumnSpan8, ColumnsSpanLabel = 12,MaxLength= Constant.MaximumTextAreaLength, Embedded = true };
        this.TxtCauses = new FormTextArea {Rows = 5, Value = this.IncidentAction.Causes, Name = "TxtCauses", Label = this.Dictionary["Item_IncidentAction_Field_Causes"], ColumnsSpan = Constant.ColumnSpan8, ColumnsSpanLabel = 12, MaxLength = Constant.MaximumTextAreaLength, Embedded = true };
        this.TxtActions = new FormTextArea {Rows = 5, Value = this.IncidentAction.Actions, Name = "TxtActions", Label = this.Dictionary["Item_IncidentAction_Field_Actions"], ColumnsSpan = Constant.ColumnSpan8, ColumnsSpanLabel = 12, MaxLength = Constant.MaximumTextAreaLength, Embedded = true };
        this.TxtMonitoring = new FormTextArea {Rows = 5, Value = this.IncidentAction.Monitoring, Name = "TxtMonitoring", Label = this.Dictionary["Item_IncidentAction_Field_Monitoring"], ColumnsSpan = 12, ColumnsSpanLabel = 12, MaxLength = Constant.MaximumTextAreaLength, Embedded = true };
        this.TxtNotes = new FormTextArea {Rows = 5, Value = this.IncidentAction.Notes, Name = "TxtNotes", Label = this.Dictionary["Item_IncidentAction_Field_Notes"], ColumnsSpan = 12, ColumnsSpanLabel = 12, MaxLength = Constant.MaximumTextAreaLength, Embedded = true };

        this.CmbWhatHappenedResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleWhatHappend"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbWhatHappenedResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = FormSelectOption.DefaultOption(this.Dictionary)
        };

        this.CmbCausesResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleCauses"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbCausesResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = FormSelectOption.DefaultOption(this.Dictionary)
        };

        this.CmbActionsResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleActions"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionsResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = FormSelectOption.DefaultOption(this.Dictionary)
        };

        this.CmbActionsExecuter = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_Executer"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionsExecuter",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = FormSelectOption.DefaultOption(this.Dictionary)
        };

        this.CmbClosedResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleClose"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbClosedResponsible",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.Dictionary["Common_Required"],
            Required = true,
            DefaultOption = FormSelectOption.DefaultOption(this.Dictionary)
        };

        this.CmbClosedExecutor = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan2,
            Label = this.Dictionary["Item_IncidentAction_Field_Executer"],
            ColumnsSpan = 3,
            Name = "CmbClosedExecutor",
            GrantToWrite = this.grantToWrite,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = FormSelectOption.DefaultOption(this.Dictionary)
        };

        long whatHappendResponsibleId = this.IncidentAction.WhatHappenedBy == null ? 0 : this.IncidentAction.WhatHappenedBy.Id;
        long causesResponsibleId = this.IncidentAction.CausesBy == null ? 0 : this.IncidentAction.CausesBy.Id;
        long actionsResponsibleId = this.IncidentAction.ActionsBy == null ? 0 : this.IncidentAction.ActionsBy.Id;
        long actionsExecuterId = this.IncidentAction.ActionsExecuter == null ? 0 : this.IncidentAction.ActionsExecuter.Id;
        long closedResponsibleId = this.IncidentAction.ClosedBy == null ? 0 : this.IncidentAction.ClosedBy.Id;
        long closedExecutorId = this.IncidentAction.ClosedExecutor == null ? 0 : this.IncidentAction.ClosedExecutor.Id;

        foreach (var e in this.Company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbWhatHappenedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == whatHappendResponsibleId });
                this.CmbCausesResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == causesResponsibleId });
                this.CmbActionsResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsResponsibleId });
                this.CmbActionsExecuter.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == actionsExecuterId });
                this.CmbClosedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = e.Id == closedResponsibleId });
            }
            else
            {
                if (this.IncidentAction.WhatHappenedBy != null)
                {
                    if (e.Id == this.IncidentAction.WhatHappenedBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbWhatHappenedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.CausesBy != null)
                {
                    if (e.Id == this.IncidentAction.CausesBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbCausesResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsBy != null)
                {
                    if (e.Id == this.IncidentAction.ActionsBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionsResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ActionsExecuter != null)
                {
                    if (e.Id == this.IncidentAction.ActionsExecuter.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbActionsExecuter.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }

                if (this.IncidentAction.ClosedBy != null)
                {
                    if (e.Id == this.IncidentAction.ClosedBy.Id && (!e.Active || e.DisabledDate != null))
                    {
                        this.CmbClosedResponsible.AddOption(new FormSelectOption { Value = e.Id.ToString(), Text = e.FullName, Selected = true });
                    }
                }
            }
        }

        var defaultOption = FormSelectOption.DefaultOption(this.Dictionary);

        this.CmbReporterDepartment = new FormSelect
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType1",
            GrantToWrite = this.grantToWrite,
            DefaultOption = defaultOption
        };

        this.CmbReporterProvider = new FormSelect
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType2",
            GrantToWrite = this.grantToWrite,
            DefaultOption = defaultOption
        };

        this.CmbReporterCustomer = new FormSelect
        {
            ColumnsSpan = 12,
            Name = "CmbReporterType3",
            GrantToWrite = this.grantToWrite,
            DefaultOption = defaultOption
        };

        this.TxtDescription = new FormText
        {
            ColumnSpan = 10,
            ColumnSpanLabel = 1,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_IncidentAction_Label_Description"],
            MaximumLength = 100,
            Name = "TxtDescription",
            Placeholder = this.Dictionary["Item_IncidentAction_Label_Description"],
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            Value = this.IncidentAction.Description

        };

        this.WhatHappenedDate = new FormDatePicker
        {
            Id = "TxtWhatHappenedDate",
            Label = this.Dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = Constant.ColumnSpan4,
            ColumnsSpan = Constant.ColumnSpan8,
            Value = this.IncidentAction.WhatHappenedOn
        };

        this.CausesDate = new FormDatePicker
        {
            Id = "TxtCausesDate",
            Label = this.Dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = Constant.ColumnSpan4,
            ColumnsSpan = Constant.ColumnSpan8,
            Value = this.IncidentAction.CausesOn
        };

        this.ActionsDate = new FormDatePicker
        {
            Id = "TxtActionsDate",
            Label = this.Dictionary["Common_DateExecution"],
            ColumnsSpanLabel = Constant.ColumnSpan4,
            ColumnsSpan = Constant.ColumnSpan8,
            Value = this.IncidentAction.ActionsOn
        };

        this.ActionsSchedule = new FormDatePicker
        {
            Id = "TxtActionsSchedule",
            Label = this.Dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = Constant.ColumnSpan4,
            ColumnsSpan = Constant.ColumnSpan8,
            Value = this.IncidentAction.ActionsSchedule
        };

        this.ClosedDate = new FormDatePicker
        {
            Id = "TxtClosedDate",
            Label = this.Dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = Constant.ColumnSpan4,
            ColumnsSpan = Constant.ColumnSpan6,
            Value = this.IncidentAction.ClosedOn,
            GrantToWrite = this.grantToWrite,
            Required = true
        };

        this.ClosedExecutorDate = new FormDatePicker
        {
            Id = "TxtClosedExecutorDate",
            Label = this.Dictionary["Item_IncidentAction_Field_Date"],
            ColumnsSpanLabel = Constant.ColumnSpan2,
            ColumnsSpan = Constant.ColumnSpan2,
            Value = this.IncidentAction.ClosedExecutorOn
        };
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(ItemIdentifiers.IncidentActions, this.IncidentActionId, this.Company.Id);
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
                    this.Company.Id,
                    file.FileName,
                    file.CreatedOn,
                    finalSize,
                    this.Dictionary["Item_Attachment_Header_CreateDate"],
                    this.Dictionary["Item_Attachment_Header_Size"],
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
                this.Company.Id,
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