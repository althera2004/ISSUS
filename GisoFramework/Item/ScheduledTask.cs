// --------------------------------
// <copyright file="ScheduledTask.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements ScheduledTask class</summary>
    public class ScheduledTask
    {
        /// <summary>Gets or sets operation identifier</summary>
        public long OperationId { get; set; }

        public Equipment Equipment { get; set; }

        public string TaskType { get; set; }

        public string Description { get; set; }

        public DateTime Expiration { get; set; }

        public long Action { get; set; }

        public string Internal { get; set; }

        public Employee Responsible { get; set; }

        public Provider Provider { get; set; }

        public string Json
        {
            get
            {
                string pattern = @"
                {{
                    ""Equipment"": {{""Id"": {0},""Description"": ""{1}""}},
                    ""Type"":""{2}"",
                    ""Description"":""{3}"",
                    ""Expiration"":""{4:yyyyMMdd}"",
                    ""Internal"": ""{5}"",
                    ""ActionId"": {6},
                    ""OperationId"": {7}
                }}";

                var date = this.Expiration;
                if(date.Year< 1950)
                {
                    date = date.AddYears(100);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Equipment.Id,
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Equipment.Description)),
                    this.TaskType,
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Description)),
                    date,
                    this.Internal,
                    this.Action,
                    this.OperationId);
            }
        }

        public static string ByEmployeeJson(ApplicationUser user, int companyId)
        {
            var tasks = ByEmployee(user, companyId);
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var task in tasks)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(task.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<ScheduledTask> ByEmployee(ApplicationUser user, int companyId)
        {
            var res = new List<ScheduledTask>();
            using (var cmd = new SqlCommand("ScheduleTask_GetByEmployee"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", user.Employee.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (user.Admin || rdr.GetInt32(ColumnsScheduleTaskGetByEmployee.EmployeeId) == user.Employee.Id)
                                {
                                    var newTask = new ScheduledTask
                                    {
                                        OperationId = rdr.GetInt64(ColumnsScheduleTaskGetByEmployee.OperationId),
                                        Description = rdr.GetString(ColumnsScheduleTaskGetByEmployee.Operation),
                                        Equipment = new Equipment
                                        {
                                            Id = rdr.GetInt64(ColumnsScheduleTaskGetByEmployee.EquipmentId),
                                            Description = rdr.GetString(ColumnsScheduleTaskGetByEmployee.Description)
                                        },
                                        Expiration = rdr.GetDateTime(ColumnsScheduleTaskGetByEmployee.Expiration),
                                        TaskType = rdr.GetString(ColumnsScheduleTaskGetByEmployee.OperationType),
                                        Responsible = new Employee
                                        {
                                            Id = rdr.GetInt32(ColumnsScheduleTaskGetByEmployee.EmployeeId),
                                            Name = rdr.GetString(ColumnsScheduleTaskGetByEmployee.EmployeeName),
                                            LastName = rdr.GetString(ColumnsScheduleTaskGetByEmployee.EmployeeLastName)
                                        },
                                        Action = rdr.GetInt64(ColumnsScheduleTaskGetByEmployee.Action),
                                        Internal = rdr[ColumnsScheduleTaskGetByEmployee.Type].ToString() == "0" ? "I" : "E"
                                    };

                                    if (!rdr.IsDBNull(ColumnsScheduleTaskGetByEmployee.ProviderName))
                                    {
                                        newTask.Provider = new Provider
                                        {
                                            Id = rdr.GetInt64(ColumnsScheduleTaskGetByEmployee.ProviderId),
                                            Description = rdr.GetString(ColumnsScheduleTaskGetByEmployee.ProviderName)
                                        };
                                    }

                                    bool exists = false;
                                    foreach (var task in res)
                                    {
                                        if (newTask.Equipment.Id == task.Equipment.Id &&
                                        newTask.OperationId == task.OperationId &&
                                        newTask.TaskType == task.TaskType &&
                                        task.Internal == newTask.Internal)
                                        {
                                            if (task.Expiration < newTask.Expiration)
                                            {
                                                task.Expiration = newTask.Expiration;
                                            }

                                            exists = true;
                                        }
                                    }

                                    if (!exists)
                                    {
                                        res.Add(newTask);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<ScheduledTask>(res);
        }

        public string Row(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string tab = "home";
            string tooltip = string.Empty;
            string link = "EquipmentView";
            string operationId = string.Empty;
            string action = string.Empty;
            string labelType = string.Empty;
            switch (this.TaskType)
            {
                case "M":
                    tooltip = dictionary["Item_Equipment_Tab_Maintenance"];
                    tab = "&Tab=mantenimiento";
                    operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", this.OperationId);
                    action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", this.Action, this.Internal);
                    labelType = this.Internal == "I" ? dictionary["Item_EquipmentMaintenance_Label_Internal"] : dictionary["Item_EquipmentMaintenance_Label_External"];
                    break;
                case "V":
                    tooltip = dictionary["Item_Equipment_Tab_Verification"];
                    tab = "&Tab=verificacion";
                    operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", this.OperationId);
                    action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", this.Action, this.Internal);
                    labelType = this.Internal == "I" ? dictionary["Item_EquipmentVerification_Label_Internal"] : dictionary["Item_EquipmentVerification_Label_External"];
                    break;
                case "C":
                    tooltip = dictionary["Item_Equipment_Tab_Calibration"];
                    tab = "&Tab=calibracion";
                    operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", this.OperationId);
                    action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", this.Action, this.Internal);
                    labelType = this.Internal == "I" ? dictionary["Item_EquipmentCalibration_Label_Internal"] : dictionary["Item_EquipmentCalibration_Label_External"];
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
                    link = "ActionView";
                    tab = "&Tab=Records";
                    labelType = dictionary["Item_Indicador"];
                    break;
                case "O":
                    tooltip = dictionary["Item_Objetivo"];
                    link = "&Tab=Records";
                    tab = string.Empty;
                    labelType = dictionary["Item_Objetivo"];
                    break;
                case "B":
                    tooltip = dictionary["Item_BusinessRisk"];
                    link = "ActionView";
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

            string pattern = @"<tr style=""cursor:pointer;"" onclick=""document.location='{6}.aspx?id={0}{9}{10}{11}'""><td title=""{5}"" style=""color:{8};"">{4} / {2}{7}</td><td style=""color:{8};width:350px;""><div title=""{1}"" style=""text-overflow: ellipsis;overflow: hidden;white-space: nowrap;width:320px;"">{1}</div></td><td style=""width:250px;padding-lewft:4px;"">{12}{13}</td><td style=""color:{8};width:90px;"">{3:dd/MM/yyyy}</td></tr>";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                this.Equipment.Id,
                this.Equipment.Description,
                this.Description,
                this.Expiration,
                labelType,
                tooltip,
                link,
                string.Empty,
                this.Expiration < DateTime.Now.Date ? "#f00" : "#000",
                tab,
                operationId,
                action,
                this.Responsible.Description,
                this.Provider != null ? string.Format(CultureInfo.InvariantCulture, "{0}<br><strong>{1}</strong>", this.Responsible.FullName, this.Provider.Description) : this.Responsible.FullName);
        }

        public string JsonRow(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string tab = "home";
            string tooltip = string.Empty;
            string link = "EquipmentView";
            string operationId = string.Empty;
            string action = string.Empty;
            string labelType = string.Empty;
            switch (this.TaskType)
            {
                case "M":
                    tooltip = dictionary["Item_Equipment_Tab_Maintenance"];
                    tab = "&Tab=mantenimiento";
                    operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", this.OperationId);
                    action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", this.Action, this.Internal);
                    labelType = this.Internal == "I" ? dictionary["Item_EquipmentMaintenance_Label_Internal"] : dictionary["Item_EquipmentMaintenance_Label_External"];
                    break;
                case "V":
                    tooltip = dictionary["Item_Equipment_Tab_Verification"];
                    tab = "&Tab=verificacion";
                    operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", this.OperationId);
                    action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", this.Action, this.Internal);
                    labelType = this.Internal == "I" ? dictionary["Item_EquipmentVerification_Label_Internal"] : dictionary["Item_EquipmentVerification_Label_External"];
                    break;
                case "C":
                    tooltip = dictionary["Item_Equipment_Tab_Calibration"];
                    tab = "&Tab=calibracion";
                    operationId = string.Format(CultureInfo.InvariantCulture, "&OperationId={0}", this.OperationId);
                    action = string.Format(CultureInfo.InvariantCulture, "&Action={0}&Type={1}", this.Action, this.Internal);
                    labelType = this.Internal == "I" ? dictionary["Item_EquipmentCalibration_Label_Internal"] : dictionary["Item_EquipmentCalibration_Label_External"];
                    break;
                case "I":
                    tooltip = dictionary["Item_Incident"];
                    link = "IncidentView";
                    tab = string.Empty;
                    labelType = dictionary["Item_Incident"];
                    break;
                case "A":
                    tooltip = dictionary["Item_IncidentAction"];
                    link = "IncidentView";
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
                ""Date"":""{3:dd/MM/yyyy}""}}";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                this.Equipment.Id,
                Tools.JsonCompliant(this.Equipment.Description),
                Tools.JsonCompliant(this.Description),
                this.Expiration,
                labelType,
                tooltip,
                link,
                string.Empty,
                this.Expiration < DateTime.Now.Date ? "#f00" : "#000",
                tab,
                operationId,
                action,
                Tools.JsonCompliant(this.Responsible.FullName),
                Tools.JsonCompliant(this.Provider != null ? this.Provider.Description : string.Empty),
                this.Responsible.Id);
        }
    }
}