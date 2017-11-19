// --------------------------------
// <copyright file="EquipmentScaleDivision.cs" company="Sbrinna">
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
    /// Implements EquipmentScaleDivision class
    /// </summary>
    public class EquipmentScaleDivision : BaseItem
    {
        public static EquipmentScaleDivision Empty
        {
            get
            {
                return new EquipmentScaleDivision()
                {
                    Id = 0
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
                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""Active"":{2},
                        ""Deletable"":{3}
                      }}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    this.Description,
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        /*public override string Differences(BaseItem item1)
        {
            EquipmentScaleDivision c1 = item1 as EquipmentScaleDivision;

            if (c1.Description != this.Description)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "Description{0}", this.Description);
            }

            return string.Empty;
        }*/

        public static string JsonList(string name, ReadOnlyCollection<EquipmentScaleDivision> list)
        {
            StringBuilder res = new StringBuilder("var ").Append(name).Append("=[");
            bool first = true;
            if (list != null)
            {
                foreach (EquipmentScaleDivision item in list)
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
            }

            res.Append("];");
            return res.ToString();
        }

        public static ReadOnlyCollection<EquipmentScaleDivision> GetByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<EquipmentScaleDivision>(new List<EquipmentScaleDivision>());
            }

            return GetByCompany(company.Id);
        }

        /// <summary>
        /// Delete an equipment scale division
        /// </summary>
        /// <param name="equipmentScaleDivisionId">Identifier of equipment scale division</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Restult of action</returns>
        public static ActionResult Delete(int equipmentScaleDivisionId, int userId, int companyId)
        {
            /*CREATE PROCEDURE EquipmentScaleDivision_Delete
             *   @EquipmentScaleDivisionId bigint,
             *   @CompanyId int,
             *   @UserId int*/
            ActionResult result = new ActionResult() { Success = false, MessageError = "No action" };
            using (SqlCommand cmd = new SqlCommand("EquipmentScaleDivision_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@EquipmentScaleDivisionId", equipmentScaleDivisionId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.SetSuccess();
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Delete Id:{0} User:{1} Company:{2}", equipmentScaleDivisionId, userId, companyId));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Delete Id:{0} User:{1} Company:{2}", equipmentScaleDivisionId, userId, companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Delete Id:{0} User:{1} Company:{2}", equipmentScaleDivisionId, userId, companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Delete Id:{0} User:{1} Company:{2}", equipmentScaleDivisionId, userId, companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Delete Id:{0} User:{1} Company:{2}", equipmentScaleDivisionId, userId, companyId));
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

        public static ReadOnlyCollection<EquipmentScaleDivision> GetByCompany(int companyId)
        {
            List<EquipmentScaleDivision> res = new List<EquipmentScaleDivision>();
            using (SqlCommand cmd = new SqlCommand("EquipmentScaleDivision_GetByCompany"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        bool deletable = true;
                        if (rdr.GetInt32(ColumnsEquipmentScaleDivisionGetByCompany.InEquipment) == 1)
                        {
                            deletable = false;
                        }

                        res.Add(new EquipmentScaleDivision()
                        {
                            Id = rdr.GetInt64(ColumnsEquipmentScaleDivisionGetByCompany.Id),
                            CompanyId = companyId,
                            Description = rdr.GetString(ColumnsEquipmentScaleDivisionGetByCompany.Description),
                            Active = rdr.GetBoolean(ColumnsEquipmentScaleDivisionGetByCompany.Active),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsEquipmentScaleDivisionGetByCompany.ModifiedByUserId),
                                UserName = rdr.GetString(ColumnsEquipmentScaleDivisionGetByCompany.ModifiedByUserName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsEquipmentScaleDivisionGetByCompany.ModifiedOn),
                            CanBeDeleted = deletable
                        });
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "EqupimentScaleDivision::GetByCompany", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0}", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "EqupimentScaleDivision::GetByCompany", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0}", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<EquipmentScaleDivision>(res);
        }

        /// <summary>
        /// Insert an equipment scale division
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Restult of action</returns>
        public ActionResult Insert(int userId)
        {
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("EquipmentScaleDivision_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentScaleDivisionId"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentScaleDivisionId"].Value, CultureInfo.GetCultureInfo("en-us"));
                    result.Success = true;
                    result.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "EqupimentScaleDivision::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "EqupimentScaleDivision::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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
        /// Update an equipment scale division
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Restult of action</returns>
        public ActionResult Update(int userId)
        {
            /*CREATE PROCEDURE EquipmentScaleDivision_Update
             *   @EquipmentScaleDivisionId bigint,
             *   @Description nvarchar(20),
             *   @CompanyId int,
             *   @UserId int  */
            ActionResult result = new ActionResult() { Success = false, MessageError = "No action" };
            using (SqlCommand cmd = new SqlCommand("EquipmentScaleDivision_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@EquipmentScaleDivisionId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.SetSuccess();
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentScaleDivision::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
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
    }
}
