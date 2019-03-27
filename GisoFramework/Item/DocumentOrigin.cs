// --------------------------------
// <copyright file="DocumentOrigin.cs" company="OpenFramework">
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
    using GisoFramework.Activity;

    /// <summary>Implements DocumentOrigin class</summary>
    public class DocumentOrigin
    {
        /// <summary>Gets an empty origin</summary>
        public static DocumentOrigin Empty
        {
            get
            {
                return new DocumentOrigin()
                {
                    Id = -1,
                    Description = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the origin identifier</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the description of origin</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the compnay identifier of origin</summary>
        public int CompanyId { get; set; }

        /// <summary>Gets or sets a value indicating whether is editable</summary>
        public bool Editable { get; set; }

        /// <summary>Gets or sets a value indicating whether is deletable</summary>
        public bool Deletable { get; set; }

        /// <summary>Gets a JSON structure of origin</summary>
        public string Json
        {
            get
            {
                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""Editable"":{2},
                        ""Deletable"":{3}
                    }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Editable ? "true" : "false",
                    this.Deletable ? "true" : "false");
            }
        }

        /// <summary>Obtain the document origins of the comany</summary>
        /// <param name="company">Compnay to serach in</param>
        /// <returns>List of document origin</returns>
        public static ReadOnlyCollection<DocumentOrigin> ByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<DocumentOrigin>(new List<DocumentOrigin>());
            }

            return ByCompany(company.Id);
        }

        /// <summary>Obtain the document origins of the comany</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of document origin</returns>
        public static ReadOnlyCollection<DocumentOrigin> ByCompany(int companyId)
        {
            List<DocumentOrigin> res = new List<DocumentOrigin>();
            /* 
             * ALTER PROCEDURE Company_GetDocumentProcedencias
             * @CompanyId int
             */

            using (var cmd = new SqlCommand("Company_GetDocumentProcedencias"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters["@CompanyId"].Value = companyId;

                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {

                    while (rdr.Read())
                    {
                        res.Add(new DocumentOrigin()
                        {
                            Id = rdr.GetInt32(0),
                            Description = rdr.GetString(1),
                            Editable = rdr.GetBoolean(2),
                            CompanyId = companyId,
                            Deletable = rdr.GetBoolean(3) && rdr.GetBoolean(2)
                        });
                    }
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentOrigin::GetByCompany({0})", companyId));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentOrigin::GetByCompany({0})", companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentOrigin::GetByCompany({0})", companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentOrigin::GetByCompany({0})", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentOrigin::GetByCompany({0})", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<DocumentOrigin>(res);
        }

        /// <summary>Delete an origin from data base</summary>
        /// <param name="id">Origin identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Reault of action</returns>
        public static ActionResult Delete(int id, int companyId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE DocumentProcedencia_Delete
             * @DocumentProcedenciaId int,
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("DocumentProcedencia_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentProcedenciaId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@DocumentProcedenciaId"].Value = id;
                    cmd.Parameters["@CompanyId"].Value = companyId;
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

        /// <summary>Insert an origin into data base</summary>
        /// <returns>Result of action</returns>
        public ActionResult Insert()
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[DocumentProcedencia_Insert]
             * @DocumentProcedenciaId int out,
             * @CompanyId int,
             * @Description nvarchar(50) */
            using (SqlCommand cmd = new SqlCommand("DocumentProcedencia_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentProcedenciaId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                    cmd.Parameters["@DocumentProcedenciaId"].Value = DBNull.Value;
                    cmd.Parameters["@DocumentProcedenciaId"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.Description, 50);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@DocumentProcedenciaId"].Value.ToString(), CultureInfo.GetCultureInfo("en-us"));
                    res.SetSuccess(this.Id.ToString(CultureInfo.GetCultureInfo("en-us")));
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

        /// <summary>Update origin in data base</summary>
        /// <returns>Result of action</returns>
        public ActionResult Update()
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE DocumentProcedencia_Update
             * @DocumentProcedenciaId int,
             * @CompanyId int,
             * @Description nvarchar(50) */
            using (var cmd = new SqlCommand("DocumentProcedencia_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentProcedenciaId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                    cmd.Parameters["@DocumentProcedenciaId"].Value = this.Id;
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.Description, 50);
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
    }
}