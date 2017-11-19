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
        string subject = "Mail de test desde www.scrambotika.com";
        string body = "Mail de test desde www.scrambotika.com";
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.scrambotika.com");
        mail.From = new MailAddress("issus@scrambotika.com", "ISSUS");
        mail.IsBodyHtml = true;
        mail.To.Add("hola@scrambotika.com");
        mail.CC.Add("althera2004@gmail.com");

        mail.Subject = subject;
        mail.Body = body;

        SmtpServer.Port = 25;
        SmtpServer.EnableSsl = false;
        SmtpServer.Credentials = new System.Net.NetworkCredential("issus@scrambotika.com", "W3&S1B%h7Jz%7W7f5$%B");
        SmtpServer.Send(mail);
    }
}