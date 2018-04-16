// --------------------------------
// <copyright file="DocumentAttach.cs" company="Sbrinna">
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
    using System.IO;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements documents attach class</summary>
    public class DocumentAttach : BaseItem
    {
        /// <summary>Gets an empty instance of attachment</summary>
        public static DocumentAttach Empty
        {
            get
            {
                return new DocumentAttach
                {
                    Id = -1,
                    Description = string.Empty,
                    Extension = string.Empty,
                    Version = -1,
                    CompanyId = -1,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        /// <summary>Gets or sets document identifier</summary>
        public long DocumentId { get; set; }

        /// <summary>Gets or sets attachment extension</summary>
        public string Extension { get; set; }

        /// <summary>Gets or sets attachment version</summary>
        public int Version { get; set; }

        /// <summary>Gets or sets version size</summary>
        public int Size { get; set; }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Description"":""{1}"",""Active"":{2}}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Active ? "true" : "false");
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string pattern = @"{{""Id"":{0},
                        ""Description"":""{1}"",
                        ""Extension"":""{2}"",
                        ""Version"":{3},
                        ""DocumentId"":{4},
                        ""CompanyId"":{5},
                        ""ModifiedOn"": ""{6:dd/MM/yyyy}"",
                        ""Size"": {7},
                        ""Active"":{8}}}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Extension,
                    this.Version,
                    this.DocumentId,
                    this.CompanyId,
                    this.ModifiedOn,
                    this.Size,
                    this.Active ? "true" : "false");
            }
        }

        /// <summary>Gets the HTML code for a document link</summary>
        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>Gets the html code for row of document's version table</summary>
        public string TableRow
        {
            get
            {
                string pattern = @"
                                <tr>
                                    <td style=""width:80px;"">{0}</td>
                                    <td style=""width:80px;"">{1:dd/MM/yyyy}</td>
                                    <td>{2}</td>
                                    <td style=""width:150px;"">{3}</td>
                                </tr>";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Version,
                    this.ModifiedOn,
                    this.Description,
                    this.ModifiedBy.UserName);
            }
        }

        /// <summary>Creates a JSON list of dicument attachments</summary>
        /// <param name="attachs">List of attachments</param>
        /// <returns>JSON list of dicument attachments</returns>
        public static string JsonList(ReadOnlyCollection<DocumentAttach> attachs)
        {
            if(attachs == null)
            {
                return "[]";
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach (var attach in attachs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(attach.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<DocumentAttach> ByDocument(long documentId, int companyId)
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);
            var res = new List<DocumentAttach>();
            using (var cmd = new SqlCommand("DocumentAttach_GetByDoumentId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var attach = new DocumentAttach
                            {
                                Id = rdr.GetInt64(ColumnsDocumentAttachGet.Id),
                                DocumentId = rdr.GetInt64(ColumnsDocumentAttachGet.DocumentId),
                                Description = rdr.GetString(ColumnsDocumentAttachGet.Description),
                                Extension = rdr.GetString(ColumnsDocumentAttachGet.Extension),
                                Version = rdr.GetInt32(ColumnsDocumentAttachGet.Version),
                                CompanyId = rdr.GetInt32(ColumnsDocumentAttachGet.CompanyId),
                                CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsDocumentAttachGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsDocumentAttachGet.CreatedByName)
                                },
                                CreatedOn = rdr.GetDateTime(ColumnsDocumentAttachGet.CreatedOn),
                                ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsDocumentAttachGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsDocumentAttachGet.ModifiedByName)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsDocumentAttachGet.ModifiedOn),
                                Active = rdr.GetBoolean(ColumnsDocumentAttachGet.Active)
                            };

                            res.Add(attach);
                        }
                    }
                }
            }

            return new ReadOnlyCollection<DocumentAttach>(res);
        }

        public static DocumentAttach ById(long id, int companyId)
        {
            var res = DocumentAttach.Empty;
            using (var cmd = new SqlCommand("DocumentAttach_GetById"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", id));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            res.Id = rdr.GetInt64(ColumnsDocumentAttachGet.Id);
                            res.DocumentId = rdr.GetInt64(ColumnsDocumentAttachGet.DocumentId);
                            res.Description = rdr.GetString(ColumnsDocumentAttachGet.Description);
                            res.Extension = rdr.GetString(ColumnsDocumentAttachGet.Extension);
                            res.Version = rdr.GetInt32(ColumnsDocumentAttachGet.Version);
                            res.CompanyId = rdr.GetInt32(ColumnsDocumentAttachGet.CompanyId);
                            res.CreatedBy = new ApplicationUser
                            {
                                Id = rdr.GetInt32(ColumnsDocumentAttachGet.CreatedBy),
                                UserName = rdr.GetString(ColumnsDocumentAttachGet.CreatedByName)
                            };
                            res.CreatedOn = rdr.GetDateTime(ColumnsDocumentAttachGet.CreatedOn);
                            res.ModifiedBy = new ApplicationUser
                            {
                                Id = rdr.GetInt32(ColumnsDocumentAttachGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsDocumentAttachGet.ModifiedByName)
                            };
                            res.ModifiedOn = rdr.GetDateTime(ColumnsDocumentAttachGet.ModifiedOn);
                            res.Active = rdr.GetBoolean(ColumnsDocumentAttachGet.Active);
                        }
                    }
                }
            }

            return res;
        }

        public static ActionResult Activate(long id, int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[DocumentAttach_Active]
             *   @Id bigint,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("DocumentAttach_Active"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", id));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(id);
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
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

        public static ActionResult Inactivate(long id, int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[DocumentAttach_Inactivate]
             *   @Id bigint,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("DocumentAttach_Inactivate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", id));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(id);
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
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

        public static ActionResult Delete(long id, int companyId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[DocumentAttach_Inactivate]
             *   @Id bigint,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("DocumentAttach_Delete"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", id));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(id);
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
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

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[DocumentAttach_Insert]
             *   @Id bigint output,
             *   @DocumentId bigint,
             *   @CompanyId int,
             *   @Version int,
             *   @Description nvarchar(50),
             *   @Extension nvarchar(10),
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("DocumentAttach_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                cmd.Parameters.Add(DataParameter.Input("@DocumentId", this.DocumentId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                cmd.Parameters.Add(DataParameter.Input("@Extension", this.Extension, 10));
                cmd.Parameters.Add(DataParameter.Input("@Version", this.Version));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString());
                        res.SetSuccess(this.Json);
                    }
                    catch (IOException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        res.SetFail(ex);
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

        public ActionResult Update(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[DocumentAttach_Update]
             *   @Id bigint,
             *   @Description nvarchar(50),
             *   @Extension nvarchar(10),
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("DocumentAttach_Update"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                cmd.Parameters.Add(DataParameter.Input("@Extension", this.Extension, 10));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].ToString());
                        res.SetSuccess(this.Id);
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
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

        public ActionResult RestoreName(string name)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                "UploadFile.RestoreName(id:{0}, name: {1}",
                this.Id,
                name);
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("DocumentAttach_RestoreName"))
            {
                /* CREATE PROCEDURE DocumentAttach_RestoreName
                 *   @Id bigint
                 *   @Name nvarchar(50) */
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                cmd.Parameters.Add(DataParameter.Input("@Name", name));
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(this.Json);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }

                return res;
            }
        }
    }
}