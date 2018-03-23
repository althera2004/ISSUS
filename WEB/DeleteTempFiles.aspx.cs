using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using GisoFramework;

public partial class DeleteTempFiles : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        path += "DOCS\\";

        foreach (string file in Directory.GetFiles(path))
        {
            var fi = new FileInfo(file);
            if (fi.LastAccessTime < Constant.Now.AddMonths(-3))
            {
                this.ltfiles.Text += fi.Name + "<br />";
                fi.Delete();
            }
        }
    }
}