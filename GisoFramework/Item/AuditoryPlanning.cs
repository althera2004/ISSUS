// --------------------------------
// <copyright file="AuditoryPlanning.cs" company="OpenFramework">
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
    using System.Text;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    public class AuditoryPlanning : BaseItem
    {
        public static AuditoryPlanning Empty
        {
            get
            {
                return new AuditoryPlanning
                {
                    Id = Constant.DefaultId,
                    Date = DateTime.Now,
                    Hour = 0,
                    Duration = "0",
                    Process = Process.Empty,
                    Auditor = ApplicationUser.Empty,
                    Audited = Employee.Empty,
                    SendMail = false,
                    ProviderEmail = string.Empty,
                    ProviderName = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public long AuditoryId { get; set; }

        public DateTime Date { get; set; }

        public int Hour { get; set; }

        public string Duration { get; set; }

        public Process Process { get; set; }

        public ApplicationUser Auditor { get; set; }

        public Employee Audited { get; set; }

        public bool SendMail { get; set; }

        public string ProviderEmail { get; set; }

        public string ProviderName { get; set; }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return this.Json;
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var pattern = @"{{""Id"":{0},
                    ""Date"":""{1:dd/MM/yyyy}"",
                    ""Hour"":{2},
                    ""Duration"":{3},
                    ""Process"":{4},
                    ""Auditor"":{5},
                    ""Audited"":{6},
                    ""SendMail"":{7},
                    ""ProviderEmail"":""{8}"",
                    ""ProviderName"":""{9}"",
                    ""Active"":{10}}}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.Date,
                    this.Hour,
                    this.Duration,
                    this.Process.JsonKeyValue,
                    this.Auditor.JsonKeyValue,
                    this.Audited.JsonKeyValue,
                    this.SendMail ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.ProviderEmail,
                    Tools.JsonCompliant(this.ProviderName),
                    this.Active ? "true" : "false");
            }
        }

        /// <summary>Gets a hyper link to questionary profile page</summary>
        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        public static string JsonList(ReadOnlyCollection<AuditoryPlanning> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var planning in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(planning.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static AuditoryPlanning ById(long id, long auditoryId, int companyId)
        {
            var sources = string.Format(CultureInfo.InvariantCulture, "AuditoryPlaning::>ByAuditory({0},{1})", auditoryId, companyId);
            var res = AuditoryPlanning.Empty;
            /* CREATE PROCEDURE AuditoryPlanning_ById
             *   @Id bigint,
             *   @CompanyId int,
             *   @AuditoryId bigint */
            using (var cmd = new SqlCommand("AuditoryPlanning_ById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = new AuditoryPlanning
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryPlanningGet.Id),
                                    Date = rdr.GetDateTime(ColumnsAuditoryPlanningGet.Date),
                                    Hour = rdr.GetInt32(ColumnsAuditoryPlanningGet.Hour),
                                    Duration = rdr.GetString(ColumnsAuditoryPlanningGet.Duration),
                                    AuditoryId = auditoryId,
                                    CompanyId = companyId,
                                    Auditor = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.AuditorId),
                                        UserName = rdr.GetString(ColumnsAuditoryPlanningGet.AuditorName)
                                    },
                                    Audited = new Employee
                                    {
                                        Id = rdr.GetInt64(ColumnsAuditoryPlanningGet.AuditedId),
                                        Name = rdr.GetString(ColumnsAuditoryPlanningGet.AuditedName),
                                        LastName = rdr.GetString(ColumnsAuditoryPlanningGet.AuditedLastName),
                                        Email = rdr.GetString(ColumnsAuditoryPlanningGet.AuditedEmail)
                                    },
                                    Process = new Process
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.ProcessId),
                                        Description = rdr.GetString(ColumnsAuditoryPlanningGet.ProcessDescription),
                                    },
                                    SendMail = rdr.GetBoolean(ColumnsAuditoryPlanningGet.SendMail),
                                    ProviderEmail = rdr.GetString(ColumnsAuditoryPlanningGet.ProviderEmail),
                                    ProviderName = rdr.GetString(ColumnsAuditoryPlanningGet.ProviderName),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryPlanningGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryPlanningGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryPlanningGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryPlanningGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryPlanningGet.Active)
                                };
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

        public static ReadOnlyCollection<AuditoryPlanning> ByAuditory(long auditoryId, int companyId)
        {
            var sources = string.Format(CultureInfo.InvariantCulture, "AuditoryPlaning::>ByAuditory({0},{1})", auditoryId, companyId);
            var res = new List<AuditoryPlanning>();
            /* CREATE PROCEDURE AuditoryPlanning_ByAuditory
             *   @CompanyId int,
             *   @AuditoryId bigint */
            using (var cmd = new SqlCommand("AuditoryPlanning_ByAuditory"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newPlanning = new AuditoryPlanning
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryPlanningGet.Id),
                                    Date = rdr.GetDateTime(ColumnsAuditoryPlanningGet.Date),
                                    Hour = rdr.GetInt32(ColumnsAuditoryPlanningGet.Hour),
                                    Duration = rdr.GetString(ColumnsAuditoryPlanningGet.Duration),
                                    AuditoryId = auditoryId,
                                    CompanyId = companyId,
                                    Auditor = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.AuditorId),
                                        UserName = rdr.GetString(ColumnsAuditoryPlanningGet.AuditorName)
                                    },
                                    Audited = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.AuditedId),
                                        Name = rdr.GetString(ColumnsAuditoryPlanningGet.AuditedName),
                                        LastName = rdr.GetString(ColumnsAuditoryPlanningGet.AuditedLastName),
                                        Email = rdr.GetString(ColumnsAuditoryPlanningGet.AuditedEmail)
                                    },
                                    Process = new Process
                                    {
                                        Id = rdr.GetInt64(ColumnsAuditoryPlanningGet.ProcessId),
                                        Description = rdr.GetString(ColumnsAuditoryPlanningGet.ProcessDescription),
                                    },
                                    SendMail = rdr.GetBoolean(ColumnsAuditoryPlanningGet.SendMail),
                                    ProviderEmail = rdr.GetString(ColumnsAuditoryPlanningGet.ProviderEmail),
                                    ProviderName = rdr.GetString(ColumnsAuditoryPlanningGet.ProviderName),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryPlanningGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryPlanningGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryPlanningGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryPlanningGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryPlanningGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryPlanningGet.Active)
                                };

                                res.Add(newPlanning);
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

            return new ReadOnlyCollection<AuditoryPlanning>(res);
        }

        public static ActionResult Inactivate(long auditoryPlanningId, int companyId, int applicationUserId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"AuditoryPlanning::Inactivate Id:{0} User:{1} Company:{2}", auditoryPlanningId, applicationUserId, companyId);
            /* CREATE PROCEDURE AuditoryPlanning_Inactivate
             *   @Id bigint,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryPlanning_Inactivate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Id", auditoryPlanningId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
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
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return result;
        }

        public static ActionResult Activate(long auditoryPlanningId, int companyId, int applicationUserId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"AuditoryPlanning::Activate Id:{0} User:{1} Company:{2}", auditoryPlanningId, applicationUserId, companyId);
            /* CREATE PROCEDURE AuditoryPlanning_Activate
             *   @Id bigint,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryPlanning_Activate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Id", auditoryPlanningId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
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
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return result;
        }

        public ActionResult Insert(int applicationUserId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "AuditoryPlanning::>Insert({0})", this.Id);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE AuditoryPlanning_Insert
             *   @Id bigint output,
             *   @CompanyId int,
             *   @AuditoryId bigint,
             *   @Date datetime,
             *   @Hour int,
             *   @Duration int,
             *   @ProcessId bigint,
             *   @Auditor int,
             *   @Audited int,
             *   @SendMail bit,
             *   @ProviderEmail nvarchar(150),
             *   @ProviderName nvarchar(50),
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("AuditoryPlanning_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                    cmd.Parameters.Add(DataParameter.Input("@Hour", this.Hour));
                    cmd.Parameters.Add(DataParameter.Input("@Duration", this.Duration, 6));
                    cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Auditor", this.Auditor.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Audited", this.Audited.Id));
                    cmd.Parameters.Add(DataParameter.Input("@SendMail", this.SendMail));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderEmail", this.ProviderEmail, 150));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderName", this.ProviderName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(this.Id);
                    }
                    catch (Exception ex)
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
                }
            }

            return res;
        }

        public ActionResult Update(int applicationUserId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "AuditoryPlanning::>Update({0})", this.Id);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE AuditoryPlanning_Update
             *   @Id bigint,
             *   @CompanyId int,
             *   @AuditoryId bigint,
             *   @Date datetime,
             *   @Hour int,
             *   @Duration int,
             *   @ProcessId bigint,
             *   @Auditor int,
             *   @Audited int,
             *   @SendMail bit,
             *   @ProviderEmail nvarchar(150),
             *   @ProviderName nvarchar(50),
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("AuditoryPlanning_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                    cmd.Parameters.Add(DataParameter.Input("@Hour", this.Hour));
                    cmd.Parameters.Add(DataParameter.Input("@Duration", this.Duration, 6));
                    cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Auditor", this.Auditor.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Audited", this.Audited.Id));
                    cmd.Parameters.Add(DataParameter.Input("@SendMail", this.SendMail));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderEmail", this.ProviderEmail, 150));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderName", this.ProviderName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(this.Id);
                    }
                    catch (Exception ex)
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
                }
            }

            return res;
        }
    }
}