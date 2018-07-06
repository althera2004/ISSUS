// --------------------------------
// <copyright file="BusinessRiskView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.DataAccess;
using GisoFramework.Item;
using GisoFramework.Item.Binding;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class BusinessRiskView : Page
{
    #region Fields
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Current BusinessRisk object</summary>
    private BusinessRisk businessRisk;

    /// <summary>Barra de BusinessRisk<summary>
    private TabBar tabBar = new TabBar() { Id = "BusinessRiskTabBar" };

    /// <summary>Footer of main tab</summary>
    private FormFooter formFooter;

    /// <summary>Footer of actions tab</summary>
    private FormFooter formFooterActions;

    /// <summary>Action associated to businessRisk</summary>
    private IncidentAction incidentAction = IncidentAction.Empty;
    
    /// <summary>Action List associated to businessRisk</summary>
    private IncidentAction incidentActionHistory = IncidentAction.Empty;

    private bool GrantToWriteBusinessRisk;

    private bool GrantToWriteIncidentAction;
    
    /// <summary>Public access to businessRiskId</summary>
    public long BusinessRiskId { get; private set; }

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
    public BusinessRisk BusinessRisk
    {
        get
        {
            return this.businessRisk;
        }
    }

    public string ActualLimit
    {
        get
        {
            if (this.businessRisk.Id > 0)
            {
                return this.businessRisk.Rules.Limit.ToString();
            }

            return "-";
        }
    }

    /// <summary>Public access to company</summary>
    public Company Company { get; private set; }

    /// <summary>Public access to dictionary</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>Public access to user</summary>
    public ApplicationUser ApplicationUser { get; private set; }

    /// <summary>Render of tabBar</summary>
    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    /// <summary>Render of formFooter</summary>
    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
        }
    }
    
    /// <summary>Render of formFooterActions</summary>
    public string FormFooterActions
    {
        get
        {
            return this.formFooterActions.Render(this.Dictionary);
        }
    }
    
    /// <summary>Public access to incidentAction</summary>
    public IncidentAction IncidentAction
    {
        get
        {
            return this.incidentAction;
        }
    }

    /// <summary>Public access to all incidentAction</summary>
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
            if (this.businessRisk.Id < 1)
            {
                return "[]";
            }

            return IncidentCost.ByCompany(this.Company.Id);
        }
    }

    public string CostsJson
    {
        get
        {
            return CostDefinition.ByCompanyJson(this.Company.Id);
        }
    }

    /// <summary>Json containing the items in the Rules BAR</summary>
    public string RulesJson
    {
        get
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (Rules rule in Rules.GetBar(this.Company.Id))
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
            return Employee.CompanyListJson(this.Company.Id);
        }
    }

    /// <summary>Json containing the items in the probability and severity combos</summary>
    public string ProbabilitySeverityJson
    {
        get
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var proSev in ProbabilitySeverityRange.GetActive(this.Company.Id))
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
            if (this.businessRisk.Id == 0)
            {
                return Constant.EmptyJsonList;
            }

            return IncidentActionCost.GetByBusinessRisk(this.businessRisk.Id, this.Company.Id);
        }
    }

    /// <summary>Json containing the items in the BusinessRisk</summary>
    public string HistoryJson
    {
        get
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var BusinessRiskHistory in BusinessRisk.ByHistory(businessRisk.Code, Company.Id))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }
                res.Append(BusinessRiskHistory.JsonHistory);
            }

            if(businessRisk.FinalResult > 0)
            {
                string formattedJson =
                    @"{{
                        ""Id"": {3},
                        ""Description"": ""{4}"",
                        ""DateStart"": ""{0:dd/MM/yyyy}"",
                        ""Result"": {1},
                        ""Assumed"": {2}
                    }}";

                string lastResult =  string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    formattedJson,
                    businessRisk.FinalDate,
                    businessRisk.FinalResult,
                    "false",
                    businessRisk.Id,
                    businessRisk.Description
                );

                res.Append(",").Append(lastResult);
            }

            res.Append("]");
            return res.ToString();
        }
    }

    /// <summary>Json containing the actions in the BusinessRisk</summary>
    public string IncidentActionHistoryJson
    {
        get
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var IncidentActionHistory in IncidentAction.ByBusinessRiskCode(businessRisk.Code, Company.Id))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }
                res.Append(IncidentActionHistory.Json);
            }
            res.Append("]");
            return res.ToString();
        }
    }
    #endregion

    #region BusinessRiskForm
    /// <summary>Text box for the Name</summary>
    public string TxtName
    {
        get
        {
            return new FormText
            {
                Name = "Name",
                Value = string.Empty,
                ColumnSpan =8,
                Placeholder = this.Dictionary["Item_BusinessRisk_LabelField_Name"],
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = GrantToWriteBusinessRisk,
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
               Name = "Code",
               Value = string.Format(CultureInfo.GetCultureInfo("en-us"),"{0:00000}", this.businessRisk.Code),
               ColumnSpan = 2,
               Placeholder = this.Dictionary["Item_BusinessRisk_LabelField_Code"],
               Required = true,
               RequiredMessage = this.Dictionary["Common_Required"],
               Duplicated = true,
               DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
               GrantToWrite = GrantToWriteBusinessRisk
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
                Value = this.businessRisk.DateStart,
                ColumnsSpan = Constant.ColumnSpan2,
                GrantToWrite = GrantToWriteBusinessRisk
            }.Render;
        }
    }

    /// <summary>TextArea for the StartControl</summary>
    public string TxtStartControl {
        get
        {
            return new FormTextArea
            {
                Name = "StartControl",
                Value = this.businessRisk.StartControl,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteBusinessRisk
            }.Render;
        }
    }

    /// <summary>TextArea for the Description</summary>
    public string TxtDescription
    {
        get
        {
            return new FormTextArea
            {
                Name = "Description",
                Value = this.businessRisk.ItemDescription,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteBusinessRisk
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
                Name = "Notes",
                Value = this.businessRisk.Notes,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteBusinessRisk
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
                Name = "Causes",
                Value = this.businessRisk.Causes,
                ColumnsSpan = 12,
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteBusinessRisk
            }.Render;
        }
    }

    /// <summary>TextBox for InitialValue</summary>
    public string TxtInitialValue
    {
        get
        {
            return new FormTextInteger
            {
                Name = "InitialValue",
                Value = this.businessRisk.InitialValue.ToString(),
                ColumnSpan = 12,
                Placeholder = this.Dictionary["Item_BusinessRisk_LabelField_InitialValue"],
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = GrantToWriteBusinessRisk
            }.Render;
        }
    }

    /// <summary>DatePicker for FinalDate</summary>
    public string TxtFinalDate
    {
        get
        {
            return new FormDatePicker
            { 
                Id = "TxtFinalDate",
                ColumnsSpan = Constant.ColumnSpan6,
                GrantToWrite = this.ApplicationUser.HasGrantToWrite(ApplicationGrant.BusinessRisk),
                Value = this.businessRisk.FinalDate,
                Required = GrantToWriteBusinessRisk
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
                GrantToWrite = GrantToWriteIncidentAction,
                Label = this.Dictionary["Item_IncidentAction_Label_Description"],
                MaximumLength = 100,
                Name = "TxtActionDescription",
                Placeholder = this.Dictionary["Item_IncidentAction_Label_Description"],
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
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
                Label = this.Dictionary["Item_IncidentAction_Field_WhatHappened"],
                ColumnsSpan = Constant.ColumnSpan8,
                ColumnsSpanLabel = 12,
                Embedded = true,
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Item_IncidentAction_Field_Causes"],
                ColumnsSpan = Constant.ColumnSpan8,
                ColumnsSpanLabel = 12,
                Embedded = true,
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Item_IncidentAction_Field_Actions"],
                ColumnsSpan = Constant.ColumnSpan8,
                ColumnsSpanLabel = 12,
                Embedded = true,
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Item_IncidentAction_Field_Monitoring"],
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Item_IncidentAction_Field_Notes"],
                MaxLength = Constant.MaximumTextAreaLength,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.WhatHappenedOn,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.CausesOn,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Common_DateExecution"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.ActionsOn,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan8,
                Value = this.IncidentAction.ActionsSchedule,
                GrantToWrite = GrantToWriteIncidentAction
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
                Label = this.Dictionary["Common_Date"],
                ColumnsSpanLabel = Constant.ColumnSpan4,
                ColumnsSpan = Constant.ColumnSpan6,
                Value = this.IncidentAction.ClosedOn,
                GrantToWrite = GrantToWriteIncidentAction
            }.Render;
        }
    }
    #endregion

    /// <summary>
    /// Page's load event
    /// </summary>
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

    private BusinessRisk ById(int companyId, long id)
    {
        var res = BusinessRisk.Empty;
        string query = "BusinessRisk_GetById";
        using (var cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@Id", id));
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            res = new BusinessRisk()
                            {
                                Id = rdr.GetInt64(ColumnsBusinessRiskGetAll.Id),
                                Description = rdr.GetString(ColumnsBusinessRiskGetAll.Description),
                                Notes = rdr.GetString(ColumnsBusinessRiskGetAll.Notes),
                                CompanyId = companyId,
                                CreatedOn = rdr.GetDateTime(ColumnsBusinessRiskGetAll.CreatedOn),
                                CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsBusinessRiskGetAll.CreatedBy),
                                    UserName = rdr.GetString(ColumnsBusinessRiskGetAll.CreatedByName)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsBusinessRiskGetAll.ModifiedOn),
                                ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsBusinessRiskGetAll.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsBusinessRiskGetAll.ModifiedByName)
                                },
                                Code = rdr.GetInt64(ColumnsBusinessRiskGetAll.Code),
                                Rules = new Rules()
                                {
                                    Id = rdr.GetInt64(ColumnsBusinessRiskGetAll.RuleId),
                                    Description = rdr.GetString(ColumnsBusinessRiskGetAll.RuleDescription),
                                    Limit = rdr.GetInt64(ColumnsBusinessRiskGetAll.RuleRangeId)
                                },
                                ItemDescription = rdr.GetString(ColumnsBusinessRiskGetAll.ItemDescription),
                                StartControl = rdr.GetString(ColumnsBusinessRiskGetAll.StartControl)
                            };

                            res.Active = rdr.GetBoolean(ColumnsBusinessRiskGetAll.Active);
                            //res.InitialValue = rdr.GetInt32(ColumnsBusinessRiskGetAll.InitialValue);
                            res.DateStart = rdr.GetDateTime(ColumnsBusinessRiskGetAll.DateStart);
                            res.ProcessId = rdr.GetInt64(ColumnsBusinessRiskGetAll.ProcessId);
                            res.Assumed = rdr.GetBoolean(ColumnsBusinessRiskGetAll.Assumed);

                            res.StartAction = rdr.GetInt32(ColumnsBusinessRiskGetAll.StartAction);
                            res.StartProbability = rdr.GetInt32(ColumnsBusinessRiskGetAll.ProbabilityId);
                            res.StartSeverity = rdr.GetInt32(ColumnsBusinessRiskGetAll.Severity);
                            res.StartResult = rdr.GetInt32(ColumnsBusinessRiskGetAll.StartResult);

                            res.FinalProbability = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalProbability);
                            res.FinalSeverity = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalSeverity);
                            res.FinalResult = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalResult);
                            res.FinalAction = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalAction);

                            if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.FinalDate))
                            {
                                res.FinalDate = rdr.GetDateTime(ColumnsBusinessRiskGetAll.FinalDate);
                            }

                            if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.Causes))
                            {
                                res.Causes = rdr.GetString(ColumnsBusinessRiskGetAll.Causes);
                            }

                            //// Field can be Null in Database (no link to a previous BusinessRisk)
                            if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.PreviousBusinessRiskId))
                            {
                                res.PreviousBusinessRiskId = rdr.GetInt64(ColumnsBusinessRiskGetAll.PreviousBusinessRiskId);
                            }
                            else
                            {
                                //// The previous BusinessRisk does not exist
                                res.PreviousBusinessRiskId = -1;
                            }

                            res.ModifiedBy.Employee = Employee.ByUserId(res.ModifiedBy.Id);
                        }
                    }

                    res.Process = new Process(res.ProcessId, companyId);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        return res;
    }
    
    /// <summary>Main action to load page elements</summary>
    private void Go()
    {
        this.Company = this.Session["company"] as Company;
        this.Dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.ApplicationUser = this.Session["User"] as ApplicationUser;
        this.Session["User"] = this.ApplicationUser;

        this.GrantToWriteBusinessRisk = this.ApplicationUser.HasGrantToWrite(ApplicationGrant.BusinessRisk);
        this.GrantToWriteIncidentAction = this.ApplicationUser.HasGrantToWrite(ApplicationGrant.IncidentActions);

        if (this.Request.QueryString["id"] != null)
        {
            this.BusinessRiskId = Convert.ToInt64(this.Request.QueryString["id"]);
        }

        string label = "Item_BusinessRisk_Title_BusinessRiskDetails";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        if (ApplicationUser.HasGrantToRead(ApplicationGrant.BusinessRisk))
        {
            this.master.AddBreadCrumb("Item_BusinessRisks", "BusinessRisksList.aspx", Constant.NotLeaft);
        }
        else
        {
            this.master.AddBreadCrumb("Item_BusinessRisks");
        }

        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;
        if (!this.Page.IsPostBack)
        {
            //this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.businessRisk, TargetTypes.BusinessRisk);
        }

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnPrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success" });
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });
        
        this.formFooterActions = new FormFooter();
        this.formFooterActions.AddButton(new UIButton { Id = "BtnSave2", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooterActions.AddButton(new UIButton { Id = "BtnCancel2", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });
        
        if (this.BusinessRiskId != -1)
        {
            this.businessRisk = ById(this.Company.Id, this.BusinessRiskId);
            this.incidentAction = IncidentAction.ByBusinessRiskId(this.businessRisk.Id, this.Company.Id);
            if (this.businessRisk.CompanyId != this.Company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.businessRisk = BusinessRisk.Empty;
            }

            this.formFooter.ModifiedBy = this.businessRisk.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.businessRisk.ModifiedOn;
            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_BusinessRisk"], this.businessRisk.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.businessRisk = BusinessRisk.Empty;
            this.incidentAction = IncidentAction.Empty;
        }
        
        historyActionActive = false;
        RenderActionHistory();

        this.tabBar.AddTab(new Tab { Id = "home", Selected = true, Active = true, Label = this.Dictionary["Item_BusinessRisk_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab { Id = "accion", Available = this.ApplicationUser.HasGrantToRead(ApplicationGrant.IncidentActions), Active = true, Label = this.Dictionary["Item_BusinessRisk_Tab_Action"] });
        this.tabBar.AddTab(new Tab { Id = "costes", Available = this.ApplicationUser.HasGrantToRead(ApplicationGrant.Cost), Active = true, Label = this.Dictionary["Item_BusinessRisk_Tab_Costs"] });
        this.tabBar.AddTab(new Tab { Id = "graphic", Available = true, Active = true, Label = this.Dictionary["Item_BusinessRisk_Tab_Graphics"] });
        this.tabBar.AddTab(new Tab { Id = "historyActions", Available = true, Active = historyActionActive == true, Label = this.Dictionary["Item_BusinessRisk_Tab_HistoryActions"], Hidden = !historyActionActive });
        this.tabBar.AddTab(new Tab { Id = "uploadFiles", Available = true, Active = true, Label = this.Dictionary["Item_Learning_Tab_UploadFiles"], Hidden = this.businessRisk.Id < 1 });

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
        var processeCollection = Process.ByCompany(this.Company.Id);
        var processList = new StringBuilder();
        processList.Append(string.Format(CultureInfo.InvariantCulture,@"<option value=""0"">{0}</option>",this.Dictionary["Common_SelectOne"]));
        foreach (var process in processeCollection.OrderBy(process => process.Description))
        {
            processList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{1}</option>", process.Id, process.Description));
        }

        this.LTProcess.Text = processList.ToString();
    }

    /// <summary>Renders the selectable Rules</summary>
    private void RenderLimit()
    {
        var limitCollection = ProbabilitySeverityRange.All(this.Company.Id);
        var limitList = new StringBuilder();
        var probabilityList = new StringBuilder();
        var severityList = new StringBuilder();
        probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""0"">{0}</option>", this.Dictionary["Common_SelectOne"]));
        probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<optgroup label=""Probability"">", this.Dictionary["Common_SelectOne"]));
        severityList.Append(string.Format(CultureInfo.InvariantCulture, @"<optgroup label=""Severity"">", this.Dictionary["Common_SelectOne"]));
        foreach (var limit in limitCollection.OrderBy(limit => limit.Code))
        {
            if(limit.Type == ProbabilitySeverityRange.ProbabilitySeverityType.Probability) 
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
        var probabilitySeverityCollection = ProbabilitySeverityRange.GetActive(this.Company.Id);
        var severityList = new StringBuilder();
        severityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""0"">{0}</option>", this.Dictionary["Common_SelectOne"]));
        var probabilityList = new StringBuilder();
        probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""0"">{0}</option>", this.Dictionary["Common_SelectOne"]));
        foreach (var probabilitySeverity in probabilitySeverityCollection.OrderBy(probabilitySeverity => probabilitySeverity.Code))
        {
            if (probabilitySeverity.Type == ProbabilitySeverityRange.ProbabilitySeverityType.Severity)
            {
                severityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{0} - {1}</option>",probabilitySeverity.Code, probabilitySeverity.Description));
            }
            else
            {
                probabilityList.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"">{0} - {1}</option>",probabilitySeverity.Code, probabilitySeverity.Description));
            }
        }
    }

    private void RenderActionsForm()
    {
        var defaultOption = new FormSelectOption { Text = this.Dictionary["Common_SelectAll"], Value = "0" };
        this.ComboActionWhatHappenedResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleWhatHappend"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionWhatHappenedResponsible",
            GrantToWrite = GrantToWriteIncidentAction,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionCausesResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleCauses"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionCausesResponsible",
            GrantToWrite = GrantToWriteIncidentAction,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionActionsResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleActions"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionActionsResponsible",
            GrantToWrite = GrantToWriteIncidentAction,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionActionsExecuter = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_Responsible"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionActionsExecuter",
            GrantToWrite = GrantToWriteIncidentAction,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        this.ComboActionClosedResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan4,
            Label = this.Dictionary["Item_IncidentAction_Field_ResponsibleClose"],
            ColumnsSpan = Constant.ColumnSpan8,
            Name = "CmbActionClosedResponsible",
            GrantToWrite = GrantToWriteIncidentAction,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = defaultOption
        };

        long whatHappenedResponsibleId = this.IncidentAction.WhatHappenedBy == null ? 0 : this.IncidentAction.WhatHappenedBy.Id;
        long causesResponsibleId = this.IncidentAction.CausesBy == null ? 0 : this.IncidentAction.CausesBy.Id;
        long actionsResponsibleId = this.IncidentAction.ActionsBy == null ? 0 : this.IncidentAction.ActionsBy.Id;
        long actionsExecuterId = this.IncidentAction.ActionsExecuter == null ? 0 : this.IncidentAction.ActionsExecuter.Id;
        long closedResponsibleId = this.IncidentAction.ClosedBy == null ? 0 : this.IncidentAction.ClosedBy.Id;

        foreach (Employee e in this.Company.Employees)
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

    private void RenderRemainder()
    {
        var probabilitySeverityCollection = ProbabilitySeverityRange.GetActive(this.Company.Id);
        var severityList = new StringBuilder();
        var probabilityList = new StringBuilder();
        foreach (var probabilitySeverity in probabilitySeverityCollection.OrderBy(probabilitySeverity => probabilitySeverity.Code))
        {
            if (probabilitySeverity.Type == ProbabilitySeverityRange.ProbabilitySeverityType.Severity)
            {
                severityList.Append(
                    string.Format(CultureInfo.GetCultureInfo("en-us"),
                    @"<tr{2}><td><span>{0} - {1}</span></td><td><span id=""Severity{0}"" onclick=""SetSeverity({0},this);"" class=""btn btn-xs btn-success""><i class=""icon-star bigger-120""></i></span></td></tr>", 
                    probabilitySeverity.Code, 
                    probabilitySeverity.Description,
                    probabilitySeverity.Code == businessRisk.Severity ? " style=\"font-weight:bold;\"" : string.Empty));
            }
            else
            {
                probabilityList.Append(
                    string.Format(CultureInfo.GetCultureInfo("en-us"),
                    @"<tr{2}><td><span>{0} - {1}</span></td><td><span id=""Probability{0}"" onclick=""SetProbability({0},this);"" class=""btn btn-xs btn-success""><i class=""icon-star bigger-120""></i></span></td></tr>",
                    probabilitySeverity.Code, 
                    probabilitySeverity.Description,
                    probabilitySeverity.Code == businessRisk.Probability ? " style=\"font-weight:bold;\"" : string.Empty));
            }
        }

        this.LTProbabilityData.Text = probabilityList.ToString();
        this.LTSeverityData.Text = severityList.ToString();
    }

    private void RenderActionHistory()
    {
        var incidentActionCollection = BusinessRisk.FindHistoryAction(businessRisk.Code, this.Company.Id);
        var res = new StringBuilder();
        var searchItem = new List<string>();
        foreach (var incidentAction in incidentActionCollection.OrderBy(incidentAction => incidentAction.WhatHappenedOn))
        {
            if (!searchItem.Contains(incidentAction.Description))
            {
                searchItem.Add(incidentAction.Description);
            }

            res.Append(incidentAction.ListBusinessRiskRow(this.Dictionary, this.ApplicationUser.Grants));

            if (incidentAction.BusinessRiskId != businessRisk.Id)
            {
                historyActionActive = true;
            }

        }

        this.BusinessRiskActionData.Text = res.ToString();
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(18, this.BusinessRiskId, this.Company.Id).ToList();

        if (this.incidentAction.Id > 0)
        {
            files.AddRange(UploadFile.GetByItem(13, this.incidentAction.Id, this.Company.Id));
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