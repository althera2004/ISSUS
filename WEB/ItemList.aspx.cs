// --------------------------------
// <copyright file="ItemList.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class ItemList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>
    /// Gets the dictionary for interface texts
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.master.Dictionary;
        }
    }

    /// <summary>
    /// Gets or sets header of items list
    /// </summary>
    public UIDataHeader DataHeader { get; set; }

    public bool AdminPage { get; set; }

    public int ItemType { get; set; }

    public string ItemName { get; set; }

    /// <summary>
    /// Event load of page
    /// </summary>
    /// <param name="sender">Page loaded</param>
    /// <param name="e">Events arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
             this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.Go();
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        if (this.Page.Request.QueryString["ItemType"] != null)
        {
            this.ItemType = Convert.ToInt32(this.Page.Request.QueryString["ItemType"]);
        }

        this.user = (ApplicationUser)Session["User"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = this.AdminPage;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        var item = Item.GetByCode(this.ItemType, this.dictionary);
        this.ItemName = item.ItemType;
        this.master.AddBreadCrumbInvariant(item.NamePlural);
        this.master.Titulo = item.NamePlural;
        this.master.TitleInvariant = true;

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = item.NewButton;
        }

        this.DataHeader = item.Headers[0];
    }
}