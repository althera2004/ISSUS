// -----------------------------------------------------------------------
// <copyright file="IncidentCost.cs" company="Microsoft">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements IncidentCost class</summary>
    public class IncidentCost : BaseItem
    {
        public long IncidentId { get; set; }

        public long BusinessRiskId { get; set; }

        public decimal Amount { get; set; }

        public decimal Quantity { get; set; }

        public Employee Responsible { get; set; }

        public string Source { get; set; }

        public static IncidentCost Empty
        {
            get
            {
                return new IncidentCost
                {
                    Id = 0,
                    IncidentId = 0,
                    BusinessRiskId = 0,
                    CompanyId = 0,
                    Description = string.Empty,
                    Amount = 0,
                    Quantity = 0,
                    Responsible = Employee.EmptySimple,
                    Active = true
                };
            }
        }

        public override string Link
        {
            get { return string.Empty; }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{{""Id"": {0}, ""Description"": ""{1}""}}", this.Id, Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(", ");
                res.Append(Tools.JsonPair("IncidentId", this.IncidentId)).Append(", ");
                res.Append(Tools.JsonPair("BusinessRiskId", this.BusinessRiskId)).Append(", ");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(", ");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(", ");
                res.Append(Tools.JsonPair("Amount", this.Amount)).Append(", ");
                res.Append(Tools.JsonPair("Quantity", this.Quantity)).Append(", ");
                res.Append(Tools.JsonPair("Responsible", this.Responsible)).Append(", ");
                res.Append(Tools.JsonPair("Active", this.Active)).Append(", ");
                res.Append(Tools.JsonPair("Source", this.Source)).Append("}");
                return res.ToString();
            }
        }

        public static ReadOnlyCollection<IncidentCost> AllCosts(long incidentId, int companyId)
        {
            var costs = ByIncidentId(incidentId, companyId).ToList();
            var actionCosts = new ReadOnlyCollection<IncidentActionCost>(new List<IncidentActionCost>());
            var action = IncidentAction.ByIncidentId(incidentId, companyId);
            if (action.Id > 0)
            {
                actionCosts = IncidentActionCost.GetByIncidentActionId(action.Id, companyId);
                foreach (var actionCost in actionCosts)
                {
                    costs.Add(new IncidentCost
                    {
                        Id = actionCost.Id,
                        IncidentId = incidentId,
                        BusinessRiskId = actionCost.IncidentActionId,
                        Description = actionCost.Description,
                        Quantity = actionCost.Quantity,
                        CompanyId = actionCost.CompanyId,
                        Responsible = actionCost.Responsible,
                        Amount = actionCost.Amount,
                        Active = actionCost.Active,
                        Source = "A"
                    });
                }
            }

            return new ReadOnlyCollection<IncidentCost>(costs);
        }

        public static string ByIncident(long incidentId, int companyId)
        {
            var res = new StringBuilder("[");
            var first = true;
            var costs = AllCosts(incidentId, companyId);
            /*List<IncidentCost> costs = GetByIncidentId(incidentId, companyId).ToList();
            ReadOnlyCollection<IncidentActionCost> actionCosts = new ReadOnlyCollection<IncidentActionCost>(new List<IncidentActionCost>());

            IncidentAction action = IncidentAction.GetByIncidentId(incidentId,companyId);
            if(action.Id > 0){
                actionCosts = IncidentActionCost.GetByIncidentActionId(action.Id, companyId);
                foreach (IncidentActionCost actionCost in actionCosts)
                {
                    costs.Add(new IncidentCost()
                    {
                        Id = actionCost.Id,
                        IncidentId = incidentId,
                        BusinessRiskId = actionCost.IncidentActionId,
                        Description = actionCost.Description,
                        Quantity = actionCost.Quantity,
                        CompanyId = actionCost.CompanyId,
                        Responsible = actionCost.Responsible,
                        Amount = actionCost.Amount,
                        Active = actionCost.Active,
                        Source = "A"
                    });
                }
            }*/

            foreach (var cost in costs)
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

        public static string ByCompany(int companyId)
        {
            var res = new StringBuilder("[");
            var first = true;
            var costs = ByCompanyId(companyId);
            foreach (var cost in costs)
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

        public static string ByBusinessRisk(long businessRiskId, int companyId)
        {
            var res = new StringBuilder("[");
            var first = true;
            var costs = ByBusinessRiskId(businessRiskId, companyId);
            foreach (var cost in costs)
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

        public static ReadOnlyCollection<IncidentCost> ByIncidentId(long incidentId, int companyId)
        {
            /* CREATE PROCEDURE IndecidentCost_GetByIndicentId
             *   @IncidentId bigint,
             *   @CompanyId int */
            var res = new List<IncidentCost>();
            using (var cmd = new SqlCommand("IndecidentCost_GetByIndicentId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new IncidentCost
                                {
                                    Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                                    IncidentId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
                                    Description = rdr.GetString(ColumnsIncidentCostGet.Description),
                                    Amount = rdr.GetDecimal(ColumnsIncidentCostGet.Amount),
                                    Quantity = rdr.GetDecimal(ColumnsIncidentCostGet.Quantity),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentCostGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsIncidentCostGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsIncidentCostGet.ResponsibleLastName)
                                    },
                                    Active = rdr.GetBoolean(ColumnsIncidentCostGet.Active),
                                    Source = "I"
                                });
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

            return new ReadOnlyCollection<IncidentCost>(res);
        }

        private static ReadOnlyCollection<IncidentCost> ByCompanyId(int companyId)
        {
            /* CREATE PROCEDURE IndecidentCost_GetByCompanyId
             *   @CompanyId int */
            var res = new List<IncidentCost>();
            using (var cmd = new SqlCommand("IndecidentCost_GetByCompanyId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new IncidentCost
                                {
                                    Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                                    IncidentId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
                                    Description = rdr.GetString(ColumnsIncidentCostGet.Description),
                                    Amount = rdr.GetDecimal(ColumnsIncidentCostGet.Amount),
                                    Quantity = rdr.GetDecimal(ColumnsIncidentCostGet.Quantity),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentCostGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsIncidentCostGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsIncidentCostGet.ResponsibleLastName)
                                    },
                                    Active = rdr.GetBoolean(ColumnsIncidentCostGet.Active)
                                });
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

            return new ReadOnlyCollection<IncidentCost>(res);
        }

        public static ReadOnlyCollection<IncidentCost> ByBusinessRiskId(long businessRiskId, int companyId)
        {
            var res = new List<IncidentCost>();
            using (var cmd = new SqlCommand("IndecidentCost_GetByBusinessRiskId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", businessRiskId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new IncidentCost
                                {
                                    Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                                    IncidentId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
                                    BusinessRiskId = businessRiskId,
                                    Description = rdr.GetString(ColumnsIncidentCostGet.Description),
                                    Amount = rdr.GetDecimal(ColumnsIncidentCostGet.Amount),
                                    Quantity = rdr.GetDecimal(ColumnsIncidentCostGet.Quantity),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsIncidentCostGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsIncidentCostGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsIncidentCostGet.ResponsibleLastName)
                                    },
                                    Active = rdr.GetBoolean(ColumnsIncidentCostGet.Active)

                                });
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

            return new ReadOnlyCollection<IncidentCost>(res);
        }

        public string Differences(IncidentCost other)
        {
            if (this == null || other == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();

            if (this.Description != other.Description)
            {
                res.Append("Description=>").Append(other.Description).Append(";");
            }

            if (this.Amount != other.Amount)
            {
                res.Append("Amount=>").Append(other.Amount).Append(";");
            }

            if (this.Quantity != other.Quantity)
            {
                res.Append("Quantity=>").Append(other.Quantity).Append(";");
            }

            return res.ToString();
        }

        public ActionResult Insert(int userId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name:{1}", this.Id, this.Description);
            /* CREATE PROCEDURE IncidentCost_Insert
             * @IncidentCostId bigint output,
             * @IncidentId bigint,
             * @CompanyId int,
             * @Description nvarchar(50),	
             * @Amount numeric(18,3),
             * @Quantity numeric(18,3),
             * @ResponsablebleId int,
             * @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("IncidentCost_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@IncidentCostId"));
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", this.IncidentId));
                        cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", this.BusinessRiskId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                        cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@IncidentCostId"].Value, CultureInfo.InvariantCulture);
                        res.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Insert", source);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Incident::Insert", source);
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
            string source = string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name{1}", this.Id, this.Description);
            /* CREATE PROCEDURE IncidentCost_Update
             *   @IncidentCostId bigint,
             *   @IncidentId bigint,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @Amount numeric(18,3),
             *   @Quantity numeric(18,3),
             *   @ResponsableId int,
             *   @UserId int,
             *   @Differences text */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("IncidentCost_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentCostId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@IncidentId", this.IncidentId));
                        cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", this.BusinessRiskId));
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
                        ExceptionManager.Trace(ex, "IncidentCost::Update", source);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "IncidentCost::Update", source);
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

        public static ActionResult Delete(long incidentCostId, int userId, int companyId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Id:{0} - UserId:{1} - CompanyId:{2}", incidentCostId, userId, companyId);
            /* CREATE PROCEDURE IncidentCost_Delete
             *   @IncidentCostId bigint,
             *   @CompanyId int,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("IncidentCost_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IncidentCostId", incidentCostId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "IncidentCost::Delete", source);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "IncidentCost::Delete", source);
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
    }
}