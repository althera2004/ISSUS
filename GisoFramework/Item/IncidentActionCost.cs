﻿// --------------------------------
// <copyright file="IncidentActionCost.cs" company="Sbrinna">
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

    /// <summary>
    /// Implements IncidentActionCost class
    /// </summary>
    public class IncidentActionCost : BaseItem
    {
        public static IncidentActionCost Empty
        {
            get
            {
                return new IncidentActionCost()
                {
                    Id = 0,
                    IncidentActionId = 0,
                    CompanyId = 0,
                    Description = string.Empty,
                    Amount = 0,
                    Quantity = 0,
                    Responsible = Employee.EmptySimple,
                    Active = true
                };
            }
        }

        /// <summary>
        /// Gets or sets the action identifier
        /// </summary>
        public long IncidentActionId { get; set; }

        /// <summary>
        /// Gets or sets the amount of cost
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the quantity of cost
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the responsible of cost
        /// </summary>
        public Employee Responsible { get; set; }

        /// <summary>
        /// Create a link for the cost
        /// </summary>
        public override string Link
        {
            get { return string.Empty; }
        }

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
                StringBuilder res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",");
                res.Append(Tools.JsonPair("IncidentActionId", this.IncidentActionId)).Append(",");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Amount", this.Amount)).Append(",");
                res.Append(Tools.JsonPair("Quantity", this.Quantity)).Append(",");
                res.Append(Tools.JsonPair("Responsible", this.Responsible)).Append(",");
                res.Append(Tools.JsonPair("Active", this.Active)).Append("}");
                return res.ToString();
            }
        }

        /// <summary>
        /// Get the costs of action
        /// </summary>
        /// <param name="incidentActionId">Incident action identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A list of cost of the action</returns>
        public static string GetByIncidentAction(long incidentActionId, int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            ReadOnlyCollection<IncidentActionCost> costs = IncidentActionCost.GetByIncidentActionId(incidentActionId, companyId);
            foreach (IncidentActionCost cost in costs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(cost.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Get the cost of company
        /// </summary>
        /// <param name="companyId">Company identififer</param>
        /// <returns>A list of cost of the company</returns>
        public static string GetByCompany(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            ReadOnlyCollection<IncidentActionCost> costs = IncidentActionCost.GetByCompanyId(companyId);
            foreach (IncidentActionCost cost in costs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(cost.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Gets the differences between tow costs
        /// </summary>
        /// <param name="item1">Cost for compare</param>
        /// <param name="item2">Cost to compare</param>
        /// <returns></returns>
        public static string Differences(IncidentActionCost item1, IncidentActionCost item2)
        {
            if(item1==null || item2==null)
            {
                return string.Empty;
            }

            StringBuilder res = new StringBuilder();

            if (item1.Description != item2.Description)
            {
                res.Append("Description=>").Append(item2.Description).Append(";");
            }

            if (item1.Amount != item2.Amount)
            {
                res.Append("Amount=>").Append(item2.Amount).Append(";");
            }

            if (item1.Quantity != item2.Quantity)
            {
                res.Append("Quantity=>").Append(item2.Quantity).Append(";");
            }

            return res.ToString();
        }

        /// <summary>
        /// Delete a cost on database
        /// </summary>
        /// <param name="incidentCostId">Cost identifier</param>
        /// <param name="userId">Identifier of user thats performs the action</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns></returns>
        public static ActionResult Delete(long incidentCostId, int userId, int companyId)
        {
            /* CREATE PROCEDURE IncidentActionCost_Delete
             *   @IncidentActionCostId bigint,
             *   @CompanyId int,
             *   @UserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentActionCost_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionCostId", incidentCostId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentActionCost::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentCostId, userId, companyId));
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentActionCost::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentCostId, userId, companyId));
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

        /// <summary>
        /// Insert a cost into database
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>The result of action</returns>
        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE IncidentActionCost_Insert
             *   @IncidentActionCostId bigint output,
             *   @IncidentActionId bigint,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @Amount numeric(18,3),
             *   @Quantity numeric(18,3),
             *   @ResponsablebleId int,
             *   @UserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentActionCost_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputInt("@IncidentActionCostId"));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionId", this.IncidentActionId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                    cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                    cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@IncidentActionCostId"].Value, CultureInfo.GetCultureInfo("en-us"));
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

            return res;
        }

        /// <summary>
        /// Update a cost on database
        /// </summary>
        /// <param name="userId">Identifier of user that peforms the action</param>
        /// <param name="differences">Differences with previous costs data</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId, string differences)
        {
            /* CREATE PROCEDURE IncidentActionCost_Update
             *   @IncidentActionCostId bigint,
             *   @IncidentActionId bigint,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @Amount numeric(18,3),
             *   @Quantity numeric(18,3),
             *   @ResponsableId int,
             *   @UserId int,
             *   @Differences text */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentActionCost_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionCostId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionId", this.IncidentActionId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                    cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                    cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@Differences", differences));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentActionCost::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentActionCost::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

        /// <summary>
        /// Delete a cost on database
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Delete(int userId)
        {
            return Delete(this.Id, userId, this.CompanyId);
        }

        public static ReadOnlyCollection<IncidentActionCost> GetByIncidentActionId(long incidentActionId, int companyId)
        {
            /* CREATE PROCEDURE IndecidentActionCost_GetByIndicentActionId
             *   @IncidentId bigint,
             *   @CompanyId int */
            List<IncidentActionCost> res = new List<IncidentActionCost>();
            using (SqlCommand cmd = new SqlCommand("IndecidentActionCost_GetByIndicentActionId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IncidentActionId", incidentActionId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new IncidentActionCost()
                        {
                            Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                            IncidentActionId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
                            Description = rdr.GetString(ColumnsIncidentCostGet.Description),
                            Amount = rdr.GetDecimal(ColumnsIncidentCostGet.Amount),
                            Quantity = rdr.GetDecimal(ColumnsIncidentCostGet.Quantity),
                            Responsible = new Employee()
                            {
                                Id = rdr.GetInt64(ColumnsIncidentCostGet.ResponsibleId),
                                Name = rdr.GetString(ColumnsIncidentCostGet.ResponsibleName),
                                LastName = rdr.GetString(ColumnsIncidentCostGet.ResponsibleLastName)
                            },
                            Active = rdr.GetBoolean(ColumnsIncidentCostGet.Active)
                        });
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

            return new ReadOnlyCollection<IncidentActionCost>(res);
        }

        private static ReadOnlyCollection<IncidentActionCost> GetByCompanyId(int companyId)
        {
            /* CREATE PROCEDURE IndecidentActionCost_GetByCompanyId
             *   @CompanyId int */
            List<IncidentActionCost> res = new List<IncidentActionCost>();
            using (SqlCommand cmd = new SqlCommand("IndecidentActionCost_GetByCompanyId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new IncidentActionCost()
                        {
                            Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                            IncidentActionId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
                            Description = rdr.GetString(ColumnsIncidentCostGet.Description),
                            Amount = rdr.GetDecimal(ColumnsIncidentCostGet.Amount),
                            Quantity = rdr.GetDecimal(ColumnsIncidentCostGet.Quantity),
                            Responsible = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIncidentCostGet.ResponsibleId),
                                Name = rdr.GetString(ColumnsIncidentCostGet.ResponsibleName),
                                LastName = rdr.GetString(ColumnsIncidentCostGet.ResponsibleLastName)
                            },
                            Active = rdr.GetBoolean(ColumnsIncidentCostGet.Active)
                        });
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

            return new ReadOnlyCollection<IncidentActionCost>(res);
        }

        public static string GetByBusinessRisk(long businessRiskId, int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            ReadOnlyCollection<IncidentActionCost> costs = IncidentActionCost.GetByBusinessRiskId(businessRiskId, companyId);
            foreach (IncidentActionCost cost in costs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(cost.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<IncidentActionCost> GetByBusinessRiskId(long businessRiskId, int companyId)
        {
            List<IncidentActionCost> res = new List<IncidentActionCost>();
            using (SqlCommand cmd = new SqlCommand("IndecidentCost_GetByBusinessRiskId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", businessRiskId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new IncidentActionCost()
                        {
                            Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                            IncidentActionId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
                            Description = rdr.GetString(ColumnsIncidentCostGet.Description),
                            Amount = rdr.GetDecimal(ColumnsIncidentCostGet.Amount),
                            Quantity = rdr.GetDecimal(ColumnsIncidentCostGet.Quantity),
                            Responsible = new Employee()
                            {
                                Id = Convert.ToInt64(rdr.GetInt32(ColumnsIncidentCostGet.ResponsibleId)),
                                Name = rdr.GetString(ColumnsIncidentCostGet.ResponsibleName),
                                LastName = rdr.GetString(ColumnsIncidentCostGet.ResponsibleLastName)
                            },
                            Active = rdr.GetBoolean(ColumnsIncidentCostGet.Active)
                        });
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

            return new ReadOnlyCollection<IncidentActionCost>(res);
        }
    }
}
