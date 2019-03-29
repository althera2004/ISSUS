// --------------------------------
// <copyright file="Auditory.cs" company="OpenFramework">
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
    using System.Linq;
    using System.Text;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    public class Auditory : BaseItem
    {
        private string rulesId;
        private List<Rules> rules { get; set; }
        public Employee InternalResponsible { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public Provider Provider { get; set; }
        public Customer Customer { get; set; }
        public string Descripcion { get; set; }
        public string Notes { get; set; }
        public DateTime? PreviewDate { get; set; }
        public Employee PlannedBy { get; set; }
        public DateTime? PlannedOn { get; set; }
        public Employee ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public Employee ValidatedBy { get; set; }
        public DateTime? ValidatedOn { get; set; }
        public ApplicationUser ValidatedUserBy { get; set; }
        public DateTime? ValidatedUserOn { get; set; }
        public string Scope { get; set; }
        public string EnterpriseAddress { get; set; }
        public int CompanyAddressId { get; set; }
        public string AuditorTeam { get; set; }

        public DateTime? ReportStart { get; set; }
        public DateTime? ReportEnd { get; set; }

        public static Auditory Empty
        {
            get
            {
                return new Auditory
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Descripcion = string.Empty,
                    Description = string.Empty,
                    Scope = string.Empty,
                    Amount = 0,
                    Notes = string.Empty,
                    CompanyAddressId = -1,
                    EnterpriseAddress = string.Empty,
                    AuditorTeam = string.Empty,
                    PlannedBy = Employee.EmptySimple,
                    ClosedBy = Employee.EmptySimple,
                    ValidatedBy = Employee.EmptySimple,
                    ValidatedUserBy = ApplicationUser.Empty,
                    Customer = Customer.Empty,
                    Provider = Provider.Empty,
                    rules = new List<Rules>(),
                    InternalResponsible = Employee.Empty,
                    Status = 0,
                    Type = Constant.DefaultId,
                    CanBeDeleted = true,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public ReadOnlyCollection<Rules> Rules
        {
            get
            {
                if (this.rules == null)
                {
                    this.rules = new List<Rules>();
                }

                return new ReadOnlyCollection<Rules>(this.rules);
            }
        }

        public ReadOnlyCollection<AuditoryCuestionarioImprovement> Improvements
        {
            get
            {
                return AuditoryCuestionarioImprovement.ByAuditory(this.Id, this.CompanyId);
            }
        }

        public ReadOnlyCollection<AuditoryCuestionarioFound> Founds
        {
            get
            {
                return AuditoryCuestionarioFound.ByAuditory(this.Id, this.CompanyId);
            }
        }

        public void AddRule(long ruleId)
        {
            if (this.rules == null)
            {
                this.rules = new List<Rules>();
            }

            bool exists = false;
            foreach (var r in this.rules)
            {
                if (r.Id == ruleId)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                this.rules.Add(new Rules { Id = ruleId });
            }
        }

        public void AddRule(Rules rule)
        {
            if (this.rules == null)
            {
                this.rules = new List<Rules>();
            }

            bool exists = false;
            foreach (var r in this.rules)
            {
                if (r.Id == rule.Id)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                this.rules.Add(rule);
            }
        }


        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Description"":""{1}"", ""Active"":{2}, ""Deletable"":{3}}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var plannedDate = string.Empty;
                if (this.PlannedOn != null)
                {
                    plannedDate = string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", this.PlannedOn);
                }

                var reportEnd = string.Empty;
                if (this.PlannedOn != null)
                {
                    reportEnd = string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", this.ReportEnd);
                }

                var validatedDate = string.Empty;
                if (this.ValidatedOn != null)
                {
                    validatedDate = string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", this.ValidatedOn);
                }

                var providerJson = this.Provider != null ? this.Provider.JsonKeyValue : Constant.JavaScriptNull;
                var customerJson = this.Customer != null ? this.Customer.JsonKeyValue : Constant.JavaScriptNull;

                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""CompanyId"":{2},
                        ""Status"":{3},
                        ""Type"":{4},
                        ""Amount"":{5},
                        ""PlannedOn"":""{6}"",
                        ""ValidatedOn"":""{7}"",
                        ""ReportEnd"":""{8}"",
                        ""Provider"":{9},
                        ""Customer"":{10},
                        ""Rules"":""{11}"",
                        ""Active"":{12}
                    }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.CompanyId,
                    this.Status,
                    this.Type,
                    string.Format(CultureInfo.InvariantCulture, "{0:0.00}", this.Amount),
                    plannedDate,
                    validatedDate,
                    reportEnd,
                    providerJson,
                    customerJson,
                    this.rulesId,
                    this.Active ? "true" : "false");
            }
        }

        /// <summary>Gets a hyper link to questionary profile page</summary>
        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"<a href=""AuditoryView.aspx?id={0}"" title=""{1}"">{1}</a>", this.Id, this.Description);
            }
        }

        public static string JsonList(ReadOnlyCollection<Auditory> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var auditory in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(auditory.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static string FilterList(int companyId, DateTime? from, DateTime? to, bool interna, bool externa, bool provider, bool status0, bool status1, bool status2, bool status3, bool status4, bool status5)
        {
            /* CREATE PROCEDURE Auditory_Filter
             *   @CompanydId int,
             *   @From datetime,
             *   @To datetime,
             *   @TypeInterna bit,
             *   @TypeExterna bit,
             *   @TypeProveedor bit,
             *   @Status0 bit,
             *   @Status1 bit,
             *   @Status2 bit,
             *   @Status3 bit,
             *   @Status4 bit,
             *   @Status5 bit */
            var res = new List<Auditory>();
            var source = string.Format(CultureInfo.InvariantCulture, @"Auditory==>Filter({0}),", companyId);
            using (var cmd = new SqlCommand("Auditory_Filter"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@From", from));
                    cmd.Parameters.Add(DataParameter.Input("@To", to));
                    cmd.Parameters.Add(DataParameter.Input("@TypeInterna", interna));
                    cmd.Parameters.Add(DataParameter.Input("@TypeExterna", externa));
                    cmd.Parameters.Add(DataParameter.Input("@TypeProveedor", provider));
                    cmd.Parameters.Add(DataParameter.Input("@Status0", status0));
                    cmd.Parameters.Add(DataParameter.Input("@Status1", status1));
                    cmd.Parameters.Add(DataParameter.Input("@Status2", status2));
                    cmd.Parameters.Add(DataParameter.Input("@Status3", status3));
                    cmd.Parameters.Add(DataParameter.Input("@Status4", status4));
                    cmd.Parameters.Add(DataParameter.Input("@Status5", status5));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newAuditory = new Auditory
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryFilter.Id),
                                    Description = rdr.GetString(ColumnsAuditoryFilter.Nombre),
                                    Amount = rdr.GetDecimal(ColumnsAuditoryFilter.Amount),
                                    Status = rdr.GetInt32(ColumnsAuditoryFilter.Status)
                                };

                                if (!rdr.IsDBNull(ColumnsAuditoryFilter.PlannedOn))
                                {
                                    newAuditory.PlannedOn = rdr.GetDateTime(ColumnsAuditoryFilter.PlannedOn);
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryFilter.ValidatedOn))
                                {
                                    newAuditory.ValidatedOn = rdr.GetDateTime(ColumnsAuditoryFilter.ValidatedOn);
                                }

                                res.Add(newAuditory);
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

            return JsonList(new ReadOnlyCollection<Auditory>(res));
        }

        /// <summary>Gets a descriptive string with the differences between two questionaries</summary>
        /// <param name="other">Second process to compare</param>
        /// <returns>A descriptive string</returns>
        public string Differences(Auditory other)
        {
            if (this == null || other == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            bool first = true;
            if (this.Description != other.Description)
            {
                res.Append("Description:").Append(other.Description);
                first = false;
            }

            if (this.Scope != other.Scope)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("scope:").Append(other.Scope);
                first = false;
            }

            return res.ToString();
        }

        public static ActionResult Activate(long auditoryId, int companyId, long applicationUserId)
        {
            var res = ActionResult.NoAction;
            return res;
        }

        public static ActionResult Inactivate(long auditoryId, int companyId, long applicationUserId)
        {
            var res = ActionResult.NoAction;
            return res;
        }

        public static Auditory ById(long auditoryId, int companyId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Auditory::>ById({0},{1})", auditoryId, companyId);
            var res = Auditory.Empty;
            using (var cmd = new SqlCommand("Auditory_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Id = rdr.GetInt64(ColumnsAuditoryGet.Id);
                                res.CompanyId = rdr.GetInt32(ColumnsAuditoryGet.CompanyId);
                                res.Description = rdr.GetString(ColumnsAuditoryGet.Nombre);
                                res.Notes = rdr.GetString(ColumnsAuditoryGet.Notes);
                                res.Descripcion = rdr.GetString(ColumnsAuditoryGet.Description);
                                res.Scope = rdr.GetString(ColumnsAuditoryGet.Scope);
                                res.rulesId = rdr.GetString(ColumnsAuditoryGet.NormaId);
                                res.EnterpriseAddress = rdr.GetString(ColumnsAuditoryGet.EnterpriseAddress);
                                res.Status = rdr.GetInt32(ColumnsAuditoryGet.Status);
                                res.Type = rdr.GetInt32(ColumnsAuditoryGet.Type);
                                res.CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsAuditoryGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsAuditoryGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsAuditoryGet.Active);
                                res.Amount = rdr.GetDecimal(ColumnsAuditoryGet.Amount);
                                res.InternalResponsible = new Employee
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryGet.InternalResponsible),
                                    Name = rdr.GetString(ColumnsAuditoryGet.InternalResponsibleName),
                                    LastName = rdr.GetString(ColumnsAuditoryGet.InternalResponsibleLastName)
                                };

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.PlannedOn))
                                {
                                    res.PlannedOn = rdr.GetDateTime(ColumnsAuditoryGet.PlannedOn);
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.PlannedBy))
                                {
                                    res.PlannedBy = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.PlannedBy),
                                        Name = rdr.GetString(ColumnsAuditoryGet.PlannedByName),
                                        LastName = rdr.GetString(ColumnsAuditoryGet.PlannedByLastName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.CompanyAddressId))
                                {
                                    res.CompanyAddressId = rdr.GetInt32(ColumnsAuditoryGet.CompanyAddressId);
                                }

                                res.AuditorTeam = rdr.GetString(ColumnsAuditoryGet.AuditorTeam);

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ProviderId) && rdr.GetInt64(ColumnsAuditoryGet.ProviderId) > 0)
                                {
                                    res.Provider = new Provider
                                    {
                                        Id = rdr.GetInt64(ColumnsAuditoryGet.ProviderId),
                                        Description = rdr.GetString(ColumnsAuditoryGet.ProviderName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.CustomerId) && rdr.GetInt64(ColumnsAuditoryGet.CustomerId) > 0)
                                {
                                    res.Customer = new Customer
                                    {
                                        Id = rdr.GetInt64(ColumnsAuditoryGet.CustomerId),
                                        Description = rdr.GetString(ColumnsAuditoryGet.CustomerName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ClosedBy) && rdr.GetInt32(ColumnsAuditoryGet.ClosedBy) > 0)
                                {
                                    res.ClosedOn = rdr.GetDateTime(ColumnsAuditoryGet.ClosedOn);
                                    res.ClosedBy = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.ClosedBy),
                                        Name = rdr.GetString(ColumnsAuditoryGet.ClosedByName),
                                        LastName = rdr.GetString(ColumnsAuditoryGet.ClosedByLastName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ValidatedBy) && rdr.GetInt32(ColumnsAuditoryGet.ValidatedBy) > 0)
                                {
                                    res.ValidatedOn = rdr.GetDateTime(ColumnsAuditoryGet.ValidatedOn);
                                    res.ValidatedBy = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.ValidatedBy),
                                        Name = rdr.GetString(ColumnsAuditoryGet.ValidatedByName),
                                        LastName = rdr.GetString(ColumnsAuditoryGet.ValidatedByLastName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ValidatedUserBy) && rdr.GetInt32(ColumnsAuditoryGet.ValidatedUserBy) > 0)
                                {
                                    res.ValidatedUserOn = rdr.GetDateTime(ColumnsAuditoryGet.ValidatedUserOn);
                                    res.ValidatedUserBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.ValidatedUserBy),
                                        UserName = rdr.GetString(ColumnsAuditoryGet.ValidatedUserByName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ProviderId) && rdr.GetInt64(ColumnsAuditoryGet.ProviderId) > 0)
                                {
                                    res.Provider = new Provider
                                    {
                                        Id = rdr.GetInt64(ColumnsAuditoryGet.ProviderId),
                                        Description = rdr.GetString(ColumnsAuditoryGet.ProviderName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.CustomerId) && rdr.GetInt64(ColumnsAuditoryGet.CustomerId) > 0)
                                {
                                    res.Customer = new Customer
                                    {
                                        Id = rdr.GetInt64(ColumnsAuditoryGet.CustomerId),
                                        Description = rdr.GetString(ColumnsAuditoryGet.CustomerName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ReportStart))
                                {
                                    res.ReportStart = rdr.GetDateTime(ColumnsAuditoryGet.ReportStart);
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ReportEnd))
                                {
                                    res.ReportEnd = rdr.GetDateTime(ColumnsAuditoryGet.ReportEnd);
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.PreviewDate))
                                {
                                    res.PreviewDate = rdr.GetDateTime(ColumnsAuditoryGet.PreviewDate);
                                }

                                res.GetRules();
                            }
                        }
                    }
                    catch (Exception ex)
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

            return res;
        }

        private void GetRules()
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Auditory::>GetRules({0},{1})", this.Id, this.CompanyId);
            this.rules = new List<Rules>();
            using (var cmd = new SqlCommand("Auditory_GetRules"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Rules", this.rulesId, 200));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                this.rules.Add(new Item.Rules
                                {
                                    Id = rdr.GetInt64(0),
                                    Description = rdr.GetString(1),
                                    Active = rdr.GetBoolean(2)
                                });
                            }
                        }
                    }
                    catch (Exception ex)
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
        }

        /// <summary>Validates auditory</summary>
        /// <param name="auditoryId">Auditory's identifier</param>
        /// <param name="validatedBy">Validator's identifier</param>
        /// <param name="validatedOn">Date of validation</param>
        /// <param name="applicationUserId">Application user identifiers</param>
        /// <param name="companyId">Company's identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Validate(long auditoryId, long validatedBy, DateTime validatedOn, int applicationUserId, int companyId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Auditory::Validate Id:{0} User:{1} Company:{2}", auditoryId, applicationUserId, companyId);
            /* CREATE PROCEDURE Auditory_Validate
             *   @AuditoryId bigint,
             *   @CompanyId int,
             *   @ValidatedBy int,
             *   @ValidatedOn datetime,
             *   @ApplicationUserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Auditory_Validate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ValidatedBy", validatedBy));
                        cmd.Parameters.Add(DataParameter.Input("@ValidatedOn", validatedOn));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ReadOnlyCollection<AuditoryCuestionarioFound> founds = AuditoryCuestionarioFound.ByAuditory(auditoryId, companyId);
                        foreach(var found in founds.Where(f=>f.Action == true))
                        {
                            IncidentAction.FromFound(found, Convert.ToInt32(validatedBy), applicationUserId, companyId);
                        }

                        ReadOnlyCollection<AuditoryCuestionarioImprovement> improvements = AuditoryCuestionarioImprovement.ByAuditory(auditoryId, companyId);
                        foreach (var improvement in improvements.Where(i=>i.Action == true))
                        {
                            IncidentAction.FromImprovement(improvement, Convert.ToInt32(validatedBy), applicationUserId, companyId);
                        }

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

        /// <summary>Validates auditory</summary>
        /// <param name="auditoryId">Auditory's identifier</param>
        /// <param name="validatedBy">Validator's identifier</param>
        /// <param name="validatedOn">Date of validation</param>
        /// <param name="applicationUserId">Application user identifiers</param>
        /// <param name="companyId">Company's identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Close(long auditoryId, long closedBy, DateTime closedOn, int applicationUserId, int companyId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Auditory::Close Id:{0} User:{1} Company:{2}", auditoryId, applicationUserId, companyId);
            /* CREATE PROCEDURE Auditory_Close
             *   @AuditoryId bigint,
             *   @CompanyId int,
             *   @ClosedBy int,
             *   @ClosedOn datetime,
             *   @ApplicationUserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Auditory_Close"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ClosedBy", closedBy));
                        cmd.Parameters.Add(DataParameter.Input("@ClosedOn", closedOn));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();

                        var auditory = Auditory.ById(auditoryId, companyId);
                        if(auditory.Type == 1)
                        {
                            var zombies = IncidentActionZombie.ByAuditoryId(auditoryId, companyId);
                            if(zombies != null && zombies.Count > 0)
                            {
                                foreach(var zombie in zombies)
                                {
                                    zombie.ConvertToIncidentAction(applicationUserId, auditory.Provider.Id, auditory.Customer.Id);
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



        /// <summary>Reopen auditory</summary>
        /// <param name="auditoryId">Auditory's identifier</param>
        /// <param name="applicationUserId">Application user identifiers</param>
        /// <param name="companyId">Company's identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Reopen(long auditoryId, int applicationUserId, int companyId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Auditory::Reopen Id:{0} User:{1} Company:{2}", auditoryId, applicationUserId, companyId);
            /* CREATE PROCEDURE Auditory_Reopen
             *   @AuditoryId bigint,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Auditory_Reopen"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();

                        var auditory = Auditory.ById(auditoryId, companyId);
                        if (auditory.Type == 1)
                        {
                            var zombies = IncidentActionZombie.ByAuditoryId(auditoryId, companyId);
                            if (zombies != null && zombies.Count > 0)
                            {
                                foreach (var zombie in zombies)
                                {
                                    zombie.ConvertToIncidentAction(applicationUserId, auditory.Provider.Id, auditory.Customer.Id);
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

        public static ActionResult Inactivate(long auditoryId, int companyId, int applicationUserId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Auditory::Inactivate Id:{0} User:{1} Company:{2}", auditoryId, applicationUserId, companyId);
            /* CREATE PROCEDURE Auditory_Inactivate
             *   @AuditoryId bigint,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Auditory_Inactivate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
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

        public static ActionResult Activate(long auditoryId, int companyId, int applicationUserId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Auditory::Activate Id:{0} User:{1} Company:{2}", auditoryId, applicationUserId, companyId);
            /* CREATE PROCEDURE Auditory_Activate
             *   @AuditoryId bigint,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Auditory_Activate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
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

        public ActionResult Insert(int applicationUserId, int companyId)
        {
            CalculateRules();
            var source = string.Format(CultureInfo.InvariantCulture, "Auditory::>Insert({0})", this.Id);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Auditory_Insert
             *   @AuditoryId bigint output,
             *   @CompanyId int,
             *   @Type int,
             *   @CustomerId bigint,
             *   @ProviderId bigint,
             *   @Nombre nvarchar(150),
             *   @NormaId nvarchar(200),
             *   @Amount decimal(18,3),
             *   @InternalResponsible int,
             *   @Description nvarchar(2000),
             *   @Scope nvarchar(150),
             *   @CompanyAddressId int,
             *   @EnterpriseAddress nvarchar(500),
             *   @Notes nvarchar(2000),
             *   @AuditorTeam nvarchar(500),
             *   @PlannedBy int,
             *   @PlannedOn datetime,
             *   @ClosedBy int,
             *   @ClosedOn datetime,
             *   @ValidatedBy int,
             *   @ValidatedOn datetime,
             *   @ValidatedUserBy int,
             *   @ValidatedUserOn datetime,
             *   @Status int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Auditory_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@AuditoryId"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Type", this.Type));
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Customer.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id));
                    cmd.Parameters.Add(DataParameter.Input("@PreviewDate", this.PreviewDate));
                    cmd.Parameters.Add(DataParameter.Input("@Nombre", this.Description, 150));
                    cmd.Parameters.Add(DataParameter.Input("@NormaId", this.rulesId, 200));
                    cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                    cmd.Parameters.Add(DataParameter.Input("@InternalResponsible", this.InternalResponsible.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Descripcion, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@Scope", this.Scope, 150));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyAddressId", this.CompanyAddressId));
                    cmd.Parameters.Add(DataParameter.Input("@EnterpriseAddress", this.EnterpriseAddress, 500));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@AuditorTeam", this.AuditorTeam, 500));
                    cmd.Parameters.Add(DataParameter.Input("@PlannedBy", this.PlannedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@PlannedOn", this.PlannedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedBy", this.ClosedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedOn", this.ClosedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedBy", this.ValidatedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedOn", this.ValidatedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedUserBy", this.ValidatedUserBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedUserOn", this.ValidatedUserOn));
                    cmd.Parameters.Add(DataParameter.Input("@Status", 0));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@AuditoryId"].Value);
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

        public ActionResult Update(int applicationUserId, int companyId, string differences)
        {
            CalculateRules();
            var source = string.Format(CultureInfo.InvariantCulture, "Auditory::>Update({0})", this.Id);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Auditory_Update
             *   @AuditoryId bigint output,
             *   @CompanyId int,
             *   @Type int,
             *   @CustomerId bigint,
             *   @ProviderId bigint,
             *   @Nombre nvarchar(150),
             *   @NormaId nvarchar(200),
             *   @Amount decimal(18,3),
             *   @InternalResponsible int,
             *   @Description nvarchar(2000),
             *   @Scope nvarchar(150),
             *   @CompanyAddressId int,
             *   @EnterpriseAddress nvarchar(500),
             *   @Notes nvarchar(2000),
             *   @AuditorTeam nvarchar(500),
             *   @PlannedBy int,
             *   @PlannedOn datetime,
             *   @ClosedBy int,
             *   @ClosedOn datetime,
             *   @ValidatedBy int,
             *   @ValidatedOn datetime,
             *   @ValidatedUserBy int,
             *   @ValidatedUserOn datetime,
             *   @Status int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Auditory_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Type", this.Type));
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Customer.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider.Id));
                    cmd.Parameters.Add(DataParameter.Input("@PreviewDate", this.PreviewDate));
                    cmd.Parameters.Add(DataParameter.Input("@Nombre", this.Description, 150));
                    cmd.Parameters.Add(DataParameter.Input("@NormaId", this.rulesId, 200));
                    cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                    cmd.Parameters.Add(DataParameter.Input("@InternalResponsible", this.InternalResponsible.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Descripcion, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@Scope", this.Scope, 150));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyAddressId", this.CompanyAddressId));
                    cmd.Parameters.Add(DataParameter.Input("@EnterpriseAddress", this.EnterpriseAddress, 500));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@AuditorTeam", this.AuditorTeam, 500));
                    cmd.Parameters.Add(DataParameter.Input("@PlannedBy", this.PlannedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@PlannedOn", this.PlannedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedBy", this.ClosedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ClosedOn", this.ClosedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedBy", this.ValidatedBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedOn", this.ValidatedOn));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedUserBy", this.ValidatedUserBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ValidatedUserOn", this.ValidatedUserOn));
                    cmd.Parameters.Add(DataParameter.Input("@ReportEnd", this.ReportEnd));
                    cmd.Parameters.Add(DataParameter.Input("@Status", this.Status));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@AuditoryId"].Value);
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

        public static ActionResult SetQuestionaries(long auditoryId, int applicationUserId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Auditory::>SetQuestionaries({0},{1}", auditoryId, applicationUserId);
            /* CREATE PROCEDURE Auditory_SetQuestionaries
             *   @AuditoryId bigint,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            var res = ActionResult.NoAction; using (var cmd = new SqlCommand("Auditory_SetQuestionaries"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        private void CalculateRules()
        {
            var res = "";
            if(this.rules != null)
            {
                foreach(var rule in this.rules)
                {
                    res += string.Format(CultureInfo.InvariantCulture, "{0}|", rule.Id);
                }
            }

            this.rulesId = res;
        }
    }
}