// --------------------------------
// <copyright file="RuleHistory.cs" company="OpenFramework">
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
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    public class RuleHistory : BaseItem
    {
        public long RuleId { get; set; }
        public int IPR { get; set; }
        public string Reason { get; set; }

        public override string Json => throw new NotImplementedException();

        public override string JsonKeyValue => throw new NotImplementedException();

        public override string Link => throw new NotImplementedException();

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, "RuleHistoryInsert({0})", this.Reason);
            /* CREATE PROCEDURE RulesHistory_Insert
             *   @RuleId bigint,
             *   @IPR int,
             *   @Reason nvarchar(500),
             *   @ApplicationUserId int */
            using(var cmd = new SqlCommand("RulesHistory_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@RuleId", this.RuleId));
                    cmd.Parameters.Add(DataParameter.Input("@IPR", this.IPR));
                    cmd.Parameters.Add(DataParameter.Input("@Reason", this.Reason, 500));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NotSupportedException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, source);
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

        public static ReadOnlyCollection<RuleHistory> ByRule(long ruleId)
        {
            var res = new List<RuleHistory>();
            using (var cmd = new SqlCommand("RulesHistory_ByRuleId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var cnn  = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@RuleId", ruleId));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new RuleHistory
                                {
                                    Id = rdr.GetInt64(ColumnsRulesHistoryGetByRule.Id),
                                    Active = true,
                                    CanBeDeleted = false,
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsRulesHistoryGetByRule.CreatedBy),
                                        UserName = rdr.GetString(ColumnsRulesHistoryGetByRule.Login),
                                        Employee = new Employee
                                        {
                                            Id = 0,
                                            Name = rdr.GetString(ColumnsRulesHistoryGetByRule.FirstName),
                                            LastName = rdr.GetString(ColumnsRulesHistoryGetByRule.LastName)
                                        }
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsRulesHistoryGetByRule.CreatedOn),
                                    IPR = rdr.GetInt32(ColumnsRulesHistoryGetByRule.IPR),
                                    Reason = rdr.GetString(ColumnsRulesHistoryGetByRule.Reason),
                                    RuleId = ruleId
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

            return new ReadOnlyCollection<RuleHistory>(res);
        }
    }
}