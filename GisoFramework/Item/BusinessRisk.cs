// -----------------------------------------------------------------------
// <copyright file="BusinessRisk.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Item representation of a Business Risk </summary>
    public class BusinessRisk : BaseItem
    {
        public const bool OnlyActive = true;
        public const bool ActiveAndInactive = false;
        #region Properties

        /// <summary>Gets a empty instance of BusinessRisk</summary>
        public static BusinessRisk Empty
        {
            get
            {
                return new BusinessRisk
                {
                    Id = -1,
                    CompanyId = -1,
                    Description = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = Constant.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = Constant.Now,
                    Active = false,
                    Code = -1,
                    Rules = Rules.Empty,
                    ItemDescription = string.Empty,
                    StartControl = string.Empty,
                    Notes = string.Empty,
                    Causes = string.Empty,
                    Probability = 0,
                    Severity = 0,
                    InitialValue = -1,
                    Result = -1,
                    ApplyAction = false,
                    DateStart = Constant.Now,
                    Process = Process.Empty,
                    Assumed = false,
                    PreviousBusinessRiskId = -1,
                };
            }
        }

        /// <summary>Gets or sets the Code of a the BusinessRisk</summary>
        public long Code { get; set; }

        /// <summary>Gets or sets the Rule associated to the Business Risk</summary>
        public Rules Rules { get; set; }

        /// <summary>Gets or sets the Description of the Risk</summary>
        public string ItemDescription { get; set; }

        /// <summary>Gets or sets the Start control of the Risk</summary>
        public string StartControl { get; set; }

        /// <summary>Gets or sets the additional information on the Risk</summary>
        public string Notes { get; set; }

        /// <summary>Gets or sets the additional information of causes on the Risk</summary>
        public string Causes { get; set; }

        /// <summary>Gets or sets the initial probability of the risk</summary>
        public long Probability { get; set; }

        /// <summary>Gets or sets the initial severity of the risk</summary>
        public long Severity { get; set; }

        /// <summary>Gets or sets the Initial value of the risk</summary>
        public int InitialValue { get; set; }

        /// <summary>Gets or sets the Result of the risk (probability times the severity)</summary>
        public int Result { get; set; }

        /// <summary>Gets or sets a value indicating whether if there is an action applied to the risk</summary>
        public bool ApplyAction { get; set; }

        /// <summary>Gets or sets the starting date of the risk</summary>
        public DateTime DateStart { get; set; }

        /// <summary>Gets or sets the final date of the risk</summary>
        public DateTime? FinalDate { get; set; }

        /// <summary>Gets or sets the identifier of the associated process</summary>
        public long ProcessId { get; set; }

        /// <summary>Gets or sets the associated process</summary>
        public Process Process { get; set; }

        /// <summary>Gets or sets a value indicating whether if the risk is assumed</summary>
        public bool Assumed { get; set; }

        /// <summary>Gets or sets the identifier of the risk that is inherited</summary>
        public long PreviousBusinessRiskId { get; set; }

        /// <summary>Gets or sets the start probability</summary>
        public int StartProbability { get; set; }

        /// <summary>Gets or sets the start severity</summary>
        public int StartSeverity { get; set; }

        /// <summary>Gets or sets the start result</summary>
        public int StartResult { get; set; }

        /// <summary>Gets or sets the start action</summary>
        public int StartAction { get; set; }

        /// <summary>Gets or sets the final probability of the risk</summary>
        public int FinalProbability { get; set; }

        /// <summary>Gets or sets the final severity of the risk</summary>
        public int FinalSeverity { get; set; }

        /// <summary>Gets or sets the final result of the risk (probability times the severity)</summary>
        public int FinalResult { get; set; }

        /// <summary>Gets or sets the indentifier of final action</summary>
        public int FinalAction { get; set; }

        /// <summary>Gets a hyper link to businessRisk profile page</summary>
        public string LinkList
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""BusinessRiskView.aspx?id={0}"" title=""{2} {1}"">{1}</a>",
                    this.Id,
                    this.Description,
                    ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }

        /// <summary>Gets a hyper link to businessRisk profile page</summary>
        public string LinkCode
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""BusinessRiskView.aspx?id={0}"">{1:00000}</a>",
                    this.Id,
                    this.Code);
            }
        }

        /// <summary>Gets a hyper link to businessRisk profile page</summary>
        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""BusinessRiskView.aspx?id={0}"" title=""{1}"">{2}</a>",
                    this.Id,
                    this.Description,
                    ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0},""Description"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string finalDate = "null";
                if (this.FinalDate.HasValue)
                {
                    finalDate = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"""{0:dd/MM/yyyy}""",
                        this.FinalDate.Value);
                }

                string formattedJson =
                    @"{{
                        ""Id"": {0},
                        ""CompanyId"": {1},
                        ""Description"": ""{2}"",
                        ""CreatedBy"": {3},
                        ""CreatedOn"": ""{4:dd/MM/yyyy}"",
                        ""ModifiedBy"": {5},
                        ""ModifiedOn"": ""{6:dd/MM/yyyy}"",
                        ""Active"": {7},
                        ""Code"": {8},
                        ""Rules"": {9},
                        ""ItemDescription"": ""{10}"",
                        ""StartControl"": ""{11}"",
                        ""Notes"": ""{12}"",
                        ""DateStart"": ""{17:dd/MM/yyyy}"",
                        ""Process"": {18},
                        ""Assumed"": {19},
                        ""PreviousBusinessRiskId"": {20},
                        ""InitialValue"": {21},
                        ""Causes"": ""{22}"",
                        ""StartProbability"": {13},
                        ""StartSeverity"": {14},
                        ""StartResult"": {15},
                        ""StartAction"": {16},
                        ""FinalProbability"":{23},
                        ""FinalSeverity"":{24},
                        ""FinalResult"":{25},
                        ""FinalAction"":{26},
                        ""FinalDate"":{27}
                    }}";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    formattedJson,
                    this.Id,
                    this.CompanyId,
                    Tools.JsonCompliant(this.Description),
                    this.CreatedBy.Id,
                    this.CreatedOn,
                    this.ModifiedBy.Id,
                    this.ModifiedOn,
                    this.Active ? "true" : "false",
                    this.Code,
                    this.Rules.JsonKeyValue,
                    Tools.JsonCompliant(this.ItemDescription),
                    Tools.JsonCompliant(this.StartControl),
                    Tools.JsonCompliant(this.Notes),
                    this.StartProbability,
                    this.StartSeverity,
                    this.StartResult,
                    this.StartAction,
                    this.DateStart,
                    this.Process.JsonKeyValue,
                    this.Assumed ? "true" : "false",
                    this.PreviousBusinessRiskId,
                    this.InitialValue,
                    Tools.JsonCompliant(this.Causes),
                    this.FinalProbability,
                    this.FinalSeverity,
                    this.FinalResult,
                    this.FinalAction,
                    finalDate);
            }
        }

        /// <summary>Gets a JSON structure of businessRisk result</summary>
        public string JsonResult
        {
            get
            {
                string formattedJson =
                    @"{{
                        ""Id"": {0},                        
                        ""Code"": {1},
                        ""Rules"": {2},                        
                        ""Result"": {3},                        
                        ""Assumed"": {4},
                        ""RuleLimit"": {5},
                        ""Probability"": {6},
                        ""Severity"": {7},
                        ""FinalAction"": {8}
                    }}";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    formattedJson,
                    this.Id,
                    this.Code,
                    this.Rules.Json,
                    this.FinalDate.HasValue ? this.FinalResult : this.StartResult,
                    this.Assumed ? "true" : "false",
                    this.Rules.Limit,
                    this.Probability,
                    this.Severity,
                    this.FinalAction
                );
            }
        }

        /// <summary>Gets a JSON structure of businessRisk history</summary>
        public string JsonHistory
        {
            get
            {
                string formattedJson =
                    @"{{
                        ""Id"": {3},
                        ""Description"": ""{4}"",
                        ""DateStart"": ""{0:dd/MM/yyyy}"",
                        ""Result"": {1},
                        ""Assumed"": {2}
                    }}";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    formattedJson,
                    this.DateStart,
                    this.StartResult,
                    this.StartAction == 1 ? "true" : "false",
                    this.Id,
                    Tools.JsonCompliant(this.Description));
            }
        }

        #endregion

        /// <summary>Get BusinessRisk objects from database</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Read-only list of BusinessRisk objects</returns>
        public static ReadOnlyCollection<BusinessRisk> All(int companyId)
        {
            return All(companyId, ActiveAndInactive);
        }

        /// <summary>Get BusinessRisk objects from database</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="isOnlyActive">Choose whether to return all objects (false) or active objects only (true)</param>
        /// <returns>Read-only list of BusinessRisk objects</returns>
        public static ReadOnlyCollection<BusinessRisk> All(int companyId, bool isOnlyActive)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"GetAll({0}, {1})",
                companyId,
                isOnlyActive);
            var res = new List<BusinessRisk>();
            string query = "BusinessRisk_GetAll";
            using (var cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var newRisk = new BusinessRisk
                            {
                                Id = rdr.GetInt64(ColumnsBusinessRiskGetAll.Id),
                                Description = rdr.GetString(ColumnsBusinessRiskGetAll.Description),
                                Notes = rdr.GetString(ColumnsBusinessRiskGetAll.Notes),
                                CompanyId = companyId,
                                CreatedOn = rdr.GetDateTime(ColumnsBusinessRiskGetAll.CreatedOn),
                                CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsBusinessRiskGetAll.CreatedBy),
                                    UserName = rdr.GetString(ColumnsBusinessRiskGetAll.CreatedByName)
                                },
                                ModifiedOn = rdr.GetDateTime(ColumnsBusinessRiskGetAll.ModifiedOn),
                                ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsBusinessRiskGetAll.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsBusinessRiskGetAll.ModifiedByName)
                                },
                                Code = rdr.GetInt64(ColumnsBusinessRiskGetAll.Code),
                                Rules = new Rules
                                {
                                    Id = rdr.GetInt64(ColumnsBusinessRiskGetAll.RuleId),
                                    Description = rdr.GetString(ColumnsBusinessRiskGetAll.RuleDescription),
                                    Limit = rdr.GetInt64(ColumnsBusinessRiskGetAll.RuleRangeId)
                                },
                                ItemDescription = rdr.GetString(ColumnsBusinessRiskGetAll.ItemDescription),
                                StartControl = rdr.GetString(ColumnsBusinessRiskGetAll.StartControl),
                                Active = rdr.GetBoolean(ColumnsBusinessRiskGetAll.Active),

                                StartProbability = rdr.GetInt32(17),
                                StartSeverity = rdr.GetInt32(18),

                                StartResult = rdr.GetInt32(12),
                                StartAction = rdr.GetInt32(13),

                                FinalResult = rdr.GetInt32(29),
                                Assumed = rdr.GetBoolean(24),

                                FinalAction = rdr.GetInt32(31)
                            };

                            if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.Causes))
                            {
                                newRisk.Causes = rdr.GetString(ColumnsBusinessRiskGetAll.Causes);
                            }

                            //// Field can be Null in Database (no link to a previous BusinessRisk)
                            if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.PreviousBusinessRiskId))
                            {
                                newRisk.PreviousBusinessRiskId = rdr.GetInt64(ColumnsBusinessRiskGetAll.PreviousBusinessRiskId);
                            }
                            else
                            {
                                //// The previous BusinessRisk does not exist
                                newRisk.PreviousBusinessRiskId = -1;
                            }

                            if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.FinalDate))
                            {
                                newRisk.FinalDate = rdr.GetDateTime(ColumnsBusinessRiskGetAll.FinalDate);
                            }

                            res.Add(newRisk);
                        }
                    }

                    if (isOnlyActive)
                    {
                        res = res.Where(br => br.Active).ToList();
                    }

                    foreach (var b in res)
                    {
                        b.Process = new Process(b.ProcessId, companyId);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
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
                catch (NotSupportedException ex)
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

            return new ReadOnlyCollection<BusinessRisk>(res);
        }

        /// <summary>Get Active BusinessRisk objects from database</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Read-only list of active BusinessRisk objects</returns>
        public static ReadOnlyCollection<BusinessRisk> GetActive(int companyId)
        {
            return All(companyId, OnlyActive);
        }

        /// <summary>Get Active BusinessRisk objects from database by Rules Id</summary>
        /// <param name="rulesId">Rule identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Active businessrisks by rule</returns>
        public static ReadOnlyCollection<BusinessRisk> GetByRulesId(long rulesId, int companyId)
        {
            return new ReadOnlyCollection<BusinessRisk>(All(companyId, OnlyActive).Where(r => r.Rules.Id == rulesId).ToList());
        }
        
        /// <summary>Return an historical list of a businessRisk actions</summary>
        /// <param name="code">Code identifier of the BusinessRisk</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>ReadOnlyCollection of BusinessRisk items</returns>
        public static ReadOnlyCollection<IncidentAction> FindHistoryAction(long code, int companyId)
        {
            var res = new List<IncidentAction>();
            string query = "IncidentAction_GetByBusinessRiskCode";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@BusinessRiskCode", code));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));

                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newAction = new IncidentAction
                                {
                                    Id = rdr.GetInt64(0),
                                    Description = rdr.GetString(1),
                                    BusinessRiskId = rdr.GetInt64(2),
                                    WhatHappenedOn = rdr.GetDateTime(3)
                                };
                                if (!rdr.IsDBNull(4))
                                {
                                    newAction.CausesOn = rdr.GetDateTime(4);
                                }
                                if (!rdr.IsDBNull(5))
                                {
                                    newAction.ActionsOn = rdr.GetDateTime(5);
                                }
                                if (!rdr.IsDBNull(6))
                                {
                                    newAction.ClosedOn = rdr.GetDateTime(6);
                                }
                                res.Add(IncidentAction.ById(rdr.GetInt64(0), companyId));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
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

            return new ReadOnlyCollection<IncidentAction>(res);
        }

        /// <summary>Return an historical list of a businessRisk</summary>
        /// <param name="code">Code identifier of the BusinessRisk</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>ReadOnlyCollection of BusinessRisk items</returns>
        public static ReadOnlyCollection<BusinessRisk> GetByHistory(long code, int companyId)
        {
            var res = new List<BusinessRisk>();
            string query = "BusinessRisk_GetHistory";
            using (var cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@Code", code));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newRisk = new BusinessRisk
                                {
                                    Id = rdr.GetInt64(1),
                                    DateStart = rdr.GetDateTime(3),
                                    StartResult = rdr.GetInt32(5),
                                    StartAction = rdr.GetInt32(6)
                                };

                                //// Field can be Null in Database (no link to a previous BusinessRisk)
                                if (!rdr.IsDBNull(2))
                                {
                                    newRisk.PreviousBusinessRiskId = rdr.GetInt64(2);
                                }
                                else
                                {
                                    //// The previous BusinessRisk does not exist
                                    newRisk.PreviousBusinessRiskId = -1;
                                }

                                res.Add(newRisk);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
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

            return new ReadOnlyCollection<BusinessRisk>(res);
        }

        /// <summary>Gets a BusinessRisk object from database</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">BusinessRisk identifier</param>
        /// <returns>BusinessRisk object</returns>
        public static BusinessRisk ById(int companyId, long id)
        {
            var res = BusinessRisk.Empty;
            using (var cmd = new SqlCommand("BusinessRisk_GetById"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = new BusinessRisk
                                {
                                    Id = rdr.GetInt64(ColumnsBusinessRiskGetAll.Id),
                                    Description = rdr.GetString(ColumnsBusinessRiskGetAll.Description),
                                    Notes = rdr.GetString(ColumnsBusinessRiskGetAll.Notes),
                                    CompanyId = companyId,
                                    CreatedOn = rdr.GetDateTime(ColumnsBusinessRiskGetAll.CreatedOn),
                                    CreatedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsBusinessRiskGetAll.CreatedBy),
                                        UserName = rdr.GetString(ColumnsBusinessRiskGetAll.CreatedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsBusinessRiskGetAll.ModifiedOn),
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsBusinessRiskGetAll.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsBusinessRiskGetAll.ModifiedByName)
                                    },
                                    Code = rdr.GetInt64(ColumnsBusinessRiskGetAll.Code),
                                    Rules = new Rules()
                                    {
                                        Id = rdr.GetInt64(ColumnsBusinessRiskGetAll.RuleId),
                                        Description = rdr.GetString(ColumnsBusinessRiskGetAll.RuleDescription),
                                        Limit = rdr.GetInt64(ColumnsBusinessRiskGetAll.RuleRangeId)
                                    },
                                    ItemDescription = rdr.GetString(ColumnsBusinessRiskGetAll.ItemDescription),
                                    StartControl = rdr.GetString(ColumnsBusinessRiskGetAll.StartControl),
                                    Active = rdr.GetBoolean(ColumnsBusinessRiskGetAll.Active),
                                    //InitialValue = rdr.GetInt32(ColumnsBusinessRiskGetAll.InitialValue);
                                    DateStart = rdr.GetDateTime(ColumnsBusinessRiskGetAll.DateStart),
                                    ProcessId = rdr.GetInt64(ColumnsBusinessRiskGetAll.ProcessId),
                                    Assumed = rdr.GetBoolean(ColumnsBusinessRiskGetAll.Assumed),
                                    StartAction = rdr.GetInt32(ColumnsBusinessRiskGetAll.StartAction),
                                    StartProbability = rdr.GetInt32(ColumnsBusinessRiskGetAll.ProbabilityId),
                                    StartSeverity = rdr.GetInt32(ColumnsBusinessRiskGetAll.Severity),
                                    StartResult = rdr.GetInt32(ColumnsBusinessRiskGetAll.StartResult),
                                    FinalProbability = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalProbability),
                                    FinalSeverity = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalSeverity),
                                    FinalResult = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalResult),
                                    FinalAction = rdr.GetInt32(ColumnsBusinessRiskGetAll.FinalAction),
                                };

                                if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.FinalDate))
                                {
                                    res.FinalDate = rdr.GetDateTime(ColumnsBusinessRiskGetAll.FinalDate);
                                }

                                if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.Causes))
                                {
                                    res.Causes = rdr.GetString(ColumnsBusinessRiskGetAll.Causes);
                                }

                                //// Field can be Null in Database (no link to a previous BusinessRisk)
                                if (!rdr.IsDBNull(ColumnsBusinessRiskGetAll.PreviousBusinessRiskId))
                                {
                                    res.PreviousBusinessRiskId = rdr.GetInt64(ColumnsBusinessRiskGetAll.PreviousBusinessRiskId);
                                }
                                else
                                {
                                    //// The previous BusinessRisk does not exist
                                    res.PreviousBusinessRiskId = -1;
                                }

                                res.ModifiedBy.Employee = Employee.ByUserId(res.ModifiedBy.Id);
                            }
                        }

                        res.Process = new Process(res.ProcessId, companyId);
                    }
                    catch (Exception ex)
                    {
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

        /// <summary>Gets a descriptive string with the differences between two BusinessRisks</summary>
        /// <param name="other">BusinessRisks to compare</param>
        /// <returns>A descriptive string</returns>
        public string Differences(BusinessRisk other)
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

            if (this.Code != other.Code)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Code:").Append(other.Code);
                first = false;
            }

            if (this.Rules.Id != other.Rules.Id)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Rules:").Append(other.Rules.Id);
            }

            if (this.ItemDescription != other.ItemDescription)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("ItemDescription:").Append(other.ItemDescription);
            }

            if (this.Notes != other.Notes)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Notes:").Append(other.Notes);
            }

            if (this.Causes != other.Causes)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Causes:").Append(other.Causes);
            }

            if (this.Probability != other.Probability)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Probability:").Append(other.Probability);
            }

            if (this.Severity != other.Severity)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Severity:").Append(other.Severity);
            }

            if (this.Result != other.Result)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Result:").Append(other.Result);
            }

            if (this.ApplyAction != other.ApplyAction)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("ApplyAction:").Append(other.ApplyAction);
            }

            if (this.InitialValue != other.InitialValue)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("InitialValue:").Append(other.InitialValue);
            }

            if (this.DateStart != other.DateStart)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("DateStart:").Append(other.DateStart);
            }

            if (this.Process.Id != other.Process.Id)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("ProcessId:").Append(other.Process.Id);
            }

            if (this.Assumed != other.Assumed)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Assumed:").Append(other.Assumed);
            }

            if (this.PreviousBusinessRiskId != other.PreviousBusinessRiskId)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("PreviousBusinessRiskId:").Append(other.PreviousBusinessRiskId);
            }

            return res.ToString();
        }

        /// <summary>Delete a BusinessRisk in database</summary>
        /// <param name="businessRiskId">Business Risk identifier</param>
        /// <param name="reason">Reason for delete</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(long businessRiskId, string reason, int companyId, int userId, bool deleteAction = true)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("BusinessRisk_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", businessRiskId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@Reason", reason, 200));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();

                        if (deleteAction)
                        {
                            var victim = IncidentAction.ByBusinessRiskId(businessRiskId, companyId);
                            victim.Delete(userId);
                        }
                    }
                    catch (SqlException ex)
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

        /// <summary>Activate a BusinessRisk in database</summary>
        /// <param name="businessRiskId">Business Risk identifier</param>
        /// <param name="reason">Reason for activation</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Activate(long businessRiskId, string reason, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("BusinessRisk_Activate"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@BusinessRiskId", businessRiskId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@Reason", reason, 200));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
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

        /// <summary>Update a BusinessRisk in database</summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            /* CREATE PROCEDURE BusinessRisk_Update
             *   @Id bigint,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @Code int,
             *   @RuleId bigint,
             *   @ProcessId bigint,
             *   @ItemDescription text,
             *   @Notes text,
             *   @Causes text,
             *   @StartProbability int,
             *   @StartSeverity int,
             *   @StartResult int,
             *   @StartAction int,
             *   @InitialValue int,
             *   @DateStart datetime,
             *   @StartControl text,
             *   @FinalProbability int,
             *   @FinalSeverity int,
             *   @FinalResult int,
             *   @FinalAction int,
             *   @FinalDate datetime,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("BusinessRisk_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Code", this.Code));
                        cmd.Parameters.Add(DataParameter.Input("@RuleId", this.Rules.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDescription", this.ItemDescription, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@InitialValue", this.InitialValue));
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                        cmd.Parameters.Add(DataParameter.Input("@StartControl", this.StartControl, Constant.MaximumTextAreaLength));

                        cmd.Parameters.Add(DataParameter.Input("@StartProbability", this.StartProbability));
                        cmd.Parameters.Add(DataParameter.Input("@StartSeverity", this.StartSeverity));
                        cmd.Parameters.Add(DataParameter.Input("@startResult", this.StartResult));
                        cmd.Parameters.Add(DataParameter.Input("@StartAction", this.StartAction));
                        cmd.Parameters.Add(DataParameter.Input("@DateStart", this.DateStart));

                        cmd.Parameters.Add(DataParameter.Input("@FinalProbability", this.FinalProbability));
                        cmd.Parameters.Add(DataParameter.Input("@FinalSeverity", this.FinalSeverity));
                        cmd.Parameters.Add(DataParameter.Input("@FinalResult", this.FinalResult));
                        cmd.Parameters.Add(DataParameter.Input("@FinalAction", this.FinalAction));
                        cmd.Parameters.Add(DataParameter.Input("@FinalDate", this.FinalDate));
                        cmd.Parameters.Add(DataParameter.Input("@Assumed", this.Assumed));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(cmd.Parameters["@Id"].Value.ToString());
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
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

        /// <summary>Insert a BusinessRisk in database</summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE BusinessRisk_Update
             *   @Id bigint out,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @Code int,
             *   @RuleId bigint,
             *   @ProcessId bigint,
             *   @PreviousBusinessRiskId bigint,
             *   @ItemDescription text,
             *   @Notes text,
             *   @Causes text,
             *   @StartControl text,
             *   @StartProbability int,
             *   @StartSeverity int,
             *   @StartResult int,
             *   @StartAction bit,
             *   @UserId int,
             *   @DateStart datetime */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("BusinessRisk_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", Tools.LimitedText(this.Description, 100)));
                        cmd.Parameters.Add(DataParameter.Input("@Code", this.Code));
                        cmd.Parameters.Add(DataParameter.Input("@RuleId", this.Rules.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                        cmd.Parameters.Add(DataParameter.Input("@PreviousBusinessRiskId", this.PreviousBusinessRiskId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDescription", this.ItemDescription, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Causes", this.Causes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@StartControl", this.StartControl, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@StartProbability", this.StartProbability));
                        cmd.Parameters.Add(DataParameter.Input("@StartSeverity", this.StartSeverity));
                        cmd.Parameters.Add(DataParameter.Input("@StartResult", this.StartResult));
                        cmd.Parameters.Add(DataParameter.Input("@StartAction", this.StartAction));
                        cmd.Parameters.Add(DataParameter.Input("@DateStart", this.DateStart));
                        cmd.Parameters.Add(DataParameter.Input("@Assumed", this.Assumed));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(cmd.Parameters["@Id"].Value.ToString());
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString(), CultureInfo.InvariantCulture);

                        if (this.PreviousBusinessRiskId > 0)
                        {
                            BusinessRisk.Delete(this.PreviousBusinessRiskId, string.Empty, this.CompanyId, userId, false);
                        }
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
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

        /// <summary>HTML Table containing fields to be showed</summary>
        /// <param name="dictionary">Dictionary containing terms to be showed</param>
        /// <param name="grants">Gets the grants of the user</param>
        /// <returns>String containing HTML table</returns>
        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            bool grantBusinessRisk = UserGrant.HasWriteGrant(grants, ApplicationGrant.BusinessRisk);
            bool grantBusinessRiskDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.BusinessRisk);

            string iconEdit = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""BusinessRiskUpdate({0},'{1}');""><i class=""icon-edit bigger-120""></i></span>", this.Id, Tools.SetTooltip(this.Description), Tools.JsonCompliant(dictionary["Common_Edit"]));

            string iconDelete = string.Empty;
            if (grantBusinessRiskDelete)
            {
                string deleteAction = string.Empty;
                iconDelete = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""BusinessRiskDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", this.Id, Tools.SetTooltip(this.Description), Tools.JsonCompliant(dictionary["Common_Delete"]));
            }

            string initial = this.InitialValue == 0 ? string.Empty : this.InitialValue.ToString();

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr id=""{1}""><td>{0:dd/MM/yyyy}</td><td>{9}</td><td>{2}</td><td class=""hidden-480"">{3}</td><td class=""hidden-480""> {4}</td><td class=""hidden-480"" align=""right"">{5}&nbsp;</td><td class=""hidden-480"" align=""right"">{6}&nbsp;</td><td>{7} {8}</td></tr>",
                this.CreatedOn,
                this.Id,
                this.LinkList,
                UserGrant.HasWriteGrant(grants, ApplicationGrant.Process) ? this.Process.Link : this.Process.Description,
                UserGrant.HasWriteGrant(grants, ApplicationGrant.Rule) ? this.Rules.Link : this.Rules.Description,
                initial,
                this.Result,
                iconEdit,
                iconDelete,
                this.LinkCode);
        }

        /// <summary>Gets a Json containing all the items filtered</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="from">Start date to filter</param>
        /// <param name="to">Final date to filter</param>
        /// <param name="rulesId">Rules identifier to filter</param>
        /// <param name="processId">Process identifier to filter</param>
        /// <param name="type">Type to filter</param>
        /// <returns>String containing a Json</returns>
        public static string FilterList(int companyId, DateTime? from, DateTime? to, long rulesId, long processId, int type)
        {
            var items = Filter(companyId, from, to, rulesId, processId, type);
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
                    res.Append(",");
                }

                res.Append("{");
                res.Append(Tools.JsonPair("BusinessRiskId", item.Id)).Append(",");
                res.Append(Tools.JsonPair("OpenDate", item.OpenDate)).Append(",");
                res.Append(Tools.JsonPair("Description", item.Description)).Append(",");
                res.Append(Tools.JsonPair("Code", item.Code)).Append(",");
                res.Append(Tools.JsonPair("CloseDate", item.CloseDate)).Append(",");
                res.Append(Tools.JsonPair("Process", item.Process.JsonKeyValue)).Append(",");
                res.Append(Tools.JsonPair("Rules", item.Rule.JsonKeyValue)).Append(",");
                res.Append(Tools.JsonPair("StartAction", item.StartAction)).Append(",");
                res.Append(Tools.JsonPair("FinalAction", item.FinalAction)).Append(",");
                res.Append(Tools.JsonPair("StartResult", item.InitialResult)).Append(",");
                res.Append(Tools.JsonPair("FinalResult", item.FinalResult)).Append(",");
                res.Append(Tools.JsonPair("RuleLimit", item.Rule.Limit)).Append(",");
                res.Append(Tools.JsonPair("Assumed", item.Assumed)).Append("}");
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Gets a List containing all the items filtered</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="from">Start date to filter</param>
        /// <param name="to">Final date to filter</param>
        /// <param name="rulesId">Rules identifier to filter</param>
        /// <param name="processId">Process identifier to filter</param>
        /// <param name="type">Type to filter</param>
        public static ReadOnlyCollection<BusinessRiskFilterItem> NewFilter(int companyId, DateTime? from, DateTime? to, long rulesId, long processId, int type)
        {
            var res = new List<BusinessRiskFilterItem>();
            var risks = BusinessRisk.GetActive(companyId).ToList();

            if (from.HasValue)
            {
                risks = risks.Where(r => r.DateStart >= from).ToList();
            }

            if (to.HasValue)
            {
                risks = risks.Where(r => r.FinalDate >= to).ToList();
            }

            if (rulesId > 0)
            {
                risks = risks.Where(r => r.Rules.Id == rulesId).ToList();
            }

            if (processId > 0)
            {
                risks = risks.Where(r => r.ProcessId == processId).ToList();
            }

            if (type == 1)
            {
                risks = risks.Where(r => r.Assumed).ToList();
            }

            return new ReadOnlyCollection<BusinessRiskFilterItem>(res);
        }

        public static ReadOnlyCollection<BusinessRiskFilterItem> Filter(int companyId, DateTime? from, DateTime? to, long rulesId, long processId, int type)
        {
            /* Create PROCEDURE BusinessRisk_Filter
             *   @CompanyId int,
             *   @DateFrom datetime,
             *   @DateTo datetime,
             *   @RulesId bigint,
             *   @ProcessId bigint,
             *   @Type int */
            var res = new List<BusinessRiskFilterItem>();
            using (var cmd = new SqlCommand("BusinessRisk_Filter"))
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

                        if (type == 0)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@Type"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@Type", type));
                        }

                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new BusinessRiskFilterItem
                                {
                                    Id = rdr.GetInt64(ColumnsBusinessRiskFilterGet.BusinessRiskId),
                                    Description = rdr.GetString(ColumnsBusinessRiskFilterGet.Description),
                                    Code = string.Format(CultureInfo.InvariantCulture, "{0}", rdr.GetInt64(ColumnsBusinessRiskFilterGet.Code)),
                                    InitialResult = rdr.GetInt32(ColumnsBusinessRiskFilterGet.StartResult),
                                    FinalResult = rdr.GetInt32(ColumnsBusinessRiskFilterGet.FinalResult),
                                    StartAction = rdr.GetInt32(ColumnsBusinessRiskFilterGet.StartAction),
                                    FinalAction = rdr.GetInt32(ColumnsBusinessRiskFilterGet.FinalAction),
                                    Assumed = rdr.GetBoolean(ColumnsBusinessRiskFilterGet.Assumed),
                                    Status = rdr.GetInt32(ColumnsBusinessRiskFilterGet.Status)
                                };

                                if (!rdr.IsDBNull(ColumnsBusinessRiskFilterGet.RuleId))
                                {
                                    item.Rule = new Rules
                                    {
                                        Id = rdr.GetInt64(ColumnsBusinessRiskFilterGet.RuleId),
                                        Description = rdr.GetString(ColumnsBusinessRiskFilterGet.RuleDescription),
                                        Limit = rdr.GetInt64(ColumnsBusinessRiskFilterGet.Limit)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsBusinessRiskFilterGet.ProcessId))
                                {
                                    item.Process = new Process
                                    {
                                        Id = rdr.GetInt64(ColumnsBusinessRiskFilterGet.ProcessId),
                                        Description = rdr.GetString(ColumnsBusinessRiskFilterGet.ProcessDescription)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsBusinessRiskFilterGet.OpenDate))
                                {
                                    item.OpenDate = rdr.GetDateTime(ColumnsBusinessRiskFilterGet.OpenDate);
                                }

                                if (!rdr.IsDBNull(ColumnsBusinessRiskFilterGet.CloseDate))
                                {
                                    item.CloseDate = rdr.GetDateTime(ColumnsBusinessRiskFilterGet.CloseDate);
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

            if(type > 0)
            {
                res = res.Where(br => br.Status == type).ToList();
            }

            HttpContext.Current.Session["BusinessRiskFilterData"] = res;
            return new ReadOnlyCollection<BusinessRiskFilterItem>(res);
        }
    }
}