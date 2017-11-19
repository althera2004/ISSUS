// --------------------------------
// <copyright file="Data.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;

/// <summary>
/// Implements JavaScript generator for data page
/// </summary>
public partial class js_Data : Page
{
    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime d1 = DateTime.Now;
        Company company = Session["company"] as Company;
        ApplicationUser user = Session["user"] as ApplicationUser;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.ContentType = "text/javascript";

        this.Response.Write(string.Format(CultureInfo.GetCultureInfo("es-es"), @"var user = {0};", user.Json));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        string userCulture = string.Empty;
        switch (company.Language)
        {
            case "es":
                userCulture = "es-ES";
                break;
            case "ca":
                userCulture = "es-CA";
                break;
            case "en":
                userCulture = "en-US";
                break;
        }

        switch (user.Language)
        {
            case "es":
                userCulture = "es-ES";
                break;
            case "ca":
                userCulture = "es-CA";
                break;
            case "en":
                userCulture = "en-US";
                break;
        }

        this.Response.Write(string.Format(CultureInfo.GetCultureInfo("es-es"), @"var UserCulture = ""{0}"";", userCulture));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        this.Response.Write(string.Format(CultureInfo.GetCultureInfo("es-es"), @"var Company = {0};", Company.Json(company)));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        StringBuilder res = new StringBuilder();
        ReadOnlyCollection<JobPosition> cargos = JobPosition.JobsPositionByCompany((Company)Session["Company"]);
        bool first = true;
        foreach (JobPosition cargo in cargos)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                res.Append(",");
            }

            res.Append(cargo.JsonSimple);
        }

        this.Response.Write(string.Format(CultureInfo.GetCultureInfo("es-es"), @"var jobPositionCompany ={0}[{1}{0}];", Environment.NewLine, res.ToString()));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        StringBuilder processTypeList = new StringBuilder("[");
        bool firstProcessType = true;
        foreach (ProcessType processType in ProcessType.ObtainByCompany(company.Id, this.dictionary))
        {
            if (processType.Active)
            {
                if (firstProcessType)
                {
                    firstProcessType = false;
                }
                else
                {
                    processTypeList.Append(",");
                }

                processTypeList.Append(Environment.NewLine).Append("    ").Append(processType.Json);
            }
        }

        processTypeList.Append(Environment.NewLine).Append("]");

        this.Response.Write("var processTypeCompany = " + Environment.NewLine + processTypeList.ToString() + ";");

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        StringBuilder departmentsCompanyJson = new StringBuilder("[");
        bool firstDepartment = true;
        foreach (Department department in Department.GetByCompany(company.Id))
        {
            if (!department.Deleted)
            {
                if (firstDepartment)
                {
                    firstDepartment = false;
                }
                else
                {
                    departmentsCompanyJson.Append(",");
                }

                departmentsCompanyJson.Append(Environment.NewLine).Append("    ").Append(department.JsonKeyValue);
            }
        }

        departmentsCompanyJson.Append(Environment.NewLine).Append("]");

        this.Response.Write("var departmentsCompany = " + Environment.NewLine + departmentsCompanyJson.ToString() + ";");

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);
        
        this.Response.Write("var Dictionary =" + Environment.NewLine);
        this.Response.Write("{" + Environment.NewLine);

        foreach (KeyValuePair<string, string> item in this.dictionary)
        {
            if (!item.Key.StartsWith("Help_") || true)
            {
                this.Response.Write(this.DictionaryItem(item.Key.Replace(' ', '_'), item.Value));
            }
        }

        this.Response.Write("    \"-\": \"-\"" + Environment.NewLine);
        this.Response.Write("};");
    }

    /// <summary>
    /// Gets a entry for dictionary JSON
    /// </summary>
    /// <param name="key">Item key</param>
    /// <param name="value">Item value</param>
    /// <returns>JSON Code</returns>
    private string DictionaryItem(string key, string value)
    {
        return string.Format(CultureInfo.GetCultureInfo("es-es"), @"    ""{0}"": ""{1}"",{2}", key, value, Environment.NewLine);
    }
}