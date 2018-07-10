// --------------------------------
// <copyright file="Oportunity.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Linq;

    public class Oportunity : BaseItem
    {
        public int? Cost { get; set; }
        public int? Impact { get; set; }
        public int? Result { get; set; }
        public string ItemDescription { get; set; }
        public string Notes { get; set; }
        public string Control { get; set; }
        public Process Process { get; set; }
        public Rules Rule { get; set; }
        public bool ApplyAction { get; set; }
        public string Causes { get; set; }
        public DateTime DateStart { get; set; }
        public int Code { get; set; }

        //public ApplicationUser CreatedBy { get; set; }
        //public ApplicationUser ModifiedBy { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public DateTime ModifiedOn { get; set; }
        //public bool Active { get; set; }

        public ApplicationUser AnulateBy { get; set; }
        public DateTime? AnulateDate { get; set; }
        public string AnulateReason { get; set; }

        public override string Json
        {
            get
            {
                var anulateDateText = Constant.JavaScriptNull;
                if (this.AnulateDate.HasValue)
                {
                    anulateDateText = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0:dd/MM/yyyy}",
                        this.AnulateDate);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""DateStart"":""{2:dd/MM/yyyy}"",
                        ""Cost"":{3},
                        ""Impact"":{4},
                        ""Result"":{5},
                        ""Rule"":{6},
                        ""Process"":{7},
                        ""ApplyAction"":{8},
                        ""Causes"":""{9}"",
                        ""ItemDescription"":""{10}"",
                        ""Control"":""{11}"",
                        ""Notes"":""{12}"",
                        ""CreatedBy"":{13},
                        ""CreatedOn"":""{14:dd/MM/yyyy}"",
                        ""ModifiedBy"":{15},
                        ""ModifiedOn"":""{16:dd/MM/yyyy}"",
                        ""Active"":{17},
                        ""AnulateBy"":{18},
                        ""AnulateDate"":""{19}"",
                        ""AnulateReason"":""{20}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.DateStart,
                    this.Cost.HasValue ? string.Format(CultureInfo.InvariantCulture,"{0}", this.Cost) : Constant.JavaScriptNull,
                    this.Impact.HasValue ? string.Format(CultureInfo.InvariantCulture, "{0}", this.Impact) : Constant.JavaScriptNull,
                    this.Result.HasValue ? string.Format(CultureInfo.InvariantCulture, "{0}", this.Result) : Constant.JavaScriptNull,
                    this.Rule.Json,
                    this.Process.Json,
                    this.ApplyAction ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    Tools.JsonCompliant(this.Causes),
                    Tools.JsonCompliant(this.ItemDescription),
                    Tools.JsonCompliant(this.Control),
                    Tools.JsonCompliant(this.Notes),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.AnulateBy.JsonKeyValue,
                    anulateDateText,
                    Tools.JsonCompliant(this.AnulateReason));
            }
        }

        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Description"":""{1}"",""Active"":{2}}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Active ? Constant.JavaScriptTrue : Constant.JavaScriptFalse);
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""/OportunityView.aspx?id={0}"">{1}</a>",
                    this.Id,
                    this.Description);
            }
        }

        public static Oportunity Empty
        {
            get
            {
                return new Oportunity
                {
                    Id = Constant.DefaultId,
                    Description = string.Empty,
                    ItemDescription = string.Empty,
                    Control = string.Empty,
                    Causes = string.Empty,
                    Notes = string.Empty,
                    Process = Process.Empty,
                    Rule = Rules.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false,
                    AnulateBy = ApplicationUser.Empty,
                    AnulateReason = string.Empty
                };
            }
        }

        public static string AllJsonList(int companyId)
        {
            return JsonList(All(companyId));
        }

        public static string JsonList(ReadOnlyCollection<Oportunity> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var oportunity in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(oportunity.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Oportunity> All(int companyId)
        {
            var res = new List<Oportunity>();
            using (var cmd = new SqlCommand("Oportunity_GetAll"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@CompannyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newOportunity = new Oportunity
                                {
                                    Id = rdr.GetInt64(ColumnsOportunityGet.Id),
                                    CompanyId = companyId,
                                    Description = rdr.GetString(ColumnsOportunityGet.Description),
                                    ItemDescription = rdr.GetString(ColumnsOportunityGet.ItemDescription),
                                    Control = rdr.GetString(ColumnsOportunityGet.StartControl),
                                    Causes = rdr.GetString(ColumnsOportunityGet.Causes),
                                    Notes = rdr.GetString(ColumnsOportunityGet.Notes),
                                    ApplyAction = rdr.GetBoolean(ColumnsOportunityGet.ApplyAction),
                                    Active = rdr.GetBoolean(ColumnsOportunityGet.Active),
                                    DateStart = rdr.GetDateTime(ColumnsOportunityGet.DateStart),
                                    CreatedOn = rdr.GetDateTime(ColumnsOportunityGet.CreatedOn),
                                    ModifiedOn = rdr.GetDateTime(ColumnsOportunityGet.ModifiedOn),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsOportunityGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsOportunityGet.CreatedByLastName)
                                    },
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsOportunityGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsOportunityGet.ModifiedByLastName)
                                    }
                                };

                                if (!rdr.IsDBNull(ColumnsOportunityGet.Cost))
                                {
                                    newOportunity.Cost = rdr.GetInt32(ColumnsOportunityGet.Cost);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.Impact))
                                {
                                    newOportunity.Impact = rdr.GetInt32(ColumnsOportunityGet.Impact);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.Result))
                                {
                                    newOportunity.Result = rdr.GetInt32(ColumnsOportunityGet.Result);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.ProcessId))
                                {
                                    newOportunity.Process = new Process
                                    {
                                        Id = rdr.GetInt64(ColumnsOportunityGet.ProcessId),
                                        Description = rdr.GetString(ColumnsOportunityGet.ProcessDescription)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.RuleId))
                                {
                                    newOportunity.Rule = new Rules
                                    {
                                        Id = rdr.GetInt64(ColumnsOportunityGet.RuleId),
                                        Description = rdr.GetString(ColumnsOportunityGet.RuleDescription)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.AnulateBy))
                                {
                                    newOportunity.AnulateDate = rdr.GetDateTime(ColumnsOportunityGet.AnulateDate);
                                    newOportunity.AnulateReason = rdr.GetString(ColumnsOportunityGet.AnulateReason);
                                    newOportunity.AnulateBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsOportunityGet.AnulateBy),
                                        UserName = rdr.GetString(ColumnsOportunityGet.AnulateByName)
                                    };
                                }

                                res.Add(newOportunity);
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

            return new ReadOnlyCollection<Oportunity>(res);
        }

        public static Oportunity ById(long id, int companyId)
        {
            var res = Oportunity.Empty;
            using (var cmd = new SqlCommand("Oportunity_GetById"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res.Id = id;
                                res.CompanyId = companyId;
                                res.Description = rdr.GetString(ColumnsOportunityGet.Description);
                                res.ItemDescription = rdr.GetString(ColumnsOportunityGet.ItemDescription);
                                res.Control = rdr.GetString(ColumnsOportunityGet.StartControl);
                                res.Causes = rdr.GetString(ColumnsOportunityGet.Causes);
                                res.Notes = rdr.GetString(ColumnsOportunityGet.Notes);
                                res.ApplyAction = rdr.GetBoolean(ColumnsOportunityGet.ApplyAction);
                                res.Active = rdr.GetBoolean(ColumnsOportunityGet.Active);
                                res.DateStart = rdr.GetDateTime(ColumnsOportunityGet.DateStart);
                                res.CreatedOn = rdr.GetDateTime(ColumnsOportunityGet.CreatedOn);
                                res.ModifiedOn = rdr.GetDateTime(ColumnsOportunityGet.ModifiedOn);
                                res.CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsOportunityGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsOportunityGet.CreatedByLastName)
                                };

                                res.ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsOportunityGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsOportunityGet.ModifiedByLastName)
                                };

                                if (!rdr.IsDBNull(ColumnsOportunityGet.Cost))
                                {
                                    res.Cost = rdr.GetInt32(ColumnsOportunityGet.Cost);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.Impact))
                                {
                                    res.Impact = rdr.GetInt32(ColumnsOportunityGet.Impact);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.Result))
                                {
                                    res.Result = rdr.GetInt32(ColumnsOportunityGet.Result);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.ProcessId))
                                {
                                    res.Process = new Process
                                    {
                                        Id = rdr.GetInt64(ColumnsOportunityGet.ProcessId),
                                        Description = rdr.GetString(ColumnsOportunityGet.ProcessDescription)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.RuleId))
                                {
                                    res.Rule = new Rules
                                    {
                                        Id = rdr.GetInt64(ColumnsOportunityGet.RuleId),
                                        Description = rdr.GetString(ColumnsOportunityGet.RuleDescription)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityGet.AnulateBy))
                                {
                                    res.AnulateDate = rdr.GetDateTime(ColumnsOportunityGet.AnulateDate);
                                    res.AnulateReason = rdr.GetString(ColumnsOportunityGet.AnulateReason);
                                    res.AnulateBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsOportunityGet.AnulateBy),
                                        UserName = rdr.GetString(ColumnsOportunityGet.AnulateByName)
                                    };
                                }
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

            return res;
        }

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Oportunity_Insert
             *   @Id int out,
             *   @CompanyId int,
             *   @Description nvarchar(100),
             *   @Code bigint,
             *   @ItemDescription nvarchar(2000),
             *   @StartControl nvarchar(2000),
             *   @Notes nvarchar(2000),
             *   @ApplyAction bit,
             *   @DateStart datetime,
             *   @Causes nvarchar(2000),
             *   @Cost int,
             *   @Impact int,
             *   @Result int,
             *   @ProcessId bigint,
             *   @RuleId bigint,
             *   @ApplicationUserId int */
            using(var cmd = new SqlCommand("Oportunity_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Code", 0));
                    cmd.Parameters.Add(DataParameter.Input("@ItemDescription", this.ItemDescription, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@StartControl", this.Control, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@ApplyAction", this.ApplyAction));
                    cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@DateStart", this.DateStart));

                    if (this.Cost == null)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@Cost"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                    }


                    if (this.Impact == null)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@Impact"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Impact", this.Impact));
                    }

                    if (this.Result == null)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@Result"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                    }

                    if (this.Process == null || this.Process.Id < 1)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@ProcessId"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                    }

                    if (this.Rule == null || this.Rule.Id < 1)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@RuleId"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@RuleId", this.Rule.Id));
                    }

                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(Convert.ToInt32(cmd.Parameters["@Id"].Value.ToString()));
                    }
                    catch(Exception ex)
                    {
                        ExceptionManager.Trace(ex, "Oporunity insert");
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
            using (var cmd = new SqlCommand("Oportunity_Update"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Code", 0));
                    cmd.Parameters.Add(DataParameter.Input("@ItemDescription", this.ItemDescription, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@StartControl", this.Control, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@ApplyAction", this.ApplyAction));
                    cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("@DateStart", this.DateStart));

                    if (this.Cost == null)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@Cost"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                    }


                    if (this.Impact == null)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@Impact"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Impact", this.Impact));
                    }

                    if (this.Result == null)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@Result"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                    }

                    if (this.Process == null || this.Process.Id < 1)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@ProcessId"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                    }

                    if (this.Rule == null || this.Rule.Id < 1)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@RuleId"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@RuleId", this.Rule.Id));
                    }

                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(Convert.ToInt32(cmd.Parameters["@Id"].Value.ToString()));
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex, "Oporunity insert");
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

        public static ActionResult Anulate(long oportunityId, int companyId, int anulateBy, DateTime anulateDate, string anulateReason, int applicationUserId)
        {
            /* ALTER PROCEDURE Oportunity_Anulate
             *   @Id int,
             *   @CompanyId int,
             *   @AnulatedBy int,
             *   @AnulateDate datetime,
             *   @AnulateReason nvarchar(2000),
             *   @ApplicationUserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Oportunity_Anulate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("Id", oportunityId));
                    cmd.Parameters.Add(DataParameter.Input("CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@AnulateBy", anulateBy));
                    cmd.Parameters.Add(DataParameter.Input("@AnulateDate", anulateDate));
                    cmd.Parameters.Add(DataParameter.Input("@AnulateReason", anulateReason, Constant.MaximumTextAreaLength));
                    cmd.Parameters.Add(DataParameter.Input("ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        public static ActionResult Activate(int oportunityId, int companyId, int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using(var cmd = new SqlCommand("Oportunity_Activate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("Id", oportunityId));
                    cmd.Parameters.Add(DataParameter.Input("CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        public static ActionResult Inactivate(int oportunityId, int companyId, int applicationUserId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Oportunity_Inactivate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("Id", oportunityId));
                    cmd.Parameters.Add(DataParameter.Input("CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        /// <summary>Gets a Json containing all the items filtered</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="from">Start date to filter</param>
        /// <param name="to">Final date to filter</param>
        /// <param name="rulesId">Rules identifier to filter</param>
        /// <param name="processId">Process identifier to filter</param>
        /// <returns>String containing a Json</returns>
        public static string FilterList(int companyId, DateTime? from, DateTime? to, long rulesId, long processId)
        {
            var items = Filter(companyId, from, to, rulesId, processId);
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var item in items)
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
                res.Append(Tools.JsonPair("OportunityId", item.Id)).Append(", ");
                res.Append(Tools.JsonPair("OpenDate", item.OpenDate)).Append(", ");
                res.Append(Tools.JsonPair("Description", item.Description)).Append(", ");
                res.Append(Tools.JsonPair("Code", item.Code)).Append(", ");
                res.Append(Tools.JsonPair("CloseDate", item.AnulateDate)).Append(", ");
                res.Append(Tools.JsonPair("Process", item.Process.JsonKeyValue)).Append(", ");
                res.Append(Tools.JsonPair("Rules", item.Rule.JsonKeyValue)).Append(", ");
                res.Append(Tools.JsonPair("Result", item.Result)).Append(", ");
                res.Append(Tools.JsonPair("RuleLimit", item.Rule.Limit)).Append("}");
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<OportunityFilterItem> Filter(int companyId, DateTime? from, DateTime? to, long rulesId, long processId)
        {
            /* CREATE PROCEDURE [dbo].[Oportunity_Filter]
             *   @CompanyId int,
             *   @DateFrom datetime,
             *   @DateTo datetime,
             *   @RulesId bigint,
             *   @ProcessId bigint */
            var res = new List<OportunityFilterItem>();
            using (var cmd = new SqlCommand("Oportunity_Filter"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@DateFrom", from));
                        cmd.Parameters.Add(DataParameter.Input("@DateTo", to));
                        if (rulesId == 0)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@RulesId"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@RulesId", rulesId));
                        }

                        if (processId == 0)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@ProcessId"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@ProcessId", processId));
                        }

                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new OportunityFilterItem
                                {
                                    Id = rdr.GetInt64(ColumnsOportunityFilterGet.OportunityId),
                                    Description = rdr.GetString(ColumnsOportunityFilterGet.Description),
                                    Code = "0",
                                    OpenDate = rdr.GetDateTime(ColumnsOportunityFilterGet.OpenDate),
                                    Result = rdr.GetInt32(ColumnsOportunityFilterGet.Result)
                                };

                                if (!rdr.IsDBNull(ColumnsOportunityFilterGet.CloseDate))
                                {
                                    item.AnulateDate = rdr.GetDateTime(ColumnsOportunityFilterGet.CloseDate);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityFilterGet.RuleId))
                                {
                                    item.Rule = new Rules
                                    {
                                        Id = rdr.GetInt64(ColumnsOportunityFilterGet.RuleId),
                                        Description = rdr.GetString(ColumnsOportunityFilterGet.RuleDescription),
                                        Limit = rdr.GetInt64(ColumnsOportunityFilterGet.Limit)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityFilterGet.ProcessId))
                                {
                                    item.Process = new Process
                                    {
                                        Id = rdr.GetInt64(ColumnsOportunityFilterGet.ProcessId),
                                        Description = rdr.GetString(ColumnsOportunityFilterGet.ProcessDescription)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityFilterGet.OpenDate))
                                {
                                    item.OpenDate = rdr.GetDateTime(ColumnsOportunityFilterGet.OpenDate);
                                }

                                if (!rdr.IsDBNull(ColumnsOportunityFilterGet.CloseDate))
                                {
                                    item.AnulateDate = rdr.GetDateTime(ColumnsOportunityFilterGet.CloseDate);
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
                }
            }

            HttpContext.Current.Session["OportunityFilterData"] = res;
            return new ReadOnlyCollection<OportunityFilterItem>(res);
        }
    }
}