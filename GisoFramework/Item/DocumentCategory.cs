// --------------------------------
// <copyright file="DocumentCategory.cs" company="Sbrinna">
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
    using GisoFramework.DataAccess;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DocumentCategory
    {
        #region Fields
        /// <summary>
        /// Document category identifier
        /// </summary>
        private int id;

        /// <summary>
        /// Document category description
        /// </summary>
        private string description;

        /// <summary>
        /// Document category's company identifier
        /// </summary>
        private int companyId;

        /// <summary>
        /// Indicates if is editable
        /// </summary>
        private bool editable;

        /// <summary>
        /// Indicates if is deletable
        /// </summary>
        private bool deletable;
        #endregion

        #region Properties
        /// <summary>
        /// Gets an empty document category
        /// </summary>
        public static DocumentCategory Empty
        {
            get
            {
                return new DocumentCategory() { id = -1, description = string.Empty };
            }
        }
        
        /// <summary>
        /// Gets or sets the category identifier
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
        /// Gets or sets the description of category
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
        /// Gets or sets the company identifier
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
        
        /// <summary>Obtains a JSON array of document's categories</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>JSON array of documents</returns>
        public static string GetAllJson(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            ReadOnlyCollection<DocumentCategory> categories = GetByCompany(companyId);
            foreach (DocumentCategory category in categories)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(category.JsonKeyValue);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Gets a JSON key value structure og document category</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0}, ""Description"":""{1}""}}",
                    this.id,
                    Tools.JsonCompliant(this.description));
            }
        }

        /// <summary>Gets a JSON structure og document category</summary>
        public string Json
        {
            get
            {
                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""Editable"":""{2}"",
                        ""Deletable"":""{3}""
                    }}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.id,
                    Tools.JsonCompliant(this.description),
                    this.editable ? "true" : "Kernel",
                    this.deletable ? "true" : (this.editable ? "InUse" : "Kernel"));
            }
        }
        #endregion

        /// <summary>
        /// Obtain all document categories of a company
        /// </summary>
        /// <param name="company">Company to search in</param>
        /// <returns>List of document categories</returns>
        public static ReadOnlyCollection<DocumentCategory> GetByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<DocumentCategory>(new List<DocumentCategory>());
            }

            return GetByCompany(company.Id);
        }

        /// <summary>
        /// Obtain all document categories of a company
        /// </summary>
        /// <param name="companyId">Compnay identififer</param>
        /// <returns>List of document categories</returns>
        public static ReadOnlyCollection<DocumentCategory> GetByCompany(int companyId)
        {
            List<DocumentCategory> res = new List<DocumentCategory>();
            /* ALTER PROCEDURE Company_GetDocumentProcedencias
             *   @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("Company_GetDocumentCategories"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new DocumentCategory()
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
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentCategory::GetByCompany({0})", companyId));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentCategory::GetByCompany({0})", companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentCategory::GetByCompany({0})", companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentCategory::GetByCompany({0})", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentCategory::GetByCompany({0})", companyId));
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentCategory::GetByCompany({0})", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<DocumentCategory>(res);
        }

        /// <summary>
        /// Delete a document category from data base
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int categoryId, int companyId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[DocumentCategory_Delete]
             * @DocumentCategoryId int,
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("DocumentCategory_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentCategoryId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@DocumentCategoryId"].Value = categoryId;
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
        /// Insert a document category into data base
        /// </summary>
        /// <returns>Result of action</returns>
        public ActionResult Insert()
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE DocumentCategory_Insert
             * @DocumentCategoryId int,
             * @CompanyId int,
             * @Description nvarchar(50) */
            using (SqlCommand cmd = new SqlCommand("DocumentCategory_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentCategoryId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                    cmd.Parameters["@DocumentCategoryId"].Value = DBNull.Value;
                    cmd.Parameters["@DocumentCategoryId"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@CompanyId"].Value = this.companyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.description, 50);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.id = Convert.ToInt32(cmd.Parameters["@DocumentCategoryId"].Value.ToString(), CultureInfo.GetCultureInfo("en-us"));
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
        /// Update a document category in data base
        /// </summary>
        /// <returns>Result of action</returns>
        public ActionResult Update()
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE DocumentCategory_Update
             * @DocumentCategoryId int,
             * @CompanyId int,
             * @Description nvarchar(50) */
            using (SqlCommand cmd = new SqlCommand("DocumentCategory_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentCategoryId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                    cmd.Parameters["@DocumentCategoryId"].Value = this.id;
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
