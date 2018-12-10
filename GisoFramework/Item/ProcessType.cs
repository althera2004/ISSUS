// --------------------------------
// <copyright file="ProcessType.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.Item.Binding;

    /// <summary>Implements ProcessType class</summary>
    public class ProcessType : BaseItem
    {
        /// <summary>Gets link of proceess type</summary>
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
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0}, ""Description"": ""{1}"", ""Active"": {2}, ""Deletable"": {3}}}", this.Id, Tools.JsonCompliant(this.Description), this.Active ? "true" : "false", this.CanBeDeleted ? "true" : "false");
            }
        }

        /// <summary>Gets a JSON list of process type of company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="dictionary">Dicitionary to set labels</param>
        /// <returns>JSON list of process type of company</returns>
        public static string GetByCompanyJsonList(int companyId, Dictionary<string, string> dictionary)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (ProcessType processType in ObtainByCompany(companyId, dictionary))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(processType.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Obtain all process type of a company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <returns>A list of type of process</returns>
        public static ReadOnlyCollection<ProcessType> ObtainByCompany(int companyId, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            List<ProcessType> res = new List<ProcessType>();

            // Job position fixed by application kernel
            res.Add(new ProcessType()
            {
                Id = 1,
                Description = dictionary["Item_ProcessType_Name_Principal"],
                Active = true,
                CompanyId = companyId,
                CanBeDeleted = false
            });

            res.Add(new ProcessType()
            {
                Id = 2,
                Description = dictionary["Item_ProcessType_Name_Support"],
                Active = true,
                CompanyId = companyId,
                CanBeDeleted = false
            });

            res.Add(new ProcessType()
            {
                Id = 3,
                Description = dictionary["Item_ProcessType_Name_Estrategic"],
                Active = true,
                CompanyId = companyId,
                CanBeDeleted = false
            });

            /* CREATE PROCEDURE ProcessType_GetByCompany
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("ProcessType_GetByCompany"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new ProcessType()
                        {
                            Id = rdr.GetInt32(ColumnsProcessTypeGetByCompany.Id),
                            Description = rdr.GetString(ColumnsProcessTypeGetByCompany.Description),
                            Active = rdr.GetBoolean(ColumnsProcessTypeGetByCompany.Active),
                            CompanyId = companyId,
                            CanBeDeleted = rdr.IsDBNull(ColumnsProcessTypeGetByCompany.CanBeDeleted)
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

            return new ReadOnlyCollection<ProcessType>(res);
        }

        /// <summary>Insert a process into data base</summary>
        /// <param name="userId">Identifier of users that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ProcessType_Insert
             * @ProcessTypeId int out,
             * @CompanyId int,
             * @Description nvarchar(50),
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("ProcessType_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@ProcessTypeId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@ProcessTypeId"].Value = DBNull.Value;
                    cmd.Parameters["@ProcessTypeId"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.Description, 50);
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@ProcessTypeId"].Value.ToString(), CultureInfo.GetCultureInfo("en-us"));
                    res.SetSuccess(this.Id);
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

        /// <summary>Update process data in data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ProcessType_Update
             * @ProcessTypeId int,
             * @CompanyId int,
             * @Description nvarchar(50),
             * @Active bit,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("ProcessType_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@ProcessTypeId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Active", SqlDbType.Bit);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@ProcessTypeId"].Value = this.Id;
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.Description, 50);
                    cmd.Parameters["@Active"].Value = this.Active;
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
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

        /// <summary>Deactive a process</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Deactive(int userId)
        {
            this.Active = false;
            return this.Update(userId);
        }
    }
}
