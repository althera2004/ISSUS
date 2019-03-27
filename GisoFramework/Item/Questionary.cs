// --------------------------------
// <copyright file="Questionary.cs" company="OpenFramework">
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
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    public class Questionary : BaseItem
    {
        public Rules Rule { get; set; }
        public Process Process { get; set; }
        public string ApartadoNorma { get; set; }
        public string Notes { get; set; }
        public int NQuestions { get; set; }

        public static Questionary Empty
        {
            get
            {
                return new Questionary
                {
                    Id = Constant.DefaultId,
                    Description = string.Empty,
                    Notes = string.Empty,
                    ApartadoNorma = string.Empty,
                    Rule = Rules.Empty,
                    Process = Process.Empty,
                    Active = false,
                    CanBeDeleted = true,
                    CompanyId = Constant.DefaultId,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    NQuestions = 0
                };
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
                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""CompanyId"":{2},
                        ""Rule"":{{""Id"":{3}, ""Description"": ""{4}""}},
                        ""Process"":{{""Id"":{5}, ""Description"": ""{6}""}},
                        ""ApartadoNorma"":""{7}"",
                        ""Notes"":""{8}"",
                        ""Active"":{9},
                        ""Deletable"":{10},
                        ""NQuestions"":{11}
                    }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.CompanyId,
                    this.Rule.Id,
                    Tools.JsonCompliant(this.Rule.Description),
                    this.Process.Id,
                    Tools.JsonCompliant(this.Process.Description),
                    Tools.JsonCompliant(this.ApartadoNorma),
                    Tools.JsonCompliant(this.Notes),
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false",
                    this.NQuestions);
            }
        }

        /// <summary>Gets a hyper link to questionary profile page</summary>
        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"<a href=""CuestionarioView.aspx?id={0}"" title=""{1}"">{1}</a>", this.Id, this.Description);
            }
        }

        /// <summary>Gets the structure json of item list</summary>
        /// <param name="list">List of items</param>
        /// <returns>Json list of items</returns>
        public static string JsonList(ReadOnlyCollection<Questionary> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var question in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(question.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Gets a descriptive string with the differences between two questionaries</summary>
        /// <param name="other">Second process to compare</param>
        /// <returns>A descriptive string</returns>
        public string Differences(Questionary other)
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

            if (this.Rule.Id != other.Rule.Id)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("rule:").Append(other.Rule.Id);
                first = false;
            }

            if (this.Process != other.Process)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("ProcessType:").Append(other.Process.Description);
            }

            if (this.ApartadoNorma != other.ApartadoNorma)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("ApartadoNorma:").Append(other.ApartadoNorma);
                first = false;
            }

            if (this.Notes != other.Notes)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Notes:").Append(other.Notes);
                first = false;
            }

            return res.ToString();
        }

        /// <summary>Insert the questionary into data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            /* ALTER PROCEDURE [userdbpre].[Cuestionario_Insert]
             *   @Id bigint output,
             *   @CompanyId int,
             *   @Description nvarchar(150),
             *   @NormaId bigint,
             *   @ProcessId bigint,
             *   @ApartadoNorma nvarchar(50),
             *   @Notes nvarchar(2000),
             *   @ApplicationUserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Cuestionario_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                        cmd.Parameters.Add(DataParameter.Input("@NormaId", this.Rule.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApartadoNorma", this.ApartadoNorma, Constant.DefaultDatabaseVarChar));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(cmd.Parameters["@Id"].Value.ToString());
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString(), CultureInfo.InvariantCulture);
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

        /// <summary>Updates the questionary into data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId, string differences)
        {
            /* CREATE PROCEDURE [dbo].[Cuestionario_Update]
             *   @Id bigint,
             *   @CompanyId int,
             *   @Description nvarchar(150),
             *   @NormaId bigint,
             *   @ProcessId bigint,
             *   @ApartadoNorma nvarchar(50),
             *   @Notes nvarchar(2000),
             *   @ApplicationUserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Cuestionario_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Process.Id));
                        cmd.Parameters.Add(DataParameter.Input("@NormaId", this.Rule.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApartadoNorma", this.ApartadoNorma, Constant.DefaultDatabaseVarChar));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(this.Id);
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

        public static ReadOnlyCollection<Questionary> All(int companyId)
        {
            var res = new List<Questionary>();
            var source = string.Format(CultureInfo.InvariantCulture, @"Questionary==>All({0}),", companyId);
            using (var cmd = new SqlCommand("Cuestionario_GetAll"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newQuestionary = new Questionary
                                {
                                    Id = rdr.GetInt64(ColumnsQuestionaryGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsQuestionaryGet.CompanyId),
                                    Description = rdr.GetString(ColumnsQuestionaryGet.Description),
                                    ApartadoNorma = rdr.GetString(ColumnsQuestionaryGet.ApartadoNorma),
                                    Notes = rdr.GetString(ColumnsQuestionaryGet.Notes),
                                    Rule = new Rules
                                    {
                                        Id = rdr.GetInt64(ColumnsQuestionaryGet.NormaId),
                                        Description = rdr.GetString(ColumnsQuestionaryGet.NormaDescription)
                                    },
                                    Process = new Process
                                    {
                                        Id = rdr.GetInt64(ColumnsQuestionaryGet.ProcessId),
                                        Description = rdr.GetString(ColumnsQuestionaryGet.ProcessDescription)
                                    },
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsQuestionaryGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsQuestionaryGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsQuestionaryGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsQuestionaryGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsQuestionaryGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsQuestionaryGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsQuestionaryGet.Active),
                                    NQuestions = rdr.GetInt32(ColumnsQuestionaryGet.NQuestions)
                                };

                                res.Add(newQuestionary);
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

            return new ReadOnlyCollection<Questionary>(res);
        }

        public static Questionary ById(long id, int companyId)
        {
            var res = Questionary.Empty;
            var source = string.Format(
                CultureInfo.InvariantCulture,
                @"Questionary==>ById({0},{1}),",
                id,
                companyId);

            using (var cmd = new SqlCommand("Cuestionario_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
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
                                res.Id = rdr.GetInt64(ColumnsQuestionaryGet.Id);
                                res.CompanyId = rdr.GetInt32(ColumnsQuestionaryGet.CompanyId);
                                res.Description = rdr.GetString(ColumnsQuestionaryGet.Description);
                                res.ApartadoNorma = rdr.GetString(ColumnsQuestionaryGet.ApartadoNorma);
                                res.Notes = rdr.GetString(ColumnsQuestionaryGet.Notes);
                                res.Rule = new Rules
                                {
                                    Id = rdr.GetInt64(ColumnsQuestionaryGet.NormaId),
                                    Description = rdr.GetString(ColumnsQuestionaryGet.Description)
                                };
                                res.Process = new Process
                                {
                                    Id = rdr.GetInt64(ColumnsQuestionaryGet.ProcessId),
                                    Description = rdr.GetString(ColumnsQuestionaryGet.ProcessDescription)
                                };
                                res.CreatedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsQuestionaryGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsQuestionaryGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsQuestionaryGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsQuestionaryGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsQuestionaryGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsQuestionaryGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsQuestionaryGet.Active);
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

            return res;
        }

        public static string FilterList(int companyId, long processId, long ruleId, string apartadoNorma)
        {
            var res = new List<Questionary>();
            var source = string.Format(CultureInfo.InvariantCulture, @"Questionary==>Filter({0}),", companyId);
            using (var cmd = new SqlCommand("Cuestionario_Filter"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ProcessId", processId));
                    cmd.Parameters.Add(DataParameter.Input("@RuleId", ruleId));
                    cmd.Parameters.Add(DataParameter.Input("@ApartadoNorma", apartadoNorma, 50));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newQuestionary = new Questionary
                                {
                                    Id = rdr.GetInt64(ColumnsQuestionaryGet.Id),
                                    Description = rdr.GetString(ColumnsQuestionaryGet.Description),
                                    ApartadoNorma = rdr.GetString(ColumnsQuestionaryGet.ApartadoNorma),
                                    Notes = rdr.GetString(ColumnsQuestionaryGet.Notes),
                                    Rule = new Rules
                                    {
                                        Id = rdr.GetInt64(ColumnsQuestionaryGet.NormaId),
                                        Description = rdr.GetString(ColumnsQuestionaryGet.NormaDescription)
                                    },
                                    Process = new Process
                                    {
                                        Id = rdr.GetInt64(ColumnsQuestionaryGet.ProcessId),
                                        Description = rdr.GetString(ColumnsQuestionaryGet.ProcessDescription)
                                    },
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsQuestionaryGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsQuestionaryGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsQuestionaryGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsQuestionaryGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsQuestionaryGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsQuestionaryGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsQuestionaryGet.Active),
                                    NQuestions = rdr.GetInt32(ColumnsQuestionaryGet.NQuestions)
                                };

                                res.Add(newQuestionary);
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

            return JsonList(new ReadOnlyCollection<Questionary>(res));
        }

        /// <summary>Activate questionary</summary>
        /// <param name="questionaryId">Questionary identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Action result</returns>
        public static ActionResult Activate(long questionaryId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Cuestionario_Activate"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@Id", questionaryId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
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

            return res;
        }

        /// <summary>Inactivate questionary</summary>
        /// <param name="questionaryId">Questionary identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Action result</returns>
        public static ActionResult Inactivate(long questionaryId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Cuestionario_Inactivate"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@Id", questionaryId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
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

            return res;
        }

        /// <summary>Inactive process</summary>
        /// <param name="questionaryId">Questionary identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Action result</returns>
        public static ActionResult Duplicate(long questionaryId,string description, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Cuestionario_Duplicate"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@Id", questionaryId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", description, 150));
                    cmd.Parameters.Add(DataParameter.OutputLong("@newId"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess(Convert.ToInt64(cmd.Parameters["@newId"].Value));
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
    }
}
