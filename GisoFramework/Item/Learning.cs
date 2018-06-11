// --------------------------------
// <copyright file="Learning.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
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
    using System.Web;
    using GisoFramework;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>
    /// Implementation of Learning class
    /// </summary>
    public class Learning : BaseItem
    {
        /// <summary>
        /// Assistance of learning
        /// </summary>
        private List<LearningAssistance> assistance;

        /// <summary>
        /// Initializes a new instance of the Learning class.
        /// </summary>
        public Learning()
        {
            this.Hours = 0;
            this.Amount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the Learning class.
        /// Obtains learning data from database based on learning identifier and company identifier
        /// </summary>
        /// <param name="learningId">Learning identifier</param>
        /// <param name="companyId">Company identifier</param>
        public Learning(int learningId, int companyId)
        {
            this.Hours = 0;
            this.Amount = 0;
            using (var cmd = new SqlCommand("Learning_GetById"))
            {
                /* CREATE PROCEDURE Learning_GetById
                 * @LearningId int,
                 * @CompanyId int */
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@LearningId", learningId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                this.Id = learningId;
                                this.CompanyId = companyId;
                                this.Amount = rdr.GetDecimal(ColumnsLearningGetById.Amount);
                                this.Description = rdr.GetString(ColumnsLearningGetById.Description);
                                this.Notes = rdr.GetString(ColumnsLearningGetById.Notes);
                                this.DateEstimated = rdr.GetDateTime(ColumnsLearningGetById.DateStimatedDate);
                                this.Hours = rdr.GetInt64(ColumnsLearningGetById.Hours);
                                this.Master = rdr.GetString(ColumnsLearningGetById.Master);
                                this.Year = rdr.GetInt32(ColumnsLearningGetById.Year);
                                this.Status = rdr.GetInt32(ColumnsLearningGetById.Status);
                                this.Objective = rdr.GetString(ColumnsLearningGetById.Objective);
                                this.Methodology = rdr.GetString(ColumnsLearningGetById.Methodology);
                                this.ModifiedOn = rdr.GetDateTime(ColumnsLearningGetById.ModifiedOn);
                                this.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsLearningGetById.ModifiedByUserId),
                                    UserName = rdr.GetString(ColumnsLearningGetById.ModifiedByUserName)
                                };

                                if (!rdr.IsDBNull(ColumnsLearningGetById.RealStart))
                                {
                                    this.RealStart = rdr.GetDateTime(ColumnsLearningGetById.RealStart);
                                }

                                if (!rdr.IsDBNull(ColumnsLearningGetById.RealFinish))
                                {
                                    this.RealFinish = rdr.GetDateTime(ColumnsLearningGetById.RealFinish);
                                }

                                this.ModifiedBy.Employee = Employee.ByUserId(this.ModifiedBy.Id);
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
        }

        /// <summary>Gets a empty learning with default values</summary>
        public static Learning Empty
        {
            get
            {
                return new Learning
                {
                    ModifiedBy = ApplicationUser.Empty,
                    Hours = 0,
                    Amount = 0
                };
            }
        }

        /// <summary>
        /// Gets or sets status of learning
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets estimated date of learning
        /// </summary>
        public DateTime DateEstimated { get; set; }

        /// <summary>
        /// Gets or sets real start date
        /// </summary>
        public DateTime? RealStart { get; set; }

        /// <summary>
        /// Gets or sets real finish date
        /// </summary>
        public DateTime? RealFinish { get; set; }

        /// <summary>
        /// Gets or sets master of learning
        /// </summary>
        public string Master { get; set; }

        /// <summary>
        /// Gets or sets number of hours of learning
        /// </summary>
        public long Hours { get; set; }

        /// <summary>
        /// Gets or sets the total amount of learning
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets notes for learning
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets  the year of learning action
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the objective of learning
        /// </summary>
        public string Objective { get; set; }

        /// <summary>
        /// Gets or sets the methodology of learning
        /// </summary>
        public string Methodology { get; set; }

        /// <summary>
        /// Gets the learning assistance
        /// </summary>
        public ReadOnlyCollection<LearningAssistance> Assistance
        {
            get
            {
                if (this.assistance == null)
                {
                    this.assistance = new List<LearningAssistance>();
                }

                return new ReadOnlyCollection<LearningAssistance>(this.assistance);
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var assistanceString = new StringBuilder("[");
                bool first = true;
                foreach (var item in this.assistance)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        assistanceString.Append(",");
                    }

                    assistanceString.Append(item.Json);
                }

                assistanceString.Append("]");

                string realStart = "null";
                string realFinish = "null";

                if (this.RealStart.HasValue)
                {
                    realStart = string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0:dd/MM/yyyy}""", this.RealStart);
                }

                if (this.RealFinish.HasValue)
                {
                    realFinish = string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0:dd/MM/yyyy}""", this.RealFinish);
                }

                string amount = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:##0.00}", this.Amount);

                string pattern = @"
                    {{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""Description"":""{2}"",
                        ""DateEstimated"":""{3:dd/MM/yyyy}"",
                        ""Hours"":{4},
                        ""Amount"":{5:#0,00},
                        ""Master"":""{6}"",
                        ""Notes"":""{7}"",
                        ""Status"":{8},
                        ""Year"":{9},
                        ""RealStart"":{10},
                        ""RealFinish"":{11},
                        ""Objective"":""{12}"",
                        ""Methodology"":""{13}"",
                        ""EstimatedMonthName"":""{14}"",
                        ""EstimatedMonthId"": {15}
                    }}";

                return string.Format(
                    CultureInfo.GetCultureInfo("es-es"),
                    pattern,
                    this.Id,
                    this.CompanyId,
                    Tools.JsonCompliant(this.Description),
                    this.DateEstimated,
                    this.Hours,
                    amount,
                    Tools.JsonCompliant(this.Master),
                    Tools.JsonCompliant(this.Notes),
                    this.Status,
                    this.Year,
                    realStart,
                    realFinish,
                    Tools.JsonCompliant(this.Objective),
                    Tools.JsonCompliant(this.Methodology),
                    Tools.TranslatedMonth(this.DateEstimated.Month, HttpContext.Current.Session["Dictionary"] as Dictionary<string,string>),
                    this.DateEstimated.Month);
            }
        }

        /// <summary>
        /// Gets a link to learing profile
        /// </summary>
        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<a href=""FormacionView.aspx?id={0}"">{1}</a>", this.Id, this.Description);
            }
        }

        /// <summary>
        /// Gets differences of two learning objects
        /// </summary>
        /// <param name="oldLearning">Old learning to compare</param>
        /// <param name="newLearning">New learning to compare</param>
        /// <returns>String with key value pairs of differences</returns>
        public static string Differences(Learning oldLearning, Learning newLearning)
        {
            if (oldLearning == null || newLearning == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            bool first = true;
            if (oldLearning.Description != newLearning.Description)
            {
                first = false;
                res.Append(",");
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"Description:{0}", newLearning.Description));
            }

            if (oldLearning.Status != newLearning.Status)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"Status:{0}", newLearning.Status));
            }

            if (oldLearning.DateEstimated != newLearning.DateEstimated)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"DateEstimated:{0:dd/MM/yyyy}", newLearning.DateEstimated));
            }

            if (oldLearning.RealStart != newLearning.RealStart)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"RealStart:{0:dd/MM/yyyy}", newLearning.RealStart));
            }

            if (oldLearning.RealFinish != newLearning.RealFinish)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"RealFinish:{0:dd/MM/yyyy}", newLearning.RealFinish));
            }

            if (oldLearning.Hours != newLearning.Hours)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"Hours:{0}", newLearning.Hours));
            }

            if (oldLearning.Amount != newLearning.Amount)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"Amount:{0}", newLearning.Amount));
            }

            if (oldLearning.Master != newLearning.Master)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"Master:{0}", newLearning.Master));
            }

            if (oldLearning.Notes != newLearning.Notes)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"Notes:{0}", newLearning.Notes));
            }

            if (oldLearning.Year != newLearning.Year)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, @"Year:{0}", newLearning.Year));
            }

            return res.ToString();
        }

        /// <summary>Delete the learning</summary>
        /// <param name="learningId">Learning identififer</param>
        /// <param name="companyId">Identifier of learning's compnay</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="reason">Reason for delete</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int learningId, int companyId, int userId, string reason)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE Learning_Delete
             * @LearningId int,
             * @CompanyId int,
             * @Reason nvarchar(200),
             * @UserId int */
            using (var cmd = new SqlCommand("Learning_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@LearningId", learningId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@Reason", reason, 200));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        /// <summary>Gets a row of learning for learnings list table</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="admin">Indicates if user that views table has administration role</param>
        /// <returns>Html code to render learning row</returns>
        public string ListRow(Dictionary<string, string> dictionary, bool admin)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            // this.ObtainAssistance();

            string month = Tools.TranslatedMonth(this.DateEstimated.Month, dictionary);

            var res = new StringBuilder();
            /*List<LearningAssistance> succededAssistants = new List<LearningAssistance>();
            List<LearningAssistance> completedAssistants = new List<LearningAssistance>();
            List<LearningAssistance> uncompletesAssistants = new List<LearningAssistance>();
            List<LearningAssistance> preAssistants = new List<LearningAssistance>();

            StringBuilder succededAssistantsText = new StringBuilder();
            StringBuilder completedAssistantsText = new StringBuilder();
            StringBuilder uncompletedAssistantsText = new StringBuilder();
            StringBuilder preAssistantsText = new StringBuilder();

            foreach (LearningAssistance assistanceItem in this.Assistance)
            {
                if (!assistanceItem.Completed.HasValue)
                {
                    preAssistants.Add(assistanceItem);
                }
                else
                {
                    if (assistanceItem.Success.HasValue)
                    {
                        if (assistanceItem.Success.Value)
                        {
                            succededAssistants.Add(assistanceItem);
                        }
                        else
                        {
                            if (assistanceItem.Completed.Value)
                            {
                                completedAssistants.Add(assistanceItem);
                            }
                            else
                            {
                                uncompletesAssistants.Add(assistanceItem);
                            }
                        }
                    }
                    else
                    {
                        if (assistanceItem.Completed.Value)
                        {
                            completedAssistants.Add(assistanceItem);
                        }
                        else
                        {
                            uncompletesAssistants.Add(assistanceItem);
                        }
                    }
                }
            }

            foreach (LearningAssistance successAssistance in succededAssistants)
            {
                succededAssistantsText.Append(successAssistance.Employee.LearningTag(dictionary, admin));
            }

            foreach (LearningAssistance completedAssistance in completedAssistants)
            {
                completedAssistantsText.Append(completedAssistance.Employee.LearningTag(dictionary, admin));
            }

            foreach (LearningAssistance uncompletedAssistance in uncompletesAssistants)
            {
                uncompletedAssistantsText.Append(uncompletedAssistance.Employee.LearningTag(dictionary, admin));
            }

            foreach (LearningAssistance preAssistance in preAssistants)
            {
                preAssistantsText.Append(preAssistance.Employee.LearningTag(dictionary, admin));
            }*/

            string iconDeleteAction = "LearningDelete";

            if (this.Status > 0)
            {
                switch (this.Status)
                {
                    case 1:
                        iconDeleteAction = "LearningDeleteDisabled1";
                        break;
                    case 2:
                        iconDeleteAction = "LearningDeleteDisabled2";
                        break;
                    default:
                        iconDeleteAction = "LearningDeleteDisabled";
                        break;
                }
            }

            // @alex: al poner la descripcion sustiuir ' por \' para evitar un javascript mal formado 
            string iconUpdate = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-info"" onclick=""LearningUpdate({0});""><i class=""icon-edit bigger-120""></i></span>", this.Id, this.Description, dictionary["Common_Edit"]);
            this.Description = this.Description.Replace('\'', '´');
            string iconDelete = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""{3}({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", this.Id, this.Description, dictionary["Common_Delete"], iconDeleteAction);

            res.Append("<tr>");
            res.Append("<td>").Append(this.Link).Append("</td>");
            res.Append("<td align=\"center\" style=\"width:100px;white-space: nowrap;\">");
            res.AppendFormat(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.DateEstimated);
            //res.Append("<td align=\"center\" class=\"hidden-480\">").Append(month).Append(" ").Append(this.DateEstimated.Year).Append("</td>");
            res.Append("<td align=\"center\" style=\"width:100px;white-space: nowrap;\">");
            res.AppendFormat(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.RealFinish);

            /*bool existsAssistants = false;
            if (succededAssistantsText.ToString().Length > 0)
            {
                res.Append(dictionary["Item_LearningAssistant_Status_Evaluated"]).Append("<div class=\"tags\">");
                res.Append(succededAssistantsText).Append("</div>");
                existsAssistants = true;
            }

            if (completedAssistantsText.ToString().Length > 0)
            {
                res.Append(dictionary["Item_LearningAssistant_Status_Done"]).Append("<div class=\"tags\">");
                res.Append(completedAssistantsText).Append("</div>");
                existsAssistants = true;
            }

            if (uncompletedAssistantsText.ToString().Length > 0)
            {
                res.Append(dictionary["Common_No"]).Append(" ").Append(dictionary["Item_LearningAssistant_Status_Done"].ToLower(CultureInfo.GetCultureInfo("en-us"))).Append("<div class=\"tags\">");
                res.Append(uncompletedAssistantsText).Append("</div>");
                existsAssistants = true;
            }

            if (preAssistantsText.ToString().Length > 0)
            {
                res.Append(dictionary["Item_LearningAssistant_Status_Selected"]).Append("<div class=\"tags\">");
                res.Append(preAssistantsText).Append("</div>");
                existsAssistants = true;
            }

            if (existsAssistants == false)
            {
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<div style=""padding:8px;""><i>{0}</i></div>",
                    dictionary["Item_Learning_Message_NoAssistants"]);
            }*/

            string amountText = string.Format(CultureInfo.GetCultureInfo("es-e"), "{0:#,##0.00}", this.Amount);

            //string import = string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", this.Amount).Replace(".", ",");


            res.Append("</td>");

            string statusText = string.Empty;
            switch (this.Status)
            {
                case 0:
                    statusText = dictionary["Item_Learning_Status_InProgress"];
                    break;
                case 1:
                    statusText = dictionary["Item_Learning_Status_Finished"];
                    break;
                case 2:
                    statusText = dictionary["Item_Learning_Status_Evaluated"];
                    break;
                default:
                    statusText = string.Empty;
                    break;
            }

            res.Append("<td align=\"center\" class=\"hidden-480\" style=\"width:100px;white-space: nowrap;\">").Append(statusText).Append("</td>");
            res.Append("<td align=\"right\" class=\"hidden-480\" style=\"width:150px;white-space: nowrap;\">").Append(amountText).Append("</td>");
            res.Append("<td class=\"hidden-480\" style=\"width:90px;white-space: nowrap;\">");
            res.Append(iconUpdate).Append("&nbsp;").Append(iconDelete);
            res.Append("</td>");
            res.Append("</tr>");
            return res.ToString();
        }

        /// <summary>
        /// Insert a learning into database
        /// </summary>
        /// <param name="userId">Identifier of user that makes actions</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Learning_Insert
             * @LearningId int out,
             * @CompanyId int,
             * @Description nvarchar(100),
             * @Status int,
             * @DateStimatedDate date,
             * @RealStart date,
             * @RealFinish date,
             * @Master nvarchar(100),
             * @Hours int,
             * @Amount numeric(18,3),
             * @Notes text,
             * @UserId int,
             * @Year int */
            using (var cmd = new SqlCommand("Learning_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@LearningId", SqlDbType.Int);
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Status", SqlDbType.Int);
                        cmd.Parameters.Add("@DateStimatedDate", SqlDbType.Date);
                        cmd.Parameters.Add("@RealStart", SqlDbType.Date);
                        cmd.Parameters.Add("@RealFinish", SqlDbType.Date);
                        cmd.Parameters.Add("@Master", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Hours", SqlDbType.Int);
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal);
                        cmd.Parameters.Add("@Notes", SqlDbType.Text);
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);
                        cmd.Parameters.Add("@Year", SqlDbType.Int);
                        cmd.Parameters.Add("@Objetivo", SqlDbType.Text);
                        cmd.Parameters.Add("@Metodologia", SqlDbType.Text);
                        cmd.Parameters["@LearningId"].Value = DBNull.Value;
                        cmd.Parameters["@LearningId"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                        cmd.Parameters["@Description"].Value = Tools.LimitedText(this.Description, 100);
                        cmd.Parameters["@Status"].Value = this.Status;
                        cmd.Parameters["@DateStimatedDate"].Value = this.DateEstimated;
                        cmd.Parameters["@Master"].Value = Tools.LimitedText(this.Master, 100);
                        cmd.Parameters["@Hours"].Value = this.Hours;
                        cmd.Parameters["@Amount"].Value = this.Amount;
                        cmd.Parameters["@Notes"].Value = Tools.LimitedText(this.Notes, 2000);
                        cmd.Parameters["@Year"].Value = this.Year;
                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.Parameters["@Objetivo"].Value = Tools.LimitedText(this.Objective, 2000);
                        cmd.Parameters["@Metodologia"].Value = Tools.LimitedText(this.Methodology, 2000);
                        if (this.RealStart.HasValue)
                        {
                            cmd.Parameters["@RealStart"].Value = this.RealStart.Value;
                        }
                        else
                        {
                            cmd.Parameters["@RealStart"].Value = DBNull.Value;
                        }

                        if (this.RealFinish.HasValue)
                        {
                            cmd.Parameters["@RealFinish"].Value = this.RealFinish.Value;
                        }
                        else
                        {
                            cmd.Parameters["@RealFinish"].Value = DBNull.Value;
                        }

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@LearningId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "{0}", cmd.Parameters["@LearningId"].Value.ToString()));
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

        /// <summary>Update learning data in database</summary>
        /// <param name="userId">Identifier of user that makes actions</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Learning_Update
             * @CompanyId int,
             * @Description nvarchar(100),
             * @Status int,
             * @DateStimatedDate date,
             * @RealStart date,
             * @RealFinish date,
             * @Master nvarchar(100),
             * @Hours int,
             * @Amount numeric(18,3),
             * @Notes text,
             * @UserId int,
             * @Year int */
            using (var cmd = new SqlCommand("Learning_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@LearningId", SqlDbType.Int);
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Status", SqlDbType.Int);
                        cmd.Parameters.Add("@DateStimatedDate", SqlDbType.Date);
                        cmd.Parameters.Add("@RealStart", SqlDbType.Date);
                        cmd.Parameters.Add("@RealFinish", SqlDbType.Date);
                        cmd.Parameters.Add("@Master", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Hours", SqlDbType.Int);
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal);
                        cmd.Parameters.Add("@Notes", SqlDbType.Text);
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);
                        cmd.Parameters.Add("@Year", SqlDbType.Int);
                        cmd.Parameters.Add("@Objetivo", SqlDbType.Text);
                        cmd.Parameters.Add("@Metodologia", SqlDbType.Text);
                        cmd.Parameters["@LearningId"].Value = this.Id;
                        cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                        cmd.Parameters["@Description"].Value = Tools.LimitedText(this.Description, 2000);
                        cmd.Parameters["@Status"].Value = this.Status;
                        cmd.Parameters["@DateStimatedDate"].Value = this.DateEstimated;
                        cmd.Parameters["@Master"].Value = Tools.LimitedText(this.Master, 100);
                        cmd.Parameters["@Hours"].Value = this.Hours;
                        cmd.Parameters["@Amount"].Value = this.Amount;
                        cmd.Parameters["@Notes"].Value = this.Notes;
                        cmd.Parameters["@Year"].Value = this.Year;
                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.Parameters["@Objetivo"].Value = Tools.LimitedText(this.Objective, 2000);
                        cmd.Parameters["@Metodologia"].Value = Tools.LimitedText(this.Methodology, 2000);
                        if (this.RealStart.HasValue)
                        {
                            cmd.Parameters["@RealStart"].Value = this.RealStart.Value;
                        }
                        else
                        {
                            cmd.Parameters["@RealStart"].Value = DBNull.Value;
                        }

                        if (this.RealFinish.HasValue)
                        {
                            cmd.Parameters["@RealFinish"].Value = this.RealFinish.Value;
                        }
                        else
                        {
                            cmd.Parameters["@RealFinish"].Value = DBNull.Value;
                        }

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

        /// <summary>Obtain assistance of learning</summary>
        public void ObtainAssistance()
        {
            this.assistance = new List<LearningAssistance>();
            using (var cmd = new SqlCommand("Learning_GetAssistance"))
            {
                /* CREATE PROCEDURE [dbo].[Learning_GetAssistance]
                 * @LearningId int,
                 * @CompanyId int */
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@LearningId", SqlDbType.Int);
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters["@LearningId"].Value = this.Id;
                        cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var jobPosition = JobPosition.Empty;
                                if (rdr.GetInt32(ColumnsLearningGetAssistance.JobPositionId) != 0)
                                {
                                    jobPosition.Id = rdr.GetInt32(ColumnsLearningGetAssistance.JobPositionId);
                                    jobPosition.Description = rdr.GetString(ColumnsLearningGetAssistance.JobPositionDescription);
                                }

                                var newAssistance = new LearningAssistance
                                {
                                    CompanyId = this.CompanyId,
                                    Id = rdr.GetInt32(ColumnsLearningGetAssistance.LearningAssistanceId),
                                    Date = rdr.GetDateTime(ColumnsLearningGetAssistance.DateEstimatedDate),
                                    JobPosition = jobPosition,
                                    Employee = new Employee() { Id = rdr.GetInt32(ColumnsLearningGetAssistance.EmployeeId), Name = rdr.GetString(ColumnsLearningGetAssistance.Name), LastName = rdr.GetString(ColumnsLearningGetAssistance.LastName) },
                                    Learning = this
                                };

                                if (!rdr.IsDBNull(ColumnsLearningGetAssistance.Completed))
                                {
                                    newAssistance.Completed = rdr.GetBoolean(ColumnsLearningGetAssistance.Completed);
                                    if (!rdr.IsDBNull(ColumnsLearningGetAssistance.Success))
                                    {
                                        newAssistance.Success = rdr.GetBoolean(ColumnsLearningGetAssistance.Success);
                                    }
                                }

                                this.assistance.Add(newAssistance);
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
        }
    }
}