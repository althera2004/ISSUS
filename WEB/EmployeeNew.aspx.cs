// --------------------------------
// <copyright file="EmployeeNew.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework.Item;

public partial class EmployeeNew : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
             this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string label = this.dictionary["Item_Employee_Button_New"];
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb(this.dictionary["Item_Employees"], "EmployeesList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;
    }
}