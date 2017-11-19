using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeleteTempFiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        path += "DOCS\\";

        string[] files = Directory.GetFiles(path);

        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.LastAccessTime < DateTime.Now.AddMonths(-3))
            {
                this.ltfiles.Text += fi.Name + "<br />";
                fi.Delete();
            }
        }
    }
}