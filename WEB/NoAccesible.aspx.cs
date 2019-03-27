// --------------------------------
// <copyright file="NoAccesible.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework.Item;

/// <summary>Implements NoAccesible page</summary>
public partial class NoAccesible : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets dictionary for fixed labels</summary>
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
        string label = "Item_User_List_Header_UserName";
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;
    }
}