// --------------------------------
// <copyright file="ApplicationUser.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Security
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
    using OpenFramework.Core;
    using OpenFramework.Core.Bindings;
    using OpenFramework.Core.DataAccess;
    using OpenFramework.Core.Product;
    using System.Web;

    /// <summary>
    /// Implements class to manage application user
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>Instance where user is part</summary>
        private List<Instance> instances;

        /// <summary>
        /// Gets an empty user
        /// </summary>
        public static ApplicationUser Empty
        {
            get
            {
                return new ApplicationUser()
                {
                    Id = -1,
                    Email = string.Empty,
                    Profile = UserProfile.Empty,
                    instances = new List<Instance>()
                };
            }
        }

        /// <summary>
        /// Gets a JSON list of all users in the current instance
        /// </summary>
        public static string GetAllJson
        {
            get
            {
                return JsonList(GetAll);
            }
        }

        /// <summary>
        /// Gets all users of current instance
        /// </summary>
        public static ReadOnlyCollection<ApplicationUser> GetAll
        {
            get
            {
                List<ApplicationUser> res = new List<ApplicationUser>();
                using (SqlCommand cmd = Command.Stored("User_GetAll", ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsApplicationUserGetAll.Id),
                                    Email = rdr.GetString(ColumnsApplicationUserGetAll.Email),
                                    FirstName = rdr.GetString(ColumnsApplicationUserGetAll.FirstName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGetAll.LastName),
                                    Core = rdr.GetBoolean(ColumnsApplicationUserGetAll.Core),
                                    Locked = rdr.GetBoolean(ColumnsApplicationUserGetAll.Locked),
                                    Active = rdr.GetBoolean(ColumnsApplicationUserGetAll.Active),
                                    CreatedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt64(ColumnsApplicationUserGetAll.CreatedBy),
                                        FirstName = rdr.GetString(ColumnsApplicationUserGetAll.CreatedByFirstName),
                                        LastName = rdr.GetString(ColumnsApplicationUserGetAll.CreatedByLastName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsApplicationUserGetAll.CreatedOn),
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt64(ColumnsApplicationUserGetAll.ModifiedBy),
                                        FirstName = rdr.GetString(ColumnsApplicationUserGetAll.ModifiedByFirstName),
                                        LastName = rdr.GetString(ColumnsApplicationUserGetAll.ModifiedByLastName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsApplicationUserGetAll.ModifiedOn)
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

                return new ReadOnlyCollection<ApplicationUser>(res);
            }
        }

        /// <summary>Gets or sets user identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets user email address</summary>
        public string Email { get; set; }

        /// <summary>Gets or sets user profile</summary>
        public UserProfile Profile { get; set; }

        /// <summary>Gets or sets the number of previous failed logon attempts</summary>
        public int FailedSignIn { get; set; }

        /// <summary>Gets or sets a value indicating whether user must reset password at next logon</summary>
        public bool MustResetPassword { get; set; }

        /// <summary>Gets or sets a value indicating whether user is locked</summary>
        public bool Locked { get; set; }

        /// <summary>Gets or sets a value indicating whether user is active</summary>
        public bool Active { get; set; }

        /// <summary>Gets or sets a value indicating whether user is member of core</summary>
        public bool Core { get; set; }

        /// <summary>Gets or sets the user that creates this</summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets the date of creation</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the user that make the last modification</summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets the date of last modification</summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>Gets or sets the first name of user</summary>
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name of user</summary>
        public string LastName { get; set; }

        /// <summary>Gets the full name of user</summary>
        public string FullName
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    Tools.JsonCompliant(this.FirstName),
                    Tools.JsonCompliant(this.LastName)).Trim();
            }
        }

        /// <summary>Gets the frameworks on user has access</summary>
        public ReadOnlyCollection<Instance> Frameworks
        {
            get
            {
                if (this.instances == null)
                {
                    this.instances = new List<Instance>();
                }

                return new ReadOnlyCollection<Instance>(this.instances);
            }
        }

        /// <summary>Gets user description</summary>
        public string Description
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FullName))
                {
                    return this.FullName;
                }

                return this.Email;
            }
        }

        /// <summary>
        /// Gets the JSON structure of user
        /// </summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Email"":""{1}"", ""FirstName"":""{2}"", ""LastName"":""{3}"", ""FullName"":""{4}"", ""Core"":{5}, ""Active"":{6},  ""Locked"":{7}}}",
                    this.Id,
                    this.Email,
                    Tools.JsonCompliant(this.FirstName),
                    Tools.JsonCompliant(this.LastName),
                    Tools.JsonCompliant(this.FullName),
                    this.Core ? ConstantValue.True : ConstantValue.False,
                    this.Active ? ConstantValue.True : ConstantValue.False,
                    this.Locked ? ConstantValue.True : ConstantValue.False);
            }
        }

        /// <summary>
        /// Gets the JSON key/value structure of user
        /// </summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Value"":""{1}""}}", this.Id, this.Description);
            }
        }

        /// <summary>
        /// Gets a JSON list of users collection
        /// </summary>
        /// <param name="list">Collection of users</param>
        /// <returns>JSON list of users collection</returns>
        public static string JsonList(ReadOnlyCollection<ApplicationUser> list)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            if (list != null)
            {
                foreach (ApplicationUser user in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(user.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Try to logon
        /// </summary>
        /// <param name="email">Email that identifies user</param>
        /// <param name="password">User password</param>
        /// <returns>Result of action</returns>
        public static ActionResult LogOn(string email, string password)
        {
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Login"))
            {
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@Email", email, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Password", password, 50));
                    try
                    {
                        ApplicationUser user = ApplicationUser.Empty;
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                user = ApplicationUser.GetById(rdr.GetInt64(0));
                                user.GetInstances();

                                if (user.instances.Count > 0)
                                {
                                    if(user.instances.Any(i=>!i.Name.Equals("core", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        HttpContext.Current.Session["Instance"] = user.instances.Where(i => i.Name.Equals("gese", StringComparison.OrdinalIgnoreCase)).First();
                                    }
                                }

                                res.SetSuccess(user);
                            }
                            else
                            {
                                res.SetFail("NOLOGIN");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, "Login(" + email + ")");
                        res.SetFail(ex);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Gets an user for current instance by identifier
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Application user of current instance if exists</returns>
        public static ApplicationUser GetById(long userId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "ApplicationUser::GetById({0})", userId);
            ApplicationUser res = ApplicationUser.Empty;
            /* CREATE PROCEDURE Core_User_GetById
             *   @UserId bigint */
            using (SqlCommand cmd = new SqlCommand("User_GetById"))
            {
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Id = rdr.GetInt64(ColumnsApplicationUserGet.Id);
                                res.Core = rdr.GetBoolean(ColumnsApplicationUserGet.Core);
                                res.FirstName = rdr.GetString(ColumnsApplicationUserGet.FirstName);
                                res.LastName = rdr.GetString(ColumnsApplicationUserGet.LastName);
                                res.Email = rdr.GetString(ColumnsApplicationUserGet.Email);
                                res.MustResetPassword = rdr.GetBoolean(ColumnsApplicationUserGet.MustResetPassword);
                                res.FailedSignIn = rdr.GetInt32(ColumnsApplicationUserGet.FailedSignIn);
                                res.Locked = rdr.GetBoolean(ColumnsApplicationUserGet.Locked);
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsApplicationUserGet.CreatedBy),
                                    FirstName = rdr.GetString(ColumnsApplicationUserGet.FirstName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGet.LastName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsApplicationUserGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt64(ColumnsApplicationUserGet.ModifiedBy),
                                    FirstName = rdr.GetString(ColumnsApplicationUserGet.ModifiedByFirtName),
                                    LastName = rdr.GetString(ColumnsApplicationUserGet.ModifiedByLastName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsApplicationUserGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsApplicationUserGet.Active);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Load profile associated to user in the current instance
        /// </summary>
        public void LoadProfile()
        {
            this.Profile = UserProfile.GetByUserId(this.Id);
        }

        public void GetInstances()
        {
            string source = string.Format(CultureInfo.InvariantCulture, "ApplicationUser::GetInstances({0})", this.Id);
            this.instances = new List<Instance>();
            using(SqlCommand cmd = new SqlCommand("User_GetInstances"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", this.Id));
                using(SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Security"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cnn.Open();
                        using(SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while(rdr.Read())
                            {
                                Instance NewInstance = new Instance()
                                {
                                    Id = rdr.GetInt64(ColumnsInstanceGet.Id),
                                    Key = rdr.GetString(ColumnsInstanceGet.Key),
                                    MultiCore = rdr.GetBoolean(ColumnsInstanceGet.MultiCore),
                                    Active = rdr.GetBoolean(ColumnsInstanceGet.Active),
                                    DefaultLanguage = DataPersistence.LanguageById((long)rdr.GetInt32(ColumnsInstanceGet.DefaultLanguage)),
                                    Name = rdr.GetString(ColumnsInstanceGet.Name),
                                    UsersLimit = rdr.GetInt32(ColumnsInstanceGet.UsersLimit),
                                    SubscriptionStart = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionStart),
                                    SubscriptionEnd = rdr.GetDateTime(ColumnsInstanceGet.SubscriptionEnd),
                                    SubscriptionType = rdr.GetInt32(ColumnsInstanceGet.SubscriptionType)
                                };

                                NewInstance.GetLanguages();
                                this.instances.Add(NewInstance);
                            }
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                    catch (NotSupportedException ex)
                    {
                        ExceptionManager.LogException(ex as Exception, source);
                    }
                }
            }
        }
    }
}