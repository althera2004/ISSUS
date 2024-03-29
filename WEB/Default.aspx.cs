﻿using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;

public partial class _Default : Page
{
    public string IssusVersion
    {
        get
        {
            return ConfigurationManager.AppSettings["issusversion"];
        }
    }

    public string BK
    {
        get
        {
            string path = this.Request.PhysicalApplicationPath;
            if(!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}WelcomeBackgrounds\", path);            
            var files = Directory.GetFiles(path);
            int index = new Random().Next(0, files.Count()-1);
            string res = Path.GetFileName(files[index]);
            Session["BK"] = res;
            return res;
        }
    }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string LanguageBrowser { get; private set; }

    public string IP { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Navigation"] = null;
        if (this.Request.UserLanguages != null)
        {
            this.LanguageBrowser = this.Request.UserLanguages[0];
        }

        this.IP = this.GetUserIP();
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