// --------------------------------
// <copyright file="ApplicationLogOn.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework.LogOn
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item;
    using GisoFramework.Item.Binding;

    /// <summary>Implements ApplicationLogOn class</summary>
    public static class ApplicationLogOn
    {
        /// <summary>Enum for log on result</summary>
        public enum LogOnResult
        {
            /// <summary>None - 0</summary>
            None = 0,

            /// <summary>Ok - 1</summary>
            Ok = 1,

            /// <summary>NoUser - 2</summary>
            NoUser = 2,

            /// <summary>LockUser - 3</summary>
            LockUser = 3,

            /// <summary>Fail - 4</summary>
            Fail = 4,

            /// <summary>Admin - 1001</summary>
            Admin = 1001,

            /// <summary>Administrative - 1002</summary>
            Administrative = 1002
        }

        /// <summary>Enum for security groups</summary>
        public enum SecurityGroup
        {
            /// <summary>None - 0</summary>
            None = 0,

            /// <summary>Company - 1</summary>
            Company = 1,

            /// <summary>Indicators - 2</summary>
            Indicators = 2,

            /// <summary>Documents - 3</summary>
            Documents = 3,

            /// <summary>Learning - 4</summary>
            Learning = 4,

            /// <summary>Providers - 5</summary>
            Providers = 5,

            /// <summary>Equipments - 6</summary>
            Equipments = 6,

            /// <summary>Incidence - 7</summary>
            Incidence = 7,

            /// <summary>Audits - 8</summary>
            Audits = 8,

            /// <summary>Reviews - 9</summary>
            Reviews = 9,

            /// <summary>Process - 10</summary>
            Process = 10,

            /// <summary>Actions - 11</summary>
            Actions = 11,

            /// <summary>Employees - 12</summary>
            Employees = 12,

            /// <summary>Job position - 13</summary>
            JobPosition = 13,

            /// <summary>Traces - 14</summary>
            Traces = 14,

            /// <summary>Users - 15</summary>
            Users = 15,

            /// <summary>Departments - 16</summary>
            Departments = 16
        }

        /// <summary>Gets the secutiry group from integer value</summary>
        /// <param name="value">Integer that represents a security group</param>
        /// <returns>Secutiry group</returns>
        public static SecurityGroup IntegerToSecurityGroup(int value)
        {
            var res = SecurityGroup.None;
            switch (value)
            {
                case 1:
                    res = SecurityGroup.Company;
                    break;
                case 2:
                    res = SecurityGroup.Indicators;
                    break;
                case 3:
                    res = SecurityGroup.Documents;
                    break;
                case 4:
                    res = SecurityGroup.Learning;
                    break;
                case 5:
                    res = SecurityGroup.Providers;
                    break;
                case 6:
                    res = SecurityGroup.Equipments;
                    break;
                case 7:
                    res = SecurityGroup.Incidence;
                    break;
                case 8:
                    res = SecurityGroup.Audits;
                    break;
                case 9:
                    res = SecurityGroup.Reviews;
                    break;
                case 10:
                    res = SecurityGroup.Process;
                    break;
            }

            return res;
        }

        /// <summary>Gets the log on result from integer value</summary>
        /// <param name="value">Integer that represents a log on result</param>
        /// <returns>Log on result</returns>
        public static LogOnResult IntegerToLogOnResult(int value)
        {
            var res = LogOnResult.Fail;
            switch (value)
            {
                case 0:
                    res = LogOnResult.None;
                    break;
                case 1:
                    res = LogOnResult.Ok;
                    break;
                case 2:
                    res = LogOnResult.LockUser;
                    break;
                case 3:
                    res = LogOnResult.Fail;
                    break;
                case 1001:
                    res = LogOnResult.Admin;
                    break;
                case 1002:
                    res = LogOnResult.Administrative;
                    break;
            }

            return res;
        }

        /// <summary>Trace a log on failed</summary>
        /// <param name="userId">Identifier of user that attemps to log on</param>
        public static void LogOnFailed(int userId)
        {
            using (var cmd = new SqlCommand("LogonFailed"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);
                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.ExecuteNonQuery();
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

        /// <summary>Log on application</summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="clientAddress">IP address from log on action</param>
        /// <returns>Result of action</returns>
        public static ActionResult GetApplicationAccess(string email, string password, string clientAddress)
        {
            HttpContext.Current.Session["Companies"] = null;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return ActionResult.NoAction;
            }

            var res = ActionResult.NoAction;
            var result = new LogOnObject
            {
                Id = -1,
                UserName = string.Empty,
                Result = LogOnResult.NoUser
            };

            var companiesId = new List<string>();
            using (var cmd = new SqlCommand("GetLogin"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Login", email));
                    cmd.Parameters.Add(DataParameter.Input("@Password", password));
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        bool multiCompany = false;
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                companiesId.Add(rdr.GetInt32(ColumnsGetLogin.CompanyId).ToString() + '|' + rdr.GetInt32(ColumnsGetLogin.Id).ToString());
                                result.Id = rdr.GetInt32(ColumnsGetLogin.Id);
                                result.Result = IntegerToLogOnResult(rdr.GetInt32(ColumnsGetLogin.Status));
                                result.UserName = email;
                                result.CompanyId = rdr.GetInt32(ColumnsGetLogin.CompanyId);
                                result.MustResetPassword = rdr.GetBoolean(ColumnsGetLogin.MustResetPassword);
                                result.Agreement = rdr.GetBoolean(ColumnsGetLogin.Agreement);

                                if (result.Result == LogOnResult.Fail)
                                {
                                    LogOnFailed(result.Id);
                                }
                                else
                                {
                                    var user = new ApplicationUser
                                    {
                                        Id = result.Id,
                                        UserName = rdr.GetString(ColumnsGetLogin.UserName),
                                        Language = rdr.GetString(ColumnsGetLogin.Language),
                                        Status = result.Result
                                    };

                                    user.ObtainGrants();

                                    HttpContext.Current.Session["User"] = user;
                                }

                                result.MultipleCompany = multiCompany;
                                multiCompany = true;
                            }
                        }
                        else
                        {
                            result.Result = LogOnResult.NoUser;
                            res.ReturnValue = result;
                            res.SetFail("NO USER");
                            return res;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    result.Result = LogOnResult.Fail;
                    result.Id = -1;
                    result.UserName = ex.Message;
                }
                catch (FormatException ex)
                {
                    result.Result = LogOnResult.Fail;
                    result.Id = -1;
                    result.UserName = ex.Message;
                }
                catch (NullReferenceException ex)
                {
                    result.Result = LogOnResult.Fail;
                    result.Id = -1;
                    result.UserName = ex.Message;
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            bool resultOk = result.Result == LogOnResult.Ok || result.Result == LogOnResult.Admin || result.Result == LogOnResult.Administrative;

            if (string.IsNullOrEmpty(clientAddress))
            {
                clientAddress = "no-ip";
            }

            HttpContext.Current.Session["Companies"] = companiesId;
            InsertLog(email, clientAddress, resultOk ? 1 : 2, result.Id, string.Empty, result.CompanyId);
            res.SetSuccess(result);
            return res;
        }

        /// <summary>Insert into data base a trace of log on action</summary>
        /// <param name="userName">User name</param>
        /// <param name="ip">IP address from log on action</param>
        /// <param name="result">Result of action</param>
        /// <param name="userId">Identifier of users that attemps to log on</param>
        /// <param name="companyCode">Code of compnay to log in</param>
        /// <param name="companyId">Identifier of company to log in</param>
        private static void InsertLog(string userName, string ip, int result, int userId, string companyCode, int companyId)
        {
            /* CREATE PROCEDURE Log_Login
             * @UserName nvarchar(50),
             * @Date datetime,
             * @Ip nvarchar(50),
             * @Result int,
             * @UserId int,
             * @CompanyCode nvarchar(10),
             * @CompanyId int
             */
            using (var cmd = new SqlCommand("Log_Login"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@UserName", userName));
                    cmd.Parameters.Add(DataParameter.Input("@Ip", ip));
                    cmd.Parameters.Add(DataParameter.Input("@Result", result));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyCode", companyCode));

                    if (companyId != 0)
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    }
                    else
                    {
                        companyId = Company.ByCode(companyCode);
                        if (companyId == 0)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@CompanyId"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        }
                    }

                    if (userId > 0)
                    {
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@UserId"));
                    }

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
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