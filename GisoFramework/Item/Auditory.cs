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
    using System.Text;
    using System.Web;
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
        public Employee PlannedBy { get; set; }
        public DateTime? PlannedOn { get; set; }
        public Employee ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public ApplicationUser ValidatedBy { get; set; }
        public DateTime? ValidatedOn { get; set; }
        public ApplicationUser ValidatedUserBy { get; set; }
        public DateTime? ValidatedUserOn { get; set; }
        public string Scope { get; set; }
        public string EnterpriseAddress { get; set; }
        public string AuditorTeam { get; set; }

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
                    EnterpriseAddress = string.Empty,
                    AuditorTeam = string.Empty,
                    PlannedBy = Employee.EmptySimple,
                    ClosedBy = Employee.EmptySimple,
                    ValidatedBy = ApplicationUser.Empty,
                    ValidatedUserBy = ApplicationUser.Empty,
                    Customer = Customer.Empty,
                    Provider = Provider.Empty,
                    rules = new List<Rules>(),
                    InternalResponsible = Employee.Empty,
                    Status = Constant.DefaultId,
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

                var validatedDate = string.Empty;
                if (this.ValidatedOn != null)
                {
                    validatedDate = string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", this.ValidatedOn);
                }

                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""CompanyId"":{2},
                        ""Status"":{3},
                        ""Type"":{4},
                        ""Amount"":{5},
                        ""PlannedOn"":""{6}"",
                        ""ValidatedOn"":""{7}"",
                        ""Active"":{8}
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

        public ActionResult Insert(long applicationUserId)
        {
            var res = ActionResult.NoAction;
            return res;
        }

        public ActionResult Update(long applicationUserId, string differences)
        {
            var res = ActionResult.NoAction;
            return res;
        }

        public static Auditory ById(long auditoryId, int companyId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Auditory::>ById({0},{1})", auditoryId, companyId);
            var res = Auditory.Empty;
            using (var cmd = new SqlCommand("Auditory_ById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", auditoryId));
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
                                res.PlannedOn = rdr.GetDateTime(ColumnsAuditoryGet.PlannedOn);
                                res.AuditorTeam = rdr.GetString(ColumnsAuditoryGet.AuditorTime);

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.CustomerId) && rdr.GetInt32(ColumnsAuditoryGet.ProviderId) > 0)
                                {
                                    res.Provider = new Provider
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.ProviderId),
                                        Description = rdr.GetString(ColumnsAuditoryGet.ProviderName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.CustomerId) && rdr.GetInt32(ColumnsAuditoryGet.CustomerId) > 0)
                                {
                                    res.Customer = new Customer
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.CustomerId),
                                        Description = rdr.GetString(ColumnsAuditoryGet.CustomerName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ClosedBy) && rdr.GetInt32(ColumnsAuditoryGet.ClosedBy) > 0)
                                {
                                    res.ClosedOn = rdr.GetDateTime(ColumnsAuditoryGet.ClosedOn);
                                    res.ClosedBy = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.ClosedBy),
                                        Name = rdr.GetString(ColumnsAuditoryGet.CloseName),
                                        LastName = rdr.GetString(ColumnsAuditoryGet.CloseLastName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ValidatedBy) && rdr.GetInt32(ColumnsAuditoryGet.ValidatedBy) > 0)
                                {
                                    res.ValidatedOn = rdr.GetDateTime(ColumnsAuditoryGet.ValidatedOn);
                                    res.ValidatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.ValidatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryGet.ValidatedName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsAuditoryGet.ValidatedUserBy) && rdr.GetInt32(ColumnsAuditoryGet.ValidatedUserBy) > 0)
                                {
                                    res.ValidatedUserOn = rdr.GetDateTime(ColumnsAuditoryGet.ValidatedUserOn);
                                    res.ValidatedUserBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryGet.ValidatedUserBy),
                                        UserName = rdr.GetString(ColumnsAuditoryGet.ValidatedUserName)
                                    };
                                }

                                if(!rdr.IsDBNull(ColumnsAuditoryGet.ProviderId) && rdr.GetInt64(ColumnsAuditoryGet.ProviderId) > 0)
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
    }
}