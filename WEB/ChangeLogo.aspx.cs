using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SbrinnaCoreFramework.UI;
using System.Drawing;
using System.IO;

public partial class ChangeLogo : Page
{
    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpPostedFile file = this.Request.Files[0];
        string path = Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }

        string fileName = string.Format(@"{0}images\Logos\{1}.jpg", path, Session["CompanyId"].ToString());

        /*int cont = 0;
        while (File.Exists(fileName))
        {
            cont++;
            fileName = string.Format(@"{0}images\Logos\{1} ({2}).jpg", path, Session["CompanyId"].ToString(), cont);
        }*/

        file.SaveAs(fileName);
        //System.Drawing.Image logo = System.Drawing.Image.FromFile(fileName);
        //logo = ScaleImage(logo, 1000, 60);
        //Bitmap logob = new Bitmap(logo);
        //logob.Save(fileName);

        this.Response.Clear();
        this.Response.ContentType = "application/json";
        this.Response.Write(ImageSelector.SizeJson(string.Format(@"images\Logos\{0}.jpg", Session["CompanyId"].ToString()), 300, 300));
        this.Response.End();
    }

    public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
    {
        var ratioX = (decimal)((decimal)maxWidth / (decimal)image.Width);
        var ratioY = (decimal)((decimal)maxHeight / (decimal)image.Height);
        var ratio = Math.Min(ratioX, ratioY);

        var newWidth = (int)(Convert.ToDecimal(image.Width) * ratio);
        var newHeight = (int)(Convert.ToDecimal(image.Height) * ratio);

        var newImage = new Bitmap(newWidth, newHeight);

        using (var graphics = Graphics.FromImage(newImage))
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

        return newImage;
    }
}