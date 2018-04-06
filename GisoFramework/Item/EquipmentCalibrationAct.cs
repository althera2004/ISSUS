// --------------------------------
// <copyright file="EquipmentCalibrationAct.cs" company="Sbrinna">
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

    /// <summary>Implements EquipmentCalibrationAct class</summary>
    public class EquipmentCalibrationAct : BaseItem
    {
        public static EquipmentCalibrationAct Empty
        {
            get
            {
                return new EquipmentCalibrationAct
                {
                    Id = -1,
                    EquipmentCalibrationType = -1,
                    EquipmentId = -1,
                    Responsible = Employee.Empty,
                    Provider = Provider.Empty
                };
            }
        }

        public int EquipmentCalibrationType { get; set; }

        public long EquipmentId { get; set; }

        public DateTime Date { get; set; }

        public DateTime Expiration { get; set; }

        public decimal Result { get; set; }

        public decimal MaxResult { get; set; }

        public decimal? Cost { get; set; }

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
                res.Append(Tools.JsonPair("EquipmentCalibrationType", this.EquipmentCalibrationType)).Append(",");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("Date", this.Date)).Append(",");
                res.Append(Tools.JsonPair("Vto", this.Expiration)).Append(",");
                res.Append(Tools.JsonPair("Result", this.Result, 6)).Append(",");
                res.Append(Tools.JsonPair("MaxResult", this.MaxResult, 6)).Append(",");
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

        public static ReadOnlyCollection<EquipmentCalibrationAct> GetByCompany(long equipmentId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentCalibrationAct_GetByEquipmentId
             *   @EquipmentId bigint,
             *   @CompanyId int */
            var res = new List<EquipmentCalibrationAct>();
            using (var cmd = new SqlCommand("EquipmentCalibrationAct_GetByEquipmentId"))
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
                                EquipmentCalibrationAct newEquipmentCalibrationAct = new EquipmentCalibrationAct()
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentCalibrationActGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsEquipmentCalibrationActGet.CompanyId),
                                    EquipmentId = rdr.GetInt64(ColumnsEquipmentCalibrationActGet.EquipmentId),
                                    Result = rdr.GetDecimal(ColumnsEquipmentCalibrationActGet.Result),
                                    MaxResult = rdr.GetDecimal(ColumnsEquipmentCalibrationActGet.MaxResult),
                                    Date = rdr.GetDateTime(ColumnsEquipmentCalibrationActGet.Date),
                                    Expiration = rdr.GetDateTime(5),
                                    EquipmentCalibrationType = rdr.GetInt32(ColumnsEquipmentCalibrationActGet.EquipmentCalibrationType),
                                    Active = rdr.GetBoolean(ColumnsEquipmentCalibrationActGet.Active),
                                    Provider = new Provider()
                                    {
                                        Id = rdr.GetInt64(ColumnsEquipmentCalibrationActGet.ProviderId),
                                        Description = rdr.GetString(ColumnsEquipmentCalibrationActGet.ProviderDescription)
                                    },
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentCalibrationActGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentCalibrationActGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentCalibrationActGet.ResponsibleLastName)
                                    },
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentCalibrationActGet.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentCalibrationActGet.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentCalibrationActGet.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentCalibrationActGet.Cost))
                                {
                                    newEquipmentCalibrationAct.Cost = rdr.GetDecimal(ColumnsEquipmentCalibrationActGet.Cost);
                                }

                                res.Add(newEquipmentCalibrationAct);
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

            return new ReadOnlyCollection<EquipmentCalibrationAct>(res);
        }

/*
        public override string Differences(BaseItem item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            EquipmentCalibrationAct item1 = item as EquipmentCalibrationAct;
            StringBuilder res = new StringBuilder();
            if (item1.Result != this.Result)
            {
                res.Append("Result:").Append(this.Result).Append("; ");
            }
            if (item1.MaxResult != this.MaxResult)
            {
                res.Append("MaxResult:").Append(this.MaxResult).Append("; ");
            }
            if (item1.Date != this.Date)
            {
                res.Append("Date:").Append(this.Date).Append("; ");
            }
            if (item1.Vto != this.Vto)
            {
                res.Append("Vto:").Append(this.Vto).Append("; ");
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

            if (item1.Responsable.Id != this.Responsable.Id)
            {
                res.Append("Responsible:").Append(this.Responsible.Id).Append("; ");
            }

            return res.ToString();
        }
        */
        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE EquipmentCalibrationAct_Insert
             *   @EquipmentCalibrationActId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @EquipmentCalibrationType int,
             *   @Operation nvarchar(50),
             *   @Date datetime,
             *   @Vto datetime,
             *   @Result numeric(18,3),
             *   @MaxResult numeric(18,3),
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @UserId int */
            ActionResult result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentCalibrationAct_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentCalibrationActId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentCalibrationType", this.EquipmentCalibrationType));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                        cmd.Parameters.Add(DataParameter.Input("@MaxResult", this.MaxResult));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        if (this.Provider == null || this.Provider.Id == 0 || this.EquipmentCalibrationType == 0)
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
                        this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentCalibrationActId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        result.Success = true;
                        result.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EquipmentCalibrationAct::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Result));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "EquipmentCalibrationAct", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Result));
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

        public static ActionResult Delete(int equipmentCalibrationActId, int userId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentCalibrationAct_Delete
             *   @EquipmentCalibrationActId bigint output,
             *   @UserId int  */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentCalibrationAct_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentCalibrationActId", equipmentCalibrationActId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Delete Id:{0} User:{1} Company:{2}", equipmentCalibrationActId, userId, companyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Delete Id:{0} User:{1} Company:{2}", equipmentCalibrationActId, userId, companyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Delete Id:{0} User:{1} Company:{2}", equipmentCalibrationActId, userId, companyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Delete Id:{0} User:{1} Company:{2}", equipmentCalibrationActId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Delete Id:{0} User:{1} Company:{2}", equipmentCalibrationActId, userId, companyId));
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
            /* CREATE PROCEDURE EquipmentCalibrationAct_Update
             *   @EquipmentCalibrationActId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @EquipmentCalibrationType int,
             *   @Date datetime,
             *   @Vto datetime,
             *   @Result numeric(18,3),
             *   @MaxResult numeric(18,3),
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @UserId int  */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentCalibrationAct_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentCalibrationActId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentCalibrationType", this.EquipmentCalibrationType));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                        cmd.Parameters.Add(DataParameter.Input("@MaxResult", this.MaxResult));

                        if (this.Provider == null || this.Provider.Id == 0 || this.EquipmentCalibrationType == 0)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@ProviderId"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id));
                        }

                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
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