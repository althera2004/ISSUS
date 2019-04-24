using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestMail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            string subject = "Mail de test desde www.scrambotika.com";
            string body = "Mail de test desde www.scrambotika.com";
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("issus.scrambotika.cat");
            mail.From = new MailAddress("info@issus.scrambotika.cat", "ISSUS");
            mail.IsBodyHtml = true;
            mail.To.Add("jcastilla@openframework.es");
            mail.CC.Add("althera2004@gmail.com");

            mail.Subject = subject;
            mail.Body = body;

            SmtpServer.Port = 25;
            SmtpServer.EnableSsl = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("info@issus.scrambotika.cat", "$Cramb0tika");
            SmtpServer.Send(mail);
            this.LtMail.Text = "ok";
        }
        catch(Exception ex)
        {
            this.LtMail.Text = ex.Message;
        }

    }
}