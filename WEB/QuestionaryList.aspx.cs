﻿// --------------------------------
// <copyright file="QuestionaryList.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

/// <summary>Implements questionaries list page</summary>
public partial class QuestionaryList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    public ApplicationUser ApplicationUser { get; private set; }

    /// <summary>Company of session</summary>
    public Company Company { get; private set; }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string Filter { get; set; }

    public long IncidentId { get; set; }

    public IncidentAction Incident { get; set; }

    public string ProcessList
    {
        get
        {
            return Process.ByCompanyJson(this.Company.Id);
        }
    }

    public string RulesList
    {
        get
        {
            return Rules.GetAllJson(this.Company.Id);
        }
    }

    public string QuestionaryJsonList
    {
        get
        {
            return Questionary.JsonList(Questionary.All(this.Company.Id));
        }
    }

    public string ApartadosNormasList
    {
        get
        {
            return ApartadoNorma.JsonList(this.Company.Id);
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
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.ApplicationUser = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.ApplicationUser.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.ApplicationUser = (ApplicationUser)Session["User"];
        this.Company = (Company)Session["company"];

        if (Session["QuestionaryFilter"] == null)
        {
            this.Filter = "null";
        }
        else
        {
            this.Filter = Session["QuestionaryFilter"].ToString();
        }

        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Questionaries");
        this.master.Titulo = "Item_Questionaries";

        if (this.ApplicationUser.HasGrantToWrite(ApplicationGrant.Incident))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Questionary_Button_New", "QuestionaryView.aspx");
        }
    }
}