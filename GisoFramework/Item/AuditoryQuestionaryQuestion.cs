// --------------------------------
// <copyright file="AuditoryQuestionaryQuestion.cs" company="OpenFramework">
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

    public class AuditoryQuestionaryQuestion : BaseItem
    {
        public long QuestionaryId { get; set; }
        public long AuditoryId { get; set; }
        public int? Compliant { get; set; }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                var compliantValue = Constant.JavaScriptNull;
                if (this.Compliant.HasValue)
                {
                    compliantValue = this.Compliant.Value.ToString();
                }
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Question"":""{1}"", ""Compliant"":{2}}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    compliantValue);
                    //this.Compliant.HasValue ? (this.Compliant.Value ? Constant.JavaScriptTrue : Constant.JavaScriptFalse) : Constant.JavaScriptNull);
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var compliantValue = Constant.JavaScriptNull;
                if (this.Compliant.HasValue)
                {
                    compliantValue = this.Compliant.Value.ToString();
                }

                string pattern = @"{{
                        ""Id"":{0},
                        ""Question"":""{1}"",
                        ""Compliant"":{1},
                        ""QuestionaryId"":{2},
                        ""AuditoryId"":{2},
                        ""CompanyId"":{3},
                        ""Active"":{4}
                    }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    //this.Compliant.HasValue ? (this.Compliant.Value ? Constant.JavaScriptTrue : Constant.JavaScriptFalse) : Constant.JavaScriptNull,
                    compliantValue,
                    this.QuestionaryId,
                    this.AuditoryId,
                    this.CompanyId,
                    this.Active ? "true" : "false");
            }
        }

        /// <summary>Gets a hyper link to questionary question profile page</summary>
        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        public static string ByAuditoryIdJson(long auditoryId, long cuestionarioId, int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var question in ByAuditoryId(auditoryId, cuestionarioId, companyId))
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

        public static ReadOnlyCollection<AuditoryQuestionaryQuestion> ByAuditoryId(long auditoryId, long cuestionarioId, int companyId)
        {
            var res = new List<AuditoryQuestionaryQuestion>();
            using (var cmd = new SqlCommand("Auditory_GetQuestions"))
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
                                var newAuditoryQuestionaryQuestion = new AuditoryQuestionaryQuestion
                                {
                                    Id = rdr.GetInt64(ColumnsAuditoryQuestionGet.Id),
                                    CompanyId = companyId,
                                    Description = rdr.GetString(ColumnsAuditoryQuestionGet.Question),
                                    QuestionaryId = rdr.GetInt64(ColumnsAuditoryQuestionGet.CuestionarioId),
                                    AuditoryId = rdr.GetInt64(ColumnsAuditoryQuestionGet.AuditoryId)
                                };

                                if (!rdr.IsDBNull(ColumnsAuditoryQuestionGet.Compliant))
                                {
                                    newAuditoryQuestionaryQuestion.Compliant = rdr.GetInt32(ColumnsAuditoryQuestionGet.Compliant);
                                }

                                res.Add(newAuditoryQuestionaryQuestion);
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

            return new ReadOnlyCollection<AuditoryQuestionaryQuestion>(res);
        }

        /// <summary>Insert the question into data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("CuestionarioPregunta_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@Question", this.Description, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@CuestionarioId", this.QuestionaryId));
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

        /// <summary>Updates the question into data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("CuestionarioPregunta_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Question", this.Description, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@CuestionarioId", this.QuestionaryId));
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

        /// <summary>Activate question</summary>
        /// <param name="questionaryQuestionId">Questionary identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Action result</returns>
        public static ActionResult Activate(long questionaryQuestionId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("CuestionarioPregunta_Activate"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@Id", questionaryQuestionId));
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

        /// <summary>Inactivate question</summary>
        /// <param name="questionaryQuestionId">Questionary identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Action result</returns>
        public static ActionResult Inactivate(long questionaryQuestionId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("CuestionarioPregunta_Inactivate"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@Id", questionaryQuestionId));
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
    }
}