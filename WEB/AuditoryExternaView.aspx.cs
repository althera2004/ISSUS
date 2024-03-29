﻿// --------------------------------
// <copyright file="AuditoryExternaView.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class AuditoryExternaView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private long auditoryId;

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string Zombie
    {
        get
        {
            return IncidentActionZombie.JsonList(IncidentActionZombie.ByAuditoryId(this.auditoryId, this.company.Id));
        }
    }

    public string DataJson
    {
        get
        {
            return this.Auditory.Json;
        }
    }

    public string ReportDateStart
    {
        get
        {
            if (this.Auditory.ReportStart == null)
            {
                return this.Dictionary["Item_Auditory_Text_NotStarted"];
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ReportStart);
        }
    }

    public string ReportDateEnd
    {
        get
        {
            if (this.Auditory.ReportEnd == null)
            {
                return this.Dictionary["Item_Auditory_Text_NotEnded"];
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ReportEnd);
        }
    }

    public string RulesIds { get; private set; }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public Auditory Auditory { get; private set; }

    public string TxtName
    {
        get
        {
            return new FormText
            {
                Name = "TxtName",
                Value = this.Auditory.Description,
                ColumnSpan = 10,
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
                MaximumLength = 100,
                Placeholder = this.Dictionary["Item_Auditory"],
                GrantToWrite = this.GrantToWrite
            }.Render;
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
            return this.user.HasGrantToWrite(ApplicationGrant.Questionary);
        }
    }

    public bool NewAuditory
    {
        get
        {
            return this.Auditory.Id < 1;
        }
    }

    public bool Admin
    {
        get
        {
            return this.user.Admin;
        }
    }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string ProvidersJson
    {
        get
        {
            return Provider.ByCompanyJson(this.company.Id);
        }
    }

    public string CustomersJson
    {
        get
        {
            return Customer.ByCompanyJson(this.company.Id);
        }
    }

    public BarPopup ProviderBarPopups { get; set; }

    public BarPopup CustomerBarPopups { get; set; }

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
        this.user = Session["User"] as ApplicationUser;
        this.company = Session["company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        // Security access control
        if (!this.user.HasGrantToRead(ApplicationGrant.Auditory))
        {
            this.Response.Redirect("NoPrivileges.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Parameters control
        if (this.Request.QueryString["id"] != null)
        {
            this.auditoryId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        this.master.formFooter = new FormFooter();
        this.master.formFooter.AddButton(new UIButton { Id = "BtnPrint", Icon = "icon-print", Text = this.Dictionary["Common_Print"], Action = "info", ColumnsSpan = 12 });

        if (this.user.HasGrantToWrite(ApplicationGrant.Auditory))
        {
            this.master.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
        }

        this.master.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        if (this.auditoryId > 0)
        {
            this.Auditory = Auditory.ById(this.auditoryId, this.company.Id);
            if (this.Auditory.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }

            //this.formFooter.ModifiedBy = this.Auditory.ModifiedBy.Description;
            //this.formFooter.ModifiedOn = this.Auditory.ModifiedOn;
            this.master.ModifiedBy = this.Auditory.ModifiedBy.Description;
            this.master.ModifiedOn = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Auditory.ModifiedOn);
            this.master.TitleInvariant = true;
            this.RenderDocuments();
        }
        else
        {
            this.Auditory = Auditory.Empty;
            if (this.Request.QueryString["t"] != null)
            {
                this.Auditory.Type = Convert.ToInt32(this.Request.QueryString["t"] as string);
            }

            this.master.ModifiedOn = Dictionary["Common_New"];
            this.master.ModifiedBy = "-";
        }

        if (!IsPostBack)
        {
            if (this.user.HasTraceGrant())
            {
                this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.auditoryId, TargetType.Auditory);
            }
        }

        string label = this.auditoryId == -1 ? this.Dictionary["Item_Auditory_BreadCrumb_Edit"] : string.Format("{0} {2}: <strong>{1}</strong>", this.Dictionary["Item_Auditory"], this.Auditory.Description, this.Dictionary["Item_Adutory_Type_Label_" + this.Auditory.Type.ToString()].ToLowerInvariant());
        this.master.AddBreadCrumb("Item_Auditories", "AuditoryList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumbInvariant(this.Dictionary["Item_Auditory_BreadCrumb_Edit"] + " " + this.Dictionary["Item_Adutory_Type_Label_" + this.Auditory.Type.ToString()].ToLowerInvariant());
        this.master.TitleInvariant = true;
        this.master.Titulo = label;
        this.FillLists();
        this.RenderRealActions();
        this.RenderBarPopups();
    }

    private void RenderBarPopups()
    {
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
            Description = this.Dictionary["Item_Provider"],
            FieldName = this.Dictionary["Common_Name"],
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
            Description = this.Dictionary["Item_Customer"],
            FieldName = this.Dictionary["Common_Name"],
            BarTitle = this.Dictionary["Item_Customers"]
        };
    }

    private void FillLists()
    {
        var rules = Rules.GetAll(this.company.Id);
        var rulesList = new StringBuilder("");
        bool firstRule = true;
        this.RulesIds = string.Empty;

        if (this.Auditory.PlannedOn != null)
        {
            rulesList.Append("<label class=\"col-sm-10 control-label\" style=\"text-align:left;\"><strong>");
        }
        else
        {
            rulesList.Append("<select class=\"col-sm-12\" id=\"CmbRules\" multiple=\"multiple\">");
        }

        foreach (var rule in rules.OrderBy(p => p.Description))
        {
            bool selected = this.Auditory.Rules.Any(r => r.Id == rule.Id);
            if (selected)
            {
                this.RulesIds += rule.Id + "|";
            }

            if (this.Auditory.PlannedOn != null)
            {
                if (selected)
                {
                    if (firstRule)
                    {
                        firstRule = false;
                    }
                    else
                    {
                        rulesList.Append(", ");
                    }

                    rulesList.Append(rule.Description);
                }
            }
            else
            {
                rulesList.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}""{2}>{1}</option>",
                    rule.Id,
                    rule.Description,
                    selected ? " selected=\"selected\"" : string.Empty);
            }
        }

        if (this.Auditory.PlannedOn != null)
        {
            rulesList.Append("</strong></label>");
        }
        else
        {
            rulesList.Append("</select>");
        }

        this.LtCmbRules.Text = rulesList.ToString();

        var employesList = new StringBuilder();
        var auditedList = new StringBuilder();
        var planningList = new StringBuilder();
        var auditorList = new StringBuilder();
        var closedList = new StringBuilder();
        var validatedList = new StringBuilder();
        employesList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        auditedList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        planningList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        auditorList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        closedList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        validatedList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        foreach (var employee in Employee.ByCompany(this.company.Id))
        {
            if (employee.DisabledDate == null || this.Auditory.InternalResponsible.Id == employee.Id)
            {
                employesList.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}""{2}>{1}</option>",
                    employee.Id,
                    employee.FullName,
                    this.Auditory.InternalResponsible.Id == employee.Id ? " selected=\"selected\"" : string.Empty);
            }

            if (employee.DisabledDate == null || this.Auditory.PlannedBy.Id == employee.Id)
            {
                planningList.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}""{2}>{1}</option>",
                    employee.Id,
                    employee.FullName,
                    this.Auditory.PlannedBy.Id == employee.Id ? " selected=\"selected\"" : string.Empty);
            }

            if (employee.DisabledDate == null || this.Auditory.ValidatedBy.Id == employee.Id)
            {
                validatedList.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}""{2}>{1}</option>",
                    employee.Id,
                    employee.FullName,
                    this.Auditory.ValidatedBy.Id == employee.Id ? " selected=\"selected\"" : string.Empty);
            }

            if (employee.DisabledDate == null)
            {
                auditedList.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}"">{1}</option>",
                    employee.Id,
                    employee.FullName);
                closedList.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}"">{1}</option>",
                    employee.Id,
                    employee.FullName);
                auditorList.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}"">{1}</option>",
                    employee.Id,
                    employee.FullName);
            }
        }

        this.LtCmbInternalResponsible.Text = employesList.ToString();
        this.LtClosedByList.Text = closedList.ToString();
        this.LtValidatedByList.Text = validatedList.ToString();
        this.LtWhatHappendByList.Text = validatedList.ToString();

        var addressList = new StringBuilder();
        employesList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        foreach (var address in CompanyAddress.GetAddressByCompanyId(this.company))
        {
            addressList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}""{2}>{1}</option>",
                address.Id,
                address.Description,
                this.Auditory.CompanyAddressId == address.Id ? " selected=\"selected\"" : string.Empty);
        }

        this.LtCmbAddress.Text = addressList.ToString();

        var providerList = new StringBuilder();
        providerList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        if (this.Auditory.Type == 1 || this.Auditory.Type == 2)
        {
            foreach (var provider in Provider.ByCompany(this.company.Id))
            {
                if (this.auditoryId > 0 || provider.Active)
                {
                    providerList.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<option value=""{0}""{2}>{1}</option>",
                        provider.Id,
                        provider.Description,
                        this.Auditory.Provider.Id == provider.Id ? " selected=\"selected\"" : string.Empty);
                }
            }
        }

        this.LtCmbProvider.Text = providerList.ToString();

        var customerList = new StringBuilder();
        customerList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        if (this.Auditory.Type == 1 || this.Auditory.Type == 2)
        {
            foreach (var customer in Customer.ByCompany(this.company.Id))
            {
                if (this.auditoryId > 0 || customer.Active)
                {
                    customerList.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<option value=""{0}""{2}>{1}</option>",
                        customer.Id,
                        customer.Description,
                        this.Auditory.Customer.Id == customer.Id ? " selected=\"selected\"" : string.Empty);
                }
            }
        }

        this.LtCmbCustomer.Text = customerList.ToString();
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(ItemIdentifiers.Auditory, this.auditoryId, this.company.Id);
        var res = new StringBuilder();
        var resList = new StringBuilder();
        int contCells = 0;
        var extensions = ToolsFile.ExtensionToShow;
        foreach (var file in files)
        {
            decimal finalSize = ToolsFile.FormatSize((decimal)file.Size);
            string fileShowed = string.IsNullOrEmpty(file.Description) ? file.FileName.Split('_')[2] : file.Description;
            if (fileShowed.Length > 25)
            {
                fileShowed = fileShowed.Substring(0, 25) + "...";
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
                            <div class=""col-sm-12"" style=""margin-bottom:8px;""><strong title=""{1}"">{9}</strong></div>
                            <div class=""col-sm-4""><img src=""/images/FileIcons/{2}.png"" /></div>
                            <div class=""col-sm-8 document-name"" style=""font-size:12px;"">   
                                {7}: <strong>{5:dd/MM/yyyy}</strong><br />
                                {8}: <strong>{6:#,##0.00} MB</strong>
                            </div>
                            <div class=""col-sm-12""></div>
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

    private void RenderRealActions()
    {
        if(this.Auditory.Status < 4)
        {
            return;
        }

        var res = new StringBuilder();
        var actions = IncidentAction.ByAuditoryId(this.auditoryId, this.company.Id);
        foreach(var action in actions)
        {
            res.Append(action.ListAuditoryRow(this.Dictionary, this.user.Grants));
        }

        this.LtActionsData.Text = res.ToString();
        this.LtActionsDataCount.Text = actions.Count.ToString();
    }
}