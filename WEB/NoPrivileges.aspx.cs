// --------------------------------
// <copyright file="NoPrivileges.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework.Item;

public partial class NoPrivileges : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets for fixed labels</summary>   
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.company = Session["company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        string label = "Common_NoGrants";
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;
    }
}