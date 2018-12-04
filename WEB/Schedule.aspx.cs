// --------------------------------
// <copyright file="Schedule.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using SbrinnaCoreFramework;
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using System.Text;
using System.Linq;
using System.Globalization;

/// <summary>Implements Schedule page</summary>
public partial class Schedule : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>Gets tasks to show</summary>
    public string Tasks { get; private set; }

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
        this.company = (Company)Session["company"];

        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_ScheduledTasks");
        this.master.Titulo = "Item_ScheduledTasks";

        this.RenderScheludedTasksList();

        //this.Tasks = ScheduledTask.ByEmployeeJson(this.user, this.company.Id);

        /*var tasksJson = new StringBuilder("[");
        var tasks = ScheduledTask.ByEmployee(this.user, this.company.Id).Where(t => t.Expiration >= Constant.Now.AddYears(-1)).ToList();
        var printedTasks = new List<ScheduledTask>();
        var res = new StringBuilder();
        tasks = tasks.OrderByDescending(t => t.Expiration).ToList();
        bool first = true;
        foreach (var task in tasks)
        {
            if (printedTasks.Any(t => t.Action == task.Action && t.Equipment.Id == task.Equipment.Id && task.TaskType == t.TaskType))
            {
                continue;
            }

            if (first)
            {
                first = false;
            }
            else
            {
                tasksJson.Append(",");
            }

            printedTasks.Add(task);
            tasksJson.Append(task.Json);
        }

        tasksJson.Append("]");
        this.Tasks = tasksJson.ToString();*/
    }

    private void RenderScheludedTasksList()
    {
        var searchItems = new List<string>();
        var tasksJson = new StringBuilder("[");
        var tasks = ScheduledTask.ByEmployee(this.user, this.company.Id).Where(t => t.Expiration >= Constant.Now.AddYears(-1)).ToList();
        var printedTasks = new List<ScheduledTask>();
        tasks = tasks.OrderByDescending(t => t.Expiration).ToList();
        bool first = true;
        foreach (var task in tasks)
        {
            if (printedTasks.Any(t => t.Action == task.Action && t.Equipment.Id == task.Equipment.Id && task.TaskType == t.TaskType))
            {
                continue;
            }

            printedTasks.Add(task);
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
            //tasksJson.Append(task.JsonRow(this.Dictionary));
            tasksJson.Append(JsonRowLocal(task));
        }

        tasksJson.Append("]");
        this.Tasks = tasksJson.ToString();
    }
    public string JsonRowLocal(ScheduledTask task)
    {
        string tab = "home";
        string tooltip = string.Empty;
        string link = "EquipmentView";
        string operationId = string.Empty;
        string action = string.Empty;
        string labelType = string.Empty;
        switch (task.TaskType)
        {
            case "M":
                tooltip = dictionary["Item_Equipment_Tab_Maintenance"];
                tab = "&Tab=mantenimiento";
                operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", task.OperationId);
                action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", task.Action, task.Internal);
                labelType = task.Internal == "I" ? dictionary["Item_EquipmentMaintenance_Label_Internal"] : dictionary["Item_EquipmentMaintenance_Label_External"];
                break;
            case "V":
                tooltip = dictionary["Item_Equipment_Tab_Verification"];
                tab = "&Tab=verificacion";
                operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", task.OperationId);
                action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", task.Action, task.Internal);
                labelType = task.Internal == "I" ? dictionary["Item_EquipmentVerification_Label_Internal"] : dictionary["Item_EquipmentVerification_Label_External"];
                break;
            case "C":
                tooltip = dictionary["Item_Equipment_Tab_Calibration"];
                tab = "&Tab=calibracion";
                operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", task.OperationId);
                action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", task.Action, task.Internal);
                labelType = task.Internal == "I" ? dictionary["Item_EquipmentCalibration_Label_Internal"] : dictionary["Item_EquipmentCalibration_Label_External"];
                break;
            case "I":
                tooltip = dictionary["Item_Incident"];
                link = "IncidentView";
                tab = string.Empty;
                labelType = dictionary["Item_Incident"];
                break;
            case "A":
                tooltip = dictionary["Item_IncidentAction"];
                link = "ActionView";
                tab = string.Empty;
                labelType = dictionary["Item_IncidentAction"];
                break;
            case "X":
                tooltip = dictionary["Item_Indicador"];
                link = "IndicadorView";
                tab = "&Tab=Records";
                labelType = dictionary["Item_Indicador"];
                break;
            case "O":
                tooltip = dictionary["Item_Objetivo"];
                link = "ObjetivoView";
                tab = "&Tab=Records";
                labelType = dictionary["Item_Objetivo"];
                break;
            case "B":
                tooltip = dictionary["Item_BusinessRisk"];
                link = "BusinessRiskView";
                tab = string.Empty;
                labelType = dictionary["Item_BusinessRisk"];
                break;
            default:
                tooltip = string.Empty;
                link = "ActionView";
                tab = "home";
                labelType = string.Empty;
                break;
        }

        string pattern = @"{{
                ""location"":""{6}.aspx?id={0}{9}{10}{11}"",
                ""title"":""{5}"",
                ""color"":""{8}"",
                ""labelType"":""{4} / {2}{7}"",
                ""Item"":""{1}"",
                ""Responsible"":""{12}"",
                ""ResponsibleId"":{14},
                ""Provider"":""{13}"",
                ""Date"":""{3:dd/MM/yyyy}"",
                ""Type"":""{15}""}}";
        return string.Format(
            CultureInfo.InvariantCulture,
            pattern,
            task.Equipment.Id,
            GisoFramework.Tools.JsonCompliant(task.Equipment.Description),
            GisoFramework.Tools.JsonCompliant(task.Description),
            task.Expiration,
            labelType,
            tooltip,
            link,
            string.Empty,
            task.Expiration < DateTime.Now.Date ? "#f00" : "#000",
            tab,
            operationId,
            action,
            GisoFramework.Tools.JsonCompliant(task.Responsible.FullName),
            GisoFramework.Tools.JsonCompliant(task.Provider != null ? task.Provider.Description : string.Empty),
            task.Responsible.Id,
            task.TaskType);
    }
}