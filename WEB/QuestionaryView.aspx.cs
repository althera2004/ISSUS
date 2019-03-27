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
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class QuestionaryView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private long questionaryId;

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

    public string TxtName
    {
        get
        {
            return new FormText
            {
                Name = "TxtName",
                Value = this.Questionary.Description,
                ColumnSpan = 10,
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
                MaximumLength = 100,
                Placeholder = this.Dictionary["Item_Questionary"],
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

    public string Questions
    {
        get
        {
            return QuestionaryQuestion.ByQuestionaryIdJson(this.questionaryId, this.company.Id);
        }
    }

    public string ApartadosNormasList
    {
        get
        {
            return ApartadoNorma.JsonList(this.company.Id);
        }
    }

    public bool NewQuestionary
    {
        get
        {
            return this.Questionary.Id < 1;
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
        if (!this.user.HasGrantToRead(ApplicationGrant.Questionary))
        {
            this.Response.Redirect("NoPrivileges.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Parameters control
        if (this.Request.QueryString["id"] != null)
        {
            this.questionaryId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        this.formFooter = new FormFooter();
        if (this.user.HasGrantToWrite(ApplicationGrant.Questionary))
        {
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
        }

        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

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

        if (!IsPostBack)
        {
            if (this.user.HasTraceGrant())
            {
                this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.questionaryId, TargetType.Questionary);
            }
        }

        string label = this.questionaryId == -1 ? "Item_Questionary_BreadCrumb_Edit" : string.Format("{0}: <strong>{1}</strong>", this.Dictionary["Item_Questionary"], this.Questionary.Description);
        this.master.AddBreadCrumb("Item_Questionaries", "QuestionaryList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb("Item_Questionary_BreadCrumb_Edit");
        this.master.Titulo = label;

        this.FillLists();
    }

    private void FillLists()
    {
        // ---- Procesos ----------------
        var processes = Process.ByCompany(this.company.Id);
        var processList = new StringBuilder("<option value=\"-1\">").Append(this.Dictionary["Common_SelectOne"]).Append("</option>");
        foreach (var process in processes.OrderBy(p => p.Description))
        {
            processList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}""{2}>{1}</option>",
                process.Id,
                process.Description,
                process.Id == this.Questionary.Process.Id ? " selected=\"selected\"" : string.Empty);
        }

        this.LtProcessList.Text = processList.ToString();

        // ---- Normas ------------------
        var rules = Rules.GetAll(this.company.Id);
        var rulesList = new StringBuilder("<option value=\"-1\">").Append(this.Dictionary["Common_SelectOne"]).Append("</option>");
        foreach (var rule in rules.OrderBy(p => p.Description))
        {
            rulesList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}""{2}>{1}</option>",
                rule.Id,
                rule.Description,
                rule.Id == this.Questionary.Rule.Id ? " selected=\"selected\"" : string.Empty);
        }

        this.LtRulesList.Text = rulesList.ToString();
    }
}