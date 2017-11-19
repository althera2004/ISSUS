// --------------------------------
// <copyright file="LoginActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
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
    using GisoFramework.DataAccess;
    using GisoFramework.Item;
    using GisoFramework.LogOn;
    using SbrinnaCoreFramework;

    /// <summary>
    /// Summary description for LoginActions
    /// </summary>
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
        public ActionResult GetLogin(string userName, string password, string ip)
        {
            ActionResult res = ActionResult.NoAction;
            res = ApplicationLogOn.GetApplicationAccess(userName, password, ip);

            if (res.Success)
            {
                LogOnObject logon = res.ReturnValue as LogOnObject;
                int userId = logon.Id;
                Session["UniqueSessionId"] = UniqueSession.SetSession(userId, ip);
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangeAvatar(int userId, string avatar, int companyId)
        {
            ActionResult res = ApplicationUser.ChangeAvatar(userId, avatar, companyId);
            if (res.Success)
            {
                if (res.Success)
                {
                    ApplicationUser user = Session["User"] as ApplicationUser;
                    HttpContext.Current.Session["User"] = new ApplicationUser(userId);
                }
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult InsertUser(ApplicationUser itemUser, long employeeId, int userId)
        {
            //itemUser.UserName = itemUser.UserName.Replace(" ", string.Empty).ToUpperInvariant();
            //ActionResult res = itemUser.Insert(userId);
            /*CREATE PROCEDURE ApplicationUser_Insert
             *   @Id int output,
             *   @CompanyId int,
             *   @Login nvarchar(50),
             *   @Email nvarchar(50),
             *   @Password nvarchar(50) */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_Insert"))
            {
                string userpass = GisoFramework.RandomPassword.Generate(5, 8);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", itemUser.CompanyId));
                cmd.Parameters.Add(DataParameter.Input("@Login", itemUser.UserName, 50));
                cmd.Parameters.Add(DataParameter.Input("@Email", itemUser.Email, 50));
                cmd.Parameters.Add(DataParameter.Input("@Password", userpass));
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.ReturnValue = Convert.ToInt32(cmd.Parameters["@Id"].Value);

                    Company company = new Company(itemUser.CompanyId);

                    string sender = ConfigurationManager.AppSettings["mailaddress"];
                    string pass = ConfigurationManager.AppSettings["mailpass"];

                    MailAddress senderMail = new MailAddress(sender, "ISSUS");
                    MailAddress to = new MailAddress(itemUser.Email);

                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.scrambotika.com";
                    client.Credentials = new System.Net.NetworkCredential(sender, pass);
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    string body = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        "Acceder a la siguiente url http://www.scrambotika.cat/Default.aspx?company={0} <br/> User:<b>{1}</b><br/>Password:<b>{2}</b>",
                        company.Code,
                        itemUser.UserName,
                        userpass);

                    string templatePath = HttpContext.Current.Request.PhysicalApplicationPath;
                    if (!templatePath.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                    {
                        templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}\Templates\NewUser.tpl", templatePath);
                    }
                    else
                    {

                        templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}Templates\NewUser.tpl", templatePath);
                    }

                    if (File.Exists(templatePath))
                    {
                        using (StreamReader input = new StreamReader(templatePath))
                        {
                            body = input.ReadToEnd();
                        }

                        body = body.Replace("#USERNAME#", itemUser.UserName).Replace("#PASSWORD#", userpass).Replace("#EMAIL#", itemUser.Email);
                    }

                    MailMessage mail = new MailMessage(senderMail, to);
                    mail.IsBodyHtml = true;
                    mail.Subject = "Benvingut/uda a ISSUS";			
                    mail.Body = body;
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

            if(res.Success && employeeId > 0)
            {
                var id = Convert.ToInt32(res.MessageError);
                Employee employee = new Employee() { Id = employeeId, UserId = id, CompanyId = itemUser.CompanyId };
                res = employee.SetUser();
            }

            Company companySession = new Company(itemUser.CompanyId);
            HttpContext.Current.Session["Company"] = companySession;

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangeUserName(ApplicationUser itemUser, long employeeId, int userId)
        {
            //itemUser.UserName = itemUser.UserName.Replace(" ", string.Empty).ToUpperInvariant();
            ActionResult res = itemUser.Update(userId);
            if (res.Success)
            {
                if (employeeId > 0)
                {
                    Employee employee = new Employee() { Id = employeeId, UserId = itemUser.Id, CompanyId = itemUser.CompanyId };
                    res = employee.SetUser();
                }
                else
                {
                    res = ApplicationUser.UnsetEmployee(itemUser.Id, itemUser.CompanyId);
                }
            }

            Company companySession = new Company(itemUser.CompanyId);
            HttpContext.Current.Session["Company"] = companySession;
            HttpContext.Current.Session["User"] = ApplicationUser.GetById(itemUser.Id, itemUser.CompanyId);
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangePassword(int userId, string oldPassword, string newPassword, int companyId)
        {
            ActionResult res = ApplicationUser.ChangePassword(userId, oldPassword, newPassword, companyId);
            if (res.MessageError == "NOPASS")
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                if (dictionary != null)
                {
                    res.MessageError = dictionary["Common_Error_IncorrectPassword"];
                }
                else
                {
                    res.MessageError = "Incorrect password";
                }
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ResetPassword(int userId, int companyId)
        {
            //return ApplicationUser.ResetPassword(userId, companyId);
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
                    Company company = new Company(companyId);

                    string userName = cmd.Parameters["@UserName"].Value as string;
                    string password = cmd.Parameters["@Password"].Value as string;
                    string email = cmd.Parameters["@Email"].Value as string;

                    string sender = ConfigurationManager.AppSettings["mailaddress"];
                    string pass = ConfigurationManager.AppSettings["mailpass"];

                    MailAddress senderMail = new MailAddress(sender, "ISSUS");
                    MailAddress to = new MailAddress(email);

                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.scrambotika.com";
                    client.Credentials = new System.Net.NetworkCredential(sender, pass);
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    MailMessage mail = new MailMessage(senderMail, to);
                    mail.IsBodyHtml = true;
                    mail.Subject = "Reinicio de contraseña en ISSUS";

                    string templatePath = HttpContext.Current.Request.PhysicalApplicationPath;
                    if (!templatePath.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
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

                    if (File.Exists(templatePath))
                    {
                        using (StreamReader input = new StreamReader(templatePath))
                        {
                            body = input.ReadToEnd();
                        }

                        body = body.Replace("#USERNAME#", userName).Replace("#PASSWORD#", password).Replace("#EMAIL#", email);
                    }


                    mail.Subject = "La teva nova contrasenya d'accés a ISSUS";
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

        [WebMethod(EnableSession = true)]
        public ActionResult SaveProfile(int userId, int companyId, string language, bool showHelp, int? blue, int? green, int? yellow, int? red)
        {
            ActionResult res = ApplicationUser.UpdateInterfaceProfile(userId, companyId, language, showHelp);
            if (res.Success)
            {
                res = ApplicationUser.UpdateShortcuts(userId, companyId, green, blue, yellow, red);
                if (res.Success)
                {
                    ApplicationUser user = Session["User"] as ApplicationUser;
                    user.Language = language;
                    HttpContext.Current.Session["User"] = new ApplicationUser(userId);
                    Dictionary<string, string> dictionary = ApplicationDictionary.Load("ca");
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
            string[] grantList = grants.Split('|');
            List<UserGrant> userGrants = new List<UserGrant>();

            foreach (string grant in grantList)
            {
                if (!string.IsNullOrEmpty(grant))
                {
                    string action = grant.Substring(0, 1);
                    string code = grant.Substring(1);
                    UserGrant item = new UserGrant()
                    {
                        UserId = itemUserId,
                        Item = new ApplicationGrant()
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

            ActionResult res = ActionResult.NoAction;
            res.SetSuccess();
            ApplicationUser.ClearGrant(itemUserId);
            foreach (UserGrant ug in userGrants)
            {
                ActionResult resTemp = ApplicationUser.SaveGrant(ug, userId);
                if (!resTemp.Success)
                {
                    res.SetFail(string.Empty);
                    res.MessageError += resTemp.MessageError;
                }
            }

            return res;
        }
    }
}