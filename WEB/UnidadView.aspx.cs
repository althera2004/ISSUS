using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Web.UI;

public partial class UnidadView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    private int unidadId;
    private Unidad unidad;
    private FormFooter formFooter;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    private TabBar tabBar = new TabBar { Id = "UnidadTabBar" };

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
                Value = this.unidad.Description,
                ColumnSpan = 11,
                Placeholder = this.dictionary["Item_Unidades"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Unidad),
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

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>User logged in session</summary>
    private ApplicationUser user;

    public string Unidades
    {
        get
        {
            var res = new StringBuilder();
            bool first = true;
            foreach (var unidad in Unidad.GetActive(((Company)Session["Company"]).Id))
            {
                if (unidad.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(unidad.JsonKeyValue);
                }
            }

            return res.ToString();
        }
    }

    public int UnidadId
    {
        get
        {
            return this.unidadId;
        }
    }

    public Unidad UnidadItem
    {
        get
        {
            return this.unidad;
        }
    }

    public Company Company
    {
        get
        {
            return this.company;
        }
    }

    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
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
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
            }
            else if (!int.TryParse(this.Request.QueryString["id"], out test))
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
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.user = Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.unidadId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        string label = "Item_Unidad";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Unidades", "UnidadList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = "Item_Unidad_Title";


        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (this.unidadId != -1)
        {
            this.unidad = Unidad.GetById(this.unidadId, this.company.Id);
            if (this.unidad.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.unidad = Unidad.Empty;
            }
            this.formFooter.ModifiedBy = this.UnidadItem.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.UnidadItem.ModifiedOn;

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Unidad"], this.unidad.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.unidad = Unidad.Empty;
        }

        this.tabBar.AddTab(new Tab { Id = "home", Available = true, Active = true, Selected = true, Label = this.dictionary["Item_Unidad_Tab_Principal"] });
        //// this.tabBar.AddTab(new Tab() { Id = "trazas", Label = this.dictionary["Item_Provider_Tab_Traces"], Active = this.ProviderId > 0, Available = this.user.HasTraceGrant() });
    }    
}