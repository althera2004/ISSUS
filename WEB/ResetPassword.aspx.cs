// --------------------------------
// <copyright file="ResetPassword.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;

public partial class ResetPassword : Page
{
    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string CompanyCode { get; private set; }

    public string LanguageBrowser { get; private set; }
    public string Ip { get; private set; }

    public string User { get; private set; }
    public string CompanyId { get; private set; }
    public string op { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form["UserId"] != null)
        {
            ApplicationUser user = new ApplicationUser(Convert.ToInt32(this.Request.Form["UserId"]));
            this.User = user.Json;
            this.Dictionary = ApplicationDictionary.Load(user.Language);
            Session["Dictionary"] = this.Dictionary;
        }
        if (Request.Form["Password"] != null)
        {
            this.op = Request.Form["Password"].ToString().Trim();
        }

        if (Request.Form["CompanyId"] != null)
        {
            Company company = new Company(Convert.ToInt32(Request.Form["CompanyId"].ToString()));
            this.LtCompnayName.Text = company.Name;
            this.ImgCompnayLogo.ImageUrl = string.Format("/images/Logos/{0}.jpg", company.Id);
            this.CompanyId = Request.Form["CompanyId"].ToString();
        }
        else
        {
            this.CompanyCode = string.Empty;
            this.TableCompany.Visible = false;
        }

        this.LanguageBrowser = this.Request.UserLanguages[0];
        this.Ip = this.GetUserIP();
    }

    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }

        return Request.ServerVariables["REMOTE_ADDR"];
    }
}