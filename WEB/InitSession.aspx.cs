// --------------------------------
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

/// <summary>Implements InitSession page</summary>
public partial class InitSession : Page
{
    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Session["UserId"] = this.Request.Form["UserId"];
        this.Session["CompanyId"] = this.Request.Form["CompanyId"];
        var company = new Company(Convert.ToInt32(this.Request.Form["CompanyId"]));
        this.Session["Company"] = company;
        var user = new ApplicationUser(Convert.ToInt32(this.Request.Form["UserId"]));
        if (user.CompanyId == 0)
        {
            user.CompanyId = Convert.ToInt32(this.Request.Form["CompanyId"]);
        }


        int loginId = 0;
        if (this.Request.Form["LoginId"] != null)
        {
            loginId = Convert.ToInt32(this.Request.Form["LoginId"]);
            this.Session["UniqueSessionId"] = UniqueSession.ReplaceUser(loginId, user.Id);
        }
        else
        {
            this.Session["UniqueSessionId"] = UniqueSession.SetSession(user.Id, string.Empty);
        }

        this.Session["User"] = user;
        this.Session["Navigation"] = new List<string>();

        var dictionary = ApplicationDictionary.Load("ca");
        if (user.Language != "ca")
        {
            dictionary = ApplicationDictionary.LoadNewLanguage(user.Language);
        }

        this.Session["AlertsDefinition"] = AlertDefinition.GetFromDisk;
        this.Session["Menu"] = MenuOption.GetMenu(user.Id, user.Admin);

        if (user.Grants.Count == 0)
        {
            this.Session["home"] = "NoGrants.aspx";
            this.Response.Redirect("NoGrants.aspx", Constant.EndResponse);
        }
        else
        {
            string landPage = user.Grants[0].Item.Page;
            if (!company.Agreement)
            {
                landPage = "/Agreement.aspx";
            }
            else if (true || user.HasGrantToRead(ApplicationGrant.CompanyProfile))
            {
                landPage = "/DashBoard.aspx";
            }

            Session["home"] = landPage;
            Response.Redirect(landPage, Constant.EndResponse);
        }

        Context.ApplicationInstance.CompleteRequest();
    }
}