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
            var res = ApplicationUser.ChangePassword(userId, oldPassword, newPassword, companyId);
            if (res.MessageError == "NOPASS")
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
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
                SendMailUserMother(userFromDB.UserName, company.Name);
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ResetPassword(int userId, int companyId)
        {
            return ApplicationUser.ResetPassword(userId, companyId);
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
            foreach (UserGrant ug in userGrants)
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

        private void SendMailUserMother(string userName, string companyName)
        {
            string sender = ConfigurationManager.AppSettings["mailaddress"];
            string pass = ConfigurationManager.AppSettings["mailpass"];
            var senderMail = new MailAddress(sender, "ISSUS");
            var to = new MailAddress(ConfigurationManager.AppSettings["mailaddress"]);

            var client = new SmtpClient()
            {
                Host = "smtp.scrambotika.com",
                Credentials = new System.Net.NetworkCredential(sender, pass),
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mail = new MailMessage(senderMail, to)
            {
                IsBodyHtml = true
            };            

            string body = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                "Se ha reestablecido la contraseña en ISSUS de un administrador primario.<br />User:<b>{0}</b><br/>Empresa:<b>{1}</b>",
                userName,
                companyName);

            mail.Subject = "Reinicio de contraseña en ISSUS de usuario primario";
            mail.Body = body;
            client.Send(mail);
        }
    }
}