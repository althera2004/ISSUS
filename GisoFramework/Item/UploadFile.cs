// --------------------------------
// <copyright file="UploadFile.cs" company="OpenFramework">
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

        /// <summary>Gets or sets file name</summary>
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
                case ItemIdentifiers.Dashboard: return "DashBoard";
                case ItemIdentifiers.CompanyProfile: return "CompanyData";
                case ItemIdentifiers.JobPosition: return "JobPositions";
                case ItemIdentifiers.Department: return "Departments";
                case ItemIdentifiers.Employee: return "Employees";
                case ItemIdentifiers.User: return "Users";
                case ItemIdentifiers.Trace: return "Traces";
                case ItemIdentifiers.Document: return "Documents";
                case ItemIdentifiers.Proccess: return "Processes";
                case ItemIdentifiers.Learning: return "Learning";
                case ItemIdentifiers.Equipment: return "Equipments";
                case ItemIdentifiers.Incident: return "Incidents";
                case ItemIdentifiers.IncidentActions: return "IncidentActions";
                case ItemIdentifiers.Provider: return "Providers";
                case ItemIdentifiers.Customer: return "Customers";
                case ItemIdentifiers.Cost: return "IncidentCosts";
                case ItemIdentifiers.BusinessRisk: return "BusinessRisks";
                case ItemIdentifiers.Rules: return "Rules";
                case ItemIdentifiers.CostDefinition: return "CostDefinition";
                case ItemIdentifiers.Objetivo: return "Objetivo";
                case ItemIdentifiers.Indicador: return "Inicador";
                case ItemIdentifiers.Oportunity: return "Oportunity";
                case ItemIdentifiers.Questionary: return "Questionary";
                case ItemIdentifiers.Auditory: return "Auditory";
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
                            if(rdr.HasRows)
                            {
                                rdr.Read();
                                var attach = new UploadFile
                                {
                                    Id = rdr.GetInt64(ColumnsUploadFileGet.Id),
                                    ItemLinked = rdr.GetInt32(ColumnsUploadFileGet.ItemLinked),
                                    ItemId = rdr.GetInt64(ColumnsUploadFileGet.ItemId),
                                    CompanyId = Convert.ToInt32(ColumnsUploadFileGet.CompanyId),
                                    FileName = rdr.GetString(ColumnsUploadFileGet.FileName),
                                    Description = rdr.GetString(ColumnsUploadFileGet.Description),
                                    Extension = rdr.GetString(ColumnsUploadFileGet.Extension).Trim().ToUpperInvariant(),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsUploadFileGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsUploadFileGet.CreatdByLogin)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsUploadFileGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
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
            string source = string.Format(CultureInfo.InvariantCulture, "UploadFile::Delete {0} {1} {2}", attachId, companyId, path);
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
                        // @alex: una vez eliminado de la bbdd se elimina físicamente del directorio
                        try
                        {
                            path = string.Format(CultureInfo.InvariantCulture, "{0}{1}", path, uploadFile.FileName);
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            else
                            {
                                ExceptionManager.Trace(new NotSupportedException(), source);
                            }
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex, source);
                        }
                        catch (IOException ex)
                        {
                            ExceptionManager.Trace(ex, source);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex, source);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex, source);
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

            decimal documents = 0;
            decimal equipments = 0;
            decimal incidents = 0;
            decimal incidentActions = 0;
            decimal businessRisk = 0;
            decimal auditory = 0;
            decimal employee = 0;
            decimal oportunity = 0;
            decimal indicator = 0;
            decimal objetive = 0;
            decimal jobPosition = 0;
            decimal learning = 0;
            decimal processes = 0;
            decimal other = 0;
            decimal free = company.DiskQuote;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                decimal size = Convert.ToDecimal(new FileInfo(file).Length) / (1024 * 1024);
                string fileName = Path.GetFileName(file).Split('_')[0];
                switch (fileName)
                {
                    case "Auditory":
                        auditory += size;
                        break;
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
                    case "Employees":
                        employee += size;
                        break;
                    case "Processes":
                        processes += size;
                        break;
                    case "Learning":
                        learning += size;
                        break;
                    case "Oportunity":
                        oportunity += size;
                        break;
                    case "JobPositions":
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
                    {{""label"": ""{15}"", ""total"": {30}, ""value"": {0}}},
                    {{""label"": ""{16}"", ""total"": {31}, ""value"": {1}}},
                    {{""label"": ""{17}"", ""total"": {32}, ""value"": {2}}},
                    {{""label"": ""{18}"", ""total"": {33}, ""value"": {3}}},
                    {{""label"": ""{19}"", ""total"": {34}, ""value"": {4}}},
                    {{""label"": ""{20}"", ""total"": {35}, ""value"": {5}}},
                    {{""label"": ""{21}"", ""total"": {36}, ""value"": {6}}},
                    {{""label"": ""{22}"", ""total"": {38}, ""value"": {7}}},
                    {{""label"": ""{23}"", ""total"": {39}, ""value"": {8}}},
                    {{""label"": ""{24}"", ""total"": {30}, ""value"": {9}}},
                    {{""label"": ""{25}"", ""total"": {40}, ""value"": {10}}},
                    {{""label"": ""{26}"", ""total"": {41}, ""value"": {11}}},
                    {{""label"": ""{27}"", ""total"": {42}, ""value"": {12}}},
                    {{""label"": ""{28}"", ""total"": {43}, ""value"": {13}}},
                    {{""label"": ""{29}"", ""total"": {44}, ""value"": {14}}}
                ];",
                auditory * 100M / company.DiskQuote,
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
                Tools.JsonCompliant(dictionary["Item_Auditory"].Trim()),
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
                Tools.JsonCompliant(dictionary["DiskQuote_Free"].Trim()),
                auditory,
                documents,
                equipments,
                incidents,
                incidentActions,
                businessRisk,
                jobPosition,
                employee,
                oportunity,
                processes,
                objetive,
                indicator,
                learning,
                other,
                free);
        }
    }
}