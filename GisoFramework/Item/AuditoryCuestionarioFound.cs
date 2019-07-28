

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

    public class AuditoryCuestionarioFound : BaseItem
    {
        public long AuditoryId { get; set; }
        public long CuestionarioId { get; set; }
        public string Text { get; set; }
        public string Requeriment { get; set; }
        public string Unconformity { get; set; }
        public bool Action { get; set; }

        public static AuditoryCuestionarioFound Empty
        {
            get
            {
                return new AuditoryCuestionarioFound
                {
                    Id = Constant.DefaultLongId,
                    CompanyId = Constant.DefaultId,
                    AuditoryId = Constant.DefaultLongId,
                    CuestionarioId = Constant.DefaultLongId,
                    Description = string.Empty,
                    Text = string.Empty,
                    Requeriment = string.Empty,
                    Unconformity = string.Empty,
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
                        ""Requeriment"":""{5}"",
                        ""Unconformity"":""{6}"",
                        ""CreatedBy"":{7},
                        ""CreatedOn"":""{8:dd/MM/yyyy}"",
                        ""ModifiedBy"":{9},
                        ""ModifiedOn"":""{10:dd/MM/yyyy}"",
                        ""Action"":{11},
                        ""Active"":{12}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.AuditoryId,
                    this.CuestionarioId,
                    Tools.JsonCompliant(this.Text),
                    Tools.JsonCompliant(this.Requeriment),
                    Tools.JsonCompliant(this.Unconformity),
                    this.CreatedBy.Json,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Action ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.Active ? Constant.JavaScriptTrue : Constant.JavaScriptFalse);
            }
        }

        public override string JsonKeyValue => Json;

        public static string JsonList(ReadOnlyCollection<AuditoryCuestionarioFound> list)
        {
            var res = new StringBuilder("[");
            if (list != null)
            {
                bool first = true;
                foreach (var found in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(found.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static AuditoryCuestionarioFound ById(long id, long cuestionarioId, long auditoryId, int companyId)
        {
            var res = AuditoryCuestionarioFound.Empty;
            using (var cmd = new SqlCommand("AuditoryCuestionarioImprovement_ById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@Cuestionario", cuestionarioId));
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Id = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.Id);
                                res.CompanyId = rdr.GetInt32(ColumnsAuditoryCuestionarioFoundGet.CompanyId);
                                res.AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.AuditoryId);
                                res.CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.CuestionarioId);
                                res.Text = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Text);
                                res.Requeriment = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Requeriment);
                                res.Unconformity = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Unconformity);
                                res.CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryCuestionarioFoundGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioFoundGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryCuestionarioFoundGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioFoundGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioFoundGet.Active);
                                res.Action = rdr.GetBoolean(ColumnsAuditoryCuestionarioFoundGet.Action);
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

        public static ReadOnlyCollection<AuditoryCuestionarioFound> ByCuestionary(long cuestionarioId, long auditoryId, int companyId)
        {
            var res = new List<AuditoryCuestionarioFound>();
            using (var cmd = new SqlCommand("AuditoryCuestionarioFound_ByCuestionarioId"))
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
                                res.Add(new AuditoryCuestionarioFound
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.Id),
                                    CompanyId = Convert.ToInt32(rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.CompanyId)),
                                    AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.AuditoryId),
                                    CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.CuestionarioId),
                                    Text = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Text),
                                    Requeriment = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Requeriment),
                                    Unconformity = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Unconformity),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioFoundGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioFoundGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioFoundGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioFoundGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioFoundGet.Active),
                                    Action = rdr.GetBoolean(ColumnsAuditoryCuestionarioFoundGet.Action)
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

            return new ReadOnlyCollection<AuditoryCuestionarioFound>(res);
        }

        public static ReadOnlyCollection<AuditoryCuestionarioFound> ByAuditory(long auditoryId, int companyId)
        {
            var res = new List<AuditoryCuestionarioFound>();
            using (var cmd = new SqlCommand("AuditoryCuestionarioFound_ByAuditoryId"))
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
                                res.Add(new AuditoryCuestionarioFound
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.Id),
                                    CompanyId = Convert.ToInt32( rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.CompanyId)),
                                    AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.AuditoryId),
                                    CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioFoundGet.CuestionarioId),
                                    Requeriment = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Requeriment),
                                    Unconformity = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Unconformity),
                                    Text = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.Text),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioFoundGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioFoundGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioFoundGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioFoundGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioFoundGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioFoundGet.Active),
                                    Action = rdr.GetBoolean(ColumnsAuditoryCuestionarioFoundGet.Action)
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

            return new ReadOnlyCollection<AuditoryCuestionarioFound>(res);
        }

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryCuestionarioFound_Insert"))
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
                    cmd.Parameters.Add(DataParameter.Input("@Requeriment", this.Requeriment, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Unconformity", this.Unconformity, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Action", this.Action));
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

        public ActionResult Update(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryCuestionarioFound_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Requeriment", this.Requeriment, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Unconformity", this.Unconformity, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Action", this.Action));
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

        public static ActionResult Activate(long id, int companyId, int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryCuestionarioFound_Activate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(id);
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

        public static ActionResult Inactivate(long id, int companyId, int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("AuditoryCuestionarioFound_Inactivate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(id);
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
