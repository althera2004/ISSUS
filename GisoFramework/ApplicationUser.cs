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
    using System.IO;
    using System.Net.Mail;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item;
    using GisoFramework.Item.Binding;
    using GisoFramework.LogOn;
    using GisoFramework.UserInterface;

    /// <summary>Implements ApplicationUser class.</summary>
    public class ApplicationUser
    {
        /// <summary>Value to indicate that an invalid password is send to change</summary>
        private const string IncorrectPassword = "NOPASS";

        /// <summary>User groups</summary>
        private List<ApplicationLogOn.SecurityGroup> groups;

        /// <summary>Collections of user grants</summary>
        private List<UserGrant> grants;

        /// <summary>User account status</summary>
        private ApplicationLogOn.LogOnResult status;

        /// <summary>Shortcuts for user interface</summary>
        private MenuShortcut menuShortcuts;

        /// <summary>
        /// Initializes a new instance of the ApplicationUser class.
        /// Retrive data from database based on user identifier
        /// </summary>
        /// <param name="userId">User identifier</param>
        public ApplicationUser(int userId)
        {
            this.Id = -1;
            this.Employee = Employee.Empty;
            this.UserName = string.Empty;
            using (var cmd = new SqlCommand("User_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            this.groups = new List<ApplicationLogOn.SecurityGroup>();
                            this.grants = new List<UserGrant>();
                            this.Id = rdr.GetInt32(ColumnsApplicationUserGetById.Id);
                            this.UserName = rdr[ColumnsApplicationUserGetById.UserName].ToString();
                            this.status = ApplicationLogOn.IntegerToLogOnResult(rdr.GetInt32(ColumnsApplicationUserGetById.Status));
                            this.Language = rdr[ColumnsApplicationUserGetById.Language].ToString();
                            this.ShowHelp = rdr.GetBoolean(ColumnsApplicationUserGetById.ShowHelp);
                            this.Avatar = rdr.GetString(ColumnsApplicationUserGetById.Avatar);
                            this.Employee = Employee.EmptySimple;
                            this.Email = rdr.GetString(ColumnsApplicationUserGetById.Email);
                            this.PrimaryUser = rdr.GetBoolean(ColumnsApplicationUserGetById.PrimaryUser);
                            this.CompanyId = rdr.GetInt32(ColumnsApplicationUserGetById.CompanyId);
                            this.Admin = rdr.GetBoolean(ColumnsApplicationUserGetById.Admin);

                            if (!rdr.IsDBNull(ColumnsApplicationUserGetById.EmployeeId))
                            {
                                this.Employee = new Employee
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.EmployeeId),
                                    Name = rdr.GetString(ColumnsApplicationUserGetById.EmployeeName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGetById.EmployeeLastName)
                                };
                            }

                            this.menuShortcuts = new MenuShortcut();

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.GreenLabel)))
                            {
                                this.menuShortcuts.Green = new Shortcut
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.GreenIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.GreenLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.GreenIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.GreenLink)
                                };
                            }

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.BlueLabel)))
                            {
                                this.menuShortcuts.Blue = new Shortcut
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.BlueIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.BlueLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.BlueIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.BlueLink)
                                };
                            }

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.RedLabel)))
                            {
                                this.menuShortcuts.Red = new Shortcut
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.RedIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.RedLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.RedIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.RedLink)
                                };
                            }

                            if (!string.IsNullOrEmpty(rdr.GetString(ColumnsApplicationUserGetById.YellowLabel)))
                            {
                                this.menuShortcuts.Yellow = new Shortcut
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationUserGetById.YellowIdentifier),
                                    Label = rdr.GetString(ColumnsApplicationUserGetById.YellowLabel),
                                    Icon = rdr.GetString(ColumnsApplicationUserGetById.YellowIcon),
                                    Link = rdr.GetString(ColumnsApplicationUserGetById.YellowLink)
                                };
                            }

                            this.ObtainGrants();
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

        /// <summary>Initializes a new instance of the ApplicationUser class.</summary>
        public ApplicationUser()
        {
            this.Id = -1;
            this.UserName = string.Empty;
            this.groups = new List<ApplicationLogOn.SecurityGroup>();
            this.Employee = Employee.EmptySimple;
        }

        /// <summary>Gets a empty user</summary>
        public static ApplicationUser Empty
        {
            get
            {
                return new ApplicationUser
                {
                    Id = -1,
                    UserName = string.Empty,
                    Employee = Employee.EmptySimple,
                    Admin = false,
                    PrimaryUser = false,
                    Email = string.Empty,
                    ShowHelp = false,
                    Avatar = string.Empty
                };
            }
        }

        /// <summary>Gets empty grants for user</summary>
        public static ReadOnlyCollection<UserGrant> GetEmptyGrants
        {
            get
            {
                var res = new List<UserGrant>();
                using (var cmd = new SqlCommand("ApplicationUser_GetGrants"))
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", DataParameter.DefaultTextLength));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new UserGrant
                                    {
                                        UserId = -1,
                                        Item = ApplicationGrant.FromIntegerUrl(rdr.GetInt32(ColumnsApplicationUserGetGrants.ItemId), rdr[ColumnsApplicationUserGetGrants.UrlList].ToString()),
                                        GrantToRead = false,
                                        GrantToWrite = false,
                                        GrantToDelete = false
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

                return new ReadOnlyCollection<UserGrant>(res);
            }
        }

        /// <summary>Gets or sets user email</summary>
        public string Email { get; set; }

        /// <summary>Gets or sets a value indicating whether user is primmary user</summary>
        public bool PrimaryUser { get; set; }

        /// <summary>Gets user description</summary>
        public string Description
        {
            get
            {
                if (this.Employee != null && !string.IsNullOrEmpty(this.Employee.FullName))
                {
                    return this.Employee.FullName;
                }

                return this.UserName;
            }
        }

        /// <summary>Gets or sets the company identifier</summary>
        public int CompanyId { get; set; }

        /// <summary>Gets or sets employee linked to user</summary>
        public Employee Employee { get; set; }

        /// <summary>Gets grants of user</summary>
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

        /// <summary>Gets the HTML code for the link to Employee view page</summary>
        public string Link
        {
            get
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                string toolTip = dictionary["Common_Edit"];
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "<a href=\"UserView.aspx?id={0}\" title=\"{2} {1}\">{1}</a>",
                    this.Id,
                    this.UserName,
                    toolTip);
            }
        }

        /// <summary>Gets a JSON structure compossed by key and value of application user</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Value"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.UserName));
            }
        }

        /// <summary>Gets the effective grants of user</summary>
        public ReadOnlyCollection<UserGrant> EffectiveGrants
        {
            get
            {
                /* ALTER PROCEDURE ApplicationUser_GetEffectiveGrants
                 *   @ApplicationUserId int */
                var res = new List<UserGrant>();
                using (var cmd = new SqlCommand("ApplicationUser_GetEffectiveGrants"))
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.Id));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var item = ApplicationGrant.FromInteger(rdr.GetInt32(ColumnsApplicationUserGetEffectiveGrants.ItemId));
                                    item.Description = rdr[ColumnsApplicationUserGetEffectiveGrants.Description].ToString();

                                    res.Add(new UserGrant
                                    {
                                        UserId = this.Id,
                                        Item = item,
                                        GrantToRead = rdr.GetBoolean(ColumnsApplicationUserGetEffectiveGrants.GrantToRead),
                                        GrantToWrite = rdr.GetBoolean(ColumnsApplicationUserGetEffectiveGrants.GrantToWrite),
                                        GrantToDelete = rdr.GetBoolean(ColumnsApplicationUserGetEffectiveGrants.GrantToDelete)
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

                return new ReadOnlyCollection<UserGrant>(res);
            }
        }

        /// <summary>Gets or sets the user avatar</summary>
        public string Avatar { get; set; }

        /// <summary>Gets the name file of avatar image</summary>
        public string AvatarImage
        {
            get
            {
                if (string.IsNullOrEmpty(this.Avatar))
                {
                    this.Avatar = "avatar2.png";
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"assets/avatars/{0}",
                    this.Avatar);
            }
        }

        /// <summary>Gets or sets application user identifier</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets application user name</summary>
        public string UserName { get; set; }

        /// <summary>Gets or sets a value indicating whether if user show help in interface</summary>
        public bool ShowHelp { get; set; }

        /// <summary>Gets or sets the language of user</summary>
        public string Language { get; set; }

        /// <summary>Gets or sets the status of user</summary>
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

        /// <summary>Gets or sets the shorcuts of user</summary>
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

        /// <summary>Gets or sets a value indicating whether user has admin role</summary>
        public bool Admin { get; set; }

        /// <summary>Gets a json structure of user's groups</summary>
        public string GroupsJson
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<ApplicationLogOn.SecurityGroup>();
                }

                var res = new StringBuilder("{");
                res.Append(string.Format(CultureInfo.InvariantCulture, @"""Administration"":{0},", this.groups.Contains(ApplicationLogOn.SecurityGroup.Company) ? "true" : "false"));
                res.Append(string.Format(CultureInfo.InvariantCulture, @"""Process"":{0},", this.groups.Contains(ApplicationLogOn.SecurityGroup.Process) ? "true" : "false"));
                res.Append(string.Format(CultureInfo.InvariantCulture, @"""Documents"":{0},", this.groups.Contains(ApplicationLogOn.SecurityGroup.Documents) ? "true" : "false"));
                res.Append(string.Format(CultureInfo.InvariantCulture, @"""Learning"":{0}", this.groups.Contains(ApplicationLogOn.SecurityGroup.Learning) ? "true" : "false"));
                return res.Append("}").ToString();
            }
        }

        /// <summary>Gets user's groups</summary>
        public ReadOnlyCollection<ApplicationLogOn.SecurityGroup> Groups
        {
            get
            {
                return new ReadOnlyCollection<ApplicationLogOn.SecurityGroup>(this.groups);
            }
        }

        /// <summary>Gets a json structure of user</summary>
        public string Json
        {
            get
            {
                var res = new StringBuilder("{").Append(Environment.NewLine);
                res.Append("    \"Id\": ").Append(this.Id).Append(",").Append(Environment.NewLine);
                res.Append("    \"CompanyId\": ").Append(this.CompanyId).Append(",").Append(Environment.NewLine);
                res.Append("    \"Login\": \"").Append(this.UserName).Append("\",").Append(Environment.NewLine);
                res.Append("    \"PrimaryUser\": ").Append(this.PrimaryUser ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("    \"Admin\":").Append(this.Admin ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("    \"Language\": \"").Append(this.Language).Append("\",").Append(Environment.NewLine);
                res.Append("    \"ShowHelp\": ").Append(this.ShowHelp ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("    \"Status\": \"").Append(this.status).Append("\",");
                /*if (this.Employee != null)
                {
                    res.Append(",").Append(Environment.NewLine).Append("\t\"Employee\":").Append(Environment.NewLine).Append("\t{").Append(Environment.NewLine);
                    res.Append("            \"Id\":").Append(this.Employee.Id).Append(",").Append(Environment.NewLine);
                    res.Append("            \"Name\":'").Append(this.Employee.Name).Append("',").Append(Environment.NewLine);
                    res.Append("            \"LastName\":'").Append(this.Employee.LastName).Append("',").Append(Environment.NewLine);
                    res.Append("        },");
                }*/

                res.Append("    \"Employee\": ").Append(this.Employee.Json).Append(",");

                res.Append(Environment.NewLine);
                res.Append("        \"Grants\": ").Append(Environment.NewLine);
                res.Append("        {");
                bool firstGrant = true;

                if (this.grants == null)
                {
                    this.ObtainGrants();
                }

                foreach (var grant in this.grants)
                {
                    if (firstGrant)
                    {
                        firstGrant = false;
                    }
                    else
                    {
                        res.Append(", ");
                    }

                    res.Append(Environment.NewLine);
                    res.Append(string.Format(
                        CultureInfo.InvariantCulture,
                        @"            ""{0}"": {{""Read"": {1}, ""Write"": {2}, ""Delete"": {3}}}",
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

        /// <summary>Set user's password</summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="password">Password to set</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetPassword(int applicationUserId, string password)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[ApplicationUser_SetPassword]
             *   @UserId int,
             *   @Password nvarchar(50) */
            using (var cmd = new SqlCommand("ApplicationUser_SetPassword"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", applicationUserId));
                cmd.Parameters.Add(DataParameter.Input("@Password", password, 50));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
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
            }

            return res;
        }

        /// <summary>Obtains user of a company</summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>List of users of a company</returns>
        public static ReadOnlyCollection<ApplicationUser> CompanyUsers(int companyId)
        {
            var res = new List<ApplicationUser>();
            /* CREATE PROCEDURE [dbo].[User_GetByCompanyId]
             *   @CompanyId int
             */
            using (var cmd = new SqlCommand("User_GetByCompanyId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
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
                                var newUser = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsUserGetByCompanyId.Id),
                                    UserName = rdr.GetString(ColumnsUserGetByCompanyId.Login),
                                    Email = rdr.GetString(ColumnsUserGetByCompanyId.UserEmail)
                                };

                                if (!rdr.IsDBNull(ColumnsUserGetByCompanyId.SecurityGroupId))
                                {
                                    newUser.groups.Add(ApplicationLogOn.IntegerToSecurityGroup(rdr.GetInt32(ColumnsUserGetByCompanyId.SecurityGroupId)));
                                }

                                if (!rdr.IsDBNull(ColumnsUserGetByCompanyId.EmployeeId))
                                {
                                    newUser.Employee = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsUserGetByCompanyId.EmployeeId),
                                        Name = rdr.GetString(ColumnsUserGetByCompanyId.EmployeeName),
                                        LastName = rdr.GetString(ColumnsUserGetByCompanyId.EmployeeLastName),
                                        Email = rdr.GetString(ColumnsUserGetByCompanyId.EmployeeEmail)
                                    };
                                }

                                if (!rdr.IsDBNull(ColumnsUserGetByCompanyId.PrimaryUser))
                                {
                                    newUser.PrimaryUser = rdr.GetBoolean(ColumnsUserGetByCompanyId.PrimaryUser);
                                }

                                if (!rdr.IsDBNull(ColumnsUserGetByCompanyId.Admin))
                                {
                                    newUser.Admin = rdr.GetBoolean(ColumnsUserGetByCompanyId.Admin);
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
            }

            return new ReadOnlyCollection<ApplicationUser>(res);
        }

        /// <summary>Gets the usernames of company users</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of usernames of company users</returns>
        public static Dictionary<string, int> CompanyUserNames(int companyId)
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            /* CREATE PROCEDURE Company_GetUserNames
             * @CompanyId int */
            using (var cmd = new SqlCommand("Company_GetUserNames"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
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
            }

            return res;
        }

        /// <summary>Set a new user name into data base</summary>
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
            var userNames = ApplicationUser.CompanyUserNames(companyId);
            int cont = 1;
            bool ok = false;
            while (!ok)
            {
                bool found = false;
                foreach (var userName in userNames)
                {
                    if (userName.Key.ToUpperInvariant() == res.ToUpperInvariant())
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    res = string.Format(CultureInfo.InvariantCulture, "{0}{1}", proposedUserName, cont);
                    cont++;
                }
                else
                {
                    ok = true;
                }
            }

            return res;
        }

        /// <summary>Change the user name</summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs actions</param>
        /// <param name="newUserName">New user name</param>
        /// <param name="employeeId">Employee identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ChangeUserName(int applicationUserId, int companyId, int userId, string newUserName, int employeeId)
        {
            string source = string.Format(CultureInfo.GetCultureInfo("en-us"), @"ApplicationUser::ChangeUserName({0}, {1}, {2}, ""{3}"")", applicationUserId, companyId, userId, newUserName);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_ChangeUserName
             * @ApplicationUserId int,
             * @CompanyId int,
             * @UserName nvarchar(50),
             * @extraData nvarchar(200),
             * @UserId int */
            using (var cmd = new SqlCommand("ApplicationUser_ChangeUserName"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
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
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
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

        /// <summary>Revoke a user privilege</summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="grant">Privilege identifier</param>
        /// <param name="userId">Identifier of user that performs actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult RevokeGrant(int applicationUserId, int companyId, int grant, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_RevokeGrant
             * @ApplicationUserId int,
             * @CompanyId int,
             * @SecurityGroupId int,
             * @UserId int */
            using (var cmd = new SqlCommand("ApplicationUser_RevokeGrant"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", grant));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
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

        /// <summary>Grant a user privilege</summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="grant">Privilege identifier</param>
        /// <param name="userId">Identifier of user that performs actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetGrant(int applicationUserId, int companyId, int grant, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_SetGrant
             * @ApplicationUserId int,
             * @CompanyId int,
             * @SecurityGroupId int,
             * @UserId int */
            using (var cmd = new SqlCommand("ApplicationUser_SetGrant"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", grant));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
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

        /// <summary>Gets user the by identifier.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>Data of application user</returns>
        public static ApplicationUser GetById(int userId, int companyId)
        {
            var res = ApplicationUser.Empty;
            res.groups = new List<ApplicationLogOn.SecurityGroup>();
            /* ALTER PROCEDURE ApplicationUser_GetById
             * @UserId int,
             * @CompanyId int*/
            using (var cmd = new SqlCommand("ApplicationUser_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Id = rdr.GetInt32(0);
                                res.UserName = rdr.GetString(1);
                                res.status = ApplicationLogOn.IntegerToLogOnResult(rdr.GetInt32(3));
                                res.Email = rdr.GetString(9);
                                res.ObtainGrants();
                                res.Employee = Employee.ByUserId(res.Id);
                                res.PrimaryUser = rdr.GetBoolean(10);
                                res.Admin = rdr.GetBoolean(11);
                                res.Language = rdr.GetString(12);
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

            return res;
        }

        /// <summary>Get a user by employee identifier</summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>result of action</returns>
        public static ApplicationUser GetByEmployee(long employeeId, int companyId)
        {
            var res = ApplicationUser.Empty;
            res.groups = new List<ApplicationLogOn.SecurityGroup>();
            /* ALTER PROCEDURE ApplicationUser_ByEmployee
             * @EmployeeId bigint,
             * @CompanyId int */
            using (var cmd = new SqlCommand("ApplicationUser_ByEmployee"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Id = rdr.GetInt32(0);
                                res.UserName = rdr.GetString(1);
                                res.status = ApplicationLogOn.IntegerToLogOnResult(rdr.GetInt32(3));
                                res.groups.Add(ApplicationLogOn.IntegerToSecurityGroup(rdr.GetInt32(4)));
                                res.PrimaryUser = rdr.GetBoolean(6);
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

            return res;
        }

        /// <summary>Change user's password</summary>
        /// <param name="userId">User identifier</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ChangePassword(int userId, string oldPassword, string newPassword, int companyId)
        {
            var res = ActionResult.NoAction;

            /* CREATE PROCEDURE ApplicationUser_ChangePassword
             * @UserId int,
             * @OldPassword nvarchar(50),
             * @NewPassword nvarchar(50),
             * @CompanyId int,
             * @Result int out */
            using (var cmd = new SqlCommand("ApplicationUser_ChangePassword"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@OldPassword", oldPassword));
                        cmd.Parameters.Add(DataParameter.Input("@NewPassword", newPassword));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.OutputInt("@Result"));
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
            }

            return res;
        }

        /// <summary>Change user's password</summary>
        /// <param name="userId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ResetPassword(int userId, int companyId)
        {
            var res = ActionResult.NoAction;
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;

            /* CREATE PROCEDURE ApplicationUser_ChangePassword
             * @UserId int,
             * @CompanyId int,
             * @Result int out */
            using (var cmd = new SqlCommand("ApplicationUser_ResetPassword"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
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

                        var company = new Company(companyId);
                        string userName = cmd.Parameters["@UserName"].Value as string;
                        string password = cmd.Parameters["@Password"].Value as string;
                        string email = cmd.Parameters["@Email"].Value as string;

                        var selectedUser = new ApplicationUser(userId);

                        string sender = ConfigurationManager.AppSettings["mailaddress"];
                        string pass = ConfigurationManager.AppSettings["mailpass"];

                        var senderMail = new MailAddress(sender, "ISSUS");
                        var to = new MailAddress(email);

                        using (var client = new SmtpClient
                        {
                            Host = "smtp.scrambotika.com",
                            Credentials = new System.Net.NetworkCredential(sender, pass),
                            Port = Constant.SmtpPort,
                            DeliveryMethod = SmtpDeliveryMethod.Network
                        })
                        {
                            var mail = new MailMessage(senderMail, to)
                            {
                                IsBodyHtml = true,
                                Subject = dictionary["MailTemplate_ResetPassword_DefaultBody"]
                            };

                            string templatePath = HttpContext.Current.Request.PhysicalApplicationPath;
                            string translatedTemplate = string.Format(
                                CultureInfo.InvariantCulture,
                                @"ResetPassword_{0}.tpl",
                                selectedUser.Language);
                            if (!templatePath.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                            {
                                templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}\", templatePath);
                            }

                            if (!File.Exists(templatePath + "Templates\\" + translatedTemplate))
                            {
                                translatedTemplate = "ResetPassword.tpl";
                            }

                            string body = string.Format(
                                CultureInfo.InvariantCulture,
                                dictionary["MailTemplate_ResetPassword_DefaultBody"],
                                userName,
                                password);

                            string templateFileName = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{0}Templates\{1}",
                                templatePath,
                                translatedTemplate);

                            if (File.Exists(templateFileName))
                            {
                                using (var input = new StreamReader(templateFileName))
                                {
                                    body = input.ReadToEnd();
                                }

                                body = body.Replace("#USERNAME#", userName).Replace("#PASSWORD#", password).Replace("#EMAIL#", email).Replace("#EMPRESA#", company.Name);
                            }

                            mail.Subject = dictionary["MailTemplate_ResetPassword_Subject"];
                            mail.Body = body;
                            client.Send(mail);
                        }

                        if (cmd.Parameters["@Result"].Value.ToString().Trim() == "1")
                        {
                            res.SetSuccess();
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "ResetPassword({0}, {1})", userId, companyId));
                        res.SetFail(dictionary["MailTemplate_ResetPassword_Error"]);
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

        /// <summary>Delete user from database</summary>
        /// <param name="userItemId">Identifier of user to delete</param>
        /// <param name="companyId">Identifier of company</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(long userItemId, int companyId, long userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_Delete
             *   @UserItemId bigint,
             *   @CompanyId int,
             *   @UserId bigint */
            using (var cmd = new SqlCommand("ApplicationUser_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
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
            }

            return res;
        }

        /// <summary>Change user's avatar</summary>
        /// <param name="userId">User identifier</param>
        /// <param name="avatar">Avatar image filename</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ChangeAvatar(int userId, string avatar, int companyId)
        {
            var res = ActionResult.NoAction;

            /* CREATE PROCEDURE [dbo].[ApplicationUser_ChangeAvatar]
             * @UserId int,
             * @Avatar nvarchar(50),
             * @CompanyId int */
            using (var cmd = new SqlCommand("ApplicationUser_ChangeAvatar"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@Avatar", avatar));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
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

        /// <summary>Gets a text representative of group</summary>
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
                case ApplicationLogOn.SecurityGroup.Departments:
                    res = "Departamentos";
                    break;
                default:
                    res = string.Empty;
                    break;
            }

            return res;
        }

        /// <summary>Update the shorcuts of application user</summary>
        /// <param name="userId">Application user identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="green">Green shortcut</param>
        /// <param name="blue">Blue shortcut</param>
        /// <param name="yellow">Yellow shortcut</param>
        /// <param name="red">Red shorcut</param>
        /// <returns>Result of action</returns>
        public static ActionResult UpdateShortcuts(int userId, int companyId, int? green, int? blue, int? yellow, int? red)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_UpdateShortCut
             * @ApplicationUserId int,
             * @CompanyId int,
             * @Green int,
             * @Blue int,
             * @Yellow int,
             * @Red int */
            using (var cmd = new SqlCommand("ApplicationUser_UpdateShortCut"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
                {
                    cmd.Connection = cnn;
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
            }

            return res;
        }

        /// <summary>Update the interface of application user</summary>
        /// <param name="userId">Aplication user identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="language">Language of interface</param>
        /// <param name="showHelp">Indicates if online help if showed</param>
        /// <returns>Result of action</returns>
        public static ActionResult UpdateInterfaceProfile(int userId, int companyId, string language, bool showHelp)
        {
            string source = string.Format(CultureInfo.GetCultureInfo("en-us"), @"UpdateInterfaceProfile({0}, {1}, ""{2}"", {3})", userId, companyId, language, showHelp);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_UpdateUserInterface
             * @ApplicationUserId int,
             * @CompanyId int,
             * @Language nvarchar(2),
             * @ShowHelp bit */
            using (var cmd = new SqlCommand("ApplicationUser_UpdateUserInterface"))
            {
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@Language", Tools.LimitedText(language, 2)));
                        cmd.Parameters.Add(DataParameter.Input("@ShowHelp", showHelp));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        /// <summary>Clear all user grants</summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ClearGrant(int userId)
        {
            /* CREATE PROCEDURE ApplicationUserGrant_Clear
             *   @ApplicationUserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("ApplicationUserGrant_Clear"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
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
            }

            return res;
        }

        /// <summary>Save user's specific grant</summary>
        /// <param name="grant">Grant to save</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult SaveGrant(UserGrant grant, int userId)
        {
            if (grant == null)
            {
                return ActionResult.NoAction;
            }

            string source = string.Format(
                CultureInfo.InvariantCulture,
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
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("ApplicationUserGrant_Save"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
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
            }

            return res;
        }

        /// <summary>Unset employee associtation of user</summary>
        /// <param name="userId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult UnsetEmployee(int userId, int companyId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_UnsetUser
             *   @UserId bigint,
             *   @CompanyId int */
            using (var cmd = new SqlCommand("Employee_UnsetUser"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
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
            }

            return res;
        }

        /// <summary>Generates a JSON structure of application user grants</summary>
        /// <returns>JSON structure</returns>
        public string GrantToJson()
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var group in this.groups)
            {
                if (first)
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

        /// <summary>Render the HTML code for a row in users list</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="userGrants">Grants of user</param>
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
                    this.Id,
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

            string iconAdmin = string.Empty;
            if (this.PrimaryUser)
            {
                iconAdmin = "<i class=\"icon-star\" style=\"color:#428bca;\" title=" + dictionary["User_PrimaryUser"] + "></i>";
            }
            else
            {
                if (this.Admin)
                {
                    iconAdmin = "<i class=\"icon-star\" style=\"color:#87b87f;\" title=" + dictionary["User_Admin"] + "></i>";
                }
            }

            string pattern = @"<tr><td style=""width:40px;"">{5}</td><td>{0}</td><td style=""width:300px;"">{1}</td><td style=""width:300px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                this.Link,
                employeeLink,
                this.Email,
                iconEdit,
                iconDelete,
                iconAdmin);
        }

        /// <summary>Determine if user has grants of trace items</summary>
        /// <returns>If user has grants of trace items</returns>
        public bool HasTraceGrant()
        {
            return this.CalculateHasGrant(ApplicationGrant.Trace.Code, 1);
        }

        /// <summary>Indicates if user has read grant over item</summary>
        /// <param name="applicationItem">Application grant</param>
        /// <returns>If user has read grant over item</returns>
        public bool HasGrantToRead(int applicationItem)
        {
            return this.CalculateHasGrant(applicationItem, 1);
        }

        /// <summary>Indicates if user has write grant over item</summary>
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

        /// <summary>Indicates if user has write grant over item</summary>
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

        /// <summary>Indicates if user has delete grant over item</summary>
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

        /// <summary>Obtain user grants from database</summary>
        public void ObtainGrants()
        {
            /* ALTER PROCEDURE ApplicationUser_GetGrants
             * @ApplicationUserId int */
            this.grants = new List<UserGrant>();

            using (var cmd = new SqlCommand("ApplicationUser_GetGrants"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.Id));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                this.grants.Add(new UserGrant()
                                {
                                    UserId = this.Id,
                                    Item = ApplicationGrant.FromIntegerUrl(rdr.GetInt32(ColumnsApplicationUserGetGrants.ItemId), rdr[ColumnsApplicationUserGetGrants.UrlList].ToString()),
                                    GrantToRead = rdr.GetBoolean(ColumnsApplicationUserGetGrants.GrantToRead),
                                    GrantToWrite = rdr.GetBoolean(ColumnsApplicationUserGetGrants.GrantToWrite),
                                    GrantToDelete = rdr.GetBoolean(ColumnsApplicationUserGetGrants.GrantToDelete)
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
        }

        /// <summary>Update application user in database</summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            string source = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"ApplicationUser::Update({0}),{1}",
                userId,
                this.Json);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ApplicationUser_Update
             *   @ApplicationUserId int,
             *   @UserName nvarchar(50) */
            using (var cmd = new SqlCommand("ApplicationUser_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserName", this.UserName, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Language", this.Language, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Admin", this.Admin));
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
            }

            return res;
        }

        /// <summary>Insert user in database</summary>
        /// <param name="applicationUserId">Identifier of user that performs the action</param>
        /// <returns>Result action</returns>
        public ActionResult Insert(int applicationUserId)
        {
            /*CREATE PROCEDURE ApplicationUser_Insert
             *   @Id int output,
             *   @CompanyId int,
             *   @Login nvarchar(50),
             *   @Email nvarchar(50),
             *   @Password nvarchar(50) */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("ApplicationUser_Insert"))
            {
                string userpass = GisoFramework.RandomPassword.Generate(5, 8);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Login", this.UserName, DataParameter.DefaultTextLength));
                    cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, DataParameter.DefaultTextLength));
                    cmd.Parameters.Add(DataParameter.Input("@Language", this.Language, DataParameter.DefaultTextLength));
                    cmd.Parameters.Add(DataParameter.Input("@Admin", this.Admin));
                    cmd.Parameters.Add(DataParameter.Input("@Password", userpass));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.ReturnValue = Convert.ToInt32(cmd.Parameters["@Id"].Value);

                        var company = new Company(this.CompanyId);
                        string sender = ConfigurationManager.AppSettings["mailaddress"];
                        string pass = ConfigurationManager.AppSettings["mailpass"];
                        var senderMail = new MailAddress(sender, "ISSUS");
                        var to = new MailAddress(this.Email);

                        using (var client = new SmtpClient
                        {
                            Host = "smtp.scrambotika.com",
                            Credentials = new System.Net.NetworkCredential(sender, pass),
                            Port = Constant.SmtpPort,
                            DeliveryMethod = SmtpDeliveryMethod.Network
                        })
                        {
                            string body = string.Format(
                                CultureInfo.GetCultureInfo("en-us"),
                                "Acceder a la siguiente url http://www.scrambotika.cat/Default.aspx?company={0} <br/> User:<b>{1}</b><br/>Password:<b>{2}</b>",
                                company.Code,
                                this.UserName,
                                userpass);

                            string templatePath = HttpContext.Current.Request.PhysicalApplicationPath;
                            if (!templatePath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
                            {
                                templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}\", templatePath);
                            }

                            string translatedTemplate = string.Format(
                                CultureInfo.InvariantCulture,
                                @"NewUser_{0}.tpl",
                                this.Language);

                            if (!File.Exists(templatePath + "Templates\\" + translatedTemplate))
                            {
                                templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}Templates\NewUser.tpl", templatePath);
                            }
                            else
                            {
                                templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}Templates\{1}", templatePath, translatedTemplate);
                            }

                            if (File.Exists(templatePath))
                            {
                                using (var input = new StreamReader(templatePath))
                                {
                                    body = input.ReadToEnd();
                                }

                                body = body.Replace("#USERNAME#", this.UserName).Replace("#PASSWORD#", userpass).Replace("#EMAIL#", this.Email).Replace("#EMPRESA#", company.Name);
                            }

                            var localDictionary = ApplicationDictionary.LoadLocal(this.Language);

                            var mail = new MailMessage(senderMail, to)
                            {
                                IsBodyHtml = true,
                                Subject = localDictionary["MailTemplate_Welcome_Subject"],
                                Body = body
                            };

                            client.Send(mail);
                        }

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
            }

            return res;
        }

        /// <summary>Determines if user has a grant over an item</summary>
        /// <param name="grant">Grant to determine</param>
        /// <param name="grantType">Item affected</param>
        /// <returns>If user has a grant over an item</returns>
        private bool CalculateHasGrant(int grant, int grantType)
        {
            foreach (var userGrant in this.grants)
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
                    default:
                        granted = false;
                        break;
                }

                if (userGrant.Item.Code == grant && granted)
                {
                    return true;
                }
            }

            return false;
        }
    }
}