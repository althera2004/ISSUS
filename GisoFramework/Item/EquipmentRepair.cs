// --------------------------------
// <copyright file="EquipmentRepair.cs" company="Sbrinna">
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

    /// <summary>Implements EquipmentRepair class</summary>
    public class EquipmentRepair
    {
        /// <summary>Gets a empty repair</summary>
        public static EquipmentRepair Empty
        {
            get
            {
                return new EquipmentRepair
                {
                    Id = -1,
                    EquipmentId = -1,
                    Provider = Provider.Empty,
                    Responsible = Employee.Empty,
                    Cost = null
                };
            }
        }

        /// <summary>Gets or sets the equipment repair identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the equipment identifier</summary>
        public long EquipmentId { get; set; }

        /// <summary>Gets or sets the company idenfitifer</summary>
        public int CompanyId { get; set; }

        /// <summary>Gets or sets the type of repair</summary>
        public int RepairType { get; set; }

        /// <summary>Gets or sets the date of repair</summary>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets the description of repair</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the tools used on repair</summary>
        public string Tools { get; set; }

        /// <summary>Gets or sets observations about repair</summary>
        public string Observations { get; set; }

        /// <summary>Gets or sets the cost of repair</summary>
        public decimal? Cost { get; set; }

        /// <summary>Gets or sets the provider of repair</summary>
        public Provider Provider { get; set; }

        /// <summary>Gets or sets the responsible of reapir</summary>
        public Employee Responsible { get; set; }

        /// <summary>Gets or sets a value indicating whether if repair is active</summary>
        public bool Active { get; set; }

        /// <summary>Gets or sets the user of last modification</summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets the date of last modification</summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>Gets a JSON structure of reapir data</summary>
        public string Json
        {
            get
            {
                var res = new StringBuilder("{");
                res.Append(GisoFramework.Tools.JsonPair("Id", this.Id)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("EquipmentId", this.EquipmentId)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("CompanyId", this.CompanyId)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("RepairType", this.RepairType)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("Description", this.Description)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("Observations", this.Observations)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("Tools", this.Tools)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("Date", this.Date)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("Cost", this.Cost)).Append(", ");
                res.Append(GisoFramework.Tools.JsonPair("Active", this.Active)).Append(", ");

                if (this.Provider == null)
                {
                    res.Append("\"Provider\": null,");
                }
                else
                {
                    res.Append("\"Provider\": ").Append(this.Provider.JsonKeyValue).Append(",");
                }

                if (this.Responsible == null)
                {
                    res.Append("\"Responsible\": null}");
                }
                else
                {
                    res.Append("\"Responsible\": ").Append(this.Responsible.JsonKeyValue).Append("}");
                }

                res.Append(Environment.NewLine);
                return res.ToString();
            }
        }

        /// <summary>Gets a JSON structure of reapir of a equipment</summary>
        /// <param name="equipmentId">Equipment identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A JSON list of equipment repairs</returns>
        public static string JsonList(long equipmentId, int companyId)
        {
            var res = new StringBuilder("[");
            var maintenance = GetByEquipmentId(equipmentId, companyId);
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

        /// <summary>Get the repairs on a equipment</summary>
        /// <param name="equipmentId">Equipment identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>The list of a equipment repairs</returns>
        public static ReadOnlyCollection<EquipmentRepair> GetByEquipmentId(long equipmentId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentRepair_GetByEquipmentId
             *   @EquipmentId bigint,
             *   @CompanyId int */
            var res = new List<EquipmentRepair>();
            using (var cmd = new SqlCommand("EquipmentRepair_GetByEquipmentId"))
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
                                var equipmentRepair = new EquipmentRepair
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentRepairGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsEquipmentRepairGet.CompanyId),
                                    EquipmentId = rdr.GetInt64(ColumnsEquipmentRepairGet.EquipmentId),
                                    Description = rdr.GetString(ColumnsEquipmentRepairGet.Description),
                                    Tools = rdr.GetString(ColumnsEquipmentRepairGet.Tools),
                                    Observations = rdr.GetString(ColumnsEquipmentRepairGet.Observations),
                                    Date = rdr.GetDateTime(ColumnsEquipmentRepairGet.Date),
                                    Active = rdr.GetBoolean(ColumnsEquipmentRepairGet.Active),
                                    RepairType = rdr.GetInt32(ColumnsEquipmentRepairGet.RepairType),
                                    Provider = new Provider
                                    {
                                        Id = rdr.GetInt64(ColumnsEquipmentRepairGet.ProviderId),
                                        Description = rdr.GetString(ColumnsEquipmentRepairGet.ProviderDescription)
                                    },
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentRepairGet.ResponsibleEmployeeId),
                                        Name = rdr.GetString(ColumnsEquipmentRepairGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentRepairGet.ResponsibleLastName)
                                    },
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentRepairGet.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentRepairGet.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentRepairGet.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentRepairGet.Cost))
                                {
                                    equipmentRepair.Cost = rdr.GetDecimal(ColumnsEquipmentRepairGet.Cost);
                                }

                                res.Add(equipmentRepair);
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

            return new ReadOnlyCollection<EquipmentRepair>(res);
        }

        /// <summary>Get the differences between tow repairs</summary>
        /// <param name="item1">Repair from compare</param>
        /// <param name="item2">Repair to compare</param>
        /// <returns>A text with the differences</returns>
        public static string Differences(EquipmentRepair item1, EquipmentRepair item2)
        {
            if (item1 == null || item2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            if (item1.Description != item2.Description)
            {
                res.Append("Description:").Append(item2.Description).Append("; ");
            }

            if (item1.Observations != item2.Observations)
            {
                res.Append("Observations:").Append(item2.Observations).Append("; ");
            }

            if (item1.Date != item2.Date)
            {
                res.Append("Date:").Append(item2.Date).Append("; ");
            }

            if (item1.Tools != item2.Tools)
            {
                res.Append("Tools:").Append(item2.Tools).Append("; ");
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
                res.Append("Responsable:").Append(item2.Responsible.Id).Append("; ");
            }

            return res.ToString();
        }

        /// <summary>Delete a repair on database</summary>
        /// <param name="equipmentRepairId">Equipment repair identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int equipmentRepairId, int userId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentRepair_Delete
             *   @EquipmentRepairId bigint,
             *   @CompanyId int,
             *   @UserId int */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentRepair_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentRepairId", equipmentRepairId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Delete Id:{0} User:{1} Company:{2}", equipmentRepairId, userId, companyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Delete Id:{0} User:{1} Company:{2}", equipmentRepairId, userId, companyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Delete Id:{0} User:{1} Company:{2}", equipmentRepairId, userId, companyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Delete Id:{0} User:{1} Company:{2}", equipmentRepairId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Delete Id:{0} User:{1} Company:{2}", equipmentRepairId, userId, companyId));
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

        /// <summary>Insert a reapir into database</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE EquipmentRepair_Insert
             *   @EquipmentRepairId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @RepairType int,
             *   @Date datetime,
             *   @Description text,
             *   @Tools text,
             *   @Observations text,
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @UserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentRepair_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentRepairId"));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@RepairType", this.RepairType));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                        cmd.Parameters.Add(DataParameter.Input("@Tools", this.Tools));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations));
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
                        this.Id = Convert.ToInt64(cmd.Parameters["@EquipmentRepairId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        result.Success = true;
                        result.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EquipmentRepair::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "EquipmentRepairInsert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

        /// <summary>Updates a reapir on database</summary>
        /// <param name="differences">The differencies with the previus repair data</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(string differences, int userId)
        {
            /* CREATE PROCEDURE EquipmentRepair_Update
             *   @EquipmentRepairId bigint,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @RepairType int,
             *   @Date datetime,
             *   @Description text,
             *   @Tools text,
             *   @Observations text,
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @UserId int */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentRepair_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentRepairId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@RepairType", this.RepairType));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                        cmd.Parameters.Add(DataParameter.Input("@Tools", this.Tools));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations));
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
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentRepair::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
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