// --------------------------------
// <copyright file="LoginActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GISOWeb
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using GisoFramework;
    using GisoFramework.Activity;
    using GisoFramework.Item;
    using GisoFramework.LogOn;
    using SbrinnaCoreFramework;

    /// <summary>Summary description for LoginActions</summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class LoginActions : WebService
    {
        public LoginActions()
        {
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult GetLogin(string email, string password, string ip)
        {
            var res = ApplicationLogOn.GetApplicationAccess(email, password, ip);
            if (res.Success)
            {
                var logon = res.ReturnValue as LogOnObject;
                int userId = logon.Id;
                HttpContext.Current.Session["UniqueSessionId"] = UniqueSession.SetSession(userId, ip);
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangeAvatar(int userId, string avatar, int companyId)
        {
            var res = ApplicationUser.ChangeAvatar(userId, avatar, companyId);
            if (res.Success)
            {
                if (res.Success)
                {
                    var user = Session["User"] as ApplicationUser;
                    HttpContext.Current.Session["User"] = new ApplicationUser(userId);
                }
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult InsertUser(ApplicationUser itemUser, long employeeId, int userId)
        {
            var res = itemUser.Insert(userId);
            if(res.Success && employeeId > 0)
            {
                var id = Convert.ToInt32(res.MessageError);
                var employee = new Employee { Id = employeeId, UserId = id, CompanyId = itemUser.CompanyId };
                res = employee.SetUser();
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangeUserName(ApplicationUser itemUser, long employeeId, int userId)
        {
            var res = itemUser.Update(userId);
            if (res.Success)
            {
                if (employeeId > 0)
                {
                    var employee = new Employee { Id = employeeId, UserId = itemUser.Id, CompanyId = itemUser.CompanyId };
                    res = employee.SetUser();
                }
                else
                {
                    res = ApplicationUser.UnsetEmployee(itemUser.Id, itemUser.CompanyId);
                }
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangePassword(int userId, string oldPassword, string newPassword, int companyId)
        {
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            var res = ApplicationUser.ChangePassword(userId, oldPassword, newPassword, companyId);
            if (res.MessageError == "NOPASS")
            {
                if (dictionary != null)
                {
                    res.MessageError = dictionary["Common_Error_IncorrectPassword"];
                }
                else
                {
                    res.MessageError = "Incorrect password";
                }
            }

            var userFromDB = ApplicationUser.GetById(userId, companyId);
            var company = new Company(companyId);
            if (userFromDB.PrimaryUser)
            {
                SendMailUserMother(userFromDB, company.Name);
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ResetPassword(int userId, int companyId)
        {
            //return ApplicationUser.ResetPassword(userId, companyId);
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
						int port = Convert.ToInt32(ConfigurationManager.AppSettings["mailport"]);
                        string server = ConfigurationManager.AppSettings["mailserver"];

                        var senderMail = new MailAddress(sender, "ISSUS");
                        var to = new MailAddress(email);

                        using (var client = new SmtpClient
                        {
                            Host = server,
                            Credentials = new System.Net.NetworkCredential(sender, pass),
                            Port = port,
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

        [WebMethod(EnableSession = true)]
        public ActionResult SaveProfile(int userId, int companyId, string language, bool showHelp, int? blue, int? green, int? yellow, int? red)
        {
            var res = ApplicationUser.UpdateInterfaceProfile(userId, companyId, language, showHelp);
            if (res.Success)
            {
                res = ApplicationUser.UpdateShortcuts(userId, companyId, green, blue, yellow, red);
                if (res.Success)
                {
                    var user = Session["User"] as ApplicationUser;
                    user.Language = language;
                    HttpContext.Current.Session["User"] = new ApplicationUser(userId);
                    var dictionary = ApplicationDictionary.Load("ca");
                    if (user.Language != "ca")
                    {
                        dictionary = ApplicationDictionary.LoadNewLanguage(language);
                    }
                }
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        public ActionResult Grants(string grants, int itemUserId, int userId)
        {
            var grantList = grants.Split('|');
            var userGrants = new List<UserGrant>();
            foreach (string grant in grantList)
            {
                if (!string.IsNullOrEmpty(grant))
                {
                    string action = grant.Substring(0, 1);
                    string code = grant.Substring(1);
                    var item = new UserGrant
                    {
                        UserId = itemUserId,
                        Item = new ApplicationGrant
                        {
                            Code = Convert.ToInt32(code)
                        },
                        GrantToDelete = false,
                        GrantToRead = action == "R",
                        GrantToWrite = action == "W"
                    };

                    bool found = false;
                    foreach (UserGrant ug in userGrants)
                    {
                        if (ug.Item.Code == item.Item.Code)
                        {
                            ug.GrantToRead = ug.GrantToRead || item.GrantToRead;
                            ug.GrantToWrite = ug.GrantToWrite || item.GrantToWrite;
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        userGrants.Add(item);
                    }
                }
            }

            var res = ActionResult.NoAction;
            res.SetSuccess();
            ApplicationUser.ClearGrant(itemUserId);
            foreach (var ug in userGrants)
            {
                var resTemp = ApplicationUser.SaveGrant(ug, userId);
                if (!resTemp.Success)
                {
                    res.SetFail(string.Empty);
                    res.MessageError += resTemp.MessageError;
                }
            }

            return res;
        }

        private void SendMailUserMother(ApplicationUser user, string companyName)
        {
            string sender = ConfigurationManager.AppSettings["mailaddress"];
            string pass = ConfigurationManager.AppSettings["mailpass"];
            var senderMail = new MailAddress(sender, "ISSUS");
            var server = ConfigurationManager.AppSettings["mailserver"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["mailport"]);
            var to = new MailAddress(ConfigurationManager.AppSettings["mailaddress"]);

            var client = new SmtpClient
            {
                Host = server,
                Credentials = new System.Net.NetworkCredential(sender, pass),
                Port = port,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mail = new MailMessage(senderMail, to)
            {
                IsBodyHtml = true
            };            

            string body = string.Format(
                CultureInfo.InvariantCulture,
                "Se ha reestablecido la contraseña en ISSUS de un administrador primario.<br />User:<b>{0}</b><br/>Empresa:<b>{1}</b>",
                user.UserName,
                companyName);

            var userDictionary = ApplicationDictionary.Load(user.Language);
            mail.Subject = userDictionary["Item_User_MailResetPassword_Subject"];
            mail.Body = body;
            client.Send(mail);
        }
    }
}