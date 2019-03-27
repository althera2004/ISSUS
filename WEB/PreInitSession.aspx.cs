// --------------------------------
// <copyright file="PreInitSession.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Web.UI;
using GisoFramework;

/// <summary>Implements PreInitSession page</summary>
public partial class PreInitSession : Page
{
    /// <summary>Gets or sets user identififer/summary>
    public string UserId { get; set; }

    /// <summary>Gets or sets user password</summary>
    public string CompanyId { get; set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string password = this.Request.Form["Password"].ToString();
        this.UserId = this.Request.Form["UserId"].ToString();
        this.CompanyId = this.Request.Form["CompanyId"].ToString();
        ApplicationUser.SetPassword(Convert.ToInt32(this.UserId), password);
    }
}