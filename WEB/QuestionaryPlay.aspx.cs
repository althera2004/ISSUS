// --------------------------------
// <copyright file="QuestionaryPlay.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

/// <summary>Implements the questionnaire filling page</summary>
public partial class QuestionaryPlay : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Footer of page</summary>
    private FormFooter formFooter;

    /// <summary>Identifier of questionnaire</summary>
    private long questionnaireId;

    /// <summary>Identifier of auditory</summary>
    private long auditoryId;

    /// <summary>Observations of questionnaire</summary>
    private AuditoryCuestionarioObservations observations;

    /// <summary>Gets a JSON structure of questionnaire's observations</summary>
    public string Observations
    {
        get
        {
            return this.observations.Json;
        }
    }

    /// <summary>Gets questionnaire's auditory</summary>
    public Auditory Auditory { get; private set; }

    /// <summary>Gets a value indicating whether if user has online help</summary>
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

    /// <summary>Gets footer of page</summary>
    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
        }
    }

    /// <summary>Gets questionnaire</summary>
    public Questionary Questionary { get; private set; }

    /// <summary>Gets a value indicating whether is user has grants trace</summary>
    public bool GrantTraces
    {
        get
        {
            return this.user.HasTraceGrant();
        }
    }

    /// <summary>Gets a value indicating whether if actual user has write grant</summary>
    public bool GrantToWrite
    {
        get
        {
            return this.user.HasGrantToWrite(ApplicationGrant.Questionary);
        }
    }

    /// <summary>Gets a value indicating whether actual user is administrator</summary>
    public bool Admin
    {
        get
        {
            return this.user.Admin;
        }
    }

    /// <summary>Gets a value indicating whether questionnaire is editable</summary>
    public string Editable
    {
        get
        {
            if (this.Request.QueryString["e"] == "1")
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

    /// <summary>Gets questionnaire's founds</summary>
    public string Founds
    {
        get
        {
            return AuditoryCuestionarioFound.JsonList(AuditoryCuestionarioFound.ByCuestionary(this.questionnaireId, this.auditoryId, this.company.Id));
        }
    }

    /// <summary>Gets questionnaire's improvements</summary>
    public string Improvements
    {
        get
        {
            return AuditoryCuestionarioImprovement.JsonList(AuditoryCuestionarioImprovement.ByCuestionary(this.questionnaireId, this.auditoryId, this.company.Id));
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
            this.questionnaireId = Convert.ToInt32(this.Request.QueryString["c"]);
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

        if (this.questionnaireId > 0)
        {
            this.Questionary = Questionary.ById(this.questionnaireId, this.company.Id);
            if (this.Questionary.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }

            //this.formFooter.ModifiedBy = this.Questionary.ModifiedBy.Description;
            //this.formFooter.ModifiedOn = this.Questionary.ModifiedOn;
            this.master.ModifiedBy = this.Questionary.ModifiedBy.Description;
            this.master.ModifiedOn = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Questionary.ModifiedOn);
            this.master.TitleInvariant = true;

            this.observations = AuditoryCuestionarioObservations.ById(this.questionnaireId, this.auditoryId, this.company.Id);
        }
        else
        {
            this.Questionary = Questionary.Empty;
            this.master.ModifiedBy = Dictionary["Common_New"];
            this.master.ModifiedOn = "-";
        }

        string label = this.questionnaireId == -1 ? "Item_Questionary_BreadCrumb_Edit" : string.Format("{0}: <strong>{1}</strong>", this.Dictionary["Item_Questionary"], this.Questionary.Description);
        this.master.AddBreadCrumb("Item_Questionaries", "QuestionaryList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb("Item_Questionary_BreadCrumb_Edit");
        this.master.Titulo = label;

        this.FillLists();
    }

    /// <summary>Populates questions list</summary>
    private void FillLists()
    {
        var res = new StringBuilder();

        foreach (var q in AuditoryQuestionaryQuestion.ByAuditoryId(this.auditoryId, this.questionnaireId, this.company.Id))
        {
            var color = "333";
            var label = "-";
            var value = "0";
            if (q.Compliant.HasValue)
            {
                switch (q.Compliant.Value)
                {
                    case 1: color = "070"; label = "Cumple"; value = "1"; break;
                    case 2: color = "700"; label = "No cumple"; value = "2"; break;
                    case 3: color = "777"; label = "N/A"; value = "3"; break;
                }
            }

            var rowPattern = @"
                <tr id=""RQ{0}"">
                    <td style=""vertical-align:top;"">{1}</td>
                    <td style=""vertical-align:top;width:157px;text-align:center;"">
                        <span id=""Q{0}"" style=""color:#{2};cursor:pointer;"" onclick=""Toggle(this);"" data-status=""{4}"">{3}</span>
                    </td>
                </tr>";
                res.AppendFormat(
                 CultureInfo.InvariantCulture,
                 rowPattern,
                 q.Id,
                 q.Description,
                 color,
                 label,
                 value);
             //q.Compliant.HasValue ? (q.Compliant.Value == true ? "070" : "700") : "333",
             //q.Compliant.HasValue ? (q.Compliant.Value == true ? "Cumple" : "No cumple") : "-",
             //q.Compliant.HasValue ? (q.Compliant.Value == true ? "1" : "2") : "0");
        }

        this.LtQuestions.Text = res.ToString();
    }
}