// --------------------------------
// <copyright file="Objetivo.cs" company="OpenFramework">
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

    public class Objetivo : BaseItem
    {
        [DifferenciableAttribute]
        public string Name { get; set; }

        [DifferenciableAttribute]
        public string Methodology { get; set; }

        [DifferenciableAttribute]
        public string Resources { get; set; }

        [DifferenciableAttribute]
        public string Notes { get; set; }

        [DifferenciableAttribute]
        public string MetaComparer { get; set; }

        [DifferenciableAttribute]
        public decimal? Meta { get; set; }

        [DifferenciableAttribute]
        public int RevisionId { get; set; }

        [DifferenciableAttribute]
        public bool VinculatedToIndicator { get; set; }

        [DifferenciableAttribute]
        public int? IndicatorId { get; set; }

        [DifferenciableAttribute]
        public DateTime StartDate { get; set; }

        [DifferenciableAttribute]
        public DateTime PreviewEndDate { get; set; }

        [DifferenciableAttribute]
        public DateTime? EndDate { get; set; }

        [DifferenciableAttribute]
        public Employee Responsible { get; set; }

        [DifferenciableAttribute]
        public Employee EndResponsible { get; set; }

        [DifferenciableAttribute]
        public string EndReason { get; set; }

        public static Objetivo Empty
        {
            get
            {
                return new Objetivo
                {
                    Id = 0,
                    Name = string.Empty,
                    Description = string.Empty,
                    Methodology = string.Empty,
                    Resources = string.Empty,
                    Notes = string.Empty,
                    VinculatedToIndicator = false,
                    StartDate = DateTime.Now,
                    PreviewEndDate = DateTime.Now,
                    Responsible = Employee.Empty,
                    EndResponsible = Employee.Empty,
                    EndReason = string.Empty,
                    RevisionId = 0,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"<a href=""ObjetivoView.aspx?id={0}"">{1}</a>", this.Id, this.Name);
            }
        }

        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Name"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Name));
            }
        }

        public override string Json
        {
            get
            {
                string metaComparer = "null";
                if (!string.IsNullOrEmpty(this.MetaComparer))
                {
                    metaComparer = string.Format(CultureInfo.InvariantCulture, @"""{0}""", this.MetaComparer);
                }

                string meta = "null";
                if (this.Meta.HasValue)
                {
                    meta = string.Format(CultureInfo.InvariantCulture, "{0:#0.0#########}", this.Meta).Replace(',','.');
                    if (meta.EndsWith(".0", StringComparison.OrdinalIgnoreCase))
                    {
                        meta = meta.Substring(0, meta.Length - 2);
                    }
                }

                string endResponsibleValue = "null";
                if (this.EndResponsible != null)
                {
                    endResponsibleValue = this.EndResponsible.JsonKeyValue;
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                    ""CompanyId"":{1},
                    ""Name"":""{2}"",
                    ""Description"":""{3}"",
                    ""Methodology"":""{4}"",
                    ""Resources"":""{5}"",
                    ""Notes"":""{6}"",
                    ""VinculatedToIndicator"":{7},
                    ""IndicatorId"":{8},
                    ""Responsible"":{9},
                    ""EndResponsible"":{10},
                    ""StartDate"":""{11:dd/MM/yyyy}"",
                    ""PreviewEndDate"":""{12:dd/MM/yyyy}"",
                    ""EndDate"":{13},
                    ""EndReason"":""{22}"",
                    ""RevisionId"":{14},
                    ""MetaComparer"": {15},
                    ""Meta"": {16},
                    ""CreatedBy"":{17},
                    ""CreatedOn"":""{18:dd/MM/yyyy}"",
                    ""ModifiedBy"":{19},
                    ""ModifiedOn"":""{20:dd/MM/yyyy}"",
                    ""Active"":{21}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.JsonCompliant(this.Name),
                    Tools.JsonCompliant(this.Description),
                    Tools.JsonCompliant(this.Methodology),
                    Tools.JsonCompliant(this.Resources),
                    Tools.JsonCompliant(this.Notes),
                    this.VinculatedToIndicator ? "true":"false",
                    this.IndicatorId.HasValue ? this.IndicatorId.Value.ToString() : "null",
                    this.Responsible.JsonKeyValue,
                    endResponsibleValue,
                    this.StartDate,
                    this.PreviewEndDate,
                    this.EndDate.HasValue ? string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", this.EndDate.Value) : "null",
                    this.RevisionId,
                    metaComparer,
                    meta,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? "true":"false",
                    Tools.JsonCompliant(this.EndReason));
            }
        }

        public static string FilterList(int companyId, DateTime? from, DateTime? to, int status)
        {
            var items = Filter(companyId, from, to, status);
            HttpContext.Current.Session["ObjetivoRecords"] = items;
            var res = new StringBuilder("[");
            bool first = true;
            foreach (ObjetivoFilterItem item in items)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(", ");
                }

                res.Append("{");
                res.Append(Tools.JsonPair("Id", item.Objetivo.Id)).Append(", ");
                res.Append(Tools.JsonPair("Name", item.Objetivo.Name)).Append(", ");
                res.Append(Tools.JsonPair("ResponsibleId", item.Objetivo.Responsible.Id)).Append(", ");
                res.Append(Tools.JsonPair("ResponsibleName", item.Objetivo.Responsible.FullName)).Append(", ");
                res.Append(Tools.JsonPair("StartDate", item.Objetivo.StartDate)).Append(", ");
                res.Append(Tools.JsonPair("EndDate", item.Objetivo.EndDate)).Append(", ");
                res.Append(Tools.JsonPair("PreviewEndDate", item.Objetivo.PreviewEndDate));
                res.Append("}");
            }

            res.Append("]");
            return res.ToString();
        }

        public static ActionResult Restore(int objetivoId, int companyId, int applicationUserId)
        {
            string source = string.Format(
               CultureInfo.InvariantCulture,
               @"Objetivo::Restore({0}, {1})",
               objetivoId,
               applicationUserId);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Objetivo_Restore]
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Objetivo_Restore"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(objetivoId);
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

        public static ActionResult Anulate(int objetivoId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Objetivo::Anulate({0}, {1})",
                objetivoId,
                applicationUserId);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Objetivo_Anulate]
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @EndDate datetime,
             *   @EndReason nvarchar(500),
             *   @EndResponsable int,
             *   @UnidadId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Objetivo_Anulate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", date));
                        cmd.Parameters.Add(DataParameter.Input("@EndReason", reason, 500));
                        cmd.Parameters.Add(DataParameter.Input("@EndResponsible", responsible));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(objetivoId);
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

        public static ReadOnlyCollection<ObjetivoFilterItem> Filter(int companyId, DateTime? from, DateTime? to, int status)
        {
            /* CREATE PROCEDURE Objetivo_Filter
             *   @CompanyId int,
             *   @DateFrom datetime,
             *   @DateTo datetime
             *   @Closed bit */
            var res = new List<ObjetivoFilterItem>();
            using (var cmd = new SqlCommand("Objetivo_Filter"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@DateFrom", from));
                    cmd.Parameters.Add(DataParameter.Input("@DateTo", to));
                    cmd.Parameters.Add(DataParameter.Input("@Status", status));

                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var item = new ObjetivoFilterItem
                            {
                                Objetivo = new Objetivo
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoFilter.ObjetivoId),
                                    Name = rdr.GetString(ColumnsObjetivoFilter.Name),
                                    StartDate = rdr.GetDateTime(ColumnsObjetivoFilter.StartDate),
                                    PreviewEndDate = rdr.GetDateTime(ColumnsObjetivoFilter.PreviewEndDate),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsObjetivoFilter.ResponsibleId),
                                        Name = rdr.GetString(ColumnsObjetivoFilter.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsObjetivoFilter.ResponsibleLastname)
                                    },
                                    Active = rdr.GetBoolean(ColumnsObjetivoFilter.Active)
                                }
                            };

                            if (!rdr.IsDBNull(ColumnsObjetivoFilter.PreviewEndDate))
                            {
                                item.Objetivo.PreviewEndDate = rdr.GetDateTime(ColumnsObjetivoFilter.PreviewEndDate);
                            }

                            if (!rdr.IsDBNull(ColumnsObjetivoFilter.EndDate))
                            {
                                item.Objetivo.EndDate = rdr.GetDateTime(ColumnsObjetivoFilter.EndDate);
                            }

                            res.Add(item);
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

                HttpContext.Current.Session["ObjetivoFilterData"] = res;
                return new ReadOnlyCollection<ObjetivoFilterItem>(res);
            }
        }

        public static ReadOnlyCollection<Objetivo> GetAvailable(int companyId)
        {
            /* CREATE PROCEDURE Objetivo_GetAvailable
             *   @CompanyId int */
            var res = new List<Objetivo>();
            using (var cmd = new SqlCommand("Objetivo_GetAvailable"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var objetivo = new Objetivo
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.Id),
                                    Name = rdr.GetString(ColumnsObjetivoGet.Name),
                                    Description = rdr.GetString(ColumnsObjetivoGet.Description),
                                    CompanyId = companyId,
                                    StartDate = rdr.GetDateTime(ColumnsObjetivoGet.StartDate),
                                    Methodology = rdr.GetString(ColumnsObjetivoGet.Methodology),
                                    Notes = rdr.GetString(ColumnsObjetivoGet.Notes),
                                    Resources = rdr.GetString(ColumnsObjetivoGet.Resources),
                                    PreviewEndDate = rdr.GetDateTime(ColumnsObjetivoGet.PreviewEndDate),
                                    VinculatedToIndicator = rdr.GetBoolean(ColumnsObjetivoGet.VinculatedToIndicator),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleLastName)
                                    },
                                    RevisionId = rdr.GetInt32(ColumnsObjetivoGet.RevisionId),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsObjetivoGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsObjetivoGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsObjetivoGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsObjetivoGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsObjetivoGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsObjetivoGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsObjetivoGet.Active),
                                    EndReason = rdr.GetString(ColumnsObjetivoGet.EndReason)
                                };

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.IndicatorId))
                                {
                                    objetivo.IndicatorId = rdr.GetInt32(ColumnsObjetivoGet.IndicatorId);
                                }

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.EndDate))
                                {
                                    objetivo.EndDate = rdr.GetDateTime(ColumnsObjetivoGet.EndDate);
                                }

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.ResponsibleClose))
                                {
                                    objetivo.EndResponsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleClose),
                                        Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseName),
                                        LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseLastName)
                                    };
                                }

                                res.Add(objetivo);
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

            return new ReadOnlyCollection<Objetivo>(res);
        }

        public static ReadOnlyCollection<Objetivo> ByCompany(Company company)
        {
            return ByCompany(company.Id);
        }

        public static ReadOnlyCollection<Objetivo> ByCompany(int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            var res = new List<Objetivo>();
            using (var cmd = new SqlCommand("Objetivo_GetAll"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var objetivo = new Objetivo
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoGet.Id),
                                Name = rdr.GetString(ColumnsObjetivoGet.Name),
                                Description = rdr.GetString(ColumnsObjetivoGet.Description),
                                CompanyId = companyId,
                                StartDate = rdr.GetDateTime(ColumnsObjetivoGet.StartDate),
                                Methodology = rdr.GetString(ColumnsObjetivoGet.Methodology),
                                Notes = rdr.GetString(ColumnsObjetivoGet.Notes),
                                Resources = rdr.GetString(ColumnsObjetivoGet.Resources),
                                PreviewEndDate = rdr.GetDateTime(ColumnsObjetivoGet.PreviewEndDate),
                                VinculatedToIndicator = rdr.GetBoolean(ColumnsObjetivoGet.VinculatedToIndicator),
                                Responsible = new Employee
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleId),
                                    Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleName),
                                    LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleLastName)
                                },
                                RevisionId = rdr.GetInt32(ColumnsObjetivoGet.RevisionId),
                                CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsObjetivoGet.CreatedByName)
                                },
                                CreatedOn = rdr.GetDateTime(ColumnsObjetivoGet.CreatedOn),
                                ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsObjetivoGet.ModifiedByName)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsObjetivoGet.ModifiedOn),
                                Active = rdr.GetBoolean(ColumnsObjetivoGet.Active),
                                EndReason = rdr.GetString(ColumnsObjetivoGet.EndReason)
                            };

                            if (!rdr.IsDBNull(ColumnsObjetivoGet.IndicatorId))
                            {
                                objetivo.IndicatorId = rdr.GetInt32(ColumnsObjetivoGet.IndicatorId);
                            }

                            if (!rdr.IsDBNull(ColumnsObjetivoGet.EndDate))
                            {
                                objetivo.EndDate = rdr.GetDateTime(ColumnsObjetivoGet.EndDate);
                            }

                            if (!rdr.IsDBNull(ColumnsObjetivoGet.ResponsibleClose))
                            {
                                objetivo.EndResponsible = new Employee
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleClose),
                                    Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseName),
                                    LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseLastName)
                                };
                            }

                            res.Add(objetivo);
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

            return new ReadOnlyCollection<Objetivo>(res);
        }

        public static ReadOnlyCollection<Objetivo> GetActive(int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            var res = new List<Objetivo>();
            using (var cmd = new SqlCommand("Objetivo_GetActive"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var objetivo = new Objetivo
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoGet.Id),
                                Name = rdr.GetString(ColumnsObjetivoGet.Name),
                                Description = rdr.GetString(ColumnsObjetivoGet.Description),
                                CompanyId = companyId,
                                StartDate = rdr.GetDateTime(ColumnsObjetivoGet.StartDate),
                                Methodology = rdr.GetString(ColumnsObjetivoGet.Methodology),
                                Notes = rdr.GetString(ColumnsObjetivoGet.Notes),
                                Resources = rdr.GetString(ColumnsObjetivoGet.Resources),
                                PreviewEndDate = rdr.GetDateTime(ColumnsObjetivoGet.PreviewEndDate),
                                VinculatedToIndicator = rdr.GetBoolean(ColumnsObjetivoGet.VinculatedToIndicator),
                                Responsible = new Employee
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleId),
                                    Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleName),
                                    LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleLastName)
                                },
                                RevisionId = rdr.GetInt32(ColumnsObjetivoGet.RevisionId),
                                CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsObjetivoGet.CreatedByName)
                                },
                                CreatedOn = rdr.GetDateTime(ColumnsObjetivoGet.CreatedOn),
                                ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsObjetivoGet.ModifiedByName)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsObjetivoGet.ModifiedOn),
                                Active = rdr.GetBoolean(ColumnsObjetivoGet.Active),
                                EndReason = rdr.GetString(ColumnsObjetivoGet.EndReason)
                            };

                            if (!rdr.IsDBNull(ColumnsObjetivoGet.IndicatorId))
                            {
                                objetivo.IndicatorId = rdr.GetInt32(ColumnsObjetivoGet.IndicatorId);
                            }

                            if (!rdr.IsDBNull(ColumnsObjetivoGet.EndDate))
                            {
                                objetivo.EndDate = rdr.GetDateTime(ColumnsObjetivoGet.EndDate);
                            }

                            if (!rdr.IsDBNull(ColumnsObjetivoGet.ResponsibleClose))
                            {
                                objetivo.EndResponsible = new Employee
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleClose),
                                    Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseName),
                                    LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseLastName)
                                };
                            }

                            res.Add(objetivo);
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

            return new ReadOnlyCollection<Objetivo>(res);
        }

        public static string ByCompanyJsonList(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var objetivo in ByCompany(companyId))
            {
                if (objetivo.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(objetivo.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objetivoId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static Objetivo ById(long objetivoId, int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            var res = Objetivo.Empty;
            using (var cmd = new SqlCommand("Objetivo_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res.Id = rdr.GetInt32(ColumnsObjetivoGet.Id);
                                res.Name = rdr.GetString(ColumnsObjetivoGet.Name);
                                res.Description = rdr.GetString(ColumnsObjetivoGet.Description);
                                res.CompanyId = companyId;
                                res.StartDate = rdr.GetDateTime(ColumnsObjetivoGet.StartDate);
                                res.Methodology = rdr.GetString(ColumnsObjetivoGet.Methodology);
                                res.Notes = rdr.GetString(ColumnsObjetivoGet.Notes);
                                res.Resources = rdr.GetString(ColumnsObjetivoGet.Resources);
                                res.PreviewEndDate = rdr.GetDateTime(ColumnsObjetivoGet.PreviewEndDate);
                                res.VinculatedToIndicator = rdr.GetBoolean(ColumnsObjetivoGet.VinculatedToIndicator);
                                res.Responsible = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleId),
                                    Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleName),
                                    LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleLastName)
                                };
                                res.RevisionId = rdr.GetInt32(ColumnsObjetivoGet.RevisionId);
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsObjetivoGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsObjetivoGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsObjetivoGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsObjetivoGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsObjetivoGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsObjetivoGet.Active);
                                res.EndReason = rdr.GetString(ColumnsObjetivoGet.EndReason);

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.IndicatorId))
                                {
                                    res.IndicatorId = rdr.GetInt32(ColumnsObjetivoGet.IndicatorId);
                                }

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.EndDate))
                                {
                                    res.EndDate = rdr.GetDateTime(ColumnsObjetivoGet.EndDate);
                                }

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.ResponsibleClose))
                                {
                                    res.EndResponsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsObjetivoGet.ResponsibleClose),
                                        Name = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseName),
                                        LastName = rdr.GetString(ColumnsObjetivoGet.ResponsibleCloseLastName)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.MetaComparer))
                                {
                                    res.MetaComparer = rdr.GetString(ColumnsObjetivoGet.MetaComparer);
                                }

                                if (!rdr.IsDBNull(ColumnsObjetivoGet.Meta))
                                {
                                    res.Meta = rdr.GetDecimal(ColumnsObjetivoGet.Meta);
                                }
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

        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Objetivo);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Objetivo);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "ObjetivoDelete({0},'{1}');", this.Id, this.Name);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>",
                    deleteFunction,
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Description)),
                    Tools.JsonCompliant(dictionary["Common_Delete"]));

            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='ObjetivoView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='ObjetivoView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            string pattenr = @"<tr><td>{0}</td><td style=""width:250px;"">{1}</td><td align=""center"" style=""width:90px;"">{4:dd/MM/yyyy}</td><td align=""center"" style=""width:90px;"">{5:dd/MM/yyyy}</td><td style=""width:90px;"">{2}&nbsp;{3}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattenr,
                this.Link,
                this.Responsible.FullName,
                iconEdit,
                iconDelete,
                this.StartDate,
                this.PreviewEndDate);
        }

        public static ActionResult Activate(int objetivoId, int compnayId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Objetivo::Activate({0}, {1})",
                objetivoId,
                applicationUserId);
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Objetivo_Activate]
             *   @ObjetivoRegsitroId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Objetivo_Activate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", compnayId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(objetivoId);
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

        public static ActionResult Inactivate(int objetivoId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Objetivo::Inactivate({0}, {1})",
                objetivoId,
                applicationUserId);
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Objetivo_Inactivate]
             *   @ObjetivoRegsitroId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Objetivo_Inactivate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(objetivoId);

                        var actions = IncidentAction.ByObjetivoId(objetivoId, companyId);
                        if(actions != null)
                        {
                            foreach(var action in actions)
                            {
                                action.CompanyId = companyId;
                                action.Delete(applicationUserId);
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

        private ActionResult Insert(int applicationUserId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Objetivo::Objetivo_Insert({0}, {1})", this.Id, applicationUserId);
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Objetivo_Insert]
             *   @ObjetivoId int output,
             *   @Name nvarchar(100),
             *   @Description nvarchar(2000),
             *   @ResponsibleId int,
             *   @StartDate datetime,
             *   @VinculatedToIndicator bit,
             *   @IndicatorId int,
             *   @RevisionId int,
             *   @Methodology nvarchar(2000),
             *   @Resources nvarchar(2000),
             *   @Notes nvarchar(2000),
             *   @PreviewEndDate datetime,
             *   @EndDate datetime,
             *   @ResponsibleClose int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Objetivo_Insert"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputInt("@ObjetivoId"));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleId", this.Responsible));
                        cmd.Parameters.Add(DataParameter.Input("@StartDate", this.StartDate));
                        cmd.Parameters.Add(DataParameter.Input("@VinculatedToIndicator", this.VinculatedToIndicator));
                        cmd.Parameters.Add(DataParameter.Input("@IndicatorId", this.IndicatorId));
                        cmd.Parameters.Add(DataParameter.Input("@RevisionId", this.RevisionId));
                        cmd.Parameters.Add(DataParameter.Input("@Methodology", this.Methodology, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Resources", this.Resources, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@PreviewendDate", this.PreviewEndDate));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", this.EndDate));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleClose", this.EndResponsible));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@MetaComparer", this.MetaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Meta", this.Meta));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@ObjetivoId"].Value.ToString());
                        res.SetSuccess(this.Id);
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

        private ActionResult Update(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Objetivo::Objetivo_Update({0}, {1})",
                this.Id,
                applicationUserId);
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Objetivo_Update]
             *   @ObjetivoId int,
             *   @Name nvarchar(100),
             *   @Description nvarchar(2000),
             *   @ResponsibleId int,
             *   @StartDate datetime,
             *   @VinculatedToIndicator bit,
             *   @IndicatorId int,
             *   @RevisionId int,
             *   @Methodology nvarchar(2000),
             *   @Resources nvarchar(2000),
             *   @Notes nvarchar(2000),
             *   @PreviewEndDate datetime,
             *   @EndDate datetime,
             *   @ResponsibleClose int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Objetivo_Update"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@StartDate", this.StartDate));
                        cmd.Parameters.Add(DataParameter.Input("@VinculatedToIndicator", this.VinculatedToIndicator));
                        cmd.Parameters.Add(DataParameter.Input("@IndicatorId", this.IndicatorId));
                        cmd.Parameters.Add(DataParameter.Input("@RevisionId", this.RevisionId));
                        cmd.Parameters.Add(DataParameter.Input("@Methodology", this.Methodology, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Resources", this.Resources, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@PreviewEndDate", this.PreviewEndDate));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", this.EndDate));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleClose", this.EndResponsible));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@MetaComparer", this.MetaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Meta", this.Meta));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@ObjetivoId"].Value.ToString());
                        res.SetSuccess(this.Id);
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

        public ActionResult Save(int applicationUserId)
        {
            if (this.Id > 0)
            {
                return this.Update(applicationUserId);
            }

            return this.Insert(applicationUserId);
        }
    }
}