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
    private Dictionary<string, string> Dictionary;

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        var d1 = DateTime.Now;
        var company = Session["company"] as Company;
        var user = Session["user"] as ApplicationUser;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string userCulture = UsedCulture(company, user);

        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.ContentType = "text/javascript";

        this.Response.Write(string.Format(CultureInfo.InvariantCulture, @"var user = {0};", user.Json));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);     

        this.Response.Write(string.Format(CultureInfo.InvariantCulture, @"var UserCulture = ""{0}"";", userCulture));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        this.Response.Write(string.Format(CultureInfo.InvariantCulture, @"var Company = {0};", Company.Json(company)));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        var res = new StringBuilder();
        var cargos = JobPosition.JobsPositionByCompany((Company)Session["Company"]);
        bool first = true;
        foreach (var cargo in cargos)
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

        this.Response.Write(string.Format(CultureInfo.InvariantCulture, @"var jobPositionCompany ={0}[{1}{0}];", Environment.NewLine, res));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        

        this.Response.Write("var processTypeCompany = " + Environment.NewLine + ProccessTypeList(company) + ";");
        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        

        this.Response.Write("var departmentsCompany = " + Environment.NewLine + CompanyDepartments(company) + ";");

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);
        
        this.Response.Write("var Dictionary =" + Environment.NewLine);
        this.Response.Write("{" + Environment.NewLine);

        foreach (KeyValuePair<string, string> item in this.Dictionary)
        {
            if (!item.Key.StartsWith("Help_") || true)
            {
                this.Response.Write(this.DictionaryItem(item.Key.Replace(' ', '_'), item.Value));
            }
        }

        this.Response.Write("    \"-\": \"-\"" + Environment.NewLine);
        this.Response.Write("};");
    }

    /// <summary>Gets a entry for dictionary JSON</summary>
    /// <param name="key">Item key</param>
    /// <param name="value">Item value</param>
    /// <returns>JSON Code</returns>
    private string DictionaryItem(string key, string value)
    {
        return string.Format(CultureInfo.InvariantCulture, @"    ""{0}"": ""{1}"",{2}", key, value, Environment.NewLine);
    }

    private string UsedCulture(Company company, ApplicationUser user)
    {
        string userCulture = string.Empty;
        switch (company.Language)
        {
            case "es":
                userCulture = "es-ES";
                break;
            case "en":
                userCulture = "en-US";
                break;
            default:
                userCulture = "es-CA";
                break;
        }

        switch (user.Language)
        {
            case "es":
                userCulture = "es-ES";
                break;
            case "en":
                userCulture = "en-US";
                break;
            default:
                userCulture = "es-CA";
                break;
        }

        return userCulture;
    }

    private string CompanyDepartments(Company company)
    {
        var departmentsCompanyJson = new StringBuilder("[");
        bool firstDepartment = true;
        foreach (var department in Department.ByCompany(company.Id))
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
        return departmentsCompanyJson.ToString();
    }

    private string ProccessTypeList(Company company)
    {
        var processTypeList = new StringBuilder("[");
        bool firstProcessType = true;
        foreach (var processType in ProcessType.ObtainByCompany(company.Id, this.Dictionary))
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
        return processTypeList.ToString();
    }
}