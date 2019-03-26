// --------------------------------
// <copyright file="IncidentActionZombie.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
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

namespace GisoFramework.Item
{
    public class IncidentActionZombie
    {
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public long AuditoryId { get; set; }
        public string Description { get; set; }
        public string WhatHappend { get; set; }
        public Employee WhatHappendBy { get; set; }
        public DateTime WhatHappendOn { get; set; }
        public int ActionType { get; set; }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""AuditoryId"":{2},""ActionType"":{3},
                    ""Description"":""{4}"",
                    ""WhatHappend"":""{5}"",
                    ""WhatHappendBy"":{6},
                    ""WhatHappendOn"":""{7:dd/MM/yyyy}""}}",
                    this.Id,
                    this.CompanyId,
                    this.AuditoryId,
                    this.ActionType,
                    Tools.JsonCompliant(this.Description),
                    Tools.JsonCompliant(this.WhatHappend),
                    this.WhatHappendBy.JsonSimple,
                    this.WhatHappendOn);
            }
        }

        public static string JsonList(ReadOnlyCollection<IncidentActionZombie> list)
        {
            var res = new StringBuilder("[");
            if(list != null && list.Count > 0)
            {
                bool first = true;
                foreach(var zombie in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(zombie.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<IncidentActionZombie> ByAuditoryId(long auditoryId, int companyId)
        {
            var res = new List<IncidentActionZombie>();
            using(var cmd = new SqlCommand("IncidentActionZombie_ByAuditoryId"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
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
                                res.Add(new IncidentActionZombie
                                {
                                    Id = rdr.GetInt64(0),
                                    CompanyId  = rdr.GetInt32(1),
                                    AuditoryId = rdr.GetInt64(2),
                                    ActionType= rdr.GetInt32(3),
                                    Description = rdr.GetString(4),
                                    WhatHappend = rdr.GetString(5),
                                    WhatHappendBy = new Employee()
                                    {
                                        Id = rdr.GetInt32(6),
                                        Name = rdr.GetString(7),
                                        LastName = rdr.GetString(8)
                                    },
                                    WhatHappendOn = rdr.GetDateTime(9)
                                });
                            }
                        }
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

            return new ReadOnlyCollection<IncidentActionZombie>(res);
        }

        public ActionResult Insert()
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("IncidentActionZombie_Insert"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@ActionType", this.ActionType));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappend", this.WhatHappend, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendBy", this.WhatHappendBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendOn", this.WhatHappendOn));
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

        public ActionResult Update()
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("IncidentActionZombie_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    cmd.Parameters.Add(DataParameter.Input("@ActionType", this.ActionType));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappend", this.WhatHappend, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendBy", this.WhatHappendBy.Id));
                    cmd.Parameters.Add(DataParameter.Input("@WhatHappendOn", this.WhatHappendOn));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
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

        public ActionResult Delete()
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("IncidentActionZombie_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", this.AuditoryId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
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

        public ActionResult ConvertToIncidentAction(int applicationUserId, long providerId, long customerId)
        {
            var res = ActionResult.NoAction;
            int reporterType = 2;
            if(customerId > 0)
            {
                reporterType = 3;
            }
            /* CREATE PROCEDURE [dbo].[IncidentAction_FromZombie]
             *   @ZombieId bigint,
             *   @CompanyId int,
             *   @ApplicationUserId int,
             *   @ReporterType int,
             *   @ProviderId bigint,
             *   @CustomerId bigint */
            using (var cmd = new SqlCommand("IncidentAction_FromZombie"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ZombieId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@ReporterType", reporterType));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", providerId));
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", customerId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
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
