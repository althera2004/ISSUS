// --------------------------------
// <copyright file="NoGrants.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;

/// <summary>Implements No grants page</summary>
public partial class NoGrants : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>Event load of page</summary>
    /// <param name="sender">Page loades</param>
    /// <param name="e">Arguments of event</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.user = Session["User"] as ApplicationUser;
        this.Company = Session["company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Common_NoGrants");
        this.master.Titulo = "Common_NoGrants";
    }
}