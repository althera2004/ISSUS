// --------------------------------
// <copyright file="ApplicationUser.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Net.Mail;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item;
    using GisoFramework.Item.Binding;
    using GisoFramework.LogOn;
    using GisoFramework.UserInterface;
    using System.IO;

    /// <summary>
    /// Implements ApplicationUser class.
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>
        /// Value to indicate that an invalid password is send to change
        /// </summary>
        private const string IncorrectPassword = "NOPASS";

        #region Fields
        /// <summary>
        /// Application user identifier
        /// </summary>
        private int id;

        /// <summary>
        /// Indicates is online help is showed
        /// </summary>
        private bool showHelp;

        /// <summary>
        /// User name
        /// </summary>
        private string userName;

        /// <summary>
        /// User groups
        /// </summary>
        private List<ApplicationLogOn.SecurityGroup> groups;

        /// <summary>
        /// Collections of user grants
        /// </summary>
        private List<UserGrant> grants;

        /// <summary>
        /// Language to show in interface
        /// </summary>
        private string language;

        /// <summary>
        /// User account status
        /// </summary>
        private ApplicationLogOn.LogOnResult status;

        /// <summary>
        /// Shortcuts for user interface
        /// </summary>
        private MenuShortcut menuShortcuts;

        /// <summary>
        /// User's avatar
        /// </summary>
        private string avatar;

        /// <summary>
        /// Gets or sets user email
        /// </summary>
        public string Email { get; set; }

        public bool PrimaryUser { get; set; }
        #endregion

        public string Description
        {
            get
            {
                if (this.Employee != null && !string.IsNullOrEmpty(this.Employee.FullName))
                {
                    return this.Employee.FullName;
                }

                return this.userName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationUser class.
        /// Retrive data from database based on user identifier
        /// </summary>
        /// <param name="userId">User identifier</param>
        public ApplicationUser(int userId)
        {
            this.id = -1;
            this.Employee = Employee.Empty;
            this.userName = string.Empty;
            using (SqlCommand cmd = new SqlCommand("User_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                try
                {
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            this.groups = new List<ApplicationLogOn.SecurityGroup>();
                            this.grants = new List<UserGrant>();
                            this.id = rdr.GetInt32(ColumnsApplicationUserGetById.Id);
                            this.userName = rdr[ColumnsApplicationUserGetById.UserName].ToString();
                            this.status = ApplicationLogOn.IntegerToLogOnResult(rdr.GetInt32(ColumnsApplicationUserGetById.Status));
                            this.language = rdr[ColumnsApplicationUserGetById.Language].ToString();
                            this.showHelp = rdr.GetBoolean(ColumnsApplicationUserGetById.ShowHelp);
                            this.avatar = rdr.GetString(ColumnsApplicationUserGetById.Avatar);
                            this.Employee = Employee.EmptySimple;
                            this.Email = rdr.GetString(ColumnsApplicationUserGetById.Email);
                            this.PrimaryUser = rdr.GetBoolean(ColumnsApplicationUserGetById.PrimaryUser);

                            if (!rdr.IsDBNull(ColumnsApplicationUserGetById.EmployeeId))
                            {
                                this.Employee = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.EmployeeId),
                                    Name = rdr.GetString(ColumnsApplicationUserGetById.EmployeeName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGetById.EmployeeLastName)
                                };
                            }

                            this.menuShortcuts = new MenuShortcut();

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.GreenLabel)))
                            {
                                this.menuShortcuts.Green = new Shortcut()
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.GreenIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.GreenLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.GreenIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.GreenLink)
                                };
                            }

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.BlueLabel)))
                            {
                                this.menuShortcuts.Blue = new Shortcut()
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.BlueIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.BlueLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.BlueIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.BlueLink)
                                };
                            }

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.RedLabel)))
                            {
                                this.menuShortcuts.Red = new Shortcut()
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.RedIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.RedLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.RedIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.RedLink)
                                };
                            }

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.YellowLabel)))
                            {
                                this.menuShortcuts.Yellow = new Shortcut()
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.YellowIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.YellowLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.YellowIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.YellowLink)
                                };
                            }

                            this.GetGrants();
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

        /// <summary>
        /// Initializes a new instance of the ApplicationUser class.
        /// </summary>
        public ApplicationUser()
        {
            this.id = -1;
            this.userName = string.Empty;
            this.groups = new List<ApplicationLogOn.SecurityGroup>();
            this.Employee = Employee.EmptySimple;
        }

        #region Properties
        public int CompanyId { get; set; }

        /// <summary>Gets a empty user</summary>
        public static ApplicationUser Empty
        {
            get
            {
                return new ApplicationUser()
                {
                    id = -1,
                    userName = string.Empty,
                    Employee = Employee.EmptySimple
                };
            }
        }

        /// <summary>Gets or sets employee linked to user</summary>
        public Employee Employee { get; set; }

        public ReadOnlyCollection<UserGrant> Grants
        {
            get
            {
                if (this.grants == null)
                {
                    this.grants = new List<UserGrant>();
                }

                return new ReadOnlyCollection<UserGrant>(this.grants);
            }
        }

        /// <summary>
        /// Gets the HTML code for the link to Employee view page
        /// </summary>
        public string Link
        {
            get
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                string toolTip = dictionary["Common_Edit"];
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "<a href=\"UserView.aspx?id={0}\" title=\"{2} {1}\">{1}</a>", this.id, this.userName, toolTip);
            }
        }

        public string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Value"":""{1}""}}", this.id, Tools.JsonCompliant(this.userName));
            }
        }

        public ReadOnlyCollection<UserGrant> EffectiveGrants
        {
            get
            {
                /* ALTER PROCEDURE ApplicationUser_GetEffectiveGrants
                 *   @ApplicationUserId int */
                List<UserGrant> res = new List<UserGrant>();

                using (SqlCommand cmd = new SqlCommand("ApplicationUser_GetEffectiveGrants"))
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.id));
                    try
                    {
                        cmd.Connection.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            ApplicationGrant item = ApplicationGrant.FromInteger(rdr.GetInt32(ColumnsApplicationUserGetEffectiveGrants.ItemId));
                            item.Description = rdr[ColumnsApplicationUserGetEffectiveGrants.Description].ToString();

                            res.Add(new UserGrant()
                            {
                                UserId = this.id,
                                Item = item,
                                GrantToRead = rdr.GetBoolean(ColumnsApplicationUserGetEffectiveGrants.GrantToRead),
                                GrantToWrite = rdr.GetBoolean(ColumnsApplicationUserGetEffectiveGrants.GrantToWrite),
                                GrantToDelete = rdr.GetBoolean(ColumnsApplicationUserGetEffectiveGrants.GrantToDelete)
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

                return new ReadOnlyCollection<UserGrant>(res);
            }
        }

        /// <summary>
        /// Gets or sets the user avatar
        /// </summary>
        public string Avatar
        {
            get
            {
                return this.avatar;
            }

            set
            {
                this.avatar = value;
            }
        }

        /// <summary>
        /// Gets the name file of avatar image
        /// </summary>
        public string AvatarImage
        {
            get
            {
                if (string.IsNullOrEmpty(this.avatar))
                {
                    this.avatar = "avatar2.png";
                }

                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"assets/avatars/{0}", this.avatar);
            }
        }

        /// <summary>
        /// Gets or sets application user identifier
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets application user name
        /// </summary>
        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                this.userName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if user show help in interface
        /// </summary>
        public bool ShowHelp
        {
            get
            {
                return this.showHelp;
            }

            set
            {
                this.showHelp = value;
            }
        }

        /// <summary>
        /// Gets or sets the language of user
        /// </summary>
        public string Language
        {
            get
            {
                return this.language;
            }

            set
            {
                this.language = value;
            }
        }

        /// <summary>
        /// Gets or sets the status of user
        /// </summary>
        public ApplicationLogOn.LogOnResult Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
            }
        }

        /*
        /// <summary>
        /// Gets or sets the employee associated to user
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
        */

        /// <summary>
        /// Gets or sets the shorcuts of user
        /// </summary>
        public MenuShortcut MenuShortcuts
        {
            get
            {
                return this.menuShortcuts;
            }

            set
            {
                this.menuShortcuts = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether user has admin role
        /// </summary>
        public bool Admin
        {
            get
            {
                if (this.groups != null)
                {
                    foreach (ApplicationLogOn.SecurityGroup group in this.groups)
                    {
                        if (group == ApplicationLogOn.SecurityGroup.Company)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a json structure of user's groups
        /// </summary>
        public string GroupsJson
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<ApplicationLogOn.SecurityGroup>();
                }

                StringBuilder res = new StringBuilder("{");
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"""Administration"":{0},", this.groups.Contains(ApplicationLogOn.SecurityGroup.Company) ? "true" : "false"));
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"""Process"":{0},", this.groups.Contains(ApplicationLogOn.SecurityGroup.Process) ? "true" : "false"));
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"""Documents"":{0},", this.groups.Contains(ApplicationLogOn.SecurityGroup.Documents) ? "true" : "false"));
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"""Learning"":{0}", this.groups.Contains(ApplicationLogOn.SecurityGroup.Learning) ? "true" : "false"));
                return res.Append("}").ToString();
            }
        }

        /// <summary>
        /// Gets user's groups
        /// </summary>
        public ReadOnlyCollection<ApplicationLogOn.SecurityGroup> Groups
        {
            get
            {
                return new ReadOnlyCollection<ApplicationLogOn.SecurityGroup>(this.groups);
            }
        }

        /// <summary>
        /// Gets a json structure of user
        /// </summary>
        public string Json
        {
            get
            {
                StringBuilder res = new StringBuilder("{").Append(Environment.NewLine);
                res.Append("    \"Id\":").Append(this.id).Append(",").Append(Environment.NewLine);
                res.Append("    \"CompanyId\":").Append(this.CompanyId).Append(",").Append(Environment.NewLine);
                res.Append("    \"Login\":\"").Append(this.userName).Append("\",").Append(Environment.NewLine);
                res.Append("    \"PrimaryUser\":").Append(this.PrimaryUser ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("    \"Language\":\"").Append(this.language).Append("\",").Append(Environment.NewLine);
                res.Append("    \"ShowHelp\":").Append(this.showHelp ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("    \"Status\":\"").Append(this.status).Append("\",");
                /*if (this.Employee != null)
                {
                    res.Append(",").Append(Environment.NewLine).Append("\t\"Employee\":").Append(Environment.NewLine).Append("\t{").Append(Environment.NewLine);
                    res.Append("            \"Id\":").Append(this.Employee.Id).Append(",").Append(Environment.NewLine);
                    res.Append("            \"Name\":'").Append(this.Employee.Name).Append("',").Append(Environment.NewLine);
                    res.Append("            \"LastName\":'").Append(this.Employee.LastName).Append("',").Append(Environment.NewLine);
                    res.Append("        },");
                }*/

                res.Append("    \"Employee\":").Append(this.Employee.Json).Append(",");

                res.Append(Environment.NewLine);
                res.Append("        \"Grants\":").Append(Environment.NewLine);
                res.Append("        {");
                bool firstGrant = true;

                if (this.grants == null)
                {
                    this.GetGrants();
                }

                foreach (UserGrant grant in this.grants)
                {
                    if (firstGrant)
                    {
                        firstGrant = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(Environment.NewLine);
                    res.Append(string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"            ""{0}"":{{""Read"": {1}, ""Write"": {2}, ""Delete"": {3}}}",
                        grant.Item.Description,
                        grant.GrantToRead ? "true" : "false",
                        grant.GrantToWrite ? "true" : "false",
                        grant.GrantToDelete ? "true" : "false"));
                }

                res.Append(Environment.NewLine);
                res.Append("        }").Append(Environment.NewLine);
                res.Append(Environment.NewLine).Append("    }");
                return res.ToString();
            }
        }
        #endregion

        public static ReadOnlyCollection<ApplicationUser> CompanyUsers(int companyId)
        {
            List<ApplicationUser> res = new List<ApplicationUser>();
            /* CREATE PROCEDURE [dbo].[User_GetByCompanyId]
             *   @CompanyId int
             */
            using (SqlCommand cmd = new SqlCommand("User_GetByCompanyId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ApplicationUser newUser = new ApplicationUser()
                            {
                                id = rdr.GetInt32(ColumnsUserGetByCompanyId.Id),
                                userName = rdr.GetString(ColumnsUserGetByCompanyId.Login),
                                Email = rdr.GetString(ColumnsUserGetByCompanyId.UserEmail)
                            };

                            if (!rdr.IsDBNull(ColumnsUserGetByCompanyId.SecurityGroupId))
                            {
                                newUser.groups.Add(ApplicationLogOn.IntegerToSecurityGroup(rdr.GetInt32(ColumnsUserGetByCompanyId.SecurityGroupId)));
                            }

                            if (!rdr.IsDBNull(ColumnsUserGetByCompanyId.EmployeeId))
                            {
                                newUser.Employee = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsUserGetByCompanyId.EmployeeId),
                                    Name = rdr.GetString(ColumnsUserGetByCompanyId.EmployeeName),
                                    LastName = rdr.GetString(ColumnsUserGetByCompanyId.EmployeeLastName),
                                    Email = rdr.GetString(ColumnsUserGetByCompanyId.EmployeeEmail)
                                };
                            }

                            res.Add(newUser);
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

            return new ReadOnlyCollection<ApplicationUser>(res);
        }

        /// <summary>
        /// Gets the usernames of company users
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of usernames of company users</returns>
        public static Dictionary<string, int> CompanyUserNames(int companyId)
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            /* CREATE PROCEDURE Company_GetUserNames
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("Company_GetUserNames"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (!res.ContainsKey(rdr[0].ToString()))
                            {
                                res.Add(rdr[0].ToString(), rdr.GetInt32(1));
                            }
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

            return res;
        }

        /// <summary>
        /// Seta new user name into data base
        /// </summary>
        /// <param name="proposedUserName">New user name</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>String With the new name saved</returns>
        public static string SetNewUserName(string proposedUserName, int companyId)
        {
            if (string.IsNullOrEmpty(proposedUserName))
            {
                proposedUserName = string.Empty;
            }

            proposedUserName = proposedUserName.ToUpper(CultureInfo.GetCultureInfo("en-us")).Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U");
            string res = proposedUserName;
            Dictionary<string, int> userNames = ApplicationUser.CompanyUserNames(companyId);
            int cont = 1;
            bool ok = false;
            while (!ok)
            {
                bool found = false;
                foreach (KeyValuePair<string, int> userName in userNames)
                {
                    if (userName.Key.ToUpperInvariant() == res.ToUpperInvariant())
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    res = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}{1}", proposedUserName, cont);
                    cont++;
                }
                else
                {
                    ok = true;
                }
            }

            return res;
        }

        /// <summary>
        /// Change the user name
        /// </summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs actions</param>
        /// <param name="newUserName">New user name</param>
        /// <param name="employeeId">Employee identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ChangeUserName(int applicationUserId, int companyId, int userId, string newUserName, int employeeId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_ChangeUserName
             * @ApplicationUserId int,
             * @CompanyId int,
             * @UserName nvarchar(50),
             * @extraData nvarchar(200),
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_ChangeUserName"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserName", Tools.LimitedText(newUserName, 50)));
                    cmd.Parameters.Add(DataParameter.Input("@extraData", Tools.LimitedText(string.Format(CultureInfo.GetCultureInfo("en-us"), "UserName:{0}", newUserName), 200)));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"ApplicationUser::ChangeUserName({0}, {1}, {2}, ""{3}"")", applicationUserId, companyId, userId, newUserName));
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"ApplicationUser::ChangeUserName({0}, {1}, {2}, ""{3}"")", applicationUserId, companyId, userId, newUserName));
                }
                catch (ArgumentNullException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"ApplicationUser::ChangeUserName({0}, {1}, {2}, ""{3}"")", applicationUserId, companyId, userId, newUserName));
                }
                catch (ArgumentException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"ApplicationUser::ChangeUserName({0}, {1}, {2}, ""{3}"")", applicationUserId, companyId, userId, newUserName));
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"ApplicationUser::ChangeUserName({0}, {1}, {2}, ""{3}"")", applicationUserId, companyId, userId, newUserName));
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
        /// Revoke a user privilege
        /// </summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="grant">Privilege identifier</param>
        /// <param name="userId">Identifier of user that performs actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult RevokeGrant(int applicationUserId, int companyId, int grant, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_RevokeGrant
             * @ApplicationUserId int,
             * @CompanyId int,
             * @SecurityGroupId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_RevokeGrant"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@ApplicationUserId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@SecurityGroupId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@ApplicationUserId"].Value = applicationUserId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@SecurityGroupId"].Value = grant;
                    cmd.Parameters["@UserId"].Value = userId;
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

            return res;
        }

        /// <summary>
        /// Grant a user privilege
        /// </summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="grant">Privilege identifier</param>
        /// <param name="userId">Identifier of user that performs actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetGrant(int applicationUserId, int companyId, int grant, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_SetGrant
             * @ApplicationUserId int,
             * @CompanyId int,
             * @SecurityGroupId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_SetGrant"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@ApplicationUserId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@SecurityGroupId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@ApplicationUserId"].Value = applicationUserId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@SecurityGroupId"].Value = grant;
                    cmd.Parameters["@UserId"].Value = userId;
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

            return res;
        }

        public static ApplicationUser GetById(int userId, int companyId)
        {
            ApplicationUser res = ApplicationUser.Empty;
            res.groups = new List<ApplicationLogOn.SecurityGroup>();
            /* ALTER PROCEDURE ApplicationUser_GetById
             * @UserId int,
             * @CompanyId int*/
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.id = rdr.GetInt32(0);
                        res.userName = rdr.GetString(1);
                        res.status = ApplicationLogOn.IntegerToLogOnResult(rdr.GetInt32(3));
                        res.Email = rdr.GetString(9);
                        res.GetGrants();
                        res.Employee = Employee.GetByUserId(res.id);
                        res.PrimaryUser = rdr.GetBoolean(10);
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

            return res;
        }

        /// <summary>
        /// Get a user by employee identifier
        /// </summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>result of action</returns>
        public static ApplicationUser GetByEmployee(long employeeId, int companyId)
        {
            ApplicationUser res = ApplicationUser.Empty;
            res.groups = new List<ApplicationLogOn.SecurityGroup>();
            /* ALTER PROCEDURE ApplicationUser_ByEmployee
             * @EmployeeId bigint,
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_ByEmployee"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@EmployeeId"].Value = employeeId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.id = rdr.GetInt32(0);
                        res.userName = rdr.GetString(1);
                        res.status = ApplicationLogOn.IntegerToLogOnResult(rdr.GetInt32(3));
                        res.groups.Add(ApplicationLogOn.IntegerToSecurityGroup(rdr.GetInt32(4)));
                        res.PrimaryUser = rdr.GetBoolean(6);
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

            return res;
        }

        /// <summary>
        /// Change user's password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ChangePassword(int userId, string oldPassword, string newPassword, int companyId)
        {
            ActionResult res = ActionResult.NoAction;

            /* CREATE PROCEDURE ApplicationUser_ChangePassword
             * @UserId int,
             * @OldPassword nvarchar(50),
             * @NewPassword nvarchar(50),
             * @CompanyId int,
             * @Result int out */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_ChangePassword"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters.Add("@OldPassword", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@NewPassword", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Result", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Parameters["@OldPassword"].Value = oldPassword;
                    cmd.Parameters["@NewPassword"].Value = newPassword;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@Result"].Value = DBNull.Value;
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@Result"].Value.ToString().Trim() == "1")
                    {
                        res.SetSuccess();
                    }

                    if (cmd.Parameters["@Result"].Value.ToString().Trim() == "0")
                    {
                        res.SetFail(IncorrectPassword);
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

            return res;
        }

        /// <summary>
        /// Change user's password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ResetPassword(int userId, int companyId)
        {
            ActionResult res = ActionResult.NoAction;

            /* CREATE PROCEDURE ApplicationUser_ChangePassword
             * @UserId int,
             * @CompanyId int,
             * @Result int out */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_ResetPassword"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Result", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@Result"].Value = DBNull.Value;
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@UserName"].Value = DBNull.Value;
                    cmd.Parameters["@Password"].Value = DBNull.Value;
                    cmd.Parameters["@Email"].Value = DBNull.Value;
                    cmd.Parameters["@UserName"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Password"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Email"].Direction = ParameterDirection.Output;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    #region Send Mail
                    string userName = cmd.Parameters["@UserName"].Value as string;
                    string password = cmd.Parameters["@Password"].Value as string;
                    string email = cmd.Parameters["@Email"].Value as string;

                    string sender = ConfigurationManager.AppSettings["mailaddress"];
                    string pass = ConfigurationManager.AppSettings["mailpass"];

                    MailAddress senderMail = new MailAddress(sender, "ISSUS");
                    MailAddress to = new MailAddress(email);

                    SmtpClient client = new SmtpClient()
                    {
                        Host = "smtp.scrambotika.com",
                        Credentials = new System.Net.NetworkCredential(sender, pass),
                        Port = 25,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    MailMessage mail = new MailMessage(senderMail, to)
                    {
                        IsBodyHtml = true,
                        Subject = "Reinicio de contraseña en ISSUS"
                    };

                    string templatePath = HttpContext.Current.Request.PhysicalApplicationPath;
                    if(!templatePath.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                    {
                        templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\ResetPassword.tpl", templatePath);
                    }
                    else
                    {

                        templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}Templates\ResetPassword.tpl", templatePath);
                    }

                    string body = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        "Se ha reestablecido su contraseña en ISSUS.<br />Acceder a la siguiente url http://issus.scrambotika.com/ <br/> User:<b>{0}</b><br/>Password:<b>{1}</b>",
                        userName,
                        password);

                    if(File.Exists(templatePath))
                    {
                        using(StreamReader input = new StreamReader(templatePath))
                        {
                            body = input.ReadToEnd();
                        }

                        body = body.Replace("#USERNAME#", userName).Replace("#PASSWORD#", password).Replace("#EMAIL#", email);
                    }


                    mail.Body = body;
                    client.Send(mail);
                    #endregion

                    if (cmd.Parameters["@Result"].Value.ToString().Trim() == "1")
                    {
                        res.SetSuccess();
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "ResetPassword({0}, {1})", userId, companyId));
                    res.SetFail("No se pudo reiniciar la contraseña");
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

        public static ActionResult Delete(long userItemId, int companyId, long userId)
        {
            ActionResult res = ActionResult.NoAction;

            /* CREATE PROCEDURE ApplicationUser_Delete
             *   @UserItemId bigint,
             *   @CompanyId int,
             *   @UserId bigint */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@UserItemId", userItemId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
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
        /// Change user's avatar
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="avatar">Avatar image filename</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ChangeAvatar(int userId, string avatar, int companyId)
        {
            ActionResult res = ActionResult.NoAction;

            /* CREATE PROCEDURE [dbo].[ApplicationUser_ChangeAvatar]
             * @UserId int,
             * @Avatar nvarchar(50),
             * @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_ChangeAvatar"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters.Add("@Avatar", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Parameters["@Avatar"].Value = avatar;
                    cmd.Parameters["@CompanyId"].Value = companyId;
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
        /// Gets a text representative of group
        /// </summary>
        /// <param name="group">Security group</param>
        /// <returns>A text representative of group</returns>
        public static string GrantToText(ApplicationLogOn.SecurityGroup group)
        {
            string res = string.Empty;
            switch (group)
            {
                case ApplicationLogOn.SecurityGroup.Company:
                    res = "Item_CompanyData";
                    break;
                case ApplicationLogOn.SecurityGroup.Indicators:
                    res = "Indicadores";
                    break;
                case ApplicationLogOn.SecurityGroup.Documents:
                    res = "Item_Documents";
                    break;
                case ApplicationLogOn.SecurityGroup.Learning:
                    res = "Item_Learning";
                    break;
                case ApplicationLogOn.SecurityGroup.Providers:
                    res = "Item_Providers";
                    break;
                case ApplicationLogOn.SecurityGroup.Reviews:
                    res = "Item_Review";
                    break;
                case ApplicationLogOn.SecurityGroup.Equipments:
                    res = "Item_Equipments";
                    break;
                case ApplicationLogOn.SecurityGroup.Audits:
                    res = "Auditorias";
                    break;
                case ApplicationLogOn.SecurityGroup.Incidence:
                    res = "Incidencias";
                    break;
            }

            return res;
        }

        /// <summary>
        /// Update the shorcuts of application user
        /// </summary>
        /// <param name="userId">Application user identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="green">Green shortcut</param>
        /// <param name="blue">Blue shortcut</param>
        /// <param name="yellow">Yellow shortcut</param>
        /// <param name="red">Red shorcut</param>
        /// <returns>Result of action</returns>
        public static ActionResult UpdateShortcuts(int userId, int companyId, int? green, int? blue, int? yellow, int? red)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_UpdateShortCut
             * @ApplicationUserId int,
             * @CompanyId int,
             * @Green int,
             * @Blue int,
             * @Yellow int,
             * @Red int */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_UpdateShortCut"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@Green", green));
                    cmd.Parameters.Add(DataParameter.Input("@Blue", blue));
                    cmd.Parameters.Add(DataParameter.Input("@Red", red));
                    cmd.Parameters.Add(DataParameter.Input("@Yellow", yellow));
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
        /// Update the interface of application user
        /// </summary>
        /// <param name="userId">Aplication user identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="language">Language of interface</param>
        /// <param name="showHelp">Indicates if online help if showed</param>
        /// <returns>Result of action</returns>
        public static ActionResult UpdateInterfaceProfile(int userId, int companyId, string language, bool showHelp)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_UpdateUserInterface
             * @ApplicationUserId int,
             * @CompanyId int,
             * @Language nvarchar(2),
             * @ShowHelp bit */
            try
            {
                using (SqlCommand cmd = new SqlCommand("ApplicationUser_UpdateUserInterface"))
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@Language", Tools.LimitedText(language, 2)));
                    cmd.Parameters.Add(DataParameter.Input("@ShowHelp", showHelp));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"UpdateInterfaceProfile({0}, {1}, ""{2}"", {3})", userId, companyId, language, showHelp));
            }
            catch (FormatException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"UpdateInterfaceProfile({0}, {1}, ""{2}"", {3})", userId, companyId, language, showHelp));
            }
            catch (ArgumentNullException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"UpdateInterfaceProfile({0}, {1}, ""{2}"", {3})", userId, companyId, language, showHelp));
            }
            catch (ArgumentException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"UpdateInterfaceProfile({0}, {1}, ""{2}"", {3})", userId, companyId, language, showHelp));
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"UpdateInterfaceProfile({0}, {1}, ""{2}"", {3})", userId, companyId, language, showHelp));
            }

            return res;
        }

        public static ActionResult ClearGrant(int userId)
        {
            /* CREATE PROCEDURE ApplicationUserGrant_Clear
             *   @ApplicationUserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("ApplicationUserGrant_Clear"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (ArgumentNullException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (ArgumentException ex)
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

        public static ActionResult SaveGrant(UserGrant grant, int userId)
        {
            if(grant == null)
            {
                return ActionResult.NoAction;
            }

            string source = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"ApplicationUser::SaveGrant({0},{1})",
                grant.Item.Description,
                userId);
            /* CREATE PROCEDURE ApplicationUserGrant_Save
             *   @ApplicationUserId int,
             *   @ItemId int,
             *   @GrantToRead bit,
             *   @GrantToWrite bit,
             *   @GrantToDelete bit,
             *   @UserId int */
            ActionResult res = ActionResult.NoAction;
            if (grant == null)
            {
                return res;
            }

            using (SqlCommand cmd = new SqlCommand("ApplicationUserGrant_Save"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", grant.UserId));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", grant.Item.Code));
                cmd.Parameters.Add(DataParameter.Input("@GrantToRead", grant.GrantToRead));
                cmd.Parameters.Add(DataParameter.Input("@GrantToWrite", grant.GrantToWrite));
                cmd.Parameters.Add(DataParameter.Input("@GrantToDelete", grant.GrantToDelete));
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentNullException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (NotSupportedException ex)
                {
                    res.SetFail(ex.Message);
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
        /// Generates a JSON structure of application user grants
        /// </summary>
        /// <returns>JSON structure</returns>
        public string GrantToJson()
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (ApplicationLogOn.SecurityGroup group in this.groups)
            {
                if (first == true)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append("{Id:'").Append(group).Append("',Description:'").Append(GrantToText(group)).Append("'}");
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Render the HTML code for a row in users list
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grants">Grants of user</param>
        /// <returns>HTML code</returns>
        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> userGrants)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(userGrants, ApplicationGrant.User);
            bool grantDelete = UserGrant.HasDeleteGrant(userGrants, ApplicationGrant.User);

            string employeeLink = this.Employee != null ? this.Employee.Link : string.Empty;

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{3} {2}"" class=""btn btn-xs btn-danger"" onclick=""UserDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>",
                    this.id,
                    this.Description,
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Description)),
                    Tools.JsonCompliant(dictionary["Common_Delete"]));

            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""UserUpdate({0},'{2}');""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""UserUpdate({0},'{2}');""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            string pattern = @"<tr><td>{0}</td><td style=""width:200px;"">{1}</td><td style=""width:200px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                this.Link,
                employeeLink,
                this.Email,
                iconEdit,
                iconDelete);
        }

        /// <summary>
        /// Determine if user has grants of trace items
        /// </summary>
        /// <returns>If user has grants of trace items</returns>
        public bool HasTraceGrant()
        {
            return this.CalculateHasGrant(ApplicationGrant.Trace.Code, 1);
        }

        /// <summary>
        /// Indicates if user has read grant over item
        /// </summary>
        /// <param name="applicationItem">Application grant</param>
        /// <returns>If user has read grant over item</returns>
        public bool HasGrantToRead(int applicationItem)
        {
            return this.CalculateHasGrant(applicationItem, 1);
        }

        /// <summary>
        /// Indicates if user has write grant over item
        /// </summary>
        /// <param name="grant">Application grant</param>
        /// <returns>If user has write grant over item</returns>
        public bool HasGrantToRead(ApplicationGrant grant)
        {
            if (grant == null)
            {
                return false;
            }

            return this.CalculateHasGrant(grant.Code, 1);
        }

        /// <summary>
        /// Indicates if user has write grant over item
        /// </summary>
        /// <param name="grant">Application grant</param>
        /// <returns>If user has write grant over item</returns>
        public bool HasGrantToWrite(ApplicationGrant grant)
        {
            if (grant == null)
            {
                return false;
            }

            return this.CalculateHasGrant(grant.Code, 2);
        }

        /// <summary>
        /// Indicates if user has delete grant over item
        /// </summary>
        /// <param name="grant">Application grant</param>
        /// <returns>If user has delete grant over item</returns>
        public bool HasGrantToDelete(ApplicationGrant grant)
        {
            if (grant == null)
            {
                return false;
            }

            return this.CalculateHasGrant(grant.Code, 3);
        }
        
        /// <summary>
        /// Gets empty grants for user
        /// </summary>
        public static ReadOnlyCollection<UserGrant> GetEmptyGrants
        {
            get
            {
                List<UserGrant> res = new List<UserGrant>();

                using (SqlCommand cmd = new SqlCommand("ApplicationUser_GetGrants"))
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", 50));
                    try
                    {
                        cmd.Connection.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            res.Add(new UserGrant()
                            {
                                UserId = -1,
                                Item = ApplicationGrant.FromIntegerUrl(rdr.GetInt32(ColumnsApplicationUserGetGrants.ItemId), rdr[ColumnsApplicationUserGetGrants.UrlList].ToString()),
                                GrantToRead = false,
                                GrantToWrite = false,
                                GrantToDelete = false
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

                return new ReadOnlyCollection<UserGrant>(res);
            }
        }

        /// <summary>
        /// Obtain user grant form database
        /// </summary>
        public void GetGrants()
        {
            /* ALTER PROCEDURE ApplicationUser_GetGrants
             * @ApplicationUserId int */
            this.grants = new List<UserGrant>();

            using (SqlCommand cmd = new SqlCommand("ApplicationUser_GetGrants"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.id));
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.grants.Add(new UserGrant()
                        {
                            UserId = this.id,
                            Item = ApplicationGrant.FromIntegerUrl(rdr.GetInt32(ColumnsApplicationUserGetGrants.ItemId), rdr[ColumnsApplicationUserGetGrants.UrlList].ToString()),
                            GrantToRead = rdr.GetBoolean(ColumnsApplicationUserGetGrants.GrantToRead),
                            GrantToWrite = rdr.GetBoolean(ColumnsApplicationUserGetGrants.GrantToWrite),
                            GrantToDelete = rdr.GetBoolean(ColumnsApplicationUserGetGrants.GrantToDelete)
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
        }

        /// <summary>
        /// Update application user in database
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            string source = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"ApplicationUser::Update({0}),{1}",
                userId,
                this.Json);
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_Update
             *   @ApplicationUserId int,
             *   @UserName nvarchar(50) */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.id));
                    cmd.Parameters.Add(DataParameter.Input("@UserName", this.userName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, 50));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res = this.Employee.Update(userId);
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentNullException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
                    ExceptionManager.Trace(ex, source);
                }
                catch (NotSupportedException ex)
                {
                    res.SetFail(ex.Message);
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
        /// Determines if user has a grant over an item
        /// </summary>
        /// <param name="grant">Grant to determine</param>
        /// <param name="grantType">Item affected</param>
        /// <returns>If user has a grant over an item</returns>
        private bool CalculateHasGrant(int grant, int grantType)
        {
            foreach (UserGrant userGrant in this.grants)
            {
                bool granted = false;
                switch (grantType)
                {
                    case 1:
                        granted = userGrant.GrantToRead;
                        break;
                    case 2:
                        granted = userGrant.GrantToWrite;
                        break;
                    case 3:
                        granted = userGrant.GrantToDelete;
                        break;
                }

                if (userGrant.Item.Code == grant && granted)
                {
                    return true;
                }
            }

            return false;
        }

        public ActionResult Insert(int applicationUserId)
        {
            /*CREATE PROCEDURE ApplicationUser_Insert
             *   @Id int output,
             *   @CompanyId int,
             *   @Login nvarchar(50),
             *   @Email nvarchar(50),
             *   @Password nvarchar(50) */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_Insert"))
            {
                string userpass = RandomPassword.Generate(5, 8);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                cmd.Parameters.Add(DataParameter.Input("@Login", this.userName, 50));
                cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, 50));
                cmd.Parameters.Add(DataParameter.Input("@Password", userpass));
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.ReturnValue = Convert.ToInt32(cmd.Parameters["@Id"].Value);

                    Company company = new Company(this.CompanyId);

                    string sender = ConfigurationManager.AppSettings["mailaddress"];
                    string pass = ConfigurationManager.AppSettings["mailpass"];

                    MailAddress senderMail = new MailAddress(sender, "ISSUS");
                    MailAddress to = new MailAddress(this.Email);

                    SmtpClient client = new SmtpClient()
                    {
                        Host = "smtp.scrambotika.com",
                        Credentials = new System.Net.NetworkCredential(sender, pass),
                        Port = 25,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    MailMessage mail = new MailMessage(senderMail, to)
                    {
                        IsBodyHtml = true,
                        Subject = "Datos de acceso a ISSUS",
                        Body = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        "Acceder a la siguiente url http://www.scrambotika.cat/Default.aspx?company={0} <br/> User:<b>{1}</b><br/>Password:<b>{2}</b>",
                        company.Code,
                        this.userName,
                        userpass)
                    };

                    client.Send(mail);
                    res.SetSuccess(Convert.ToInt32(cmd.Parameters["@Id"].Value));
                }
                catch (Exception ex)
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

        public static ActionResult UnsetEmployee(int userId, int companyId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_UnsetUser
             *   @UserId bigint,
             *   @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("Employee_UnsetUser"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (Exception ex)
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
    }
}