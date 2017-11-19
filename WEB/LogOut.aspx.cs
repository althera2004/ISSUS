// --------------------------------
// <copyright file="LogOut.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Web.UI;
using GisoFramework.Item;
using SbrinnaCoreFramework;

/// <summary>
/// Implements LogOut page
/// </summary>
public partial class LogOut : Page
{
    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        if (this.Request.QueryString["bye"] != null)
        {
            if ((now - (DateTime)Session["LastTime"]).Minutes < 30)
            {
                this.Response.Redirect(this.Request.UrlReferrer.AbsolutePath);
            }
        }

        this.Session["UserId"] = null;
        this.Session["CompanyId"] = null;
        this.Session["User"] = null;
        this.Session["IncidentFilter"] = null;
        this.Session["IncidentActionFilter"] = null;
        if (this.Request.QueryString["company"] != null)
        {
            this.Response.Redirect(string.Format("Default.aspx?company={0}", this.Request.QueryString["company"].ToString()), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        if (this.Session["Company"] != null)
        {
            Company company = Session["Company"] as Company;
            this.Session["Company"] = null;
            this.Response.Redirect(string.Format("Default.aspx?company={0}", company.Code), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        Context.ApplicationInstance.CompleteRequest();
    }
}