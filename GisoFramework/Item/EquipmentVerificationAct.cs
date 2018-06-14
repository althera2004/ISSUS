// --------------------------------
// <copyright file="equipmentverificationact.cs" company="Sbrinna">
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

    /// <summary>Implements EquipmentVerificationAct class</summary>
    public class EquipmentVerificationAct : BaseItem
    {
        /// <summary>Gets a empty instance of equipment verification</summary>
        public static EquipmentVerificationAct Empty
        {
            get
            {
                return new EquipmentVerificationAct
                {
                    Id = -1,
                    EquipmentVerificationType = -1,
                    EquipmentId = -1,
                    Responsible = Employee.Empty,
                    Provider = Provider.Empty
                };
            }
        }

        /// <summary>Gets or sets the type of verification</summary>
        public int EquipmentVerificationType { get; set; }

        /// <summary>Gets or sets equipment identifier</summary>
        public long EquipmentId { get; set; }

        /// <summary>Gets or sets date of verification</summary>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets expiration date</summary>
        public DateTime Expiration { get; set; }

        /// <summary>Gets or sets result of verification</summary>
        public decimal Result { get; set; }

        /// <summary>Gets or sets maximum value of result</summary>
        public decimal MaxResult { get; set; }

        /// <summary>Gets or sets cost of verification</summary>
        public decimal? Cost { get; set; }

        /// <summary>Gets or sets responsible of verification</summary>
        public Employee Responsible { get; set; }

        /// <summary>Gets or sets provider of verification if external</summary>
        public Provider Provider { get; set; }

        /// <summary>Gets link to verification profile</summary>
        public override string Link
        {
            get
            {
                return string.Empty;
            }
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
                var res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",");
                res.Append(Tools.JsonPair("EquipmentId", this.EquipmentId)).Append(",");
                res.Append(Tools.JsonPair("EquipmentVerificationType", this.EquipmentVerificationType)).Append(",");
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

        /// <summary>Creates JSON list of verification of equipment</summary>
        /// <param name="equipmentId">Equipment identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>JSON list of verification of equipment</returns>
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

        /// <summary>Gets verifications of an equipment from database</summary>
        /// <param name="equipmentId">Equipment identififer</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of verifications of an equipment from database</returns>
        public static ReadOnlyCollection<EquipmentVerificationAct> GetByCompany(long equipmentId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentVerificationAct_GetByEquipmentId
             *   @EquipmentId bigint,
             *   @CompanyId int */
            var res = new List<EquipmentVerificationAct>();
            using (var cmd = new SqlCommand("EquipmentVerificationAct_GetByEquipmentId"))
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
                                EquipmentVerificationAct newEquipmentVerificationAct = new EquipmentVerificationAct()
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentVerificationActGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsEquipmentVerificationActGet.CompanyId),
                                    EquipmentId = rdr.GetInt64(ColumnsEquipmentVerificationActGet.EquipmentId),
                                    Result = rdr.GetDecimal(ColumnsEquipmentVerificationActGet.Result),
                                    MaxResult = rdr.GetDecimal(ColumnsEquipmentVerificationActGet.MaxResult),
                                    Date = rdr.GetDateTime(ColumnsEquipmentVerificationActGet.Date),
                                    Expiration = rdr.GetDateTime(ColumnsEquipmentVerificationActGet.Expiration),
                                    EquipmentVerificationType = rdr.GetInt32(ColumnsEquipmentVerificationActGet.EquipmentVerificationType),
                                    Active = rdr.GetBoolean(ColumnsEquipmentVerificationActGet.Active),
                                    Provider = new Provider()
                                    {
                                        Id = rdr.GetInt64(ColumnsEquipmentVerificationActGet.ProviderId),
                                        Description = rdr.GetString(ColumnsEquipmentVerificationActGet.ProviderDescription)
                                    },
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentVerificationActGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentVerificationActGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentVerificationActGet.ResponsibleLastName)
                                    },
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentVerificationActGet.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentVerificationActGet.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentVerificationActGet.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentVerificationActGet.Cost))
                                {
                                    newEquipmentVerificationAct.Cost = rdr.GetDecimal(ColumnsEquipmentVerificationActGet.Cost);
                                }

                                res.Add(newEquipmentVerificationAct);
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

            return new ReadOnlyCollection<EquipmentVerificationAct>(res);
        }

        /// <summary>Gets diferrences between two verifications</summary>
        /// <param name="item1">Verification base</param>
        /// <param name="item2">Verification to caompare</param>
        /// <returns>String with differences</returns>
        public static string Differences(EquipmentVerificationAct item1, EquipmentVerificationAct item2)
        {
            if (item1 == null || item2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            if (item1.Result != item2.Result)
            {
                res.Append("Result:").Append(item2.Result).Append("; ");
            }

            if (item1.MaxResult != item2.MaxResult)
            {
                res.Append("MaxResult:").Append(item2.MaxResult).Append("; ");
            }

            if (item1.Date != item2.Date)
            {
                res.Append("Date:").Append(item2.Date).Append("; ");
            }

            if (item1.Cost != item2.Cost)
            {
                res.Append("Cost:").Append(item2.Cost).Append("; ");
            }

            if (item1.Responsible.Id != item2.Responsible.Id)
            {
                res.Append("Responsible:").Append(item2.Responsible.Id).Append("; ");
            }

            return res.ToString();
        }

        /// <summary>Delete a verification on database</summary>
        /// <param name="equipmentVerificationActId">Verification identifier</param>
        /// <param name="userId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int equipmentVerificationActId, int userId, int companyId)
        {
            /* CREATE PROCEDURE EquipmentVerificationAct_Delete
             *   @EquipmentVerificationActId bigint output,
             *   @UserId int  */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentVerificationAct_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentVerificationActId", equipmentVerificationActId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Delete Id:{0} User:{1} Company:{2}", equipmentVerificationActId, userId, companyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Delete Id:{0} User:{1} Company:{2}", equipmentVerificationActId, userId, companyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Delete Id:{0} User:{1} Company:{2}", equipmentVerificationActId, userId, companyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Delete Id:{0} User:{1} Company:{2}", equipmentVerificationActId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Delete Id:{0} User:{1} Company:{2}", equipmentVerificationActId, userId, companyId));
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

        /// <summary>Insert verification into database</summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE EquipmentVerificationAct_Insert
             *   @EquipmentVerificationActId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @EquipmentVerificationType int,
             *   @Operation nvarchar(50),
             *   @Date datetime,
             *   @Vto datetime,
             *   @Result numeric(18,3),
             *   @MaxResult numeric(18,3),
             *   @Cost numeric(18,3),
             *   @ResponsableId int,
             *   @UserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentVerificationAct_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentVerificationActId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentVerificationType", this.EquipmentVerificationType));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                        cmd.Parameters.Add(DataParameter.Input("@MaxResult", this.MaxResult));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        if (this.Provider == null || this.Provider.Id == 0 || this.EquipmentVerificationType == 0)
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
                        this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentVerificationActId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        result.Success = true;
                        result.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "EquipmentVerificationAct::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Result));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "EquipmentVerificationAct", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Result));
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

        /// <summary>Update verification on database</summary>
        /// <param name="differences">Differences from previous data</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(string differences, int userId)
        {
            /* CREATE PROCEDURE EquipmentVerificationAct_Update
             *   @EquipmentVerificationActId bigint,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @EquipmentVerificationType int,
             *   @Date datetime,
             *   @Vto datetime,
             *   @Result numeric(18,3),
             *   @MaxResult numeric(18,3),
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @UserId int  */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("EquipmentVerificationAct_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentVerificationActId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentVerificationType", this.EquipmentVerificationType));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                        cmd.Parameters.Add(DataParameter.Input("@MaxResult", this.MaxResult));

                        if (this.Provider == null || this.Provider.Id == 0 || this.EquipmentVerificationType == 0)
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
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId));
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