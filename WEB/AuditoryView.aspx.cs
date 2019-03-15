// --------------------------------
// <copyright file="QuestionaryView.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.DataAccess;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;
public partial class AuditoryView : Page
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
            if(this.Auditory.ReportStart == null)
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

    public string Improvements
    {
        get
        {
            return AuditoryCuestionarioImprovement.JsonList(this.Auditory.Improvements);
        }
    }

    public string Founds
    {
        get
        {
            return AuditoryCuestionarioFound.JsonList(this.Auditory.Founds);
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

    private FormFooter formFooter;

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
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

    public string Planning
    {
        get
        {
            return AuditoryPlanning.JsonList(AuditoryPlanning.ByAuditory(this.auditoryId, this.company.Id));
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

        this.formFooter = new FormFooter();
        if (this.user.HasGrantToWrite(ApplicationGrant.Auditory))
        {
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
        }

        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        if (this.auditoryId > 0)
        {
            this.Auditory = Auditory.ById(this.auditoryId, this.company.Id);
            if (this.Auditory.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }

            this.formFooter.ModifiedBy = this.Auditory.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Auditory.ModifiedOn;
            this.master.TitleInvariant = true;
        }
        else
        {
            this.Auditory = Auditory.Empty;
            if (this.Request.QueryString["t"] != null)
            {
                this.Auditory.Type = Convert.ToInt32(this.Request.QueryString["t"] as string);
            }
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

        if(this.Auditory.Status > 0)
        {
            this.RenderCuestionarios();
        }
    }

    private void FillLists()
    {
        var processes = Process.ByCompany(this.company.Id);
        var processList = new StringBuilder("<option>").Append(this.Dictionary["Common_SelectOne"]).Append("</option>");
        foreach (var process in processes.OrderBy(p => p.Description))
        {
            processList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"">{1}</option>",
                process.Id,
                process.Description);
        }

        this.LtProcessList.Text = processList.ToString();


        var rules = Rules.GetAll(this.company.Id);
        var rulesList = new StringBuilder("");
        bool firstRule = true;
        this.RulesIds = string.Empty;

        if(this.Auditory.PlannedOn != null) { rulesList.Append("<label class=\"col-sm-10 control-label\" style=\"text-align:left;\"><strong>"); }
        else { rulesList.Append("<select class=\"col-sm-12\" id=\"CmbRules\" multiple=\"multiple\">"); }
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

        if (this.Auditory.PlannedOn != null) { rulesList.Append("</strong></label>"); }
        else { rulesList.Append("</select>"); }
        this.LtCmbRules.Text = rulesList.ToString();

        var employesList = new StringBuilder();
        var auditedList = new StringBuilder();
        var planningList = new StringBuilder();
        employesList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        auditedList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        planningList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        foreach (var employee in Employee.ByCompany(this.company.Id))
        {
            employesList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}""{2}>{1}</option>",
                employee.Id,
                employee.FullName,
                this.Auditory.InternalResponsible.Id == employee.Id ? " selected=\"selected\"" : string.Empty);
            planningList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}""{2}>{1}</option>",
                employee.Id,
                employee.FullName,
                this.Auditory.PlannedBy.Id == employee.Id ? " selected=\"selected\"" : string.Empty);
            auditedList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"">{1}</option>",
                employee.Id,
                employee.FullName);
        }

        this.LtCmbInternalResponsible.Text = employesList.ToString();
        this.LtAuditedList.Text = auditedList.ToString();
        this.LtAuditoryPlanningResponsible.Text = planningList.ToString();

        var addressList = new StringBuilder();
        employesList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        foreach(var address in CompanyAddress.GetAddressByCompanyId(this.company))
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
            providerList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
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
        this.LtCmbProvider2.Text = providerList.ToString();

        var customerList = new StringBuilder();
        customerList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
        if (this.Auditory.Type == 1 || this.Auditory.Type == 2)
        {
            customerList.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""-1"">{0}</option>", this.Dictionary["Common_SelectOne"]);
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

    private void RenderCuestionarios()
    {
        var res = new StringBuilder();

        using(var cmd = new SqlCommand("Auditory_GetQuestionaries"))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.auditoryId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.company.Id));
                try
                {
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"<tr style=""height:30px;"">
                                    <td style=""height:30px;width:120px;text-align:center;
                                        background: -webkit-linear-gradient(left, #cfc {4}%, #fcc {4}%);
                                        background: -moz-linear-gradient(left, #cfc {4}%, #fcc {4}%);
                                        background: -ms-linear-gradient(left, #cfc {4}%, #fcc {4}%);
                                        background: linear-gradient(left, #cfc {4}%, #fcc {4}%);"">
                                        <strong>{0} / {1}</strong></td>
                                    <td>{2}</td>
                                    <td style=""width:50px;text-align:center;"">
                                      <span class=""btn btn-xs btn-success"" id=""{3}"" title=""{5}"" onclick=""QuestionaryPlay({6},{3});"">
                                        <i class=""icon-play bigger-120""></i>
                                      </span>
                                    </td>
                                </tr>",
                                rdr.GetInt32(3),
                                rdr.GetInt32(2),
                                rdr.GetString(1),
                                rdr.GetInt64(0),
                                rdr.GetInt32(3) * 100 / rdr.GetInt32(2),
                                rdr.GetInt32(3) > 0 ? this.Dictionary["Item_AuditoryQuestionary_Tooltip_Continue"] : this.Dictionary["Item_AuditoryQuestionary_Tooltip_Start"],
                                this.auditoryId);
                        }
                    }
                }
                finally
                {
                    if(cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }


        this.LtCuestionariosList.Text = res.ToString();
    }
}