

namespace GisoFramework.Item
{
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;
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

    public class UploadFile
    {
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public int ItemLinked { get; set; }
        public long ItemId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static string ResolveItemLinked(int itemLinked)
        {
            switch (itemLinked)
            {

                case 1: return "DashBoard";
                case 2: return "CompanyData";
                case 3: return "JobPositions";
                case 4: return "Departments";
                case 5: return "Employees";
                case 6: return "Users";
                case 7: return "Traces";
                case 8: return "Documents";
                case 9: return "Processes";
                case 10: return "Learning";
                case 11: return "Equipments";
                case 12: return "Incidents";
                case 13: return "IncidentActions";
                case 14: return "Providers";
                case 15: return "Customers";
                case 16: return "IncidentCosts";
                case 18: return "BusinessRisks";
                case 19: return "Rules";
                case 20: return "CostDefinition";
            }

            return string.Empty;
        }

        public static UploadFile Empty
        {
            get
            {
                return new UploadFile()
                {
                    Id = 0,
                    CompanyId = 0,
                    ItemLinked = 0,
                    ItemId = 0,
                    Description = string.Empty,
                    Extension = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string JsonKeyValue
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

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"": {0},
                        ""CompanyId"": {1},
                        ""ItemLinked"": {2},
                        ""ItemId"": {3},
                        ""Description"": ""{4}"",
                        ""Extension"": ""{5}"",
                        ""FileName"": ""{6}"",
                        ""CreatedBy"": {7},
                        ""CreatedOn"": ""{8:dd/MM/yyyy}"",
                        ""ModifiedBy"": {9},
                        ""ModifiedOn"": ""{10:dd/MM/yyyy}"",
                        ""Active"": {11},
                        ""Size"": {12}
                    }}",
                       this.Id,
                       this.CompanyId,
                       this.ItemLinked,
                       this.ItemId,
                       Tools.JsonCompliant(this.Description),
                       this.Extension,
                       this.FileName,
                       this.CreatedBy.JsonKeyValue,
                       this.CreatedOn,
                       this.ModifiedBy.JsonKeyValue,
                       this.ModifiedOn,
                       this.Active ? "true" : "false",
                       this.Size);
            }
        }

        public static ReadOnlyCollection<UploadFile> GetByItem(int itemLinked, int itemId, int companyId)
        {
            return GetByItem(itemLinked, Convert.ToInt64(itemId), companyId);
        }

        public static ReadOnlyCollection<UploadFile> GetByItem(int itemLinked, long itemId, int companyId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                "UploadFile.GetByItem(itemLinked:{0}, itemId:{1}, companyId{2})",
                itemLinked,
                itemId,
                companyId);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);
            List<UploadFile> res = new List<UploadFile>();
            using (SqlCommand cmd = new SqlCommand("UploadFiles_GetByItem"))
            {
                try
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ItemLinked", itemLinked));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            UploadFile attach = new UploadFile()
                            {
                                Id = rdr.GetInt64(ColumnsUploadFileGet.Id),
                                ItemLinked = rdr.GetInt32(ColumnsUploadFileGet.ItemLinked),
                                ItemId = rdr.GetInt64(ColumnsUploadFileGet.ItemId),
                                CompanyId = Convert.ToInt32(ColumnsUploadFileGet.CompanyId),
                                FileName = rdr.GetString(ColumnsUploadFileGet.FileName),
                                Description = rdr.GetString(ColumnsUploadFileGet.Description),
                                Extension = rdr.GetString(ColumnsUploadFileGet.Extension).Trim().ToUpperInvariant(),
                                CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsUploadFileGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsUploadFileGet.CreatdByLogin)
                                },
                                CreatedOn = rdr.GetDateTime(ColumnsUploadFileGet.CreatedOn),
                                ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsUploadFileGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsUploadFileGet.ModifiedByLogin)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsUploadFileGet.ModifiedOn),
                                Active = rdr.GetBoolean(ColumnsUploadFileGet.Active)
                            };

                            string fileName = string.Format(CultureInfo.InvariantCulture, @"{0}{1}", path, attach.FileName);
                            if (File.Exists(fileName))
                            {
                                long length = new System.IO.FileInfo(fileName).Length;
                                attach.Size = length;
                            }
                            else
                            {
                                attach.Size = 0;
                                attach.Description = Path.GetFileName(fileName);
                                attach.Extension = "nofile";
                            }

                            res.Add(attach);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<UploadFile>(res);
        }

        public static bool HasAttachemnts(int itemLinked, long itemId, int companyId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                "UploadFile.HasAttachemnts(itemLinked:{0}, itemId:{1}, companyId{2})",
                itemLinked,
                itemId,
                companyId);
            bool res = false;
            using (SqlCommand cmd = new SqlCommand("UploadFiles_GetByItem"))
            {
                try
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ItemLinked", itemLinked));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            res = true;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, source);
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

        public static UploadFile GetById(long id, int companyId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                "UploadFile.GetByItem(id:{0}, companyId{1})",
                id,
                companyId);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);
            using (SqlCommand cmd = new SqlCommand("UploadFiles_GetById"))
            {
                try
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            UploadFile attach = new UploadFile()
                            {
                                Id = rdr.GetInt64(ColumnsUploadFileGet.Id),
                                ItemLinked = rdr.GetInt32(ColumnsUploadFileGet.ItemLinked),
                                ItemId = rdr.GetInt64(ColumnsUploadFileGet.ItemId),
                                CompanyId = Convert.ToInt32(ColumnsUploadFileGet.CompanyId),
                                FileName = rdr.GetString(ColumnsUploadFileGet.FileName),
                                Description = rdr.GetString(ColumnsUploadFileGet.Description),
                                Extension = rdr.GetString(ColumnsUploadFileGet.Extension).Trim().ToUpperInvariant(),
                                CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsUploadFileGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsUploadFileGet.CreatdByLogin)
                                },
                                CreatedOn = rdr.GetDateTime(ColumnsUploadFileGet.CreatedOn),
                                ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsUploadFileGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsUploadFileGet.ModifiedByLogin)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsUploadFileGet.ModifiedOn),
                                Active = rdr.GetBoolean(ColumnsUploadFileGet.Active)
                            };

                            long length = 0;
                            string finalPath = string.Format(CultureInfo.InvariantCulture, @"{0}{1}", path, attach.FileName);
                            if (File.Exists(finalPath))
                            {
                                length = new System.IO.FileInfo(finalPath).Length;
                            }

                            attach.Size = length;

                            return attach;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return UploadFile.Empty;
        }

        public ActionResult Insert(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                "UploadFile.Insert(ApplicationUserId:{0}",
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("UploadFiles_Insert"))
            {
                /* CREATE PROCEDURE UploadFiles_Insert
                 *   @Id bigint output,
                 *   @CompanyId bigint,
                 *   @ItemLinked int,
                 *   @ItemId bigint,
                 *   @FileName nvarchar(250),
                 *   @Description nvarchar(100),
                 *   @Extension nvarchar(10),
                 *   @ApplicationUserId int */
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                cmd.Parameters.Add(DataParameter.Input("@ItemLinked", this.ItemLinked));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                cmd.Parameters.Add(DataParameter.Input("@FileName", this.FileName, 250));
                cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                cmd.Parameters.Add(DataParameter.Input("@Extension", this.Extension.Trim().ToUpperInvariant(), 10));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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

        public static ActionResult Delete(long attachId, int companyId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE UploadFiled_Inactive
             *   @Id bigint,
             *   @CompanyId bigint */

            var uploadFile = GetById(attachId, companyId);
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);
            using (var cmd = new SqlCommand("UploadFiled_Inactive"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@Id", attachId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(attachId);
                        try
                        {
                            path = string.Format(CultureInfo.InvariantCulture, "{0}{1}", path, uploadFile.FileName);
                            if (File.Exists(path))
                            {

                                File.Delete(path);

                            }
                        }
                        catch (Exception ex) { }
                        finally
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                }
            }
            return res;
        }

        public static string GetQuota(Company company)
        {
            if (company == null)
            {
                return string.Empty;
            }

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, company.Id);

            long documents = 0;
            long equipments = 0;
            long incidents = 0;
            long incidentActions = 0;
            long free = company.DiskQuote;

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                long size = new FileInfo(file).Length;
                string fileName = Path.GetFileName(file).Split('_')[0];
                switch (fileName)
                {
                    case "Document":
                        documents += size;
                        break;
                    case "Equipments":
                        equipments += size;
                        break;
                    case "Incidents":
                        incidents += size;
                        break;
                    case "IncidentActions":
                        incidentActions += size;
                        break;
                }
            }

            free -= documents + equipments + incidentActions + incidents;

            return string.Format(
                CultureInfo.InvariantCulture,
                @"[
                    {{""label"": ""Documents"", ""value"": {0}}},
                    {{""label"": ""Equipments"", ""value"": {1}}},
                    {{""label"": ""Incidents"", ""value"": {2}}},
                    {{""label"": ""IncidentActions"", ""value"": {3}}},
                    {{""label"": ""Free"", ""value"": {4}}}
                ]; //{{""Documents"":{0},""Equipments"":{1},""Incidents"":{2},""IncidentActions"":{3},""Free"":{4}}}",
                documents * 100M / company.DiskQuote,
                equipments * 100M / company.DiskQuote,
                incidents * 100M / company.DiskQuote,
                incidentActions * 100M / company.DiskQuote,
                free * 100M / company.DiskQuote
                );
        }
    }
}
