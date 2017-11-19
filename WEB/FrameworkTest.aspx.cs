using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;

public partial class FrameworkTest : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>
    /// Gets dictionary for fixed labels
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string label = "Item_Employee_Button_New";
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Employees", "EmployeesList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;

        UIButton bt = new UIButton()
        {
            Icon = "icon-plus-sign",
             Id = "01",
             Text = this.Dictionary["Item_Employee"],
             Action ="success"
        };

        TextBox tx = new TextBox()
        {
            Label = this.Dictionary["Item_Document"],
            Name = "TxtDocumento",
            Id = "TxtDocumento",
            Placeholder = this.Dictionary["Item_Document"],
            Required = true
        };

        this.lt.Text = bt.Render;
        this.lt.Text += tx.Html;
    }
}