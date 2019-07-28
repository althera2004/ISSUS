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
    using System.Linq;
    using System.Text;

    public class AuditoryCuestionarioObservations : BaseItem
    {
        public long AuditoryId { get; set; }
        public long CuestionarioId { get; set; }
        public string Text { get; set; }
        public bool Action { get; set; }

        public static AuditoryCuestionarioObservations Empty
        {
            get
            {
                return new AuditoryCuestionarioObservations
                {
                    Id = Constant.DefaultLongId,
                    CompanyId = Constant.DefaultId,
                    AuditoryId = Constant.DefaultLongId,
                    CuestionarioId = Constant.DefaultLongId,
                    Text = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Action = false,
                    Active = false
                };
            }
        }

        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
                    {{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""AuditoryId"":{2},
                        ""CuestionarioId"":{3},
                        ""Text"":""{4}"",
                        ""CreatedBy"":{5},
                        ""CreatedOn"":""{6:dd/MM/yyyy}"",
                        ""ModifiedBy"":{7},
                        ""ModifiedOn"":""{8:dd/MM/yyyy}"",
                        ""Active"":{9}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.AuditoryId,
                    this.CuestionarioId,
                    Tools.JsonCompliant(this.Text),
                    this.CreatedBy.Json,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Action ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.Active ? Constant.JavaScriptTrue : Constant.JavaScriptFalse);
            }
        }

        public override string JsonKeyValue => Json;

        public static string JsonList(ReadOnlyCollection<AuditoryCuestionarioObservations> list)
        {
            var res = new StringBuilder("[");
            if (list != null)
            {
                bool first = true;
                foreach (var observation in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(observation.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static AuditoryCuestionarioObservations ById(long cuestionarioId, long auditoryId, int companyId)
        {
            var res = AuditoryCuestionarioObservations.Empty;
            using (var cmd = new SqlCommand("AuditoryCuestionarioObservations_ById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@CuestionarioId", cuestionarioId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Id = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.Id);
                                res.CompanyId = Convert.ToInt32(rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.CompanyId));
                                res.AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.AuditoryId);
                                res.CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.CuestionarioId);
                                res.Text = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.Text);
                                res.CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryCuestionarioObservationsGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioObservationsGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryCuestionarioObservationsGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioObservationsGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioObservationsGet.Active);
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State == ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        public static ReadOnlyCollection<AuditoryCuestionarioObservations> ByCuestionary(long cuestionarioId, long auditoryId, int companyId)
        {
            var res = new List<AuditoryCuestionarioObservations>();
            using (var cmd = new SqlCommand("AuditoryCuestionarioObservations_ByCuestionarioId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@CuestionarioId", cuestionarioId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new AuditoryCuestionarioObservations
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.Id),
                                    CompanyId = Convert.ToInt32(rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.CompanyId)),
                                    AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.AuditoryId),
                                    CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.CuestionarioId),
                                    Text = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.Text),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioObservationsGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioObservationsGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioObservationsGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioObservationsGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioObservationsGet.Active)
                                });
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State == ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<AuditoryCuestionarioObservations>(res);
        }

        public static ReadOnlyCollection<AuditoryCuestionarioObservations> ByAuditory(long auditoryId, int companyId)
        {
            var res = new List<AuditoryCuestionarioObservations>();
            using (var cmd = new SqlCommand("AuditoryCuestionarioObservations_ByAuditoryId"))
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
                                res.Add(new AuditoryCuestionarioObservations
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.Id),
                                    CompanyId = Convert.ToInt32(rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.CompanyId)),
                                    AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.AuditoryId),
                                    CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioObservationsGet.CuestionarioId),
                                    Text = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.Text),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioObservationsGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioObservationsGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioObservationsGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioObservationsGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioObservationsGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioObservationsGet.Active)
                                });
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State == ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<AuditoryCuestionarioObservations>(res);
        }

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryCuestionarioObservations_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@CuestionarioId", this.CuestionarioId));
                    cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, Constant.MaximumTextAreaLength));
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

        public ActionResult Save(int applicationUserId)
        {
            if (this.Id > 0)
            {
                return this.Update(applicationUserId);
            }

            return this.Insert(applicationUserId);
        }

        public ActionResult Update(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryCuestionarioObservations_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, Constant.MaximumTextAreaLength));
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