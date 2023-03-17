using System;
using System.IO;
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
        var companyId = this.Request.Params["companyId"];
        var equipmentId = this.Request.Params["equipmentId"];
        //file.SaveAs(Request.PhysicalApplicationPath + @"\images\equipments\" + Session["EquipmentId"].ToString() + ".jpg");

        var folderPath = Request.PhysicalApplicationPath + @"\DOCS\" + companyId + "\\Equipments";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        file.SaveAs(Request.PhysicalApplicationPath + @"\DOCS\" + companyId + "\\Equipments\\" + equipmentId + ".jpg");
    }
}