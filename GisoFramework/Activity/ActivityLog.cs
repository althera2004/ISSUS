// -----------------------------------------------------------------------
// <copyright file="ActivityLog.cs" company="Sbrinna">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Activity
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using GisoFramework.DataAccess;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum TargetType
    {   
        /// <summary>Undefined = 0</summary>
        None = 0,

        /// <summary>Company = 1</summary>
        Company = 1,

        /// <summary>User = 2</summary>
        User = 2,

        /// <summary>Security group = 3</summary>
        SecurityGroup = 3,

        /// <summary>Document = 4</summary>
        Document = 4,

        /// <summary>Department = 5</summary>
        Department = 5,

        /// <summary>Company address = 6</summary>
        CompanyAddress = 6,

        /// <summary>Log on = 7</summary>
        LogOn = 7,

        /// <summary>Employee = 8</summary>
        Employee = 8,

        /// <summary>Job position - 9</summary>
        JobPosition = 9,

        /// <summary>Process - 10</summary>
        Process = 10,

        /// <summary>Learning - 11</summary>
        Learning = 11,

        /// <summary>Learning assistant - 12</summary>
        LearningAssistant = 12,

        /// <summary>Provider - 14</summary>
        Provider = 14,

        /// <summary>Customer -15</summary>
        Customer = 15,

        /// <summary>BusinessRisk - 18</summary>
        BusinessRisk = 18,

        /// <summary>Rules - 19</summary>
        Rules = 19
    }

    /// <summary>Implements Activity log class</summary>
    public static class ActivityLog
    {
        /// <summary>
       /// Generates a trace for an Employee action
       /// </summary>
       /// <param name="targetId">Employee identifier</param>
       /// <param name="userId">User of action identifier</param>
       /// <param name="companyId">Company identifer</param>
       /// <param name="actionId">Action identifier from specific action's employee</param>
       /// <param name="extraData">Extra data if needed</param>
       /// <returns>result of action</returns>
        public static ActionResult Employee(long targetId, int userId, int companyId, EmployeeLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Employee, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a Logon
        /// </summary>
        /// <param name="internetAddress">Ip from remote machine</param>
        /// <param name="companyCode">Code of alias of company</param>
        /// <param name="result">Result of logon</param>
        /// <param name="companyId">Identify of the company where user is logged</param>
        /// <returns>Result of action</returns>
        public static ActionResult LogOn(string internetAddress, string companyCode, int result, int companyId)
        {
            string extradata = string.Format(CultureInfo.InvariantCulture, "{0} - {1:dd/MM/yyyy hh:mm:ss} - {2}", companyCode, DateTime.Now, internetAddress);
            return InsertLogActivity(TargetType.LogOn, 0, 0, companyId, result, extradata);
        }

        /// <summary>
        /// Generate a trace from a company
        /// </summary>
        /// <param name="targetId">Identity of the company</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company where of the login</param>
        /// <param name="actionId">Action identifier from specific action's company</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Result of action</returns>
        public static ActionResult Company(int targetId, int userId, int companyId, CompanyLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Company, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a job position action
        /// </summary>
        /// <param name="targetId">Identity of the job position</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company where of the login</param>
        /// <param name="actionId">Action identifier from specific action's company</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Result of action</returns>
        public static ActionResult JobPosition(long targetId, int userId, int companyId, JobPositionLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.JobPosition, Convert.ToInt32(targetId), userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a job position action
        /// </summary>
        /// <param name="targetId">Identity of the job position</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company where of the login</param>
        /// <param name="actionId">Action identifier from specific action's company</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Result of action</returns>
        public static ActionResult Provider(long targetId, int userId, int companyId, ProviderLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Provider, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a job position action
        /// </summary>
        /// <param name="targetId">Identity of the job position</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company where of the login</param>
        /// <param name="actionId">Action identifier from specific action's company</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Result of action</returns>
        public static ActionResult Customer(long targetId, int userId, int companyId, CustomerLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Customer, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a process action
        /// </summary>
        /// <param name="targetId">Identity of the process</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company where of the login</param>
        /// <param name="actionId">Action identifier from specific process's company</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Result of action</returns>
        public static ActionResult Process(long targetId, int userId, int companyId, ProcessLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Process, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a learning action
        /// </summary>
        /// <param name="targetId">Identity of the learning</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company where of the login</param>
        /// <param name="actionId">Action identifier from specific learning's company</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Result of action</returns>
        public static ActionResult Learning(long targetId, int userId, int companyId, LearningLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Learning, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a learning assistant action
        /// </summary>
        /// <param name="targetId">Identity of the learning assistant</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company where of the login</param>
        /// <param name="actionId">Action identifier from specific learning assistant's company</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Result of action</returns>
        public static ActionResult LearningAssistant(int targetId, int userId, int companyId, LearningAssistantLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Process, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from a user
        /// </summary>
        /// <param name="targetId">Identity of the user</param>
        /// <param name="userId">Identity of the user where of the login</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="actionId">Action identity from specific action's user</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Resutl of action</returns>
        public static ActionResult User(int targetId, int userId, int companyId, int actionId, string extraData)
        {
            return InsertLogActivity(TargetType.User, targetId, userId, companyId, actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from security group
        /// </summary>
        /// <param name="targetId">Identity of the security group</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">identity of the company</param>
        /// <param name="actionId">Action identity from specific action's security group</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Return of action</returns>       
        public static ActionResult SecurityGroup(int targetId, int userId, int companyId, int actionId, string extraData)
        {
            return InsertLogActivity(TargetType.SecurityGroup, targetId, userId, companyId, actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from document Version
        /// </summary>
        /// <param name="targetId">Identity of the document Version</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="version">Action identity from specific action's security group</param>
        /// <returns>Return of action</returns>
        public static ActionResult DocumentVersioned(int targetId, int userId, int companyId, int version)
        {
            return Document(targetId, userId, companyId, DocumentLogAction.Versioned, "Version:" + version.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generate a trace from document
        /// </summary>
        /// <param name="targetId">Identity of the document</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="actionId">Action identity from specific action's document</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns> Return of the action</returns>
        public static ActionResult Document(int targetId, int userId, int companyId, DocumentLogAction actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Document, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace from Departament
        /// </summary>
        /// <param name="targetId">Identity of the Departament</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="actionId">Action identity from specific action's Departament</param>
        /// <param name="extraData">Exta data if needed</param>
        /// <returns>Return of the action</returns>
        public static ActionResult Department(long targetId, int userId, int companyId, DepartmentLogActions actionId, string extraData)
        {
            return InsertLogActivity(TargetType.Department, targetId, userId, companyId, (int)actionId, extraData);
        }

        /// <summary>
        /// Generate a trace
        /// </summary>
        /// <param name="targetType">Identity of the update sumary</param>
        /// <param name="targetId">Identity of the insert log activity</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="actionId">Action identity from specific action's Insert log activity</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Return of the action</returns>
        public static ActionResult InsertLogActivity(TargetType targetType, int targetId, int userId, int companyId, int actionId, string extraData)
        {
            var res = ActionResult.NoAction;
            string storedProcedureName = string.Empty;

            switch (targetType)
            {
                case TargetType.Company:
                    storedProcedureName = "ActivityCompany";
                    break;
                case TargetType.User:
                    storedProcedureName = "ActivityUser";
                    break;
                case TargetType.SecurityGroup:
                    storedProcedureName = "ActivitySecurityGroup";
                    break;
                case TargetType.Document:
                    storedProcedureName = "ActivityDocument";
                    break;
                case TargetType.Department:
                    storedProcedureName = "ActivityDepartment";
                    break;
                case TargetType.LogOn:
                    storedProcedureName = "ActivityLogin";
                    break;
                case TargetType.Employee:
                    storedProcedureName = "ActivityEmployee";
                    break;
                case TargetType.JobPosition:
                    storedProcedureName = "ActivityJobPosition";
                    break;
                case TargetType.Process:
                    storedProcedureName = "ActivityProcess";
                    break;
                case TargetType.Learning:
                    storedProcedureName = "ActivityLearning";
                    break;
                case TargetType.Provider:
                    storedProcedureName = "ActivityProvider";
                    break;
                case TargetType.Customer:
                    storedProcedureName = "ActivityCustomer";
                    break;
                default:
                    storedProcedureName = string.Empty;
                    break;
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                res.MessageError = "No valid item";
            }

            using (SqlCommand cmd = new SqlCommand(storedProcedureName))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@TargetId", targetId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ActionId", actionId));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraData", extraData, 150));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
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
        /// Generate a trace
        /// </summary>
        /// <param name="targetType">Identity of the update sumary</param>
        /// <param name="targetId">Identity of the insert log activity</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="actionId">Action identity from specific action's Insert log activity</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Return of the action</returns>
        public static ActionResult InsertLogActivity(TargetType targetType, long targetId, int userId, int companyId, int actionId, string extraData)
        {
            var res = ActionResult.NoAction;
            string storedProcedureName = string.Empty;

            switch (targetType)
            {
                case TargetType.Company:
                    storedProcedureName = "ActivityCompany";
                    break;
                case TargetType.User:
                    storedProcedureName = "ActivityUser";
                    break;
                case TargetType.SecurityGroup:
                    storedProcedureName = "ActivitySecurityGroup";
                    break;
                case TargetType.Document:
                    storedProcedureName = "ActivityDocument";
                    break;
                case TargetType.Department:
                    storedProcedureName = "ActivityDepartment";
                    break;
                case TargetType.LogOn:
                    storedProcedureName = "ActivityLogin";
                    break;
                case TargetType.Employee:
                    storedProcedureName = "ActivityEmployee";
                    break;
                case TargetType.JobPosition:
                    storedProcedureName = "ActivityJobPosition";
                    break;
                case TargetType.Process:
                    storedProcedureName = "ActivityProcess";
                    break;
                case TargetType.Learning:
                    storedProcedureName = "ActivityLearning";
                    break;
                case TargetType.Provider:
                    storedProcedureName = "ActivityProvider";
                    break;
                case TargetType.Customer:
                    storedProcedureName = "ActivityCustomer";
                    break;
                default:
                    storedProcedureName = string.Empty;
                    break;
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                res.MessageError = "No valid item";
            }

            using (SqlCommand cmd = new SqlCommand(storedProcedureName))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@TargetId", targetId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ActionId", actionId));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraData", extraData, 150));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
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
        /// Generate a trace 
        /// </summary>
        /// <param name="itemId">identifier of item search, optional</param>
        /// <param name="targetType">type of item</param>
        /// <param name="companyId">Type of companyid</param>
        /// <param name="from">Date of start periode, not required</param>
        /// <param name="to">Date of end periode, not required</param>
        /// <returns>Return a list of log activity matching filter conditions, ordered from most recent</returns>
        public static ReadOnlyCollection<ActivityTrace> GetActivity(int itemId, TargetType targetType, int companyId, DateTime? from, DateTime? to)
        {
            var res = new List<ActivityTrace>();
            /* ALTER PROCEDURE [dbo].[Get_Activity]
             * @CompanyId int,
             * @TargetType int,
             * @ItemId int,
             * @From date,
             * @To date */

            using (var cmd = new SqlCommand("Get_Activity"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@TargetType", (int)targetType));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                    cmd.Parameters.Add(DataParameter.Input("@From", from));
                    cmd.Parameters.Add(DataParameter.Input("@To", to));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ActivityTrace()
                                {
                                    Date = rdr.GetDateTime(1),
                                    Target = rdr.GetString(4),
                                    Changes = rdr.GetString(6),
                                    ActionEmployee = rdr.GetString(7),
                                    Action = rdr.GetString(5)
                                });
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

            return new ReadOnlyCollection<ActivityTrace>(res);
        }

        /// <summary>
        /// Generate a trace 
        /// </summary>
        /// <param name="itemId">identifier of item search, optional</param>
        /// <param name="targetType">type of item</param>
        /// <param name="companyId">Type of companyid</param>
        /// <param name="from">Date of start periode, not required</param>
        /// <param name="to">Date of end periode, not required</param>
        /// <returns>Return a list of log activity matching filter conditions, ordered from most recent</returns>
        public static ReadOnlyCollection<ActivityTrace> GetActivity(long itemId, TargetType targetType, int companyId, DateTime? from, DateTime? to)
        {
            List<ActivityTrace> res = new List<ActivityTrace>();
            /* ALTER PROCEDURE [dbo].[Get_Activity]
             * @CompanyId int,
             * @TargetType int,
             * @ItemId int,
             * @From date,
             * @To date */

            using (SqlCommand cmd = new SqlCommand("Get_Activity"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@TargetType", (int)targetType));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                cmd.Parameters.Add(DataParameter.Input("@From", from));
                cmd.Parameters.Add(DataParameter.Input("@To", to));
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new ActivityTrace()
                        {
                            Date = rdr.GetDateTime(1),
                            Target = rdr.GetString(4),
                            Changes = rdr.GetString(6),
                            ActionEmployee = rdr.GetString(7),
                            Action = rdr.GetString(5)
                        });
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

            return new ReadOnlyCollection<ActivityTrace>(res);
        }

        /// <summary>
        /// Get all activity of company in the last 24 hours
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of actions</returns>
        public static ReadOnlyCollection<ActivityTrace> GetActivity24H(int companyId)
        {
            var res = new List<ActivityTrace>();
            /* ALTER PROCEDURE Get_ActivityLastDay
             * @CompanyId int */

            using (var cmd = new SqlCommand("Get_ActivityLastDay"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ActivityTrace()
                                {
                                    Date = rdr.GetDateTime(1),
                                    Target = rdr.GetString(4),
                                    Changes = rdr.GetString(6),
                                    ActionEmployee = rdr.GetString(7),
                                    Action = rdr.GetString(5),
                                    TargetId = Convert.ToInt32(rdr["TargetId"].ToString(), CultureInfo.GetCultureInfo("en-us")),
                                    Description = rdr["Description"].ToString()
                                });
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

            return new ReadOnlyCollection<ActivityTrace>(res);
        }
    }
}
