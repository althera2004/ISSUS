// --------------------------------
// <copyright file="QuestionaryPlay.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class QuestionaryPlay : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private long questionaryId;
    private long auditoryId;

    public Auditory Auditory { get; private set; }

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
            return this.formFooter.Render(this.Dictionary);
        }
    }

    public Questionary Questionary { get; private set; }

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

    public bool Admin
    {
        get
        {
            return this.user.Admin;
        }
    }

    public string Editable
    {
        get
        {
            if(this.Request.QueryString["e"] == "1")
            {
                return "true";
            }
            else
            {
                return "false";
            }

        }
    }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string Founds
    {
        get
        {
            return AuditoryCuestionarioFound.JsonList(AuditoryCuestionarioFound.ByCuestionary(this.questionaryId, this.auditoryId, this.company.Id));
        }
    }

    public string Improvements
    {
        get
        {
            return AuditoryCuestionarioImprovement.JsonList(AuditoryCuestionarioImprovement.ByCuestionary(this.questionaryId, this.auditoryId, this.company.Id));
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
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
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
        if (!this.user.HasGrantToRead(ApplicationGrant.Questionary))
        {
            this.Response.Redirect("NoPrivileges.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Parameters control
        if (this.Request.QueryString["c"] != null)
        {
            this.questionaryId = Convert.ToInt32(this.Request.QueryString["c"]);
        }

        this.Auditory = Auditory.Empty;
        if (this.Request.QueryString["a"] != null)
        {
            this.auditoryId = Convert.ToInt32(this.Request.QueryString["a"]);
            this.Auditory = Auditory.ById(this.auditoryId, this.company.Id);
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        this.formFooter = new FormFooter();

        if (this.questionaryId > 0)
        {
            this.Questionary = Questionary.ById(this.questionaryId, this.company.Id);
            if (this.Questionary.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }

            this.formFooter.ModifiedBy = this.Questionary.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Questionary.ModifiedOn;
            this.master.TitleInvariant = true;
        }
        else
        {
            this.Questionary = Questionary.Empty;
        }

        string label = this.questionaryId == -1 ? "Item_Questionary_BreadCrumb_Edit" : string.Format("{0}: <strong>{1}</strong>", this.Dictionary["Item_Questionary"], this.Questionary.Description);
        this.master.AddBreadCrumb("Item_Questionaries", "QuestionaryList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb("Item_Questionary_BreadCrumb_Edit");
        this.master.Titulo = label;

        this.FillLists();
    }

    private void FillLists()
    {
        var res = new StringBuilder();
        foreach (var q in AuditoryQuestionaryQuestion.ByAuditoryId(this.auditoryId, this.questionaryId, this.company.Id))
        {
            res.AppendFormat(
             CultureInfo.InvariantCulture,
             @"
                <tr id=""RQ{0}"">
                    <td style=""vertical-align:top;"">{1}</td>
                    <td style=""vertical-align:top;width:157px;text-align:center;"">
                        <span id=""Q{0}"" style=""color:#{2};cursor:pointer;"" onclick=""Toggle(this);"" data-status=""{4}"">{3}</span>
                    </td>
                </tr>",
             q.Id,
             q.Description,
             q.Compliant.HasValue ? (q.Compliant.Value == true ? "070" : "700") : "333",
             q.Compliant.HasValue ? (q.Compliant.Value == true ? "Cumple" : "No cumple") : "-",
             q.Compliant.HasValue ? (q.Compliant.Value == true ? "1" : "2") : "0");
        }

        this.LtQuestions.Text = res.ToString();
    }
}