// --------------------------------
// <copyright file="IncidentAction.cs" company="Sbrinna">
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
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;
    using System.Web;

    /// <summary>
    /// Implements IncidentAction class
    /// </summary>
    public class IncidentAction : BaseItem
    {
        #region Fields
        public static IncidentAction Empty
        {
            get
            {
                return new IncidentAction()
                {
                    Id = 0,
                    Description = string.Empty,
                    WhatHappened = string.Empty,
                    WhatHappenedBy = Employee.Empty,
                    Causes = string.Empty,
                    CausesBy = Employee.Empty,
                    Actions = string.Empty,
                    ActionsBy = Employee.Empty,
                    ClosedBy = Employee.Empty,
                    Department = Department.Empty,
                    Provider = Provider.Empty,
                    Customer = Customer.Empty,
                    ModifiedBy = ApplicationUser.Empty
                };
            }
        }

        public string Status { get; set; }

        public int ActionType { get; set; }

        public int Origin { get; set; }

        public int ReporterType { get; set; }

        public Department Department { get; set; }

        public Provider Provider { get; set; }

        public Customer Customer { get; set; }

        public long Number { get; set; }

        public long IncidentId { get; set; }

        public long BusinessRiskId { get; set; }

        public string WhatHappened { get; set; }

        public Employee WhatHappenedBy { get; set; }

        public DateTime? WhatHappenedOn { get; set; }

        public string Causes { get; set; }

        public Employee CausesBy { get; set; }

        public DateTime? CausesOn { get; set; }

        public string Actions { get; set; }

        public Employee ActionsBy { get; set; }

        public DateTime? ActionsOn { get; set; }

        public Employee ActionsExecuter { get; set; }

        public DateTime? ActionsSchedule { get; set; }

        public string Monitoring { get; set; }

        public Employee ClosedBy { get; set; }

        public DateTime? ClosedOn { get; set; }

        public Employee ClosedExecutor { get; set; }

        public DateTime? ClosedExecutorOn { get; set; }

        public string Notes { get; set; }
        #endregion

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                StringBuilder res = new StringBuilder("{").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ActionType", this.ActionType)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Origin", this.Origin)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ReporterType", this.ReporterType)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Department", this.Department)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Provider", this.Provider)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Customer", this.Customer)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Number", this.Number)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("IncidentId", this.IncidentId)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("BusinessRiskId", this.BusinessRiskId)).Append(",").Append(Environment.NewLine).Append("\t"); 
                res.Append(Tools.JsonPair("WhatHappened", this.WhatHappened)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("WhatHappenedBy", this.WhatHappenedBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("WhatHappenedOn", this.WhatHappenedOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Causes", this.Causes)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("CausesBy", this.CausesBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("CausesOn", this.CausesOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Actions", this.Actions)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ActionsBy", this.ActionsBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ActionsOn", this.ActionsOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Monitoring", this.Monitoring)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ClosedBy", this.ClosedBy)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("ClosedOn", this.ClosedOn)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Notes", this.Notes)).Append(",").Append(Environment.NewLine).Append("\t");
                res.Append(Tools.JsonPair("Active", this.Active)).Append(Environment.NewLine).Append("\t}");
                return res.ToString();
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<a href=""ActionView.aspx?id={0}"">{1:00000} - {2}</a>",
                    this.Id,
                    this.Number,
                    this.Description);
            }
        }

        /*
        public override string Differences(BaseItem item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            IncidentAction c1 = item as IncidentAction;
            StringBuilder res = new StringBuilder();
            if (c1.Description != this.Description)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "Description:{0},", this.Description);
            }
            if (c1.WhatHappend != this.WhatHappend)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "WhatHappend:{0},", this.WhatHappend);
            }
            if (c1.WhatHappendBy.FullName != this.WhatHappendBy.FullName)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "WhatHappendBy.FullName:{0},", this.WhatHappendBy.FullName);
            }
            if (c1.WhatHappendOn != this.WhatHappendOn)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "WhatHappendOn:{0:dd/MM/yyyy},", this.WhatHappendOn);
            }

            return res.ToString();
        }
        */

        public static IncidentAction GetById(long incidentActionId, int companyId)
        {
            IncidentAction res = IncidentAction.Empty;
            using (SqlCommand cmd = new SqlCommand("IncidentAction_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionId", incidentActionId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res = new IncidentAction()
                        {
                            Id = incidentActionId,
                            CompanyId = rdr.GetInt32(ColumnsIncidentActionGet.CompanyId),
                            Description = rdr.GetString(ColumnsIncidentActionGet.Description),
                            ActionType = rdr.GetInt32(ColumnsIncidentActionGet.ActionType),
                            Origin = rdr.GetInt32(ColumnsIncidentActionGet.Origin),
                            ReporterType = rdr.GetInt32(ColumnsIncidentActionGet.ReporterType),
                            WhatHappened = rdr.GetString(ColumnsIncidentActionGet.WhatHappend),
                            WhatHappenedBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.WhatHappendId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.WhatHappendName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.WhatHappendLastName)
                            },
                            WhatHappenedOn = rdr.GetDateTime(ColumnsIncidentActionGet.WhatHappendOn),
                            Monitoring = rdr.GetString(ColumnsIncidentActionGet.Monitoring),
                            Notes = rdr.GetString(ColumnsIncidentActionGet.Notes),
                            Active = rdr.GetBoolean(ColumnsIncidentActionGet.Active),
                            ModifiedBy = new ApplicationUser
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsIncidentActionGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsIncidentActionGet.ModifiedOn),
                            Number = rdr.GetInt64(ColumnsIncidentActionGet.Number)
                        };

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.CausesId))
                        {
                            res.Causes = rdr.GetString(ColumnsIncidentActionGet.Causes);
                            res.CausesOn = rdr.GetDateTime(ColumnsIncidentActionGet.CausesOn);
                            res.CausesBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.CausesId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.CausesName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.CausesLastName)
                            };
                        }
                        else
                        {
                            res.CausesBy = Employee.Empty;
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.ActionsId))
                        {
                            res.Actions = rdr.GetString(ColumnsIncidentActionGet.Actions);
                            res.ActionsOn = rdr.GetDateTime(ColumnsIncidentActionGet.ActionsOn);
                            res.ActionsBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ActionsId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.ActionsName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.ActionsLastName)
                            };
                        }
                        else
                        {
                            res.ActionsBy = Employee.Empty;
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.ActionsExecuterId))
                        {
                            res.ActionsSchedule = rdr.GetDateTime(ColumnsIncidentActionGet.ActionsExecuterSchedule);
                            res.ActionsExecuter = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ActionsExecuterId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.ActionsExecuterName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.ActionsExecuterLastName)
                            };
                        }
                        else
                        {
                            res.ActionsExecuter = Employee.Empty;
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.ClosedOn))
                        {
                            res.ClosedOn = rdr.GetDateTime(ColumnsIncidentActionGet.ClosedOn);
                            res.ClosedBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ClosedId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.ClosedName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.ClosedLastName)
                            };
                        }
                        else
                        {
                            res.ClosedBy = Employee.Empty;
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.ClosedExecuterOn))
                        {
                            res.ClosedExecutorOn = rdr.GetDateTime(ColumnsIncidentActionGet.ClosedExecuterOn);
                            res.ClosedExecutor = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ClosedExecuterId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.ClosedExecuterName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.ClosedExecuterLastName)
                            };
                        }
                        else
                        {
                            res.ClosedExecutor = Employee.Empty;
                        }

                        if (res.ReporterType == 1 && !rdr.IsDBNull(ColumnsIncidentActionGet.DepartmentId))
                        {
                            res.Department = new Department() { Id = rdr.GetInt32(ColumnsIncidentActionGet.DepartmentId) };
                        }

                        if (res.ReporterType == 2 && !rdr.IsDBNull(ColumnsIncidentActionGet.ProviderId))
                        {
                            res.Provider = new Provider() { Id = rdr.GetInt64(ColumnsIncidentActionGet.ProviderId) };
                        }

                        if (res.ReporterType == 3 && !rdr.IsDBNull(ColumnsIncidentActionGet.CustomerId))
                        {
                            res.Customer = new Customer() { Id = rdr.GetInt64(ColumnsIncidentActionGet.CustomerId) };
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.BusinessRiskId))
                        {
                            res.BusinessRiskId = rdr.GetInt64(ColumnsIncidentActionGet.BusinessRiskId);
                        }
                        else
                        {
                            res.BusinessRiskId = -1;
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.IncidentId))
                        {
                            res.IncidentId = rdr.GetInt64(ColumnsIncidentActionGet.IncidentId);
                        }
                        else
                        {
                            res.IncidentId = -1;
                        }

                        res.ModifiedBy.Employee = Employee.GetByUserId(res.ModifiedBy.Id);
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

            return res;
        }

        public static IncidentAction GetByIncidentId(long incidentId, int companyId)
        {
            IncidentAction res = IncidentAction.Empty;
            using (SqlCommand cmd = new SqlCommand("IncidentAction_GetByIncidentId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res = new IncidentAction()
                        {
                            Id = rdr.GetInt64(ColumnsIncidentActionGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsIncidentActionGet.CompanyId),
                            Number = rdr.GetInt64(ColumnsIncidentActionGet.Number),
                            Description = rdr.GetString(ColumnsIncidentActionGet.Description),
                            ActionType = rdr.GetInt32(ColumnsIncidentActionGet.ActionType),
                            Origin = rdr.GetInt32(ColumnsIncidentActionGet.Origin),
                            ReporterType = rdr.GetInt32(ColumnsIncidentActionGet.ReporterType),
                            WhatHappened = rdr.GetString(ColumnsIncidentActionGet.WhatHappend),
                            WhatHappenedBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.WhatHappendId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.WhatHappendName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.WhatHappendLastName)
                            },
                            WhatHappenedOn = rdr.GetDateTime(ColumnsIncidentActionGet.WhatHappendOn),
                            Monitoring = rdr.GetString(ColumnsIncidentActionGet.Monitoring),
                            Notes = rdr.GetString(ColumnsIncidentActionGet.Notes),
                            Active = rdr.GetBoolean(ColumnsIncidentActionGet.Active),
                            ModifiedBy = new ApplicationUser
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsIncidentActionGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsIncidentActionGet.ModifiedOn)
                        };

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.CausesId))
                        {
                            res.Causes = rdr.GetString(ColumnsIncidentActionGet.Causes);
                            res.CausesOn = rdr.GetDateTime(ColumnsIncidentActionGet.CausesOn);
                            res.CausesBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.CausesId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.CausesName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.CausesLastName)
                            };
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.ActionsId))
                        {
                            res.Actions = rdr.GetString(ColumnsIncidentActionGet.Actions);
                            res.ActionsOn = rdr.GetDateTime(ColumnsIncidentActionGet.ActionsOn);
                            res.ActionsBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ActionsId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.ActionsName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.ActionsLastName)
                            };
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.ClosedId))
                        {
                            res.ClosedOn = rdr.GetDateTime(ColumnsIncidentActionGet.ClosedOn);
                            res.ClosedBy = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ClosedId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.ClosedName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.ClosedLastName)
                            };
                        }

                        if (!rdr.IsDBNull(ColumnsIncidentActionGet.ActionsExecuterId))
                        {
                            res.ActionsSchedule = rdr.GetDateTime(ColumnsIncidentActionGet.ActionsExecuterSchedule);
                            res.ActionsExecuter = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentActionGet.ActionsExecuterId),
                                Name = rdr.GetString(ColumnsIncidentActionGet.ActionsExecuterName),
                                LastName = rdr.GetString(ColumnsIncidentActionGet.ActionsExecuterLastName)
                            };
                        }

                        if (res.ReporterType == 1 && !rdr.IsDBNull(ColumnsIncidentActionGet.DepartmentId))
                        {
                            res.Department = new Department() { Id = rdr.GetInt32(ColumnsIncidentActionGet.DepartmentId) };
                        }

                        if (res.ReporterType == 2 && !rdr.IsDBNull(ColumnsIncidentActionGet.ProviderId))
                        {
                            res.Provider = new Provider() { Id = rdr.GetInt64(ColumnsIncidentActionGet.ProviderId) };
                        }

                        if (res.ReporterType == 3 && !rdr.IsDBNull(ColumnsIncidentActionGet.CustomerId))
                        {
                            res.Customer = new Customer() { Id = rdr.GetInt64(ColumnsIncidentActionGet.CustomerId) };
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

            return res;
        }

        public static IncidentAction GetByBusinessRiskId(long businessRiskId, int companyId)
        {
            IncidentAction res = IncidentAction.Empty;
            using (SqlCommand cmd = new SqlCommand("IncidentAction_GetByBusinessRiskId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", businessRiskId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        long actualBusinessRiskId = rdr.GetInt64(ColumnsIncidentActionGet.BusinessRiskId);
                        if (actualBusinessRiskId == businessRiskId)
                        {
                            res = new IncidentAction()
                            {
                                Id = rdr.GetInt64(ColumnsIncidentActionGet.Id),
                                CompanyId = rdr.GetInt32(ColumnsIncidentActionGet.CompanyId),
                                BusinessRiskId = businessRiskId,
                                Description = rdr.GetString(ColumnsIncidentActionGet.Description),
                                ActionType = rdr.GetInt32(ColumnsIncidentActionGet.ActionType),
                                Origin = rdr.GetInt32(ColumnsIncidentActionGet.Origin),
                                ReporterType = rdr.GetInt32(ColumnsIncidentActionGet.ReporterType),
                                WhatHappened = rdr.GetString(ColumnsIncidentActionGet.WhatHappend),
                                WhatHappenedBy = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIncidentActionGet.WhatHappendId),
                                    Name = rdr.GetString(ColumnsIncidentActionGet.WhatHappendName),
                                    LastName = rdr.GetString(ColumnsIncidentActionGet.WhatHappendLastName)
                                },
                                WhatHappenedOn = rdr.GetDateTime(ColumnsIncidentActionGet.WhatHappendOn),
                                Monitoring = rdr.GetString(ColumnsIncidentActionGet.Monitoring),
                                Notes = rdr.GetString(ColumnsIncidentActionGet.Notes),
                                Active = rdr.GetBoolean(ColumnsIncidentActionGet.Active),
                                ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsIncidentActionGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsIncidentActionGet.ModifiedByName)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsIncidentActionGet.ModifiedOn)
                            };

                            if (!rdr.IsDBNull(ColumnsIncidentActionGet.CausesId))
                            {
                                res.Causes = rdr.GetString(ColumnsIncidentActionGet.Causes);
                                res.CausesOn = rdr.GetDateTime(ColumnsIncidentActionGet.CausesOn);
                                res.CausesBy = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIncidentActionGet.CausesId),
                                    Name = rdr.GetString(ColumnsIncidentActionGet.CausesName),
                                    LastName = rdr.GetString(ColumnsIncidentActionGet.CausesLastName)
                                };
                            }

                            if (!rdr.IsDBNull(ColumnsIncidentActionGet.ActionsId))
                            {
                                res.Actions = rdr.GetString(ColumnsIncidentActionGet.Actions);
                                res.ActionsOn = rdr.GetDateTime(ColumnsIncidentActionGet.ActionsOn);
                                res.ActionsBy = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIncidentActionGet.ActionsId),
                                    Name = rdr.GetString(ColumnsIncidentActionGet.ActionsName),
                                    LastName = rdr.GetString(ColumnsIncidentActionGet.ActionsLastName)
                                };
                            }

                            if (!rdr.IsDBNull(ColumnsIncidentActionGet.ClosedId))
                            {
                                res.ClosedOn = rdr.GetDateTime(ColumnsIncidentActionGet.ClosedOn);
                                res.ClosedBy = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIncidentActionGet.ClosedId),
                                    Name = rdr.GetString(ColumnsIncidentActionGet.ClosedName),
                                    LastName = rdr.GetString(ColumnsIncidentActionGet.ClosedLastName)
                                };
                            }

                            if (!rdr.IsDBNull(ColumnsIncidentActionGet.ActionsExecuterId))
                            {
                                res.ActionsSchedule = rdr.GetDateTime(ColumnsIncidentActionGet.ActionsExecuterSchedule);
                                res.ActionsExecuter = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIncidentActionGet.ActionsExecuterId),
                                    Name = rdr.GetString(ColumnsIncidentActionGet.ActionsExecuterName),
                                    LastName = rdr.GetString(ColumnsIncidentActionGet.ActionsExecuterLastName)
                                };
                            }

                            if (res.ReporterType == 1 && !rdr.IsDBNull(ColumnsIncidentActionGet.DepartmentId))
                            {
                                res.Department = new Department() { Id = rdr.GetInt32(ColumnsIncidentActionGet.DepartmentId) };
                            }

                            if (res.ReporterType == 2 && !rdr.IsDBNull(ColumnsIncidentActionGet.ProviderId))
                            {
                                res.Provider = new Provider() { Id = rdr.GetInt64(ColumnsIncidentActionGet.ProviderId) };
                            }

                            if (res.ReporterType == 3 && !rdr.IsDBNull(ColumnsIncidentActionGet.CustomerId))
                            {
                                res.Customer = new Customer() { Id = rdr.GetInt64(ColumnsIncidentActionGet.CustomerId) };
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

            return res;
        }

        public static string FilterList(int companyId, DateTime? from, DateTime? to, bool statusIdentified, bool statusAnalyzed, bool statusInProgress, bool statusClose, bool typeImprovement, bool typeFix, bool typePrevent, int origin)
        {
            ReadOnlyCollection<IncidentActionFilterItem> items = Filter(companyId, from, to, statusIdentified, statusAnalyzed, statusInProgress, statusClose, typeImprovement, typeFix, typePrevent, origin);
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (IncidentActionFilterItem item in items)
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
                res.Append(Tools.JsonPair("Id", item.IncidentActionId)).Append(",");
                res.Append(Tools.JsonPair("Number", item.Number)).Append(",");
                res.Append(Tools.JsonPair("ActionType", item.ActionType)).Append(",");
                res.Append(Tools.JsonPair("Origin", item.Origin)).Append(",");
                res.Append(Tools.JsonPair("ReporterType", item.ReporterType)).Append(",");
                res.Append(Tools.JsonPair("Status", item.Status)).Append(",");
                res.Append(Tools.JsonPair("Description", item.Description)).Append(",");
                res.Append(Tools.JsonPair("OpenDate", item.OpenDate)).Append(",");
                res.Append(Tools.JsonPair("ImplementationDate", item.ImplementationDate)).Append(",");
                res.Append(Tools.JsonPair("CloseDate", item.CloseDate)).Append(",");
                res.Append(Tools.JsonPair("Amount", item.Amount)).Append(",");
                res.Append("\"Associated\":{\"Id\":").Append(item.Incident.Id).Append(",\"Description\":\"").Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}", Tools.JsonCompliant(item.Incident.Description))).Append("\"}");
                res.Append("}");
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<IncidentActionFilterItem> Filter(int companyId, DateTime? from, DateTime? to, bool statusIdentified, bool statusAnalyzed, bool statusInProgress, bool statusClose, bool typeImprovement, bool typeFix, bool typePrevent, int origin)
        {
            /* CREATE PROCEDURE IncidentActions_Filter
             *   @CompanyId int,
             *   @DateFrom datetime,
             *   @DateTo datetime,
             *   @StatusIdentified bit,
             *   @StatusAnalyzed bit,
             *   @StatusInProcess bit,
             *   @StatusClosed bit,
             *   @TypeImpovement bit,
             *   @TypeFix bit,
             *   @TypePrevent bit,
             *   @Origin int */
            List<IncidentActionFilterItem> res = new List<IncidentActionFilterItem>();
            using (SqlCommand cmd = new SqlCommand("IncidentActions_Filter"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@DateFrom", from));
                    cmd.Parameters.Add(DataParameter.Input("@DateTo", to));
                    cmd.Parameters.Add(DataParameter.Input("@StatusIdentified", statusIdentified));
                    cmd.Parameters.Add(DataParameter.Input("@StatusAnalyzed", statusAnalyzed));
                    cmd.Parameters.Add(DataParameter.Input("@StatusInProcess", statusInProgress));
                    cmd.Parameters.Add(DataParameter.Input("@StatusClosed", statusClose));
                    cmd.Parameters.Add(DataParameter.Input("@TypeImpovement", typeImprovement));
                    cmd.Parameters.Add(DataParameter.Input("@TypeFix", typeFix));
                    cmd.Parameters.Add(DataParameter.Input("@TypePrevent", typePrevent));
                    cmd.Parameters.Add(DataParameter.Input("@Origin", origin));

                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        IncidentActionFilterItem item = new IncidentActionFilterItem()
                        {
                            IncidentActionId = rdr.GetInt64(IndicentActionFilterGet.IncidentActionId),
                            Description = rdr.GetString(IndicentActionFilterGet.Description),
                            Origin = rdr.GetInt32(IndicentActionFilterGet.Origin),
                            ActionType = rdr.GetInt32(IndicentActionFilterGet.ActionType),
                            Number = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:00000}", rdr.GetInt64(IndicentActionFilterGet.Number)),
                            ReporterType = rdr.GetInt32(IndicentActionFilterGet.ReporterType),
                            Incident = new Incident()
                            {
                                Id = rdr.GetInt64(IndicentActionFilterGet.IncidentId),
                                Description = rdr.GetString(IndicentActionFilterGet.IncidentCode)
                            },
                            Amount = rdr.GetDecimal(IndicentActionFilterGet.Amount)
                        };

                        if (!rdr.IsDBNull(IndicentActionFilterGet.OpenDate))
                        {
                            item.OpenDate = rdr.GetDateTime(IndicentActionFilterGet.OpenDate);
                            item.Status = 1;
                        }

                        if (!rdr.IsDBNull(IndicentActionFilterGet.CausesDate))
                        {
                            item.Status = 2;
                        }

                        if (!rdr.IsDBNull(IndicentActionFilterGet.ImplementationDate))
                        {
                            item.ImplementationDate = rdr.GetDateTime(IndicentActionFilterGet.ImplementationDate);
                            item.Status = 3;
                        }

                        if (!rdr.IsDBNull(IndicentActionFilterGet.CloseDate))
                        {
                            item.CloseDate = rdr.GetDateTime(IndicentActionFilterGet.CloseDate);
                            item.Status = 4;
                        }

                        res.Add(item);
                    }
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }

                HttpContext.Current.Session["IncidentActionFilterData"] = res;
                return new ReadOnlyCollection<IncidentActionFilterItem>(res);
            }
        }

        /// <summary>
        /// Return an historical list of a businessRisk action
        /// </summary>
        /// <param name="code">Code identifier of the BusinessRisk</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>ReadOnlyCollection of BusinessRisk items</returns>
        public static ReadOnlyCollection<IncidentAction> GetByBusinessRiskCode(long code, int companyId)
        {
            List<IncidentAction> res = new List<IncidentAction>();
            string query = "IncidentAction_GetByBusinessRiskCode";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.Input("@BusinessRiskCode", code));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));

                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        res.Add(IncidentAction.GetById(rdr.GetInt64(0), companyId));
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<IncidentAction>(res);
        }

        /// <summary>
        /// Insert incident action in database
        /// </summary>
        /// <param name="userId">User identififer</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE IncidentAction_Insert
             *   @IncidentActionId bigint output,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @ActionType int,
             *   @Origin int,
             *   @ReportType int,
             *   @DeparmentId int,
             *   @ProviderId int,
             *   @CustomerId int,
             *   @Number bigint,
             *   @IncidentId bigint,
             *   @WhatHappend nvarchar(255),
             *   @WhatHappendBy int,
             *   @WhatHappendDate datetime,
             *   @Causes nvarchar(255),
             *   @CausesBy int,
             *   @CausesDate datetime,
             *   @Actions nvarchar(255),
             *   @ActionsBy int,
             *   @ActionsDate datetime,
             *   @ActionsExecuter int,
             *   @ActionsSchedule datetime,
             *   @Monitoring nvarchar(255),
             *   @ClosedBy int,
             *   @ClosedDate datetime,
             *   @Notes text,
             *   @UserId int */
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentAction_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputInt("@IncidentActionId"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description,100));
                    cmd.Parameters.Add(DataParameter.Input("@ActionType", this.ActionType));
                    cmd.Parameters.Add(DataParameter.Input("@Origin", this.Origin));
                    cmd.Parameters.Add(DataParameter.Input("@ReporterType", this.ReporterType));
                    cmd.Parameters.Add(DataParameter.Input("@Number", this.Number));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentId", this.IncidentId));
                    cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", this.BusinessRiskId));

                    cmd.Parameters.Add(DataParameter.Input("@DeparmentId", this.Department));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider));
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Customer));

                    //// if (this.Department == null) { cmd.Parameters.Add(DataParameter.InputNull("@DeparmentId")); } else { cmd.Parameters.Add(DataParameter.Input("@DeparmentId", this.Department.Id)); }
                    //// if (this.Provider == null) { cmd.Parameters.Add(DataParameter.InputNull("@ProviderId")); } else { cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id)); }
                    //// if (this.Customer == null) { cmd.Parameters.Add(DataParameter.InputNull("@CustomerId")); } else { cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Customer.Id)); }

                    cmd.Parameters.Add(DataParameter.Input("@WhatHappend", this.WhatHappened ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendBy", this.WhatHappenedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendDate", this.WhatHappenedOn));

                    cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@CausesBy", this.CausesBy));
                    cmd.Parameters.Add(DataParameter.Input("@CausesDate", this.CausesOn));

                    cmd.Parameters.Add(DataParameter.Input("@Actions", this.Actions ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsBy", this.ActionsBy));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsDate", this.ActionsOn));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsExecuter", this.ActionsExecuter));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsSchedule", this.ActionsSchedule));
                    cmd.Parameters.Add(DataParameter.Input("@Monitoring", this.Monitoring ?? string.Empty, 2000));

                    cmd.Parameters.Add(DataParameter.Input("@ClosedBy", this.ClosedBy));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedDate", this.ClosedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedExecutor", this.ClosedExecutorOn));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedExecutorOn", this.ClosedExecutorOn));

                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@IncidentActionId"].Value, CultureInfo.GetCultureInfo("en-us"));
                    result.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "IncidentAction::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Update incident action in database
        /// </summary>
        /// <param name="userId">User identififer</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            /* CREATE PROCEDURE IncidentAction_Update
             *   @IncidentActionId bigint,
             *   @CompanyId int,
             *   @ActionType int,
             *   @Description nvarchar(50),
             *   @Origin int,
             *   @ReporterType int,
             *   @DepartmentId int,
             *   @ProviderId bigint,
             *   @CustomerId bigint,
             *   @Number bigint,
             *   @IncidentId bigint,
             *   @WhatHappend nvarchar(155),
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
             *   @Monitoring nvarchar(255),
             *   @ClosedBy int,
             *   @ClosedOn datetime,
             *   @ClosedExecutor bigint,
             *   @ClosedExecutorOn datetime,
             *   @Notes text,
             *   @UserId int  */
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentAction_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description,100));
                    cmd.Parameters.Add(DataParameter.Input("@ActionType", this.ActionType));
                    cmd.Parameters.Add(DataParameter.Input("@Origin", this.Origin));
                    cmd.Parameters.Add(DataParameter.Input("@ReporterType", this.ReporterType));
                    cmd.Parameters.Add(DataParameter.Input("@Number", this.Number));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentId", this.IncidentId));
                    cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", this.BusinessRiskId));

                    cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.Department));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider));
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Customer));

                    cmd.Parameters.Add(DataParameter.Input("@WhatHappend", this.WhatHappened ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendBy", this.WhatHappenedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendOn", this.WhatHappenedOn));

                    cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@CausesBy", this.CausesBy));
                    cmd.Parameters.Add(DataParameter.Input("@CausesOn", this.CausesOn));

                    cmd.Parameters.Add(DataParameter.Input("@Actions", this.Actions ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsBy", this.ActionsBy));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsOn", this.ActionsOn));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsExecuter", this.ActionsExecuter));
                    cmd.Parameters.Add(DataParameter.Input("@ActionsSchedule", this.ActionsSchedule));
                    cmd.Parameters.Add(DataParameter.Input("@Monitoring", this.Monitoring ?? string.Empty, 2000));

                    cmd.Parameters.Add(DataParameter.Input("@ClosedBy", this.ClosedBy));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedOn", this.ClosedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedExecutor", this.ClosedExecutor));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedExecutorOn", this.ClosedExecutorOn));

                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes ?? string.Empty, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.SetSuccess();
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (InvalidCastException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (FormatException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NotSupportedException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Delete incident action in database
        /// </summary>
        /// <param name="userId">User identififer</param>
        /// <returns>Result of action</returns>
        public ActionResult Delete(int userId)
        {
            /* CREATE PROCEDURE IncidentAction_Delete
             *   @IncidentActionId bigint,
             *   @CompanyId int,
             *   @UserId int  */
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentAction_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.SetSuccess();
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (InvalidCastException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (FormatException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NotSupportedException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentAction::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// HTML Table containing fields to be showed
        /// </summary>
        /// <param name="dictionary">Dictionary containing terms to be showed</param>
        /// <param name="grants">Gets the grants of the user</param>
        /// <returns>String containing HTML table</returns>
        public string ListBusinessRiskRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants, int order)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantIncidentActions = UserGrant.HasWriteGrant(grants, ApplicationGrant.IncidentActions);
            bool grantIncidentActionsDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.IncidentActions);

            string iconView = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""ActionsDialog(this);""><i class=""icon-edit bigger-120""></i></span>", this.Id, Tools.SetTooltip(this.Description), Tools.JsonCompliant(dictionary["Common_Edit"]));

            if (this.WhatHappenedOn != null)
            {
                this.Status = @"<i class=""fa icon-pie-chart"" style=""color: rgb(255, 0, 0);""></i>" + dictionary["Item_Incident_Status1"];
            }

            if (this.CausesOn != null)
            {
                this.Status = @"<i class=""fa icon-pie-chart"" style=""color: rgb(221, 221, 0);""></i>" + dictionary["Item_Incident_Status2"];
            }

            if (this.ActionsOn != null)
            {
                this.Status = @"<i class=""fa icon-play"" style=""color: rgb(0, 119, 0);""></i>" + dictionary["Item_Incident_Status3"];
            }

            if (this.ClosedOn != null)
            {
                this.Status = @"<i class=""fa icon-lock"" style=""color: rgb(0, 0, 0);""></i>" + dictionary["Item_Incident_Status4"];
            }

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr id=""{1}""><td>{0:dd/MM/yyyy}</td><td>{2}</td><td class=""hidden-480"">{3}</td><td class=""hidden-480""> {4:dd/MM/yyyy}</td><td class=""hidden-480"">{5:dd/MM/yyyy}</td><td>{6}</td></tr>",
                this.WhatHappenedOn,
                this.Id,
                this.Status,
                this.Description,
                this.ActionsOn,
                this.ClosedOn,
                iconView);
        }
    }
}