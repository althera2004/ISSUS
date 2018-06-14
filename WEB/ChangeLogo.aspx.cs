using System;
using System.Web.UI;
using SbrinnaCoreFramework.UI;

public partial class ChangeLogo : Page
{
    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        var file = this.Request.Files[0];
        string path = Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }

        string fileName = string.Format(@"{0}images\Logos\{1}.jpg", path, Session["CompanyId"].ToString());
        file.SaveAs(fileName);
        this.Response.Clear();
        this.Response.ContentType = "application/json";
        this.Response.Write(ImageSelector.SizeJson(string.Format(@"images\Logos\{0}.jpg", Session["CompanyId"].ToString()), 300, 300));
        this.Response.End();
    }
}