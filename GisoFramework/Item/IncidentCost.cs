// -----------------------------------------------------------------------
// <copyright file="IncidentCost.cs" company="Microsoft">
// TODO: Update copyright text.
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

    /// <summary>
    /// Implements IncidentCost class
    /// </summary>
    public class IncidentCost : BaseItem
    {
        public long IncidentId { get; set; }
        public long BusinessRiskId { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public Employee Responsible { get; set; }

        public static IncidentCost Empty
        {
            get
            {
                return new IncidentCost()
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
                res.Append(Tools.JsonPair("IncidentId", this.IncidentId)).Append(",");
                res.Append(Tools.JsonPair("BusinessRiskId", this.BusinessRiskId)).Append(",");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Amount", this.Amount)).Append(",");
                res.Append(Tools.JsonPair("Quantity", this.Quantity)).Append(",");
                res.Append(Tools.JsonPair("Responsible", this.Responsible)).Append(",");
                res.Append(Tools.JsonPair("Active", this.Active)).Append("}");
                return res.ToString();
            }
        }

        public static string GetByIncident(long incidentId, int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            List<IncidentCost> costs = GetByIncidentId(incidentId, companyId).ToList();            
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
                        Quantity = actionCost.Quantity,
                        CompanyId = actionCost.CompanyId,
                        Responsible = actionCost.Responsible,
                        Amount = actionCost.Amount,
                        Active = actionCost.Active
                    });
                }
            }


            foreach (IncidentCost cost in costs)
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

        public static string GetByCompany(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            ReadOnlyCollection<IncidentCost> costs = GetByCompanyId(companyId);
            foreach (IncidentCost cost in costs)
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

        public static string GetByBusinessRisk(long businessRiskId, int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            ReadOnlyCollection<IncidentCost> costs = GetByBusinessRiskId(businessRiskId, companyId);
            foreach (IncidentCost cost in costs)
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

        public static ReadOnlyCollection<IncidentCost> GetByIncidentId(long incidentId, int companyId)
        {
            /* CREATE PROCEDURE IndecidentCost_GetByIndicentId
             *   @IncidentId bigint,
             *   @CompanyId int */
            List<IncidentCost> res = new List<IncidentCost>();
            using (SqlCommand cmd = new SqlCommand("IndecidentCost_GetByIndicentId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IncidentId", incidentId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new IncidentCost()
                        {
                            Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                            IncidentId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
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

            return new ReadOnlyCollection<IncidentCost>(res);
        }

        private static ReadOnlyCollection<IncidentCost> GetByCompanyId(int companyId)
        {
            /* CREATE PROCEDURE IndecidentCost_GetByCompanyId
             *   @CompanyId int */
            List<IncidentCost> res = new List<IncidentCost>();
            using (SqlCommand cmd = new SqlCommand("IndecidentCost_GetByCompanyId"))
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
                        res.Add(new IncidentCost()
                        {
                            Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                            IncidentId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
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

            return new ReadOnlyCollection<IncidentCost>(res);
        }

        public static ReadOnlyCollection<IncidentCost> GetByBusinessRiskId(long businessRiskId, int companyId)
        {
            List<IncidentCost> res = new List<IncidentCost>();
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
                        res.Add(new IncidentCost()
                        {
                            Id = rdr.GetInt64(ColumnsIncidentCostGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsIncidentCostGet.CompanyId),
                            IncidentId = rdr.GetInt64(ColumnsIncidentCostGet.IncidentActionId),
                            BusinessRiskId = businessRiskId,
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

            return new ReadOnlyCollection<IncidentCost>(res);
        }

        public static string Differences(IncidentCost item1, IncidentCost item2)
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

        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE IncidentCost_Insert
             * @IncidentCostId bigint output,
             * @IncidentId bigint,
             * @CompanyId int,
             * @Description nvarchar(50),	
             * @Amount numeric(18,3),
             * @Quantity numeric(18,3),
             * @ResponsablebleId int,
             * @UserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentCost_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputInt("@IncidentCostId"));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentId", this.IncidentId));
                    cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", this.BusinessRiskId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description,100));
                    cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                    cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                    cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@IncidentCostId"].Value, CultureInfo.GetCultureInfo("en-us"));
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

        public ActionResult Update(int userId, string differences)
        {
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
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentCost_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IncidentCostId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@IncidentId", this.IncidentId));
                    cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", this.BusinessRiskId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description,100));
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
                    ExceptionManager.Trace(ex, "IncidentCost::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentCost::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

        public ActionResult Delete(int userId)
        {
            return Delete(this.Id, userId, this.CompanyId);
        }

        public static ActionResult Delete(long incidentCostId, int userId, int companyId)
        {
            /* CREATE PROCEDURE IncidentCost_Delete
             *   @IncidentCostId bigint,
             *   @CompanyId int,
             *   @UserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("IncidentCost_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
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
                    ExceptionManager.Trace(ex, "IncidentCost::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentCostId, userId, companyId));
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "IncidentCost::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - UserId:{1} - CompanyId:{2}", incidentCostId, userId, companyId));
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
    }
}
