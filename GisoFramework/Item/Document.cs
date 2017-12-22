﻿// --------------------------------
// <copyright file="Document.cs" company="Sbrinna">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;
    using System.Text;

    /// <summary>
    /// Implements document class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1127:AvoidStringFormatUseStringInterpolation", Justification = "Reviewed.")]
    public class Document : BaseItem
    {
        /// <summary>
        /// Versions of document
        /// </summary>
        private List<DocumentVersion> versions;

        /// <summary>
        /// Initializes a new instance of the Document class
        /// </summary>
        public Document()
        {
            this.versions = new List<DocumentVersion>();
            this.Conservation = 0;
            this.ConservationType = 0;
            this.Source = true;
        }

        /// <summary>
        /// Gets or sets the code of document
        /// </summary>
        [DifferenciableAttribute]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the category of document
        /// </summary>
        [DifferenciableAttribute]
        public DocumentCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the origin of document
        /// </summary>
        [DifferenciableAttribute]
        public DocumentOrigin Origin { get; set; }

        /// <summary>
        /// Gets or sets the location of document
        /// </summary>
        [DifferenciableAttribute]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the record date of document
        /// </summary>
        [DifferenciableAttribute]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the disabling date of document
        /// </summary>
        [DifferenciableAttribute]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the source of document
        /// </summary>
        [DifferenciableAttribute]
        public bool Source { get; set; }

        /// <summary>
        /// Gets or sets the quantity of document conservation
        /// </summary>
        [DifferenciableAttribute]
        public int Conservation { get; set; }

        /// <summary>
        /// Gets or sets the time unity of document conservation
        /// </summary>
        [DifferenciableAttribute]
        public int ConservationType { get; set; }

        /// <summary>
        /// Gets the versions of document
        /// </summary>
        public ReadOnlyCollection<DocumentVersion> Versions
        {
            get
            {
                return new ReadOnlyCollection<DocumentVersion>(this.versions.OrderByDescending(x => x.Date).ToList());
            }
        }

        /// <summary>
        /// Gets the HTML code for a document link
        /// </summary>
        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "<a href=\"DocumentView.aspx?id={0}\" title=\"{2} {1}\">{1}</a>", this.Id, this.Description, ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return this.JsonSimple;
            }
        }

        /// <summary>
        /// Gets the JSON strucutre with basic data
        /// </summary>
        public string JsonSimple
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Code"":""{1}"",""Description"":""{2}""}}", this.Id, this.Code.Replace("\"", "\\\""), this.Description.Replace("\"", "\\\""));
            }
        }

        public static string GetAllJson(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            ReadOnlyCollection<Document> documents = GetByCompany(companyId);
            foreach(Document document in documents)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(document.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                DocumentVersion actual = this.LastVersion;
                string fechaBaja = string.Empty;
                if (this.EndDate.HasValue)
                {
                    fechaBaja = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", this.EndDate.Value);
                }

                string pattenr = @"
            {{
                ""Id"":{0},
                ""Company"":{{""Id"":{13}}},
                ""Code"":""{1}"",
                ""Description"":""{2}"",
                ""StartDate"":""{3}"",
                ""EndDate"":""{4}"",
                ""Category"":{{""Id"":{5}}},
                ""RevisionDate"":""{6}"",
                ""Origin"":{{""Id"":{7}}},
                ""ConservationType"":{8},
                ""Conservation"":{9},
                ""Source"":{10},
                ""Location"":""{11}"",
                ""LastVersion"":{13},
                ""Baja"": {14},
                ""Active"":{12}
            }}";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattenr,
                    this.Id,
                    this.Code.Replace("\"", "\\\""),
                    this.Description.Replace("\"", "\\\""),
                    string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyy}", this.StartDate),
                    fechaBaja,
                    this.Category.Id,
                    string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyy}", actual.Date),
                    this.Origin.Id,
                    this.ConservationType,
                    this.Conservation,
                    this.Source ? "true" : "false",
                    this.Location,
                    this.Active ? "true" : "false",
                    actual.Version,
                    this.EndDate.HasValue ? "true" : "false");
            }
        }

        /// <summary>
        /// Gets the last version
        /// </summary>
        public DocumentVersion LastVersion
        {
            get
            {
                DocumentVersion res = new DocumentVersion();
                foreach (DocumentVersion version in this.Versions)
                {
                    if (res.Date == null || version.Date >= res.Date)
                    {
                        res = version;
                    }
                }

                return res;
            }
        }

        /// <summary>
        /// Gets a value indicating whether if document is active
        /// </summary>
        public new bool Active
        {
            get
            {
                return !this.EndDate.HasValue;
            }
        }

        /// <summary>
        /// Gets the HTML code for a document in the table of inactive documents
        /// </summary>
        public string RowInactive
        {
            get
            {
                string pattern = @"
                <tr>
                    <td{5}>{0}</td>
                    <td>{1}</td>
                    <td>{2}</td>
                    <td>{3}</td>
                    <td>{4:dd/MM/yyyy}</td>
                </tr>";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Description,
                    this.Code,
                    this.LastVersion.Version,
                    this.ModifiedBy.Description,
                    this.ModifiedOn,
                    this.Active ? string.Empty : " style=\"text-decoration:line-through;\"");
            }
        }

        /// <summary>
        /// Filter the inactive documents from company's documents
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> LastModified(int companyId)
        {
            List<Document> res = new List<Document>();
            /* CREATE PROCEDURE Documents_GetInactive
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("Documents_LastModified"))
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
                        Document newDocument = new Document()
                        {
                            Id = rdr.GetInt64(ColumnsDocumentsLastModified.Id),
                            Code = rdr.GetString(ColumnsDocumentsLastModified.Code),
                            Description = rdr.GetString(ColumnsDocumentsLastModified.Description),
                            CompanyId = companyId,
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsDocumentsLastModified.ModifiedByUserId),
                                UserName = rdr.GetString(ColumnsDocumentsLastModified.ModifiedByUserName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsDocumentsLastModified.ModifiedOn)
                        };

                        newDocument.AddVersion(new DocumentVersion()
                        {
                            Company = new Company() { Id = companyId },
                            DocumentId = rdr.GetInt64(ColumnsDocumentsLastModified.Id),
                            Version = rdr.GetInt32(ColumnsDocumentsLastModified.ActualVersion)
                        });

                        res.Add(newDocument);
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

            return new ReadOnlyCollection<Document>(res);
        }

        /// <summary>
        /// Gets the inactive documents of company
        /// </summary>
        /// <param name="company">Company to search in</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> GetByCompanyInactive(Company company)
        {
            List<Document> res = new List<Document>();
            if (company != null)
            {

                using (SqlCommand cmd = new SqlCommand("Company_GetDocumentsInactive"))
                {
                    try
                    {
                        cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters["@CompanyId"].Value = company.Id;
                        Document newDocument = new Document();
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (newDocument.Id != rdr.GetInt64(ColumnsCompanyGetDocuments.DocumentId))
                                {
                                    newDocument = new Document()
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyGetDocuments.DocumentId),
                                        CompanyId = company.Id,
                                        Description = rdr.GetString(ColumnsCompanyGetDocuments.Description),
                                        Code = rdr.GetString(ColumnsCompanyGetDocuments.Code),
                                        Category = new DocumentCategory() { Description = rdr.GetString(ColumnsCompanyGetDocuments.CategoryName), Id = rdr.GetInt32(ColumnsCompanyGetDocuments.CategoryId) },
                                        Origin = new DocumentOrigin() { Description = rdr.GetString(ColumnsCompanyGetDocuments.SourceName), Id = rdr.GetInt32(ColumnsCompanyGetDocuments.SourceId) },
                                        ModifiedOn = rdr.GetDateTime(ColumnsCompanyGetDocuments.ModifiedOn),
                                        ModifiedBy = new ApplicationUser()
                                        {
                                            Id = rdr.GetInt32(ColumnsCompanyGetDocuments.ModifiedById),
                                            UserName = rdr.GetString(ColumnsCompanyGetDocuments.ModifiedByUserName)
                                        }
                                    };

                                    res.Add(newDocument);
                                }

                                newDocument.AddVersion(new DocumentVersion()
                                {
                                    DocumentId = rdr.GetInt64(ColumnsCompanyGetDocuments.DocumentId),
                                    Date = rdr.GetDateTime(ColumnsCompanyGetDocuments.DocumentDateVersion),
                                    Company = company,
                                    Id = rdr.GetInt64(ColumnsCompanyGetDocuments.VersionId),
                                    State = DocumentVersion.IntegerToStatus(rdr.GetInt32(ColumnsCompanyGetDocuments.Status)),
                                    User = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsCompanyGetDocuments.UserCreate),
                                        UserName = rdr.GetString(ColumnsCompanyGetDocuments.ModifiedByUserName)
                                    },
                                    Version = rdr.GetInt32(ColumnsCompanyGetDocuments.Version),
                                    Reason = rdr.GetString(ColumnsCompanyGetDocuments.Reason),
                                    UserCreateName = rdr.GetString(10)
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetByCompanyInactive({0})", company.Id));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetByCompanyInactive({0})", company.Id));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetByCompanyInactive({0})", company.Id));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetByCompanyInactive({0})", company.Id));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetByCompanyInactive({0})", company.Id));
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

            return new ReadOnlyCollection<Document>(res);
        }

        /// <summary>
        /// Get all document of company from data base
        /// </summary>
        /// <param name="company">Company to search in</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> GetByCompany(Company company)
        {
            if(company == null)
            {
                return new ReadOnlyCollection<Document>(new List<Document>());
            }

            return GetByCompany(company.Id);
        }

        /// <summary>
        /// Get all document of company from data base
        /// </summary>
        /// <param name="companyID">Company's identifier to search in</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> GetByCompany(int companyId)
        {
            List<Document> res = new List<Document>();
            using (SqlCommand cmd = new SqlCommand("Company_GetDocuments"))
            {
                try
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    Document newDocument = new Document();
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (newDocument.Id != rdr.GetInt64(ColumnsCompanyGetDocuments.DocumentId))
                        {
                            newDocument = new Document()
                            {
                                Id = rdr.GetInt64(ColumnsCompanyGetDocuments.DocumentId),
                                CompanyId = companyId,
                                Description = rdr.GetString(ColumnsCompanyGetDocuments.Description),
                                Code = rdr.GetString(ColumnsCompanyGetDocuments.Code),
                                Category = new DocumentCategory() { Description = rdr.GetString(ColumnsCompanyGetDocuments.CategoryName), Id = rdr.GetInt32(ColumnsCompanyGetDocuments.CategoryId) },
                                Origin = new DocumentOrigin() { Description = rdr.GetString(ColumnsCompanyGetDocuments.SourceName), Id = rdr.GetInt32(ColumnsCompanyGetDocuments.SourceId) },
                                Location = rdr.GetString(ColumnsCompanyGetDocuments.Location)
                            };

                            if (!rdr.IsDBNull(ColumnsCompanyGetDocuments.EndDate))
                            {
                                newDocument.EndDate = rdr.GetDateTime(ColumnsCompanyGetDocuments.EndDate);
                            }

                            res.Add(newDocument);
                        }

                        newDocument.AddVersion(new DocumentVersion()
                        {
                            DocumentId = rdr.GetInt64(ColumnsCompanyGetDocuments.DocumentId),
                            Date = rdr.GetDateTime(ColumnsCompanyGetDocuments.DocumentDateVersion),
                            Company = new Company() { Id = companyId },
                            Id = rdr.GetInt64(ColumnsCompanyGetDocuments.VersionId),
                            State = DocumentVersion.IntegerToStatus(rdr.GetInt32(ColumnsCompanyGetDocuments.Status)),
                            User = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsCompanyGetDocuments.UserCreate)
                            },
                            Version = rdr.GetInt32(ColumnsCompanyGetDocuments.Version),
                            Reason = rdr.GetString(ColumnsCompanyGetDocuments.Reason),
                            UserCreateName = rdr.GetString(10)
                        });
                    }

                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), " GetByCompany({0})", companyId));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), " GetByCompany({0})", companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), " GetByCompany({0})", companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), " GetByCompany({0})", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), " GetByCompany({0})", companyId));
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), " GetByCompany({0})", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<Document>(res);
        }

        /// <summary>
        /// Get a document from data base
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="company">Company to search in</param>
        /// <returns>Document objet</returns>
        public static Document GetById(long documentId, Company company)
        {
            if (company == null)
            {
                return new Document();
            }

            return GetById(documentId, company.Id);
        }

        /// <summary>
        /// Get a document from data base
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Document objet</returns>
        public static Document GetById(long documentId, int companyId)
        {
            Document res = new Document();

            Company company = new Company(companyId);

            using (SqlCommand cmd = new SqlCommand("Document_GetById"))
            {
                try
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DocumentId", SqlDbType.BigInt);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@DocumentId"].Value = documentId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        bool first = true;
                        while (rdr.Read())
                        {
                            if (first)
                            {
                                first = false;
                                res.Id = documentId;
                                res.CompanyId = company.Id;
                                res.Description = rdr.GetString(ColumnsDocumentGetById.Description);
                                res.Category = new DocumentCategory()
                                {
                                    Id = rdr.GetInt32(ColumnsDocumentGetById.CategoryId),
                                    Description = rdr.GetString(ColumnsDocumentGetById.CategoryName),
                                    CompanyId = company.Id
                                };

                                res.Origin = new DocumentOrigin()
                                {
                                    Id = rdr.GetInt32(ColumnsDocumentGetById.SourceId),
                                    Description = rdr.GetString(ColumnsDocumentGetById.SourceName),
                                    CompanyId = company.Id
                                };

                                res.Code = rdr.GetString(ColumnsDocumentGetById.Code);
                                res.StartDate = rdr.GetDateTime(ColumnsDocumentGetById.StartDate);
                                if (!rdr.IsDBNull(ColumnsDocumentGetById.EndDate))
                                {
                                    res.EndDate = rdr.GetDateTime(ColumnsDocumentGetById.EndDate);
                                }

                                res.Conservation = rdr.GetInt32(ColumnsDocumentGetById.Conservation);
                                res.ConservationType = rdr.GetInt32(ColumnsDocumentGetById.ConservationType);
                                res.Source = rdr.GetInt32(ColumnsDocumentGetById.Origen) == 1;
                                res.Location = rdr.GetString(ColumnsDocumentGetById.Location);
                                res.ModifiedOn = rdr.GetDateTime(ColumnsDocumentGetById.ModifiedOn);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsDocumentGetById.ModifiedByUserId),
                                    UserName = rdr.GetString(ColumnsDocumentGetById.ModifiedByUserName)
                                };

                                res.ModifiedBy.Employee = Employee.GetByUserId(res.ModifiedBy.Id);
                            }

                            res.AddVersion(new DocumentVersion()
                            {
                                Id = rdr.GetInt64(ColumnsDocumentGetById.VersionId),
                                Company = company,
                                DocumentId = documentId,
                                User = new ApplicationUser(rdr.GetInt32(ColumnsDocumentGetById.UserCreate)),
                                Version = rdr.GetInt32(ColumnsDocumentGetById.Version),
                                Date = rdr.GetDateTime(ColumnsDocumentGetById.VersionDate),
                                State = DocumentVersion.IntegerToStatus(rdr.GetInt32(ColumnsDocumentGetById.Status)),
                                Reason = rdr.GetString(ColumnsDocumentGetById.Reason),
                                UserCreateName = rdr.GetString(ColumnsDocumentGetById.ModifiedByUserName)
                            });
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetById({0},{1}))", documentId, company));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetById({0},{1}))", documentId, company));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetById({0},{1}))", documentId, company));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetById({0},{1}))", documentId, company));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetById({0},{1}))", documentId, company));
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Document::GetById({0},{1}))", documentId, company));
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
        /// Filter drafts documents in company documents
        /// </summary>
        /// <param name="documents">List of documents</param>
        /// <returns>List of documents in draft mode</returns>
        public static ReadOnlyCollection<Document> Drafts(ReadOnlyCollection<Document> documents)
        {
            List<Document> res = new List<Document>();
            if (documents != null)
            {
                foreach (Document document in documents)
                {
                    if (document.LastVersion.State == DocumentStatus.Draft)
                    {
                        res.Add(document);
                    }
                }
            }

            return new ReadOnlyCollection<Document>(res);
        }

        /// <summary>
        /// Gets recent documents
        /// </summary>
        /// <param name="documents">List of documents</param>
        /// <returns>Filtered list of documents</returns>
        public static ReadOnlyCollection<Document> Recent(ReadOnlyCollection<Document> documents)
        {
            List<Document> res = new List<Document>();
            if (documents != null)
            {
                foreach (Document document in documents)
                {
                    if (document.LastVersion.Date > DateTime.Now.AddDays(-7))
                    {
                        res.Add(document);
                    }
                }
            }

            return new ReadOnlyCollection<Document>(res);
        }

        /// <summary>
        /// Set a version of document
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="companyId">Identifier of company</param>
        /// <param name="version">Number of version</param>
        /// <param name="reason">Reason to upgrade version</param>
        /// <returns>Result of action</returns>
        public static ActionResult Versioned(int documentId, int userId, int companyId, int version, string reason)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Versioned
             * @DocumentId int,
             * @CompanyId int,
             * @Version int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Document_Versioned"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@Version", version));
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Parameters.Add(DataParameter.Input("@Reason", Tools.LimitedText(reason, 100)));

                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}", version));
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
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

            return res;
        }

        /// <summary>
        /// Delete a document
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="companyId">Identifier of company</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="reason">Reason to delete</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int documentId, int companyId, int userId, string reason)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Delete
             * @DocumentId bigint,
             * @CompanyId int,
             * @UserId int,
             * @ExtraData nvarchar(200) */
            using (SqlCommand cmd = new SqlCommand("Document_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@DocumentId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters.Add("@ExtraData", SqlDbType.NVarChar);
                    cmd.Parameters["@DocumentId"].Value = documentId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Parameters["@ExtraData"].Value = Tools.LimitedText(reason, 200);
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
        /// Restore a document
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult Restore(int documentId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Restore
             * @DocumentId int,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Document_Restore"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
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
        /// Creates the HTML code to show a row into table documents
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grantWrite">Indicates if user has grant to write</param>
        /// <param name="grantDelete">Indicates if user has grant to delete</param>
        /// <returns>String with HTML code</returns>
        public string ListRow(Dictionary<string, string> dictionary, bool grantWrite, bool grantDelete)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            string iconRenameFigure = grantWrite ? "icon-edit" : "icon-eye-open";

            string iconRename = string.Format(CultureInfo.GetCultureInfo("en-us"),
                @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-info"" onclick=""DocumentUpdate({0},'{1}');""><i class=""{3} bigger-120""></i></span>", 
                this.Id, 
                Tools.SetTooltip(this.Description), 
                grantWrite ? dictionary["Common_Edit"] : dictionary["Common_View"],
                iconRenameFigure);

            string iconDelete = grantDelete ? string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""DocumentDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", this.Id, Tools.SetTooltip(this.Description), dictionary["Common_Delete"]) : string.Empty;

            DocumentVersion actual = this.LastVersion;
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr><td>{0}</td><td class=""hidden-480"" style=""width:110px;"">{1}</td><td class=""hidden-480"" align=""right"" style=""width:110px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</td<</tr>",
                this.Link,
                this.Code,
                actual.Version,
                iconRename, 
                iconDelete);
        }

        /// <summary>
        /// Creates the HTML code to show a row of an inactive document into table documents
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grantWrite">Indicates if user has grant to write</param>
        /// <param name="grantDelete">Indicates if user has grant to delete</param>
        /// <returns>String with HTML code</returns>
        public string ListRowInactive(Dictionary<string, string> dictionary, bool grantWrite, bool grantDelete)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            DocumentVersion actual = this.LastVersion;
            string pattern = @"
                <tr>
                    <td>{0}</td>
                    <td class=""hidden-480"" style=""width:110px;"">{1}</td>
                    <td class=""hidden-480"" style=""width:60px;"" align=""right"">{2}</td>
                    <td style=""width:120px;"">{3}</td>
                    <td style=""width:140px;"">{4:dd/MM/yyyy}</td>
                    <td style=""width:90px;""><span title=""{0}"" class=""btn btn-xs btn-info"" onclick=""Restore({5},'{0}');""><i class=""icon-undo bigger-120""></i></span></td>
                </tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                this.Description,
                this.Code,
                actual.Version,
                this.ModifiedBy.Description,
                this.ModifiedOn,
                this.Id);
        }

        /// <summary>
        /// Obtain the last version number of document
        /// </summary>
        /// <returns>The last version number of document</returns>
        public int LastVersionNumber()
        {
            int res = 0;
            /* CREATE PROCEDURE Document_LastVersion
             * @DocumentId bigint,
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("Document_LastVersion"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DocumentId", SqlDbType.BigInt);
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters["@DocumentId"].Value = this.Id;
                cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res = rdr.GetInt32(0);
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

            return res;
        }

        /// <summary>
        /// Insert a document into data base
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="version">Document version</param>
        /// <returns>Result of action with new document identifier if success</returns>
        public ActionResult Insert(int userId, int version)
        {
            ActionResult res = new ActionResult() { Success = false, MessageError = "No action" };
            /* CREATE PROCEDURE Document_Insert
             * @DocumentId bigint out,
             * @CompanyId int,
             * @Description nvarchar(50),
             * @CategoryId int,
             * @Origen int,
             * @ProcedenciaId int,
             * @Conservacion int,
             * @ConservacionType int,
             * @Activo bit,
             * @Codigo nvarchar(10),
             * @Ubicacion nvarchar(100),
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Document_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.OutputInt("@DocumentId"));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                cmd.Parameters.Add(DataParameter.Input("@Origen", this.Source));
                cmd.Parameters.Add(DataParameter.Input("@CategoryId", this.Category.Id));
                cmd.Parameters.Add(DataParameter.Input("@ProcedenciaId", this.Origin.Id));
                cmd.Parameters.Add(DataParameter.Input("@Conservacion", this.Conservation));
                cmd.Parameters.Add(DataParameter.Input("@ConservacionType", this.ConservationType));
                cmd.Parameters.Add(DataParameter.Input("@Activo", true));
                cmd.Parameters.Add(DataParameter.Input("@Codigo", this.Code));
                cmd.Parameters.Add(DataParameter.Input("@Ubicacion", this.Location));
                cmd.Parameters.Add(DataParameter.Input("@Version", version));
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Parameters.Add(DataParameter.Input("@FechaAlta", this.StartDate));
                if (this.Origin.Id == 0)
                {
                    cmd.Parameters["@ProcedenciaId"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@ProcedenciaId"].Value = this.Origin.Id;
                }

                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@DocumentId"].Value, CultureInfo.GetCultureInfo("en-us"));
                    res.SetSuccess(Convert.ToInt32(cmd.Parameters["@DocumentId"].Value, CultureInfo.GetCultureInfo("en-us")));
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, "Document::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "UserId:{0}", userId));
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, "Document::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "UserId:{0}", userId));
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, "Document::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "UserId:{0}", userId));
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
        /// Update a documento in data base
        /// </summary>
        /// <param name="userId">Identifer of user that perfoms the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Update
             * @DocumentId bigint,
             * @CompanyId int,
             * @Description nvarchar(50),
             * @CategoryId int,
             * @FechaAlta date,
             * @FechaBaja date,
             * @Origen int,
             * @ProcedenciaId int,
             * @Conservacion int,
             * @ConservacionType int,
             * @Activo bit,
             * @Codigo nvarchar(10),
             * @Ubicacion nvarchar(100),
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Document_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DocumentId", SqlDbType.BigInt);
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                cmd.Parameters.Add("@FechaAlta", SqlDbType.Date);
                //cmd.Parameters.Add("@FechaBaja", SqlDbType.Date);
                cmd.Parameters.Add("@Origen", SqlDbType.Int);
                cmd.Parameters.Add("@CategoryId", SqlDbType.Int);
                cmd.Parameters.Add("@ProcedenciaId", SqlDbType.Int);
                cmd.Parameters.Add("@Conservacion", SqlDbType.Int);
                cmd.Parameters.Add("@ConservacionType", SqlDbType.Int);
                cmd.Parameters.Add("@Activo", SqlDbType.Bit);
                cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Ubicacion", SqlDbType.NVarChar);
                cmd.Parameters.Add("@UserId", SqlDbType.Int);

                cmd.Parameters["@DocumentId"].Value = this.Id;
                cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                cmd.Parameters["@Description"].Value = this.Description;
                cmd.Parameters["@FechaAlta"].Value = this.StartDate;
                

                //if (this.EndDate.HasValue)
                //{
                //    cmd.Parameters["@FechaBaja"].Value = this.EndDate;
                //}
                //else
                //{
                //    cmd.Parameters["@FechaBaja"].Value = DBNull.Value;
                //}

                cmd.Parameters["@Origen"].Value = this.Source;
                cmd.Parameters["@CategoryId"].Value = this.Category.Id;

                if (this.Origin.Id > 0)
                {
                    cmd.Parameters["@ProcedenciaId"].Value = this.Origin.Id;
                }
                else
                {
                    cmd.Parameters["@ProcedenciaId"].Value = DBNull.Value;
                }

                cmd.Parameters["@Conservacion"].Value = this.Conservation;
                cmd.Parameters["@ConservacionType"].Value = this.ConservationType;
                cmd.Parameters["@Activo"].Value = this.Active;
                cmd.Parameters["@Codigo"].Value = this.Code;
                cmd.Parameters["@Ubicacion"].Value = this.Location;
                cmd.Parameters["@UserId"].Value = userId;

                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.Success = true;
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
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

            return res;
        }

        /// <summary>
        /// Add a version to document
        /// </summary>
        /// <param name="version">Document version</param>
        public void AddVersion(DocumentVersion version)
        {
            if (this.versions == null)
            {
                this.versions = new List<DocumentVersion>();
            }

            this.versions.Add(version);
        }
    }
}
