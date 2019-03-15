// --------------------------------
// <copyright file="AuditoryCuestionarioImprovement.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
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

    public class AuditoryCuestionarioImprovement : BaseItem
    {
        public string Text { get; set; }
        public long AuditoryId { get; set; }
        public long CuestionarioId { get; set; }

        public static AuditoryCuestionarioImprovement Empty
        {
            get
            {
                return new AuditoryCuestionarioImprovement
                {
                    Id = Constant.DefaultLongId,
                    CompanyId = Constant.DefaultId,
                    AuditoryId = Constant.DefaultLongId,
                    CuestionarioId = Constant.DefaultLongId,
                    Description = string.Empty,
                    Text = string.Empty,
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
                return string.Empty;
            }
        }

        public override string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""CompanyId"":{1}, ""AuditoryId"":{2},""CuestionarioId"":{3},
                      ""Text"":""{4}"", ""CreatedBy"":{5}, ""CreatedOn"":""{6:dd/MM/yyy}"",
                      ""ModifiedBy"":{7}, ""ModifiedOn"":""{7:dd/MM/yyyy}"", ""Active"":{8}}}",
                    this.Id,
                    this.CompanyId,
                    this.AuditoryId,
                    this.CuestionarioId,
                    Tools.JsonCompliant(this.Text),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? Constant.JavaScriptTrue : Constant.JavaScriptFalse);
            }
        }

        public override string JsonKeyValue => Json;

        public static string JsonList(ReadOnlyCollection<AuditoryCuestionarioImprovement> list)
        {
            var res = new StringBuilder("[");
            if(list != null)
            {
                bool first = true;
                foreach(var improvement in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(improvement.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static AuditoryCuestionarioImprovement ById(long id, long cuestionarioId, long auditoryId, int companyId)
        {
            var res = AuditoryCuestionarioImprovement.Empty;
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
                                res.Id = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.Id);
                                res.CompanyId = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.CompanyId);
                                res.AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.AuditoryId);
                                res.CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.CuestionarioId);
                                res.Text = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.Text);
                                res.CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioImprovementGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioImprovementGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioImprovementGet.Active);
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

        public static ReadOnlyCollection<AuditoryCuestionarioImprovement> ByCuestionary(long cuestionarioId, long auditoryId, int companyId)
        {
            var res = new List<AuditoryCuestionarioImprovement>();
            using (var cmd = new SqlCommand("AuditoryCuestionarioImprovement_ByCuestionario"))
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
                                res.Add(new AuditoryCuestionarioImprovement
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.CompanyId),
                                    AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.AuditoryId),
                                    CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.CuestionarioId),
                                    Text = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.Text),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioImprovementGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioImprovementGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioImprovementGet.Active)
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

            return new ReadOnlyCollection<AuditoryCuestionarioImprovement>(res);
        }

        public static ReadOnlyCollection<AuditoryCuestionarioImprovement> ByAuditory(long auditoryId, int companyId)
        {
            var res = new List<AuditoryCuestionarioImprovement>();
            using(var cmd = new SqlCommand("AuditoryCuestionarioImprovement_ByAuditoryId"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditoryId));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new AuditoryCuestionarioImprovement
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.CompanyId),
                                    AuditoryId = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.AuditoryId),
                                    CuestionarioId = rdr.GetInt64(ColumnsAuditoryCuestionarioImprovementGet.CuestionarioId),
                                    Text = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.Text),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioImprovementGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsAuditoryCuestionarioImprovementGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsAuditoryCuestionarioImprovementGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAuditoryCuestionarioImprovementGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAuditoryCuestionarioImprovementGet.Active)
                                });
                            }
                        }
                    }
                    finally
                    {
                        if(cmd.Connection.State == ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<AuditoryCuestionarioImprovement>(res);
        }

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using(var cmd = new SqlCommand("AuditoryCuestionarioImprovement_Insert"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@CuestionarioId", this.CuestionarioId));
                    cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(this.Id);
                    }
                    catch(Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if(cmd.Connection.State != ConnectionState.Closed)
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
            using (var cmd = new SqlCommand("AuditoryCuestionarioImprovement_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@CuestionarioId", this.CuestionarioId));
                    cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 2000));
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
            using (var cmd = new SqlCommand("AuditoryCuestionarioImprovement_Activate"))
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
            using (var cmd = new SqlCommand("AuditoryCuestionarioImprovement_Inactivate"))
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
