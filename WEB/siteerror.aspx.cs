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

/// <summary>
/// Implements SiteError page
/// </summary>
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

    /// <summary>
    /// Event load of page
    /// </summary>
    /// <param name="sender">Page loaded</param>
    /// <param name="e">Arguments of event</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.company = this.Session["company"] as Company;
        this.dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.user = this.Session["User"] as ApplicationUser;
        Exception exception = Server.GetLastError();

        if (exception == null)
        {
            if (this.Session["Error"] != null)
            {
                exception = this.Session["Error"] as Exception;
            }
        }

        this.ErrorMessage.Text = this.dictionary["Common_Error_Application"];
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
        this.master.AddBreadCrumb("Common_Error");
        this.master.Titulo = "Common_Error";

        /*MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("mail.scrambotika.com");
        mail.From = new MailAddress("issus@scrambotika.com", "ISSUS");
        mail.IsBodyHtml = true;
        mail.To.Add("althera2004@gmail.com");

        mail.Subject = "Error en la aplicacion";
        mail.Body = this.ErrorMessage.Text;

        SmtpServer.Port = 25;
        SmtpServer.Credentials = new System.Net.NetworkCredential("issus@scrambotika.com", "WSBhz7WB");
        SmtpServer.Send(mail);*/
    }
}