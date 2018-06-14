using System;
using System.Web;
using System.Web.UI;

public partial class ChangeEquipmentImage : Page
{
    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpPostedFile file = this.Request.Files[0];
        file.SaveAs(Request.PhysicalApplicationPath + @"\images\equipments\" + Session["EquipmentId"].ToString() + ".jpg");
    }
}