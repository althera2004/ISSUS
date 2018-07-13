// --------------------------------
// <copyright file="DashBoard.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;

/// <summary>Implementation of DashBoard page</summary>
public partial class DashBoard : Page
{
    /// <summary>Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string Filter
    {
        get
        {
            string filter = @"{""Owners"":true,""Others"":true,""Passed"": false}";

            if (Session["DashBoardFilter"] != null)
            {
                filter = Session["DashBoardFilter"].ToString();
            }

            return filter;
        }
    }

    public string Tasks { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        Session["User"] = this.user;
        this.master = this.Master as Giso;
        this.company = Session["Company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.master.AddBreadCrumb("Item_DashBoard");
        this.master.Titulo = "Item_DashBoard";
        this.RenderScheludedTasksList();
    }

    private void RenderScheludedTasksList()
    {
        var searchItems = new List<string>();
        var tasksJson = new StringBuilder("[");
        var tasks = ScheduledTask.ByEmployee(this.user.Employee.Id, this.company.Id).Where(t => t.Expiration >= Constant.Now.AddYears(-1)).ToList();
        var printedTasks = new List<ScheduledTask>();
        var res = new StringBuilder();
        tasks = tasks.OrderByDescending(t => t.Expiration).ToList();
        bool first = true;
        foreach (var task in tasks)
        {
            if(printedTasks.Any(t=> t.Action == task.Action && t.Equipment.Id == task.Equipment.Id && task.TaskType == t.TaskType))
            {
                continue;
            }

            printedTasks.Add(task);
            res.Append(task.Row(this.Dictionary));
            if (first)
            {
                first = false;
            }
            else
            {
                tasksJson.Append(",");
            }
            
            if (!searchItems.Contains(task.Equipment.Description)) { searchItems.Add(task.Equipment.Description); };
            
            if (!searchItems.Contains(task.Responsible.FullName)) { searchItems.Add(task.Responsible.FullName); };

            if (task.Provider != null)
            {
                if (!searchItems.Contains(task.Provider.Description)) { searchItems.Add(task.Provider.Description); };
            }

            var text = string.Empty;
            switch (task.TaskType)
            {
                case "M":
                    text = task.Internal == "I" ? Dictionary["Item_EquipmentMaintenance_Label_Internal"] : Dictionary["Item_EquipmentMaintenance_Label_External"];
                    break;
                case "V":
                    text = task.Internal == "I" ? Dictionary["Item_EquipmentVerification_Label_Internal"] : Dictionary["Item_EquipmentVerification_Label_External"];
                    break;
                case "C":
                    text = task.Internal == "I" ? Dictionary["Item_EquipmentCalibration_Label_Internal"] : Dictionary["Item_EquipmentCalibration_Label_External"];
                    break;
                case "I":
                    text = Dictionary["Item_Incident"];
                    break;
                case "A":
                    text = Dictionary["Item_IncidentAction"];
                    break;
                case "X":
                    text = Dictionary["Item_Indicador"];
                    break;
                case "O":
                    text = Dictionary["Item_Objetivo"];
                    break;
                case "B":
                    text = Dictionary["Item_BusinessRisk"];
                    break;
            }

            if (!searchItems.Contains(text)) { searchItems.Add(text); };

            tasksJson.Append(task.JsonRow(this.Dictionary));
        }

        //this.DataTotal.Text = string.Format(CultureInfo.InvariantCulture, "{0}", tasks.Count);
        this.LtScheduledTasks.Text = res.ToString();

        tasksJson.Append("]");
        this.Tasks = tasksJson.ToString();

        var searchItem = new StringBuilder();
        first = true;
        foreach (string item in searchItems.OrderBy(s => s))
        {
            if (first)
            {
                first = false;
            }
            else
            {
                searchItem.Append(",");
            }

            searchItem.AppendFormat(CultureInfo.InvariantCulture, @"""{0}""", item);
        }

        this.master.SearcheableItems = searchItem.ToString();
    }
}