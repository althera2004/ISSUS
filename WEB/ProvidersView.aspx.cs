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

public partial class ProvidersView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    private int providerId;
    private Provider provider;
    private FormFooter formFooter;

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
            return new FormText()
            {
                Name = "TxtName",
                Value = this.provider.Description,
                ColumnSpan = 11,
                Placeholder = this.dictionary["Item_Provider"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
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

    /// <summary>
    /// User logged in session
    /// </summary>
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

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
             this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                 this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
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


        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (this.providerId != -1)
        {
            this.provider = Provider.ById(this.providerId, this.company.Id);
            if (this.provider.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.provider = Provider.Empty;
            }
            this.formFooter.ModifiedBy = this.ProviderItem.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.ProviderItem.ModifiedOn;

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Provider"], this.provider.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;

            this.RenderTableDefinitions();
            this.RenderTableCost();
            this.RenderTableActions();            
        }
        else
        {
            this.provider = Provider.Empty;
            //this.TableDefinitions.Text = string.Empty;
            this.TableCosts.Text = string.Empty;
            this.TableActions.Text = string.Empty;
        }

        this.tabBar.AddTab(new Tab { Id = "home", Available = true, Active = true, Selected = true, Label = this.dictionary["Item_Provider_Tab_Principal"] });
        //// this.tabBar.AddTab(new Tab() { Id = "trazas", Label = this.dictionary["Item_Provider_Tab_Traces"], Active = this.ProviderId > 0, Available = this.user.HasTraceGrant() });
    }

    public void RenderTableCost()
    {
        var tableCosts = new StringBuilder();
        var costs = ProviderCost.GetByProvider(this.provider);
        decimal total = 0;
        foreach (var cost in costs)
        {
            tableCosts.Append(cost.Row(this.dictionary, this.user.Grants));
            if (cost.Calibration != null)
            {
                total += cost.Calibration.Cost.HasValue ? cost.Calibration.Cost.Value : 0;
            }
            else if (cost.Maintenance != null)
            {
                total += cost.Maintenance.Cost.HasValue ? cost.Maintenance.Cost.Value : 0;
            }
            else if (cost.Verification != null)
            {
                total += cost.Verification.Cost.HasValue ? cost.Verification.Cost.Value : 0;
            }
            else
            {
                total += cost.Repair.Cost.HasValue ? cost.Repair.Cost.Value : 0;
            }
        }

        tableCosts.Append("<tr><td colspan=\"5\" align=\"right\">").Append(this.dictionary["Common_Total"]).Append("</td>");
        tableCosts.Append("<td align=\"right\"><strong>").Append(string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:#,##0.00}", total)).Append("</td></tr>");

        this.TableCosts.Text = tableCosts.ToString();
    }

    public void RenderTableActions()
    {
        var tableActions = new StringBuilder();
        var actions = ProviderIncidentActions.ByProvider(this.provider);
        foreach (var action in actions)
        {
            tableActions.Append(action.Row(this.dictionary, this.user.Grants));
        }

        this.TableActions.Text = tableActions.ToString();
    }

    public void RenderTableDefinitions()
    {
        /*StringBuilder tableDefinitions = new StringBuilder();
        ReadOnlyCollection<ProviderDefinition> definitions = ProviderDefinition.GetByProvider(this.provider);
        foreach (ProviderDefinition definition in definitions)
        {
            tableDefinitions.Append(definition.Row(this.dictionary, this.user.Grants));
        }

        this.TableDefinitions.Text = tableDefinitions.ToString();*/
    }
}