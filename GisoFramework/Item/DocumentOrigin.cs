// --------------------------------
// <copyright file="DocumentOrigin.cs" company="Sbrinna">
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
    using GisoFramework.Activity;

    /// <summary>
    /// Implements DocumentOrigin class
    /// </summary>
    public class DocumentOrigin
    {
        #region Fields
        /// <summary>
        /// Origin identifier
        /// </summary>
        private int id;

        /// <summary>
        /// Origin description
        /// </summary>
        private string description;

        /// <summary>
        /// Identifier of compnay of origin
        /// </summary>
        private int companyId;

        /// <summary>
        /// Indicates if origin is editable
        /// </summary>
        private bool editable;

        /// <summary>
        /// Indicates if origin is deletable
        /// </summary>
        private bool deletable;
        #endregion

        #region Properties
        /// <summary>
        /// Gets an empty origin
        /// </summary>
        public static DocumentOrigin Empty
        {
            get
            {
                return new DocumentOrigin() { id = -1, description = string.Empty };
            }
        }

        /// <summary>
        /// Gets or sets the origin identifier
        /// </summary>
        public int Id
        {
            get 
            { 
                return this.id; 
            }

            set 
            { 
                this.id = value; 
            }
        }

        /// <summary>
        /// Gets or sets the description of origin
        /// </summary>
        public string Description
        {
            get 
            { 
                return this.description;
            }

            set 
            { 
                this.description = value; 
            }
        }

        /// <summary>
        /// Gets or sets the compnay identifier of origin
        /// </summary>
        public int CompanyId
        {
            get 
            { 
                return this.companyId;
            }

            set 
            {
                this.companyId = value; 
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether is editable
        /// </summary>
        public bool Editable
        {
            get 
            {
                return this.editable; 
            }

            set 
            {
                this.editable = value; 
            }
        }

        /// <summary>
        /// Gets a JSON structure of origin
        /// </summary>
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
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.id,
                    Tools.JsonCompliant(this.description),
                    this.editable ? "true" : "false",
                    this.deletable ? "true" : "false");
            }
        }
        #endregion

        /// <summary>
        /// Obtain the document origins of the comany
        /// </summary>
        /// <param name="company">Compnay to serach in</param>
        /// <returns>List of document origin</returns>
        public static ReadOnlyCollection<DocumentOrigin> GetByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<DocumentOrigin>(new List<DocumentOrigin>());
            }

            return GetByCompany(company.Id);
        }

        /// <summary>
        /// Obtain the document origins of the comany
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of document origin</returns>
        public static ReadOnlyCollection<DocumentOrigin> GetByCompany(int companyId)
        {
            List<DocumentOrigin> res = new List<DocumentOrigin>();
            /* 
             * ALTER PROCEDURE Company_GetDocumentProcedencias
             * @CompanyId int
             */

            using (SqlCommand cmd = new SqlCommand("Company_GetDocumentProcedencias"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters["@CompanyId"].Value = companyId;

                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new DocumentOrigin()
                        {
                            id = rdr.GetInt32(0),
                            description = rdr.GetString(1),
                            editable = rdr.GetBoolean(2),
                            companyId = companyId,
                            deletable = rdr.GetBoolean(3) && rdr.GetBoolean(2)
                        });
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

        /// <summary>
        /// Delete an origin from data base
        /// </summary>
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

        /// <summary>
        /// Insert an origin into data base
        /// </summary>
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
                    cmd.Parameters["@CompanyId"].Value = this.companyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.description, 50);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.id = Convert.ToInt32(cmd.Parameters["@DocumentProcedenciaId"].Value.ToString(), CultureInfo.GetCultureInfo("en-us"));
                    res.SetSuccess(this.id.ToString(CultureInfo.GetCultureInfo("en-us")));
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

        /// <summary>
        /// Update origin in data base
        /// </summary>
        /// <returns>Result of action</returns>
        public ActionResult Update()
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE DocumentProcedencia_Update
             * @DocumentProcedenciaId int,
             * @CompanyId int,
             * @Description nvarchar(50) */
            using (SqlCommand cmd = new SqlCommand("DocumentProcedencia_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentProcedenciaId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                    cmd.Parameters["@DocumentProcedenciaId"].Value = this.id;
                    cmd.Parameters["@CompanyId"].Value = this.companyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.description, 50);
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
