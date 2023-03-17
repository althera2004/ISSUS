using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class ProvidersView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    private int providerId;
    private Provider provider;

    private TabBar tabBar = new TabBar() { Id = "ProviderTabBar" };

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public bool IsActual
    {
        get
        {
            return false;
        }
    }

    public Employee Employee
    {
        get
        {
            return Employee.EmptySimple;
        }
    }

    public string TxtName
    {
        get
        {
            return new FormText
            {
                Name = "TxtName",
                Value = this.provider.Description,
                ColumnSpan = 11,
                Placeholder = this.Dictionary["Item_Provider"],
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Provider),
                MaximumLength = 100
            }.Render;
        }
    }

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    /// <summary>Company of session</summary>
    public Company Company { get; private set; }

    /// <summary>Dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>User logged in session</summary>
    private ApplicationUser user;

    public string Providers
    {
        get
        {
            StringBuilder res = new StringBuilder();
            bool first = true;
            foreach (Provider provider in Provider.ByCompany(((Company)Session["Company"]).Id))
            {

                if (provider.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }
                    res.Append(provider.JsonKeyValue);
                }
            }

            return res.ToString();
        }
    }

    public int ProviderId
    {
        get
        {
            return this.providerId;
        }
    }

    public Provider ProviderItem
    {
        get
        {
            return this.provider;
        }
    }

    public new ApplicationUser User
    {
        get
        {
            return this.user;
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
             this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                 this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
            }
            else if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
            }
            else
            {
                this.Go();
            }
        }

        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.Company = (Company)Session["company"];
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.user = Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.providerId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        string label = "Item_Provider_Detail";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Providers", "ProvidersList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;


        this.master.formFooter = new FormFooter();
        this.master.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.master.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        if (this.providerId != -1)
        {
            this.provider = Provider.ById(this.providerId, this.Company.Id);
            if (this.provider.CompanyId != this.Company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.provider = Provider.Empty;
            }

            this.master.ModifiedBy = this.ProviderItem.ModifiedBy.Description;
            this.master.ModifiedOn = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.ProviderItem.ModifiedOn);

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_Provider"], this.provider.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;            
        }
        else
        {
            this.provider = Provider.Empty;
            this.master.ModifiedBy = Dictionary["Common_New"];
            this.master.ModifiedOn = "-";
        }

        this.tabBar.AddTab(new Tab { Id = "home", Available = true, Active = true, Selected = true, Label = this.Dictionary["Item_Provider_Tab_Principal"] });
    }
}