using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BackUp : System.Web.UI.Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>User logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private FormFooter formFooter;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string CompanyId
    {
        get
        {
            return this.company.Id.ToString();
        }
    }

    public string Description
    {
        get
        {
            if (!string.IsNullOrEmpty(this.user.Description))
            {
                return this.user.Description;
            }

            return this.user.UserName;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    private void Go()
    {
        this.user = Session["User"] as ApplicationUser;
        this.company = Session["company"] as Company;
        this.user = new ApplicationUser(this.user.Id);
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AddBreadCrumbInvariant(this.user.UserName);
        this.master.Titulo = "Item_Backup";

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new SbrinnaCoreFramework.UI.UIButton { Id = "BtnSave", Text = this.dictionary["Common_Accept"], Icon = "icon-ok", Action = "success" });
        this.formFooter.AddButton(new SbrinnaCoreFramework.UI.UIButton { Id = "BtnCancel", Text = this.dictionary["Common_Cancel"], Icon = "icon-undo" });
        this.LtExplain.Text = this.dictionary["Item_Backup_Explain"];
    }
}