// --------------------------------
// <copyright file="EquipmentMaintenanceDefinition.cs" company="Sbrinna">
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

    /// <summary>Implements EquipmentMaintenanceDefinition class</summary>
    public class EquipmentMaintenanceDefinition : BaseItem
    {
        public long EquipmentId { get; set; }

        public int MaintenanceType { get; set; }

        public int Periodicity { get; set; }

        public string Accessories { get; set; }

        public decimal? Cost { get; set; }

        public DateTime? FirstDate { get; set; }

        public Provider Provider { get; set; }

        public Employee Responsible { get; set; }

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
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("MaintenanceType", this.MaintenanceType)).Append(",");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Periodicity", this.Periodicity)).Append(",");
                res.Append(Tools.JsonPair("Accessories", this.Accessories)).Append(",");
                res.Append(Tools.JsonPair("Cost", this.Cost)).Append(",");
                res.Append(Tools.JsonPair("Active", this.Active)).Append(",");

                if (this.FirstDate == null)
                {
                    res.Append("\"FirstDate\":null,");
                }
                else
                {
                    res.Append("\"FirstDate\":").AppendFormat(CultureInfo.InvariantCulture, "{0:yyyyMMdd}", this.FirstDate).Append(",");
                }

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
            var maintenance = ByCompany(equipmentId, companyId);
            bool first = true;
            foreach (EquipmentMaintenanceDefinition item in maintenance)
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

        /*public string Row(Dictionary<string, string> dictionary, bool grantToWrite)
        {
            string maintenanceType = this.MaintenanceType == 0 ? dictionary["Common_Internal"] : dictionary["Common_External"];
            string iconEdit = string.Empty;
            string iconDelete = string.Empty;
            string iconAct = string.Empty;

            if (grantToWrite)
            {
                iconEdit = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<span class=""btn btn-xs btn-info"" title=""{0}"" onclick=""EquipmentMaintenanceEdit({1});""><i class=""icon-edit bigger-120""></i></span>",
                    dictionary["Common_Edit"],
                    this.Id);
                iconDelete = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<span class=""btn btn-xs btn-danger"" title=""{0}"" onclick=""EquipmentMaintenanceDelete({1});""><i class=""icon-trash bigger-120""></i></span>",
                    dictionary["Common_Delete"],
                    this.Id);
                iconAct = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<span class=""btn btn-xs btn-success"" title=""{0}"" onclick=""EquipmentMaintenanceEdit({1});""><i class=""icon-star bigger-120""></i></span>",
                    dictionary["Item_EquipmentMaintenance_Button_Register"],
                    this.Id);
            }

            StringBuilder res = new StringBuilder("<tr>");
            res.Append("<td>").Append(this.Operation).Append("</td><td>");
            res.Append(maintenanceType).Append("</td><td align=\"right\">");
            res.Append(this.Periodicity).Append("</td><td>");
            res.Append(this.Accessories).Append("</td><td align=\"right\">");
            res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Cost)).Append("</td><td>");
            res.Append(iconEdit).Append("&nbsp;").Append(iconDelete).Append("&nbsp;").Append(iconAct).Append("</td></tr>");
            return res.ToString();
        }*/

        public static ReadOnlyCollection<EquipmentMaintenanceDefinition> ByCompany(long equipmentId, int companyId)
        {
            /*CREATE PROCEDURE EquipmentMaintenance_GetByEquipmentId
             *   @EquipmentId bigint,
             *   @CompanyId int */
            var res = new List<EquipmentMaintenanceDefinition>();
            using (var cmd = new SqlCommand("EquipmentMaintenance_GetByEquipmentId"))
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
                                var newEquipmentMaintenanceDefinition = new EquipmentMaintenanceDefinition
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentMaintenanceGetByEquipmentId.Id),
                                    CompanyId = rdr.GetInt32(ColumnsEquipmentMaintenanceGetByEquipmentId.CompanyId),
                                    EquipmentId = rdr.GetInt64(ColumnsEquipmentMaintenanceGetByEquipmentId.EquipmentId),
                                    Description = rdr.GetString(ColumnsEquipmentMaintenanceGetByEquipmentId.Operation),
                                    Accessories = rdr.GetString(ColumnsEquipmentMaintenanceGetByEquipmentId.Accessories),
                                    MaintenanceType = rdr.GetInt32(ColumnsEquipmentMaintenanceGetByEquipmentId.Type),
                                    Periodicity = rdr.GetInt32(ColumnsEquipmentMaintenanceGetByEquipmentId.Periodicity),
                                    Active = rdr.GetBoolean(ColumnsEquipmentMaintenanceGetByEquipmentId.Active),
                                    Provider = new Provider
                                    {
                                        Id = rdr.GetInt64(ColumnsEquipmentMaintenanceGetByEquipmentId.ProviderId),
                                        Description = rdr.GetString(ColumnsEquipmentMaintenanceGetByEquipmentId.ProviderDescription)
                                    },
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentMaintenanceGetByEquipmentId.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentMaintenanceGetByEquipmentId.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentMaintenanceGetByEquipmentId.ResponsibleLastName)
                                    },
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentMaintenanceGetByEquipmentId.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentMaintenanceGetByEquipmentId.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentMaintenanceGetByEquipmentId.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentMaintenanceGetByEquipmentId.FirstDate))
                                {
                                    newEquipmentMaintenanceDefinition.FirstDate = rdr.GetDateTime(ColumnsEquipmentMaintenanceGetByEquipmentId.FirstDate);
                                }

                                if (!rdr.IsDBNull(ColumnsEquipmentMaintenanceGetByEquipmentId.Cost))
                                {
                                    newEquipmentMaintenanceDefinition.Cost = rdr.GetDecimal(ColumnsEquipmentMaintenanceGetByEquipmentId.Cost);
                                }

                                res.Add(newEquipmentMaintenanceDefinition);
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

            return new ReadOnlyCollection<EquipmentMaintenanceDefinition>(res);
        }

        public static ActionResult Delete(int equipmentMaintenanceDefinitionId, int userId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentMaintenanceDefinition_Delete
             *   @EquipmentMaintenanceDefinitionId bigint,
             *   @CompanyId int,
             *   @UserId int*/
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentMaintenanceDefinition_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentMaintenanceDefinitionId", equipmentMaintenanceDefinitionId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceDefinition::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceDefinitionId, userId, companyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceDefinition::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceDefinitionId, userId, companyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceDefinition::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceDefinitionId, userId, companyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceDefinition::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceDefinitionId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentMaintenanceDefinition::Delete Id:{0} User:{1} Company:{2}", equipmentMaintenanceDefinitionId, userId, companyId));
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

        /*
        public override string Differences(BaseItem item)
        {
            if(item==null)
            {
                return string.Empty;
            }

            EquipmentMaintenanceDefinition item1 = item as EquipmentMaintenanceDefinition;
            StringBuilder res = new StringBuilder();
            if (item1.Description != this.Description)
            {
                res.Append("Operation:").Append(this.Description).Append("; ");
            }
            if (item1.MaintenanceType != this.MaintenanceType)
            {
                res.Append("MaintenanceType:").Append(this.MaintenanceType).Append("; ");
            }
            if (item1.Periodicity != this.Periodicity)
            {
                res.Append("Periodicity:").Append(this.Periodicity).Append("; ");
            }
            if (item1.Accessories != this.Accessories)
            {
                res.Append("Accessories:").Append(this.Accessories).Append("; ");
            }
            if (item1.Cost != this.Cost)
            {
                res.Append("Cost:").Append(this.Cost).Append("; ");
            }

            if (item1.Provider != null || this.Provider != null)
            {
                if (item1.Provider == null)
                {
                    res.Append("Provider:").Append(this.Provider.Id).Append("; ");
                }
                else if (this.Provider == null)
                {
                    res.Append("Provider:null; ");
                }
                else if (item1.Provider.Id != this.Provider.Id)
                {
                    res.Append("Provider:").Append(this.Provider.Id).Append("; ");
                }
            }

            if (item1.Responsible.Id != this.Responsible.Id)
            {
                res.Append("Responsible:").Append(this.Responsible.Id).Append("; ");
            }

            return res.ToString();
        }
*/

        public ActionResult Insert(int userId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name:{1}", this.Id, this.Description);
            /* CREATE PROCEDURE EquipmentMaintenance_Insert
             *   @EquipmentMaintenanceId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Operation nvarchar(50),
             *   @EquipmentMaintenanceType int,
             *   @Periodicity int,
             *   @Accessories nvarchar(50),
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @UserId int	*/
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentMaintenance_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentMaintenanceId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description, 200));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentMaintenanceType", this.MaintenanceType));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@Accessories", this.Accessories ?? string.Empty));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@FirstDate", this.FirstDate));

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
                        this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentMaintenanceId"].Value, CultureInfo.InvariantCulture);
                        result.Success = true;
                        result.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EqupimentScaleDivision::Insert", source);
                    }
                    catch (FormatException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EqupimentScaleDivision::Insert", source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EqupimentScaleDivision::Insert", source);
                    }
                    catch (ArgumentException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EqupimentScaleDivision::Insert", source);
                    }
                    catch (NullReferenceException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EqupimentScaleDivision::Insert", source);
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
            string source = string.Format(CultureInfo.InvariantCulture, @"EquipmentMaintenanceDefinition::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId);
            /* CREATE PROCEDURE EquipmentMaintnanceDefinition_Update
             *   @EquipmentMaintenanceDefinitionId bigint,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Operation nvarchar(50),
             *   @MaintenanceType int,
             *   @Periodicity int,
             *   @Accessories nvarchar(50),
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @Differences text,
             *   @UserId int */
            var result = new ActionResult { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentMaintnanceDefinition_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentMaintenanceDefinitionId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description, 200));
                        cmd.Parameters.Add(DataParameter.Input("@MaintenanceType", this.MaintenanceType));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@Accessories", this.Accessories));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@FirstDate", this.FirstDate));

                        if (this.Responsible == null)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@ProviderId"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id));
                        }

                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Differences", differences));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, source);
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