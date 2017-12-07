// --------------------------------
// <copyright file="Assistant.cs" company="Sbrinna">
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
    using GisoFramework.Activity;

    /// <summary>
    /// Implements then Assistant class
    /// </summary>
    public class Assistant : BaseItem
    {
        #region Fields

        /// <summary>
        /// Employee of assistance
        /// </summary>
        private Employee employee;

        /// <summary>
        /// Job position of assistance
        /// </summary>
        private JobPosition jobPosition;

        /// <summary>
        /// Learning of assistance
        /// </summary>
        private Learning learning;

        /// <summary>
        /// Indicates if assistance is completed
        /// </summary>
        private bool? completed;

        /// <summary>
        /// Indicates if assistance is successfull completed
        /// </summary>
        private bool? success;
        #endregion

        /// <summary>
        /// Initializes a new instance of the Assistant class.
        /// </summary>
        public Assistant()
        {
            this.JobPosition = JobPosition.Empty;
            this.Employee = Employee.EmptySimple;
            this.Learning = Learning.Empty;
        }

        #region Properties
        /// <summary>
        /// Gets an empty assistant
        /// </summary>
        public static Assistant Empty
        {
            get
            {
                return new Assistant();
            }
        }

        /// <summary>
        /// Gets or sets the employee of assistance
        /// </summary>
        public Employee Employee
        {
            get 
            { 
                return this.employee; 
            }

            set 
            { 
                this.employee = value; 
            }
        }

        /// <summary>
        /// Gets or sets the job position of assistance
        /// </summary>
        public JobPosition JobPosition
        {
            get 
            { 
                return this.jobPosition; 
            }

            set 
            { 
                this.jobPosition = value; 
            }
        }

        /// <summary>
        /// Gets or sets the learning of assistance
        /// </summary>
        public Learning Learning
        {
            get 
            { 
                return this.learning;
            }

            set 
            { 
                this.learning = value; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether assistace is completed
        /// </summary>
        public bool? Completed
        {
            get 
            {
                return this.completed; 
            }

            set 
            { 
                this.completed = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether assistance is successfull completed
        /// </summary>
        public bool? Success
        {
            get 
            { 
                return this.success;
            }

            set 
            { 
                this.success = value; 
            }
        }

        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a JSON structure of assistant
        /// </summary>
        public override string Json
        {
            get
            {
                string completedText = "-";
                string successText = "-";
                if (this.completed.HasValue)
                {
                    completedText = this.completed.Value ? "true" : "false";
                    if (this.completed.Value)
                    {
                        if (this.success.HasValue)
                        {
                            successText = this.success.Value ? "true" : "false";
                        }
                    }
                }

                string pattern = @"{{
                    ""Id"":{0},
                    ""CompanyId"":{1},
                    ""Employee"":{{""Id"":{2}, ""FullName"":""{3}""}},
                    ""Item_JobPosition"":{{""Id"":{4}, ""Description"":""{5}""}},
                    ""Learning"":{{""Id"":{6},""Description"":""{7}""}},
                    ""Completed"":{8},
                    ""Success"":{9}
                    }}";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    this.CompanyId,
                    this.Employee.Id,
                    this.Employee.FullName,
                    this.JobPosition.Id,
                    this.JobPosition.Description,
                    this.Learning.Id,
                    this.Learning.Description,
                    completedText,
                    successText);
            }
        }
        #endregion

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Description"":""{1}""}}", this.Id, this.Description);
            }
        }

        /*public override string Differences(BaseItem item1)
        {
            StringBuilder res = new StringBuilder();

            Assistant a1 = item1 as Assistant;

            if (a1.Description != this.Description)
            {
                res.Append("Description:").Append(this.Description).Append(";");
            }

            if (a1.employee.FullName != this.employee.FullName)
            {
                res.Append("Employee:").Append(this.employee.FullName).Append(";");
            }

            if (a1.Completed != this.completed)
            {
                res.Append("Completed:").Append(this.completed).Append(";");
            }

            if (a1.success != this.success)
            {
                res.Append("Success:").Append(this.success).Append(";");
            }
            return res.ToString();
        }*/

        /// <summary>
        /// Delete an assistant from data base
        /// </summary>
        /// <param name="assistantId">Assistant identifier</param>
        /// <param name="learningId">Learning identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int assistantId, int learningId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Learning_AssistantDelete
             * @LearningAssistantId int,
             * @LearningId int,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Learning_AssistantDelete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LearningAssistantId", SqlDbType.Int);
                    cmd.Parameters.Add("@LearningId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@LearningAssistantId"].Value = assistantId;
                    cmd.Parameters["@LearningId"].Value = learningId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
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

        /// <summary>
        /// Set the status of assistant in data base
        /// </summary>
        /// <param name="assistantId">Assistant identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <param name="completed">Inidicates if assistance is completed</param>
        /// <param name="success">Inidcates if assistance is successfull completed</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetStatus(int assistantId, int companyId, int userId, bool? completed, bool? success)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE LearningAssistant_SetStatus
             * @LearningAssitantId int,
             * @CompanyId int,
             * @UserId int,
             * @Completed bit,
             * @Success bit */
            using (SqlCommand cmd = new SqlCommand("LearningAssistant_SetStatus"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LearningAssitantId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters.Add("@Completed", SqlDbType.Bit);
                    cmd.Parameters.Add("@Success", SqlDbType.Bit);
                    cmd.Parameters["@LearningAssitantId"].Value = assistantId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;

                    if (completed.HasValue)
                    {
                        cmd.Parameters["@Completed"].Value = completed.Value;
                    }
                    else
                    {
                        cmd.Parameters["@Completed"].Value = DBNull.Value;
                    }

                    if (success.HasValue)
                    {
                        cmd.Parameters["@Success"].Value = success.Value;
                    }
                    else
                    {
                        cmd.Parameters["@Success"].Value = DBNull.Value;
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

            return res;
        }

        /// <summary>
        /// Set the assistance as completed
        /// </summary>
        /// <param name="assistantId">Assistant identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Complete(int assistantId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE LearningAssistant_Complete
             * @LearningAssitantId int,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("LearningAssistant_Complete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LearningAssitantId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@LearningAssitantId"].Value = assistantId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
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

        /// <summary>
        /// Set the assistance as completed
        /// </summary>
        /// <param name="assistantId">Assistant identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Unevaluated(int assistantId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE LearningAssistant_Complete
             * @LearningAssitantId int,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("LearningAssistant_Unevaluated"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LearningAssitantId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@LearningAssitantId"].Value = assistantId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
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

        /// <summary>
        /// Set the assistance as completed
        /// </summary>
        /// <param name="assistantId">Assistant identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult CompleteFail(int assistantId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE LearningAssistant_Complete
             * @LearningAssitantId int,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("LearningAssistant_Complete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LearningAssitantId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@LearningAssitantId"].Value = assistantId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
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

        /// <summary>
        /// Set the assistance as successfull completed
        /// </summary>
        /// <param name="assistantId">Assistant identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult CompleteAndSuccess(int assistantId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE LearningAssistant_Success
             * @LearningAssitantId int,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("LearningAssistant_Success"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LearningAssitantId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@LearningAssitantId"].Value = assistantId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
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

        /// <summary>
        /// Set the assistance as successfull completed
        /// </summary>
        /// <param name="assistantId">Assistant identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult CompleteAndSuccessFail(int assistantId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE LearningAssistant_SuccessFail
             * @LearningAssitantId int,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("LearningAssistant_SuccessFail"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LearningAssitantId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@LearningAssitantId"].Value = assistantId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
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

        /// <summary>
        /// Obtain all assistants from a compnay based on filter
        /// </summary>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="year">Year of learning</param>
        /// <param name="status">Assistant status</param>
        /// <returns>List of assistants</returns>
        public static ReadOnlyCollection<Assistant> GetAll(int companyId, int? year, int? status)
        {
            List<Assistant> res = new List<Assistant>();

            /* ALTER PROCEDURE Learning_GetAll
             * @CompanyId int,
             * @Year int,
             * @Status int */
            using (SqlCommand cmd = new SqlCommand("Learning_GetAll"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters.Add("@Year", SqlDbType.Int);
                cmd.Parameters.Add("@Status", SqlDbType.Int);

                cmd.Parameters["@CompanyId"].Value = companyId;
                if (year.HasValue)
                {
                    cmd.Parameters["@Year"].Value = year.Value;
                }
                else
                {
                    cmd.Parameters["@Year"].Value = DBNull.Value;
                }

                if (status.HasValue)
                {
                    cmd.Parameters["@Status"].Value = status.Value;
                }
                else
                {
                    cmd.Parameters["@Status"].Value = DBNull.Value;
                }

                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new Assistant()
                        {
                            Id = rdr.GetInt32(2),
                            Completed = rdr.GetBoolean(6),
                            Success = rdr.GetBoolean(7),
                            Employee = new Employee()
                            {
                                Id = rdr.GetInt32(3),
                                Name = rdr.GetString(4),
                                LastName = rdr.GetString(5)
                            },
                            Learning = new Learning()
                            {
                                Id = rdr.GetInt32(0),
                                Description = rdr.GetString(1),
                                DateEstimated = rdr.GetDateTime(8)
                            }
                        });
                    }
                }
                catch (SqlException ex)
                {
                    res = new List<Assistant>();
                    ExceptionManager.Trace(ex, "Assistant::GetAll", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0}", companyId));
                }
                catch (FormatException ex)
                {
                    res = new List<Assistant>();
                    ExceptionManager.Trace(ex, "Assistant::GetAll", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0}", companyId));
                }
                catch (NullReferenceException ex)
                {
                    res = new List<Assistant>();
                    ExceptionManager.Trace(ex, "Assistant::GetAll", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0}", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<Assistant>(res);
        }

        /// <summary>
        /// Render the HTML code for a row in assistant table of learning profile
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="admin">Indicates if user that rqeust the data has admninistrator role</param>
        /// <returns>HTML code</returns>
        public string GetRow(Dictionary<string, string> dictionary, bool admin)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            string yes = dictionary["Common_Yes"];
            string no = dictionary["Common_No"];

            string month = string.Empty;
            switch (this.Learning.DateEstimated.Month)
            {
                case 1:
                    month = dictionary["Enero"];
                    break;
                case 2:
                    month = dictionary["Febrero"];
                    break;
                case 3:
                    month = dictionary["Marzo"];
                    break;
                case 4:
                    month = dictionary["Abril"];
                    break;
                case 5:
                    month = dictionary["Mayo"];
                    break;
                case 6:
                    month = dictionary["Junio"];
                    break;
                case 7:
                    month = dictionary["Julio"];
                    break;
                case 8:
                    month = dictionary["Agosto"];
                    break;
                case 9:
                    month = dictionary["Septiembre"];
                    break;
                case 10:
                    month = dictionary["Octubre"];
                    break;
                case 11:
                    month = dictionary["Noviembre"];
                    break;
                case 12:
                    month = dictionary["Diciembre"];
                    break;
            }

            string completedText = "-";
            string successText = "-";
            string completedColor = "#000";
            string successColor = "#000";
            if (this.completed.HasValue)
            {
                completedText = this.completed.Value ? yes : no;
                completedColor = this.completed.Value ? "green" : "red";
                if (this.completed.Value)
                {
                    if (this.success.HasValue)
                    {
                        successText = this.success.Value ? yes : no;
                        successColor = this.success.Value ? "green" : "red";
                    }
                }
            }

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr id=""{0}""><td><input type=""checkbox"" id=""{0}|{10}|{11}"" onclick=""selectRow(this);"" /></td><td><a href=""FormacionView.aspx?id={10}"">{2}</a></td><td>{3}</td><td align=""center"" style=""color:{4};"">{5}</td><td align=""center"" style=""color:{6};"">{7}</td><td align=""center"">{8} {9}</td></tr>",
                this.Id,
                string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:000}", this.Id),
                this.learning.Description,
                admin ? this.employee.Link : this.employee.FullName,
                completedColor,
                completedText,
                successColor,
                successText,
                month,
                this.learning.DateEstimated.Year,
                this.learning.Id,
                this.employee.Id);
        }

        /// <summary>
        /// Insert a learning participant into data base
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE Learning_AddAssistant
             * @LearningAssistantId int out,
             * @LearningId int,
             * @CompanyId int,
             * @EmployeeId int,
             * @JobPositionId bigint,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Learning_AddAssistant"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@LearningAssistantId", SqlDbType.Int);
                    cmd.Parameters.Add("@LearningId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
                    cmd.Parameters.Add("@JobPositionId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@LearningAssistantId"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@LearningAssistantId"].Value = DBNull.Value;
                    cmd.Parameters["@LearningId"].Value = this.learning.Id;
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                    cmd.Parameters["@EmployeeId"].Value = this.employee.Id;

                    if (this.jobPosition == null)
                    {
                        cmd.Parameters["@JobPositionId"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@JobPositionId"].Value = this.jobPosition.Id;
                    }

                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    long assistantId = Convert.ToInt64(cmd.Parameters["@LearningAssistantId"].Value);
                    res.SetSuccess(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}|{1}", assistantId, this.employee.Id));
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
