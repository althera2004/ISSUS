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
            //SmtpClient SmtpServer = new SmtpClient("afacreanova-org.correoseguro.dinaserver.com");
            //mail.From = new MailAddress("info@afacreanova.org", "ISSUS");
			SmtpClient SmtpServer = new SmtpClient("mail.scrambotika.com");
			mail.From = new MailAddress("issus@scrambotika.com", "ISSUS");
						
			mail.IsBodyHtml = true;
            mail.To.Add("alex@gotikasoftware.com");
            mail.CC.Add("althera2004@gmail.com");

            mail.Subject = subject;
            mail.Body = body;

            SmtpServer.Port = 587;
            //SmtpServer.EnableSsl = true;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("info@afacreanova.org", "EGic0D54!*/2");
			SmtpServer.Credentials = new System.Net.NetworkCredential("issus@scrambotika.com", "Gu2PdtC5sYJ3FNs8BnPk");
            SmtpServer.Send(mail);
            this.LtMail.Text = "ok";
        }
        catch(Exception ex)
        {
            this.LtMail.Text = ex.Message;
        }

    }
}