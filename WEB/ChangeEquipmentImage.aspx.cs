using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangeEquipmentImage : Page
{
    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpPostedFile file = this.Request.Files[0];
        file.SaveAs(Request.PhysicalApplicationPath + @"\images\equipments\" + Session["EquipmentId"].ToString() + ".jpg");
    }
}