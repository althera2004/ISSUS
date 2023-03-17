// --------------------------------
// <copyright file="CompanyCreation.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Services;
using System;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Web;
using GisoFramework;
using GisoFramework.Activity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using GisoFramework.DataAccess;
using System.Collections.Generic;

/// <summary>Summary description for CompanyCreation</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CompanyCreation : WebService
{
    /// <summary>Character for separator</summary>
    private const string Separator = "|";

    public CompanyCreation()
    {
    }

    [WebMethod(EnableSession = true)]
    public string Create(
        string companyName,
        string companyCode,
        string companyNif,
        string companyAddress,
        string companyPostalCode,
        string companyCity,
        string companyProvince,
        string companyCountry,
        string companyPhone,
        string companyMobile,
        string companyFax,
        string userName,
        string companyEmail,
        string language)
    {
        var res = CreateDB(
            companyName,
            companyCode,
            companyNif,
            companyAddress,
            companyPostalCode,
            companyCity,
            companyProvince,
            companyCountry,
            companyPhone,
            companyMobile,
            companyFax,
            userName,
            companyEmail,
            language);

        if (res.Success)
        {
            var dictionary = ApplicationDictionary.Load("ca");
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            string destino = path;
            if (!path.EndsWith("\\", StringComparison.Ordinal))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\images\noimage.jpg", path);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\images\noimage.jpg", path);
            }

            if (!destino.EndsWith("\\", StringComparison.Ordinal))
            {
                destino = string.Format(CultureInfo.InvariantCulture, @"{0}\images\Logos\{1}.jpg", destino, res.MessageError.Split('|')[0]);
            }
            else
            {
                destino = string.Format(CultureInfo.InvariantCulture, @"{0}\images\Logos\{1}.jpg", destino, res.MessageError.Split('|')[0]);
            }

            //System.IO.File.Copy(path, destino);

            path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}Templates\WelcomeMail.tpl", path);
            string bodyPattern = string.Empty;
            using (var rdr = new StreamReader(path))
            {
                bodyPattern = rdr.ReadToEnd();
                bodyPattern = bodyPattern.Replace("#USERNAME#", "{2}");
                bodyPattern = bodyPattern.Replace("#EMAIL#", "{0}");
                bodyPattern = bodyPattern.Replace("#PASSWORD#", "{1}");
                bodyPattern = bodyPattern.Replace("#EMPRESA#", "{3}");
            }

            string subject = string.Format(dictionary["Mail_Message_WelcomeSubject"], res.MessageError.Split('|')[0]);
            string body = string.Format(
                CultureInfo.InvariantCulture,
                bodyPattern,
                res.MessageError.Split('|')[1],
                res.MessageError.Split('|')[2],
                res.MessageError.Split('|')[0],
                res.MessageError.Split('|')[3]);

            string sender = ConfigurationManager.AppSettings["mailaddress"];
            string pass = ConfigurationManager.AppSettings["mailpass"];
            var senderMail = new MailAddress(sender, "ISSUS");
            string server = ConfigurationManager.AppSettings["mailserver"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["mailport"]);

            var mail = new MailMessage
            {
                From = senderMail,
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };
            mail.To.Add("alex@scrambotika.com");
            //mail.To.Add("hola@scrambotika.com");
            //mail.CC.Add(companyEmail);

            var key = Tools.DecryptString(ConfigurationManager.AppSettings["mailpass"] as string);
            if (key.StartsWith("Error::", StringComparison.OrdinalIgnoreCase))
            {
                res.SetFail(key);
            }
            else
            {
                var smtpServer = new SmtpClient
                {
                    Host = server,
                    Port = port,
                    Credentials = new System.Net.NetworkCredential(sender, key),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtpServer.Send(mail);
            }
        }

        return res.MessageError;
    }

    /// <summary>Insert a new company in database</summary>
    /// <param name="companyName">Company name</param>
    /// <param name="companyCode">Company code</param>
    /// <param name="companyNif">Company nif</param>
    /// <param name="companyAddress">Company street address</param>
    /// <param name="companyPostalCode">Company postal code</param>
    /// <param name="companyCity">Company city</param>
    /// <param name="companyProvince">Company province</param>
    /// <param name="companyCountry">Company country</param>
    /// <param name="companyPhone">Company phone</param>
    /// <param name="companyMobile">Company mobile</param>
    /// <param name="companyFax">Company fax</param>
    /// <param name="companyEmail">Company email</param>
    /// <returns>Result of action</returns>
    public static ActionResult CreateDB(string companyName, string companyCode, string companyNif, string companyAddress, string companyPostalCode, string companyCity, string companyProvince, string companyCountry, string companyPhone, string companyMobile, string companyFax, string userName, string companyEmail, string language)
    {
        /* CREATE PROCEDURE Company_Create
         *   @CompanyId int out,
         *   @Login nvarchar(50) out,
         *   @Password nvarchar(50) out,
         *   @Name nvarchar(50),
         *   @Code nvarchar(10),
         *   @NIF nvarchar(15),
         *   @Address nvarchar(50),
         *   @PostalCode nvarchar(10),
         *   @City nvarchar(50),
         *   @Province nvarchar(50),
         *   @Country nvarchar(15),
         *   @Phone nvarchar(15),
         *   @Mobile nvarchar(15),
         *   @Email nvarchar(50),
         *   @Fax nvarchar(50),
         *   @EmployeeName nvarchar(50),
         *   @EmployeeLastName nvarchar(50),
         *   @EmployeeNif nvarchar(15),
         *   @EmployeePhone nvarchar(15),
         *   @UserName nvarchar(50),
         *   @EmployeeEmail nvarchar(50) */
        var res = ActionResult.NoAction;
        using (var cmd = new SqlCommand("Company_Create"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.OutputInt("@CompanyId"));
                    cmd.Parameters.Add(DataParameter.OutputString("@Login", 50));
                    cmd.Parameters.Add(DataParameter.OutputString("@Password", 50));
                    cmd.Parameters.Add(DataParameter.Input("@Name", companyName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Code", companyCode, 10));
                    cmd.Parameters.Add(DataParameter.Input("@NIF", companyNif, 15));
                    cmd.Parameters.Add(DataParameter.Input("@Address", companyAddress, 50));
                    cmd.Parameters.Add(DataParameter.Input("@PostalCode", companyPostalCode, 10));
                    cmd.Parameters.Add(DataParameter.Input("@City", companyCity, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Province", companyProvince, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Country", companyCountry, 15));
                    cmd.Parameters.Add(DataParameter.Input("@Phone", companyPhone, 15));
                    cmd.Parameters.Add(DataParameter.Input("@Mobile", companyMobile, 15));
                    cmd.Parameters.Add(DataParameter.Input("@UserName", userName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Email", companyEmail, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Fax", companyFax, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Language", language, 2));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    int companyId = Convert.ToInt32(cmd.Parameters["@CompanyId"].Value.ToString());
                    string path = path = HttpContext.Current.Request.PhysicalApplicationPath;
                    if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                    {
                        path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
                    }

                    var directory = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}DOCS\{1}",
                        path,
                        companyId);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    res.SetSuccess(userName + Separator + companyEmail + Separator + cmd.Parameters["@Password"].Value.ToString() + Separator + companyName);
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (InvalidOperationException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
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
}