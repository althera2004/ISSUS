// --------------------------------
// <copyright file="ObjetivoList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class ObjetivoList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string UserLanguage
    {
        get
        {
            return this.user.Language;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public string Filter { get; set; }

    public UIDataHeader DataHeader { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
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
        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];

        if (this.Session["ObjetivoFilter"] == null)
        {
            this.Filter = "null";
        }
        else
        {
            this.Filter = Session["ObjetivoFilter"].ToString();
        }

        this.dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AddBreadCrumb("Item_Objetivos");
        this.master.Titulo = "Item_Objetivos";

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Objetivo_Button_New", "ObjetivoView.aspx");
        }
    }
}