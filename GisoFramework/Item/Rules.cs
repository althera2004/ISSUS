// --------------------------------
// <copyright file="Rules.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
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

namespace GisoFramework.Item
{
    /// <summary>
    /// Item defining a rule
    /// </summary>
    public class Rules : BaseItem
    {
        /// <summary>Additional information on the rule</summary>
        public string Notes { get; set; }

        /// <summary>Specifies the limit of the rule</summary>
        public long Limit { get; set; }

        /// <summary>Specifies if the rule is unused (deletable from database)</summary>
        public Boolean Deletable { get; set; }

        /// <summary>Gets an empty instance of item</summary>
        public static Rules Empty
        {
            get
            {
                return new Rules()
                {
                    Id = -1,
                    Description = string.Empty,
                    Limit = 10, // ISSUS-226
                    Active = false,
                    CompanyId = -1,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Notes = string.Empty
                };
            }
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

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Rule);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Rule);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "RuleDelete({0},'{1}');", this.Id, this.Description);
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
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='RulesView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='RulesView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            /*if (grantWrite)
            {
                string validDescription = Tools.LiteralQuote(Tools.JsonCompliant(this.Description));
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "RuleDelete({0},'{1}');", this.Id, this.Description);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconEdit = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""document.location='RulesView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>", this.Id, validDescription, Tools.JsonCompliant(dictionary["Common_Edit"]));
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>", deleteFunction, validDescription, Tools.JsonCompliant(dictionary["Common_Delete"]));
            }*/

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr><td>{0}</td><td align=""right"" style=""width:100px;"">{1}</td><td style=""width:90px;"">{2}&nbsp;{3}</td></tr>",
                this.Link,
                this.Limit,
                iconEdit,
                iconDelete);
        }

        /// <summary>
        /// Gets an hyperlink to a detailed page of the rule (unused)
        /// </summary>
        public override string Link 
        {
            get {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<a href=""RulesView.aspx?id={0}"">{1}</a>",
                    this.Id,
                    this.Description);
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
                    this.Description
                );
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0},""Description"":""{1}"",""Notes"":""{2}"",""Limit"":{3}}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    Tools.JsonCompliant(this.Notes),
                    this.Limit
                );
            }
        }

        /// <summary>Gets a Json containing the relevant information of the rule for a BAR object</summary>
        public string JsonBAR
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0},""Description"":""{1}"",""Notes"":""{2}"",""Limit"":{3},""Editable"":""true"",""Deletable"":{4}}}",
                    this.Id,
                    this.Description,
                    this.Notes,
                    this.Limit,
                    this.Deletable? "true":"false"
                );
            }
        }

        public static string GetAllJson(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            ReadOnlyCollection<Rules> rules = GetActive(companyId);
            bool first = true;
            foreach (Rules rule in rules)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(rule.JsonKeyValue);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Returns Rules instances on the database
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of Rules</returns>
        public static ReadOnlyCollection<Rules> GetAll(int companyId)
        {
            return GetAll(companyId, false);
        }

        /// <summary>
        /// Returns Rules instances on the database
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="onlyActive">Set to return only active instances</param>
        /// <returns>List of Rules</returns>
        public static ReadOnlyCollection<Rules> GetAll(int companyId, bool onlyActive)
        {
            List<Rules> res = new List<Rules>();
            /* CREATE PROCEDURE [dbo].[Rules_GetAll]
             * @CompanyId int */

            string query = onlyActive ? "Rules_GetActive" : "Rules_GetAll";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        Rules newRule = new Rules()
                        {
                            Id = rdr.GetInt64(ColumnsRulesGetAll.Id),
                            Description = rdr.GetString(ColumnsRulesGetAll.Description),
                            Notes = rdr.GetString(ColumnsRulesGetAll.Notes),
                            CompanyId = companyId,
                            CreatedOn = rdr.GetDateTime(ColumnsRulesGetAll.CreatedOn),
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsRulesGetAll.CreatedBy),
                                UserName = rdr.GetString(ColumnsRulesGetAll.CreatedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsRulesGetAll.ModifiedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsRulesGetAll.ModifiedBy),
                                UserName = rdr.GetString(ColumnsRulesGetAll.ModifiedByName)
                            },
                            Active = rdr.GetBoolean(ColumnsRulesGetAll.Active),
                            Limit = rdr.GetInt64(ColumnsRulesGetAll.Limit),
                            CanBeDeleted = rdr.GetInt32(ColumnsRulesGetAll.CanBeDeleted) == 1
                        };

                        res.Add(newRule);
                    }
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"Ruless::GetAll({0})", companyId));
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"Ruless::GetAll({0})", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"Ruless::GetAll({0})", companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"Ruless::GetAll({0})", companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"Ruless::GetAll({0})", companyId));
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"Ruless::GetAll({0})", companyId));
                }
                finally
                {
                    if(cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<Rules>(res);
        }

        /// <summary>
        /// Returns active Rules insantces from database
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of Rules</returns>
        public static ReadOnlyCollection<Rules> GetActive(int companyId)
        {
            return GetAll(companyId, true);
        }

        /// <summary>
        /// Returns a list of Rules containing information for a BAR object
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of Rules</returns>
        public static ReadOnlyCollection<Rules> GetBar(int companyId)
        {
            List<Rules> res = new List<Rules>();
            /* CREATE PROCEDURE [dbo].[Rules_GetAll]
             * @CompanyId int */

            string query = "Rules_GetBAR";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                try
                {

                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Rules newRule = new Rules()
                        {
                            Id = rdr.GetInt64(ColumnsRulesGetBAR.Id),
                            Description = rdr.GetString(ColumnsRulesGetBAR.Description),
                            Notes = rdr.GetString(ColumnsRulesGetBAR.Notes),
                            Limit = rdr.GetInt64(ColumnsRulesGetBAR.Limit),
                            Deletable = rdr.GetBoolean(ColumnsRulesGetBAR.Deletable)
                        };

                        res.Add(newRule);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"GetBar({0})", companyId));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"GetBar({0})", companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"GetBar({0})", companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"GetBar({0})", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"GetBar({0})", companyId));
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"GetBar({0})", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<Rules>(res);
        }

        /// <summary>
        /// Returns a determined Rules object
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">Object identifier</param>
        /// <returns>Rules object</returns>
        public static Rules GetById(int companyId, long id)
        {
            string source = string.Format(CultureInfo.GetCultureInfo("en-us"), @"GetById({0},{1})", companyId, id);
            Rules res = Rules.Empty;
            /* CREATE PROCEDURE [dbo].[Rules_GetAll]
             * @CompanyId int */

            using (SqlCommand cmd = new SqlCommand("Rules_GetById"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@Id", id));
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res = new Rules()
                        {
                            Id = rdr.GetInt64(ColumnsRulesGetById.Id),
                            Description = rdr.GetString(ColumnsRulesGetById.Description),
                            Notes = rdr.GetString(ColumnsRulesGetById.Notes),
                            CompanyId = companyId,
                            CreatedOn = rdr.GetDateTime(ColumnsRulesGetById.CreatedOn),
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsRulesGetById.CreatedBy),
                                UserName = rdr.GetString(ColumnsRulesGetById.CreatedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsRulesGetById.ModifiedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsRulesGetById.ModifiedBy),
                                UserName = rdr.GetString(ColumnsRulesGetById.ModifiedByName)
                            },
                            Active = rdr.GetBoolean(ColumnsRulesGetById.Active),
                            Limit = rdr.GetInt64(ColumnsRulesGetById.Limit)
                        };

                        res.ModifiedBy.Employee = Employee.ByUserId(res.ModifiedBy.Id);
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
        /// <summary>
        /// Insert Rule into database
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Rules_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 500));
                    cmd.Parameters.Add(DataParameter.Input("@Limit", this.Limit));

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value, CultureInfo.GetCultureInfo("en-us"));
                    result.SetSuccess(this.Id);
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "Rules::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Rules::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Update a Rule in database
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            ActionResult result = new ActionResult() { Success = false, MessageError = "No action" };
            using (SqlCommand cmd = new SqlCommand("Rules_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 500));
                    cmd.Parameters.Add(DataParameter.Input("@Limit", this.Limit));

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.Success = true;
                    result.MessageError = string.Empty;
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "Rules::Update", string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Rules::Update", string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Delete a Rule in database
        /// </summary>
        /// <param name="ruleId">Rule identifier</param>
        /// <param name="reason">Reason for delete</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int ruleId, string reason, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Rules_Delete
             *   @RulesId int,
             *   @CompanyId int,
             *   @Reason nvarchar(200),
             *   @UserId int */
            using (SqlCommand cmd = new SqlCommand("Rules_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@RulesId", ruleId));
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

            return res;
        }

        /// <summary>
        /// Activate a Rule in database
        /// </summary>
        /// <param name="ruleId">Rule identifier</param>
        /// <param name="reason">Reason for delete</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Activate(int ruleId, string reason, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Rules_Delete
             *   @RulesId int,
             *   @CompanyId int,
             *   @Reason nvarchar(200),
             *   @UserId int */
            using (SqlCommand cmd = new SqlCommand("Rules_Active"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@RulesId", ruleId));
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

            return res;
        }

        /// <summary>
        /// Gets a descriptive string with the differences between two Rules
        /// </summary>
        /// <param name="rule1">First Rule to compare</param>
        /// <param name="rule2">Second Rule to compare</param>
        /// <returns>A descriptive string</returns>
        public static string Differences(Rules rule1, Rules rule2)
        {
            if (rule1 == null || rule2 == null)
            {
                return string.Empty;
            }

            StringBuilder res = new StringBuilder();
            bool first = true;
            if (rule1.Description != rule2.Description)
            {
                res.Append("Description:").Append(rule2.Description);
                first = false;
            }

            if (rule1.Notes != rule2.Notes)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Notes:").Append(rule2.Notes);
                first = false;
            }

            if (rule1.Limit != rule2.Limit)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Limit:").Append(rule2.Limit);
            }
            return res.ToString();
        }

        /// <summary>
        /// Gets a descriptive string with the differences between this and another Rule item
        /// </summary>
        /// <param name="rule">Rule to compare with</param>
        /// <returns>A descriptive string</returns>
        public string Differences(Rules rule)
        {
            return Rules.Differences(this, rule);
        }

        public ActionResult SetLimit(long applicationUserId)
        {
            /*CREATE PROCEDURE Rules_SetLimit
             *   @Id bigint out,
             *   @CompanyId int,
             *   @Limit int,
             *   @UserId int*/
            ActionResult result = new ActionResult() { Success = false, MessageError = "No action" };
            using (SqlCommand cmd = new SqlCommand("Rules_SetLimit"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", applicationUserId));
                    cmd.Parameters.Add(DataParameter.Input("@Limit", this.Limit));

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.Success = true;
                    result.MessageError = string.Empty;
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "Rules::Update", string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Rules::Update", string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }
    }
}
