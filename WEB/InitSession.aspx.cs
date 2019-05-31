// --------------------------------
// <copyright file="InitSession.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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

        // Filtros predeterminado
        this.Session["TasksFilter"] = @"{""Owners"":true,""Others"":true,""Passed"": false}";
        this.Session["BusinessRiskFilter"] = @"{""companyId"": 0,""from"": null,""to"": null,""rulesId"": 0,""processId"": 0,""itemType"": 0,""type"": 0}";
        this.Session["AuditoryFilter"] = @"{""companyId"": 0, ""externa"": true, ""from"": null, ""interna"": true, ""provider"": true, ""status0"": true, ""status1"": true, ""status2"": true, ""status3"": true, ""status4"": true, ""status5"": false, ""to"": null }";
        this.Session["QuestionaryFilter"] = @"{""apartadoNorma"": ""-1"", ""companyId"": 0,""processId"": -1,""ruleId"": -1}";
        this.Session["ProcessFilter"] = @"AI";
        this.Session["EquipmentFilter"] = @"CVM|1";
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
                if (user.PrimaryUser)
                {
                    landPage = "/Agreement.aspx";
                }
                else
                {
                    landPage = "/AgreementNotice.aspx";
                }
            }
            else //if (true || user.HasGrantToRead(ApplicationGrant.CompanyProfile))
            {
                landPage = "/DashBoard.aspx";
            }

            Session["home"] = landPage;
            Response.Redirect(landPage, Constant.EndResponse);
        }

        Context.ApplicationInstance.CompleteRequest();
    }
}