// --------------------------------
// <copyright file="RulesView.aspx.cs" company="Sbrinna">
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
using System.Globalization;
using SbrinnaCoreFramework;

public partial class RulesView : Page
{
    /// <summary>Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private FormFooter formFooter;

    /// <summary>Company of session</summary>
    public Company company { get; set; }

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

    public bool GrantTraces
    {
        get
        {
            return this.user.HasGrantToRead(ApplicationGrant.Rule);
        }
    }

    public string RulesJson
    {
        get
        {
            return Rules.GetAllJson(this.company.Id);
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

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    private long RuleId;

    public Rules Rule { get; set; }

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
        if (!this.user.HasGrantToRead(ApplicationGrant.Rule))
        {
            this.Response.Redirect("NoPrivileges.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Parameters control
        if (this.Request.QueryString["id"] != null)
        {
            this.RuleId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        this.formFooter = new FormFooter();
        if (this.user.HasGrantToWrite(ApplicationGrant.Rule))
        {
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
        }

        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (this.RuleId > 0)
        {
            this.Rule = Rules.GetById(this.company.Id, this.RuleId);
            if (this.Rule.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }

            this.formFooter.ModifiedBy = this.Rule.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Rule.ModifiedOn;
            this.master.TitleInvariant = true;
        }
        else
        {
            this.Rule = Rules.Empty;
        }

        if (!IsPostBack)
        {
            if (this.user.HasTraceGrant())
            {
                this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.RuleId, TargetType.Rules);
            }
        }

        string label = this.RuleId == -1 ? "Item_Rules_Button_New" : string.Format("{0}: <strong>{1}</strong>", this.dictionary["Item_Rule"], this.Rule.Description);
        this.master.AddBreadCrumb("Item_Rules", "RulesList.aspx", false);
        this.master.AddBreadCrumb("Item_Rule");
        this.master.Titulo = label;

        this.RenderBusinessRiskTable();
    }

    private void RenderBusinessRiskTable()
    {
        int total = 0;
        if (this.RuleId > 0)
        {
            StringBuilder res = new StringBuilder();
            ReadOnlyCollection<BusinessRisk> risks = BusinessRisk.GetByRulesId(this.RuleId, this.company.Id);
            if (risks.Count > 0)
            {
                foreach (BusinessRisk risk in risks)
                {
                    long result = risk.FinalResult;
                    if (result == 0)
                    {
                        result = risk.StartResult;
                    }

                    string status = string.Empty;
                    string color = "#000";

                    if (risk.Assumed || risk.FinalAction == 1)
                    {
                        status = this.Dictionary["Item_BusinessRisk_Status_Assumed"];
                        color = "#ffb752";
                    }
                    else if (result == 0)
                    {
                        status = this.Dictionary["Item_BusinessRisk_Status_Unevaluated"];
                        color = "#777777";
                    }
                    else
                    {
                        if (result < this.Rule.Limit)
                        {
                            status = this.dictionary["Item_BusinessRisk_Status_NotSignificant"];
                            color = "#87b87f";
                        }
                        else
                        {
                            status = this.dictionary["Item_BusinessRisk_Status_Significant"];
                            color = "#d15b47";
                        }
                    }

                    total++;
                    res.AppendFormat(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"<tr><td style=""display:none;width:90px;"">{0}</td><td>{1}</td><td style=""width:120px;font-weight:bold;color:{4}"">{2}</td><td style=""Width:70px;"" align=""right"">{3}</td></tr>",
                        risk.LinkCode,
                        risk.LinkList,
                        status,
                        result == 0 ? string.Empty : result.ToString(),
                        color);
                }
            }
            else
            {
                res.AppendFormat(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<td colspan=""4"" align=""center"" style=""background-color:#ddddff;color:#0000aa;""><table style=""border:none;""><tbody><tr><td rowspan=""2"" style=""border:none;""><i class=""icon-info-sign"" style=""font-size:48px;""></i></td><td style=""border:none;""><h4>{0}</h4></td></tr></tbody></table></td>",
                    this.dictionary["Item_Rules_Section_BusinessRisk_NoData"]
                );
            }

            this.TableBusiness.Text = res.ToString();
            this.TotalData.Text = total.ToString();
        }
    }
}