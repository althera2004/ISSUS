// --------------------------------
// <copyright file="Document.cs" company="OpenFramework">
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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements document class</summary>
    public class Document : BaseItem
    {
        /// <summary>Versions of document</summary>
        private List<DocumentVersion> versions;

        /// <summary>Initializes a new instance of the Document class</summary>
        public Document()
        {
            this.versions = new List<DocumentVersion>();
            this.Conservation = 0;
            this.ConservationType = 0;
            this.Source = true;
        }

        /// <summary>Gets or sets the code of document</summary>
        [DifferenciableAttribute]
        public string Code { get; set; }

        /// <summary>Gets or sets the category of document</summary>
        [DifferenciableAttribute]
        public DocumentCategory Category { get; set; }

        /// <summary>Gets or sets the origin of document</summary>
        [DifferenciableAttribute]
        public DocumentOrigin Origin { get; set; }

        /// <summary>Gets or sets the location of document</summary>
        [DifferenciableAttribute]
        public string Location { get; set; }

        /// <summary>Gets or sets the record date of document</summary>
        [DifferenciableAttribute]
        public DateTime StartDate { get; set; }

        /// <summary>Gets or sets the disabling date of document</summary>
        [DifferenciableAttribute]
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the source of document</summary>
        [DifferenciableAttribute]
        public bool Source { get; set; }

        /// <summary>Gets or sets the quantity of document conservation</summary>
        [DifferenciableAttribute]
        public int Conservation { get; set; }

        /// <summary>Gets or sets the time unity of document conservation</summary>
        [DifferenciableAttribute]
        public int ConservationType { get; set; }

        /// <summary>Gets the versions of document</summary>
        public ReadOnlyCollection<DocumentVersion> Versions
        {
            get
            {
                return new ReadOnlyCollection<DocumentVersion>(this.versions.OrderByDescending(x => x.Date).ToList());
            }
        }

        /// <summary>Gets the HTML code for a document link</summary>
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

        /// <summary>Gets the JSON strucutre with basic data</summary>
        public string JsonSimple
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Code"":""{1}"",""Description"":""{2}""}}", this.Id, this.Code.Replace("\"", "\\\""), this.Description.Replace("\"", "\\\""));
            }
        }

        /// <summary>Gets or sets the reason of inactivate document</summary>
        public string EndReason { get; set; }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var actual = this.LastVersion;
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
                ""Category"":{5},
                ""RevisionDate"":""{6}"",
                ""Origin"":{{""Id"":{7}}},
                ""ConservationType"":{8},
                ""Conservation"":{9},
                ""Source"":{10},
                ""Location"":""{11}"",
                ""LastVersion"":{13},
                ""Baja"": {14},
                ""EndReason"":""{15}"",
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
                    this.Category.JsonKeyValue,
                    string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyy}", actual.Date),
                    this.Origin.Id,
                    this.ConservationType,
                    this.Conservation,
                    this.Source ? "true" : "false",
                    Tools.JsonCompliant(this.Location),
                    this.Active ? "true" : "false",
                    actual.Version,
                    this.EndDate.HasValue ? "true" : "false",
                    Tools.JsonCompliant(this.EndReason));
            }
        }

        /// <summary>Gets the last version</summary>
        public DocumentVersion LastVersion
        {
            get
            {
                var res = new DocumentVersion();
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

        /// <summary>Gets the last version</summary>
        public int LastNumber
        {
            get
            {
                int res = 0;
                foreach (var version in this.Versions)
                {
                    if (version.Version > res)
                    {
                        res = version.Version;
                    }
                }

                return res;
            }
        }

        /// <summary>Gets a value indicating whether if document is active</summary>
        public new bool Active
        {
            get
            {
                return !this.EndDate.HasValue;
            }
        }

        /// <summary>Gets the HTML code for a document in the table of inactive documents</summary>
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

        /// <summary>Obtains a JSON array of documents</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>JSON array of documents</returns>
        public static string GetAllJson(int companyId)
        {
            var res = new StringBuilder("[");
            var first = true;
            var documents = ByCompany(companyId);
            foreach (var document in documents)
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

        /// <summary>Filter the inactive documents from company's documents</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> LastModified(int companyId)
        {
            var res = new List<Document>();
            /* CREATE PROCEDURE Documents_GetInactive
             * @CompanyId int */
            using (var cmd = new SqlCommand("Documents_LastModified"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters["@CompanyId"].Value = companyId;
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newDocument = new Document()
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

        /// <summary>Gets the inactive documents of company</summary>
        /// <param name="company">Company to search in</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> InactiveByCompany(Company company)
        {
            var res = new List<Document>();
            if (company != null)
            {
                using (var cmd = new SqlCommand("Company_GetDocumentsInactive"))
                {
                    try
                    {
                        using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", company.Id));
                            var newDocument = new Document();
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
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

        /// <summary>Get all document of company from data base</summary>
        /// <param name="company">Company to search in</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> GetByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<Document>(new List<Document>());
            }

            return ByCompany(company.Id);
        }

        /// <summary>Get all document of company from data base</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A list of documents</returns>
        public static ReadOnlyCollection<Document> ByCompany(int companyId)
        {
            var res = new List<Document>();
            using (var cmd = new SqlCommand("Company_GetDocuments"))
            {
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        var newDocument = new Document();
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
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
                                        StartDate = rdr.GetDateTime(ColumnsCompanyGetDocuments.StartDate),
                                        Category = new DocumentCategory()
                                        {
                                            Description = rdr.GetString(ColumnsCompanyGetDocuments.CategoryName),
                                            Id = rdr.GetInt32(ColumnsCompanyGetDocuments.CategoryId)
                                        },
                                        Origin = new DocumentOrigin()
                                        {
                                            Description = rdr.GetString(ColumnsCompanyGetDocuments.SourceName),
                                            Id = rdr.GetInt32(ColumnsCompanyGetDocuments.SourceId)
                                        },
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

        /// <summary>Get a document from data base</summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="company">Company to search in</param>
        /// <returns>Document objet</returns>
        public static Document GetById(long documentId, Company company)
        {
            if (company == null)
            {
                return new Document();
            }

            return ById(documentId, company.Id);
        }

        /// <summary>Get a document from data base</summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Document objet</returns>
        public static Document ById(long documentId, int companyId)
        {
            var res = new Document();
            var company = new Company(companyId);
            using (var cmd = new SqlCommand("Document_GetById"))
            {
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
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
                                    res.Category = new DocumentCategory
                                    {
                                        Id = rdr.GetInt32(ColumnsDocumentGetById.CategoryId),
                                        Description = rdr.GetString(ColumnsDocumentGetById.CategoryName),
                                        CompanyId = company.Id
                                    };

                                    res.Origin = new DocumentOrigin
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
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsDocumentGetById.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsDocumentGetById.ModifiedByUserName)
                                    };

                                    res.ModifiedBy.Employee = Employee.ByUserId(res.ModifiedBy.Id);
                                    res.EndReason = rdr.GetString(ColumnsDocumentGetById.EndReason);
                                }

                                res.AddVersion(new DocumentVersion
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

        /// <summary>Filter drafts documents in company documents</summary>
        /// <param name="documents">List of documents</param>
        /// <returns>List of documents in draft mode</returns>
        public static ReadOnlyCollection<Document> Drafts(ReadOnlyCollection<Document> documents)
        {
            var res = new List<Document>();
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    if (document.LastVersion.State == DocumentsStatus.Draft)
                    {
                        res.Add(document);
                    }
                }
            }

            return new ReadOnlyCollection<Document>(res);
        }

        /// <summary>Gets recent documents</summary>
        /// <param name="documents">List of documents</param>
        /// <returns>Filtered list of documents</returns>
        public static ReadOnlyCollection<Document> Recent(ReadOnlyCollection<Document> documents)
        {
            var res = new List<Document>();
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    if (document.LastVersion.Date > DateTime.Now.AddDays(-7))
                    {
                        res.Add(document);
                    }
                }
            }

            return new ReadOnlyCollection<Document>(res);
        }

        /// <summary>Set a version of document</summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="companyId">Identifier of company</param>
        /// <param name="version">Number of version</param>
        /// <param name="reason">Reason to upgrade version</param>
        /// <returns>Result of action</returns>
        public static ActionResult Versioned(int documentId, int userId, int companyId, int version, string reason, DateTime date)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Versioned
             * @DocumentId int,
             * @CompanyId int,
             * @Version int,
             * @UserId int */
            using (var cmd = new SqlCommand("Document_Versioned"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@Version", version));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@Reason", reason, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Date", date));

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "{0}", version));
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
            }

            return res;
        }

        /// <summary>Delete a document</summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="companyId">Identifier of company</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="reason">Reason to delete</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int documentId, int companyId, int userId, string reason)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Delete
             * @DocumentId bigint,
             * @CompanyId int,
             * @UserId int,
             * @ExtraData nvarchar(200) */
            using (var cmd = new SqlCommand("Document_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@ExtraData", Tools.LimitedText(reason, 200)));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        //Document_7_V

                        /*****************/
                        string path = HttpContext.Current.Request.PhysicalApplicationPath;
                        if (!path.EndsWith("\\"))
                        {
                            path += "\\";
                        }

                        path = string.Format(@"{0}DOCS\{1}", path, companyId);
                        var searchPattenr = string.Format("Document_{0}_v*.*", documentId);
                        var filePaths = Directory.GetFiles(path, searchPattenr);
                        {
                            foreach(var fileName in filePaths)
                            {
                                if (File.Exists(fileName))
                                {
                                    File.Delete(fileName);
                                }
                            }
                        }
                         /****************/


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

        /// <summary>Restore a document</summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult Restore(int documentId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Restore
             * @DocumentId int,
             * @CompanyId int,
             * @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Document_Restore"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
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

        /// <summary>Anulates document</summary>
        /// <param name="documentId">document identifier</param>
        /// <param name="endDate">Date of anulation</param>
        /// <param name="endReason">Reason for anulation</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Anulate(int documentId, DateTime endDate, string endReason, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Document_Anulate]
             *   @DocumentId int,
             *   @CompanyId int,
             *   @EndDate datetime,
             *   @EndReason nvarchar(500),
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Document_Anulate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", endDate));
                        cmd.Parameters.Add(DataParameter.Input("@EndReason", endReason, 500));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
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

        /// <summary>Creates the HTML code to show a row into table documents</summary>
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

            string iconRename = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-info"" onclick=""DocumentUpdate({0},'{1}');""><i class=""{3} bigger-120""></i></span>",
                this.Id,
                Tools.SetTooltip(this.Description),
                grantWrite ? dictionary["Common_Edit"] : dictionary["Common_View"],
                iconRenameFigure);

            string iconDelete = grantDelete ? string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""DocumentDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", this.Id, Tools.SetTooltip(this.Description), dictionary["Common_Delete"]) : string.Empty;

            var actual = this.LastVersion;
            return string.Format(
                CultureInfo.InvariantCulture,
                @"<tr><td>{0}</td><td class=""hidden-480"" style=""width:110px;"">{1}</td><td class=""hidden-480"" align=""right"" style=""width:110px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</td<</tr>",
                this.Link,
                this.Code,
                actual.Version,
                iconRename,
                iconDelete);
        }

        /// <summary>Creates the HTML code to show a row of an inactive document into table documents</summary>
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

            var actual = this.LastVersion;
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
                CultureInfo.InvariantCulture,
                pattern,
                this.Description,
                this.Code,
                actual.Version,
                this.ModifiedBy.Description,
                this.ModifiedOn,
                this.Id);
        }

        /// <summary>Obtain the last version number of document</summary>
        /// <returns>The last version number of document</returns>
        public int LastVersionNumber()
        {
            int res = 0;
            /* CREATE PROCEDURE Document_LastVersion
             * @DocumentId bigint,
             * @CompanyId int */
            using (var cmd = new SqlCommand("Document_LastVersion"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@DocumentId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = rdr.GetInt32(0);
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

            return res;
        }

        /// <summary>Insert a document into data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="version">Document version</param>
        /// <returns>Result of action with new document identifier if success</returns>
        public ActionResult Insert(int userId, int version, DateTime revisionDate)
        {
            var res = ActionResult.NoAction;
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
            using (var cmd = new SqlCommand("Document_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputInt("@DocumentId"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Origen", this.Source));
                    cmd.Parameters.Add(DataParameter.Input("@CategoryId", this.Category.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ProcedenciaId", this.Origin.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Conservacion", this.Conservation));
                    cmd.Parameters.Add(DataParameter.Input("@ConservacionType", this.ConservationType));
                    cmd.Parameters.Add(DataParameter.Input("@Activo", Constant.DefaultActive));
                    cmd.Parameters.Add(DataParameter.Input("@Codigo", this.Code, 25));
                    cmd.Parameters.Add(DataParameter.Input("@Ubicacion", this.Location, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Version", version));
                    cmd.Parameters.Add(DataParameter.Input("@Revisiondate", revisionDate));
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
            }

            return res;
        }

        /// <summary>Update a documento in data base</summary>
        /// <param name="userId">Identifer of user that perfoms the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Document_Update
             * @DocumentId bigint,
             * @CompanyId int,
             * @Description nvarchar(100),
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
            using (var cmd = new SqlCommand("Document_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@DocumentId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                    cmd.Parameters.Add(DataParameter.Input("@FechaAlta", this.StartDate));
                    cmd.Parameters.Add(DataParameter.Input("@Origen", this.Source));
                    cmd.Parameters.Add(DataParameter.Input("@CategoryId", this.Category.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Conservacion", this.Conservation));
                    cmd.Parameters.Add(DataParameter.Input("@ConservacionType", this.ConservationType));
                    cmd.Parameters.Add(DataParameter.Input("@Activo", this.Active));
                    cmd.Parameters.Add(DataParameter.Input("@Codigo", this.Code, 25));
                    cmd.Parameters.Add(DataParameter.Input("@Ubicacion", this.Location, 100));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add("@ProcedenciaId", SqlDbType.Int);
                    if (this.Origin.Id > 0)
                    {
                        cmd.Parameters["@ProcedenciaId"].Value = this.Origin.Id;
                    }
                    else
                    {
                        cmd.Parameters["@ProcedenciaId"].Value = DBNull.Value;
                    }

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
            }

            return res;
        }

        /// <summary>Add a version to document</summary>
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