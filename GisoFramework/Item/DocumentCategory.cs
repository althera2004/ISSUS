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
    using System.Text;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;

    /// <summary>Implements document's category class</summary>
    public class DocumentCategory
    {
        /// <summary>Gets an empty document category</summary>
        public static DocumentCategory Empty
        {
            get
            {
                return new DocumentCategory { Id = -1, Description = string.Empty };
            }
        }
        
        /// <summary>Gets or sets the category identifier</summary>
        public int Id { get; set; }
        
        /// <summary>Gets or sets the description of category</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the company identifier</summary>
        public int CompanyId { get; set; }

        /// <summary>Gets or sets a value indicating whether is editable</summary>
        public bool Editable { get; set; }

        /// <summary>Gets or sets a value indicating whether is deletable</summary>
        public bool Deletable { get; set; }

        /// <summary>Gets a JSON key value structure og document category</summary>
        public string JsonKeyValue
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

        /// <summary>Gets a JSON structure og document category</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Description"":""{1}"", ""Editable"":""{2}"", ""Deletable"":""{3}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Editable ? "true" : "Kernel",
                    this.Deletable ? "true" : (this.Editable ? "InUse" : "Kernel"));
            }
        }

        /// <summary>Obtains a JSON array of document's categories</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>JSON array of documents</returns>
        public static string GetAllJson(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            var categories = ByCompany(companyId);
            foreach (var category in categories)
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

        /// <summary>Obtain all document categories of a company</summary>
        /// <param name="company">Company to search in</param>
        /// <returns>List of document categories</returns>
        public static ReadOnlyCollection<DocumentCategory> ByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<DocumentCategory>(new List<DocumentCategory>());
            }

            return ByCompany(company.Id);
        }

        /// <summary>Obtain all document categories of a company</summary>
        /// <param name="companyId">Compnay identififer</param>
        /// <returns>List of document categories</returns>
        public static ReadOnlyCollection<DocumentCategory> ByCompany(int companyId)
        {
            List<DocumentCategory> res = new List<DocumentCategory>();
            /* ALTER PROCEDURE Company_GetDocumentProcedencias
             *   @CompanyId int */
            using (var cmd = new SqlCommand("Company_GetDocumentCategories"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new DocumentCategory
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
            }

            return new ReadOnlyCollection<DocumentCategory>(res);
        }

        /// <summary>Delete a document category from data base</summary>
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

        /// <summary>Insert a document category into data base</summary>
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
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                    cmd.Parameters["@Description"].Value = Tools.LimitedText(this.Description, 50);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@DocumentCategoryId"].Value.ToString(), CultureInfo.InvariantCulture);
                    res.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));
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

        /// <summary>Update a document category in data base</summary>
        /// <returns>Result of action</returns>
        public ActionResult Update()
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE DocumentCategory_Update
             * @DocumentCategoryId int,
             * @CompanyId int,
             * @Description nvarchar(50) */
            using (var cmd = new SqlCommand("DocumentCategory_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DocumentCategoryId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
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
            }

            return res;
        }
    }
}