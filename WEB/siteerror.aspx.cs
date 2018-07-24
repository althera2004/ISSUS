// --------------------------------
// <copyright file="SiteError.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using System.Net.Mail;
using System.Configuration;

/// <summary>Implements SiteError page</summary>
public partial class SiteError : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Event load of page</summary>
    /// <param name="sender">Page loaded</param>
    /// <param name="e">Arguments of event</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.company = this.Session["company"] as Company;
            this.dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
            this.user = this.Session["User"] as ApplicationUser;
            var exception = Server.GetLastError();

            if (exception == null)
            {
                if (this.Session["Error"] != null)
                {
                    exception = this.Session["Error"] as Exception;
                }
            }

            this.ErrorMessage.Text = "Application Error";
            if (this.dictionary != null)
            {
                this.ErrorMessage.Text = "Error";
            }

            if (exception != null)
            {
                if (exception.InnerException != null)
                {
                    this.ErrorMessage.Text = exception.InnerException.Message + "<hr />" + exception.InnerException.StackTrace;
                }
                else
                {
                    this.ErrorMessage.Text = exception.Message + "<hr />" + exception.StackTrace;
                }
            }

            this.master = this.Master as Giso;
            this.master.AddBreadCrumbInvariant("Error");
            this.master.Titulo = "Error";
            this.master.TitleInvariant = true;

            string from = ConfigurationManager.AppSettings["mailaddress"];
            string pass = ConfigurationManager.AppSettings["mailpass"];
            var senderMail = new MailAddress(from, "ISSUS");
            var to = new MailAddress(ConfigurationManager.AppSettings["mailaddress"]);

            using (var client = new SmtpClient
            {
                Host = "smtp.scrambotika.com",
                Credentials = new System.Net.NetworkCredential(from, pass),
                Port = Constant.SmtpPort,
                DeliveryMethod = SmtpDeliveryMethod.Network
            })
            {
                var errorText = "Error no definido";
                if(exception != null)
                {
                    errorText = exception.Message;
                }

                using (var mail = new MailMessage(senderMail, to)
                {
                    IsBodyHtml = true,
                    Body = errorText,
                    Subject = "Error en " + ConfigurationManager.AppSettings["issusVersion"].ToString()
                })
                {
                    //client.Send(mail);
                }
            }
        }
        catch (Exception ex)
        {
            this.ErrorMessage.Text = ex.Message;
        }
    }
}