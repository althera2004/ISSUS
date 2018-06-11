// --------------------------------
// <copyright file="UploadFile.cs" company="Sbrinna">
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
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

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
                case ItemValues.Dashboard: return "DashBoard";
                case ItemValues.CompanyProfile: return "CompanyData";
                case ItemValues.JobPosition: return "JobPositions";
                case ItemValues.Department: return "Departments";
                case ItemValues.Employee: return "Employees";
                case ItemValues.User: return "Users";
                case ItemValues.Trace: return "Traces";
                case ItemValues.Document: return "Documents";
                case ItemValues.Proccess: return "Processes";
                case ItemValues.Learning: return "Learning";
                case ItemValues.Equipment: return "Equipments";
                case ItemValues.Incident: return "Incidents";
                case ItemValues.IncidentActions: return "IncidentActions";
                case ItemValues.Provider: return "Providers";
                case ItemValues.Customer: return "Customers";
                case ItemValues.Cost: return "IncidentCosts";
                case ItemValues.BusinessRisk: return "BusinessRisks";
                case ItemValues.Rules: return "Rules";
                case ItemValues.CostDefinition: return "CostDefinition";
                case ItemValues.Objetivo: return "Objetivo";
                case ItemValues.Indicador: return "Inicador";
                case ItemValues.Oportunity: return "Oportunity";
                default:return string.Empty;
            }
        }

        public static UploadFile Empty
        {
            get
            {
                return new UploadFile
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
            var res = new List<UploadFile>();
            using (var cmd = new SqlCommand("UploadFiles_GetByItem"))
            {
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemLinked", itemLinked));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var attach = new UploadFile
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
            using (var cmd = new SqlCommand("UploadFiles_GetByItem"))
            {
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemLinked", itemLinked));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                res = true;
                            }
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
            using (var cmd = new SqlCommand("UploadFiles_GetById"))
            {
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var attach = new UploadFile
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
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("UploadFiles_Insert"))
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
            var res = ActionResult.NoAction;
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

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;

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
            long businessRisk = 0;
            long employee = 0;
            long oportunity = 0;
            long indicator = 0;
            long objetive = 0;
            long jobPosition = 0;
            long learning = 0;
            long processes = 0;
            long other = 0;
            long free = company.DiskQuote;

            var files = Directory.GetFiles(path);
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
                    case "Employee":
                        employee += size;
                        break;
                    case "Processes":
                        processes += size;
                        break;
                    case "Learning":
                        learning += size;
                        break;
                    case "Oportinuty":
                        oportunity += size;
                        break;
                    case "JobPosition":
                        jobPosition += size;
                        break;
                    case "BusinessRisks":
                        businessRisk += size;
                        break;
                    default:
                        other += size;
                        break;
                }

                free -= size;
            }

            if (free < 0)
            {
                free = 0;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"[
                    {{""label"": ""{14}"", ""value"": {0}}},
                    {{""label"": ""{15}"", ""value"": {1}}},
                    {{""label"": ""{16}"", ""value"": {2}}},
                    {{""label"": ""{17}"", ""value"": {3}}},
                    {{""label"": ""{18}"", ""value"": {4}}},
                    {{""label"": ""{19}"", ""value"": {5}}},
                    {{""label"": ""{20}"", ""value"": {6}}},
                    {{""label"": ""{21}"", ""value"": {7}}},
                    {{""label"": ""{22}"", ""value"": {8}}},
                    {{""label"": ""{23}"", ""value"": {9}}},
                    {{""label"": ""{24}"", ""value"": {10}}},
                    {{""label"": ""{25}"", ""value"": {11}}},
                    {{""label"": ""{26}"", ""value"": {12}}},
                    {{""label"": ""{27}"", ""value"": {13}}}
                ];",
                documents * 100M / company.DiskQuote,
                equipments * 100M / company.DiskQuote,
                incidents * 100M / company.DiskQuote,
                incidentActions * 100M / company.DiskQuote,
                businessRisk * 100M / company.DiskQuote,
                jobPosition * 100M / company.DiskQuote,
                employee * 100M / company.DiskQuote,
                oportunity * 100M / company.DiskQuote,
                processes * 100M / company.DiskQuote,
                objetive * 100M / company.DiskQuote,
                indicator * 100M / company.DiskQuote,
                learning * 100M / company.DiskQuote,
                other * 100M / company.DiskQuote,
                free * 100M / company.DiskQuote,
                Tools.JsonCompliant(dictionary["Item_Documents"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Equipments"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Incidents"].Trim()),
                Tools.JsonCompliant(dictionary["Item_IncidentActions"].Trim()),
                Tools.JsonCompliant(dictionary["Item_BusinessRisks"].Trim()),
                Tools.JsonCompliant(dictionary["Item_JobPositions"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Employees"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Oportunities"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Processes"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Objetivos"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Indicadores"].Trim()),
                Tools.JsonCompliant(dictionary["Item_Learning"].Trim()),
                Tools.JsonCompliant(dictionary["DiskQuote_Other"].Trim()),
                Tools.JsonCompliant(dictionary["DiskQuote_Free"].Trim()));
        }
    }
}