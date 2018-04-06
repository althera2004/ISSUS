// --------------------------------
// <copyright file="EquipmentMaintenanceAct.cs" company="Sbrinna">
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

    /// <summary>Implements EquipmentMaintenanceAct class</summary>
    public class EquipmentMaintenanceAct : BaseItem
    {
        public static EquipmentMaintenanceAct Empty
        {
            get
            {
                return new EquipmentMaintenanceAct
                {
                    Id = -1,
                    EquipmentMaintenanceDefinitionId = -1,
                    EquipmentId = -1,
                    Provider = Provider.Empty,
                    Responsible = Employee.Empty
                };
            }
        }

        public long EquipmentMaintenanceDefinitionId { get; set; }

        public long EquipmentId { get; set; }

        public DateTime Date { get; set; }

        public string Observations { get; set; }

        public Provider Provider { get; set; }

        public Employee Responsible { get; set; }

        public decimal? Cost { get; set; }

        public DateTime Expiration { get; set; }

        public override string Link
        {
            get { return string.Empty; }
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
                var res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",");
                res.Append(Tools.JsonPair("EquipmentId", this.EquipmentId)).Append(",");
                res.Append(Tools.JsonPair("EquipmentMaintenanceDefinitionId", this.EquipmentMaintenanceDefinitionId)).Append(",");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Observations", this.Observations)).Append(",");
                res.Append(Tools.JsonPair("Expiration", this.Expiration)).Append(",");
                res.Append(Tools.JsonPair("Date", this.Date)).Append(",");
                res.Append(Tools.JsonPair("Cost", this.Cost)).Append(",");
                res.Append(Tools.JsonPair("Active", this.Active)).Append(",");

                if (this.Provider == null)
                {
                    res.Append("\"Provider\":null,");
                }
                else
                {
                    res.Append("\"Provider\":").Append(this.Provider.JsonKeyValue).Append(",");
                }

                if (this.Responsible == null)
                {
                    res.Append("\"Responsible\":null}");
                }
                else
                {
                    res.Append("\"Responsible\":").Append(this.Responsible.JsonKeyValue).Append("}");
                }

                return res.ToString();
            }
        }

        public static string JsonList(long equipmentId, int companyId)
        {
            var res = new StringBuilder("[");
            var maintenance = GetByCompany(equipmentId, companyId);
            bool first = true;
            foreach (var item in maintenance)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(item.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<EquipmentMaintenanceAct> GetByCompany(long equipmentId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentMaintenanceAct_GetByEquipmentId
             *   @EquipmentId bigint,
             *   @CompanyId int */
            var res = new List<EquipmentMaintenanceAct>();
            using (var cmd = new SqlCommand("EquipmentMaintenanceAct_GetByEquipmentId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", equipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                EquipmentMaintenanceAct newMaintenanceAct = new EquipmentMaintenanceAct()
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentMaintenanceActGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsEquipmentMaintenanceActGet.CompanyId),
                                    EquipmentId = rdr.GetInt64(ColumnsEquipmentMaintenanceActGet.EquipmentId),
                                    Description = rdr.GetString(ColumnsEquipmentMaintenanceActGet.Operation),
                                    Observations = rdr.GetString(ColumnsEquipmentMaintenanceActGet.Observations),
                                    Date = rdr.GetDateTime(ColumnsEquipmentMaintenanceActGet.Date),
                                    EquipmentMaintenanceDefinitionId = rdr.GetInt64(ColumnsEquipmentMaintenanceActGet.EquipmentMaintenanceDefinitionId),
                                    Expiration = rdr.GetDateTime(ColumnsEquipmentMaintenanceActGet.Expiration),
                                    Active = rdr.GetBoolean(ColumnsEquipmentMaintenanceActGet.Active),
                                    Provider = new Provider()
                                    {
                                        Id = rdr.GetInt64(ColumnsEquipmentMaintenanceActGet.ProviderId),
                                        Description = rdr.GetString(ColumnsEquipmentMaintenanceActGet.ProviderDescription)
                                    },
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentMaintenanceActGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentMaintenanceActGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentMaintenanceActGet.ResponsibleLastName)
                                    },
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentMaintenanceActGet.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentMaintenanceActGet.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentMaintenanceActGet.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentMaintenanceActGet.Cost))
                                {
                                    newMaintenanceAct.Cost = rdr.GetDecimal(ColumnsEquipmentMaintenanceActGet.Cost);
                                }

                                res.Add(newMaintenanceAct);
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

            return new ReadOnlyCollection<EquipmentMaintenanceAct>(res);
        }

        public static string Differences(EquipmentMaintenanceAct item1, EquipmentMaintenanceAct item2)
        {
            if (item1 == null || item2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            if (item1.Description != item2.Description)
            {
                res.Append("Operation:").Append(item2.Description).Append("; ");
            }

            if (item1.Observations != item2.Observations)
            {
                res.Append("Observations:").Append(item2.Observations).Append("; ");
            }

            if (item1.Date != item2.Date)
            {
                res.Append("Date:").Append(item2.Date).Append("; ");
            }

            if (item1.Expiration != item2.Expiration)
            {
                res.Append("Vto:").Append(item2.Expiration).Append("; ");
            }

            if (item1.Cost != item2.Cost)
            {
                res.Append("Cost:").Append(item2.Cost).Append("; ");
            }

            if (item1.Provider != null || item2.Provider != null)
            {
                if (item1.Provider == null)
                {
                    res.Append("Provider:").Append(item2.Provider.Id).Append("; ");
                }
                else if (item2.Provider == null)
                {
                    res.Append("Provider:null; ");
                }
                else if (item1.Provider.Id != item2.Provider.Id)
                {
                    res.Append("Provider:").Append(item2.Provider.Id).Append("; ");
                }
            }

            if (item1.Responsible.Id != item2.Responsible.Id)
            {
                res.Append("Responsible:").Append(item2.Responsible.Id).Append("; ");
            }

            return res.ToString();
        }

        public static ActionResult Delete(int equipmentMaintenanceActId, int userId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentMaintenanceAct_Delete
             *   @EquipmentMaintenanceActId bigint,
             *   @CompanyId int,
             *   @UserId int */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentMaintenanceAct_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentMaintenanceActId", equipmentMaintenanceActId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceActId, userId, companyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceActId, userId, companyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceActId, userId, companyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceActId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceActId, userId, companyId));
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

            return result;
        }

        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE EquipmentMaintenanceAct_Insert
             *   @EquipmentMaintenanceActId bigint output,
             *   @EquipmentMaintenanceDefinitionId bigint,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Date datetime,
             *   @Operation nvarchar(50),
             *   @Observations text,
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @Cost numeric(18,3),
             *   @Vto datetime,
             *   @UserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentMaintenanceAct_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentMaintenanceActId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentMaintenanceDefinitionId", this.EquipmentMaintenanceDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        if (this.Provider == null)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@ProviderId"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id));
                        }

                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentMaintenanceActId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        result.Success = true;
                        result.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EquipmentMaintenanceAct::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "EquipmentMaintenanceActInsert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

            return result;
        }

        public ActionResult Update(string differences, int userId)
        {
            /* CREATE PROCEDURE EquipmentMaintenanceAct_Update
             *   @EquipmentMaintenanceActId bigint,
             *   @CompanyId int,
             *   @Date datetime,
             *   @Operation nvarchar(50),
             *   @Observations text,
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @Cost numeric(18,3),
             *   @Vto datetime,
             *   @UserId int */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentMaintenanceAct_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentMaintenanceActId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations));

                        if (this.Provider == null)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@ProviderId"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id));
                        }

                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
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

            return result;
        }
    }
}