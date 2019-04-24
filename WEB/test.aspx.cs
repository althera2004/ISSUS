// --------------------------------
// <copyright file="test.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using SbrinnaCoreFramework.UI;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using GisoFramework.Item;
using System.Globalization;
using System.Net.Mail;

public partial class test : Page
{
    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string cns = ConfigurationManager.ConnectionStrings["cns"].ConnectionString;
        this.ltCns.Text = "Connection string: " + cns + "<br />";

        var cnn = new SqlConnection(cns);
        try
        {
            cnn.Open();
            this.ltCnn.Text = "Conexión sql: OK";

            this.ltCnn.Text += "<br />Application Path:" + this.Request.PhysicalApplicationPath;
            this.ltCnn.Text += "<br />Root Path:" + this.Server.MapPath("~");

        }
        catch (Exception ex)
        {
            this.ltCnn.Text = "Conexión sql:" + ex.Message;
        }
        finally
        {
            if (cnn.State != ConnectionState.Closed)
            {
                cnn.Close();
            }
            cnn.Dispose();
        }

        Employee em = new Employee();
    }

    private void SendMail()
    {
        string sender = ConfigurationManager.AppSettings["mailaddress"];
        string pass = ConfigurationManager.AppSettings["mailpass"];

        var senderMail = new MailAddress(sender, "ISSUS");
        var to = new MailAddress("jcastilla@openframework.es");

        var client = new SmtpClient
        {
            Host = "smtp.scrambotika.com",
            Credentials = new System.Net.NetworkCredential(sender, pass),
            Port = 25,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        var mail = new MailMessage(senderMail, to)
        {
            IsBodyHtml = true,
            Subject = "test email",
            Body = "test"
        };

        client.Send(mail);
    }

    protected void BtnMail_Click(object sender, EventArgs e)
    {
        try
        {
            SendMail();
            this.ltMail.Text = "Mail ok";
        }
        catch(Exception ex)
        {
            this.ltMail.Text = ex.Message;
        }
    }
}