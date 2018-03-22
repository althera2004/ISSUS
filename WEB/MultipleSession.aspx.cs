using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MultipleSession : Page
{

    public string BK
    {
        get
        {
            string path = this.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}WelcomeBackgrounds\", path);

            var files = Directory.GetFiles(path);
            var rnd = new Random();
            int index = rnd.Next(0, files.Count() - 1);
            string res = Path.GetFileName(files[index]);
            Session["BK"] = res;
            return res;
        }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Session["UserId"] = null;
        this.Session["CompanyId"] = null;
        this.Session["User"] = null;
        this.Session["IncidentFilter"] = null;
        this.Session["IncidentActionFilter"] = null;
        this.Session["EquipmentFilter"] = null;
        if (this.Request.QueryString["company"] != null)
        {
            this.Response.Redirect(string.Format("Default.aspx?company={0}", this.Request.QueryString["company"]), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}