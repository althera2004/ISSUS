﻿// --------------------------------
// <copyright file="InitSession.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Alerts;
using GisoFramework.Item;
using GisoFramework.UserInterface;
using SbrinnaCoreFramework;

/// <summary>
/// Implements InitSession page
/// </summary>
public partial class InitSession : Page
{
    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Session["UserId"] = this.Request.Form["UserId"];
        this.Session["CompanyId"] = this.Request.Form["CompanyId"];
        this.Session["Company"] = new Company(Convert.ToInt32(this.Request.Form["CompanyId"]));
        ApplicationUser user = new ApplicationUser(Convert.ToInt32(this.Request.Form["UserId"]));
        if (user.CompanyId == 0)
        {
            user.CompanyId = Convert.ToInt32(this.Request.Form["CompanyId"].ToString());
        }


        int loginId = 0;
        if (this.Request.Form["LoginId"] != null)
        {
            loginId = Convert.ToInt32(this.Request.Form["LoginId"].ToString());
            this.Session["UniqueSessionId"] = UniqueSession.ReplaceUser(loginId, user.Id);
        }
        else
        {
            this.Session["UniqueSessionId"] = UniqueSession.SetSession(user.Id, string.Empty);
        }

        this.Session["User"] = user;
        this.Session["Navigation"] = new List<string>();

        Dictionary<string, string> dictionary = ApplicationDictionary.Load("ca");
        if (user.Language != "ca")
        {
            dictionary = ApplicationDictionary.LoadNewLanguage(user.Language);
        }

        this.Session["AlertsDefinition"] = AlertDefinition.GetFromDisk(dictionary);
        this.Session["Menu"] = MenuOption.GetMenu(Convert.ToInt32(this.Request.Form["UserId"]));

        if (user.Grants.Count == 0)
        {
            this.Session["home"] = "NoGrants.aspx";
            this.Response.Redirect("NoGrants.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            if (true || user.HasGrantToRead(ApplicationGrant.CompanyProfile))
            {
                this.Session["home"] = "/DashBoard.aspx";
                Response.Redirect("/DashBoard.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Session["home"] = user.Grants[0].Item.Page;
                Response.Redirect(user.Grants[0].Item.Page, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}