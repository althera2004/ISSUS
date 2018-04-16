// --------------------------------
// <copyright file="Incident.cs" company="Sbrinna">
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
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements Incident class</summary>
    public class Incident : BaseItem
    {
        public static Incident Empty
        {
            get
            {
                return new Incident
                {
                    Id = 0,
                    WhatHappened = string.Empty,
                    WhatHappenedBy = Employee.Empty,
                    Causes = string.Empty,
                    CausesBy = Employee.Empty,
                    Actions = string.Empty,
                    ActionsBy = Employee.Empty,
                    ActionsExecuter = Employee.Empty,
                    ClosedBy = Employee.Empty,
                    Department = Department.Empty,
                    Provider = Provider.Empty,
                    Customer = Customer.Empty,
                    ModifiedBy = ApplicationUser.Empty
                };
            }
        }

        /// <summary>Gets or sets incident code</summary>
        public long Code { get; set; }

        /// <summary>Gets or sets incident origin</summary>
        public int Origin { get; set; }

        /// <summary>Gets or sets incident department</summary>
        public Department Department { get; set; }

        /// <summary>Gets or sets incident provider</summary>
        public Provider Provider { get; set; }

        /// <summary>Gets or sets incident customer</summary>
        public Customer Customer { get; set; }

        /// <summary>Gets or sets what happened</summary>
        public string WhatHappened { get; set; }

        /// <summary>Gets or sets who informes what happenned</summary>
        public Employee WhatHappenedBy { get; set; }

        /// <summary>Gets or sets when what happenned is informed</summary>
        public DateTime? WhatHappenedOn { get; set; }

        public string Causes { get; set; }

        public Employee CausesBy { get; set; }

        public DateTime? CausesOn { get; set; }

        public string Actions { get; set; }

        public Employee ActionsBy { get; set; }

        public DateTime? ActionsOn { get; set; }

        public Employee ActionsExecuter { get; set; }

        public DateTime? ActionsSchedule { get; set; }

        public bool ApplyAction { get; set; }

        public string Notes { get; set; }

        public string Annotations { get; set; }

        public Employee ClosedBy { get; set; }

        public DateTime? ClosedOn { get; set; }

        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""IncidentView.aspx?id={0}"" title=""{2} {1}"">{1}</a>",
                    this.Id,
                    this.Description,
                    ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Description"":""{1}""}}",
                    this.Id, 
                    Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var res = new StringBuilder("{").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Code", this.Code)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Origin", this.Origin)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Department", this.Department)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Provider", this.Provider)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Customer", this.Customer)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("WhatHappened", this.WhatHappened)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("WhatHappenedBy", this.WhatHappenedBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("WhatHappenedOn", this.WhatHappenedOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Causes", this.Causes)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("CausesBy", this.CausesBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("CausesOn", this.CausesOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Actions", this.Actions)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ActionsBy", this.ActionsBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ActionsOn", this.ActionsOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ClosedBy", this.ClosedBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ClosedOn", this.ClosedOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ApplyAction", this.ApplyAction)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Notes", this.Notes)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Active", this.Active)).Append(Environment.NewLine).Append("\t}");
                return res.ToString();
            }
        }

        public static ActionResult SaveAction(IncidentAction action, int userId)
        {
            if (action == null)
            {
                return ActionResult.NoAction;
            }

            if (action.Id == 0)
            {
               return action.Insert(userId);
            }
            else
            {
                return action.Update(userId);
            }
        }

        public static ActionResult DeleteActions(long incidentId, int companyId, int userId)
        {
            /* CREATE PROCEDURE Incident_DeleteActions
             *   @IncidentId bigint,
             *   @CompanyId int,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Incident_DeleteActions"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex.Message);
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }

                        cmd.Connection.Dispose();
                    }
                }
            }

            return res;
        }

        public static string FilterList(int companyId, DateTime? from, DateTime? to, bool statusIdentified, bool statusAnalyzed, bool statusInProgress, bool statusClosed, int origin, int departmentId, long providerId, long customerId)
        {
            var items = Filter(companyId, from, to, statusIdentified, statusAnalyzed, statusInProgress, statusClosed, origin, departmentId, providerId, customerId);
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var item in items)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append("{");
                res.Append(Tools.JsonPair("IncidentId", item.Id)).Append(",");
                res.Append(Tools.JsonPair("Code", item.Code)).Append(",");
                res.Append(Tools.JsonPair("Description", item.Description)).Append(",");
                res.Append(Tools.JsonPair("Status", item.Status)).Append(",");
                res.Append(Tools.JsonPair("Action", item.Action.JsonKeyValue)).Append(",");
                res.Append(Tools.JsonPair("Department", item.Department)).Append(",");
                res.Append(Tools.JsonPair("Provider", item.Provider)).Append(",");
                res.Append(Tools.JsonPair("Customer", item.Customer)).Append(",");
                res.Append(Tools.JsonPair("Action", item.Action)).Append(",");
                res.Append(Tools.JsonPair("Amount", item.Amount)).Append(",");
                res.Append(Tools.JsonPair("Open", item.Open)).Append(",");
                res.Append(Tools.JsonPair("Close", item.Close)).Append("}");
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<IncidentFilterItem> Filter(int companyId, DateTime? from, DateTime? to, bool statusIdentified, bool statusAnalyzed, bool statusInProgress, bool statusClosed, int origin, int departmentId, long providerId, long customerId)
        {
            /* CREATE PROCEDURE Incident_Filter
             *   @CompanyId int,
             *   @DateFrom datetime,
             *   @DateTo datetime,
             *   @StatusIdentified bit,
             *   @StatusAnalyzed bit,
             *   @StatusInProcess bit,
             *   @StatusClosed bit,
             *   @Origin int,
             *   @DepartmentId int,
             *   @ProviderId bigint,
             *   @CustomerId bigint */
            var res = new List<IncidentFilterItem>();
            using (var cmd = new SqlCommand("Incident_Filter"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@DateFrom", from));
                        cmd.Parameters.Add(DataParameter.Input("@DateTo", to));
                        cmd.Parameters.Add(DataParameter.Input("@StatusIdentified", statusIdentified));
                        cmd.Parameters.Add(DataParameter.Input("@StatusAnalyzed", statusAnalyzed));
                        cmd.Parameters.Add(DataParameter.Input("@StatusInProcess", statusInProgress));
                        cmd.Parameters.Add(DataParameter.Input("@StatusClosed", statusClosed));
                        cmd.Parameters.Add(DataParameter.Input("@Origin", origin));
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", departmentId));
                        cmd.Parameters.Add(DataParameter.Input("@ProviderId", providerId));
                        cmd.Parameters.Add(DataParameter.Input("@CustomerId", customerId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var incidentActionId = rdr.GetInt64(ColumnsIncidentFilterGet.IncidentActionId);
                                var item = new IncidentFilterItem
                                {
                                    Id = rdr.GetInt64(ColumnsIncidentFilterGet.IncidentId),
                                    Description = rdr.GetString(ColumnsIncidentFilterGet.IncidentDescription),
                                    Code = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:00000}", rdr.GetInt64(ColumnsIncidentFilterGet.Code)),
                                    Action = new IncidentAction()
                                    {
                                        Id = incidentActionId,
                                        Description = rdr.GetString(ColumnsIncidentFilterGet.IncidentDescription)
                                    },
                                    Origin = rdr.GetInt32(ColumnsIncidentFilterGet.Origin),
                                    Department = new Department()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentFilterGet.DepartmentId),
                                        Description = rdr.GetString(ColumnsIncidentFilterGet.DepartmentName)
                                    },
                                    Provider = new Provider()
                                    {
                                        Id = rdr.GetInt64(ColumnsIncidentFilterGet.ProviderId),
                                        Description = rdr.GetString(ColumnsIncidentFilterGet.ProviderDescription)
                                    },
                                    Customer = new Customer()
                                    {
                                        Id = rdr.GetInt64(ColumnsIncidentFilterGet.CustomerId),
                                        Description = rdr.GetString(ColumnsIncidentFilterGet.CustomerDescription)
                                    },
                                    Amount = rdr.GetDecimal(ColumnsIncidentFilterGet.Amount),
                                    Status = rdr.GetInt32(ColumnsIncidentFilterGet.Status)
                                };

                                if (incidentActionId != 0)
                                {
                                    item.Action = new IncidentAction
                                    {
                                        Id = incidentActionId,
                                        Description = rdr.GetString(ColumnsIncidentFilterGet.IncidentActionDescription)
                                    };
                                }
                                else
                                {
                                    item.Action = IncidentAction.Empty;
                                }

                                if (!rdr.IsDBNull(ColumnsIncidentFilterGet.OpenDate))
                                {
                                    item.Open = rdr.GetDateTime(ColumnsIncidentFilterGet.OpenDate);
                                }

                                if (!rdr.IsDBNull(ColumnsIncidentFilterGet.CloseDate))
                                {
                                    item.Close = rdr.GetDateTime(ColumnsIncidentFilterGet.CloseDate);
                                }

                                res.Add(item);
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

            HttpContext.Current.Session["IncidentFilterData"] = res;
            return new ReadOnlyCollection<IncidentFilterItem>(res);
        }

        /*public override string Differences(BaseItem item1)
        {
            if(item1==null)
            {
                return string.Empty;
            }

            return Differences(item1 as Incident, this);
        }*/

        public static string Differences(Incident item1, Incident item2)
        {
            if (item1 == null || item2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();

            if (item1.Description != item2.Description)
            {
                res.Append("Description=>").Append(item2.Description).Append(";");
            }

            if (item1.WhatHappened != item2.WhatHappened)
            {
                res.Append("WhatHappened=>").Append(item2.WhatHappened).Append(";");
            }

            if (item1.WhatHappenedBy.Id != item2.WhatHappenedBy.Id)
            {
                res.Append("WhatHappenedBy=>").Append(item2.WhatHappenedBy.Id).Append(";");
            }

            if (item1.WhatHappenedOn.HasValue)
            {
                if (item2.WhatHappenedOn.HasValue && item1.WhatHappenedOn != item2.WhatHappenedOn)
                {
                    res.Append("WhatHappenedOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.WhatHappenedOn.Value)).Append(";");
                }
                else
                {
                    res.Append("WhatHappenedOn=>;");
                }
            }
            else
            {
                if (item2.WhatHappenedOn.HasValue)
                {
                    res.Append("WhatHappenedOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.WhatHappenedOn.Value)).Append(";");
                }
            }

            if (item1.Causes != item2.Causes)
            {
                res.Append("Causes=>").Append(item2.Causes).Append(";");
            }

            if (item1.CausesBy != null)
            {
                if (item2.CausesBy == null)
                {
                    res.Append("CausesBy=>;");
                }
                else if (item1.CausesBy.Id != item2.CausesBy.Id)
                {
                    res.Append("CausesBy=>").Append(item2.CausesBy.Id).Append(";");
                }
            }
            else
            {
                if (item2.CausesBy != null)
                {
                    res.Append("CausesBy=>").Append(item2.CausesBy.Id).Append(";");
                }
            }

            if (item1.CausesOn.HasValue)
            {
                if (item2.CausesOn.HasValue && item1.CausesOn != item2.CausesOn)
                {
                    res.Append("CausesOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.CausesOn.Value)).Append(";");
                }
                else
                {
                    res.Append("CausesOn=>;");
                }
            }
            else
            {
                if (item2.CausesOn.HasValue)
                {
                    res.Append("CausesOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.CausesOn.Value)).Append(";");
                }
            }

            if (item1.Actions != item2.Actions)
            {
                res.Append("Actions=>").Append(item2.Actions).Append(";");
            }

            if (item1.ActionsBy != null)
            {
                if (item2.ActionsBy == null)
                {
                    res.Append("ActionsBy=>;");
                }
                else if (item1.ActionsBy.Id != item2.ActionsBy.Id)
                {
                    res.Append("ActionsBy=>").Append(item2.ActionsBy.Id).Append(";");
                }
            }
            else
            {
                if (item2.ActionsBy != null)
                {
                    res.Append("ActionsBy=>").Append(item2.ActionsBy.Id).Append(";");
                }
            }

            if (item1.ActionsOn.HasValue)
            {
                if (item2.ActionsOn.HasValue && item1.ActionsOn != item2.ActionsOn)
                {
                    res.Append("ActionsOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.ActionsOn.Value)).Append(";");
                }
                else
                {
                    res.Append("ActionsOn=>;");
                }
            }
            else
            {
                if (item2.ActionsOn.HasValue)
                {
                    res.Append("ActionsOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.ActionsOn.Value)).Append(";");
                }
            }

            if (item1.ClosedBy != null)
            {
                if (item2.ClosedBy == null)
                {
                    res.Append("ClosedBy=>;");
                }
                else if (item1.ClosedBy.Id != item2.ClosedBy.Id)
                {
                    res.Append("ClosedBy=>").Append(item2.ClosedBy.Id).Append(";");
                }
            }
            else
            {
                if (item2.ClosedBy != null)
                {
                    res.Append("ClosedBy=>").Append(item2.ClosedBy.Id).Append(";");
                }
            }

            if (item1.ClosedOn.HasValue)
            {
                if (item2.ClosedOn.HasValue && item1.ClosedOn != item2.ClosedOn)
                {
                    res.Append("ClosedOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.ClosedOn.Value)).Append(";");
                }
                else
                {
                    res.Append("ClosedOn=>;");
                }
            }
            else
            {
                if (item2.ClosedOn.HasValue)
                {
                    res.Append("ClosedOn=>").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", item2.ClosedOn.Value)).Append(";");
                }
            }

            return res.ToString();
        }

        public static Incident GetById(long incidentId, int companyId)
        {
            /* CREATE PROCEDURE Incident_GetById
             * @IncidentId bigint,
             * @CompanyId int */
            var res = new Incident();
            using (var cmd = new SqlCommand("Incident_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = new Incident()
                                {
                                    Id = incidentId,
                                    Code = rdr.GetInt64(ColumnsIncidentGet.Code),
                                    CompanyId = rdr.GetInt32(ColumnsIncidentGet.CompanyId),
                                    Description = rdr.GetString(ColumnsIncidentGet.Description),
                                    Origin = rdr.GetInt32(3),
                                    WhatHappened = rdr.GetString(ColumnsIncidentGet.WhatHappened),
                                    WhatHappenedBy = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.WhatHappenedById),
                                        Name = rdr.GetString(ColumnsIncidentGet.WhatHappenedByName),
                                        LastName = rdr.GetString(ColumnsIncidentGet.WhatHappenedByLastName)
                                    },
                                    WhatHappenedOn = rdr.GetDateTime(ColumnsIncidentGet.WhatHappenedOn),
                                    Notes = rdr.GetString(ColumnsIncidentGet.Notes),
                                    Annotations = rdr.GetString(ColumnsIncidentGet.Annotations),
                                    Active = rdr.GetBoolean(ColumnsIncidentGet.Active),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsIncidentGet.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsIncidentGet.ModifiedOn),
                                    Department = new Department()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.DepartmentId),
                                        Description = rdr.GetString(ColumnsIncidentGet.DepartmentName)
                                    },
                                    Provider = new Provider()
                                    {
                                        Id = rdr.GetInt64(ColumnsIncidentGet.ProviderId),
                                        Description = rdr.GetString(ColumnsIncidentGet.ProviderName)
                                    },
                                    Customer = new Customer()
                                    {
                                        Id = rdr.GetInt64(ColumnsIncidentGet.CustomerId),
                                        Description = rdr.GetString(ColumnsIncidentGet.CustomerName)
                                    }
                                };

                                if (!rdr.IsDBNull(ColumnsIncidentGet.CausesOn))
                                {
                                    res.Causes = rdr.GetString(ColumnsIncidentGet.Causes);
                                    res.CausesOn = rdr.GetDateTime(ColumnsIncidentGet.CausesOn);
                                    res.CausesBy = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.CausesById),
                                        Name = rdr.GetString(ColumnsIncidentGet.CausesByName),
                                        LastName = rdr.GetString(ColumnsIncidentGet.CausesByLastName)
                                    };
                                }
                                else
                                {
                                    res.CausesBy = Employee.Empty;
                                }

                                if (!rdr.IsDBNull(ColumnsIncidentGet.ActionsOn))
                                {
                                    res.Actions = rdr.GetString(ColumnsIncidentGet.Actions);
                                    res.ActionsOn = rdr.GetDateTime(ColumnsIncidentGet.ActionsOn);
                                    res.ActionsBy = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.ActionsById),
                                        Name = rdr.GetString(ColumnsIncidentGet.ActionsByName),
                                        LastName = rdr.GetString(ColumnsIncidentGet.ActionsByLastName)
                                    };
                                }
                                else
                                {
                                    res.ActionsBy = Employee.Empty;
                                }

                                if (!rdr.IsDBNull(ColumnsIncidentGet.ActionsExecuterId))
                                {
                                    res.ActionsSchedule = rdr.GetDateTime(ColumnsIncidentGet.ActionsSchedule);
                                    res.ActionsExecuter = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.ActionsExecuterId),
                                        Name = rdr.GetString(ColumnsIncidentGet.ActionsExecuterName),
                                        LastName = rdr.GetString(ColumnsIncidentGet.ActionsExecuterLastName)
                                    };
                                }
                                else
                                {
                                    res.ActionsExecuter = Employee.Empty;
                                }

                                if (!rdr.IsDBNull(ColumnsIncidentGet.ClosedOn))
                                {
                                    res.ClosedOn = rdr.GetDateTime(ColumnsIncidentGet.ClosedOn);
                                    res.ClosedBy = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.ClosedById),
                                        Name = rdr.GetString(ColumnsIncidentGet.ClosedByName),
                                        LastName = rdr.GetString(ColumnsIncidentGet.ClosedByLastName)
                                    };
                                }
                                else
                                {
                                    res.ClosedBy = Employee.Empty;
                                }

                                if (res.Origin == 1 && !rdr.IsDBNull(ColumnsIncidentGet.DepartmentId))
                                {
                                    res.Department = new Department()
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentGet.DepartmentId),
                                        Description = rdr.GetString(ColumnsIncidentGet.DepartmentName)
                                    };
                                }

                                if (res.Origin == 2 && !rdr.IsDBNull(ColumnsIncidentGet.ProviderId))
                                {
                                    res.Provider = new Provider()
                                    {
                                        Id = rdr.GetInt64(ColumnsIncidentGet.ProviderId),
                                        Description = rdr.GetString(ColumnsIncidentGet.ProviderName)
                                    };
                                }

                                if (res.Origin == 3 && !rdr.IsDBNull(ColumnsIncidentGet.CustomerId))
                                {
                                    res.Customer = new Customer()
                                    {
                                        Id = rdr.GetInt64(ColumnsIncidentGet.CustomerId),
                                        Description = rdr.GetString(ColumnsIncidentGet.CustomerName)
                                    };
                                }

                                res.ModifiedBy.Employee = Employee.ByUserId(res.ModifiedBy.Id);
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

            return res;
        }

        public static ActionResult Restore(long incidentId, int companyId, int userId)
        {
            /* CREATE PROCEDURE [dbo].[Incident_Restore]
             *   @IncidentId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Incident_Restore"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Restore", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Restore", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentId, userId, companyId));
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

            return res;
        }

        public static ActionResult Anulate(int incidentId, int companyId, int applicationUserId, DateTime date, int responsible)
        {
            /* CREATE PROCEDURE [dbo].[Incident_Anulate]
             *   @IncidentId int,
             *   @CompanyId int,
             *   @EndDate datetime,
             *   @EndResponsible int,
             *   @ApplicationUserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Incident_Anulate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", date));
                        cmd.Parameters.Add(DataParameter.Input("@EndResponsible", responsible));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Anulate", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentId, applicationUserId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Anulate", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentId, applicationUserId, companyId));
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

            return res;
        }

        public static ActionResult Delete(long incidentId, int userId, int companyId)
        {
            /* CREATE PROCEDURE Incident_Delete
             *   @IncidentId bigint,
             *   @CompanyId int,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Incident_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentId, userId, companyId));
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

            return res;
        }

        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE Incident_Insert
             *   @IncidentId bigint output,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @Origin int,
             *   @DepartmentId int,
             *   @ProviderId bigint,
             *   @CustomerId bigint,
             *   @WhatHappend nvarchar(255),
             *   @WhatHappendBy int,
             *   @WhatHappendOn datetime,
             *   @Causes nvarchar(255),
             *   @CausesBy int,
             *   @CausesOn datetime,
             *   @Actions nvarchar(255),
             *   @ActionsBy int,
             *   @ActionsOn datetime,
             *   @ActionsExecuter int,
             *   @ActionsSchedule datetime,
             *   @ApplyAction bit,
             *   @Notes text,
             *   @Anotations text,
             *   @ClosedBy int,
             *   @ClosedOn datetime,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Incident_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@IncidentId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Origin", this.Origin));

                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.Department));
                        cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider));
                        cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Customer));

                        cmd.Parameters.Add(DataParameter.Input("@WhatHappend", this.WhatHappened ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@WhatHappendBy", this.WhatHappenedBy));
                        cmd.Parameters.Add(DataParameter.Input("@WhatHappendOn", this.WhatHappenedOn));

                        cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@CausesBy", this.CausesBy.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CausesOn", this.CausesOn));

                        cmd.Parameters.Add(DataParameter.Input("@Actions", this.Actions ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsBy", this.ActionsBy.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsOn", this.ActionsOn));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsExecuter", this.ActionsExecuter.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsSchedule", this.ActionsSchedule));

                        cmd.Parameters.Add(DataParameter.Input("@ClosedBy", this.ClosedBy.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ClosedOn", this.ClosedOn));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Anotations", this.Annotations ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ApplyAction", this.ApplyAction));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@IncidentId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        res.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

            return res;
        }

        public ActionResult Update(int userId, string differences)
        {
            /* CREATE PROCEDURE Incident_Update
             *   @IncidentId bigint,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @Code int,
             *   @Origin int,
             *   @DepartmentId int,
             *   @ProviderId bigint,
             *   @CustomerId bigint,
             *   @WhatHappend nvarchar(255),
             *   @WhatHappendBy int,
             *   @WhatHappendOn datetime,
             *   @Causes nvarchar(255),
             *   @CausesBy int,
             *   @CausesOn datetime,
             *   @Actions nvarchar(255),
             *   @ActionsBy int,
             *   @ActionsOn datetime,
             *   @ActionsExecuter bigint,
             *   @ActionsSchedule datetime,
             *   @ApplyAction bit,
             *   @Notes text,
             *   @Anotations text,
             *   @ClosedBy int,
             *   @ClosedOn datetime,
             *   @UserId int 
             *   @Differences text */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Incident_Update"))
            {
                if (!this.ApplyAction)
                {
                    res = DeleteActions(this.Id, this.CompanyId, userId);
                }

                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Code", this.Code));
                        cmd.Parameters.Add(DataParameter.Input("@Origin", this.Origin));
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.Department.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Customer.Id));

                        cmd.Parameters.Add(DataParameter.Input("@WhatHappend", this.WhatHappened ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@WhatHappendBy", this.WhatHappenedBy.Id));
                        cmd.Parameters.Add(DataParameter.Input("@WhatHappendOn", this.WhatHappenedOn));

                        cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@CausesBy", this.CausesBy.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CausesOn", this.CausesOn));

                        cmd.Parameters.Add(DataParameter.Input("@Actions", this.Actions ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsBy", this.ActionsBy.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsOn", this.ActionsOn));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsExecuter", this.ActionsExecuter.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ActionsSchedule", this.ActionsSchedule));

                        cmd.Parameters.Add(DataParameter.Input("@ClosedBy", this.ClosedBy.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ClosedOn", this.ClosedOn));

                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Anotations", this.Annotations ?? string.Empty, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@Differences", differences));
                        cmd.Parameters.Add(DataParameter.Input("@ApplyAction", this.ApplyAction));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

            return res;
        }
        
        public ActionResult Delete(int userId)
        {
            return Delete(this.Id, userId, this.CompanyId);
        }
    }
}